using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using SWPEditor.Dominio.TextoFormato;

namespace SWPEditor.Dominio.Datos
{
    public class SWPWriter:IEscritor
    {
        Stream _stDestino;
        XmlWriter esc;
        public SWPWriter(Stream st)
        {
            _stDestino = st;
            esc = XmlWriter.Create(_stDestino);
            idsparrafos = new Dictionary<FormatoParrafo, int>();
        }
        #region Miembros de IEscritor

        public void IniciarDocumento()
        {
            esc.WriteStartElement("document");
        }
        Dictionary<FormatoParrafo, int> idsparrafos;
        public void IniciarParrafo(SWPEditor.Dominio.TextoFormato.FormatoParrafo formato)
        {
            //if (!idsparrafos.ContainsKey(formato))
            //{
                
            //    esc.WriteStartElement("style");
            //    esc.WriteEndElement();
            //}
            //esc.WriteStartElement("p");
        }

        public void EscribirTexto(string texto, SWPEditor.Dominio.TextoFormato.Formato formato)
        {
            
        }

        public void TerminarParrafo()
        {
            esc.WriteEndElement();
        }

        public void TerminarDocumento()
        {
            esc.WriteEndElement();
        }

        public string ObtenerTexto()
        {
            return null;
        }

        public byte[] ObtenerBytes()
        {
            return null;
            //throw new NotImplementedException();
        }

        #endregion
    }
}
