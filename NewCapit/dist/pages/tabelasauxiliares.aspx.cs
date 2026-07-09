using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NewCapit.dist.pages
{
    public partial class tabelasauxiliares : System.Web.UI.Page
    {
        SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString());
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregarTiposViagem();
            }
        }
        protected void btnNovo_Click(object sender, EventArgs e)
        {
            Response.Redirect("frm_cadastro_tipoviagem.aspx");
        }
        protected void gvTiposViagem_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "Editar")
            {
                Response.Redirect("frm_cadastro_tipoviagem.aspx?id=" + id);
            }

            if (e.CommandName == "Inativar")
            {
                using (SqlConnection conn = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
                {
                    string sql = @"UPDATE tbtiposdeviagem
                           SET status='INATIVO'
                           WHERE codigo=@id";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id", id);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                CarregarTiposViagem();
            }
        }
        private void CarregarTiposViagem()
        {
            using (SqlConnection conn = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                string sql = @"SELECT
                            codigo,
                            descricao,
                            abreviacao,
                            interplantas,
                            cnti,
                            codigo_recebedor_interplantas,
                            nome_recebedor_interplantas,
                            codigo_recebedor_cnti,
                            nome_recebedor_cnti,
                            status
                       FROM tbtiposdeviagem
                       ORDER BY descricao";

                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvTiposViagem.DataSource = dt;
                gvTiposViagem.DataBind();
            }
        }
    }
}