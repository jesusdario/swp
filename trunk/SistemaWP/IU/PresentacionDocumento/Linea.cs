using System;
using System.Collections.Generic;
using System.Text;
using SistemaWP.Dominio;
using System.Drawing;
using SistemaWP.Aplicacion;
using System.Windows.Forms;
using SistemaWP.IU.Graficos;
using SistemaWP.Dominio.TextoFormato;
using System.Diagnostics;
using System.Linq;
using SistemaWP.Dominio.TextoFormato;

namespace SistemaWP.IU.PresentacionDocumento
{
    public  class Linea
    {
        //List<Bloque> _bloques = new List<Bloque>();
        public Parrafo Parrafo { get; private set; }
        public int Inicio { get; private set; }
        public int Cantidad { get; private set; }
        public Medicion AltoLinea { get; private set; }
        public Medicion AltoBase { get; private set; }
        public Medicion AnchoLinea { get; private set; }
        private Linea(Parrafo parrafo, int inicio, int cantidad, Medicion altoLinea, Medicion altoBase,Medicion anchoLinea)
        {
            Parrafo = parrafo; Inicio = inicio; Cantidad = cantidad; AltoLinea = altoLinea; AltoBase = altoBase;
            AnchoLinea = anchoLinea;
        }
        private void Normalizar(ref int posicion)
        {
            if (posicion > Cantidad)
                posicion = Cantidad;
            if (posicion < 0)
                posicion = 0;
            
        }

        public void Completar(ListaLineas lineas, Posicion posicionLinea, Medicion anchoLinea, int numCaracter)
        {
            Normalizar(ref numCaracter);
            if (posicionLinea.ReferenciaX.HasValue)
            {
                numCaracter = ObtenerNumCaracter(posicionLinea.ReferenciaX.Value,anchoLinea);
            }
            posicionLinea.PosicionCaracter = numCaracter;
           
        }
        public void CompletarPosicionPixels(ListaLineas lineas, Posicion posicionLinea, Medicion anchoLinea, bool incluirEspacioAnterior, bool incluirEspacioPosterior)
        {
            Medicion suma = new Medicion(0);
            for (int i = posicionLinea.Pagina.LineaInicio; i < posicionLinea.IndiceLinea; i++)
            {
                suma += lineas.Obtener(i).AltoLinea;
            }
            Medicion deltaAnterior=(incluirEspacioAnterior && EsPrimeraLineaParrafo ? Parrafo.Formato.ObtenerEspacioAnterior() : Medicion.Cero);
            Medicion deltaSiguiente = (incluirEspacioPosterior && EsUltimaLineaParrafo ? Parrafo.Formato.ObtenerEspacioPosterior() : Medicion.Cero);
            posicionLinea.PosicionPagina = new Punto(
                ObtenerPosicionCaracter(posicionLinea.PosicionCaracter)
                + ObtenerDeltaAlineacion(anchoLinea)
                , suma + deltaAnterior);
            posicionLinea.AltoLinea = AltoLinea
                - deltaSiguiente
                - deltaAnterior;
        }
        public TamBloque MedirDeParrafo(int inicio, int cantidad,out Medicion maximaBase)
        {
            Debug.Assert(inicio >= 0&&inicio+cantidad<=Parrafo.ObtenerLongitud()&&cantidad>=0);
            
            AvanceBloques av = new AvanceBloques(this);
            int inicioact=inicio;
            TamBloque bq = new TamBloque(Medicion.Cero, Medicion.Cero);
            Medicion maxbase = Medicion.Cero;
            foreach (Bloque b in av.ObtenerBloquesDe(this))
            {
                Estilo e = new Estilo( b);
                Medicion medicionbase = e.MedirBase();
                if (medicionbase > maxbase)
                {
                    maxbase = medicionbase;
                }
                int cantidadbloque=Math.Min(b.Cantidad,cantidad);
                TamBloque tm=e.Medir(Parrafo.ObtenerSubCadena(inicioact, cantidadbloque));
                if (tm.Alto > bq.Alto) bq.Alto = tm.Alto;
                bq.Ancho = bq.Ancho + tm.Ancho;
                cantidad -= cantidadbloque;
                inicioact += cantidadbloque;
            }
            maximaBase = maxbase;
            return bq;
        }
        public Medicion ObtenerPosicionCaracter( int numcaracter,out Medicion mbase)
        {
            if (numcaracter < 0) numcaracter = 0;
            //string cad = Parrafo.ToString().Substring(Inicio, Math.Min(numcaracter,Cantidad));
            return MedirDeParrafo(Inicio, numcaracter, out mbase).Ancho;
        }
        public Medicion ObtenerPosicionCaracter(int numcaracter)
        {
            if (numcaracter < 0) numcaracter = 0;
            //string cad = Parrafo.ToString().Substring(Inicio, Math.Min(numcaracter,Cantidad));
            Medicion mbase;
            return MedirDeParrafo(Inicio,numcaracter,out mbase).Ancho;
        }
        public bool EsPrimeraLineaParrafo
        {
            get
            {
                return Inicio == 0;
            }
        }
        public bool EsUltimaLineaParrafo
        {
            get
            {
                return Inicio + Cantidad == Parrafo.ObtenerLongitud();
            }
        }
        public int ObtenerNumCaracter(Medicion posicionx,Medicion anchoLinea)
        {
            if (Cantidad == 0) return 0;
            posicionx -= ObtenerDeltaAlineacion(anchoLinea);
            if (posicionx < new Medicion(0))
            {
                posicionx = new Medicion(0);
                return 0;
            }
            
            string linea = Parrafo.ToString().Substring(Inicio, Cantidad);
            Medicion maxbase;
            TamBloque tam = MedirDeParrafo(Inicio,Cantidad,out maxbase);
            Medicion anchocaracterespromedio = tam.Ancho / linea.Length;
            if (posicionx > tam.Ancho)
            {
                return Cantidad;
            }
            else
            {
                int posicionestimada = (int)(posicionx/anchocaracterespromedio);//((posicionx / anchocaracterespromedio) * tam.Width);
                Medicion pos = ObtenerPosicionCaracter(posicionestimada, out maxbase);
                while (posicionestimada>0&&pos - posicionx > Medicion.Cero)
                {
                    posicionestimada--;
                    pos = ObtenerPosicionCaracter(posicionestimada);
                }
                while (posicionestimada < Cantidad-1 && pos - posicionx < Medicion.Cero)
                {
                    posicionestimada++;
                    pos = ObtenerPosicionCaracter(posicionestimada, out maxbase);
                }
                if (posicionestimada > 0 && pos - posicionx > Medicion.Cero)
                {
                    posicionestimada--;
                    pos = ObtenerPosicionCaracter(posicionestimada, out maxbase);
                }
                Normalizar(ref posicionestimada);
                return posicionestimada;                
            }
        }
        private Medicion ObtenerDeltaAlineacion(Medicion anchoLinea)
        {
            AlineacionParrafo alineacion = Parrafo.Formato.ObtenerAlineacionHorizontal();
            Medicion deltacalc = Medicion.Cero;
            if (alineacion == AlineacionParrafo.Derecha || alineacion == AlineacionParrafo.Centro)
            {
                Medicion altobase;
                TamBloque tam = MedirDeParrafo(Inicio, Cantidad, out altobase);
                Medicion anchotexto = tam.Ancho;
                if (alineacion == AlineacionParrafo.Derecha)
                    deltacalc = anchoLinea - anchotexto;
                else
                    deltacalc = (anchoLinea - anchotexto) * 0.5;
            }
            return deltacalc;
        }
        public Punto Dibujar(IGraficador g, Punto posicionInicial, Seleccion seleccion, AvanceBloques avance,Medicion anchoLinea,bool incluirEspacioAnterior,bool incluirEspacioPosterior)
        {
            Bloque[] lista=avance.ObtenerBloquesDe(this).ToArray();
            bool esultimalinea=EsUltimaLineaParrafo;
            Medicion espacioanterior = EsPrimeraLineaParrafo?(incluirEspacioAnterior ? (Parrafo.Formato.ObtenerEspacioAnterior()) : Medicion.Cero):Medicion.Cero;
            Medicion espacioposterior= EsUltimaLineaParrafo?(incluirEspacioPosterior?(Parrafo.Formato.ObtenerEspacioPosterior()):Medicion.Cero):Medicion.Cero;
            Medicion altolinea = AltoLinea - espacioposterior;
            Medicion deltacalc = ObtenerDeltaAlineacion(anchoLinea);
            AlineacionParrafo alineacion = Parrafo.Formato.ObtenerAlineacionHorizontal();
            Punto posicion = new Punto(posicionInicial.X, posicionInicial.Y);
            for (int i = 0; i < 2; i++)
            {
                Medicion deltax;
                if (alineacion == AlineacionParrafo.Centro || alineacion == AlineacionParrafo.Derecha)
                {
                    deltax = posicion.X + deltacalc;
                }
                else
                {
                    deltax = posicion.X;
                }
                Punto pt = posicion;
                int posbase = Inicio;
                
                Medicion altobase = AltoBase;
                foreach (Bloque b in lista)
                {
                    Estilo e = new Estilo(b);
                    Medicion baset = e.MedirAlto();
                    string total = Parrafo.ObtenerSubCadena(posbase, b.Cantidad);
                    TamBloque tb = e.Medir(total);
                    Punto posdibujo = new Punto(deltax, posicion.Y + (altolinea - baset) - altobase);
                    Dibujar(g, posdibujo, seleccion, e, posbase, b.Cantidad, total,i);
                    posbase += b.Cantidad;
                    deltax += tb.Ancho;
                }
            }
            return new Punto(posicion.X, posicion.Y + AltoLinea);
        }
        private void Dibujar(Estilo e, IGraficador graficos, Punto posicionbase, string texto, int numpasada)
        {
            if (numpasada == 0)
            {
                e.DibujarFondo(graficos, posicionbase, texto,"");
            }
            else
            {
                e.Dibujar(graficos, posicionbase, texto);
            }
        }
        private Punto DibujarConTam(Estilo e,IGraficador graficos, Punto posicionbase, string texto, string anteriortexto,int numpasada)
        {
            if (numpasada == 0)
            {
                return e.DibujarFondo(graficos, posicionbase, texto,"");
            }
            else
            {
                return e.DibujarConTam(graficos, posicionbase, texto, anteriortexto);
            }
        }
        private void Dibujar(IGraficador g,Punto posicion,Seleccion seleccion,Estilo estiloBase,int Inicio,int Cantidad,string subcadena,int contador)
        {
            try
            {
                string tot = subcadena;//Parrafo.ObtenerSubCadena(Inicio, Cantidad);
                if (seleccion != null)
                {
                    Parrafo inicial = seleccion.ObtenerParrafoInicial();
                    Parrafo final = seleccion.ObtenerParrafoFinal();
                    Estilo e = estiloBase;
                    Estilo sel = estiloBase.Clonar();
                    sel.ColorFondo = new BrochaSolida(new ColorDocumento(0, 0, 0));
                    sel.ColorLetra = new BrochaSolida(new ColorDocumento(255, 255, 255));
                    string c1=null, c2=null;
                    //for (int contador = 0; contador <= 1; contador++)
                    {
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
                                c1 = c1 ?? tot.Substring(0, ini);
                                c2 = c2 ?? tot.Substring(ini, fin - ini);
                                possiguiente = DibujarConTam(e, g, possiguiente, c1, null, contador);
                                possiguiente = DibujarConTam(sel, g, possiguiente, c2, c1, contador);
                                possiguiente = DibujarConTam(e, g, possiguiente, tot.Substring(fin, Cantidad - fin), c2, contador);
                            }
                            else
                            {
                                ini -= Inicio;
                                if (ini < 0) ini = 0;
                                if (ini > Cantidad) ini = Cantidad;
                                c1 = c1 ?? tot.Substring(0, ini);
                                possiguiente = DibujarConTam(e, g, possiguiente, c1, null, contador);
                                possiguiente = DibujarConTam(sel, g, possiguiente, tot.Substring(ini, Cantidad - ini), c1, contador);
                            }
                        }
                        else
                        {
                            if (final == Parrafo)
                            {
                                fin -= Inicio;
                                if (fin < 0) fin = 0;
                                if (fin > Cantidad) fin = Cantidad;
                                c1 = c1 ?? tot.Substring(0, fin);
                                possiguiente = DibujarConTam(sel, g, possiguiente, c1, null, contador);
                                possiguiente = DibujarConTam(e, g, possiguiente, tot.Substring(fin, Cantidad - fin), c1, contador);
                            }
                            else
                            {
                                if (inicial.EsSiguiente(Parrafo) && !final.EsSiguiente(Parrafo))
                                {
                                    Dibujar(sel, g, possiguiente, tot, contador);
                                }
                                else
                                {
                                    Dibujar(e, g, possiguiente, tot, contador);
                                }
                            }
                        }
                    }
                }
                else
                {
                    //for (int contador = 0; contador <= 1; contador++)
                    {
                        Dibujar(estiloBase, g, posicion, tot, contador);
                    }
                }
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
        
        internal static Linea ObtenerSiguienteLinea(Parrafo parrafo, int caracterinicio, Medicion ancho,bool incluirAltoParrafo,bool incluirBaseParrafo)
        {            
            int tamparrafo = parrafo.ObtenerLongitud();
            //Estilo e = new Estilo();
            if (tamparrafo == 0)
            {
                Estilo estparrafo=new Estilo(new Bloque(0,null));
                Medicion alto = estparrafo.Medir(string.Empty).Alto;//e.Medir(string.Empty).Alto;
                if (incluirAltoParrafo)
                {
                    alto += parrafo.Formato.ObtenerEspacioAnterior();
                }
                if (incluirBaseParrafo)
                {
                    alto += parrafo.Formato.ObtenerEspacioPosterior();
                }
                ;
                return new Linea(parrafo,caracterinicio,0,alto,estparrafo.MedirBase(),ancho);/*
                {   Inicio = caracterinicio, 
                    Cantidad = 0, 
                    Parrafo = parrafo, 
                    AltoLinea = alto,
                    AltoBase = estparrafo.MedirAlto()
                };*/
            }
            Linea actual=new Linea(parrafo,caracterinicio,0,Medicion.Cero,Medicion.Cero,ancho);
/*                Parrafo = parrafo, 
                Cantidad = 0, 
                Inicio = caracterinicio, 
                AltoLinea = Medicion.Cero
            };*/
            TamBloque tampromedio=new Estilo(new Bloque(4,null)).Medir("MMMM");
            
            Medicion anchocaracter = tampromedio.Ancho / 4;
            int numcaracteres=(int)(ancho/anchocaracter);
            int limitecaracteres = tamparrafo - caracterinicio;
            if (numcaracteres>limitecaracteres) {
                numcaracteres=limitecaracteres;
            }
            actual.Cantidad = numcaracteres;
            Medicion mbase;
            TamBloque bloque = actual.MedirDeParrafo(actual.Inicio, actual.Cantidad, out mbase);//e.Medir(parrafo.ObtenerSubCadena(caracterinicio, numcaracteres));
            while (numcaracteres < limitecaracteres && bloque.Ancho < ancho)
            {
                actual.Cantidad = numcaracteres;
                bloque = actual.MedirDeParrafo(caracterinicio, numcaracteres, out mbase);//parrafo.ObtenerSubCadena(caracterinicio, numcaracteres));
                numcaracteres++;
            }            
            while (numcaracteres > 0 && bloque.Ancho > ancho)
            {
                actual.Cantidad = numcaracteres;
                //string subcad = parrafo.ObtenerSubCadena(caracterinicio, numcaracteres);
                bloque = actual.MedirDeParrafo(caracterinicio, numcaracteres, out mbase);// e.Medir(subcad);
                if (numcaracteres > 0 && bloque.Ancho > ancho)
                {
                    string subcad = parrafo.ObtenerSubCadena(caracterinicio, numcaracteres);
                    numcaracteres = ObtenerAnteriorDivision(subcad, numcaracteres);
                }
            }
            actual.AltoBase = mbase;
            actual.Cantidad = numcaracteres;
            actual.AltoLinea = bloque.Alto;
            actual.AnchoLinea = ancho;
            if (parrafo.Formato.ObtenerEspaciadoInterlineal() != 1)
            {
                actual.AltoLinea = actual.AltoLinea*(float)parrafo.Formato.ObtenerEspaciadoInterlineal();
            }
            if (incluirAltoParrafo&&actual.Inicio==0)
            {
                actual.AltoLinea += parrafo.Formato.ObtenerEspacioAnterior();
            }
            if (incluirBaseParrafo&&actual.Inicio+actual.Cantidad==tamparrafo)
            {
                actual.AltoLinea += parrafo.Formato.ObtenerEspacioPosterior();
            }
            return actual;
            //return new Linea() { Parrafo = parrafo, Cantidad = numcaracteres, Inicio = caracterinicio,AltoLinea=tampromedio.Alto };
        }

        internal TamBloque ObtenerMargenEdicion()
        {
            Estilo e = new Estilo(new Bloque(0,null));
            return e.Medir("MM");
        }
    }    
}
