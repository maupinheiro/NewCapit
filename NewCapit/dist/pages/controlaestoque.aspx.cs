using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace NewCapit.dist.pages
{
    public partial class controlaestoque : System.Web.UI.Page
    {
        string conexao = WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregarProdutos();
                CarregarEstoque();
                CarregarHistorico();
            }
        }

        private void CarregarProdutos()
        {
            using (SqlConnection conn = new SqlConnection(conexao))
            {
                string sql = "SELECT id_peca, descricao_peca FROM tbestoque_pecas ORDER BY descricao_peca";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlProdutos.DataSource = dt;
                ddlProdutos.DataTextField = "descricao_peca";
                ddlProdutos.DataValueField = "id_peca";
                ddlProdutos.DataBind();
                ddlProdutos.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Selecione...", ""));
            }
        }

        private void CarregarEstoque(string pesquisa = null)
        {
            using (SqlConnection conn = new SqlConnection(conexao))
            {
                string sql = "SELECT * FROM tbestoque_pecas WHERE (@pesquisa IS NULL OR descricao_peca LIKE '%' + @pesquisa + '%') ORDER BY descricao_peca";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@pesquisa", string.IsNullOrWhiteSpace(pesquisa) ? (object)DBNull.Value : pesquisa.Trim());

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvEstoque.DataSource = dt;
                gvEstoque.DataBind();
            }
        }

        protected void btnPesquisarProduto_Click(object sender, EventArgs e)
        {
            CarregarEstoque(txtPesquisaProduto.Text);
        }

        protected void gvEstoque_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int estoqueAtual = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "estoque_peca"));
                int estoqueMinimo = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "estoque_minimo"));

                if (estoqueAtual < estoqueMinimo)
                {
                    e.Row.BackColor = System.Drawing.Color.LightCoral; // destaque em vermelho
                }
            }
        }

        protected void btnMovimentar_Click(object sender, EventArgs e)
        {
            int idProduto = Convert.ToInt32(ddlProdutos.SelectedValue);
            string tipoMov = ddlTipoMov.SelectedValue;
            int qtd = Convert.ToInt32(txtQtd.Text);
            string obs = txtObs.Text;
            string responsavel = Session["UsuarioLogado"].ToString();

            using (SqlConnection conn = new SqlConnection(conexao))
            {
                conn.Open();

                string sqlMov = @"INSERT INTO tbmovimentacao_pecas 
                              (id_peca, tipoMov, quantidade, dataMov, responsavel, observacao)
                              VALUES (@idProd, @tipo, @qtd, GETDATE(), @resp, @obs)";
                SqlCommand cmdMov = new SqlCommand(sqlMov, conn);
                cmdMov.Parameters.AddWithValue("@idProd", idProduto);
                cmdMov.Parameters.AddWithValue("@tipo", tipoMov);
                cmdMov.Parameters.AddWithValue("@qtd", qtd);
                cmdMov.Parameters.AddWithValue("@resp", responsavel);
                cmdMov.Parameters.AddWithValue("@obs", obs);
                cmdMov.ExecuteNonQuery();

                string sqlAtualiza = tipoMov == "E" ?
                    "UPDATE tbestoque_pecas SET estoque_peca = estoque_peca + @qtd WHERE id_peca = @idProd" :
                    "UPDATE tbestoque_pecas SET estoque_peca = estoque_peca - @qtd WHERE id_peca = @idProd";
                SqlCommand cmdAtualiza = new SqlCommand(sqlAtualiza, conn);
                cmdAtualiza.Parameters.AddWithValue("@qtd", qtd);
                cmdAtualiza.Parameters.AddWithValue("@idProd", idProduto);
                cmdAtualiza.ExecuteNonQuery();
            }

            CarregarEstoque();
            CarregarHistorico();

            txtQtd.Text = "";
            txtObs.Text = "";
        }

        private void CarregarHistorico()
        {
            using (SqlConnection conn = new SqlConnection(conexao))
            {
                string sql = @"
            SELECT m.idMov, p.descricao_peca, m.tipoMov, m.quantidade, m.dataMov, m.responsavel, m.observacao
            FROM tbmovimentacao_pecas m
            INNER JOIN tbestoque_pecas p ON m.id_peca = p.id_peca
            ORDER BY m.dataMov DESC";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvHistorico.DataSource = dt;
                gvHistorico.DataBind();
            }
        }
    }
}
