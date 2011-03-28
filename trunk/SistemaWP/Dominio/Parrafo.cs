using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using SistemaWP.Dominio.TextoFormato;

namespace SistemaWP.Dominio
{
    public class Parrafo
    {
        Documento _contenedor;
        TextoFormato.Texto bufferTexto = new TextoFormato.Texto();
        public int ID { get; private set; }       
        private int Posicion { get; set; }
        public Parrafo Anterior { get; private set; }
        public Parrafo Siguiente { get; private set; }
        FormatoParrafo _Formato;
        public FormatoParrafo Formato { 
            get {
                if (_Formato == null) return FormatoParrafo.ObtenerPredefinido();
                return _Formato;
            }
            set
            {
                _Formato = value;
            }
        }
        public void ConectarDespues(Parrafo parrafo)
        {
            if (parrafo != null)
            {
                parrafo.Anterior = this;
            }
            Siguiente = parrafo;
        }
        public void ConectarAntes(Parrafo parrafo)
        {
            if (parrafo != null)
            {
                parrafo.Siguiente = this;
            }
            Anterior = parrafo;
        }
        public Parrafo(Documento contenedor,int id,Parrafo anterior,Parrafo siguiente)
        {
            ID = id;
            _contenedor = contenedor;
            Posicion = 1;
            Anterior = anterior;
            Siguiente = siguiente;
            bufferTexto.Iniciar();
        }
        public Parrafo(Documento contenedor, int id, Parrafo anterior, Parrafo siguiente,Parrafo formatoBase)
        {
            ID = id;
            _contenedor = contenedor;
            _Formato = formatoBase._Formato==null?null:formatoBase.Formato.Clonar();
            Posicion = 1;
            Anterior = anterior;
            Siguiente = siguiente;
            bufferTexto.Iniciar();
        }
        public string ObtenerSubCadena(int inicio, int cantidad)
        {
            return bufferTexto.ToString(inicio,cantidad);
        }
        public void AgregarCadena(string cad)
        {
            bufferTexto.Append(cad);
        }

        public void AgregarCaracter(int posicionInsercion, char caracter)
        {
            bufferTexto.Insert(posicionInsercion, caracter);
        }
        public override string ToString()
        {
            return bufferTexto.ToString();
        }

        public void BorrarCaracter(int posicion)
        {
            if (posicion==bufferTexto.Length)
            {
                _contenedor.FusionarSiguiente(this);
            }
            else
            {
                bufferTexto.Remove(posicion, 1);
            }
        }

        public void FusionarCon(Parrafo parrafoSiguiente)
        {
            bufferTexto.Agregar(parrafoSiguiente.bufferTexto);
            ConectarDespues(parrafoSiguiente.Siguiente);
            if (_Formato != null || parrafoSiguiente._Formato != null)
            {
                _Formato = Formato.Fusionar(parrafoSiguiente.Formato);
            }
        }

        internal int ObtenerLongitud()
        {
            return bufferTexto.Length;
        }

        internal Parrafo DividirParrafo(int idnuevo,int posicionDivision)
        {
            Parrafo nuevo = new Parrafo(_contenedor, idnuevo, this, Siguiente);
            nuevo._Formato = _Formato;
            InsertarSiguiente(nuevo);
            nuevo.bufferTexto=bufferTexto.Dividir(posicionDivision);
            /*for (int i = posicionDivision; i < bufferTexto.Length; i++)
            {
                nuevo.bufferTexto.Append(bufferTexto[i]);
            }
            bufferTexto.Remove(posicionDivision, bufferTexto.Length-posicionDivision);*/
            return nuevo;
        }

        internal void CambiarID(int indicellave)
        {
            ID = indicellave;
        }

        internal void InsertarAnterior(Parrafo nuevo)
        {
            if (Anterior != null)
            {
                Anterior.Siguiente = nuevo;
            }
            nuevo.Anterior = Anterior;
            nuevo.Siguiente = this;
            Anterior = nuevo;
            Parrafo ant=Anterior;
            int contador = Posicion-1;
            while (ant != null)
            {
                ant.Posicion = contador;
                ant = ant.Anterior;
                contador--;
            }
        }
        public bool EsSiguiente(Parrafo parrafo2)
        {
            return Posicion < parrafo2.Posicion;
        }
        internal void InsertarSiguiente(Parrafo nuevo)
        {
            if (Siguiente != null)
            {
                Siguiente.Anterior = nuevo;
            }
            nuevo.Anterior = this;
            nuevo.Siguiente = Siguiente;
            Siguiente = nuevo;
            Parrafo sig = Siguiente;
            int contador = Posicion+1;
            while (sig != null)
            {
                sig.Posicion = contador; 
                sig = sig.Siguiente;
                contador++;
            }
        }

        internal void BorrarHastaFin(int posicionFinRango)
        {
            bufferTexto.Remove(posicionFinRango, bufferTexto.Length - posicionFinRango);
        }

        internal void BorrarHastaInicio(int posicionInicio)
        {
            bufferTexto.Remove(0, posicionInicio);
        }

        internal void BorrarRangoCaracteres(int posicionInicio, int posicionFin)
        {
            if (posicionInicio == posicionFin) return;
            Debug.Assert(posicionInicio < posicionFin);
            bufferTexto.Remove(posicionInicio, posicionFin - posicionInicio);
        }

        internal int ObtenerSiguientePalabra(int posicionInsercion)
        {
            for (int i = posicionInsercion; i < bufferTexto.Length; i++)
            {
                if (bufferTexto[i] == ' ')
                {
                    return i + 1;
                }
            }
            return bufferTexto.Length;
        }

        internal int ObtenerAnteriorPalabra(int posicionInsercion)
        {
            if (posicionInsercion == bufferTexto.Length)
            {
                posicionInsercion--;
            }
            if (posicionInsercion>=1&&bufferTexto[posicionInsercion - 1] == ' ') //Si se está al inicio de una palabra ir a la anterior
            {
                for (int i = posicionInsercion - 2; i >= 0; i--)
                {
                    if (bufferTexto[i] == ' ')
                    {
                        return i + 1;
                    }
                }
            }
            else // ir al inicio de palabra actual
            {
                for (int i = posicionInsercion; i >= 0; i--)
                {
                    if (bufferTexto[i] == ' ')
                    {
                        return i + 1;
                    }
                }
            }
            
            return 0;
        }
#if FORMATO
        internal void CambiarFormato(Formato formato, int posicionInicio, int cantidad)
        {
            bufferTexto.AplicarFormato(formato, posicionInicio, cantidad);
        }
        internal Formato ObtenerFormatoComun(int posicionInicio, int cantidad)
        {
            return bufferTexto.ObtenerFormatoComun(posicionInicio, cantidad);
        }
        public void SimplificarFormato()
        {
            bufferTexto.SimplificarFormato();
        }
        public IEnumerable<Bloque> ObtenerBloques()
        {
            for (int i = 0; i < bufferTexto.ObtenerNumBloques(); i++)
            {
                yield return bufferTexto.ObtenerBloque(i);
            }
        }
#endif

        public void AlinearIzquierda()
        {
            Formato = Formato.Fusionar(FormatoParrafo.CrearAlineacionIzquierda());
        }
        public void AlinearDerecha()
        {
            Formato = Formato.Fusionar(FormatoParrafo.CrearAlineacionDerecha());
            
        }
        public void AlinearCentro()
        {
            Formato = Formato.Fusionar(FormatoParrafo.CrearAlineacionCentro());
        }

        internal void AumentarInterlineado()
        {
            Formato = Formato.Fusionar(FormatoParrafo.CrearEspacioInterlineal(Formato.ObtenerEspaciadoInterlineal() + 0.5m));
        }

        internal void DisminuirInterlineado()
        {
            decimal valor = Formato.ObtenerEspaciadoInterlineal()-0.5m;
            if (valor < 1) valor = 1;
            Formato = Formato.Fusionar(FormatoParrafo.CrearEspacioInterlineal(valor));
        }
    }
}
