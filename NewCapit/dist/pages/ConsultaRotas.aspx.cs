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
    public partial class ConsultaRotas : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString());
        protected void Page_Load(object sender, EventArgs e)
        {           
            if (!IsPostBack)
            {
                AllDataRotas();
            }
        }       
        private void AllDataRotas()
        {
            var dataTable = DAL.ConRotas.FetchDataTable();
            if (dataTable.Rows.Count <= 0)
            {
                return;
            }
            gvListRotas.DataSource = dataTable;
            gvListRotas.DataBind();

            //gvListAgregados.UseAccessibleHeader = true;
            //gvListAgregados.HeaderRow.TableSection = TableRowSection.TableHeader;
            //gvListAgregados.FooterRow.TableSection = TableRowSection.TableFooter;

        }
        protected void gvListRotas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvListRotas.PageIndex = e.NewPageIndex;
            AllDataRotas();  // Método para recarregar os dados no GridView
        }
       
        protected void Editar(object sender, EventArgs e)
        {
            using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
            {
                string id = gvListRotas.DataKeys[row.RowIndex].Value.ToString();

                Response.Redirect("Frm_Rotas.aspx?id=" + id);
            }
        }
        private void AllData(string searchTerm = "")
        {
            var dataTable = DAL.ConRotas.FetchDataTable2(searchTerm);
            if (dataTable.Rows.Count <= 0)
            {
                gvListRotas.DataSource = null;
                gvListRotas.DataBind();
                return;
            }

            gvListRotas.DataSource = dataTable;
            gvListRotas.DataBind();
        }

        protected void myInput_TextChanged(object sender, EventArgs e)
        {
            string searchTerm = myInput.Text.Trim();
            AllData(searchTerm);
        }
    }
}