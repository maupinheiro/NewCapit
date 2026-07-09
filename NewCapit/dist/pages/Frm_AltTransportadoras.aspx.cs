using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using DocumentFormat.OpenXml.Wordprocessing;
using Domain;
using FluentEmail.Core;
using Microsoft.SqlServer.Server;
using RazorLight.Extensions;

namespace NewCapit.dist.pages
{
    public partial class Frm_AltTransportadoras : PaginaBase
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
                    txtUsuAltCadastro.Text = nomeUsuario;
                }
                else
                {
                    var lblUsuario = "<Usuário>";
                    Response.Redirect("Login.aspx");

                }
                VerificarBotoesPagina(btnAlterar: btnSalvar);
                PreencherComboFiliais();                
                DateTime dataHoraAtual = DateTime.Now;                
                txtAltDtUsu.Text = dataHoraAtual.ToString("dd/MM/yyyy HH:mm"); 
                CarregarBancos();
                CarregaDadosAgregado();
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

        protected void btnCnpj_Click(object sender, EventArgs e)
        {
            PesquisarCnpj();
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
                        string nomeUsuario = txtUsuAltCadastro.Text;
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
            // Obtém o ID da transportadora da QueryString           

            if (HttpContext.Current.Request.QueryString["id"].ToString() != "")
            {
                id = HttpContext.Current.Request.QueryString["id"].ToString();
            }
            string query = "UPDATE tbtransportadoras SET nomtra=@NomTra, contra=@ConTra, fantra=@FanTra, fone1=@Fone1, fone2=@Fone2, endtra=@EndTra, ceptra=@CepTra, baitra=@BaiTra, cidtra=@CidTra, uftra=@UfTra, ativa_inativa=@AtivaInativa, pessoa=@Pessoa, cnpj=@Cnpj, inscestadual=@InscEstadual, numero=@Numero, complemento=@Complemento, antt=@Antt, filial=@Filial, dtcalt=@DtCAlt, usualt=@UsuAlt, tipo=@Tipo, tac_etc_ctc=@tac_etc_ctc,banco=@banco,nome_banco=@nome_banco,agencia=@agencia,conta_corrente=@conta_corrente,email=@email,tipo_pagamento=@tipo_pagamento,forma_pagamento=@forma_pagamento,numero_cartao=@numero_cartao,cod_sapiens=@cod_sapiens,cod_rubi=@cod_rubi,gera_ciot=@gera_ciot,valor_ciot=@valor_ciot, cod_vw=@cod_vw WHERE ID = @id";

            // Atualiza informações do usuário logado e data de alteração
            string usuarioLogado = Session["UsuarioLogado"]?.ToString() ?? "Sistema";
            DateTime dataAlteracao = DateTime.Now;

            //string codigoRubi = txtCodRubi_Sapiens.Text;
            //if (codigoRubi.Contains("/"))
            //{
            //    codigoRubi = codigoRubi.Substring(0, codigoRubi.IndexOf("/"));
            //}
            //string codigoSapiens = txtCodRubi_Sapiens.Text.Split('/')[1];

            using (SqlConnection connection = new SqlConnection(con.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                //try
                //{
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
                    command.Parameters.AddWithValue("@AtivaInativa", ddlSituacao.SelectedItem.ToString());
                    command.Parameters.AddWithValue("@Pessoa", cboPessoa.SelectedItem.ToString());
                    command.Parameters.AddWithValue("@Cnpj", txtCpf_Cnpj.Text);
                    command.Parameters.AddWithValue("@InscEstadual", txtRg.Text);
                    command.Parameters.AddWithValue("@Numero", txtNumero.Text);
                    command.Parameters.AddWithValue("@Complemento", txtComplemento.Text);
                    command.Parameters.AddWithValue("@DtCAlt", dataAlteracao.ToString("dd/MM/yyyy HH:mm"));
                    command.Parameters.AddWithValue("@UsuAlt", usuarioLogado);
                    command.Parameters.AddWithValue("@Antt", txtAntt.Text);
                    command.Parameters.AddWithValue("@Filial", cbFiliais.SelectedItem.ToString());
                    command.Parameters.AddWithValue("@Tipo", ddlTipo.SelectedItem.ToString());
                    command.Parameters.AddWithValue("@tac_etc_ctc", tipoTAC.SelectedValue);
                    command.Parameters.AddWithValue("@banco", txtCodigoBanco.Text.Trim());
                    command.Parameters.AddWithValue("@nome_banco", ddlBanco.SelectedItem.Text.Trim());
                    command.Parameters.AddWithValue("@agencia", txtAgencia.Text.Trim().ToUpper());
                    command.Parameters.AddWithValue("@conta_corrente", txtConta.Text.Trim().ToUpper());
                    command.Parameters.AddWithValue("@email", txtEmail.Text.Trim());
                    command.Parameters.AddWithValue("@tipo_pagamento", ddlTipoPagamento.SelectedValue.ToString());
                    command.Parameters.AddWithValue("@forma_pagamento", ddlFormaPagamento.SelectedValue.ToString());
                    command.Parameters.AddWithValue("@numero_cartao", txtCartao.Text.Trim());
                    command.Parameters.AddWithValue("@cod_rubi", txtCod_Rubi.Text.Trim());
                    command.Parameters.AddWithValue("@cod_sapiens", txtCod_Sapiens.Text.Trim());
                    command.Parameters.AddWithValue("@cod_vw", txtCodVW.Text.Trim().ToUpper());
                    command.Parameters.AddWithValue("@gera_ciot", ddlGeraCIOT.SelectedValue.ToString());
                decimal valorCIOT;
                if (ddlTipo.SelectedItem.Text == "EMPRESA")
                {
                    command.Parameters.AddWithValue("@valor_ciot", "0.00");
                }
                else
                {
                    //decimal valorCIOT;
                    if (decimal.TryParse(txtValorCIOT.Text,
                                             NumberStyles.Any,
                                             new CultureInfo("pt-BR"),
                                             out valorCIOT))
                    {
                        command.Parameters.Add("@valor_ciot", SqlDbType.Decimal).Value = valorCIOT;
                    }
                    else
                    {
                        // Valor inválido
                        ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", "alert('Valor do CIOT inválido. Verifique, por favor!');", true);
                        txtValorCIOT.Focus();
                    }
                }

                command.Parameters.AddWithValue("@ID", id);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        string mensagem = $"Olá, {usuarioLogado}! Registro atualizado com sucesso.";
                        ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", $"alert('{HttpUtility.JavaScriptStringEncode(mensagem)}');", true);
                        Response.Redirect("/dist/pages/Consulta_Agregados.aspx");
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", "alert('Nenhum registro foi encontrado para atualizar.');", true);
                    }
                //}
                //catch (Exception ex)
                //{
                //    string mensagemErro = $"Erro ao atualizar: {HttpUtility.JavaScriptStringEncode(ex.Message)}";
                //    ClientScript.RegisterStartupScript(this.GetType(), "Erro", $"alert('{mensagemErro}');", true);
                //}
            }
        }
        protected void txtCodVW_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCodVW.Text))
                return;

            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                conn.Open();

                string sql = @"SELECT TOP 1 *
                       FROM tbtransportadoras
                       WHERE cod_vw = @cod_vw";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@cod_vw", txtCodVW.Text.Trim());

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            // Encontrou                            
                            MostrarMsg("Código VW já cadastrado para: " + dr["codtra"].ToString() + " - " + dr["fantra"].ToString(), "warning");
                            txtCodVW.Focus();
                            return;
                        }
                    }
                }
            }
        }
        protected void MostrarMsg(string mensagem, string tipo = "warning")
        {
            divMsg.Attributes["class"] = "alert alert-" + tipo + " alert-dismissible fade show mt-3";
            lblMsgGeral.InnerText = mensagem;
            divMsg.Style["display"] = "block";

            string script = @"setTimeout(function() {
                        var div = document.getElementById('divMsg');
                        if (div) div.style.display = 'none';
                      }, 5000);";

            ScriptManager.RegisterStartupScript(this, GetType(), "EscondeMsg", script, true);
        }
        public void CarregaDadosAgregado()
        {
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString());
            if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["id"]))
            {
                id = HttpContext.Current.Request.QueryString["id"].ToString();
            }
            string sql = "select * from tbtransportadoras where id='" + id + "'";
            SqlDataAdapter apt = new SqlDataAdapter(sql, con);
            DataTable dt = new DataTable();
            con.Open();
            apt.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                txtCodTra.Text = dt.Rows[0][1].ToString();
                txtDtCadastro.Text = DateTime.Parse(dt.Rows[0][2].ToString()).ToString("dd/MM/yyyy");
                txtRazCli.Text = dt.Rows[0][3].ToString();
                txtContato.Text = dt.Rows[0][4].ToString();
                txtFantasia.Text = dt.Rows[0][5].ToString();
                txtFixo.Text = dt.Rows[0][6].ToString();
                txtCelular.Text = dt.Rows[0][7].ToString();
                txtEndCli.Text = dt.Rows[0][8].ToString();
                txtCepCli.Text = dt.Rows[0][9].ToString();
                txtBaiCli.Text = dt.Rows[0][10].ToString();
                txtCidCli.Text = dt.Rows[0][11].ToString();
                txtEstCli.Text = dt.Rows[0][12].ToString();
                ddlSituacao.Items.Insert(0, dt.Rows[0][13].ToString());
                cboPessoa.Items.Insert(0, dt.Rows[0][14].ToString());
                txtCpf_Cnpj.Text = dt.Rows[0][15].ToString();
                txtRg.Text = dt.Rows[0][16].ToString();
                txtNumero.Text = dt.Rows[0][17].ToString();
                txtComplemento.Text = dt.Rows[0][18].ToString();
                txtDtCad.Text = dt.Rows[0][19].ToString();
                txtUsuCad.Text = dt.Rows[0][20].ToString();
                txtAntt.Text = dt.Rows[0][23].ToString();
                txtAltDtUsu.Text = dt.Rows[0][21].ToString();
                txtUsuAltCadastro.Text = dt.Rows[0][22].ToString();
                cbFiliais.Items.Insert(0, dt.Rows[0][24].ToString());
                ddlTipo.Items.Insert(0, dt.Rows[0][25].ToString());
                if (dt.Rows[0][29].ToString() == "1")
                {
                    tipoTAC.SelectedValue = "1";                   
                }
                else if (dt.Rows[0][29].ToString() == "2")
                {
                    tipoTAC.SelectedValue = "2";                   
                }
                else if (dt.Rows[0][29].ToString() == "3")
                {
                    tipoTAC.SelectedValue = "3";                    
                }
                else if (dt.Rows[0][29].ToString() == "4")
                {
                    tipoTAC.SelectedValue = "4";                   
                }
                else
                {
                    tipoTAC.SelectedItem.Text = "Selecione...";
                }

                txtCodigoBanco.Text = dt.Rows[0][30].ToString();
                
                if (dt.Rows[0][31].ToString() != string.Empty)
                {
                    ddlBanco.Items.Insert(0, dt.Rows[0][31].ToString());
                }

                txtAgencia.Text = dt.Rows[0][32].ToString();
                txtConta.Text = dt.Rows[0][33].ToString();
                txtEmail.Text = dt.Rows[0][34].ToString();
                if (dt.Rows[0][35].ToString() == "1")
                {
                    ddlTipoPagamento.SelectedValue = "1";
                }
                else if (dt.Rows[0][35].ToString() == "2")
                {
                    ddlTipoPagamento.SelectedValue = "2";
                }
                else if (dt.Rows[0][35].ToString() == "3")
                {
                    ddlTipoPagamento.SelectedValue ="3";
                }
                else if (dt.Rows[0][35].ToString() == "4")
                {
                    ddlTipoPagamento.SelectedValue = "4";
                }
                else
                {
                    ddlTipoPagamento.SelectedItem.Text = "Selecione...";
                }

                if (dt.Rows[0][36].ToString() == "1")
                {
                    ddlFormaPagamento.SelectedValue = "1";
                }
                else if (dt.Rows[0][36].ToString() == "2")
                {
                    ddlFormaPagamento.SelectedValue ="2";
                }
                else if (dt.Rows[0][36].ToString() == "3")
                {
                    ddlFormaPagamento.SelectedValue ="3";
                }
                else
                {
                    ddlFormaPagamento.SelectedItem.Text = "Selecione...";
                }

                txtCartao.Text = dt.Rows[0][37].ToString();
                txtCod_Sapiens.Text = dt.Rows[0][38].ToString();
                txtCod_Rubi.Text = dt.Rows[0][39].ToString();

                if (dt.Rows[0][40].ToString() == "1")
                {
                    ddlGeraCIOT.SelectedValue = "1";
                }
                else if (dt.Rows[0][40].ToString() == "2")
                {
                    ddlGeraCIOT.SelectedValue ="2";
                }
                else
                {
                    ddlGeraCIOT.SelectedItem.Text = "Selecione...";
                }

                decimal valorCIOT;
                if (decimal.TryParse(dt.Rows[0][41].ToString(), out valorCIOT))
                {
                    txtValorCIOT.Text = valorCIOT.ToString("N2", new CultureInfo("pt-BR"));
                }
                txtCodVW.Text = dt.Rows[0][42].ToString();
            }

        }        
        [System.Web.Services.WebMethod]
        public static string BuscarBancos(string codigo)
        {
            string descricao = null;

            string conn = ConfigurationManager
                .ConnectionStrings["conexao"]
                .ConnectionString;

            using (SqlConnection con = new SqlConnection(conn))
            {
                string sql = @"SELECT nome
                       FROM tbbancos
                       WHERE codigo = @codigo";

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@codigo", codigo);

                con.Open();

                object result = cmd.ExecuteScalar();

                if (result != null)
                    descricao = result.ToString();
                

            }

            return descricao;
        }
        private void CarregarBancos()
        {
            string conn = ConfigurationManager
                .ConnectionStrings["conexao"]
                .ConnectionString;

            using (SqlConnection con = new SqlConnection(conn))
            {
                string sql = @"SELECT codigo, nome
                       FROM tbbancos
                       ORDER BY nome";

                SqlDataAdapter da = new SqlDataAdapter(sql, con);

                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlBanco.DataSource = dt;
                ddlBanco.DataTextField = "nome";
                ddlBanco.DataValueField = "codigo";
                ddlBanco.DataBind();

                ddlBanco.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Selecione...", ""));
            }
        }
        protected void txtCodigoBanco_TextChanged(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(
        WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                string sql = "SELECT codigo, nome FROM tbbancos WHERE codigo = @codigo";

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@codigo", txtCodigoBanco.Text.Trim());

                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    string codigo = dr["codigo"].ToString();

                    System.Web.UI.WebControls.ListItem item = ddlBanco.Items.FindByValue(codigo);

                    if (item != null)
                    {
                        ddlBanco.ClearSelection();
                        item.Selected = true;
                    }
                }
                else
                {
                    //ddlBanco.SelectedIndex = 0;
                    ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", "alert('Banco, não encontrado no sistema. Verifique, por favor!');", true);
                    txtCodigoBanco.Focus();
                }
            }
        }
        protected void ddlBanco_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtCodigoBanco.Text = ddlBanco.SelectedValue.ToString();
        }
    }
}