using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace SWPEditor.Dominio.TextoFormato
{
    public struct Bloque
    {
        public int Cantidad { get; private set; }
        Formato _Formato;
        public Formato Formato
        {
            get
            {
                return _Formato;
            }
            internal set
            {
                _Formato = value;
            }
        }
        
        public Bloque(int cantidad, Formato formato):this()
        {
            Cantidad = cantidad;
            Debug.Assert(cantidad >= 0);
            if (formato != null) 
                formato = formato.Clonar();
            _Formato = formato;
        }
        public Bloque Clonar()
        {
            return (Bloque)this.MemberwiseClone();
        }

        internal void IncrementarCantidad(int incremento)
        {
            Cantidad += incremento;
            Debug.Assert(incremento >= 0 && Cantidad >= 0);
        }
        internal void DisminuirCantidad(int disminucion)
        {
            Cantidad -= disminucion;
            Debug.Assert(disminucion >= 0 && Cantidad >= 0);
        }
        internal void CambiarCantidad(int nuevoValor)
        {
            Cantidad = nuevoValor;
            Debug.Assert(Cantidad >= 0);
        }
        internal void FusionarFormato(Formato formato)
        {
            if (Formato == null)
            {
                _Formato = formato.Fusionar(null);
            }
            else
            {
                _Formato = Formato.Fusionar(formato);
            }
        }
    }
}
