//using DocumentFormat.OpenXml.Drawing.Charts;
//using DocumentFormat.OpenXml.Office2010.Excel;
//using DocumentFormat.OpenXml.Presentation;
//using DocumentFormat.OpenXml.Wordprocessing;
using iText.Signatures.Validation.Report.Pades;
using MathNet.Numerics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Net;
using System.Text;
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
                cmd.Parameters.AddWithValue("@Origem", RemoverAcentos(origem[0].ToUpper()));
                cmd.Parameters.AddWithValue("@UF_Destino", destino[1].ToUpper());
                cmd.Parameters.AddWithValue("@Destino", RemoverAcentos(destino[0].ToUpper()));
                cmd.Parameters.AddWithValue("@Distancia", distancia);
                cmd.Parameters.AddWithValue("@tempo_min", tempo);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            string uforigem = origem[1].ToUpper();
            string cidadeorigem = RemoverAcentos(origem[0].ToUpper());
            string ufdestino = destino[1].ToUpper();
            string cidadedestino = RemoverAcentos(destino[0].ToUpper());

            //InsertRotaPopup(distancia, tempo,uforigem,cidadeorigem, ufdestino,cidadedestino);

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

            string cidadeOrigem = RemoverAcentos(ddlCidadeOrigem.SelectedValue);
            string cidadeDestino = RemoverAcentos(ddlCidadeDestino.SelectedValue);

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
            string usuario = Session["UsuarioLogado"].ToString();
            string descr_rota =
                RemoverAcentos(ddlCidadeOrigem.SelectedItem.Text.ToUpper()) + "/" + ddlUfOrigem.SelectedItem.Text.ToUpper() +
                " X " +
                RemoverAcentos(ddlCidadeDestino.SelectedItem.Text.ToUpper()) + "/" + ddlUfDestino.SelectedItem.Text;
            bool rotaExiste = false;

            using (SqlConnection conn = new SqlConnection(
                WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                string sqlExiste = @"
                    IF EXISTS (
                        SELECT 1
                        FROM tbrotasdeentregas
                        WHERE desc_rota = @desc_rota
                    )
                        SELECT 1
                    ELSE
                        SELECT 0";

                SqlCommand cmdExiste = new SqlCommand(sqlExiste, conn);
                cmdExiste.Parameters.AddWithValue("@desc_rota", descr_rota);

                conn.Open();

                rotaExiste = Convert.ToInt32(cmdExiste.ExecuteScalar()) == 1;
            }
           

            // Crie uma conexão com o banco de dados
            if (!rotaExiste)
            {
                string query = "SELECT (rota + incremento) as ProximaRota FROM tbcontadores";

                using (SqlConnection conn = new SqlConnection(
                    WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            rota = reader["ProximaRota"].ToString();
                        }
                    }
                }

                // Atualiza o contador somente para nova rota
                string sqlContador = @"UPDATE tbcontadores SET rota = @rota WHERE id = 1";

                using (SqlConnection con = new SqlConnection(
                    WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
                using (SqlCommand cmd = new SqlCommand(sqlContador, con))
                {
                    cmd.Parameters.AddWithValue("@rota", rota);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                //InsertTbDistancia();
            }
            else
            {
                // Busca o código da rota existente
                using (SqlConnection conn = new SqlConnection(
                    WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
                {
                    string sql = @"SELECT rota
                       FROM tbrotasdeentregas
                       WHERE desc_rota = @desc_rota";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@desc_rota", descr_rota);

                    conn.Open();

                    object result = cmd.ExecuteScalar();

                    if (result != null)
                        rota = result.ToString();
                }
            }
            int minutos = int.Parse(lblTempo.InnerHtml.Replace(" min", ""));

            TimeSpan tempo = TimeSpan.FromMinutes(minutos);
            string resultado = tempo.ToString(@"hh\:mm\:ss");


            decimal distancia = 0;

            string valor = lblDistancia.InnerHtml.Trim();

            if (decimal.TryParse(
                    valor.Replace(",", "."),
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out distancia))
            {
                // sucesso
            }
            else
            {
                // valor inválido
                distancia = 0;
            }

            using (SqlConnection conn = new SqlConnection(
                WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                
                string sql = @"
                IF EXISTS (
                    SELECT 1
                    FROM tbrotasdeentregas
                    WHERE desc_rota = @desc_rota
                )
                BEGIN
                    UPDATE tbrotasdeentregas
                        SET
                            desc_rota = @desc_rota,
                            cidade_expedidor = @cidade_expedidor,
                            uf_expedidor = @uf_expedidor,
                            cidade_recebedor = @cidade_recebedor,
                            uf_recebedor = @uf_recebedor,
                            distancia = @distancia,
                            tempo = @tempo,
                            deslocamento = @deslocamento,
                            valor_icms = @valor_icms,
                            valor_pis = @valor_pis,
                            valor_cofins = @valor_cofins,
                            valor_irpj = @valor_irpj,
                            valor_csll = @valor_csll,
                            valor_ibs = @valor_ibs,
                            valor_cbs = @valor_cbs,
                            valor_iss = @valor_iss,
                            valor_sestsenat = @valor_sestsenat,
                            valor_inss = @valor_inss,
                            data_alteracao = GETDATE(),
                            usuario_alteracao = @usuario_alteracao,
                            pedagio = @pedagio
                        WHERE desc_rota = @desc_rota
                END
                ELSE
                BEGIN
                    INSERT INTO tbrotasdeentregas
                        (
                            rota,
                            desc_rota,
                            cidade_expedidor,
                            uf_expedidor,
                            cidade_recebedor,
                            uf_recebedor,
                            distancia,
                            tempo,
                            deslocamento,
                            situacao,
                            valor_icms,
                            valor_pis,
                            valor_cofins,
                            valor_irpj,
                            valor_csll,
                            valor_ibs,
                            valor_cbs,
                            valor_iss,
                            valor_sestsenat,
                            valor_inss,
                            data_cadastro,
                            usuario_cadastro,
                            pedagio
                        )
                        VALUES
                        (
                            @rota,
                            @desc_rota,
                            @cidade_expedidor,
                            @uf_expedidor,
                            @cidade_recebedor,
                            @uf_recebedor,
                            @distancia,
                            @tempo,
                            @deslocamento,
                            'ATIVO',
                            @valor_icms,
                            @valor_pis,
                            @valor_cofins,
                            @valor_irpj,
                            @valor_csll,
                            @valor_ibs,
                            @valor_cbs,
                            @valor_iss,
                            @valor_sestsenat,
                            @valor_inss,
                            CONVERT(VARCHAR(16), GETDATE(), 103) + ' ' + LEFT(CONVERT(VARCHAR(8), GETDATE(), 108), 5),
                            @usuario_cadastro,
                            @pedagio
                        )
                END";

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@rota", rota);
                cmd.Parameters.AddWithValue("@desc_rota", descr_rota);
                cmd.Parameters.AddWithValue("@cidade_expedidor", RemoverAcentos(ddlCidadeOrigem.SelectedItem.Text));
                cmd.Parameters.AddWithValue("@uf_expedidor", ddlUfOrigem.SelectedItem.Text);
                cmd.Parameters.AddWithValue("@cidade_recebedor", RemoverAcentos(ddlCidadeDestino.SelectedItem.Text));
                cmd.Parameters.AddWithValue("@uf_recebedor", ddlUfDestino.SelectedItem.Text);
                cmd.Parameters.AddWithValue("@distancia", distancia);
                cmd.Parameters.AddWithValue("@tempo", resultado);
                cmd.Parameters.AddWithValue("@deslocamento", lblDeslocamento.Text);
                cmd.Parameters.AddWithValue("@pedagio", ddlPedagio.SelectedItem.Text);
                cmd.Parameters.Add("@valor_icms", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtICMS_I.Text);
                cmd.Parameters.Add("@valor_pis", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtPIS_I.Text);
                cmd.Parameters.Add("@valor_cofins", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtCOFINS_I.Text);
                cmd.Parameters.Add("@valor_irpj", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtIRPJ_I.Text);
                cmd.Parameters.Add("@valor_csll", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtCSLL_I.Text);
                cmd.Parameters.Add("@valor_ibs", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtIBS_I.Text);
                cmd.Parameters.Add("@valor_cbs", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtCBS_I.Text);
                cmd.Parameters.Add("@valor_iss", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtISS_I.Text);
                cmd.Parameters.Add("@valor_sestsenat", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtSestSenat_I.Text);
                cmd.Parameters.Add("@valor_inss", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtINSS_I.Text);
                cmd.Parameters.AddWithValue("@usuario_cadastro", usuario);
                cmd.Parameters.AddWithValue("@usuario_alteracao", usuario);
                conn.Open();

                cmd.ExecuteNonQuery();
                if (rotaExiste)
                {
                    MostrarMsg("Rota atualizada com sucesso!");
                }
                else
                {
                    MostrarMsg(rota + " - Rota salva com sucesso!");
                }


                btnCadastrarRota.Enabled = false;
            }
        }
        private decimal LimparMascaraMoeda(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
            {
                return 0m;
            }
            // Remove pontos de milhar e substitui vírgula decimal por ponto
            string valorLimpo = valor.Replace(".", "").Replace(",", ".");
            if (decimal.TryParse(valorLimpo, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal resultado))
            {
                return resultado;
            }
            return 0m;
        }

       // public void InsertRotaPopup(decimal distancia_, int tempo_, string uforigem_, string cidadeorigem_, string ufdestino_, string cidadedestino_)
       // {

       //     decimal? distancia = null;
       //     int? tempo = null;

       //     // DISTÂNCIA
       //     if (!string.IsNullOrWhiteSpace(txtDistancia.Text))
       //     {
       //         decimal d;

       //         if (!decimal.TryParse(
       //                 txtDistancia.Text.Replace(",", "."),
       //                 NumberStyles.Any,
       //                 CultureInfo.InvariantCulture,
       //                 out d))
       //         {
       //             MostrarMsg("Distância inválida.", "warning");
       //             return;
       //         }

       //         distancia = d;
       //     }

       //     // TEMPO
       //     if (!string.IsNullOrWhiteSpace(txtTempo.Text))
       //     {
       //         int t;

       //         if (!int.TryParse(
       //                 txtTempo.Text.Replace(" min", "").Trim(),
       //                 out t))
       //         {
       //             MostrarMsg("Tempo inválido.", "warning");
       //             return;
       //         }

       //         tempo = t;
       //     }

       //     using (SqlConnection conn = new SqlConnection(
       //         WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
       //     {
       //         string sql = @"
       // IF EXISTS (
       //     SELECT 1
       //     FROM tbdistanciapremio
       //     WHERE
       //         UF_Origem = @UF_Origem
       //         AND Origem = @Origem
       //         AND UF_Destino = @UF_Destino
       //         AND Destino = @Destino
       // )
       // BEGIN
       //     UPDATE tbdistanciapremio
       //     SET
       //         Distancia = @Distancia,
       //         tempo_min = @tempo_min
       //     WHERE
       //         UF_Origem = @UF_Origem
       //         AND Origem = @Origem
       //         AND UF_Destino = @UF_Destino
       //         AND Destino = @Destino
       // END
       //";

       //         using (SqlCommand cmd = new SqlCommand(sql, conn))
       //         {
       //             cmd.Parameters.Add("@UF_Origem", SqlDbType.Char, 2)
       //                 .Value = RemoverAcentos(ddlUfOrigem.SelectedItem.Text.ToUpper());

       //             cmd.Parameters.Add("@Origem", SqlDbType.VarChar, 100)
       //                 .Value = RemoverAcentos(ddlCidadeOrigem.SelectedItem.Text.ToUpper());

       //             cmd.Parameters.Add("@UF_Destino", SqlDbType.Char, 2)
       //                 .Value = RemoverAcentos(ddlUfDestino.SelectedItem.Text.ToUpper());

       //             cmd.Parameters.Add("@Destino", SqlDbType.VarChar, 100)
       //                 .Value = RemoverAcentos(ddlCidadeDestino.SelectedItem.Text.ToUpper());

       //             cmd.Parameters.Add("@Distancia", SqlDbType.Decimal)
       //                 .Value = distancia.HasValue
       //                     ? (object)distancia.Value
       //                     : DBNull.Value;

       //             cmd.Parameters.Add("@tempo_min", SqlDbType.Int)
       //                 .Value = tempo.HasValue
       //                     ? (object)tempo.Value
       //                     : DBNull.Value;

       //             conn.Open();

       //             try
       //             {
       //                 cmd.ExecuteNonQuery();

       //                 MostrarMsg(rota + " - Rota cadastrada com sucesso!");
       //                 btnCadastrarRota.Enabled = false;
       //             }
       //             catch (Exception)
       //             {
       //                 MostrarMsg("Erro ao salvar os dados.", "danger");
       //             }
       //         }
       //     }

       // }

        protected void btnCadastrarRota_Click(object sender, EventArgs e)
        {
            InsertRota();
           
        }
       // public void InsertTbDistancia()
       // {
       //     decimal? distancia = null;
       //     int? tempo = null;

       //     // DISTÂNCIA
       //     if (!string.IsNullOrWhiteSpace(txtDistancia.Text))
       //     {
       //         decimal d;

       //         if (!decimal.TryParse(
       //                 txtDistancia.Text.Replace(",", "."),
       //                 NumberStyles.Any,
       //                 CultureInfo.InvariantCulture,
       //                 out d))
       //         {
       //             MostrarMsg("Distância inválida.", "warning");
       //             return;
       //         }

       //         distancia = d;
       //     }

       //     // TEMPO
       //     if (!string.IsNullOrWhiteSpace(txtTempo.Text))
       //     {
       //         int t;

       //         if (!int.TryParse(
       //                 txtTempo.Text.Replace(" min", "").Trim(),
       //                 out t))
       //         {
       //             MostrarMsg("Tempo inválido.", "warning");
       //             return;
       //         }

       //         tempo = t;
       //     }

       //     using (SqlConnection conn = new SqlConnection(
       //         WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
       //     {
       //         string sql = @"
       // IF EXISTS (
       //     SELECT 1
       //     FROM tbdistanciapremio
       //     WHERE
       //         UF_Origem = @UF_Origem
       //         AND Origem = @Origem
       //         AND UF_Destino = @UF_Destino
       //         AND Destino = @Destino
       // )
       // BEGIN
       //     UPDATE tbdistanciapremio
       //     SET
       //         Distancia = @Distancia,
       //         tempo_min = @tempo_min
       //     WHERE
       //         UF_Origem = @UF_Origem
       //         AND Origem = @Origem
       //         AND UF_Destino = @UF_Destino
       //         AND Destino = @Destino
       // END
       //";

       //         using (SqlCommand cmd = new SqlCommand(sql, conn))
       //         {
       //             cmd.Parameters.Add("@UF_Origem", SqlDbType.Char, 2)
       //                 .Value = RemoverAcentos(ddlUfOrigem.SelectedItem.Text.ToUpper());

       //             cmd.Parameters.Add("@Origem", SqlDbType.VarChar, 100)
       //                 .Value = RemoverAcentos(ddlCidadeOrigem.SelectedItem.Text.ToUpper());

       //             cmd.Parameters.Add("@UF_Destino", SqlDbType.Char, 2)
       //                 .Value = RemoverAcentos(ddlUfDestino.SelectedItem.Text.ToUpper());

       //             cmd.Parameters.Add("@Destino", SqlDbType.VarChar, 100)
       //                 .Value = RemoverAcentos(ddlCidadeDestino.SelectedItem.Text.ToUpper());

       //             cmd.Parameters.Add("@Distancia", SqlDbType.Decimal)
       //                 .Value = distancia.HasValue
       //                     ? (object)distancia.Value
       //                     : DBNull.Value;

       //             cmd.Parameters.Add("@tempo_min", SqlDbType.Int)
       //                 .Value = tempo.HasValue
       //                     ? (object)tempo.Value
       //                     : DBNull.Value;

       //             conn.Open();

       //             try
       //             {
       //                 cmd.ExecuteNonQuery();

       //                 MostrarMsg(rota + " - Rota cadastrada com sucesso!");
       //                 btnCadastrarRota.Enabled = false;
       //             }
       //             catch (Exception)
       //             {
       //                 MostrarMsg("Erro ao salvar os dados.", "danger");
       //             }
       //         }
       //     }
       // }
        public static string RemoverAcentos(string texto)
        {
            if (string.IsNullOrEmpty(texto))
                return texto;

            var normalized = texto.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();

            foreach (char c in normalized)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }

            return sb.ToString().Normalize(NormalizationForm.FormC);
        }

    }


}