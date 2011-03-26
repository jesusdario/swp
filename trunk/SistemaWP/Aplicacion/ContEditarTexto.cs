using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SistemaWP.Dominio;

namespace SistemaWP.Aplicacion
{
    enum TipoAvance
    {
        AvanzarPorCaracteres,
        AvanzarPorPalabras,
        AvanzarPorLineas,
        AvanzarPorParrafos,
        AvanzarPorPaginas
    }
    class ContEditarTexto
    {
        /// <summary>
        /// Mantiene una referencia al párrafo donde se encuentra el cursor
        /// </summary>
        Parrafo parrafoSeleccionado;
        /// <summary>
        /// Posición de inicio dentro del párrafo seleccionado donde se encuentra el cursor
        /// </summary>
        int posicionInsercion;
        /// <summary>
        /// Mantiene una referencia al párrafo de inicio de la selección.
        /// </summary>
        Parrafo parrafoInicioRango;
        /// <summary>
        /// Es la posición de inicio dentro del párrafo de selección.
        /// </summary>
        int? posicionInicioRango;
        /// <summary>
        /// Mantiene una referencia al último párrafo de la selección.
        /// </summary>
        Parrafo parrafoFinRango;
        /// <summary>
        /// Es la posición de fin de rango dentro del último párrafo de la selección
        /// </summary>
        int? posicionFinRango;
        /// <summary>
        /// Documento de edición
        /// </summary>
        Documento _documentoEdicion;
        public Documento DocumentoEdicion { get { return _documentoEdicion; } }
        public ContEditarTexto()
        {
            _documentoEdicion = Documento.Instancia;
        }
        public void IrAPosicion(int numCaracter,bool seleccionar)
        {
            Parrafo p=_documentoEdicion.ObtenerPrimerParrafo();
            int suma = 0;
            Parrafo ultimoParrafo;
            while (p != null)
            {
                int longitud=p.ObtenerLongitud();
                if (suma+longitud>numCaracter)  {
                    IndicarPosicion(p.ID,numCaracter-suma,seleccionar);
                    return;
                }
                ultimoParrafo = p;
                p=p.Siguiente;
            }
        }
        public bool ExisteSeleccion
        {
            get { return posicionFinRango != null; }
        }
        public void LimpiarSeleccion()
        {
            parrafoInicioRango = null;
            posicionInicioRango = null;
            parrafoFinRango = null;
            posicionFinRango = null;
        }
        public void IndicarPosicion(int idparrafo, int posicioninsercion,bool moverSeleccion)
        {
            if (moverSeleccion)
            {
                if (!ExisteSeleccion)
                {
                    posicionInicioRango = posicionInsercion;
                    parrafoInicioRango = parrafoSeleccionado;
                }
                IndicarFinRango(idparrafo, posicioninsercion);
            }
            else
            {
                LimpiarSeleccion();
            }
            parrafoSeleccionado = _documentoEdicion.ObtenerParrafo(idparrafo);
            posicionInsercion = posicioninsercion;            
        }
        public void IndicarFinRango(int idparrafo, int posicion)
        {
            parrafoFinRango = _documentoEdicion.ObtenerParrafo(idparrafo);
            posicionFinRango = posicion;
        }
        public void InsertarTexto(string cadena)
        {
            ReemplazarSeleccion();
            foreach (char c in cadena)
            {
                if (c >= 32)
                {
                    parrafoSeleccionado.AgregarCaracter(posicionInsercion, c);
                    posicionInsercion = posicionInsercion + 1;
                }
                else
                {
                    if (c == '\n')
                        InsertarParrafo();
                }
            }
        }
        public void AgregarCaracter(char caracter)
        {
            ReemplazarSeleccion();
            parrafoSeleccionado.AgregarCaracter(posicionInsercion, caracter);
            posicionInsercion = posicionInsercion + 1;
        }
        public void BorrarCaracterAnterior()
        {
            if (ExisteSeleccion)
            {
                ReemplazarSeleccion();
            }
            else
            {
                bool res=IrAnteriorCaracter(false,TipoAvance.AvanzarPorCaracteres);
                if (res)
                {
                    parrafoSeleccionado.BorrarCaracter(posicionInsercion);
                }
            }
        }
        int SiguientePosicion(TipoAvance tipo)
        {
            switch (tipo)
            {
                case TipoAvance.AvanzarPorCaracteres:
                    return posicionInsercion + 1;
                case TipoAvance.AvanzarPorPalabras:
                    return parrafoSeleccionado.ObtenerSiguientePalabra(posicionInsercion);                
                default:
                    throw new NotImplementedException();
            }
        }
        int AnteriorPosicion(TipoAvance tipo)
        {
            switch (tipo)
            {
                case TipoAvance.AvanzarPorCaracteres:
                    return posicionInsercion - 1;
                case TipoAvance.AvanzarPorPalabras:
                    return parrafoSeleccionado.ObtenerAnteriorPalabra(posicionInsercion);
                default:
                    throw new NotImplementedException();
            }
        }
        public bool IrSiguienteCaracter(bool moverSeleccion,TipoAvance TipoAvance)
        {
            if (posicionInsercion == parrafoSeleccionado.ObtenerLongitud())
            {
                if (parrafoSeleccionado.Siguiente == null) return false;
                IndicarPosicion(parrafoSeleccionado.Siguiente.ID, 0, moverSeleccion);
            }
            else
            {
                IndicarPosicion(parrafoSeleccionado.ID, SiguientePosicion(TipoAvance), moverSeleccion);
            }
            return true;
        }

        public bool IrAnteriorCaracter(bool moverSeleccion, TipoAvance TipoAvance)
        {
            if (posicionInsercion == 0)
            {
                if (parrafoSeleccionado.Anterior == null) return false; //no se puede borrar antes del primer párrafo.
                IndicarPosicion(parrafoSeleccionado.Anterior.ID, parrafoSeleccionado.Anterior.ObtenerLongitud(), moverSeleccion);
                
            }
            else
            {
                IndicarPosicion(parrafoSeleccionado.ID, AnteriorPosicion(TipoAvance), moverSeleccion);
            }
            return true;
        }
        public void BorrarCaracter()
        {
            if (ExisteSeleccion)
            {
                ReemplazarSeleccion();
            }
            else
            {
                parrafoSeleccionado.BorrarCaracter(posicionInsercion);
            }

        }
        void ReemplazarSeleccion() {
            if (ExisteSeleccion)
            {
                Seleccion s = ObtenerSeleccion();
                int inicio=s.ObtenerPosicionInicial();
                int fin=s.ObtenerPosicionFinal();
                Parrafo res = _documentoEdicion.BorrarRango(s.ObtenerParrafoInicial(), 
                    inicio, s.ObtenerParrafoFinal(), fin);
                if (res == parrafoFinRango)
                {
                    posicionInsercion = inicio;
                }
                LimpiarSeleccion();
            }
        }
        public void InsertarParrafo()
        {
            ReemplazarSeleccion();
            parrafoSeleccionado=_documentoEdicion.InsertarParrafo(parrafoSeleccionado, posicionInsercion);
            posicionInsercion = 0;
        }

        public Seleccion ObtenerSeleccion()
        {
            if (ExisteSeleccion)
            {
                return new Seleccion()
                {
                    PosicionParrafoInicio = posicionInicioRango.Value,
                    PosicionParrafoFin = posicionFinRango.Value,
                    Inicio = parrafoInicioRango,
                    Fin = parrafoFinRango
                };
            }
            else
            {
                return null;
            }
        }
        internal int ObtenerPosicionInsercion()
        {
            return posicionInsercion;
        }

        internal Parrafo ObtenerParrafoSeleccionado()
        {
            return parrafoSeleccionado;
        }

        internal void SeleccionarTodo()
        {         
            Parrafo inicio=_documentoEdicion.ObtenerPrimerParrafo();
            Parrafo fin = _documentoEdicion.ObtenerUltimoParrafo();
            parrafoInicioRango = inicio;
            posicionInicioRango = 0;
            parrafoFinRango = fin;
            posicionFinRango = fin.ObtenerLongitud();
            parrafoSeleccionado = parrafoInicioRango;
            posicionInsercion = 0;
        }

        internal string ObtenerTexto()
        {
            return _documentoEdicion.ToString();
        }
    }
}
