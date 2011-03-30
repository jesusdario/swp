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
