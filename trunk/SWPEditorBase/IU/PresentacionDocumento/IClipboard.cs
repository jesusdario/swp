/**********************************************
This Program was written by Dario Romero A.
It is licensed under the Apache License v.2.0.
***********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using SWPEditor.Dominio.TextoFormato;
using SWPEditor.Aplicacion;

namespace SWPEditor.IU.PresentacionDocumento
{
    public interface IClipboard
    {
        void Cortar(Seleccion seleccion);
        void Copiar(Seleccion seleccion);
        void Pegar(ContPresentarDocumento editor);
    }
}
