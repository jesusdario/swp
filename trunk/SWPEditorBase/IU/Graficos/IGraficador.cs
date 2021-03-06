/**********************************************
This Program was written by Dario Romero A.
It is licensed under the Apache License v.2.0.
***********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using SWPEditor.Dominio;

namespace SWPEditor.IU.Graficos
{
    public interface IGraficador:IDisposable
    {
        void DibujarLinea(SWPEditor.IU.Graficos.Lapiz lapiz, SWPEditor.IU.PresentacionDocumento.Punto inicio, SWPEditor.IU.PresentacionDocumento.Punto fin);
        void DibujarRectangulo(SWPEditor.IU.Graficos.Lapiz lapiz, SWPEditor.IU.PresentacionDocumento.Punto inicio, SWPEditor.Dominio.TamBloque bloque);
        void DibujarTexto(SWPEditor.IU.PresentacionDocumento.Punto posicion, SWPEditor.IU.Graficos.Letra letra, SWPEditor.IU.Graficos.Brocha brocha, string texto);
        SWPEditor.Dominio.TamBloque MedirTexto(SWPEditor.IU.Graficos.Letra letra, string texto);
        void RellenarRectangulo(SWPEditor.IU.Graficos.Brocha brocha, SWPEditor.IU.PresentacionDocumento.Punto inicio, SWPEditor.Dominio.TamBloque bloque);
        Medicion MedirUnion(Letra letra, string a, string b);
        void TrasladarOrigen(SWPEditor.IU.PresentacionDocumento.Punto Punto);
        Medicion MedirBaseTexto(Letra letra);
        Medicion MedirAltoTexto(Letra letra);
        Medicion MedirEspacioLineas(Letra letra);
    }
}
