using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NewCapit.dist.pages
{
    public partial class Frm_CadPosto : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString());       
        string id;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UsuarioLogado"] != null)
                {
                    string nomeUsuario = Session["UsuarioLogado"].ToString();
                    var lblUsuario = nomeUsuario;

                   // txtUsuCadastro.Text = nomeUsuario;

                }
                else
                {
                    var lblUsuario = "<Usuário>";
                    //txtUsuCadastro.Text = lblUsuario;
                    Response.Redirect("Login.aspx");
                }
                CarregaDadosFornecedor();
            }
            //DateTime dataHoraAtual = DateTime.Now;
            //lblDtCadastro.Text = dataHoraAtual.ToString("dd/MM/yyyy HH:mm");
            
        }
        public void CarregaDadosFornecedor()
        {
            if (HttpContext.Current.Request.QueryString["id"].ToString() != "")
            {
                id = HttpContext.Current.Request.QueryString["id"].ToString();
            }
            string sql = "SELECT codfor, razaosocial, fantasia, cnpj, inscestadual, inscccm, tipoempresa, abertura, situacaoreceita, tipofornecedor, contato, fonefixo, fonecelular, email, site, cep, endereco, numero, complemento, bairro, cidade, estado, pais, combustivel_S10, combustivel_S500, combustivel_etanol, combustivel_gasolina, combustivel_arla, data_cadastro, usuario_cadastro FROM tbfornecedores WHERE id = " + id;

            SqlDataAdapter adpt = new SqlDataAdapter(sql, con);
            DataTable dt = new DataTable();
            con.Open();
            adpt.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                // Preenchendo os TextBoxes com valores do DataTable
                if (dt.Rows[0][0].ToString() != string.Empty)
                {
                    txtCodFor.Text = dt.Rows[0][0].ToString();
                    txtCnpj.Text = dt.Rows[0][3].ToString();
                    txtRazFor.Text = dt.Rows[0][1].ToString();
                    txtNomFor.Text = dt.Rows[0][2].ToString();
                    txtInscEstadual.Text = dt.Rows[0][4].ToString();
                    txtInscCCM.Text = dt.Rows[0][5].ToString();
                    txtTipo.Text = dt.Rows[0][6].ToString();
                    if (dt.Rows[0][7].ToString() != string.Empty)
                    {
                       // DateTime abertura = Convert.ToDateTime(dt.Rows[0][7]);
                        txtAbertura.Text = dt.Rows[0][7].ToString();
                    }
                    txtSituacao.Text = dt.Rows[0][8].ToString();
                    txtTipoFornecedor.Text = dt.Rows[0][9].ToString();
                    txtConFor.Text = dt.Rows[0][10].ToString();
                    txtTc1For.Text = dt.Rows[0][11].ToString();
                    txtTc2For.Text = dt.Rows[0][12].ToString();
                    txtEmail.Text = dt.Rows[0][13].ToString();
                    txtSite.Text = dt.Rows[0][14].ToString();
                    txtCepFor.Text = dt.Rows[0][15].ToString();
                    txtEndFor.Text = dt.Rows[0][16].ToString();
                    txtNumero.Text = dt.Rows[0][17].ToString();
                    txtComplemento.Text = dt.Rows[0][18].ToString();
                    txtBaiFor.Text = dt.Rows[0][19].ToString();
                    txtCidFor.Text = dt.Rows[0][20].ToString();
                    txtEstFor.Text = dt.Rows[0][21].ToString();
                    ddlPaises.Text = dt.Rows[0][22].ToString();
                    txtS10.Text = dt.Rows[0][23].ToString();
                    txtS500.Text = dt.Rows[0][24].ToString();
                    txtEtanol.Text = dt.Rows[0][25].ToString();
                    txtGasolina.Text = dt.Rows[0][26].ToString();
                    txtArla.Text = dt.Rows[0][27].ToString();
                    lblDtCadastro.Text = Convert.ToDateTime(dt.Rows[0][28]).ToString("dd/MM/yyyy HH:mm");
                    txtUsuCadastro.Text = dt.Rows[0][29].ToString();
                }
                

            }
        }


        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if (HttpContext.Current.Request.QueryString["id"].ToString() != "")
            {
                id = HttpContext.Current.Request.QueryString["id"].ToString();
            }
            decimal novoValorS500;
            decimal novoValorS10;
            decimal novoValorEtanol;
            decimal novoValorGasolina;
            decimal novoValorArla;
            string nomeUsuario = Session["UsuarioLogado"].ToString();

            if (!decimal.TryParse(txtS500.Text
                                     .Replace("R$", "")
                                     .Trim()
                                     .Replace(".", "")
                                     .Replace(",", "."),
                                  System.Globalization.NumberStyles.Any,
                                  System.Globalization.CultureInfo.InvariantCulture,
                                  out novoValorS500))
            {
                string script = "<script>showToast('Valor do Diesel S500, é inválido.');</script>";
                ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                txtS500.Focus();
                return;
            }
            if (!decimal.TryParse(txtS10.Text
                                    .Replace("R$", "")
                                    .Trim()
                                    .Replace(".", "")
                                    .Replace(",", "."),
                                 System.Globalization.NumberStyles.Any,
                                 System.Globalization.CultureInfo.InvariantCulture,
                                 out novoValorS10))
            {
                string script = "<script>showToast('Valor do Diesel S10, é inválido.');</script>";
                ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                txtS10.Focus();
                return;
            }
            if (!decimal.TryParse(txtEtanol.Text
                                    .Replace("R$", "")
                                    .Trim()
                                    .Replace(".", "")
                                    .Replace(",", "."),
                                 System.Globalization.NumberStyles.Any,
                                 System.Globalization.CultureInfo.InvariantCulture,
                                 out novoValorEtanol))
            {
                string script = "<script>showToast('Valor do Etanol, é inválido.');</script>";
                ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                txtEtanol.Focus();
                return;
            }
            if (!decimal.TryParse(txtGasolina.Text
                                    .Replace("R$", "")
                                    .Trim()
                                    .Replace(".", "")
                                    .Replace(",", "."),
                                 System.Globalization.NumberStyles.Any,
                                 System.Globalization.CultureInfo.InvariantCulture,
                                 out novoValorGasolina))
            {
                string script = "<script>showToast('Valor da Gasolina, é inválido.');</script>";
                ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                txtGasolina.Focus();
                return;
            }
            if (!decimal.TryParse(txtArla.Text
                                    .Replace("R$", "")
                                    .Trim()
                                    .Replace(".", "")
                                    .Replace(",", "."),
                                 System.Globalization.NumberStyles.Any,
                                 System.Globalization.CultureInfo.InvariantCulture,
                                 out novoValorArla))
            {
                string script = "<script>showToast('Valor da Arla, é inválido.');</script>";
                ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                txtArla.Focus();
                return;
            }
            using (SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                string query = "UPDATE tbfornecedores SET combustivel_S500 = @combustivel_S500,combustivel_S10=@combustivel_S10, combustivel_Etanol=@combustivel_Etanol, combustivel_gasolina=@combustivel_gasolina, combustivel_arla=@combustivel_arla, data_alteracao=@data_alteracao, usuario_alteracao=@usuario_alteracao WHERE id = @Id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.Add("@combustivel_S500", SqlDbType.Decimal).Value = novoValorS500;
                cmd.Parameters.Add("@combustivel_S10", SqlDbType.Decimal).Value = novoValorS10;
                cmd.Parameters.Add("@combustivel_Etanol", SqlDbType.Decimal).Value = novoValorEtanol;
                cmd.Parameters.Add("@combustivel_gasolina", SqlDbType.Decimal).Value = novoValorGasolina;
                cmd.Parameters.Add("@combustivel_arla", SqlDbType.Decimal).Value = novoValorArla;
                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                cmd.Parameters.Add("@data_alteracao", SqlDbType.VarChar).Value = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                cmd.Parameters.Add("@usuario_alteracao", SqlDbType.VarChar).Value = nomeUsuario;
                try
                {
                    con.Open();
                    int linhasAfetadas = cmd.ExecuteNonQuery();

                    if (linhasAfetadas > 0)
                    {
                        // Acione o toast quando a página for carregada
                        string script = "<script>showToast('Reajuste atualizado com sucesso.');</script>";
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                        txtS500.Focus();
                    }
                    else
                    {
                        // Acione o toast quando a página for carregada
                        string script = "<script>showToast('Id não encontrado para atualização.');</script>";
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                        txtS500.Focus();
                    }
                }
                catch (Exception ex)
                {
                    //lblMensagem.Text = "Erro: " + ex.Message;
                    // Acione o toast quando a página for carregada
                    string script = "<script>showToast('Erro ao atualizar fornecedor.');</script>";
                    ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                    txtS500.Focus();
                }
            }
        }
    }
}