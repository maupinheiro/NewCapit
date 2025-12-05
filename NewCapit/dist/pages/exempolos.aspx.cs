using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NewCapit.dist.pages
{
    public partial class exempolos : System.Web.UI.Page
    {
        SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString());
        public string fotoMotorista;
        string codmot, caminhofoto;
        string num_coleta;
        DateTime dataHoraAtual = DateTime.Now;
        string sDuracao, sPercurso;
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
                    Response.Redirect("Login.aspx");
                }

                lblDtCadastro.Text = dataHoraAtual.ToString("dd/MM/yyyy HH:mm");
                txtCadastro.Text = dataHoraAtual.ToString("dd/MM/yyyy HH:mm");

                PreencherComboMotoristas();
                //CarregarGrid();
                PreencherNumColeta();
                fotoMotorista = "/img/totalFunc.png";
            }
            CarregaFoto();

        }
        
        
        
        
        private void PreencherComboMotoristas()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT codmot, nommot FROM tbmotoristas WHERE fl_exclusao is null AND status = 'ATIVO' ORDER BY nommot";

            // Crie uma conexão com o banco de dados
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                try
                {
                    // Abra a conexão com o banco de dados
                    conn.Open();

                    // Crie o comando SQL
                    SqlCommand cmd = new SqlCommand(query, conn);



                    SqlDataReader reader = cmd.ExecuteReader();

                    // Preencher o ComboBox com os dados do DataReader
                    ddlMotorista.DataSource = reader;
                    ddlMotorista.DataTextField = "nommot";
                    ddlMotorista.DataValueField = "codmot";
                    ddlMotorista.DataBind();
                    ddlMotorista.Items.Insert(0, "");

                    // Feche o reader
                    reader.Close();
                }
                catch (Exception ex)
                {
                    //    // Trate exceções
                    Response.Write("Erro: " + ex.Message);
                }
            }
        }
        protected void txtCodMotorista_TextChanged(object sender, EventArgs e)
        {
            if (txtCodMotorista.Text.Trim() != "")
            {
                txtCodMotorista.Text = txtCodMotorista.Text.ToUpper();
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
                            //if (File.Exists(path + file))
                            //{
                            //    fotoMotorista = "../../fotos/" + file + "";
                            //}
                            //else
                            //{
                            //    fotoMotorista = "/img/totalFunc.png";
                            //}
                        }
                        // Acione o toast quando a página for carregada
                        //string script = "<script>showToast('Motorista excluido ou inativo!');</script>";
                        //ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);                        
                        string script = "<script>showToast('Motorista inativo no sistema!');</script>";
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                        txtCodMotorista.Text = "";
                        txtCodMotorista.Focus();
                    }
                    else
                    {
                        txtFilialMot.Text = ConsultaMotorista.nucleo;
                        txtTipoMot.Text = ConsultaMotorista.tipomot;
                        txtFuncao.Text = ConsultaMotorista.cargo;
                        txtLibGR.Text = ConsultaMotorista.validade;
                        txtExameToxic.Text = ConsultaMotorista.venceti;
                        txtCNH.Text = ConsultaMotorista.venccnh.ToString("dd/MM/yyyy");
                        ddlMotorista.SelectedItem.Text = ConsultaMotorista.nommot;
                        txtCPF.Text = ConsultaMotorista.cpf;
                        txtCartao.Text = ConsultaMotorista.cartaomot;
                        txtValCartao.Text = ConsultaMotorista.venccartao;
                        txtCelular.Text = ConsultaMotorista.fone2;
                        txtCodTransportadora.Text = ConsultaMotorista.codtra;
                        txtTransportadora.Text = ConsultaMotorista.transp;

                        if (ConsultaMotorista.tipomot.Trim() == "AGREGADO" || ConsultaMotorista.tipomot.Trim() == "TERCEIRO")
                        {
                            txtCodVeiculo.Text = ConsultaMotorista.codvei;
                            txtPlaca.Text = ConsultaMotorista.placa;
                            txtTipoMot.Text = ConsultaMotorista.tipomot;
                            txtReboque1.Text = ConsultaMotorista.reboque1;
                            txtReboque2.Text = ConsultaMotorista.reboque2;
                            ETI.Visible = false;
                            var codigoAgregado = txtCodVeiculo.Text.Trim();

                            var objVeiculo = new Domain.ConsultaVeiculo
                            {
                                codvei = codigoAgregado
                            };
                            var ConsultaVeiculo = DAL.UsersDAL.CheckVeiculo(objVeiculo);
                            if (ConsultaVeiculo != null)
                            {
                                txtVeiculoTipo.Text = ConsultaVeiculo.tipoveiculo;
                                txtTipoVeiculo.Text = ConsultaVeiculo.tipvei;
                                txtOpacidade.Text = ConsultaVeiculo.vencimentolaudofumaca;
                                txtCRLVVeiculo.Text = ConsultaVeiculo.venclicenciamento;
                                txtCET.Text = ConsultaVeiculo.venclicencacet;
                                txtCarreta.Text = ConsultaVeiculo.tiporeboque;
                                txtTecnologia.Text = ConsultaVeiculo.rastreador;
                                txtRastreamento.Text = ConsultaVeiculo.rastreamento;
                                txtConjunto.Text = ConsultaVeiculo.tipocarreta;
                                txtCodProprietario.Text = ConsultaVeiculo.codtra;
                                txtProprietario.Text = ConsultaVeiculo.transp;
                                txtCrono.Text = ConsultaVeiculo.venccronotacografo;
                                //txtCapCarga.Text = ConsultaVeiculo.cap;
                            }
                            ETI.Visible = false;
                            valCET.Visible = false;
                            crono.Visible = false;

                        }

                        if (ConsultaMotorista.tipomot.Trim() == "FUNCIONÁRIO")
                        {
                            txtCodVeiculo.Text = ConsultaMotorista.frota;
                            txtFilialMot.Text = ConsultaMotorista.nucleo;
                            //txtPlaca.Text = ConsultaMotorista.placa;
                            txtVeiculoTipo.Text = "FROTA";
                            txtTipoVeiculo.Text = ConsultaMotorista.tipoveiculo;
                            //txtReboque1.Text = ConsultaMotorista.reboque1;
                            //txtReboque2.Text = ConsultaMotorista.reboque2;
                            ETI.Visible = true;
                            valCET.Visible = true;
                            crono.Visible = true;
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
                                txtCrono.Text = ConsultaVeiculo.venccronotacografo;
                                txtPlaca.Text = ConsultaVeiculo.plavei;
                                txtReboque1.Text = ConsultaVeiculo.reboque1;
                                txtReboque2.Text = ConsultaVeiculo.reboque2;
                                //txtCapCarga.Text = ConsultaVeiculo.cap;
                            }
                            // pesquisar validade do Exame Toxicologico
                            if (txtExameToxic.Text != "")
                            {
                                DateTime dataHoje = Convert.ToDateTime(DateTime.Now.Date);
                                DateTime dataETI = Convert.ToDateTime(txtExameToxic.Text).Date;

                                TimeSpan diferencaETI = dataETI - dataHoje;
                                // Agora você pode comparar a diferença
                                if (diferencaETI.TotalDays >= 1 && diferencaETI.TotalDays <= 30)
                                {
                                    txtExameToxic.BackColor = System.Drawing.Color.Khaki;
                                    txtExameToxic.ForeColor = System.Drawing.Color.OrangeRed;

                                    // Acione o toast quando a página for carregada
                                    //string script = "<script>showToast('Exame Toxicologico do Motorista, vence em menos de 30 dias!');</script>";
                                    //ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                                    string script = "<script>showToast('Exame Toxicologico do Motorista, vence em menos de 30 dias!');</script>";
                                    ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                                    txtCodFrota.Focus();

                                }
                                if (diferencaETI.TotalDays <= 0)
                                {
                                    txtExameToxic.BackColor = System.Drawing.Color.Red;
                                    txtExameToxic.ForeColor = System.Drawing.Color.White;

                                    // Acione o toast quando a página for carregada
                                    //string script = "<script>showToast('Exame Toxicologico do motorista, está vencido. Atualize no cadastro do motorista!');</script>";
                                    //ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                                    string script = "<script>showToast('Exame Toxicologico do Motorista, está vencido!');</script>";
                                    ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                                    txtCodMotorista.Text = "";
                                    txtCodMotorista.Focus();
                                }
                                else
                                {
                                    if (txtExameToxic.Text == "")
                                    {
                                        txtExameToxic.BackColor = System.Drawing.Color.Red;
                                        txtExameToxic.ForeColor = System.Drawing.Color.White;
                                        // Acione o toast quando a página for carregada                                        
                                        string script = "<script>showToast('Motorista, não tem Exame Toxicologico lançado no sistema!');</script>";
                                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                                        txtCodMotorista.Text = "";
                                        txtCodMotorista.Focus();
                                    }
                                }
                            }
                        }
                        // Pesquisar validade da CNH
                        if (txtCNH.Text != "")
                        {
                            DateTime dataHoje = Convert.ToDateTime(DateTime.Now.Date);
                            DateTime dataCNH = Convert.ToDateTime(txtCNH.Text).Date;

                            TimeSpan diferenca = dataCNH - dataHoje;
                            // Agora você pode comparar a diferença
                            if (diferenca.TotalDays >= 1 && diferenca.TotalDays <= 30)
                            {
                                txtCNH.BackColor = System.Drawing.Color.Khaki;
                                txtCNH.ForeColor = System.Drawing.Color.OrangeRed;
                                // Acione o toast quando a página for carregada
                                //string script = "<script>showToast('Validade da CNH do motorista, vence em menos de 30 dias!');</script>";
                                //ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);

                                string script = "<script>showToast('Validade da CNH do motorista, vence em menos de 30 dias!');</script>";
                                ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                                txtCodFrota.Focus();
                            }
                            if (diferenca.TotalDays <= 0)
                            {
                                txtCNH.BackColor = System.Drawing.Color.Red;
                                txtCNH.ForeColor = System.Drawing.Color.White;
                                string script = "<script>showToast('Validade da CNH do motorista, está vencida!');</script>";
                                ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                                txtCodMotorista.Text = "";
                                txtCodMotorista.Focus();
                            }
                            else
                            {
                                if (txtCNH.Text == "")
                                {
                                    txtCNH.BackColor = System.Drawing.Color.Red;
                                    txtCNH.ForeColor = System.Drawing.Color.White;
                                    string script = "<script>showToast('CNH do motorista, sem data de validade!');</script>";
                                    ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                                    txtCodMotorista.Text = "";
                                    txtCodMotorista.Focus();
                                }
                            }
                        }

                        // pesquisar validade da liberação GR
                        if (txtLibGR.Text != "")
                        {
                            string dataGRTexto = txtLibGR.Text;
                            DateTime data = DateTime.Parse(dataGRTexto);

                            DateTime dataHoje = Convert.ToDateTime(DateTime.Now.Date);
                            DateTime dataGR = Convert.ToDateTime(data).Date;

                            TimeSpan diferencaGR = dataGR - dataHoje;
                            // Agora você pode comparar a diferença
                            if (diferencaGR.TotalDays >= 0 && diferencaGR.TotalDays <= 30)
                            {
                                txtLibGR.BackColor = System.Drawing.Color.Khaki;
                                txtLibGR.ForeColor = System.Drawing.Color.OrangeRed;
                                string script = "<script>showToast('Motorista, sem liberação de risco cadastrada no sistema!');</script>";
                                ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                                txtCodFrota.Focus();

                                //string nomeUsuario = txtUsuCadastro.Text;
                                //string linha1 = "Olá, " + nomeUsuario + "!";
                                //string linha2 = "A liberãção de risco do motorista " + ddlMotorista.SelectedItem.Text.Trim() + ", vence em menos de 30 dias.";
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
                            if (diferencaGR.TotalDays <= 0)
                            {
                                txtLibGR.BackColor = System.Drawing.Color.Red;
                                txtLibGR.ForeColor = System.Drawing.Color.White;
                                string script = "<script>showToast('Liberação de Risco do Motorista, está vencida!');</script>";
                                ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                                txtCodFrota.Focus();
                                //string nomeUsuario = txtUsuCadastro.Text;
                                //string linha1 = "Olá, " + nomeUsuario + "!";
                                //string linha2 = "A liberação de risco do motorista " + ddlMotorista.SelectedItem.Text.Trim() + ", está vencida.";
                                //string linha3 = "Data do vencimento: " + dataGR.ToString("dd/MM/yyyy") + ".";
                                ////string linha4 = "Unidade: " + unidade + ". Por favor, verifique.";
                                //// Concatenando as linhas com '\n' para criar a mensagem
                                //string mensagem = $"{linha1}\n{linha2}\n{linha3}";
                                //string mensagemCodificada = HttpUtility.JavaScriptStringEncode(mensagem);
                                ////// Gerando o script JavaScript para exibir o alerta
                                //string script = $"alert('{mensagemCodificada}');";
                                ////// Registrando o script para execução no lado do cliente
                                //ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", script, true);
                                //txtCodMotorista.Text = "";
                                //txtCodMotorista.Focus();

                            }
                            else
                            {
                                if (txtLibGR.Text == "")
                                {
                                    txtLibGR.BackColor = System.Drawing.Color.Red;
                                    txtLibGR.ForeColor = System.Drawing.Color.White;
                                    string script = "<script>showToast('Motorista, sem liberação de risco cadastrada no sistema!');</script>";
                                    ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                                    txtCodMotorista.Text = "";
                                    txtCodMotorista.Focus();
                                }
                            }
                        }

                        // Pesquisar validade do Laudo de Fumaça
                        if (txtOpacidade.Text != "")
                        {
                            DateTime dataHoje = Convert.ToDateTime(DateTime.Now.Date);
                            DateTime dataOpacidade = Convert.ToDateTime(txtOpacidade.Text).Date;
                            txtOpacidade.Text = dataOpacidade.ToString("dd/MM/yyyy");
                            TimeSpan diferencaOpacidade = dataOpacidade - dataHoje;
                            // Agora você pode comparar a diferença
                            if (diferencaOpacidade.TotalDays >= 1 && diferencaOpacidade.TotalDays <= 30)
                            {
                                txtOpacidade.BackColor = System.Drawing.Color.Khaki;
                                txtOpacidade.ForeColor = System.Drawing.Color.OrangeRed;
                                // Acione o toast quando a página for carregada
                                //string script = "<script>showToast('Validade da CNH do motorista, vence em menos de 30 dias!');</script>";
                                //ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);                                
                                string script = "<script>showToast('Laudo de Opacidade do Veículo, vence em menos de 30 dias!');</script>";
                                ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                                // txtCodFrota.Focus();
                            }
                            if (diferencaOpacidade.TotalDays <= 0)
                            {
                                txtOpacidade.BackColor = System.Drawing.Color.Red;
                                txtOpacidade.ForeColor = System.Drawing.Color.White;
                                string script = "<script>showToast('Laudo de Opacidade do Veículo, está vencido!');</script>";
                                ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                                txtCodVeiculo.Text = "";
                                txtCodVeiculo.Focus();
                            }
                            else
                            {
                                if (txtOpacidade.Text == "")
                                {
                                    txtOpacidade.BackColor = System.Drawing.Color.Red;
                                    txtOpacidade.ForeColor = System.Drawing.Color.White;
                                    string script = "<script>showToast('Laudo de Opacidade do Veículo, não encontrado no sistema!');</script>";
                                    ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                                    txtOpacidade.Text = "";
                                    txtOpacidade.Focus();
                                }
                            }
                        }

                        // Pesquisar licenciamento do veiculo
                        if (txtCRLVVeiculo.Text != "")
                        {
                            DateTime dataHoje = Convert.ToDateTime(DateTime.Now.Date);
                            DateTime dataPlaca = Convert.ToDateTime(txtCRLVVeiculo.Text).Date;
                            txtCRLVVeiculo.Text = dataPlaca.ToString("dd/MM/yyyy");
                            TimeSpan diferencaPlaca = dataPlaca - dataHoje;
                            // Agora você pode comparar a diferença
                            if (diferencaPlaca.TotalDays >= 1 && diferencaPlaca.TotalDays <= 30)
                            {
                                txtCRLVVeiculo.BackColor = System.Drawing.Color.Khaki;
                                txtCRLVVeiculo.ForeColor = System.Drawing.Color.OrangeRed;
                                // Acione o toast quando a página for carregada
                                //string script = "<script>showToast('Validade da CNH do motorista, vence em menos de 30 dias!');</script>";
                                //ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);

                                string script = "<script>showToast('Licenciamento do Veículo, vence em menos de 30 dias!');</script>";
                                ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                                // txtCodFrota.Focus();
                            }
                            if (diferencaPlaca.TotalDays <= 0)
                            {
                                txtCRLVVeiculo.BackColor = System.Drawing.Color.Red;
                                txtCRLVVeiculo.ForeColor = System.Drawing.Color.White;
                                string script = "<script>showToast('Licenciamento do veículo, está vencido!');</script>";
                                ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                                txtCodVeiculo.Text = "";
                                txtCodVeiculo.Focus();
                            }
                            else
                            {
                                if (txtCRLVVeiculo.Text == "")
                                {
                                    txtCRLVVeiculo.BackColor = System.Drawing.Color.Red;
                                    txtCRLVVeiculo.ForeColor = System.Drawing.Color.White;
                                    string script = "<script>showToast('Veículo sem licenciamento lançado!');</script>";
                                    ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                                    txtCodVeiculo.Text = "";
                                    txtCodVeiculo.Focus();
                                }
                            }
                        }

                        // Pesquisar validade da Licença CET
                        if (txtVeiculoTipo.Text.Trim() == "FROTA")
                        {
                            if (txtCET.Text != "")
                            {
                                DateTime dataHoje = Convert.ToDateTime(DateTime.Now.Date);
                                DateTime dataCET = Convert.ToDateTime(txtOpacidade.Text).Date;
                                txtCET.Text = dataCET.ToString("dd/MM/yyyy");
                                TimeSpan diferencaCET = dataCET - dataHoje;
                                // Agora você pode comparar a diferença
                                if (diferencaCET.TotalDays >= 1 && diferencaCET.TotalDays <= 30)
                                {
                                    txtCET.BackColor = System.Drawing.Color.Khaki;
                                    txtCET.ForeColor = System.Drawing.Color.OrangeRed;
                                    // Acione o toast quando a página for carregada
                                    //string script = "<script>showToast('Validade da CNH do motorista, vence em menos de 30 dias!');</script>";
                                    //ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);                                  
                                    string script = "<script>showToast('Autorização CET do veículo, vence em menos de 30 dias!');</script>";
                                    ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                                    // txtCodFrota.Focus();
                                }
                                if (diferencaCET.TotalDays <= 0)
                                {
                                    txtCET.BackColor = System.Drawing.Color.Red;
                                    txtCET.ForeColor = System.Drawing.Color.White;
                                    string script = "<script>showToast('Autorização CET do veículo, está vencida. Pare o veículo imediatamente!');</script>";
                                    ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                                    txtCodVeiculo.Text = "";
                                    txtCodVeiculo.Focus();
                                }
                                else
                                {
                                    if (txtCET.Text == "")
                                    {
                                        txtCET.BackColor = System.Drawing.Color.Red;
                                        txtCET.ForeColor = System.Drawing.Color.White;
                                        string script = "<script>showToast('Laudo de Opacidade do Veículo, não encontrado no sistema!');</script>";
                                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                                        txtCET.Text = "";
                                        txtCET.Focus();
                                    }
                                }
                            }
                            if (txtCrono.Text != "")
                            {
                                DateTime dataHoje = Convert.ToDateTime(DateTime.Now.Date);
                                DateTime dataCrono = Convert.ToDateTime(txtCrono.Text).Date;
                                txtCrono.Text = dataCrono.ToString("dd/MM/yyyy");
                                TimeSpan diferencaCrono = dataCrono - dataHoje;
                                // Agora você pode comparar a diferença
                                if (diferencaCrono.TotalDays >= 1 && diferencaCrono.TotalDays <= 30)
                                {
                                    txtCET.BackColor = System.Drawing.Color.Khaki;
                                    txtCET.ForeColor = System.Drawing.Color.OrangeRed;
                                    // Acione o toast quando a página for carregada
                                    //string script = "<script>showToast('Validade da CNH do motorista, vence em menos de 30 dias!');</script>";
                                    //ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                                    string script = "<script>showToast('Cronotacografo do veículo, vence em menos de 30 dias!');</script>";
                                    ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                                    // txtCodFrota.Focus();
                                }
                                if (diferencaCrono.TotalDays <= 0)
                                {
                                    txtCET.BackColor = System.Drawing.Color.Red;
                                    txtCET.ForeColor = System.Drawing.Color.White;
                                    string script = "<script>showToast('Cronotacografo do veículo, está vencido. Pare o veículo imediatamente!');</script>";
                                    ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                                    txtCodVeiculo.Text = "";
                                    txtCodVeiculo.Focus();
                                }
                                else
                                {
                                    if (txtCrono.Text == "")
                                    {
                                        txtCrono.BackColor = System.Drawing.Color.Red;
                                        txtCrono.ForeColor = System.Drawing.Color.White;
                                        string script = "<script>showToast('Validade do cronotacografo, não lançada no sistema!');</script>";
                                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                                        txtCrono.Text = "";
                                        txtCrono.Focus();
                                    }
                                }
                            }

                            if (txtTipoVeiculo.Text.Trim() == "BITREM")
                            {
                                reboque1.Visible = true;
                                reboque2.Visible = true;
                                reb1.Visible = true;
                                reb2.Visible = true;

                                // pesquisar primeiro reboque
                                if (txtReboque1.Text != "")
                                {
                                    var placaReboque1 = txtReboque1.Text.Trim();

                                    var objCarreta = new Domain.ConsultaReboque
                                    {
                                        placacarreta = placaReboque1

                                    };
                                    var ConsultaReboque = DAL.UsersDAL.CheckReboque(objCarreta);
                                    if (ConsultaReboque != null)
                                    {
                                        txtCRLVReb1.Text = ConsultaReboque.licenciamento;
                                    }
                                    // Pesquisar licenciamento da carreta 1
                                    if (txtCRLVReb1.Text != "")
                                    {
                                        DateTime dataHoje = Convert.ToDateTime(DateTime.Now.Date);
                                        DateTime dataCRLVReboque1 = Convert.ToDateTime(txtCRLVReb1.Text).Date;
                                        txtCRLVReb1.Text = dataCRLVReboque1.ToString("dd/MM/yyyy");
                                        TimeSpan diferencaReb1 = dataCRLVReboque1 - dataHoje;
                                        // Agora você pode comparar a diferença
                                        if (diferencaReb1.TotalDays >= 1 && diferencaReb1.TotalDays <= 30)
                                        {
                                            txtCRLVReb1.BackColor = System.Drawing.Color.Khaki;
                                            txtCRLVReb1.ForeColor = System.Drawing.Color.OrangeRed;
                                            // Acione o toast quando a página for carregada
                                            //string script = "<script>showToast('Validade da CNH do motorista, vence em menos de 30 dias!');</script>";
                                            //ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                                            string script = "<script>showToast('Licenciamento da carreta, vence em menos de 30 dias!');</script>";
                                            ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                                            // txtCodFrota.Focus();
                                        }
                                        if (diferencaReb1.TotalDays <= 0)
                                        {
                                            txtCRLVReb1.BackColor = System.Drawing.Color.Red;
                                            txtCRLVReb1.ForeColor = System.Drawing.Color.White;
                                            string script = "<script>showToast('Licenciamento da carreta, está vencido!');</script>";
                                            ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                                            txtCodVeiculo.Text = "";
                                            txtCodVeiculo.Focus();
                                        }
                                        else
                                        {
                                            if (txtCodVeiculo.Text == "")
                                            {
                                                txtCRLVReb1.BackColor = System.Drawing.Color.Red;
                                                txtCRLVReb1.ForeColor = System.Drawing.Color.White;
                                                string script = "<script>showToast('Carreta sem licenciamento lançado!');</script>";
                                                ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                                                txtCodVeiculo.Text = "";
                                                txtCodVeiculo.Focus();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        txtCRLVReb1.BackColor = System.Drawing.Color.Red;
                                        txtCRLVReb1.ForeColor = System.Drawing.Color.White;
                                        string script = "<script>showToast('Carreta sem licenciamento lançado!');</script>";
                                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                                        txtCodVeiculo.Text = "";
                                        txtCodVeiculo.Focus();

                                    }
                                }

                                // pesquisar segundo reboque
                                if (txtReboque2.Text != "")
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

                                // Pesquisar licenciamento da carreta 2
                                if (txtCRLVReb2.Text != "")
                                {
                                    DateTime dataHoje = Convert.ToDateTime(DateTime.Now.Date);
                                    DateTime dataReboque2 = Convert.ToDateTime(txtCRLVReb2.Text).Date;
                                    txtCRLVReb2.Text = dataReboque2.ToString("dd/MM/yyyy");
                                    TimeSpan diferencaReboque2 = dataReboque2 - dataHoje;
                                    // Agora você pode comparar a diferença
                                    if (diferencaReboque2.TotalDays >= 1 && diferencaReboque2.TotalDays <= 30)
                                    {
                                        txtCRLVReb2.BackColor = System.Drawing.Color.Khaki;
                                        txtCRLVReb2.ForeColor = System.Drawing.Color.OrangeRed;
                                        string script = "<script>showToast('Licenciamento da Carreta, vence em menos de 30 dias!');</script>";
                                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                                        // txtCodFrota.Focus();
                                    }
                                    if (diferencaReboque2.TotalDays <= 0)
                                    {
                                        txtCRLVReb2.BackColor = System.Drawing.Color.Red;
                                        txtCRLVReb2.ForeColor = System.Drawing.Color.White;
                                        string script = "<script>showToast('Licenciamento da carreta, está vencido!');</script>";
                                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                                        txtCodVeiculo.Text = "";
                                        txtCodVeiculo.Focus();
                                    }
                                    else
                                    {
                                        if (txtCRLVReb2.Text == "")
                                        {
                                            txtCRLVReb2.BackColor = System.Drawing.Color.Red;
                                            txtCRLVReb2.ForeColor = System.Drawing.Color.White;
                                            string script = "<script>showToast('Carreta sem licenciamento lançado!');</script>";
                                            ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                                            txtCodVeiculo.Text = "";
                                            txtCodVeiculo.Focus();
                                        }
                                    }
                                }

                                // Pesquisar licenciamento da carreta 2

                            }

                            if (txtTipoVeiculo.Text.Trim() == "CAVALO SIMPLES" || txtTipoVeiculo.Text.Trim() == "CAVALO TRUCADO" || txtTipoVeiculo.Text.Trim() == "CAVALO 4 EIXOS")
                            {
                                reboque1.Visible = true;
                                reboque2.Visible = false;
                                reb1.Visible = true;
                                reb2.Visible = false;

                                // pesquisar primeiro reboque
                                if (txtReboque1.Text != "")
                                {
                                    var placaReboque1 = txtReboque1.Text.Trim();

                                    var objCarreta = new Domain.ConsultaReboque
                                    {
                                        placacarreta = placaReboque1

                                    };
                                    var ConsultaReboque = DAL.UsersDAL.CheckReboque(objCarreta);
                                    if (ConsultaReboque != null)
                                    {
                                        txtCRLVReb1.Text = ConsultaReboque.licenciamento;
                                    }
                                    // Pesquisar licenciamento da carreta 1
                                    if (txtCRLVReb1.Text != "")
                                    {
                                        DateTime dataHoje = Convert.ToDateTime(DateTime.Now.Date);
                                        DateTime dataCRLVReboque1 = Convert.ToDateTime(txtCRLVReb1.Text).Date;
                                        txtCRLVReb1.Text = dataCRLVReboque1.ToString("dd/MM/yyyy");
                                        TimeSpan diferencaReb1 = dataCRLVReboque1 - dataHoje;
                                        // Agora você pode comparar a diferença
                                        if (diferencaReb1.TotalDays >= 1 && diferencaReb1.TotalDays <= 30)
                                        {
                                            txtCRLVReb1.BackColor = System.Drawing.Color.Khaki;
                                            txtCRLVReb1.ForeColor = System.Drawing.Color.OrangeRed;
                                            // Acione o toast quando a página for carregada
                                            //string script = "<script>showToast('Validade da CNH do motorista, vence em menos de 30 dias!');</script>";
                                            //ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);  
                                            string script = "<script>showToast('Licenciamento da carreta, vence em menos de 30 dias!');</script>";
                                            ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                                            // txtCodFrota.Focus();
                                        }
                                        if (diferencaReb1.TotalDays <= 0)
                                        {
                                            txtCRLVReb1.BackColor = System.Drawing.Color.Red;
                                            txtCRLVReb1.ForeColor = System.Drawing.Color.White;
                                            string script = "<script>showToast('Licenciamento da carreta, está vencido!');</script>";
                                            ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                                            txtCodVeiculo.Text = "";
                                            txtCodVeiculo.Focus();
                                        }
                                        else
                                        {
                                            if (txtCodVeiculo.Text == "")
                                            {
                                                txtCRLVReb1.BackColor = System.Drawing.Color.Red;
                                                txtCRLVReb1.ForeColor = System.Drawing.Color.White;
                                                string script = "<script>showToast('Carreta sem licenciamento lançado!');</script>";
                                                ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                                                txtCodVeiculo.Text = "";
                                                txtCodVeiculo.Focus();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        txtCRLVReb1.BackColor = System.Drawing.Color.Red;
                                        txtCRLVReb1.ForeColor = System.Drawing.Color.White;
                                        string script = "<script>showToast('Carreta sem licenciamento lançado!');</script>";
                                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                                        txtCodVeiculo.Text = "";
                                        txtCodVeiculo.Focus();

                                    }
                                }

                            }

                        }
                        else if (txtVeiculoTipo.Text.Trim() == "AGREGADO" || txtVeiculoTipo.Text.Trim() == "TERCEIRO")
                        {
                            if (txtTipoVeiculo.Text.Trim() == "BITREM")
                            {
                                reboque1.Visible = true;
                                reboque2.Visible = true;
                                reb1.Visible = true;
                                reb2.Visible = true;

                                // pesquisar primeiro reboque
                                if (txtReboque1.Text != "")
                                {
                                    var placaReboque1 = txtReboque1.Text.Trim();

                                    var objCarreta = new Domain.ConsultaReboque
                                    {
                                        placacarreta = placaReboque1

                                    };
                                    var ConsultaReboque = DAL.UsersDAL.CheckReboque(objCarreta);
                                    if (ConsultaReboque != null)
                                    {
                                        txtCRLVReb1.Text = ConsultaReboque.licenciamento;
                                    }
                                    // Pesquisar licenciamento da carreta 1
                                    if (txtCRLVReb1.Text != "")
                                    {
                                        DateTime dataHoje = Convert.ToDateTime(DateTime.Now.Date);
                                        DateTime dataCRLVReboque1 = Convert.ToDateTime(txtCRLVReb1.Text).Date;
                                        txtCRLVReb1.Text = dataCRLVReboque1.ToString("dd/MM/yyyy");
                                        TimeSpan diferencaReb1 = dataCRLVReboque1 - dataHoje;
                                        // Agora você pode comparar a diferença
                                        if (diferencaReb1.TotalDays >= 1 && diferencaReb1.TotalDays <= 30)
                                        {
                                            txtCRLVReb1.BackColor = System.Drawing.Color.Khaki;
                                            txtCRLVReb1.ForeColor = System.Drawing.Color.OrangeRed;
                                            // Acione o toast quando a página for carregada
                                            //string script = "<script>showToast('Validade da CNH do motorista, vence em menos de 30 dias!');</script>";
                                            //ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script); 
                                            string script = "<script>showToast('Licenciamento da carreta, vence em menos de 30 dias!');</script>";
                                            ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                                            // txtCodFrota.Focus();
                                        }
                                        if (diferencaReb1.TotalDays <= 0)
                                        {
                                            txtCRLVReb1.BackColor = System.Drawing.Color.Red;
                                            txtCRLVReb1.ForeColor = System.Drawing.Color.White;
                                            string script = "<script>showToast('Licenciamento da carreta, está vencido!');</script>";
                                            ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                                            txtCodVeiculo.Text = "";
                                            txtCodVeiculo.Focus();
                                        }
                                        else
                                        {
                                            if (txtCodVeiculo.Text == "")
                                            {
                                                txtCRLVReb1.BackColor = System.Drawing.Color.Red;
                                                txtCRLVReb1.ForeColor = System.Drawing.Color.White;
                                                string script = "<script>showToast('Carreta sem licenciamento lançado!');</script>";
                                                ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                                                txtCodVeiculo.Text = "";
                                                txtCodVeiculo.Focus();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        txtCRLVReb1.BackColor = System.Drawing.Color.Red;
                                        txtCRLVReb1.ForeColor = System.Drawing.Color.White;
                                        string script = "<script>showToast('Carreta sem licenciamento lançado!');</script>";
                                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                                        txtCodVeiculo.Text = "";
                                        txtCodVeiculo.Focus();

                                    }
                                }

                                // pesquisar segundo reboque
                                if (txtReboque2.Text != "")
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

                                // Pesquisar licenciamento da carreta 2
                                if (txtCRLVReb2.Text != "")
                                {
                                    DateTime dataHoje = Convert.ToDateTime(DateTime.Now.Date);
                                    DateTime dataReboque2 = Convert.ToDateTime(txtCRLVReb2.Text).Date;
                                    txtCRLVReb2.Text = dataReboque2.ToString("dd/MM/yyyy");
                                    TimeSpan diferencaReboque2 = dataReboque2 - dataHoje;
                                    // Agora você pode comparar a diferença
                                    if (diferencaReboque2.TotalDays >= 1 && diferencaReboque2.TotalDays <= 30)
                                    {
                                        txtCRLVReb2.BackColor = System.Drawing.Color.Khaki;
                                        txtCRLVReb2.ForeColor = System.Drawing.Color.OrangeRed;
                                        string script = "<script>showToast('Licenciamento da Carreta, vence em menos de 30 dias!');</script>";
                                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                                        // txtCodFrota.Focus();
                                    }
                                    if (diferencaReboque2.TotalDays <= 0)
                                    {
                                        txtCRLVReb2.BackColor = System.Drawing.Color.Red;
                                        txtCRLVReb2.ForeColor = System.Drawing.Color.White;
                                        string script = "<script>showToast('Licenciamento da carreta, está vencido!');</script>";
                                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                                        txtCodVeiculo.Text = "";
                                        txtCodVeiculo.Focus();
                                    }
                                    else
                                    {
                                        if (txtCRLVReb2.Text == "")
                                        {
                                            txtCRLVReb2.BackColor = System.Drawing.Color.Red;
                                            txtCRLVReb2.ForeColor = System.Drawing.Color.White;
                                            string script = "<script>showToast('Carreta sem licenciamento lançado!');</script>";
                                            ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                                            txtCodVeiculo.Text = "";
                                            txtCodVeiculo.Focus();
                                        }
                                    }
                                }

                                // Pesquisar licenciamento da carreta 2

                            }
                            else if (txtTipoVeiculo.Text.Trim() == "CAVALO SIMPLES")
                            {
                                reboque1.Visible = true;
                                reboque2.Visible = false;
                                reb1.Visible = true;
                                reb2.Visible = false;

                                // pesquisar primeiro reboque
                                if (txtReboque1.Text != "")
                                {
                                    var placaReboque1 = txtReboque1.Text.Trim();

                                    var objCarreta = new Domain.ConsultaReboque
                                    {
                                        placacarreta = placaReboque1

                                    };
                                    var ConsultaReboque = DAL.UsersDAL.CheckReboque(objCarreta);
                                    if (ConsultaReboque != null)
                                    {
                                        txtCRLVReb1.Text = ConsultaReboque.licenciamento;
                                    }
                                    // Pesquisar licenciamento da carreta 1
                                    if (txtCRLVReb1.Text != "")
                                    {
                                        DateTime dataHoje = Convert.ToDateTime(DateTime.Now.Date);
                                        DateTime dataCRLVReboque1 = Convert.ToDateTime(txtCRLVReb1.Text).Date;
                                        txtCRLVReb1.Text = dataCRLVReboque1.ToString("dd/MM/yyyy");
                                        TimeSpan diferencaReb1 = dataCRLVReboque1 - dataHoje;
                                        // Agora você pode comparar a diferença
                                        if (diferencaReb1.TotalDays >= 1 && diferencaReb1.TotalDays <= 30)
                                        {
                                            txtCRLVReb1.BackColor = System.Drawing.Color.Khaki;
                                            txtCRLVReb1.ForeColor = System.Drawing.Color.OrangeRed;
                                            // Acione o toast quando a página for carregada
                                            //string script = "<script>showToast('Validade da CNH do motorista, vence em menos de 30 dias!');</script>";
                                            //ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);                                           
                                            string script = "<script>showToast('Licenciamento da carreta, vence em menos de 30 dias!');</script>";
                                            ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                                            // txtCodFrota.Focus();
                                        }
                                        if (diferencaReb1.TotalDays <= 0)
                                        {
                                            txtCRLVReb1.BackColor = System.Drawing.Color.Red;
                                            txtCRLVReb1.ForeColor = System.Drawing.Color.White;
                                            string script = "<script>showToast('Licenciamento da carreta, está vencido!');</script>";
                                            ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);

                                            txtCodVeiculo.Text = "";
                                            txtCodVeiculo.Focus();
                                        }
                                        else
                                        {
                                            if (txtCodVeiculo.Text == "")
                                            {
                                                txtCRLVReb1.BackColor = System.Drawing.Color.Red;
                                                txtCRLVReb1.ForeColor = System.Drawing.Color.White;
                                                string script = "<script>showToast('Carreta sem licenciamento lançado!');</script>";
                                                ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                                                txtCodVeiculo.Text = "";
                                                txtCodVeiculo.Focus();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        txtCRLVReb1.BackColor = System.Drawing.Color.Red;
                                        txtCRLVReb1.ForeColor = System.Drawing.Color.White;
                                        string script = "<script>showToast('Carreta sem licenciamento lançado!');</script>";
                                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);

                                        txtCodVeiculo.Text = "";
                                        txtCodVeiculo.Focus();

                                    }
                                }
                            }
                            else if (txtTipoVeiculo.Text.Trim() == "CAVALO TRUCADO")
                            {
                                reboque1.Visible = true;
                                reboque2.Visible = false;
                                reb1.Visible = true;
                                reb2.Visible = false;

                                // pesquisar primeiro reboque
                                if (txtReboque1.Text != "")
                                {
                                    var placaReboque1 = txtReboque1.Text.Trim();

                                    var objCarreta = new Domain.ConsultaReboque
                                    {
                                        placacarreta = placaReboque1

                                    };
                                    var ConsultaReboque = DAL.UsersDAL.CheckReboque(objCarreta);
                                    if (ConsultaReboque != null)
                                    {
                                        txtCRLVReb1.Text = ConsultaReboque.licenciamento;
                                    }
                                    // Pesquisar licenciamento da carreta 1
                                    if (txtCRLVReb1.Text != "")
                                    {
                                        DateTime dataHoje = Convert.ToDateTime(DateTime.Now.Date);
                                        DateTime dataCRLVReboque1 = Convert.ToDateTime(txtCRLVReb1.Text).Date;
                                        txtCRLVReb1.Text = dataCRLVReboque1.ToString("dd/MM/yyyy");
                                        TimeSpan diferencaReb1 = dataCRLVReboque1 - dataHoje;
                                        // Agora você pode comparar a diferença
                                        if (diferencaReb1.TotalDays >= 1 && diferencaReb1.TotalDays <= 30)
                                        {
                                            txtCRLVReb1.BackColor = System.Drawing.Color.Khaki;
                                            txtCRLVReb1.ForeColor = System.Drawing.Color.OrangeRed;
                                            // Acione o toast quando a página for carregada
                                            //string script = "<script>showToast('Validade da CNH do motorista, vence em menos de 30 dias!');</script>";
                                            //ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);


                                            string script = "<script>showToast('Licenciamento da carreta, vence em menos de 30 dias!');</script>";
                                            ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);

                                            // txtCodFrota.Focus();
                                        }
                                        if (diferencaReb1.TotalDays <= 0)
                                        {
                                            txtCRLVReb1.BackColor = System.Drawing.Color.Red;
                                            txtCRLVReb1.ForeColor = System.Drawing.Color.White;
                                            string script = "<script>showToast('Licenciamento da carreta, está vencido!');</script>";
                                            ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);

                                            txtCodVeiculo.Text = "";
                                            txtCodVeiculo.Focus();
                                        }
                                        else
                                        {
                                            if (txtCodVeiculo.Text == "")
                                            {
                                                txtCRLVReb1.BackColor = System.Drawing.Color.Red;
                                                txtCRLVReb1.ForeColor = System.Drawing.Color.White;
                                                string script = "<script>showToast('Carreta sem licenciamento lançado!');</script>";
                                                ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                                                txtCodVeiculo.Text = "";
                                                txtCodVeiculo.Focus();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        txtCRLVReb1.BackColor = System.Drawing.Color.Red;
                                        txtCRLVReb1.ForeColor = System.Drawing.Color.White;
                                        string script = "<script>showToast('Carreta sem licenciamento lançado!');</script>";
                                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);

                                        txtCodVeiculo.Text = "";
                                        txtCodVeiculo.Focus();

                                    }
                                }

                            }
                            else if (txtTipoVeiculo.Text.Trim() == "CAVALO 4 EIXOS")
                            {
                                reboque1.Visible = true;
                                reboque2.Visible = false;
                                reb1.Visible = true;
                                reb2.Visible = false;

                                // pesquisar primeiro reboque
                                if (txtReboque1.Text != "")
                                {
                                    var placaReboque1 = txtReboque1.Text.Trim();

                                    var objCarreta = new Domain.ConsultaReboque
                                    {
                                        placacarreta = placaReboque1

                                    };
                                    var ConsultaReboque = DAL.UsersDAL.CheckReboque(objCarreta);
                                    if (ConsultaReboque != null)
                                    {
                                        txtCRLVReb1.Text = ConsultaReboque.licenciamento;
                                    }
                                    // Pesquisar licenciamento da carreta 1
                                    if (txtCRLVReb1.Text != "")
                                    {
                                        DateTime dataHoje = Convert.ToDateTime(DateTime.Now.Date);
                                        DateTime dataCRLVReboque1 = Convert.ToDateTime(txtCRLVReb1.Text).Date;
                                        txtCRLVReb1.Text = dataCRLVReboque1.ToString("dd/MM/yyyy");
                                        TimeSpan diferencaReb1 = dataCRLVReboque1 - dataHoje;
                                        // Agora você pode comparar a diferença
                                        if (diferencaReb1.TotalDays >= 1 && diferencaReb1.TotalDays <= 30)
                                        {
                                            txtCRLVReb1.BackColor = System.Drawing.Color.Khaki;
                                            txtCRLVReb1.ForeColor = System.Drawing.Color.OrangeRed;
                                            string script = "<script>showToast('Validade da CNH do motorista, vence em menos de 30 dias!');</script>";
                                            ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);



                                            // Acione o toast quando a página for carregada
                                            //string script = "<script>showToast('Validade da CNH do motorista, vence em menos de 30 dias!');</script>";
                                            //ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);

                                            // ShowToastrWarningVeiculo("Licenciamento da carreta, vence em menos de 30 dias!");
                                            // txtCodFrota.Focus();
                                        }
                                        if (diferencaReb1.TotalDays <= 0)
                                        {
                                            txtCRLVReb1.BackColor = System.Drawing.Color.Red;
                                            txtCRLVReb1.ForeColor = System.Drawing.Color.White;
                                            string script = "<script>showToast('Licenciamento da carreta, está vencido!');</script>";
                                            ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                                            txtCodVeiculo.Text = "";
                                            txtCodVeiculo.Focus();
                                        }
                                        else
                                        {
                                            if (txtCodVeiculo.Text == "")
                                            {
                                                txtCRLVReb1.BackColor = System.Drawing.Color.Red;
                                                txtCRLVReb1.ForeColor = System.Drawing.Color.White;
                                                string script = "<script>showToast('Carreta sem licenciamento lançado!');</script>";
                                                ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                                                txtCodVeiculo.Text = "";
                                                txtCodVeiculo.Focus();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        txtCRLVReb1.BackColor = System.Drawing.Color.Red;
                                        txtCRLVReb1.ForeColor = System.Drawing.Color.White;
                                        // Acione o toast quando a página for carregada
                                        string script = "<script>showToast('Carreta sem licenciamento lançado!');</script>";
                                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                                        txtCodVeiculo.Text = "";
                                        txtCodVeiculo.Focus();

                                    }
                                }

                            }
                        }

                    }
                }
                else
                {
                    // Acione o toast quando a página for carregada
                    //string script = "<script>showToast('Validade da CNH do motorista, vence em menos de 30 dias!');</script>";
                    //ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);

                    // Acione o toast quando a página for carregada
                    string script = "<script>showToast('Motorista não cadastrado no sistema!');</script>";
                    ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                    fotoMotorista = "/img/totalFunc.png";
                    txtCodMotorista.Text = "";
                    txtCodMotorista.Focus();

                }
            }

        }
        protected void txtCodFrota_TextChanged(object sender, EventArgs e)
        {
            if (txtCodFrota.Text.Trim() != "")
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


                }
                else
                {
                    txtCodContato.Text = txtCodFrota.Text;
                    txtCadCelular.Focus();
                    // Exemplo: abrir o modal ao carregar a página
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "abrirModalTelefone", "abrirModalTelefone();", true);
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

                        //if (!File.Exists(fotoMotorista))
                        //{
                        //    fotoMotorista = ConsultaMotorista.caminhofoto.Trim().ToString();
                        //}
                        //else
                        //{
                        //    fotoMotorista = "/img/totalFunc.png";
                        //}
                    }

                }

            }

        }
        private void PreencherNumColeta()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT (carga + incremento) as ProximaColeta FROM tbcontadores";

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
                        // Acione o toast quando a página for carregada
                        string script = "<script>showToast('ID invalido ou não fornecido.');</script>";
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
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
                                // Acione o toast quando a página for carregada
                                string script = "<script>showToast('Erro ao atualizar o número do carregamento.');</script>";
                                ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
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
        protected void txtCarga_TextChanged(object sender, EventArgs e)
        {
            if (txtCarga.Text.Trim() == "")
            {
                string nomeUsuario = txtUsuCadastro.Text;

                string linha1 = "Olá, " + nomeUsuario + "!";
                string linha2 = "Por favor, digite o número da coleta.";

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

                string codigoPlaca = txtCarga.Text.Trim();
                string strConn = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    string query = "SELECT carga, cliorigem, clidestino, status FROM tbcargas WHERE carga = @Codigo ";

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
                                    searchTerm = txtCarga.Text.Trim();
                                    CarregarColetas(searchTerm);
                                    txtCarga.Text = string.Empty;
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

                                    txtCarga.Text = "";
                                    txtCarga.Focus();
                                }

                            }
                            else
                            {
                                // se a coleta for = 1 abre o modal para colocar origem e destino

                                if (txtCarga.Text == "1")
                                {
                                    // Exemplo: abrir o modal ao carregar a página
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "abrirModal", "abrirModal();", true);
                                }
                                else
                                {
                                    string nomeUsuario = txtUsuCadastro.Text;

                                    string linha1 = "Olá, " + nomeUsuario + "!";
                                    string linha2 = "Coleta " + txtCarga.Text.Trim() + ", não encontrada. Verifique o número digitado.";

                                    // Concatenando as linhas com '\n' para criar a mensagem
                                    string mensagem = $"{linha1}\n{linha2}";

                                    string mensagemCodificada = HttpUtility.JavaScriptStringEncode(mensagem);
                                    //// Gerando o script JavaScript para exibir o alerta
                                    string script = $"alert('{mensagemCodificada}');";

                                    //// Registrando o script para execução no lado do cliente
                                    ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", script, true);

                                    txtCarga.Text = "";
                                    txtCarga.Focus();
                                }


                            }
                        }
                    }

                }







            }
        }
        protected void txtCodVeiculo_TextChanged(object sender, EventArgs e)
        {
            if (txtCodVeiculo.Text.Trim() != "")
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
                        txtPlaca.Text = "";
                        txtReboque1.Text = "";
                        txtReboque2.Text = "";
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
                        txtCodVeiculo.Focus();
                    }
                    else
                    {
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
                        //txtCapCarga.Text = ConsultaVeiculo.cap;
                        // verifica se o motorista pertence a transportadora
                        if (txtCodTransportadora.Text.Trim() != txtCodProprietario.Text.Trim())
                        {

                            string nomeUsuario = txtUsuCadastro.Text;
                            string linha1 = "Olá, " + nomeUsuario + "!";
                            string linha2 = "O motorista " + ddlMotorista.SelectedItem.Text.Trim() + ", não pertence a transportadora do veículo " + txtProprietario.Text.Trim() + ".";
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
                            txtCodVeiculo.Text = "";
                            txtPlaca.Text = "";
                            txtReboque1.Text = "";
                            txtReboque2.Text = "";
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
                            txtCodVeiculo.Focus();


                        }
                        // verifica se a funcao do motorista permite dirigir o veiculo
                        string primeiraLetraString = txtFuncao.Text.Trim().Substring(0, 1);
                        if (txtFuncao.Text != "")
                        {
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
                                    string linha2 = "O motorista " + ddlMotorista.SelectedItem.Text.Trim() + ", não é apto a dirigir este tipo de veículo " + txtTipoVeiculo.Text.Trim() + ".";
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
                                    txtCodVeiculo.Text = "";
                                    txtPlaca.Text = "";
                                    txtReboque1.Text = "";
                                    txtReboque2.Text = "";
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
                                    txtCodVeiculo.Focus();
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
                                    string linha2 = "O motorista " + ddlMotorista.SelectedItem.Text.Trim() + ", não é apto a dirigir este tipo de veículo " + txtTipoVeiculo.Text.Trim() + ".";
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
                                    txtCodVeiculo.Text = "";
                                    txtPlaca.Text = "";
                                    txtReboque1.Text = "";
                                    txtReboque2.Text = "";
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
                                    txtCodVeiculo.Focus();
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
                                    string linha2 = "O motorista " + ddlMotorista.SelectedItem.Text.Trim() + ", não é apto a dirigir este tipo de veículo " + txtTipoVeiculo.Text.Trim() + ".";
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
                                    txtCodVeiculo.Text = "";
                                    txtPlaca.Text = "";
                                    txtReboque1.Text = "";
                                    txtReboque2.Text = "";
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
                                    txtCodVeiculo.Focus();
                                }

                            }
                            else
                            {
                                string nomeUsuario = txtUsuCadastro.Text;
                                string linha1 = "Olá, " + nomeUsuario + "!";
                                string linha2 = "O motorista " + ddlMotorista.SelectedItem.Text.Trim() + ", não é apto a dirigir nenhum tipo de veículo.";
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
                                txtCodVeiculo.Text = "";
                                txtPlaca.Text = "";
                                txtReboque1.Text = "";
                                txtReboque2.Text = "";
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
                                txtCodVeiculo.Focus();
                            }
                        }


                        //// pesquisar primeiro reboque1
                        if (txtTipoVeiculo.Text.Trim() == "CAVALO SIMPLES" || txtTipoVeiculo.Text.Trim() == "CAVALO TRUCADO" || txtTipoVeiculo.Text.Trim() == "CAVALO 4 EIXOS")
                        {
                            if (txtReboque1.Text == "")
                            {
                                string nomeUsuario = txtUsuCadastro.Text;
                                string linha1 = "Olá, " + nomeUsuario + "!";
                                string linha2 = "Veículo digitado " + txtPlaca.Text.Trim() + ", não tem reboque engatado.";
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
                                txtCodVeiculo.Text = "";
                                txtPlaca.Text = "";
                                txtReboque1.Text = "";
                                txtReboque2.Text = "";
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
                                txtCodVeiculo.Focus();

                            }

                        }
                        //// Verifica reboque 2 - bitrem
                        if (txtTipoVeiculo.Text.Trim() == "BITREM")
                        {
                            if (txtReboque2.Text == "")
                            {
                                string nomeUsuario = txtUsuCadastro.Text;
                                string linha1 = "Olá, " + nomeUsuario + "!";
                                string linha2 = "Veículo digitado " + txtPlaca.Text.Trim() + ", trata-se de um Bitrem, não tem o segundo reboque engatado.";
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
                                txtCodVeiculo.Text = "";
                                txtPlaca.Text = "";
                                txtReboque1.Text = "";
                                txtReboque2.Text = "";
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
                                txtCodVeiculo.Focus();
                            }
                        }
                    }
                }
                else
                {

                    string script = "<script>showToast('Motorista digitado, não encontrado no sistema. Verifique o código digitado!');</script>";
                    ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);

                    txtCodMotorista.Text = "";
                    txtCodMotorista.Focus();

                }

            }

        }
        private void CarregarColetas(string searchTerm = "")
        {
            foreach (Repeater ri in rptCarregamento.Items)
            {
                var novosDados = DAL.ConCargas.FetchDataTableCargasMatriz2(searchTerm);

                DataTable dadosAtuais = ViewState["Coletas"] as DataTable;

                var carga = new Domain.ConsultaCarga
                {
                    carga = searchTerm
                };
                var ConsultaCarga = DAL.ConCargas.CheckCargasMatriz(carga);

                if (ConsultaCarga != null)
                {
                    if (ConsultaCarga.codmot == "" || ConsultaCarga.codmot == null)
                    {
                        if (dadosAtuais == null)
                        {
                            dadosAtuais = novosDados.Clone(); // estrutura idêntica
                        }

                        // Adiciona somente as coletas que ainda não estão em dadosAtuais
                        foreach (DataRow novaRow in novosDados.Rows)
                        {
                            string novaCarga = novaRow["carga"].ToString();

                            bool jaExiste = dadosAtuais.AsEnumerable()
                                .Any(r => r["carga"].ToString() == novaCarga);

                            if (!jaExiste)
                            {
                                dadosAtuais.ImportRow(novaRow);
                            }
                        }

                        ViewState["Coletas"] = dadosAtuais;
                        Repeater rptColetas = (Repeater)ri.FindControl("rptColetas");
                        rptColetas.DataSource = dadosAtuais;
                        rptColetas.DataBind();
                        lblMensagem.Text = string.Empty;
                    }
                    else
                    {
                        lblMensagem.Text = "Coleta em andamento ou já concluida!";
                    }
                }
            }


        }

        protected void rptCarregamento_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item ||
                e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Repeater rptColeta = (Repeater)e.Item.FindControl("rptColeta");

                string cargaid = DataBinder.Eval(e.Item.DataItem, "carga").ToString();

                //if (rptColeta != null && !string.IsNullOrEmpty(cargaid))
                //{
                DataTable dtColetas = DAL.ConCargas.FetchDataTableCargasMatriz2(cargaid);
                rptColeta.DataSource = dtColetas;
                rptColeta.DataBind();
                //}
            }
        }


        protected void ddlCliInicial_TextChanged(object sender, EventArgs e)
        {
            codCliInicial.Text = ddlCliInicial.SelectedValue;
        }

        protected void ddlCliFinal_TextChanged(object sender, EventArgs e)
        {
            codCliFinal.Text = ddlCliFinal.SelectedValue;
            string sql = "select Distancia, UF_Origem, Origem, UF_Destino, Destino from tbdistanciapremio where UF_Origem=(SELECT estcli FROM tbclientes where codcli='" + ddlCliInicial.SelectedValue + "') and Origem=(SELECT cidcli FROM tbclientes where codcli='" + ddlCliInicial.SelectedValue + "') and UF_Destino=(SELECT estcli FROM tbclientes where codcli='" + ddlCliFinal.SelectedValue + "') and Destino=(SELECT cidcli FROM tbclientes where codcli='" + ddlCliFinal.SelectedValue + "')";
            SqlDataAdapter adp = new SqlDataAdapter(sql, conn);
            DataTable dt = new DataTable();
            conn.Open();
            adp.Fill(dt);
            conn.Close();
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

        protected void rptColeta_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            string previsaoStr = DataBinder.Eval(e.Item.DataItem, "previsao")?.ToString();
            //string dataHoraStr = DataBinder.Eval(e.Item.DataItem, "data_hora")?.ToString();
            //string status = DataBinder.Eval(e.Item.DataItem, "status")?.ToString();

            //Label lblAtendimento = (Label)e.Item.FindControl("lblAtendimento");
            //HtmlTableCell tdAtendimento = (HtmlTableCell)e.Item.FindControl("tdAtendimento");

            //if (lblAtendimento != null && tdAtendimento != null)
            //{
            //    DateTime previsao, dataHora;
            //    DateTime agora = DateTime.Now;

            //    if (DateTime.TryParse(previsaoStr, out previsao) && DateTime.TryParse(dataHoraStr, out dataHora))
            //    {
            //        DateTime dataPrevisao = previsao.Date;
            //        DateTime dataHoraComparacao = new DateTime(
            //            dataPrevisao.Year, dataPrevisao.Month, dataPrevisao.Day,
            //            dataHora.Hour, dataHora.Minute, dataHora.Second
            //        );

            //        if (dataHoraComparacao < agora && (status == "Concluído" || status == "Pendente"))
            //        {
            //            lblAtendimento.Text = "Atrasado";
            //            tdAtendimento.BgColor = "Red";
            //            tdAtendimento.Attributes["style"] = "color: white; font-weight: bold;";
            //        }
            //        else if (dataHoraComparacao.Date == agora.Date && dataHoraComparacao.TimeOfDay <= agora.TimeOfDay
            //                 && (status == "Concluído" || status == "Pendente"))
            //        {
            //            lblAtendimento.Text = "No Prazo";
            //            tdAtendimento.BgColor = "Green";
            //            tdAtendimento.Attributes["style"] = "color: white; font-weight: bold;";
            //        }
            //        else if (dataHoraComparacao > agora && status == "Concluído")
            //        {
            //            lblAtendimento.Text = "Antecipado";
            //            tdAtendimento.BgColor = "Orange";
            //            tdAtendimento.Attributes["style"] = "color: white; font-weight: bold;";
            //        }
            //        else
            //        {
            //            lblAtendimento.Text = status;

            //        }
            //    }
            //}


        }
    }
}