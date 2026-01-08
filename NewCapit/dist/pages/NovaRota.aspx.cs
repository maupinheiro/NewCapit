using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Script.Serialization;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Web.UI;
using System.Collections.Generic;
using System.Web.Services;
using System.Web.Configuration;
using System.Web.UI.WebControls;
using DocumentFormat.OpenXml.Presentation;

namespace NewCapit.dist.pages
{
    public partial class NovaRota : System.Web.UI.Page
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["conexao"].ToString());
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UsuarioLogado"] != null)
                {
                    string nomeUsuario = Session["UsuarioLogado"].ToString();
                    var lblUsuario = nomeUsuario;
                    //txtUsuCadastro.Text = nomeUsuario;
                }
                else
                {                                  
                    Response.Redirect("Login.aspx");
                }
                DateTime dataHoraAtual = DateTime.Now;
                //lblDtCadastro.Text = dataHoraAtual.ToString("dd/MM/yyyy HH:mm");
                CarregarUFs();
            }

        }
        private void CarregarUFs()
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT DISTINCT SiglaUf FROM tbestadosbrasileiros ORDER BY SiglaUf", conn);

                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                ddlUfOrigem.Items.Clear();
                ddlUfDestino.Items.Clear();

                ddlUfOrigem.Items.Add("-- Selecione a UF --");
                ddlUfDestino.Items.Add("-- Selecione a UF --");

                while (dr.Read())
                {
                    ddlUfOrigem.Items.Add(dr["SiglaUf"].ToString());
                    ddlUfDestino.Items.Add(dr["SiglaUf"].ToString());
                }
            }
        }
        protected void ddlUfOrigem_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarCidades(ddlUfOrigem.SelectedValue, ddlCidadeOrigem);
        }

        protected void ddlUfDestino_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarCidades(ddlUfDestino.SelectedValue, ddlCidadeDestino);
        }

        private void CarregarCidades(string uf, DropDownList ddl)
        {
            ddl.Items.Clear();

            if (uf == "-- UF --") return;

            using (SqlConnection conn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT nome_municipio FROM tbmunicipiosbrasileiros WHERE sigla = @sigla ORDER BY nome_municipio", conn);

                cmd.Parameters.AddWithValue("@sigla", uf);
                conn.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                ddl.Items.Add("-- Selecione a Cidade --");

                while (dr.Read())
                {
                    ddl.Items.Add(dr["nome_municipio"].ToString());
                }
            }
        }
        // [System.Web.Services.WebMethod]
        public DistanciaTempoDTO BuscarDistancia(
     string ufOrigem, string origem,
     string ufDestino, string destino)
        {
            DistanciaTempoDTO resultado = null;

            using (SqlConnection conn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(@"
            SELECT TOP 1 Distancia, tempo_min
            FROM tbdistanciapremio
            WHERE 
            (
                UF_Origem COLLATE Latin1_General_CI_AI = @ufo
                AND Origem COLLATE Latin1_General_CI_AI = @origem
                AND UF_Destino COLLATE Latin1_General_CI_AI = @ufd
                AND Destino COLLATE Latin1_General_CI_AI = @destino
            )
            OR
            (
                UF_Origem COLLATE Latin1_General_CI_AI = @ufd
                AND Origem COLLATE Latin1_General_CI_AI = @destino
                AND UF_Destino COLLATE Latin1_General_CI_AI = @ufo
                AND Destino COLLATE Latin1_General_CI_AI = @origem
            )", conn);

                cmd.Parameters.Add("@ufo", SqlDbType.VarChar).Value = ufOrigem.Trim();
                cmd.Parameters.Add("@origem", SqlDbType.VarChar).Value = origem.Trim();
                cmd.Parameters.Add("@ufd", SqlDbType.VarChar).Value = ufDestino.Trim();
                cmd.Parameters.Add("@destino", SqlDbType.VarChar).Value = destino.Trim();

                conn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        resultado = new DistanciaTempoDTO
                        {
                            Distancia = dr["Distancia"] != DBNull.Value
                                ? Convert.ToDecimal(dr["Distancia"])
                                : 0,

                            TempoMinutos = dr["tempo_min"] != DBNull.Value
                                ? Convert.ToInt32(dr["tempo_min"])
                                : 0
                        };
                    }
                }
            }

            return resultado; // ✅ sempre retorna
        }
        protected void btnCalcular_Click(object sender, EventArgs e)
        {
            var dados = BuscarDistancia(
                ddlUfOrigem.SelectedValue,
                ddlCidadeOrigem.SelectedItem.Text,
                ddlUfDestino.SelectedValue,
                ddlCidadeDestino.SelectedItem.Text
            );

            if (dados != null)
            {
                lblDistancia.InnerText = dados.Distancia.ToString("N2");
                lblTempo.InnerText = dados.TempoMinutos.ToString();
               
            }
            else
            {
                MostrarMsg("Distância não cadastrada para essa rota.", "info");
                lblDistancia.InnerText = "";
                lblTempo.InnerText = "";
            }
        }
        public class DistanciaTempoDTO
        {
            public decimal Distancia { get; set; }
            public int TempoMinutos { get; set; }
        }
        protected void MostrarMsg(string mensagem, string tipo = "warning")
        {
            divMsg.Attributes["class"] = "alert alert-" + tipo + " alert-dismissible fade show mt-3";
            lblMsgGeral.InnerText = mensagem;
            divMsg.Style["display"] = "block";

            string script = @"setTimeout(function() {
                        var div = document.getElementById('divMsg');
                        if (div) div.style.display = 'none';
                      }, 5000);";

            ScriptManager.RegisterStartupScript(this, GetType(), "EscondeMsg", script, true);
        }
    }
}