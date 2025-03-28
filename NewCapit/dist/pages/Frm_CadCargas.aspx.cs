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
                PreencherComboMaterial();
                PreencherComboRemetente();
                PreencherComboDestinatario();
                PreencherComboSolicitantes();
            }
           
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
                    cbFiliais.Items.Insert(0, new ListItem("Selecione...", "0"));
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
            string query = "SELECT id, nome FROM tbsolicitantes";

            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        ddlSolicitante.DataSource = reader;
                        ddlSolicitante.DataTextField = "nome";
                        ddlSolicitante.DataValueField = "id";
                        ddlSolicitante.DataBind();
                    }

                    // Insere a opção inicial no DropDownList
                    ddlSolicitante.Items.Insert(0, new ListItem("Selecione...", "0"));
                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('Erro ao carregar solicitantes: " + ex.Message + "');</script>");
                }
            }
        }
        protected void ddlSolicitante_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlSolicitante.SelectedValue != "0")
            {
                PreencherCampos(ddlSolicitante.SelectedItem.Text);
            }
            else
            {
                ddlTomador.Items.Clear();
                txtGr.Text = "";
            }
        }
        private void PreencherCampos(string nome)
        {
            string query = "SELECT id, tomador, gr FROM tbsolicitantes WHERE nome = @nome";

            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@nome", nome);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            ddlTomador.Items.Clear();
                            //ddlTomador.Items.Add(new ListItem("Selecione...", ""));

                            string grValor = ""; // Variável para armazenar o valor de `gr`

                            while (reader.Read())
                            {
                                string tomador = reader["tomador"]?.ToString() ?? "";
                                string id = reader["id"]?.ToString() ?? "";

                                ddlTomador.Items.Add(new ListItem(tomador, id));

                                // Preenche `grValor` caso não seja nulo
                                if (!(reader["gr"] is DBNull))
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
                    Response.Write("<script>alert('Erro ao preencher campos: " + ex.Message + "');</script>");
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
        private void PreencherComboMaterial()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT id, descricao FROM tbtipomaterial";

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
                    ddlMaterial.DataSource = reader;
                    ddlMaterial.DataTextField = "descricao";  // Campo que será mostrado no ComboBox
                    ddlMaterial.DataValueField = "id";  // Campo que será o valor de cada item                    
                    ddlMaterial.DataBind();  // Realiza o binding dos dados                   
                    ddlMaterial.Items.Insert(0, new ListItem("Selecione...", "0"));
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
        protected void btnPedido_Click(object sender, EventArgs e)
        {
            if (txtNumPedido.Text.Trim() == "")
            {
                string nomeUsuario = txtUsuCadastro.Text;

                string linha1 = "Olá, " + nomeUsuario + "!";
                string linha2 = "Por favor, digite o número do pedido.";

                // Concatenando as linhas com '\n' para criar a mensagem
                string mensagem = $"{linha1}\n{linha2}";

                string mensagemCodificada = HttpUtility.JavaScriptStringEncode(mensagem);
                // Gerando o script JavaScript para exibir o alerta
                string script = $"alert('{mensagemCodificada}');";

                // Registrando o script para execução no lado do cliente
                ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", script, true);
                txtNumPedido.Focus();

            }
            else
            {
                var numPedido = txtNumPedido.Text.Trim();

                var obj = new Domain.ConsultaPedido
                {
                    pedido = numPedido
                };


                var ConsultaPedido = DAL.UsersDAL.CheckPedido(obj);
                if (ConsultaPedido != null)
                {
                    string nomeUsuario = txtUsuCadastro.Text;
                    string razaoSocial = ConsultaPedido.clidestino.ToString();
                    string unidade = ConsultaPedido.carga.ToString();

                    string linha1 = "Olá, " + nomeUsuario + "!";
                    string linha2 = "Pedido " + numPedido + ", já cadastrado no sistema.";
                    string linha3 = "Destinatário: " + razaoSocial + ".";
                    string linha4 = "Carga: " + unidade + ". Por favor, verifique.";

                    // Concatenando as linhas com '\n' para criar a mensagem
                    string mensagem = $"{linha1}\n{linha2}\n{linha3}\n{linha4}";

                    string mensagemCodificada = HttpUtility.JavaScriptStringEncode(mensagem);
                    // Gerando o script JavaScript para exibir o alerta
                    string script = $"alert('{mensagemCodificada}');";

                    // Registrando o script para execução no lado do cliente
                    ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", script, true);

                    txtNumPedido.Text = "";
                    txtNumPedido.Focus();
                }
                else
                {
                    ddlMaterial.Focus();
                }

            }
        }







    }
}