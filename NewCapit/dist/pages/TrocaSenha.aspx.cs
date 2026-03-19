using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace NewCapit.dist.pages
{
    public partial class TrocaSenha : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conexao"].ToString());
        protected void Page_Load(object sender, EventArgs e)
        {
            string login = Session["UsuarioLogado"].ToString();

            if (!IsPostBack)
            {
                if (login == null)
                {
                    Response.Redirect("/dist/pages/TrocaSenha.aspx");
                }
            }
            
        }

        protected void btnSalvarSenha_Click(object sender, EventArgs e)
        {
            string codusuario = Session["CodUsuario"].ToString();


            if (txtSenhaAtual.Text == "" || txtNovaSenha.Text == "" || txtConfirmarSenha.Text == "")
            {

                string retorno = "Os o campos senha, nova senha e confirmar senha devem ser preenchidos";
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<script type = 'text/javascript'>");
                sb.Append("window.onload=function(){");
                sb.Append("alert('");
                sb.Append(retorno);
                sb.Append("')};");
                sb.Append("</script>");
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
            }

            else
            {


                string sql = "select ds_senha from tb_usuario where cod_usuario='" + codusuario + "'";

                con.Open();
                SqlDataAdapter dat = new SqlDataAdapter();

                dat.SelectCommand = new SqlCommand(sql, con);

                DataTable dt = new DataTable();
                dat.Fill(dt);
                con.Close();

                if (txtSenhaAtual.Text != dt.Rows[0][0].ToString())
                {

                    string retorno = "A senha digitada não confere, por favor digite novamente!";
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append("<script type = 'text/javascript'>");
                    sb.Append("window.onload=function(){");
                    sb.Append("alert('");
                    sb.Append(retorno);
                    sb.Append("')};");
                    sb.Append("</script>");
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
                }

                else
                {
                    if (txtNovaSenha.Text != txtConfirmarSenha.Text)
                    {

                        string retorno = "A nova senha não confere com a senha de confirmação, por favor digite novamente!";
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append("<script type = 'text/javascript'>");
                        sb.Append("window.onload=function(){");
                        sb.Append("alert('");
                        sb.Append(retorno);
                        sb.Append("')};");
                        sb.Append("</script>");
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
                    }

                    else
                    {
                        if (txtNovaSenha.Text == txtConfirmarSenha.Text && txtConfirmarSenha.Text != txtSenhaAtual.Text)
                        {
                            string sql1 = " update tb_usuario  set ds_senha = @ds_senha, dt_troca_senha=@dt_troca_senha  where cod_usuario=@cod_usuario  ";
                            SqlCommand comand = new SqlCommand(sql1, con);
                            comand.Parameters.AddWithValue("@ds_senha", txtConfirmarSenha.Text);
                            comand.Parameters.AddWithValue("@dt_troca_senha", DateTime.Now.ToString("yyyy-MM-dd"));
                            comand.Parameters.AddWithValue("@cod_usuario", codusuario);
                            con.Open();
                            comand.ExecuteNonQuery();
                            con.Close();

                            string retorno = "Senha alterada com sucesso!";
                            System.Text.StringBuilder sb = new System.Text.StringBuilder();

                            sb.Append("<script type = 'text/javascript'>");
                            sb.Append("window.onload=function(){");
                            sb.Append("alert('");
                            sb.Append(retorno);
                            sb.Append("');"); // Fecha o comando do alert
                            sb.Append("window.location.href = '/dist/pages/Home.aspx';"); // Redireciona após o OK
                            sb.Append("};");
                            sb.Append("</script>");

                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
                        }
                        else
                        {
                            string retorno = "A nova senha deve ser diferente da senha atual, por favor digite novamente!";
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

            }
        }
    }
}