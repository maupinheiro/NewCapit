using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NewCapit.dist.pages
{
    public partial class Frm_AltFornecedor : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString());
        string id;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UsuarioLogado"] != null)
                {
                    string nomeUsuario = Session["UsuarioLogado"].ToString();
                    var lblUsuario = nomeUsuario;

                    // txtUsuCadastro.Text = nomeUsuario;

                }
                else
                {
                    var lblUsuario = "<Usuário>";
                    //txtUsuCadastro.Text = lblUsuario;
                }
                CarregaDadosFornecedor();
            }
            //DateTime dataHoraAtual = DateTime.Now;
            //lblDtCadastro.Text = dataHoraAtual.ToString("dd/MM/yyyy HH:mm");

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


        public void CarregaDadosFornecedor()
        {
            if (HttpContext.Current.Request.QueryString["id"].ToString() != "")
            {
                id = HttpContext.Current.Request.QueryString["id"].ToString();
            }
            string sql = "SELECT codfor, razaosocial, fantasia, cnpj, inscestadual, inscccm, tipoempresa, abertura, situacaoreceita, tipofornecedor, contato, fonefixo, fonecelular, email, site, cep, endereco, numero, complemento, bairro, cidade, estado, pais, data_cadastro, usuario_cadastro, data_alteracao, usuario_alteracao, dtcadastro, status FROM tbfornecedores WHERE id = " + id;

            SqlDataAdapter adpt = new SqlDataAdapter(sql, con);
            DataTable dt = new DataTable();
            con.Open();
            adpt.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                // Preenchendo os TextBoxes com valores do DataTable
                if (dt.Rows[0][0].ToString() != string.Empty)
                {
                    txtCodFor.Text = dt.Rows[0][0].ToString();
                    txtCnpj.Text = dt.Rows[0][3].ToString();
                    txtRazFor.Text = dt.Rows[0][1].ToString();
                    txtNomFor.Text = dt.Rows[0][2].ToString();
                    txtInscEstadual.Text = dt.Rows[0][4].ToString();
                    txtInscCCM.Text = dt.Rows[0][5].ToString();
                    txtTipo.Text = dt.Rows[0][6].ToString();
                    if (dt.Rows[0][7].ToString() != string.Empty)
                    {
                        // DateTime abertura = Convert.ToDateTime(dt.Rows[0][7]);
                        txtAbertura.Text = dt.Rows[0][7].ToString();
                    }
                    txtSituacao.Text = dt.Rows[0][8].ToString();
                    ddlTipoFornecedor.SelectedItem.Text = dt.Rows[0][9].ToString();
                    txtConFor.Text = dt.Rows[0][10].ToString();
                    txtTc1For.Text = dt.Rows[0][11].ToString();
                    txtTc2For.Text = dt.Rows[0][12].ToString();
                    txtEmail.Text = dt.Rows[0][13].ToString();
                    txtSite.Text = dt.Rows[0][14].ToString();
                    txtCepFor.Text = dt.Rows[0][15].ToString();
                    txtEndFor.Text = dt.Rows[0][16].ToString();
                    txtNumero.Text = dt.Rows[0][17].ToString();
                    txtComplemento.Text = dt.Rows[0][18].ToString();
                    txtBaiFor.Text = dt.Rows[0][19].ToString();
                    txtCidFor.Text = dt.Rows[0][20].ToString();
                    txtEstFor.Text = dt.Rows[0][21].ToString();
                    ddlPaises.Items.Insert(0, new ListItem(dt.Rows[0][22].ToString(), "0"));
                    lblDtCadastro.Text = dt.Rows[0][23].ToString();
                    txtUsuCadastro.Text = dt.Rows[0][24].ToString();
                    txtUltimaAtualizacao.Text = dt.Rows[0][25].ToString();
                    txtAtualizadoPor.Text = dt.Rows[0][26].ToString();
                    txtDtCadastro.Text = dt.Rows[0][27].ToString();
                    ddlStatus.SelectedItem.Text = dt.Rows[0][28].ToString();
                }


            }
        }


        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if (HttpContext.Current.Request.QueryString["id"].ToString() != "")
            {
                id = HttpContext.Current.Request.QueryString["id"].ToString();
            }
            
            string nomeUsuario = Session["UsuarioLogado"].ToString();
            
            using (SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                string query = "UPDATE tbfornecedores SET razaosocial=@razaosocial, fantasia=@fantasia, cnpj=@cnpj, inscestadual=@inscestadual, inscccm=@inscccm, tipoempresa=@tipoempresa, abertura=@abertura, situacaoreceita=@situacaoreceita, tipofornecedor=@tipofornecedor, contato=@contato, fonefixo=@fonefixo, fonecelular=@fonecelular, email=@email, site=@site, cep=@cep, endereco=@endereco, numero=@numero, complemento=@complemento, bairro=@bairro, cidade=@cidade, estado=@estado, pais=@pais, data_alteracao=@data_alteracao, usuario_alteracao=@usuario_alteracao, dtcadastro=@dtcadastro, status=@status WHERE id = @Id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.Add("@razaosocial", SqlDbType.VarChar).Value = txtRazFor.Text.Trim().ToUpper();
                cmd.Parameters.Add("@fantasia", SqlDbType.VarChar).Value = txtNomFor.Text.Trim().ToUpper();
                cmd.Parameters.Add("@cnpj", SqlDbType.VarChar).Value = txtCnpj.Text.Trim().ToUpper();
                cmd.Parameters.Add("@inscestadual", SqlDbType.VarChar).Value = txtInscEstadual.Text.Trim().ToUpper();
                cmd.Parameters.Add("@inscccm", SqlDbType.VarChar).Value = txtInscCCM.Text.Trim().ToUpper();
                cmd.Parameters.Add("@tipoempresa", SqlDbType.VarChar).Value = txtTipo.Text.Trim().ToUpper();
                cmd.Parameters.Add("@abertura", SqlDbType.VarChar).Value = txtAbertura.Text;
                cmd.Parameters.Add("@situacaoreceita", SqlDbType.VarChar).Value = txtSituacao.Text.ToUpper();
                cmd.Parameters.Add("@tipofornecedor", SqlDbType.VarChar).Value = ddlTipoFornecedor.SelectedItem.Text;
                cmd.Parameters.Add("@contato", SqlDbType.VarChar).Value = txtConFor.Text.Trim().ToUpper();
                cmd.Parameters.Add("@fonefixo", SqlDbType.VarChar).Value = txtTc1For.Text.Trim().ToUpper();
                cmd.Parameters.Add("@fonecelular", SqlDbType.VarChar).Value = txtTc2For.Text.Trim().ToUpper();
                cmd.Parameters.Add("@email", SqlDbType.VarChar).Value = txtEmail.Text.Trim().ToLower();
                cmd.Parameters.Add("@site", SqlDbType.VarChar).Value = txtSite.Text.Trim().ToLower();
                cmd.Parameters.Add("@cep", SqlDbType.VarChar).Value = txtCepFor.Text.Trim().ToLower();
                cmd.Parameters.Add("@endereco", SqlDbType.VarChar).Value = txtEndFor.Text.Trim().ToUpper();
                cmd.Parameters.Add("@numero", SqlDbType.VarChar).Value = txtNumero.Text.Trim().ToUpper();
                cmd.Parameters.Add("@complemento", SqlDbType.VarChar).Value = txtComplemento.Text.Trim().ToUpper();
                cmd.Parameters.Add("@bairro", SqlDbType.VarChar).Value = txtBaiFor.Text.Trim().ToUpper();
                cmd.Parameters.Add("@cidade", SqlDbType.VarChar).Value = txtCidFor.Text.Trim().ToUpper();
                cmd.Parameters.Add("@estado", SqlDbType.VarChar).Value = txtEstFor.Text.Trim().ToUpper();
                cmd.Parameters.Add("@pais", SqlDbType.VarChar).Value = ddlPaises.SelectedItem.Text;
                cmd.Parameters.Add("@data_alteracao", SqlDbType.VarChar).Value = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                cmd.Parameters.Add("@usuario_alteracao", SqlDbType.VarChar).Value = nomeUsuario;
                cmd.Parameters.Add("@dtcadastro", SqlDbType.VarChar).Value = txtDtCadastro.Text;
                cmd.Parameters.Add("@status", SqlDbType.VarChar).Value = ddlStatus.SelectedItem.Text;
                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                try
                {
                    con.Open();
                    int linhasAfetadas = cmd.ExecuteNonQuery();

                    if (linhasAfetadas > 0)
                    {
                        // Acione o toast quando a página for carregada
                        string script = "<script>showToast('Atualização realizada com sucesso.');</script>";
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);                        
                    }
                    else
                    {
                        // Acione o toast quando a página for carregada
                        string script = "<script>showToast('Id não encontrado para atualização.');</script>";
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                       
                    }
                }
                catch (Exception ex)
                {
                    //lblMensagem.Text = "Erro: " + ex.Message;
                    // Acione o toast quando a página for carregada
                    string script = "<script>showToast('Erro ao atualizar fornecedor.');</script>";
                    ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);

                }
            }
        }
    }
}