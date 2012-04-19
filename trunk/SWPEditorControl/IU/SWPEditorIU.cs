/**********************************************
This Program was written by Dario Romero A.
It is licensed under the Apache License v.2.0.
***********************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SWPEditor.Dominio;
using SWPEditor.Aplicacion;
using SWPEditor.IU.PresentacionDocumento;
using SWPEditor.IU.VistaDocumento;
using SWPEditor.IU.Graficos;
using System.Threading;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.IO;
using System.Diagnostics;

namespace SWPEditor.IU
{
    public partial class SWPEditorIU : Form
    {
        
        public SWPEditorIU()
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
            swpEditor1.DocumentChanged += new EventHandler(swpEditor1_DocumentChanged);
        }

        void swpEditor1_DocumentChanged(object sender, EventArgs e)
        {
            DocumentPosition pos=swpEditor1.Position;
            LabelPage.Text = "Page: "+pos.Page;
            LabelLine.Text = "Line: " + pos.Line;
            LabelCharacter.Text = "Character: " + pos.Character;
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

        private void IncreaseSpaceBeforeParagraph_Click(object sender, EventArgs e)
        {
            swpEditor1.IncreaseSpaceBeforeParagraph();
        }

        private void DecreaseSpaceBeforeParagraph_Click(object sender, EventArgs e)
        {
            swpEditor1.DecreaseSpaceBeforeParagraph();
        }

        private void getHTMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string html = swpEditor1.GetHTML();
            SaveFileDialog s = new SaveFileDialog();
            s.DefaultExt = "html";
            if (s.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter st = File.CreateText(s.FileName))
                {
                    st.Write(html);
                }
                Process.Start(s.FileName);
            }
        }
        

    }
}
