/**********************************************
This Program was written by Dario Romero A.
It is licensed under the Apache License v.2.0.
***********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using SWPEditor.Dominio;
using SWPEditor.Dominio.TextoFormato;
using System.Diagnostics;

namespace SWPEditor.IU.PresentacionDocumento
{
    public class AvanceBloques
    {
        Parrafo _parrafoActual;
        IEnumerator<IndiceBloque> _enumerador;
        IndiceBloque _primerBloque;
        int inicioActual;
        public AvanceBloques(Linea lineaInicio)
        {
            AvanzarHastaLinea(lineaInicio.Parrafo, lineaInicio);
        }
        private void AvanzarHastaLinea(Parrafo parrafoInicio,Linea linea)
        {
            _parrafoActual = parrafoInicio;
            _enumerador = _parrafoActual.ObtenerIndices().GetEnumerator();
            while (_enumerador.MoveNext())
            {
                _primerBloque = _enumerador.Current;
                if (_primerBloque.ContieneCaracter(linea.Inicio))
                {
                    _primerBloque.AvanzarIndice(linea.Inicio - _primerBloque.Inicio);
                    break;
                }
            }
        }
        public IEnumerable<Bloque> ObtenerBloquesDe(Linea linea)
        {
            if (linea.Parrafo != _parrafoActual)
            {
                if (linea.Inicio == 0)
                {
                    _parrafoActual = linea.Parrafo;
                    _enumerador = _parrafoActual.ObtenerIndices().GetEnumerator();
                    _primerBloque = new IndiceBloque(0, 0, null);
                }
                else
                {
                    AvanzarHastaLinea(linea.Parrafo, linea);
                }
            }
            int saldoprocesar = linea.Cantidad;
            while (saldoprocesar >0)
            {
                if (_primerBloque.Cantidad==0)
                {
                    bool avanzado = _enumerador.MoveNext();
                    if (!avanzado)
                    {
                        yield break;
                    }
                    _primerBloque = _enumerador.Current;
                }
                int avance = Math.Min(_primerBloque.Cantidad, saldoprocesar);
                IndiceBloque parteLinea = _primerBloque.BloqueHasta(avance);
                yield return new Bloque(parteLinea.Cantidad, parteLinea.Formato);
                _primerBloque.AvanzarIndice(avance);
                saldoprocesar -= avance;
            }
            /*if (!_primerbloqueconsiderado)
            {
                int avance = Math.Min(_primerBloque.Cantidad, linea.Cantidad);
                IndiceBloque parteLinea = _primerBloque.BloqueHasta(avance);
                saldoprocesar += avance;
                yield return new Bloque(parteLinea.Cantidad, parteLinea.Formato);
                _primerBloque.AvanzarIndice(avance);
                if (_primerBloque.Cantidad == 0)
                {
                    _primerbloqueconsiderado = true;
                }
            }
            else
            {
                if (_enumerador.MoveNext())
                {
                    int avance = Math.Min(b.Cantidad, saldoprocesar);
                    
                }
            }*/
            /*
            int baselinea = 0;
            if (linea.Parrafo != _parrafoActual)
            {
                _parrafoActual = linea.Parrafo;
                _primerbloqueconsiderado = true;
                _enumerador = linea.Parrafo.ObtenerBloques().GetEnumerator();
                baselinea = 0;
                _inicioBloque = linea.Inicio;
            }
            else
            {
                int _cantidadBloqueInicio = _primerbloque.Cantidad;
                
                while (!_primerbloqueconsiderado)
                {
                    Bloque nuevo = _primerbloque.Clonar();
                    int cantidadconsiderar = Math.Min(_primerbloque.Cantidad, linea.Cantidad);
                    nuevo.CambiarCantidad(cantidadconsiderar);
                    _primerbloque.DisminuirCantidad(cantidadconsiderar);
                    yield return nuevo;
                    
                    if (_inicioBloque+cantidadconsiderar == linea.Inicio+linea.Cantidad)
                    {
                        yield break;
                    }
                    _primerbloqueconsiderado = _primerbloque.Cantidad == 0;
                    _inicioBloque += cantidadconsiderar;
                    if (_primerbloque.Cantidad == 0)
                    {
                        break;
                    }
                }
                
            }
            Debug.Assert(_inicioBloque >= linea.Inicio);
            int _finBloque;
            while (_enumerador.MoveNext())
            {
                Bloque b=_enumerador.Current;
                _finBloque = _inicioBloque + b.Cantidad;
                int finlinea=linea.Inicio+linea.Cantidad;
                if (_finBloque > finlinea)
                {
                    Bloque b2 = b.Clonar();
                    int cantidadconsiderar = finlinea - _inicioBloque;
                    b2.CambiarCantidad(cantidadconsiderar);
                    yield return b2;
                    _primerbloque = b.Clonar();
                    _primerbloque.CambiarCantidad(b.Cantidad - cantidadconsiderar);
                    _primerbloqueconsiderado = false;
                    yield break;                    
                }
                else
                {
                    yield return b;
                }                
                _inicioBloque = _finBloque;
            }*/
            
            //if (linea.Parrafo != parrafoActual)
            //{
            //    parrafoActual = linea.Parrafo;
            //    sumainicio = 0;
            //    primerbloqueconsiderado = true;
            //    enumerador = linea.Parrafo.ObtenerBloques().GetEnumerator();
            //}
            //else
            //{
            //    if (!primerbloqueconsiderado)
            //    {
            //        int tamdib = linea.Cantidad;
            //        int tamrestante = primerbloque.Cantidad;
            //        bool enbloquediv = false;
            //        if (tamdib < tamrestante)
            //        {
            //            primerbloque.CambiarCantidad(tamdib);
            //            enbloquediv = true;
            //        }
            //        yield return primerbloque;
            //        sumainicio += primerbloque.Cantidad;
            //        if (enbloquediv)
            //        {
            //            primerbloque = primerbloque.Clonar();
            //            primerbloque.CambiarCantidad(tamrestante - tamdib);
            //            yield break;
            //        }
            //        else
            //        {
            //            primerbloqueconsiderado = true;
            //        }
                    
            //    }
            //}
            //int fin = linea.Inicio + linea.Cantidad;
            //while (enumerador.MoveNext())
            //{

            //    Bloque actual = enumerador.Current;
            //    int sumaanterior = sumainicio;
            //    sumainicio += actual.Cantidad;
            //    if (sumainicio > fin)
            //    {
            //        Bloque b = actual.Clonar();
            //        int saldofaltante = b.Cantidad;
            //        b.DisminuirCantidad(sumainicio-fin);
            //        saldofaltante = saldofaltante - b.Cantidad;
            //        yield return b;
            //        primerbloque = actual.Clonar();
            //        primerbloque.CambiarCantidad(saldofaltante);
            //        primerbloqueconsiderado = false;                    
            //        sumainicio = fin;
            //        yield break;
            //    }
            //    else
            //    {
            //        yield return actual;
            //    }
            //}        
        }
    }
}
