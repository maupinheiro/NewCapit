using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Security.Policy;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using Domain;
using FluentEmail.Core;
using NewCapit.DynamicData.FieldTemplates;
using NPOI.SS.Formula.Functions;
using NPOI.XSSF.UserModel;
using static NPOI.HSSF.Util.HSSFColor;

namespace NewCapit.dist.pages
{
    public partial class empresas : System.Web.UI.Page
    {
        private EmpresaDAL empresaDAL = new EmpresaDAL();
        SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString());
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
            txtCadastro.Text = DateTime.Now.ToString("dd/MM/yyyy");
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
            txtCadastro.Text = DateTime.Now.ToString("dd/MM/yyyy");

            txtCNPJ.Focus();

            imgLogo.ImageUrl = "~/dist/img/logo_em_branco.png";
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
            string usuario = Session["UsuarioLogado"]?.ToString();
            empresa.Codigo = Convert.ToInt32(txtCodigo.Text.Trim());
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
            empresa.Complemento = txtComplemento.Text.Trim();
            empresa.RNTRC = txtRNTRC.Text.Trim();
            empresa.Status = ddlStatus.SelectedValue;
            empresa.Tipo = txtTipo.Text.Trim();
            empresa.Email = txtEmail.Text.Trim();
            empresa.Site = txtSite.Text.Trim();
            empresa.AtividadePrincipal = txtAtividade_Principal.Text.Trim();            
            empresa.UsuarioCadastro = Session["UsuarioLogado"]?.ToString();
            empresa.UsuarioAlteracao = Session["UsuarioLogado"]?.ToString();
            empresa.Situacao = txtSituacao.Text.Trim();
            DateTime cadastro;
            if (DateTime.TryParse(txtCadastro.Text, out cadastro))
                empresa.Cadastro = cadastro;
            DateTime abertura;
            if (DateTime.TryParse(txtDtAbertura.Text, out abertura))
                empresa.Abertura = abertura;
            DateTime dataCadastro;
            if (DateTime.TryParse(txtDataCadastro.Text, out dataCadastro))
                empresa.DataCadastro = dataCadastro;
            DateTime dataAlteracao;
            if (DateTime.TryParse(txtDataAlteracao.Text, out dataAlteracao))
                empresa.DataAlteracao = dataAlteracao;

            return empresa;
        }
        private void PreencherTela(Domain.EmpresaDTO empresa)
        {
            txtCodigo.Text = Convert.ToString(empresa.Codigo);
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
            txtComplemento.Text = empresa.Complemento;
            txtRNTRC.Text = empresa.RNTRC;
            ddlStatus.SelectedValue = empresa.Status;
            txtTipo.Text = empresa.Tipo;
            txtEmail.Text = empresa.Email;
            txtSite.Text = empresa.Site;
            txtAtividade_Principal.Text = empresa.AtividadePrincipal;
            txtSituacao.Text = empresa.Situacao;
            txtTipo.Text = empresa.Tipo;
            txtUsuarioCadastro.Text = empresa.UsuarioCadastro;
            txtUsuarioAlteracao.Text = empresa.UsuarioAlteracao;

            
            if (empresa.Abertura.HasValue)
            {
                txtDtAbertura.Text = empresa.Abertura.Value.ToString("dd/MM/yyyy");
            }
            else
            {
                txtDtAbertura.Text = "";
            }


            if (empresa.Cadastro.HasValue)
            {
                txtCadastro.Text = empresa.Cadastro.Value.ToString("dd/MM/yyyy");
            }
            else
            {
                txtCadastro.Text = "";
            }

            if (empresa.DataCadastro.HasValue)
            {
                txtDataCadastro.Text = empresa.DataCadastro.Value.ToString("dd/MM/yyyy HH:mm");
            }
            else
            {
                txtDataCadastro.Text = "";
            }
            if (empresa.DataAlteracao.HasValue)
            {
                txtDataAlteracao.Text = empresa.DataAlteracao.Value.ToString("dd/MM/yyyy HH:mm");
            }
            else
            {
                txtDataAlteracao.Text = "";
            }

            if (!string.IsNullOrWhiteSpace(empresa.Logo))
                imgLogo.ImageUrl = empresa.Logo;
        }
        private void CarregarGrid()
        {
            DAL.EmpresaDAL dal = new DAL.EmpresaDAL();

            gvEmpresas.DataSource = dal.Listar();

            gvEmpresas.DataBind();
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

            string pasta = Server.MapPath("~/dist/img/");

            if (!Directory.Exists(pasta))
                Directory.CreateDirectory(pasta);

            string nomeArquivo = codigoEmpresa.ToString("000000") + extensao;

            string caminhoFisico = Path.Combine(pasta, nomeArquivo);

            fuLogo.SaveAs(caminhoFisico);

            return "~/dist/img/" + nomeArquivo;
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
            LimparTela();
            txtCodigo.Text = "";
            ddlStatus.SelectedValue = "ATIVO";
            txtCadastro.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtCodigo.Focus();
            EmEdicao = false;   
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
                txtCadastro.Text = DateTime.Now.ToString("dd/MM/yyyy");

                if (ddlStatus.Items.FindByText("ATIVO") != null)
                {
                    ddlStatus.SelectedValue = "ATIVO";
                }

                txtCNPJ.Focus();
            }            
        }
        public int Salvar(EmpresaDTO empresa)
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                conn.Open();

                // Verifica se já existe
                string sqlExiste = "SELECT COUNT(*) FROM tbempresa WHERE codigo_empresa = @codigo";

                SqlCommand cmdExiste = new SqlCommand(sqlExiste, conn);
                cmdExiste.Parameters.AddWithValue("@codigo", empresa.Codigo);

                bool existe = Convert.ToInt32(cmdExiste.ExecuteScalar()) > 0;

                if (existe)
                {
                    AtualizarEmpresa(conn, empresa);
                    return empresa.Codigo;
                }
                else
                {
                    return InserirEmpresa(conn, empresa);
                }
            }
        }
        private int InserirEmpresa(SqlConnection conn, EmpresaDTO empresa)
        {
            string sql = @"
            INSERT INTO tbempresa
            (
                codigo_empresa,
                razao_social,
                nome_fantasia,
                cnpj,
                inscricao_estadual,
                codigo_municipal,
                endereco,
                cep,
                bairro,
                municipio,
                uf,
                nome_uf,
                telefone,
                modal,
                numero,
                complemento,
                rntrc,                
                logo,
                abertura,
                tipo,
                situacao,
                status,
                cadastro,
                atividade_principal,
                email,
                site,
                data_cadastro,
                usuario_cadastro                
            )
            VALUES
            (
                @codigo,
                @razao_social,
                @nome_fantasia,
                @cnpj,
                @inscricao_estadual,
                @codigo_municipal,
                @endereco,
                @cep,
                @bairro,
                @municipio,
                @uf,
                @nome_uf,
                @telefone,
                @modal,
                @numero,
                @complemento,
                @rntrc,               
                @logo,
                @abertura,
                @tipo,
                @situacao,
                @status,
                @cadastro,
                @atividade_principal,
                @email,
                @site,
                @data_cadastro,
                @usuario_cadastro
            );
            SELECT @codigo;";
            string usuario = HttpContext.Current.Session["UsuarioLogado"]?.ToString() ?? "";
            
            SqlCommand cmd = new SqlCommand(sql, conn);

            PreencherParametros(cmd, empresa);
            cmd.Parameters.AddWithValue("@data_cadastro", DateTime.Now);
            cmd.Parameters.AddWithValue("@usuario_cadastro", usuario);

            return Convert.ToInt32(cmd.ExecuteScalar());
        }
        private void AtualizarEmpresa(SqlConnection conn, EmpresaDTO empresa)
        {
            string sql = @"
            UPDATE tbempresa
            SET
               razao_social        = @razao_social,
               nome_fantasia       = @nome_fantasia,
               cnpj                = @cnpj,
               inscricao_estadual  = @inscricao_estadual,
               codigo_municipal    = @codigo_municipal,
               endereco            = @endereco,
               cep                 = @cep,
               bairro              = @bairro,
               municipio           = @municipio,
               uf                  = @uf,
               uf_nome             = @nome_uf,
               telefone            = @telefone,
               modal               = @modal,
               numero              = @numero,
               complemento         = @complemento,
               rntrc               = @rntrc,               
               logo                = @logo,
               abertura            = @abertura,
               tipo                = @tipo,
               situacao            = @situacao,
               status              = @status,               
               atividade_principal = @atividade_principal,
               email               = @email,
               site                = @site,                
               data_alteracao      = @data_alteracao,
               usuario_alteracao   = @usuario_alteracao
            WHERE codigo_empresa   = @codigo";

            SqlCommand cmd = new SqlCommand(sql, conn);

            PreencherParametros(cmd, empresa);
            string usuario = HttpContext.Current.Session["UsuarioLogado"]?.ToString() ?? "";
            cmd.Parameters.AddWithValue("@data_alteracao", DateTime.Now);
            cmd.Parameters.AddWithValue("@usuario_alteracao", usuario);
            cmd.ExecuteNonQuery();
        }
        private void PreencherParametros(SqlCommand cmd, EmpresaDTO empresa)
        {
            cmd.Parameters.AddWithValue("@codigo", empresa.Codigo);
            cmd.Parameters.AddWithValue("@razao_social", empresa.RazaoSocial ?? "");
            cmd.Parameters.AddWithValue("@nome_fantasia", empresa.NomeFantasia ?? "");
            cmd.Parameters.AddWithValue("@cnpj", empresa.CNPJ ?? "");
            cmd.Parameters.AddWithValue("@inscricao_estadual", empresa.InscricaoEstadual ?? "");
            cmd.Parameters.AddWithValue("@codigo_municipal", empresa.CodigoMunicipal ?? "");
            cmd.Parameters.AddWithValue("@abertura", (object)empresa.Abertura ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@cep", empresa.CEP ?? "");
            cmd.Parameters.AddWithValue("@endereco", empresa.Endereco ?? "");
            cmd.Parameters.AddWithValue("@numero", empresa.Numero ?? "");
            cmd.Parameters.AddWithValue("@complemento", empresa.Complemento ?? "");
            cmd.Parameters.AddWithValue("@bairro", empresa.Bairro ?? "");
            cmd.Parameters.AddWithValue("@municipio", empresa.Municipio ?? "");
            cmd.Parameters.AddWithValue("@uf", empresa.UF ?? "");
            cmd.Parameters.AddWithValue("@nome_uf", empresa.UFNome ?? "");
            cmd.Parameters.AddWithValue("@telefone", empresa.Telefone ?? "");
            cmd.Parameters.AddWithValue("@email", empresa.Email ?? "");            
            cmd.Parameters.AddWithValue("@situacao", empresa.Situacao ?? "");
            cmd.Parameters.AddWithValue("@modal", empresa.Modal ?? "");
            cmd.Parameters.AddWithValue("@rntrc", empresa.RNTRC ?? "");
            cmd.Parameters.AddWithValue("@logo", empresa.Logo ?? "");
            cmd.Parameters.AddWithValue("@tipo", empresa.Tipo ?? "");
            cmd.Parameters.AddWithValue("@status", empresa.Status ?? "");
            cmd.Parameters.AddWithValue("@cadastro", (object)empresa.Cadastro ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@atividade_principal", empresa.AtividadePrincipal ?? "");
            cmd.Parameters.AddWithValue("@site", empresa.Site ?? "");            
        }
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if (!ValidarTela())
                return;

            EmpresaDTO empresa = LerTela();

            EmpresaDAL dal = new EmpresaDAL();

            // Aqui chama o método que decide se insere ou atualiza
            int codigo = dal.Salvar(empresa);

            string logo = SalvarLogo(codigo);

            if (!string.IsNullOrWhiteSpace(logo))
            {
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

    }
}