using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SistemaWP.IU.Graficos;
using SistemaWP.Dominio;
using SistemaWP.IU.PresentacionDocumento;
using SistemaWP.Aplicacion;

namespace SistemaWP.IU.VistaDocumento
{
    class Escritorio
    {
        public List<LienzoPagina> _Lienzos = new List<LienzoPagina>();
        public TamBloque Dimensiones { get; set; }
        public DocumentoImpreso Documento { get { return Controlador.Documento; } }
        public Punto EsquinaSuperior { get; set; }
        public int PaginaSuperior { get; set; }
        private int LineaAnterior { get; set; }
        public Medicion EspacioEntrePaginas { get; set; }
        public ContPresentarDocumento Controlador { get; set; }
        
        public Escritorio(ContPresentarDocumento controlador)
        {
            EspacioEntrePaginas = new Medicion(10,Unidad.Milimetros);
            Controlador = controlador;
            EsquinaSuperior = new Punto(Medicion.Cero, Medicion.Cero);
        }
        public void Dibujar(IGraficador graficador,Seleccion seleccion)
        {
            Posicion pos = Controlador.ObtenerPosicion();
            AsegurarVisibilidad(pos);
            Medicion inicio = EsquinaSuperior.Y;
            Medicion derecha = EsquinaSuperior.X;
            int i=PaginaSuperior;
            IEnumerable<Pagina> pags=Documento.ObtenerDesde(PaginaSuperior);
            foreach (Pagina p in pags) {
                LienzoPagina l = new LienzoPagina(i,new Punto(derecha,inicio));
                l.Dibujar(graficador, Documento, pos, seleccion);
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
            TamBloque margen = posicion.ObtenerMargenEdicion();
            Punto pt = posicion.PosicionPagina;
            Punto arribaizq = pt.Agregar(Medicion.Cero - margen.Ancho, Medicion.Cero - margen.Alto);
            Punto abajoder = pt.Agregar(margen.Ancho, margen.Alto + posicion.AltoLinea);
            AsegurarVisibilidadPunto(abajoder);
            AsegurarVisibilidadPunto(arribaizq);
        }
        
        public void AsegurarVisibilidad(Posicion posicion)
        {
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

        internal void RegistrarPosicion(Punto punto, bool ampliarSeleccion)
        {
            Punto pt2 = punto + EsquinaSuperior;
            int pagsig = PaginaSuperior;
            int indice = PaginaSuperior;
            foreach (Pagina pag in Documento.ObtenerDesde(PaginaSuperior))
            {
                if (pt2.Y > pag.Dimensiones.Alto)
                {
                    pt2.Y -= pag.Dimensiones.Alto;
                }
                else
                    break;
                indice++;
            }
            Controlador.RegistrarPosicion(indice, pt2, ampliarSeleccion);
        }
    }
}
