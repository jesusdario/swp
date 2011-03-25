using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SistemaWP.Dominio;
using SistemaWP.Aplicacion;
using SistemaWP.IU.PresentacionDocumento;
using SistemaWP.IU.VistaDocumento;
using SistemaWP.IU.Graficos;
using System.Threading;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;

namespace SistemaWP.IU
{
    public partial class PresentadorDocumento : Form
    {
        public PresentadorDocumento()
        {
            InitializeComponent();
        }

        private void cortarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            swpEditor1.Cut();
        }

        private void copiarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            swpEditor1.Copy();
        }

        private void pegarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            swpEditor1.Paste();
        }

        private void imprimirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            swpEditor1.Print();
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            swpEditor1.SelectAll();
        }

        private void PresentadorDocumento_Load(object sender, EventArgs e)
        {
            swpEditor1.Select();
        }
    }
    class PresentadorCursor
    {
        public IGraficador Graficador { get; set; }
        public Punto Inicio { get; set; }
        public Punto Fin { get; set; }
        public bool Visible {get;set;}
        public PresentadorCursor(IGraficador graficador)
        {
            Thread t = new Thread(Ejecutar);
            t.IsBackground = true;
            t.Start();
        }
        public void Ejecutar()
        {
            //Lapiz l = new Lapiz() { Ancho = new Medicion(0.5, Unidad.Milimetros), Brocha = new BrochaSolida() { Color = new ColorDocumento(127, 0, 0) } };            //Graficador.DibujarLinea(l, Inicio, Fin);
            //Graphics g = new Graphics();
            //g.
        }
    }
}
