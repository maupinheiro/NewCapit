using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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
            object DecimalOuDBNull(string valor)
            {
                if (string.IsNullOrWhiteSpace(valor))
                    return DBNull.Value;

                string formatado = valor.Trim().Replace(',', '.');

                if (decimal.TryParse(formatado, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal numero))
                    return numero;

                return DBNull.Value;
            }

            DateTime dataHoraAtual = DateTime.Now;

            string sqlSalvarCarreta = @"
        INSERT INTO tbcarretas
        (
            codcarreta, modelo, placacarreta, tipocarreta, anocarreta, tiporeboque,
            codprop, descprop, nucleo, ativo_inativo, marca, renavan, cor, antt,
            codrastreador, tecnologia, idrastreador, comunicacao, chassi,
            licenciamento, kilometragem, carretaalugada, alugada_de, cnpj_de,
            inicio_contrato, termino_contrato, uf_placa_carreta,
            municipio_placa_carreta, data_cadastro, cadastrado_por, dt_cadastro,
            patrimonio, tara, comprimento, largura, altura, aquisicao
        )
        VALUES
        (
            @codcarreta, @modelo, @placacarreta, @tipocarreta, @anocarreta, @tiporeboque,
            @codprop, @descprop, @nucleo, @ativo_inativo, @marca, @renavan, @cor, @antt,
            @codrastreador, @tecnologia, @idrastreador, @comunicacao, @chassi,
            @licenciamento, @kilometragem, @carretaalugada, @alugada_de, @cnpj_de,
            @inicio_contrato, @termino_contrato, @uf_placa_carreta,
            @municipio_placa_carreta, @data_cadastro, @cadastrado_por, @dt_cadastro,
            @patrimonio, @tara, @comprimento, @largura, @altura, @aquisicao
        )";

            using (SqlCommand comando = new SqlCommand(sqlSalvarCarreta, con))
            {
                // 🔤 STRINGS COM TAMANHO (ANTI-TRUNCAMENTO)
                comando.Parameters.Add("@codcarreta", SqlDbType.VarChar, 20).Value = txtCodVei.Text.Trim().ToUpper();
                comando.Parameters.Add("@modelo", SqlDbType.VarChar, 50).Value = txtModelo.Text.Trim().ToUpper();
                comando.Parameters.Add("@placacarreta", SqlDbType.Char, 7).Value = txtPlaca.Text.Replace("-", "").Trim().ToUpper();
                comando.Parameters.Add("@tipocarreta", SqlDbType.VarChar, 30).Value = ddlCarroceria.SelectedItem.Text.ToUpper();
                comando.Parameters.Add("@anocarreta", SqlDbType.Char, 7).Value = txtAno.Text.Trim();
                comando.Parameters.Add("@tiporeboque", SqlDbType.VarChar, 30).Value = ddlTipo.SelectedItem.Text.ToUpper();
                comando.Parameters.Add("@codprop", SqlDbType.VarChar, 15).Value = txtCodTra.Text.Trim().ToUpper();
                comando.Parameters.Add("@descprop", SqlDbType.VarChar, 60).Value = ddlAgregados.SelectedItem.Text.ToUpper();
                comando.Parameters.Add("@nucleo", SqlDbType.VarChar, 40).Value = cbFiliais.SelectedItem.Text.ToUpper();
                comando.Parameters.Add("@ativo_inativo", SqlDbType.VarChar, 5).Value = "ATIVO";
                comando.Parameters.Add("@marca", SqlDbType.VarChar, 40).Value = ddlMarca.SelectedItem.Text.ToUpper();
                comando.Parameters.Add("@renavan", SqlDbType.VarChar, 20).Value = txtRenavam.Text.Trim().ToUpper();
                comando.Parameters.Add("@cor", SqlDbType.VarChar, 20).Value = ddlCor.SelectedItem.Text.ToUpper();
                comando.Parameters.Add("@antt", SqlDbType.VarChar, 20).Value = txtAntt.Text.Trim().ToUpper();
                comando.Parameters.Add("@codrastreador", SqlDbType.VarChar, 20).Value = txtCodRastreador.Text.Trim().ToUpper();
                comando.Parameters.Add("@tecnologia", SqlDbType.VarChar, 20).Value = ddlTecnologia.SelectedItem.Text.ToUpper();
                comando.Parameters.Add("@idrastreador", SqlDbType.VarChar, 20).Value = txtId.Text.Trim();
                comando.Parameters.Add("@comunicacao", SqlDbType.VarChar, 20).Value = ddlComunicacao.SelectedItem.Text.ToUpper();
                comando.Parameters.Add("@chassi", SqlDbType.VarChar, 30).Value = txtChassi.Text.Trim().ToUpper();
                comando.Parameters.Add("@carretaalugada", SqlDbType.VarChar, 10).Value = ddlCarreta.SelectedItem.Text.ToUpper();
                comando.Parameters.Add("@alugada_de", SqlDbType.VarChar, 60).Value = txtAlugada_De.Text.Trim().ToUpper();
                comando.Parameters.Add("@cnpj_de", SqlDbType.VarChar, 18).Value = txtCNPJ.Text.Trim();
                comando.Parameters.Add("@uf_placa_carreta", SqlDbType.Char, 2).Value = ddlEstados.SelectedItem.Text.ToUpper();
                comando.Parameters.Add("@municipio_placa_carreta", SqlDbType.VarChar, 50).Value = ddlCidades.SelectedItem.Text.ToUpper();
                comando.Parameters.Add("@cadastrado_por", SqlDbType.VarChar, 40).Value = txtCadastradoPor.Text.Trim().ToUpper();
                comando.Parameters.Add("@patrimonio", SqlDbType.VarChar, 30).Value = txtControlePatrimonio.Text.Trim().ToUpper();

                // 🔢 NUMÉRICOS
                comando.Parameters.Add("@kilometragem", SqlDbType.VarChar, 15).Value = txtOdometro.Text.Trim();
                comando.Parameters.Add("@tara", SqlDbType.VarChar, 10).Value = txtTara.Text.Trim();

                comando.Parameters.Add("@comprimento", SqlDbType.Decimal).Value = DecimalOuDBNull(txtComprimento.Text);
                comando.Parameters.Add("@largura", SqlDbType.Decimal).Value = DecimalOuDBNull(txtLargura.Text);
                comando.Parameters.Add("@altura", SqlDbType.Decimal).Value = DecimalOuDBNull(txtAltura.Text);

                // 📅 DATAS
                comando.Parameters.Add("@licenciamento", SqlDbType.VarChar, 10).Value = txtLicenciamento.Text.Trim();
                comando.Parameters.Add("@inicio_contrato", SqlDbType.VarChar, 10).Value = txtInicioContrato.Text.Trim();
                comando.Parameters.Add("@termino_contrato", SqlDbType.VarChar, 10).Value = txtTerminoContrato.Text.Trim();
                comando.Parameters.Add("@aquisicao", SqlDbType.VarChar, 10).Value = txtDataAquisicao.Text.Trim();

                comando.Parameters.Add("@data_cadastro", SqlDbType.VarChar, 16)
                    .Value = dataHoraAtual.ToString("dd/MM/yyyy HH:mm");

                comando.Parameters.Add("@dt_cadastro", SqlDbType.VarChar, 10)
                    .Value = dataHoraAtual.ToString("dd/MM/yyyy");

                con.Open();
                comando.ExecuteNonQuery();
                con.Close();
            }

            ExibirToastCadastro(
                "Placa da carreta: " + txtPlaca.Text.Trim() + ", cadastrada com sucesso!"
            );

            Thread.Sleep(5000);
            Response.Redirect("/dist/pages/ControleCarretas.aspx");
        }


        private object DecimalOuDBNull(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                return DBNull.Value;

            return decimal.Parse(
                valor.Replace(",", "."),
                CultureInfo.InvariantCulture
            );
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