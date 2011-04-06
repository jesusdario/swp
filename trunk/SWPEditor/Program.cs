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
                MessageBox.Show(ex.Message+"\r\n"+ex.StackTrace, "Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            return;
            
        }
    }
}

