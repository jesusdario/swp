using System;
using System.Collections.Generic;
using System.Text;
using SistemaWP.Dominio;
using System.Drawing;
using SistemaWP.Aplicacion;
using System.Windows.Forms;
using SistemaWP.IU.Graficos;

namespace SistemaWP.IU.PresentacionDocumento
{
    public  class Linea
    {
        //List<Bloque> _bloques = new List<Bloque>();
        public Parrafo Parrafo { get; set; }
        public int Inicio { get; set; }
        public int Cantidad { get; set; }
        public Medicion AltoLinea { get; set; }
        private void Normalizar(ref int posicion)
        {
            if (posicion > Cantidad)
                posicion = Cantidad;
            if (posicion < 0)
                posicion = 0;
            
        }
       
        public void Completar(List<Linea> lineas,Posicion posicionLinea,int numCaracter)
        {
            Normalizar(ref numCaracter);
            if (posicionLinea.ReferenciaX.HasValue)
            {
                numCaracter=ObtenerNumCaracter(posicionLinea.ReferenciaX.Value);
            }
            posicionLinea.PosicionCaracter = numCaracter;
           
        }
        public void CompletarPosicionPixels(List<Linea> lineas, Posicion posicionLinea)
        {
            Medicion suma = new Medicion(0);
            for (int i = posicionLinea.Pagina.LineaInicio; i < posicionLinea.IndiceLinea; i++)
            {
                suma += lineas[i].AltoLinea;
            }
            posicionLinea.PosicionPagina = new Punto(ObtenerPosicionCaracter(posicionLinea.PosicionCaracter),suma);
            //posicionLinea.PosicionPixelY = suma;
            //posicionLinea.PosicionPixelX = ObtenerPosicionCaracter(posicionLinea.PosicionCaracter);
            posicionLinea.AltoLinea = AltoLinea;
        }

        public Medicion ObtenerPosicionCaracter(int numcaracter)
        {
            if (numcaracter < 0) numcaracter = 0;
            string cad = Parrafo.ToString().Substring(Inicio, Math.Min(numcaracter,Cantidad));
            Estilo e = new Estilo();
            return e.Medir(cad).Ancho;
        }
        public int ObtenerNumCaracter(Medicion posicionx)
        {
            if (Cantidad == 0) return 0;
            if (posicionx < new Medicion(0))
            {
                posicionx = new Medicion(0);
                return 0;
            }
            string linea = Parrafo.ToString().Substring(Inicio, Cantidad);
            Estilo e = new Estilo();
            TamBloque tam=e.Medir(linea);
            Medicion anchocaracterespromedio = tam.Ancho / linea.Length;
            if (posicionx > tam.Ancho)
            {
                return Cantidad;
            }
            else
            {
                int posicionestimada = (int)(posicionx/anchocaracterespromedio);//((posicionx / anchocaracterespromedio) * tam.Width);
                Medicion pos = ObtenerPosicionCaracter(posicionestimada);
                while (posicionestimada>0&&pos - posicionx > Medicion.Cero)
                {
                    pos = ObtenerPosicionCaracter(posicionestimada);
                    posicionestimada--;
                }
                while (posicionestimada < Cantidad && pos - posicionx < Medicion.Cero)
                {
                    pos = ObtenerPosicionCaracter(posicionestimada);
                    posicionestimada++;
                }
                if (posicionestimada > 0 && pos - posicionx > Medicion.Cero)
                {
                    pos = ObtenerPosicionCaracter(posicionestimada);
                    posicionestimada--;
                }
                Normalizar(ref posicionestimada);
                return posicionestimada;                
            }
        }
        public Punto Dibujar(IGraficador g,Punto posicion,Seleccion seleccion)
        {
            try
            {
                string tot = Parrafo.ObtenerSubCadena(Inicio, Cantidad);
                if (seleccion != null)
                {
                    Parrafo inicial = seleccion.ObtenerParrafoInicial();
                    Parrafo final = seleccion.ObtenerParrafoFinal();
                    Estilo e = new Estilo();
                    Estilo sel = new Estilo();
                    sel.ColorFondo = new BrochaSolida() { Color = new ColorDocumento(0, 0, 0) };
                    sel.ColorLetra = new BrochaSolida() { Color = new ColorDocumento(255, 255, 255) };
                    int ini = seleccion.ObtenerPosicionInicial();
                    int fin = seleccion.ObtenerPosicionFinal();
                    Punto possiguiente = posicion;
                    if (inicial == Parrafo)
                    {
                        if (final == Parrafo)
                        {
                            ini -= Inicio;
                            fin -= Inicio;
                            if (ini < 0) ini = 0;
                            if (ini > Cantidad) ini = Cantidad;
                            if (fin < 0) fin = 0;
                            if (fin > Cantidad) fin = Cantidad;
                            string c1=tot.Substring(0, ini);
                            possiguiente = e.DibujarConTam(g, possiguiente, c1,null);                            
                            string c2=tot.Substring(ini, fin - ini);
                            possiguiente = sel.DibujarConTam(g, possiguiente, c2,c1);
                            possiguiente = e.DibujarConTam(g, possiguiente, tot.Substring(fin, Cantidad - fin),c2);
                        }
                        else
                        {
                            ini -= Inicio;
                            if (ini < 0) ini = 0;
                            if (ini > Cantidad) ini = Cantidad;
                            string c1 = tot.Substring(0, ini);
                            possiguiente = e.DibujarConTam(g, possiguiente, c1,null);
                            possiguiente = sel.DibujarConTam(g, possiguiente, tot.Substring(ini, Cantidad - ini),c1);
                        }
                    }
                    else
                    {
                        if (final == Parrafo)
                        {
                            fin -= Inicio;
                            if (fin < 0) fin = 0;
                            if (fin > Cantidad) fin = Cantidad;
                            string c1 = tot.Substring(0, fin);
                            possiguiente = sel.DibujarConTam(g, possiguiente, c1,null);
                            possiguiente = e.DibujarConTam(g, possiguiente, tot.Substring(fin, Cantidad - fin),c1);
                        }
                        else
                        {
                            if (inicial.EsSiguiente(Parrafo) && !final.EsSiguiente(Parrafo))
                            {
                                sel.Dibujar(g, possiguiente, tot);
                            }
                            else
                            {
                                e.Dibujar(g, possiguiente, tot);
                            }
                        }
                    }
                }
                else
                {
                    Estilo e = new Estilo();
                    e.Dibujar(g, posicion, tot);
                }
                return new Punto(posicion.X, posicion.Y + AltoLinea);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message+"\r\n"+ex.StackTrace, "Error");
                throw ex;
            }
        }
        public static int ObtenerAnteriorDivision(string cadena, int posicion)
        {
            for (int i = posicion-1; i > 0; i--)
            {
                if (cadena[i-1] == ' ')
                {
                    return i;
                }
            }
            return posicion - 1;
        }
        
        internal static Linea ObtenerSiguienteLinea(Parrafo parrafo, int caracterinicio, Medicion ancho)
        {            
            int tamparrafo = parrafo.ObtenerLongitud();
            Estilo e = new Estilo();
            if (tamparrafo == 0)
            {
                Medicion alto=e.Medir(string.Empty).Alto;
                return new Linea() 
                {   Inicio = caracterinicio, 
                    Cantidad = 0, 
                    Parrafo = parrafo, 
                    AltoLinea = alto };
            }
            TamBloque tampromedio=e.Medir("MMMM");
            Medicion anchocaracter = tampromedio.Ancho / 4;
            int numcaracteres=(int)(ancho/anchocaracter);
            int limitecaracteres = tamparrafo - caracterinicio;
            if (numcaracteres>limitecaracteres) {
                numcaracteres=limitecaracteres;
            }
            TamBloque bloque=e.Medir(parrafo.ObtenerSubCadena(caracterinicio, numcaracteres));
            while (numcaracteres < limitecaracteres && bloque.Ancho < ancho)
            {
                bloque=e.Medir(parrafo.ObtenerSubCadena(caracterinicio, numcaracteres));
                numcaracteres++;
            }            
            while (numcaracteres > 0 && bloque.Ancho > ancho)
            {
                string subcad = parrafo.ObtenerSubCadena(caracterinicio, numcaracteres);
                bloque = e.Medir(subcad);
                if (numcaracteres > 0 && bloque.Ancho > ancho)
                {
                    numcaracteres = ObtenerAnteriorDivision(subcad, numcaracteres);
                }
            }            
            return new Linea() { Parrafo = parrafo, Cantidad = numcaracteres, Inicio = caracterinicio,AltoLinea=tampromedio.Alto };
        }

        internal TamBloque ObtenerMargenEdicion()
        {
            Estilo e = new Estilo();
            return e.Medir("MM");
        }
    }    
}
