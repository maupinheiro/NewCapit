using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class ConsultaMotorista
    {
        public string codmot { get; set; }
        public string nommot { get; set; }
        public string status { get; set; }
        public string nucleo { get; set; }
        public string cpf { get; set; }
        public DateTime venccnh { get; set; }
        public string fone2 { get; set; }
        public string validade { get; set; }
        public string codliberacao { get; set; }    
        public string cartaomot { get; set; }
        public string venccartao { get; set; }
        public string venceti {  get; set; }
        public string caminhofoto { get; set; }
        public string fl_exclusao { get; set; }
        public string tipomot { get; set; }

        public string frota { get; set; }
        public string codvei { get; set; }  
        public string placa { get; set; }
        public string reboque1 { get; set; }
        public string reboque2 { get; set; }
        public string tipoveiculo { get; set; }
        public string cargo { get; set; }
        public string veiculotipo { get; set; }
        public string codtranspmotorista { get; set; }
        public string nomtranspmotorista { get; set; }
        public string codtra { get; set; }
        public string transp { get; set; }
    }
}
