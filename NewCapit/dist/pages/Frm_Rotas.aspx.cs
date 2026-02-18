using NPOI.SS.Formula.PTG;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Imaging;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NewCapit.dist.pages
{
    public partial class Frm_Rotas : System.Web.UI.Page
    {
        SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString());
        string nomeUsuario = string.Empty;
        DateTime dataHoraAtual = DateTime.Now;
        string id;
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
                CarregaRotas();
            }
            DateTime dataHoraAtual = DateTime.Now;
            //lbDtAtualizacao.Text= dataHoraAtual.ToString("dd/MM/yyyy HH:mm");
           

        }
        public void CarregaRotas()
        {
            if (HttpContext.Current.Request.QueryString["id"].ToString() != "")
            {
                id = HttpContext.Current.Request.QueryString["id"].ToString();
            }
            string rota = id;
            string strConn = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                string queryRota = "SELECT rota,desc_rota,distancia,tempo,deslocamento,situacao,data_cadastro, pedagio, usuario_cadastro, data_alteracao, usuario_alteracao FROM tbrotasdeentregas where rota=@idrota";

                using (SqlCommand cmdTNG = new SqlCommand(queryRota, conn))
                {
                    cmdTNG.Parameters.AddWithValue("@idrota", rota);
                    conn.Open();

                    using (SqlDataReader reader = cmdTNG.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtRota.Text = reader["rota"].ToString();
                            txtDesc_Rota.Text = reader["desc_rota"].ToString();
                            txtDistancia.Text = reader["distancia"].ToString();
                            txtDuracao.Text = reader["tempo"].ToString();
                            cboDeslocamento.SelectedItem.Text = reader["deslocamento"].ToString();
                            ddlStatus.SelectedItem.Text = reader["situacao"].ToString();
                            ddlPedagio.SelectedValue = reader["pedagio"].ToString();
                            txtCadastro.Text = reader["data_cadastro"].ToString();
                            lblDtCadastro.Text = reader["data_cadastro"].ToString();
                            txtUsuCadastro.Text = reader["usuario_cadastro"].ToString();
                            lbDtAtualizacao.Text = reader["data_alteracao"].ToString();
                            txtAltCad.Text = reader["usuario_alteracao"].ToString();
                        }
                    }
                }
            }
        }
        protected void btnAlterar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                string id = Request.QueryString["id"].ToString();
                string strConn = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    string sql = @"
            UPDATE dbo.tbrotasdeentregas
               SET desc_rota           = @desc_rota,                   
                   distancia           = @distancia,
                   tempo               = @tempo,
                   deslocamento        = @deslocamento,
                   situacao            = @situacao,                   
                   pedagio             = @pedagio,
                   data_alteracao      = @data_alteracao,
                   usuario_alteracao   = @usuario_alteracao
                   WHERE rota = @rota";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        // parâmetros
                        cmd.Parameters.AddWithValue("@desc_rota", RemoverAcentos(txtDesc_Rota.Text.Trim()));
                        cmd.Parameters.AddWithValue("@distancia", txtDistancia.Text.Trim().Replace(',', '.'));
                        TimeSpan tempo;

                        string valorDigitado = txtDuracao.Text.Trim();

                        // Validação mínima no C# antes de mandar pro SQL
                        if (!string.IsNullOrEmpty(valorDigitado) && valorDigitado.Contains(":"))
                        {
                            // Se você mudou para NVARCHAR, pode mandar a string direto, 
                            // mas passe por um filtro básico:
                            cmd.Parameters.Add("@tempo", SqlDbType.NVarChar).Value = valorDigitado+"";
                        }
                        else
                        {
                            // Caso alguém tenha burlado a máscara
                            MostrarMsg("O formato do tempo está incorreto.");
                        }
                        cmd.Parameters.AddWithValue("@deslocamento", cboDeslocamento.SelectedItem.Text);
                        cmd.Parameters.AddWithValue("@pedagio", ddlPedagio.SelectedItem.Text);
                        cmd.Parameters.AddWithValue("@situacao", ddlStatus.SelectedItem.Text);
                        cmd.Parameters.Add("@data_alteracao", SqlDbType.DateTime)
              .Value = DateTime.Now;
                        cmd.Parameters.AddWithValue("@usuario_alteracao", Session["UsuarioLogado"].ToString());
                        cmd.Parameters.AddWithValue("@rota", id); // rota (chave)

                        conn.Open();
                        cmd.ExecuteNonQuery();

                        MostrarMsg("Rota alterada com sucesso.", "success");
                    }
                }
            }
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