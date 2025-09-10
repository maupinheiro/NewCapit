using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using Org.BouncyCastle.Asn1.Cmp;

namespace NewCapit.dist.pages
{
    public partial class Frm_OrdemAbastecimento : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString());
        string id;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PreencherComboFiliais();
                CarregaDadosFornecedor();
               // Carrega apenas na primeira vez
            }
            else
            {
                // Se você precisa recarregar o ddlFrutas em postbacks por algum motivo,
                // certifique-se de que a seleção do usuário seja restaurada APÓS o DataBind.
                // No entanto, a melhor prática é carregar apenas uma vez se os dados não mudam.
                // Se os dados mudam, você pode precisar de uma lógica mais complexa para preservar a seleção.
                // Por exemplo, você pode salvar o valor selecionado em ViewState antes do DataBind
                // e restaurá-lo depois.

                // Exemplo (se CarregaCombustivel() precisar ser chamado em postbacks):
                // string selectedValue = ddlFrutas.SelectedValue;
                // CarregaCombustivel();
                 
            }



        }
        
        protected void ddlFrutas_SelectedIndexChanged(object sender, EventArgs e)
        {
            string idSelecionado = ddlFrutas.SelectedItem.Text;

            if (string.IsNullOrEmpty(idSelecionado))
            {
                txtPreco.Text = "";
                return;
            }

           // string connectionString = "sua_connection_string_aqui";
            string query = "SELECT valor FROM tbprecocombustivel WHERE combustivel = @combustivel";

            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@combustivel", idSelecionado);
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    // Exemplo: mostrar cor e sabor juntos
                    string valorCombustivel = reader["valor"].ToString();
                    //string sabor = reader["Sabor"].ToString();

                    txtPreco.Text = $"{valorCombustivel}";
                }

                reader.Close();
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
                    cbFiliais.Items.Insert(0, new ListItem("", "0"));
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
        public void CarregaDadosFornecedor()
        {
            if (HttpContext.Current.Request.QueryString["id"].ToString() != "")
            {
                id = HttpContext.Current.Request.QueryString["id"].ToString();
            }
            string sql = "SELECT codfor, fantasia FROM tbfornecedores WHERE id = " + id;

            SqlDataAdapter adpt = new SqlDataAdapter(sql, con);
            DataTable dt = new DataTable();
            con.Open();
            adpt.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                // Preenchendo os TextBoxes com valores do DataTable
                if (dt.Rows[0][0].ToString() != string.Empty)
                {
                    txtCodFor.Text = dt.Rows[0][0].ToString(); 
                    txtNomFor.Text = dt.Rows[0][1].ToString();
                    if (txtNomFor.Text == "TRANSNOVAG")
                    {
                        txtExterno.BackColor = System.Drawing.Color.Purple;
                        txtExterno.ForeColor = System.Drawing.Color.White;
                        txtExterno.Text = "INTERNO";

                    }
                    else
                    {
                        txtExterno.BackColor = System.Drawing.Color.Purple;
                        txtExterno.ForeColor = System.Drawing.Color.White;
                        txtExterno.Text = "EXTERNO";
                    }

                    CarregaCombustivel();

                }

                //CarregaCombustivel();
            }
        }

        public void CarregaCombustivel()
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                string query = "SELECT id, combustivel FROM tbprecocombustivel WHERE codposto = @codposto";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    int codPosto;
                    if (!int.TryParse(txtCodFor.Text.Trim(), out codPosto))
                    {
                        return;
                    }

                    cmd.Parameters.AddWithValue("@codposto", codPosto);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    // Salva o valor selecionado antes do DataBind, se houver um postback
                    string selectedValue = string.Empty;
                    if (IsPostBack && ddlFrutas.SelectedItem != null)
                    {
                        selectedValue = ddlFrutas.SelectedValue;
                    }

                    ddlFrutas.DataSource = reader;
                    ddlFrutas.DataTextField = "combustivel";
                    ddlFrutas.DataValueField = "id";
                    ddlFrutas.DataBind();

                    // Insere o item padrão. É importante que ele tenha um DataValueField único, como uma string vazia.
                    ddlFrutas.Items.Insert(0, new ListItem("-- Combustível --", ""));

                    // Tenta restaurar a seleção após o DataBind e a inserção do item padrão
                    if (IsPostBack && !string.IsNullOrEmpty(selectedValue))
                    {
                        try
                        {
                            ddlFrutas.SelectedValue = selectedValue;
                        }
                        catch (Exception ex)
                        {
                            // Log ou trate o erro se o valor selecionado não for encontrado na nova lista
                            // Isso pode acontecer se a lista de combustíveis mudar dinamicamente
                            System.Diagnostics.Debug.WriteLine("Erro ao restaurar seleção do ddlFrutas: " + ex.Message);
                        }
                    }

                    reader.Close();
                    conn.Close();
                }
            }
        }

    }
}