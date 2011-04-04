using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SWPEditor.Tests;
using SWPEditor.IU.VistaDocumento;
using SWPEditor.Dominio;
using SWPEditor.IU.Graficadores;
using System.Drawing;

namespace SWPEditorTests
{
    class Program
    {
        static void Main(string[] args)
        {
            Bitmap bmp=new Bitmap(100,100,System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            
            Escritorio g = new Escritorio(new Documento(),new GraficadorGDI(Graphics.FromImage(bmp)));
            SWPEditor.Tests.PruebaTexto p = new SWPEditor.Tests.PruebaTexto();
            p.ProbarFormato();
            p.ProbarInsercion();
            p.ProbarEliminacion();
            PruebaBloques t = new PruebaBloques();
            t.ProbarBloques();
        }
    }
}
