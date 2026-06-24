using DAL;
using Domain;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NewCapit
{
    public partial class Login : System.Web.UI.Page
    {
        SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString());
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["PermissaoUsuario"] = string.Empty;
        }
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            // 1. Validação básica de campos vazios
            if (string.IsNullOrWhiteSpace(txtUsuario.Text) || string.IsNullOrWhiteSpace(txtSenha.Text))
            {
                this.lblError.Text = "Por favor, entre com Usuário e Senha!";
                return;
            }

            this.lblError.Text = "";
            var usuario = txtUsuario.Text.Trim();
            var senha = txtSenha.Text.Trim();

            // 2. Criação do objeto e chamada da camada de dados
            var obj = new Users
            {
                nm_usuario = usuario,
                ds_senha = senha
            };

            var user = UsersDAL.CheckLogin(obj);

            if (user != null)
            {
                // Define o ID do usuário na sessão (necessário em ambos os casos)
                Session["CodUsuario"] = user.cod_usuario.ToString();
                Session["UsuarioLogado"] = user.nm_nome;

                // 3. Verifica se a senha é a temporária para forçar a troca
                if (senha == "mudar123")
                {
                    // Redireciona imediatamente sem carregar permissões ou outras sessões
                    Session["CodUsuario"] = user.cod_usuario.ToString();
                    Session["UsuarioLogado"] = user.nm_nome;
                    Session["SenhaAtual"] = "mudar123";
                    Response.Redirect("/dist/pages/TrocaSenha.aspx");
                }
                else
                {
                    // Fluxo normal: Carrega todas as informações de sessão
                    Session["EmpresaTrabalho"] = user.emp_usuario;
                    Session["FuncaoUsuario"] = user.fun_usuario;
                    Session["PermissaoUsuario"] = user.fl_permissao;
                    Session["FotoUsuario"] = user.foto_usuario;
                    Session["UsuarioSistema"] = user.nm_usuario;

                    // Registro de login
                    int idLog = UsersDAL.RegistrarLogin(user.cod_usuario);
                    Session["IdSessaoLog"] = idLog;

                    // Carregamento das permissões (Telas)
                    List<string> idsTelas = new List<string>();
                    try
                    {
                        conn.Open();
                        string sql = "SELECT DISTINCT IdTela FROM Usuario_permissao WHERE IdUsuario = @IdUsuario";
                        using (SqlCommand cmd = new SqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@IdUsuario", user.cod_usuario);
                            using (SqlDataReader rdr = cmd.ExecuteReader())
                            {
                                while (rdr.Read())
                                {
                                    idsTelas.Add(rdr["IdTela"].ToString());
                                }
                            }
                        }
                    }
                    finally
                    {
                        conn.Close();
                    }

                    Session["TelasPermitidas"] = string.Join(",", idsTelas);

                    // Redireciona para o sistema
                    Response.Redirect("/dist/pages/Home.aspx");
                }
            }
            else
            {
                // Caso o usuário ou senha não confiram
                lblError.Text = "Usuário ou Senha Incorreto(s)!";
            }
        }
    }
}