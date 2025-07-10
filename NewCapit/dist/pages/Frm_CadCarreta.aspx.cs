using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NewCapit.dist.pages
{
    public partial class Frm_CadCarreta : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString());
        DateTime dataHoraAtual = DateTime.Now;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UsuarioLogado"] != null)
                {
                    string nomeUsuario = Session["UsuarioLogado"].ToString();
                    var lblUsuario = nomeUsuario;

                    txtCadastradoPor.Text = nomeUsuario;

                }
                else
                {
                    var lblUsuario = "<Usuário>";
                    txtCadastradoPor.Text = lblUsuario;
                }
                txtDataCadastro.Text = dataHoraAtual.ToString("dd/MM/yyyy HH:mm");

                CarregarDDLAgregados();
                PreencherComboRastreadores();
                PreencherComboFiliais();
                PreencherComboMarcasVeiculos();
                PreencherComboCoresVeiculos();
                PreencherComboRastreadores();
                PreencherComboEstados();
            }
        }
        private void CarregarDDLAgregados()
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                conn.Open();
                string query = "SELECT ID, fantra FROM tbtransportadoras WHERE fl_exclusao is null AND ativa_inativa = 'ATIVO' ORDER BY fantra";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                ddlAgregados.DataSource = reader;
                ddlAgregados.DataTextField = "fantra";  // Campo a ser exibido
                ddlAgregados.DataValueField = "ID";  // Valor associado ao item
                ddlAgregados.DataBind();

                // Adicionar o item padrão
                ddlAgregados.Items.Insert(0, new ListItem("", "0"));
            }
        }

        private void PreencherComboRastreadores()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT codRastreador, nomRastreador, codBuonny FROM tbrastreadores";

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
                    ddlTecnologia.DataSource = reader;
                    ddlTecnologia.DataTextField = "nomRastreador";  // Campo que será mostrado no ComboBox
                    ddlTecnologia.DataValueField = "codRastreador";  // Campo que será o valor de cada item                    
                    ddlTecnologia.DataBind();  // Realiza o binding dos dados

                    ddlTecnologia.Items.Insert(0, new ListItem("", "0"));

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

        private void PreencherComboFiliais()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT codFilial, nomFilial FROM tbfiliais";
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
                    cbFiliais.DataTextField = "nomFilial";  // Campo que será mostrado no ComboBox
                    cbFiliais.DataValueField = "codFilial";  // Campo que será o valor de cada item                    
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

        private void PreencherComboMarcasVeiculos()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT id, marca FROM tbmarcasveiculos";

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
                    ddlMarca.DataSource = reader;
                    ddlMarca.DataTextField = "marca";  // Campo que será mostrado no ComboBox
                    ddlMarca.DataValueField = "id";  // Campo que será o valor de cada item                    
                    ddlMarca.DataBind();  // Realiza o binding dos dados                   
                    ddlMarca.Items.Insert(0, new ListItem("", "0"));
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

        private void PreencherComboCoresVeiculos()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT id, cor FROM tbcores";

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
                    ddlCor.DataSource = reader;
                    ddlCor.DataTextField = "cor";  // Campo que será mostrado no ComboBox
                    ddlCor.DataValueField = "id";  // Campo que será o valor de cada item                    
                    ddlCor.DataBind();  // Realiza o binding dos dados                   
                    ddlCor.Items.Insert(0, new ListItem("", "0"));
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

        private void PreencherComboEstados()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT Uf, SiglaUf FROM tbestadosbrasileiros";

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
                    ddlEstados.DataSource = reader;
                    ddlEstados.DataTextField = "SiglaUf";  // Campo que será mostrado no ComboBox
                    ddlEstados.DataValueField = "Uf";  // Campo que será o valor de cada item                    
                    ddlEstados.DataBind();  // Realiza o binding dos dados                   
                    //ddlEstados.Items.Insert(0, new ListItem("", "0"));
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

        protected void ddlCidades_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["CidadeSelecionada"] = ddlCidades.SelectedValue;
        }

        protected void ddlTecnologia_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtCodRastreador.Text = ddlTecnologia.SelectedValue.ToString();
        }

        private void CarregarCidades(string uf)
        {
            string strConn = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                string query = "SELECT cod_municipio, nome_municipio FROM tbmunicipiosbrasileiros WHERE uf = @UF";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UF", uf);
                conn.Open();
                ddlCidades.DataSource = cmd.ExecuteReader();
                ddlCidades.DataTextField = "nome_municipio";
                ddlCidades.DataValueField = "cod_municipio"; // valor único
                ddlCidades.DataBind();

                ddlCidades.Items.Insert(0, new ListItem("-- Selecione uma cidade --", "0"));
            }
        }
        
        protected void ddlEstados_SelectedIndexChanged(object sender, EventArgs e)
        {
            string uf = ddlEstados.SelectedValue;
            CarregarCidades(uf);

            // Restaurar cidade se estiver em ViewState
            if (ViewState["CidadeSelecionada"] != null)
            {
                string cidadeId = ViewState["CidadeSelecionada"].ToString();
                if (ddlCidades.Items.FindByValue(cidadeId) != null)
                {
                    ddlCidades.SelectedValue = cidadeId;
                }
            }
        }

        protected void txtCodTra_TextChanged(object sender, EventArgs e)
        {
            if (txtCodTra.Text != "")
            {

                string codigoRemetente = txtCodTra.Text.Trim();
                string strConn = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    string query = "SELECT codtra, fantra, antt FROM tbtransportadoras WHERE codtra = @Codigo";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Codigo", codigoRemetente);
                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                ddlAgregados.SelectedItem.Text = reader["fantra"].ToString();
                                txtAntt.Text = reader["antt"].ToString();                                
                            }
                            else
                            {
                                ddlAgregados.ClearSelection();
                                txtAntt.Text = string.Empty;
                                txtCodTra.Text = string.Empty;
                                // Aciona o Toast via JavaScript

                                ScriptManager.RegisterStartupScript(this, GetType(), "toastNaoEncontrado", "mostrarToastNaoEncontrado();", true);
                                txtCodTra.Focus();
                                // Opcional: exibir mensagem ao usuário
                            }
                        }
                    }

                }

            }

        }

        private void PreencherCamposProprietario(int id)
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                conn.Open();
                string query = "SELECT codtra, fantra, antt FROM tbtransportadoras WHERE ID = @ID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtCodTra.Text = reader["codtra"].ToString();
                    //ddlAgregados.Text = reader["fantra"].ToString();
                    txtAntt.Text = reader["antt"].ToString();
                }
            }
        }

        // Função para limpar os campos
        private void LimparCamposProprietario()
        {
            txtCodTra.Text = string.Empty;
            txtAntt.Text = string.Empty;
        }

        protected void ddlAgregados_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idSelecionado = int.Parse(ddlAgregados.SelectedValue);

            // Preencher os campos com base no valor selecionado
            if (idSelecionado > 0)
            {
                PreencherCamposProprietario(idSelecionado);
            }
            else
            {
                LimparCamposProprietario();
            }
        }

        protected void txtCodRastreador_TextChanged(object sender, EventArgs e)
        {
            if (txtCodRastreador.Text != "")
            {

                string codigoRastreador = txtCodRastreador.Text.Trim();
                string strConn = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    string query = "SELECT codRastreador, nomRastreador FROM tbrastreadores WHERE codRastreador = @Codigo";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Codigo", codigoRastreador);
                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                ddlTecnologia.SelectedItem.Text = reader["nomRastreador"].ToString();
                            }
                            else
                            {
                                ddlTecnologia.ClearSelection();
                                txtCodRastreador.Text = string.Empty;
                                // Aciona o Toast via JavaScript

                                ScriptManager.RegisterStartupScript(this, GetType(), "toastNaoEncontrado", "mostrarToastNaoEncontrado();", true);
                                txtCodRastreador.Focus();
                                // Opcional: exibir mensagem ao usuário
                            }

                        }
                    }

                }

            }

        }

        protected void txtPlaca_TextChanged(object sender, EventArgs e)
        {
            string termo = txtPlaca.Text.ToUpper();
            string strConn = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(strConn))
            {
                string query = "SELECT TOP 1 placacarreta, codcarreta FROM tbcarretas WHERE placacarreta LIKE @termo";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@termo", "%" + termo + "%");
                conn.Open();

                object res = cmd.ExecuteScalar();
                if (res != null)
                {
                    //resultado = "Resultado: " + res.ToString();
                    //string retorno = "Placa: " + res.ToString() + ", já cadastrado. ";
                    //System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    //sb.Append("<script type = 'text/javascript'>");
                    //sb.Append("window.onload=function(){");
                    //sb.Append("alert('");
                    //sb.Append(retorno);
                    //sb.Append("')};");
                    //sb.Append("</script>");
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
                    //txtPlaca.Text = "";
                    //txtPlaca.Focus();

                    ScriptManager.RegisterStartupScript(this, GetType(), "toastNaoEncontrado", "mostrarToastNaoEncontrado();", true);
                    txtPlaca.Text = "";
                    txtPlaca.Focus();

                }
            }


            txtPlaca.Text.ToUpper();
            ddlEstados.Focus();
        }
        protected void btnSalvarCarreta_Click(object sender, EventArgs e)
        {

        }
    }
}