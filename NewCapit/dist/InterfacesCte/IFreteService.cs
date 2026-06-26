using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NewCapit.dist.Models;

namespace NewCapit.dist.InterfacesCte
{
    public interface IFreteService
    {
        decimal CalcularComponentes(List<ComponenteFrete> componentes);
    }
}