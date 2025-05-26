using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Configuration;
using System.Web.UI.HtmlControls;
using System.Collections;
using Org.BouncyCastle.Asn1.Cmp;
using NPOI.SS.Formula.Functions;
using Domain;

namespace NewCapit.dist.pages
{
    public partial class Frm_AtualizaOrdemColeta : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conexao"].ToString());
        public string fotoMotorista;
        string codmot, caminhofoto;
        string num_coleta;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UsuarioLogado"] != null)
                {
                    string nomeUsuario = Session["UsuarioLogado"].ToString();
                    var lblUsuario = nomeUsuario;
                    txtAtualizadoPor.Text = nomeUsuario;
                }
                else
                {
                    var lblUsuario = "<Usuário>";
                    txtAtualizadoPor.Text = lblUsuario;
                }
                DateTime dataHoraAtual = DateTime.Now;
                lblAtualizadoEm.Text = dataHoraAtual.ToString("dd/MM/yyyy HH:mm");
                CarregaDados();
                CarregaNumColeta();
                PreencherComboResponsavel();
                PreencherComboipoOcorrencia();

                //PreencherComboStatus();
                //PreencherNumColeta();
                //fotoMotorista = "../../fotos/usuario.jpg";

            }
            CarregaFoto();
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
                    //ddlCliInicial.DataSource = reader;
                    //ddlCliInicial.DataTextField = "nomcli";  // Campo que será mostrado no ComboBox
                    //ddlCliInicial.DataValueField = "id";  // Campo que será o valor de cada item                    
                    //ddlCliInicial.DataBind();  // Realiza o binding dos dados                   
                    //ddlCliInicial.Items.Insert(0, new ListItem("Selecione...", "0"));
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
                    //ddlCliFinal.DataSource = reader;
                    //ddlCliFinal.DataTextField = "nomcli";  // Campo que será mostrado no ComboBox
                    //ddlCliFinal.DataValueField = "id";  // Campo que será o valor de cada item                    
                    //ddlCliFinal.DataBind();  // Realiza o binding dos dados                   
                    //ddlCliFinal.Items.Insert(0, new ListItem("Selecione...", "0"));
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
        protected void ddlCliInicial_TextChanged(object sender, EventArgs e)
        {
            //codCliInicial.Text = ddlCliInicial.SelectedValue;
        }

        protected void ddlCliFinal_TextChanged(object sender, EventArgs e)
        {
            //codCliFinal.Text = ddlCliFinal.SelectedValue;

           
        }
       
        public void CarregaNumColeta()
        {
            if (HttpContext.Current.Request.QueryString["carregamento"].ToString() != "")
            {
                num_coleta = HttpContext.Current.Request.QueryString["carregamento"].ToString();
            }
            novaColeta.Text = num_coleta;
        }
        public void CarregaDados()
        {
            if (HttpContext.Current.Request.QueryString["carregamento"].ToString() != "")
            {
                num_coleta = HttpContext.Current.Request.QueryString["carregamento"].ToString();
            }
            string sql = "select codmotorista,veiculo,dtcad,usucad,codcliorigem, nomcliorigem, codclidestino, nomclidestino, distancia,tipoveiculo from tbcarregamentos where num_carregamento='" + num_coleta+"'";
            SqlDataAdapter adtp = new SqlDataAdapter(sql, con);
            DataTable dt = new DataTable();
            con.Open();
            adtp.Fill(dt);
            con.Close();
            //Carrega Motorista
            var codigo = dt.Rows[0][0].ToString();
            txtCodMotorista.Text = codigo;
            txtUsuCadastro.Text = dt.Rows[0][3].ToString();
            lblDtCadastro.Text = dt.Rows[0][2].ToString();
            //codCliInicial.Text = dt.Rows[0][4].ToString();
            //ddlCliInicial.Items.Insert(0, new ListItem(dt.Rows[0][5].ToString(),""));
            //codCliFinal.Text = dt.Rows[0][6].ToString();
            //ddlCliFinal.Items.Insert(0, new ListItem(dt.Rows[0][7].ToString(),""));
            //txtDistancia.Text = dt.Rows[0][8].ToString();
            //ddlVeiculosCNT.Items.Insert(0, new ListItem(dt.Rows[0][9].ToString(), ""));
            var obje = new Domain.ConsultaMotorista
            {
                codmot = codigo
            };
          
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



                        if (!File.Exists(fotoMotorista))
                        {
                            fotoMotorista = ConsultaMotorista.caminhofoto.Trim().ToString();
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

                        var codigoAgregado = dt.Rows[0][1].ToString();

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

                    string idviagem;
                    idviagem = num_coleta;
                    CarregarColetas(idviagem);
                    
                }
                
            }
        }

        protected void rptColetas_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            var ddlStatus = (DropDownList)e.Item.FindControl("ddlStatus");
            if (ddlStatus == null) return;

            // 1) carrega os status da tabela
            const string sql = "SELECT cod_status, ds_status FROM tb_status";
            using (var conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            using (var cmd = new SqlCommand(sql, conn))
            {
                try
                {
                    conn.Open();
                    using (var rdr = cmd.ExecuteReader())
                    {
                        ddlStatus.DataSource = rdr;
                        ddlStatus.DataTextField = "ds_status";
                        ddlStatus.DataValueField = "cod_status";
                        ddlStatus.DataBind();
                    }
                    var drv = (HiddenField)e.Item.FindControl("hdfStatus"); ;
                    string statusDaColeta = drv.Value;  // o nome da coluna do seu DataTable
                    // opcional: insere item em branco no topo
                    ddlStatus.Items.Insert(0, new ListItem(statusDaColeta, "0"));
                }
                catch (Exception ex)
                {
                    // trate o erro como preferir
                    Response.Write("Erro ao carregar status: " + ex.Message);
                    return;
                }
            }

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
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
                        tdAtendimento.BgColor = "Red";
                        tdAtendimento.Attributes["style"] = "color: white; font-weight: bold;";
                    }
                    else if (dataHoraComparacao.Date == agora.Date && dataHoraComparacao.TimeOfDay <= agora.TimeOfDay
                             && (status == "Concluído" || status == "PENDENTE"))
                    {
                        lblAtendimento.Text = "No Prazo";
                        tdAtendimento.BgColor = "Green";
                        tdAtendimento.Attributes["style"] = "color: white; font-weight: bold;";
                    }
                    else if (dataHoraComparacao > agora && status == "Concluído")
                    {
                        lblAtendimento.Text = "Antecipado";
                        tdAtendimento.BgColor = "Orange";
                        tdAtendimento.Attributes["style"] = "color: white; font-weight: bold;";
                    }
                    else
                    {
                        lblAtendimento.Text = status; 
                        
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
                                codmot=@codmot, 
                                tempoesperagate=@tempoesperagate,
                                frota=@frota
                                WHERE carga = @carga";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@carga", carga);
                    cmd.Parameters.AddWithValue("@cva", txtCVA.Text.Trim());
                    cmd.Parameters.AddWithValue("@gate", SafeDateValue(txtGate.Text.Trim()));
                    cmd.Parameters.AddWithValue("@status", ddlStatus.SelectedItem.Text ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@chegadaorigem", SafeDateValue(txtChegadaOrigem.Text.Trim()));
                    cmd.Parameters.AddWithValue("@saidaorigem", SafeDateValue(txtSaidaOrigem.Text.Trim()));
                    cmd.Parameters.AddWithValue("@tempoagcarreg", SafeValue(txtAgCarreg.Text.Trim()));
                    cmd.Parameters.AddWithValue("@chegadadestino", SafeDateValue(txtChegadaDestino.Text.Trim()));
                    cmd.Parameters.AddWithValue("@entradaplanta", SafeDateValue(txtEntrada.Text.Trim()));
                    cmd.Parameters.AddWithValue("@saidaplanta", SafeDateValue(txtSaidaPlanta.Text.Trim()));
                    cmd.Parameters.AddWithValue("@tempodentroplanta", SafeValue(txtDentroPlanta.Text.Trim()));
                    cmd.Parameters.AddWithValue("@tempoesperagate", SafeValue(txtEsperaGate.Text.Trim()));
                    cmd.Parameters.AddWithValue("@codmot", txtCodMotorista.Text.Trim() ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@frota", txtCodFrota.Text.Trim() ?? (object)DBNull.Value);
                    // continue os parâmetros conforme seu banco

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                // Após atualizar, recarregar os dados no Repeater
                AtualizarColetasVisiveis();
            }
            if (e.CommandName == "Ocorrencias")
            {
                int id = Convert.ToInt32(e.CommandArgument);

                string connStr = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    SqlCommand cmd1 = new SqlCommand("SELECT carga, andamento FROM tbcargas WHERE carga = @Id", conn);
                    cmd1.Parameters.AddWithValue("@Id", id);
                    SqlDataReader reader1 = cmd1.ExecuteReader();
                    if (reader1.Read())
                    {

                        lblColeta.BackColor = System.Drawing.Color.LightGreen;
                        lblColeta.Text = reader1["carga"].ToString();

                        if (reader1["andamento"].ToString() == "CONCLUIDO")
                        {
                            lblStatus.BackColor = System.Drawing.Color.LightGreen;
                            lblStatus.Text = reader1["andamento"].ToString();
                        }
                        else if (reader1["andamento"].ToString() == "PENDENTE")
                        {
                            lblStatus.BackColor = System.Drawing.Color.Yellow;
                            lblStatus.Text = reader1["andamento"].ToString();
                        }
                        else if (reader1["andamento"].ToString() == "ANDAMENTO")
                        {
                            lblStatus.BackColor = System.Drawing.Color.LightCoral;
                            lblStatus.Text = reader1["andamento"].ToString();
                        }
                        else
                        {
                            lblStatus.BackColor = System.Drawing.Color.Black;
                            lblStatus.Text = reader1["andamento"].ToString();
                        }

                        using (SqlConnection con = new SqlConnection(connStr))
                        {
                            string query = "SELECT id, responsavel, motivo, observacao, data_inclusao, usuario_inclusao FROM tbocorrencias WHERE carga = @numeroCarga";

                            SqlCommand cmd = new SqlCommand(query, con);
                            cmd.Parameters.AddWithValue("@numeroCarga", id);

                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            DataTable dt = new DataTable();
                            da.Fill(dt);

                            GridViewCarga.DataSource = dt;
                            GridViewCarga.DataBind();
                        }


                        // Exibe o modal com JavaScript
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "abrirModal", "$('#exampleModalCenter').modal('show');", true);

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

                    //txtColeta.Text = "";
                    //txtColeta.Focus();

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

        private void CarregarColetas(string idviagem = "")
        {
            // Obtem os dados atuais (novos dados)
            var novosDados = DAL.ConCargas.FetchDataTableColetas3(idviagem);

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
            string query = @"UPDATE tbcarregamentos SET
                            codmotorista = @codmotorista,
                            nucleo = @nucleo,
                            tipomot = @tipomot,
                            valtoxicologico = @valtoxicologico,
                            venccnh = @venccnh,
                            valgr = @valgr,
                            foto = @foto,
                            nomemotorista = @nomemotorista,
                            cpf = @cpf,
                            cartaopedagio = @cartaopedagio,
                            valcartao = @valcartao,
                            foneparticular = @foneparticular,
                            veiculo = @veiculo,
                            veiculotipo = @veiculotipo,
                            filialveiculo = @filialveiculo,
                            valcet = @valcet,
                            valcrlvveiculo = @valcrlvveiculo,
                            valcrlvreboque1 = @valcrlvreboque1,
                            valcrlvreboque2 = @valcrlvreboque2,
                            placa = @placa,
                            tipoveiculo = @tipoveiculo,
                            reboque1 = @reboque1,
                            reboque2 = @reboque2,
                            carreta = @carreta,
                            tecnologia = @tecnologia,
                            rastreamento = @rastreamento,
                            tipocarreta = @tipocarreta,
                            codtra = @codtra,
                            transportadora = @transportadora,
                            codcontato = @codcontato,
                            fonecorporativo = @fonecorporativo,
                            empresa = @empresa,
                            dtalt = @dtalt,
                            usualt = @usualt
                           
                        WHERE num_carregamento = @num_carregamento";
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
                cmd.Parameters.AddWithValue("@valcet", SafeDateValue(txtCET.Text));
                cmd.Parameters.AddWithValue("@valcrlvveiculo", SafeDateValue(txtCRLVVeiculo.Text));
                cmd.Parameters.AddWithValue("@valcrlvreboque1", SafeDateValue(txtCRLVReb1.Text));
                cmd.Parameters.AddWithValue("@valcrlvreboque2", SafeDateValue(txtCRLVReb2.Text));
                cmd.Parameters.AddWithValue("@placa", SafeValue(txtPlaca.Text));
                cmd.Parameters.AddWithValue("@tipoveiculo", SafeValue(txtTipoVeiculo.Text));
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
                cmd.Parameters.AddWithValue("@empresa", SafeValue("CNT"));
                cmd.Parameters.AddWithValue("@dtalt", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                cmd.Parameters.AddWithValue("@usualt", nomeUsuario);
                //cmd.Parameters.AddWithValue("@situacao", "PENDENTE");

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    nomeUsuario = txtUsuCadastro.Text;
                    string mensagem = $"Olá, {nomeUsuario}!\nCarregamento atualizado no sistema com sucesso.";
                    string mensagemCodificada = HttpUtility.JavaScriptStringEncode(mensagem);
                    string script = $"alert('{mensagemCodificada}');";
                    ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", script, true);


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
                return dt.ToString("yyyy-MM-dd HH:mm");
            else
                return DBNull.Value;
        }

        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            if (HttpContext.Current.Request.QueryString["carregamento"].ToString() != "")
            {
                num_coleta = HttpContext.Current.Request.QueryString["carregamento"].ToString();
            }
            string idCarga = num_coleta; // esse valor viria da lógica do seu código

            string url = $"OrdemColetaImpressao.aspx?id={idCarga}";
            string script = $"window.open('{url}', '_blank', 'toolbar=no,location=no,status=no,menubar=no,scrollbars=yes,resizable=yes,width=794,height=1123');";
            ClientScript.RegisterStartupScript(this.GetType(), "abrirJanela", script, true);
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

        private void PreencherComboResponsavel()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT id, descricao FROM tbresponsavelocorrencia ORDER BY descricao ASC";

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
                    cboResponsavel.DataSource = reader;
                    cboResponsavel.DataTextField = "descricao";  // Campo que será mostrado no ComboBox
                    cboResponsavel.DataValueField = "id";  // Campo que será o valor de cada item                    
                    cboResponsavel.DataBind();  // Realiza o binding dos dados                   
                    cboResponsavel.Items.Insert(0, new ListItem("Selecione...", "0"));
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

        private void PreencherComboipoOcorrencia()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT id, descricao FROM tbtipodeocorrencias ORDER BY descricao ASC";

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
                    cboMotivo.DataSource = reader;
                    cboMotivo.DataTextField = "descricao";  // Campo que será mostrado no ComboBox
                    cboMotivo.DataValueField = "id";  // Campo que será o valor de cada item                    
                    cboMotivo.DataBind();  // Realiza o binding dos dados                   
                    cboMotivo.Items.Insert(0, new ListItem("Selecione...", "0"));
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

        protected void btnSalvarOcorrencia_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
            int numColeta = int.Parse(lblColeta.Text.Trim());
            string responsavel = cboResponsavel.SelectedItem.ToString().Trim().ToUpper();
            string motivo = cboMotivo.SelectedItem.ToString().Trim().ToUpper();
            string ocorrencia = txtObservacao.Text.Trim();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO tbocorrencias (carga, responsavel, motivo, observacao, data, usuario_inclusao, data_inclusao) VALUES (@Carga, @Responsavel, @Motivo, @Observacao, GETDATE(), @Usuario_Inclusao, GETDATE())";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Carga", numColeta);
                cmd.Parameters.AddWithValue("@Responsavel", responsavel);
                cmd.Parameters.AddWithValue("@Motivo", motivo);
                cmd.Parameters.AddWithValue("@Observacao", ocorrencia);
                cmd.Parameters.AddWithValue("@Usuario_Inclusao", Session["UsuarioLogado"].ToString());

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    // Opcional: limpar ou fechar modal                    
                    ClientScript.RegisterStartupScript(this.GetType(), "HideModal", "hideModal();", true);
                }
                catch (Exception ex)
                {
                    // Logar ou exibir erro
                    Response.Write("<script>alert('Erro: " + ex.Message + "');</script>");
                }
            }
        }
    }
}