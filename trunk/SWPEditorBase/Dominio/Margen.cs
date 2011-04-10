/**********************************************
This Program was written by Dario Romero A.
It is licensed under the Apache License v.2.0.
***********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using SWPEditor.Dominio;

namespace SWPEditor.Dominio
{
    public class Margen
    {
        public Medicion Derecho { get; set; }
        public Medicion Izquierdo { get; set; }
        public Medicion Superior { get; set; }
        public Medicion Inferior { get; set; }
        public Margen()
            : this(Medicion.Cero)
        {

        }
        public Margen(Medicion valor)
        {
            Derecho = Izquierdo = Superior = Inferior = valor;
        }
    }
}
