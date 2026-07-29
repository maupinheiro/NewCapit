using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class ControleFaltasDTO
    {
        public int Id { get; set; }
        public string CodMot { get; set; }
        public string NomMot { get; set; }
        public string Funcao { get; set; }
        public string Nucleo { get; set; }
        public DateTime DataFalta { get; set; }
        public string Motivo { get; set; }
        public string Observacao { get; set; }
        public string Usuario { get; set; }
        public DateTime DataCadastro { get; set; }        
        
    }
}
