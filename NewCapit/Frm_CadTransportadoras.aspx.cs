using System;
using System.Collections.Generic;
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

namespace NewCapit
{
    public partial class Frm_CadTransportadoras : System.Web.UI.Page
    {
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

                    DateTime dataHoraAtual = DateTime.Now;
                    txtDtCadastro.Text = dataHoraAtual.ToString("dd/MM/yyyy");
                    txtDtUsu.Text = dataHoraAtual.ToString("dd/MM/yyyy HH:mm");
                }
                else
                {
                    var lblUsuario = "<Usuário>";
                }
                

            }

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
                var codigo = txtCodTra.Text.Trim();
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
            using (SqlConnection connection = con)
            {
                string query = @"
                    INSERT INTO tbtransportadoras 
                    (codtra, dtcad, nomtra, contra, fantra, fone1, fone2, endtra, ceptra, baitra, cidtra, uftra, ativa_inativa, pessoa, cnpj, inscestadual, numero, complemento, dtccad, usucad, antt, filial) 
                    VALUES 
                    (@CodTra, @DtCad, @NomTra, @ConTra, @FanTra, @Fone1, @Fone2, @EndTra, @CepTra, @BaiTra, @CidTra, @UfTra, @AtivaInativa, @Pessoa, @Cnpj, @InscEstadual, @Numero, @Complemento, @DtCCad, @UsuCad, @Antt, @Filial)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CodTra", txtCodTra.Text);
                    command.Parameters.AddWithValue("@DtCad", txtDtCadastro.Text);
                    command.Parameters.AddWithValue("@NomTra", txtRazCli.Text);
                    command.Parameters.AddWithValue("@ConTra", txtContato.Text);
                    command.Parameters.AddWithValue("@FanTra", txtFantasia.Text);
                    command.Parameters.AddWithValue("@Fone1", txtFixo.Text);
                    command.Parameters.AddWithValue("@Fone2", txtCelular.Text);
                    command.Parameters.AddWithValue("@EndTra", txtEndCli.Text);
                    command.Parameters.AddWithValue("@CepTra", txtCepCli.Text);
                    command.Parameters.AddWithValue("@BaiTra", txtBaiCli.Text);
                    command.Parameters.AddWithValue("@CidTra", txtCidCli.Text);
                    command.Parameters.AddWithValue("@UfTra", txtEstCli.Text);
                    command.Parameters.AddWithValue("@AtivaInativa", ddlSituacao.SelectedValue);
                    command.Parameters.AddWithValue("@Pessoa", cboPessoa.SelectedValue);
                    command.Parameters.AddWithValue("@Cnpj", txtCpf_Cnpj.Text);
                    command.Parameters.AddWithValue("@InscEstadual", txtRg.Text);
                    command.Parameters.AddWithValue("@Numero", txtNumero.Text);
                    command.Parameters.AddWithValue("@Complemento", txtComplemento.Text);
                    command.Parameters.AddWithValue("@DtCCad", txtDtCadastro.Text);
                    command.Parameters.AddWithValue("@UsuCad", txtUsuCadastro.Text);
                    //command.Parameters.AddWithValue("@DtCAlt", txt);
                    //command.Parameters.AddWithValue("@UsuAlt", usuAlt);
                    command.Parameters.AddWithValue("@Antt", txtAntt.Text);
                    command.Parameters.AddWithValue("@Filial", cbFiliais.SelectedValue);
                    //command.Parameters.AddWithValue("@Tipo", );

                    try
                    {
                        con.Open();
                        command.ExecuteNonQuery();
                        con.Close();
                        string nomeUsuario = txtUsuCadastro.Text;
                        string linha1 = "Olá, " + nomeUsuario + "!";
                        string linha2 = "Código " + txtCodTra.Text + ", cadastrado com sucesso.";
                        // Concatenando as linhas com '\n' para criar a mensagem
                        string mensagem = $"{linha1}\n{linha2}";
                        string mensagemCodificada = HttpUtility.JavaScriptStringEncode(mensagem);
                        // Gerando o script JavaScript para exibir o alerta
                        string script = $"alert('{mensagemCodificada}');";
                        // Registrando o script para execução no lado do cliente
                        ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", script, true);
                        //Chama a página de consulta clientes
                        Response.Redirect("ConsultaAgregados.aspx");

                    }
                    catch (Exception ex)
                    {
                        var message = new JavaScriptSerializer().Serialize(ex.Message.ToString());
                        string retorno = "Erro! Contate o administrador. Detalhes do erro: " + message;
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append("<script type = 'text/javascript'>");
                        sb.Append("window.onload=function(){");
                        sb.Append("alert('");
                        sb.Append(retorno);
                        sb.Append("')};");
                        sb.Append("</script>");
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
                        //Chama a página de consulta clientes
                        Response.Redirect("ConsultaClientes.aspx");
                    }

                    finally
                    {
                        con.Close();
                    }
                }
            }

        }
    }
}