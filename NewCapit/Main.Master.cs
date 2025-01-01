using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NewCapit
{
    public partial class Main : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) {
                if (Session["UsuarioLogado"] != null) {
                    string nomeUsuario = Session["UsuarioLogado"].ToString();
                    lblUsuario.Text = nomeUsuario;
                }
                else
                {
                    lblUsuario.Text = "<Usuário>";
                }

                if (Session["FuncaoUsuario"] != null)
                {
                    string nomeFuncao = Session["FuncaoUsuario"].ToString();
                    lblFuncao.Text = nomeFuncao;
                }
                else
                {
                    lblFuncao.Text = "<Função>";
                }

                if (Session["EmpresaTrabalho"] != null)
                {
                    string nomeEmpresa = Session["EmpresaTrabalho"].ToString();
                    lblEmpresa.Text = nomeEmpresa;
                }
                else
                {
                    lblEmpresa.Text = "<Empresa>";
                }
            }
        }
    }
}