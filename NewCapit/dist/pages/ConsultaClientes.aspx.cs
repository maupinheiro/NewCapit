﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Web.Services.Description;
using System.Configuration;
using System.Drawing;
using System.Web.UI.HtmlControls;
using System.Web.Configuration;
using System.Runtime.Remoting;
using System.Web.Script.Serialization;

namespace NewCapit
{
    public partial class ConsultaClientes : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conexao"].ToString());

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                AllData();
                CarregaRegioes();
            }

        }


        private void AllData()
        {
            var dataTable = DAL.codCliente.FetchDataTable();
            if (dataTable.Rows.Count <= 0)
            {
                return;
            }
            gvList.DataSource = dataTable;
            gvList.DataBind();

            //gvList.UseAccessibleHeader = true;
            //gvList.HeaderRow.TableSection = TableRowSection.TableHeader;
            //gvList.FooterRow.TableSection = TableRowSection.TableFooter;

        }

        public void CarregaRegioes()
        {
            // total de clientes ativos e inativos
            string sqlTotal = "select count(*) from tbclientes where fl_exclusao is null";
            SqlDataAdapter adptTotal = new SqlDataAdapter(sqlTotal, con);
            DataTable dtTotal = new DataTable();
            con.Open();
            adptTotal.Fill(dtTotal);
            con.Close();
            TotalClientes.Text = dtTotal.Rows[0][0].ToString();

            // clientes na região norte
            string sql = "select count(*) from tbclientes where regiao = 'NORTE' ";
            SqlDataAdapter adpt = new SqlDataAdapter(sql, con);
            DataTable dt = new DataTable();
            con.Open();
            adpt.Fill(dt);
            con.Close();
            Norte.Text = dt.Rows[0][0].ToString();

            // clientes na região nordeste
            string sql1 = "select count(*) from tbclientes where regiao = 'NORDESTE' ";
            SqlDataAdapter adpt1 = new SqlDataAdapter(sql1, con);
            DataTable dt1 = new DataTable();
            con.Open();
            adpt1.Fill(dt1);
            con.Close();
            Nordeste.Text = dt1.Rows[0][0].ToString();

            // clientes na região SUL
            string sql2 = "select count(*) from tbclientes where regiao = 'SUL' ";
            SqlDataAdapter adpt2 = new SqlDataAdapter(sql2, con);
            DataTable dt2 = new DataTable();
            con.Open();
            adpt2.Fill(dt2);
            con.Close();
            Sul.Text = dt2.Rows[0][0].ToString();

            // clientes na região Sudeste
            string sql3 = "select count(*) from tbclientes where regiao = 'SUDESTE' ";
            SqlDataAdapter adpt3 = new SqlDataAdapter(sql3, con);
            DataTable dt3 = new DataTable();
            con.Open();
            adpt3.Fill(dt3);
            con.Close();
            Sudeste.Text = dt3.Rows[0][0].ToString();

            // clientes na região Centro-Oeste
            string sql4 = "select count(*) from tbclientes where regiao = 'CENTRO-OESTE' ";
            SqlDataAdapter adpt4 = new SqlDataAdapter(sql4, con);
            DataTable dt4 = new DataTable();
            con.Open();
            adpt4.Fill(dt4);
            con.Close();
            CentroOeste.Text = dt4.Rows[0][0].ToString();
        }
        protected void gvList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvList.PageIndex = e.NewPageIndex;
            AllData();  // Método para recarregar os dados no GridView
        }
        protected void Editar(object sender, EventArgs e)
        {
            using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
            {
                string id = gvList.DataKeys[row.RowIndex].Value.ToString();
                
                Response.Redirect("/dist/pages/Frm_AltClientes.aspx?id=" + id);
            }
        }
        protected void Mapa(object sender, EventArgs e)
        {
            using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
            {
                string id = gvList.DataKeys[row.RowIndex].Value.ToString();

                string sql = "select razcli,latitude,longitude,raio, endcli,baicli,cidcli,estcli from tbclientes where latitude <>'' and longitude <>'' and  id=" + id;

                SqlDataAdapter adpt = new SqlDataAdapter(sql, con);
                DataTable dt = new DataTable();
                con.Open();
                adpt.Fill(dt);
                con.Close();

                if (dt.Rows.Count > 0)
                {
                    string url = "MapaCliente.aspx?id=" + id;
                    string script = $"window.open('{url}', '_blank', 'width=800,height=600,scrollbars=yes,resizable=yes');";
                    ClientScript.RegisterStartupScript(this.GetType(), "openWindow", script, true);
                }
                else
                {
                    string linha1 = "Cliente não possui coordenadas cadastradas.";


                    // Concatenando as linhas com '\n' para criar a mensagem
                    string mensagem = $"{linha1}";

                    string mensagemCodificada = HttpUtility.JavaScriptStringEncode(mensagem);
                    // Gerando o script JavaScript para exibir o alerta
                    string script = $"alert('{mensagemCodificada}');";

                    // Registrando o script para execução no lado do cliente
                    ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", script, true);
                }

                
            }
        }

        //Método que faz a "exclusão" do dado deixando ele com o status de invisivel
        //protected void Excluir(object sender, EventArgs e)
        //{
        //    if (txtconformmessageValue.Value == "Yes")
        //    {
        //        using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
        //        {
        //            string id = gvList.DataKeys[row.RowIndex].Value.ToString();

        //            string sql = "update tbclientes set fl_exclusao='S' where id=@id";
        //            SqlCommand comando = new SqlCommand(sql, con);
        //            comando.Parameters.AddWithValue("@id", id);
        //            try
        //            {
        //                con.Open();
        //                comando.ExecuteNonQuery();
        //                con.Close();
        //                AllData();
        //                string retorno = "Registro excluído com sucesso!";
        //                System.Text.StringBuilder sb = new System.Text.StringBuilder();
        //                sb.Append("<script type = 'text/javascript'>");
        //                sb.Append("window.onload=function(){");
        //                sb.Append("alert('");
        //                sb.Append(retorno);
        //                sb.Append("')};");
        //                sb.Append("</script>");
        //                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());

        //            }
        //            catch (Exception ex)
        //            {
        //                var message = new JavaScriptSerializer().Serialize(ex.Message.ToString());
        //                string retorno = "Erro! Contate o administrador. Detalhes do erro: " + message;
        //                System.Text.StringBuilder sb = new System.Text.StringBuilder();
        //                sb.Append("<script type = 'text/javascript'>");
        //                sb.Append("window.onload=function(){");
        //                sb.Append("alert('");
        //                sb.Append(retorno);
        //                sb.Append("')};");
        //                sb.Append("</script>");
        //                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
        //                //Chama a página de consulta clientes
        //                Response.Redirect("ConsultaClientes.aspx");
        //            }

        //            finally
        //            {
        //                con.Close();
        //            }
        //        }
        //    }

        //    //Método que faz a "exclusão" do dado deixando ele com o status de invisivel


        //}
        private void AllData(string searchTerm = "")
        {
            var dataTable = DAL.codCliente.FetchDataTable2(searchTerm);
            if (dataTable.Rows.Count <= 0)
            {
                gvList.DataSource = null;
                gvList.DataBind();
                return;
            }

            gvList.DataSource = dataTable;
            gvList.DataBind();
        }

        protected void myInput_TextChanged(object sender, EventArgs e)
        {
            string searchTerm = myInput.Text.Trim();
            AllData(searchTerm);
        }
    }
    
}
 

        







