using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using Org.BouncyCastle.Pqc.Crypto.Lms;
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
        int idTabela;
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
               // int tabelaFrete = Convert.ToInt32(novaTabelaDeFrete.Text);
                PreencherComboRemetente();
                PreencherComboExpedidor();
                PreencherComboDestinatario();
                PreencherComboRecebedor();
                PreencherComboConsignario();
                PreencherComboPagador();
                PreencherComboTipoVeiculos();
                PreencherComboMateriais();
                PreencherComboTipoViagens();
                PreencherNumTabelaDeFrete();
                PreencherTabelaANTT();
                ViewState["idTabela"] = novaTabelaDeFrete.Text.Trim();
                int idTabela = Convert.ToInt32(ViewState["idTabela"]);               
            }

            //DateTime dataHoraAtual = DateTime.Now;
            txtCadastro.Text = dataHoraAtual.ToString("dd/MM/yyyy HH:mm");
            lblDtCadastro.Text = dataHoraAtual.ToString("dd/MM/yyyy HH:mm");
            txtStatusRota.Text = "ATIVO";
            ddlTipoCargaANTT.Text = "Carga Geral";
            txtFranquia.Text = "00:00:00";
            decimal total =
            Converter(txtSecCat.Text) +
            Converter(txtDespacho.Text) +
            Converter(txtOutros.Text) +
            Converter(txtDespAdm.Text) +
            Converter(txtGRIS.Text) +
            Converter(txtColeta.Text) +
            Converter(txtEntrega.Text) +
            Converter(txtTDE.Text) +
            Converter(txtTDA.Text) +
            Converter(txtFreteReceber.Text);

            txtTotalFrete.Text = total.ToString("N2");
        }
        private void PreencherTabelaANTT()
        {
            // Consulta SQL que retorna os dados desejados
            string query = @"
            SELECT tabela
            FROM tbresolucoesantt
            WHERE vigente = 'SIM'
            GROUP BY tabela
            ORDER BY tabela ASC";

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
                    ddlTabela.DataSource = reader;
                    ddlTabela.DataTextField = "tabela";  // Campo que será mostrado no ComboBox
                    ddlTabela.DataValueField = "tabela";  // Campo que será o valor de cada item                    
                    ddlTabela.DataBind();  // Realiza o binding dos dados                   
                    ddlTabela.Items.Insert(0, new ListItem("Selecione...", "0"));
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
        private decimal Converter(string valor)
        {
            decimal resultado;
            decimal.TryParse(valor,
                System.Globalization.NumberStyles.Any,
                new System.Globalization.CultureInfo("pt-BR"),
                out resultado);

            return resultado;
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
                string sql = "SELECT codcli, razcli, cidcli, estcli, ativo_inativo, fl_exclusao,cnpj FROM tbclientes where codcli = '" + cod + "' and ativo_inativo = 'ATIVO' and fl_exclusao is null";
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
                        txtCNPJRemetente.Text = dt.Rows[0][6].ToString();
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
                string query = "SELECT codcli, razcli, cidcli, estcli, cnpj FROM tbclientes WHERE codcli = @ID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtCodRemetente.Text = reader["codcli"].ToString();
                    txtMunicipioRemetente.Text = reader["cidcli"].ToString();
                    txtCNPJRemetente.Text = reader["cnpj"].ToString();
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
                string sql = "SELECT codcli, razcli, cidcli, estcli, ativo_inativo, fl_exclusao, cepcli, cnpj FROM tbclientes where codcli = '" + cod + "' and ativo_inativo = 'ATIVO' and fl_exclusao is null";
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
                        txtCNPJExpedidor.Text = dt.Rows[0][7].ToString();
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
                string query = "SELECT codcli, razcli, cidcli, estcli, cnpj FROM tbclientes WHERE codcli = @ID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtCodExpedidor.Text = reader["codcli"].ToString();
                    txtCNPJExpedidor.Text = reader["cnpj"].ToString();
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
                string sql = "SELECT codcli, razcli, cidcli, estcli, ativo_inativo, fl_exclusao, cnpj FROM tbclientes where codcli = '" + cod + "' and ativo_inativo = 'ATIVO' and fl_exclusao is null";
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
                        txtCNPJDestinatario.Text = dt.Rows[0][6].ToString();
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
                string query = "SELECT codcli, razcli, cidcli, estcli, cnpj FROM tbclientes WHERE codcli = @ID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtCodDestinatario.Text = reader["codcli"].ToString();
                    txtCNPJDestinatario.Text = reader["cnpj"].ToString();
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
            txtCNPJDestinatario.Text = string.Empty;
            txtUFDestinatario.Text = string.Empty;
        }

        protected void txtCodRecebedor_TextChanged(object sender, EventArgs e)
        {
            if (txtCodRecebedor.Text != "")
            {
                string cod = txtCodRecebedor.Text;
                string sql = "SELECT codcli, razcli, cidcli, estcli, cepcli, ativo_inativo, fl_exclusao, cnpj FROM tbclientes where codcli = '" + cod + "' and ativo_inativo = 'ATIVO' and fl_exclusao is null";
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
                        txtCNPJRecebedor.Text = dt.Rows[0][7].ToString();
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
                string query = "SELECT codcli, razcli, cidcli, estcli, cnpj FROM tbclientes WHERE codcli = @ID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtCodRecebedor.Text = reader["codcli"].ToString();
                    txtCNPJRecebedor.Text = reader["cnpj"].ToString();
                    txtCidRecebedor.Text = reader["cidcli"].ToString();
                    txtUFRecebedor.Text = reader["estcli"].ToString();
                }
            }
        }
        //Função para limpar os campos
        private void LimparCamposRecebedor()
        {
            txtCodRecebedor.Text = string.Empty;
            cboRecebedor.SelectedItem.Text = string.Empty;
            txtCNPJRecebedor.Text = string.Empty;
            txtCidRecebedor.Text = string.Empty;
            txtUFRecebedor.Text = string.Empty;
        }

        protected void txtCodConsignatario_TextChanged(object sender, EventArgs e)
        {
            if (txtCodConsignatario.Text != "")
            {
                string cod = txtCodConsignatario.Text;
                string sql = "SELECT codcli, razcli, cidcli, estcli, ativo_inativo, fl_exclusao, cnpj FROM tbclientes where codcli = '" + cod + "' and ativo_inativo = 'ATIVO' and fl_exclusao is null";
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
                        txtCNPJConsignatario.Text = dt.Rows[0][6].ToString();
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
                string query = "SELECT codcli, razcli, cidcli, estcli, cnpj FROM tbclientes WHERE codcli = @ID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtCodConsignatario.Text = reader["codcli"].ToString();
                    txtCNPJConsignatario.Text = reader["cnpj"].ToString();
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
            txtCNPJConsignatario.Text = string.Empty;
            txtUFConsignatario.Text = string.Empty;
        }

        protected void txtCodPagador_TextChanged(object sender, EventArgs e)
        {
            if (txtCodPagador.Text != "")
            {
                string cod = txtCodPagador.Text;
                string sql = "SELECT codcli, razcli, cidcli, estcli, ativo_inativo, fl_exclusao, cnpj FROM tbclientes where codcli = '" + cod + "' and ativo_inativo = 'ATIVO' and fl_exclusao is null";
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
                        txtCNPJPagador.Text = dt.Rows[0][6].ToString();
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

                    // 🔹 Monta o campo descr_frete
                    string[] pagador = cboPagador.SelectedItem.Text.Split(' ');
                    string[] remetente = cboRemetente.Text.Split(' ');
                    string[] expedidor = cboExpedidor.Text.Split(' ');
                    string[] destinatario = cboDestinatario.Text.Split(' ');
                    string[] recebedor = cboRecebedor.Text.Split(' ');

                    string descr_frete = $"{txtCodPagador.Text} - {pagador[0]} - Inicio Prestação: {txtCidExpedidor.Text}/{txtUFExpedidor.Text} - Term. Prestação: {txtCidRecebedor.Text}/{txtUFRecebedor.Text}";

                    string connectionString = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        string verificaSql = @"
                        SELECT TOP 1 cod_frete
                        FROM tbtabeladefretes
                        WHERE desc_frete = @desc_frete";

                        using (SqlCommand verificaCmd = new SqlCommand(verificaSql, conn))
                        {
                            verificaCmd.Parameters.AddWithValue("@desc_frete", descr_frete);

                            object resultado = verificaCmd.ExecuteScalar();

                            if (resultado != null)
                            {
                                int cod_frete = Convert.ToInt32(resultado);

                                MostrarMsg(
                                    "Já existe uma tabela com essa descrição. Verifique o código da tabela: " + cod_frete + ".",
                                    "danger");

                                return;
                            }
                        }
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
            string sqle = "select endcli,cidcli,estcli from tbclientes where codcli='" + txtCodExpedidor.Text + "' and ativo_inativo = 'ATIVO' and fl_exclusao is null";
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

            string ceporigem = dte.Rows[0][0].ToString() + ", " + dte.Rows[0][1].ToString() + ", " + dte.Rows[0][2].ToString() + "";
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
                string origem = cepOrigem_ + ", Brasil";

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
                valor_icms, 
                valor_pis, 
                valor_cofins,
                valor_irpj,
                valor_csll,
                valor_ibs,
                valor_cbs,
                valor_iss,
                valor_sestsenat,
                valor_inss,                
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
                        Percurso = DbSafe.ToString(dr["deslocamento"]),
                        ICMS = DbSafe.ToDecimal(dr["valor_icms"]),
                        PIS = DbSafe.ToDecimal(dr["valor_pis"]),
                        COFINS = DbSafe.ToDecimal(dr["valor_cofins"]),
                        IRPJ = DbSafe.ToDecimal(dr["valor_irpj"]),
                        CSLL = DbSafe.ToDecimal(dr["valor_csll"]),
                        IBS = DbSafe.ToDecimal(dr["valor_ibs"]),
                        CBS = DbSafe.ToDecimal(dr["valor_cbs"]),
                        ISS = DbSafe.ToDecimal(dr["valor_iss"]),
                        SESTSENAT = DbSafe.ToDecimal(dr["valor_sestsenat"]),
                        INSS = DbSafe.ToDecimal(dr["valor_inss"])
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
            public decimal ICMS { get; set; }
            public decimal PIS { get; set; }
            public decimal COFINS { get; set; }
            public decimal IRPJ { get; set; }
            public decimal CSLL { get; set; }
            public decimal IBS { get; set; }
            public decimal CBS { get; set; }
            public decimal ISS { get; set; }
            public decimal SESTSENAT { get; set; }
            public decimal INSS { get; set; }
        }
        private void PreencherCamposRota(RotaEntrega rota)
        {
            txtRota.Text = rota.Rota;

            txtDistanciaCentro.Text = rota.Distancia.ToString("N0");
            txtDistancia.Text = rota.Distancia.ToString("N0");

            txtTempoCentro.Text = rota.Tempo.ToString(@"hh\:mm");
            txtDuracao.Text = rota.Tempo.ToString(@"hh\:mm");

            txtDesc_Rota.Text = rota.Descricao;
            txtDeslocamento.Text = rota.Percurso;
            ddlEmitePedagio.SelectedItem.Text = rota.EmitePedagio;
            txtICMS.Text = rota.ICMS.ToString("N2", new CultureInfo("pt-BR"));
            txtPIS.Text = rota.PIS.ToString("N2", new CultureInfo("pt-BR"));
            txtCOFINS.Text = rota.COFINS.ToString("N2", new CultureInfo("pt-BR"));
            txtIRPJ.Text = rota.IRPJ.ToString("N2", new CultureInfo("pt-BR"));
            txtCSLL.Text = rota.CSLL.ToString("N2", new CultureInfo("pt-BR"));
            txtIBS.Text = rota.IBS.ToString("N2", new CultureInfo("pt-BR"));
            txtCBS.Text = rota.CBS.ToString("N2", new CultureInfo("pt-BR"));
            txtISS.Text = rota.ISS.ToString("N2", new CultureInfo("pt-BR"));
            txtSestSenat.Text = rota.SESTSENAT.ToString("N2", new CultureInfo("pt-BR"));
            txtINSS.Text = rota.INSS.ToString("N2", new CultureInfo("pt-BR"));
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
                string query = "SELECT codcli, razcli, cidcli, estcli, cnpj FROM tbclientes WHERE codcli = @ID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtCodPagador.Text = reader["codcli"].ToString();
                    txtCNPJPagador.Text = reader["cnpj"].ToString();
                    txtCidPagador.Text = reader["cidcli"].ToString();
                    txtUFPagador.Text = reader["estcli"].ToString();

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
            }
        }
        // Função para limpar os campos
        private void LimparCamposPagador()
        {
            txtCodPagador.Text = string.Empty;
            cboPagador.SelectedItem.Text = string.Empty;
            txtCidPagador.Text = string.Empty;
            txtCNPJPagador.Text = string.Empty;
            txtUFPagador.Text = string.Empty;
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

            // 🔹 Monta o campo descr_frete
            string[] pagador = cboPagador.SelectedItem.Text.Split(' ');
            string[] remetente = cboRemetente.Text.Split(' ');
            string[] expedidor = cboExpedidor.Text.Split(' ');
            string[] destinatario = cboDestinatario.Text.Split(' ');
            string[] recebedor = cboRecebedor.Text.Split(' ');

            string descr_frete = $"{txtCodPagador.Text} - {pagador[0]} - Inicio Prestação: {txtCidExpedidor.Text}/{txtUFExpedidor.Text} - Term. Prestação: {txtCidRecebedor.Text}/{txtUFRecebedor.Text}";


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
                            "<script>alert('❌ Já existe um frete com a mesma descrição: " + descr_frete + ". Inclusão cancelada.');</script>");
                        return;
                    }
                }

                // 🔹 Monta o INSERT completo com todos os campos
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
                {
                    string sql = @"
                    INSERT INTO tbtabeladefretes
                    (
                        cod_frete, desc_frete, rota, desc_rota,  cod_remetente, remetente, cid_remetente, uf_remetente, cod_expedidor, expedidor, cid_expedidor, uf_expedidor,  cod_destinatario, destinatario, cid_destinatario, uf_destinatario, cod_recebedor, recebedor, cid_recebedor, uf_recebedor,  cod_consignatario, consignatario, cid_consignatario, uf_consignatario,  cod_pagador, pagador, cid_pagador, uf_pagador,  distancia, Tempo,adicional_sobrenf, sec_cat, despacho, outros, data_cadastro, situacao, deslocamento,aluguel_carreta,  cadastro_usuario, emitepedagio, despesa_adm, cobra_hora_parada, valor_hora_parada, franquia_hora_parada, resolucao_vigente, endereco_resolucao, valor_icms, valor_iss, valor_pis, valor_cofins, valor_irpj, valor_csll, valor_ibs, valor_cbs, valor_sestsenat, valor_inss, cnpj_remetente, cnpj_expedidor, cnpj_destinatario, cnpj_recebedor, cnpj_consignatario, cnpj_pagador, gris, coleta, entrega, tde, tda, total_frete
                    )
                    VALUES
                    (
                        @cod_frete, @desc_frete, @rota, @desc_rota, @cod_remetente, @remetente, @cid_remetente, @uf_remetente, @cod_expedidor, @expedidor, @cid_expedidor, @uf_expedidor,  @cod_destinatario, @destinatario, @cid_destinatario, @uf_destinatario, @cod_recebedor, @recebedor, @cid_recebedor, @uf_recebedor, @cod_consignatario, @consignatario, @cid_consignatario, @uf_consignatario, @cod_pagador, @pagador, @cid_pagador, @uf_pagador,  @distancia, @Tempo, @adicional_sobrenf, @sec_cat, @despacho, @outros, @data_cadastro, @situacao, @deslocamento, @aluguel_carreta, @cadastro_usuario, @emitepedagio, @despesa_adm, @cobra_hora_parada, @valor_hora_parada, @franquia_hora_parada, @resolucao_vigente, @endereco_resolucao, @valor_icms, @valor_iss, @valor_pis, @valor_cofins, @valor_irpj, @valor_csll, @valor_ibs, @valor_cbs, @valor_sestsenat, @valor_inss, @cnpj_remetente, @cnpj_expedidor, @cnpj_destinatario, @cnpj_recebedor, @cnpj_consignatario, @cnpj_pagador, @gris, @coleta, @entrega, @tde, @tda, @total_frete                     
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
                        cmd.Parameters.Add("@Tempo", SqlDbType.Time).Value = ToTimeSpanSafe(txtDuracao.Text);
                        cmd.Parameters.Add("@adicional_sobrenf", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtAdicional.Text);
                        cmd.Parameters.Add("@sec_cat", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtSecCat.Text);
                        cmd.Parameters.Add("@despacho", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtDespacho.Text);                        
                        cmd.Parameters.Add("@outros", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtOutros.Text);
                        cmd.Parameters.Add("@data_cadastro", SqlDbType.NVarChar).Value = txtCadastro.Text;
                        cmd.Parameters.Add("@situacao", SqlDbType.NVarChar).Value = txtStatusRota.Text; 
                        cmd.Parameters.Add("@deslocamento", SqlDbType.NVarChar).Value = txtDeslocamento.Text;
                        cmd.Parameters.Add("@aluguel_carreta", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtPercentualAluguelCarreta.Text);
                        cmd.Parameters.Add("@cadastro_usuario", SqlDbType.NVarChar).Value = txtUsuCadastro.Text;
                        cmd.Parameters.Add("@emitepedagio", SqlDbType.NVarChar).Value = ddlEmitePedagio.SelectedItem.Text;
                        cmd.Parameters.Add("@despesa_adm", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtDespAdm.Text);                       
                        cmd.Parameters.Add("@cobra_hora_parada", SqlDbType.NVarChar).Value = ddlHoraParada.SelectedValue;
                        cmd.Parameters.Add("@valor_hora_parada", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtValorFranquia.Text);                       
                        string txt = txtFranquia.Text;
                        cmd.Parameters.Add("@franquia_hora_parada", SqlDbType.Time).Value =
                            string.IsNullOrWhiteSpace(txt) ? (object)DBNull.Value : ToTimeSpanSafe(txt);
                        cmd.Parameters.Add("@quant_eixos", SqlDbType.Int).Value = ddlEixos.SelectedItem.Text;
                        cmd.Parameters.Add("@tabela_antt", SqlDbType.NChar).Value = ddlTabela.SelectedItem.Text.Trim();
                        cmd.Parameters.Add("@tipo_carga_antt", SqlDbType.NVarChar).Value = ddlTipoCargaANTT.SelectedItem.Text.Trim();
                        cmd.Parameters.Add("@resolucao_vigente", SqlDbType.NVarChar).Value = lnkUrl.Text.ToString().Trim();
                        cmd.Parameters.Add("@endereco_resolucao", SqlDbType.NVarChar).Value = lnkUrl.NavigateUrl;
                        cmd.Parameters.Add("@valor_icms", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtICMS.Text);
                        cmd.Parameters.Add("@valor_iss", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtISS.Text);
                        cmd.Parameters.Add("@valor_pis", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtPIS.Text);
                        cmd.Parameters.Add("@valor_cofins", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtCOFINS.Text);
                        cmd.Parameters.Add("@valor_irpj", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtIRPJ.Text);
                        cmd.Parameters.Add("@valor_csll", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtCSLL.Text);
                        cmd.Parameters.Add("@valor_ibs", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtIBS.Text);
                        cmd.Parameters.Add("@valor_cbs", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtCBS.Text);
                        cmd.Parameters.Add("@valor_sestsenat", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtSestSenat.Text);
                        cmd.Parameters.Add("@valor_inss", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtINSS.Text);
                        cmd.Parameters.Add("@cnpj_remetente", SqlDbType.NVarChar).Value = txtCNPJRemetente.Text.Trim();
                        cmd.Parameters.Add("@cnpj_expedidor", SqlDbType.NVarChar).Value = txtCNPJExpedidor.Text.Trim();
                        cmd.Parameters.Add("@cnpj_destinatario", SqlDbType.NVarChar).Value = txtCNPJDestinatario.Text.Trim();
                        cmd.Parameters.Add("@cnpj_recebedor", SqlDbType.NVarChar).Value = txtCNPJRecebedor.Text.Trim();
                        cmd.Parameters.Add("@cnpj_consignatario", SqlDbType.NVarChar).Value = txtCNPJConsignatario.Text.Trim();
                        cmd.Parameters.Add("@cnpj_pagador", SqlDbType.NVarChar).Value = txtCNPJPagador.Text.Trim();
                        cmd.Parameters.Add("@gris", SqlDbType.Decimal).Value =
                            LimparMascaraMoeda(txtGRIS.Text);
                        cmd.Parameters.Add("@coleta", SqlDbType.Decimal).Value =
                            LimparMascaraMoeda(txtColeta.Text);
                        cmd.Parameters.Add("@entrega", SqlDbType.Decimal).Value =
                            LimparMascaraMoeda(txtEntrega.Text);
                        cmd.Parameters.Add("@tde", SqlDbType.Decimal).Value =
                            LimparMascaraMoeda(txtTDE.Text);
                        cmd.Parameters.Add("@tda", SqlDbType.Decimal).Value =
                            LimparMascaraMoeda(txtTDA.Text);
                        cmd.Parameters.Add("@total_frete", SqlDbType.Decimal).Value =
                            LimparMascaraMoeda(txtTotalFrete.Text);
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

        protected void ddlTabela_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 🔎 Verifica distância
            if (string.IsNullOrWhiteSpace(txtDistancia.Text))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg",
                    "alert('Informe a distância primeiro.');", true);
                ddlTabela.SelectedIndex = 0;
                return;
            }

            // 🔎 Verifica eixos
            if (ddlEixos.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg",
                    "alert('Selecione a quantidade de eixos.');", true);
                ddlTabela.SelectedIndex = 0;
                return;
            }

            // ✅ Converter valores
            decimal distancia;
            if (!decimal.TryParse(txtDistancia.Text, out distancia))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg",
                    "alert('Distância inválida.');", true);
                return;
            }

            int eixos = Convert.ToInt32(ddlEixos.SelectedValue);
            decimal valorKm = 0;
            decimal ccd = 0;

            string coluna = "_" + eixos + "Eixos";
            string cargaDescarga = "Valor" + eixos + "Eixos";

            using (SqlConnection conn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                string sql = $@"
            SELECT {coluna}, {cargaDescarga}, resolucao, link
            FROM tbresolucoesantt
            WHERE RTRIM(vigente) = 'SIM'
            AND Tabela = @tabela
            AND TipoCarga = @tipoCarga";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@tabela", ddlTabela.SelectedValue);
                cmd.Parameters.AddWithValue("@tipoCarga", ddlTipoCargaANTT.SelectedItem.Text.Trim());

                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    if (dr[coluna] != DBNull.Value)
                        valorKm = Convert.ToDecimal(dr[coluna]);

                    if (dr[cargaDescarga] != DBNull.Value)
                        ccd = Convert.ToDecimal(dr[cargaDescarga]);

                    if (dr["resolucao"] != DBNull.Value)
                        lnkUrl.Text = dr["resolucao"].ToString();
                    lnkUrl.NavigateUrl = dr["link"].ToString().Trim();
                }
            }

            // ✅ AGORA SIM a variável existe
            decimal frete = (distancia * valorKm) + ccd;

            txtFreteMinimo.Text = frete.ToString("N2");
            if (rbCentro.Checked = true)
            {
                txtDistancia.Text = txtDistanciaCentro.Text.Trim();
            }
            if (rbCEP.Checked = true)
            {
                txtDistancia.Text = txtDistanciaCEP.Text.Trim();
                
            }
        }
        protected void btnLancarTabela_Click(object sender, EventArgs e)
        {
            if (ddlFrete.SelectedIndex == 0)
            {
                MostrarMsg("Escolha o Frete: Frota/Agregado/Terceiro.", "warning");
                ddlFrete.Focus();
                return;
            }               

            if (ddlTipoFrete.SelectedIndex == 0)
            {
                MostrarMsg("Escolha o Tipo de frete por Tonelada/Quilo ou FTL.", "warning");
                ddlTipoFrete.Focus();
                return;
            }
               
            if (cboTipoViagem.SelectedIndex == 0)
            {
                MostrarMsg("Escolha o tipo de viagem.", "warning");
                cboTipoViagem.Focus();
                return;
            }
               
            if (cboTipoVeiculo.SelectedIndex == 0)
            {
                MostrarMsg("Escolha o tipo de veículo.", "warning");
                cboTipoVeiculo.Focus();
                return;
            }
                
            if (ddlEixos.SelectedIndex == 0)
            {
                MostrarMsg("Escolha a quantidade de eixos.", "warning");
                ddlEixos.Focus();
                return;
            }
               
            if (ddlTabela.SelectedIndex == 0)
            {
                MostrarMsg("Escolha a tabela ANTT.", "warning");
                ddlTabela.Focus();
                return;
            }
                
            if (ddlTipoCargaANTT.SelectedIndex == 0)
            {
                MostrarMsg("Escolha o tipo de carga ANTT.", "warning");
                ddlTipoCargaANTT.Focus();
                return;
            }             
            
            if (cboTipoMaterial.SelectedIndex == 0)
            {                
                MostrarMsg("Escolha o tipo de material.", "warning");
                cboTipoMaterial.Focus();
                return;
            }               
            
            
            // CONVERSÕES SEGURAS            
            DateTime vigenciaInicial;
            DateTime vigenciaFinal;

            CultureInfo cultura = new CultureInfo("pt-BR");
            decimal freteANTT;
            decimal freteReceber;
            decimal fretePagar;
            decimal freteMargem;
            
            if (string.IsNullOrWhiteSpace(txtVigenciaInicial.Text) ||
                string.IsNullOrWhiteSpace(txtVigenciaFinal.Text))
            {
                MostrarMsg("As datas de vigência não podem estar vazias. Verifique.", "danger");                
                return;
            }

            // tenta converter inicial
            if (!DateTime.TryParse(txtVigenciaInicial.Text, new CultureInfo("pt-BR"), DateTimeStyles.None, out vigenciaInicial))
            {                
                MostrarMsg("Data inicial vazia ou inválida.","danger");                
                txtVigenciaInicial.Focus();
                return;
            }

            //tenta converter final
            if (!DateTime.TryParse(txtVigenciaFinal.Text, new CultureInfo("pt-BR"), DateTimeStyles.None, out vigenciaFinal))
            {
                MostrarMsg("Data final vazoa ou inválida.", "danger");
                txtVigenciaFinal.Focus();
                return;
            }

            // valida regra de negócio
            if (vigenciaInicial > vigenciaFinal)
            {
                MostrarMsg("A vigência inicial não pode ser maior que a vigência final.", "warning");
                txtVigenciaFinal.Focus();
                return;
            }

            // converte inicial
            if (!DateTime.TryParse(txtVigenciaInicial.Text, new CultureInfo("pt-BR"), DateTimeStyles.None, out vigenciaInicial))
            {                
                MostrarMsg("Data inicial inválida.", "danger");
                txtVigenciaInicial.Focus();
                return;
            }

            // converte final
            if (!DateTime.TryParse(txtVigenciaFinal.Text, new CultureInfo("pt-BR"), DateTimeStyles.None, out vigenciaFinal))
            {                
                MostrarMsg("Data final inválida.", "danger");
                txtVigenciaFinal.Focus();
                return;
            }

            // regra de negócio
            if (vigenciaFinal < vigenciaInicial)
            {               
                MostrarMsg("A data final não pode ser menor que a data inicial.", "warning");
                txtVigenciaFinal.Focus();
                return;
            }
            // valida vazio
            if (string.IsNullOrWhiteSpace(txtFreteMinimo.Text) ||
                string.IsNullOrWhiteSpace(txtFreteReceber.Text) ||
                string.IsNullOrWhiteSpace(txtFretePagar.Text) ||
                string.IsNullOrWhiteSpace(txtMargem.Text))
            {
                MostrarMsg("Todos os campos de frete devem ser preenchidos.", "warning");
                return;
            }

            // conversão segura
            if (!decimal.TryParse(txtFreteMinimo.Text, NumberStyles.Any, cultura, out freteANTT))
            {
                MostrarMsg("Frete ANTT inválido.", "danger");
                txtFreteMinimo.Focus();
                return;
            }               
                

            if (!decimal.TryParse(txtFreteReceber.Text, NumberStyles.Any, cultura, out freteReceber))
            {
                MostrarMsg("Frete a receber inválido.", "danger");
                txtFreteReceber.Focus();
                return;
            }
                
                
            if (!decimal.TryParse(txtFretePagar.Text, NumberStyles.Any, cultura, out fretePagar))
            {
                MostrarMsg("Frete a pagar inválido.", "danger");
                txtFretePagar.Focus();
                return;
            }               
               
            if (!decimal.TryParse(txtMargem.Text, NumberStyles.Any, cultura, out freteMargem))
            {
                MostrarMsg("Margem inválida.", "danger");
                txtMargem.Focus();
                return;
            }                 
            

            string usuario = Session["UsuarioLogado"] != null
                ? Session["UsuarioLogado"].ToString()
                : "SISTEMA";

            int idRota;
            if (!int.TryParse(txtRota.Text, out idRota))
            {
                MostrarMsg("ID da rota inválido!", "danger");
                return;
            }

            int idTabela;
            if (!int.TryParse(novaTabelaDeFrete.Text, out idTabela))
            {
                MostrarMsg("ID da tabela inválido!", "danger");
                return;
            }

            int pesoLotacao;
            if (!int.TryParse(txtPesoLotacao.Text, out pesoLotacao))
            {
                MostrarMsg("Verifique o peso digistado!", "warning");
                txtPesoLotacao.Focus();
                return;
            }

            // Frete Receber não pode ser menor que o mínimo ANTT
            if (freteReceber < freteANTT)
            {
                MostrarMsg("O frete a Receber não pode ser menor que o Frete Mínimo ANTT!", "warning");
                txtFreteReceber.Focus();
                return;
            }

            // Frete Pagar não pode ser menor que o mínimo ANTT
            if (fretePagar < freteANTT && ddlFrete.SelectedValue == "Terceiro")
            {                
                MostrarMsg("O Frete a Pagar não pode ser menor que o Frete Minimo!", "warning");
                txtFretePagar.Focus();
                return;
            }

            // Frete Pagar não pode ser maior que o Frete a Receber
            if (fretePagar > freteReceber)
            {
                MostrarMsg("Frete a Pagar não pode ser maior que o Frete a Receber!", "warning");
                txtFretePagar.Focus();
                return;
            }

            // 🔹 Monta o campo descr_frete
            string[] pagador = cboPagador.SelectedItem.Text.Split(' ');
            string[] remetente = cboRemetente.Text.Split(' ');
            string[] expedidor = cboExpedidor.Text.Split(' ');
            string[] destinatario = cboDestinatario.Text.Split(' ');
            string[] recebedor = cboRecebedor.Text.Split(' ');

            string descr_frete = $"{txtCodPagador.Text} - {pagador[0]} - Inicio Prestação: {txtCidExpedidor.Text}/{txtUFExpedidor.Text} - Term. Prestação: {txtCidRecebedor.Text}/{txtUFRecebedor.Text}";
            string sqlVerifica = @"
            SELECT TOP 1 cod_frete
            FROM tbtabeladefretes
            WHERE desc_frete = @desc_frete";
            
            using (SqlCommand cmd = new SqlCommand(sqlVerifica, conn))
            {                
                conn.Close();
                // Inserir novo registro
                using (SqlConnection conn = new SqlConnection(
           WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
                {
                    conn.Open();
                    string sql = @"
                IF EXISTS (
                    SELECT 1
                    FROM tbfretes
                    WHERE cod_frete = @cod_frete
                      AND frete = @frete
                      AND tipo_veiculo = @tipo_veiculo
                      AND fl_exclusao IS NULL
                )
                BEGIN
                UPDATE tbfretes
                SET
                    medida = @medida,
                    tipo_viagem = @tipo_viagem,
                    eixos = @eixos,
                    tabela_antt = @tabela_antt,
                    tipo_carga = @tipo_carga,
                    material = @material,
                    detalhe_material = @detalhe_material,                   
                    lotacao_peso = @lotacao_peso,
                    vigencia_inicial = @vigencia_inicial,
                    vigencia_final = @vigencia_final,
                    frete_antt = @frete_antt,
                    frete_receber = @frete_receber,
                    frete_pagar = @frete_pagar,
                    margem = @margem,
                    mensagem = @mensagem,
                    status = @status,
                    gris=@gris,
                    coleta=@coleta,
                    entrega=@entrega,
                    tde=@tde,
                    tda=@tda,
                    total_frete=@totalfrete,
                    despesa_adm=@despesa_adm,
                    sec_cat=@sec_cat,
                    despacho=@despacho,
                    outros=@outros,
                    responsavel = @responsavel,
                    data_alteracao = GETDATE()
                WHERE cod_frete = @cod_frete
                  AND frete = @frete
                  AND tipo_veiculo = @tipo_veiculo
                  AND fl_exclusao IS NULL
                END
                ELSE
                BEGIN
                    INSERT INTO tbfretes
                    (
                        cod_frete,
                        id_rota,
                        frete,
                        medida,
                        tipo_viagem,
                        tipo_veiculo,
                        eixos,
                        tabela_antt,
                        tipo_carga,
                        material,
                        detalhe_material,                        
                        lotacao_peso,
                        vigencia_inicial,
                        vigencia_final,
                        frete_antt,
                        frete_receber,
                        frete_pagar,
                        margem,
                        mensagem,
                        status,
                        gris,
                        coleta,
                        entrega,
                        tde,
                        tda,
                        total_frete,
                        responsavel,
                        despesa_adm,
                        sec_cat,
                        despacho,
                        outros,
                        data_alteracao
                    )
                    VALUES
                    (
                        @cod_frete,
                        @id_rota,
                        @frete,
                        @medida,
                        @tipo_viagem,
                        @tipo_veiculo,
                        @eixos,
                        @tabela_antt,
                        @tipo_carga,
                        @material,
                        @detalhe_material,                       
                        @lotacao_peso,
                        @vigencia_inicial,
                        @vigencia_final,
                        @frete_antt,
                        @frete_receber,
                        @frete_pagar,
                        @margem,
                        @mensagem,
                        @status,
                        @gris,
                        @coleta,
                        @entrega,
                        @tde,
                        @tda,
                        @totalfrete,
                        @responsavel,
                        @despesa_adm,
                        @sec_cat,
                        @despacho,
                        @outros,
                        GETDATE()
                    )
                END";

                    SqlCommand cmd2 = new SqlCommand(sql, conn);

                    // PARÂMETROS
                    cmd2.Parameters.AddWithValue("@cod_frete", idTabela);
                    cmd2.Parameters.AddWithValue("@id_rota", idRota);
                    cmd2.Parameters.AddWithValue("@frete", ddlFrete.SelectedValue);
                    cmd2.Parameters.AddWithValue("@medida", ddlTipoFrete.SelectedValue);
                    cmd2.Parameters.AddWithValue("@tipo_viagem", cboTipoViagem.SelectedItem.Text.Trim());
                    cmd2.Parameters.AddWithValue("@tipo_veiculo", cboTipoVeiculo.SelectedItem.Text.Trim());
                    cmd2.Parameters.AddWithValue("@eixos", ddlEixos.SelectedValue);
                    cmd2.Parameters.AddWithValue("@tabela_antt", ddlTabela.SelectedItem.Text.Trim());
                    cmd2.Parameters.AddWithValue("@tipo_carga", ddlTipoCargaANTT.SelectedItem.Text.Trim());
                    cmd2.Parameters.AddWithValue("@material", cboTipoMaterial.SelectedItem.Text.Trim());
                    cmd2.Parameters.AddWithValue("@detalhe_material", txtDetalheMaterial.Text.Trim().ToUpper()); 
                    cmd2.Parameters.AddWithValue("@lotacao_peso", pesoLotacao);

                    cmd2.Parameters.AddWithValue("@vigencia_inicial", vigenciaInicial);
                    cmd2.Parameters.AddWithValue("@vigencia_final", vigenciaFinal);

                    cmd2.Parameters.Add("@frete_antt", SqlDbType.Decimal).Value =
                        LimparMascaraMoeda(txtFreteMinimo.Text);

                    cmd2.Parameters.Add("@frete_receber", SqlDbType.Decimal).Value =
                        LimparMascaraMoeda(txtFreteReceber.Text);

                    cmd2.Parameters.Add("@frete_pagar", SqlDbType.Decimal).Value =
                        LimparMascaraMoeda(txtFretePagar.Text);

                    cmd2.Parameters.Add("@margem", SqlDbType.Decimal).Value =
                        LimparMascaraMoeda(txtMargem.Text);

                    cmd2.Parameters.Add("@gris", SqlDbType.Decimal).Value =
                        LimparMascaraMoeda(txtGRIS.Text);
                    cmd2.Parameters.Add("@coleta", SqlDbType.Decimal).Value =
                        LimparMascaraMoeda(txtColeta.Text);
                    cmd2.Parameters.Add("@entrega", SqlDbType.Decimal).Value =
                        LimparMascaraMoeda(txtEntrega.Text);
                    cmd2.Parameters.Add("@tde", SqlDbType.Decimal).Value =
                        LimparMascaraMoeda(txtTDE.Text);
                    cmd2.Parameters.Add("@tda", SqlDbType.Decimal).Value =
                        LimparMascaraMoeda(txtTDA.Text);
                    cmd2.Parameters.Add("@totalfrete", SqlDbType.Decimal).Value =
                        LimparMascaraMoeda(txtTotalFrete.Text);
                    cmd2.Parameters.Add("@despesa_adm", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtDespAdm.Text);
                    cmd2.Parameters.Add("@sec_cat", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtSecCat.Text);
                    cmd2.Parameters.Add("@despacho", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtDespacho.Text);
                    cmd2.Parameters.Add("@outros", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtOutros.Text);
                    cmd2.Parameters.AddWithValue("@status", "ATIVO");
                    cmd2.Parameters.AddWithValue("@mensagem", txtObservacao.Text.Trim().ToUpper());
                    cmd2.Parameters.AddWithValue("@responsavel", usuario);

                    cmd2.ExecuteNonQuery();

                }
                CarregarFretes(idTabela);
                MostrarMsg("Frete atualizado com sucesso!", "success");
                return;
            }
        }
        private void CarregarFretes(int idTabela)
        {            
            using (SqlConnection conn = new SqlConnection(
                WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                string sql = @"
                SELECT 
                    id_frete,
                    cod_frete,
                    frete,
                    medida,
                    tipo_viagem,
                    tipo_veiculo,
                    material,
                    detalhe_material,
                    tabela_antt,
                    frete_antt,
                    frete_receber,
                    frete_pagar,
                    gris,
                    coleta, 
                    entrega,
                    tde,
                    tda,
                    total_frete,
                    margem,         
                    vigencia_inicial,
                    vigencia_final,
                    status
                FROM tbfretes
                WHERE cod_frete = @cod_frete
                  AND fl_exclusao IS NULL
                ORDER BY id_frete ASC";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@cod_frete", idTabela);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvFretes.DataSource = dt;
                gvFretes.DataBind();
            }
        }
        protected void gvFretes_RowCommand(object sender, GridViewCommandEventArgs e)
        {            
            if (e.CommandName == "Status")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                int id_frete = Convert.ToInt32(gvFretes.DataKeys[index].Value);

                using (SqlConnection conn = new SqlConnection(
                    WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
                {
                    conn.Open();

                    string sql = @"
                    UPDATE tbfretes
                    SET status = CASE 
                                    WHEN status = 'ATIVO' THEN 'INATIVO'
                                    ELSE 'ATIVO'
                                 END
                    WHERE id_frete = @id_frete";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id_frete", id_frete);
                    cmd.ExecuteNonQuery();
                }                
                ViewState["idTabela"] = Convert.ToInt32(novaTabelaDeFrete.Text);
                int idTabela = Convert.ToInt32(ViewState["idTabela"]);
                CarregarFretes(idTabela);                
            }
            if (e.CommandName == "Editar")
            {
                CarregarRegistro(Convert.ToInt32(e.CommandArgument));
            }
        }
        private void CarregarRegistro(int id)
        {
            using (SqlConnection conn = new SqlConnection(
                WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                string sql = "SELECT * FROM tbfretes WHERE id_frete=@id";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);

                conn.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    ddlFrete.SelectedValue = dr["frete"].ToString();
                    ddlTipoFrete.SelectedValue = dr["medida"].ToString();
                    cboTipoViagem.SelectedItem.Text = dr["tipo_viagem"].ToString();
                    cboTipoVeiculo.SelectedItem.Text = dr["tipo_veiculo"].ToString();
                    cboTipoMaterial.SelectedItem.Text = dr["material"].ToString();
                    txtDetalheMaterial.Text = dr["detalhe_material"].ToString();
                    ddlTabela.SelectedItem.Text = dr["tabela_antt"].ToString();
                    txtPesoLotacao.Text = dr["lotacao_peso"].ToString();
                    ddlEixos.SelectedItem.Text = dr["eixos"].ToString();
                    ddlTipoCargaANTT.SelectedItem.Text = dr["tipo_carga"].ToString();

                    txtFreteMinimo.Text = Convert.ToDecimal(dr["frete_antt"]).ToString("N2", new CultureInfo("pt-BR"));
                    txtFreteReceber.Text = Convert.ToDecimal(dr["frete_receber"]).ToString("N2", new CultureInfo("pt-BR"));
                    txtFretePagar.Text = Convert.ToDecimal(dr["frete_pagar"]).ToString("N2", new CultureInfo("pt-BR"));
                    txtMargem.Text = Convert.ToDecimal(dr["margem"]).ToString("N2", new CultureInfo("pt-BR"));
                    txtVigenciaInicial.Text = Convert.ToDateTime(dr["vigencia_inicial"]).ToString("dd/MM/yyyy");
                    txtVigenciaFinal.Text = Convert.ToDateTime(dr["vigencia_final"]).ToString("dd/MM/yyyy");

                    txtGRIS.Text = Convert.ToDecimal(dr["gris"]).ToString("N2", new CultureInfo("pt-BR"));
                    txtColeta.Text = Convert.ToDecimal(dr["coleta"]).ToString("N2", new CultureInfo("pt-BR"));
                    txtEntrega.Text = Convert.ToDecimal(dr["entrega"]).ToString("N2", new CultureInfo("pt-BR"));
                    txtTDE.Text = Convert.ToDecimal(dr["tde"]).ToString("N2", new CultureInfo("pt-BR"));
                    txtTDA.Text = Convert.ToDecimal(dr["tda"]).ToString("N2", new CultureInfo("pt-BR"));
                    txtTotalFrete.Text = Convert.ToDecimal(dr["total_frete"]).ToString("N2", new CultureInfo("pt-BR"));


                    txtObservacao.Text = dr["mensagem"].ToString();

                    ViewState["id_frete"] = id;
                }
            }

        }        

    }
}