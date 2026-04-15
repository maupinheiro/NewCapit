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
using Microsoft.Extensions.Primitives;

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
            string baseUrl = Request.Url.GetLeftPart(UriPartial.Authority);

            // O link agora leva o NR e o TOKEN para validação automática
            //string linkAprovacao = $"{baseUrl}/clientes/VerificaOrcamento.aspx?id={nr}&tk={tokenUsuario}";
            sb.Append("<div class='wrapper'>");

            // COLUNA DO LOGO
            sb.Append("<table class='header-table' style='width:100%;'>");
            sb.Append("<tr>");

            // COLUNA DO LOGO
            sb.Append("<td style='width:25%;'>");
            sb.Append("<img src='" + ResolveUrl("~/img/logo_transnovag.png") + "' style='max-width:120px;' />");
            sb.Append("</td>");

            // COLUNA DOS DADOS
            sb.Append("<td style='font-size:14px; text-align:center;'>");
            sb.Append("<span style='font-size:20px; font-weight:bold;'>TRANSNOVAG TRANSPORTES S/A.</span><br />");
            sb.Append("<span style='font-size:14px;'>RUA CADIRIRI, 851 - MOOCA - SP<br />");
            sb.Append("CNPJ: 55.890.016/0001-09<br />");
            sb.Append("FONE: (11) 2126-3555</span>");
            sb.Append("<br />");
            sb.Append("<span style='font-size:20px; font-weight:bold;'>Nº: "+ r["ordem_abastecimento"] + "</span><br />");
            sb.Append("</td>");

            //QRCODE
            sb.Append("<td style='width:25%;'>");
            // QR Code [cite: 78, 102]
            string qrUrl = $"https://api.qrserver.com/v1/create-qr-code/?size=150x150&data={HttpUtility.UrlEncode(linkQr)}";
            sb.Append("<div class='qrcode-container'>");
            sb.Append($"<div class='qrcode-box'><img src='{qrUrl}' /></div>");
            sb.Append("</div>");  
            sb.Append("</td>");
            sb.Append("</tr>");
            sb.Append("</table>");


            // tabela 
            sb.Append("<table border='1' style='width:100%; border-collapse:collapse; border:2px solid #000;'>");

            // CABEÇALHO (TÍTULO)
            sb.Append("<tr>");
            sb.Append("<th style='text-align:center; font-size:14px; padding:5px;'>Nº Ordem</th>");
            sb.Append("<th style='text-align:center; font-size:14px; padding:5px;'>Veiculo</th>");
            sb.Append("<th style='text-align:center; font-size:14px; padding:5px;'>Qt. Lts. Autorizados</th>");
            sb.Append("<th style='text-align:center; font-size:14px; padding:5px;'>Valor Total</th>");
            sb.Append("</tr>");

            // LINHA DE CONTEÚDO
            sb.Append("<tr>");
            sb.Append("<td style='text-align:center; font-size:20px; padding:8px;'><b>" + r["ordem_abastecimento"] + "</b></td>");
            sb.Append("<td style='text-align:center; font-size:20px; padding:8px;'><b>" + r["plavei"] + "</b></td>");            
            decimal litros = 0;
            decimal.TryParse(r["litros"].ToString(), out litros);
            if (litros == 0)
            {
                sb.Append("<td style='text-align:center; font-size:20px; padding:8px;'><b></b></td>");
            }
            else
            {
                sb.Append("<td style='text-align:center; font-size:20px; padding:8px;'><b>" + litros + "</b></td>");
            }

            decimal valorTotal = 0;
            decimal.TryParse(r["valor_total"].ToString(), out valorTotal);
            if (valorTotal == 0)
            {
                sb.Append("<td style='text-align:center; font-size:20px; padding:8px;'><b></b></td>");
            }
            else
            {
                sb.Append("<td style='text-align:center; font-size:20px; padding:8px;'><b>" +
                          valorTotal.ToString("C2") +
                          "</b></td>");
            }

            sb.Append("</tr>");           

            sb.Append("</table>");
            sb.Append("<table class='header-table' style='width:100%;'>");
            sb.Append("<tr>");
            sb.Append("<br/>");
            sb.Append("<td style='width:100%;'><b>Tipo......:</b>" + r["tipo_abastecimento"] + "</td>");
            sb.Append("<tr><td style='width:100%;'><b>Fornecedor:</b> " + r["cod_posto"] + " - " + r["nome_posto"] + "</td></tr>");
            sb.Append("<tr><td style='width:100%;'><b>Endereço..:</b></td></tr>");
            sb.Append("<tr><td style='width:100%;'><b>Produto...:</b>" + r["cod_combustivel"] + " - " + r["combustivel"] + "            Informe KM Atual: ______________</td></tr>");            
            sb.Append("<tr><td style='width:100%;'><b>Preço Unit:</b>" + string.Format("{0:N2}", r["valor_unitario"]) + " - Tipo Doc.:" + r["tipo_documento"] + " - Nº.:" + r["numero_documento"] + "</td></tr>");
            sb.Append("<tr><td style='width:100%;'><b>Veiculo...:</b>" + r["plavei"] + " - " + r["descricao_veiculo"] + "</td></tr>");
            sb.Append("<tr><td style='width:100%;'><b>Prop......:</b>" + r["codtra"] + " - " + r["nomtra"] + " - CNPJ/CPF:" + r["cnpj_cpf"] + "</td></tr>");
            sb.Append("<tr><td style='width:100%;'><b>Motorista.:</b>" + r["codmot"] + " - " + r["nommot"] + " - CPF:" + r["cpf"] + "</td></tr>");
            
            sb.Append("<tr><td style='width:100%; text-align:center; font-size:10px;'><b>ATENÇÃO:</b></td></tr>");
            sb.Append("<tr><td style='width:100%; text-align:center; font-size:8px;'><b>VERIFIQUE A VALIDADE DA ORDEM DE ABASTECIMENTO, ANTES DE INICIAR O ABASTECIMENTO LEIA O QRCODE PARA VALIDAR E LANÇAR A NOTA FISCAL DE ABASTECIMENTO FIQUE ATENTO, NÃO SERÁ PAGO, PRODUTO FORA DA AUTORIZAÇÃO, NEM VALORES SUPERIORES AOS ESTIPULADOS.</b></td></tr>");              
            //sb.Append("</tr>");
            sb.Append("</table>");  

            // Rodapé ajustado para não encavalar           
             // Assinaturas [cite: 17, 20, 24, 41, 44, 48, 71, 74, 75, 97, 99, 100]
            sb.Append("<div class='footer-sigs'>");           
            string usuario = Session["UsuarioLogado"].ToString();

            if (string.IsNullOrEmpty(usuario))
                usuario = "DESCONHECIDO";
            string data = DateTime.Now.ToString("dd/MM/yyyy HH:mm");            
            sb.Append("<div class='sig-box'>Autorizado<br/>" +
                      usuario + "<br/> " +
                      data +
                      "</div>");
            sb.Append("<div class='sig-box'>Motorista<br/>" +
                r["nommot"] + "</div>");  
            
            sb.Append("<div class='sig-box'>Resp. Posto</div>");

            sb.Append("</div>");
            sb.Append("<table style='width:100%;'>");
            sb.Append("<tr>");

            // coluna 1
            sb.Append("<td style='text-align:left; font-size:8px;'>Gerada em: " + r["lancado_por"] + "</td>");

            // coluna 2
            sb.Append("<td style='text-align:right; font-size:8px;'>");

            if (Convert.ToInt32(r["num_impressao"]) > 0)
            {
                sb.Append("<b>REIMPRESSÃO (" + r["num_impressao"] + ")</b>");
            }

            sb.Append("</td>");

            sb.Append("</tr>");
            sb.Append("</table>");


            //sb.Append("</div>");
            return sb.ToString();
        }
        private DataTable BuscarDadosOrdem(string ordem)
        {
            DataTable dt = new DataTable();

            // Obtém a string de conexão do Web.config
            string strConexao = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString;

            // O SELECT baseado nos campos da sua tabela 
            string sql = @"SELECT
                          [ordem_abastecimento], [data_geracao], [cod_posto], [nome_posto], 
                          [tipo_abastecimento], [frota_agregado], [filial], [cod_combustivel], 
                          [combustivel], [litros], [valor_unitario], [valor_total], 
                          [numero_documento], [tipo_documento], [data_emissao], [codmot], 
                          [nommot], [cpf], [codvei], [plavei], [descricao_veiculo], 
                          [codtra], [nomtra], [cnpj_cpf],[lancado_por],[num_impressao],[impressa],  [token_acesso],[cpf],[plavei]
                   FROM [dbo].[tbsaida_combustivel] 
                   WHERE [ordem_abastecimento] = @ordem";

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
            // Usamos COALESCE no num_impressao para garantir que comece em 1 caso esteja NULL
            string sql = @"UPDATE [dbo].[tbsaida_combustivel] 
                   SET impressa = 'S', 
                       num_impressao = ISNULL(num_impressao, 0) + 1, 
                       data_impressao = GETDATE() 
                   WHERE [ordem_abastecimento] = @ordem";

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
    }
}