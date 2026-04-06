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
    public partial class GestaoPostos : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString());
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                AllDataPostos();
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
            }
        }

        private void AllDataPostos()
        {
            var dataTable = DAL.ConPostos.FetchDataTable();
            if (dataTable.Rows.Count <= 0)
            {
                return;
            }
            gvListPostos.DataSource = dataTable;
            gvListPostos.DataBind();

        }
        protected void gvListPostos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvListPostos.PageIndex = e.NewPageIndex;
            AllDataPostos();  // Método para recarregar os dados no GridView
        }
        protected void Reajuste(object sender, EventArgs e)
        {
            using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
            {
                string id = gvListPostos.DataKeys[row.RowIndex].Value.ToString();

                Response.Redirect("Frm_CadPosto.aspx?codfor=" + id);
            }
        }
        protected void Editar(object sender, EventArgs e)
        {
            using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
            {
                string id = gvListPostos.DataKeys[row.RowIndex].Value.ToString();

                Response.Redirect("Frm_OrdemAbastecimento.aspx?codfor=" + id);
            }
        }
        private void AllData(string searchTerm = "")
        {
            var dataTable = DAL.ConPostos.FetchDataTable2(searchTerm);
            if (dataTable.Rows.Count <= 0)
            {
                gvListPostos.DataSource = null;
                gvListPostos.DataBind();
                return;
            }

            gvListPostos.DataSource = dataTable;
            gvListPostos.DataBind();
        }
        protected void myInput_TextChanged(object sender, EventArgs e)
        {
            string searchTerm = myInput.Text.Trim();
            AllData(searchTerm);
        }
        protected void lnkHistorico_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            string fantasia = row.Cells[2].Text; // coluna 2 (0 = primeira coluna visível)
            string codfor = gvListPostos.DataKeys[row.RowIndex].Value.ToString();
            ViewState["codforHistorico"] = codfor;

            // Preenche os TextBox
            txtCodFor.Text = codfor;
            txtFornecedor.Text = fantasia;

            // Carrega histórico inicial
            CarregarHistorico(codfor, 30);

            // Abre o modal via ScriptManager
            string script = "$('#" + pnlHistorico.ClientID + "').modal('show');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModal", script, true);
        }        
        private void CarregarHistorico(string codfor, int dias)
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                conn.Open();
                string query = @"
            SELECT combustivel, valor, dtinicio, dtreajuste, reajustadopor
            FROM tbPrecoCombustivel
            WHERE codposto = @codfor 
              AND dtinicio >= DATEADD(DAY, -@dias, GETDATE())
            ORDER BY dtinicio DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@codfor", codfor);
                cmd.Parameters.AddWithValue("@dias", dias);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ViewState["HistoricoDT"] = dt; // salva para ordenação

                gvHistorico.DataSource = dt;
                gvHistorico.DataBind();
            }
        }
        protected void ddlPeriodo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ViewState["codforHistorico"] != null)
            {
                string codfor = ViewState["codforHistorico"].ToString();
                int dias = int.Parse(ddlPeriodo.SelectedValue);

                // Recarrega histórico
                CarregarHistorico(codfor, dias);

                // Reabre o modal
                string script = "$('#" + pnlHistorico.ClientID + "').modal('show');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModal", script, true);
            }
        }
        protected void gvHistorico_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (ViewState["HistoricoDT"] != null)
            {
                DataTable dt = (DataTable)ViewState["HistoricoDT"];
                string sortDirection = GetSortDirection(e.SortExpression);
                dt.DefaultView.Sort = e.SortExpression + " " + sortDirection;
                gvHistorico.DataSource = dt;
                gvHistorico.DataBind();
            }
        }
        private string GetSortDirection(string column)
        {
            string sortDirection = "ASC";

            // Verifica se já existe sort expression
            string sortExpression = ViewState["SortExpression"] as string;
            if (sortExpression != null)
            {
                if (sortExpression == column)
                {
                    string lastDirection = ViewState["SortDirection"] as string;
                    if ((lastDirection != null) && (lastDirection == "ASC"))
                    {
                        sortDirection = "DESC";
                    }
                }
            }

            ViewState["SortDirection"] = sortDirection;
            ViewState["SortExpression"] = column;

            return sortDirection;
        }
    }
}