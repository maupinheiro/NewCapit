﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
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
    public partial class Frm_AltTransportadoras : System.Web.UI.Page
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
                }


                PreencherComboFiliais();

                DateTime dataHoraAtual = DateTime.Now;                
                txtAltDtUsu.Text = dataHoraAtual.ToString("dd/MM/yyyy HH:mm");               
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
            //if (!int.TryParse(HttpContext.Current.Request.QueryString["id"], out int id))
           // {
           //     ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", "alert('ID /inválido ou não fornecido.');", true);
           //     return;
           // }

            if (HttpContext.Current.Request.QueryString["id"].ToString() != "")
            {
                id = HttpContext.Current.Request.QueryString["id"].ToString();
            }
            string query = "UPDATE tbtransportadoras SET nomtra=@NomTra, contra=@ConTra, fantra=@FanTra, fone1=@Fone1, fone2=@Fone2, endtra=@EndTra, ceptra=@CepTra, baitra=@BaiTra, cidtra=@CidTra, uftra=@UfTra, ativa_inativa=@AtivaInativa, pessoa=@Pessoa, cnpj=@Cnpj, inscestadual=@InscEstadual, numero=@Numero, complemento=@Complemento, antt=@Antt, filial=@Filial, dtcalt=@DtCAlt, usualt=@UsuAlt, tipo=@Tipo WHERE ID = @id";

            // Atualiza informações do usuário logado e data de alteração
            string usuarioLogado = Session["UsuarioLogado"]?.ToString() ?? "Usuário não identificado";
            DateTime dataAlteracao = DateTime.Now;   

            using (SqlConnection connection = new SqlConnection(con.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                try
                {
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
                }
                catch (Exception ex)
                {
                    string mensagemErro = $"Erro ao atualizar: {HttpUtility.JavaScriptStringEncode(ex.Message)}";
                    ClientScript.RegisterStartupScript(this.GetType(), "Erro", $"alert('{mensagemErro}');", true);
                }
            }
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
            }
            else
            {
                //lblMsg.Visible = true;
                //pnlMot.Visible = false;
            }
        }

        public void CarregaDadosAgregado3()
        {
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString());
            if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["id"]))
            {
                id = HttpContext.Current.Request.QueryString["id"].ToString();
            }
            string sql = "SELECT * FROM tbtransportadoras WHERE ID='" + id + "'";
            SqlCommand command = new SqlCommand(sql, con);

            try
            {
                con.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Popula os campos da tela com os dados retornados do banco
                        txtCodTra.Text = reader["codtra"].ToString();
                        txtDtCadastro.Text = reader["dtcad"].ToString();
                        txtRazCli.Text = reader["nomtra"].ToString();
                        txtContato.Text = reader["contra"].ToString();
                        txtFantasia.Text = reader["fantra"].ToString();
                        txtFixo.Text = reader["fone1"].ToString();
                        txtCelular.Text = reader["fone2"].ToString();
                        txtEndCli.Text = reader["endtra"].ToString();
                        txtCepCli.Text = reader["ceptra"].ToString();
                        txtBaiCli.Text = reader["baitra"].ToString();
                        txtCidCli.Text = reader["cidtra"].ToString();
                        txtEstCli.Text = reader["uftra"].ToString();
                        ddlSituacao.SelectedValue = reader["ativa_inativa"].ToString();
                        cboPessoa.SelectedValue = reader["pessoa"].ToString();
                        txtCpf_Cnpj.Text = reader["cnpj"].ToString();
                        txtRg.Text = reader["inscestadual"].ToString();
                        txtNumero.Text = reader["numero"].ToString();
                        txtComplemento.Text = reader["complemento"].ToString();
                        txtDtCadastro.Text = reader["dtccad"].ToString();
                        txtUsuCad.Text = reader["usucad"].ToString();
                        txtAntt.Text = reader["antt"].ToString();
                        string filial = reader["filial"].ToString();
                        ddlTipo.SelectedValue = reader["tipo"].ToString();
                        System.Diagnostics.Debug.WriteLine("Filial lida do banco: " + filial);
                        cbFiliais.Items.Insert(0, filial);

                    }
                    else
                    {
                        // Exibe mensagem caso o registro não seja encontrado
                        string mensagem = "Nenhum registro foi encontrado para o ID fornecido.";
                        string script = $"alert('{HttpUtility.JavaScriptStringEncode(mensagem)}');";
                        ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", script, true);
                    }
                }
                con.Close();
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
                Response.Redirect("ConsultaAgregados.aspx");
            }
            finally
            {
                con.Close();
            }



        }
    }
}