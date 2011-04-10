using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using SWPEditor.IU.PresentacionDocumento;
using SWPEditor.IU.VistaDocumento;
using SWPEditor.Dominio;
using System.Threading;
using System.Drawing.Printing;
using SWPEditor.Aplicacion;
using SWPEditor.IU.Graficos;
using SWPEditor.IU.Graficadores;

namespace SWPEditor.IU
{
    public partial class SWPEditorControl : Control
    {
        SWPGenericControl _base;
        SWPClipboard _clipboard;
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                _base.BackgroundColor = new SWPEditor.Dominio.TextoFormato.ColorDocumento(value.A, value.R, value.G, value.B);
                base.BackColor = value;
            }
        }
        public SWPEditorControl()
            : base()
        {
            DoubleBuffered = true;
            
            Documento _documento = new Documento();
            _clipboard = new SWPClipboard();
            _base = new SWPGenericControl(GraficadorGDI.ObtenerGraficadorConsultas());
            this.SetStyle(ControlStyles.Selectable, true);
            Enabled = true;
            Visible = true;
            _base.PrintRequested += new EventHandler(_base_PrintRequested);
            _base.RefreshRequested += new EventHandler(_base_RefreshRequested);
        }
        public event EventHandler DocumentChanged
        {
            add
            {
                _base.DocumentChanged += value;
            }
            remove
            {
                _base.DocumentChanged -= value;
            }
        }
        void _base_RefreshRequested(object sender, EventArgs e)
        {
            Invalidate(true);
        }

        void _base_PrintRequested(object sender, EventArgs e)
        {
            SWPEditorPrinter imp = new SWPEditorPrinter(_base.GetDocument());
            imp.Print();
        }
        protected override bool IsInputChar(char charCode)
        {
            return true;
        }
        protected override bool IsInputKey(Keys keyData)
        {
            return true;
        }
        protected override void OnResize(EventArgs e)
        {
            using (Graphics g = Graphics.FromHwnd(Handle))
            {
                _base.NotifySizeChanged(new GraficadorGDI(g).Traducir(new SizeF(Width, Height)));
            }
            Invalidate();
            base.OnResize(e);
        }
        
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            _base.NotifyCharacterPressed(e.KeyChar);
            e.Handled = true;
            base.OnKeyPress(e);
        }
        //private Unidad CentesimaPulgada = new Unidad("CentPlg", "CP", 0.01, Unidad.Pulgadas);
        public void Print()
        {
            _base.Print();
        }
        static Dictionary<Keys, SWPGenericControl.Key> _traduccion = new Dictionary<Keys, SWPGenericControl.Key>();
        static Dictionary<Keys, SWPGenericControl.Key> _traduccioncontrol = new Dictionary<Keys, SWPGenericControl.Key>();
        static SWPEditorControl()
        {
            _traduccioncontrol.Add(Keys.X, SWPGenericControl.Key.Cut);
            _traduccioncontrol.Add(Keys.C, SWPGenericControl.Key.Copy);
            _traduccioncontrol.Add(Keys.V, SWPGenericControl.Key.Paste);
            _traduccioncontrol.Add(Keys.E, SWPGenericControl.Key.SelectAll);
            _traduccioncontrol.Add(Keys.Up, SWPGenericControl.Key.ParagraphPrevious);
            _traduccioncontrol.Add(Keys.Down, SWPGenericControl.Key.ParagraphNext);
            _traduccioncontrol.Add(Keys.Home, SWPGenericControl.Key.DocumentHome);
            _traduccioncontrol.Add(Keys.End, SWPGenericControl.Key.DocumentEnd);
            _traduccioncontrol.Add(Keys.Left, SWPGenericControl.Key.PreviousWord);
            _traduccioncontrol.Add(Keys.Right, SWPGenericControl.Key.NextWord);
            _traduccion.Add(Keys.Up, SWPGenericControl.Key.Up);
            _traduccion.Add(Keys.Down, SWPGenericControl.Key.Down);
            _traduccion.Add(Keys.Left, SWPGenericControl.Key.Left);
            _traduccion.Add(Keys.Right, SWPGenericControl.Key.Right);
            _traduccion.Add(Keys.PageDown, SWPGenericControl.Key.PageDown);
            _traduccion.Add(Keys.PageUp, SWPGenericControl.Key.PageUp);
            _traduccion.Add(Keys.Back, SWPGenericControl.Key.BackSpace);
            _traduccion.Add(Keys.Enter, SWPGenericControl.Key.Enter);
            _traduccion.Add(Keys.Delete, SWPGenericControl.Key.Delete);
            _traduccion.Add(Keys.Home, SWPGenericControl.Key.Home);
            _traduccion.Add(Keys.End, SWPGenericControl.Key.End);
        }
        
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Control&&_traduccioncontrol.ContainsKey(e.KeyCode))
            {
                _base.NotifyKeyDown(_traduccioncontrol[e.KeyCode],e.Shift,_clipboard);
                e.Handled = true;
                e.SuppressKeyPress = true;                
            } else if (_traduccion.ContainsKey(e.KeyCode))
            {
                _base.NotifyKeyDown(_traduccion[e.KeyCode], e.Shift, _clipboard);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
            base.OnKeyDown(e);
        }
        
        public void InsertText(string text)
        {
            _base.InsertText(text);
        }
        public string GetText() {
            return _base.GetText();
        }
        public void SetText(string value) {
            _base.SetText(value);
        }
        public void SelectAll()
        {
            _base.SelectAll();
        }

        public void GotoDown(bool select)
        {
            _base.GotoDown(select);
        }

        public void GotoUp(bool select)
        {
            _base.GotoUp(select); 
        }

        public void InsertParagraph(bool select)
        {
            _base.InsertParagraph(select);
        }

        public void GotoPreviousWord(bool select)
        {
            _base.GotoPreviousWord(select);
        }

        public void GotoNextWord(bool select)
        {
            _base.GotoNextWord(select);
        }
        public void GotoRight(bool select)
        {
            _base.GotoRight(select);
        }

        public void GotoLeft(bool select)
        {
            _base.GotoLeft(select);
        }

        public void GotoLineEnd(bool select)
        {
            _base.GotoLineEnd(select);
        }

        public void GotoLineStart(bool select)
        {
            _base.GotoLineStart(select);
        }

        public void GotoNextPage(bool select)
        {
            _base.GotoNextPage(select);
        }
        public void GotoPreviousPage(bool select)
        {
            _base.GotoPreviousPage(select);
        }
        public void DeletePreviousCharacter()
        {
            _base.DeletePreviousCharacter();
        }
        public void DeleteCharacter()
        {
            _base.DeleteCharacter();
        }
        public void Cut()
        {
            _base.Cut(_clipboard);
        }
        public void Copy()
        {
            _base.Copy(_clipboard);
        }

        public void Paste()
        {
            _base.Paste(_clipboard);
        }
        
        const int deltax = 0;
        const int deltay = 0;
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            //base.OnPaintBackground(pevent);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            try
            {
                using (IGraficador graf = new GraficadorGDI(e.Graphics))
                {
                    _base.DrawDesktop(graf, true, true);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message +
                    "\r\n" +
                    ex.StackTrace);
            }            
        }
        
        bool EnCaptura = false;
        /*
        protected void RegistrarPosicionSinc(float x, float y, bool ampliarSeleccion)
        {
            using (Graphics g = Graphics.FromHwnd(this.Handle))
            {
                float posx = x - deltax;
                float posy = y - deltay;
                GraficadorGDI graf = new GraficadorGDI(g);
                _baa.IrAPosicion(graf.Traducir(new PointF(posx, posy)), ampliarSeleccion);
            }
        }*/
        private Punto TranslatePoint(PointF punto)
        {
            using (Graphics g = Graphics.FromHwnd(Handle))
            {
                using (GraficadorGDI gr = new GraficadorGDI(g))
                {
                    return gr.Traducir(punto);
                }
            }
        }
        
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (!Focused)
            {
                Select();
            }
            if (!EnCaptura)
            {
                _base.NotifyMouseDown(TranslatePoint(new PointF(e.X, e.Y)), (ModifierKeys & Keys.Shift) != 0);
                Capture = true;
                EnCaptura = true;
            } 
            base.OnMouseDown(e);
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (EnCaptura)
            {
                _base.NotifyMouseUp(TranslatePoint(new PointF(e.X, e.Y)));
                Capture = false;
                EnCaptura = false;
                
            }
            base.OnMouseUp(e);
        }
       
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (EnCaptura)
            {
                _base.NotifyMouseMove(TranslatePoint(new PointF(e.X, e.Y)));
            }
            base.OnMouseMove(e);
        }

        public void ChangeFontColor(System.Drawing.Color color)
        {
            _base.ChangeFontColor(new SWPEditor.Dominio.TextoFormato.ColorDocumento(color.A, color.R, color.G, color.B));
            
        }
        public void ChangeFontBackground(System.Drawing.Color color)
        {
            _base.ChangeFontBackground(new SWPEditor.Dominio.TextoFormato.ColorDocumento(color.A, color.R, color.G, color.B));
        }
        public void ChangeFontBold()
        {
            _base.ChangeFontBold();
        }

        public void ChangeFontItalic()
        {
            _base.ChangeFontItalic();
        }

        public void ChangeFontUnderlined()
        {
            _base.ChangeFontUnderlined();
        }

        public void IncreaseFontSize()
        {
            _base.IncreaseFontSize();
        }

        public void ReduceFontSize()
        {
            _base.ReduceFontSize();
        }

        internal void ChangeFont(Font font)
        {
            _base.ChangeFont(font.FontFamily.Name,new Medicion(font.SizeInPoints,Unidad.Puntos));
        }

        internal void SetFontSizeInPoints(decimal valor)
        {
            _base.SetFontSizeInPoints(valor);
        }

        internal void AlignLeft()
        {
            _base.AlignLeft();
        }

        internal void AlignCenter()
        {
            _base.AlignCenter();
        }

        internal void AlignRight()
        {
            _base.AlignRight();
        }

        internal void IncreaseLineSpace()
        {
            _base.IncreaseLineSpace();
        }

        internal void DecreaseLineSpace()
        {
            _base.DecreaseLineSpace();
        }
        public DocumentPosition Position {
            get {
                return _base.GetPosition();
            }
        }

        internal void IncreaseSpaceBeforeParagraph()
        {
            _base.AddSpaceBeforeParagraph();
        }

        internal void DecreaseSpaceBeforeParagraph()
        {
            _base.RemoveSpaceBeforeParagraph();
        }
    }
    
}
