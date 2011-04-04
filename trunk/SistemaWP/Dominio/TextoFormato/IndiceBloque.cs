using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace SWPEditor.Dominio.TextoFormato
{
    struct IndiceBloque
    {
        public int Inicio;
        public int Cantidad;
        public Formato Formato;
        public int Fin { get { return Inicio + Cantidad; } }
        public IndiceBloque(int inicio, int cantidad, Formato formato)
        {
            Debug.Assert(cantidad >= 0);
            Debug.Assert(inicio >= 0);
            Inicio = inicio; Cantidad = cantidad; Formato = formato;
            
        }
        public IndiceBloque BloqueHasta(int delta)
        {
            Debug.Assert(delta <= Cantidad);
            return new IndiceBloque(Inicio, delta, Formato);
        }
        public void AvanzarIndice(int delta)
        {
            Inicio += delta;
            Cantidad -= delta;
        }
        public bool ContieneCaracter(int posicion)
        {
            return Inicio <= posicion && posicion < Inicio + Cantidad;
        }
    }
}
