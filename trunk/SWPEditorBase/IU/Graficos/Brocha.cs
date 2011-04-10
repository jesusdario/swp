using System;
using System.Collections.Generic;
using System.Text;
using SWPEditor.Dominio.TextoFormato;

namespace SWPEditor.IU.Graficos
{
    public class Brocha
    {
    }
    public class BrochaSolida : Brocha
    {
        public ColorDocumento Color { get; private set; }
        public BrochaSolida(ColorDocumento color)
        {
            Color = color;
        }
 
        public override int GetHashCode()
        {
            return Color.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            BrochaSolida b = (BrochaSolida)obj;
            return Color.Equals(b.Color);
        }
        public static readonly BrochaSolida Transparente = new BrochaSolida(new ColorDocumento(0,0,0,0));
        public static readonly BrochaSolida Negro = new BrochaSolida(ColorDocumento.Negro);
        public static readonly BrochaSolida Rojo = new BrochaSolida(ColorDocumento.Rojo);
        public static readonly BrochaSolida Amarillo = new BrochaSolida(ColorDocumento.Amarillo);
        public static readonly BrochaSolida Verde = new BrochaSolida(ColorDocumento.Verde);
        public static readonly BrochaSolida Azul = new BrochaSolida(ColorDocumento.Azul);
        public static readonly BrochaSolida Blanco = new BrochaSolida(ColorDocumento.Blanco);
    }
}
