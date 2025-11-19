using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Configuration;
using System.Text;
using System.Web.Services.Description;

namespace NewCapit.dist.pages
{
    public partial class ConsultaColetasPopUp : System.Web.UI.Page
    {
        string mensagem;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PreencherComboResponsavel();

                CarregarGrid();


            }
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

        }

        private void CarregarGrid()
        {
            string connStr = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT id, carga, cva, atendimento, CONVERT(varchar, CAST(data_hora AS datetime), 103) + ' ' + CONVERT(varchar, CAST(data_hora AS datetime), 108) AS data_hora, cliorigem, clidestino, veiculo, tipo_viagem, solicitacoes,peso,pedidos, andamento FROM tbcargas WHERE andamento = 'PENDENTE' AND empresa = 'CNT (CC)'  AND ISDATE(data_hora) = 1\r\nORDER BY CAST(data_hora AS datetime)";
                SqlCommand command = new SqlCommand(query, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                GVColetas.DataSource = dataTable;
                GVColetas.DataBind();
            }
        }
        private void CarregarGridPesquisa(string searchTerm)
        {
            var sql = new StringBuilder(@"
                             SELECT id, carga, cva, atendimento, CONVERT(varchar, CAST(data_hora AS datetime), 103) + ' ' + CONVERT(varchar, CAST(data_hora AS datetime), 108) AS data_hora, cliorigem, clidestino, veiculo, tipo_viagem, solicitacoes, peso, pedidos, andamento FROM tbcargas WHERE andamento = 'PENDENTE' AND ISDATE(data_hora) = 1 ORDER BY CAST(data_hora AS datetime)");

            string connStr = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            using (var cmd = conn.CreateCommand())
            {
                // Filtro por searchTerm (opcional)
                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    sql.Append(@"
                AND (
                    data_hora LIKE @searchTerm OR 
                    solicitacoes LIKE @searchTerm OR 
                    veiculo LIKE @searchTerm OR 
                    tipo_viagem LIKE @searchTerm OR 
                    cliorigem LIKE @searchTerm OR 
                    clidestino LIKE @searchTerm OR 
                    cva LIKE @searchTerm
                )");

                    cmd.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");
                }
                // Finaliza e executa
                cmd.CommandText = sql.ToString();
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    GVColetas.DataSource = dt;
                    GVColetas.DataBind();
                    conn.Close();
                }
            }

        }
        protected void myInput_TextChanged(object sender, EventArgs e)
        {
            string searchTerm = myInput.Text.Trim();
            CarregarGridPesquisa(searchTerm);
        }
        //protected void timerAtualiza_Tick(object sender, EventArgs e)
        //{
        //    CarregarGrid();
        //}

        //protected void btnAtualizar_Click(object sender, EventArgs e)
        //{
        //    CarregarGrid();
        //}

        //private void PreencherComboResponsavel()
        //{
        //    // Consulta SQL que retorna os dados desejados
        //    string query = "SELECT id, descricao FROM tbresponsavelocorrencia ORDER BY descricao ASC";

        //    // Crie uma conexão com o banco de dados
        //    using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
        //    {
        //        try
        //        {
        //            // Abra a conexão com o banco de dados
        //            conn.Open();

        //            // Crie o comando SQL
        //            SqlCommand cmd = new SqlCommand(query, conn);

        //            // Execute o comando e obtenha os dados em um DataReader
        //            SqlDataReader reader = cmd.ExecuteReader();

        //            // Preencher o ComboBox com os dados do DataReader
        //            cboResponsavel.DataSource = reader;
        //            cboResponsavel.DataTextField = "descricao";  // Campo que será mostrado no ComboBox
        //            cboResponsavel.DataValueField = "id";  // Campo que será o valor de cada item                    
        //            cboResponsavel.DataBind();  // Realiza o binding dos dados                   
        //            cboResponsavel.Items.Insert(0, new ListItem("Selecione...", "0"));
        //            // Feche o reader
        //            reader.Close();
        //        }
        //        catch (Exception ex)
        //        {
        //            // Trate exceções
        //            Response.Write("Erro: " + ex.Message);
        //        }
        //    }
        //}

        //private void PreencherComboipoOcorrencia()
        //{
        //    // Consulta SQL que retorna os dados desejados
        //    string query = "SELECT id, descricao FROM tbtipodeocorrencias ORDER BY descricao ASC";

        //    // Crie uma conexão com o banco de dados
        //    using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
        //    {
        //        try
        //        {
        //            // Abra a conexão com o banco de dados
        //            conn.Open();

        //            // Crie o comando SQL
        //            SqlCommand cmd = new SqlCommand(query, conn);

        //            // Execute o comando e obtenha os dados em um DataReader
        //            SqlDataReader reader = cmd.ExecuteReader();

        //            // Preencher o ComboBox com os dados do DataReader
        //            cboMotivo.DataSource = reader;
        //            cboMotivo.DataTextField = "descricao";  // Campo que será mostrado no ComboBox
        //            cboMotivo.DataValueField = "id";  // Campo que será o valor de cada item                    
        //            cboMotivo.DataBind();  // Realiza o binding dos dados                   
        //            cboMotivo.Items.Insert(0, new ListItem("Selecione...", "0"));
        //            // Feche o reader
        //            reader.Close();
        //        }
        //        catch (Exception ex)
        //        {
        //            // Trate exceções
        //            Response.Write("Erro: " + ex.Message);
        //        }
        //    }
        //}

        //protected void btnSalvarOcorrencia_Click(object sender, EventArgs e)
        //{
        //    string connectionString = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
        //    int numColeta = int.Parse(lblColeta.Text.Trim());
        //    string responsavel = cboResponsavel.SelectedItem.ToString().Trim().ToUpper();
        //    string motivo = cboMotivo.SelectedItem.ToString().Trim().ToUpper();
        //    string ocorrencia = txtObservacao.Text.Trim();

        //    using (SqlConnection conn = new SqlConnection(connectionString))
        //    {
        //        string query = "INSERT INTO tbocorrencias (carga, responsavel, motivo, observacao, data, usuario_inclusao, data_inclusao) VALUES (@Carga, @Responsavel, @Motivo, @Observacao, GETDATE(), @Usuario_Inclusao, GETDATE())";
        //        SqlCommand cmd = new SqlCommand(query, conn);
        //        cmd.Parameters.AddWithValue("@Carga", numColeta);
        //        cmd.Parameters.AddWithValue("@Responsavel", responsavel);
        //        cmd.Parameters.AddWithValue("@Motivo", motivo);
        //        cmd.Parameters.AddWithValue("@Observacao", ocorrencia);
        //        cmd.Parameters.AddWithValue("@Usuario_Inclusao", Session["UsuarioLogado"].ToString());

        //        try
        //        {
        //            conn.Open();
        //            cmd.ExecuteNonQuery();
        //            // Opcional: limpar ou fechar modal                    
        //            ClientScript.RegisterStartupScript(this.GetType(), "HideModal", "hideModal();", true);
        //        }
        //        catch (Exception ex)
        //        {
        //            // Logar ou exibir erro
        //            Response.Write("<script>alert('Erro: " + ex.Message + "');</script>");
        //        }
        //    }
        //}

        protected void GVColetas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Ocorrencias")
            {
                int id = Convert.ToInt32(e.CommandArgument);

                string connStr = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    SqlCommand cmd1 = new SqlCommand("SELECT carga, cva, andamento FROM tbcargas WHERE carga = @Id", conn);
                    cmd1.Parameters.AddWithValue("@Id", id);
                    SqlDataReader reader1 = cmd1.ExecuteReader();
                    if (reader1.Read())
                    {
                        lblColeta.BackColor = System.Drawing.Color.LightGreen;
                        lblColeta.Text = reader1["carga"].ToString();

                        lblCVA.BackColor = System.Drawing.Color.Magenta;
                        lblCVA.ForeColor = System.Drawing.Color.White;
                        lblCVA.Text = reader1["cva"].ToString();

                        if (reader1["andamento"].ToString() == "CONCLUIDO")
                        {
                            lblStatus.BackColor = System.Drawing.Color.LightGreen;
                            lblStatus.Text = reader1["andamento"].ToString();
                        }
                        else if (reader1["andamento"].ToString() == "PENDENTE" || reader1["andamento"].ToString() == "Pendente")
                        {
                            lblStatus.BackColor = System.Drawing.Color.Yellow;
                            lblStatus.Text = reader1["andamento"].ToString();
                        }
                        else if (reader1["andamento"].ToString() == "EM ANDAMENTO")
                        {
                            lblStatus.BackColor = System.Drawing.Color.Purple;
                            lblCVA.ForeColor = System.Drawing.Color.White;
                            lblStatus.Text = reader1["andamento"].ToString();
                        }
                        else
                        {
                            lblStatus.BackColor = System.Drawing.Color.Black;
                            lblCVA.ForeColor = System.Drawing.Color.White;
                            lblStatus.Text = reader1["andamento"].ToString();
                        }

                        using (SqlConnection con = new SqlConnection(connStr))
                        {
                            string query = "SELECT id, responsavel, motivo, observacao, data_inclusao, usuario_inclusao FROM tbocorrencias WHERE carga = @numeroCarga";

                            SqlCommand cmd = new SqlCommand(query, con);
                            cmd.Parameters.AddWithValue("@numeroCarga", id);

                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            DataTable dt = new DataTable();
                            da.Fill(dt);

                            GridViewCarga.DataSource = dt;
                            GridViewCarga.DataBind();
                        }


                        // Exibe o modal com JavaScript
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "abrirModal", "$('#modalOcorrencia').modal('show');", true);

                    }
                }
            }
            if (e.CommandName == "Motoristas")
            {
                int id = Convert.ToInt32(e.CommandArgument);

                string connStr = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    SqlCommand cmd1 = new SqlCommand("SELECT cva,codvworigem from tbcargas WHERE carga = @Id", conn);
                    cmd1.Parameters.AddWithValue("@Id", id);
                    SqlDataReader reader1 = cmd1.ExecuteReader();
                    if (reader1.Read())
                    {
                        try
                        {
                            using (SqlConnection connection = new SqlConnection(connStr))
                            {
                                connection.Open();

                                // Adiciona um manipulador de eventos para capturar mensagens PRINT
                                // Renomeamos os parâmetros da lambda para 's' e 'args' para evitar conflito
                                connection.InfoMessage += (s, args) =>
                                {
                                    // Em ASP.NET, Console.WriteLine não é visível para o usuário final.
                                    // Você precisa atualizar um controle na página ou logar em algum lugar.
                                    // Exemplo usando um Label (lblMensagens) ou Literal (litMensagens):
                                    // lblMensagens.Text += "Mensagem do SQL Server: " + args.Message + "  

                                    // litMensagens.Text += "Mensagem do SQL Server: " + args.Message + "  


                                    // Para fins de depuração no Visual Studio, você ainda pode usar:
                                    System.Diagnostics.Debug.WriteLine("Mensagem do SQL Server: " + args.Message);
                                    mensagem = args.Message;

                                };

                                using (SqlCommand command = new SqlCommand("sp_AtualizarCVAIndividual", connection))
                                {
                                    command.Parameters.AddWithValue("@p_cva", reader1["cva"].ToString());
                                    command.Parameters.AddWithValue("@p_codvworigem", reader1["codvworigem"].ToString());
                                    command.CommandType = CommandType.StoredProcedure;
                                    command.ExecuteNonQuery();
                                }

                                string script = $"alert('{HttpUtility.JavaScriptStringEncode(mensagem)}');";
                                ClientScript.RegisterStartupScript(this.GetType(), "MotoristaDiferenteAlerta", script, true);
                                // lblMensagens.Text += " Procedure executada com sucesso!"; // Mensagem de sucesso
                            }
                        }
                        catch (SqlException ex)
                        {
                            // Captura erros específicos do SQL Server
                            // lblMensagens.Text += "    Erro no SQL Server: " + ex.Message;
                            System.Diagnostics.Debug.WriteLine("Erro no SQL Server: " + ex.Message);
                            foreach (SqlError error in ex.Errors)
                            {
                                string msg = $"Erro {error.Number}: {error.Message} (Linha: {error.LineNumber})";
                                string script = $"alert('{HttpUtility.JavaScriptStringEncode(msg)}');";
                                ClientScript.RegisterStartupScript(this.GetType(), "MotoristaDiferenteAlerta", script, true);
                            }
                        }
                        catch (Exception ex)
                        {
                            // Captura outros erros gerais
                            // lblMensagens.Text += "Ocorreu um erro inesperado: " + ex.Message;
                            string script = $"alert('{HttpUtility.JavaScriptStringEncode(ex.Message)}');";
                            ClientScript.RegisterStartupScript(this.GetType(), "MotoristaDiferenteAlerta", script, true);
                            //System.Diagnostics.Debug.WriteLine("Ocorreu um erro inesperado: " + ex.Message);
                        }

                        //using (SqlConnection con = new SqlConnection(connStr))
                        //{
                        //    string query = "SELECT id, responsavel, motivo, observacao, data_inclusao, usuario_inclusao FROM tbocorrencias WHERE carga = @numeroCarga";

                        //    SqlCommand cmd = new SqlCommand(query, con);
                        //    cmd.Parameters.AddWithValue("@numeroCarga", id);

                        //    SqlDataAdapter da = new SqlDataAdapter(cmd);
                        //    DataTable dt = new DataTable();
                        //    da.Fill(dt);

                        //    GridViewCarga.DataSource = dt;
                        //    GridViewCarga.DataBind();
                        //}


                        // Exibe o modal com JavaScript
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "abrirModal", "$('#modalOcorrencia').modal('show');", true);

                    }
                }
            }
        }

        protected void GVColetas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Obtendo os valores das colunas

                string dataHoraStr = DataBinder.Eval(e.Row.DataItem, "data_hora")?.ToString();

                string andamento = DataBinder.Eval(e.Row.DataItem, "andamento")?.ToString();
                string atendeu = DataBinder.Eval(e.Row.DataItem, "atendimento")?.ToString();
                // Verifica se os valores são nulos ou vazios
                if (/*string.IsNullOrEmpty(previsaoStr) ||*/ string.IsNullOrEmpty(dataHoraStr) /*|| string.IsNullOrEmpty(status)*/ || string.IsNullOrEmpty(andamento))
                {
                    return; // Se algum valor for nulo ou vazio, não faz nada
                }
                // Índice da célula correspondente à coluna "ATENDIMENTO" (ajuste conforme necessário)
                int colunaAtendimentoIndex = 4; // Ajustar conforme a posição real da coluna no GridView
                TableCell cell = e.Row.Cells[colunaAtendimentoIndex];

                DateTime previsao, dataHora;
                DateTime agora = DateTime.Now;

                if (andamento == "PENDENTE")
                {
                    if (/*DateTime.TryParse(previsaoStr, out previsao) &&*/ DateTime.TryParse(dataHoraStr, out dataHora))
                    {
                        // Mantendo apenas a data para a comparação principal
                        //DateTime dataPrevisao = previsao.Date;
                        DateTime dataHoraComparacao = new DateTime(dataHora.Year, dataHora.Month, dataHora.Day, dataHora.Hour, dataHora.Minute, dataHora.Second);

                        if (dataHoraComparacao < agora /* && (status == "CONCLUIDO" || status == "PENDENTE")*/)
                        {
                            cell.Text = "ATRASADO / " + andamento.ToUpper();
                            cell.BackColor = System.Drawing.Color.Red;
                            cell.ForeColor = System.Drawing.Color.White;
                        }
                        else if (dataHoraComparacao.Date == agora.Date && dataHoraComparacao.TimeOfDay <= agora.TimeOfDay /*&& (status == "CONCLUIDO" || status == "PENDENTE")*/)
                        {
                            cell.Text = "NO PRAZO / " + andamento.ToUpper();
                            cell.BackColor = System.Drawing.Color.Green;
                            cell.ForeColor = System.Drawing.Color.White;
                        }
                        else if (dataHoraComparacao > agora /*&& status == "CONCLUIDO"*/)
                        {
                            cell.Text = "NO PRAZO / " + andamento.ToUpper();
                            cell.BackColor = System.Drawing.Color.Orange;
                            cell.ForeColor = System.Drawing.Color.White;

                        }
                    }
                }
                else
                {
                    cell.Text = atendeu;
                    cell.BackColor = System.Drawing.Color.CadetBlue;
                    cell.ForeColor = System.Drawing.Color.White;
                }
            }
        }
        private void CarregarMotivoOcorrencias(string codigo)
        {
            string strConn = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                string query = "SELECT codigo, motivo FROM tbmotivoocorrenciacnt WHERE codigo_responsavel = @Cod_Responsavel";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Cod_Responsavel", codigo);
                conn.Open();
                cboMotivo.DataSource = cmd.ExecuteReader();
                cboMotivo.DataTextField = "motivo";
                cboMotivo.DataValueField = "codigo"; // valor único
                cboMotivo.DataBind();

                cboMotivo.Items.Insert(0, new ListItem("Selecione...", "0"));
            }
        }
        private void PreencherComboResponsavel()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT codigo, responsavel FROM tbresponsavelocorrenciacnt ORDER BY responsavel";

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
                    cboResponsavel.DataSource = reader;
                    cboResponsavel.DataTextField = "responsavel";  // Campo que será mostrado no ComboBox
                    cboResponsavel.DataValueField = "codigo";  // Campo que será o valor de cada item                    
                    cboResponsavel.DataBind();  // Realiza o binding dos dados                   
                    cboResponsavel.Items.Insert(0, new ListItem("Selecione...", "0"));
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
        protected void cboResponsavel_SelectedIndexChanged(object sender, EventArgs e)
        {
            string codigo = cboResponsavel.SelectedValue;
            CarregarMotivoOcorrencias(codigo);

            // Restaurar cidade se estiver em ViewState
            if (ViewState["MotivoSelecionado"] != null)
            {
                string motivoId = ViewState["MotivoSelecionado"].ToString();
                if (cboMotivo.Items.FindByValue(motivoId) != null)
                {
                    cboMotivo.SelectedValue = motivoId;
                }
            }
            // Reexibe o modal após postback
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModal", "$('#modalOcorrencia').modal('show');", true);

        }
        protected void btnSalvarOcorrencia_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
            int numCVA = int.Parse(lblCVA.Text.Trim());
            int numColeta = int.Parse(lblColeta.Text.Trim());
            string responsavel = cboResponsavel.SelectedItem.ToString().Trim().ToUpper();
            string motivo = cboMotivo.SelectedItem.ToString().Trim().ToUpper();
            string ocorrencia = txtObservacao.Text.Trim();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO tbocorrencias (carga, responsavel, motivo, observacao, data, usuario_inclusao, data_inclusao) VALUES (@Carga, @Responsavel, @Motivo, @Observacao, GETDATE(), @Usuario_Inclusao, GETDATE())";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Carga", numColeta);
                cmd.Parameters.AddWithValue("@Responsavel", responsavel);
                cmd.Parameters.AddWithValue("@Motivo", motivo);
                cmd.Parameters.AddWithValue("@Observacao", ocorrencia);
                cmd.Parameters.AddWithValue("@Usuario_Inclusao", Session["UsuarioLogado"].ToString());

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    // Logar ou exibir erro
                    Response.Write("<script>alert('Erro: " + ex.Message + "');</script>");
                }
                // Opcional: limpar ou fechar modal
                ScriptManager.RegisterStartupScript(
                                                        this,
                                                        this.GetType(),
                                                        "fecharModalOcorrencia",
                                                        "$('#modalOcorrencia').modal('hide'); $('.modal-backdrop').remove(); $('body').removeClass('modal-open');",
                                                        true
                                                    );
            }

        }
        protected void btnFechar_Click(object sender, EventArgs e)
        {
            // Opcional: limpar ou fechar modal
            ClientScript.RegisterStartupScript(this.GetType(), "HideModal", "hideModal();", true);
            //ScriptManager.RegisterStartupScript(
            //                                        this,
            //                                        this.GetType(),
            //                                        "fecharModalOcorrencia",
            //                                        "$('#modalOcorrencia').modal('hide'); $('.modal-backdrop').remove(); $('body').removeClass('modal-open');",
            //                                        true
            //                                    );
        }

    }
}

