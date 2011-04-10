/**********************************************
This Program was written by Dario Romero A.
It is licensed under the Apache License v.2.0.
***********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using SWPEditor.IU.Graficadores;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Printing;
using SWPEditor.Dominio;
using SWPEditor.IU.PresentacionDocumento;

namespace SWPEditor.IU
{
    class SWPEditorPrinter 
    {
        SWPGenericPrinter _impgenerica;
        int? numpagina;
        public SWPEditorPrinter(Documento document)
        {
            _impgenerica = new SWPGenericPrinter(document);
        }
        public void Print()
        {
            PrintDialog impr = new PrintDialog();
            PrintDocument docimpr = new PrintDocument();
            impr.Document = docimpr;
            impr.UseEXDialog = true;
            if (impr.ShowDialog() == DialogResult.OK)
            {
                docimpr.QueryPageSettings += new QueryPageSettingsEventHandler(docimpr_QueryPageSettings);
                docimpr.OriginAtMargins = true;
                docimpr.PrintPage += new PrintPageEventHandler(impresora_PrintPage);
                docimpr.Print();
            }
        }
        private Unidad CentesimaPulgada = new Unidad("CentPlg", "CP", 0.01, Unidad.Pulgadas);
        void docimpr_QueryPageSettings(object sender, QueryPageSettingsEventArgs e)
        {
            if (!numpagina.HasValue)
            {
                numpagina = 0;
            }
            
            //int lim = escritorio.Controlador.Documento.ObtenerNumPaginas();
            TamBloque tam = _impgenerica.GetNextPageSize();
            
            e.PageSettings.PaperSize = new PaperSize("Personalizado",
                (int)tam.Ancho.ConvertirA(CentesimaPulgada).Valor,
                (int)tam.Alto.ConvertirA(CentesimaPulgada).Valor);
        }
        void impresora_PrintPage(object sender, PrintPageEventArgs e)
        {
            GraficadorGDI g = new GraficadorGDI(e.Graphics);
            //e.Graphics.ResetTransform();
            //e.Graphics.PageUnit = GraphicsUnit.Display;
            g.CambiarResolucion(96, 96);//Utilizar resolucion pantalla
            e.HasMorePages = _impgenerica.PrintNextPage(g);            
        }
    }
}
