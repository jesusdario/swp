﻿using System;
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
            lock (this)
            {
                if (repaginar)
                {
                    _documento.Repaginar(-1);
                }
                if (ActualizarPresentacion != null)
                    ActualizarPresentacion(this, EventArgs.Empty);
            }
        }
        public void TeclaPulsada(char caracter)
        {
            lock (this)
            {
                if ((int)caracter >= 32)
                {
                    conttexto.AgregarCaracter(caracter);
                    EnActualizarPresentacion(true);
                }
            }
        }
        public void Pegar()
        {
            lock (this)
            {
                string cad = Clipboard.GetText();
                if (cad != null)
                {
                    conttexto.InsertarTexto(cad);
                    EnActualizarPresentacion(true);
                }
            }
        }
        public void InsertarTexto(string cadena)
        {
            lock (this)
            {
                conttexto.InsertarTexto(cadena);
            }
        }
        public void Copiar()
        {
            lock (this)
            {
                Seleccion sel = conttexto.ObtenerSeleccion();
                string cad = null;
                if (sel == null)
                    cad = string.Empty;
                else
                    cad = sel.ObtenerTexto();
                Clipboard.SetText(cad, TextDataFormat.UnicodeText);
            }
        }
        
        public void BorrarCaracter()
        {
            lock (this)
            {
                conttexto.BorrarCaracter();
                EnActualizarPresentacion(true);
            }
        }

        public void BorrarCaracterAnterior()
        {
            lock (this)
            {
                conttexto.BorrarCaracterAnterior();
                EnActualizarPresentacion(true);
            }
        }

        public void IrAnteriorCaracter(bool moverSeleccion, TipoAvance tipoAvance)
        {
            lock (this)
            {
                if (tipoAvance == TipoAvance.AvanzarPorLineas)
                {
                    Posicion posicion = ObtenerPosicion();
                    Posicion nueva = posicion.ObtenerInicioLinea();
                    CambiarPosicion(nueva, moverSeleccion);
                }
                else if (tipoAvance == TipoAvance.AvanzarPorPaginas)
                {
                    IrPaginaAnterior(moverSeleccion);
                }
                else
                {
                    conttexto.IrAnteriorCaracter(moverSeleccion, tipoAvance);
                }
                EnActualizarPresentacion(false);
            }
        }

        public void IrSiguienteCaracter(bool moverSeleccion, TipoAvance tipoAvance)
        {
            lock (this)
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
                }
                else
                {
                    conttexto.IrSiguienteCaracter(moverSeleccion, tipoAvance);
                }
                EnActualizarPresentacion(false);
            }
        }

        public void InsertarParrafo(bool ampliarSeleccion)
        {
            lock (this)
            {
                conttexto.InsertarParrafo();
                EnActualizarPresentacion(true);
            }
        }
        private void CambiarPosicion(Posicion nuevaPosicion,bool ampliarSeleccion)
        {
            lock (this)
            {
                conttexto.IndicarPosicion(nuevaPosicion.Linea.Parrafo.ID, nuevaPosicion.Linea.Inicio + nuevaPosicion.PosicionCaracter, ampliarSeleccion);
            }
        }
        internal void IrLineaSuperior(bool ampliarSeleccion)
        {
            lock (this)
            {
                Posicion pos = ObtenerPosicion();
                Posicion posanterior = pos.ObtenerLineaSuperior();
                CambiarPosicion(posanterior, ampliarSeleccion);
                EnActualizarPresentacion(false);
            }
        }

        internal void IrLineaInferior(bool ampliarSeleccion)
        {
            lock (this)
            {
                Posicion pos = ObtenerPosicion();
                Posicion possiguiente = pos.ObtenerLineaInferior();
                CambiarPosicion(possiguiente, ampliarSeleccion);
                EnActualizarPresentacion(false);
            }
        }
        internal void IrPaginaAnterior(bool ampliarSeleccion)
        {
            lock (this)
            {
                Posicion pos = ObtenerPosicion();
                Posicion possiguiente = pos.ObtenerPaginaAnterior();
                CambiarPosicion(possiguiente, ampliarSeleccion);
                EnActualizarPresentacion(false);
            }
        }
        internal void IrPaginaSiguiente(bool ampliarSeleccion)
        {
            lock (this)
            {
                Posicion pos = ObtenerPosicion();
                Posicion possiguiente = pos.ObtenerPaginaSiguiente();
                CambiarPosicion(possiguiente, ampliarSeleccion);
                EnActualizarPresentacion(false);
            }
        }
        public void DibujarPaginas(IGraficador generador,out Punto InicioCursor,out Punto FinCursor)
        {
            lock (this)
            {
                _documento.DibujarPagina(generador, Punto.Origen, 0, conttexto.ObtenerSeleccion());
                Posicion pos = _documento.ObtenerPosicionCursor(0,
                    conttexto.ObtenerParrafoSeleccionado(),
                    conttexto.ObtenerPosicionInsercion());
                _documento.CompletarPixels(pos);
                InicioCursor = new Punto(pos.PosicionPixelX, pos.PosicionPixelY);
                FinCursor = new Punto(pos.PosicionPixelX, pos.PosicionPixelY + pos.AltoLinea);
            }
            //e.Graphics.DrawLine(Pens.Black, new PointF(pos.PosicionPixelX, pos.PosicionPixelY), new PointF(pos.PosicionPixelX, pos.PosicionPixelY + pos.AltoLinea));
        }

        public void RegistrarPosicion(int pagina,Punto punto, bool ampliarSeleccion)
        {
            lock (this)
            {
                Posicion pos = _documento.ObtenerPosicionPixels(pagina, punto);
                conttexto.IndicarPosicion(pos.Linea.Parrafo.ID, pos.Linea.Inicio + pos.PosicionCaracter, ampliarSeleccion);
                EnActualizarPresentacion(false);
            }
        }

        public void Cortar()
        {
            lock (this)
            {
                Seleccion sel = conttexto.ObtenerSeleccion();
                if (sel != null)
                {
                    string cad = sel.ObtenerTexto();
                    conttexto.BorrarCaracter();
                }
                EnActualizarPresentacion(true);
            }
        }

        internal Seleccion ObtenerSeleccion()
        {
            lock (this)
            {
                return conttexto.ObtenerSeleccion();
            }
        }

        internal Posicion ObtenerPosicion()
        {
            lock (this)
            {
                return _documento.ObtenerPosicionCursor(0, conttexto.ObtenerParrafoSeleccionado(), conttexto.ObtenerPosicionInsercion());
            }
        }

        internal void SeleccionarTodo()
        {
            lock (this)
            {
                conttexto.SeleccionarTodo();
                EnActualizarPresentacion(false);
            }
        }

        internal string ObtenerTexto()
        {
            lock (this)
            {
                return conttexto.ObtenerTexto();
            }
        }
    }
}