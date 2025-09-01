using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NewCapit.dist.pages
{
    public partial class Frm_CadFornecedores : System.Web.UI.Page
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
            CarregarPaises();
        }
        private void CarregarPaises()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT id, descricao FROM tbpaises";

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
                    ddlPaises.DataSource = reader;
                    ddlPaises.DataTextField = "descricao";  // Campo que será mostrado no ComboBox
                    ddlPaises.DataValueField = "id";  // Campo que será o valor de cada item                    
                    ddlPaises.DataBind();  // Realiza o binding dos dados                   
                                           //ddlEstados.Items.Insert(0, new ListItem("", "0"));
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
        protected void btnFornecedor_Click(object sender, EventArgs e)
        {
            if (txtCodFor.Text.Trim() != "")
            {
                var codigo = txtCodFor.Text.Trim();

                var obj = new Domain.ConsultaFornecedor
                {
                    codfor = codigo
                };


                var ConsultaFornecedor = DAL.UsersDAL.CheckFornecedor(obj);
                if (ConsultaFornecedor != null)
                {
                    // Acione o toast quando a página for carregada
                    string script = "<script>showToast('Código fornecedor, já cadastrado no sistema.');</script>";
                    ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                    txtCodFor.Text = "";
                    txtCodFor.Focus();
                }
                else
                {
                    txtCnpj.Focus();
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
            if (txtCnpj.Text == "")
            {
                // Acione o toast quando a página for carregada
                string script = "<script>showToast('Digite um CNPJ para pesquisar.');</script>";
                ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                txtCnpj.Focus();

            }
            else
            {

                string cnpjSemMascara = RemoverMascaraCNPJ(txtCnpj.Text);
                var cnpj = Empresa.ObterCnpj(cnpjSemMascara);
                if (cnpj != null)
                {
                    string cnpjPesquisar = txtCnpj.Text.Trim();
                    if (ValidarCNPJ(cnpjPesquisar))
                    {
                        var cep = RemoverMascaraCep(cnpj.cep);
                        txtRazFor.Text = cnpj.nome;
                        txtTipo.Text = cnpj.tipo;
                        txtAbertura.Text = cnpj.abertura;
                        txtSituacao.Text = cnpj.situacao;
                        txtNomFor.Text = cnpj.fantasia;
                        txtCepFor.Text = cep;
                        txtEndFor.Text = cnpj.logradouro;
                        txtNumero.Text = cnpj.numero;
                        txtComplemento.Text = cnpj.complemento;
                        txtBaiFor.Text = cnpj.bairro;
                        txtCidFor.Text = cnpj.municipio;
                        txtEstFor.Text = cnpj.uf;
                    }
                    else
                    {
                        // Acione o toast quando a página for carregada
                        string script = "<script>showToast('Digite um CNPJ válido.');</script>";
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                        txtCnpj.Text = "";
                        txtCnpj.Focus();
                    }
                }

            }
        }
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            string sqlSalvarFornecedor = "insert into tbfornecedores" +
              "(codfor, razaosocial, fantasia, cnpj, inscestadual, inscccm, tipoempresa, abertura, situacaoreceita, tipofornecedor, contato, fonefixo, fonecelular, email, site, cep, endereco, numero, complemento, bairro, cidade, estado, pais, dtcadastro, status, data_cadastro, usuario_cadastro)" +
              "values" +
              "(@codfor, @razaosocial, @fantasia, @cnpj, @inscestadual, @inscccm, @tipoempresa, @abertura, @situacaoreceita, @tipofornecedor, @contato, @fonefixo, @fonecelular, @email, @site, @cep, @endereco, @numero, @complemento, @bairro, @cidade, @estado, @pais, @dtcadastro, @status, @data_cadastro, @usuario_cadastro)";

            SqlCommand comando = new SqlCommand(sqlSalvarFornecedor, con);
            comando.Parameters.AddWithValue("@codfor", txtCodFor.Text);
            comando.Parameters.AddWithValue("@dtcadastro", DateTime.Parse(lblDtCadastro.Text).ToString("dd/MM/yyyy"));
            comando.Parameters.AddWithValue("@razaosocial", txtRazFor.Text.ToUpper());
            comando.Parameters.AddWithValue("@contato", txtConFor.Text.ToUpper());
            comando.Parameters.AddWithValue("@fantasia", txtNomFor.Text.ToUpper());
            comando.Parameters.AddWithValue("@fonefixo", txtTc1For.Text.ToUpper());
            comando.Parameters.AddWithValue("@fonecelular", txtTc2For.Text.ToUpper());
            comando.Parameters.AddWithValue("@endereco", txtEndFor.Text.ToUpper());
            comando.Parameters.AddWithValue("@cep", txtCepFor.Text.ToUpper());
            comando.Parameters.AddWithValue("@bairro", txtBaiFor.Text.ToUpper());
            comando.Parameters.AddWithValue("@cidade", txtCidFor.Text.ToUpper());
            comando.Parameters.AddWithValue("@estado", txtEstFor.Text.ToUpper());
            comando.Parameters.AddWithValue("@email", txtEmail.Text.ToLower());
            comando.Parameters.AddWithValue("@cnpj", txtCnpj.Text);
            comando.Parameters.AddWithValue("@inscestadual", txtInscEstadual.Text);
            comando.Parameters.AddWithValue("@numero", txtNumero.Text.ToUpper());
            comando.Parameters.AddWithValue("@complemento", txtComplemento.Text.ToUpper());
            comando.Parameters.AddWithValue("@usuario_cadastro", txtUsuCadastro.Text.ToUpper());
            comando.Parameters.AddWithValue("@data_cadastro", lblDtCadastro.Text);
            comando.Parameters.AddWithValue("@abertura", txtAbertura.Text);
            comando.Parameters.AddWithValue("@situacaoreceita", txtSituacao.Text.ToUpper());
            comando.Parameters.AddWithValue("@tipoempresa", txtTipo.Text.ToUpper());
            comando.Parameters.AddWithValue("@pais", ddlPaises.SelectedItem.Text);
            comando.Parameters.AddWithValue("@tipofornecedor", ddlTipoFornecedor.SelectedItem.Text);
            comando.Parameters.AddWithValue("@inscccm", txtInscCCM.Text.ToUpper());
            comando.Parameters.AddWithValue("@site", txtSite.Text.ToLower());
            comando.Parameters.AddWithValue("@status", "ATIVO");


            try
            {
                con.Open();
                comando.ExecuteNonQuery();
                con.Close();                

                // Acione o toast quando a página for carregada
                string script = "<script>showToast('Fornecedor cadastrado com sucesso.');</script>";
                ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                //Chama a página de consulta clientes
                Response.Redirect("/dist/pages/ConsultaFornecedores.aspx");

            }
            catch (Exception ex)
            {           

                // Acione o toast quando a página for carregada
                string script = "<script>showToast('Erro ao cadastrar fornecedor. Contate o administrador.');</script>";
                ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                //Chama a página de consulta clientes
                Response.Redirect("/dist/pages/ConsultaFornecedores.aspx");
            }

            finally
            {
                con.Close();
            }
        }

        protected void btnCep_Click(object sender, EventArgs e)
        {
            if (txtCepFor.Text == "")
            {
                // Acione o toast quando a página for carregada
                string script = "<script>showToast('Digite um CEP para pesquisar.');</script>";
                ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                txtCnpj.Focus();

            }
            else
            {
                WebCEP cep = new WebCEP(txtCepFor.Text);
                txtBaiFor.Text = cep.Bairro.ToString();
                txtCidFor.Text = cep.Cidade.ToString();
                txtEndFor.Text = cep.TipoLagradouro.ToString() + " " + cep.Lagradouro.ToString();
                txtEstFor.Text = cep.UF.ToString();
                txtNumero.Focus();
            }
        }
        public bool ValidarCNPJ(string cnpj)
        {
            cnpj = cnpj.Replace(".", "").Replace("/", "").Replace("-", "");

            if (cnpj.Length != 14 || !cnpj.All(char.IsDigit))
                return false;

            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCnpj = cnpj.Substring(0, 12);
            int soma = 0;

            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

            int resto = (soma % 11);
            resto = resto < 2 ? 0 : 11 - resto;

            tempCnpj += resto.ToString();
            soma = 0;

            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];

            resto = (soma % 11);
            resto = resto < 2 ? 0 : 11 - resto;

            return cnpj.EndsWith(resto.ToString());
        }
    }
}