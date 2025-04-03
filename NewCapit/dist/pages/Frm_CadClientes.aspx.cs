using System;
using System.Data.SqlClient;
using System.Security.Policy;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using Domain;
using DAL;
using System.Data;

namespace NewCapit.dist.pages
{
    public partial class Frm_CadClientes : System.Web.UI.Page
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

                }
                else
                {
                    var lblUsuario = "<Usuário>";
                    txtUsuCadastro.Text = lblUsuario;
                } 
            }
            DateTime dataHoraAtual = DateTime.Now;
            lblDtCadastro.Text = dataHoraAtual.ToString("dd/MM/yyyy HH:mm");
        }
       
        protected void btnCliente_Click(object sender, EventArgs e)
        {
            if (txtCodCli.Text.Trim() == "")
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
                txtCodCli.Focus();

            }
            else
            {
                var codigo = txtCodCli.Text.Trim();

                var obj = new ConsultaCliente
                {
                    codcli = codigo
                };


                var ConsultaCliente = DAL.UsersDAL.CheckCliente(obj);
                if (ConsultaCliente != null)
                {
                    string nomeUsuario = txtUsuCadastro.Text;
                    string razaoSocial = ConsultaCliente.razcli;
                    string unidade = ConsultaCliente.unidade;

                    string linha1 = "Olá, " + nomeUsuario + "!";
                    string linha2 = "Código " + codigo + ", já cadastrado no sistema.";
                    string linha3 = "Razão Social: " + razaoSocial + ".";
                    string linha4 = "Unidade: " + unidade + ". Por favor, verifique.";

                    // Concatenando as linhas com '\n' para criar a mensagem
                    string mensagem = $"{linha1}\n{linha2}\n{linha3}\n{linha4}";

                    string mensagemCodificada = HttpUtility.JavaScriptStringEncode(mensagem);
                    // Gerando o script JavaScript para exibir o alerta
                    string script = $"alert('{mensagemCodificada}');";

                    // Registrando o script para execução no lado do cliente
                    ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", script, true);

                    txtCodCli.Text = "";
                    txtCodCli.Focus();
                }
                else
                {
                    cboTipo.Focus();
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
            string cnpjSemMascara = RemoverMascaraCNPJ(txtCnpj.Text);
            var cnpj = Empresa.ObterCnpj(cnpjSemMascara);
            if (cnpj != null)
            {
                var cep = RemoverMascaraCep(cnpj.cep);
                txtRazCli.Text = cnpj.nome;
                txtTipo.Text = cnpj.tipo;
                txtAbertura.Text = cnpj.abertura;
                txtSituacao.Text = cnpj.situacao;
                txtNomCli.Text = cnpj.fantasia;
                txtCepCli.Text = cep;
                txtEndCli.Text = cnpj.logradouro;
                txtNumero.Text = cnpj.numero;
                txtComplemento.Text = cnpj.complemento;
                txtBaiCli.Text = cnpj.bairro;
                txtCidCli.Text = cnpj.municipio;
                txtEstCli.Text = cnpj.uf;

            }


        }
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            string sqlSalvarCliente = "insert into tbclientes" + "(codcli,dtccli,razcli,concli,nomcli,tc1cli,tc2cli,endcli,cepcli,baicli,cidcli,estcli,programador,contato,email,codvw,cnpj,inscestadual,numero,complemento,codsapiens,longitude,latitude,ativo_inativo,usucad,dtccad,tipo,unidade,raio,regiao,abertura,situacao,tipoempresa,ramal)" +
              "values" + "(@codcli,@dtccli,@razcli,@concli,@nomcli,@tc1cli,@tc2cli,@endcli,@cepcli,@baicli,@cidcli,@estcli,@programador,@contato,@email,@codvw,@cnpj,@inscestadual,@numero,@complemento,@codsapiens,@longitude,@latitude,@ativo_inativo,@usucad,@dtccad,@tipo,@unidade,@raio,@regiao,@abertura,@situacao,@tipoempresa,@ramal)";
            //teste

            SqlCommand comando = new SqlCommand(sqlSalvarCliente, con);
            comando.Parameters.AddWithValue("@codcli", txtCodCli.Text);
            comando.Parameters.AddWithValue("@dtccli", DateTime.Parse(lblDtCadastro.Text).ToString("yyyy-MM-dd"));
            comando.Parameters.AddWithValue("@razcli", txtRazCli.Text.ToUpper());
            comando.Parameters.AddWithValue("@concli", txtConCli.Text.ToUpper());
            comando.Parameters.AddWithValue("@nomcli", txtNomCli.Text.ToUpper());
            comando.Parameters.AddWithValue("@tc1cli", txtTc1Cli.Text.ToUpper());
            comando.Parameters.AddWithValue("@tc2cli", txtTc2Cli.Text.ToUpper());
            comando.Parameters.AddWithValue("@endcli", txtEndCli.Text.ToUpper());
            comando.Parameters.AddWithValue("@cepcli", txtCepCli.Text.ToUpper());
            comando.Parameters.AddWithValue("@baicli", txtBaiCli.Text.ToUpper());
            comando.Parameters.AddWithValue("@cidcli", txtCidCli.Text.ToUpper());
            comando.Parameters.AddWithValue("@estcli", txtEstCli.Text.ToUpper());
            comando.Parameters.AddWithValue("@programador", txtProgramador.Text.ToUpper());
            comando.Parameters.AddWithValue("@contato", txtContato.Text.ToUpper());
            comando.Parameters.AddWithValue("@email", txtEmail.Text.ToUpper());
            comando.Parameters.AddWithValue("@codvw", txtCodVw.Text.ToUpper());
            comando.Parameters.AddWithValue("@cnpj", txtCnpj.Text);
            comando.Parameters.AddWithValue("@inscestadual", txtInscEstadual.Text);
            comando.Parameters.AddWithValue("@numero", txtNumero.Text.ToUpper());
            comando.Parameters.AddWithValue("@complemento", txtComplemento.Text.ToUpper());
            comando.Parameters.AddWithValue("@codsapiens", txtCodSapiens.Text.ToUpper());
            comando.Parameters.AddWithValue("@longitude", longitude.Text.ToUpper());
            comando.Parameters.AddWithValue("@latitude", latitude.Text.ToUpper());
            comando.Parameters.AddWithValue("@ativo_inativo", status.SelectedValue.ToUpper());
            comando.Parameters.AddWithValue("@usucad", txtUsuCadastro.Text.ToUpper());
            comando.Parameters.AddWithValue("@dtccad", lblDtCadastro.Text);
            comando.Parameters.AddWithValue("@tipo", cboTipo.SelectedValue.ToUpper());
            comando.Parameters.AddWithValue("@unidade", txtUnidade.Text.ToUpper());
            comando.Parameters.AddWithValue("@raio", txtRaio.Text.ToUpper());
            comando.Parameters.AddWithValue("@regiao", cboRegiao.SelectedValue.ToUpper());
            comando.Parameters.AddWithValue("@abertura", txtAbertura.Text);
            comando.Parameters.AddWithValue("@situacao", txtSituacao.Text.ToUpper());
            comando.Parameters.AddWithValue("@tipoempresa", txtTipo.Text.ToUpper());
            comando.Parameters.AddWithValue("@ramal", txtRamal.Text);

            try
            {
                con.Open();
                comando.ExecuteNonQuery();
                con.Close();
                string nomeUsuario = txtUsuCadastro.Text;
                string linha1 = "Olá, " + nomeUsuario + "!";
                string linha2 = "Código " + txtCodCli.Text + ", cadastrado com sucesso.";
                // Concatenando as linhas com '\n' para criar a mensagem
                string mensagem = $"{linha1}\n{linha2}";
                string mensagemCodificada = HttpUtility.JavaScriptStringEncode(mensagem);
                // Gerando o script JavaScript para exibir o alerta
                string script = $"alert('{mensagemCodificada}');";
                // Registrando o script para execução no lado do cliente
                ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", script, true);
                //Chama a página de consulta clientes
                Response.Redirect("ConsultaClientes.aspx");

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

        private void SalvarCliente()
        {
            string sqlSalvarCliente = "insert into tbclientes" + "(codcli,dtccli,razcli,concli,nomcli,tc1cli,tc2cli,endcli,cepcli,baicli,cidcli,estcli,programador,contato,email,codvw,cnpj,inscestadual,numero,complemento,codsapiens,longitude,latitude,ativo_inativo,usucad,dtccad,tipo,unidade,raio,regiao,abertura,situacao,tipoempresa)" +
              "values" + "(@codcli,@dtccli,@razcli,@concli,@nomcli,@tc1cli,@tc2cli,@endcli,@cepcli,@baicli,@cidcli,@estcli,@programador,@contato,@email,@codvw,@cnpj,@inscestadual,@numero,@complemento,@codsapiens,@longitude,@latitude,@ativo_inativo,@usucad,@dtccad,@tipo,@unidade,@raio,@regiao,@abertura,@situacao,@tipoempresa)";
            SqlCommand comando = new SqlCommand(sqlSalvarCliente, con);
            comando.Parameters.AddWithValue("@codcli", txtCodCli.Text);
            comando.Parameters.AddWithValue("@dtccli", DateTime.Parse(lblDtCadastro.Text).ToString("yyyy-MM-dd"));
            comando.Parameters.AddWithValue("@razcli", txtRazCli.Text.ToUpper());
            comando.Parameters.AddWithValue("@concli", txtConCli.Text.ToUpper());
            comando.Parameters.AddWithValue("@nomcli", txtNomCli.Text.ToUpper());
            comando.Parameters.AddWithValue("@tc1cli", txtTc1Cli.Text.ToUpper());
            comando.Parameters.AddWithValue("@tc2cli", txtTc2Cli.Text.ToUpper());
            comando.Parameters.AddWithValue("@endcli", txtEndCli.Text.ToUpper());
            comando.Parameters.AddWithValue("@cepcli", txtCepCli.Text.ToUpper());
            comando.Parameters.AddWithValue("@baicli", txtBaiCli.Text.ToUpper());
            comando.Parameters.AddWithValue("@cidcli", txtCidCli.Text.ToUpper());
            comando.Parameters.AddWithValue("@estcli", txtEstCli.Text.ToUpper());
            comando.Parameters.AddWithValue("@programador", txtProgramador.Text.ToUpper());
            comando.Parameters.AddWithValue("@contato", txtContato.Text.ToUpper());
            comando.Parameters.AddWithValue("@email", txtEmail.Text.ToUpper());
            comando.Parameters.AddWithValue("@codvw", txtCodVw.Text.ToUpper());
            comando.Parameters.AddWithValue("@cnpj", txtCnpj.Text);
            comando.Parameters.AddWithValue("@inscestadual", txtInscEstadual.Text);
            comando.Parameters.AddWithValue("@numero", txtNumero.Text.ToUpper());
            comando.Parameters.AddWithValue("@complemento", txtComplemento.Text.ToUpper());
            comando.Parameters.AddWithValue("@codsapiens", txtCodSapiens.Text.ToUpper());
            comando.Parameters.AddWithValue("@longitude", longitude.Text.ToUpper());
            comando.Parameters.AddWithValue("@latitude", latitude.Text.ToUpper());
            comando.Parameters.AddWithValue("@ativo_inativo", status.SelectedValue.ToUpper());
            comando.Parameters.AddWithValue("@usucad", txtUsuCadastro.Text.ToUpper());
            comando.Parameters.AddWithValue("@dtccad", lblDtCadastro.Text);
            comando.Parameters.AddWithValue("@tipo", cboTipo.SelectedValue.ToUpper());
            comando.Parameters.AddWithValue("@unidade", txtUnidade.Text.ToUpper());
            comando.Parameters.AddWithValue("@raio", txtRaio.Text.ToUpper());
            comando.Parameters.AddWithValue("@regiao", cboRegiao.SelectedValue.ToUpper());
            comando.Parameters.AddWithValue("@abertura", txtAbertura.Text);
            comando.Parameters.AddWithValue("@situacao", txtSituacao.Text.ToUpper());
            comando.Parameters.AddWithValue("@tipoempresa", txtTipo.Text.ToUpper());

            try
            {
                con.Open();
                comando.ExecuteNonQuery();
                con.Close();
                string nomeUsuario = txtUsuCadastro.Text;
                string linha1 = "Olá, " + nomeUsuario + "!";
                string linha2 = "Código " + txtCodCli.Text + ", cadastrado com sucesso.";
                // Concatenando as linhas com '\n' para criar a mensagem
                string mensagem = $"{linha1}\n{linha2}";
                string mensagemCodificada = HttpUtility.JavaScriptStringEncode(mensagem);
                // Gerando o script JavaScript para exibir o alerta
                string script = $"alert('{mensagemCodificada}');";
                // Registrando o script para execução no lado do cliente
                ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", script, true);
                //Chama a página de consulta clientes
                Response.Redirect("ConsultaClientes.aspx");

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

        protected void btnCep_Click(object sender, EventArgs e)
        {
            WebCEP cep = new WebCEP(txtCepCli.Text);
            txtBaiCli.Text = cep.Bairro.ToString();
            txtCidCli.Text = cep.Cidade.ToString();
            txtEndCli.Text = cep.TipoLagradouro.ToString() + " " + cep.Lagradouro.ToString();
            txtEstCli.Text = cep.UF.ToString();
            txtNumero.Focus();
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {

            string url = "MapaClientePesquisa.aspx?lat=" + latitude.Text + "&long=" + longitude.Text;
            string script = $"window.open('{url}', '_blank', 'width=800,height=600,scrollbars=yes,resizable=yes');";
            ClientScript.RegisterStartupScript(this.GetType(), "openWindow", script, true);
        }
    }
}