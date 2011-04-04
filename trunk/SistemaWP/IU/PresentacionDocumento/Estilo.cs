using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using SWPEditor.Dominio;
using SWPEditor.IU.Graficos;
using SWPEditor.IU.VistaDocumento;
using SWPEditor.Dominio.TextoFormato;
using System.Diagnostics;

namespace SWPEditor.IU.PresentacionDocumento
{
    class Estilo
    {
        public Letra Letra { get; set; }
        public Brocha ColorFondo { get; set; }
        public Brocha ColorLetra { get; set; }
        public Estilo(Bloque bloque)
        {
            //Debug.Assert(bloque != null);
            Debug.Assert(bloque.Formato != null);
            Formato f = bloque.Formato;
            
            ColorFondo = new BrochaSolida(f.ObtenerColorFondo());
            ColorLetra = new BrochaSolida(f.ObtenerColorLetra());
            Letra = new Letra()
            {
                Tamaño = f.ObtenerTamLetraEscalado(),
                Familia = f.ObtenerFamiliaLetra(),
                Negrilla = f.ObtenerNegrilla(),
                Subrayado = f.ObtenerSubrayado(),
                Cursiva = f.ObtenerCursiva()
            };
        }       
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
                posicionbase.X += GraficadorConsultas.MedirUnion(Letra, anteriortexto, texto);
            }
            TamBloque b = Medir(texto);
            graficos.RellenarRectangulo(ColorFondo, posicionbase, b);
            return new Punto(posicionbase.X + b.Ancho, posicionbase.Y);
        }
        public Punto DibujarConTam(IGraficador graficos, Punto posicionbase, string texto, string anteriortexto)
        {
            if (!string.IsNullOrEmpty(anteriortexto))
            {
                posicionbase.X += GraficadorConsultas.MedirUnion(Letra, anteriortexto, texto);
            }
            TamBloque b = Medir(texto);
            //graficos.RellenarRectangulo(ColorFondo, posicionbase, b);
            graficos.DibujarTexto(posicionbase, Letra, ColorLetra, texto);
            return new Punto(posicionbase.X+b.Ancho,posicionbase.Y);
        }
        [ThreadStatic]
        static IGraficador _GraficadorConsultas;
        public static IGraficador GraficadorConsultas
        {
            get
            {
                return _GraficadorConsultas;
            }
            set
            {
                _GraficadorConsultas = value;
            }
        }
        static Estilo()
        {
            
        }
        public TamBloque Medir(string texto)
        {
            return _GraficadorConsultas.MedirTexto(Letra, texto);
            
        }
        public Medicion MedirBase()
        {
            return _GraficadorConsultas.MedirBaseTexto(Letra);
        }
        public Medicion MedirAlto()
        {
            return _GraficadorConsultas.MedirAltoTexto(Letra);
        }
        public Medicion MedirEspacioLineas()
        {
            return _GraficadorConsultas.MedirEspacioLineas(Letra);
        }
        internal Estilo Clonar()
        {
            return (Estilo)this.MemberwiseClone();
        }
    }
}
