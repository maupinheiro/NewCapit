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

namespace NewCapit.dist.pages
{
    public partial class Frm_AvaliaDesempenho : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conexao"].ToString());

        public string fotoMotorista;
        string id;
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                
                CarregaMotorista();
            }
            CarregaFoto();
        }

        public void CarregaFoto()
        {

            if (HttpContext.Current.Request.QueryString["id"].ToString() != "")
            {
                id = HttpContext.Current.Request.QueryString["id"].ToString();
            }

            var codigo = id;

            var obj = new Domain.ConsultaMotorista
            {
                codmot = codigo
            };

            var ConsultaMotorista = DAL.UsersDAL.CheckMotorista(obj);

            if (ConsultaMotorista != null)
            {
                if (!string.Equals(ConsultaMotorista.status?.Trim(), "INATIVO", StringComparison.OrdinalIgnoreCase))
                {
                    // Se o campo caminhofoto for nulo ou vazio, usa a foto padrão
                    if (!string.IsNullOrWhiteSpace(ConsultaMotorista.caminhofoto))
                    {
                        fotoMotorista = "../.." + ConsultaMotorista.caminhofoto.Trim();

                        string caminhoFisico = Server.MapPath(fotoMotorista);

                        if (!File.Exists(caminhoFisico))
                        {
                            fotoMotorista = "../../fotos/usuario.jpg";
                        }
                    }
                    else
                    {
                        fotoMotorista = "../../fotos/usuario.jpg";
                    }
                }
                else
                {
                    fotoMotorista = "../../fotos/inativo.jpg";
                }
            }
            else
            {
                fotoMotorista = "../../fotos/usuario.jpg";
            }
        }

        public void CarregaMotorista()
        {
            if (HttpContext.Current.Request.QueryString["id"].ToString() != "")
            {
                id = HttpContext.Current.Request.QueryString["id"].ToString();
            }
            string sql = "select cracha, nome, funcao,admissao,nucleo,mes,frota from tbavaliacaomotorista where ISNUMERIC(cracha)=1 and cracha=" + id;
            SqlDataAdapter adp = new SqlDataAdapter(sql, con);
            DataTable dt = new DataTable();
            con.Open();
            adp.Fill(dt);
            con.Close();
            if (dt.Rows.Count > 0)
            {
                lblCracha.Text = dt.Rows[0][0].ToString();
                lblNomeMot.Text = dt.Rows[0][1].ToString();
                lblFuncao.Text = dt.Rows[0][2].ToString();
                lblDtAdmissao.Text = dt.Rows[0][3].ToString();
                lblNucleo.Text = dt.Rows[0][4].ToString();
                lblMes.Text = lblNucleo.Text = dt.Rows[0][5].ToString();
                lblFrota.Text = lblNucleo.Text = dt.Rows[0][6].ToString();


            }
            else
            {
                
            }
        }

        protected void Recalcular(object sender, EventArgs e)
        {
            CalcularTotal();
        }
        private void CalcularTotal()
        {
            decimal total = 0;

            total += CalcularItem(rb_documentacao_1, rb_documentacao_2, rb_documentacao_3, ParsePeso(lblPesoDocumentacao.Text));
            total += CalcularItem(rb_pontualidade_1, rb_pontualidade_2, rb_pontualidade_3, ParsePeso(lblPesoPontualidade.Text));
            total += CalcularItem(rb_seguranca_1, rb_seguranca_2, rb_seguranca_3, ParsePeso(lblPesoSegurancaCarga.Text));
            total += CalcularItem(rb_cargadescarga_1, rb_cargadescarga_2, rb_cargadescarga_3, ParsePeso(lblPesoCargaDescarga.Text));
            total += CalcularItem(rb_comunicacao_1, rb_comunicacao_2, rb_comunicacao_3, ParsePeso(lblPesoComunicacao.Text));
            total += CalcularItem(rb_transito_1, rb_transito_2, rb_transito_3, ParsePeso(lblPesoSegurancaTransito.Text));
            total += CalcularItem(rb_combustivel_1, rb_combustivel_2, rb_combustivel_3, ParsePeso(lblPesoConsumoCombustivel.Text));
            total += CalcularItem(rb_conservacao_1, rb_conservacao_2, rb_conservacao_3, ParsePeso(lblPesoConservacaoVeiculo.Text));
            // repita para os outros itens...

            lblResultadoTotal.Text = total.ToString("0.##") + "%";
        }
        private decimal ParsePeso(string textoComPercent)
        {
            if (string.IsNullOrWhiteSpace(textoComPercent)) return 0;
            textoComPercent = textoComPercent.Replace("%", "").Trim();
            if (decimal.TryParse(textoComPercent, out decimal v))
                return v;
            return 0;
        }

        private decimal CalcularItem(RadioButton rb1, RadioButton rb2, RadioButton rb3, decimal peso)
        {
            if (rb1.Checked) return 0m;
            if (rb2.Checked) return peso * 0.5m;
            if (rb3.Checked) return peso;
            return 0m;
        }

        protected void lnkSalvar_Click(object sender, EventArgs e)
        {
            string v_documentacao = GetRadioValue(rb_documentacao_1, rb_documentacao_2, rb_documentacao_3);
            string v_pontualidade = GetRadioValue(rb_pontualidade_1, rb_pontualidade_2, rb_pontualidade_3);
            string v_seguranca = GetRadioValue(rb_seguranca_1, rb_seguranca_2, rb_seguranca_3);
            string v_cargadescarga = GetRadioValue(rb_cargadescarga_1, rb_cargadescarga_2, rb_cargadescarga_3);
            string v_comunicacao = GetRadioValue(rb_comunicacao_1, rb_comunicacao_2, rb_comunicacao_3);
            string v_transito = GetRadioValue(rb_transito_1, rb_transito_2, rb_transito_3);
            string v_combustivel = GetRadioValue(rb_combustivel_1, rb_combustivel_2, rb_combustivel_3);
            string v_conservacao = GetRadioValue(rb_conservacao_1, rb_conservacao_2, rb_conservacao_3);

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                string sql = @"
                            UPDATE tbavaliacaomotorista
                            SET documentos     = @documentos,
                                pontualidade   = @pontualidade,
                                segcarga       = @segcarga,
                                cargaedescarga = @cargaedescarga,
                                comunicacao    = @comunicacao,
                                segtransito    = @segtransito,
                                consumocomb    = @consumocomb,
                                conservacao    = @conservacao,
                                observacao     = @observacao,
                                avaliado       = @avaliado,
                                vl_total       = @vl_total
                            WHERE cracha       = @cracha 
                            and   mes          = @mes";

                SqlCommand cmd = new SqlCommand(sql, con);

                // RADIOBUTTONS
                cmd.Parameters.AddWithValue("@documentos", v_documentacao);
                cmd.Parameters.AddWithValue("@pontualidade", v_pontualidade);
                cmd.Parameters.AddWithValue("@segcarga", v_seguranca);
                cmd.Parameters.AddWithValue("@cargaedescarga", v_cargadescarga);
                cmd.Parameters.AddWithValue("@comunicacao", v_comunicacao);
                cmd.Parameters.AddWithValue("@segtransito", v_transito);
                cmd.Parameters.AddWithValue("@consumocomb", v_combustivel);
                cmd.Parameters.AddWithValue("@conservacao", v_conservacao);

                // CAMPOS EXTRAS
                
                 cmd.Parameters.AddWithValue("@observacao", txtObs.Text);
                cmd.Parameters.AddWithValue("@avaliado", "S");
                cmd.Parameters.AddWithValue("@vl_total", lblResultadoTotal.Text.Replace("%",""));

                // WHERE
                cmd.Parameters.AddWithValue("@cracha", lblCracha.Text);
                cmd.Parameters.AddWithValue("@mes", lblMes.Text);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    string linha1 = "Avaliação salva com sucesso!";


                    // Concatenando as linhas com '\n' para criar a mensagem
                    string mensagem = $"{linha1}";

                    string mensagemCodificada = HttpUtility.JavaScriptStringEncode(mensagem);
                    // Gerando o script JavaScript para exibir o alerta
                    string script = $"alert('{mensagemCodificada}');";

                    //// Registrando o script para execução no lado do cliente
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "MensagemDeAlerta", script, true);
                }
                catch (Exception eg)
                {
                    string linha1 = "Erro: " + eg.ToString();


                    // Concatenando as linhas com '\n' para criar a mensagem
                    string mensagem = $"{linha1}";

                    string mensagemCodificada = HttpUtility.JavaScriptStringEncode(mensagem);
                    // Gerando o script JavaScript para exibir o alerta
                    string script = $"alert('{mensagemCodificada}');";

                    //// Registrando o script para execução no lado do cliente
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "MensagemDeAlerta", script, true);
                }
                finally
                {
                    con.Close();
                }
                
            }


        }
        private string GetRadioValue(RadioButton rb1, RadioButton rb2, RadioButton rb3)
        {
            if (rb1.Checked) return "1";
            if (rb2.Checked) return "2";
            if (rb3.Checked) return "3";
            return "0"; // caso nenhum esteja selecionado
        }
    }
}