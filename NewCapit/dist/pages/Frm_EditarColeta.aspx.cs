using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using static NPOI.HSSF.Util.HSSFColor;

namespace NewCapit.dist.pages
{
    public partial class Frm_EditarColeta : System.Web.UI.Page
    {
        DateTime dataHoraAtual = DateTime.Now;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UsuarioLogado"] != null)
                {
                    string nomeUsuario = Session["UsuarioLogado"].ToString();
                    var lblUsuario = nomeUsuario;

                    txtAlterado.Text = nomeUsuario;

                }
                else
                {
                    var lblUsuario = "<Usuário>";
                    //txtAlteradoPor.Text = lblUsuario;
                }

                
                PreencherComboFiliais();
                PreencherComboPlantas();
                PreencherComboVeicuos();


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
                    cbFiliais.Items.Insert(0, new ListItem("Selecione ...", "0"));
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
        private void PreencherComboPlantas()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT codigo, descricao FROM tbPlantavw";

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
                    ddlPlanta.DataSource = reader;
                    ddlPlanta.DataTextField = "descricao";  // Campo que será mostrado no ComboBox
                    ddlPlanta.DataValueField = "codigo";  // Campo que será o valor de cada item                    
                    ddlPlanta.DataBind();  // Realiza o binding dos dados                   
                    ddlPlanta.Items.Insert(0, new ListItem("Selecione ...", "0"));
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
        private void PreencherComboVeicuos()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT ID, descricao FROM tbtiposveiculoscnt";

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
                    ddlTipoVeiculo.DataSource = reader;
                    ddlTipoVeiculo.DataTextField = "descricao";  // Campo que será mostrado no ComboBox
                    ddlTipoVeiculo.DataValueField = "ID";  // Campo que será o valor de cada item                    
                    ddlTipoVeiculo.DataBind();  // Realiza o binding dos dados                   
                    ddlTipoVeiculo.Items.Insert(0, new ListItem("Selecione ...", "0"));
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
        protected void txtCodRemetente_TextChanged(object sender, EventArgs e)
        {
            if (txtCodRemetente.Text != "") 
            {

                string codigoRemetente = txtCodRemetente.Text.Trim();
                string strConn = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    string query = "SELECT codcli, nomcli, cidcli, estcli FROM tbclientes WHERE codcli = @Codigo OR codvw=@Codigo";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Codigo", codigoRemetente);
                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtCodRemetente.Text = reader["codcli"].ToString();
                                txtNomRemetente.Text = reader["nomcli"].ToString();
                                txtMunicipioRemetente.Text = reader["cidcli"].ToString();
                                txtUFRemetente.Text = reader["estcli"].ToString();
                                txtCodDestinatario.Focus();
                            }
                            else
                            {
                                txtNomRemetente.Text = "";
                                txtMunicipioRemetente.Text = "";
                                txtUFRemetente.Text = "";
                                txtCodRemetente.Text = "";
                                // Aciona o Toast via JavaScript
                                ScriptManager.RegisterStartupScript(this, GetType(), "toastNaoEncontrado", "mostrarToastNaoEncontrado();", true);
                                txtCodRemetente.Focus();
                                // Opcional: exibir mensagem ao usuário
                            }
                        }
                    }

                }

            }
            
            
        }
        protected void txtCodDestinatario_TextChanged(object sender, EventArgs e)
        {
            if (txtCodDestinatario.Text != "")
            {

                string codigoRemetente = txtCodDestinatario.Text.Trim();
                string strConn = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    string query = "SELECT codcli, nomcli, cidcli, estcli FROM tbclientes WHERE codcli = @Codigo OR codvw=@Codigo";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Codigo", codigoRemetente);
                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtCodDestinatario.Text = reader["codcli"].ToString();
                                txtNomDestinatario.Text = reader["nomcli"].ToString();
                                txtMunicipioDestinatario.Text = reader["cidcli"].ToString();
                                txtUFDestinatario.Text = reader["estcli"].ToString();
                                txtDataColeta.Focus();
                            }
                            else
                            {
                                txtNomDestinatario.Text = "";
                                txtMunicipioDestinatario.Text = "";
                                txtUFDestinatario.Text = "";
                                txtCodDestinatario.Text = "";
                                // Aciona o Toast via JavaScript
                                ScriptManager.RegisterStartupScript(this, GetType(), "toastNaoEncontrado", "mostrarToastNaoEncontrado();", true);
                                txtCodDestinatario.Focus();
                                
                            }
                        }
                    }

                }

            }


        }
    }

}