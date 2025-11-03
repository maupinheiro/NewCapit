using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

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

                Response.Redirect("Frm_CadPosto.aspx?id=" + id);
            }
        }
        protected void Editar(object sender, EventArgs e)
        {
            using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
            {
                string id = gvListPostos.DataKeys[row.RowIndex].Value.ToString();

                Response.Redirect("Frm_OrdemAbastecimento.aspx?id=" + id);
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
    }
}