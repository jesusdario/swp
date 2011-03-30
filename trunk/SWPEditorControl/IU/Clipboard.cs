using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using SWPEditor.IU.PresentacionDocumento;
using SWPEditor.Dominio;

namespace SWPEditor.IU
{
    class DatosClipboard : System.Windows.Forms.IDataObject
    {
        Documento _documento;
        #region Miembros de IDataObject
        public DatosClipboard(Documento doc)
        {
            _documento = doc;
        }
        public object GetData(Type format)
        {
            if (format == typeof(string))
            {
                return _documento.ObtenerTexto();
            }
            return null;
        }

        public object GetData(string format)
        {
            if (format == DataFormats.Html)
            {
                string cad = null;
                cad = _documento.ObtenerHTML();
                string cadini = "<body class='e0'>";
                string cadini2 = "<!--StartFragment-->";
                string cadfin = "</body>";
                string cadfin2 = "<!--EndFragment--></body>";
                string cnueva = cad.Replace(cadini, cadini2).Replace(cadfin, cadfin2);
                string cadbase = @"Version:1.0
                    StartHTML:-1
                    EndHTML:-1
                    StartFragment:AAAAAAAAAA
                    EndFragment:BBBBBBBBBB
                <!DOCUMENT>";
                int indice1 = cnueva.IndexOf(cadini2) + cadbase.Length;
                int indice2 = cnueva.IndexOf(cadfin2) + cadfin2.Length + cadbase.Length;
                cadbase = cadbase
                    .Replace("AAAAAAAAAA", indice1.ToString().PadLeft(10, '0'))
                    .Replace("BBBBBBBBBB", indice2.ToString().PadLeft(10, '0'));
                cnueva = cadbase + cnueva;

                return cnueva;
                //Clipboard.SetText(cad, TextDataFormat.UnicodeText);
            }
            else if (format == DataFormats.Text)
            {
                return _documento.ObtenerTexto();
            }
            return string.Empty;
        }

        public object GetData(string format, bool autoConvert)
        {
            return GetData(format);
        }

        public bool GetDataPresent(Type format)
        {
            return false;
        }

        public bool GetDataPresent(string format)
        {
            return format == DataFormats.Html || format == DataFormats.Text;
        }

        public bool GetDataPresent(string format, bool autoConvert)
        {
            return format == DataFormats.Html || format == DataFormats.Text;
        }

        public string[] GetFormats()
        {
            return new string[] { DataFormats.Html, DataFormats.Text };
        }

        public string[] GetFormats(bool autoConvert)
        {
            return new string[] { DataFormats.Html, DataFormats.Text };
        }

        public void SetData(object data)
        {
            
        }

        public void SetData(Type format, object data)
        {
            
        }

        public void SetData(string format, object data)
        {
            
        }

        public void SetData(string format, bool autoConvert, object data)
        {
            
        }

        #endregion
    }
    class SWPClipboard : SWPEditor.IU.PresentacionDocumento.IClipboard
    {

        #region Miembros de IClipboard

        void IClipboard.Cortar(SWPEditor.Aplicacion.Seleccion seleccion)
        {
            if (seleccion != null)
            {
                
                seleccion.ObtenerDocumento();
            }
        }

        void IClipboard.Copiar(SWPEditor.Aplicacion.Seleccion seleccion)
        {
            throw new NotImplementedException();
        }

        void IClipboard.Pegar(SWPEditor.IU.PresentacionDocumento.ContPresentarDocumento editor)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
