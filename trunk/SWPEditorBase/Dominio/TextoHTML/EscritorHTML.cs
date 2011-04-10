/**********************************************
This Program was written by Dario Romero A.
It is licensed under the Apache License v.2.0.
***********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using SWPEditor.Dominio.TextoFormato;

namespace SWPEditor.Dominio.Html
{
    class EscritorHTML:IEscritor 
    {
        StringBuilder _html = new StringBuilder();
        StringBuilder _estilos=new StringBuilder();
        Dictionary<Formato, int> cadestilos=new Dictionary<Formato,int>();
        Dictionary<Formato, string> estiloslin = new Dictionary<Formato, string>();
        int estiloactual;
        public EscritorHTML()
        {
            

        }
        int posinsestilos;
        #region Miembros de IEscritor
        public void IniciarDocumento()
        {
            _html.AppendLine("<html>");
            _html.AppendLine("<head>");
            posinsestilos = _html.Length;
            _html.AppendLine("</head>");
            Formato f = Formato.ObtenerPredefinido();
            string familiadefecto = f.ObtenerFamiliaLetra();
            _html.AppendLine("<body class='e"+AgregarEstilo(f)+"'>");
        }
        public void TerminarDocumento()
        {
            _html.AppendLine("</body>");
            _html.AppendLine("</html>");
            _html.Insert(posinsestilos, "<style type='text/css'>"+_estilos+"</style>");
        }
        public void IniciarParrafo(FormatoParrafo formato)
        {
            Medicion medicionAnterior  = formato.ObtenerEspacioAnterior();
            Medicion medicionPosterior = formato.ObtenerEspacioPosterior();
            string estilos="";
            estilos += "margin-top:"+medicionAnterior.ConvertirA(Unidad.Puntos)+"pt;";
            estilos += "margin-bottom:" + medicionPosterior.ConvertirA(Unidad.Puntos) + "pt;";
            estilos += "line-spacing:"+formato.ObtenerEspaciadoInterlineal()+"em;";
            _html.Append("<p style='"+estilos+"'>");            
        }
        private int AgregarEstilo(Formato formato)
        {
            _estilos.Append(".e" + estiloactual + " {");   
            string estilo=ObtenerEstilo(formato).ToString();
            _estilos.Append(estilo);
            _estilos.AppendLine("}");
            int numestilo = estiloactual++;
            estiloslin.Add(formato, estilo);
            cadestilos.Add(formato, numestilo);
            return numestilo;
        }
        private StringBuilder ObtenerEstilo(Formato formato)
        {
            StringBuilder st2 = new StringBuilder();
            if (formato.ObtenerNegrilla())
            {
                st2.Append("font-weight:bold;");
            }
            if (formato.ObtenerCursiva())
            {
                st2.Append("font-style:italic;");
            }
            st2.Append("font-family:" + formato.ObtenerFamiliaLetra() + ";");
            if (formato.ObtenerSubrayado())
            {
                st2.Append("text-decoration:underline;");
            }
            st2.Append("font-size:");
            st2.Append(formato.ObtenerTamLetraEscalado().ConvertirA(Unidad.Puntos).Valor.ToString(System.Globalization.CultureInfo.InvariantCulture));
            st2.Append("pt;");
        
            if (formato.ColorLetra.HasValue)
            {
                st2.Append("color:");
                st2.Append(ObtenerColorHTML(formato.ColorLetra.Value));
                st2.Append(";");
            }
            if (formato.ColorFondo.HasValue)
            {
                st2.Append("background-color:");
                st2.Append(ObtenerColorHTML(formato.ColorFondo.Value));
                st2.Append(";");
            }
            return st2;
        }
        private string ObtenerColorHTML(ColorDocumento color)
        {
            return "#" + 
                color.R.ToString("x").PadLeft(2, '0') + 
                color.G.ToString("x").PadLeft(2, '0') + 
                color.B.ToString("x").PadLeft(2, '0');
        }
        public void EscribirTexto(string texto, SWPEditor.Dominio.TextoFormato.Formato formato)
        {
            bool formatoigual = false;
            //if (!formato.Equals(Formato.ObtenerPredefinido()))
            //{
                _html.Append("<span ");
                _html.Append(" style=\"");
                if (estiloslin.ContainsKey(formato))
                    _html.Append(estiloslin[formato]);
                else
                {
                    AgregarEstilo(formato);
                    _html.Append(estiloslin[formato]);
                }
                _html.Append("\"");
                //int numestilo = 0;
                //if (cadestilos.ContainsKey(formato))
                //{
                //    numestilo = cadestilos[formato];
                //}
                //else
                //{
                //    numestilo = AgregarEstilo(formato);
                    
                //}
                //_html.Append(" class=\"e" + numestilo + "\"");
                _html.Append(">");
            //}
            //else
                formatoigual = true;
            _html.Append(texto);
            //if (!formatoigual)
            //{
                _html.Append("</span>");
            //}
        }

        public void TerminarParrafo()
        {
            _html.AppendLine("</p>");
        }

        #endregion

        #region Miembros de IEscritor


        public string ObtenerTexto()
        {
            return _html.ToString();
        }

        public byte[] ObtenerBytes()
        {
            return UTF8Encoding.UTF8.GetBytes(_html.ToString());
        }

        #endregion
    }
}
