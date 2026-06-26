using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewCapit.dist.InterfacesCte
{
    public interface IImpostoService
    {
        decimal Calcular(decimal baseCalculo, decimal aliquota);
    }
}