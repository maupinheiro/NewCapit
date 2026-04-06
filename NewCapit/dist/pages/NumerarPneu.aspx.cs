using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace NewCapit.dist.pages
{
    public partial class NumerarPneu : System.Web.UI.Page
    {
        string conexao = WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
        string idPneu;
        DateTime dataHoraAtual = DateTime.Now;
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
                CarregaDadosDoPneu();
            }
        }
        public void CarregaDadosDoPneu()
        {
            if (Request.QueryString["id"] != null)
            {
                idPneu = Request.QueryString["id"].ToString();
            }
            else
            {
                Mensagem("info", "Nenhum ID de pneu informado.");
                return;
            }


            string codigoPneu = idPneu;
            string strConn = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                string queryPneu = "SELECT * FROM tbpneus WHERE id =  @idPneu";

                using (SqlCommand cmdPneu = new SqlCommand(queryPneu, conn))
                {
                    cmdPneu.Parameters.AddWithValue("@idPneu", codigoPneu);
                    conn.Open();

                    using (SqlDataReader reader = cmdPneu.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (reader["numero"].ToString() != "")
                            {
                                Mensagem("warning", "Pneu, já numerado.");
                                txtNumero.ReadOnly = true;
                                // Converte para string antes de atribuir aos TextBox
                                txtDescricao.Text = reader["descricao"].ToString();
                                txtID.Text = reader["id"].ToString();
                                if (reader["datacompra"] != DBNull.Value)
                                {
                                    txtDtCompra.Text = Convert.ToDateTime(reader["datacompra"]).ToString("dd/MM/yyyy");
                                }
                                if (reader["valor"] != DBNull.Value)
                                {
                                    txtValor.Text = Convert.ToDecimal(reader["valor"])
                                        .ToString("C2", new System.Globalization.CultureInfo("pt-BR"));
                                }
                                txtResp_entrada.Text = reader["resp_entrada"].ToString();
                                txtNumero.Text = reader["numero"].ToString();
                                txtMarca.Text = reader["marca"].ToString();
                                txtModelo.Text = reader["modelo"].ToString();
                                txtMedida.Text = reader["medida"].ToString();
                                txtStatus.Text = reader["status"].ToString();
                                return;

                            }
                            else
                            {
                                // Converte para string antes de atribuir aos TextBox
                                txtDescricao.Text = reader["descricao"].ToString();
                                txtID.Text = reader["id"].ToString();
                                if (reader["datacompra"] != DBNull.Value)
                                {
                                    txtDtCompra.Text = Convert.ToDateTime(reader["datacompra"]).ToString("dd/MM/yyyy");
                                }
                                if (reader["valor"] != DBNull.Value)
                                {
                                    txtValor.Text = Convert.ToDecimal(reader["valor"])
                                        .ToString("C2", new System.Globalization.CultureInfo("pt-BR"));
                                }
                                txtResp_entrada.Text = reader["resp_entrada"].ToString();
                                txtNumero.Text = reader["numero"].ToString();
                                txtMarca.Text = reader["marca"].ToString();
                                txtModelo.Text = reader["modelo"].ToString();
                                txtMedida.Text = reader["medida"].ToString();
                                txtStatus.Text = reader["status"].ToString();
                                txtNumero.Focus();
                            }
                        }
                    }
                }
            }
        }
        protected void btnSalvar_Click(object sender, EventArgs e) 
        {

            if (string.IsNullOrEmpty(txtNumero.Text)) { 
                Mensagem("info", "Número do pneu deve ser digitado.");
                txtNumero.Focus();
                return;            
            }
            
            string numero = txtNumero.Text.Trim();
            using (SqlConnection conn = new SqlConnection(conexao))
            {
                conn.Open();

                // 1. Verifica se já existe
                SqlCommand cmdVerifica = new SqlCommand(
                    "SELECT COUNT(*) FROM tbpneus WHERE numero = @numero", conn);

                cmdVerifica.Parameters.AddWithValue("@numero", numero);

                int existe = (int)cmdVerifica.ExecuteScalar();

                if (existe > 0)
                {
                    // Já existe → bloqueia
                    Mensagem("danger", " - Número " + txtNumero.Text.Trim() + ", já foi lançado em outro pneu. Não pode ser duplicado.");
                    txtNumero.Text = "";
                    txtNumero.Focus();
                    return;
                }

                SqlCommand cmd = new SqlCommand(@"UPDATE tbPneus 
                                 SET Numero=@n 
                                 WHERE id=@id", conn);

                cmd.Parameters.AddWithValue("@n", txtNumero.Text.Trim());
                cmd.Parameters.AddWithValue("@id", txtID.Text.Trim());                
                cmd.ExecuteNonQuery();




                //SqlCommand cmd;                
                //cmd = new SqlCommand("UPDATE tbPneus SET Numero=@n WHERE Id=@id", conn);                
                //cmd.Parameters.AddWithValue("@n", txtNumero.Text);
                //cmd.Parameters.AddWithValue("@id", idPneu);
                //cmd.ExecuteNonQuery();
            }

            Response.Redirect("ControlePneus.aspx");
        }
        protected void Mensagem(string tipo, string texto)
        {
            divMsg.Visible = true;

            divMsg.Attributes["class"] =
                "alert alert-" + tipo + " alert-dismissible fade show mt-3";

            lblMsgGeral.Text = texto;
        }
        

    }
}