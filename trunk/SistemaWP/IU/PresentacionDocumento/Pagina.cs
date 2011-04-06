using System;
using System.Collections.Generic;
using System.Text;
using SWPEditor.Dominio;
using System.Drawing;
using SWPEditor.Aplicacion;
using SWPEditor.IU.Graficos;
using SWPEditor.Dominio.TextoFormato;
namespace SWPEditor.IU.PresentacionDocumento
{
    public class Pagina
    {
        public TamBloque Dimensiones { get; set; }
        public Margen Margen { get; set; }
        public int LineaInicio { get; private set; }
        public int LineaSiguientePagina { get { return LineaInicio + Cantidad; } }
        public int UltimaLinea { get { return LineaInicio + Cantidad - 1; } }
        private int _Cantidad;
        public int Cantidad
        {
            get
            {
                CompletarLineas();
                return _Cantidad;
            }            
        }
        private bool Completa { get; set; }
        private Medicion AltoActual { get; set; }
        ListaPaginas _paginas;
        ListaLineas _lineas;
        public bool EsPrimeraPagina
        {
            get
            {
                Linea lin = _lineas.Obtener(LineaInicio);
                return lin.EsPrimeraLineaParrafo && lin.Parrafo.EsPrimerParrafo;
            }
        }
        public bool EsUltimaPagina { 
            get {
                Linea lin=_lineas.Obtener(LineaInicio + Math.Max(0, Cantidad - 1));
                return lin.EsUltimaLineaParrafo&&lin.Parrafo.EsUltimoParrafo;
            }
        }
        public Pagina(int lineaInicio,ListaPaginas paginas,ListaLineas lineas)
        {
            _lineas = lineas;
            _paginas = paginas;
            LineaInicio = lineaInicio;
            Dimensiones = new TamBloque(new Medicion(210, Unidad.Milimetros), new Medicion(270, Unidad.Milimetros));
            Margen = new Margen() { Derecho = new Medicion(10, Unidad.Milimetros), Izquierdo = new Medicion(10, Unidad.Milimetros), Superior = new Medicion(10, Unidad.Milimetros), Inferior = new Medicion(10, Unidad.Milimetros) };
            AltoActual = Medicion.Cero;
        }
        public IEnumerable<Linea> ObtenerLineas()
        {
            for (int i = LineaInicio; i < Cantidad; i++)
            {
                yield return _lineas.Obtener(i);
            }
        }
        public bool Recalcular(int indice)
        {
            if (Completa && indice >= LineaInicio && indice < LineaInicio + _Cantidad)
            {
                _Cantidad = indice - LineaInicio-1;
                if (_Cantidad < 0) _Cantidad = 0;
                int lim=LineaInicio+_Cantidad;
                AltoActual = Medicion.Cero;
                for (int i = LineaInicio; i < lim; i++)
                {
                    AltoActual += _lineas.Obtener(i).AltoLinea;
                }
                Completa = false;
                return true;
            }
            else
            {
                AsegurarLinea(indice);
                if (indice <= LineaInicio + _Cantidad)
                    return true;
            }
            return false;
        }
        const int lineacompletarmax = 10000000;
        private void CompletarLineas()
        {
            AsegurarLinea(lineacompletarmax);                
            
        }
        private void AsegurarLinea(int numlinea)
        {
            if (!Completa)
            {
                if (numlinea >= LineaInicio + _Cantidad)
                {
                    using (IDisposable cursor = (IDisposable)_lineas.ObtenerDesde(LineaInicio + _Cantidad))
                    {
                        IEnumerable<Linea> lin = cursor as IEnumerable<Linea>;
                        Medicion altomax = ObtenerAltoLineas();
                        int indice = LineaInicio + _Cantidad;
                        foreach (Linea l in lin)
                        {
                            if (l.AltoLinea + AltoActual > altomax)
                            {
                                if (_Cantidad != 0) //cada página deberá tener por lo menos una línea (caso contrario sería bucle infinito)
                                {
                                    break;
                                }
                            }
                            AltoActual += l.AltoLinea;
                            _Cantidad++;
                            if (indice >= numlinea)
                            {
                                return;
                            }
                            indice++;
                        }
                        Completa = true;
                    }
                }
            }
        }
        public bool ContieneLinea(int numlinea)
        {
            AsegurarLinea(numlinea);
            return numlinea >= LineaInicio && numlinea < LineaInicio + _Cantidad;
        }
        public void Completar(ListaLineas lineas,Posicion posicion,int lineaPagina,int numCaracter)
        {
            posicion.Linea = lineas.Obtener(lineaPagina);
            posicion.IndiceLinea = lineaPagina;
            posicion.IndiceLineaPagina = lineaPagina - posicion.Pagina.LineaInicio;
            posicion.Linea.Completar(lineas, posicion, ObtenerAnchoLineas(),numCaracter);
        }
        private Medicion ObtenerAnchoLineas()
        {
            return Dimensiones.Ancho - Margen.Derecho - Margen.Izquierdo;
        }
        public void Dibujar(IGraficador g, Punto esquinaSuperior, ListaLineas _Lineas, Seleccion seleccion,AvanceBloques avance)
        {
            CompletarLineas();
            Punto pt = esquinaSuperior;
            pt.X += Margen.Izquierdo;
            pt.Y += Margen.Superior;
            Medicion anchoPagina = ObtenerAnchoLineas();
            int lim=LineaInicio+Cantidad;
            for (int numlinea=LineaInicio;numlinea<lim;numlinea++) 
            {
                Linea l = _Lineas.Obtener(numlinea);
                pt=l.Dibujar(g,pt, seleccion,avance,anchoPagina,true,true);
            }
        }
        private Medicion ObtenerAltoLineas(ListaLineas _Lineas)
        {
            int lim = LineaInicio + _Cantidad;
            Medicion suma = Medicion.Cero;
            for (int i = LineaInicio; i < lim; i++)
            {
                suma+=_Lineas.Obtener(i).AltoLinea;
            }
            return suma;
        }
        

        internal void CompletarPixels(ListaLineas _Lineas, Posicion posicion)
        {
            posicion.Linea.CompletarPosicionPixels(_Lineas, posicion,ObtenerAnchoLineas(),true,true);
            posicion.PosicionPagina = posicion.PosicionPagina+(new Punto(Margen.Izquierdo, Margen.Superior));
            
        }

        internal Posicion ObtenerPosicionPixels(ListaLineas _Lineas, Punto punto, Posicion posicion)
        {
            int lim = LineaInicio + Cantidad;
            Medicion suma = Medicion.Cero;
            Medicion posx = punto.X-Margen.Izquierdo;
            Medicion posy = punto.Y-Margen.Superior;
            if (posy < Medicion.Cero) posy = Medicion.Cero;
            Medicion anchoLineas=ObtenerAnchoLineas();
            for (int i = LineaInicio; i < lim; i++)
            {
                Linea l = _Lineas.Obtener(i);
                if (posy >= suma && posy <= suma + l.AltoLinea)
                {
                    int numcaracter = l.ObtenerNumCaracter(posx,anchoLineas);
                    Completar(_Lineas, posicion, i, numcaracter);
                    return posicion;
                }
                suma = suma + l.AltoLinea;
            }
            if (posy > suma && Cantidad > 0)
            {
                Linea l=_Lineas.Obtener(lim - 1);
                int nc = l.ObtenerNumCaracter(posx, anchoLineas);
                Completar(_Lineas, posicion, lim-1, nc);
                return posicion; 
                //return documento.CrearPosicion(numpagina, lim - 1, nc);
            }
            return posicion;
        }

        internal Medicion ObtenerAnchoLinea(int numlinea)
        {
            return Dimensiones.Ancho - Margen.Izquierdo - Margen.Derecho;
        }

        internal Medicion ObtenerAltoLineas()
        {
            return Dimensiones.Alto - Margen.Superior - Margen.Inferior;
        }
    }
    public class Margen
    {
        public Medicion Derecho { get; set; }
        public Medicion Izquierdo { get; set; }
        public Medicion Superior { get; set; }
        public Medicion Inferior { get; set; }
    }
}
