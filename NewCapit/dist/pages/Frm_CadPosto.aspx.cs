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
using System.Globalization;

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
            if (HttpContext.Current.Request.QueryString["codfor"].ToString() != "")
            {
                id = HttpContext.Current.Request.QueryString["codfor"].ToString();
            }
            string sql = "SELECT codfor, razaosocial, fantasia, cnpj, inscestadual, inscccm, tipoempresa, abertura, situacaoreceita, tipofornecedor, contato, fonefixo, fonecelular, email, site, cep, endereco, numero, complemento, bairro, cidade, estado, pais, combustivel_S10, combustivel_S500, combustivel_etanol, combustivel_gasolina, combustivel_arla, data_cadastro, usuario_cadastro,data_alteracao, usuario_alteracao FROM tbfornecedores WHERE codfor = " + id;

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
                    if (dt.Rows.Count > 0)
                    {
                        object valor = dt.Rows[0]["data_alteracao"];

                        if (valor != DBNull.Value && valor != null)
                            lblDataAlteracao.Text = Convert.ToDateTime(valor).ToString("dd/MM/yyyy HH:mm");
                        else
                            lblDataAlteracao.Text = "-"; // ou string vazia, se preferir
                    }
                    txtAlteradoPor.Text = dt.Rows[0]["usuario_alteracao"].ToString();
                }
                

            }
        }       
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            string codigoPosto = txtCodFor.Text.Trim();
            string nomePosto = txtNomFor.Text.Trim();
            string nomeUsuario = Session["UsuarioLogado"].ToString();

            decimal novoValorS500 = 0;
            decimal novoValorS10 = 0;
            decimal novoValorEtanol = 0;
            decimal novoValorGasolina = 0;
            decimal novoValorArla = 0;

            // Diesel S500
            if (!string.IsNullOrWhiteSpace(txtS500.Text))
            {
                if (!decimal.TryParse(txtS500.Text, NumberStyles.Any, new CultureInfo("pt-BR"), out novoValorS500))
                {
                    Mensagem("warning", "Valor do Diesel S500 inválido");
                    txtS500.Text = "";
                    txtS500.Focus();
                    return;
                }
            }

            // Diesel S10
            if (!string.IsNullOrWhiteSpace(txtS10.Text))
            {
                if (!decimal.TryParse(txtS10.Text, NumberStyles.Any, new CultureInfo("pt-BR"), out novoValorS10))
                {                    
                    Mensagem("warning", "Valor do Diesel S10 inválido");
                    txtS10.Text = "";
                    txtS10.Focus();
                    return;
                }
            }

            // Etanol
            if (!string.IsNullOrWhiteSpace(txtEtanol.Text))
            {
                if (!decimal.TryParse(txtEtanol.Text, NumberStyles.Any, new CultureInfo("pt-BR"), out novoValorEtanol))
                {                   
                    Mensagem("warning", "Valor do Etanol inválido");
                    txtEtanol.Text = "";
                    txtEtanol.Focus();
                    return;
                }
            }

            // Gasolina
            if (!string.IsNullOrWhiteSpace(txtGasolina.Text))
            {
                if (!decimal.TryParse(txtGasolina.Text, NumberStyles.Any, new CultureInfo("pt-BR"), out novoValorGasolina))
                {                   
                    Mensagem("warning", "Valor da Gasolina inválido");
                    txtGasolina.Text = "";
                    txtGasolina.Focus();
                    return;
                }
            }

            // Arla
            if (!string.IsNullOrWhiteSpace(txtArla.Text))
            {
                if (!decimal.TryParse(txtArla.Text, NumberStyles.Any, new CultureInfo("pt-BR"), out novoValorArla))
                {                   
                    Mensagem("warning", "Valor do Arla 32 inválido");
                    txtArla.Text = "";
                    txtArla.Focus();
                    return;
                }
            }

            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();

                try
                {
                    // 1️⃣ Ler valores atuais do fornecedor
                    string selectFornecedor = @"SELECT combustivel_S500, combustivel_S10, combustivel_Etanol, combustivel_gasolina, combustivel_arla
                                    FROM tbfornecedores
                                    WHERE codfor = @codfor";

                    SqlCommand cmdSelect = new SqlCommand(selectFornecedor, conn, trans);
                    cmdSelect.Parameters.AddWithValue("@codfor", codigoPosto);

                    SqlDataReader dr = cmdSelect.ExecuteReader();
                    decimal dbS500 = 0, dbS10 = 0, dbEtanol = 0, dbGasolina = 0, dbArla = 0;

                    if (dr.Read())
                    {
                        dbS500 = dr["combustivel_S500"] != DBNull.Value ? Convert.ToDecimal(dr["combustivel_S500"]) : 0;
                        dbS10 = dr["combustivel_S10"] != DBNull.Value ? Convert.ToDecimal(dr["combustivel_S10"]) : 0;
                        dbEtanol = dr["combustivel_Etanol"] != DBNull.Value ? Convert.ToDecimal(dr["combustivel_Etanol"]) : 0;
                        dbGasolina = dr["combustivel_gasolina"] != DBNull.Value ? Convert.ToDecimal(dr["combustivel_gasolina"]) : 0;
                        dbArla = dr["combustivel_arla"] != DBNull.Value ? Convert.ToDecimal(dr["combustivel_arla"]) : 0;
                    }
                    dr.Close();

                    // 2️⃣ Função para atualizar histórico + fornecedor se valor mudou
                    void AtualizarSeMudou(string combustivel, decimal valorNovo, decimal valorAtual, string campoFornecedor)
                    {
                        if (valorNovo != valorAtual)
                        {
                            // Atualiza histórico
                            string updateHist = @"UPDATE tbPrecoCombustivel
                                      SET status = 'INATIVO', dtreajuste = GETDATE()
                                      WHERE codposto = @codposto AND combustivel = @combustivel AND status = 'ATIVO'";
                            SqlCommand cmdUpdate = new SqlCommand(updateHist, conn, trans);
                            cmdUpdate.Parameters.AddWithValue("@codposto", codigoPosto);
                            cmdUpdate.Parameters.AddWithValue("@combustivel", combustivel);
                            cmdUpdate.ExecuteNonQuery();

                            // Inserir novo registro histórico
                            string insertHist = @"INSERT INTO tbPrecoCombustivel
                                      (codposto, nomeposto, combustivel, valor, dtinicio, status, reajustadopor)
                                      VALUES (@codposto, @nomeposto, @combustivel, @valor, GETDATE(), 'ATIVO', @reajustadopor)";
                            SqlCommand cmdInsert = new SqlCommand(insertHist, conn, trans);
                            cmdInsert.Parameters.AddWithValue("@codposto", codigoPosto);
                            cmdInsert.Parameters.AddWithValue("@nomeposto", nomePosto);
                            cmdInsert.Parameters.AddWithValue("@combustivel", combustivel);
                            cmdInsert.Parameters.AddWithValue("@valor", valorNovo);
                            cmdInsert.Parameters.AddWithValue("@reajustadopor", nomeUsuario);
                            cmdInsert.ExecuteNonQuery();

                            // Atualiza tbfornecedores
                            string updateFornecedor = $"UPDATE tbfornecedores SET {campoFornecedor}=@valor, data_alteracao=GETDATE(), usuario_alteracao=@usuario WHERE codfor=@codfor";
                            SqlCommand cmdFornecedor = new SqlCommand(updateFornecedor, conn, trans);
                            cmdFornecedor.Parameters.AddWithValue("@valor", valorNovo);
                            cmdFornecedor.Parameters.AddWithValue("@usuario", nomeUsuario);
                            cmdFornecedor.Parameters.AddWithValue("@codfor", codigoPosto);
                            cmdFornecedor.ExecuteNonQuery();
                        }
                    }

                    // 3️⃣ Comparar e atualizar somente se mudou
                    AtualizarSeMudou("DIESEL S500", novoValorS500, dbS500, "combustivel_S500");
                    AtualizarSeMudou("DIESEL S10", novoValorS10, dbS10, "combustivel_S10");
                    AtualizarSeMudou("ETANOL", novoValorEtanol, dbEtanol, "combustivel_Etanol");
                    AtualizarSeMudou("GASOLINA", novoValorGasolina, dbGasolina, "combustivel_gasolina");
                    AtualizarSeMudou("ARLA 32", novoValorArla, dbArla, "combustivel_arla");

                    trans.Commit();
                    Mensagem("success", "Reajuste salvo com sucesso!");
                    return;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    Mensagem("danger", "Erro: " + ex.Message + " ao salvar reajuste.");
                    return;
                }
            }
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