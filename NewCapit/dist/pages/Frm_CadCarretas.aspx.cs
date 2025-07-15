using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;

namespace NewCapit.dist.pages
{
    public partial class Frm_CadCarretas : System.Web.UI.Page
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
                txtDtCadastro.Text = dataHoraAtual.ToString("dd/MM/yyyy");
                PreencherComboFiliais();
                PreencherComboEstados();
                PreencherComboRastreadores();
                CarregarDDLAgregados();
                PreencherComboMarcasVeiculos();
                PreencherComboCoresVeiculos();

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
                    // Adicionar o item padrão
                    //ddlEstados.Items.Insert(0, new ListItem("Selecione...", "0"));
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

        protected void txtCodVei_TextChanged(object sender, EventArgs e)
        {
            if (txtCodVei.Text != "")
            {

                string codigoCarreta = txtCodVei.Text.Trim();
                string strConn = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    string query = "SELECT placacarreta, codcarreta, descprop FROM tbcarretas WHERE codcarreta = @Codigo";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Codigo", codigoCarreta);
                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                lblPlaca.Text = reader.GetString(0);
                                lblCodigo.Text = reader.GetString(1);
                                lblProprietario.Text = reader.GetString(2);
                                txtCodVei.Text = string.Empty;
                                // Aciona o Toast via JavaScript
                                ScriptManager.RegisterStartupScript(this, GetType(), "toastNaoEncontrado", "mostrarToastNaoEncontrado();", true);
                                txtCodVei.Focus();
                                // Opcional: exibir mensagem ao usuário                                
                            }
                            else
                            {
                                ddlTipo.Focus();
                            }
                        }
                    }

                }

            }
        }

        protected void txtPlaca_TextChanged(object sender, EventArgs e)
        {
            if (txtPlaca.Text != "")
            {

                string placaCarreta = txtPlaca.Text.Trim();
                string strConn = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    string query = "SELECT placacarreta, codcarreta, descprop FROM tbcarretas WHERE placacarreta = @Codigo";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Codigo", placaCarreta);
                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                lblPlaCarreta.Text = reader.GetString(0);
                                lblCodCarreta.Text = reader.GetString(1);
                                lblPropCarreta.Text = reader.GetString(2);
                                txtPlaca.Text = string.Empty;
                                // Aciona o Toast via JavaScript
                                ScriptManager.RegisterStartupScript(this, GetType(), "placaNaoEncontrado", "mostrarPlacaNaoEncontrado();", true);
                                txtPlaca.Focus();
                                // Opcional: exibir mensagem ao usuário                                
                            }
                            else
                            {
                                ddlEstados.Focus();
                            }
                        }
                    }

                }

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

                ddlCidades.Items.Insert(0, new ListItem("Selecione o municipio...", "0"));
            }
        }
        private void PreencherComboEstados()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT uf, SiglaUf FROM tbestadosbrasileiros";

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
                    ddlEstados.DataValueField = "uf";  // Campo que será o valor de cada item                    
                    ddlEstados.DataBind();  // Realiza o binding dos dados                   

                    // Adicionar o item padrão
                    ddlEstados.Items.Insert(0, new ListItem("Selecione...", "0"));
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
        // Função para carregar o DropDownList com dados dos agregados
        
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
                ddlAgregados.Items.Insert(0, new ListItem("Selecione...", "0"));
            }
        }
        // Evento disparado quando o item do DropDownList é alterado
        protected void ddlAgregados_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idSelecionado = int.Parse(ddlAgregados.SelectedValue);

            // Preencher os campos com base no valor selecionado
            if (idSelecionado > 0)
            {
                PreencherCamposAgregados(idSelecionado);
            }
            else
            {
                LimparCamposAgregados();
            }
        }
        // Função para preencher os campos com os dados do banco
        private void PreencherCamposAgregados(int id)
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
                    txtAntt.Text = reader["antt"].ToString();
                    if (txtCodTra.Text == "1111")
                    {
                        ddlCarreta.Enabled = true;
                        ddlTipo.SelectedItem.Text = "FROTA";
                        ddlTipo.Enabled = false;
                    }
                }
            }
        }
        // Função para limpar os campos
        private void LimparCamposAgregados()
        {
            txtCodTra.Text = string.Empty;
            txtAntt.Text = string.Empty;
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
                                lblCodigoRastreador.Text = txtCodRastreador.Text.Trim();                                
                                // Aciona o Toast via JavaScript
                                ScriptManager.RegisterStartupScript(this, GetType(), "rastreadorNaoEncontrado", "mostrarRastreadorNaoEncontrado();", true);
                                //ddlTecnologia.ClearSelection();
                               
                                //ddlTecnologia.Items.Clear();
                                txtCodRastreador.Text = string.Empty;
                                txtCodRastreador.Focus();
                                // Opcional: exibir mensagem ao usuário
                            }

                        }
                    }

                }

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
                    // Adicionar o item padrão
                    ddlTecnologia.Items.Insert(0, new ListItem("Selecione...", "0"));
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
                    // Adicionar o item padrão
                    ddlMarca.Items.Insert(0, new ListItem("Selecione...", "0"));
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
                    // Adicionar o item padrão
                    ddlCor.Items.Insert(0, new ListItem("Selecione...", "0"));
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
                                txtCodTra.Text = reader["codtra"].ToString();
                                ddlAgregados.SelectedItem.Text = reader["fantra"].ToString();
                                txtAntt.Text = reader["antt"].ToString(); 
                                if (txtCodTra.Text == "1111") 
                                {
                                    ddlCarreta.Enabled = true;
                                    ddlTipo.SelectedItem.Text = "FROTA";
                                    ddlTipo.Enabled = false;
                                }
                            }
                            else
                            {
                                lblCodigoRastreador.Text = txtCodTra.Text.Trim();
                                // Aciona o Toast via JavaScript
                                ScriptManager.RegisterStartupScript(this, GetType(), "rastreadorNaoEncontrado", "mostrarRastreadorNaoEncontrado();", true);
                                //ddlTecnologia.ClearSelection();

                                //ddlTecnologia.Items.Clear();
                                txtCodTra.Text = string.Empty;                                
                                txtCodTra.Focus();
                                // Opcional: exibir mensagem ao usuário
                            }
                        }
                    }

                }

            }
        }

        private string RemoverMascaraCNPJ(string cnpj)
        {
            // Remove os caracteres não numéricos (pontos, barras e traços)
            return System.Text.RegularExpressions.Regex.Replace(cnpj, @"[^\d]", "");
        }
        protected void txtCnpj_TextChanged(object sender, EventArgs e)
        {
            string cnpjSemMascara = RemoverMascaraCNPJ(txtCnpj.Text);
            var cnpj = Empresa.ObterCnpj(cnpjSemMascara);
            if (cnpj != null)
            {               
                txtRazCli.Text = cnpj.nome;                
                
            }
        }

        protected void ddlCarreta_TextChanged(object sender, EventArgs e)
        {
            if (ddlCarreta.SelectedItem.Text == "ALUGADA") 
            {
                divAcao.Visible = true;
            }
            else 
            {
                divAcao.Visible = false;
            }
        }
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
           
                    string strConn = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
                    using (SqlConnection conn = new SqlConnection(strConn))
                    {
                        string query = "INSERT INTO tbcarretas (codcarreta, modelo, placacarreta, tipocarreta, anocarreta, tiporeboque, codprop, descprop, nucleo, ativo_inativo, marca, renavan, cor, antt, codrastreador, tecnologia, idrastreador, comunicacao, cnpj, chassi, licenciamento, carretaalugada, proprietario, cnpj_proprietario, contrato, uf_placa_carreta, municipio_placa_carreta, data_cadastro, cadastrado_por) " +
                                       "VALUES (@codcarreta, @modelo, @placacarreta, @tipocarreta, @anocarreta, @tiporeboque, @codprop, @descprop, @nucleo, @ativo_inativo, @marca, @renavan, @cor, @antt, @codrastreador, @tecnologia, @idrastreador, @comunicacao, @cnpj, @chassi, @licenciamento, @carretaalugada, @proprietario, @cnpj_proprietario, @contrato, @uf_placa_carreta, @municipio_placa_carreta, @data_cadastro, @cadastrado_por)";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            if (txtCodTra.Text != "") 
                            {
                                cmd.Parameters.AddWithValue("@codprop", txtCodTra.Text.Trim());  
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@codprop", DBNull.Value);                               
                            }   
                            if(txtModelo.Text != "")
                            {
                                cmd.Parameters.AddWithValue("@modelo", txtModelo.Text.Trim());
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@modelo", DBNull.Value);
                            }
                            if (txtPlaca.Text != "")
                            {
                                cmd.Parameters.AddWithValue("@placacarreta", txtPlaca.Text.Trim().ToUpper());
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@placacarreta", DBNull.Value);
                            }
                            if (ddlTipo.SelectedItem.Text != "")
                            {
                                cmd.Parameters.AddWithValue("@tipocarreta", ddlTipo.SelectedItem.Text.Trim());
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@tipocarreta", DBNull.Value);
                            }
                            if (txtAno.Text != "")
                            {
                                cmd.Parameters.AddWithValue("@anocarreta", txtAno.Text.Trim());
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@anocarreta", DBNull.Value);
                            }
                            if (ddlTipo.SelectedItem.Text != "")
                            {
                                cmd.Parameters.AddWithValue("@tiporeboque", ddlTipo.SelectedItem.Text.Trim());
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@tiporeboque", DBNull.Value);
                            }
                            if (txtCodVei.Text != "")
                            {
                                cmd.Parameters.AddWithValue("@codcarreta", txtCodVei.Text.Trim());
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@codcarreta", DBNull.Value);
                            }
                            if (ddlAgregados.SelectedItem.Text != "")
                            {
                                cmd.Parameters.AddWithValue("@descprop", ddlAgregados.Text.Trim());
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@descprop", DBNull.Value);
                            }
                            if (cbFiliais.SelectedItem.Text != "")
                            {
                                cmd.Parameters.AddWithValue("@nucleo", cbFiliais.Text.Trim());
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@nucleo", DBNull.Value);
                            }
                            if (ddlCarreta.SelectedItem.Text != "")
                            {
                                cmd.Parameters.AddWithValue("@carretaalugada", ddlCarreta.SelectedItem.Text.Trim());
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@carretaalugada", DBNull.Value);
                            }                           
                            if (ddlSituacao.SelectedItem.Text != "")
                            {
                                cmd.Parameters.AddWithValue("@ativo_inativo", ddlSituacao.SelectedItem.Text.Trim());
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@ativo_inativo", DBNull.Value);
                            }
                            if (ddlMarca.SelectedItem.Text != "")
                            {
                                cmd.Parameters.AddWithValue("@marca", ddlMarca.SelectedItem.Text.Trim());
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@marca", DBNull.Value);
                            }
                            if (txtRenavam.Text != "")
                            {
                                cmd.Parameters.AddWithValue("@renavan", txtRenavam.Text.Trim());
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@renavan", DBNull.Value);
                            }
                            if (ddlCor.SelectedItem.Text != "")
                            {
                                cmd.Parameters.AddWithValue("@cor", ddlCor.SelectedItem.Text.Trim());
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@cor", DBNull.Value);
                            }
                            if (txtAntt.Text != "")
                            {
                                cmd.Parameters.AddWithValue("@antt", txtAntt.Text.Trim());
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@antt", DBNull.Value);
                            }
                            if (txtCodRastreador.Text != "")
                            {
                                cmd.Parameters.AddWithValue("@codrastreador", txtCodRastreador.Text.Trim());
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@codrastreador", DBNull.Value);
                            }
                            if (ddlTecnologia.SelectedItem.Text != "")
                            {
                                cmd.Parameters.AddWithValue("@tecnologia", ddlTecnologia.SelectedItem.Text.Trim());
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@tecnologia", DBNull.Value);
                            }
                            if (txtId.Text != "")
                            {
                                cmd.Parameters.AddWithValue("@idrastreador", txtId.Text.Trim());
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@idrastreador", DBNull.Value);
                            }
                            if (ddlComunicacao.SelectedItem.Text != "")
                            {
                                cmd.Parameters.AddWithValue("@comunicacao", ddlComunicacao.SelectedItem.Text.Trim());
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@comunicacao", DBNull.Value);
                            }
                            if (txtCnpj.Text != "")
                            {
                                cmd.Parameters.AddWithValue("@cnpj", txtCnpj.Text.Trim());
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@cnpj", DBNull.Value);
                            }
                            if (txtChassi.Text != "")
                            {
                                cmd.Parameters.AddWithValue("@chassi", txtChassi.Text.Trim());
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@chassi", DBNull.Value);
                            }
                            if (txtLicenciamento.Text != "")
                            {
                                cmd.Parameters.AddWithValue("@licenciamento", txtLicenciamento.Text.Trim());
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@licenciamento", DBNull.Value);
                            }                            
                            if (txtRazCli.Text != "")
                            {
                                cmd.Parameters.AddWithValue("@proprietario", txtRazCli.Text.Trim());
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@proprietario", DBNull.Value);
                            }
                            if (txtCnpj.Text != "")
                            {
                                cmd.Parameters.AddWithValue("@cnpj_proprietario", txtCnpj.Text.Trim());
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@cnpj_proprietario", DBNull.Value);
                            }
                            if (txtDtContrato.Text != "")
                            {
                                cmd.Parameters.AddWithValue("@contrato", txtDtContrato.Text.Trim());
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@contrato", DBNull.Value);
                            }
                            if (ddlEstados.SelectedItem.Text != "")
                            {
                                cmd.Parameters.AddWithValue("@uf_placa_carreta", ddlEstados.SelectedItem.Text.Trim());
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@uf_placa_carreta", DBNull.Value);
                            }
                            if (ddlCidades.SelectedItem.Text != "")
                            {
                                cmd.Parameters.AddWithValue("@municipio_placa_carreta", ddlCidades.SelectedItem.Text.Trim());
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@municipio_placa_carreta", DBNull.Value);
                            }
                            // Adicionar os parâmetros necessários
                            cmd.Parameters.AddWithValue("@codprop", txtCodTra.Text.Trim().ToUpper());
                            cmd.Parameters.AddWithValue("@modelo", txtModelo.Text.Trim().ToUpper());
                            cmd.Parameters.AddWithValue("@placacarreta", txtPlaca.Text.Trim().ToUpper());
                            cmd.Parameters.AddWithValue("@tipocarreta", ddlTipo.SelectedItem.Text.Trim().ToUpper());
                            cmd.Parameters.AddWithValue("@anocarreta", txtAno.Text.Trim().ToUpper());
                            cmd.Parameters.AddWithValue("@tiporeboque", ddlTipo.SelectedItem.Text.Trim().ToUpper());
                            cmd.Parameters.AddWithValue("@codcarreta", txtCodVei.Text.Trim().ToUpper());
                            cmd.Parameters.AddWithValue("@descprop", ddlAgregados.SelectedItem.Text.Trim().ToUpper());
                            cmd.Parameters.AddWithValue("@nucleo", cbFiliais.SelectedItem.Text.Trim().ToUpper());
                            cmd.Parameters.AddWithValue("@carretaalugada", ddlCarreta.SelectedItem.Text.Trim().ToUpper());
                            cmd.Parameters.AddWithValue("@ativo_inativo", ddlSituacao.SelectedItem.Text.Trim().ToUpper());
                            cmd.Parameters.AddWithValue("@marca", ddlMarca.SelectedItem.Text.Trim().ToUpper());
                            cmd.Parameters.AddWithValue("@renavan", txtRenavam.Text.Trim().ToUpper());
                            cmd.Parameters.AddWithValue("@cor", ddlCor.SelectedItem.Text.Trim().ToUpper());
                            cmd.Parameters.AddWithValue("@antt", txtAntt.Text.Trim().ToUpper());
                            cmd.Parameters.AddWithValue("@codrastreador", txtCodRastreador.Text.Trim().ToUpper());
                            cmd.Parameters.AddWithValue("@tecnologia", ddlTecnologia.SelectedItem.Text.Trim().ToUpper());
                            cmd.Parameters.AddWithValue("@idrastreador", txtId.Text.Trim().ToUpper());
                            cmd.Parameters.AddWithValue("@comunicacao", ddlComunicacao.SelectedItem.Text.Trim().ToUpper());
                            cmd.Parameters.AddWithValue("@cnpj", txtCnpj.Text.Trim().ToUpper());
                            cmd.Parameters.AddWithValue("@chassi", txtChassi.Text.Trim().ToUpper());
                            cmd.Parameters.AddWithValue("@licenciamento", txtLicenciamento.Text.Trim().ToUpper());
                            cmd.Parameters.AddWithValue("@proprietario", txtRazCli.Text.Trim().ToUpper());
                            cmd.Parameters.AddWithValue("@cnpj_proprietario", txtCnpj.Text.Trim().ToUpper());
                            cmd.Parameters.AddWithValue("@contrato", txtDtContrato.Text.Trim().ToUpper());
                            cmd.Parameters.AddWithValue("@uf_placa_carreta", ddlEstados.SelectedItem.Text.Trim().ToUpper());
                            cmd.Parameters.AddWithValue("@municipio_placa_carreta", ddlCidades.SelectedItem.Text.Trim().ToUpper());
                            // Data e hora atual
                            cmd.Parameters.AddWithValue("@data_cadastro", dataHoraAtual);
                            cmd.Parameters.AddWithValue("@cadastrado_por", txtCadastradoPor.Text.Trim().ToUpper());

                            try
                            {
                                conn.Open();
                                cmd.ExecuteNonQuery();

                                // Exibe mensagem e fecha a tela
                                ScriptManager.RegisterStartupScript(this, GetType(), "toastSuccess", "mostrarToastSuccess();", true);
                                Response.Redirect("/dist/pages/ControleCarretas.aspx");

                            }
                            catch (Exception ex)
                            {
                                

                                // Exibe mensagem e fecha a tela
                                //ScriptManager.RegisterStartupScript(this, GetType(), "toastSuccess", "mostrarToastSuccess();", true);
                                //Response.Redirect("/dist/pages/ConsultaClientes.aspx");
                            }

                            finally
                            {
                                conn.Close();
                            }






                  



                        }
                    }
                    // Exibir mensagem de sucesso
                    //ScriptManager.RegisterStartupScript(this, GetType(), "toastSuccess", "mostrarToastSuccess();", true);
               
            
        }
    }
}