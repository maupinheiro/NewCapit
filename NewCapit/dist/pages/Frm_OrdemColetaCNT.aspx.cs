using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Domain;
using NPOI.SS.Formula.Functions;
using static NPOI.HSSF.Util.HSSFColor;

namespace NewCapit.dist.pages
{
    public partial class Frm_OrdemColetaCNT : System.Web.UI.Page
    {
        public string fotoMotorista;
        string codmot, caminhofoto;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UsuarioLogado"] != null)
                {
                    string nomeUsuario = Session["UsuarioLogado"].ToString();
                    var lblUsuario = nomeUsuario;
                    txtUsuCadastro.Text = nomeUsuario;
                }
                else
                {
                    var lblUsuario = "<Usuário>";
                    txtUsuCadastro.Text = lblUsuario;
                }
                DateTime dataHoraAtual = DateTime.Now;
                lblDtCadastro.Text = dataHoraAtual.ToString("dd/MM/yyyy HH:mm");
                PreencherComboStatus();
                PreencherNumColeta();
                fotoMotorista = "../../fotos/usuario.jpg";
            }


                      

        }
        private void PreencherNumColeta()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT (carregamento + incremento) as ProximaColeta FROM tbcontadores";

            // Crie uma conexão com o banco de dados
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Crie o comando SQL
                        //SqlCommand cmd = new SqlCommand(query, conn);

                        // Execute o comando e obtenha os dados em um DataReader
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                // Preencher o TextBox com o nome encontrado 
                                novaColeta.Text = reader["ProximaColeta"].ToString();
                            }
                        }

                    }
                    string id = "1";

                    // Verifica se o ID foi fornecido e é um número válido
                    if (string.IsNullOrEmpty(id) || !int.TryParse(id, out int idConvertido))
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", "alert('ID inválido ou não fornecido.');", true);
                        return;
                    }
                    string sql = @"UPDATE tbcontadores SET carregamento = @carregamento WHERE id = @id";
                    try
                    {
                        using (SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
                        using (SqlCommand cmd = new SqlCommand(sql, con))
                        {
                            cmd.Parameters.AddWithValue("@carregamento", novaColeta.Text);
                            cmd.Parameters.AddWithValue("@id", idConvertido);

                            con.Open();
                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                // atualiza  
                            }
                            else
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", "alert('Erro ao atualizar o número da coleta.');", true);
                            }

                        }


                    }
                    catch (Exception ex)
                    {
                        string mensagemErro = $"Erro ao atualizar: {HttpUtility.JavaScriptStringEncode(ex.Message)}";
                        string script = $"alert('{mensagemErro}');";
                        ClientScript.RegisterStartupScript(this.GetType(), "Erro", script, true);
                    }
                }
                catch (Exception ex)
                {
                    //Tratar erro
                    //txtResultado.Text = "Erro: " + ex.Message;
                }
            }
        }
        private void PreencherComboStatus()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT cod_status, ds_status FROM tb_status";

            // Crie uma conexão com o banco de dados
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                try
                {
                    // Abra a conexão com o banco de dados
                    conn.Open();

                    // Crie o comando SQL
                    SqlCommand cmd = new SqlCommand(query, conn);

                    // Execute o comando e obtenha os dados em um DataReader
                    SqlDataReader reader = cmd.ExecuteReader();

                    // Preencher o ComboBox com os dados do DataReader
                    ddlStatus.DataSource = reader;
                    ddlStatus.DataTextField = "ds_status";  // Campo que será mostrado no ComboBox
                    ddlStatus.DataValueField = "cod_status";  // Campo que será o valor de cada item                    
                    ddlStatus.DataBind();  // Realiza o binding dos dados                   
                    ddlStatus.Items.Insert(0, new ListItem("", "0"));
                    // Feche o reader
                    reader.Close();
                }
                catch (Exception ex)
                {
                    // Trate exceções
                    Response.Write("Erro: " + ex.Message);
                }
            }
        }
        protected void btnPesquisarMotorista_Click(object sender, EventArgs e)
        {
            if (txtCodMotorista.Text.Trim() == "")
            {
                string nomeUsuario = txtUsuCadastro.Text;

                string linha1 = "Olá, " + nomeUsuario + "!";
                string linha2 = "Por favor, digite o código do motorista.";

                // Concatenando as linhas com '\n' para criar a mensagem
                string mensagem = $"{linha1}\n{linha2}";

                string mensagemCodificada = HttpUtility.JavaScriptStringEncode(mensagem);
                // Gerando o script JavaScript para exibir o alerta
                string script = $"alert('{mensagemCodificada}');";

                // Registrando o script para execução no lado do cliente
                ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", script, true);
                txtCodMotorista.Focus();

            }
            else
            {
                var codigo = txtCodMotorista.Text.Trim();
                var obj = new Domain.ConsultaMotorista
                {
                    codmot = codigo
                };
                var ConsultaMotorista = DAL.UsersDAL.CheckMotorista(obj);
                if (ConsultaMotorista != null)
                {
                    if (ConsultaMotorista.status.Trim() == "INATIVO")
                    {
                        string nomeUsuario = txtUsuCadastro.Text;
                        string razaoSocial = ConsultaMotorista.nommot;
                        string unidade = ConsultaMotorista.nucleo;

                        string linha1 = "Olá, " + nomeUsuario + "!";
                        string linha2 = "Código " + codigo + ", excluido ou inativo no sistema.";
                        string linha3 = "Motorista: " + razaoSocial + ".";
                        string linha4 = "Filial: " + unidade + ". Por favor, verifique.";

                        // Concatenando as linhas com '\n' para criar a mensagem
                        string mensagem = $"{linha1}\n{linha2}\n{linha3}\n{linha4}";

                        string mensagemCodificada = HttpUtility.JavaScriptStringEncode(mensagem);
                        // Gerando o script JavaScript para exibir o alerta
                        string script = $"alert('{mensagemCodificada}');";

                        // Registrando o script para execução no lado do cliente
                        ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", script, true);
                        txtCodMotorista.Text = "";
                        txtCodMotorista.Focus();
                    }
                    else{
                        txtFilialMot.Text = ConsultaMotorista.nucleo;
                        txtTipoMot.Text = ConsultaMotorista.tipomot;
                        txtExameToxic.Text = ConsultaMotorista.venceti;
                        txtCNH.Text = ConsultaMotorista.venccnh.ToString();
                        txtLibGR.Text = ConsultaMotorista.validade;
                        txtNomMot.Text = ConsultaMotorista.nommot;
                        txtCPF.Text = ConsultaMotorista.cpf;
                        txtCartao.Text = ConsultaMotorista.cartaomot;
                        txtValCartao.Text = ConsultaMotorista.venccartao;
                        txtCelular.Text = ConsultaMotorista.fone2;
                        if (txtCodMotorista.Text.Trim() != "")
                        {
                            fotoMotorista = ConsultaMotorista.caminhofoto.Trim().ToString();

                            String path = Server.MapPath("../../fotos/");
                            string file = fotoMotorista;
                            if (File.Exists(path + file))
                            {
                                fotoMotorista = "../../fotos/" + file + "";
                            }
                            else
                            {
                                fotoMotorista = "../../fotos/usuario.jpg";
                            }
                        }

                        if (ConsultaMotorista.tipomot.Trim() == "AGREGADO" || ConsultaMotorista.tipomot.Trim() == "TERCEIRO")
                        {
                            txtCodVeiculo.Text = ConsultaMotorista.codvei;
                            txtFilialVeicCNT.Text = ConsultaMotorista.nucleo; 
                            txtPlaca.Text = ConsultaMotorista.placa;
                            txtVeiculoTipo.Text = ConsultaMotorista.tipomot;
                            txtTipoVeiculo.Text = ConsultaMotorista.tipoveiculo;
                            txtReboque1.Text = ConsultaMotorista.reboque1;
                            txtReboque2.Text = ConsultaMotorista.reboque2;

                            var codigoAgregado = txtCodVeiculo.Text.Trim();

                            var objVeiculo = new Domain.ConsultaVeiculo
                            {
                                codvei = codigoAgregado
                            };
                            var ConsultaVeiculo = DAL.UsersDAL.CheckVeiculo(objVeiculo);
                            if (ConsultaVeiculo != null)
                            {
                                txtCRLVVeiculo.Text = ConsultaVeiculo.venclicenciamento;
                                txtCarreta.Text = ConsultaVeiculo.tiporeboque;
                                txtTecnologia.Text = ConsultaVeiculo.rastreador;
                                txtRastreamento.Text = ConsultaVeiculo.rastreamento;
                                txtConjunto.Text = ConsultaVeiculo.tipocarreta;
                                txtCodProprietario.Text = ConsultaVeiculo.codtra;
                                txtProprietario.Text = ConsultaVeiculo.transp;

                            }
                        }
                        // pesquisar primeiro reboque
                        if (txtReboque1.Text.Trim() != "")
                        {
                            var placaReboque1 = txtReboque1.Text.Trim();

                            var objCarreta = new Domain.ConsultaReboque
                            {
                                placacarreta = placaReboque1
                            };
                            var ConsultaReboque = DAL.UsersDAL.CheckReboque(objCarreta);
                            if (ConsultaReboque != null)
                            {                                 
                                txtCRLVReb1.Text = ConsultaReboque.licenciamento.Trim().ToString();
                            }
                        }

                        // pesquisar segundo reboque
                        if (txtReboque2.Text.Trim() != "")
                        {
                            var placaReboque2 = txtReboque2.Text.Trim();

                            var objCarreta = new Domain.ConsultaReboque
                            {
                                placacarreta = placaReboque2
                            };
                            var ConsultaReboque = DAL.UsersDAL.CheckReboque(objCarreta);
                            if (ConsultaReboque != null)
                            {
                                txtCRLVReb2.Text = ConsultaReboque.licenciamento.Trim().ToString();
                            }
                        }
                                               
                        txtCodVeiculo.Focus();
                    }
                }
                else
                {
                   
                    string nomeUsuario = txtUsuCadastro.Text;                   

                    string linha1 = "Olá, " + nomeUsuario + "!";
                    string linha2 = "Motorista " + codigo + ", não cadastrado no sistema.";
                    string linha3 = "Verifique o código digitado: " + codigo + ".";
                    //string linha4 = "Unidade: " + unidade + ". Por favor, verifique.";

                    // Concatenando as linhas com '\n' para criar a mensagem
                    string mensagem = $"{linha1}\n{linha2}\n{linha3}";

                    string mensagemCodificada = HttpUtility.JavaScriptStringEncode(mensagem);
                    //// Gerando o script JavaScript para exibir o alerta
                    string script = $"alert('{mensagemCodificada}');";

                    //// Registrando o script para execução no lado do cliente
                    ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", script, true);

                    txtCodMotorista.Text = "";
                    txtCodMotorista.Focus();
                   
                }

            }
        }

        protected void btnPesquisarVeiculo_Click(object sender, EventArgs e)
        {
            if (txtCodVeiculo.Text.Trim() == "")
            {
                string nomeUsuario = txtUsuCadastro.Text;

                string linha1 = "Olá, " + nomeUsuario + "!";
                string linha2 = "Por favor, digite o código do veículo.";

                // Concatenando as linhas com '\n' para criar a mensagem
                string mensagem = $"{linha1}\n{linha2}";

                string mensagemCodificada = HttpUtility.JavaScriptStringEncode(mensagem);
                // Gerando o script JavaScript para exibir o alerta
                string script = $"alert('{mensagemCodificada}');";

                // Registrando o script para execução no lado do cliente
                ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", script, true);
                txtCodVeiculo.Focus();

            }
            else
            {
                var codigo = txtCodVeiculo.Text.Trim();

                var obj = new Domain.ConsultaVeiculo
                {
                    codvei = codigo
                };


                var ConsultaVeiculo = DAL.UsersDAL.CheckVeiculo(obj);
                if (ConsultaVeiculo != null)
                {
                    if (ConsultaVeiculo.ativo_inativo.Trim() == "INATIVO")
                    {
                        string nomeUsuario = txtUsuCadastro.Text;
                        string placaVeiculo = ConsultaVeiculo.plavei;
                        string unidade = ConsultaVeiculo.nucleo;

                        string linha1 = "Olá, " + nomeUsuario + "!";
                        string linha2 = "Código " + codigo + ", excluido ou inativo no sistema.";
                        string linha3 = "Motorista: " + placaVeiculo + ".";
                        string linha4 = "Filial: " + unidade + ". Por favor, verifique.";

                        // Concatenando as linhas com '\n' para criar a mensagem
                        string mensagem = $"{linha1}\n{linha2}\n{linha3}\n{linha4}";

                        string mensagemCodificada = HttpUtility.JavaScriptStringEncode(mensagem);
                        // Gerando o script JavaScript para exibir o alerta
                        string script = $"alert('{mensagemCodificada}');";

                        // Registrando o script para execução no lado do cliente
                        ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", script, true);
                        txtCodVeiculo.Text = "";
                        txtCodVeiculo.Focus();
                    }
                    else
                    {
                        txtFilialVeicCNT.Text = ConsultaVeiculo.nucleo;
                        txtVeiculoTipo.Text = ConsultaVeiculo.tipoveiculo;
                        txtCET.Text = ConsultaVeiculo.venclicencacet;
                        txtCRLVVeiculo.Text = ConsultaVeiculo.venclicenciamento;
                        txtPlaca.Text = ConsultaVeiculo.plavei;
                        txtTipoVeiculo.Text = ConsultaVeiculo.tipvei;
                        txtReboque1.Text = ConsultaVeiculo.reboque1;
                        txtReboque2.Text = ConsultaVeiculo.reboque2;
                        txtCarreta.Text = ConsultaVeiculo.tiporeboque;
                        txtTecnologia.Text = ConsultaVeiculo.rastreador;
                        txtRastreamento.Text = ConsultaVeiculo.rastreamento;
                        txtConjunto.Text = ConsultaVeiculo.tipocarreta;
                        txtCodProprietario.Text = ConsultaVeiculo.codtra;
                        txtProprietario.Text = ConsultaVeiculo.transp;

                        // pesquisar primeiro reboque
                        if (txtReboque1.Text.Trim() != "")
                        {
                            var placaReboque1 = txtReboque1.Text.Trim();

                            var objCarreta = new Domain.ConsultaReboque
                            {
                                placacarreta = placaReboque1
                            };
                            var ConsultaReboque = DAL.UsersDAL.CheckReboque(objCarreta);
                            if (ConsultaReboque != null)
                            {
                                txtCRLVReb1.Text = ConsultaReboque.licenciamento.Trim().ToString();
                            }
                        }

                        // pesquisar segundo reboque
                        if (txtReboque2.Text.Trim() != "")
                        {
                            var placaReboque2 = txtReboque2.Text.Trim();

                            var objCarreta = new Domain.ConsultaReboque
                            {
                                placacarreta = placaReboque2
                            };
                            var ConsultaReboque = DAL.UsersDAL.CheckReboque(objCarreta);
                            if (ConsultaReboque != null)
                            {
                                txtCRLVReb2.Text = ConsultaReboque.licenciamento.Trim().ToString();
                            }
                        }
                        txtCodFrota.Text = "";
                        txtCodFrota.Focus();
                    }
                }
                else
                {

                    string nomeUsuario = txtUsuCadastro.Text;

                    string linha1 = "Olá, " + nomeUsuario + "!";
                    string linha2 = "Código/Frota " + codigo + ", não cadastrado no sistema.";
                    string linha3 = "Verifique o código/frota digitado: " + codigo + ".";
                    //string linha4 = "Unidade: " + unidade + ". Por favor, verifique.";

                    // Concatenando as linhas com '\n' para criar a mensagem
                    string mensagem = $"{linha1}\n{linha2}\n{linha3}";

                    string mensagemCodificada = HttpUtility.JavaScriptStringEncode(mensagem);
                    //// Gerando o script JavaScript para exibir o alerta
                    string script = $"alert('{mensagemCodificada}');";

                    //// Registrando o script para execução no lado do cliente
                    ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", script, true);

                    txtCodMotorista.Text = "";
                    txtCodMotorista.Focus();

                }

            }
        }
    }
}