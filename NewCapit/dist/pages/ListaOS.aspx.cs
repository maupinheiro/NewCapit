using DAL;
using DocumentFormat.OpenXml.Office.Word;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static NewCapit.dist.pages.AbrirOS;

namespace NewCapit.dist.pages
{
    public partial class ListaOS : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conexao"].ToString());
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ListarOS();
            }
        }
        private void ListarOS()
        {
            using (SqlConnection conn = new SqlConnection(
 ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                //DATEDIFF(DAY, o.data_abertura, GETDATE()) AS dias_aberta, 

                string sql = @"SELECT 
                o.id_os,
                o.placa,
                o.tipo_veiculo,
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
                DATEDIFF(DAY, o.data_abertura,
                    CASE 
                        WHEN o.data_fechamento IS NOT NULL THEN o.data_fechamento
                        ELSE GETDATE()
                    END
                ) AS dias_aberta,
                CASE 
                    WHEN o.status = '1' THEN 'Aberta'
                    WHEN o.status = '2' THEN 'Finalizada'
                    WHEN o.status = '3' THEN 'Cancelada'
                END AS status_texto
                FROM tbordem_servico o
                where (
                    @pesquisa IS NULL
                    OR o.placa LIKE '%' + @pesquisa + '%'
                    OR o.nome_motorista LIKE '%' + @pesquisa + '%'
                    OR o.nome_fornecedor LIKE '%' + @pesquisa + '%'
                    OR o.status LIKE '%' + @pesquisa + '%'
                    OR o.tipo_os LIKE '%' + @pesquisa + '%'
                    OR o.interno_externo LIKE '%' + @pesquisa + '%' 
                    OR o.nucleo_veiculo LIKE '%' + @pesquisa + '%'   
                    OR CAST(o.id_os AS VARCHAR) LIKE '%' + @pesquisa + '%'
                    OR CAST(o.id_veiculo AS VARCHAR) LIKE '%' + @pesquisa + '%'
                    OR CAST(o.id_carreta AS VARCHAR) LIKE '%' + @pesquisa + '%'
                    OR CAST(o.tipo_veiculo AS VARCHAR) LIKE '%' + @pesquisa + '%'
                    )

                    AND (@status IS NULL OR o.status = @status)                    

                    AND (@dataInicial IS NULL OR o.data_abertura >= @dataInicial)

                    AND (@dataFinal IS NULL OR o.data_abertura <= @dataFinal)

                    ORDER BY o.id_os DESC";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@pesquisa",
                string.IsNullOrWhiteSpace(txtPesquisa.Text) ? (object)DBNull.Value : txtPesquisa.Text);

                cmd.Parameters.AddWithValue("@status",
                string.IsNullOrWhiteSpace(ddlStatus.SelectedValue) ? (object)DBNull.Value : ddlStatus.SelectedValue);

                cmd.Parameters.AddWithValue("@ordem_servico",
                string.IsNullOrWhiteSpace(txtOrdem_Servico.Text) ? (object)DBNull.Value : txtOrdem_Servico.Text);

                cmd.Parameters.AddWithValue("@dataInicial",
                string.IsNullOrWhiteSpace(txtDataInicial.Text) ? (object)DBNull.Value : txtDataInicial.Text);

                cmd.Parameters.AddWithValue("@dataFinal",
                string.IsNullOrWhiteSpace(txtDataFinal.Text) ? (object)DBNull.Value : txtDataFinal.Text);


                conn.Open();

                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                da.Fill(dt);

                gvOS.DataSource = dt;
                gvOS.DataBind();
            }
        }
       
        protected void PesquisarOS(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(
                System.Configuration.ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                string sql = @"
        SELECT 
            o.id_os,
            o.placa,
            o.tipo_veiculo,
            o.nome_fornecedor,
            CASE WHEN o.tipo_veiculo = 'CARRETA' THEN o.id_carreta ELSE o.id_veiculo END AS codigo_veiculo,
            CASE WHEN o.interno_externo = 'I' THEN 'Interno'
                 WHEN o.interno_externo = 'E' THEN 'Externo' END AS servico_interno_externo,
            CASE WHEN o.tipo_os = 'C' THEN 'Corretiva'
                 WHEN o.tipo_os = 'P' THEN 'Preventiva' END AS os_preventiva_corretiva,
            o.nucleo_veiculo,
            o.nome_motorista,
            o.tipo_os,
            o.data_abertura,
            DATEDIFF(DAY, o.data_abertura, GETDATE()) AS dias_aberta,
            CASE 
                WHEN o.status = '1' THEN 'Aberta'
                WHEN o.status = '2' THEN 'Finalizada'
                WHEN o.status = '3' THEN 'Cancelada'
            END AS status_texto
        FROM tbordem_servico o
        WHERE (
            @pesquisa IS NULL
            OR o.placa LIKE '%' + @pesquisa + '%'
            OR o.nome_motorista LIKE '%' + @pesquisa + '%'
            OR o.nome_fornecedor LIKE '%' + @pesquisa + '%'
            OR o.nucleo_veiculo LIKE '%' + @pesquisa + '%'             
            OR CAST(o.id_veiculo AS VARCHAR) LIKE '%' + @pesquisa + '%'
            OR CAST(o.id_carreta AS VARCHAR) LIKE '%' + @pesquisa + '%'
            OR CAST(o.tipo_veiculo AS VARCHAR) LIKE '%' + @pesquisa + '%'
        )
        AND (@status IS NULL OR o.status = @status)
        AND (@interno_externo IS NULL OR o.interno_externo = @interno_externo)
        AND (CAST(o.id_os AS VARCHAR) = @os_id)
        AND (@tipo_os IS NULL OR o.tipo_os = @tipo_os)
        AND (@dataInicial IS NULL OR CAST(o.data_abertura AS DATE) >= @dataInicial)
        AND (@dataFinal IS NULL OR CAST(o.data_abertura AS DATE) <= @dataFinal)
        ORDER BY o.id_os DESC
        ";

                SqlCommand cmd = new SqlCommand(sql, conn);

                // Filtros
                cmd.Parameters.AddWithValue("@pesquisa",
                    string.IsNullOrWhiteSpace(txtPesquisa.Text) ? (object)DBNull.Value : txtPesquisa.Text.Trim());
                cmd.Parameters.AddWithValue("@status",
                    string.IsNullOrWhiteSpace(ddlStatus.SelectedValue) ? (object)DBNull.Value : ddlStatus.SelectedValue);
                cmd.Parameters.AddWithValue("@tipo_os",
                    string.IsNullOrWhiteSpace(ddlManutencao.SelectedValue) ? (object)DBNull.Value : ddlManutencao.SelectedValue);
                cmd.Parameters.AddWithValue("@interno_externo",
                    string.IsNullOrWhiteSpace(ddlServico.SelectedValue) ? (object)DBNull.Value : ddlServico.SelectedValue);
                cmd.Parameters.AddWithValue("@os_id",
                    string.IsNullOrWhiteSpace(txtOrdem_Servico.Text) ? (object)DBNull.Value : txtOrdem_Servico.Text.Trim());

                // Datas (somente data)
                DateTime dataInicial, dataFinal;
                object paramDataInicial = DBNull.Value;
                object paramDataFinal = DBNull.Value;

                if (!string.IsNullOrWhiteSpace(txtDataInicial.Text))
                {
                    if (DateTime.TryParseExact(txtDataInicial.Text, "dd/MM/yyyy",
                        new CultureInfo("pt-BR"), DateTimeStyles.None, out dataInicial))
                    {
                        paramDataInicial = dataInicial.Date;
                    }
                }

                if (!string.IsNullOrWhiteSpace(txtDataFinal.Text))
                {
                    if (DateTime.TryParseExact(txtDataFinal.Text, "dd/MM/yyyy",
                        new CultureInfo("pt-BR"), DateTimeStyles.None, out dataFinal))
                    {
                        paramDataFinal = dataFinal.Date;
                    }
                }

                cmd.Parameters.Add("@dataInicial", SqlDbType.Date).Value = paramDataInicial;
                cmd.Parameters.Add("@dataFinal", SqlDbType.Date).Value = paramDataFinal;

                // Executar
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Nenhuma OS encontrada.');", true);
                }

                gvOS.DataSource = dt;
                gvOS.DataBind();
            }
        }
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

                // 2. Busca os dados que acabaram de ser gravados na tabela
                string sqlBusca = "SELECT * FROM tbordem_servico WHERE id_os = @id"; // Ajuste 'id_os' para o nome da sua PK

                // Garante que a conexão está aberta para o segundo comando
                if (con.State == ConnectionState.Closed) con.Open();

                using (SqlCommand cmdBusca = new SqlCommand(sqlBusca, con))
                {
                    cmdBusca.Parameters.AddWithValue("@id", numeroOS);

                    using (SqlDataReader dr = cmdBusca.ExecuteReader())
                    {
                        OrdemServico os = new OrdemServico();

                        if (dr.Read())
                        {
                            // --- Populando o objeto PDF com os campos da TABELA ---
                            os.numero_os = numeroOS;
                            os.data_abertura = Convert.ToDateTime(dr["data_abertura"]);
                            os.resp_abertura = dr["resp_abertura"].ToString();

                            // Veículo
                            os.id_veiculo = dr["id_veiculo"].ToString();
                           
                            os.placa = dr["placa"].ToString();
                            os.tipo_veiculo = dr["tipo_veiculo"].ToString();
                            os.marca = dr["marca"].ToString();
                            os.modelo = dr["modelo"].ToString();
                            os.ano_modelo = dr["ano_modelo"].ToString();
                            os.nucleo_veiculo = dr["nucleo_veiculo"].ToString();
                            os.km_abertura = dr["km_abertura"].ToString();

                            // Motorista
                            os.id_motorista = dr["id_motorista"].ToString();
                            os.nome_motorista = dr["nome_motorista"].ToString();
                            os.transp_motorista = dr["transp_motorista"].ToString();
                            os.nucleo_motorista = dr["nucleo_motorista"].ToString();

                            // Serviço
                            os.tipo_os = dr["tipo_os"].ToString();
                            os.tipo_servico = dr["interno_externo"].ToString();

                            // Fornecedor (Lógica: se nulo no banco, assume Interno)
                            if (dr["id_fornecedor"] == DBNull.Value)
                            {
                                os.id_fornecedor = "6424";
                                os.nome_fornecedor = "MANUTENÇÃO - INTERNA";
                            }
                            else
                            {
                                os.id_fornecedor = dr["id_fornecedor"].ToString();
                                os.nome_fornecedor = dr["nome_fornecedor"].ToString();
                            }

                            // Descrições
                            os.parte_mecanica = dr["parte_mecanica"].ToString();
                            os.parte_eletrica = dr["parte_eletrica"].ToString();
                            os.parte_borracharia = dr["parte_borracharia"].ToString();
                            os.parte_funilaria = dr["parte_funilaria"].ToString();

                            // 3. Gera o PDF após fechar o Reader (dentro do if para garantir que achou a OS)
                            dr.Close(); // Fechar o Reader antes de gerar o PDF para liberar a conexão

                            byte[] pdf = GeradorPDFOS.GerarPDF(os);

                            Response.Clear();
                            Response.ContentType = "application/pdf";
                            Response.AddHeader("content-disposition", "attachment;filename=OS_" + numeroOS + ".pdf");
                            Response.BinaryWrite(pdf);
                            Response.Flush();
                            HttpContext.Current.ApplicationInstance.CompleteRequest();
                        }
                    }
                }
            }
        }
        //protected void gvOS_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        string status = DataBinder.Eval(e.Row.DataItem, "status_texto").ToString();

        //        switch (status)
        //        {
        //            case "Aberta":
        //                e.Row.BackColor = System.Drawing.Color.LightGreen;
        //                break;
        //            case "Finalizada":
        //                e.Row.BackColor = System.Drawing.Color.Orange;
        //                break;
        //            case "Cancelada":
        //                e.Row.BackColor = System.Drawing.Color.Red;
        //                e.Row.ForeColor = System.Drawing.Color.White;
        //                break;
        //        }
        //    }
        //}

        protected void gvOS_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string status = DataBinder.Eval(e.Row.DataItem, "status_texto").ToString();

                // Encontrar controles
                Label lblStatus = (Label)e.Row.FindControl("lblStatus");
                Image imgStatus = (Image)e.Row.FindControl("imgStatus");

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
                Image imgStatus = (Image)e.Row.FindControl("imgStatus");

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


    }
}