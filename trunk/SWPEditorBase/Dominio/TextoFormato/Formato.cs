/**********************************************
This Program was written by Dario Romero A.
It is licensed under the Apache License v.2.0.
***********************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace SWPEditor.Dominio.TextoFormato
{
    [Flags]
    public enum FlagsFormato
    {
        NegrillaND=1,
        Negrilla=2,
        CursivaND = 4,
        Cursiva=8,
        SubrayadoND=16,
        Subrayado=32
    }
    public class Formato
    {
        
        public string FamiliaLetra { get; private set; }
        Medicion? _TamLetra;
        public Medicion? TamLetra
        {
            get
            {
                return _TamLetra;
            }
            set
            {
                if (value.HasValue)
                {
                    _TamLetra = value.Value.ConvertirA(Unidad.Puntos).Redondear(2);
                }
                else
                {
                    _TamLetra = null;
                }
            }
        }
        public ColorDocumento? ColorLetra { get; private set; }
        public ColorDocumento? ColorFondo { get; private set; }
        private float FactorEscalaLetra { get; set; }
        FlagsFormato Flags { get; set; }
        public string ObtenerFamiliaLetra()
        {
            return FamiliaLetra ?? Contexto.FamiliaLetra;
        }
        public Medicion ObtenerTamLetraEscalado()
        {
            if (FactorEscalaLetra != 1)
            {
                return (TamLetra ?? Contexto.TamLetra.Value)*FactorEscalaLetra;
            }
            else
            {
                return TamLetra ?? Contexto.TamLetra.Value;
            }
        }
        public float ObtenerEscalaLetra()
        {
            return FactorEscalaLetra;
        }
        public ColorDocumento ObtenerColorLetra()
        {
            return ColorLetra ?? Contexto.ColorLetra.Value;
        }
        public ColorDocumento ObtenerColorFondo()
        {
            return ColorFondo ?? Contexto.ColorFondo.Value;
        }
        public bool ObtenerNegrilla()
        {
            return Negrilla ?? Contexto.Negrilla.Value;
        }
        public bool ObtenerCursiva()
        {
            return Cursiva ?? Contexto.Cursiva.Value;
        }
        public bool ObtenerSubrayado()
        {
            return Subrayado ?? Contexto.Subrayado.Value;
        }
        private static Formato Contexto;
        static Formato()
        {
            Contexto = new Formato();
            Contexto.FamiliaLetra = "Arial";
            Contexto.TamLetra = new Medicion(12, Unidad.Puntos);
            Contexto.Subrayado = false;
            Contexto.Negrilla = false;
            Contexto.Cursiva = false;
            Contexto.ColorLetra = ColorDocumento.Negro;
            Contexto.ColorFondo = ColorDocumento.Transparente;
            Contexto.FactorEscalaLetra = 1;
        }
        public void Limpiar()
        {
            FamiliaLetra = null;
            TamLetra = null;
            Flags = SinFlags;
            FactorEscalaLetra = 1.0f;
            ColorLetra = null;
            ColorFondo = null;
        }
        const FlagsFormato SinFlags = (FlagsFormato)((int)(FlagsFormato.Negrilla | FlagsFormato.Cursiva | FlagsFormato.Subrayado) >> 1);
        private Formato() {
            Flags = SinFlags;//Dejar los flags en "SIN DEFINIR"
        }
        private Formato(string familiaLetra, Medicion? tamLetra):this()
        {
            FamiliaLetra = familiaLetra;
            TamLetra = tamLetra;            
        }
        public static Formato CrearNegrilla(bool valor)
        {
            return Crear(null, null, valor, null, null, null, null,null);
        }
        public static Formato CrearSubrayado(bool valor)
        {
            return Crear(null, null, null, null, valor, null, null, null);
        }
        public static Formato CrearCursiva(bool valor)
        {
            return Crear(null, null, null, valor, null, null, null, null);
        }
        public static Formato CrearTipoLetra(string familia)
        {
            return Crear(familia, null, null, null, null, null, null, null);
        }
        public static Formato CrearTamLetra(Medicion tamLetra)
        {
            return Crear(null, tamLetra, null, null, null, null, null, null);
        }
        public static Formato CrearColorFondo(ColorDocumento colorFondo)
        {
            return Crear(null, null, null, null, null, colorFondo, null, null);
        }
        public static Formato CrearColorLetra(ColorDocumento colorLetra)
        {
            return Crear(null, null, null, null, null, null, colorLetra, null);
        }
        public static Formato CrearLetra(string familia, Medicion tamLetra)
        {
            return Crear(familia, tamLetra, null, null, null, null, null, null);
        }
        public static Formato CrearEscalaLetra(float factor)
        {
            return Crear(null, null, null, null, null, null, null, factor);
        }
        [ThreadStatic]
        private static Formato formatoConstructor;
        private static Formato Crear(
            string familiaLetra,Medicion? tamLetra,
            bool? negrilla,bool? cursiva,bool? subrayado,
            ColorDocumento? colorFondo,ColorDocumento? colorLetra,
            float? factorEscala)
        {
            if (formatoConstructor==null) formatoConstructor = new Formato();
            Formato f = formatoConstructor;
            f.FamiliaLetra = familiaLetra;
            f.TamLetra = tamLetra;
            f.ColorFondo = colorFondo;
            f.Negrilla = negrilla;
            f.Subrayado = subrayado;
            f.Cursiva = cursiva;
            f.ColorLetra = colorLetra;
            f.ColorFondo = colorFondo;
            f.FactorEscalaLetra = factorEscala??1;
            
            return ObtenerDeCache(f);
        }
        private bool? TieneFlag(FlagsFormato flag)
        {
            int num = (int)flag >>1;
            if (((int)Flags & num) != 0) return null;
            return (Flags & flag) != 0;
        }
        private void CambiarFlag(FlagsFormato flag,bool? valor)
        {
            FlagsFormato num = (FlagsFormato)((int)flag >> 1);
            if (valor.HasValue)
            {
                if (valor.Value)
                {
                    Flags = Flags | flag;
                }
                else
                {
                    Flags = Flags & (~flag);
                }
                Flags = Flags & (~num);
            }
            else
            {
                Flags = Flags & (~flag);
                Flags = Flags | num;
            }
        }
        public bool? Negrilla { get { return TieneFlag(FlagsFormato.Negrilla); } private set { CambiarFlag(FlagsFormato.Negrilla, value); } }
        public bool? Cursiva { get { return TieneFlag(FlagsFormato.Cursiva); } private set { CambiarFlag(FlagsFormato.Cursiva, value); } }
        public bool? Subrayado { get { return TieneFlag(FlagsFormato.Subrayado); } private set { CambiarFlag(FlagsFormato.Subrayado, value); } }
        public override int GetHashCode()
        {
            return (FamiliaLetra == null ? 0 : FamiliaLetra.GetHashCode()) ^
                (TamLetra.HasValue ? TamLetra.Value.GetHashCode() : 0) ^
                Flags.GetHashCode()^
                ColorFondo.GetHashCode()^
                ColorLetra.GetHashCode()^
                FactorEscalaLetra.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            Formato f = obj as Formato;
            if (f == null) return false;
            return f.Flags == Flags && TamLetra.Equals(f.TamLetra) && f.FamiliaLetra == FamiliaLetra
                &&ColorFondo.Equals(f.ColorFondo)&&ColorLetra.Equals(f.ColorLetra)
                &&FactorEscalaLetra==f.FactorEscalaLetra;
        }
        static Dictionary<Formato, Formato> m_Formatos=new Dictionary<Formato,Formato>();
       
        static Formato ObtenerDeCache(Formato f)
        {
            if (m_Formatos.ContainsKey(f))
            {
                lock (m_Formatos)
                {
                    return m_Formatos[f];
                }
            }
            else
            {
                lock (m_Formatos)
                {
                    if (f==formatoConstructor) f = f.Clonar();
                    m_Formatos.Add(f, f);
                    return f;
                }
            }            
        }
        public Formato Fusionar(Formato formato2)
        {
            Formato f;
            if (formato2 != null)
            {
                if (formatoConstructor == null) formatoConstructor = new Formato();
                f = formatoConstructor;
                f.FamiliaLetra = formato2.FamiliaLetra ?? FamiliaLetra;
                f.TamLetra = formato2.TamLetra ?? TamLetra;
                f.Negrilla = formato2.Negrilla ?? Negrilla;
                f.Cursiva = formato2.Cursiva ?? Cursiva;
                f.Subrayado = formato2.Subrayado ?? Subrayado;
                f.ColorFondo = formato2.ColorFondo ?? ColorFondo;
                f.ColorLetra = formato2.ColorLetra ?? ColorLetra;
                f.FactorEscalaLetra = (float)Math.Round(formato2.FactorEscalaLetra * FactorEscalaLetra,2);
                return ObtenerDeCache(f);
            }
            else
            {   
                return this;
            }
        }
        public Formato ObtenerInterseccion(Formato formato2)
        {
            if (formatoConstructor == null) formatoConstructor = new Formato();
            Formato f = formatoConstructor;
            if (FamiliaLetra == formato2.FamiliaLetra) f.FamiliaLetra = FamiliaLetra;
            if (TamLetra.Equals(formato2.TamLetra)) f.TamLetra=TamLetra;
            if (Negrilla.Equals(formato2.Negrilla)) f.Negrilla = Negrilla;
            if (Cursiva.Equals(formato2.Cursiva)) f.Cursiva = Cursiva;
            if (Subrayado.Equals(formato2.Subrayado)) f.Subrayado = Subrayado;
            if (ColorFondo.Equals(formato2.ColorFondo)) f.ColorFondo = ColorFondo;
            if (ColorLetra.Equals(formato2.ColorLetra)) f.ColorLetra = ColorLetra;
            return ObtenerDeCache(f);
        }
        public Formato Clonar()
        {
            return (Formato)this.MemberwiseClone();
        }

        internal static Formato ObtenerPredefinido()
        {
            return Contexto;
        }
    }
}
