using System;
using System.Collections.Generic;
using System.Text;
using SWPEditor.IU.Graficos;
using SWPEditor.Dominio;
using SWPEditor.IU.PresentacionDocumento;
using SWPEditor.Aplicacion;
using SWPEditor.Dominio.TextoFormato;

namespace SWPEditor.IU.VistaDocumento
{
    public class LienzoPagina
    {
        public int IDPagina { get; set; }
        public Punto PosicionInicioDibujo { get; set; }
        public LienzoPagina(int idpagina,Punto esquinaSuperior)
        {
            IDPagina = idpagina;
            PosicionInicioDibujo = esquinaSuperior;
        }
        public void DibujarCursor(IGraficador graficador,Posicion posicion)
        {
            Lapiz lp = new Lapiz() { Ancho = new Medicion(0.5, Unidad.Milimetros), Brocha = new BrochaSolida(new ColorDocumento(127, 0, 0)) };
            Posicion pos = posicion ;
            Punto punto2 = new Punto(pos.PosicionPagina.X, pos.PosicionPixelY + pos.AltoLinea);
            graficador.DibujarLinea(lp, pos.PosicionPagina - PosicionInicioDibujo, punto2-PosicionInicioDibujo);
        }
        public void Dibujar(IGraficador graf,DocumentoImpreso documento,Posicion posicion,Seleccion seleccion)
        {
            Pagina p=documento.ObtenerPagina(IDPagina);
            if (p == null) return;
            graf.RellenarRectangulo(BrochaSolida.Blanco, new Punto(Medicion.Cero, Medicion.Cero)-PosicionInicioDibujo, p.Dimensiones);
            graf.DibujarRectangulo(Lapiz.Negro, new Punto(Medicion.Cero, Medicion.Cero) - PosicionInicioDibujo, p.Dimensiones);
            documento.DibujarPagina(graf, new Punto(Medicion.Cero, Medicion.Cero) - PosicionInicioDibujo, IDPagina, seleccion);
            if (IDPagina == posicion.IndicePagina&&seleccion==null)
            {
                DibujarCursor(graf,posicion);
            }
        }
    }
}
