using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Domain;
using Microsoft.SqlServer.Server;

namespace NewCapit.dist.pages
{
    public partial class Frm_CadTransportadoras : System.Web.UI.Page
    {
        int sequencia;
        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString());
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
                }


            }
            DateTime dataHoraAtual = DateTime.Now;
            txtDtCadastro.Text = dataHoraAtual.ToString("dd/MM/yyyy");
            txtDtUsu.Text = dataHoraAtual.ToString("dd/MM/yyyy HH:mm");
            PreencherComboFiliais();            
            btnCnpj.Visible = false;

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
        protected void btnAgregado_Click(object sender, EventArgs e)
        {
            if (txtCodTra.Text.Trim() == "")
            {
                string nomeUsuario = txtUsuCadastro.Text;

                string linha1 = "Olá, " + nomeUsuario + "!";
                string linha2 = "Por favor, digite um código para cadastro.";

                // Concatenando as linhas com '\n' para criar a mensagem
                string mensagem = $"{linha1}\n{linha2}";

                string mensagemCodificada = HttpUtility.JavaScriptStringEncode(mensagem);
                // Gerando o script JavaScript para exibir o alerta
                string script = $"alert('{mensagemCodificada}');";

                // Registrando o script para execução no lado do cliente
                ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", script, true);
                txtCodTra.Focus();

            }
            else
            {
                var codigo = txtCodTra.Text.Trim().ToUpper();
                var obj = new ConsultaAgregado()
                {
                    codtra = codigo
                };


                var ConsultaAgregados = DAL.UsersDAL.CheckAgregado(obj);
                if (ConsultaAgregados != null)
                {
                    string nomeUsuario = txtUsuCadastro.Text;
                    string razaoSocial = ConsultaAgregados.fantra;
                    string filial = ConsultaAgregados.filial;


                    string linha1 = "Olá, " + nomeUsuario + "!";
                    string linha2 = "Código " + codigo + ", já cadastrado no sistema.";
                    string linha3 = "Nome: " + razaoSocial + ".";
                    string linha4 = "Filial: " + filial.Trim() + ". Por favor, verifique.";

                    // Concatenando as linhas com '\n' para criar a mensagem
                    string mensagem = $"{linha1}\n{linha2}\n{linha3}\n{linha4}";

                    string mensagemCodificada = HttpUtility.JavaScriptStringEncode(mensagem);
                    // Gerando o script JavaScript para exibir o alerta
                    string script = $"alert('{mensagemCodificada}');";

                    // Registrando o script para execução no lado do cliente
                    ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", script, true);

                    txtCodTra.Text = "";
                    txtCodTra.Focus();
                }
                else
                {
                    cboPessoa.Focus();
                }

            }
        }
        protected void btnCnpj_Click(object sender, EventArgs e)
        {
            PesquisarCnpj();
        }
        private string RemoverMascaraCNPJ(string cnpj)
        {
            // Remove os caracteres não numéricos (pontos, barras e traços)
            return System.Text.RegularExpressions.Regex.Replace(cnpj, @"[^\d]", "");
        }
        private string RemoverMascaraCep(string cep)
        {
            // Remove os caracteres não numéricos (pontos, barras e traços)
            return System.Text.RegularExpressions.Regex.Replace(cep, @"[^\d]", "");
        }
        private void PesquisarCnpj()
        {
            if (txtCpf_Cnpj.Text.Length == 14)
            {
                string cnpjSemMascara = RemoverMascaraCNPJ(txtCpf_Cnpj.Text);
                var cnpj = Empresa.ObterCnpj(cnpjSemMascara);
                if (cnpj != null)
                {
                    var cep = RemoverMascaraCep(cnpj.cep);
                    txtRazCli.Text = cnpj.nome;
                    txtCepCli.Text = cep;
                    txtEndCli.Text = cnpj.logradouro;
                    txtNumero.Text = cnpj.numero;
                    txtComplemento.Text = cnpj.complemento;
                    txtBaiCli.Text = cnpj.bairro;
                    txtCidCli.Text = cnpj.municipio;
                    txtEstCli.Text = cnpj.uf;
                }
                btnCnpj.Visible = true;
                //if (cnpj != null)
                //{
                //    if (cboPessoa.SelectedValue == "JURÍDICA")
                //    {                        
                //        string cnpjFormatado = string.Format("{000\\.000\\.000\\/0000-00}", long.Parse(cnpjSemMascara));
                //        txtCpf_Cnpj.Text = cnpjFormatado;
                //    }
                //}
            }
            else
            {
                string nomeUsuario = txtUsuCadastro.Text;
                string linha1 = "Olá, " + nomeUsuario + "!";
                string linha2 = "Quantidade de números digistados, não correspondem a um CNPJ válido.";
                string linha3 = "Por favor, verifique e tente novamente.";

                // Concatenando as linhas com '\n' para criar a mensagem
                string mensagem = $"{linha1}\n{linha2}\n{linha3}";

                string mensagemCodificada = HttpUtility.JavaScriptStringEncode(mensagem);
                // Gerando o script JavaScript para exibir o alerta
                string script = $"alert('{mensagemCodificada}');";

                // Registrando o script para execução no lado do cliente
                ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", script, true);

                txtCpf_Cnpj.Text = "";
                txtCpf_Cnpj.Focus();
                btnCnpj.Visible = true;
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
        protected void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboPessoa.SelectedValue == "JURÍDICA")
            {
                btnCnpj.Visible = true;
            }
            else
            {
                btnCnpj.Visible = false;
            }
        }
        protected void validaCPF()
        {

            if (txtCpf_Cnpj.Text != null)
            {
                if (txtCpf_Cnpj.Text.Length == 11)
                {
                    if (cboPessoa.SelectedValue == "FÍSICA")
                    {
                        string cpfFormatado = string.Format("{000\\.000\\.000\\-00}", long.Parse(txtCpf_Cnpj.Text));
                        txtCpf_Cnpj.Text = cpfFormatado;
                    }
                    else
                    {
                        string nomeUsuario = txtUsuCadastro.Text;
                        string linha1 = "Olá, " + nomeUsuario + "!";
                        string linha2 = "Quantidade de números digistados, não correspondem a um CPF válido.";
                        string linha3 = "Por favor, verifique e tente novamente.";

                        // Concatenando as linhas com '\n' para criar a mensagem
                        string mensagem = $"{linha1}\n{linha2}\n{linha3}";

                        string mensagemCodificada = HttpUtility.JavaScriptStringEncode(mensagem);
                        // Gerando o script JavaScript para exibir o alerta
                        string script = $"alert('{mensagemCodificada}');";

                        // Registrando o script para execução no lado do cliente
                        ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", script, true);

                        txtCpf_Cnpj.Text = "";
                        txtCpf_Cnpj.Focus();
                    }
                }
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                string query = @"
            INSERT INTO tbtransportadoras 
            (codtra, dtcad, nomtra, contra, fantra, fone1, fone2, endtra, ceptra, baitra, cidtra, uftra, ativa_inativa, pessoa, cnpj, inscestadual, numero, complemento, dtccad, usucad, antt, filial, tipo) 
            VALUES 
            (@CodTra, @DtCad, @NomTra, @ConTra, @FanTra, @Fone1, @Fone2, @EndTra, @CepTra, @BaiTra, @CidTra, @UfTra, @AtivaInativa, @Pessoa, @Cnpj, @InscEstadual, @Numero, @Complemento, @DtcCad, @UsuCad, @Antt, @Filial, @Tipo)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    GerarNumero();
                    command.Parameters.AddWithValue("@CodTra", txtCodTra.Text.Trim().ToUpper());
                    command.Parameters.AddWithValue("@NomTra", txtRazCli.Text.Trim().ToUpper());
                    command.Parameters.AddWithValue("@ConTra", txtContato.Text.Trim().ToUpper());
                    command.Parameters.AddWithValue("@FanTra", txtFantasia.Text.Trim().ToUpper());
                    command.Parameters.AddWithValue("@Fone1", txtFixo.Text.Trim());
                    command.Parameters.AddWithValue("@Fone2", txtCelular.Text.Trim());
                    command.Parameters.AddWithValue("@EndTra", txtEndCli.Text.Trim().ToUpper());
                    command.Parameters.AddWithValue("@CepTra", txtCepCli.Text.Trim());
                    command.Parameters.AddWithValue("@BaiTra", txtBaiCli.Text.Trim().ToUpper());
                    command.Parameters.AddWithValue("@CidTra", txtCidCli.Text.Trim().ToUpper());
                    command.Parameters.AddWithValue("@UfTra", txtEstCli.Text.Trim().ToUpper());
                    command.Parameters.AddWithValue("@AtivaInativa", ddlSituacao.SelectedValue);
                    command.Parameters.AddWithValue("@Pessoa", cboPessoa.SelectedItem.ToString());
                    command.Parameters.AddWithValue("@Cnpj", txtCpf_Cnpj.Text.Trim());
                    command.Parameters.AddWithValue("@InscEstadual", txtRg.Text.Trim());
                    command.Parameters.AddWithValue("@Numero", txtNumero.Text.Trim());
                    command.Parameters.AddWithValue("@Complemento", txtComplemento.Text.Trim().ToUpper());
                    command.Parameters.AddWithValue("@UsuCad", txtUsuCadastro.Text.Trim());
                    command.Parameters.AddWithValue("@Antt", txtAntt.Text.Trim());
                    command.Parameters.AddWithValue("@Filial", cbFiliais.SelectedItem.ToString());
                    command.Parameters.AddWithValue("@Tipo", ddlTipo.SelectedItem.ToString());
                    //command.Parameters.AddWithValue("@id", sequencia);

                    // Tratamento de Datas (Evita erro de formato)
                    DateTime dtCadastro;
                    if (DateTime.TryParse(txtDtCadastro.Text, out dtCadastro))
                    {
                        command.Parameters.AddWithValue("@DtCad", dtCadastro);
                        command.Parameters.AddWithValue("@DtCCad", txtDtUsu.Text);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@DtCad", DBNull.Value);
                        command.Parameters.AddWithValue("@DtCCad", DBNull.Value);
                    }

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();

                        // Mensagem de sucesso
                        string nomeUsuario = txtUsuCadastro.Text;
                        string mensagem = $"Olá, {nomeUsuario}!\nCódigo {txtCodTra.Text} cadastrado com sucesso.";
                        string mensagemCodificada = HttpUtility.JavaScriptStringEncode(mensagem);
                        string script = $"alert('{mensagemCodificada}');";
                        ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", script, true);

                        // Redireciona para a página de consulta
                        Response.Redirect("/dist/pages/Consulta_Agregados.aspx");
                    }
                    catch (Exception ex)
                    {
                        // Mensagem de erro
                        string erroMensagem = $"Erro ao cadastrar! Contate o administrador.\nDetalhes: {ex.Message}";
                        string erroScript = $"alert('{HttpUtility.JavaScriptStringEncode(erroMensagem)}');";
                        ClientScript.RegisterStartupScript(this.GetType(), "ErroCadastro", erroScript, true);
                    }
                }
            }
        }
        public void GerarNumero()
        {
            string sql_sequncia = " select isnull(max(id+1),1) as id from tbtransportadoras";

            con.Open();

            SqlDataAdapter da = new SqlDataAdapter(sql_sequncia, con);

            DataTable dt2 = new DataTable();

            da.Fill(dt2);

            sequencia = int.Parse(dt2.Rows[0][0].ToString());

            con.Close();
        }


    }


}