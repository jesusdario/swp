using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SWPEditor.Dominio;
using SWPEditor.IU.Graficos;
using System.Windows.Media;
using SWPEditor.IU.PresentacionDocumento;
using System.Windows;
namespace SWPEditor.UI.WPF.Graficadores
{
    class Graficador:SWPEditor.IU.Graficos.IGraficador
    {
        System.Windows.Media.DrawingGroup grupo = new DrawingGroup();
        System.Windows.Media.DrawingContext contexto;
        public Graficador()
        {
            contexto=grupo.Open();
        }
        Color CrearColor(SWPEditor.Dominio.TextoFormato.ColorDocumento color)
        {
            return Color.FromArgb((byte)color.A, (byte)color.R, (byte)color.G, (byte)color.B);
        }
        public Brush CrearBrocha(SWPEditor.IU.Graficos.Brocha brocha)
        {
            SolidColorBrush b = new SolidColorBrush(CrearColor(((SWPEditor.IU.Graficos.BrochaSolida)brocha).Color));
            b.Freeze();
            return b;
        }
        public System.Windows.Point CrearPunto(Punto punto)
        {
            return new System.Windows.Point(ObtenerMedida(punto.X), ObtenerMedida(punto.Y));
        }
        public double ObtenerMedida(Medicion medida)
        {
            return medida.ConvertirA(Unidad.Pantalla).Valor;
        }
        public Medicion ObtenerMedida(double valor)
        {
            return new Medicion(valor, Unidad.Puntos);
        }
        public Pen CrearLapiz(SWPEditor.IU.Graficos.Lapiz lapiz)
        {
            Pen p = new Pen(CrearBrocha(lapiz.Brocha), ObtenerMedida(lapiz.Ancho));
            p.Freeze();
            return p;
        }
        public Size CrearTam(TamBloque tambloque)
        {
            return new Size(ObtenerMedida(tambloque.Ancho), ObtenerMedida(tambloque.Alto));
        }
        public void DibujarLinea(SWPEditor.IU.Graficos.Lapiz lapiz, SWPEditor.IU.PresentacionDocumento.Punto inicio, SWPEditor.IU.PresentacionDocumento.Punto fin)
        {
            contexto.DrawLine(CrearLapiz(lapiz), CrearPunto(inicio), CrearPunto(fin));
        }
        public void DibujarRectangulo(SWPEditor.IU.Graficos.Lapiz lapiz, SWPEditor.IU.PresentacionDocumento.Punto inicio, SWPEditor.IU.PresentacionDocumento.TamBloque bloque)
        {
            contexto.DrawRectangle(null, CrearLapiz(lapiz), new Rect(CrearPunto(inicio), CrearTam(bloque)));
        }
        public void DibujarTexto(SWPEditor.IU.PresentacionDocumento.Punto posicion, SWPEditor.IU.Graficos.Letra letra, SWPEditor.IU.Graficos.Brocha brocha, string texto)
        {
            Typeface t=new Typeface(new FontFamily(letra.Familia),new FontStyle(),new FontWeight(),new FontStretch());
            FormattedText f=new FormattedText(texto,System.Globalization.CultureInfo.InvariantCulture,FlowDirection.LeftToRight,t,ObtenerMedida(letra.Tamaño),
                CrearBrocha(brocha));
            contexto.DrawText(f, CrearPunto(posicion));
        }
        public SWPEditor.IU.PresentacionDocumento.TamBloque MedirTexto(SWPEditor.IU.Graficos.Letra letra, string texto)
        {
            Typeface t = new Typeface(new FontFamily(letra.Familia), new FontStyle(), new FontWeight(), new FontStretch());
            FormattedText f = new FormattedText(texto, System.Globalization.CultureInfo.InvariantCulture, FlowDirection.LeftToRight, t, ObtenerMedida(letra.Tamaño),
                Brushes.Black);
            return new TamBloque(ObtenerMedida(f.WidthIncludingTrailingWhitespace),ObtenerMedida(f.Height));            
        }
        public void RellenarRectangulo(SWPEditor.IU.Graficos.Brocha brocha, SWPEditor.IU.PresentacionDocumento.Punto inicio, SWPEditor.IU.PresentacionDocumento.TamBloque bloque)
        {
            contexto.DrawRectangle(CrearBrocha(brocha), null, new Rect(CrearPunto(inicio), CrearTam(bloque)));
        }
        public Medicion MedirUnion(Letra letra, string a, string b)
        {
            return Medicion.Cero;
        }
        public void TrasladarOrigen(SWPEditor.IU.PresentacionDocumento.Punto Punto)
        {
        }
        public Medicion MedirBaseTexto(SWPEditor.IU.Graficos.Letra letra)
        {
            //FontFamily f=Fonts.SystemFontFamilies.Where(x => x.FamilyNames.Values.Contains(letra.Familia));
            Typeface t = new Typeface(letra.Familia);
            FormattedText f = new FormattedText("M", System.Globalization.CultureInfo.InvariantCulture, FlowDirection.LeftToRight, t, ObtenerMedida(letra.Tamaño), Brushes.Black);
            return ObtenerMedida(f.Height-f.Baseline);
            //return //ObtenerMedida(f.Height, f.WidthIncludingTrailingWhitespace);
        }
        public Medicion MedirAltoTexto(SWPEditor.IU.Graficos.Letra letra)
        {
            Typeface t = new Typeface(letra.Familia);
            FormattedText f = new FormattedText("M", System.Globalization.CultureInfo.InvariantCulture, FlowDirection.LeftToRight, t, ObtenerMedida(letra.Tamaño), Brushes.Black);
            return ObtenerMedida(f.Height);
        }
        public Medicion MedirEspacioLineas(SWPEditor.IU.Graficos.Letra letra)
        {
            Typeface t = new Typeface(letra.Familia);
            FormattedText f = new FormattedText("M", System.Globalization.CultureInfo.InvariantCulture, FlowDirection.LeftToRight, t, ObtenerMedida(letra.Tamaño), Brushes.Black);
            return ObtenerMedida(f.LineHeight);
        }
    }
}
