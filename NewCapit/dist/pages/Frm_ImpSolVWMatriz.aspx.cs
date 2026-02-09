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
            string dtCadastro = "";
            string hrCadastro = "";
            string dtColeta = "";
            string planta = "";

            int seqPedido = 0;
            decimal pesoTotal = 0;

            foreach (string linha in linhas)
            {
                string tipo = linha.Substring(0, 2);

                // =========================
                // TIPO 01 – CARGA
                // =========================
                if (tipo == "01")
                {
                    seqPedido = 0;
                    pesoTotal = 0;

                    numSolic = linha.Substring(2, 10).Trim();
                    planta = linha.Substring(23, 2).Trim();
                    dtCadastro = linha.Substring(33, 10).Trim();
                    hrCadastro = linha.Substring(43, 5).Trim();
                    dtColeta = linha.Substring(51, 10).Trim();

                    numSolic = linha.Substring(2, 10).Trim();
                    lblTipoGeracao = linha.Substring(12, 4).Trim();
                    lblTipoSolicitacao = linha.Substring(16, 7).Trim();
                    lblPlanta = linha.Substring(23, 2).Trim();
                    lblOrigem = linha.Substring(25, 4).Trim();
                    lblDestino = linha.Substring(29, 4).Trim();
                    lblCadastro = linha.Substring(33, 10).Trim();
                    lblHoraCadastro = linha.Substring(43, 5).Trim();
                    lblColeta = linha.Substring(51, 10).Trim();
                    lblHora = linha.Substring(61, 5).Trim();
                    lblTipoVeiculo = linha.Substring(70, 4).Trim();
                    contaDebito = linha.Substring(78, 8).Trim();
                    centroCusto = linha.Substring(86, 4).Trim();

                    // string descricaoPlanta = BuscarPlanta(conn, lblPlanta.Trim());

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
                 SELECT codigo, codvw, descricao, descricao_tng
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

                    if (lblTipoSolicitacao == "18")
                    {
                        codCliExpedidor = codCliOrigem;
                        razCliExpedidor = razCliOrigem;
                        cidCliExpedidor = cidCliOrigem;
                        cnpjCliExpedidor = cnpjCliOrigem;
                        estCliExpedidor = estCliOrigem;

                        codCliOrigem = "1020";
                        razCliOrigem = "VOLKSWAGEN DO BRASIL INDUSTRIA DE VEICULOS AUTOMOTORES LTDA";
                        cidCliOrigem = "SÃO BERNARDO DO CAMPO";
                        estCliOrigem = "SP";
                        cnpjCliOrigem = "59.104.422/0057-04";

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
                }

                // =========================
                // TIPO 02 – PEDIDO
                // =========================
                if (tipo == "02")
                {
                    seqPedido++;

                    decimal peso = decimal.Parse(
                        linha.Substring(25, 10).Trim(),
                        CultureInfo.InvariantCulture);

                    pesoTotal += peso;

                    string pedido = $"{numSolic}{seqPedido:D2}";

                    string sqlPedido = @"
                                    INSERT INTO tbpedidos
                                    (pedido, carga, emissao, status, solicitante, entrega, peso, material,
                                     portao, situacao, previsao, codorigem, cliorigem, coddestino, clidestino,
                                     observacao, idviagem, andamento, ufcliorigem, ufclidestino, placas, saida,
                                     chegada, tomador, cidorigem, ciddestino, gr, cadastro, atualizacao,
                                     motcar, iniciocar, termcar, duracao, controledocliente)
                                    VALUES
                                    (@pedido, @carga, @emissao, @status, @solicitante, @entrega, @peso, @material,
                                     @portao, @situacao, @previsao, @codorigem, @cliorigem, @coddestino, @clidestino,
                                     @observacao, @idviagem, @andamento, @ufcliorigem, @ufclidestino, @placas, @saida,
                                     @chegada, @tomador, @cidorigem, @ciddestino, @gr, @cadastro, @atualizacao,
                                     @motcar, @iniciocar, @termcar, @duracao, @controledocliente)";

                    if (!PedidoJaExiste(conn, pedido))
                    {

                        using (SqlCommand cmd = new SqlCommand(sqlPedido, conn))
                        {
                            cmd.Parameters.AddWithValue("@pedido", pedido);
                            cmd.Parameters.AddWithValue("@carga", numSolic);
                            cmd.Parameters.AddWithValue("@emissao", SafeDateTimeValue(dtCadastro + " " + hrCadastro));
                            cmd.Parameters.AddWithValue("@status", "Pendente");
                            cmd.Parameters.AddWithValue("@solicitante", planta);
                            cmd.Parameters.AddWithValue("@entrega", "Normal");
                            cmd.Parameters.AddWithValue("@peso", peso);
                            cmd.Parameters.AddWithValue("@material", "Solicitação");
                            cmd.Parameters.AddWithValue("@portao", planta);
                            cmd.Parameters.AddWithValue("@situacao", "Pronto");
                            cmd.Parameters.AddWithValue("@previsao", SafeDateValue(dtColeta));

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

            string cidExpedidor = cidCliExpedidor;
            string ufExpedidor = estCliExpedidor;
            string cidRecebedor = cidCliRecebedor;
            string ufRecebedor = estCliRecebedor;
            string rotaEntrega = null;
            string achou = null;


            string sqlPesquisarFrete = @"
            SELECT rota, desc_rota, distancia, deslocamento, pedagio, tempo 
            FROM tbrotasdeentregas
            WHERE cidade_expedidor = @cidade_expedidor
              AND uf_expedidor = @uf_expedidor
              AND cidade_recebedor = @cidade_recebedor
              AND uf_recebedor = @uf_recebedor";

            using (SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            using (SqlCommand cmdPesquisarFrete = new SqlCommand(sqlPesquisarFrete, con))
            {
                cmdPesquisarFrete.Parameters.Add("@cidade_expedidor", SqlDbType.VarChar, 120).Value = cidExpedidor;
                cmdPesquisarFrete.Parameters.Add("@uf_expedidor", SqlDbType.VarChar, 50).Value = ufExpedidor;
                cmdPesquisarFrete.Parameters.Add("@cidade_recebedor", SqlDbType.VarChar, 120).Value = cidRecebedor;
                cmdPesquisarFrete.Parameters.Add("@uf_recebedor", SqlDbType.VarChar, 50).Value = ufRecebedor;
                con.Open();
                using (SqlDataReader drPesquisarFrete = cmdPesquisarFrete.ExecuteReader())
                {
                    if (drPesquisarFrete.Read())
                    {
                        //codCliPagador = drPesquisarFrete["cod_pagador"].ToString();
                        //razCliPagador = drPesquisarFrete["pagador"].ToString();
                        //cidCliPagador = drPesquisarFrete["cid_pagador"].ToString();
                        //estCliPagador = drPesquisarFrete["uf_pagador"].ToString();

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
                        
                    }
                }
            }
            string frete = rotaEntrega;
            //conn.Open();

            // ===============================
            // pesquisar se a carga ja existe
            // ===============================           
             if (!CargaJaExiste(conn, numSolic))
                {
                    string sqlCarga = @"INSERT INTO tbcargas (carga, emissao, status, tomador, entrega, peso, material, portao, situacao, previsao, codorigem, cliorigem, coddestino, clidestino, ufcliorigem, ufclidestino, cidorigem, ciddestino, cadastro, gr, solicitante, empresa, andamento, codvworigem, codvwdestino, 
                  distancia, cod_expedidor, expedidor, cid_expedidor, uf_expedidor, cod_recebedor, recebedor, cid_recebedor, uf_recebedor, nucleo, tipo_solicitacao, tipo_geracao_solicitacao, tipo_veiculo_solicitacao,  duracao, deslocamento, conta_debito_solicitacao, centro_custo_solicitacao, emitepedagio, desc_veic_vw, cod_pagador, pagador, cid_pagador, uf_pagador, data_hora_coleta, rota_entrega, cnpj_remetente, cnpj_destinatario, cnpj_expedidor, cnpj_recebedor, cnpj_pagador, observacao)
                    VALUES
                    (@carga, @emissao, @status, @tomador, @entrega, @peso, @material,
                     @portao, @situacao, @previsao, @codorigem, @cliorigem, @coddestino, 
                     @clidestino, @ufcliorigem, @ufclidestino, @cidorigem, @ciddestino, @cadastro, @gr, @solicitante, @empresa, @andamento, @codvworigem, @codvwdestino, @distancia, @cod_expedidor, @expedidor,
                       @cid_expedidor, @uf_expedidor, @cod_recebedor, @recebedor, @cid_recebedor, @uf_recebedor, @nucleo, @tipo_solicitacao,
                        @tipo_geracao_solicitacao, @tipo_veiculo_solicitacao,  @duracao, @deslocamento, @conta_debito_solicitacao, @centro_custo_solicitacao, @emitepedagio, @desc_veic_vw, @cod_pagador, @pagador, @cid_pagador, @uf_pagador, @data_hora_coleta, @rota_entrega, @cnpj_remetente, @cnpj_destinatario, @cnpj_expedidor, @cnpj_recebedor, @cnpj_pagador, @observacao)";

                    using (SqlCommand cmd = new SqlCommand(sqlCarga, conn))
                    {
                        cmd.Parameters.Add("@carga", SqlDbType.VarChar, 50).Value = numSolic;
                        cmd.Parameters.Add("@emissao", SqlDbType.DateTime).Value = SafeDateTimeValue(dtCadastro + " " + hrCadastro);
                        cmd.Parameters.Add("@status", SqlDbType.VarChar, 50).Value = "Pendente";
                        cmd.Parameters.Add("@tomador", SqlDbType.VarChar, 170).Value = nomePlanta;
                        cmd.Parameters.Add("@entrega", SqlDbType.VarChar, 20).Value = "Normal";
                        cmd.Parameters.Add("@peso", SqlDbType.Decimal).Value = pesoTotal;
                        cmd.Parameters.Add("@material", SqlDbType.VarChar, 50).Value = "OUTROS";
                        cmd.Parameters.Add("@portao", SqlDbType.VarChar, 20).Value = planta;
                        cmd.Parameters.Add("@situacao", SqlDbType.VarChar, 20).Value = "Pronto";
                        cmd.Parameters.Add("@previsao", SqlDbType.Date).Value = SafeDateValue(dtColeta);
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

                    cmd.Parameters.Add("@cod_pagador", SqlDbType.Int).Value = "1020";
                    cmd.Parameters.Add("@pagador", SqlDbType.VarChar, 120)
                       .Value = "VOLKSWAGEN DO BRASIL INDUSTRIA DE VEICULOS AUTOMOTORES LTDA";
                    cmd.Parameters.Add("@cnpj_pagador", SqlDbType.VarChar, 50).Value = "59.104.422/0057-04";
                    cmd.Parameters.Add("@cid_pagador", SqlDbType.VarChar, 50)
                       .Value = "SÃO BERNARDO DO CAMPO";
                    cmd.Parameters.Add("@uf_pagador", SqlDbType.VarChar, 2)
                       .Value = "SP"; 

                    cmd.Parameters.Add("@cadastro", SqlDbType.VarChar, 80).Value = Frm_ImpSolVWMatriz.DataHoraAtual.ToString("dd/MM/yyyy HH:mm") + " - " + usuario.ToUpper();
                        cmd.Parameters.Add("@gr", SqlDbType.VarChar, 50).Value = grPlanta;
                        cmd.Parameters.Add("@solicitante", SqlDbType.VarChar, 50).Value = nomePlanta;
                        cmd.Parameters.Add("@rota_entrega", SqlDbType.VarChar, 50).Value = frete;
                        cmd.Parameters.Add("@empresa", SqlDbType.VarChar, 50).Value = "1111";
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
                        cmd.Parameters.Add("@nucleo", SqlDbType.VarChar, 50).Value = "MATRIZ";
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

                        cmd.Parameters.Add("@data_hora_coleta", SqlDbType.DateTime)
                           .Value = DbString(lblColeta + " " + lblHora);

                        foreach (SqlParameter p in cmd.Parameters)
                        {
                            if (p.Value == null)
                                p.Value = DBNull.Value;
                        }
                        cmd.ExecuteNonQuery();
                    }
                }
           
            

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