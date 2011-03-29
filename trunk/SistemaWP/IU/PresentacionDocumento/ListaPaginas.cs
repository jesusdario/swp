using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SistemaWP.Dominio;
using System.Diagnostics;

namespace SistemaWP.IU.PresentacionDocumento
{
    public class ListaPaginas:IObservadorDocumento
    {
        List<Pagina> _Paginas;
        ListaLineas _Lineas;
        bool _listaCompleta;
        Documento _documento;
        DocumentoImpreso _docimpreso;
        public IEnumerable<Pagina> ObtenerDesde(int numpaginaInicio, int cantidad)
        {

            int inicio = numpaginaInicio;
            List<Pagina> pag = new List<Pagina>();
            lock (_documento)
            {
                for (int i = inicio; i < cantidad; i++)
                {
                    Pagina p = ObtenerSiguiente(i);
                    if (p == null) break;
                    yield return p;
                }
            }
        }
        public IEnumerable<Pagina> ObtenerDesde(int paginaBase)
        {
            Pagina p;
            int act = paginaBase;
            lock (_documento)
            {
                do
                {
                    p = ObtenerSiguiente(act);
                    if (p == null) break;
                    act++;
                    yield return p;
                } while (p != null);
            }
        }
        internal Medicion ObtenerAnchoLinea(int numlinea)
        {
            Pagina act = _Paginas[_Paginas.Count - 1];
            if (numlinea < act.LineaInicio)
            {
                for (int i = _Paginas.Count - 2; i >= 0; i++)
                {
                    if (_Paginas[i].ContieneLinea(numlinea))
                    {
                        return _Paginas[i].ObtenerAnchoLinea(numlinea);
                    }
                }
            }
            return act.ObtenerAnchoLinea(numlinea);
        }
        private void AsegurarExistencia(int indiceSolicitado)
        {
            if (!_listaCompleta)
            {
                if (indiceSolicitado >= _Paginas.Count - 1)
                {
                    Pagina actual = _Paginas[_Paginas.Count - 1];
                    if (!actual.Completa)
                    {
                        Medicion inicio = actual.AltoActual;
                        Medicion altoPagina = actual.ObtenerAltoLineas();
                        int indice = actual.LineaInicio + actual.Cantidad;
                        IEnumerable<Linea> lista = _Lineas.ObtenerDesde(indice);
                        bool existente = true;
                        foreach (Linea l in lista)
                        {
                            if (indiceSolicitado>_Paginas.Count-1)
                            {
                                break;
                            }
                            Medicion anterior = inicio;
                            inicio = inicio + l.AltoLinea;
                            if (inicio > altoPagina)
                            {
                                Pagina nueva = new Pagina();
                                nueva.LineaInicio = actual.LineaInicio + actual.Cantidad;
                                if (!existente)
                                    _Paginas.Add(nueva);
                                existente = false;
                                actual.Completa = true;
                                actual = nueva;
                                nueva.AltoActual = Medicion.Cero;
                                inicio = l.AltoLinea;
                            }
                            actual.AltoActual += l.AltoLinea;
                            actual.Cantidad++;
                        }
                        if (!existente)
                        {
                            _Paginas.Add(actual);
                        }
                        actual.Completa = true;
                        _listaCompleta = true;
                    }
                }
            }
        }
        private Pagina ObtenerSiguiente(int indice)
        {
            if (_listaCompleta && indice >= _Paginas.Count)
                return null;
            AsegurarExistencia(indice);
            return _Paginas[indice];
        }
        public ListaPaginas(Documento documento, DocumentoImpreso docimpreso)
        {
            _docimpreso = docimpreso;
            _documento = documento;
            _Paginas = new List<Pagina>();
            Pagina p = new Pagina();
            _Paginas.Add(p);
            _documento.AgregarObservador(this);
        }
        public void Iniciar(ListaLineas listalineas)
        {
            _Lineas = listalineas;
        }
        public Medicion ObtenerAncho(int numlinea)
        {
            int suma = 0;
            for (int i = 0; i < _Paginas.Count; i++)
            {
                Pagina act = _Paginas[i];
                suma += act.Cantidad;
                if (suma > numlinea)
                {
                    return act.ObtenerAnchoLinea(numlinea);
                }
            }
            Pagina ultimapag = _Paginas[_Paginas.Count - 1];
            return ultimapag.ObtenerAnchoLinea(numlinea);
        }
        public void CalcularSiguiente()
        {
            int ultimapagina = _Paginas.Count;
            Pagina anterior = _Paginas[_Paginas.Count - 1];
            Pagina nueva = new Pagina();
        }

        internal Pagina Obtener(int indicePagina)
        {
            AsegurarExistencia(indicePagina);
            if (_listaCompleta && indicePagina >= _Paginas.Count)
                return null;
            return _Paginas[indicePagina];
        }


        public Posicion BuscarParrafo(Parrafo parrafoBuscado)
        {
            Pagina pag = _Paginas[_Paginas.Count - 1];
            Linea inicial = _Lineas.Obtener(pag.LineaInicio);
            if (inicial.Parrafo.EsSiguiente(parrafoBuscado))
            {
                int indice = _Paginas.Count - 1;
                int lineabusqueda = -1;

                //Hacer búsqueda lineal, puesto que aún no se ha calculado
                IEnumerable<Pagina> pags = ObtenerDesde(_Paginas.Count - 1);
                bool salirbucle=false;
                foreach (Pagina p in pags)
                {
                    int lim = p.LineaInicio + p.Cantidad;
                    for (int q = p.LineaInicio; q < lim; q++)
                    {
                        Linea l = _Lineas.Obtener(q);
                        if (l.Parrafo == parrafoBuscado)
                        {
                            lineabusqueda = p.LineaInicio;
                            salirbucle = true;
                            break;
                        }
                        else if (l.Parrafo.EsSiguiente(parrafoBuscado))
                        {
                            //continuar
                        }
                        else
                        {
                            lineabusqueda = p.LineaInicio;
                            salirbucle = true;
                            break;
                        }
                    }
                    if (salirbucle) break;
                    indice++;
                }
                if (lineabusqueda == -1) throw new Exception("Párrafo no encontrado");
                int linea = BuscarLineaInicioParrafo(lineabusqueda, parrafoBuscado);
                Posicion pos = new Posicion(_docimpreso);
                
                _docimpreso.Completar(pos, indice, linea, 0);
                Debug.Assert(pos.Linea != null);
                return pos;
            }
            else
            {
                Posicion pos= BuscarParrafo(parrafoBuscado, 0, _Paginas.Count - 1);
                Debug.Assert(pos.Linea != null);
                return pos;
            }
        }
        Posicion BuscarParrafo(Parrafo parrafoBuscado, int paginainicio, int paginafin)
        {
            if (paginainicio >= paginafin)
            {
                int centro2 = Math.Max(paginainicio, paginafin);
            reintentar:
                if (centro2 >= _Paginas.Count)
                    centro2 = _Paginas.Count - 1;
                Pagina elemento2 = _Paginas[centro2];
                int linea = BuscarLineaInicioParrafo(elemento2.LineaInicio, parrafoBuscado);
                Posicion p = new Posicion(_docimpreso);
                _docimpreso.Completar(p, centro2, linea, 0);
                return p;

            }
            int centro = paginainicio + paginafin;
            Pagina elemento = _Paginas[centro];
            Linea inicial = _Lineas.Obtener(elemento.LineaInicio);
            if (inicial.Parrafo == parrafoBuscado)
            {
                int linea = BuscarLineaInicioParrafo(elemento.LineaInicio, parrafoBuscado);
                Posicion p = new Posicion(_docimpreso);
                _docimpreso.Completar(p, centro, linea, 0);
                return p;
                //return CrearPosicion(centro, elemento.LineaInicio, 0);
            }
            else
            {
                if (inicial.Parrafo.EsSiguiente(parrafoBuscado))
                {
                    return BuscarParrafo(parrafoBuscado, centro + 1, paginafin);
                }
                else
                {
                    return BuscarParrafo(parrafoBuscado, paginainicio, centro - 1);
                }
            }
        }
        private int BuscarLineaInicioParrafo(int lineaInicio, Parrafo p)
        {
            return _Lineas.BuscarInicialDeParrafo(lineaInicio, p);
        }

        internal bool EsUltimaPagina(int indicePagina)
        {
            if (indicePagina < _Paginas.Count - 1)
            {
                return false;
            }
            else
            {
                if (_listaCompleta && indicePagina == _Paginas.Count - 1)
                    return true;
                AsegurarExistencia(indicePagina);
                if (_listaCompleta && indicePagina == _Paginas.Count - 1)
                    return true;
                return false;
            }
        }

        #region Miembros de IObservadorDocumento
        private void Recalcular(Parrafo p)
        {
            Parrafo busq = p;
            Posicion pos = BuscarParrafo(busq);
            _Paginas.RemoveRange(pos.IndicePagina+1, _Paginas.Count - pos.IndicePagina-1);            
            Pagina act = _Paginas[_Paginas.Count - 1];
            act.Cantidad = pos.IndiceLinea-act.LineaInicio-1;
            act.AltoActual=Medicion.Cero;
            if (act.Cantidad < 0) act.Cantidad = 0;
            int lim = act.LineaInicio + act.Cantidad;
            for (int i = act.LineaInicio; i < lim; i++)
            {
                act.AltoActual += _Lineas.Obtener(i).AltoLinea;
            }
            act.Completa = false;
            _listaCompleta = false;
            _Lineas.Recalcular(pos.IndiceLinea, p);
        }
        public void ParrafoAgregado(Parrafo p)
        {
            Recalcular(p.Anterior ?? p.Siguiente);
        }

        public void ParrafoCambiado(Parrafo p)
        {
            Recalcular(p);
        }

        public void ParrafoEliminado(Parrafo p)
        {
            //Recalcular(p.Anterior ?? p.Siguiente);
        }

        

        public void ParrafosCambiados(Parrafo parrafoInicio, Parrafo parrafoFin)
        {
            Recalcular(parrafoInicio);
        }

        #endregion
    }
}
