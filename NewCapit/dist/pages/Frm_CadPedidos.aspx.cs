using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NewCapit.dist.pages
{
    public partial class Frm_CadPedidos : System.Web.UI.Page
    {
        SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString());
        string nomeUsuario = string.Empty;
        DateTime dataHoraAtual = DateTime.Now;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UsuarioLogado"] != null)
                {
                    string nomeUsuario = Session["UsuarioLogado"].ToString();
                    var lblUsuario = nomeUsuario; 
                }
                else
                {
                    var lblUsuario = "<Usuário>";                    
                }
                PreencherComboSolicitantes();
                PreencherComboPagador();
                PreencherComboFiliais();
                PreencherComboRemetente();

                PreencherComboMateriais();

            }
            //DateTime dataHoraAtual = DateTime.Now;
            txtCadastro.Text = dataHoraAtual.ToString("dd/MM/yyyy HH:mm");
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
                    cboFilial.DataSource = reader;
                    cboFilial.DataTextField = "descricao";  // Campo que será mostrado no ComboBox
                    cboFilial.DataValueField = "codigo";  // Campo que será o valor de cada item                    
                    cboFilial.DataBind();  // Realiza o binding dos dados                   
                    cboFilial.Items.Insert(0, new ListItem("Selecione...", "0"));
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
            string query = "SELECT id, nome FROM tbsolicitantes";

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
                    cbSolicitantes.DataSource = reader;
                    cbSolicitantes.DataTextField = "nome";  // Campo que será mostrado no ComboBox
                    cbSolicitantes.DataValueField = "id";  // Campo que será o valor de cada item                    
                    cbSolicitantes.DataBind();  // Realiza o binding dos dados                   
                    cbSolicitantes.Items.Insert(0, new ListItem("Selecione...", "0"));
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
        private void PreencherComboPagador()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT codpagador, pagador FROM tbtomadorservico";

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
                    cboPagador.DataSource = reader;
                    cboPagador.DataTextField = "pagador";  // Campo que será mostrado no ComboBox
                    cboPagador.DataValueField = "codpagador";  // Campo que será o valor de cada item                    
                    cboPagador.DataBind();  // Realiza o binding dos dados                   
                    cboPagador.Items.Insert(0, new ListItem("Selecione...", "0"));
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
            string query = "SELECT codcli, nomcli, cidcli, estcli, ativo_inativo, fl_exclusao FROM tbclientes";

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
                    cboRemetente.DataSource = reader;
                    cboRemetente.DataTextField = "nomcli";  // Campo que será mostrado no ComboBox
                    cboRemetente.DataValueField = "codcli";  // Campo que será o valor de cada item                    
                    cboRemetente.DataBind();  // Realiza o binding dos dados                   
                    cboRemetente.Items.Insert(0, new ListItem("Selecione...", "0"));
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
        private void PreencherComboMateriais()
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
                    cboMaterial.DataSource = reader;
                    cboMaterial.DataTextField = "descricao";  // Campo que será mostrado no ComboBox
                    cboMaterial.DataValueField = "id";  // Campo que será o valor de cada item                    
                    cboMaterial.DataBind();  // Realiza o binding dos dados                   
                    cboMaterial.Items.Insert(0, new ListItem("Selecione...", "0"));
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
        protected void cboPagador_SelectedIndexChanged(object sender, EventArgs e)
        {
            string pagadorId = cboPagador.SelectedValue;

            if (pagadorId != "0")
            {
                using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
                {
                    string query = "SELECT gerenciadora FROM tbtomadorservico WHERE codpagador = @Id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Id", pagadorId);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        txtGR.Text = reader["gerenciadora"].ToString();
                    }
                    else
                    {
                        txtGR.Text = "GR não cadastrada";
                    }
                }
            }
            else
            {
                txtGR.Text = "";
            }
        }
        protected void txtCodRemetente_TextChanged(object sender, EventArgs e)
        {
            if (txtCodRemetente.Text != "")
            {
                string cod = txtCodRemetente.Text;
                string sql = "SELECT codcli, nomcli, cidcli, estcli, ativo_inativo, fl_exclusao FROM tbclientes where codcli = '" + cod + "'";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                conn.Open();
                da.Fill(dt);
                conn.Close();

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0][5].ToString() == null)
                    {
                        // Acione o toast quando a página for carregada
                        string script = "<script>showToast('Cliente deletado do sistema.');</script>";
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                        txtCodRemetente.Text = "";
                        txtCodRemetente.Focus();
                        return;
                    }
                    else if (dt.Rows[0][4].ToString() == "INATIVO")
                    {
                        // Acione o toast quando a página for carregada
                        string script = "<script>showToast('Cliente inativo no sistema.');</script>";
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                        txtCodRemetente.Text = "";
                        txtCodRemetente.Focus();
                        return;
                    }
                    else
                    {
                        cboRemetente.SelectedItem.Text = dt.Rows[0][1].ToString();
                        txtMunicipioRemetente.Text = dt.Rows[0][2].ToString();
                        txtUFRemetente.Text = dt.Rows[0][3].ToString();
                        return;
                    }

                }
                else
                {
                    // Acione o toast quando a página for carregada
                    string script = "<script>showToast('Cliente não encontrado no sistema.');</script>";
                    ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                    txtCodRemetente.Text = "";
                    txtCodRemetente.Focus();
                    return;
                }
            }

        }
        protected void cboRemetente_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idSelecionado = int.Parse(cboRemetente.SelectedValue);

            // Preencher os campos com base no valor selecionado
            if (idSelecionado > 0)
            {
                PreencherCamposRemetente(idSelecionado);
            }
            else
            {
                LimparCamposRemetente();
            }
        }
        // Função para preencher os campos com os dados do banco
        private void PreencherCamposRemetente(int id)
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                conn.Open();
                string query = "SELECT codcli, nomcli, cidcli, estcli FROM tbclientes WHERE codcli = @ID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtCodRemetente.Text = reader["codtra"].ToString();                    
                    txtMunicipioRemetente.Text = reader["cidcli"].ToString();
                    txtUFRemetente.Text = reader["estcli"].ToString();
                }
            }
        }
        // Função para limpar os campos
        private void LimparCamposRemetente()
        {
            txtCodRemetente.Text = string.Empty;
            txtMunicipioRemetente.Text = string.Empty;
            txtUFRemetente.Text = string.Empty;
        }
    }
}