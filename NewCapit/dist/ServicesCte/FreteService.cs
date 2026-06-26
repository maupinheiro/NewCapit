using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NewCapit.dist.Models;

namespace NewCapit.dist.ServicesCte
{
    public class FreteService
    {
        public decimal CalcularComponentes(List<ComponenteFrete> componentes)
        {
            return componentes.Sum(x => x.Valor);
        }
    }
}