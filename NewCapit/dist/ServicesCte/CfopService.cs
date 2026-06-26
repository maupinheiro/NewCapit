using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewCapit.dist.ServicesCte
{
    public class CfopService
    {
        public string ObterCFOP(string ufOrigem, string ufDestino, string tipoCte)
        {
            if (tipoCte == "NORMAL")
                return ufOrigem == ufDestino ? "5352" : "6352";

            if (tipoCte == "COMPLEMENTAR")
                return ufOrigem == ufDestino ? "5351" : "6351";

            if (tipoCte == "ANULACAO")
                return ufOrigem == ufDestino ? "5206" : "6206";

            if (tipoCte == "SUBSTITUICAO")
                return ufOrigem == ufDestino ? "5352" : "6352";

            return "6352";
        }
    }
}