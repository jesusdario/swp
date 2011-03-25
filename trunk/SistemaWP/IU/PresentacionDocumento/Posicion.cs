﻿using System;
using System.Collections.Generic;
using System.Text;
using SistemaWP.Dominio;

namespace SistemaWP.IU.PresentacionDocumento
{
    public class Posicion
    {
        /// <summary>
        /// Vista de documento
        /// </summary>
        public DocumentoImpreso VDocumento { get; set; }
        /// <summary>
        /// Indice de Página
        /// </summary>
        public int IndicePagina { get; set; }
        /// <summary>
        /// Página
        /// </summary>
        public Pagina Pagina { get; set; }
        /// <summary>
        /// Línea
        /// </summary>
        public Linea Linea { get; set; }
        /// <summary>
        /// Número de línea en el documento
        /// </summary>
        public int IndiceLinea { get; set; }
        /// <summary>
        /// Número de línea en la página
        /// </summary>
        public int IndiceLineaPagina { get; set; }
        /// <summary>
        /// Posición de caracter relativa al inicio de la línea
        /// </summary>
        public int PosicionCaracter { get; set; }
        Punto _PosicionPagina;
        /// <summary>
        /// Posición de caracter en la página
        /// </summary>
        public Punto PosicionPagina { get { CompletarPosicion(); return _PosicionPagina; } set { _PosicionPagina = value; } }
        /// <summary>
        /// Posición de Pixel Y relativa al inicio de la página
        /// </summary>
        public Medicion PosicionPixelY { get { return PosicionPagina.Y; } }
        /// <summary>
        /// Posición de Pixel X relativa al inicio de la página
        /// </summary>
        public Medicion PosicionPixelX { get { return PosicionPagina.X; } }
        bool _ConPosicionPixels = false;
        /// <summary>
        /// Alto de la línea
        /// </summary>
        public Medicion AltoLinea { get; set; }
        public Medicion? ReferenciaX { get; set; }
        public Posicion(DocumentoImpreso documento)
        {
            PosicionPagina = new Punto(Medicion.Cero, Medicion.Cero);
            VDocumento = documento;
        }
        public void CompletarPosicion()
        {
            if (!_ConPosicionPixels)
            {
                try
                {
                    _ConPosicionPixels = true;
                    VDocumento.CompletarPixels(this);
                }
                catch
                {
                    _ConPosicionPixels = false;
                }
            }
        }
        public Posicion ObtenerInicioLinea()
        {
            Posicion p = new Posicion(VDocumento);
            VDocumento.Completar(p, IndicePagina, IndiceLinea, 0);
            return p;
        }
        public Posicion ObtenerFinLinea()
        {
            Posicion p = new Posicion(VDocumento);
            VDocumento.Completar(p, IndicePagina, IndiceLinea, Math.Max(Linea.Cantidad-1,0));
            return p;
        }
        public Posicion ObtenerLineaSuperior()
        {
            if (IndiceLinea != 0)
            {
                Posicion p = new Posicion(VDocumento);
                p.ReferenciaX = ReferenciaX;
                VDocumento.Completar(p, IndicePagina, IndiceLinea - 1, PosicionCaracter);
                return p;
            }
            else
            {
                return (Posicion)this.MemberwiseClone();
            }
           
        }
        public TamBloque ObtenerMargenEdicion()
        {
            return Linea.ObtenerMargenEdicion();
        }
        
        public Posicion ObtenerLineaInferior()
        {
            if (IndiceLinea != VDocumento.ObtenerNumLineas() - 1)
            {
                Posicion p = new Posicion(VDocumento);
                p.ReferenciaX = ReferenciaX;
                VDocumento.Completar(p, IndicePagina, IndiceLinea + 1, PosicionCaracter);
                return p;
            }
            else
            {
                return ObtenerCopia();
            }
            
        }
        public Posicion ObtenerCopia()
        {
            return (Posicion)this.MemberwiseClone();
        }
        public Posicion ObtenerPaginaAnterior()
        {
            if (IndicePagina != 0)
            {
                Punto pt = PosicionPagina;
                return VDocumento.ObtenerPosicionPixels(IndicePagina - 1, pt);
            }
            else
                return ObtenerCopia();
        }
        public Posicion ObtenerPaginaSiguiente()
        {
            if (IndicePagina != VDocumento.ObtenerNumPaginas()-1)
            {
                Punto pt = PosicionPagina;
                return VDocumento.ObtenerPosicionPixels(IndicePagina + 1, pt);
            }
            else
                return ObtenerCopia();
        }
        internal void AvanzarLinea()
        {
            if (IndiceLinea < Pagina.LineaInicio + Pagina.Cantidad)
            {
                VDocumento.Completar(this, IndicePagina, IndiceLinea + 1, 0);
            }
            else
            {
                VDocumento.Completar(this, IndicePagina + 1, 0, 0);
            }
        }        
        internal void Avanzar(int posicion)
        {
            ReferenciaX = null;
            int indicelinea;
            while (posicion > Linea.Cantidad)
            {
                posicion -= Linea.Cantidad - PosicionCaracter;
                indicelinea = IndiceLinea;
                AvanzarLinea();
                if (posicion>Linea.Cantidad&&IndiceLinea == indicelinea) //Bucle infinoto?
                {
                    PosicionCaracter = 0;
                    return;
                }
            }
            if (posicion == Linea.Cantidad&&Linea.Inicio+posicion<Linea.Parrafo.ObtenerLongitud())
            {
                posicion = 0;
                AvanzarLinea();
            }
            PosicionCaracter = posicion;
            //while (Linea.Inicio+Linea.Cantidad
        }
    }
}
