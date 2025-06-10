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

namespace NewCapit.dist.pages
{
    public partial class ConsultaColetasPopUp : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
         
                CarregarGrid();
                PreencherComboResponsavel();
                PreencherComboipoOcorrencia();
            }
           
        }

        private void CarregarGrid()
        {
            string connStr = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT id, carga, data_hora, cliorigem, clidestino, veiculo, tipo_viagem, rota, andamento FROM tbcargas WHERE andamento = 'PENDENTE'";
                SqlCommand command = new SqlCommand(query, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                GVColetas.DataSource = dataTable;
                GVColetas.DataBind();
            }
        }

        protected void timerAtualiza_Tick(object sender, EventArgs e)
        {
            CarregarGrid();
        }


        protected void btnAtualizar_Click(object sender, EventArgs e)
        {
            CarregarGrid();
        }

        private void PreencherComboResponsavel()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT id, descricao FROM tbresponsavelocorrencia ORDER BY descricao ASC";

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
                    cboResponsavel.DataTextField = "descricao";  // Campo que será mostrado no ComboBox
                    cboResponsavel.DataValueField = "id";  // Campo que será o valor de cada item                    
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

        private void PreencherComboipoOcorrencia()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT id, descricao FROM tbtipodeocorrencias ORDER BY descricao ASC";

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
                    cboMotivo.DataSource = reader;
                    cboMotivo.DataTextField = "descricao";  // Campo que será mostrado no ComboBox
                    cboMotivo.DataValueField = "id";  // Campo que será o valor de cada item                    
                    cboMotivo.DataBind();  // Realiza o binding dos dados                   
                    cboMotivo.Items.Insert(0, new ListItem("Selecione...", "0"));
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

        protected void btnSalvarOcorrencia_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
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
                    // Opcional: limpar ou fechar modal                    
                    ClientScript.RegisterStartupScript(this.GetType(), "HideModal", "hideModal();", true);
                }
                catch (Exception ex)
                {
                    // Logar ou exibir erro
                    Response.Write("<script>alert('Erro: " + ex.Message + "');</script>");
                }
            }
        }

        protected void GVColetas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Ocorrencias")
            {
                int id = Convert.ToInt32(e.CommandArgument);

                string connStr = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    SqlCommand cmd1 = new SqlCommand("SELECT carga, andamento FROM tbcargas WHERE carga = @Id", conn);
                    cmd1.Parameters.AddWithValue("@Id", id);
                    SqlDataReader reader1 = cmd1.ExecuteReader();
                    if (reader1.Read())
                    {

                        lblColeta.BackColor = System.Drawing.Color.LightGreen;
                        lblColeta.Text = reader1["carga"].ToString();

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
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "abrirModal", "$('#exampleModalCenter').modal('show');", true);

                    }
                }
            }
        }

        protected void GVColetas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Obtendo os valores das colunas
                
                string andamento = DataBinder.Eval(e.Row.DataItem, "andamento")?.ToString();

                // Índice da célula correspondente à coluna "ATENDIMENTO" (ajuste conforme necessário)
                int colunaAtendimentoIndex = 4; // Ajustar conforme a posição real da coluna no GridView
                TableCell cell = e.Row.Cells[colunaAtendimentoIndex];

               
                if (andamento == "CONCLUIDO")
                {
                    cell.BackColor = System.Drawing.Color.LightGreen;
                    cell.ForeColor = System.Drawing.Color.Black;
                    cell.Text = andamento;
                }
                else if (andamento == "PENDENTE")
                {
                    cell.BackColor = System.Drawing.Color.Yellow;
                    cell.ForeColor = System.Drawing.Color.Black;
                    cell.Text = andamento;
                }
                else if (andamento == "ANDAMENTO")
                {
                    cell.BackColor = System.Drawing.Color.LightCoral;
                    cell.ForeColor = System.Drawing.Color.Black;
                    cell.Text = andamento;
                }
                else
                {
                    cell.BackColor = System.Drawing.Color.Black;
                    cell.ForeColor = System.Drawing.Color.Black;
                    cell.Text = andamento;
                }
                
                
            }
        }
    }
}

