/**********************************************
This Program was written by Dario Romero A.
It is licensed under the Apache License v.2.0.
***********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using SWPEditor.Dominio;
using SWPEditor.IU.Graficos;
using SWPEditor.IU.VistaDocumento;
using SWPEditor.IU.PresentacionDocumento;
using SWPEditor.Aplicacion;
using SWPEditor.Dominio.TextoFormato;

namespace SWPEditor.IU
{
    public class SWPGenericControl
    {
        Escritorio escritorio;
        private ContPresentarDocumento _ControlDocumento
        {
            get
            {
                return escritorio.ControlDocumento;
            }
        }
        Documento _documento;
        public ColorDocumento BackgroundColor
        {
            get
            {
                return escritorio.ColorFondo;
            }
            set
            {
                escritorio.ColorFondo = value;
                OnRefreshRequested(this, EventArgs.Empty);
            }
        }
        public SWPGenericControl(IGraficador graficadorConsultas)
            : base()
        {
            _documento = new Documento();
            escritorio = new Escritorio(_documento,graficadorConsultas);
            escritorio.ActualizarPresentacion += new EventHandler(contpresentacion_ActualizarPresentacion);
            escritorio.Dimensiones=new TamBloque(new Medicion(50, Unidad.Milimetros), new Medicion(50, Unidad.Milimetros));
        }
        public Documento GetDocument()
        {
            return _documento;
        }
        event EventHandler _RefreshRequested;
        public event EventHandler RefreshRequested
        {
            add
            {
                _RefreshRequested += value;
            }
            remove
            {
                _RefreshRequested -= value;
            }
        }
        EventHandler _DocumentChanged;
        public event EventHandler DocumentChanged
        {
            add
            {
                _DocumentChanged += value;
            }
            remove
            {
                _DocumentChanged -= value;
            }
        }
        protected void OnRefreshRequested(object sender, EventArgs e)
        {
            if (_RefreshRequested != null)
            {
                _RefreshRequested(this, EventArgs.Empty);
                if (_DocumentChanged != null)
                {
                    _DocumentChanged(this, EventArgs.Empty);
                }
            }
        }
        void contpresentacion_ActualizarPresentacion(object sender, EventArgs e)
        {
            OnRefreshRequested(sender, e);
        }
        public void DrawDesktop(IGraficador graficador,bool dibujarSeleccion,bool dibujarCursor)
        {
            escritorio.Dibujar(graficador, dibujarSeleccion?_ControlDocumento.ObtenerSeleccion():null,dibujarCursor);
        }
        public void NotifySizeChanged(TamBloque nuevoTamaño)
        {
            escritorio.Dimensiones = nuevoTamaño;            
        }
        public void NotifyCharacterPressed(char tecla)
        {
            if (tecla>=32) {
                _ControlDocumento.TeclaPulsada(tecla);
            }
        }
        event EventHandler _PrintRequested;

        public event EventHandler PrintRequested
        {
            add
            {
                _PrintRequested += value;
            }
            remove
            {
                _PrintRequested -= value;
            }
        }
        public void Print()
        {
            if (_PrintRequested != null)
            {
                _PrintRequested(this, EventArgs.Empty);
            }
        }
        public void Cut(IClipboard clipboard)
        {
            _ControlDocumento.Cortar(clipboard);
        }
        public void Copy(IClipboard clipboard)
        {
            _ControlDocumento.Copiar(clipboard);
        }
        public void Paste(IClipboard clipboard)
        {
            _ControlDocumento.Pegar(clipboard);
        }
        public void NotifyKeyDown(Key tecla, bool select, IClipboard clipboard)
        {
            switch (tecla)
            {
                case Key.Cut: Cut(clipboard); break;
                case Key.Paste: Paste(clipboard); break;
                case Key.Copy: Copy(clipboard); break;
                case Key.Print: Print(); break;
                case Key.SelectAll: SelectAll(); break;
                case Key.DocumentHome: GotoDocumentStart(select); break;
                case Key.DocumentEnd: GotoDocumentEnd(select); break;
                case Key.ParagraphPrevious: GotoPreviousParagraph(select); break;
                case Key.ParagraphNext: GotoNextParragraph(select); break;
                case Key.Delete:
                    DeleteCharacter();
                    break;
                case Key.BackSpace:
                    DeletePreviousCharacter();
                    break;
                case Key.PageUp:
                    GotoPreviousPage(select);
                    break;
                case Key.PageDown:
                    GotoNextPage(select);
                    break;
                case Key.Home:
                    GotoLineStart(select);
                    break;
                case Key.End:
                    GotoLineEnd(select);
                    break;
                case Key.Left:
                    GotoLeft(select);
                    break;
                case Key.Right:
                    GotoRight(select);
                    break;
                case Key.PreviousWord:
                    GotoPreviousWord(select);
                    break;
                case Key.NextWord:
                    GotoNextWord(select);
                    break;
                case Key.Enter:
                    InsertParagraph(select);
                    break;
                case Key.Up:
                    GotoUp(select);
                    break;
                case Key.Down:
                    GotoDown(select);
                    break;

            }
        }

        private void GotoPreviousParagraph(bool select)
        {
            _ControlDocumento.IrAParrafoAnterior(select);
        }

        private void GotoNextParragraph(bool select)
        {
            _ControlDocumento.IrAParrafoSiguiente(select);
        }


        private void GotoDocumentEnd(bool select)
        {
            _ControlDocumento.IrAFinDocumento(select);
        }

        private void GotoDocumentStart(bool select)
        {
            _ControlDocumento.IrAInicioDocumento(select);
        }
        public enum Key
        {
            Up,Down,Right,Left,PageUp,PageDown,Home,End,Enter,Delete,BackSpace,Cut,Copy,Paste,Print,SelectAll,
            DocumentHome,
            DocumentEnd,
            ParagraphPrevious,
            ParagraphNext,
            PreviousWord,
            NextWord
        }
        
        private Unidad CentesimaPulgada = new Unidad("CentPlg", "CP", 0.01, Unidad.Pulgadas);
        
        public void InsertText(string text)
        {
            _ControlDocumento.InsertarTexto(text);
        }
        public string GetText() {
            return _ControlDocumento.ObtenerTexto();
        }
        public string GetHTML()
        {
            return _ControlDocumento.ObtenerHTML();
        }
        public void SetText(string value) {
            _ControlDocumento.SeleccionarTodo();
            _ControlDocumento.InsertarTexto(value);        
        }
        public void SelectAll()
        {
            _ControlDocumento.SeleccionarTodo();
        }

        public void GotoDown(bool select)
        {
            _ControlDocumento.IrLineaInferior(select);
        }

        public void GotoUp(bool select)
        {
            _ControlDocumento.IrLineaSuperior(select);
        }

        public void InsertParagraph(bool select)
        {
            _ControlDocumento.InsertarParrafo(select);

        }

        public void GotoRight(bool select)
        {
            _ControlDocumento.IrSiguienteCaracter(select, TipoAvance.AvanzarPorCaracteres);
        }

        public void GotoLeft(bool select)
        {
            _ControlDocumento.IrAnteriorCaracter(select, TipoAvance.AvanzarPorCaracteres);
        }
        public void GotoPreviousWord(bool select)
        {
            _ControlDocumento.IrAnteriorCaracter(select, TipoAvance.AvanzarPorPalabras);
        }
        public void GotoNextWord(bool select)
        {
            _ControlDocumento.IrSiguienteCaracter(select, TipoAvance.AvanzarPorPalabras);
        }

        public void GotoLineEnd(bool select)
        {
            _ControlDocumento.IrSiguienteCaracter(select, TipoAvance.AvanzarPorLineas);
        }

        public void GotoLineStart(bool select)
        {
            _ControlDocumento.IrAnteriorCaracter(select, TipoAvance.AvanzarPorLineas);
        }

        public void GotoNextPage(bool select)
        {
            _ControlDocumento.IrSiguienteCaracter(select, TipoAvance.AvanzarPorPaginas);
        }
        public void GotoPreviousPage(bool select)
        {
            _ControlDocumento.IrAnteriorCaracter(select, TipoAvance.AvanzarPorPaginas);                    
        }
        public void DeletePreviousCharacter()
        {
            _ControlDocumento.BorrarCaracterAnterior();            
        }
        public void DeleteCharacter()
        {
            _ControlDocumento.BorrarCaracter();                    
        }
        
        public void NotifyMouseDown(Punto point,bool extendSelection)
        {
            escritorio.IrAPosicion(point, extendSelection);
        }
        public void NotifyMouseMove(Punto point)
        {
            escritorio.IrAPosicion(point, true);
        }
        public void NotifyMouseUp(Punto point)
        {
            escritorio.IrAPosicion(point, true);
            Seleccion s = _ControlDocumento.ObtenerSeleccion();
            if (s != null && s.EstaVacia)
            {
                _ControlDocumento.QuitarSeleccion();
            }
        }        
        public void ChangeFontColor(ColorDocumento color)
        {
            _ControlDocumento.CambiarColorLetra(color);
        }
        public void ChangeFontBackground(ColorDocumento color)
        {
            _ControlDocumento.CambiarColorFondo(color);
        }
        public void ChangeFontBold()
        {
            _ControlDocumento.CambiarLetraNegrilla();
        }

        public void ChangeFontItalic()
        {
            _ControlDocumento.CambiarLetraCursiva();
        }

        public void ChangeFontUnderlined()
        {
            _ControlDocumento.CambiarLetraSubrayado();
        }

        public void IncreaseFontSize()
        {
            _ControlDocumento.AgrandarLetra();
        }

        public void ReduceFontSize()
        {
            _ControlDocumento.ReducirLetra();
        }
        public void ChangeFont(string fontFamilyName, Medicion size)
        {
            _ControlDocumento.CambiarLetra(fontFamilyName, size);
        }
        
        public void SetFontSizeInPoints(decimal value)
        {
            _ControlDocumento.CambiarTamLetra(new Medicion((float)value, Unidad.Puntos));
        }
        
        public void AlignLeft()
        {
            _ControlDocumento.AlinearIzquierda();
        }

        public void AlignCenter()
        {
            _ControlDocumento.AlinearCentro();
        }

        public void AlignRight()
        {
            _ControlDocumento.AlinearDerecha();
        }

        public void IncreaseLineSpace()
        {
            _ControlDocumento.AumentarInterlineado();
        }

        public void DecreaseLineSpace()
        {
            _ControlDocumento.DisminuirInterlineado();
        }

        public void AddSpaceBeforeParagraph()
        {
            _ControlDocumento.AumentarEspacioAntesParrafo();
        }
        public void RemoveSpaceBeforeParagraph()
        {
            _ControlDocumento.QuitarEspacioAntesParrafo();
        }
        public DocumentPosition GetPosition()
        {
            Posicion posicion=_ControlDocumento.ObtenerPosicion();
            return new DocumentPosition(posicion.IndicePagina+1, posicion.IndiceLinea+1, posicion.PosicionCaracter+1);
        }
    }
}
