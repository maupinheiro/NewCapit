using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using DocumentFormat.OpenXml.Wordprocessing;

namespace NewCapit.dist.pages
{
    public partial class EntradaPecas : System.Web.UI.Page
    {
        string conexao = WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
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
                CarregaDadosDaPeca();
                CarregarMarcaPneu();
            }

        }
        public void CarregaDadosDaPeca()
        {  
            if (Request.QueryString["id"] != null)
            {
                idPeca = Request.QueryString["id"].ToString();
            }
            else
            {
                Mensagem("info", "Nenhum ID de peça informado.");
                return;
            }


            string codigoPeca = idPeca;
            string strConn = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                string queryTNG = "SELECT id_peca, descricao_peca, estoque_peca, unidade, tipo_peca FROM tbestoque_pecas WHERE id_peca =  @id_peca";

                using (SqlCommand cmdTNG = new SqlCommand(queryTNG, conn))
                {
                    cmdTNG.Parameters.AddWithValue("@id_peca", codigoPeca);
                    conn.Open();

                    using (SqlDataReader reader = cmdTNG.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Converte para string antes de atribuir aos TextBox
                            txtIdPeca.Text = reader["id_peca"].ToString();
                            txtPeca.Text = reader["descricao_peca"].ToString();
                            txtEstoqueAtual.Text = reader["estoque_peca"].ToString();
                            txtUnidade.Text = reader["unidade"].ToString();
                            txtTipoPeca.Text = reader["tipo_peca"].ToString();
                            if (reader["tipo_peca"].ToString() == "PNEU")
                            {                                
                                divPneu.Visible = true;

                            }
                            else
                            {
                                divPneu.Visible = false;
                            }
                        }
                    }
                }
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
        SELECT codfor, fantasia 
        FROM tbfornecedores
        WHERE REPLACE(REPLACE(REPLACE(cnpj,'.',''),'-',''),'/','') = @cnpj";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@cnpj", cnpj);

                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    txtCodFor.Text = dr["codfor"].ToString();
                    txtRazSocial.Text = dr["fantasia"].ToString();
                    txtEmissaoNF.Focus();
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
        protected void btnConfirmar_Click(object sender, EventArgs e)
        {
            // Variáveis para validação            
            string tipoPeca = txtTipoPeca.Text.Trim();
            string chave = txtChave.Text.Trim();
            string emissao = txtEmissaoNF.Text.Trim();
            string quantidadeStr = txtQuantidade.Text.Trim();
            string valorStr = txtValor.Text.Trim();
            string marca = ddlMarca.SelectedItem.Text;
            string modelo = txtModelo.Text.Trim();
            string medidas = txtMedida.Text.Trim();

            // Validações (Chave, Data, Quantidade, Valor)
            if (string.IsNullOrEmpty(chave)) { Mensagem("info", "Informe a chave da NF."); txtChave.Focus(); return; }

            DateTime dataEmissao;
            if (!DateTime.TryParseExact(emissao, "dd/MM/yyyy", new CultureInfo("pt-BR"), DateTimeStyles.None, out dataEmissao)) { Mensagem("info", "Data de emissão inválida (dd/MM/aaaa)."); txtEmissaoNF.Focus(); return; }
            if (dataEmissao > DateTime.Today) { Mensagem("danger", "Data de emissão não pode ser maior que a data atual."); txtEmissaoNF.Focus(); return; }

            int anoEmissao2Dig = dataEmissao.Year % 100;
            int anoChave = int.Parse(chave.Substring(2, 2));
            int mesChave = int.Parse(chave.Substring(4, 2));
            if (anoEmissao2Dig != anoChave || dataEmissao.Month != mesChave) { Mensagem("info", "Mês e ano da data de emissão não correspondem ao mês e ano da NF."); txtEmissaoNF.Focus(); return; }

            long quantidade;
            if (!long.TryParse(quantidadeStr, NumberStyles.Number, CultureInfo.InvariantCulture, out quantidade) || quantidade <= 0) { Mensagem("info", "Informe uma quantidade válida."); txtQuantidade.Focus(); return; }

            decimal valor;
            if (!decimal.TryParse(valorStr, NumberStyles.Currency, new CultureInfo("pt-BR"), out valor) || valor <= 0) { Mensagem("info", "Informe um valor válido."); txtValor.Focus(); return; }

            string usuariologado = Session["UsuarioLogado"].ToString();
            string dataHoraAtual = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();

                try
                {
                    // --- 1. Inserir na tbentrada_peca ---
                    string sqlEntrada = @"
                INSERT INTO tbentrada_peca
                (id_peca, descricao_peca, quantidade, data_entrada, tipo_entrada, id_fornecedor, fornecedor, cnpj_fornecedor, valor_unitario, responsavel, chave_nf, nota_fiscal, serie_nf, emissao_nf, unidade, marca_pneu, modelo_pneu, medida_pneu)
                VALUES
                (@id_peca, @descricao_peca, @quantidade, GETDATE(), @tipo, @id_fornecedor, @fornecedor, @cnpj_fornecedor, @valor, @resp, @chave_nf, @nota_fiscal, @serie_nf, @emissao_nf, @unidade, @marca_pneu, @modelo_pneu, @medida_pneu)";

                    SqlCommand cmdEntrada = new SqlCommand(sqlEntrada, conn, trans);
                    cmdEntrada.Parameters.AddWithValue("@id_peca", txtIdPeca.Text);
                    cmdEntrada.Parameters.AddWithValue("@descricao_peca", txtPeca.Text);
                    cmdEntrada.Parameters.AddWithValue("@quantidade", quantidade);
                    cmdEntrada.Parameters.AddWithValue("@tipo", ddlTipoEntrada.Text);
                    cmdEntrada.Parameters.AddWithValue("@id_fornecedor", txtCodFor.Text);
                    cmdEntrada.Parameters.AddWithValue("@fornecedor", txtRazSocial.Text);
                    cmdEntrada.Parameters.AddWithValue("@cnpj_fornecedor", txtCNPJ.Text);
                    cmdEntrada.Parameters.AddWithValue("@valor", valor);
                    cmdEntrada.Parameters.AddWithValue("@resp", usuariologado);
                    cmdEntrada.Parameters.AddWithValue("@chave_nf", txtChave.Text);
                    cmdEntrada.Parameters.AddWithValue("@nota_fiscal", int.Parse(txtNF.Text));
                    cmdEntrada.Parameters.AddWithValue("@serie_nf", int.Parse(txtSerieNF.Text));
                    cmdEntrada.Parameters.AddWithValue("@emissao_nf", dataEmissao.ToString("yyyy-MM-dd"));
                    cmdEntrada.Parameters.AddWithValue("@unidade", txtUnidade.Text);

                    // Campos específicos de pneu
                    if (tipoPeca == "PNEU")
                    {
                        cmdEntrada.Parameters.AddWithValue("@marca_pneu", marca);
                        cmdEntrada.Parameters.AddWithValue("@modelo_pneu", modelo);
                        cmdEntrada.Parameters.AddWithValue("@medida_pneu", medidas);
                    }
                    else
                    {
                        cmdEntrada.Parameters.AddWithValue("@marca_pneu", "");
                        cmdEntrada.Parameters.AddWithValue("@modelo_pneu", "");
                        cmdEntrada.Parameters.AddWithValue("@medida_pneu", "");
                    }

                    cmdEntrada.ExecuteNonQuery();

                    // --- 2. Inserir múltiplos registros na tbPneus se for PNEU ---
                    if (tipoPeca == "PNEU")
                    {
                        for (int i = 0; i < quantidade; i++)
                        {
                            SqlCommand cmdPneu = new SqlCommand(@"
                        INSERT INTO tbPneus
                        (Id_Peca, Descricao, Marca, Modelo, Medida, DataCompra, Valor, Status, Resp_Entrada)
                        VALUES (@id_peca, @descricao, @marca, @modelo, @medida, @datacompra, @valor, @status, @resp_entrada)",
                                conn, trans); // ✅ Transação aplicada

                            cmdPneu.Parameters.AddWithValue("@id_peca", txtIdPeca.Text);
                            cmdPneu.Parameters.AddWithValue("@descricao", txtPeca.Text);
                            cmdPneu.Parameters.AddWithValue("@marca", marca);
                            cmdPneu.Parameters.AddWithValue("@modelo", modelo);
                            cmdPneu.Parameters.AddWithValue("@medida", medidas);
                            cmdPneu.Parameters.AddWithValue("@datacompra", dataEmissao.ToString("yyyy-MM-dd"));
                            cmdPneu.Parameters.AddWithValue("@valor", valor);
                            cmdPneu.Parameters.AddWithValue("@status", "Estoque");
                            cmdPneu.Parameters.AddWithValue("@resp_entrada", dataHoraAtual + " - " + usuariologado);

                            cmdPneu.ExecuteNonQuery();
                        }
                    }

                    // --- 3. Atualizar estoque ---
                    SqlCommand cmdEstoque = new SqlCommand(
                        "UPDATE tbestoque_pecas SET valor_unitario=@valor, estoque_peca = estoque_peca + @qtd WHERE id_peca = @id",
                        conn, trans);

                    cmdEstoque.Parameters.AddWithValue("@qtd", quantidade);
                    cmdEstoque.Parameters.AddWithValue("@id", txtIdPeca.Text);
                    cmdEstoque.Parameters.AddWithValue("@valor", valor);
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


        //protected void btnConfirmar_Click(object sender, EventArgs e)
        //{           
        //    // Variáveis para validação            
        //    string tipoPeca = txtTipoPeca.Text.Trim();
        //    string chave = txtChave.Text.Trim();
        //    string emissao = txtEmissaoNF.Text.Trim();
        //    string quantidadeStr = txtQuantidade.Text.Trim();
        //    string valorStr = txtValor.Text.Trim();
        //    string marca = ddlMarca.SelectedItem.Text;
        //    string modelo = txtModelo.Text.Trim();
        //    string medidas = txtMedida.Text.Trim();

        //    // 1️⃣ Validar Chave (não vazio)
        //    if (string.IsNullOrEmpty(chave))
        //    {                
        //        Mensagem("info", "Informe a chave da NF.");
        //        txtChave.Focus();
        //        return;
        //    }

        //    // 2️⃣ Validar Emissão NF (dd/MM/yyyy)
        //    DateTime dataEmissao;
        //    if (!DateTime.TryParseExact(emissao, "dd/MM/yyyy", new CultureInfo("pt-BR"), DateTimeStyles.None, out dataEmissao))
        //    {                
        //        Mensagem("info", "Data de emissão inválida (dd/MM/aaaa).");
        //        txtEmissaoNF.Focus();
        //        return;
        //    }
        //    // Valida se não é maior que hoje
        //    if (dataEmissao > DateTime.Today)
        //    {
        //        Mensagem("danger", "Data de emissão não pode ser maior que a data atual.");
        //        txtEmissaoNF.Focus();
        //        return;
        //    }

        //    // Converte ano da emissão para 2 dígitos
        //    int anoEmissao2Dig = dataEmissao.Year % 100;
        //    // Extrai ano e mês
        //    int anoChave = int.Parse(chave.Substring(2, 2));
        //    int mesChave = int.Parse(chave.Substring(4, 2));
        //    if (anoEmissao2Dig != anoChave || dataEmissao.Month != mesChave)
        //    {
        //        Mensagem("info", "Mês e ano da data de emissão não correspondem ao mês e ano da NF.");
        //        txtEmissaoNF.Focus();
        //        return;
        //    }

        //    // 3️⃣ Validar Quantidade (maior que 0)
        //    long quantidade;
        //    if (!long.TryParse(quantidadeStr, NumberStyles.Number, CultureInfo.InvariantCulture, out quantidade) || quantidade <= 0)
        //    {                
        //        Mensagem("info", "Informe uma quantidade válida.");
        //        txtQuantidade.Focus();
        //        return;
        //    }

        //    // 4️⃣ Validar Valor (moeda)
        //    decimal valor;
        //    if (!decimal.TryParse(valorStr, NumberStyles.Currency, new CultureInfo("pt-BR"), out valor) || valor <= 0)
        //    {                
        //        Mensagem("info", "Informe um valor válido.");
        //        txtValor.Focus();
        //        return;
        //    }
        //    using (SqlConnection conn = new SqlConnection(
        //        WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
        //    {
        //        conn.Open();
        //        SqlTransaction trans = conn.BeginTransaction();

        //        try
        //        {
        //            string sql = @"INSERT INTO tbentrada_peca
        //    (id_peca,descricao_peca, quantidade, data_entrada, tipo_entrada, id_fornecedor, fornecedor, cnpj_fornecedor, valor_unitario, responsavel, chave_nf, nota_fiscal, serie_nf, emissao_nf, unidade, marca_pneu, modelo_pneu, medida_pneu)
        //    VALUES
        //    (@id_peca, @descricao_peca, @quantidade, GETDATE(), @tipo, @id_fornecedor, @fornecedor, @cnpj_fornecedor, @valor, @resp, @chave_nf, @nota_fiscal, @serie_nf, @emissao_nf, @unidade, @marca_pneu, @modelo_pneu, @medida_pneu)";

        //            SqlCommand cmd = new SqlCommand(sql, conn, trans);
        //            cmd.Parameters.AddWithValue("@id_peca", txtIdPeca.Text);
        //            cmd.Parameters.AddWithValue("@descricao_peca", txtPeca.Text);
        //            long quantidadeComprada = 0;
        //            if (!string.IsNullOrEmpty(txtQuantidade.Text))
        //            {
        //                quantidadeComprada = long.Parse(txtQuantidade.Text);
        //            }
        //            cmd.Parameters.AddWithValue("@quantidade", quantidadeComprada);
        //            cmd.Parameters.AddWithValue("@tipo", ddlTipoEntrada.Text);
        //            cmd.Parameters.AddWithValue("@id_fornecedor", txtCodFor.Text);
        //            cmd.Parameters.AddWithValue("@unidade", txtUnidade.Text);
        //            cmd.Parameters.AddWithValue("@fornecedor", txtRazSocial.Text);
        //            cmd.Parameters.AddWithValue("@cnpj_fornecedor", txtCNPJ.Text);
        //            cmd.Parameters.AddWithValue("@valor", string.IsNullOrEmpty(txtValor.Text) ? 0 : Convert.ToDecimal(txtValor.Text));
        //            cmd.Parameters.AddWithValue("@resp", Session["UsuarioLogado"]);
        //            string numNF = txtNF.Text;
        //            int numeroNF = int.Parse(numNF);
        //            cmd.Parameters.AddWithValue("@chave_nf", txtChave.Text);
        //            cmd.Parameters.AddWithValue("@nota_fiscal", numeroNF);
        //            string numSerie = txtSerieNF.Text;
        //            int numeroSerie = int.Parse(numSerie);
        //            cmd.Parameters.AddWithValue("@serie_nf", numeroSerie);                    
        //            cmd.Parameters.AddWithValue("@emissao_nf", dataEmissao.ToString("yyyy-MM-dd"));

        //            // Campos específicos de pneu
        //            if (tipoPeca == "PNEU")
        //            {
        //                cmd.Parameters.AddWithValue("@marca_pneu", marca);
        //                cmd.Parameters.AddWithValue("@modelo_pneu", modelo);
        //                cmd.Parameters.AddWithValue("@medida_pneu", medidas);
        //            }
        //            else
        //            {
        //                cmd.Parameters.AddWithValue("@marca_pneu", "");
        //                cmd.Parameters.AddWithValue("@modelo_pneu", "");
        //                cmd.Parameters.AddWithValue("@medida_pneu", "");

        //            }
        //            cmd.ExecuteNonQuery();


        //            // --- 2. Se for PNEU, salvar múltiplos registros na tbPneus ---
        //            if (tipoPeca == "PNEU")
        //            {
        //                for (int i = 0; i < quantidade; i++)
        //                {
        //                    SqlCommand cmdPneu = new SqlCommand(@"
        //            INSERT INTO tbPneus
        //            (Id_Peca, Descricao, Marca, Modelo, Medida, DataCompra, Valor, Status, Resp_Entrada)
        //            VALUES (@id_peca, @descricao, @marca, @modelo, @medida, @datacompra, @valor, @status, @resp_entrada)", conn);

        //                    cmdPneu.Parameters.AddWithValue("@id_peca", txtIdPeca.Text);
        //                    cmdPneu.Parameters.AddWithValue("@descricao", txtPeca.Text);
        //                    cmdPneu.Parameters.AddWithValue("@marca", marca);
        //                    cmdPneu.Parameters.AddWithValue("@modelo", modelo);
        //                    cmdPneu.Parameters.AddWithValue("@medida", medidas);
        //                    cmdPneu.Parameters.AddWithValue("@datacompra", dataEmissao.ToString("yyyy-MM-dd"));
        //                    cmdPneu.Parameters.AddWithValue("@valor", string.IsNullOrEmpty(txtValor.Text) ? 0 : Convert.ToDecimal(txtValor.Text));
        //                    cmdPneu.Parameters.AddWithValue("@status", "Estoque");
        //                    cmdPneu.Parameters.AddWithValue("@resp_entrada", dataHoraAtual.ToString("dd/MM/yyyy HH:mm") + " - " + Session["UsuarioLogado"]);

        //                    cmdPneu.ExecuteNonQuery();
        //                }
        //            }



        //            long quantComprada = 0;
        //            // Tenta converter, se falhar usa 0 ou trata erro
        //            if (!long.TryParse(txtQuantidade.Text, out quantComprada))
        //            {
        //                // Opcional: mostrar mensagem de erro
        //                Mensagem("info", "Informe uma quantidade válida, para atualizar estoque.");
        //                txtQuantidade.Focus();
        //                return;
        //            }

        //            // Atualiza estoque
        //            SqlCommand cmd2 = new SqlCommand(
        //                "UPDATE tbestoque_pecas SET valor_unitario=@valor, estoque_peca = estoque_peca + @qtd WHERE id_peca = @id", conn, trans);

        //            cmd2.Parameters.AddWithValue("@qtd", quantComprada);
        //            cmd2.Parameters.AddWithValue("@id", txtIdPeca.Text);
        //            cmd2.Parameters.AddWithValue("@valor", string.IsNullOrEmpty(txtValor.Text) ? 0 : Convert.ToDecimal(txtValor.Text));

        //            cmd2.ExecuteNonQuery();

        //            trans.Commit();

        //            Response.Redirect("EntradaPecas.aspx");
        //        }
        //        catch (Exception ex)
        //        {
        //            // ❌ Rollback se qualquer erro
        //            try
        //            {
        //                trans.Rollback();
        //            }
        //            catch { /* se a transação já estiver concluída */ }

        //            //lblErro.Text = ex.Message;
        //            Mensagem("danger", "Erro na transação " + ex.Message);
        //            //trans.Rollback();
        //            //throw;
        //        }
        //        Response.Redirect("EntradaPecas.aspx");

        //    }

        //}
        private void CarregarMarcaPneu()
        {
            ddlMarca.Items.Clear();

            string sql = @"SELECT id, descricao
                   FROM tbmarca_de_pneu
                   ORDER BY descricao ASC"
            ;

            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();

                ddlMarca.DataSource = dr;
                ddlMarca.DataTextField = "descricao";
                ddlMarca.DataValueField = "id";
                ddlMarca.DataBind();
            }

            ddlMarca.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Selecione...", ""));
        }
    }
}