/**********************************************
This Program was written by Dario Romero A.
It is licensed under the Apache License v.2.0.
***********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using SWPEditor.Dominio.TextoFormato;
using System.Diagnostics;
using SWPEditor.Dominio;

namespace SWPEditor.Tests
{
    public class PruebaTexto
    {
        public void ProbarAdicion()
        {
            Texto t = new Texto();
            t.Iniciar();
            t.Append("HOLA");
            t.Append(" PRUEBA DE ");
            t.Append("TEXTO");
            Debug.Assert(t.ToString() == "HOLA PRUEBA DE TEXTO");
        }
        Texto ObtenerTextoPrueba()
        {
            Texto t = new Texto();
            t.Iniciar();
            t.Append("HOLA");
            t.Append(" PRUEBA DE ");
            t.Append("TEXTO");
            t.AplicarFormato(Formato.CrearNegrilla(true), 2, 9);
            t.AplicarFormato(Formato.CrearCursiva(true), 5, 6);
            t.AplicarFormato(Formato.CrearEscalaLetra(1.1f), 8, 10);
            t.AplicarFormato(Formato.CrearSubrayado(true), t.Length,t.Length-5);
            return t;
        }
        private Texto ObtenerTextoFormato()
        {
            Texto t = new Texto();
            t.Iniciar();
            t.Append("HOLA");
            t.Append(" PRUEBA DE ");
            t.Append("TEXTO");
            t.AplicarFormato(Formato.CrearTamLetra(new Medicion(5, Unidad.Puntos)),0,6);
            t.AplicarFormato(Formato.CrearNegrilla(true), 2, 9);
            t.AplicarFormato(Formato.CrearCursiva(true), 5, 6);
            t.AplicarFormato(Formato.CrearEscalaLetra(1.1f), 8, 10);
            t.AplicarFormato(Formato.CrearEscalaLetra(1.1f), 1, 2);
            t.AplicarFormato(Formato.CrearSubrayado(true), t.Length-5,  5);
            //Revisar negrilla
            foreach (IndiceBloque i in t.ObtenerIndices())
            {
                for (int o = 0; o < 6; o++)
                {
                    if (i.ContieneCaracter(o))
                    {
                        Debug.Assert(i.Formato.TamLetra.Value.Valor == 5 && i.Formato.TamLetra.Value.Unidad == Unidad.Puntos);
                    }
                }
                for (int o = 2; o < 2+9; o++)
                {
                    if (i.ContieneCaracter(o))
                    {
                        Debug.Assert(i.Formato.Negrilla.Value == true);
                    }                
                }
                for (int o = 5; o < 5+6; o++)
                {
                    if (i.ContieneCaracter(o))
                    {
                        Debug.Assert(i.Formato.Cursiva.Value == true);
                    }
                }
                for (int o = 8; o < 8+10; o++)
                {
                    if (i.ContieneCaracter(o))
                    {
                        Debug.Assert(i.Formato.ObtenerEscalaLetra() == 1.1F);
                    }
                }
                for (int o = t.Length-5; o < t.Length; o++)
                {
                    if (i.ContieneCaracter(o))
                    {
                        Debug.Assert(i.Formato.Subrayado==true);
                    }
                }
            }
            t.SimplificarFormato();
            return t;
        }
        public void ProbarFormato()
        {
            Texto a = ObtenerTextoFormato();
        }
        public void ProbarInsercion()
        {
            Texto a = ObtenerTextoFormato();
            int q = 0;
            //Inserción al inicio de bloque (aumenta el bloque anterior)
            foreach (IndiceBloque i in a.ObtenerIndices())
            {
                if (i.Inicio!=0)
                {
                    Bloque c=a.ObtenerBloque(q-1);
                    int cantidad = c.Cantidad;
                    a.Insert(i.Inicio, "HOLA");
                    c = a.ObtenerBloque(q-1);
                    Debug.Assert(c.Cantidad == cantidad + 4);
                    break;
                }
                q++;
            }
            q = 0;
            //Inserción al fin del bloque (en posición del último caracter del bloque)
            foreach (IndiceBloque i in a.ObtenerIndices())
            {
                if (i.Inicio != 0&&i.Cantidad>4)
                {
                    Bloque c = a.ObtenerBloque(q);
                    int cantidad = c.Cantidad;
                    a.Insert(i.Inicio+c.Cantidad-1, "HOLA");
                    c = a.ObtenerBloque(q);
                    Debug.Assert(c.Cantidad == cantidad + 4);
                    break;
                }
                q++;
            }
            q = 0;
            //Insercion al medio del bloque
            foreach (IndiceBloque i in a.ObtenerIndices())
            {
                if (i.Inicio != 0&&i.Cantidad>4)
                {
                    Bloque c = a.ObtenerBloque(q);
                    int cantidad = c.Cantidad;
                    a.Insert(i.Inicio + 2, "HOLA");
                    c = a.ObtenerBloque(q);
                    Debug.Assert(c.Cantidad == cantidad + 4);
                    break;
                }
                q++;
            }
            //Inserción al inicio del texto
            Bloque ab=a.ObtenerBloque(0);
            int cantidad1=ab.Cantidad;
            a.Insert(0, "ABCDEF");
            ab = a.ObtenerBloque(0);
            Debug.Assert(ab.Cantidad == cantidad1 + 6);
            //Inserción al fin del texto
            Bloque ab2 = a.ObtenerBloque(a.ObtenerNumBloques()-1);
            cantidad1 = ab2.Cantidad;
            a.Insert(a.Length, "ABCDEF");
            ab = a.ObtenerBloque(a.ObtenerNumBloques() - 1);
            Debug.Assert(ab.Cantidad == cantidad1 + 6);
        }
        public void ProbarEliminacion()
        {
            Texto a = ObtenerTextoFormato();
            //Eliminación de bloque completo
            int q = 0;
            foreach (IndiceBloque i in a.ObtenerIndices())
            {
                if (i.Inicio != 0&&i.Cantidad>3)
                {
                    Bloque c = a.ObtenerBloque(q);
                    int cantidadanterior = a.Length;
                    int bloquesant = a.ObtenerNumBloques();
                    a.Remove(i.Inicio, i.Cantidad);
                    Debug.Assert(a.Length == cantidadanterior - i.Cantidad);
                    Debug.Assert(a.ObtenerNumBloques() == bloquesant - 1);
                    break;
                }
                q++;
            }
            a = ObtenerTextoFormato();
            q = 0;
            //Eliminación al inicio del bloque (no completa)
            foreach (IndiceBloque i in a.ObtenerIndices())
            {
                if (i.Inicio != 0 && i.Cantidad > 3)
                {
                    Bloque c = a.ObtenerBloque(q);
                    int cantidadanterior = a.Length;
                    int bloquesant = a.ObtenerNumBloques();
                    a.Remove(i.Inicio, i.Cantidad-2);
                    Debug.Assert(a.Length == cantidadanterior - (i.Cantidad-2));
                    Debug.Assert(a.ObtenerNumBloques() == bloquesant);
                    break;
                }
                q++;
            }
            a = ObtenerTextoFormato();
            //Eliminación del medio del bloque hasta el final
            q = 0;
            foreach (IndiceBloque i in a.ObtenerIndices())
            {
                if (i.Inicio != 0 && i.Cantidad > 3)
                {
                    Bloque c = a.ObtenerBloque(q);
                    int cantidadanterior = a.Length;
                    int bloquesant = a.ObtenerNumBloques();
                    a.Remove(i.Inicio+1, i.Cantidad-1);
                    Debug.Assert(a.Length == cantidadanterior - (i.Cantidad-1));
                    Debug.Assert(a.ObtenerNumBloques() == bloquesant);
                    break;
                }
                q++;
            }
            a = ObtenerTextoFormato();
            //Eliminación al medio del bloque
            q = 0;
            foreach (IndiceBloque i in a.ObtenerIndices())
            {
                if (i.Inicio != 0 && i.Cantidad > 3)
                {
                    Bloque c = a.ObtenerBloque(q);
                    int cantidadanterior = a.Length;
                    int bloquesant = a.ObtenerNumBloques();
                    a.Remove(i.Inicio + 1, i.Cantidad - 2);
                    Debug.Assert(a.Length == cantidadanterior - (i.Cantidad-2));
                    Debug.Assert(a.ObtenerNumBloques() == bloquesant);
                    break;
                }
                q++;
            }
            a = ObtenerTextoFormato();
            //Eliminación de varios bloques completos
            q = 0;
            foreach (IndiceBloque i in a.ObtenerIndices())
            {
                if (i.Inicio != 0 && i.Cantidad > 3)
                {
                    Bloque c = a.ObtenerBloque(q);
                    Bloque d = a.ObtenerBloque(q+1);
                    Bloque e = a.ObtenerBloque(q + 2);
                    int cantidadanterior = a.Length;
                    int bloquesant = a.ObtenerNumBloques();
                    int deltaelim=c.Cantidad+d.Cantidad+e.Cantidad;
                    a.Remove(i.Inicio, deltaelim);
                    Debug.Assert(a.Length == cantidadanterior - deltaelim);
                    Debug.Assert(a.ObtenerNumBloques() == bloquesant-3);
                    break;
                }
                q++;
            }
            a = ObtenerTextoFormato();
            //Eliminación con bloque final incompleto
            q = 0;
            foreach (IndiceBloque i in a.ObtenerIndices())
            {
                if (i.Inicio != 0 && i.Cantidad > 3)
                {
                    Bloque c = a.ObtenerBloque(q);
                    Bloque d = a.ObtenerBloque(q + 1);
                    Bloque e = a.ObtenerBloque(q + 2);
                    int cantidadanterior = a.Length;
                    int bloquesant = a.ObtenerNumBloques();
                    int cantidadelim=c.Cantidad + d.Cantidad + e.Cantidad-1;
                    a.Remove(i.Inicio, cantidadelim);
                    Debug.Assert(a.Length == cantidadanterior - cantidadelim);
                    Debug.Assert(a.ObtenerNumBloques() == bloquesant - 2);
                    break;
                }
                q++;
            }
            a = ObtenerTextoFormato();
            //Eliminación con bloque inicial incompleto
            q = 0;
            foreach (IndiceBloque i in a.ObtenerIndices())
            {
                if (i.Inicio != 0 && i.Cantidad > 3)
                {
                    Bloque c = a.ObtenerBloque(q);
                    Bloque d = a.ObtenerBloque(q + 1);
                    Bloque e = a.ObtenerBloque(q + 2);
                    int cantidadanterior = a.Length;
                    int bloquesant = a.ObtenerNumBloques();
                    int cantidadelim = c.Cantidad + d.Cantidad + e.Cantidad - 1;
                    a.Remove(i.Inicio+1, cantidadelim);
                    Debug.Assert(a.Length == cantidadanterior - cantidadelim);
                    Debug.Assert(a.ObtenerNumBloques() == bloquesant - 2);
                    break;
                }
                q++;
            }
            a = ObtenerTextoFormato();
            //Eliminación con bloque inicial/final incompleto
            q = 0;
            foreach (IndiceBloque i in a.ObtenerIndices())
            {
                if (i.Inicio != 0 && i.Cantidad > 3)
                {
                    Bloque c = a.ObtenerBloque(q);
                    Bloque d = a.ObtenerBloque(q + 1);
                    Bloque e = a.ObtenerBloque(q + 2);
                    int cantidadanterior = a.Length;
                    int bloquesant = a.ObtenerNumBloques();
                    int cantidadelim = c.Cantidad + d.Cantidad + e.Cantidad - 1 -1;
                    a.Remove(i.Inicio + 1, cantidadelim);
                    Debug.Assert(a.Length == cantidadanterior - cantidadelim);
                    Debug.Assert(a.ObtenerNumBloques() == bloquesant - 1);
                    break;
                }
                q++;
            }
        }
    }
}
