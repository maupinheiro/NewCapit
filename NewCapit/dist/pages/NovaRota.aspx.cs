using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml.Wordprocessing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
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
        protected void btnDistancia_Click(object sender, EventArgs e)
        {
            try
            {
                string origem = HttpUtility.JavaScriptStringEncode(txtOrigem.Text.Trim());
                string destino = HttpUtility.JavaScriptStringEncode(txtDestino.Text.Trim());
                string key = "AIzaSyApI6da0E4OJktNZ-zZHgL6A5jtk0L6Cww";

                string url = "https://routes.googleapis.com/directions/v2:computeRoutes";

               

                string jsonBody = $@"
        {{
            ""origin"": {{ ""address"": ""{origem}"" }},
            ""destination"": {{ ""address"": ""{destino}"" }},
            ""travelMode"": ""DRIVE""
        }}";

                using (var client = new WebClient())
                {
                    client.Headers.Add("Content-Type", "application/json");
                    client.Headers.Add("X-Goog-Api-Key", key);
                    client.Headers.Add("X-Goog-FieldMask", "routes.distanceMeters,routes.duration");

                    string response = client.UploadString(url, "POST", jsonBody);
                    dynamic data = JsonConvert.DeserializeObject(response);

                    if (data.routes == null || data.routes.Count == 0)
                        throw new Exception("Rota não encontrada");

                    // 📏 Distância
                    double metros = (double)data.routes[0].distanceMeters;
                    txtDistancia.Text = (metros / 1000).ToString("0.##");

                    // ⏱ Tempo
                    string duracao = data.routes[0].duration.ToString().Replace("s", "");
                    int minutos = int.Parse(duracao) / 60;
                    txtTempo.Text = minutos + " min";

                    ScriptManager.RegisterStartupScript(
                        this,
                        this.GetType(),
                        "AbrirModal",
                        "abrirModal();",
                        true
                    );
                }
            }
            catch (Exception ex)
            {
                txtDistancia.Text = "0";
                txtTempo.Text = "0";

                MostrarMsg("Erro ao calcular rota: " + ex.Message, "danger");
            }

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
                //MostrarMsg("Distância não cadastrada para essa rota.", "info");
                //lblDistancia.InnerText = "";
                //lblTempo.InnerText = "";

                txtOrigem.Text = ddlCidadeOrigem.SelectedItem.Text  + " - " + ddlUfOrigem.SelectedItem.Text;
                txtDestino.Text = ddlCidadeDestino.SelectedItem.Text  + " - " + ddlUfDestino.SelectedItem.Text;

                ScriptManager.RegisterStartupScript(this, this.GetType(), "abrirModal", "abrirModal();", true);
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

        protected void btnCadastrar_Click(object sender, EventArgs e)
        {
            string[] origem = txtOrigem.Text.Split(new[] { " - " }, StringSplitOptions.RemoveEmptyEntries);
            string[] destino = txtDestino.Text.Split(new[] { " - " }, StringSplitOptions.RemoveEmptyEntries);

            if (origem.Length != 2 || destino.Length != 2)
            {
                MostrarMsg("Formato inválido. Use: Cidade - UF", "warning");
                return;
            }

            decimal distancia = decimal.Parse(
                txtDistancia.Text.Replace(",", "."),
                CultureInfo.InvariantCulture
            );

            int tempo = int.Parse(txtTempo.Text.Replace(" min", ""));

            using (SqlConnection conn = new SqlConnection(
                WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                string sql = @"
        INSERT INTO tbdistanciapremio
        (UF_Origem, Origem, UF_Destino, Destino, Distancia, tempo_min)
        VALUES
        (@UF_Origem, @Origem, @UF_Destino, @Destino, @Distancia, @tempo_min)";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@UF_Origem", origem[1].ToUpper());
                cmd.Parameters.AddWithValue("@Origem", origem[0].ToUpper());
                cmd.Parameters.AddWithValue("@UF_Destino", destino[1].ToUpper());
                cmd.Parameters.AddWithValue("@Destino", destino[0].ToUpper());
                cmd.Parameters.AddWithValue("@Distancia", distancia);
                cmd.Parameters.AddWithValue("@tempo_min", tempo);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            MostrarMsg("Distância cadastrada para essa rota.", "success");
        }

    }
}