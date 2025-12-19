using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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

            string pastaDownloads = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            "Downloads");

            string[] arquivos = Directory.GetFiles(pastaDownloads, "SG*.txt");

            int totalArquivos = arquivos.Length;
            int arquivosProcessados = 0;

            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                conn.Open();
               
                int nPedido = 0;
                foreach (string arquivo in arquivos)
                {
                    
                    string nomeArquivo = System.IO.Path.GetFileName(arquivo);

                    //if (ArquivoJaImportado(conn, nomeArquivo))
                    //{
                    //    arquivosProcessados++;
                    //    AtualizarBarra(arquivosProcessados, totalArquivos);
                    //    continue;
                    //}

                    string[] linhas = File.ReadAllLines(arquivo);

                    //if (linhas.Length != 10)
                    //    throw new Exception($"Arquivo {nomeArquivo} não possui 10 linhas.");

                    int linhaNum = 0;
                    
                    foreach (string linha in linhas)
                    {
                        linhaNum++;
                        
                        //string codigo = linha.Substring(0, 10).Trim();
                        //decimal valor = Convert.ToDecimal(linha.Substring(10, 10)) / 100;


                        if (linha.Substring(0, 2) == "01")
                        {
                            // Processar linha do tipo 01
                            // 🧱 POSIÇÕES FIXAS
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
                        if (linha.Substring(0, 2) == "02")
                        {
                            pesoTotal = 0;
                            // Processar linha do tipo 02
                            nPedido++;
                            string sProduto = linha.Substring(2, 13).Trim();
                            sPeso = Decimal.Parse(linha.Substring(25, 10).Trim());
                            string sPedidos = $"{numSolic}{nPedido:D2}";
                            pesoTotal = pesoTotal + sPeso;

                            string sqlPedidos = "INSERT INTO tbpedidos (pedido, carga, emissao, status, solicitante, entrega, peso, material, portao, situacao, previsao, codorigem, cliorigem, coddestino, clidestino, andamento, ufcliorigem, ufclidestino, tomador, cidorigem, ciddestino, gr, cadastro) VALUES (@pedido, @carga, @emissao, @status, @solicitante, @entrega, @peso, @material, @portao, @situacao, @previsao, @codorigem, @cliorigem, @coddestino, @clidestino, @andamento, @ufcliorigem, @ufclidestino, @tomador, @cidorigem, @ciddestino, @gr, @cadastro)";
                            using (SqlCommand cmd = new SqlCommand(sqlPedidos, conn))
                            {
                                cmd.Parameters.Add("@pedido", SqlDbType.Int).Value = sPedidos;
                                cmd.Parameters.Add("@carga", SqlDbType.Int).Value = numSolic;
                                cmd.Parameters.Add("@emissao", SqlDbType.DateTime).Value = SafeDateTimeValue(lblCadastro + " " + lblHoraCadastro);
                                cmd.Parameters.Add("@status", SqlDbType.NVarChar).Value = "Pendente";
                                cmd.Parameters.Add("@solicitante", SqlDbType.NVarChar).Value = nomePlanta;
                                cmd.Parameters.Add("@entrega", SqlDbType.NVarChar).Value = "Normal";
                                cmd.Parameters.Add("@peso", SqlDbType.Decimal).Value = sPeso;
                                cmd.Parameters.Add("@material", SqlDbType.NVarChar).Value = "Solicitação";
                                cmd.Parameters.Add("@portao", SqlDbType.NVarChar).Value = lblPlanta;
                                cmd.Parameters.Add("@situacao", SqlDbType.NVarChar).Value = "Pronto";
                                cmd.Parameters.Add("@previsao", SqlDbType.Date).Value = SafeDateValue(lblColeta);
                                cmd.Parameters.Add("@codorigem", SqlDbType.NVarChar).Value = codCliOrigem;
                                cmd.Parameters.Add("@cliorigem", SqlDbType.NVarChar).Value = razCliOrigem;
                                cmd.Parameters.Add("@coddestino", SqlDbType.NVarChar).Value = codCliDestino;
                                cmd.Parameters.Add("@clidestino", SqlDbType.NVarChar).Value = razCliDestino;
                                cmd.Parameters.Add("@andamento", SqlDbType.NVarChar).Value = "PENDENTE";
                                cmd.Parameters.Add("@ufcliorigem", SqlDbType.NVarChar).Value = estCliOrigem;
                                cmd.Parameters.Add("@ufclidestino", SqlDbType.NVarChar).Value = estCliDestino;
                                cmd.Parameters.Add("@tomador", SqlDbType.NVarChar).Value = nomePlanta;
                                cmd.Parameters.Add("@cidorigem", SqlDbType.NVarChar).Value = cidCliOrigem;
                                cmd.Parameters.Add("@ciddestino", SqlDbType.NVarChar).Value = cidCliDestino;
                                cmd.Parameters.Add("@gr", SqlDbType.NVarChar).Value = grPlanta;
                                cmd.Parameters.Add("@cadastro", SqlDbType.NVarChar).Value = txtUsuario.Text.ToUpper();
                                
                                cmd.ExecuteNonQuery();

                            }

                        }
                        if (linha.Substring(0, 2) == "01")
                        {
                            string sqlCarga = "INSERT INTO tbcargas (carga, emissao, status, tomador, entrega, peso, material, portao, situacao, previsao, codorigem, cliorigem, coddestino, clidestino, ufcliorigem, ufclidestino, cidorigem, ciddestino, cadastro, gr, solicitante, empresa, andamento, codvworigem, codvwdestino, distancia, cod_expedidor, expedidor, cid_expedidor, uf_expedidor, cod_recebedor, recebedor, cid_recebedor, uf_recebedor, nucleo, tipo_solicitacao, tipo_geracao_solicitacao, tipo_veiculo_solicitacao)" +
                               " VALUES" +
                               " (@carga, @emissao, @status, @tomador, @entrega, @peso, @material, @portao, @situacao, @previsao, @codorigem, @cliorigem, @coddestino, @clidestino, @ufcliorigem, @ufclidestino, @cidorigem, @ciddestino, @cadastro, @gr, @solicitante, @empresa, @andamento, @codvworigem, @codvwdestino, @distancia, @cod_expedidor, @expedidor, @cid_expedidor, @uf_expedidor, @cod_recebedor, @recebedor, @cid_recebedor, @uf_recebedor, @nucleo, @tipo_solicitacao, @tipo_geracao_solicitacao, @tipo_veiculo_solicitacao)";

                            using (SqlCommand cmd = new SqlCommand(sqlCarga, conn))
                            {
                                cmd.Parameters.Add("@carga", SqlDbType.Int).Value = numSolic;
                                cmd.Parameters.Add("@emissao", SqlDbType.DateTime).Value = SafeDateTimeValue(lblCadastro + " " + lblHoraCadastro);
                                cmd.Parameters.Add("@status", SqlDbType.NVarChar).Value = "Pendente";
                                cmd.Parameters.Add("@tomador", SqlDbType.NVarChar).Value = nomePlanta;
                                cmd.Parameters.Add("@entrega", SqlDbType.NVarChar).Value = "Normal";
                                cmd.Parameters.Add("@peso", SqlDbType.Decimal).Value = pesoTotal;
                                cmd.Parameters.Add("@material", SqlDbType.NVarChar).Value = "Solicitação";
                                cmd.Parameters.Add("@portao", SqlDbType.NVarChar).Value = lblPlanta;
                                cmd.Parameters.Add("@situacao", SqlDbType.NVarChar).Value = "Pronto";
                                cmd.Parameters.Add("@previsao", SqlDbType.Date).Value = SafeDateValue(lblColeta);
                                cmd.Parameters.Add("@codorigem", SqlDbType.NVarChar).Value = codCliOrigem;
                                cmd.Parameters.Add("@cliorigem", SqlDbType.NVarChar).Value = razCliOrigem;
                                cmd.Parameters.Add("@coddestino", SqlDbType.NVarChar).Value = codCliDestino;
                                cmd.Parameters.Add("@clidestino", SqlDbType.NVarChar).Value = razCliDestino;
                                cmd.Parameters.Add("@ufcliorigem", SqlDbType.NVarChar).Value = estCliOrigem;
                                cmd.Parameters.Add("@ufclidestino", SqlDbType.NVarChar).Value = estCliDestino;
                                cmd.Parameters.Add("@cidorigem", SqlDbType.NVarChar).Value = cidCliOrigem;
                                cmd.Parameters.Add("@ciddestino", SqlDbType.NVarChar).Value = cidCliDestino;
                                cmd.Parameters.Add("@cadastro", SqlDbType.NVarChar).Value = txtUsuario.Text.ToUpper();
                                cmd.Parameters.Add("@gr", SqlDbType.NVarChar).Value = grPlanta;
                                cmd.Parameters.Add("@solicitante", SqlDbType.NVarChar).Value = nomePlanta;
                                cmd.Parameters.Add("@empresa", SqlDbType.NVarChar).Value = "1111";
                                cmd.Parameters.Add("@andamento", SqlDbType.NVarChar).Value = "PENDENTE";
                                cmd.Parameters.Add("@codvworigem", SqlDbType.NVarChar).Value = lblOrigem;
                                cmd.Parameters.Add("@codvwdestino", SqlDbType.NVarChar).Value = lblDestino;
                                cmd.Parameters.Add("@distancia", SqlDbType.Int).Value = 0;
                                cmd.Parameters.Add("@cod_expedidor", SqlDbType.NVarChar).Value = codCliExpedidor;
                                cmd.Parameters.Add("@expedidor", SqlDbType.NVarChar).Value = razCliExpedidor;
                                cmd.Parameters.Add("@cid_expedidor", SqlDbType.NVarChar).Value = cidCliExpedidor;
                                cmd.Parameters.Add("@uf_expedidor", SqlDbType.NVarChar).Value = estCliExpedidor;
                                cmd.Parameters.Add("@cod_recebedor", SqlDbType.NVarChar).Value = codCliRecebedor;
                                cmd.Parameters.Add("@recebedor", SqlDbType.NVarChar).Value = razCliRecebedor;
                                cmd.Parameters.Add("@cid_recebedor", SqlDbType.NVarChar).Value = cidCliRecebedor;
                                cmd.Parameters.Add("@uf_recebedor", SqlDbType.NVarChar).Value = estCliRecebedor;
                                cmd.Parameters.Add("@nucleo", SqlDbType.NVarChar).Value = "MATRIZ";
                                cmd.Parameters.Add("@tipo_solicitacao", SqlDbType.NVarChar).Value = descricaoTipoSolicitacao;
                                cmd.Parameters.Add("@tipo_geracao_solicitacao", SqlDbType.NVarChar).Value = descricaoTipoGeracao;
                                cmd.Parameters.Add("@tipo_veiculo_solicitacao", SqlDbType.NVarChar).Value = descricaoTipoVeiculo;
                                cmd.ExecuteNonQuery();

                            }

                        }

                    }
                    using (SqlCommand cmd = new SqlCommand(
                "INSERT INTO ImportacaoArquivo (NomeArquivo) VALUES (@NomeArquivo)", conn))
                    {
                        cmd.Parameters.AddWithValue("@NomeArquivo", nomeArquivo);
                        cmd.ExecuteNonQuery();
                    }

                    arquivosProcessados++;
                    AtualizarBarra(arquivosProcessados, totalArquivos);
                   
                }

                lblStatus.Text = "✅ Importação finalizada!";
            }


        }

        private bool ArquivoJaImportado(SqlConnection conn, string nomeArquivo)
        {
            using (SqlCommand cmd = new SqlCommand(
                "SELECT COUNT(*) FROM ImportacaoArquivo WHERE NomeArquivo = @NomeArquivo", conn))
            {
                cmd.Parameters.AddWithValue("@NomeArquivo", nomeArquivo);
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        private void AtualizarBarra(int atual, int total)
        {
            int percentual = (int)((double)atual / total * 100);

            barProgresso.Style["width"] = percentual + "%";
            barProgresso.InnerText = percentual + "%";

            lblStatus.Text = $"Processando {atual} de {total} arquivos...";

            upd.Update();
            System.Threading.Thread.Sleep(200); // só para visualizar
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