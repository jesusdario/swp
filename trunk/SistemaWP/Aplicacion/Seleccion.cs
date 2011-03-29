using System;
using System.Collections.Generic;
using System.Text;
using SWPEditor.Dominio;
using SWPEditor.Dominio.Html;
using SWPEditor.Dominio.Texto;

namespace SWPEditor.Aplicacion
{
    public class Seleccion
    {
        public Documento Documento { get; private set; }
        public Parrafo Inicio { get; private set; }
        public int PosicionParrafoInicio { get; private set; }
        public Parrafo Fin { get; private set; }
        public int PosicionParrafoFin { get; private set; }
        public Seleccion(Documento documento, Parrafo inicio, int posicionInicio, Parrafo fin, int posicionFin)
        {
            Documento = documento;
            Inicio = inicio;
            Fin = fin;
            PosicionParrafoInicio = posicionInicio;
            PosicionParrafoFin = posicionFin;
        }
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
        public string ObtenerHtml()
        {
            return ObtenerTexto(new EscritorHtml());
        }
        public string ObtenerTexto()
        {
            return ObtenerTexto(new EscritorTexto());
        }
        public string ObtenerTexto(IEscritor escritor)
        {
            return Documento.ObtenerTexto(escritor, ObtenerParrafoInicial(), ObtenerPosicionInicial(), ObtenerParrafoFinal(), ObtenerPosicionFinal());
        }
    }
}
