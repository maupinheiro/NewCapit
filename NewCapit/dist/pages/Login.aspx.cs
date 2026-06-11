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
                    string permissaoUsuario = user.fl_permissao;
                    //O cod do funcionario é passado para a página master para carregar a foto na pagina//
                    string codFuncionario = user.cod_usuario.ToString();
                    string fotoFuncionario = user.foto_usuario;
                    string usuarioSistema = user.nm_usuario;
                    Session["UsuarioLogado"] = nomeUsuario;
                    Session["EmpresaTrabalho"] = nomeEmpresa;
                    Session["FuncaoUsuario"] = funcaoUsuario;
                    Session["CodUsuario"] = codFuncionario;
                    Session["PermissaoUsuario"] = permissaoUsuario;
                    Session["FotoUsuario"] = fotoFuncionario;
                    Session["UsuarioSistema"] = usuarioSistema;
                    int idLog = UsersDAL.RegistrarLogin(user.cod_usuario);
                    Session["IdSessaoLog"] = idLog; // Importante para o logout saber quem atualizar
                                                    // Dentro do seu método de autenticação/login:
                    List<string> idsTelas = new List<string>();
                    conn.Open();
                    string sql = "SELECT DISTINCT IdTela FROM Usuario_permissao WHERE IdUsuario = @IdUsuario";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@IdUsuario", codFuncionario);
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                idsTelas.Add(rdr["IdTela"].ToString());
                            }
                        }
                    }
                    conn.Close();
                    // Transforma a lista ["3", "6", "11"] na string "3,6,11" para guardar na Session
                    Session["TelasPermitidas"] = string.Join(",", idsTelas);
                    Response.Redirect("/dist/pages/Home.aspx");

                }
                else 
                {
                    lblError.Text = "Usuário ou Senha Incorreto(s)!";
                }

            }

        }
    }
}