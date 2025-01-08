using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls.WebParts;


namespace NewCapit
{
    public partial class Frm_AltClientes : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conexao"].ToString());
        string id;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //string id = Request.QueryString["codcli"];
                //if (!string.IsNullOrEmpty(id))
                //{
                //    // Use o ID para carregar os detalhes
                //    txtCodCli.Text = id;
                //}
                CarregaDados();
            }
        }

        public void CarregaDados()
        {
            if (HttpContext.Current.Request.QueryString["id"].ToString() != "")
            {
                id = HttpContext.Current.Request.QueryString["id"].ToString();
            }
            string sql = "select codcli, razcli, nomcli,tc1cli,tc2cli,endcli,cepcli,numero,complemento,baicli,cidcli,estcli,cnpj,inscestadual,programador,contato,email,codvw,unidade,codsapiens,longitude,latitude,ativo_inativo,usucad,CONVERT(varchar, dtccad, 103) as dtccad,usualt,CONVERT(varchar, dtcalt, 103) as dtcalt,tipo,raio,regiao,abertura,situacao,tipoempresa from tbclientes where id=" + id;
            SqlDataAdapter adpt = new SqlDataAdapter(sql, con);
            DataTable dt = new DataTable();
            con.Open();
            adpt.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                // Preenchendo os TextBoxes com valores do DataTable
                txtCodCli.Text = dt.Rows[0][0].ToString();
                txtRazCli.Text = dt.Rows[0][1].ToString();
                txtNomCli.Text = dt.Rows[0][2].ToString();
                txtTc1Cli.Text = dt.Rows[0][3].ToString();
                txtTc2Cli.Text = dt.Rows[0][4].ToString();
                txtEndCli.Text = dt.Rows[0][5].ToString();
                txtCepCli.Text = dt.Rows[0][6].ToString();
                txtNumero.Text = dt.Rows[0][7].ToString();
                txtComplemento.Text = dt.Rows[0][8].ToString();
                txtBaiCli.Text = dt.Rows[0][9].ToString();
                txtCidCli.Text = dt.Rows[0][10].ToString();
                txtEstCli.Text = dt.Rows[0][11].ToString();
                txtCnpj.Text = dt.Rows[0][12].ToString();
                txtInscEstadual.Text = dt.Rows[0][13].ToString();
                txtProgramador.Text = dt.Rows[0][14].ToString();
                txtContato.Text = dt.Rows[0][15].ToString();
                txtEmail.Text = dt.Rows[0][16].ToString();
                txtCodVw.Text = dt.Rows[0][17].ToString();
                txtUnidade.Text = dt.Rows[0][18].ToString();
                txtCodSapiens.Text = dt.Rows[0][19].ToString();
                longitude.Text = dt.Rows[0][20].ToString();
                latitude.Text = dt.Rows[0][21].ToString();
                ddlStatus.SelectedValue = dt.Rows[0][22].ToString();
                txtUsuCadastro.Text = dt.Rows[0][23].ToString();
                lblDtCadastro.Text = (dt.Rows[0][24].ToString());
                txtUsuAlteracao.Text = dt.Rows[0][25].ToString();
                lblDtAlteracao.Text = dt.Rows[0][26].ToString();
                cboTipo.SelectedValue = dt.Rows[0][27].ToString();
                txtRaio.Text = dt.Rows[0][28].ToString();
                cboRegiao.SelectedValue = dt.Rows[0][29].ToString();
                txtAbertura.Text = dt.Rows[0][30].ToString();
                txtSituacao.Text = dt.Rows[0][31].ToString();
                txtTipo.Text = dt.Rows[0][32].ToString();
                //txtConCli.Text = dt.Rows[0]["ConCli"].ToString();
                txtRamal.Text = dt.Rows[0][34].ToString();
            }
        }

        protected void btnAlterar_Click(object sender, EventArgs e)
        {

            if (HttpContext.Current.Request.QueryString["id"].ToString() != "")
            {
                id = HttpContext.Current.Request.QueryString["id"].ToString();
            }
            string sqlAtualizarCliente = "UPDATE tbclientes SET razcli = @razcli, concli = @concli, nomcli = @nomcli, tc1cli = @tc1cli, tc2cli = @tc2cli, endcli = @endcli, cepcli = @cepcli, baicli = @baicli, cidcli = @cidcli, estcli = @estcli, programador = @programador, contato = @contato, email = @email, codvw = @codvw, cnpj = @cnpj, inscestadual = @inscestadual, numero = @numero, complemento = @complemento, codsapiens = @codsapiens, longitude = @longitude, latitude = @latitude, ativo_inativo = @ativo_inativo, usualt = @usualt, dtcalt = @dtcalt, tipo = @tipo, unidade = @unidade, raio = @raio, regiao = @regiao, abertura = @abertura, situacao = @situacao, tipoempresa = @tipoempresa, ramal = @ramal WHERE id=" + id;
            //teste

            SqlCommand comando = new SqlCommand(sqlAtualizarCliente, con);
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
            comando.Parameters.AddWithValue("@ativo_inativo", ddlStatus.SelectedValue.ToUpper());
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


                CarregaDados();

                if (Session["UsuarioLogado"] != null)
                {
                    // Use o ID para carregar os detalhes
                    txtCodCli.Text = id;
                }
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
    }          
}