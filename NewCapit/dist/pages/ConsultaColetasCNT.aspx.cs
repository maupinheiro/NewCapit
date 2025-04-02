using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Web.Script.Serialization;
using System.Configuration;

namespace NewCapit.dist.pages
{
    public partial class ConsultaColetasCNT : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                PreencherComboStatus();
                PreencherColetas();
                PreencherComboVeiculos();
            }
            
        }
        private void PreencherComboStatus()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT cod_status, ds_status FROM tb_status";

            // Crie uma conexão com o banco de dados
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                try
                {
                    // Abra a conexão com o banco de dados
                    conn.Open();

                    // Crie o comando SQL
                    SqlCommand cmd = new SqlCommand(query, conn);

                    // Execute o comando e obtenha os dados em um DataReader
                    SqlDataReader reader = cmd.ExecuteReader();

                    // Preencher o ComboBox com os dados do DataReader
                    ddlStatus.DataSource = reader;
                    ddlStatus.DataTextField = "ds_status";  // Campo que será mostrado no ComboBox
                    ddlStatus.DataValueField = "cod_status";  // Campo que será o valor de cada item                    
                    ddlStatus.DataBind();  // Realiza o binding dos dados                   
                    ddlStatus.Items.Insert(0, new ListItem("Selecione...", "0"));
                    // Feche o reader
                    reader.Close();
                }
                catch (Exception ex)
                {
                    // Trate exceções
                    Response.Write("Erro: " + ex.Message);
                }
            }
        }
        private void PreencherComboVeiculos()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT ID, descricao FROM tbtiposveiculoscnt ORDER BY descricao";

            // Crie uma conexão com o banco de dados
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                try
                {
                    // Abra a conexão com o banco de dados
                    conn.Open();

                    // Crie o comando SQL
                    SqlCommand cmd = new SqlCommand(query, conn);

                    // Execute o comando e obtenha os dados em um DataReader
                    SqlDataReader reader = cmd.ExecuteReader();

                    // Preencher o ComboBox com os dados do DataReader
                    ddlVeiculos.DataSource = reader;
                    ddlVeiculos.DataTextField = "descricao";  // Campo que será mostrado no ComboBox
                    ddlVeiculos.DataValueField = "id";  // Campo que será o valor de cada item                    
                    ddlVeiculos.DataBind();  // Realiza o binding dos dados                   
                    ddlVeiculos.Items.Insert(0, new ListItem("Selecione...", "0"));
                    // Feche o reader
                    reader.Close();
                }
                catch (Exception ex)
                {
                    // Trate exceções
                    Response.Write("Erro: " + ex.Message);
                }
            }
        }
        private void PreencherColetas()
        {

            var dataTable = DAL.ConCargas.FetchDataTableColetas();
            if (dataTable.Rows.Count <= 0)
            {
                return;
            }
            gvListCargas.DataSource = dataTable;
            gvListCargas.DataBind();

            //gvListCargas.UseAccessibleHeader = true;
            //gvListCargas.HeaderRow.TableSection = TableRowSection.TableHeader;
            //gvListCargas.FooterRow.TableSection = TableRowSection.TableFooter;
        }

        protected void lnkPesquisar_Click(object sender, EventArgs e)
        {
            string sql = @"SELECT id, peso, status, cliorigem, clidestino, CONVERT(varchar, previsao, 103) AS previsao, situacao,rota, andamento, data_hora, veiculo, tipo_viagem, solicitacoes FROM tbcargas WHERE empresa = 'CNT' and fl_exclusao is null ";

            using (SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                //try
                //{
                    con.Open();

                    using (var cmd = con.CreateCommand())
                    {
                        DateTime? dtInicial = null;
                        DateTime? dtFinal = null;

                        string dataInicial = txtInicioData.Text.Trim();
                        string dataFinal = txtFimData.Text.Trim();
                        string status = ddlStatus.SelectedItem.Text;
                        string veiculos = ddlVeiculos.SelectedItem.Text;

                        if (!string.IsNullOrWhiteSpace(dataInicial) && DateTime.TryParse(dataInicial, out DateTime parsedDataInicial))
                        {
                            dtInicial = parsedDataInicial;
                        }

                        if (!string.IsNullOrWhiteSpace(dataFinal) && DateTime.TryParse(dataFinal, out DateTime parsedDataFinal))
                        {
                            dtFinal = parsedDataFinal;
                        }

                        if (dtInicial.HasValue && dtFinal.HasValue)
                        {
                            sql += " AND previsao BETWEEN @dataInicial AND @dataFinal";
                            cmd.Parameters.Add("@dataInicial", SqlDbType.DateTime).Value = dtInicial.Value.ToString("yyyy-MM-dd");
                            cmd.Parameters.Add("@dataFinal", SqlDbType.DateTime).Value = dtFinal.Value.ToString("yyyy-MM-dd");
                        }
                        else if (dtInicial.HasValue)
                        {
                            sql += " AND previsao >= @dataInicial";
                            cmd.Parameters.Add("@dataInicial", SqlDbType.DateTime).Value = dtInicial.Value.ToString("yyyy-MM-dd");
                        }
                        else if (dtFinal.HasValue)
                        {
                            sql += " AND previsao <= @dataFinal";
                            cmd.Parameters.Add("@dataFinal", SqlDbType.DateTime).Value = dtFinal.Value.ToString("yyyy-MM-dd");
                        }

                        if (!string.IsNullOrEmpty(status) && status != "Selecione...") // Supondo que "0" seja a opção "Todos"
                        {
                            sql += " AND status = @status";
                            cmd.Parameters.Add("@status", SqlDbType.VarChar, 50).Value = status;
                        }
                        if (!string.IsNullOrEmpty(veiculos) && veiculos != "Selecione...") // Supondo que "0" seja a opção "Todos"
                        {
                            sql += " AND veiculo = @veiculo";
                            cmd.Parameters.Add("@veiculo", SqlDbType.VarChar, 50).Value = veiculos;
                        }

                    cmd.CommandText = sql;

                        using (var reader = cmd.ExecuteReader())
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Load(reader);
                            gvListCargas.DataSource = dataTable;
                            gvListCargas.DataBind();
                        }
                    }
                //}
                //catch (Exception ex)
                //{
                //    //lblMensagem.Text = "Erro ao buscar cargas: " + ex.Message;
                //    //lblMensagem.ForeColor = System.Drawing.Color.Red;
                //}
            }
        }

        protected void gvListCargas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvListCargas.PageIndex = e.NewPageIndex;
            PreencherColetas();  // Método para recarregar os dados no GridView
        }
        protected void gvListCargas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Obtendo os valores das colunas
                string previsaoStr = DataBinder.Eval(e.Row.DataItem, "previsao")?.ToString();
                string status = DataBinder.Eval(e.Row.DataItem, "status")?.ToString();

                // Índice da célula correspondente à coluna "ATENDIMENTO" (ajuste conforme necessário)
                int colunaAtendimentoIndex = 3; // Alterar conforme a posição real da coluna no GridView

                DateTime previsao;
                if (DateTime.TryParse(previsaoStr, out previsao))
                {
                    DateTime hoje = DateTime.Now.Date;
                    TableCell cell = e.Row.Cells[colunaAtendimentoIndex]; // Obtém a célula da coluna "ATENDIMENTO"

                    if (previsao < hoje && (status == "Concluído" || status == "Pendente"))
                    {
                        cell.Text = "Atrasado";
                        cell.BackColor = System.Drawing.Color.Red; // Fundo vermelho
                        cell.ForeColor = System.Drawing.Color.White; // Texto branco
                    }
                    else if (previsao == hoje && status == "Concluído")
                    {
                        cell.Text = "No Prazo";
                        cell.BackColor = System.Drawing.Color.Green; // Fundo verde
                        cell.ForeColor = System.Drawing.Color.White; // Texto branco
                    }
                    else if (previsao > hoje && status == "Concluído")
                    {
                        cell.Text = "Antecipado";
                        cell.BackColor = System.Drawing.Color.Orange; // Fundo laranja
                        cell.ForeColor = System.Drawing.Color.White; // Texto branco
                    }
                }
            }
        }





    }

}