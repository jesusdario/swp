using System;
using System.Collections.Generic;
using System.Text;
using SWPEditor.IU.Graficos;
using SWPEditor.Dominio;
using SWPEditor.IU.PresentacionDocumento;
using SWPEditor.Aplicacion;

namespace SWPEditor.IU.VistaDocumento
{
    public class Escritorio
    {
        public List<LienzoPagina> _Lienzos = new List<LienzoPagina>();
        public TamBloque Dimensiones { get; set; }
        private DocumentoImpreso _Documento
        {
            get
            {
                return _ControlDocumento.Documento;
            }
        }
        public DocumentoImpreso Documento { 
            get {
                AsegurarGraficador();
                return _ControlDocumento.Documento; 
            } 
        }
        private Punto EsquinaSuperior { get; set; }
        private int PaginaSuperior { get; set; }
        private int LineaAnterior { get; set; }
        private Medicion EspacioEntrePaginas { get; set; }
        private ContPresentarDocumento _ControlDocumento;
        public ContPresentarDocumento ControlDocumento { 
            get {
                AsegurarGraficador();
                return _ControlDocumento; 
            } 
            private set { 
                _ControlDocumento = value; 
            } 
        }
        private void AsegurarGraficador()
        {
            if (Estilo.GraficadorConsultas != _graficadorConsultas)
                Estilo.GraficadorConsultas = _graficadorConsultas;
        }
        public event EventHandler ActualizarPresentacion
        {
            add
            {
                ControlDocumento.ActualizarPresentacion += value;
            }
            remove {
                ControlDocumento.ActualizarPresentacion -= value;
            }
        }
        private IGraficador _graficadorConsultas;
        public Escritorio(Documento _documento,IGraficador graficadorConsultas)
        {
            if (graficadorConsultas == null)
                throw new Exception("Debe indicarse un objeto graficador para efectuar consultas");
            _graficadorConsultas = graficadorConsultas;
            AsegurarGraficador();
            ContPresentarDocumento controlador = new ContPresentarDocumento(_documento);
            EspacioEntrePaginas = new Medicion(10,Unidad.Milimetros);
            ControlDocumento = controlador;
            EsquinaSuperior = new Punto(Medicion.Cero, Medicion.Cero);
        }
        public void Dibujar(IGraficador graficador,Seleccion seleccion)
        {
            AsegurarGraficador();
            Posicion pos = ControlDocumento.ObtenerPosicion();
            AsegurarVisibilidad(pos);
            Medicion inicio = EsquinaSuperior.Y;
            Medicion derecha = EsquinaSuperior.X;
            int i=PaginaSuperior;
            IEnumerable<Pagina> pags = _Documento.ObtenerDesde(PaginaSuperior);
            foreach (Pagina p in pags) {
                LienzoPagina l = new LienzoPagina(i,new Punto(derecha,inicio));
                l.Dibujar(graficador, _Documento, pos, seleccion);
                if (Medicion.Cero-inicio > Dimensiones.Alto+EsquinaSuperior.Y)
                {
                    return;
                }
                inicio -= p.Dimensiones.Alto + EspacioEntrePaginas;
                i++;
            }
        }
        
        public bool EnRango(Punto esquinaSuperior, TamBloque tamaño,Punto puntoTest)
        {
            return puntoTest.X >= esquinaSuperior.X && (puntoTest.X < esquinaSuperior.X + tamaño.Ancho)
                && puntoTest.Y >= esquinaSuperior.Y && (puntoTest.Y < esquinaSuperior.Y + tamaño.Alto);
        }
        /// <summary>
        /// Asegura la visibilidad de un punto en la página.
        /// </summary>
        /// <param name="pt"></param>
        private void AsegurarVisibilidadPunto(Punto pt)
        {
            if (pt.Y > EsquinaSuperior.Y + Dimensiones.Alto)
            {
                EsquinaSuperior = new Punto(EsquinaSuperior.X, pt.Y - Dimensiones.Alto);
            }
            if (pt.Y < EsquinaSuperior.Y)
            {
                EsquinaSuperior = new Punto(EsquinaSuperior.X, pt.Y);
            }

            if (pt.X > EsquinaSuperior.X + Dimensiones.Ancho)
            {
                EsquinaSuperior = new Punto(pt.X - Dimensiones.Ancho, EsquinaSuperior.Y);
            }
            if (pt.X < EsquinaSuperior.X)
            {
                EsquinaSuperior = new Punto(pt.X, EsquinaSuperior.Y);
            }
        }
        public void AsegurarVisibilidadMargen(Posicion posicion)
        {
            AsegurarGraficador();
            TamBloque margen = posicion.ObtenerMargenEdicion();
            Punto pt = posicion.PosicionPagina;
            Punto arribaizq = pt.Agregar(Medicion.Cero - margen.Ancho, Medicion.Cero - margen.Alto);
            Punto abajoder = pt.Agregar(margen.Ancho, margen.Alto + posicion.AltoLinea);
            AsegurarVisibilidadPunto(abajoder);
            AsegurarVisibilidadPunto(arribaizq);
        }
        
        public void AsegurarVisibilidad(Posicion posicion)
        {
            AsegurarGraficador();
            int numpagina = posicion.IndicePagina;
            Punto pt = posicion.PosicionPagina;
            
            if (posicion.IndicePagina != PaginaSuperior)
            {
                if (posicion.IndicePagina < PaginaSuperior)
                {
                    PaginaSuperior = posicion.IndicePagina;
                    EsquinaSuperior = new Punto(EsquinaSuperior.X, Dimensiones.Alto);
                    AsegurarVisibilidadPunto(new Punto(Medicion.Cero, Dimensiones.Alto));
                }
                else
                {
                    PaginaSuperior = posicion.IndicePagina;
                    EsquinaSuperior = new Punto(EsquinaSuperior.X, Medicion.Cero);
                    AsegurarVisibilidadPunto(new Punto(Medicion.Cero, Medicion.Cero));
                }
                AsegurarVisibilidad(posicion);
            }
            else
            {
                
                TamBloque pos = posicion.ObtenerMargenEdicion();
                if (LineaAnterior != posicion.IndiceLinea)
                {
                    AsegurarVisibilidadPunto(new Punto(posicion.Pagina.Margen.Izquierdo,pt.Y+posicion.AltoLinea));
                }
                AsegurarVisibilidadMargen(posicion);
            }
            LineaAnterior = posicion.IndiceLinea;
        }

        public void IrAPosicion(Punto punto, bool ampliarSeleccion)
        {
            AsegurarGraficador();
            Punto pt2 = punto + EsquinaSuperior;
            int pagsig = PaginaSuperior;
            int indice = PaginaSuperior;
            IEnumerable<Pagina> pags = _Documento.ObtenerDesde(PaginaSuperior);
            foreach (Pagina pag in pags)
            {
                if (pt2.Y > pag.Dimensiones.Alto + EspacioEntrePaginas)
                {
                    pt2.Y -= pag.Dimensiones.Alto + EspacioEntrePaginas;
                }
                else
                    break;
                indice++;
            }
            ControlDocumento.RegistrarPosicion(indice, pt2, ampliarSeleccion);
        }       
    }
}
