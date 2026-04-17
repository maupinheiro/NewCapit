using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using DocumentFormat.OpenXml.Math;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Extensions.Primitives;
using iText.Html2pdf;
using System.IO;

namespace NewCapit.dist.pages
{
    public partial class ImprimirOrdem : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UsuarioLogado"] != null)
                {
                    string nomeUsuario = Session["UsuarioLogado"].ToString();
                    var lblUsuario = nomeUsuario;                   
                }
                else
                {
                    var lblUsuario = "<Usuário>";                    
                }

                string id = Request.QueryString["id"];
                if (id != null)
                {
                    DataTable dt = BuscarDadosOrdem(id);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        DataRow r = dt.Rows[0];
                        string token = r["token_acesso"].ToString();

                        // Se o token estiver vazio, gera um de 25 caracteres
                        if (string.IsNullOrEmpty(token))
                        {
                            token = GerarToken(r["cpf"].ToString(), r["plavei"].ToString());
                            SalvarTokenNoBanco(id, token);
                        }

                        // Gera o link para o QRCode
                        string baseUrl = Request.Url.GetLeftPart(UriPartial.Authority);
                        string linkAprovacao = $"{baseUrl}/dist/pages/UploadNF.aspx?id={id}&tk={token}";

                        // Renderiza as vias
                        string via1 = GerarHtmlVia(r, "1ª VIA - EMISSOR", linkAprovacao);
                        string via2 = GerarHtmlVia(r, "2ª VIA - FORNECEDOR", linkAprovacao);

                        litConteudo.Text = via1 + "<div class='linha-corte'></div>" + via2;
                        RegistrarImpressao(id);
                    }
                }
                
            }
        }
        private string GerarToken(string cpf, string placa)
        {
            // Combina CPF + Placa + Guid para garantir 25 caracteres únicos
            string raw = (cpf + placa + Guid.NewGuid().ToString("N")).Replace("-", "");
            return raw.Length > 25 ? raw.Substring(0, 25) : raw.PadRight(25, 'X');
        }
        private void SalvarTokenNoBanco(string id, string token)
        {
            string connStr = WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "UPDATE tbsaida_combustivel SET token_acesso = @tk WHERE ordem_abastecimento = @id";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@tk", token);
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }       
        private string GerarHtmlVia(DataRow r, string identificacaoVia, string linkQr)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<html><head>");
            sb.Append("<meta charset='UTF-8'>");

            // ===== CSS =====
            sb.Append("<style>");

            sb.Append("* { margin:0; padding:0; box-sizing:border-box; }");

            sb.Append("body { font-family: Arial; font-size:11px; }");

            /* Página A4 */
            sb.Append(".page { width: 750px; margin: 0 auto; }");

            /* Cada via */
            sb.Append(".via { border:1px solid #000; padding:5px; margin-bottom:10px; page-break-inside: avoid; }");

            sb.Append("table { width:100%; border-collapse: collapse; }");
            sb.Append("td, th { padding:3px; font-size:11px; }");
            sb.Append("tr { line-height:1.2; }");

            sb.Append(".center { text-align:center; }");
            sb.Append(".right { text-align:right; }");

            sb.Append(".title { font-size:14px; font-weight:bold; }");
            sb.Append(".big { font-size:12px; font-weight:bold; }");

            sb.Append(".border { border:1px solid #000; }");

            sb.Append(".qrcode img { width:70px; }");

            sb.Append(".assinatura-box { border:1px solid #000; padding:5px; margin-top:5px; }");
            sb.Append(".linha-assinatura { border-top:1px dashed #000; margin-top:10px; padding-top:2px; font-size:10px; text-align:center; }");

            sb.Append("@media print {");
            sb.Append("  .page { page-break-after: avoid; }");
            sb.Append("}");

            sb.Append("</style>");
            sb.Append("</head><body>");

            sb.Append("<div class='page'>");
            
            sb.Append("<div class='via'>");

            // ===== CABEÇALHO COM MOLDURA =====
            sb.Append("<div class='box'>");
            sb.Append("<table>");
            sb.Append("<tr>");

            sb.Append("<td class='center' style='width:25%; border:1px solid #000;' >");
            sb.Append("<img src='" + ResolveUrl("~/img/logo_transnovag.png") + "' style='width:100px;'>");
            sb.Append("</td>");

            sb.Append("<td class='center' style='border:1px solid #000;'>");
            sb.Append("<div class='title'>TRANSNOVAG TRANSPORTES S/A.</div>");
            sb.Append("<div class='normal'>RUA CADIRIRI, 851 - MOOCA - SP</div>");
            sb.Append("<div class='normal'>CNPJ: 55.890.016/0001-09</div>");
            sb.Append("<div class='normal'>FONE: (11) 2126-3555</div>");
           // sb.Append("<div class='big'>Nº: " + r["ordem_abastecimento"] + "</div>");
            sb.Append("</td>");

            string qrUrl = $"https://api.qrserver.com/v1/create-qr-code/?size=250x250&data={HttpUtility.UrlEncode(linkQr)}";

            sb.Append("<td class='center qrcode' style='width:25%; border:1px solid #000;'>");
            sb.Append("<img src='" + qrUrl + "'>");
            sb.Append("</td>");

            sb.Append("</tr>");
            sb.Append("</table>");
            sb.Append("</div>");

            // ===== TABELA PRINCIPAL =====
            sb.Append("<table class='border'>");

            sb.Append("<tr class='center bold'>");
            sb.Append("<td style='border:1px solid #000;' >Nº Ordem</td>");
            sb.Append("<td style='border:1px solid #000;'>Veículo</td>");
            sb.Append("<td style='border:1px solid #000;'>Qt. Lts.</td>");
            sb.Append("<td style='border:1px solid #000;'>Valor Total</td>");
            sb.Append("</tr>");

            decimal litros = 0;
            decimal.TryParse(r["litros"].ToString(), out litros);

            decimal valorTotal = 0;
            decimal.TryParse(r["valor_total"].ToString(), out valorTotal);

            sb.Append("<tr class='center' style='font-size:20px; font-weight:bold;'>");

            sb.Append("<td style='font-size:20px; border:1px solid #000;'>" + r["ordem_abastecimento"] + "</td>");
            sb.Append("<td style='font-size:20px; border:1px solid #000;'>" + r["plavei"] + "</td>");
            sb.Append("<td style='font-size:20px; border:1px solid #000;'>" + (litros == 0 ? "" : litros.ToString()) + "</td>");
            sb.Append("<td style='font-size:20px; border:1px solid #000;'>" + (valorTotal == 0 ? "" : valorTotal.ToString("C2")) + "</td>");

            sb.Append("</tr>");

            sb.Append("</table>");

            // ===== DETALHES =====
            sb.Append("<table class='border normal'>");

            sb.Append("<tr><td><b>Tipo:</b> " + r["tipo_abastecimento"] + "</td></tr>");
            sb.Append("<tr><td><b>Fornecedor..:</b> " + r["cod_posto"] + " - " + r["nome_posto"] + "</td></tr>");
            sb.Append("<tr><td><b>Endereço.....:</b> " + r["endereco"] + ", " + r["numero"] + ", " + r["complemento"] + " - " + r["cidade"] + "/" + r["estado"] + "</td></tr>");
            sb.Append("<tr><td><b>Produto.......:</b> " + r["cod_combustivel"] + " - " + r["combustivel"] + "</td></tr>");
            sb.Append("<tr><td><b>Preço Unit...:</b> " + string.Format("{0:N2}", r["valor_unitario"]) + " | <b>Doc:</b> " + r["tipo_documento"] + " Nº " + r["numero_documento"] + "</td></tr>");
            sb.Append("<tr><td><b>Veículo........:</b> " + r["plavei"] + " - " + r["descricao_veiculo"] + "</td></tr>");
            sb.Append("<tr><td><b>Proprietário.:</b> " + r["codtra"] + " - " + r["nomtra"] + " - CPF/CNPJ: " + r["cnpj_cpf"] + "</td></tr>");
            sb.Append("<tr><td><b>Motorista.....:</b> " + r["codmot"] + " - " + r["nommot"] + " - CPF: " + r["cpf"] + "</td></tr>");
            sb.Append("<tr><td><b>Informe KM Atual:</b> _____________________</td></tr>");

            sb.Append("<tr>");
            sb.Append("<td class='small center bold'>");
            sb.Append("ANTES DE INICIAR O ABASTECIMENTO VALIDE A ORDEM ATRAVÉS DO QRCODE.<br/>");
            sb.Append("FIQUE ATENTO, NÃO SERÁ PAGO, PRODUTO FORA DA AUTORIZAÇÃO, NEM VALORES SUPERIORES AOS ESTIPULADOS NA ORDEM.<br/>");
            sb.Append("NÃO SE ESQUEÇA DE LANÇAR A NOTA FISCAL, ATRAVÉS DO QRCODE.");
            sb.Append("</td>");
            sb.Append("</tr>");

            sb.Append("</table>");

            // ===== ASSINATURAS COM MOLDURA =====
            string usuario = Session["UsuarioLogado"]?.ToString() ?? "DESCONHECIDO";
            string data = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

            sb.Append("<div class='assinatura-box'>");

            sb.Append("<table>");
            sb.Append("<tr>");

            sb.Append("<td style='width:33%;'>");
            sb.Append("<br/>");
            sb.Append("<br/>");
            sb.Append("<div class='linha-assinatura'>Autorizado<br>" + usuario + "<br>" + data + "</div>");
            sb.Append("</td>");

            sb.Append("<td style='width:33%;'>");
            sb.Append("<br/>");
            sb.Append("<br/>");
            sb.Append("<div class='linha-assinatura'>Motorista<br>" + r["nommot"] + "</div>");
            sb.Append("</td>");

            sb.Append("<td style='width:33%;'>");
            sb.Append("<br/>");
            sb.Append("<br/>");
            sb.Append("<div class='linha-assinatura'>Resp. Posto</div>");
            sb.Append("</td>");

            sb.Append("</tr>");
            sb.Append("</table>");

            sb.Append("</div>");

            // ===== RODAPÉ =====
            sb.Append("<table class='small'>");
            sb.Append("<tr>");
            sb.Append("<td>Gerada em: " + r["lancado_por"] + "</td>");

            sb.Append("<td class='right'>");
            if (Convert.ToInt32(r["num_impressao"]) > 0)
                sb.Append("<b>REIMPRESSÃO (" + r["num_impressao"] + ")</b>");
            sb.Append("</td>");

            sb.Append("</tr>");
            sb.Append("</table>");
            sb.Append("</div>");
           
            sb.Append("</div></body></html>");

            return sb.ToString();
        }        
        private DataTable BuscarDadosOrdem(string ordem)
        {
            DataTable dt = new DataTable();

            // Obtém a string de conexão do Web.config
            string strConexao = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
            string sql = @"
                            SELECT
                                sc.[ordem_abastecimento],
                                sc.[data_geracao],
                                sc.[cod_posto],
                                sc.[nome_posto],
                                sc.[tipo_abastecimento],
                                sc.[frota_agregado],
                                sc.[filial],
                                sc.[cod_combustivel],
                                sc.[combustivel],
                                sc.[litros],
                                sc.[valor_unitario],
                                sc.[valor_total],
                                sc.[numero_documento],
                                sc.[tipo_documento],
                                sc.[data_emissao],
                                sc.[codmot],
                                sc.[nommot],
                                sc.[cpf],
                                sc.[codvei],
                                sc.[plavei],
                                sc.[descricao_veiculo],
                                sc.[codtra],
                                sc.[nomtra],
                                sc.[cnpj_cpf],
                                sc.[lancado_por],
                                sc.[num_impressao],
                                sc.[impressa],
                                sc.[token_acesso],
                                f.[endereco],
                                f.[numero],
                                f.[complemento],
                                f.[cidade],
                                f.[estado]
                            FROM dbo.tbsaida_combustivel sc
                            LEFT JOIN dbo.tbfornecedores f
                                ON f.codfor = sc.cod_posto
                            WHERE sc.ordem_abastecimento = @ordem";

            using (SqlConnection conn = new SqlConnection(strConexao))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    // Passagem segura do parâmetro
                    cmd.Parameters.AddWithValue("@ordem", ordem);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        try
                        {
                            conn.Open();
                            da.Fill(dt);
                        }
                        catch (Exception ex)
                        {
                            // Logar o erro se necessário: ex.Message
                            throw new Exception("Erro ao buscar dados da ordem: " + ex.Message);
                        }
                    }
                }
            }

            return dt;
        }
        private void RegistrarImpressao(string ordem)
        {
            // Obtém a string de conexão do Web.config
            string strConexao = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString;

            // SQL que incrementa o contador de impressões e atualiza a data/status            
            string sql = @"
            UPDATE tbsaida_combustivel
            SET 
                num_impressao = ISNULL(num_impressao, 0) + 1,
                impressa = CASE 
                              WHEN ISNULL(num_impressao, 0) + 1 <= 1 THEN 'IMPRESSA'
                              ELSE 'REIMPRESSA'
                           END,
                data_impressao = GETDATE()
            WHERE ordem_abastecimento = @ordem";

            using (SqlConnection conn = new SqlConnection(strConexao))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ordem", ordem);

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // Em sistemas de impressão, se o registro falhar, 
                        // geralmente apenas logamos o erro para não travar a visualização do usuário
                        System.Diagnostics.Debug.WriteLine("Erro ao registrar impressão: " + ex.Message);
                    }
                }
            }
        }
        //private void GerarPdf(string html)
        //{
        //    Response.Clear();
        //    Response.ContentType = "application/pdf";
        //    Response.AddHeader("content-disposition", "inline;filename=OrdemAbastecimento.pdf");
        //    Response.Cache.SetCacheability(HttpCacheability.NoCache);

        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        ConverterProperties props = new ConverterProperties();

        //        HtmlConverter.ConvertToPdf(html, ms, props);

        //        byte[] bytes = ms.ToArray();
        //        Response.OutputStream.Write(bytes, 0, bytes.Length);
        //    }

        //    Response.End();
        //}

    }
}