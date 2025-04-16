using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NewCapit.dist.pages
{
    public partial class ConsultaEntregas : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PreencherComboStatus();
            PreencherVeiculosCNT();
            if(!IsPostBack)
            {
                CarregarColetas();
            }
            
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
        private void PreencherVeiculosCNT()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT id, descricao FROM tbtiposveiculoscnt order by descricao";

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
                    ddlVeiculosCNT.DataSource = reader;
                    ddlVeiculosCNT.DataTextField = "descricao";  // Campo que será mostrado no ComboBox
                    ddlVeiculosCNT.DataValueField = "id";  // Campo que será o valor de cada item                    
                    ddlVeiculosCNT.DataBind();  // Realiza o binding dos dados                   
                    ddlVeiculosCNT.Items.Insert(0, new ListItem("Selecione...", "0"));
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

        private void CarregarColetas()
        {
            var novosDados = DAL.ConEntrega.FetchDataTable();

            rptCarregamento.DataSource = novosDados;
            rptCarregamento.DataBind();

            // Armazena no ViewState, se necessário para outras operações
            ViewState["rptCarregamento"] = novosDados;

            lblMensagem.Text = string.Empty;
        }
        protected void rptCarregamento_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                // Pega o valor da carga ou do CVA do item atual
                string cva = DataBinder.Eval(e.Item.DataItem, "cva").ToString();

                // Pega o repeater interno
                Repeater rptColeta = (Repeater)e.Item.FindControl("rptColeta");

                if (rptColeta != null && !string.IsNullOrEmpty(cva))
                {
                    // Busca os dados de coletas relacionadas àquele CVA
                    DataTable dtColetas = DAL.ConCargas.FetchDataTableColetas3(cva);

                    // Bind dos dados ao repeater interno
                    rptColeta.DataSource = dtColetas;
                    rptColeta.DataBind();
                }
            }
        }

    }
}