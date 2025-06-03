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
using System.Globalization;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;

namespace NewCapit.dist.pages
{
    public partial class ConsultaColetasCNT : System.Web.UI.Page
    {
        string idCarga;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
               
                PreencherColetas();
                PreencherComboVeiculos();
                PreencherComboStatus();

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
            string sql = "SELECT id, carga, peso, status, cliorigem, clidestino, ";
                   sql += "CONVERT(varchar, previsao, 103) AS previsao, situacao, rota,  ";
                   sql += "andamento, data_hora, veiculo, tipo_viagem, solicitacoes ";
                   sql += "FROM tbcargas ";
                   sql += "WHERE empresa = 'CNT' AND fl_exclusao IS NULL ";

            using (SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                try
                {
                    con.Open();

                    using (var cmd = con.CreateCommand())
                    {
                        DateTime? dtInicial = null;
                        DateTime? dtFinal = null;

                        string dataInicial = txtInicioData.Text.Trim();
                        string dataFinal = txtFimData.Text.Trim();
                        string status = ddlStatus.SelectedItem.Text.Trim();
                        string veiculos = ddlVeiculos.SelectedItem.Text.Trim();

                        //// Verificando e convertendo as datas corretamente
                        if (!string.IsNullOrWhiteSpace(dataInicial) &&
                            DateTime.TryParseExact(dataInicial, "dd/MM/yyyy hh:mm",
                                                   CultureInfo.InvariantCulture, DateTimeStyles.None,
                                                   out DateTime parsedDataInicial))
                        {
                            dtInicial = parsedDataInicial;
                        }

                        if (!string.IsNullOrWhiteSpace(dataFinal) &&
                            DateTime.TryParseExact(dataFinal, "dd/MM/yyyy hh:mm",
                                                   CultureInfo.InvariantCulture, DateTimeStyles.None,
                                                   out DateTime parsedDataFinal))
                        {
                            dtFinal = parsedDataFinal;
                        }

                        // Adicionando os filtros de data apenas se existirem valores válidos
                        if (txtInicioData.Text != string.Empty && txtFimData.Text != string.Empty)
                        {


                            sql += " AND data_hora BETWEEN @dataInicial AND @dataFinal";
                            cmd.Parameters.AddWithValue("@dataInicial", DateTime.Parse(txtInicioData.Text).ToString("dd/MM/yyyy HH:mm"));
                            cmd.Parameters.AddWithValue("@dataFinal", DateTime.Parse(txtFimData.Text).ToString("dd/MM/yyyy HH:mm"));

                            //cmd.Parameters.Add("@dataInicial", SqlDbType.DateTime).Value = DateTime.Parse(txtInicioData.Text).ToString("dd/MM/yyyy");
                            //    cmd.Parameters.Add("@dataFinal", SqlDbType.DateTime).Value = DateTime.Parse(txtFimData.Text).ToString("dd/MM/yyyy");
                        }
                        else if (txtInicioData.Text != string.Empty)
                        {
                            sql += " AND data_hora >= @dataInicial";
                            cmd.Parameters.AddWithValue("@dataInicial", DateTime.Parse(txtInicioData.Text).ToString("dd/MM/yyyy HH:mm"));
                        }
                        else if (txtFimData.Text != string.Empty)
                        {
                            sql += " AND data_hora <= @dataFinal";
                            cmd.Parameters.AddWithValue("@dataFinal", DateTime.Parse(txtFimData.Text).ToString("dd/MM/yyyy HH:mm"));
                        }

                        // Filtrando status, se necessário
                        if (/*!string.IsNullOrEmpty(status) && */status != "Selecione...")
                        {
                            sql += " AND status = @status";
                            cmd.Parameters.Add("@status", SqlDbType.VarChar, 50).Value = status;
                        }

                        // Filtrando veículo, se necessário
                        if (/*!string.IsNullOrEmpty(veiculos) &&*/ veiculos != "Selecione...")
                        {
                            sql += " AND veiculo = @veiculo";
                            cmd.Parameters.Add("@veiculo", SqlDbType.VarChar, 50).Value = veiculos;
                        }

                        cmd.CommandText = sql;

                        // Executando a consulta e preenchendo o GridView
                        using (var reader = cmd.ExecuteReader())
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Load(reader);
                            gvListCargas.DataSource = dataTable;
                            gvListCargas.DataBind();
                        }

                    }
                }
                catch (Exception ex)
                {
                    //lblMensagem.Text = "Erro ao buscar cargas: " + ex.Message;
                    //lblMensagem.ForeColor = System.Drawing.Color.Red;
                }
            }
        }
        protected void gvListCargas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Obtendo os valores das colunas
                string previsaoStr = DataBinder.Eval(e.Row.DataItem, "previsao")?.ToString();
                string dataHoraStr = DataBinder.Eval(e.Row.DataItem, "data_hora")?.ToString();
                string status = DataBinder.Eval(e.Row.DataItem, "status")?.ToString();

                // Índice da célula correspondente à coluna "ATENDIMENTO" (ajuste conforme necessário)
                int colunaAtendimentoIndex = 4; // Ajustar conforme a posição real da coluna no GridView
                TableCell cell = e.Row.Cells[colunaAtendimentoIndex];

                DateTime previsao, dataHora;
                DateTime agora = DateTime.Now;

                if (DateTime.TryParse(previsaoStr, out previsao) && DateTime.TryParse(dataHoraStr, out dataHora))
                {
                    // Mantendo apenas a data para a comparação principal
                    DateTime dataPrevisao = previsao.Date;
                    DateTime dataHoraComparacao = new DateTime(dataPrevisao.Year, dataPrevisao.Month, dataPrevisao.Day, dataHora.Hour, dataHora.Minute, dataHora.Second);

                    if (dataHoraComparacao < agora && (status == "CONCLUIDO" || status == "PENDENTE"))
                    {
                        cell.Text = "ATRASADO";
                        cell.BackColor = System.Drawing.Color.Red;
                        cell.ForeColor = System.Drawing.Color.White;
                    }
                    else if (dataHoraComparacao.Date == agora.Date && dataHoraComparacao.TimeOfDay <= agora.TimeOfDay && (status == "CONCLUIDO" || status == "PENDENTE"))
                    {
                        cell.Text = "NO PRAZO";
                        cell.BackColor = System.Drawing.Color.Green;
                        cell.ForeColor = System.Drawing.Color.White;
                    }
                    else if (dataHoraComparacao > agora && status == "CONCLUIDO")
                    {
                        cell.Text = "ANTECIPADO";
                        cell.BackColor = System.Drawing.Color.Orange;
                        cell.ForeColor = System.Drawing.Color.White;
                    }
                }
            }
        }
        protected void Editar(object sender, EventArgs e)
        {
            using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
            {
                string id = gvListCargas.DataKeys[row.RowIndex].Value.ToString();

                Response.Redirect("/dist/pages/Frm_EditarColeta.aspx?id=" + id);
            }
        }
        protected void gvListCargas_RowCommand(object sender, GridViewCommandEventArgs e)
        {     
            if (e.CommandName == "MostrarDetalhes")
            {
                int id = Convert.ToInt32(e.CommandArgument);

                string connStr = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    // Tab 1
                    SqlCommand cmd1 = new SqlCommand("SELECT carga, empresa, tomador, veiculo, codorigem, codvworigem,cliorigem, cidorigem, ufcliorigem, coddestino, codvwdestino, clidestino, ciddestino, ufclidestino, data_hora, solicitacoes, rota, tipo_viagem, peso, pedidos, estudo_rota, remessa, quant_palet, andamento, observacao FROM tbcargas WHERE Id = @Id", conn);
                    cmd1.Parameters.AddWithValue("@Id", id);
                    SqlDataReader reader1 = cmd1.ExecuteReader();
                    if (reader1.Read())
                    {
                        // Tab 1 dados da coleta
                        txtColeta.Text = reader1["carga"].ToString();
                        txtFilial.Text = reader1["empresa"].ToString();
                        txtPlanta.Text = reader1["tomador"].ToString();
                        txtTipoVeiculo.Text = reader1["veiculo"].ToString();
                        txtCodCliOrigem.Text = reader1["codorigem"].ToString() + "/" + reader1["codvworigem"].ToString();
                        txtRemetente.Text = reader1["cliorigem"].ToString();
                        txtMunicOrigem.Text = reader1["cidorigem"].ToString();
                        txtUFOrigem.Text = reader1["ufcliorigem"].ToString();
                        txtCodCliDestino.Text = reader1["coddestino"].ToString() + "/" + reader1["codvwdestino"].ToString();
                        txtDestinatario.Text = reader1["clidestino"].ToString();
                        txtMunicDestinatario.Text = reader1["ciddestino"].ToString();
                        txtUFDestinatario.Text = reader1["ufclidestino"].ToString();
                        lblDataColeta.Text = reader1["data_hora"].ToString();
                        lblSolicitacoes.Text = reader1["solicitacoes"].ToString();
                        txtRota.Text = reader1["rota"].ToString();
                        lblTipoViagem.Text = reader1["tipo_viagem"].ToString();
                        txtPeso.Text = reader1["peso"].ToString();
                        lblMetragem.Text = reader1["pedidos"].ToString();
                        txtEstudoRota.Text = reader1["estudo_rota"].ToString();
                        txtRemessa.Text = reader1["remessa"].ToString();
                        quantPallet.Text = reader1["quant_palet"].ToString();
                       
                        if (reader1["andamento"].ToString() == "CONCLUIDO")
                        {
                            lblStatus.BackColor = System.Drawing.Color.LightGreen;
                            lblStatus.Text = reader1["andamento"].ToString();
                        }
                        else if (reader1["andamento"].ToString() == "PENDENTE")
                        {
                            lblStatus.BackColor = System.Drawing.Color.Yellow;
                            lblStatus.Text = reader1["andamento"].ToString();
                        }
                        else if (reader1["andamento"].ToString() == "ANDAMENTO")
                        {
                            lblStatus.BackColor = System.Drawing.Color.LightCoral;
                            lblStatus.Text = reader1["andamento"].ToString();
                        }
                        else
                        {
                            lblStatus.BackColor = System.Drawing.Color.Black;
                            lblStatus.Text = reader1["andamento"].ToString();
                        }
                        
                        // Tab 3 dados do carregamento
                        txtObservacao.Text = reader1["observacao"].ToString();
                        // Tab 4 Ocorrências
                        txtObservacao.Text = reader1["observacao"].ToString();
                        string idColeta = reader1["carga"].ToString();
                        reader1.Close();
                        
                        // Tab 2 dados do motorista e veiculo vindos da tabela tbcarregamentos
                        SqlCommand cmd2 = new SqlCommand("SELECT codtra, transportadora, tipoveiculo, veiculo, placa, reboque1, reboque2, codmotorista, nomemotorista, num_carregamento, nucleo, carreta, tipomot  FROM tbcarregamentos WHERE carga = @idColeta", conn);
                        cmd2.Parameters.AddWithValue("@idColeta", idColeta);
                        SqlDataReader readerCarregamento = cmd2.ExecuteReader();
                        if (readerCarregamento.Read())
                        {
                            txtOrdemColeta.Text = reader1["num_carregamento"].ToString();
                            txtFilialMot.Text = reader1["nucleo"].ToString();
                            txtTipoMot.Text = readerCarregamento["tipomot"].ToString();
                            txtCodMotorista.Text = readerCarregamento["codmotorista"].ToString();
                            txtNomMot.Text = readerCarregamento["nomemotorista"].ToString();
                            txtCodTra.Text = readerCarregamento["codtra"].ToString();
                            txtTransp.Text = readerCarregamento["transportadora"].ToString();
                            txtVeiculoTipo.Text = readerCarregamento["tipoveiculo"].ToString();
                            txtCodVeiculo.Text = readerCarregamento["veiculo"].ToString();
                            txtPlaca.Text = readerCarregamento["placa"].ToString();
                            txtReboque1.Text = readerCarregamento["reboque1"].ToString();
                            txtReboque2.Text = readerCarregamento["reboque2"].ToString();
                            txtCarreta.Text = readerCarregamento["carreta"].ToString();
                        }
                        else 
                        {
                            txtFilialMot.Text = "";
                            txtTipoMot.Text = "";
                            txtCodMotorista.Text = "";
                            txtNomMot.Text = "";
                            txtCodTra.Text = "";
                            txtTransp.Text = "";
                            txtVeiculoTipo.Text = "";
                            txtCodVeiculo.Text = "";
                            txtPlaca.Text = "";
                            txtReboque1.Text = "";
                            txtReboque2.Text = "";
                            txtCarreta.Text = "";

                        }
                        readerCarregamento.Close();


                        ////Exibir o modal                        
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#infoModal').modal('show');", true);
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#detalhesModal').modal('show');", true);
                    }
                }
               
            }
        }

        
    }
}