using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace SWPEditor.Dominio.TextoFormato
{
    struct Texto
    {
        StringBuilder st;
        List<Bloque> _bloques;
        public void Iniciar()
        {
            st = new StringBuilder();
        }
        public int Length { get { return st.Length; } }
        public char this[int indice]
        {
            get
            {
                return st[indice];
            }
        }
        
        static Bloque _BloqueVacio = new Bloque(0,null);
        public Bloque ObtenerBloque(int indice)
        {
            if (_bloques == null&&indice==0)
            {
                if (st.Length == 0)
                {
                    return _BloqueVacio;
                }
                else
                {
                    return new Bloque(st.Length, null);
                }
            }
            return _bloques[indice];
        }
        public IEnumerable<Bloque> ObtenerRangoBloques(int inicio, int cantidad)
        {
            if (_bloques == null)
            {
                if (st.Length == 0)
                {
                    yield return _BloqueVacio;
                }
                else
                {
                    yield return new Bloque(cantidad, null);
                }
            }
            else
            {
                if (_bloques.Count == 1)
                {
                    yield return _bloques[0];
                    yield break;
                }
                int inicioBloque = 0;
                int finSeleccion=inicio+cantidad;
                int finBloque = 0;
                for (int i = 0; i < _bloques.Count; i++)
                {
                    Bloque actual=_bloques[i];
                    finBloque = inicioBloque + actual.Cantidad ;
                    if (inicio == inicioBloque&&finSeleccion == finBloque)
                    {
                        yield return actual;
                    }
                    else
                    {
                        bool inicioAnteriorBloque=inicio < inicioBloque;
                        bool finPosteriorBloque=finSeleccion > finBloque;
                        bool inicioEnBloque = inicio >= inicioBloque && inicio < finBloque;
                        bool finEnBloque = finSeleccion >= inicioBloque && finSeleccion <= finBloque;
                        if (inicioAnteriorBloque&&finPosteriorBloque)
                        {
                            yield return actual;
                        }                           
                        else
                        {
                            if (inicioEnBloque)
                            {
                                if (finEnBloque)
                                {
                                    Bloque b = actual.Clonar();
                                    b.CambiarCantidad(finSeleccion - inicio);
                                    yield return b;
                                }
                                else
                                {
                                    Bloque b = actual.Clonar();
                                    b.CambiarCantidad(finBloque - inicio);
                                    yield return b;
                                }
                            }
                            else
                            {
                                if (finEnBloque)
                                {
                                    Bloque b = actual.Clonar();
                                    b.CambiarCantidad(finSeleccion - inicioBloque);
                                    yield return b;
                                    yield break;
                                }
                            }
                        }
                    }
                    inicioBloque += actual.Cantidad;
                }
            }
        }
        internal string ToString(int inicio, int cantidad)
        {
            Debug.Assert(inicio >= 0 && inicio + cantidad >= 0 && inicio+cantidad <= Length&&cantidad>=0);
            return st.ToString(inicio, cantidad);
        }
        public override string ToString()
        {
            return st.ToString();
        }
        
        internal void Append(string cad)
        {
            st.Append(cad);
            ExtenderBloques(cad.Length);
        }
        private void ExtenderBloques(int delta)
        {
            if (_bloques != null)
            {
                _bloques[_bloques.Count - 1].IncrementarCantidad(delta);
#if DEBUG

                //Debug.Assert(_bloques.Sum(x => x.Cantidad) == Length); ;
#endif

            }
        }
        private void IncrementarBloque(int posicion, int delta)
        {
            if (_bloques != null)
            {
                int suma = 0;
                for (int i = 0; i < _bloques.Count; i++)
                {
                    suma += _bloques[i].Cantidad;
                    if (suma >= posicion)//||(_bloques[i].Cantidad==0&&suma==posicion))
                    {
                        int inc = i + 1;
                        while (inc < _bloques.Count && _bloques[inc].Cantidad==0)
                        {
                            inc++;
                        }
                        inc=inc-1;
                        _bloques[inc].IncrementarCantidad(delta);
                        return;
                    }
                }
                _bloques[_bloques.Count-1].IncrementarCantidad(delta);
#if DEBUG

                //Debug.Assert(_bloques.Sum(x => x.Cantidad) == Length); ;
#endif

            }
        }
        internal void Insert(int posicionInsercion, char caracter)
        {
            st.Insert(posicionInsercion, caracter);
            IncrementarBloque(posicionInsercion, 1);
            
#if DEBUG
            if (_bloques != null)
            {
                //Debug.Assert(_bloques.Sum(x => x.Cantidad) == Length); ;
            }
#endif

        }
        internal void Append(char caracter)
        {
            st.Append(caracter);
            ExtenderBloques(1);
#if DEBUG
            if (_bloques != null)
            {
                //Debug.Assert(_bloques.Sum(x => x.Cantidad) == Length); ;
            }
#endif

        }

        internal void Insert(int posicionInsercion, string cadena)
        {
            st.Insert(posicionInsercion, cadena);
            IncrementarBloque(posicionInsercion, cadena.Length);
#if DEBUG
            if (_bloques != null)
            {
                //Debug.Assert(_bloques.Sum(x => x.Cantidad) == Length); ;
            }
#endif        
        }
        internal void SimplificarFormato()
        {
            if (_bloques == null) return;
            for (int i = _bloques.Count-1; i >= 0; i--)
            {
                if (_bloques[i].Cantidad == 0)
                {
                    _bloques.RemoveAt(i);
                }
            }
#if DEBUG

            //Debug.Assert(_bloques.Sum(x => x.Cantidad) == Length); ;
#endif

        }
        internal void Remove(int posicion, int cantidad)
        {
            st.Remove(posicion, cantidad);
            if (_bloques!=null) {
                int primerbloque, ultimobloque;
                int deltaini, deltafin;
                ObtenerRango(posicion, cantidad,
                    out primerbloque, out deltaini,
                    out ultimobloque, out deltafin);
                if (primerbloque == ultimobloque)
                {
                    _bloques[primerbloque].DisminuirCantidad(deltafin - deltaini);
                }
                else
                {
                    _bloques[primerbloque].DisminuirCantidad((_bloques[primerbloque] .Cantidad - deltaini));
                    _bloques[ultimobloque].DisminuirCantidad(deltafin);
                    int bloquesremover = ultimobloque - 1 - primerbloque;
                    if (bloquesremover > 0)
                    {
                        _bloques.RemoveRange(primerbloque + 1, bloquesremover);
                    }
#if DEBUG

                    //Debug.Assert(_bloques.Sum(x => x.Cantidad) == Length); ;
#endif
                }
            }
        }

        private void ObtenerRango(int posicionini, int cantidad,
            out int primerbloque, out int deltaini, 
            out int ultimobloque, out int deltafin)
        {
            Debug.Assert(cantidad >= 0);
            int posicionfin = posicionini + cantidad;
            primerbloque = 0;
            ultimobloque = 0;
            int suma = 0;
            int sumaant = 0;
            deltaini = 0;
            deltafin = 0;
            bool bloqueini = false;
            for (int i = 0; i < _bloques.Count; i++)
            {
                sumaant = suma;
                suma = suma + _bloques[i].Cantidad;
                if (!bloqueini && posicionini < suma)
                {
                    deltaini = posicionini - sumaant;
                    bloqueini = true;
                    primerbloque = i;
                }
                if (posicionfin < suma)
                {
                    deltafin = posicionfin - sumaant;
                    ultimobloque = i;
                    return;
                    
                }            
            }
            if (!bloqueini)
            {
                deltaini = _bloques[_bloques.Count - 1].Cantidad;
                primerbloque = _bloques.Count - 1;
            }
            deltafin = _bloques[_bloques.Count - 1].Cantidad; 
            ultimobloque = _bloques.Count - 1;
        }
        private void AsegurarBloques()
        {
            if (_bloques == null)
            {
                _bloques = new List<Bloque>();
                _bloques.Add(new Bloque(Length, null));
            }
        }
        public void AplicarFormato(Formato formato, int inicio, int cantidad)
        {
            AsegurarBloques();
            int primerbloque, ultimobloque;
            int deltaini, deltafin;
            ObtenerRango(inicio, cantidad, out primerbloque, out deltaini, out ultimobloque, out deltafin);
            if (primerbloque == ultimobloque)
            {
                Bloque a = _bloques[primerbloque];
                Bloque b = new Bloque(deltafin - deltaini, a.Formato);
                b.FusionarFormato(formato);
                Bloque c = new Bloque(a.Cantidad - deltafin, a.Formato);
                a.DisminuirCantidad(a.Cantidad - deltaini);
                _bloques.Insert(primerbloque + 1, b);
                _bloques.Insert(primerbloque + 2, c);
            }
            else
            {
                Bloque a = _bloques[primerbloque];
                //disminuir el tamaño de los bloques e insertar nuevos bloques al medio combinados.
                if (a.Cantidad != deltaini)
                {
                    _bloques.Insert(primerbloque + 1, new Bloque(a.Cantidad - deltaini, a.Formato));
                    ultimobloque++;
                }
                a.DisminuirCantidad(a.Cantidad - deltaini);
                Bloque b = _bloques[ultimobloque];
                if (deltafin != 0)
                {
                    _bloques.Insert(ultimobloque, new Bloque(deltafin, b.Formato));
                    ultimobloque++;
                }
                b.DisminuirCantidad(deltafin);
                for (int i = primerbloque + 1; i < ultimobloque; i++)
                {
                    _bloques[i].FusionarFormato(formato);

                }
               
#if DEBUG
            
                //Debug.Assert(_bloques.Sum(x=>x.Cantidad)==Length);;
#endif
            }
        }

        internal Formato ObtenerFormatoComun(int posicionInicio, int cantidad)
        {
            throw new NotImplementedException();
        }

        internal int ObtenerNumBloques()
        {
            if (_bloques == null) return 1;
            return _bloques.Count;
        }

        internal void Agregar(Texto texto)
        {
            if (_bloques != null || texto._bloques != null)
            {
                AsegurarBloques();
                if (texto._bloques == null)
                {
                    Bloque b=_BloqueVacio.Clonar();
                    b.CambiarCantidad(texto.st.Length);
                    _bloques.Add(b);
                }
                else
                {
                    _bloques.AddRange(texto._bloques);
                }
            }
            st.Append(texto.st);  
#if DEBUG
            if (_bloques != null)
            {
                //Debug.Assert(_bloques.Sum(x => x.Cantidad) == Length); ;
            }
#endif
        }

        internal Texto Dividir(int posicionDivision)
        {
            Texto texto2 = new Texto();
            
            if (_bloques != null)
            {
                int primerbloque, ultimobloque;
                int deltaini, deltafin;
                ObtenerRango(posicionDivision, st.Length-posicionDivision, out primerbloque, out deltaini, out ultimobloque, out deltafin);
                int saldoprimerbloque = _bloques[primerbloque].Cantidad - deltaini;
                _bloques[primerbloque].CambiarCantidad(deltaini);

                texto2._bloques = new List<Bloque>();
                Bloque clon = null;
                if (saldoprimerbloque != 0)
                {
                    clon = _bloques[primerbloque].Clonar();
                    clon.CambiarCantidad(saldoprimerbloque);
                    texto2._bloques.Add(clon);
                }
                for (int i = primerbloque + 1; i < _bloques.Count; i++)
                {
                    texto2._bloques.Add(_bloques[i]);
                }
                _bloques.RemoveRange(primerbloque + 1, _bloques.Count - (primerbloque + 1));
                
            }
            texto2.Iniciar();
            texto2.st.Append(st.ToString(posicionDivision, st.Length - posicionDivision));
            st.Remove(posicionDivision, st.Length - posicionDivision);
#if DEBUG
            if (_bloques != null)
            {
                //Debug.Assert(_bloques.Sum(x => x.Cantidad) == Length);
                //Debug.Assert(texto2._bloques.Sum(x => x.Cantidad) == texto2.st.Length); 
            }
#endif
            return texto2;
        }

        internal Texto ObtenerRangoTexto(int inicio, int cantidad)
        {
            Texto t = new Texto();
            IEnumerable<Bloque> bloques=ObtenerRangoBloques(inicio,cantidad);
            t.st.Append(st);
            t._bloques = new List<Bloque>();
            foreach (Bloque b in bloques)
            {
                t._bloques.Add(b.Clonar());
            }
            return t;
        }
    }
}
