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
    public struct TamBloque
    {
        public TamBloque(Medicion ancho, Medicion alto):this()
        {
            Ancho = ancho;
            Alto = alto;
        }
        public Medicion Ancho { get; set; }
        public Medicion Alto { get; set; }
    }
}
