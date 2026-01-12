using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NewCapit.dist.pages
{
    public partial class Frm_TabelaPrecoMatriz : System.Web.UI.Page
    {
        SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString());
        string nomeUsuario = string.Empty;
        DateTime dataHoraAtual = DateTime.Now;
        string lotacaomin;
        string radioSim;
        string radioNao;
        string customRadioFrota;
        string customRadioAgregado;
        string nomeRota;
        string sCepOrigem;
        string sCepDestino;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UsuarioLogado"] != null)
                {
                    string nomeUsuario = Session["UsuarioLogado"].ToString();
                    var lblUsuario = nomeUsuario;
                    txtUsuCadastro.Text = lblUsuario.Trim().ToUpper();
                }
                else
                {
                    var lblUsuario = "<Usuário>";
                    txtUsuCadastro.Text = lblUsuario.Trim().ToUpper();
                }

                PreencherComboRemetente();
                PreencherComboExpedidor();
                PreencherComboDestinatario();
                PreencherComboRecebedor();                
                PreencherComboConsignario();
                PreencherComboPagador();
                PreencherComboTipoVeiculos();
                PreencherComboMateriais();
                PreencherComboTipoViagens();
                PreencherComboMotorista();
                PreencherNumTabelaDeFrete();

                //PreencherCombosClientes();
                // PreencherComboRotas();
                

            }

            //DateTime dataHoraAtual = DateTime.Now;
            txtCadastro.Text = dataHoraAtual.ToString("dd/MM/yyyy HH:mm");
            lblDtCadastro.Text = dataHoraAtual.ToString("dd/MM/yyyy HH:mm"); 
            txtStatusRota.Text = "ATIVO";
        }
        private void PreencherCombosClientes()
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT DISTINCT id, razcli FROM tbclientes ORDER BY razcli", conn);

                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                                
                cboRemetente.Items.Clear();
                cboExpedidor.Items.Clear();
                cboDestinatario.Items.Clear();
                cboRecebedor.Items.Clear();
                cboConsignatario.Items.Clear();
                cboPagador.Items.Clear();

                cboRemetente.Items.Add(" Selecione...");
                cboExpedidor.Items.Add(" Selecione...");
                cboDestinatario.Items.Add(" Selecione...");
                cboRecebedor.Items.Add(" Selecione...");
                cboConsignatario.Items.Add(" Selecione...");
                cboPagador.Items.Add(" Selecione...");

                while (dr.Read())
                {
                    cboRemetente.Items.Add(dr["razcli"].ToString());
                    cboExpedidor.Items.Add(dr["razcli"].ToString());
                    cboDestinatario.Items.Add(dr["razcli"].ToString());
                    cboRecebedor.Items.Add(dr["razcli"].ToString());
                    cboConsignatario.Items.Add(dr["razcli"].ToString());
                    cboPagador.Items.Add(dr["razcli"].ToString());
                }
            }

        }                      
        private void PreencherComboMateriais()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT codigo, descricao FROM tbtipodematerial order by descricao";

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
                    cboTipoMaterial.DataSource = reader;
                    cboTipoMaterial.DataTextField = "descricao";  // Campo que será mostrado no ComboBox
                    cboTipoMaterial.DataValueField = "codigo";  // Campo que será o valor de cada item                    
                    cboTipoMaterial.DataBind();  // Realiza o binding dos dados                   
                    cboTipoMaterial.Items.Insert(0, new ListItem("Selecione...", "0"));
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
        private void PreencherComboMotorista()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT id, codmot, nommot, codtra, transp FROM tbmotoristas where fl_exclusao is null and status = 'ATIVO' and tipomot != 'FUNCIONÁRIO' order by nommot";

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
                    cboNomAgregado.DataSource = reader;
                    cboNomAgregado.DataTextField = "nommot";  // Campo que será mostrado no ComboBox
                    cboNomAgregado.DataValueField = "id";  // Campo que será o valor de cada item                    
                    cboNomAgregado.DataBind();  // Realiza o binding dos dados                   
                    cboNomAgregado.Items.Insert(0, new ListItem("Selecione...", "0"));
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
        private void PreencherComboTipoVeiculos()
        {
            // Consulta SQL que retorna os dados desejados
            string query = @"
            SELECT DISTINCT RTRIM(LTRIM(descricao_tng)) AS descricao_tng
            FROM tbtipoveic
            WHERE descricao_tng IS NOT NULL
            ORDER BY descricao_tng";

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
                    cboTipoVeiculo.DataSource = reader;
                    cboTipoVeiculo.DataTextField = "descricao_tng";  // Campo que será mostrado no ComboBox
                    cboTipoVeiculo.DataValueField = "descricao_tng";  // Campo que será o valor de cada item                    
                    cboTipoVeiculo.DataBind();  // Realiza o binding dos dados                   
                    cboTipoVeiculo.Items.Insert(0, new ListItem("Selecione...", "0"));
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
        private void PreencherComboTipoViagens()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT codigo, descricao FROM tbtiposdeviagem order by descricao";

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
                    cboTipoViagem.DataSource = reader;
                    cboTipoViagem.DataTextField = "descricao";  // Campo que será mostrado no ComboBox
                    cboTipoViagem.DataValueField = "codigo";  // Campo que será o valor de cada item                    
                    cboTipoViagem.DataBind();  // Realiza o binding dos dados                   
                    cboTipoViagem.Items.Insert(0, new ListItem("Selecione...", "0"));
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
            string query = "SELECT codcli, razcli, cidcli, estcli, ativo_inativo, fl_exclusao FROM tbclientes where ativo_inativo = 'ATIVO' order by razcli";

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
                    cboRemetente.DataTextField = "razcli";  // Campo que será mostrado no ComboBox
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
            string query = "SELECT codcli, razcli, cidcli, estcli, ativo_inativo, fl_exclusao FROM tbclientes where ativo_inativo = 'ATIVO' order by razcli";

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
                    cboExpedidor.DataTextField = "razcli";  // Campo que será mostrado no ComboBox
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
            string query = "SELECT codcli, razcli, cidcli, estcli, ativo_inativo, fl_exclusao FROM tbclientes where ativo_inativo = 'ATIVO' order by razcli";

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
                    cboDestinatario.DataTextField = "razcli";  // Campo que será mostrado no ComboBox
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
            string query = "SELECT codcli, razcli, cidcli, estcli, ativo_inativo, fl_exclusao FROM tbclientes where ativo_inativo = 'ATIVO' order by razcli";

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
                    cboRecebedor.DataTextField = "razcli";  // Campo que será mostrado no ComboBox
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
        private void PreencherComboConsignario()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT codcli, razcli, cidcli, estcli, ativo_inativo, fl_exclusao FROM tbclientes where ativo_inativo = 'ATIVO' order by razcli";

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
                    cboConsignatario.DataSource = reader;
                    cboConsignatario.DataTextField = "razcli";  // Campo que será mostrado no ComboBox
                    cboConsignatario.DataValueField = "codcli";  // Campo que será o valor de cada item                    
                    cboConsignatario.DataBind();  // Realiza o binding dos dados                   
                    cboConsignatario.Items.Insert(0, new ListItem("Selecione...", "0"));
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
            string query = "SELECT codcli, razcli, cidcli, estcli, ativo_inativo, fl_exclusao FROM tbclientes where ativo_inativo = 'ATIVO' order by razcli";

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
                    cboPagador.DataTextField = "razcli";  // Campo que será mostrado no ComboBox
                    cboPagador.DataValueField = "codcli";  // Campo que será o valor de cada item                    
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
        private void PreencherNumTabelaDeFrete()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT (tabela_frete + incremento) as ProximaTabela FROM tbcontadores";

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
                                novaTabelaDeFrete.Text = reader["ProximaTabela"].ToString();
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
                    string sql = @"UPDATE tbcontadores SET tabela_frete = @tabela_frete WHERE id = @id";
                    try
                    {
                        using (SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
                        using (SqlCommand cmd = new SqlCommand(sql, con))
                        {
                            cmd.Parameters.AddWithValue("@tabela_frete", novaTabelaDeFrete.Text);
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
                                string script = "<script>showToast('Erro ao atualizar o número do carregamento.');</script>";
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

        protected void txtCodRemetente_TextChanged(object sender, EventArgs e)
        {
            if (txtCodRemetente.Text != "")
            {
                string cod = txtCodRemetente.Text;
                string sql = "SELECT codcli, razcli, cidcli, estcli, ativo_inativo, fl_exclusao FROM tbclientes where codcli = '" + cod + "' and ativo_inativo = 'ATIVO' and fl_exclusao is null";
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
                string query = "SELECT codcli, razcli, cidcli, estcli FROM tbclientes WHERE codcli = @ID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtCodRemetente.Text = reader["codcli"].ToString();
                    txtMunicipioRemetente.Text = reader["cidcli"].ToString();
                    txtUFRemetente.Text = reader["estcli"].ToString();
                }
            }
        }
        // Função para limpar os campos
        private void LimparCamposRemetente()
        {
            txtCodRemetente.Text = string.Empty;
            cboRemetente.SelectedItem.Text = string.Empty;
            txtMunicipioRemetente.Text = string.Empty;
            txtUFRemetente.Text = string.Empty;
        }

        protected void txtCodExpedidor_TextChanged(object sender, EventArgs e)
        {
            if (txtCodExpedidor.Text != "")
            {
                string cod = txtCodExpedidor.Text;
                string sql = "SELECT codcli, razcli, cidcli, estcli, ativo_inativo, fl_exclusao, cepcli FROM tbclientes where codcli = '" + cod + "' and ativo_inativo = 'ATIVO' and fl_exclusao is null";
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
                        txtCidExpedidor.Text = dt.Rows[0][2].ToString();
                        txtUFExpedidor.Text = dt.Rows[0][3].ToString();
                        sCepOrigem = dt.Rows[0][6].ToString();
                        txtCodDestinatario.Focus();
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
                string query = "SELECT codcli, razcli, cidcli, estcli FROM tbclientes WHERE codcli = @ID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtCodExpedidor.Text = reader["codcli"].ToString();
                    txtCidExpedidor.Text = reader["cidcli"].ToString();
                    txtUFExpedidor.Text = reader["estcli"].ToString();
                }
            }
        }
        // Função para limpar os campos
        private void LimparCamposExpedidor()
        {
            txtCodExpedidor.Text = string.Empty;
            cboExpedidor.SelectedItem.Text = string.Empty;
            txtCidExpedidor.Text = string.Empty;
            txtUFExpedidor.Text = string.Empty;
        }

        protected void txtCodDestinatario_TextChanged(object sender, EventArgs e)
        {
            if (txtCodDestinatario.Text != "")
            {
                string cod = txtCodDestinatario.Text;
                string sql = "SELECT codcli, razcli, cidcli, estcli, ativo_inativo, fl_exclusao FROM tbclientes where codcli = '" + cod + "' and ativo_inativo = 'ATIVO' and fl_exclusao is null";
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
                        txtMunicipioDestinatario.Text = dt.Rows[0][2].ToString();
                        txtUFDestinatario.Text = dt.Rows[0][3].ToString();
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
                string query = "SELECT codcli, razcli, cidcli, estcli FROM tbclientes WHERE codcli = @ID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtCodDestinatario.Text = reader["codcli"].ToString();
                    txtMunicipioDestinatario.Text = reader["cidcli"].ToString();
                    txtUFDestinatario.Text = reader["estcli"].ToString();
                }
            }
        }
        // Função para limpar os campos
        private void LimparCamposDestinatario()
        {
            txtCodDestinatario.Text = string.Empty;
            cboDestinatario.SelectedItem.Text = string.Empty;
            txtMunicipioDestinatario.Text = string.Empty;
            txtUFDestinatario.Text = string.Empty;
        }

        protected void txtCodRecebedor_TextChanged(object sender, EventArgs e)
        {
            if (txtCodRecebedor.Text != "")
            {
                string cod = txtCodRecebedor.Text;
                string sql = "SELECT codcli, razcli, cidcli, estcli, cepcli, ativo_inativo, fl_exclusao, cepcli FROM tbclientes where codcli = '" + cod + "' and ativo_inativo = 'ATIVO' and fl_exclusao is null";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                conn.Open();
                da.Fill(dt);
                conn.Close();

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0][6].ToString() == null)
                    {
                        // Acione o toast quando a página for carregada
                        string script = "<script>showToast('Cliente deletado do sistema.');</script>";
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                        txtCodRecebedor.Text = "";                        
                        txtCodRecebedor.Focus();
                        return;
                    }
                    else if (dt.Rows[0][5].ToString() == "INATIVO")
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
                        cboRecebedor.SelectedItem.Text = dt.Rows[0][1].ToString();
                        txtCidRecebedor.Text = dt.Rows[0][2].ToString();
                        txtUFRecebedor.Text = dt.Rows[0][3].ToString();
                        sCepDestino = dt.Rows[0][4].ToString();
                        txtCodConsignatario.Focus();
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
                string query = "SELECT codcli, razcli, cidcli, estcli FROM tbclientes WHERE codcli = @ID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtCodRecebedor.Text = reader["codcli"].ToString();
                    txtCidRecebedor.Text = reader["cidcli"].ToString();
                    txtUFRecebedor.Text = reader["estcli"].ToString();
                }
            }
        }
        // Função para limpar os campos
        private void LimparCamposRecebedor()
        {
            txtCodRecebedor.Text = string.Empty;
            cboRecebedor.SelectedItem.Text = string.Empty;
            txtCidRecebedor.Text = string.Empty;
            txtUFRecebedor.Text = string.Empty;
        }

        protected void txtCodConsignatario_TextChanged(object sender, EventArgs e)
        {
            if (txtCodConsignatario.Text != "")
            {
                string cod = txtCodConsignatario.Text;
                string sql = "SELECT codcli, razcli, cidcli, estcli, ativo_inativo, fl_exclusao FROM tbclientes where codcli = '" + cod + "' and ativo_inativo = 'ATIVO' and fl_exclusao is null";
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
                        txtCodConsignatario.Text = "";
                        txtCodConsignatario.Focus();
                        return;
                    }
                    else if (dt.Rows[0][4].ToString() == "INATIVO")
                    {
                        // Acione o toast quando a página for carregada
                        string script = "<script>showToast('Cliente inativo no sistema.');</script>";
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                        txtCodConsignatario.Text = "";
                        txtCodConsignatario.Focus();
                        return;
                    }
                    else
                    {
                        cboConsignatario.SelectedItem.Text = dt.Rows[0][1].ToString();
                        txtCidConsignatario.Text = dt.Rows[0][2].ToString();
                        txtUFConsignatario.Text = dt.Rows[0][3].ToString();
                        txtCodPagador.Focus();
                        return;
                    }

                }
                else
                {
                    // Acione o toast quando a página for carregada
                    string script = "<script>showToast('Cliente não encontrado no sistema.');</script>";
                    ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                    txtCodConsignatario.Text = "";
                    txtCodConsignatario.Focus();
                    return;
                }
            }

        }
        protected void cboConsignatario_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idSelecionado = int.Parse(cboConsignatario.SelectedValue);

            // Preencher os campos com base no valor selecionado
            if (idSelecionado > 0)
            {
                PreencherCamposConsignatario(idSelecionado);
            }
            else
            {
                LimparCamposConsignatario();
            }
        }
        // Função para preencher os campos com os dados do banco
        private void PreencherCamposConsignatario(int id)
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                conn.Open();
                string query = "SELECT codcli, razcli, cidcli, estcli FROM tbclientes WHERE codcli = @ID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtCodConsignatario.Text = reader["codcli"].ToString();
                    txtCidConsignatario.Text = reader["cidcli"].ToString();
                    txtUFConsignatario.Text = reader["estcli"].ToString();
                }
            }
        }
        // Função para limpar os campos
        private void LimparCamposConsignatario()
        {
            txtCodConsignatario.Text = string.Empty;
            cboConsignatario.SelectedItem.Text = string.Empty;
            txtCidConsignatario.Text = string.Empty;
            txtUFConsignatario.Text = string.Empty;
        }

        protected void txtCodPagador_TextChanged(object sender, EventArgs e)
        {
            if (txtCodPagador.Text != "")
            {
                string cod = txtCodPagador.Text;
                string sql = "SELECT codcli, razcli, cidcli, estcli, ativo_inativo, fl_exclusao FROM tbclientes where codcli = '" + cod + "' and ativo_inativo = 'ATIVO' and fl_exclusao is null";
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
                        txtCodPagador.Text = "";
                        txtCodPagador.Focus();
                        return;
                    }
                    else if (dt.Rows[0][4].ToString() == "INATIVO")
                    {
                        // Acione o toast quando a página for carregada
                        string script = "<script>showToast('Cliente inativo no sistema.');</script>";
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                        txtCodPagador.Text = "";
                        txtCodPagador.Focus();
                        return;
                    }
                    else
                    {
                        cboPagador.SelectedItem.Text = dt.Rows[0][1].ToString();
                        txtCidPagador.Text = dt.Rows[0][2].ToString();
                        txtUFPagador.Text = dt.Rows[0][3].ToString();
                       // return;
                        
                    }
                    
                    if (txtCodRemetente.Text == "")
                    {
                        MostrarMsg("Digite o Remetente, por favor!", "danger");
                        txtCodRemetente.Text = string.Empty;
                        txtCodRemetente.Focus();
                        return;
                    }
                    if (txtCodExpedidor.Text == "")
                    {
                        MostrarMsg("Digite o Expedidor, por favor!", "danger");
                        txtCodExpedidor.Text = string.Empty;
                        txtCodExpedidor.Focus();
                        return;
                    }
                    if (txtCodDestinatario.Text == "")
                    {
                        MostrarMsg("Digite o Destinatário, por favor!", "danger");
                        txtCodDestinatario.Text = string.Empty;
                        txtCodDestinatario.Focus();
                        return;
                    }
                    if (txtCodRecebedor.Text == "")
                    {
                        MostrarMsg("Digite o Recebedor, por favor!", "danger");
                        txtCodRecebedor.Text = string.Empty;
                        txtCodRecebedor.Focus();
                        return;
                    }
                    if (txtCodPagador.Text == "")
                    {
                        MostrarMsg("Digite o Pagador, por favor!", "danger");
                        txtCodPagador.Text = string.Empty;
                        txtCodPagador.Focus();
                        return;
                    }
                    string nomeRota = txtCidExpedidor.Text.Trim() + "/" + txtUFExpedidor.Text.Trim() + " X " + txtCidRecebedor.Text.Trim() + "/" + txtUFRecebedor.Text.Trim();

                    BuscarRota(nomeRota);
                    CepOrigemDestino();

                }
                else
                {
                    // Acione o toast quando a página for carregada
                    string script = "<script>showToast('Cliente não encontrado no sistema.');</script>";
                    ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                    txtCodPagador.Text = "";
                    txtCodPagador.Focus();
                    return;
                }
            }

        }
        public void CepOrigemDestino()
        {
            string sqle = "select endcli,cidcli,estcli from tbclientes where codcli='" + txtCodExpedidor.Text+"' and ativo_inativo = 'ATIVO' and fl_exclusao is null";
            SqlDataAdapter dae = new SqlDataAdapter(sqle, conn);
            DataTable dte = new DataTable();
            conn.Open();
            dae.Fill(dte);
            conn.Close();

            string sqlr = "select endcli,cidcli,estcli from tbclientes where codcli='" + txtCodDestinatario.Text + "' and ativo_inativo = 'ATIVO' and fl_exclusao is null";
            SqlDataAdapter dar = new SqlDataAdapter(sqlr, conn);
            DataTable dtr = new DataTable();
            conn.Open();
            dar.Fill(dtr);
            conn.Close();

            string ceporigem = dte.Rows[0][0].ToString()+", "+ dte.Rows[0][1].ToString() + ", "+ dte.Rows[0][2].ToString() + "";
            string cepdestino = dtr.Rows[0][0].ToString() + ", " + dtr.Rows[0][1].ToString() + ", " + dtr.Rows[0][2].ToString() + "";

            BuscarPorCep(ceporigem, cepdestino);

        }
        public void BuscarPorCep(string cepOrigem_, string cepDestino_)
        {
            try
            {
                string key = "AIzaSyApI6da0E4OJktNZ-zZHgL6A5jtk0L6Cww";
                string url = "https://routes.googleapis.com/directions/v2:computeRoutes";

                // 🛣️ Endereços SEM número (padrão)
                string origem =  cepOrigem_+", Brasil";

                string destino = cepDestino_ + ", Brasil";


                //if (string.IsNullOrWhiteSpace(txtRuaOrigem.Text) ||
                //    string.IsNullOrWhiteSpace(txtRuaDestino.Text))
                //    throw new Exception("Informe a rua de origem e destino.");

                string jsonBody = $@"
                    {{
                      ""origin"": {{
                        ""address"": ""{origem}""
                      }},
                      ""destination"": {{
                        ""address"": ""{destino}""
                      }},
                      ""travelMode"": ""DRIVE"",
                      ""routingPreference"": ""TRAFFIC_AWARE"",
                      ""computeAlternativeRoutes"": false
                    }}";

                using (var client = new WebClient())
                {
                    client.Headers.Add("Content-Type", "application/json");
                    client.Headers.Add("X-Goog-Api-Key", key);
                    client.Headers.Add(
                        "X-Goog-FieldMask",
                        "routes.distanceMeters,routes.duration"
                    );

                    string response = client.UploadString(url, "POST", jsonBody);
                    dynamic data = JsonConvert.DeserializeObject(response);

                    if (data.routes == null || data.routes.Count == 0)
                        throw new Exception("Rota não encontrada para os endereços informados.");

                    // 📏 Distância (km)
                    double metros = (double)data.routes[0].distanceMeters;
                    txtDistanciaCEP.Text = (metros / 1000).ToString("0.##");

                    // ⏱ Tempo (min)
                    string duracaoStr = data.routes[0].duration.ToString().Replace("s", "");
                    double segundos = double.Parse(duracaoStr, CultureInfo.InvariantCulture);

                    TimeSpan tempo = TimeSpan.FromSeconds(segundos);
                    //txtTempoCEP.Text = tempo.ToString(@"hh\:mm");


                }
            }
            catch (Exception ex)
            {
               
                MostrarMsg(
                    "Erro ao calcular rota: " + ex.Message,
                    "danger"
                );
            }
        }
       
        public RotaEntrega ObterRotaPorDescricao(string nomeRota)
        {
            const string sql = @"
            SELECT 
                rota,
                desc_rota,
                distancia,
                tempo,
                deslocamento,
                pedagio
            FROM tbrotasdeentregas
            WHERE desc_rota COLLATE Latin1_General_CI_AI LIKE '%' + @nomeRota + '%'
        ";

            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add("@nomeRota", SqlDbType.VarChar).Value = nomeRota;
                conn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (!dr.Read())
                        return null;

                    return new RotaEntrega
                    {
                        Rota = DbSafe.ToString(dr["rota"]),
                        Descricao = DbSafe.ToString(dr["desc_rota"]),
                        Distancia = DbSafe.ToDecimal(dr["distancia"]),
                        Tempo = DbSafe.ToTimeSpan(dr["tempo"]),
                        EmitePedagio = DbSafe.ToString(dr["pedagio"]),
                        Percurso = DbSafe.ToString(dr["deslocamento"])
                    };
                }
            }
        }
        public class RotaEntrega
        {
            public string Rota { get; set; }
            public string Descricao { get; set; }
            public decimal Distancia { get; set; }
            public TimeSpan Tempo { get; set; }
            public string Percurso { get; set; }
            public string EmitePedagio { get; set; }
        }
        private void PreencherCamposRota(RotaEntrega rota)
        {
            txtRota.Text = rota.Rota;

            txtDistanciaCentro.Text = rota.Distancia.ToString("N2");
            txtDistancia.Text = rota.Distancia.ToString("N2");

            txtTempoCentro.Text = rota.Tempo.ToString(@"hh\:mm");
            txtDuracao.Text = rota.Tempo.ToString(@"hh\:mm");

            txtDesc_Rota.Text = rota.Descricao;
            txtDeslocamento.Text = rota.Percurso;
            txtEmitePedagio.Text = rota.EmitePedagio;
        }
        protected void BuscarRota(string nomeRota)
        {
            var rota = ObterRotaPorDescricao(nomeRota);

            if (rota == null)
            {
               // LimparCamposRota();
                return;
            }

            PreencherCamposRota(rota);
        }


        protected void cboPagador_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idSelecionado = int.Parse(cboPagador.SelectedValue);

            // Preencher os campos com base no valor selecionado
            if (idSelecionado > 0)
            {
                PreencherCamposPagador(idSelecionado);
            }
            else
            {
                LimparCamposPagador();
            }
        }
        // Função para preencher os campos com os dados do banco
        private void PreencherCamposPagador(int id)
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                conn.Open();
                string query = "SELECT codcli, razcli, cidcli, estcli FROM tbclientes WHERE codcli = @ID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtCodPagador.Text = reader["codcli"].ToString();
                    txtCidPagador.Text = reader["cidcli"].ToString();
                    txtUFPagador.Text = reader["estcli"].ToString();
                }
            }
        }
        // Função para limpar os campos
        private void LimparCamposPagador()
        {
            txtCodPagador.Text = string.Empty;
            cboPagador.SelectedItem.Text = string.Empty;
            txtCidPagador.Text = string.Empty;
            txtUFPagador.Text = string.Empty;
        }

        protected void txtCodAgregado_TextChanged(object sender, EventArgs e)
        {
            if (txtCodAgregado.Text != "")
            {
                string cod = txtCodAgregado.Text;
                string sql = "SELECT * FROM tbmotoristas where codmot = '" + cod + "' and status = 'ATIVO' and tipomot != 'FUNCIONÁRIO' and fl_exclusao is null";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                conn.Open();
                da.Fill(dt);
                conn.Close();

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0][68].ToString() == null)
                    {
                        // Acione o toast quando a página for carregada
                        string script = "<script>showToast('Agregado/Terceiro deletado do sistema.');</script>";
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                        txtCodAgregado.Text = "";
                        txtCodAgregado.Focus();
                        return;
                    }
                    else if (dt.Rows[0][3].ToString() == "INATIVO")
                    {
                        // Acione o toast quando a página for carregada
                        string script = "<script>showToast('Agregado/Terceiro inativa no sistema.');</script>";
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                        txtCodAgregado.Text = "";
                        cboNomAgregado.SelectedItem.Text = "";
                        txtCodTra.Text = "";
                        txtTransp.Text = "";
                        txtCodAgregado.Text = "";
                        txtCodAgregado.Focus();
                        return;
                    }
                    else if (dt.Rows[0][44].ToString() == "FUNCIONÁRIO")
                    {
                        // Acione o toast quando a página for carregada
                        string script = "<script>showToast('Motorista inválido, é funcionário.');</script>";
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                        txtCodAgregado.Text = "";
                        cboNomAgregado.SelectedItem.Text = "";
                        txtCodTra.Text = "";
                        txtTransp.Text = "";
                        txtCodAgregado.Text = "";
                        txtCodAgregado.Focus();
                        return;
                    }
                    else
                    {
                        txtCodAgregado.Text = dt.Rows[0][1].ToString();
                        cboNomAgregado.SelectedItem.Text = dt.Rows[0][2].ToString();
                        txtCodTra.Text = dt.Rows[0][29].ToString();
                        txtTransp.Text = dt.Rows[0][30].ToString();
                        return;
                    }

                }
                else
                {
                    // Acione o toast quando a página for carregada
                    string script = "<script>showToast('Agregado/Terceiro não encontrado no sistema.');</script>";
                    ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                    txtCodAgregado.Text = "";
                    cboNomAgregado.SelectedItem.Text = "";
                    txtCodTra.Text = "";
                    txtTransp.Text = "";

                    txtCodAgregado.Text = "";
                    txtCodAgregado.Focus();
                    return;
                }
            }
        }
        protected void cboNomAgregado_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idSelecionado = int.Parse(cboNomAgregado.SelectedValue);

            // Preencher os campos com base no valor selecionado
            if (idSelecionado > 0)
            {
                PreencherCamposMotorista(idSelecionado);
            }
            else
            {
                LimparCamposMotorista();
            }
        }
        private void PreencherCamposMotorista(int id)
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                conn.Open();
                string query = "SELECT codmot, nommot, codtra, transp FROM tbmotoristas WHERE id = @ID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtCodAgregado.Text = reader["codmot"].ToString();
                    cboNomAgregado.SelectedItem.Text = reader["nommot"].ToString();
                    txtCodTra.Text = reader["codtra"].ToString();
                    txtTransp.Text = reader["transp"].ToString();
                    return;
                }
            }
        }
        // Função para limpar os campos
        private void LimparCamposMotorista()
        {
            //txtCodPagador.Text = string.Empty;
            //txtCidPagador.Text = string.Empty;
            //txtUFPagador.Text = string.Empty;
        }            

        protected void btnAlterar_Click(object sender, EventArgs e)
        {
            // 🔹 Verifica se campos obrigatórios (que compõem descr_frete) estão preenchidos
            if (string.IsNullOrWhiteSpace(txtCodPagador.Text) ||
                cboPagador.SelectedItem == null ||
                string.IsNullOrWhiteSpace(txtCodExpedidor.Text) ||
                cboExpedidor.Text == null ||
                string.IsNullOrWhiteSpace(txtCidExpedidor.Text) ||
                string.IsNullOrWhiteSpace(txtUFExpedidor.Text) ||
                string.IsNullOrWhiteSpace(txtCodRecebedor.Text) ||
                cboRecebedor.Text == null ||
                string.IsNullOrWhiteSpace(txtCidRecebedor.Text) ||
                string.IsNullOrWhiteSpace(txtUFRecebedor.Text) ||
                cboTipoMaterial.SelectedItem == null ||
                cboTipoVeiculo.SelectedItem == null)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "CamposNulos",
                    "<script>alert('⚠️ Campos obrigatórios para gerar a descrição do frete não podem estar vazios.');</script>");
                return;
            }           

            string valor = Request.Form["customRadioTipo"]; // nome do grupo
            if (!string.IsNullOrEmpty(valor))
                lotacaomin = valor;
            else
                lotacaomin = "NÃO";

            // 🔹 Monta o campo descr_frete
            string[] pagador = cboPagador.SelectedItem.Text.Split(' ');
            string[] remetente = cboRemetente.Text.Split(' ');
            string[] expedidor = cboExpedidor.Text.Split(' ');
            string[] destinatario = cboDestinatario.Text.Split(' ');
            string[] recebedor = cboRecebedor.Text.Split(' ');

            string descr_frete = $"{txtCodPagador.Text} - {pagador[0]} - Exped./Recb.: {txtCodExpedidor.Text} - {expedidor[0]}({txtCidExpedidor.Text}/{txtUFExpedidor.Text})x {txtCodRecebedor.Text} - {recebedor[0]}({txtCidRecebedor.Text}/{txtUFRecebedor.Text}) - Material: {cboTipoMaterial.SelectedItem.Text} - Veículo: {cboTipoVeiculo.SelectedItem.Text}";

            string connectionString = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // 🔹 Verifica duplicidade de desc_frete
                string verificaSql = "SELECT COUNT(*) FROM tbtabeladefretes WHERE desc_frete = @desc_frete";
                using (SqlCommand verificaCmd = new SqlCommand(verificaSql, conn))
                {
                    verificaCmd.Parameters.AddWithValue("@desc_frete", descr_frete);
                    int existe = (int)verificaCmd.ExecuteScalar();

                    if (existe > 0)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "Duplicado",
                            "<script>alert('❌ Já existe um frete com a mesma descrição: "+descr_frete+". Inclusão cancelada.');</script>");
                        return;
                    }
                }

                // 🔹 Monta o INSERT completo com todos os campos
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
                {
                    string sql = @"
                    INSERT INTO tbtabeladefretes
                    (
                        cod_frete, desc_frete, rota, desc_rota,  cod_remetente, remetente, cid_remetente, uf_remetente, cod_expedidor, expedidor, cid_expedidor, uf_expedidor,  cod_destinatario, destinatario, cid_destinatario, uf_destinatario, cod_recebedor, recebedor, cid_recebedor, uf_recebedor,  cod_consignatario, consignatario, cid_consignatario, uf_consignatario,  cod_pagador, pagador, cid_pagador, uf_pagador,  distancia, Tempo, frete_tng, frete_agregado, frete_agregado_com_desc_carreta, frete_terceiro, adicional_sobrenf, sec_cat, despacho, pedagio, outros, tipo_veiculo, tipo_material, data_cadastro, situacao, tipo_viagem, deslocamento, vigencia_inicial, vigencia_final, lotacao_minima, valor_fixo_terceiro, aluguel_carreta, desc_carreta, valor_fixo_tng, valor_especial, desc_especial, valor_com_desconto_especial,  observacao, cadastro_usuario, emitepedagio, vigencia_inicial_agregado, vigencia_final_agregado, vigencia_inicial_terceiro, vigencia_final_terceiro, despesa_adm,  codmot_especial, nommot_especial, codtra_especial, transp_especial, perc_frete_agregado, perc_frete_terceiro, perc_frete_especial, cobra_hora_parada, valor_hora_parada, franquia_hora_parada
                    )
                    VALUES
                    (
                        @cod_frete, @desc_frete, @rota, @desc_rota,  @cod_remetente, @remetente, @cid_remetente, @uf_remetente, @cod_expedidor, @expedidor, @cid_expedidor, @uf_expedidor,  @cod_destinatario, @destinatario, @cid_destinatario, @uf_destinatario, @cod_recebedor, @recebedor, @cid_recebedor, @uf_recebedor,  @cod_consignatario, @consignatario, @cid_consignatario, @uf_consignatario,  @cod_pagador, @pagador, @cid_pagador, @uf_pagador,  @distancia, @Tempo, @frete_tng, @frete_agregado, @frete_agregado_com_desc_carreta, @frete_terceiro, @adicional_sobrenf, @sec_cat, @despacho, @pedagio, @outros, @tipo_veiculo, @tipo_material, @data_cadastro, @situacao, @tipo_viagem, @deslocamento, @vigencia_inicial, @vigencia_final, @lotacao_minima, @valor_fixo_terceiro, @aluguel_carreta, @desc_carreta, @valor_fixo_tng, @valor_especial, @desc_especial, @valor_com_desconto_especial, @observacao, @cadastro_usuario, @emitepedagio, @vigencia_inicial_agregado, @vigencia_final_agregado, @vigencia_inicial_terceiro, @vigencia_final_terceiro, @despesa_adm, @codmot_especial, @nommot_especial, @codtra_especial, @transp_especial, @perc_frete_agregado, @perc_frete_terceiro, @perc_frete_especial,@cobra_hora_parada, @valor_hora_parada, @franquia_hora_parada                      
                    )";

                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        // Campos inteiros
                        cmd.Parameters.Add("@cod_frete", SqlDbType.Int).Value = Convert.ToInt32(novaTabelaDeFrete.Text);
                        cmd.Parameters.Add("@desc_frete", SqlDbType.NVarChar).Value = descr_frete;
                        cmd.Parameters.Add("@rota", SqlDbType.Int).Value = Convert.ToInt32(txtRota.Text);
                        cmd.Parameters.Add("@desc_rota", SqlDbType.NVarChar).Value = txtDesc_Rota.Text;
                        cmd.Parameters.Add("@cod_remetente", SqlDbType.Int).Value = Convert.ToInt32(txtCodRemetente.Text);
                        cmd.Parameters.Add("@remetente", SqlDbType.NVarChar).Value = cboRemetente.SelectedItem.Text.Trim();
                        cmd.Parameters.Add("@cid_remetente", SqlDbType.NVarChar).Value = txtMunicipioRemetente.Text;
                        cmd.Parameters.Add("@uf_remetente", SqlDbType.NVarChar).Value = txtUFRemetente.Text;
                        cmd.Parameters.Add("@cod_expedidor", SqlDbType.Int).Value = Convert.ToInt32(txtCodExpedidor.Text);
                        cmd.Parameters.Add("@expedidor", SqlDbType.NVarChar).Value = cboExpedidor.SelectedItem.Text.Trim();
                        cmd.Parameters.Add("@cid_expedidor", SqlDbType.NVarChar).Value = txtCidExpedidor.Text;
                        cmd.Parameters.Add("@uf_expedidor", SqlDbType.NVarChar).Value = txtUFExpedidor.Text;
                        cmd.Parameters.Add("@cod_destinatario", SqlDbType.Int).Value = Convert.ToInt32(txtCodDestinatario.Text);
                        cmd.Parameters.Add("@destinatario", SqlDbType.NVarChar).Value = cboDestinatario.SelectedItem.Text.Trim();
                        cmd.Parameters.Add("@cid_destinatario", SqlDbType.NVarChar).Value = txtMunicipioDestinatario.Text;
                        cmd.Parameters.Add("@uf_destinatario", SqlDbType.NVarChar).Value = txtUFDestinatario.Text;
                        cmd.Parameters.Add("@cod_recebedor", SqlDbType.Int).Value = Convert.ToInt32(txtCodRecebedor.Text);
                        cmd.Parameters.Add("@recebedor", SqlDbType.NVarChar).Value = cboRecebedor.SelectedItem.Text.Trim();
                        cmd.Parameters.Add("@cid_recebedor", SqlDbType.NVarChar).Value = txtCidRecebedor.Text;
                        cmd.Parameters.Add("@uf_recebedor", SqlDbType.NVarChar).Value = txtUFRecebedor.Text;
                        if (txtCodConsignatario.Text == "")
                            cmd.Parameters.Add("@cod_consignatario", SqlDbType.Int).Value = DBNull.Value;
                        else
                            cmd.Parameters.Add("@cod_consignatario", SqlDbType.Int).Value = Convert.ToInt32(txtCodConsignatario.Text);
                        if (cboConsignatario.SelectedItem.Text == "Selecione...")
                            cmd.Parameters.Add("@consignatario", SqlDbType.NVarChar).Value = DBNull.Value;
                        else
                            cmd.Parameters.Add("@consignatario", SqlDbType.NVarChar).Value = cboConsignatario.SelectedItem.Text.Trim();
                        cmd.Parameters.Add("@cid_consignatario", SqlDbType.NVarChar).Value = txtCidConsignatario.Text;
                        cmd.Parameters.Add("@uf_consignatario", SqlDbType.NVarChar).Value = txtUFConsignatario.Text;
                        cmd.Parameters.Add("@cod_pagador", SqlDbType.Int).Value = Convert.ToInt32(txtCodPagador.Text);
                        cmd.Parameters.Add("@pagador", SqlDbType.NVarChar).Value = cboPagador.SelectedItem.Text.Trim();
                        cmd.Parameters.Add("@cid_pagador", SqlDbType.NVarChar).Value = txtCidPagador.Text;
                        cmd.Parameters.Add("@uf_pagador", SqlDbType.NVarChar).Value = txtUFPagador.Text;                        
                        cmd.Parameters.Add("@distancia", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtDistancia.Text);
                        //cmd.Parameters.Add("@Tempo", SqlDbType.Time).Value = txtDuracao.Text;
                        cmd.Parameters.Add("@frete_tng", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtFreteTNG.Text);
                        cmd.Parameters.Add("@frete_agregado", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtFreteAgregado.Text);
                        cmd.Parameters.Add("@frete_agregado_com_desc_carreta", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtFreteAgregadoComDesconto.Text);
                        cmd.Parameters.Add("@frete_terceiro", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtFreteTerceiro.Text);
                        cmd.Parameters.Add("@adicional_sobrenf", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtAdicional.Text);
                        cmd.Parameters.Add("@sec_cat", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtSecCat.Text);
                        cmd.Parameters.Add("@despacho", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtDespacho.Text);
                        cmd.Parameters.Add("@pedagio", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtPedagio.Text);
                        cmd.Parameters.Add("@outros", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtOutros.Text);
                        cmd.Parameters.Add("@tipo_veiculo", SqlDbType.NVarChar).Value = cboTipoVeiculo.SelectedItem.Text;
                        cmd.Parameters.Add("@tipo_material", SqlDbType.NVarChar).Value = cboTipoMaterial.SelectedItem.Text;
                        cmd.Parameters.Add("@data_cadastro", SqlDbType.DateTime).Value = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                        cmd.Parameters.Add("@situacao", SqlDbType.NVarChar).Value = txtStatusRota.Text;
                        cmd.Parameters.Add("@tipo_viagem", SqlDbType.NVarChar).Value = cboTipoViagem.SelectedItem.Text;
                        cmd.Parameters.Add("@deslocamento", SqlDbType.NVarChar).Value = txtDeslocamento.Text;
                        cmd.Parameters.Add("@vigencia_inicial", SqlDbType.Date).Value = SafeDateValue(txtVigenciaInicial.Text);
                        cmd.Parameters.Add("@vigencia_final", SqlDbType.Date).Value = SafeDateValue(txtVigenciaFinal.Text);
                        cmd.Parameters.Add("@lotacao_minima", SqlDbType.NVarChar).Value = lotacaomin;
                        cmd.Parameters.Add("@valor_fixo_terceiro", SqlDbType.NVarChar).Value = ddlTerceiro.SelectedItem.Text;
                        cmd.Parameters.Add("@aluguel_carreta", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtPercentualAluguelCarreta.Text);
                        cmd.Parameters.Add("@desc_carreta", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtAluguelCarretaEspecial.Text);
                        cmd.Parameters.Add("@valor_fixo_tng", SqlDbType.NVarChar).Value = ddlValorFixoTng.SelectedItem.Text;
                        cmd.Parameters.Add("@valor_especial", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtFreteEspecial.Text);
                        cmd.Parameters.Add("@desc_especial", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtAluguelCarretaEspecial.Text);
                        cmd.Parameters.Add("@valor_com_desconto_especial", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtFreteEspecialComDesconto.Text);
                        cmd.Parameters.Add("@observacao", SqlDbType.NVarChar).Value = txtObservacao.Text;
                        cmd.Parameters.Add("@cadastro_usuario", SqlDbType.NVarChar).Value = txtUsuCadastro.Text;
                        cmd.Parameters.Add("@emitepedagio", SqlDbType.NVarChar).Value = txtEmitePedagio.Text;
                        cmd.Parameters.Add("@vigencia_inicial_agregado", SqlDbType.Date).Value = SafeDateValue(txtVigenciaAgregadoInicial.Text);
                        cmd.Parameters.Add("@vigencia_final_agregado", SqlDbType.Date).Value = SafeDateValue(txtVigenciaAgregadoFinal.Text);
                        cmd.Parameters.Add("@vigencia_inicial_terceiro", SqlDbType.Date).Value = SafeDateValue(txtVigenciaTerceiroInicial.Text);
                        cmd.Parameters.Add("@vigencia_final_terceiro", SqlDbType.Date).Value = SafeDateValue(txtVigenciaTerceiroFinal.Text);
                        cmd.Parameters.Add("@despesa_adm", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtDespAdm.Text);
                        cmd.Parameters.Add("@codmot_especial", SqlDbType.NChar).Value = txtCodAgregado.Text;
                        cmd.Parameters.Add("@nommot_especial", SqlDbType.NVarChar).Value = cboNomAgregado.SelectedItem.Text;
                        cmd.Parameters.Add("@codtra_especial", SqlDbType.NChar).Value = txtCodTra.Text;
                        cmd.Parameters.Add("@transp_especial", SqlDbType.NVarChar).Value = txtTransp.Text;
                        cmd.Parameters.Add("@perc_frete_agregado", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtPercTNGAgregado.Text);
                        cmd.Parameters.Add("@perc_frete_terceiro", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtPercTngTerceiro.Text);
                        cmd.Parameters.Add("@perc_frete_especial", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtPercTNGEspecial.Text);
                        cmd.Parameters.Add("@cobra_hora_parada", SqlDbType.NVarChar).Value = ddlHoraParada.SelectedValue;
                        cmd.Parameters.Add("@valor_hora_parada", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtValorFranquia.Text);
                        //cmd.Parameters.Add("@franquia_hora_parada", SqlDbType.Time).Value = txtFranquia.Text;

                        cmd.Parameters.Add("@Tempo", SqlDbType.Time).Value = ToTimeSpanSafe(txtDuracao.Text);
                        
                        string txt = txtFranquia.Text;
                        cmd.Parameters.Add("@franquia_hora_parada", SqlDbType.Time).Value =
                            string.IsNullOrWhiteSpace(txt) ? (object)DBNull.Value : ToTimeSpanSafe(txt);

                        //cmd.Parameters.Add("@franquia_hora_parada", SqlDbType.Time).Value = ToTimeSpanSafe(txtFranquia.Text);


                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }


                ClientScript.RegisterStartupScript(this.GetType(), "Sucesso",
                    "<script>alert('✅ Frete cadastrado com sucesso!');</script>");
            }
            Response.Redirect("/dist/pages/ConsultaFretes.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }
        private object SafeDateValue(string input)
        {
            DateTime dt;
            if (DateTime.TryParse(input, out dt))
                return dt.ToString("yyyy-MM-dd");
            else
                return DBNull.Value;
        }
        private decimal LimparMascaraMoeda(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
            {
                return 0m;
            }
            // Remove pontos de milhar e substitui vírgula decimal por ponto
            string valorLimpo = valor.Replace(".", "").Replace(",", ".");
            if (decimal.TryParse(valorLimpo, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal resultado))
            {
                return resultado;
            }
            return 0m;
        }
        protected void MostrarMsg(string mensagem, string tipo = "warning")
        {
            divMsg.Attributes["class"] = "alert alert-" + tipo + " alert-dismissible fade show mt-3";
            lblMsg.InnerText = mensagem;
            divMsg.Style["display"] = "block";

            string script = @"setTimeout(function() {
                        var div = document.getElementById('divMsg');
                        if (div) div.style.display = 'none';
                      }, 5000);";

            ScriptManager.RegisterStartupScript(this, GetType(), "EscondeMsg", script, true);
        }
        private decimal SafeDecimal(object value)
        {
            if (value == DBNull.Value || value == null)
                return 0m;

            return Convert.ToDecimal(value);
        }

        public static TimeSpan ToTimeSpan(object value)
        {
            if (value == null || value == DBNull.Value)
                return TimeSpan.Zero;

            // SQL TIME vem direto como TimeSpan
            if (value is TimeSpan ts)
                return ts;

            string texto = value.ToString().Trim();

            if (string.IsNullOrEmpty(texto))
                return TimeSpan.Zero;

            // Corrige formatos incompletos
            // 2:30  -> 02:30:00
            // 02:30 -> 02:30:00
            if (texto.Count(c => c == ':') == 1)
                texto += ":00";

            if (TimeSpan.TryParse(texto, out ts))
                return ts;

            // Se tudo falhar, retorna zero (não quebra o sistema)
            return TimeSpan.Zero;
        }
        public static TimeSpan ToTimeSpanSafe(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return TimeSpan.Zero;

            texto = texto.Trim()
                         .Replace("h", "")
                         .Replace("hs", "")
                         .Replace(" ", "");

            // Normaliza 2:30 → 02:30:00
            if (texto.Count(c => c == ':') == 1)
                texto += ":00";

            TimeSpan ts;
            string[] formatos = { @"hh\:mm\:ss", @"h\:mm\:ss", @"hh\:mm", @"h\:mm" };

            if (TimeSpan.TryParseExact(texto, formatos, CultureInfo.InvariantCulture, out ts))
                return ts;

            return TimeSpan.Zero;
        }
        public static class DbSafe
        {
            public static decimal ToDecimal(object value)
            {
                return value == DBNull.Value ? 0m : Convert.ToDecimal(value);
            }

            public static TimeSpan ToTimeSpan(object value)
            {
                if (value == null || value == DBNull.Value)
                    return TimeSpan.Zero;

                // SQL TIME vem direto como TimeSpan
                if (value is TimeSpan ts)
                    return ts;

                string texto = value.ToString().Trim();

                if (string.IsNullOrEmpty(texto))
                    return TimeSpan.Zero;

                // Corrige formatos incompletos
                // 2:30  -> 02:30:00
                // 02:30 -> 02:30:00
                if (texto.Count(c => c == ':') == 1)
                    texto += ":00";

                if (TimeSpan.TryParse(texto, out ts))
                    return ts;

                // Se tudo falhar, retorna zero (não quebra o sistema)
                return TimeSpan.Zero;
            }

            public static string ToString(object value)
            {
                return value == DBNull.Value ? string.Empty : value.ToString();
            }
        }



    }
}