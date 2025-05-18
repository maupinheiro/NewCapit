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
using DAL;
using NPOI.SS.Formula.Functions;

namespace NewCapit.dist.pages
{
    public partial class ConsultaColetasCNT : System.Web.UI.Page
    {
        public string pallet;
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
            string sql = @"SELECT carga, peso, status, cliorigem, clidestino, 
                          CONVERT(varchar, previsao, 103) AS previsao, situacao, rota, 
                          andamento, data_hora, veiculo, tipo_viagem, solicitacoes 
                          FROM tbcargas 
                          WHERE empresa = 'CNT' AND fl_exclusao IS NULL ";

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
                        string status = ddlStatus.SelectedItem.Text;
                        string veiculos = ddlVeiculos.SelectedItem.Text;

                        //// Verificando e convertendo as datas corretamente
                        //if (!string.IsNullOrWhiteSpace(dataInicial) &&
                        //    DateTime.TryParseExact(dataInicial, "dd/MM/yyyy hh:mm",
                        //                           CultureInfo.InvariantCulture, DateTimeStyles.None,
                        //                           out DateTime parsedDataInicial))
                        //{
                        //    dtInicial = parsedDataInicial;
                        //}

                        //if (!string.IsNullOrWhiteSpace(dataFinal) &&
                        //    DateTime.TryParseExact(dataFinal, "dd/MM/yyyy hh:mm",
                        //                           CultureInfo.InvariantCulture, DateTimeStyles.None,
                        //                           out DateTime parsedDataFinal))
                        //{
                        //    dtFinal = parsedDataFinal;
                        //}
                        
                        // Adicionando os filtros de data apenas se existirem valores válidos
                        if (txtInicioData.Text != string.Empty && txtFimData.Text!= string.Empty)
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
                        if (!string.IsNullOrEmpty(status) && status != "Selecione...")
                        {
                            sql += " AND status = @status";
                            cmd.Parameters.Add("@status", SqlDbType.VarChar, 50).Value = status;
                        }

                        // Filtrando veículo, se necessário
                        if (!string.IsNullOrEmpty(veiculos) && veiculos != "Selecione...")
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
                int colunaAtendimentoIndex = 3; // Ajustar conforme a posição real da coluna no GridView
                TableCell cell = e.Row.Cells[colunaAtendimentoIndex];
        
                DateTime previsao, dataHora;
                DateTime agora = DateTime.Now;

                if (DateTime.TryParse(previsaoStr, out previsao) && DateTime.TryParse(dataHoraStr, out dataHora))
                {
                    // Mantendo apenas a data para a comparação principal
                    DateTime dataPrevisao = previsao.Date;
                    DateTime dataHoraComparacao = new DateTime(dataPrevisao.Year, dataPrevisao.Month, dataPrevisao.Day, dataHora.Hour, dataHora.Minute, dataHora.Second);
            
                    if (dataHoraComparacao < agora && (status == "Concluído" || status == "PENDENTE"))
                    {
                        cell.Text = "Atrasado";
                        cell.BackColor = System.Drawing.Color.Red;
                        cell.ForeColor = System.Drawing.Color.White;
                    }
                    else if (dataHoraComparacao.Date == agora.Date && dataHoraComparacao.TimeOfDay <= agora.TimeOfDay && (status == "Concluído" || status == "PENDENTE"))
                    {
                        cell.Text = "No Prazo";
                        cell.BackColor = System.Drawing.Color.Green;
                        cell.ForeColor = System.Drawing.Color.White;
                    }
                    else if (dataHoraComparacao > agora && status == "Concluído")
                    {
                        cell.Text = "Antecipado";
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

        protected void gvProdutos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "AbrirModal")
            {
                string id = e.CommandArgument.ToString();

                string nomeUsuario = Session["UsuarioLogado"].ToString();

                DataTable dt = ConCargas.FetchDataTableColetas2(id);
                using (SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
                {
                    if (dt.Rows.Count > 0)
                    {
                        DataRow row = dt.Rows[0];
                        SqlDataAdapter adpto = new SqlDataAdapter("SELECT nomcli, cidcli, estcli, codcli FROM tbclientes WHERE codvw = @codvw", con);
                        adpto.SelectCommand.Parameters.AddWithValue("@codvw", row["codvworigem"].ToString());
                        DataTable dto = new DataTable();
                        adpto.Fill(dto);

                        SqlDataAdapter adptd = new SqlDataAdapter("SELECT nomcli, cidcli, estcli, codcli FROM tbclientes WHERE codvw = @codvw", con);
                        adptd.SelectCommand.Parameters.AddWithValue("@codvw", row["codvwdestino"].ToString());
                        DataTable dtd = new DataTable();
                        adptd.Fill(dtd);



                        ddlSolicitante.Text = row["solicitante"].ToString();
                        txtCodCliOrigem.Text = dto.Rows[0][3].ToString();
                        lblRemetente.Text = dto.Rows[0][0].ToString();
                        txtMunicOrigem.Text = dto.Rows[0][1].ToString();
                        txtUFOrigem.Text = dto.Rows[0][2].ToString();

                        txtCodCliDestino.Text = dtd.Rows[0][3].ToString();
                        ddlDestinatario.Text = dtd.Rows[0][0].ToString();
                        txtMunicDestinatario.Text = dtd.Rows[0][1].ToString();
                        txtUFDestinatario.Text = dtd.Rows[0][2].ToString();

                        lblDataColeta.Text = row["emissao"].ToString();
                        lblSolicitacoes.Text = row["solicitacoes"].ToString();
                        txtPeso.Text = row["peso"].ToString();
                        lblMetragem.Text = row["pedidos"].ToString();

                        lblTipoViagem.Text = row["tipo_viagem"].ToString();
                        txtRota.Text = row["rota"].ToString();
                        txtEstudoRota.Text = row["estudo_rota"].ToString();
                        txtRemessa.Text = row["remessa"].ToString();

                        pallet = row["quant_palet"].ToString();






                        // DropDownLists (exemplo com dados fictícios)

                        cbFiliais.Text = row["empresa"].ToString();



                        ddlTomador.Text = row["tomador"].ToString();
                        //ddlPlanta.Items.Add(new ListItem("Planta A", "A"));
                        //ddlPlanta.Items.Add(new ListItem("Planta B", "B"));


                        txtGr.Text = row["veiculo"].ToString();

                    }
                }

                // Exibe o modal
                mpeModal.Show();
            }
        }
        public void CarregaDados(string id)
        {
            string nomeUsuario = Session["UsuarioLogado"].ToString();
           
            DataTable dt = ConCargas.FetchDataTableColetas2(id);
            using (SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    SqlDataAdapter adpto = new SqlDataAdapter("SELECT nomcli, cidcli, estcli, codcli FROM tbclientes WHERE codvw = @codvw", con);
                    adpto.SelectCommand.Parameters.AddWithValue("@codvw", row["codvworigem"].ToString());
                    DataTable dto = new DataTable();
                    adpto.Fill(dto);

                    SqlDataAdapter adptd = new SqlDataAdapter("SELECT nomcli, cidcli, estcli, codcli FROM tbclientes WHERE codvw = @codvw", con);
                    adptd.SelectCommand.Parameters.AddWithValue("@codvw", row["codvwdestino"].ToString());
                    DataTable dtd = new DataTable();
                    adptd.Fill(dtd);



                    ddlSolicitante.Text = row["solicitante"].ToString();
                    txtCodCliOrigem.Text = dto.Rows[0][3].ToString();
                    lblRemetente.Text = dto.Rows[0][0].ToString();
                    txtMunicOrigem.Text = dto.Rows[0][1].ToString();
                    txtUFOrigem.Text = dto.Rows[0][2].ToString();

                    txtCodCliDestino.Text = dtd.Rows[0][3].ToString();
                    ddlDestinatario.Text = dtd.Rows[0][0].ToString();
                    txtMunicDestinatario.Text = dtd.Rows[0][1].ToString();
                    txtUFDestinatario.Text = dtd.Rows[0][2].ToString();

                    lblDataColeta.Text = row["emissao"].ToString();
                    lblSolicitacoes.Text = row["solicitacoes"].ToString();
                    txtPeso.Text = row["peso"].ToString();
                    lblMetragem.Text = row["pedidos"].ToString();

                    lblTipoViagem.Text = row["tipo_viagem"].ToString();
                    txtRota.Text = row["rota"].ToString();
                    txtEstudoRota.Text = row["estudo_rota"].ToString();
                    txtRemessa.Text = row["remessa"].ToString();

                    pallet = row["quant_palet"].ToString();






                    // DropDownLists (exemplo com dados fictícios)

                    cbFiliais.Text = row["empresa"].ToString();



                    ddlTomador.Text = row["tomador"].ToString();
                    //ddlPlanta.Items.Add(new ListItem("Planta A", "A"));
                    //ddlPlanta.Items.Add(new ListItem("Planta B", "B"));


                    txtGr.Text = row["veiculo"].ToString();

                }
            }
            

        }


    }

}