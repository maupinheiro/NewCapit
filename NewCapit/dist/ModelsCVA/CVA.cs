using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewCapit.dist.ModelsCVA
{
    public class CVA
    {

        public int Id { get; set; }

        public string TipoRegistro { get; set; }
        public int CodPlanta { get; set; }
        public string NumeroCVA { get; set; }
        public string CodTransportadora { get; set; }
        public string Placa { get; set; }
        public string CPFMotorista { get; set; }
        public DateTime? PrevChegGate { get; set; }
        public string TipoVeiculo { get; set; }
        public string TipoViagem { get; set; }
        public string CodTipoSolicitacao { get; set; }
        public string NumeroSolicitacao { get; set; }
        public string TipoGeracaoCVA { get; set; }
        public string CodJustReagendamento { get; set; }
        public string ObsJustReagendamento { get; set; }
        public string RGMotorista { get; set; }
        public string NomeMotorista { get; set; }
        public string NumRota { get; set; }
        public string Fluxo { get; set; }
        public string JustTipoVeiculo { get; set; }
        public string Conta { get; set; }
        public string Setor { get; set; }
        public decimal? KmTotal { get; set; }
        public string NumRemessa { get; set; }

        public string Situacao { get; set; }
        public string Tipo { get; set; }

        public string ViagemComRetorno { get; set; }
        public string DevolucaoPeca { get; set; }

        public string CodRemetente { get; set; }
        public string Remetente { get; set; }
        public string CNPJRemetente { get; set; }
        public string CidadeRemetente { get; set; }
        public string UFRemetente { get; set; }

        public string CodExpedidor { get; set; }
        public string Expedidor { get; set; }
        public string CNPJExpedidor { get; set; }
        public string CidadeExpedidor { get; set; }
        public string UFExpedidor { get; set; }

        public string CodDestinatario { get; set; }
        public string Destinatario { get; set; }
        public string CNPJDestinatario { get; set; }
        public string CidadeDestinatario { get; set; }
        public string UFDestinatario { get; set; }

        public string CodRecebedor { get; set; }
        public string Recebedor { get; set; }
        public string CNPJRecebedor { get; set; }
        public string CidadeRecebedor { get; set; }
        public string UFRecebedor { get; set; }

        public string CodPagador { get; set; }
        public string Pagador { get; set; }
        public string CNPJPagador { get; set; }
        public string CidadePagador { get; set; }
        public string UFPagador { get; set; }

        public string TipoCVA { get; set; }
        public string Estabelecimento { get; set; }

        public string CNPJRecebedor1 { get; set; }

        public string TipoViagemCVA { get; set; }
        public string TipoGeracao { get; set; }

        public string ContaCVA { get; set; }
        public string CentroCusto { get; set; }

        public DateTime? DataSolicitacao { get; set; }
        public DateTime? DataEntrega { get; set; }

        public string TipoSolicitacao { get; set; }
        public string TipoVeiculoCVA { get; set; }

        public string CapVeiculo { get; set; }

        public string CodMotorista { get; set; }

        public string TranspMotorista { get; set; }
        public string TranspVeiculo { get; set; }
        public string TranspReboque { get; set; }

        public string Reboque1 { get; set; }
        public string Reboque2 { get; set; }

        public string ResponsavelAbertura { get; set; }
    }
}