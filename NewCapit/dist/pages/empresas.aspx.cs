using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using NPOI.XSSF.UserModel;
using static NPOI.HSSF.Util.HSSFColor;

namespace NewCapit.dist.pages
{
    public partial class empresas : System.Web.UI.Page
    {
        private EmpresaDAL empresaDAL = new EmpresaDAL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UsuarioLogado"] != null)
                {
                    string nomeUsuario = Session["UsuarioLogado"].ToString();
                    var lblUsuario = nomeUsuario;
                }
                else
                {
                    Response.Redirect("Login.aspx");
                }
                CarregarGrid();
                Novo();
            }

        }
        private bool EmEdicao
        {
            get
            {
                return ViewState["EmEdicao"] != null &&
                       (bool)ViewState["EmEdicao"];
            }
            set
            {
                ViewState["EmEdicao"] = value;
            }
        }
        private void Novo()
        {
            LimparTela();
            txtCodigo.Text = "";
            ddlStatus.SelectedValue = "ATIVO";
            txtAbertura.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtCodigo.Focus();
            EmEdicao = false;
        }
        private void LimparTela()
        {
            //foreach (Control c in Page.Form.Controls)
            //{
            //    LimparControles(c);
            //}
            LimparControles(this);

            ViewState["IdEmpresa"] = null;
            ViewState["Modo"] = "Novo";
            ddlStatus.SelectedValue = "ATIVO";
            txtAbertura.Text = DateTime.Now.ToString("dd/MM/yyyy");

            txtCNPJ.Focus();

            imgLogo.ImageUrl = "~/dist/img/logo_transnovag.png";
        }
        private void LimparControles(Control controle)
        {
            foreach (Control c in controle.Controls)
            {
                if (c is TextBox txt)
                {
                    txt.Text = string.Empty;
                }
                else if (c is DropDownList ddl)
                {
                    if (ddl.Items.Count > 0)
                        ddl.SelectedIndex = 0;
                }
                else if (c is CheckBox chk)
                {
                    chk.Checked = false;
                }
                else if (c is HiddenField hidden)
                {
                    hidden.Value = string.Empty;
                }

                if (c.HasControls())
                {
                    LimparControles(c);
                }
            }
        }
        private Domain.EmpresaDTO LerTela()
        {
            Domain.EmpresaDTO empresa = new Domain.EmpresaDTO();

            //int codigo;
            //string.TryParse(txtCodigo.Text, out codigo);

            empresa.CodigoEmpresa = txtCodigo.Text.Trim();            
            empresa.RazaoSocial = txtRazaoSocial.Text.Trim();
            empresa.NomeFantasia = txtNomeFantasia.Text.Trim();
            empresa.CNPJ = txtCNPJ.Text.Trim();
            empresa.InscricaoEstadual = txtInscricaoEstadual.Text.Trim();
            empresa.CodigoMunicipal = txtCodigoMunicipio.Text.Trim();
            empresa.Endereco = txtEndereco.Text.Trim();
            empresa.CEP = txtCEP.Text.Trim();
            empresa.Bairro = txtBairro.Text.Trim();
            empresa.Municipio = txtMunicipio.Text.Trim();
            empresa.UF = ddlUF.SelectedValue;
            empresa.UFNome = txtUFNome.Text.Trim();
            empresa.Telefone = txtTelefone.Text.Trim();
            empresa.Modal = ddlModal.SelectedValue;
            empresa.Numero = txtNumero.Text.Trim();
            empresa.RNTRC = txtRNTRC.Text.Trim();
            empresa.Status = ddlStatus.SelectedValue;

            DateTime abertura;
            if (DateTime.TryParse(txtAbertura.Text, out abertura))
                empresa.Abertura = abertura;

            return empresa;
        }
        private void PreencherTela(Domain.EmpresaDTO empresa)
        {           
            txtCodigo.Text = empresa.CodigoEmpresa;            
            txtRazaoSocial.Text = empresa.RazaoSocial;
            txtNomeFantasia.Text = empresa.NomeFantasia;
            txtCNPJ.Text = empresa.CNPJ;
            txtInscricaoEstadual.Text = empresa.InscricaoEstadual;
            txtCodigoMunicipio.Text = empresa.CodigoMunicipal;
            txtEndereco.Text = empresa.Endereco;
            txtCEP.Text = empresa.CEP;
            txtBairro.Text = empresa.Bairro;
            txtMunicipio.Text = empresa.Municipio;
            ddlUF.SelectedValue = empresa.UF;
            txtUFNome.Text = empresa.UFNome;
            txtTelefone.Text = empresa.Telefone;
            ddlModal.SelectedValue = empresa.Modal;
            txtNumero.Text = empresa.Numero;
            txtRNTRC.Text = empresa.RNTRC;
            ddlStatus.SelectedValue = empresa.Status;

            if (empresa.Abertura.HasValue)
            {
                txtAbertura.Text = empresa.Abertura.Value.ToString("yyyy-MM-dd");
            }
            else
            {
                txtAbertura.Text = "";
            }

            if (!string.IsNullOrWhiteSpace(empresa.Logo))
                imgLogo.ImageUrl = empresa.Logo;
        }
        private void CarregarGrid()
        {
            DAL.EmpresaDAL dal = new DAL.EmpresaDAL();

            rpEmpresas.DataSource = dal.Listar();

            rpEmpresas.DataBind();
        }
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if (!ValidarTela())
                return;

            Domain.EmpresaDTO empresa = LerTela();

            DAL.EmpresaDAL dal = new DAL.EmpresaDAL();

            int codigo = dal.Salvar(empresa);

            string logo = SalvarLogo(codigo);

            if (logo != empresa.Logo)
            {
                empresa.Codigo = codigo;
                empresa.Logo = logo;

                dal.AtualizarLogo(codigo, logo);
            }

            CarregarGrid();

            Novo();

            ScriptManager.RegisterStartupScript(
                this,
                GetType(),
                "ok",
                "MensagemSucesso('Empresa salva com sucesso.');",
                true);
        }
        protected void btnEditar_Click(object sender, EventArgs e)
        {
            LinkButton botao = (LinkButton)sender;

            int codigo = Convert.ToInt32(botao.CommandArgument);

            DAL.EmpresaDAL dal = new DAL.EmpresaDAL();

            Domain.EmpresaDTO empresa = dal.Obter(codigo);

            PreencherTela(empresa);

            EmEdicao = true;
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Novo();
        }
        private string SalvarLogo(int codigoEmpresa)
        {
            if (!fuLogo.HasFile)
                return imgLogo.ImageUrl;

            string extensao = Path.GetExtension(fuLogo.FileName).ToLower();

            if (extensao != ".jpg" &&
                extensao != ".jpeg" &&
                extensao != ".png")
            {
                throw new Exception("Formato de imagem inválido.");
            }

            string pasta = Server.MapPath("~/Uploads/");

            if (!Directory.Exists(pasta))
                Directory.CreateDirectory(pasta);

            string nomeArquivo = codigoEmpresa.ToString("000000") + extensao;

            string caminhoFisico = Path.Combine(pasta, nomeArquivo);

            fuLogo.SaveAs(caminhoFisico);

            return "~/Uploads/" + nomeArquivo;
        }
        private bool ValidarTela()
        {
            if (string.IsNullOrWhiteSpace(txtCodigo.Text))
            {
                ScriptManager.RegisterStartupScript(
                    this,
                    GetType(),
                    "msg",
                    "MensagemErro('Informe o código da empresa.');",
                    true);

                txtCodigo.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtRazaoSocial.Text))
            {
                ScriptManager.RegisterStartupScript(
                    this,
                    GetType(),
                    "msg",
                    "MensagemErro('Informe a razão social.');",
                    true);

                txtRazaoSocial.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtCNPJ.Text))
            {
                ScriptManager.RegisterStartupScript(
                    this,
                    GetType(),
                    "msg",
                    "MensagemErro('Informe o CNPJ.');",
                    true);

                txtCNPJ.Focus();
                return false;
            }

            DAL.EmpresaDAL dal = new DAL.EmpresaDAL();

            int codigo = 0;

            int.TryParse(txtCodigo.Text, out codigo);

            if (dal.ExisteCNPJ(txtCNPJ.Text.Trim(), codigo))
            {
                ScriptManager.RegisterStartupScript(
                    this,
                    GetType(),
                    "msg",
                    "MensagemErro('Já existe uma empresa cadastrada com este CNPJ.');",
                    true);

                txtCNPJ.Focus();

                return false;
            }

            return true;
        }
        protected void btnNovo_Click(object sender, EventArgs e)
        {


            // Limpa os campos para iniciar um novo cadastro
            Novo();
            //txtCNPJ.Text = "";
            //txtRazaoSocial.Text = "";
            //txtNomeFantasia.Text = "";
            //txtInscricaoEstadual.Text = "";
            //txtEmail.Text = "";
            //txtTelefone.Text = "";

            // Caso tenha DropDownLists
            // ddlTipoEmpresa.SelectedIndex = 0;

            // Caso tenha campos de endereço
            //txtCEP.Text = "";
            //txtEndereco.Text = "";
            //txtNumero.Text = "";
            //txtBairro.Text = "";
            //txtMunicipio.Text = "";
            //ddlUF.SelectedIndex = 0;

            //// Foco no primeiro campo
            //txtCNPJ.Focus();

            //// Controle de modo da tela
            //ViewState["Modo"] = "Novo";
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
            string cnpjSemMascara = RemoverMascaraCNPJ(txtCNPJ.Text);
            var cnpj = Empresa.ObterCnpj(cnpjSemMascara);
            if (cnpj != null)
            {
                var cep = RemoverMascaraCep(cnpj.cep);
                txtRazaoSocial.Text = cnpj.nome;
                txtTipo.Text = cnpj.tipo;
                txtDtAbertura.Text = cnpj.abertura;
                txtSituacao.Text = cnpj.situacao;
                if (cnpj.atividade_principal != null && cnpj.atividade_principal.Count > 0)
                {
                    txtAtividade_Principal.Text = cnpj.atividade_principal[0].text;
                }
                txtNomeFantasia.Text = cnpj.fantasia;
                txtCEP.Text = cep;
                txtEndereco.Text = cnpj.logradouro;
                txtNumero.Text = cnpj.numero;
                txtComplemento.Text = cnpj.complemento;
                txtBairro.Text = cnpj.bairro;
                txtMunicipio.Text = cnpj.municipio;
                ddlUF.SelectedItem.Text = cnpj.uf;
                txtUFNome.Text = NomeEstado(ddlUF.SelectedItem.Text);
            }


        }
        
        private string NomeEstado(string uf)
        {
            switch (uf.ToUpper())
            {
                case "AC": return "Acre";
                case "AL": return "Alagoas";
                case "AP": return "Amapá";
                case "AM": return "Amazonas";
                case "BA": return "Bahia";
                case "CE": return "Ceará";
                case "DF": return "Distrito Federal";
                case "ES": return "Espírito Santo";
                case "GO": return "Goiás";
                case "MA": return "Maranhão";
                case "MT": return "Mato Grosso";
                case "MS": return "Mato Grosso do Sul";
                case "MG": return "Minas Gerais";
                case "PA": return "Pará";
                case "PB": return "Paraíba";
                case "PR": return "Paraná";
                case "PE": return "Pernambuco";
                case "PI": return "Piauí";
                case "RJ": return "Rio de Janeiro";
                case "RN": return "Rio Grande do Norte";
                case "RS": return "Rio Grande do Sul";
                case "RO": return "Rondônia";
                case "RR": return "Roraima";
                case "SC": return "Santa Catarina";
                case "SP": return "São Paulo";
                case "SE": return "Sergipe";
                case "TO": return "Tocantins";
                default: return "";
            }
        }
        protected void txtCodigo_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCodigo.Text))
                return;
            if (txtCodigo.Text.Length > 0)
            {
                int codigo = Convert.ToInt32(txtCodigo.Text.Trim());
                if (empresaDAL.EmpresaExiste(codigo))
                {
                    ScriptManager.RegisterStartupScript(
                        this,
                        GetType(),
                        "alerta",
                        "alert('Código da empresa já cadastrado!');",
                        true
                    );
                    txtCodigo.Text = "";
                    txtCodigo.Focus();
                    return;
                }

                // Novo cadastro
                txtAbertura.Text = DateTime.Now.ToString("dd/MM/yyyy");

                if (ddlStatus.Items.FindByText("ATIVO") != null)
                {
                    ddlStatus.SelectedValue = "ATIVO";
                }

                txtCNPJ.Focus();
            }            
        }
        
    }
}