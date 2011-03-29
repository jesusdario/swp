using System;
using System.Collections.Generic;
using System.Text;
using SWPEditor.Dominio.TextoFormato;

namespace SWPEditor.Dominio.Html
{
    class EscritorHtml:IEscritor
    {
        StringBuilder _html = new StringBuilder();
        StringBuilder _estilos=new StringBuilder();
        Dictionary<Formato, int> cadestilos=new Dictionary<Formato,int>();
        int estiloactual;
        public EscritorHtml()
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
            _html.AppendLine("<body style='"+ObtenerEstilo(f)+"'>");
        }
        public void TerminarDocumento()
        {
            _html.AppendLine("</body>");
            _html.AppendLine("</html>");
        }
        public void IniciarParrafo()
        {
            _html.Append("<p>");
        }
        private int AgregarEstilo(Formato formato)
        {
            _estilos.Append(".e" + estiloactual + " {");   
            _estilos.Append(ObtenerEstilo(formato));
            _estilos.AppendLine("}");
            return estiloactual++;
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
            if (formato.ObtenerFamiliaLetra() != null)
            {
                st2.Append("font-family:" + formato.ObtenerFamiliaLetra() + ";");
            }
            if (formato.ObtenerSubrayado())
            {
                st2.Append("text-decoration:underline;");
            }
            return st2;
        }
        public void EscribirTexto(string texto, SWPEditor.Dominio.TextoFormato.Formato formato)
        {
            bool formatoigual = false;
            if (!formato.Equals(Formato.ObtenerPredefinido()))
            {
                _html.Append("<span ");
                int numestilo = 0;
                if (cadestilos.ContainsKey(formato))
                {
                    numestilo = cadestilos[formato];
                }
                else
                {
                    numestilo = AgregarEstilo(formato);
                }
                _html.Append(" class='e" + numestilo + "'");
                _html.Append(">");
            }
            else
                formatoigual = true;
            _html.Append(texto);
            if (!formatoigual)
            {
                _html.Append("</span>");
            }
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
