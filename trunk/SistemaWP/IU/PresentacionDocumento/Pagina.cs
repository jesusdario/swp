using System;
using System.Collections.Generic;
using System.Text;
using SistemaWP.Dominio;
using System.Drawing;
using SistemaWP.Aplicacion;
using SistemaWP.IU.Graficos;
namespace SistemaWP.IU.PresentacionDocumento
{
    public class Pagina
    {
        public TamBloque Dimensiones { get; set; }
        public Margen Margen { get; set; }
        public int LineaInicio { get; set; }
        public int Cantidad { get; set; }
        public Pagina()
        {
            Dimensiones = new TamBloque(new Medicion(210, Unidad.Milimetros), new Medicion(270, Unidad.Milimetros));
            Margen = new Margen() { Derecho = new Medicion(10, Unidad.Milimetros), Izquierdo = new Medicion(10, Unidad.Milimetros), Superior = new Medicion(10, Unidad.Milimetros), Inferior = new Medicion(10, Unidad.Milimetros) };
        }
        public bool ContieneLinea(int numlinea)
        {
            return numlinea >= LineaInicio && numlinea < LineaInicio + Cantidad;
        }
        public void Completar(List<Linea> lineas,Posicion posicion,int lineaPagina,int numCaracter)
        {
            posicion.Linea = lineas[lineaPagina];
            posicion.IndiceLinea = lineaPagina;
            posicion.IndiceLineaPagina = lineaPagina - posicion.Pagina.LineaInicio;            
            posicion.Linea.Completar(lineas, posicion, numCaracter);
        }
        public void Dibujar(IGraficador g, Punto esquinaSuperior, List<Linea> _Lineas, Seleccion seleccion)
        {
            Punto pt = esquinaSuperior;
            pt.X += Margen.Izquierdo;
            pt.Y += Margen.Superior;
            int lim=LineaInicio+Cantidad;
            for (int numlinea=LineaInicio;numlinea<lim;numlinea++) 
            {
                Linea l = _Lineas[numlinea];
                pt=l.Dibujar(g,pt, seleccion);
            }
        }
        public Medicion ObtenerAltoLineas(List<Linea> _Lineas)
        {
            int lim = LineaInicio + Cantidad;
            Medicion suma = Medicion.Cero;
            for (int i = LineaInicio; i < lim; i++)
            {
                suma+=_Lineas[i].AltoLinea;
            }
            return suma;
        }
        public void Paginar(List<Linea> _Lineas, Parrafo ParrafoInicial, int CaracterInicial,out Parrafo ParrafoFinal, out int CaracterSiguiente)
        {
            Parrafo p = ParrafoInicial;
            Medicion ancho=Dimensiones.Ancho-Margen.Derecho-Margen.Izquierdo;
            Medicion alto=Dimensiones.Alto-Margen.Superior-Margen.Inferior;
            int caracterinicio=CaracterInicial;
            Medicion altoactual = Medicion.Cero;
            while (p != null)
            {
                Linea l=Linea.ObtenerSiguienteLinea(p, caracterinicio, ancho);                
                if (altoactual+l.AltoLinea > alto) //la línea pasa el fin de página.
                {
                    ParrafoFinal = p;
                    CaracterSiguiente = caracterinicio;
                    return;
                }                
                altoactual = altoactual + l.AltoLinea;
                _Lineas.Add(l);
                caracterinicio = l.Inicio + l.Cantidad;
                if (l.Inicio + l.Cantidad == p.ObtenerLongitud())
                {
                    p = p.Siguiente;
                    caracterinicio = 0;
                }
            }
            ParrafoFinal = null;
            CaracterSiguiente = 0;
        }

        internal void CompletarPixels(List<Linea> _Lineas, Posicion posicion)
        {
            posicion.Linea.CompletarPosicionPixels(_Lineas, posicion);
            posicion.PosicionPagina = posicion.PosicionPagina+(new Punto(Margen.Izquierdo, Margen.Superior));
            
        }

        internal Posicion ObtenerPosicionPixels(List<Linea> _Lineas,Punto punto,Posicion posicion)
        {
            int lim = LineaInicio + Cantidad;
            Medicion suma = Medicion.Cero;
            Medicion posx = punto.X-Margen.Izquierdo;
            Medicion posy = punto.Y-Margen.Superior;
            if (posy < Medicion.Cero) posy = Medicion.Cero;

            for (int i = LineaInicio; i < lim; i++)
            {
                Linea l = _Lineas[i];
                if (posy >= suma && posy <= suma + l.AltoLinea)
                {
                    int numcaracter = l.ObtenerNumCaracter(posx);
                    Completar(_Lineas, posicion, i, numcaracter);
                    return posicion;
                }
                suma = suma + l.AltoLinea;
            }
            if (posy > suma && Cantidad > 0)
            {
                Linea l=_Lineas[lim - 1];
                int nc = l.ObtenerNumCaracter(posx);
                Completar(_Lineas, posicion, lim-1, nc);
                return posicion; 
                //return documento.CrearPosicion(numpagina, lim - 1, nc);
            }
            return posicion;
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
