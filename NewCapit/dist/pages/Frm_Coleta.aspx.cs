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

namespace NewCapit.dist.pages
{
    public partial class Frm_Coleta : System.Web.UI.Page
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["conexao"].ToString());
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
                    txtUsuCadastro.Text = nomeUsuario;
                }
                else
                {
                    var lblUsuario = "<Usuário>";
                    txtUsuCadastro.Text = lblUsuario;
                }
                DateTime dataHoraAtual = DateTime.Now;
                lblDtCadastro.Text = dataHoraAtual.ToString("dd/MM/yyyy HH:mm");
                txtCadastro.Text = dataHoraAtual.ToString("dd/MM/yyyy HH:mm");

                PreencherComboMotoristas();
                //CarregarGrid();
                PreencherNumColeta();
                fotoMotorista = "/img/totalFunc.png";
            }
            CarregaFoto();

        }
        protected void btnSelecionar_Click(object sender, EventArgs e)
        {
            List<string> selecionados = new List<string>();

            foreach (GridViewRow row in gvPedidos.Rows)
            {
                CheckBox chk = (CheckBox)row.FindControl("chkSelect");
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
        protected void txtCodMot_TextChanged(object sender, EventArgs e)
        {
            if (txtCodMot.Text.Trim() != "")
            {                
                var codigo = txtCodMot.Text.Trim();
                var obj = new Domain.ConsultaMotorista
                {
                    codmot = codigo
                };
                var ConsultaMotorista = DAL.UsersDAL.CheckMotorista(obj);
                if (ConsultaMotorista != null)
                {
                    if (ConsultaMotorista.status.Trim() == "INATIVO")
                    {
                        if (txtCodMot.Text.Trim() != "")
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
                        string script = "<script>showToast('Motorista excluido ou inativo!');</script>";
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                        txtCodMot.Text = "";
                        txtCodMot.Focus();
                    }
                    else
                    {
                        //txtFilialMot.Text = ConsultaMotorista.nucleo;
                        //txtTipoMot.Text = ConsultaMotorista.tipomot;
                        //txtExameToxic.Text = ConsultaMotorista.venceti;
                        //txtCNH.Text = ConsultaMotorista.venccnh.ToString();                        
                        cboMotorista.SelectedItem.Text = ConsultaMotorista.nommot;
                        txtCPF.Text = ConsultaMotorista.cpf;
                        txtCartaoPamcard.Text = ConsultaMotorista.cartaomot;
                       // txtValCartao.Text = ConsultaMotorista.venccartao;
                        txtFoneParticular.Text = ConsultaMotorista.fone2;
                       // txtCodTransportadora.Text = ConsultaMotorista.codtra;
                       // txtTransportadora.Text = ConsultaMotorista.transp;

                        if (ConsultaMotorista.tipomot.Trim() == "AGREGADO" || ConsultaMotorista.tipomot.Trim() == "TERCEIRO")
                        {
                            //txtCodVeiculo.Text = ConsultaMotorista.codvei;
                            //txtFilialVeicCNT.Text = ConsultaMotorista.nucleo;
                            //txtPlaca.Text = ConsultaMotorista.placa;
                            //txtVeiculoTipo.Text = ConsultaMotorista.tipomot;
                            //txtTipoVeiculo.Text = ConsultaMotorista.tipoveiculo;
                            //txtReboque1.Text = ConsultaMotorista.reboque1;
                            //txtReboque2.Text = ConsultaMotorista.reboque2;
                            //ETI.Visible = false;
                            //var codigoAgregado = txtCodVeiculo.Text.Trim();

                            var objVeiculo = new Domain.ConsultaVeiculo
                            {
                              //  codvei = codigoAgregado
                            };
                            var ConsultaVeiculo = DAL.UsersDAL.CheckVeiculo(objVeiculo);
                            if (ConsultaVeiculo != null)
                            {
                                //txtOpacidade.Text = ConsultaVeiculo.vencimentolaudofumaca;
                                //txtCRLVVeiculo.Text = ConsultaVeiculo.venclicenciamento;
                                //txtCET.Text = ConsultaVeiculo.venclicencacet;
                                //txtCarreta.Text = ConsultaVeiculo.tiporeboque;
                                //txtTecnologia.Text = ConsultaVeiculo.rastreador;
                                //txtRastreamento.Text = ConsultaVeiculo.rastreamento;
                                //txtConjunto.Text = ConsultaVeiculo.tipocarreta;
                                //txtCodProprietario.Text = ConsultaVeiculo.codtra;
                                //txtProprietario.Text = ConsultaVeiculo.transp;

                            }
                        }
                        else if (ConsultaMotorista.tipomot.Trim() == "FUNCIONÁRIO")
                        {
                            //txtCodVeiculo.Text = ConsultaMotorista.frota;
                            //txtFilialVeicCNT.Text = ConsultaMotorista.nucleo;
                            //txtPlaca.Text = ConsultaMotorista.placa;
                            //txtVeiculoTipo.Text = "FROTA";
                            //txtTipoVeiculo.Text = ConsultaMotorista.tipoveiculo;
                            //txtReboque1.Text = ConsultaMotorista.reboque1;
                            //txtReboque2.Text = ConsultaMotorista.reboque2;
                            //ETI.Visible = true;
                            //var codigoAgregado = txtCodVeiculo.Text.Trim();

                            var objVeiculo = new Domain.ConsultaVeiculo
                            {
                               // codvei = codigoAgregado
                            };
                            var ConsultaVeiculo = DAL.UsersDAL.CheckVeiculo(objVeiculo);
                            if (ConsultaVeiculo != null)
                            {
                                //txtOpacidade.Text = ConsultaVeiculo.vencimentolaudofumaca;
                                //txtCRLVVeiculo.Text = ConsultaVeiculo.venclicenciamento;
                                //txtCET.Text = ConsultaVeiculo.venclicencacet;
                                //txtCarreta.Text = ConsultaVeiculo.tiporeboque;
                                //txtTecnologia.Text = ConsultaVeiculo.rastreador;
                                //txtRastreamento.Text = ConsultaVeiculo.rastreamento;
                                //txtConjunto.Text = ConsultaVeiculo.tipocarreta;
                                //txtCodProprietario.Text = ConsultaVeiculo.codtra;
                                //txtProprietario.Text = ConsultaVeiculo.transp;

                            }
                            // pesquisar validade do Exame Toxicologico
                            //if (txtExameToxic.Text != "")
                            //{
                            //    DateTime dataHoje = Convert.ToDateTime(DateTime.Now.Date);
                            //    DateTime dataETI = Convert.ToDateTime(txtExameToxic.Text).Date; ;

                            //    TimeSpan diferencaETI = dataETI - dataHoje;
                            //    // Agora você pode comparar a diferença
                            //    if (diferencaETI.TotalDays < 30)
                            //    {
                            //        txtExameToxic.BackColor = System.Drawing.Color.Khaki;
                            //        txtExameToxic.ForeColor = System.Drawing.Color.OrangeRed;
                            //        string nomeUsuario = txtUsuCadastro.Text;
                            //        string linha1 = "Olá, " + nomeUsuario + "!";
                            //        string linha2 = "O Exame Toxicologico do motorista " + ddlMotorista.SelectedItem.Text.Trim() + ", vence em menos de 30 dias.";
                            //        string linha3 = "Data de vencimento: " + dataETI.ToString("dd/MM/yyyy") + ".";
                            //        //string linha4 = "Unidade: " + unidade + ". Por favor, verifique.";
                            //        // Concatenando as linhas com '\n' para criar a mensagem
                            //        string mensagem = $"{linha1}\n{linha2}\n{linha3}";
                            //        string mensagemCodificada = HttpUtility.JavaScriptStringEncode(mensagem);
                            //        //// Gerando o script JavaScript para exibir o alerta
                            //        string script = $"alert('{mensagemCodificada}');";
                            //        //// Registrando o script para execução no lado do cliente
                            //        ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", script, true);
                            //    }
                            //    else if (diferencaETI.TotalDays <= 0)
                            //    {
                            //        txtExameToxic.BackColor = System.Drawing.Color.Red;
                            //        txtExameToxic.ForeColor = System.Drawing.Color.White;
                            //        string nomeUsuario = txtUsuCadastro.Text;
                            //        string linha1 = "Olá, " + nomeUsuario + "!";
                            //        string linha2 = "O Exame Toxicologico do motorista " + ddlMotorista.SelectedItem.Text.Trim() + ", está vencido.";
                            //        string linha3 = "Data do vencimento: " + dataETI.ToString("dd/MM/yyyy") + ".";
                            //        //string linha4 = "Unidade: " + unidade + ". Por favor, verifique.";
                            //        // Concatenando as linhas com '\n' para criar a mensagem
                            //        string mensagem = $"{linha1}\n{linha2}\n{linha3}";
                            //        string mensagemCodificada = HttpUtility.JavaScriptStringEncode(mensagem);
                            //        //// Gerando o script JavaScript para exibir o alerta
                            //        string script = $"alert('{mensagemCodificada}');";
                            //        //// Registrando o script para execução no lado do cliente
                            //        ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", script, true);
                            //        txtCodMotorista.Text = "";
                            //        txtCodMotorista.Focus();

                            //    }
                            //    else
                            //    {
                            //        if (txtExameToxic.Text == "")
                            //        {
                            //            string nomeUsuario = txtUsuCadastro.Text;
                            //            string linha1 = "Olá, " + nomeUsuario + "!";
                            //            string linha2 = "O motorista " + ddlMotorista.SelectedItem.Text.Trim() + ", não tem Exame Toxicologico, lançado no sistema.";
                            //            string linha3 = "Verifique, por favor.";
                            //            //string linha4 = "Unidade: " + unidade + ". Por favor, verifique.";
                            //            // Concatenando as linhas com '\n' para criar a mensagem
                            //            string mensagem = $"{linha1}\n{linha2}\n{linha3}";
                            //            string mensagemCodificada = HttpUtility.JavaScriptStringEncode(mensagem);
                            //            //// Gerando o script JavaScript para exibir o alerta
                            //            string script = $"alert('{mensagemCodificada}');";
                            //            //// Registrando o script para execução no lado do cliente
                            //            ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", script, true);
                            //            txtCodMotorista.Text = "";
                            //            txtCodMotorista.Focus();

                            //        }
                            //        else
                            //        {
                            //            txtExameToxic.BackColor = System.Drawing.Color.LightGray;
                            //            txtExameToxic.ForeColor = System.Drawing.Color.Black;
                            //        }
                            //    }
                            //}
                        }
                        // Pesquisar validade da CNH
                        //if (txtCNH.Text != "")
                        //{
                        //    DateTime dataHoje = Convert.ToDateTime(DateTime.Now.Date);
                        //    DateTime dataCNH = Convert.ToDateTime(txtCNH.Text).Date; ;

                        //    TimeSpan diferenca = dataCNH - dataHoje;
                        //    // Agora você pode comparar a diferença
                        //    if (diferenca.TotalDays < 30)
                        //    {
                        //        txtCNH.BackColor = System.Drawing.Color.Khaki;
                        //        txtCNH.ForeColor = System.Drawing.Color.OrangeRed;
                        //        string nomeUsuario = txtUsuCadastro.Text;
                        //        string linha1 = "Olá, " + nomeUsuario + "!";
                        //        string linha2 = "A CNH do motorista " + ddlMotorista.SelectedItem.Text.Trim() + ", vence em menos de 30 dias.";
                        //        string linha3 = "Data de vencimento: " + dataCNH.ToString("dd/MM/yyyy") + ".";
                        //        //string linha4 = "Unidade: " + unidade + ". Por favor, verifique.";
                        //        // Concatenando as linhas com '\n' para criar a mensagem
                        //        string mensagem = $"{linha1}\n{linha2}\n{linha3}";
                        //        string mensagemCodificada = HttpUtility.JavaScriptStringEncode(mensagem);
                        //        //// Gerando o script JavaScript para exibir o alerta
                        //        string script = $"alert('{mensagemCodificada}');";
                        //        //// Registrando o script para execução no lado do cliente
                        //        ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", script, true);
                        //    }
                        //    else if (diferenca.TotalDays <= 0)
                        //    {
                        //        txtCNH.BackColor = System.Drawing.Color.Red;
                        //        txtCNH.ForeColor = System.Drawing.Color.White;
                        //        string nomeUsuario = txtUsuCadastro.Text;
                        //        string linha1 = "Olá, " + nomeUsuario + "!";
                        //        string linha2 = "A CNH do motorista " + ddlMotorista.SelectedItem.Text.Trim() + ", está vencida.";
                        //        string linha3 = "Data de vencimento: " + dataCNH.ToString("dd/MM/yyyy") + ".";
                        //        //string linha4 = "Unidade: " + unidade + ". Por favor, verifique.";
                        //        // Concatenando as linhas com '\n' para criar a mensagem
                        //        string mensagem = $"{linha1}\n{linha2}\n{linha3}";
                        //        string mensagemCodificada = HttpUtility.JavaScriptStringEncode(mensagem);
                        //        //// Gerando o script JavaScript para exibir o alerta
                        //        string script = $"alert('{mensagemCodificada}');";
                        //        //// Registrando o script para execução no lado do cliente
                        //        ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", script, true);
                        //        txtCodMotorista.Text = "";
                        //        txtCodMotorista.Focus();

                        //    }
                        //    else
                        //    {
                        //        if (txtCNH.Text == "")
                        //        {
                        //            string nomeUsuario = txtUsuCadastro.Text;
                        //            string linha1 = "Olá, " + nomeUsuario + "!";
                        //            string linha2 = "O motorista " + ddlMotorista.SelectedItem.Text.Trim() + ", não tem validade de CNH, lançada no sistema.";
                        //            string linha3 = "Verifique, por favor.";
                        //            //string linha4 = "Unidade: " + unidade + ". Por favor, verifique.";
                        //            // Concatenando as linhas com '\n' para criar a mensagem
                        //            string mensagem = $"{linha1}\n{linha2}\n{linha3}";
                        //            string mensagemCodificada = HttpUtility.JavaScriptStringEncode(mensagem);
                        //            //// Gerando o script JavaScript para exibir o alerta
                        //            string script = $"alert('{mensagemCodificada}');";
                        //            //// Registrando o script para execução no lado do cliente
                        //            ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", script, true);
                        //            txtCodMotorista.Text = "";
                        //            txtCodMotorista.Focus();

                        //        }
                        //        else
                        //        {
                        //            txtCNH.BackColor = System.Drawing.Color.LightGray;
                        //            txtCNH.ForeColor = System.Drawing.Color.Black;
                        //        }
                        //    }
                        //}

                        //// pesquisar validade da liberação GR
                        //if (!string.IsNullOrWhiteSpace(txtLibGR.Text))
                        //{
                        //    DateTime dataHoje = DateTime.Now;
                        //    DateTime dataGR;
                        //    if (DateTime.TryParseExact(txtLibGR.Text.Trim(), "dd/MM/yyyy",
                        //       System.Globalization.CultureInfo.InvariantCulture,
                        //       System.Globalization.DateTimeStyles.None,
                        //       out dataGR))
                        //    {

                        //        TimeSpan diferencaGR = dataGR - dataHoje;
                        //        // Agora você pode comparar a diferença
                        //        if (diferencaGR.TotalDays < 30 && diferencaGR.TotalDays > 0)
                        //        {
                        //            txtExameToxic.BackColor = System.Drawing.Color.Khaki;
                        //            txtExameToxic.ForeColor = System.Drawing.Color.OrangeRed;
                        //            string nomeUsuario = txtUsuCadastro.Text;
                        //            string linha1 = "Olá, " + nomeUsuario + "!";
                        //            string linha2 = "A liberãção de risco do motorista " + ddlMotorista.SelectedItem.Text.Trim() + ", vence em menos de 30 dias.";
                        //            string linha3 = "Data de vencimento: " + dataGR.ToString("dd/MM/yyyy") + ".";
                        //            //string linha4 = "Unidade: " + unidade + ". Por favor, verifique.";
                        //            // Concatenando as linhas com '\n' para criar a mensagem
                        //            string mensagem = $"{linha1}\n{linha2}\n{linha3}";
                        //            string mensagemCodificada = HttpUtility.JavaScriptStringEncode(mensagem);
                        //            //// Gerando o script JavaScript para exibir o alerta
                        //            string script = $"alert('{mensagemCodificada}');";
                        //            //// Registrando o script para execução no lado do cliente
                        //            ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", script, true);
                        //        }
                        //        else if (diferencaGR.TotalDays <= 0)
                        //        {
                        //            txtExameToxic.BackColor = System.Drawing.Color.Red;
                        //            txtExameToxic.ForeColor = System.Drawing.Color.White;
                        //            string nomeUsuario = txtUsuCadastro.Text;
                        //            string linha1 = "Olá, " + nomeUsuario + "!";
                        //            string linha2 = "A liberação de risco do motorista " + ddlMotorista.SelectedItem.Text.Trim() + ", está vencida.";
                        //            string linha3 = "Data do vencimento: " + dataGR.ToString("dd/MM/yyyy") + ".";
                        //            //string linha4 = "Unidade: " + unidade + ". Por favor, verifique.";
                        //            // Concatenando as linhas com '\n' para criar a mensagem
                        //            string mensagem = $"{linha1}\n{linha2}\n{linha3}";
                        //            string mensagemCodificada = HttpUtility.JavaScriptStringEncode(mensagem);
                        //            //// Gerando o script JavaScript para exibir o alerta
                        //            string script = $"alert('{mensagemCodificada}');";
                        //            //// Registrando o script para execução no lado do cliente
                        //            ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", script, true);
                        //            txtCodMotorista.Text = "";
                        //            txtCodMotorista.Focus();

                        //        }
                        //    }
                        //    else
                        //    {
                        //        if (txtLibGR.Text == "")
                        //        {
                        //            string nomeUsuario = txtUsuCadastro.Text;
                        //            string linha1 = "Olá, " + nomeUsuario + "!";
                        //            string linha2 = "O motorista " + ddlMotorista.SelectedItem.Text.Trim() + ", não tem liberação de risco cadastrada.";
                        //            string linha3 = "Verifique, por favor.";
                        //            //string linha4 = "Unidade: " + unidade + ". Por favor, verifique.";
                        //            // Concatenando as linhas com '\n' para criar a mensagem
                        //            string mensagem = $"{linha1}\n{linha2}\n{linha3}";
                        //            string mensagemCodificada = HttpUtility.JavaScriptStringEncode(mensagem);
                        //            //// Gerando o script JavaScript para exibir o alerta
                        //            string script = $"alert('{mensagemCodificada}');";
                        //            //// Registrando o script para execução no lado do cliente
                        //            ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", script, true);
                        //            txtCodMotorista.Text = "";
                        //            txtCodMotorista.Focus();

                        //        }

                        //        else
                        //        {
                        //            txtLibGR.BackColor = System.Drawing.Color.LightGray;
                        //            txtLibGR.ForeColor = System.Drawing.Color.Black;
                        //        }
                        //    }
                        //}


                        //// pesquisar primeiro reboque
                        //if (txtReboque1.Text.Trim() != "")
                        //{
                        //    var placaReboque1 = txtReboque1.Text.Trim();

                        //    var objCarreta = new Domain.ConsultaReboque
                        //    {
                        //        placacarreta = placaReboque1
                        //    };
                        //    var ConsultaReboque = DAL.UsersDAL.CheckReboque(objCarreta);
                        //    if (ConsultaReboque != null)
                        //    {
                        //        txtCRLVReb1.Text = ConsultaReboque.licenciamento.Trim().ToString();
                        //    }
                        //}

                        //// pesquisar segundo reboque
                        //if (txtReboque2.Text.Trim() != "")
                        //{
                        //    var placaReboque2 = txtReboque2.Text.Trim();

                        //    var objCarreta = new Domain.ConsultaReboque
                        //    {
                        //        placacarreta = placaReboque2
                        //    };
                        //    var ConsultaReboque = DAL.UsersDAL.CheckReboque(objCarreta);
                        //    if (ConsultaReboque != null)
                        //    {
                        //        txtCRLVReb2.Text = ConsultaReboque.licenciamento.Trim().ToString();
                        //    }
                        //}
                        //txtCodVeiculo.Focus();
                    }
                }
                else
                {                    
                    // Acione o toast quando a página for carregada
                    string script = "<script>showToast('Motorista não cadastrado no sistema!');</script>";
                    ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);

                    fotoMotorista = "/img/totalFunc.png";
                    txtCodMot.Text = "";
                    txtCodMot.Focus();

                }

            }

        }
        public void CarregaFoto()
        {
            var codigo = txtCodMot.Text.Trim();

            var obj = new Domain.ConsultaMotorista
            {
                codmot = codigo
            };
            var ConsultaMotorista = DAL.UsersDAL.CheckMotorista(obj);
            if (ConsultaMotorista != null)
            {
                if (ConsultaMotorista.status.Trim() != "INATIVO")
                {
                    if (txtCodMot.Text.Trim() != "")
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
                CarregarGridPedidos(numeroCarga);

            }
        }
        // Função chamada no <%# ... %> para calcular o tempo de carregamento
        
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
                cboMotorista.DataSource = reader;
                cboMotorista.DataTextField = "nommot";
                cboMotorista.DataValueField = "codmot";
                cboMotorista.DataBind();
                cboMotorista.Items.Insert(0, "");

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

    }
}