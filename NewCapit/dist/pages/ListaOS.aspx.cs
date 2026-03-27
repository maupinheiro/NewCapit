using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;

namespace NewCapit.dist.pages
{
    public partial class ListaOS : System.Web.UI.Page
    {
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
        }
        
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
        }

        public string CorDias(int dias)
        {
            if (dias <= 1)
                return "badge bg-success";   // verde

            if (dias <= 3)
                return "badge bg-warning";   // amarelo

            return "badge bg-danger";       // vermelho
        }
    }
}