using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NewCapit.dist.pages
{
    public partial class ConsultaFornecedores : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString());
        protected void Page_Load(object sender, EventArgs e)
        {            
            if (!IsPostBack)
            {
                AllDataFornecedores();
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

        private void AllDataFornecedores()
        {
            var dataTable = DAL.ConFornecedores.FetchDataTable();
            if (dataTable.Rows.Count <= 0)
            {
                return;
            }
            gvListFornecedores.DataSource = dataTable;
            gvListFornecedores.DataBind();

        }
        protected void gvListFornecedores_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvListFornecedores.PageIndex = e.NewPageIndex;
            AllDataFornecedores();  // Método para recarregar os dados no GridView
        }
       
        protected void Editar(object sender, EventArgs e)
        {
            using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
            {
                string id = gvListFornecedores.DataKeys[row.RowIndex].Value.ToString();

                Response.Redirect("Frm_AltFornecedor.aspx?id=" + id);
            }
        }
        private void AllData(string searchTerm = "")
        {
            var dataTable = DAL.ConFornecedores.FetchDataTable2(searchTerm);
            if (dataTable.Rows.Count <= 0)
            {
                gvListFornecedores.DataSource = null;
                gvListFornecedores.DataBind();
                return;
            }

            gvListFornecedores.DataSource = dataTable;
            gvListFornecedores.DataBind();
        }

        protected void myInput_TextChanged(object sender, EventArgs e)
        {
            string searchTerm = myInput.Text.Trim();
            AllData(searchTerm);
        }
    }
}