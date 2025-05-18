using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Web.Services.Description;
using System.Web.UI.WebControls;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace NewCapit.dist.pages
{
    public partial class OrdemColetaImpressao : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string id = Request.QueryString["id"];
                if (!string.IsNullOrEmpty(id))
                {
                    DataTable dados = ObterDadosDoBanco(id);
                    var grupos = AgruparFormulariosPorEmpresa(dados);
                    rptFormularios.DataSource = grupos;
                    rptFormularios.DataBind();
                }
            }
        }
        public class FormularioColeta
        {
            public string Numero { get; set; }
            public string Remetente { get; set; }
            public string IE { get; set; }
            public string Endereco { get; set; }
            public string CNPJ { get; set; }
            public string Motorista { get; set; }
            public string Placa { get; set; }
            public string Contato { get; set; }
            public string Pedido { get; set; }
            public string QuanPallet { get; set; }
            public string Controle { get; set; }
        }

        public class GrupoFormularios
        {
            public string Empresa { get; set; }
            public List<FormularioColeta> Formularios { get; set; }
        }

        private DataTable ObterDadosDoBanco(string id)
        {
            DataTable dt = new DataTable();

            string connectionString = WebConfigurationManager.ConnectionStrings["conexao"].ToString();

            string query = @"
                    SELECT 
                        o.carga, 
                        c.razcli, 
                        c.endcli, 
                        c.numero, 
                        c.baicli,
                        c.cidcli, 
                        c.estcli,
                        c.cnpj, 
                        c.inscestadual,
                        o.pedidos,
                        o.quant_palet,
                        m.nommot,
                        v.plavei 
                    FROM tbcargas AS o 
                    INNER JOIN tbclientes AS c ON o.codorigem = c.codvw 
                    INNER JOIN tbmotoristas AS m ON m.codmot = o.codmot 
                    INNER JOIN tbveiculos AS v ON v.codvei = o.frota 
                    WHERE o.idviagem = @idviagem";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@idviagem", id);

                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
            }

            return dt;
        }
        private List<GrupoFormularios> AgruparFormulariosPorEmpresa(DataTable dt)
        {
            var resultadoFinal = new List<GrupoFormularios>();

            foreach (DataRow row in dt.Rows)
            {
                var formularioOriginal = new FormularioColeta
                {
                    Numero = row["carga"].ToString(),
                    Remetente = row["razcli"].ToString(),
                    IE = row["inscestadual"].ToString(),
                    Endereco = $"{row["endcli"]}, {row["numero"]} - {row["baicli"]} - {row["cidcli"]}/{row["estcli"]}",
                    CNPJ = row["cnpj"].ToString(),
                    Motorista = row["nommot"].ToString(),
                    Placa = row["plavei"].ToString(),
                    Pedido = row["pedidos"].ToString(),
                    QuanPallet = row["quant_palet"].ToString(),
                    Controle = row["carga"].ToString(),
                    Contato = "" // adicionar se houver campo
                };

                // Duplicar o formulário
                var formularioCopia = new FormularioColeta
                {
                    Numero = formularioOriginal.Numero,
                    Remetente = formularioOriginal.Remetente,
                    IE = formularioOriginal.IE,
                    Endereco = formularioOriginal.Endereco,
                    CNPJ = formularioOriginal.CNPJ,
                    Motorista = formularioOriginal.Motorista,
                    Placa = formularioOriginal.Placa,
                    Contato = formularioOriginal.Contato,
                    Pedido = formularioOriginal.Pedido,
                    Controle = formularioOriginal.Controle,
                    QuanPallet = formularioOriginal.QuanPallet
                };

                // Adicionar os dois formulários no mesmo grupo (duas vias)
                var grupo = new GrupoFormularios
                {
                    Empresa = formularioOriginal.Remetente,
                    Formularios = new List<FormularioColeta> { formularioOriginal, formularioCopia }
                };

                resultadoFinal.Add(grupo);
            }

            return resultadoFinal;
        }

    }
}