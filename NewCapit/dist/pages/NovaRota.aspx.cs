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
                    txtUsuCadastro.Text = nomeUsuario;
                }
                else
                {                                  
                    Response.Redirect("Login.aspx");
                }
                DateTime dataHoraAtual = DateTime.Now;
                lblDtCadastro.Text = dataHoraAtual.ToString("dd/MM/yyyy HH:mm");
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

                ddlUfOrigem.Items.Add("-- UF --");
                ddlUfDestino.Items.Add("-- UF --");

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

                ddl.Items.Add("-- Cidade --");

                while (dr.Read())
                {
                    ddl.Items.Add(dr["nome_municipio"].ToString());
                }
            }
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