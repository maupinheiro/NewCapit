using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text.RegularExpressions;
using iText.Html2pdf;
using System.IO;
using System.Text;
using System.Net.Mail;
using System.Net;

namespace NewCapit.dist.pages
{
    public partial class ControleAbastecimento : System.Web.UI.Page
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
                    Response.Redirect("Login.aspx");
                }
                CarregarGrid();
            }

        }
        private void CarregarGrid()
        {
            using (SqlConnection conn = new SqlConnection(
    WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                string query = @"SELECT * FROM tbsaida_combustivel WHERE 1=1";

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;

                // 📅 Data inicial
                if (!string.IsNullOrEmpty(txtDataInicial.Text))
                {
                    DateTime dataInicial = Convert.ToDateTime(txtDataInicial.Text);
                    query += " AND data_geracao >= @dataInicial";
                    cmd.Parameters.Add("@dataInicial", SqlDbType.DateTime).Value = dataInicial;
                }

                // 📅 Data final (ajuste para pegar o dia todo)
                if (!string.IsNullOrEmpty(txtDataFinal.Text))
                {
                    DateTime dataFinal = Convert.ToDateTime(txtDataFinal.Text).AddDays(1).AddSeconds(-1);
                    query += " AND data_geracao <= @dataFinal";
                    cmd.Parameters.Add("@dataFinal", SqlDbType.DateTime).Value = dataFinal;
                }

                // 🔍 Busca
                if (!string.IsNullOrEmpty(txtBusca.Text))
                {
                    query += @" AND (
            nommot LIKE @busca 
            OR plavei LIKE @busca 
            OR CAST(codvei AS VARCHAR) LIKE @busca 
            OR CAST(ordem_abastecimento AS VARCHAR) LIKE @busca
        )";

                    cmd.Parameters.Add("@busca", SqlDbType.VarChar).Value = "%" + txtBusca.Text + "%";
                }

                cmd.CommandText = query;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvAbastecimento.DataSource = dt;
                gvAbastecimento.DataBind();
            }


            //using (SqlConnection conn = new SqlConnection(
            //    WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            //{
            //    string query = @"SELECT * FROM tbsaida_combustivel WHERE 1=1";

            //    if (!string.IsNullOrEmpty(txtDataInicial.Text))
            //        query += " AND data_geracao >= @dataInicial";

            //    if (!string.IsNullOrEmpty(txtDataFinal.Text))
            //        query += " AND data_geracao <= @dataFinal";

            //    if (!string.IsNullOrEmpty(txtBusca.Text))
            //        query += @" AND (nommot LIKE @busca 
            //             OR plavei LIKE @busca 
            //             OR codvei LIKE @busca 
            //             OR ordem_abastecimento LIKE @busca)";

            //    SqlCommand cmd = new SqlCommand(query, conn);

            //    if (!string.IsNullOrEmpty(txtDataInicial.Text))
            //        cmd.Parameters.AddWithValue("@dataInicial", txtDataInicial.Text);

            //    if (!string.IsNullOrEmpty(txtDataFinal.Text))
            //        cmd.Parameters.AddWithValue("@dataFinal", txtDataFinal.Text);

            //    if (!string.IsNullOrEmpty(txtBusca.Text))
            //        cmd.Parameters.AddWithValue("@busca", "%" + txtBusca.Text + "%");

            //    SqlDataAdapter da = new SqlDataAdapter(cmd);
            //    DataTable dt = new DataTable();
            //    da.Fill(dt);

            //    gvAbastecimento.DataSource = dt;
            //    gvAbastecimento.DataBind();
            //}
        }
        protected void gvAbastecimento_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvAbastecimento.PageIndex = e.NewPageIndex;
            CarregarGrid();
        }
        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            CarregarGrid();
        }        
        protected void gvAbastecimento_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string ordem = e.CommandArgument.ToString();

            if (e.CommandName == "Imprimir")
            {
                

                // 1. Verificar se a ordem já possui arquivo de nota fiscal
                if (OrdemJaConcluida(ordem))
                {
                    // Se o campo arquivonf não estiver vazio, exibe o alerta e bloqueia a impressão
                    string msg = "alert('Ordem de Abastecimento já concluída! Não é permitido imprimir após o envio da Nota Fiscal.');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "AlertaConcluido", msg, true);
                }
                else
                {
                    // 2. Se estiver vazio, segue com o processo normal de impressão
                    string url = "ImprimirOrdem.aspx?id=" + ordem;
                    string script = "window.open('" + url + "', '_blank', 'width=800,height=600');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "PrintWindow", script, true);
                }
            }
            else if (e.CommandName == "WhatsApp")
            {
                if (e.CommandName == "WhatsApp")
                {
                    string ordemAtual = hdOrdem.Value;                    

                    ScriptManager.RegisterStartupScript(this, GetType(),
                        "modal", "abrirModal('" + ordemAtual + "');", true);
                }

                if (e.CommandName == "EnviarWhats")
                {
                    EnviarWhatsApp();
                }
            }
            else if (e.CommandName == "Email")
            {                
                if (e.CommandName == "Email")
                {
                    string ordemAtual = hdOrdem.Value;                    

                    ScriptManager.RegisterStartupScript(this, GetType(),
                        "modal", "abrirModalEmail('" + ordemAtual + "');", true);
                }

                if (e.CommandName == "EnviarEmail")
                {
                    EnviarEmailComPdf();
                }
            }
            else if (e.CommandName == "Confirmar")
            {
                ConfirmarAbastecimento(ordem);
            }
            else if (e.CommandName == "Cancelar")
            {
                CancelarAbastecimento(ordem);
            }
            else if (e.CommandName == "Visualizar")
            {
                // abrir modal ou redirecionar
            }
        }
        private bool OrdemJaConcluida(string ordem)
        {
            bool concluida = false;
            string connStr = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString;

            // Consulta para verificar se arquivonf está preenchido (não nulo e não vazio)
            string sql = "SELECT COUNT(1) FROM tbsaida_combustivel WHERE ordem_abastecimento = @id AND arquivonf IS NOT NULL AND arquivonf <> ''";

            using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(connStr))
            {
                using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", ordem);
                    conn.Open();
                    int count = (int)cmd.ExecuteScalar();
                    concluida = (count > 0);
                }
            }
            return concluida;
        }
        protected void gvAbastecimento_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lbl = (Label)e.Row.FindControl("lblStatus");

                if (lbl != null)
                {
                    switch (lbl.Text)
                    {
                        case "PENDENTE":
                            lbl.CssClass += " bg-warning";
                            break;
                        case "IMPRESSA":
                            lbl.CssClass += " bg-success";
                            break;
                        case "REIMPRESSA":
                            lbl.CssClass += " bg-primary";
                            break;
                        case "CANCELADA":
                            lbl.CssClass += " bg-danger";
                            break;
                        case "UTILIZADA":
                            lbl.CssClass += " bg-secondary";
                            break;
                    }
                }
            }
        }
        private void ConfirmarAbastecimento(string ordem)
        {
            //using (SqlConnection conn = new SqlConnection(
            //    WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            //{
            //    string query = "UPDATE tbsaida_combustivel SET status = 'CONFIRMADO' WHERE ordem_abastecimento = @ordem";

            //    SqlCommand cmd = new SqlCommand(query, conn);
            //    cmd.Parameters.AddWithValue("@ordem", ordem);

            //    conn.Open();
            //    cmd.ExecuteNonQuery();
            //}

            CarregarGrid();
        }
        private void CancelarAbastecimento(string ordem)
        {
            //using (SqlConnection conn = new SqlConnection(
            //    WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            //{
            //    string query = "UPDATE tbsaida_combustivel SET status = 'CONFIRMADO' WHERE ordem_abastecimento = @ordem";

            //    SqlCommand cmd = new SqlCommand(query, conn);
            //    cmd.Parameters.AddWithValue("@ordem", ordem);

            //    conn.Open();
            //    cmd.ExecuteNonQuery();
            //}

            CarregarGrid();
        }
        protected void gvAbastecimento_DataBound(object sender, EventArgs e)
        {
            GridViewRow pagerRow = gvAbastecimento.BottomPagerRow;

            if (pagerRow != null)
            {
                Label lblAtual = (Label)pagerRow.FindControl("lblPaginaAtual");
                Label lblTotal = (Label)pagerRow.FindControl("lblTotalPaginas");

                if (lblAtual != null)
                    lblAtual.Text = (gvAbastecimento.PageIndex + 1).ToString();

                if (lblTotal != null)
                    lblTotal.Text = gvAbastecimento.PageCount.ToString();
            }
        }
        protected void btnIrPagina_Click(object sender, EventArgs e)
        {
            GridViewRow pagerRow = gvAbastecimento.BottomPagerRow;

            if (pagerRow != null)
            {
                TextBox txtIr = (TextBox)pagerRow.FindControl("txtIrPagina");

                int pagina;
                if (int.TryParse(txtIr.Text, out pagina))
                {
                    if (pagina > 0 && pagina <= gvAbastecimento.PageCount)
                    {
                        gvAbastecimento.PageIndex = pagina - 1;
                        CarregarGrid();
                    }
                }
            }
        }
        private bool TelefoneValido(string telefone)
        {
            telefone = Regex.Replace(telefone, @"\D", ""); // só números

            // 10 ou 11 dígitos (com DDD)
            return telefone.Length == 10 || telefone.Length == 11;
        }
        private void EnviarWhatsApp()
        {
            string ordem = hdOrdem.Value;

            string telefone = Request.Form["txtTelefone"];
            telefone = telefone.Replace("(", "")
                               .Replace(")", "")
                               .Replace("-", "")
                               .Replace(" ", "");

            // 1. BUSCAR DADOS
            var r = BuscarOrdem(ordem); // seu método já existente

            // 2. GERAR HTML
            string html = GerarHtml(r); // seu StringBuilder atual

            // 3. GERAR PDF
            string nomeArquivo = "Ordem_" + ordem + ".pdf";
            string pasta = Server.MapPath("~/pdf/");
            string caminho = Path.Combine(pasta, nomeArquivo);

            if (!Directory.Exists(pasta))
                Directory.CreateDirectory(pasta);

            using (FileStream fs = new FileStream(caminho, FileMode.Create))
            {
                HtmlConverter.ConvertToPdf(html, fs);
            }

            // 4. LINK PÚBLICO
            string urlPdf = "https://SEU_DOMINIO.com/pdf/" + nomeArquivo;

            // 5. MENSAGEM WHATSAPP
            string msg = "Segue a Ordem de Abastecimento: " + urlPdf;

            string urlWhats =
                "https://wa.me/" + telefone +
                "?text=" + HttpUtility.UrlEncode(msg);

            // 6. ABRIR WHATSAPP
            ScriptManager.RegisterStartupScript(this, GetType(),
                "wa", "window.open('" + urlWhats + "','_blank');", true);
        }
        private DataRow BuscarOrdem(string ordem)
        {
            using (SqlConnection conn = new SqlConnection(
                WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                string sql = @"SELECT TOP 1 *
                       FROM tbsaida_combustivel
                       WHERE ordem_abastecimento = @ordem";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ordem", ordem);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        if (dt.Rows.Count > 0)
                            return dt.Rows[0];
                        else
                            return null;
                    }
                }
            }
        }
        private string GerarHtml(DataRow r)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<html><head>");
            sb.Append("<meta charset='UTF-8'>");

            sb.Append("<style>");
            sb.Append("body { font-family: Arial; font-size:11px; }");
            sb.Append(".page { width:750px; margin:0 auto; }");
            sb.Append(".border { border:1px solid #000; }");
            sb.Append(".center { text-align:center; }");
            sb.Append(".title { font-size:14px; font-weight:bold; }");
            sb.Append("</style>");

            sb.Append("</head><body>");
            sb.Append("<div class='page'>");

            // ===== CABEÇALHO =====
            sb.Append("<div class='center title'>TRANSNOVAG TRANSPORTES S/A</div>");
            sb.Append("<div class='center'>Ordem de Abastecimento</div>");
            sb.Append("<br/>");

            // ===== DADOS =====
            sb.Append("<table class='border' width='100%'>");

            sb.Append("<tr><td><b>Ordem:</b> " + r["ordem_abastecimento"] + "</td></tr>");
            sb.Append("<tr><td><b>Veículo:</b> " + r["plavei"] + "</td></tr>");
            sb.Append("<tr><td><b>Motorista:</b> " + r["nommot"] + "</td></tr>");
            sb.Append("<tr><td><b>Litros:</b> " + r["litros"] + "</td></tr>");
            sb.Append("<tr><td><b>Valor Total:</b> " + r["valor_total"] + "</td></tr>");

            sb.Append("</table>");

            sb.Append("</div>");
            sb.Append("</body></html>");

            return sb.ToString();
        }
        private byte[] GerarPdf(string html)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                HtmlConverter.ConvertToPdf(html, ms);
                return ms.ToArray();
            }
        }
        private void EnviarEmailComPdf()
        {
            string ordem = hdOrdem.Value;

            string emails = Request.Form["txtEmails"];

            if (string.IsNullOrEmpty(emails))
                return;

            var r = BuscarOrdem(ordem);

            if (r == null)
                return;

            string html = GerarHtml(r);

            byte[] pdf = GerarPdf(html);

            MailMessage mail = new MailMessage();

            mail.From = new MailAddress("seuemail@empresa.com");

            // múltiplos e-mails
            foreach (string email in emails.Split(';'))
            {
                if (!string.IsNullOrWhiteSpace(email))
                    mail.To.Add(email.Trim());
            }

            mail.Subject = "Ordem de Abastecimento - " + ordem;
            mail.Body = "Segue em anexo a ordem de abastecimento.";
            mail.IsBodyHtml = true;

            mail.Attachments.Add(new Attachment(
                new MemoryStream(pdf),
                "Ordem_" + ordem + ".pdf"
            ));

            SmtpClient smtp = new SmtpClient("smtp.office365.com", 587);

            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(
                "progtrans2@transnovag.com.br",
                "Sascar@2007"
            );

            smtp.Send(mail);
        }

    }
}