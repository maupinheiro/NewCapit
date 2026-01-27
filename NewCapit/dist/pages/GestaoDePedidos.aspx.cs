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
    public partial class GestaoDePedidos : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregarGridPedidos();

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
        protected void CarregarGridPedidos()
        {

            string connStr = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT Id, pedido, solicitante, carga, CONVERT(varchar, previsao, 103) AS previsao, cliorigem, clidestino, andamento, chegada, idviagem FROM tbpedidos";

                if (!string.IsNullOrEmpty(DataInicio.Text))
                    query += " AND previsao >= @DataInicio";

                if (!string.IsNullOrEmpty(DataFim.Text))
                    query += " AND previsao <= @DataFim";

                if (!string.IsNullOrEmpty(ddlStatus.SelectedValue))
                    query += " AND andamento = @Status";

                SqlCommand cmd = new SqlCommand(query, conn);

                if (!string.IsNullOrEmpty(DataInicio.Text))
                    cmd.Parameters.AddWithValue("@DataInicio", DateTime.Parse(DataInicio.Text));

                if (!string.IsNullOrEmpty(DataFim.Text))
                    cmd.Parameters.AddWithValue("@DataFim", DateTime.Parse(DataFim.Text));

                if (!string.IsNullOrEmpty(ddlStatus.SelectedValue))
                    cmd.Parameters.AddWithValue("@Status", ddlStatus.SelectedValue);
                query += @"
                ORDER BY emissao DESC) ";


                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvPedidos.DataSource = dt;
                gvPedidos.DataBind();

                // Armazena os dados no ViewState para usar na exportação
                ViewState["Pedidos"] = dt;
            }
        }
        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            CarregarGridPedidos();
        }


        protected void btnExportarExcel_Click(object sender, EventArgs e)
        {
            DataTable dt = ViewState["Pedidos"] as DataTable;
            if (dt == null) return;

            using (XLWorkbook wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add("Pedidos");
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
                string id = gvPedidos.DataKeys[row.RowIndex].Value.ToString();

                Response.Redirect("/dist/pages/Frm_AltCarga.aspx?id=" + id);
            }
        }




        protected void gvPedidos_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            gvPedidos.PageIndex = e.NewPageIndex;
            CarregarGridPedidos();
        }

        protected void gvPedidos_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            gvPedidos.EditIndex = e.NewEditIndex;
            CarregarGridPedidos();
        }

        protected void gvPedidos_RowCancelingEdit(object sender, System.Web.UI.WebControls.GridViewCancelEditEventArgs e)
        {
            gvPedidos.EditIndex = -1;
            CarregarGridPedidos();
        }

        protected void gvPedidos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Oc")
            {
                string numCarregamento = e.CommandArgument.ToString();
                string url = $"Frm_AtualizaColetaMatriz.aspx?carregamento={numCarregamento}";
                Response.Redirect(url);

            }
        }
    }
}