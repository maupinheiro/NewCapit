using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;

namespace NewCapit
{
    public partial class ConsultaVeiculos : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString());
        protected void Page_Load(object sender, EventArgs e)
        {
            ContagemVeiculo();
        }
        public void ContagemVeiculo()
        {
            // total de veiculos
            string sqlTotalVeiculos = "select count(*) from tbveiculos where ativo_inativo = 'ATIVO'";
            SqlDataAdapter adptTotalVeiculos = new SqlDataAdapter(sqlTotalVeiculos, con);
            DataTable dtTotalVeiculos = new DataTable();
            con.Open();
            adptTotalVeiculos.Fill(dtTotalVeiculos);
            con.Close();
            TotalVeiculos.Text = dtTotalVeiculos.Rows[0][0].ToString();

            // Frota Total
            string sqlFrota = "select count(*) from tbveiculos where ativo_inativo = 'ATIVO' and tipoveiculo = 'FROTA'  ";
            SqlDataAdapter adptFrota = new SqlDataAdapter(sqlFrota, con);
            DataTable dtFrota = new DataTable();
            con.Open();
            adptFrota.Fill(dtFrota);
            con.Close();
            TotalFrota.Text = dtFrota.Rows[0][0].ToString();

            // agregados Matriz
            string sqlAgregadoMatriz = "select count(*) from tbveiculos where ativo_inativo = 'ATIVO' and tipoveiculo = 'AGREGADO' and nucleo = 'MATRIZ'";
            SqlDataAdapter adptAgregadoMatriz = new SqlDataAdapter(sqlAgregadoMatriz, con);
            DataTable dtAgregadoMatriz = new DataTable();
            con.Open();
            adptAgregadoMatriz.Fill(dtAgregadoMatriz);
            con.Close();
            TotalAgregadoMatriz.Text = dtAgregadoMatriz.Rows[0][0].ToString();

            // Terceiros Matriz
            string sqlTerceiroMatriz = "select count(*) from tbveiculos where ativo_inativo = 'ATIVO' and tipoveiculo = 'TERCEIRO' and nucleo = 'MATRIZ'";
            SqlDataAdapter adptTerceiroMatriz = new SqlDataAdapter(sqlTerceiroMatriz, con);
            DataTable dtTerceiroMatriz = new DataTable();
            con.Open();
            adptTerceiroMatriz.Fill(dtTerceiroMatriz);
            con.Close();
            TotalTerceirosMatriz.Text = dtTerceiroMatriz.Rows[0][0].ToString();

            // clientes na região Centro-Oeste
            string sql4 = "select count(*) from tbtransportadoras where ativa_inativa = 'INATIVO' ";
            SqlDataAdapter adpt4 = new SqlDataAdapter(sql4, con);
            DataTable dt4 = new DataTable();
            con.Open();
            adpt4.Fill(dt4);
            con.Close();
            //veiculosInativos.Text = dt4.Rows[0][0].ToString();
        }
    }
}