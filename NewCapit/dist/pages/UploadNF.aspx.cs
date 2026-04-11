using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NewCapit.dist.pages
{
    public partial class UploadNF : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                litOrdem.Text = Request.QueryString["id"];
            }
        }

        protected void btnEnviar_Click(object sender, EventArgs e)
        {
            string id = Request.QueryString["id"];
            string tk = Request.QueryString["tk"];

            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(tk))
            {
                lblStatus.Text = "❌ Link inválido.";
                lblStatus.CssClass = "text-danger";
                return;
            }

            if (FileUpload1.HasFile)
            {
                try
                {
                    // Validação de segurança: Verifica se id e token combinam no banco
                    if (ValidarToken(id, tk))
                    {
                        string extensao = Path.GetExtension(FileUpload1.FileName).ToLower();
                        string nomeArquivo = "NF_" + id + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + extensao;

                        // Garante que a pasta existe
                        string pasta = Server.MapPath("~/docnf/");
                        if (!Directory.Exists(pasta)) Directory.CreateDirectory(pasta);

                        FileUpload1.SaveAs(Path.Combine(pasta, nomeArquivo));

                        // Atualiza banco de dados
                        AtualizarArquivoNoBanco(id, nomeArquivo);

                        lblStatus.Text = "✅ Arquivo enviado com sucesso!";
                        lblStatus.CssClass = "text-success";
                        btnEnviar.Visible = false; // Esconde o botão após sucesso
                    }
                    else
                    {
                        lblStatus.Text = "❌ Falha na autenticação do token.";
                        lblStatus.CssClass = "text-danger";
                    }
                }
                catch (Exception ex)
                {
                    lblStatus.Text = "❌ Erro: " + ex.Message;
                    lblStatus.CssClass = "text-danger";
                }
            }
            else
            {
                lblStatus.Text = "⚠️ Selecione um arquivo primeiro.";
                lblStatus.CssClass = "text-warning";
            }
        }

        private bool ValidarToken(string id, string tk)
        {
            string connStr = WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "SELECT COUNT(1) FROM tbsaida_combustivel WHERE ordem_abastecimento = @id AND token_acesso = @tk";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@tk", tk);
                conn.Open();
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        private void AtualizarArquivoNoBanco(string id, string nomeArquivo)
        {
            string connStr = WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "UPDATE tbsaida_combustivel SET arquivonf = @arq WHERE ordem_abastecimento = @id";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@arq", nomeArquivo);
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}