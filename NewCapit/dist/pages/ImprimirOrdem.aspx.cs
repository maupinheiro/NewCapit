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

namespace NewCapit.dist.pages
{
    public partial class ImprimirOrdem : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
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
            string qrUrl = $"https://api.qrserver.com/v1/create-qr-code/?size=200x200&data={HttpUtility.UrlEncode(linkQr)}";
            sb.Append("<div class='qrcode-container'>");
            sb.Append($"<div class='qrcode-box'><img src='{qrUrl}' /></div>");
            sb.Append("</div>");  
            sb.Append("</td>");
            sb.Append("</tr>");
            sb.Append("</table>");

            //sb.Append("<div style='display:flex; justify-content:space-between;'>");
            //sb.Append("<span>" + identificacaoVia + "</span>"); // [cite: 52, 53, 81, 82]
            //sb.Append("<span><b>Nº Ordem: " + r["ordem_abastecimento"] + "</b></span>"); // [cite: 3, 4, 33, 34, 77, 101]
            //sb.Append("</div>");

             // Tabela de Dados [cite: 7, 37]
            sb.Append("<table class='content-table'>");
            sb.Append("<tr><td colspan='2'><b>Tipo:</b> " + r["tipo_abastecimento"] + "</td>"); // [cite: 54, 83]
            sb.Append("<td><b>Veículo:</b> " + r["plavei"] + "</td></tr>"); // [cite: 5, 6, 35, 36, 62, 89]

            sb.Append("<tr><td colspan='2'><b>Fornecedor:</b> " + r["nome_posto"] + "</td>"); // [cite: 55, 84]
            sb.Append("<td><b>Frota:</b> Proprio</td></tr>"); // [cite: 63, 89]

            sb.Append("<tr><td colspan='2'><b>Produto:</b> " + r["combustivel"] + "</td>"); // [cite: 56, 85]
            
            sb.Append("<td><b>Descrição:</b> " + r["descricao_veiculo"] + "</td></tr>"); // [cite: 64, 73, 90, 98]
            sb.Append("<td><b>" + r["descricao_veiculo"] + "</td></tr>"); // [cite: 64, 73, 90, 98]

            sb.Append("<tr><td><b>Preço:</b> " + string.Format("{0:N2}", r["valor_unitario"]) + "</td>"); // [cite: 57, 86]
            sb.Append("<td><b>Doc:</b> " + r["numero_documento"] + "</td>"); // [cite: 57, 86]
            sb.Append("<td><b>Motorista:</b> " + r["nommot"] + "</td></tr>"); // [cite: 66, 92]

            sb.Append("<tr><td colspan='2'><b>Proprietário:</b> TRANSNOVAG S/A</td>"); // [cite: 57, 86]
            sb.Append("<td><b>CPF:</b> " + r["cpf"] + "</td></tr>"); // [cite: 67, 93]

            sb.Append("<tr><td colspan='2'><b>Lts Autorizada:</b> COMPLETAR</td>"); // [cite: 13, 38, 58, 87]
            sb.Append("<td><b>KM Atual:</b> ________________</td></tr>"); // [cite: 68, 94]
            sb.Append("</table>");

            // Rodapé ajustado para não encavalar
            sb.Append("<div class='info-footer'>");
            if (Convert.ToInt32(r["num_impressao"]) > 0)
            {
                sb.Append("<div class='reimpressao-texto'>** REIMPRESSÃO **</div>"); // [cite: 23, 47, 69, 95]
            }
            sb.Append("<div class='emissao-dados'>");
            sb.Append("Emissão: " + string.Format("{0:dd/MM/yyyy HH:mm}", r["data_geracao"]) + " por " + r["lancado_por"]); // [cite: 12, 31, 59, 87]
            sb.Append("</div>");
            sb.Append("</div>");

             // Assinaturas [cite: 17, 20, 24, 41, 44, 48, 71, 74, 75, 97, 99, 100]
            sb.Append("<div class='footer-sigs'>");
            sb.Append("<div class='sig-box'>Autorizado</div>");
            sb.Append("<div class='sig-box'>Motorista</div>");
            sb.Append("<div class='sig-box'>Resp. Posto</div>");
            sb.Append("</div>");

            // QR Code [cite: 78, 102]
            //string qrUrl = $"https://api.qrserver.com/v1/create-qr-code/?size=200x200&data={HttpUtility.UrlEncode(linkQr)}";

            //sb.Append("<div class='qrcode-container'>");
            //sb.Append($"<div class='qrcode-box'><img src='{qrUrl}' /></div>");
            //sb.Append("</div>");

            sb.Append("</div>");
            return sb.ToString();
        }
        private DataTable BuscarDadosOrdem(string ordem)
        {
            DataTable dt = new DataTable();

            // Obtém a string de conexão do Web.config
            string strConexao = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString;

            // O SELECT baseado nos campos da sua tabela 
            string sql = @"SELECT [ordem_abastecimento], [data_geracao], [cod_posto], [nome_posto], 
                          [tipo_abastecimento], [frota_agregado], [filial], [cod_combustivel], 
                          [combustivel], [litros], [valor_unitario], [valor_total], 
                          [numero_documento], [tipo_documento], [data_emissao], [codmot], 
                          [nommot], [cpf], [codvei], [plavei], [descricao_veiculo], 
                          [codtra], [nomtra], [cnpj_cpf],[lancado_por],[num_impressao],[impressa],[token_acesso],[cpf],[plavei]
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