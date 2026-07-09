using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NewCapit.dist.InterfacesCte;
using NewCapit.dist.Models;
namespace NewCapit.dist.ServicesCte
{
    public class FreteService : IFreteService
    {
        public decimal CalcularComponentes(List<ComponenteFrete> componentes)
        {
            return componentes.Sum(x => x.Valor);
        }
    }
}