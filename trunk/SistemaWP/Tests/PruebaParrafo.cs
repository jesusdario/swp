using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SistemaWP.Dominio;
using System.Diagnostics;

namespace SistemaWP.Tests
{
    class PruebaParrafo
    {
        public static void PruebaAgregarCaracter()
        {
            Documento doc = new Documento();
            Parrafo p = doc.ObtenerParrafo(1);
            p.AgregarCaracter(0, 'a');
            Debug.Assert(p.ToString() == "a");
            p.AgregarCaracter(0, 'h');
            Debug.Assert(p.ToString() == "ha");
            p.AgregarCaracter(1, 'o');
            Debug.Assert(p.ToString() == "hoa");
            p.AgregarCaracter(2, 'l');
            Debug.Assert(p.ToString() == "hola");
        }
        public static void PruebaBorrarCaracter()
        {
            Documento doc = new Documento();
            Parrafo p = doc.ObtenerParrafo(1);
            p.AgregarCaracter(0, 'a');
            Debug.Assert(p.ToString() == "a");
            p.AgregarCaracter(0, 'h');
            p.BorrarCaracter(0);
            Debug.Assert("h" == p.ToString());
            p.BorrarCaracter(0);
            Debug.Assert("" == p.ToString());
            p.BorrarCaracter(0);
        }
        public static void PruebaDividirParrafo()
        {
            Documento doc = new Documento();
            Parrafo p = doc.ObtenerParrafo(1);
            p.AgregarCaracter(0, 'h');
            p.AgregarCaracter(1, 'o');
            p.AgregarCaracter(2, 'l');
            p.AgregarCaracter(3, 'a');
            Parrafo nuevo=p.DividirParrafo(2, 2);
            Debug.Assert(nuevo.ToString() == "la");
            Debug.Assert(p.ToString() == "ho");
            Debug.Assert(p.Anterior == null);
            Debug.Assert(nuevo.Anterior == p);
            Debug.Assert(nuevo.Siguiente == null);
        }
        public static void PruebaDividirParrafo2()
        {
            Documento doc = new Documento();
            Parrafo p = doc.ObtenerParrafo(1);
            p.AgregarCaracter(0, 'h');
            p.AgregarCaracter(1, 'o');
            p.AgregarCaracter(2, 'l');
            p.AgregarCaracter(3, 'a');
            Parrafo nuevo = p.DividirParrafo(2, 0);
            Debug.Assert(nuevo.ToString() == "hola");
            Debug.Assert(p.ToString() == "");
            Debug.Assert(p.Anterior == null);
            Debug.Assert(nuevo.Anterior == p);
            Debug.Assert(nuevo.Siguiente == null);
        }
        public static void PruebaDividirParrafo3()
        {
            Documento doc = new Documento();
            Parrafo p = doc.ObtenerParrafo(1);
            p.AgregarCaracter(0, 'h');
            p.AgregarCaracter(1, 'o');
            p.AgregarCaracter(2, 'l');
            p.AgregarCaracter(3, 'a');
            Parrafo nuevo = p.DividirParrafo(2, 4);
            Debug.Assert(nuevo.ToString() == "");
            Debug.Assert(p.ToString() == "hola");
            Debug.Assert(p.Anterior == null);
            Debug.Assert(nuevo.Anterior == p);
            Debug.Assert(nuevo.Siguiente == null);
        }
    }
}

