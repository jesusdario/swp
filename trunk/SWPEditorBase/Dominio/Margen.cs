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
