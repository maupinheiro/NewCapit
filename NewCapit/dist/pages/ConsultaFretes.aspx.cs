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
    public partial class ConsultaFretes : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString());
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                AllDataFretes();
            }
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
        private void AllDataFretes()
        {
            var dataTable = DAL.ConFretes.FetchDataTable();
            if (dataTable.Rows.Count <= 0)
            {
                return;
            }
            gvListFretes.DataSource = dataTable;
            gvListFretes.DataBind();

        }
        protected void gvListFretes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvListFretes.PageIndex = e.NewPageIndex;
            AllDataFretes();  // Método para recarregar os dados no GridView
        }

        protected void Editar(object sender, EventArgs e)
        {
            using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
            {
                string id = gvListFretes.DataKeys[row.RowIndex].Value.ToString();

                Response.Redirect("Frm_AltTransportadoras.aspx?id=" + id);
            }
        }
        private void AllData(string searchTerm = "")
        {
            var dataTable = DAL.ConFretes.FetchDataTable2(searchTerm);
            if (dataTable.Rows.Count <= 0)
            {
                gvListFretes.DataSource = null;
                gvListFretes.DataBind();
                return;
            }

            gvListFretes.DataSource = dataTable;
            gvListFretes.DataBind();
        }

        protected void myInput_TextChanged(object sender, EventArgs e)
        {
            string searchTerm = myInput.Text.Trim();
            AllData(searchTerm);
        }
    }
}