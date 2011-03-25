using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaWP.IU.Graficos
{
    public class Brocha
    {
    }
    public class BrochaSolida : Brocha
    {
        public ColorDocumento Color { get; set; }
        public override int GetHashCode()
        {
            return Color.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            BrochaSolida b = (BrochaSolida)obj;
            return Color.Equals(b.Color);
        }
        public static readonly BrochaSolida Negro = new BrochaSolida() { Color = ColorDocumento.Negro };
        public static readonly BrochaSolida Rojo = new BrochaSolida() { Color = ColorDocumento.Rojo };
        public static readonly BrochaSolida Amarillo = new BrochaSolida() { Color = ColorDocumento.Amarillo };
        public static readonly BrochaSolida Verde = new BrochaSolida() { Color = ColorDocumento.Verde };
        public static readonly BrochaSolida Azul = new BrochaSolida() { Color = ColorDocumento.Azul };
        public static readonly BrochaSolida Blanco = new BrochaSolida() { Color = ColorDocumento.Blanco };
    }
}
