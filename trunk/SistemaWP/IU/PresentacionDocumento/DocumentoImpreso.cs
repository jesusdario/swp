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

        //List<Pagina> _Paginas = new List<Pagina>();
        ListaPaginas _Paginas;
        ListaLineas _Lineas;
        Documento _documento;
        public Documento Documento { get { return _documento; } }
        public DocumentoImpreso(Documento documento)
        {
            _documento = documento;
            _Paginas = new ListaPaginas(_documento, this);
            _Lineas = new ListaLineas(documento, _Paginas);
            _Paginas.Iniciar(_Lineas);
        }
        [System.Diagnostics.Conditional("DEBUG")]
        public void RevisarIntegridad()
        {
            int lineaanterior = 0;

            int posparrafoanterior = 0;
            foreach (Linea l in _Lineas)
            {
                if (l.Inicio == 0)
                {
                    posparrafoanterior = 0;
                }
                try
                {
                    Parrafo par = _documento.ObtenerParrafo(l.Parrafo.ID);
                }
                catch
                {
                    Debug.Assert(false);
                }
                Debug.Assert(posparrafoanterior == l.Inicio);
                Debug.Assert(l.Inicio >= 0);
                Debug.Assert(l.Inicio + l.Cantidad <= l.Parrafo.ObtenerLongitud());
                posparrafoanterior = l.Inicio + l.Cantidad;
            }
        }
        internal void Completar(Posicion posicion, int paginaInicioBusqueda, int indiceLinea, int numCaracter)
        {
            if (_Paginas.Obtener(paginaInicioBusqueda).ContieneLinea(indiceLinea))
            {
                Completar2(posicion, paginaInicioBusqueda, indiceLinea, numCaracter);
            }
            else if (_Paginas.Obtener(paginaInicioBusqueda).LineaInicio < indiceLinea)
            {
                int i = paginaInicioBusqueda;
                IEnumerable<Pagina> pags = _Paginas.ObtenerDesde(paginaInicioBusqueda);
                foreach (Pagina p in pags)
                {
                    if (p.ContieneLinea(indiceLinea))
                    {
                        Completar2(posicion, i, indiceLinea, numCaracter);
                        return;
                    }
                    i++;
                }

            }
            else
            {
                for (int i = paginaInicioBusqueda; i >= 0; i--)
                {
                    if (_Paginas.Obtener(i).ContieneLinea(indiceLinea))
                    {
                        Completar2(posicion, i, indiceLinea, numCaracter);
                        return;
                    }
                }
            }
        }

        private void Completar2(Posicion posicion, int indicePagina, int indiceLinea, int numCaracter)
        {
            Pagina p = _Paginas.Obtener(indicePagina);
            posicion.VDocumento = this;
            posicion.IndicePagina = indicePagina;
            posicion.Pagina = p;
            posicion.Pagina.Completar(_Lineas, posicion, indiceLinea, numCaracter);
        }
        internal void CompletarPixels(Posicion posicion)
        {
            posicion.Pagina.CompletarPixels(_Lineas, posicion);

        }
        public void DibujarPagina(IGraficador g, Punto esquinaSuperior, int numpagina, Seleccion seleccion)
        {
            AvanceBloques av = new AvanceBloques(_Lineas.Obtener(_Paginas.Obtener(numpagina).LineaInicio));
            _Paginas.Obtener(numpagina).Dibujar(g, esquinaSuperior, _Lineas, seleccion, av);
        }
        public Posicion CrearPosicion(int indicePagina, int indiceLinea, int numCaracter)
        {
            Posicion p = new Posicion(this);
            Completar(p, indicePagina, indiceLinea, numCaracter);
            return p;
        }

        public Posicion BuscarParrafo(Parrafo p)
        {
            return _Paginas.BuscarParrafo(p);
        }
        public Posicion ObtenerPosicionCursor(
            int lineabusqueda,
            Parrafo parrafo,
            int posicion)
        {
            Posicion pos = BuscarParrafo(parrafo);
            pos.Avanzar(posicion);
            return pos;
        }
        public int ObtenerNumPaginaConLinea(int numlinea)
        {
            int indice = 0;
            foreach (Pagina p in _Paginas.ObtenerDesde(0))
            {
                if (p.ContieneLinea(numlinea))
                {
                    return indice;
                }
                indice++;
            }

            return -1;
        }
        public void Repaginar(int lineainicio)
        {
            //_Paginas = new ListaPaginas(_documento, this);
            //_Lineas = new ListaLineas(_documento, _Paginas);
            //_Paginas.Iniciar(_Lineas);





            //Parrafo p=null;
            //Pagina actual=null;
            //Medicion posicionbase;
            //if (lineainicio == -1)
            //{
            //    lineainicio = 0;
            //    _Lineas.Limpiar();
            //    _Paginas.Clear();
            //    actual = new Pagina() { Cantidad = 0 };
            //    posicionbase = Medicion.Cero;
            //    p = _documento.ObtenerPrimerParrafo();
            //}
            //else
            //{
            //    if (_Paginas.Count > 0)
            //    {

            //        while (_Lineas.Obtener(lineainicio).Inicio != 0)
            //        {
            //            lineainicio--;
            //        }

            //        int indice = ObtenerNumPaginaConLinea(lineainicio);
            //        Pagina pag = ObtenerPagina(indice);
            //        _Paginas.RemoveRange(indice, _Paginas.Count - indice);
            //        _Lineas.RemoverDesde(lineainicio);

            //        pag.Cantidad = lineainicio - pag.LineaInicio;
            //        posicionbase = pag.ObtenerAltoLineas(_Lineas);
            //        actual = pag;
            //        if (_Lineas.Count > 0)
            //        {
            //            int idanterior = _Lineas[_Lineas.Count - 1].Parrafo.ID;
            //            p = _documento.ObtenerParrafo(idanterior).Siguiente;
            //        }
            //        else
            //        {
            //            p = _documento.ObtenerPrimerParrafo();
            //        }
            //    }
            //    else
            //    {
            //        actual = new Pagina() { Cantidad = 0 };
            //        posicionbase = Medicion.Cero;
            //        p = _documento.ObtenerPrimerParrafo();
            //    }
            //}
            //Parrafo siguiente;
            //int caractersiguiente;
            //int caracterinicio = 0;
            //do
            //{
            //    actual.Paginar(_Lineas, p, caracterinicio, out siguiente, out caractersiguiente);
            //    actual.Cantidad = _Lineas.Count-actual.LineaInicio;
            //    _Paginas.Add(actual);
            //    if (siguiente != null)
            //    {
            //        actual = new Pagina() { LineaInicio=_Lineas.Count, Cantidad = 0 };
            //    }
            //    p = siguiente;
            //    caracterinicio = caractersiguiente;
            //} while (siguiente != null);            
        }
        internal Posicion ObtenerPosicionPixels(int numpagina, Punto punto)
        {
            Pagina p = _Paginas.Obtener(numpagina);
            Posicion pos = new Posicion(this);
            pos.IndicePagina = numpagina;
            pos.Pagina = p;
            return p.ObtenerPosicionPixels(_Lineas, punto, pos);
        }
        public IEnumerable<Pagina> ObtenerDesde(int indicePagina)
        {
            foreach (Pagina p in _Paginas.ObtenerDesde(indicePagina))
            {
                yield return p;
            }
        }
        internal Pagina ObtenerPagina(int indicePagina)
        {
            return _Paginas.Obtener(indicePagina);
        }



        internal Linea ObtenerLinea(int indiceLinea)
        {
            return _Lineas.Obtener(indiceLinea);
        }
        //internal int ObtenerNumLineas()
        //{
        //    return _Lineas.Count;
        //}



        internal bool EsUltimaLinea(int indiceLinea)
        {
            return _Lineas.EsUltimaLinea(indiceLinea);
        }

        internal bool EsUltimaPagina(int indicePagina)
        {
            return _Paginas.EsUltimaPagina(indicePagina);
        }
    }
}
    
   
