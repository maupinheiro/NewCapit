using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Net.Mail;
using System.Text;
using System.Drawing;
using System.Web.UI.HtmlControls;
using System.Globalization;
using System.Configuration;
using DAL;
using Domain;
using Microsoft.AspNet.SignalR.Hosting;
using PuppeteerSharp;
using PuppeteerSharp.Media;



namespace NewCapit.dist.pages
{
    public partial class Frm_AvaliaDesempenho : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conexao"].ToString());

        public string fotoMotorista;
        string id;
        private string nomeUsuario;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.IsPostBack)
                return;
            System.Web.UI.ScriptManager.GetCurrent(this.Page).RegisterPostBackControl((Control)this.lnkBaixar);
            if (this.Session["UsuarioLogado"] != null)
            {
                this.nomeUsuario = this.Session["UsuarioLogado"].ToString();
                string nomeUsuario = this.nomeUsuario;
            }
            this.CarregaMotorista();
            this.CarregarAvaliacao();
            this.CarregaFoto();
        }

        public void CarregaFoto()
        {
            if (!string.IsNullOrEmpty(this.Request.QueryString["id"]))
                this.id = this.Request.QueryString["id"];
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("select cracha, nome, funcao,admissao,nucleo,mes,frota from tbavaliacaomotorista where ISNUMERIC(cracha)=1 and Id =" + this.id, this.con);
            DataTable dataTable = new DataTable();
            this.con.Open();
            sqlDataAdapter.Fill(dataTable);
            this.con.Close();
            string str1 = dataTable.Rows[0][0].ToString();
            ConsultaMotorista consultaMotorista = UsersDAL.CheckMotorista(new ConsultaMotorista()
            {
                codmot = str1
            });
            string str2 = "../../fotos/motoristasemfoto.jpg";
            if (consultaMotorista != null)
            {
                if (!string.Equals(consultaMotorista.status?.Trim(), "INATIVO", StringComparison.OrdinalIgnoreCase))
                {
                    if (!string.IsNullOrWhiteSpace(consultaMotorista.caminhofoto))
                    {
                        string path = "../.." + consultaMotorista.caminhofoto.Trim();
                        str2 = File.Exists(this.Server.MapPath(path)) ? path : "../../fotos/motoristasemfoto.jpg";
                    }
                }
                else
                    str2 = "../../fotos/inativo.jpg";
            }
            this.imgMotorista.ImageUrl = str2;
        }

        public void CarregaMotorista()
        {
            if (HttpContext.Current.Request.QueryString["id"].ToString() != "")
                this.id = HttpContext.Current.Request.QueryString["id"].ToString();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("select cracha, nome, funcao,admissao,nucleo,mes,frota, nm_usuario from tbavaliacaomotorista where ISNUMERIC(cracha)=1 and id=" + this.id, this.con);
            DataTable dataTable = new DataTable();
            this.con.Open();
            sqlDataAdapter.Fill(dataTable);
            this.con.Close();
            if (dataTable.Rows.Count <= 0)
                return;
            this.lblCracha.Text = dataTable.Rows[0][0].ToString();
            this.lblNomeMot.Text = dataTable.Rows[0][1].ToString();
            this.lblFuncao.Text = dataTable.Rows[0][2].ToString();
            DateTime result;
            this.lblDtAdmissao.Text = DateTime.TryParse(Convert.ToString(dataTable.Rows[0][3]), out result) ? result.ToString("dd/MM/yyyy") : "";
            this.lblNucleo.Text = dataTable.Rows[0][4].ToString();
            this.lblMes.Text = dataTable.Rows[0][5].ToString();
            this.lblFrota.Text = dataTable.Rows[0][6].ToString();
            this.lblUsuario.Text = dataTable.Rows[0][7].ToString();
            this.lblNomeAss.Text = dataTable.Rows[0][1].ToString();
        }

        protected void Recalcular(object sender, EventArgs e) => this.CalcularTotal();

        private void CalcularTotal()
        {
            this.lblResultadoTotal.Text = (0M + this.CalcularItem(this.rb_documentacao_1, this.rb_documentacao_2, this.rb_documentacao_3, this.ParsePeso(this.lblPesoDocumentacao.Text)) + this.CalcularItem(this.rb_pontualidade_1, this.rb_pontualidade_2, this.rb_pontualidade_3, this.ParsePeso(this.lblPesoPontualidade.Text)) + this.CalcularItem(this.rb_seguranca_1, this.rb_seguranca_2, this.rb_seguranca_3, this.ParsePeso(this.lblPesoSegurancaCarga.Text)) + this.CalcularItem(this.rb_cargadescarga_1, this.rb_cargadescarga_2, this.rb_cargadescarga_3, this.ParsePeso(this.lblPesoCargaDescarga.Text)) + this.CalcularItem(this.rb_comunicacao_1, this.rb_comunicacao_2, this.rb_comunicacao_3, this.ParsePeso(this.lblPesoComunicacao.Text)) + this.CalcularItem(this.rb_transito_1, this.rb_transito_2, this.rb_transito_3, this.ParsePeso(this.lblPesoSegurancaTransito.Text)) + this.CalcularItem(this.rb_combustivel_1, this.rb_combustivel_2, this.rb_combustivel_3, this.ParsePeso(this.lblPesoConsumoCombustivel.Text)) + this.CalcularItem(this.rb_conservacao_1, this.rb_conservacao_2, this.rb_conservacao_3, this.ParsePeso(this.lblPesoConservacaoVeiculo.Text))).ToString("0.##") + "%";
        }

        private Decimal ParsePeso(string textoComPercent)
        {
            if (string.IsNullOrWhiteSpace(textoComPercent))
                return 0M;
            textoComPercent = textoComPercent.Replace("%", "").Trim();
            Decimal result;
            return Decimal.TryParse(textoComPercent, out result) ? result : 0M;
        }

        private Decimal CalcularItem(RadioButton rb1, RadioButton rb2, RadioButton rb3, Decimal peso)
        {
            if (rb1.Checked)
                return 0M;
            if (rb2.Checked)
                return peso * 0.5M;
            return rb3.Checked ? peso : 0M;
        }

        protected void lnkSalvar_Click(object sender, EventArgs e)
        {
            if (HttpContext.Current.Request.QueryString["id"].ToString() != "")
                this.id = HttpContext.Current.Request.QueryString["id"].ToString();
            if (!this.GrupoSelecionado(this.rb_documentacao_1, this.rb_documentacao_2, this.rb_documentacao_3) || !this.GrupoSelecionado(this.rb_pontualidade_1, this.rb_pontualidade_2, this.rb_pontualidade_3) || !this.GrupoSelecionado(this.rb_seguranca_1, this.rb_seguranca_2, this.rb_seguranca_3) || !this.GrupoSelecionado(this.rb_cargadescarga_1, this.rb_cargadescarga_2, this.rb_cargadescarga_3) || !this.GrupoSelecionado(this.rb_comunicacao_1, this.rb_comunicacao_2, this.rb_comunicacao_3) || !this.GrupoSelecionado(this.rb_transito_1, this.rb_transito_2, this.rb_transito_3) || !this.GrupoSelecionado(this.rb_combustivel_1, this.rb_combustivel_2, this.rb_combustivel_3) || !this.GrupoSelecionado(this.rb_conservacao_1, this.rb_conservacao_2, this.rb_conservacao_3))
            {
                string script = $"alert('Existem itens sem avaliação. Por favor, selecione 1, 2 ou 3 em todas as linhas.');";
                this.ClientScript.RegisterClientScriptBlock(this.GetType(), "validacao", script, true);
            }
            else
            {
                string radioValue1 = this.GetRadioValue(this.rb_documentacao_1, this.rb_documentacao_2, this.rb_documentacao_3);
                string radioValue2 = this.GetRadioValue(this.rb_pontualidade_1, this.rb_pontualidade_2, this.rb_pontualidade_3);
                string radioValue3 = this.GetRadioValue(this.rb_seguranca_1, this.rb_seguranca_2, this.rb_seguranca_3);
                string radioValue4 = this.GetRadioValue(this.rb_cargadescarga_1, this.rb_cargadescarga_2, this.rb_cargadescarga_3);
                string radioValue5 = this.GetRadioValue(this.rb_comunicacao_1, this.rb_comunicacao_2, this.rb_comunicacao_3);
                string radioValue6 = this.GetRadioValue(this.rb_transito_1, this.rb_transito_2, this.rb_transito_3);
                string radioValue7 = this.GetRadioValue(this.rb_combustivel_1, this.rb_combustivel_2, this.rb_combustivel_3);
                string radioValue8 = this.GetRadioValue(this.rb_conservacao_1, this.rb_conservacao_2, this.rb_conservacao_3);
                if (this.Session["UsuarioLogado"] != null)
                {
                    this.nomeUsuario = this.Session["UsuarioLogado"].ToString();
                    string nomeUsuario = this.nomeUsuario;
                }
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand("UPDATE tbavaliacaomotorista SET documentos = @documentos, pontualidade   = @pontualidade, segcarga= @segcarga, cargaedescarga = @cargaedescarga,  comunicacao= @comunicacao, segtransito = @segtransito, consumocomb = @consumocomb, conservacao  = @conservacao,  observacao = @observacao, avaliado = @avaliado,vl_total = @vl_total, dt_avaliacao = @dt_avaliacao, nm_usuario= @nm_usuario WHERE id = @id", connection);
                    sqlCommand.Parameters.AddWithValue("@documentos", (object)radioValue1);
                    sqlCommand.Parameters.AddWithValue("@pontualidade", (object)radioValue2);
                    sqlCommand.Parameters.AddWithValue("@segcarga", (object)radioValue3);
                    sqlCommand.Parameters.AddWithValue("@cargaedescarga", (object)radioValue4);
                    sqlCommand.Parameters.AddWithValue("@comunicacao", (object)radioValue5);
                    sqlCommand.Parameters.AddWithValue("@segtransito", (object)radioValue6);
                    sqlCommand.Parameters.AddWithValue("@consumocomb", (object)radioValue7);
                    sqlCommand.Parameters.AddWithValue("@conservacao", (object)radioValue8);
                    sqlCommand.Parameters.AddWithValue("@observacao", (object)this.txtObs.Text);
                    sqlCommand.Parameters.AddWithValue("@avaliado", (object)"S");
                    sqlCommand.Parameters.AddWithValue("@vl_total", (object)this.lblResultadoTotal.Text.Replace("%", ""));
                    sqlCommand.Parameters.AddWithValue("@dt_avaliacao", (object)DateTime.Now);
                    sqlCommand.Parameters.AddWithValue("@nm_usuario", (object)this.nomeUsuario);
                    sqlCommand.Parameters.AddWithValue("@id", (object)this.id);
                    try
                    {
                        connection.Open();
                        sqlCommand.ExecuteNonQuery();
                        connection.Close();
                        string script = $"alert('{HttpUtility.JavaScriptStringEncode("Avaliação salva com sucesso!" ?? "")}');";
                        this.ClientScript.RegisterClientScriptBlock(this.GetType(), "MensagemDeAlerta", script, true);
                    }
                    catch (Exception ex)
                    {
                        string script = $"alert('{HttpUtility.JavaScriptStringEncode("Erro: " + ex.ToString() ?? "")}');";
                        this.ClientScript.RegisterClientScriptBlock(this.GetType(), "MensagemDeAlerta", script, true);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        private string GetRadioValue(RadioButton rb1, RadioButton rb2, RadioButton rb3)
        {
            if (rb1.Checked)
                return "1";
            if (rb2.Checked)
                return "2";
            return rb3.Checked ? "3" : "0";
        }

        private bool GrupoSelecionado(RadioButton r1, RadioButton r2, RadioButton r3)
        {
            return r1.Checked || r2.Checked || r3.Checked;
        }

        private void CarregarAvaliacao()
        {
            if (HttpContext.Current.Request.QueryString["id"].ToString() != "")
                this.id = HttpContext.Current.Request.QueryString["id"].ToString();
            string cmdText = "SELECT * FROM tbavaliacaomotorista WHERE Id = @Id";
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(cmdText, connection))
                {
                    sqlCommand.Parameters.AddWithValue("@Id", (object)this.id);
                    connection.Open();
                    using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                    {
                        if (!sqlDataReader.Read())
                            return;
                        this.txtObs.Text = sqlDataReader["observacao"].ToString();
                        this.lblResultadoTotal.Text = sqlDataReader["vl_total"].ToString() + "%";
                        this.SelecionarRadio(sqlDataReader["documentos"].ToString(), this.rb_documentacao_1, this.rb_documentacao_2, this.rb_documentacao_3);
                        this.SelecionarRadio(sqlDataReader["pontualidade"].ToString(), this.rb_pontualidade_1, this.rb_pontualidade_2, this.rb_pontualidade_3);
                        this.SelecionarRadio(sqlDataReader["segcarga"].ToString(), this.rb_seguranca_1, this.rb_seguranca_2, this.rb_seguranca_3);
                        this.SelecionarRadio(sqlDataReader["cargaedescarga"].ToString(), this.rb_cargadescarga_1, this.rb_cargadescarga_2, this.rb_cargadescarga_3);
                        this.SelecionarRadio(sqlDataReader["comunicacao"].ToString(), this.rb_comunicacao_1, this.rb_comunicacao_2, this.rb_comunicacao_3);
                        this.SelecionarRadio(sqlDataReader["segtransito"].ToString(), this.rb_transito_1, this.rb_transito_2, this.rb_transito_3);
                        this.SelecionarRadio(sqlDataReader["consumocomb"].ToString(), this.rb_combustivel_1, this.rb_combustivel_2, this.rb_combustivel_3);
                        this.SelecionarRadio(sqlDataReader["conservacao"].ToString(), this.rb_conservacao_1, this.rb_conservacao_2, this.rb_conservacao_3);
                        this.Recalcular((object)null, (EventArgs)null);
                    }
                }
            }
        }

        private void SelecionarRadio(string valor, RadioButton rb1, RadioButton rb2, RadioButton rb3)
        {
            rb1.Checked = valor == "1";
            rb2.Checked = valor == "2";
            rb3.Checked = valor == "3";
        }

        protected async void lnkBaixar_Click(object sender, EventArgs e)
        {
            // Nome do arquivo PDF
            string nomeArquivo = $"Avaliacao_{lblCracha.Text}_{lblMes.Text}.pdf";
            string caminhoCompleto = Server.MapPath("../../pdfs/" + nomeArquivo);

            // Garante a pasta
            Directory.CreateDirectory(Path.GetDirectoryName(caminhoCompleto));

            IBrowser browser = null;

            try
            {
                // Baixa o Chromium da versão suportada pelo seu PuppeteerSharp
                var fetcher = new BrowserFetcher();
                await fetcher.DownloadAsync();

                // Abre o navegador
                browser = await Puppeteer.LaunchAsync(new LaunchOptions
                {
                    Headless = true
                });

                // Nova página
                var page = await browser.NewPageAsync();

                // Acessa esta própria URL
                await page.GoToAsync(Request.Url.AbsoluteUri);

                // Gera o PDF
                await page.PdfAsync(caminhoCompleto, new PdfOptions
                {
                    Format = PaperFormat.A4,
                    PrintBackground = true
                });

                await page.CloseAsync();

                // Envia o PDF ao cliente
                EnviarPdfParaCliente(caminhoCompleto, nomeArquivo);
            }
            catch (Exception ex)
            {
                // Aqui você pode logar ou exibir a mensagem
                System.Diagnostics.Debug.WriteLine("ERRO PDF: " + ex);
            }
            finally
            {
                if (browser != null)
                    await browser.CloseAsync();
            }
        }


        private void EnviarPdfParaCliente(string caminhoArquivo, string nomeArquivo)
        {
            HttpResponse response = HttpContext.Current.Response;
            response.Clear();
            response.ContentType = "application/pdf";
            response.AddHeader("Content-Disposition", "attachment; filename=" + nomeArquivo);
            response.WriteFile(caminhoArquivo);
            response.End();
        }                     
    }
}