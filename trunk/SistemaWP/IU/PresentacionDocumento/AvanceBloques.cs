using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SistemaWP.Dominio;
using SistemaWP.Dominio.TextoFormato;

namespace SistemaWP.IU.PresentacionDocumento
{
    public class AvanceBloques
    {
        Parrafo parrafoActual;
        IEnumerator<Bloque> enumerador;
        Bloque primerbloque;
        bool primerbloqueconsiderado;
        int sumainicio;
        public AvanceBloques(Linea lineaInicio)
        {
            enumerador = lineaInicio.Parrafo.ObtenerBloques().GetEnumerator();
            int sumaactual = 0;
            while (enumerador.MoveNext())
            {
                Bloque bloqueactual = enumerador.Current;
                int sumaanterior = sumaactual;
                sumaactual += bloqueactual.Cantidad;
                if (sumaactual > lineaInicio.Inicio)
                {
                    //inicio de bloque
                    int posinibloque=lineaInicio.Inicio - sumaanterior;
                    int tambloquesiguiente = bloqueactual.Cantidad - posinibloque;
                    primerbloque = bloqueactual.Clonar();
                    primerbloque.DisminuirCantidad(posinibloque);
                    parrafoActual = lineaInicio.Parrafo;
                    sumainicio = lineaInicio.Inicio;
                    /*parrafoActual = lineaInicio.Parrafo;
                    bloqueactual = bloqueactual.Clonar();
                    int tammax = Math.Max(lineaInicio.Cantidad,sumaactual-lineaInicio.Inicio);
                    bloqueactual.CambiarCantidad(tammax);
                    sumainicio = sumaactual;
                    primerbloqueconsiderado = false;
                    primerbloque = bloqueactual;*/
                    break;
                }
                else
                {
                    primerbloque = bloqueactual;
                    parrafoActual = lineaInicio.Parrafo;
                }
            }
        }
        public IEnumerable<Bloque> ObtenerBloquesDe(Linea linea)
        {
            if (linea.Parrafo != parrafoActual)
            {
                parrafoActual = linea.Parrafo;
                sumainicio = 0;
                primerbloqueconsiderado = true;
                enumerador = linea.Parrafo.ObtenerBloques().GetEnumerator();
            }
            else
            {
                if (!primerbloqueconsiderado)
                {
                    int tamdib = linea.Cantidad;
                    int tamrestante = primerbloque.Cantidad;
                    bool enbloquediv = false;
                    if (tamdib < tamrestante)
                    {
                        primerbloque.CambiarCantidad(tamdib);
                        enbloquediv = true;
                    }
                    yield return primerbloque;
                    sumainicio += primerbloque.Cantidad;
                    if (enbloquediv)
                    {
                        primerbloque = primerbloque.Clonar();
                        primerbloque.CambiarCantidad(tamrestante - tamdib);
                        yield break;
                    }
                    else
                    {
                        primerbloqueconsiderado = true;
                    }
                    
                }
            }
            int fin = linea.Inicio + linea.Cantidad;
            while (enumerador.MoveNext())
            {

                Bloque actual = enumerador.Current;
                int sumaanterior = sumainicio;
                sumainicio += actual.Cantidad;
                if (sumainicio > fin)
                {
                    Bloque b = actual.Clonar();
                    int saldofaltante = b.Cantidad;
                    b.DisminuirCantidad(sumainicio-fin);
                    saldofaltante = saldofaltante - b.Cantidad;
                    yield return b;
                    primerbloque = actual.Clonar();
                    primerbloque.CambiarCantidad(saldofaltante);
                    primerbloqueconsiderado = false;                    
                    sumainicio = fin;
                    yield break;
                }
                else
                {
                    yield return actual;
                }
            }        
        }
    }
}
