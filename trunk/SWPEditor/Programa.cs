using System;
using System.Collections.Generic;
using System.Text;
using SWPEditor.Tests;
using SWPEditor.IU;
using System.Windows.Forms;
using SWPEditor.Dominio;

namespace SWPEditor
{
    
    class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            
            try
            {
                SWPEditorIU doc = new SWPEditorIU();
                doc.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message+"\r\n"+ex.StackTrace, "Advertencia");
            }
            return;
            
        }
        //public static void Tests()
        //{
        //    try
        //    {
        //        PruebaParrafo.PruebaAgregarCaracter();
        //        PruebaParrafo.PruebaDividirParrafo();
        //        PruebaDocumento.ProbarObtenerParrafo();
        //        PruebaDocumento.ProbarInsertarParrafoInicio();
        //        PruebaDocumento.ProbarInsertarParrafoMedio();
        //        PruebaDocumento.ProbarInsertarParrafoFin();
        //        Console.WriteLine("PRUEBAS EXITOSAS");
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message + ":" + ex.StackTrace);
        //    }
        //    return;

        //}
    }
}

