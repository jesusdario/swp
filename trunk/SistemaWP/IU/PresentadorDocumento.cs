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

        private void negrillaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            swpEditor1.ChangeFontBold();
        }

        private void cursivaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            swpEditor1.ChangeFontItalic();
        }

        private void subrayadoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            swpEditor1.ChangeFontUnderlined();
        }

        private void enlargeFontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            swpEditor1.IncreaseFontSize();
        }

        private void reduceFontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            swpEditor1.ReduceFontSize();
        }

        private void changeFontColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog c = new ColorDialog();
            c.AnyColor = true;
            c.FullOpen = true;
            if (c.ShowDialog() == DialogResult.OK)
            {
                swpEditor1.ChangeFontColor(c.Color);
            }
        }

        private void changeFontBackgroundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowFontBackgroundColorDialog();
        }

        
        void ShowFontDialog()
        {
            FontDialog f = new FontDialog();
            f.ShowColor = true;
            f.ShowEffects = true;
            f.FontMustExist = true;
            f.AllowVerticalFonts = false;
            f.AllowVectorFonts = false;
            
            f.Font = new Font("Arial", 11);
            f.AllowScriptChange = false;
            f.Color = Color.Black;
            if (f.ShowDialog() == DialogResult.OK)
            {
                swpEditor1.ChangeFont(f.Font);
                swpEditor1.ChangeFontColor(f.Color);
            }
        }
        private void changeFontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowFontDialog();
        }

        private void IncreaseFontSize_Click(object sender, EventArgs e)
        {
            swpEditor1.IncreaseFontSize();
        }

        private void ReduceFontSize_Click(object sender, EventArgs e)
        {
            swpEditor1.ReduceFontSize();
        }

        private void Bold_Click(object sender, EventArgs e)
        {
            swpEditor1.ChangeFontBold();
        }

        private void Italic_Click(object sender, EventArgs e)
        {
            swpEditor1.ChangeFontItalic();
        }

        private void Underline_Click(object sender, EventArgs e)
        {
            swpEditor1.ChangeFontUnderlined();
        }

        private void FontSelection_Click(object sender, EventArgs e)
        {
            ShowFontDialog();
        }
        private void FontSizeList_TextChanged(object sender, EventArgs e)
        {
            try
            {
                decimal valor;
                if (string.IsNullOrEmpty(FontSizeList.SelectedItem.ToString()))
                {
                    valor = decimal.Parse(FontSizeList.SelectedText);
                }
                else
                {
                    valor = decimal.Parse(FontSizeList.SelectedItem.ToString());
                }
                swpEditor1.SetFontSizeInPoints(valor);
            }
            catch { }
            swpEditor1.Select();
        }
        

        private void AlignLeft_Click(object sender, EventArgs e)
        {
            swpEditor1.AlignLeft();
        }

        private void AlignCenter_Click(object sender, EventArgs e)
        {
            swpEditor1.AlignCenter();
        }

        private void AlignRight_Click(object sender, EventArgs e)
        {
            swpEditor1.AlignRight();
        }

        private void FontColor_Click(object sender, EventArgs e)
        {
            ShowFontColorDialog();
        }

        private void ShowFontColorDialog()
        {
            ColorDialog c = new ColorDialog();
            c.AnyColor = true;
            c.FullOpen = true;
            if (c.ShowDialog() == DialogResult.OK)
            {
                swpEditor1.ChangeFontColor(c.Color);
            }
        }
        private void ShowFontBackgroundColorDialog()
        {
            ColorDialog c = new ColorDialog();
            c.AnyColor = true;
            c.FullOpen = true;
            if (c.ShowDialog() == DialogResult.OK)
            {
                swpEditor1.ChangeFontBackground(c.Color);
            }
        }

        private void BackgroundColor_Click(object sender, EventArgs e)
        {
            ShowFontBackgroundColorDialog();
        }

        private void IncreaseLineSpace_Click(object sender, EventArgs e)
        {
            swpEditor1.IncreaseLineSpace();
        }

        private void DecreaseLineSpace_Click(object sender, EventArgs e)
        {
            swpEditor1.DecreaseLineSpace();
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
