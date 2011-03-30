using System;
using System.Collections.Generic;
using System.Text;
using SWPEditor.Dominio;
using SWPEditor.Dominio.Html;
using SWPEditor.Dominio.Texto;
using SWPEditor.Dominio.TextoFormato;

namespace SWPEditor.Aplicacion
{
    public class Seleccion
    {
        internal Documento Documento { get; private set; }
        internal Parrafo Inicio { get; private set; }
        internal int PosicionParrafoInicio { get; private set; }
        internal Parrafo Fin { get; private set; }
        internal int PosicionParrafoFin { get; private set; }
        internal Seleccion(Documento documento, Parrafo inicio, int posicionInicio, Parrafo fin, int posicionFin)
        {
            Documento = documento;
            Inicio = inicio;
            Fin = fin;
            PosicionParrafoInicio = posicionInicio;
            PosicionParrafoFin = posicionFin;
        }
        internal Parrafo ObtenerParrafoInicial()
        {
            if (Inicio.EsSiguiente(Fin))
                return Inicio;
            else
                return Fin;
        }
        internal Parrafo ObtenerParrafoFinal()
        {
            if (Inicio.EsSiguiente(Fin))
                return Fin;
            else
                return Inicio;
        }
        internal int ObtenerPosicionInicial()
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
        internal int ObtenerPosicionFinal()
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
        public bool EstaVacia
        {
            get
            {
                return ObtenerPosicionInicial() == ObtenerPosicionFinal() &&
                    ObtenerParrafoInicial() == ObtenerParrafoFinal();
            }
        }
        public Documento ObtenerDocumento()
        {
            return Documento.ObtenerSubdocumento(ObtenerParrafoInicial(), ObtenerPosicionInicial(), ObtenerParrafoFinal(), ObtenerPosicionFinal());
        }
        public string ObtenerHtml()
        {
            return ObtenerTexto(new EscritorHTML());
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
