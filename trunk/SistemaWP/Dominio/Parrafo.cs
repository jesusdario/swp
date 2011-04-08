using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using SWPEditor.Dominio.TextoFormato;

namespace SWPEditor.Dominio
{
    public class Parrafo
    {
        Documento _contenedor;
        TextoFormato.Texto bufferTexto = new TextoFormato.Texto();
        internal int ID { get; private set; }       
        private int Posicion { get; set; }
        private Parrafo _Anterior;
        private Parrafo _Siguiente;
        internal Parrafo Anterior { get { return _Anterior; } }
        internal Parrafo Siguiente { get { return _Siguiente; } }
        public bool EsPrimerParrafo { get { return _Anterior == null; } }
        public bool EsUltimoParrafo { get { return _Siguiente == null; } }
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
        internal IEnumerable<IndiceBloque> ObtenerIndices()
        {
            foreach (IndiceBloque b in bufferTexto.ObtenerIndices())
            {
                IndiceBloque bloque = b;
                bloque.Formato=Formato.ObtenerFormatoTexto().Fusionar(b.Formato);
                yield return bloque;
            }
        }
        internal void ConectarDespues(Parrafo parrafo)
        {
            if (parrafo != null)
            {
                parrafo._Anterior = this;
            }
            _Siguiente = parrafo;
        }
        internal void ConectarAntes(Parrafo parrafo)
        {
            if (parrafo != null)
            {
                parrafo._Siguiente = this;
            }
            _Anterior = parrafo;
        }
        internal Parrafo(Documento documento, int id, Parrafo anterior, Parrafo siguiente)
        {
            _contenedor = documento;
            ID = id;
            Posicion = 1;
            _Anterior = anterior;
            _Siguiente = siguiente;
            bufferTexto.Iniciar();
        }
        internal Parrafo(Documento _documento,int id, Parrafo anterior, Parrafo siguiente,Parrafo formatoBase)
        {
            _contenedor = _documento;
            ID = id;
            _Formato = formatoBase._Formato==null?null:formatoBase.Formato.Clonar();
            Posicion = 1;
            _Anterior = anterior;
            _Siguiente = siguiente;
            bufferTexto.Iniciar();
        }
        public string ObtenerSubCadena(int inicio, int cantidad)
        {
            return bufferTexto.ToString(inicio,cantidad);
        }
        public void AgregarCadena(string cad)
        {
            bufferTexto.Append(cad);
            _contenedor.NotificarCambio(this);
        }
        public void AgregarCaracter(int posicionInsercion, char caracter)
        {
            bufferTexto.Insert(posicionInsercion, caracter);
            _contenedor.NotificarCambio(this);
        }
        public override string ToString()
        {
            return bufferTexto.ToString();
        }

        public bool BorrarCaracter(int posicion)
        {
            if (posicion==bufferTexto.Length)
            {
                _contenedor.FusionarSiguiente(this);
                return true;
            }
            else
            {
                bufferTexto.Remove(posicion, 1);
                _contenedor.NotificarCambio(this);
                return false;
            }
        }

        internal void FusionarCon(Parrafo parrafo_Siguiente)
        {
            bufferTexto.Agregar(parrafo_Siguiente.bufferTexto);
            ConectarDespues(parrafo_Siguiente._Siguiente);
            if (_Formato != null || parrafo_Siguiente._Formato != null)
            {
                _Formato = Formato.Fusionar(parrafo_Siguiente.Formato);
            }
            _contenedor.NotificarCambio(this);
        }

        public int Longitud
        {
            get
            {
                return bufferTexto.Length;
            }
        }

        internal Parrafo DividirParrafo(int idnuevo,int posicionDivision)
        {
            Parrafo nuevo = new Parrafo(_contenedor,idnuevo, this, _Siguiente);
            nuevo._Formato = _Formato;
            InsertarSiguiente(nuevo);
            nuevo.bufferTexto=bufferTexto.Dividir(posicionDivision);          
            return nuevo;
        }

        internal void CambiarID(int indicellave)
        {
            ID = indicellave;
        }

        internal void InsertarAnterior(Parrafo nuevo)
        {
            if (_Anterior != null)
            {
                _Anterior._Siguiente = nuevo;
            }
            nuevo._Anterior = _Anterior;
            nuevo._Siguiente = this;
            _Anterior = nuevo;
            _contenedor.NotificarCambioOrden();
            
            /*Parrafo ant=_Anterior;
            int contador = Posicion-5;
            while (ant != null)
            {
                ant.Posicion = contador;
                ant = ant._Anterior;
                int incremento=Math.Max(1,ant!=null&&ant.Posicion<contador?(ant.Posicion-contador)/2:10);
                contador-=incremento;
                if (ant != null && ant.Posicion < contador)
                {
                    break;
                }
            }*/
        }
        public bool EsSiguiente(Parrafo parrafo2)
        {
            _contenedor.AsegurarOrden();
            return Posicion < parrafo2.Posicion;
        }
        internal void IndicarOrden(int orden)
        {
            Posicion = orden;
        }
        internal void InsertarSiguiente(Parrafo nuevo)
        {
            if (_Siguiente != null)
            {
                _Siguiente._Anterior = nuevo;
            }
            nuevo._Anterior = this;
            nuevo._Siguiente = _Siguiente;
            _Siguiente = nuevo;
            _contenedor.NotificarCambioOrden();
            /*
            Parrafo sig = _Siguiente;
            int contador = Posicion+5;
            while (sig != null)
            {
                sig.Posicion = contador; 
                sig = sig._Siguiente;
                int incremento=Math.Max(1, sig != null&&sig.Posicion>contador ? (sig.Posicion - contador) / 2 : 10);
                contador += incremento;
                if (sig != null && sig.Posicion > contador)
                {
                    break;
                }
            }*/
        }

        internal void BorrarHastaFin(int posicionFinRango)
        {
            bufferTexto.Remove(posicionFinRango, bufferTexto.Length - posicionFinRango);
        }

        internal void BorrarHastaInicio(int posicionInicio)
        {
            bufferTexto.Remove(0, posicionInicio);
        }

        internal void BorrarRangoCaracteres(int posicionInicio, int cantidad)
        {
            if (cantidad==0) return;
            Debug.Assert(cantidad>0);
            bufferTexto.Remove(posicionInicio, cantidad);
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

        internal void CambiarFormato(Formato formato, int posicionInicio, int cantidad)
        {
            bufferTexto.AplicarFormato(formato, posicionInicio, cantidad);
        }
        internal Formato ObtenerFormatoComun(int posicionInicio, int cantidad)
        {
            return bufferTexto.ObtenerFormatoComun(posicionInicio, cantidad);
        }
        internal void SimplificarFormato()
        {
            bufferTexto.SimplificarFormato();
        }
        internal IEnumerable<Bloque> ObtenerBloques()
        {
            Bloque bc = new Bloque(0,null);
            for (int i = 0; i < bufferTexto.ObtenerNumBloques(); i++)
            {
                Bloque b=bufferTexto.ObtenerBloque(i);
                bc.CambiarCantidad(b.Cantidad);
                bc.Formato=Formato.ObtenerFormatoTexto().Fusionar(b.Formato);
                yield return bc;                
            }
        }
        internal IEnumerable<Bloque> ObtenerBloques(int inicio, int cantidad)
        {
            Bloque bc = new Bloque(0, null);
            IEnumerable<Bloque> rango=bufferTexto.ObtenerRangoBloques(inicio,cantidad);
            foreach (Bloque b in rango)
            {
                bc.CambiarCantidad(b.Cantidad);
                bc.Formato = Formato.ObtenerFormatoTexto().Fusionar(b.Formato);
                yield return bc;
            }
        }
        public void AlinearIzquierda()
        {
            Formato = Formato.Fusionar(FormatoParrafo.CrearAlineacionIzquierda());
            _contenedor.NotificarCambio(this);
        }
        public void AlinearDerecha()
        {
            Formato = Formato.Fusionar(FormatoParrafo.CrearAlineacionDerecha());
            _contenedor.NotificarCambio(this);
        }
        public void AlinearCentro()
        {
            Formato = Formato.Fusionar(FormatoParrafo.CrearAlineacionCentro());
            _contenedor.NotificarCambio(this);
        }

        public void AumentarInterlineado()
        {
            Formato = Formato.Fusionar(FormatoParrafo.CrearEspacioInterlineal(Formato.ObtenerEspaciadoInterlineal() + 0.5m));
            _contenedor.NotificarCambio(this);
        }

        public void DisminuirInterlineado()
        {
            decimal valor = Formato.ObtenerEspaciadoInterlineal()-0.5m;
            if (valor < 1) valor = 1;
            Formato = Formato.Fusionar(FormatoParrafo.CrearEspacioInterlineal(valor));
            _contenedor.NotificarCambio(this);
        }

        public void InsertarCadena(int posicionInsercion, string cadena)
        {
            Debug.Assert(posicionInsercion >= 0 && posicionInsercion <= Longitud);
            bufferTexto.Insert(posicionInsercion, cadena);
            _contenedor.NotificarCambio(this);
        }

        public void Escribir(SWPEditor.Dominio.IEscritor esc,int inicio,int cantidad)
        {
            esc.IniciarParrafo(Formato);
            int inicioBloque = 0;
            int finSeleccion = inicio + cantidad;
            IEnumerable<Bloque> bloques=bufferTexto.ObtenerRangoBloques(inicio,cantidad);
            inicioBloque = inicio;            
            foreach (Bloque b in bloques)
            {
                esc.EscribirTexto(ObtenerSubCadena(inicioBloque, b.Cantidad), Formato.ObtenerFormatoTexto().Fusionar(b.Formato));
                inicioBloque += b.Cantidad;
            }
            esc.TerminarParrafo();
        }
        public Parrafo ObtenerSubParrafo(Documento contenedor,int inicio, int cantidad)
        {
            Parrafo p = new Parrafo(contenedor, 0, null, null);
            p.bufferTexto = bufferTexto.ObtenerRangoTexto(inicio, cantidad);
            p._Formato = _Formato;
            return p;
        }
        public bool EstaEliminado
        {
            get
            {
                return ID == int.MaxValue;
            }
        }
        internal void NotificarEliminacion()
        {
            ID=int.MaxValue;
            _Anterior = null;
            _Siguiente = null;
            _Formato = null;

        }

        internal Parrafo Clonar(Documento documento)
        {
            Parrafo p = new Parrafo(documento, int.MaxValue, null, null);
            p.bufferTexto = bufferTexto.ObtenerRangoTexto(0, bufferTexto.Length);
            return p;
        }
    }
}
