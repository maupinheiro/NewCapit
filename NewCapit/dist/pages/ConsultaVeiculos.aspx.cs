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
using System.Collections;
using System.Web.Script.Serialization;


namespace NewCapit
{
    public partial class ConsultaVeiculos : System.Web.UI.Page
    {
       SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString());
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                ContagemVeiculo();
                AllDataVeiculos();
            }



        }
        public void ContagemVeiculo()
        {
            // total de veiculos
            string sqlTotalVeiculos = "select count(*) from tbveiculos";
            SqlDataAdapter adptTotalVeiculos = new SqlDataAdapter(sqlTotalVeiculos, con);
            DataTable dtTotalVeiculos = new DataTable();
            con.Open();
            adptTotalVeiculos.Fill(dtTotalVeiculos);
            con.Close();
            TotalVeiculos.Text = dtTotalVeiculos.Rows[0][0].ToString();

            // total de veiculos ativos
            string sqlTotalVeiculosAtivos = "select count(*) from tbveiculos where ativo_inativo = 'ATIVO'";
            SqlDataAdapter adptTotalVeiculosAtivos = new SqlDataAdapter(sqlTotalVeiculosAtivos, con);
            DataTable dtTotalVeiculosAtivos = new DataTable();
            con.Open();
            adptTotalVeiculosAtivos.Fill(dtTotalVeiculosAtivos);
            con.Close();
            LbAtivos.Text = dtTotalVeiculosAtivos.Rows[0][0].ToString();

            // total de veiculos inativos
            string sqlTotalVeiculosInativos = "select count(*) from tbveiculos where ativo_inativo = 'INATIVO'";
            SqlDataAdapter adptTotalVeiculosInativos = new SqlDataAdapter(sqlTotalVeiculosInativos, con);
            DataTable dtTotalVeiculosInativos = new DataTable();
            con.Open();
            adptTotalVeiculosInativos.Fill(dtTotalVeiculosInativos);
            con.Close();
            LbInativos.Text = dtTotalVeiculosInativos.Rows[0][0].ToString();


            // Frota Total
            string sqlFrota = "select count(*) from tbveiculos where ativo_inativo = 'ATIVO' and tipoveiculo = 'FROTA' and fl_exclusao is null  ";
            SqlDataAdapter adptFrota = new SqlDataAdapter(sqlFrota, con);
            DataTable dtFrota = new DataTable();
            con.Open();
            adptFrota.Fill(dtFrota);
            con.Close();
            TotalFrota.Text = dtFrota.Rows[0][0].ToString();

            // agregados 
            string sqlAgregadoMatriz = "select count(*) from tbveiculos where ativo_inativo = 'ATIVO' and tipoveiculo = 'AGREGADO' and fl_exclusao is null";
            SqlDataAdapter adptAgregadoMatriz = new SqlDataAdapter(sqlAgregadoMatriz, con);
            DataTable dtAgregadoMatriz = new DataTable();
            con.Open();
            adptAgregadoMatriz.Fill(dtAgregadoMatriz);
            con.Close();
            TotalAgregados.Text = dtAgregadoMatriz.Rows[0][0].ToString();

            // terceiros
            string sql4 = "select count(*) from tbveiculos where ativo_inativo = 'ATIVO' and tipoveiculo = 'TERCEIRO' and fl_exclusao is null";
            SqlDataAdapter adpt4 = new SqlDataAdapter(sql4, con);
            DataTable dt4 = new DataTable();
            con.Open();
            adpt4.Fill(dt4);
            con.Close();
            TotalTerceiros.Text = dt4.Rows[0][0].ToString();

            // Distribuição da frota por nucleo
            string sqlDistCNT = "SELECT count(*) FROM tbveiculos where ativo_inativo = 'ATIVO' and tipoveiculo='FROTA' and nucleo = 'CNT (CC)' and fl_exclusao is null ";
            SqlDataAdapter adptDistCNT = new SqlDataAdapter(sqlDistCNT, con);
            DataTable dtDistCNT = new DataTable();
            con.Open();
            adptDistCNT.Fill(dtDistCNT);
            con.Close();
            FrotaCNT.Text = dtDistCNT.Rows[0][0].ToString();

            string sqlDistMinas = "select count(*) from tbveiculos where ativo_inativo = 'ATIVO' and tipoveiculo = 'FROTA' and nucleo = 'MG - MINAS GERAIS' and fl_exclusao is null";
            SqlDataAdapter adptDistMinas = new SqlDataAdapter(sqlDistMinas, con);
            DataTable dtDistMinas = new DataTable();
            con.Open();
            adptDistMinas.Fill(dtDistMinas);
            con.Close();
            FrotaMINAS.Text = dtDistMinas.Rows[0][0].ToString();

            string sqlDistFrota = "select count(*) from tbveiculos where ativo_inativo = 'ATIVO' and tipoveiculo = 'FROTA' and nucleo = 'MATRIZ' and fl_exclusao is null";
            SqlDataAdapter adptDistFrota = new SqlDataAdapter(sqlDistFrota, con);
            DataTable dtDistFrota = new DataTable();
            con.Open();
            adptDistFrota.Fill(dtDistFrota);
            con.Close();
            FrotaMATRIZ.Text = dtDistFrota.Rows[0][0].ToString();

            string sqlDistIpiranga = "select count(*) from tbveiculos where ativo_inativo = 'ATIVO' and tipoveiculo = 'FROTA' and nucleo = 'IPIRANGA' and fl_exclusao is null";
            SqlDataAdapter adptDistIpiranga = new SqlDataAdapter(sqlDistIpiranga, con);
            DataTable dtDistIpiranga = new DataTable();
            con.Open();
            adptDistIpiranga.Fill(dtDistIpiranga);
            con.Close();
            FrotaIpiranga.Text = dtDistIpiranga.Rows[0][0].ToString();

            string sqlDistPE = "select count(*) from tbveiculos where ativo_inativo = 'ATIVO' and tipoveiculo = 'FROTA' and nucleo = 'PE - PERNAMBUCO' and fl_exclusao is null";
            SqlDataAdapter adptDistPE = new SqlDataAdapter(sqlDistPE, con);
            DataTable dtDistPE = new DataTable();
            con.Open();
            adptDistPE.Fill(dtDistPE);
            con.Close();
            FrotaPE.Text = dtDistPE.Rows[0][0].ToString();

            string sqlDistSBC = "select count(*) from tbveiculos where ativo_inativo = 'ATIVO' and tipoveiculo = 'FROTA' and nucleo = 'ANCHIETA' and fl_exclusao is null";
            SqlDataAdapter adptDistSBC = new SqlDataAdapter(sqlDistSBC, con);
            DataTable dtDistSBC = new DataTable();
            con.Open();
            adptDistSBC.Fill(dtDistSBC);
            con.Close();
            FrotaSBC.Text = dtDistSBC.Rows[0][0].ToString();

            string sqlDistTaubate = "select count(*) from tbveiculos where ativo_inativo = 'ATIVO' and tipoveiculo = 'FROTA' and nucleo = 'TAUBATE' and fl_exclusao is null";
            SqlDataAdapter adptDistTaubate = new SqlDataAdapter(sqlDistTaubate, con);
            DataTable dtDistTaubate = new DataTable();
            con.Open();
            adptDistTaubate.Fill(dtDistTaubate);
            con.Close();
            FrotaTaubate.Text = dtDistTaubate.Rows[0][0].ToString();

            string sqlDistSC = "select count(*) from tbveiculos where ativo_inativo = 'ATIVO' and tipoveiculo = 'FROTA' and nucleo = 'SAO CARLOS' and fl_exclusao is null";
            SqlDataAdapter adptDistSC = new SqlDataAdapter(sqlDistSC, con);
            DataTable dtDistSC = new DataTable();
            con.Open();
            adptDistSC.Fill(dtDistSC);
            con.Close();
            FrotaSC.Text = dtDistSC.Rows[0][0].ToString();

            string sqlDistPR = "select count(*) from tbveiculos where ativo_inativo = 'ATIVO' and tipoveiculo = 'FROTA' and nucleo = 'PR - PARANA' and fl_exclusao is null";
            SqlDataAdapter adptDistPR = new SqlDataAdapter(sqlDistPR, con);
            DataTable dtDistPR = new DataTable();
            con.Open();
            adptDistPR.Fill(dtDistPR);
            con.Close();
            FrotaPR.Text = dtDistPR.Rows[0][0].ToString();


            // Distribuição de terceiros por nucleo
            string sqlDistTerCNT = "SELECT count(*) FROM tbveiculos where ativo_inativo = 'ATIVO' and tipoveiculo='TERCEIRO' and nucleo = 'CNT (CC)' and fl_exclusao is null";
            SqlDataAdapter adptDistTerCNT = new SqlDataAdapter(sqlDistTerCNT, con);
            DataTable dtDistTerCNT = new DataTable();
            con.Open();
            adptDistTerCNT.Fill(dtDistTerCNT);
            con.Close();
            lbTerCNT.Text = dtDistTerCNT.Rows[0][0].ToString();

            string sqlDistTerMinas = "select count(*) from tbveiculos where ativo_inativo = 'ATIVO' and tipoveiculo = 'TERCEIRO' and nucleo = 'MG - MINAS GERAIS' and fl_exclusao is null";
            SqlDataAdapter adptDistTerMinas = new SqlDataAdapter(sqlDistTerMinas, con);
            DataTable dtDistTerMinas = new DataTable();
            con.Open();
            adptDistTerMinas.Fill(dtDistTerMinas);
            con.Close();
            lbTerMG.Text = dtDistTerMinas.Rows[0][0].ToString();

            string sqlDistTerMatriz = "select count(*) from tbveiculos where ativo_inativo = 'ATIVO' and tipoveiculo = 'TERCEIRO' and nucleo = 'MATRIZ' and fl_exclusao is null";
            SqlDataAdapter adptDistTerMatriz = new SqlDataAdapter(sqlDistTerMatriz, con);
            DataTable dtDistTerMatriz = new DataTable();
            con.Open();
            adptDistTerMatriz.Fill(dtDistTerMatriz);
            con.Close();
            lbTerMatriz.Text = dtDistTerMatriz.Rows[0][0].ToString();

            string sqlDistTerIpiranga = "select count(*) from tbveiculos where ativo_inativo = 'ATIVO' and tipoveiculo = 'TERCEIRO' and nucleo = 'IPIRANGA' and fl_exclusao is null";
            SqlDataAdapter adptDistTerIpiranga = new SqlDataAdapter(sqlDistTerIpiranga, con);
            DataTable dtDistTerIpiranga = new DataTable();
            con.Open();
            adptDistTerIpiranga.Fill(dtDistTerIpiranga);
            con.Close();
            lbTerIpiranga.Text = dtDistTerIpiranga.Rows[0][0].ToString();

            string sqlDistTerPE = "select count(*) from tbveiculos where ativo_inativo = 'ATIVO' and tipoveiculo = 'TERCEIRO' and nucleo = 'PE - PERNAMBUCO' and fl_exclusao is null";
            SqlDataAdapter adptDistTerPE = new SqlDataAdapter(sqlDistTerPE, con);
            DataTable dtDistTerPE = new DataTable();
            con.Open();
            adptDistTerPE.Fill(dtDistTerPE);
            con.Close();
            lbTerPE.Text = dtDistTerPE.Rows[0][0].ToString();

            string sqlDistTerSBC = "select count(*) from tbveiculos where ativo_inativo = 'ATIVO' and tipoveiculo = 'TERCEIRO' and nucleo = 'ANCHIETA' and fl_exclusao is null";
            SqlDataAdapter adptDistTerSBC = new SqlDataAdapter(sqlDistTerSBC, con);
            DataTable dtDistTerSBC = new DataTable();
            con.Open();
            adptDistTerSBC.Fill(dtDistTerSBC);
            con.Close();
            lbTerSBC.Text = dtDistTerSBC.Rows[0][0].ToString();

            string sqlDistTerTaubate = "select count(*) from tbveiculos where ativo_inativo = 'ATIVO' and tipoveiculo = 'TERCEIRO' and nucleo = 'TAUBATE' and fl_exclusao is null";
            SqlDataAdapter adptDistTerTaubate = new SqlDataAdapter(sqlDistTerTaubate, con);
            DataTable dtDistTerTaubate = new DataTable();
            con.Open();
            adptDistTerTaubate.Fill(dtDistTerTaubate);
            con.Close();
            lbTerTaubate.Text = dtDistTerTaubate.Rows[0][0].ToString();

            string sqlDistTerSCarlos = "select count(*) from tbveiculos where ativo_inativo = 'ATIVO' and tipoveiculo = 'TERCEIRO' and nucleo = 'SAO CARLOS' and fl_exclusao is null";
            SqlDataAdapter adptDistTerSCarlos = new SqlDataAdapter(sqlDistTerSCarlos, con);
            DataTable dtDistTerSCarlos = new DataTable();
            con.Open();
            adptDistTerSCarlos.Fill(dtDistTerSCarlos);
            con.Close();
            lbTerSCarlos.Text = dtDistTerSCarlos.Rows[0][0].ToString();

            string sqlDistTerPR = "select count(*) from tbveiculos where ativo_inativo = 'ATIVO' and tipoveiculo = 'TERCEIRO' and nucleo = 'PR - PARANA' and fl_exclusao is null";
            SqlDataAdapter adptDistTerPR = new SqlDataAdapter(sqlDistTerPR, con);
            DataTable dtDistTerPR = new DataTable();
            con.Open();
            adptDistTerPR.Fill(dtDistTerPR);
            con.Close();
            lbTerPR.Text = dtDistTerPR.Rows[0][0].ToString();


            // Distribuição de agregados por nucleo
            string sqlDistAgCNT = "SELECT count(*) FROM tbveiculos where ativo_inativo = 'ATIVO' and tipoveiculo='AGREGADO' and nucleo = 'CNT (CC)' and fl_exclusao is null";
            SqlDataAdapter adptDistAgCNT = new SqlDataAdapter(sqlDistAgCNT, con);
            DataTable dtDistAgCNT = new DataTable();
            con.Open();
            adptDistAgCNT.Fill(dtDistAgCNT);
            con.Close();
            AgCNT.Text = dtDistAgCNT.Rows[0][0].ToString();

            string sqlDistAgMinas = "select count(*) from tbveiculos where ativo_inativo = 'ATIVO' and tipoveiculo = 'AGREGADO' and nucleo = 'MG - MINAS GERAIS' and fl_exclusao is null";
            SqlDataAdapter adptDistAgMinas = new SqlDataAdapter(sqlDistAgMinas, con);
            DataTable dtDistAgMinas = new DataTable();
            con.Open();
            adptDistAgMinas.Fill(dtDistAgMinas);
            con.Close();
            AgMG.Text = dtDistAgMinas.Rows[0][0].ToString();

            string sqlDistAgMatriz = "select count(*) from tbveiculos where ativo_inativo = 'ATIVO' and tipoveiculo = 'AGREGADO' and nucleo = 'MATRIZ' and fl_exclusao is null";
            SqlDataAdapter adptDistAgMatriz = new SqlDataAdapter(sqlDistAgMatriz, con);
            DataTable dtDistAgMatriz = new DataTable();
            con.Open();
            adptDistAgMatriz.Fill(dtDistAgMatriz);
            con.Close();
            AgMatriz.Text = dtDistAgMatriz.Rows[0][0].ToString();

            string sqlDistAgIpiranga = "select count(*) from tbveiculos where ativo_inativo = 'ATIVO' and tipoveiculo = 'AGREGADO' and nucleo = 'IPIRANGA' and fl_exclusao is null";
            SqlDataAdapter adptDistAgIpiranga = new SqlDataAdapter(sqlDistAgIpiranga, con);
            DataTable dtDistAgIpiranga = new DataTable();
            con.Open();
            adptDistAgIpiranga.Fill(dtDistAgIpiranga);
            con.Close();
            AgIpiranga.Text = dtDistAgIpiranga.Rows[0][0].ToString();

            string sqlDistAgPE = "select count(*) from tbveiculos where ativo_inativo = 'ATIVO' and tipoveiculo = 'AGREGADO' and nucleo = 'PE - PERNAMBUCO' and fl_exclusao is null";
            SqlDataAdapter adptDistAgPE = new SqlDataAdapter(sqlDistAgPE, con);
            DataTable dtDistAgPE = new DataTable();
            con.Open();
            adptDistAgPE.Fill(dtDistAgPE);
            con.Close();
            AgPE.Text = dtDistAgPE.Rows[0][0].ToString();

            string sqlDistAgSBC = "select count(*) from tbveiculos where ativo_inativo = 'ATIVO' and tipoveiculo = 'AGREGADO' and nucleo = 'ANCHIETA' and fl_exclusao is null";
            SqlDataAdapter adptDistAgSBC = new SqlDataAdapter(sqlDistAgSBC, con);
            DataTable dtDistAgSBC = new DataTable();
            con.Open();
            adptDistAgSBC.Fill(dtDistAgSBC);
            con.Close();
            AgSBC.Text = dtDistAgSBC.Rows[0][0].ToString();

            string sqlDistAgTaubate = "select count(*) from tbveiculos where ativo_inativo = 'ATIVO' and tipoveiculo = 'AGREGADO' and nucleo = 'TAUBATE' and fl_exclusao is null";
            SqlDataAdapter adptDistAgTaubate = new SqlDataAdapter(sqlDistAgTaubate, con);
            DataTable dtDistAgTaubate = new DataTable();
            con.Open();
            adptDistAgTaubate.Fill(dtDistAgTaubate);
            con.Close();
            AgTaubate.Text = dtDistAgTaubate.Rows[0][0].ToString();

            string sqlDistAgSC = "select count(*) from tbveiculos where ativo_inativo = 'ATIVO' and tipoveiculo = 'AGREGADO' and nucleo = 'SAO CARLOS' and fl_exclusao is null";
            SqlDataAdapter adptDistAgSC = new SqlDataAdapter(sqlDistAgSC, con);
            DataTable dtDistAgSC = new DataTable();
            con.Open();
            adptDistAgSC.Fill(dtDistAgSC);
            con.Close();
            AgSC.Text = dtDistAgSC.Rows[0][0].ToString();

            string sqlDistAgPR = "select count(*) from tbveiculos where ativo_inativo = 'ATIVO' and tipoveiculo = 'AGREGADO' and nucleo = 'PR - PARANA' and fl_exclusao is null";
            SqlDataAdapter adptDistAgPR = new SqlDataAdapter(sqlDistAgPR, con);
            DataTable dtDistAgPR = new DataTable();
            con.Open();
            adptDistAgPR.Fill(dtDistAgPR);
            con.Close();
            AgPR.Text = dtDistAgPR.Rows[0][0].ToString();



        }

        private void AllDataVeiculos(string searchTerm = "")
        {
            // var dataTable = DAL.ConVeiculos.FetchDataTable();
            var dataTable = DAL.ConVeiculos.FetchDataTable2(searchTerm);
            if (dataTable.Rows.Count <= 0)
            {
                gvVeiculos.DataSource = null;
                gvVeiculos.DataBind();
                return;
            }
            gvVeiculos.DataSource = dataTable;
            gvVeiculos.DataBind();

            //gvVeiculos.UseAccessibleHeader = true;
            //gvVeiculos.HeaderRow.TableSection = TableRowSection.TableHeader;
            //gvVeiculos.FooterRow.TableSection = TableRowSection.TableFooter;

        }
        protected void gvVeiculos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvVeiculos.PageIndex = e.NewPageIndex;
            AllDataVeiculos();  // Método para recarregar os dados no GridView

            //string searchTerm = myInput.Text.Trim();
            //AllDataVeiculos(searchTerm);

        }
        protected void Editar(object sender, EventArgs e)
        {
            //LinkButton btn = (LinkButton)sender;
            //string id = btn.CommandArgument;

            //Response.Redirect("Frm_AltVeiculos.aspx?id=" + id);


            using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
            {
                string id = gvVeiculos.DataKeys[row.RowIndex].Value.ToString();

                Response.Redirect("Frm_AltVeiculos.aspx?id=" + id);
            }

        }

        private void AllData(string searchTerm)
        {
            var dataTable = DAL.ConVeiculos.FetchDataTable2(searchTerm);
            if (dataTable.Rows.Count <= 0)
            {
                gvVeiculos.DataSource = null;
                gvVeiculos.DataBind();
                return;
            }

            gvVeiculos.DataSource = dataTable;
            gvVeiculos.DataBind();
        }

        protected void myInput_TextChanged(object sender, EventArgs e)
        {
            string searchTerm = myInput.Text.Trim();
            AllData(searchTerm);
        }
    }
}