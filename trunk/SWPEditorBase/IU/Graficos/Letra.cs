/**********************************************
This Program was written by Dario Romero A.
It is licensed under the Apache License v.2.0.
***********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using SWPEditor.Dominio;

namespace SWPEditor.IU.Graficos
{
    public class Letra
    {
        public string Familia { get; private set; }
        public Medicion Tamaño { get; private set; }
        public bool Negrilla { get; private set; }
        public bool Cursiva { get; private set; }
        public bool Subrayado { get; private set; }
        private Letra()
        {
            Familia = "Arial";
            Tamaño = new Medicion(1.4);
        }
        static Dictionary<Letra, Letra> _letras;
        [ThreadStatic]
        static Letra _prototipo;
        static Letra()
        {
            _letras = new Dictionary<Letra, Letra>();
        }
        public static Letra Crear(string familia,Medicion tamaño,bool negrilla,bool cursiva,bool subrayado)
        {
            Letra p=_prototipo;
            if (p==null) {
                _prototipo = p = new Letra() { Familia = familia, Tamaño = tamaño, Negrilla = negrilla, Cursiva = cursiva, Subrayado = subrayado };
            }
            p.Familia=familia;
            p.Tamaño=tamaño;
            p.Negrilla=negrilla;
            p.Cursiva=cursiva;
            p.Subrayado=subrayado;
            if (!_letras.ContainsKey(p))
            {
                Letra nueva = p.Clonar();
                _letras.Add(nueva, nueva);
                return nueva;
            }
            else
            {
                return _letras[p];
            }
        }

        private Letra Clonar()
        {
            Letra l = (Letra)MemberwiseClone();
            return l;
        }
        public override int GetHashCode()
        {
            return (Familia??string.Empty).GetHashCode() ^ Tamaño.GetHashCode()^ 
                (Negrilla?256:0)^(Cursiva?65000:0)^(Subrayado?10000000:0);
        }
        public override bool Equals(object obj)
        {
            Letra l = (Letra)obj;
            return Familia == l.Familia &&
                Negrilla == l.Negrilla &&
                Cursiva == l.Cursiva &&
                Subrayado == l.Subrayado &&
                Tamaño.Equals(l.Tamaño);
        }
        Medicion? baseTexto;
        Medicion? altoTexto;
        Medicion? espacioLineas;
        public Medicion MedirBaseTexto(IGraficador _GraficadorConsultas)
        {
            if (!baseTexto.HasValue) 
                baseTexto = _GraficadorConsultas.MedirBaseTexto(this);
            return baseTexto.Value;
        }

        public Medicion MedirAltoTexto(IGraficador _GraficadorConsultas)
        {
            if (!altoTexto.HasValue)
                altoTexto = _GraficadorConsultas.MedirAltoTexto(this);
            return altoTexto.Value;
        }

        public Medicion MedirEspacioLineas(IGraficador _GraficadorConsultas)
        {
            if (!espacioLineas.HasValue)
                espacioLineas = _GraficadorConsultas.MedirEspacioLineas(this);
            return espacioLineas.Value;
        }
    }
}
