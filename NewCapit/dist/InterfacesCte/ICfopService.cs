using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewCapit.dist.InterfacesCte
{
    public interface ICfopService
    {
        string ObterCFOP(string ufOrigem, string ufDestino, string tipoCte);
    }
}


