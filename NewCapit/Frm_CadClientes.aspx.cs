using System;
using System.Data.SqlClient;
using System.Web.Configuration;
using Domain;

namespace NewCapit
{
    public partial class Frm_CadClientes : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString());
        protected void Page_Load(object sender, EventArgs e)
        {

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

        protected void btnCliente_Click(object sender, EventArgs e)
        {
            if (txtCodCli.Text.Trim() == "")
            {
                Response.Write("<script>alert('Por favor, digite um código para cadastro');</script>");
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
                    Response.Write("<script>alert('Código digite já cadastrado no sistema. Por favor, digiate outro.');</script>");
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
            if (cnpj != null) {
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