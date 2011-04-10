/**********************************************
This Program was written by Dario Romero A.
It is licensed under the Apache License v.2.0.
***********************************************/
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
        public int Recalcular(Parrafo inicio, Parrafo fin)
        {
            if (!_enIteracionLineas)
            {
                if (_lineas.Count > 0)
                {
                    int a = BuscarInt(0, inicio);
                    if (a != -1)
                    {
                        Recalcular(a, inicio);
                        return a;
                    }
                    else
                    {
                        if (inicio.Anterior != null)
                        {
                            a = BuscarInt(0, inicio.Anterior);
                            if (a != -1)
                            {
                                Recalcular(a, inicio.Anterior);
                                return a;
                            }
                            else
                            {
                                return Math.Max(_lineas.Count - 1, 0);
                                //throw new Exception("Párrafo no encontrado");
                            }
                        }
                        else
                        {
                            Recalcular(0, _documento.ObtenerPrimerParrafo());
                            return 0;
                        }
                    }
                }
                else
                {
                    return 0;
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
        bool cursorLibre=true;
        private void LiberarCursor()
        {
            cursorLibre = true;
        }
        class CursorLineas : IDisposable, IEnumerable<Linea>
        {
            ListaLineas lista;
            int indicebase;
            public CursorLineas(ListaLineas l,int indiceinicio)
            {
                lista = l;
                indicebase = indiceinicio;
            }
            class Enumerador : IEnumerator<Linea>,IDisposable
            {
                int indice;
                Linea actual;
                CursorLineas cursor;
                public Enumerador(CursorLineas c)
                {
                    cursor = c;
                    indice = c.indicebase;
                    actual = null;
                }
                #region Miembros de IEnumerator<Linea>

                public Linea Current
                {
                    get {
                        return actual;
                    }
                }

                #endregion

                #region Miembros de IDisposable

                public void Dispose()
                {
                    cursor.Dispose();
                    cursor = null;
                }

                #endregion

                #region Miembros de IEnumerator

                object System.Collections.IEnumerator.Current
                {
                    get { return actual; }
                }

                public bool MoveNext()
                {
                    cursor.lista.AsegurarHasta(indice);
                    if (cursor.lista.completo&& indice >= cursor.lista._lineas.Count)
                    {
                        return false;
                    }
                    actual = cursor.lista._lineas[indice];
                    indice++;
                    return true;
                }

                public void Reset()
                {
                    indice = cursor.indicebase;
                    actual = null;
                }

                #endregion
            }
            #region Miembros de IEnumerable<Linea>

            public IEnumerator<Linea> GetEnumerator()
            {
                return new Enumerador(this);
            }

            public void Dispose()
            {
                if (lista != null)
                {
                    lista.LiberarCursor();
                    lista = null;
                }
            }
            #endregion

            #region Miembros de IEnumerable

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return new Enumerador(this);
            }

            #endregion
        }
        public IEnumerable<Linea> ObtenerDesde(int indice)
        {
            if (!cursorLibre) throw new Exception("Ya existe una iteración de líneas. Debe terminarse esa iteración");
            cursorLibre = false;
            return new CursorLineas(this, indice);            
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
        private int BusquedaBin(List<Linea> lista,Parrafo parrafo,int inicio, int fin)
        {
            if (inicio > fin)
            {
                return -1;
            }
            int centro = (inicio + fin) / 2;
            if (lista[centro].Parrafo == parrafo)
            {
                return centro;
            }
            else if (lista[centro].Parrafo.EsSiguiente(parrafo))
            {
                return BusquedaBin(lista, parrafo, centro + 1, fin);
            }
            else
            {
                return BusquedaBin(lista, parrafo, inicio, centro - 1);
            }
        }
        private int BuscarInt(int lineainicio, Parrafo parrafoBuscado)
        {
            if (_lineas.Count == 0) return -1;
            if (_lineas[_lineas.Count-1].Parrafo.EsSiguiente(parrafoBuscado))
            {
                return -1;//No se encuentra en líneas disponibles
            }
            int iniciob=BusquedaBin(_lineas, parrafoBuscado, 0, _lineas.Count-1);
            if (iniciob != -1)
            {
                while (iniciob > 0 && _lineas[iniciob].Inicio != 0)
                {
                    iniciob--;
                }
            }
            return iniciob;
            //if (_lineas[lineainicio].Parrafo.EsSiguiente(p))
            //{
            //    for (int i = lineainicio + 1; i < _lineas.Count; i++)
            //    {
                    
            //        if (_lineas[i].Parrafo == p)
            //        {
            //            return i;
            //        }
            //        if (!_lineas[i].Parrafo.EsSiguiente(p))
            //        {
            //            break;
            //        }
            //    }
            //}
            //else
            //{
            //    for (int i = lineainicio; i >= 0; i--)
            //    {
            //        Linea act = _lineas[i];
            //        if (act.Parrafo == p)
            //        {
            //            if (act.Inicio == 0)
            //            {
            //                return i;
            //            }
            //        } else if (!p.EsSiguiente(_lineas[i].Parrafo))
            //        {
            //            break;
            //        }
            //    }
            //}
            //return -1;
        }
        public int BuscarInicialDeParrafo(int lineainicio, Parrafo p)
        {
            AsegurarHasta(lineainicio);
            
            if (lineainicio<_lineas.Count)
            {
                while (!completo&&_lineas[lineainicio].Parrafo.EsSiguiente(p))
                {
                    AsegurarHasta(lineainicio + 1);
                    lineainicio++;
                }
            }
            
            int res = BuscarInt(lineainicio, p);
            //if (res==-1) throw new Exception("Linea no encontrada");
            return res;
        }

        public IEnumerator<Linea> GetEnumerator()
        {
            for (int i = 0; i < _lineas.Count; i++)
            {
                yield return _lineas[i];
                AsegurarHasta(i + 1);
            }
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        private void Limpiar()
        {
            _lineas.Clear();
        }

        private void RemoverDesde(int lineainicio)
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

