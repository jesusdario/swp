using System;
using System.Collections.Generic;
using System.Text;
using SistemaWP.Dominio;
using System.Drawing;
using SistemaWP.Aplicacion;
using SistemaWP.IU.Graficos;
using System.Diagnostics;

namespace SistemaWP.IU.PresentacionDocumento
{
    public class DocumentoImpreso
    {
        class ListaLineas:List<Linea>
        {
            public int BuscarInicialDeParrafo(int lineainicio, Parrafo p)
            {
                if (this[lineainicio].Parrafo.EsSiguiente(p))
                {
                    for (int i = lineainicio + 1; i < Count; i++)
                    {
                        if (this[i].Parrafo == p)
                        {
                            return i;
                        }
                    }
                }
                else
                {
                    for (int i = lineainicio; i >= 0; i--)
                    {
                        Linea act=this[i];
                        if (act.Parrafo == p && act.Inicio == 0) {
                            return i;
                        }
                    }
                }
                throw new Exception("Linea no encontrada");
            }
        }
        List<Pagina> _Paginas = new List<Pagina>();
        ListaLineas _Lineas = new ListaLineas();
        Documento _documento;
        public DocumentoImpreso(Documento documento)
        {
            _documento = documento;            
        }
        [System.Diagnostics.Conditional("DEBUG")]
        public void RevisarIntegridad()
        {
            int lineaanterior=0;
            foreach (Pagina p in _Paginas)
            {
                Debug.Assert(p.LineaInicio >= 0);
                Debug.Assert(p.LineaInicio == lineaanterior);
                Debug.Assert(p.LineaInicio < _Lineas.Count);
                Debug.Assert(p.LineaInicio+p.Cantidad <= _Lineas.Count);
                lineaanterior = p.LineaInicio + p.Cantidad;
            }
            int posparrafoanterior = 0;
            foreach (Linea l in _Lineas)
            {
                if (l.Inicio == 0)
                {
                    posparrafoanterior = 0;
                }
                try {
                    Parrafo par=_documento.ObtenerParrafo(l.Parrafo.ID);
                } catch {
                    Debug.Assert(false);
                }
                Debug.Assert(posparrafoanterior == l.Inicio);
                Debug.Assert(l.Inicio >= 0);
                Debug.Assert(l.Inicio+l.Cantidad <= l.Parrafo.ObtenerLongitud());
                posparrafoanterior=l.Inicio+l.Cantidad;
            }
        }
        public void Completar(Posicion posicion, int paginaInicioBusqueda,int indiceLinea, int numCaracter)
        {
            if (_Paginas[paginaInicioBusqueda].ContieneLinea(indiceLinea))
            {
                Completar2(posicion, paginaInicioBusqueda, indiceLinea, numCaracter);
            } else if (_Paginas[paginaInicioBusqueda].LineaInicio < indiceLinea)
            {
                for (int i = paginaInicioBusqueda; i < _Paginas.Count; i++)
                {
                    if (_Paginas[i].ContieneLinea(indiceLinea))
                    {
                        Completar2(posicion, i, indiceLinea, numCaracter);
                        return;
                    }
                }
            }
            else 
            {
                for (int i = paginaInicioBusqueda; i >=0; i--)
                {
                    if (_Paginas[i].ContieneLinea(indiceLinea))
                    {
                        Completar2(posicion, i, indiceLinea, numCaracter);
                        return;
                    }
                }
            }
        }
        private int BuscarLineaInicioParrafo(int lineaInicio, Parrafo p)
        {
            return _Lineas.BuscarInicialDeParrafo(lineaInicio, p);
        }
        //    Linea l = _Lineas[lineaInicio];
        //    if (l.Parrafo == p)
        //    {
        //        if (l.Inicio == 0)
        //        {
        //            return lineaInicio;
        //        }
        //        else
        //        {
        //            while (l.Inicio != 0)
        //            {
        //                lineaInicio--;
        //                l = _Lineas[lineaInicio];
        //            }
        //            return lineaInicio;
        //        }
        //    }
        //    else
        //    {
        //        if (l.Parrafo.EsSiguiente(p))
        //        {
        //            int lim=_Lineas.Count;
        //            while (lineaInicio<lim&&l.Parrafo != p)
        //            {
        //                lineaInicio++;
        //                l = _Lineas[lineaInicio];
        //            }
        //            return lineaInicio;
        //        }
        //        else
        //        {
        //            while (lineaInicio>0&&l.Parrafo != p)
        //            {
        //                lineaInicio--;
        //                l = _Lineas[lineaInicio];
        //            }
        //            return lineaInicio;
        //        }
        //    }
        //}
        private void Completar2(Posicion posicion, int indicePagina, int indiceLinea, int numCaracter)
        {
            Pagina p=_Paginas[indicePagina];
            posicion.VDocumento = this;
            posicion.IndicePagina = indicePagina;
            posicion.Pagina = p;
            posicion.Pagina.Completar(_Lineas, posicion, indiceLinea, numCaracter);
        }
        public void CompletarPixels(Posicion posicion)
        {
            posicion.Pagina.CompletarPixels(_Lineas, posicion);
            
        }
        public void DibujarPagina(IGraficador g, Punto esquinaSuperior, int numpagina, Seleccion seleccion)
        {
            if (_Paginas.Count == 0) return;
            _Paginas[numpagina].Dibujar(g, esquinaSuperior,_Lineas, seleccion);
        }
        public Posicion CrearPosicion(int indicePagina, int indiceLinea, int numCaracter)
        {
            Posicion p = new Posicion(this);
            Completar(p, indicePagina, indiceLinea, numCaracter);
            return p;
        }
        Posicion BuscarParrafo(Parrafo parrafoBuscado,int paginainicio, int paginafin)
        {
            if (paginainicio >= paginafin)
            {
                int centro2 = Math.Max(paginainicio, paginafin);
            reintentar:
                if (centro2 >= _Paginas.Count) 
                    centro2 = _Paginas.Count - 1;
                Pagina elemento2 = _Paginas[centro2];
                int linea = BuscarLineaInicioParrafo(elemento2.LineaInicio,parrafoBuscado);
                Posicion p = new Posicion(this);
                Completar(p, centro2, linea, 0);
                return p;
                
            }
            int centro = paginainicio + paginafin;
            Pagina elemento = _Paginas[centro];
            Linea inicial = _Lineas[elemento.LineaInicio];
            if (inicial.Parrafo == parrafoBuscado)
            {
                int linea = BuscarLineaInicioParrafo(elemento.LineaInicio, parrafoBuscado);
                Posicion p = new Posicion(this);
                Completar(p, centro, linea, 0);
                return p;
                //return CrearPosicion(centro, elemento.LineaInicio, 0);
            }
            else
            {
                if (inicial.Parrafo.EsSiguiente(parrafoBuscado))
                {
                    return BuscarParrafo(parrafoBuscado, centro + 1, paginafin);
                }
                else
                {
                    return BuscarParrafo(parrafoBuscado, paginainicio,centro-1);
                }
            }
        }
        public Posicion BuscarParrafo(Parrafo p)
        {
            return BuscarParrafo(p,0,_Paginas.Count-1);
        }
        public Posicion ObtenerPosicionCursor(
            int lineabusqueda,
            Parrafo parrafo, 
            int posicion)
        {
            Posicion pos=BuscarParrafo(parrafo);
            pos.Avanzar(posicion);            
            return pos;
        }
        public int ObtenerNumPaginaConLinea(int numlinea)
        {
            for (int i = 0; i < _Paginas.Count;i++ )
            {
                Pagina p = _Paginas[i];
                
                if (p.ContieneLinea(numlinea))
                {
                    return i;
                }
            }
            return -1;
        }
        public void Repaginar(int lineainicio)
        {
            Parrafo p=null;
            Pagina actual=null;
            Medicion posicionbase;
            if (lineainicio == -1)
            {
                lineainicio = 0;
                _Lineas.Clear();
                _Paginas.Clear();
                actual = new Pagina() { Cantidad = 0 };
                posicionbase = Medicion.Cero;
                p = _documento.ObtenerPrimerParrafo();
            }
            else
            {
                if (_Paginas.Count > 0)
                {

                    while (_Lineas[lineainicio].Inicio != 0)
                    {
                        lineainicio--;
                    }

                    int indice = ObtenerNumPaginaConLinea(lineainicio);
                    Pagina pag = ObtenerPagina(indice);
                    _Paginas.RemoveRange(indice, _Paginas.Count - indice);
                    _Lineas.RemoveRange(lineainicio, _Lineas.Count - lineainicio);

                    pag.Cantidad = lineainicio - pag.LineaInicio;
                    posicionbase = pag.ObtenerAltoLineas(_Lineas);
                    actual = pag;
                    if (_Lineas.Count > 0)
                    {
                        int idanterior = _Lineas[_Lineas.Count - 1].Parrafo.ID;
                        p = _documento.ObtenerParrafo(idanterior).Siguiente;
                    }
                    else
                    {
                        p = _documento.ObtenerPrimerParrafo();
                    }
                }
                else
                {
                    actual = new Pagina() { Cantidad = 0 };
                    posicionbase = Medicion.Cero;
                    p = _documento.ObtenerPrimerParrafo();
                }
            }
            Parrafo siguiente;
            int caractersiguiente;
            int caracterinicio = 0;
            do
            {
                actual.Paginar(_Lineas, p, caracterinicio, out siguiente, out caractersiguiente);
                actual.Cantidad = _Lineas.Count-actual.LineaInicio;
                _Paginas.Add(actual);
                if (siguiente != null)
                {
                    actual = new Pagina() { LineaInicio=_Lineas.Count, Cantidad = 0 };
                }
                p = siguiente;
                caracterinicio = caractersiguiente;
            } while (siguiente != null);            
        }
        internal Posicion ObtenerPosicionPixels(int numpagina,Punto punto)
        {
            Pagina p = _Paginas[numpagina];
            Posicion pos = new Posicion(this);
            pos.IndicePagina = numpagina;
            pos.Pagina = p;
            return p.ObtenerPosicionPixels(_Lineas,punto, pos);
        }

        internal Pagina ObtenerPagina(int indicePagina)
        {
            return _Paginas[indicePagina];
        }


        internal int ObtenerNumPaginas()
        {
            return _Paginas.Count;
        }

        internal Linea ObtenerLinea(int indiceLinea)
        {
            return _Lineas[indiceLinea];
        }
        internal int ObtenerNumLineas()
        {
            return _Lineas.Count;
        }

        
    }    
}
