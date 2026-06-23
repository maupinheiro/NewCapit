using DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NewCapit.dist.pages
{
    public partial class Consulta_Agregados : PaginaBase
    {
        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString());
        
        protected void Page_Load(object sender, EventArgs e)
        {
            ContagemAgregados();
            if (!IsPostBack)
            {
                int idUsuarioLogado = Convert.ToInt32(Session["CodUsuario"]);
                string telaAtual = System.IO.Path.GetFileName(Request.Url.AbsolutePath);

                // 2. Busca no banco de dados (Apenas 1 vez no carregamento)
                VerificarBotoesPagina(btnInserir: lnkNovoCadastro);
                AllDataAgregados();
            }
            if (Session["UsuarioLogado"] != null)
            {
                string nomeUsuario = Session["UsuarioLogado"].ToString();
                var lblUsuario = nomeUsuario;



            }
            else
            {
                var lblUsuario = "<Usuário>";

                Response.Redirect("Login.aspx");
            }
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
            string sql3 = "select count(*) from tbtransportadoras where ativa_inativa = 'ATIVO' and tipo = 'AGREGADO' ";
            SqlDataAdapter adpt3 = new SqlDataAdapter(sql3, con);
            DataTable dt3 = new DataTable();
            con.Open();
            adpt3.Fill(dt3);
            con.Close();
            veiculosAtivos.Text = dt3.Rows[0][0].ToString();

            // clientes na região Centro-Oeste
            string sql4 = "select count(*) from tbtransportadoras where ativa_inativa = 'INATIVO' and tipo = 'AGREGADO' ";
            SqlDataAdapter adpt4 = new SqlDataAdapter(sql4, con);
            DataTable dt4 = new DataTable();
            con.Open();
            adpt4.Fill(dt4);
            con.Close();
            veiculosInativos.Text = dt4.Rows[0][0].ToString();

            // clientes na região Sudeste
            string sql5 = "select count(*) from tbtransportadoras where ativa_inativa = 'ATIVO' and tipo = 'TERCEIRO' ";
            SqlDataAdapter adpt5 = new SqlDataAdapter(sql5, con);
            DataTable dt5 = new DataTable();
            con.Open();
            adpt5.Fill(dt5);
            con.Close();
            lbAtivosTerceiros.Text = dt5.Rows[0][0].ToString();

            // clientes na região Centro-Oeste
            string sql6 = "select count(*) from tbtransportadoras where ativa_inativa = 'INATIVO' and tipo = 'TERCEIRO' ";
            SqlDataAdapter adpt6 = new SqlDataAdapter(sql6, con);
            DataTable dt6 = new DataTable();
            con.Open();
            adpt6.Fill(dt6);
            con.Close();
            lbInativosTerceiros.Text = dt6.Rows[0][0].ToString();
        }

        private void AllDataAgregados()
        {
            var dataTable = DAL.ConAgregados.FetchDataTable();
            if (dataTable.Rows.Count <= 0)
            {
                return;
            }
            gvListAgregados.DataSource = dataTable;
            gvListAgregados.DataBind();

            //gvListAgregados.UseAccessibleHeader = true;
            //gvListAgregados.HeaderRow.TableSection = TableRowSection.TableHeader;
            //gvListAgregados.FooterRow.TableSection = TableRowSection.TableFooter;

        }
        protected void gvListAgregados_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvListAgregados.PageIndex = e.NewPageIndex;
            AllDataAgregados();  // Método para recarregar os dados no GridView
        }
       
        protected void Editar(object sender, EventArgs e)
        {
            using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
            {
                string id = gvListAgregados.DataKeys[row.RowIndex].Value.ToString();

                Response.Redirect("Frm_AltTransportadoras.aspx?id=" + id);
            }            
        }
        private void AllData(string searchTerm = "")
        {
            var dataTable = DAL.ConAgregados.FetchDataTable2(searchTerm);
            if (dataTable.Rows.Count <= 0)
            {
                gvListAgregados.DataSource = null;
                gvListAgregados.DataBind();
                return;
            }

            gvListAgregados.DataSource = dataTable;
            gvListAgregados.DataBind();
        }

        protected void myInput_TextChanged(object sender, EventArgs e)
        {
            string searchTerm = myInput.Text.Trim();
            AllData(searchTerm);
        }

        protected void lnkNovoCadastro_Click(object sender, EventArgs e)
        {
            Response.Redirect("Frm_CadTransportadoras.aspx");
        }

        protected void gvListAgregados_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            VerificarBotoesGrid(e, idBtnEditar: "lnkEditar                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               ", idBtnExcluir: "btnExcluirLinha");
        }
    }

}