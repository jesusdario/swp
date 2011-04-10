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
            else if (format == typeof(Documento))
            {
                return _documento;
            }
            return null;
        }

        public object GetData(string format)
        {
            if (format == "SWPEditor.Document")
            {
                return _documento;
            } else if (format == DataFormats.Html)
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
                int indice1 = cadbase.Length;//cnueva.IndexOf(cadini2) + cadbase.Length;
                int indice2 = cnueva.Length + cadbase.Length;//cnueva.IndexOf(cadfin2) + cadfin2.Length + cadbase.Length;
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
            return format==typeof(Documento)||format==typeof(string);
        }

        public bool GetDataPresent(string format)
        {
            return format == DataFormats.Html || format == DataFormats.Text;
        }

        public bool GetDataPresent(string format, bool autoConvert)
        {
            return format == DataFormats.Html || format == DataFormats.Text||format=="SWPEditor.Document";
        }

        public string[] GetFormats()
        {
            return new string[] { "SWPEditor.Document", DataFormats.Html, DataFormats.Text };
        }

        public string[] GetFormats(bool autoConvert)
        {
            return new string[] { "SWPEditor.Document", DataFormats.Html, DataFormats.Text };
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

                Clipboard.SetDataObject(new DatosClipboard(seleccion.ObtenerDocumento()), false);
            }
        }

        void IClipboard.Copiar(SWPEditor.Aplicacion.Seleccion seleccion)
        {
            if (seleccion != null)
            {
                Clipboard.SetDataObject(new DatosClipboard(seleccion.ObtenerDocumento()), false);
            }
        }

        void IClipboard.Pegar(SWPEditor.IU.PresentacionDocumento.ContPresentarDocumento editor)
        {
            
            IDataObject obj=Clipboard.GetDataObject();
            if (obj != null)
            {
                
                /*if (obj.GetDataPresent("SWPEditor.Document",true))
                {
                    Documento doc=(Documento)obj.GetData("SWPEditor.Document",true);
                    if (doc != null)
                    {
                        editor.Pegar(doc);
                    }
                }
                else*/
                {
                    string cad = (string)obj.GetData(DataFormats.UnicodeText, true);
                    if (cad != null)
                    {
                        editor.InsertarTexto(cad);
                    }
                }
            }
        
        }

        #endregion
    }
}
