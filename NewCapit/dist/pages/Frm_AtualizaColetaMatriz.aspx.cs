using AjaxControlToolkit.Design;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Office.Word;
using DocumentFormat.OpenXml.Wordprocessing;
using Domain;
using GMaps;
using GMaps.Classes;
using ICSharpCode.SharpZipLib.Zip;
using MathNet.Numerics;
using MathNet.Numerics.Providers.SparseSolver;
using NewCapit.dist.Models;
using NewCapit.dist.Services;
using NewCapit.dist.ServicesCte;
using NewCapit.Models.Krona;
using Newtonsoft.Json;
using NPOI.SS.Formula;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.Utils;
using Org.BouncyCastle.Asn1.Cmp;
using Org.BouncyCastle.Asn1.Ocsp;
using Subgurim;
using Subgurim.Controles;
using Subgurim.Controls;
using Subgurim.Maps;
using Subgurim.Web;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Numerics;
using System.Security.Principal;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml;
using static NewCapit.dist.pages.Frm_TabelaPrecoMatriz;
using static NPOI.HSSF.Util.HSSFColor;
using static System.Reflection.Metadata.BlobBuilder;
using DataTable = System.Data.DataTable;
using NewCapit.dist.InterfacesCte;
using System.Net.NetworkInformation;
using DocumentFormat.OpenXml.Spreadsheet;
using WebPage = System.Web.UI.Page;
using DocumentFormat.OpenXml.VariantTypes;
using NewCapit.dist.ModelsCVA;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using DocumentFormat.OpenXml.Vml;
using static NPOI.HSSF.Record.UnicodeString;
using TextBox = System.Web.UI.WebControls.TextBox;


namespace NewCapit.dist.pages
{

    public partial class Frm_AtualizaColetaMatriz : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conexao"].ToString());
        public string fotoMotorista;
        string codmot, caminhofoto;
        string num_coleta;
        string statusOC;
        string situacaoOC;
        string cvaOC;
        string chegada;
        string andamentoCarga;

        private DataTable dtStatus;
        private DataTable dtEmpresas;
        private DataTable dtEstabelecimentos;
        private DataTable dtVeiculos;
        public string Data { get; set; } // errado se não mapear

        DateTime dataHoraAtual = DateTime.Now;
        BigInteger idveiculo;
        string cidade, empresa, lat, lon, ignicao, bairro, rua, uf, id, placa, hora, velocidade, preferencia, bloqueio;

        GInfoWindow window;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UsuarioLogado"] != null)
                {
                    string nomeUsuario = Session["UsuarioLogado"].ToString();
                    var lblUsuario = nomeUsuario;
                }
                else
                {
                    Response.Redirect("Login.aspx");
                }
                DateTime dataHoraAtual = DateTime.Now;
                fotoMotorista = "/fotos/motoristasemfoto.jpg";

                CarregarMotoristas();
                CarregaDados();
                CarregaMap(txtPlaca.Text);
                PreencherClienteInicial();
                PreencherClienteFinal();
                CarregarCombos();
                CarregarColetas();
            }
            CarregaFoto();
        }
        protected void btnCalcular_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            RepeaterItem item = (RepeaterItem)btn.NamingContainer;

            // Recupera os controles do Repeater           
            DropDownList ddlTipoCte = (DropDownList)item.FindControl("ddlTipoCte");
            GridView gvComponentes = (GridView)item.FindControl("gvComponentes");

            Label lblCFOP = (Label)item.FindControl("lblCFOP");
            Label lblTotalFrete = (Label)item.FindControl("lblTotalFrete");
            Label lblICMS = (Label)item.FindControl("lblICMS");
            Label lblIBS = (Label)item.FindControl("lblIBS");
            Label lblCBS = (Label)item.FindControl("lblCBS");
            Label lblTotal = (Label)item.FindControl("lblTotal");

            var service = new CteService(
                new CfopService(),
                new FreteService(),
                new ImpostoService()
            );

            string ufOrigem = txtUfOrigem.Text;
            string ufDestino = txtUfDestino.Text;
            string tipoCte = ddlTipoCte.SelectedValue;

            List<ComponenteFrete> componentes = new List<ComponenteFrete>();

            foreach (GridViewRow row in gvComponentes.Rows)
            {
                TextBox txtTipo = (TextBox)row.FindControl("txtTipo");
                TextBox txtValor = (TextBox)row.FindControl("txtValor");

                decimal valor = 0;
                decimal.TryParse(txtValor.Text, out valor);

                componentes.Add(new ComponenteFrete
                {
                    Tipo = txtTipo.Text,
                    Valor = valor
                });
            }

            var resultado = service.CalcularCte(
                ufOrigem,
                ufDestino,
                tipoCte,
                componentes,
                12m, // ICMS
                18m, // IBS
                9m   // CBS
            );

            lblCFOP.Text = resultado.CFOP;
            lblTotalFrete.Text = resultado.TotalComponentes.ToString("N2");
            lblICMS.Text = resultado.ICMS.ToString("N2");
            lblIBS.Text = resultado.IBS.ToString("N2");
            lblCBS.Text = resultado.CBS.ToString("N2");
            lblTotal.Text = resultado.TotalGeral.ToString("N2");
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
                            fotoMotorista = "/fotos/motoristasemfoto.jpg";
                        }
                    }

                }

            }

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

            string sql = "select * from tbcarregamentos where num_carregamento='" + num_coleta + "'";

            SqlDataAdapter adtp = new SqlDataAdapter(sql, con);
            System.Data.DataTable dt = new System.Data.DataTable();
            con.Open();
            adtp.Fill(dt);
            con.Close();
            //Carrega Motorista
            novaColeta.Text = num_coleta;
            var codigo = dt.Rows[0][6].ToString();
            txtCodMotorista.Text = codigo;
            txtFilialMot.Text = dt.Rows[0][38].ToString();
            if (dt.Rows[0][130].ToString() == "")
            {
                txtLibGR.BackColor = System.Drawing.Color.Red;
                txtLibGR.ForeColor = System.Drawing.Color.White;
                txtLibGR.Text = "Verifique";
            }
            else
            {
                txtLibGR.Text = DateTime.Parse(dt.Rows[0][130].ToString()).ToString("dd/MM/yyyy");
                DateTime dataHoje = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                DateTime dataGR = Convert.ToDateTime(txtLibGR.Text);

                TimeSpan diferencaGR = dataGR - dataHoje;
                // Agora você pode comparar a diferença
                if (diferencaGR.TotalDays <= 30)
                {
                    string diasGR = diferencaGR.TotalDays.ToString();
                    txtLibGR.Text = txtLibGR.Text + " (" + diasGR + " dias)";
                    txtLibGR.BackColor = System.Drawing.Color.Khaki;
                    txtLibGR.ForeColor = System.Drawing.Color.OrangeRed;
                }
                else if (diferencaGR.TotalDays <= 0)
                {
                    txtLibGR.Text = DateTime.Parse(dt.Rows[0][130].ToString()).ToString("dd/MM/yyyy");
                    txtLibGR.BackColor = System.Drawing.Color.Red;
                    txtLibGR.ForeColor = System.Drawing.Color.White;
                    txtLibGR.Text = txtLibGR.Text + " (Vencida)";
                }
            }
            txtTipoMot.Text = dt.Rows[0][128].ToString();

            if (dt.Rows[0][86].ToString() == "")
            {
                txtCNH.BackColor = System.Drawing.Color.Red;
                txtCNH.ForeColor = System.Drawing.Color.White;
                txtCNH.Text = "Verifique";

            }
            else
            {
                txtCNH.Text = DateTime.Parse(dt.Rows[0][86].ToString()).ToString("dd/MM/yyyy");
                DateTime dataHoje = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                DateTime dataCNH = Convert.ToDateTime(txtCNH.Text);

                TimeSpan diferenca = dataCNH - dataHoje;
                // Agora você pode comparar a diferença
                if (diferenca.TotalDays < 30)
                {
                    string diasCNH = diferenca.TotalDays.ToString();
                    txtCNH.Text = txtCNH.Text + " (" + diasCNH + " dias)";
                    txtCNH.BackColor = System.Drawing.Color.Khaki;
                    txtCNH.ForeColor = System.Drawing.Color.OrangeRed;
                }
                else if (diferenca.TotalDays <= 0)
                {
                    txtCNH.Text = DateTime.Parse(dt.Rows[0][86].ToString()).ToString("dd/MM/yyyy");
                    txtCNH.BackColor = System.Drawing.Color.Red;
                    txtCNH.ForeColor = System.Drawing.Color.White;
                    txtCNH.Text = txtCNH.Text + " (Vencida)";
                }

            }
            txtFuncao.Text = dt.Rows[0][143].ToString();
            if (txtTipoMot.Text == "FUNCIONÁRIO")
            {
                ETI.Visible = true;
                if (dt.Rows[0][129].ToString() == "")
                {
                    txtExameToxic.BackColor = System.Drawing.Color.Red;
                    txtExameToxic.ForeColor = System.Drawing.Color.White;
                    txtExameToxic.Text = "Verifique";
                }
                else
                {
                    txtExameToxic.Text = DateTime.Parse(dt.Rows[0][129].ToString()).ToString("dd/MM/yyyy");
                    if (txtExameToxic.Text == "")
                    {
                        txtExameToxic.BackColor = System.Drawing.Color.Red;
                        txtExameToxic.ForeColor = System.Drawing.Color.White;
                        txtExameToxic.Text = "Verifique";
                    }
                    else
                    {
                        DateTime dataHoje = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                        DateTime dataETI = Convert.ToDateTime(txtExameToxic.Text);

                        TimeSpan diferencaETI = dataETI - dataHoje;
                        // Agora você pode comparar a diferença
                        if (diferencaETI.TotalDays < 30)
                        {
                            string diasETI = diferencaETI.TotalDays.ToString();
                            txtExameToxic.Text = txtExameToxic.Text + " (" + diasETI + " dias)";
                            txtExameToxic.BackColor = System.Drawing.Color.Khaki;
                            txtExameToxic.ForeColor = System.Drawing.Color.OrangeRed;
                        }
                        else if (diferencaETI.TotalDays <= 0)
                        {
                            txtExameToxic.BackColor = System.Drawing.Color.Red;
                            txtExameToxic.ForeColor = System.Drawing.Color.White;
                            txtExameToxic.Text = txtExameToxic.Text + " (Vencido)";
                        }
                        else
                        {
                            if (txtExameToxic.Text == "")
                            {
                                txtExameToxic.BackColor = System.Drawing.Color.Red;
                                txtExameToxic.ForeColor = System.Drawing.Color.White;
                                txtExameToxic.Text = "Verifique";
                            }
                            else
                            {
                                txtExameToxic.BackColor = System.Drawing.Color.LightGray;
                                txtExameToxic.ForeColor = System.Drawing.Color.Black;
                            }
                        }
                    }

                }


            }
            else
            {
                ETI.Visible = false;
            }
            ddlMotorista.Items.Insert(0, new System.Web.UI.WebControls.ListItem(dt.Rows[0][5].ToString(), ""));
            txtCPF.Text = dt.Rows[0][80].ToString();
            txtCartao.Text = dt.Rows[0][81].ToString();
            string val = dt.Rows[0][131].ToString();
            if (!string.IsNullOrEmpty(val) && val != "NULL")
            {
                txtValCartao.Text = DateTime.Parse(val).ToString("dd/MM/yyyy");
            }

            txtCelular.Text = dt.Rows[0][10].ToString();


            fotoMotorista = dt.Rows[0][87].ToString();

            if (!File.Exists(fotoMotorista))
            {
                fotoMotorista = "../.." + dt.Rows[0][87].ToString();
            }
            else
            {
                fotoMotorista = "/fotos/motoristasemfoto.jpg";
            }

            txtUsuCadastro.Text = dt.Rows[0][76].ToString();
            lblDtCadastro.Text = DateTime.Parse(dt.Rows[0][77].ToString()).ToString("dd/MM/yyyy HH:mm");
            txtCodFrota.Text = dt.Rows[0][7].ToString();
            txtFoneCorp.Text = dt.Rows[0][9].ToString();
            txtCodVeiculo.Text = dt.Rows[0][15].ToString(); //+ "/" + dt.Rows[0][13].ToString();            
            txtPlaca.Text = dt.Rows[0][16].ToString();
            CarregaMap(dt.Rows[0][16].ToString());
            txtVeiculoTipo.Text = dt.Rows[0][132].ToString();
            txtTipoVeiculo.Text = dt.Rows[0][14].ToString();
            txtReboque1.Text = dt.Rows[0][17].ToString();

            if (dt.Rows[0][135].ToString() == "")
            {
                txtOpacidade.BackColor = System.Drawing.Color.Red;
                txtOpacidade.ForeColor = System.Drawing.Color.White;
                txtOpacidade.Text = "Verifique";
            }
            else
            {
                txtOpacidade.Text = DateTime.Parse(dt.Rows[0][135].ToString()).ToString("dd/MM/yyyy");
                DateTime dataHoje = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                DateTime dataLicenciamento = Convert.ToDateTime(txtOpacidade.Text);

                TimeSpan diferencaLicenciamento = dataLicenciamento - dataHoje;
                // Agora você pode comparar a diferença
                if (diferencaLicenciamento.TotalDays < 30)
                {
                    string diasLicenciamento = diferencaLicenciamento.TotalDays.ToString();
                    txtOpacidade.Text = txtOpacidade.Text + " (" + diasLicenciamento + " dias)";
                    txtOpacidade.BackColor = System.Drawing.Color.Khaki;
                    txtOpacidade.ForeColor = System.Drawing.Color.OrangeRed;
                }
                else if (diferencaLicenciamento.TotalDays <= 0)
                {
                    txtOpacidade.Text = DateTime.Parse(dt.Rows[0][135].ToString()).ToString("dd/MM/yyyy");
                    txtOpacidade.BackColor = System.Drawing.Color.Red;
                    txtOpacidade.ForeColor = System.Drawing.Color.White;
                    txtOpacidade.Text = txtOpacidade.Text + " (Vencido)";
                }
            }

            if (dt.Rows[0][136].ToString().Length > 0)
            {
                txtCET.Text = DateTime.Parse(dt.Rows[0][136].ToString()).ToString("dd/MM/yyyy");
                if (txtCET.Text == "")
                {
                    txtCET.BackColor = System.Drawing.Color.Red;
                    txtCET.ForeColor = System.Drawing.Color.White;
                    txtCET.Text = "Verifique";
                }
                else
                {
                    DateTime dataHoje = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                    DateTime dataLicenciamento = Convert.ToDateTime(txtCET.Text);

                    TimeSpan diferencaLicenciamento = dataLicenciamento - dataHoje;
                    // Agora você pode comparar a diferença
                    if (diferencaLicenciamento.TotalDays < 30)
                    {
                        string diasLicenciamento = diferencaLicenciamento.TotalDays.ToString();
                        txtCET.Text = txtCET.Text + " (" + diasLicenciamento + " dias)";
                        txtCET.BackColor = System.Drawing.Color.Khaki;
                        txtCET.ForeColor = System.Drawing.Color.OrangeRed;
                    }
                    else if (diferencaLicenciamento.TotalDays <= 0)
                    {
                        txtCET.Text = DateTime.Parse(dt.Rows[0][137].ToString()).ToString("dd/MM/yyyy");
                        txtCET.BackColor = System.Drawing.Color.Red;
                        txtCET.ForeColor = System.Drawing.Color.White;
                        txtCET.Text = txtCRLVVeiculo.Text + " (Vencido)";
                    }
                }
            }


            if (dt.Rows[0][137].ToString().Length > 0)
            {
                txtCRLVVeiculo.Text = DateTime.Parse(dt.Rows[0][137].ToString()).ToString("dd/MM/yyyy");
                if (txtCRLVVeiculo.Text == "")
                {
                    txtCRLVVeiculo.BackColor = System.Drawing.Color.Red;
                    txtCRLVVeiculo.ForeColor = System.Drawing.Color.White;
                    txtCRLVVeiculo.Text = "Verifique";
                }
                else
                {
                    DateTime dataHoje = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                    DateTime dataLicenciamento = Convert.ToDateTime(txtCRLVVeiculo.Text);

                    TimeSpan diferencaLicenciamento = dataLicenciamento - dataHoje;
                    // Agora você pode comparar a diferença
                    if (diferencaLicenciamento.TotalDays < 30 && diferencaLicenciamento.TotalDays >= 1)
                    {
                        string diasLicenciamento = diferencaLicenciamento.TotalDays.ToString();
                        txtCRLVVeiculo.Text = txtCRLVVeiculo.Text + " (" + diasLicenciamento + " dias)";
                        txtCRLVVeiculo.BackColor = System.Drawing.Color.Khaki;
                        txtCRLVVeiculo.ForeColor = System.Drawing.Color.OrangeRed;
                    }
                    else if (diferencaLicenciamento.TotalDays <= 0)
                    {
                        txtCRLVVeiculo.Text = DateTime.Parse(dt.Rows[0][137].ToString()).ToString("dd/MM/yyyy");
                        txtCRLVVeiculo.BackColor = System.Drawing.Color.Red;
                        txtCRLVVeiculo.ForeColor = System.Drawing.Color.White;
                        txtCRLVVeiculo.Text = txtCRLVVeiculo.Text + " (Vencido)";
                    }
                }
            }

            if (txtTipoVeiculo.Text == "BITRUCK" || txtTipoVeiculo.Text.Trim() == "UTILITÁRIO/FURGÃO" || txtTipoVeiculo.Text.Trim() == "LEVE" || txtTipoVeiculo.Text.Trim() == "FIORINO" || txtTipoVeiculo.Text.Trim() == "TOCO" || txtTipoVeiculo.Text.Trim() == "TRUCK" || txtTipoVeiculo.Text.Trim() == "VUC OU 3/4")
            {
                reb1.Visible = false;
                reboque1.Visible = false;
                carretas.Visible = false;
            }
            else
            {
                reb1.Visible = true;
                reboque1.Visible = true;
                carretas.Visible = true;
                if (dt.Rows[0][138].ToString() == "")
                {
                    txtCRLVReb1.BackColor = System.Drawing.Color.Red;
                    txtCRLVReb1.ForeColor = System.Drawing.Color.White;
                    txtCRLVReb1.Text = "Verifique";
                }
                else
                {
                    txtCRLVReb1.Text = DateTime.Parse(dt.Rows[0][138].ToString()).ToString("dd/MM/yyyy");
                    DateTime dataHoje = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                    DateTime dataLicenciamento = Convert.ToDateTime(txtCRLVReb1.Text);

                    TimeSpan diferencaLicenciamento = dataLicenciamento - dataHoje;
                    // Agora você pode comparar a diferença
                    if (diferencaLicenciamento.TotalDays < 30)
                    {
                        string diasLicenciamento = diferencaLicenciamento.TotalDays.ToString();
                        txtCRLVReb1.Text = txtCRLVReb1.Text + " (" + diasLicenciamento + " dias)";
                        txtCRLVReb1.BackColor = System.Drawing.Color.Khaki;
                        txtCRLVReb1.ForeColor = System.Drawing.Color.OrangeRed;
                    }
                    if (diferencaLicenciamento.TotalDays <= 0)
                    {
                        txtCRLVReb1.Text = DateTime.Parse(dt.Rows[0][138].ToString()).ToString("dd/MM/yyyy");
                        txtCRLVReb1.BackColor = System.Drawing.Color.Red;
                        txtCRLVReb1.ForeColor = System.Drawing.Color.White;
                        txtCRLVReb1.Text = txtCRLVReb1.Text + " (Vencido)";
                    }
                }

            }

            if (txtTipoVeiculo.Text.Trim() == "CAVALO SIMPLES" || txtTipoVeiculo.Text.Trim() == "CAVALO TRUCADO" || txtTipoVeiculo.Text.Trim() == "CAVALO 4 EIXOS")
            {
                reb1.Visible = true;
                reboque1.Visible = true;
                carretas.Visible = true;
                if (dt.Rows[0][138].ToString() == "")
                {
                    txtCRLVReb1.BackColor = System.Drawing.Color.Red;
                    txtCRLVReb1.ForeColor = System.Drawing.Color.White;
                    txtCRLVReb1.Text = "Verifique";
                }
                else
                {
                    txtCRLVReb1.Text = DateTime.Parse(dt.Rows[0][138].ToString()).ToString("dd/MM/yyyy");
                    DateTime dataHoje = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                    DateTime dataLicenciamento = Convert.ToDateTime(txtCRLVReb1.Text);

                    TimeSpan diferencaLicenciamento = dataLicenciamento - dataHoje;
                    // Agora você pode comparar a diferença
                    if (diferencaLicenciamento.TotalDays < 30)
                    {
                        string diasLicenciamento = diferencaLicenciamento.TotalDays.ToString();
                        txtCRLVReb1.Text = txtCRLVReb1.Text + " (" + diasLicenciamento + " dias)";
                        txtCRLVReb1.BackColor = System.Drawing.Color.Khaki;
                        txtCRLVReb1.ForeColor = System.Drawing.Color.OrangeRed;
                    }
                    if (diferencaLicenciamento.TotalDays <= 0)
                    {
                        txtCRLVReb1.Text = DateTime.Parse(dt.Rows[0][138].ToString()).ToString("dd/MM/yyyy");
                        txtCRLVReb1.BackColor = System.Drawing.Color.Red;
                        txtCRLVReb1.ForeColor = System.Drawing.Color.White;
                        txtCRLVReb1.Text = txtCRLVReb1.Text + " (Vencido)";
                    }
                }


            }
            else
            {
                reb2.Visible = false;
                reboque2.Visible = false;
            }

            if (txtTipoVeiculo.Text == "BITREM 7 EIXOS" || txtTipoVeiculo.Text == "BITREM 8 EIXOS" || txtTipoVeiculo.Text == "BITREM 9 EIXOS")
            {
                reb2.Visible = true;
                reboque2.Visible = true;

                txtReboque2.Text = dt.Rows[0][18].ToString();
                if (dt.Rows[0][139].ToString() == "")
                {
                    txtCRLVReb2.BackColor = System.Drawing.Color.Red;
                    txtCRLVReb2.ForeColor = System.Drawing.Color.White;
                    txtCRLVReb2.Text = "Verifique";
                }
                else
                {
                    txtCRLVReb2.Text = DateTime.Parse(dt.Rows[0][139].ToString()).ToString("dd/MM/yyyy");
                    if (txtCRLVReb2.Text == "")
                    {
                        txtCRLVReb2.BackColor = System.Drawing.Color.Red;
                        txtCRLVReb2.ForeColor = System.Drawing.Color.White;
                        txtCRLVReb2.Text = "Verifique";
                    }
                    else
                    {
                        DateTime dataHoje = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                        DateTime dataLicenciamento = Convert.ToDateTime(txtCRLVReb2.Text);

                        TimeSpan diferencaLicenciamento = dataLicenciamento - dataHoje;
                        // Agora você pode comparar a diferença
                        if (diferencaLicenciamento.TotalDays < 30)
                        {
                            string diasLicenciamento = diferencaLicenciamento.TotalDays.ToString();
                            txtCRLVReb2.Text = txtCRLVReb1.Text + " (" + diasLicenciamento + " dias)";
                            txtCRLVReb2.BackColor = System.Drawing.Color.Khaki;
                            txtCRLVReb2.ForeColor = System.Drawing.Color.OrangeRed;
                        }
                        else if (diferencaLicenciamento.TotalDays <= 0)
                        {
                            txtCRLVReb2.Text = DateTime.Parse(dt.Rows[0][139].ToString()).ToString("dd/MM/yyyy");
                            txtCRLVReb2.BackColor = System.Drawing.Color.Red;
                            txtCRLVReb2.ForeColor = System.Drawing.Color.White;
                            txtCRLVReb2.Text = txtCRLVReb1.Text + " (Vencido)";
                        }
                    }
                }
            }
            else
            {
                reboque2.Visible = false;
                reb2.Visible = false;
            }
            txtCarreta.Text = dt.Rows[0][104].ToString();
            txtTecnologia.Text = dt.Rows[0][84].ToString();
            txtRastreamento.Text = dt.Rows[0][82].ToString();
            txtConjunto.Text = dt.Rows[0][90].ToString();
            txtCodProprietario.Text = dt.Rows[0][12].ToString();
            txtProprietario.Text = dt.Rows[0][13].ToString();
            txtCodTransportadora.Text = dt.Rows[0][145].ToString();
            txtTransportadora.Text = dt.Rows[0][146].ToString();
            txtLiberacao.Text = dt.Rows[0][171].ToString();
            txtProtocoloCET.Text = dt.Rows[0][172].ToString();
            txtAtualizadoPor.Text = dt.Rows[0][141].ToString();
            DateTime? data = null;

            if (dt.Rows[0]["dtalt"] != DBNull.Value)
            {
                data = Convert.ToDateTime(dt.Rows[0]["dtalt"]);
            }
            string dataAlteracao = data?.ToString("dd/MM/yyyy HH:mm");
            lblAtualizadoEm.Text = dataAlteracao;
            Session["Coletas"] = null;
            string idviagem;
            idviagem = num_coleta;
            CarregarColetas(idviagem);

        }
        // protected void rptColetas_ItemDataBound(object sender, RepeaterItemEventArgs e)
        // {
        //     //if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
        //     //    return;

        //     Label lblTotal = (Label)e.Item.FindControl("lblTotal");
        //     if (lblTotal != null)
        //     {
        //         lblTotal.Text = "0,00";
        //     }
        //     Label lblCBS = (Label)e.Item.FindControl("lblCBS");
        //     if (lblCBS != null)
        //     {
        //         lblCBS.Text = "0,00";
        //     }
        //     Label lblTotalFrete = (Label)e.Item.FindControl("lblTotalFrete");
        //     if (lblTotalFrete != null)
        //     {
        //         lblTotalFrete.Text = "0,00";
        //     }
        //     Label lblICMS = (Label)e.Item.FindControl("lblICMS");
        //     if (lblICMS != null)
        //     {
        //         lblICMS.Text = "0,00";
        //     }
        //     Label lblIBS = (Label)e.Item.FindControl("lblIBS");
        //     if (lblIBS != null)
        //     {
        //         lblIBS.Text = "0,00";
        //     }

        //     DropDownList ddlTipoCte = (DropDownList)e.Item.FindControl("ddlTipoCte");
        //     if (ddlTipoCte != null)
        //     {
        //         string tipo = ddlTipoCte.SelectedValue;
        //     }



        //     var ddlStatus = (DropDownList)e.Item.FindControl("ddlStatus");
        //     //if (ddlStatus == null) return;

        //     // 1) carrega os status da tabela
        //     const string sql = "SELECT cod_status, ds_status FROM tb_status";
        //     using (var conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
        //     using (var cmd = new SqlCommand(sql, conn))
        //     {
        //         try
        //         {
        //             conn.Open();
        //             using (var rdr = cmd.ExecuteReader())
        //             {
        //                 ddlStatus.DataSource = rdr;
        //                 ddlStatus.DataTextField = "ds_status";
        //                 ddlStatus.DataValueField = "ds_status";
        //                 ddlStatus.DataBind();
        //             }
        //             var drv = (HiddenField)e.Item.FindControl("hdfStatus"); ;
        //             string statusDaColeta = drv.Value;  // o nome da coluna do seu DataTable
        //             // opcional: insere item em branco no topo
        //             ddlStatus.Items.Insert(0, new System.Web.UI.WebControls.ListItem(statusDaColeta, "0"));
        //         }
        //         catch (Exception ex)
        //         {
        //             // trate o erro como preferir
        //             Response.Write("Erro ao carregar status: " + ex.Message);
        //             return;
        //         }
        //     }

        //     // Estabelecimentos no CVA
        //     var ddlEstabelecimentoCVA = (DropDownList)e.Item.FindControl("ddlEstabelecimentoCVA");            
        //     if (ddlEstabelecimentoCVA == null) return;
        //     const string sqlEstabelecimentos = "select cod_estabelecimento, nom_estabelecimento from tbestabelecimentos where fl_exclusao is null and sit_estabelecimento = 'ATIVO'";
        //     using (var conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
        //         using (var cmd = new SqlCommand(sqlEstabelecimentos, conn))
        //     {
        //         try
        //         {
        //             conn.Open();
        //             using (var rdr = cmd.ExecuteReader())
        //             {
        //                 ddlEstabelecimentoCVA.DataSource = rdr;
        //                 ddlEstabelecimentoCVA.DataTextField = "nom_estabelecimento";
        //                 ddlEstabelecimentoCVA.DataValueField = "cod_estabelecimento";
        //                 ddlEstabelecimentoCVA.DataBind();
        //             }                  
        //             //var drv = (HiddenField)e.Item.FindControl("hdfCVA"); ;
        //             //string filialDaColeta = drv.Value;  // o nome da coluna do seu DataTable
        //             // opcional: insere item em branco no topo
        //             //cbFiliais.Items.Insert(0, new System.Web.UI.WebControls.ListItem(filialDaColeta, "0"));
        //         }
        //         catch (Exception ex)
        //         {
        //             // trate o erro como preferir
        //             Response.Write("Erro ao carregar filiais: " + ex.Message);
        //         }
        //     }

        //     // Filiais na Notas Fiscais
        //     var ddlEmpresa = (DropDownList)e.Item.FindControl("ddlEmpresa");
        //     if (ddlEmpresa == null) return;
        //     const string sqlEmpresas = "SELECT codigo, descricao FROM tbempresa";
        //     using (var conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
        //     using (var cmd = new SqlCommand(sqlEmpresas, conn))
        //     {
        //         try
        //         {
        //             conn.Open();
        //             using (var rdr = cmd.ExecuteReader())
        //             {
        //                 ddlEmpresa.DataSource = rdr;
        //                 ddlEmpresa.DataTextField = "descricao";
        //                 ddlEmpresa.DataValueField = "codigo";
        //                 ddlEmpresa.DataBind();
        //             }
        //             ddlEmpresa.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Selecione a FILIAL", "0")); 
        //         }
        //         catch (Exception ex)
        //         {
        //             // trate o erro como preferir
        //             Response.Write("Erro ao carregar filiais: " + ex.Message);
        //         }
        //     }

        //     // Veiculo cobrado
        //     var ddlVeiculoCobrado = (DropDownList)e.Item.FindControl("ddlVeiculoCobrado");
        //     if (ddlVeiculoCobrado == null) return;
        //     const string sqlVeiculoCobrado = @"
        //     SELECT DISTINCT RTRIM(LTRIM(descricao_tng)) AS descricao_tng
        //     FROM tbtipoveic
        //     WHERE descricao_tng IS NOT NULL
        //     ORDER BY descricao_tng";
        //     using (var conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
        //     using (var cmd = new SqlCommand(sqlVeiculoCobrado, conn))
        //     {
        //         try
        //         {
        //             conn.Open();
        //             using (var rdr = cmd.ExecuteReader())
        //             {
        //                 ddlVeiculoCobrado.DataSource = rdr;
        //                 ddlVeiculoCobrado.DataTextField = "descricao_tng";
        //                 ddlVeiculoCobrado.DataValueField = "descricao_tng";
        //                 ddlVeiculoCobrado.DataBind();
        //             }
        //             ddlVeiculoCobrado.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Selecione o VEICULO", "0"));
        //         }
        //         catch (Exception ex)
        //         {
        //             // trate o erro como preferir
        //             Response.Write("Erro ao carregar filiais: " + ex.Message);
        //         }
        //     }

        //     if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        //     {
        //         // ... (suas declarações de variáveis iniciais) ...
        //         string previsaoStr = DataBinder.Eval(e.Item.DataItem, "previsao")?.ToString();
        //         string dataHoraStr = DataBinder.Eval(e.Item.DataItem, "data_hora")?.ToString();
        //         string status = DataBinder.Eval(e.Item.DataItem, "status")?.ToString();
        //         string ufOrigem = DataBinder.Eval(e.Item.DataItem, "uf_expedidor")?.ToString();
        //         string ufDestino = DataBinder.Eval(e.Item.DataItem, "uf_recebedor")?.ToString();
        //         Label lblAtendimento = (Label)e.Item.FindControl("lblAtendimento");
        //         HtmlTableCell tdAtendimento = (HtmlTableCell)e.Item.FindControl("tdAtendimento");
        //         TextBox txtUfInicio = (TextBox)e.Item.FindControl("txtUfInicio");
        //         TextBox txtUfFim = (TextBox)e.Item.FindControl("txtUfFim");

        //         // Abertura CVA
        //         TextBox txtNumCVA = (TextBox)e.Item.FindControl("txtNumCVA");
        //         string descMaterial = DataBinder.Eval(e.Item.DataItem, "desc_material")?.ToString();
        //         string numSolicitacao = DataBinder.Eval(e.Item.DataItem, "carga")?.ToString();
        //         string cvaSolicitacao = DataBinder.Eval(e.Item.DataItem, "cva")?.ToString();                
        //         TextBox txtSitCVA = (TextBox)e.Item.FindControl("txtSitCVA"); 
        //         if (string.IsNullOrWhiteSpace(cvaSolicitacao))
        //         {                    
        //             txtSitCVA.Text = "EM LANCAMENTO";
        //             using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
        //             {
        //                 con.Open();

        //                 // Procura a solicitação
        //                 string sqlR1 = "SELECT r1_sol_tipo_geracao FROM tbsolicitacoes_r1 WHERE r1_sol_numero = @num";

        //                 using (SqlCommand cmd = new SqlCommand(sqlR1, con))
        //                 {
        //                     cmd.Parameters.AddWithValue("@num", numSolicitacao);

        //                     object tipoGeracao = cmd.ExecuteScalar();

        //                     if (tipoGeracao == null)
        //                     {
        //                         MostrarMsgSolicitacao(e.Item, "Solicitação não encontrada. Faça a importação manual.", "danger");
        //                         return;
        //                     }

        //                     // Procura o tipo de viagem
        //                     string sqlTipo = "SELECT descricao FROM tbtipogeracaosolicitacao WHERE codvw = @cod";

        //                     using (SqlCommand cmd2 = new SqlCommand(sqlTipo, con))
        //                     {
        //                         cmd2.Parameters.AddWithValue("@cod", tipoGeracao.ToString());

        //                         object descricao = cmd2.ExecuteScalar();

        //                         if (descricao == null)
        //                         {
        //                             ScriptManager.RegisterStartupScript(this, GetType(), "msg",
        //                                 "alert('Tipo de geração não encontrado na tabela tbtipogeracaosolicitacao.');", true);


        //                         }
        //                         else
        //                         {
        //                             //txtTipoViagemCVA.Text = descricao.ToString();
        //                         }
        //                     }
        //                 }
        //             }

        //         }
        //         else
        //         {
        //             txtSitCVA.Text = "CADASTRADO";

        //         }
        //         DateTime previsao, dataHora;
        //         DateTime agora = DateTime.Now;

        //         if (DateTime.TryParse(previsaoStr, out previsao) && DateTime.TryParse(dataHoraStr, out dataHora))
        //         {
        //             DateTime dataPrevisao = previsao.Date;
        //             DateTime dataHoraComparacao = new DateTime(
        //                 dataPrevisao.Year, dataPrevisao.Month, dataPrevisao.Day,
        //                 dataHora.Hour, dataHora.Minute, dataHora.Second
        //             );

        //             // Lógica para "Atrasado"
        //             if (dataHoraComparacao < agora)
        //             {
        //                 lblAtendimento.Text = "Atrasado";
        //                 // CORREÇÃO AQUI: Mova o BgColor para dentro do style
        //                 tdAtendimento.Attributes["style"] = "background-color: Red; color: white; font-weight: bold;";
        //             }
        //             // Lógica para "No Prazo"
        //             else if (dataHoraComparacao.Date == agora.Date && dataHoraComparacao.TimeOfDay <= agora.TimeOfDay)
        //             {
        //                 lblAtendimento.Text = "No Prazo";
        //                 // CORREÇÃO AQUI: Mova o BgColor para dentro do style
        //                 tdAtendimento.Attributes["style"] = "background-color: Green; color: white; font-weight: bold;";
        //             }
        //             // Lógica para "Antecipado"
        //             else if (dataHoraComparacao > agora)
        //             {
        //                 lblAtendimento.Text = "Antecipado";
        //                 // CORREÇÃO AQUI: Mova o BgColor para dentro do style
        //                 tdAtendimento.Attributes["style"] = "background-color: Orange; color: white; font-weight: bold;";
        //             }
        //             else
        //             {
        //                 lblAtendimento.Text = status;
        //                 // Opcional: Limpar estilos se não cair em nenhuma condição
        //                 tdAtendimento.Attributes["style"] = "";
        //             }
        //         }





        //     }
        //     if (e.Item.ItemType == ListItemType.Item ||
        //  e.Item.ItemType == ListItemType.AlternatingItem)
        //     {
        //         HiddenField hdIdCarga =
        //             (HiddenField)e.Item.FindControl("hdIdCarga");

        //         GridView gvPedidos =
        //             (GridView)e.Item.FindControl("gvPedidos");

        //         UpdatePanel upd =
        //             (UpdatePanel)e.Item.FindControl("updTabs");

        //         if (hdIdCarga != null && gvPedidos != null)
        //         {
        //             int idCarga;
        //             if (int.TryParse(hdIdCarga.Value, out idCarga))
        //             {
        //                 CarregarPedidos(idCarga, gvPedidos);

        //                 // força renderização do conteúdo
        //                 //upd.Update();
        //             }
        //         }
        //     }

        //     if (e.Item.ItemType == ListItemType.Item ||
        // e.Item.ItemType == ListItemType.AlternatingItem)
        //     {
        //         // 🔎 Pega o status
        //         HiddenField hdfStatus = (HiddenField)e.Item.FindControl("hdfStatus");

        //         if (hdfStatus != null &&
        //             hdfStatus.Value.Equals("Concluido", StringComparison.OrdinalIgnoreCase))
        //         {
        //             // 🔘 Botões
        //             Button btnAtualizar = (Button)e.Item.FindControl("btnAtualizarColeta");
        //             Button btnPedagioManual = (Button)e.Item.FindControl("btnPedagadioManual");
        //             Button btnWhats = (Button)e.Item.FindControl("WhatsApp");
        //             Button btnOrdem = (Button)e.Item.FindControl("btnOrdemColeta");

        //             if (btnAtualizar != null)
        //             {
        //                 btnAtualizar.Enabled = false;
        //                 btnAtualizar.CssClass += " disabled";
        //             }

        //             if (btnWhats != null)
        //             {
        //                 btnWhats.Enabled = false;
        //                 btnWhats.CssClass += " disabled";
        //             }

        //             if (btnOrdem != null)
        //             {
        //                 btnOrdem.Enabled = false;
        //                 btnOrdem.CssClass += " disabled";
        //             }
        //         }
        //     }

        //     if (e.Item.ItemType == ListItemType.Item ||
        // e.Item.ItemType == ListItemType.AlternatingItem)
        //     {
        //         DropDownList ddlRotaKrona = (DropDownList)e.Item.FindControl("ddlRotaKrona");
        //         string carga = DataBinder.Eval(e.Item.DataItem, "carga").ToString();
        //         using (SqlConnection conn = new SqlConnection(
        //             ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
        //         {
        //             string sqlr = @"SELECT id_rota, descricao_rota 
        //                    FROM tbrotaskrona 
        //                    ORDER BY descricao_rota";

        //             using (SqlCommand cmd = new SqlCommand(sqlr, conn))
        //             {
        //                 conn.Open();
        //                 ddlRotaKrona.DataSource = cmd.ExecuteReader();
        //                 ddlRotaKrona.DataTextField = "descricao_rota";
        //                 ddlRotaKrona.DataValueField = "id_rota";
        //                 ddlRotaKrona.DataBind();
        //             }
        //         }

        //         string sqlg = "select rota_krona from tbcargas where carga=" + carga + " and  rota_krona is not null";
        //         System.Data.DataTable dt = new System.Data.DataTable();
        //         SqlDataAdapter adp = new SqlDataAdapter(sqlg, con);
        //         con.Open();
        //         adp.Fill(dt);
        //         con.Close();




        //         if (dt.Rows.Count > 0)
        //         {
        //             string texto = dt.Rows[0][0].ToString();


        //             //ddlRotaKrona.Items.Insert(0, new System.Web.UI.WebControls.ListItem(texto, codigo));

        //             ddlRotaKrona.SelectedItem.Text = texto;
        //         }
        //         else
        //         {
        //             ddlRotaKrona.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Selecione a rota --", ""));
        //         }

        //     }

        //     if (e.Item.ItemType == ListItemType.Item ||
        //e.Item.ItemType == ListItemType.AlternatingItem)
        //     {
        //         string carga = DataBinder.Eval(e.Item.DataItem, "carga").ToString();
        //         DropDownList ddlPercurso = (DropDownList)e.Item.FindControl("ddlPercurso");


        //         string sqlg = "select percurso from tbcargas where carga=" + carga + " and percurso is not null";
        //         System.Data.DataTable dt = new System.Data.DataTable();
        //         SqlDataAdapter adp = new SqlDataAdapter(sqlg, con);
        //         con.Open();
        //         adp.Fill(dt);
        //         con.Close();




        //         if (dt.Rows.Count > 0)
        //         {
        //             string texto = dt.Rows[0][0].ToString();


        //             //ddlRotaKrona.Items.Insert(0, new System.Web.UI.WebControls.ListItem(texto, codigo));

        //             ddlPercurso.SelectedItem.Text = texto;
        //         }


        //     }

        //     if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        //     {
        //         string idViagem = DataBinder.Eval(e.Item.DataItem, "carga").ToString();
        //         GridView gv = (GridView)e.Item.FindControl("gvCte");
        //         int index = e.Item.ItemIndex;

        //         if (gv != null)
        //         {
        //             CarregarGridCte(gv, idViagem, index);
        //         }
        //     }

        //     if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        //     {
        //         string idCarga = DataBinder.Eval(e.Item.DataItem, "carga").ToString();
        //         GridView gvn = (GridView)e.Item.FindControl("gvNF");
        //         int index = e.Item.ItemIndex;

        //         if (gvn != null)
        //         {
        //             CarregarNF(idCarga, gvn);
        //         }
        //     }
        //     if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        //     {
        //         GridView gv = (GridView)e.Item.FindControl("gvNF");
        //         gv.RowCommand += gvNF_RowCommand;
        //     }

        //     if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        //     {
        //         // Como no HTML está <asp:Button>, fazemos o Cast para Button aqui
        //         Button btnGeraDoc = (Button)e.Item.FindControl("btnGeraDoc");

        //         if (btnGeraDoc != null)
        //         {
        //             // Força o ScriptManager a tratar este Button específico como PostBack total (ignora o AJAX)
        //             ScriptManager.GetCurrent(this).RegisterPostBackControl(btnGeraDoc);
        //         }
        //     }

        // }

        protected void rptColetas_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item &&
                e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            InicializarLabels(e.Item);

            CarregarStatus(e.Item);
            CarregarEstabelecimentosCVA(e.Item);
            CarregarEmpresas(e.Item);
            CarregarVeiculos(e.Item);
            ProcessarCVA(e.Item);
            AtualizarAtendimento(e.Item);
            CarregarPedidos(e.Item);
            DesabilitarBotoes(e.Item);
            CarregarRotaKrona(e.Item);
            CarregarPercurso(e.Item);
            CarregarGridCTe(e.Item);
            CarregarGridNF(e.Item);
            //RegistrarEventos(e.Item);

            string numSolicitacao = DataBinder.Eval(e.Item.DataItem, "carga").ToString();

            CarregarDadosSolicitacao(e.Item, numSolicitacao);


        }
        private void CarregarPercurso(RepeaterItem item)
        {
            DropDownList ddlPercurso = (DropDownList)item.FindControl("ddlPercurso");

            if (ddlPercurso == null)
                return;

            string carga = DataBinder.Eval(item.DataItem, "carga").ToString();

            using (SqlConnection conn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                string sql = @"SELECT percurso
                       FROM tbcargas
                       WHERE carga = @carga
                         AND percurso IS NOT NULL";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@carga", carga);

                    conn.Open();

                    object percurso = cmd.ExecuteScalar();

                    if (percurso != null)
                    {
                        System.Web.UI.WebControls.ListItem li = ddlPercurso.Items.FindByText(percurso.ToString());

                        if (li != null)
                        {
                            ddlPercurso.ClearSelection();
                            li.Selected = true;
                        }
                    }
                }
            }
        }
        private void CarregarRotaKrona(RepeaterItem item)
        {
            //DropDownList ddl = (DropDownList)item.FindControl("ddlRotaKrona");

            //if (ddl == null)
            //    return;

            //string carga = DataBinder.Eval(item.DataItem, "carga").ToString();

            //using (SqlConnection conn = new SqlConnection(
            //    ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            //{
            //    conn.Open();

            //    // Carrega a lista
            //    SqlDataAdapter da = new SqlDataAdapter(
            //        "SELECT id_rota, descricao_rota FROM tbrotaskrona ORDER BY descricao_rota", conn);

            //    DataTable dt = new DataTable();
            //    da.Fill(dt);

            //    ddl.DataSource = dt;
            //    ddl.DataTextField = "descricao_rota";
            //    ddl.DataValueField = "id_rota";
            //    ddl.DataBind();

            //    ddl.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Selecione --", ""));

            //    // Recupera a rota da carga
            //    SqlCommand cmd = new SqlCommand(
            //        "SELECT rota_krona FROM tbcargas WHERE carga=@carga", conn);

            //    cmd.Parameters.AddWithValue("@carga", carga);

            //    object rota = cmd.ExecuteScalar();

            //    if (rota != null && rota != DBNull.Value)
            //    {
            //        ddl.SelectedItem.Text = rota.ToString();
            //    }
            //}

            DropDownList ddl = (DropDownList)item.FindControl("ddlRotaKrona");

            if (ddl == null)
                return;

            string carga = Convert.ToString(DataBinder.Eval(item.DataItem, "carga"));

            using (SqlConnection conn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                conn.Open();

                // Carrega as rotas
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT id_rota, descricao_rota FROM tbrotaskrona ORDER BY descricao_rota", conn);

                DataTable dt = new DataTable();
                da.Fill(dt);

                ddl.DataSource = dt;
                ddl.DataTextField = "descricao_rota";
                ddl.DataValueField = "id_rota";
                ddl.DataBind();

                ddl.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Selecione a rota --", ""));

                // Recupera a rota da carga
                SqlCommand cmd = new SqlCommand(
                    "SELECT rota_krona FROM tbcargas WHERE carga = @carga", conn);

                cmd.Parameters.AddWithValue("@carga", carga);

                object rota = cmd.ExecuteScalar();

                if (rota != null && rota != DBNull.Value)
                {
                    string rotaAtual = rota.ToString();

                    // Primeiro tenta pelo Value (id_rota)
                    System.Web.UI.WebControls.ListItem itemSelecionado = ddl.Items.FindByValue(rotaAtual);

                    // Se não encontrar, tenta pelo Text (descricao_rota)
                    if (itemSelecionado == null)
                        itemSelecionado = ddl.Items.FindByText(rotaAtual);

                    if (itemSelecionado != null)
                    {
                        ddl.ClearSelection();
                        itemSelecionado.Selected = true;
                    }
                }
            }




        }
        private void CarregarGridCTe(RepeaterItem item)
        {
            string idViagem = DataBinder.Eval(item.DataItem, "carga").ToString();
            GridView gv = (GridView)item.FindControl("gvCte");
            int index = item.ItemIndex;

            if (gv != null)
            {
                CarregarGridCte(gv, idViagem, index);
            }
        }
        private void CarregarGridNF(RepeaterItem item)
        {
            HiddenField hdIdCarga = item.FindControl("hdIdCarga") as HiddenField;
            GridView gvNF = item.FindControl("gvNF") as GridView;

            if (hdIdCarga == null || gvNF == null)
                return;

            if (int.TryParse(hdIdCarga.Value, out int idCarga))
            {
                CarregarNotas(idCarga, gvNF);
            }



        }
        private void InicializarLabels(RepeaterItem item)
        {
            Label lbl;

            lbl = item.FindControl("lblPeso") as Label;
            if (lbl != null) lbl.Text = "0,000";

            lbl = item.FindControl("lblPesoCTe") as Label;
            if (lbl != null) lbl.Text = "0,000";

            lbl = item.FindControl("lblPesoCarregadoCTe") as Label;
            if (lbl != null) lbl.Text = "0,00";

            lbl = item.FindControl("lblValorMercCTe") as Label;
            if (lbl != null) lbl.Text = "0,00";

            lbl = item.FindControl("lblTotal") as Label;
            if (lbl != null) lbl.Text = "0,00";

            lbl = item.FindControl("lblCBS") as Label;
            if (lbl != null) lbl.Text = "0,00";

            lbl = item.FindControl("lblIBS") as Label;
            if (lbl != null) lbl.Text = "0,00";

            lbl = item.FindControl("lblICMS") as Label;
            if (lbl != null) lbl.Text = "0,00";

            lbl = item.FindControl("lblTotalFrete") as Label;
            if (lbl != null) lbl.Text = "0,00";
        }
        private void CarregarStatus(RepeaterItem item)
        {
            DropDownList ddlStatus = item.FindControl("ddlStatus") as DropDownList;

            if (ddlStatus == null)
                return;

            const string sql = @"SELECT cod_status,ds_status
                         FROM tb_status
                         ORDER BY ds_status";

            using (SqlConnection conn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                conn.Open();

                ddlStatus.DataSource = cmd.ExecuteReader();
                ddlStatus.DataTextField = "ds_status";
                ddlStatus.DataValueField = "ds_status";
                ddlStatus.DataBind();
            }

            HiddenField hdfStatus = item.FindControl("hdfStatus") as HiddenField;

            if (hdfStatus != null)
                ddlStatus.Items.Insert(0, new System.Web.UI.WebControls.ListItem(hdfStatus.Value, "0"));
        }
        private void CarregarEstabelecimentosCVA(RepeaterItem item)
        {
            DropDownList ddl = item.FindControl("ddlEstabelecimentoCVA") as DropDownList;

            if (ddl == null)
                return;

            const string sql = @"select cod_estabelecimento,
                                nom_estabelecimento
                         from tbestabelecimentos
                         where fl_exclusao is null
                         and sit_estabelecimento='ATIVO'
                         order by nom_estabelecimento";

            using (SqlConnection conn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                conn.Open();

                ddl.DataSource = cmd.ExecuteReader();
                ddl.DataTextField = "nom_estabelecimento";
                ddl.DataValueField = "cod_estabelecimento";
                ddl.DataBind();
            }
        }
        private void CarregarEmpresas(RepeaterItem item)
        {
            DropDownList ddl = item.FindControl("ddlEmpresa") as DropDownList;

            if (ddl == null)
                return;

            const string sql = @"SELECT codigo,descricao
                         FROM tbempresa
                         ORDER BY descricao";

            using (SqlConnection conn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                conn.Open();

                ddl.DataSource = cmd.ExecuteReader();
                ddl.DataTextField = "descricao";
                ddl.DataValueField = "codigo";
                ddl.DataBind();
            }

            ddl.Items.Insert(0,
                new System.Web.UI.WebControls.ListItem("Selecione a FILIAL", "0"));
        }
        private void CarregarVeiculos(RepeaterItem item)
        {
            DropDownList ddl = item.FindControl("ddlVeiculoCobrado") as DropDownList;

            if (ddl == null)
                return;

            const string sql = @"
            SELECT DISTINCT RTRIM(LTRIM(descricao_tng)) descricao_tng
            FROM tbtipoveic
            WHERE descricao_tng IS NOT NULL
            ORDER BY descricao_tng";

            using (SqlConnection conn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                conn.Open();

                ddl.DataSource = cmd.ExecuteReader();
                ddl.DataTextField = "descricao_tng";
                ddl.DataValueField = "descricao_tng";
                ddl.DataBind();
            }

            ddl.Items.Insert(0,
                new System.Web.UI.WebControls.ListItem("Selecione o VEICULO", "0"));
        }
        private void ProcessarCVA(RepeaterItem item)
        {
            TextBox txtSitCVA = item.FindControl("txtSitCVA") as TextBox;
            TextBox txtTipoViagemCVA = item.FindControl("txtTipoViagemCVA") as TextBox;
            TextBox txtNumCVA = item.FindControl("txtNumCVA") as TextBox;
            DropDownList ddlDevolucaoPecaCVA = (DropDownList)item.FindControl("ddlDevolucaoPecaCVA");
            DropDownList ddlExpedidorCVA = (DropDownList)item.FindControl("ddlExpedidorCVA");
            DropDownList ddlRecCVA = (DropDownList)item.FindControl("ddlRecCVA");
            TextBox txtCodigoRemetenteCVA = item.FindControl("txtCodigoRemetenteCVA") as TextBox;
            TextBox txtRemetenteCVA = item.FindControl("txtRemetenteCVA") as TextBox;
            TextBox txtCodigoExpedidorCVA = item.FindControl("txtCodigoExpedidorCVA") as TextBox;
            TextBox txtCodigoDestCVA = item.FindControl("txtCodigoDestCVA") as TextBox;
            TextBox txtDestCVA = item.FindControl("txtDestCVA") as TextBox;
            TextBox txtCodigoRecCVA = item.FindControl("txtCodigoRecCVA") as TextBox;
            TextBox txtLocalColetaCVA = item.FindControl("txtLocalColetaCVA") as TextBox;
            TextBox txtUFColetaCVA = item.FindControl("txtUFColetaCVA") as TextBox;
            TextBox txtLocalEntregaCVA = item.FindControl("txtLocalEntregaCVA") as TextBox;
            TextBox txtUFEntregaCVA = item.FindControl("txtUFEntregaCVA") as TextBox;
            TextBox txtDataHoraColetaCVA = item.FindControl("txtDataHoraColetaCVA") as TextBox;
            TextBox txtDataHoraEntregaCVA = item.FindControl("txtDataHoraEntregaCVA") as TextBox;
            TextBox txtTipoSolicitacaoCVA = item.FindControl("txtTipoSolicitacaoCVA") as TextBox;
            TextBox txtTipoVeiculoCVA = item.FindControl("txtTipoVeiculoCVA") as TextBox;
            TextBox txtJustificaVeiculoCVA = item.FindControl("txtJustificaVeiculoCVA") as TextBox;
            TextBox txtPlacaCVA = item.FindControl("txtPlacaCVA") as TextBox;
            TextBox txtVeiculoCVA = item.FindControl("txtVeiculoCVA") as TextBox;
            TextBox txtCapVeiculoCVA = item.FindControl("txtCapVeiculoCVA") as TextBox;
            TextBox txtProprietarioVeiculoCVA = item.FindControl("txtProprietarioVeiculoCVA") as TextBox;
            TextBox txtReboque1CVA = item.FindControl("txtReboque1CVA") as TextBox;
            TextBox txtReboque2CVA = item.FindControl("txtReboque2CVA") as TextBox;
            TextBox txtPropReb1 = item.FindControl("txtPropReb1") as TextBox;
            TextBox txtPropReb2 = item.FindControl("txtPropReb2") as TextBox;
            TextBox txtMotoristaCVA = item.FindControl("txtMotoristaCVA") as TextBox;
            TextBox txtCPFCVA = item.FindControl("txtCPFCVA") as TextBox;
            TextBox txtRGCVA = item.FindControl("txtRGCVA") as TextBox;
            TextBox txtTranspMotoristaCVA = item.FindControl("txtTranspMotoristaCVA") as TextBox;
            if (txtSitCVA == null)
                return;
            string numSolicitacao = Convert.ToString(DataBinder.Eval(item.DataItem, "carga"));
            string cva = Convert.ToString(DataBinder.Eval(item.DataItem, "cva"));
            string codigoMotorista = txtCodMotorista.Text.Trim();
            string placaVeiculo = txtPlaca.Text.Trim();
            string reboque1 = txtReboque1.Text.Trim();
            string reboque2 = txtReboque2.Text.Trim();
            txtMotoristaCVA.Text = codigoMotorista;
            txtPlacaCVA.Text = placaVeiculo;
            txtReboque1CVA.Text = reboque1;
            txtReboque2CVA.Text = reboque2;
            if (String.IsNullOrWhiteSpace(cva))
            {
                txtSitCVA.Text = "EM LANCAMENTO";

                CarregarTipoViagemCVA(
                    item,
                    numSolicitacao,
                    txtTipoViagemCVA);
            }
            else
            {
                txtSitCVA.Text = "CADASTRADO";

                if (txtNumCVA != null)
                    txtNumCVA.Text = cva;
            }

            // Pesquisar dados do motorista
            using (SqlConnection con = new SqlConnection(
               ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                con.Open();
                string sqlMotorista = @"SELECT *
                FROM tbmotoristas
                WHERE codmot = @cod";

                using (SqlCommand cmdMotorista = new SqlCommand(sqlMotorista, con))
                {
                    cmdMotorista.Parameters.AddWithValue("@cod", codigoMotorista);

                    using (SqlDataReader drMotorista = cmdMotorista.ExecuteReader())
                    {
                        if (drMotorista.Read())
                        {
                            if (txtMotoristaCVA != null)
                            {
                                txtMotoristaCVA.Text = drMotorista["codmot"].ToString() +
                                                         " - " +
                                                       drMotorista["nommot"].ToString();
                                txtTranspMotoristaCVA.Text = drMotorista["codtra"].ToString() +
                                                         " - " +
                                                       drMotorista["transp"].ToString();
                                txtCPFCVA.Text = drMotorista["cpf"].ToString();
                                txtRGCVA.Text = drMotorista["numrg"].ToString();
                            }
                        }
                        else
                        {
                            if (txtMotoristaCVA != null)
                                txtMotoristaCVA.Text = "";

                            MostrarMsgSolicitacao(
                                item,
                                $"Motorista '{codigoMotorista}' não cadastrado na tabela tbmotoristas.",
                                "warning");
                        }
                    }
                }
                //Pesquisar dados do veiculo
                string sqlPlaca = @"SELECT *
                FROM tbveiculos
                WHERE plavei = @placa";
                using (SqlCommand cmdPlaca = new SqlCommand(sqlPlaca, con))
                {
                    cmdPlaca.Parameters.AddWithValue("@placa", placaVeiculo);

                    using (SqlDataReader drPlaca = cmdPlaca.ExecuteReader())
                    {
                        if (drPlaca.Read())
                        {
                            if (txtPlacaCVA != null)
                            {
                                txtPlacaCVA.Text = drPlaca["codvei"].ToString() +
                                                         " - " +
                                                     drPlaca["plavei"].ToString();

                                txtVeiculoCVA.Text = drPlaca["tipvei"].ToString();

                                txtCapVeiculoCVA.Text = drPlaca["pbt"].ToString();

                                txtProprietarioVeiculoCVA.Text = drPlaca["codtra"].ToString() +
                                                                 " - " +
                                                                 drPlaca["transp"].ToString();
                            }
                        }
                        else
                        {
                            if (txtMotoristaCVA != null)
                                txtMotoristaCVA.Text = "";

                            MostrarMsgSolicitacao(
                                item,
                                $"Veiculo '{placaVeiculo}' não cadastrado na tabela tbveiculos.",
                                "warning");
                        }
                    }
                }

                //Pesquisar dados das carretas
                if (reboque1 != null && reboque1 != "")
                {
                    string sqlReboque1 = @"SELECT *
                    FROM tbcarretas
                    WHERE placacarreta = @placaReboque1";
                    using (SqlCommand cmdReboque1 = new SqlCommand(sqlReboque1, con))
                    {
                        cmdReboque1.Parameters.AddWithValue("@placaReboque1", reboque1);
                        using (SqlDataReader drReboque1 = cmdReboque1.ExecuteReader())
                        {
                            if (drReboque1.Read())
                            {
                                if (txtReboque1CVA != null)
                                {
                                    txtReboque1CVA.Text = drReboque1["codcarreta"].ToString() +
                                                       " - " +
                                                       drReboque1["placacarreta"].ToString();
                                    txtPropReb1.Text = drReboque1["codprop"].ToString() +
                                                       " - " +
                                                       drReboque1["descprop"].ToString();
                                }
                            }
                            else
                            {
                                if (txtReboque1CVA != null)
                                    txtReboque1CVA.Text = "";

                                MostrarMsgSolicitacao(
                                    item,
                                    $"Carreta1 '{reboque1}' não cadastrada na tabela tbcarretas.",
                                    "warning");
                            }
                        }
                    }
                }

                if (reboque2 != null && reboque2 != "")
                {
                    string sqlReboque2 = @"SELECT *
                    FROM tbcarretas
                    WHERE placacarreta = @placaReboque2";
                    using (SqlCommand cmdReboque2 = new SqlCommand(sqlReboque2, con))
                    {
                        cmdReboque2.Parameters.AddWithValue("@placaReboque2", reboque2);
                        using (SqlDataReader drReboque2 = cmdReboque2.ExecuteReader())
                        {
                            if (drReboque2.Read())
                            {
                                if (txtReboque2CVA != null)
                                {
                                    txtReboque2CVA.Text = drReboque2["codcarreta"].ToString() +
                                                       " - " +
                                                       drReboque2["placacarreta"].ToString();
                                    txtPropReb2.Text = drReboque2["codprop"].ToString() +
                                                       " - " +
                                                       drReboque2["descprop"].ToString();
                                }
                            }
                            else
                            {
                                if (txtReboque2CVA != null)
                                    txtReboque2CVA.Text = "";

                                MostrarMsgSolicitacao(
                                    item,
                                    $"Carreta2 '{reboque2}' não cadastrada na tabela tbcarretas.",
                                    "warning");
                            }
                        }
                    }
                }




            }
        }
        private void CarregarDadosSolicitacao(RepeaterItem item, string numSolicitacao)
        {
            using (SqlConnection con = new SqlConnection(
                ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                con.Open();

                string sql = @"SELECT *
                       FROM tbsolicitacoes_r1
                       WHERE r1_sol_numero = @num";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@num", numSolicitacao);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (!dr.Read())
                        {
                            MostrarMsgSolicitacao(
                                item,
                                "Solicitação não encontrada.",
                                "danger");
                            return;
                        }

                        // A partir daqui você chama métodos específicos
                        CarregarTipoViagem(item, dr);
                        //CarregarTipoGeracao(item, dr);
                        //CarregarOrigem(item, dr);
                        //CarregarDestino(item, dr);
                        //CarregarCliente(item, dr);
                        //CarregarTransportador(item, dr);
                        // ...
                    }
                }
            }
        }
        private void CarregarTipoViagem(RepeaterItem item, SqlDataReader dr)
        {
            TextBox txtTipoViagemCVA =
                (TextBox)item.FindControl("txtTipoViagemCVA");

            string tipo = dr["r1_sol_tipo"].ToString();
            string tipoVeiculo = dr["r1_sol_tipo_veiculo"].ToString();


            using (SqlConnection con = new SqlConnection(
                ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                con.Open();

                string sql = @"SELECT codvw, descricao
                       FROM tbtiposolicitacao
                       WHERE codvw = @cod";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@cod", tipo);

                    using (SqlDataReader drTipo = cmd.ExecuteReader())
                    {
                        if (drTipo.Read())
                        {
                            txtTipoViagemCVA.Text =
                                drTipo["codvw"] + " - " + drTipo["descricao"];
                        }
                        else
                        {
                            MostrarMsgSolicitacao(
                                item,
                                "Tipo de viagem não cadastrado.",
                                "warning");
                        }
                    }
                }
            }

            // trata o tipo de geracao
            TextBox txtTipoGeracaoCVA = (TextBox)item.FindControl("txtTipoGeracaoCVA");
            TextBox txtContaCVA = item.FindControl("txtContaCVA") as TextBox;
            TextBox txtCentroCustoCVA = item.FindControl("txtCentroCustoCVA") as TextBox;
            TextBox txtComRetornoCVA = item.FindControl("txtComRetornoCVA") as TextBox;
            TextBox txtDataSolicitacaoCVA = item.FindControl("txtDataSolicitacaoCVA") as TextBox;
            TextBox txtDataHoraColetaCVA = item.FindControl("txtDataHoraColetaCVA") as TextBox;
            TextBox txtTipoSolicitacaoCVA = item.FindControl("txtTipoSolicitacaoCVA") as TextBox;
            TextBox txtTipoVeiculoCVA = item.FindControl("txtTipoVeiculoCVA") as TextBox;
            string tipoGeracao = dr["r1_sol_tipo_geracao"].ToString();
            if (tipoGeracao == "7" || tipoGeracao == "12")
            {
                txtComRetornoCVA.Text = "SIM";
            }
            else
            {
                txtComRetornoCVA.Text = "NAO";
            }
            txtContaCVA.Text = dr["r1_sol_conta"].ToString();
            txtCentroCustoCVA.Text = dr["r1_sol_centro_custo"].ToString();
            txtDataSolicitacaoCVA.Text = Convert.ToDateTime(dr["r1_sol_data_cadastro"]).ToString("dd/MM/yyyy HH:mm");
            txtDataHoraColetaCVA.Text = Convert.ToDateTime(dr["r1_sol_data_hora_coleta"]).ToString("dd/MM/yyyy HH:mm");

            using (SqlConnection con = new SqlConnection(
                ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                con.Open();

                string sqlGeracao = @"SELECT codvw, descricao
                       FROM tbtipogeracaosolicitacao
                       WHERE codvw = @cod";

                using (SqlCommand cmdGeracao = new SqlCommand(sqlGeracao, con))
                {
                    cmdGeracao.Parameters.AddWithValue("@cod", tipoGeracao);

                    using (SqlDataReader drTipoGeracao = cmdGeracao.ExecuteReader())
                    {
                        if (drTipoGeracao.Read())
                        {
                            txtTipoGeracaoCVA.Text =
                                drTipoGeracao["codvw"] + " - " + drTipoGeracao["descricao"];
                        }
                        else
                        {
                            MostrarMsgSolicitacao(
                                item,
                                "Tipo de viagem não cadastrado.",
                                "warning");
                        }
                    }
                }

                string sqlSolicitacao = @"SELECT codvw, descricao
                       FROM tbtiposolicitacao
                       WHERE codvw = @cod";

                using (SqlCommand cmdSolicitacao = new SqlCommand(sqlSolicitacao, con))
                {
                    cmdSolicitacao.Parameters.AddWithValue("@cod", tipo);

                    using (SqlDataReader drSolicitacao = cmdSolicitacao.ExecuteReader())
                    {
                        if (drSolicitacao.Read())
                        {
                            txtTipoSolicitacaoCVA.Text = drSolicitacao["descricao"].ToString();
                        }
                        else
                        {
                            MostrarMsgSolicitacao(
                                item,
                                "Tipo de viagem não cadastrado.",
                                "warning");
                        }
                    }
                }
                // Procura o Tipo de Veiculo
                string sqlVeiculo = @"SELECT codvw, descricao
                FROM tbtipoveic
                WHERE codvw = @cod";

                using (SqlCommand cmdVeiculo = new SqlCommand(sqlVeiculo, con))
                {
                    cmdVeiculo.Parameters.AddWithValue("@cod", tipoVeiculo);

                    using (SqlDataReader drVeiculo = cmdVeiculo.ExecuteReader())
                    {
                        if (drVeiculo.Read())
                        {
                            if (txtTipoVeiculoCVA != null)
                            {
                                txtTipoVeiculoCVA.Text = drVeiculo["codvw"].ToString() +
                                                         " - " +
                                                         drVeiculo["descricao"].ToString();
                            }
                        }
                        else
                        {
                            if (txtTipoVeiculoCVA != null)
                                txtTipoVeiculoCVA.Text = "";

                            MostrarMsgSolicitacao(
                                item,
                                $"Tipo de veículo '{tipoVeiculo}' não cadastrado na tabela tbtipoveic.",
                                "warning");
                        }
                    }
                }
            }
        }
        private void CarregarTipoViagemCVA(RepeaterItem item, string numSolicitacao, TextBox txtTipoViagemCVA)
        {
            // Localiza o TextBox do Tipo de Geração
            TextBox txtTipoGeracaoCVA = (TextBox)item.FindControl("txtTipoGeracaoCVA");
            using (SqlConnection con = new SqlConnection(
                ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                con.Open();

                // Procura a solicitação e obtém o tipo
                string sql = @"SELECT r1_sol_tipo
                       FROM tbsolicitacoes_r1
                       WHERE r1_sol_numero = @num";

                string tipo = null;

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@num", numSolicitacao);

                    object obj = cmd.ExecuteScalar();

                    if (obj != null && obj != DBNull.Value)
                        tipo = obj.ToString();
                }

                if (string.IsNullOrWhiteSpace(tipo))
                {
                    MostrarMsgSolicitacao(
                        item,
                        "Solicitação não encontrada na tabela tbsolicitacoes_r1.",
                        "danger");

                    txtTipoViagemCVA.Text = "";
                    //txtTipoSolicitacaoCVA.Text = "";
                    if (txtTipoGeracaoCVA != null)
                        txtTipoGeracaoCVA.Text = "";

                    return;
                }
            }
        }
        private void CarregarCombos()
        {
            dtStatus = BuscarTabela(
                "SELECT cod_status, ds_status FROM tb_status ORDER BY ds_status");

            dtEmpresas = BuscarTabela(
                "SELECT codigo, descricao FROM tbempresa ORDER BY descricao");

            dtEstabelecimentos = BuscarTabela(
                @"SELECT cod_estabelecimento,
                 nom_estabelecimento
              FROM tbestabelecimentos
              WHERE fl_exclusao IS NULL
              AND sit_estabelecimento='ATIVO'
              ORDER BY nom_estabelecimento");

            dtVeiculos = BuscarTabela(
                @"SELECT DISTINCT descricao_tng
              FROM tbtipoveic
              WHERE descricao_tng IS NOT NULL
              ORDER BY descricao_tng");
        }
        private DataTable BuscarTabela(string sql)
        {
            using (SqlConnection con = new SqlConnection(
                ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            using (SqlDataAdapter da = new SqlDataAdapter(sql, con))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
        private void AtualizarAtendimento(RepeaterItem item)
        {
            string previsaoStr = Convert.ToString(DataBinder.Eval(item.DataItem, "previsao"));
            string dataHoraStr = Convert.ToString(DataBinder.Eval(item.DataItem, "data_hora"));
            string status = Convert.ToString(DataBinder.Eval(item.DataItem, "status"));

            Label lblAtendimento = item.FindControl("lblAtendimento") as Label;
            HtmlTableCell tdAtendimento = item.FindControl("tdAtendimento") as HtmlTableCell;

            if (lblAtendimento == null || tdAtendimento == null)
                return;

            DateTime previsao;
            DateTime dataHora;

            if (!DateTime.TryParse(previsaoStr, out previsao) ||
                !DateTime.TryParse(dataHoraStr, out dataHora))
            {
                lblAtendimento.Text = status;
                return;
            }

            DateTime agora = DateTime.Now;

            DateTime dataHoraComparacao = new DateTime(
                previsao.Year,
                previsao.Month,
                previsao.Day,
                dataHora.Hour,
                dataHora.Minute,
                dataHora.Second);

            if (dataHoraComparacao < agora)
            {
                lblAtendimento.Text = "Atrasado";
                tdAtendimento.Attributes["style"] =
                    "background-color:red;color:white;font-weight:bold;";
            }
            else if (dataHoraComparacao.Date == agora.Date &&
                     dataHoraComparacao.TimeOfDay <= agora.TimeOfDay)
            {
                lblAtendimento.Text = "No Prazo";
                tdAtendimento.Attributes["style"] =
                    "background-color:green;color:white;font-weight:bold;";
            }
            else
            {
                lblAtendimento.Text = "Antecipado";
                tdAtendimento.Attributes["style"] =
                    "background-color:orange;color:white;font-weight:bold;";
            }
        }
        private void DesabilitarBotoes(RepeaterItem item)
        {
            HiddenField hdfStatus = item.FindControl("hdfStatus") as HiddenField;

            if (hdfStatus == null)
                return;

            if (!hdfStatus.Value.Equals("Concluido", StringComparison.OrdinalIgnoreCase))
                return;

            Button btnAtualizar = item.FindControl("btnAtualizarColeta") as Button;
            Button btnPedagioManual = item.FindControl("btnPedagadioManual") as Button;
            Button btnWhats = item.FindControl("WhatsApp") as Button;
            Button btnOrdem = item.FindControl("btnOrdemColeta") as Button;

            if (btnAtualizar != null)
            {
                btnAtualizar.Enabled = false;
                btnAtualizar.CssClass += " disabled";
            }

            if (btnPedagioManual != null)
            {
                btnPedagioManual.Enabled = false;
                btnPedagioManual.CssClass += " disabled";
            }

            if (btnWhats != null)
            {
                btnWhats.Enabled = false;
                btnWhats.CssClass += " disabled";
            }

            if (btnOrdem != null)
            {
                btnOrdem.Enabled = false;
                btnOrdem.CssClass += " disabled";
            }
        }
        private void AtualizarResumoCarga(ControlesCarga c)
        {
            decimal pesoTotal = 0;
            decimal valorTotal = 0;

            using (SqlConnection conn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                string sql = @"
            SELECT
                ISNULL(SUM(peso_documento),0) AS PesoTotal,
                ISNULL(SUM(valor_documento),0) AS ValorTotal
            FROM tbcte
            WHERE id_viagem = @carga";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@carga", Convert.ToInt32(c.Carga));

                    conn.Open();

                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        pesoTotal = Convert.ToDecimal(dr["PesoTotal"]);
                        valorTotal = Convert.ToDecimal(dr["ValorTotal"]);
                    }
                }
            }
            // Atualiza tbcte
            AtualizarCTe(Convert.ToInt32(c.Carga), pesoTotal, valorTotal);

            // Atualiza a interface
            c.lblPesoCarregadoCTe.Text = pesoTotal.ToString("N3");
            c.LblValorMercCTe.Text = valorTotal.ToString("N2");
        }
        private void AtualizarCTe(int idCarga, decimal peso, decimal valor)
        {
            using (SqlConnection conn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                conn.Open();

                string sql = @"
                IF EXISTS (SELECT 1 FROM tbcte WHERE id_viagem = @carga)
                BEGIN
                    UPDATE tbcte
                    SET peso_documento = @peso,
                        valor_documento = @valor
                    WHERE id_viagem = @carga
                END
                ELSE
                BEGIN
                    INSERT INTO tbcte (id_viagem, peso_documento, valor_documento)
                    VALUES (@carga, @peso, @valor)
                END";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    //SqlHelper.Int(cmd, "@carga", idCarga);
                    //SqlHelper.Decimal(cmd, "@peso", 18, 3, peso);
                    //SqlHelper.Decimal(cmd, "@valor", 18, 2, valor);
                    cmd.Parameters.AddWithValue("@carga", idCarga);
                    cmd.Parameters.AddWithValue("@peso", peso);
                    cmd.Parameters.AddWithValue("@valor", valor);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        private (decimal peso, decimal valor) CalcularTotais(int idCarga)
        {
            decimal peso = 0;
            decimal valor = 0;

            using (SqlConnection conn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                string sql = @"
            SELECT 
                ISNULL(SUM(peso_documento),0) AS PesoTotal,
                ISNULL(SUM(valor_documento),0) AS ValorTotal
            FROM tbcte
            WHERE id_viagem = @carga";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@carga", idCarga);

                    conn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            peso = Convert.ToDecimal(dr["PesoTotal"]);
                            valor = Convert.ToDecimal(dr["ValorTotal"]);
                        }
                    }
                }
            }

            return (peso, valor);
        }

        private void CarregarNotas(int idCarga, GridView gvNF)
        {
            using (SqlConnection conn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                string sql = @"
                SELECT
                    idnfe,
                    chavenfe,
                    numeronfe,
                    serienfe,
                    peso,
                    vnf,
                    carga
                FROM tbnfe
                WHERE carga = @carga
                ORDER BY numeronfe";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@carga", idCarga);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        gvNF.DataSource = dt;
                        gvNF.DataBind();
                    }
                }
            }
        }
        private void CarregarPedidos(int idCarga, GridView gv)
        {
            using (SqlConnection conn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                string sql = @"SELECT pedido, emissao, peso, material, portao,
                              iniciocar, termcar, duracao, motcar
                       FROM tbpedidos
                       WHERE carga = @idCarga";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@idCarga", SqlDbType.Int).Value = idCarga;
                    conn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        gv.DataSource = dr;
                        gv.DataBind();
                    }

                }
            }
        }
        private void CarregarPedidos(RepeaterItem item)
        {
            HiddenField hdIdCarga = item.FindControl("hdIdCarga") as HiddenField;
            GridView gvPedidos = item.FindControl("gvPedidos") as GridView;

            if (hdIdCarga == null || gvPedidos == null)
                return;

            if (int.TryParse(hdIdCarga.Value, out int idCarga))
            {
                CarregarPedidos(idCarga, gvPedidos);
            }
        }

        private decimal? SafeDecimalValueNullable(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                return null;

            if (decimal.TryParse(valor.Trim(),
                                 NumberStyles.Number,
                                 new CultureInfo("pt-BR"),
                                 out decimal resultado))
            {
                return resultado;
            }

            return null;
        }
        private DateTime? SafeDateValue(string valor)
        {
            if (DateTime.TryParse(valor, out DateTime data))
                return data;

            return null;
        }
        private object SafeDecimalValue(string valor)
        {
            if (!string.IsNullOrWhiteSpace(valor) &&
                decimal.TryParse(valor, NumberStyles.Number,
                    new CultureInfo("pt-BR"), out decimal resultado))
            {
                return resultado;
            }

            return DBNull.Value;
        }
        // protected void rptColetas_ItemCommand(object source, RepeaterCommandEventArgs e)
        // {
        //     GridView gv = (GridView)e.Item.FindControl("gvPedidos");

        //     if (e.CommandName == "Atualizar")
        //     {
        //         //genildo
        //         string carga = e.CommandArgument.ToString();
        //         //GridView gv = (GridView)e.Item.FindControl("gvPedidos");

        //         // Recuperar os controles de dentro do item
        //         TextBox txtDataHoraColeta = (TextBox)e.Item.FindControl("txtDataHoraColeta");
        //         TextBox txtVeiculoDisponivel = (TextBox)e.Item.FindControl("txtVeiculoDisponivel");
        //         TextBox txtGateOrigem = (TextBox)e.Item.FindControl("txtGateOrigem");
        //         TextBox txtPrevisaoChegada = (TextBox)e.Item.FindControl("txtPrevisaoChegada");
        //         TextBox txtGateDestino = (TextBox)e.Item.FindControl("txtGateDestino");
        //         TextBox txtCVA = (TextBox)e.Item.FindControl("txtCVA");
        //         DropDownList ddlStatus = (DropDownList)e.Item.FindControl("ddlStatus");
        //         TextBox txtSaidaOrigem = (TextBox)e.Item.FindControl("txtSaidaOrigem");
        //         TextBox txtAgCarreg = (TextBox)e.Item.FindControl("txtAgCarreg");
        //         TextBox txtAgDescarga = (TextBox)e.Item.FindControl("txtAgDescarga");
        //         TextBox txtDurTransp = (TextBox)e.Item.FindControl("txtDurTransp");
        //         TextBox txtChegadaDestino = (TextBox)e.Item.FindControl("txtChegadaDestino");
        //         TextBox txtSaidaPlanta = (TextBox)e.Item.FindControl("txtSaidaPlanta");
        //         Label lblMensagem = (Label)e.Item.FindControl("lblMensagem");
        //         TextBox txtMaterial = (TextBox)e.Item.FindControl("txtMaterial");

        //         TextBox txtCodExpedidor = (TextBox)e.Item.FindControl("txtCodExpedidor");
        //         TextBox cboExpedidor = (TextBox)e.Item.FindControl("cboExpedidor");
        //         TextBox txtCidExpedidor = (TextBox)e.Item.FindControl("txtCidExpedidor");
        //         TextBox txtUFExpedidor = (TextBox)e.Item.FindControl("txtUFExpedidor");

        //         TextBox txtCodRecebedor = (TextBox)e.Item.FindControl("txtCodRecebedor");
        //         TextBox cboRecebedor = (TextBox)e.Item.FindControl("cboRecebedor");
        //         TextBox txtCidRecebedor = (TextBox)e.Item.FindControl("txtCidRecebedor");
        //         TextBox txtUFRecebedor = (TextBox)e.Item.FindControl("txtUFRecebedor");

        //         TextBox txtLocalPernoite = (TextBox)e.Item.FindControl("txtLocalPernoite");
        //         TextBox txtInicioPernoite = (TextBox)e.Item.FindControl("txtInicioPernoite");
        //         TextBox txtFimPernoite = (TextBox)e.Item.FindControl("txtFimPernoite");
        //         TextBox txtDuracaoP = (TextBox)e.Item.FindControl("txtDuracaoP");

        //         // Recuperar os TextBoxes de UF
        //         //TextBox txtUfInicio = (TextBox)e.Item.FindControl("txtUFExpedidor");
        //         //TextBox txtUfFim = (TextBox)e.Item.FindControl("txtUFRecebedor");

        //         DropDownList ddlPercursoKrona = (DropDownList)e.Item.FindControl("ddlPercurso");
        //         DropDownList ddlRotaKrona = (DropDownList)e.Item.FindControl("ddlRotaKrona");
        //         TextBox pesoKrona = (TextBox)e.Item.FindControl("txtPeso");
        //         TextBox valorKrona = (TextBox)e.Item.FindControl("txtValorTotal");
        //         DateTime? previsaoInicial = SafeDateValue(e.Item, "txtPrevisaoInicio");
        //         DateTime? previsaoFinal = SafeDateValue(e.Item, "txtPrevisaoTermino");
        //         string percursoKrona = ddlPercursoKrona.SelectedItem.Text;
        //         string rotaKrona = ddlRotaKrona.SelectedItem.Text;
        //         string id_rotaKrona = ddlRotaKrona.SelectedValue;

        //         DateTime agora = DateTime.Now;

        //         // Exemplo: atualizando no banco
        //         using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
        //         {
        //             // Extrai os valores uma única vez                    
        //             var saida = SafeDateValue2(txtSaidaOrigem.Text.Trim());
        //             var saidaPlanta = SafeDateValue2(txtSaidaPlanta.Text.Trim());
        //             var chegada = SafeDateValue2(txtChegadaDestino.Text.Trim());
        //             var disponivel_solicitacao = SafeDateValue2(txtVeiculoDisponivel.Text.Trim());
        //             var cvaOC = txtCVA.Text.Trim();

        //             var CodExpedidor = SafeDateValue2(txtCodExpedidor.Text.Trim());
        //             var Expedidor = SafeDateValue2(cboExpedidor.Text.Trim());
        //             var Cid_Expedidor = SafeDateValue2(txtCidExpedidor.Text.Trim());
        //             var UFExpedidor = SafeDateValue2(txtUFExpedidor.Text.Trim());

        //             var CodRecebedor = SafeDateValue2(txtCodRecebedor.Text.Trim());
        //             var Recebedor = SafeDateValue2(cboRecebedor.Text.Trim());
        //             var CidRecebedor = SafeDateValue2(txtCidRecebedor.Text.Trim());
        //             var UFRecebedor = SafeDateValue2(txtUFRecebedor.Text.Trim());
        //             var localPernoite = SafeDateValue2(txtLocalPernoite.Text.Trim());
        //             var inicioPernoite = SafeDateValue2(txtInicioPernoite.Text.Trim());
        //             var fimPernoite = SafeDateValue2(txtFimPernoite.Text.Trim());
        //             var duracaoP = txtDuracaoP.Text.Trim();

        //             if (chegada != null && saida != null && saidaPlanta != null)
        //             {
        //                 statusOC = "Concluido";
        //                 situacaoOC = "Pronto";
        //                 andamentoCarga = "Entregue";
        //             }
        //             else if (chegada != null && saida != null)
        //             {
        //                 statusOC = "Ag. Descarga";
        //                 situacaoOC = "EM ANDAMENTO";
        //                 andamentoCarga = "Em Andamento";
        //             }
        //             else if (chegada != null && saida != null)
        //             {
        //                 statusOC = "Em Transito";
        //                 situacaoOC = "EM ANDAMENTO";
        //                 andamentoCarga = "Em Andamento";
        //             }
        //             else if (chegada != null)
        //             {
        //                 statusOC = "Ag. Carregamento";
        //                 situacaoOC = "EM ANDAMENTO";
        //                 andamentoCarga = "Em Andamento";
        //             }
        //             else
        //             {
        //                 statusOC = "Pendente";
        //                 situacaoOC = "PROGRAMADA";
        //                 andamentoCarga = "Programada";
        //             }
        //             if (ddlStatus.SelectedItem.Text != "Pendente" || ddlStatus.SelectedItem.Text != "Concluido")
        //             {
        //                 situacaoOC = "EM ANDAMENTO";
        //                 andamentoCarga = "Em Andamento";
        //                 statusOC = ddlStatus.SelectedItem.Text.Trim();

        //             }
        //             if (ddlStatus.SelectedItem.Text == "Pendente")
        //             {
        //                 situacaoOC = "PROGRAMADA";
        //                 andamentoCarga = "Programada";
        //                 //statusOC = ddlStatus.SelectedItem.Text.Trim();

        //             }

        //             string duracao = txtDuracaoP.Text.Trim();

        //             string statusPernoite = "";

        //             if (!string.IsNullOrEmpty(duracao))
        //             {
        //                 TimeSpan tsDuracao = TimeSpan.Parse(duracao);
        //                 TimeSpan tsLimite = new TimeSpan(11, 0, 0);

        //                 if (tsDuracao < tsLimite)
        //                 {
        //                     statusPernoite = "Não Cumpriu a Jornada de 11h";
        //                 }
        //                 else
        //                 {
        //                     statusPernoite = "Cumpriu Jornada de 11h";
        //                 }
        //             }

        //             string query = @"UPDATE tbcargas SET                                  
        //                          gate_origem = @gate_origem,
        //                          gate_destino = @gate_destino,
        //                          status = @status, 
        //                          cva = @cva, 
        //                          andamento = @andamento,
        //                          saidaorigem = @saidaorigem,
        //                          tempoagcarreg = @tempoagcarreg,
        //                          chegadadestino = @chegadadestino,
        //                          saidaplanta = @saidaplanta,
        //                          prev_chegada = @prev_chegada,
        //                          tempoagdescarreg=@tempoagdescarreg,
        //                          duracao_transp=@duracao_transp,
        //                          disponivel_solicitacao = @disponivel_solicitacao,
        //                          codmot=@codmot, 
        //                          local_pernoite=@local_pernoite,
        //                          inicio_pernoite=@inicio_pernoite,
        //                          fim_pernoite=@fim_pernoite,
        //                          duracao_pernoite=@duracao_pernoite,
        //                          status_pernoite=@status_pernoite,
        //                          frota=@frota,
        //                          percurso = @percurso,
        //                          valor_total = @valor_total,
        //                          previsao_inicio_krona = @previsao_inicio_krona,
        //                          previsao_termino_krona = @previsao_termino_krona,
        //                          rota_krona = @rota_krona,
        //                          id_rota_krona = @id_rota_krona
        //                          WHERE carga = @carga";

        //             SqlCommand cmd = new SqlCommand(query, conn);
        //             cmd.Parameters.AddWithValue("@carga", carga);
        //             cmd.Parameters.AddWithValue("@status", ddlStatus.SelectedItem.Text);
        //             cmd.Parameters.AddWithValue("@andamento", andamentoCarga);
        //             cmd.Parameters.AddWithValue("@cva", txtCVA.Text.Trim());
        //             cmd.Parameters.AddWithValue("@saidaorigem", SafeDateValue(txtSaidaOrigem.Text.Trim()));
        //             cmd.Parameters.AddWithValue("@tempoagcarreg", SafeValue(txtAgCarreg.Text.Trim()));
        //             cmd.Parameters.AddWithValue("@duracao_transp", SafeValue(txtDurTransp.Text.Trim()));
        //             cmd.Parameters.AddWithValue("@tempoagdescarreg", SafeValue(txtAgDescarga.Text.Trim()));
        //             cmd.Parameters.AddWithValue("@chegadadestino", SafeDateValue(txtChegadaDestino.Text.Trim()));
        //             cmd.Parameters.AddWithValue("@disponivel_solicitacao", SafeDateValue(txtVeiculoDisponivel.Text.Trim()));
        //             cmd.Parameters.AddWithValue("@prev_chegada", SafeDateValue(txtPrevisaoChegada.Text.Trim()));
        //             cmd.Parameters.AddWithValue("@saidaplanta", SafeDateValue(txtSaidaPlanta.Text.Trim()));
        //             cmd.Parameters.AddWithValue("@codmot", txtCodMotorista.Text.Trim() ?? (object)DBNull.Value);
        //             cmd.Parameters.AddWithValue("@frota", txtCodVeiculo.Text.Trim() ?? (object)DBNull.Value);
        //             cmd.Parameters.AddWithValue("@gate_origem", SafeDateValue(txtGateOrigem.Text.Trim()) ?? (object)DBNull.Value);
        //             cmd.Parameters.AddWithValue("@gate_destino", SafeDateValue(txtGateDestino.Text.Trim()) ?? (object)DBNull.Value);

        //             cmd.Parameters.AddWithValue("@local_pernoite", txtLocalPernoite.Text.Trim().ToUpper());
        //             cmd.Parameters.AddWithValue("@inicio_pernoite", SafeDateValue(txtInicioPernoite.Text.Trim()));
        //             cmd.Parameters.AddWithValue("@fim_pernoite", SafeDateValue(txtFimPernoite.Text.Trim()));
        //             cmd.Parameters.AddWithValue("@duracao_pernoite", txtDuracaoP.Text.Trim());
        //             cmd.Parameters.AddWithValue("@status_pernoite", statusPernoite);

        //             cmd.Parameters.AddWithValue("@percurso", percursoKrona);
        //             cmd.Parameters.AddWithValue("@valor_total", SafeDecimalValue(valorKrona?.Text));
        //             cmd.Parameters.AddWithValue("@previsao_inicio_krona", (object)previsaoInicial ?? DBNull.Value);
        //             cmd.Parameters.AddWithValue("@previsao_termino_krona", (object)previsaoFinal ?? DBNull.Value);

        //             cmd.Parameters.AddWithValue("@rota_krona", rotaKrona);
        //             cmd.Parameters.AddWithValue("@id_rota_krona", id_rotaKrona);
        //             // Chama método que verifica no banco
        //             conn.Open();
        //             cmd.ExecuteNonQuery();

        //             // TRATANDO O PREMIO AUTOMATICO DO MOTORISTA
        //             if (ddlStatus.SelectedItem.Text == "Concluido" && txtMaterial.Text != "Vazio" && txtMaterial.Text != "Embalagem")
        //             {
        //                 // SOMENTE FUNCIONÁRIO
        //                 if (txtTipoMot.Text.Trim().ToUpper() == "FUNCIONÁRIO")
        //                 {
        //                     using (SqlConnection conn2 = new SqlConnection(
        //                         WebConfigurationManager
        //                         .ConnectionStrings["conexao"].ConnectionString))
        //                     {
        //                         conn2.Open();

        //                         SqlTransaction trans = conn2.BeginTransaction();

        //                         try
        //                         {
        //                             // ============================================
        //                             // FUNÇÃO
        //                             // ============================================
        //                             string funcao = "";

        //                             if (!string.IsNullOrWhiteSpace(txtFuncao.Text))
        //                             {
        //                                 funcao = txtFuncao.Text
        //                                     .Trim()
        //                                     .Split(' ')[0]
        //                                     .ToUpper();
        //                             }

        //                             decimal distancia = 0;
        //                             decimal valorPremio = 0;

        //                             // ============================================
        //                             // BUSCA DISTÂNCIA
        //                             // ============================================
        //                             using (SqlCommand cmdDist = new SqlCommand(@"
        //                              SELECT TOP 1 distancia
        //                              FROM tbcargas
        //                              WHERE carga = @carga",
        //                                 conn2, trans))
        //                             {
        //                                 cmdDist.Parameters.AddWithValue("@carga", carga);

        //                                 object result = cmdDist.ExecuteScalar();

        //                                 if (result != null &&
        //                                     result != DBNull.Value)
        //                                 {
        //                                     distancia =
        //                                         Convert.ToDecimal(result);
        //                                 }
        //                                 else
        //                                 {
        //                                     trans.Rollback();
        //                                     return;
        //                                 }
        //                             }

        //                             // ============================================
        //                             // BUSCA VALOR PRÊMIO
        //                             // ============================================
        //                             using (SqlCommand cmdPremio = new SqlCommand(@"
        //                              SELECT TOP 1
        //                                  motorista,
        //                                  carreteiro,
        //                                  bitrem
        //                              FROM tbvalorpremiomotoristas
        //                              WHERE @distancia
        //                              BETWEEN distancia1 AND distancia2 AND status = 'ATIVO'AND empresa='MATRIZ'",
        //                                 conn2, trans))
        //                             {
        //                                 cmdPremio.Parameters.AddWithValue(
        //                                     "@distancia",
        //                                     distancia);

        //                                 using (SqlDataReader dr =
        //                                     cmdPremio.ExecuteReader())
        //                                 {
        //                                     if (dr.Read())
        //                                     {
        //                                         switch (funcao)
        //                                         {
        //                                             case "MOTORISTA":

        //                                                 valorPremio =
        //                                                     dr["motorista"] != DBNull.Value
        //                                                     ? Convert.ToDecimal(dr["motorista"])
        //                                                     : 0;

        //                                                 break;

        //                                             case "CARRETEIRO":

        //                                                 valorPremio =
        //                                                     dr["carreteiro"] != DBNull.Value
        //                                                     ? Convert.ToDecimal(dr["carreteiro"])
        //                                                     : 0;

        //                                                 break;

        //                                             case "BITREM":

        //                                                 valorPremio =
        //                                                     dr["bitrem"] != DBNull.Value
        //                                                     ? Convert.ToDecimal(dr["bitrem"])
        //                                                     : 0;

        //                                                 break;
        //                                         }
        //                                     }
        //                                 }
        //                             }

        //                             // ============================================
        //                             // VERIFICA EXISTÊNCIA
        //                             // ============================================
        //                             using (SqlCommand cmdExiste = new SqlCommand(@"
        //                              SELECT COUNT(*)
        //                              FROM tb_custo_motorista
        //                              WHERE cod_cracha = @cod
        //                              AND dt_custo = @data",
        //                                 conn2, trans))
        //                             {
        //                                 cmdExiste.Parameters.AddWithValue(
        //                                     "@cod",
        //                                     txtCodMotorista.Text.Trim());

        //                                 cmdExiste.Parameters.AddWithValue(
        //                                     "@data",
        //                                     SafeDateValue(txtSaidaPlanta.Text.Trim()));

        //                                 int existe =
        //                                     Convert.ToInt32(
        //                                         cmdExiste.ExecuteScalar());

        //                                 // ============================================
        //                                 // UPDATE
        //                                 // ============================================
        //                                 if (existe > 0)
        //                                 {
        //                                     using (SqlCommand cmdUpdate =
        //                                         new SqlCommand(@"
        //                                      UPDATE tb_custo_motorista
        //                                      SET vl_premio =
        //                                          ISNULL(vl_premio,0) + @valor
        //                                      WHERE cod_cracha = @cod
        //                                      AND dt_custo = @data",
        //                                         conn2, trans))
        //                                     {
        //                                         cmdUpdate.Parameters.AddWithValue(
        //                                             "@valor",
        //                                             valorPremio);

        //                                         cmdUpdate.Parameters.AddWithValue(
        //                                             "@cod",
        //                                             txtCodMotorista.Text.Trim());

        //                                         cmdUpdate.Parameters.AddWithValue(
        //                                             "@data",
        //                                             SafeDateValue(
        //                                                 txtSaidaPlanta.Text.Trim()));

        //                                         cmdUpdate.ExecuteNonQuery();
        //                                     }
        //                                 }
        //                                 else
        //                                 {
        //                                     // ============================================
        //                                     // INSERT
        //                                     // ============================================
        //                                     using (SqlCommand cmdInsert =
        //                                         new SqlCommand(@"
        //                                          INSERT INTO tb_custo_motorista
        //                                          (
        //                                              cod_cracha,
        //                                              dt_custo,
        //                                              vl_premio
        //                                          )
        //                                          VALUES
        //                                          (
        //                                              @cod,
        //                                              @data,
        //                                              @valor
        //                                          )",
        //                                         conn2, trans))
        //                                     {
        //                                         cmdInsert.Parameters.AddWithValue(
        //                                             "@cod",
        //                                             txtCodMotorista.Text.Trim());

        //                                         cmdInsert.Parameters.AddWithValue(
        //                                             "@data",
        //                                             SafeDateValue(
        //                                                 txtSaidaPlanta.Text.Trim()));

        //                                         cmdInsert.Parameters.AddWithValue(
        //                                             "@valor",
        //                                             valorPremio);

        //                                         cmdInsert.ExecuteNonQuery();
        //                                     }
        //                                 }
        //                             }

        //                             trans.Commit();
        //                         }
        //                         catch (Exception ex)
        //                         {
        //                             trans.Rollback();

        //                             MostrarMsg2(
        //                                 "Erro prêmio motorista: "
        //                                 + ex.Message);
        //                         }
        //                     }
        //                 }
        //             }

        //             // TRATANDO A PERNOITE AUTOMATICA DO MOTORISTA
        //             DateTime dataPernoite;
        //             if (agora.TimeOfDay >= new TimeSpan(19, 0, 0) &&
        //                 agora.TimeOfDay <= new TimeSpan(23, 59, 59))
        //             {
        //                 dataPernoite = agora.Date;
        //                 if (ddlStatus.SelectedItem.Text.Trim() == "Pernoite")
        //                 {
        //                     // SOMENTE FUNCIONÁRIO
        //                     if (txtTipoMot.Text.Trim().ToUpper() == "FUNCIONÁRIO")
        //                     {
        //                         using (SqlConnection conn2 = new SqlConnection(
        //                             WebConfigurationManager
        //                             .ConnectionStrings["conexao"].ConnectionString))
        //                         {
        //                             conn2.Open();

        //                             SqlTransaction trans = conn2.BeginTransaction();

        //                             try
        //                             {
        //                                 // ============================================
        //                                 // VALOR PERNOITE
        //                                 // ============================================
        //                                 decimal valorPernoite = 0;
        //                                 decimal valorCafe = 0;

        //                                 using (SqlCommand cmdPernoite =
        //                                     new SqlCommand(@"
        //                                  SELECT TOP 1 pernoite
        //                                  FROM tbvalorpremiomotoristas WHERE status = 'ATIVO' and empresa='MATRIZ'",
        //                                     conn2, trans))
        //                                 {
        //                                     object result =
        //                                         cmdPernoite.ExecuteScalar();

        //                                     if (result != null &&
        //                                         result != DBNull.Value)
        //                                     {
        //                                         valorPernoite =
        //                                             Convert.ToDecimal(result);
        //                                     }
        //                                 }

        //                                 // ============================================
        //                                 // DATA
        //                                 // ============================================
        //                                 object dataCusto = dataPernoite;

        //                                 if (dataCusto == null ||
        //                                     dataCusto == DBNull.Value)
        //                                 {
        //                                     trans.Rollback();

        //                                     MostrarMsg2(
        //                                         "Data Pernoite inválida.");

        //                                     return;
        //                                 }

        //                                 // ============================================
        //                                 // VERIFICA EXISTÊNCIA
        //                                 // ============================================
        //                                 int existe = 0;

        //                                 using (SqlCommand cmdExiste =
        //                                     new SqlCommand(@"
        //                                  SELECT COUNT(*)
        //                                  FROM tb_custo_motorista
        //                                  WHERE cod_cracha = @cod
        //                                  AND dt_custo = @data",
        //                                     conn2, trans))
        //                                 {
        //                                     cmdExiste.Parameters.AddWithValue(
        //                                         "@cod",
        //                                         txtCodMotorista.Text.Trim());

        //                                     cmdExiste.Parameters.AddWithValue(
        //                                         "@data",
        //                                         dataCusto);

        //                                     existe =
        //                                         Convert.ToInt32(
        //                                             cmdExiste.ExecuteScalar());
        //                                 }

        //                                 // ============================================
        //                                 // UPDATE
        //                                 // ============================================
        //                                 if (existe > 0)
        //                                 {
        //                                     using (SqlCommand cmdUpdate =
        //                                         new SqlCommand(@"
        //                                      UPDATE tb_custo_motorista
        //                                      SET vl_pernoite = @valor, vl_cafe = @cafe
        //                                      WHERE cod_cracha = @cod
        //                                      AND dt_custo = @data",
        //                                         conn2, trans))
        //                                     {
        //                                         cmdUpdate.Parameters.AddWithValue(
        //                                             "@valor",
        //                                             valorPernoite);

        //                                         cmdUpdate.Parameters.AddWithValue(
        //                                             "@cafe",
        //                                             valorCafe);

        //                                         cmdUpdate.Parameters.AddWithValue(
        //                                             "@cod",
        //                                             txtCodMotorista.Text.Trim());

        //                                         cmdUpdate.Parameters.AddWithValue(
        //                                             "@data",
        //                                             dataCusto);

        //                                         cmdUpdate.ExecuteNonQuery();
        //                                     }
        //                                 }
        //                                 else
        //                                 {
        //                                     // ============================================
        //                                     // INSERT
        //                                     // ============================================
        //                                     using (SqlCommand cmdInsert =
        //                                         new SqlCommand(@"
        //                                      INSERT INTO tb_custo_motorista
        //                                      (
        //                                          cod_cracha,
        //                                          dt_custo,
        //                                          vl_pernoite,
        //                                          vl_cafe
        //                                      )
        //                                      VALUES
        //                                      (
        //                                          @cod,
        //                                          @data,
        //                                          @valor,
        //                                          @cafe
        //                                      )",
        //                                         conn2, trans))
        //                                     {
        //                                         cmdInsert.Parameters.AddWithValue(
        //                                             "@cod",
        //                                             txtCodMotorista.Text.Trim());

        //                                         cmdInsert.Parameters.AddWithValue(
        //                                             "@data",
        //                                             dataCusto);

        //                                         cmdInsert.Parameters.AddWithValue(
        //                                             "@valor",
        //                                             valorPernoite);

        //                                         cmdInsert.Parameters.AddWithValue(
        //                                             "@cafe",
        //                                             valorCafe);

        //                                         cmdInsert.ExecuteNonQuery();
        //                                     }
        //                                 }

        //                                 trans.Commit();

        //                                 //MostrarMsg2(
        //                                 //    "Pernoite salvo com sucesso.");
        //                             }
        //                             catch (Exception ex)
        //                             {
        //                                 trans.Rollback();

        //                                 MostrarMsg2(
        //                                     "Erro pernoite: "
        //                                     + ex.Message);
        //                             }
        //                         }
        //                     }
        //                 }
        //             }

        //             // TRATANDO O ALMOÇO AUTOMATICO DO MOTORISTA

        //         }

        //         // Atualizando a ordem de coleta 
        //         using (SqlConnection conn = new SqlConnection(
        //WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
        //         {
        //             var cvaOC = txtCVA.Text.Trim();

        //             string queryCarregamento = @"
        //              UPDATE tbcarregamentos SET 
        //                  situacao = @situacao,
        //                  cva = @cva,
        //                  status = @status,                        
        //                  cod_expedidor = @cod_expedidor,
        //                  expedidor = @expedidor,
        //                  cid_expedidor = @cid_expedidor,
        //                  uf_expedidor = @uf_expedidor,
        //                  cod_recebedor = @cod_recebedor,
        //                  recebedor = @recebedor,
        //                  cid_recebedor = @cid_recebedor,
        //                  carga = @carga,
        //                  dtsaida=@dtsaida,
        //                  dtchegada=@dtchegada,
        //                  dtconclusao=@dtconclusao,
        //                  uf_recebedor = @uf_recebedor,
        //                  dtalt = @dtalt,
        //                  usualt = @usualt

        //              WHERE num_carregamento = @num_carregamento";

        //             using (SqlCommand cmdCarregamento = new SqlCommand(queryCarregamento, conn))
        //             {
        //                 // Num carregamento (obrigatório)
        //                 cmdCarregamento.Parameters.Add("@num_carregamento", SqlDbType.NVarChar, 20)
        //                     .Value = novaColeta.Text.Trim();

        //                 // Status
        //                 cmdCarregamento.Parameters.Add("@situacao", SqlDbType.NVarChar, 50)
        //                     .Value = string.IsNullOrWhiteSpace(situacaoOC)
        //                         ? (object)DBNull.Value
        //                         : situacaoOC.Trim();

        //                 cmdCarregamento.Parameters.Add("@cva", SqlDbType.NVarChar, 50)
        //                     .Value = string.IsNullOrWhiteSpace(cvaOC)
        //                         ? (object)DBNull.Value
        //                         : cvaOC.Trim();

        //                 cmdCarregamento.Parameters.Add("@status", SqlDbType.NVarChar, 50)
        //                     .Value = string.IsNullOrWhiteSpace(statusOC)
        //                         ? (object)DBNull.Value
        //                         : statusOC.Trim();
        //                 int cargaCarreg;
        //                 cmdCarregamento.Parameters.Add("@carga", SqlDbType.Int)
        //                     .Value = int.TryParse(carga, out cargaCarreg)
        //                         ? (object)cargaCarreg
        //                         : (object)DBNull.Value;

        //                 // Expedidor
        //                 int codExp;
        //                 cmdCarregamento.Parameters.Add("@cod_expedidor", SqlDbType.Int)
        //                     .Value = int.TryParse(txtCodExpedidor.Text, out codExp)
        //                         ? (object)codExp
        //                         : (object)DBNull.Value;

        //                 cmdCarregamento.Parameters.Add("@expedidor", SqlDbType.NVarChar, 100)
        //                     .Value = string.IsNullOrWhiteSpace(cboExpedidor.Text)
        //                         ? (object)DBNull.Value
        //                         : cboExpedidor.Text.Trim();

        //                 cmdCarregamento.Parameters.Add("@cid_expedidor", SqlDbType.NVarChar, 100)
        //                     .Value = string.IsNullOrWhiteSpace(txtCidExpedidor.Text)
        //                         ? (object)DBNull.Value
        //                         : txtCidExpedidor.Text.Trim();

        //                 cmdCarregamento.Parameters.Add("@uf_expedidor", SqlDbType.NVarChar, 2)
        //                     .Value = string.IsNullOrWhiteSpace(txtUFExpedidor.Text)
        //                         ? (object)DBNull.Value
        //                         : txtUFExpedidor.Text.Trim();

        //                 // Recebedor
        //                 int codRec;
        //                 cmdCarregamento.Parameters.Add("@cod_recebedor", SqlDbType.Int)
        //                     .Value = int.TryParse(txtCodRecebedor.Text, out codRec)
        //                         ? (object)codRec
        //                         : (object)DBNull.Value;

        //                 cmdCarregamento.Parameters.Add("@recebedor", SqlDbType.NVarChar, 100)
        //                     .Value = string.IsNullOrWhiteSpace(cboRecebedor.Text)
        //                         ? (object)DBNull.Value
        //                         : cboRecebedor.Text.Trim();

        //                 cmdCarregamento.Parameters.Add("@cid_recebedor", SqlDbType.NVarChar, 100)
        //                     .Value = string.IsNullOrWhiteSpace(txtCidRecebedor.Text)
        //                         ? (object)DBNull.Value
        //                         : txtCidRecebedor.Text.Trim();
        //                 cmdCarregamento.Parameters.AddWithValue("@dtsaida", SafeDateValue(txtSaidaOrigem.Text.Trim()));
        //                 cmdCarregamento.Parameters.AddWithValue("@dtchegada", SafeDateValue(txtChegadaDestino.Text.Trim()));
        //                 cmdCarregamento.Parameters.AddWithValue("@dtconclusao", SafeDateValue(txtSaidaPlanta.Text.Trim()));

        //                 cmdCarregamento.Parameters.Add("@uf_recebedor", SqlDbType.NVarChar, 2)
        //                     .Value = string.IsNullOrWhiteSpace(txtUFRecebedor.Text)
        //                         ? (object)DBNull.Value
        //                         : txtUFRecebedor.Text.Trim();
        //                 cmdCarregamento.Parameters.AddWithValue("@dtalt", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
        //                 cmdCarregamento.Parameters.AddWithValue("@usualt", Session["UsuarioLogado"].ToString());

        //                 conn.Open();
        //                 cmdCarregamento.ExecuteNonQuery();
        //             }
        //         }

        //         using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
        //         {
        //             TextBox txtRedes = (TextBox)e.Item.FindControl("txtRedes");
        //             TextBox txtCatracas = (TextBox)e.Item.FindControl("txtCatracas");
        //             TextBox txtOT = (TextBox)e.Item.FindControl("txtOT");


        //             string query = @"UPDATE tbpedidos SET                                  
        //                          andamento = @andamento
        //                          WHERE carga = @carga";

        //             SqlCommand cmd = new SqlCommand(query, conn);
        //             cmd.Parameters.AddWithValue("@carga", carga);
        //             cmd.Parameters.AddWithValue("@andamento", andamentoCarga);



        //             // Chama método que verifica no banco
        //             conn.Open();
        //             cmd.ExecuteNonQuery();
        //         }





        //         // Após atualizar, recarregar os dados no Repeater
        //         Session["Coletas"] = null;
        //         CarregarColetas(novaColeta.Text);
        //         //BuscarCteSalvos(idViagem);
        //         CarregarPedidos(int.Parse(carga), gv);
        //     }
        //     if (e.CommandName == "AtualizarAbas")
        //     {
        //         //Atualiza CT-e
        //         string carga = e.CommandArgument.ToString();
        //         string idViagem = e.CommandArgument.ToString(); // O 'carga' que você passou no Eval
        //         int index = e.Item.ItemIndex; // O índice da linha no Repeater
        //         string nomeUsuario = Session["UsuarioLogado"].ToString();


        //         // 1. Verificar se existem CT-es lidos na Session para este item
        //         if (ListaCtePorItem.ContainsKey(index) && ListaCtePorItem[index].Count > 0)
        //         {
        //             List<CteLido> listaParaSalvar = ListaCtePorItem[index];

        //             try
        //             {
        //                 if (con.State == ConnectionState.Closed) con.Open();
        //                 SqlTransaction trans = con.BeginTransaction();

        //                 try
        //                 {
        //                     foreach (var cte in listaParaSalvar)
        //                     {
        //                         string sql = @"INSERT INTO tbcte 
        //                      (chave_de_acesso, uf_emissor, cnpj_empresa, empresa_emissora, 
        //                       num_documento, serie_documento, tipo_documento,mes_ano_documento,emitido_por, emissao_documento, id_viagem)
        //                      VALUES 
        //                      (@chave, @uf, @cnpj, @empresa, @num, @serie, @tipo,@mes_ano_documento, @emitido_por, @emissao_documento, @idViagem)";

        //                         using (SqlCommand cmd = new SqlCommand(sql, con, trans))
        //                         {
        //                             // Extraímos o CNPJ (posições 7 a 20 da chave de 44 dígitos)
        //                             string cnpjExtraido = cte.ChaveOriginal.Substring(6, 14);

        //                             cmd.Parameters.AddWithValue("@chave", cte.ChaveOriginal);
        //                             cmd.Parameters.AddWithValue("@uf", cte.Estado);
        //                             cmd.Parameters.AddWithValue("@cnpj", cnpjExtraido);
        //                             cmd.Parameters.AddWithValue("@empresa", cte.Filial);
        //                             cmd.Parameters.AddWithValue("@num", cte.Numero);
        //                             cmd.Parameters.AddWithValue("@serie", cte.Serie);
        //                             cmd.Parameters.AddWithValue("@tipo", "CT-e");
        //                             cmd.Parameters.AddWithValue("@mes_ano_documento", cte.Emissao);
        //                             cmd.Parameters.AddWithValue("@emitido_por", nomeUsuario);
        //                             cmd.Parameters.AddWithValue("@emissao_documento", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.000"));
        //                             cmd.Parameters.AddWithValue("@idViagem", carga);

        //                             cmd.ExecuteNonQuery();
        //                         }
        //                     }

        //                     trans.Commit();

        //                     // 2. Limpar a Session deste índice específico
        //                     ListaCtePorItem.Remove(index);

        //                     // 3. Sucesso!
        //                     MostrarMsg2("Sucesso: " + listaParaSalvar.Count + " documentos salvos.");

        //                     // 4. IMPORTANTE: Rebindar o Repeater. 
        //                     // Isso fará com que o ItemDataBound rode novamente, 
        //                     // alimentando o GridView com os dados que agora estão no Banco.
        //                     rptColetas.DataBind();
        //                 }
        //                 catch (Exception ex)
        //                 {
        //                     string erroLimpo = ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "");
        //                     MostrarMsg2("ERRO REAL: " + erroLimpo);
        //                 }
        //             }
        //             finally
        //             {
        //                 con.Close();
        //             }
        //         }
        //         else
        //         {
        //             MostrarMsg2("Aviso: Nenhuma nova leitura pendente para salvar.");
        //         }

        //         if (gv != null)
        //         {
        //             try
        //             {
        //                 if (con.State == ConnectionState.Closed) con.Open();
        //                 SqlTransaction trans = con.BeginTransaction();

        //                 try
        //                 {
        //                     foreach (GridViewRow linha in gv.Rows)
        //                     {
        //                         // 1. Pegar as chaves da Grid
        //                         // Index 0 = pedido, Index 1 = id_viagem (conforme definido no DataKeyNames)
        //                         string numPedido = gv.DataKeys[linha.RowIndex].Values[0].ToString();


        //                         // 2. Localizar os controles da linha
        //                         TextBox txtIni = (TextBox)linha.FindControl("txtInicioCar");
        //                         TextBox txtFim = (TextBox)linha.FindControl("txtTermCar");
        //                         TextBox txtDur = (TextBox)linha.FindControl("txtTempoTotal");
        //                         DropDownList ddlMot = (DropDownList)linha.FindControl("ddlMotCar");

        //                         // 3. Query de Update
        //                         string sql = @"UPDATE tbpedidos SET 
        //                                 iniciocar = @ini, 
        //                                 termcar = @fim, 
        //                                 duracao = @dur,
        //                                 motcar = @mot
        //                                 WHERE pedido = @pedido ";

        //                         using (SqlCommand cmd = new SqlCommand(sql, con, trans))
        //                         {
        //                             // Conversão segura para DateTime (DateTimeLocal envia yyyy-MM-ddTHH:mm)
        //                             DateTime dtIni, dtFim;

        //                             if (DateTime.TryParse(txtIni.Text, out dtIni))
        //                                 cmd.Parameters.AddWithValue("@ini", dtIni);
        //                             else
        //                                 cmd.Parameters.AddWithValue("@ini", DBNull.Value);

        //                             if (DateTime.TryParse(txtFim.Text, out dtFim))
        //                                 cmd.Parameters.AddWithValue("@fim", dtFim);
        //                             else
        //                                 cmd.Parameters.AddWithValue("@fim", DBNull.Value);

        //                             cmd.Parameters.AddWithValue("@dur", txtDur.Text);
        //                             cmd.Parameters.AddWithValue("@mot", ddlMot.SelectedItem.Text);
        //                             cmd.Parameters.AddWithValue("@pedido", numPedido);
        //                             //cmd.Parameters.AddWithValue("@idViagem", idViagem);

        //                             cmd.ExecuteNonQuery();
        //                         }
        //                     }

        //                     trans.Commit();
        //                     MostrarMsg2("Alterações salvas com sucesso!");
        //                 }
        //                 catch (Exception ex)
        //                 {
        //                     trans.Rollback();
        //                     throw ex;
        //                 }
        //             }
        //             catch (Exception ex)
        //             {
        //                 MostrarMsg2("Erro ao salvar pedidos: " + ex.Message);
        //             }
        //             finally
        //             {
        //                 con.Close();
        //             }
        //         }

        //         TextBox txtHistoricoObservacao = (TextBox)e.Item.FindControl("txtHistoricoObservacao");

        //         if (txtHistoricoObservacao.Text != string.Empty)
        //         {
        //             try
        //             {
        //                 if (con.State == ConnectionState.Closed) con.Open();
        //                 SqlTransaction trans = con.BeginTransaction();

        //                 try
        //                 {

        //                     // 3. Query de Update
        //                     string sql = @"UPDATE tbcargas SET 
        //                                 observacao = @observacao 
        //                                 WHERE carga = @carga ";

        //                     using (SqlCommand cmd = new SqlCommand(sql, con, trans))
        //                     {

        //                         cmd.Parameters.AddWithValue("@observacao", txtHistoricoObservacao.Text);
        //                         cmd.Parameters.AddWithValue("@carga", carga);
        //                         //cmd.Parameters.AddWithValue("@idViagem", idViagem);

        //                         cmd.ExecuteNonQuery();
        //                     }


        //                     trans.Commit();
        //                     MostrarMsg("Alterações salvas com sucesso!");
        //                 }
        //                 catch (Exception ex)
        //                 {
        //                     trans.Rollback();
        //                     throw ex;
        //                 }
        //             }
        //             catch (Exception ex)
        //             {
        //                 MostrarMsg("Erro ao salvar Observacao: " + ex.Message);
        //             }
        //             finally
        //             {
        //                 con.Close();
        //             }
        //         }

        //         TextBox txtMDFe = (TextBox)e.Item.FindControl("txtMDFe");

        //         if (txtMDFe.Text != string.Empty)
        //         {
        //             try
        //             {
        //                 if (con.State == ConnectionState.Closed) con.Open();
        //                 SqlTransaction trans = con.BeginTransaction();

        //                 try
        //                 {

        //                     // 3. Query de Update
        //                     string sql = @"UPDATE tbcargas SET 
        //                                 mdfe = @mdfe 
        //                                 WHERE carga = @carga ";

        //                     using (SqlCommand cmd = new SqlCommand(sql, con, trans))
        //                     {

        //                         cmd.Parameters.AddWithValue("@mdfe", txtMDFe.Text);
        //                         cmd.Parameters.AddWithValue("@carga", carga);
        //                         //cmd.Parameters.AddWithValue("@idViagem", idViagem);

        //                         cmd.ExecuteNonQuery();
        //                     }


        //                     trans.Commit();
        //                     MostrarMsg("Alterações salvas com sucesso!");
        //                 }
        //                 catch (Exception ex)
        //                 {
        //                     trans.Rollback();
        //                     throw ex;
        //                 }
        //             }
        //             catch (Exception ex)
        //             {
        //                 MostrarMsg2("Erro ao salvar MDFe: " + ex.Message);
        //             }
        //             finally
        //             {
        //                 con.Close();
        //             }
        //         }
        //         TextBox txtNFSe = (TextBox)e.Item.FindControl("txtNFSe");

        //         if (txtNFSe != null && !string.IsNullOrWhiteSpace(txtNFSe.Text))
        //         {
        //             try
        //             {
        //                 if (con.State == ConnectionState.Closed) con.Open();
        //                 string sqlExiste = "SELECT COUNT(1) FROM tbnfse WHERE num_documento = @num_documento";

        //                 using (SqlCommand cmdExiste = new SqlCommand(sqlExiste, con))
        //                 {

        //                     cmdExiste.Parameters.AddWithValue("@num_documento", txtNFSe.Text);

        //                     int existe = Convert.ToInt32(cmdExiste.ExecuteScalar());

        //                     if (existe > 0)
        //                     {
        //                         MostrarMsg2("Já existe uma NFS-e cadastrada com esta chave:\n" + txtNFSe.Text);
        //                         return; // ⛔ não continua o método
        //                     }
        //                 }

        //                 using (SqlTransaction trans = con.BeginTransaction())
        //                 {
        //                     try
        //                     {
        //                         string sql = @"INSERT INTO tbnfse 
        //                      (num_documento, serie_documento, tipo_documento, emitido_por, emissao_documento, status_documento,situacao_documento, idviagem)
        //                      VALUES 
        //                      (@num, @serie, @tipo, @emitido_por, @emissao_documento, @status_documento,@situacao_documento, @idViagem)";

        //                         using (SqlCommand cmd = new SqlCommand(sql, con, trans))
        //                         {
        //                             DateTime agora = DateTime.Now;

        //                             cmd.Parameters.AddWithValue("@num", txtNFSe.Text);
        //                             cmd.Parameters.AddWithValue("@serie", "1");
        //                             cmd.Parameters.AddWithValue("@tipo", "NFS-e");
        //                             cmd.Parameters.AddWithValue("@emitido_por", nomeUsuario);
        //                             // Passando como DateTime puro (mais seguro)
        //                             cmd.Parameters.AddWithValue("@status_documento", "Pendente");
        //                             cmd.Parameters.AddWithValue("@situacao_documento", "Emitido");
        //                             cmd.Parameters.AddWithValue("@emissao_documento", agora);
        //                             cmd.Parameters.AddWithValue("@idViagem", carga);

        //                             cmd.ExecuteNonQuery();
        //                         }

        //                         trans.Commit(); // Commit primeiro

        //                         // Agora que os dados estão "vivos" no banco, fazemos o bind
        //                         rptColetas.DataBind();

        //                         MostrarMsg2("Alterações salvas com sucesso!");
        //                     }
        //                     catch (Exception ex)
        //                     {
        //                         trans.Rollback();
        //                         throw ex;
        //                     }
        //                 }
        //             }
        //             catch (Exception ex)
        //             {
        //                 MostrarMsg("Erro ao salvar NFS-e: " + ex.Message);
        //             }
        //             finally
        //             {
        //                 if (con.State == ConnectionState.Open) con.Close();
        //             }
        //         }

        //         Session["Coletas"] = null;
        //         CarregarColetas(novaColeta.Text);
        //         BuscarCteSalvos(idViagem);
        //         CarregarPedidos(int.Parse(carga), gv);




        //     }
        //     if (e.CommandName == "Coletas")
        //     {
        //         int id = Convert.ToInt32(e.CommandArgument);
        //         string idCarga = id.ToString(); // esse valor viria da lógica do seu código
        //                                         //hdIdCarga.Value = idCarga;
        //         Session["idCarga"] = idCarga;
        //         //GetPedidos();
        //         //string url = $"OrdemColetaImpressaoIndividual.aspx?id={idCarga}";
        //         //string script = $"window.open('{url}', '_blank', 'toolbar=no,location=no,status=no,menubar=no,scrollbars=yes,resizable=yes,width=794,height=1123');";
        //         //ClientScript.RegisterStartupScript(this.GetType(), "abrirJanela", script, true);
        //     }
        //     if (e.CommandName == "AtualizarColeta")
        //     {
        //         string carga = e.CommandArgument.ToString();

        //         // Recuperar os controles de dentro do item                



        //         // Exemplo: atualizando no banco
        //         using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
        //         {
        //             TextBox txtRedes = (TextBox)e.Item.FindControl("txtRedes");
        //             TextBox txtCatracas = (TextBox)e.Item.FindControl("txtCatracas");
        //             TextBox txtOT = (TextBox)e.Item.FindControl("txtOT");


        //             string query = @"UPDATE tbcargas SET                                  
        //                          ot = @ot,
        //                          catraca = @catraca,
        //                          rede = @rede 
        //                          WHERE carga = @carga";

        //             SqlCommand cmd = new SqlCommand(query, conn);
        //             cmd.Parameters.AddWithValue("@carga", carga);
        //             cmd.Parameters.AddWithValue("@ot", txtOT.Text);
        //             cmd.Parameters.AddWithValue("@catraca", txtCatracas.Text);
        //             cmd.Parameters.AddWithValue("@rede", txtRedes.Text.Trim());


        //             // Chama método que verifica no banco
        //             conn.Open();
        //             cmd.ExecuteNonQuery();
        //         }



        //         // Após atualizar, recarregar os dados no Repeater
        //         MostrarMsg2("Dados Salvos!");
        //         Session["Coletas"] = null;
        //         CarregarColetas(novaColeta.Text);
        //     }
        //     if (e.CommandName == "PedagioManual")
        //     {
        //         using (SqlConnection conn = new SqlConnection(
        //         WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
        //         {
        //             string query = @"UPDATE tbcarregamentos SET 
        //              pedagio = @pedagio,                                 
        //              pedagiofeito = @pedagiofeito 
        //              WHERE num_carregamento = @num_carregamento";

        //             using (SqlCommand cmdPedagio = new SqlCommand(query, conn))
        //             {
        //                 cmdPedagio.Parameters.Add("@num_carregamento", SqlDbType.Int)
        //                                      .Value = Convert.ToInt32(novaColeta.Text);

        //                 cmdPedagio.Parameters.Add("@pedagio", SqlDbType.VarChar)
        //                                      .Value = "SIM";

        //                 cmdPedagio.Parameters.Add("@pedagiofeito", SqlDbType.VarChar)
        //                                      .Value = "Pendente";

        //                 conn.Open();
        //                 int linhas = cmdPedagio.ExecuteNonQuery();

        //                 if (linhas == 0)
        //                 {
        //                     throw new Exception("Nenhum registro encontrado para atualizar.");
        //                 }
        //             }
        //         }


        //         // Após atualizar, recarregar os dados no Repeater
        //         // MostrarMsg2("Pedágio enviado com sucesso!");
        //         //Session["Coletas"] = null;
        //         //CarregarColetas(novaColeta.Text);
        //     }
        //     if (e.CommandName == "EnviarSM")
        //     {
        //         // 1. Captura os dados da interface ANTES do processo assíncrono
        //         string idCarga = e.CommandArgument.ToString();

        //         DropDownList ddlPercurso = (DropDownList)e.Item.FindControl("ddlPercurso");
        //         DropDownList ddlRotaKrona = (DropDownList)e.Item.FindControl("ddlRotaKrona");
        //         string peso = ((TextBox)e.Item.FindControl("txtPeso")).Text;
        //         string valor = ((TextBox)e.Item.FindControl("txtValorTotal")).Text;
        //         string previsao_inicial = ((TextBox)e.Item.FindControl("txtPrevisaoInicio")).Text;
        //         string previsao_final = ((TextBox)e.Item.FindControl("txtPrevisaoTermino")).Text;
        //         string codmotorista = txtCodMotorista.Text;
        //         string percurso = ddlPercurso.SelectedItem.Text;
        //         string rota = ddlRotaKrona.SelectedItem.Text;
        //         string id_rota = ddlRotaKrona.SelectedValue;
        //         string placa = txtPlaca.Text;
        //         string codveiculo = txtCodVeiculo.Text;

        //         try
        //         {
        //             // 1. Monta o objeto
        //             var solicitacao = CriarObjetoSolicitacao(idCarga, placa, valor, peso, previsao_inicial, previsao_final, percurso, rota, id_rota, codmotorista, codveiculo);

        //             // 2. Serializa
        //             string jsonEnvio = System.Text.Json.JsonSerializer.Serialize(solicitacao, new JsonSerializerOptions
        //             {
        //                 WriteIndented = true,
        //                 Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        //             });

        //             // 3. Grava Arquivo Físico (Auditoria)
        //             string caminhoFisico = Server.MapPath("~/EnviaSM/SM_Solicitacao_" + idCarga + ".json");
        //             System.IO.File.WriteAllText(caminhoFisico, jsonEnvio, System.Text.Encoding.UTF8);

        //             // 4. ENVIO SÍNCRONO (A página vai "carregar" enquanto espera a resposta)
        //             string jsonResposta = EnviarRequisicaoKronaSincrona(jsonEnvio);

        //             // 5. Salva no Banco (Corrigindo a ordem dos parâmetros para bater com a definição do método)
        //             // Ordem correta baseada no seu método: (json, carga, valor, previsao_init, previsao_fim, percurso, rota, id_rota)
        //             ProcessarESalvarRetorno(jsonResposta, idCarga, valor, previsao_inicial, previsao_final, percurso, rota, id_rota);

        //         }
        //         catch (Exception ex)
        //         {
        //             MostrarMsg2("Erro: " + ex.Message);
        //         }
        //     }
        //     if (e.CommandName == "BuscarNfe")
        //     {
        //         string idCarga = e.CommandArgument.ToString();
        //         string chave = ((TextBox)e.Item.FindControl("txtChaveNF")).Text;
        //         GridView gvn = (GridView)e.Item.FindControl("gvNF");
        //         TextBox txtChaveNF = (TextBox)e.Item.FindControl("txtChaveNF");

        //         //string chave = "35260259104422002446550370009554061775712904";
        //         string apiKey = "025caf00-6477-4d97-b133-f34ad21594f3";

        //         // ========= 1ª CHAMADA (PUT) =========
        //         var request = (HttpWebRequest)WebRequest.Create(
        //             "https://api.meudanfe.com.br/v2/fd/add/" + chave);

        //         request.Method = "PUT";
        //         request.Accept = "application/json";
        //         request.Headers.Add("Api-Key", apiKey);
        //         request.ContentLength = 0; // 👈 MUITO IMPORTANTE

        //         string jsonPut;

        //         using (var requestStream = request.GetRequestStream())
        //         {
        //             // não escreve nada, só força o envio do corpo vazio
        //         }

        //         using (var response = (HttpWebResponse)request.GetResponse())
        //         using (var reader = new StreamReader(response.GetResponseStream()))
        //         {
        //             jsonPut = reader.ReadToEnd();
        //         }

        //         var serializer = new JavaScriptSerializer();
        //         RetornoPut retornoPut = serializer.Deserialize<RetornoPut>(jsonPut);

        //         // ========= SE STATUS FOR OK =========
        //         if (retornoPut.status == "OK")
        //         {
        //             // ========= 2ª CHAMADA (GET XML) =========
        //             var requestXml = (HttpWebRequest)WebRequest.Create(
        //                 "https://api.meudanfe.com.br/v2/fd/get/xml/" + chave);

        //             requestXml.Method = "GET";
        //             requestXml.Accept = "application/json";
        //             requestXml.Headers.Add("Api-Key", apiKey);

        //             string jsonXml;

        //             using (var responseXml = (HttpWebResponse)requestXml.GetResponse())
        //             using (var readerXml = new StreamReader(responseXml.GetResponseStream()))
        //             {
        //                 jsonXml = readerXml.ReadToEnd();
        //             }

        //             RetornoXml retornoXml = serializer.Deserialize<RetornoXml>(jsonXml);

        //             string xmlNfe = retornoXml.data;

        //             // aqui você já tem o XML puro
        //             // pode salvar em arquivo, banco, ou carregar em XmlDocument

        //             // exemplo salvando em arquivo:
        //             //File.WriteAllText(Server.MapPath("~/nfe.xml"), xmlNfe);

        //             SalvarXmlNoBanco(xmlNfe, idCarga);
        //             txtChaveNF.Text = string.Empty;
        //             txtChaveNF.Focus();
        //             CarregarNF(idCarga, gvn);


        //         }
        //         else
        //         {
        //             // erro retornado pela API
        //             string msgErro = retornoPut.statusMessage;

        //             MostrarMsg2("Erro ao executar o processo: " + msgErro);
        //         }

        //     }
        //     if (e.CommandName == "GeraDoc")
        //     {
        //         int idCarga = int.Parse(e.CommandArgument.ToString());

        //         // 1. Sua validação que você confirmou que funciona:
        //         if (!PossuiNotasLancadas(idCarga))
        //         {
        //             MostrarMsg2("O arquivo só pode ser gerado quando as NF-e forem lançadas.");
        //             return;
        //         }

        //         // 2. Define o tipo e gera o número no banco
        //         bool ehServico = VerificarSeEhServico(idCarga);
        //         string numeroDocumento = ehServico ? GerarEObterProximoNFse() : GerarEObterProximoCte();

        //         if (string.IsNullOrEmpty(numeroDocumento))
        //         {
        //             MostrarMsg2("Erro ao gerar a numeração do documento.");
        //             return;
        //         }

        //         // 3. Em vez de chamar o DispararDownload aqui, chamamos a página dedicada via JavaScript.
        //         // O window.open faz o navegador abrir o fluxo de download nativo sem sair da página atual.
        //         string urlDownload = $"DownloadSapiens.aspx?idCarga={idCarga}&numDoc={numeroDocumento}&serv={ehServico.ToString().ToLower()}";
        //         string script = $"window.open('{urlDownload}', '_blank');";

        //         ScriptManager.RegisterStartupScript(this, this.GetType(), "dispararDownloadSapiens", script, true);
        //     }
        //     if (e.CommandName == "GeraXml")
        //     {
        //         try
        //         {
        //             int idCarga = int.Parse(e.CommandArgument.ToString());

        //             // 1. Validação de segurança
        //             if (!PossuiNotasLancadas(idCarga))
        //             {
        //                 MostrarMsg2("O arquivo só pode ser gerado quando as NF-e forem lançadas.");
        //                 return;
        //             }

        //             // 2. Determina o tipo e busca o próximo número no banco
        //             bool ehServico = VerificarSeEhServico(idCarga);
        //             string numeroDoc = ehServico ? GerarEObterProximoNFse() : GerarEObterProximoCte();

        //             if (string.IsNullOrEmpty(numeroDoc))
        //             {
        //                 MostrarMsg2("Erro ao gerar a numeração do documento.");
        //                 return;
        //             }

        //             // 3. Em vez de chamar o DispararDownload interno com problema de AJAX,
        //             // redireciona para a página dedicada passando o parâmetro &formato=xml
        //             string urlDownload = $"DownloadSapiens.aspx?idCarga={idCarga}&numDoc={numeroDoc}&serv={ehServico.ToString().ToLower()}&formato=xml";
        //             string script = $"window.open('{urlDownload}', '_blank');";

        //             ScriptManager.RegisterStartupScript(this, this.GetType(), "dispararDownloadXml", script, true);
        //         }
        //         catch (Exception ex)
        //         {
        //             MostrarMsg2("Erro ao processar documento: " + ex.Message);
        //         }
        //     }
        //     if (e.CommandName == "CancelarCarga")
        //     {
        //         // 1. Verificação de permissão por usuário
        //         string usuarioLogado = Session["UsuarioLogado"]?.ToString();
        //         string usuarioSistema = Session["UsuarioSistema"]?.ToString();
        //         if (usuarioSistema != "TNG30976" && usuarioSistema != "admin")
        //         {
        //             // Aqui você pode usar um ScriptManager para alertar o usuário no navegador
        //             ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage",
        //                 "alert('Essa opção não está disponível para o seu usuário.');", true);
        //             return;
        //         }

        //         string carga = e.CommandArgument.ToString();
        //         string usuarioFormatado = DateTime.Now.ToString("dd/MM/yyyy HH:mm") + " - " + usuarioLogado;

        //         using (SqlConnection conn = new SqlConnection(
        //             WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
        //         {
        //             conn.Open();
        //             using (SqlTransaction trans = conn.BeginTransaction())
        //             {
        //                 try
        //                 {
        //                     int idViagem;

        //                     // 🔹 1. Buscar idviagem da carga
        //                     string sqlBusca = "SELECT idviagem FROM tbcargas WHERE carga = @carga";
        //                     using (SqlCommand cmd = new SqlCommand(sqlBusca, conn, trans))
        //                     {
        //                         cmd.Parameters.Add("@carga", SqlDbType.Int).Value = Convert.ToInt32(carga);
        //                         idViagem = Convert.ToInt32(cmd.ExecuteScalar());
        //                     }

        //                     // 🔹 2. Cancelar a carga na tbcargas
        //                     string sqlUpdateCarga = @"UPDATE tbcargas 
        //                                   SET status = 'Cancelada',
        //                                       andamento = 'CANCELADA',
        //                                       atualizacao = @usuario,
        //                                       material = 'Vazio',
        //                                       fl_exclusao = 'S' 
        //                                   WHERE carga = @carga";

        //                     using (SqlCommand cmd = new SqlCommand(sqlUpdateCarga, conn, trans))
        //                     {
        //                         cmd.Parameters.Add("@carga", SqlDbType.Int).Value = Convert.ToInt32(carga);
        //                         cmd.Parameters.Add("@usuario", SqlDbType.VarChar).Value = usuarioFormatado;
        //                         cmd.ExecuteNonQuery();
        //                     }

        //                     // 🔹 NOVO: 3. Cancelar na tbpedidos usando a carga como parâmetro
        //                     string sqlUpdatePedidos = "UPDATE tbpedidos SET fl_exclusao = 'S' WHERE carga = @carga";
        //                     using (SqlCommand cmd = new SqlCommand(sqlUpdatePedidos, conn, trans))
        //                     {
        //                         cmd.Parameters.Add("@carga", SqlDbType.Int).Value = Convert.ToInt32(carga);
        //                         cmd.ExecuteNonQuery();
        //                     }

        //                     // 🔹 4. Verificar se ainda existem cargas ativas
        //                     string sqlVerifica = @"SELECT COUNT(*) 
        //                                 FROM tbcargas 
        //                                 WHERE idviagem = @idviagem 
        //                                 AND fl_exclusao IS NULL";

        //                     int totalAtivas = 0;
        //                     using (SqlCommand cmd = new SqlCommand(sqlVerifica, conn, trans))
        //                     {
        //                         cmd.Parameters.Add("@idviagem", SqlDbType.Int).Value = idViagem;
        //                         totalAtivas = Convert.ToInt32(cmd.ExecuteScalar());
        //                     }

        //                     // 🔹 5. Se não existir nenhuma ativa → cancelar OC
        //                     if (totalAtivas == 0)
        //                     {
        //                         string sqlOC = @"UPDATE tbcarregamentos
        //                               SET status = 'Cancelada',
        //                                   situacao = 'O. C. CANCELADA',
        //                                   fl_exclusao = 'S',
        //                                   usualt = @usuario,
        //                                   dtalt = GETDATE()
        //                               WHERE num_carregamento = @idviagem";

        //                         using (SqlCommand cmd = new SqlCommand(sqlOC, conn, trans))
        //                         {
        //                             cmd.Parameters.Add("@idviagem", SqlDbType.Int).Value = idViagem;
        //                             cmd.Parameters.Add("@usuario", SqlDbType.VarChar).Value = usuarioLogado;
        //                             cmd.ExecuteNonQuery();
        //                         }
        //                     }

        //                     trans.Commit();

        //                     if (totalAtivas == 0)
        //                     {
        //                         Response.Redirect("GestaoDeEntregasMatriz.aspx");
        //                     }
        //                     else
        //                     {
        //                         Session["Coletas"] = null;
        //                         CarregarColetas(novaColeta.Text);
        //                     }
        //                 }
        //                 catch (Exception)
        //                 {
        //                     //trans.Rollback();
        //                     throw;
        //                 }
        //             }
        //         }
        //     }
        // }
        private ControlesCarga ObterControles(GridView gvNF)
        {
            RepeaterItem item = (RepeaterItem)gvNF.NamingContainer;
            HiddenField hdIdCarga = (HiddenField)item.FindControl("hdIdCarga");

            DropDownList ddlStatus = (DropDownList)item.FindControl("ddlStatus");
            TextBox txtCVA = (TextBox)item.FindControl("txtCVA");

            Label lblPesoCTe = (Label)item.FindControl("lblPesoCTe");
            Label lblPesoCarregadoCTe = (Label)item.FindControl("lblPesoCarregadoCTe");
            Label lblValorMercCTe = (Label)item.FindControl("lblValorMercCTe");
            Label lblTotal = (Label)item.FindControl("lblTotal");
            Label lblCBS = (Label)item.FindControl("lblCBS");
            Label lblIBS = (Label)item.FindControl("lblIBS");
            Label lblICMS = (Label)item.FindControl("lblICMS");
            Label lblTotalFrete = (Label)item.FindControl("lblTotalFrete");


            return new ControlesCarga
            {
                Carga = Convert.ToInt32(hdIdCarga.Value),

                Status = ddlStatus?.SelectedItem.Text,
                CVA = txtCVA?.Text.Trim(),

                GvNF = gvNF,

                LblPesoCTe = lblPesoCTe,
                lblPesoCarregadoCTe = lblPesoCarregadoCTe,
                LblValorMercCTe = lblValorMercCTe,
                LblTotal = lblTotal,
                LblCBS = lblCBS,
                LblIBS = lblIBS,
                LblICMS = lblICMS,
                LblTotalFrete = lblTotalFrete
            };
        }
        private ControlesCarga ObterControles(RepeaterCommandEventArgs e)
        {
            DropDownList ddlStatus = (DropDownList)e.Item.FindControl("ddlStatus");
            TextBox txtCVA = (TextBox)e.Item.FindControl("txtCVA");
            TextBox txtDuracaoPernoite = (TextBox)e.Item.FindControl("txtDuracaoPernoite");
            TextBox txtSaidaOrigem = (TextBox)e.Item.FindControl("txtSaidaOrigem");
            TextBox txtLocalPernoite = (TextBox)e.Item.FindControl("txtLocalPernoite");
            if (ddlStatus.SelectedItem.Text == "Cancelada" || ddlStatus.SelectedItem.Text == "Concluido")
            {
                andamentoCarga = "CONCLUIDO";
            }
            // Krona            
            TextBox txtPrevisaoInicio = (TextBox)e.Item.FindControl("txtPrevisaoInicio");
            TextBox txtPrevisaoTermino = (TextBox)e.Item.FindControl("txtPrevisaoTermino");
            TextBox txtPeso = (TextBox)e.Item.FindControl("txtPeso");
            TextBox txtValorTotal = (TextBox)e.Item.FindControl("txtValorTotal");
            TextBox txtSmEnviadaPor = (TextBox)e.Item.FindControl("txtSmEnviadaPor");
            TextBox txtSM = (TextBox)e.Item.FindControl("txtSM");
            DropDownList ddlPercurso = (DropDownList)e.Item.FindControl("ddlPercurso");
            DropDownList ddlRota = (DropDownList)e.Item.FindControl("ddlRotaKrona");
            int sm = 0;
            int.TryParse(txtSM?.Text, out sm);
            decimal peso;
            decimal.TryParse(
                txtPeso.Text.Replace(".", ","),
                out peso
            );

            // Nota Fiscal
            GridView gvNF = (GridView)e.Item.FindControl("gvNF");
            Label lblPesoCTe = (Label)e.Item.FindControl("lblPesoCTe");
            Label lblValorMercCTe = (Label)e.Item.FindControl("lblValorMercCTe");
            Label lblTotal = (Label)e.Item.FindControl("lblTotal");
            Label lblCBS = (Label)e.Item.FindControl("lblCBS");
            Label lblIBS = (Label)e.Item.FindControl("lblIBS");
            Label lblICMS = (Label)e.Item.FindControl("lblICMS");
            Label lblTotalFrete = (Label)e.Item.FindControl("lblTotalFrete");
            //Label lblPeso = (Label)e.Item.FindControl("lblPeso");


            return new ControlesCarga
            {
                Carga = Convert.ToInt32(e.CommandArgument),
                // Nota Fiscal
                GvNF = gvNF,
                //LblPesoCTe = txtPeso,
                LblValorMercCTe = lblValorMercCTe,
                LblTotal = lblTotal,
                LblCBS = lblCBS,
                LblIBS = lblIBS,
                LblICMS = lblICMS,
                LblTotalFrete = lblTotalFrete,

                // Outros
                Status = ddlStatus.SelectedItem.Text,
                Andamento = andamentoCarga,
                CVA = txtCVA.Text.Trim(),
                statusPernoite = txtDuracaoPernoite.Text,
                SaidaOrigem = SafeDateValue(txtSaidaOrigem.Text),

                LocalPernoite = txtLocalPernoite?.Text.Trim().ToUpper(),
                // Krona
                ValorTotal = SafeDecimalValueNullable(txtValorTotal.Text),
                Percurso = ddlPercurso.SelectedItem.Text,
                RotaKrona = ddlRota.SelectedItem.Text,
                IdRotaKrona = ddlRota.SelectedValue,
                PrevisaoInicioKrona = SafeDateValue(txtPrevisaoInicio.Text),
                PrevisaoTerminoKrona = SafeDateValue(txtPrevisaoTermino.Text),
                SM = sm,
                SmEnviadaPor = txtSmEnviadaPor?.Text.Trim().ToUpper(),
                txtPeso = peso
            };
        }
        private string ObterSqlAtualizacao()
        {
            return @"
            UPDATE tbcargas
            SET
                status = @status,
                andamento = @andamento,
                cva = @cva,
                saidaorigem = @saidaorigem,
                tempoagcarreg = @tempoagcarreg,
                duracao_transp = @duracao_transp,
                tempoagdescarreg = @tempoagdescarreg,
                chegadadestino = @chegadadestino,
                disponivel_solicitacao = @disponivel_solicitacao,
                prev_chegada = @prev_chegada,
                saidaplanta = @saidaplanta,
                codmot = @codmot,
                frota = @frota,
                gate_origem = @gate_origem,
                gate_destino = @gate_destino,
                local_pernoite = @local_pernoite,
                inicio_pernoite = @inicio_pernoite,
                fim_pernoite = @fim_pernoite,
                duracao_pernoite = @duracao_pernoite,
                status_pernoite = @status_pernoite,
                percurso = @percurso,
                valor_total = @valor_total,
                previsao_inicio_krona = @previsao_inicio_krona,
                previsao_termino_krona = @previsao_termino_krona,
                rota_krona = @rota_krona,
                id_rota_krona = @id_rota_krona,
                num_sm=@num_sm,                 
                usu_envio_krona=@usu_envio_krona
               
            WHERE carga = @carga";

            
            
        }

        private string ObterSqlAtualizacaoTbcarregamentos()
        {
            return @"
                   UPDATE tbcarregamentos 
                   SET 
                          situacao = @situacao,
                          cva = @cva,
                          status = @status,                        
                          cod_expedidor = @cod_expedidor,
                          expedidor = @expedidor,
                          cid_expedidor = @cid_expedidor,
                          uf_expedidor = @uf_expedidor,
                          cod_recebedor = @cod_recebedor,
                          recebedor = @recebedor,
                          cid_recebedor = @cid_recebedor,
                          carga = @carga,
                          dtsaida=@dtsaida,
                          dtchegada=@dtchegada,
                          dtconclusao=@dtconclusao,
                          uf_recebedor = @uf_recebedor,
                          dtalt = @dtalt,
                          usualt = @usualt
                      WHERE num_carregamento = @num_carregamento";


        }


        private void InserirMotoristaViagem(SqlConnection con, RepeaterItem item)
        {
            HiddenField hdIdCarga = (HiddenField)item.FindControl("hdIdCarga");

            if (hdIdCarga == null)
                throw new Exception("HiddenField hdIdCarga não encontrado.");

            string carga = hdIdCarga.Value.Trim();

            if (string.IsNullOrEmpty(carga))
                throw new Exception("Número da carga não informado.");

            
            TextBox txtSaidaOrigemViagem = (TextBox)item.FindControl("txtSaidaOrigem");
            TextBox txtChegadaDestinoViagem = (TextBox)item.FindControl("txtChegadaDestino");
            TextBox txtSaidaPlantaViagem = (TextBox)item.FindControl("txtSaidaPlanta");

            TextBox txtCodRecebedor = (TextBox)item.FindControl("txtCodRecebedor");
            TextBox cboRecebedor = (TextBox)item.FindControl("cboRecebedor");
            TextBox txtCidRecebedor = (TextBox)item.FindControl("txtCidRecebedor");
            TextBox txtUFRecebedor = (TextBox)item.FindControl("txtUFRecebedor");

            TextBox txtCodExpedidorViagem = (TextBox)item.FindControl("txtCodExpedidor");
            TextBox cboExpedidorViagem = (TextBox)item.FindControl("cboExpedidor");
            TextBox txtCidExpedidorViagem = (TextBox)item.FindControl("txtCidExpedidor");
            TextBox txtUFExpedidorViagem = (TextBox)item.FindControl("txtUFExpedidor");

            TextBox txtMaterial = (TextBox)item.FindControl("txtMaterial");
            string placa = txtPlaca.Text.Trim();
            string codVeiculo = txtCodVeiculo.Text.Trim();
            string codTransportadora = txtCodTransportadora.Text.Trim();
            string codMotorista = txtCodMotorista.Text.Trim();
            string motorista = ddlMotorista.SelectedItem.Text.Trim();
            string transportadora = txtTransportadora.Text.Trim();

            if (!string.Equals(txtMaterial.Text, "Vazio", StringComparison.OrdinalIgnoreCase) &&
    !string.IsNullOrWhiteSpace(txtSaidaOrigemViagem.Text))
            {
                string sql = @"
            INSERT INTO tbmotoristas_viagens
            (
                carga,
                data,
                material,
                cod_recebedor,
                recebedor,
                cid_recebedor,
                uf_recebedor,
                cod_expedidor,
                expedidor,
                cid_expedidor,
                uf_expedidor,
                cod_motorista,
                motorista,
                cod_veiculo,
                placa,
                cod_transportadora,
                transportadora,
                situacao,
                saida,
                chegada,
                conclusao
            )
            VALUES
            (
                @carga,
                @data,
                @material,
                @cod_recebedor,
                @recebedor,
                @cid_recebedor,
                @uf_recebedor,
                @cod_expedidor,
                @expedidor,
                @cid_expedidor,
                @uf_expedidor,
                @cod_motorista,
                @motorista,
                @cod_veiculo,
                @placa,
                @cod_transportadora,
                @transportadora,
                'Efetuada',
                @saida,
                @chegada,
                @conclusao
            )";

                SqlCommand cmd = new SqlCommand(sql, con);

                cmd.Parameters.AddWithValue("@carga", carga);

                DateTime data;
                if (DateTime.TryParse(txtSaidaOrigemViagem.Text, out data))
                    cmd.Parameters.AddWithValue("@data", data);
                else
                    cmd.Parameters.AddWithValue("@data", DBNull.Value);

                cmd.Parameters.AddWithValue("@material", txtMaterial.Text.Trim());
                cmd.Parameters.AddWithValue("@cod_recebedor", txtCodRecebedor.Text.Trim());
                cmd.Parameters.AddWithValue("@recebedor", cboRecebedor.Text.Trim());
                cmd.Parameters.AddWithValue("@cid_recebedor", txtCidRecebedor.Text.Trim());
                cmd.Parameters.AddWithValue("@uf_recebedor", txtUFRecebedor.Text.Trim());

                cmd.Parameters.AddWithValue("@cod_expedidor", txtCodExpedidorViagem.Text.Trim());
                cmd.Parameters.AddWithValue("@expedidor", cboExpedidorViagem.Text.Trim());
                cmd.Parameters.AddWithValue("@cid_expedidor", txtCidExpedidorViagem.Text.Trim());
                cmd.Parameters.AddWithValue("@uf_expedidor", txtUFExpedidorViagem.Text.Trim());

                cmd.Parameters.AddWithValue("@cod_motorista", codMotorista);
                cmd.Parameters.AddWithValue("@motorista", motorista);

                cmd.Parameters.AddWithValue("@cod_veiculo", codVeiculo);
                cmd.Parameters.AddWithValue("@placa", placa);

                cmd.Parameters.AddWithValue("@cod_transportadora", codTransportadora);
                cmd.Parameters.AddWithValue("@transportadora", transportadora);

                DateTime saida;
                if (DateTime.TryParse(txtSaidaOrigemViagem.Text, out saida))
                    cmd.Parameters.AddWithValue("@saida", saida);
                else
                    cmd.Parameters.AddWithValue("@saida", DBNull.Value);

                DateTime chegada;
                if (DateTime.TryParse(txtChegadaDestinoViagem.Text, out chegada))
                    cmd.Parameters.AddWithValue("@chegada", chegada);
                else
                    cmd.Parameters.AddWithValue("@chegada", DBNull.Value);
               
                DateTime conclusao;
                if (DateTime.TryParse(txtSaidaPlantaViagem.Text, out conclusao))
                    cmd.Parameters.AddWithValue("@conclusao", conclusao);
                else
                    cmd.Parameters.AddWithValue("@conclusao", DBNull.Value);

                cmd.ExecuteNonQuery();

            }


            
        }
        

        private void AtualizarColetaMotorista(RepeaterItem item)
        {            
            HiddenField hdIdCarga = (HiddenField)item.FindControl("hdIdCarga");

            if (hdIdCarga == null)
                throw new Exception("HiddenField hdIdCarga não encontrado.");

            string carga = hdIdCarga.Value.Trim();

            if (string.IsNullOrEmpty(carga))
                throw new Exception("Número da carga não informado.");
                     
            string placa = txtPlaca.Text.Trim();
            string codVeiculo = txtCodVeiculo.Text.Trim();
            string codTransportadora = txtCodTransportadora.Text.Trim();
            string codMotorista = txtCodMotorista.Text.Trim();
            string motorista = ddlMotorista.SelectedItem.Text;
            string transportadora = txtTransportadora.Text.Trim();
            //string material = txtMaterial.Text.Trim();

            TextBox txtMaterial = (TextBox)item.FindControl("txtMaterial");
            TextBox txtSaidaOrigemViagem = (TextBox)item.FindControl("txtSaidaOrigem");
            TextBox txtChegadaDestinoViagem = (TextBox)item.FindControl("txtChegadaDestino");
            TextBox txtSaidaPlantaViagem = (TextBox)item.FindControl("txtSaidaPlanta");

            TextBox txtCodRecebedor = (TextBox)item.FindControl("txtCodRecebedor");
            TextBox cboRecebedor = (TextBox)item.FindControl("cboRecebedor");
            TextBox txtCidRecebedor = (TextBox)item.FindControl("txtCidRecebedor");
            TextBox txtUFRecebedor = (TextBox)item.FindControl("txtUFRecebedor");

            TextBox txtCodExpedidorViagem = (TextBox)item.FindControl("txtCodExpedidor");
            TextBox cboExpedidorViagem = (TextBox)item.FindControl("cboExpedidor");
            TextBox txtCidExpedidorViagem = (TextBox)item.FindControl("txtCidExpedidor");
            TextBox txtUFExpedidorViagem = (TextBox)item.FindControl("txtUFExpedidor");

            if (txtMaterial.Text != "Vazio" && txtMaterial.Text != "VAZIO")
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
                {
                    con.Open();

                    // Procura a carga ativa
                    string sqlBusca = @"SELECT TOP 1
                    id,
                    cod_motorista
                    FROM tbmotoristas_viagens
                    WHERE carga=@carga
                    AND situacao='Efetuada'";

                    SqlCommand cmdBusca = new SqlCommand(sqlBusca, con);
                    cmdBusca.Parameters.AddWithValue("@carga", carga);

                    SqlDataReader dr = cmdBusca.ExecuteReader();

                    if (!dr.Read())
                    {
                        dr.Close();

                        // Não existe a carga
                        InserirMotoristaViagem(con, item);
                        return;
                    }

                    int id = Convert.ToInt32(dr["id"]);
                    string motoristaBanco = dr["cod_motorista"].ToString();

                    dr.Close();

                    // Mesmo motorista -> Atualiza os dados
                    if (motoristaBanco == codMotorista)
                    {
                        string sqlAtualiza = @"
                    UPDATE tbmotoristas_viagens
                    SET
                        data = @data,
                        material = @material,
                        cod_recebedor = @cod_recebedor,
                        recebedor = @recebedor,
                        cid_recebedor = @cid_recebedor,
                        uf_recebedor = @uf_recebedor,
                        cod_expedidor = @cod_expedidor,
                        expedidor = @expedidor,
                        cid_expedidor = @cid_expedidor,
                        uf_expedidor = @uf_expedidor,
                        motorista = @motorista,
                        cod_veiculo = @cod_veiculo,
                        placa = @placa,
                        cod_transportadora = @cod_transportadora,
                        transportadora = @transportadora,
                        saida=@saida,
                        chegada=@chegada,
                        conclusao=@conclusao
                    WHERE id=@id";

                        SqlCommand cmd = new SqlCommand(sqlAtualiza, con);

                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@data",
                            string.IsNullOrWhiteSpace(txtSaidaOrigemViagem.Text)
                                ? (object)DBNull.Value
                                : Convert.ToDateTime(txtSaidaOrigemViagem.Text));
                        cmd.Parameters.AddWithValue("@material", txtMaterial.Text);
                        cmd.Parameters.AddWithValue("@cod_recebedor", txtCodRecebedor.Text);
                        cmd.Parameters.AddWithValue("@recebedor", cboRecebedor.Text);
                        cmd.Parameters.AddWithValue("@cid_recebedor", txtCidRecebedor.Text);
                        cmd.Parameters.AddWithValue("@uf_recebedor", txtUFRecebedor.Text);

                        cmd.Parameters.AddWithValue("@cod_expedidor", txtCodExpedidorViagem.Text);
                        cmd.Parameters.AddWithValue("@expedidor", cboExpedidorViagem.Text);
                        cmd.Parameters.AddWithValue("@cid_expedidor", txtCidExpedidorViagem.Text);
                        cmd.Parameters.AddWithValue("@uf_expedidor", txtUFExpedidorViagem.Text);

                        cmd.Parameters.AddWithValue("@motorista", motorista);

                        cmd.Parameters.AddWithValue("@cod_veiculo", codVeiculo);
                        cmd.Parameters.AddWithValue("@placa", placa);

                        cmd.Parameters.AddWithValue("@cod_transportadora", codTransportadora);
                        cmd.Parameters.AddWithValue("@transportadora", transportadora); 

                        cmd.Parameters.AddWithValue("@saida",
                            string.IsNullOrWhiteSpace(txtSaidaOrigemViagem.Text)
                                ? (object)DBNull.Value
                                : Convert.ToDateTime(txtSaidaOrigemViagem.Text));

                        cmd.Parameters.AddWithValue("@chegada",
                            string.IsNullOrWhiteSpace(txtChegadaDestinoViagem.Text)
                                ? (object)DBNull.Value
                                : Convert.ToDateTime(txtChegadaDestinoViagem.Text));

                        cmd.Parameters.AddWithValue("@conclusao",
                            string.IsNullOrWhiteSpace(txtSaidaPlantaViagem.Text)
                                ? (object)DBNull.Value
                                : Convert.ToDateTime(txtSaidaPlantaViagem.Text));


                        cmd.ExecuteNonQuery();

                        return;
                    }

                    // Motorista diferente -> Cancela o registro anterior
                    string sqlCancela = @"
                UPDATE tbmotoristas_viagens
                SET situacao='Cancelada'
                WHERE id=@id";

                    SqlCommand cmdCancela = new SqlCommand(sqlCancela, con);
                    cmdCancela.Parameters.AddWithValue("@id", id);
                    cmdCancela.ExecuteNonQuery();

                    // Insere o novo registro
                    InserirMotoristaViagem(con, item);
                }

            }


           
        }
        protected void rptColetas_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            GridView gv = (GridView)e.Item.FindControl("gvPedidos");
            //if (e.CommandName == "Atualizar")
            //{
            //    var c = ObterControles(e);
            //    using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            //    {
            //        SqlCommand cmd = new SqlCommand(ObterSqlAtualizacao(), conn);
            //        cmd.Parameters.AddWithValue("@carga", c.Carga);

            //        AdicionarParametrosGerais(cmd, c, andamentoCarga);
            //        AdicionarParametrosPernoite(cmd, c);
            //        AdicionarParametrosKrona(cmd, c);
            //        AdicionarParametrosTbcarregamentos(cmd, c);

            //        conn.Open();

            //        cmd.ExecuteNonQuery();
            //    }

            //    Session["Coletas"] = null;
            //    CarregarColetas(novaColeta.Text);
            //}

            if (e.CommandName == "Atualizar")
            {
                string carga = e.CommandArgument.ToString();

                //Lançar viagem do motorista na tbmotoristas_viagens
                AtualizarColetaMotorista(e.Item);                
               
                GridView gvPedidos = (GridView)e.Item.FindControl("gvPedidos");

                /// Recuperar os controles de dentro do item
                TextBox txtDataHoraColeta = (TextBox)e.Item.FindControl("txtDataHoraColeta");
                TextBox txtVeiculoDisponivel = (TextBox)e.Item.FindControl("txtVeiculoDisponivel");
                TextBox txtGateOrigem = (TextBox)e.Item.FindControl("txtGateOrigem");
                TextBox txtPrevisaoChegada = (TextBox)e.Item.FindControl("txtPrevisaoChegada");
                TextBox txtGateDestino = (TextBox)e.Item.FindControl("txtGateDestino");
                TextBox txtCVA = (TextBox)e.Item.FindControl("txtCVA");
                DropDownList ddlStatus = (DropDownList)e.Item.FindControl("ddlStatus");
                TextBox txtSaidaOrigem = (TextBox)e.Item.FindControl("txtSaidaOrigem");
                TextBox txtAgCarreg = (TextBox)e.Item.FindControl("txtAgCarreg");
                TextBox txtAgDescarga = (TextBox)e.Item.FindControl("txtAgDescarga");
                TextBox txtDurTransp = (TextBox)e.Item.FindControl("txtDurTransp");
                TextBox txtChegadaDestino = (TextBox)e.Item.FindControl("txtChegadaDestino");
                TextBox txtSaidaPlanta = (TextBox)e.Item.FindControl("txtSaidaPlanta");
                Label lblMensagem = (Label)e.Item.FindControl("lblMensagem");
                TextBox txtMaterial = (TextBox)e.Item.FindControl("txtMaterial");

                TextBox txtCodExpedidor = (TextBox)e.Item.FindControl("txtCodExpedidor");
                TextBox cboExpedidor = (TextBox)e.Item.FindControl("cboExpedidor");
                TextBox txtCidExpedidor = (TextBox)e.Item.FindControl("txtCidExpedidor");
                TextBox txtUFExpedidor = (TextBox)e.Item.FindControl("txtUFExpedidor");

                TextBox txtCodRecebedor = (TextBox)e.Item.FindControl("txtCodRecebedor");
                TextBox cboRecebedor = (TextBox)e.Item.FindControl("cboRecebedor");
                TextBox txtCidRecebedor = (TextBox)e.Item.FindControl("txtCidRecebedor");
                TextBox txtUFRecebedor = (TextBox)e.Item.FindControl("txtUFRecebedor");

                TextBox txtLocalPernoite = (TextBox)e.Item.FindControl("txtLocalPernoite");
                TextBox txtInicioPernoite = (TextBox)e.Item.FindControl("txtInicioPernoite");
                TextBox txtFimPernoite = (TextBox)e.Item.FindControl("txtFimPernoite");
                TextBox txtDuracaoP = (TextBox)e.Item.FindControl("txtDuracaoP");

                // Recuperar os TextBoxes de UF
                TextBox txtUfInicio = (TextBox)e.Item.FindControl("txtUFExpedidor");
                TextBox txtUfFim = (TextBox)e.Item.FindControl("txtUFRecebedor");

                DropDownList ddlPercursoKrona = (DropDownList)e.Item.FindControl("ddlPercurso");
                DropDownList ddlRotaKrona = (DropDownList)e.Item.FindControl("ddlRotaKrona");
                TextBox pesoKrona = (TextBox)e.Item.FindControl("txtPeso");
                TextBox valorKrona = (TextBox)e.Item.FindControl("txtValorTotal");
                DateTime? previsaoInicial = SafeDateValue(e.Item, "txtPrevisaoInicio");
                DateTime? previsaoFinal = SafeDateValue(e.Item, "txtPrevisaoTermino");
                string percursoKrona = ddlPercursoKrona.SelectedItem.Text;
                string rotaKrona = ddlRotaKrona.SelectedItem.Text;
                string id_rotaKrona = ddlRotaKrona.SelectedValue;

                DateTime agora = DateTime.Now;

            //Exemplo: atualizando no banco
                using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
                {
                   // Extrai os valores uma única vez
                    var saida = SafeDateValue2(txtSaidaOrigem.Text.Trim());
                    var saidaPlanta = SafeDateValue2(txtSaidaPlanta.Text.Trim());
                    var chegada = SafeDateValue2(txtChegadaDestino.Text.Trim());
                    var disponivel_solicitacao = SafeDateValue2(txtVeiculoDisponivel.Text.Trim());
                    var cvaOC = txtCVA.Text.Trim();

                    var CodExpedidor = SafeDateValue2(txtCodExpedidor.Text.Trim());
                    var Expedidor = SafeDateValue2(cboExpedidor.Text.Trim());
                    var Cid_Expedidor = SafeDateValue2(txtCidExpedidor.Text.Trim());
                    var UFExpedidor = SafeDateValue2(txtUFExpedidor.Text.Trim());

                    var CodRecebedor = SafeDateValue2(txtCodRecebedor.Text.Trim());
                    var Recebedor = SafeDateValue2(cboRecebedor.Text.Trim());
                    var CidRecebedor = SafeDateValue2(txtCidRecebedor.Text.Trim());
                    var UFRecebedor = SafeDateValue2(txtUFRecebedor.Text.Trim());
                    var localPernoite = SafeDateValue2(txtLocalPernoite.Text.Trim());
                    var inicioPernoite = SafeDateValue2(txtInicioPernoite.Text.Trim());
                    var fimPernoite = SafeDateValue2(txtFimPernoite.Text.Trim());
                    var duracaoP = txtDuracaoP.Text.Trim();

                    if (chegada != null && saida != null && saidaPlanta != null)
                    {
                        statusOC = "Concluido";
                        situacaoOC = "Pronto";
                        andamentoCarga = "Entregue";
                    }
                    else if (chegada != null && saida != null)
                    {
                        statusOC = "Ag. Descarga";
                        situacaoOC = "EM ANDAMENTO";
                        andamentoCarga = "Em Andamento";
                    }
                    else if (chegada != null && saida != null)
                    {
                        statusOC = "Em Transito";
                        situacaoOC = "EM ANDAMENTO";
                        andamentoCarga = "Em Andamento";
                    }
                    else if (chegada != null)
                    {
                        statusOC = "Ag. Carregamento";
                        situacaoOC = "EM ANDAMENTO";
                        andamentoCarga = "Em Andamento";
                    }
                    else
                    {
                        statusOC = "Pendente";
                        situacaoOC = "PROGRAMADA";
                        andamentoCarga = "Programada";
                    }
                    if (ddlStatus.SelectedItem.Text != "Pendente" || ddlStatus.SelectedItem.Text != "Concluido")
                    {
                        situacaoOC = "EM ANDAMENTO";
                        andamentoCarga = "Em Andamento";
                        statusOC = ddlStatus.SelectedItem.Text.Trim();

                    }
                    if (ddlStatus.SelectedItem.Text == "Pendente")
                    {
                        situacaoOC = "PROGRAMADA";
                        andamentoCarga = "Programada";
                        statusOC = ddlStatus.SelectedItem.Text.Trim();

                    }

                    string duracao = txtDuracaoP.Text.Trim();

                    string statusPernoite = "";

                    if (!string.IsNullOrEmpty(duracao))
                    {
                        TimeSpan tsDuracao = TimeSpan.Parse(duracao);
                        TimeSpan tsLimite = new TimeSpan(11, 0, 0);

                        if (tsDuracao < tsLimite)
                        {
                            statusPernoite = "Não Cumpriu a Jornada de 11h";
                        }
                        else
                        {
                            statusPernoite = "Cumpriu Jornada de 11h";
                        }
                    }

                    string query = @"UPDATE tbcargas SET
                        gate_origem = @gate_origem,
                        gate_destino = @gate_destino,
                        status = @status,
                        cva = @cva,
                        andamento = @andamento,
                        saidaorigem = @saidaorigem,
                        tempoagcarreg = @tempoagcarreg,
                        chegadadestino = @chegadadestino,
                        saidaplanta = @saidaplanta,
                        prev_chegada = @prev_chegada,
                        tempoagdescarreg = @tempoagdescarreg,
                        duracao_transp = @duracao_transp,
                        disponivel_solicitacao = @disponivel_solicitacao,
                        codmot = @codmot,
                        local_pernoite = @local_pernoite,
                        inicio_pernoite = @inicio_pernoite,
                        fim_pernoite = @fim_pernoite,
                        duracao_pernoite = @duracao_pernoite,
                        status_pernoite = @status_pernoite,
                        frota = @frota
                    WHERE carga = @carga";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@gate_origem", (object)SafeDateValue(txtGateOrigem.Text.Trim()) ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@gate_destino", (object)SafeDateValue(txtGateDestino.Text.Trim()) ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@status", ddlStatus.SelectedItem.Text);
                        cmd.Parameters.AddWithValue("@cva", txtCVA.Text.Trim());
                        cmd.Parameters.AddWithValue("@andamento", andamentoCarga);

                        cmd.Parameters.AddWithValue("@saidaorigem", (object)SafeDateValue(txtSaidaOrigem.Text.Trim()) ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@tempoagcarreg", SafeValue(txtAgCarreg.Text.Trim()));
                        cmd.Parameters.AddWithValue("@chegadadestino", (object)SafeDateValue(txtChegadaDestino.Text.Trim()) ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@saidaplanta", (object)SafeDateValue(txtSaidaPlanta.Text.Trim()) ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@prev_chegada", (object)SafeDateValue(txtPrevisaoChegada.Text.Trim()) ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@tempoagdescarreg", SafeValue(txtAgDescarga.Text.Trim()));
                        cmd.Parameters.AddWithValue("@duracao_transp", SafeValue(txtDurTransp.Text.Trim()));
                        cmd.Parameters.AddWithValue("@disponivel_solicitacao", (object)SafeDateValue(txtVeiculoDisponivel.Text.Trim()) ?? DBNull.Value);

                        cmd.Parameters.AddWithValue("@codmot",
                            string.IsNullOrWhiteSpace(txtCodMotorista.Text)
                                ? (object)DBNull.Value
                                : txtCodMotorista.Text.Trim());

                        cmd.Parameters.AddWithValue("@local_pernoite", txtLocalPernoite.Text.Trim().ToUpper());

                        cmd.Parameters.AddWithValue("@inicio_pernoite", (object)SafeDateValue(txtInicioPernoite.Text.Trim()) ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@fim_pernoite", (object)SafeDateValue(txtFimPernoite.Text.Trim()) ?? DBNull.Value);

                        cmd.Parameters.AddWithValue("@duracao_pernoite", txtDuracaoP.Text.Trim());
                        cmd.Parameters.AddWithValue("@status_pernoite", statusPernoite);

                        cmd.Parameters.AddWithValue("@frota",
                            string.IsNullOrWhiteSpace(txtCodVeiculo.Text)
                                ? (object)DBNull.Value
                                : txtCodVeiculo.Text.Trim());

                        cmd.Parameters.AddWithValue("@carga", carga);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }

                    // TRATANDO O PREMIO AUTOMATICO DO MOTORISTA
                    if (ddlStatus.SelectedItem.Text == "Concluido" && txtMaterial.Text != "Vazio" && txtMaterial.Text != "Embalagem")
                    {
                       // SOMENTE FUNCIONÁRIO
                        if (txtTipoMot.Text.Trim().ToUpper() == "FUNCIONÁRIO")
                        {
                            using (SqlConnection conn2 = new SqlConnection(
                                WebConfigurationManager
                                .ConnectionStrings["conexao"].ConnectionString))
                            {
                                conn2.Open();

                                SqlTransaction trans = conn2.BeginTransaction();

                                try
                                {
                                  //   ============================================
                                 //    FUNÇÃO
                                 //    ============================================
                                    string funcao = "";

                                    if (!string.IsNullOrWhiteSpace(txtFuncao.Text))
                                    {
                                        funcao = txtFuncao.Text
                                            .Trim()
                                            .Split(' ')[0]
                                            .ToUpper();
                                    }

                                    decimal distancia = 0;
                                    decimal valorPremio = 0;

                                 //    ============================================
                                 //    BUSCA DISTÂNCIA
                                  //   ============================================
                                    using (SqlCommand cmdDist = new SqlCommand(@"
                                      SELECT TOP 1 distancia
                                      FROM tbcargas
                                      WHERE carga = @carga",
                                        conn2, trans))
                                    {
                                        cmdDist.Parameters.AddWithValue("@carga", carga);

                                        object result = cmdDist.ExecuteScalar();

                                        if (result != null &&
                                            result != DBNull.Value)
                                        {
                                            distancia =
                                                Convert.ToDecimal(result);
                                        }
                                        else
                                        {
                                            trans.Rollback();
                                            return;
                                        }
                                    }

                                 //    ============================================
                                 //    BUSCA VALOR PRÊMIO
                                 //    ============================================
                                    using (SqlCommand cmdPremio = new SqlCommand(@"
                                      SELECT TOP 1
                                          motorista,
                                          carreteiro,
                                          bitrem
                                      FROM tbvalorpremiomotoristas
                                      WHERE @distancia
                                      BETWEEN distancia1 AND distancia2 AND status = 'ATIVO'AND empresa='MATRIZ'",
                                        conn2, trans))
                                    {
                                        cmdPremio.Parameters.AddWithValue(
                                            "@distancia",
                                            distancia);

                                        using (SqlDataReader dr =
                                            cmdPremio.ExecuteReader())
                                        {
                                            if (dr.Read())
                                            {
                                                switch (funcao)
                                                {
                                                    case "MOTORISTA":

                                                        valorPremio =
                                                            dr["motorista"] != DBNull.Value
                                                            ? Convert.ToDecimal(dr["motorista"])
                                                            : 0;

                                                        break;

                                                    case "CARRETEIRO":

                                                        valorPremio =
                                                            dr["carreteiro"] != DBNull.Value
                                                            ? Convert.ToDecimal(dr["carreteiro"])
                                                            : 0;

                                                        break;

                                                    case "BITREM":

                                                        valorPremio =
                                                            dr["bitrem"] != DBNull.Value
                                                            ? Convert.ToDecimal(dr["bitrem"])
                                                            : 0;

                                                        break;
                                                }
                                            }
                                        }
                                    }

                                   //  ============================================
                                   //  VERIFICA EXISTÊNCIA
                                   //  ============================================
                                    using (SqlCommand cmdExiste = new SqlCommand(@"
                                      SELECT COUNT(*)
                                      FROM tb_custo_motorista
                                      WHERE cod_cracha = @cod
                                      AND dt_custo = @data",
                                        conn2, trans))
                                    {
                                        cmdExiste.Parameters.AddWithValue(
                                            "@cod",
                                            txtCodMotorista.Text.Trim());

                                        cmdExiste.Parameters.AddWithValue(
                                            "@data",
                                            SafeDateValue(txtSaidaPlanta.Text.Trim()));

                                        int existe =
                                            Convert.ToInt32(
                                                cmdExiste.ExecuteScalar());

                                     //    ============================================
                                     //    UPDATE
                                      //   ============================================
                                        if (existe > 0)
                                        {
                                            using (SqlCommand cmdUpdate =
                                                new SqlCommand(@"
                                              UPDATE tb_custo_motorista
                                              SET vl_premio =
                                                  ISNULL(vl_premio,0) + @valor
                                              WHERE cod_cracha = @cod
                                              AND dt_custo = @data",
                                                conn2, trans))
                                            {
                                                cmdUpdate.Parameters.AddWithValue(
                                                    "@valor",
                                                    valorPremio);

                                                cmdUpdate.Parameters.AddWithValue(
                                                    "@cod",
                                                    txtCodMotorista.Text.Trim());

                                                cmdUpdate.Parameters.AddWithValue(
                                                    "@data",
                                                    SafeDateValue(
                                                        txtSaidaPlanta.Text.Trim()));

                                                cmdUpdate.ExecuteNonQuery();
                                            }
                                        }
                                        else
                                        {
                                          //   ============================================
                                         //    INSERT
                                         //    ============================================
                                            using (SqlCommand cmdInsert =
                                                new SqlCommand(@"
                                                  INSERT INTO tb_custo_motorista
                                                  (
                                                      cod_cracha,
                                                      dt_custo,
                                                      vl_premio
                                                  )
                                                  VALUES
                                                  (
                                                      @cod,
                                                      @data,
                                                      @valor
                                                  )",
                                                conn2, trans))
                                            {
                                                cmdInsert.Parameters.AddWithValue(
                                                    "@cod",
                                                    txtCodMotorista.Text.Trim());

                                                cmdInsert.Parameters.AddWithValue(
                                                    "@data",
                                                    SafeDateValue(
                                                        txtSaidaPlanta.Text.Trim()));

                                                cmdInsert.Parameters.AddWithValue(
                                                    "@valor",
                                                    valorPremio);

                                                cmdInsert.ExecuteNonQuery();
                                            }
                                        }
                                    }

                                    trans.Commit();
                                }
                                catch (Exception ex)
                                {
                                    trans.Rollback();

                                    MostrarMsg(
                                        "Erro prêmio motorista: "
                                        + ex.Message);
                                }
                            }
                        }
                    }

                   // TRATANDO A PERNOITE AUTOMATICA DO MOTORISTA
                    //DateTime dataPernoite;
                    //if (agora.TimeOfDay >= new TimeSpan(19, 0, 0) &&
                    //    agora.TimeOfDay <= new TimeSpan(23, 59, 59))
                    //{
                    //    dataPernoite = agora.Date;
                    //    if (ddlStatus.SelectedItem.Text.Trim() == "Pernoite")
                    //    {
                    //       // SOMENTE FUNCIONÁRIO
                    //        if (txtTipoMot.Text.Trim().ToUpper() == "FUNCIONÁRIO")
                    //        {
                    //            using (SqlConnection conn2 = new SqlConnection(
                    //                WebConfigurationManager
                    //                .ConnectionStrings["conexao"].ConnectionString))
                    //            {
                    //                conn2.Open();

                    //                SqlTransaction trans = conn2.BeginTransaction();

                    //                try
                    //                {
                    //                  //   ============================================
                    //                  //   VALOR PERNOITE
                    //                   //  ============================================
                    //                    decimal valorPernoite = 0;
                    //                    decimal valorCafe = 0;

                    //                    using (SqlCommand cmdPernoite =
                    //                        new SqlCommand(@"
                    //                      SELECT TOP 1 pernoite
                    //                      FROM tbvalorpremiomotoristas WHERE status = 'ATIVO' and empresa='MATRIZ'",
                    //                        conn2, trans))
                    //                    {
                    //                        object result =
                    //                            cmdPernoite.ExecuteScalar();

                    //                        if (result != null &&
                    //                            result != DBNull.Value)
                    //                        {
                    //                            valorPernoite =
                    //                                Convert.ToDecimal(result);
                    //                        }
                    //                    }

                    //                  //   ============================================
                    //                  //   DATA
                    //                  //   ============================================
                    //                    object dataCusto = dataPernoite;

                    //                    if (dataCusto == null ||
                    //                        dataCusto == DBNull.Value)
                    //                    {
                    //                        trans.Rollback();

                    //                        MostrarMsg(
                    //                            "Data Pernoite inválida.");

                    //                        return;
                    //                    }

                    //                   //  ============================================
                    //                   //  VERIFICA EXISTÊNCIA
                    //                   //  ============================================
                    //                    int existe = 0;

                    //                    using (SqlCommand cmdExiste =
                    //                        new SqlCommand(@"
                    //                      SELECT COUNT(*)
                    //                      FROM tb_custo_motorista
                    //                      WHERE cod_cracha = @cod
                    //                      AND dt_custo = @data",
                    //                        conn2, trans))
                    //                    {
                    //                        cmdExiste.Parameters.AddWithValue(
                    //                            "@cod",
                    //                            txtCodMotorista.Text.Trim());

                    //                        cmdExiste.Parameters.AddWithValue(
                    //                            "@data",
                    //                            dataCusto);

                    //                        existe =
                    //                            Convert.ToInt32(
                    //                                cmdExiste.ExecuteScalar());
                    //                    }

                    //                    // ============================================
                    //                    // UPDATE
                    //                    // ============================================
                    //                    if (existe > 0)
                    //                    {
                    //                        using (SqlCommand cmdUpdate =
                    //                            new SqlCommand(@"
                    //                          UPDATE tb_custo_motorista
                    //                          SET vl_pernoite = @valor, vl_cafe = @cafe
                    //                          WHERE cod_cracha = @cod
                    //                          AND dt_custo = @data",
                    //                            conn2, trans))
                    //                        {
                    //                            cmdUpdate.Parameters.AddWithValue(
                    //                                "@valor",
                    //                                valorPernoite);

                    //                            cmdUpdate.Parameters.AddWithValue(
                    //                                "@cafe",
                    //                                valorCafe);

                    //                            cmdUpdate.Parameters.AddWithValue(
                    //                                "@cod",
                    //                                txtCodMotorista.Text.Trim());

                    //                            cmdUpdate.Parameters.AddWithValue(
                    //                                "@data",
                    //                                dataCusto);

                    //                            cmdUpdate.ExecuteNonQuery();
                    //                        }
                    //                    }
                    //                    else
                    //                    {
                    //                      //   ============================================
                    //                      //   INSERT
                    //                      //   ============================================
                    //                        using (SqlCommand cmdInsert =
                    //                            new SqlCommand(@"
                    //                          INSERT INTO tb_custo_motorista
                    //                          (
                    //                              cod_cracha,
                    //                              dt_custo,
                    //                              vl_pernoite,
                    //                              vl_cafe
                    //                          )
                    //                          VALUES
                    //                          (
                    //                              @cod,
                    //                              @data,
                    //                              @valor,
                    //                              @cafe
                    //                          )",
                    //                            conn2, trans))
                    //                        {
                    //                            cmdInsert.Parameters.AddWithValue(
                    //                                "@cod",
                    //                                txtCodMotorista.Text.Trim());

                    //                            cmdInsert.Parameters.AddWithValue(
                    //                                "@data",
                    //                                dataCusto);

                    //                            cmdInsert.Parameters.AddWithValue(
                    //                                "@valor",
                    //                                valorPernoite);

                    //                            cmdInsert.Parameters.AddWithValue(
                    //                                "@cafe",
                    //                                valorCafe);

                    //                            cmdInsert.ExecuteNonQuery();
                    //                        }
                    //                    }

                    //                    trans.Commit();

                    //                    MostrarMsg(
                    //                        "Pernoite salvo com sucesso.");
                    //                }
                    //                catch (Exception ex)
                    //                {
                    //                    trans.Rollback();

                    //                    MostrarMsg(
                    //                        "Erro pernoite: "
                    //                        + ex.Message);
                    //                }
                    //            }
                    //        }
                    //    }
                    //}

                   // TRATANDO O ALMOÇO AUTOMATICO DO MOTORISTA

                }

               // Atualizando a ordem de coleta
                using (SqlConnection conn = new SqlConnection(
       WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
                {
                    var cvaOC = txtCVA.Text.Trim();

                    string queryCarregamento = @"
                      UPDATE tbcarregamentos SET 
                          situacao = @situacao,
                          cva = @cva,
                          status = @status,                        
                          cod_expedidor = @cod_expedidor,
                          expedidor = @expedidor,
                          cid_expedidor = @cid_expedidor,
                          uf_expedidor = @uf_expedidor,
                          cod_recebedor = @cod_recebedor,
                          recebedor = @recebedor,
                          cid_recebedor = @cid_recebedor,
                          carga = @carga,
                          dtsaida=@dtsaida,
                          dtchegada=@dtchegada,
                          dtconclusao=@dtconclusao,
                          uf_recebedor = @uf_recebedor,
                          dtalt = @dtalt,
                          usualt = @usualt

                      WHERE num_carregamento = @num_carregamento";

                    using (SqlCommand cmdCarregamento = new SqlCommand(queryCarregamento, conn))
                    {
                        //Num carregamento(obrigatório)
                        cmdCarregamento.Parameters.Add("@num_carregamento", SqlDbType.NVarChar, 20)
                            .Value = novaColeta.Text.Trim();

                        //Status
                        cmdCarregamento.Parameters.Add("@situacao", SqlDbType.NVarChar, 50)
                            .Value = string.IsNullOrWhiteSpace(situacaoOC)
                                ? (object)DBNull.Value
                                : situacaoOC.Trim();

                        cmdCarregamento.Parameters.Add("@cva", SqlDbType.NVarChar, 50)
                            .Value = string.IsNullOrWhiteSpace(cvaOC)
                                ? (object)DBNull.Value
                                : cvaOC.Trim();

                        cmdCarregamento.Parameters.Add("@status", SqlDbType.NVarChar, 50)
                            .Value = string.IsNullOrWhiteSpace(statusOC)
                                ? (object)DBNull.Value
                                : statusOC.Trim();
                        int cargaCarreg;
                        cmdCarregamento.Parameters.Add("@carga", SqlDbType.Int)
                            .Value = int.TryParse(carga, out cargaCarreg)
                                ? (object)cargaCarreg
                                : (object)DBNull.Value;

                       // Expedidor
                        int codExp;
                        cmdCarregamento.Parameters.Add("@cod_expedidor", SqlDbType.Int)
                            .Value = int.TryParse(txtCodExpedidor.Text, out codExp)
                                ? (object)codExp
                                : (object)DBNull.Value;

                        cmdCarregamento.Parameters.Add("@expedidor", SqlDbType.NVarChar, 100)
                            .Value = string.IsNullOrWhiteSpace(cboExpedidor.Text)
                                ? (object)DBNull.Value
                                : cboExpedidor.Text.Trim();

                        cmdCarregamento.Parameters.Add("@cid_expedidor", SqlDbType.NVarChar, 100)
                            .Value = string.IsNullOrWhiteSpace(txtCidExpedidor.Text)
                                ? (object)DBNull.Value
                                : txtCidExpedidor.Text.Trim();

                        cmdCarregamento.Parameters.Add("@uf_expedidor", SqlDbType.NVarChar, 2)
                            .Value = string.IsNullOrWhiteSpace(txtUFExpedidor.Text)
                                ? (object)DBNull.Value
                                : txtUFExpedidor.Text.Trim();

                       // Recebedor
                        int codRec;
                        cmdCarregamento.Parameters.Add("@cod_recebedor", SqlDbType.Int)
                            .Value = int.TryParse(txtCodRecebedor.Text, out codRec)
                                ? (object)codRec
                                : (object)DBNull.Value;

                        cmdCarregamento.Parameters.Add("@recebedor", SqlDbType.NVarChar, 100)
                            .Value = string.IsNullOrWhiteSpace(cboRecebedor.Text)
                                ? (object)DBNull.Value
                                : cboRecebedor.Text.Trim();

                        cmdCarregamento.Parameters.Add("@cid_recebedor", SqlDbType.NVarChar, 100)
                            .Value = string.IsNullOrWhiteSpace(txtCidRecebedor.Text)
                                ? (object)DBNull.Value
                                : txtCidRecebedor.Text.Trim();
                        cmdCarregamento.Parameters.Add("@dtsaida", SqlDbType.DateTime)
    .Value = SafeDateValue(txtSaidaOrigem.Text.Trim()) ?? (object)DBNull.Value;

                        cmdCarregamento.Parameters.Add("@dtchegada", SqlDbType.DateTime)
                            .Value = SafeDateValue(txtChegadaDestino.Text.Trim()) ?? (object)DBNull.Value;

                        cmdCarregamento.Parameters.Add("@dtconclusao", SqlDbType.DateTime)
                            .Value = SafeDateValue(txtSaidaPlanta.Text.Trim()) ?? (object)DBNull.Value;

                        cmdCarregamento.Parameters.Add("@uf_recebedor", SqlDbType.NVarChar, 2)
                            .Value = string.IsNullOrWhiteSpace(txtUFRecebedor.Text)
                                ? (object)DBNull.Value
                                : txtUFRecebedor.Text.Trim();
                        cmdCarregamento.Parameters.AddWithValue("@dtalt", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                        cmdCarregamento.Parameters.Add("@usualt", SqlDbType.NVarChar, 50)
    .Value = Session["UsuarioLogado"] == null
        ? (object)DBNull.Value
        : Session["UsuarioLogado"].ToString();

                        conn.Open();
                        cmdCarregamento.ExecuteNonQuery();
                    }
                }

                using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
                {
                    TextBox txtRedes = (TextBox)e.Item.FindControl("txtRedes");
                    TextBox txtCatracas = (TextBox)e.Item.FindControl("txtCatracas");
                    TextBox txtOT = (TextBox)e.Item.FindControl("txtOT");


                    string query = @"UPDATE tbpedidos SET                                  
                                  andamento = @andamento
                                  WHERE carga = @carga";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@carga", carga);
                    cmd.Parameters.AddWithValue("@andamento", andamentoCarga);



                   // Chama método que verifica no banco
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }





               // Após atualizar, recarregar os dados no Repeater
                Session["Coletas"] = null;
                CarregarColetas(novaColeta.Text);
                BuscarCteSalvos(carga);  // idViagem
                CarregarPedidos(int.Parse(carga), gv);
            }

            if (e.CommandName == "AtualizarAbas")
            {
                //Atualiza CT-e
                string carga = e.CommandArgument.ToString();
                string idViagem = e.CommandArgument.ToString(); // O 'carga' que você passou no Eval
                int index = e.Item.ItemIndex; // O índice da linha no Repeater
                string nomeUsuario = Session["UsuarioLogado"].ToString();
                // 1. Verificar se existem CT-es lidos na Session para este item
                if (ListaCtePorItem.ContainsKey(index) && ListaCtePorItem[index].Count > 0)
                {
                    List<CteLido> listaParaSalvar = ListaCtePorItem[index];

                    try
                    {
                        if (con.State == ConnectionState.Closed) con.Open();
                        SqlTransaction trans = con.BeginTransaction();

                        try
                        {
                            foreach (var cte in listaParaSalvar)
                            {
                                string sql = @"INSERT INTO tbcte 
                             (chave_de_acesso, uf_emissor, cnpj_empresa, empresa_emissora, 
                              num_documento, serie_documento, tipo_documento,mes_ano_documento,emitido_por, emissao_documento, id_viagem)
                             VALUES 
                             (@chave, @uf, @cnpj, @empresa, @num, @serie, @tipo,@mes_ano_documento, @emitido_por, @emissao_documento, @idViagem)";

                                using (SqlCommand cmd = new SqlCommand(sql, con, trans))
                                {
                                    // Extraímos o CNPJ (posições 7 a 20 da chave de 44 dígitos)
                                    string cnpjExtraido = cte.ChaveOriginal.Substring(6, 14);

                                    cmd.Parameters.AddWithValue("@chave", cte.ChaveOriginal);
                                    cmd.Parameters.AddWithValue("@uf", cte.Estado);
                                    cmd.Parameters.AddWithValue("@cnpj", cnpjExtraido);
                                    cmd.Parameters.AddWithValue("@empresa", cte.Filial);
                                    cmd.Parameters.AddWithValue("@num", cte.Numero);
                                    cmd.Parameters.AddWithValue("@serie", cte.Serie);
                                    cmd.Parameters.AddWithValue("@tipo", "CT-e");
                                    cmd.Parameters.AddWithValue("@mes_ano_documento", cte.Emissao);
                                    cmd.Parameters.AddWithValue("@emitido_por", nomeUsuario);
                                    cmd.Parameters.AddWithValue("@emissao_documento", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.000"));
                                    cmd.Parameters.AddWithValue("@idViagem", carga);

                                    cmd.ExecuteNonQuery();
                                }
                            }

                            trans.Commit();

                            // 2. Limpar a Session deste índice específico
                            ListaCtePorItem.Remove(index);

                            // 3. Sucesso!
                            MostrarMsg("Sucesso: " + listaParaSalvar.Count + " documentos salvos.");

                            // 4. IMPORTANTE: Rebindar o Repeater. 
                            // Isso fará com que o ItemDataBound rode novamente, 
                            // alimentando o GridView com os dados que agora estão no Banco.
                            rptColetas.DataBind();
                        }
                        catch (Exception ex)
                        {
                            string erroLimpo = ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "");
                            MostrarMsg("ERRO REAL: " + erroLimpo);
                        }
                    }
                    finally
                    {
                        con.Close();
                    }
                }
                else
                {
                    MostrarMsg("Aviso: Nenhuma nova leitura pendente para salvar.");
                }
                if (gv != null)
                {
                    try
                    {
                        if (con.State == ConnectionState.Closed) con.Open();
                        SqlTransaction trans = con.BeginTransaction();

                        try
                        {
                            foreach (GridViewRow linha in gv.Rows)
                            {
                                // 1. Pegar as chaves da Grid
                                // Index 0 = pedido, Index 1 = id_viagem (conforme definido no DataKeyNames)
                                string numPedido = gv.DataKeys[linha.RowIndex].Values[0].ToString();


                                // 2. Localizar os controles da linha
                                TextBox txtIni = (TextBox)linha.FindControl("txtInicioCar");
                                TextBox txtFim = (TextBox)linha.FindControl("txtTermCar");
                                TextBox txtDur = (TextBox)linha.FindControl("txtTempoTotal");
                                DropDownList ddlMot = (DropDownList)linha.FindControl("ddlMotCar");

                                // 3. Query de Update
                                string sql = @"UPDATE tbpedidos SET 
                                        iniciocar = @ini, 
                                        termcar = @fim, 
                                        duracao = @dur,
                                        motcar = @mot
                                        WHERE pedido = @pedido ";

                                using (SqlCommand cmd = new SqlCommand(sql, con, trans))
                                {
                                    DateTime dtIni, dtFim;
                                    if (DateTime.TryParse(txtIni.Text, out dtIni))
                                        cmd.Parameters.AddWithValue("@ini", dtIni);
                                    else
                                        cmd.Parameters.AddWithValue("@ini", DBNull.Value);

                                    if (DateTime.TryParse(txtFim.Text, out dtFim))
                                        cmd.Parameters.AddWithValue("@fim", dtFim);
                                    else
                                        cmd.Parameters.AddWithValue("@fim", DBNull.Value);

                                    cmd.Parameters.AddWithValue("@dur", txtDur.Text);
                                    cmd.Parameters.AddWithValue("@mot", ddlMot.SelectedItem.Text);
                                    cmd.Parameters.AddWithValue("@pedido", numPedido);
                                    //cmd.Parameters.AddWithValue("@idViagem", idViagem);

                                    cmd.ExecuteNonQuery();
                                }
                            }

                            trans.Commit();
                            MostrarMsg("Alterações salvas com sucesso!");
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            throw ex;
                        }
                    }
                    catch (Exception ex)
                    {
                        MostrarMsg("Erro ao salvar pedidos: " + ex.Message);
                    }
                    finally
                    {
                        con.Close();
                    }
                }

                //Salvar historico
                TextBox txtHistoricoObservacao = (TextBox)e.Item.FindControl("txtHistoricoObservacao");
                if (txtHistoricoObservacao.Text != string.Empty)
                {
                    try
                    {
                        if (con.State == ConnectionState.Closed) con.Open();
                        SqlTransaction trans = con.BeginTransaction();

                        try
                        {
                            string sql = @"UPDATE tbcargas SET 
                                        observacao = @observacao 
                                        WHERE carga = @carga ";

                            using (SqlCommand cmd = new SqlCommand(sql, con, trans))
                            {
                                cmd.Parameters.AddWithValue("@observacao", txtHistoricoObservacao.Text);
                                cmd.Parameters.AddWithValue("@carga", carga);
                                cmd.ExecuteNonQuery();
                            }
                            trans.Commit();
                            MostrarMsg("Alterações salvas com sucesso!");
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            throw ex;
                        }
                    }
                    catch (Exception ex)
                    {
                        MostrarMsg("Erro ao salvar Observacao: " + ex.Message);
                    }
                    finally
                    {
                        con.Close();
                    }
                }

                // Salvar MDFe
                TextBox txtMDFe = (TextBox)e.Item.FindControl("txtMDFe");
                if (txtMDFe.Text != string.Empty)
                {
                    try
                    {
                        if (con.State == ConnectionState.Closed) con.Open();
                        SqlTransaction trans = con.BeginTransaction();

                        try
                        {
                            string sql = @"UPDATE tbcargas SET 
                                        mdfe = @mdfe 
                                        WHERE carga = @carga ";

                            using (SqlCommand cmd = new SqlCommand(sql, con, trans))
                            {

                                cmd.Parameters.AddWithValue("@mdfe", txtMDFe.Text);
                                cmd.Parameters.AddWithValue("@carga", carga);
                                //cmd.Parameters.AddWithValue("@idViagem", idViagem);

                                cmd.ExecuteNonQuery();
                            }
                            trans.Commit();
                            MostrarMsg("Alterações salvas com sucesso!");
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            throw ex;
                        }
                    }
                    catch (Exception ex)
                    {
                        MostrarMsg("Erro ao salvar MDFe: " + ex.Message);
                    }
                    finally
                    {
                        con.Close();
                    }
                }

                // Salvar NFS-e
                TextBox txtNFSe = (TextBox)e.Item.FindControl("txtNFSe");
                if (txtNFSe != null && !string.IsNullOrWhiteSpace(txtNFSe.Text))
                {
                    try
                    {
                        if (con.State == ConnectionState.Closed) con.Open();
                        string sqlExiste = "SELECT COUNT(1) FROM tbnfse WHERE num_documento = @num_documento";

                        using (SqlCommand cmdExiste = new SqlCommand(sqlExiste, con))
                        {
                            cmdExiste.Parameters.AddWithValue("@num_documento", txtNFSe.Text);
                            int existe = Convert.ToInt32(cmdExiste.ExecuteScalar());
                            if (existe > 0)
                            {
                                MostrarMsg("Já existe uma NFS-e cadastrada com esta chave:\n" + txtNFSe.Text);
                                return; // ⛔ não continua o método
                            }
                        }
                        using (SqlTransaction trans = con.BeginTransaction())
                        {
                            try
                            {
                                string sql = @"INSERT INTO tbnfse 
                             (num_documento, serie_documento, tipo_documento, emitido_por, emissao_documento, status_documento,situacao_documento, idviagem)
                             VALUES 
                             (@num, @serie, @tipo, @emitido_por, @emissao_documento, @status_documento,@situacao_documento, @idViagem)";

                                using (SqlCommand cmd = new SqlCommand(sql, con, trans))
                                {
                                    DateTime agora = DateTime.Now;

                                    cmd.Parameters.AddWithValue("@num", txtNFSe.Text);
                                    cmd.Parameters.AddWithValue("@serie", "1");
                                    cmd.Parameters.AddWithValue("@tipo", "NFS-e");
                                    cmd.Parameters.AddWithValue("@emitido_por", nomeUsuario);
                                    // Passando como DateTime puro (mais seguro)
                                    cmd.Parameters.AddWithValue("@status_documento", "Pendente");
                                    cmd.Parameters.AddWithValue("@situacao_documento", "Emitido");
                                    cmd.Parameters.AddWithValue("@emissao_documento", agora);
                                    cmd.Parameters.AddWithValue("@idViagem", carga);

                                    cmd.ExecuteNonQuery();
                                }
                                trans.Commit(); // Commit primeiro

                                // Agora que os dados estão no banco, fazemos o bind
                                rptColetas.DataBind();
                                MostrarMsg("Alterações salvas com sucesso!");
                            }
                            catch (Exception ex)
                            {
                                trans.Rollback();
                                throw ex;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MostrarMsg("Erro ao salvar NFS-e: " + ex.Message);
                    }
                    finally
                    {
                        if (con.State == ConnectionState.Open) con.Close();
                    }
                }

                Session["Coletas"] = null;
                CarregarColetas(novaColeta.Text);
                BuscarCteSalvos(idViagem);
                CarregarPedidos(int.Parse(carga), gv);




            }
            if (e.CommandName == "Coletas")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                string idCarga = id.ToString(); // esse valor viria da lógica do seu código
                                                //hdIdCarga.Value = idCarga;
                Session["idCarga"] = idCarga;
                //GetPedidos();
                //string url = $"OrdemColetaImpressaoIndividual.aspx?id={idCarga}";
                //string script = $"window.open('{url}', '_blank', 'toolbar=no,location=no,status=no,menubar=no,scrollbars=yes,resizable=yes,width=794,height=1123');";
                //ClientScript.RegisterStartupScript(this.GetType(), "abrirJanela", script, true);
            }
            if (e.CommandName == "AtualizarColeta")
            {
                string carga = e.CommandArgument.ToString();

                // Recuperar os controles de dentro do item                



                // Exemplo: atualizando no banco
                using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
                {
                    TextBox txtRedes = (TextBox)e.Item.FindControl("txtRedes");
                    TextBox txtCatracas = (TextBox)e.Item.FindControl("txtCatracas");
                    TextBox txtOT = (TextBox)e.Item.FindControl("txtOT");


                    string query = @"UPDATE tbcargas SET                                  
                                 ot = @ot,
                                 catraca = @catraca,
                                 rede = @rede 
                                 WHERE carga = @carga";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@carga", carga);
                    cmd.Parameters.AddWithValue("@ot", txtOT.Text);
                    cmd.Parameters.AddWithValue("@catraca", txtCatracas.Text);
                    cmd.Parameters.AddWithValue("@rede", txtRedes.Text.Trim());


                    // Chama método que verifica no banco
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }



                // Após atualizar, recarregar os dados no Repeater
                MostrarMsg("Dados Salvos!");
                Session["Coletas"] = null;
                CarregarColetas(novaColeta.Text);
            }
            if (e.CommandName == "PedagioManual")
            {
                using (SqlConnection conn = new SqlConnection(
                WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
                {
                    string query = @"UPDATE tbcarregamentos SET 
                     pedagio = @pedagio,                                 
                     pedagiofeito = @pedagiofeito 
                     WHERE num_carregamento = @num_carregamento";

                    using (SqlCommand cmdPedagio = new SqlCommand(query, conn))
                    {
                        cmdPedagio.Parameters.Add("@num_carregamento", SqlDbType.Int)
                                             .Value = Convert.ToInt32(novaColeta.Text);

                        cmdPedagio.Parameters.Add("@pedagio", SqlDbType.VarChar)
                                             .Value = "SIM";

                        cmdPedagio.Parameters.Add("@pedagiofeito", SqlDbType.VarChar)
                                             .Value = "Pendente";

                        conn.Open();
                        int linhas = cmdPedagio.ExecuteNonQuery();

                        if (linhas == 0)
                        {
                            throw new Exception("Nenhum registro encontrado para atualizar.");
                        }
                    }
                }


                // Após atualizar, recarregar os dados no Repeater
                // MostrarMsg("Pedágio enviado com sucesso!");
                //Session["Coletas"] = null;
                //CarregarColetas(novaColeta.Text);
            }
            if (e.CommandName == "EnviarSM")
            {
                // 1. Captura os dados da interface ANTES do processo assíncrono
                string idCarga = e.CommandArgument.ToString();

                DropDownList ddlPercurso = (DropDownList)e.Item.FindControl("ddlPercurso");
                DropDownList ddlRotaKrona = (DropDownList)e.Item.FindControl("ddlRotaKrona");
                string peso = ((TextBox)e.Item.FindControl("txtPeso")).Text;
                string valor = ((TextBox)e.Item.FindControl("txtValorTotal")).Text;
                string previsao_inicial = ((TextBox)e.Item.FindControl("txtPrevisaoInicio")).Text;
                string previsao_final = ((TextBox)e.Item.FindControl("txtPrevisaoTermino")).Text;
                string codmotorista = txtCodMotorista.Text;
                string percurso = ddlPercurso.SelectedItem.Text;
                string rota = ddlRotaKrona.SelectedItem.Text;
                string id_rota = ddlRotaKrona.SelectedValue;
                string placa = txtPlaca.Text;
                string codveiculo = txtCodVeiculo.Text;

                try
                {
                    // 1. Monta o objeto
                    var solicitacao = CriarObjetoSolicitacao(idCarga, placa, valor, peso, previsao_inicial, previsao_final, percurso, rota, id_rota, codmotorista, codveiculo);

                    // 2. Serializa
                    string jsonEnvio = System.Text.Json.JsonSerializer.Serialize(solicitacao, new JsonSerializerOptions
                    {
                        WriteIndented = true,
                        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                    });

                    // 3. Grava Arquivo Físico (Auditoria)
                    string caminhoFisico = Server.MapPath("~/EnviaSM/SM_Solicitacao_" + idCarga + ".json");
                    System.IO.File.WriteAllText(caminhoFisico, jsonEnvio, System.Text.Encoding.UTF8);

                    // 4. ENVIO SÍNCRONO (A página vai "carregar" enquanto espera a resposta)
                    string jsonResposta = EnviarRequisicaoKronaSincrona(jsonEnvio);

                    // 5. Salva no Banco (Corrigindo a ordem dos parâmetros para bater com a definição do método)
                    // Ordem correta baseada no seu método: (json, carga, valor, previsao_init, previsao_fim, percurso, rota, id_rota)
                    ProcessarESalvarRetorno(jsonResposta, idCarga, valor, previsao_inicial, previsao_final, percurso, rota, id_rota);

                }
                catch (Exception ex)
                {
                    MostrarMsg("Erro: " + ex.Message);
                }
            }
            if (e.CommandName == "BuscarNfe")
            {
                //string idCarga = e.CommandArgument.ToString();
                int idCarga = Convert.ToInt32(e.CommandArgument);
                string chave = ((TextBox)e.Item.FindControl("txtChaveNF")).Text;
                GridView gvNF = (GridView)e.Item.FindControl("gvNF");
                TextBox txtChaveNF = (TextBox)e.Item.FindControl("txtChaveNF");

                //string chave = "35260259104422002446550370009554061775712904";
                string apiKey = "025caf00-6477-4d97-b133-f34ad21594f3";

                // ========= 1ª CHAMADA (PUT) =========
                var request = (HttpWebRequest)WebRequest.Create(
                    "https://api.meudanfe.com.br/v2/fd/add/" + chave);

                request.Method = "PUT";
                request.Accept = "application/json";
                request.Headers.Add("Api-Key", apiKey);
                request.ContentLength = 0; // 👈 MUITO IMPORTANTE

                string jsonPut;

                using (var requestStream = request.GetRequestStream())
                {
                    // não escreve nada, só força o envio do corpo vazio
                }

                using (var response = (HttpWebResponse)request.GetResponse())
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    jsonPut = reader.ReadToEnd();
                }

                var serializer = new JavaScriptSerializer();
                RetornoPut retornoPut = serializer.Deserialize<RetornoPut>(jsonPut);

                // ========= SE STATUS FOR OK =========
                if (retornoPut.status == "OK")
                {
                    // ========= 2ª CHAMADA (GET XML) =========
                    var requestXml = (HttpWebRequest)WebRequest.Create(
                        "https://api.meudanfe.com.br/v2/fd/get/xml/" + chave);

                    requestXml.Method = "GET";
                    requestXml.Accept = "application/json";
                    requestXml.Headers.Add("Api-Key", apiKey);

                    string jsonXml;

                    using (var responseXml = (HttpWebResponse)requestXml.GetResponse())
                    using (var readerXml = new StreamReader(responseXml.GetResponseStream()))
                    {
                        jsonXml = readerXml.ReadToEnd();
                    }

                    RetornoXml retornoXml = serializer.Deserialize<RetornoXml>(jsonXml);

                    string xmlNfe = retornoXml.data;

                    // aqui você já tem o XML puro
                    // pode salvar em arquivo, banco, ou carregar em XmlDocument

                    // exemplo salvando em arquivo:
                    //File.WriteAllText(Server.MapPath("~/nfe.xml"), xmlNfe);

                    SalvarXmlNoBanco(xmlNfe, idCarga);
                    txtChaveNF.Text = string.Empty;
                    txtChaveNF.Focus();
                    CarregarNotas(idCarga, gvNF);


                }
                else
                {
                    // erro retornado pela API
                    string msgErro = retornoPut.statusMessage;

                    MostrarMsg("Erro ao executar o processo: " + msgErro);
                }

            }
            if (e.CommandName == "GeraDoc")
            {
                int idCarga = int.Parse(e.CommandArgument.ToString());

                // 1. Sua validação que você confirmou que funciona:
                if (!PossuiNotasLancadas(idCarga))
                {
                    MostrarMsg("O arquivo só pode ser gerado quando as NF-e forem lançadas.");
                    return;
                }

                // 2. Define o tipo e gera o número no banco
                bool ehServico = VerificarSeEhServico(idCarga);
                string numeroDocumento = ehServico ? GerarEObterProximoNFse() : GerarEObterProximoCte();

                if (string.IsNullOrEmpty(numeroDocumento))
                {
                    MostrarMsg("Erro ao gerar a numeração do documento.");
                    return;
                }

                // 3. Em vez de chamar o DispararDownload aqui, chamamos a página dedicada via JavaScript.
                // O window.open faz o navegador abrir o fluxo de download nativo sem sair da página atual.
                string urlDownload = $"DownloadSapiens.aspx?idCarga={idCarga}&numDoc={numeroDocumento}&serv={ehServico.ToString().ToLower()}";
                string script = $"window.open('{urlDownload}', '_blank');";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "dispararDownloadSapiens", script, true);
            }
            if (e.CommandName == "GeraXml")
            {
                try
                {
                    int idCarga = int.Parse(e.CommandArgument.ToString());

                    // 1. Validação de segurança
                    if (!PossuiNotasLancadas(idCarga))
                    {
                        MostrarMsg("O arquivo só pode ser gerado quando as NF-e forem lançadas.");
                        return;
                    }

                    // 2. Determina o tipo e busca o próximo número no banco
                    bool ehServico = VerificarSeEhServico(idCarga);
                    string numeroDoc = ehServico ? GerarEObterProximoNFse() : GerarEObterProximoCte();

                    if (string.IsNullOrEmpty(numeroDoc))
                    {
                        MostrarMsg("Erro ao gerar a numeração do documento.");
                        return;
                    }

                    // 3. Em vez de chamar o DispararDownload interno com problema de AJAX,
                    // redireciona para a página dedicada passando o parâmetro &formato=xml
                    string urlDownload = $"DownloadSapiens.aspx?idCarga={idCarga}&numDoc={numeroDoc}&serv={ehServico.ToString().ToLower()}&formato=xml";
                    string script = $"window.open('{urlDownload}', '_blank');";

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "dispararDownloadXml", script, true);
                }
                catch (Exception ex)
                {
                    MostrarMsg("Erro ao processar documento: " + ex.Message);
                }
            }
            if (e.CommandName == "CancelarCarga")
            {
                // 1. Verificação de permissão por usuário
                string usuarioLogado = Session["UsuarioLogado"]?.ToString();
                string usuarioSistema = Session["UsuarioSistema"]?.ToString();
                if (usuarioSistema != "TNG30976" && usuarioSistema != "admin")
                {
                    // Aqui você pode usar um ScriptManager para alertar o usuário no navegador
                    ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage",
                        "alert('Essa opção não está disponível para o seu usuário.');", true);
                    return;
                }

                string carga = e.CommandArgument.ToString();
                string usuarioFormatado = DateTime.Now.ToString("dd/MM/yyyy HH:mm") + " - " + usuarioLogado;

                using (SqlConnection conn = new SqlConnection(
                    WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            int idViagem;

                            // 🔹 1. Buscar idviagem da carga
                            string sqlBusca = "SELECT idviagem FROM tbcargas WHERE carga = @carga";
                            using (SqlCommand cmd = new SqlCommand(sqlBusca, conn, trans))
                            {
                                cmd.Parameters.Add("@carga", SqlDbType.Int).Value = Convert.ToInt32(carga);
                                idViagem = Convert.ToInt32(cmd.ExecuteScalar());
                            }

                            // 🔹 2. Cancelar a carga na tbcargas
                            string sqlUpdateCarga = @"UPDATE tbcargas 
                                          SET status = 'Cancelada',
                                              andamento = 'CANCELADA',
                                              atualizacao = @usuario,
                                              material = 'Vazio',
                                              fl_exclusao = 'S' 
                                          WHERE carga = @carga";

                            using (SqlCommand cmd = new SqlCommand(sqlUpdateCarga, conn, trans))
                            {
                                cmd.Parameters.Add("@carga", SqlDbType.Int).Value = Convert.ToInt32(carga);
                                cmd.Parameters.Add("@usuario", SqlDbType.VarChar).Value = usuarioFormatado;
                                cmd.ExecuteNonQuery();
                            }

                            // 🔹 NOVO: 3. Cancelar na tbpedidos usando a carga como parâmetro
                            string sqlUpdatePedidos = "UPDATE tbpedidos SET fl_exclusao = 'S' WHERE carga = @carga";
                            using (SqlCommand cmd = new SqlCommand(sqlUpdatePedidos, conn, trans))
                            {
                                cmd.Parameters.Add("@carga", SqlDbType.Int).Value = Convert.ToInt32(carga);
                                cmd.ExecuteNonQuery();
                            }

                            // 🔹 4. Verificar se ainda existem cargas ativas
                            string sqlVerifica = @"SELECT COUNT(*) 
                                        FROM tbcargas 
                                        WHERE idviagem = @idviagem 
                                        AND fl_exclusao IS NULL";

                            int totalAtivas = 0;
                            using (SqlCommand cmd = new SqlCommand(sqlVerifica, conn, trans))
                            {
                                cmd.Parameters.Add("@idviagem", SqlDbType.Int).Value = idViagem;
                                totalAtivas = Convert.ToInt32(cmd.ExecuteScalar());
                            }

                            // 🔹 5. Se não existir nenhuma ativa → cancelar OC
                            if (totalAtivas == 0)
                            {
                                string sqlOC = @"UPDATE tbcarregamentos
                                      SET status = 'Cancelada',
                                          situacao = 'O. C. CANCELADA',
                                          fl_exclusao = 'S',
                                          usualt = @usuario,
                                          dtalt = GETDATE()
                                      WHERE num_carregamento = @idviagem";

                                using (SqlCommand cmd = new SqlCommand(sqlOC, conn, trans))
                                {
                                    cmd.Parameters.Add("@idviagem", SqlDbType.Int).Value = idViagem;
                                    cmd.Parameters.Add("@usuario", SqlDbType.VarChar).Value = usuarioLogado;
                                    cmd.ExecuteNonQuery();
                                }
                            }

                            trans.Commit();

                            if (totalAtivas == 0)
                            {
                                Response.Redirect("GestaoDeEntregasMatriz.aspx");
                            }
                            else
                            {
                                Session["Coletas"] = null;
                                CarregarColetas(novaColeta.Text);
                            }
                        }
                        catch (Exception)
                        {
                            //trans.Rollback();
                            throw;
                        }
                    }
                }
            }
        }

        private void AdicionarParametrosKrona(SqlCommand cmd,
                                      ControlesCarga c)
        {
            SqlHelper.NVarChar(cmd, "@percurso", 20, c.Percurso);
            SqlHelper.Decimal(cmd, "@valor_total", c.ValorTotal);
            SqlHelper.DateTime(cmd, "@previsao_inicio_krona", c.PrevisaoInicioKrona);
            SqlHelper.DateTime(cmd, "@previsao_termino_krona", c.PrevisaoTerminoKrona);
            SqlHelper.NVarChar(cmd, "@rota_krona", 50, c.RotaKrona);
            SqlHelper.NVarChar(cmd, "@id_rota_krona", 10, c.IdRotaKrona);
            SqlHelper.Int(cmd, "@num_sm", c.SM);
            SqlHelper.NVarChar(cmd, "@usu_envio_krona", 60, c.SmEnviadaPor);

        }
        private void AdicionarParametrosGerais(SqlCommand cmd, ControlesCarga c, string andamentoCarga)
        {
            SqlHelper.NVarChar(cmd, "@cva", 20, c.CVA);
            SqlHelper.NVarChar(cmd, "@status", 50, c.Status);
            SqlHelper.NVarChar(cmd, "@andamento", 50, c.Andamento);
            SqlHelper.DateTime(cmd, "@saidaorigem", c.SaidaOrigem);
            SqlHelper.NVarChar(cmd, "@tempoagcarreg", 20, c.TempoAgCarreg);
            SqlHelper.NVarChar(cmd, "@duracao_transp", 20, c.DuracaoTransporte);
            SqlHelper.NVarChar(cmd, "@tempoagdescarreg", 20, c.TempoAgDescarga);
            SqlHelper.DateTime(cmd, "@chegadadestino", c.ChegadaDestino);
            SqlHelper.DateTime(cmd, "@disponivel_solicitacao", c.VeiculoDisponivel);
            SqlHelper.DateTime(cmd, "@prev_chegada", c.PrevisaoChegada);
            SqlHelper.DateTime(cmd, "@saidaplanta", c.SaidaPlanta);
            SqlHelper.NVarChar(cmd, "@codmot", 10, c.CodMotorista);
            SqlHelper.NVarChar(cmd, "@frota", 10, c.Frota);
            SqlHelper.DateTime(cmd, "@gate_origem", c.GateOrigem);
            SqlHelper.DateTime(cmd, "@gate_destino", c.GateDestino);
        }
        private void AdicionarParametrosPernoite(SqlCommand cmd, ControlesCarga c)
        {
            SqlHelper.NVarChar(cmd, "@local_pernoite", 50, c.LocalPernoite.ToUpper());
            SqlHelper.DateTime(cmd, "@inicio_pernoite", c.InicioPernoite);
            SqlHelper.DateTime(cmd, "@fim_pernoite", c.FimPernoite);
            SqlHelper.NVarChar(cmd, "@duracao_pernoite", 5, c.DuracaoPernoite);
            SqlHelper.NVarChar(cmd, "@status_pernoite", 50, c.statusPernoite);
        }
        protected void gvPedidos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;

            // Select2 Motoristas
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddl = (DropDownList)e.Row.FindControl("ddlMotCar");
                if (ddl != null)
                {
                    GetMotoristas(ddl);
                }
            }

            //// Calcular tempo
            //DateTime? inicio = DataBinder.Eval(e.Row.DataItem, "iniciocar") as DateTime?;
            //DateTime? fim = DataBinder.Eval(e.Row.DataItem, "termcar") as DateTime?;

            //Label lblTempo = (Label)e.Row.FindControl("lblTempo");

            //if (inicio.HasValue && fim.HasValue)
            //{
            //    TimeSpan t = fim.Value - inicio.Value;
            //    lblTempo.Text = $"{t.Hours:D2}:{t.Minutes:D2}";
            //}

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddl = (DropDownList)e.Row.FindControl("ddlMotCar");

                if (ddl != null)
                {
                    // 1. Buscamos os motoristas (Ideal buscar uma vez só fora do loop, mas vamos focar na correção agora)
                    DataTable dtMot = BuscarTodosMotoristas();

                    ddl.DataSource = dtMot;
                    ddl.DataTextField = "nommot"; // Verifique se o nome da coluna no seu SQL é 'nome'
                    ddl.DataValueField = "id";   // Verifique se o nome da coluna no seu SQL é 'id'
                    ddl.DataBind();



                    // 2. Pegar o valor que veio do banco para esta linha
                    // Importante: motcar deve ser o ID do motorista
                    string motoristaSalvo = DataBinder.Eval(e.Row.DataItem, "motcar").ToString();


                    ddl.Items.Insert(0, new System.Web.UI.WebControls.ListItem(motoristaSalvo, "0"));
                    //ddl.SelectedItem.Text = motoristaSalvo;

                }
            }
        }


        //protected void gvPedidos_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    //if (e.Row.RowType == DataControlRowType.DataRow)
        //    //{
        //    //    // 1. Localiza o DropDownList da linha
        //    //    DropDownList ddlMotCar = (DropDownList)e.Row.FindControl("ddlMotCar");

        //    //    if (ddlMotCar != null)
        //    //    {
        //    //        // 2. Busca os motoristas do banco de dados (Exemplo de método)
        //    //        DataTable dtMotoristas = ObterListaMotoristas();

        //    //        ddlMotCar.DataSource = dtMotoristas;
        //    //        ddlMotCar.DataTextField = "nommot";
        //    //        ddlMotCar.DataValueField = "codmot";
        //    //        ddlMotCar.DataBind();

        //    //        // 3. Adiciona uma opção em branco padrão
        //    //        ddlMotCar.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Motorista que irá carregar o veículo.", ""));

        //    //        // 4. Seleciona o motorista atual do banco de dados para este pedido
        //    //        string motcarAtual = DataBinder.Eval(e.Row.DataItem, "motcar").ToString();
        //    //        if (!string.IsNullOrEmpty(motcarAtual))
        //    //        {
        //    //            ddlMotCar.SelectedItem.Text = motcarAtual;
        //    //        }
        //    //    }
        //    //}

        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        // Localiza o DropDownList da linha
        //        DropDownList ddlMotCar = (DropDownList)e.Row.FindControl("ddlMotCar");

        //        if (ddlMotCar != null)
        //        {
        //            // Carrega a lista de motoristas
        //            DataTable dtMotoristas = ObterListaMotoristas();

        //            ddlMotCar.DataSource = dtMotoristas;
        //            ddlMotCar.DataTextField = "nommot";
        //            ddlMotCar.DataValueField = "codmot";
        //            ddlMotCar.DataBind();

        //            // Adiciona a opção padrão
        //            ddlMotCar.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Motorista que irá carregar o veículo.", ""));

        //            // Recupera o motorista gravado na linha
        //            string motcarAtual = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "motcar"));

        //            if (!string.IsNullOrWhiteSpace(motcarAtual))
        //            {
        //                // Primeiro tenta localizar pelo código
        //                System.Web.UI.WebControls.ListItem item = ddlMotCar.Items.FindByValue(motcarAtual);

        //                // Se não encontrar, tenta localizar pelo nome
        //                if (item == null)
        //                    item = ddlMotCar.Items.FindByText(motcarAtual);

        //                if (item != null)
        //                {
        //                    ddlMotCar.ClearSelection();
        //                    item.Selected = true;
        //                }
        //            }
        //        }
        //    }


        //    //if (e.Row.RowType != DataControlRowType.DataRow) return;

        //    //// Select2 Motoristas
        //    //if (e.Row.RowType == DataControlRowType.DataRow)
        //    //{
        //    //    DropDownList ddl = (DropDownList)e.Row.FindControl("ddlMotCar");
        //    //    if (ddl != null)
        //    //    {
        //    //        GetMotoristas(ddl);
        //    //    }
        //    //}

        //    // Calcular tempo
        //    //DateTime? inicio = DataBinder.Eval(e.Row.DataItem, "iniciocar") as DateTime?;
        //    //DateTime? fim = DataBinder.Eval(e.Row.DataItem, "termcar") as DateTime?;

        //    //Label lblTempo = (Label)e.Row.FindControl("lblTempo");

        //    //if (inicio.HasValue && fim.HasValue)
        //    //{
        //    //    TimeSpan t = fim.Value - inicio.Value;
        //    //    lblTempo.Text = $"{t.Hours:D2}:{t.Minutes:D2}";
        //    //}

        //    //if (e.Row.RowType == DataControlRowType.DataRow)
        //    //{
        //    //    DropDownList ddl = (DropDownList)e.Row.FindControl("ddlMotCar");

        //    //    if (ddl != null)
        //    //    {
        //    //        // 1. Buscamos os motoristas (Ideal buscar uma vez só fora do loop, mas vamos focar na correção agora)
        //    //        DataTable dtMot = BuscarTodosMotoristas();

        //    //        ddl.DataSource = dtMot;
        //    //        ddl.DataTextField = "nommot"; // Verifique se o nome da coluna no seu SQL é 'nome'
        //    //        ddl.DataValueField = "id";   // Verifique se o nome da coluna no seu SQL é 'id'
        //    //        ddl.DataBind();



        //    //        // 2. Pegar o valor que veio do banco para esta linha
        //    //        // Importante: motcar deve ser o ID do motorista
        //    //        string motoristaSalvo = DataBinder.Eval(e.Row.DataItem, "motcar").ToString();


        //    //        ddl.Items.Insert(0, new System.Web.UI.WebControls.ListItem(motoristaSalvo, "0"));
        //    //        //ddl.SelectedItem.Text = motoristaSalvo;

        //    //    }
        //    //}


        //}
        private DataTable ObterListaMotoristas()
        {
            string stringConexao = System.Configuration.ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(stringConexao))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT codmot, nommot FROM tbmotoristas where fl_exclusao IS NULL and status = 'ATIVO' ORDER BY Nommot", conn))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        return dt;
                    }
                }
            }
        }
        protected void btnSalvarCarregamento_Click(object sender, EventArgs e)
        {
            // 1. Identifica o botão que disparou o clique            
            LinkButton btn = (LinkButton)sender;

            // 2. Sobe na árvore de componentes para achar a linha da GridView (GridViewRow)
            GridViewRow linha = (GridViewRow)btn.NamingContainer;

            // 3. Sobe mais um nível para achar a GridView correspondente
            GridView gvPedidos = (GridView)linha.NamingContainer;

            // 4. Pega o índice da linha da GridView
            int index = linha.RowIndex;

            // 5. Recupera as chaves "pedido" e "carga" daquela linha específica
            string numPedido = gvPedidos.DataKeys[index]["pedido"].ToString();

            // 6. Localiza os controles na linha usando FindControl
            TextBox txtIni = (TextBox)linha.FindControl("txtInicioCar");
            TextBox txtFim = (TextBox)linha.FindControl("txtTermCar");
            TextBox txtDur = (TextBox)linha.FindControl("txtTempoTotal");
            DropDownList ddlMot = (DropDownList)linha.FindControl("ddlMotCar");

            // Calcular tempo
            DateTime? inicio = DataBinder.Eval(linha.DataItem, "iniciocar") as DateTime?;
            DateTime? fim = DataBinder.Eval(linha.DataItem, "termcar") as DateTime?;

            Label lblTempo = (Label)linha.FindControl("lblTempo");

            if (inicio.HasValue && fim.HasValue)
            {
                TimeSpan t = fim.Value - inicio.Value;
                lblTempo.Text = $"{t.Hours:D2}:{t.Minutes:D2}";
            }



            // 8. Executa a atualização no Banco de Dados
            string query = @"UPDATE tbpedidos 
                     SET motcar = @motcar, 
                         iniciocar = @ini, 
                         termcar = @fim, 
                         duracao = @dur 
                     WHERE pedido = @pedido";

            string stringConexao = System.Configuration.ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(stringConexao))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                DateTime dtIni, dtFim;

                if (DateTime.TryParse(txtIni.Text, out dtIni))
                    cmd.Parameters.AddWithValue("@ini", dtIni);
                else
                    cmd.Parameters.AddWithValue("@ini", DBNull.Value);

                if (DateTime.TryParse(txtFim.Text, out dtFim))
                    cmd.Parameters.AddWithValue("@fim", dtFim);
                else
                    cmd.Parameters.AddWithValue("@fim", DBNull.Value);

                cmd.Parameters.AddWithValue("@dur", txtDur.Text);
                cmd.Parameters.AddWithValue("@motcar", ddlMot.SelectedItem.Text);
                cmd.Parameters.AddWithValue("@pedido", numPedido);                
                cmd.ExecuteNonQuery();
            }

            // Opcional: Recarregar apenas os dados deste Repeater se necessário
        }
        private DateTime? SafeDateValue(RepeaterItem item, string controlId)
        {
            TextBox txt = item.FindControl(controlId) as TextBox;

            if (txt == null || string.IsNullOrWhiteSpace(txt.Text))
                return null;

            DateTime data;

            string[] formatos =
            {
        "dd/MM/yyyy",
        "yyyy-MM-dd",
        "dd/MM/yyyy HH:mm",
        "yyyy-MM-dd HH:mm"
    };

            if (DateTime.TryParseExact(
                    txt.Text,
                    formatos,
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None,
                    out data))
            {
                return data;
            }

            return null;
        }





        private string GerarEObterProximoCte()
        {
            string numdocumento = "";
            string query = "SELECT (cte + incremento) as ProximCte FROM tbcontadores WHERE id = 1";
            string updateSql = "UPDATE tbcontadores SET cte = @cte WHERE id = 1";
            string connString = WebConfigurationManager.ConnectionStrings["conexao"].ToString();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    // 1. Busca o próximo número
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            numdocumento = result.ToString();
                        }
                    }

                    // 2. Atualiza o contador no banco para o próximo uso
                    if (!string.IsNullOrEmpty(numdocumento))
                    {
                        using (SqlCommand cmdUp = new SqlCommand(updateSql, conn))
                        {
                            cmdUp.Parameters.AddWithValue("@cte", numdocumento);
                            cmdUp.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MostrarMsg("Erro no contador CTE: " + ex.Message);
                }
            }
            return numdocumento;
        }
        private string GerarEObterProximoNFse()
        {
            string numdocumento = "";
            string query = "SELECT (nfse + incremento) as ProximCte FROM tbcontadores WHERE id = 1";
            string updateSql = "UPDATE tbcontadores SET nfse = @nfse WHERE id = 1";
            string connString = WebConfigurationManager.ConnectionStrings["conexao"].ToString();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            numdocumento = result.ToString();
                        }
                    }

                    if (!string.IsNullOrEmpty(numdocumento))
                    {
                        using (SqlCommand cmdUp = new SqlCommand(updateSql, conn))
                        {
                            cmdUp.Parameters.AddWithValue("@nfse", numdocumento);
                            cmdUp.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MostrarMsg("Erro no contador NFSe: " + ex.Message);
                }
            }
            return numdocumento;
        }
        private bool PossuiNotasLancadas(int idCarga)
        {
            bool temNota = false;
            string connString = WebConfigurationManager.ConnectionStrings["conexao"].ToString();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                // Usamos IF EXISTS para ser mais rápido, pois só precisamos saber se há pelo menos uma
                string sql = "SELECT CASE WHEN EXISTS (SELECT 1 FROM tbnfe WHERE carga = @carga) THEN 1 ELSE 0 END";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@carga", idCarga);

                conn.Open();
                temNota = (int)cmd.ExecuteScalar() == 1;
            }

            return temNota;
        }
        private bool VerificarSeEhServico(int idCarga)
        {
            bool resultado = false;
            string connString = WebConfigurationManager.ConnectionStrings["conexao"].ToString();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                // Sua query: seleciona origem e destino da carga
                string sql = "SELECT cidorigem, ciddestino FROM tbcargas WHERE carga = @carga";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@carga", idCarga);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string origem = reader["cidorigem"].ToString();
                        string destino = reader["ciddestino"].ToString();

                        // Regra: Cidades iguais = Serviço (NFSe)
                        if (origem == destino)
                        {
                            resultado = true;
                        }
                    }
                }
            }
            return resultado;
        }
        // Adicionamos o '= "application/octet-stream"' no final. 
        // Isso torna o terceiro parâmetro opcional e não quebra as chamadas antigas.
        private void DispararDownload(string texto, string nomeArquivo, string contentType = "application/octet-stream")
        {
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();

            // Agora ele usa o contentType que você passar (ou o padrão se não passar nada)
            Response.ContentType = contentType;
            Response.AddHeader("Content-Disposition", "attachment; filename=" + nomeArquivo);

            // IMPORTANTE: Para XML o ideal é UTF-8, para o TXT do Sapiens é ISO.
            // Vamos ajustar para respeitar o formato:
            byte[] dados;
            if (contentType.Contains("xml"))
            {
                dados = Encoding.UTF8.GetBytes(texto);
            }
            else
            {
                dados = Encoding.GetEncoding("ISO-8859-1").GetBytes(texto);
            }

            Response.AddHeader("Content-Length", dados.Length.ToString());
            Response.BinaryWrite(dados);
            Response.Flush();

            try { Response.End(); } catch { }
        }
        public class RetornoPut
        {
            public string value { get; set; }
            public string type { get; set; }
            public string status { get; set; }
            public string statusMessage { get; set; }
        }
        public class RetornoXml
        {
            public string name { get; set; }
            public string type { get; set; }
            public string format { get; set; }
            public string data { get; set; } // aqui vem o XML
        }
        public void SalvarXmlNoBanco(string xml, int carga)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            XmlNamespaceManager ns = new XmlNamespaceManager(doc.NameTable);
            ns.AddNamespace("nfe", "http://www.portalfiscal.inf.br/nfe");

            // ===== DADOS PRINCIPAIS =====
            string chave = doc.SelectSingleNode("//nfe:infNFe", ns).Attributes["Id"].Value.Replace("NFe", "");
            string numero = doc.SelectSingleNode("//nfe:ide/nfe:nNF", ns)?.InnerText;
            string serie = doc.SelectSingleNode("//nfe:ide/nfe:serie", ns)?.InnerText;
            string dhEmi = doc.SelectSingleNode("//nfe:ide/nfe:dhEmi", ns)?.InnerText;

            string emitCnpj = doc.SelectSingleNode("//nfe:emit/nfe:CNPJ", ns)?.InnerText;
            string emitNome = doc.SelectSingleNode("//nfe:emit/nfe:xNome", ns)?.InnerText;
            string emitFant = doc.SelectSingleNode("//nfe:emit/nfe:xFant", ns)?.InnerText;
            string emitIE = doc.SelectSingleNode("//nfe:emit/nfe:IE", ns)?.InnerText;
            string emitEnd = doc.SelectSingleNode("//nfe:emit/nfe:enderEmit/nfe:xLgr", ns)?.InnerText;
            string emitNum = doc.SelectSingleNode("//nfe:emit/nfe:enderEmit/nfe:nro", ns)?.InnerText;
            string emitBairro = doc.SelectSingleNode("//nfe:emit/nfe:enderEmit/nfe:xBairro", ns)?.InnerText;
            string emitMun = doc.SelectSingleNode("//nfe:emit/nfe:enderEmit/nfe:xMun", ns)?.InnerText;
            string emitUF = doc.SelectSingleNode("//nfe:emit/nfe:enderEmit/nfe:UF", ns)?.InnerText;
            string emitCEP = doc.SelectSingleNode("//nfe:emit/nfe:enderEmit/nfe:CEP", ns)?.InnerText;

            string destCnpj = doc.SelectSingleNode("//nfe:dest/nfe:CNPJ", ns)?.InnerText;
            string destNome = doc.SelectSingleNode("//nfe:dest/nfe:xNome", ns)?.InnerText;
            string destIE = doc.SelectSingleNode("//nfe:dest/nfe:IE", ns)?.InnerText;
            string destEnd = doc.SelectSingleNode("//nfe:dest/nfe:enderDest/nfe:xLgr", ns)?.InnerText;
            string destNum = doc.SelectSingleNode("//nfe:dest/nfe:enderDest/nfe:nro", ns)?.InnerText;
            string destBairro = doc.SelectSingleNode("//nfe:dest/nfe:enderDest/nfe:xBairro", ns)?.InnerText;
            string destMun = doc.SelectSingleNode("//nfe:dest/nfe:enderDest/nfe:xMun", ns)?.InnerText;
            string destUF = doc.SelectSingleNode("//nfe:dest/nfe:enderDest/nfe:UF", ns)?.InnerText;
            string destCEP = doc.SelectSingleNode("//nfe:dest/nfe:enderDest/nfe:CEP", ns)?.InnerText;

            string vbc = doc.SelectSingleNode("//nfe:ICMSTot/nfe:vBC", ns)?.InnerText;
            string vicms = doc.SelectSingleNode("//nfe:ICMSTot/nfe:vICMS", ns)?.InnerText;
            string vprod = doc.SelectSingleNode("//nfe:ICMSTot/nfe:vProd", ns)?.InnerText;
            string vfrete = doc.SelectSingleNode("//nfe:ICMSTot/nfe:vFrete", ns)?.InnerText;
            string vseg = doc.SelectSingleNode("//nfe:ICMSTot/nfe:vSeg", ns)?.InnerText;
            string vdesc = doc.SelectSingleNode("//nfe:ICMSTot/nfe:vDesc", ns)?.InnerText;
            string vipi = doc.SelectSingleNode("//nfe:ICMSTot/nfe:vIPI", ns)?.InnerText;
            string vpis = doc.SelectSingleNode("//nfe:ICMSTot/nfe:vPIS", ns)?.InnerText;
            string vcofins = doc.SelectSingleNode("//nfe:ICMSTot/nfe:vCOFINS", ns)?.InnerText;
            string vnf = doc.SelectSingleNode("//nfe:ICMSTot/nfe:vNF", ns)?.InnerText;

            string protocolo = doc.SelectSingleNode("//nfe:protNFe/nfe:infProt/nfe:nProt", ns)?.InnerText;
            string dataAut = doc.SelectSingleNode("//nfe:protNFe/nfe:infProt/nfe:dhRecbto", ns)?.InnerText;

            int idNfe;
            if (con.State == ConnectionState.Closed) con.Open();
            string sqlExiste = "SELECT COUNT(1) FROM tbnfe WHERE chavenfe = @chave";

            using (SqlCommand cmdExiste = new SqlCommand(sqlExiste, con))
            {

                cmdExiste.Parameters.AddWithValue("@chave", chave);

                int existe = Convert.ToInt32(cmdExiste.ExecuteScalar());

                if (existe > 0)
                {
                    MostrarMsg("Já existe uma NF-e cadastrada com esta chave:\n" + chave);
                    return; // ⛔ não continua o método
                }
                con.Close();
            }

            string emitCnpjLimpo = LimpaCnpj(emitCnpj);
            string destCnpjLimpo = LimpaCnpj(destCnpj);

            string cnpjRemCarga = "";
            string cnpjDestCarga = "";
            if (con.State == ConnectionState.Closed) con.Open();
            string sqlCarga = @"
                                select 
                                 REPLACE(REPLACE(REPLACE(cnpj_remetente, '.', ''), '/', ''), '-', '') AS cnpj_remetente,
                                 REPLACE(REPLACE(REPLACE(cnpj_destinatario, '.', ''), '/', ''), '-', '') AS cnpj_destinatario
                                from tbcargas
                                where carga = @carga";

            using (SqlCommand cmdCarga = new SqlCommand(sqlCarga, con))
            {

                cmdCarga.Parameters.AddWithValue("@carga", carga);

                using (SqlDataReader dr = cmdCarga.ExecuteReader())
                {
                    if (!dr.Read())
                    {
                        MostrarMsg("Carga não encontrada na tbcargas.");
                        return;
                    }
                    cnpjRemCarga = dr["cnpj_remetente"].ToString().Trim();
                    cnpjDestCarga = dr["cnpj_destinatario"].ToString().Trim();
                }
                con.Close();
            }

            // 🔍 VALIDAÇÃO DE CAMPOS EM BRANCO
            if (string.IsNullOrWhiteSpace(cnpjRemCarga) && string.IsNullOrWhiteSpace(cnpjDestCarga))
            {
                MostrarMsg("CNPJ do REMETENTE e do DESTINATÁRIO não estão cadastrados na carga.");
                return;
            }

            if (string.IsNullOrWhiteSpace(cnpjRemCarga))
            {
                MostrarMsg("CNPJ do REMETENTE não está cadastrado na carga.");
                return;
            }

            if (string.IsNullOrWhiteSpace(cnpjDestCarga))
            {
                MostrarMsg("CNPJ do DESTINATÁRIO não está cadastrado na carga.");
                return;
            }

            // 🔐 VALIDAÇÃO DE CONFERÊNCIA COM O XML
            if (emitCnpjLimpo != cnpjRemCarga)
            {
                MostrarMsg("CNPJ do EMITENTE da NF não confere com o REMETENTE da carga.");
                return;
            }

            if (destCnpjLimpo != cnpjDestCarga)
            {
                MostrarMsg("CNPJ do DESTINATÁRIO da NF não confere com o DESTINATÁRIO da carga.");
                return;
            }



            using (SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                con.Open();

                string sqlNfe = @"
                        INSERT INTO tbnfe
                        (chavenfe,numeronfe,serienfe,datemissao,
                        emit_cnpj,emit_nome,emit_fantasia,emit_ie,emit_endereco,emit_numero,emit_bairro,emit_municipio,emit_uf,emit_cep,
                        dest_cnpj,dest_nome,dest_ie,dest_endereco,dest_numero,dest_bairro,dest_municipio,dest_uf,dest_cep,
                        vbc,vicms,vprod,vfrete,vseg,vdesc,vipi,vpis,vcofins,vnf,
                        protocolo,dataautorizacao,carga)
                        VALUES
                        (@chave,@numero,@serie,@dataemi,
                        @emitcnpj,@emitnome,@emitfant,@emitie,@emitend,@emitnum,@emitbairro,@emitmun,@emituf,@emitcep,
                        @destcnpj,@destnome,@destie,@destend,@destnum,@destbairro,@destmun,@destuf,@destcep,
                        @vbc,@vicms,@vprod,@vfrete,@vseg,@vdesc,@vipi,@vpis,@vcofins,@vnf,
                        @prot,@dataaut,@carga);
                        SELECT SCOPE_IDENTITY();";

                SqlCommand cmd = new SqlCommand(sqlNfe, con);

                cmd.Parameters.AddWithValue("@chave", chave);
                cmd.Parameters.AddWithValue("@numero", numero);
                cmd.Parameters.AddWithValue("@serie", serie);
                cmd.Parameters.AddWithValue("@dataemi", DateTime.Parse(dhEmi));
                cmd.Parameters.AddWithValue("@emitcnpj", emitCnpj);
                cmd.Parameters.AddWithValue("@emitnome", emitNome);
                AddParam(cmd, "@emitfant", emitFant);
                AddParam(cmd, "@emitie", emitIE);
                cmd.Parameters.AddWithValue("@emitend", emitEnd);
                cmd.Parameters.AddWithValue("@emitnum", emitNum);
                cmd.Parameters.AddWithValue("@emitbairro", emitBairro);
                cmd.Parameters.AddWithValue("@emitmun", emitMun);
                cmd.Parameters.AddWithValue("@emituf", emitUF);
                cmd.Parameters.AddWithValue("@emitcep", emitCEP);
                cmd.Parameters.AddWithValue("@destcnpj", destCnpj);
                cmd.Parameters.AddWithValue("@destnome", destNome);
                AddParam(cmd, "@destie", destIE);
                cmd.Parameters.AddWithValue("@destend", destEnd);
                cmd.Parameters.AddWithValue("@destnum", destNum);
                cmd.Parameters.AddWithValue("@destbairro", destBairro);
                cmd.Parameters.AddWithValue("@destmun", destMun);
                cmd.Parameters.AddWithValue("@destuf", destUF);
                cmd.Parameters.AddWithValue("@destcep", destCEP);
                cmd.Parameters.AddWithValue("@vbc", vbc);
                cmd.Parameters.AddWithValue("@vicms", vicms);
                cmd.Parameters.AddWithValue("@vprod", vprod);
                cmd.Parameters.AddWithValue("@vfrete", vfrete);
                AddParam(cmd, "@vseg", vseg);
                AddParam(cmd, "@vdesc", vdesc);
                cmd.Parameters.AddWithValue("@vipi", vipi);
                cmd.Parameters.AddWithValue("@vpis", vpis);
                cmd.Parameters.AddWithValue("@vcofins", vcofins);
                cmd.Parameters.AddWithValue("@vnf", vnf);
                cmd.Parameters.AddWithValue("@prot", protocolo);
                cmd.Parameters.AddWithValue("@dataaut", DateTime.Parse(dataAut));
                cmd.Parameters.AddWithValue("@carga", carga);

                idNfe = Convert.ToInt32(cmd.ExecuteScalar());

                // ===== ITENS =====
                var itens = doc.SelectNodes("//nfe:det", ns);

                foreach (XmlNode item in itens)
                {
                    string sqlItem = @"
                            INSERT INTO tbnfeitem
                            (idnfe,numerolinha,codproduto,descricao,ncm,cfop,unidade,quantidade,valorunitario,valortotal,
                            icms_origem,icms_cst,ipi_cst,pis_cst,cofins_cst)
                            VALUES
                            (@id,@linha,@cod,@desc,@ncm,@cfop,@und,@qtd,@vun,@vtot,
                            @orig,@cst,@ipi,@pis,@cof)";

                    SqlCommand cmdItem = new SqlCommand(sqlItem, con);

                    cmdItem.Parameters.AddWithValue("@id", idNfe);
                    cmdItem.Parameters.AddWithValue("@linha", item["nItem"].InnerText);
                    cmdItem.Parameters.AddWithValue("@cod", item["prod"]["cProd"].InnerText);
                    cmdItem.Parameters.AddWithValue("@desc", item["prod"]["xProd"].InnerText);
                    cmdItem.Parameters.AddWithValue("@ncm", item["prod"]["NCM"].InnerText);
                    cmdItem.Parameters.AddWithValue("@cfop", item["prod"]["CFOP"].InnerText);
                    cmdItem.Parameters.AddWithValue("@und", item["prod"]["uCom"].InnerText);
                    cmdItem.Parameters.AddWithValue("@qtd", item["prod"]["qCom"].InnerText);
                    cmdItem.Parameters.AddWithValue("@vun", item["prod"]["vUnCom"].InnerText);
                    cmdItem.Parameters.AddWithValue("@vtot", item["prod"]["vProd"].InnerText);
                    cmdItem.Parameters.AddWithValue("@orig", XmlValue(item, "imposto/ICMS/*/orig"));
                    cmdItem.Parameters.AddWithValue("@cst", XmlValue(item, "imposto/ICMS/*/CST"));
                    cmdItem.Parameters.AddWithValue("@ipi", XmlValue(item, "imposto/IPI/*/CST"));
                    cmdItem.Parameters.AddWithValue("@pis", XmlValue(item, "imposto/PIS/*/CST"));
                    cmdItem.Parameters.AddWithValue("@cof", XmlValue(item, "imposto/COFINS/*/CST"));


                    cmdItem.ExecuteNonQuery();
                }
            }
        }
        object XmlValue(XmlNode node, string xpath)
        {
            var n = node.SelectSingleNode(xpath);
            return n == null ? DBNull.Value : (object)n.InnerText;
        }
        void AddParam(SqlCommand cmd, string nome, object valor)
        {
            cmd.Parameters.AddWithValue(nome,
                valor == null || string.IsNullOrWhiteSpace(valor.ToString())
                ? (object)DBNull.Value
                : valor);
        }
        private string LimpaCnpj(string cnpj)
        {
            return cnpj.Replace(".", "").Replace("/", "").Replace("-", "").Trim();
        }

        private SolicitacaoViagemRequest CriarObjetoSolicitacao(string idCarga, string placa, string valor, string peso, string previsao_inicial, string previsao_final, string percurso, string rota, string id_rota, string codmotorista, string codveiculo)
        {
            // --- 1. MOTORISTA ---
            string mot = "select nommot,cpf,numrg,orgaorg,dtnasc,nomemae,estcivil,numregcnh,catcnh,venccnh, endmot,complemento,baimot,cidmot,ufmot,cepmot,fone3,fone2,tipomot,codtra from tbmotoristas where codmot='" + codmotorista + "'";
            SqlDataAdapter adptm = new SqlDataAdapter(mot, con);
            System.Data.DataTable dm = new System.Data.DataTable();
            adptm.Fill(dm);

            // Proteção: Se não achar motorista, cria linha vazia para não quebrar o dm.Rows[0]
            if (dm.Rows.Count == 0)
            {
                dm.Columns.Add("codtra"); // Garante que a coluna existe para a próxima query
                var row = dm.NewRow();
                row["codtra"] = "0";
                dm.Rows.Add(row);
            }

            // --- 2. TRANSPORTADORA (MOTORISTA) ---
            string codTraMot = dm.Rows[0][19].ToString();
            DataTable dt = new DataTable();
            if (codTraMot != "0" && !string.IsNullOrEmpty(codTraMot))
            {
                string trans = "select cnpj, nomtra,fantra, baitra, endtra,numero,complemento,baitra,cidtra,uftra,ceptra,fone1 from tbtransportadoras where codtra='1111'";
                new SqlDataAdapter(trans, con).Fill(dt);
            }
            if (dt.Rows.Count == 0) { dt.Columns.Add("dummy"); dt.Rows.Add(dt.NewRow()); }

            // --- 3. VEÍCULO PRINCIPAL ---
            string veic = "select plavei,renavan,marca,modelo,cor,ano,tipoveiculo,cap,v.antt,t.nomtra,t.cnpj,endtra,numero,complemento,baitra,cidtra,uftra,ceptra,rastreador,terminal,comunicacao,reboque1,reboque2 from tbveiculos as v inner join tbtransportadoras as t on v.codtra=t.codtra where codvei='" + codveiculo + "'";
            DataTable dv = new DataTable();
            new SqlDataAdapter(veic, con).Fill(dv);

            if (dv.Rows.Count == 0)
            {
                // Se o veículo não existe, criamos uma linha fake para as próximas queries (reboques) não darem erro
                for (int i = 0; i < 23; i++) dv.Columns.Add();
                var r = dv.NewRow();
                r[21] = "0"; r[22] = "0"; // IDs de reboque fake
                dv.Rows.Add(r);
            }

            // --- 4. REBOQUE 1 (Tratamento especial) ---
            string idReb1 = dv.Rows[0][21].ToString();
            DataTable db1 = new DataTable();
            if (!string.IsNullOrEmpty(idReb1) && idReb1 != "0")
            {
                string reb1 = "select STUFF(placacarreta, 4, 0, '-') AS placacarreta,renavan,marca,modelo,cor,anocarreta,tipocarreta,tara,v.antt,t.nomtra,t.cnpj,endtra,numero,complemento,baitra,cidtra,uftra,ceptra,tecnologia,idrastreador,comunicacao from tbcarretas as v inner join tbtransportadoras as t on v.codprop=t.codtra where placacarreta='" + idReb1 + "'";
                new SqlDataAdapter(reb1, con).Fill(db1);
            }
            if (db1.Rows.Count == 0) { db1 = dv.Clone(); db1.Rows.Add(db1.NewRow()); }

            // --- 5. REBOQUE 2 (Tratamento especial) ---
            //string idReb2 = dv.Rows[0][22].ToString();
            //DataTable db2 = new DataTable();
            //if (!string.IsNullOrEmpty(idReb2) && idReb2 != "0")
            //{
            //    string reb2 = "select STUFF(placacarreta, 4, 0, '-') AS placacarreta,renavan,marca,modelo,cor,anocarreta,tipocarreta,tara,v.antt,t.nomtra,t.cnpj,endtra,numero,complemento,baitra,cidtra,uftra,ceptra,tecnologia,idrastreador,comunicacao from tbcarretas as v inner join tbtransportadoras as t on v.codprop=t.codtra where placacarreta='" + idReb2 + "'";
            //    new SqlDataAdapter(reb2, con).Fill(db2);
            //}
            //if (db2.Rows.Count == 0) { db2 = dv.Clone(); db2.Rows.Add(db2.NewRow()); }

            // --- 6. CARGA ---
            DataTable dg = new DataTable();
            if (!string.IsNullOrEmpty(idCarga) && idCarga != "0")
            {
                string carga = "select codorigem, coddestino, cod_expedidor, RTRIM(cva) AS cva from tbcargas where carga=" + idCarga;
                new SqlDataAdapter(carga, con).Fill(dg);
            }

            // Se a carga falhar ou não existir, criamos a estrutura necessária
            if (dg.Rows.Count == 0)
            {
                // É importante dar NOME às colunas para evitar confusão no Rows[0][index]
                if (dg.Columns.Count == 0)
                {
                    dg.Columns.Add("codorigem");
                    dg.Columns.Add("coddestino");
                    dg.Columns.Add("cod_expedidor");
                }
                var r = dg.NewRow();
                r["codorigem"] = "0";
                r["coddestino"] = "0";
                r["cod_expedidor"] = "0";
                dg.Rows.Add(r);
            }

            // --- 7. ORIGEM (dto), DESTINO (dtd) E EXPEDIDOR (dte) ---

            // Função auxiliar interna para evitar repetição de código e erro de colunas faltando
            Action<DataTable> AdicionarColunasVazias = (tabela) =>
            {
                if (tabela.Rows.Count == 0)
                {
                    // Garante que a tabela tenha 12 colunas para não dar erro de índice no JSON
                    for (int i = tabela.Columns.Count; i < 12; i++)
                    {
                        tabela.Columns.Add();
                    }
                    tabela.Rows.Add(tabela.NewRow());
                }
            };

            // Agora você chama passando as suas tabelas específicas:
            DataTable dto = new DataTable();
            new SqlDataAdapter("select cnpj, razcli, nomcli, baicli, endcli, numero, complemento, baicli, cidcli, estcli, cepcli, tc1cli from tbclientes where codcli='" + dg.Rows[0][0].ToString() + "'", con).Fill(dto);
            AdicionarColunasVazias(dto);

            DataTable dtd = new DataTable();
            new SqlDataAdapter("select cnpj, razcli, nomcli, baicli, endcli, numero, complemento, baicli, cidcli, estcli, cepcli, tc1cli from tbclientes where codcli='" + dg.Rows[0][1].ToString() + "'", con).Fill(dtd);
            AdicionarColunasVazias(dtd);

            DataTable dte = new DataTable();
            new SqlDataAdapter("select cnpj, razcli, nomcli, baicli, endcli, numero, complemento, baicli, cidcli, estcli, cepcli, tc1cli from tbclientes where codcli='" + dg.Rows[0][2].ToString() + "'", con).Fill(dte);
            AdicionarColunasVazias(dte);
            return new SolicitacaoViagemRequest
            {
                kronaService = new KronaService
                {
                    usuario_login = new UsuarioLogin
                    {
                        login = "VOLKSWAGEN.TRANSNOVAG",
                        senha = "WSTRANSNOVAG2024"
                    },

                    transportador = new Transportador
                    {
                        tipo = "TRANSPORTADOR",
                        cnpj = dt.Rows[0][0].ToString(),
                        razao_social = dt.Rows[0][1].ToString(),
                        nome_fantasia = dt.Rows[0][2].ToString(),
                        unidade = dt.Rows[0][3].ToString(),
                        codigo = "",
                        end_rua = dt.Rows[0][4].ToString(),
                        end_numero = dt.Rows[0][5].ToString(),
                        end_complemento = dt.Rows[0][6].ToString(),
                        end_bairro = dt.Rows[0][7].ToString(),
                        end_cidade = dt.Rows[0][8].ToString(),
                        end_uf = dt.Rows[0][9].ToString(),
                        end_cep = dt.Rows[0][10].ToString(),
                        latitude = "",
                        longitude = "",
                        telefone_1 = dt.Rows[0][11].ToString(),
                        telefone_2 = "",
                        responsavel = "",
                        responsavel_cargo = "",
                        responsavel_telefone = "",
                        responsavel_celular = "",
                        responsavel_email = "",
                    },

                    motorista_1 = new Motorista
                    {
                        nome = dm.Rows[0][0].ToString(),
                        cpf = dm.Rows[0][1].ToString(),
                        rg = dm.Rows[0][2].ToString(),
                        orgao_emissao = dm.Rows[0][3].ToString(),
                        rg_uf = "",
                        data_nascimento = dm.Rows[0][4].ToString(),
                        nome_mae = dm.Rows[0][5].ToString(),
                        estado_civil = dm.Rows[0][6].ToString(),
                        escolaridade = "",
                        cnh_numero = dm.Rows[0][7].ToString(),
                        cnh_categoria = dm.Rows[0][8].ToString(),
                        cnh_vencimento = dm.Rows[0][9].ToString(),
                        end_rua = dm.Rows[0][10].ToString(),
                        end_numero = "",
                        end_complemento = dm.Rows[0][11].ToString(),
                        end_bairro = dm.Rows[0][12].ToString(),
                        end_cidade = dm.Rows[0][13].ToString(),
                        end_uf = dm.Rows[0][14].ToString(),
                        end_cep = dm.Rows[0][15].ToString(),
                        fone = dm.Rows[0][16].ToString(),
                        celular = dm.Rows[0][17].ToString(),
                        nextel = "",
                        mopp = "",
                        aso = "",
                        cdd = "",
                        capacitacao = "",
                        vinculo = dm.Rows[0][18].ToString(),


                    },

                    veiculo = new Veiculo
                    {
                        placa = dv.Rows[0][0].ToString().Substring(0, 3) + "-" + placa.Substring(3),
                        renavan = dv.Rows[0][1].ToString(),
                        marca = dv.Rows[0][2].ToString(),
                        modelo = dv.Rows[0][3].ToString(),
                        cor = dv.Rows[0][4].ToString(),
                        ano = dv.Rows[0][5].ToString(),
                        tipo = dv.Rows[0][6].ToString(),
                        capacidade = dv.Rows[0][7].ToString(),
                        numero_antt = dv.Rows[0][8].ToString(),
                        validade_antt = "",
                        numero_frota = "",
                        transp_frota = "",
                        proprietario = "TRANSNOVAG TRANSPORTES SA",
                        proprietario_cpfcnpj = "55.890.016/0001-09",
                        end_rua = "Rua Cadiriri",
                        end_numero = "851",
                        end_complemento = "",
                        end_bairro = "Parque da Mooca",
                        end_cidade = "São Paulo",
                        end_uf = "SP",
                        end_cep = "03109-040",
                        tecnologia = dv.Rows[0][18].ToString(),
                        id_rastreador = dv.Rows[0][19].ToString(),
                        comunicacao = dv.Rows[0][20].ToString(),
                        tecnologia_sec = "",
                        id_rastreador_sec = "",
                        comunicacao_sec = "",
                        fixo = "N"

                    },

                    reboque_1 = new Veiculo
                    {
                        placa = db1.Rows[0][0].ToString(),
                        renavan = db1.Rows[0][1].ToString() ?? "",
                        marca = db1.Rows[0][2].ToString(),
                        modelo = db1.Rows[0][3].ToString(),
                        cor = db1.Rows[0][4].ToString(),
                        ano = db1.Rows[0][5].ToString(),
                        tipo = db1.Rows[0][6].ToString(),
                        capacidade = db1.Rows[0][7].ToString(),
                        numero_antt = db1.Rows[0][8].ToString(),
                        validade_antt = "",
                        numero_frota = "",
                        transp_frota = "",
                        proprietario = db1.Rows[0][9].ToString(),
                        proprietario_cpfcnpj = db1.Rows[0][10].ToString(),
                        end_rua = db1.Rows[0][11].ToString(),
                        end_numero = db1.Rows[0][12].ToString(),
                        end_complemento = db1.Rows[0][13].ToString(),
                        end_bairro = db1.Rows[0][14].ToString(),
                        end_cidade = db1.Rows[0][15].ToString(),
                        end_uf = db1.Rows[0][16].ToString(),
                        end_cep = db1.Rows[0][17].ToString(),
                        tecnologia = db1.Rows[0][18].ToString(),
                        id_rastreador = db1.Rows[0][19].ToString(),
                        comunicacao = db1.Rows[0][20].ToString(),
                        tecnologia_sec = "",
                        id_rastreador_sec = "",
                        comunicacao_sec = "",
                        fixo = "N"
                    },

                    //reboque_2 = new Veiculo
                    //{
                    //    placa = db2.Rows[0][0].ToString(),
                    //    renavam = db2.Rows[0][1].ToString(),

                    //    marca = db2.Rows[0][2].ToString(),
                    //    modelo = db2.Rows[0][3].ToString(),
                    //    cor = db2.Rows[0][4].ToString(),
                    //    ano = db2.Rows[0][5].ToString(),
                    //    tipo = db2.Rows[0][6].ToString(),
                    //    capacidade = db2.Rows[0][7].ToString(),
                    //    numero_att = db2.Rows[0][8].ToString(),
                    //    validade_antt = "",
                    //    numero_frota = "",
                    //    transp_frota = "",
                    //    proprietario = db2.Rows[0][9].ToString(),
                    //    proprietario_cnpj = db2.Rows[0][10].ToString(),
                    //    end_rua = db2.Rows[0][11].ToString(),
                    //    end_numero = db2.Rows[0][12].ToString(),
                    //    end_complemento = db2.Rows[0][13].ToString(),
                    //    end_bairro = db2.Rows[0][14].ToString(),
                    //    end_cidade = db2.Rows[0][15].ToString(),
                    //    end_uf = db2.Rows[0][16].ToString(),
                    //    end_cep = db2.Rows[0][17].ToString(),
                    //    tecnologia = db2.Rows[0][18].ToString(),
                    //    id_rastreador = db2.Rows[0][19].ToString(),
                    //    comunicacao = db2.Rows[0][20].ToString(),
                    //    tecnologia_sec = "",
                    //    id_rastreador_sec = "",
                    //    comunicacao_sec = "",
                    //    fixo = "N"
                    //},

                    origem = new EntidadeCompleta
                    {
                        tipo = "TRANSPORTADOR",
                        cnpj = dto.Rows[0][0].ToString(),
                        razao_social = dto.Rows[0][1].ToString(),
                        nome_fantasia = dto.Rows[0][2].ToString(),
                        unidade = dto.Rows[0][3].ToString(),
                        codigo = "",
                        end_rua = dto.Rows[0][4].ToString(),
                        end_numero = dto.Rows[0][5].ToString(),
                        end_complemento = dto.Rows[0][6].ToString(),
                        end_bairro = dto.Rows[0][7].ToString(),
                        end_cidade = dto.Rows[0][8].ToString(),
                        end_uf = dto.Rows[0][9].ToString(),
                        end_cep = dto.Rows[0][10].ToString(),
                        latitude = "",
                        longitude = "",
                        telefone_1 = dto.Rows[0][11].ToString(),
                        telefone_2 = "",
                        responsavel = "",
                        responsavel_cargo = "",
                        responsavel_telefone = "",
                        responsavel_celular = "",
                        responsavel_email = "",
                    },

                    destinos = new Dictionary<string, Destino>
                    {
                        {
                            "1", new Destino
                            {
                                // --- CAMPOS DO CLIENTE (Raiz do nó "1") ---
                                tipo = "CLIENTE",
                                cnpj = dtd.Rows[0][0].ToString(),
                                razao_social = dtd.Rows[0][1].ToString(),
                                nome_fantasia = dtd.Rows[0][2].ToString(),
                                unidade = dtd.Rows[0][3].ToString(),
                                codigo = "",
                                end_rua = dtd.Rows[0][4].ToString(),
                                end_numero = dtd.Rows[0][5].ToString(),
                                end_complemento = dtd.Rows[0][6].ToString(),
                                end_bairro = dtd.Rows[0][7].ToString(),
                                end_cidade = dtd.Rows[0][8].ToString(),
                                end_uf = dtd.Rows[0][9].ToString(),
                                end_cep = dtd.Rows[0][10].ToString(),
                                telefone_1 = dtd.Rows[0][11].ToString(),
                                latitude = "",
                                longitude = "",
                                telefone_2 = "",
                                responsavel = "",
                                responsavel_cargo = "",
                                responsavel_telefone = "",
                                responsavel_celular = "",
                                responsavel_email = "",

                                // --- NÓ DADOS_ADICIONAIS (Conforme o exemplo) ---
                                dados_adicionais = new DadosAdicionais
                                {
                                    remetente = new EntidadeCompleta
                                    {
                                        tipo = "TRANSPORTADOR",
                                        cnpj = dte.Rows[0][0].ToString(),
                                        razao_social = dte.Rows[0][1].ToString(),
                                        nome_fantasia = dte.Rows[0][2].ToString(),
                                        unidade = dte.Rows[0][3].ToString(),
                                        codigo = "",
                                        end_rua = dte.Rows[0][4].ToString(),
                                        end_numero = dte.Rows[0][5].ToString(),
                                        end_complemento = dte.Rows[0][6].ToString(),
                                        end_bairro = dte.Rows[0][7].ToString(),
                                        end_cidade = dte.Rows[0][8].ToString(),
                                        end_uf = dte.Rows[0][9].ToString(),
                                        end_cep = dte.Rows[0][10].ToString(),
                                        telefone_1 = dte.Rows[0][11].ToString(),
                                        latitude = "", longitude = "", telefone_2 = "", responsavel = "",
                                        responsavel_cargo = "", responsavel_telefone = "", responsavel_celular = "", responsavel_email = ""
                                    },
                                    mercadoria = "",
                                    valor = "",
                                    norma = "",
                                    grupo_norma = "",
                                    nota = "",
                                    observacao = ""
                                }
                            }
                        }
                    },

                    viagem = new Viagem
                    {
                        tipo_viagem = "ENTREGA ÚNICA",
                        rastreada = "S",
                        percurso = percurso.ToUpper(),
                        tipo_cliente = "TRANSPORTADOR",
                        doca_origem = "",
                        fpp = "",
                        mercadoria_id = "12",
                        valor = valor.Replace(".", "").Replace(",", "."),
                        peso_total = peso.Replace(",000", ""),
                        rota = rota,
                        rota_id = id_rota,
                        inicio_previsto = DateTime.Parse(previsao_inicial).ToString("yyyy-MM-dd HH:mm:ss"),
                        fim_previsto = DateTime.Parse(previsao_final).ToString("yyyy-MM-dd HH:mm:ss"),
                        liberacao = idCarga,
                        numero_cliente = dg.Rows[0][3].ToString(),
                        observacao = "",
                        localizador1_1 = "",
                        id_localizador1_1 = "",
                        localizador1_2 = "",
                        id_localizador1_2 = "",
                        localizador1_3 = "",
                        id_localizador1_3 = "",
                        localizador2_1 = "",
                        id_localizador2_1 = "",
                        localizador2_2 = "",
                        id_localizador2_2 = "",
                        localizador2_3 = "",
                        id_localizador2_3 = "",
                        localizador3_1 = "",
                        id_localizador3_1 = "",
                        localizador3_2 = "",
                        id_localizador3_2 = "",
                        localizador3_3 = "",
                        id_localizador3_3 = ""

                    }
                }
            };
        }

        private string EnviarRequisicaoKronaSincrona(string jsonContent)
        {
            string url = "https://k1.grupokrona.com.br/kronaservice/viagem_new";

            using (var client = new HttpClient())
            {
                // 1. Criamos o conteúdo
                var content = new StringContent(jsonContent, Encoding.UTF8);

                // 2. Limpamos o Content-Type padrão que o .NET coloca (que vem com o charset)
                content.Headers.ContentType.Parameters.Clear();

                // 3. Forçamos apenas application/json sem mais nada
                content.Headers.ContentType.MediaType = "application/json";

                // 4. Executa a chamada
                var response = client.PostAsync(url, content).Result;
                return response.Content.ReadAsStringAsync().Result;
            }
        }

        // MÉTODO PARA SALVAR NO BANCO


        private void ProcessarESalvarRetorno(string jsonResposta, string idCarga, string valor, string previsao_inicial, string previsao_final, string percurso, string rota, string id_rota)
        {
            try
            {
                string nomeUsuario = Session["UsuarioLogado"].ToString();
                if (string.IsNullOrEmpty(jsonResposta))
                {
                    MostrarMsg("Erro: A API da Krona não retornou nenhum dado.");
                    return;
                }

                var retorno = JsonConvert.DeserializeObject<KronaResponse>(jsonResposta);

                // Tenta pegar o número da SM de qualquer um dos campos possíveis
                string numeroSM = retorno?.protocolo ?? retorno?.sm ?? "";
                string status = retorno?.status ?? "";

                // Tenta pegar a mensagem de erro de qualquer campo
                string mensagemErro = retorno?.mensagem ?? retorno?.erro ?? "";

                // Se NÃO tem número de SM, precisamos saber o motivo real
                if (string.IsNullOrEmpty(numeroSM))
                {
                    // Se a mensagem de erro também estiver vazia, mostramos o JSON bruto para depurar
                    string msgFinal = !string.IsNullOrEmpty(mensagemErro) ? mensagemErro : "Resposta bruta: " + jsonResposta;
                    MostrarMsg("Krona não gerou SM. Motivo: " + msgFinal);
                    return;
                }

                using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
                {
                    // SQL corrigido com a vírgula antes de usu_envio_krona
                    string sql = @"UPDATE tbcargas 
                            SET num_sm=@num_sm, 
                                percurso=@percurso, 
                                valor_total=@valor_total, 
                                previsao_inicio_krona=@previsao_inicio_krona, 
                                previsao_termino_krona=@previsao_termino_krona, 
                                usu_envio_krona=@usu_envio_krona,
                                rota_krona=@rota_krona,
                                id_rota_krona = @id_rota_krona
                            WHERE carga = @carga";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@num_sm", numeroSM);
                    cmd.Parameters.AddWithValue("@carga", idCarga);
                    cmd.Parameters.AddWithValue("@percurso", percurso);
                    cmd.Parameters.AddWithValue("@valor_total", valor.Replace(".", "").Replace(",", "."));
                    cmd.Parameters.AddWithValue("@previsao_inicio_krona", DateTime.Parse(previsao_inicial));
                    cmd.Parameters.AddWithValue("@previsao_termino_krona", DateTime.Parse(previsao_final));
                    cmd.Parameters.AddWithValue("@usu_envio_krona", nomeUsuario ?? "SISTEMA");
                    cmd.Parameters.AddWithValue("@rota_krona", rota);
                    cmd.Parameters.AddWithValue("@id_rota_krona", id_rota);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                MostrarMsg("Sucesso! SM Gerada: " + numeroSM);
                Session["Coletas"] = null;
                CarregarColetas(novaColeta.Text);
            }
            catch (Exception ex)
            {
                MostrarMsg("Erro ao processar SM: " + ex.Message);
            }
        }

        protected void ddlMotorista_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtCodMotorista.Text = ddlMotorista.SelectedValue;
            string sql = @"SELECT codmot, nommot, status, cargo, nucleo, cpf, venccnh, codliberacao, validade, venceti, cartaomot, tipomot, venccartao, ISNULL(caminhofoto, '/fotos/motoristasemfoto.jpg') AS caminhofoto,fone2, codtra, transp, frota 
                        FROM tbmotoristas 
                        WHERE codmot = @id";

            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@id", txtCodMotorista.Text);

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    MostrarMsg("Motorista: " + txtCodMotorista.Text.Trim() + ", não cadastrado no sistema. Verifique!", "danger");
                    fotoMotorista = "/fotos/motoristasemfoto.jpg";
                    txtCodMotorista.Text = "";
                    txtCodMotorista.Focus();
                    return;
                }

                txtCodMotorista.Text = dt.Rows[0]["codmot"].ToString();
                ddlMotorista.SelectedItem.Text = dt.Rows[0]["nommot"].ToString();
                txtFilialMot.Text = dt.Rows[0]["nucleo"].ToString();
                txtTipoMot.Text = dt.Rows[0]["tipomot"].ToString();
                txtFuncao.Text = dt.Rows[0]["cargo"].ToString();

                if (dt.Rows[0]["venceti"].ToString() != "")
                {
                    txtExameToxic.Text = Convert.ToDateTime(dt.Rows[0]["venceti"]).ToString("dd/MM/yyyy");

                }
                if (dt.Rows[0]["venccnh"].ToString() != "")
                {
                    txtCNH.Text = Convert.ToDateTime(dt.Rows[0]["venccnh"]).ToString("dd/MM/yyyy");
                }
                if (dt.Rows[0]["validade"].ToString() != "")
                {
                    txtLibGR.Text = Convert.ToDateTime(dt.Rows[0]["validade"]).ToString("dd/MM/yyyy");
                }

                txtCelular.Text = dt.Rows[0]["fone2"].ToString();
                txtCPF.Text = dt.Rows[0]["cpf"].ToString();
                txtCartao.Text = dt.Rows[0]["cartaomot"].ToString();
                txtValCartao.Text = dt.Rows[0]["venccartao"].ToString();
                txtCodTransportadora.Text = dt.Rows[0]["codtra"].ToString();
                txtTransportadora.Text = dt.Rows[0]["transp"].ToString();
                fotoMotorista = dt.Rows[0]["caminhofoto"].ToString();
                txtLiberacao.Text = dt.Rows[0]["codliberacao"].ToString();

                // valida Exame Toxicologico
                if (dt.Rows[0]["tipomot"].ToString() == "FUNCIONÁRIO")
                {
                    DateTime dataETI;
                    if (txtExameToxic.Text != "")
                    {
                        if (!DateTime.TryParse(txtExameToxic.Text, out dataETI))
                        {
                            MostrarMsg("Exame Toxicologico do " + ddlMotorista.SelectedItem.Text.Trim() + ", não foi lançado. Verifique", "danger");

                            txtCodMotorista.Text = "";
                            txtCodMotorista.Focus();

                        }
                        else
                        {
                            DateTime validadeET = Convert.ToDateTime(dt.Rows[0]["venceti"]);
                            TimeSpan diferencaET = validadeET - DateTime.Today;

                            if (validadeET < DateTime.Today)
                            {
                                MostrarMsg("Exame Toxicologico do " + ddlMotorista.SelectedItem.Text.Trim() + ", está VENCIDO. Verifique!", "danger");
                                txtExameToxic.BackColor = System.Drawing.Color.Red;
                                txtExameToxic.ForeColor = System.Drawing.Color.White;
                                txtCodMotorista.Text = "";
                                txtCodMotorista.Focus();
                            }
                            else if (diferencaET.TotalDays <= 30)
                            {
                                MostrarMsg("Atenção! Exame Toxicologico do " + ddlMotorista.SelectedItem.Text.Trim() + ", vence em " + diferencaET.Days + " dias.", "warning");
                                txtExameToxic.BackColor = System.Drawing.Color.Khaki;
                                txtExameToxic.ForeColor = System.Drawing.Color.OrangeRed;
                            }
                        }
                    }
                }
                // valida CNH
                DateTime dataCNH;
                if (txtCNH.Text != "")
                {
                    if (!DateTime.TryParse(txtCNH.Text, out dataCNH))
                    {
                        MostrarMsgCNH("Validade da CNH do " + ddlMotorista.SelectedItem.Text.Trim() + ", não foi lançada. Verifique!", "danger");
                        txtCodMotorista.Text = "";
                        txtCodMotorista.Focus();
                    }
                    else
                    {
                        // valida cnh
                        DateTime validadeCNH = Convert.ToDateTime(dt.Rows[0]["venccnh"]);
                        TimeSpan diferencaCNH = validadeCNH - DateTime.Today;

                        if (validadeCNH < DateTime.Today)
                        {
                            MostrarMsgCNH("CNH do motorista " + ddlMotorista.SelectedItem.Text.Trim() + ", está VENCIDA. Verifique!", "danger");
                            txtCNH.BackColor = System.Drawing.Color.Red;
                            txtCNH.ForeColor = System.Drawing.Color.White;
                            txtCodMotorista.Text = "";
                            txtCodMotorista.Focus();
                        }
                        else if (diferencaCNH.TotalDays <= 30)
                        {
                            MostrarMsgCNH("Atenção! A CNH do " + ddlMotorista.SelectedItem.Text.Trim() + ", vence em " + diferencaCNH.Days + " dias.", "warning");
                            txtCNH.BackColor = System.Drawing.Color.Khaki;
                            txtCNH.ForeColor = System.Drawing.Color.OrangeRed;
                        }
                    }
                }
                // valida GR
                DateTime dataGR;
                if (txtLibGR.Text != "")
                {
                    if (!DateTime.TryParse(txtLibGR.Text, out dataGR))
                    {
                        MostrarMsgGR("Validade da Liberação de Risco do " + ddlMotorista.SelectedItem.Text.Trim() + ", não foi lançada. Verifique!", "danger");
                        txtCodMotorista.Text = "";
                        txtCodMotorista.Focus();
                    }
                    else
                    {
                        // valida liberação de risco 
                        DateTime validadeGR = Convert.ToDateTime(dt.Rows[0]["validade"]);
                        TimeSpan diferencaGR = validadeGR - DateTime.Today;

                        if (validadeGR < DateTime.Today)
                        {
                            MostrarMsgGR("Liberação de Risco do " + ddlMotorista.SelectedItem.Text.Trim() + ", está VENCIDA. Verifique!.", "danger");
                            txtLibGR.BackColor = System.Drawing.Color.Red;
                            txtLibGR.ForeColor = System.Drawing.Color.White;
                            txtCodMotorista.Text = "";
                            txtCodMotorista.Focus();
                        }
                        else if (diferencaGR.TotalDays <= 30)
                        {
                            MostrarMsgGR("Atenção! Liberação de Risco do motorista " + ddlMotorista.SelectedItem.Text.Trim() + ", vence em " + diferencaGR.Days + " dias.", "warning");
                            txtLibGR.BackColor = System.Drawing.Color.Khaki;
                            txtLibGR.ForeColor = System.Drawing.Color.OrangeRed;
                        }
                    }
                }
                txtCodVeiculo.Text = dt.Rows[0]["frota"].ToString();

            }

            //if(txtTipoMot.Text != "FUNCIONÁRIO")
            //{
            //Dados do veiculos
            string sqlVeiculos = @"SELECT codvei, plavei, reboque1, reboque2, tipoveiculo, tipvei, tiporeboque, tipocarreta, vencimentolaudofumaca, venclicenciamento, codtra, transp, venclicencacet, protocolocet, venccronotacografo, rastreamento, rastreador, ativo_inativo, fl_exclusao
                            FROM tbveiculos 
                            WHERE codvei = @idVeiculo AND ativo_inativo = 'ATIVO' AND fl_exclusao IS NULL";
            using (SqlCommand cmdVeiculos = new SqlCommand(sqlVeiculos, con))
            {
                cmdVeiculos.Parameters.AddWithValue("@idVeiculo", txtCodVeiculo.Text.Trim());

                DataTable dtVeiculos = new DataTable();
                SqlDataAdapter daVeiculos = new SqlDataAdapter(cmdVeiculos);

                daVeiculos.Fill(dtVeiculos);

                if (dtVeiculos.Rows.Count == 0)
                {
                    MostrarMsgVeic(txtPlaca.Text.Trim() + " - Veículo NÂO encontrado na base de dados ou INATIVO. Verifique!", "danger");
                    //LimparCamposMotorista();
                    return;
                }

                if (dtVeiculos.Rows[0]["ativo_inativo"].ToString() == "INATIVO" || dtVeiculos.Rows[0]["fl_exclusao"].ToString() == "S")
                {
                    MostrarMsgVeic(txtPlaca.Text.Trim() + " - Veículo INATIVO ou EXCLUIDO da base de dados. Verique!", "danger");
                    //LimparCamposMotorista();
                    return;

                }

                if (dtVeiculos.Rows[0]["tipvei"].ToString() == "CAVALO TRUCADO")
                {
                    carretas.Visible = true;
                    reboque1.Visible = true;
                }
                else if (dtVeiculos.Rows[0]["tipvei"].ToString() == "CAVALO SIMPLES")
                {
                    carretas.Visible = true;
                    reboque1.Visible = true;
                }
                else if (dtVeiculos.Rows[0]["tipvei"].ToString() == "CAVALO 4 EIXOS")
                {
                    carretas.Visible = true;
                    reboque1.Visible = true;
                }
                else if (dtVeiculos.Rows[0]["tipvei"].ToString() == "BITREM")
                {
                    carretas.Visible = true;
                    reboque1.Visible = true;
                    reboque2.Visible = true;
                }
                else
                {
                    carretas.Visible = false;
                    reboque1.Visible = false;
                    reboque2.Visible = false;
                }


                // Dados do veículo
                txtCodVeiculo.Text = dtVeiculos.Rows[0]["codvei"].ToString();
                txtPlaca.Text = dtVeiculos.Rows[0]["plavei"].ToString();
                txtReboque1.Text = dtVeiculos.Rows[0]["reboque1"].ToString();
                txtReboque2.Text = dtVeiculos.Rows[0]["reboque2"].ToString();

                txtVeiculoTipo.Text = dtVeiculos.Rows[0]["tipoveiculo"].ToString();
                txtTipoVeiculo.Text = dtVeiculos.Rows[0]["tipvei"].ToString().Trim();
                txtCarreta.Text = dtVeiculos.Rows[0]["tiporeboque"].ToString();
                txtConjunto.Text = dtVeiculos.Rows[0]["tipocarreta"].ToString();
                txtCodProprietario.Text = dtVeiculos.Rows[0]["codtra"].ToString();
                txtProprietario.Text = dtVeiculos.Rows[0]["transp"].ToString();
                txtTecnologia.Text = dtVeiculos.Rows[0]["rastreamento"].ToString();
                txtRastreamento.Text = dtVeiculos.Rows[0]["rastreador"].ToString();

                txtOpacidade.Text = dtVeiculos.Rows[0]["vencimentolaudofumaca"].ToString();
                txtCET.Text = dtVeiculos.Rows[0]["venclicencacet"].ToString();
                txtProtocoloCET.Text = dtVeiculos.Rows[0]["protocolocet"].ToString();
                txtCRLVVeiculo.Text = dtVeiculos.Rows[0]["venclicenciamento"].ToString();
                txtCrono.Text = dtVeiculos.Rows[0]["venccronotacografo"].ToString();



                // valida Laudo de Fumaça
                DateTime dataOpacidade;
                if (!DateTime.TryParse(txtOpacidade.Text, out dataOpacidade))
                {
                    MostrarMsgVeic(txtPlaca.Text.Trim() + " - Laudo de OPACIDADE(Fumaça), não foi lançado.", "danger");
                    txtCodMotorista.Text = "";
                    txtCodMotorista.Focus();
                }
                else
                {
                    DateTime validadeOpacidade = Convert.ToDateTime(dtVeiculos.Rows[0]["vencimentolaudofumaca"]);
                    TimeSpan diferencaOpacidade = validadeOpacidade - DateTime.Today;

                    if (validadeOpacidade < DateTime.Today)
                    {
                        MostrarMsgVeic(txtPlaca.Text.Trim() + " - Laudo de OPACIDADE(Fumaça), está VENCIDO.", "danger");
                        txtOpacidade.BackColor = System.Drawing.Color.Red;
                        txtOpacidade.ForeColor = System.Drawing.Color.White;
                        txtCodMotorista.Text = "";
                        txtCodMotorista.Focus();
                    }
                    else if (diferencaOpacidade.TotalDays <= 30)
                    {
                        MostrarMsgVeic(txtPlaca.Text.Trim() + " - Atenção! Laudo de OPACIDADE(Fumaça) vence em " + diferencaOpacidade.Days + " dias.", "warning");
                        txtOpacidade.BackColor = System.Drawing.Color.Khaki;
                        txtOpacidade.ForeColor = System.Drawing.Color.OrangeRed;
                    }
                }

                // valida Cronotacografo
                DateTime dataCrono;
                if (!DateTime.TryParse(txtCrono.Text, out dataCrono))
                {
                    MostrarMsgCrono(txtPlaca.Text.Trim() + " - Laudo do CRONOTACOGRAFO, não foi lançado. Verifique!", "danger");
                    txtCodMotorista.Text = "";
                    txtCodMotorista.Focus();
                }
                else
                {
                    DateTime validadeCrono = Convert.ToDateTime(dtVeiculos.Rows[0]["venccronotacografo"]);
                    TimeSpan diferencaCrono = validadeCrono - DateTime.Today;

                    if (validadeCrono < DateTime.Today)
                    {
                        MostrarMsgCrono(txtPlaca.Text.Trim() + " - Laudo do CRONOTACOGRAFO, está VENCIDO. Verifique!", "danger");
                        txtCrono.BackColor = System.Drawing.Color.Red;
                        txtCrono.ForeColor = System.Drawing.Color.White;
                        txtCodMotorista.Text = "";
                        txtCodMotorista.Focus();
                    }
                    else if (diferencaCrono.TotalDays <= 30)
                    {
                        MostrarMsgCrono(txtPlaca.Text.Trim() + " - Atenção! Laudo do CRONOTACOGRAFO vence em " + diferencaCrono.Days + " dias.", "warning");
                        txtCrono.BackColor = System.Drawing.Color.Khaki;
                        txtCrono.ForeColor = System.Drawing.Color.OrangeRed;
                    }
                }

                // valida Licenciamento
                DateTime dataLinc;
                if (!DateTime.TryParse(txtCRLVVeiculo.Text, out dataLinc))
                {
                    MostrarMsgLinc(txtPlaca.Text.Trim() + " - LICENCIAMENTO do veículo, não foi lançado. Verifique!", "danger");
                    txtCodMotorista.Text = "";
                    txtCodMotorista.Focus();
                }
                else
                {
                    DateTime validadeLinc = Convert.ToDateTime(dtVeiculos.Rows[0]["vencimentolaudofumaca"]);
                    TimeSpan diferencaLinc = validadeLinc - DateTime.Today;

                    if (validadeLinc < DateTime.Today)
                    {
                        MostrarMsgLinc(txtPlaca.Text.Trim() + " - LICENCIAMENTO do veículo, está VENCIDO. Verifique!", "danger");
                        txtCRLVVeiculo.BackColor = System.Drawing.Color.Red;
                        txtCRLVVeiculo.ForeColor = System.Drawing.Color.White;
                        txtCodMotorista.Text = "";
                        txtCodMotorista.Focus();
                    }
                    else if (diferencaLinc.TotalDays <= 30)
                    {
                        MostrarMsgLinc(txtPlaca.Text.Trim() + " - Atenção! LICENCIAMENTO do veículo, vence em " + diferencaLinc.Days + " dias.", "warning");
                        txtCRLVVeiculo.BackColor = System.Drawing.Color.Khaki;
                        txtCRLVVeiculo.ForeColor = System.Drawing.Color.OrangeRed;
                    }
                }

                // valida CET
                DateTime dataCET;
                if (!DateTime.TryParse(txtCET.Text, out dataCET))
                {
                    MostrarMsgCET(txtPlaca.Text.Trim() + " - LICENÇA CET para o veículo, não foi lançado. Verifique!", "danger");
                    txtCodMotorista.Text = "";
                    txtCodMotorista.Focus();
                }
                else
                {
                    DateTime validadeCET = Convert.ToDateTime(dtVeiculos.Rows[0]["venclicencacet"]);
                    TimeSpan diferencaCET = validadeCET - DateTime.Today;

                    if (validadeCET < DateTime.Today)
                    {
                        MostrarMsgCET(txtPlaca.Text.Trim() + " - LICENÇA CET do veículo, está VENCIDA. Verifique!", "danger");
                        txtCET.BackColor = System.Drawing.Color.Red;
                        txtCET.ForeColor = System.Drawing.Color.White;
                        txtCodMotorista.Text = "";
                        txtCodMotorista.Focus();
                    }
                    else if (diferencaCET.TotalDays <= 30)
                    {
                        MostrarMsgCET(txtPlaca.Text.Trim() + " - Atenção! LICENÇA CET do veículo, vence em " + diferencaCET.Days + " dias.", "warning");
                        txtCET.BackColor = System.Drawing.Color.Khaki;
                        txtCET.ForeColor = System.Drawing.Color.OrangeRed;
                    }
                }



            }

            // Dados da primeira carreta
            if (txtReboque1.Text != "")
            {
                string sqlCarreta1 = @"SELECT placacarreta, licenciamento, ativo_inativo, fl_exclusao
                            FROM tbcarretas 
                            WHERE placacarreta = @idCarreta1 AND ativo_inativo = 'ATIVO' AND fl_exclusao IS NULL";
                using (SqlCommand cmdCarreta1 = new SqlCommand(sqlCarreta1, con))
                {
                    cmdCarreta1.Parameters.AddWithValue("@idCarreta1", txtReboque1.Text.Trim());

                    DataTable dtCarreta1 = new DataTable();
                    SqlDataAdapter daCarreta1 = new SqlDataAdapter(cmdCarreta1);

                    daCarreta1.Fill(dtCarreta1);

                    if (dtCarreta1.Rows.Count == 0)
                    {
                        MostrarMsgCarreta1(txtReboque1.Text.Trim() + " - Carreta NÂO encontrada na base de dados ou INATIVA. Verifique!", "danger");
                        //LimparCamposMotorista();
                        return;
                    }

                    if (dtCarreta1.Rows[0]["ativo_inativo"].ToString() == "INATIVO" || dtCarreta1.Rows[0]["fl_exclusao"].ToString() == "S")
                    {
                        MostrarMsgCarreta1(txtReboque1.Text.Trim() + " - Carreta INATIVA ou EXCLUIDA da base de dados. Verique!", "danger");
                        //LimparCamposMotorista();
                        return;

                    }

                    // Dados da Carreta 
                    txtCRLVReb1.Text = dtCarreta1.Rows[0]["licenciamento"].ToString();
                    // valida Licenciamento reboque 1
                    DateTime dataLincReb1;
                    if (!DateTime.TryParse(txtCRLVReb1.Text, out dataLincReb1))
                    {
                        MostrarMsgCarreta1(txtReboque1.Text.Trim() + " - LICENCIAMENTO da carreta, não foi lançado. Verifique!", "danger");
                        //return;
                    }
                    else
                    {
                        DateTime validadeLincReb1 = Convert.ToDateTime(dtCarreta1.Rows[0]["licenciamento"]);
                        TimeSpan diferencaLincReb1 = validadeLincReb1 - DateTime.Today;

                        if (validadeLincReb1 < DateTime.Today)
                        {
                            MostrarMsgCarreta1(txtReboque1.Text.Trim() + " - LICENCIAMENTO da carreta, está VENCIDO. Verifique!", "danger");
                            txtCRLVReb1.BackColor = System.Drawing.Color.Red;
                            txtCRLVReb1.ForeColor = System.Drawing.Color.White;
                        }
                        else if (diferencaLincReb1.TotalDays <= 30)
                        {
                            MostrarMsgCarreta1(txtReboque1.Text.Trim() + " - Atenção! LICENCIAMENTO da carreta, vence em " + diferencaLincReb1.Days + " dias.", "warning");
                            txtCRLVReb1.BackColor = System.Drawing.Color.Khaki;
                            txtCRLVReb1.ForeColor = System.Drawing.Color.OrangeRed;
                        }
                    }
                }

            }


            // Dados da segunda carreta
            if (txtReboque2.Text != "")
            {
                string sqlCarreta2 = @"SELECT placacarreta, licenciamento, ativo_inativo, fl_exclusao
                        FROM tbcarretas 
                        WHERE placacarreta = @idCarreta2 AND ativo_inativo = 'ATIVO' AND fl_exclusao IS NULL";
                using (SqlCommand cmdCarreta2 = new SqlCommand(sqlCarreta2, con))
                {
                    cmdCarreta2.Parameters.AddWithValue("@idCarreta2", txtReboque2.Text.Trim());

                    DataTable dtCarreta2 = new DataTable();
                    SqlDataAdapter daCarreta2 = new SqlDataAdapter(cmdCarreta2);

                    daCarreta2.Fill(dtCarreta2);

                    if (dtCarreta2.Rows.Count == 0)
                    {
                        MostrarMsgCarreta2(txtReboque2.Text.Trim() + " - Carreta NÂO encontrada na base de dados ou INATIVA. Verifique!", "danger");
                        //LimparCamposMotorista();
                        return;
                    }

                    if (dtCarreta2.Rows[0]["ativo_inativo"].ToString() == "INATIVO" || dtCarreta2.Rows[0]["fl_exclusao"].ToString() == "S")
                    {
                        MostrarMsgCarreta2(txtReboque2.Text.Trim() + " - Carreta INATIVA ou EXCLUIDA da base de dados. Verique!", "danger");
                        //LimparCamposMotorista();
                        return;

                    }

                    // Dados da Carreta 2
                    txtCRLVReb2.Text = dtCarreta2.Rows[0]["licenciamento"].ToString();
                    // valida Licenciamento reboque 1
                    DateTime dataLincReb2;
                    if (!DateTime.TryParse(txtCRLVReb2.Text, out dataLincReb2))
                    {
                        MostrarMsgCarreta2(txtReboque2.Text.Trim() + " - LICENCIAMENTO da carreta, não foi lançado. Verifique!", "danger");
                        txtCodMotorista.Text = "";
                        txtCodMotorista.Focus();
                    }
                    else
                    {
                        DateTime validadeLincReb2 = Convert.ToDateTime(dtCarreta2.Rows[0]["licenciamento"]);
                        TimeSpan diferencaLincReb2 = validadeLincReb2 - DateTime.Today;

                        if (validadeLincReb2 < DateTime.Today)
                        {
                            MostrarMsgCarreta2(txtReboque2.Text.Trim() + " - LICENCIAMENTO da carreta, está VENCIDO. Verifique!", "danger");
                            txtCRLVReb2.BackColor = System.Drawing.Color.Red;
                            txtCRLVReb2.ForeColor = System.Drawing.Color.White;
                            txtCodMotorista.Text = "";
                            txtCodMotorista.Focus();
                        }
                        else if (diferencaLincReb2.TotalDays <= 30)
                        {
                            MostrarMsgCarreta2(txtReboque2.Text.Trim() + " - Atenção! LICENCIAMENTO da carreta, vence em " + diferencaLincReb2.Days + " dias.", "warning");
                            txtCRLVReb2.BackColor = System.Drawing.Color.Khaki;
                            txtCRLVReb2.ForeColor = System.Drawing.Color.OrangeRed;
                        }
                    }
                }

            }
         }
        //private void PreencherComboMotoristas()
        //{
        //    // Consulta SQL que retorna os dados desejados
        //    string query = "SELECT codmot, nommot FROM tbmotoristas WHERE fl_exclusao is null AND status = 'ATIVO' ORDER BY nommot";

        //    // Crie uma conexão com o banco de dados
        //    using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
        //    {
        //        //try
        //        //{
        //        // Abra a conexão com o banco de dados
        //        conn.Open();

        //        // Crie o comando SQL
        //        SqlCommand cmd = new SqlCommand(query, conn);



        //        SqlDataReader reader = cmd.ExecuteReader();

        //        // Preencher o ComboBox com os dados do DataReader
        //        ddlMotorista.DataSource = reader;
        //        ddlMotorista.DataTextField = "nommot";
        //        ddlMotorista.DataValueField = "codmot";
        //        ddlMotorista.DataBind();

        //        // motoristaBanco = nome do motorista vindo do banco
        //        string motoristaBanco = ddlMotorista.SelectedItem.Text.Trim(); /* ex.: "JOÃO SILVA" */;

        //        ddlMotorista.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Selecione o motorista...", ""));

        //        if (!string.IsNullOrWhiteSpace(motoristaBanco))
        //        {
        //            System.Web.UI.WebControls.ListItem item = ddlMotorista.Items.FindByText(motoristaBanco);

        //            if (item != null)
        //            {
        //                ddlMotorista.ClearSelection();
        //                item.Selected = true;
        //            }
        //        }



        //        //ddlMotorista.Items.Insert(0, "");

        //        // Feche o reader
        //        reader.Close();
        //        //}
        //        //catch (Exception ex)
        //        //{
        //        //    // Trate exceções
        //        //    Response.Write("Erro: " + ex.Message);
        //        //}
        //    }
        //}

        private void CarregarMotoristas()
        {
            // Guarda o motorista selecionado
            string codMotorista = ddlMotorista.SelectedValue;

            string query = @"SELECT codmot, nommot
                     FROM tbmotoristas
                     WHERE fl_exclusao IS NULL
                       AND status = 'ATIVO'
                     ORDER BY nommot";

            using (SqlConnection conn = new SqlConnection(
                WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    ddlMotorista.DataSource = dr;
                    ddlMotorista.DataTextField = "nommot";
                    ddlMotorista.DataValueField = "codmot";
                    ddlMotorista.DataBind();
                }
            }

            ddlMotorista.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Selecione o motorista...", ""));

            // Restaura a seleção
            if (!string.IsNullOrWhiteSpace(codMotorista) &&
                ddlMotorista.Items.FindByValue(codMotorista) != null)
            {
                ddlMotorista.SelectedValue = codMotorista;
            }
        }


        protected void btnPesquisarVeiculo_Click(object sender, EventArgs e)
        {
            if (txtCodVeiculo.Text.Trim() == "")
            {
                string nomeUsuario = txtAtualizadoPor.Text;

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

                        string linha1 = "Olá, " + Session["UsuarioLogado"].ToString() + "!";
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
                        // verifica se o motorista pertence a transportadora
                        if (txtCodTransportadora.Text.Trim() != txtCodProprietario.Text.Trim())
                        {

                            string nomeUsuario = txtUsuCadastro.Text;
                            string linha1 = "Olá, " + Session["UsuarioLogado"].ToString() + "!";
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

                            //fotoMotorista = "../../fotos/motoristasemfoto.jpg";
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
                                    string linha1 = "Olá, " + Session["UsuarioLogado"].ToString() + "!";
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

                                    //fotoMotorista = "../../fotos/motoristasemfoto.jpg";
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
                                    string linha1 = "Olá, " + Session["UsuarioLogado"].ToString() + "!";
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

                                    //fotoMotorista = "../../fotos/motoristasemfoto.jpg";
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
                                    string linha1 = "Olá, " + Session["UsuarioLogado"].ToString() + "!";
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

                                    //fotoMotorista = "../../fotos/motoristasemfoto.jpg";
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
                                string linha1 = "Olá, " + Session["UsuarioLogado"].ToString() + "!";
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

                                //fotoMotorista = "../../fotos/motoristasemfoto.jpg";
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


                        // pesquisar primeiro reboque1
                        if (txtTipoVeiculo.Text.Trim() == "CAVALO SIMPLES" || txtTipoVeiculo.Text.Trim() == "CAVALO TRUCADO" || txtTipoVeiculo.Text.Trim() == "CAVALO 4 EIXOS")
                        {
                            if (txtReboque1.Text == "")
                            {
                                string nomeUsuario = txtUsuCadastro.Text;
                                string linha1 = "Olá, " + Session["UsuarioLogado"].ToString() + "!";
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

                                //fotoMotorista = "../../fotos/motoristasemfoto.jpg";
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
                        // Verifica reboque 2 - bitrem
                        if (txtTipoVeiculo.Text.Trim() == "BITREM")
                        {
                            if (txtReboque2.Text == "")
                            {
                                string nomeUsuario = txtUsuCadastro.Text;
                                string linha1 = "Olá, " + Session["UsuarioLogado"].ToString() + "!";
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

                                //fotoMotorista = "../../fotos/motoristasemfoto.jpg";
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

                    string nomeUsuario = txtUsuCadastro.Text;

                    string linha1 = "Olá, " + Session["UsuarioLogado"].ToString() + "!";
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
                string nomeUsuario = txtAtualizadoPor.Text;

                string linha1 = "Olá, " + Session["UsuarioLogado"].ToString() + "!";
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


                }
                else
                {
                    // Exemplo: abrir o modal ao carregar a página
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "abrirModalTelefone", "abrirModalTelefone();", true);
                }

            }
        }
        private void CarregarColetas(string idviagem = "")
        {
            // Obtem os dados atuais (novos dados)
            var novosDados = DAL.ConCargas.FetchDataTableColetasMatriz4(idviagem);

            // Verifica se há dados anteriores no Session
            DataTable dadosAtuais = Session["Coletas"] as DataTable;

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

            // Atualiza o Session
            Session["Coletas"] = dadosAtuais;

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
                            valcet = @valcet,
                            valcrlvveiculo = @valcrlvveiculo,
                            valcrlvreboque1 = @valcrlvreboque1,
                            valcrlvreboque2 = @valcrlvreboque2,
                            valopacidade = @valopacidade,
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
                            numero_gr = @numero_gr,
                            numero_protocolo_cet = numero_protocolo_cet, 
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
                cmd.Parameters.AddWithValue("@valtoxicologico", SafeDateValue2(txtExameToxic.Text));
                cmd.Parameters.AddWithValue("@venccnh", SafeDateCNH(txtCNH.Text));
                cmd.Parameters.AddWithValue("@valgr", SafeDateValue2(txtLibGR.Text));
                cmd.Parameters.AddWithValue("@foto", SafeValue(fotoMotorista)); // Se for byte[], troque tipo do parâmetro!
                cmd.Parameters.AddWithValue("@nomemotorista", SafeValue(ddlMotorista.SelectedItem.Text));
                cmd.Parameters.AddWithValue("@cpf", SafeValue(txtCPF.Text));
                cmd.Parameters.AddWithValue("@cartaopedagio", SafeValue(txtCartao.Text));
                cmd.Parameters.AddWithValue("@valcartao", txtValCartao.Text);
                cmd.Parameters.AddWithValue("@foneparticular", SafeValue(txtCelular.Text));
                cmd.Parameters.AddWithValue("@veiculo", SafeValue(txtCodVeiculo.Text));
                cmd.Parameters.AddWithValue("@veiculotipo", SafeValue(txtVeiculoTipo.Text));
                cmd.Parameters.AddWithValue("@valcet", SafeDateValue2(txtCET.Text));
                cmd.Parameters.AddWithValue("@valcrlvveiculo", SafeDateValue2(txtCRLVVeiculo.Text));
                cmd.Parameters.AddWithValue("@valcrlvreboque1", SafeDateValue2(txtCRLVReb1.Text));
                cmd.Parameters.AddWithValue("@valcrlvreboque2", SafeDateValue2(txtCRLVReb2.Text));
                cmd.Parameters.AddWithValue("@valopacidade", SafeDateValue2(txtOpacidade.Text));
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
                cmd.Parameters.AddWithValue("@numero_gr", SafeValue(txtLiberacao.Text));
                cmd.Parameters.AddWithValue("@numero_protocolo_cet", SafeValue(txtProtocoloCET.Text));


                //cmd.Parameters.AddWithValue("@comissao", SafeValue(txtComissao.Text));
                //cmd.Parameters.AddWithValue("@vlpernoite", SafeValue(txtPernoite.Text));
                //cmd.Parameters.AddWithValue("@cafe", SafeValue(txtCafe.Text));
                //cmd.Parameters.AddWithValue("@almoco", SafeValue(txtAlmoco.Text));
                //cmd.Parameters.AddWithValue("@janta", SafeValue(txtJanta.Text));

                cmd.Parameters.AddWithValue("@empresa", SafeValue("1111"));
                cmd.Parameters.AddWithValue("@dtalt", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                cmd.Parameters.AddWithValue("@usualt", Session["UsuarioLogado"].ToString());


                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    // CalculaKmTotal();
                    //nomeUsuario = txtUsuCadastro.Text;
                    //string mensagem = $"Olá, {nomeUsuario}!\nCarregamento atualizado no sistema com sucesso.";
                    //string mensagemCodificada = HttpUtility.JavaScriptStringEncode(mensagem);
                    //string script = $"alert('{mensagemCodificada}');";
                    //ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", script, true);


                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "Mensagem", "alert('Coletas salvas com sucesso!');", true);
                    Session["Coletas"] = null;
                    CarregarColetas(novaColeta.Text);


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

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            divMsg.Visible = true;
            divMsgCNH.Visible = false;
            divMsgCarreta1.Visible = false;
            divMsgCarreta2.Visible = false;
            divMsgCET.Visible = false;
            divMsgCrono.Visible = false;
            divMsgGR.Visible = false;
            divMsgLinc.Visible = false;
            divMsgVeic.Visible = false;
            divMsg.Attributes["class"] = "alert alert-danger d-none";

            if (string.IsNullOrWhiteSpace(txtCarga.Text))
            {
                MostrarMsg("Digite uma carga!");
                return;
            }

            BuscarCargaNoBanco(txtCarga.Text.Trim());
        }
        private string ValidarEncerramento()
        {
            using (SqlConnection conn = new SqlConnection(
                WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                // A lógica agora busca por qualquer documento em ambas as tabelas
                string sql = @"
            -- 1. Verifica se há cargas não concluídas
            DECLARE @NaoConcluidas INT;
            SELECT @NaoConcluidas = COUNT(*) FROM tbcargas WHERE idviagem = @idviagem AND status NOT IN ('Concluido', 'Liberado Vazio','Cancelada') ;

            -- 2. Verifica se existe PELO MENOS UM CT-e para esta viagem
            DECLARE @TotalCTe INT;
            SELECT @TotalCTe = COUNT(*) 
            FROM tbcte 
            WHERE id_viagem IN (SELECT carga FROM tbcargas WHERE idviagem = @idviagem);

            -- 3. Verifica se existe PELO MENOS UMA NFS-e para esta viagem
            DECLARE @TotalNFSe INT;
            SELECT @TotalNFSe = COUNT(*) 
            FROM tbnfse 
            WHERE idviagem IN (SELECT carga FROM tbcargas WHERE idviagem = @idviagem);

            -- 4. Verifica se a viagem possui algum material preenchido (exigência de documento)
            DECLARE @PossuiMaterial INT;
            SELECT @PossuiMaterial = COUNT(*) 
            FROM tbcargas 
            WHERE idviagem = @idviagem AND ISNULL(NULLIF(LTRIM(RTRIM(material)), ''), '') <> 'Vazio';

            SELECT @NaoConcluidas AS NaoConcluidas, 
                   (@TotalCTe + @TotalNFSe) AS TotalDocumentos, 
                   @PossuiMaterial AS PossuiMaterial";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@idviagem", novaColeta.Text);
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int naoConcluidas = Convert.ToInt32(reader["NaoConcluidas"]);
                            int totalDocumentos = Convert.ToInt32(reader["TotalDocumentos"]);
                            int possuiMaterial = Convert.ToInt32(reader["PossuiMaterial"]);

                            // Validação 1: Todas as cargas devem estar concluídas
                            if (naoConcluidas > 0)
                                return $"Existem {naoConcluidas} carga(s) pendentes de conclusão.";

                            // Validação 2: Se houver material, exige-se pelo menos um CT-e OU NFS-e
                            if (possuiMaterial > 0 && totalDocumentos == 0)
                                return "Não é possível encerrar: cargas com material exigem pelo menos um CT-e ou NFS-e anexado.";
                        }
                    }
                }
            }
            return null; // Retorna null se passar em todas as regras (OK para prosseguir)
        }

        private void BuscarCargaNoBanco(string carga)
        {
            // Consulta no banco
            if (txtCarga.Text.Trim() == "1")
            {
                PreencherNumCargaVazia();
                // Modal para incluir carga  
                ScriptManager.RegisterStartupScript(this, this.GetType(), "abrirModal", "abrirModal();", true);
            }
            else
            {
                using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
                {
                    using (SqlCommand cmd = new SqlCommand(@"
            SELECT id, carga, previsao, cliorigem, cidorigem, ufcliorigem, clidestino, ciddestino, ufclidestino, status, andamento, idviagem,mdfe
            FROM tbcargas
            WHERE carga = @c and idviagem is null
        ", conn))
                    {
                        cmd.Parameters.AddWithValue("@c", carga);

                        conn.Open();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        if (dt.Rows.Count == 0)
                        {
                            MostrarMsg("Carga NÃO encontrada!");
                            txtCarga.Text = "";
                            txtCarga.Focus();
                            return;
                        }
                        else
                        {
                            if (dt.Rows[0][10].ToString() == "Pendente" || dt.Rows[0][10].ToString() == "PENDENTE")
                            {
                                // ✔ Agora é seguro acessar dt.Rows[0]
                                //string carga = gvCargas.DataKeys[row.RowIndex].Value.ToString();

                                string updateCargas = @"
                                    UPDATE tbcargas SET
                                        emissao = @emissao,
                                        idviagem = @idviagem,
                                        codmot = @codmot,
                                        frota = @frota,
                                        status = @status,
                                        andamento = @andamento,
                                        atendimento = @atendimento,
                                        funcaomot = @funcaomot
                                    WHERE carga = @carga";

                                using (SqlCommand cmds = new SqlCommand(updateCargas, conn))
                                {
                                    cmds.Parameters.Add("@carga", SqlDbType.VarChar).Value = carga;
                                    cmds.Parameters.Add("@idviagem", SqlDbType.VarChar).Value = novaColeta.Text;
                                    cmds.Parameters.Add("@codmot", SqlDbType.VarChar).Value = txtCodMotorista.Text;
                                    cmds.Parameters.Add("@frota", SqlDbType.VarChar).Value = txtCodVeiculo.Text;
                                    cmds.Parameters.Add("@status", SqlDbType.VarChar).Value = "Pendente";
                                    cmds.Parameters.Add("@andamento", SqlDbType.VarChar).Value = "EM ANDAMENTO";
                                    cmds.Parameters.Add("@atendimento", SqlDbType.VarChar).Value = "";
                                    cmds.Parameters.Add("@funcaomot", SqlDbType.VarChar).Value = txtFuncao.Text;
                                    cmds.Parameters.Add("@emissao", SqlDbType.DateTime).Value = DateTime.Now;

                                    cmds.ExecuteNonQuery();
                                }

                                Session["Coletas"] = null;
                                CarregarColetas(novaColeta.Text);

                                txtCarga.Text = "";
                            }
                            else
                            {
                                MostrarMsg("CARGA: " + txtCarga.Text.Trim() + " está " + dt.Rows[0][10].ToString().Trim() + " NA ORDEM DE COLETA: " + dt.Rows[0][11].ToString().Trim() + ".");
                                txtCarga.Text = "";
                                txtCarga.Focus();
                                return;

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

        private object SafeDateValue2(string input)
        {
            DateTime dt;
            if (DateTime.TryParse(input, out dt))
                return dt.ToString("yyyy-MM-dd HH:mm");
            else
                return DBNull.Value;
        }
        private object SafeDateCNH(string input)
        {
            DateTime dt;
            if (DateTime.TryParse(input, out dt))
                return dt.ToString("yyyy-MM-dd");
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
        protected void txtCod_PagadorVazio_TextChanged(object sender, EventArgs e)
        {
            if (txtCod_PagadorVazio.Text != "")
            {

                string codigoPagador = txtCod_PagadorVazio.Text.Trim();
                string strConn = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    string query = "SELECT codcli, razcli, cidcli, estcli, codvw FROM tbclientes WHERE codcli = @Codigo OR codvw=@Codigo";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Codigo", codigoPagador);
                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtCod_PagadorVazio.Text = reader["codcli"].ToString();
                                txtPagadorVazio.Text = reader["razcli"].ToString();
                                txtCid_PagadorVazio.Text = reader["cidcli"].ToString();
                                txtUf_PagadorVazio.Text = reader["estcli"].ToString();
                                codCliFinal.Focus();
                            }
                            else
                            {
                                txtCod_PagadorVazio.Text = "";
                                // Aciona o Toast via JavaScript
                                ScriptManager.RegisterStartupScript(this, GetType(), "toastNaoEncontrado", "mostrarToastNaoEncontrado();", true);
                                txtCod_PagadorVazio.Focus();
                                // Opcional: exibir mensagem ao usuário
                            }
                        }
                    }

                }
                if (txtPagadorVazio.Text != "")
                {
                    string descricaoRota = txtMunicipioOrigem.Text.Trim() + "/" + txtUfOrigem.Text.Trim() + " X " + txtMunicipioDestino.Text.Trim() + "/" + txtUfDestino.Text.Trim();

                    DataTable dt = BuscarRota(descricaoRota);

                    if (dt.Rows.Count > 0)
                    {
                        DataRow r = dt.Rows[0];

                        // Preenche a rota encontrada
                        txtRotaVazio.Text = r["rota"].ToString() + " - " + r["desc_rota"].ToString();
                        txtTrajeto.Text = r["deslocamento"].ToString();
                        txtDistancia.Text = r["distancia"].ToString();
                        txtDuracaoVazio.Text = Convert.ToDateTime(r["tempo"]).ToString("HH:mm");
                        txtPedagio.Text = r["pedagio"].ToString();
                        // Guarda dados para salvar depois
                        Session["distancia"] = r["distancia"];
                        Session["deslocamento"] = r["deslocamento"];
                        Session["pedagio"] = r["pedagio"];
                        Session["tempo"] = r["tempo"];
                    }
                    else
                    {
                        txtRotaVazio.Text = "";

                        Session["distancia"] = null;
                        Session["deslocamento"] = null;
                        Session["pedagio"] = null;
                        Session["tempo"] = null;
                    }
                }


            }

        }
        private DataTable BuscarRota(string descricaoRota)
        {
            string strConexao = System.Web.Configuration.WebConfigurationManager
                .ConnectionStrings["conexao"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(strConexao))
            {
                string sql = @"SELECT rota, desc_rota, distancia, deslocamento, pedagio, tempo
               FROM tbrotasdeentregas
               WHERE desc_rota COLLATE Latin1_General_CI_AI LIKE @desc";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@desc", "%" + descricaoRota + "%");

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                return dt;
            }
        }
        private void AtualizarColetasVisiveis()
        {
            DataTable dadosAtuais = Session["Coletas"] as DataTable;

            if (dadosAtuais != null && dadosAtuais.Rows.Count > 0)
            {
                // Obtem os "carga" visíveis
                var cargasVisiveis = dadosAtuais.AsEnumerable()
                    .Select(r => r["carga"].ToString())
                    .Distinct()
                    .ToList();

                // Consulta novamente apenas essas coletas no banco
                var dadosAtualizados = DAL.ConCargas.FetchDataTableColetasPorCargas(cargasVisiveis);

                // Atualiza o Session com os novos dados
                Session["Coletas"] = dadosAtualizados;

                // Atualiza o Repeater
                rptColetas.DataSource = dadosAtualizados;
                rptColetas.DataBind();
            }
        }
        protected void btnFechar_Click(object sender, EventArgs e)
        {
            // Opcional: limpar ou fechar modal
            ClientScript.RegisterStartupScript(this.GetType(), "HideModal", "hideModal();", true);

        }
        protected void btnSalvarColeta_Click(object sender, EventArgs e)
        {
            //AplicarRegras();
            string novaCarga = novaCargaVazia.Text.Trim();
            int numCarga = int.Parse(novaCargaVazia.Text.Trim());
            string pesoTexto = txtPesoVazio.Text.Trim().Replace(".", ",");
            decimal.TryParse(pesoTexto, out decimal pesoMaterial);
            string codigoOrigem = Request.Form[codCliInicial.UniqueID] ?? codCliInicial.Text;
            codigoOrigem = codigoOrigem.Trim();
            string nomeOrigem = ddlCliInicial.SelectedItem.Text.Trim().ToUpper();
            string codigoDestino = codCliFinal.Text.Trim();
            string nomeDestino = ddlCliFinal.SelectedItem.Text.Trim().ToUpper();
            string municipioOrigem = txtMunicipioOrigem.Text.Trim().ToUpper();
            string municipioDestino = txtMunicipioDestino.Text.Trim().ToUpper();
            string ufOrigem = txtUfOrigem.Text.Trim().ToUpper();
            string ufDestino = txtUfDestino.Text.Trim().ToUpper();
            string codigoPagadorVazio = txtCod_PagadorVazio.Text.Trim();
            string nomePagadorVazio = txtPagadorVazio.Text.Trim().ToUpper();
            string materialVazio = ddlTipoMaterial.SelectedItem.Text;
            string municipioPagadorVazio = txtCid_PagadorVazio.Text.Trim().ToUpper();
            string ufPagadorVazio = txtUf_PagadorVazio.Text.Trim().ToUpper();
            string nomeCompleto = nomePagadorVazio;
            string DuracaoVazio = txtDuracaoVazio.Text.Trim();
            string primeiroNome = nomeCompleto.Split(' ')[0];
            string rotaVazio = txtRotaVazio.Text.Trim();
            decimal distancia = Convert.ToDecimal(txtDistancia.Text);
            string pedagio = txtPedagio.Text.Trim();

            string connectionString = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;

            if (codCliInicial.Text != string.Empty || codCliFinal.Text != string.Empty || txtPesoVazio.Text != string.Empty || txtCod_PagadorVazio.Text != string.Empty)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO tbcargas (carga, emissao, status, entrega, peso, material, portao, situacao, previsao, codorigem, cliorigem, coddestino, clidestino, ufcliorigem, ufclidestino, cidorigem, ciddestino, empresa, cadastro,  tomador, andamento, cod_expedidor, expedidor, cid_expedidor, uf_expedidor, cod_recebedor, recebedor, cid_recebedor, uf_recebedor, cod_pagador, pagador, cid_pagador, uf_pagador, duracao, deslocamento, rota_entrega, distancia, emitepedagio)" +
                      "VALUES (@Carga, GETDATE(), @status, @entrega, @peso, @material, @portao, @situacao, @previsao, @codorigem, @cliorigem, @coddestino, @clidestino, @ufcliorigem, @ufclidestino, @cidorigem, @ciddestino, @empresa, @cadastro,  @tomador, @andamento, @cod_expedidor, @expedidor, @cid_expedidor, @uf_expedidor, @cod_recebedor, @recebedor, @cid_recebedor, @uf_recebedor, @cod_pagador, @pagador, @cid_pagador, @uf_pagador, @duracao, @deslocamento, @rota_entrega, @distancia, @emitepedagio)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@carga", numCarga);
                    cmd.Parameters.AddWithValue("@status", "Pendente");
                    cmd.Parameters.AddWithValue("@entrega", "Normal");
                    cmd.Parameters.AddWithValue("@peso", pesoMaterial); // ou valor padrão
                    cmd.Parameters.AddWithValue("@material", materialVazio); // ou valor padrão
                    cmd.Parameters.AddWithValue("@portao", codigoDestino); // ou valor padrão
                    cmd.Parameters.AddWithValue("@situacao", "Pronto");
                    cmd.Parameters.AddWithValue("@tomador", primeiroNome);
                    cmd.Parameters.AddWithValue("@previsao", DateTime.Now.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@codorigem", codigoOrigem);
                    cmd.Parameters.AddWithValue("@cliorigem", nomeOrigem);
                    cmd.Parameters.AddWithValue("@coddestino", codigoDestino);
                    cmd.Parameters.AddWithValue("@clidestino", nomeDestino);
                    cmd.Parameters.AddWithValue("@ufcliorigem", ufOrigem);
                    cmd.Parameters.AddWithValue("@ufclidestino", ufDestino);
                    cmd.Parameters.AddWithValue("@cidorigem", municipioOrigem);
                    cmd.Parameters.AddWithValue("@ciddestino", municipioDestino);
                    cmd.Parameters.AddWithValue("@cod_expedidor", codigoOrigem);
                    cmd.Parameters.AddWithValue("@expedidor", nomeOrigem);
                    cmd.Parameters.AddWithValue("@cod_recebedor", codigoDestino);
                    cmd.Parameters.AddWithValue("@recebedor", nomeDestino);
                    cmd.Parameters.AddWithValue("@uf_expedidor", ufOrigem);
                    cmd.Parameters.AddWithValue("@uf_recebedor", ufDestino);
                    cmd.Parameters.AddWithValue("@cid_expedidor", municipioOrigem);
                    cmd.Parameters.AddWithValue("@cid_recebedor", municipioDestino);
                    cmd.Parameters.AddWithValue("@empresa", "1111"); // ou valor padrão
                    cmd.Parameters.AddWithValue("@cadastro", DateTime.Now.ToString("dd/MM/yyyy HH:mm") + " - " + Session["UsuarioLogado"].ToString());
                    cmd.Parameters.AddWithValue("@andamento", "Pendente");
                    cmd.Parameters.AddWithValue("@cod_pagador", codigoPagadorVazio);
                    cmd.Parameters.AddWithValue("@pagador", nomeCompleto);
                    cmd.Parameters.AddWithValue("@cid_pagador", municipioPagadorVazio);
                    cmd.Parameters.AddWithValue("@uf_pagador", ufPagadorVazio);
                    cmd.Parameters.AddWithValue("@duracao", DuracaoVazio);
                    cmd.Parameters.AddWithValue("@deslocamento", txtTrajeto.Text.Trim());
                    cmd.Parameters.AddWithValue("@rota_entrega", rotaVazio);
                    cmd.Parameters.AddWithValue("@distancia", distancia);
                    cmd.Parameters.AddWithValue("@emitepedagio", pedagio);
                    // Abrindo a conexão e executando a query
                    conn.Open();
                    int rowsInserted = cmd.ExecuteNonQuery();

                    if (rowsInserted > 0)
                    {
                        txtCarga.Text = novaCarga;

                        BuscarCargaNoBanco(novaCarga);

                    }

                    else
                    {
                        string mensagem = "Falha ao cadastrar a viagem. Tente novamente.";
                        string script = $"alert('{HttpUtility.JavaScriptStringEncode(mensagem)}');";
                        ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeErro", script, true);
                    }
                    ScriptManager.RegisterStartupScript(
                            this,
                            this.GetType(),
                            "FechaModal",
                            "$('#meuModal').modal('hide');",
                            true
                        );

                }


            }
            else
            {
                MostrarMsg("Informar campos obrigatórios da carga!");
            }



        }
        private void PreencherClienteInicial()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT id, codcli, razcli FROM tbclientes order by razcli";

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
                    ddlCliInicial.DataTextField = "razcli";  // Campo que será mostrado no ComboBox
                    ddlCliInicial.DataValueField = "codcli";  // Campo que será o valor de cada item                    
                    ddlCliInicial.DataBind();  // Realiza o binding dos dados                   
                    ddlCliInicial.Items.Insert(0, new System.Web.UI.WebControls.ListItem("...", "0"));
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
            string query = "SELECT id, codcli, razcli FROM tbclientes order by razcli";

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
                    ddlCliFinal.DataTextField = "razcli";  // Campo que será mostrado no ComboBox
                    ddlCliFinal.DataValueField = "codcli";  // Campo que será o valor de cada item                    
                    ddlCliFinal.DataBind();  // Realiza o binding dos dados                   
                    ddlCliFinal.Items.Insert(0, new System.Web.UI.WebControls.ListItem("...", "0"));
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
            codCliInicial.Text = ddlCliInicial.SelectedValue;
        }
        protected void ddlCliFinal_TextChanged(object sender, EventArgs e)
        {
            codCliFinal.Text = ddlCliFinal.SelectedValue;
            //string sql = "select Distancia, UF_Origem, Origem, UF_Destino, Destino from tbdistanciapremio where UF_Origem=(SELECT estcli FROM tbclientes where codcli='" + ddlCliInicial.SelectedValue + "') and Origem=(SELECT cidcli FROM tbclientes where codcli='" + ddlCliInicial.SelectedValue + "') and UF_Destino=(SELECT estcli FROM tbclientes where codcli='" + ddlCliFinal.SelectedValue + "') and Destino=(SELECT cidcli FROM tbclientes where codcli='" + ddlCliFinal.SelectedValue + "')";
            //SqlDataAdapter adp = new SqlDataAdapter(sql, con);
            //DataTable dt = new DataTable();
            //con.Open();
            //adp.Fill(dt);
            //con.Close();
            //if (dt.Rows.Count > 0)
            //{
            //    txtDistancia.Text = dt.Rows[0][0].ToString();
            //    txtUfOrigem.Text = dt.Rows[0][1].ToString();
            //    txtMunicipioOrigem.Text = dt.Rows[0][2].ToString();
            //    txtUfDestino.Text = dt.Rows[0][3].ToString();
            //    txtMunicipioDestino.Text = dt.Rows[0][4].ToString();

            //}
            //else
            //{
            //    lblDistancia.Text = "Não há distância cadastrada entre ORIGEM e DESTINO...";
            //}


        }
        protected void codCliInicial_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(codCliInicial.Text))
            {
                string codigoRemetente = codCliInicial.Text.Trim();
                string strConn = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    string query = @"SELECT codcli, razcli, cidcli, estcli 
                         FROM tbclientes 
                         WHERE codcli = @Codigo OR codvw = @Codigo";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Codigo", codigoRemetente);
                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string codCli = reader["codcli"].ToString();

                                //codCliInicial.Text = codCli;

                                // 🔥 AQUI ESTÁ A CORREÇÃO
                                //if (ddlCliInicial.Items.FindByValue(codCli) != null)
                                ddlCliInicial.SelectedValue = codCli;
                                //else
                                //    ddlCliInicial.SelectedIndex = 0;

                                txtMunicipioOrigem.Text = reader["cidcli"] == DBNull.Value ? "" : reader["cidcli"].ToString();
                                txtUfOrigem.Text = reader["estcli"] == DBNull.Value ? "" : reader["estcli"].ToString();

                                codCliFinal.Focus();
                            }
                            else
                            {
                                //ddlCliInicial.ClearSelection();
                                //ddlCliInicial.SelectedIndex = 0;

                                //codCliInicial.Text = "";

                                ScriptManager.RegisterStartupScript(
                                    this,
                                    GetType(),
                                    "toastNaoEncontrado",
                                    "mostrarToastNaoEncontrado();",
                                    true
                                );

                                codCliInicial.Focus();
                            }
                        }
                    }
                }
            }


        }
        protected void codCliFinal_TextChanged(object sender, EventArgs e)
        {

            if (!string.IsNullOrWhiteSpace(codCliFinal.Text))
            {
                string codigoDestinatario = codCliFinal.Text.Trim();
                string strConn = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    string query = @"SELECT codcli, razcli, cidcli, estcli 
                         FROM tbclientes 
                         WHERE codcli = @Codigo OR codvw = @Codigo";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Codigo", codigoDestinatario);
                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string codCli = reader["codcli"].ToString();

                                codCliFinal.Text = codCli;

                                // 🔥 AQUI ESTÁ A CORREÇÃO
                                //if (ddlCliInicial.Items.FindByValue(codCli) != null)
                                ddlCliFinal.SelectedValue = codCli;
                                //else
                                //    ddlCliInicial.SelectedIndex = 0;

                                txtMunicipioDestino.Text = reader["cidcli"] == DBNull.Value ? "" : reader["cidcli"].ToString();
                                txtUfDestino.Text = reader["estcli"] == DBNull.Value ? "" : reader["estcli"].ToString();

                                codCliFinal.Focus();
                            }
                            else
                            {
                                //ddlCliInicial.ClearSelection();
                                //ddlCliInicial.SelectedIndex = 0;

                                //codCliFinal.Text = "";

                                ScriptManager.RegisterStartupScript(
                                    this,
                                    GetType(),
                                    "toastNaoEncontrado",
                                    "mostrarToastNaoEncontrado();",
                                    true
                                );

                                codCliFinal.Focus();
                            }
                        }
                    }
                }
            }


            if (codCliInicial.Text != "" && codCliFinal.Text != "")
            {
                codCliFinal.Text = ddlCliFinal.SelectedValue;
                //string sql = "select Distancia, UF_Origem, Origem, UF_Destino, Destino from tbdistanciapremio where UF_Origem=(SELECT estcli FROM tbclientes where codcli='" + ddlCliInicial.SelectedValue + "') and Origem=(SELECT cidcli FROM tbclientes where codcli='" + ddlCliInicial.SelectedValue + "') and UF_Destino=(SELECT estcli FROM tbclientes where codcli='" + ddlCliFinal.SelectedValue + "') and Destino=(SELECT cidcli FROM tbclientes where codcli='" + ddlCliFinal.SelectedValue + "')";
                //SqlDataAdapter adp = new SqlDataAdapter(sql, con);
                //DataTable dt = new DataTable();
                //con.Open();
                //adp.Fill(dt);
                //con.Close();
                //if (dt.Rows.Count > 0)
                //{
                //    txtDistancia.Text = dt.Rows[0][0].ToString();
                //    txtUfOrigem.Text = dt.Rows[0][1].ToString();
                //    txtMunicipioOrigem.Text = dt.Rows[0][2].ToString();
                //    txtUfDestino.Text = dt.Rows[0][3].ToString();
                //    txtMunicipioDestino.Text = dt.Rows[0][4].ToString();
                //    lblDistancia.Text = string.Empty;
                //}
                //else
                //{
                //    lblDistancia.Text = "Não há distância cadastrada entre ORIGEM e DESTINO...";
                //}



                //string UFOrigem = txtUfOrigem.Text.Trim();
                //string Origem = ddlCliInicial.SelectedItem.Text.Trim();
                //string UFDestino = txtUfDestino.Text.Trim();
                //string Destino = ddlCliFinal.SelectedItem.Text.Trim();

                //string strConn = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
                //using (SqlConnection conn = new SqlConnection(strConn))
                //{
                //    SqlCommand cmd = new SqlCommand(@"
                //        SELECT 
                //        c1.LATITUDE  AS LatOrigem,
                //        c1.LONGITUDE AS LonOrigem,
                //        c2.LATITUDE  AS LatDestino,
                //        c2.LONGITUDE AS LonDestino
                //    FROM tbcidadesdobrasil c1
                //    JOIN tbcidadesdobrasil c2 ON 1 = 1
                //    WHERE 
                //        c1.NOME_MUNICIPIO COLLATE Latin1_General_CI_AI  = @CidadeOrigem AND c1.UF = @UFOrigem
                //    AND c2.NOME_MUNICIPIO COLLATE Latin1_General_CI_AI  = @CidadeDestino AND c2.UF = @UFDestino
                //", conn);

                //    cmd.Parameters.AddWithValue("@CidadeOrigem", txtMunicipioOrigem.Text.Trim());
                //    cmd.Parameters.AddWithValue("@UFOrigem", txtUfOrigem.Text.Trim());
                //    cmd.Parameters.AddWithValue("@CidadeDestino", txtMunicipioDestino.Text.Trim());
                //    cmd.Parameters.AddWithValue("@UFDestino", txtUfDestino.Text.Trim());

                //    conn.Open();
                //    SqlDataReader dr = cmd.ExecuteReader();

                //    if (dr.Read())
                //    {
                //        double distancia = CalcularDistancia(
                //            Convert.ToDouble(dr["LatOrigem"]),
                //            Convert.ToDouble(dr["LonOrigem"]),
                //            Convert.ToDouble(dr["LatDestino"]),
                //            Convert.ToDouble(dr["LonDestino"])
                //        );

                //        txtDistancia.Text = $"{distancia:N2}"; //$"Distância: {distancia:N2} km";
                //    }
                //    else
                //    {
                //        lblDistancia.Text = "Cidade não encontrada.";
                //    }
                //}
            }
        }
        private void PreencherNumCargaVazia()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT (carga + incremento) as ProximaCargaVazia FROM tbcontadores";

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
                                novaCargaVazia.Text = reader["ProximaCargaVazia"].ToString();
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
                            cmd.Parameters.AddWithValue("@carga", novaCargaVazia.Text);
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
        public static double CalcularDistancia(double lat1, double lon1, double lat2, double lon2)
        {
            double R = 6371; // KM
            double dLat = (lat2 - lat1) * Math.PI / 180;
            double dLon = (lon2 - lon1) * Math.PI / 180;

            double a =
                Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(lat1 * Math.PI / 180) *
                Math.Cos(lat2 * Math.PI / 180) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }

        protected string CalcularTempo(object inicio, object fim)
        {
            if (inicio == DBNull.Value || fim == DBNull.Value)
                return "";

            DateTime dtInicio = Convert.ToDateTime(inicio);
            DateTime dtFim = Convert.ToDateTime(fim);

            TimeSpan tempo = dtFim - dtInicio;

            return $"{tempo.Hours:D2}:{tempo.Minutes:D2}";


        }
        void GetMotoristas(DropDownList ddl)
        {
            using (SqlConnection conn = new SqlConnection(
        ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                string sql = "SELECT id, nommot FROM tbmotoristas";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        ddl.DataSource = dr;
                        ddl.DataTextField = "nommot";
                        ddl.DataValueField = "id";
                        ddl.DataBind();
                    }
                }
            }

            //ddl.Items.Insert(0, new ListItem("Selecione", ""));
        }
        private DataTable BuscarTodosMotoristas()
        {
            DataTable dt = new DataTable();
            string sql = "SELECT id, nommot FROM tbmotoristas"; // Ajuste os nomes das colunas
            using (SqlDataAdapter adp = new SqlDataAdapter(sql, con))
            {
                adp.Fill(dt);
            }
            return dt;
        }


        [System.Web.Services.WebMethod]
        public static bool VerificarCVA(string numeroCVA)
        {
            bool existe = false;

            string connectionString = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = "SELECT COUNT(*) FROM tbcargas WHERE cva = @numero";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@numero", numeroCVA);
                    conn.Open();
                    int count = (int)cmd.ExecuteScalar();
                    existe = (count > 0);
                }
            }

            return existe;
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
                    ClientScript.RegisterStartupScript(this.GetType(), "HideModal", "hideModal();", true);
                }
                catch (Exception ex)
                {
                    // Logar ou exibir erro
                    Response.Write("<script>alert('Erro: " + ex.Message + "');</script>");
                }


                //// Abrindo a conexão e executando a query
                //conn.Open();
                //int rowsInserted = cmd.ExecuteNonQuery();

                //if (rowsInserted > 0)
                //{
                //    txtCodFrota.Text = txtCodContato.Text.Trim();
                //    txtFoneCorp.Text = txtCadCelular.Text;

                //}
                //else
                //{
                //    string mensagem = "Falha ao cadastrar telefone do motorista.";
                //    string script = $"alert('{HttpUtility.JavaScriptStringEncode(mensagem)}');";
                //    ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeErro", script, true);
                //}                
            }
        }
        public void CalculaInicioPrestacao(string CidadiInicio, string UfInicio)
        {
            if (HttpContext.Current.Request.QueryString["carregamento"].ToString() != "")
            {
                num_coleta = HttpContext.Current.Request.QueryString["carregamento"].ToString();
            }

            string sqli = "select Top 1 cidorigem, ufcliorigem from tbcargas where idviagem=" + num_coleta + " order by id asc";
            SqlDataAdapter adpi = new SqlDataAdapter(sqli, con);
            DataTable dti = new DataTable();
            con.Open();
            adpi.Fill(dti);
            con.Close();

            if (dti.Rows.Count > 0)
            {
                string sqld = "select  Distancia from tbdistanciapremio where Origem='" + CidadiInicio + "' and UF_Origem='" + UfInicio + "' and Destino='" + dti.Rows[0][0].ToString() + "' and UF_Destino='" + dti.Rows[0][1].ToString() + "'";
                SqlDataAdapter adpd = new SqlDataAdapter(sqld, con);
                DataTable dtd = new DataTable();
                con.Open();
                adpd.Fill(dtd);
                con.Close();

                if (dtd.Rows.Count > 0)
                {
                    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["conexao"].ToString()))
                    {
                        string query = "update tbcarregamentos set km_inicial=@km_inicial where num_carregamento=@num_carregamento";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@num_carregamento", num_coleta);
                        cmd.Parameters.AddWithValue("@km_inicial", decimal.Parse(dtd.Rows[0][0].ToString()));


                        try
                        {
                            conn.Open();
                            cmd.ExecuteNonQuery();
                            conn.Close();
                        }
                        catch (Exception ex)
                        {
                            // Logar ou exibir erro
                            Response.Write("<script>alert('Erro: " + ex.Message + "');</script>");
                        }
                        // Opcional: limpar ou fechar modal

                    }
                }
                else
                {
                    string mensagem = "Nao foi encontrado a Distancia do inicio de de Prestação.";
                    string script = $"alert('{HttpUtility.JavaScriptStringEncode(mensagem)}');";
                    ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeErro", script, true);
                }
            }


        }
        public void CalculaFimPrestacao(string CidadiFim, string UfFim)
        {
            if (HttpContext.Current.Request.QueryString["carregamento"].ToString() != "")
            {
                num_coleta = HttpContext.Current.Request.QueryString["carregamento"].ToString();
            }

            string sqlf = "select Top 1 cidorigem, ufcliorigem from tbcargas where idviagem=" + num_coleta + " order by id desc";
            SqlDataAdapter adpf = new SqlDataAdapter(sqlf, con);
            DataTable dtf = new DataTable();
            con.Open();
            adpf.Fill(dtf);
            con.Close();

            if (dtf.Rows.Count > 0)
            {
                string sqld = "select  Distancia from tbdistanciapremio where Origem='" + dtf.Rows[0][0].ToString() + "' and UF_Origem='" + dtf.Rows[0][1].ToString() + "' and Destino='" + CidadiFim + "' and UF_Destino='" + UfFim + "'";
                SqlDataAdapter adpd = new SqlDataAdapter(sqld, con);
                DataTable dtd = new DataTable();
                con.Open();
                adpd.Fill(dtd);
                con.Close();

                if (dtd.Rows.Count > 0)
                {
                    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["conexao"].ToString()))
                    {
                        string query = "update tbcarregamentos set km_final=@km_final where num_carregamento=@num_carregamento";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@num_carregamento", num_coleta);
                        cmd.Parameters.AddWithValue("@km_final", decimal.Parse(dtd.Rows[0][0].ToString()));


                        try
                        {
                            conn.Open();
                            cmd.ExecuteNonQuery();
                            conn.Close();
                        }
                        catch (Exception ex)
                        {
                            // Logar ou exibir erro
                            Response.Write("<script>alert('Erro: " + ex.Message + "');</script>");
                        }
                        // Opcional: limpar ou fechar modal

                    }
                }
                else
                {
                    string mensagem = "Nao foi encontrado a Distancia do fim de de Prestação.";
                    string script = $"alert('{HttpUtility.JavaScriptStringEncode(mensagem)}');";
                    ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeErro", script, true);
                }
            }


        }
        public void CalculaPremio(decimal kmtotalcoleta)
        {
            if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["carregamento"]))
                num_coleta = HttpContext.Current.Request.QueryString["carregamento"].ToString();

            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                string sql = @"SELECT distancia1, distancia2, motorista, carreteiro, bitrem, desengate 
                       FROM tbvalorpremiomotoristas 
                       WHERE distancia1 <= ROUND(@distancia, 0) 
                         AND distancia2 >= ROUND(@distancia, 0) and empresa='MATRIZ'";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@distancia", kmtotalcoleta);
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dt);
                }
            }

            if (dt.Rows.Count == 0)
            {
                string script = "alert('Nenhum registro encontrado para a distância informada.');";
                ClientScript.RegisterStartupScript(this.GetType(), "Erro", script, true);
                return;
            }

            decimal premio = 0, desengate = 0;

            string funcao = txtFuncao.Text.Substring(0, 1);

            if (funcao == "M")
                premio = decimal.Parse(dt.Rows[0]["motorista"].ToString(), CultureInfo.InvariantCulture);
            else if (funcao == "C")
                premio = decimal.Parse(dt.Rows[0]["carreteiro"].ToString(), CultureInfo.InvariantCulture);
            else if (funcao == "B")
            {
                premio = decimal.Parse(dt.Rows[0]["bitrem"].ToString(), CultureInfo.InvariantCulture);
                desengate = decimal.Parse(dt.Rows[0]["desengate"].ToString(), CultureInfo.InvariantCulture);
            }

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                string query = funcao == "B"
                    ? "UPDATE tbcarregamentos SET premio=@premio, desengate=@desengate WHERE num_carregamento=@num_carregamento"
                    : "UPDATE tbcarregamentos SET premio=@premio WHERE num_carregamento=@num_carregamento";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@num_carregamento", num_coleta);
                    cmd.Parameters.AddWithValue("@premio", premio);
                    if (funcao == "B") cmd.Parameters.AddWithValue("@desengate", desengate);

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        string mensagem = funcao == "B"
                            ? "Prêmio e desengate cadastrados com sucesso."
                            : "Prêmio cadastrado com sucesso.";
                        string script = $"alert('{HttpUtility.JavaScriptStringEncode(mensagem)}');";
                        ClientScript.RegisterStartupScript(this.GetType(), "Sucesso", script, true);
                    }
                    catch (Exception ex)
                    {
                        Response.Write("<script>alert('Erro: " + ex.Message + "');</script>");
                    }
                }
            }
        }
        public void InserePremioMotorista()
        {

        }
        protected void btnEncerrar_Click(object sender, EventArgs e)
        {
            // Atualizando a ordem de coleta 
            string mensagemErro = ValidarEncerramento();

            if (string.IsNullOrEmpty(mensagemErro))
            {
                using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
                {
                    string queryCarregamento = @"UPDATE tbcarregamentos SET 
                                situacao = @situacao,
                                status = @status
                                WHERE num_carregamento = @num_carregamento";

                    SqlCommand cmdCarregamento = new SqlCommand(queryCarregamento, conn);
                    cmdCarregamento.Parameters.AddWithValue("@num_carregamento", novaColeta.Text);
                    cmdCarregamento.Parameters.AddWithValue("@situacao", "VIAGEM CONCLUIDA");
                    cmdCarregamento.Parameters.AddWithValue("@status", "Concluido");

                    // Chama método que verifica no banco
                    conn.Open();
                    cmdCarregamento.ExecuteNonQuery();

                    Response.Redirect("/dist/pages/GestaoDeEntregasMatriz.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                }
            }
            else
            {
                MostrarMsg(mensagemErro);
            }

        }
        protected void txtCodMotorista_TextChanged(object sender, EventArgs e)
        {
            divMsg.Visible = true;
            divMsgCNH.Visible = true;
            divMsgCarreta1.Visible = true;
            divMsgCarreta2.Visible = true;
            divMsgCET.Visible = true;
            divMsgCrono.Visible = true;
            divMsgGR.Visible = true;
            divMsgLinc.Visible = true;
            divMsgVeic.Visible = true;

            if (string.IsNullOrEmpty(txtCodMotorista.Text))
            {
                MostrarMsg("Digite o código do motorista!", "danger");
                return;
            }

            string sql = @"SELECT codmot, nommot, status, cargo, nucleo, cpf, venccnh, codliberacao, validade, venceti, cartaomot, tipomot, venccartao, ISNULL(caminhofoto, '/fotos/motoristasemfoto.jpg') AS caminhofoto,fone2, codtra, transp, frota, status, inativo 
                   FROM tbmotoristas 
                   WHERE codmot = @id";

            //using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@id", txtCodMotorista.Text);

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    MostrarMsg("Motorista: " + txtCodMotorista.Text.Trim() + ", não cadastrado no sistema. Verifique!", "danger");
                    fotoMotorista = "/fotos/motoristasemfoto.jpg";
                    txtCodMotorista.Text = "";
                    txtCodMotorista.Focus();
                    return;
                }

                txtCodMotorista.Text = dt.Rows[0]["codmot"].ToString();
                ddlMotorista.SelectedItem.Text = dt.Rows[0]["nommot"].ToString();
                txtFilialMot.Text = dt.Rows[0]["nucleo"].ToString();
                txtTipoMot.Text = dt.Rows[0]["tipomot"].ToString();
                txtFuncao.Text = dt.Rows[0]["cargo"].ToString();
                txtExameToxic.Text = Convert.ToDateTime(dt.Rows[0]["venceti"]).ToString("dd/MM/yyyy");
                txtCNH.Text = Convert.ToDateTime(dt.Rows[0]["venccnh"]).ToString("dd/MM/yyyy");
                txtLibGR.Text = Convert.ToDateTime(dt.Rows[0]["validade"]).ToString("dd/MM/yyyy");
                txtCelular.Text = dt.Rows[0]["fone2"].ToString();
                txtCPF.Text = dt.Rows[0]["cpf"].ToString();
                txtCartao.Text = dt.Rows[0]["cartaomot"].ToString();
                txtValCartao.Text = dt.Rows[0]["venccartao"].ToString();
                txtCodTransportadora.Text = dt.Rows[0]["codtra"].ToString();
                txtTransportadora.Text = dt.Rows[0]["transp"].ToString();
                fotoMotorista = dt.Rows[0]["caminhofoto"].ToString();
                txtLiberacao.Text = dt.Rows[0]["codliberacao"].ToString();
                if (dt.Rows[0]["status"].ToString() == "INATIVO")
                {
                    MostrarMsg("- " + dt.Rows[0]["codmot"].ToString() + " - " + dt.Rows[0]["nommot"].ToString() + ", << INATIVO >> MOTIVO: " + dt.Rows[0]["inativo"].ToString(), "danger");
                    txtCodMotorista.Text = "";
                    txtCodMotorista.Focus();
                    return;
                }
                else if (dt.Rows[0]["status"].ToString() == "ATIVO")
                {
                    // valida Exame Toxicologico
                    if (dt.Rows[0]["tipomot"].ToString() == "FUNCIONÁRIO")
                {
                    DateTime dataETI;
                    if (!DateTime.TryParse(txtExameToxic.Text, out dataETI))
                    {
                        MostrarMsg("Exame Toxicologico do " + ddlMotorista.SelectedItem.Text.Trim() + ", não foi lançado. Verifique", "danger");

                        txtCodMotorista.Text = "";
                        txtCodMotorista.Focus();

                    }
                    else
                    {
                        DateTime validadeET = Convert.ToDateTime(dt.Rows[0]["venceti"]);
                        TimeSpan diferencaET = validadeET - DateTime.Today;

                        if (validadeET < DateTime.Today)
                        {
                            MostrarMsg("Exame Toxicologico do " + ddlMotorista.SelectedItem.Text.Trim() + ", está VENCIDO. Verifique!", "danger");
                            txtExameToxic.BackColor = System.Drawing.Color.Red;
                            txtExameToxic.ForeColor = System.Drawing.Color.White;
                            txtCodMotorista.Text = "";
                            txtCodMotorista.Focus();
                        }
                        else if (diferencaET.TotalDays <= 30)
                        {
                            MostrarMsg("Atenção! Exame Toxicologico do " + ddlMotorista.SelectedItem.Text.Trim() + ", vence em " + diferencaET.Days + " dias.", "warning");
                            txtExameToxic.BackColor = System.Drawing.Color.Khaki;
                            txtExameToxic.ForeColor = System.Drawing.Color.OrangeRed;
                        }
                    }

                }
                    // valida CNH
                    DateTime dataCNH;
                    if (!DateTime.TryParse(txtCNH.Text, out dataCNH))
                    {
                        MostrarMsgCNH("Validade da CNH do " + ddlMotorista.SelectedItem.Text.Trim() + ", não foi lançada. Verifique!", "danger");
                        txtCodMotorista.Text = "";
                        txtCodMotorista.Focus();
                    }
                    else
                    {
                        // valida cnh
                        DateTime validadeCNH = Convert.ToDateTime(dt.Rows[0]["venccnh"]);
                        TimeSpan diferencaCNH = validadeCNH - DateTime.Today;

                        if (validadeCNH < DateTime.Today)
                        {
                            MostrarMsgCNH("CNH do motorista " + ddlMotorista.SelectedItem.Text.Trim() + ", está VENCIDA. Verifique!", "danger");
                            txtCNH.BackColor = System.Drawing.Color.Red;
                            txtCNH.ForeColor = System.Drawing.Color.White;
                            txtCodMotorista.Text = "";
                            txtCodMotorista.Focus();
                        }
                        else if (diferencaCNH.TotalDays <= 30)
                        {
                            MostrarMsgCNH("Atenção! A CNH do " + ddlMotorista.SelectedItem.Text.Trim() + ", vence em " + diferencaCNH.Days + " dias.", "warning");
                            txtCNH.BackColor = System.Drawing.Color.Khaki;
                            txtCNH.ForeColor = System.Drawing.Color.OrangeRed;
                        }
                    }
                    // valida GR
                    DateTime dataGR;
                    if (!DateTime.TryParse(txtLibGR.Text, out dataGR))
                    {
                        MostrarMsgGR("Validade da Liberação de Risco do " + ddlMotorista.SelectedItem.Text.Trim() + ", não foi lançada. Verifique!", "danger");
                        txtCodMotorista.Text = "";
                        txtCodMotorista.Focus();
                    }
                    else
                    {
                        // valida liberação de risco 
                        DateTime validadeGR = Convert.ToDateTime(dt.Rows[0]["validade"]);
                        TimeSpan diferencaGR = validadeGR - DateTime.Today;

                        if (validadeGR < DateTime.Today)
                        {
                            MostrarMsgGR("Liberação de Risco do " + ddlMotorista.SelectedItem.Text.Trim() + ", está VENCIDA. Verifique!.", "danger");
                            txtLibGR.BackColor = System.Drawing.Color.Red;
                            txtLibGR.ForeColor = System.Drawing.Color.White;
                            txtCodMotorista.Text = "";
                            txtCodMotorista.Focus();
                        }
                        else if (diferencaGR.TotalDays <= 30)
                        {
                            MostrarMsgGR("Atenção! Liberação de Risco do motorista " + ddlMotorista.SelectedItem.Text.Trim() + ", vence em " + diferencaGR.Days + " dias.", "warning");
                            txtLibGR.BackColor = System.Drawing.Color.Khaki;
                            txtLibGR.ForeColor = System.Drawing.Color.OrangeRed;
                        }
                    }

                    txtCodVeiculo.Text = dt.Rows[0]["frota"].ToString();
                }
            }

            //if(txtTipoMot.Text != "FUNCIONÁRIO")
            //{
            //Dados do veiculos
            string sqlVeiculos = @"SELECT codvei, plavei, reboque1, reboque2, tipoveiculo, tipvei, tiporeboque, tipocarreta, vencimentolaudofumaca, venclicenciamento, codtra, transp, venclicencacet, protocolocet, venccronotacografo, rastreamento, rastreador, ativo_inativo, fl_exclusao
                       FROM tbveiculos 
                       WHERE codvei = @idVeiculo AND ativo_inativo = 'ATIVO' AND fl_exclusao IS NULL";
            using (SqlCommand cmdVeiculos = new SqlCommand(sqlVeiculos, con))
            {
                cmdVeiculos.Parameters.AddWithValue("@idVeiculo", txtCodVeiculo.Text.Trim());

                DataTable dtVeiculos = new DataTable();
                SqlDataAdapter daVeiculos = new SqlDataAdapter(cmdVeiculos);

                daVeiculos.Fill(dtVeiculos);

                if (dtVeiculos.Rows.Count == 0)
                {
                    MostrarMsgVeic(txtPlaca.Text.Trim() + " - Veículo NÂO encontrado na base de dados ou INATIVO. Verifique!", "danger");
                    //LimparCamposMotorista();
                    return;
                }

                if (dtVeiculos.Rows[0]["ativo_inativo"].ToString() == "INATIVO" || dtVeiculos.Rows[0]["fl_exclusao"].ToString() == "S")
                {
                    MostrarMsgVeic(txtPlaca.Text.Trim() + " - Veículo INATIVO ou EXCLUIDO da base de dados. Verique!", "danger");
                    //LimparCamposMotorista();
                    return;

                }

                if (dtVeiculos.Rows[0]["tipvei"].ToString() == "CAVALO TRUCADO")
                {
                    carretas.Visible = true;
                    reboque1.Visible = true;
                }
                else if (dtVeiculos.Rows[0]["tipvei"].ToString() == "CAVALO SIMPLES")
                {
                    carretas.Visible = true;
                    reboque1.Visible = true;
                }
                else if (dtVeiculos.Rows[0]["tipvei"].ToString() == "CAVALO 4 EIXOS")
                {
                    carretas.Visible = true;
                    reboque1.Visible = true;
                }
                else if (dtVeiculos.Rows[0]["tipvei"].ToString() == "BITREM")
                {
                    carretas.Visible = true;
                    reboque1.Visible = true;
                    reboque2.Visible = true;
                }
                else
                {
                    carretas.Visible = false;
                    reboque1.Visible = false;
                    reboque2.Visible = false;
                }


                // Dados do veículo
                txtCodVeiculo.Text = dtVeiculos.Rows[0]["codvei"].ToString();
                txtPlaca.Text = dtVeiculos.Rows[0]["plavei"].ToString();
                txtReboque1.Text = dtVeiculos.Rows[0]["reboque1"].ToString();
                txtReboque2.Text = dtVeiculos.Rows[0]["reboque2"].ToString();

                txtVeiculoTipo.Text = dtVeiculos.Rows[0]["tipoveiculo"].ToString();
                txtTipoVeiculo.Text = dtVeiculos.Rows[0]["tipvei"].ToString().Trim();
                txtCarreta.Text = dtVeiculos.Rows[0]["tiporeboque"].ToString();
                txtConjunto.Text = dtVeiculos.Rows[0]["tipocarreta"].ToString();
                txtCodProprietario.Text = dtVeiculos.Rows[0]["codtra"].ToString();
                txtProprietario.Text = dtVeiculos.Rows[0]["transp"].ToString();
                txtTecnologia.Text = dtVeiculos.Rows[0]["rastreamento"].ToString();
                txtRastreamento.Text = dtVeiculos.Rows[0]["rastreador"].ToString();

                txtOpacidade.Text = dtVeiculos.Rows[0]["vencimentolaudofumaca"].ToString();
                txtCET.Text = dtVeiculos.Rows[0]["venclicencacet"].ToString();
                txtProtocoloCET.Text = dtVeiculos.Rows[0]["protocolocet"].ToString();
                txtCRLVVeiculo.Text = dtVeiculos.Rows[0]["venclicenciamento"].ToString();
                txtCrono.Text = dtVeiculos.Rows[0]["venccronotacografo"].ToString();



                // valida Laudo de Fumaça
                DateTime dataOpacidade;
                if (!DateTime.TryParse(txtOpacidade.Text, out dataOpacidade))
                {
                    MostrarMsgVeic(txtPlaca.Text.Trim() + " - Laudo de OPACIDADE(Fumaça), não foi lançado.", "danger");
                    txtCodMotorista.Text = "";
                    txtCodMotorista.Focus();
                }
                else
                {
                    DateTime validadeOpacidade = Convert.ToDateTime(dtVeiculos.Rows[0]["vencimentolaudofumaca"]);
                    TimeSpan diferencaOpacidade = validadeOpacidade - DateTime.Today;

                    if (validadeOpacidade < DateTime.Today)
                    {
                        MostrarMsgVeic(txtPlaca.Text.Trim() + " - Laudo de OPACIDADE(Fumaça), está VENCIDO.", "danger");
                        txtOpacidade.BackColor = System.Drawing.Color.Red;
                        txtOpacidade.ForeColor = System.Drawing.Color.White;
                        txtCodMotorista.Text = "";
                        txtCodMotorista.Focus();
                    }
                    else if (diferencaOpacidade.TotalDays <= 30)
                    {
                        MostrarMsgVeic(txtPlaca.Text.Trim() + " - Atenção! Laudo de OPACIDADE(Fumaça) vence em " + diferencaOpacidade.Days + " dias.", "warning");
                        txtOpacidade.BackColor = System.Drawing.Color.Khaki;
                        txtOpacidade.ForeColor = System.Drawing.Color.OrangeRed;
                    }
                }

                // valida Cronotacografo
                DateTime dataCrono;
                if (!DateTime.TryParse(txtCrono.Text, out dataCrono))
                {
                    MostrarMsgCrono(txtPlaca.Text.Trim() + " - Laudo do CRONOTACOGRAFO, não foi lançado. Verifique!", "danger");
                    txtCodMotorista.Text = "";
                    txtCodMotorista.Focus();
                }
                else
                {
                    DateTime validadeCrono = Convert.ToDateTime(dtVeiculos.Rows[0]["venccronotacografo"]);
                    TimeSpan diferencaCrono = validadeCrono - DateTime.Today;

                    if (validadeCrono < DateTime.Today)
                    {
                        MostrarMsgCrono(txtPlaca.Text.Trim() + " - Laudo do CRONOTACOGRAFO, está VENCIDO. Verifique!", "danger");
                        txtCrono.BackColor = System.Drawing.Color.Red;
                        txtCrono.ForeColor = System.Drawing.Color.White;
                        txtCodMotorista.Text = "";
                        txtCodMotorista.Focus();
                    }
                    else if (diferencaCrono.TotalDays <= 30)
                    {
                        MostrarMsgCrono(txtPlaca.Text.Trim() + " - Atenção! Laudo do CRONOTACOGRAFO vence em " + diferencaCrono.Days + " dias.", "warning");
                        txtCrono.BackColor = System.Drawing.Color.Khaki;
                        txtCrono.ForeColor = System.Drawing.Color.OrangeRed;
                    }
                }

                // valida Licenciamento
                DateTime dataLinc;
                if (!DateTime.TryParse(txtCRLVVeiculo.Text, out dataLinc))
                {
                    MostrarMsgLinc(txtPlaca.Text.Trim() + " - LICENCIAMENTO do veículo, não foi lançado. Verifique!", "danger");
                    txtCodMotorista.Text = "";
                    txtCodMotorista.Focus();
                }
                else
                {
                    DateTime validadeLinc = Convert.ToDateTime(dtVeiculos.Rows[0]["vencimentolaudofumaca"]);
                    TimeSpan diferencaLinc = validadeLinc - DateTime.Today;

                    if (validadeLinc < DateTime.Today)
                    {
                        MostrarMsgLinc(txtPlaca.Text.Trim() + " - LICENCIAMENTO do veículo, está VENCIDO. Verifique!", "danger");
                        txtCRLVVeiculo.BackColor = System.Drawing.Color.Red;
                        txtCRLVVeiculo.ForeColor = System.Drawing.Color.White;
                        txtCodMotorista.Text = "";
                        txtCodMotorista.Focus();
                    }
                    else if (diferencaLinc.TotalDays <= 30)
                    {
                        MostrarMsgLinc(txtPlaca.Text.Trim() + " - Atenção! LICENCIAMENTO do veículo, vence em " + diferencaLinc.Days + " dias.", "warning");
                        txtCRLVVeiculo.BackColor = System.Drawing.Color.Khaki;
                        txtCRLVVeiculo.ForeColor = System.Drawing.Color.OrangeRed;
                    }
                }

                // valida CET
                DateTime dataCET;
                if (!DateTime.TryParse(txtCET.Text, out dataCET))
                {
                    MostrarMsgCET(txtPlaca.Text.Trim() + " - LICENÇA CET para o veículo, não foi lançado. Verifique!", "danger");
                    txtCodMotorista.Text = "";
                    txtCodMotorista.Focus();
                }
                else
                {
                    DateTime validadeCET = Convert.ToDateTime(dtVeiculos.Rows[0]["venclicencacet"]);
                    TimeSpan diferencaCET = validadeCET - DateTime.Today;

                    if (validadeCET < DateTime.Today)
                    {
                        MostrarMsgCET(txtPlaca.Text.Trim() + " - LICENÇA CET do veículo, está VENCIDA. Verifique!", "danger");
                        txtCET.BackColor = System.Drawing.Color.Red;
                        txtCET.ForeColor = System.Drawing.Color.White;
                        txtCodMotorista.Text = "";
                        txtCodMotorista.Focus();
                    }
                    else if (diferencaCET.TotalDays <= 30)
                    {
                        MostrarMsgCET(txtPlaca.Text.Trim() + " - Atenção! LICENÇA CET do veículo, vence em " + diferencaCET.Days + " dias.", "warning");
                        txtCET.BackColor = System.Drawing.Color.Khaki;
                        txtCET.ForeColor = System.Drawing.Color.OrangeRed;
                    }
                }



            }

            // Dados da primeira carreta
            if (txtReboque1.Text != "")
            {
                string sqlCarreta1 = @"SELECT placacarreta, licenciamento, ativo_inativo, fl_exclusao
                       FROM tbcarretas 
                       WHERE placacarreta = @idCarreta1 AND ativo_inativo = 'ATIVO' AND fl_exclusao IS NULL";
                using (SqlCommand cmdCarreta1 = new SqlCommand(sqlCarreta1, con))
                {
                    cmdCarreta1.Parameters.AddWithValue("@idCarreta1", txtReboque1.Text.Trim());

                    DataTable dtCarreta1 = new DataTable();
                    SqlDataAdapter daCarreta1 = new SqlDataAdapter(cmdCarreta1);

                    daCarreta1.Fill(dtCarreta1);

                    if (dtCarreta1.Rows.Count == 0)
                    {
                        MostrarMsgCarreta1(txtReboque1.Text.Trim() + " - Carreta NÂO encontrada na base de dados ou INATIVA. Verifique!", "danger");
                        //LimparCamposMotorista();
                        return;
                    }

                    if (dtCarreta1.Rows[0]["ativo_inativo"].ToString() == "INATIVO" || dtCarreta1.Rows[0]["fl_exclusao"].ToString() == "S")
                    {
                        MostrarMsgCarreta1(txtReboque1.Text.Trim() + " - Carreta INATIVA ou EXCLUIDA da base de dados. Verique!", "danger");
                        //LimparCamposMotorista();
                        return;

                    }

                    // Dados da Carreta 
                    txtCRLVReb1.Text = dtCarreta1.Rows[0]["licenciamento"].ToString();
                    // valida Licenciamento reboque 1
                    DateTime dataLincReb1;
                    if (!DateTime.TryParse(txtCRLVReb1.Text, out dataLincReb1))
                    {
                        MostrarMsgCarreta1(txtReboque1.Text.Trim() + " - LICENCIAMENTO da carreta, não foi lançado. Verifique!", "danger");
                        //return;
                    }
                    else
                    {
                        DateTime validadeLincReb1 = Convert.ToDateTime(dtCarreta1.Rows[0]["licenciamento"]);
                        TimeSpan diferencaLincReb1 = validadeLincReb1 - DateTime.Today;

                        if (validadeLincReb1 < DateTime.Today)
                        {
                            MostrarMsgCarreta1(txtReboque1.Text.Trim() + " - LICENCIAMENTO da carreta, está VENCIDO. Verifique!", "danger");
                            txtCRLVReb1.BackColor = System.Drawing.Color.Red;
                            txtCRLVReb1.ForeColor = System.Drawing.Color.White;
                        }
                        else if (diferencaLincReb1.TotalDays <= 30)
                        {
                            MostrarMsgCarreta1(txtReboque1.Text.Trim() + " - Atenção! LICENCIAMENTO da carreta, vence em " + diferencaLincReb1.Days + " dias.", "warning");
                            txtCRLVReb1.BackColor = System.Drawing.Color.Khaki;
                            txtCRLVReb1.ForeColor = System.Drawing.Color.OrangeRed;
                        }
                    }
                }

            }


            // Dados da segunda carreta
            if (txtReboque2.Text != "")
            {
                string sqlCarreta2 = @"SELECT placacarreta, licenciamento, ativo_inativo, fl_exclusao
                   FROM tbcarretas 
                   WHERE placacarreta = @idCarreta2 AND ativo_inativo = 'ATIVO' AND fl_exclusao IS NULL";
                using (SqlCommand cmdCarreta2 = new SqlCommand(sqlCarreta2, con))
                {
                    cmdCarreta2.Parameters.AddWithValue("@idCarreta2", txtReboque2.Text.Trim());

                    DataTable dtCarreta2 = new DataTable();
                    SqlDataAdapter daCarreta2 = new SqlDataAdapter(cmdCarreta2);

                    daCarreta2.Fill(dtCarreta2);

                    if (dtCarreta2.Rows.Count == 0)
                    {
                        MostrarMsgCarreta2(txtReboque2.Text.Trim() + " - Carreta NÂO encontrada na base de dados ou INATIVA. Verifique!", "danger");
                        //LimparCamposMotorista();
                        return;
                    }

                    if (dtCarreta2.Rows[0]["ativo_inativo"].ToString() == "INATIVO" || dtCarreta2.Rows[0]["fl_exclusao"].ToString() == "S")
                    {
                        MostrarMsgCarreta2(txtReboque2.Text.Trim() + " - Carreta INATIVA ou EXCLUIDA da base de dados. Verique!", "danger");
                        //LimparCamposMotorista();
                        return;

                    }

                    // Dados da Carreta 2
                    txtCRLVReb2.Text = dtCarreta2.Rows[0]["licenciamento"].ToString();
                    // valida Licenciamento reboque 1
                    DateTime dataLincReb2;
                    if (!DateTime.TryParse(txtCRLVReb2.Text, out dataLincReb2))
                    {
                        MostrarMsgCarreta2(txtReboque2.Text.Trim() + " - LICENCIAMENTO da carreta, não foi lançado. Verifique!", "danger");
                        txtCodMotorista.Text = "";
                        txtCodMotorista.Focus();
                    }
                    else
                    {
                        DateTime validadeLincReb2 = Convert.ToDateTime(dtCarreta2.Rows[0]["licenciamento"]);
                        TimeSpan diferencaLincReb2 = validadeLincReb2 - DateTime.Today;

                        if (validadeLincReb2 < DateTime.Today)
                        {
                            MostrarMsgCarreta2(txtReboque2.Text.Trim() + " - LICENCIAMENTO da carreta, está VENCIDO. Verifique!", "danger");
                            txtCRLVReb2.BackColor = System.Drawing.Color.Red;
                            txtCRLVReb2.ForeColor = System.Drawing.Color.White;
                            txtCodMotorista.Text = "";
                            txtCodMotorista.Focus();
                        }
                        else if (diferencaLincReb2.TotalDays <= 30)
                        {
                            MostrarMsgCarreta2(txtReboque2.Text.Trim() + " - Atenção! LICENCIAMENTO da carreta, vence em " + diferencaLincReb2.Days + " dias.", "warning");
                            txtCRLVReb2.BackColor = System.Drawing.Color.Khaki;
                            txtCRLVReb2.ForeColor = System.Drawing.Color.OrangeRed;
                        }
                    }
                }

            }
            //}


        }
        protected void ddlRotaKrona_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            // Linha do Repeater onde o evento ocorreu
            RepeaterItem item = (RepeaterItem)ddl.NamingContainer;

            // TextBox da MESMA linha
            TextBox txtId_Rota = (TextBox)item.FindControl("txtId_Rota");

            if (txtId_Rota == null)
                return;

            if (!string.IsNullOrEmpty(ddl.SelectedValue))
            {
                txtId_Rota.Text = ddl.SelectedValue;
            }
            else
            {
                txtId_Rota.Text = "";
            }
        }
        private DataTable BuscarCteSalvos(string idViagem)
        {
            DataTable dt = new DataTable();
            // Usamos ALIAS (AS) para garantir que o nome da coluna no C# seja amigável
            string sql = @"SELECT uf_emissor, empresa_emissora, num_documento, 
                          serie_documento, chave_de_acesso, mes_ano_documento
                   FROM tbcte 
                   WHERE id_viagem = @idViagem";

            try
            {
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@idViagem", idViagem.Trim());
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Erro SQL: " + ex.Message);
            }
            return dt;
        }
        private void CarregarGridCte(GridView gv, string idViagem, int index)
        {
            try
            {
                // 1. Busca o que já está salvo no Banco de Dados
                DataTable dtFinal = BuscarCteSalvos(idViagem);

                // Garantimos que a coluna Status exista no DataTable para o GridView não dar erro
                if (!dtFinal.Columns.Contains("Status"))
                {
                    dtFinal.Columns.Add("Status", typeof(string));
                }

                // Marcamos os itens vindos do banco como "Gravado"
                foreach (DataRow r in dtFinal.Rows)
                {
                    r["Status"] = "Gravado";
                }

                // 2. Mesclamos com o que está na Session (Leituras que ainda não foram para o banco)
                if (ListaCtePorItem.ContainsKey(index))
                {
                    foreach (var cte in ListaCtePorItem[index])
                    {
                        DataRow dr = dtFinal.NewRow();
                        // ATENÇÃO: Os nomes abaixo devem ser IGUAIS aos do seu SELECT em BuscarCteSalvos
                        dr["uf_emissor"] = cte.Estado;
                        dr["empresa_emissora"] = cte.Filial;
                        dr["num_documento"] = cte.Numero;
                        dr["serie_documento"] = cte.Serie;
                        dr["Status"] = "Lido (Pendente)";
                        dtFinal.Rows.Add(dr);
                    }
                }

                // 3. Vincula o resultado final ao GridView
                gv.DataSource = dtFinal;
                gv.DataBind();
            }
            catch (Exception ex)
            {
                MostrarMsg("Erro ao carregar Grid: " + ex.Message);
            }
        }
        private string RemoverEspacos(string texto)
        {   // remover espaços em branco entre os números digitados 
            if (string.IsNullOrWhiteSpace(texto))
                return string.Empty;

            return new string(texto
                .Where(c => !char.IsWhiteSpace(c))
                .ToArray());
        }
        protected void txtChaveCte_TextChanged(object sender, EventArgs e)
        {
            //35251055890016000109570010001725711001725910
            TextBox txt = (TextBox)sender;
            string chave = txt.Text.Trim();
            chave = RemoverEspacos(txt.Text);

            if (chave.Length != 44) return;

            try
            {
                RepeaterItem item = (RepeaterItem)txt.NamingContainer;
                int index = item.ItemIndex;

                HiddenField lblId = (HiddenField)item.FindControl("hdflIdviagem");
                string idViagem = lblId != null ? lblId.Value : "";

                // --- 1. VALIDAÇÃO DE DUPLICIDADE ---

                // Verifica na Session (ListaCtePorItem)
                bool jaLidoSessao = ListaCtePorItem.ContainsKey(index) &&
                                    ListaCtePorItem[index].Any(x => x.ChaveOriginal == chave);

                // Verifica no Banco de Dados (tbcte)
                bool jaSalvoBanco = CteJaExisteNoBanco(chave);

                if (jaLidoSessao || jaSalvoBanco)
                {
                    string msg = jaLidoSessao ? "Este CT-e já foi lido agora." : "Este CT-e já está salvo no banco de dados.";
                    MostrarMsg("Aviso: " + msg);
                    txt.Text = string.Empty;
                    txt.Focus();
                    return;
                }

                // --- 2. BUSCA DE DADOS DO EMISSOR ---
                string CNPJ = chave.Substring(6, 14);
                string sql = @"SELECT fantra, (select Estado from tbestadosbrasileiros where SiglaUf=uftra) as uftra, cidtra 
                       FROM tbtransportadoras 
                       WHERE REPLACE(REPLACE(REPLACE(cnpj, '.', ''), '/', ''), '-', '') = @cnpj";

                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@cnpj", CNPJ);
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dt);
                }

                if (dt.Rows.Count > 0)
                {
                    // --- 3. TRATAMENTO DOS NÚMEROS (Remover zeros à esquerda) ---
                    string numTratado = chave.Substring(25, 9).TrimStart('0');
                    string serieTratada = chave.Substring(22, 3).TrimStart('0');
                    string Emissao = chave.Substring(4, 2) + "/" + "20" + chave.Substring(2, 2);

                    var cte = new CteLido
                    {
                        ChaveOriginal = chave,
                        Estado = dt.Rows[0]["uftra"].ToString(),
                        Municipio = dt.Rows[0]["cidtra"].ToString(),
                        Filial = dt.Rows[0]["fantra"].ToString(),
                        Numero = string.IsNullOrEmpty(numTratado) ? "0" : numTratado,
                        Serie = string.IsNullOrEmpty(serieTratada) ? "0" : serieTratada,
                        Emissao = Emissao,
                        Lancamento = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.000"),
                        Status = "Lido"
                    };

                    if (!ListaCtePorItem.ContainsKey(index))
                        ListaCtePorItem[index] = new List<CteLido>();

                    ListaCtePorItem[index].Add(cte);

                    // --- 4. ATUALIZAR GRIDVIEW ---
                    GridView gv = (GridView)item.FindControl("gvCte");
                    if (gv != null)
                    {
                        CarregarGridCte(gv, idViagem, index);
                    }

                }
                else
                {
                    MostrarMsg("CNPJ do emissor não encontrado no cadastro de clientes.");



                    // 2. Limpa o campo
                    txt.Text = string.Empty;


                    txt.Focus();
                }

                //txt.Text = string.Empty;
                //txt.Focus();
            }
            catch (Exception ex)
            {
                MostrarMsg("Erro ao processar chave: " + ex.Message);
            }
        }
        private bool CteJaExisteNoBanco(string chave)
        {
            bool existe = false;
            try
            {
                string sql = "SELECT COUNT(1) FROM tbcte WHERE chave_de_acesso = @chave";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@chave", chave);
                    if (con.State == ConnectionState.Closed) con.Open();
                    existe = Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                }
            }
            finally { con.Close(); }
            return existe;
        }
        public class CteLido
        {
            public string ChaveOriginal { get; set; } // Adicione este campo
            public string Estado { get; set; }
            public string Municipio { get; set; }
            public string Filial { get; set; }
            public string Numero { get; set; }
            public string Serie { get; set; }
            public string Lancamento { get; set; }
            public string Emissao { get; set; }
            public string Status { get; set; }
        }
        private Dictionary<int, List<CteLido>> ListaCtePorItem
        {
            get
            {
                if (Session["CTE_ITENS"] == null)
                    Session["CTE_ITENS"] = new Dictionary<int, List<CteLido>>();

                return (Dictionary<int, List<CteLido>>)Session["CTE_ITENS"];
            }
        }
        // mostrar dados da nota fiscal na grid
        public class NFeResponse
        {
            public string chave { get; set; }
            public string numero { get; set; }
            public string serie { get; set; }
            public DateTime data_emissao { get; set; }
            public string status { get; set; }
            public Emitente emitente { get; set; }
            public Destinatario destinatario { get; set; }
            public List<NFeProduto> produtos { get; set; }
        }
        public class NFeProduto
        {
            public string produto { get; set; }
            public decimal quantidade { get; set; }
            public decimal peso { get; set; }
            public decimal valor { get; set; }
        }
        protected async void txtChaveNF_TextChanged(object sender, EventArgs e)
        {
            TextBox txtChave = (TextBox)sender;
            RepeaterItem item = (RepeaterItem)txtChave.NamingContainer;

            TextBox txtCNPJRem = (TextBox)item.FindControl("txtCNPJRemetente");
            TextBox txtCNPJDest = (TextBox)item.FindControl("txtCNPJDestinatario");

            Label lblChave = (Label)item.FindControl("lblChaveNF");
            Label lblNumero = (Label)item.FindControl("lblNumeroNF");
            Label lblSerie = (Label)item.FindControl("lblSerieNF");
            Label lblEmissao = (Label)item.FindControl("lblEmissaoNF");
            Label lblStatus = (Label)item.FindControl("lblStatusNF");

            GridView gv = (GridView)item.FindControl("gvProdutosNF");

            Label lblPeso = (Label)item.FindControl("lblPesoTotalNF");
            Label lblValor = (Label)item.FindControl("lblValorTotalNF");

            try
            {
                string chave = txtChave.Text.Trim();
                chave = RemoverEspacos(txtChave.Text);
                MeuDanfeService service = new MeuDanfeService();
                NFeResponse nfe = await service.ConsultarNFe(chave);

                // ✅ Validação CNPJ
                if (LimparCNPJ(nfe.emitente.cnpj) != LimparCNPJ(txtCNPJRem.Text))
                    throw new Exception("CNPJ do remetente não confere");

                if (LimparCNPJ(nfe.destinatario.cpf_cnpj) != LimparCNPJ(txtCNPJDest.Text))
                    throw new Exception("CNPJ do destinatário não confere");

                // Preenche labels
                lblChave.Text = nfe.chave;
                lblNumero.Text = nfe.numero;
                lblSerie.Text = nfe.serie;
                lblEmissao.Text = nfe.data_emissao.ToString("dd/MM/yyyy");
                lblStatus.Text = nfe.status;

                // Grid
                gv.DataSource = nfe.produtos;
                gv.DataBind();

                // Totais
                lblPeso.Text = nfe.produtos.Sum(p => p.peso).ToString("N3");
                lblValor.Text = nfe.produtos.Sum(p => p.valor)
                    .ToString("C", new CultureInfo("pt-BR"));

                // Guarda no próprio item
                item.DataItem = nfe;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(),
                    "erro", $"alert('{ex.Message}');", true);
            }
        }
        private string LimparCNPJ(string cnpj)
        {
            return new string(cnpj.Where(char.IsDigit).ToArray());
        }
        public class Emitente
        {
            public string nome { get; set; }
            public string cnpj { get; set; }
        }
        public class Destinatario
        {
            public string nome { get; set; }
            public string cpf_cnpj { get; set; }
        }
        public void CarregaMap(string ds_placa)
        {

            string sql = "Select t.nr_idveiculo, v.ds_placa, t.ds_cidade, t.dt_posicao, t.nr_dist_referencia, t.fl_ignicao,t.ds_lat,t.ds_long,t.nr_velocidade, t.ds_rua, t.ds_uf, t.nr_direcao,t.nr_pontoreferencia,t.fl_bloqueio   ";
            sql += " from tb_transmissao as t inner join tb_veiculo_sascar as v on t.nr_idveiculo=v.nr_idveiculo where v.ds_placa='" + ds_placa + "'";
            SqlDataAdapter adpt = new SqlDataAdapter(sql, con);
            DataTable dt = new DataTable();
            con.Open();
            adpt.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                try
                {
                    hora = DateTime.Parse(dt.Rows[0][3].ToString()).ToString("dd/MM/yyyy - HH:mm:ss");

                    placa = dt.Rows[0][1].ToString();
                    lat = dt.Rows[0][6].ToString();
                    lon = dt.Rows[0][7].ToString();

                    if (dt.Rows[0][5].ToString() == "1")
                    {
                        ignicao = "Ligada";
                    }
                    else
                    {
                        ignicao = "Desligada";
                    }

                    velocidade = dt.Rows[0][8].ToString() + " Km/h";
                    rua = dt.Rows[0][9].ToString();
                    uf = dt.Rows[0][10].ToString();
                    preferencia = dt.Rows[0][12].ToString();
                    bloqueio = dt.Rows[0][13].ToString();
                    GLatLng latlng1 = new GLatLng(Convert.ToDouble(lat, CultureInfo.InvariantCulture), Convert.ToDouble(lon, CultureInfo.InvariantCulture));

                    window = new GInfoWindow(latlng1, string.Format(@"<b>Informações:</b><br />Horário: {0}<br/>Placa: {1}<br/>P. Ref: {2}<br/>Bloqueio: {3}<br/>End: {4}<br/>UF: {5}<br/>Ignição: {6}<br/>Velocidade: {7}",
                    hora, placa, preferencia, bloqueio, rua, uf, ignicao, velocidade), true);

                    // GMap1.Add(window);


                    // Focar o veículo no mapa com zoom adequado
                    // GMap1.setCenter(latlng1);
                    // GMap1.GZoom = 18;
                    //GMap1; // Ajuste conforme necessário
                    GIcon ico = new GIcon();


                    ico.iconAnchor = new GPoint(25, 10);
                    string valor = dt.Rows[0][11]?.ToString() ?? "";

                    if (valor.Length > 0)
                    {
                        string prefixo = valor.Substring(0, 1);

                        switch (prefixo)
                        {
                            case "0":
                                ico.image = "../img/ico_truck.png";
                                break;
                            case "1":
                                ico.image = "../img/ico_truck1.png";
                                break;
                            case "2":
                                ico.image = "../img/ico_truck2.png";
                                break;
                            case "3":
                                ico.image = "../img/ico_truck3.png";
                                break;
                            case "4":
                                ico.image = "../img/ico_truck4.png";
                                break;
                            case "5":
                                ico.image = "../img/ico_truck5.png";
                                break;
                            case "6":
                                ico.image = "../img/ico_truck6.png";
                                break;
                            case "7":
                                ico.image = "../img/ico_truck7.png";
                                break;
                            default:
                                ico.image = "../img/ico_truck.png";
                                break;
                        }
                    }
                    else
                    {
                        // caso valor esteja vazio ou nulo
                        ico.image = "../img/ico_truck.png";
                    }

                    GMarkerOptions mOpts = new GMarkerOptions();
                    mOpts.clickable = true;
                    mOpts.icon = ico;
                    mOpts.draggable = false;

                    GMarker marker = new GMarker(latlng1, mOpts);

                    GInfoWindow window2 = new GInfoWindow(marker, latlng1.ToString(), false, GListener.Event.mouseover);
                    GMap1.Add(window2);

                    GMap1.Add(marker);
                    GMap1.Add(window);
                    GMap1.setCenter(latlng1);
                    GMap1.GZoom = 18;
                }
                catch
                {
                    //CarregaMap();
                }



                //return "document.getElementById('Msg1').innerHTML="+ e.point.lat.ToString() + ";" + "document.getElementById('Msg2').innerHTML="+ e.point.lng.ToString();

                //return window.ToString(e.map);
            }
            else
            {
                //string linha1 = "Placa não encontrada no sistema.";


                //// Concatenando as linhas com '\n' para criar a mensagem
                //string mensagem = $"{linha1}";

                //string mensagemCodificada = HttpUtility.JavaScriptStringEncode(mensagem);
                //// Gerando o script JavaScript para exibir o alerta
                //string script = $"alert('{mensagemCodificada}');";

                //// Registrando o script para execução no lado do cliente
                //ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", script, true);

                MostrarMsgMapa(txtPlaca.Text.Trim() + " Sinal do veículo não direcionado a base de dados.", "info");
            }
        }
        protected void gvNF_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView gv = (GridView)sender;
            ControlesCarga c = ObterControles(gv);

            if (e.CommandName == "SalvarPeso")
            {
                GridViewRow row = (GridViewRow)((System.Web.UI.Control)e.CommandSource).NamingContainer;

                HiddenField hfIdNfe = (HiddenField)row.FindControl("hfIdNfe");
                TextBox txtPeso = (TextBox)row.FindControl("txtPeso");

                int idNfe = Convert.ToInt32(hfIdNfe.Value);
                decimal peso = Convert.ToDecimal(txtPeso.Text);

                int idCarga = Convert.ToInt32(e.CommandArgument);

                // 1. salva peso da NF
                AtualizarPeso(idNfe, peso);

                // 2. recalcula totais automaticamente
                var totais = CalcularTotais(idCarga);

                // 3. atualiza tbcte
                //AtualizarCTe(idCarga, totais.peso, totais.valor);

                // recarrega a grid
                CarregarNotas(Convert.ToInt32(c.Carga), c.GvNF);

                // atualiza os totais
                AtualizarResumoCarga(c);
            }
            if (e.CommandName == "BaixarDanfe")
            {
                string chaveNfe = e.CommandArgument.ToString();

                BaixarDanfe(chaveNfe);
            }

        }


        public void BaixarDanfe(string chave)
        {
            string url = "https://api.meudanfe.com.br/v2/fd/get/da/" + chave;

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "GET";
            req.Headers.Add("Api-Key", "025caf00-6477-4d97-b133-f34ad21594f3");
            req.Accept = "application/json";

            using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
            using (StreamReader reader = new StreamReader(resp.GetResponseStream()))
            {
                string json = reader.ReadToEnd();

                DanfeResult result = JsonConvert.DeserializeObject<DanfeResult>(json);

                if (result == null || string.IsNullOrEmpty(result.data))
                    MostrarMsg("API não retornou o PDF. Resposta: " + json);

                byte[] pdfBytes = Convert.FromBase64String(result.data);

                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ContentType = "application/pdf";
                HttpContext.Current.Response.AddHeader(
                    "Content-Disposition",
                    "attachment; filename=" + result.name
                );
                HttpContext.Current.Response.BinaryWrite(pdfBytes);
                HttpContext.Current.Response.End();
            }
        }
        private void AtualizarPeso(int idNfe, decimal peso)
        {
            using (SqlConnection con = new SqlConnection(
                WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                con.Open();

                string sql = "UPDATE tbnfe SET peso = @peso WHERE idnfe = @id";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@peso", peso);
                    cmd.Parameters.AddWithValue("@id", idNfe);
                    cmd.ExecuteNonQuery();

                    MostrarMsg("Peso salvo com sucesso");
                }
            }
        }
        public class ApiDanfeResponse
        {
            public bool success { get; set; }
            public string name { get; set; }
            public string data { get; set; }
        }
        public class DanfeResult
        {
            public string name { get; set; }
            public string type { get; set; }
            public string format { get; set; }
            public string data { get; set; }
        }
        protected void MostrarMsgMapa(string mensagem, string tipo = "info")
        {
            divMsgMapa.Attributes["class"] = "alert alert-" + tipo + " alert-dismissible fade show mt-3";
            lblMsgMapa.InnerText = mensagem;
            divMsgMapa.Style["display"] = "block";

            string script = @"setTimeout(function() {
                        var div = document.getElementById('divMsgMapa');
                        if (div) div.style.display = 'none';
                      }, 5000);";

            ScriptManager.RegisterStartupScript(this, GetType(), "EscondeMsg", script, true);
        }
        protected void ddlTipoMaterial_SelectedIndexChanged(object sender, EventArgs e)
        {
            AplicarRegras();
        }
        protected void tmAtualizaMapa_Tick(object sender, EventArgs e)
        {
            if (Session["placaAtual"] != null)
            {
                string ds_placa = Session["placaAtual"].ToString();
                CarregaMap(ds_placa);
            }
        }
        protected void MostrarMsg(string mensagem, string tipo = "warning")
        {
            divMsg.Attributes["class"] = "alert alert-" + tipo + " alert-dismissible fade show mt-3";
            lblMsgGeral.InnerText = mensagem;
            divMsg.Style["display"] = "block";

            string script = @"setTimeout(function() {
                        var div = document.getElementById('divMsg');
                        if (div) div.style.display = 'none';
                      }, 5000);";

            ScriptManager.RegisterStartupScript(this, GetType(), "EscondeMsg", script, true);
        }
        protected void MostrarMsgCNH(string mensagem, string tipo = "warning")
        {
            divMsgCNH.Attributes["class"] = "alert alert-" + tipo + " alert-dismissible fade show mt-3";
            lblMsgCNH.InnerText = mensagem;
            divMsgCNH.Style["display"] = "block";

            string script = @"setTimeout(function() {
                        var div = document.getElementById('divMsgCNH');
                        if (div) div.style.display = 'none';
                      }, 5000);";

            ScriptManager.RegisterStartupScript(this, GetType(), "EscondeMsg", script, true);
        }
        protected void MostrarMsgGR(string mensagem, string tipo = "warning")
        {
            divMsgGR.Attributes["class"] = "alert alert-" + tipo + " alert-dismissible fade show mt-3";
            lblMsgGR.InnerText = mensagem;
            divMsgGR.Style["display"] = "block";

            string script = @"setTimeout(function() {
                        var div = document.getElementById('divMsgGR');
                        if (div) div.style.display = 'none';
                      }, 5000);";

            ScriptManager.RegisterStartupScript(this, GetType(), "EscondeMsg", script, true);
        }
        protected void MostrarMsgVeic(string mensagem, string tipo = "warning")
        {
            divMsgVeic.Attributes["class"] = "alert alert-" + tipo + " alert-dismissible fade show mt-3";
            lblMsgVeic.InnerText = mensagem;
            divMsgVeic.Style["display"] = "block";

            string script = @"setTimeout(function() {
                        var div = document.getElementById('divMsgVeic');
                        if (div) div.style.display = 'none';
                      }, 5000);";

            ScriptManager.RegisterStartupScript(this, GetType(), "EscondeMsg", script, true);
        }
        protected void MostrarMsgCrono(string mensagem, string tipo = "warning")
        {
            divMsgCrono.Attributes["class"] = "alert alert-" + tipo + " alert-dismissible fade show mt-3";
            lblMsgCrono.InnerText = mensagem;
            divMsgCrono.Style["display"] = "block";

            string script = @"setTimeout(function() {
                        var div = document.getElementById('divMsgCrono');
                        if (div) div.style.display = 'none';
                      }, 5000);";

            ScriptManager.RegisterStartupScript(this, GetType(), "EscondeMsg", script, true);
        }
        protected void MostrarMsgLinc(string mensagem, string tipo = "warning")
        {
            divMsgLinc.Attributes["class"] = "alert alert-" + tipo + " alert-dismissible fade show mt-3";
            lblMsgLinc.InnerText = mensagem;
            divMsgLinc.Style["display"] = "block";

            string script = @"setTimeout(function() {
                        var div = document.getElementById('divMsgLinc');
                        if (div) div.style.display = 'none';
                      }, 5000);";

            ScriptManager.RegisterStartupScript(this, GetType(), "EscondeMsg", script, true);
        }
        protected void MostrarMsgCET(string mensagem, string tipo = "warning")
        {
            divMsgCET.Attributes["class"] = "alert alert-" + tipo + " alert-dismissible fade show mt-3";
            lblMsgCET.InnerText = mensagem;
            divMsgCET.Style["display"] = "block";

            string script = @"setTimeout(function() {
                        var div = document.getElementById('divMsgCET');
                        if (div) div.style.display = 'none';
                      }, 5000);";

            ScriptManager.RegisterStartupScript(this, GetType(), "EscondeMsg", script, true);
        }
        protected void MostrarMsgCarreta1(string mensagem, string tipo = "warning")
        {
            divMsgCarreta1.Attributes["class"] = "alert alert-" + tipo + " alert-dismissible fade show mt-3";
            lblMsgCarreta1.InnerText = mensagem;
            divMsgCarreta1.Style["display"] = "block";

            string script = @"setTimeout(function() {
                        var div = document.getElementById('divMsgCarreta1');
                        if (div) div.style.display = 'none';
                      }, 5000);";

            ScriptManager.RegisterStartupScript(this, GetType(), "EscondeMsg", script, true);
        }
        protected void MostrarMsgCarreta2(string mensagem, string tipo = "warning")
        {
            divMsgCarreta1.Attributes["class"] = "alert alert-" + tipo + " alert-dismissible fade show mt-3";
            lblMsgCarreta2.InnerText = mensagem;
            divMsgCarreta2.Style["display"] = "block";

            string script = @"setTimeout(function() {
                        var div = document.getElementById('divMsgCarreta2');
                        if (div) div.style.display = 'none';
                      }, 5000);";

            ScriptManager.RegisterStartupScript(this, GetType(), "EscondeMsg", script, true);
        }

        protected void txtMDFe_TextChanged(object sender, EventArgs e)
        {

            // 35260255890016000109580010000169611446726211
            TextBox txtMDFe = (TextBox)sender;

            // Pega o item do repeater
            RepeaterItem item = (RepeaterItem)txtMDFe.NamingContainer;

            // Agora você pode acessar outros controles do mesmo item
            HiddenField hdflIdviagem = (HiddenField)item.FindControl("hdflIdviagem");

            string chave = txtMDFe.Text.Trim();
            string idViagem = hdflIdviagem.Value;


            // 🔎 1 - Validar tamanho
            if (chave.Length != 44 || !chave.All(char.IsDigit))
            {
                MostrarMsg("A chave de acesso deve conter 44 dígitos numéricos.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                conn.Open();

                // 🔎 2 - Verificar se já existe
                SqlCommand cmdExiste = new SqlCommand(
                    "SELECT COUNT(*) FROM tbcargas WHERE mdfe = @mdfe", conn);
                cmdExiste.Parameters.AddWithValue("@mdfe", chave);

                int existe = (int)cmdExiste.ExecuteScalar();
                if (existe > 0)
                {
                    MostrarMsg("Esta chave já está cadastrada.", "warning");
                    return;
                }

                // =============================
                // 🔎 3 - Extrair dados da chave
                // =============================

                string ufCodigo = chave.Substring(0, 2);          // posição 1-2
                string cnpj = chave.Substring(6, 14);             // posição 7-20
                string serie = chave.Substring(22, 3);            // posição 23-25
                string numero = chave.Substring(25, 9);           // posição 26-34
                string dv = chave.Substring(43, 1);               // posição 44

                serie = Convert.ToInt32(serie).ToString();   // remove zeros
                numero = Convert.ToInt64(numero).ToString(); // remove zeros

                // =============================
                // 🔎 4 - Buscar UF
                // =============================

                SqlCommand cmdUF = new SqlCommand(
                    "SELECT Estado FROM tbestadosbrasileiros WHERE Uf = @uf", conn);
                cmdUF.Parameters.AddWithValue("@uf", ufCodigo);

                object estadoObj = cmdUF.ExecuteScalar();
                if (estadoObj == null)
                {
                    MostrarMsg("UF não encontrada.");
                    return;
                }
                string estado = estadoObj.ToString();

                // =============================
                // 🔎 5 - Buscar Empresa (CNPJ)
                // =============================

                SqlCommand cmdEmpresa = new SqlCommand(@"
            SELECT nomcli 
            FROM tbclientes 
            WHERE REPLACE(REPLACE(REPLACE(cnpj,'.',''),'/',''),'-','') = @cnpj", conn);

                cmdEmpresa.Parameters.AddWithValue("@cnpj", cnpj);

                object empresaObj = cmdEmpresa.ExecuteScalar();
                if (empresaObj == null)
                {
                    MostrarMsg("Empresa não encontrada para o CNPJ informado.");
                    return;
                }
                string empresa = empresaObj.ToString();

                // =============================
                // 🔎 6 - Validar Dígito Verificador
                // =============================

                if (!ValidarChaveMDFe(chave))
                {
                    MostrarMsg("Chave inválida (dígito verificador incorreto).");
                    return;
                }

                // =============================
                // 🔎 7 - Atualizar tbcargas
                // =============================

                SqlCommand cmdUpdate = new SqlCommand(@"
            UPDATE tbcargas
            SET mdfe = @mdfe,
                mdfe_uf = @uf,
                mdfe_empresa = @empresa,
                mdfe_numero = @numero,
                mdfe_serie = @serie,
                mdfe_situacao = 'Pendente',
                mdfe_dv = @dv
            WHERE carga = @carga", conn);

                cmdUpdate.Parameters.AddWithValue("@mdfe", chave);
                cmdUpdate.Parameters.AddWithValue("@uf", estado);
                cmdUpdate.Parameters.AddWithValue("@empresa", empresa);
                cmdUpdate.Parameters.AddWithValue("@numero", numero);
                cmdUpdate.Parameters.AddWithValue("@serie", serie);
                cmdUpdate.Parameters.AddWithValue("@dv", dv);
                cmdUpdate.Parameters.AddWithValue("@carga", idViagem);

                cmdUpdate.ExecuteNonQuery();

                MostrarMsg("MDF-e vinculado com sucesso.");
            }
        }

        private bool ValidarChaveMDFe(string chave)
        {
            int[] peso = { 4, 3, 2, 9, 8, 7, 6, 5 };
            int soma = 0;
            int pesoIndex = 0;

            for (int i = 0; i < 43; i++)
            {
                soma += (chave[i] - '0') * peso[pesoIndex];
                pesoIndex++;
                if (pesoIndex == 8)
                    pesoIndex = 0;
            }

            int resto = soma % 11;
            int dvCalculado = (resto == 0 || resto == 1) ? 0 : 11 - resto;

            return dvCalculado.ToString() == chave[43].ToString();
        }
        protected void AplicarRegras()
        {
            string tipoMaterial = ddlTipoMaterial.SelectedValue;
            int codInicial = Convert.ToInt32(codCliInicial.Text);

            // 🟢 ALMOXARIFADO
            if (tipoMaterial == "Almoxarifado")
            {
                if (codInicial == 1000 || codInicial == 7236)
                {
                    txtCod_PagadorVazio.Text = codInicial.ToString();
                    txtPagadorVazio.Text = "FERROLENE SA INDUSTRIA E COMERCIO DE METAIS";
                }
            }

            // 🟢 EMBALAGEM
            else if (tipoMaterial == "Embalagem")
            {
                txtCod_PagadorVazio.Text = "1000";
                txtPagadorVazio.Text = "FERROLENE SA INDUSTRIA E COMERCIO DE METAIS";
            }

            // 🟢 TIPO MATERIAL VAZIO
            else if (tipoMaterial == "Vazio")
            {
                txtCod_PagadorVazio.Text = "1111";
                txtPagadorVazio.Text = "TRANSNOVAG TRANSPORTES SA";
            }
        }
        private void CancelarCarga(string carga)
        {
            string usuario = dataHoraAtual.ToString("dd/MM/yyyy HH:mm") + " - " + Session["UsuarioLogado"].ToString();
            string usuario2 = Session["UsuarioLogado"].ToString();

            using (SqlConnection conn = new SqlConnection(
                WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        int idViagem;

                        // 🔹 1. Buscar idviagem da carga
                        string sqlBusca = "SELECT idviagem FROM tbcargas WHERE carga = @carga";

                        using (SqlCommand cmd = new SqlCommand(sqlBusca, conn, trans))
                        {
                            cmd.Parameters.Add("@carga", SqlDbType.Int).Value = Convert.ToInt32(carga);
                            idViagem = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        // 🔹 2. Cancelar a carga
                        string sqlUpdate = @"UPDATE tbcargas 
                                         SET status = 'Cancelada',
                                         andamento = 'CANCELADA',
                                         atualizacao = @usuario, 
                                         fl_exclusao = 'S' 
                                         WHERE carga = @carga";

                        using (SqlCommand cmd = new SqlCommand(sqlUpdate, conn, trans))
                        {
                            cmd.Parameters.Add("@carga", SqlDbType.Int).Value = Convert.ToInt32(carga);
                            cmd.Parameters.Add("@usuario", SqlDbType.VarChar).Value = usuario;
                            cmd.ExecuteNonQuery();
                        }

                        // 🔹 3. Verificar se ainda existem cargas ativas
                        string sqlVerifica = @"SELECT COUNT(*) 
                                       FROM tbcargas 
                                       WHERE idviagem = @idviagem 
                                       AND fl_exclusao IS NULL";

                        int totalAtivas = 0;

                        using (SqlCommand cmd = new SqlCommand(sqlVerifica, conn, trans))
                        {
                            cmd.Parameters.Add("@idviagem", SqlDbType.Int).Value = idViagem;
                            totalAtivas = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        // 🔹 4. Se não existir nenhuma ativa → cancelar OC
                        if (totalAtivas == 0)
                        {
                            string sqlOC = @"UPDATE tbcarregamentos
                                     SET status = 'Cancelada',
                                         situacao = 'O. C. CANCELADA',
                                         fl_exclusao = 'S',
                                         usualt = @usuario,
                                         dtalt = GETDATE()
                                     WHERE num_carregamento = @idviagem";

                            using (SqlCommand cmd = new SqlCommand(sqlOC, conn, trans))
                            {
                                cmd.Parameters.Add("@idviagem", SqlDbType.Int).Value = idViagem;
                                cmd.Parameters.Add("@usuario", SqlDbType.VarChar).Value = usuario2;

                                cmd.ExecuteNonQuery();
                            }
                        }

                        // 🔥 commit
                        trans.Commit();
                        CarregarColetas(novaColeta.Text);
                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            // 🔄 Recarrega o grid


        }

        void GravarHistorico(
        string tabela,
        int idRegistro,
        string campo,
        string valorAntigo,
        string valorNovo,
        string usuario)
        {
            if (valorAntigo == valorNovo) return;

            SqlCommand cmd = new SqlCommand(@"
        INSERT INTO tbHistoricoCampo
        (tabela, idRegistro, campo, valorAntigo, valorNovo, usuario, dataAlteracao)
        VALUES
        (@tabela, @idRegistro, @campo, @valorAntigo, @valorNovo, @usuario, GETDATE())
    ", con);

            cmd.Parameters.AddWithValue("@tabela", tabela);
            cmd.Parameters.AddWithValue("@idRegistro", idRegistro);
            cmd.Parameters.AddWithValue("@campo", campo);
            cmd.Parameters.AddWithValue("@valorAntigo", valorAntigo ?? "");
            cmd.Parameters.AddWithValue("@valorNovo", valorNovo ?? "");
            cmd.Parameters.AddWithValue("@usuario", usuario);

            cmd.ExecuteNonQuery();
        }

        private void AtualizarPremioMotorista()
        {
            // SOMENTE FUNCIONÁRIO
            if (txtTipoMot.Text.Trim().ToUpper() != "FUNCIONÁRIO")
                return;

            using (SqlConnection conn = new SqlConnection(
                WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                conn.Open();

                SqlTransaction trans = conn.BeginTransaction();

                try
                {
                    // FUNÇÃO
                    string funcao = "";

                    if (!string.IsNullOrWhiteSpace(txtFuncao.Text))
                    {
                        funcao = txtFuncao.Text
                            .Trim()
                            .Split(' ')[0]
                            .ToUpper();
                    }

                    // ============================================
                    // PERCORRE O REPEATER
                    // ============================================
                    foreach (RepeaterItem item in rptColetas.Items)
                    {
                        // ============================================
                        // CARGA
                        // ============================================
                        Label lblCarga = (Label)item.FindControl("lblCarga");

                        if (lblCarga == null)
                            continue;

                        string carga = lblCarga.Text.Trim();

                        // ============================================
                        // DATA SAÍDA
                        // ============================================
                        TextBox txtSaidaPlanta = (TextBox)item.FindControl("txtSaidaPlanta");

                        if (txtSaidaPlanta == null)
                            continue;

                        DateTime dataSaida;

                        if (!DateTime.TryParse(txtSaidaPlanta.Text, out dataSaida))
                            continue;

                        decimal distancia = 0;
                        decimal valorPremio = 0;

                        // ============================================
                        // BUSCA DISTÂNCIA
                        // ============================================
                        using (SqlCommand cmdDist = new SqlCommand(@"
                    SELECT TOP 1 distancia
                    FROM tbcargas
                    WHERE carga = @carga", conn, trans))
                        {
                            cmdDist.Parameters.AddWithValue("@carga", carga);

                            object result = cmdDist.ExecuteScalar();

                            if (result == null || result == DBNull.Value)
                                continue;

                            distancia = Convert.ToDecimal(result);
                        }

                        // ============================================
                        // BUSCA VALOR DO PRÊMIO
                        // ============================================
                        using (SqlCommand cmdPremio = new SqlCommand(@"
                    SELECT TOP 1
                        motorista,
                        carreteiro,
                        bitrem,
                        desengate,
                        cafe,
                        refeicao,
                        pernoite
                    FROM tbvalorpremiomotoristas
                    WHERE @distancia BETWEEN distancia1 AND distancia2 and empresa='MATRIZ'",
                            conn, trans))
                        {
                            cmdPremio.Parameters.AddWithValue("@distancia", distancia);

                            using (SqlDataReader dr = cmdPremio.ExecuteReader())
                            {
                                if (dr.Read())
                                {
                                    switch (funcao)
                                    {
                                        case "MOTORISTA":
                                            valorPremio =
                                                dr["motorista"] != DBNull.Value
                                                ? Convert.ToDecimal(dr["motorista"])
                                                : 0;
                                            break;

                                        case "CARRETEIRO":
                                            valorPremio =
                                                dr["carreteiro"] != DBNull.Value
                                                ? Convert.ToDecimal(dr["carreteiro"])
                                                : 0;
                                            break;

                                        case "BITREM":
                                            valorPremio =
                                                dr["bitrem"] != DBNull.Value
                                                ? Convert.ToDecimal(dr["bitrem"])
                                                : 0;
                                            break;

                                        default:
                                            valorPremio = 0;
                                            break;
                                    }
                                }
                            }
                        }

                        // ============================================
                        // VERIFICA SE EXISTE
                        // ============================================
                        using (SqlCommand cmdExiste = new SqlCommand(@"
                    SELECT COUNT(*)
                    FROM tb_custo_motorista
                    WHERE cod_cracha = @cod
                    AND dt_custo = @data",
                            conn, trans))
                        {
                            cmdExiste.Parameters.AddWithValue("@cod", txtCodMotorista.Text);
                            cmdExiste.Parameters.AddWithValue("@data", dataSaida);

                            int existe = Convert.ToInt32(cmdExiste.ExecuteScalar());

                            // ============================================
                            // UPDATE
                            // ============================================
                            if (existe > 0)
                            {
                                using (SqlCommand cmdUpdate = new SqlCommand(@"
                            UPDATE tb_custo_motorista
                            SET vl_premio = ISNULL(vl_premio,0) + @valor
                            WHERE cod_cracha = @cod
                            AND dt_custo = @data",
                                    conn, trans))
                                {
                                    cmdUpdate.Parameters.AddWithValue("@valor", valorPremio);
                                    cmdUpdate.Parameters.AddWithValue("@cod", txtCodMotorista.Text);
                                    cmdUpdate.Parameters.AddWithValue("@data", dataSaida);

                                    cmdUpdate.ExecuteNonQuery();
                                }
                            }
                            else
                            {
                                // ============================================
                                // INSERT
                                // ============================================
                                using (SqlCommand cmdInsert = new SqlCommand(@"
                            INSERT INTO tb_custo_motorista
                            (
                                cod_cracha,
                                dt_custo,
                                vl_premio
                            )
                            VALUES
                            (
                                @cod,
                                @data,
                                @valor
                            )",
                                    conn, trans))
                                {
                                    cmdInsert.Parameters.AddWithValue("@cod", txtCodMotorista.Text);
                                    cmdInsert.Parameters.AddWithValue("@data", dataSaida);
                                    cmdInsert.Parameters.AddWithValue("@valor", valorPremio);

                                    cmdInsert.ExecuteNonQuery();
                                }
                            }
                        }
                    }

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();

                    throw new Exception(
                        "Erro ao atualizar prêmio do motorista: " + ex.Message);
                }
            }
        }

        protected void MostrarMsgSolicitacao(RepeaterItem item, string mensagem, string tipo = "warning")
        {
            HtmlGenericControl divMsg =
                (HtmlGenericControl)item.FindControl("divMsgSolicitacao");

            HtmlGenericControl lblMsg =
                (HtmlGenericControl)item.FindControl("lblMsgSolicitacao");

            if (divMsg == null || lblMsg == null)
                return;

            divMsg.Attributes["class"] = $"alert alert-{tipo} alert-dismissible fade show mt-3";
            lblMsg.InnerText = mensagem;
            divMsg.Style["display"] = "block";
        }

        //private string GerarNumeroCVA(SqlConnection conn,
        //                      SqlTransaction trans)
        //{
        //    SqlCommand cmd = new SqlCommand(@"

        //    DECLARE @Numero INT;

        //    SELECT @Numero = cva
        //    FROM tbcontadores WITH (UPDLOCK,HOLDLOCK);

        //    SET @Numero=@Numero+1;

        //    UPDATE tbcontadores
        //    SET cva=@Numero;

        //    SELECT @Numero;

        //    ", conn, trans);

        //    int numero = Convert.ToInt32(cmd.ExecuteScalar());

        //    return "TNG" + numero.ToString("D7");
        //}
        //private void SalvarCVA()
        //{
        //    using (SqlConnection conn = new SqlConnection(
        //        ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
        //    {
        //        conn.Open();

        //        SqlTransaction trans = conn.BeginTransaction();

        //        try
        //        {
        //            // 1 - Gera número CVA
        //            string numeroCVA = GerarNumeroCVA(conn, trans);


        //            // 2 - Grava cabeçalho
        //            InserirCabecalho(conn, trans, numeroCVA);


        //            // 3 - Copia produtos da solicitação
        //            CopiarProdutos(conn,
        //                           trans,
        //                           numeroCVA,
        //                           numeroSolicitacao);


        //            // 4 - Copia embalagens da solicitação
        //            CopiarEmbalagens(conn,
        //                             trans,
        //                             numeroCVA,
        //                             txtNumeroSolicitacao.Text);


        //            // 5 - Copia quantidades da solicitação
        //            CopiarQuantidades(conn,
        //                              trans,
        //                              numeroCVA,
        //                              txtNumeroSolicitacao.Text);


        //            // 6 - Grava fornecedor vindo da tela
        //            InserirFornecedor(conn,
        //                              trans,
        //                              numeroCVA);


        //            // Confirma tudo
        //            trans.Commit();


        //            lblMensagem.Text =
        //                "CVA gerada com sucesso: " + numeroCVA;
        //        }
        //        catch (Exception ex)
        //        {
        //            trans.Rollback();

        //            lblMensagem.Text =
        //                "Erro ao gerar CVA: " + ex.Message;
        //        }
        //    }
        //}
        //private void CopiarProdutos(SqlConnection conn,
        //                    SqlTransaction trans,
        //                    string numeroCVA,
        //                    string numeroSolicitacao)
        //{
        //    string sql = @"
        //INSERT INTO tbcva_produtos
        //(
        //    r3_tipo_registro,
        //    r3_numero_cva,
        //    r3_numero_solicitacao,
        //    r3_cod_produto,
        //    r3_quant_produto
        //)
        //SELECT
        //    r2_sol_tipo_registro,
        //    @numeroCVA,
        //    r2_sol_numero,
        //    r2_sol_codigo_produto,
        //    r2_sol_quant_solicitada_produto
        //FROM tbsolicitacoes_produtos
        //WHERE r2_sol_numero = @numeroSolicitacao";

        //    using (SqlCommand cmd = new SqlCommand(sql, conn, trans))
        //    {
        //        cmd.Parameters.AddWithValue("@numeroCVA", numeroCVA);
        //        cmd.Parameters.AddWithValue("@numeroSolicitacao", numeroSolicitacao);

        //        cmd.ExecuteNonQuery();
        //    }
        //}

        //private void CopiarEmbalagens(SqlConnection conn,
        //                      SqlTransaction trans,
        //                      string numeroCVA,
        //                      string numeroSolicitacao)
        //{
        //    string sql = @"
        //INSERT INTO tbcva_embalagens
        //(
        //    r4_tipo_registro,
        //    r4_numero_cva,
        //    r4_numero_solicitacao,
        //    r4_cod_embalagem,
        //    r4_quant_embalagem
        //)
        //SELECT
        //    r3_sol_tipo_registro,
        //    @numeroCVA,
        //    r3_sol_numero,
        //    r3_sol_codigo_embalagem,
        //    r3_sol_quant_embalagem
        //FROM tbsolicitacoes_embalagens
        //WHERE r3_sol_numero = @numeroSolicitacao";

        //    using (SqlCommand cmd = new SqlCommand(sql, conn, trans))
        //    {
        //        cmd.Parameters.AddWithValue("@numeroCVA", numeroCVA);
        //        cmd.Parameters.AddWithValue("@numeroSolicitacao", numeroSolicitacao);

        //        cmd.ExecuteNonQuery();
        //    }
        //}
        //private void CopiarQuantidades(SqlConnection conn,
        //                       SqlTransaction trans,
        //                       string numeroCVA,
        //                       string numeroSolicitacao)
        //{
        //    string sql = @"
        //INSERT INTO tbcva_quantidades
        //(
        //    r6_tipo_registro,
        //    r6_numero_cva,
        //    r6_numero_solicitacao,
        //    r6_quant_registro_01,
        //    r6_quant_registro_02,
        //    r6_quant_registro_03,
        //    r6_quant_registro_04,
        //    r6_quant_registro_05
        //)
        //SELECT
        //    r4_sol_tipo_registro,
        //    @numeroCVA,
        //    r4_sol_numero_solicitacao,
        //    r4_sol_quant_registro_01,
        //    r4_sol_quant_registro_02,
        //    r4_sol_quant_registro_03,
        //    r4_sol_quant_registro_04,
        //    r4_sol_quant_registro_05
        //FROM tbsolicitacoes_quantidades
        //WHERE r4_sol_numero_solicitacao = @numeroSolicitacao";

        //    using (SqlCommand cmd = new SqlCommand(sql, conn, trans))
        //    {
        //        cmd.Parameters.AddWithValue("@numeroCVA", numeroCVA);
        //        cmd.Parameters.AddWithValue("@numeroSolicitacao", numeroSolicitacao);

        //        cmd.ExecuteNonQuery();
        //    }
        //}
        //private void InserirFornecedor(SqlConnection conn,
        //                        SqlTransaction trans,
        //                        string numeroCVA)
        //{
        //    string sql = @"
        //INSERT INTO tbcva_fornecedor
        //(
        //    r2_tipo_registro,
        //    r2_numero_cva,
        //    r2_numero_solicitacao,
        //    r2_tipo_registro1,
        //    r2_cod_fornecedor,
        //    r2_prev_chegada,
        //    r2_prev_saida,
        //    r2_cod_just_reagendamento,
        //    r2_obs_just_reagendamento,
        //    r2_coleta_entrega
        //)
        //VALUES
        //(
        //    @tipoRegistro,
        //    @numeroCVA,
        //    @numeroSolicitacao,
        //    @tipoRegistro1,
        //    @codFornecedor,
        //    @prevChegada,
        //    @prevSaida,
        //    @codJustReagendamento,
        //    @obsJustReagendamento,
        //    @coletaEntrega
        //)";

        //    using (SqlCommand cmd = new SqlCommand(sql, conn, trans))
        //    {
        //        cmd.Parameters.AddWithValue("@tipoRegistro", "05");
        //        cmd.Parameters.AddWithValue("@numeroCVA", numeroCVA);
        //        cmd.Parameters.AddWithValue("@numeroSolicitacao", txtNumeroSolicitacao.Text);

        //        cmd.Parameters.AddWithValue("@tipoRegistro1", ddlTipoRegistro.SelectedValue);
        //        cmd.Parameters.AddWithValue("@codFornecedor", txtCodFornecedor.Text);

        //        cmd.Parameters.AddWithValue("@prevChegada",
        //            string.IsNullOrEmpty(txtPrevChegada.Text)
        //            ? (object)DBNull.Value
        //            : Convert.ToDateTime(txtPrevChegada.Text));

        //        cmd.Parameters.AddWithValue("@prevSaida",
        //            string.IsNullOrEmpty(txtPrevSaida.Text)
        //            ? (object)DBNull.Value
        //            : Convert.ToDateTime(txtPrevSaida.Text));

        //        cmd.Parameters.AddWithValue("@codJustReagendamento",
        //            txtCodJustReagendamento.Text);

        //        cmd.Parameters.AddWithValue("@obsJustReagendamento",
        //            txtObsJustReagendamento.Text);

        //        cmd.Parameters.AddWithValue("@coletaEntrega",
        //            ddlColetaEntrega.SelectedValue);

        //        cmd.ExecuteNonQuery();
        //    }
        //}
        //private void InserirCabecalho(SqlConnection conn,
        //                      SqlTransaction trans,
        //                      string numeroCVA)
        //{
        //    string sql = @"
        //INSERT INTO tbcvas
        //(
        //    r1_tipo_registro,
        //    r1_cod_planta,
        //    r1_numero_cva,
        //    r1_cod_transportadora,
        //    r1_placa,
        //    r1_cpf_motorista,
        //    r1_prev_cheg_gate,
        //    r1_tipo_veiculo,
        //    r1_tipo_viagem,
        //    r1_cod_tipo_solicitacao,
        //    r1_numero_solicitacao,
        //    r1_tipo_geracao_cva,
        //    r1_nome_motorista,
        //    r1_num_rota,
        //    r1_fluxo,
        //    r1_km_total,
        //    situacao,
        //    tipo,
        //    viagem_com_retorno,
        //    devolucao_peca,
        //    data_solicitacao
        //)
        //VALUES
        //(
        //    @tipoRegistro,
        //    @codPlanta,
        //    @numeroCVA,
        //    @codTransportadora,
        //    @placa,
        //    @cpfMotorista,
        //    @prevChegadaGate,
        //    @tipoVeiculo,
        //    @tipoViagem,
        //    @codTipoSolicitacao,
        //    @numeroSolicitacao,
        //    @tipoGeracaoCVA,
        //    @nomeMotorista,
        //    @numRota,
        //    @fluxo,
        //    @kmTotal,
        //    @situacao,
        //    @tipo,
        //    @viagemRetorno,
        //    @devolucaoPeca,
        //    GETDATE()
        //)";


        //    using (SqlCommand cmd = new SqlCommand(sql, conn, trans))
        //    {
        //        cmd.Parameters.AddWithValue("@tipoRegistro", "01");

        //        cmd.Parameters.AddWithValue("@codPlanta",
        //            txtCodPlanta.Text);

        //        cmd.Parameters.AddWithValue("@numeroCVA",
        //            numeroCVA);

        //        cmd.Parameters.AddWithValue("@codTransportadora",
        //            txtCodTransportadora.Text);

        //        cmd.Parameters.AddWithValue("@placa",
        //            txtPlaca.Text);

        //        cmd.Parameters.AddWithValue("@cpfMotorista",
        //            txtCpfMotorista.Text);

        //        cmd.Parameters.AddWithValue("@prevChegadaGate",
        //            string.IsNullOrEmpty(txtPrevChegadaGate.Text)
        //            ? (object)DBNull.Value
        //            : Convert.ToDateTime(txtPrevChegadaGate.Text));

        //        cmd.Parameters.AddWithValue("@tipoVeiculo",
        //            txtTipoVeiculo.Text);

        //        cmd.Parameters.AddWithValue("@tipoViagem",
        //            txtTipoViagem.Text);

        //        cmd.Parameters.AddWithValue("@codTipoSolicitacao",
        //            txtCodTipoSolicitacao.Text);

        //        cmd.Parameters.AddWithValue("@numeroSolicitacao",
        //            txtNumeroSolicitacao.Text);

        //        cmd.Parameters.AddWithValue("@tipoGeracaoCVA",
        //            "MANUAL");

        //        cmd.Parameters.AddWithValue("@nomeMotorista",
        //            txtNomeMotorista.Text);

        //        cmd.Parameters.AddWithValue("@numRota",
        //            txtRota.Text);

        //        cmd.Parameters.AddWithValue("@fluxo",
        //            txtFluxo.Text);

        //        cmd.Parameters.AddWithValue("@kmTotal",
        //            string.IsNullOrEmpty(txtKmTotal.Text)
        //            ? 0
        //            : Convert.ToDecimal(txtKmTotal.Text));

        //        cmd.Parameters.AddWithValue("@situacao",
        //            "ABERTA");

        //        cmd.Parameters.AddWithValue("@tipo",
        //            ddlTipo.SelectedValue);

        //        cmd.Parameters.AddWithValue("@viagemRetorno",
        //            ddlRetorno.SelectedValue);

        //        cmd.Parameters.AddWithValue("@devolucaoPeca",
        //            ddlDevolucaoPeca.SelectedValue);


        //        cmd.ExecuteNonQuery();
        //    }
        //}
        //private const string SQL_INSERT_CVA = @"

        //    INSERT INTO tbcvas
        //    (
        //        r1_tipo_registro,
        //        r1_cod_planta,
        //        r1_numero_cva,
        //        r1_cod_transportadora,
        //        r1_placa,
        //        r1_cpf_motorista,
        //        r1_prev_cheg_gate,
        //        r1_tipo_veiculo,
        //        r1_tipo_viagem,
        //        r1_cod_tipo_solicitacao,
        //        r1_numero_solicitacao,
        //        r1_tipo_geracao_cva,
        //        r1_cod_just_reagendamento,
        //        r1_obs_just_reagendamento,
        //        r1_rg_motorista,
        //        r1_nome_motorista,
        //        r1_num_rota,
        //        r1_fluxo,
        //        r1_just_tipo_veiculo,
        //        r1_conta,
        //        r1_setor,
        //        r1_km_total,
        //        r1_num_remessa,

        //        situacao,
        //        tipo,
        //        viagem_com_retorno,
        //        devolucao_peca,

        //        cod_remetente,
        //        remetente,
        //        cnpj_remetente,
        //        cid_remetente,
        //        uf_remetente,

        //        cod_expedidor,
        //        expedidor,
        //        cnpj_expedidor,
        //        cid_expedidor,
        //        uf_expedidor,

        //        cod_destinatario,
        //        destinatario,
        //        cnpj_destinatario,
        //        cid_destinatario,
        //        uf_destinatario,

        //        cod_recebedor,
        //        recebedor,
        //        cnpj_recebedor,
        //        cid_recebedor,
        //        uf_recebedor,

        //        cod_pagador,
        //        pagador,
        //        cnpj_pagador,
        //        cid_pagador,
        //        uf_pagador,

        //        tipo_cva,
        //        estabelecimento,
        //        cnpj_recebedor1,

        //        tipo_viagem,
        //        tipo_geracao,
        //        conta,
        //        centro_custo,

        //        data_solicitacao,
        //        data_entrega,

        //        tipo_solicitacao,
        //        tipo_veiculo,
        //        cap_veiculo,

        //        cod_motorista,
        //        transp_motorista,
        //        transp_veiculo,
        //        transp_reboque,

        //        reboque1,
        //        reboque2,

        //        resp_abertura
        //    )

        //    VALUES
        //    (
        //        @r1_tipo_registro,
        //        @r1_cod_planta,
        //        @r1_numero_cva,
        //        @r1_cod_transportadora,
        //        @r1_placa,
        //        @r1_cpf_motorista,
        //        @r1_prev_cheg_gate,
        //        @r1_tipo_veiculo,
        //        @r1_tipo_viagem,
        //        @r1_cod_tipo_solicitacao,
        //        @r1_numero_solicitacao,
        //        @r1_tipo_geracao_cva,
        //        @r1_cod_just_reagendamento,
        //        @r1_obs_just_reagendamento,
        //        @r1_rg_motorista,
        //        @r1_nome_motorista,
        //        @r1_num_rota,
        //        @r1_fluxo,
        //        @r1_just_tipo_veiculo,
        //        @r1_conta,
        //        @r1_setor,
        //        @r1_km_total,
        //        @r1_num_remessa,

        //        @situacao,
        //        @tipo,
        //        @viagem_com_retorno,
        //        @devolucao_peca,

        //        @cod_remetente,
        //        @remetente,
        //        @cnpj_remetente,
        //        @cid_remetente,
        //        @uf_remetente,

        //        @cod_expedidor,
        //        @expedidor,
        //        @cnpj_expedidor,
        //        @cid_expedidor,
        //        @uf_expedidor,

        //        @cod_destinatario,
        //        @destinatario,
        //        @cnpj_destinatario,
        //        @cid_destinatario,
        //        @uf_destinatario,

        //        @cod_recebedor,
        //        @recebedor,
        //        @cnpj_recebedor,
        //        @cid_recebedor,
        //        @uf_recebedor,

        //        @cod_pagador,
        //        @pagador,
        //        @cnpj_pagador,
        //        @cid_pagador,
        //        @uf_pagador,

        //        @tipo_cva,
        //        @estabelecimento,
        //        @cnpj_recebedor1,

        //        @tipo_viagem2,
        //        @tipo_geracao,
        //        @conta,
        //        @centro_custo,

        //        @data_solicitacao,
        //        @data_entrega,

        //        @tipo_solicitacao,
        //        @tipo_veiculo2,
        //        @cap_veiculo,

        //        @cod_motorista,
        //        @transp_motorista,
        //        @transp_veiculo,
        //        @transp_reboque,

        //        @reboque1,
        //        @reboque2,

        //        @resp_abertura
        //)";
        //private void InserirCabecalho(SqlConnection conn,
        //                          SqlTransaction trans,
        //                          CVA c)
        //{
        //    using (SqlCommand cmd = new SqlCommand(SQL_INSERT_CVA, conn, trans))
        //    {
        //        AdicionarParametrosCabecalho(cmd, c);

        //        cmd.ExecuteNonQuery();
        //    }
        //}
        //private void AdicionarParametrosCabecalho(SqlCommand cmd, CVA c)
        //{
        //    SqlHelper.NVarChar(cmd, "@r1_tipo_registro", 2, c.TipoRegistro);

        //    SqlHelper.Int(cmd, "@r1_cod_planta", c.CodPlanta);

        //    SqlHelper.NVarChar(cmd, "@r1_numero_cva", 20, c.NumeroCVA);

        //    SqlHelper.NVarChar(cmd, "@r1_cod_transportadora", 20, c.CodTransportadora);

        //    SqlHelper.NVarChar(cmd, "@r1_placa", 10, c.Placa);

        //    SqlHelper.NVarChar(cmd, "@r1_cpf_motorista", 20, c.CPFMotorista);

        //    SqlHelper.Date(cmd, "@r1_prev_cheg_gate", c.PrevChegGate);

        //    SqlHelper.NVarChar(cmd, "@r1_tipo_veiculo", 100, c.TipoVeiculo);

        //    SqlHelper.NVarChar(cmd, "@r1_tipo_viagem", 100, c.TipoViagem);

        //    SqlHelper.NVarChar(cmd, "@r1_cod_tipo_solicitacao", 30, c.CodTipoSolicitacao);

        //    SqlHelper.NVarChar(cmd, "@r1_numero_solicitacao", 30, c.NumeroSolicitacao);

        //    SqlHelper.NVarChar(cmd, "@r1_tipo_geracao_cva", 50, c.TipoGeracaoCVA);

        //    SqlHelper.NVarChar(cmd, "@r1_cod_just_reagendamento", 30, c.CodJustReagendamento);

        //    SqlHelper.NVarChar(cmd, "@r1_obs_just_reagendamento", 500, c.ObsJustReagendamento);

        //    SqlHelper.NVarChar(cmd, "@r1_rg_motorista", 20, c.RGMotorista);

        //    SqlHelper.NVarChar(cmd, "@r1_nome_motorista", 150, c.NomeMotorista);

        //    SqlHelper.NVarChar(cmd, "@r1_num_rota", 30, c.NumRota);

        //    SqlHelper.NVarChar(cmd, "@r1_fluxo", 50, c.Fluxo);

        //    SqlHelper.NVarChar(cmd, "@r1_just_tipo_veiculo", 500, c.JustTipoVeiculo);

        //    SqlHelper.NVarChar(cmd, "@r1_conta", 30, c.Conta);

        //    SqlHelper.NVarChar(cmd, "@r1_setor", 100, c.Setor);

        //    SqlHelper.Decimal(cmd, "@r1_km_total", c.KmTotal);

        //    SqlHelper.NVarChar(cmd, "@r1_num_remessa", 30, c.NumRemessa);

        //    SqlHelper.NVarChar(cmd, "@situacao", 30, c.Situacao);

        //    SqlHelper.NVarChar(cmd, "@tipo", 30, c.Tipo);

        //    SqlHelper.NVarChar(cmd, "@viagem_com_retorno", 10, c.ViagemComRetorno);

        //    SqlHelper.NVarChar(cmd, "@devolucao_peca", 10, c.DevolucaoPeca);

        //    SqlHelper.NVarChar(cmd, "@cod_remetente", 30, c.CodRemetente);

        //    SqlHelper.NVarChar(cmd, "@remetente", 150, c.Remetente);

        //    SqlHelper.NVarChar(cmd, "@cnpj_remetente", 20, c.CNPJRemetente);

        //    SqlHelper.NVarChar(cmd, "@cid_remetente", 100, c.CidadeRemetente);

        //    SqlHelper.NVarChar(cmd, "@uf_remetente", 2, c.UFRemetente);

        //    SqlHelper.NVarChar(cmd, "@cod_expedidor", 30, c.CodExpedidor);

        //    SqlHelper.NVarChar(cmd, "@expedidor", 150, c.Expedidor);

        //    SqlHelper.NVarChar(cmd, "@cnpj_expedidor", 20, c.CNPJExpedidor);

        //    SqlHelper.NVarChar(cmd, "@cid_expedidor", 100, c.CidadeExpedidor);

        //    SqlHelper.NVarChar(cmd, "@uf_expedidor", 2, c.UFExpedidor);

        //    SqlHelper.NVarChar(cmd, "@cod_destinatario", 30, c.CodDestinatario);

        //    SqlHelper.NVarChar(cmd, "@destinatario", 150, c.Destinatario);

        //    SqlHelper.NVarChar(cmd, "@cnpj_destinatario", 20, c.CNPJDestinatario);

        //    SqlHelper.NVarChar(cmd, "@cid_destinatario", 100, c.CidadeDestinatario);

        //    SqlHelper.NVarChar(cmd, "@uf_destinatario", 2, c.UFDestinatario);

        //    SqlHelper.NVarChar(cmd, "@cod_recebedor", 30, c.CodRecebedor);

        //    SqlHelper.NVarChar(cmd, "@recebedor", 150, c.Recebedor);

        //    SqlHelper.NVarChar(cmd, "@cnpj_recebedor", 20, c.CNPJRecebedor);

        //    SqlHelper.NVarChar(cmd, "@cid_recebedor", 100, c.CidadeRecebedor);

        //    SqlHelper.NVarChar(cmd, "@uf_recebedor", 2, c.UFRecebedor);

        //    SqlHelper.NVarChar(cmd, "@cod_pagador", 30, c.CodPagador);

        //    SqlHelper.NVarChar(cmd, "@pagador", 150, c.Pagador);

        //    SqlHelper.NVarChar(cmd, "@cnpj_pagador", 20, c.CNPJPagador);

        //    SqlHelper.NVarChar(cmd, "@cid_pagador", 100, c.CidadePagador);

        //    SqlHelper.NVarChar(cmd, "@uf_pagador", 2, c.UFPagador);

        //    SqlHelper.NVarChar(cmd, "@tipo_cva", 50, c.TipoCVA);

        //    SqlHelper.NVarChar(cmd, "@estabelecimento", 50, c.Estabelecimento);

        //    SqlHelper.NVarChar(cmd, "@cnpj_recebedor1", 20, c.CNPJRecebedor1);

        //    SqlHelper.NVarChar(cmd, "@tipo_viagem2", 100, c.TipoViagemCVA);

        //    SqlHelper.NVarChar(cmd, "@tipo_geracao", 50, c.TipoGeracao);

        //    SqlHelper.NVarChar(cmd, "@conta", 30, c.ContaCVA);

        //    SqlHelper.NVarChar(cmd, "@centro_custo", 30, c.CentroCusto);

        //    SqlHelper.Date(cmd, "@data_solicitacao", c.DataSolicitacao);

        //    SqlHelper.Date(cmd, "@data_entrega", c.DataEntrega);

        //    SqlHelper.NVarChar(cmd, "@tipo_solicitacao", 100, c.TipoSolicitacao);

        //    SqlHelper.NVarChar(cmd, "@tipo_veiculo2", 100, c.TipoVeiculoCVA);

        //    SqlHelper.NVarChar(cmd, "@cap_veiculo", 30, c.CapVeiculo);

        //    SqlHelper.NVarChar(cmd, "@cod_motorista", 20, c.CodMotorista);

        //    SqlHelper.NVarChar(cmd, "@transp_motorista", 150, c.TranspMotorista);

        //    SqlHelper.NVarChar(cmd, "@transp_veiculo", 150, c.TranspVeiculo);

        //    SqlHelper.NVarChar(cmd, "@transp_reboque", 150, c.TranspReboque);

        //    SqlHelper.NVarChar(cmd, "@reboque1", 20, c.Reboque1);

        //    SqlHelper.NVarChar(cmd, "@reboque2", 20, c.Reboque2);

        //    SqlHelper.NVarChar(cmd, "@resp_abertura", 100, c.ResponsavelAbertura);

        //}
        //private CVA LerTela()
        //{
        //    CVA c = new CVA();

        //    //=========================
        //    // Dados R1
        //    //=========================

        //    c.TipoRegistro = "01";

        //    c.CodPlanta = SafeInt(txtCodPlanta.Text);

        //    c.CodTransportadora = txtCodTransportadora.Text.Trim();

        //    c.Placa = txtPlacaCVA.Text.Trim();

        //    c.CPFMotorista = txtCPFCVA.Text.Trim();

        //    c.PrevChegGate = SafeDate(txtPrevChegadaGate.Text);

        //    c.TipoVeiculo = txtTipoVeiculoCVA.Text.Trim();

        //    c.TipoViagem = txtTipoViagemCVA.Text.Trim();

        //    c.CodTipoSolicitacao = txtCodTipoSolicitacao.Text.Trim();

        //    c.NumeroSolicitacao = txtNumeroSolicitacao.Text.Trim();

        //    c.TipoGeracaoCVA = txtTipoGeracaoCVA.Text.Trim();

        //    c.CodJustReagendamento =
        //        txtCodJustificativa.Text.Trim();

        //    c.ObsJustReagendamento =
        //        txtObsJustificativa.Text.Trim();

        //    c.RGMotorista = txtRGCVA.Text.Trim();

        //    c.NomeMotorista = txtMotoristaCVA.Text.Trim();

        //    c.NumRota = txtRota.Text.Trim();

        //    c.Fluxo = txtFluxo.Text.Trim();

        //    c.JustTipoVeiculo =
        //        txtJustificaVeiculoCVA.Text.Trim();

        //    c.Conta = txtContaCVA.Text.Trim();

        //    c.Setor = txtSetor.Text.Trim();

        //    c.KmTotal = SafeDecimal(txtKmTotal.Text);

        //    c.NumRemessa = txtRemessa.Text.Trim();

        //    c.CodRemetente = txtCodigoRemetenteCVA.Text.Trim();

        //    c.Remetente = txtRemetenteCVA.Text.Trim();

        //    c.CNPJRemetente = txtCNPJRemetente.Text.Trim();

        //    c.CidadeRemetente = txtLocalColetaCVA.Text.Trim();

        //    c.UFRemetente = txtUFColetaCVA.Text.Trim();

        //    c.CodExpedidor = txtCodigoExpedidorCVA.Text.Trim();

        //    c.Expedidor = txtExpedidorCVA.Text.Trim();

        //    c.CNPJExpedidor = txtCNPJExpedidor.Text.Trim();

        //    c.CidadeExpedidor = txtCidadeExpedidor.Text.Trim();

        //    c.UFExpedidor = txtUFExpedidor.Text.Trim();

        //    c.CodDestinatario = txtCodigoDestCVA.Text.Trim();

        //    c.Destinatario =
        //        txtDestCVA.Text.Trim();

        //    c.CNPJDestinatario =
        //        txtCNPJDestinatario.Text.Trim();

        //    c.CidadeDestinatario =
        //        txtCidadeDestino.Text.Trim();

        //    c.UFDestinatario =
        //        txtUFDestino.Text.Trim();

        //    c.CodRecebedor = txtCodigoRecCVA.Text.Trim();

        //    c.Recebedor =
        //        txtRecCVA.Text.Trim();

        //    c.CNPJRecebedor =
        //        txtCNPJRecebedor.Text.Trim();

        //    c.CidadeRecebedor =
        //        txtLocalEntregaCVA.Text.Trim();

        //    c.UFRecebedor =
        //        txtUFEntregaCVA.Text.Trim();

        //    c.TipoCVA = ddlTipoCVA.SelectedValue;

        //    c.Estabelecimento =
        //        ddlEstabelecimentoCVA.SelectedValue;

        //    c.TipoViagemCVA =
        //        txtTipoViagemCVA.Text.Trim();

        //    c.TipoGeracao =
        //        txtTipoGeracaoCVA.Text.Trim();

        //    c.ContaCVA =
        //        txtContaCVA.Text.Trim();

        //    c.CentroCusto =
        //        txtCentroCustoCVA.Text.Trim();

        //    c.DataSolicitacao =
        //        SafeDate(txtDataSolicitacaoCVA.Text);

        //    c.DataEntrega =
        //        SafeDate(txtDataHoraEntregaCVA.Text);

        //    c.TipoSolicitacao =
        //        txtTipoSolicitacaoCVA.Text.Trim();

        //    c.TipoVeiculoCVA =
        //        txtTipoVeiculoCVA.Text.Trim();

        //    c.CapVeiculo =
        //        txtCapVeiculoCVA.Text.Trim();

        //    c.CodMotorista = txtCodigoMotorista.Text.Trim();

        //    c.TranspMotorista =
        //        txtTranspMotoristaCVA.Text.Trim();

        //    c.TranspVeiculo =
        //        txtProprietarioVeiculoCVA.Text.Trim();

        //    c.TranspReboque =
        //        txtPropReb1.Text.Trim();

        //    c.Reboque1 =
        //        txtReboque1CVA.Text.Trim();

        //    c.Reboque2 =
        //        txtReboque2CVA.Text.Trim();

        //    c.ResponsavelAbertura =
        //        Session["usuario"]?.ToString();

        //    return c;
        //}
        //private int SafeInt(string valor)
        //{
        //    int.TryParse(valor, out int n);
        //    return n;
        //}

        //private decimal? SafeDecimal(string valor)
        //{
        //    if (decimal.TryParse(valor, out decimal n))
        //        return n;

        //    return null;
        //}

        //private DateTime? SafeDate(string valor)
        //{
        //    if (DateTime.TryParse(valor, out DateTime d))
        //        return d;

        //    return null;
        //}

        //protected void btnSalvar_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        string numero = GerarCVA();

        //        Mensagem("CVA " + numero + " gerada com sucesso.");

        //        CarregarCVA(numero);
        //    }
        //    catch (Exception ex)
        //    {
        //        Mensagem(ex.Message);
        //    }
        //}
        //private bool ValidarTela()
        //{
        //    if (string.IsNullOrWhiteSpace(txtNumeroSolicitacao.Text))
        //    {
        //        Mensagem("Informe a Solicitação.");
        //        txtNumeroSolicitacao.Focus();
        //        return false;
        //    }

        //    if (string.IsNullOrWhiteSpace(txtTipoCVA.Text))
        //    {
        //        Mensagem("Informe o Tipo da CVA.");
        //        txtTipoCVA.Focus();
        //        return false;
        //    }

        //    if (string.IsNullOrWhiteSpace(txtPlacaCVA.Text))
        //    {
        //        Mensagem("Informe a Placa.");
        //        txtPlacaCVA.Focus();
        //        return false;
        //    }

        //    if (string.IsNullOrWhiteSpace(txtMotoristaCVA.Text))
        //    {
        //        Mensagem("Informe o Motorista.");
        //        txtMotoristaCVA.Focus();
        //        return false;
        //    }

        //    if (string.IsNullOrWhiteSpace(txtCodigoFornecedor.Text))
        //    {
        //        Mensagem("Informe o Fornecedor.");
        //        txtCodigoFornecedor.Focus();
        //        return false;
        //    }

        //    return true;
        //}

        //public class CVAGravacao
        //{
        //    public CVA Cabecalho { get; set; }

        //    public List<CVAFornecedor> Fornecedores { get; set; }

        //    public List<CVAProduto> Produtos { get; set; }

        //    public List<CVAEmbalagem> Embalagens { get; set; }

        //    public List<CVAQuantidade> Quantidades { get; set; }
        //}

        //public class CVAParametros
        //{
        //    //Solicitação
        //    public string NumeroSolicitacao { get; set; }

        //    //Cabeçalho
        //    public string TipoCVA { get; set; }
        //    public string Estabelecimento { get; set; }
        //    public string TipoViagem { get; set; }
        //    public string TipoGeracao { get; set; }
        //    public string Conta { get; set; }
        //    public string CentroCusto { get; set; }

        //    public string ViagemComRetorno { get; set; }
        //    public string DevolucaoPeca { get; set; }

        //    //Remetente
        //    public string CodRemetente { get; set; }
        //    public string Remetente { get; set; }

        //    //Expedidor
        //    public string CodExpedidor { get; set; }
        //    public string Expedidor { get; set; }

        //    //Destinatário
        //    public string CodDestinatario { get; set; }
        //    public string Destinatario { get; set; }

        //    //Recebedor
        //    public string CodRecebedor { get; set; }
        //    public string Recebedor { get; set; }

        //    public string CidadeColeta { get; set; }
        //    public string UFColeta { get; set; }

        //    public string CidadeEntrega { get; set; }
        //    public string UFEntrega { get; set; }

        //    public DateTime? DataEntrega { get; set; }

        //    public string TipoSolicitacao { get; set; }

        //    public string TipoVeiculo { get; set; }

        //    public string JustificativaVeiculo { get; set; }

        //    public string Placa { get; set; }

        //    public string Veiculo { get; set; }

        //    public decimal? Capacidade { get; set; }

        //    public string ProprietarioVeiculo { get; set; }

        //    public string Reboque1 { get; set; }

        //    public string ProprietarioReboque1 { get; set; }

        //    public string Reboque2 { get; set; }

        //    public string ProprietarioReboque2 { get; set; }

        //    public string Motorista { get; set; }

        //    public string CPF { get; set; }

        //    public string RG { get; set; }

        //    public string Transportadora { get; set; }

        //    //Fornecedor
        //    public string CodFornecedor { get; set; }

        //    public DateTime? PrevChegada { get; set; }

        //    public DateTime? PrevSaida { get; set; }

        //    public int TipoOperacao { get; set; }

        //    public string ColetaEntrega { get; set; }

        //    public string Usuario { get; set; }
        //}

        //private CVAParametros LerTela()
        //{
        //    CVAParametros c = new CVAParametros();

        //    // Dados principais CVA
        //    c.Id = string.IsNullOrEmpty(hfIdCVA.Value)
        //        ? 0
        //        : Convert.ToInt32(hfIdCVA.Value);

        //    c.TipoRegistro = txtTipoRegistroCVA.Text.Trim();
        //    c.Numero = txtNumeroCVA.Text.Trim();
        //    c.NumeroCVA = txtNumeroCVA2.Text.Trim();

        //    c.TipoGeracao = ddlTipoGeracaoCVA.SelectedValue;
        //    c.Tipo = ddlTipoCVA.SelectedValue;


        //    // Dados do solicitante
        //    c.Solicitante = txtSolicitanteCVA.Text.Trim();
        //    c.DataSolicitacao = SafeDate(txtDataSolicitacaoCVA.Text);


        //    // Dados de viagem
        //    c.TipoViagem = txtTipoViagemCVA.Text.Trim();

        //    c.Origem = txtOrigemCVA.Text.Trim();
        //    c.Destino = txtDestinoCVA.Text.Trim();

        //    c.CidadeOrigem = txtCidadeOrigemCVA.Text.Trim();
        //    c.UfOrigem = ddlUfOrigemCVA.SelectedValue;

        //    c.CidadeDestino = txtCidadeDestinoCVA.Text.Trim();
        //    c.UfDestino = ddlUfDestinoCVA.SelectedValue;


        //    // Veículo
        //    c.Placa = txtPlacaCVA.Text.Trim();
        //    c.Reboque1 = txtReboque1CVA.Text.Trim();
        //    c.Reboque2 = txtReboque2CVA.Text.Trim();

        //    c.Motorista = txtMotoristaCVA.Text.Trim();
        //    c.CodMotorista = SafeInt(txtCodMotoristaCVA.Text);


        //    // Valores
        //    c.Peso = SafeDecimal(txtPesoCVA.Text);

        //    c.Observacao = txtObservacaoCVA.Text.Trim();


        //    // Status
        //    c.Status = ddlStatusCVA.SelectedValue;


        //    return c;
        //}


    }
}







