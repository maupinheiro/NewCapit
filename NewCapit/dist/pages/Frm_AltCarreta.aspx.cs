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
using DocumentFormat.OpenXml.Office.Word;
using NPOI.POIFS.Crypt.Dsig;

namespace NewCapit.dist.pages
{
    public partial class Frm_AltCarreta : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString());
        string idCarreta;
        DateTime dataHoraAtual = DateTime.Now;
        DateTime dataLicenciamento;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UsuarioLogado"] != null)
                {
                    string nomeUsuario = Session["UsuarioLogado"].ToString();
                    var lblUsuario = nomeUsuario;

                    Alterado_Por.Text = nomeUsuario;

                }
                else
                {
                    var lblUsuario = "<Usuário>";
                    Alterado_Por.Text = lblUsuario;
                    Response.Redirect("Login.aspx");

                }

                CarregarDDLAgregados();
                PreencherComboRastreadores();
                PreencherComboFiliais();
                PreencherComboMarcasVeiculos();
                PreencherComboCoresVeiculos();
                PreencherComboRastreadores();
                PreencherComboEstados();

                CarregaDadosCarreta();
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
        //protected void txtPlaca_TextChanged(object sender, EventArgs e)
        //{
        //    string termo = txtPlaca.Text.ToUpper();
        //    string strConn = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;

        //    using (SqlConnection conn = new SqlConnection(strConn))
        //    {
        //        string query = "SELECT TOP 1 placacarreta, codcarreta FROM tbcarretas WHERE placacarreta LIKE @termo";
        //        SqlCommand cmd = new SqlCommand(query, conn);
        //        cmd.Parameters.AddWithValue("@termo", "%" + termo + "%");
        //        conn.Open();

        //        object res = cmd.ExecuteScalar();
        //        if (res != null)
        //        {
        //            ExibirToastErro("Placa carreta: " + txtPlaca.Text.Trim() + ", já cadastrado no sistema.");
        //            Thread.Sleep(5000);
        //            txtPlaca.Text = "";
        //            txtPlaca.Focus();
        //            return;


        //        }
        //    }


        //    txtPlaca.Text.ToUpper();
        //    ddlEstados.Focus();
        //}
        void AddDate(SqlCommand cmd, string nome, string valor)
        {
            if (DateTime.TryParseExact(valor, "dd/MM/yyyy",
                CultureInfo.GetCultureInfo("pt-BR"),
                DateTimeStyles.None, out DateTime data))
            {
                cmd.Parameters.Add(nome, SqlDbType.DateTime).Value = data;
            }
            else
            {
                cmd.Parameters.Add(nome, SqlDbType.DateTime).Value = DBNull.Value;
            }
        }

        void AddDecimal(SqlCommand cmd, string nome, string valor)
        {
            if (decimal.TryParse(valor.Replace(',', '.'),
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out decimal numero))
            {
                cmd.Parameters.Add(nome, SqlDbType.Decimal).Value = numero;
            }
            else
            {
                cmd.Parameters.Add(nome, SqlDbType.Decimal).Value = DBNull.Value;
            }
        }

        void AddString(SqlCommand cmd, string nome, string valor)
        {
            SqlParameter p = cmd.Parameters.Add(nome, SqlDbType.VarChar);

            if (string.IsNullOrWhiteSpace(valor))
                p.Value = DBNull.Value;
            else
                p.Value = valor.Trim().ToUpper();
        }


        protected void btnSalvarCarreta_Click(object sender, EventArgs e)
        {
            
            if (HttpContext.Current.Request.QueryString["id"].ToString() != "")
            {
                idCarreta = HttpContext.Current.Request.QueryString["id"].ToString();
            }
            string codigoTNG = idCarreta;
            string sql = @"UPDATE tbcarretas SET
            modelo=@modelo,
            tipocarreta=@tipocarreta,
            anocarreta=@anocarreta,
            tiporeboque=@tiporeboque,
            codprop=@codprop,
            descprop=@descprop,
            nucleo=@nucleo,
            ativo_inativo=@ativo_inativo,
            marca=@marca,
            renavan=@renavan,
            cor=@cor,
            antt=@antt,
            codrastreador=@codrastreador,
            tecnologia=@tecnologia,
            idrastreador=@idrastreador,
            comunicacao=@comunicacao,
            chassi=@chassi,
            licenciamento=@licenciamento,
            kilometragem=@kilometragem,
            carretaalugada=@carretaalugada,
            alugada_de=@alugada_de,
            cnpj_de=@cnpj_de,
            inicio_contrato=@inicio_contrato,
            termino_contrato=@termino_contrato,
            uf_placa_carreta=@uf_placa_carreta,
            municipio_placa_carreta=@municipio_placa_carreta,
            patrimonio=@patrimonio,
            alterado_por=@alterado_por,
            data_alteracao=@data_alteracao,
            tara=@tara,
            comprimento=@comprimento,
            largura=@largura,
            altura=@altura,
            aquisicao=@aquisicao,
            data_inativo=@data_inativo,
            motivo_inativacao=@motivo_inativacao
            WHERE idcarreta=@idCarreta";
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                AddString(cmd, "@modelo", txtModelo.Text);
                AddString(cmd, "@tipocarreta", ddlCarroceria.SelectedItem.Text);
                AddString(cmd, "@anocarreta", txtAno.Text);
                AddString(cmd, "@tiporeboque", ddlTipo.SelectedItem.Text);
                AddString(cmd, "@codprop", txtCodTra.Text);
                AddString(cmd, "@descprop", ddlAgregados.SelectedItem.Text);
                AddString(cmd, "@nucleo", cbFiliais.SelectedItem.Text);
                AddString(cmd, "@marca", ddlMarca.SelectedItem.Text);
                AddString(cmd, "@renavan", txtRenavam.Text);
                AddString(cmd, "@cor", ddlCor.SelectedItem.Text);
                AddString(cmd, "@antt", txtAntt.Text);
                AddString(cmd, "@codrastreador", txtCodRastreador.Text);
                AddString(cmd, "@tecnologia", ddlTecnologia.SelectedItem.Text);
                AddString(cmd, "@idrastreador", txtId.Text);
                AddString(cmd, "@comunicacao", ddlComunicacao.SelectedItem.Text);
                AddString(cmd, "@chassi", txtChassi.Text);
                AddString(cmd, "@kilometragem", txtOdometro.Text);
                AddString(cmd, "@carretaalugada", ddlCarreta.SelectedItem.Text);
                AddString(cmd, "@alugada_de", txtAlugada_De.Text);
                AddString(cmd, "@cnpj_de", txtCNPJ.Text);
                AddString(cmd, "@uf_placa_carreta", ddlEstados.SelectedItem.Text);
                AddString(cmd, "@municipio_placa_carreta", ddlCidades.SelectedItem.Text);
                AddString(cmd, "@patrimonio", txtControlePatrimonio.Text);
                AddString(cmd, "@motivo_inativacao", txtMotivo_Inativacao.Text);
                AddString(cmd, "@alterado_por", Alterado_Por.Text);
                AddString(cmd, "@ativo_inativo", ddlSituacao.SelectedItem.Text);

                AddDate(cmd, "@licenciamento", txtLicenciamento.Text);
                AddDate(cmd, "@inicio_contrato", txtInicioContrato.Text);
                AddDate(cmd, "@termino_contrato", txtTerminoContrato.Text);
                AddDate(cmd, "@aquisicao", txtDataAquisicao.Text);
                AddDate(cmd, "@data_inativo", txtData_Inativo.Text);

                AddDecimal(cmd, "@comprimento", txtComprimento.Text);
                AddDecimal(cmd, "@largura", txtLargura.Text);
                AddDecimal(cmd, "@altura", txtAltura.Text);
                AddDecimal(cmd, "@tara", txtTara.Text);

                cmd.Parameters.Add("@data_alteracao", SqlDbType.DateTime).Value = DateTime.Now;
                cmd.Parameters.Add("@idCarreta", SqlDbType.Int).Value = idCarreta;

                con.Open();
                cmd.ExecuteNonQuery();
            }

            con.Close();
            ExibirToastCadastro("Atualização realizada com sucesso!");
            Thread.Sleep(5000);
            //Chama a página de controle de carretas
            Response.Redirect("/dist/pages/ControleCarretas.aspx");

            //    }
            //    catch (Exception ex)
            //    {
            //        var message = new JavaScriptSerializer().Serialize(ex.Message.ToString());
            //ExibirToastErro("Erro ao atualizar a placa da carreta: " + txtPlaca.Text.Trim() + " - " + message);
            //Thread.Sleep(5000);
            //        //Chama a página de controle de carretas
            //        Response.Redirect("/dist/pages/ControleCarretas.aspx");
            //    }
            //    finally
            //    {
            //        con.Close();
            //    }

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
            if (ddlTipo.SelectedItem.Text == "FROTA")
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
        public void CarregaDadosCarreta()
        {
            if (HttpContext.Current.Request.QueryString["id"].ToString() != "")
            {
                idCarreta = HttpContext.Current.Request.QueryString["id"].ToString();
            }

            string codigoTNG = idCarreta;
            string strConn = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                string queryTNG = "SELECT codcarreta, modelo, placacarreta, tipocarreta, anocarreta, tiporeboque, codprop, descprop, nucleo, ativo_inativo, marca, renavan, cor, antt, codrastreador, tecnologia, idrastreador, comunicacao, chassi, CONVERT(varchar, licenciamento, 103) as licenciamento, kilometragem, carretaalugada, alugada_de,cnpj_de, CONVERT(varchar, inicio_contrato, 103) as inicio_contrato, CONVERT(varchar, termino_contrato, 103) as termino_contrato, uf_placa_carreta, municipio_placa_carreta, data_cadastro, cadastrado_por, CONVERT(varchar, dt_cadastro, 103) as dt_cadastro, patrimonio, tara, comprimento, largura, altura, CONVERT(varchar, aquisicao, 103) as aquisicao, CONVERT(varchar, data_inativo, 103) as data_inativo, motivo_inativacao, tipo_de_seguro, companhia, apolice, CONVERT(varchar, validade, 103) as validade, valor_franquia, frota, placa_cavalo, data_ultima_viagem, transportador, data_alteracao, alterado_por FROM tbcarretas WHERE idcarreta =  @IdCarreta";

                using (SqlCommand cmdTNG = new SqlCommand(queryTNG, conn))
                {
                    cmdTNG.Parameters.AddWithValue("@IdCarreta", codigoTNG);
                    conn.Open();

                    using (SqlDataReader reader = cmdTNG.ExecuteReader())
                    {
                        if (reader.Read())
                        {                            
                            txtCodVei.Text = reader["codcarreta"].ToString();
                            txtModelo.Text = reader["modelo"].ToString();
                            txtPlaca.Text = reader["placacarreta"].ToString();
                            ddlCarroceria.SelectedItem.Text = reader["tipocarreta"].ToString();
                            txtAno.Text = reader["anocarreta"].ToString();
                            ddlTipo.SelectedItem.Text = reader["tiporeboque"].ToString();
                            txtCodTra.Text = reader["codprop"].ToString();
                            ddlAgregados.SelectedItem.Text = reader["descprop"].ToString();
                            cbFiliais.SelectedItem.Text = reader["nucleo"].ToString();
                            ddlSituacao.SelectedItem.Text = reader["ativo_inativo"].ToString();
                            ddlMarca.SelectedItem.Text = reader["marca"].ToString();
                            txtRenavam.Text = reader["renavan"].ToString();
                            ddlCor.SelectedItem.Text = reader["cor"].ToString();
                            txtAntt.Text = reader["antt"].ToString();
                            txtCodRastreador.Text = reader["codrastreador"].ToString();
                            ddlTecnologia.SelectedItem.Text = reader["tecnologia"].ToString();
                            txtId.Text = reader["idrastreador"].ToString();
                            ddlComunicacao.SelectedItem.Text = reader["comunicacao"].ToString();
                            txtChassi.Text = reader["chassi"].ToString();
                            txtLicenciamento.Text = reader["licenciamento"].ToString();
                            txtOdometro.Text = reader["kilometragem"].ToString();
                            ddlCarreta.SelectedItem.Text = reader["carretaalugada"].ToString();
                            txtAlugada_De.Text = reader["alugada_de"].ToString();
                            txtCNPJ.Text = reader["cnpj_de"].ToString();
                            txtInicioContrato.Text = reader["inicio_contrato"].ToString();
                            txtTerminoContrato.Text = reader["termino_contrato"].ToString();
                            ddlEstados.SelectedItem.Text = reader["uf_placa_carreta"].ToString();
                            if (reader["municipio_placa_carreta"].ToString() != "NULL")
                            {
                                ddlCidades.Items.Clear(); // Limpa as cidades antes de carregar
                                CarregarCidades(ddlEstados.SelectedValue); // Carrega as cidades com base     no estado selecionado
                            }
                            ddlCidades.SelectedItem.Text = reader["municipio_placa_carreta"].ToString();
                            txtDataCadastro.Text = reader["data_cadastro"].ToString();
                            txtCadastradoPor.Text = reader["cadastrado_por"].ToString();
                            txtDt_Cadastro.Text = reader["dt_cadastro"].ToString();                           
                            txtControlePatrimonio.Text = reader["patrimonio"].ToString();
                            txtTara.Text = reader["tara"].ToString();
                            txtComprimento.Text = reader["comprimento"].ToString();
                            txtLargura.Text = reader["largura"].ToString();
                            txtAltura.Text = reader["altura"].ToString();
                            txtDataAquisicao.Text = reader["aquisicao"].ToString();
                            txtData_Inativo.Text = reader["data_inativo"].ToString();
                            txtMotivo_Inativacao.Text = reader["motivo_inativacao"].ToString();
                            txtTipo_De_Seguro.Text = reader["tipo_de_seguro"].ToString();
                            txtCompanhia.Text = reader["companhia"].ToString();
                            txtApolice.Text = reader["apolice"].ToString();
                            txtValidade.Text = reader["validade"].ToString();
                            txtValor_Franquia.Text = reader["valor_franquia"].ToString();
                            txtData_Ultima_Viagem.Text = reader["data_ultima_viagem"].ToString();
                            txtFrota.Text = reader["frota"].ToString();
                            txtPlaca_Cavalo.Text = reader["placa_cavalo"].ToString();
                            txtTransportador.Text = reader["transportador"].ToString();
                            txtData_Alteracao.Text = reader["data_alteracao"].ToString();
                            txtAlterado_Por.Text = reader["alterado_por"].ToString();

                            if (reader["tiporeboque"].ToString() == "FROTA" && reader["carretaalugada"].ToString() == "ALUGADA")
                            {
                                alugada.Visible = true;
                                aluguel.Visible = true;
                            }
                            else
                            {
                                alugada.Visible = false;
                                aluguel.Visible = false;
                            }
                            if (reader["ativo_inativo"].ToString() == "INATIVO")
                            {
                                colunaInativo.Visible = true;                                
                            }
                            else
                            {
                                colunaInativo.Visible = false;                               
                            }
                        }
                    }
                }

            }
        }
        protected void ddlSituacao_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlSituacao.SelectedItem.Text == "INATIVO")
            {
                colunaInativo.Visible = true;
                txtData_Inativo.Text = dataHoraAtual.ToString("dd/MM/yyyy").ToString();
                txtMotivo_Inativacao.Focus();
            }
            else
            {
                colunaInativo.Visible = false;
                txtData_Inativo.Text = "";
                txtMotivo_Inativacao.Text = "";
            }

        }
        private object SafeDateValue(string input)
        {
            DateTime dt;
            if (DateTime.TryParse(input, out dt))
                return dt.ToString("yyyy-MM-dd");
            else
                return DBNull.Value;
        }
    }
}