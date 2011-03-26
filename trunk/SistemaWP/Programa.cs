using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SistemaWP.Tests;
using SistemaWP.IU;
using System.Windows.Forms;

namespace SistemaWP
{
    class Programa
    {
        //STAThread es para windows forms.
        [STAThread]
        public static void Main(string[] args)
        {
            
            try
            {
                PresentadorDocumento doc = new PresentadorDocumento();
                doc.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message+"\r\n"+ex.StackTrace, "Advertencia");
            }
            return;
            
        }
        public static void Tests()
        {
            try
            {
                PruebaParrafo.PruebaAgregarCaracter();
                PruebaParrafo.PruebaDividirParrafo();
                PruebaDocumento.ProbarObtenerParrafo();
                PruebaDocumento.ProbarInsertarParrafoInicio();
                PruebaDocumento.ProbarInsertarParrafoMedio();
                PruebaDocumento.ProbarInsertarParrafoFin();
                Console.WriteLine("PRUEBAS EXITOSAS");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ":" + ex.StackTrace);
            }
            return;

        }
    }
}

