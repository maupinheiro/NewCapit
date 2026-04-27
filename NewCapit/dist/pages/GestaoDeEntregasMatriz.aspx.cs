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
using ClosedXML.Excel;
using System.Web.UI.HtmlControls;
using System.Web.Configuration;
using MathNet.Numerics;
using NPOI.SS.Formula.Functions;
using System.Security.Cryptography;
using System.Drawing;
using System.Globalization;
using ICSharpCode.SharpZipLib.Zip;

namespace NewCapit.dist.pages
{
    public partial class GestaoDeEntregasMatriz : System.Web.UI.Page
    {
        string conn = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
        string idViagem;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UsuarioLogado"] != null)
                {
                    string nomeUsuario = Session["UsuarioLogado"].ToString();
                }
                else
                {
                    Response.Redirect("Login.aspx");
                    return;
                }

                //CarregarCarregamentos();
                DataInicio.Text = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
                DataFim.Text = DateTime.Now.ToString("yyyy-MM-dd");

                DateTime dataInicio = string.IsNullOrWhiteSpace(DataInicio.Text)
                ? DateTime.Now.AddDays(-7)
                : DateTime.ParseExact(DataInicio.Text, "yyyy-MM-dd", null);

                DateTime dataFim = string.IsNullOrWhiteSpace(DataFim.Text)
                    ? DateTime.Now
                    : DateTime.ParseExact(DataFim.Text, "yyyy-MM-dd", null);

                CarregarGrid();
            }
        }
        protected void FiltroPeriodo_TextChanged(object sender, EventArgs e)
        {
            CarregarGrid();
        }
        protected void chkOcultarConcluidos_CheckedChanged(object sender, EventArgs e)
        {
            ViewState["Pagina"] = 1; // opcional (volta pra primeira página)
            CarregarGrid();
        }
        private void CarregarGrid()
        {
            int pageSize = 35;
            int paginaAtual = ViewState["Pagina"] != null ? (int)ViewState["Pagina"] : 1;

            string frota = txtFrota.Text?.Trim();
            string placa = txtPlaca.Text?.Trim();
            string motorista = txtMotorista.Text?.Trim();
            string expedidor = txtExpedidor.Text?.Trim();
            string recebedor = txtRecebedor.Text?.Trim();
            string status = txtStatus.Text?.Trim();

            // ✅ DATAS
            string[] formatos = { "dd/MM/yyyy", "yyyy-MM-dd" };

            DateTime dataInicio, dataFim;

            if (!DateTime.TryParseExact(DataInicio.Text, formatos, CultureInfo.InvariantCulture, DateTimeStyles.None, out dataInicio))
            {
                dataInicio = DateTime.Now.AddDays(-7);
            }

            if (!DateTime.TryParseExact(DataFim.Text, formatos, CultureInfo.InvariantCulture, DateTimeStyles.None, out dataFim))
            {
                dataFim = DateTime.Now;
            }

            string sqlFiltro = @"
            WHERE empresa = '1111'
            AND emissao >= @ini
            AND emissao < DATEADD(DAY,1,@fim)";

                    List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("@ini", dataInicio),
                new SqlParameter("@fim", dataFim)
            };

            // ✅ INTERRUPTOR (OCULTAR CONCLUÍDAS)
            if (chkOcultarConcluidos.Checked)
            {
                sqlFiltro += " AND situacao <> 'VIAGEM CONCLUIDA'";
            }

            // 🔎 FILTROS
            if (!string.IsNullOrEmpty(frota))
            {
                sqlFiltro += " AND (tipoveiculo COLLATE Latin1_General_CI_AI LIKE @veiculo OR veiculo COLLATE Latin1_General_CI_AI LIKE @veiculo)";
                parametros.Add(new SqlParameter("@veiculo", "%" + frota + "%"));
            }

            if (!string.IsNullOrEmpty(placa))
            {
                sqlFiltro += " AND (placa LIKE @placa OR reboque1 LIKE @placa OR reboque2 LIKE @placa)";
                parametros.Add(new SqlParameter("@placa", "%" + placa + "%"));
            }

            if (!string.IsNullOrEmpty(motorista))
            {
                sqlFiltro += " AND (codmotorista LIKE @motorista OR nomemotorista COLLATE Latin1_General_CI_AI LIKE @motorista)";
                parametros.Add(new SqlParameter("@motorista", "%" + motorista + "%"));
            }

            if (!string.IsNullOrEmpty(expedidor))
            {
                sqlFiltro += " AND (cod_expedidor LIKE @expedidor OR expedidor COLLATE Latin1_General_CI_AI LIKE @expedidor)";
                parametros.Add(new SqlParameter("@expedidor", "%" + expedidor + "%"));
            }

            if (!string.IsNullOrEmpty(recebedor))
            {
                sqlFiltro += " AND (cod_recebedor LIKE @recebedor OR recebedor COLLATE Latin1_General_CI_AI LIKE @recebedor)";
                parametros.Add(new SqlParameter("@recebedor", "%" + recebedor + "%"));
            }

            if (!string.IsNullOrEmpty(status))
            {
                sqlFiltro += " AND (status LIKE @status OR situacao LIKE @status)";
                parametros.Add(new SqlParameter("@status", "%" + status + "%"));
            }

            string conn = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;

            using (SqlConnection con = new SqlConnection(conn))
            {
                con.Open();

            // 🔢 TOTAL
            SqlCommand cmdTotal = new SqlCommand($@"
            SELECT COUNT(*) 
            FROM tbcarregamentos
            {sqlFiltro}", con);

                foreach (var p in parametros)
                    cmdTotal.Parameters.AddWithValue(p.ParameterName, p.Value);

                int totalRegistros = (int)cmdTotal.ExecuteScalar();
                int totalPaginas = (int)Math.Ceiling((double)totalRegistros / pageSize);

                lblTotalGeral.InnerText = $"Página {paginaAtual} de {totalPaginas} | Total: {totalRegistros}";
                lblPaginaAtual.Text = paginaAtual.ToString().Trim();
                lblTotalPaginas.Text = totalPaginas.ToString().Trim();                    
                ViewState["TotalPaginas"] = totalPaginas;

                // 📋 GRID
                string sql = $@"
                WITH Dados AS(
                    SELECT *,
                    ROW_NUMBER() OVER(ORDER BY veiculo ASC) AS RowNum
                    FROM tbcarregamentos
                    {sqlFiltro}
                )
                SELECT *
                FROM Dados
                WHERE RowNum BETWEEN ((@pagina - 1) * @pageSize + 1)
                         AND (@pagina * @pageSize)";

                SqlCommand cmd = new SqlCommand(sql, con);

                foreach (var p in parametros)
                    cmd.Parameters.AddWithValue(p.ParameterName, p.Value);

                cmd.Parameters.AddWithValue("@pagina", paginaAtual);
                cmd.Parameters.AddWithValue("@pageSize", pageSize);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvOrdens.DataSource = dt;
                gvOrdens.DataBind();

                // 📊 KPI
                SqlCommand cmdStatus = new SqlCommand($@"
                SELECT status, COUNT(*) total
                FROM tbcarregamentos
                {sqlFiltro}
                GROUP BY status", con);

                foreach (var p in parametros)
                    cmdStatus.Parameters.AddWithValue(p.ParameterName, p.Value);

                SqlDataReader dr = cmdStatus.ExecuteReader();

                string html = "";
                Dictionary<string, int> dadosGrafico = new Dictionary<string, int>();

                while (dr.Read())
                {
                    string st = dr["status"].ToString();
                    int total = Convert.ToInt32(dr["total"]);

                    html += $"<span class='badge bg-primary me-1'>{st}: {total}</span>";
                    dadosGrafico[st] = total;
                }
                dr.Close();

                string labels = string.Join(",", dadosGrafico.Keys.Select(x => $"'{x}'"));
                string valores = string.Join(",", dadosGrafico.Values);

                ScriptManager.RegisterStartupScript(this, GetType(), "kpi", $@"
                    renderKPI([{labels}], [{valores}]);
                ", true);
            }
        }
        protected void btnIrPagina_Click(object sender, EventArgs e)
        {
            int paginaDigitada;

            if (int.TryParse(txtIrPagina.Text, out paginaDigitada))
            {
                int totalPaginas = ViewState["TotalPaginas"] != null ? (int)ViewState["TotalPaginas"] : 1;

                // 🔒 limita entre 1 e totalPaginas
                if (paginaDigitada < 1)
                    paginaDigitada = 1;

                if (paginaDigitada > totalPaginas)
                    paginaDigitada = totalPaginas;

                ViewState["Pagina"] = paginaDigitada;
                CarregarGrid();
            }
        }
        //protected void btnIrPagina_Click(object sender, EventArgs e)
        //{
        //    GridViewRow pagerRow = gvOrdens.BottomPagerRow;

        //    if (pagerRow != null)
        //    {
        //        TextBox txtIr = (TextBox)pagerRow.FindControl("txtIrPagina");

        //        int pagina;
        //        if (int.TryParse(txtIr.Text, out pagina))
        //        {
        //            if (pagina > 0 && pagina <= gvOrdens.PageCount)
        //            {
        //                gvOrdens.PageIndex = pagina - 1;
        //                CarregarGrid();
        //            }
        //        }
        //    }
        //}
        protected void gvOrdens_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvOrdens.PageIndex = e.NewPageIndex;
            CarregarGrid();
        }
        protected void btnPrimeiro_Click(object sender, EventArgs e)
        {
            ViewState["Pagina"] = 1;
            CarregarGrid();
        }
        protected void btnAnterior_Click(object sender, EventArgs e)
        {
            int pagina = (int)ViewState["Pagina"];
            if (pagina > 1)
                ViewState["Pagina"] = pagina - 1;

            CarregarGrid();
        }
        protected void btnProximo_Click(object sender, EventArgs e)
        {
            int pagina = (int)ViewState["Pagina"];
            int total = (int)ViewState["TotalPaginas"];

            if (pagina < total)
                ViewState["Pagina"] = pagina + 1;

            CarregarGrid();
        }
        protected void btnUltimo_Click(object sender, EventArgs e)
        {
            ViewState["Pagina"] = (int)ViewState["TotalPaginas"];
            CarregarGrid();
        }
        protected void gvOrdens_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Pager)
            {
                int paginaAtual = ViewState["Pagina"] != null ? (int)ViewState["Pagina"] : 1;
                int totalPaginas = ViewState["TotalPaginas"] != null ? (int)ViewState["TotalPaginas"] : 1;

                // 🔢 LABELS
                Label lblPaginaAtual = (Label)e.Row.FindControl("lblPaginaAtual");
                Label lblTotalPaginas = (Label)e.Row.FindControl("lblTotalPaginas");

                if (lblPaginaAtual != null)
                    lblPaginaAtual.Text = paginaAtual.ToString();

                if (lblTotalPaginas != null)
                    lblTotalPaginas.Text = totalPaginas.ToString();

                // 🔒 BOTÕES (habilita/desabilita)
                LinkButton btnFirst = (LinkButton)e.Row.FindControl("btnPrimeiro");
                LinkButton btnPrev = (LinkButton)e.Row.FindControl("btnAnterior");
                LinkButton btnNext = (LinkButton)e.Row.FindControl("btnProximo");
                LinkButton btnLast = (LinkButton)e.Row.FindControl("btnUltimo");

                if (btnFirst != null) btnFirst.Enabled = paginaAtual > 1;
                if (btnPrev != null) btnPrev.Enabled = paginaAtual > 1;
                if (btnNext != null) btnNext.Enabled = paginaAtual < totalPaginas;
                if (btnLast != null) btnLast.Enabled = paginaAtual < totalPaginas;
            }
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
        protected void Timer1_Tick(object sender, EventArgs e)
        {
            CarregarGrid(); // seu método
        }
        private DataTable BuscarDados()
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                string frota = txtFrota.Text?.Trim();
                string placa = txtPlaca.Text?.Trim();
                string motorista = txtMotorista.Text?.Trim();
                string expedidor = txtExpedidor.Text?.Trim();
                string recebedor = txtRecebedor.Text?.Trim();
                string status = txtStatus.Text?.Trim();

                string[] formatos = { "dd/MM/yyyy", "yyyy-MM-dd" };

                DateTime dataInicio, dataFim;

                if (!DateTime.TryParseExact(DataInicio.Text, formatos, CultureInfo.InvariantCulture, DateTimeStyles.None, out dataInicio))
                    dataInicio = DateTime.Now.AddDays(-7);

                if (!DateTime.TryParseExact(DataFim.Text, formatos, CultureInfo.InvariantCulture, DateTimeStyles.None, out dataFim))
                    dataFim = DateTime.Now;

                string sql = @" 
                SELECT 
                    veiculo, tipoveiculo, placa, reboque1, reboque2,
                    codmotorista, nomemotorista,
                    codtra, transportadora,
                    cod_expedidor, expedidor,
                    cod_recebedor, recebedor,
                    cid_expedidor, uf_expedidor,
                    cid_recebedor, uf_recebedor,
                    num_carregamento, carga, emissao, situacao, status
                FROM tbcarregamentos
                WHERE empresa = '1111'
                AND emissao >= @ini
                AND emissao < DATEADD(DAY,1,@fim)";

                List<SqlParameter> parametros = new List<SqlParameter>
                {
                    new SqlParameter("@ini", dataInicio),
                    new SqlParameter("@fim", dataFim)
                };

                if (chkOcultarConcluidos.Checked)
                    sql += " AND situacao <> 'VIAGEM CONCLUIDA'";

                if (!string.IsNullOrEmpty(frota))
                {
                    sql += " AND (tipoveiculo LIKE @veiculo OR veiculo LIKE @veiculo)";
                    parametros.Add(new SqlParameter("@veiculo", "%" + frota + "%"));
                }

                if (!string.IsNullOrEmpty(placa))
                {
                    sql += " AND (placa LIKE @placa OR reboque1 LIKE @placa OR reboque2 LIKE @placa)";
                    parametros.Add(new SqlParameter("@placa", "%" + placa + "%"));
                }

                if (!string.IsNullOrEmpty(motorista))
                {
                    sql += " AND (codmotorista LIKE @motorista OR nomemotorista LIKE @motorista)";
                    parametros.Add(new SqlParameter("@motorista", "%" + motorista + "%"));
                }

                if (!string.IsNullOrEmpty(expedidor))
                {
                    sql += " AND (cod_expedidor LIKE @expedidor OR expedidor LIKE @expedidor)";
                    parametros.Add(new SqlParameter("@expedidor", "%" + expedidor + "%"));
                }

                if (!string.IsNullOrEmpty(recebedor))
                {
                    sql += " AND (cod_recebedor LIKE @recebedor OR recebedor LIKE @recebedor)";
                    parametros.Add(new SqlParameter("@recebedor", "%" + recebedor + "%"));
                }

                if (!string.IsNullOrEmpty(status))
                {
                    sql += " AND (status LIKE @status OR situacao LIKE @status)";
                    parametros.Add(new SqlParameter("@status", "%" + status + "%"));
                }

                SqlCommand cmd = new SqlCommand(sql, conn);

                foreach (var p in parametros)
                {
                    cmd.Parameters.Add(p);
                }

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }

            return dt;
        }
        public override void VerifyRenderingInServerForm(System.Web.UI.Control control)
        {
            // Necessário para exportação do GridView
        }       
        protected void gvOrdens_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // 🔥 pega direto do banco
                string situacaoTexto = DataBinder.Eval(e.Row.DataItem, "situacao")
                    .ToString()
                    .Trim()
                    .ToUpper();

                // 👉 atributo usado pelo JavaScript
                e.Row.Attributes["data-situacao"] = situacaoTexto;

                Label lblStatus = (Label)e.Row.FindControl("lblStatus");

                if (lblStatus != null)
                {
                    // 👉 PEGA O STATUS DO BANCO
                    string statusTexto = DataBinder.Eval(e.Row.DataItem, "status").ToString().Trim();

                    // 👉 SE FOR INT (ENUM), usa isso:
                    int status;
                    if (int.TryParse(statusTexto, out status))
                    {
                        // lblStatus.Text = StatusHelper.GetDescricao(status);
                        //lblStatus.CssClass = StatusHelper.GetCss(status);
                    }
                    else
                    {
                        // 👉 SE FOR STRING (ex: "ABERTA")
                        lblStatus.Text = statusTexto;

                        switch (statusTexto)
                        {
                            case "Pronto":
                                lblStatus.CssClass = "badge bg-warning text-white";
                                break;
                            case "Em Transito":
                                lblStatus.CssClass = "badge bg-success text-white";
                                break;
                            case "Ag. Descarga":
                                lblStatus.CssClass = "badge bg-danger text-white";
                                break;
                            case "Ag. Carregamento":
                                lblStatus.CssClass = "badge bg-pink text-black";
                                break;
                            case "Ag. Documentos":
                                lblStatus.CssClass = "badge bg-yellow text-dark";
                                break;
                            case "Carregando":
                                lblStatus.CssClass = "badge bg-purple text-white";
                                break;
                            case "Pendente":
                                lblStatus.CssClass = "badge bg-black text-white";
                                break;
                            case "Pernoite":
                                lblStatus.CssClass = "badge bg-purple text-white";
                                break;
                            case "Concluido":
                                lblStatus.CssClass = "badge bg-info text-white";
                                break;
                            case "Liberado Vazio":
                                lblStatus.CssClass = "badge bg-info text-pink";
                                break;
                            case "Veic. Quebrado":
                                lblStatus.CssClass = "badge bg-success text-purple";
                                break;
                            case "Cancelada":
                                lblStatus.CssClass = "badge bg-warning text-purple";
                                break;
                            default:
                                lblStatus.CssClass = "badge bg-secondary text-white";
                                break;
                        }
                    }
                }
            }
        }
        protected void gvOrdens_DataBound(object sender, EventArgs e)
        {
            GridViewRow pagerRow = gvOrdens.BottomPagerRow;

            if (pagerRow != null)
            {
                Label lblAtual = (Label)pagerRow.FindControl("lblPaginaAtual");
                Label lblTotal = (Label)pagerRow.FindControl("lblTotalPaginas");

                if (lblAtual != null)
                    lblAtual.Text = (gvOrdens.PageIndex + 1).ToString();

                if (lblTotal != null)
                    lblTotal.Text = gvOrdens.PageCount.ToString();
            }
        }
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                string nomeArquivo = "Relatorio_" + DateTime.Now.ToString("ddMMyyyy_HHmm") + ".xls";

                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=" + nomeArquivo);
                Response.Charset = "";
                Response.ContentType = "application/vnd.ms-excel";

                DataTable dt = BuscarDados();

                GridView gv = new GridView();
                gv.DataSource = dt;
                gv.DataBind();

                // 🔽 Opcional: remover colunas
                // gv.Columns.RemoveAt(0);

                using (StringWriter sw = new StringWriter())
                {
                    HtmlTextWriter hw = new HtmlTextWriter(sw);

                    gv.HeaderRow.Style.Add("background-color", "#2F75B5");
                    gv.HeaderRow.Style.Add("color", "white");

                    gv.RenderControl(hw);

                    Response.Write(sw.ToString());
                    Response.Flush();
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "erro",
                    $"alert('Erro ao exportar: {ex.Message.Replace("'", "")}');", true);
            }
        }

        [System.Web.Services.WebMethod]
        protected void btnSalvarBaixa_Click(object sender, EventArgs e)
        {
            if (gridRadiosCTe.Checked == true)
            {
                try
                {
                    // 1. Verificação de segurança para a Session
                    if (Session["UsuarioLogado"] == null)
                    {
                        Response.Redirect("Login.aspx");
                        return;
                    }

                    string usuario = Session["UsuarioLogado"].ToString();
                    string numeroDoc = txtNumeroDocumento.Text.Trim(); // .Trim() evita espaços vazios

                    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
                    {
                        string sql = @"
                UPDATE tbcte 
                SET baixado_por = @usuario, 
                    status_documento = @status_documento, 
                    data_baixa = GETDATE() 
                WHERE num_documento = @numero";

                        SqlCommand cmd = new SqlCommand(sql, conn);
                        cmd.Parameters.AddWithValue("@usuario", usuario);
                        cmd.Parameters.AddWithValue("@numero", numeroDoc);
                        cmd.Parameters.AddWithValue("@status_documento", "Baixado");

                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {


                            ScriptManager.RegisterStartupScript(this, GetType(), "ok", "alert('CTe " + numeroDoc + " baixado com sucesso!');", true);
                            Limpar();
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "erro", "alert('Erro: Documento não encontrado no banco.');", true);
                            Limpar();
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Remove quebras de linha e aspas simples/duplas para não quebrar o alert do JS
                    string mensagemErro = ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", " ");

                    string script = $"alert('Erro técnico: {mensagemErro}');";

                    ScriptManager.RegisterStartupScript(this, GetType(), "erro_catch", script, true);
                }
            }
            else if (gridRadiosNFSe.Checked == true)
            {
                try
                {
                    // 1. Verificação de segurança para a Session
                    if (Session["UsuarioLogado"] == null)
                    {
                        Response.Redirect("Login.aspx");
                        return;
                    }

                    string usuario = Session["UsuarioLogado"].ToString();
                    string numeroDoc = txtNumeroDocumento.Text.Trim(); // .Trim() evita espaços vazios

                    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
                    {
                        string sql = @"
                UPDATE tbnfse 
                SET baixado_por = @usuario, 
                    status_documento = @status_documento, 
                    data_baixa = GETDATE() 
                WHERE num_documento = @numero";

                        SqlCommand cmd = new SqlCommand(sql, conn);
                        cmd.Parameters.AddWithValue("@usuario", usuario);
                        cmd.Parameters.AddWithValue("@numero", numeroDoc);
                        cmd.Parameters.AddWithValue("@status_documento", "Baixado");

                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {

                            ScriptManager.RegisterStartupScript(this, GetType(), "ok", "alert('NFS-e " + numeroDoc + " baixado com sucesso!');", true);
                            Limpar();
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "erro", "alert('Erro: Documento não encontrado no banco.');", true);
                            Limpar();
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Remove quebras de linha e aspas simples/duplas para não quebrar o alert do JS
                    string mensagemErro = ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", " ");

                    string script = $"alert('Erro técnico: {mensagemErro}');";

                    ScriptManager.RegisterStartupScript(this, GetType(), "erro_catch", script, true);
                }
            }



        }
        public void Limpar()
        {
            txtNumeroDocumento.Text = string.Empty;
            lblChave.Text = string.Empty;
            lblCidade.Text = string.Empty;
            lblDataSaida.Text = string.Empty;
            lblDestino.Text = string.Empty;
            lblEmissao.Text = string.Empty;
            lblEmpresa.Text = string.Empty;
            lblMotorista.Text = string.Empty;

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
        private object SafeDateTimeValue(string input)
        {
            DateTime dt;
            if (DateTime.TryParse(input, out dt))
                return dt.ToString("yyyy-MM-dd HH:mm:ss");
            else
                return DBNull.Value;
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {

            if (gridRadiosCTe.Checked == true)
            {
                using (SqlConnection conn = new SqlConnection(
                   ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
                {
                    conn.Open();

                    string sqlCte = @"
            SELECT chave_de_acesso, emissao_documento, empresa_emissora, 
                   id_viagem, status_documento
            FROM tbcte
            WHERE num_documento = @numero";

                    SqlCommand cmd = new SqlCommand(sqlCte, conn);
                    cmd.Parameters.AddWithValue("@numero", txtNumeroDocumento.Text);

                    SqlDataReader dr = cmd.ExecuteReader();

                    // 🔍 NÃO encontrou registro
                    if (!dr.Read())
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(),
                            "ok", "alert('Não há documento cadastrado!');", true);
                        Limpar();
                        return;
                    }

                    // 📦 Encontrou, mas já está baixado
                    if (dr["status_documento"].ToString() == "Baixado")
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(),
                            "ok", "alert('Documento já foi baixado!');", true);
                        Limpar();
                        return;
                    }

                    // 📄 Encontrou e está Pendente → segue fluxo
                    idViagem = dr["id_viagem"].ToString();
                    lblChave.Text = dr["chave_de_acesso"].ToString();
                    SafeDateTimeValue(lblEmissao.Text = dr["emissao_documento"].ToString());
                    lblEmpresa.Text = dr["empresa_emissora"].ToString();

                    dr.Close();

                    string sqlCar = @"
            SELECT m.nommot, c.cid_recebedor, c.uf_recebedor, 
                   c.cheg_cliente, c.recebedor  
            FROM tbcargas AS c  
            INNER JOIN tbmotoristas AS m ON c.codmot = m.codmot 
            WHERE carga = @idviagem";

                    SqlCommand cmdCar = new SqlCommand(sqlCar, conn);
                    cmdCar.Parameters.AddWithValue("@idviagem", idViagem);

                    SqlDataReader dr2 = cmdCar.ExecuteReader();

                    if (dr2.Read())
                    {
                        lblMotorista.Text = dr2["nommot"].ToString();
                        lblDestino.Text = dr2["recebedor"].ToString();
                        lblCidade.Text = dr2["cid_recebedor"].ToString() + "/" + dr2["uf_recebedor"].ToString();
                        SafeDateTimeValue(lblDataSaida.Text = dr2["cheg_cliente"].ToString());
                    }

                    dr2.Close();
                }
            }
            else if (gridRadiosNFSe.Checked == true)
            {
                using (SqlConnection conn = new SqlConnection(
                   ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
                {
                    conn.Open();

                    string sqlCte = @"
                                SELECT num_documento, emissao_documento, 
                                       idviagem, status_documento
                                FROM tbnfse
                                WHERE num_documento =@numero";

                    SqlCommand cmd = new SqlCommand(sqlCte, conn);
                    cmd.Parameters.AddWithValue("@numero", txtNumeroDocumento.Text);

                    SqlDataReader dr = cmd.ExecuteReader();

                    // 🔍 NÃO encontrou registro
                    if (!dr.Read())
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(),
                            "ok", "alert('Não há documento cadastrado!');", true);
                        Limpar();
                        return;
                    }

                    // 📦 Encontrou, mas já está baixado
                    if (dr["status_documento"].ToString() == "Baixado")
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(),
                            "ok", "alert('Documento já foi baixado!');", true);
                        Limpar();
                        return;
                    }

                    // 📄 Encontrou e está Pendente → segue fluxo
                    idViagem = dr["idviagem"].ToString();
                    lblChave.Text = "N/A";
                    SafeDateTimeValue(lblEmissao.Text = dr["emissao_documento"].ToString());
                    lblEmpresa.Text = "N/A";

                    dr.Close();

                    string sqlCar = @"
                        SELECT m.nommot, c.cid_recebedor, c.uf_recebedor, 
                               c.cheg_cliente, c.recebedor  
                        FROM tbcargas AS c  
                        INNER JOIN tbmotoristas AS m ON c.codmot = m.codmot 
                        WHERE carga = @idviagem";

                    SqlCommand cmdCar = new SqlCommand(sqlCar, conn);
                    cmdCar.Parameters.AddWithValue("@idviagem", idViagem);

                    SqlDataReader dr2 = cmdCar.ExecuteReader();

                    if (dr2.Read())
                    {
                        lblMotorista.Text = dr2["nommot"].ToString();
                        lblDestino.Text = dr2["recebedor"].ToString();
                        lblCidade.Text = dr2["cid_recebedor"].ToString() + "/" + dr2["uf_recebedor"].ToString();
                        SafeDateTimeValue(lblDataSaida.Text = dr2["cheg_cliente"].ToString());
                    }

                    dr2.Close();
                }
            }

        }
        protected void btnAbrirMdfe_Click(object sender, EventArgs e)
        {
            ddlFiltroStatus.SelectedValue = "Pendente";
            CarregarMdfeFiltro();
            ReabrirModal();

        }
        protected void FiltroChanged(object sender, EventArgs e)
        {
            CarregarMdfeFiltro();
            ScriptManager.RegisterStartupScript(this, GetType(),
                "openModal", "$('#modalMdfe').modal('show');", true);
        }
        void CarregarMdfeFiltro()
        {
            string sql = @"
                    SELECT id, status, mdfe_uf, mdfe_empresa, mdfe_numero, mdfe_serie,
                           mdfe_situacao, cid_expedidor, uf_expedidor,
                           cid_recebedor, uf_recebedor, mdfe_dv,
                           mdfe_baixado, mdfe_data_baixa
                    FROM tbcargas
                    WHERE mdfe IS NOT NULL
                ";

            if (!string.IsNullOrEmpty(ddlFiltroStatus.SelectedValue))
                sql += " AND mdfe_situacao = @Status";
            if (!string.IsNullOrEmpty(txtPesquisarMDFe.Text))
            {
                // Ajustado: Coluna antes do LIKE e parênteses para isolar o OR
                sql += " AND (mdfe_numero = @Pesquisa OR mdfe_empresa = @Pesquisa OR clidestino LIKE @PesquisaLike or carga=@Pesquisa)";
            }

            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                if (!string.IsNullOrEmpty(ddlFiltroStatus.SelectedValue))
                    cmd.Parameters.AddWithValue("@Status", ddlFiltroStatus.SelectedValue);
                if (!string.IsNullOrEmpty(txtPesquisarMDFe.Text))
                {
                    cmd.Parameters.AddWithValue("@Pesquisa", txtPesquisarMDFe.Text);
                    cmd.Parameters.AddWithValue("@PesquisaLike", "%" + txtPesquisarMDFe.Text + "%");
                }
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvMdfe.DataSource = dt;
                gvMdfe.DataBind();
                //Response.Write(dt.Rows.Count);

            }
        }
        protected void btnBaixarMDFe_Click(object sender, EventArgs e)
        {

            string usuario = Session["UsuarioLogado"].ToString() ?? "Sistema";
            bool selecionouAlgum = false;

            // Percorre cada linha da Grid
            foreach (GridViewRow row in gvMdfe.Rows)
            {
                // Encontra o CheckBox dentro da linha pelo ID
                CheckBox chk = (CheckBox)row.FindControl("chkSelecionar");

                if (chk != null && chk.Checked)
                {
                    selecionouAlgum = true;

                    // Pega o ID da linha (definido no DataKeyNames="id" da GridView)
                    string idMdf = gvMdfe.DataKeys[row.RowIndex].Value.ToString();

                    // Se precisar de outro valor de uma coluna BoundField (ex: Número do MDF-e na coluna 3)
                    // string numero = row.Cells[3].Text;

                    AtualizarBaixaNoBanco(idMdf, usuario);
                }
            }

            if (selecionouAlgum)
            {
                // Recarrega a grid para refletir as mudanças
                CarregarMdfeFiltro(); // Chame sua função que preenche a gvMdfe

                //ScriptManager.RegisterStartupScript(this, this.GetType(), "FecharModal",
                //    "$('#modalMdfe').modal('hide'); alert('MDF-e(s) baixado(s) com sucesso!');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "FecharEDesbloquear",
                    "$('#modalMdfe').modal('hide'); $('body').removeClass('modal-open'); $('.modal-backdrop').remove();", true);
            }
            else
            {
                // Opcional: avisar que nada foi selecionado sem fechar o modal
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Aviso", "alert('Selecione ao menos um item!');", true);
            }
            ScriptManager.RegisterStartupScript(this, GetType(),
                "openModal", "$('#modalMdfe').modal('show');", true);
        }
        private void AtualizarBaixaNoBanco(string id, string usuario)
        {
            string connStr = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                // Importante: Filtramos pelo ID da linha selecionada
                string sql = @"UPDATE tbcargas 
                       SET mdfe_situacao = 'Baixado', 
                           mdfe_baixado = @usuario, 
                           mdfe_data_baixa = GETDATE() 
                       WHERE id = @id";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@usuario", usuario);
                cmd.Parameters.AddWithValue("@id", id);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        protected void btnCancelarMDFe_Click(object sender, EventArgs e)
        {

            ScriptManager.RegisterStartupScript(this, this.GetType(), "FecharModal",
                    "$('#modalMdfe').modal('hide');", true);
        }
        void ExecutarSql(string sql, string usuario)
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                if (usuario != null)
                    cmd.Parameters.AddWithValue("@usuario", usuario);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            CarregarMdfeFiltro();
            ScriptManager.RegisterStartupScript(this, GetType(),
                "openModal", "$('#modalMdfe').modal('show');", true);
        }
        void ReabrirModal()
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(),
                "modalMdfe",
                "var modal = new bootstrap.Modal(document.getElementById('modalMdfe')); modal.show();",
                true);
        }
        protected void gvMdfe_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "CancelarMDF")
            {
                // O CommandArgument contém o ID que passamos no Eval("id")
                string idMdf = e.CommandArgument.ToString();

                CancelarMDFNoBanco(idMdf);

                // Atualiza a Grid para mostrar que os dados sumiram/mudaram
                CarregarMdfeFiltro();

                // Alerta opcional via JS
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('MDF-e cancelado com sucesso!');", true);
            }
        }
        private void CancelarMDFNoBanco(string id)
        {
            string connStr = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                // SQL baseado no seu modelo, filtrando estritamente pelo ID da linha
                string sql = @"
            UPDATE tbcargas 
            SET mdfe = NULL, 
                mdfe_situacao = NULL, 
                mdfe_empresa = NULL, 
                mdfe_numero = NULL, 
                mdfe_serie = NULL, 
                mdfe_uf = NULL, 
                mdfe_dv = NULL, 
                mdfe_baixado = NULL, 
                mdfe_data_baixa = NULL 
            WHERE id = @id";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        protected string GetSituacaoMDFE(object obj)
        {
            string situacao = (obj == null || obj == DBNull.Value)
                ? "Pendente"
                : obj.ToString().Trim();

            string classe = "bg-warning text-dark"; // padrão = Pendente

            switch (situacao.ToUpper())
            {
                case "BAIXADO":
                    classe = "bg-success";
                    break;

                case "CANCELADO":
                    classe = "bg-danger";
                    break;

                case "ENCERRADO":
                    classe = "bg-primary";
                    break;

                case "PENDENTE":
                    classe = "bg-warning text-dark";
                    break;
            }

            return $"<span class='badge {classe}'>{situacao}</span>";
        }
    }
}