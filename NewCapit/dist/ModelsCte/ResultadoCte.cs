using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewCapit.dist.Models
{
    public class ResultadoCte
    {
        public string CFOP { get; set; }

        public decimal TotalComponentes { get; set; }

        public decimal ICMS { get; set; }
        public decimal IBS { get; set; }
        public decimal CBS { get; set; }

        public decimal TotalGeral { get; set; }
    }
}