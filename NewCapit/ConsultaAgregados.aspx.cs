using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NewCapit
{
    public partial class ConsultaAgregados : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString());
        protected void Page_Load(object sender, EventArgs e)
        {
            ContagemAgregados();            

        }
        private double CalcularPercentualAtivos()
        {
            string query = @"
            SELECT 
                (COUNT(CASE WHEN ativa_inativa = 'ATIVO' THEN 'ATIVO' END) * 100.0) / COUNT(*) AS PercentualAtivos
            FROM tbtransportadoras";

            double percentualAtivos = 0;

            using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        var result = command.ExecuteScalar();
                        if (result != DBNull.Value)
                        {
                            // percentualAtivos = Convert.ToDouble(result);
                            percentualAtivos = Convert.ToInt32(result);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Tratar erros
                    Console.WriteLine("Erro ao calcular percentual de registros ativos: " + ex.Message);
                }
            }

            return percentualAtivos;
        }       



        public void ContagemAgregados()
        {
            // total de agregados e terceiros
            string sqlTotal = "select count(*) from tbtransportadoras where tipo = 'AGREGADO' or tipo = 'TERCEIRO'";
            SqlDataAdapter adptTotal = new SqlDataAdapter(sqlTotal, con);
            DataTable dtTotal = new DataTable();
            con.Open();
            adptTotal.Fill(dtTotal);
            con.Close();
            TotalVeiculos.Text = dtTotal.Rows[0][0].ToString();

            // clientes na região nordeste
            string sql1 = "select count(*) from tbtransportadoras where tipo = 'AGREGADO' ";
            SqlDataAdapter adpt1 = new SqlDataAdapter(sql1, con);
            DataTable dt1 = new DataTable();
            con.Open();
            adpt1.Fill(dt1);
            con.Close();
            Agregados.Text = dt1.Rows[0][0].ToString();

            // clientes na região SUL
            string sql2 = "select count(*) from tbtransportadoras where tipo = 'TERCEIRO'";
            SqlDataAdapter adpt2 = new SqlDataAdapter(sql2, con);
            DataTable dt2 = new DataTable();
            con.Open();
            adpt2.Fill(dt2);
            con.Close();
            Terceiros.Text = dt2.Rows[0][0].ToString();

            // clientes na região Sudeste
            string sql3 = "select count(*) from tbtransportadoras where ativa_inativa = 'ATIVO' ";
            SqlDataAdapter adpt3 = new SqlDataAdapter(sql3, con);
            DataTable dt3 = new DataTable();
            con.Open();
            adpt3.Fill(dt3);
            con.Close();
            veiculosAtivos.Text = dt3.Rows[0][0].ToString();

            // clientes na região Centro-Oeste
            string sql4 = "select count(*) from tbtransportadoras where ativa_inativa = 'INATIVO' ";
            SqlDataAdapter adpt4 = new SqlDataAdapter(sql4, con);
            DataTable dt4 = new DataTable();
            con.Open();
            adpt4.Fill(dt4);
            con.Close();
            veiculosInativos.Text = dt4.Rows[0][0].ToString();
        }



    }   
}