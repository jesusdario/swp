using System;
using System.Collections.Generic;
using System.Text;
using SWPEditor.Dominio;

namespace SWPEditor.Aplicacion
{
    public class Seleccion
    {
        public Parrafo Inicio { get; set; }
        public int PosicionParrafoInicio { get; set; }
        public Parrafo Fin { get; set; }
        public int PosicionParrafoFin { get; set; }

        public Parrafo ObtenerParrafoInicial()
        {
            if (Inicio.EsSiguiente(Fin))
                return Inicio;
            else
                return Fin;
        }
        public Parrafo ObtenerParrafoFinal()
        {
            if (Inicio.EsSiguiente(Fin))
                return Fin;
            else
                return Inicio;
        }
        public int ObtenerPosicionInicial()
        {
            if (Inicio == Fin)
            {
                return Math.Min(PosicionParrafoInicio, PosicionParrafoFin);
            }
            if (Inicio.EsSiguiente(Fin))
                return PosicionParrafoInicio;
            else
                return PosicionParrafoFin;
        }
        public int ObtenerPosicionFinal()
        {
            if (Inicio == Fin)
            {
                return Math.Max(PosicionParrafoInicio, PosicionParrafoFin);
            }
            if (Inicio.EsSiguiente(Fin))
                return PosicionParrafoFin;
            else
                return PosicionParrafoInicio;
        }
        public string ObtenerTexto()
        {
            Parrafo a = ObtenerParrafoInicial();
            int inicio = ObtenerPosicionInicial();
            Parrafo b = ObtenerParrafoFinal();
            int fin = ObtenerPosicionFinal();
            if (a == b)
            {
                return a.ObtenerSubCadena(inicio, fin - inicio);
            }
            else
            {
                StringBuilder st = new StringBuilder();
                st.Append(a.ObtenerSubCadena(inicio,a.ObtenerLongitud()-inicio));
                Parrafo act = a.Siguiente;
                while (act != b)
                {
                    st.Append("\r\n");
                    st.Append(act.ToString());
                    act = act.Siguiente;
                }
                st.Append("\r\n");
                st.Append(b.ObtenerSubCadena(0,fin));
                return st.ToString();
            }

        }
    }
}
