/**********************************************
This Program was written by Dario Romero A.
It is licensed under the Apache License v.2.0.
***********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using SWPEditor.Dominio.TextoFormato;

namespace SWPEditor.Dominio
{
    public interface IEscritor
    {
        void IniciarDocumento();
        void IniciarParrafo(FormatoParrafo formato);
        void EscribirTexto(string texto, Formato formato);
        void TerminarParrafo();
        void TerminarDocumento();
        string ObtenerTexto();
        byte[] ObtenerBytes();
    }
}
