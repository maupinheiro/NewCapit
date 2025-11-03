using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing.Imaging;
using NPOI.SS.UserModel;
using NPOI.SS.Formula.PTG;
using System.Configuration;

namespace NewCapit.dist.pages
{
    public partial class Frm_Rotas : System.Web.UI.Page
    {
        SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString());
        string nomeUsuario = string.Empty;
        DateTime dataHoraAtual = DateTime.Now;
        string id;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UsuarioLogado"] != null)
                {
                    string nomeUsuario = Session["UsuarioLogado"].ToString();
                    txtAltCad.Text = Session["UsuarioLogado"].ToString().ToUpper();
                }
                else
                {
                    txtUsuCadastro.Text = "<USUÁRIO>";
                }
                PreencherNumRota();
                PreencherComboRemetente();
                PreencherComboExpedidor();
                PreencherComboDestinatario();
                PreencherComboRecebedor();
                CarregaRotas();
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
            DateTime dataHoraAtual = DateTime.Now;
            lbDtAtualizacao.Text= dataHoraAtual.ToString("dd/MM/yyyy HH:mm");
           

        }
        private void PreencherNumRota()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT (rota + incremento) as ProximaRota FROM tbcontadores";

            // Crie uma conexão com o banco de dados
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Crie o comando SQL
                        //SqlCommand cmd = new SqlCommand(query, conn);

                        // Execute o comando e obtenha os dados em um DataReader
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                // Preencher o TextBox com o nome encontrado 
                                txtRota.Text = reader["ProximaRota"].ToString();
                            }
                        }

                    }
                    string id = "1";

                    // Verifica se o ID foi fornecido e é um número válido
                    if (string.IsNullOrEmpty(id) || !int.TryParse(id, out int idConvertido))
                    {
                        // Acione o toast quando a página for carregada
                        string script = "<script>showToast('ID invalido ou não fornecido.');</script>";
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                        return;
                    }
                    string sql = @"UPDATE tbcontadores SET rota = @rota WHERE id = @id";
                    try
                    {
                        using (SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
                        using (SqlCommand cmd = new SqlCommand(sql, con))
                        {
                            cmd.Parameters.AddWithValue("@rota", txtRota.Text);
                            cmd.Parameters.AddWithValue("@id", idConvertido);

                            con.Open();
                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                // atualiza  
                            }
                            else
                            {
                                // Acione o toast quando a página for carregada
                                string script = "<script>showToast('Erro ao atualizar o número da rota.');</script>";
                                ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                            }

                        }


                    }
                    catch (Exception ex)
                    {
                        string mensagemErro = $"Erro ao atualizar: {HttpUtility.JavaScriptStringEncode(ex.Message)}";
                        string script = $"alert('{mensagemErro}');";
                        ClientScript.RegisterStartupScript(this.GetType(), "Erro", script, true);
                    }
                }
                catch (Exception ex)
                {
                    //Tratar erro
                    //txtResultado.Text = "Erro: " + ex.Message;
                }
            }
        }

        private void PreencherComboRemetente()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT codcli, nomcli, cidcli, estcli, ativo_inativo, fl_exclusao FROM tbclientes where fl_exclusao is null and ativo_inativo = 'ATIVO' order by nomcli";

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
        private void PreencherComboExpedidor()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT codcli, nomcli, cidcli, estcli, ativo_inativo, fl_exclusao FROM tbclientes order by nomcli";

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
                    cboExpedidor.DataSource = reader;
                    cboExpedidor.DataTextField = "nomcli";  // Campo que será mostrado no ComboBox
                    cboExpedidor.DataValueField = "codcli";  // Campo que será o valor de cada item                    
                    cboExpedidor.DataBind();  // Realiza o binding dos dados                   
                    cboExpedidor.Items.Insert(0, new ListItem("Selecione...", "0"));
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
            string query = "SELECT codcli, nomcli, cidcli, estcli, ativo_inativo, fl_exclusao FROM tbclientes order by nomcli";

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
                    cboDestinatario.DataSource = reader;
                    cboDestinatario.DataTextField = "nomcli";  // Campo que será mostrado no ComboBox
                    cboDestinatario.DataValueField = "codcli";  // Campo que será o valor de cada item                    
                    cboDestinatario.DataBind();  // Realiza o binding dos dados                   
                    cboDestinatario.Items.Insert(0, new ListItem("Selecione...", "0"));
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
        private void PreencherComboRecebedor()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT codcli, nomcli, cidcli, estcli, ativo_inativo, fl_exclusao FROM tbclientes order by nomcli";

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
                    cboRecebedor.DataSource = reader;
                    cboRecebedor.DataTextField = "nomcli";  // Campo que será mostrado no ComboBox
                    cboRecebedor.DataValueField = "codcli";  // Campo que será o valor de cada item                    
                    cboRecebedor.DataBind();  // Realiza o binding dos dados                   
                    cboRecebedor.Items.Insert(0, new ListItem("Selecione...", "0"));
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
                        cboRemetente.SelectedItem.Text = dt.Rows[0][1].ToString().ToUpper();
                        txtMunicipioRemetente.Text = dt.Rows[0][2].ToString().ToUpper();
                        txtUFRemetente.Text = dt.Rows[0][3].ToString().ToUpper();
                        if (txtMunicipioRemetente.Text == "" || txtUFRemetente.Text == "")
                        {
                            // Acione o toast quando a página for carregada
                            string script = "<script>showToast('Município ou UF, não está preenchido. Por favor, verifique.');</script>";
                            ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                            txtCodRemetente.Text = "";
                            txtCodRemetente.Focus();
                            return;
                        }

                        txtCodExpedidor.Focus();
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
                    txtCodRemetente.Text = reader["codcli"].ToString();
                    txtMunicipioRemetente.Text = reader["cidcli"].ToString().ToUpper();
                    txtUFRemetente.Text = reader["estcli"].ToString().ToUpper();
                    if (txtMunicipioRemetente.Text == "" || txtUFRemetente.Text == "")
                    {
                        // Acione o toast quando a página for carregada
                        string script = "<script>showToast('Município ou UF, não está preenchido. Por favor, verifique.');</script>";
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                        txtCodRemetente.Text = "";
                        txtCodRemetente.Focus();
                        return;
                    }
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

        protected void txtCodExpedidor_TextChanged(object sender, EventArgs e)
        {
            if (txtCodExpedidor.Text != "")
            {
                string cod = txtCodExpedidor.Text;
                string sql = "SELECT codcli, nomcli, cidcli, estcli, ativo_inativo, fl_exclusao FROM tbclientes where codcli = '" + cod + "' and ativo_inativo = 'ATIVO' and fl_exclusao is null";
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
                        txtCodExpedidor.Text = "";
                        txtCodExpedidor.Focus();
                        return;
                    }
                    else if (dt.Rows[0][4].ToString() == "INATIVO")
                    {
                        // Acione o toast quando a página for carregada
                        string script = "<script>showToast('Cliente inativo no sistema.');</script>";
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                        txtCodExpedidor.Text = "";
                        txtCodExpedidor.Focus();
                        return;
                    }
                    else
                    {
                        cboExpedidor.SelectedItem.Text = dt.Rows[0][1].ToString();
                        txtCidExpedidor.Text = dt.Rows[0][2].ToString().ToUpper();
                        txtUFExpedidor.Text = dt.Rows[0][3].ToString().ToUpper();
                        txtCodDestinatario.Focus();

                        if (txtCidExpedidor.Text == "" || txtUFExpedidor.Text == "")
                        {
                            // Acione o toast quando a página for carregada
                            string script = "<script>showToast('Município ou UF, não está preenchido. Por favor, verifique.');</script>";
                            ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                            txtCodExpedidor.Text = "";
                            txtCodExpedidor.Focus();
                            return;
                        }
                        return;
                    }


                }
                else
                {
                    // Acione o toast quando a página for carregada
                    string script = "<script>showToast('Cliente não encontrado no sistema.');</script>";
                    ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                    txtCodExpedidor.Text = "";
                    txtCodExpedidor.Focus();
                    return;
                }
            }

        }
        protected void cboExpedidor_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idSelecionado = int.Parse(cboExpedidor.SelectedValue);

            // Preencher os campos com base no valor selecionado
            if (idSelecionado > 0)
            {
                PreencherCamposExpedidor(idSelecionado);
            }
            else
            {
                LimparCamposExpedidor();
            }
        }
        // Função para preencher os campos com os dados do banco
        private void PreencherCamposExpedidor(int id)
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
                    txtCodExpedidor.Text = reader["codcli"].ToString();
                    txtCidExpedidor.Text = reader["cidcli"].ToString().ToUpper();
                    txtUFExpedidor.Text = reader["estcli"].ToString().ToUpper();
                    if (txtCidExpedidor.Text == "" || txtUFExpedidor.Text == "")
                    {
                        // Acione o toast quando a página for carregada
                        string script = "<script>showToast('Município ou UF, não está preenchido. Por favor, verifique.');</script>";
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                        txtCodExpedidor.Text = "";
                        txtCodExpedidor.Focus();
                        return;
                    }
                    return;
                }
            }
        }
        // Função para limpar os campos
        private void LimparCamposExpedidor()
        {
            txtCodExpedidor.Text = string.Empty;
            txtCidExpedidor.Text = string.Empty;
            txtUFExpedidor.Text = string.Empty;
        }

        protected void txtCodDestinatario_TextChanged(object sender, EventArgs e)
        {
            if (txtCodDestinatario.Text != "")
            {
                string cod = txtCodDestinatario.Text;
                string sql = "SELECT codcli, nomcli, cidcli, estcli, ativo_inativo, fl_exclusao FROM tbclientes where codcli = '" + cod + "' and ativo_inativo = 'ATIVO' and fl_exclusao is null";
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
                        txtCodDestinatario.Text = "";
                        txtCodDestinatario.Focus();
                        return;
                    }
                    else if (dt.Rows[0][4].ToString() == "INATIVO")
                    {
                        // Acione o toast quando a página for carregada
                        string script = "<script>showToast('Cliente inativo no sistema.');</script>";
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                        txtCodDestinatario.Text = "";
                        txtCodDestinatario.Focus();
                        return;
                    }
                    else
                    {
                        cboDestinatario.SelectedItem.Text = dt.Rows[0][1].ToString();
                        txtMunicipioDestinatario.Text = dt.Rows[0][2].ToString().ToUpper();
                        txtUFDestinatario.Text = dt.Rows[0][3].ToString().ToUpper();
                        if (txtMunicipioDestinatario.Text == "" || txtUFDestinatario.Text == "")
                        {
                            // Acione o toast quando a página for carregada
                            string script = "<script>showToast('Município ou UF, não está preenchido. Por favor, verifique.');</script>";
                            ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                            txtCodDestinatario.Text = "";
                            txtCodDestinatario.Focus();
                            return;
                        }
                        txtCodRecebedor.Focus();
                        return;
                    }

                }
                else
                {
                    // Acione o toast quando a página for carregada
                    string script = "<script>showToast('Cliente não encontrado no sistema.');</script>";
                    ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                    txtCodDestinatario.Text = "";
                    txtCodDestinatario.Focus();
                    return;
                }
            }

        }
        protected void cboDestinatario_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idSelecionado = int.Parse(cboDestinatario.SelectedValue);

            // Preencher os campos com base no valor selecionado
            if (idSelecionado > 0)
            {
                PreencherCamposDestinatario(idSelecionado);
            }
            else
            {
                LimparCamposDestinatario();
            }
        }
        // Função para preencher os campos com os dados do banco
        private void PreencherCamposDestinatario(int id)
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
                    txtCodDestinatario.Text = reader["codcli"].ToString();
                    txtMunicipioDestinatario.Text = reader["cidcli"].ToString().ToUpper();
                    txtUFDestinatario.Text = reader["estcli"].ToString().ToUpper();
                    if (txtMunicipioDestinatario.Text == "" || txtUFDestinatario.Text == "")
                    {
                        // Acione o toast quando a página for carregada
                        string script = "<script>showToast('Município ou UF, não está preenchido. Por favor, verifique.');</script>";
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                        txtCodDestinatario.Text = "";
                        txtCodDestinatario.Focus();
                        return;
                    }
                }
            }
        }
        // Função para limpar os campos
        private void LimparCamposDestinatario()
        {
            txtCodDestinatario.Text = string.Empty;
            txtMunicipioDestinatario.Text = string.Empty;
            txtUFDestinatario.Text = string.Empty;
        }

        protected void txtCodRecebedor_TextChanged(object sender, EventArgs e)
        {
            if (txtCodRecebedor.Text != "")
            {
                string cod = txtCodRecebedor.Text;
                string sql = "SELECT codcli, nomcli, cidcli, estcli, ativo_inativo, fl_exclusao FROM tbclientes where codcli = '" + cod + "' and ativo_inativo = 'ATIVO' and fl_exclusao is null";
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
                        txtCodRecebedor.Text = "";
                        txtCodRecebedor.Focus();
                        return;
                    }
                    else if (dt.Rows[0][4].ToString() == "INATIVO")
                    {
                        // Acione o toast quando a página for carregada
                        string script = "<script>showToast('Cliente inativo no sistema.');</script>";
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                        txtCodRecebedor.Text = "";
                        txtCodRecebedor.Focus();
                        return;
                    }
                    else
                    {
                        cboRecebedor.SelectedItem.Text = dt.Rows[0][1].ToString().ToUpper();
                        txtCidRecebedor.Text = dt.Rows[0][2].ToString().ToUpper().ToUpper();
                        txtUFRecebedor.Text = dt.Rows[0][3].ToString().ToUpper().ToUpper();
                        if (txtCidRecebedor.Text == "" || txtUFRecebedor.Text == "")
                        {
                            // Acione o toast quando a página for carregada
                            string script = "<script>showToast('Município ou UF, não está preenchido. Por favor, verifique.');</script>";
                            ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                            txtCodRecebedor.Text = "";
                            txtCodRecebedor.Focus();
                            return;
                        }
                        // PESQUISAR DISTANCIA
                        string cidadeExpedidor = txtCidExpedidor.Text.Trim();
                        string ufExpedidor = txtUFExpedidor.Text.Trim();
                        string cidadeRecebedor = txtCidRecebedor.Text.Trim();
                        string ufRecebedor = txtUFRecebedor.Text.Trim();
                        string sqlPesquisa = "SELECT UF_Origem, Origem, UF_Destino, Destino, Distancia FROM tbdistanciapremio WHERE UF_Origem = '" + ufExpedidor + "' AND Origem = '" + cidadeExpedidor + "' AND UF_Destino = '" + ufRecebedor + "' AND destino = '" + cidadeRecebedor + "'";
                        SqlDataAdapter daPesquisa = new SqlDataAdapter(sqlPesquisa, conn);
                        DataTable dtPesquisa = new DataTable();
                        conn.Open();
                        daPesquisa.Fill(dtPesquisa);
                        conn.Close();

                        if (dtPesquisa.Rows.Count > 0)
                        {
                            txtDistancia.Text = dtPesquisa.Rows[0][4].ToString();
                        }
                        else
                        {
                            // Acione o toast quando a página for carregada
                            string script = "<script>showToast('Verifique a tabela de distância. Parametro não encontrado.');</script>";
                            ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                            txtCodRecebedor.Text = "";
                            txtCodRecebedor.Focus();
                            return;
                        }

                        // FIM DA PESQUISA 

                        if (txtUFExpedidor.Text != "" && txtUFRecebedor.Text != "")
                        {
                            if (txtUFExpedidor.Text != txtUFRecebedor.Text)
                            {
                                cboDeslocamento.SelectedItem.Text = "INTERESTADUAL";
                            }
                        }
                        if (txtCidExpedidor.Text != "" && txtCidRecebedor.Text != "")
                        {
                            if (txtCidExpedidor.Text != txtCidRecebedor.Text && txtUFExpedidor.Text == txtUFRecebedor.Text)
                            {
                                cboDeslocamento.SelectedItem.Text = "INTERMUNICIPAL";
                            }
                            if (txtCidExpedidor.Text == txtCidRecebedor.Text && txtUFExpedidor.Text == txtUFRecebedor.Text)
                            {
                                cboDeslocamento.SelectedItem.Text = "MUNICIPAL";
                            }
                        }

                        // DEFINE DESCRIÇÃO DA ROTA
                        if (txtCodRemetente.Text == "" || cboRemetente.SelectedItem.Text == "")
                        {
                            // Acione o toast quando a página for carregada
                            string script = "<script>showToast('Remetente, está faltando parametro. Verifique CODIGO/NOME/CIDADE/ESTADO.');</script>";
                            ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                            txtCodRecebedor.Text = "";
                            txtCodRemetente.Text = "";
                            txtCodRemetente.Focus();
                            return;
                        }
                        else if (txtCodExpedidor.Text == "" || cboExpedidor.SelectedItem.Text == "")
                        {
                            // Acione o toast quando a página for carregada
                            string script = "<script>showToast('Expedidor, está faltando parametro. Verifique CODIGO/NOME/CIDADE/ESTADO.');</script>";
                            ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                            txtCodRecebedor.Text = "";
                            txtCodExpedidor.Text = "";
                            txtCodExpedidor.Focus();
                            return;
                        }
                        else if (txtCodDestinatario.Text == "" || cboDestinatario.SelectedItem.Text == "")
                        {
                            // Acione o toast quando a página for carregada
                            string script = "<script>showToast('Destinatário, está faltando parametro. Verifique CODIGO/NOME/CIDADE/ESTADO.');</script>";
                            ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                            txtCodRecebedor.Text = "";
                            txtCodDestinatario.Text = "";
                            txtCodDestinatario.Focus();
                            return;
                        }
                        else if (txtCodRecebedor.Text == "" || cboRecebedor.SelectedItem.Text == "")
                        {
                            // Acione o toast quando a página for carregada
                            string script = "<script>showToast('Recebedor, está faltando parametro. Verifique CODIGO/NOME/CIDADE/ESTADO.');</script>";
                            ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                            txtCodRecebedor.Text = "";
                            txtCodRecebedor.Focus();
                            return;
                        }
                        else
                        {
                            string texto = cboRemetente.SelectedItem.Text.Trim();
                            int indexEspaco = texto.IndexOf(" ");
                            string remetente;
                            if (indexEspaco != -1)
                            {
                                // Pega do início até antes do primeiro espaço
                                remetente = texto.Substring(0, indexEspaco);
                            }
                            else
                            {
                                // Não há espaço, pega o texto inteiro
                                remetente = texto;
                            }

                            string textoDestinatario = cboDestinatario.SelectedItem.Text.Trim();
                            int indexEspacoDestinatario = textoDestinatario.IndexOf(" ");
                            string destinatario;
                            if (indexEspacoDestinatario != -1)
                            {
                                // Pega do início até antes do primeiro espaço
                                destinatario = textoDestinatario.Substring(0, indexEspacoDestinatario);
                            }
                            else
                            {
                                // Não há espaço, pega o texto inteiro
                                destinatario = textoDestinatario;
                            }

                            string textoExpedidor = cboExpedidor.SelectedItem.Text.Trim();
                            int indexEspacoExpedidor = textoExpedidor.IndexOf(" ");
                            string expedidor;
                            if (indexEspacoExpedidor != -1)
                            {
                                // Pega do início até antes do primeiro espaço
                                expedidor = textoExpedidor.Substring(0, indexEspacoExpedidor);
                            }
                            else
                            {
                                // Não há espaço, pega o texto inteiro
                                expedidor = textoExpedidor;
                            }

                            string textoRecebedor = cboRecebedor.SelectedItem.Text.Trim();
                            int indexEspacoRecebedor = textoRecebedor.IndexOf(" ");
                            string recebedor;
                            if (indexEspacoRecebedor != -1)
                            {
                                // Pega do início até antes do primeiro espaço
                                recebedor = textoRecebedor.Substring(0, indexEspacoRecebedor);
                            }
                            else
                            {
                                // Não há espaço, pega o texto inteiro
                                recebedor = textoRecebedor;
                            }


                            txtDesc_Rota.Text = "Rem./Dest.: " + txtCodRemetente.Text.Trim() + " - " + remetente + " / " + txtCodDestinatario.Text.Trim() + " - " + destinatario + " - Expedidor/Recebedor: " + txtCodExpedidor.Text.Trim() + " - " + expedidor + "(" + txtCidExpedidor.Text.Trim() + "/" + txtUFExpedidor.Text.Trim() + ") / " + txtCodRecebedor.Text.Trim() + " - " + recebedor + "(" + txtCidRecebedor.Text.Trim() + "/" + txtUFRecebedor.Text.Trim() + ")";

                            if (txtDesc_Rota.Text != "")
                            {
                                string codDesc_Rota = txtDesc_Rota.Text;
                                string sqlDescricao = "SELECT rota, desc_rota, situacao, fl_exclusao FROM tbrotasdeentregas where desc_rota = '" + codDesc_Rota + "'";
                                SqlDataAdapter daRota = new SqlDataAdapter(sqlDescricao, conn);
                                DataTable dtRota = new DataTable();
                                conn.Open();
                                daRota.Fill(dtRota);
                                conn.Close();

                                if (dt.Rows.Count > 0)
                                {
                                    if (dt.Rows[0][3].ToString() == null)
                                    {
                                        // Acione o toast quando a página for carregada
                                        string script = "<script>showToast('Rota deletada, recuperada.');</script>";
                                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                                        // aqui recupera o registro voltando para o sistema e volta para a tela de gestão
                                        return;
                                    }
                                    else if (dt.Rows[0][2].ToString() == "INATIVO")
                                    {
                                        // Acione o toast quando a página for carregada
                                        string script = "<script>showToast('Rota inativa, foi ativada.');</script>";
                                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                                        // aqui recupera o registro voltando para o sistema e volta para a tela de gestão
                                        return;
                                    }
                                }
                            }
                        }



                        return;
                    }

                }
                else
                {
                    // Acione o toast quando a página for carregada
                    string script = "<script>showToast('Cliente não encontrado no sistema.');</script>";
                    ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                    txtCodRecebedor.Text = "";
                    txtCodRecebedor.Focus();
                    return;
                }
            }

        }
        protected void cboRecebedor_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idSelecionado = int.Parse(cboRecebedor.SelectedValue);

            // Preencher os campos com base no valor selecionado
            if (idSelecionado > 0)
            {
                PreencherCamposRecebedor(idSelecionado);
            }
            else
            {
                LimparCamposRecebedor();
            }
        }
        // Função para preencher os campos com os dados do banco
        private void PreencherCamposRecebedor(int id)
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
                    txtCodRecebedor.Text = reader["codcli"].ToString();
                    txtCidRecebedor.Text = reader["cidcli"].ToString();
                    txtUFRecebedor.Text = reader["estcli"].ToString();
                    if (txtCidRecebedor.Text == "" || txtUFRecebedor.Text == "")
                    {
                        // Acione o toast quando a página for carregada
                        string script = "<script>showToast('Município ou UF, não está preenchido. Por favor, verifique.');</script>";
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                        txtCodRecebedor.Text = "";
                        txtCodRecebedor.Focus();
                        return;
                    }
                }
            }
        }
        // Função para limpar os campos
        private void LimparCamposRecebedor()
        {
            txtCodRecebedor.Text = string.Empty;
            txtCidRecebedor.Text = string.Empty;
            txtUFRecebedor.Text = string.Empty;
        }
        public void CarregaRotas()
        {
            if (HttpContext.Current.Request.QueryString["id"].ToString() != "")
            {
                id = HttpContext.Current.Request.QueryString["id"].ToString();
            }
            string rota = id;
            string strConn = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                string queryRota = "SELECT rota,desc_rota,codigo_remetente,nome_remetente,cidade_remetente,uf_remetente,codigo_expedidor,nome_expedidor,cidade_expedidor,uf_expedidor,codigo_destinatario,nome_destinatario,cidade_destinatario,uf_destinatario ";
                queryRota += " ,codigo_recebedor,nome_recebedor,cidade_recebedor,uf_recebedor,distancia,tempo,deslocamento,situacao,data_cadastro,usuario_cadastro  FROM dbo.tbrotasdeentregas where rota=@idrota";

                using (SqlCommand cmdTNG = new SqlCommand(queryRota, conn))
                {
                    cmdTNG.Parameters.AddWithValue("@idrota", rota);
                    conn.Open();

                    using (SqlDataReader reader = cmdTNG.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtRota.Text = reader["rota"].ToString();
                            txtDesc_Rota.Text = reader["desc_rota"].ToString();
                            txtCodRemetente.Text = reader["codigo_remetente"].ToString();
                            cboRemetente.SelectedItem.Text = reader["nome_remetente"].ToString();
                            txtMunicipioRemetente.Text = reader["cidade_remetente"].ToString();
                            txtUFRemetente.Text = reader["uf_remetente"].ToString();
                            txtCodExpedidor.Text = reader["codigo_expedidor"].ToString();
                            cboExpedidor.SelectedItem.Text = reader["nome_expedidor"].ToString();
                            txtCidExpedidor.Text = reader["cidade_expedidor"].ToString();
                            txtUFExpedidor.Text = reader["uf_expedidor"].ToString();
                            txtCodDestinatario.Text = reader["codigo_destinatario"].ToString();
                            cboDestinatario.SelectedItem.Text = reader["nome_destinatario"].ToString();
                            txtMunicipioDestinatario.Text = reader["cidade_destinatario"].ToString();
                            txtUFDestinatario.Text = reader["uf_destinatario"].ToString();
                            txtCodRecebedor.Text = reader["codigo_recebedor"].ToString();
                            cboRecebedor.SelectedItem.Text = reader["nome_recebedor"].ToString();
                            txtCidRecebedor.Text = reader["cidade_recebedor"].ToString();
                            txtUFRecebedor.Text = reader["uf_recebedor"].ToString();
                            txtDistancia.Text = reader["distancia"].ToString();
                            txtDuracao.Text = reader["tempo"].ToString();
                            cboDeslocamento.SelectedItem.Text = reader["deslocamento"].ToString();
                            ddlStatus.SelectedItem.Text = reader["situacao"].ToString();
                            lblDtCadastro.Text = reader["data_cadastro"].ToString();
                            txtCadastro.Text = reader["usuario_cadastro"].ToString();
                            txtUsuCadastro.Text = reader["usuario_cadastro"].ToString();


                        }
                    }
                }
            }
        }
        protected void btnAlterar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                string id = Request.QueryString["id"].ToString();
                string strConn = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    string sql = @"
            UPDATE dbo.tbrotasdeentregas
               SET desc_rota           = @desc_rota,
                   codigo_remetente    = @codigo_remetente,
                   nome_remetente      = @nome_remetente,
                   cidade_remetente    = @cidade_remetente,
                   uf_remetente        = @uf_remetente,
                   codigo_expedidor    = @codigo_expedidor,
                   nome_expedidor      = @nome_expedidor,
                   cidade_expedidor    = @cidade_expedidor,
                   uf_expedidor        = @uf_expedidor,
                   codigo_destinatario = @codigo_destinatario,
                   nome_destinatario   = @nome_destinatario,
                   cidade_destinatario = @cidade_destinatario,
                   uf_destinatario     = @uf_destinatario,
                   codigo_recebedor    = @codigo_recebedor,
                   nome_recebedor      = @nome_recebedor,
                   cidade_recebedor    = @cidade_recebedor,
                   uf_recebedor        = @uf_recebedor,
                   distancia           = @distancia,
                   tempo               = @tempo,
                   deslocamento        = @deslocamento,
                   situacao            = @situacao,
                   data_alteracao      = @data_alteracao,
                   usuario_alteracao   = @usuario_alteracao
                   WHERE rota = @rota";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        // parâmetros
                        cmd.Parameters.AddWithValue("@desc_rota", txtDesc_Rota.Text.Trim());
                        cmd.Parameters.AddWithValue("@codigo_remetente", txtCodRemetente.Text.Trim());
                        cmd.Parameters.AddWithValue("@nome_remetente", cboRemetente.SelectedItem.Text);
                        cmd.Parameters.AddWithValue("@cidade_remetente", txtMunicipioRemetente.Text.Trim());
                        cmd.Parameters.AddWithValue("@uf_remetente", txtUFRemetente.Text.Trim());
                        cmd.Parameters.AddWithValue("@codigo_expedidor", txtCodExpedidor.Text.Trim());
                        cmd.Parameters.AddWithValue("@nome_expedidor", cboExpedidor.SelectedItem.Text);
                        cmd.Parameters.AddWithValue("@cidade_expedidor", txtCidExpedidor.Text.Trim());
                        cmd.Parameters.AddWithValue("@uf_expedidor", txtUFExpedidor.Text.Trim());
                        cmd.Parameters.AddWithValue("@codigo_destinatario", txtCodDestinatario.Text.Trim());
                        cmd.Parameters.AddWithValue("@nome_destinatario", cboDestinatario.SelectedItem.Text);
                        cmd.Parameters.AddWithValue("@cidade_destinatario", txtMunicipioDestinatario.Text.Trim());
                        cmd.Parameters.AddWithValue("@uf_destinatario", txtUFDestinatario.Text.Trim());
                        cmd.Parameters.AddWithValue("@codigo_recebedor", txtCodRecebedor.Text.Trim());
                        cmd.Parameters.AddWithValue("@nome_recebedor", cboRecebedor.SelectedItem.Text);
                        cmd.Parameters.AddWithValue("@cidade_recebedor", txtCidRecebedor.Text.Trim());
                        cmd.Parameters.AddWithValue("@uf_recebedor", txtUFRecebedor.Text.Trim());
                        cmd.Parameters.AddWithValue("@distancia", txtDistancia.Text.Trim().Replace(',', '.'));
                        cmd.Parameters.AddWithValue("@tempo", txtDuracao.Text.Trim());
                        cmd.Parameters.AddWithValue("@deslocamento", cboDeslocamento.SelectedItem.Text);
                        cmd.Parameters.AddWithValue("@situacao", ddlStatus.SelectedItem.Text);
                        cmd.Parameters.AddWithValue("@data_alteracao", txtAltCad.Text.Trim());
                        cmd.Parameters.AddWithValue("@usuario_alteracao", lblDtCadastro.Text.Trim());
                        cmd.Parameters.AddWithValue("@rota", id); // rota (chave)

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }

        }
    }
}