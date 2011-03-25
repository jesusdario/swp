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
        ContPresentarDocumento contpresentacion = new ContPresentarDocumento();
        Escritorio escritorio;
        public PresentadorDocumento()
        {
            InitializeComponent();
            escritorio = new Escritorio(contpresentacion);
            escritorio.Dimensiones = new TamBloque(new Medicion(50, Unidad.Milimetros), new Medicion(100, Unidad.Milimetros));
            escritorio.Controlador.ActualizarPresentacion += new EventHandler(Controlador_ActualizarPresentacion);
        }

        void Controlador_ActualizarPresentacion(object sender, EventArgs e)
        {
            Refresh();
        }

        private void PresentadorDocumento_KeyPress(object sender, KeyPressEventArgs e)
        {
            contpresentacion.TeclaPulsada(e.KeyChar);
            e.Handled = true;
        }
        private Unidad CentesimaPulgada = new Unidad("CentPlg", "CP", 0.01, Unidad.Pulgadas);
        private void PresentadorDocumento_KeyDown(object sender, KeyEventArgs e)
        {
            
            switch (e.KeyCode)
            {
                case Keys.X:
                    if (e.Control) //CTRL+V=PEGAR
                    {
                        contpresentacion.Cortar();
                        e.Handled = true;
                    }
                    break;
                case Keys.V:
                    if (e.Control) //CTRL+V=PEGAR
                    {
                        contpresentacion.Pegar();
                        e.Handled = true;
                    }
                    break;
                case Keys.C: //CTRL+C=COPIAR
                    if (e.Control)
                    {
                        contpresentacion.Copiar();
                        e.Handled = true;
                    }
                    break;
                case Keys.P: //CTRL+P=IMPRIMIR
                    if (e.Control)
                    {
                        PrintDialog impr = new PrintDialog();
                        impr.UseEXDialog = true;
                        if (impr.ShowDialog(this)==DialogResult.OK)
                        {
                            PrintDocument docimpr = new PrintDocument();
                            docimpr.PrinterSettings = impr.PrinterSettings;
                            docimpr.DefaultPageSettings.PaperSize=new PaperSize("DEF",
                                (int)new Medicion(210).ConvertirA(CentesimaPulgada).Valor,
                                (int)new Medicion(270).ConvertirA(CentesimaPulgada).Valor);
                            docimpr.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
                            docimpr.OriginAtMargins = true;
                            docimpr.PrintPage += new PrintPageEventHandler(impresora_PrintPage);
                            numpagina = null;
                            docimpr.Print();
                        }
                    }
                    break;
                case Keys.E://CTRL+E=SELECCIONAR TODO
                    if (e.Control)
                    {
                        contpresentacion.SeleccionarTodo();
                        e.Handled = true;
                    }
                    break;
                case Keys.Delete:
                    contpresentacion.BorrarCaracter();
                    e.Handled = true;
                    break;                    
                case Keys.Back:
                    contpresentacion.BorrarCaracterAnterior();
                    e.Handled = true;
                    break;
                case Keys.PageUp:
                    contpresentacion.IrAnteriorCaracter(e.Shift, TipoAvance.AvanzarPorPaginas);
                    break;
                case Keys.PageDown:
                    contpresentacion.IrSiguienteCaracter(e.Shift, TipoAvance.AvanzarPorPaginas);
                    break;
                case Keys.Home:
                    contpresentacion.IrAnteriorCaracter(e.Shift,TipoAvance.AvanzarPorLineas);
                    e.Handled = true;
                    break;
                case Keys.End:
                    contpresentacion.IrSiguienteCaracter(e.Shift, TipoAvance.AvanzarPorLineas);
                    break;
                case Keys.Left:
                    contpresentacion.IrAnteriorCaracter(e.Shift,e.Control?TipoAvance.AvanzarPorPalabras:TipoAvance.AvanzarPorCaracteres);
                    e.Handled = true;
                    break;
                case Keys.Right:
                    contpresentacion.IrSiguienteCaracter(e.Shift, e.Control ? TipoAvance.AvanzarPorPalabras : TipoAvance.AvanzarPorCaracteres);
                    e.Handled = true;
                    break;
                case Keys.Enter:
                    contpresentacion.InsertarParrafo(e.Shift);
                    e.Handled = true;
                    break;
                case Keys.Up:
                    contpresentacion.IrLineaSuperior(e.Shift);
                    e.Handled = true;
                    break;
                case Keys.Down:
                    contpresentacion.IrLineaInferior(e.Shift);
                    e.Handled = true;
                    break;

            }
            
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
        
        private void PresentadorDocumento_Paint(object sender, PaintEventArgs e)
        {
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

        private void PresentadorDocumento_Load(object sender, EventArgs e)
        {
            
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
        private void PresentadorDocumento_MouseDown(object sender, 
            MouseEventArgs e)
        {
            if (!EnCaptura)
            {
                RegistrarPosicion(e.X, e.Y, false);
                Capture = true;
                EnCaptura = true;
            }
        }

        private void PresentadorDocumento_MouseMove(object sender, MouseEventArgs e)
        {
            if (EnCaptura)
            {
                RegistrarPosicion(e.X, e.Y, true);
            }
        }

        private void PresentadorDocumento_MouseUp(object sender, MouseEventArgs e)
        {
            if (EnCaptura)
            {
                RegistrarPosicion(e.X, e.Y, true);
                Capture = false;
                EnCaptura = false;
            }
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
