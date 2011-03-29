﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SWPEditor.Dominio.Texto
{
    class EscritorTexto : IEscritor
    {
        StringBuilder st = new StringBuilder();
        #region Miembros de IEscritor

        public void IniciarDocumento()
        {
            
        }

        public void IniciarParrafo()
        {
            
        }

        public void EscribirTexto(string texto, SWPEditor.Dominio.TextoFormato.Formato formato)
        {
            st.Append(texto);
        }

        public void TerminarParrafo()
        {
            st.AppendLine();
        }

        public void TerminarDocumento()
        {
            
        }

        public string ObtenerTexto()
        {
            return st.ToString();
        }

        public byte[] ObtenerBytes()
        {
            return Encoding.UTF8.GetBytes(st.ToString());
        }

        #endregion
    }
}
