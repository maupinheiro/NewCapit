using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Web.Services;
using DocumentFormat.OpenXml.Spreadsheet;


namespace NewCapit.dist.pages
{
    public partial class GerarTabelaDeAvaliacaoMotoristas : System.Web.UI.Page
    {
        SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString());
        string nomeUsuario = string.Empty;        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
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
                CarregarNucleo();
                CarregarMotoristas();
            }
            
        }  

        private void CarregarNucleo()
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                string query = "SELECT DISTINCT nucleo FROM tbmotoristas ORDER BY nucleo";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                ddlStatus.DataSource = dr;
                ddlStatus.DataTextField = "nucleo";
                ddlStatus.DataValueField = "nucleo";
                ddlStatus.DataBind();
            }
        }
        private void CarregarMotoristas(string[] statusSelecionados = null)
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                string query = "SELECT id, caminhofoto, codmot, nommot, cargo, tipomot, cadmot, frota, nucleo FROM tbmotoristas";

                if (statusSelecionados != null && statusSelecionados.Length > 0)
                {
                    string filtros = string.Join(",", statusSelecionados.Select((s, i) => "@status" + i));
                    query += $" WHERE nucleo IN ({filtros}) AND status='ATIVO'";
                    txtSelecionados.Text = string.Join("_", statusSelecionados);
                }

                SqlCommand cmd = new SqlCommand(query, conn);

                if (statusSelecionados != null)
                {
                    for (int i = 0; i < statusSelecionados.Length; i++)
                        cmd.Parameters.AddWithValue("@status" + i, statusSelecionados[i]);
                }

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvPedidos.DataSource = dt;
                gvPedidos.DataBind();
                
                
            }
        }
        protected void btnPesquisar_Click(object sender, EventArgs e)
        {

            // ddlStatus é HTML, então o valor vem assim:
            string valores = Request.Form["ddlStatus"];

            if (!string.IsNullOrEmpty(valores))
            {
                // valores vêm assim: "Pendente_Aprovado"
                txtSelecionados.Text = valores;
            }


            string[] selecionados = ddlStatus.Items.Cast<System.Web.UI.WebControls.ListItem>()
                .Where(x => x.Selected)
                .Select(x => x.Value)
                .ToArray();            
            CarregarMotoristas(selecionados);
        }


    }



}




