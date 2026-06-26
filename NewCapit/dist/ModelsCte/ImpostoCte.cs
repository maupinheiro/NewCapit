using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewCapit.dist.Models
{
    public class ImpostoCte
    {
        public string Imposto { get; set; }
        public decimal BaseCalculo { get; set; }
        public decimal Aliquota { get; set; }
        public decimal Valor { get; set; }

        public static decimal Calcular(decimal baseCalculo, decimal aliquota)
            => baseCalculo * aliquota / 100;
    }
}