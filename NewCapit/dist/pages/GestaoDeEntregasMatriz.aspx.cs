using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using System.IO;
using System.Collections;
using OfficeOpenXml; // Namespace da EPPlus
using OfficeOpenXml.Style;
using ClosedXML.Excel;

namespace NewCapit.dist.pages
{
    public partial class GestaoDeEntregasMatriz : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregarGrid();

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
        protected void CarregarGrid()
        {

            string connStr = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT Id, carga, emissao, peso, status, CONVERT(varchar, previsao, 103) AS previsao, cliorigem, cidorigem, clidestino, ciddestino FROM tbcargas where empresa = '1111' ";

                if (!string.IsNullOrEmpty(DataInicio.Text))
                    query += " AND previsao >= @DataInicio";

                if (!string.IsNullOrEmpty(DataFim.Text))
                    query += " AND previsao <= @DataFim";

                if (!string.IsNullOrEmpty(ddlStatus.SelectedValue))
                    query += " AND status = @Status";

                SqlCommand cmd = new SqlCommand(query, conn);

                if (!string.IsNullOrEmpty(DataInicio.Text))
                    cmd.Parameters.AddWithValue("@DataInicio", DateTime.Parse(DataInicio.Text));

                if (!string.IsNullOrEmpty(DataFim.Text))
                    cmd.Parameters.AddWithValue("@DataFim", DateTime.Parse(DataFim.Text));

                if (!string.IsNullOrEmpty(ddlStatus.SelectedValue))
                    cmd.Parameters.AddWithValue("@Status", ddlStatus.SelectedValue);
                query += @"
            GROUP BY CONVERT(VARCHAR(10), previsao, 103)
            ORDER BY CONVERT(DATE, previsao) ";


                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvCargas.DataSource = dt;
                gvCargas.DataBind();

                // Armazena os dados no ViewState para usar na exportação
                ViewState["Cargas"] = dt;
            }
        }
        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            CarregarGrid();
        }

        protected void btnExportarExcel_Click(object sender, EventArgs e)
        {
            DataTable dt = ViewState["Cargas"] as DataTable;
            if (dt == null) return;

            using (XLWorkbook wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add("Cargas");
                ws.Cell(1, 1).InsertTable(dt);
                ws.Columns().AdjustToContents();

                using (MemoryStream ms = new MemoryStream())
                {
                    wb.SaveAs(ms);
                    Response.Clear();
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=Relatorio.xlsx");
                    ms.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
        }

        protected void Editar(object sender, EventArgs e)
        {
            using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
            {
                string id = gvCargas.DataKeys[row.RowIndex].Value.ToString();

                Response.Redirect("/dist/pages/Frm_AltCarga.aspx?id=" + id);
            }
        }



        protected void gvCargas_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            gvCargas.PageIndex = e.NewPageIndex;
            CarregarGrid();
        }

        protected void gvCargas_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            gvCargas.EditIndex = e.NewEditIndex;
            CarregarGrid();
        }

        protected void gvCargas_RowCancelingEdit(object sender, System.Web.UI.WebControls.GridViewCancelEditEventArgs e)
        {
            gvCargas.EditIndex = -1;
            CarregarGrid();
        }

        protected void gvCargas_RowUpdating(object sender, System.Web.UI.WebControls.GridViewUpdateEventArgs e)
        {
            //GridViewRow row = gvPedidos.Rows[e.RowIndex];
            //int id = Convert.ToInt32(gvPedidos.DataKeys[e.RowIndex].Value);
            //string cliente = ((TextBox)row.Cells[1].Controls[0]).Text;
            //string data = ((TextBox)row.Cells[2].Controls[0]).Text;
            //string valor = ((TextBox)row.Cells[3].Controls[0]).Text;

            //using (SqlConnection conn = new SqlConnection(connStr))
            //{
            //    string sql = "UPDATE Pedidos SET Cliente=@Cliente, DataPedido=@DataPedido, Valor=@Valor WHERE Id=@Id";
            //    SqlCommand cmd = new SqlCommand(sql, conn);
            //    cmd.Parameters.AddWithValue("@Cliente", cliente);
            //    cmd.Parameters.AddWithValue("@DataPedido", DateTime.Parse(data));
            //    cmd.Parameters.AddWithValue("@Valor", decimal.Parse(valor));
            //    cmd.Parameters.AddWithValue("@Id", id);

            //    conn.Open();
            //    cmd.ExecuteNonQuery();
            //}

            //gvPedidos.EditIndex = -1;
            //CarregarPedidos();
        }
    }
}