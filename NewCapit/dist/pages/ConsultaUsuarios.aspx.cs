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
                CarregarCargos();
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

            }            
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
            string sqlUsuarios = "SELECT foto_usuario, cod_usuario, nm_nome, nm_usuario, emp_usuario, fun_usuario, dep_usuario, CONVERT(varchar, dt_ultimo_acesso, 103) AS  dt_ultimo_acesso, fl_tipo, fl_status FROM tb_usuario ORDER BY nm_nome";
            SqlDataAdapter adptUsuarios = new SqlDataAdapter(sqlUsuarios, con);
            DataTable dtUsuarios = new DataTable();
            con.Open();
            adptUsuarios.Fill(dtUsuarios);
            gvListUsuarios.DataSource = adptUsuarios;
            gvListUsuarios.DataBind();
        }
        private void CarregarCargos()
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                conn.Open();
                string query = "SELECT cod_funcao, nm_funcao FROM tb_funcao ORDER BY nm_funcao";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                ddlFun_Usuario.DataSource = reader;
                ddlFun_Usuario.DataTextField = "nm_funcao";  // Campo a ser exibido
                ddlFun_Usuario.DataValueField = "cod_funcao";  // Valor associado ao item
                ddlFun_Usuario.DataBind();

                // Adicionar o item padrão
                ddlFun_Usuario.Items.Insert(0, new ListItem("Selecione...", "0"));
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
            using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
            {
                string id = gvListUsuarios.DataKeys[row.RowIndex].Value.ToString();

                // Response.Redirect("Frm_AltMotoristas.aspx?id=" + id);
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
           // string nome = txtNome.Text.Trim();
           // string email = txtEmail.Text.Trim();
          //  string status = ddlStatus.SelectedValue;
           
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                string insertQuery = "INSERT INTO Usuarios (Nome, Email, Status) VALUES (@Nome, @Email, @Status)";
                SqlCommand cmd = new SqlCommand(insertQuery, conn);
                //cmd.Parameters.AddWithValue("@Nome", nome);
                //cmd.Parameters.AddWithValue("@Email", email);
                //cmd.Parameters.AddWithValue("@Status", status);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            lblMensagem.Text = "Usuário cadastrado com sucesso!";
           // LimparCampos();
            CarregarUsuarios();

            // Reabrir modal com JS se quiser mostrar a mensagem
            ScriptManager.RegisterStartupScript(this, this.GetType(), "abrirModal", "$('#modalCadastro').modal('show');", true);
        }

       

    }
}