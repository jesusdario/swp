using System;
using System.Collections.Generic;
using System.Text;
using SWPEditor.IU.PresentacionDocumento;
using SWPEditor.Dominio;
using SWPEditor.Aplicacion;
using System.Diagnostics;
using SWPEditor.Dominio.TextoFormato;

namespace SWPEditor.Tests
{
    public class PruebaBloques
    {
        public void ProbarBloques()
        {
            Documento documento = new SWPEditor.Dominio.Documento();
            DocumentoImpreso docimp = new DocumentoImpreso(documento);
            ContPresentarDocumento cont = new ContPresentarDocumento(documento);
            cont.InsertarTexto("HOLA, esta es una prueba de texto que deberá distribuirse adecuadamente en la página");
            cont.IrAInicioDocumento(false);
            cont.IrSiguienteCaracter(false, TipoAvance.AvanzarPorPalabras);
            cont.IrSiguienteCaracter(false, TipoAvance.AvanzarPorPalabras);
            cont.IrSiguienteCaracter(true, TipoAvance.AvanzarPorPalabras);
            cont.IrSiguienteCaracter(true, TipoAvance.AvanzarPorPalabras);
            for (int i = 0; i <= 50; i++)
            {
                cont.AgrandarLetra();
                Pagina pag=docimp.ObtenerPagina(0);
                int contador = 0;
                foreach (Linea l in pag.ObtenerLineas())
                {
                    if (contador < 2)
                    {
                        contador++;
                        continue;
                    }
                    Debug.Assert(l.AnchoLinea <= pag.ObtenerAnchoLinea(pag.LineaInicio));
                    AvanceBloques av = new AvanceBloques(l);
                    IEnumerable<Bloque> bloques=av.ObtenerBloquesDe(l);
                    int suma=0;
                    foreach (Bloque b in bloques) {
                        suma+=b.Cantidad;
                    }
                    Debug.Assert(suma == l.Cantidad);
                    contador++;
                }
            }
        }
    }
}
