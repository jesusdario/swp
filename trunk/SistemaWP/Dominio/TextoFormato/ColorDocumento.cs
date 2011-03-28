using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaWP.Dominio.TextoFormato
{
    public struct ColorDocumento
    {
        public int A { get; set; }
        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }
        public override int GetHashCode()
        {
            return (A << 24) | (R << 16) | (G << 8) | B;
        }
        public override bool Equals(object obj)
        {
            ColorDocumento c=(ColorDocumento)obj;
            return  R == c.R && 
                    G == c.G && 
                    B == c.B && 
                    A == c.A;
        }
        public ColorDocumento(int r, int g, int b):this()
        {
            A = 255;
            R = r;
            G = g;
            B = b;
        }
        public ColorDocumento(int a, int r, int g, int b)
            : this()
        {
            A = a;
            R = r;
            G = g;
            B = b;
        }
        public static readonly ColorDocumento Transparente = new ColorDocumento(0,0, 0, 0);
        public static readonly ColorDocumento Negro = new ColorDocumento(0, 0, 0);
        public static readonly ColorDocumento Rojo = new ColorDocumento(255, 0, 0);
        public static readonly ColorDocumento Amarillo = new ColorDocumento(255, 255, 0);
        public static readonly ColorDocumento Verde = new ColorDocumento(0, 255, 0);
        public static readonly ColorDocumento Azul = new ColorDocumento(0, 0, 255);
        public static readonly ColorDocumento Blanco = new ColorDocumento(255, 255, 255);
    }
}
