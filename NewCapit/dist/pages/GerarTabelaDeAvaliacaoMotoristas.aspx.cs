using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Web.Services;


namespace NewCapit.dist.pages
{
    public partial class GerarTabelaDeAvaliacaoMotoristas : System.Web.UI.Page
    {
        SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString());
        string nomeUsuario = string.Empty;        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // CarregarGrid();

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
                CarregarStatus();
                CarregarPedidos();
            }
            PreencherComboFiliais();
           
            
        }
        private void PreencherComboFiliais()
        {
            // Consulta SQL que retorna os dados desejados
            //string query = "SELECT DISTINCT descricao FROM tbempresa";

            //// Crie uma conexão com o banco de dados
            //using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            //{
            //    try
            //    {
            //        // Abra a conexão com o banco de dados
            //        conn.Open();

            //        // Crie o comando SQL
            //        SqlCommand cmd = new SqlCommand(query, conn);

            //        // Execute o comando e obtenha os dados em um DataReader
            //        SqlDataReader reader = cmd.ExecuteReader();

            //        // Preencher o ComboBox com os dados do DataReader
            //        cbFiliais.DataSource = reader;
            //        cbFiliais.DataTextField = "descricao";  // Campo que será mostrado no ComboBox
            //        cbFiliais.DataValueField = "descricao";  // Campo que será o valor de cada item                    
            //        cbFiliais.DataBind();  // Realiza o binding dos dados                   
            //        cbFiliais.Items.Insert(0, new ListItem("", "0"));
            //        // Feche o reader
            //        reader.Close();
            //    }
            //    catch (Exception ex)
            //    {
            //        // Trate exceções
            //        Response.Write("Erro: " + ex.Message);
            //    }
            //}

            //using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            //{
            //    string query = "SELECT DISTINCT nucleo FROM tbmotoristas ORDER BY nucleo";
            //    SqlCommand cmd = new SqlCommand(query, conn);
            //    conn.Open();
            //    SqlDataReader dr = cmd.ExecuteReader();

            //    cbFiliais.DataSource = dr;
            //    cbFiliais.DataTextField = "nucleo";
            //    cbFiliais.DataValueField = "nucleo";
            //    cbFiliais.DataBind();
            //}
        }


        private void CarregarStatus()
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                string query = "SELECT DISTINCT nucleo FROM tbmotoristas ORDER BY nucleo";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                ddlStatus.DataSource = dr;
                ddlStatus.DataTextField = "nucleo";
                ddlStatus.DataValueField = "nucleo";
                ddlStatus.DataBind();
            }
        }
        private void CarregarPedidos(string[] statusSelecionados = null)
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                string query = "SELECT codmot, nommot, cargo, tipomot, cadmot, frota, nucleo FROM tbmotoristas";

                if (statusSelecionados != null && statusSelecionados.Length > 0)
                {
                    string filtros = string.Join(",", statusSelecionados.Select((s, i) => "@status" + i));
                    query += $" WHERE nucleo IN ({filtros})";
                }

                SqlCommand cmd = new SqlCommand(query, conn);

                if (statusSelecionados != null)
                {
                    for (int i = 0; i < statusSelecionados.Length; i++)
                        cmd.Parameters.AddWithValue("@status" + i, statusSelecionados[i]);
                }

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvPedidos.DataSource = dt;
                gvPedidos.DataBind();
            }
        }
        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            string[] selecionados = ddlStatus.Items.Cast<System.Web.UI.WebControls.ListItem>()
                .Where(x => x.Selected)
                .Select(x => x.Value)
                .ToArray();

            CarregarPedidos(selecionados);
        }


    }



}




