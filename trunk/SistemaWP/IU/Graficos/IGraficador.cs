using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SistemaWP.Dominio;

namespace SistemaWP.IU.Graficos
{
    public interface IGraficador
    {
        void DibujarLinea(SistemaWP.IU.Graficos.Lapiz lapiz, SistemaWP.IU.PresentacionDocumento.Punto inicio, SistemaWP.IU.PresentacionDocumento.Punto fin);
        void DibujarRectangulo(SistemaWP.IU.Graficos.Lapiz lapiz, SistemaWP.IU.PresentacionDocumento.Punto inicio, SistemaWP.IU.PresentacionDocumento.TamBloque bloque);
        void DibujarTexto(SistemaWP.IU.PresentacionDocumento.Punto posicion, SistemaWP.IU.Graficos.Letra letra, SistemaWP.IU.Graficos.Brocha brocha, string texto);
        SistemaWP.IU.PresentacionDocumento.TamBloque MedirTexto(SistemaWP.IU.Graficos.Letra letra, string texto);
        void RellenarRectangulo(SistemaWP.IU.Graficos.Brocha brocha, SistemaWP.IU.PresentacionDocumento.Punto inicio, SistemaWP.IU.PresentacionDocumento.TamBloque bloque);
        Medicion MedirUnion(Letra letra, string a, string b);
        void TrasladarOrigen(SistemaWP.IU.PresentacionDocumento.Punto Punto); 
    }
}
