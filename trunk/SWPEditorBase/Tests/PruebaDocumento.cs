using System;
using System.Collections.Generic;
using System.Text;
using SWPEditor.Dominio;
using System.Diagnostics;

namespace SWPEditor.Tests
{
    class PruebaDocumento
    {
        public static void ProbarObtenerParrafo()
        {
            Documento d = new Documento();
            Parrafo p=d.ObtenerParrafo(1);
            Debug.Assert(p != null);
        }
        public static void ProbarInsertarParrafoMedio()
        {
            Documento d = new Documento();
            Parrafo p = d.ObtenerParrafo(1);
            p.AgregarCadena("Esta es una prueba");

            d.InsertarParrafo(p, 5);
            //cursor antes de caracter
            Debug.Assert(p.ToString() == "Esta ");
            Debug.Assert(p.Siguiente.ToString() == "es una prueba");
            Debug.Assert(p.Siguiente.Anterior == p);
            
        }
        public static void ProbarInsertarParrafoInicio()
        {
            Documento d = new Documento();
            Parrafo p = d.ObtenerParrafo(1);
            p.AgregarCadena("Esta es una prueba");

            d.InsertarParrafo(p, 0);
            Debug.Assert(p.ToString() == "Esta es una prueba");
            Debug.Assert(p.Anterior.ToString() == "");
            
        }
        public static void ProbarInsertarParrafoFin()
        {
            Documento d = new Documento();
            Parrafo p = d.ObtenerParrafo(1);
            p.AgregarCadena("Esta es una prueba");

            d.InsertarParrafo(p, p.Longitud);
            Debug.Assert(p.ToString() == "Esta es una prueba");
            Debug.Assert(p.Siguiente.ToString() == "");
        }
    }
}
