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
                lblDtCadCarga.Text = dataHoraAtual.ToString("dd/MM/yyyy");
                
                PreencherComboFiliais();
               // PreencherComboSolicitantes();
               // PreencherComboTomador();
                PreencherComboRemetente();
                PreencherComboDestinatario();
                // Preencher o DropDownList de Categorias na primeira carga da página
                PreencherCategorias();
            }
        }

        // Preenche o DropDownList de Categorias
        private void PreencherCategorias()
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                SqlCommand cmd = new SqlCommand("SELECT controle, nome FROM tbsolicitantes", conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                conn.Open();
                da.Fill(dt);
                conn.Close();

                ddlSolicitante.DataSource = dt;
                ddlSolicitante.DataTextField = "nome";
                ddlSolicitante.DataValueField = "controle";
                ddlSolicitante.DataBind();
                ddlSolicitante.Items.Insert(0, new ListItem("", "0"));
            }
        }

        // Evento para quando o usuário escolher uma categoria
        protected void ddlSolicitante_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Obter a categoria selecionada
            int categoriaID = Convert.ToInt32(ddlSolicitante.SelectedValue);

            // Preencher o DropDownList de Produtos com base na categoria escolhida
            PreencherProdutos(categoriaID);
        }

        // Preencher o DropDownList de Produtos com base na Categoria
        private void PreencherProdutos(int categoriaID)
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                SqlCommand cmd = new SqlCommand("SELECT codpagador, pagador, gerenciadora FROM tbtomadorservico WHERE codpagador = @CategoriaID", conn);
                cmd.Parameters.AddWithValue("@CategoriaID", categoriaID);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                conn.Open();
                da.Fill(dt);
                conn.Close();

                ddlTomador.DataSource = dt;
                ddlTomador.DataTextField = "pagador";
                ddlTomador.DataValueField = "codpagador";
                ddlTomador.DataBind();
            }
        }

        // Evento para quando o usuário escolher um produto
        protected void DropDownListProdutos_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Obter o ProdutoID selecionado
            int produtoID = Convert.ToInt32(ddlTomador.SelectedValue);

            // Preencher o TextBox com o preço do produto escolhido
            PreencherPreco(produtoID);
        }

        // Preencher o TextBox com o preço do produto
        private void PreencherPreco(int produtoID)
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                SqlCommand cmd = new SqlCommand("SELECT gerenciadora FROM tbtomadorservico WHERE codpagador = @ProdutoID", conn);
                cmd.Parameters.AddWithValue("@ProdutoID", produtoID);

                conn.Open();
                var gerenciadora = cmd.ExecuteScalar();
                conn.Close();

                // Preencher o TextBox com o preço
                //TextBoxPreco.Text = preco != DBNull.Value ? Convert.ToDecimal(preco).ToString("C") : "Preço não encontrado";
                txtGr.Text = (string)gerenciadora;
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
            string query = "SELECT controle, nome, tomador FROM tbsolicitantes";

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
                    ddlSolicitante.DataValueField = "controle";  // Campo que será o valor de cada item                    
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

        // Evento disparado quando o item do DropDownList é alterado
        protected void ddlTomador_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Verifica se foi selecionado um produto
            if (ddlTomador.SelectedValue != "")
            {
                int codpagador = int.Parse(ddlTomador.SelectedValue);
                GetGerenciadoraDescricao(codpagador);
            }
            else
            {
                txtGr.Text = "";
            }
        }

        private void GetGerenciadoraDescricao(int codpagador)
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                string query = "SELECT gerenciadora FROM tbtomadorservico WHERE codpagador = @codpagador";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@codpagador", codpagador);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtGr.Text = reader.GetString(0);
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