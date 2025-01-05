using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NewCapit
{
    public partial class Frm_CadAgregados : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString());
        protected void Page_Load(object sender, EventArgs e)
        {
            PreencherComboBoxFiliais();
            // Define a data atual do CadastroLabel
            lblDataAtual.Text = DateTime.Now.ToString("dd/MM/yyyy");
            lblDtCadastro.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            btnCnpj.Visible = false;
        }
        private void PreencherComboBoxFiliais()
        {
            // Defina a string de conexão com o seu banco de dados SQL Server
            //string connectionString = "Server=seu_servidor; Database=seu_banco_de_dados; Integrated Security=True;";

            // Consulta SQL para obter os dados
            string query = "SELECT codigo, descricao FROM tbempresa";

            // Criação da conexão e comando SQL
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                try
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    // Limpa o DropDownList antes de adicionar os itens
                    ddlCombo.Items.Clear();

                    // Adiciona um item padrão
                    ddlCombo.Items.Add(new ListItem("Selecione", "0"));

                    // Preenche o DropDownList com os dados do banco
                    while (reader.Read())
                    {
                        string codigo = reader["codigo"].ToString();
                        string descricao = reader["descricao"].ToString();
                        ddlCombo.Items.Add(new ListItem(descricao, codigo));
                    }
                }
                catch (Exception ex)
                {
                    // Tratar erros aqui (exemplo, exibir uma mensagem de erro)
                    Response.Write("Erro: " + ex.Message);
                }
            }
        }

    }
}