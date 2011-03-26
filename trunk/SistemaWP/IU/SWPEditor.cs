using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using SistemaWP.IU.PresentacionDocumento;
using SistemaWP.IU.VistaDocumento;
using SistemaWP.Dominio;
using System.Threading;
using System.Drawing.Printing;
using SistemaWP.Aplicacion;
using SistemaWP.IU.Graficos;

namespace SistemaWP.IU
{
    public partial class SWPEditor : Control
    {
        ContPresentarDocumento contpresentacion = new ContPresentarDocumento();
        Escritorio escritorio;
        public SWPEditor():base()
        {
            DoubleBuffered = true;
            escritorio = new Escritorio(contpresentacion);
            contpresentacion.ActualizarPresentacion += new EventHandler(contpresentacion_ActualizarPresentacion);
            escritorio.Dimensiones = new TamBloque(new Medicion(50, Unidad.Milimetros), new Medicion(50, Unidad.Milimetros));
            this.SetStyle(ControlStyles.Selectable, true);
            Enabled = true;
            Visible = true;
        }

        void contpresentacion_ActualizarPresentacion(object sender, EventArgs e)
        {
            Refresh();
        }
        protected override bool IsInputChar(char charCode)
        {
            return true;
        }
        protected override void OnResize(EventArgs e)
        {
            using (Graphics graf = Graphics.FromHwnd(Handle))
            {
                Graficador g = new Graficador(graf);
                escritorio.Dimensiones = g.Traducir(new SizeF(Width, Height));
            }
            base.OnResize(e);
        }
        
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            contpresentacion.TeclaPulsada(e.KeyChar);
            e.Handled = true;
            base.OnKeyPress(e);
        }
        
        private Unidad CentesimaPulgada = new Unidad("CentPlg", "CP", 0.01, Unidad.Pulgadas);
        public void Print()
        {
            PrintDialog impr = new PrintDialog();
            impr.UseEXDialog = true;
            if (impr.ShowDialog(this) == DialogResult.OK)
            {
                PrintDocument docimpr = new PrintDocument();
                docimpr.PrinterSettings = impr.PrinterSettings;
                docimpr.DefaultPageSettings.PaperSize = new PaperSize("DEF",
                    (int)new Medicion(210,Unidad.Milimetros).ConvertirA(CentesimaPulgada).Valor,
                    (int)new Medicion(270, Unidad.Milimetros).ConvertirA(CentesimaPulgada).Valor);
                docimpr.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
                docimpr.OriginAtMargins = true;
                docimpr.PrintPage += new PrintPageEventHandler(impresora_PrintPage);
                numpagina = null;
                docimpr.Print();
            }
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.X:
                    if (e.Control) //CTRL+V=PEGAR
                    {
                        Cut();
                        e.Handled = true;
                    }
                    break;
                case Keys.V:
                    if (e.Control) //CTRL+V=PEGAR
                    {
                        Paste();
                        e.Handled = true;
                    }
                    break;
                case Keys.C: //CTRL+C=COPIAR
                    if (e.Control)
                    {
                        Copy();
                        e.Handled = true;
                    }
                    break;
                case Keys.P: //CTRL+P=IMPRIMIR
                    if (e.Control)
                    {
                        Print();
                    }
                    break;
                case Keys.E://CTRL+E=SELECCIONAR TODO
                    if (e.Control)
                    {
                        SelectAll();
                        e.Handled = true;
                    }
                    break;
                case Keys.Delete:
                    DeleteCharacter();
                    e.Handled = true;
                    break;
                case Keys.Back:
                    DeletePreviousCharacter();
                    e.Handled = true;
                    break;
                case Keys.PageUp:
                    GotoPreviousPage(e.Shift);
                    break;
                case Keys.PageDown:
                    GotoNextPage(e.Shift); ;
                    break;
                case Keys.Home:
                    GotoLineStart(e.Shift);
                    e.Handled = true;
                    break;
                case Keys.End:
                    GotoLineEnd(e.Shift);
                    break;
                case Keys.Left:
                    GotoLeft(e.Shift, e.Control);
                    e.Handled = true;
                    break;
                case Keys.Right:
                    GotoRight(e.Shift, e.Control);
                    e.Handled = true;
                    break;
                case Keys.Enter:
                    InsertParagraph(e.Shift);
                    e.Handled = true;
                    break;
                case Keys.Up:
                    GotoUp(e.Shift);
                    e.Handled = true;
                    break;
                case Keys.Down:
                    GotoDown(e.Shift);
                    e.Handled = true;
                    break;

            }
            base.OnKeyDown(e);
        }
        
        public void InsertText(string text)
        {
            contpresentacion.InsertarTexto(text);
        }
        public override string Text
        {
            get
            {
                return contpresentacion.ObtenerTexto();
            }
            set
            {
                contpresentacion.SeleccionarTodo();
                contpresentacion.InsertarTexto(value);
            }
        }
        public void SelectAll()
        {
            contpresentacion.SeleccionarTodo();
        }

        public void GotoDown(bool select)
        {
            contpresentacion.IrLineaInferior(select);
        }

        public void GotoUp(bool select)
        {
            contpresentacion.IrLineaSuperior(select);
        }

        public void InsertParagraph(bool select)
        {
            contpresentacion.InsertarParrafo(select);

        }

        public void GotoRight(bool select, bool advanceByWord)
        {
            contpresentacion.IrSiguienteCaracter(select, advanceByWord ? TipoAvance.AvanzarPorPalabras : TipoAvance.AvanzarPorCaracteres);
        }

        public void GotoLeft(bool select, bool advanceByWord)
        {
            contpresentacion.IrAnteriorCaracter(select, advanceByWord ? TipoAvance.AvanzarPorPalabras : TipoAvance.AvanzarPorCaracteres);
        }

        public void GotoLineEnd(bool select)
        {
            contpresentacion.IrSiguienteCaracter(select, TipoAvance.AvanzarPorLineas);
        }

        public void GotoLineStart(bool select)
        {
            contpresentacion.IrAnteriorCaracter(select, TipoAvance.AvanzarPorLineas);
        }

        public void GotoNextPage(bool select)
        {
            contpresentacion.IrSiguienteCaracter(select, TipoAvance.AvanzarPorPaginas);
        }
        public void GotoPreviousPage(bool select)
        {
            contpresentacion.IrAnteriorCaracter(select, TipoAvance.AvanzarPorPaginas);                    
        }
        public void DeletePreviousCharacter()
        {
            contpresentacion.BorrarCaracterAnterior();            
        }
        public void DeleteCharacter()
        {
            contpresentacion.BorrarCaracter();                    
        }
        public void Cut()
        {
            contpresentacion.Cortar();
        }
        public void Copy()
        {
            contpresentacion.Copiar();
        }

        public void Paste()
        {
            contpresentacion.Pegar();            
        }

        //void docimpr_QueryPageSettings(object sender, QueryPageSettingsEventArgs e)
        //{
        //    if (!numpagina.HasValue)
        //    {
        //        numpagina = 0;
        //    }
        //    int lim = escritorio.Controlador.Documento.ObtenerNumPaginas();
        //    Pagina pag = escritorio.Controlador.Documento.ObtenerPagina(numpagina);

        //    e.PageSettings.PaperSize = new PaperSize(pag.Dimensiones.Ancho.Valor, pag.Dimensiones.Alto.Valor);

        //}

        //void doc_QueryPageSettings(object sender, QueryPageSettingsEventArgs e)
        //{
        //    if (!numpagina.HasValue) {
        //        numpagina=0;
        //    }
        //    int lim=escritorio.Controlador.Documento.ObtenerNumPaginas();
        //    Pagina pag=escritorio.Controlador.Documento.ObtenerPagina(numpagina);
            
        //    e.PageSettings.PaperSize = new PaperSize(pag.Dimensiones.Ancho, pag.Dimensiones.Alto);
        //}
        int? numpagina;
        void impresora_PrintPage(object sender, PrintPageEventArgs e)
        {
            if (!numpagina.HasValue) {
                numpagina=0;
            }
            if (numpagina == escritorio.Controlador.Documento.ObtenerNumPaginas()-1)
            {
                e.HasMorePages = false;
            }
            Graficador g=new Graficador(e.Graphics);
            escritorio.Controlador.Documento.DibujarPagina(g, new Punto(Medicion.Cero, Medicion.Cero), numpagina.Value, null);
            numpagina++;
        }
        const int deltax = 0;
        const int deltay = 0;
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            try
            {
                IGraficador graf = new Graficador(e.Graphics);
                escritorio.Dibujar(graf, contpresentacion.ObtenerSeleccion());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message +
                    "\r\n" +
                    ex.StackTrace);
            }            
        }
        
        bool EnCaptura = false;
        Thread TareaRegistro = null;
        float ultimaposx = 0;
        float ultimaposy = 0;
        int nummensajes;
        protected void RegistrarPosicionSinc(float x, float y, bool ampliarSeleccion)
        {
            using (Graphics g = Graphics.FromHwnd(this.Handle))
            {
                float posx = x - deltax;
                float posy = y - deltay;
                Graficador graf = new Graficador(g);
                escritorio.RegistrarPosicion(graf.Traducir(new PointF(posx, posy)), ampliarSeleccion);
            }
        }
        protected void RegistrarPosicion(float x,float y,bool ampliarSeleccion) {
            DateTime fecharegistro=DateTime.Now;;    
            ultimaposx=x;
            ultimaposy=y;
            RegistrarPosicionSinc(x, y, ampliarSeleccion);
            
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (!EnCaptura)
            {
                RegistrarPosicion(e.X, e.Y, false);
                Capture = true;
                EnCaptura = true;
            } 
            base.OnMouseDown(e);
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (EnCaptura)
            {
                RegistrarPosicion(e.X, e.Y, true);
                Capture = false;
                EnCaptura = false;
            }
            base.OnMouseUp(e);
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (EnCaptura)
            {
                RegistrarPosicion(e.X, e.Y, true);
            }
            base.OnMouseMove(e);
        }
        
    }
}
