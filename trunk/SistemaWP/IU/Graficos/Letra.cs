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
        public bool Negrilla { get; set; }
        public bool Cursiva { get; set; }
        public bool Subrayado { get; set; }
        public Letra()
        {
            Familia = "Arial";
            Tamaño = new Medicion(1.4);
        }
        public override int GetHashCode()
        {
            return (Familia??string.Empty).GetHashCode() ^ Tamaño.GetHashCode()^ 
                (Negrilla?256:0)^(Cursiva?65000:0)^(Subrayado?10000000:0);
        }
        public override bool Equals(object obj)
        {
            Letra l = (Letra)obj;
            return Familia == l.Familia &&
                Negrilla == l.Negrilla &&
                Cursiva == l.Cursiva &&
                Subrayado == l.Subrayado &&
                Tamaño.Equals(l.Tamaño);
        }
    }
}
