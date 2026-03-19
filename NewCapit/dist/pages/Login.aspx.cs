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
            if (string.IsNullOrWhiteSpace(txtUsuario.Text) || string.IsNullOrWhiteSpace(txtSenha.Text))
            {
                this.lblError.Text = "Por favor, entre com Usuário e Senha!";
            }
            else
            {
                this.lblError.Text = "";
                var usuario = txtUsuario.Text.Trim();
                var senha = txtSenha.Text.Trim();

                var obj = new Users { nm_usuario = usuario, ds_senha = senha };
                var user = UsersDAL.CheckLogin(obj);

                if (user != null)
                {
                    // --- Grava as Sessions (Mantendo seu código original) ---
                    Session["UsuarioLogado"] = user.nm_nome;
                    Session["EmpresaTrabalho"] = user.emp_usuario;
                    Session["FuncaoUsuario"] = user.fun_usuario;
                    Session["CodUsuario"] = user.cod_usuario.ToString();
                    Session["PermissaoUsuario"] = user.fl_permissao;
                    Session["FotoUsuario"] = user.foto_usuario;

                    int idLog = UsersDAL.RegistrarLogin(user.cod_usuario);
                    Session["IdSessaoLog"] = idLog;

                    // --- NOVA LÓGICA DE VERIFICAÇÃO DE SENHA ---
                    bool precisaTrocar = false;

                    if (user.dt_troca_senha == null)
                    {
                        // Se a data for nula
                        precisaTrocar = true;
                    }
                    else
                    {
                        // Calcula a diferença em dias entre hoje e a última troca
                        TimeSpan diferenca = DateTime.Now - user.dt_troca_senha.Value;
                        if (diferenca.TotalDays > 60)
                        {
                            precisaTrocar = true;
                        }
                    }

                    if (precisaTrocar)
                    {
                        // Redireciona para troca de senha (ajuste o caminho se necessário)
                        Response.Redirect("/dist/pages/TrocaSenha.aspx");
                    }
                    else
                    {
                        // Fluxo normal
                        Response.Redirect("/dist/pages/Home.aspx");
                    }
                }
                else
                {
                    lblError.Text = "Usuário ou Senha Incorreto(s)!";
                }
            }
        }
    }
}