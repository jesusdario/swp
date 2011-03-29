using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SistemaWP.Dominio;

namespace SistemaWP.IU.PresentacionDocumento
{
     public class ListaLineas:IEnumerable<Linea>
    {
        List<Linea> _lineas;
        Parrafo parrafoActual;
        int numcaracterActual;
        bool completo;
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
            
        }
        internal void Recalcular(int indiceLinea,Parrafo actual)
        {
            if (indiceLinea < _lineas.Count)
            {
                parrafoActual = actual;
                numcaracterActual = 0;
                completo = false;
                _lineas.RemoveRange(indiceLinea, _lineas.Count - indiceLinea);
            }
        }
        public IEnumerable<Linea> ObtenerDesde(int indice)
        {
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
       
       
        public int BuscarInicialDeParrafo(int lineainicio, Parrafo p)
        {
            AsegurarHasta(lineainicio);
            if (_lineas[lineainicio].Parrafo.EsSiguiente(p))
            {
                for (int i = lineainicio + 1; i < _lineas.Count; i++)
                {
                    if (_lineas[i].Parrafo == p)
                    {
                        return i;
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
                }
            }
            throw new Exception("Linea no encontrada");
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

