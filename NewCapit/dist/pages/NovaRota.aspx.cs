using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml.Wordprocessing;
using MathNet.Numerics;
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
        string rota;
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
                btnCadastrarRota.Enabled = false;
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
                // 📍 Endereços completos (cidade, UF e país)
                string origem = $"{txtOrigem.Text.Trim()}, Brasil";
                string destino = $"{txtDestino.Text.Trim()}, Brasil";

                string key = "AIzaSyApI6da0E4OJktNZ-zZHgL6A5jtk0L6Cww";
                string url = "https://routes.googleapis.com/directions/v2:computeRoutes";

                // 📦 Corpo JSON compatível com Routes API v2
                string jsonBody = $@"
{{
  ""origin"": {{
    ""address"": ""{origem}""
  }},
  ""destination"": {{
    ""address"": ""{destino}""
  }},
  ""travelMode"": ""DRIVE"",
  ""routingPreference"": ""TRAFFIC_AWARE"",
  ""computeAlternativeRoutes"": false
}}";

                using (var client = new WebClient())
                {
                    client.Headers.Add("Content-Type", "application/json");
                    client.Headers.Add("X-Goog-Api-Key", key);
                    client.Headers.Add(
                        "X-Goog-FieldMask",
                        "routes.distanceMeters,routes.duration"
                    );

                    string response = client.UploadString(url, "POST", jsonBody);
                    dynamic data = JsonConvert.DeserializeObject(response);

                    // 🚫 Nenhuma rota encontrada
                    if (data.routes == null || data.routes.Count == 0)
                        throw new Exception("Rota não encontrada para os endereços informados.");

                    // 📏 Distância (metros → km)
                    double metros = (double)data.routes[0].distanceMeters;
                    txtDistancia.Text = (metros / 1000).ToString("0.##");

                    // ⏱ Tempo (segundos → minutos)
                    string duracaoStr = data.routes[0].duration.ToString().Replace("s", "");
                    double segundos = double.Parse(
                        duracaoStr,
                        CultureInfo.InvariantCulture
                    );

                    int minutos = (int)Math.Round(segundos / 60);
                    txtTempo.Text = minutos + " min";

                    // 🔓 Reabre o modal com os valores preenchidos
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

                MostrarMsg(
                    "Erro ao calcular rota: " + ex.Message,
                    "danger"
                );
            }
        }


        public void BuscaTempo( string origem_, string destino_)
        {
            try
            {
                string origem = HttpUtility.JavaScriptStringEncode(origem_.Trim());
                string destino = HttpUtility.JavaScriptStringEncode(destino_.Trim());
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
                    //double metros = (double)data.routes[0].distanceMeters;
                    //txtDistancia.Text = (metros / 1000).ToString("0.##");

                    // ⏱ Tempo
                    string duracao = data.routes[0].duration.ToString().Replace("s", "");
                    int minutos = int.Parse(duracao) / 60;
                    lblTempo.InnerHtml = minutos + " min";

                    //ScriptManager.RegisterStartupScript(
                    //    this,
                    //    this.GetType(),
                    //    "AbrirModal",
                    //    "abrirModal();",
                    //    true
                    //);
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
                btnCadastrarRota.Enabled = true;

                string origem = ddlCidadeOrigem.SelectedItem.Text + " - " + ddlUfOrigem.SelectedItem.Text;
                string destino = ddlCidadeDestino.SelectedItem.Text + " - " + ddlUfDestino.SelectedItem.Text;
                BuscaTempo(origem, destino);
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

            string uforigem = origem[1].ToUpper();
            string cidadeorigem = origem[0].ToUpper();
            string ufdestino = destino[1].ToUpper();
            string cidadedestino = destino[0].ToUpper();

            InsertRotaPopup(distancia, tempo,uforigem,cidadeorigem, ufdestino,cidadedestino);

            //MostrarMsg("Distância cadastrada para essa rota.", "success");
        }

        protected void AtualizarDeslocamento(object sender, EventArgs e)
        {
            // garante que tudo foi selecionado
            if (ddlUfOrigem.SelectedIndex <= 0 ||
                ddlUfDestino.SelectedIndex <= 0 ||
                ddlCidadeOrigem.SelectedIndex <= 0 ||
                ddlCidadeDestino.SelectedIndex <= 0)
            {
                lblDeslocamento.Text = "";
                return;
            }

            string ufOrigem = ddlUfOrigem.SelectedValue;
            string ufDestino = ddlUfDestino.SelectedValue;

            string cidadeOrigem = ddlCidadeOrigem.SelectedValue;
            string cidadeDestino = ddlCidadeDestino.SelectedValue;

            if (ufOrigem != ufDestino)
            {
                lblDeslocamento.Text = "INTERESTADUAL";
                lblDeslocamentoNovo.Text= "INTERESTADUAL";
            }
            else if (cidadeOrigem != cidadeDestino)
            {
                lblDeslocamento.Text = "INTERMUNICIPAL";
                lblDeslocamentoNovo.Text = "INTERMUNICIPAL";

            }
            else
            {
                lblDeslocamento.Text = "MUNICIPAL";
                lblDeslocamentoNovo.Text = "MUNICIPAL";
            }
        }
        public void InsertRota()
        {
            
            string query = "SELECT (rota + incremento) as ProximaRota FROM tbcontadores";

            // Crie uma conexão com o banco de dados
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Crie o comando SQL
                        //SqlCommand cmd = new SqlCommand(query, conn);

                        // Execute o comando e obtenha os dados em um DataReader
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                // Preencher o TextBox com o nome encontrado 
                                rota = reader["ProximaRota"].ToString();
                            }
                        }

                    }
                    string id = "1";

                    // Verifica se o ID foi fornecido e é um número válido
                    if (string.IsNullOrEmpty(id) || !int.TryParse(id, out int idConvertido))
                    {
                        // Acione o toast quando a página for carregada
                        string script = "<script>showToast('ID invalido ou não fornecido.');</script>";
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                        return;
                    }
                    string sql = @"UPDATE tbcontadores SET rota = @rota WHERE id = @id";
                    try
                    {
                        using (SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
                        using (SqlCommand cmd = new SqlCommand(sql, con))
                        {
                            cmd.Parameters.AddWithValue("@rota", rota);
                            cmd.Parameters.AddWithValue("@id", idConvertido);

                            con.Open();
                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                // atualiza  
                            }
                            else
                            {
                                // Acione o toast quando a página for carregada
                                string script = "<script>showToast('Erro ao atualizar o número da rota.');</script>";
                                ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                            }

                        }


                    }
                    catch (Exception ex)
                    {
                        string mensagemErro = $"Erro ao atualizar: {HttpUtility.JavaScriptStringEncode(ex.Message)}";
                        string script = $"alert('{mensagemErro}');";
                        ClientScript.RegisterStartupScript(this.GetType(), "Erro", script, true);
                    }
                }
                catch (Exception ex)
                {
                    //Tratar erro
                    //txtResultado.Text = "Erro: " + ex.Message;
                }
            }
            int minutos = int.Parse(lblTempo.InnerHtml.Replace(" min", ""));

            TimeSpan tempo = TimeSpan.FromMinutes(minutos);
            string resultado = tempo.ToString(@"hh\:mm\:ss");

            decimal distancia = decimal.Parse(
                lblDistancia.InnerHtml.Replace(",", "."),
                CultureInfo.InvariantCulture);

            string descr_rota =
                ddlCidadeOrigem.SelectedItem.Text.ToUpper() + "/" + ddlUfOrigem.SelectedItem.Text.ToUpper() +
                " X " +
                ddlCidadeDestino.SelectedItem.Text.ToUpper() + "/" + ddlUfDestino.SelectedItem.Text;

            using (SqlConnection conn = new SqlConnection(
                WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                string sql = @"
        IF NOT EXISTS (
            SELECT 1 
            FROM tbrotasdeentregas 
            WHERE desc_rota = @desc_rota
        )
        BEGIN
            INSERT tbrotasdeentregas
            (rota,desc_rota, cidade_expedidor, uf_expedidor,
             cidade_recebedor, uf_recebedor,
             distancia, tempo, deslocamento, pedagio)
            VALUES
            (@rota,@desc_rota, @cidade_expedidor, @uf_expedidor,
             @cidade_recebedor, @uf_recebedor,
             @distancia, @tempo, @deslocamento, @pedagio)
        END
        ELSE
        BEGIN
            RAISERROR ('ROTA_DUPLICADA', 16, 1)
        END";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@rota", rota);
                cmd.Parameters.AddWithValue("@desc_rota", descr_rota);
                cmd.Parameters.AddWithValue("@cidade_expedidor", ddlCidadeOrigem.SelectedItem.Text);
                cmd.Parameters.AddWithValue("@uf_expedidor", ddlUfOrigem.SelectedItem.Text);
                cmd.Parameters.AddWithValue("@cidade_recebedor", ddlCidadeDestino.SelectedItem.Text);
                cmd.Parameters.AddWithValue("@uf_recebedor", ddlUfDestino.SelectedItem.Text); // 👈 corrigido
                cmd.Parameters.AddWithValue("@distancia", distancia);
                cmd.Parameters.AddWithValue("@tempo", resultado);
                cmd.Parameters.AddWithValue("@deslocamento", lblDeslocamento.Text);
                cmd.Parameters.AddWithValue("@pedagio", ddlPedagio.SelectedItem.Text);

                conn.Open();

                try
                {
                    cmd.ExecuteNonQuery();
                    MostrarMsg("Rota cadastrada com sucesso!");
                }
                catch (SqlException ex)
                {
                    if (ex.Message.Contains("ROTA_DUPLICADA"))
                        MostrarMsg("Esta rota já está cadastrada.");
                    else
                        throw;
                }
            }
        }

        public void InsertRotaPopup(decimal distancia_, int tempo_, string uforigem_, string cidadeorigem_, string ufdestino_, string cidadedestino_)
        {

            string query = "SELECT (rota + incremento) as ProximaRota FROM tbcontadores";

            // Crie uma conexão com o banco de dados
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Crie o comando SQL
                        //SqlCommand cmd = new SqlCommand(query, conn);

                        // Execute o comando e obtenha os dados em um DataReader
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                // Preencher o TextBox com o nome encontrado 
                                rota = reader["ProximaRota"].ToString();
                            }
                        }

                    }
                    string id = "1";

                    // Verifica se o ID foi fornecido e é um número válido
                    if (string.IsNullOrEmpty(id) || !int.TryParse(id, out int idConvertido))
                    {
                        // Acione o toast quando a página for carregada
                        string script = "<script>showToast('ID invalido ou não fornecido.');</script>";
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                        return;
                    }
                    string sql = @"UPDATE tbcontadores SET rota = @rota WHERE id = @id";
                    try
                    {
                        using (SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
                        using (SqlCommand cmd = new SqlCommand(sql, con))
                        {
                            cmd.Parameters.AddWithValue("@rota", rota);
                            cmd.Parameters.AddWithValue("@id", idConvertido);

                            con.Open();
                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                // atualiza  
                            }
                            else
                            {
                                // Acione o toast quando a página for carregada
                                string script = "<script>showToast('Erro ao atualizar o número da rota.');</script>";
                                ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                            }

                        }


                    }
                    catch (Exception ex)
                    {
                        string mensagemErro = $"Erro ao atualizar: {HttpUtility.JavaScriptStringEncode(ex.Message)}";
                        string script = $"alert('{mensagemErro}');";
                        ClientScript.RegisterStartupScript(this.GetType(), "Erro", script, true);
                    }
                }
                catch (Exception ex)
                {
                    //Tratar erro
                    //txtResultado.Text = "Erro: " + ex.Message;
                }
            }
            //int minutos = int.Parse(lblTempo.InnerHtml.Replace(" min", ""));

            TimeSpan tempo = TimeSpan.FromMinutes(tempo_);
            string resultado = tempo.ToString(@"hh\:mm\:ss");

            //decimal distancia = decimal.Parse(
            //    lblDistancia.InnerHtml.Replace(",", "."),
            //    CultureInfo.InvariantCulture);

            string descr_rota =
                ddlCidadeOrigem.SelectedItem.Text.ToUpper() + "/" + ddlUfOrigem.SelectedItem.Text.ToUpper() +
                " X " +
                ddlCidadeDestino.SelectedItem.Text.ToUpper() + "/" + ddlUfDestino.SelectedItem.Text.ToUpper();

            using (SqlConnection conn = new SqlConnection(
                WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                string sql = @"
        IF NOT EXISTS (
            SELECT 1 
            FROM tbrotasdeentregas 
            WHERE desc_rota COLLATE Latin1_General_CI_AI = @desc_rota
        )
        BEGIN
            INSERT tbrotasdeentregas
            (rota,desc_rota, cidade_expedidor, uf_expedidor,
             cidade_recebedor, uf_recebedor,
             distancia, tempo, deslocamento, pedagio, situacao, data_cadastro, usuario_cadastro)
            VALUES
            (@rota,@desc_rota, @cidade_expedidor, @uf_expedidor,
             @cidade_recebedor, @uf_recebedor,
             @distancia, @tempo, @deslocamento, @pedagio, @situacao, @data_cadastro, @usuario_cadastro)
        END
        ELSE
        BEGIN
            RAISERROR ('ROTA_DUPLICADA', 16, 1)
        END";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@rota", rota);
                cmd.Parameters.AddWithValue("@desc_rota", descr_rota);
                cmd.Parameters.AddWithValue("@cidade_expedidor", cidadeorigem_);
                cmd.Parameters.AddWithValue("@uf_expedidor", uforigem_);
                cmd.Parameters.AddWithValue("@cidade_recebedor", cidadedestino_);
                cmd.Parameters.AddWithValue("@uf_recebedor", ufdestino_); // 👈 corrigido
                cmd.Parameters.AddWithValue("@distancia", distancia_);
                cmd.Parameters.AddWithValue("@tempo", resultado);
                cmd.Parameters.AddWithValue("@deslocamento", lblDeslocamentoNovo.Text);
                cmd.Parameters.AddWithValue("@pedagio", ddlPedagioNovo.SelectedItem.Text);
                cmd.Parameters.AddWithValue("@situacao", "ATIVO");
                cmd.Parameters.AddWithValue("@data_cadastro", DateTime.Now);
                cmd.Parameters.AddWithValue("@usuario_cadastro", Session["UsuarioLogado"]);


                conn.Open();

                try
                {
                    cmd.ExecuteNonQuery();
                    MostrarMsg("Rota cadastrada com sucesso!");
                }
                catch (SqlException ex)
                {
                    if (ex.Message.Contains("ROTA_DUPLICADA"))
                        MostrarMsg("Esta rota já está cadastrada.");
                    else
                        throw;
                }
            }
        }

        protected void btnCadastrarRota_Click(object sender, EventArgs e)
        {
            InsertRota();
        }
    }
}