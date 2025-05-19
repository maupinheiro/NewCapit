using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace NewCapit.dist.pages
{
    public partial class ConsultaColetasPopUp : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
         
                CarregarGrid();
            }
        }

        private void CarregarGrid()
        {
            string connStr = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT id, carga, data_hora, cliorigem, clidestino, veiculo, tipo_viagem, rota, andamento FROM tbcargas WHERE andamento = 'PENDENTE'";
                SqlCommand command = new SqlCommand(query, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                GVColetas.DataSource = dataTable;
                GVColetas.DataBind();
            }
        }

        protected void timerAtualiza_Tick(object sender, EventArgs e)
        {
            CarregarGrid();
        }


        protected void btnAtualizar_Click(object sender, EventArgs e)
        {
            CarregarGrid();
        }
    }
}

