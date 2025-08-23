using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Net.Mail;
using System.Text;
using System.Drawing;
using System.Web.UI.HtmlControls;
using System.Globalization;
using System.Configuration;
namespace NewCapit.dist.pages
{
    public partial class Frm_DiarioDeBordo : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conexao"].ToString());
        string cod_ref_parada;
        string login, html;
        bool valido, valido2;
        protected void Page_Load(object sender, EventArgs e)
        {
            btnExcluiMotoristas.Visible = false;
            btnExcluiTodas.Visible = false;

        }
        protected void grdCusto_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {

                int index = Convert.ToInt32(e.CommandArgument.ToString());
                string id = grdCusto.DataKeys[Convert.ToInt32(e.CommandArgument)].Value.ToString();

                string sql = "delete tb_custo_motorista where cod_custo=" + id;
                SqlCommand cmd6 = new SqlCommand(sql, con);
                try
                {
                    con.Open();
                    cmd6.ExecuteNonQuery();
                    con.Close();
                }
                catch
                {
                    string message2 = "Erro ao Excluir Custo!";
                    System.Text.StringBuilder sb2 = new System.Text.StringBuilder();
                    sb2.Append("<script type = 'text/javascript'>");
                    sb2.Append("window.onload=function(){");
                    sb2.Append("alert('");
                    sb2.Append(message2);
                    sb2.Append("')};");
                    sb2.Append("</script>");
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb2.ToString());

                }
                finally
                {
                    con.Close();

                }



                string message = "Custo excluído com sucesso!";
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<script type = 'text/javascript'>");
                sb.Append("window.onload=function(){");
                sb.Append("alert('");
                sb.Append(message);
                sb.Append("')};");
                sb.Append("</script>");
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
                //Limpar2();
                //CarregaCusto();

            }

        }
        protected void grdMotoristas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {

                int index = Convert.ToInt32(e.CommandArgument.ToString());
                string id = grdMotoristas.DataKeys[Convert.ToInt32(e.CommandArgument)].Value.ToString();

                string sql = "select * from tb_parada where cod_parada=" + id;
                SqlDataAdapter adtp = new SqlDataAdapter(sql, con);
                DataTable dt = new DataTable();
                con.Open();
                adtp.Fill(dt);
                con.Close();

                if (dt.Rows.Count > 0)
                {
                    string sql1 = "insert tb_exclusao_macro (dt_macro, hr_macro, cod_transmissao, cod_veiculo, cod_ref_parada, ds_macro, tipo_macro, cod_cracha, dt_exclusao, nm_usuario) ";
                    sql1 += "values (@dt_macro, @hr_macro,@cod_transmissao, @cod_veiculo, @cod_ref_parada, @ds_macro, @tipo_macro, @cod_cracha, @dt_exclusao, @nm_usuario)";

                    SqlCommand cmd5 = new SqlCommand(sql1, con);
                    cmd5.Parameters.AddWithValue("@dt_macro", DateTime.Parse(dt.Rows[0][3].ToString()).ToString("yyyy-MM-dd"));
                    cmd5.Parameters.AddWithValue("@hr_macro", DateTime.Parse(dt.Rows[0][4].ToString()).ToString("HH:mm:ss.000"));
                    cmd5.Parameters.AddWithValue("@cod_transmissao", dt.Rows[0][1].ToString());
                    cmd5.Parameters.AddWithValue("@cod_veiculo", dt.Rows[0][2].ToString());
                    cmd5.Parameters.AddWithValue("@cod_ref_parada", dt.Rows[0][12].ToString());
                    cmd5.Parameters.AddWithValue("@ds_macro", dt.Rows[0][6].ToString());
                    cmd5.Parameters.AddWithValue("@tipo_macro", dt.Rows[0][11].ToString());
                    cmd5.Parameters.AddWithValue("@cod_cracha", dt.Rows[0][10].ToString());
                    cmd5.Parameters.AddWithValue("@dt_exclusao", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.000"));
                    cmd5.Parameters.AddWithValue("@nm_usuario", Page.Session["usuario"].ToString());

                    try
                    {
                        con.Open();
                        cmd5.ExecuteNonQuery();
                        con.Close();

                        string sql3 = "update tb_parada set fl_deletado='S' where cod_parada=" + id;
                        SqlCommand cmd6 = new SqlCommand(sql3, con);
                        try
                        {
                            con.Open();
                            cmd6.ExecuteNonQuery();
                            con.Close();
                        }
                        catch
                        {
                            string message2 = "Erro ao Excluir Macro!";
                            System.Text.StringBuilder sb2 = new System.Text.StringBuilder();
                            sb2.Append("<script type = 'text/javascript'>");
                            sb2.Append("window.onload=function(){");
                            sb2.Append("alert('");
                            sb2.Append(message2);
                            sb2.Append("')};");
                            sb2.Append("</script>");
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb2.ToString());

                        }
                        finally
                        {
                            con.Close();

                        }



                        string message = "Macro excluída com sucesso!";
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append("<script type = 'text/javascript'>");
                        sb.Append("window.onload=function(){");
                        sb.Append("alert('");
                        sb.Append(message);
                        sb.Append("')};");
                        sb.Append("</script>");
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
                        //Limpar();
                        //CarregaGrid2();
                        //CarregaTodas();

                    }
                    catch (Exception ex)
                    {
                        var message = new JavaScriptSerializer().Serialize(ex.Message.ToString());
                        string retorno = "Erro! Contate o administrador. Detalhes do erro: " + message;
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append("<script type = 'text/javascript'>");
                        sb.Append("window.onload=function(){");
                        sb.Append("alert('");
                        sb.Append(retorno);
                        sb.Append("')};");
                        sb.Append("</script>");
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());

                    }
                    finally
                    {
                        con.Close();
                    }
                }

            }
        }
        protected void grdTodas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {

                int index = Convert.ToInt32(e.CommandArgument.ToString());
                string id = grdTodas.DataKeys[Convert.ToInt32(e.CommandArgument)].Value.ToString();

                string sql = "select * from tb_parada where cod_parada=" + id;
                SqlDataAdapter adtp = new SqlDataAdapter(sql, con);
                DataTable dt = new DataTable();
                con.Open();
                adtp.Fill(dt);
                con.Close();

                if (dt.Rows.Count > 0)
                {
                    string sql1 = "insert tb_exclusao_macro (dt_macro, hr_macro, cod_transmissao, cod_veiculo, cod_ref_parada, ds_macro, tipo_macro, cod_cracha, dt_exclusao, nm_usuario) ";
                    sql1 += "values (@dt_macro, @hr_macro,@cod_transmissao, @cod_veiculo, @cod_ref_parada, @ds_macro, @tipo_macro, @cod_cracha, @dt_exclusao, @nm_usuario)";

                    SqlCommand cmd5 = new SqlCommand(sql1, con);
                    cmd5.Parameters.AddWithValue("@dt_macro", DateTime.Parse(dt.Rows[0][3].ToString()).ToString("yyyy-MM-dd"));
                    cmd5.Parameters.AddWithValue("@hr_macro", DateTime.Parse(dt.Rows[0][4].ToString()).ToString("HH:mm:ss.000"));
                    cmd5.Parameters.AddWithValue("@cod_transmissao", dt.Rows[0][1].ToString());
                    cmd5.Parameters.AddWithValue("@cod_veiculo", dt.Rows[0][2].ToString());
                    cmd5.Parameters.AddWithValue("@cod_ref_parada", dt.Rows[0][12].ToString());
                    cmd5.Parameters.AddWithValue("@ds_macro", dt.Rows[0][6].ToString());
                    cmd5.Parameters.AddWithValue("@tipo_macro", dt.Rows[0][11].ToString());
                    cmd5.Parameters.AddWithValue("@cod_cracha", dt.Rows[0][10].ToString());
                    cmd5.Parameters.AddWithValue("@dt_exclusao", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.000"));
                    cmd5.Parameters.AddWithValue("@nm_usuario", Page.Session["usuario"].ToString());

                    try
                    {
                        con.Open();
                        cmd5.ExecuteNonQuery();
                        con.Close();

                        string sql3 = "update tb_parada set fl_deletado='S' where cod_parada=" + id;
                        SqlCommand cmd6 = new SqlCommand(sql3, con);
                        try
                        {
                            con.Open();
                            cmd6.ExecuteNonQuery();
                            con.Close();
                        }
                        catch
                        {
                            string message2 = "Erro ao Excluir Macro!";
                            System.Text.StringBuilder sb2 = new System.Text.StringBuilder();
                            sb2.Append("<script type = 'text/javascript'>");
                            sb2.Append("window.onload=function(){");
                            sb2.Append("alert('");
                            sb2.Append(message2);
                            sb2.Append("')};");
                            sb2.Append("</script>");
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb2.ToString());

                        }
                        finally
                        {
                            con.Close();

                        }



                        string message = "Macro excluída com sucesso!";
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append("<script type = 'text/javascript'>");
                        sb.Append("window.onload=function(){");
                        sb.Append("alert('");
                        sb.Append(message);
                        sb.Append("')};");
                        sb.Append("</script>");
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
                        //Limpar();
                        //CarregaGrid2();
                        //CarregaTodas();

                    }
                    catch (Exception ex)
                    {
                        var message = new JavaScriptSerializer().Serialize(ex.Message.ToString());
                        string retorno = "Erro! Contate o administrador. Detalhes do erro: " + message;
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append("<script type = 'text/javascript'>");
                        sb.Append("window.onload=function(){");
                        sb.Append("alert('");
                        sb.Append(retorno);
                        sb.Append("')};");
                        sb.Append("</script>");
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());

                    }
                    finally
                    {
                        con.Close();
                    }
                }

            }
        }
        protected void btnExcluiTodas_Click(object sender, EventArgs e)
        {
            if (txtconformmessageValue6.Value == "Yes")
            {
                foreach (GridViewRow row in grdTodas.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        CheckBox chkRow = (row.Cells[0].FindControl("chkT") as CheckBox);
                        if (chkRow.Checked)
                        {
                            string id = grdTodas.DataKeys[Convert.ToInt32(row.RowIndex)].Value.ToString();
                            string sql = "select * from tb_parada where cod_parada=" + id;
                            SqlDataAdapter adtp = new SqlDataAdapter(sql, con);
                            DataTable dt = new DataTable();
                            con.Open();
                            adtp.Fill(dt);
                            con.Close();

                            if (dt.Rows.Count > 0)
                            {
                                string sql1 = "insert tb_exclusao_macro (dt_macro, hr_macro, cod_transmissao, cod_veiculo, cod_ref_parada, ds_macro, tipo_macro, cod_cracha, dt_exclusao, nm_usuario) ";
                                sql1 += "values (@dt_macro, @hr_macro,@cod_transmissao, @cod_veiculo, @cod_ref_parada, @ds_macro, @tipo_macro, @cod_cracha, @dt_exclusao, @nm_usuario)";

                                SqlCommand cmd5 = new SqlCommand(sql1, con);
                                cmd5.Parameters.AddWithValue("@dt_macro", DateTime.Parse(dt.Rows[0][3].ToString()).ToString("yyyy-MM-dd"));
                                cmd5.Parameters.AddWithValue("@hr_macro", DateTime.Parse(dt.Rows[0][4].ToString()).ToString("HH:mm:ss.000"));
                                cmd5.Parameters.AddWithValue("@cod_transmissao", dt.Rows[0][1].ToString());
                                cmd5.Parameters.AddWithValue("@cod_veiculo", dt.Rows[0][2].ToString());
                                cmd5.Parameters.AddWithValue("@cod_ref_parada", dt.Rows[0][12].ToString());
                                cmd5.Parameters.AddWithValue("@ds_macro", dt.Rows[0][6].ToString());
                                cmd5.Parameters.AddWithValue("@tipo_macro", dt.Rows[0][11].ToString());
                                cmd5.Parameters.AddWithValue("@cod_cracha", dt.Rows[0][10].ToString());
                                cmd5.Parameters.AddWithValue("@dt_exclusao", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.000"));
                                cmd5.Parameters.AddWithValue("@nm_usuario", Page.Session["usuario"].ToString());

                                try
                                {
                                    con.Open();
                                    cmd5.ExecuteNonQuery();
                                    con.Close();

                                    string sql3 = "update tb_parada set fl_deletado='S' where cod_parada=" + id;
                                    SqlCommand cmd6 = new SqlCommand(sql3, con);
                                    try
                                    {
                                        con.Open();
                                        cmd6.ExecuteNonQuery();
                                        con.Close();
                                    }
                                    catch
                                    {
                                        string message2 = "Erro ao Excluir Macro!";
                                        System.Text.StringBuilder sb2 = new System.Text.StringBuilder();
                                        sb2.Append("<script type = 'text/javascript'>");
                                        sb2.Append("window.onload=function(){");
                                        sb2.Append("alert('");
                                        sb2.Append(message2);
                                        sb2.Append("')};");
                                        sb2.Append("</script>");
                                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb2.ToString());

                                    }
                                    finally
                                    {
                                        con.Close();

                                    }




                                }
                                catch (Exception ex)
                                {


                                }
                                finally
                                {
                                    con.Close();
                                }
                            }
                        }
                    }
                }

                string message = "Macro excluída com sucesso!";
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<script type = 'text/javascript'>");
                sb.Append("window.onload=function(){");
                sb.Append("alert('");
                sb.Append(message);
                sb.Append("')};");
                sb.Append("</script>");
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
                //Limpar();
                //CarregaGrid2();
                //CarregaTodas();
            }
        }
        public void CarregaTodas()
        {
            string data = string.Empty;
            string cod = string.Empty;
            string login = string.Empty;
            grdTodas.DataSource = null;
            grdTodas.DataBind();
           
            data = txtData.Text;



            login = txtMotorista.Text.Trim();

            if (!login.All(char.IsDigit)) // verifica se tem apenas números
            {
                string message = "Motorista não possui marcações!";
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<script type = 'text/javascript'>");
                sb.Append("window.onload=function(){");
                sb.Append("alert('");
                sb.Append(message);
                sb.Append("')};");
                sb.Append("</script>");
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
                return;
            }


            string sql1 = "exec sp_tempo_total '" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "','" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "','" + login + "'";
            SqlDataAdapter adtp1 = new SqlDataAdapter(sql1, con);
            DataTable dt = new DataTable();
            con.Open();
            adtp1.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                grdTodas.DataSource = dt;
                grdTodas.DataBind();
                lblTodas.Text = "Todas as Marcações do Motorista";
                btnExcluiTodas.Visible = true;
            }
            else
            {
                lblTodas.Text = "Não existem marcações para o motorista solicitado!";
            }

            //string message = DateTime.Parse(data).ToString("yyyy-MM-dd");
            //System.Text.StringBuilder sb = new System.Text.StringBuilder();
            //sb.Append("<script type = 'text/javascript'>");
            //sb.Append("window.onload=function(){");
            //sb.Append("alert('");
            //sb.Append(message);
            //sb.Append("')};");
            //sb.Append("</script>");
            //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());

            btnExcluiMotoristas.Visible = false;
            btnExcluiTodas.Visible = true;

        }
        public void Limpar()
        {
            ddlMacros.ClearSelection();
            txtHora.Text = "";
            ddlMacros.ClearSelection();
            txtTipoMarcacao.Text = "";

        }
        public void Limpar2()
        {
            txtAlmoco.Text = "0,00";
            txtCafe.Text = "0,00";
            txtJantar.Text = "0,00";
            txtPernoite.Text = "0,00";
            //txtPremio.Text = "0,00";
            txtEngDesen.Text = "0,00";
            txtRel1.Text = string.Empty;
            txtRel2.Text = string.Empty;
            txtRel3.Text = string.Empty;
            txtRel4.Text = string.Empty;
        }
        protected void btnMacromanual_Click(object sender, EventArgs e)
        {
            string cod_usuario = Page.Session["cod_usuario"].ToString();

            if (txtconformmessageValue1.Value == "Yes")
            {

                if (txtMotorista.Text != string.Empty)
                {
                    if (ddlMacros.SelectedValue != "0")
                    {

                        if (ddlMacros.SelectedValue == "PARADA" || ddlMacros.SelectedValue == "PARADA INTERNA" || ddlMacros.SelectedValue == "PARADA CLIENTE/FORNECEDOR" || ddlMacros.SelectedValue == "PARADA OFICINA")
                        {

                            if (ddlNumero.SelectedValue != "0")
                            {
                                string sql_ini5 = "insert tb_parada(cod_idveiculo, cod_transmissao, dt_posicao_parada, hr_posicao, ds_macro, cod_ref_parada,ds_tipo,ds_local_parada,cod_cracha, cod_usuario_alt) ";
                                sql_ini5 += " values (@cod_veiculo,@cod_transmissao, @dt_posicao_parada, @hr_posicao_parada,@ds_macro, @cod_ref_parada,@ds_tipo,@ds_local_parada,@cod_cracha,@cod_usuario_alt)";

                                SqlCommand cmd5 = new SqlCommand(sql_ini5, con);
                                cmd5.Parameters.AddWithValue("@cod_veiculo", "123456");
                                cmd5.Parameters.AddWithValue("@cod_transmissao", "000" + DateTime.Now.ToString("mmss"));
                                cmd5.Parameters.AddWithValue("@dt_posicao_parada", DateTime.Parse(txtData.Text).ToString("yyyy-MM-dd"));
                                cmd5.Parameters.AddWithValue("@hr_posicao_parada", DateTime.Parse(txtHora.Text).ToString("HH:mm:ss.000"));
                                cmd5.Parameters.AddWithValue("@cod_ref_parada", ddlNumero.SelectedValue);
                                cmd5.Parameters.AddWithValue("@ds_tipo", txtTipoMarcacao.Text);
                                cmd5.Parameters.AddWithValue("@ds_macro", ddlMacros.SelectedValue);
                                cmd5.Parameters.AddWithValue("@ds_local_parada", "MARCAÇÃO MANUAL");
                                cmd5.Parameters.AddWithValue("@cod_cracha", txtMotorista.Text);
                                cmd5.Parameters.AddWithValue("@cod_usuario_alt", cod_usuario);

                                try
                                {
                                    con.Open();
                                    cmd5.ExecuteNonQuery();
                                    con.Close();
                                    string message = "Informações cadastradas com sucesso!";
                                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                                    sb.Append("<script type = 'text/javascript'>");
                                    sb.Append("window.onload=function(){");
                                    sb.Append("alert('");
                                    sb.Append(message);
                                    sb.Append("')};");
                                    sb.Append("</script>");
                                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
                                    AtualizaJornada();
                                    Limpar();
                                    CarregaGrid2();
                                    CarregaTodas();

                                }
                                catch (Exception ex)
                                {
                                    var message = new JavaScriptSerializer().Serialize(ex.Message.ToString());
                                    string retorno = "Erro! Contate o administrador. Detalhes do erro: " + message;
                                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                                    sb.Append("<script type = 'text/javascript'>");
                                    sb.Append("window.onload=function(){");
                                    sb.Append("alert('");
                                    sb.Append(retorno);
                                    sb.Append("')};");
                                    sb.Append("</script>");
                                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());

                                }
                            }
                            else
                            {
                                string message = "Necessário informar numero referente a parada!";
                                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                                sb.Append("<script type = 'text/javascript'>");
                                sb.Append("window.onload=function(){");
                                sb.Append("alert('");
                                sb.Append(message);
                                sb.Append("')};");
                                sb.Append("</script>");
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());

                            }
                        }
                        else
                        {
                            string sql_ini5 = "insert into tb_parada(cod_idveiculo, cod_transmissao, dt_posicao_parada, hr_posicao, ds_macro, cod_ref_parada,ds_tipo,ds_local_parada,cod_cracha,cod_usuario_alt) ";
                            sql_ini5 += " values (@cod_veiculo,@cod_transmissao, @dt_posicao_parada, @hr_posicao_parada,@ds_macro, @cod_ref_parada,@ds_tipo,@ds_local_parada,@cod_cracha,@cod_usuario_alt)";

                            SqlCommand cmd5 = new SqlCommand(sql_ini5, con);
                            cmd5.Parameters.AddWithValue("@cod_veiculo", "000000");
                            cmd5.Parameters.AddWithValue("@cod_transmissao", "000" + DateTime.Now.ToString("mmss"));
                            cmd5.Parameters.AddWithValue("@dt_posicao_parada", DateTime.Parse(txtData.Text).ToString("yyyy-MM-dd"));
                            cmd5.Parameters.AddWithValue("@hr_posicao_parada", DateTime.Parse(txtHora.Text).ToString("HH:mm:ss.000"));
                            cmd5.Parameters.AddWithValue("@cod_ref_parada", ddlNumero.SelectedValue);
                            cmd5.Parameters.AddWithValue("@ds_tipo", txtTipoMarcacao.Text);
                            cmd5.Parameters.AddWithValue("@ds_macro", ddlMacros.SelectedValue);
                            cmd5.Parameters.AddWithValue("@ds_local_parada", "MARCAÇÃO MANUAL");
                            cmd5.Parameters.AddWithValue("@cod_cracha", txtMotorista.Text);
                            cmd5.Parameters.AddWithValue("@cod_usuario_alt", cod_usuario);
                            try
                            {
                                con.Open();
                                cmd5.ExecuteNonQuery();
                                con.Close();
                                string message = "Informações cadastradas com sucesso!";
                                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                                sb.Append("<script type = 'text/javascript'>");
                                sb.Append("window.onload=function(){");
                                sb.Append("alert('");
                                sb.Append(message);
                                sb.Append("')};");
                                sb.Append("</script>");
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
                                AtualizaJornada();
                                Limpar();
                                CarregaGrid2();
                                CarregaTodas();

                            }
                            catch (Exception ex)
                            {
                                var message = new JavaScriptSerializer().Serialize(ex.Message.ToString());
                                string retorno = "Erro! Contate o administrador. Detalhes do erro: " + message;
                                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                                sb.Append("<script type = 'text/javascript'>");
                                sb.Append("window.onload=function(){");
                                sb.Append("alert('");
                                sb.Append(retorno);
                                sb.Append("')};");
                                sb.Append("</script>");
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());

                            }
                        }
                    }
                    else
                    {
                        string message = "Obrigatório informar macro!";
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append("<script type = 'text/javascript'>");
                        sb.Append("window.onload=function(){");
                        sb.Append("alert('");
                        sb.Append(message);
                        sb.Append("')};");
                        sb.Append("</script>");
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());

                    }
                }
                else
                {
                    string message = "Necessário informar crachá do motorista";
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append("<script type = 'text/javascript'>");
                    sb.Append("window.onload=function(){");
                    sb.Append("alert('");
                    sb.Append(message);
                    sb.Append("')};");
                    sb.Append("</script>");
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
                }

            }




        }
        public void CarregaGrid2()
        {
            try
            {
                string data = string.Empty;
                string cod = string.Empty;
                string login = string.Empty;
                html = string.Empty;
                grdMotoristas.DataSource = null;
                grdMotoristas.DataBind();
                lblBloco.Text = "";

                
                



                data = DateTime.Parse(txtData.Text).ToString("yyyy-MM-dd");



                login = txtMotorista.Text;


                string sql1 = "exec sp_tempo_total '" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "','" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "'," + login + "";
                SqlDataAdapter adtp1 = new SqlDataAdapter(sql1, con);
                DataTable dt = new DataTable();
                con.Open();
                adtp1.Fill(dt);
                con.Close();



                //string sql2 = "select nm_motorista, cod_login from tb_motorista where cod_login='" + login + "'";
                string sql2 = "select  nommot, codmot, funcao,nucleo from tbmotoristas where codmot='" + login + "'";
                SqlDataAdapter adtp2 = new SqlDataAdapter(sql2, con);
                DataTable dt2 = new DataTable();
                con.Open();
                adtp2.Fill(dt2);
                con.Close();

                string sql3 = "select  * from tb_parada where cod_cracha='" + login + "' and dt_posicao_parada='" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "' and fl_deletado is null  order by hr_posicao";
                SqlDataAdapter adtp3 = new SqlDataAdapter(sql3, con);
                DataTable dt3 = new DataTable();
                con.Open();
                adtp3.Fill(dt3);
                con.Close();

                if (dt.Rows.Count > 0)
                {
                    if (dt2.Rows.Count > 0)
                    {
                        txtNome.Text = dt2.Rows[0][0].ToString();
                        txtFuncao.Text = dt2.Rows[0][2].ToString();
                        txtNucleo.Text = dt2.Rows[0][3].ToString();

                    }
                    else
                    {
                        txtNome.Text = "NÃO LOCALIZADO";


                    }

                    if (dt3.Rows.Count > 0)
                    {

                        if (dt3.Rows[0][6].ToString() == "INICIO DE JORNADA")
                        {

                            //VERIFICA SE A ULTIMA MARCAÇÃO DO DIA É PARADA PERNOITE OU FIM DE JORNADA
                            string sqlv = "select TOP 1 ds_macro  from tb_parada where cod_cracha=" + login + " and dt_posicao_parada='" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "' and fl_deletado is null order by hr_posicao  DESC ";
                            SqlDataAdapter adtpv = new SqlDataAdapter(sqlv, con);
                            DataTable dtv = new DataTable();
                            con.Open();
                            adtpv.Fill(dtv);
                            con.Close();


                            if (dtv.Rows[0][0].ToString() == "FIM DE JORNADA" || dtv.Rows[0][0].ToString() == "PARADA PERNOITE")
                            {

                                //JORNADA NORMAL INICIO DE JORNADA
                                grdMotoristas.DataSource = dt;
                                grdMotoristas.DataBind();
                                html = "<br/>";
                                html += "<a class='btn btn-outline-info' href=JavaScript:ver_demo('" + login + "','" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "','" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "');><i class='icon_profile' title='Ver Demonstrativo'></i></a>";
                                HtmlGenericControl table = this.button;
                                table.InnerHtml = html;
                                lblBloco.Text = "BLOCO1A";
                                btnExcluiMotoristas.Visible = true;
                            }
                            else
                            {
                                //JORNADA PERNOITE INICIO DE JORNADA
                                string sqli = "select top 1  hr_posicao from tb_parada where cod_cracha=" + login + " and ds_macro = 'INICIO DE JORNADA' and dt_posicao_parada='" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "' and fl_deletado is null order by hr_posicao";
                                SqlDataAdapter adtpi = new SqlDataAdapter(sqli, con);
                                DataTable dti = new DataTable();
                                con.Open();
                                adtpi.Fill(dti);
                                con.Close();

                                string sqlf = "select top 1 hr_posicao from tb_parada where cod_cracha=" + login + " and ds_macro = 'FIM DE JORNADA' and dt_posicao_parada='" + DateTime.Parse(data).AddDays(1).ToString("yyyy-MM-dd") + "' and fl_deletado is null OR cod_cracha=" + login + " and ds_macro='PARADA PERNOITE' and dt_posicao_parada='" + DateTime.Parse(data).AddDays(1).ToString("yyyy-MM-dd") + "' and fl_deletado is null order by hr_posicao";
                                SqlDataAdapter adtpf = new SqlDataAdapter(sqlf, con);
                                DataTable dtf = new DataTable();
                                con.Open();
                                adtpf.Fill(dtf);
                                con.Close();



                                if (dti.Rows.Count > 0 && dtf.Rows.Count > 0)
                                {
                                    string sql4 = "exec sp_tempo_total_noturno '" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "','" + DateTime.Parse(data).AddDays(1).ToString("yyyy-MM-dd") + "','" + dti.Rows[0][0].ToString() + "','" + dtf.Rows[0][0].ToString() + "', " + login + "";
                                    SqlDataAdapter adtp4 = new SqlDataAdapter(sql4, con);
                                    DataTable dt4 = new DataTable();
                                    con.Open();
                                    adtp4.Fill(dt4);
                                    con.Close();

                                    grdMotoristas.DataSource = dt4;
                                    grdMotoristas.DataBind();
                                    html = "<br/>";
                                    html += "<a class='btn btn-outline-info' href=JavaScript:ver_demo2('" + dt2.Rows[0][1].ToString() + "','" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "','" + DateTime.Parse(data).AddDays(1).ToString("yyyy-MM-dd") + "','" + dti.Rows[0][0].ToString() + "','" + dtf.Rows[0][0].ToString() + "');>Imprimir Diário</a>";
                                    HtmlGenericControl table = this.button;
                                    table.InnerHtml = html;
                                    lblBloco.Text = "BLOCO1B";
                                    btnExcluiMotoristas.Visible = true;
                                }
                            }

                        }
                        else if (dt3.Rows[0][6].ToString() == "REINICIO DE VIAGEM")
                        {
                            //VERIFICA SE A ULTIMA MARCAÇÃO DO DIA ANTERIOR É PARADA PERNOITE OU FIM DE VIAGEM
                            string sqlv = "select TOP 1 ds_macro from tb_parada where cod_cracha=" + login + " and dt_posicao_parada='" + DateTime.Parse(data).AddDays(-1).ToString("yyyy-MM-dd") + "' and fl_deletado is null order by hr_posicao  DESC ";
                            SqlDataAdapter adtpv = new SqlDataAdapter(sqlv, con);
                            DataTable dtv = new DataTable();
                            con.Open();
                            adtpv.Fill(dtv);
                            con.Close();

                            if (dtv.Rows.Count > 0)
                            {
                                if (dtv.Rows[0][0].ToString() == "PARADA PERNOITE" || dtv.Rows[0][0].ToString() == "FIM DE JORNADA")
                                {
                                    //JORNADA NORMAL REINICIO DE VIAGEM

                                    //VERIFICA SE A ULTIMA MARCAÇÃO DO DIA É PARADA PERNOITE OU FIM DE VIAGEM
                                    string sqlu = "select ds_macro from tb_parada where cod_cracha=" + login + " and dt_posicao_parada='" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "' and fl_deletado is null order by hr_posicao  DESC";
                                    SqlDataAdapter adtpu = new SqlDataAdapter(sqlu, con);
                                    DataTable dtu = new DataTable();
                                    con.Open();
                                    adtpu.Fill(dtu);
                                    con.Close();


                                    if (dtu.Rows[0][0].ToString() == "FIM DE JORNADA" || dtu.Rows[0][0].ToString() == "PARADA PERNOITE")
                                    {
                                        //JORNADA NORMAL REINICIO DE VIAGEM
                                        grdMotoristas.DataSource = dt;
                                        grdMotoristas.DataBind();
                                        html = "<br/>";
                                        html += "<a class='btn btn-outline-info' href=JavaScript:ver_demo('" + login + "','" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "','" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "');><i class='icon_profile' title='Ver Demonstrativo'></i></a>";
                                        HtmlGenericControl table = this.button;
                                        table.InnerHtml = html;
                                        lblBloco.Text = "BLOCO2A";
                                        btnExcluiMotoristas.Visible = true;

                                    }
                                    else
                                    {
                                        //JORNADA PERNOITE REINICIO DE VIAGEM
                                        string sqli = "select top 1  hr_posicao from tb_parada where cod_cracha=" + login + " and ds_macro = 'REINICIO DE VIAGEM' and dt_posicao_parada='" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "' and fl_deletado is null order by hr_posicao";
                                        SqlDataAdapter adtpi = new SqlDataAdapter(sqli, con);
                                        DataTable dti = new DataTable();
                                        con.Open();
                                        adtpi.Fill(dti);
                                        con.Close();

                                        string sqlf = "select top 1 hr_posicao from tb_parada where cod_cracha=" + login + " and ds_macro = 'FIM DE JORNADA' and dt_posicao_parada='" + DateTime.Parse(data).AddDays(1).ToString("yyyy-MM-dd") + "' and fl_deletado is null OR cod_cracha=" + login + " and ds_macro='PARADA PERNOITE' and dt_posicao_parada='" + DateTime.Parse(data).AddDays(1).ToString("yyyy-MM-dd") + "' and fl_deletado is null order by hr_posicao";
                                        SqlDataAdapter adtpf = new SqlDataAdapter(sqlf, con);
                                        DataTable dtf = new DataTable();
                                        con.Open();
                                        adtpf.Fill(dtf);
                                        con.Close();



                                        if (dti.Rows.Count > 0 && dtf.Rows.Count > 0)
                                        {
                                            string sql4 = "exec sp_tempo_total_noturno '" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "','" + DateTime.Parse(data).AddDays(1).ToString("yyyy-MM-dd") + "','" + dti.Rows[0][0].ToString() + "','" + dtf.Rows[0][0].ToString() + "', " + login + "";
                                            SqlDataAdapter adtp4 = new SqlDataAdapter(sql4, con);
                                            DataTable dt4 = new DataTable();
                                            con.Open();
                                            adtp4.Fill(dt4);
                                            con.Close();

                                            grdMotoristas.DataSource = dt4;
                                            grdMotoristas.DataBind();
                                            html = "<br/>";
                                            html += "<a class='btn btn-outline-info' href=JavaScript:ver_demo2('" + dt2.Rows[0][1].ToString() + "','" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "','" + DateTime.Parse(data).AddDays(1).ToString("yyyy-MM-dd") + "','" + dti.Rows[0][0].ToString() + "','" + dtf.Rows[0][0].ToString() + "');>Imprimir Diário</a>";
                                            HtmlGenericControl table = this.button;
                                            table.InnerHtml = html;
                                            lblBloco.Text = "BLOCO2B";
                                            btnExcluiMotoristas.Visible = true;
                                        }



                                    }

                                }
                                else
                                {
                                    //JORNADA DIFERENCIA DE COMEÇO

                                    //VERIFICA SE A ULTIMA MARCAÇÃO DO DIA É PARADA PERNOITE OU FIM DE VIAGEM
                                    string sqlz = "select TOP 1 ds_macro from tb_parada where cod_cracha=" + login + " and dt_posicao_parada='" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "' and fl_deletado is null order by hr_posicao  DESC ";
                                    SqlDataAdapter adtpz = new SqlDataAdapter(sqlz, con);
                                    DataTable dtz = new DataTable();
                                    con.Open();
                                    adtpz.Fill(dtz);
                                    con.Close();




                                    if (dtz.Rows[0][0].ToString() == "FIM DE JORNADA" || dtz.Rows[0][0].ToString() == "PARADA PERNOITE")
                                    {
                                        //JORNADA NORMAL COM DIFERENÇA NO COMEÇO
                                        string sqld = "select TOP 1  ds_macro, hr_posicao from tb_parada  where  cod_cracha=" + login + " and fl_deletado is null and dt_posicao_parada='" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "'  and hr_posicao > (select top 1 hr_posicao from tb_parada where cod_cracha=" + login + " and fl_deletado is null and dt_posicao_parada='" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "'  and ds_macro='FIM DE JORNADA'  OR cod_cracha=" + login + " and fl_deletado is null and dt_posicao_parada='" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "'  and ds_macro='PARADA PERNOITE' order by hr_posicao) order by hr_posicao ";
                                        SqlDataAdapter adtpd = new SqlDataAdapter(sqld, con);
                                        DataTable dtd = new DataTable();
                                        con.Open();
                                        adtpd.Fill(dtd);
                                        con.Close();

                                        if (dtd.Rows.Count > 0)
                                        {
                                            string sqlx = "exec sp_tempo_total_dif '" + DateTime.Parse(txtData.Text).ToString("yyyy-MM-dd") + "','" + DateTime.Parse(txtData.Text).ToString("yyyy-MM-dd") + "','" + dtd.Rows[0][1].ToString() + "'," + login + "";
                                            SqlDataAdapter adtpx = new SqlDataAdapter(sqlx, con);
                                            DataTable dtx = new DataTable();
                                            con.Open();
                                            adtpx.Fill(dtx);
                                            con.Close();
                                            grdMotoristas.DataSource = dtx;
                                            grdMotoristas.DataBind();
                                            html = "<br/>";
                                            html += "<a class='btn btn-outline-info' href=JavaScript:ver_demo('" + login + "','" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "','" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "','" + dtd.Rows[0][1].ToString() + "');>Imprimir Diário</a>";
                                            HtmlGenericControl table = this.button;
                                            table.InnerHtml = html;
                                            lblBloco.Text = "BLOCO3a";
                                            btnExcluiMotoristas.Visible = true;
                                        }
                                        else
                                        {
                                            html = "";
                                             HtmlGenericControl table = this.button;
                                            table.InnerHtml = html;
                                        }

                                    }
                                    else
                                    {
                                        //JORNADA PERNOITE COM DIFERENÇA NO COMEÇO
                                        string sqli = "select TOP 1  hr_posicao from tb_parada  where  cod_cracha=" + login + " and fl_deletado is null and dt_posicao_parada='" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "'  and hr_posicao > (select top 1 hr_posicao from tb_parada where cod_cracha=" + login + " and fl_deletado is null and dt_posicao_parada='" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "'  and ds_macro='FIM DE JORNADA'  OR cod_cracha=" + login + " and fl_deletado is null and dt_posicao_parada='" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "'  and ds_macro='PARADA PERNOITE' order by hr_posicao) order by hr_posicao ";
                                        SqlDataAdapter adtpi = new SqlDataAdapter(sqli, con);
                                        DataTable dti = new DataTable();
                                        con.Open();
                                        adtpi.Fill(dti);
                                        con.Close();

                                        string sqlf = "select top 1 hr_posicao from tb_parada where cod_cracha=" + login + " and ds_macro = 'FIM DE JORNADA' and dt_posicao_parada='" + DateTime.Parse(data).AddDays(1).ToString("yyyy-MM-dd") + "' and fl_deletado is null OR cod_cracha=" + login + " and ds_macro='PARADA PERNOITE' and dt_posicao_parada='" + DateTime.Parse(data).AddDays(1).ToString("yyyy-MM-dd") + "' and fl_deletado is null order by hr_posicao";
                                        SqlDataAdapter adtpf = new SqlDataAdapter(sqlf, con);
                                        DataTable dtf = new DataTable();
                                        con.Open();
                                        adtpf.Fill(dtf);
                                        con.Close();



                                        if (dti.Rows.Count > 0 && dtf.Rows.Count > 0)
                                        {
                                            string sql4 = "exec sp_tempo_total_noturno '" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "','" + DateTime.Parse(data).AddDays(1).ToString("yyyy-MM-dd") + "','" + dti.Rows[0][0].ToString() + "','" + dtf.Rows[0][0].ToString() + "', " + login + "";
                                            SqlDataAdapter adtp4 = new SqlDataAdapter(sql4, con);
                                            DataTable dt4 = new DataTable();
                                            con.Open();
                                            adtp4.Fill(dt4);
                                            con.Close();

                                            grdMotoristas.DataSource = dt4;
                                            grdMotoristas.DataBind();
                                            html = "<br/>";
                                            html += "<a class='btn btn-outline-info' href=JavaScript:ver_demo2('" + dt2.Rows[0][1].ToString() + "','" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "','" + DateTime.Parse(data).AddDays(1).ToString("yyyy-MM-dd") + "','" + dti.Rows[0][0].ToString() + "','" + dtf.Rows[0][0].ToString() + "');>Imprimir Diário</a>";
                                            HtmlGenericControl table = this.button;
                                            table.InnerHtml = html;
                                            lblBloco.Text = "BLOCO3c";
                                            btnExcluiMotoristas.Visible = true;
                                        }
                                    }
                                }

                            }

                        }

                        else
                        {
                            //JORNADA DIFERENCIA DE COMEÇO

                            //VERIFICA SE A ULTIMA MARCAÇÃO DO DIA É PARADA PERNOITE OU FIM DE VIAGEM
                            string sqlz = "select TOP 1 ds_macro from tb_parada where cod_cracha=" + login + " and dt_posicao_parada='" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "' and fl_deletado is null order by hr_posicao  DESC ";
                            SqlDataAdapter adtpz = new SqlDataAdapter(sqlz, con);
                            DataTable dtz = new DataTable();
                            con.Open();
                            adtpz.Fill(dtz);
                            con.Close();




                            if (dtz.Rows[0][0].ToString() == "FIM DE JORNADA" || dtz.Rows[0][0].ToString() == "PARADA PERNOITE")
                            {
                                //JORNADA NORMAL COM DIFERENÇA NO COMEÇO
                                string sqld = "select TOP 1  ds_macro, hr_posicao from tb_parada  where  cod_cracha=" + login + " and fl_deletado is null and dt_posicao_parada='" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "'  and hr_posicao > (select top 1 hr_posicao from tb_parada where cod_cracha=" + login + " and fl_deletado is null and dt_posicao_parada='" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "'  and ds_macro='FIM DE JORNADA'  OR cod_cracha=" + login + " and fl_deletado is null and dt_posicao_parada='" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "'  and ds_macro='PARADA PERNOITE' order by hr_posicao) order by hr_posicao ";
                                SqlDataAdapter adtpd = new SqlDataAdapter(sqld, con);
                                DataTable dtd = new DataTable();
                                con.Open();
                                adtpd.Fill(dtd);
                                con.Close();

                                if (dtd.Rows.Count > 0)
                                {
                                    string sqlx = "exec sp_tempo_total_dif '" + DateTime.Parse(txtData.Text).ToString("yyyy-MM-dd") + "','" + DateTime.Parse(txtData.Text).ToString("yyyy-MM-dd") + "','" + dtd.Rows[0][1].ToString() + "'," + login + "";
                                    SqlDataAdapter adtpx = new SqlDataAdapter(sqlx, con);
                                    DataTable dtx = new DataTable();
                                    con.Open();
                                    adtpx.Fill(dtx);
                                    con.Close();
                                    grdMotoristas.DataSource = dtx;
                                    grdMotoristas.DataBind();
                                    html = "<br/>";
                                    html += "<a class='btn btn-outline-info' href=JavaScript:ver_demo('" + login + "','" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "','" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "','" + dtd.Rows[0][1].ToString() + "');>Imprimir Diário</a>";
                                    HtmlGenericControl table = this.button;
                                    table.InnerHtml = html;
                                    lblBloco.Text = "BLOCO3a";
                                    btnExcluiMotoristas.Visible = true;
                                }
                                else
                                {
                                    html = "";
                                     HtmlGenericControl table = this.button;
                                    table.InnerHtml = html;
                                }

                            }
                            else
                            {
                                //JORNADA PERNOITE COM DIFERENÇA NO COMEÇO
                                string sqli = "select TOP 1  hr_posicao from tb_parada  where  cod_cracha=" + login + " and fl_deletado is null and dt_posicao_parada='" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "'  and hr_posicao > (select top 1 hr_posicao from tb_parada where cod_cracha=" + login + " and fl_deletado is null and dt_posicao_parada='" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "'  and ds_macro='FIM DE JORNADA'  OR cod_cracha=" + login + " and fl_deletado is null and dt_posicao_parada='" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "'  and ds_macro='PARADA PERNOITE' order by hr_posicao) order by hr_posicao ";
                                SqlDataAdapter adtpi = new SqlDataAdapter(sqli, con);
                                DataTable dti = new DataTable();
                                con.Open();
                                adtpi.Fill(dti);
                                con.Close();

                                string sqlf = "select top 1 hr_posicao from tb_parada where cod_cracha=" + login + " and ds_macro = 'FIM DE JORNADA' and dt_posicao_parada='" + DateTime.Parse(data).AddDays(1).ToString("yyyy-MM-dd") + "' and fl_deletado is null OR cod_cracha=" + login + " and ds_macro='PARADA PERNOITE' and dt_posicao_parada='" + DateTime.Parse(data).AddDays(1).ToString("yyyy-MM-dd") + "' and fl_deletado is null order by hr_posicao";
                                SqlDataAdapter adtpf = new SqlDataAdapter(sqlf, con);
                                DataTable dtf = new DataTable();
                                con.Open();
                                adtpf.Fill(dtf);
                                con.Close();



                                if (dti.Rows.Count > 0 && dtf.Rows.Count > 0)
                                {
                                    string sql4 = "exec sp_tempo_total_noturno '" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "','" + DateTime.Parse(data).AddDays(1).ToString("yyyy-MM-dd") + "','" + dti.Rows[0][0].ToString() + "','" + dtf.Rows[0][0].ToString() + "', " + login + "";
                                    SqlDataAdapter adtp4 = new SqlDataAdapter(sql4, con);
                                    DataTable dt4 = new DataTable();
                                    con.Open();
                                    adtp4.Fill(dt4);
                                    con.Close();

                                    grdMotoristas.DataSource = dt4;
                                    grdMotoristas.DataBind();
                                    html = "<br/>";
                                    html += "<a class='btn btn-outline-info' href=JavaScript:ver_demo2('" + dt2.Rows[0][1].ToString() + "','" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "','" + DateTime.Parse(data).AddDays(1).ToString("yyyy-MM-dd") + "','" + dti.Rows[0][0].ToString() + "','" + dtf.Rows[0][0].ToString() + "');>Imprimir Diário</a>";
                                     HtmlGenericControl table = this.button;
                                    table.InnerHtml = html;
                                    lblBloco.Text = "BLOCO3c";
                                    btnExcluiMotoristas.Visible = true;
                                }
                            }
                        }
                    }


                }
                else
                {
                    string message = "Não há marcações para esse motorista!";
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append("<script type = 'text/javascript'>");
                    sb.Append("window.onload=function(){");
                    sb.Append("alert('");
                    sb.Append(message);
                    sb.Append("')};");
                    sb.Append("</script>");
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
                }
            }
            catch (Exception ex)
            {
                string message = ex.ToString();
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<script type = 'text/javascript'>");
                sb.Append("window.onload=function(){");
                sb.Append("alert('");
                sb.Append(message);
                sb.Append("')};");
                sb.Append("</script>");
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
            }

        }

        public void CarregaGrid()
        {

            //try
            //{

            string data = string.Empty;
            string cod = string.Empty;
            string login = string.Empty;
            
            data = DateTime.Parse(txtData.Text).ToString("yyyy-MM-dd");



            login = txtMotorista.Text;


            string sql1 = "exec sp_tempo_total '" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "','" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "'," + login + "";
            SqlDataAdapter adtp1 = new SqlDataAdapter(sql1, con);
            DataTable dt = new DataTable();
            con.Open();
            adtp1.Fill(dt);
            con.Close();

            //string sql4 = "exec sp_tempo_total_noturno '" + data + "','" + DateTime.Parse(data).AddDays(1).ToString("dd/MM/yyyy") + "'," + login + "";
            //SqlDataAdapter adtp4 = new SqlDataAdapter(sql4, con);
            //DataTable dt4 = new DataTable();
            //con.Open();
            //adtp4.Fill(dt4);
            //con.Close();



            //string sql2 = "select nm_motorista, cod_login from tb_motorista where cod_login='" + login + "'";
            string sql2 = "select  nommot, codmot, funcao,nucleo from tbmotoristas where codmot='" + login + "'";
            SqlDataAdapter adtp2 = new SqlDataAdapter(sql2, con);
            DataTable dt2 = new DataTable();
            con.Open();
            adtp2.Fill(dt2);
            con.Close();

            string sql3 = "select  * from tb_parada where cod_cracha='" + login + "' and dt_posicao_parada='" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "' and fl_deletado is null  order by hr_posicao";
            SqlDataAdapter adtp3 = new SqlDataAdapter(sql3, con);
            DataTable dt3 = new DataTable();
            con.Open();
            adtp3.Fill(dt3);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                if (dt2.Rows.Count > 0)
                {
                    txtNome.Text = dt2.Rows[0][0].ToString();
                    txtFuncao.Text = dt2.Rows[0][2].ToString();
                    txtNucleo.Text =  dt2.Rows[0][3].ToString();
                }
                else
                {
                    txtNome.Text = "NÃO LOCALIZADO"; /*| |  MATRICULA: " + dt2.Rows[0][1].ToString();*/
                }

                if (dt3.Rows.Count > 0)
                {
                    if (dt3.Rows[0][6].ToString() == "FIM DE JORNADA" || dt3.Rows[0][6].ToString() == "PARADA PERNOITE" || dt3.Rows[0][6].ToString() == "PARADA REFEICAO" || dt3.Rows[0][6].ToString() == "RETORNO REFEICAO")
                    {
                        //string sqlx = "select * from tb_parada where cod_cracha='" + login + "' and dt_posicao_parada='" + DateTime.Parse(txtData.Text).ToString("dd/MM/yyyy") + "' and fl_deletado is null and hr_posicao > '"+dt3.Rows[0][4].ToString()+"' order by hr_posicao ";


                        string sqlo = "select ds_macro from tb_parada where cod_cracha='" + login + "' and dt_posicao_parada='" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "' and fl_deletado is null OR cod_cracha='" + login + "' and dt_posicao_parada='" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "' and fl_deletado is null order by hr_posicao";
                        SqlDataAdapter adtpo = new SqlDataAdapter(sqlo, con);
                        DataTable dto = new DataTable();
                        con.Open();
                        adtpo.Fill(dto);
                        con.Close();

                        if (dto.Rows[0][0].ToString() == "INICIO DE JORNADA" || dto.Rows[0][0].ToString() == "REINICIO DE VIAGEM")
                        {
                            grdMotoristas.DataSource = dt;
                            grdMotoristas.DataBind();

                            string html = "<a class='btn btn-outline-info' href=JavaScript:ver_demo('" + dt2.Rows[0][1].ToString() + "','" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "','" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "');>Imprimir Diário</a>";
                             HtmlGenericControl table = this.button;
                            table.InnerHtml = html;
                            lblBloco.Text = "BLOCO1";
                            btnExcluiMotoristas.Visible = true;
                        }
                        else
                        {

                            string sqlt = "select top 1 ds_macro, hr_posicao from tb_parada where cod_cracha='" + login + "' and dt_posicao_parada='" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "' and ds_macro='INICIO DE JORNADA' and fl_deletado is null  order by hr_posicao";
                            SqlDataAdapter adtpt = new SqlDataAdapter(sqlt, con);
                            DataTable dtt = new DataTable();
                            con.Open();
                            adtpt.Fill(dtt);
                            con.Close();

                            if (dtt.Rows.Count > 0)
                            {
                                string sqln = "select ds_macro from tb_parada where cod_cracha='" + login + "' and dt_posicao_parada='" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "' and fl_deletado is null OR cod_cracha='" + login + "' and dt_posicao_parada='" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "' and fl_deletado is null order by hr_posicao";
                                SqlDataAdapter adtpn = new SqlDataAdapter(sqln, con);
                                DataTable dtn = new DataTable();
                                con.Open();
                                adtpn.Fill(dtn);
                                con.Close();

                                string sqli = "select top 1  hr_posicao from tb_parada where cod_cracha=" + login + " and ds_macro = 'INICIO DE JORNADA' and dt_posicao_parada='" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "' and fl_deletado is null OR cod_cracha=" + login + " and ds_macro='REINICIO DE VIAGEM' and dt_posicao_parada='" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "' and fl_deletado is null order by hr_posicao";
                                SqlDataAdapter adtpi = new SqlDataAdapter(sqli, con);
                                DataTable dti = new DataTable();
                                con.Open();
                                adtpi.Fill(dti);
                                con.Close();

                                string sqlf = "select top 1 hr_posicao from tb_parada where cod_cracha=" + login + " and ds_macro = 'FIM DE JORNADA' and dt_posicao_parada='" + DateTime.Parse(data).AddDays(1).ToString("yyyy-MM-dd") + "' and fl_deletado is null OR cod_cracha=" + login + " and ds_macro='PARADA PERNOITE' and dt_posicao_parada='" + DateTime.Parse(data).AddDays(1).ToString("yyyy-MM-dd") + "' and fl_deletado is null order by hr_posicao";
                                SqlDataAdapter adtpf = new SqlDataAdapter(sqlf, con);
                                DataTable dtf = new DataTable();
                                con.Open();
                                adtpf.Fill(dtf);
                                con.Close();



                                if (dti.Rows.Count > 0 && dtf.Rows.Count > 0)
                                {
                                    string sql4 = "exec sp_tempo_total_noturno '" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "','" + DateTime.Parse(data).AddDays(1).ToString("yyyy-MM-dd") + "','" + dti.Rows[0][0].ToString() + "','" + dtf.Rows[0][0].ToString() + "', " + login + "";
                                    SqlDataAdapter adtp4 = new SqlDataAdapter(sql4, con);
                                    DataTable dt4 = new DataTable();
                                    con.Open();
                                    adtp4.Fill(dt4);
                                    con.Close();

                                    grdMotoristas.DataSource = dt4;
                                    grdMotoristas.DataBind();
                                    string html = "<a class='btn btn-outline-info' href=JavaScript:ver_demo2('" + dt2.Rows[0][1].ToString() + "','" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "','" + DateTime.Parse(data).AddDays(1).ToString("yyyy-MM-dd") + "','" + dti.Rows[0][0].ToString() + "','" + dtf.Rows[0][0].ToString() + "');>Imprimir Diário</a>";
                                     HtmlGenericControl table = this.button;
                                    table.InnerHtml = html;
                                    lblBloco.Text = "BLOCO2a";
                                    btnExcluiMotoristas.Visible = true;
                                }
                                else
                                {
                                    string sqlx = "exec sp_tempo_total_dif '" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "','" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "','" + dt3.Rows[0][4].ToString() + "'," + login + "";
                                    SqlDataAdapter adtpx = new SqlDataAdapter(sqlx, con);
                                    DataTable dtx = new DataTable();
                                    con.Open();
                                    adtpx.Fill(dtx);
                                    con.Close();
                                    grdMotoristas.DataSource = dtx;
                                    grdMotoristas.DataBind();
                                    string html = "<a class='btn btn-outline-info' href=JavaScript:ver_demo('" + dt2.Rows[0][1].ToString() + "','" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "','" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "');>Imprimir Diário</a>";
                                     HtmlGenericControl table = this.button;
                                    table.InnerHtml = html;
                                    lblBloco.Text = "BLOCO2b";
                                    btnExcluiMotoristas.Visible = true;
                                }





                            }
                            else
                            {
                                string sqlx = "exec sp_tempo_total_dif '" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "','" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "','" + dt3.Rows[0][4].ToString() + "'," + login + "";
                                SqlDataAdapter adtpx = new SqlDataAdapter(sqlx, con);
                                DataTable dtx = new DataTable();
                                con.Open();
                                adtpx.Fill(dtx);
                                con.Close();
                                grdMotoristas.DataSource = dtx;
                                grdMotoristas.DataBind();
                                string html = "<a class='btn btn-outline-info' href=JavaScript:ver_demo('" + dt2.Rows[0][1].ToString() + "','" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "','" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "');>Imprimir Diário</a>";
                                 HtmlGenericControl table = this.button;
                                table.InnerHtml = html;
                                lblBloco.Text = "BLOCO2c";
                                btnExcluiMotoristas.Visible = true;
                            }






                        }






                    }
                    else if (dt3.Rows[0][6].ToString() == "INICIO DE JORNADA" || dt3.Rows[0][6].ToString() == "REINICIO DE VIAGEM")
                    {



                        string sqlo = "select  * from tb_parada where cod_cracha='" + login + "' and ds_macro='FIM DE JORNADA' and dt_posicao_parada='" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "' and fl_deletado is null OR cod_cracha='" + login + "' and ds_macro='PARADA PERNOITE' and dt_posicao_parada='" + data + "' and fl_deletado is null order by hr_posicao";
                        SqlDataAdapter adtpo = new SqlDataAdapter(sqlo, con);
                        DataTable dto = new DataTable();
                        con.Open();
                        adtpo.Fill(dto);
                        con.Close();

                        if (dto.Rows.Count > 0)
                        {
                            string sqlK = "select top 1  hr_posicao, ds_macro from tb_parada where cod_cracha=" + login + " and ds_macro in ('INICIO DE JORNADA','REINICIO DE VIAGEM') and dt_posicao_parada='" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "' and fl_deletado is null  order by hr_posicao";
                            SqlDataAdapter adtpK = new SqlDataAdapter(sqlK, con);
                            DataTable dtK = new DataTable();
                            con.Open();
                            adtpK.Fill(dtK);
                            con.Close();

                            if (dtK.Rows[0][1].ToString() == "REINICIO DE VIAGEM")
                            {
                                string sqli = "select top 1  hr_posicao from tb_parada where  cod_cracha=" + login + " and ds_macro='REINICIO DE VIAGEM' and dt_posicao_parada='" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "' and fl_deletado is null order by hr_posicao";
                                SqlDataAdapter adtpi = new SqlDataAdapter(sqli, con);
                                DataTable dti = new DataTable();
                                con.Open();
                                adtpi.Fill(dti);
                                con.Close();

                                string sqlf = "select top 1 hr_posicao from tb_parada where cod_cracha=" + login + " and ds_macro = 'FIM DE JORNADA' and dt_posicao_parada='" + DateTime.Parse(data).AddDays(1).ToString("yyyy-MM-dd") + "' and fl_deletado is null OR cod_cracha=" + login + " and ds_macro='PARADA PERNOITE' and dt_posicao_parada='" + DateTime.Parse(data).AddDays(1).ToString("yyyy-MM-dd") + "' and fl_deletado is null order by hr_posicao";
                                SqlDataAdapter adtpf = new SqlDataAdapter(sqlf, con);
                                DataTable dtf = new DataTable();
                                con.Open();
                                adtpf.Fill(dtf);
                                con.Close();



                                if (dti.Rows.Count > 0 && dtf.Rows.Count > 0)
                                {
                                    string sql4 = "exec sp_tempo_total_noturno '" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "','" + DateTime.Parse(data).AddDays(1).ToString("yyyy-MM-dd") + "','" + dti.Rows[0][0].ToString() + "','" + dtf.Rows[0][0].ToString() + "', " + login + "";
                                    SqlDataAdapter adtp4 = new SqlDataAdapter(sql4, con);
                                    DataTable dt4 = new DataTable();
                                    con.Open();
                                    adtp4.Fill(dt4);
                                    con.Close();

                                    grdMotoristas.DataSource = dt4;
                                    grdMotoristas.DataBind();
                                    string html = "<a class='btn btn-outline-info' href=JavaScript:ver_demo2('" + dt2.Rows[0][1].ToString() + "','" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "','" + DateTime.Parse(data).AddDays(1).ToString("yyyy-MM-dd") + "','" + dti.Rows[0][0].ToString() + "','" + dtf.Rows[0][0].ToString() + "');>Imprimir Diário</a>";
                                     HtmlGenericControl table = this.button;
                                    table.InnerHtml = html;
                                    lblBloco.Text = "BLOCO3a";
                                    btnExcluiMotoristas.Visible = true;
                                }
                                else
                                {
                                    grdMotoristas.DataSource = dt;
                                    grdMotoristas.DataBind();
                                    string html = "<a class='btn btn-outline-info' href=JavaScript:ver_demo('" + dt2.Rows[0][1].ToString() + "','" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "','" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "');>Imprimir Diário</a>";
                                     HtmlGenericControl table = this.button;
                                    table.InnerHtml = html;
                                    lblBloco.Text = "BLOCO3b";
                                    btnExcluiMotoristas.Visible = true;
                                }
                            }
                            else
                            {
                                string sqli = "select top 1  hr_posicao from tb_parada where cod_cracha=" + login + " and ds_macro = 'INICIO DE JORNADA' and dt_posicao_parada='" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "' and fl_deletado is null  order by hr_posicao";
                                SqlDataAdapter adtpi = new SqlDataAdapter(sqli, con);
                                DataTable dti = new DataTable();
                                con.Open();
                                adtpi.Fill(dti);
                                con.Close();

                                string sqlf = "select top 1 hr_posicao from tb_parada where cod_cracha=" + login + " and ds_macro = 'FIM DE JORNADA' and dt_posicao_parada='" + DateTime.Parse(data).AddDays(1).ToString("yyyy-MM-dd") + "' and fl_deletado is null OR cod_cracha=" + login + " and ds_macro='PARADA PERNOITE' and dt_posicao_parada='" + DateTime.Parse(data).AddDays(1).ToString("yyyy-MM-dd") + "' and fl_deletado is null order by hr_posicao";
                                SqlDataAdapter adtpf = new SqlDataAdapter(sqlf, con);
                                DataTable dtf = new DataTable();
                                con.Open();
                                adtpf.Fill(dtf);
                                con.Close();



                                if (dti.Rows.Count > 0 && dtf.Rows.Count > 0)
                                {
                                    string sql4 = "exec sp_tempo_total_noturno '" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "','" + DateTime.Parse(data).AddDays(1).ToString("yyyy-MM-dd") + "','" + dti.Rows[0][0].ToString() + "','" + dtf.Rows[0][0].ToString() + "', " + login + "";
                                    SqlDataAdapter adtp4 = new SqlDataAdapter(sql4, con);
                                    DataTable dt4 = new DataTable();
                                    con.Open();
                                    adtp4.Fill(dt4);
                                    con.Close();

                                    grdMotoristas.DataSource = dt4;
                                    grdMotoristas.DataBind();
                                    string html = "<a class='btn btn-outline-info' href=JavaScript:ver_demo2('" + dt2.Rows[0][1].ToString() + "','" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "','" + DateTime.Parse(data).AddDays(1).ToString("yyyy-MM-dd") + "','" + dti.Rows[0][0].ToString() + "','" + dtf.Rows[0][0].ToString() + "');>Imprimir Diário</a>";
                                     HtmlGenericControl table = this.button;
                                    table.InnerHtml = html;
                                    lblBloco.Text = "BLOCO3C";
                                    btnExcluiMotoristas.Visible = true;
                                }
                                else
                                { 
                                    grdMotoristas.DataSource = dt;
                                    grdMotoristas.DataBind();
                                    string html = "<a class='btn btn-outline-info' href=JavaScript:ver_demo('" + dt2.Rows[0][1].ToString() + "','" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "','" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "');>Imprimir Diário</a>";
                                     HtmlGenericControl table = this.button;
                                    table.InnerHtml = html;
                                    lblBloco.Text = "BLOCO3D";
                                    btnExcluiMotoristas.Visible = true;
                                }
                            }







                            //grdMotoristas.DataSource = dt;
                            //grdMotoristas.DataBind();
                            //string html = "<a class='btn btn-outline-info' href=JavaScript:ver_demo('" + dt2.Rows[0][1].ToString() + "','" + data + "','" + data + "');>Imprimir Diário</a>";
                            // HtmlGenericControl table = this.button;
                            //table.InnerHtml = html;
                            //lblBloco.Text = "BLOCO3";
                        }
                        else
                        {
                            string sqli = "select top 1  hr_posicao from tb_parada where cod_cracha=" + login + " and ds_macro = 'INICIO DE JORNADA' and dt_posicao_parada='" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "' and fl_deletado is null OR cod_cracha=" + login + " and ds_macro='REINICIO DE VIAGEM' and dt_posicao_parada='" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "' and fl_deletado is null order by hr_posicao";
                            SqlDataAdapter adtpi = new SqlDataAdapter(sqli, con);
                            DataTable dti = new DataTable();
                            con.Open();
                            adtpi.Fill(dti);
                            con.Close();

                            string sqlf = "select top 1 hr_posicao from tb_parada where cod_cracha=" + login + " and ds_macro = 'FIM DE JORNADA' and dt_posicao_parada='" + DateTime.Parse(data).AddDays(1).ToString("yyyy-MM-dd") + "' and fl_deletado is null OR cod_cracha=" + login + " and ds_macro='PARADA PERNOITE' and dt_posicao_parada='" + DateTime.Parse(data).AddDays(1).ToString("yyyy-MM-dd") + "' and fl_deletado is null order by hr_posicao";
                            SqlDataAdapter adtpf = new SqlDataAdapter(sqlf, con);
                            DataTable dtf = new DataTable();
                            con.Open();
                            adtpf.Fill(dtf);
                            con.Close();



                            if (dti.Rows.Count > 0 && dtf.Rows.Count > 0)
                            {
                                string sql4 = "exec sp_tempo_total_noturno '" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "','" + DateTime.Parse(data).AddDays(1).ToString("yyyy-MM-dd") + "','" + dti.Rows[0][0].ToString() + "','" + dtf.Rows[0][0].ToString() + "', " + login + "";
                                SqlDataAdapter adtp4 = new SqlDataAdapter(sql4, con);
                                DataTable dt4 = new DataTable();
                                con.Open();
                                adtp4.Fill(dt4);
                                con.Close();

                                grdMotoristas.DataSource = dt4;
                                grdMotoristas.DataBind();
                                string html = "<a class='btn btn-outline-info' href=JavaScript:ver_demo2('" + dt2.Rows[0][1].ToString() + "','" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "','" + DateTime.Parse(data).AddDays(1).ToString("yyyy-MM-dd") + "','" + dti.Rows[0][0].ToString() + "','" + dtf.Rows[0][0].ToString() + "');>Imprimir Diário</a>";
                                 HtmlGenericControl table = this.button;
                                table.InnerHtml = html;
                                lblBloco.Text = "BLOCO5";
                                btnExcluiMotoristas.Visible = true;
                            }
                            else
                            {
                                grdMotoristas.DataSource = dt;
                                grdMotoristas.DataBind();
                                string html = "<a class='btn btn-outline-info' href=JavaScript:ver_demo('" + dt2.Rows[0][1].ToString() + "','" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "','" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "');>Imprimir Diário</a>";
                                 HtmlGenericControl table = this.button;
                                table.InnerHtml = html;
                                lblBloco.Text = "BLOCO6";
                                btnExcluiMotoristas.Visible = true;
                            }
                        }


                    }





                }
                else if (dt3.Rows[0][6].ToString() == "INICIO DE JORNADA" || dt3.Rows[0][6].ToString() == "REINICIO DE VIAGEM")
                {
                    grdMotoristas.DataSource = dt;
                    grdMotoristas.DataBind();

                    string html = "<a class='btn btn-outline-info' href=JavaScript:ver_demo('" + dt2.Rows[0][1].ToString() + "','" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "','" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "');>Imprimir Diário</a>";
                     HtmlGenericControl table = this.button;
                    table.InnerHtml = html;
                    lblBloco.Text = "BLOCO4";
                    btnExcluiMotoristas.Visible = true;
                }

                else
                {

                    string sqli = "select top 1  hr_posicao from tb_parada where cod_cracha=" + login + " and ds_macro = 'INICIO DE JORNADA' and dt_posicao_parada='" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "' and fl_deletado is null OR cod_cracha=" + login + " and ds_macro='REINICIO DE VIAGEM' and dt_posicao_parada='" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "' and fl_deletado is null order by hr_posicao";
                    SqlDataAdapter adtpi = new SqlDataAdapter(sqli, con);
                    DataTable dti = new DataTable();
                    con.Open();
                    adtpi.Fill(dti);
                    con.Close();

                    string sqlf = "select top 1 hr_posicao from tb_parada where cod_cracha=" + login + " and ds_macro = 'FIM DE JORNADA' and dt_posicao_parada='" + DateTime.Parse(data).AddDays(1).ToString("yyyy-MM-dd") + "' and fl_deletado is null OR cod_cracha=" + login + " and ds_macro='PARADA PERNOITE' and dt_posicao_parada='" + DateTime.Parse(data).AddDays(1).ToString("yyyy-MM-dd") + "' and fl_deletado is null order by hr_posicao";
                    SqlDataAdapter adtpf = new SqlDataAdapter(sqlf, con);
                    DataTable dtf = new DataTable();
                    con.Open();
                    adtpf.Fill(dtf);
                    con.Close();



                    if (dti.Rows.Count > 0 && dtf.Rows.Count > 0)
                    {
                        string sql4 = "exec sp_tempo_total_noturno '" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "','" + DateTime.Parse(data).AddDays(1).ToString("yyyy-MM-dd") + "','" + dti.Rows[0][0].ToString() + "','" + dtf.Rows[0][0].ToString() + "', " + login + "";
                        SqlDataAdapter adtp4 = new SqlDataAdapter(sql4, con);
                        DataTable dt4 = new DataTable();
                        con.Open();
                        adtp4.Fill(dt4);
                        con.Close();

                        grdMotoristas.DataSource = dt4;
                        grdMotoristas.DataBind();
                        string html = "<a class='btn btn-outline-info' href=JavaScript:ver_demo2('" + dt2.Rows[0][1].ToString() + "','" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "','" + DateTime.Parse(data).AddDays(1).ToString("yyyy-MM-dd") + "','" + dti.Rows[0][0].ToString() + "','" + dtf.Rows[0][0].ToString() + "');>Imprimir Diário</a>";
                         HtmlGenericControl table = this.button;
                        table.InnerHtml = html;
                        lblBloco.Text = "BLOCO5";
                        btnExcluiMotoristas.Visible = true;
                    }
                    else
                    {
                        grdMotoristas.DataSource = dt;
                        grdMotoristas.DataBind();
                        string html = "<a class='btn btn-outline-info' href=JavaScript:ver_demo('" + dt2.Rows[0][1].ToString() + "','" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "','" + DateTime.Parse(data).ToString("yyyy-MM-dd") + "');>Imprimir Diário</a>";
                         HtmlGenericControl table = this.button;
                        table.InnerHtml = html;
                        lblBloco.Text = "BLOCO6";
                        btnExcluiMotoristas.Visible = true;
                    }





                }



                


            }
            else
            {
                string message = "Não há marcações para esse motorista!";
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<script type = 'text/javascript'>");
                sb.Append("window.onload=function(){");
                sb.Append("alert('");
                sb.Append(message);
                sb.Append("')};");
                sb.Append("</script>");
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
            }
            //}
            //catch(Exception ex)
            //{
            //    string message = ex.ToString();
            //    System.Text.StringBuilder sb = new System.Text.StringBuilder();
            //    sb.Append("<script type = 'text/javascript'>");
            //    sb.Append("window.onload=function(){");
            //    sb.Append("alert('");
            //    sb.Append(message);
            //    sb.Append("')};");
            //    sb.Append("</script>");
            //    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
            //}
            btnExcluiMotoristas.Visible = true;
            btnExcluiTodas.Visible = false;

        }

        public void AtualizaJornada()
        {
            //string sql = "select cod_transmissao, nr_idveiculo, nr_codmacro, ds_mensagem, ds_nomemesagem, ds_textomensagem,CAST(dt_posicao as date) data, CAST(dt_posicao as time(3)) hora,ds_rua, ds_cidade,ds_uf   from tb_transmissao_historico where nr_codmacro between 1 and 20 and dt_posicao > '" + DateTime.Now.AddDays(-2).ToString("dd/MM/yyyy 00:00:00.000") + "' order by dt_posicao asc  ";
            //string sql = "select cod_transmissao, cod_idveiculo, nr_codmacro, cod_cracha,ds_macro, dt_posicao_parada, hr_posicao, ds_rua, ds_cidade, ds_uf  from tb_parada where dt_posicao_parada ='"+DateTime.Parse(txtData.Text).ToString("dd/MM/yyyy")+"' and cod_cracha="+txtMotorista.Text+"order by dt_posicao asc";

            //string sql = "select cod_transmissao, nr_idveiculo, nr_codmacro, ds_mensagem, ds_nomemesagem, ds_textomensagem,CAST(dt_posicao as date) data, CAST(dt_posicao as time(3)) hora,ds_rua, ds_cidade,ds_uf   from tb_transmissao_historico where nr_codmacro between 1 and 20 and dt_posicao > '" + DateTime.Now.AddDays(-2).ToString("dd/MM/yyyy 00:00:00.000") + "' order by dt_posicao asc  ";
            // string sql = "select cod_transmissao, nr_idveiculo, nr_codmacro, ds_mensagem, ds_nomemesagem, ds_textomensagem,CAST(dt_pacote as date) data, CAST(dt_pacote as time(3)) hora,ds_rua, ds_cidade,ds_uf   from tb_transmissao_historico where ds_mensagem  like'%_%'  and nr_codmacro <> 0 and dt_posicao > '" + DateTime.Now.AddDays(-4).ToString("dd/MM/yyyy 00:00:00.000") + "' order by dt_pacote asc";
            //string sql = "select cod_transmissao, nr_idveiculo, nr_codmacro, ds_mensagem, ds_nomemesagem, ds_textomensagem,CAST(dt_pacote as date) data, CAST(dt_pacote as time(3)) hora,ds_rua, ds_cidade,ds_uf   from tb_transmissao_historico where ds_mensagem  like'%_%'  and nr_codmacro <> 0 and dt_posicao between '19/01/2017 01:00:00' and '19/01/2017 23:59:59' order by dt_pacote asc";
            string sql = "select cod_transmissao, cod_idveiculo, nr_codmacro, cod_cracha, ds_macro,CAST(dt_posicao_parada as date) as dt_parada , hr_posicao, ds_rua, ds_cidade, ds_uf, cod_ref_parada from tb_parada where dt_posicao_parada ='" + DateTime.Parse(txtData.Text).ToString("yyyy-MM-dd") + "'  order by dt_posicao_parada, hr_posicao,cod_cracha";

            SqlDataAdapter adpt = new SqlDataAdapter(sql, con);

            DataTable dt = new DataTable();
            con.Open();
            adpt.Fill(dt);
            con.Close();


            for (int x = 0; x < dt.Rows.Count; x++)
            {


                if (dt.Rows[x][3].ToString() != string.Empty)
                {



                    string login = dt.Rows[x][3].ToString();

                    valido = Regex.IsMatch(login, @"^\d{3}$");
                    valido2 = Regex.IsMatch(login, @"^\d{4}$");



                    if (dt.Rows[x][4].ToString() == "INICIO DE JORNADA")
                    {
                        if (valido || valido2)
                        {

                            // string sqlv = "select nr_idveiculo from tb_jornada where nr_idveiculo=" + dt.Rows[x][1].ToString() + " and dt_jornada='" + DateTime.Parse(dt.Rows[x][6].ToString()).ToString("dd/MM/yyyy") + "'";
                            string sqlv = "select nr_idveiculo from tb_jornada where  dt_jornada='" + DateTime.Parse(dt.Rows[x][5].ToString()).ToString("yyyy-MM-dd") + "' and cod_login=" + login + " ";
                            SqlDataAdapter adptv = new SqlDataAdapter(sqlv, con);
                            DataTable dtv = new DataTable();
                            con.Open();
                            adptv.Fill(dtv);
                            con.Close();

                            if (dtv.Rows.Count < 1)
                            {
                                string sql2 = "insert into tb_jornada (nr_id_giga, dt_jornada, cod_login, hr_inicio_jornada, cod_transmissao) ";
                                sql2 += "values (@nr_id_giga,@dt_jornada, @cod_login, @hr_inicio_jornada, @cod_transmissao) ";


                                SqlCommand cmd = new SqlCommand(sql2, con);
                                cmd.Parameters.AddWithValue("@nr_id_giga", dt.Rows[x][1].ToString());
                                cmd.Parameters.AddWithValue("@dt_jornada", DateTime.Parse(dt.Rows[x][5].ToString()).ToString("yyyy-MM-dd"));
                                cmd.Parameters.AddWithValue("@cod_login", login);
                                cmd.Parameters.AddWithValue("@hr_inicio_jornada", DateTime.Parse(dt.Rows[x][6].ToString()).ToString("HH:mm:ss.000"));
                                cmd.Parameters.AddWithValue("@cod_transmissao", dt.Rows[x][0].ToString());

                                try
                                {
                                    con.Open();
                                    cmd.ExecuteNonQuery();
                                    con.Close();
                                }
                                catch (Exception ex)
                                {
                                    StreamWriter write = new StreamWriter(@"C:\Erros_SisLog\Log.txt", true);
                                    write.WriteLine("Erro Inserir na Tabela Jornada Inicio Jornada: " + ex.ToString() + " - " + DateTime.Now.ToString());
                                    write.Flush();
                                    write.Close();
                                    con.Close();

                                }
                            }
                        }

                    }


                    if (dt.Rows[x][4].ToString() == "INICIO JORNADA")
                    {
                        if (valido || valido2)
                        {
                            // string sqlv = "select nr_idveiculo from tb_jornada where nr_idveiculo=" + dt.Rows[x][1].ToString() + " and dt_jornada='" + DateTime.Parse(dt.Rows[x][6].ToString()).ToString("dd/MM/yyyy") + "'";
                            string sqlv = "select nr_idveiculo from tb_jornada where dt_jornada='" + DateTime.Parse(dt.Rows[x][5].ToString()).ToString("yyyy-MM-dd") + "' and cod_login=" + login + " and hr_inicio_jornada is null ";
                            SqlDataAdapter adptv = new SqlDataAdapter(sqlv, con);
                            DataTable dtv = new DataTable();
                            con.Open();
                            adptv.Fill(dtv);
                            con.Close();

                            if (dtv.Rows.Count > 0)
                            {
                                string sql2 = "insert into tb_jornada (nr_idveiculo, dt_jornada, cod_login, hr_inicio_jornada, cod_transmissao) ";
                                sql2 += "values (@nr_idveiculo, @dt_jornada, @cod_login, @hr_inicio_jornada, @cod_transmissao) ";


                                SqlCommand cmd = new SqlCommand(sql2, con);
                                cmd.Parameters.AddWithValue("@nr_idveiculo", dt.Rows[x][1].ToString());
                                cmd.Parameters.AddWithValue("@dt_jornada", DateTime.Parse(dt.Rows[x][5].ToString()).ToString("yyyy-MM-dd"));
                                cmd.Parameters.AddWithValue("@cod_login", login);
                                cmd.Parameters.AddWithValue("@hr_inicio_jornada", DateTime.Parse(dt.Rows[x][6].ToString()).ToString("HH:mm:ss.000"));
                                cmd.Parameters.AddWithValue("@cod_transmissao", dt.Rows[x][0].ToString());

                                try
                                {
                                    con.Open();
                                    cmd.ExecuteNonQuery();
                                    con.Close();
                                }
                                catch (Exception ex)
                                {
                                    StreamWriter write = new StreamWriter(@"C:\Erros_SisLog\Log.txt", true);
                                    write.WriteLine("Erro Inserir na Tabela Jornada Inicio Jornada: " + ex.ToString() + " - " + DateTime.Now.ToString());
                                    write.Flush();
                                    write.Close();
                                    con.Close();

                                }
                            }

                        }

                    }




                    if (dt.Rows[x][4].ToString() == "PARADA")
                    {
                        if (valido || valido2)
                        {

                            if (dt.Rows[x][10].ToString() == "14")
                            {
                                string sqla1 = "select nr_idveiculo, dt_jornada, cod_jornada from tb_jornada where cod_login=" + login + " and  dt_jornada='" + DateTime.Parse(dt.Rows[x][5].ToString()).ToString("yyyy-MM-dd") + "' and hr_inicio_intervalo is null ";
                                SqlDataAdapter adpta = new SqlDataAdapter(sqla1, con);
                                DataTable dta = new DataTable();
                                con.Open();
                                adpta.Fill(dta);
                                con.Close();

                                if (dta.Rows.Count > 0)
                                {
                                    string sqla = "update tb_jornada set hr_inicio_intervalo=@hr_inicio_intervalo,ds_rua=@ds_rua, ds_cidade=@ds_cidade, ds_uf=@ds_uf, cod_transmissao=@cod_transmissao, nr_idveiculo=@nr_idveiculo where dt_jornada=@dt_jornada and cod_login=@cod_login";
                                    SqlCommand cmda = new SqlCommand(sqla, con);
                                    cmda.Parameters.AddWithValue("@hr_inicio_intervalo", DateTime.Parse(dt.Rows[x][6].ToString()).ToString("HH:mm:ss.000"));
                                    cmda.Parameters.AddWithValue("@dt_jornada", DateTime.Parse(dt.Rows[x][5].ToString()).ToString("yyyy-MM-dd"));
                                    cmda.Parameters.AddWithValue("@nr_idveiculo", dt.Rows[x][1].ToString());
                                    cmda.Parameters.AddWithValue("@ds_rua", dt.Rows[x][7].ToString());
                                    cmda.Parameters.AddWithValue("@ds_cidade", dt.Rows[x][8].ToString());
                                    cmda.Parameters.AddWithValue("@ds_uf", dt.Rows[x][9].ToString());
                                    cmda.Parameters.AddWithValue("@cod_transmissao", dt.Rows[x][0].ToString());
                                    cmda.Parameters.AddWithValue("@cod_login", login);

                                    try
                                    {
                                        con.Open();
                                        cmda.ExecuteNonQuery();
                                        con.Close();
                                    }
                                    catch (Exception ex)
                                    {
                                        StreamWriter write = new StreamWriter(@"C:\Erros_SisLog\Log.txt", true);
                                        write.WriteLine("Erro Inserir na Tabela Jornada Trecho: " + ex.ToString() + " - " + DateTime.Now.ToString());
                                        write.Flush();
                                        write.Close();
                                        con.Close();

                                    }
                                }


                            }
                            else
                            {

                                string sqlta = "select nr_idveiculo, hr_inicio_intervalo, cod_jornada, cod_transmissao from tb_jornada where cod_login=" + login + " and dt_jornada='" + DateTime.Parse(dt.Rows[x][5].ToString()).ToString("yyyy-MM-dd") + "' and hr_inicio_intervalo is not null and hr_fim_intervalo is null ";
                                SqlDataAdapter adptta = new SqlDataAdapter(sqlta, con);
                                DataTable dtta = new DataTable();
                                con.Open();
                                adptta.Fill(dtta);
                                con.Close();



                                if (dtta.Rows.Count > 0)
                                {
                                    TimeSpan hr_ini = new TimeSpan();
                                    TimeSpan hr_fim = new TimeSpan();

                                    hr_ini = TimeSpan.Parse(dtta.Rows[0][1].ToString());
                                    hr_fim = TimeSpan.Parse(dt.Rows[x][6].ToString());

                                    if (hr_fim > hr_ini)
                                    {
                                        string sqlf = "update tb_jornada set hr_fim_intervalo=@hr_fim_intervalo, nr_idveiculo=@nr_idveiculo where  dt_jornada=@dt_jornada and cod_login=@cod_login";


                                        SqlCommand cmdf = new SqlCommand(sqlf, con);
                                        cmdf.Parameters.AddWithValue("@nr_idveiculo", dt.Rows[x][1].ToString());
                                        cmdf.Parameters.AddWithValue("@dt_jornada", DateTime.Parse(dt.Rows[x][5].ToString()).ToString("yyyy-MM-dd"));
                                        cmdf.Parameters.AddWithValue("@cod_login", login);
                                        // cmdf.Parameters.AddWithValue("@cod_jornada", dtta.Rows[0][2].ToString());
                                        cmdf.Parameters.AddWithValue("@hr_fim_intervalo", DateTime.Parse(dt.Rows[x][6].ToString()).ToString("HH:mm:ss.000"));

                                        try
                                        {
                                            con.Open();
                                            cmdf.ExecuteNonQuery();
                                            con.Close();
                                        }
                                        catch (Exception ex)
                                        {
                                            StreamWriter write = new StreamWriter(@"C:\Erros_SisLog\Log.txt", true);
                                            write.WriteLine("Erro Inserir na Tabela Jornada Reinicio Viagem: " + ex.ToString() + " - " + DateTime.Now.ToString());
                                            write.Flush();
                                            write.Close();
                                            con.Close();

                                        }
                                    }
                                }

                            }

                        }

                    }

                    if (dt.Rows[x][4].ToString() == "PARADA CLIENTE / FORNECEDOR")
                    {
                        if (valido || valido2)
                        {

                            if (dt.Rows[x][10].ToString() == "14")
                            {
                                string sqla1 = "select nr_idveiculo, dt_jornada, cod_jornada from tb_jornada where cod_login=" + login + " and  dt_jornada='" + DateTime.Parse(dt.Rows[x][5].ToString()).ToString("yyyy-MM-dd") + "' and hr_inicio_intervalo is null ";
                                SqlDataAdapter adpta = new SqlDataAdapter(sqla1, con);
                                DataTable dta = new DataTable();
                                con.Open();
                                adpta.Fill(dta);
                                con.Close();

                                if (dta.Rows.Count > 0)
                                {
                                    string sqla = "update tb_jornada set hr_inicio_intervalo=@hr_inicio_intervalo,ds_rua=@ds_rua, ds_cidade=@ds_cidade, ds_uf=@ds_uf, cod_transmissao=@cod_transmissao, nr_idveiculo=@nr_idveiculo where dt_jornada=@dt_jornada and cod_login=@cod_login";
                                    SqlCommand cmda = new SqlCommand(sqla, con);
                                    cmda.Parameters.AddWithValue("@hr_inicio_intervalo", DateTime.Parse(dt.Rows[x][6].ToString()).ToString("HH:mm:ss.000"));
                                    cmda.Parameters.AddWithValue("@dt_jornada", DateTime.Parse(dt.Rows[x][5].ToString()).ToString("yyyy-MM-dd"));
                                    cmda.Parameters.AddWithValue("@nr_idveiculo", dt.Rows[x][1].ToString());
                                    cmda.Parameters.AddWithValue("@ds_rua", dt.Rows[x][7].ToString());
                                    cmda.Parameters.AddWithValue("@ds_cidade", dt.Rows[x][8].ToString());
                                    cmda.Parameters.AddWithValue("@ds_uf", dt.Rows[x][9].ToString());
                                    cmda.Parameters.AddWithValue("@cod_transmissao", dt.Rows[x][0].ToString());
                                    cmda.Parameters.AddWithValue("@cod_login", login);

                                    try
                                    {
                                        con.Open();
                                        cmda.ExecuteNonQuery();
                                        con.Close();
                                    }
                                    catch (Exception ex)
                                    {
                                        StreamWriter write = new StreamWriter(@"C:\Erros_SisLog\Log.txt", true);
                                        write.WriteLine("Erro Inserir na Tabela Jornada Trecho: " + ex.ToString() + " - " + DateTime.Now.ToString());
                                        write.Flush();
                                        write.Close();
                                        con.Close();

                                    }
                                }


                            }
                            else
                            {

                                string sqlta = "select nr_idveiculo, hr_inicio_intervalo, cod_jornada, cod_transmissao from tb_jornada where cod_login=" + login + " and dt_jornada='" + DateTime.Parse(dt.Rows[x][5].ToString()).ToString("yyyy-MM-dd") + "' and hr_inicio_intervalo is not null and hr_fim_intervalo is null ";
                                SqlDataAdapter adptta = new SqlDataAdapter(sqlta, con);
                                DataTable dtta = new DataTable();
                                con.Open();
                                adptta.Fill(dtta);
                                con.Close();



                                if (dtta.Rows.Count > 0)
                                {
                                    TimeSpan hr_ini = new TimeSpan();
                                    TimeSpan hr_fim = new TimeSpan();

                                    hr_ini = TimeSpan.Parse(dtta.Rows[0][1].ToString());
                                    hr_fim = TimeSpan.Parse(dt.Rows[x][6].ToString());

                                    if (hr_fim > hr_ini)
                                    {
                                        string sqlf = "update tb_jornada set hr_fim_intervalo=@hr_fim_intervalo, nr_idveiculo=@nr_idveiculo where  dt_jornada=@dt_jornada and cod_login=@cod_login";


                                        SqlCommand cmdf = new SqlCommand(sqlf, con);
                                        cmdf.Parameters.AddWithValue("@nr_idveiculo", dt.Rows[x][1].ToString());
                                        cmdf.Parameters.AddWithValue("@dt_jornada", DateTime.Parse(dt.Rows[x][5].ToString()).ToString("yyyy-MM-dd"));
                                        cmdf.Parameters.AddWithValue("@cod_login", login);
                                        // cmdf.Parameters.AddWithValue("@cod_jornada", dtta.Rows[0][2].ToString());
                                        cmdf.Parameters.AddWithValue("@hr_fim_intervalo", DateTime.Parse(dt.Rows[x][6].ToString()).ToString("HH:mm:ss.000"));

                                        try
                                        {
                                            con.Open();
                                            cmdf.ExecuteNonQuery();
                                            con.Close();
                                        }
                                        catch (Exception ex)
                                        {
                                            StreamWriter write = new StreamWriter(@"C:\Erros_SisLog\Log.txt", true);
                                            write.WriteLine("Erro Inserir na Tabela Jornada Reinicio Viagem: " + ex.ToString() + " - " + DateTime.Now.ToString());
                                            write.Flush();
                                            write.Close();
                                            con.Close();

                                        }
                                    }
                                }
                            }
                        }
                    }

                    else if (dt.Rows[x][4].ToString() == "PARADA INTERNA")
                    {
                        if (valido || valido2)
                        {

                            if (dt.Rows[x][10].ToString() == "14")
                            {
                                string sqla1 = "select nr_idveiculo, dt_jornada, cod_jornada from tb_jornada where cod_login=" + login + " and  dt_jornada='" + DateTime.Parse(dt.Rows[x][5].ToString()).ToString("yyyy-MM-dd") + "' and hr_inicio_intervalo is  null ";
                                SqlDataAdapter adpta = new SqlDataAdapter(sqla1, con);
                                DataTable dta = new DataTable();
                                con.Open();
                                adpta.Fill(dta);
                                con.Close();

                                if (dta.Rows.Count > 0)
                                {
                                    string sqla = "update tb_jornada set hr_inicio_intervalo=@hr_inicio_intervalo,ds_rua=@ds_rua, ds_cidade=@ds_cidade, ds_uf=@ds_uf, nr_idveiculo=@nr_idveiculo, cod_transmissao=@cod_transmissao where dt_jornada=@dt_jornada and cod_login=@cod_login";
                                    SqlCommand cmda = new SqlCommand(sqla, con);
                                    cmda.Parameters.AddWithValue("@hr_inicio_intervalo", DateTime.Parse(dt.Rows[x][6].ToString()).ToString("HH:mm:ss.000"));
                                    cmda.Parameters.AddWithValue("@dt_jornada", DateTime.Parse(dt.Rows[x][5].ToString()).ToString("yyyy-MM-dd"));
                                    cmda.Parameters.AddWithValue("@nr_idveiculo", dt.Rows[x][1].ToString());
                                    cmda.Parameters.AddWithValue("@ds_rua", dt.Rows[x][7].ToString());
                                    cmda.Parameters.AddWithValue("@ds_cidade", dt.Rows[x][8].ToString());
                                    cmda.Parameters.AddWithValue("@ds_uf", dt.Rows[x][9].ToString());
                                    cmda.Parameters.AddWithValue("@cod_transmissao", dt.Rows[x][0].ToString());
                                    cmda.Parameters.AddWithValue("@cod_login", login);

                                    try
                                    {
                                        con.Open();
                                        cmda.ExecuteNonQuery();
                                        con.Close();
                                    }
                                    catch (Exception ex)
                                    {
                                        StreamWriter write = new StreamWriter(@"C:\Erros_SisLog\Log.txt", true);
                                        write.WriteLine("Erro Inserir na Tabela Jornada Trecho: " + ex.ToString() + " - " + DateTime.Now.ToString());
                                        write.Flush();
                                        write.Close();
                                        con.Close();

                                    }
                                }


                            }
                            else
                            {

                                string sqlta = "select nr_idveiculo, hr_inicio_intervalo, cod_jornada, cod_transmissao from tb_jornada where cod_login=" + login + " and dt_jornada='" + DateTime.Parse(dt.Rows[x][5].ToString()).ToString("yyyy-MM-dd") + "' and hr_inicio_intervalo is not null and hr_fim_intervalo is null ";
                                SqlDataAdapter adptta = new SqlDataAdapter(sqlta, con);
                                DataTable dtta = new DataTable();
                                con.Open();
                                adptta.Fill(dtta);
                                con.Close();


                                if (dtta.Rows.Count > 0)
                                {
                                    TimeSpan hr_ini = new TimeSpan();
                                    TimeSpan hr_fim = new TimeSpan();

                                    hr_ini = TimeSpan.Parse(dtta.Rows[0][1].ToString());
                                    hr_fim = TimeSpan.Parse(dt.Rows[x][7].ToString());

                                    if (hr_fim > hr_ini)
                                    {
                                        string sqlf = "update tb_jornada set hr_fim_intervalo=@hr_fim_intervalo, nr_idveiculo=@nr_idveiculo where  dt_jornada=@dt_jornada and cod_login=@cod_login";


                                        SqlCommand cmdf = new SqlCommand(sqlf, con);
                                        cmdf.Parameters.AddWithValue("@nr_idveiculo", dt.Rows[x][1].ToString());
                                        cmdf.Parameters.AddWithValue("@dt_jornada", DateTime.Parse(dt.Rows[x][5].ToString()).ToString("yyyy-MM-dd"));
                                        cmdf.Parameters.AddWithValue("@cod_login", login);
                                        // cmdf.Parameters.AddWithValue("@cod_jornada", dtta.Rows[0][2].ToString());
                                        cmdf.Parameters.AddWithValue("@hr_fim_intervalo", DateTime.Parse(dt.Rows[x][6].ToString()).ToString("HH:mm:ss.000"));

                                        try
                                        {
                                            con.Open();
                                            cmdf.ExecuteNonQuery();
                                            con.Close();
                                        }
                                        catch (Exception ex)
                                        {
                                            StreamWriter write = new StreamWriter(@"C:\Erros_SisLog\Log.txt", true);
                                            write.WriteLine("Erro Inserir na Tabela Jornada Reinicio Viagem: " + ex.ToString() + " - " + DateTime.Now.ToString());
                                            write.Flush();
                                            write.Close();
                                            con.Close();

                                        }
                                    }
                                }

                            }


                        }
                    }



                    else if (dt.Rows[x][4].ToString() == "PARADA REFEICAO")
                    {
                        if (valido || valido2)
                        {
                            string sqla1 = "select nr_idveiculo, dt_jornada, cod_jornada from tb_jornada where  cod_login=" + login + " and dt_jornada='" + DateTime.Parse(dt.Rows[x][5].ToString()).ToString("yyyy-MM-dd") + "' ";
                            SqlDataAdapter adpta = new SqlDataAdapter(sqla1, con);
                            DataTable dta = new DataTable();
                            con.Open();
                            adpta.Fill(dta);
                            con.Close();

                            if (dta.Rows.Count > 0)
                            {
                                string sqla = "update tb_jornada set hr_inicio_intervalo=@hr_inicio_intervalo,ds_rua=@ds_rua, ds_cidade=@ds_cidade, ds_uf=@ds_uf, cod_transmissao=@cod_transmissao, nr_id_giga=@nr_id_giga where dt_jornada=@dt_jornada and cod_login=@cod_login ";
                                SqlCommand cmda = new SqlCommand(sqla, con);
                                cmda.Parameters.AddWithValue("@hr_inicio_intervalo", DateTime.Parse(dt.Rows[x][6].ToString()).ToString("HH:mm:ss.000"));
                                cmda.Parameters.AddWithValue("@dt_jornada", DateTime.Parse(dt.Rows[x][5].ToString()).ToString("yyyy-MM-dd"));

                                cmda.Parameters.AddWithValue("@ds_rua", dt.Rows[x][7].ToString());
                                cmda.Parameters.AddWithValue("@ds_cidade", dt.Rows[x][8].ToString());
                                cmda.Parameters.AddWithValue("@ds_uf", dt.Rows[x][9].ToString());
                                cmda.Parameters.AddWithValue("@cod_transmissao", dt.Rows[x][0].ToString());
                                cmda.Parameters.AddWithValue("@cod_login", login);
                                cmda.Parameters.AddWithValue("@nr_id_giga", dt.Rows[x][1].ToString());

                                try
                                {
                                    con.Open();
                                    cmda.ExecuteNonQuery();
                                    con.Close();
                                }
                                catch (Exception ex)
                                {
                                    StreamWriter write = new StreamWriter(@"C:\Erros_SisLog\Log.txt", true);
                                    write.WriteLine("Erro Inserir na Tabela Jornada Trecho: " + ex.ToString() + " - " + DateTime.Now.ToString());
                                    write.Flush();
                                    write.Close();
                                    con.Close();

                                }
                            }


                        }


                    }

                    else if (dt.Rows[x][4].ToString() == "RETORNO REFEICAO")
                    {
                        if (valido || valido2)
                        {
                            string sqlta = "select nr_idveiculo, hr_inicio_intervalo, cod_jornada from tb_jornada  where  cod_login=" + login + " and dt_jornada='" + DateTime.Parse(dt.Rows[x][5].ToString()).ToString("yyyy-MM-dd") + "' and hr_inicio_intervalo is not null ";
                            SqlDataAdapter adptta = new SqlDataAdapter(sqlta, con);
                            DataTable dtta = new DataTable();
                            con.Open();
                            adptta.Fill(dtta);
                            con.Close();



                            string sqlf = "update tb_jornada set hr_fim_intervalo=@hr_fim_intervalo, nr_id_giga=@nr_id_giga where dt_jornada=@dt_jornada and cod_login=@cod_login";



                            if (dtta.Rows.Count > 0)
                            {
                                SqlCommand cmdf = new SqlCommand(sqlf, con);
                                //cmdf.Parameters.AddWithValue("@nr_idveiculo", dt.Rows[x][1].ToString());
                                cmdf.Parameters.AddWithValue("@dt_jornada", DateTime.Parse(dt.Rows[x][5].ToString()).ToString("yyyy-MM-dd"));
                                cmdf.Parameters.AddWithValue("@hr_fim_intervalo", DateTime.Parse(dt.Rows[x][6].ToString()).ToString("HH:mm:ss.000"));
                                cmdf.Parameters.AddWithValue("@cod_login", login);
                                cmdf.Parameters.AddWithValue("@nr_id_giga", dt.Rows[x][1].ToString());

                                try
                                {
                                    con.Open();
                                    cmdf.ExecuteNonQuery();
                                    con.Close();
                                }
                                catch (Exception ex)
                                {
                                    StreamWriter write = new StreamWriter(@"C:\Erros_SisLog\Log.txt", true);
                                    write.WriteLine("Erro Inserir na Tabela Jornada Reinicio Viagem: " + ex.ToString() + " - " + DateTime.Now.ToString());
                                    write.Flush();
                                    write.Close();
                                    con.Close();

                                }
                            }

                        }
                    }



                    if (dt.Rows[x][4].ToString() == "INICIO DE VIAGEM" || dt.Rows[x][4].ToString() == "REINICIO DE VIAGEM")
                    {
                        if (valido || valido2)
                        {

                            string sqlta = "select nr_idveiculo, hr_inicio_intervalo, cod_jornada, cod_transmissao from tb_jornada where cod_login=" + login + " and  dt_jornada='" + DateTime.Parse(dt.Rows[x][5].ToString()).ToString("yyyy-MM-dd") + "'  and hr_inicio_intervalo is not null  and hr_fim_intervalo is null ";
                            SqlDataAdapter adptta = new SqlDataAdapter(sqlta, con);
                            DataTable dtta = new DataTable();
                            con.Open();
                            adptta.Fill(dtta);
                            con.Close();




                            if (dtta.Rows.Count > 0)
                            {
                                TimeSpan hr_ini = new TimeSpan();
                                TimeSpan hr_fim = new TimeSpan();




                                hr_ini = TimeSpan.Parse(dtta.Rows[0][1].ToString());
                                hr_fim = TimeSpan.Parse(dt.Rows[x][6].ToString());

                                if (hr_fim > hr_ini)
                                {
                                    string sqlf = "update tb_jornada set hr_fim_intervalo=@hr_fim_intervalo,nr_idveiculo=@nr_idveiculo where dt_jornada=@dt_jornada and cod_login=@cod_login";


                                    SqlCommand cmdf = new SqlCommand(sqlf, con);
                                    cmdf.Parameters.AddWithValue("@nr_idveiculo", dt.Rows[x][1].ToString());
                                    cmdf.Parameters.AddWithValue("@dt_jornada", DateTime.Parse(dt.Rows[x][5].ToString()).ToString("yyyy-MM-dd"));
                                    cmdf.Parameters.AddWithValue("@cod_login", login);
                                    // cmdf.Parameters.AddWithValue("@cod_jornada", dtta.Rows[0][2].ToString());
                                    cmdf.Parameters.AddWithValue("@hr_fim_intervalo", DateTime.Parse(dt.Rows[x][6].ToString()).ToString("HH:mm:ss.000"));

                                    try
                                    {
                                        con.Open();
                                        cmdf.ExecuteNonQuery();
                                        con.Close();
                                    }
                                    catch (Exception ex)
                                    {
                                        StreamWriter write = new StreamWriter(@"C:\Erros_SisLog\Log.txt", true);
                                        write.WriteLine("Erro Inserir na Tabela Jornada Reinicio Viagem: " + ex.ToString() + " - " + DateTime.Now.ToString());
                                        write.Flush();
                                        write.Close();
                                        con.Close();

                                    }
                                }

                            }

                        }
                    }






                    else if (dt.Rows[x][4].ToString() == "FIM DE JORNADA")
                    {
                        if (valido || valido2)
                        {


                            //string sqlta1 = "select nr_idveiculo, hr_inicio_jornada, cod_jornada from tb_jornada where nr_idveiculo=" + dt.Rows[x][1].ToString() + " and dt_jornada='" + DateTime.Parse(dt.Rows[x][6].ToString()).ToString("dd/MM/yyyy") + "' and hr_inicio_jornada is not null and hr_fim_jornada is null";
                            string sqlta1 = "select nr_idveiculo, hr_inicio_jornada, cod_jornada from tb_jornada where dt_jornada='" + DateTime.Parse(dt.Rows[x][5].ToString()).ToString("yyyy-MM-dd") + "' and cod_login=" + login + ""; //and hr_fim_jornada is null"; //and hr_fim_jornada is null";
                            SqlDataAdapter adptta1 = new SqlDataAdapter(sqlta1, con);
                            DataTable dtta1 = new DataTable();
                            con.Open();
                            adptta1.Fill(dtta1);
                            con.Close();

                            if (dtta1.Rows.Count > 0)
                            {

                                if (dt.Rows[x][1].ToString() == "725405" || dt.Rows[x][1].ToString() == "725407")
                                {
                                    string sqlf1 = "update tb_jornada set hr_fim_jornada=@hr_fim_jornada, nr_id_giga=@nr_id_giga where dt_jornada=@dt_jornada and cod_login=@cod_login";


                                    SqlCommand cmdf1 = new SqlCommand(sqlf1, con);
                                    cmdf1.Parameters.AddWithValue("@nr_id_giga", dt.Rows[x][1].ToString());
                                    cmdf1.Parameters.AddWithValue("@dt_jornada", DateTime.Parse(dt.Rows[x][5].ToString()).ToString("yyyy-MM-dd"));
                                    cmdf1.Parameters.AddWithValue("@hr_fim_jornada", DateTime.Parse(dt.Rows[x][6].ToString()).ToString("HH:mm:ss.000"));
                                    cmdf1.Parameters.AddWithValue("@cod_login", login);

                                    try
                                    {
                                        con.Open();
                                        cmdf1.ExecuteNonQuery();
                                        con.Close();
                                    }
                                    catch (Exception ex)
                                    {
                                        StreamWriter write = new StreamWriter(@"C:\Erros_SisLog\Log.txt", true);
                                        write.WriteLine("Erro Inserir na Tabela Jornada Fim Jornada: " + ex.ToString() + " - " + DateTime.Now.ToString());
                                        write.Flush();
                                        write.Close();
                                        con.Close();

                                    }
                                }
                                else
                                {
                                    string sqlf1 = "update tb_jornada set hr_fim_jornada=@hr_fim_jornada, nr_idveiculo=@nr_idveiculo where dt_jornada=@dt_jornada  and cod_login=@cod_login";


                                    SqlCommand cmdf1 = new SqlCommand(sqlf1, con);
                                    cmdf1.Parameters.AddWithValue("@nr_idveiculo", dt.Rows[x][1].ToString());
                                    cmdf1.Parameters.AddWithValue("@dt_jornada", DateTime.Parse(dt.Rows[x][5].ToString()).ToString("yyyy-MM-dd"));

                                    cmdf1.Parameters.AddWithValue("@hr_fim_jornada", DateTime.Parse(dt.Rows[x][6].ToString()).ToString("HH:mm:ss.000"));
                                    cmdf1.Parameters.AddWithValue("@cod_login", login);

                                    try
                                    {
                                        con.Open();
                                        cmdf1.ExecuteNonQuery();
                                        con.Close();
                                    }
                                    catch (Exception ex)
                                    {
                                        StreamWriter write = new StreamWriter(@"C:\Erros_SisLog\Log.txt", true);
                                        write.WriteLine("Erro Inserir na Tabela Jornada Fim Jornada: " + ex.ToString() + " - " + DateTime.Now.ToString());
                                        write.Flush();
                                        write.Close();
                                        con.Close();

                                    }

                                }




                            }

                        }
                    }

                    if (dt.Rows[x][4].ToString() == "PARADA PERNOITE")
                    {

                        if (valido || valido2)
                        {

                            //string sqlta1 = "select nr_idveiculo, hr_inicio_jornada, cod_jornada from tb_jornada where nr_idveiculo=" + dt.Rows[x][1].ToString() + " and dt_jornada='" + DateTime.Parse(dt.Rows[x][6].ToString()).ToString("dd/MM/yyyy") + "' and hr_inicio_jornada is not null and hr_fim_jornada is null";
                            string sqlta1 = "select nr_idveiculo, hr_inicio_jornada, cod_jornada from tb_jornada where dt_jornada='" + DateTime.Parse(dt.Rows[x][5].ToString()).ToString("yyyy-MM-dd") + "' and cod_login=" + login + ""; //and hr_fim_jornada is null"; //and hr_fim_jornada is null";
                            SqlDataAdapter adptta1 = new SqlDataAdapter(sqlta1, con);
                            DataTable dtta1 = new DataTable();
                            con.Open();
                            adptta1.Fill(dtta1);
                            con.Close();

                            if (dtta1.Rows.Count > 0)
                            {

                                string sqlf1 = "update tb_jornada set hr_fim_jornada=@hr_fim_jornada, nr_idveiculo=@nr_idveiculo where dt_jornada=@dt_jornada  and cod_login=@cod_login";


                                SqlCommand cmdf1 = new SqlCommand(sqlf1, con);
                                cmdf1.Parameters.AddWithValue("@nr_idveiculo", dt.Rows[x][1].ToString());
                                cmdf1.Parameters.AddWithValue("@dt_jornada", DateTime.Parse(dt.Rows[x][5].ToString()).ToString("yyyy-MM-dd"));

                                cmdf1.Parameters.AddWithValue("@hr_fim_jornada", DateTime.Parse(dt.Rows[x][6].ToString()).ToString("HH:mm:ss.000"));
                                cmdf1.Parameters.AddWithValue("@cod_login", login);

                                try
                                {
                                    con.Open();
                                    cmdf1.ExecuteNonQuery();
                                    con.Close();
                                }
                                catch (Exception ex)
                                {
                                    StreamWriter write = new StreamWriter(@"C:\Erros_SisLog\Log.txt", true);
                                    write.WriteLine("Erro Inserir na Tabela Jornada Fim Jornada: " + ex.ToString() + " - " + DateTime.Now.ToString());
                                    write.Flush();
                                    write.Close();
                                    con.Close();

                                }


                            }


                        }

                    }

                }





            }
        }

        protected void ddlNumero_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlNumero.SelectedValue == "1")
            {
                txtTipoMarcacao.Text = "PARADA POSTO FISCAL";
            }
            else if (ddlNumero.SelectedValue == "2")
            {
                txtTipoMarcacao.Text = "AGUARDANDO CARREGAMENTO";
            }
            else if (ddlNumero.SelectedValue == "3")
            {
                txtTipoMarcacao.Text = "AGUARDANDO DESCARREGAMENTO";
            }
            else if (ddlNumero.SelectedValue == "4")
            {
                txtTipoMarcacao.Text = "AGUARDANDO DOC/NOTA FISCAL";
            }
            else if (ddlNumero.SelectedValue == "6")
            {
                txtTipoMarcacao.Text = "CARREGADO AG. LIBERAÇÃO/RODIZIO";
            }
            else if (ddlNumero.SelectedValue == "7")
            {
                txtTipoMarcacao.Text = "CARRO ABASTECENDO";
            }
            else if (ddlNumero.SelectedValue == "9")
            {
                txtTipoMarcacao.Text = "CARRO BORRACHARIA";
            }
            else if (ddlNumero.SelectedValue == "11")
            {
                txtTipoMarcacao.Text = "CARRO QUEBRADO";
            }
            else if (ddlNumero.SelectedValue == "14")
            {
                txtTipoMarcacao.Text = "MOTORISTA EM REFEIÇÃO";
            }
            else if (ddlNumero.SelectedValue == "13")
            {
                txtTipoMarcacao.Text = "FISCALIZAÇÃO RODOVIARIA";
            }
            else if (ddlNumero.SelectedValue == "15")
            {
                txtTipoMarcacao.Text = "PARADA 30 MINUTOS";
            }
            else if (ddlNumero.SelectedValue == "16")
            {
                txtTipoMarcacao.Text = "ACIDENTE COM O CARRO";
            }
            else if (ddlNumero.SelectedValue == "17")
            {
                txtTipoMarcacao.Text = "VERIFICANDO CARRO";
            }
            else if (ddlNumero.SelectedValue == "22")
            {
                txtTipoMarcacao.Text = "MOT. A DISPOSIÇÃO";
            }
            else if (ddlNumero.SelectedValue == "23")
            {
                txtTipoMarcacao.Text = "PARADA BANHEIRO";
            }
            else if (ddlNumero.SelectedValue == "0")
            {
                txtTipoMarcacao.Text = "";
            }
        }
        public void CarregaCusto()
        {
            //txtAlmoco.Text = "0,00";
            //txtCafe.Text = "0,00";
            //txtJantar.Text = "0,00";
            //txtPernoite.Text = "0,00";
            //txtPremio.Text = "0,00";
            string sql = "select  cod_custo, ISNULL(vl_cafe, 0) AS cafe,ISNULL(vl_almoco, 0) AS almoco,ISNULL(vl_jantar, 0) AS jantar,ISNULL(vl_pernoite, 0) AS pernoite,ISNULL(vl_premio, 0) AS premio,ISNULL(vl_engate_des, 0) AS engatedes,";
            sql += " SUM(ISNULL(vl_cafe, 0) + ISNULL(vl_almoco, 0) + ISNULL(vl_jantar, 0) + ISNULL(vl_pernoite, 0) + ISNULL(vl_premio, 0) + ISNULL(vl_engate_des, 0)) AS total,ds_rel1, ds_rel2, ds_rel3, ds_rel4 from tb_custo_motorista ";
            sql += " where cod_cracha=" + txtMotorista.Text + " and dt_custo='" + DateTime.Parse(txtData.Text).ToString("yyyy-MM-dd") + "' group by cod_custo, cod_cracha, dt_custo,vl_cafe,vl_almoco,vl_jantar,vl_pernoite,vl_premio,vl_engate_des,ds_rel1,ds_rel2,ds_rel3,ds_rel4";
            SqlDataAdapter adtp1 = new SqlDataAdapter(sql, con);
            DataTable dt = new DataTable();
            con.Open();
            adtp1.Fill(dt);
            con.Close();
            grdCusto.DataSource = dt;
            grdCusto.DataBind();

        }

        protected void btnValor_Click(object sender, EventArgs e)
        {

            if (txtMotorista.Text != "" && txtData.Text != "")
            {

                if (txtconformmessageValue2.Value == "Yes")
                {
                    string sql = "insert tb_custo_motorista (cod_cracha, dt_custo, vl_cafe, vl_almoco, vl_jantar, vl_pernoite, ds_rel1,ds_rel2,ds_rel3,ds_rel4,vl_engate_des)";
                    sql += " values(@cod_cracha, @dt_custo, @vl_cafe, @vl_almoco, @vl_jantar, @vl_pernoite, @ds_rel1,@ds_rel2,@ds_rel3,@ds_rel4, @vl_engate_des)";
                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@cod_cracha", txtMotorista.Text);
                    cmd.Parameters.AddWithValue("@dt_custo", DateTime.Parse(txtData.Text).ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@vl_cafe", txtCafe.Text.Replace(",", "."));
                    cmd.Parameters.AddWithValue("@vl_almoco", txtAlmoco.Text.Replace(",", "."));
                    cmd.Parameters.AddWithValue("@vl_jantar", txtJantar.Text.Replace(",", "."));
                    cmd.Parameters.AddWithValue("@vl_pernoite", txtPernoite.Text.Replace(",", "."));
                    //cmd.Parameters.AddWithValue("@vl_premio", txtPremio.Text.Replace(",", "."));
                    cmd.Parameters.AddWithValue("@vl_engate_des", txtEngDesen.Text.Replace(",", "."));
                    cmd.Parameters.AddWithValue("@ds_rel1", txtRel1.Text);
                    cmd.Parameters.AddWithValue("@ds_rel2", txtRel2.Text);
                    cmd.Parameters.AddWithValue("@ds_rel3", txtRel3.Text);
                    cmd.Parameters.AddWithValue("@ds_rel4", txtRel4.Text);

                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                        string message = "Informações cadastradas com sucesso!";
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append("<script type = 'text/javascript'>");
                        sb.Append("window.onload=function(){");
                        sb.Append("alert('");
                        sb.Append(message);
                        sb.Append("')};");
                        sb.Append("</script>");
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
                        Limpar2();
                        CarregaCusto();


                    }
                    catch (Exception ex)
                    {
                        var message = new JavaScriptSerializer().Serialize(ex.Message.ToString());
                        string retorno = "Erro! Contate o administrador. Detalhes do erro: " + message;
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append("<script type = 'text/javascript'>");
                        sb.Append("window.onload=function(){");
                        sb.Append("alert('");
                        sb.Append(retorno);
                        sb.Append("')};");
                        sb.Append("</script>");
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());

                    }
                }
            }
            else
            {

                string retorno = "É necessário escolher um Motorista ";
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<script type = 'text/javascript'>");
                sb.Append("window.onload=function(){");
                sb.Append("alert('");
                sb.Append(retorno);
                sb.Append("')};");
                sb.Append("</script>");
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            if (txtMotorista.Text != string.Empty && txtData.Text != string.Empty)
            {

                CarregaGrid2();


                CarregaTodas();

                CarregaCusto();
            }
            else
            {
                string message = "Necessário informar Cracha e Data";
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<script type = 'text/javascript'>");
                sb.Append("window.onload=function(){");
                sb.Append("alert('");
                sb.Append(message);
                sb.Append("')};");
                sb.Append("</script>");
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
            }

        }

       
    }
}