using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace NewCapit.dist.pages
{
    public class ControlesCarga
    {
        public int Carga { get; set; }

        // Gerais
        public string Status { get; set; }
        public string Andamento { get; set; }
        public string CVA { get; set; }

        public DateTime? SaidaOrigem { get; set; }
        public DateTime? ChegadaDestino { get; set; }
        public DateTime? SaidaPlanta { get; set; }
       
        public DateTime? GateOrigem { get; set; }
        public DateTime? GateDestino { get; set; }

        public DateTime? VeiculoDisponivel { get; set; }
        public DateTime? PrevisaoChegada { get; set; }

        public string TempoAgCarreg { get; set; }
        public string TempoAgDescarga { get; set; }
        public string DuracaoTransporte { get; set; }

        public string CodMotorista { get; set; }
        public string Frota { get; set; }

        // Krona
        public string Percurso { get; set; }
        public string RotaKrona { get; set; }
        public string IdRotaKrona { get; set; }
        public decimal? ValorTotal { get; set; }
        public DateTime? PrevisaoInicioKrona { get; set; }
        public DateTime? PrevisaoTerminoKrona { get; set; }
        public int SM { get; set; }
        public string SmEnviadaPor { get; set; }
        public decimal txtPeso { get; set; }

        // Pernoite
        public string LocalPernoite { get; set; }
        public DateTime? InicioPernoite { get; set; }
        public DateTime? FimPernoite { get; set; }
        public string DuracaoPernoite { get; set; }
        public string statusPernoite { get; set; }

        //Nota Fiscal        
        public GridView GvNF { get; set; }
        public Label lblPeso { get; set; }
        public Label LblPesoCTe { get; set; }
        public Label LblValorMercCTe { get; set; }
        public Label lblPesoCarregadoCTe { get; set; }
        public Label lblMaterialCTe { get; set; }
        public Label lblDescricaoMaterialCTe { get; set; }
        public Label lblFreteUnitCTe { get; set; }
        public Label lblFreteCTePor { get; set; }
        public Label lblFreteTercAgregCTe { get; set; }
        public Label LblTotal { get; set; }
        public Label LblCBS { get; set; }
        public Label LblIBS { get; set; }
        public Label LblICMS { get; set; }
        public Label LblTotalFrete { get; set; }

        public RepeaterItem Item { get; }

        public TextBox txtCVA;
        public TextBox txtSaidaOrigem;
        public TextBox txtChegadaDestino;
        public TextBox txtSaidaPlanta;
        public TextBox txtGateOrigem;
        public TextBox txtGateDestino;
        public TextBox txtAgCarreg;
        public TextBox txtAgDescarga;
        public TextBox txtDurTransp;

        public TextBox txtCodMotorista;
        public TextBox txtCodVeiculo;

        public DropDownList ddlStatus;
          

    }
}