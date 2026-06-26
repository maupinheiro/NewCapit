using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using NPOI.XWPF.UserModel;
using SixLabors.Fonts.Tables.AdvancedTypographic;

namespace NewCapit.dist.pages
{
    public partial class ConsultaUsuarios : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString());
        private SqlCommand cmd;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                listarUsuarios();
                // CarregarUsuarios();
                //CarregarCargos();
                PreencherComboFiliais();
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
                CarregarSetores();
            }            
        }
        private void CarregarSetores()
        {
            using (SqlConnection conn = new SqlConnection(
                WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                string sql = @"
            SELECT DISTINCT setor
            FROM tbsetorfuncao
            WHERE setor IS NOT NULL
            ORDER BY setor";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();

                    ddlDep_Usuario.DataSource = cmd.ExecuteReader();
                    ddlDep_Usuario.DataTextField = "setor";
                    ddlDep_Usuario.DataValueField = "setor";
                    ddlDep_Usuario.DataBind();
                }
            }

            ddlDep_Usuario.Items.Insert(0, new ListItem("-- Selecione --", ""));
        }
        
        protected void ddlDep_Usuario_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarFuncoes(ddlDep_Usuario.SelectedValue);

            ScriptManager.RegisterStartupScript(
                this,
                GetType(),
                "ShowModal",
                "var modal = new bootstrap.Modal(document.getElementById('modalCadastro')); modal.show();",
                true);
        }


        private void CarregarFuncoes(string setor)
        {
            ddlFun_Usuario.Items.Clear();

            if (string.IsNullOrEmpty(setor))
                return;

            using (SqlConnection conn = new SqlConnection(
                WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                string sql = @"
            SELECT DISTINCT funcao
            FROM tbsetorfuncao
            WHERE setor = @setor
            ORDER BY funcao";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@setor", setor);

                    conn.Open();

                    ddlFun_Usuario.DataSource = cmd.ExecuteReader();
                    ddlFun_Usuario.DataTextField = "funcao";
                    ddlFun_Usuario.DataValueField = "funcao";
                    ddlFun_Usuario.DataBind();
                }
            }

            ddlFun_Usuario.Items.Insert(0, new ListItem("-- Selecione --", ""));
        }




        private void listarUsuarios()
        {

            var dataTable = DAL.ConUsuarios.FetchDataTable();
            if (dataTable.Rows.Count <= 0)
            {
                return;
            }
            gvListUsuarios.DataSource = dataTable;
            gvListUsuarios.DataBind();

        }
        private void CarregarUsuarios()
        {
            string sqlUsuarios = "SELECT foto_usuario, cod_usuario, nm_nome, nm_usuario, emp_usuario, fun_usuario, dep_usuario, CONVERT(varchar, dt_ultimo_acesso, 103) AS dt_ultimo_acesso, fl_tipo, fl_status FROM tb_usuario  ORDER BY nm_nome";
            SqlDataAdapter adptUsuarios = new SqlDataAdapter(sqlUsuarios, con);
            DataTable dtUsuarios = new DataTable();

            try
            {
                con.Open();
                adptUsuarios.Fill(dtUsuarios);

                // CORREÇÃO: Passar a DataTable (dtUsuarios) e não o Adapter (adptUsuarios)
                gvListUsuarios.DataSource = dtUsuarios;
                gvListUsuarios.DataBind();
            }
            catch (Exception ex)
            {
                // Trate o erro aqui como preferir (ex: Console.WriteLine, MessageBox, etc)
                throw ex;
            }
            finally
            {
                // Sempre feche a conexão após o uso
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }
        //private void CarregarCargos()
        //{
        //    using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
        //    {
        //        conn.Open();
        //        string query = "SELECT cod_funcao, nm_funcao FROM tb_funcao ORDER BY nm_funcao";
        //        SqlCommand cmd = new SqlCommand(query, conn);
        //        SqlDataReader reader = cmd.ExecuteReader();

        //        ddlFun_Usuario.DataSource = reader;
        //        ddlFun_Usuario.DataTextField = "nm_funcao";  // Campo a ser exibido
        //        ddlFun_Usuario.DataValueField = "cod_funcao";  // Valor associado ao item
        //        ddlFun_Usuario.DataBind();

        //        // Adicionar o item padrão
        //        ddlFun_Usuario.Items.Insert(0, new ListItem("Selecione...", "0"));
        //    }
        //}
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
                    ddlEmp_Usuario.DataSource = reader;
                    ddlEmp_Usuario.DataTextField = "descricao";  // Campo que será mostrado no ComboBox
                    ddlEmp_Usuario.DataValueField = "codigo";  // Campo que será o valor de cada item                    
                    ddlEmp_Usuario.DataBind();  // Realiza o binding dos dados                   
                    ddlEmp_Usuario.Items.Insert(0, new ListItem("Selecione...", "0"));
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
        protected void gvListUsuarios_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string status = DataBinder.Eval(e.Row.DataItem, "fl_status").ToString();

                if (status == "A")
                {
                    e.Row.Cells[9].Text = "ATIVO"; // Coluna Status
                }
                else if (status == "I")
                {
                    e.Row.Cells[9].Text = "INATIVO";
                }
            }
        }

        protected void gvListUsuarios_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvListUsuarios.PageIndex = e.NewPageIndex;
            listarUsuarios();  // Método para recarregar os dados no GridView
        }
        private void AllData(string searchTerm = "")
        {
            var dataTable = DAL.ConUsuarios.FetchDataTable2(searchTerm);
            if (dataTable.Rows.Count <= 0)
            {
                gvListUsuarios.DataSource = null;
                gvListUsuarios.DataBind();
                return;
            }

            gvListUsuarios.DataSource = dataTable;
            gvListUsuarios.DataBind();
        }
        protected void myInput_TextChanged(object sender, EventArgs e)
        {
            string searchTerm = myInput.Text.Trim();
            AllData(searchTerm);
        }
        protected void Editar(object sender, EventArgs e)
        {
            // 1. Captura o LinkButton que foi clicado
            LinkButton lnk = (LinkButton)sender;

            // 2. Encontra com segurança a linha do GridView (GridViewRow) onde ele está instalado
            GridViewRow row = (GridViewRow)lnk.NamingContainer;

            // 3. Recupera o ID mapeado no DataKeyNames do GridView
            string id = gvListUsuarios.DataKeys[row.RowIndex].Value.ToString();

            // 4. Executa o redirecionamento para a tela de permissões
            Response.Redirect("ControleAcesso2.aspx?id=" + id);
        }
        protected void ResetarSenha_Click(object sender, EventArgs e)
        {
            // Recupera o ID do usuário através do CommandArgument
            LinkButton btn = (LinkButton)sender;
            string idUsuario = btn.CommandArgument;

            // Lógica de Update (Exemplo genérico usando SQL Client)
            using (SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                string query = "UPDATE tb_usuario SET ds_senha = 'mudar123' WHERE cod_usuario = @id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", idUsuario);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            // Exibe o alerta (MessageBox) usando ScriptManager
            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Senha resetada com sucesso!');", true);

            // Opcional: Rebind do grid para refletir qualquer mudança visual
            CarregarUsuarios();
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            string nome = txtNm_Nome.Text;
            string usuario = txtNm_Usuario.Text;
            string senha = "mudar123";
            string email = txtDs_Email.Text.Trim();
            string status = ddlStatus.SelectedValue;
            string tipo = ddlNivel.SelectedValue;

            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                string insertQuery = "insert tb_usuario (nm_nome, nm_usuario, ds_senha, ds_email, fl_tipo, fl_status)";
                insertQuery += "values (@nm_nome, @nm_usuario, @ds_senha, @ds_email, @fl_tipo, @fl_status)";
                SqlCommand cmd = new SqlCommand(insertQuery, conn);
                cmd.Parameters.AddWithValue("@nm_nome", nome);
                cmd.Parameters.AddWithValue("@nm_usuario", usuario);
                cmd.Parameters.AddWithValue("@ds_senha", senha);
                cmd.Parameters.AddWithValue("@ds_email", email);
                cmd.Parameters.AddWithValue("@fl_tipo", tipo);
                cmd.Parameters.AddWithValue("@fl_status", status);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            lblMensagem.Text = "Usuário cadastrado com sucesso!";
           // LimparCampos();
            CarregarUsuarios();

            // Reabrir modal com JS se quiser mostrar a mensagem
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "abrirModal", "$('#modalCadastro').modal('show');", true);

            ScriptManager.RegisterStartupScript(
                updSetorFuncao,
                updSetorFuncao.GetType(),
                "CloseModal",
                @"
                var modalEl = document.getElementById('modalCadastro');
                var modal = bootstrap.Modal.getInstance(modalEl);
                if(modal) modal.hide();
                ",
            true);

        }

       

    }
}