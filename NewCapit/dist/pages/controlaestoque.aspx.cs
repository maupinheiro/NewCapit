using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Configuration;
using System.Globalization;

namespace NewCapit.dist.pages
{
    public partial class controlaestoque : System.Web.UI.Page
    {
        string conexao = WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString;

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
                ListarPecas();

            }
        }
        private void ListarPecas()
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                string sql = @"SELECT 
                id_peca,
                descricao_peca,
                unidade,
                estoque_peca,
                estoque_minimo,
                valor_unitario,
                tipo_peca       
                FROM tbestoque_pecas
                where (
                    @pesquisa IS NULL
                    OR id_peca LIKE '%' + @pesquisa + '%'
                    OR descricao_peca LIKE '%' + @pesquisa + '%'    
                    )
                    ORDER BY descricao_peca DESC";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@pesquisa",
                string.IsNullOrWhiteSpace(txtPesquisa.Text) ? (object)DBNull.Value : txtPesquisa.Text);
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvEstoque.DataSource = dt;
                gvEstoque.DataBind();
            }
        }
        protected void gvEstoque_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                object estoqueObj = DataBinder.Eval(e.Row.DataItem, "estoque_peca");
                int estoqueAtual = estoqueObj != DBNull.Value ? Convert.ToInt32(estoqueObj) : 0;

                object estoqueMinObj = DataBinder.Eval(e.Row.DataItem, "estoque_minimo");
                int estoqueMin = estoqueMinObj != DBNull.Value ? Convert.ToInt32(estoqueMinObj) : 0;

                if (estoqueAtual <= estoqueMin)
                {
                    e.Row.CssClass = "estoque-baixo";
                    e.Row.ToolTip = "Atenção: Estoque abaixo ou igual ao mínimo!";
                }

                // Pega o valor do TIPO_PECA
                string tipoPeca = DataBinder.Eval(e.Row.DataItem, "tipo_peca").ToString();

                // Localiza o botão de Entrada
                
                LinkButton btnEntrada = (LinkButton)e.Row.FindControl("btnEntrada");
                if (tipoPeca.Equals("COMBUSTIVEL", StringComparison.OrdinalIgnoreCase))
                {
                    // Desabilita botão
                    btnEntrada.Enabled = false;

                    // Opcional: muda cor / tooltip
                    btnEntrada.CssClass = "btn btn-secondary btn-sm disabled";
                    btnEntrada.ToolTip = "Não é possível dar entrada em combustível, por aqui.";

                    // Opcional: destaca a linha
                    e.Row.BackColor = System.Drawing.Color.LightYellow;
                }
            }
        }
        protected void gvEstoque_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "entrada")
            { 
                string idPeca = e.CommandArgument.ToString();

                Response.Redirect("EntradaPecas.aspx?id=" + idPeca);
            }
            if (e.CommandName == "historico")
            {
                string idPeca = e.CommandArgument.ToString();

                Response.Redirect("HistoricoPeca.aspx?id=" + idPeca);
            }
        }
        protected void PesquisaPeca(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(
                System.Configuration.ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                string sql = @"SELECT 
                id_peca,
                descricao_peca,
                unidade,
                estoque_peca,
                estoque_minimo,
                valor_unitario
                FROM tbestoque_pecas
                where (
                    @pesquisa IS NULL
                    OR id_peca LIKE '%' + @pesquisa + '%'
                    OR descricao_peca LIKE '%' + @pesquisa + '%'    
                    )
                    ORDER BY descricao_peca DESC";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@pesquisa",
                string.IsNullOrWhiteSpace(txtPesquisa.Text) ? (object)DBNull.Value : txtPesquisa.Text);
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvEstoque.DataSource = dt;
                gvEstoque.DataBind();
            }
        }
        protected void btnSalvarPecaModal_Click(object sender, EventArgs e)
        {
            string descricao = txtDescricaoPecaModal.Text.Trim();
            string unidade = ddlUnidadeModal.SelectedValue;
            string estoque = txtEstoqueMinimoModal.Text.Trim();

            if (string.IsNullOrWhiteSpace(txtDescricaoPecaModal.Text) ||
        string.IsNullOrEmpty(ddlUnidadeModal.SelectedValue))
            {
                //lblMsgModal.Text = "Preencha todos os campos corretamente!";
                Mensagem("info", "Preencha todos os campos corretamente!");

                // 🔥 Reabre o modal
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", "$('#modalCadastrarPeca').modal('show');", true);
                return;
            }

            // salvar normalmente...

            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                conn.Open();

                // Verifica se a peça já existe
                string sqlCheck = "SELECT COUNT(*) FROM tbestoque_pecas WHERE descricao_peca=@desc";
                SqlCommand cmdCheck = new SqlCommand(sqlCheck, conn);
                cmdCheck.Parameters.AddWithValue("@desc", descricao.ToUpper());
                int count = Convert.ToInt32(cmdCheck.ExecuteScalar());

                if (count > 0)
                {
                    Mensagem("danger", "Peça já cadastrada no sistema.");
                    return; // Mantém modal aberto
                }

                // Inserir nova peça
                string sqlInsert = @"INSERT INTO tbestoque_pecas (descricao_peca, unidade, estoque_minimo) 
                             VALUES (@desc, @unidade, @estoqueMin)";
                SqlCommand cmdInsert = new SqlCommand(sqlInsert, conn);
                cmdInsert.Parameters.AddWithValue("@desc", descricao.ToUpper());
                cmdInsert.Parameters.AddWithValue("@unidade", unidade);
                cmdInsert.Parameters.AddWithValue("@estoqueMin", estoque);
                cmdInsert.ExecuteNonQuery();

                // Limpa campos e mensagens
                txtDescricaoPecaModal.Text = "";
                ddlUnidadeModal.SelectedIndex = 0;
                txtEstoqueMinimoModal.Text = "";
                lblMsgModal.Text = "";
                Mensagem("success", "Peça cadastrada com sucesso.");
                // Fecha o modal somente após salvar com sucesso
                // Se quiser fechar após salvar:
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", "$('#modalCadastrarPeca').modal('hide');", true);
            }
            // Atualiza GridView
            ListarPecas();
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
