using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NewCapit.dist.pages
{
    public partial class Frm_CadMotoristas : System.Web.UI.Page
    {
        SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString());
        string nomeUsuario = string.Empty;
        DateTime dataHoraAtual = DateTime.Now;
        string caminhoFoto;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UsuarioLogado"] != null)
                {
                    string nomeUsuario = Session["UsuarioLogado"].ToString();
                    var lblUsuario = nomeUsuario;

                    txtUsuCadastro.Text = nomeUsuario;

                }
                else
                {
                    var lblUsuario = "<Usuário>";
                    txtUsuCadastro.Text = lblUsuario;
                    Response.Redirect("Login.aspx");

                }
                PreencherComboFiliais();
                PreencherComboCargo();
                PreencherComboJornada();
                CarregarDDLAgregados();


                CarregarEstCNH();
                CarregarRegioes();
                CarregarEstadosNascimento();
                CarregarMunicipioNasc();




            }
            DateTime dataHoraAtual = DateTime.Now;
            txtDtCad.Text = dataHoraAtual.ToString("dd/MM/yyyy");
            lblDtCadastro.Text = dataHoraAtual.ToString("dd/MM/yyyy HH:mm");

            string defaultImage = ResolveUrl("/fotos/motoristasemfoto.jpg");
            string base64 = hiddenImage.Value;

            // Validar se a imagem em base64 não é muito grande (por exemplo, maior que 1.3MB em texto base64)
            bool imagemValida = !string.IsNullOrEmpty(base64) && base64.Length < 1_300_000;

            string script = $@"
        <script type='text/javascript'>
            document.addEventListener('DOMContentLoaded', function() {{
                var img = document.getElementById('preview');
                img.src = {(imagemValida ? $"'{base64}'" : $"'{defaultImage}'")};
            }});
        </script>";

            ClientScript.RegisterStartupScript(this.GetType(), "restoreImage", script, false);





        }
        private void CarregarRegioes()
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                SqlCommand cmd = new SqlCommand("SELECT id, regiao FROM tbregioesdopais", conn);
                conn.Open();
                ddlRegioes.DataSource = cmd.ExecuteReader();
                ddlRegioes.DataTextField = "regiao";
                ddlRegioes.DataValueField = "id";
                ddlRegioes.DataBind();
                ddlRegioes.Items.Insert(0, new ListItem("Selecione", "0"));
            }
        }
        private void CarregarEstadosNascimento()
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                SqlCommand cmd = new SqlCommand("SELECT Uf, IdRegiao, SiglaUf FROM tbestadosbrasileiros WHERE IdRegiao = @RegiaoId", conn);
                cmd.Parameters.AddWithValue("@RegiaoId", ddlRegioes.SelectedValue);
                conn.Open();
                ddlEstNasc.DataSource = cmd.ExecuteReader();
                ddlEstNasc.DataTextField = "SiglaUf";
                ddlEstNasc.DataValueField = "Uf";
                ddlEstNasc.DataBind();
                ddlEstNasc.Items.Insert(0, new ListItem("Selecione", "0"));
            }
        }
        private void CarregarMunicipioNasc()
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                SqlCommand cmd = new SqlCommand("SELECT nome_municipio FROM tbmunicipiosbrasileiros WHERE IdRegiao = @RegiaoId AND Uf = @Uf", conn);
                cmd.Parameters.AddWithValue("@RegiaoId", ddlRegioes.SelectedValue);
                cmd.Parameters.AddWithValue("@Uf", ddlEstNasc.SelectedValue);
                conn.Open();
                ddlMunicipioNasc.DataSource = cmd.ExecuteReader();
                ddlMunicipioNasc.DataTextField = "nome_municipio";
                ddlMunicipioNasc.DataValueField = "nome_municipio";
                ddlMunicipioNasc.DataBind();
                ddlMunicipioNasc.Items.Insert(0, new ListItem("Selecione", "0"));
            }
        }
        protected void ddlRegioes_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarEstadosNascimento(); // Atualiza subcategorias com base na categoria
            CarregarMunicipioNasc();         // Atualiza itens com base na nova subcategoria
        }
        protected void ddlEstNasc_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarMunicipioNasc(); // Atualiza itens com base na nova subcategoria
        }
        private void CarregarEstCNH()
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                SqlCommand cmd = new SqlCommand("SELECT Uf, SiglaUf FROM tbestadosbrasileiros", conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                ddlCNH.DataSource = reader;
                ddlCNH.DataTextField = "SiglaUf";
                ddlCNH.DataValueField = "Uf";
                ddlCNH.DataBind();

                ddlCNH.Items.Insert(0, new ListItem("Selecione", "0"));
            }
        }
        protected void ddlCNH_SelectedIndexChanged(object sender, EventArgs e)
        {
            int cidadeId = int.Parse(ddlCNH.SelectedValue);
            CarregarMunicipioCNH(cidadeId);
        }
        private void CarregarMunicipioCNH(int cidadeId)
        {
            ddlMunicCnh.Items.Clear();

            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                SqlCommand cmd = new SqlCommand("SELECT Uf, nome_municipio FROM tbmunicipiosbrasileiros WHERE Uf = @cidadeId", conn);
                cmd.Parameters.AddWithValue("@cidadeId", cidadeId);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                ddlMunicCnh.DataSource = reader;
                ddlMunicCnh.DataTextField = "nome_municipio";
                ddlMunicCnh.DataValueField = "Uf";
                ddlMunicCnh.DataBind();

                ddlMunicCnh.Items.Insert(0, new ListItem("Selecione uma cidade ...", "0"));
            }
        }
        protected void btnCep_Click(object sender, EventArgs e)
        {
            WebCEP cep = new WebCEP(txtCepCli.Text);
            txtBaiCli.Text = cep.Bairro.ToString();
            txtCidCli.Text = cep.Cidade.ToString();
            txtEndCli.Text = cep.TipoLagradouro.ToString() + " " + cep.Lagradouro.ToString();
            txtEstCli.Text = cep.UF.ToString();
            txtNumero.Focus();
        }
        protected void btnSalvar1_Click(object sender, EventArgs e)
        {
            string sql = @"INSERT INTO tbmotoristas (codmot, nommot, status, emissaorg, numrg, cargo, nucleo, orgaorg, cpf, numregcnh, codsegurancacnh, catcnh, venccnh, codliberacao, numpis, endmot, baimot, cidmot, ufmot, cepmot, fone3, fone2, validade, dtnasc, estcivil, sexo, nomepai, nomemae, codtra, transp, cadmot, cartaomot, naturalmot, numero, complemento, tipomot, venccartao, horario, funcao, frota, usucad, dtccad, venceti, caminhofoto, ufnascimento, formulariocnh, ufcnh, municipiocnh, vencmoop, cracha, regiao, numinss)
              VALUES
              (@codmot, @nommot, @status, @emissaorg, @numrg, @cargo, @nucleo, @orgaorg, @cpf, @numregcnh, @codsegurancacnh, @catcnh,@venccnh, @codliberacao, @numpis, @endmot, @baimot, @cidmot, @ufmot, @cepmot, @fone3, @fone2, @validade, @dtnasc, @estcivil, @sexo, @nomepai, @nomemae, @codtra, @transp, @cadmot, @cartaomot, @naturalmot, @numero, @complemento, @tipomot, @venccartao, @horario, @funcao, @frota, @usucad, @dtccad, @venceti, @caminhofoto, @ufnascimento, @formulariocnh, @ufcnh, @municipiocnh, @vencmoop, @cracha, @regiao, @numinss)";

            try
            {
                string connStr = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    // Adicionando os parâmetros da inserção
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@codmot", txtCodMot.Text.Trim().ToUpper());
                    cmd.Parameters.AddWithValue("@nommot", txtNomMot.Text.Trim().ToUpper());
                    cmd.Parameters.AddWithValue("@status", "ATIVO");
                    cmd.Parameters.AddWithValue("@emissaorg", DateTime.Parse(txtDtEmissao.Text).ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@numrg", txtRG.Text.Trim().ToUpper());
                    cmd.Parameters.AddWithValue("@cargo", ddlCargo.SelectedItem.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@nucleo", cbFiliais.SelectedItem.ToString().Trim().ToUpper());
                    cmd.Parameters.AddWithValue("@orgaorg", txtEmissor.Text.Trim().ToUpper());
                    cmd.Parameters.AddWithValue("@cpf", txtCPF.Text);
                    cmd.Parameters.AddWithValue("@numregcnh", txtRegCNH.Text.ToUpper().Trim());
                    cmd.Parameters.AddWithValue("@codsegurancacnh", txtCodSeguranca.Text.ToUpper().Trim());
                    cmd.Parameters.AddWithValue("@catcnh", ddlCat.SelectedItem.ToString().Trim().ToUpper());
                    cmd.Parameters.AddWithValue("@venccnh", DateTime.Parse(txtValCNH.Text.ToUpper()).ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@codliberacao", txtCodLibRisco.Text.Trim());
                    cmd.Parameters.AddWithValue("@numpis", txtPIS.Text);
                    cmd.Parameters.AddWithValue("@endmot", txtEndCli.Text.ToUpper().Trim());
                    cmd.Parameters.AddWithValue("@baimot", txtBaiCli.Text.ToUpper().Trim());
                    cmd.Parameters.AddWithValue("@cidmot", txtCidCli.Text.ToUpper().Trim());
                    cmd.Parameters.AddWithValue("@ufmot", txtEstCli.Text.ToUpper().Trim());
                    cmd.Parameters.AddWithValue("@cepmot", txtCepCli.Text.Trim());
                    cmd.Parameters.AddWithValue("@fone3", txtFixo.Text);
                    cmd.Parameters.AddWithValue("@fone2", txtCelular.Text);
                    cmd.Parameters.AddWithValue("@validade", txtValLibRisco.Text);
                    cmd.Parameters.AddWithValue("@dtnasc", DateTime.Parse(txtDtNasc.Text).ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@estcivil", ddlEstCivil.SelectedItem.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@sexo", ddlSexo.SelectedItem.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@nomepai", txtNomePai.Text.Trim().ToUpper());
                    cmd.Parameters.AddWithValue("@nomemae", txtNomeMae.Text.Trim().ToUpper());
                    cmd.Parameters.AddWithValue("@codtra", txtCodTra.Text.Trim());
                    cmd.Parameters.AddWithValue("@transp", ddlAgregados.SelectedItem.ToString().Trim().ToUpper());
                    cmd.Parameters.AddWithValue("@cadmot", DateTime.Parse(txtDtCad.Text).ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@cartaomot", txtCartao.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@naturalmot", ddlMunicipioNasc.SelectedItem.ToString().Trim().ToUpper());
                    cmd.Parameters.AddWithValue("@numero", txtNumero.Text.Trim());
                    cmd.Parameters.AddWithValue("@complemento", txtComplemento.Text.Trim());
                    cmd.Parameters.AddWithValue("@tipomot", ddlTipoMot.SelectedItem.ToString().Trim().ToUpper());
                    cmd.Parameters.AddWithValue("@venccartao", txtValCartao.Text);
                    cmd.Parameters.AddWithValue("@horario", ddlJornada.SelectedItem.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@funcao", ddlFuncao.SelectedItem.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@frota", txtFrota.Text.Trim());
                    cmd.Parameters.AddWithValue("@usucad", txtUsuCadastro.Text.Trim().ToUpper()); // Usuário atual
                    cmd.Parameters.AddWithValue("@dtccad", dataHoraAtual.ToString("dd/MM/yyyy HH:mm"));
                    cmd.Parameters.AddWithValue("@venceti", txtVAlExameTox.Text);
                    if (FileUpload1.HasFile)
                    {
                        try
                        {
                            // Nome original
                            string originalName = Path.GetFileName(FileUpload1.FileName);

                            // Novo nome (por exemplo, com timestamp)
                            //string novoNome = "img_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + Path.GetExtension(originalName);
                            string novoNome = txtCodMot.Text.Trim().ToUpper() + Path.GetExtension(originalName);

                            // Caminho da nova pasta (por exemplo: ~/FotosSalvas/)
                            string pastaDestino = Server.MapPath("~/fotos/");

                            // Cria a pasta se não existir
                            if (!Directory.Exists(pastaDestino))
                            {
                                Directory.CreateDirectory(pastaDestino);
                            }

                            // Caminho completo para salvar a cópia
                            string caminhoCompleto = Path.Combine(pastaDestino, novoNome);

                            // Salva o arquivo com novo nome na nova pasta
                            FileUpload1.SaveAs(caminhoCompleto);



                            //lblMensagem.Text = "Imagem salva com sucesso como " + novoNome;
                        }
                        catch (Exception ex)
                        {
                            Response.Write("Erro ao salvar foto: " + ex.Message);
                        }
                    }
                    else
                    {
                        //cmd.Parameters.AddWithValue("@caminhofoto", "/fotos/");
                    }
                    cmd.Parameters.AddWithValue("@caminhofoto", "/fotos/" + txtCodMot.Text.Trim().ToUpper() + ".jpg");
                    cmd.Parameters.AddWithValue("@ufnascimento", ddlEstNasc.SelectedItem.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@formulariocnh", txtFormCNH.Text.Trim());
                    cmd.Parameters.AddWithValue("@ufcnh", ddlCNH.SelectedItem.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@municipiocnh", ddlMunicCnh.SelectedItem.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@vencmoop", txtVAlMoop.Text);
                    cmd.Parameters.AddWithValue("@cracha", txtCracha.Text.Trim());
                    cmd.Parameters.AddWithValue("@regiao", ddlRegioes.SelectedItem.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@numinss", txtINSS.Text.Trim());




                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        string nomeUsuario = txtUsuCadastro.Text;
                        string mensagem = $"Olá, {nomeUsuario}!\nMotorista com código {txtCodMot.Text} cadastrado com sucesso.";
                        string script = $"alert('{HttpUtility.JavaScriptStringEncode(mensagem)}');";
                        ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", script, true);

                        // Redirecionar para a página de consulta
                        Response.Redirect("/dist/pages/ConsultaMotoristas.aspx");
                    }
                    catch (Exception ex)
                    {
                        Response.Write("Erro ao salvar: " + ex.Message);
                    }

                    // Abrindo a conexão e executando a query
                    //conn.Open();
                    //int rowsInserted = cmd.ExecuteNonQuery();

                    //if (rowsInserted > 0)
                    //{
                    //    string nomeUsuario = txtUsuCadastro.Text;
                    //    string mensagem = $"Olá, {nomeUsuario}!\nMotorista com código {txtCodMot.Text} cadastrado com sucesso.";
                    //    string script = $"alert('{HttpUtility.JavaScriptStringEncode(mensagem)}');";
                    //    ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", script, true);

                    //    // Redirecionar para a página de consulta
                    //    Response.Redirect("/dist/pages/ConsultaMotoristas.aspx");
                    //}
                    //else
                    //{
                    //    string mensagem = "Falha ao cadastrar o veículo. Tente novamente.";
                    //    string script = $"alert('{HttpUtility.JavaScriptStringEncode(mensagem)}');";
                    //    ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeErro", script, true);
                    //}
                }
            }
            catch (Exception ex)
            {
                string mensagemErro = "Erro ao cadastrar o motorista: " + ex.Message;
                string scriptErro = $"alert('{HttpUtility.JavaScriptStringEncode(mensagemErro)}');";
                ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeErro", scriptErro, true);
            }
        }
        protected void txtCodMot_TextChanged(object sender, EventArgs e)
        {
            //string termo = txtCodMot.Text.ToUpper();
            //string strConn = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;

            //using (SqlConnection conn = new SqlConnection(strConn))
            //{
            //    string query = "SELECT TOP 1 codmot FROM tbmotoristas WHERE codmot LIKE @termo";
            //    SqlCommand cmd = new SqlCommand(query, conn);
            //    cmd.Parameters.AddWithValue("@termo", "%" + termo + "%");
            //    conn.Open();

            //    object res = cmd.ExecuteScalar();
            //    if (res != null)
            //    {
            //        //resultado = "Resultado: " + res.ToString();
            //        string retorno = "Código: " + res.ToString() + ", já cadastrado. ";
            //        System.Text.StringBuilder sb = new System.Text.StringBuilder();
            //        sb.Append("<script type = 'text/javascript'>");
            //        sb.Append("document.addEventListener('DOMContentLoaded', function() {");
            //        sb.Append("alert('");
            //        sb.Append(retorno);
            //        sb.Append("');");
            //        sb.Append("});");
            //        sb.Append("</script>");
            //        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());

            //    }
            //}

            ////lblResultado.Text = resultado;
            //txtCodMot.Text.ToUpper();
            //// ddlTipoMot.Focus();
            ///
            if (txtCodMot.Text != "")
            {
                string cod = txtCodMot.Text.Trim().ToUpper();
                string sql = "select codmot, nommot from tbmotoristas where codmot='" + cod + "'";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                conn.Open();
                da.Fill(dt);
                conn.Close();

                if (dt.Rows.Count > 0)
                {

                    ExibirToast("Código: " + txtCodMot.Text.Trim() + " ("+ dt.Rows[0][1].ToString() + "), já cadastrado no sistema.");
                    Thread.Sleep(5000);
                    txtCodMot.Text = "";
                    txtCodMot.Focus();
                }
            }
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
                ddlAgregados.Items.Insert(0, new ListItem("", "0"));
            }
        }
        // Evento disparado quando o item do DropDownList é alterado
        protected void ddlAgregados_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idSelecionado = int.Parse(ddlAgregados.SelectedValue);

            // Preencher os campos com base no valor selecionado
            if (idSelecionado > 0)
            {
                PreencherCampos(idSelecionado);
            }
            else
            {
                LimparCampos();
            }
        }
        // Função para preencher os campos com os dados do banco
        private void PreencherCampos(int id)
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                conn.Open();
                string query = "SELECT codtra, fantra FROM tbtransportadoras WHERE ID = @ID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtCodTra.Text = reader["codtra"].ToString();
                    //ddlAgregados.Text = reader["fantra"].ToString();

                }
            }
        }
        // Função para limpar os campos
        private void LimparCampos()
        {
            txtCodTra.Text = string.Empty;

        }

        private void CarregarCargos()
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                conn.Open();
                string query = "SELECT cod_funcao, nm_funcao FROM tb_funcao ORDER BY nm_funcao";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                ddlCargo.DataSource = reader;
                ddlCargo.DataTextField = "nm_funcao";  // Campo a ser exibido
                ddlCargo.DataValueField = "cod_funcao";  // Valor associado ao item
                ddlCargo.DataBind();

                // Adicionar o item padrão
                ddlCargo.Items.Insert(0, new ListItem("", "0"));
            }
        }
        // Evento disparado quando o item do DropDownList é alterado
        protected void ddlCargo_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idSelecionado = int.Parse(ddlCargo.SelectedValue);

            // Preencher os campos com base no valor selecionado
            if (idSelecionado > 0)
            {
                // PreencherCamposCargo(idSelecionado);
            }
            else
            {
                // LimparCamposCidades();
            }
        }
        // Função para preencher os campos com os dados do banco
        private void PreencherComboCargo()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT cod_funcao, nm_funcao FROM tb_funcao";

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
                    ddlCargo.DataSource = reader;
                    ddlCargo.DataTextField = "nm_funcao";  // Campo que será mostrado no ComboBox
                    ddlCargo.DataValueField = "cod_funcao";  // Campo que será o valor de cada item                    
                    ddlCargo.DataBind();  // Realiza o binding dos dados                   
                                          // Adicionar o item padrão
                    ddlCargo.Items.Insert(0, new ListItem("", "0"));
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
        // Função para limpar os campos
        private void LimparCamposCargo()
        {
            ddlCargo.Text = string.Empty;
        }
        // Função para preencher a combo Filial
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
        private void PreencherComboJornada()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT id, descricao FROM tbhorarios";

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
                    ddlJornada.DataSource = reader;
                    ddlJornada.DataTextField = "descricao";  // Campo que será mostrado no ComboBox
                    ddlJornada.DataValueField = "id";  // Campo que será o valor de cada item                    
                    ddlJornada.DataBind();  // Realiza o binding dos dados                   
                    ddlJornada.Items.Insert(0, new ListItem("", "0"));
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
        protected void ExibirToast(string mensagem)
        {
            string script = $@"
        <script>
            document.getElementById('toastMessage').innerText = '{mensagem.Replace("'", "\\'")}';
            var toastEl = document.getElementById('myToast');
            var toast = new bootstrap.Toast(toastEl);
            toast.show();
        </script>";

            ClientScript.RegisterStartupScript(this.GetType(), "toastScript", script, false);
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

        protected void txtCodSeguranca_TextChanged(object sender, EventArgs e)
        {
            if (txtCodSeguranca.Text != "")
            {
                string textoDigitado = txtCodSeguranca.Text;
                int numeroDeCaracteres = textoDigitado.Length;
                if (numeroDeCaracteres < 22)
                {
                    //ExibirToastErro("Código de Segurança: " + txtCodSeguranca.Text.Trim() + " é invalido.");
                    //Thread.Sleep(5000);

                    // Acione o toast quando a página for carregada
                    string script = "<script>showToast('Código de segurança digitado, é inválido.');</script>";
                    ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);

                    txtCodSeguranca.Text = "";
                    txtCodSeguranca.Focus();

                }

            }
        }
    }
}