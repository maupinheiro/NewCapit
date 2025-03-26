using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NewCapit.dist.pages
{
    public partial class FrmCadCargas : System.Web.UI.Page
    {
       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UsuarioLogado"] != null)
                {
                    string nomeUsuario = Session["UsuarioLogado"].ToString();
                    var lblUsuario = nomeUsuario;
                    txtUsuCadastro.Text = nomeUsuario;
                }
                else
                {
                    var lblUsuario = "<Usuário>";
                }
                DateTime dataHoraAtual = DateTime.Now;
                lblDtCadastro.Text = dataHoraAtual.ToString("dd/MM/yyyy HH:mm");
                lblDtCadCarga.Text = dataHoraAtual.ToString("dd/MM/yyyy HH:mm");
                
                PreencherComboFiliais();
                
               // PreencherComboTomador();
                PreencherComboRemetente();
                PreencherComboDestinatario();                
            }
            PreencherComboSolicitantes();
        }

        private void PreencherComboFiliais()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT codigo, descricao FROM tbempresa";

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
                    cbFiliais.DataSource = reader;
                    cbFiliais.DataTextField = "descricao";  // Campo que será mostrado no ComboBox
                    cbFiliais.DataValueField = "codigo";  // Campo que será o valor de cada item                    
                    cbFiliais.DataBind();  // Realiza o binding dos dados                   
                    cbFiliais.Items.Insert(0, new ListItem("", "0"));
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
        private void PreencherComboSolicitantes()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT id, nome, tomador, gr FROM tbsolicitantes";

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
                    ddlSolicitante.DataSource = reader;
                    ddlSolicitante.DataTextField = "nome";  // Campo que será mostrado no ComboBox
                    ddlSolicitante.DataValueField = "id";  // Campo que será o valor de cada item                    
                    ddlSolicitante.DataBind();  // Realiza o binding dos dados                   
                    ddlSolicitante.Items.Insert(0, new ListItem("", "0"));
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
        protected void ddlSolicitante_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Obter o valor selecionado no primeiro ComboBox
            string valorSelecionado = ddlSolicitante.SelectedItem.Text;

            if (!string.IsNullOrEmpty(valorSelecionado))
            {
                // Preencher o segundo ComboBox e o TextBox de acordo com a seleção
                PreencherCampos(valorSelecionado);
                //PreencherComboTomador();
            }
        }
        private void PreencherCampos(string gr)
        {
            // Conexão com o banco de dados            
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                try
                {
                    conn.Open();

                    string query = "SELECT id, tomador, gr FROM tbsolicitantes WHERE tomador = @tomador";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@tomador", gr);

                        // Preencher o segundo ComboBox (DropDownList)
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            ddlTomador.Items.Clear();
                            ddlTomador.Items.Add(new ListItem("Selecione...", ""));

                            string grValor = ""; // Variável para armazenar o valor de `gr`

                            while (reader.Read())
                            {
                                // Adiciona corretamente no DropDownList
                                ddlTomador.Items.Add(new ListItem(reader["tomador"].ToString(), reader["id"].ToString()));

                                // Verifica se o valor de "gr" não é nulo
                                if (reader["gr"] != DBNull.Value)
                                {
                                    grValor = reader["gr"].ToString();
                                }
                            }

                            // Preencher o TextBox com o valor correto de `gr`
                            txtGr.Text = grValor;
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Exibe o erro para depuração
                    Response.Write("<script>alert('Erro: " + ex.Message + "');</script>");
                }
            }
        }

        private void PreencherComboTomador()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT codpagador, pagador, gerenciadora FROM tbtomadorservico ORDER BY pagador";

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
                    ddlTomador.DataSource = reader;
                    ddlTomador.DataTextField = "pagador";  // Campo que será mostrado no ComboBox
                    ddlTomador.DataValueField = "codpagador";  // Campo que será o valor de cada item
                    ddlTomador.DataBind();  // Realiza o binding dos dados                   
                    ddlTomador.Items.Insert(0, new ListItem("", "0"));
                                   

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
        private void PreencherComboRemetente()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT codcli, nomcli, cidcli, estcli FROM tbclientes ORDER BY nomcli";

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
                    ddlRemetente.DataSource = reader;
                    ddlRemetente.DataTextField = "nomcli";  // Campo que será mostrado no ComboBox
                    ddlRemetente.DataValueField = "codcli";  // Campo que será o valor de cada item                    
                    ddlRemetente.DataBind();  // Realiza o binding dos dados                   
                    ddlRemetente.Items.Insert(0, new ListItem("", "0"));
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
        private void PreencherComboDestinatario()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT codcli, nomcli, cidcli, estcli FROM tbclientes ORDER BY nomcli";

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
                    ddlDestinatario.DataSource = reader;
                    ddlDestinatario.DataTextField = "nomcli";  // Campo que será mostrado no ComboBox
                    ddlDestinatario.DataValueField = "codcli";  // Campo que será o valor de cada item                    
                    ddlDestinatario.DataBind();  // Realiza o binding dos dados                   
                    ddlDestinatario.Items.Insert(0, new ListItem("", "0"));
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








    }
}