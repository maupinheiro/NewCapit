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
        string nomeMes;
        
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
                //   CarregarMotoristas();
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
                string query = @"
                                SELECT 
                                    m.id, m.caminhofoto, m.codmot, m.nommot, m.cargo, m.tipomot, 
                                    m.cadmot, m.frota, m.nucleo 
                                FROM tbmotoristas AS m
                                INNER JOIN tbavaliacaomotorista AS a ON m.codmot = a.cracha WHERE a.mes = @mes";

                if (statusSelecionados != null && statusSelecionados.Length > 0)
                {
                    string filtros = string.Join(",", statusSelecionados.Select((s, i) => "@status" + i));

                    query += $" and m.nucleo IN ({filtros})"
                          + " AND m.status = 'ATIVO'"
                          + " AND ISNUMERIC(m.codmot) = 1";
                        
                }

                SqlCommand cmd = new SqlCommand(query, conn);

                // Adiciona parâmetros dos núcleos selecionados
                if (statusSelecionados != null)
                {
                    for (int i = 0; i < statusSelecionados.Length; i++)
                        cmd.Parameters.AddWithValue("@status" + i, statusSelecionados[i]);
                }

                // Monta o valor no formato correto "MM/YYYY"
                string mesAno = ddlMes.SelectedValue.PadLeft(2, '0') + "/" + ddlAno.SelectedValue;

                // Adiciona parâmetro do mês/ano
                cmd.Parameters.AddWithValue("@mes", mesAno);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvMotoristas.DataSource = dt;
                gvMotoristas.DataBind();



            }
        }
        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ddlMes.SelectedValue))
            {
                // Acione o toast quando a página for carregada
                string script = "<script>showToast('Período, está vazia!');</script>";
                ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                
                return;
            }
            
            if (string.IsNullOrWhiteSpace(ddlAno.SelectedValue))
            {
                // Acione o toast quando a página for carregada
                string script = "<script>showToast('Data final do período, está vazia!');</script>";
                ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                
                return;
            }


            // ddlStatus é HTML, então o valor vem assim:
            string valores = Request.Form["ddlStatus"];

            if (valores == "")
            {
                string script = "<script>showToast('Selecione pelo menos uma filial para gerar o arquivo!');</script>";
                ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                return;
            }
            else
            {
               
                if (!string.IsNullOrEmpty(valores))
                {
                    
                    txtSelecionados.Text = nomeMes + "_" + "_" + valores;

                }
                string[] selecionados = ddlStatus.Items.Cast<System.Web.UI.WebControls.ListItem>()
                    .Where(x => x.Selected)
                    .Select(x => x.Value)
                    .ToArray();
                CarregarMotoristas(selecionados);

            }
        }

        

        private object SafeDateValue(string input)
        {
            DateTime dt;
            if (DateTime.TryParse(input, out dt))
                return dt.ToString("yyyy-MM-dd");
            else
                return DBNull.Value;
        }

        protected void Editar(object sender, EventArgs e)
        {
            using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
            {
                string id = gvMotoristas.DataKeys[row.RowIndex].Value.ToString();

                string url = "Frm_AvaliaDesempenho.aspx?id=" + id;
                string script = $"window.open('{url}', '_blank', 'width=900,height=780,scrollbars=yes,resizable=yes');";
                ClientScript.RegisterStartupScript(this.GetType(), "openWindow", script, true);

               
            }
        }
    }
   
  


}




