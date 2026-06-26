using DAL;
using DocumentFormat.OpenXml.Office.Word;
using ICSharpCode.SharpZipLib.Zip;
using iTextSharp.text.pdf;
using iTextSharp.text;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using static NewCapit.dist.pages.AbrirOS;
using NewCapit.Models.Krona;


namespace NewCapit.dist.pages
{
    public partial class ListaOS : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conexao"].ToString());
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
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
                txtDataInicial.Text = DateTime.Now.AddDays(-7).ToString("dd/MM/yyyy");
                txtDataFinal.Text = DateTime.Now.ToString("dd/MM/yyyy");

                //ListarOS();
                Session["Pagina"] = 1;
                CarregarGrid();
            }

        }
        private void CarregarGrid()
        {
            int pageSize = 10;
            int paginaAtual = Session["Pagina"] != null ? (int)Session["Pagina"] : 1;

            string frota_placa = txtFrotaPlaca.Text?.Trim();
            string cracha_motorista = txtCrachaMotorista.Text?.Trim();
            string fornecedor = txtFornecedor.Text?.Trim();
            string ordem_servico = txtOrdem_Servico.Text?.Trim();
            string status = ddlStatus.SelectedValue?.Trim();
            string manutencao = ddlManutencao.SelectedValue?.Trim();
            string servico = ddlServico.SelectedValue?.Trim();

            string[] formatos = { "dd/MM/yyyy", "yyyy-MM-dd" };

            DateTime dataInicio;
            DateTime dataFim;

            if (!DateTime.TryParseExact(txtDataInicial.Text, formatos,
                CultureInfo.InvariantCulture, DateTimeStyles.None, out dataInicio))
            {
                dataInicio = DateTime.Now.AddDays(-7);
            }

            if (!DateTime.TryParseExact(txtDataFinal.Text, formatos,
                CultureInfo.InvariantCulture, DateTimeStyles.None, out dataFim))
            {
                dataFim = DateTime.Now;
            }

            string sqlWhere = @"
    WHERE o.data_abertura >= @ini
      AND o.data_abertura < DATEADD(DAY,1,@fim)";

            List<SqlParameter> parametros = new List<SqlParameter>
    {
        new SqlParameter("@ini", dataInicio),
        new SqlParameter("@fim", dataFim)
    };

            // FILTROS

            if (!string.IsNullOrEmpty(frota_placa))
            {
                sqlWhere += @"
        AND (
            o.placa COLLATE Latin1_General_CI_AI LIKE @placa
            OR o.id_veiculo LIKE @veiculo
            OR o.id_carreta LIKE @carreta
        )";

                parametros.Add(new SqlParameter("@placa", "%" + frota_placa + "%"));
                parametros.Add(new SqlParameter("@veiculo", "%" + frota_placa + "%"));
                parametros.Add(new SqlParameter("@carreta", "%" + frota_placa + "%"));
            }

            if (!string.IsNullOrEmpty(cracha_motorista))
            {
                sqlWhere += @"
        AND (
            o.id_motorista LIKE @cracha
            OR o.nome_motorista LIKE @motorista
        )";

                parametros.Add(new SqlParameter("@cracha", "%" + cracha_motorista + "%"));
                parametros.Add(new SqlParameter("@motorista", "%" + cracha_motorista + "%"));
            }

            if (!string.IsNullOrEmpty(fornecedor))
            {
                sqlWhere += @"
        AND (
            o.id_fornecedor LIKE @idFornecedor
            OR o.nome_fornecedor COLLATE Latin1_General_CI_AI LIKE @fornecedor
        )";

                parametros.Add(new SqlParameter("@idFornecedor", "%" + fornecedor + "%"));
                parametros.Add(new SqlParameter("@fornecedor", "%" + fornecedor + "%"));
            }

            if (!string.IsNullOrEmpty(ordem_servico))
            {
                sqlWhere += " AND o.id_os LIKE @oServico";
                parametros.Add(new SqlParameter("@oServico", "%" + ordem_servico + "%"));
            }

            if (!string.IsNullOrEmpty(status))
            {
                sqlWhere += " AND o.status = @status";
                parametros.Add(new SqlParameter("@status", status));
            }

            if (!string.IsNullOrEmpty(manutencao))
            {
                sqlWhere += " AND o.tipo_os = @tipo_os";
                parametros.Add(new SqlParameter("@tipo_os", manutencao));
            }

            if (!string.IsNullOrEmpty(servico))
            {
                sqlWhere += " AND o.interno_externo = @internoExterno";
                parametros.Add(new SqlParameter("@internoExterno", servico));
            }

            string sqlBase = $@"
            SELECT
                o.id_os,
                o.placa,
                o.tipo_veiculo,
                o.marca,
                o.modelo,
                o.nome_fornecedor,

                CASE
                    WHEN o.tipo_veiculo = 'CARRETA'
                        THEN o.id_carreta
                    ELSE o.id_veiculo
                END AS codigo_veiculo,

                CASE
                    WHEN o.interno_externo = 'I' THEN 'Interno'
                    WHEN o.interno_externo = 'E' THEN 'Externo'
                END AS servico_interno_externo,

                CASE
                    WHEN o.tipo_os = 'C' THEN 'Corretiva'
                    WHEN o.tipo_os = 'P' THEN 'Preventiva'
                END AS os_preventiva_corretiva,

                o.nucleo_veiculo,
                o.nome_motorista,
                o.tipo_os,
                o.data_abertura,

                DATEDIFF(
                    DAY,
                    o.data_abertura,
                    ISNULL(o.data_fechamento, GETDATE())
                ) AS dias_aberta,

                CASE
                    WHEN o.status = '1' THEN 'Aberta'
                    WHEN o.status = '2' THEN 'Finalizada'
                    WHEN o.status = '3' THEN 'Cancelada'
                END AS status_texto

            FROM tbordem_servico o
            {sqlWhere}";

            string conn = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;

            using (SqlConnection con = new SqlConnection(conn))
            {
                con.Open();

                string sqlCount = $@"
                SELECT COUNT(*)
                FROM tbordem_servico o
                {sqlWhere}";

                SqlCommand cmdTotal = new SqlCommand(sqlCount, con);

                foreach (SqlParameter p in parametros)
                    cmdTotal.Parameters.AddWithValue(p.ParameterName, p.Value);

                int totalRegistros = Convert.ToInt32(cmdTotal.ExecuteScalar());
                int totalPaginas = (int)Math.Ceiling((double)totalRegistros / pageSize);
                lblTotalGeral.InnerText = $"Página {paginaAtual} de {totalPaginas} | Total: {totalRegistros}";
                lblPaginaAtual.Text = paginaAtual.ToString().Trim();
                lblTotalPaginas.Text = totalPaginas.ToString().Trim();
                Session["TotalPaginas"] = totalPaginas;

                string sql = $@"
                WITH Dados AS
                (
                    SELECT *,
                           ROW_NUMBER() OVER(ORDER BY id_os DESC) AS RowNum
                    FROM
                    (
                        {sqlBase}
                    ) X
                )
                SELECT *
                FROM Dados
                WHERE RowNum BETWEEN ((@pagina - 1) * @pageSize + 1)
                                 AND (@pagina * @pageSize)
                ORDER BY RowNum";

                SqlCommand cmd = new SqlCommand(sql, con);

                foreach (SqlParameter p in parametros)
                    cmd.Parameters.AddWithValue(p.ParameterName, p.Value);

                cmd.Parameters.AddWithValue("@pagina", paginaAtual);
                cmd.Parameters.AddWithValue("@pageSize", pageSize);

                DataTable dt = new DataTable();

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                gvOS.DataSource = dt;
                gvOS.DataBind();
            }
        }

        //private void CarregarGrid()
        //{
        //    int pageSize = 35;
        //    int paginaAtual = Session["Pagina"] != null ? (int)Session["Pagina"] : 1;

        //    string frota_placa = txtFrotaPlaca.Text?.Trim();
        //    string cracha_motorista = txtCrachaMotorista.Text?.Trim();
        //    string fornecedor = txtFornecedor.Text?.Trim();
        //    string ordem_servico = txtOrdem_Servico.Text?.Trim();
        //    string status = ddlStatus.SelectedValue?.Trim();
        //    string manutencao = ddlManutencao.SelectedValue?.Trim();
        //    string servico = ddlServico.SelectedValue?.Trim();

        //    // ✅ DATAS
        //    string[] formatos = { "dd/MM/yyyy", "yyyy-MM-dd" };

        //    DateTime dataInicio, dataFim;

        //    if (!DateTime.TryParseExact(txtDataInicial.Text, formatos, CultureInfo.InvariantCulture, DateTimeStyles.None, out dataInicio))
        //    {
        //        dataInicio = DateTime.Now.AddDays(-7);
        //    }

        //    if (!DateTime.TryParseExact(txtDataFinal.Text, formatos, CultureInfo.InvariantCulture, DateTimeStyles.None, out dataFim))
        //    {
        //        dataFim = DateTime.Now;
        //    }

        //    string sqlBase = @"
        //    SELECT
        //        o.id_os,
        //        o.placa,
        //        o.tipo_veiculo,
        //        o.marca,
        //        o.modelo,
        //        o.nome_fornecedor,
        //        CASE
        //            WHEN o.tipo_veiculo = 'CARRETA'
        //                THEN o.id_carreta
        //            ELSE o.id_veiculo
        //        END AS codigo_veiculo,
        //        CASE
        //            WHEN o.interno_externo = 'I' THEN 'Interno'
        //            WHEN o.interno_externo = 'E' THEN 'Externo'
        //        END AS servico_interno_externo,
        //        CASE
        //            WHEN o.tipo_os = 'C' THEN 'Corretiva'
        //            WHEN o.tipo_os = 'P' THEN 'Preventiva'
        //        END AS os_preventiva_corretiva,
        //        o.nucleo_veiculo,
        //        o.nome_motorista,
        //        o.tipo_os,
        //        o.data_abertura,
        //        DATEDIFF(DAY,o.data_abertura,
        //            ISNULL(o.data_fechamento,GETDATE())
        //        ) AS dias_aberta,
        //        CASE
        //            WHEN o.status = '1' THEN 'Aberta'
        //            WHEN o.status = '2' THEN 'Finalizada'
        //            WHEN o.status = '3' THEN 'Cancelada'
        //        END AS status_texto
        //    FROM tbordem_servico o
        //    WHERE o.data_abertura >= @ini
        //    AND o.data_abertura < DATEADD(DAY,1,@fim)";

        //    List<SqlParameter> parametros = new List<SqlParameter>
        //    {
        //        new SqlParameter("@ini", dataInicio),
        //        new SqlParameter("@fim", dataFim)
        //    };
        //    // 🔎 FILTROS
        //    if (!string.IsNullOrEmpty(frota_placa))
        //    {
        //        sqlFiltro += " AND (o.placa COLLATE Latin1_General_CI_AI LIKE @placa OR o.id_veiculo COLLATE Latin1_General_CI_AI LIKE @veiculo) OR o.id_carreta COLLATE Latin1_General_CI_AI LIKE @carreta)";
        //        parametros.Add(new SqlParameter("@placa", "%" + frota_placa + "%"));
        //        parametros.Add(new SqlParameter("@veiculo", "%" + frota_placa + "%"));
        //        parametros.Add(new SqlParameter("@carreta", "%" + frota_placa + "%"));
        //    }

        //    if (!string.IsNullOrEmpty(cracha_motorista))
        //    {
        //        sqlFiltro += " AND (id_motorista LIKE @cracha OR nome_motorista LIKE @motorista)";
        //        parametros.Add(new SqlParameter("@cracha", "%" + cracha_motorista + "%"));
        //        parametros.Add(new SqlParameter("@motorista", "%" + cracha_motorista + "%"));
        //    }

        //    if (!string.IsNullOrEmpty(fornecedor))
        //    {
        //        sqlFiltro += " AND (id_fornecedor LIKE @idFornecedor OR nome_fornecedor COLLATE Latin1_General_CI_AI LIKE @fornecedor)";
        //        parametros.Add(new SqlParameter("@ifFornecedor", "%" + fornecedor + "%"));
        //        parametros.Add(new SqlParameter("@fornecedor", "%" + fornecedor + "%"));
        //    }

        //    if (!string.IsNullOrEmpty(ordem_servico))
        //    {
        //        sqlFiltro += " AND (id_os LIKE @oServico)";
        //        parametros.Add(new SqlParameter("@oServico", "%" + ordem_servico + "%"));
        //    }

        //    if (!string.IsNullOrEmpty(status))
        //    {
        //        sqlFiltro += " AND (status LIKE @status)";
        //        parametros.Add(new SqlParameter("@status", "%" + status + "%"));
        //    }

        //    if (!string.IsNullOrEmpty(manutencao))
        //    {
        //        sqlFiltro += " AND (tipo_os LIKE @tipo_os)";
        //        parametros.Add(new SqlParameter("@tipo_os", "%" + manutencao + "%"));
        //    }

        //    if (!string.IsNullOrEmpty(servico))
        //    {
        //        sqlFiltro += " AND (iterno_externo LIKE @internoExteno)";
        //        parametros.Add(new SqlParameter("@internoExterno", "%" + servico + "%"));
        //    }

        //    string conn = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;

        //    using (SqlConnection con = new SqlConnection(conn))
        //    {
        //        con.Open();

        //        // 🔢 TOTAL
        //        SqlCommand cmdTotal = new SqlCommand($@"
        //        SELECT COUNT(*) 
        //        FROM tbordem_servico
        //        {sqlFiltro}", con);

        //        foreach (var p in parametros)
        //            cmdTotal.Parameters.AddWithValue(p.ParameterName, p.Value);

        //        int totalRegistros = (int)cmdTotal.ExecuteScalar();
        //        int totalPaginas = (int)Math.Ceiling((double)totalRegistros / pageSize);

        //        //lblTotalGeral.InnerText = $"Página {paginaAtual} de {totalPaginas} | Total: {totalRegistros}";
        //        //lblPaginaAtual.Text = paginaAtual.ToString().Trim();
        //        //lblTotalPaginas.Text = totalPaginas.ToString().Trim();
        //        //Session["TotalPaginas"] = totalPaginas;

        //        // 📋 GRID
        //        string sql = $@"
        //        WITH Dados AS(
        //            SELECT *,
        //            ROW_NUMBER() OVER(ORDER BY id_os ASC) AS RowNum
        //            FROM tbordem_servico
        //            {sqlFiltro}
        //        )
        //        SELECT *
        //        FROM Dados
        //        WHERE RowNum BETWEEN ((@pagina - 1) * @pageSize + 1)
        //                 AND (@pagina * @pageSize)";

        //        SqlCommand cmd = new SqlCommand(sql, con);

        //        foreach (var p in parametros)
        //            cmd.Parameters.AddWithValue(p.ParameterName, p.Value);

        //        cmd.Parameters.AddWithValue("@pagina", paginaAtual);
        //        cmd.Parameters.AddWithValue("@pageSize", pageSize);

        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        DataTable dt = new DataTable();
        //        da.Fill(dt);

        //        gvOS.DataSource = dt;
        //        gvOS.DataBind();


        //    }
        //}

        //       private void ListarOS()
        //       {
        //           DateTime dataInicio = string.IsNullOrWhiteSpace(txtDataInicial.Text)
        //               ? DateTime.Now.AddDays(-7)
        //               : DateTime.ParseExact(txtDataInicial.Text, "yyyy-MM-dd", null);

        //           DateTime dataFim = string.IsNullOrWhiteSpace(txtDataFinal.Text)
        //               ? DateTime.Now
        //               : DateTime.ParseExact(txtDataFinal.Text, "yyyy-MM-dd", null);

        //           using (SqlConnection conn = new SqlConnection(
        //ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
        //           {
        //               //DATEDIFF(DAY, o.data_abertura, GETDATE()) AS dias_aberta, 

        //               string sql = @"SELECT 
        //               o.id_os,
        //               o.placa,
        //               o.tipo_veiculo,
        //               o.marca,
        //               o.modelo,
        //               o.nome_fornecedor,
        //               CASE 
        //                   WHEN o.tipo_veiculo = 'CARRETA'
        //                       THEN o.id_carreta
        //                   ELSE o.id_veiculo
        //               END AS codigo_veiculo,
        //               CASE 
        //                   WHEN o.interno_externo = 'I' THEN 'Interno'
        //                   WHEN o.interno_externo = 'E' THEN 'Externo'    
        //               END AS servico_interno_externo,
        //               CASE 
        //                   WHEN o.tipo_os = 'C' THEN 'Corretiva'
        //                   WHEN o.tipo_os = 'P' THEN 'Preventiva'    
        //               END AS os_preventiva_corretiva,
        //               o.nucleo_veiculo,
        //               o.nome_motorista,
        //               o.tipo_os,
        //               o.data_abertura,                
        //               DATEDIFF(DAY, o.data_abertura,
        //                   CASE 
        //                       WHEN o.data_fechamento IS NOT NULL THEN o.data_fechamento
        //                       ELSE GETDATE()
        //                   END
        //               ) AS dias_aberta,
        //               CASE 
        //                   WHEN o.status = '1' THEN 'Aberta'
        //                   WHEN o.status = '2' THEN 'Finalizada'
        //                   WHEN o.status = '3' THEN 'Cancelada'
        //               END AS status_texto
        //               FROM tbordem_servico o
        //               where (
        //                   @pesquisa IS NULL
        //                   OR o.placa LIKE '%' + @pesquisa + '%'
        //                   OR o.id_veiculo LIKE '%' + @pesquisa + '%' 
        //                   OR o.id_carreta LIKE '%' + @pesquisa + '%' 
        //                   OR o.nome_motorista LIKE '%' + @pesquisa + '%'
        //                   OR o.nome_fornecedor LIKE '%' + @pesquisa + '%'
        //                   OR o.status LIKE '%' + @pesquisa + '%'
        //                   OR o.tipo_os LIKE '%' + @pesquisa + '%'
        //                   OR o.interno_externo LIKE '%' + @pesquisa + '%' 
        //                   OR o.nucleo_veiculo LIKE '%' + @pesquisa + '%'   
        //                   OR CAST(o.id_os AS VARCHAR) LIKE '%' + @pesquisa + '%'
        //                   OR CAST(o.id_veiculo AS VARCHAR) LIKE '%' + @pesquisa + '%'
        //                   OR CAST(o.id_carreta AS VARCHAR) LIKE '%' + @pesquisa + '%'
        //                   OR CAST(o.tipo_veiculo AS VARCHAR) LIKE '%' + @pesquisa + '%'
        //                   )

        //                   AND (@status IS NULL OR o.status = @status)                    

        //                   AND (@dataInicial IS NULL OR o.data_abertura >= @dataInicial)

        //                   AND (@dataFinal IS NULL OR o.data_abertura <= @dataFinal)

        //                   ORDER BY o.id_os DESC";
        //               SqlCommand cmd = new SqlCommand(sql, conn);

        //               cmd.Parameters.AddWithValue("@pesquisa",
        //               string.IsNullOrWhiteSpace(txtPesquisa.Text) ? (object)DBNull.Value : txtPesquisa.Text);

        //               cmd.Parameters.AddWithValue("@status",
        //               string.IsNullOrWhiteSpace(ddlStatus.SelectedValue) ? (object)DBNull.Value : ddlStatus.SelectedValue);

        //               cmd.Parameters.AddWithValue("@ordem_servico",
        //               string.IsNullOrWhiteSpace(txtOrdem_Servico.Text) ? (object)DBNull.Value : txtOrdem_Servico.Text);

        //               cmd.Parameters.AddWithValue("@dataInicial", Convert.ToDateTime(dataInicio)); 

        //               //cmd.Parameters.AddWithValue("@dataFinal",
        //               //string.IsNullOrWhiteSpace(txtDataFinal.Text) ? (object)DBNull.Value : txtDataFinal.Text);

        //               cmd.Parameters.AddWithValue("@dataFinal", Convert.ToDateTime(dataFim));


        //               conn.Open();

        //               SqlDataAdapter da = new SqlDataAdapter(cmd);

        //               DataTable dt = new DataTable();
        //               da.Fill(dt);

        //               gvOS.DataSource = dt;
        //               gvOS.DataBind();
        //           }
        //       }

        //protected void PesquisarOS(object sender, EventArgs e)
        //{
        //    using (SqlConnection conn = new SqlConnection(
        //        System.Configuration.ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
        //    {
        //        string sql = @"
        //SELECT 
        //    o.id_os,
        //    o.placa,
        //    o.tipo_veiculo,
        //    o.nome_fornecedor,
        //    CASE WHEN o.tipo_veiculo = 'CARRETA' THEN o.id_carreta ELSE o.id_veiculo END AS codigo_veiculo,
        //    CASE WHEN o.interno_externo = 'I' THEN 'Interno'
        //         WHEN o.interno_externo = 'E' THEN 'Externo' END AS servico_interno_externo,
        //    CASE WHEN o.tipo_os = 'C' THEN 'Corretiva'
        //         WHEN o.tipo_os = 'P' THEN 'Preventiva' END AS os_preventiva_corretiva,
        //    o.nucleo_veiculo,
        //    o.nome_motorista,
        //    o.tipo_os,
        //    o.data_abertura,
        //    DATEDIFF(DAY, o.data_abertura, GETDATE()) AS dias_aberta,
        //    CASE 
        //        WHEN o.status = '1' THEN 'Aberta'
        //        WHEN o.status = '2' THEN 'Finalizada'
        //        WHEN o.status = '3' THEN 'Cancelada'
        //    END AS status_texto
        //FROM tbordem_servico o
        //WHERE (
        //    @pesquisa IS NULL
        //    OR o.placa LIKE '%' + @pesquisa + '%'
        //    OR o.nome_motorista LIKE '%' + @pesquisa + '%'
        //    OR o.nome_fornecedor LIKE '%' + @pesquisa + '%'
        //    OR o.nucleo_veiculo LIKE '%' + @pesquisa + '%'             
        //    OR CAST(o.id_veiculo AS VARCHAR) LIKE '%' + @pesquisa + '%'
        //    OR CAST(o.id_carreta AS VARCHAR) LIKE '%' + @pesquisa + '%'
        //    OR CAST(o.tipo_veiculo AS VARCHAR) LIKE '%' + @pesquisa + '%'
        //)
        //AND (@status IS NULL OR o.status = @status)
        //AND (@interno_externo IS NULL OR o.interno_externo = @interno_externo)
        //AND (CAST(o.id_os AS VARCHAR) = @os_id)
        //AND (@tipo_os IS NULL OR o.tipo_os = @tipo_os)
        //AND (@dataInicial IS NULL OR CAST(o.data_abertura AS DATE) >= @dataInicial)
        //AND (@dataFinal IS NULL OR CAST(o.data_abertura AS DATE) <= @dataFinal)
        //ORDER BY o.id_os DESC
        //";

        //        SqlCommand cmd = new SqlCommand(sql, conn);

        //        // Filtros
        //        cmd.Parameters.AddWithValue("@pesquisa",
        //            string.IsNullOrWhiteSpace(txtPesquisa.Text) ? (object)DBNull.Value : txtPesquisa.Text.Trim());
        //        cmd.Parameters.AddWithValue("@status",
        //            string.IsNullOrWhiteSpace(ddlStatus.SelectedValue) ? (object)DBNull.Value : ddlStatus.SelectedValue);
        //        cmd.Parameters.AddWithValue("@tipo_os",
        //            string.IsNullOrWhiteSpace(ddlManutencao.SelectedValue) ? (object)DBNull.Value : ddlManutencao.SelectedValue);
        //        cmd.Parameters.AddWithValue("@interno_externo",
        //            string.IsNullOrWhiteSpace(ddlServico.SelectedValue) ? (object)DBNull.Value : ddlServico.SelectedValue);
        //        cmd.Parameters.AddWithValue("@os_id",
        //            string.IsNullOrWhiteSpace(txtOrdem_Servico.Text) ? (object)DBNull.Value : txtOrdem_Servico.Text.Trim());

        //        // Datas (somente data)
        //        DateTime dataInicial, dataFinal;
        //        object paramDataInicial = DBNull.Value;
        //        object paramDataFinal = DBNull.Value;

        //        if (!string.IsNullOrWhiteSpace(txtDataInicial.Text))
        //        {
        //            if (DateTime.TryParseExact(txtDataInicial.Text, "dd/MM/yyyy",
        //                new CultureInfo("pt-BR"), DateTimeStyles.None, out dataInicial))
        //            {
        //                paramDataInicial = dataInicial.Date;
        //            }
        //        }

        //        if (!string.IsNullOrWhiteSpace(txtDataFinal.Text))
        //        {
        //            if (DateTime.TryParseExact(txtDataFinal.Text, "dd/MM/yyyy",
        //                new CultureInfo("pt-BR"), DateTimeStyles.None, out dataFinal))
        //            {
        //                paramDataFinal = dataFinal.Date;
        //            }
        //        }

        //        cmd.Parameters.Add("@dataInicial", SqlDbType.Date).Value = paramDataInicial;
        //        cmd.Parameters.Add("@dataFinal", SqlDbType.Date).Value = paramDataFinal;

        //        // Executar
        //        conn.Open();
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        DataTable dt = new DataTable();
        //        da.Fill(dt);

        //        if (dt.Rows.Count == 0)
        //        {
        //            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Nenhuma OS encontrada.');", true);
        //        }

        //        gvOS.DataSource = dt;
        //        gvOS.DataBind();
        //    }
        //}
        protected void gvOS_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int numeroOS = Convert.ToInt32(e.CommandArgument);

            //if (e.CommandName == "pdf")
            //{
            //    Response.Redirect("ImprimirOS.aspx?os=" + numeroOS);
            //}

            if (e.CommandName == "Finalizar")
            {
                // int numeroOS = Convert.ToInt32(e.CommandArgument);

                // 🔹 Redireciona para tela de finalização
                Response.Redirect("FinalizarOSGrid.aspx?os=" + numeroOS);
            }

            if (e.CommandName == "abrir")
            {
                //Response.Redirect("EditarOS.aspx?os=" + numeroOS);
            }

            if (e.CommandName == "pdf")
            {

                string id = e.CommandArgument.ToString();

                //Response.Write("ID: " + id);
                //Response.End();

                CarregarOrdem(id);

                // abre modal
                ScriptManager.RegisterStartupScript(this, this.GetType(), "modal", "abrirModal();", true);


            }
        }
        private void CarregarOrdem(string id)
        {
            string strConn = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(strConn))
            {
                //string sql = @"SELECT status, id_os, tipo_os, data_abertura, interno_externo, tipo_os, id_fornecedor, nome_fornecedor, id_motorista, nome_motorista,transp_motorista, nucleo_motorista, tipo_veiculo, id_veiculo, placa, tipo_veiculo, marca, modelo, ano_modelo, id_carreta, parte_mecanica, parte_eletrica, parte_borracharia, parte_funilaria, servico_executado_mecanica, servico_executado_eletrica, servico_executado_borracharia, servico_executado_funilaria
                //  FROM tbordem_servico
                //  WHERE id_os = @id";

                string sql = @"
                SELECT 
                    os.status,
                    os.id_os,
                    os.tipo_os,
                    os.data_abertura,
                    os.interno_externo,
                    os.id_fornecedor,
                    os.nome_fornecedor,
                    os.id_motorista,
                    os.nome_motorista,
                    os.transp_motorista,
                    os.nucleo_motorista,
                    os.tipo_veiculo,
                    os.id_veiculo,
                    os.placa,
                    os.marca,
                    os.modelo,
                    os.ano_modelo,
                    os.id_carreta,
                    os.parte_mecanica,
                    os.parte_eletrica,
                    os.parte_borracharia,
                    os.parte_funilaria,
                    os.servico_executado_mecanica,
                    os.servico_executado_eletrica,
                    os.servico_executado_borracharia,
                    os.servico_executado_funilaria,
                    p.descricao,
                    p.quant,
                    p.cracha,
                    p.nome,
                    p.inicio,
                    p.termino,
                    p.tempo_minutos
                FROM tbordem_servico os
                INNER JOIN tbos_pecas p
                    ON os.id_os = p.id_os
                WHERE os.id_os = @id";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);

                conn.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    if (dr["status"].ToString() == "1")
                    {
                        txtStatus.BackColor = System.Drawing.Color.Yellow;
                        txtStatus.ForeColor = System.Drawing.Color.Black;
                        txtStatus.Text = "Aberta";

                    }
                    else if (dr["status"].ToString() == "2")
                    {
                        txtStatus.BackColor = System.Drawing.Color.Green;
                        txtStatus.ForeColor = System.Drawing.Color.White;
                        txtStatus.Text = "Finalizada";
                    }
                    else if (dr["status"].ToString() == "3")
                    {
                        txtStatus.BackColor = System.Drawing.Color.Red;
                        txtStatus.ForeColor = System.Drawing.Color.White;
                        txtStatus.Text = "Cancelada";
                    }
                    else
                    {
                        txtStatus.Text = "N/I";
                    }
                    lblOS.Text = "<b>Nº O.S.:</b></br> " + dr["id_os"].ToString();
                    string idOS = dr["id_os"].ToString().Trim();
                    if (dr["tipo_os"].ToString() == "P")
                    {
                        lblTipo_Os.Text = "<b>Tipo O.S.:</b></br> Preventiva";
                    }
                    else if (dr["tipo_os"].ToString() == "C")
                    {
                        lblTipo_Os.Text = "<b>Tipo O.S.:</b></br> Corretiva";
                    }
                    else
                    {
                        lblTipo_Os.Text = "<b>Tipo O.S.:</b></br> N/I";
                    }
                    lblEmissao.Text = "<b>Emissão:</b></br> " + Convert.ToDateTime(dr["data_abertura"]).ToString("dd/MM/yyyy HH:mm");
                    if (dr["interno_externo"].ToString() == "I")
                    {
                        lblInternoExterno.Text = "<b>Serviço:</b></br> Interno";
                    }
                    else if (dr["interno_externo"].ToString() == "E")
                    {
                        lblInternoExterno.Text = "<b>Serviço:</b></br> Externo";
                    }
                    else
                    {

                        lblInternoExterno.Text = "<b>Serviço:</b></br> N/I";
                    }
                    lblPrestador.Text = "<b>Prestador:</b></br> " + dr["id_fornecedor"].ToString() + " - " + dr["nome_fornecedor"].ToString();
                    lblMotorista.Text = "<b>Motorista:</b> " + dr["id_motorista"].ToString().Trim() + " - " + dr["nome_motorista"].ToString().Trim() + "</br>  <b>Transp.:</b> " + dr["transp_motorista"].ToString().Trim() + "</br>  <b>Núcleo:</b> " + dr["nucleo_motorista"].ToString().Trim();
                    if (dr["tipo_veiculo"].ToString() == "CARRETA")
                    {
                        lblVeiculo.Text = "<b>Veículo:</b>" + dr["id_carreta"].ToString().Trim() + " - " + dr["placa"].ToString().Trim() + "</br>  <b>Tipo:</b> " + dr["tipo_veiculo"].ToString().Trim() + "</br>  <b>Marca/Modelo:</b> " + dr["marca"].ToString().Trim() + "/" + dr["modelo"].ToString().Trim() + "</br>  <b>Fab/Mod:</b> " + dr["ano_modelo"].ToString().Trim();
                    }
                    else
                    {
                        lblVeiculo.Text = "<b>Veículo:</b> " + dr["id_veiculo"].ToString().Trim() + " - " + dr["placa"].ToString().Trim() + "</br>  <b>Tipo:</b> " + dr["tipo_veiculo"].ToString().Trim() + "</br>  <b>Marca/Modelo:</b> " + dr["marca"].ToString().Trim() + "/" + dr["modelo"].ToString().Trim() + " </br> <b>Fab/Mod:</b> " + dr["ano_modelo"].ToString().Trim();
                    }
                    // defeitos mecanicos
                    lblDefeitosMecanicos.Text = "<b>Defeitos Mecânicos Relatos:</b></br> " + dr["parte_mecanica"].ToString();
                    // defeitos mecanicos
                    lblServicoExecutadoMecanica.Text = "<b>Serviços Mecânicos Executados:</b></br> " + dr["servico_executado_mecanica"].ToString();
                    CarregarPecasMecanicas(idOS);
                    // defeitos eletricos
                    lblDefeitosEletricos.Text = "<b>Defeitos Eletricos Relatos:</b></br> " + dr["parte_eletrica"].ToString();
                    // defeitos eletricos
                    lblServicoExecutadoEletrico.Text = "<b>Serviços Eletricos Executados:</b></br> " + dr["servico_executado_eletrica"].ToString();
                    CarregarPecasEletricos(idOS);
                    // defeitos borracharia
                    lblServicoExecutadoBorracharia.Text = "<b>Serviços Borracharia Executados:</b></br> " + dr["servico_executado_borracharia"].ToString();
                    CarregarPecasBorracharia(idOS);
                    // defeitos funilaria
                    lblServicoExecutadoFunilaria.Text = "<b>Serviços Funilaria Executados:</b></br> " + dr["servico_executado_funilaria"].ToString();
                    CarregarPecasFunilaria(idOS);




                }
            }
        }
        private void CarregarPecasMecanicas(string idOS)
        {
            using (SqlConnection conn = new SqlConnection(
                WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                string sql = @"
                SELECT
                    descricao,
                    quant,
                    cracha,
                    nome,
                    inicio,
                    termino,
                    tempo_minutos,
                    tipo
                FROM tbos_pecas
                WHERE id_os = @id_os AND tipo = 'Mecanica'";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id_os", idOS);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvPecasMecanica.DataSource = dt;
                gvPecasMecanica.DataBind();
            }
        }
        private void CarregarPecasEletricos(string idOS)
        {
            using (SqlConnection conn = new SqlConnection(
                WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                string sql = @"
                SELECT
                    descricao,
                    quant,
                    cracha,
                    nome,
                    inicio,
                    termino,
                    tempo_minutos,
                    tipo
                FROM tbos_pecas
                WHERE id_os = @id_os AND tipo = 'Eletrica'";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id_os", idOS);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvPecasEletrica.DataSource = dt;
                gvPecasEletrica.DataBind();
            }
        }
        private void CarregarPecasBorracharia(string idOS)
        {
            using (SqlConnection conn = new SqlConnection(
                WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                string sql = @"
                SELECT
                    descricao,
                    quant,
                    cracha,
                    nome,
                    inicio,
                    termino,
                    tempo_minutos,
                    tipo
                FROM tbos_pecas
                WHERE id_os = @id_os AND tipo = 'Borracharia'";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id_os", idOS);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvPecasBorracharia.DataSource = dt;
                gvPecasBorracharia.DataBind();
            }
        }
        private void CarregarPecasFunilaria(string idOS)
        {
            using (SqlConnection conn = new SqlConnection(
                WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                string sql = @"
                SELECT
                    descricao,
                    quant,
                    cracha,
                    nome,
                    inicio,
                    termino,
                    tempo_minutos,
                    tipo
                FROM tbos_pecas
                WHERE id_os = @id_os AND tipo = 'Funilaria'";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id_os", idOS);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvPecasFunilaria.DataSource = dt;
                gvPecasFunilaria.DataBind();
            }
        }

        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            string idOS = lblOS.Text
                .Replace("<b>Nº O.S.:</b></br>", "")
                .Trim();

            string script = $"window.open('ImprimirOS.aspx?id={idOS}','_blank');";

            ScriptManager.RegisterStartupScript(
                this,
                GetType(),
                "AbrirPDF",
                script,
                true);
        }


        protected void gvOS_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string status = DataBinder.Eval(e.Row.DataItem, "status_texto").ToString();

                // Encontrar controles
                Label lblStatus = (Label)e.Row.FindControl("lblStatus");
                System.Web.UI.WebControls.Image imgStatus = (System.Web.UI.WebControls.Image)e.Row.FindControl("imgStatus");

                if (lblStatus != null && imgStatus != null)
                {
                    switch (status)
                    {
                        case "Aberta":
                            lblStatus.ForeColor = System.Drawing.Color.Green;
                            imgStatus.ImageUrl = "~/img/os_aberta.png"; // coloque seu ícone
                            imgStatus.ToolTip = "Aberta";
                            break;

                        case "Finalizada":
                            lblStatus.ForeColor = System.Drawing.Color.Orange;
                            imgStatus.ImageUrl = "~/img/os_finalizada.png";
                            imgStatus.ToolTip = "Finalizada";
                            break;

                        case "Cancelada":
                            lblStatus.ForeColor = System.Drawing.Color.Red;
                            imgStatus.ImageUrl = "~/img/os_cancelada.png";
                            imgStatus.ToolTip = "Cancelada";
                            break;
                    }
                }
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // 1. Obtém o valor, converte para string e remove espaços em branco (Trim)
                // Usamos ToUpper para garantir que "finalizada", "Finalizada" ou "FINALIZADA" funcionem
                string status = DataBinder.Eval(e.Row.DataItem, "status_texto")?.ToString().Trim().ToUpper() ?? "";

                LinkButton btnFinalizar = (LinkButton)e.Row.FindControl("btnFinalizar");
                Label lblStatus = (Label)e.Row.FindControl("lblStatus");
                System.Web.UI.WebControls.Image imgStatus = (System.Web.UI.WebControls.Image)e.Row.FindControl("imgStatus");

                if (btnFinalizar != null)
                {
                    // 2. Verificação múltipla para garantir a captura do status
                    if (status == "FINALIZADA" || status == "FINALIZADO")
                    {
                        // Desativa a funcionalidade de clique no servidor
                        btnFinalizar.Enabled = false;

                        // Força o visual de desabilitado do Bootstrap (cinza e sem eventos de mouse)
                        btnFinalizar.CssClass = "btn btn-secondary btn-sm disabled";
                        btnFinalizar.Attributes.Add("style", "pointer-events: none; cursor: default;");

                        // Limpa o CommandName para garantir que o RowCommand não processe nada
                        btnFinalizar.CommandName = "";
                        btnFinalizar.Text = "Concluída";
                    }
                }

                // Aproveitando para definir a imagem do status conforme prometido
                if (imgStatus != null)
                {
                    if (status == "FINALIZADA" || status == "FINALIZADO")
                    {
                        imgStatus.ImageUrl = "~/img/os_finalizada.png"; // Altere para o seu caminho real
                        if (lblStatus != null) lblStatus.ForeColor = System.Drawing.Color.Green;
                    }
                    else
                    {
                        imgStatus.ImageUrl = "~/img/os_aberta.png"; // Altere para o seu caminho real
                    }
                }
            }
        }

        public string CorDias(int dias)
        {
            if (dias <= 1)
                return "badge bg-success";   // verde

            if (dias <= 3)
                return "badge bg-warning";   // amarelo

            return "badge bg-danger";       // vermelho
        }

        protected void btnAbrirOs_Click(object sender, EventArgs e)
        {
            Response.Redirect("AbrirOs.aspx");
        }

        protected void txtFrotaPlaca_TextChanged(object sender, EventArgs e)
        {
            CarregarGrid();
        }

        protected void txtOrdem_Servico_TextChanged(object sender, EventArgs e)
        {
            CarregarGrid();
        }

        protected void txtCrachaMotorista_TextChanged(object sender, EventArgs e)
        {
            CarregarGrid();
        }

        protected void txtFornecedor_TextChanged(object sender, EventArgs e)
        {
            CarregarGrid();
        }

        protected void ddlStatus_TextChanged(object sender, EventArgs e)
        {
            CarregarGrid();
        }

        protected void ddlManutencao_TextChanged(object sender, EventArgs e)
        {
            CarregarGrid();
        }

        protected void ddlServico_TextChanged(object sender, EventArgs e)
        {
            CarregarGrid();
        }

        protected void txtDataInicial_TextChanged(object sender, EventArgs e)
        {
            CarregarGrid();
        }

        protected void txtDataFinal_TextChanged(object sender, EventArgs e)
        {
            CarregarGrid();
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

        protected void gvOS_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvOS.PageIndex = e.NewPageIndex;
            CarregarGrid();
        }
        protected void btnPrimeiro_Click(object sender, EventArgs e)
        {
            Session["Pagina"] = 1;
            CarregarGrid();
        }
        protected void btnAnterior_Click(object sender, EventArgs e)
        {
            int pagina = Session["Pagina"] != null
         ? Convert.ToInt32(Session["Pagina"])
         : 1;

            if (pagina > 1)
            {
                Session["Pagina"] = pagina - 1;
            }

            CarregarGrid();
        }
        protected void btnProximo_Click(object sender, EventArgs e)
        {
            int pagina = Session["Pagina"] != null
            ? Convert.ToInt32(Session["Pagina"])
            : 1;

            int total = Session["TotalPaginas"] != null
                ? Convert.ToInt32(Session["TotalPaginas"])
                : 1;

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
        protected void gvOS_RowCreated(object sender, GridViewRowEventArgs e)
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
    }
}