/**********************************************
This Program was written by Dario Romero A.
It is licensed under the Apache License v.2.0.
***********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace SWPEditor.Dominio
{
    public struct Medicion
    {
        public double Valor { get; set; }
        public Unidad Unidad { get; set; }
        public static readonly Medicion Cero = new Medicion(0, Unidad.Milimetros);
        public Medicion(double valor):this()
        {
            Valor = valor;
            Unidad = Unidad.Milimetros;
        }
        public Medicion Redondear(int numdecimales)
        {
            return new Medicion(Math.Round(Valor, numdecimales), Unidad);
        }
        public override int GetHashCode()
        {
            return Valor.GetHashCode() | (Unidad.FactorConversion.GetHashCode()<<16);
        }
        public override bool Equals(object obj)
        {
            Medicion m = (Medicion)obj;
            if (Unidad == m.Unidad &&
                Valor == m.Valor)
                return true;
            return false;
        }
        public Medicion(double valor, Unidad unidad)
            : this()
        {
            Valor = valor;
            Unidad = unidad;
            Debug.Assert(unidad != null);
        }
        public Medicion ConvertirA(Unidad unidad2)
        {
            Debug.Assert(unidad2 != null);
            Debug.Assert(Unidad != null);
            Unidad act = Unidad;
            double factor = 1;
            do
            {
                factor*=act.FactorConversion;
                act = act.UnidadRelativa;
            } while (act != null);
            double factor2 = 1;
            act = unidad2;
            do
            {
                factor2 *= act.FactorConversion;
                act = act.UnidadRelativa;
            } while (act != null);
            return new Medicion(Valor * factor / factor2,unidad2);
        }
      
        public static Medicion operator + (Medicion a,Medicion b) {
            if (a.Unidad == b.Unidad)
                return new Medicion(a.Valor + b.Valor, a.Unidad);
            else
            {
                return a + b.ConvertirA(a.Unidad);
            }
        }
        public static Medicion operator -(Medicion a, Medicion b)
        {
            if (a.Unidad == b.Unidad)
                return new Medicion(a.Valor - b.Valor, a.Unidad);
            else
            {
                return a - b.ConvertirA(a.Unidad);
            }
        }
        public static bool operator <(Medicion a, Medicion b)
        {
            if (a.Unidad == b.Unidad)
            {
                return a.Valor < b.Valor;
            }
            else
            {
                return a < b.ConvertirA(a.Unidad);
            }
        }
        public static bool operator >(Medicion a, Medicion b)
        {
            return b < a;
        }
        public static bool operator <=(Medicion a, Medicion b)
        {
            if (a.Unidad == b.Unidad)
            {
                return a.Valor <= b.Valor;
            }
            else
            {
                return a < b.ConvertirA(a.Unidad);
            }
        }
        public static bool operator >=(Medicion a, Medicion b)
        {
            return b <= a;
        }
        public static Medicion operator *(Medicion a, double escalar)
        {
            return new Medicion(a.Valor * escalar, a.Unidad);            
        }
        public static Medicion operator *(double escalar,Medicion a)
        {
            return new Medicion(a.Valor * escalar, a.Unidad);
        }
        public static Medicion operator /(Medicion a,double escalar)
        {
            return new Medicion(a.Valor / escalar, a.Unidad);
        }
        public static double operator /(Medicion a, Medicion b)
        {
            if (a.Unidad == b.Unidad)
            {
                return a.Valor / b.Valor;
            }
            else
            {
                return a / b.ConvertirA(a.Unidad);
            }
        }
        public override string ToString()
        {
            return Valor + " "+Unidad.Nombre;
        }
    }
}
