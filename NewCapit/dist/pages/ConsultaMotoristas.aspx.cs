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
    public partial class ConsultaMotoristas : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString());
        private SqlCommand cmd;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) {

                ContagemMotorista();
                listarMotoristas();

            }
            
        }

        private void listarMotoristas()
        {

            var dataTable = DAL.ConMotoristas.FetchDataTable();
            if (dataTable.Rows.Count <= 0)
            {
                return;
            }            
            gvListMotoristas.DataSource = dataTable;
            gvListMotoristas.DataBind();

            gvListMotoristas.UseAccessibleHeader = true;
            gvListMotoristas.HeaderRow.TableSection = TableRowSection.TableHeader;
            gvListMotoristas.FooterRow.TableSection = TableRowSection.TableFooter;
        }

        public void ContagemMotorista()
        {
            // total de veiculos
            string sqlTotalVeiculos = "select count(*) from tbmotoristas where fl_exclusao is null";
            SqlDataAdapter adptTotalVeiculos = new SqlDataAdapter(sqlTotalVeiculos, con);
            DataTable dtTotalVeiculos = new DataTable();
            con.Open();
            adptTotalVeiculos.Fill(dtTotalVeiculos);
            con.Close();
            TotalCondutores.Text = dtTotalVeiculos.Rows[0][0].ToString();

            // total de colabodores ativos
            string sqlTotalVeiculosAtivos = "select count(*) from tbmotoristas where status = 'ATIVO' and fl_exclusao is null";
            SqlDataAdapter adptTotalVeiculosAtivos = new SqlDataAdapter(sqlTotalVeiculosAtivos, con);
            DataTable dtTotalVeiculosAtivos = new DataTable();
            con.Open();
            adptTotalVeiculosAtivos.Fill(dtTotalVeiculosAtivos);
            con.Close();
            LbAtivos.Text = dtTotalVeiculosAtivos.Rows[0][0].ToString();

            // total de colaboradores inativos
            string sqlTotalVeiculosInativos = "select count(*) from tbmotoristas where status = 'INATIVO' and fl_exclusao is null";
            SqlDataAdapter adptTotalVeiculosInativos = new SqlDataAdapter(sqlTotalVeiculosInativos, con);
            DataTable dtTotalVeiculosInativos = new DataTable();
            con.Open();
            adptTotalVeiculosInativos.Fill(dtTotalVeiculosInativos);
            con.Close();
            LbInativos.Text = dtTotalVeiculosInativos.Rows[0][0].ToString();


            // colaboradores Total
            string sqlFrota = "select count(*) from tbmotoristas where status = 'ATIVO' and tipomot = 'FUNCIONÁRIO'  and fl_exclusao is null";
            SqlDataAdapter adptFrota = new SqlDataAdapter(sqlFrota, con);
            DataTable dtFrota = new DataTable();
            con.Open();
            adptFrota.Fill(dtFrota);
            con.Close();
            lblTotalEmpregados.Text = dtFrota.Rows[0][0].ToString();

            // agregados 
            string sqlAgregadoMatriz = "select count(*) from tbmotoristas where status = 'ATIVO' and tipomot = 'AGREGADO' and fl_exclusao is null";
            SqlDataAdapter adptAgregadoMatriz = new SqlDataAdapter(sqlAgregadoMatriz, con);
            DataTable dtAgregadoMatriz = new DataTable();
            con.Open();
            adptAgregadoMatriz.Fill(dtAgregadoMatriz);
            con.Close();
            Agregados.Text = dtAgregadoMatriz.Rows[0][0].ToString();

            // terceiros
            string sql4 = "select count(*) from tbmotoristas where status = 'ATIVO' and tipomot = 'TERCEIRO' and fl_exclusao is null";
            SqlDataAdapter adpt4 = new SqlDataAdapter(sql4, con);
            DataTable dt4 = new DataTable();
            con.Open();
            adpt4.Fill(dt4);
            con.Close();
            Terceiros.Text = dt4.Rows[0][0].ToString();

            // Distribuição da frota por nucleo
            string sqlDistCNT = "SELECT count(*) FROM tbmotoristas where status = 'ATIVO' and tipomot='FUNCIONÁRIO' and nucleo = 'CNT' and fl_exclusao is null";
            SqlDataAdapter adptDistCNT = new SqlDataAdapter(sqlDistCNT, con);
            DataTable dtDistCNT = new DataTable();
            con.Open();
            adptDistCNT.Fill(dtDistCNT);
            con.Close();
            FrotaCNT.Text = dtDistCNT.Rows[0][0].ToString();

            string sqlDistMinas = "select count(*) from tbmotoristas where status = 'ATIVO' and tipomot = 'FUNCIONÁRIO' and nucleo = 'MG - MINAS GERAIS' and fl_exclusao is null";
            SqlDataAdapter adptDistMinas = new SqlDataAdapter(sqlDistMinas, con);
            DataTable dtDistMinas = new DataTable();
            con.Open();
            adptDistMinas.Fill(dtDistMinas);
            con.Close();
            FrotaMINAS.Text = dtDistMinas.Rows[0][0].ToString();

            string sqlDistFrota = "select count(*) from tbmotoristas where status = 'ATIVO' and tipomot = 'FUNCIONÁRIO' and nucleo = 'MATRIZ' and fl_exclusao is null";
            SqlDataAdapter adptDistFrota = new SqlDataAdapter(sqlDistFrota, con);
            DataTable dtDistFrota = new DataTable();
            con.Open();
            adptDistFrota.Fill(dtDistFrota);
            con.Close();
            FrotaMATRIZ.Text = dtDistFrota.Rows[0][0].ToString();

            string sqlDistIpiranga = "select count(*) from tbmotoristas where status = 'ATIVO' and tipomot = 'FUNCIONÁRIO' and nucleo = 'IPIRANGA' and fl_exclusao is null";
            SqlDataAdapter adptDistIpiranga = new SqlDataAdapter(sqlDistIpiranga, con);
            DataTable dtDistIpiranga = new DataTable();
            con.Open();
            adptDistIpiranga.Fill(dtDistIpiranga);
            con.Close();
            FrotaIpiranga.Text = dtDistIpiranga.Rows[0][0].ToString();

            string sqlDistPE = "select count(*) from tbmotoristas where status = 'ATIVO' and tipomot = 'FUNCIONÁRIO' and nucleo = 'PE - PERNAMBUCO' and fl_exclusao is null";
            SqlDataAdapter adptDistPE = new SqlDataAdapter(sqlDistPE, con);
            DataTable dtDistPE = new DataTable();
            con.Open();
            adptDistPE.Fill(dtDistPE);
            con.Close();
            FrotaPE.Text = dtDistPE.Rows[0][0].ToString();

            string sqlDistSBC = "select count(*) from tbmotoristas where status = 'ATIVO' and tipomot = 'FUNCIONÁRIO' and nucleo = 'SBC' and fl_exclusao is null";
            SqlDataAdapter adptDistSBC = new SqlDataAdapter(sqlDistSBC, con);
            DataTable dtDistSBC = new DataTable();
            con.Open();
            adptDistSBC.Fill(dtDistSBC);
            con.Close();
            FrotaSBC.Text = dtDistSBC.Rows[0][0].ToString();

            string sqlDistTaubate = "select count(*) from tbmotoristas where status = 'ATIVO' and tipomot = 'FUNCIONÁRIO' and nucleo = 'TAUBATE' and fl_exclusao is null";
            SqlDataAdapter adptDistTaubate = new SqlDataAdapter(sqlDistTaubate, con);
            DataTable dtDistTaubate = new DataTable();
            con.Open();
            adptDistTaubate.Fill(dtDistTaubate);
            con.Close();
            FrotaTaubate.Text = dtDistTaubate.Rows[0][0].ToString();

            string sqlDistSC = "select count(*) from tbmotoristas where status = 'ATIVO' and tipomot = 'FUNCIONÁRIO' and nucleo = 'SAO CARLOS' and fl_exclusao is null";
            SqlDataAdapter adptDistSC = new SqlDataAdapter(sqlDistSC, con);
            DataTable dtDistSC = new DataTable();
            con.Open();
            adptDistSC.Fill(dtDistSC);
            con.Close();
            FrotaSC.Text = dtDistSC.Rows[0][0].ToString();

            string sqlDistPR = "select count(*) from tbmotoristas where status = 'ATIVO' and tipomot = 'FUNCIONÁRIO' and nucleo = 'PR - PARANA' and fl_exclusao is null";
            SqlDataAdapter adptDistPR = new SqlDataAdapter(sqlDistPR, con);
            DataTable dtDistPR = new DataTable();
            con.Open();
            adptDistPR.Fill(dtDistPR);
            con.Close();
            FrotaPR.Text = dtDistPR.Rows[0][0].ToString();

            // Distribuição de agregados por nucleo
            string sqlDistAgCNT = "SELECT count(*) FROM tbmotoristas where status = 'ATIVO' and tipomot='AGREGADO' and nucleo = 'CNT' and fl_exclusao is null";
            SqlDataAdapter adptDistAgCNT = new SqlDataAdapter(sqlDistAgCNT, con);
            DataTable dtDistAgCNT = new DataTable();
            con.Open();
            adptDistAgCNT.Fill(dtDistAgCNT);
            con.Close();
            AgCNT.Text = dtDistAgCNT.Rows[0][0].ToString();

            string sqlDistAgMinas = "select count(*) from tbmotoristas where status = 'ATIVO' and tipomot = 'AGREGADO' and nucleo = 'MG - MINAS GERAIS' and fl_exclusao is null";
            SqlDataAdapter adptDistAgMinas = new SqlDataAdapter(sqlDistAgMinas, con);
            DataTable dtDistAgMinas = new DataTable();
            con.Open();
            adptDistAgMinas.Fill(dtDistAgMinas);
            con.Close();
            AgMG.Text = dtDistAgMinas.Rows[0][0].ToString();

            string sqlDistAgMatriz = "select count(*) from tbmotoristas where status = 'ATIVO' and tipomot = 'AGREGADO' and nucleo = 'MATRIZ' and fl_exclusao is null";
            SqlDataAdapter adptDistAgMatriz = new SqlDataAdapter(sqlDistAgMatriz, con);
            DataTable dtDistAgMatriz = new DataTable();
            con.Open();
            adptDistAgMatriz.Fill(dtDistAgMatriz);
            con.Close();
            AgMatriz.Text = dtDistAgMatriz.Rows[0][0].ToString();

            string sqlDistAgIpiranga = "select count(*) from tbmotoristas where status = 'ATIVO' and tipomot = 'AGREGADO' and nucleo = 'IPIRANGA' and fl_exclusao is null";
            SqlDataAdapter adptDistAgIpiranga = new SqlDataAdapter(sqlDistAgIpiranga, con);
            DataTable dtDistAgIpiranga = new DataTable();
            con.Open();
            adptDistAgIpiranga.Fill(dtDistAgIpiranga);
            con.Close();
            AgIpiranga.Text = dtDistAgIpiranga.Rows[0][0].ToString();

            string sqlDistAgPE = "select count(*) from tbmotoristas where status = 'ATIVO' and tipomot = 'AGREGADO' and nucleo = 'PE - PERNAMBUCO' and fl_exclusao is null";
            SqlDataAdapter adptDistAgPE = new SqlDataAdapter(sqlDistAgPE, con);
            DataTable dtDistAgPE = new DataTable();
            con.Open();
            adptDistAgPE.Fill(dtDistAgPE);
            con.Close();
            AgPE.Text = dtDistAgPE.Rows[0][0].ToString();

            string sqlDistAgSBC = "select count(*) from tbmotoristas where status = 'ATIVO' and tipomot = 'AGREGADO' and nucleo = 'SBC' and fl_exclusao is null";
            SqlDataAdapter adptDistAgSBC = new SqlDataAdapter(sqlDistAgSBC, con);
            DataTable dtDistAgSBC = new DataTable();
            con.Open();
            adptDistAgSBC.Fill(dtDistAgSBC);
            con.Close();
            AgSBC.Text = dtDistAgSBC.Rows[0][0].ToString();

            string sqlDistAgTaubate = "select count(*) from tbmotoristas where status = 'ATIVO' and tipomot = 'AGREGADO' and nucleo = 'TAUBATE' and fl_exclusao is null";
            SqlDataAdapter adptDistAgTaubate = new SqlDataAdapter(sqlDistAgTaubate, con);
            DataTable dtDistAgTaubate = new DataTable();
            con.Open();
            adptDistAgTaubate.Fill(dtDistAgTaubate);
            con.Close();
            AgTaubate.Text = dtDistAgTaubate.Rows[0][0].ToString();

            string sqlDistAgSC = "select count(*) from tbmotoristas where status = 'ATIVO' and tipomot = 'AGREGADO' and nucleo = 'SAO CARLOS' and fl_exclusao is null";
            SqlDataAdapter adptDistAgSC = new SqlDataAdapter(sqlDistAgSC, con);
            DataTable dtDistAgSC = new DataTable();
            con.Open();
            adptDistAgSC.Fill(dtDistAgSC);
            con.Close();
            AgSC.Text = dtDistAgSC.Rows[0][0].ToString();

            string sqlDistAgPR = "select count(*) from tbmotoristas where status = 'ATIVO' and tipomot = 'AGREGADO' and nucleo = 'PR - PARANA' and fl_exclusao is null";
            SqlDataAdapter adptDistAgPR = new SqlDataAdapter(sqlDistAgPR, con);
            DataTable dtDistAgPR = new DataTable();
            con.Open();
            adptDistAgPR.Fill(dtDistAgPR);
            con.Close();
            AgPR.Text = dtDistAgPR.Rows[0][0].ToString();

            // Distribuição de terceiros por nucleo
            string sqlDistTCNT = "SELECT count(*) FROM tbmotoristas where status = 'ATIVO' and tipomot='TERCEIRO' and nucleo = 'CNT' and fl_exclusao is null";
            SqlDataAdapter adptDistTCNT = new SqlDataAdapter(sqlDistTCNT, con);
            DataTable dtDistTCNT = new DataTable();
            con.Open();
            adptDistTCNT.Fill(dtDistTCNT);
            con.Close();
            TCNT.Text = dtDistTCNT.Rows[0][0].ToString();

            string sqlDistTMG = "select count(*) from tbmotoristas where status = 'ATIVO' and tipomot = 'TERCEIRO' and nucleo = 'MG - MINAS GERAIS' and fl_exclusao is null";
            SqlDataAdapter adptDistTMG = new SqlDataAdapter(sqlDistTMG, con);
            DataTable dtDistTMG = new DataTable();
            con.Open();
            adptDistTMG.Fill(dtDistTMG);
            con.Close();
            TMG.Text = dtDistTMG.Rows[0][0].ToString();

            string sqlDistTMatriz = "select count(*) from tbmotoristas where status = 'ATIVO' and tipomot = 'TERCEIRO' and nucleo = 'MATRIZ' and fl_exclusao is null";
            SqlDataAdapter adptDistTMatriz = new SqlDataAdapter(sqlDistTMatriz, con);
            DataTable dtDistTMatriz = new DataTable();
            con.Open();
            adptDistTMatriz.Fill(dtDistTMatriz);
            con.Close();
            TMatriz.Text = dtDistTMatriz.Rows[0][0].ToString();

            string sqlDistTIpiranga = "select count(*) from tbmotoristas where status = 'ATIVO' and tipomot = 'TERCEIRO' and nucleo = 'IPIRANGA' and fl_exclusao is null";
            SqlDataAdapter adptDistTIpiranga = new SqlDataAdapter(sqlDistTIpiranga, con);
            DataTable dtDistTIpiranga = new DataTable();
            con.Open();
            adptDistTIpiranga.Fill(dtDistTIpiranga);
            con.Close();
            TIpiranga.Text = dtDistTIpiranga.Rows[0][0].ToString();

            string sqlDistTPE = "select count(*) from tbmotoristas where status = 'ATIVO' and tipomot = 'TERCEIRO' and nucleo = 'PE - PERNAMBUCO' and fl_exclusao is null";
            SqlDataAdapter adptDistTPE = new SqlDataAdapter(sqlDistTPE, con);
            DataTable dtDistTPE = new DataTable();
            con.Open();
            adptDistTPE.Fill(dtDistTPE);
            con.Close();
            TPE.Text = dtDistTPE.Rows[0][0].ToString();

            string sqlDistTSBC = "select count(*) from tbmotoristas where status = 'ATIVO' and tipomot = 'TERCEIRO' and nucleo = 'SBC' and fl_exclusao is null";
            SqlDataAdapter adptDistTSBC = new SqlDataAdapter(sqlDistTSBC, con);
            DataTable dtDistTSBC = new DataTable();
            con.Open();
            adptDistTSBC.Fill(dtDistTSBC);
            con.Close();
            TSBC.Text = dtDistTSBC.Rows[0][0].ToString();

            string sqlDistTTaubate = "select count(*) from tbmotoristas where status = 'ATIVO' and tipomot = 'TERCEIRO' and nucleo = 'TAUBATE' and fl_exclusao is null";
            SqlDataAdapter adptDistTTaubate = new SqlDataAdapter(sqlDistTTaubate, con);
            DataTable dtDistTTaubate = new DataTable();
            con.Open();
            adptDistTTaubate.Fill(dtDistTTaubate);
            con.Close();
            TTaubate.Text = dtDistTTaubate.Rows[0][0].ToString();

            string sqlDistTSC = "select count(*) from tbmotoristas where status = 'ATIVO' and tipomot = 'TERCEIRO' and nucleo = 'SAO CARLOS' and fl_exclusao is null";
            SqlDataAdapter adptDistTSC = new SqlDataAdapter(sqlDistTSC, con);
            DataTable dtDistTSC = new DataTable();
            con.Open();
            adptDistTSC.Fill(dtDistTSC);
            con.Close();
            TSC.Text = dtDistTSC.Rows[0][0].ToString();

            string sqlDistTPR = "select count(*) from tbmotoristas where status = 'ATIVO' and tipomot = 'TERCEIRO' and nucleo = 'PR - PARANA' and fl_exclusao is null";
            SqlDataAdapter adptDistTPR = new SqlDataAdapter(sqlDistTPR, con);
            DataTable dtDistTPR = new DataTable();
            con.Open();
            adptDistTPR.Fill(dtDistTPR);
            con.Close();
            TPR.Text = dtDistTPR.Rows[0][0].ToString();


        }
    }
}