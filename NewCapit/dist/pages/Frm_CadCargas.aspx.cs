using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NewCapit.dist.pages
{
    public partial class FrmCadCargas : System.Web.UI.Page
    {
       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UsuarioLogado"] != null)
                {
                    string nomeUsuario = Session["UsuarioLogado"].ToString();
                    var lblUsuario = nomeUsuario;
                    txtUsuCadastro.Text = nomeUsuario;
                }
                else
                {
                    var lblUsuario = "<Usuário>";
                }
                DateTime dataHoraAtual = DateTime.Now;
                lblDtCadastro.Text = dataHoraAtual.ToString("dd/MM/yyyy HH:mm");
                lblDtCadCarga.Text = dataHoraAtual.ToString("dd/MM/yyyy");
                
            }
        }
    }
}