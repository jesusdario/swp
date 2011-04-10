/**********************************************
This Program was written by Dario Romero A.
It is licensed under the Apache License v.2.0.
***********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace SWPEditor.Dominio.TextoFormato
{
    struct Texto
    {
        StringBuilder st;
        struct ContenedorBloques
        {
            List<Bloque> _bloques;
            public bool SinAsignar()
            {
                return _bloques == null;
            }
            public int Cantidad
            {
                get
                {
                    return _bloques.Count;
                }
            }
            public void Eliminar(int indice)
            {
                _bloques.RemoveAt(indice);
            }
            public void Asignar(int cantidad)
            {
                _bloques = new List<Bloque>();
                _bloques.Add(new Bloque(cantidad, null));
            }
            public Bloque Obtener(int i)
            {
                return _bloques[i];
            }
            public void Agregar(int cantidad, Formato formato)
            {
                _bloques.Add(new Bloque(cantidad, formato));
            }
            public void Guardar(int i, Bloque bloque)
            {
                _bloques[i] = bloque;
            }

            internal void EliminarRango(int indice, int cantidad)
            {
                _bloques.RemoveRange(indice, cantidad);
            }

            internal void Insertar(int indice, Bloque bloque)
            {
                _bloques.Insert(indice, bloque);
            }

            internal void AgregarDesde(ContenedorBloques contenedorBloques)
            {
                _bloques.AddRange(contenedorBloques._bloques);
            }
        }
        internal IEnumerable<IndiceBloque> ObtenerIndices()
        {
            int posicion=0;
            for (int i = 0; i < ObtenerNumBloques(); i++)
            {
                Bloque b=ObtenerBloque(i);
                yield return new IndiceBloque(posicion, b.Cantidad, b.Formato);
                posicion += b.Cantidad;
            }            
        }
        ContenedorBloques _bloques;
        public void Iniciar()
        {
            st = new StringBuilder();
            _bloques = new ContenedorBloques();
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
            if (_bloques.SinAsignar()&&indice==0)
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
            return _bloques.Obtener(indice);
        }
        public IEnumerable<Bloque> ObtenerRangoBloques(int inicio, int cantidad)
        {
            Bloque bc = new Bloque(0, null);
            if (_bloques.SinAsignar())
            {
                if (st.Length == 0)
                {
                    yield return _BloqueVacio;
                }
                else
                {
                    bc.CambiarCantidad(cantidad);
                    yield return bc;                    
                }
            }
            else
            {
                
                if (_bloques.Cantidad == 1)
                {
                    Bloque b = _bloques.Obtener(0);
                    bc.CambiarCantidad(b.Cantidad);
                    bc.Formato = b.Formato;
                    yield return bc;
                    yield break;
                }
                int inicioBloque = 0;
                int finSeleccion = inicio + cantidad;
                for (int i = 0; i < _bloques.Cantidad; i++)
                {
                    Bloque b=_bloques.Obtener(i);
                    int finBloque = inicioBloque + b.Cantidad;
                    bool inicioEnBloque = inicio >= inicioBloque && inicio < finBloque;
                    bool enmedio=inicioBloque>finSeleccion&&finBloque>finSeleccion;
                    bool finEnBloque = finSeleccion >= inicioBloque && finSeleccion <= finBloque;
                    if (inicioEnBloque)
                    {
                        if (finEnBloque)
                        {
                            bc.CambiarCantidad(finBloque - inicioBloque);
                            bc.Formato = b.Formato;
                            yield return bc;
                            yield break;
                        }
                        else
                        {
                            bc.Formato=b.Formato;
                            bc.CambiarCantidad(b.Cantidad);
                            bc.DisminuirCantidad(inicio - inicioBloque);
                            yield return bc;
                        }
                    }
                    else
                    {
                        if (finEnBloque)
                        {
                            bc.Formato = b.Formato;
                            bc.CambiarCantidad(finSeleccion-inicioBloque);
                            yield return bc;
                            yield break;                            
                        } else if (enmedio)
                        {
                            bc.Formato = b.Formato;
                            bc.CambiarCantidad(b.Cantidad);
                            yield return bc;
                        }                        
                    }
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
        private void Verificar()
        {
            if (_bloques.SinAsignar()) return;
            int suma = 0;
            for (int i = 0; i < _bloques.Cantidad; i++)
            {
                suma += _bloques.Obtener(i).Cantidad;
            }
            Debug.Assert(suma == Length);
        }
        private void ExtenderBloques(int delta)
        {
            if (!_bloques.SinAsignar())
            {
                Bloque b = _bloques.Obtener(_bloques.Cantidad - 1);
                b.IncrementarCantidad(delta);
                _bloques.Guardar(_bloques.Cantidad - 1, b);
                
#if DEBUG
                Verificar();
#endif

            }
        }
        private void IncrementarBloque(int posicion, int delta)
        {
            if (!_bloques.SinAsignar())
            {
                int suma = 0;
                for (int i = 0; i < _bloques.Cantidad; i++)
                {
                    Bloque b = _bloques.Obtener(i);
                    suma += b.Cantidad;
                    if (suma >= posicion)//||(_bloques[i].Cantidad==0&&suma==posicion))
                    {
                        int inc = i + 1;
                        while (inc < _bloques.Cantidad && _bloques.Obtener(inc).Cantidad==0)
                        {
                            inc++;
                        }
                        inc=inc-1;
                        Bloque bc=_bloques.Obtener(inc);
                        bc.IncrementarCantidad(delta);
                        _bloques.Guardar(inc, bc);
#if DEBUG
                Verificar();
#endif
                        return;
                    }
                }
                Bloque bd = _bloques.Obtener(_bloques.Cantidad - 1);
                bd.IncrementarCantidad(delta);
                _bloques.Guardar(_bloques.Cantidad-1,bd);
#if DEBUG
                Verificar();
#endif

            }
        }
        internal void Insert(int posicionInsercion, char caracter)
        {
            st.Insert(posicionInsercion, caracter);
            IncrementarBloque(posicionInsercion, 1);
            
#if DEBUG
            Verificar();
#endif

        }
        internal void Append(char caracter)
        {
            st.Append(caracter);
            ExtenderBloques(1);
#if DEBUG
            Verificar();
#endif

        }

        internal void Insert(int posicionInsercion, string cadena)
        {
            st.Insert(posicionInsercion, cadena);
            IncrementarBloque(posicionInsercion, cadena.Length);
#if DEBUG
            Verificar();
#endif
        }
        internal void SimplificarFormato()
        {
            if (_bloques.SinAsignar()) return;
            for (int i = _bloques.Cantidad-1; i >= 0; i--)
            {
                if (_bloques.Obtener(i).Cantidad == 0&&_bloques.Cantidad>1)
                {
                    _bloques.Eliminar(i);
                }
            }
#if DEBUG
            Verificar();
#endif

        }
        internal void Remove(int posicion, int cantidad)
        {
            st.Remove(posicion, cantidad);
            if (!_bloques.SinAsignar()) {
                int primerbloque, ultimobloque;
                int deltaini, deltafin;
                ObtenerRango(posicion, cantidad,
                    out primerbloque, out deltaini,
                    out ultimobloque, out deltafin);
                if (primerbloque == ultimobloque)
                {
                    Bloque b=_bloques.Obtener(primerbloque);
                    b.DisminuirCantidad(deltafin - deltaini);
                    if (b.Cantidad == 0)
                    {
                        _bloques.Eliminar(primerbloque);
                    }
                    else
                    {
                        _bloques.Guardar(primerbloque, b);
                    }
                    
                }
                else
                {
                    Bloque b = _bloques.Obtener(primerbloque);
                    b.DisminuirCantidad((b.Cantidad - deltaini));
                    _bloques.Guardar(primerbloque, b);
                    Bloque b2 = _bloques.Obtener(ultimobloque);
                    b2.DisminuirCantidad(deltafin);
                    _bloques.Guardar(ultimobloque,b2);
                    int primerbloqueelim = primerbloque + 1;
                    if (b.Cantidad == 0) primerbloqueelim--;
                    int ultimobloqueelim = ultimobloque - 1;
                    if (b2.Cantidad == 0) ultimobloqueelim++;
                    int bloquesremover = ultimobloqueelim-primerbloqueelim+1;//Incluir el último bloque si su cantidad es cero
                    if (bloquesremover > 0)
                    {
                        _bloques.EliminarRango(primerbloque + 1, bloquesremover);
                    }                    
#if DEBUG
                    Verificar();
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
            for (int i = 0; i < _bloques.Cantidad; i++)
            {
                sumaant = suma;
                suma = suma + _bloques.Obtener(i).Cantidad;
                if (!bloqueini && posicionini <= suma)
                {
                    deltaini = posicionini - sumaant;
                    bloqueini = true;
                    primerbloque = i;
                }
                if (posicionfin <= suma)
                {
                    deltafin = posicionfin - sumaant;
                    ultimobloque = i;
                    return;
                    
                }            
            }
            if (!bloqueini)
            {
                deltaini = _bloques.Obtener(_bloques.Cantidad - 1).Cantidad;
                primerbloque = _bloques.Cantidad - 1;
            }
            deltafin = _bloques.Obtener(_bloques.Cantidad - 1).Cantidad; 
            ultimobloque = _bloques.Cantidad - 1;
        }
        private void AsegurarBloques()
        {
            if (_bloques.SinAsignar())
            {
                _bloques.Asignar(Length);
            }
        }
        public void AplicarFormato(Formato formato, int inicio, int cantidad)
        {
            Debug.Assert(inicio + cantidad <= Length);
            AsegurarBloques();
            int primerbloque, ultimobloque;
            int deltaini, deltafin;
            ObtenerRango(inicio, cantidad, out primerbloque, out deltaini, out ultimobloque, out deltafin);
            if (primerbloque == ultimobloque)
            {
                Bloque a = _bloques.Obtener(primerbloque);
                Bloque b = new Bloque(deltafin - deltaini, a.Formato);
                b.FusionarFormato(formato);
                Bloque c = new Bloque(a.Cantidad - deltafin, a.Formato);
                a.DisminuirCantidad(a.Cantidad - deltaini);
                _bloques.Guardar(primerbloque,a);
                _bloques.Insertar(primerbloque + 1, b);
                _bloques.Insertar(primerbloque + 2, c);
            }
            else
            {
                Bloque a = _bloques.Obtener(primerbloque);
                //disminuir el tamaño de los bloques e insertar nuevos bloques al medio combinados.
                if (a.Cantidad != deltaini)
                {
                    _bloques.Insertar(primerbloque + 1, new Bloque(a.Cantidad - deltaini, a.Formato));
                    ultimobloque++;
                }
                a.DisminuirCantidad(a.Cantidad - deltaini);
                _bloques.Guardar(primerbloque,a);
                Bloque b = _bloques.Obtener(ultimobloque);
                if (deltafin != 0)
                {
                    _bloques.Insertar(ultimobloque, new Bloque(deltafin, b.Formato));
                    ultimobloque++;
                }
                b.DisminuirCantidad(deltafin);
                _bloques.Guardar(ultimobloque, b);
                for (int i = primerbloque + 1; i < ultimobloque; i++)
                {
                    Bloque bq=_bloques.Obtener(i);
                    bq.FusionarFormato(formato);
                    _bloques.Guardar(i, bq);
                }
               
#if DEBUG
                Verificar();
#endif
            }
        }

        internal Formato ObtenerFormatoComun(int posicionInicio, int cantidad)
        {
            throw new NotImplementedException();
        }

        internal int ObtenerNumBloques()
        {
            if (_bloques.SinAsignar()) return 1;
            return _bloques.Cantidad;
        }

        internal void Agregar(Texto texto)
        {
            if (texto.Length == 0) return;
            if (_bloques.SinAsignar() || !texto._bloques.SinAsignar())
            {
                AsegurarBloques();
                if (texto._bloques.SinAsignar())
                {
                    //Bloque b=_BloqueVacio.Clonar();
                    //b.CambiarCantidad(texto.st.Length);
                    _bloques.Agregar(texto.st.Length,null);
                }
                else
                {
                    _bloques.AgregarDesde(texto._bloques);
                }
            }
            st.Append(texto.st);  
#if DEBUG
            Verificar();
#endif
        }

        internal Texto Dividir(int posicionDivision)
        {
            Texto texto2 = new Texto();
            texto2.Iniciar();
            if (!_bloques.SinAsignar())
            {
                int primerbloque, ultimobloque;
                int deltaini, deltafin;
                ObtenerRango(posicionDivision, st.Length-posicionDivision, out primerbloque, out deltaini, out ultimobloque, out deltafin);
                Bloque bloqueinicio = _bloques.Obtener(primerbloque);
                int saldoprimerbloque = bloqueinicio.Cantidad - deltaini;
                bloqueinicio.CambiarCantidad(deltaini);
                _bloques.Guardar(primerbloque, bloqueinicio);
                texto2.AsegurarBloques();
                int deltaelim=0;
                if (saldoprimerbloque != 0)
                {
                    Bloque clon = _bloques.Obtener(primerbloque);//.Clonar();
                    texto2._bloques.Agregar(saldoprimerbloque,clon.Formato);
                    deltaelim++;
                }
                for (int i = primerbloque + 1; i < _bloques.Cantidad; i++)
                {
                    Bloque b = _bloques.Obtener(i);
                    texto2._bloques.Agregar(b.Cantidad,b.Formato);
                }
                if (primerbloque + 1 != _bloques.Cantidad)
                {
                    _bloques.EliminarRango(primerbloque + 1, _bloques.Cantidad - (primerbloque + 1));
                }
                
            }            
            texto2.st.Append(st.ToString(posicionDivision, st.Length - posicionDivision));
            st.Remove(posicionDivision, st.Length - posicionDivision);
#if DEBUG
            Verificar();
            texto2.Verificar();
#endif
            return texto2;
        }

        internal Texto ObtenerRangoTexto(int inicio, int cantidad)
        {
            Texto t = new Texto();
            t.Iniciar();
            if (!_bloques.SinAsignar())
            {
                IEnumerable<Bloque> bloques = ObtenerRangoBloques(inicio, cantidad);
                t.AsegurarBloques();
                foreach (Bloque b in bloques)
                {
                    t._bloques.Agregar(b.Cantidad, b.Formato);
                }
                t.st.Append(st);
            }
            t.Iniciar();
            t.Append(st.ToString(inicio, cantidad));
#if DEBUG
            t.Verificar();
#endif
            return t;
        }
    }
}
