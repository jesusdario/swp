using System;
using System.Collections.Generic;
using System.Text;

namespace SWPEditor.Dominio.TextoFormato
{
    public enum AlineacionParrafo
    {
        Izquierda,Derecha,Centro
    }
    public class FormatoParrafo
    {
        public Medicion? EspacioAnterior { get; private set; }
        public Medicion? EspacioPosterior { get; private set; }
        public decimal? EspaciadoInterlineal { get; private set; }
        public AlineacionParrafo? AlineacionHorizontal { get; private set; }
        public Formato FormatoTexto { get; private set; }
        public AlineacionParrafo ObtenerAlineacionHorizontal()
        {
            return AlineacionHorizontal.HasValue ? AlineacionHorizontal.Value : FormatoBase.AlineacionHorizontal.Value;
        }

        public Medicion ObtenerEspacioAnterior()
        {
            return EspacioAnterior ?? FormatoParrafo.FormatoBase.EspacioAnterior.Value;
        }
        public Medicion ObtenerEspacioPosterior()
        {
            return EspacioPosterior ?? FormatoParrafo.FormatoBase.EspacioPosterior.Value;
        }
        public decimal ObtenerEspaciadoInterlineal()
        {
            return EspaciadoInterlineal ?? FormatoParrafo.FormatoBase.EspaciadoInterlineal.Value;
        }
        public FormatoParrafo() {
        }
        private static readonly FormatoParrafo FormatoBase=new FormatoParrafo() {
            EspacioAnterior=new Medicion(5,Unidad.Milimetros),
            EspacioPosterior=new Medicion(5,Unidad.Milimetros),
            EspaciadoInterlineal=1.0m,
            AlineacionHorizontal=AlineacionParrafo.Izquierda};
        public FormatoParrafo Clonar()
        {
            return (FormatoParrafo)MemberwiseClone();
        }
        public FormatoParrafo Fusionar(FormatoParrafo formato2)
        {
            FormatoParrafo res = Clonar();
            res.EspaciadoInterlineal = formato2.EspaciadoInterlineal ?? EspaciadoInterlineal;
            res.EspacioAnterior = formato2.EspacioAnterior ?? EspacioAnterior;
            res.EspacioPosterior = formato2.EspacioPosterior ?? EspacioPosterior;
            res.AlineacionHorizontal = formato2.AlineacionHorizontal ?? AlineacionHorizontal;
            return res;
        }
        public static FormatoParrafo CrearEspacioInterlineal(decimal interlineado)
        {
            FormatoParrafo f = new FormatoParrafo();
            f.EspaciadoInterlineal = interlineado;
            return f;
        }
        internal static FormatoParrafo ObtenerPredefinido()
        {
            return FormatoBase;
        }

        internal static FormatoParrafo CrearAlineacionDerecha()
        {
            FormatoParrafo f = new FormatoParrafo();
            f.AlineacionHorizontal = AlineacionParrafo.Derecha;
            return f;
        }

        internal static FormatoParrafo CrearAlineacionIzquierda()
        {
            FormatoParrafo f = new FormatoParrafo();
            f.AlineacionHorizontal = AlineacionParrafo.Izquierda;
            return f;
        }

        internal static FormatoParrafo CrearAlineacionCentro()
        {
            FormatoParrafo f = new FormatoParrafo();
            f.AlineacionHorizontal = AlineacionParrafo.Centro;
            return f;
        }

        internal Formato ObtenerFormatoTexto()
        {
            return FormatoTexto??Formato.ObtenerPredefinido();
        }
    }
}
