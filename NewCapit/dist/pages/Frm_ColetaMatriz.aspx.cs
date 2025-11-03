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
using static NPOI.HSSF.Util.HSSFColor;
using System.Threading;
using System.Diagnostics.Eventing.Reader;
using System.Web.Services.Description;
using NPOI.SS.UserModel;
using ICSharpCode.SharpZipLib.Zip;
using MathNet.Numerics.Providers.SparseSolver;
using System.Drawing.Drawing2D;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using System.Runtime.InteropServices.ComTypes;
using System.Globalization;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Drawing;
using System.Web.Script.Serialization;

namespace NewCapit.dist.pages
{
    public partial class Frm_ColetaMatriz : System.Web.UI.Page
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["conexao"].ToString());
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
            PreencherComboMateriais();
            PreencherComboStatus();

        }
        protected void btnSelecionar_Click(object sender, EventArgs e)
        {
            List<string> selecionados = new List<string>();

            foreach (GridViewRow row in gvPedidos.Rows)
            {
                System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)row.FindControl("chkSelect");
                if (chk != null && chk.Checked)
                {
                    string id = gvPedidos.DataKeys[row.RowIndex].Value.ToString();
                    selecionados.Add(id);
                }
            }

            // Exemplo: mostrar resultado
            string resultado = "Selecionados: " + string.Join(", ", selecionados);
            Response.Write("<script>alert('" + resultado + "');</script>");
        }
        //private void CarregarGrid()
        //{
        //    // Consulta SQL que retorna os dados desejados
        //    string query = "SELECT  ID, codmot, nommot FROM tbmotoristas WHERE fl_exclusao is null AND status = 'ATIVO' ORDER BY nommot";

        //    //using (SqlConnection conn = new SqlConnection(conexao))
        //    //{
        //    // Crie uma conexão com o banco de dados
        //    using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
        //    {
        //            try
        //            {
        //                SqlDataAdapter da = new SqlDataAdapter(query, conn);
        //                DataTable dt = new DataTable();
        //                da.Fill(dt);
        //                gvPedidos.DataSource = dt;
        //                gvPedidos.DataBind();
        //            }
        //            catch (Exception ex)
        //            {
        //                //    // Trate exceções
        //                Response.Write("Erro: " + ex.Message);
        //            }
        //    }
        //    //}
        //}
        protected void gvPedidos_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvPedidos.EditIndex = e.NewEditIndex;
            string numeroCarga = txtCarga.Text.Trim();
            CarregarGridPedidos(numeroCarga);
        }
        protected void gvPedidos_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvPedidos.EditIndex = -1;
            string numeroCarga = txtCarga.Text.Trim();
            CarregarGridPedidos(numeroCarga);
        }
        protected void gvPedidos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && gvPedidos.EditIndex == e.Row.RowIndex)
            {
                DropDownList ddl = (DropDownList)e.Row.FindControl("ddlMotoristas");

                if (ddl != null)
                {
                    // Carrega os cargos no DropDownList
                    using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
                    {
                        string query = "SELECT codmot, nommot FROM tbmotoristas";
                        SqlDataAdapter da = new SqlDataAdapter(query, conn);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        ddl.DataSource = dt;
                        ddl.DataTextField = "nommot";
                        ddl.DataValueField = "codmot";
                        ddl.DataBind();

                        // Seleciona o valor atual
                        string cargoIdAtual = DataBinder.Eval(e.Row.DataItem, "nommot").ToString();
                        ddl.SelectedValue = cargoIdAtual;
                    }
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
                            if (File.Exists(path + file))
                            {
                                fotoMotorista = "../../fotos/" + file + "";
                            }
                            else
                            {
                                fotoMotorista = "/img/totalFunc.png";
                            }
                        }
                        // Acione o toast quando a página for carregada
                        //string script = "<script>showToast('Motorista excluido ou inativo!');</script>";
                        //ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                        ShowToastrInfo("Motorista inativo no sistema!");
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
                                txtCapCarga.Text = ConsultaVeiculo.cap;
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
                                txtCapCarga.Text = ConsultaVeiculo.cap;
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


                                    ShowToastrWarning("Exame Toxicologico do Motorista, vence em menos de 30 dias!");
                                    txtCodFrota.Focus();

                                }
                                if (diferencaETI.TotalDays <= 0)
                                {
                                    txtExameToxic.BackColor = System.Drawing.Color.Red;
                                    txtExameToxic.ForeColor = System.Drawing.Color.White;

                                    // Acione o toast quando a página for carregada
                                    //string script = "<script>showToast('Exame Toxicologico do motorista, está vencido. Atualize no cadastro do motorista!');</script>";
                                    //ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                                    ShowToastrError("Exame Toxicologico do motorista, está vencido. Atualize no cadastro do motorista!");
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
                                        ShowToastrError("Motorista, não tem Exame Toxicologico lançado no sistema!");                                        
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

                                ShowToastrWarning("Validade da CNH do motorista, vence em menos de 30 dias!");
                                txtCodFrota.Focus();
                            }
                            if (diferenca.TotalDays <= 0)
                            {
                                txtCNH.BackColor = System.Drawing.Color.Red;
                                txtCNH.ForeColor = System.Drawing.Color.White;                               
                                ShowToastrError("Validade da CNH do motorista, está vencida!");
                                txtCodMotorista.Text = "";
                                txtCodMotorista.Focus();
                            }
                            else
                            {
                                if (txtCNH.Text == "")
                                {
                                    txtCNH.BackColor = System.Drawing.Color.Red;
                                    txtCNH.ForeColor = System.Drawing.Color.White;
                                    ShowToastrError("CNH do motorista, sem data de validade!");
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
                                ShowToastrWarning("Liberação de Risco do Motorista, vence em menos de 30 dias!");
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
                                ShowToastrError("Liberação de Risco do Motorista, está vencida!");
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
                                    ShowToastrError("Motorista, sem liberação de risco cadastrada no sistema!");
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

                                ShowToastrWarningVeiculo("Laudo de Opacidade do Veículo, vence em menos de 30 dias!");
                               // txtCodFrota.Focus();
                            }
                            if (diferencaOpacidade.TotalDays <= 0)
                            {
                                txtOpacidade.BackColor = System.Drawing.Color.Red;
                                txtOpacidade.ForeColor = System.Drawing.Color.White;
                                ShowToastrErrorVeiculo("Laudo de Opacidade do Veículo, está vencido!");
                                txtCodVeiculo.Text = "";
                                txtCodVeiculo.Focus();
                            }
                            else
                            {
                                if (txtOpacidade.Text == "")
                                {
                                    txtOpacidade.BackColor = System.Drawing.Color.Red;
                                    txtOpacidade.ForeColor = System.Drawing.Color.White;
                                    ShowToastrErrorVeiculo("Laudo de Opacidade do Veículo, não encontrado no sistema!");
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

                                ShowToastrWarningVeiculo("Licenciamento do Veículo, vence em menos de 30 dias!");
                                // txtCodFrota.Focus();
                            }
                            if (diferencaPlaca.TotalDays <= 0)
                            {
                                txtCRLVVeiculo.BackColor = System.Drawing.Color.Red;
                                txtCRLVVeiculo.ForeColor = System.Drawing.Color.White;
                                ShowToastrErrorVeiculo("Licenciamento do veículo, está vencido!");
                                txtCodVeiculo.Text = "";
                                txtCodVeiculo.Focus();
                            }
                            else
                            {
                                if (txtCRLVVeiculo.Text == "")
                                {
                                    txtCRLVVeiculo.BackColor = System.Drawing.Color.Red;
                                    txtCRLVVeiculo.ForeColor = System.Drawing.Color.White;
                                    ShowToastrErrorVeiculo("Veículo sem licenciamento lançado!");
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

                                    ShowToastrWarningVeiculo("Autorização CET do veículo, vence em menos de 30 dias!");
                                    // txtCodFrota.Focus();
                                }
                                if (diferencaCET.TotalDays <= 0)
                                {
                                    txtCET.BackColor = System.Drawing.Color.Red;
                                    txtCET.ForeColor = System.Drawing.Color.White;
                                    ShowToastrErrorVeiculo("Autorização CET do veículo, está vencida. Pare o veículo imediatamente!");
                                    txtCodVeiculo.Text = "";
                                    txtCodVeiculo.Focus();
                                }
                                else
                                {
                                    if (txtCET.Text == "")
                                    {
                                        txtCET.BackColor = System.Drawing.Color.Red;
                                        txtCET.ForeColor = System.Drawing.Color.White;
                                        ShowToastrErrorVeiculo("Laudo de Opacidade do Veículo, não encontrado no sistema!");
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

                                    ShowToastrWarningVeiculo("Cronotacografo do veículo, vence em menos de 30 dias!");
                                    // txtCodFrota.Focus();
                                }
                                if (diferencaCrono.TotalDays <= 0)
                                {
                                    txtCET.BackColor = System.Drawing.Color.Red;
                                    txtCET.ForeColor = System.Drawing.Color.White;
                                    ShowToastrErrorVeiculo("Cronotacografo do veículo, está vencido. Pare o veículo imediatamente!");
                                    txtCodVeiculo.Text = "";
                                    txtCodVeiculo.Focus();
                                }
                                else
                                {
                                    if (txtCrono.Text == "")
                                    {
                                        txtCrono.BackColor = System.Drawing.Color.Red;
                                        txtCrono.ForeColor = System.Drawing.Color.White;
                                        ShowToastrErrorVeiculo("Cronotacografo do Veículo, não encontrado no sistema!");
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

                                            ShowToastrWarningVeiculo("Licenciamento da carreta, vence em menos de 30 dias!");
                                            // txtCodFrota.Focus();
                                        }
                                        if (diferencaReb1.TotalDays <= 0)
                                        {
                                            txtCRLVReb1.BackColor = System.Drawing.Color.Red;
                                            txtCRLVReb1.ForeColor = System.Drawing.Color.White;
                                            ShowToastrErrorVeiculo("Licenciamento da carreta, está vencido!");
                                            txtCodVeiculo.Text = "";
                                            txtCodVeiculo.Focus();
                                        }
                                        else
                                        {
                                            if (txtCodVeiculo.Text == "")
                                            {
                                                txtCRLVReb1.BackColor = System.Drawing.Color.Red;
                                                txtCRLVReb1.ForeColor = System.Drawing.Color.White;
                                                ShowToastrErrorVeiculo("Carreta sem licenciamento lançado!");
                                                txtCodVeiculo.Text = "";
                                                txtCodVeiculo.Focus();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        txtCRLVReb1.BackColor = System.Drawing.Color.Red;
                                        txtCRLVReb1.ForeColor = System.Drawing.Color.White;
                                        ShowToastrErrorVeiculo("Carreta sem licenciamento lançado!");
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

                                        ShowToastrWarningVeiculo("Licenciamento da Carreta, vence em menos de 30 dias!");
                                        // txtCodFrota.Focus();
                                    }
                                    if (diferencaReboque2.TotalDays <= 0)
                                    {
                                        txtCRLVReb2.BackColor = System.Drawing.Color.Red;
                                        txtCRLVReb2.ForeColor = System.Drawing.Color.White;
                                        ShowToastrErrorVeiculo("Licenciamento da carreta, está vencido!");
                                        txtCodVeiculo.Text = "";
                                        txtCodVeiculo.Focus();
                                    }
                                    else
                                    {
                                        if (txtCRLVReb2.Text == "")
                                        {
                                            txtCRLVReb2.BackColor = System.Drawing.Color.Red;
                                            txtCRLVReb2.ForeColor = System.Drawing.Color.White;
                                            ShowToastrErrorVeiculo("Carreta sem licenciamento lançado!");
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

                                            ShowToastrWarningVeiculo("Licenciamento da carreta, vence em menos de 30 dias!");
                                            // txtCodFrota.Focus();
                                        }
                                        if (diferencaReb1.TotalDays <= 0)
                                        {
                                            txtCRLVReb1.BackColor = System.Drawing.Color.Red;
                                            txtCRLVReb1.ForeColor = System.Drawing.Color.White;
                                            ShowToastrErrorVeiculo("Licenciamento da carreta, está vencido!");
                                            txtCodVeiculo.Text = "";
                                            txtCodVeiculo.Focus();
                                        }
                                        else
                                        {
                                            if (txtCodVeiculo.Text == "")
                                            {
                                                txtCRLVReb1.BackColor = System.Drawing.Color.Red;
                                                txtCRLVReb1.ForeColor = System.Drawing.Color.White;
                                                ShowToastrErrorVeiculo("Carreta sem licenciamento lançado!");
                                                txtCodVeiculo.Text = "";
                                                txtCodVeiculo.Focus();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        txtCRLVReb1.BackColor = System.Drawing.Color.Red;
                                        txtCRLVReb1.ForeColor = System.Drawing.Color.White;
                                        ShowToastrErrorVeiculo("Carreta sem licenciamento lançado!");
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

                                            ShowToastrWarningVeiculo("Licenciamento da carreta, vence em menos de 30 dias!");
                                            // txtCodFrota.Focus();
                                        }
                                        if (diferencaReb1.TotalDays <= 0)
                                        {
                                            txtCRLVReb1.BackColor = System.Drawing.Color.Red;
                                            txtCRLVReb1.ForeColor = System.Drawing.Color.White;
                                            ShowToastrErrorVeiculo("Licenciamento da carreta, está vencido!");
                                            txtCodVeiculo.Text = "";
                                            txtCodVeiculo.Focus();
                                        }
                                        else
                                        {
                                            if (txtCodVeiculo.Text == "")
                                            {
                                                txtCRLVReb1.BackColor = System.Drawing.Color.Red;
                                                txtCRLVReb1.ForeColor = System.Drawing.Color.White;
                                                ShowToastrErrorVeiculo("Carreta sem licenciamento lançado!");
                                                txtCodVeiculo.Text = "";
                                                txtCodVeiculo.Focus();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        txtCRLVReb1.BackColor = System.Drawing.Color.Red;
                                        txtCRLVReb1.ForeColor = System.Drawing.Color.White;
                                        ShowToastrErrorVeiculo("Carreta sem licenciamento lançado!");
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

                                        ShowToastrWarningVeiculo("Licenciamento da Carreta, vence em menos de 30 dias!");
                                        // txtCodFrota.Focus();
                                    }
                                    if (diferencaReboque2.TotalDays <= 0)
                                    {
                                        txtCRLVReb2.BackColor = System.Drawing.Color.Red;
                                        txtCRLVReb2.ForeColor = System.Drawing.Color.White;
                                        ShowToastrErrorVeiculo("Licenciamento da carreta, está vencido!");
                                        txtCodVeiculo.Text = "";
                                        txtCodVeiculo.Focus();
                                    }
                                    else
                                    {
                                        if (txtCRLVReb2.Text == "")
                                        {
                                            txtCRLVReb2.BackColor = System.Drawing.Color.Red;
                                            txtCRLVReb2.ForeColor = System.Drawing.Color.White;
                                            ShowToastrErrorVeiculo("Carreta sem licenciamento lançado!");
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

                                            ShowToastrWarningVeiculo("Licenciamento da carreta, vence em menos de 30 dias!");
                                            // txtCodFrota.Focus();
                                        }
                                        if (diferencaReb1.TotalDays <= 0)
                                        {
                                            txtCRLVReb1.BackColor = System.Drawing.Color.Red;
                                            txtCRLVReb1.ForeColor = System.Drawing.Color.White;
                                            ShowToastrErrorVeiculo("Licenciamento da carreta, está vencido!");
                                            txtCodVeiculo.Text = "";
                                            txtCodVeiculo.Focus();
                                        }
                                        else
                                        {
                                            if (txtCodVeiculo.Text == "")
                                            {
                                                txtCRLVReb1.BackColor = System.Drawing.Color.Red;
                                                txtCRLVReb1.ForeColor = System.Drawing.Color.White;
                                                ShowToastrErrorVeiculo("Carreta sem licenciamento lançado!");
                                                txtCodVeiculo.Text = "";
                                                txtCodVeiculo.Focus();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        txtCRLVReb1.BackColor = System.Drawing.Color.Red;
                                        txtCRLVReb1.ForeColor = System.Drawing.Color.White;
                                        ShowToastrErrorVeiculo("Carreta sem licenciamento lançado!");
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

                                            ShowToastrWarningVeiculo("Licenciamento da carreta, vence em menos de 30 dias!");
                                            // txtCodFrota.Focus();
                                        }
                                        if (diferencaReb1.TotalDays <= 0)
                                        {
                                            txtCRLVReb1.BackColor = System.Drawing.Color.Red;
                                            txtCRLVReb1.ForeColor = System.Drawing.Color.White;
                                            ShowToastrErrorVeiculo("Licenciamento da carreta, está vencido!");
                                            txtCodVeiculo.Text = "";
                                            txtCodVeiculo.Focus();
                                        }
                                        else
                                        {
                                            if (txtCodVeiculo.Text == "")
                                            {
                                                txtCRLVReb1.BackColor = System.Drawing.Color.Red;
                                                txtCRLVReb1.ForeColor = System.Drawing.Color.White;
                                                ShowToastrErrorVeiculo("Carreta sem licenciamento lançado!");
                                                txtCodVeiculo.Text = "";
                                                txtCodVeiculo.Focus();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        txtCRLVReb1.BackColor = System.Drawing.Color.Red;
                                        txtCRLVReb1.ForeColor = System.Drawing.Color.White;
                                        ShowToastrErrorVeiculo("Carreta sem licenciamento lançado!");
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
                                            // Acione o toast quando a página for carregada
                                            //string script = "<script>showToast('Validade da CNH do motorista, vence em menos de 30 dias!');</script>";
                                            //ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);

                                            ShowToastrWarningVeiculo("Licenciamento da carreta, vence em menos de 30 dias!");
                                            // txtCodFrota.Focus();
                                        }
                                        if (diferencaReb1.TotalDays <= 0)
                                        {
                                            txtCRLVReb1.BackColor = System.Drawing.Color.Red;
                                            txtCRLVReb1.ForeColor = System.Drawing.Color.White;
                                            ShowToastrErrorVeiculo("Licenciamento da carreta, está vencido!");
                                            txtCodVeiculo.Text = "";
                                            txtCodVeiculo.Focus();
                                        }
                                        else
                                        {
                                            if (txtCodVeiculo.Text == "")
                                            {
                                                txtCRLVReb1.BackColor = System.Drawing.Color.Red;
                                                txtCRLVReb1.ForeColor = System.Drawing.Color.White;
                                                ShowToastrErrorVeiculo("Carreta sem licenciamento lançado!");
                                                txtCodVeiculo.Text = "";
                                                txtCodVeiculo.Focus();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        txtCRLVReb1.BackColor = System.Drawing.Color.Red;
                                        txtCRLVReb1.ForeColor = System.Drawing.Color.White;
                                        ShowToastrErrorVeiculo("Carreta sem licenciamento lançado!");
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
                            fotoMotorista = "/img/totalFunc.png";
                        }
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
        private void CarregarGridPedidos(string numeroCarga)
        {
            if (txtCarga.Text.Trim() != "")
            {
                // string numeroCarga = txtCarga.Text.Trim();


                string connStr = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    string query = "SELECT * FROM tbpedidos WHERE carga = @NumeroCarga";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@NumeroCarga", numeroCarga);

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);

                            if (dt.Rows.Count > 0)
                            {
                                gvPedidos.DataSource = dt;
                                gvPedidos.DataBind();
                            }
                            else
                            {
                                gvPedidos.DataSource = null;
                                gvPedidos.DataBind();
                                return;
                            }
                        }
                    }
                }
            }

        }
        protected void txtCarga_TextChanged(object sender, EventArgs e)
        {
            if (txtCarga.Text.Trim() != "")
            {
                string numeroCarga = txtCarga.Text.Trim();
                string sql = "SELECT * from tbcargas where carga = " + numeroCarga + " AND empresa = '1111'";
                SqlDataAdapter adpt = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                conn.Open();
                adpt.Fill(dt);
                conn.Close();

                if (dt.Rows.Count > 0)
                {
                    // Preenchendo os TextBoxes com valores do DataTable                    
                    txtCadastro.Text = dataHoraAtual.ToString("dd/MM/yyyy HH:mm");
                    cboStatus.SelectedItem.Text = "Ag. Carreg.";
                    txtSituacao.BackColor = System.Drawing.Color.Purple;
                    txtSituacao.ForeColor = System.Drawing.Color.White;
                    txtSituacao.Text = dt.Rows[0][40].ToString();
                    txtGR.Text = dt.Rows[0][28].ToString();
                    txtSolicitante.Text = dt.Rows[0][30].ToString();
                    cboMaterial.SelectedItem.Text = dt.Rows[0][8].ToString();
                    //cboMaterial.Items.Insert(0, new ListItem(dt.Rows[0][8].ToString(), ""));
                    txtPesoCarga.Text = dt.Rows[0][7].ToString();
                    
                    txtCodRemetente.Text = dt.Rows[0][12].ToString();
                    cboRemetente.Text = dt.Rows[0][13].ToString();
                    txtMunicipioRemetente.Text = dt.Rows[0][23].ToString();
                    txtUFRemetente.Text = dt.Rows[0][19].ToString();
                    
                    txtCodDestinatario.Text = dt.Rows[0][14].ToString();
                    cboDestinatario.Text = dt.Rows[0][15].ToString();
                    txtMunicipioDestinatario.Text = dt.Rows[0][24].ToString();
                    txtUFDestinatario.Text = dt.Rows[0][20].ToString();
                    
                    txtCodPagador.Text = dt.Rows[0][76].ToString();
                    txtPagador.Text = dt.Rows[0][77].ToString();
                    txtCidPagador.Text = dt.Rows[0][78].ToString();
                    txtUFPagador.Text = dt.Rows[0][79].ToString();

                    txtPedagio.Text = dt.Rows[0][63].ToString();
                    string sDuracao = dt.Rows[0][81].ToString();
                    txtPercurso.Text = dt.Rows[0][84].ToString();
                    // Carregar pedidos
                    CarregarGridPedidos(numeroCarga);

                }
                else
                {
                    ShowToastrInfo("Carga não cadastrada, ou pertence a outra filial!");
                    txtCarga.Text = "";
                    txtCarga.Focus();
                }

            }
        }
        private void PreencherComboMateriais()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT id, descricao FROM tbtipomaterial";

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
                    cboMaterial.DataSource = reader;
                    cboMaterial.DataTextField = "descricao";  // Campo que será mostrado no ComboBox
                    cboMaterial.DataValueField = "id";  // Campo que será o valor de cada item                    
                    cboMaterial.DataBind();  // Realiza o binding dos dados                   
                    //cboMaterial.Items.Insert(0, new ListItem("Selecione...", "0"));
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
        private void PreencherComboStatus()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT descricao FROM tbtipostatus";

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
                    cboStatus.DataSource = reader;
                    cboStatus.DataTextField = "descricao";  // Campo que será mostrado no ComboBox
                    cboStatus.DataValueField = "descricao";  // Campo que será o valor de cada item                    
                    cboStatus.DataBind();  // Realiza o binding dos dados                   
                    //cboMaterial.Items.Insert(0, new ListItem("Selecione...", "0"));
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
        protected void gvPedidos_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            //int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value);
            //GridViewRow row = GridView1.Rows[e.RowIndex];

            //TextBox txtNome = (TextBox)row.FindControl("txtNome");
            //TextBox txtPreco = (TextBox)row.FindControl("txtPreco");

            //string nome = txtNome.Text.Trim();
            //decimal preco = Convert.ToDecimal(txtPreco.Text);

            //using (SqlConnection conn = new SqlConnection(conexao))
            //{
            //    string query = "UPDATE Produtos SET Nome = @Nome, Preco = @Preco WHERE ID = @ID";
            //    SqlCommand cmd = new SqlCommand(query, conn);
            //    cmd.Parameters.AddWithValue("@Nome", nome);
            //    cmd.Parameters.AddWithValue("@Preco", preco);
            //    cmd.Parameters.AddWithValue("@ID", id);

            //    conn.Open();
            //    cmd.ExecuteNonQuery();
            //    conn.Close();
            //}

            //GridView1.EditIndex = -1;
            //CarregarGrid();
        }
        private void ShowToastrSuccess(string message)
        {
            string script = $"showSuccessToast('{message}');";
            ClientScript.RegisterStartupScript(this.GetType(), "toastrSuccess", script, true);
        }
        private void ShowToastrError(string message)
        {
            string script = $"showErrorToast('{message}');";
            ClientScript.RegisterStartupScript(this.GetType(), "toastrError", script, true);

        }
        private void ShowToastrInfo(string message)
        {
            string script = $"showInfoToast('{message}');";
            ClientScript.RegisterStartupScript(this.GetType(), "toastrInfo", script, true);
        }
        private void ShowToastrWarning(string message)
        {
            string script = $"showWarningToast('{message}');";
            ClientScript.RegisterStartupScript(this.GetType(), "toastrWarning", script, true);
        }
        private void ShowToastrSuccessVeiculo(string message)
        {
            string script = $"showSuccessToast('{message}');";
            ClientScript.RegisterStartupScript(this.GetType(), "toastrSuccess", script, true);
        }
        private void ShowToastrErrorVeiculo(string message)
        {
            string script = $"showErrorToast('{message}');";
            ClientScript.RegisterStartupScript(this.GetType(), "toastrError", script, true);

        }
        private void ShowToastrInfoVeiculo(string message)
        {
            string script = $"showInfoToast('{message}');";
            ClientScript.RegisterStartupScript(this.GetType(), "toastrInfo", script, true);
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
                        txtCapCarga.Text = ConsultaVeiculo.cap;
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

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
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
                    // Opcional: limpar ou fechar modal
                    conn.Close();
                    ClientScript.RegisterStartupScript(this.GetType(), "HideModal", "hideModal();", true);
                }
                catch (Exception ex)
                {
                    // Logar ou exibir erro
                    Response.Write("<script>alert('Erro: " + ex.Message + "');</script>");
                }
            }
        }        

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            // Primeiro, converter a string para DateTime
            DateTime converterDataCadastro = DateTime.ParseExact(
                txtCadastro.Text,
                "dd/MM/yyyy HH:mm",
                CultureInfo.InvariantCulture
            );

            // Depois, converter o DateTime para string no formato ISO (yyyy-MM-dd HH:mm)
            string dataFormatada = converterDataCadastro.ToString("yyyy-MM-dd HH:mm");


            string sqlSalvarColeta = "INSERT INTO tbcarregamentos(funcao, valtoxicologico, venccnh, valgr, foneparticular, cpf, cartaopedagio, valcartao, codtranspmotorista, nomtranspmotorista, veiculotipo, tipoveiculo, carreta, tipocarreta, valopacidade, valcet, valcrlvveiculo, valcrlvreboque1, valcrlvreboque2, venccronotacografo, codtra, transportadora, tecnologia, rastreamento, emissao, num_carregamento, status, situacao, controle_ferrolene, solicitante, gr, material, peso, cargaliq, rede, catraca, dtottu, codcliorigem, nomcliorigem, cidorigem, uforigem, codclidestino, nomclidestino, ciddestino, ufdestino, cod_pagador, nom_pagador, cid_pagador, uf_pagador, emite_pedagio, percurso, duracao_viagem, pedagio, solicitantepedagio, pagadorpedagioida,pagadorpedagiovolta,historicopedagio) VALUES(@funcao, @valtoxicologico,@venccnh,@valgr,@foneparticular,@cpf, @cartaopedagio, @valcartao, @codtranspmotorista, @nomtranspmotorista, @veiculotipo, @tipoveiculo, @carreta, @tipocarreta, @valopacidade, @valcet, @valcrlvveiculo, @valcrlvreboque1, @valcrlvreboque2, @venccronotacografo, @codtra, @transportadora, @tecnologia, @rastreamento, @emissao, @num_carregamento, @status, @situacao, @controle_ferrolene, @solicitante, @gr, @material, @peso, @cargaliq, @rede, @catraca, @dtottu, @codcliorigem, @nomcliorigem, @cidorigem, @uforigem, @codclidestino, @nomclidestino, @ciddestino, @ufdestino, @cod_pagador, @nom_pagador, @cid_pagador, @uf_pagador, @emite_pedagio, @percurso, @duracao_viagem, @pedagio, @solicitantepedagio, @pagadorpedagioida, @pagadorpedagiovolta, @historicopedagio)";

            SqlCommand comando = new SqlCommand(sqlSalvarColeta, conn);
            comando.Parameters.AddWithValue("@codmotorista", txtCodMotorista.Text.ToUpper());
            comando.Parameters.AddWithValue("@nomemotorista", ddlMotorista.SelectedItem.Text.ToUpper());
            comando.Parameters.AddWithValue("@codcontato", txtCodFrota.Text.ToUpper());
            comando.Parameters.AddWithValue("@fonecorporativo", txtFoneCorp.Text.ToUpper());
            comando.Parameters.AddWithValue("@veiculo", txtCodVeiculo.Text);
            comando.Parameters.AddWithValue("@placa", txtPlaca.Text.ToUpper());
            comando.Parameters.AddWithValue("@reboque1", txtReboque1.Text.Trim().ToUpper());
            comando.Parameters.AddWithValue("@reboque2", txtReboque2.Text.ToUpper());
            comando.Parameters.AddWithValue("@nucleo", txtFilialMot.Text.ToUpper());
            comando.Parameters.AddWithValue("@tipomot", txtTipoMot.Text.ToUpper());
            comando.Parameters.AddWithValue("@funcao", txtFuncao.Text.ToUpper());
            comando.Parameters.AddWithValue("@valtoxicologico", SafedDateValue(txtExameToxic.Text));
            comando.Parameters.AddWithValue("@venccnh", SafedDateValue(txtCNH.Text));
            comando.Parameters.AddWithValue("@valgr", SafedDateValue(txtLibGR.Text));
            comando.Parameters.AddWithValue("@foneparticular", txtCelular.Text);
            comando.Parameters.AddWithValue("@cpf", txtCPF.Text);
            comando.Parameters.AddWithValue("@cartaopedagio", txtCartao.Text.ToUpper());
            comando.Parameters.AddWithValue("@valcartao", txtValCartao.Text.ToUpper());
            comando.Parameters.AddWithValue("@codtranspmotorista", txtCodTransportadora.Text);
            comando.Parameters.AddWithValue("@nomtranspmotorista", txtTransportadora.Text.ToUpper());
            comando.Parameters.AddWithValue("@veiculotipo", txtVeiculoTipo.Text.ToUpper());
            comando.Parameters.AddWithValue("@tipoveiculo", txtTipoVeiculo.Text);
            comando.Parameters.AddWithValue("@carreta", txtCarreta.Text.ToUpper());
            comando.Parameters.AddWithValue("@tipocarreta", txtConjunto.Text.ToUpper());
            comando.Parameters.AddWithValue("@valopacidade", SafedDateValue(txtOpacidade.Text));
            comando.Parameters.AddWithValue("@valcet", SafedDateValue(txtCET.Text));
            comando.Parameters.AddWithValue("@valcrlvveiculo", SafedDateValue(txtCRLVVeiculo.Text));
            comando.Parameters.AddWithValue("@valcrlvreboque1", SafedDateValue(txtCRLVReb1.Text));
            comando.Parameters.AddWithValue("@valcrlvreboque2", SafedDateValue(txtCRLVReb2.Text));
            comando.Parameters.AddWithValue("@venccronotacografo", SafedDateValue(txtCrono.Text));
            comando.Parameters.AddWithValue("@codtra", txtCodProprietario.Text.ToUpper());
            comando.Parameters.AddWithValue("@transportadora", txtProprietario.Text.ToUpper());
            comando.Parameters.AddWithValue("@tecnologia", txtTecnologia.Text.ToUpper());
            comando.Parameters.AddWithValue("@rastreamento", txtRastreamento.Text.ToUpper());
            comando.Parameters.AddWithValue("@emissao", converterDataCadastro);
            comando.Parameters.AddWithValue("@num_carregamento", txtCarga.Text.ToUpper());
            comando.Parameters.AddWithValue("@status", cboStatus.SelectedItem.Text.ToUpper());
            comando.Parameters.AddWithValue("@situacao", txtSituacao.Text.ToUpper()); // EM ANDAMENTO
            comando.Parameters.AddWithValue("@controle_ferrolene", txtContFerrolene.Text.ToUpper());
            comando.Parameters.AddWithValue("@solicitante", txtSolicitante.Text.ToUpper());
            comando.Parameters.AddWithValue("@gr", txtGR.Text.ToUpper());
            comando.Parameters.AddWithValue("@material", cboMaterial.SelectedItem.Text.ToUpper());
            comando.Parameters.AddWithValue("@peso", txtPesoCarga.Text.ToUpper()); // A VIRGULA POR PONTO
            comando.Parameters.AddWithValue("@cargaliq", txtCapCarga.Text.ToUpper());
            comando.Parameters.AddWithValue("@rede", txtCintas.Text.ToUpper());
            comando.Parameters.AddWithValue("@catraca", txtCatracas.Text.ToUpper());
            comando.Parameters.AddWithValue("@dtottu", txtControleCli.Text.ToUpper());
            comando.Parameters.AddWithValue("@codcliorigem", txtCodRemetente.Text.ToUpper());
            comando.Parameters.AddWithValue("@nomcliorigem", cboRemetente.Text.ToUpper());
            comando.Parameters.AddWithValue("@cidorigem", txtMunicipioRemetente.Text.ToUpper());
            comando.Parameters.AddWithValue("@uforigem", txtUFRemetente.Text.ToUpper());
            comando.Parameters.AddWithValue("@codclidestino", txtCodDestinatario.Text.ToUpper());
            comando.Parameters.AddWithValue("@nomclidestino", cboDestinatario.Text.ToUpper());
            comando.Parameters.AddWithValue("@ciddestino", txtMunicipioDestinatario.Text.ToUpper());
            comando.Parameters.AddWithValue("@ufdestino", txtUFDestinatario.Text.ToUpper());
            comando.Parameters.AddWithValue("@cod_pagador", txtCodPagador.Text.ToUpper());
            comando.Parameters.AddWithValue("@nom_pagador", txtPagador.Text.ToUpper());
            comando.Parameters.AddWithValue("@cid_pagador", txtCidPagador.Text.ToUpper());
            comando.Parameters.AddWithValue("@uf_pagador", txtUFPagador.Text.ToUpper());
            comando.Parameters.AddWithValue("@emite_pedagio", txtPedagio.Text.ToUpper());
            comando.Parameters.AddWithValue("@percurso", txtPercurso.Text);
            comando.Parameters.AddWithValue("@duracao_viagem", SafeHourValue(sDuracao));


            if (txtPedagio.Text == "SIM")
            {
                if (txtCodPagador.Text == "6164" || txtCodPagador.Text == "6580" || txtCodPagador.Text == "7479" || txtCodPagador.Text == "6024" || txtCodPagador.Text == "5577" || txtCodPagador.Text == "5566" || txtCodPagador.Text == "9984" || txtCodPagador.Text == "6289")
                {
                    comando.Parameters.AddWithValue("@pedagio", "NAO");                    
                    comando.Parameters.AddWithValue("@solicitantepedagio", DBNull.Value);
                    comando.Parameters.AddWithValue("@pagadorpedagioida", DBNull.Value);
                    comando.Parameters.AddWithValue("@pagadorpedagiovolta", DBNull.Value);                    
                    comando.Parameters.AddWithValue("@historicopedagio", "Pedágio por conta do pagador do frete...");
                }
                else
                { 
                    comando.Parameters.AddWithValue("@pedagio", "SIM");
                    comando.Parameters.AddWithValue("@historicopedagio", "Pedido enviado automaticamente.");
                    comando.Parameters.AddWithValue("@solicitantepedagio", txtUsuCadastro.Text.ToUpper());
                    if (txtCodPagador.Text == "1000")
                    {
                        comando.Parameters.AddWithValue("@pagadorpedagioida", "FERROLENE");
                    }

                    if (txtCodPagador.Text == "1020" || txtCodPagador.Text == "1236" || txtCodPagador.Text == "2763")
                    {
                        comando.Parameters.AddWithValue("@pagadorpedagioida", "VOLKSWAGEN");
                    }    
                    
                    if (txtCodPagador.Text != "1020" || txtCodPagador.Text != "1236" || txtCodPagador.Text != "2763" || txtCodPagador.Text != "2763" || txtCodPagador.Text != "1000") 
                    {
                        comando.Parameters.AddWithValue("@pagadorpedagioida", "TRANSNOVAG");
                    }

                    if (txtVeiculoTipo.Text == "AGREGADO" || txtVeiculoTipo.Text == "FROTA")
                    {
                        comando.Parameters.AddWithValue("@pagadorpedagiovolta", "TRANSNOVAG");
                        comando.Parameters.AddWithValue("@pagadorpedagioida", "SIM");
                    }
                    else
                    {
                        comando.Parameters.AddWithValue("@pagadorpedagiovolta", DBNull.Value);
                        comando.Parameters.AddWithValue("@historicopedagio", "Pedágio emitido pelo pagador do frete.");
                    }

                }


            }
            else
            {
                comando.Parameters.AddWithValue("@pedagio", "NAO");
                comando.Parameters.AddWithValue("@historicopedagio", DBNull.Value);
                comando.Parameters.AddWithValue("@solicitantepedagio", DBNull.Value);                
                comando.Parameters.AddWithValue("@pagadorpedagiovolta", DBNull.Value);
                comando.Parameters.AddWithValue("@pagadorpedagioida", "NAO");
            }


                //if (txtAltura.Text != "")
                //{
                //    string entradaAltura = txtAltura.Text.Trim();
                //    // Substitui vírgula por ponto
                //    string formatado = entradaAltura.Replace(',', '.');
                //    if (decimal.TryParse(formatado, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal numero))
                //    {
                //        comando.Parameters.AddWithValue("@altura", numero.ToString(CultureInfo.InvariantCulture));
                //    }
                //}
               
                //try
                //{
                conn.Open();
                comando.ExecuteNonQuery();
                conn.Close();
                ShowToastrInfo("Coleta cadastrada com sucesso!");
                Thread.Sleep(5000);
                //Chama a página de controle de carretas
                Response.Redirect("/dist/pages/ControleCarretas.aspx");

                //}
                //catch (Exception ex)
                //{
                //    var message = new JavaScriptSerializer().Serialize(ex.Message.ToString());
                //    ShowToastrError("Erro, coleta não cadastrada. Verifique informações digitadas.");
                //    Thread.Sleep(5000);
                //    //Chama a página de controle de carretas
                //    //Response.Redirect("/dist/pages/ControleCarretas.aspx");
                //}

                //finally
                //{
                //    conn.Close();
                //}
        }
        private object SafeHourValue(string input)
        {
            DateTime dt;
            if (DateTime.TryParse(input, out dt))
                return dt.ToString("HH:mm:ss");
            else
                return DBNull.Value;
        }
        private object SafedDateHourValue(string input)
        {
            DateTime dt;
            if (DateTime.TryParse(input, out dt))
                return dt.ToString("yyyy-MM-dd HH:mm");
            else
                return DBNull.Value;
        }
        private object SafedDateValue(string input)
        {
            DateTime dt;
            if (DateTime.TryParse(input, out dt))
                return dt.ToString("yyyy-MM-dd");
            else
                return DBNull.Value;
        }
        private void ShowToastrWarningVeiculo(string message)
        {
            string script = $"showWarningToast('{message}');";
            ClientScript.RegisterStartupScript(this.GetType(), "toastrWarning", script, true);
        }



        //protected void btnSalvar_Click(object sender, EventArgs e)
        //{
        //    int idCliente = int.Parse(hfIdCliente.Value); // hidden field com o ID
        //    string usuario = Session["Usuario"]?.ToString() ?? "Desconhecido";

        //    // 1️⃣ Buscar dados antigos no banco
        //    var dadosAntigos = ObterDadosAntigos(idCliente);

        //    // 2️⃣ Novos dados do formulário
        //    var dadosNovos = new Dictionary<string, string>
        //{
        //    { "Nome", txtNome.Text },
        //    { "Email", txtEmail.Text },
        //    { "Telefone", txtTelefone.Text }
        //};

        //    // 3️⃣ Comparar e salvar histórico
        //    foreach (var campo in dadosNovos)
        //    {
        //        string valorAntigo = dadosAntigos.ContainsKey(campo.Key) ? dadosAntigos[campo.Key] : null;
        //        string valorNovo = campo.Value;

        //        if (valorAntigo != valorNovo)
        //            SalvarHistorico("Cliente", idCliente, campo.Key, valorAntigo, valorNovo, usuario);
        //    }

        //    // 4️⃣ Atualizar dados principais
        //    AtualizarCliente(idCliente, dadosNovos);
        //}

        //private Dictionary<string, string> ObterDadosAntigos(int idCliente)
        //{
        //    var resultado = new Dictionary<string, string>();

        //    using (SqlConnection conn = new SqlConnection(connectionString))
        //    {
        //        string sql = "SELECT Nome, Email, Telefone FROM Cliente WHERE Id = @Id";
        //        SqlCommand cmd = new SqlCommand(sql, conn);
        //        cmd.Parameters.AddWithValue("@Id", idCliente);

        //        conn.Open();
        //        using (SqlDataReader dr = cmd.ExecuteReader())
        //        {
        //            if (dr.Read())
        //            {
        //                resultado["Nome"] = dr["Nome"].ToString();
        //                resultado["Email"] = dr["Email"].ToString();
        //                resultado["Telefone"] = dr["Telefone"].ToString();
        //            }
        //        }
        //    }
        //    return resultado;
        //}

        //private void SalvarHistorico(string tabela, int chave, string campo, string valorAntigo, string valorNovo, string usuario)
        //{
        //    using (SqlConnection conn = new SqlConnection(connectionString))
        //    {
        //        string sql = @"
        //        INSERT INTO HistoricoAlteracoes (Tabela, ChaveRegistro, Campo, ValorAntigo, ValorNovo, Usuario)
        //        VALUES (@Tabela, @ChaveRegistro, @Campo, @ValorAntigo, @ValorNovo, @Usuario)";

        //        SqlCommand cmd = new SqlCommand(sql, conn);
        //        cmd.Parameters.AddWithValue("@Tabela", tabela);
        //        cmd.Parameters.AddWithValue("@ChaveRegistro", chave);
        //        cmd.Parameters.AddWithValue("@Campo", campo);
        //        cmd.Parameters.AddWithValue("@ValorAntigo", (object)valorAntigo ?? DBNull.Value);
        //        cmd.Parameters.AddWithValue("@ValorNovo", (object)valorNovo ?? DBNull.Value);
        //        cmd.Parameters.AddWithValue("@Usuario", usuario);

        //        conn.Open();
        //        cmd.ExecuteNonQuery();
        //    }
        //}

        //private void AtualizarCliente(int idCliente, Dictionary<string, string> dadosNovos)
        //{
        //    using (SqlConnection conn = new SqlConnection(connectionString))
        //    {
        //        string sql = @"UPDATE Cliente SET Nome = @Nome, Email = @Email, Telefone = @Telefone WHERE Id = @Id";
        //        SqlCommand cmd = new SqlCommand(sql, conn);
        //        cmd.Parameters.AddWithValue("@Nome", dadosNovos["Nome"]);
        //        cmd.Parameters.AddWithValue("@Email", dadosNovos["Email"]);
        //        cmd.Parameters.AddWithValue("@Telefone", dadosNovos["Telefone"]);
        //        cmd.Parameters.AddWithValue("@Id", idCliente);

        //        conn.Open();
        //        cmd.ExecuteNonQuery();
        //    }
        //}
    }
}