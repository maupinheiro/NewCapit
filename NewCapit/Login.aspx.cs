using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Domain;
using DAL;

namespace NewCapit
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtUsuario.Text.Trim() == "" || txtSenha.Text.Trim() == "")
            {
                this.lblError.Text = "Por favor, entre com Usuário e Senha!";

            }
            else
            {
                this.lblError.Text = "";

                var usuario = txtUsuario.Text.Trim();
                var senha = txtSenha.Text.Trim();
                //string departamento, funcao, empresaUsuario, foto;
                var obj = new Users
                {
                    nm_usuario = usuario,
                    ds_senha = senha                  

                };
                var user = UsersDAL.CheckLogin(obj);
                if (user != null)
                {
                    //Dados do usuário
                    string nomeUsuario = user.nm_nome;
                    string nomeEmpresa = user.emp_usuario;
                    string funcaoUsuario = user.fun_usuario;
                    // falta a foto
                    //
                    Session["UsuarioLogado"] = nomeUsuario;
                    Session["EmpresaTrabalho"] = nomeEmpresa;
                    Session["FuncaoUsuario"] = funcaoUsuario;

                    //Chama a página principal
                    Response.Redirect("Home.aspx");
                }
                else 
                {
                    lblError.Text = "Usuário ou Senha Incorreto(s)!";
                }

            }

        }
    }
}