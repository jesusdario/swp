using System;
using System.Collections.Generic;
using System.Text;
using SWPEditor.Dominio;
using System.Diagnostics;

namespace SWPEditor.IU.PresentacionDocumento
{
    
    public class ListaPaginas : IObservadorDocumento
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
                for (int i = _Paginas.Count - 2; i >= 0; i--)
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
                    while (_Paginas.Count <= indiceSolicitado)
                    {
                        if (indiceSolicitado < _Paginas.Count) break;
                        Pagina p = new Pagina(actual.LineaInicio + actual.Cantidad, this, _Lineas);
                        if (p.Cantidad != 0)
                        {
                            _Paginas.Add(p);
                            actual = p;
                        }
                        else
                        {
                            _listaCompleta = true;
                            break;
                        }
                    }
                }
            }
        }
        private Pagina ObtenerSiguiente(int indice)
        {
            if (_listaCompleta && indice >= _Paginas.Count)
                return null;
            AsegurarExistencia(indice);
            if (_listaCompleta && indice >= _Paginas.Count)
                return null;

            return _Paginas[indice];
        }
        public ListaPaginas(Documento documento, DocumentoImpreso docimpreso)
        {
            _docimpreso = docimpreso;
            _documento = documento;
            _Paginas = new List<Pagina>();
            _documento.AgregarObservador(this);
        }
        public void Iniciar(ListaLineas listalineas)
        {
            _Lineas = listalineas;
            Pagina p = new Pagina(0, this, _Lineas);
            _Paginas.Add(p);

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
        /*
        private void CalcularSiguiente()
        {
            int ultimapagina = _Paginas.Count;
            Pagina anterior = _Paginas[_Paginas.Count - 1];
            
        }*/

        public Pagina Obtener(int indicePagina)
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
            if (inicial.Parrafo == parrafoBuscado || inicial.Parrafo.EsSiguiente(parrafoBuscado))
            {
                int indice = _Paginas.Count - 1;
                int lineabusqueda = -1;

                //Hacer búsqueda lineal, puesto que aún no se ha calculado
                IEnumerable<Pagina> pags = ObtenerDesde(_Paginas.Count - 1);
                bool salirbucle = false;
                foreach (Pagina p in pags)
                {
                    int lim = p.LineaInicio + p.Cantidad;
                    for (int q = p.LineaInicio; q < lim; q++)
                    {
                        Linea l = _Lineas.Obtener(q);
                        if (l.Parrafo == parrafoBuscado)
                        {
                            lineabusqueda = q;
                            salirbucle = true;
                            break;
                        }
                        else if (l.Parrafo.EsSiguiente(parrafoBuscado))
                        {
                            //continuar
                        }
                        else
                        {
                            lineabusqueda = q;
                            salirbucle = true;
                            break;
                        }
                    }
                    if (salirbucle) break;
                    indice++;
                }
                if (lineabusqueda == -1)
                    throw new Exception("Párrafo no encontrado");
                int linea = lineabusqueda;//BuscarLineaInicioParrafo(lineabusqueda, parrafoBuscado);
                Posicion pos = new Posicion(_docimpreso);

                _docimpreso.Completar(pos, indice, linea, 0);
                Debug.Assert(pos.Linea != null);
                return pos;
            }
            else
            {
                Posicion pos = BuscarParrafo(parrafoBuscado, 0, _Paginas.Count - 1);
                Debug.Assert(pos.Linea != null);
                return pos;
            }
        }
        Posicion BuscarParrafo(Parrafo parrafoBuscado, int paginainicio, int paginafin)
        {
            if (paginainicio > paginafin)
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

        public bool EsUltimaPagina(int indicePagina)
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
        private int BuscarPaginaExistenteLinea(int linea,int inicio,int fin)
        {
            if (inicio > fin)
            {
                return -1;
            }
            int centro = (inicio + fin) / 2;
            if (_Paginas[centro].ContieneLinea(linea))
            {
                return centro;
            }
            else if (linea<_Paginas[centro].LineaInicio)
            {
                return BuscarPaginaExistenteLinea(linea, inicio, centro - 1);
            }
            else
            {
                return BuscarPaginaExistenteLinea(linea, centro+1,fin);
            }

        }
        #region Miembros de IObservadorDocumento
        private void Recalcular(Parrafo p)
        {
            Parrafo busq = p;
            int indicelinea = 0;
            int indicepagina = 0;
            indicelinea = _Lineas.Recalcular(p, p);
            if (_Paginas.Count > 0)
            {
                Pagina act;
                indicelinea = Math.Max(indicelinea - 1, 0);//Recalcular desde una linea antes (caso de inicio de página, y linea que disminuye de tamaño hasta entrar en página anterior
                indicepagina = BuscarPaginaExistenteLinea(indicelinea, 0, _Paginas.Count - 1);
                if (indicepagina == -1)
                {
                    indicepagina = 0;
                    act = _Paginas[0];
                    while (indicepagina < _Paginas.Count - 1 && indicelinea > act.LineaInicio + act.Cantidad)
                    {
                        indicepagina++;
                        act = _Paginas[indicepagina];
                    }
                }
                else
                {
                    act = _Paginas[indicepagina];
                }
                int inicioPagina = act.LineaInicio;
                int finPagina = act.LineaInicio + act.Cantidad;

                if (inicioPagina <= indicelinea && indicelinea < finPagina)
                {
                    act.Recalcular(indicelinea);
                    _Paginas.RemoveRange(indicepagina + 1, _Paginas.Count - indicepagina - 1);
                    _listaCompleta = false;

                }
                else
                {
                    if (indicepagina == _Paginas.Count - 1)
                    {
                        _listaCompleta = false;
                    }
                }
            }
            else
            {
                _listaCompleta = false;
            }        
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
