using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaWP.Dominio
{
    public class Unidad
    {
        public string Nombre { get; set; }
        public string Abreviatura { get; set; }
        public double FactorConversion { get; set; }
        public Unidad UnidadRelativa { get; set; }
        public Unidad(string nombre, string abreviatura,double factorConversion, Unidad unidadRelativa)
        {
            Nombre = nombre;
            Abreviatura=abreviatura;
            FactorConversion = factorConversion;
            UnidadRelativa = unidadRelativa;
        }
        
        public static readonly Unidad Metros = new Unidad("Metros", "m", 1, null);
        public static readonly Unidad Centimetros = new Unidad("Centimetros", "cm", 0.01, Metros);
        public static readonly Unidad Milimetros = new Unidad("Milímetros", "mm", 0.001, Metros);
        public static readonly Unidad Pulgadas = new Unidad("Pulgadas", "\"", 0.0254,Metros);
    }
}
