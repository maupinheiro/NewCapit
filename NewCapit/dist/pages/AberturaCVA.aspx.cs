using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

namespace NewCapit.dist.pages
{
    public partial class AberturaCVA : System.Web.UI.Page
    {
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
                    Response.Redirect("Login.aspx");
                }
                CarregarEstabelecimentosCVA();
            }

        }
        private void CarregarEstabelecimentosCVA()
        {
            string sql = @"
            SELECT 
                cod_estabelecimento,
                nom_estabelecimento
            FROM tbestabelecimentos
            WHERE fl_exclusao IS NULL
              AND sit_estabelecimento = 'ATIVO'
            ORDER BY nom_estabelecimento";

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                try
                {
                    conn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        ddlEstabelecimentoCVA.DataSource = dr;
                        ddlEstabelecimentoCVA.DataValueField = "cod_estabelecimento";
                        ddlEstabelecimentoCVA.DataTextField = "nom_estabelecimento";
                        ddlEstabelecimentoCVA.DataBind();
                    }

                    // Opção inicial
                    ddlEstabelecimentoCVA.Items.Insert(
                        0,
                        new ListItem("-- Selecione --", "")
                    );
                }
                catch (Exception ex)
                {
                    MostrarMensagem("Erro ao carregar os estabelecimentos: " + ex.Message);
                }
            }
        }

        protected void btnLimpar_Click(object sender, EventArgs e)
        {
            txtSolicitacao.Text = "";
            LimparTela();

            txtSolicitacao.Focus();
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            try
            {
                string solicitacao = txtSolicitacao.Text.Trim();

                if (string.IsNullOrWhiteSpace(solicitacao))
                {
                    MostrarMensagem("Informe o número da solicitação.");
                    return;
                }

                //LimparTela();

                CarregarSolicitacao(solicitacao);
            }
            catch (Exception ex)
            {
                MostrarMensagem("Erro ao pesquisar a solicitação: " + ex.Message);
            }

            try
            {
                string solicitacao = txtSolicitacao.Text.Trim();

                if (string.IsNullOrWhiteSpace(solicitacao))
                {
                    MostrarMensagem("Informe o número da solicitação.");
                    return;
                }

                CarregarProdutosSolicitacao(solicitacao);
            }
            catch (Exception ex)
            {
                MostrarMensagem(
                    "Erro ao pesquisar a solicitação: " +
                    ex.Message);
            }
            
        }       
        private void CarregarSolicitacao(string solicitacao)
        {
            string connectionString =
                ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // ============================================================
                // 1 - PESQUISA NA TBCARGAS
                // ============================================================

                string sqlCarga = @"
                SELECT
                    carga,
                    desc_material,
                    idviagem,
                    codmot,
                    frota
                FROM tbcargas
                WHERE carga = @carga";

                string codMot = "";
                string frota = "";

                using (SqlCommand cmd = new SqlCommand(sqlCarga, conn))
                {
                    cmd.Parameters.Add("@carga", SqlDbType.NVarChar, 11)
                                  .Value = solicitacao.Trim();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (!dr.Read())
                        {
                            MostrarMensagem(
                                "Solicitação não encontrada. Verifique o número digitado.");

                            return;
                        }

                        // ----------------------------------------------------
                        // Verifica se é realmente uma Solicitação
                        // ----------------------------------------------------

                        string descMaterial = dr["desc_material"] == DBNull.Value
                            ? ""
                            : dr["desc_material"].ToString().Trim();

                        if (!descMaterial.Equals(
                                "Solicitação",
                                StringComparison.OrdinalIgnoreCase))
                        {
                            MostrarMensagem(
                                "Solicitação não encontrada. Verifique o número digitado.");

                            return;
                        }

                        // ----------------------------------------------------
                        // Verifica programação
                        // ----------------------------------------------------

                        if (dr["idviagem"] == DBNull.Value ||
                            string.IsNullOrWhiteSpace(dr["idviagem"].ToString()))
                        {
                            MostrarMensagem("Solicitação não foi programada.");
                            return;
                        }

                        // ----------------------------------------------------
                        // Guarda os dados
                        // ----------------------------------------------------

                        codMot = dr["codmot"] == DBNull.Value
                            ? ""
                            : dr["codmot"].ToString().Trim();

                        frota = dr["frota"] == DBNull.Value
                            ? ""
                            : dr["frota"].ToString().Trim();
                    }
                }

                // ============================================================
                // IMPORTANTE:
                // O DataReader da TBCARGAS já foi fechado aqui.
                // ============================================================

                // ============================================================
                // 2 - DADOS DA SOLICITAÇÃO
                // ============================================================

                CarregarDadosSolicitacao(conn, solicitacao);

                // ============================================================
                // 3 - MOTORISTA
                // ============================================================

                if (!string.IsNullOrWhiteSpace(codMot))
                {
                    CarregarMotorista(conn, codMot);
                }

                // ============================================================
                // 4 - VEÍCULO
                // ============================================================

                if (!string.IsNullOrWhiteSpace(frota))
                {
                    CarregarVeiculo(conn, frota);
                }
            }
        }
        private void CarregarDadosSolicitacao(SqlConnection conn, string solicitacao)
        {
            string sql = @"
            SELECT
                r1_sol_numero,
                r1_sol_data_cadastro,
                r1_sol_data_hora_coleta,
                r1_sol_centro_custo,
                r1_sol_conta,
                r1_sol_tipo_geracao,
                r1_sol_tipo,
                r1_sol_tipo_veiculo,
                r1_sol_origem,
                r1_sol_destino
            FROM tbsolicitacoes_r1
            WHERE r1_sol_numero = @numero";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add("@numero", SqlDbType.VarChar, 50)
                              .Value = solicitacao;

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (!dr.Read())
                    {
                        MostrarMensagem(
                            "Solicitação encontrada na carga, mas não localizada em tbsolicitacoes_r1.");

                        return;
                    }

                    // ========================================================
                    // DATAS
                    // ========================================================

                    if (dr["r1_sol_data_cadastro"] != DBNull.Value)
                    {
                        DateTime data = Convert.ToDateTime(
                            dr["r1_sol_data_cadastro"]);

                        txtEmissao.Text =
                            data.ToString("dd/MM/yyyy HH:mm");
                    }

                    if (dr["r1_sol_data_hora_coleta"] != DBNull.Value)
                    {
                        DateTime data = Convert.ToDateTime(
                            dr["r1_sol_data_hora_coleta"]);

                        txtColeta.Text =
                            data.ToString("dd/MM/yyyy HH:mm");
                    }

                    // ========================================================
                    // CENTRO DE CUSTO
                    // ========================================================

                    txtCentroCusto.Text =
                        dr["r1_sol_centro_custo"] == DBNull.Value
                            ? ""
                            : dr["r1_sol_centro_custo"].ToString();

                    // ========================================================
                    // CONTA
                    // ========================================================

                    txtConta.Text =
                        dr["r1_sol_conta"] == DBNull.Value
                            ? ""
                            : dr["r1_sol_conta"].ToString();

                    // ========================================================
                    // CÓDIGOS
                    // ========================================================

                    string tipoGeracao =
                        dr["r1_sol_tipo_geracao"] == DBNull.Value
                            ? ""
                            : dr["r1_sol_tipo_geracao"].ToString().Trim();

                    string tipoSolicitacao =
                        dr["r1_sol_tipo"] == DBNull.Value
                            ? ""
                            : dr["r1_sol_tipo"].ToString().Trim();

                    string tipoVeiculo =
                        dr["r1_sol_tipo_veiculo"] == DBNull.Value
                            ? ""
                            : dr["r1_sol_tipo_veiculo"].ToString().Trim();

                    string origem =
                        dr["r1_sol_origem"] == DBNull.Value
                            ? ""
                            : dr["r1_sol_origem"].ToString().Trim();

                    string destino =
                        dr["r1_sol_destino"] == DBNull.Value
                            ? ""
                            : dr["r1_sol_destino"].ToString().Trim();

                    dr.Close();

                    // ========================================================
                    // TIPO DE GERAÇÃO
                    // ========================================================

                    if (!string.IsNullOrWhiteSpace(tipoGeracao))
                    {
                        CarregarTipoGeracao(
                            conn,
                            tipoGeracao);
                    }

                    // ========================================================
                    // TIPO DE SOLICITAÇÃO
                    // ========================================================

                    if (!string.IsNullOrWhiteSpace(tipoSolicitacao))
                    {
                        CarregarTipoSolicitacao(
                            conn,
                            tipoSolicitacao);
                    }

                    // ========================================================
                    // TIPO DE VEÍCULO
                    // ========================================================

                    if (!string.IsNullOrWhiteSpace(tipoVeiculo))
                    {
                        CarregarTipoVeiculo(
                            conn,
                            tipoVeiculo);
                    }

                    // ========================================================
                    // ORIGEM
                    // ========================================================

                    if (!string.IsNullOrWhiteSpace(origem))
                    {
                        CarregarOrigem(
                            conn,
                            origem);
                    }

                    // ========================================================
                    // DESTINO
                    // ========================================================

                    if (!string.IsNullOrWhiteSpace(destino))
                    {
                        CarregarDestino(
                            conn,
                            destino);
                    }
                }
            }
        }

        private void CarregarTipoGeracao(SqlConnection conn, string codigo)
        {
            string sql = @"
            SELECT
                codvw,
                descricao
            FROM tbtipogeracaosolicitacao
            WHERE codvw = @codigo";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add("@codigo", SqlDbType.VarChar, 50)
                              .Value = codigo;

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        txtCodTipoGeracao.Text =
                            dr["codvw"].ToString();

                        txtTipoGeracao.Text =
                            dr["descricao"].ToString();
                    }
                }
            }
        }

        private void CarregarTipoSolicitacao(SqlConnection conn, string codigo)
        {
            string sql = @"
            SELECT
                codvw,
                descricao
            FROM tbtiposolicitacao
            WHERE codvw = @codigo";
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add("@codigo", SqlDbType.VarChar, 50)
                              .Value = codigo;
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        txtTipoSolicitacao.Text =
                            dr["codvw"].ToString();
                        txtTipoSolicitacaoDescricao.Text =
                            dr["descricao"].ToString();
                    }
                }
            }
        }
       
        private void CarregarTipoVeiculo(SqlConnection conn,string codigo)
        {
            string sql = @"
            SELECT
                codvw,
                descricao
            FROM tbtipoveic
            WHERE codvw = @codigo";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add("@codigo", SqlDbType.NVarChar, 50)
                              .Value = codigo.Trim();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        txtTipoVeiculo.Text =
                            dr["codvw"].ToString();

                        txtTipoVeiculoDescricao.Text =
                            dr["descricao"].ToString();
                    }
                }
            }
        }

        private void CarregarOrigem(SqlConnection conn, string codigoOrigem)
        {
            // ============================================================
            // Busca origem
            // ============================================================

            ClienteDados origem = BuscarCliente(
                conn,
                codigoOrigem);

            if (origem == null)
                return;

            // ============================================================
            // ORIGEM = G471
            // ============================================================

            if (codigoOrigem.Equals(
                    "G471",
                    StringComparison.OrdinalIgnoreCase))
            {
                // --------------------------------------------------------
                // EXPEDIDOR = G471
                // --------------------------------------------------------

                PreencherExpedidor(origem);

                // --------------------------------------------------------
                // REMETENTE = 11
                // --------------------------------------------------------

                ClienteDados remetente = BuscarCliente(
                    conn,
                    "11");

                if (remetente != null)
                {
                    PreencherRemetente(remetente);
                }
            }
            else
            {
                // ========================================================
                // ORIGEM DIFERENTE DE G471
                // A própria origem será o REMETENTE
                // ========================================================

                PreencherRemetente(origem);
            }
        }

        private class ClienteDados
        {
            public string Codvw { get; set; }
            public string Razcli { get; set; }
            public string Cnpj { get; set; }
            public string Cidcli { get; set; }
            public string Estcli { get; set; }
        }

        private ClienteDados BuscarCliente(SqlConnection conn, string codigo)
        {
            string sql = @"
            SELECT
                codvw,
                razcli,
                cnpj,
                cidcli,
                estcli
            FROM tbclientes
            WHERE codvw = @codigo";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add("@codigo", SqlDbType.VarChar, 50)
                              .Value = codigo;

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (!dr.Read())
                        return null;

                    return new ClienteDados
                    {
                        Codvw = dr["codvw"]?.ToString(),
                        Razcli = dr["razcli"]?.ToString(),
                        Cnpj = dr["cnpj"]?.ToString(),
                        Cidcli = dr["cidcli"]?.ToString(),
                        Estcli = dr["estcli"]?.ToString()
                    };
                }
            }
        }

        
        private void PreencherExpedidor(ClienteDados cliente)
        {
            txtCodExpedidor.Text = cliente.Codvw;
            txtExpedidor.Text = cliente.Razcli;
            txtCNPJExpedidor.Text = cliente.Cnpj;
            txtCidExpedidor.Text = cliente.Cidcli;
            txtUfExpedidor.Text = cliente.Estcli;
        }

        private void PreencherRemetente(ClienteDados cliente)
        {
            txtCodRemetente.Text = cliente.Codvw;
            txtRemetente.Text = cliente.Razcli;
            txtCNPJRemetente.Text = cliente.Cnpj;
            txtCidRemetente.Text = cliente.Cidcli;
            txtUfRemetente.Text = cliente.Estcli;
        }

        
        private void CarregarDestino(SqlConnection conn, string codigoDestino)
        {
            ClienteDados cliente = BuscarCliente(
                conn,
                codigoDestino);

            if (cliente == null)
                return;

            // ============================================================
            // DESTINATÁRIO
            // ============================================================

            txtCodDestinatario.Text = cliente.Codvw;
            txtDestinatario.Text = cliente.Razcli;
            txtCNPJDestinatario.Text = cliente.Cnpj;
            txtCidDestinatario.Text = cliente.Cidcli;
            txtUfDestinatario.Text = cliente.Estcli;

            // ============================================================
            // RECEBEDOR
            // ============================================================

            txtCodRecebedor.Text = cliente.Codvw;
            txtRecebedor.Text = cliente.Razcli;
            txtCNPJRecebedor.Text = cliente.Cnpj;
            txtCidRecebedor.Text = cliente.Cidcli;
            txtUfRecebedor.Text = cliente.Estcli;
        }

        private void CarregarMotorista(SqlConnection conn,string codMot)
        {
            string sql = @"
            SELECT
                codmot,
                nommot,
                cpf,
                codtra,
                transp,
                numrg
            FROM tbmotoristas
            WHERE codmot = @codmot";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add("@codmot", SqlDbType.VarChar, 50)
                              .Value = codMot;

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        txtCodMot.Text =
                            dr["codmot"].ToString();

                        txtNome.Text =
                            dr["nommot"].ToString();

                        txtCPF.Text =
                            dr["cpf"].ToString();

                        txtRG.Text =
                            dr["numrg"].ToString();

                        txtTranspMotorista.Text =
                            dr["codtra"].ToString().Trim() + " - " + dr["transp"].ToString().Trim();
                    }
                }
            }
        }

        
        private void CarregarVeiculo(SqlConnection conn, string codVei)
        {
            string sql = @"
            SELECT
                codvei,
                tipvei,
                plavei,
                reboque1,
                reboque2,
                codtra,
                transp,
                pbt
            FROM tbveiculos
            WHERE codvei = @codvei";

            // Variáveis para guardar os dados
            string codVeiculo = "";
            string tipoVeiculo = "";
            string placa = "";
            string reboque1 = "";
            string reboque2 = "";
            string capacidade = "";
            string transpVeiculo = "";
            string codTransp = "";

            // ============================================================
            // 1 - CONSULTA O VEÍCULO
            // ============================================================

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add("@codvei", SqlDbType.VarChar, 50)
                              .Value = codVei;

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (!dr.Read())
                    {
                        return;
                    }

                    codVeiculo = dr["codvei"] == DBNull.Value
                        ? ""
                        : dr["codvei"].ToString().Trim();

                    tipoVeiculo = dr["tipvei"] == DBNull.Value
                        ? ""
                        : dr["tipvei"].ToString().Trim();

                    codTransp = dr["codtra"] == DBNull.Value
                        ? ""
                        : dr["codtra"].ToString().Trim();
                    transpVeiculo = dr["transp"] == DBNull.Value
                        ? ""
                        : dr["transp"].ToString().Trim();
                    placa = dr["plavei"] == DBNull.Value
                        ? ""
                        : dr["plavei"].ToString().Trim();

                    capacidade = dr["pbt"] == DBNull.Value
                        ? ""
                        : dr["pbt"].ToString().Trim();

                    reboque1 = dr["reboque1"] == DBNull.Value
                        ? ""
                        : dr["reboque1"].ToString().Trim();

                    reboque2 = dr["reboque2"] == DBNull.Value
                        ? ""
                        : dr["reboque2"].ToString().Trim();
                }
            }

            // ============================================================
            // IMPORTANTE:
            // O DataReader de tbveiculos já foi fechado aqui.
            // ============================================================

            // ============================================================
            // 2 - PREENCHIMENTO DO VEÍCULO
            // ============================================================

            txtCodVei.Text = codVeiculo;
            txtTipVei.Text = tipoVeiculo;
            txtPlaca.Text = placa;
            txtCapacidade.Text = capacidade;
            txtTranspVeiculo.Text = codTransp.Trim() + " - " + transpVeiculo.Trim();

            // ============================================================
            // 3 - REBOQUE 1
            // ============================================================

            if (!string.IsNullOrWhiteSpace(reboque1))
            {
                txtReboque1.Text = reboque1;

                CarregarCarreta(
                    conn,
                    reboque1,
                    txtReboque1);
            }
            else
            {
                txtReboque1.Text = "";
            }

            // ============================================================
            // 4 - REBOQUE 2
            // ============================================================

            if (!string.IsNullOrWhiteSpace(reboque2))
            {
                CarregarCarreta(
                    conn,
                    reboque2,
                    txtPlacaReboque2);
            }
            else
            {
                txtPlacaReboque2.Text = "";
            }
        }


        private void CarregarCarreta(SqlConnection conn, string placa,TextBox campoDestino)
        {
            string sql = @"
            SELECT
                placacarreta
            FROM tbcarretas
            WHERE placacarreta = @placa";

            using (SqlCommand cmdCarreta = new SqlCommand(sql, conn))
            {
                cmdCarreta.Parameters.Add("@placa", SqlDbType.NVarChar, 9)
                                    .Value = placa;

                using (SqlDataReader drCarreta = cmdCarreta.ExecuteReader())
                {
                    if (drCarreta.Read())
                    {
                        campoDestino.Text =
                            drCarreta["placacarreta"].ToString().Trim();
                    }
                    else
                    {
                        campoDestino.Text = "";
                    }
                }
            }
        }

        private void LimparTela()
        {
            txtEmissao.Text = "";
            txtColeta.Text = "";
            txtCentroCusto.Text = "";
            txtConta.Text = "";

            txtCodTipoGeracao.Text = "";
            txtTipoGeracao.Text = "";

            txtTipoSolicitacao.Text = "";
            txtTipoSolicitacaoDescricao.Text = "";

            txtTipoVeiculo.Text = "";
            txtTipoVeiculoDescricao.Text = "";

            txtCodExpedidor.Text = "";
            txtExpedidor.Text = "";
            txtCNPJExpedidor.Text = "";
            txtCidExpedidor.Text = "";
            txtUfExpedidor.Text = "";

            txtCodRemetente.Text = "";
            txtRemetente.Text = "";
            txtCNPJRemetente.Text = "";
            txtCidRemetente.Text = "";
            txtUfRemetente.Text = "";

            txtCodDestinatario.Text = "";
            txtDestinatario.Text = "";
            txtCNPJDestinatario.Text = "";
            txtCidDestinatario.Text = "";
            txtUfDestinatario.Text = "";

            txtCodRecebedor.Text = "";
            txtRecebedor.Text = "";
            txtCNPJRecebedor.Text = "";
            txtCidRecebedor.Text = "";
            txtUfRecebedor.Text = "";

            txtCodMot.Text = "";
            txtNome.Text = "";
            txtCPF.Text = "";
            txtRG.Text = "";

            txtCodVei.Text = "";
            txtTipVei.Text = "";
            txtPlaca.Text = "";
            txtCapacidade.Text = "";

            txtReboque1.Text = "";
            txtPlacaReboque2.Text = "";
        }
               
        private void MostrarMensagem(string mensagem)
        {
            ScriptManager.RegisterStartupScript(
                this,
                GetType(),
                "msg",
                "alert('" + mensagem.Replace("'", "\\'") + "');",
                true);
        }

        protected void btnGerarCVA_Click(object sender, EventArgs e)
        {
            try
            {
                // VALIDAR DATA DA ENTREGA
                DateTime dataEntrega;

                if (!DateTime.TryParseExact(
                        txtDataHoraEntregaCVA.Text.Trim(),
                        "dd/MM/yyyy HH:mm",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out dataEntrega))
                {
                    MostrarMensagem("Informe uma data de entrega válida.");
                    return;
                }


                // VALIDAR DATA DO RETORNO
                DateTime dataRetorno;

                if (!DateTime.TryParseExact(
                        txtDtRetorno.Text.Trim(),
                        "dd/MM/yyyy HH:mm",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out dataRetorno))
                {
                    MostrarMensagem("Informe uma data de retorno válida.");
                    return;
                }

                // A partir daqui você pode continuar o salvamento
                // dataEntrega contém a data já convertida para DateTime
                // dataRetorno contém a data já convertida para DateTime

                // Exemplo:
                // SalvarCVA(dataEntrega, dataRetorno);

            }
            catch (Exception ex)
            {
                MostrarMensagem("Erro ao salvar CVA: " + ex.Message);
            }
        }
        private void CarregarProdutosSolicitacao(string numeroSolicitacao)
        {
            string sql = @"
            SELECT
                r2_sol_codigo_produto,
                r2_sol_numero,
                r2_sol_quant_solicitada_produto
            FROM tbsolicitacoes_produtos
            WHERE r2_sol_numero = @numeroSolicitacao
            ORDER BY r2_sol_codigo_produto";

            using (SqlConnection conn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@numeroSolicitacao", numeroSolicitacao);

                try
                {
                    conn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(dr);

                        // Criar as colunas já formatadas
                        dt.Columns.Add("CodigoProduto", typeof(string));
                        dt.Columns.Add("NumeroSolicitacao", typeof(string));
                        dt.Columns.Add("Quantidade", typeof(string));

                        foreach (DataRow row in dt.Rows)
                        {
                            // Código do produto
                            row["CodigoProduto"] =
                                row["r2_sol_codigo_produto"].ToString().Trim();

                            // Número da solicitação com 1 zero à esquerda
                            row["NumeroSolicitacao"] =
                                "01" + row["r2_sol_numero"].ToString().Trim();

                            // Quantidade
                            decimal quantidade;

                            if (decimal.TryParse(
                                row["r2_sol_quant_solicitada_produto"].ToString(),
                                out quantidade))
                            {
                                string valor = quantidade.ToString("0.00");

                                // Completar com zeros à esquerda até 10 caracteres
                                row["Quantidade"] =
                                    valor.PadLeft(10, '0');
                            }
                            else
                            {
                                row["Quantidade"] = "0000000000";
                            }
                        }

                        gvProdutosSolicitacao.DataSource = dt;
                        gvProdutosSolicitacao.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    MostrarMensagem(
                        "Erro ao carregar os produtos da solicitação: " +
                        ex.Message);
                }
            }
        }
        
    }
}