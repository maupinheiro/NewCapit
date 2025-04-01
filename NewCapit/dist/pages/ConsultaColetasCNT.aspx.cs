using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NewCapit.dist.pages
{
    public partial class ConsultaColetasCNT : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PreencherComboStatus();
            PreencherColetas();
            PreencherComboVeiculos();
        }
        private void PreencherComboStatus()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT cod_status, ds_status FROM tb_status";

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
                    ddlStatus.DataSource = reader;
                    ddlStatus.DataTextField = "ds_status";  // Campo que será mostrado no ComboBox
                    ddlStatus.DataValueField = "cod_status";  // Campo que será o valor de cada item                    
                    ddlStatus.DataBind();  // Realiza o binding dos dados                   
                    ddlStatus.Items.Insert(0, new ListItem("Selecione...", "0"));
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
        private void PreencherComboVeiculos()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT ID, descricao FROM tbtiposveiculoscnt ORDER BY descricao";

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
                    ddlVeiculos.DataSource = reader;
                    ddlVeiculos.DataTextField = "descricao";  // Campo que será mostrado no ComboBox
                    ddlVeiculos.DataValueField = "id";  // Campo que será o valor de cada item                    
                    ddlVeiculos.DataBind();  // Realiza o binding dos dados                   
                    ddlVeiculos.Items.Insert(0, new ListItem("Selecione...", "0"));
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
        private void PreencherColetas()
        {

            var dataTable = DAL.ConCargas.FetchDataTableColetas();
            if (dataTable.Rows.Count <= 0)
            {
                return;
            }
            gvListCargas.DataSource = dataTable;
            gvListCargas.DataBind();

            //gvListCargas.UseAccessibleHeader = true;
            //gvListCargas.HeaderRow.TableSection = TableRowSection.TableHeader;
            //gvListCargas.FooterRow.TableSection = TableRowSection.TableFooter;
        }
    }        
     
}