using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

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
                // lógica de impressão
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

    }
}