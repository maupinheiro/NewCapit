using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewCapit.dist.pages
{
    public class Jornada
    {
        public int linha { get; set; }
        public string dt_jornada{get;set;}
        public int    cod_login {get;set;}
        public string hr_inicio_jornada {get;set;}
        public string hr_inicio_intervalo {get;set;}
        public string hr_fim_intervalo {get;set;}
        public string hr_fim_jornada {get;set;}
        public string vl_almoco {get;set;}
        public string vl_janta {get;set;}
        public string vl_pernoite {get;set;}
        public string vl_premio { get; set; }
        public string total { get; set; }
    }
}