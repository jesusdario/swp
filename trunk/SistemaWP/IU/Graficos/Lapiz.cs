using System;
using System.Collections.Generic;
using System.Text;
using SWPEditor.Dominio;

namespace SWPEditor.IU.Graficos
{
    public class Lapiz
    {
        public Brocha Brocha { get; set; }
        public Medicion Ancho { get; set; }
        public Lapiz Duplicar(Brocha brocha)
        {
            return new Lapiz() { Brocha = brocha, Ancho = Ancho};
        }
        public Lapiz Duplicar(Medicion ancho)
        {
            return new Lapiz() { Brocha = Brocha, Ancho = ancho };
        }
        public override int GetHashCode()
        {
            return Brocha.GetHashCode() & Ancho.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            Lapiz l = (Lapiz)obj;
            return Ancho.Equals(l.Ancho) && Brocha.Equals(l.Brocha);
        }
        public static readonly Lapiz Negro = new Lapiz() { Brocha = BrochaSolida.Negro, Ancho = new Medicion(0.01) };
        public static readonly Lapiz Rojo = new Lapiz() { Brocha = BrochaSolida.Rojo, Ancho = new Medicion(0.01) };
        public static readonly Lapiz Amarillo = new Lapiz() { Brocha = BrochaSolida.Amarillo, Ancho = new Medicion(0.01) };
        public static readonly Lapiz Verde = new Lapiz() { Brocha = BrochaSolida.Verde, Ancho = new Medicion(0.01) };
        public static readonly Lapiz Azul = new Lapiz() { Brocha = BrochaSolida.Azul, Ancho = new Medicion(0.01) };
        public static readonly Lapiz Blanco = new Lapiz() { Brocha = BrochaSolida.Blanco, Ancho = new Medicion(0.01) };
    }
}
