/**********************************************
This Program was written by Dario Romero A.
It is licensed under the Apache License v.2.0.
***********************************************/
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
    public class Documento:IEnumerable<Parrafo>,IEscritor
    {
        Dictionary<int, Parrafo> m_Parrafos = new Dictionary<int, Parrafo>();
        List<IObservadorDocumento> m_Observadores = new List<IObservadorDocumento>();
        public object ObjetoLock { get { return m_Parrafos; } }

        #region Miembros de IEnumerable<Parrafo>

        public IEnumerator<Parrafo> GetEnumerator()
        {
            lock (ObjetoLock)
            {
                Parrafo actual = ObtenerPrimerParrafo();
                while (actual != null)
                {
                    yield return actual;
                    actual = actual.Siguiente;
                }
            }
        }

        #endregion

        #region Miembros de IEnumerable

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
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
            if (evitarNotificaciones) return;
            foreach (IObservadorDocumento obs in m_Observadores)
            {
                obs.ParrafoAgregado(p);
            }
        }
        private void EnEliminacionParrafo(Parrafo p)
        {
            if (evitarNotificaciones) return;
            foreach (IObservadorDocumento obs in m_Observadores)
            {
                obs.ParrafoEliminado(p);
            }
        }
        private void EnCambioParrafo(Parrafo p)
        {
            if (evitarNotificaciones) return;
            foreach (IObservadorDocumento obs in m_Observadores)
            {
                obs.ParrafoCambiado(p);
            }
        }
        private void EnParrafosCambiados(Parrafo inicio,Parrafo fin)
        {
            if (evitarNotificaciones) return;
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
            elim.NotificarEliminacion();
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
                    parrafo.AgregarTextoDe(parrafoSiguiente);
                    parrafo.ConectarDespues(parrafoSiguiente.Siguiente);
                    EliminarParrafo(parrafoSiguiente.ID);
                    EnParrafosCambiados(parrafo, parrafoSiguiente.Siguiente);
                    //Las posiciones no deberían haber cambiado
                    //NotificarCambioOrden();                    
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
                /*if (posicionInsercion == 0&&parrafoSeleccionado.Longitud!=0)
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
                else */if (posicionInsercion == parrafoSeleccionado.Longitud)
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
                    inicio.Escribir(esc, posicionInicio, inicio.Longitud - posicionInicio);
                    Parrafo p = inicio.Siguiente;
                    while (p != null)
                    {
                        if (p == fin) break;
                        p.Escribir(esc, 0, p.Longitud);
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
                    parrafoSeleccionado.BorrarRangoCaracteres(Math.Min(posicionInicio,posicionFinRango), Math.Abs(posicionFinRango-posicionInicio));//En caso que la posición inicio sea mayor a fin, el valor absoluto devolverá siempre el valor correcto.
                    EnCambioParrafo(parrafoSeleccionado);
                    return parrafoSeleccionado;
                }
                else
                {
                    if (!parrafoSeleccionado.EsSiguiente(parrafoFinRango))
                    {
                        Parrafo aux = parrafoFinRango;
                        int posaux = posicionFinRango;
                        parrafoFinRango = parrafoSeleccionado;
                        posicionFinRango = posicionInicio;
                        parrafoSeleccionado = aux;
                        posicionInicio = posaux;
                    }
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
                        p.CambiarFormato(formato, 0, p.Longitud);
                        p = p.Siguiente;
                    }
                    parrafoInicio.CambiarFormato(formato, posicionInicio, parrafoInicio.Longitud - posicionInicio);
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
                        f = f.ObtenerInterseccion(p.ObtenerFormatoComun(inicio, p.Longitud - inicio));
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

        public static Documento ObtenerSubdocumento(Parrafo parrafoInicial, int posicionInicial, Parrafo parrafoFinal, int posicionFinal)
        {
            Documento doc = new Documento();
            doc.EliminarParrafo(doc.ObtenerPrimerParrafo().ID);
            if (parrafoInicial == parrafoFinal)
            {
                Parrafo p = parrafoInicial.ObtenerSubParrafo(doc, posicionInicial, posicionFinal - posicionInicial);
                p.CambiarID(doc.CrearNuevoID(1));
                doc.AgregarParrafo(p);
            }
            else
            {
                Parrafo actual = parrafoInicial;
                Parrafo p;
                p = actual.ObtenerSubParrafo(doc, posicionInicial, actual.Longitud - posicionInicial);
                p.CambiarID(doc.CrearNuevoID(1));
                doc.AgregarParrafo(p);
                Parrafo anterior = p;
                actual=actual.Siguiente;
                while (actual != null)
                {
                    if (p == parrafoFinal)
                        break;
                    p=actual.ObtenerSubParrafo(doc,0,actual.Longitud);
                    p.CambiarID(doc.CrearNuevoID(anterior.ID));
                    doc.AgregarParrafo(p);
                    anterior.ConectarDespues(p);
                    anterior = p;
                    actual = actual.Siguiente;
                }
                p=parrafoFinal.ObtenerSubParrafo(doc, 0, posicionFinal);
                p.CambiarID(doc.CrearNuevoID(anterior.ID));
                doc.AgregarParrafo(p);
                anterior.ConectarDespues(p);
            }
            return doc;
        }
        internal void AgregarParrafo(Parrafo parrafoBase, int posicionInsercion, Parrafo parrafo, bool romperPrimerParrafo,bool romperUltimoParrafo)
        {
            Parrafo nuevo=parrafoBase.DividirParrafo(CrearNuevoID(parrafoBase.ID), posicionInsercion);

            Parrafo clon = parrafo.Clonar(this);
            Parrafo siguienteActual = parrafo.Siguiente;
            parrafoBase.ConectarDespues(clon);
            clon.ConectarDespues(siguienteActual);
            if (!romperPrimerParrafo)
            {
                FusionarSiguiente(parrafoBase);
            }
            if (!romperUltimoParrafo)
            {
                FusionarSiguiente(clon);
            }
            EnParrafosCambiados(parrafoBase, nuevo);
        }

        bool evitarNotificaciones = false;
        private void SuprimirNotificaciones()
        {
            evitarNotificaciones = true;
        }
        private void ReanudarNotificaciones()
        {
            evitarNotificaciones = false;
        }

        internal void NotificarCambios(Parrafo pinicio, Parrafo pfin)
        {
            EnParrafosCambiados(pinicio, pfin);
        }

        internal void AsegurarOrden()
        {
            lock (this)
            {
                if (_parrafosDesordenados)
                {
                    Parrafo p = ObtenerPrimerParrafo();
                    int contador = 1;
                    while (p != null)
                    {
                        p.IndicarOrden(contador);
                        p = p.Siguiente;
                        contador +=1000;
                        //sig.Posicion = contador;
                        //sig = sig._Siguiente;
                        //int incremento = Math.Max(1, p != null && p.Posicion > contador ? (p.Posicion - contador) / 2 : 10);
                        //contador += incremento;
                        //if (sig != null && sig.Posicion > contador)
                        //{
                        //    break;
                        //}
                    }
                    _parrafosDesordenados = false;
                }
            }
        }
        bool _parrafosDesordenados = true;
        internal void NotificarCambioOrden()
        {
            lock (this)
            {
                _parrafosDesordenados = true;
            }
        }

        internal Parrafo InsertarParrafos(IEnumerable<string> listaParrafos,Parrafo parrafoInicial,int posicion)
        {
            Parrafo inicio=parrafoInicial;
            SuprimirNotificaciones();
            foreach (string c in listaParrafos)
            {
                parrafoInicial.InsertarCadena(posicion, c);
                Parrafo par2=InsertarParrafo(parrafoInicial, posicion + c.Length);
                parrafoInicial = par2;
                posicion = 0;
            }
            ReanudarNotificaciones();
            Parrafo fin = parrafoInicial;
            NotificarCambios(inicio, fin);
            return fin;
        }

        #region Miembros de IEscritor
        Parrafo parrafoActual;
        int posicionInsercion;
        Parrafo parrafoIns;
        void IEscritor.IniciarDocumento()
        {
            parrafoIns = ObtenerUltimoParrafo();
            posicionInsercion = parrafoIns.Longitud;
        }

        void IEscritor.IniciarParrafo(FormatoParrafo formato)
        {
            parrafoActual = InsertarParrafo(parrafoIns,posicionInsercion);
            parrafoActual.Formato = formato.Clonar();
            posicionInsercion = 0;
        }

        void IEscritor.EscribirTexto(string texto, Formato formato)
        {
            parrafoActual.CambiarFormato(formato, posicionInsercion, 0);
            parrafoActual.AgregarCadena(texto);
            posicionInsercion = parrafoActual.Longitud;
        }

        void IEscritor.TerminarParrafo()
        {
            posicionInsercion = parrafoActual.Longitud;
        }

        void IEscritor.TerminarDocumento()
        {
            
        }

        public byte[] ObtenerBytes()
        {
            return null;
        }

        #endregion
    }
}
