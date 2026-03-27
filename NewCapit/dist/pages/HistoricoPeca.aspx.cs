using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Configuration;

namespace NewCapit.dist.pages
{
    public partial class HistoricoPeca : System.Web.UI.Page
    {
        string conexao = WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
        string idPeca;
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
                CarregaDadosDaPeca();

                if (Request.QueryString["id"] != null)
                {
                    ViewState["idPeca"] = Request.QueryString["id"].ToString();
                    CarregaDadosDaPeca();

                    CarregarHistorico(ViewState["idPeca"].ToString()); // já carrega ao abrir
                }
            }
        }
        public void CarregaDadosDaPeca()
        {
            if (HttpContext.Current.Request.QueryString["id"].ToString() != "")
            {
                idPeca = HttpContext.Current.Request.QueryString["id"].ToString();
            }

            string codigoPeca = idPeca;
            string strConn = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                string queryTNG = "SELECT id_peca, descricao_peca, estoque_peca, unidade FROM tbestoque_pecas WHERE id_peca =  @id_peca";

                using (SqlCommand cmdTNG = new SqlCommand(queryTNG, conn))
                {
                    cmdTNG.Parameters.AddWithValue("@id_peca", codigoPeca);
                    conn.Open();

                    using (SqlDataReader reader = cmdTNG.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Converte para string antes de atribuir aos TextBox
                            txtIdPeca.Text = reader["id_peca"].ToString();
                            txtPeca.Text = reader["descricao_peca"].ToString();
                            txtEstoqueAtual.Text = reader["estoque_peca"].ToString();
                            txtUnidade.Text = reader["unidade"].ToString();
                        }
                    }
                }
            }
        }
        protected void ddlPeriodo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ViewState["idPeca"] != null)
            {
                CarregarHistorico(ViewState["idPeca"].ToString());
            }
        }
        private void CarregarHistorico(string idPeca)
        {
            int dias = Convert.ToInt32(ddlPeriodo.SelectedValue);

            string sql = @"
                            SELECT 
                                fornecedor,
                                nota_fiscal,
                                emissao_nf,
                                quantidade,
                                valor_unitario,
                                (quantidade * valor_unitario) AS valor_total
                            FROM tbentrada_peca
                            WHERE id_peca = @idPeca
                        ";

            if (dias > 0)
            {
                sql += " AND emissao_nf >= DATEADD(DAY, -@dias, GETDATE())";
            }

            sql += " ORDER BY emissao_nf DESC";

            string strConn = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(strConn))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@idPeca", idPeca);

                if (dias > 0)
                    cmd.Parameters.AddWithValue("@dias", dias);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvHistorico.DataSource = dt;
                gvHistorico.DataBind();
            }
        }
        protected void Mensagem(string tipo, string texto)
        {
            divMsg.Visible = true;

            divMsg.Attributes["class"] =
                "alert alert-" + tipo + " alert-dismissible fade show mt-3";

            lblMsgGeral.Text = texto;
        }
    }
}
