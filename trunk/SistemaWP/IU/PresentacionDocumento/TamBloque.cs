using System;
using System.Collections.Generic;
using System.Text;
using SistemaWP.Dominio;

namespace SistemaWP.IU.PresentacionDocumento
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
