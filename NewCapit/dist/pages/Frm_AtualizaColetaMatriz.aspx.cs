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
using GMaps;
using GMaps.Classes;
using Subgurim;
using Subgurim.Controles;
using Subgurim.Controls;
using Subgurim.Maps;
using Subgurim.Web;
using System.Globalization;
using DocumentFormat.OpenXml.Bibliography;
using MathNet.Numerics;
using NPOI.SS.Formula;
using System.Drawing;
using System.Numerics;
using System.Web.Script.Serialization;
using System.Security.Principal;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Text;
using System.Web.Services;
using DocumentFormat.OpenXml.Office.Word;
using OfficeOpenXml.Drawing.Chart;
using AjaxControlToolkit.Design;



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

        BigInteger idveiculo;
        string cidade, empresa, lat, lon, ignicao, bairro, rua, uf, id, placa, hora, velocidade,preferencia,bloqueio;

        GInfoWindow window;
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
                    Response.Redirect("Login.aspx");

                }
                DateTime dataHoraAtual = DateTime.Now;
                lblAtualizadoEm.Text = dataHoraAtual.ToString("dd/MM/yyyy HH:mm");
                fotoMotorista = "/fotos/motoristasemfoto.jpg";
                PreencherComboMotoristas();
                CarregaDados();
                CarregaMap(txtPlaca.Text);

                PreencherClienteInicial();
                PreencherClienteFinal();

            }
            //CarregarFotoMotorista(fotoMotorista);
            CarregaFoto();
            //VerificaCargasFechadas();
            
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
            DataTable dt = new DataTable();
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

            string idviagem;
            idviagem = num_coleta;
            CarregarColetas(idviagem);
            
            //GetPedidos();


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
                        ddlStatus.DataValueField = "ds_status";
                        ddlStatus.DataBind();
                    }
                    var drv = (HiddenField)e.Item.FindControl("hdfStatus"); ;
                    string statusDaColeta = drv.Value;  // o nome da coluna do seu DataTable
                    // opcional: insere item em branco no topo
                    ddlStatus.Items.Insert(0, new System.Web.UI.WebControls.ListItem(statusDaColeta, "0"));
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
                // ... (suas declarações de variáveis iniciais) ...
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

                    // Lógica para "Atrasado"
                    if (dataHoraComparacao < agora)
                    {
                        lblAtendimento.Text = "Atrasado";
                        // CORREÇÃO AQUI: Mova o BgColor para dentro do style
                        tdAtendimento.Attributes["style"] = "background-color: Red; color: white; font-weight: bold;";
                    }
                    // Lógica para "No Prazo"
                    else if (dataHoraComparacao.Date == agora.Date && dataHoraComparacao.TimeOfDay <= agora.TimeOfDay)
                    {
                        lblAtendimento.Text = "No Prazo";
                        // CORREÇÃO AQUI: Mova o BgColor para dentro do style
                        tdAtendimento.Attributes["style"] = "background-color: Green; color: white; font-weight: bold;";
                    }
                    // Lógica para "Antecipado"
                    else if (dataHoraComparacao > agora)
                    {
                        lblAtendimento.Text = "Antecipado";
                        // CORREÇÃO AQUI: Mova o BgColor para dentro do style
                        tdAtendimento.Attributes["style"] = "background-color: Orange; color: white; font-weight: bold;";
                    }
                    else
                    {
                        lblAtendimento.Text = status;
                        // Opcional: Limpar estilos se não cair em nenhuma condição
                        tdAtendimento.Attributes["style"] = "";
                    }
                }
            }
            if (e.Item.ItemType == ListItemType.Item ||
         e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HiddenField hdIdCarga =
                    (HiddenField)e.Item.FindControl("hdIdCarga");

                GridView gvPedidos =
                    (GridView)e.Item.FindControl("gvPedidos");

                UpdatePanel upd =
                    (UpdatePanel)e.Item.FindControl("updTabs");

                if (hdIdCarga != null && gvPedidos != null)
                {
                    int idCarga;
                    if (int.TryParse(hdIdCarga.Value, out idCarga))
                    {
                        CarregarPedidos(idCarga, gvPedidos);

                        // força renderização do conteúdo
                        //upd.Update();
                    }
                }
            }

            if (e.Item.ItemType == ListItemType.Item ||
        e.Item.ItemType == ListItemType.AlternatingItem)
            {
                // 🔎 Pega o status
                HiddenField hdfStatus = (HiddenField)e.Item.FindControl("hdfStatus");

                if (hdfStatus != null &&
                    hdfStatus.Value.Equals("Concluido", StringComparison.OrdinalIgnoreCase))
                {
                    // 🔘 Botões
                    Button btnAtualizar = (Button)e.Item.FindControl("btnAtualizarColeta");
                    Button btnWhats = (Button)e.Item.FindControl("WhatsApp");
                    Button btnOrdem = (Button)e.Item.FindControl("btnOrdemColeta");

                    if (btnAtualizar != null)
                    {
                        btnAtualizar.Enabled = false;
                        btnAtualizar.CssClass += " disabled";
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
            }

            if (e.Item.ItemType == ListItemType.Item ||
        e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DropDownList ddlRotaKrona = (DropDownList)e.Item.FindControl("ddlRotaKrona");

                using (SqlConnection conn = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
                {
                    string sqlr = @"SELECT id_rota, descricao_rota 
                           FROM tbrotaskrona 
                           ORDER BY descricao_rota";

                    using (SqlCommand cmd = new SqlCommand(sqlr, conn))
                    {
                        conn.Open();
                        ddlRotaKrona.DataSource = cmd.ExecuteReader();
                        ddlRotaKrona.DataTextField = "descricao_rota";
                        ddlRotaKrona.DataValueField = "id_rota";
                        ddlRotaKrona.DataBind();
                    }
                }

                ddlRotaKrona.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Selecione a rota --", ""));
            }


            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string idViagem = DataBinder.Eval(e.Item.DataItem, "carga").ToString();
                GridView gv = (GridView)e.Item.FindControl("gvCte");
                int index = e.Item.ItemIndex;

                if (gv != null)
                {
                    CarregarGridCte(gv, idViagem, index);
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

        protected void rptColetas_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            GridView gv = (GridView)e.Item.FindControl("gvPedidos");

            if (e.CommandName == "Atualizar")
            {
                string carga = e.CommandArgument.ToString();
                //GridView gv = (GridView)e.Item.FindControl("gvPedidos");

                // Recuperar os controles de dentro do item
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

                TextBox txtCodExpedidor = (TextBox)e.Item.FindControl("txtCodExpedidor");
                TextBox cboExpedidor = (TextBox)e.Item.FindControl("cboExpedidor");
                TextBox txtCidExpedidor = (TextBox)e.Item.FindControl("txtCidExpedidor");
                TextBox txtUFExpedidor = (TextBox)e.Item.FindControl("txtUFExpedidor");

                TextBox txtCodRecebedor = (TextBox)e.Item.FindControl("txtCodRecebedor");
                TextBox cboRecebedor = (TextBox)e.Item.FindControl("cboRecebedor");
                TextBox txtCidRecebedor = (TextBox)e.Item.FindControl("txtCidRecebedor");
                TextBox txtUFRecebedor = (TextBox)e.Item.FindControl("txtUFRecebedor");

                
                // Exemplo: atualizando no banco
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

                    if (chegada != null && saida != null && saidaPlanta != null)
                    {
                        statusOC = "Concluido";
                        situacaoOC = "EM ANDAMENTO";
                        andamentoCarga = "Em Andamento";
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
                        //statusOC = ddlStatus.SelectedItem.Text.Trim();

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
                                tempoagdescarreg=@tempoagdescarreg,
                                duracao_transp=@duracao_transp,
                                disponivel_solicitacao = @disponivel_solicitacao,
                                codmot=@codmot, 
                                frota=@frota
                                WHERE carga = @carga";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@carga", carga);                    
                    cmd.Parameters.AddWithValue("@status", ddlStatus.SelectedItem.Text);
                    cmd.Parameters.AddWithValue("@andamento", andamentoCarga);
                    cmd.Parameters.AddWithValue("@cva", txtCVA.Text.Trim());
                    cmd.Parameters.AddWithValue("@saidaorigem", SafeDateValue(txtSaidaOrigem.Text.Trim()));
                    cmd.Parameters.AddWithValue("@tempoagcarreg", SafeValue(txtAgCarreg.Text.Trim()));
                    cmd.Parameters.AddWithValue("@duracao_transp", SafeValue(txtDurTransp.Text.Trim()));
                    cmd.Parameters.AddWithValue("@tempoagdescarreg", SafeValue(txtAgDescarga.Text.Trim()));
                    cmd.Parameters.AddWithValue("@chegadadestino", SafeDateValue(txtChegadaDestino.Text.Trim()));
                    cmd.Parameters.AddWithValue("@disponivel_solicitacao", SafeDateValue(txtVeiculoDisponivel.Text.Trim()));
                    cmd.Parameters.AddWithValue("@prev_chegada", SafeDateValue(txtPrevisaoChegada.Text.Trim()));
                    cmd.Parameters.AddWithValue("@saidaplanta", SafeDateValue(txtSaidaPlanta.Text.Trim())); 
                    cmd.Parameters.AddWithValue("@codmot", txtCodMotorista.Text.Trim() ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@frota", txtCodFrota.Text.Trim() ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@gate_origem", SafeDateValue(txtGateOrigem.Text.Trim()) ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@gate_destino", SafeDateValue(txtGateDestino.Text.Trim()) ?? (object)DBNull.Value);                   

                    // Chama método que verifica no banco
                    conn.Open();
                    cmd.ExecuteNonQuery();
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
            uf_recebedor = @uf_recebedor
        WHERE num_carregamento = @num_carregamento";

                    using (SqlCommand cmdCarregamento = new SqlCommand(queryCarregamento, conn))
                    {
                        // Num carregamento (obrigatório)
                        cmdCarregamento.Parameters.Add("@num_carregamento", SqlDbType.NVarChar, 20)
                            .Value = novaColeta.Text.Trim();

                        // Status
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
                        cmdCarregamento.Parameters.AddWithValue("@dtsaida", SafeDateValue(txtSaidaOrigem.Text.Trim()));
                        cmdCarregamento.Parameters.AddWithValue("@dtchegada", SafeDateValue(txtChegadaDestino.Text.Trim()));
                        cmdCarregamento.Parameters.AddWithValue("@dtconclusao", SafeDateValue(txtSaidaPlanta.Text.Trim()));

                        cmdCarregamento.Parameters.Add("@uf_recebedor", SqlDbType.NVarChar, 2)
                            .Value = string.IsNullOrWhiteSpace(txtUFRecebedor.Text)
                                ? (object)DBNull.Value
                                : txtUFRecebedor.Text.Trim();

                        conn.Open();
                        cmdCarregamento.ExecuteNonQuery();
                    }
                }


                
               

                   

                // Após atualizar, recarregar os dados no Repeater
                ViewState["Coletas"] = null;
                CarregarColetas(novaColeta.Text);
                //BuscarCteSalvos(idViagem);
                CarregarPedidos(int.Parse(carga), gv);
            }
           
            if (e.CommandName == "AtualizarAbas")
            {
                //Atualiza CT-e
                string carga = e.CommandArgument.ToString();
                string idViagem = e.CommandArgument.ToString(); // O 'carga' que você passou no Eval
                int index = e.Item.ItemIndex; // O índice da linha no Repeater
               


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
                                    cmd.Parameters.AddWithValue("@emitido_por", txtUsuCadastro.Text);
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
                            MostrarMsg2("ERRO REAL: " + erroLimpo);
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
                                    // Conversão segura para DateTime (DateTimeLocal envia yyyy-MM-ddTHH:mm)
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

                TextBox txtHistoricoObservacao = (TextBox)e.Item.FindControl("txtHistoricoObservacao");

                if (txtHistoricoObservacao.Text != string.Empty)
                {
                    try
                    {
                        if (con.State == ConnectionState.Closed) con.Open();
                        SqlTransaction trans = con.BeginTransaction();

                        try
                        {

                            // 3. Query de Update
                            string sql = @"UPDATE tbcargas SET 
                                       observacao = @observacao 
                                       WHERE carga = @carga ";

                            using (SqlCommand cmd = new SqlCommand(sql, con, trans))
                            {

                                cmd.Parameters.AddWithValue("@observacao", txtHistoricoObservacao.Text);
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
                        MostrarMsg("Erro ao salvar Observacao: " + ex.Message);
                    }
                    finally
                    {
                        con.Close();
                    }
                }

                TextBox txtMDFe = (TextBox)e.Item.FindControl("txtMDFe");

                if (txtMDFe.Text != string.Empty)
                {
                    try
                    {
                        if (con.State == ConnectionState.Closed) con.Open();
                        SqlTransaction trans = con.BeginTransaction();

                        try
                        {

                            // 3. Query de Update
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

               
                ViewState["Coletas"] = null;
                CarregarColetas(novaColeta.Text);
                BuscarCteSalvos(idViagem);
                CarregarPedidos(int.Parse(carga), gv);
            }



            // Após atualizar, recarregar os dados no Repeater
            
            //if (e.CommandName == "Ocorrencias")
            //{
            //    int id = Convert.ToInt32(e.CommandArgument);

            //    string connStr = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
            //    using (SqlConnection conn = new SqlConnection(connStr))
            //    {
            //        conn.Open();

            //        SqlCommand cmd1 = new SqlCommand("SELECT carga,cva, andamento FROM tbcargas WHERE carga = @Id", conn);
            //        cmd1.Parameters.AddWithValue("@Id", id);
            //        SqlDataReader reader1 = cmd1.ExecuteReader();
            //        if (reader1.Read())
            //        {
            //            lblCVA.BackColor = System.Drawing.Color.Magenta;
            //            lblCVA.ForeColor = System.Drawing.Color.White;
            //            lblCVA.Text = reader1["cva"].ToString();

            //            lblColeta.BackColor = System.Drawing.Color.LightGreen;
            //            lblColeta.Text = reader1["carga"].ToString();

            //            if (reader1["andamento"].ToString() == "CONCLUIDO")
            //            {
            //                lblStatus.BackColor = System.Drawing.Color.LightGreen;
            //                lblStatus.Text = reader1["andamento"].ToString();
            //            }
            //            else if (reader1["andamento"].ToString() == "PENDENTE" || reader1["andamento"].ToString() == "Pendente")
            //            {
            //                lblStatus.BackColor = System.Drawing.Color.Yellow;
            //                lblStatus.Text = reader1["andamento"].ToString();
            //            }
            //            else if (reader1["andamento"].ToString() == "EM ANDAMENTO")
            //            {
            //                lblStatus.BackColor = System.Drawing.Color.Purple;
            //                lblCVA.ForeColor = System.Drawing.Color.White;
            //                lblStatus.Text = reader1["andamento"].ToString();
            //            }
            //            else
            //            {
            //                lblStatus.BackColor = System.Drawing.Color.Black;
            //                lblStatus.Text = reader1["andamento"].ToString();
            //            }

            //            using (SqlConnection con = new SqlConnection(connStr))
            //            {
            //                string query = "SELECT id, responsavel, motivo, observacao, data_inclusao, usuario_inclusao FROM tbocorrencias WHERE carga = @numeroCarga";

            //                SqlCommand cmd = new SqlCommand(query, con);
            //                cmd.Parameters.AddWithValue("@numeroCarga", id);

            //                SqlDataAdapter da = new SqlDataAdapter(cmd);
            //                DataTable dt = new DataTable();
            //                da.Fill(dt);

            //                GridViewCarga.DataSource = dt;
            //                GridViewCarga.DataBind();
            //            }


            //            // Exibe o modal com JavaScript
            //            ScriptManager.RegisterStartupScript(this, this.GetType(), "abrirModal", "$('#modalOcorrencia').modal('show');", true);

            //        }
            //    }
            //}
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
                MostrarMsg2("Dados Salvos!");
                ViewState["Coletas"] = null;
                CarregarColetas(novaColeta.Text);
            }
        }

        protected void ddlMotorista_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtCodMotorista.Text = ddlMotorista.SelectedValue;


            using (SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                string sql = "SELECT * FROM tbveiculos WHERE codmot = @codmot";
                con.Open();

                SqlDataAdapter da = new SqlDataAdapter(sql, con);
                da.SelectCommand.Parameters.AddWithValue("@codmot", txtCodMotorista.Text);

                DataTable dt2 = new DataTable();
                da.Fill(dt2);

                if (dt2.Rows.Count > 0)
                {
                    // Registra o script de confirmação no lado do cliente
                    string script = "ConfirmMessage();";
                    ClientScript.RegisterStartupScript(this.GetType(), "ConfirmMessage", script, true);

                    // Verifica a resposta do usuário via txtconformmessageValue
                    if (txtconformmessageValue.Value == "Yes")
                    {
                        string updateSql = "UPDATE tbveiculos SET codmot = NULL, motorista = NULL WHERE id = @id";
                        using (SqlCommand cmd = new SqlCommand(updateSql, con))
                        {
                            cmd.Parameters.AddWithValue("@id", dt2.Rows[0][0].ToString());

                            int rowsAffected = cmd.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                // Exibe mensagem de sucesso
                                //string nomeUsuario = txtAlteradoPor.Text;
                                string mensagem = $"Código {txtCodMotorista.Text}, foi desvinculado do veículo com sucesso.";
                                string mensagemCodificada = HttpUtility.JavaScriptStringEncode(mensagem);
                                string successScript = $"alert('{mensagemCodificada}');";
                                ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeSucesso", successScript, true);
                            }
                            else
                            {
                                // Log ou mensagem indicando falha na atualização
                                string failScript = "alert('Falha ao desvincular o veículo.');";
                                ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeErro", failScript, true);
                            }
                        }
                    }
                    else
                    {
                        //ddlMotorista.ClearSelection();
                        //txtCodMotorista.Text = string.Empty;
                        txtCodVeiculo.Focus();
                    }



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
                //try
                //{
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

                //ddlMotorista.Items.Insert(0, "");

                // Feche o reader
                reader.Close();
                //}
                //catch (Exception ex)
                //{
                //    // Trate exceções
                //    Response.Write("Erro: " + ex.Message);
                //}
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
                cmd.Parameters.AddWithValue("@valtoxicologico", SafeDateValue(txtExameToxic.Text));
                cmd.Parameters.AddWithValue("@venccnh", SafeDateCNH(txtCNH.Text));
                cmd.Parameters.AddWithValue("@valgr", SafeDateValue(txtLibGR.Text));
                cmd.Parameters.AddWithValue("@foto", SafeValue(fotoMotorista)); // Se for byte[], troque tipo do parâmetro!
                cmd.Parameters.AddWithValue("@nomemotorista", SafeValue(ddlMotorista.SelectedItem.Text));
                cmd.Parameters.AddWithValue("@cpf", SafeValue(txtCPF.Text));
                cmd.Parameters.AddWithValue("@cartaopedagio", SafeValue(txtCartao.Text));
                cmd.Parameters.AddWithValue("@valcartao", txtValCartao.Text);
                cmd.Parameters.AddWithValue("@foneparticular", SafeValue(txtCelular.Text));
                cmd.Parameters.AddWithValue("@veiculo", SafeValue(txtCodVeiculo.Text));
                cmd.Parameters.AddWithValue("@veiculotipo", SafeValue(txtVeiculoTipo.Text));
                cmd.Parameters.AddWithValue("@valcet", SafeDateValue(txtCET.Text));
                cmd.Parameters.AddWithValue("@valcrlvveiculo", SafeDateValue(txtCRLVVeiculo.Text));
                cmd.Parameters.AddWithValue("@valcrlvreboque1", SafeDateValue(txtCRLVReb1.Text));
                cmd.Parameters.AddWithValue("@valcrlvreboque2", SafeDateValue(txtCRLVReb2.Text));
                cmd.Parameters.AddWithValue("@valopacidade", SafeDateValue(txtOpacidade.Text));
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
                cmd.Parameters.AddWithValue("@usualt", nomeUsuario);


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
                    ViewState["Coletas"] = null;
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
                // Alteramos a query para nos dizer EXATAMENTE o que está errado
                string sql = @"
            SELECT 
                COUNT(CASE WHEN status <> 'Concluido' THEN 1 END) as NaoConcluidas,
                COUNT(CASE WHEN ISNULL(NULLIF(TRIM(material), ''), '') <> '' AND cte.idcte IS NULL THEN 1 END) as SemCTe
            FROM tbcargas c
            LEFT JOIN tbcte cte ON c.idcarga = cte.idcarga 
            WHERE c.idviagem = @idviagem";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@idviagem", novaColeta.Text);
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int naoConcluidas = Convert.ToInt32(reader["NaoConcluidas"]);
                            int semCTe = Convert.ToInt32(reader["SemCTe"]);

                            if (naoConcluidas > 0)
                                return $"Existem {naoConcluidas} carga(s) que ainda não foram concluídas.";

                            if (semCTe > 0)
                                return "Não é possível encerrar: existem cargas com material informado que não possuem CT-e anexado.";
                        }
                    }
                }
            }
            return null; // Tudo certo!
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
                                    cmds.Parameters.Add("@frota", SqlDbType.VarChar).Value = txtCodFrota.Text;
                                    cmds.Parameters.Add("@status", SqlDbType.VarChar).Value = "Pendente";
                                    cmds.Parameters.Add("@andamento", SqlDbType.VarChar).Value = "EM ANDAMENTO";
                                    cmds.Parameters.Add("@atendimento", SqlDbType.VarChar).Value = "";
                                    cmds.Parameters.Add("@funcaomot", SqlDbType.VarChar).Value = txtFuncao.Text;
                                    cmds.Parameters.Add("@emissao", SqlDbType.DateTime).Value = DateTime.Now;

                                    cmds.ExecuteNonQuery();
                                }

                                ViewState["Coletas"] = null;
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
        private DateTime? SafeDateValue2(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return null;

            DateTime dt;
            if (DateTime.TryParse(input, out dt))
                return dt;

            return null;
        }
        private object SafeDateValue(string input)
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
        protected void btnFechar_Click(object sender, EventArgs e)
        {
            // Opcional: limpar ou fechar modal
            ClientScript.RegisterStartupScript(this.GetType(), "HideModal", "hideModal();", true);

        }      
        protected void btnSalvarColeta_Click(object sender, EventArgs e)
        {
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
            //double distancia = double.Parse(txtDistancia.Text.Trim());
            string codigoPagadorVazio = txtCod_PagadorVazio.Text.Trim();
            string nomePagadorVazio = txtPagadorVazio.Text.Trim().ToUpper();
            string materialVazio = ddlTipoMaterial.SelectedItem.Text;
            string municipioPagadorVazio = txtCid_PagadorVazio.Text.Trim().ToUpper();
            string ufPagadorVazio = txtUf_PagadorVazio.Text.Trim().ToUpper();
            string nomeCompleto = nomePagadorVazio;
            string DuracaoVazio = txtDuracaoVazio.Text.Trim();
            string primeiroNome = nomeCompleto.Split(' ')[0];
            string connectionString = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO tbcargas (carga, emissao, status, entrega, peso, material, portao, situacao, previsao, codorigem, cliorigem, coddestino, clidestino, ufcliorigem, ufclidestino, cidorigem, ciddestino, empresa, cadastro,  tomador, andamento, cod_expedidor, expedidor, cid_expedidor, uf_expedidor, cod_recebedor, recebedor, cid_recebedor, uf_recebedor, cod_pagador, pagador, cid_pagador, uf_pagador, duracao)" +
                  "VALUES (@Carga, GETDATE(), @status, @entrega, @peso, @material, @portao, @situacao, @previsao, @codorigem, @cliorigem, @coddestino, @clidestino, @ufcliorigem, @ufclidestino, @cidorigem, @ciddestino, @empresa, @cadastro,  @tomador, @andamento, @cod_expedidor, @expedidor, @cid_expedidor, @uf_expedidor, @cod_recebedor, @recebedor, @cid_recebedor, @uf_recebedor, @cod_pagador, @pagador, @cid_pagador, @uf_pagador, @duracao)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@carga", numCarga);
                cmd.Parameters.AddWithValue("@status", "Pendente");
                cmd.Parameters.AddWithValue("@entrega", "Normal");
                cmd.Parameters.AddWithValue("@peso", pesoMaterial); // ou valor padrão
                cmd.Parameters.AddWithValue("@material", materialVazio); // ou valor padrão
                cmd.Parameters.AddWithValue("@portao", codigoDestino); // ou valor padrão
                cmd.Parameters.AddWithValue("@situacao", "Pronto");
                cmd.Parameters.AddWithValue("@tomador", nomeCompleto);
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
                //cmd.Parameters.AddWithValue("@distancia", distancia);
                cmd.Parameters.AddWithValue("@andamento", "PENDENTE");
                cmd.Parameters.AddWithValue("@cod_pagador", codigoPagadorVazio);
                cmd.Parameters.AddWithValue("@pagador", primeiroNome);
                cmd.Parameters.AddWithValue("@cid_pagador", municipioPagadorVazio);
                cmd.Parameters.AddWithValue("@uf_pagador", ufPagadorVazio);
                cmd.Parameters.AddWithValue("@duracao", DuracaoVazio);

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

        void CarregarPedidos(int idCarga)
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                string sql = @"SELECT pedido, emissao, peso, material, portao,
                            iniciocar, termcar
                    FROM tbPedidos
                    WHERE id = @idCarga";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add("@idCarga", SqlDbType.Int).Value = idCarga;

                conn.Open();
                //gvPedidos.DataSource = cmd.ExecuteReader();
                //gvPedidos.DataBind();
            }
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

            // Calcular tempo
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
                         AND distancia2 >= ROUND(@distancia, 0)";

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
                MostrarMsg2("Necessário Anexar Ct-e a Carga para finalizar a Ordem de Coleta!");
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

            string sql = @"SELECT codmot, nommot, status, cargo, nucleo, cpf, venccnh, codliberacao, validade, venceti, cartaomot, tipomot, venccartao, ISNULL(caminhofoto, '/fotos/motoristasemfoto.jpg') AS caminhofoto,fone2, codtra, transp, frota 
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



        protected void ExibirToastErro(string mensagem)
        {
            string script = $@"
            <script>
                document.getElementById('toastMessage3').innerText = '{mensagem.Replace("'", "\\'")}';
                var toastEl = document.getElementById('myToast3');
                var toast = new bootstrap.Toast(toastEl);
                toast.show();
            </script>";

            ClientScript.RegisterStartupScript(this.GetType(), "toastScript", script, false);
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
        private void ShowToastrWarningVeiculo(string message)
        {
            string script = $"showWarningToast('{message}');";
            ClientScript.RegisterStartupScript(this.GetType(), "toastrWarning", script, true);
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
        protected void txtChaveCte_TextChanged(object sender, EventArgs e)
        {
            //35251055890016000109570010001725711001725910
            TextBox txt = (TextBox)sender;
            string chave = txt.Text.Trim();

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
                    MostrarMsg2("Aviso: " + msg);
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
                    string Emissao = chave.Substring(4, 2)+"/" + "20"+chave.Substring(2, 2);

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
                    MostrarMsg2("CNPJ do emissor não encontrado no cadastro de clientes.");

                  

                    // 2. Limpa o campo
                    txt.Text = string.Empty;
                                       

                    txt.Focus();
                }

                //txt.Text = string.Empty;
                //txt.Focus();
            }
            catch (Exception ex)
            {
                MostrarMsg2("Erro ao processar chave: " + ex.Message);
            }
        }
        public void MostrarMsg2(string mensagem)
        {
            // Substitua o alert por um Toastr ou SweetAlert se preferir, mas o RegisterStartupScript é essencial
            string script = $"alert('{mensagem.Replace("'", "")}');";
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", script, true);
        }

        // Função auxiliar para checar o banco
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

                    /* GInfoWindow window2 = new GInfoWindow(marker, latlng1.ToString(), false, GListener.Event.mouseover);
                     GMap1.Add(window2);*/

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

        protected void tmAtualizaMapa_Tick(object sender, EventArgs e)
        {
            if (ViewState["placaAtual"] != null)
            {
                string ds_placa = ViewState["placaAtual"].ToString();
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

    }
}