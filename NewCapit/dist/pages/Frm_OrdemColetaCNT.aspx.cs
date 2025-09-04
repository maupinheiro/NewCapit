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
using System.Web.UI.HtmlControls;
using System.Drawing;
using System.Configuration;

namespace NewCapit.dist.pages
{
    public partial class Frm_OrdemColetaCNT : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conexao"].ToString());
        public string fotoMotorista;
        string codmot, caminhofoto;
        private object lblVeiculo;
        string motoristaNaTela;
        string motoristaNovaColeta;
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
                if (txtCodMotorista.Text == "")
                {
                    fotoMotorista = "../../fotos/usuario.JPG";
                }

                DateTime dataHoraAtual = DateTime.Now;
                lblDtCadastro.Text = dataHoraAtual.ToString("dd/MM/yyyy HH:mm");
                //PreencherComboStatus();
                PreencherNumColeta();
                PreencherNumCargaVazia();
                PreencherClienteInicial();
                PreencherClienteFinal();
                PreencherVeiculosCNT();
            }
            CarregaFoto();
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
        private void PreencherVeiculosCNT()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT id, descricao FROM tbtiposveiculoscnt order by descricao";

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
                    //ddlVeiculosCNT.DataSource = reader;
                    //ddlVeiculosCNT.DataTextField = "descricao";  // Campo que será mostrado no ComboBox
                    //ddlVeiculosCNT.DataValueField = "id";  // Campo que será o valor de cada item                    
                    //ddlVeiculosCNT.DataBind();  // Realiza o binding dos dados                   
                    //ddlVeiculosCNT.Items.Insert(0, new ListItem("Selecione...", "0"));
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
        private void PreencherClienteInicial()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT id, codcli, nomcli FROM tbclientes order by nomcli";

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
                    ddlCliInicial.DataSource = reader;
                    ddlCliInicial.DataTextField = "nomcli";  // Campo que será mostrado no ComboBox
                    ddlCliInicial.DataValueField = "codcli";  // Campo que será o valor de cada item                    
                    ddlCliInicial.DataBind();  // Realiza o binding dos dados                   
                    ddlCliInicial.Items.Insert(0, new ListItem("Selecione...", "0"));
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
        private void PreencherClienteFinal()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT id, codcli, nomcli FROM tbclientes order by nomcli";

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
                    ddlCliFinal.DataSource = reader;
                    ddlCliFinal.DataTextField = "nomcli";  // Campo que será mostrado no ComboBox
                    ddlCliFinal.DataValueField = "codcli";  // Campo que será o valor de cada item                    
                    ddlCliFinal.DataBind();  // Realiza o binding dos dados                   
                    ddlCliFinal.Items.Insert(0, new ListItem("Selecione...", "0"));
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
        protected void rptColetas_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DropDownList ddlStatus = (DropDownList)e.Item.FindControl("ddlStatus");

                if (ddlStatus != null)
                {
                    // Aqui pegamos o texto do status (ds_status) vindo do banco
                    string statusTexto = DataBinder.Eval(e.Item.DataItem, "status")?.ToString();

                    string query = "SELECT cod_status, ds_status FROM tb_status";

                    using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
                    {
                        try
                        {
                            conn.Open();
                            SqlCommand cmd = new SqlCommand(query, conn);
                            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                            DataTable dtStatus = new DataTable();
                            adapter.Fill(dtStatus);

                            ddlStatus.DataSource = dtStatus;
                            ddlStatus.DataTextField = "ds_status";
                            ddlStatus.DataValueField = "cod_status";
                            ddlStatus.DataBind();

                            // Se o texto do status estiver presente na lista, seleciona
                            ListItem itemSelecionado = ddlStatus.Items.FindByText(statusTexto);
                            if (itemSelecionado != null)
                            {
                                itemSelecionado.Selected = true;
                            }
                            else if (!string.IsNullOrEmpty(statusTexto))
                            {
                                // Se não estiver na lista, adiciona ele no topo
                                ddlStatus.Items.Insert(0, new ListItem(statusTexto, "0"));
                                ddlStatus.SelectedIndex = 0;
                            }
                        }
                        catch (Exception ex)
                        {
                            Response.Write("Erro ao carregar status: " + ex.Message);
                        }
                    }
                }


                string previsaoStr = DataBinder.Eval(e.Item.DataItem, "previsao")?.ToString();
                string dataHoraStr = DataBinder.Eval(e.Item.DataItem, "data_hora")?.ToString();
                string status = DataBinder.Eval(e.Item.DataItem, "status")?.ToString();

                Label lblAtendimento = (Label)e.Item.FindControl("lblAtendimento");
                HtmlTableCell tdAtendimento = (HtmlTableCell)e.Item.FindControl("tdAtendimento");

                DateTime previsao, dataHora;
                DateTime agora = DateTime.Now;

                if (DateTime.TryParse(previsaoStr, out previsao) && DateTime.TryParse(dataHoraStr, out dataHora))
                {
                    DateTime dataPrevisao = previsao.Date;
                    DateTime dataHoraComparacao = new DateTime(
                        dataPrevisao.Year, dataPrevisao.Month, dataPrevisao.Day,
                        dataHora.Hour, dataHora.Minute, dataHora.Second
                    );

                    if (dataHoraComparacao < agora && (status == "Concluído" || status == "PENDENTE"))
                    {
                        lblAtendimento.Text = "Atrasado";
                        tdAtendimento.Attributes["style"] = "background-color:red; color:white; font-weight:bold;";
                    }
                    else if (dataHoraComparacao.Date == agora.Date && dataHoraComparacao.TimeOfDay <= agora.TimeOfDay
                             && (status == "Concluído" || status == "Pendente"))
                    {
                        lblAtendimento.Text = "No Prazo";
                        tdAtendimento.Attributes["style"] = "background-color:green; color:white; font-weight:bold;";
                    }
                    else if (dataHoraComparacao > agora && status == "Concluído")
                    {
                        lblAtendimento.Text = "Antecipado";
                        tdAtendimento.Attributes["style"] = "background-color:orange; color:white; font-weight:bold;";
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
                if (txtCVA.Text != string.Empty)
                {
                    // Exemplo: atualizando no banco
                    using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
                    {
                        string query = @"UPDATE tbcargas SET 
                                emissao=@emissao,
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
                        cmd.Parameters.AddWithValue("@gate", DateTime.Parse(txtGate.Text.Trim()).ToString("yyyy-MM-dd HH:mm"));
                        cmd.Parameters.AddWithValue("@status", ddlStatus.SelectedItem.Text);
                        cmd.Parameters.AddWithValue("@chegadaorigem", DateTime.Parse(txtChegadaOrigem.Text.Trim()).ToString("yyyy-MM-dd HH:mm"));
                        cmd.Parameters.AddWithValue("@saidaorigem", DateTime.Parse(txtSaidaOrigem.Text.Trim()).ToString("yyyy-MM-dd HH:mm"));
                        cmd.Parameters.AddWithValue("@tempoagcarreg", txtAgCarreg.Text.Trim());
                        cmd.Parameters.AddWithValue("@chegadadestino", DateTime.Parse(txtChegadaDestino.Text.Trim()).ToString("yyyy-MM-dd HH:mm"));
                        cmd.Parameters.AddWithValue("@entradaplanta", DateTime.Parse(txtEntrada.Text.Trim()).ToString("yyyy-MM-dd HH:mm"));
                        cmd.Parameters.AddWithValue("@saidaplanta", DateTime.Parse(txtSaidaPlanta.Text.Trim()).ToString("yyyy-MM-dd HH:mm"));
                        cmd.Parameters.AddWithValue("@tempodentroplanta", txtDentroPlanta.Text.Trim());
                        cmd.Parameters.AddWithValue("@idviagem", novaColeta.Text.Trim());
                        cmd.Parameters.AddWithValue("@codmot", txtCodMotorista.Text.Trim());
                        cmd.Parameters.AddWithValue("@frota", txtCodFrota.Text.Trim());
                        cmd.Parameters.AddWithValue("@tempoesperagate", txtEsperaGate.Text.Trim());
                        cmd.Parameters.AddWithValue("@emissao", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));


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
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "MensagemDeAlerta", script, true);
                    }

                    // Após atualizar, recarregar os dados no Repeater
                    AtualizarColetasVisiveis();
                }
                else
                {
                    string linha1 = "Nao é possível atrelar a Coleta " + carga + " a esse Carregamento.";
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

            }
            else if (e.CommandName == "Remover")
            {
                string carga = e.CommandArgument.ToString();
                DataTable dados = ViewState["Coletas"] as DataTable;

                if (dados != null)
                {
                    DataRow[] linhas = dados.Select($"carga = '{carga}'");
                    foreach (DataRow linha in linhas)
                    {
                        dados.Rows.Remove(linha);
                    }

                    dados.AcceptChanges();

                    ViewState["Coletas"] = dados;
                    rptColetas.DataSource = dados;
                    rptColetas.DataBind();
                }
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
                if (ConsultaMotorista.status.Trim() != "INATIVO")
                {
                    if (txtCodMotorista.Text.Trim() != "")
                    {
                        fotoMotorista = ConsultaMotorista.caminhofoto.Trim().ToString();

                        if (!File.Exists(fotoMotorista))
                        {
                            fotoMotorista = ConsultaMotorista.caminhofoto.Trim().ToString();
                        }
                        else
                        {
                            fotoMotorista = "../../fotos/usuario.jpg";
                        }
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
                    else
                    {
                        txtFilialMot.Text = ConsultaMotorista.nucleo;
                        txtTipoMot.Text = ConsultaMotorista.tipomot;
                        txtExameToxic.Text = ConsultaMotorista.venceti;
                        txtCNH.Text = ConsultaMotorista.venccnh.ToString("dd/MM/yyyy");
                        txtLibGR.Text = ConsultaMotorista.validade;
                        txtNomMot.Text = ConsultaMotorista.nommot;
                        txtCPF.Text = ConsultaMotorista.cpf;
                        txtCartao.Text = ConsultaMotorista.cartaomot;
                        txtValCartao.Text = ConsultaMotorista.venccartao;
                        txtCelular.Text = ConsultaMotorista.fone2;
                        txtFuncao.Text = ConsultaMotorista.cargo;
                        txtCodTransportadora.Text = ConsultaMotorista.codtra;
                        txtTransportadora.Text = ConsultaMotorista.transp;
                        
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
                                DateTime dataConvertida = DateTime.ParseExact(ConsultaVeiculo.vencimentolaudofumaca, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                                string dataOpacidade = dataConvertida.ToString("dd/MM/yyyy");
                                txtOpacidade.Text = dataOpacidade; //ConsultaVeiculo.vencimentolaudofumaca;
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
                            txtCodVeiculo.Text = ConsultaMotorista.frota;
                            txtFilialVeicCNT.Text = ConsultaMotorista.nucleo;
                            txtPlaca.Text = ConsultaMotorista.placa;
                            txtVeiculoTipo.Text = "FROTA"; // ConsultaMotorista.tipomot;
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
                                DateTime dataConvertida = DateTime.ParseExact(ConsultaVeiculo.vencimentolaudofumaca, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                                string dataOpacidade = dataConvertida.ToString("dd/MM/yyyy");
                                txtOpacidade.Text = dataOpacidade;
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
            if (txtColeta.Text.Trim() == "")
            {
                string nomeUsuario = txtUsuCadastro.Text;

                string linha1 = "Olá, " + nomeUsuario + "!";
                string linha2 = "Por favor, digite o número do CVA.";

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

                string codigoPlaca = txtColeta.Text.Trim();
                string strConn = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    string query = "SELECT carga, cliorigem, clidestino, status, codmot FROM tbcargas WHERE cva = @Codigo";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Codigo", codigoPlaca);
                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                if (reader["status"].ToString() == "PENDENTE" || reader["status"].ToString() == "Pendente")
                                {
                                    string searchTerm;
                                    searchTerm = txtColeta.Text.Trim();
                                   
                                    CarregarColetas(searchTerm);
                                    txtColeta.Text = string.Empty;
                                }
                                else
                                {
                                    string nomeUsuario = txtUsuCadastro.Text;

                                    string linha1 = "Olá, " + nomeUsuario + "!";
                                    string linha2 = "Situação da coleta: " + reader["carga"].ToString() + " (" + reader["status"].ToString() + ").";
                                    string linha3 = "Local de Coleta: " + reader["cliorigem"].ToString() + " - Local de Entrega: " + reader["clidestino"].ToString() + ".";
                                    string linha4 = "Não permite inclusão. Verifique o número digitado.";
                                    // Concatenando as linhas com '\n' para criar a mensagem
                                    string mensagem = $"{linha1}\n{linha2}\n{linha3}\n{linha4}";

                                    string mensagemCodificada = HttpUtility.JavaScriptStringEncode(mensagem);
                                    //// Gerando o script JavaScript para exibir o alerta
                                    string script = $"alert('{mensagemCodificada}');";

                                    //// Registrando o script para execução no lado do cliente
                                    ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", script, true);

                                    txtColeta.Text = "";
                                    txtColeta.Focus();
                                }

                            }
                            else
                            {
                                // se a coleta for = 1 abre o modal para colocar origem e destino

                                if (txtColeta.Text == "1")
                                {
                                    // Exemplo: abrir o modal ao carregar a página
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "abrirModal", "abrirModal();", true);
                                }
                                else
                                {
                                    string nomeUsuario = txtUsuCadastro.Text;

                                    string linha1 = "Olá, " + nomeUsuario + "!";
                                    string linha2 = "Coleta " + txtColeta.Text.Trim() + ", não encontrada. Verifique o número digitado.";

                                    // Concatenando as linhas com '\n' para criar a mensagem
                                    string mensagem = $"{linha1}\n{linha2}";

                                    string mensagemCodificada = HttpUtility.JavaScriptStringEncode(mensagem);
                                    //// Gerando o script JavaScript para exibir o alerta
                                    string script = $"alert('{mensagemCodificada}');";

                                    //// Registrando o script para execução no lado do cliente
                                    ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", script, true);

                                    txtColeta.Text = "";
                                    txtColeta.Focus();
                                }


                            }
                        }
                    }

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
                txtCodVeiculo.Text = "";
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
                        //txtCodFrota.Text = "";
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

                    txtCodVeiculo.Text = "";
                    txtCodVeiculo.Focus();

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

                    // Exemplo: abrir o modal ao carregar a página   
                    ScriptManager.RegisterStartupScript(this, GetType(), "abrirModal", "$('#telefoneModal').modal('show');", true);
                }

            }
        }
        private void CarregarColetas(string searchTerm = "")
        {
            var novosDados = DAL.ConCargas.FetchDataTableColetas2(searchTerm);

            DataTable dadosAtuais = ViewState["Coletas"] as DataTable ?? new DataTable();

            var carga = new Domain.ConsultaCarga
            {
                carga = searchTerm
            };
            var ConsultaCarga = DAL.ConCargas.CheckColetas(carga);

            if (ConsultaCarga != null)
            {
                motoristaNovaColeta = ConsultaCarga.codmot?.Trim() ?? "";
                motoristaNaTela = hdfCodMotorista.Value.Trim();
                if (motoristaNaTela != string.Empty)
                {
                    if (motoristaNaTela != motoristaNovaColeta)
                    {
                        string nomeUsuario = txtUsuCadastro.Text;
                        string mensagem = $"Olá, {nomeUsuario}!\nA coleta {searchTerm} pertence a um motorista diferente do que já está na viagem. Apenas coletas do mesmo motorista podem ser adicionadas.";
                        string script = $"alert('{HttpUtility.JavaScriptStringEncode(mensagem)}');";
                        ClientScript.RegisterStartupScript(this.GetType(), "MotoristaDiferenteAlerta", script, true);
                        txtColeta.Focus();
                        return;
                    }
                }
                else
                {
                    
                    if (ConsultaCarga.andamento == "PENDENTE" || ConsultaCarga.andamento == "Pendente" && ConsultaCarga.andamento != null)
                    {
                        if (ConsultaCarga.codmot == "" || ConsultaCarga.codmot == null)
                        {
                            // não tem motorista cadastrado
                            string nomeUsuario = txtUsuCadastro.Text;

                            string linha1 = "Olá, " + nomeUsuario + "!";
                            string linha2 = "CVA, não tem motorista lançado. Verifique, por favor!";

                            // Concatenando as linhas com '\n' para criar a mensagem
                            string mensagem = $"{linha1}\n{linha2}";

                            string mensagemCodificada = HttpUtility.JavaScriptStringEncode(mensagem);
                            // Gerando o script JavaScript para exibir o alerta
                            string script = $"alert('{mensagemCodificada}');";

                            // Registrando o script para execução no lado do cliente
                            ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", script, true);
                            txtColeta.Focus();
                            return;
                        }
                        else
                        {
                            txtCodMotorista.Text = ConsultaCarga.codmot.Trim();
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
                                    string nomeUsuario = txtUsuCadastro.Text;
                                    string razaoSocial = ConsultaMotorista.nommot;
                                    string unidade = ConsultaMotorista.nucleo;

                                    string linha1 = "Olá, " + nomeUsuario + "!";
                                    string linha2 = "Código " + codigo + ", excluido ou inativo no sistema.";
                                    string linha3 = "Motorista: " + txtCodMotorista.Text.Trim() + " - " + razaoSocial + ".";
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
                                else
                                {
                                    string nomeUsuario = txtUsuCadastro.Text;
                                    txtFilialMot.Text = ConsultaMotorista.nucleo;
                                    txtTipoMot.Text = ConsultaMotorista.tipomot;
                                    txtExameToxic.Text = ConsultaMotorista.venceti;
                                    txtFuncao.Text = ConsultaMotorista.cargo;
                                    txtCNH.Text = ConsultaMotorista.venccnh.ToString("dd/MM/yyyy");
                                    txtNomMot.Text = ConsultaMotorista.nommot;
                                    if (ConsultaMotorista.validade == "" || ConsultaMotorista.validade == null)
                                    {
                                        string linha1 = "Olá, " + nomeUsuario + "!";
                                        string linha2 = "O motorista " + txtCodMotorista.Text.Trim() + " - " + txtNomMot.Text.Trim() + ", não tem liberação de risco.";
                                        string linha3 = "Insira uma data de validade.";
                                        //string linha4 = "Unidade: " + unidade + ". Por favor, verifique.";
                                        // Concatenando as linhas com '\n' para criar a mensagem
                                        string mensagem = $"{linha1}\n{linha2}\n{linha3}";
                                        string mensagemCodificada = HttpUtility.JavaScriptStringEncode(mensagem);
                                        //// Gerando o script JavaScript para exibir o alerta
                                        string script = $"alert('{mensagemCodificada}');";
                                        //// Registrando o script para execução no lado do cliente
                                        ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", script, true);
                                        txtCodMotorista.Text = "";
                                        txtColeta.Focus();
                                        return;

                                    }
                                    else
                                    {
                                        txtLibGR.Text = ConsultaMotorista.validade;
                                    }

                                    // ddlMotorista.Items.Insert(0, new ListItem(ConsultaMotorista.nommot, ""));

                                    txtCPF.Text = ConsultaMotorista.cpf;
                                    txtCartao.Text = ConsultaMotorista.cartaomot;
                                    txtValCartao.Text = ConsultaMotorista.venccartao;
                                    txtCelular.Text = ConsultaMotorista.fone2;
                                    txtCodTransportadora.Text = ConsultaMotorista.codtra;
                                    txtTransportadora.Text = ConsultaMotorista.transp;

                                    if (ConsultaMotorista.tipomot.Trim() == "AGREGADO" || ConsultaMotorista.tipomot.Trim() == "TERCEIRO")
                                    {
                                        txtCodVeiculo.Text = ConsultaMotorista.codvei;
                                        txtFilialVeicCNT.Text = ConsultaMotorista.nucleo;
                                        txtPlaca.Text = ConsultaMotorista.placa;
                                        txtVeiculoTipo.Text = ConsultaMotorista.tipomot;
                                        txtTipoVeiculo.Text = ConsultaMotorista.tipoveiculo;
                                        txtReboque1.Text = ConsultaMotorista.reboque1;
                                        txtReboque2.Text = ConsultaMotorista.reboque2;
                                        // ETI.Visible = false;
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
                                        txtCodVeiculo.Text = ConsultaMotorista.frota;
                                        txtFilialVeicCNT.Text = ConsultaMotorista.nucleo;
                                        txtPlaca.Text = ConsultaMotorista.placa;
                                        txtVeiculoTipo.Text = "FROTA";
                                        txtTipoVeiculo.Text = ConsultaMotorista.tipoveiculo;
                                        txtReboque1.Text = ConsultaMotorista.reboque1;
                                        txtReboque2.Text = ConsultaMotorista.reboque2;
                                        //ETI.Visible = true;
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
                                        // pesquisar validade do Exame Toxicologico
                                        if (txtExameToxic.Text != "")
                                        {
                                            DateTime dataHoje = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                                            DateTime dataETI = Convert.ToDateTime(txtExameToxic.Text);

                                            TimeSpan diferencaETI = dataETI - dataHoje;
                                            // Agora você pode comparar a diferença
                                            if (diferencaETI.TotalDays < 30)
                                            {
                                                string diasExame = diferencaETI.TotalDays.ToString();
                                                txtExameToxic.Text = txtExameToxic.Text + " (" + diasExame + " dias)";
                                                txtExameToxic.BackColor = System.Drawing.Color.Khaki;
                                                txtExameToxic.ForeColor = System.Drawing.Color.OrangeRed;
                                                ////string nomeUsuario = txtUsuCadastro.Text;
                                                //string linha1 = "Olá, " + nomeUsuario + "!";
                                                //string linha2 = "O Exame Toxicologico do motorista " + txtNomMot.Text.Trim() + ", vence em menos de 30 dias.";
                                                //string linha3 = "Data de vencimento: " + dataETI.ToString("dd/MM/yyyy") + ".";
                                                ////string linha4 = "Unidade: " + unidade + ". Por favor, verifique.";
                                                //// Concatenando as linhas com '\n' para criar a mensagem
                                                //string mensagem = $"{linha1}\n{linha2}\n{linha3}";
                                                //string mensagemCodificada = HttpUtility.JavaScriptStringEncode(mensagem);
                                                ////// Gerando o script JavaScript para exibir o alerta
                                                //string script = $"alert('{mensagemCodificada}');";
                                                ////// Registrando o script para execução no lado do cliente
                                                //ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", script, true);
                                            }
                                            else if (diferencaETI.TotalDays <= 0)
                                            {
                                                txtExameToxic.BackColor = System.Drawing.Color.Red;
                                                txtExameToxic.ForeColor = System.Drawing.Color.White;
                                                // string nomeUsuario = txtUsuCadastro.Text;
                                                string linha1 = "Olá, " + nomeUsuario + "!";
                                                string linha2 = "O Exame Toxicologico do motorista " + txtCodMotorista.Text.Trim() + " - " + txtNomMot.Text.Trim() + ", está vencido.";
                                                string linha3 = "Data do vencimento: " + dataETI.ToString("dd/MM/yyyy") + ".";
                                                //string linha4 = "Unidade: " + unidade + ". Por favor, verifique.";
                                                // Concatenando as linhas com '\n' para criar a mensagem
                                                string mensagem = $"{linha1}\n{linha2}\n{linha3}";
                                                string mensagemCodificada = HttpUtility.JavaScriptStringEncode(mensagem);
                                                //// Gerando o script JavaScript para exibir o alerta
                                                string script = $"alert('{mensagemCodificada}');";
                                                //// Registrando o script para execução no lado do cliente
                                                ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", script, true);
                                                txtCodMotorista.Text = "";
                                                txtColeta.Focus();
                                                return;
                                            }
                                            else
                                            {
                                                if (txtExameToxic.Text == "")
                                                {
                                                    // string nomeUsuario = txtUsuCadastro.Text;
                                                    string linha1 = "Olá, " + nomeUsuario + "!";
                                                    string linha2 = "O motorista " + txtCodMotorista.Text.Trim() + " - " + txtNomMot.Text.Trim() + ", não tem Exame Toxicologico, lançado no sistema.";
                                                    string linha3 = "Verifique, por favor.";
                                                    //string linha4 = "Unidade: " + unidade + ". Por favor, verifique.";
                                                    // Concatenando as linhas com '\n' para criar a mensagem
                                                    string mensagem = $"{linha1}\n{linha2}\n{linha3}";
                                                    string mensagemCodificada = HttpUtility.JavaScriptStringEncode(mensagem);
                                                    //// Gerando o script JavaScript para exibir o alerta
                                                    string script = $"alert('{mensagemCodificada}');";
                                                    //// Registrando o script para execução no lado do cliente
                                                    ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", script, true);

                                                    txtColeta.Focus();
                                                    return;
                                                }

                                            }
                                        }
                                    }
                                    // Pesquisar validade da CNH
                                    if (txtCNH.Text != "")
                                    {
                                        DateTime dataHoje = Convert.ToDateTime(DateTime.Now.Date);
                                        DateTime dataCNH = Convert.ToDateTime(txtCNH.Text).Date; ;

                                        TimeSpan diferenca = dataCNH - dataHoje;
                                        // Agora você pode comparar a diferença
                                        if (diferenca.TotalDays < 30)
                                        {
                                            string diasCNH = diferenca.TotalDays.ToString();
                                            txtCNH.Text = txtCNH.Text + " (" + diasCNH + " dias)";
                                            txtCNH.BackColor = System.Drawing.Color.Khaki;
                                            txtCNH.ForeColor = System.Drawing.Color.OrangeRed;
                                            // string nomeUsuario = txtUsuCadastro.Text;
                                            //string linha1 = "Olá, " + nomeUsuario + "!";
                                            //string linha2 = "A CNH do motorista " + txtNomMot.Text.Trim() + ", vence em menos de 30 dias.";
                                            //string linha3 = "Data de vencimento: " + dataCNH.ToString("dd/MM/yyyy") + ".";
                                            ////string linha4 = "Unidade: " + unidade + ". Por favor, verifique.";
                                            //// Concatenando as linhas com '\n' para criar a mensagem
                                            //string mensagem = $"{linha1}\n{linha2}\n{linha3}";
                                            //string mensagemCodificada = HttpUtility.JavaScriptStringEncode(mensagem);
                                            ////// Gerando o script JavaScript para exibir o alerta
                                            //string script = $"alert('{mensagemCodificada}');";
                                            ////// Registrando o script para execução no lado do cliente
                                            //ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", script, true);
                                        }
                                        else if (diferenca.TotalDays <= 0)
                                        {
                                            txtCNH.BackColor = System.Drawing.Color.Red;
                                            txtCNH.ForeColor = System.Drawing.Color.White;
                                            // string nomeUsuario = txtUsuCadastro.Text;
                                            string linha1 = "Olá, " + nomeUsuario + "!";
                                            string linha2 = "A CNH do motorista " + txtCodMotorista.Text.Trim() + " - " + txtNomMot.Text.Trim() + ", está vencida.";
                                            string linha3 = "Data de vencimento: " + dataCNH.ToString("dd/MM/yyyy") + ".";
                                            //string linha4 = "Unidade: " + unidade + ". Por favor, verifique.";
                                            // Concatenando as linhas com '\n' para criar a mensagem
                                            string mensagem = $"{linha1}\n{linha2}\n{linha3}";
                                            string mensagemCodificada = HttpUtility.JavaScriptStringEncode(mensagem);
                                            //// Gerando o script JavaScript para exibir o alerta
                                            string script = $"alert('{mensagemCodificada}');";
                                            //// Registrando o script para execução no lado do cliente
                                            ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", script, true);

                                            txtColeta.Focus();
                                            return;

                                        }
                                        else
                                        {
                                            if (txtCNH.Text == "")
                                            {
                                                // string nomeUsuario = txtUsuCadastro.Text;
                                                string linha1 = "Olá, " + nomeUsuario + "!";
                                                string linha2 = "O motorista " + txtCodMotorista.Text.Trim() + " - " + txtNomMot.Text.Trim() + ", não tem validade de CNH, lançada no sistema.";
                                                string linha3 = "Verifique, por favor.";
                                                //string linha4 = "Unidade: " + unidade + ". Por favor, verifique.";
                                                // Concatenando as linhas com '\n' para criar a mensagem
                                                string mensagem = $"{linha1}\n{linha2}\n{linha3}";
                                                string mensagemCodificada = HttpUtility.JavaScriptStringEncode(mensagem);
                                                //// Gerando o script JavaScript para exibir o alerta
                                                string script = $"alert('{mensagemCodificada}');";
                                                //// Registrando o script para execução no lado do cliente
                                                ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", script, true);

                                                txtColeta.Focus();
                                                return;
                                            }
                                        }
                                    }

                                    // pesquisar validade da liberação GR
                                    if (txtLibGR.Text != "")
                                    {
                                        DateTime dataHoje = Convert.ToDateTime(DateTime.Now.Date);
                                        DateTime dataGR = Convert.ToDateTime(txtLibGR.Text).Date; ;

                                        TimeSpan diferencaGR = dataGR - dataHoje;
                                        // Agora você pode comparar a diferença
                                        if (diferencaGR.TotalDays < 30)
                                        {
                                            string diasGR = diferencaGR.TotalDays.ToString();
                                            txtLibGR.Text = txtLibGR.Text + " (" + diasGR + " dias)";
                                            txtLibGR.BackColor = System.Drawing.Color.Khaki;
                                            txtLibGR.ForeColor = System.Drawing.Color.OrangeRed;
                                            // string nomeUsuario = txtUsuCadastro.Text;
                                            //string linha1 = "Olá, " + nomeUsuario + "!";
                                            //string linha2 = "A liberãção de risco do motorista " + txtNomMot.Text.Trim() + ", vence em menos de 30 dias.";
                                            //string linha3 = "Data de vencimento: " + dataGR.ToString("dd/MM/yyyy") + ".";
                                            ////string linha4 = "Unidade: " + unidade + ". Por favor, verifique.";
                                            //// Concatenando as linhas com '\n' para criar a mensagem
                                            //string mensagem = $"{linha1}\n{linha2}\n{linha3}";
                                            //string mensagemCodificada = HttpUtility.JavaScriptStringEncode(mensagem);
                                            ////// Gerando o script JavaScript para exibir o alerta
                                            //string script = $"alert('{mensagemCodificada}');";
                                            ////// Registrando o script para execução no lado do cliente
                                            //ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", script, true);
                                        }
                                        else if (diferencaGR.TotalDays <= 0)
                                        {
                                            txtLibGR.BackColor = System.Drawing.Color.Red;
                                            txtLibGR.ForeColor = System.Drawing.Color.White;
                                            // string nomeUsuario = txtUsuCadastro.Text;
                                            string linha1 = "Olá, " + nomeUsuario + "!";
                                            string linha2 = "A liberação de risco do motorista " + txtCodMotorista.Text.Trim() + " - " + txtNomMot.Text.Trim() + ", está vencida.";
                                            string linha3 = "Data do vencimento: " + dataGR.ToString("dd/MM/yyyy") + ".";
                                            //string linha4 = "Unidade: " + unidade + ". Por favor, verifique.";
                                            // Concatenando as linhas com '\n' para criar a mensagem
                                            string mensagem = $"{linha1}\n{linha2}\n{linha3}";
                                            string mensagemCodificada = HttpUtility.JavaScriptStringEncode(mensagem);
                                            //// Gerando o script JavaScript para exibir o alerta
                                            string script = $"alert('{mensagemCodificada}');";
                                            //// Registrando o script para execução no lado do cliente
                                            ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", script, true);
                                            txtColeta.Focus();
                                            return;

                                        }
                                        else
                                        {
                                            if (txtLibGR.Text == "")
                                            {
                                                // string nomeUsuario = txtUsuCadastro.Text;
                                                string linha1 = "Olá, " + nomeUsuario + "!";
                                                string linha2 = "O motorista " + txtCodMotorista.Text.Trim() + " - " + txtNomMot.Text.Trim() + ", não tem liberação de risco cadastrada.";
                                                string linha3 = "Verifique, por favor.";
                                                //string linha4 = "Unidade: " + unidade + ". Por favor, verifique.";
                                                // Concatenando as linhas com '\n' para criar a mensagem
                                                string mensagem = $"{linha1}\n{linha2}\n{linha3}";
                                                string mensagemCodificada = HttpUtility.JavaScriptStringEncode(mensagem);
                                                //// Gerando o script JavaScript para exibir o alerta
                                                string script = $"alert('{mensagemCodificada}');";
                                                //// Registrando o script para execução no lado do cliente
                                                ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", script, true);

                                                txtColeta.Focus();
                                                return;

                                            }
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

                                fotoMotorista = "../../fotos/usuario.jpg";
                                txtCodMotorista.Text = "";
                                txtCodMotorista.Focus();

                            }
                            if (txtCodMotorista.Text.Trim() != "")
                            {
                                if (ConsultaMotorista.caminhofoto == null || ConsultaMotorista.caminhofoto == "")
                                {
                                    fotoMotorista = "../../fotos/usuario.jpg";

                                }
                                else
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

                        // dados do veículo
                        if (ConsultaCarga.frota == null || ConsultaCarga.frota.Trim() == "")
                        {
                            // Sem frota cadastrada
                            string nomeUsuario = txtUsuCadastro.Text;

                            string linha1 = "Olá, " + nomeUsuario + "!";
                            string linha2 = "CVA, não tem veiculo lançado. Verifique, por favor!";

                            // Concatenando as linhas com '\n' para criar a mensagem
                            string mensagem = $"{linha1}\n{linha2}";

                            string mensagemCodificada = HttpUtility.JavaScriptStringEncode(mensagem);
                            // Gerando o script JavaScript para exibir o alerta
                            string script = $"alert('{mensagemCodificada}');";

                            // Registrando o script para execução no lado do cliente
                            ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", script, true);
                            txtColeta.Focus();
                            return;
                        }
                        else
                        {
                            txtCodVeiculo.Text = ConsultaCarga.frota.Trim();
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
                                txtColeta.Focus();
                                return;

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
                                        //txtCodVeiculo.Text = "";
                                        txtPlaca.Text = "";
                                        txtReboque1.Text = "";
                                        txtReboque2.Text = "";
                                        txtFilialVeicCNT.Text = "";
                                        txtVeiculoTipo.Text = "";
                                        txtTipoVeiculo.Text = "";
                                        txtCarreta.Text = "";
                                        txtConjunto.Text = "";
                                        txtOpacidade.Text = "";
                                        txtCET.Text = "";
                                        txtCRLVVeiculo.Text = "";
                                        txtCRLVReb1.Text = "";
                                        txtCRLVReb2.Text = " ";
                                        txtCodProprietario.Text = "";
                                        txtProprietario.Text = "";
                                        txtTecnologia.Text = "";
                                        txtRastreamento.Text = "";
                                        txtColeta.Focus();
                                        return;
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
                                        // verifica se o motorista pertence a transportadora
                                        if (txtCodTransportadora.Text.Trim() != txtCodProprietario.Text.Trim())
                                        {

                                            string nomeUsuario = txtUsuCadastro.Text;
                                            string linha1 = "Olá, " + nomeUsuario + "!";
                                            string linha2 = "O motorista " + txtCodMotorista.Text.Trim() + " - " + txtNomMot.Text.Trim() + ", não pertence a transportadora do veículo " + txtProprietario.Text.Trim() + ".";
                                            string linha3 = "Verifique o código digitado: " + codigo + ". Ou altere o proprietário do veículo";
                                            //string linha4 = "Unidade: " + unidade + ". Por favor, verifique.";

                                            // Concatenando as linhas com '\n' para criar a mensagem
                                            string mensagem = $"{linha1}\n{linha2}\n{linha3}";

                                            string mensagemCodificada = HttpUtility.JavaScriptStringEncode(mensagem);
                                            //// Gerando o script JavaScript para exibir o alerta
                                            string script = $"alert('{mensagemCodificada}');";

                                            //// Registrando o script para execução no lado do cliente
                                            ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", script, true);

                                            //fotoMotorista = "../../fotos/usuario.jpg";
                                            // txtCodVeiculo.Text = "";
                                            txtPlaca.Text = "";
                                            txtReboque1.Text = "";
                                            txtReboque2.Text = "";
                                            txtFilialVeicCNT.Text = "";
                                            txtVeiculoTipo.Text = "";
                                            txtTipoVeiculo.Text = "";
                                            txtCarreta.Text = "";
                                            txtConjunto.Text = "";
                                            txtOpacidade.Text = "";
                                            txtCET.Text = "";
                                            txtCRLVVeiculo.Text = "";
                                            txtCRLVReb1.Text = "";
                                            txtCRLVReb2.Text = " ";
                                            txtCodProprietario.Text = "";
                                            txtProprietario.Text = "";
                                            txtTecnologia.Text = "";
                                            txtRastreamento.Text = "";
                                            txtColeta.Focus();
                                            return;


                                        }
                                        // verifica se a funcao do motorista permite dirigir o veiculo                                     
                                        if (txtFuncao.Text != "")
                                        {
                                            string primeiraLetraString = txtFuncao.Text.Trim().Substring(0, 1);
                                            if (primeiraLetraString == "M")
                                            {
                                                if (txtTipoVeiculo.Text == "BITRUCK" || txtTipoVeiculo.Text.Trim() == "UTILITÁRIO/FURGÃO" || txtTipoVeiculo.Text.Trim() == "LEVE" || txtTipoVeiculo.Text.Trim() == "FIORINO" || txtTipoVeiculo.Text.Trim() == "TOCO" || txtTipoVeiculo.Text.Trim() == "TRUCK" || txtTipoVeiculo.Text.Trim() == "VUC OU 3/4")
                                                {
                                                    // motorista apto a dirigir o veiculo
                                                }
                                                else
                                                {
                                                    string nomeUsuario = txtUsuCadastro.Text;
                                                    string linha1 = "Olá, " + nomeUsuario + "!";
                                                    string linha2 = "O motorista " + txtCodMotorista.Text.Trim() + " - " + txtNomMot.Text.Trim() + ", não é apto a dirigir este tipo de veículo " + txtTipoVeiculo.Text.Trim() + ".";
                                                    string linha3 = "Verifique sua função.";
                                                    //string linha4 = "Unidade: " + unidade + ". Por favor, verifique.";

                                                    // Concatenando as linhas com '\n' para criar a mensagem
                                                    string mensagem = $"{linha1}\n{linha2}\n{linha3}";

                                                    string mensagemCodificada = HttpUtility.JavaScriptStringEncode(mensagem);
                                                    //// Gerando o script JavaScript para exibir o alerta
                                                    string script = $"alert('{mensagemCodificada}');";

                                                    //// Registrando o script para execução no lado do cliente
                                                    ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", script, true);

                                                    //fotoMotorista = "../../fotos/usuario.jpg";
                                                    //txtCodVeiculo.Text = "";
                                                    txtPlaca.Text = "";
                                                    txtReboque1.Text = "";
                                                    txtReboque2.Text = "";
                                                    txtFilialVeicCNT.Text = "";
                                                    txtVeiculoTipo.Text = "";
                                                    txtTipoVeiculo.Text = "";
                                                    txtCarreta.Text = "";
                                                    txtConjunto.Text = "";
                                                    txtOpacidade.Text = "";
                                                    txtCET.Text = "";
                                                    txtCRLVVeiculo.Text = "";
                                                    txtCRLVReb1.Text = "";
                                                    txtCRLVReb2.Text = " ";
                                                    txtCodProprietario.Text = "";
                                                    txtProprietario.Text = "";
                                                    txtTecnologia.Text = "";
                                                    txtRastreamento.Text = "";
                                                    txtColeta.Focus();
                                                    return;
                                                }
                                            }
                                            else if (primeiraLetraString == "C")
                                            {
                                                if (txtTipoVeiculo.Text.Trim() == "CAVALO SIMPLES" || txtTipoVeiculo.Text.Trim() == "CAVALO TRUCADO" || txtTipoVeiculo.Text.Trim() == "CAVALO 4 EIXOS" || txtTipoVeiculo.Text.Trim() == "BITRUCK" || txtTipoVeiculo.Text.Trim() == "UTILITÁRIO/FURGÃO" || txtTipoVeiculo.Text.Trim() == "LEVE" || txtTipoVeiculo.Text.Trim() == "FIORINO" || txtTipoVeiculo.Text.Trim() == "TOCO" || txtTipoVeiculo.Text.Trim() == "TRUCK" || txtTipoVeiculo.Text.Trim() == "VUC OU 3/4")
                                                {
                                                    // motorista apto a dirigir qualquer veiculo, exceto bitrem
                                                }
                                                else
                                                {
                                                    string nomeUsuario = txtUsuCadastro.Text;
                                                    string linha1 = "Olá, " + nomeUsuario + "!";
                                                    string linha2 = "O motorista " + txtCodMotorista.Text.Trim() + " - " + txtNomMot.Text.Trim() + ", não é apto a dirigir este tipo de veículo " + txtTipoVeiculo.Text.Trim() + ".";
                                                    string linha3 = "Verifique sua função.";
                                                    //string linha4 = "Unidade: " + unidade + ". Por favor, verifique.";

                                                    // Concatenando as linhas com '\n' para criar a mensagem
                                                    string mensagem = $"{linha1}\n{linha2}\n{linha3}";

                                                    string mensagemCodificada = HttpUtility.JavaScriptStringEncode(mensagem);
                                                    //// Gerando o script JavaScript para exibir o alerta
                                                    string script = $"alert('{mensagemCodificada}');";

                                                    //// Registrando o script para execução no lado do cliente
                                                    ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", script, true);

                                                    //fotoMotorista = "../../fotos/usuario.jpg";
                                                    //txtCodVeiculo.Text = "";
                                                    txtPlaca.Text = "";
                                                    txtReboque1.Text = "";
                                                    txtReboque2.Text = "";
                                                    txtFilialVeicCNT.Text = "";
                                                    txtVeiculoTipo.Text = "";
                                                    txtTipoVeiculo.Text = "";
                                                    txtCarreta.Text = "";
                                                    txtConjunto.Text = "";
                                                    txtOpacidade.Text = "";
                                                    txtCET.Text = "";
                                                    txtCRLVVeiculo.Text = "";
                                                    txtCRLVReb1.Text = "";
                                                    txtCRLVReb2.Text = " ";
                                                    txtCodProprietario.Text = "";
                                                    txtProprietario.Text = "";
                                                    txtTecnologia.Text = "";
                                                    txtRastreamento.Text = "";
                                                    txtColeta.Focus();
                                                    return;
                                                }

                                            }
                                            else if (primeiraLetraString == "B")
                                            {
                                                if (txtTipoVeiculo.Text.Trim() == "CAVALO SIMPLES" || txtTipoVeiculo.Text.Trim() == "CAVALO TRUCADO" || txtTipoVeiculo.Text.Trim() == "CAVALO 4 EIXOS" || txtTipoVeiculo.Text.Trim() == "BITRUCK" || txtTipoVeiculo.Text.Trim() == "UTILITÁRIO/FURGÃO" || txtTipoVeiculo.Text.Trim() == "LEVE" || txtTipoVeiculo.Text.Trim() == "FIORINO" || txtTipoVeiculo.Text.Trim() == "TOCO" || txtTipoVeiculo.Text.Trim() == "TRUCK" || txtTipoVeiculo.Text.Trim() == "VUC OU 3/4" || txtTipoVeiculo.Text.Trim() == "BITREM")
                                                {
                                                    // motorista apto a dirigir qualquer veiculo, exceto bitrem
                                                }
                                                else
                                                {
                                                    string nomeUsuario = txtUsuCadastro.Text;
                                                    string linha1 = "Olá, " + nomeUsuario + "!";
                                                    string linha2 = "O motorista " + txtCodMotorista.Text.Trim() + " - " + txtNomMot.Text.Trim() + ", não é apto a dirigir este tipo de veículo " + txtTipoVeiculo.Text.Trim() + ".";
                                                    string linha3 = "Verifique sua função.";
                                                    //string linha4 = "Unidade: " + unidade + ". Por favor, verifique.";

                                                    // Concatenando as linhas com '\n' para criar a mensagem
                                                    string mensagem = $"{linha1}\n{linha2}\n{linha3}";

                                                    string mensagemCodificada = HttpUtility.JavaScriptStringEncode(mensagem);
                                                    //// Gerando o script JavaScript para exibir o alerta
                                                    string script = $"alert('{mensagemCodificada}');";

                                                    //// Registrando o script para execução no lado do cliente
                                                    ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", script, true);

                                                    //fotoMotorista = "../../fotos/usuario.jpg";
                                                    // txtCodVeiculo.Text = "";
                                                    txtPlaca.Text = "";
                                                    txtReboque1.Text = "";
                                                    txtReboque2.Text = "";
                                                    txtFilialVeicCNT.Text = "";
                                                    txtVeiculoTipo.Text = "";
                                                    txtTipoVeiculo.Text = "";
                                                    txtCarreta.Text = "";
                                                    txtConjunto.Text = "";
                                                    txtOpacidade.Text = "";
                                                    txtCET.Text = "";
                                                    txtCRLVVeiculo.Text = "";
                                                    txtCRLVReb1.Text = "";
                                                    txtCRLVReb2.Text = " ";
                                                    txtCodProprietario.Text = "";
                                                    txtProprietario.Text = "";
                                                    txtTecnologia.Text = "";
                                                    txtRastreamento.Text = "";
                                                    txtColeta.Focus();
                                                    return;
                                                }

                                            }
                                            else
                                            {
                                                string nomeUsuario = txtUsuCadastro.Text;
                                                string linha1 = "Olá, " + nomeUsuario + "!";
                                                string linha2 = "O motorista " + txtCodMotorista.Text.Trim() + " - " + txtNomMot.Text.Trim() + ", não é apto a dirigir nenhum tipo de veículo.";
                                                string linha3 = "Verifique seu cadastro.";
                                                //string linha4 = "Unidade: " + unidade + ". Por favor, verifique.";

                                                // Concatenando as linhas com '\n' para criar a mensagem
                                                string mensagem = $"{linha1}\n{linha2}\n{linha3}";

                                                string mensagemCodificada = HttpUtility.JavaScriptStringEncode(mensagem);
                                                //// Gerando o script JavaScript para exibir o alerta
                                                string script = $"alert('{mensagemCodificada}');";

                                                //// Registrando o script para execução no lado do cliente
                                                ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", script, true);

                                                //fotoMotorista = "../../fotos/usuario.jpg";
                                                //txtCodVeiculo.Text = "";
                                                txtPlaca.Text = "";
                                                txtReboque1.Text = "";
                                                txtReboque2.Text = "";
                                                txtFilialVeicCNT.Text = "";
                                                txtVeiculoTipo.Text = "";
                                                txtTipoVeiculo.Text = "";
                                                txtCarreta.Text = "";
                                                txtConjunto.Text = "";
                                                txtOpacidade.Text = "";
                                                txtCET.Text = "";
                                                txtCRLVVeiculo.Text = "";
                                                txtCRLVReb1.Text = "";
                                                txtCRLVReb2.Text = " ";
                                                txtCodProprietario.Text = "";
                                                txtProprietario.Text = "";
                                                txtTecnologia.Text = "";
                                                txtRastreamento.Text = "";
                                                txtColeta.Focus();
                                                return;
                                            }
                                        }

                                        // pesquisar primeiro reboque1
                                        if (txtTipoVeiculo.Text.Trim() == "CAVALO SIMPLES" || txtTipoVeiculo.Text.Trim() == "CAVALO TRUCADO" || txtTipoVeiculo.Text.Trim() == "CAVALO 4 EIXOS")
                                        {
                                            if (txtReboque1.Text == "")
                                            {
                                                string nomeUsuario = txtUsuCadastro.Text;
                                                string linha1 = "Olá, " + nomeUsuario + "!";
                                                string linha2 = "Veículo digitado " + txtCodVeiculo.Text.Trim() + " - " + txtPlaca.Text.Trim() + ", não tem reboque engatado.";
                                                string linha3 = "Verifique seu cadastro.";
                                                //string linha4 = "Unidade: " + unidade + ". Por favor, verifique.";

                                                // Concatenando as linhas com '\n' para criar a mensagem
                                                string mensagem = $"{linha1}\n{linha2}\n{linha3}";

                                                string mensagemCodificada = HttpUtility.JavaScriptStringEncode(mensagem);
                                                //// Gerando o script JavaScript para exibir o alerta
                                                string script = $"alert('{mensagemCodificada}');";

                                                //// Registrando o script para execução no lado do cliente
                                                ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", script, true);

                                                //fotoMotorista = "../../fotos/usuario.jpg";
                                                //txtCodVeiculo.Text = "";
                                                txtPlaca.Text = "";
                                                txtReboque1.Text = "";
                                                txtReboque2.Text = "";
                                                txtFilialVeicCNT.Text = "";
                                                txtVeiculoTipo.Text = "";
                                                txtTipoVeiculo.Text = "";
                                                txtCarreta.Text = "";
                                                txtConjunto.Text = "";
                                                txtOpacidade.Text = "";
                                                txtCET.Text = "";
                                                txtCRLVVeiculo.Text = "";
                                                txtCRLVReb1.Text = "";
                                                txtCRLVReb2.Text = " ";
                                                txtCodProprietario.Text = "";
                                                txtProprietario.Text = "";
                                                txtTecnologia.Text = "";
                                                txtRastreamento.Text = "";
                                                txtColeta.Focus();
                                                return;

                                            }

                                        }
                                        // Verifica reboque 2 - bitrem
                                        if (txtTipoVeiculo.Text.Trim() == "BITREM")
                                        {
                                            if (txtReboque2.Text == "")
                                            {
                                                string nomeUsuario = txtUsuCadastro.Text;
                                                string linha1 = "Olá, " + nomeUsuario + "!";
                                                string linha2 = "Veículo digitado " + txtCodVeiculo.Text.Trim() + " - " + txtPlaca.Text.Trim() + ", trata-se de um Bitrem, não tem o segundo reboque engatado.";
                                                string linha3 = "Verifique seu cadastro.";
                                                //string linha4 = "Unidade: " + unidade + ". Por favor, verifique.";

                                                // Concatenando as linhas com '\n' para criar a mensagem
                                                string mensagem = $"{linha1}\n{linha2}\n{linha3}";

                                                string mensagemCodificada = HttpUtility.JavaScriptStringEncode(mensagem);
                                                //// Gerando o script JavaScript para exibir o alerta
                                                string script = $"alert('{mensagemCodificada}');";

                                                //// Registrando o script para execução no lado do cliente
                                                ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", script, true);

                                                //fotoMotorista = "../../fotos/usuario.jpg";
                                                //txtCodVeiculo.Text = "";
                                                txtPlaca.Text = "";
                                                txtReboque1.Text = "";
                                                txtReboque2.Text = "";
                                                txtFilialVeicCNT.Text = "";
                                                txtVeiculoTipo.Text = "";
                                                txtTipoVeiculo.Text = "";
                                                txtCarreta.Text = "";
                                                txtConjunto.Text = "";
                                                txtOpacidade.Text = "";
                                                txtCET.Text = "";
                                                txtCRLVVeiculo.Text = "";
                                                txtCRLVReb1.Text = "";
                                                txtCRLVReb2.Text = " ";
                                                txtCodProprietario.Text = "";
                                                txtProprietario.Text = "";
                                                txtTecnologia.Text = "";
                                                txtRastreamento.Text = "";
                                                txtColeta.Focus();
                                                return;
                                            }
                                        }
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

                                    txtColeta.Focus();
                                    return;

                                }

                            }

                        }

                        //    if (dadosAtuais == null)
                        //    {
                        //        dadosAtuais = novosDados.Clone(); // estrutura idêntica
                        //    }

                        //    // Adiciona somente as coletas que ainda não estão em dadosAtuais
                        //    foreach (DataRow novaRow in novosDados.Rows)
                        //    {
                        //        string novaCarga = novaRow["carga"].ToString();

                        //        bool jaExiste = dadosAtuais.AsEnumerable()
                        //            .Any(r => r["carga"].ToString() == novaCarga);

                        //        if (!jaExiste)
                        //        {
                        //            dadosAtuais.ImportRow(novaRow);
                        //        }
                        //    }

                        //    ViewState["Coletas"] = dadosAtuais;

                        //    rptColetas.DataSource = dadosAtuais;
                        //    rptColetas.DataBind();
                        //    lblMensagem.Text = string.Empty;
                        //}
                        //else
                        //{
                        //    lblMensagem.Text = "Coleta em andamento ou já concluida. Verifique, por favor!";

                        //}


                        // 4. ADICIONA A NOVA COLETA AO REPEATER (se todas as validações passaram)
                        //var novosDados = DAL.ConCargas.FetchDataTableColetas2(searchTerm);
                        motoristaNovaColeta = ConsultaCarga.codmot?.Trim() ?? "";
                        motoristaNaTela = hdfCodMotorista.Value.Trim();





                        if (dadosAtuais.Columns.Count == 0)
                        {
                            dadosAtuais = novosDados.Clone(); // Clona a estrutura se o DataTable for novo.


                        }
                        else
                        {
                            if (motoristaNaTela != string.Empty)
                            {
                                if (motoristaNaTela != motoristaNovaColeta)
                                {
                                    //// Se o motorista for diferente, exibe um alerta e não adiciona a coleta.
                                    string nomeUsuario = txtUsuCadastro.Text;
                                    string mensagem = $"Olá, {nomeUsuario}!\nA coleta {searchTerm} pertence a um motorista diferente do que já está na viagem. Apenas coletas do mesmo motorista podem ser adicionadas.";
                                    string script = $"alert('{HttpUtility.JavaScriptStringEncode(mensagem)}');";
                                    ClientScript.RegisterStartupScript(this.GetType(), "MotoristaDiferenteAlerta", script, true);
                                    txtColeta.Focus();
                                    return; // Impede a adição da coleta.


                                }
                            }
                        }



                        if (string.IsNullOrEmpty(motoristaNovaColeta))
                        {
                            string msg = $"A coleta {searchTerm} não possui um motorista associado e não pode ser adicionada a uma viagem existente.";
                            string script = $"alert('{HttpUtility.JavaScriptStringEncode(msg)}');";
                            ClientScript.RegisterStartupScript(this.GetType(), "SemMotoristaAlerta", script, true);
                            txtColeta.Focus();
                            return;
                        }



                        // Adiciona a nova linha somente se ela ainda não existir no repeater.
                        foreach (DataRow novaRow in novosDados.Rows)
                        {
                            string novaCarga = novaRow["carga"].ToString();
                            bool jaExiste = dadosAtuais.AsEnumerable().Any(r => r["carga"].ToString() == novaCarga);

                            if (!jaExiste)
                            {
                                dadosAtuais.ImportRow(novaRow);
                            }
                        }
                        //string nomeUsuario = txtUsuCadastro.Text;
                        //string mensagem = $"Olá, {nomeUsuario}!\nA coleta {motoristaNaTela} motorista tela {motoristaNovaColeta} motorista nova coleta";
                        //string script = $"alert('{HttpUtility.JavaScriptStringEncode(mensagem)}');";
                        //ClientScript.RegisterStartupScript(this.GetType(), "MotoristaDiferenteAlerta", script, true);
                        // 5. ATUALIZA O VIEWSTATE E O REPEATER
                        ViewState["Coletas"] = dadosAtuais;
                        rptColetas.DataSource = dadosAtuais;
                        rptColetas.DataBind();
                        lblMensagem.Text = string.Empty; // Limpa mensagens de erro anteriores
                        txtColeta.Text = ""; // Limpa o campo de texto da coleta para a próxima inserção
                        txtColeta.Focus();
                        DataTable dtColetas = (DataTable)ViewState["Coletas"];

                        // 2. Verifique se o DataTable existe e se ele contém pelo menos uma linha.
                        //    Isso é crucial para evitar erros de "Object reference not set to an instance of an object".
                        if (dtColetas != null && dtColetas.Rows.Count > 0)
                        {
                            // 3. Acesse a primeira linha usando o índice 0.
                            DataRow primeiraLinha = dtColetas.Rows[0];

                            // 4. Pegue o valor da coluna "codmot" dessa linha e converta para string.
                            hdfCodMotorista.Value = primeiraLinha["codmot"].ToString();

                            // Pronto! A variável 'codigoMotorista' agora contém o valor que você precisa.
                            // Você pode usá-la como quiser. Por exemplo, para exibir em um alerta:
                            //string script = $"alert('O código do motorista da primeira coleta é: {codigoMotorista}');";
                            //ClientScript.RegisterStartupScript(this.GetType(), "MostrarMotorista", script, true);
                        }
                        else
                        {
                            // Opcional: Lógica para o caso de não haver coletas no ViewState.
                            // Por exemplo, exibir uma mensagem informando que não há dados.
                        }



                    }
                    else
                    {
                        lblMensagem.Text = "Coleta em andamento ou já concluida. Verifique, por favor!";

                    }
                }
                
            }
        }
        protected void btnSalvar1_Click(object sender, EventArgs e)
        {
            string query = @"INSERT INTO tbcarregamentos (
                        num_carregamento, codmotorista, nucleo, tipomot, valtoxicologico, venccnh, valgr, foto, nomemotorista, cpf,
                        cartaopedagio, valcartao, foneparticular, veiculo, veiculotipo, filialveiculo, valcet, valcrlvveiculo,
                        valcrlvreboque1, valcrlvreboque2, placa, tipoveiculo, reboque1, reboque2, carreta, tecnologia, rastreamento,
                        tipocarreta, codtra, transportadora, codcontato, fonecorporativo, empresa,dtcad,usucad,situacao, funcao, codtranspmotorista, nomtranspmotorista,venccronotacografo,valopacidade
                    ) VALUES (
                        @num_carregamento, @codmotorista, @nucleo, @tipomot, @valtoxicologico, @venccnh, @valgr, @foto,@nomemotorista, @cpf,
                        @cartaopedagio, @valcartao, @foneparticular, @veiculo, @veiculotipo, @filialveiculo, @valcet, @valcrlvveiculo,
                        @valcrlvreboque1, @valcrlvreboque2, @placa, @tipoveiculo, @reboque1, @reboque2, @carreta, @tecnologia, @rastreamento,@tipocarreta, @codtra, @transportadora, @codcontato, @fonecorporativo, @empresa,@dtcad,@usucad,@situacao,@funcao, @codtranspmotorista, @nomtranspmotorista, @venccronotacografo, @valopacidade
                    )";

            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                string nomeUsuario = Session["UsuarioLogado"].ToString();
                // Adiciona parâmetros com tratamento de vazio/nulo
                cmd.Parameters.AddWithValue("@num_carregamento", SafeValue(novaColeta.Text));
                cmd.Parameters.AddWithValue("@codmotorista", SafeValue(txtCodMotorista.Text));
                cmd.Parameters.AddWithValue("@nucleo", SafeValue(txtFilialMot.Text));
                cmd.Parameters.AddWithValue("@tipomot", SafeValue(txtTipoMot.Text));
                cmd.Parameters.AddWithValue("@valtoxicologico", SafeDateValue(txtExameToxic.Text));
                cmd.Parameters.AddWithValue("@venccnh", SafeDateValue(txtCNH.Text));
                cmd.Parameters.AddWithValue("@valgr", SafeDateValue(txtLibGR.Text));
                cmd.Parameters.AddWithValue("@foto", SafeValue(fotoMotorista)); // Se for byte[], troque tipo do parâmetro!
                cmd.Parameters.AddWithValue("@nomemotorista", SafeValue(txtNomMot.Text));
                cmd.Parameters.AddWithValue("@cpf", SafeValue(txtCPF.Text));
                cmd.Parameters.AddWithValue("@cartaopedagio", SafeValue(txtCartao.Text));
                cmd.Parameters.AddWithValue("@valcartao", SafeDateValue(txtValCartao.Text));
                cmd.Parameters.AddWithValue("@foneparticular", SafeValue(txtCelular.Text));
                cmd.Parameters.AddWithValue("@veiculo", SafeValue(txtCodVeiculo.Text));
                cmd.Parameters.AddWithValue("@veiculotipo", SafeValue(txtVeiculoTipo.Text));
                cmd.Parameters.AddWithValue("@filialveiculo", SafeValue(txtFilialVeicCNT.Text));
                cmd.Parameters.AddWithValue("@tipoveiculo", SafeValue(txtTipoVeiculo.Text));
                cmd.Parameters.AddWithValue("@valcet", SafeDateValue(txtCET.Text));
                cmd.Parameters.AddWithValue("@valcrlvveiculo", SafeDateValue(txtCRLVVeiculo.Text));
                cmd.Parameters.AddWithValue("@valcrlvreboque1", SafeDateValue(txtCRLVReb1.Text));
                cmd.Parameters.AddWithValue("@valcrlvreboque2", SafeDateValue(txtCRLVReb2.Text));
                cmd.Parameters.AddWithValue("@placa", SafeValue(txtPlaca.Text));
                cmd.Parameters.AddWithValue("@venccronotacografo", SafeValue(txtCrono.Text));
                cmd.Parameters.AddWithValue("@valopacidade", SafeValue(txtOpacidade.Text));
                cmd.Parameters.AddWithValue("@reboque1", SafeValue(txtReboque1.Text));
                cmd.Parameters.AddWithValue("@reboque2", SafeValue(txtReboque2.Text));
                cmd.Parameters.AddWithValue("@carreta", SafeValue(txtCarreta.Text));
                cmd.Parameters.AddWithValue("@tecnologia", SafeValue(txtTecnologia.Text));
                cmd.Parameters.AddWithValue("@rastreamento", SafeValue(txtRastreamento.Text));
                cmd.Parameters.AddWithValue("@tipocarreta", SafeValue(txtConjunto.Text));
                cmd.Parameters.AddWithValue("@codtra", SafeValue(txtCodProprietario.Text));
                cmd.Parameters.AddWithValue("@transportadora", SafeValue(txtProprietario.Text));
                cmd.Parameters.AddWithValue("@codcontato", SafeValue(txtCodFrota.Text));
                cmd.Parameters.AddWithValue("@fonecorporativo", SafeValue(txtFoneCorp.Text));
                cmd.Parameters.AddWithValue("@empresa", SafeValue("CNT (CC)"));
                cmd.Parameters.AddWithValue("@dtcad", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                cmd.Parameters.AddWithValue("@usucad", nomeUsuario);
                cmd.Parameters.AddWithValue("@situacao", "EM ANDAMENTO");
                cmd.Parameters.AddWithValue("@funcao", SafeValue(txtFuncao.Text));
                cmd.Parameters.AddWithValue("@codtranspmotorista", SafeValue(txtCodTransportadora.Text));
                cmd.Parameters.AddWithValue("@nomtranspmotorista", SafeValue(txtTransportadora.Text));
               

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();



                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "Mensagem", "alert('Coletas salvas com sucesso!');", true);
                    //AtualizarColetasVisiveis();


                }
                catch (Exception ex)
                {
                    string mensagemErro = $"Erro ao salvar: {ex.Message}";
                    string mensagemCodificada1 = HttpUtility.JavaScriptStringEncode(mensagemErro);
                    string script1 = $"alert('{mensagemCodificada1}');";
                    ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeErro", script1, true);

                    // Caso queira exibir também em um Label
                    // lblMensagem.Text = mensagemErro;
                }
                foreach (RepeaterItem item in rptColetas.Items)
                {
                    if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                    {
                        // Recuperar os valores dos controles
                        string carga = ((Label)item.FindControl("lblCarga"))?.Text; // ou Eval direto se não tiver Label
                        string atendimento = ((Label)item.FindControl("lblAtendimento"))?.Text;



                        if (!string.IsNullOrEmpty(carga))
                        {
                            using (SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
                            {
                                string queryc = @"UPDATE tbcargas SET 
                                                    emissao=@emissao,
                                                    idviagem=@idviagem,
                                                    codmot=@codmot,
                                                    frota=@frota,
                                                    status=@status,
                                                    andamento=@andamento,
                                                    atendimento=@atendimento,
                                                    funcaomot=@funcaomot
                                                    WHERE carga = @carga";

                                SqlCommand cmdc = new SqlCommand(queryc, con);
                                cmdc.Parameters.AddWithValue("@carga", carga);
                                cmdc.Parameters.AddWithValue("@idviagem", novaColeta?.Text ?? "");
                                cmdc.Parameters.AddWithValue("@codmot", txtCodMotorista?.Text ?? "");
                                cmdc.Parameters.AddWithValue("@frota", txtCodFrota?.Text ?? "");
                                cmdc.Parameters.AddWithValue("@status", "PENDENTE");
                                cmdc.Parameters.AddWithValue("@andamento", "EM ANDAMENTO");
                                cmdc.Parameters.AddWithValue("@atendimento", atendimento);
                                cmdc.Parameters.AddWithValue("@funcaomot", txtFuncao.Text.Trim());
                                cmdc.Parameters.AddWithValue("@emissao", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));

                                con.Open();
                                cmdc.ExecuteNonQuery();

                                //nomeUsuario = txtUsuCadastro.Text;
                                //string mensagem = $"Olá, {nomeUsuario}!\nOrdem de Coleta, cadastrada com sucesso!";
                                //string mensagemCodificada = HttpUtility.JavaScriptStringEncode(mensagem);
                                //string script = $"alert('{mensagemCodificada}');";
                                //ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", script, true);

                            }
                        }
                    }
                }

            }
        }
        private object SafeValue(string input)
        {
            return string.IsNullOrWhiteSpace(input) ? (object)DBNull.Value : input;


        }
        private object SafeDateValue(string input)
        {
            DateTime dt;
            if (DateTime.TryParse(input, out dt))
                return dt.ToString("yyyy-MM-dd");
            else
                return DBNull.Value;
        }

        protected void ddlCliInicial_TextChanged(object sender, EventArgs e)
        {
            codCliInicial.Text = ddlCliInicial.SelectedValue;
        }

        protected void ddlCliFinal_TextChanged(object sender, EventArgs e)
        {
            codCliFinal.Text = ddlCliFinal.SelectedValue;
            string sql = "select Distancia, UF_Origem, Origem, UF_Destino, Destino from tbdistanciapremio where UF_Origem=(SELECT estcli FROM tbclientes where codcli='" + ddlCliInicial.SelectedValue + "') and Origem=(SELECT cidcli FROM tbclientes where codcli='" + ddlCliInicial.SelectedValue + "') and UF_Destino=(SELECT estcli FROM tbclientes where codcli='" + ddlCliFinal.SelectedValue + "') and Destino=(SELECT cidcli FROM tbclientes where codcli='" + ddlCliFinal.SelectedValue + "')";
            SqlDataAdapter adp = new SqlDataAdapter(sql, con);
            DataTable dt = new DataTable();
            con.Open();
            adp.Fill(dt);
            con.Close();
            if (dt.Rows.Count > 0)
            {
                txtDistancia.Text = dt.Rows[0][0].ToString();
                txtUfOrigem.Text = dt.Rows[0][1].ToString();
                txtMunicipioOrigem.Text = dt.Rows[0][2].ToString();
                txtUfDestino.Text = dt.Rows[0][3].ToString();
                txtMunicipioDestino.Text = dt.Rows[0][4].ToString();
                lblDistancia.Text = string.Empty;
            }
            else
            {
                lblDistancia.Text = "Não há distância cadastrada para essa origem e destino";
            }


        }

        protected void codCliInicial_TextChanged(object sender, EventArgs e)
        {
            if (codCliInicial.Text != "")
            {

                string codigoRemetente = codCliInicial.Text.Trim();
                string strConn = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    string query = "SELECT codcli, nomcli, cidcli, estcli FROM tbclientes WHERE codcli = @Codigo OR codvw=@Codigo";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Codigo", codigoRemetente);
                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                codCliInicial.Text = reader["codcli"].ToString();
                                ddlCliInicial.SelectedItem.Text = reader["nomcli"].ToString();
                                txtMunicipioOrigem.Text = reader["cidcli"].ToString();
                                txtUfOrigem.Text = reader["estcli"].ToString();
                                codCliFinal.Focus();
                            }
                            else
                            {
                                ddlCliInicial.SelectedItem.Text = "Selecione...";
                                codCliInicial.Text = "";
                                // Aciona o Toast via JavaScript
                                ScriptManager.RegisterStartupScript(this, GetType(), "toastNaoEncontrado", "mostrarToastNaoEncontrado();", true);
                                codCliInicial.Focus();
                                // Opcional: exibir mensagem ao usuário
                            }
                        }
                    }

                }

            }
        }

        protected void codCliFinal_TextChanged(object sender, EventArgs e)
        {
            if (codCliFinal.Text != "")
            {


                string codigoRemetente = codCliFinal.Text.Trim();
                string strConn = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    string query = "SELECT codcli, nomcli, cidcli, estcli FROM tbclientes WHERE codcli = @Codigo OR codvw=@Codigo";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Codigo", codigoRemetente);
                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                codCliFinal.Text = reader["codcli"].ToString();
                                ddlCliFinal.SelectedItem.Text = reader["nomcli"].ToString();
                                txtMunicipioDestino.Text = reader["cidcli"].ToString();
                                txtUfDestino.Text = reader["estcli"].ToString();
                            }
                            else
                            {
                                ddlCliFinal.SelectedItem.Text = "Selecione...";
                                codCliFinal.Text = "";
                                // Aciona o Toast via JavaScript
                                ScriptManager.RegisterStartupScript(this, GetType(), "toastNaoEncontrado", "mostrarToastNaoEncontrado();", true);
                                codCliInicial.Focus();
                                // Opcional: exibir mensagem ao usuário
                            }
                        }
                    }

                }

            }
        }

        private void PreencherNumCargaVazia()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT (carga + incremento) as ProximaCarga FROM tbcontadores";

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
                                novaCarga.Text = reader["ProximaCarga"].ToString();
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
                    string sql = @"UPDATE tbcontadores SET carga = @carga WHERE id = @id";
                    try
                    {
                        using (SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
                        using (SqlCommand cmd = new SqlCommand(sql, con))
                        {
                            cmd.Parameters.AddWithValue("@carga", novaCarga.Text);
                            cmd.Parameters.AddWithValue("@id", idConvertido);

                            con.Open();
                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                // atualiza  
                            }
                            else
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", "alert('Erro ao atualizar o número da viagem.');", true);
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
        protected void btnSalvarColeta_Click(object sender, EventArgs e)
        {
            txtColeta.Text = novaCarga.Text.Trim();

            int numCarga = int.Parse(novaCarga.Text.Trim());
            string codigoOrigem = codCliInicial.Text.Trim();
            string nomeOrigem = ddlCliInicial.SelectedItem.Text.Trim().ToUpper();
            string codigoDestino = codCliFinal.Text.Trim();
            string nomeDestino = ddlCliFinal.SelectedItem.Text.Trim().ToUpper();
            string municipioOrigem = txtMunicipioOrigem.Text.Trim().ToUpper();
            string municipioDestino = txtMunicipioDestino.Text.Trim().ToUpper();
            string ufOrigem = txtUfOrigem.Text.Trim().ToUpper();
            string ufDestino = txtUfDestino.Text.Trim().ToUpper();
            int distancia = int.Parse(txtDistancia.Text.Trim());

            string connectionString = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO tbcargas (carga, emissao, status, entrega, peso, material, portao, situacao, previsao, codorigem, cliorigem, coddestino, clidestino, ufcliorigem, ufclidestino, cidorigem, ciddestino, empresa, cadastro, distancia)" +
                  "VALUES (@Carga, GETDATE(), @status, @entrega, @peso, @material, @portao, @situacao, @previsao, @codorigem, @cliorigem, @coddestino, @clidestino, @ufcliorigem, @ufclidestino, @cidorigem, @ciddestino, @empresa, @cadastro, @distancia)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@carga", numCarga);
                cmd.Parameters.AddWithValue("@status", "Pendente");
                cmd.Parameters.AddWithValue("@entrega", "NORMAL");
                cmd.Parameters.AddWithValue("@peso", "0"); // ou valor padrão
                cmd.Parameters.AddWithValue("@material", "Vazio"); // ou valor padrão
                cmd.Parameters.AddWithValue("@portao", "vz"); // ou valor padrão
                cmd.Parameters.AddWithValue("@situacao", "Pendente");
                cmd.Parameters.AddWithValue("@previsao", DateTime.Now.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@codorigem", codigoOrigem);
                cmd.Parameters.AddWithValue("@cliorigem", nomeOrigem);
                cmd.Parameters.AddWithValue("@coddestino", codigoDestino);
                cmd.Parameters.AddWithValue("@clidestino", nomeDestino);
                cmd.Parameters.AddWithValue("@ufcliorigem", ufOrigem);
                cmd.Parameters.AddWithValue("@ufclidestino", ufDestino);
                cmd.Parameters.AddWithValue("@cidorigem", municipioOrigem);
                cmd.Parameters.AddWithValue("@ciddestino", municipioDestino);
                cmd.Parameters.AddWithValue("@empresa", "CNT (CC)"); // ou valor padrão
                cmd.Parameters.AddWithValue("@cadastro", DateTime.Now.ToString("dd/MM/yyyy HH:mm") + " - " + Session["UsuarioLogado"].ToString());
                cmd.Parameters.AddWithValue("@distancia", distancia);

                // Abrindo a conexão e executando a query
                conn.Open();
                int rowsInserted = cmd.ExecuteNonQuery();

                if (rowsInserted > 0)
                {
                    txtColeta.Text = novaCarga.Text.Trim();
                    PreencherNumCargaVazia(); // Atualiza o número da coleta para o próximo valor
                }
                else
                {
                    string mensagem = "Falha ao cadastrar a viagem. Tente novamente.";
                    string script = $"alert('{HttpUtility.JavaScriptStringEncode(mensagem)}');";
                    ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeErro", script, true);
                }
            }

        }

        protected void btnCadContato_Click(object sender, EventArgs e)
        {


            string codigoTelefone = txtCodContato.Text.Trim();
            string numeroTelefone = txtCadCelular.Text;

            string connectionString = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO tbfoneveiculos (veiculo, numero)" +
                  "VALUES (@veiculo,  @numero)";
                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@veiculo", codigoTelefone);
                cmd.Parameters.AddWithValue("@numero", numeroTelefone);

                // Abrindo a conexão e executando a query
                conn.Open();
                int rowsInserted = cmd.ExecuteNonQuery();

                if (rowsInserted > 0)
                {
                    txtCodFrota.Text = txtCodContato.Text.Trim();
                    txtFoneCorp.Text = txtCadCelular.Text;
                }
                else
                {
                    string mensagem = "Falha ao cadastrar telefone do motorista.";
                    string script = $"alert('{HttpUtility.JavaScriptStringEncode(mensagem)}');";
                    ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeErro", script, true);
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




    }



}