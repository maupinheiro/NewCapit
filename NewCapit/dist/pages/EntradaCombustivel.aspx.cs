using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using DocumentFormat.OpenXml.Office.Word;
using NPOI.HPSF;

namespace NewCapit.dist.pages
{
    public partial class EntradaCombustivel : System.Web.UI.Page
    {
        SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString());
        string idPeca;
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
                CarregarCombustivel();
                CarregarTanqueCombustivel();
            }

        }
        public bool ValidarChaveNFe(string chave)
        {
            if (chave.Length != 44 || !chave.All(char.IsDigit))
                return false;

            int[] peso = { 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            int soma = 0;

            for (int i = 0; i < 43; i++)
                soma += (chave[i] - '0') * peso[i];

            int resto = soma % 11;
            int dv = resto < 2 ? 0 : 11 - resto;

            return dv == (chave[43] - '0');
        }
        protected void txtChave_TextChanged(object sender, EventArgs e)
        {
            string chave = txtChave.Text.Trim();

            if (chave != "")
            {
                if (!ValidarChaveNFe(chave))
                {
                    //ScriptManager.RegisterStartupScript(this, GetType(), "msg",
                    //    "alert('Chave NF-e inválida!');", true);
                    Mensagem("danger", "Chave de acesso da NF-e inválida!");
                    LimparCamposNF();
                    txtChave.Focus();
                    return;
                }

                //Pesquisar a chave na tabela
                using (SqlConnection conn = new SqlConnection(
                WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
                {
                    string sql = @"
                    SELECT chave_nf, fornecedor FROM tbentrada_peca
                    WHERE chave_nf = @chave";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@chave", chave);

                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        Mensagem("warning", "Chave: " + chave + " já lançada para o fornecedor: " + dr["fornecedor"].ToString());
                        txtChave.Text = "";
                        txtChave.Focus();
                    }
                    else
                    {
                        // Extrair dados
                        string cnpj = chave.Substring(6, 14);
                        string serie = chave.Substring(22, 3);
                        string numero = chave.Substring(25, 9);
                        // Extrai ano e mês
                        int anoChave = int.Parse(chave.Substring(2, 2));
                        int mesChave = int.Parse(chave.Substring(4, 2));

                        txtNF.Text = numero;
                        txtSerieNF.Text = serie;
                        txtCNPJ.Text = Convert.ToUInt64(cnpj).ToString(@"00\.000\.000\/0000\-00");

                        BuscarFornecedor(cnpj);

                    }
                }
            }
        }
        private void BuscarFornecedor(string cnpj)
        {
            using (SqlConnection conn = new SqlConnection(
                WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                string sql = @"
        SELECT codfor, fantasia, tipofornecedor 
        FROM tbfornecedores
        WHERE REPLACE(REPLACE(REPLACE(cnpj,'.',''),'-',''),'/','') = @cnpj";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@cnpj", cnpj);

                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    if(dr["tipofornecedor"].ToString() == "COMBUSTÍVEL")
                    {
                        txtCodFor.Text = dr["codfor"].ToString();
                        txtRazSocial.Text = dr["fantasia"].ToString();
                        txtEmissaoNF.Focus();
                    }
                    else
                    {
                        Mensagem("info", "- " + dr["fantasia"].ToString().Trim() + ", não é um fornecedor de combustível, verifique a nota ou altere o cadastro.");
                        LimparCamposNF();
                        txtChave.Focus();
                    }
                    
                }
                else
                {
                    //ScriptManager.RegisterStartupScript(this, GetType(), "msg",
                    //    "alert('Fornecedor não cadastrado! Cadastre antes de continuar.');", true);
                    Mensagem("info", "Fornecedor não cadastrado! Cadastre antes de continuar.");
                    txtCodFor.Text = "";
                    txtRazSocial.Text = "";
                    txtChave.Focus();
                }
            }
        }
        private void LimparCamposNF()
        {
            txtChave.Text = "";
            txtNF.Text = "";
            txtSerieNF.Text = "";
            txtCNPJ.Text = "";
            txtCodFor.Text = "";
            txtRazSocial.Text = "";
        }
        protected void Mensagem(string tipo, string texto)
        {
            divMsg.Visible = true;

            divMsg.Attributes["class"] =
                "alert alert-" + tipo + " alert-dismissible fade show mt-3";

            lblMsgGeral.Text = texto;
        }
        protected void MensagemModal(string tipo, string texto)
        {
            divMsg1.Visible = true;

            divMsg1.Attributes["class"] =
                "alert alert-" + tipo + " alert-dismissible fade show mt-3";

            lblMsgGeral1.Text = texto;
        }
        protected void btnSalvarEntrada_Click(object sender, EventArgs e)
        {
            // Variáveis para validação   
            string combustivel = txtIdCombustivel.Text.Trim();
            string chave = txtChave.Text.Trim();
            string emissao = txtEmissaoNF.Text.Trim();
            string DtEntrada = txtDataEntrada.Text.Trim();
            string quantidadeLitros = txtLitrosEntrada.Text.Trim();
            string valorUnitario = txtValorUnitario.Text.Trim();
            string valorTotal = txtValorTotalEntrada.Text.Trim();
            
            decimal estoqueAtual = decimal.Parse(txtEstoqueAtual.Text); 
            decimal litrosEntrada = decimal.Parse(txtLitrosEntrada.Text);
            decimal capacidade = decimal.Parse(txtCapTotal.Text);

            // Validações (Chave, Data, Quantidade, Valor)
            if (string.IsNullOrEmpty(chave)) { Mensagem("info", "Informe a chave da NF."); txtChave.Focus(); return; }

            DateTime dataEmissao;
            if (!DateTime.TryParseExact(emissao, "dd/MM/yyyy", new CultureInfo("pt-BR"), DateTimeStyles.None, out dataEmissao)) { Mensagem("info", "Data de emissão inválida (dd/MM/aaaa)."); txtEmissaoNF.Focus(); return; }

            DateTime dataEntrada;
            if (!DateTime.TryParseExact(DtEntrada, "dd/MM/yyyy", new CultureInfo("pt-BR"), DateTimeStyles.None, out dataEntrada)) { Mensagem("info", "Data de entrada inválida (dd/MM/aaaa)."); txtDataEntrada.Focus(); return; }

            if (dataEntrada < dataEmissao || dataEntrada > DateTime.Today) { Mensagem("danger", "Data de entrada não pode ser maior que a data atual."); txtDataEntrada.Focus(); return; }
            
            if (dataEmissao > DateTime.Today) { Mensagem("danger", "Data de emissão não pode ser maior que a data atual."); txtEmissaoNF.Focus(); return; }

            int anoEmissao2Dig = dataEmissao.Year % 100;
            int anoChave = int.Parse(chave.Substring(2, 2));
            int mesChave = int.Parse(chave.Substring(4, 2));
            if (anoEmissao2Dig != anoChave || dataEmissao.Month != mesChave) { Mensagem("info", "Mês e ano da data de emissão não correspondem ao mês e ano da NF."); txtEmissaoNF.Focus(); return; }

            decimal quantidade;
            if (!decimal.TryParse(quantidadeLitros, NumberStyles.Number, new CultureInfo("pt-BR"), out quantidade) || quantidade <= 0)
            {
                Mensagem("info", "Informe uma quantidade válida.");
                txtLitrosEntrada.Focus();
                return;
            }

            decimal valorUnitarioDecimal;
            if (!decimal.TryParse(valorUnitario, NumberStyles.Number, new CultureInfo("pt-BR"), out valorUnitarioDecimal) || valorUnitarioDecimal <= 0)
            {
                Mensagem("info", "Informe um valor válido.");
                txtValorUnitario.Focus();
                return;
            }

            decimal valorTotalEntrada;
            if (!decimal.TryParse(valorTotal, NumberStyles.Number, new CultureInfo("pt-BR"), out valorTotalEntrada) || valorTotalEntrada <= 0)
            {
                Mensagem("info", "Informe um valor total válido.");
                txtValorUnitario.Focus();
                return;
            }

            string usuariologado = Session["UsuarioLogado"].ToString();
            string dataHoraAtual = DateTime.Now.ToString("dd/MM/yyyy HH:mm");


            if (estoqueAtual + litrosEntrada > capacidade)
            {                
                Mensagem("info", "⚠️ Não é possível registrar. Estoque atual " + estoqueAtual + "L + entrada " + litrosEntrada + "L ultrapassa a capacidade total de " + capacidade +"L.");
                return;
            }


            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();

                try
                {
                    // --- 1. Inserir na tbentrada_combustivel ---
                    string sqlEntrada = @"
                INSERT INTO tbentrada_combustivel
                (DataEntrada, TanqueId, Litros, ValorTotal,ValorLitro,NotaFiscal,Serie,ChaveNF,Emissao,Fornecedor,Cnpj,Usuario)
                 VALUES (@Data, @Tanque, @Litros, @Valor,@ValorLitro,@NotaFiscal,@Serie,@ChaveNF,@Emissao,@Fornecedor,@Cnpj,@Usuario)";

                    SqlCommand cmdEntrada = new SqlCommand(sqlEntrada, conn, trans);
                    cmdEntrada.Parameters.AddWithValue("@Data", dataEntrada.ToString("yyyy-MM-dd"));
                    cmdEntrada.Parameters.AddWithValue("@Tanque", ddlTanqueEntrada.SelectedValue);
                    cmdEntrada.Parameters.AddWithValue("@Litros", litrosEntrada);
                    cmdEntrada.Parameters.AddWithValue("@Valor", valorTotalEntrada);
                    cmdEntrada.Parameters.AddWithValue("@ValorLitro", valorUnitarioDecimal);
                    cmdEntrada.Parameters.AddWithValue("@NotaFiscal", int.Parse(txtNF.Text));
                    cmdEntrada.Parameters.AddWithValue("@Serie", int.Parse(txtSerieNF.Text));
                    cmdEntrada.Parameters.AddWithValue("@ChaveNF", txtChave.Text);
                    cmdEntrada.Parameters.AddWithValue("@Emissao", dataEmissao.ToString("yyyy-MM-dd"));
                    cmdEntrada.Parameters.AddWithValue("@Fornecedor", txtRazSocial.Text);
                    cmdEntrada.Parameters.AddWithValue("@Cnpj", txtCNPJ.Text);                    
                    cmdEntrada.Parameters.AddWithValue("@Usuario", dataHoraAtual + " - " + usuariologado);
                    cmdEntrada.ExecuteNonQuery();

                    // --- 3. Atualizar estoque ---
                    SqlCommand cmdEstoque = new SqlCommand(
                        "UPDATE tbestoque_pecas SET valor_unitario=@valor, estoque_peca = estoque_peca + @qtd WHERE id_peca = @id",
                        conn, trans);

                    cmdEstoque.Parameters.AddWithValue("@qtd", quantidade);
                    cmdEstoque.Parameters.AddWithValue("@id", combustivel);
                    cmdEstoque.Parameters.AddWithValue("@valor", valorUnitarioDecimal);
                    cmdEstoque.ExecuteNonQuery();

                    // --- 4. Commit ---
                    trans.Commit();

                    //Limpar();
                    //CarregarGrid();
                    Response.Redirect("EntradaPecas.aspx");
                }
                catch (Exception ex)
                {
                    try { trans.Rollback(); } catch { }
                    Mensagem("danger", "Erro na transação: " + ex.Message);
                }
            }
        }
        private void CarregarCombustivel()
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                conn.Open();
                string query = "SELECT id_peca, descricao_peca FROM tbestoque_pecas where tipo_peca = 'COMBUSTIVEL' ORDER BY descricao_peca";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                ddlCombustivel.DataSource = reader;
                ddlCombustivel.DataTextField = "descricao_peca";  // Campo a ser exibido
                ddlCombustivel.DataValueField = "id_peca";  // Valor associado ao item
                ddlCombustivel.DataBind();

                // Adicionar o item padrão
                ddlCombustivel.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Selecione...", ""));
            }
        }
        private void CarregarTanqueCombustivel()
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                conn.Open();
                string query = "SELECT id,nome FROM tbtanque_combustivel ORDER BY nome";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                ddlTanqueEntrada.DataSource = reader;
                ddlTanqueEntrada.DataTextField = "nome";  // Campo a ser exibido
                ddlTanqueEntrada.DataValueField = "id";  // Valor associado ao item
                ddlTanqueEntrada.DataBind();

                // Adicionar o item padrão
                ddlTanqueEntrada.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Selecione...", ""));
            }
        }
        protected void btnSalvarTanque_Click(object sender, EventArgs e)
        {
            string nomeTanque = txtNomeTanque.Text.ToUpper().Trim();            
            string capacidadeTanque = txtCapacidadeTotal.Text.Trim();
            using (SqlConnection conn = new SqlConnection(
                WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                conn.Open();

                // 🔍 Verifica se já existe
                string checkSql = "SELECT COUNT(*) FROM tbtanque_combustivel WHERE Nome = @Nome";
                SqlCommand checkCmd = new SqlCommand(checkSql, conn);
                checkCmd.Parameters.AddWithValue("@Nome", nomeTanque);

                int existe = (int)checkCmd.ExecuteScalar();

                if (existe > 0)
                {
                    // ❌ Não deixa salvar
                    ScriptManager.RegisterStartupScript(this, GetType(), "erro",
                        "alert('❌ Já existe um tanque com esse nome!'); abrirModalTanque();", true);                   
                    return;
                }

                decimal quantidadeMax;
                if (!decimal.TryParse(capacidadeTanque, NumberStyles.Number, new CultureInfo("pt-BR"), out quantidadeMax) || quantidadeMax <= 0)
                {
                    //Mensagem1("info", "Informe uma quantidade válida.");
                    txtCapTotal.Focus();
                    return;
                }

                // 💾 Salvar
                string sql = @"INSERT INTO tbtanque_combustivel (Nome, Capacidade, IdCombustivel, DescricaoCombustivel)
                       VALUES (@Nome, @Capacidade, @IdCombustivel, @DescricaoCombustivel)";

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@Nome", nomeTanque);
                cmd.Parameters.AddWithValue("@Capacidade", quantidadeMax);
                cmd.Parameters.AddWithValue("@IdCombustivel", ddlCombustivel.SelectedValue);
                cmd.Parameters.AddWithValue("@DescricaoCombustivel", ddlCombustivel.SelectedItem.Text);
                cmd.ExecuteNonQuery();
            }
            CarregarCombustivel();
            CarregarTanqueCombustivel();

            // ✅ Sucesso
            ScriptManager.RegisterStartupScript(this, GetType(), "ok",
                "alert('✅ Tanque cadastrado com sucesso!'); fecharModalTanque();", true);
        }
        protected void txtIdTanque_TextChanged(object sender, EventArgs e)
        {
            if (txtIdTanque.Text != "")
            {
                string codTanque = txtIdTanque.Text;
                string sql = "SELECT  * FROM tbtanque_combustivel where id = '" + codTanque + "' and Status = 'ATIVO' and fl_exclusao is null";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                conn.Open();
                da.Fill(dt);
                conn.Close();

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["fl_exclusao"].ToString() == null)
                    {                        
                        Mensagem("info", "- " + dt.Rows[0]["nome"].ToString().Trim() + ", não é um fornecedor de combustível valído.");
                        txtIdTanque.Text = "";
                        txtIdTanque.Focus();
                        return;
                    }
                    else if (dt.Rows[0]["Status"].ToString() == "INATIVO")
                    {
                        // Acione o toast quando a página for carregada
                        Mensagem("info", "- " + dt.Rows[0]["nome"].ToString().Trim() + ", não está ATIVO.");
                        txtIdTanque.Text = "";
                        txtIdTanque.Focus();
                        return;
                    }
                    else
                    {
                        txtIdTanque.Text = dt.Rows[0]["id"].ToString();
                        ddlTanqueEntrada.SelectedItem.Text = dt.Rows[0]["nome"].ToString();
                        txtIdCombustivel.Text = dt.Rows[0]["idcombustivel"].ToString();
                        txtDescCombustivel.Text = dt.Rows[0]["descricaocombustivel"].ToString();
                        txtEstoqueAtual.Text = dt.Rows[0]["saldoatual"].ToString();
                        txtCapTotal.Text = dt.Rows[0]["capacidade"].ToString();
                        txtDataEntrada.Focus();
                        return;
                    }

                }
                else
                {
                    // Acione o toast quando a página for carregada
                    Mensagem("info", "- Fornecedor não está cadastrado no sistema.");
                    txtIdTanque.Text = "";
                    txtIdTanque.Focus();
                    return;
                }
            }

        } 
        protected void ddlTanqueEntrada_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtIdTanque.Text = ddlTanqueEntrada.SelectedValue;
            if (txtIdTanque.Text != "")
            {
                string codTanque = txtIdTanque.Text;
                string sql = "SELECT  * FROM tbtanque_combustivel where id = '" + codTanque + "' and Status = 'ATIVO' and fl_exclusao is null";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                conn.Open();
                da.Fill(dt);
                conn.Close();

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["fl_exclusao"].ToString() == null)
                    {
                        // Acione o toast quando a página for carregada
                        //string script = "<script>showToast(' - Remetente deletado do sistema.');</script>";
                        //ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                        Mensagem("info", "- " + dt.Rows[0]["nome"].ToString().Trim() + ", não é um fornecedor de combustível valído.");
                        txtIdTanque.Text = "";
                        txtIdTanque.Focus();
                        return;
                    }
                    else if (dt.Rows[0]["Status"].ToString() == "INATIVO")
                    {
                        // Acione o toast quando a página for carregada
                        Mensagem("info", "- " + dt.Rows[0]["nome"].ToString().Trim() + ", não está ATIVO.");
                        txtIdTanque.Text = "";
                        txtIdTanque.Focus();
                        return;
                    }
                    else
                    {
                        txtIdTanque.Text = dt.Rows[0]["id"].ToString();
                        ddlTanqueEntrada.SelectedItem.Text = dt.Rows[0]["nome"].ToString();
                        txtIdCombustivel.Text = dt.Rows[0]["idcombustivel"].ToString();
                        txtDescCombustivel.Text = dt.Rows[0]["descricaocombustivel"].ToString();
                        txtEstoqueAtual.Text = dt.Rows[0]["saldoatual"].ToString();
                        txtCapTotal.Text = dt.Rows[0]["capacidade"].ToString();
                        txtDataEntrada.Focus();
                        return;
                    }

                }
                else
                {
                    // Acione o toast quando a página for carregada
                    Mensagem("info", "- Fornecedor não está cadastrado no sistema.");
                    txtIdTanque.Text = "";
                    txtIdTanque.Focus();
                    return;
                }
            }
        }
    }
}