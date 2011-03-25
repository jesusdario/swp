using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SistemaWP.Aplicacion;
using System.Windows.Forms;
using SistemaWP.Dominio;
using SistemaWP.IU.Graficos;

namespace SistemaWP.IU.PresentacionDocumento
{
    class ContPresentarDocumento
    {
        ContEditarTexto conttexto = new ContEditarTexto();
        PresentacionDocumento.DocumentoImpreso _documento;
        public DocumentoImpreso Documento { get { return _documento; } }
        public event EventHandler ActualizarPresentacion;
        public ContPresentarDocumento()
        {
            conttexto.IndicarPosicion(1, 0, false);
            _documento = new SistemaWP.IU.PresentacionDocumento.DocumentoImpreso(conttexto.DocumentoEdicion);
            _documento.Repaginar(0);
        }
        protected void EnActualizarPresentacion(bool repaginar)
        {
            if (repaginar)
            {
                _documento.Repaginar(-1);
            }
            if (ActualizarPresentacion != null)
                ActualizarPresentacion(this,EventArgs.Empty);
        }
        public void TeclaPulsada(char caracter)
        {
            if ((int)caracter >= 32)
            {
                conttexto.AgregarCaracter(caracter);
                EnActualizarPresentacion(true);
            }
        }
        public void Pegar()
        {
            string cad = Clipboard.GetText();
            if (cad != null)
            {
                conttexto.InsertarTexto(cad);
                EnActualizarPresentacion(true);
            }
        }
        public void InsertarTexto(string cadena)
        {
            conttexto.InsertarTexto(cadena);
        }
        public void Copiar()
        {
            Seleccion sel = conttexto.ObtenerSeleccion();
            string cad = null;
            if (sel == null)
                cad = string.Empty;
            else
                cad = sel.ObtenerTexto();
            Clipboard.SetText(cad, TextDataFormat.UnicodeText);
        }
        
        public void BorrarCaracter()
        {
            conttexto.BorrarCaracter();
            EnActualizarPresentacion(true);
        }

        public void BorrarCaracterAnterior()
        {
            conttexto.BorrarCaracterAnterior();
            EnActualizarPresentacion(true);
        }

        public void IrAnteriorCaracter(bool moverSeleccion, TipoAvance tipoAvance)
        {
            if (tipoAvance==TipoAvance.AvanzarPorLineas)
            {
                Posicion posicion=ObtenerPosicion();
                Posicion nueva=posicion.ObtenerInicioLinea();
                CambiarPosicion(nueva, moverSeleccion);
            }
            else if (tipoAvance == TipoAvance.AvanzarPorPaginas)
            {
                IrPaginaAnterior(moverSeleccion);
            } else 
            {
                conttexto.IrAnteriorCaracter(moverSeleccion, tipoAvance);                
            }
            EnActualizarPresentacion(false);
        }

        public void IrSiguienteCaracter(bool moverSeleccion, TipoAvance tipoAvance)
        {
            if (tipoAvance == TipoAvance.AvanzarPorLineas)
            {
                Posicion posicion = ObtenerPosicion();
                Posicion nueva = posicion.ObtenerFinLinea();
                CambiarPosicion(nueva, moverSeleccion);
            }
            else if (tipoAvance == TipoAvance.AvanzarPorPaginas)
            {
                IrPaginaSiguiente(moverSeleccion);
            } else 
            {
                conttexto.IrSiguienteCaracter(moverSeleccion, tipoAvance);
            }
            EnActualizarPresentacion(false);
        }

        public void InsertarParrafo(bool ampliarSeleccion)
        {
            conttexto.InsertarParrafo();
            EnActualizarPresentacion(true);
        }
        private void CambiarPosicion(Posicion nuevaPosicion,bool ampliarSeleccion)
        {
            conttexto.IndicarPosicion(nuevaPosicion.Linea.Parrafo.ID,nuevaPosicion.Linea.Inicio+nuevaPosicion.PosicionCaracter,ampliarSeleccion);
        }
        internal void IrLineaSuperior(bool ampliarSeleccion)
        {
            Posicion pos = ObtenerPosicion();
            Posicion posanterior = pos.ObtenerLineaSuperior();
            CambiarPosicion(posanterior,ampliarSeleccion);
            EnActualizarPresentacion(false);
        }

        internal void IrLineaInferior(bool ampliarSeleccion)
        {
            Posicion pos = ObtenerPosicion();
            Posicion possiguiente = pos.ObtenerLineaInferior();
            CambiarPosicion(possiguiente, ampliarSeleccion);
            EnActualizarPresentacion(false);
        }
        internal void IrPaginaAnterior(bool ampliarSeleccion)
        {
            Posicion pos = ObtenerPosicion();
            Posicion possiguiente = pos.ObtenerPaginaAnterior();
            CambiarPosicion(possiguiente, ampliarSeleccion);
            EnActualizarPresentacion(false);
        }
        internal void IrPaginaSiguiente(bool ampliarSeleccion)
        {
            Posicion pos = ObtenerPosicion();
            Posicion possiguiente = pos.ObtenerPaginaSiguiente();
            CambiarPosicion(possiguiente, ampliarSeleccion);
            EnActualizarPresentacion(false);
        }
        public void DibujarPaginas(IGraficador generador,out Punto InicioCursor,out Punto FinCursor)
        {

            _documento.DibujarPagina(generador,Punto.Origen, 0, conttexto.ObtenerSeleccion());
            Posicion pos = _documento.ObtenerPosicionCursor(0,
                conttexto.ObtenerParrafoSeleccionado(),
                conttexto.ObtenerPosicionInsercion());
            _documento.CompletarPixels(pos);
            InicioCursor = new Punto(pos.PosicionPixelX, pos.PosicionPixelY);
            FinCursor = new Punto(pos.PosicionPixelX, pos.PosicionPixelY + pos.AltoLinea);
            //e.Graphics.DrawLine(Pens.Black, new PointF(pos.PosicionPixelX, pos.PosicionPixelY), new PointF(pos.PosicionPixelX, pos.PosicionPixelY + pos.AltoLinea));
        }

        public void RegistrarPosicion(int pagina,Punto punto, bool ampliarSeleccion)
        {
            Posicion pos = _documento.ObtenerPosicionPixels(pagina, punto);
            conttexto.IndicarPosicion(pos.Linea.Parrafo.ID, pos.Linea.Inicio + pos.PosicionCaracter, ampliarSeleccion);
            EnActualizarPresentacion(false);
        }

        public void Cortar()
        {
            Seleccion sel=conttexto.ObtenerSeleccion();
            if (sel != null)
            {
                string cad=sel.ObtenerTexto();
                conttexto.BorrarCaracter();
            }
            EnActualizarPresentacion(true);
        }

        internal Seleccion ObtenerSeleccion()
        {
            return conttexto.ObtenerSeleccion();
        }

        internal Posicion ObtenerPosicion()
        {
            return _documento.ObtenerPosicionCursor(0, conttexto.ObtenerParrafoSeleccionado(), conttexto.ObtenerPosicionInsercion());
        }

        internal void SeleccionarTodo()
        {
            conttexto.SeleccionarTodo();
            EnActualizarPresentacion(false);
        }

        internal string ObtenerTexto()
        {
            return conttexto.ObtenerTexto();
        }
    }
}
