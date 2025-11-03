using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NewCapit
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
                    Response.Redirect("Login.aspx");

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

        private void PreencherComboMarcasVeiculos()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT id, marca FROM tbmarcascarretas";

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
                    ddlEstados.Items.Insert(0, new ListItem("--", "0"));
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
                                if (ddlTipo.SelectedItem.Text == "FROTA" && txtCodTra.Text != "1111")
                                {
                                    // Aciona o Toast via JavaScript
                                    ExibirToastErro("Tipo de carreta, não permite outro proprietário, que não seja 1111");
                                    Thread.Sleep(5000);
                                    ddlAgregados.ClearSelection();
                                   // txtAntt.Text = string.Empty;
                                    txtCodTra.Text = "1111";
                                    txtCodTra.Focus();
                                }
                                else if (ddlTipo.SelectedItem.Text != "FROTA" && txtCodTra.Text == "1111")
                                {
                                    // Aciona o Toast via JavaScript
                                    ExibirToastErro("Proprietário inválido, escolha outro que não seja 1111");
                                    Thread.Sleep(5000);
                                    ddlAgregados.ClearSelection();
                                    // txtAntt.Text = string.Empty;
                                    txtCodTra.Text = "1111";
                                    txtCodTra.Focus();
                                }
                                else
                                {
                                    ddlAgregados.SelectedItem.Text = reader["fantra"].ToString();
                                    txtAntt.Text = reader["antt"].ToString();
                                }
                            }
                            else
                            {                               
                                // Aciona o Toast via JavaScript
                                ExibirToastErro("Código transportadora: " + txtCodTra.Text.Trim() + ", não cadastrada no sistema.");
                                Thread.Sleep(5000);
                                ddlAgregados.ClearSelection();
                                txtAntt.Text = string.Empty;
                                txtCodTra.Text = string.Empty;
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
                    if (ddlTipo.SelectedItem.Text == "FROTA" && txtCodTra.Text != "1111")
                    {
                        // Aciona o Toast via JavaScript
                        ExibirToastErro("Tipo de carreta, não permite outro proprietário, que não seja 1111");
                        Thread.Sleep(5000);
                        ddlAgregados.ClearSelection();
                        // txtAntt.Text = string.Empty;
                        txtCodTra.Text = "1111";
                        txtCodTra.Focus();
                    }
                    else if (ddlTipo.SelectedItem.Text != "FROTA" && txtCodTra.Text == "1111")
                    {
                        // Aciona o Toast via JavaScript
                        ExibirToastErro("Proprietário inválido, escolha outro que não seja 1111");
                        Thread.Sleep(5000);
                        ddlAgregados.ClearSelection();
                        // txtAntt.Text = string.Empty;
                        txtCodTra.Text = "1111";
                        txtCodTra.Focus();
                    }
                    else
                    {
                        txtCodTra.Text = reader["codtra"].ToString();
                        ddlAgregados.SelectedItem.Text = reader["fantra"].ToString();
                        txtAntt.Text = reader["antt"].ToString();
                    }
                }
                else
                {
                    // Aciona o Toast via JavaScript
                    ExibirToastErro("Código transportadora: " + txtCodTra.Text.Trim() + ", não cadastrada no sistema.");
                    Thread.Sleep(5000);
                    ddlAgregados.ClearSelection();
                    txtAntt.Text = string.Empty;
                    txtCodTra.Text = string.Empty;
                    txtCodTra.Focus();
                    // Opcional: exibir mensagem ao usuário
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
                    ExibirToastErro("Placa carreta: " + txtPlaca.Text.Trim() + ", já cadastrado no sistema.");
                    Thread.Sleep(5000);
                    txtPlaca.Text = "";
                    txtPlaca.Focus();
                    return;
                   

                }
            }


            txtPlaca.Text.ToUpper();
            ddlEstados.Focus();
        }
        protected void btnSalvarCarreta_Click(object sender, EventArgs e)
        {
            string sqlSalvarCarreta = "insert into tbcarretas (codcarreta, modelo, placacarreta, tipocarreta, anocarreta, tiporeboque, codprop, descprop, nucleo, ativo_inativo, marca, renavan, cor, antt, codrastreador, tecnologia, idrastreador, comunicacao, chassi, licenciamento, kilometragem, carretaalugada, alugada_de,cnpj_de, inicio_contrato, termino_contrato, uf_placa_carreta, municipio_placa_carreta, data_cadastro, cadastrado_por, dt_cadastro, patrimonio, tara, comprimento, largura, altura, aquisicao) values (@codcarreta, @modelo, @placacarreta, @tipocarreta, @anocarreta, @tiporeboque, @codprop, @descprop, @nucleo, @ativo_inativo, @marca, @renavan, @cor, @antt, @codrastreador, @tecnologia, @idrastreador, @comunicacao, @chassi, @licenciamento, @kilometragem, @carretaalugada, @alugada_de, @cnpj_de, @inicio_contrato, @termino_contrato,@uf_placa_carreta, @municipio_placa_carreta, @data_cadastro, @cadastrado_por, @dt_cadastro, @patrimonio, @tara, @comprimento, @largura, @altura, @aquisicao)";
           
            SqlCommand comando = new SqlCommand(sqlSalvarCarreta, con);
            comando.Parameters.AddWithValue("@codcarreta", txtCodVei.Text.ToUpper());
            comando.Parameters.AddWithValue("@modelo", txtModelo.Text.ToUpper());
            comando.Parameters.AddWithValue("@placacarreta", txtPlaca.Text.ToUpper());
            comando.Parameters.AddWithValue("@tipocarreta", ddlCarroceria.SelectedItem.Text.ToUpper());
            comando.Parameters.AddWithValue("@anocarreta", txtAno.Text );
            comando.Parameters.AddWithValue("@tiporeboque", ddlTipo.SelectedItem.Text.ToUpper());
            comando.Parameters.AddWithValue("@codprop",txtCodTra.Text.Trim().ToUpper());
            comando.Parameters.AddWithValue("@descprop", ddlAgregados.SelectedItem.Text.ToUpper());
            comando.Parameters.AddWithValue("@nucleo", cbFiliais.SelectedItem.Text.ToUpper());
            comando.Parameters.AddWithValue("@ativo_inativo", "ATIVO");
            comando.Parameters.AddWithValue("@marca", ddlMarca.SelectedItem.Text.ToUpper());
            comando.Parameters.AddWithValue("@renavan", txtRenavam.Text.ToUpper());
            comando.Parameters.AddWithValue("@cor", ddlCor.SelectedItem.Text.ToUpper());
            comando.Parameters.AddWithValue("@antt", txtAntt.Text.ToUpper());
            comando.Parameters.AddWithValue("@codrastreador", txtCodRastreador.Text.ToUpper());
            comando.Parameters.AddWithValue("@tecnologia", ddlTecnologia.SelectedItem.Text.ToUpper());
            comando.Parameters.AddWithValue("@idrastreador", txtId.Text);
            comando.Parameters.AddWithValue("@comunicacao", ddlComunicacao.SelectedItem.Text.ToUpper());
            comando.Parameters.AddWithValue("@chassi", txtChassi.Text.ToUpper());
            comando.Parameters.AddWithValue("@licenciamento", txtLicenciamento.Text.ToUpper());
            comando.Parameters.AddWithValue("@kilometragem", txtOdometro.Text);
            comando.Parameters.AddWithValue("@carretaalugada", ddlCarreta.SelectedItem.Text.ToUpper());
            comando.Parameters.AddWithValue("@alugada_de", txtAlugada_De.Text.ToUpper());
            comando.Parameters.AddWithValue("@cnpj_de", txtCNPJ.Text);
            comando.Parameters.AddWithValue("@inicio_contrato", txtInicioContrato.Text.ToUpper());
            comando.Parameters.AddWithValue("@termino_contrato", txtTerminoContrato.Text);
            comando.Parameters.AddWithValue("@uf_placa_carreta", ddlEstados.SelectedItem.Text.ToUpper());
            comando.Parameters.AddWithValue("@municipio_placa_carreta", ddlCidades.SelectedItem.Text.ToUpper());
            comando.Parameters.AddWithValue("@data_cadastro", dataHoraAtual.ToString("dd/MM/yyyy HH:mm"));
            comando.Parameters.AddWithValue("@cadastrado_por", txtCadastradoPor.Text.Trim().ToUpper());
            comando.Parameters.AddWithValue("@dt_cadastro", dataHoraAtual.ToString("dd/MM/yyyy"));            
            comando.Parameters.AddWithValue("@patrimonio", txtControlePatrimonio.Text.ToUpper());
            comando.Parameters.AddWithValue("@tara", txtTara.Text.ToUpper());
            if (txtComprimento.Text != "")
            {
                string entradaComprimento = txtComprimento.Text.Trim();
                // Substitui vírgula por ponto
                string formatado = entradaComprimento.Replace(',', '.');

                if (decimal.TryParse(formatado, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal numero))
                {
                    comando.Parameters.AddWithValue("@comprimento", numero.ToString(CultureInfo.InvariantCulture));
                }
            }
            if (txtLargura.Text != "")
            {
                string entradaLargura = txtLargura.Text.Trim();
                // Substitui vírgula por ponto
                string formatado = entradaLargura.Replace(',', '.');
                if (decimal.TryParse(formatado, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal numero))
                {
                    comando.Parameters.AddWithValue("@largura", numero.ToString(CultureInfo.InvariantCulture));
                }
            }
            if (txtAltura.Text != "")
            {
                string entradaAltura = txtAltura.Text.Trim();
                // Substitui vírgula por ponto
                string formatado = entradaAltura.Replace(',', '.');
                if (decimal.TryParse(formatado, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal numero))
                {
                    comando.Parameters.AddWithValue("@altura", numero.ToString(CultureInfo.InvariantCulture));
                }
            }
            comando.Parameters.AddWithValue("@aquisicao", txtDataAquisicao.Text.ToUpper());
            try
            {
                con.Open();
                comando.ExecuteNonQuery();
                con.Close();
                ExibirToastCadastro("Placa da carreta: " + txtPlaca.Text.Trim() + ", cadastrada com sucesso!");
                Thread.Sleep(5000);
                //Chama a página de controle de carretas
                Response.Redirect("/dist/pages/ControleCarretas.aspx");

            }
            catch (Exception ex)
            {
                var message = new JavaScriptSerializer().Serialize(ex.Message.ToString());
                ExibirToastErro("Erro ao cadastrar a placa da carreta: " + txtPlaca.Text.Trim() + " - " + message);
                Thread.Sleep(5000);
                //Chama a página de controle de carretas
                //Response.Redirect("/dist/pages/ControleCarretas.aspx");
            }

            finally
            {
                con.Close();
            }

        }

        protected void txtCodVei_TextChanged(object sender, EventArgs e)
        {
            if (txtCodVei.Text != "")
            {
                string termo = txtCodVei.Text.ToUpper();
                string strConn = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    string query = "SELECT TOP 1 placacarreta, codcarreta FROM tbcarretas WHERE codcarreta LIKE @termo";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@termo", "%" + termo + "%");
                    conn.Open();

                    object res = cmd.ExecuteScalar();
                    if (res != null)
                    {
                        ExibirToastErro("Código carreta: " + txtCodVei.Text.Trim() + ", já cadastrado no sistema.");
                        Thread.Sleep(5000);
                        txtCodVei.Text = "";
                        txtCodVei.Focus();
                        return;
                    }
                }
                //txtCodVei.Text.ToUpper();
                //ddlTipo.Focus();

            }            
        }
        protected void ExibirToastCadastro(string mensagem)
        {
            string script = $@"
            <script>
                document.getElementById('toastMessage2').innerText = '{mensagem.Replace("'", "\\'")}';
                var toastEl = document.getElementById('myToast2');
                var toast = new bootstrap.Toast(toastEl);
                toast.show();
            </script>";

            ClientScript.RegisterStartupScript(this.GetType(), "toastScript", script, false);
        }
        protected void ExibirToastErro(string mensagem)
        {
            string script = $@"
            <script>
                document.getElementById('toastMessage3').innerText = '{mensagem.Replace("'", "\\'")}';
                var toastEl = document.getElementById('myToast3');
                var toast = new bootstrap.Toast(toastEl);
                toast.show();
            </script>";

            ClientScript.RegisterStartupScript(this.GetType(), "toastScript", script, false);
        }
        protected void ddlTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(ddlTipo.SelectedItem.Text == "FROTA")
            {
                alugada.Visible = true;
                string codigoTNG = "1111";
                string strConn = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    string queryTNG = "SELECT codtra, fantra, antt FROM tbtransportadoras WHERE codtra = @Codigo";

                    using (SqlCommand cmdTNG = new SqlCommand(queryTNG, conn))
                    {
                        cmdTNG.Parameters.AddWithValue("@Codigo", codigoTNG);
                        conn.Open();

                        using (SqlDataReader reader = cmdTNG.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtCodTra.Text = reader["codtra"].ToString().ToUpper();
                                ddlAgregados.SelectedItem.Text = reader["fantra"].ToString().ToUpper();
                                txtAntt.Text = reader["antt"].ToString().ToUpper();
                                //txtCodTra.Enabled = false;
                                //ddlAgregados.Enabled = false;
                                //txtAntt.Enabled = false;
                            }                            
                        }
                    }

                }
            }
            else
            {
                alugada.Visible = false;
            }
        }
        protected void ddlCarreta_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCarreta.SelectedItem.Text == "ALUGADA")
            {
                aluguel.Visible = true;

            }
            else
            {
                aluguel.Visible = false;
            }

        }
        private string RemoverMascaraCNPJ(string cnpj)
        {
            // Remove os caracteres não numéricos (pontos, barras e traços)
            return System.Text.RegularExpressions.Regex.Replace(cnpj, @"[^\d]", "");
        }
        protected void txtCNPJ_TextChanged(object sender, EventArgs e)
        {
            if (txtCNPJ.Text != "")
            {
                string cnpjSemMascara = RemoverMascaraCNPJ(txtCNPJ.Text);
                var cnpj = Empresa.ObterCnpj(cnpjSemMascara);
                if (cnpj != null)
                {
                    txtAlugada_De.Text = cnpj.nome;
                }
            }
        }
    }
}