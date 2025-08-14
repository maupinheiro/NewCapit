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

namespace NewCapit.dist.pages
{
    public partial class Frm_AtualizaOrdemColeta : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conexao"].ToString());
        public string fotoMotorista;
        string codmot, caminhofoto;
        string num_coleta;
        string status;        
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

                PreencherComboMotoristas(); 
                CarregaDados();
                CarregaNumColeta();
                PreencherComboResponsavel();
                

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
        //private void PreencherClienteInicial()
        //{
        //    // Consulta SQL que retorna os dados desejados
        //    string query = "SELECT id, codcli, nomcli FROM tbclientes order by nomcli";

        //    // Crie uma conexão com o banco de dados
        //    using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
        //    {
        //        try
        //        {
        //            // Abra a conexão com o banco de dados
        //            conn.Open();

        //            // Crie o comando SQL
        //            SqlCommand cmd = new SqlCommand(query, conn);

        //            // Execute o comando e obtenha os dados em um DataReader
        //            SqlDataReader reader = cmd.ExecuteReader();

        //            // Preencher o ComboBox com os dados do DataReader
        //            //ddlCliInicial.DataSource = reader;
        //            //ddlCliInicial.DataTextField = "nomcli";  // Campo que será mostrado no ComboBox
        //            //ddlCliInicial.DataValueField = "id";  // Campo que será o valor de cada item                    
        //            //ddlCliInicial.DataBind();  // Realiza o binding dos dados                   
        //            //ddlCliInicial.Items.Insert(0, new ListItem("Selecione...", "0"));
        //            // Feche o reader
        //            reader.Close();
        //        }
        //        catch (Exception ex)
        //        {
        //            // Trate exceções
        //            Response.Write("Erro: " + ex.Message);
        //        }
        //    }
        //}
        //private void PreencherClienteFinal()
        //{
        //    // Consulta SQL que retorna os dados desejados
        //    string query = "SELECT id, codcli, nomcli FROM tbclientes order by nomcli";

        //    // Crie uma conexão com o banco de dados
        //    using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
        //    {
        //        try
        //        {
        //            // Abra a conexão com o banco de dados
        //            conn.Open();

        //            // Crie o comando SQL
        //            SqlCommand cmd = new SqlCommand(query, conn);

        //            // Execute o comando e obtenha os dados em um DataReader
        //            SqlDataReader reader = cmd.ExecuteReader();

        //            // Preencher o ComboBox com os dados do DataReader
        //            //ddlCliFinal.DataSource = reader;
        //            //ddlCliFinal.DataTextField = "nomcli";  // Campo que será mostrado no ComboBox
        //            //ddlCliFinal.DataValueField = "id";  // Campo que será o valor de cada item                    
        //            //ddlCliFinal.DataBind();  // Realiza o binding dos dados                   
        //            //ddlCliFinal.Items.Insert(0, new ListItem("Selecione...", "0"));
        //            // Feche o reader
        //            reader.Close();
        //        }
        //        catch (Exception ex)
        //        {
        //            // Trate exceções
        //            Response.Write("Erro: " + ex.Message);
        //        }
        //    }
        //}
        //protected void ddlCliInicial_TextChanged(object sender, EventArgs e)
        //{
        //    //codCliInicial.Text = ddlCliInicial.SelectedValue;
        //}
        //protected void ddlCliFinal_TextChanged(object sender, EventArgs e)
        //{
        //    //codCliFinal.Text = ddlCliFinal.SelectedValue;


        //}
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
                if (diferencaGR.TotalDays < 30)
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
                    txtCNH.Text = txtCNH.Text + " ("+ diasCNH + " dias)";
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
            ddlMotorista.Items.Insert(0, new ListItem(dt.Rows[0][5].ToString(), ""));
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
                fotoMotorista = "../../fotos/usuario.jpg";
            }

            txtUsuCadastro.Text = dt.Rows[0][76].ToString();
            lblDtCadastro.Text = DateTime.Parse(dt.Rows[0][77].ToString()).ToString("dd/MM/yyyy");
            txtCodFrota.Text = dt.Rows[0][7].ToString();
            txtFoneCorp.Text = dt.Rows[0][9].ToString();
            txtCodVeiculo.Text = dt.Rows[0][15].ToString(); //+ "/" + dt.Rows[0][13].ToString();
            txtFilialVeicCNT.Text = dt.Rows[0][133].ToString();
            txtPlaca.Text = dt.Rows[0][16].ToString();
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
                    if (diferencaLicenciamento.TotalDays < 30 && diferencaLicenciamento.TotalDays >=1)
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

            if(txtTipoVeiculo.Text == "BITRUCK" || txtTipoVeiculo.Text.Trim() == "UTILITÁRIO/FURGÃO" || txtTipoVeiculo.Text.Trim() == "LEVE" || txtTipoVeiculo.Text.Trim() == "FIORINO" || txtTipoVeiculo.Text.Trim() == "TOCO" || txtTipoVeiculo.Text.Trim() == "TRUCK" || txtTipoVeiculo.Text.Trim() == "VUC OU 3/4")
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

            if (txtTipoVeiculo.Text.Trim() == "CAVALO SIMPLES" || txtTipoVeiculo.Text.Trim() == "CAVALO TRUCADO" || txtTipoVeiculo.Text.Trim() == "CAVALO 4 EIXOS" )
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
            string idviagem;
            idviagem = num_coleta;
            CarregarColetas(idviagem);

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
                Label lblMensagem = (Label)e.Item.FindControl("lblMensagem");


                // continue com os demais campos que quiser atualizar...

                // Exemplo: atualizando no banco
                using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
                {
                    // Extrai os valores uma única vez
                    var chegada = SafeDateValue2(txtChegadaOrigem.Text.Trim());
                    var saida = SafeDateValue2(txtSaidaOrigem.Text.Trim());
                    var entrada = SafeDateValue2(txtEntrada.Text.Trim());
                    var saidaPlanta = SafeDateValue2(txtSaidaPlanta.Text.Trim());

                    if (chegada != null && saida != null && entrada != null && saidaPlanta != null)
                    {
                        status = "Concluido";
                    }
                    else if (chegada != null && saida != null && entrada != null)
                    {
                        status = "Ag. Descarga.";
                    }
                    else if (chegada != null && saida != null)
                    {
                        status = "Em Transito";
                    }
                    else if (chegada != null)
                    {
                        status = "Ag. Carreg.";
                    }
                    else
                    {
                        status = "Pendente";
                    }





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

                    cmd.Parameters.AddWithValue("@gate", SafeDateValue(txtGate.Text.Trim()));
                    cmd.Parameters.AddWithValue("@status", status);
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
                    cmd.Parameters.AddWithValue("@cva", txtCVA.Text.Trim());
                    // continue os parâmetros conforme seu banco
                    string valorDigitado = txtCVA.Text.Trim();

                    // Chama método que verifica no banco

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
                        else if (reader1["andamento"].ToString() == "PENDENTE" || reader1["andamento"].ToString() == "Pendente")
                        {
                            lblStatus.BackColor = System.Drawing.Color.Yellow;
                            lblStatus.Text = reader1["andamento"].ToString();
                        }
                        else if (reader1["andamento"].ToString() == "EM ANDAMENTO")
                        {
                            lblStatus.BackColor = System.Drawing.Color.Purple;
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
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "abrirModal", "$('#modalOcorrencia').modal('show');", true);

                    }
                }
            }
            if (e.CommandName == "Coletas")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                string idCarga = id.ToString(); // esse valor viria da lógica do seu código

                string url = $"OrdemColetaImpressaoIndividual.aspx?id={idCarga}";
                string script = $"window.open('{url}', '_blank', 'toolbar=no,location=no,status=no,menubar=no,scrollbars=yes,resizable=yes,width=794,height=1123');";
                ClientScript.RegisterStartupScript(this.GetType(), "abrirJanela", script, true);
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
                        // verifica se o motorista pertence a transportadora
                        if (txtCodTransportadora.Text.Trim() != txtCodProprietario.Text.Trim())
                        {

                            string nomeUsuario = txtUsuCadastro.Text;
                            string linha1 = "Olá, " + nomeUsuario + "!";
                            string linha2 = "O motorista " + ddlMotorista.SelectedItem.Text.Trim() + ", não pertence a transportadora do veículo " + txtProprietario.Text.Trim() +".";
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
                            txtCodVeiculo.Focus();


                        }
                        // verifica se a funcao do motorista permite dirigir o veiculo
                        string primeiraLetraString = txtFuncao.Text.Trim().Substring(0, 1);                        
                        if (txtFuncao.Text != "")
                        {
                            if (primeiraLetraString == "M")
                            {
                                if (txtTipoVeiculo.Text ==  "BITRUCK" || txtTipoVeiculo.Text.Trim() == "UTILITÁRIO/FURGÃO" || txtTipoVeiculo.Text.Trim() == "LEVE" || txtTipoVeiculo.Text.Trim() == "FIORINO" || txtTipoVeiculo.Text.Trim() == "TOCO" || txtTipoVeiculo.Text.Trim() == "TRUCK" || txtTipoVeiculo.Text.Trim() == "VUC OU 3/4")
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
                                    txtCodVeiculo.Focus();
                                }
                            }
                            else if (primeiraLetraString == "C")
                            {
                                if (txtTipoVeiculo.Text.Trim() == "CAVALO SIMPLES" || txtTipoVeiculo.Text.Trim() == "CAVALO TRUCADO" || txtTipoVeiculo.Text.Trim() == "CAVALO 4 EIXOS" || txtTipoVeiculo.Text.Trim() == "BITRUCK" || txtTipoVeiculo.Text.Trim() == "UTILITÁRIO/FURGÃO" || txtTipoVeiculo.Text.Trim() == "LEVE" ||  txtTipoVeiculo.Text.Trim() == "FIORINO" || txtTipoVeiculo.Text.Trim() == "TOCO" || txtTipoVeiculo.Text.Trim() == "TRUCK" || txtTipoVeiculo.Text.Trim() == "VUC OU 3/4")
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

                                //fotoMotorista = "../../fotos/usuario.jpg";
                                txtCodVeiculo.Text = "";
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

                                //fotoMotorista = "../../fotos/usuario.jpg";
                                txtCodVeiculo.Text = "";
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
                cmd.Parameters.AddWithValue("@filialveiculo", SafeValue(txtFilialVeicCNT.Text));
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
                cmd.Parameters.AddWithValue("@empresa", SafeValue("CNT (CC)"));
                cmd.Parameters.AddWithValue("@dtalt", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                cmd.Parameters.AddWithValue("@usualt", nomeUsuario);


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
        protected void txtCodMotorista_TextChanged(object sender, EventArgs e)
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
                        txtLibGR.Text = ConsultaMotorista.validade.ToString();
                        // ddlMotorista.Items.Insert(0, new ListItem(ConsultaMotorista.nommot, ""));
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
                            txtFilialVeicCNT.Text = ConsultaMotorista.nucleo;
                            txtPlaca.Text = ConsultaMotorista.placa;
                            txtVeiculoTipo.Text = ConsultaMotorista.tipomot;
                            txtTipoVeiculo.Text = ConsultaMotorista.tipoveiculo;
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
                            ETI.Visible = true;
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
                                DateTime dataHoje = Convert.ToDateTime(DateTime.Now.Date);
                                DateTime dataETI = Convert.ToDateTime(txtExameToxic.Text).Date; ;

                                TimeSpan diferencaETI = dataETI - dataHoje;
                                // Agora você pode comparar a diferença
                                if (diferencaETI.TotalDays < 30)
                                {
                                    txtExameToxic.BackColor = System.Drawing.Color.Khaki;
                                    txtExameToxic.ForeColor = System.Drawing.Color.OrangeRed;
                                    string nomeUsuario = txtUsuCadastro.Text;
                                    string linha1 = "Olá, " + nomeUsuario + "!";
                                    string linha2 = "O Exame Toxicologico do motorista " + ddlMotorista.SelectedItem.Text.Trim() + ", vence em menos de 30 dias.";
                                    string linha3 = "Data de vencimento: " + dataETI.ToString("dd/MM/yyyy") + ".";
                                    //string linha4 = "Unidade: " + unidade + ". Por favor, verifique.";
                                    // Concatenando as linhas com '\n' para criar a mensagem
                                    string mensagem = $"{linha1}\n{linha2}\n{linha3}";
                                    string mensagemCodificada = HttpUtility.JavaScriptStringEncode(mensagem);
                                    //// Gerando o script JavaScript para exibir o alerta
                                    string script = $"alert('{mensagemCodificada}');";
                                    //// Registrando o script para execução no lado do cliente
                                    ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", script, true);
                                }
                                else if (diferencaETI.TotalDays <= 0)
                                {
                                    txtExameToxic.BackColor = System.Drawing.Color.Red;
                                    txtExameToxic.ForeColor = System.Drawing.Color.White;
                                    string nomeUsuario = txtUsuCadastro.Text;
                                    string linha1 = "Olá, " + nomeUsuario + "!";
                                    string linha2 = "O Exame Toxicologico do motorista " + ddlMotorista.SelectedItem.Text.Trim() + ", está vencido.";
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
                                    txtCodMotorista.Focus();

                                }
                                else
                                {
                                    if (txtExameToxic.Text == "")
                                    {
                                        string nomeUsuario = txtUsuCadastro.Text;
                                        string linha1 = "Olá, " + nomeUsuario + "!";
                                        string linha2 = "O motorista " + ddlMotorista.SelectedItem.Text.Trim() + ", não tem Exame Toxicologico, lançado no sistema.";
                                        string linha3 = "Verifique, por favor.";
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
                                    else
                                    {
                                        txtExameToxic.BackColor = System.Drawing.Color.LightGray;
                                        txtExameToxic.ForeColor = System.Drawing.Color.Black;
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
                                txtCNH.BackColor = System.Drawing.Color.Khaki;
                                txtCNH.ForeColor = System.Drawing.Color.OrangeRed;
                                string nomeUsuario = txtUsuCadastro.Text;
                                string linha1 = "Olá, " + nomeUsuario + "!";
                                string linha2 = "A CNH do motorista " + ddlMotorista.SelectedItem.Text.Trim() + ", vence em menos de 30 dias.";
                                string linha3 = "Data de vencimento: " + dataCNH.ToString("dd/MM/yyyy") + ".";
                                //string linha4 = "Unidade: " + unidade + ". Por favor, verifique.";
                                // Concatenando as linhas com '\n' para criar a mensagem
                                string mensagem = $"{linha1}\n{linha2}\n{linha3}";
                                string mensagemCodificada = HttpUtility.JavaScriptStringEncode(mensagem);
                                //// Gerando o script JavaScript para exibir o alerta
                                string script = $"alert('{mensagemCodificada}');";
                                //// Registrando o script para execução no lado do cliente
                                ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", script, true);
                            }
                            else if (diferenca.TotalDays <= 0)
                            {
                                txtCNH.BackColor = System.Drawing.Color.Red;
                                txtCNH.ForeColor = System.Drawing.Color.White;
                                string nomeUsuario = txtUsuCadastro.Text;
                                string linha1 = "Olá, " + nomeUsuario + "!";
                                string linha2 = "A CNH do motorista " + ddlMotorista.SelectedItem.Text.Trim() + ", está vencida.";
                                string linha3 = "Data de vencimento: " + dataCNH.ToString("dd/MM/yyyy") + ".";
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
                            else
                            {
                                if (txtCNH.Text == "")
                                {
                                    string nomeUsuario = txtUsuCadastro.Text;
                                    string linha1 = "Olá, " + nomeUsuario + "!";
                                    string linha2 = "O motorista " + ddlMotorista.SelectedItem.Text.Trim() + ", não tem validade de CNH, lançada no sistema.";
                                    string linha3 = "Verifique, por favor.";
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
                                else
                                {
                                    txtCNH.BackColor = System.Drawing.Color.LightGray;
                                    txtCNH.ForeColor = System.Drawing.Color.Black;
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
                                txtExameToxic.BackColor = System.Drawing.Color.Khaki;
                                txtExameToxic.ForeColor = System.Drawing.Color.OrangeRed;
                                string nomeUsuario = txtUsuCadastro.Text;
                                string linha1 = "Olá, " + nomeUsuario + "!";
                                string linha2 = "A liberãção de risco do motorista " + ddlMotorista.SelectedItem.Text.Trim() + ", vence em menos de 30 dias.";
                                string linha3 = "Data de vencimento: " + dataGR.ToString("dd/MM/yyyy") + ".";
                                //string linha4 = "Unidade: " + unidade + ". Por favor, verifique.";
                                // Concatenando as linhas com '\n' para criar a mensagem
                                string mensagem = $"{linha1}\n{linha2}\n{linha3}";
                                string mensagemCodificada = HttpUtility.JavaScriptStringEncode(mensagem);
                                //// Gerando o script JavaScript para exibir o alerta
                                string script = $"alert('{mensagemCodificada}');";
                                //// Registrando o script para execução no lado do cliente
                                ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", script, true);
                            }
                            else if (diferencaGR.TotalDays <= 0)
                            {
                                txtExameToxic.BackColor = System.Drawing.Color.Red;
                                txtExameToxic.ForeColor = System.Drawing.Color.White;
                                string nomeUsuario = txtUsuCadastro.Text;
                                string linha1 = "Olá, " + nomeUsuario + "!";
                                string linha2 = "A liberação de risco do motorista " + ddlMotorista.SelectedItem.Text.Trim() + ", está vencida.";
                                string linha3 = "Data do vencimento: " + dataGR.ToString("dd/MM/yyyy") + ".";
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
                            else
                            {
                                if (txtLibGR.Text == "")
                                {
                                    string nomeUsuario = txtUsuCadastro.Text;
                                    string linha1 = "Olá, " + nomeUsuario + "!";
                                    string linha2 = "O motorista " + ddlMotorista.SelectedItem.Text.Trim() + ", não tem liberação de risco cadastrada.";
                                    string linha3 = "Verifique, por favor.";
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
                                else
                                {
                                    txtLibGR.BackColor = System.Drawing.Color.LightGray;
                                    txtLibGR.ForeColor = System.Drawing.Color.Black;
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
                }
                catch (Exception ex)
                {
                    // Logar ou exibir erro
                    Response.Write("<script>alert('Erro: " + ex.Message + "');</script>");
                }
                // Opcional: limpar ou fechar modal
                ScriptManager.RegisterStartupScript(
                                                        this,
                                                        this.GetType(),
                                                        "fecharModalOcorrencia",
                                                        "$('#modalOcorrencia').modal('hide'); $('.modal-backdrop').remove(); $('body').removeClass('modal-open');",
                                                        true
                                                    );
            }
            
        }
        protected void btnFechar_Click(object sender, EventArgs e)
        {
            // Opcional: limpar ou fechar modal
            ScriptManager.RegisterStartupScript(
                                                    this,
                                                    this.GetType(),
                                                    "fecharModalOcorrencia",
                                                    "$('#modalOcorrencia').modal('hide'); $('.modal-backdrop').remove(); $('body').removeClass('modal-open');",
                                                    true
                                                );
        }

        //private bool ExisteCarga(string numeroCarga)
        //{
        //    bool existe = false;

        //    string connectionString = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
        //    using (SqlConnection conn = new SqlConnection(connectionString))
        //    {
        //        string sql = "SELECT COUNT(*) FROM tbcargas WHERE cva = @numero";
        //        using (SqlCommand cmd = new SqlCommand(sql, conn))
        //        {
        //            cmd.Parameters.AddWithValue("@numero", numeroCarga);
        //            conn.Open();

        //            int count = (int)cmd.ExecuteScalar();
        //            existe = (count > 0);
        //        }
        //    }

        //    return existe;
        //}

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
        private void CarregarMotivoOcorrencias(string codigo)
        {
            string strConn = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                string query = "SELECT codigo, motivo FROM tbmotivoocorrenciacnt WHERE codigo_responsavel = @Cod_Responsavel";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Cod_Responsavel", codigo);
                conn.Open();
                cboMotivo.DataSource = cmd.ExecuteReader();
                cboMotivo.DataTextField = "motivo";
                cboMotivo.DataValueField = "codigo"; // valor único
                cboMotivo.DataBind();

                cboMotivo.Items.Insert(0, new ListItem("Selecione...", "0"));
            }
        }
        private void PreencherComboResponsavel()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT codigo, responsavel FROM tbresponsavelocorrenciacnt ORDER BY responsavel";

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
                    cboResponsavel.DataTextField = "responsavel";  // Campo que será mostrado no ComboBox
                    cboResponsavel.DataValueField = "codigo";  // Campo que será o valor de cada item                    
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
        protected void cboResponsavel_SelectedIndexChanged(object sender, EventArgs e)
        {
            string codigo = cboResponsavel.SelectedValue;
            CarregarMotivoOcorrencias(codigo);

            // Restaurar cidade se estiver em ViewState
            if (ViewState["MotivoSelecionado"] != null)
            {
                string motivoId = ViewState["MotivoSelecionado"].ToString();
                if (cboMotivo.Items.FindByValue(motivoId) != null)
                {
                    cboMotivo.SelectedValue = motivoId;
                }
            }
            // Reexibe o modal após postback
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModal", "$('#modalOcorrencia').modal('show');", true);

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

    }
}