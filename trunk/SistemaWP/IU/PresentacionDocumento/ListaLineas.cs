using System;
using System.Collections.Generic;
using System.Text;
using SWPEditor.Dominio;

namespace SWPEditor.IU.PresentacionDocumento
{
     public class ListaLineas:IEnumerable<Linea>
    {
        List<Linea> _lineas;
        Parrafo parrafoActual;
        int numcaracterActual;
        bool completo;
        bool _enIteracionLineas;
        ListaPaginas _listaPaginas;
        Documento _documento;
        public ListaLineas(Documento documento,ListaPaginas listaPaginas)
        {
            _documento = documento;
            _lineas = new List<Linea>();
            parrafoActual = documento.ObtenerPrimerParrafo();
            numcaracterActual = 0;
            _listaPaginas = listaPaginas;
        }
        public void Recalcular(Parrafo inicio, Parrafo fin)
        {
            if (!_enIteracionLineas)
            {
                int a = BuscarInt(0, inicio);
                if (a != -1)
                {
                    Recalcular(a, inicio);
                }
                else
                {
                    if (inicio.Anterior != null)
                    {
                        a = BuscarInt(0,inicio.Anterior);
                        if (a != -1)
                        {
                            Recalcular(a, inicio.Anterior);
                        }
                    }
                    else
                    {
                        Recalcular(0, _documento.ObtenerPrimerParrafo());
                    }
                }
            }
            else
            {
                throw new Exception("No se puede recalcular mientras se itera por las lineas");
            }
            
        }
        private void Recalcular(int indiceLinea, Parrafo actual)
        {
            if (!_enIteracionLineas)
            {
                if (indiceLinea < _lineas.Count)
                {
                    parrafoActual = actual;
                    numcaracterActual = 0;
                    completo = false;
                    _lineas.RemoveRange(indiceLinea, _lineas.Count - indiceLinea);
                }
            }
            else
            {
                throw new Exception("No se puede recalcular mientras se itera por las lineas");
            }
        }
        public IEnumerable<Linea> ObtenerDesde(int indice)
        {
            try
            {
                _enIteracionLineas = true;
                AsegurarHasta(indice);

                while (true)
                {
                    if (completo && indice >= _lineas.Count)
                        yield break;
                    yield return _lineas[indice];
                    indice++;
                    AsegurarHasta(indice);
                }
            }
            finally
            {
                _enIteracionLineas = false;
            }
        }
        private void CalcularSiguiente()
        {
            Linea l = Linea.ObtenerSiguienteLinea(
                parrafoActual, numcaracterActual, 
                _listaPaginas.ObtenerAnchoLinea(_lineas.Count), 
                true, 
                true);
            numcaracterActual += l.Cantidad;
            if (l.EsUltimaLineaParrafo)
            {
                parrafoActual = parrafoActual.Siguiente;
                numcaracterActual = 0;
                if (parrafoActual == null) completo = true;                
            }
            _lineas.Add(l); 
        }
        private void AsegurarHasta(int indice)
        {
            while (!completo&&_lineas.Count <= indice)
            {
                CalcularSiguiente();
            }
        }
        public Linea Obtener(int indice)
        {
            AsegurarHasta(indice);
            return _lineas[indice];
        }

        private int BuscarInt(int lineainicio, Parrafo p)
        {
            if (_lineas[lineainicio].Parrafo.EsSiguiente(p))
            {
                for (int i = lineainicio + 1; i < _lineas.Count; i++)
                {
                    
                    if (_lineas[i].Parrafo == p)
                    {
                        return i;
                    }
                    if (!_lineas[i].Parrafo.EsSiguiente(p))
                    {
                        break;
                    }
                }
            }
            else
            {
                for (int i = lineainicio; i >= 0; i--)
                {
                    Linea act = _lineas[i];
                    if (act.Parrafo == p && act.Inicio == 0)
                    {
                        return i;
                    }
                    if (!p.EsSiguiente(_lineas[i].Parrafo))
                    {
                        break;
                    }
                }
            }
            return -1;
        }
        public int BuscarInicialDeParrafo(int lineainicio, Parrafo p)
        {
            AsegurarHasta(lineainicio);
            int res = BuscarInt(lineainicio, p);
            if (res==-1) throw new Exception("Linea no encontrada");
            return res;
        }

        #region Miembros de IEnumerable<Linea>

        public IEnumerator<Linea> GetEnumerator()
        {
            foreach (Linea l in _lineas)
            {
                yield return l;
            }
        }

        #endregion

        #region Miembros de IEnumerable

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        internal void Limpiar()
        {
            _lineas.Clear();
        }

        internal void RemoverDesde(int lineainicio)
        {
            _lineas.RemoveRange(lineainicio, _lineas.Count - lineainicio);
        }

        internal bool EsUltimaLinea(int indiceLinea)
        {
            if (completo)
                return indiceLinea == _lineas.Count - 1;
            else
            {
                AsegurarHasta(indiceLinea);
                if (completo && indiceLinea == _lineas.Count - 1)
                    return true;
                return false;
            }
        }


        
    }
}

