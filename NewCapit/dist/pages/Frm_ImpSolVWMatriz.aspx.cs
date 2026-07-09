using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using DocumentFormat.OpenXml.Vml;
using DocumentFormat.OpenXml.Wordprocessing;
using NPOI.SS.Formula.Functions;
using Org.BouncyCastle.Asn1.Cmp;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Text;
using Path = System.IO.Path;
using static NPOI.HSSF.Util.HSSFColor;

namespace NewCapit.dist.pages
{

    public partial class Frm_ImpSolVWMatriz : System.Web.UI.Page
    {
        private static int _total = 0;
        private static int _atual = 0;
        private static bool _concluido = false;
        SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString());
        DateTime dataHoraAtual = DateTime.Now;
        private static string numSolic;
        private static string lblTipoGeracao;
        private static string lblTipoSolicitacao;
        private static string lblPlanta;
        private static string lblOrigem;
        private static string lblDestino;
        private static string lblCadastro;
        private static string lblHoraCadastro;
        private static string lblColeta;
        private static string lblHora;
        private static string lblTipoVeiculo;
        private static decimal pesoTotal;
        private static string codPlanta;
        private static string grPlanta;
        private static string nomePlanta;
        private static string codCliOrigem;
        private static string razCliOrigem;
        private static string cnpjCliOrigem;
        private static string cidCliOrigem;
        private static string estCliOrigem;

        private static string codPlantaPagadora;
        private static string razPlantaPagadora;
        private static string cnpjPlantaPagadora;
        private static string cidPlantaPagadora;
        private static string estPlantaPagadora;

        private static string codCliDestino;
        private static string razCliDestino;
        private static string cnpjCliDestino;
        private static string cidCliDestino;
        private static string estCliDestino;
        private static string codigoTipoGeracao;
        private static string codvwTipoGeracao;
        private static string descricaoTipoGeracao;
        private static string codigoTipoSolicitacao;
        private static string codvwTipoSolicitacao;
        private static string descricaoTipoSolicitacao;
        private static string codigoTipoVeiculo;
        private static string empresaTNG;
        private static string codvwTipoVeiculo;
        private static string descricaoTipoVeiculo;
        private static string codCliExpedidor;
        private static string razCliExpedidor;
        private static string cnpjCliExpedidor;
        private static string cidCliExpedidor;
        private static string estCliExpedidor;
        private static string codCliRecebedor;
        private static string razCliRecebedor;
        private static string cnpjCliRecebedor;
        private static string cidCliRecebedor;
        private static string estCliRecebedor;
        private static decimal sPeso;
        private static string usuario;
        private static string descVeicVW;
        private static string contaDebito;
        private static string centroCusto;
        private static string sDuracao = "00:00";
        private static string sEmitePedagio = "NAO";
        private static string sDeslocamento = "NENHUM";
        private static string sDistancia = "0";
        private static string codCliPagador;
        private static string razCliPagador;
        private static string cnpjCliPagador;
        private static string cidCliPagador;
        private static string estCliPagador;
        private static string codSapiens;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UsuarioLogado"] != null)
                {
                    string nomeUsuario = Session["UsuarioLogado"].ToString();
                    var lblUsuario = nomeUsuario;
                    txtUsuario.Text = dataHoraAtual.ToString("dd/MM/yyyy HH:mm") + " - " + lblUsuario.ToUpper();
                    usuario = nomeUsuario;
                }
                else
                {
                    var lblUsuario = "<Usuário>";
                    Response.Redirect("Login.aspx");
                }


            }

        }
        //protected void btnImportar_Click(object sender, EventArgs e)
        //{
        //    string pastaDownloads = System.IO.Path.Combine(
        //        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
        //        "Downloads");

        //    string[] arquivos = Directory.GetFiles(pastaDownloads, "SG*.txt");

        //    if (arquivos.Length == 0)
        //    {
        //        lblStatus.Text = "Nenhum arquivo encontrado.";
        //        return;
        //    }




        //    using (SqlConnection conn = new SqlConnection(
        //        WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
        //    {
        //        conn.Open();

        //        int atual = 0;

        //        foreach (string arquivo in arquivos)
        //        {
        //            ProcessarArquivo(conn, arquivo);
        //            atual++;
        //            File.Delete(arquivo);
        //            AtualizarBarra(atual, arquivos.Length);

        //        }


        //    }

        //    lblStatus.Text = $"✅ Importação finalizada com sucesso! {arquivos.Length} arquivo(s) importado(s).";

        //}
        [WebMethod]
        public static void IniciarImportacao()
        {
            _concluido = false;
            _atual = 0;

            string pasta = HttpContext.Current.Server.MapPath("../../ImportacaoTemp/");

            string[] arquivos = Directory.GetFiles(pasta, "SG*.txt");
            _total = arquivos.Length;

            Task.Run(() =>
            {
                using (SqlConnection conn = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
                {
                    conn.Open();

                    foreach (string arquivo in arquivos)
                    {
                        // sua função real
                        ProcessarArquivo(conn, arquivo);

                        File.Delete(arquivo);
                        _atual++;
                    }
                }

                _concluido = true;
            });
        }
        [WebMethod]
        public static object Progresso()
        {
            int percentual = _total == 0 ? 0 : (_atual * 100 / _total);

            return new
            {
                atual = _atual,
                total = _total,
                percentual = percentual,
                concluido = _concluido
            };
        }

        private static void ProcessarArquivo(SqlConnection conn, string arquivo)
        {
            string[] linhas = File.ReadAllLines(arquivo);

            string numSolic = "";   
            string planta = "";

            int seqPedido = 0;
            decimal pesoTotal = 0;

            foreach (string linha in linhas)
            {
                //string tipo = linha.Substring(0, 2);
                string tipo = SubStringSeguro(linha, 0, 2);
                // =========================
                // TIPO 01 – CARGA
                // =========================
                if (tipo == "01")
                {
                    seqPedido = 0;
                    pesoTotal = 0;

                    numSolic = linha.Substring(2, 10);
                    lblTipoGeracao = linha.Substring(12, 4);
                    lblTipoSolicitacao = linha.Substring(16, 7);
                    lblPlanta = linha.Substring(23, 2);
                    lblOrigem = linha.Substring(25, 4);
                    lblDestino = linha.Substring(29, 4);
                    lblCadastro = linha.Substring(33, 10);
                    lblHoraCadastro = linha.Substring(43, 8);
                    lblColeta = linha.Substring(51, 10);
                    lblHora = linha.Substring(61, 8);
                    string r1_sol_responsavel = linha.Substring(69, 1);
                    lblTipoVeiculo = linha.Substring(70, 4);
                    string r1_sol_transportadora = linha.Substring(74, 4);
                    contaDebito = linha.Substring(78, 8);
                    centroCusto = linha.Substring(86, 4);
                    string r1_sol_fornec_responsavel = linha.Substring(90, 4);
                    string r1_sol_motivo = linha.Substring(94, 3);
                    
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
                    {
                        // Pesquisar dados da planta
                        using (SqlCommand cmd = new SqlCommand(@"
                         SELECT codigo, descricao, gerenciadora
                         FROM tbPlantavw
                         WHERE codigo = @codigo", conn))
                        {
                            cmd.Parameters.AddWithValue("@codigo", lblPlanta);

                            // conn.Open();

                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                if (dr.Read())
                                {
                                    codPlanta = dr["codigo"].ToString();
                                    nomePlanta = dr["descricao"].ToString();
                                    grPlanta = dr["gerenciadora"].ToString();
                                }
                                else
                                {
                                    // Não encontrou
                                    codPlanta = "";
                                    nomePlanta = "";
                                    grPlanta = "";
                                    // Opcional
                                    // lblMsg.Text = "Cliente não encontrado";
                                }
                            }
                            //conn.Close();
                        }

                        // Pesquisar dados da planta pagadora
                        using (SqlCommand cmd = new SqlCommand(@"
                         SELECT codcli, razcli, cidcli, estcli, cnpj
                         FROM tbclientes
                         WHERE codvw = @codigo", conn))
                        {
                            cmd.Parameters.AddWithValue("@codigo", lblPlanta);

                            // conn.Open();

                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                if (dr.Read())
                                {
                                    codPlantaPagadora = dr["codcli"].ToString();
                                    razPlantaPagadora = dr["razcli"].ToString();
                                    cnpjPlantaPagadora = dr["cnpj"].ToString();
                                    cidPlantaPagadora = dr["cidcli"].ToString();
                                    estPlantaPagadora = dr["estcli"].ToString();
                                }
                                else
                                {
                                    // Não encontrou
                                    codPlantaPagadora = "";
                                    razPlantaPagadora = "";
                                    cnpjPlantaPagadora = "";
                                    cidPlantaPagadora = "";
                                    estPlantaPagadora = "";
                                }
                            }
                            //conn.Close();
                        }

                        // Pesquisar dados do cliente origem
                        using (SqlCommand cmd = new SqlCommand(@"
                         SELECT codcli, razcli, cidcli, estcli, cnpj
                         FROM tbclientes
                         WHERE codvw = @codvw", conn))
                        {
                            cmd.Parameters.AddWithValue("@codvw", lblOrigem);

                            //conn.Open();

                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                if (dr.Read())
                                {
                                    codCliOrigem = dr["codcli"].ToString();
                                    razCliOrigem = dr["razcli"].ToString();
                                    cnpjCliOrigem = dr["cnpj"].ToString();
                                    cidCliOrigem = dr["cidcli"].ToString();
                                    estCliOrigem = dr["estcli"].ToString();
                                }
                                else
                                {
                                    // Não encontrou
                                    codCliOrigem = "";
                                    razCliOrigem = "";
                                    cnpjCliOrigem = "";
                                    cidCliOrigem = "";
                                    estCliOrigem = "";

                                    // Opcional
                                    // lblMsg.Text = "Cliente não encontrado";
                                }
                            }
                            // conn.Close();
                        }

                        // Pesquisar dados do cliente destino
                        using (SqlCommand cmd = new SqlCommand(@"
                         SELECT codcli, razcli, cidcli, estcli, cnpj, codsapiens
                         FROM tbclientes
                         WHERE codvw = @codvw", conn))
                        {
                            cmd.Parameters.AddWithValue("@codvw", lblDestino);

                            //conn.Open();

                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                if (dr.Read())
                                {
                                    codCliDestino = dr["codcli"].ToString();
                                    razCliDestino = dr["razcli"].ToString();
                                    cnpjCliDestino = dr["cnpj"].ToString();
                                    cidCliDestino = dr["cidcli"].ToString();
                                    estCliDestino = dr["estcli"].ToString();
                                    codSapiens = dr["codsapiens"].ToString();
                                }
                                else
                                {
                                    // Não encontrou
                                    codCliDestino = "";
                                    razCliDestino = "";
                                    cnpjCliDestino = "";
                                    cidCliDestino = "";
                                    estCliDestino = "";

                                    // Opcional
                                    // lblMsg.Text = "Cliente não encontrado";
                                }
                            }
                            // conn.Close();
                        }

                        // Pesquisar dados do tipo de geracao da solicitcao
                        using (SqlCommand cmd = new SqlCommand(@"
                         SELECT codigo, codvw, descricao
                         FROM tbtipogeracaosolicitacao
                         WHERE codvw = @codvw", conn))
                        {
                            cmd.Parameters.AddWithValue("@codvw", lblTipoGeracao);

                            // conn.Open();

                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                if (dr.Read())
                                {
                                    codigoTipoGeracao = dr["codigo"].ToString();
                                    codvwTipoGeracao = dr["codvw"].ToString();
                                    descricaoTipoGeracao = dr["descricao"].ToString().Trim();
                                }
                                else
                                {
                                    // Não encontrou
                                    codigoTipoGeracao = "";
                                    codvwTipoGeracao = "";
                                    descricaoTipoGeracao = "";

                                    // Opcional
                                    // lblMsg.Text = "Cliente não encontrado";
                                }
                            }
                            // conn.Close();
                        }

                        // Pesquisar dados do tipo de solicitcao
                        using (SqlCommand cmd = new SqlCommand(@"
                         SELECT codigo, codvw, descricao
                         FROM tbtiposolicitacao
                         WHERE codvw = @codvw", conn))
                        {
                            cmd.Parameters.AddWithValue("@codvw", lblTipoSolicitacao);

                            // conn.Open();

                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                if (dr.Read())
                                {
                                    codigoTipoSolicitacao = dr["codigo"].ToString();
                                    codvwTipoSolicitacao = dr["codvw"].ToString();
                                    descricaoTipoSolicitacao = dr["descricao"].ToString();
                                }
                                else
                                {
                                    // Não encontrou
                                    codigoTipoSolicitacao = "";
                                    codvwTipoSolicitacao = "";
                                    descricaoTipoSolicitacao = "";

                                    // Opcional
                                    // lblMsg.Text = "Cliente não encontrado";
                                }
                            }


                        }
                        // Pesquisar dados do tipo de veiculos na tabela tbtipoveic
                        using (SqlCommand cmd = new SqlCommand(@"
                         SELECT codigo, codvw, descricao, descricao_tng, empresa, status
                         FROM tbtipoveic
                         WHERE codvw = @codvw", conn))
                        {
                            cmd.Parameters.AddWithValue("@codvw", lblTipoVeiculo);

                            // conn.Open();

                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                if (dr.Read())
                                {
                                    codigoTipoVeiculo = dr["codigo"].ToString();
                                    codvwTipoVeiculo = dr["codvw"].ToString();
                                    descricaoTipoVeiculo = dr["descricao"].ToString();
                                    descVeicVW = dr["descricao_tng"].ToString();
                                    empresaTNG = dr["empresa"].ToString();
                                    
                                }
                                else
                                {
                                    // Não encontrou
                                    codigoTipoVeiculo = "";
                                    codvwTipoVeiculo = "";
                                    descricaoTipoVeiculo = "";
                                    descVeicVW = "";

                                    // Opcional
                                    // lblMsg.Text = "Cliente não encontrado";
                                }
                            }
                        }
                    }

                    // Tipo de Solicitação = 18 - Distribuição de Aço
                    if (lblTipoSolicitacao == "18")
                    {
                        codCliExpedidor = codCliOrigem;
                        razCliExpedidor = razCliOrigem;
                        cidCliExpedidor = cidCliOrigem;
                        cnpjCliExpedidor = cnpjCliOrigem;
                        estCliExpedidor = estCliOrigem;

                        codCliOrigem = codPlantaPagadora;
                        razCliOrigem = razPlantaPagadora;
                        cidCliOrigem = codPlantaPagadora;
                        estCliOrigem = estPlantaPagadora;
                        cnpjCliOrigem = cnpjPlantaPagadora;

                        codCliRecebedor = codCliDestino;
                        razCliRecebedor = razCliDestino;
                        cnpjCliRecebedor = cnpjCliDestino;
                        cidCliRecebedor = cidCliDestino;
                        estCliRecebedor = estCliDestino;

                    }
                    else
                    {

                        codCliExpedidor = codCliOrigem;
                        razCliExpedidor = razCliOrigem;
                        cnpjCliExpedidor = cnpjCliOrigem;
                        cidCliExpedidor = cidCliOrigem;
                        estCliExpedidor = estCliOrigem;

                        codCliRecebedor = codCliDestino;
                        razCliRecebedor = razCliDestino;
                        cnpjCliRecebedor = cnpjCliDestino;
                        cidCliRecebedor = cidCliDestino;
                        estCliRecebedor = estCliDestino;
                    }

                    //SALVANDO NA TABELA tbsolicitacoes_r1  
                    using (SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
                    {

                        string sql = @"
                        IF EXISTS
                        (
                            SELECT 1
                            FROM tbsolicitacoes_r1
                            WHERE r1_sol_numero = @numero
                        )
                        BEGIN
                            UPDATE tbsolicitacoes_r1
                               SET
                                    r1_sol_tipo_registro = @tipoRegistro,
                                    r1_sol_tipo_geracao = @tipoGeracao,
                                    r1_sol_tipo = @tipo,
                                    r1_sol_planta = @planta,
                                    r1_sol_origem = @origem,
                                    r1_sol_destino = @destino,
                                    r1_sol_data_cadastro = @dataCadastro,
                                    r1_sol_data_hora_coleta = @dataHoraColeta,
                                    r1_sol_responsavel = @responsavel,
                                    r1_sol_tipo_veiculo = @tipoVeiculo,
                                    r1_sol_transportadora = @transportadora,
                                    r1_sol_conta = @conta,
                                    r1_sol_centro_custo = @centroCusto,
                                    r1_sol_fornec_responsavel = @fornecResponsavel,
                                    r1_sol_motivo = @motivo,
                                    r1_sol_material = @r1_sol_material,
                                    r1_sol_desc_material = @r1_sol_desc_material,
                                    r1_sol_empresa = @r1_sol_empresa,
                                    r1_sol_cod_planta_pagadora = @r1_sol_cod_planta_pagadora,
                                    r1_sol_planta_pagadora = @r1_sol_planta_pagadora,
                                    r1_sol_cnpj_planta_pagadora = @r1_sol_cnpj_planta_pagadora,
                                    r1_sol_cid_planta_pagadora = @r1_sol_cid_planta_pagadora,
                                    r1_sol_uf_planta_pagadora = @r1_sol_uf_planta_pagadora
                             WHERE r1_sol_numero = @numero

                        END
                        ELSE
                        BEGIN

                        INSERT INTO tbsolicitacoes_r1
                        (
                            r1_sol_tipo_registro,
                            r1_sol_numero,
                            r1_sol_tipo_geracao,
                            r1_sol_tipo,
                            r1_sol_planta,
                            r1_sol_origem,
                            r1_sol_destino,
                            r1_sol_data_cadastro,
                            r1_sol_data_hora_coleta,
                            r1_sol_responsavel,
                            r1_sol_tipo_veiculo,
                            r1_sol_transportadora,
                            r1_sol_conta,
                            r1_sol_centro_custo,
                            r1_sol_fornec_responsavel,
                            r1_sol_motivo,
                            r1_sol_material,
                            r1_sol_desc_material,
                            r1_sol_empresa,
                            r1_sol_cod_planta_pagadora,
                            r1_sol_planta_pagadora,
                            r1_sol_cnpj_planta_pagadora,
                            r1_sol_cid_planta_pagadora,
                            r1_sol_uf_planta_pagadora
                        )
                        VALUES
                        (
                            @tipoRegistro,
                            @numero,
                            @tipoGeracao,
                            @tipo,
                            @planta,
                            @origem,
                            @destino,
                            @dataCadastro,
                            @dataHoraColeta,
                            @responsavel,
                            @tipoVeiculo,
                            @transportadora,
                            @conta,
                            @centroCusto,
                            @fornecResponsavel,
                            @motivo,
                            @r1_sol_material,
                            @r1_sol_desc_material,
                            @r1_sol_empresa,
                            @r1_sol_cod_planta_pagadora,
                            @r1_sol_planta_pagadora,
                            @r1_sol_cnpj_planta_pagadora,
                            @r1_sol_cid_planta_pagadora,
                            @r1_sol_uf_planta_pagadora
                        )
                    END";
                        con.Open();
                        SqlCommand cmd = new SqlCommand(sql, con);
                        cmd.Parameters.AddWithValue("@tipoRegistro", tipo.Trim());
                        cmd.Parameters.AddWithValue("@numero", numSolic.Trim());
                        cmd.Parameters.AddWithValue("@tipoGeracao", lblTipoGeracao.Trim());
                        cmd.Parameters.AddWithValue("@tipo", lblTipoSolicitacao.Trim());
                        cmd.Parameters.AddWithValue("@planta", lblPlanta.Trim());
                        cmd.Parameters.AddWithValue("@origem", lblOrigem.Trim());
                        cmd.Parameters.AddWithValue("@destino", lblDestino.Trim());
                        cmd.Parameters.AddWithValue("@dataCadastro", SqlDbType.DateTime).Value = SafeDateTimeValue(lblCadastro + " " + lblHoraCadastro);
                        cmd.Parameters.AddWithValue("@dataHoraColeta", SqlDbType.DateTime).Value = SafeDateTimeValue(lblColeta + " " + lblHora);
                        cmd.Parameters.AddWithValue("@responsavel", r1_sol_responsavel.Trim());
                        cmd.Parameters.AddWithValue("@tipoVeiculo", lblTipoVeiculo.Trim());
                        cmd.Parameters.AddWithValue("@transportadora", r1_sol_transportadora.Trim());
                        cmd.Parameters.AddWithValue("@conta", contaDebito.Trim());
                        cmd.Parameters.AddWithValue("@centroCusto", centroCusto.Trim());
                        cmd.Parameters.AddWithValue("@fornecResponsavel", r1_sol_fornec_responsavel.Trim());
                        cmd.Parameters.AddWithValue("@motivo", r1_sol_motivo.Trim());
                        cmd.Parameters.AddWithValue("@r1_sol_material", "OUTROS");
                        cmd.Parameters.AddWithValue("@r1_sol_desc_material", "Solicitação");
                        cmd.Parameters.AddWithValue("@r1_sol_empresa", empresaTNG);
                        cmd.Parameters.AddWithValue("@r1_sol_cod_planta_pagadora", codPlantaPagadora);
                        cmd.Parameters.AddWithValue("@r1_sol_planta_pagadora", razPlantaPagadora);
                        cmd.Parameters.AddWithValue("@r1_sol_cnpj_planta_pagadora", cnpjPlantaPagadora);
                        cmd.Parameters.AddWithValue("@r1_sol_cid_planta_pagadora", cidPlantaPagadora);
;                        cmd.Parameters.AddWithValue("@r1_sol_uf_planta_pagadora", estPlantaPagadora);

                        cmd.ExecuteNonQuery();
                    }

                }

                // ===========================================
                // TIPO 02 – PEDIDO - produtos na solicitacao
                // ===========================================
                if (tipo == "02")
                {
                    seqPedido++;
                    string r2_sol_tipo_registro = linha.Substring(0, 2);
                    string r2_sol_codigo_produto = linha.Substring(2, 23);   
                    decimal r2_sol_quant_solicitada_produto = ConverterDecimal(linha.Substring(25, 10));
                    string r2_sol_embalagem_produto = linha.Substring(35, 8).Trim();
                    decimal r2_sol_altura_embalagem = ConverterDecimal(linha.Substring(43, 10));
                    decimal r2_sol_largura_embalagem = ConverterDecimal(linha.Substring(53, 10));
                    decimal r2_sol_comprimento_embalagem = ConverterDecimal(linha.Substring(63, 10));
                    decimal r2_sol_peso_embalagem = ConverterDecimal(linha.Substring(73, 10));
                    decimal r2_sol_quant_prod_embalagem = ConverterDecimal(linha.Substring(83, 10));
                    decimal r2_sol_peso_produto = ConverterDecimal(linha.Substring(93, 10));
                    string r2_sol_empilhamento_maximo = linha.Substring(103, 5).Trim();
                    string r2_sol_palet = linha.Substring(108, 8).Trim();
                    decimal r2_sol_altura_palet = ConverterDecimal(linha.Substring(116, 10));
                    decimal r2_sol_largura_palet = ConverterDecimal(linha.Substring(126, 10));
                    decimal r2_sol_comprimento_palet = ConverterDecimal(linha.Substring(136, 10));
                    decimal r2_sol_peso_palet = ConverterDecimal(linha.Substring(146, 10));
                    decimal r2_sol_quant_embal_por_camada = ConverterDecimal(linha.Substring(156, 10));

                    string r2_sol_cdg_origem_destino = linha.Substring(166, 4);
                    string r2_sol_hora_coleta_entrada = linha.Substring(170, 5);
                                        
                    decimal peso = decimal.Parse(
                        linha.Substring(25, 10).Trim(),
                        CultureInfo.InvariantCulture);

                    pesoTotal += peso;

                    string pedido = $"{numSolic}{seqPedido:D2}";

                    string sqlPedido = @"
                    INSERT INTO tbpedidos
                    (
                    pedido, carga, emissao, status, solicitante, entrega, peso, material, portao, situacao, previsao, codorigem, cliorigem, coddestino, clidestino, observacao, idviagem, andamento, ufcliorigem, ufclidestino, placas, saida, chegada, tomador, cidorigem, ciddestino, gr, cadastro, atualizacao, motcar, iniciocar, termcar, duracao, controledocliente
                    )
                    VALUES
                    (
                    @pedido, @carga, @emissao, @status, @solicitante, @entrega, @peso, @material, @portao, @situacao, @previsao, @codorigem, @cliorigem, @coddestino, @clidestino, @observacao, @idviagem, @andamento, @ufcliorigem, @ufclidestino,@placas, @saida, @chegada, @tomador, @cidorigem, @ciddestino, @gr, @cadastro, @atualizacao, @motcar, @iniciocar, @termcar, @duracao, @controledocliente
                    )";

                    if (!PedidoJaExiste(conn, pedido))
                    {

                        using (SqlCommand cmd = new SqlCommand(sqlPedido, conn))
                        {
                            cmd.Parameters.AddWithValue("@pedido", pedido);
                            cmd.Parameters.AddWithValue("@carga", numSolic);
                            cmd.Parameters.AddWithValue("@emissao", SafeDateTimeValue(lblCadastro + " " + lblHoraCadastro));
                            cmd.Parameters.AddWithValue("@status", "Pendente");
                            cmd.Parameters.AddWithValue("@solicitante", lblPlanta);
                            cmd.Parameters.AddWithValue("@entrega", "Normal");
                            cmd.Parameters.AddWithValue("@peso", peso);
                            cmd.Parameters.AddWithValue("@material", "Solicitação");
                            cmd.Parameters.AddWithValue("@portao", codCliOrigem);
                            cmd.Parameters.AddWithValue("@situacao", "Pronto");
                            cmd.Parameters.AddWithValue("@previsao", SafeDateValue(lblColeta));

                            // Campos sem dados no TXT
                            cmd.Parameters.AddWithValue("@codorigem", codCliOrigem);
                            cmd.Parameters.AddWithValue("@cliorigem", razCliOrigem);
                            cmd.Parameters.AddWithValue("@coddestino", codCliDestino);
                            cmd.Parameters.AddWithValue("@clidestino", razCliDestino);
                            cmd.Parameters.AddWithValue("@observacao", DBNull.Value);
                            cmd.Parameters.AddWithValue("@idviagem", DBNull.Value);
                            cmd.Parameters.AddWithValue("@andamento", "Pendente");
                            cmd.Parameters.AddWithValue("@ufcliorigem", estCliOrigem);
                            cmd.Parameters.AddWithValue("@ufclidestino", estCliDestino);
                            cmd.Parameters.AddWithValue("@placas", DBNull.Value);
                            cmd.Parameters.AddWithValue("@saida", DBNull.Value);
                            cmd.Parameters.AddWithValue("@chegada", DBNull.Value);
                            cmd.Parameters.AddWithValue("@tomador", planta);
                            cmd.Parameters.AddWithValue("@cidorigem", cidCliOrigem);
                            cmd.Parameters.AddWithValue("@ciddestino", cidCliDestino);
                            cmd.Parameters.AddWithValue("@gr", grPlanta);
                            cmd.Parameters.AddWithValue("@cadastro", usuario.ToUpper());
                            cmd.Parameters.AddWithValue("@atualizacao", DBNull.Value);
                            cmd.Parameters.AddWithValue("@motcar", DBNull.Value);
                            cmd.Parameters.AddWithValue("@iniciocar", DBNull.Value);
                            cmd.Parameters.AddWithValue("@termcar", DBNull.Value);
                            cmd.Parameters.AddWithValue("@duracao", DBNull.Value);
                            cmd.Parameters.AddWithValue("@controledocliente", DBNull.Value);

                            cmd.ExecuteNonQuery();
                        }

                    }

                    // SALVANDO OS PRODUTOS DA SOLICITACAO
                    string sql = @"

                    IF EXISTS
                    (
                        SELECT 1
                          FROM tbsolicitacoes_produtos
                         WHERE r2_sol_numero = @numero
                           AND r2_sol_codigo_produto = @codigoProduto
                    )
                    BEGIN

                        UPDATE tbsolicitacoes_produtos
                           SET
                                r2_sol_tipo_registro = @tipoRegistro,
                                r2_sol_quant_solicitada_produto = @quantSolicitada,
                                r2_sol_embalagem_produto = @embalagem,
                                r2_sol_altura_embalagem = @alturaEmb,
                                r2_sol_largura_embalagem = @larguraEmb,
                                r2_sol_comprimento_embalagem = @comprimentoEmb,
                                r2_sol_peso_embalagem = @pesoEmb,
                                r2_sol_quant_prod_embalagem = @quantProdEmb,
                                r2_sol_peso_produto = @pesoProduto,
                                r2_sol_empilhamento_maximo = @empilhamento,
                                r2_sol_palet = @palet,
                                r2_sol_altura_palet = @alturaPalet,
                                r2_sol_largura_palet = @larguraPalet,
                                r2_sol_comprimento_palet = @comprimentoPalet,
                                r2_sol_peso_palet = @pesoPalet,
                                r2_sol_quant_embal_por_camada = @quantCamada,
                                r2_sol_cdg_origem_destino = @origemDestino,
                                r2_sol_hora_coleta_entrada = @horaEntrada
                         WHERE r2_sol_numero = @numero
                           AND r2_sol_codigo_produto = @codigoProduto

                    END
                    ELSE
                    BEGIN

                        INSERT INTO tbsolicitacoes_produtos
                        (
                            r2_sol_tipo_registro,
                            r2_sol_numero,
                            r2_sol_codigo_produto,
                            r2_sol_quant_solicitada_produto,
                            r2_sol_embalagem_produto,
                            r2_sol_altura_embalagem,
                            r2_sol_largura_embalagem,
                            r2_sol_comprimento_embalagem,
                            r2_sol_peso_embalagem,
                            r2_sol_quant_prod_embalagem,
                            r2_sol_peso_produto,
                            r2_sol_empilhamento_maximo,
                            r2_sol_palet,
                            r2_sol_altura_palet,
                            r2_sol_largura_palet,
                            r2_sol_comprimento_palet,
                            r2_sol_peso_palet,
                            r2_sol_quant_embal_por_camada,
                            r2_sol_cdg_origem_destino,
                            r2_sol_hora_coleta_entrada
                        )
                        VALUES
                        (
                            @tipoRegistro,
                            @numero,
                            @codigoProduto,
                            @quantSolicitada,
                            @embalagem,
                            @alturaEmb,
                            @larguraEmb,
                            @comprimentoEmb,
                            @pesoEmb,
                            @quantProdEmb,
                            @pesoProduto,
                            @empilhamento,
                            @palet,
                            @alturaPalet,
                            @larguraPalet,
                            @comprimentoPalet,
                            @pesoPalet,
                            @quantCamada,
                            @origemDestino,
                            @horaEntrada
                        )

                    END";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@tipoRegistro", r2_sol_tipo_registro);
                        cmd.Parameters.AddWithValue("@numero", numSolic);
                        cmd.Parameters.AddWithValue("@codigoProduto", r2_sol_codigo_produto);
                        cmd.Parameters.AddWithValue("@quantSolicitada", r2_sol_quant_solicitada_produto);
                        cmd.Parameters.AddWithValue("@embalagem", r2_sol_embalagem_produto);
                        cmd.Parameters.AddWithValue("@alturaEmb", r2_sol_altura_embalagem);
                        cmd.Parameters.AddWithValue("@larguraEmb", r2_sol_largura_embalagem);
                        cmd.Parameters.AddWithValue("@comprimentoEmb", r2_sol_comprimento_embalagem);
                        cmd.Parameters.AddWithValue("@pesoEmb", r2_sol_peso_embalagem);
                        cmd.Parameters.AddWithValue("@quantProdEmb", r2_sol_quant_prod_embalagem);
                        cmd.Parameters.AddWithValue("@pesoProduto", r2_sol_peso_produto);
                        cmd.Parameters.AddWithValue("@empilhamento", r2_sol_empilhamento_maximo);
                        cmd.Parameters.AddWithValue("@palet", r2_sol_palet);
                        cmd.Parameters.AddWithValue("@alturaPalet", r2_sol_altura_palet);
                        cmd.Parameters.AddWithValue("@larguraPalet", r2_sol_largura_palet);
                        cmd.Parameters.AddWithValue("@comprimentoPalet", r2_sol_comprimento_palet);
                        cmd.Parameters.AddWithValue("@pesoPalet", r2_sol_peso_palet);
                        cmd.Parameters.AddWithValue("@quantCamada", r2_sol_quant_embal_por_camada);
                        cmd.Parameters.AddWithValue("@origemDestino", r2_sol_cdg_origem_destino);
                        cmd.Parameters.AddWithValue("@horaEntrada", (object)r2_sol_hora_coleta_entrada ?? DBNull.Value);

                        cmd.ExecuteNonQuery();
                    }

                }

                // ====================================
                // TIPO 03 - EMBALAGENS NA SOLICITACAO
                //=====================================
                if (tipo == "03")
                {
                    string r3_sol_tipo_registro = linha.Substring(0, 2).Trim();
                    string r3_sol_codigo_embalagem = linha.Substring(2, 8).Trim();

                    decimal r3_sol_quant_solicitada = ConverterDecimal(linha.Substring(10, 10));
                    decimal r3_sol_altura_embalagem = ConverterDecimal(linha.Substring(20, 10));
                    decimal r3_sol_largura_embalagem = ConverterDecimal(linha.Substring(30, 10));
                    decimal r3_sol_comprimento_embalagem = ConverterDecimal(linha.Substring(40, 10));
                    decimal r3_sol_peso_embalagem = ConverterDecimal(linha.Substring(50, 10));

                    string sql = @"

                    IF EXISTS
                    (
                        SELECT 1
                          FROM tbsolicitacoes_embalagens
                         WHERE r3_sol_numero = @numero
                           AND r3_sol_codigo_embalagem = @codigoEmbalagem
                    )
                    BEGIN

                        UPDATE tbsolicitacoes_embalagens
                           SET
                                r3_sol_tipo_registro = @tipoRegistro,
                                r3_sol_quant_solicitada = @quantSolicitada,
                                r3_sol_altura_embalagem = @altura,
                                r3_sol_largura_embalagem = @largura,
                                r3_sol_comprimento_embalagem = @comprimento,
                                r3_sol_peso_embalagem = @peso
                         WHERE r3_sol_numero = @numero
                           AND r3_sol_codigo_embalagem = @codigoEmbalagem

                    END
                    ELSE
                    BEGIN

                        INSERT INTO tbsolicitacoes_embalagens
                        (
                            r3_sol_tipo_registro,
                            r3_sol_numero,
                            r3_sol_codigo_embalagem,
                            r3_sol_quant_solicitada,
                            r3_sol_altura_embalagem,
                            r3_sol_largura_embalagem,
                            r3_sol_comprimento_embalagem,
                            r3_sol_peso_embalagem
                        )
                        VALUES
                        (
                            @tipoRegistro,
                            @numero,
                            @codigoEmbalagem,
                            @quantSolicitada,
                            @altura,
                            @largura,
                            @comprimento,
                            @peso
                        )

                    END";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@tipoRegistro", r3_sol_tipo_registro.Trim());
                        cmd.Parameters.AddWithValue("@numero", numSolic.Trim());
                        cmd.Parameters.AddWithValue("@codigoEmbalagem", r3_sol_codigo_embalagem.Trim());

                        cmd.Parameters.AddWithValue("@quantSolicitada", r3_sol_quant_solicitada);
                        cmd.Parameters.AddWithValue("@altura", r3_sol_altura_embalagem);
                        cmd.Parameters.AddWithValue("@largura", r3_sol_largura_embalagem);
                        cmd.Parameters.AddWithValue("@comprimento", r3_sol_comprimento_embalagem);
                        cmd.Parameters.AddWithValue("@peso", r3_sol_peso_embalagem);

                        cmd.ExecuteNonQuery();
                    }
                }

                // ====================================
                // TIPO 03 - QUANTIDADES DA SOLICITACAO
                //=====================================
                if (tipo == "04")
                {
                    string r4_sol_tipo_registro = linha.Substring(0, 2).Trim();
                    decimal r4_sol_quant_embalagens = ConverterDecimal(linha.Substring(2, 10));
                    decimal r4_sol_quant_paletes = ConverterDecimal(linha.Substring(12, 10));
                    decimal r4_sol_quant_produtos = ConverterDecimal(linha.Substring(22, 10));
                    string sql = @"
                    IF EXISTS
                    (
                        SELECT 1
                          FROM tbsolicitacoes_quantidades
                         WHERE r4_sol_numero = @numero
                    )
                    BEGIN
                        UPDATE tbsolicitacoes_quantidades
                           SET
                                r4_sol_tipo_registro = @tipoRegistro,
                                r4_sol_quant_registro_01 = @quantEmbalagens,
                                r4_sol_quant_registro_02 = @quantPaletes,
                                r4_sol_quant_registro_03 = @quantProdutos
                         WHERE r4_sol_numero = @numero
                    END
                    ELSE
                    BEGIN
                        INSERT INTO tbsolicitacoes_quantidades
                        (
                            r4_sol_tipo_registro,
                            r4_sol_numero,
                            r4_sol_quant_registro_01,
                            r4_sol_quant_registro_02,
                            r4_sol_quant_registro_03
                        )
                        VALUES
                        (
                            @tipoRegistro,
                            @numero,
                            @quantEmbalagens,
                            @quantPaletes,
                            @quantProdutos
                        )
                    END";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@tipoRegistro", r4_sol_tipo_registro.Trim());
                        cmd.Parameters.AddWithValue("@numero", numSolic.Trim());
                        cmd.Parameters.AddWithValue("@quantEmbalagens", r4_sol_quant_embalagens);
                        cmd.Parameters.AddWithValue("@quantPaletes", r4_sol_quant_paletes);
                        cmd.Parameters.AddWithValue("@quantProdutos", r4_sol_quant_produtos);
                        cmd.ExecuteNonQuery();
                    }
                }




            }

            // =======================================================
            // INSERT CARGA COMPLETO ANTES PESQUISAR A TABELA DE FRETE
            // =======================================================
            decimal? distanciaFrete = null;
            string duracaoFrete = null;
            string deslocamentoFrete = null;
            string emitePedagioFrete = null;
            int? codPagador = null;
            string nomePagador = null;
            string cidPagador = null;
            string ufPagador = null;
            string id_rota_entrega = null;

            string cidExpedidor = cidCliExpedidor;
            string ufExpedidor = estCliExpedidor;
            string cidRecebedor = cidCliRecebedor;
            string ufRecebedor = estCliRecebedor;
            string rotaEntrega = null;
            string achou = null;


            string sqlPesquisarFrete = @"
            SELECT rota, desc_rota, distancia, deslocamento, pedagio, tempo 
            FROM tbrotasdeentregas
            WHERE cidade_expedidor COLLATE Latin1_General_CI_AI = @cidade_expedidor
              AND uf_expedidor = @uf_expedidor
              AND cidade_recebedor COLLATE Latin1_General_CI_AI = @cidade_recebedor
              AND uf_recebedor = @uf_recebedor";

            using (SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            using (SqlCommand cmdPesquisarFrete = new SqlCommand(sqlPesquisarFrete, con))
            {
                cmdPesquisarFrete.Parameters.Add("@cidade_expedidor", SqlDbType.VarChar, 120).Value = cidExpedidor;
                cmdPesquisarFrete.Parameters.Add("@uf_expedidor", SqlDbType.VarChar, 2).Value = ufExpedidor;
                cmdPesquisarFrete.Parameters.Add("@cidade_recebedor", SqlDbType.VarChar, 120).Value = cidRecebedor;
                cmdPesquisarFrete.Parameters.Add("@uf_recebedor", SqlDbType.VarChar, 2).Value = ufRecebedor;
                con.Open();
                using (SqlDataReader drPesquisarFrete = cmdPesquisarFrete.ExecuteReader())
                {
                    if (drPesquisarFrete.Read())
                    {
                        //codCliPagador = drPesquisarFrete["cod_pagador"].ToString();
                        //razCliPagador = drPesquisarFrete["pagador"].ToString();
                        //cidCliPagador = drPesquisarFrete["cid_pagador"].ToString();
                        //estCliPagador = drPesquisarFrete["uf_pagador"].ToString();
                        
                        id_rota_entrega = drPesquisarFrete["rota"]?.ToString();
                        duracaoFrete = drPesquisarFrete["tempo"]?.ToString();
                        deslocamentoFrete = drPesquisarFrete["deslocamento"]?.ToString();
                        emitePedagioFrete = drPesquisarFrete["pedagio"]?.ToString();
                        rotaEntrega = drPesquisarFrete["rota"]?.ToString() + " - " + drPesquisarFrete["desc_rota"]?.ToString();
                        distanciaFrete = drPesquisarFrete["distancia"] == DBNull.Value
                        ? (decimal?)null
                        : Convert.ToDecimal(drPesquisarFrete["distancia"]);

                        sDistancia = distanciaFrete?.ToString() ?? "";
                        sDuracao = duracaoFrete ?? "";
                        sDeslocamento = deslocamentoFrete ?? "";
                        sEmitePedagio = emitePedagioFrete ?? "";
                        achou = "Sim";
                    }
                    else
                    {
                        cidExpedidor = null;
                        ufExpedidor = null;
                        cidRecebedor = null;
                        ufRecebedor = null;
                        rotaEntrega = null;
                        sDuracao = null;
                        sDeslocamento = null;
                        sEmitePedagio = null;
                        sDistancia = null;
                        id_rota_entrega = null;

                    }
                }
            }
            string frete = rotaEntrega;
            
            // ===============================
            // pesquisar se a carga ja existe
            // ===============================           
            if (!CargaJaExiste(conn, numSolic))
            {
                string sqlCarga = @"INSERT INTO tbcargas (carga, emissao, status, tomador, entrega, peso, material, portao, situacao, previsao, codorigem, cliorigem, coddestino, clidestino, ufcliorigem, ufclidestino, cidorigem, ciddestino, cadastro, gr, solicitante, empresa, andamento, codvworigem, codvwdestino, 
                  distancia, cod_expedidor, expedidor, cid_expedidor, uf_expedidor, cod_recebedor, recebedor, cid_recebedor, uf_recebedor, tipo_solicitacao, tipo_geracao_solicitacao, tipo_veiculo_solicitacao,  duracao, deslocamento, conta_debito_solicitacao, centro_custo_solicitacao, emitepedagio, desc_veic_vw, cod_pagador, pagador, cid_pagador, uf_pagador, data_hora_coleta, rota_entrega, cnpj_remetente, cnpj_destinatario, cnpj_expedidor, cnpj_recebedor, cnpj_pagador, observacao, id_rota_entrega, desc_material)
                    VALUES
                    (@carga, @emissao, @status, @tomador, @entrega, @peso, @material,
                     @portao, @situacao, @previsao, @codorigem, @cliorigem, @coddestino, 
                     @clidestino, @ufcliorigem, @ufclidestino, @cidorigem, @ciddestino, @cadastro, @gr, @solicitante, @empresa, @andamento, @codvworigem, @codvwdestino, @distancia, @cod_expedidor, @expedidor,
                       @cid_expedidor, @uf_expedidor, @cod_recebedor, @recebedor, @cid_recebedor, @uf_recebedor,  @tipo_solicitacao,
                        @tipo_geracao_solicitacao, @tipo_veiculo_solicitacao,  @duracao, @deslocamento, @conta_debito_solicitacao, @centro_custo_solicitacao, @emitepedagio, @desc_veic_vw, @cod_pagador, @pagador, @cid_pagador, @uf_pagador, @data_hora_coleta, @rota_entrega, @cnpj_remetente, @cnpj_destinatario, @cnpj_expedidor, @cnpj_recebedor, @cnpj_pagador, @observacao, @id_rota_entrega, @desc_material)";

                using (SqlCommand cmd = new SqlCommand(sqlCarga, conn))
                {
                    cmd.Parameters.Add("@carga", SqlDbType.VarChar, 50).Value = numSolic;
                    cmd.Parameters.Add("@emissao", SqlDbType.DateTime).Value = SafeDateTimeValue(lblCadastro + " " + lblHoraCadastro);
                    cmd.Parameters.Add("@status", SqlDbType.VarChar, 50).Value = "Pendente";
                    cmd.Parameters.Add("@tomador", SqlDbType.VarChar, 170).Value = nomePlanta;
                    cmd.Parameters.Add("@entrega", SqlDbType.VarChar, 20).Value = "Normal";
                    cmd.Parameters.Add("@peso", SqlDbType.Decimal).Value = pesoTotal;
                    cmd.Parameters.Add("@material", SqlDbType.VarChar, 50).Value = "OUTROS";
                    cmd.Parameters.Add("@portao", SqlDbType.VarChar, 20).Value = codCliOrigem;
                    cmd.Parameters.Add("@situacao", SqlDbType.VarChar, 20).Value = "Pronto";
                    cmd.Parameters.Add("@previsao", SqlDbType.Date).Value = SafeDateValue(lblColeta);
                    cmd.Parameters.Add("@codorigem", SqlDbType.Int).Value = DbInt(codCliOrigem);
                    cmd.Parameters.Add("@cliorigem", SqlDbType.VarChar, 150).Value = razCliOrigem;
                    cmd.Parameters.Add("@coddestino", SqlDbType.Int).Value = DbInt(codCliDestino);
                    cmd.Parameters.Add("@clidestino", SqlDbType.VarChar, 150).Value = razCliDestino;
                    cmd.Parameters.Add("@ufcliorigem", SqlDbType.Char, 2).Value = estCliOrigem;
                    cmd.Parameters.Add("@ufclidestino", SqlDbType.Char, 2).Value = estCliDestino;
                    cmd.Parameters.Add("@cidorigem", SqlDbType.VarChar, 50).Value = cidCliOrigem;
                    cmd.Parameters.Add("@ciddestino", SqlDbType.VarChar, 50).Value = cidCliDestino;
                    cmd.Parameters.Add("@cnpj_remetente", SqlDbType.VarChar, 50).Value = cnpjCliOrigem;
                    cmd.Parameters.Add("@cnpj_expedidor", SqlDbType.VarChar, 50).Value = cnpjCliExpedidor;
                    cmd.Parameters.Add("@cnpj_destinatario", SqlDbType.VarChar, 50).Value = cnpjCliDestino;
                    cmd.Parameters.Add("@cnpj_recebedor", SqlDbType.VarChar, 50).Value = cnpjCliRecebedor;
                    cmd.Parameters.Add("@empresa", SqlDbType.VarChar, 50).Value = empresaTNG;
                    cmd.Parameters.Add("@desc_material", SqlDbType.VarChar, 50).Value = "Solicitação";

                    cmd.Parameters.Add("@id_rota_entrega", SqlDbType.Int, 50).Value = id_rota_entrega;
                    cmd.Parameters.Add("@rota_entrega", SqlDbType.VarChar, 150).Value = rotaEntrega;

                    cmd.Parameters.Add("@cod_pagador", SqlDbType.Int).Value = codPlantaPagadora;
                    cmd.Parameters.Add("@pagador", SqlDbType.VarChar, 120).Value = razPlantaPagadora;
                    cmd.Parameters.Add("@cnpj_pagador", SqlDbType.VarChar, 50).Value = cnpjPlantaPagadora;
                    cmd.Parameters.Add("@cid_pagador", SqlDbType.VarChar, 50).Value = cidPlantaPagadora;
                    cmd.Parameters.Add("@uf_pagador", SqlDbType.VarChar, 2).Value = estPlantaPagadora;

                    cmd.Parameters.Add("@cadastro", SqlDbType.VarChar, 80).Value = Frm_ImpSolVWMatriz.DataHoraAtual.ToString("dd/MM/yyyy HH:mm") + " - " + usuario.ToUpper();
                    cmd.Parameters.Add("@gr", SqlDbType.VarChar, 50).Value = grPlanta;
                    cmd.Parameters.Add("@solicitante", SqlDbType.VarChar, 50).Value = nomePlanta;
                    //cmd.Parameters.Add("@rota_entrega", SqlDbType.VarChar, 50).Value = frete;                    
                    cmd.Parameters.Add("@andamento", SqlDbType.VarChar, 50).Value = "Pendente";
                    cmd.Parameters.Add("@codvworigem", SqlDbType.VarChar, 10).Value = lblOrigem;
                    cmd.Parameters.Add("@codvwdestino", SqlDbType.VarChar, 10).Value = lblDestino;
                    cmd.Parameters.Add("@distancia", SqlDbType.Decimal).Value = sDistancia ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@cod_expedidor", SqlDbType.Int).Value = DbInt(codCliExpedidor);
                    cmd.Parameters.Add("@expedidor", SqlDbType.VarChar, 150).Value = razCliExpedidor;
                    cmd.Parameters.Add("@cid_expedidor", SqlDbType.VarChar, 50).Value = cidCliExpedidor;
                    cmd.Parameters.Add("@uf_expedidor", SqlDbType.VarChar, 2).Value = estCliExpedidor;
                    cmd.Parameters.Add("@cod_recebedor", SqlDbType.Int)
                       .Value = DbInt(codCliRecebedor);
                    cmd.Parameters.Add("@recebedor", SqlDbType.VarChar, 120)
                       .Value = DbString(razCliRecebedor);
                    cmd.Parameters.Add("@cid_recebedor", SqlDbType.VarChar, 80)
                       .Value = DbString(cidCliRecebedor);
                    cmd.Parameters.Add("@uf_recebedor", SqlDbType.Char, 2)
                       .Value = DbString(estCliRecebedor);                    
                    cmd.Parameters.Add("@tipo_solicitacao", SqlDbType.VarChar, 50).Value = descricaoTipoSolicitacao;
                    cmd.Parameters.Add("@tipo_geracao_solicitacao", SqlDbType.VarChar, 50).Value = descricaoTipoGeracao;
                    cmd.Parameters.Add("@tipo_veiculo_solicitacao", SqlDbType.VarChar, 90).Value = descricaoTipoVeiculo;
                    cmd.Parameters.Add("@duracao", SqlDbType.VarChar, 15).Value = sDuracao;
                    cmd.Parameters.Add("@deslocamento", SqlDbType.VarChar, 30).Value = sDeslocamento;
                    cmd.Parameters.Add("@conta_debito_solicitacao", SqlDbType.VarChar, 30).Value = contaDebito;
                    cmd.Parameters.Add("@centro_custo_solicitacao", SqlDbType.VarChar, 20).Value = centroCusto;
                    cmd.Parameters.Add("@emitepedagio", SqlDbType.VarChar, 3).Value = sEmitePedagio;
                    cmd.Parameters.Add("@desc_veic_vw", SqlDbType.VarChar, 25).Value = descVeicVW;
                    cmd.Parameters.Add("@observacao", SqlDbType.VarChar, 25).Value = "Código SAPIENS: " + codSapiens;
                    cmd.Parameters.AddWithValue("@data_hora_coleta", SqlDbType.DateTime).Value = SafeDateTimeValue(lblColeta + " " + lblHora);                   

                    foreach (SqlParameter p in cmd.Parameters)
                    {
                        if (p.Value == null)
                            p.Value = DBNull.Value;
                    }
                    cmd.ExecuteNonQuery();
                }
            }

            
        }
        private static decimal ConverterDecimal(string valor)
        {
            valor = valor.Trim().Replace(",", ".");

            decimal numero;

            if (decimal.TryParse(valor,
                                 NumberStyles.Any,
                                 CultureInfo.InvariantCulture,
                                 out numero))
                return numero;

            return 0;
        }
        private static string SubStringSeguro(string texto, int inicio, int tamanho)
        {
            if (string.IsNullOrEmpty(texto))
                return "";

            if (inicio >= texto.Length)
                return "";

            if (inicio + tamanho > texto.Length)
                tamanho = texto.Length - inicio;

            return texto.Substring(inicio, tamanho);
        }        
        private static bool CargaJaExiste(SqlConnection conn, string carga)
        {
            using (SqlCommand cmd = new SqlCommand(
                "SELECT 1 FROM tbcargas WHERE carga = @carga", conn))
            {
                cmd.Parameters.Add("@carga", SqlDbType.VarChar, 50).Value = carga;
                return cmd.ExecuteScalar() != null;
            }
        }
        private static bool PedidoJaExiste(SqlConnection conn, string pedido)
        {
            using (SqlCommand cmd = new SqlCommand(
                "SELECT 1 FROM tbpedidos WHERE pedido = @pedido", conn))
            {
                cmd.Parameters.AddWithValue("@pedido", pedido);
                return cmd.ExecuteScalar() != null;
            }
        }
        private void AtualizarBarra(int atual, int total)
        {
            int percentual = 0;
            percentual = (int)((double)atual / total * 100);

            barProgresso.Style["width"] = percentual + "%";
            barProgresso.InnerText = percentual + "%";

            lblStatus.Text = $"Processando {atual} de {total} arquivos...";
        }
        private string BuscarPlanta(SqlConnection conn, string lblPlanta)
        {
            using (SqlCommand cmd = new SqlCommand(
                "SELECT descricao FROM tbPlantavw WHERE codigo = @Codigo", conn))
            {
                cmd.Parameters.AddWithValue("@Codigo", lblPlanta);

                object result = cmd.ExecuteScalar();
                return result == null ? "NÃO CADASTRADO" : result.ToString();
            }
        }
        private static object SafeDateTimeValue(string input)
        {
            DateTime dt;
            if (DateTime.TryParse(input, out dt))
                return dt.ToString("yyyy-MM-dd HH:mm");
            else
                return DBNull.Value;
        }
        private static object SafeDateValue(string input)
        {
            DateTime dt;
            if (DateTime.TryParse(input, out dt))
                return dt.ToString("yyyy-MM-dd");
            else
                return DBNull.Value;
        }        
        protected void btnSair_Click(object sender, EventArgs e)
        {
            Response.Redirect("GestaoDeCargasMatriz.aspx");
        }
        public static object DbValue(object value)
        {
            return value == null ? (object)DBNull.Value : value;
        }
        public static object DbString(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? (object)DBNull.Value : value.Trim();
        }
        public static DateTime DataHoraAtual
        {
            get { return DateTime.Now; }
        }
        public static object DbInt(string valor)
        {
            return int.TryParse(valor, out int v) ? v : (object)DBNull.Value;
        }        
        private void MostrarMsg(string msg, string tipo)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "showalert",
                $"showAlert('{msg}', '{tipo}');", true);

        }
        





    }
}