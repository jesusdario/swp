using System;
using System.Collections.Generic;
using System.Text;
using SWPEditor.Aplicacion;
using System.Windows.Forms;
using SWPEditor.Dominio;
using SWPEditor.IU.Graficos;
using SWPEditor.Dominio.TextoFormato;

namespace SWPEditor.IU.PresentacionDocumento
{
    public class ContPresentarDocumento
    {
        ContEditarTexto conttexto;
        PresentacionDocumento.DocumentoImpreso _documento;
        public DocumentoImpreso Documento { get { return _documento; } }
        public event EventHandler ActualizarPresentacion;
        public ContPresentarDocumento(Documento documento)
        {
            conttexto = new ContEditarTexto(documento);
            conttexto.IndicarPosicion(1, 0, false);
            _documento = new SWPEditor.IU.PresentacionDocumento.DocumentoImpreso(documento);
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
                _documento.RevisarIntegridad();
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
                EnActualizarPresentacion(true);
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
                    cad = sel.ObtenerHtml();
                string cadini="<body class='e0'>";
                string cadini2 = "<!--StartFragment-->";
                string cadfin="</body>";
                string cadfin2 = "<!--EndFragment--></body>";
                string cnueva=cad.Replace(cadini, cadini2).Replace(cadfin,cadfin2);
                string cadbase = @"Version:1.0
    StartHTML:-1
    EndHTML:-1
    StartFragment:AAAAAAAAAA
    EndFragment:BBBBBBBBBB
<!DOCUMENT>";
                int indice1 = cnueva.IndexOf(cadini2)+cadbase.Length;
                int indice2 = cnueva.IndexOf(cadfin2) + cadfin2.Length + cadbase.Length;
                cadbase=cadbase
                    .Replace("AAAAAAAAAA",indice1.ToString().PadLeft(10,'0'))
                    .Replace("BBBBBBBBBB",indice2.ToString().PadLeft(10,'0'));
                cnueva=cadbase+cnueva;
                
                Clipboard.SetText(cnueva, TextDataFormat.Html);
                //Clipboard.SetText(cad, TextDataFormat.UnicodeText);
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
        public void IrLineaSuperior(bool ampliarSeleccion)
        {
            lock (this)
            {
                Posicion pos = ObtenerPosicion();
                Posicion posanterior = pos.ObtenerLineaSuperior();
                CambiarPosicion(posanterior, ampliarSeleccion);
                EnActualizarPresentacion(false);
            }
        }

        public void IrLineaInferior(bool ampliarSeleccion)
        {
            lock (this)
            {
                Posicion pos = ObtenerPosicion();
                Posicion possiguiente = pos.ObtenerLineaInferior();
                CambiarPosicion(possiguiente, ampliarSeleccion);
                EnActualizarPresentacion(false);
            }
        }
        public void IrPaginaAnterior(bool ampliarSeleccion)
        {
            lock (this)
            {
                Posicion pos = ObtenerPosicion();
                Posicion possiguiente = pos.ObtenerPaginaAnterior();
                CambiarPosicion(possiguiente, ampliarSeleccion);
                EnActualizarPresentacion(false);
            }
        }
        public void IrPaginaSiguiente(bool ampliarSeleccion)
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

        public Seleccion ObtenerSeleccion()
        {
            lock (this)
            {
                return conttexto.ObtenerSeleccion();
            }
        }

        public Posicion ObtenerPosicion()
        {
            lock (this)
            {
                return _documento.ObtenerPosicionCursor(0, conttexto.ObtenerParrafoSeleccionado(), conttexto.ObtenerPosicionInsercion());
            }
        }

        public void SeleccionarTodo()
        {
            lock (this)
            {
                conttexto.SeleccionarTodo();
                EnActualizarPresentacion(false);
            }
        }

        public string ObtenerTexto()
        {
            lock (this)
            {
                return conttexto.ObtenerTexto();
            }
        }

        public void CambiarLetraNegrilla()
        {
            conttexto.AplicarNegrilla();
            EnActualizarPresentacion(true);
        }

        public void CambiarLetraCursiva()
        {
            conttexto.AplicarCursiva();
            EnActualizarPresentacion(true);
        }

        public void CambiarLetraSubrayado()
        {
            conttexto.AplicarSubrayado();
            EnActualizarPresentacion(true);
        }

        public void AgrandarLetra()
        {
            conttexto.AgrandarLetra();
            EnActualizarPresentacion(true);
        }

        public void ReducirLetra()
        {
            conttexto.ReducirLetra();
            EnActualizarPresentacion(true);
        }

        public void QuitarSeleccion()
        {
            Seleccion s=conttexto.ObtenerSeleccion();
            conttexto.IndicarPosicion(s.ObtenerParrafoInicial().ID,s.ObtenerPosicionInicial(), false);
            EnActualizarPresentacion(false);
        }
        public void CambiarColorLetra(ColorDocumento color)
        {
            conttexto.CambiarColorLetra(color);
            EnActualizarPresentacion(false);
        }
        public void CambiarColorFondo(ColorDocumento color)
        {
            conttexto.CambiarColorFondo(color);
            EnActualizarPresentacion(false);
        }

        public void CambiarLetra(string familia, Medicion tamLetra)
        {
            conttexto.CambiarLetra(familia, tamLetra);
            EnActualizarPresentacion(true);
        }

        public void CambiarTamLetra(Medicion medicion)
        {
            conttexto.CambiarTamLetra(medicion);
            EnActualizarPresentacion(true);
        }

        public void AlinearIzquierda()
        {
            conttexto.AlinearIzquierda();
            EnActualizarPresentacion(false);
        }

        public void AlinearCentro()
        {
            conttexto.AlinearCentro();
            EnActualizarPresentacion(false);
        }

        public void AlinearDerecha()
        {
            conttexto.AlinearDerecha();
            EnActualizarPresentacion(false);
        }

        public void AumentarInterlineado()
        {
            conttexto.AumentarInterlineado();
            EnActualizarPresentacion(true);
        }
        public void DisminuirInterlineado()
        {
            conttexto.DisminuirInterlineado();
            EnActualizarPresentacion(true);
        }
    }
}
