using System;
using System.Collections.Generic;

using System.Text;
using System.Diagnostics;
using SWPEditor.Dominio.TextoFormato;
using System.Collections;

namespace SWPEditor.Dominio
{
    interface IObservadorDocumento
    {
        void ParrafoAgregado(Parrafo p);
        void ParrafoCambiado(Parrafo p);
        void ParrafoEliminado(Parrafo p);
        void ParrafosCambiados(Parrafo parrafoInicio, Parrafo parrafoFin);
    }
    public class Documento
    {
        Dictionary<int, Parrafo> m_Parrafos = new Dictionary<int, Parrafo>();
        List<IObservadorDocumento> m_Observadores = new List<IObservadorDocumento>();
        public object ObjetoLock { get { return m_Parrafos; } }
        internal void AgregarObservador(IObservadorDocumento observador)
        {
            m_Observadores.Add(observador);
        }
        internal void QuitarObservador(IObservadorDocumento observador)
        {
            m_Observadores.Remove(observador);
        }
        private void EnAdicionParrafo(Parrafo p)
        {
            foreach (IObservadorDocumento obs in m_Observadores)
            {
                obs.ParrafoAgregado(p);
            }
        }
        private void EnEliminacionParrafo(Parrafo p)
        {
            foreach (IObservadorDocumento obs in m_Observadores)
            {
                obs.ParrafoEliminado(p);
            }
        }
        private void EnCambioParrafo(Parrafo p)
        {
            foreach (IObservadorDocumento obs in m_Observadores)
            {
                obs.ParrafoCambiado(p);
            }
        }
        private void EnParrafosCambiados(Parrafo inicio,Parrafo fin)
        {
            foreach (IObservadorDocumento obs in m_Observadores)
            {
                obs.ParrafosCambiados(inicio,fin);
            }
        }
        int indentificadorActual;
        private void AgregarParrafo(Parrafo p)
        {
            m_Parrafos.Add(p.ID, p);
        }
        private void EliminarParrafo(int id)
        {
            Parrafo elim = m_Parrafos[id];
            m_Parrafos.Remove(id);
        }
        
        public Documento()
        {
            indentificadorActual = 1;
            AgregarParrafo(new Parrafo(this,1,null,null));
        }

        public Parrafo ObtenerParrafo(int idparrafo)
        {
            try
            {
                return m_Parrafos[idparrafo];
            }
            catch
            {
                int a = 3425;
                throw;
            }
        }
        public void AplicarOperacionParrafos(Parrafo parrafoInicial, Parrafo parrafoFinal,
            Action<Parrafo> accion)
        {
            lock (m_Parrafos)
            {
                if (parrafoInicial == parrafoFinal)
                {
                    accion(parrafoInicial);
                    EnCambioParrafo(parrafoInicial);
                }
                else
                {
                    Debug.Assert(parrafoInicial.EsSiguiente(parrafoFinal));
                    Parrafo p = parrafoInicial;
                    while (p != null && p != parrafoFinal)
                    {
                        accion(p);
                        p = p.Siguiente;
                    }
                    accion(parrafoFinal);
                    EnParrafosCambiados(parrafoInicial,parrafoInicial);
                }
            }
        }
        internal void FusionarSiguiente(Parrafo parrafo)
        {
            lock (m_Parrafos)
            {
                Parrafo parrafoSiguiente = parrafo.Siguiente;
                if (parrafoSiguiente != null)
                {
                    parrafo.FusionarCon(parrafoSiguiente);
                    EliminarParrafo(parrafoSiguiente.ID);
                    EnParrafosCambiados(parrafo, parrafoSiguiente);
                }
                else
                {
                    EnCambioParrafo(parrafo);
                }
            }
            
        }

        internal Parrafo InsertarParrafo(Parrafo parrafoSeleccionado, int posicionInsercion)
        {
            lock (m_Parrafos)
            {
                if (posicionInsercion == 0)
                {
                    int idnuevo = CrearNuevoID(parrafoSeleccionado.ID);
                    Parrafo p = new Parrafo(this,idnuevo,
                        parrafoSeleccionado.Anterior,
                        parrafoSeleccionado, parrafoSeleccionado);
                    parrafoSeleccionado.InsertarAnterior(p);
                    AgregarParrafo(p);
                    EnParrafosCambiados(p, parrafoSeleccionado);
                    return parrafoSeleccionado;
                }
                else if (posicionInsercion == parrafoSeleccionado.ObtenerLongitud())
                {
                    int idnuevo = CrearNuevoID(parrafoSeleccionado.ID + 1);
                    Parrafo p = new Parrafo(this, idnuevo, parrafoSeleccionado, parrafoSeleccionado.Siguiente, parrafoSeleccionado);
                    parrafoSeleccionado.InsertarSiguiente(p);
                    AgregarParrafo(p);
                    EnParrafosCambiados(parrafoSeleccionado, p);
                    return p;
                }
                else
                {
                    int idnuevo = CrearNuevoID(parrafoSeleccionado.ID + 1);
                    Parrafo parrafoNuevo = parrafoSeleccionado.DividirParrafo(idnuevo, posicionInsercion);
                    AgregarParrafo(parrafoNuevo);
                    EnParrafosCambiados(parrafoSeleccionado, parrafoNuevo);
                    return parrafoNuevo;
                }
            }
            
        }

        private int CrearNuevoID(int idbase)
        {
            return ++indentificadorActual;
        }
        public string ObtenerTexto()
        {
            return ObtenerTexto(new Texto.EscritorTexto(), ObtenerPrimerParrafo(), 0, null, 0);
        }
        public string ObtenerHTML()
        {
            return ObtenerTexto(new Html.EscritorHTML(), ObtenerPrimerParrafo(), 0, null, 0);
        }
        public string ObtenerTexto(IEscritor escritor, Parrafo inicio,int posicionInicio, Parrafo fin,int posicionFin)
        {
            lock (m_Parrafos)
            {
                if (inicio == null)
                {
                    inicio = ObtenerPrimerParrafo();
                    posicionInicio = 0;
                }

                IEscritor esc = escritor;
                esc.IniciarDocumento();
                if (inicio == fin)
                {
                    if (inicio == null)
                    {
                        return ObtenerTexto(escritor, ObtenerPrimerParrafo(), 0, null, 0);
                    }
                    inicio.Escribir(esc, posicionInicio, posicionFin - posicionInicio);
                }
                else
                {
                    inicio.Escribir(esc, posicionInicio, inicio.ObtenerLongitud() - posicionInicio);
                    Parrafo p = inicio.Siguiente;
                    while (p != null)
                    {
                        if (p == fin) break;
                        p.Escribir(esc, 0, p.ObtenerLongitud());
                        p = p.Siguiente;
                    }
                    if (fin != null)
                    {
                        fin.Escribir(esc, 0, posicionFin);
                    }
                }
                esc.TerminarDocumento();
                return esc.ObtenerTexto();
            }
        }
        public override string ToString()
        {
            return ObtenerTexto();
            
        }
        public Parrafo ObtenerPrimerParrafo() {
            lock (m_Parrafos)
            {
                IEnumerator e=m_Parrafos.Values.GetEnumerator();
                
                Parrafo p = (Parrafo)(e.MoveNext()?e.Current:null);
                while (p != null)
                {
                    if (p.Anterior != null)
                    {
                        p = p.Anterior;
                    }
                    else
                        break;
                }
                return p;
            }
        }
        
        public Parrafo BorrarRango(
            Parrafo parrafoSeleccionado, int posicionInicio, 
            Parrafo parrafoFinRango,int posicionFinRango)
        {
            lock (m_Parrafos)
            {
                if (parrafoSeleccionado == parrafoFinRango)
                {
                    parrafoSeleccionado.BorrarRangoCaracteres(posicionInicio, posicionFinRango);
                    EnCambioParrafo(parrafoSeleccionado);
                    return parrafoSeleccionado;
                }
                else
                {
                    Debug.Assert(parrafoSeleccionado.EsSiguiente(parrafoFinRango));
                    parrafoSeleccionado.BorrarHastaFin(posicionInicio);
                    Parrafo s = parrafoSeleccionado.Siguiente;
                    while (s != null)
                    {
                        if (s == parrafoFinRango)
                        {
                            break;
                        }
                        else
                        {
                            EliminarParrafo(s.ID);
                        }
                        s = s.Siguiente;
                    }
                    parrafoFinRango.BorrarHastaInicio(posicionFinRango);
                    parrafoSeleccionado.ConectarDespues(parrafoFinRango);
                    FusionarSiguiente(parrafoFinRango);
                    EnParrafosCambiados(parrafoSeleccionado,parrafoSeleccionado.Siguiente);
                    return parrafoSeleccionado;
                }
            }
        }

        internal Parrafo ObtenerUltimoParrafo()
        {
            lock (m_Parrafos)
            {
                Parrafo p = ObtenerPrimerParrafo();
                do
                {
                    if (p.Siguiente != null)
                        p = p.Siguiente;
                } while (p.Siguiente != null);
                return p;
            }
        }

        internal void CambiarFormato(Formato formato, 
            Parrafo parrafoInicio, int posicionInicio, 
            Parrafo parrafoFin, int posicionFin)
        {
            lock (m_Parrafos)
            {
                if (parrafoInicio == parrafoFin)
                {
                    parrafoInicio.CambiarFormato(formato, posicionInicio, posicionFin - posicionInicio);
                    EnCambioParrafo(parrafoInicio);
                }
                else
                {
                    Parrafo p = parrafoInicio.Siguiente;
                    while (p != parrafoFin)
                    {
                        p.CambiarFormato(formato, 0, p.ObtenerLongitud());
                        p = p.Siguiente;
                    }
                    parrafoInicio.CambiarFormato(formato, posicionInicio, parrafoInicio.ObtenerLongitud() - posicionInicio);
                    parrafoFin.CambiarFormato(formato, 0, posicionFin);
                    EnParrafosCambiados(parrafoInicio, parrafoFin);
                }
            }
        }
       
        internal Formato ObtenerFormatoComun(Parrafo parrafoInicio, int posicionInicio,
            Parrafo parrafoFin, int posicionFin)
        {
            lock (m_Parrafos)
            {
                if (parrafoInicio == parrafoFin)
                {
                    return parrafoInicio.ObtenerFormatoComun(posicionInicio, posicionFin - posicionInicio);
                }
                else
                {
                    Formato f = Formato.ObtenerPredefinido().Clonar();
                    Parrafo p = parrafoInicio;
                    int inicio = posicionInicio;
                    while (p != parrafoFin)
                    {
                        f = f.ObtenerInterseccion(p.ObtenerFormatoComun(inicio, p.ObtenerLongitud() - inicio));
                        p = p.Siguiente;
                        inicio = 0;
                    }
                    f = f.ObtenerInterseccion(parrafoFin.ObtenerFormatoComun(0, posicionFin));
                    return f;
                }
            }
        }

        internal void NotificarCambio(Parrafo parrafo)
        {
            EnCambioParrafo(parrafo);
        }
    }
}
