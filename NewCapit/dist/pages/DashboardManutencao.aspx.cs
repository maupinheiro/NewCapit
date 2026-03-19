using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NewCapit.dist.pages
{
    public partial class DashboardManutencao : System.Web.UI.Page
    {
        SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString());

        public string jsonCustos;
        public string jsonStatus;
        public string jsonDiesel;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregarCards();
                CarregarGraficos();
            }
        }
        private void CarregarCards()
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                conn.Open();

                lblAbertas.Text = new SqlCommand(
                    "SELECT COUNT(*) FROM tbordem_servico WHERE status = 1", conn)
                    .ExecuteScalar().ToString();

                lblFinalizadas.Text = new SqlCommand(
                    "SELECT COUNT(*) FROM tbordem_servico WHERE status = 2 AND MONTH(data_fechamento)=MONTH(GETDATE())", conn)
                    .ExecuteScalar().ToString();

                lblCustoMes.Text = "R$ " + new SqlCommand(
                    @"SELECT ISNULL(SUM(valor_unitario * quant),0)
                  FROM tbos_pecas op
                  INNER JOIN tbordem_servico os ON os.id_os = op.id_os
                  WHERE MONTH(os.data_abertura)=MONTH(GETDATE())", conn)
                    .ExecuteScalar().ToString();
            }
        }

        private void CarregarGraficos()
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            // Custos por mês
            var labels = new List<string>();
            var valores = new List<decimal>();

            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(
                    @"SELECT DATENAME(MONTH,data_abertura) mes,
                  SUM(valor_unitario * quant) total
                  FROM tbos_pecas op
                  INNER JOIN tbordem_servico os ON os.id_os = op.id_os
                  GROUP BY DATENAME(MONTH,data_abertura), MONTH(data_abertura)
                  ORDER BY MONTH(data_abertura)", conn);

                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    labels.Add(dr["mes"].ToString());
                    valores.Add(Convert.ToDecimal(dr["total"]));
                }
            }

            jsonCustos = js.Serialize(new { labels, valores });

            // Status
            jsonStatus = js.Serialize(new
            {
                labels = new[] { "Aberta", "Finalizada", "Cancelada" },
                valores = new[] {
                GetCountStatus(1),
                GetCountStatus(2),
                GetCountStatus(3)
            }
            });

            // Diesel (média km/l simplificada)
            jsonDiesel = js.Serialize(new
            {
                labels = new[] { "Jan", "Fev", "Mar", "Abr" },
                valores = new[] { 3.5, 3.2, 3.8, 3.6 }
            });
        }

        private int GetCountStatus(int status)
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                conn.Open();
                return Convert.ToInt32(new SqlCommand(
                    "SELECT COUNT(*) FROM tbordem_servico WHERE status=" + status, conn)
                    .ExecuteScalar());
            }
        }
    }
}