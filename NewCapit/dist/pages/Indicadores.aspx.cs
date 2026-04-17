using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.Script.Serialization;
using System.Web.Configuration;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using System.Drawing;
using System.Configuration;

namespace NewCapit.dist.pages
{
    public partial class Indicadores : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conexao"].ToString());

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) { CarregarEmpresas(); CarregaGraficos(); }
        }

        private void CarregarEmpresas()
        {
            DataTable dt = ExecuteQuery("SELECT DISTINCT descricao FROM tbempresa ORDER BY descricao");
            ddlEmpresa.DataSource = dt;
            ddlEmpresa.DataTextField = "descricao";
            ddlEmpresa.DataValueField = "descricao";
            ddlEmpresa.DataBind();
            ddlEmpresa.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Todas --", "0"));
        }

        protected void btnFiltrar_Click(object sender, EventArgs e) => CarregaGraficos();

        public void CarregaGraficos()
        {
            string dataIni = txtDataInicio.Text;
            string dataFim = txtDataFim.Text;
            string empresa = ddlEmpresa.SelectedValue;

            string filtro = " WHERE fl_exclusao IS NULL ";
            if (!string.IsNullOrEmpty(dataIni)) filtro += $" AND emissao >= '{dataIni} 00:00:00'";
            if (!string.IsNullOrEmpty(dataFim)) filtro += $" AND emissao <= '{dataFim} 23:59:59'";
            if (empresa != "0") filtro += $" AND nucleo = '{empresa}'";

            // --- CARDS ---
            litRealizadas.Text = ExecuteScalar($"SELECT COUNT(*) FROM tbcargas {filtro} AND andamento='Entregue'");
            litPendentes.Text = ExecuteScalar($"SELECT COUNT(*) FROM tbcargas {filtro} AND andamento='Pendente'");
            litAndamento.Text = ExecuteScalar($"SELECT COUNT(*) FROM tbcargas {filtro} AND andamento='Em Andamento'");
            litAtrasadas.Text = ExecuteScalar($"SELECT COUNT(*) FROM tbcargas {filtro} AND GETDATE() > previsao AND andamento='Em Andamento'");

            // Faturamento Total (Card)
            decimal totalGeral = 0;
            decimal.TryParse(ExecuteScalar($"SELECT ISNULL(SUM(CAST(valor_total AS DECIMAL(18,2))), 0) FROM tbcargas {filtro}"), out totalGeral);
            litFaturamentoTotal.Text = totalGeral.ToString("C");

            // --- QUERIES PARA OS GRÁFICOS ---

            // Substitua a query do dtFatVeiculo por esta:
            DataTable dtFatVeiculo = ExecuteQuery($@"
                        SELECT TOP 10 
                            LTRIM(RTRIM(frota)) as frota, 
                            SUM(CAST(ISNULL(valor_total, 0) AS DECIMAL(18,2))) as Total 
                        FROM tbcargas 
                        {filtro} 
                        AND frota IS NOT NULL 
                        AND LTRIM(RTRIM(frota)) <> '' 
                        AND LTRIM(RTRIM(frota)) NOT LIKE '%[^0-9]%' 
                        GROUP BY LTRIM(RTRIM(frota)) 
                        ORDER BY Total DESC");

            // 7. Motoristas
            DataTable dtMotoristas = ExecuteQuery($"SELECT TOP 10 ISNULL(solicitante, 'N/I') as motorista, COUNT(*) as Qtd FROM tbcargas {filtro} GROUP BY solicitante ORDER BY Qtd DESC");

            // 6. Tipos
            DataTable dtVeiculo = ExecuteQuery($"SELECT tipo_veiculo, COUNT(*) as Qtd FROM tbcargas {filtro} GROUP BY tipo_veiculo");

            // 10. Rotas (Corrigido para rota_entrega)
            DataTable dtRotas = ExecuteQuery($"SELECT TOP 10 ISNULL(NULLIF(rota_entrega, ''), 'NÃO INFORMADA') as rota_entrega, COUNT(*) as Qtd FROM tbcargas {filtro} GROUP BY rota_entrega ORDER BY Qtd DESC");

            // 11. Cidades
            DataTable dtCidades = ExecuteQuery($"SELECT TOP 10 ciddestino, COUNT(*) as Qtd FROM tbcargas {filtro} GROUP BY ciddestino ORDER BY Qtd DESC");

            // 8 e 9. Clientes (Recebedor e Pagador)
            DataTable dtRecebedor = ExecuteQuery($"SELECT TOP 10 ISNULL(recebedor, 'N/I') as recebedor, COUNT(*) as Qtd FROM tbcargas {filtro} GROUP BY recebedor ORDER BY Qtd DESC");
            DataTable dtPagador = ExecuteQuery($"SELECT TOP 10 ISNULL(pagador, 'N/I') as pagador, COUNT(*) as Qtd FROM tbcargas {filtro} GROUP BY pagador ORDER BY Qtd DESC");

            // --- JAVASCRIPT ---
            StringBuilder sb = new StringBuilder();
            sb.Append("$(function () { ");
            sb.Append("function draw(id, cfg) { var ctx = document.getElementById(id); if(ctx) new Chart(ctx, cfg); } ");

            // Gráficos de Barra
            sb.Append($@"draw('chartFatVeiculo', {{ 
                    type: 'bar', 
                    data: {{ 
                        labels: [{string.Join(",", dtFatVeiculo.AsEnumerable().Select(r => $"'{r["frota"].ToString().Replace("'", "")}'"))}], 
                        datasets: [{{ 
                            label: 'Faturamento R$', 
                            data: [{string.Join(",", dtFatVeiculo.AsEnumerable().Select(r => Convert.ToDecimal(r["Total"]).ToString(System.Globalization.CultureInfo.InvariantCulture)))}], 
                            backgroundColor: '#007bff' 
                        }}] 
                    }}, 
                    options: {{ maintainAspectRatio: false }} 
                }});");
            sb.Append($@"draw('chartMotoristas', {{ type: 'bar', data: {{ labels: [{string.Join(",", dtMotoristas.AsEnumerable().Select(r => $"'{r["motorista"].ToString().Replace("'", "")}'"))}], datasets: [{{ label: 'Cargas', data: [{string.Join(",", dtMotoristas.AsEnumerable().Select(r => r["Qtd"]))}], backgroundColor: '#28a745' }}] }}, options: {{ maintainAspectRatio: false }} }});");

            // Gráficos Pizza/Rosca
            sb.Append($@"draw('chartTipoVeiculo', {{ type: 'pie', data: {{ labels: [{string.Join(",", dtVeiculo.AsEnumerable().Select(r => $"'{r["tipo_veiculo"]}'"))}], datasets: [{{ data: [{string.Join(",", dtVeiculo.AsEnumerable().Select(r => r["Qtd"]))}], backgroundColor: ['#17a2b8', '#ffc107', '#dc3545'] }}] }}, options: {{ maintainAspectRatio: false }} }});");
            sb.Append($@"draw('chartRotasFull', {{ type: 'doughnut', data: {{ labels: [{string.Join(",", dtRotas.AsEnumerable().Select(r => $"'{r["rota_entrega"].ToString().Replace("'", "")}'"))}], datasets: [{{ data: [{string.Join(",", dtRotas.AsEnumerable().Select(r => r["Qtd"]))}], backgroundColor: ['#fd7e14', '#20c997', '#001a35', '#adb5bd', '#3c8dbc'] }}] }}, options: {{ maintainAspectRatio: false }} }});");

            // Gráficos Horizontais (Cidades, Recebedor, Pagador)
            sb.Append($@"draw('chartCidades', {{ type: 'bar', data: {{ labels: [{string.Join(",", dtCidades.AsEnumerable().Select(r => $"'{r["ciddestino"].ToString().Replace("'", "")}'"))}], datasets: [{{ label: 'Qtd', data: [{string.Join(",", dtCidades.AsEnumerable().Select(r => r["Qtd"]))}], backgroundColor: '#6c757d' }}] }}, options: {{ indexAxis: 'y', maintainAspectRatio: false }} }});");
            sb.Append($@"draw('chartRecebedor', {{ type: 'bar', data: {{ labels: [{string.Join(",", dtRecebedor.AsEnumerable().Select(r => $"'{r["recebedor"].ToString().Replace("'", "")}'"))}], datasets: [{{ label: 'Cargas', data: [{string.Join(",", dtRecebedor.AsEnumerable().Select(r => r["Qtd"]))}], backgroundColor: '#6f42c1' }}] }}, options: {{ indexAxis: 'y', maintainAspectRatio: false }} }});");
            sb.Append($@"draw('chartPagador', {{ type: 'bar', data: {{ labels: [{string.Join(",", dtPagador.AsEnumerable().Select(r => $"'{r["pagador"].ToString().Replace("'", "")}'"))}], datasets: [{{ label: 'Cargas', data: [{string.Join(",", dtPagador.AsEnumerable().Select(r => r["Qtd"]))}], backgroundColor: '#e83e8c' }}] }}, options: {{ indexAxis: 'y', maintainAspectRatio: false }} }});");

            sb.Append(" });");
            ScriptManager.RegisterStartupScript(this, GetType(), "dashFull", sb.ToString(), true);
        }

        private DataTable ExecuteQuery(string sql)
        {
            DataTable dt = new DataTable();
            using (SqlDataAdapter adpt = new SqlDataAdapter(sql, con)) { adpt.Fill(dt); }
            return dt;
        }

        private string ExecuteScalar(string sql)
        {
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                if (con.State == ConnectionState.Closed) con.Open();
                var res = cmd.ExecuteScalar();
                con.Close();
                return res == null || res == DBNull.Value ? "0" : res.ToString();
            }
        }

        protected void lnkMapa_Click(object sender, EventArgs e)
        {
            Response.Redirect("MapaVeiculo.aspx");
        }

        //protected void ddlEmpresa_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    CarregaBloco();
        //}
    }
}