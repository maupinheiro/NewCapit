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
using System.Web.UI;
using System.Web.UI.WebControls;
using DocumentFormat.OpenXml.Vml;
using DocumentFormat.OpenXml.Wordprocessing;
using NPOI.SS.Formula.Functions;
using Org.BouncyCastle.Asn1.Cmp;

namespace NewCapit.dist.pages
{

    public partial class Frm_ImpSolVWMatriz : System.Web.UI.Page
    {
        SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString());
        DateTime dataHoraAtual = DateTime.Now;
        string numSolic;
        string lblTipoGeracao;
        string lblTipoSolicitacao;
        string lblPlanta;
        string lblOrigem;
        string lblDestino;
        string lblCadastro;
        string lblHoraCadastro;
        string lblColeta;
        string lblHora;
        string lblTipoVeiculo;
        decimal pesoTotal;
        string codPlanta;
        string grPlanta;
        string nomePlanta;
        string codCliOrigem;
        string razCliOrigem;
        string cidCliOrigem;
        string estCliOrigem;
        string codCliDestino;
        string razCliDestino;
        string cidCliDestino;
        string estCliDestino;
        string codigoTipoGeracao;
        string codvwTipoGeracao;
        string descricaoTipoGeracao;
        string codigoTipoSolicitacao;
        string codvwTipoSolicitacao;
        string descricaoTipoSolicitacao;
        string codigoTipoVeiculo;
        string codvwTipoVeiculo;
        string descricaoTipoVeiculo;
        string codCliExpedidor;
        string razCliExpedidor;
        string cidCliExpedidor;
        string estCliExpedidor;
        string codCliRecebedor;
        string razCliRecebedor;
        string cidCliRecebedor;
        string estCliRecebedor;
        decimal sPeso;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UsuarioLogado"] != null)
                {
                    string nomeUsuario = Session["UsuarioLogado"].ToString();
                    var lblUsuario = nomeUsuario;
                    txtUsuario.Text = dataHoraAtual.ToString("dd/MM/yyyy HH:mm") + " - " + lblUsuario.ToUpper();
                }
                else
                {
                    var lblUsuario = "<Usuário>";
                    Response.Redirect("Login.aspx");
                }


            }

        }
        protected void btnImportar_Click(object sender, EventArgs e)
        {
            string pastaDownloads = System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                "Downloads");

            string[] arquivos = Directory.GetFiles(pastaDownloads, "SG*.txt");

            if (arquivos.Length == 0)
            {
                lblStatus.Text = "Nenhum arquivo encontrado.";
                return;
            }

            using (SqlConnection conn = new SqlConnection(
                WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                conn.Open();

                int atual = 0;

                foreach (string arquivo in arquivos)
                {
                    ProcessarArquivo(conn, arquivo);
                    atual++;
                    AtualizarBarra(atual, arquivos.Length);
                }
            }

            lblStatus.Text = $"✅ Importação finalizada com sucesso! {arquivos.Length} arquivo(s) importado(s).";
        }

        
        private void ProcessarArquivo(SqlConnection conn, string arquivo)
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
                 SELECT codcli, razcli, cidcli, estcli
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
                                    cidCliOrigem = dr["cidcli"].ToString();
                                    estCliOrigem = dr["estcli"].ToString();
                                }
                                else
                                {
                                    // Não encontrou
                                    codCliOrigem = "";
                                    razCliOrigem = "";
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
                 SELECT codcli, razcli, cidcli, estcli
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
                                    cidCliDestino = dr["cidcli"].ToString();
                                    estCliDestino = dr["estcli"].ToString();
                                }
                                else
                                {
                                    // Não encontrou
                                    codCliDestino = "";
                                    razCliDestino = "";
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
                 SELECT codigo, codvw, descricao
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
                                }
                                else
                                {
                                    // Não encontrou
                                    codigoTipoVeiculo = "";
                                    codvwTipoVeiculo = "";
                                    descricaoTipoVeiculo = "";

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
                        estCliExpedidor = estCliOrigem;

                        codCliRecebedor = codCliDestino;
                        razCliRecebedor = razCliDestino;
                        cidCliRecebedor = cidCliDestino;
                        estCliRecebedor = estCliDestino;

                    }
                    else
                    {

                        codCliExpedidor = codCliOrigem;
                        razCliExpedidor = razCliOrigem;
                        cidCliExpedidor = cidCliOrigem;
                        estCliExpedidor = estCliOrigem;

                        codCliRecebedor = codCliDestino;
                        razCliRecebedor = razCliDestino;
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
                            cmd.Parameters.AddWithValue("@status", "PENDENTE");
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
                            cmd.Parameters.AddWithValue("@andamento", "PENDENTE");
                            cmd.Parameters.AddWithValue("@ufcliorigem", estCliOrigem);
                            cmd.Parameters.AddWithValue("@ufclidestino", estCliDestino);
                            cmd.Parameters.AddWithValue("@placas", DBNull.Value);
                            cmd.Parameters.AddWithValue("@saida", DBNull.Value);
                            cmd.Parameters.AddWithValue("@chegada", DBNull.Value);
                            cmd.Parameters.AddWithValue("@tomador", planta);
                            cmd.Parameters.AddWithValue("@cidorigem", cidCliOrigem);
                            cmd.Parameters.AddWithValue("@ciddestino", cidCliDestino);
                            cmd.Parameters.AddWithValue("@gr", grPlanta);
                            cmd.Parameters.AddWithValue("@cadastro", txtUsuario.Text.ToUpper());
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

            // =========================
            // INSERT CARGA COMPLETO
            // =========================
            if (!CargaJaExiste(conn, numSolic))
            {
                string sqlCarga = @"INSERT INTO tbcargas (carga, emissao, status, tomador, entrega, peso, material, portao, situacao, previsao, codorigem, cliorigem, coddestino, 
                                    clidestino, ufcliorigem, ufclidestino, cidorigem, ciddestino, cadastro, gr, solicitante, empresa, andamento, codvworigem, codvwdestino, 
                                    distancia, cod_expedidor, expedidor, cid_expedidor, uf_expedidor, cod_recebedor, recebedor, cid_recebedor, uf_recebedor, nucleo, tipo_solicitacao,
                                    tipo_geracao_solicitacao, tipo_veiculo_solicitacao)
                                     VALUES
                                    (@carga, @emissao, @status, @tomador, @entrega, @peso, @material, 
                                    @portao, @situacao, @previsao, @codorigem, @cliorigem, @coddestino, 
                                    @clidestino, @ufcliorigem, @ufclidestino, @cidorigem, @ciddestino, @cadastro,
                                    @gr, @solicitante, @empresa, @andamento, @codvworigem, @codvwdestino, @distancia, @cod_expedidor, @expedidor,
                                    @cid_expedidor, @uf_expedidor, @cod_recebedor, @recebedor, @cid_recebedor, @uf_recebedor, @nucleo, @tipo_solicitacao,
                                    @tipo_geracao_solicitacao, @tipo_veiculo_solicitacao)";

                using (SqlCommand cmd = new SqlCommand(sqlCarga, conn))
                {
                    cmd.Parameters.AddWithValue("@carga", numSolic);
                    cmd.Parameters.AddWithValue("@emissao", SafeDateTimeValue(dtCadastro + " " + hrCadastro));
                    cmd.Parameters.AddWithValue("@status", "PENDENTE");
                    cmd.Parameters.AddWithValue("@tomador", planta);
                    cmd.Parameters.AddWithValue("@entrega", "Normal");
                    cmd.Parameters.AddWithValue("@peso", pesoTotal);
                    cmd.Parameters.AddWithValue("@material", "Solicitação");
                    cmd.Parameters.AddWithValue("@portao", planta);
                    cmd.Parameters.AddWithValue("@situacao", "Pronto");
                    cmd.Parameters.AddWithValue("@previsao", SafeDateValue(dtColeta));

                    cmd.Parameters.AddWithValue("@codorigem", codCliOrigem);
                    cmd.Parameters.AddWithValue("@cliorigem", razCliOrigem);
                    cmd.Parameters.AddWithValue("@coddestino", codCliDestino);
                    cmd.Parameters.AddWithValue("@clidestino", razCliDestino);
                    cmd.Parameters.AddWithValue("@ufcliorigem", estCliOrigem);
                    cmd.Parameters.AddWithValue("@ufclidestino", estCliDestino);
                    cmd.Parameters.AddWithValue("@cidorigem", cidCliOrigem);
                    cmd.Parameters.AddWithValue("@ciddestino", cidCliDestino);
                    cmd.Parameters.AddWithValue("@cadastro", txtUsuario.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@gr", grPlanta);
                    cmd.Parameters.AddWithValue("@solicitante", planta);
                    cmd.Parameters.AddWithValue("@empresa", "1111");
                    cmd.Parameters.AddWithValue("@andamento", "PENDENTE");
                    cmd.Parameters.AddWithValue("@codvworigem", lblOrigem);
                    cmd.Parameters.AddWithValue("@codvwdestino", lblDestino);
                    cmd.Parameters.AddWithValue("@distancia", 0);
                    cmd.Parameters.AddWithValue("@cod_expedidor", codCliExpedidor);
                    cmd.Parameters.AddWithValue("@expedidor", razCliExpedidor);
                    cmd.Parameters.AddWithValue("@cid_expedidor", cidCliExpedidor);
                    cmd.Parameters.AddWithValue("@uf_expedidor", estCliExpedidor);
                    cmd.Parameters.AddWithValue("@cod_recebedor", codCliRecebedor);
                    cmd.Parameters.AddWithValue("@recebedor", razCliRecebedor);
                    cmd.Parameters.AddWithValue("@cid_recebedor", cidCliRecebedor);
                    cmd.Parameters.AddWithValue("@uf_recebedor", estCliRecebedor);
                    cmd.Parameters.AddWithValue("@nucleo", "MATRIZ");
                    cmd.Parameters.AddWithValue("@tipo_solicitacao", descricaoTipoSolicitacao);
                    cmd.Parameters.AddWithValue("@tipo_geracao_solicitacao", descricaoTipoGeracao);
                    cmd.Parameters.AddWithValue("@tipo_veiculo_solicitacao", descricaoTipoVeiculo);


                    cmd.ExecuteNonQuery();
                }
            }

        }


        private bool CargaJaExiste(SqlConnection conn, string carga)
        {
            using (SqlCommand cmd = new SqlCommand(
                "SELECT 1 FROM tbcargas WHERE carga = @carga", conn))
            {
                cmd.Parameters.AddWithValue("@carga", carga);
                return cmd.ExecuteScalar() != null;
            }
        }
        private bool PedidoJaExiste(SqlConnection conn, string pedido)
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
            int percentual = (int)((double)atual / total * 100);

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

        private object SafeDateTimeValue(string input)
        {
            DateTime dt;
            if (DateTime.TryParse(input, out dt))
                return dt.ToString("yyyy-MM-dd HH:mm");
            else
                return DBNull.Value;
        }
        private object SafeDateValue(string input)
        {
            DateTime dt;
            if (DateTime.TryParse(input, out dt))
                return dt.ToString("yyyy-MM-dd");
            else
                return DBNull.Value;
        }
        //protected void MostrarMsg(string mensagem, string tipo = "warning")
        //{
        //    divMsg.Attributes["class"] = "alert alert-" + tipo + " alert-dismissible fade show mt-3";
        //    lblMsg.InnerText = mensagem;
        //    divMsg.Style["display"] = "block";

        //    string script = @"setTimeout(function() {
        //                var div = document.getElementById('divMsg');
        //                if (div) div.style.display = 'none';
        //              }, 5000);";

        //    ScriptManager.RegisterStartupScript(this, GetType(), "EscondeMsg", script, true);
        //}

        //protected void btnImportar_Click(object sender, EventArgs e)
        //{
        //    string pastaDownloads = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
        //"Downloads");

        //    // pegar apenas arquivos SG*.txt
        //    // var arquivos = Directory.GetFiles(pastaDownloads, "SG*.txt");


        //    string[] arquivos = Directory.GetFiles(pastaDownloads, "SG*.txt");
        //    int totalArquivos = arquivos.Length;
        //    int arquivosProcessados = 0;
        //    lblProgresso.Visible = true;

        //    using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
        //    {
        //        conn.Open();

        //        int totalArquivos = arquivos.Length;
        //        int arquivoAtual = 0;

        //        foreach (string arquivo in arquivos)
        //        {
        //            arquivoAtual++;

        //            string nomeArquivo = System.IO.Path.GetFileName(arquivo);

        //            // 🔒 EVITAR ARQUIVO REPETIDO
        //            if (ArquivoJaImportado(conn, nomeArquivo))
        //            {
        //                lblProgresso.Text = $"Arquivo {nomeArquivo} já importado – ignorado.";
        //                continue;
        //            }

        //            string[] linhas = File.ReadAllLines(arquivo);

        //            //if (linhas.Length != 10)
        //            //    throw new Exception($"Arquivo {nomeArquivo} não possui 10 linhas.");

        //            int linhaNumero = 0;

        //            foreach (string linha in linhas)
        //            {
        //                linhaNumero++;

        //                if (linha.Substring(0, 2) == "01")
        //                {
        //                    // Processar linha do tipo 01
        //                    // 🧱 POSIÇÕES FIXAS
        //                    string numSolic = linha.Substring(2, 10).Trim();
        //                    String lblTipoGeracao = linha.Substring(12, 4).Trim();
        //                    String lblTipoSolicitacao = linha.Substring(16, 7).Trim();
        //                    String lblPlanta = linha.Substring(23, 2).Trim();
        //                    String lblOrigem = linha.Substring(25, 4).Trim();
        //                    String lblDestino = linha.Substring(29, 4).Trim();
        //                    String lblCadastro = linha.Substring(33, 10).Trim();
        //                    String lblHoraCadastro = linha.Substring(43, 5).Trim();
        //                    String lblColeta = linha.Substring(51, 10).Trim();
        //                    String lblHora = linha.Substring(61, 5).Trim();
        //                    String lblTipoVeiculo = linha.Substring(70, 4).Trim();

        //                    string sql = "INSERT INTO tbcargas (carga) VALUES (@numSolic)";

        //                    using (SqlCommand cmd = new SqlCommand(sql, conn))
        //                    {
        //                        cmd.Parameters.Add("@numSolic", SqlDbType.Int).Value = numSolic;
        //                        cmd.ExecuteNonQuery();
        //                    }

        //                }
        //                else if (linha.Substring(0, 10) == "02")
        //                {
        //                    // Processar linha do tipo 02
        //                }                        
        //            }

        //            // 📝 Marca arquivo como importado
        //            using (SqlCommand cmd = new SqlCommand(
        //                "INSERT INTO ImportacaoArquivo (NomeArquivo) VALUES (@NomeArquivo)", conn))
        //            {
        //                cmd.Parameters.AddWithValue("@NomeArquivo", nomeArquivo);
        //                cmd.ExecuteNonQuery();
        //            }

        //            // 📊 PROGRESSO NA TELA
        //            lblProgresso.Text = $"Importando {arquivoAtual} de {totalArquivos} arquivos...";
        //            Thread.Sleep(300); // apenas para visualizar o progresso
        //        }

        //        lblProgresso.Text = "✅ Importação concluída com sucesso!";
        //    }
        //}
        //private bool ArquivoJaImportado(SqlConnection conn, string nomeArquivo)
        //{
        //    using (SqlCommand cmd = new SqlCommand(
        //        "SELECT COUNT(1) FROM ImportacaoArquivo WHERE NomeArquivo = @NomeArquivo", conn))
        //    {
        //        cmd.Parameters.AddWithValue("@NomeArquivo", nomeArquivo);
        //        return (int)cmd.ExecuteScalar() > 0;
        //    }
        //}

    }
}