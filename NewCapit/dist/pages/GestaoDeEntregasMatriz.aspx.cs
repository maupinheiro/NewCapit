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
        string conn = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // CarregarGrid();

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

                bool ocultar = false;
                if (bool.TryParse(hfOcultarViagens.Value, out ocultar))
                {
                    // Chame sua função de filtrar ou carregar dados aqui
                    FiltrarViagens(ocultar);
                }

                CarregarColetas();
                CarregarGridBarraPesquisa();
            }


        }
        protected void btnPostbackOcultar_Click(object sender, EventArgs e)
        {
            bool ocultar = bool.Parse(hfOcultarViagens.Value);
            FiltrarViagens(ocultar);
        }
        protected void CarregarGrid()
        {

            //string conn = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
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
            bool ocultar = false;

            if (bool.TryParse(hfOcultarViagens.Value, out ocultar))
            {
                FiltrarViagens(ocultar);
            }
            else
            {
                // fallback de segurança
                CarregarColetas();
            }
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
            if (ocultar == false)
            {

                CarregarColetas();
            }
            else
            {
                CarregarColetasConcluidas();


            }
        }

        private void CarregarColetas()
        {
            var dados = DAL.ConEntrega.FetchDataTableEntregasMatriz(GetDataInicio(), GetDataFim());

            rptCarregamento.DataSource = dados;
            rptCarregamento.DataBind();

            ViewState["rptCarregamento"] = dados;
            lblMensagem.Text = string.Empty;
        }
        private void CarregarGridBarraPesquisa()
        {
            DataTable dados = DAL.ConEntrega
        .FetchDataTableEntregasMatriz(GetDataInicio(), GetDataFim());

            ViewState["rptCarregamento"] = dados;

            rptCarregamento.DataSource = dados;
            rptCarregamento.DataBind();

            lblMensagem.Text = string.Empty;
        }
        protected void txtPesquisar_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = ViewState["rptCarregamento"] as DataTable;
            if (dt == null) return;

            string filtro = txtPesquisar.Text.Trim().Replace("'", "''");

            if (string.IsNullOrWhiteSpace(filtro))
            {
                rptCarregamento.DataSource = dt;
                rptCarregamento.DataBind();
                return;
            }

            List<string> filtros = new List<string>();

            foreach (DataColumn col in dt.Columns)
            {
                filtros.Add(
                    $"CONVERT([{col.ColumnName}], 'System.String') LIKE '%{filtro}%'"
                );
            }

            DataView dv = new DataView(dt);
            dv.RowFilter = string.Join(" OR ", filtros);

            rptCarregamento.DataSource = dv;
            rptCarregamento.DataBind();
            lblMensagem.Text = "Pesquisa retornou (" + dv.Count + ") registro(s).";
        }


        private DateTime? GetDataInicio()
        {
            return DateTime.TryParse(DataInicio.Text, out var d) ? d : (DateTime?)null;
        }
        private DateTime? GetDataFim()
        {
            return DateTime.TryParse(DataFim.Text, out var d) ? d : (DateTime?)null;
        }
        private void CarregarColetasConcluidas()
        {
            var dados = DAL.ConEntrega.FetchDataTableEntregasMatrizConcluida(
                GetDataInicio(),
                GetDataFim()

            );

            rptCarregamento.DataSource = dados;
            rptCarregamento.DataBind();

            ViewState["rptCarregamento"] = dados;
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


                    Label lblStatus = e.Item.FindControl("lblStatus") as Label;
                    if (lblStatus == null) return;

                    string status = lblStatus.Text.Trim();
                    switch (status)
                    {
                        case "Pronto":
                            lblStatus.CssClass += " bg-warning text-white";
                            break;

                        case "Em Transito":
                            lblStatus.CssClass += " bg-success text-white";
                            break;

                        case "Ag. Descarga":
                            lblStatus.CssClass += " bg-danger text-white";
                            break;

                        case "Ag. Carregamento":
                            lblStatus.CssClass += " bg-red text-white";
                            break;

                        case "Ag. Documentos":
                            lblStatus.CssClass += " bg-yellow text-dark";
                            break;

                        case "Carregando":
                            lblStatus.CssClass += " bg-purple text-white";
                            break;
                        case "Pendente":
                            lblStatus.CssClass += " bg-black text-white";
                            break;
                        case "Concluido":
                            lblStatus.CssClass += " bg-info text-white";
                            break;
                    }
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

        [System.Web.Services.WebMethod]
        public static object BuscarDocumento(string numeroDocumento)
        {      
            using (SqlConnection conn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                conn.Open();

                string sqlCte = @"
            SELECT chave_de_acesso, emissao_documento, empresa_emissora, idviagem
            FROM tbcte
            WHERE num_documento = @numero";

                SqlCommand cmd = new SqlCommand(sqlCte, conn);
                cmd.Parameters.AddWithValue("@numero", numeroDocumento);

                SqlDataReader dr = cmd.ExecuteReader();

                if (!dr.Read())
                    return new { encontrado = false };

                string idViagem = dr["idviagem"].ToString();

                var resultado = new
                {
                    encontrado = true,
                    chave = dr["chave_de_acesso"].ToString(),
                    emissao = Convert.ToDateTime(dr["emissao_documento"]).ToString("dd/MM/yyyy"),
                    empresa = dr["empresa_emissora"].ToString(),
                    motorista = "",
                    destino = "",
                    cidade = "",
                    uf = "",
                    dataSaida = ""
                };

                dr.Close();

                string sqlCar = @"
            SELECT nomemotorista, recebedpr. cod_recebedor, uf_recebedor, dtchegada
            FROM tbcarregamentos
            WHERE num_carregamento = @num";

                SqlCommand cmdCar = new SqlCommand(sqlCar, conn);
                cmdCar.Parameters.AddWithValue("@num", idViagem);

                SqlDataReader dr2 = cmdCar.ExecuteReader();

                if (dr2.Read())
                {
                    resultado = new
                    {
                        encontrado = true,
                        chave = resultado.chave,
                        emissao = resultado.emissao,
                        empresa = resultado.empresa,
                        motorista = dr2["nomemotorista"].ToString(),
                        destino = dr2["recebedor"].ToString(),
                        cidade = dr2["cid_recebedor"].ToString(),
                        uf = dr2["uf_recebedor"].ToString(),
                        dataSaida = Convert.ToDateTime(dr2["dtchegada"])
                                        .ToString("dd/MM/yyyy HH:mm")
                    };
                }

                return resultado;
            }
        }

        protected void btnSalvarBaixa_Click(object sender, EventArgs e)
        {
            
            string usuario = Session["UsuarioLogado"].ToString();
            using (SqlConnection conn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                conn.Open();

                string sql = @"
            UPDATE tbcte
            SET baixado_por = @usuario,
                status_documento = @status_documento,
                data_baixa = GETDATE()
            WHERE numero_documento = @numero";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@usuario", usuario);
                cmd.Parameters.AddWithValue("@numero", Request.Form["txtNumeroDocumento"]);
                cmd.Parameters.AddWithValue("@status_documento", "Baixado");

                cmd.ExecuteNonQuery();
            }

            ScriptManager.RegisterStartupScript(this, GetType(),
                "ok", "alert('CTe baixado com sucesso!');", true);
        }
        protected void btnBaixar_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(
                this,
                GetType(),
                "abrirModal",
                "var m = new bootstrap.Modal(document.getElementById('modalCTE')); m.show();",
                true
            );
        }
    }
}