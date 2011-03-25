using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using SistemaWP.Dominio;
using SistemaWP.IU.Graficos;
using SistemaWP.IU.VistaDocumento;

namespace SistemaWP.IU.PresentacionDocumento
{
    class Estilo
    {
        public Letra Letra { get; set; }
        public Brocha ColorFondo { get; set; }
        public Brocha ColorLetra { get; set; }
        public Estilo()
        {
            Letra = new Letra() { Tamaño = new Medicion(10) };
            ColorFondo = new BrochaSolida() { Color = new ColorDocumento(255, 255, 255) };
            ColorLetra = new BrochaSolida() { Color = new ColorDocumento(0, 0, 0) };
        }
        public Punto DibujarFondo(IGraficador graficos, Punto posicionbase, string texto)
        {
            TamBloque b = Medir(texto);
            graficos.RellenarRectangulo(ColorFondo, posicionbase, b);
            return new Punto(posicionbase.X + b.Ancho, posicionbase.Y);
        }
        public void DibujarSinFondo(IGraficador graficos, Punto posicionbase, string texto)
        {
            graficos.DibujarTexto(posicionbase, Letra, ColorLetra, texto);
        }
        public void Dibujar(IGraficador graficos, Punto posicionbase, string texto)
        {
            TamBloque b=Medir(texto);
            graficos.RellenarRectangulo(ColorFondo, posicionbase, b);
            graficos.DibujarTexto(posicionbase, Letra, ColorLetra, texto);
        }
        public Punto DibujarConTam(IGraficador graficos, Punto posicionbase, string texto,string anteriortexto)
        {
            if (!string.IsNullOrEmpty(anteriortexto))
            {
                posicionbase.X += grafpantalla.MedirUnion(Letra, anteriortexto, texto);
            }
            TamBloque b = Medir(texto);
            graficos.RellenarRectangulo(ColorFondo, posicionbase, b);
            graficos.DibujarTexto(posicionbase, Letra, ColorLetra, texto);
            return new Punto(posicionbase.X+b.Ancho,posicionbase.Y);
        }
        
        static Graficador grafpantalla;
        static Estilo()
        {
            Bitmap bmp = new Bitmap(1, 1, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            bmp.SetResolution(10000, 10000);
            grafpantalla=new Graficador(Graphics.FromImage(bmp));
        }
        public TamBloque Medir(string texto)
        {
            return grafpantalla.MedirTexto(Letra, texto);
            
        }
    }
}
