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
using System.Web.UI.HtmlControls;

namespace NewCapit.dist.pages
{
    public partial class GestaoDeEntregasMatriz : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
               // CarregarGrid();

                if (Session["UsuarioLogado"] != null)
                {
                    CarregarColetas();
                    string nomeUsuario = Session["UsuarioLogado"].ToString();
                    var lblUsuario = nomeUsuario;
                }
                else
                {
                    var lblUsuario = "<Usuário>";

                    Response.Redirect("Login.aspx");
                }

                bool ocultar = false;
                if (bool.TryParse(hfOcultarViagens.Value, out ocultar))
                {
                    // Chame sua função de filtrar ou carregar dados aqui
                    FiltrarViagens(ocultar);
                }
            }


        }
        protected void btnPostbackOcultar_Click(object sender, EventArgs e)
        {
            bool ocultar = bool.Parse(hfOcultarViagens.Value);
            FiltrarViagens(ocultar);
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

                //if (!string.IsNullOrEmpty(ddlStatus.SelectedValue))
                //    query += " AND status = @Status";

                SqlCommand cmd = new SqlCommand(query, conn);

                if (!string.IsNullOrEmpty(DataInicio.Text))
                    cmd.Parameters.AddWithValue("@DataInicio", DateTime.Parse(DataInicio.Text));

                if (!string.IsNullOrEmpty(DataFim.Text))
                    cmd.Parameters.AddWithValue("@DataFim", DateTime.Parse(DataFim.Text));

                //if (!string.IsNullOrEmpty(ddlStatus.SelectedValue))
                //    cmd.Parameters.AddWithValue("@Status", ddlStatus.SelectedValue);
                query += @"
            GROUP BY CONVERT(VARCHAR(10), previsao, 103)
            ORDER BY CONVERT(DATE, previsao) ";


                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                //gvCargas.DataSource = dt;
                //gvCargas.DataBind();

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
            //using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
            //{
            //    string id = gvCargas.DataKeys[row.RowIndex].Value.ToString();

            //    Response.Redirect("/dist/pages/Frm_AltCarga.aspx?id=" + id);
            //}
        }

        protected void gvCargas_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            //gvCargas.PageIndex = e.NewPageIndex;
            //CarregarGrid();
        }

        protected void gvCargas_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            //gvCargas.EditIndex = e.NewEditIndex;
            //CarregarGrid();
        }

        protected void gvCargas_RowCancelingEdit(object sender, System.Web.UI.WebControls.GridViewCancelEditEventArgs e)
        {
            //gvCargas.EditIndex = -1;
            //CarregarGrid();
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

        public void FiltrarViagens(bool ocultar)
        {
            if (ocultar == true)
            {
                CarregarColetasConcluidas();
            }
            else
            {
                CarregarColetas();
            }
        }

        private void CarregarColetas()
        {
            var novosDados = DAL.ConEntrega.FetchDataTableEntregasMatriz();

            rptCarregamento.DataSource = novosDados;
            rptCarregamento.DataBind();

            // Armazena no ViewState, se necessário para outras operações
            ViewState["rptCarregamento"] = novosDados;

            lblMensagem.Text = string.Empty;
        }

        private void CarregarColetasConcluidas()
        {
            var novosDados = DAL.ConEntrega.FetchDataTableEntregasMatrizConcluida();

            rptCarregamento.DataSource = novosDados;
            rptCarregamento.DataBind();

            // Armazena no ViewState, se necessário para outras operações
            ViewState["rptCarregamento"] = novosDados;

            lblMensagem.Text = string.Empty;
        }

        protected void lnkEditar_Command(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "Editar")
            {
                string numCarregamento = e.CommandArgument.ToString();
                string url = $"Frm_AtualizaColetaMatriz.aspx?carregamento={numCarregamento}";
                Response.Redirect(url);
            }
        }
        protected void rptCarregamento_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                // Pega o valor da carga ou do idviagem do item atual
                string idviagem = DataBinder.Eval(e.Item.DataItem, "num_carregamento").ToString();

                // Pega o repeater interno
                Repeater rptColeta = (Repeater)e.Item.FindControl("rptColeta");

                if (rptColeta != null && !string.IsNullOrEmpty(idviagem))
                {
                    // Busca os dados de coletas relacionadas àquele CVA
                    DataTable dtColetas = DAL.ConCargas.FetchDataTableColetas3(idviagem);

                    // Bind dos dados ao repeater interno
                    rptColeta.DataSource = dtColetas;
                    rptColeta.DataBind();
                }
            }
        }
        protected void rptColeta_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            string previsaoStr = DataBinder.Eval(e.Item.DataItem, "previsao")?.ToString();
            string dataHoraStr = DataBinder.Eval(e.Item.DataItem, "data_hora")?.ToString();
            string status = DataBinder.Eval(e.Item.DataItem, "status")?.ToString();

            Label lblAtendimento = (Label)e.Item.FindControl("lblAtendimento");
            HtmlTableCell tdAtendimento = (HtmlTableCell)e.Item.FindControl("tdAtendimento");

            if (lblAtendimento != null && tdAtendimento != null)
            {
                DateTime previsao, dataHora;
                DateTime agora = DateTime.Now;

                if (DateTime.TryParse(previsaoStr, out previsao) && DateTime.TryParse(dataHoraStr, out dataHora))
                {
                    DateTime dataPrevisao = previsao.Date;
                    DateTime dataHoraComparacao = new DateTime(
                        dataPrevisao.Year, dataPrevisao.Month, dataPrevisao.Day,
                        dataHora.Hour, dataHora.Minute, dataHora.Second
                    );

                    if (dataHoraComparacao < agora && (status == "Concluído" || status == "Pendente"))
                    {
                        lblAtendimento.Text = "Atrasado";
                        tdAtendimento.BgColor = "Red";
                        tdAtendimento.Attributes["style"] = "color: white; font-weight: bold;";
                    }
                    else if (dataHoraComparacao.Date == agora.Date && dataHoraComparacao.TimeOfDay <= agora.TimeOfDay
                             && (status == "Concluído" || status == "Pendente"))
                    {
                        lblAtendimento.Text = "No Prazo";
                        tdAtendimento.BgColor = "Green";
                        tdAtendimento.Attributes["style"] = "color: white; font-weight: bold;";
                    }
                    else if (dataHoraComparacao > agora && status == "Concluído")
                    {
                        lblAtendimento.Text = "Antecipado";
                        tdAtendimento.BgColor = "Orange";
                        tdAtendimento.Attributes["style"] = "color: white; font-weight: bold;";
                    }
                    else
                    {
                        lblAtendimento.Text = status;

                    }
                }
            }
        }
    }
}