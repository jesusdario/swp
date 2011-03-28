using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using SistemaWP.Dominio.TextoFormato;

namespace SistemaWP.Dominio
{
    public class Documento
    {
      
        Dictionary<int, Parrafo> m_Parrafos = new Dictionary<int, Parrafo>();
        int indentificadorActual;
        
        
        public Documento()
        {
            indentificadorActual = 1;
            m_Parrafos.Add(1, new Parrafo(this,1,null,null));
            //formatoBase.Negrilla = false;
            //formatoBase.Cursiva = false;
            //formatoBase.Subrayado = false;
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
            if (parrafoInicial == parrafoFinal)
            {
                accion(parrafoInicial);
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
            }
        }
        public void FusionarSiguiente(Parrafo parrafo)
        {
            Parrafo parrafoSiguiente = parrafo.Siguiente;
            if (parrafoSiguiente != null)
            {
                parrafo.FusionarCon(parrafoSiguiente);
                m_Parrafos.Remove(parrafoSiguiente.ID);
            }
            
        }

        internal Parrafo InsertarParrafo(Parrafo parrafoSeleccionado, int posicionInsercion)
        {
            if (posicionInsercion == 0)
            {
                int idnuevo = CrearNuevoID(parrafoSeleccionado.ID);
                Parrafo p = new Parrafo(this, idnuevo,
                    parrafoSeleccionado.Anterior,
                    parrafoSeleccionado,parrafoSeleccionado);
                parrafoSeleccionado.InsertarAnterior(p);   
                
                m_Parrafos.Add(idnuevo, p);
                return parrafoSeleccionado;
            }
            else if (posicionInsercion == parrafoSeleccionado.ObtenerLongitud())
            {
                int idnuevo = CrearNuevoID(parrafoSeleccionado.ID+1);
                Parrafo p = new Parrafo(this, idnuevo, parrafoSeleccionado, parrafoSeleccionado.Siguiente, parrafoSeleccionado);
                parrafoSeleccionado.InsertarSiguiente(p);
                m_Parrafos.Add(idnuevo, p);
                return p;
            }
            else
            {
                int idnuevo = CrearNuevoID(parrafoSeleccionado.ID + 1);
                Parrafo parrafoNuevo = parrafoSeleccionado.DividirParrafo(idnuevo, posicionInsercion);
                m_Parrafos.Add(idnuevo, parrafoNuevo);
                return parrafoNuevo;
            }
            
        }

        private int CrearNuevoID(int idbase)
        {
            return ++indentificadorActual;
        }
  
        public override string ToString()
        {
            Parrafo p = m_Parrafos.Values.FirstOrDefault();
            while (p != null)
            {
                if (p.Anterior != null)
                {
                    p = p.Anterior;
                }
                else
                    break;
            }
            StringBuilder st = new StringBuilder();
            while (p != null)
            {
                st.AppendLine(p.ToString());
                p = p.Siguiente;
            }
            return st.ToString();
        }
        public Parrafo ObtenerPrimerParrafo() {
            Parrafo p = m_Parrafos.Values.FirstOrDefault();
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
        protected void AsegurarParrafo(Parrafo parrafo)
        {
            if (m_Parrafos.Count == 0)
            {
                m_Parrafos.Add(parrafo.ID, parrafo);
            }
        }
        public Parrafo BorrarRango(
            Parrafo parrafoSeleccionado, int posicionInicio, 
            Parrafo parrafoFinRango,int posicionFinRango)
        {
            if (parrafoSeleccionado == parrafoFinRango)
            {
                parrafoSeleccionado.BorrarRangoCaracteres(posicionInicio, posicionFinRango);
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
                        m_Parrafos.Remove(s.ID);
                    }
                    s = s.Siguiente;
                }
                parrafoFinRango.BorrarHastaInicio(posicionFinRango);
                parrafoSeleccionado.ConectarDespues(parrafoFinRango);
                FusionarSiguiente(parrafoFinRango);
                AsegurarParrafo(parrafoSeleccionado);
                return parrafoSeleccionado;
            }
        }

        internal Parrafo ObtenerUltimoParrafo()
        {
            Parrafo p=m_Parrafos.Values.LastOrDefault();
            do
            {
                if (p.Siguiente != null)  
                    p = p.Siguiente;
            } while (p.Siguiente != null);
            return p;            
        }

        internal void CambiarFormato(Formato formato, 
            Parrafo parrafoInicio, int posicionInicio, 
            Parrafo parrafoFin, int posicionFin)
        {
            if (parrafoInicio == parrafoFin)
            {
                parrafoInicio.CambiarFormato(formato, posicionInicio, posicionFin - posicionInicio);
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
            }
        }
       
        internal Formato ObtenerFormatoComun(Parrafo parrafoInicio, int posicionInicio,
            Parrafo parrafoFin, int posicionFin)
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
                    f=f.ObtenerInterseccion(p.ObtenerFormatoComun(inicio, p.ObtenerLongitud() - inicio));
                    p = p.Siguiente;
                    inicio = 0;
                }
                f = f.ObtenerInterseccion(parrafoFin.ObtenerFormatoComun(0, posicionFin));
                return f;
            }
        }
    }
}
