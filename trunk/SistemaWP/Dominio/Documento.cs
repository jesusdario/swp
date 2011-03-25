using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SistemaWP.Dominio
{
    public class Documento
    {
        /*PATRON SINGLETON (visibilidad global, una sola instancia de una clase)*/
        static Documento m_Instancia;
        public static Documento Instancia
        {
            get
            {
                if (m_Instancia == null)
                {
                    m_Instancia = new Documento();
                    
                }
                return m_Instancia;
            }
        }
        
        Dictionary<int, Parrafo> m_Parrafos = new Dictionary<int, Parrafo>();
        int indentificadorActual;
        public Documento()
        {
            indentificadorActual = 1;
            m_Parrafos.Add(1, new Parrafo(this,1,null,null));
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
                    parrafoSeleccionado);
                parrafoSeleccionado.InsertarAnterior(p);                
                m_Parrafos.Add(idnuevo, p);
                return parrafoSeleccionado;
            }
            else if (posicionInsercion == parrafoSeleccionado.ObtenerLongitud())
            {
                int idnuevo = CrearNuevoID(parrafoSeleccionado.ID+1);
                Parrafo p = new Parrafo(this, idnuevo,parrafoSeleccionado,parrafoSeleccionado.Siguiente);
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
                parrafoSeleccionado.BorrarRangoCaracteres(
                    posicionInicio, posicionFinRango);
                return parrafoSeleccionado;
            }
            else
            {
                if (parrafoSeleccionado.EsSiguiente(parrafoFinRango))
                {
                    Parrafo s = parrafoSeleccionado;
                    s.BorrarHastaFin(posicionInicio);

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
                    parrafoSeleccionado.ConectarDespues(parrafoFinRango);
                    parrafoFinRango.BorrarHastaInicio(posicionFinRango);
                    parrafoSeleccionado.FusionarCon(parrafoFinRango);
                    m_Parrafos.Remove(parrafoFinRango.ID);
                    AsegurarParrafo(parrafoSeleccionado);
                    return parrafoSeleccionado;
                }
                else
                {
                    Parrafo s = parrafoSeleccionado;
                    s.BorrarHastaInicio(posicionInicio);
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
                        s = s.Anterior;
                    }
                    parrafoSeleccionado.ConectarAntes(parrafoFinRango);
                    parrafoFinRango.BorrarHastaFin(posicionFinRango);
                    parrafoFinRango.FusionarCon(parrafoSeleccionado);
                    m_Parrafos.Remove(parrafoSeleccionado.ID);
                    AsegurarParrafo(parrafoFinRango);
                    return parrafoFinRango;
                }
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
    }
}
