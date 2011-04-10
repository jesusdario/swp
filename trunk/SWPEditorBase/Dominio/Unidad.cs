/**********************************************
This Program was written by Dario Romero A.
It is licensed under the Apache License v.2.0.
***********************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace SWPEditor.Dominio
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
        public static readonly Unidad Puntos = new Unidad("Puntos", "\"", 1/72.0, Pulgadas);
        public static readonly Unidad Documento = new Unidad("Documento", "doc", 1 / 300.0, Pulgadas);
        public static readonly Unidad Pantalla = new Unidad("Pantalla", "pg", 1 / 96.0, Pulgadas);
    }
}
