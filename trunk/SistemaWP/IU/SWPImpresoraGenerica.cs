using System;
using System.Collections.Generic;
using System.Text;
using SWPEditor.IU.PresentacionDocumento;
using SWPEditor.Dominio;
using SWPEditor.IU.Graficos;

namespace SWPEditor.IU
{
    public class SWPGenericPrinter
    {
        int? numpagina;
        DocumentoImpreso _documento;
        public bool ImpresionCompleta { get; private set; }
        public SWPGenericPrinter(Documento documento)
        {
            _documento = new DocumentoImpreso(documento);
        }
        public TamBloque GetNextPageSize()
        {
            if (!numpagina.HasValue)
            {
                numpagina = 0;
            }
            //int lim = escritorio.Controlador.Documento.ObtenerNumPaginas();
            Pagina pag = _documento.ObtenerPagina(numpagina.Value);

            return pag.Dimensiones;
        }
        public bool PrintNextPage(IGraficador graficador)
        {
            if (!numpagina.HasValue)
            {
                numpagina = 0;
            }
            bool resultado = false;
            if (_documento.EsUltimaPagina(numpagina.Value))
            {
                resultado = false;
            }
            _documento.DibujarPagina(graficador, new Punto(Medicion.Cero, Medicion.Cero), numpagina.Value, null);
            numpagina++;
            if (!resultado) 
                ImpresionCompleta = true;
            return resultado;
        }
    }
}
