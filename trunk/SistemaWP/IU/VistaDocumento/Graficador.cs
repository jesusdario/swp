using System;
using System.Collections.Generic;
using System.Text;
using SistemaWP.IU.PresentacionDocumento;
using SistemaWP.IU.Graficos;
using System.Drawing;
using SistemaWP.Dominio;

namespace SistemaWP.IU.VistaDocumento
{
    class Stock<K,T>
    {
        Dictionary<K, T> _datos = new Dictionary<K, T>();
        Func<K, T> _constructor;
        public Stock(Func<K,T> constructor) {
            _constructor = constructor;
        }
        public T Obtener(K llave)
        {
            if (!_datos.ContainsKey(llave))
            {
                T nuevovalor=_constructor(llave);
                _datos.Add(llave, nuevovalor);
            }
            return _datos[llave];
        }
    }
    class Graficador : IGraficador
    {
        System.Drawing.Graphics g;
        Stock<Brocha, SolidBrush> _brochas;
        Stock<Lapiz, Pen> _lapices;
        Stock<Letra, Font> _letras;
        static System.Drawing.Color ObtenerColor(ColorDocumento color)
        {
            return System.Drawing.Color.FromArgb(color.A,color.R, color.G, color.B);
        }
        public Punto Traducir(PointF pt)
        {
            return new Punto(
                new Medicion(pt.X, unidaddispositivox).ConvertirA(Unidad.Milimetros),
                new Medicion(pt.Y, unidaddispositivoy).ConvertirA(Unidad.Milimetros));
        }
        public TamBloque Traducir(SizeF sz)
        {
            return new TamBloque(
                new Medicion(sz.Width, unidaddispositivox).ConvertirA(Unidad.Milimetros),
                new Medicion(sz.Height, unidaddispositivoy).ConvertirA(Unidad.Milimetros));
        }
        public PointF Traducir(Punto punto)
        {
            return new PointF(
                (float)punto.X.ConvertirA(unidaddispositivox).Valor,
                (float)punto.Y.ConvertirA(unidaddispositivoy).Valor);
        }
        public SizeF Traducir(TamBloque bloque)
        {
            return new SizeF(
                (float)bloque.Ancho.ConvertirA(unidaddispositivox).Valor,
                (float)bloque.Alto.ConvertirA(unidaddispositivoy).Valor);
        }
        
        Unidad unidaddispositivox;
        Unidad unidaddispositivoy;
        Unidad intermedia;
        public Graficador(System.Drawing.Graphics graficos)
        {
            float dpix = graficos.DpiX;
            float dpiy = graficos.DpiY;
            //graficos.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            graficos.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            unidaddispositivox = new Unidad("UnidadDispositivoX", "dpix", 1 / dpix, Unidad.Pulgadas);
            unidaddispositivoy = new Unidad("UnidadDispositivoY", "dpiy", 1 / dpiy, Unidad.Pulgadas);
            intermedia = new Unidad("UnidadDispositivoY", "dpixy", 2 / (dpix + dpiy), Unidad.Pulgadas);
            g = graficos;
            
            _brochas = new Stock<Brocha, SolidBrush>(delegate(Brocha brocha) {
                return new SolidBrush(ObtenerColor(((BrochaSolida)brocha).Color));
            });
            _lapices = new Stock<Lapiz, Pen>(delegate(Lapiz lapiz)
            {
                Brush b=_brochas.Obtener(lapiz.Brocha);
                Medicion m=lapiz.Ancho=lapiz.Ancho.ConvertirA(intermedia);
                return new Pen(b,(float)m.Valor);
                //return new Pen(;
            });
            _letras=new Stock<Letra,Font>(delegate(Letra letra) {
                Medicion med=letra.Tamaño.ConvertirA(Unidad.Milimetros);
                return new Font(letra.Familia,(float)med.Valor,GraphicsUnit.Millimeter);
            });
        }
        public void DibujarLinea(Lapiz lapiz, Punto inicio, Punto fin)
        {
            Pen l=_lapices.Obtener(lapiz);
            g.DrawLine(l, Traducir(inicio), Traducir(fin));
        }
        public void DibujarRectangulo(Lapiz lapiz, Punto inicio, TamBloque bloque)
        {
            Pen l = _lapices.Obtener(lapiz);
            PointF pt=Traducir(inicio);
            SizeF tam=Traducir(bloque);
            g.DrawRectangle(l, pt.X,pt.Y,tam.Width,tam.Height);
        }
        public void RellenarRectangulo(Brocha brocha, Punto inicio, TamBloque bloque)
        {
            Brush b = _brochas.Obtener(brocha);
            PointF pt = Traducir(inicio);
            SizeF tam = Traducir(bloque);
            g.FillRectangle(b, pt.X, pt.Y, tam.Width, tam.Height);
        }
        public void DibujarTexto(Punto posicion, Letra letra, Brocha brocha, string texto)
        {
            Font f=_letras.Obtener(letra);
            Brush b=_brochas.Obtener(brocha);
            g.DrawString(texto, f, b, Traducir(posicion),FormatoPresentacion);
        }
        static StringFormat FormatoPresentacion;
        static StringFormat FormatoMedicion;
        static Graficador()
        {
            //Formato=StringFormat.GenericTypographic;
            //Formato = new StringFormat(StringFormat.GenericTypographic);
            //Formato.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
            //FormatoPresentacion = StringFormat.GenericDefault;
            FormatoMedicion = new StringFormat(StringFormat.GenericTypographic);
            FormatoMedicion.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
            FormatoPresentacion = FormatoMedicion;
            //Formato.FormatFlags |= StringFormatFlags.DisplayFormatControl;
            //Formato.FormatFlags &= ~StringFormatFlags.FitBlackBox;
            /*Formato.Trimming = StringTrimming.None;
            Formato.FormatFlags = StringFormat.GenericTypographic.FormatFlags;
            Formato.LineAlignment = StringAlignment.Near;
            Formato.Trimming = StringTrimming.None;
            //Formato.DigitSubstitutionMethod = StringDigitSubstitute.None;
            Formato.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
            Formato.FormatFlags |= StringFormatFlags.NoClip;
            Formato.FormatFlags |= StringFormatFlags.NoWrap;*/

            //Formato.FormatFlags |= StringFormatFlags.FitBlackBox;
        }
        public TamBloque MedirTexto(Letra letra, string texto)
        {
            Font f = _letras.Obtener(letra);
            if (texto.Length == 0)
            {
                SizeF tam2 = g.MeasureString("M", f, new PointF(0, 0), FormatoMedicion);
                tam2.Width = 0;
                return Traducir(tam2);
            }
            SizeF tam=g.MeasureString(texto, f, new PointF(0,0), FormatoMedicion);            
            return Traducir(tam);
        }
        public Medicion MedirUnion(Letra letra, string a,string b)
        {
            if (a.Length > 0 && b.Length > 0)
            {
                TamBloque t1=MedirTexto(letra, a[a.Length - 1].ToString() + b[0]);
                TamBloque t2 = MedirTexto(letra, a[a.Length - 1].ToString());
                TamBloque t3 = MedirTexto(letra, b[0].ToString());
                return t1.Ancho-(t2.Ancho + t3.Ancho);
            }
            return Medicion.Cero;
        }
        public void TrasladarOrigen(SistemaWP.IU.PresentacionDocumento.Punto Punto)
        {
            PointF p=Traducir(Punto);
            g.TranslateTransform(p.X, p.Y);

        }
    }
    
}
