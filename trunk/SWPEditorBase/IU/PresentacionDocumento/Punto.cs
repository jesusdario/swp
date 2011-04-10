using System;
using System.Collections.Generic;
using System.Text;
using SWPEditor.Dominio;

namespace SWPEditor.IU.PresentacionDocumento
{
    public struct Punto
    {
        public Medicion X { get; set; }
        public Medicion Y { get; set; }
        public static Punto operator +(Punto a,Punto b) {
            return new Punto(a.X+b.X,a.Y+b.Y);
        }
        public static Punto operator -(Punto a, Punto b)
        {
            return new Punto(a.X - b.X, a.Y - b.Y);
        }
        public Punto AgregarX(Medicion deltaX)
        {
            return new Punto(X + deltaX, Y);
        }
        public Punto AgregarY(Medicion deltaY)
        {
            return new Punto(X, Y + deltaY);
        }
        public Punto Agregar(Medicion deltaX, Medicion deltaY)
        {
            return new Punto(X+deltaX , Y + deltaY);
        }
        public Punto(Medicion x, Medicion y):this()
        {
            X = x; Y = y;
        }
        public static readonly Punto Origen = new Punto(Medicion.Cero, Medicion.Cero);
    }
}
