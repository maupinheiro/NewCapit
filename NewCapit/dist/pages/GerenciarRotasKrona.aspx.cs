using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Wordprocessing;

namespace NewCapit.dist.pages
{
    public partial class GerenciarRotasKrona : System.Web.UI.Page
    {
        string conn = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregarGrid();
            }
        }

        private void CarregarGrid(string filtro = "")
        {
            using (SqlConnection conn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                string sql = @"SELECT id_rota,
                                  descricao_rota,
                                  cod_expedidor_rota,
                                  expedidor_rota,
                                  cod_recebedor_rota,
                                  recebedor_rota
                           FROM tbrotaskrona
                           WHERE (@filtro = '' OR
                                  descricao_rota LIKE '%' + @filtro + '%' OR
                                  recebedor_rota LIKE '%' + @filtro + '%' OR
                                  cod_recebedor_rota LIKE '%' + @filtro + '%')
                           ORDER BY descricao_rota";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@filtro", filtro);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvRotas.DataSource = dt;
                gvRotas.DataBind();
            }
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            CarregarGrid(txtPesquisa.Text.Trim());
        }
        protected void Editar(object sender, EventArgs e)
        {
            using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
            {
                string id = gvRotas.DataKeys[row.RowIndex].Value.ToString();

                Response.Redirect("Frm_EditarRotaKrona.aspx?id=" + id);
            }
        }
        protected void gvRotas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvRotas.PageIndex = e.NewPageIndex;
            CarregarGrid();  // Método para recarregar os dados no GridView
        }
        protected void gvRotas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Remover")
            {
                int id = Convert.ToInt32(e.CommandArgument);

                using (SqlConnection conn = new SqlConnection(
               ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("DELETE FROM tbrotaskrona WHERE id_rota = @id", conn);
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }

                MostrarMsg("Rota removida com sucesso!", "success");

                CarregarGrid(); // Rebind da grid
            }
        }

        protected void MostrarMsg(string mensagem, string tipo = "warning")
        {
            divMsg.Attributes["class"] = "alert alert-" + tipo + " alert-dismissible fade show mt-3";
            lblMsgGeral.InnerText = mensagem;
            divMsg.Style["display"] = "block";
        }
        [System.Web.Services.WebMethod]
        public static object BuscarRota(int id)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["suaConexao"].ConnectionString))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM tbrotaskrona WHERE id_rota = @id", con);
                cmd.Parameters.AddWithValue("@id", id);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    return new
                    {
                        id_rota = dr["id_rota"].ToString(),
                        desc_rota = dr["descricao_rota"].ToString()
                    };
                }
            }

            return null;
        }
    }
}