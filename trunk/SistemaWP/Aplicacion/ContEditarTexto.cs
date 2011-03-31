using System;
using System.Collections.Generic;
using System.Text;
using SWPEditor.Dominio;
using System.Diagnostics;
using SWPEditor.Dominio.TextoFormato;

namespace SWPEditor.Aplicacion
{
    public enum TipoAvance
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

        struct EstadisticasCambios
        {
            Parrafo parrafoInicial;
            Parrafo parrafoFinal;
           
            public Parrafo ObtenerParrafoInicial()
            {
                return parrafoInicial;
            }
            public Parrafo ObtenerParrafoFinal()
            {
                return parrafoFinal;
            }
            public void Reiniciar()
            {
                parrafoInicial = null;
                parrafoFinal = null;
            }
            public void CambiarParrafoFin(Parrafo parrafo)
            {
                parrafoFinal = parrafo;
            }
            public void RegistrarCambios(Parrafo parrafo)
            {
                RegistrarCambios(parrafo, parrafo);
            }
            public void RegistrarCambios(Parrafo inicio, Parrafo fin)
            {
                if (parrafoInicial == null) 
                    parrafoInicial = inicio;
                else {
                    if (inicio.EsSiguiente(parrafoInicial))
                    {
                        parrafoInicial = inicio;
                    }
                }
                
                if (parrafoFinal == null)
                    parrafoFinal = fin;
                else
                {
                    if (parrafoFinal.EsSiguiente(fin))
                    {
                        parrafoInicial = fin;
                    }
                }            
            }
            public void RegistrarInsercion(Parrafo parrafo, int posicion,int cantidad)
            {
                RegistrarCambios(parrafo);
            }
            public void RegistrarEliminacion(Parrafo parrafo, int posicion, int cantidad)
            {
                RegistrarCambios(parrafo);
            }
            public void RegistrarEliminacion(Parrafo parrafoInicio, int posicionInicio,Parrafo parrafoFin,int posicionfin)
            {
                if (parrafoInicio == parrafoFin)
                {
                    RegistrarCambios(parrafoInicio);
                }
                else
                {
                    if (parrafoInicial == parrafoFin)
                    {
                        parrafoInicial = parrafoInicio;
                    }
                    if (parrafoFinal == parrafoFin)
                    {
                        parrafoFinal = parrafoInicio;
                    }
                }
            }

            internal void RegistrarInsercionParrafo(Parrafo parrafoSeleccionado, int posicion, Parrafo nuevo)
            {
                if (parrafoSeleccionado.EsSiguiente(nuevo))
                {
                    RegistrarCambios(parrafoSeleccionado, nuevo);
                }
                else
                {
                    RegistrarCambios(nuevo,parrafoSeleccionado);
                }
            }

            internal void RegistrarCambioFormato(Formato formato, Parrafo parrafoInicio, int p, Parrafo parrafoFin, int p_5)
            {
                RegistrarCambios(parrafoInicio,parrafoFin);
            }

            internal void RegistrarCambioFormatoParrafo(Parrafo parrafoSeleccionado)
            {
                RegistrarCambios(parrafoSeleccionado);
            }
        }
        EstadisticasCambios _estadisticas = new EstadisticasCambios();
        public Parrafo CambioInicio()
        {
            return _estadisticas.ObtenerParrafoInicial();
        }
        public Parrafo CambioFin()
        {
            return _estadisticas.ObtenerParrafoFinal();
        }
        public void CambioReiniciar()
        {
            _estadisticas.Reiniciar();
        }
        public ContEditarTexto(Documento documento)
        {
            _documentoEdicion = documento;
        }
        public void IrAPosicion(int numCaracter,bool seleccionar)
        {
            Parrafo p=_documentoEdicion.ObtenerPrimerParrafo();
            int suma = 0;
            Parrafo ultimoParrafo;
            while (p != null)
            {
                int longitud=p.Longitud;
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
            int inicio = 0;
            int cantidad = 0;
            int poscadena = 0;
            
            for (int i=0;i<cadena.Length;i++)
            {
                char c = cadena[i];
                if (c >= 32)
                {
                    cantidad++;
                }
                else
                {
                    if (cantidad > 0)
                    {
                        parrafoSeleccionado.InsertarCadena(posicionInsercion, cadena.Substring(inicio, cantidad));
                        _estadisticas.RegistrarInsercion(parrafoSeleccionado, posicionInsercion, cantidad);
                        posicionInsercion = posicionInsercion + cantidad;
                        inicio = i+1;
                        cantidad = 0;
                    }
                    if (c == '\n')
                    {
                        InsertarParrafo();
                        inicio = i + 1;
                        cantidad = 0;
                    }
                }
                poscadena++;
            }
            if (cantidad > 0)
            {
                parrafoSeleccionado.InsertarCadena(posicionInsercion, cadena.Substring(inicio, cantidad));
                _estadisticas.RegistrarInsercion(parrafoSeleccionado, posicionInsercion, cantidad);                        
                posicionInsercion = posicionInsercion + cantidad;
                inicio = poscadena + 1;
                cantidad = 0;
            }
            
        }
        public void AgregarCaracter(char caracter)
        {
            ReemplazarSeleccion();
            parrafoSeleccionado.AgregarCaracter(posicionInsercion, caracter);
            _estadisticas.RegistrarInsercion(parrafoSeleccionado, posicionInsercion, 1);
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
                    _estadisticas.RegistrarEliminacion(parrafoSeleccionado, posicionInsercion, 1);
                }
            }
        }
        int SiguientePosicion(TipoAvance tipo)
        {
            if (anteriorFormato != null)
            {
                parrafoSeleccionado.SimplificarFormato();
                anteriorFormato = null;
            }
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
            if (anteriorFormato != null)
            {
                parrafoSeleccionado.SimplificarFormato();
                anteriorFormato = null;
            }
            switch (tipo)
            {
                case TipoAvance.AvanzarPorCaracteres:
                    Debug.Assert(posicionInsercion >= 1);
                    return posicionInsercion - 1;
                case TipoAvance.AvanzarPorPalabras:
                    return parrafoSeleccionado.ObtenerAnteriorPalabra(posicionInsercion);
                default:
                    throw new NotImplementedException();
            }
        }
        public bool IrSiguienteCaracter(bool moverSeleccion,TipoAvance TipoAvance)
        {
            if (posicionInsercion == parrafoSeleccionado.Longitud)
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
                IndicarPosicion(parrafoSeleccionado.Anterior.ID, parrafoSeleccionado.Anterior.Longitud, moverSeleccion);
                
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
                _estadisticas.RegistrarEliminacion(parrafoSeleccionado, posicionInsercion, 1);
            }

        }
        void ReemplazarSeleccion() {
            if (ExisteSeleccion)
            {
                Seleccion s = ObtenerSeleccion();
                Parrafo parrafoinicio = s.ObtenerParrafoInicial();
                Parrafo parrafofin = s.ObtenerParrafoFinal();
                int inicio=s.ObtenerPosicionInicial();
                int fin=s.ObtenerPosicionFinal();
                _estadisticas.RegistrarEliminacion(parrafoinicio, inicio,parrafofin,fin);
                Parrafo res = _documentoEdicion.BorrarRango(
                    parrafoinicio,inicio, 
                    parrafofin, fin);
                //En este caso, el párrafo final se habrá fusionado con el anterior asi que cambiarlo
                //por la selección actual
                
                parrafoSeleccionado = parrafoinicio;
                posicionInsercion = inicio;
                
                LimpiarSeleccion();
            }
        }
        public void InsertarParrafo()
        {
            ReemplazarSeleccion();
            ;
            Parrafo nuevo=_documentoEdicion.InsertarParrafo(parrafoSeleccionado, posicionInsercion);
            _estadisticas.RegistrarInsercionParrafo(parrafoSeleccionado,posicionInsercion,nuevo);
            parrafoSeleccionado=nuevo;            
            posicionInsercion = 0;
        }

        public Seleccion ObtenerSeleccion()
        {
            if (ExisteSeleccion)
            {
                return new Seleccion(_documentoEdicion,
                    parrafoInicioRango, 
                    posicionInicioRango.Value,
                    parrafoFinRango,
                    posicionFinRango.Value);
            }
            else
            {
                return null;
            }
        }
        public int ObtenerPosicionInsercion()
        {
            return posicionInsercion;
        }

        public Parrafo ObtenerParrafoSeleccionado()
        {
            return parrafoSeleccionado;
        }

        public void SeleccionarTodo()
        {         
            Parrafo inicio=_documentoEdicion.ObtenerPrimerParrafo();
            Parrafo fin = _documentoEdicion.ObtenerUltimoParrafo();
            parrafoInicioRango = inicio;
            posicionInicioRango = 0;
            parrafoFinRango = fin;
            posicionFinRango = fin.Longitud;
            parrafoSeleccionado = parrafoInicioRango;
            posicionInsercion = 0;
        }

        public string ObtenerTexto()
        {
            return _documentoEdicion.ToString();
        }
        Formato anteriorFormato;
        private void AplicarFormato(Formato formato)
        {
            Seleccion s = ObtenerSeleccion();
            if (s != null)
            {
                _documentoEdicion.CambiarFormato(formato, s.ObtenerParrafoInicial(), s.ObtenerPosicionInicial(), s.ObtenerParrafoFinal(), s.ObtenerPosicionFinal());
                _estadisticas.RegistrarCambioFormato(formato, s.ObtenerParrafoInicial(), s.ObtenerPosicionInicial(), s.ObtenerParrafoFinal(), s.ObtenerPosicionFinal());
            }
            else
            {
                _documentoEdicion.CambiarFormato(formato, parrafoSeleccionado, posicionInsercion, parrafoSeleccionado, posicionInsercion);
                _estadisticas.RegistrarCambioFormato(formato, parrafoSeleccionado, posicionInsercion, parrafoSeleccionado, posicionInsercion);
            }
        }
        public Formato ObtenerFormatoComunSeleccion()
        {
            Seleccion s = ObtenerSeleccion();
            if (s != null)
            {
                return _documentoEdicion.ObtenerFormatoComun(s.ObtenerParrafoInicial(), s.ObtenerPosicionInicial(), s.ObtenerParrafoFinal(), s.ObtenerPosicionFinal());
            }
            return null;
        }
        public void AplicarNegrilla()
        {
            bool valor = true;
            if (anteriorFormato != null && anteriorFormato.Negrilla.HasValue && anteriorFormato.Negrilla.Value)
            {
                valor = false;
            }
            Formato f = Formato.CrearNegrilla(valor);
            AplicarFormato(f);
            anteriorFormato = f;
        }
        public void AplicarCursiva()
        {
            bool valor = true;
            if (anteriorFormato!=null&&anteriorFormato.Cursiva.HasValue && anteriorFormato.Cursiva.Value)
            {
                valor = false;
            }
            Formato f = Formato.CrearCursiva(valor);
            AplicarFormato(f);
            anteriorFormato = f;
        }
        public void AplicarSubrayado()
        {
            bool valor = true;
            if (anteriorFormato != null && anteriorFormato.Subrayado.HasValue && anteriorFormato.Subrayado.Value)
            {
                valor = false;
            }
            Formato f = Formato.CrearSubrayado(valor);
            AplicarFormato(f);
            anteriorFormato = f;
        }
        public void AgrandarLetra()
        {
            Formato f = Formato.CrearEscalaLetra(1.1f);
            AplicarFormato(f);
            anteriorFormato = f;
        }
        public void ReducirLetra()
        {
            Formato f = Formato.CrearEscalaLetra(1/1.1f); 
            AplicarFormato(f);
            anteriorFormato = f;
        }
        public void CambiarColorLetra(ColorDocumento nuevoColor)
        {
            Formato f = Formato.CrearColorLetra(nuevoColor);
            AplicarFormato(f);
            anteriorFormato = f;
        }
        public void CambiarColorFondo(ColorDocumento nuevoColorFondo)
        {
            Formato f = Formato.CrearColorFondo(nuevoColorFondo);
            AplicarFormato(f);
            anteriorFormato = f;
        }

        public void CambiarLetra(string familia, Medicion tamLetra)
        {
            Formato f = Formato.CrearLetra(familia, tamLetra);
            AplicarFormato(f);
            anteriorFormato = f;
        }
        public void CambiarTipoLetra(string familia)
        {
            Formato f = Formato.CrearTipoLetra(familia);
            AplicarFormato(f);
            anteriorFormato = f;
        }
        public void CambiarTamLetra(Medicion tamLetra)
        {
            Formato f = Formato.CrearTamLetra(tamLetra);
            AplicarFormato(f);
            anteriorFormato = f;
        }
        private void AplicarOperacionParrafos(Action<Parrafo> accion)
        {
            Seleccion s = ObtenerSeleccion();
            if (s == null)
            {
                accion(parrafoSeleccionado);
                _estadisticas.RegistrarCambioFormatoParrafo(parrafoSeleccionado);
            }
            else
            {
                _documentoEdicion.AplicarOperacionParrafos(s.ObtenerParrafoInicial(), s.ObtenerParrafoFinal(),
                    delegate(Parrafo p) {
                        accion(p);
                        _estadisticas.RegistrarCambioFormatoParrafo(p);
                    });
            }
        }
        
        public void AlinearIzquierda()
        {
            AplicarOperacionParrafos(x => x.AlinearIzquierda());
        }

        public void AlinearCentro()
        {
            AplicarOperacionParrafos(x => x.AlinearCentro());
        }

        public void AlinearDerecha()
        {
            AplicarOperacionParrafos(x => x.AlinearDerecha());
        }

        internal void AumentarInterlineado()
        {
            AplicarOperacionParrafos(x => x.AumentarInterlineado());
        }

        internal void DisminuirInterlineado()
        {
            AplicarOperacionParrafos(x => x.DisminuirInterlineado());
        }
    }
}
