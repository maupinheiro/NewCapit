using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Net.Mail;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Text.RegularExpressions;
using System.Globalization;
using FluentEmail.Core;


namespace NewCapit.dist.pages
{
    public partial class ColaboradoresManutencao : System.Web.UI.Page
    {
        string conexao = WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
        DateTime dataHoraAtual = DateTime.Now;
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
                    var lblUsuario = "<Usuário>";
                    Response.Redirect("Login.aspx");
                }
                txtAdmissao.Text = dataHoraAtual.ToString("dd/MM/yyyy");
                PreencherComboFiliais();
                CarregarCargos();
                PreencherComboJornada();
                CarregarProfissionais();

            }
        }
        private void CarregarCargos()
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                conn.Open();
                string query = "SELECT id, setor, funcao FROM tbsetorfuncao where setor = 'MANUTENCAO' ORDER BY funcao";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                ddlCargo.DataSource = reader;
                ddlCargo.DataTextField = "funcao";  // Campo a ser exibido
                ddlCargo.DataValueField = "id";  // Valor associado ao item
                ddlCargo.DataBind();

                // Adicionar o item padrão
                ddlCargo.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", "0"));                
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
                    cbFiliais.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", "0"));
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
        private void PreencherComboJornada()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT id, descricao FROM tbhorarios order by descricao ASC";

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
                    ddlJornada.DataSource = reader;
                    ddlJornada.DataTextField = "descricao";  // Campo que será mostrado no ComboBox
                    ddlJornada.DataValueField = "id";  // Campo que será o valor de cada item                    
                    ddlJornada.DataBind();  // Realiza o binding dos dados                   
                    ddlJornada.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", "0"));
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
        private void CarregarProfissionais()
        {
            using (SqlConnection conn = new SqlConnection(conexao))
            {
                string sql = "SELECT * FROM tbprofissional_manutencao WHERE fl_exclusao IS NULL";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvProfissionais.DataSource = dt;
                gvProfissionais.DataBind();
            }
        }
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            
            string cracha = txtCracha.Text;
            string nome = txtNome.Text.Trim();
            string cpf = txtCPF.Text.Trim();
            string funcao = ddlCargo.SelectedItem.Text ;
            string jornada = ddlJornada.SelectedItem.Text;
            string filial = cbFiliais.SelectedItem.Text;
            string telefone = txtTelefone.Text.Trim();           
            string status = ddlStatus.SelectedItem.Text;
            string email = txtEmail.Text.Trim();
            string motivo = txtMotivo.Text.Trim();
            string dtdemissao = txtDemissao.Text;

            DateTime admissao;
            DateTime? demissao = null;

            if (ddlStatus.SelectedValue == "INATIVO" && !string.IsNullOrEmpty(txtDemissao.Text))
            {
                demissao = Convert.ToDateTime(txtDemissao.Text);
            }

            // Converte do TextBox
            if (!DateTime.TryParseExact(txtAdmissao.Text.Trim(), "dd/MM/yyyy", new CultureInfo("pt-BR"), DateTimeStyles.None, out admissao))
            {
                Mensagem("info", "Data de admissão inválida (dd/MM/yyyy)");                
                txtAdmissao.Focus();
                return;
            }
                        
            // 1️⃣ Validações
            if (string.IsNullOrEmpty(cracha)) { Mensagem("info", "Informe o crachá do colaborador."); txtCracha.Focus(); return; }
            if (string.IsNullOrEmpty(nome)) { Mensagem("info", "Informe o nome do colaborador."); txtNome.Focus(); return; }
            if (!ValidarCPF(cpf)) { Mensagem("info", "CPF inválido."); txtCPF.Focus(); return; }
            if (string.IsNullOrEmpty(ddlCargo.SelectedValue)) { Mensagem("info", "Informe a função do colaborador."); ddlCargo.Focus(); return; }
            if (string.IsNullOrEmpty(ddlJornada.SelectedValue)) { Mensagem("info", "Informe horário de trabalho do colaborador."); ddlJornada.Focus(); return; }
            if (string.IsNullOrEmpty(cbFiliais.SelectedValue)) { Mensagem("info", "Informe a filial de trabalho do colaborador."); cbFiliais.Focus(); return; }
            if (!string.IsNullOrEmpty(email) && !ValidarEmail(email)) { Mensagem("info", "E-mail inválido."); txtEmail.Focus(); return; }
            if (string.IsNullOrEmpty(telefone)) { Mensagem("info", "Informe o celular do colaborador."); txtTelefone.Focus(); return; }

            string fotoPath = "";

            // 2️⃣ Upload da nova foto ou mantém a antiga
            if (fuFoto.HasFile)
            {
                string fileName = Guid.NewGuid() + Path.GetExtension(fuFoto.FileName);
                string folder = Server.MapPath("~/Fotos/");
                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
                fuFoto.SaveAs(Path.Combine(folder, fileName));
                fotoPath = "~/Fotos/" + fileName;
                hfFoto.Value = fotoPath;
            }
            else if (!string.IsNullOrEmpty(hfFoto.Value))
            {
                fotoPath = hfFoto.Value;
            }

            using (SqlConnection conn = new SqlConnection(conexao))
            {
                conn.Open();

                // 3️⃣ Verifica duplicidade CPF
                SqlCommand cmdCheckCPF = new SqlCommand(
                    "SELECT COUNT(1) FROM tbprofissional_manutencao WHERE cpf=@cpf AND (@id=0 OR id<>@id)", conn);
                cmdCheckCPF.Parameters.AddWithValue("@cpf", cpf);
                cmdCheckCPF.Parameters.AddWithValue("@id", string.IsNullOrEmpty(hfId.Value) ? 0 : Convert.ToInt32(hfId.Value));
                if ((int)cmdCheckCPF.ExecuteScalar() > 0) { Mensagem("info", "CPF já cadastrado"); txtCPF.Focus(); return; }

                // 4️⃣ Verifica duplicidade E-mail
                if (!string.IsNullOrEmpty(email))
                {
                    SqlCommand cmdCheckEmail = new SqlCommand(
                        "SELECT COUNT(1) FROM tbprofissional_manutencao WHERE LOWER(email)=LOWER(@email) AND (@id=0 OR id<>@id)", conn);
                    cmdCheckEmail.Parameters.AddWithValue("@email", email);
                    cmdCheckEmail.Parameters.AddWithValue("@id", string.IsNullOrEmpty(hfId.Value) ? 0 : Convert.ToInt32(hfId.Value));
                    if ((int)cmdCheckEmail.ExecuteScalar() > 0) { Mensagem("info", "E-mail já cadastrado"); txtEmail.Focus(); return; }
                }

                // 5️⃣ Inserir ou atualizar
                if (string.IsNullOrEmpty(hfId.Value))
                {
                    string sql = @"INSERT INTO tbprofissional_manutencao
                            (cracha, nome, cpf, funcao, telefone, email, foto, horario, filial, status, admissao, motivo, demissao)
                            VALUES (@cracha, @nome, @cpf,@funcao,@telefone,@email,@foto, @horario, @filial, @status, @admissao, @motivo, @demissao)";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@cracha", cracha);
                    cmd.Parameters.AddWithValue("@nome", nome);
                    cmd.Parameters.AddWithValue("@cpf", cpf);
                    cmd.Parameters.AddWithValue("@funcao", funcao);
                    cmd.Parameters.AddWithValue("@telefone", telefone);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@foto", fotoPath);
                    cmd.Parameters.AddWithValue("@horario", jornada);
                    cmd.Parameters.AddWithValue("@filial", filial);
                    cmd.Parameters.AddWithValue("@status", status);
                    cmd.Parameters.AddWithValue("@admissao", admissao);
                    cmd.Parameters.AddWithValue("@motivo", motivo);
                    cmd.Parameters.AddWithValue("@demissao", demissao.HasValue ? (object)demissao.Value : DBNull.Value);
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    string sql = @"UPDATE tbprofissional_manutencao SET
                            nome=@nome, cpf=@cpf, funcao=@funcao, telefone=@telefone, email=@email, foto=@foto, horario=@horario, filial=@filial, admissao=@admissao, status=@status, motivo=@motivo, demissao=@demissao
                            WHERE id=@id";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id", Convert.ToInt32(hfId.Value));
                    cmd.Parameters.AddWithValue("@cracha", nome);
                    cmd.Parameters.AddWithValue("@nome", nome);
                    cmd.Parameters.AddWithValue("@cpf", cpf);
                    cmd.Parameters.AddWithValue("@funcao", funcao);
                    cmd.Parameters.AddWithValue("@telefone", telefone);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@foto", fotoPath);
                    cmd.Parameters.AddWithValue("@horario", jornada);
                    cmd.Parameters.AddWithValue("@filial", filial);
                    cmd.Parameters.AddWithValue("@status", status);
                    cmd.Parameters.AddWithValue("@admissao", admissao);
                    cmd.Parameters.AddWithValue("@motivo", motivo);
                    cmd.Parameters.AddWithValue("@demissao", demissao.HasValue ? (object)demissao.Value : DBNull.Value);

                    cmd.ExecuteNonQuery();
                }
            }

            // 6️⃣ Limpar campos e atualizar grid
            LimparCampos();
            CarregarProfissionais();
            Mensagem("success", "Colaborador salvo com sucesso!");
        }
        private void LimparCampos()
        {
            hfId.Value = "";
            hfFoto.Value = "";

            txtCracha.Text = "";
            txtNome.Text = "";
            txtCPF.Text = "";

            ddlCargo.SelectedIndex = 0;
            ddlJornada.SelectedIndex = 0;
            cbFiliais.SelectedIndex = 0;

            txtAdmissao.Text = "";
            ddlStatus.SelectedIndex = 0;

            txtTelefone.Text = "";
            txtEmail.Text = "";

            txtDemissao.Text = "";
            txtMotivo.Text = "";
            divDemissao.Visible = false;

            imgPreview.Style["display"] = "none";
            // Limpar arquivo
            fuFoto.Dispose();
        }
        protected void gvProfissionais_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(conexao))
            {
                conn.Open();

                if (e.CommandName == "Excluir")
                {
                    //string sql = "DELETE FROM tbprofissional_manutencao WHERE id=@id";
                    //SqlCommand cmd = new SqlCommand(sql, conn);
                    //cmd.Parameters.AddWithValue("@id", e.CommandArgument);
                    //cmd.ExecuteNonQuery();

                    string sql = @"UPDATE tbprofissional_manutencao SET
                            status=@status, fl_exclusao=@fl_exclusao
                            WHERE id=@id";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id", e.CommandArgument);
                    cmd.Parameters.AddWithValue("@status", "INATIVO");
                    cmd.Parameters.AddWithValue("@fl_exclusao", "S"); 
                    cmd.ExecuteNonQuery();
                }

                if (e.CommandName == "Editar")
                {
                    int id = Convert.ToInt32(e.CommandArgument); 
                    string sql = "SELECT * FROM tbprofissional_manutencao WHERE id = @id";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id", id);

                    //conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        hfId.Value = dr["id"].ToString();
                        txtCracha.Text = dr["cracha"].ToString();
                        txtNome.Text = dr["nome"].ToString();
                        txtCPF.Text = dr["cpf"].ToString();

                        ddlCargo.SelectedItem.Text = dr["funcao"].ToString();
                        ddlJornada.SelectedItem.Text = dr["horario"].ToString();
                        cbFiliais.SelectedItem.Text = dr["filial"].ToString();                       
                        if (dr["admissao"] != DBNull.Value)
                        {
                            txtAdmissao.Text = Convert.ToDateTime(dr["admissao"]).ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            txtAdmissao.Text = "";
                        }
                        ddlStatus.SelectedItem.Text = dr["status"].ToString();

                        txtTelefone.Text = dr["telefone"].ToString();
                        txtEmail.Text = dr["email"].ToString();
                        if (dr["status"].ToString() == "INATIVO")
                        {
                            divDemissao.Visible = true;

                            txtDemissao.Text = Convert.ToDateTime(dr["demissao"]).ToString("dd/MM/yyyy");
                            txtMotivo.Text = dr["motivo"].ToString();
                        }
                        else
                        {
                            divDemissao.Visible = false;
                        }

                        // FOTO
                        if (dr["foto"] != DBNull.Value)
                        {
                            string foto = dr["foto"].ToString();
                            hfFoto.Value = foto;

                            imgPreview.Src = foto;
                            imgPreview.Style["display"] = "block";
                        }
                    }
                   

                    // 👉 opcional: rolar a tela até o formulário
                    ScriptManager.RegisterStartupScript(this, GetType(), "scroll",
                        "window.scrollTo({ top: 0, behavior: 'smooth' });", true);
                }
            }

            CarregarProfissionais();
        }
        private void CarregarProfissionais(string filtro = "")
        {
            using (SqlConnection conn = new SqlConnection(conexao))
            {
                string sql = "SELECT * FROM tbprofissional_manutencao WHERE nome LIKE @nome AND fl_exclusao IS NULL";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@nome", "%" + filtro + "%");

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvProfissionais.DataSource = dt;
                gvProfissionais.DataBind();
            }
        }
        protected void txtBusca_TextChanged(object sender, EventArgs e)
        {
            CarregarProfissionais(txtBusca.Text);
        }
        public bool ValidarCPF(string cpf)
        {
            cpf = cpf.Replace(".", "").Replace("-", "").Trim();

            if (cpf.Length != 11)
                return false;

            // Elimina CPFs inválidos conhecidos
            if (cpf == "00000000000" ||
                cpf == "11111111111" ||
                cpf == "22222222222" ||
                cpf == "33333333333" ||
                cpf == "44444444444" ||
                cpf == "55555555555" ||
                cpf == "66666666666" ||
                cpf == "77777777777" ||
                cpf == "88888888888" ||
                cpf == "99999999999")
                return false;

            int[] multiplicador1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCpf = cpf.Substring(0, 9);
            int soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;

            string digito = resto.ToString();
            tempCpf += digito;

            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;

            digito += resto.ToString();

            return cpf.EndsWith(digito);
        }
        public bool ValidarEmail(string email)
        {
            try
            {
                MailAddress mail = new MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }
        protected void Mensagem(string tipo, string texto)
        {
            divMsg.Visible = true;

            divMsg.Attributes["class"] =
                "alert alert-" + tipo + " alert-dismissible fade show mt-3";

            lblMsgGeral.Text = texto;
        }
        public bool ValidarEmailRegex(string email)
        {
            return Regex.IsMatch(email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }
        protected void txtCracha_TextChanged(object sender, EventArgs e)
        {
            string cracha = txtCracha.Text.Trim();

            if (string.IsNullOrEmpty(cracha))
                return;

            using (SqlConnection conn = new SqlConnection(conexao))
            {
                conn.Open();
                string sqlCheck = "SELECT COUNT(1) FROM tbprofissional_manutencao WHERE cracha = @cracha";
                using (SqlCommand cmdCheck = new SqlCommand(sqlCheck, conn))
                {
                    cmdCheck.Parameters.AddWithValue("@cracha", cracha);
                    int existe = (int)cmdCheck.ExecuteScalar();

                    if (existe > 0)
                    {
                        Mensagem("warning", "Crachá ja cadastrado no sistema.");
                        txtCracha.Text = "";
                        txtCracha.Focus();
                        return;
                    }                    
                }
            }
        }
        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlStatus.SelectedValue == "INATIVO")
            {
                divDemissao.Visible = true;
            }
            else
            {
                divDemissao.Visible = false;

                // limpa os campos ao voltar para ativo
                txtDemissao.Text = "";
                txtMotivo.Text = "";
            }
        }
    }
}