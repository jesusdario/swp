using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using SistemaWP.Dominio;
using SistemaWP.IU.Graficos;
using SistemaWP.IU.VistaDocumento;
using SistemaWP.Dominio.TextoFormato;
using System.Diagnostics;

namespace SistemaWP.IU.PresentacionDocumento
{
    class Estilo
    {
        public Letra Letra { get; set; }
        public Brocha ColorFondo { get; set; }
        public Brocha ColorLetra { get; set; }
        public Estilo(Bloque bloque)
        {
            Debug.Assert(bloque != null);
            Formato f = bloque.Formato;
            
            ColorFondo = new BrochaSolida(f.ObtenerColorFondo());
            ColorLetra = new BrochaSolida(f.ObtenerColorLetra());
            Letra = new Letra()
            {
                Tamaño = f.ObtenerTamLetra(),
                Familia = f.ObtenerFamiliaLetra(),
                Negrilla = f.ObtenerNegrilla(),
                Subrayado = f.ObtenerSubrayado(),
                Cursiva = f.ObtenerCursiva()
            };
        }
        //public void DibujarSinFondo(IGraficador graficos, Punto posicionbase, string texto)
        //{
        //    graficos.DibujarTexto(posicionbase, Letra, ColorLetra, texto);
        //}
        public void Dibujar(IGraficador graficos, Punto posicionbase, string texto)
        {
            //TamBloque b=Medir(texto);
            //graficos.RellenarRectangulo(ColorFondo, posicionbase, b);
            graficos.DibujarTexto(posicionbase, Letra, ColorLetra, texto);
        }
        public Punto DibujarFondo(IGraficador graficos, Punto posicionbase, string texto, string anteriortexto)
        {
            if (!string.IsNullOrEmpty(anteriortexto))
            {
                posicionbase.X += grafpantalla.MedirUnion(Letra, anteriortexto, texto);
            }
            TamBloque b = Medir(texto);
            graficos.RellenarRectangulo(ColorFondo, posicionbase, b);
            return new Punto(posicionbase.X + b.Ancho, posicionbase.Y);
        }
        public Punto DibujarConTam(IGraficador graficos, Punto posicionbase, string texto, string anteriortexto)
        {
            if (!string.IsNullOrEmpty(anteriortexto))
            {
                posicionbase.X += grafpantalla.MedirUnion(Letra, anteriortexto, texto);
            }
            TamBloque b = Medir(texto);
            //graficos.RellenarRectangulo(ColorFondo, posicionbase, b);
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
        public Medicion MedirBase()
        {
            return grafpantalla.MedirBaseTexto(Letra);
        }
        public Medicion MedirAlto()
        {
            return grafpantalla.MedirAltoTexto(Letra);
        }
        public Medicion MedirEspacioLineas()
        {
            return grafpantalla.MedirEspacioLineas(Letra);
        }
        internal Estilo Clonar()
        {
            return (Estilo)this.MemberwiseClone();
        }
    }
}
