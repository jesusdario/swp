using System;
using System.Collections.Generic;
using System.Text;
using SWPEditor.IU.Graficos;
using SWPEditor.Dominio;
using SWPEditor.IU.PresentacionDocumento;
using SWPEditor.Aplicacion;
using System.Diagnostics;
using SWPEditor.Dominio.TextoFormato;

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
        BrochaSolida fondo;
        ColorDocumento _colorFondo;
        public ColorDocumento ColorFondo
        {
            get
            {
                return _colorFondo;
            }
            set
            {
                _colorFondo = value;
                fondo = new BrochaSolida(_colorFondo);
            }
        }
        public Escritorio(Documento _documento, IGraficador graficadorConsultas)
        {
            if (graficadorConsultas == null)
                throw new Exception("Debe indicarse un objeto graficador para efectuar consultas");
            ColorFondo = new SWPEditor.Dominio.TextoFormato.ColorDocumento(50, 25, 25);
            _graficadorConsultas = graficadorConsultas;
            AsegurarGraficador();
            ContPresentarDocumento controlador = new ContPresentarDocumento(_documento);
            EspacioEntrePaginas = new Medicion(10,Unidad.Milimetros);
            ControlDocumento = controlador;
            EsquinaSuperior = new Punto(Medicion.Cero, Medicion.Cero);            
        }
        public void Dibujar(IGraficador graficador,Seleccion seleccion,bool dibujarCursor)
        {
            AsegurarGraficador();
            Posicion pos = ControlDocumento.ObtenerPosicion();
            AsegurarVisibilidad(pos);
            Medicion inicio = Medicion.Cero-EsquinaSuperior.Y;
            Medicion derecha = Medicion.Cero - EsquinaSuperior.X;
            int i=PaginaSuperior;
            IEnumerable<Pagina> pags = _Documento.ObtenerDesde(PaginaSuperior);
            graficador.RellenarRectangulo(fondo, Punto.Origen, Dimensiones);
            foreach (Pagina p in pags) {
                LienzoPagina l = new LienzoPagina(i,new Punto(derecha,inicio));
                l.Dibujar(graficador, _Documento, pos, seleccion, dibujarCursor);
                if (inicio > Dimensiones.Alto+EsquinaSuperior.Y)
                {
                    return;
                }
                inicio += p.Dimensiones.Alto + EspacioEntrePaginas;
                i++;
            }
        }
        
        public bool EnRango(Punto esquinaSuperior, TamBloque tamaño,Punto puntoTest)
        {
            return puntoTest.X >= esquinaSuperior.X && (puntoTest.X < esquinaSuperior.X + tamaño.Ancho)
                && puntoTest.Y >= esquinaSuperior.Y && (puntoTest.Y < esquinaSuperior.Y + tamaño.Alto);
        }
        public void AsegurarVisibilidadMargen(Posicion posicion)
        {
            AsegurarGraficador();
            TamBloque margen = posicion.ObtenerMargenEdicion();
            Punto pt = posicion.PosicionPagina;
            Punto arribaizq = pt.Agregar(Medicion.Cero - margen.Ancho, Medicion.Cero - margen.Alto);
            Punto abajoder = pt.Agregar(margen.Ancho, margen.Alto + posicion.AltoLinea);
            AsegurarVisibilidadPuntoPagina(posicion.IndicePagina,abajoder);
            AsegurarVisibilidadPuntoPagina(posicion.IndicePagina, arribaizq);
        }
        private void IrPaginaAnterior()
        {
            if (PaginaSuperior>0) {
                Pagina p=_Documento.ObtenerPagina(PaginaSuperior-1);
                if (p == null)
                {
                    PaginaSuperior--;
                    p = _Documento.ObtenerPagina(PaginaSuperior - 1);
                    return;
                }
                Medicion deltatot = Medicion.Cero;
                Medicion delta = p.Dimensiones.Alto + EspacioEntrePaginas;
                EsquinaSuperior = new Punto(EsquinaSuperior.X, EsquinaSuperior.Y + delta);
                PaginaSuperior--;
           
            }
        }
        private Medicion IrPaginaSiguiente()
        {
            Pagina p = _Documento.ObtenerPagina(PaginaSuperior + 1);
            if (p == null) return Medicion.Cero;
            PaginaSuperior = PaginaSuperior + 1;
            Medicion delta = EsquinaSuperior.Y-(p.Dimensiones.Alto + EspacioEntrePaginas);
            EsquinaSuperior = new Punto(EsquinaSuperior.X, EsquinaSuperior.Y+delta);
            return delta;
        }
        private void IncrementarEsquinaY(Medicion delta)
        {
            EsquinaSuperior = EsquinaSuperior.AgregarY(delta);
        }
        private void IncrementarEsquinaX(Medicion delta)
        {
            EsquinaSuperior = EsquinaSuperior.AgregarX(delta);
        }
        private void AsegurarVisibilidadX(Punto pt)
        {
            if (pt.X < EsquinaSuperior.X)
            {
                Medicion delta = pt.X - EsquinaSuperior.X;
                IncrementarEsquinaX(delta);
            }
            if (pt.X > EsquinaSuperior.X + Dimensiones.Ancho)
            {
                Medicion delta = pt.X - (EsquinaSuperior.X + Dimensiones.Ancho);
                IncrementarEsquinaX(delta);
            }
        }
        private Pagina ObtenerPaginaSuperior()
        {
            return _Documento.ObtenerPagina(PaginaSuperior);
        }
        private Punto ObtenerFinEscritorioY()
        {
            return EsquinaSuperior.AgregarY(Dimensiones.Alto);
        }
        private void AsegurarVisibilidadPagina(int numpagina,Punto pt)
        {
            Debug.Assert(PaginaSuperior <= numpagina);
            IEnumerable<Pagina> pags = _Documento.ObtenerDesde(PaginaSuperior);
            Medicion inicioPagina = Medicion.Cero-EsquinaSuperior.Y;
            int indice = PaginaSuperior;
            Pagina paginaSuperior=Documento.ObtenerPagina(numpagina);
            foreach (Pagina p in pags)
            {
                while (inicioPagina > Dimensiones.Alto)
                {
                    Medicion delta;
                    delta = inicioPagina - Dimensiones.Alto;
                    IncrementarEsquinaY(delta);
                    inicioPagina = inicioPagina - delta;
                    Pagina superior = ObtenerPaginaSuperior();
                    while (superior!=null&&EsquinaSuperior.Y > superior.Dimensiones.Alto + EspacioEntrePaginas)
                    {
                        Medicion altoact = ObtenerPaginaSuperior().Dimensiones.Alto + EspacioEntrePaginas;
                        PaginaSuperior = PaginaSuperior + 1;
                        IncrementarEsquinaY(Medicion.Cero-altoact);
                        inicioPagina -= altoact;
                        superior = ObtenerPaginaSuperior();
                    }
                }
                Medicion finPagina = inicioPagina + p.Dimensiones.Alto;
                if (indice == numpagina)
                {
                    pt = pt.AgregarY(inicioPagina);//calcular coordenadas de punto en relacion a escritorio
                    if (pt.Y < Medicion.Cero)
                    {
                        Medicion delta = pt.Y;
                        EsquinaSuperior = new Punto(EsquinaSuperior.X, EsquinaSuperior.Y + delta);
                        pt = pt.AgregarY(Medicion.Cero-delta);
                        AsegurarVisibilidadX(pt);
                        return;
                    }
                    if (pt.Y > Dimensiones.Alto)
                    {
                        Medicion delta = pt.Y-Dimensiones.Alto;
                        EsquinaSuperior = new Punto(EsquinaSuperior.X, EsquinaSuperior.Y + delta);
                        pt = pt.AgregarY(Medicion.Cero - delta);
                        AsegurarVisibilidadX(pt);
                        return;
                    }
                    AsegurarVisibilidadX(pt);
                    return;
                }
                inicioPagina = finPagina + EspacioEntrePaginas;
                indice++;
            }
        }
        public void AsegurarVisibilidad(Posicion posicion)
        {
            TamBloque pos = posicion.ObtenerMargenEdicion();
            if (LineaAnterior != posicion.IndiceLinea)
            {
                AsegurarVisibilidadPuntoPagina(posicion.IndicePagina, posicion.PosicionPagina);
            }
            AsegurarVisibilidadMargen(posicion);
        }
        void AsegurarVisibilidadPuntoPagina(int numpagina,Punto pt)
        {
            AsegurarGraficador();
            /*   
             *     -----
             * 
             * ->
             *     -----
             *     -----
             * 
             * 
             *     -----
             *     -----
             * 
             * 
             *     -----
             * 
             * */
            while (numpagina < PaginaSuperior) //No es visible la página superior, disminuir una página
            {
                IrPaginaAnterior();                
            }
            AsegurarVisibilidadPagina(numpagina, pt);
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
            if (punto.Y < Medicion.Cero&&indice>0)
            {
                indice--;
                pt2.Y -= _Documento.ObtenerPagina(indice).Dimensiones.Alto + EspacioEntrePaginas;
            }
            if (punto.Y > Dimensiones.Alto)
            {
                indice++;
                Pagina p=_Documento.ObtenerPagina(indice);
                if (p!=null) {
                    pt2.Y -= p.Dimensiones.Alto + EspacioEntrePaginas;                    
                } else
                    indice--;
            }
            ControlDocumento.RegistrarPosicion(indice, pt2, ampliarSeleccion);
        }       
    }
    public class DistribucionPaginas
    {
        
        public Punto ObtenerPuntoEscritorio(int pagina, Punto punto)
        {
            return Punto.Origen;
        }
        public Punto ObtenerPuntoPagina(Punto posicionEscritorio)
        {
            return Punto.Origen;
        }
        public void AsegurarVisibilidadPuntoPagina(int pagina, Punto punto)
        {
            
        }        
    }
}
