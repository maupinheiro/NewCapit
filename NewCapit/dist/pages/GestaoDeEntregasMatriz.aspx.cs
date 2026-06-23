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
            Session["Pagina"] = 1; // opcional (volta pra primeira página)
            CarregarGrid();
        }
        private void CarregarGrid()
        {
            int pageSize = 35;
            int paginaAtual = Session["Pagina"] != null ? (int)Session["Pagina"] : 1;

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

            string sqlFiltro = @"
    WHERE c.empresa = '1111'
    AND c.emissao >= @ini
    AND c.emissao < DATEADD(DAY, 1, @fim)";

            List<SqlParameter> parametros = new List<SqlParameter>
    {
        new SqlParameter("@ini", dataInicio),
        new SqlParameter("@fim", dataFim)
    };

            if (chkOcultarConcluidos.Checked)
            {
                sqlFiltro += " AND c.situacao <> 'VIAGEM CONCLUIDA' AND c.fl_exclusao IS NULL";
            }

            ConfigurarFiltrosDinamicos(ref sqlFiltro, parametros, frota, placa, motorista, expedidor, recebedor, status);

            string conn = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
            DataTable dt = new DataTable();
            int totalRegistros = 0;
            Dictionary<string, int> dadosGrafico = new Dictionary<string, int>();

            using (SqlConnection con = new SqlConnection(conn))
            {
                con.Open();

                // 🚀 MELHORIA: Filtrando ev.dt_posicao para trazer apenas registros do dia de HOJE (meia-noite em diante)
                string queryGridOtimizada = @"
WITH CarregamentosBase AS (
    SELECT c.*, v.codvei, v.terminal
    FROM tbcarregamentos c
    LEFT JOIN tbveiculos v ON c.placa = v.plavei AND v.fl_exclusao IS NULL
    " + sqlFiltro + @"
),
DadosPaginados AS (
    SELECT cb.*, 
           e.cod_evento, e.nr_evento, e.ds_cidade, e.ds_uf, e.dt_posicao,
           ROW_NUMBER() OVER(ORDER BY cb.veiculo ASC) AS RowNum,
           COUNT(*) OVER() AS TotalGeral
    FROM CarregamentosBase cb
    OUTER APPLY (
        SELECT TOP 1 ev.cod_evento, ev.nr_evento, ev.ds_cidade, ev.ds_uf, ev.dt_posicao
        FROM tb_evento ev
        WHERE ev.nr_idveiculo = cb.terminal
          AND ev.dt_posicao >= CAST(GETDATE() AS DATE) -- 📅 Filtra a partir da meia-noite do dia atual
          AND (ev.fl_tratado = 0 OR ev.fl_tratado IS NULL)
          AND ev.nr_evento IS NOT NULL 
          AND ev.nr_evento <> '' 
          AND ev.nr_evento NOT LIKE '%-%'
        ORDER BY ev.dt_posicao DESC
    ) e
)
SELECT * FROM DadosPaginados 
WHERE RowNum BETWEEN ((@pagina - 1) * @pageSize + 1) AND (@pagina * @pageSize)";

                using (SqlCommand cmd = new SqlCommand(queryGridOtimizada, con))
                {
                    cmd.CommandTimeout = 90;

                    cmd.Parameters.AddRange(parametros.Select(p => new SqlParameter(p.ParameterName, p.Value)).ToArray());
                    cmd.Parameters.AddWithValue("@pagina", paginaAtual);
                    cmd.Parameters.AddWithValue("@pageSize", pageSize);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }

                if (dt.Rows.Count > 0)
                {
                    totalRegistros = Convert.ToInt32(dt.Rows[0]["TotalGeral"]);
                }

                // QUERY DOS KPIS
                string queryStatus = "SELECT status, COUNT(*) total FROM tbcarregamentos c " + sqlFiltro + " GROUP BY status";
                using (SqlCommand cmdStatus = new SqlCommand(queryStatus, con))
                {
                    cmdStatus.CommandTimeout = 90;
                    cmdStatus.Parameters.AddRange(parametros.Select(p => new SqlParameter(p.ParameterName, p.Value)).ToArray());

                    using (SqlDataReader dr = cmdStatus.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            dadosGrafico[dr["status"].ToString()] = Convert.ToInt32(dr["total"]);
                        }
                    }
                }

                // TRADUÇÃO DE EVENTOS EM MEMÓRIA
                if (dt.Rows.Count > 0)
                {
                    dt.Columns.Add("ds_evento", typeof(string));
                    Dictionary<string, string> dicionarioEventos = ObterDicionarioEventos(con);

                    foreach (DataRow row in dt.Rows)
                    {
                        if (row["nr_evento"] != DBNull.Value && !string.IsNullOrEmpty(row["nr_evento"].ToString()))
                        {
                            string nrEventoOriginal = row["nr_evento"].ToString();
                            string[] codigos = nrEventoOriginal.Split(',');
                            List<string> descricoesEncontradas = new List<string>();

                            foreach (string cod in codigos)
                            {
                                string codLimpo = cod.Trim();
                                if (dicionarioEventos.TryGetValue(codLimpo, out string descricao))
                                {
                                    descricoesEncontradas.Add(descricao);
                                }
                            }

                            row["ds_evento"] = descricoesEncontradas.Count > 0
                                ? string.Join(" / ", descricoesEncontradas)
                                : "Evento Desconhecido (" + nrEventoOriginal + ")";
                        }
                        else
                        {
                            row["ds_evento"] = "";
                        }
                    }
                }
            }

            // ATUALIZAÇÃO DA INTERFACE (UI)
            int totalPaginas = (int)Math.Ceiling((double)totalRegistros / pageSize);
            if (totalPaginas < 1) totalPaginas = 1;

            lblTotalGeral.InnerText = $"Página {paginaAtual} de {totalPaginas} | Total: {totalRegistros}";
            lblPaginaAtual.Text = paginaAtual.ToString();
            lblTotalPaginas.Text = totalPaginas.ToString();
            Session["TotalPaginas"] = totalPaginas;

            gvOrdens.DataSource = dt;
            gvOrdens.DataBind();

            if (dadosGrafico.Count > 0)
            {
                string labels = string.Join(",", dadosGrafico.Keys.Select(x => $"'{x}'"));
                string valores = string.Join(",", dadosGrafico.Values);

                ScriptManager.RegisterStartupScript(this, GetType(), "kpi", $"renderKPI([{labels}], [{valores}]);", true);
            }
        }

        // Método auxiliar para não poluir o código principal e garantir o Cache
        private Dictionary<string, string> ObterDicionarioEventos(SqlConnection con)
        {
            var dicionario = Cache["ListaEventos"] as Dictionary<string, string>;
            if (dicionario == null)
            {
                dicionario = new Dictionary<string, string>();
                using (SqlCommand cmdLista = new SqlCommand("SELECT CAST(cod_evento AS VARCHAR) cod, ds_evento FROM tb_lista_evento", con))
                using (SqlDataReader readerLista = cmdLista.ExecuteReader())
                {
                    while (readerLista.Read())
                    {
                        dicionario[readerLista["cod"].ToString().Trim()] = readerLista["ds_evento"].ToString().Trim();
                    }
                }
                Cache.Insert("ListaEventos", dicionario, null, DateTime.Now.AddHours(2), TimeSpan.Zero);
            }
            return dicionario;
        }

        // Método auxiliar para organizar os filtros
        private void ConfigurarFiltrosDinamicos(ref string sqlFiltro, List<SqlParameter> parametros, string frota, string placa, string motorista, string expedidor, string recebedor, string status)
        {
            if (!string.IsNullOrEmpty(frota)) { sqlFiltro += " AND (c.tipoveiculo LIKE @veiculo OR c.veiculo LIKE @veiculo)"; parametros.Add(new SqlParameter("@veiculo", "%" + frota + "%")); }
            if (!string.IsNullOrEmpty(placa)) { sqlFiltro += " AND (c.placa LIKE @placa OR c.reboque1 LIKE @placa OR c.reboque2 LIKE @placa)"; parametros.Add(new SqlParameter("@placa", "%" + placa + "%")); }
            if (!string.IsNullOrEmpty(motorista)) { sqlFiltro += " AND (c.codmotorista LIKE @motorista OR c.nomemotorista LIKE @motorista)"; parametros.Add(new SqlParameter("@motorista", "%" + motorista + "%")); }
            if (!string.IsNullOrEmpty(expedidor)) { sqlFiltro += " AND (c.cod_expedidor LIKE @expedidor OR c.expedidor LIKE @expedidor)"; parametros.Add(new SqlParameter("@expedidor", "%" + expedidor + "%")); }
            if (!string.IsNullOrEmpty(recebedor)) { sqlFiltro += " AND (c.cod_recebedor LIKE @recebedor OR c.recebedor LIKE @recebedor)"; parametros.Add(new SqlParameter("@recebedor", "%" + recebedor + "%")); }
            if (!string.IsNullOrEmpty(status)) { sqlFiltro += " AND (c.status LIKE @status OR c.situacao LIKE @status)"; parametros.Add(new SqlParameter("@status", "%" + status + "%")); }
        }
        protected void btnIrPagina_Click(object sender, EventArgs e)
        {
            int paginaDigitada;

            if (int.TryParse(txtIrPagina.Text, out paginaDigitada))
            {
                int totalPaginas = Session["TotalPaginas"] != null ? (int)Session["TotalPaginas"] : 1;

                // 🔒 limita entre 1 e totalPaginas
                if (paginaDigitada < 1)
                    paginaDigitada = 1;

                if (paginaDigitada > totalPaginas)
                    paginaDigitada = totalPaginas;

                Session["Pagina"] = paginaDigitada;
                CarregarGrid();
            }
        }        
        protected void gvOrdens_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // O ASP.NET usa índice 0 para a primeira página, por isso somamos 1
            Session["Pagina"] = e.NewPageIndex + 1;
            CarregarGrid();
        }
        protected void btnPrimeiro_Click(object sender, EventArgs e)
        {
            Session["Pagina"] = 1;
            CarregarGrid();
        }
        protected void btnAnterior_Click(object sender, EventArgs e)
        {
            // Proteção idêntica contra valores nulos
            int pagina = Session["Pagina"] != null ? (int)Session["Pagina"] : 1;

            if (pagina > 1)
            {
                Session["Pagina"] = pagina - 1;
            }

            CarregarGrid();
        }
        protected void btnProximo_Click(object sender, EventArgs e)
        {
            // Proteção: Se a Session["Pagina"] estiver nula, assume que está na página 1
            int pagina = Session["Pagina"] != null ? (int)Session["Pagina"] : 1;

            // Proteção: Se a Session["TotalPaginas"] estiver nula, assume 1 para não quebrar
            int total = Session["TotalPaginas"] != null ? (int)Session["TotalPaginas"] : 1;

            if (pagina < total)
            {
                Session["Pagina"] = pagina + 1;
            }

            CarregarGrid();
        }
        protected void btnUltimo_Click(object sender, EventArgs e)
        {
            Session["Pagina"] = (int)Session["TotalPaginas"];
            CarregarGrid();
        }
        protected void gvOrdens_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Pager)
            {
                int paginaAtual = Session["Pagina"] != null ? (int)Session["Pagina"] : 1;
                int totalPaginas = Session["TotalPaginas"] != null ? (int)Session["TotalPaginas"] : 1;

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

        
            //if (e.CommandName == "Editar")
            //{
            //    string numCarregamento = e.CommandArgument.ToString();
            //    string url = $"Frm_AtualizaColetaMatriz.aspx?carregamento={numCarregamento}";

            //    // Cria o script JavaScript para abrir em nova aba
            //    string script = $"window.open('{url}', '_blank');";

            //    // Injeta o script na página para ser executado no navegador
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "AbrirNovaAba", script, true);
            //}
        }
        protected void Timer1_Tick(object sender, EventArgs e)
        {
            CarregarGrid(); // seu método
            //up1.Update();
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
                    veiculo, c.tipoveiculo, c.placa, c.reboque1, c.reboque2,
                    codmotorista, nomemotorista,
                    c.codtra, transportadora,
                    cod_expedidor, expedidor,
                    cod_recebedor, recebedor,
                    cid_expedidor, uf_expedidor,
                    cid_recebedor, uf_recebedor,
                    num_carregamento, carga, emissao, situacao, status
                FROM tbcarregamentos as c
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
                    sql += " AND (c.tipoveiculo LIKE @veiculo OR veiculo LIKE @veiculo)";
                    parametros.Add(new SqlParameter("@veiculo", "%" + frota + "%"));
                }

                if (!string.IsNullOrEmpty(placa))
                {
                    sql += " AND (placa LIKE @placa OR c.reboque1 LIKE @placa OR c.reboque2 LIKE @placa)";
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

        protected void btnTratarEvento_Click(object sender, EventArgs e)
        {
            string codEvento = hfCodEvento.Value;
            // Pega o usuário logado (ajuste conforme seu sistema de login)
            string usuario = Session["Usuario"] != null ? Session["Usuario"].ToString() : "Sistema";

            if (!string.IsNullOrEmpty(codEvento))
            {
                string conn = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;

                using (SqlConnection con = new SqlConnection(conn))
                {
                    string sqlUpdate = @"
                UPDATE tb_evento 
                SET fl_tratado = 1, 
                    dt_tratamento = GETDATE(),
                    usuario_tratado = @usuario
                WHERE cod_evento = @cod_evento";

                    using (SqlCommand cmd = new SqlCommand(sqlUpdate, con))
                    {
                        cmd.Parameters.AddWithValue("@usuario", usuario);
                        cmd.Parameters.AddWithValue("@cod_evento", codEvento);

                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                // Limpa o campo oculto
                hfCodEvento.Value = "";

                // Fecha o modal via Javascript e recarrega a Grid
                ScriptManager.RegisterStartupScript(this, GetType(), "fecharModal", "fecharModalAlerta();", true);

                // Atualiza a listagem sumindo com a sirene que foi tratada
                CarregarGrid();
            }
        }
    }
}