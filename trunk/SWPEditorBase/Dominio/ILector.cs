using System;
using System.Collections.Generic;
using System.Text;

namespace SWPEditor.Dominio
{
    public interface ILector
    {
        void Leer(IEscritor destino);             
    }
}
