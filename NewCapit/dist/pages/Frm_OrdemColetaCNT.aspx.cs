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
using System.Collections;
using System.Data;

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
                //PreencherComboStatus();
                PreencherNumColeta();
                CarregaFoto();
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
        
        protected void rptColetas_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DropDownList ddlStatus = (DropDownList)e.Item.FindControl("ddlStatus");

                if (ddlStatus != null)
                {
                    string query = "SELECT cod_status, ds_status FROM tb_status";

                    using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
                    {
                        try
                        {
                            conn.Open();
                            SqlCommand cmd = new SqlCommand(query, conn);
                            SqlDataReader reader = cmd.ExecuteReader();

                            ddlStatus.DataSource = reader;
                            ddlStatus.DataTextField = "ds_status";
                            ddlStatus.DataValueField = "cod_status";
                            ddlStatus.DataBind();
                            ddlStatus.Items.Insert(0, new ListItem("", "0"));

                            reader.Close();
                        }
                        catch (Exception ex)
                        {
                            // você pode exibir esse erro em um label, se quiser
                            Response.Write("Erro: " + ex.Message);
                        }
                    }
                }
            }
        }

        protected void rptColetas_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Atualizar")
            {
                string carga = e.CommandArgument.ToString();

                // Recuperar os controles de dentro do item
                TextBox txtCVA = (TextBox)e.Item.FindControl("txtCVA");
                TextBox txtGate = (TextBox)e.Item.FindControl("txtGate");
                DropDownList ddlStatus = (DropDownList)e.Item.FindControl("ddlStatus");
                TextBox txtChegadaOrigem = (TextBox)e.Item.FindControl("txtChegadaOrigem");
                TextBox txtSaidaOrigem = (TextBox)e.Item.FindControl("txtSaidaOrigem");
                TextBox txtAgCarreg = (TextBox)e.Item.FindControl("txtAgCarreg");
                TextBox txtChegadaDestino = (TextBox)e.Item.FindControl("txtChegadaDestino");
                TextBox txtEntrada = (TextBox)e.Item.FindControl("txtEntrada");
                TextBox txtSaidaPlanta = (TextBox)e.Item.FindControl("txtSaidaPlanta");
                TextBox txtDentroPlanta = (TextBox)e.Item.FindControl("txtDentroPlanta");
                TextBox txtEsperaGate = (TextBox)e.Item.FindControl("txtEsperaGate");
                // continue com os demais campos que quiser atualizar...
               
                // Exemplo: atualizando no banco
                using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
                {
                    string query = @"UPDATE tbcargas SET 
                                cva = @cva, 
                                gate = @gate, 
                                status = @status, 
                                chegadaorigem = @chegadaorigem, 
                                saidaorigem = @saidaorigem,
                                tempoagcarreg = @tempoagcarreg,
                                chegadadestino = @chegadadestino,
                                entradaplanta = @entradaplanta,
                                saidaplanta = @saidaplanta,
                                tempodentroplanta = @tempodentroplanta,
                                idviagem=@idviagem,
                                codmot=@codmot,
                                frota=@frota,
                                tempoesperagate=@tempoesperagate
                                WHERE carga = @carga";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@carga", carga);
                    cmd.Parameters.AddWithValue("@cva", txtCVA.Text.Trim());
                    cmd.Parameters.AddWithValue("@gate", DateTime.Parse(txtGate.Text.Trim()).ToString("dd/MM/yyyy HH:mm"));
                    cmd.Parameters.AddWithValue("@status", ddlStatus.SelectedItem.Text);
                    cmd.Parameters.AddWithValue("@chegadaorigem", DateTime.Parse(txtChegadaOrigem.Text.Trim()).ToString("dd/MM/yyyy HH:mm"));
                    cmd.Parameters.AddWithValue("@saidaorigem", DateTime.Parse(txtSaidaOrigem.Text.Trim()).ToString("dd/MM/yyyy HH:mm"));
                    cmd.Parameters.AddWithValue("@tempoagcarreg", txtAgCarreg.Text.Trim());
                    cmd.Parameters.AddWithValue("@chegadadestino", DateTime.Parse(txtChegadaDestino.Text.Trim()).ToString("dd/MM/yyyy HH:mm"));
                    cmd.Parameters.AddWithValue("@entradaplanta", DateTime.Parse(txtEntrada.Text.Trim()).ToString("dd/MM/yyyy HH:mm"));
                    cmd.Parameters.AddWithValue("@saidaplanta", DateTime.Parse(txtSaidaPlanta.Text.Trim()).ToString("dd/MM/yyyy HH:mm"));
                    cmd.Parameters.AddWithValue("@tempodentroplanta", txtDentroPlanta.Text.Trim());
                    cmd.Parameters.AddWithValue("@idviagem", novaColeta.Text.Trim());
                    cmd.Parameters.AddWithValue("@codmot", txtCodMotorista.Text.Trim());
                    cmd.Parameters.AddWithValue("@frota", txtCodFrota.Text.Trim());
                    cmd.Parameters.AddWithValue("@tempoesperagate", txtEsperaGate.Text.Trim());
                    // continue os parâmetros conforme seu banco

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    
                    string linha1 = "Coleta " + carga + ", cadastrado no sistema com sucesso.";
                    //string linha3 = "Verifique o código digitado: " + codigo + ".";
                    //string linha4 = "Unidade: " + unidade + ". Por favor, verifique.";

                    // Concatenando as linhas com '\n' para criar a mensagem
                    string mensagem = $"{linha1}";

                    string mensagemCodificada = HttpUtility.JavaScriptStringEncode(mensagem);
                    //// Gerando o script JavaScript para exibir o alerta
                    string script = $"alert('{mensagemCodificada}');";

                    //// Registrando o script para execução no lado do cliente
                    ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", script, true);
                }

                // Após atualizar, recarregar os dados no Repeater
                AtualizarColetasVisiveis();
            }
        }
        public void CarregaFoto()
        {
            var codigo = txtCodMotorista.Text.Trim();
           
            var obj = new Domain.ConsultaMotorista
            {
                codmot = codigo
            };
            var ConsultaMotorista = DAL.UsersDAL.CheckMotorista(obj);
            if (ConsultaMotorista != null)
            {
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
                                txtOpacidade.Text = ConsultaVeiculo.vencimentolaudofumaca;
                                txtCRLVVeiculo.Text = ConsultaVeiculo.venclicenciamento;
                                txtCET.Text = ConsultaVeiculo.venclicencacet;
                                txtCarreta.Text = ConsultaVeiculo.tiporeboque;
                                txtTecnologia.Text = ConsultaVeiculo.rastreador;
                                txtRastreamento.Text = ConsultaVeiculo.rastreamento;
                                txtConjunto.Text = ConsultaVeiculo.tipocarreta;
                                txtCodProprietario.Text = ConsultaVeiculo.codtra;
                                txtProprietario.Text = ConsultaVeiculo.transp;

                            }
                        }
                        else if (ConsultaMotorista.tipomot.Trim() == "FUNCIONÁRIO")
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
                                txtOpacidade.Text = ConsultaVeiculo.vencimentolaudofumaca;
                                txtCRLVVeiculo.Text = ConsultaVeiculo.venclicenciamento;
                                txtCET.Text = ConsultaVeiculo.venclicencacet;
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

        protected void bntPesquisaColeta_Click(object sender, EventArgs e)
        {
            string searchTerm;
            searchTerm = txtColeta.Text ;
            CarregarColetas(searchTerm);
            txtColeta.Text = string.Empty;

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
                        txtOpacidade.Text = ConsultaVeiculo.vencimentolaudofumaca;
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

        protected void btnPesquisarContato_Click(object sender, EventArgs e)
        {
            if (txtCodFrota.Text.Trim() == "")
            {
                string nomeUsuario = txtUsuCadastro.Text;

                string linha1 = "Olá, " + nomeUsuario + "!";
                string linha2 = "Por favor, digite o código do contato corporativo.";

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
                var codigo = txtCodFrota.Text.Trim();

                var obj = new Domain.ConsultaContato
                {
                    veiculo = codigo
                };


                var ConsultaContato = DAL.UsersDAL.CheckContato(obj);
                if (ConsultaContato != null)
                {
                    txtCodFrota.Text = ConsultaContato.veiculo.ToString();
                    txtFoneCorp.Text = ConsultaContato.numero.ToString();

                    txtColeta.Text = "";
                    txtColeta.Focus();
                   
                }
                else
                {

                    string nomeUsuario = txtUsuCadastro.Text;

                    string linha1 = "Olá, " + nomeUsuario + "!";
                    string linha2 = "Código " + codigo + ", não cadastrado no sistema.";
                    string linha3 = "Verifique o código digitado: " + codigo + ".";                    

                    // Concatenando as linhas com '\n' para criar a mensagem
                    string mensagem = $"{linha1}\n{linha2}\n{linha3}";

                    string mensagemCodificada = HttpUtility.JavaScriptStringEncode(mensagem);
                    //// Gerando o script JavaScript para exibir o alerta
                    string script = $"alert('{mensagemCodificada}');";

                    //// Registrando o script para execução no lado do cliente
                    ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", script, true);

                    txtCodFrota.Text = "";
                    txtCodFrota.Focus();

                }

            }
        }

        private void CarregarColetas(string searchTerm = "")
        {
            // Obtem os dados atuais (novos dados)
            var novosDados = DAL.ConCargas.FetchDataTableColetas2(searchTerm);

            // Verifica se há dados anteriores no ViewState
            DataTable dadosAtuais = ViewState["Coletas"] as DataTable;

            if (dadosAtuais == null)
            {
                // Se não havia dados, inicializa com os novos
                dadosAtuais = novosDados.Clone(); // cria com a mesma estrutura
            }

            // Adiciona os novos dados aos dados atuais
            foreach (DataRow row in novosDados.Rows)
            {
                dadosAtuais.ImportRow(row);
            }

            // Atualiza o ViewState
            ViewState["Coletas"] = dadosAtuais;

            // Alimenta o Repeater com todos os dados acumulados
            rptColetas.DataSource = dadosAtuais;
            rptColetas.DataBind();
        }

        protected void btnSalvar1_Click(object sender, EventArgs e)
        {
            string query = @"INSERT INTO tbcarregamentos (
                            num_carregamento, codmotorista, nucleo, tipomot, valtoxicologico, venccnh, valgr, foto, nomemotorista, cpf, 
                            cartaopedagio, valcartao, foneparticular, veiculo, veiculotipo, filialveiculo, valcet, valcrlvveiculo, 
                            valcrlvreboque1, valcrlvreboque2, placa, tipoveiculo, reboque1, reboque2, carreta, tecnologia, rastreamento, 
                            tipocarreta, codtra, transportadora, codcontato, fonecorporativo, empresa
                             ) VALUES (
                            @num_carregamento, @codmotorista, @nucleo, @tipomot, @valtoxicologico, @venccnh, @valgr, @foto, @nomemotorista, @cpf, 
                            @cartaopedagio, @valcartao, @foneparticular, @veiculo, @veiculotipo, @filialveiculo, @valcet, @valcrlvveiculo, 
                            @valcrlvreboque1, @valcrlvreboque2, @placa, @tipoveiculo, @reboque1, @reboque2, @carreta, @tecnologia, @rastreamento, 
                            @tipocarreta, @codtra, @transportadora, @codcontato, @fonecorporativo, @empresa
                             )";

            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@num_carregamento", novaColeta.Text);
                cmd.Parameters.AddWithValue("@codmotorista", txtCodMotorista.Text);
                cmd.Parameters.AddWithValue("@nucleo", txtFilialMot.Text);
                cmd.Parameters.AddWithValue("@tipomot", txtTipoMot.Text);
                cmd.Parameters.AddWithValue("@valtoxicologico", txtExameToxic.Text);
                cmd.Parameters.AddWithValue("@venccnh", txtCNH.Text);
                cmd.Parameters.AddWithValue("@valgr", txtLibGR.Text);
                cmd.Parameters.AddWithValue("@foto", fotoMotorista); // se for um Upload, trate o arquivo separadamente
                cmd.Parameters.AddWithValue("@nomemotorista", txtNomMot.Text);
                cmd.Parameters.AddWithValue("@cpf", txtCPF.Text);
                cmd.Parameters.AddWithValue("@cartaopedagio", txtCartao.Text);
                cmd.Parameters.AddWithValue("@valcartao", txtValCartao.Text);
                cmd.Parameters.AddWithValue("@foneparticular", txtCelular.Text);
                cmd.Parameters.AddWithValue("@veiculo", txtCodVeiculo.Text);
                cmd.Parameters.AddWithValue("@veiculotipo", txtVeiculoTipo.Text);
                cmd.Parameters.AddWithValue("@filialveiculo", txtFilialVeicCNT.Text);
                cmd.Parameters.AddWithValue("@valcet", txtCET.Text);
                cmd.Parameters.AddWithValue("@valcrlvveiculo", txtCRLVVeiculo.Text);
                cmd.Parameters.AddWithValue("@valcrlvreboque1", txtCRLVReb1.Text);
                cmd.Parameters.AddWithValue("@valcrlvreboque2", txtCRLVReb2.Text);
                cmd.Parameters.AddWithValue("@placa", txtPlaca.Text);
                cmd.Parameters.AddWithValue("@tipoveiculo", txtTipoVeiculo.Text);
                cmd.Parameters.AddWithValue("@reboque1", txtReboque1.Text);
                cmd.Parameters.AddWithValue("@reboque2", txtReboque2.Text);
                cmd.Parameters.AddWithValue("@carreta", txtCarreta.Text);
                cmd.Parameters.AddWithValue("@tecnologia", txtTecnologia.Text);
                cmd.Parameters.AddWithValue("@rastreamento", txtRastreamento.Text);
                cmd.Parameters.AddWithValue("@tipocarreta", txtConjunto.Text);
                cmd.Parameters.AddWithValue("@codtra", txtCodProprietario.Text);
                cmd.Parameters.AddWithValue("@transportadora", txtProprietario.Text);
                cmd.Parameters.AddWithValue("@codcontato", txtCodFrota.Text);
                cmd.Parameters.AddWithValue("@fonecorporativo", txtFoneCorp.Text);
                cmd.Parameters.AddWithValue("@empresa", txtFilial.Text);

                try
                {
                    string nomeUsuario = txtUsuCadastro.Text;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    string linha1 = "Olá, " + nomeUsuario + "!";
                    string linha2 = "Carregamento cadastrado no sistema com sucesso.";
                    //string linha3 = "Verifique o código/frota digitado: " + codigo + ".";
                    //string linha4 = "Unidade: " + unidade + ". Por favor, verifique.";

                    // Concatenando as linhas com '\n' para criar a mensagem
                    string mensagem = $"{linha1}\n{linha2}";

                    string mensagemCodificada = HttpUtility.JavaScriptStringEncode(mensagem);
                    //// Gerando o script JavaScript para exibir o alerta
                    string script = $"alert('{mensagemCodificada}');";

                    //// Registrando o script para execução no lado do cliente
                    ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", script, true);
                }
                catch (Exception ex)
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    string linha1 = "Erro ao salvar: " + ex.Message;
                    //string linha2 = "Carregamento cadastrado no sistema com sucesso.";
                    //string linha3 = "Verifique o código/frota digitado: " + codigo + ".";
                    //string linha4 = "Unidade: " + unidade + ". Por favor, verifique.";

                    // Concatenando as linhas com '\n' para criar a mensagem
                    string mensagem = $"{linha1}";

                    string mensagemCodificada = HttpUtility.JavaScriptStringEncode(mensagem);
                    //// Gerando o script JavaScript para exibir o alerta
                    string script = $"alert('{mensagemCodificada}');";

                    //// Registrando o script para execução no lado do cliente
                    ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", script, true);
                    //lblMensagem.Text = "Erro ao salvar: " + ex.Message;
                }
            }
        }


        private void AtualizarColetasVisiveis()
        {
            DataTable dadosAtuais = ViewState["Coletas"] as DataTable;

            if (dadosAtuais != null && dadosAtuais.Rows.Count > 0)
            {
                // Obtem os "carga" visíveis
                var cargasVisiveis = dadosAtuais.AsEnumerable()
                    .Select(r => r["carga"].ToString())
                    .Distinct()
                    .ToList();

                // Consulta novamente apenas essas coletas no banco
                var dadosAtualizados = DAL.ConCargas.FetchDataTableColetasPorCargas(cargasVisiveis);

                // Atualiza o ViewState com os novos dados
                ViewState["Coletas"] = dadosAtualizados;

                // Atualiza o Repeater
                rptColetas.DataSource = dadosAtualizados;
                rptColetas.DataBind();
            }
        }
        

        //protected void LimparColetas_Click(object sender, EventArgs e)
        //{
        //    ViewState["Coletas"] = null;
        //    rptColetas.DataSource = null;
        //    rptColetas.DataBind();
        //}

    }



}