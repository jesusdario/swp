using System;
using System.Collections.Generic;
using System.Text;
using SistemaWP.Dominio;

namespace SistemaWP.IU.Graficos
{
    public class Letra
    {
        public string Familia { get; set; }
        public Medicion Tamaño { get; set; }
        public Letra()
        {
            Familia = "Arial";
            Tamaño = new Medicion(1.4);
        }
        public override int GetHashCode()
        {
            return Familia.GetHashCode() ^ Tamaño.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            Letra l = (Letra)obj;
            return Familia == l.Familia && 
                Tamaño.Valor==l.Tamaño.Valor &&
                Tamaño.Unidad==l.Tamaño.Unidad;
        }
    }
}
