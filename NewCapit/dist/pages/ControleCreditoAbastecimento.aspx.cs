using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using Org.BouncyCastle.Asn1.Cmp;

namespace NewCapit.dist.pages
{
    public partial class ControleCreditoAbastecimento : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString());
        private SqlCommand cmd;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                listarProprietarios();                

            }

        }
        private void listarProprietarios()
        {

            var dataTable = DAL.ConAgregados.FetchDataTableLimite();
            if (dataTable.Rows.Count <= 0)
            {
                return;
            }
            gvListLimiteCredito.DataSource = dataTable;
            gvListLimiteCredito.DataBind();

        }
        
        protected void gvListLimiteCredito_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvListLimiteCredito.PageIndex = e.NewPageIndex;
            listarProprietarios();  // Método para recarregar os dados no GridView
        }
        private void AllData(string searchTerm = "")
        {
            var dataTable = DAL.ConAgregados.FetchDataTable2(searchTerm);
            if (dataTable.Rows.Count <= 0)
            {
                gvListLimiteCredito.DataSource = null;
                gvListLimiteCredito.DataBind();
                return;
            }

            gvListLimiteCredito.DataSource = dataTable;
            gvListLimiteCredito.DataBind();
        }
        protected void myInput_TextChanged(object sender, EventArgs e)
        {
            string searchTerm = myInput.Text.Trim();
            AllData(searchTerm);
        }
        protected void Editar(object sender, EventArgs e)
        {
            using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
            {
                string id = gvListLimiteCredito.DataKeys[row.RowIndex].Value.ToString();

                // Response.Redirect("Frm_AltMotoristas.aspx?id=" + id);
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {

        }

        protected void gvListLimiteCredito_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "LimiteCredito")
            {
                int id = Convert.ToInt32(e.CommandArgument);

                string connStr = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    SqlCommand cmd1 = new SqlCommand("SELECT ID, codtra, fantra, pessoa, cnpj, filial, fone2, ativa_inativa, limitecreditoabastecimento, saldoparaabastecimento FROM tbtransportadoras WHERE id = @Id", conn);
                    cmd1.Parameters.AddWithValue("@Id", id);
                    SqlDataReader reader1 = cmd1.ExecuteReader();
                    if (reader1.Read())
                    {
                        txtCodTra.Text = reader1["codtra"].ToString();
                        txtFanTra.Text = reader1["fantra"].ToString();
                        txtFilial.Text = reader1["filial"].ToString();
                        txtPessoa.Text = reader1["pessoa"].ToString();
                        txtCnpj.Text = reader1["cnpj"].ToString();
                        txtAtivo_Inativo.Text = reader1["ativa_inativa"].ToString();
                        txtLimiteCreditoAbastecimento.Text = reader1["limitecreditoabastecimento"].ToString();
                        txtSaldoParaAbastecimento.Text = reader1["saldoparaabastecimento"].ToString();
                        hdfId.Value = id.ToString();
                        
                    }
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#modalCadastro').modal('show');", true);

                }



            }
        }
    }
}