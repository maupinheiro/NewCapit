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
using System.Text;
using System.Windows.Interop;


namespace NewCapit.dist.pages
{
    public partial class ColetasMatriz : System.Web.UI.Page
    {
        SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString());
        public string fotoMotorista;
        string codmot, caminhofoto;
        string codFrota;
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
                    //txtUsuCadastro.Text = nomeUsuario;
                }
                else
                {
                    var lblUsuario = "<Usuário>";
                    //  txtUsuCadastro.Text = lblUsuario;
                    Response.Redirect("Login.aspx");
                }

                // lblDtCadastro.Text = dataHoraAtual.ToString("dd/MM/yyyy HH:mm");
                txtCadastro.Text = dataHoraAtual.ToString("dd/MM/yyyy HH:mm");                

                fotoMotorista = "/fotos/motoristasemfoto.jpg";                
                PreencherNumColeta();
                PreencherComboMotoristas();
                PreencherClienteInicial();
                PreencherClienteFinal();

                divMsg.Visible = false;
                divMsgCNH.Visible = false;
                divMsgCarreta1.Visible = false;
                divMsgCarreta2.Visible = false;
                divMsgCET.Visible = false;
                divMsgCrono.Visible = false;
                divMsgGR.Visible = false;
                divMsgLinc.Visible = false;
                divMsgVeic.Visible = false;

                Session["Cargas"] = CriarTabelaCargas();
            }
            //CarregarFotoMotorista(fotoMotorista);
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
        private void CarregarFotoMotorista(string fotoMotorista)
        {
            //fotoMotorista = "/fotos/motoristasemfoto.jpg";
            // 1️⃣ NULL ou vazio
            //if (string.IsNullOrEmpty(fotoMotorista))
            //{
            //    imgFoto.ImageUrl = "/fotos/motoristasemfoto.jpg";
            //    return;
            //}

            // 2️⃣ Verifica se o arquivo existe
            //string caminhoFisico = Server.MapPath(fotoMotorista);

            //if (!System.IO.File.Exists(caminhoFisico))
            //{
            //    imgFoto.ImageUrl = "/fotos/motoristasemfoto.jpg";
            //    return;
            //}

            // 3️⃣ Tudo certo → mostra a foto
            //imgFoto.ImageUrl = fotoMotorista;

        }
        
        protected void CarregaFoto()
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
        protected void btnBuscarMotorista_Click(object sender, EventArgs e)
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
            using (SqlCommand cmd = new SqlCommand(sql, conn))
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
                txtValCNH.Text = Convert.ToDateTime(dt.Rows[0]["venccnh"]).ToString("dd/MM/yyyy");
                txtValGR.Text = dt.Rows[0]["validade"].ToString();
                txtCelularParticular.Text = dt.Rows[0]["fone2"].ToString();
                txtCPF.Text = dt.Rows[0]["cpf"].ToString();
                txtNumCartao.Text = dt.Rows[0]["cartaomot"].ToString();
                txtValCartao.Text = dt.Rows[0]["venccartao"].ToString();
                txtCodTransportadora.Text = dt.Rows[0]["codtra"].ToString();
                txtTransportadora.Text = dt.Rows[0]["transp"].ToString();
                fotoMotorista = dt.Rows[0]["caminhofoto"].ToString();
                txtLiberacaoGR.Text = dt.Rows[0]["codliberacao"].ToString();
                // FOTO
                CarregarFotoMotorista(fotoMotorista);
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
                if (!DateTime.TryParse(txtValCNH.Text, out dataCNH))
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
                        txtValCNH.BackColor = System.Drawing.Color.Red;
                        txtValCNH.ForeColor = System.Drawing.Color.White;
                        txtCodMotorista.Text = "";
                        txtCodMotorista.Focus();
                    }
                    else if (diferencaCNH.TotalDays <= 30)
                    {
                        MostrarMsgCNH("Atenção! A CNH do " + ddlMotorista.SelectedItem.Text.Trim() + ", vence em " + diferencaCNH.Days + " dias.", "warning");
                        txtValCNH.BackColor = System.Drawing.Color.Khaki;
                        txtValCNH.ForeColor = System.Drawing.Color.OrangeRed;
                    }
                }

                // valida GR
                DateTime dataGR;
                if (!DateTime.TryParse(txtValGR.Text, out dataGR))
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
                        txtValGR.BackColor = System.Drawing.Color.Red;
                        txtValGR.ForeColor = System.Drawing.Color.White;
                        txtCodMotorista.Text = "";
                        txtCodMotorista.Focus();
                    }
                    else if (diferencaGR.TotalDays <= 30)
                    {
                        MostrarMsgGR("Atenção! Liberação de Risco do motorista " + ddlMotorista.SelectedItem.Text.Trim() + ", vence em " + diferencaGR.Days + " dias.", "warning");
                        txtValGR.BackColor = System.Drawing.Color.Khaki;
                        txtValGR.ForeColor = System.Drawing.Color.OrangeRed;
                    }
                }

                txtCodVeiculo.Text = dt.Rows[0]["frota"].ToString();

            }

            //Dados do veiculos
            string sqlVeiculos = @"SELECT codvei, plavei, reboque1, reboque2, tipoveiculo, tipvei, tiporeboque, tipocarreta, vencimentolaudofumaca, venclicenciamento, codtra, transp, venclicencacet, protocolocet, venccronotacografo, rastreamento, rastreador, ativo_inativo, fl_exclusao
                   FROM tbveiculos 
                   WHERE codvei = @idVeiculo AND ativo_inativo = 'ATIVO' AND fl_exclusao IS NULL";
            using (SqlCommand cmdVeiculos = new SqlCommand(sqlVeiculos, conn))
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
                txtNumProtCET.Text = dtVeiculos.Rows[0]["protocolocet"].ToString();
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
                using (SqlCommand cmdCarreta1 = new SqlCommand(sqlCarreta1, conn))
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
                using (SqlCommand cmdCarreta2 = new SqlCommand(sqlCarreta2, conn))
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

        private DataTable CriarTabelaCargas()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("carga", typeof(string));
            dt.Columns.Add("previsao", typeof(DateTime));
            dt.Columns.Add("cliorigem", typeof(string));
            dt.Columns.Add("cidorigem", typeof(string));
            dt.Columns.Add("ufcliorigem", typeof(string));
            dt.Columns.Add("clidestino", typeof(string));
            dt.Columns.Add("ciddestino", typeof(string));
            dt.Columns.Add("ufclidestino", typeof(string));

            return dt;
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

        private void BuscarCargaNoBanco(string carga)
        {
           
            // Recupera a lista atual
            DataTable lista = (DataTable)Session["Cargas"];

            // ❌ Verifica Duplicata
            foreach (DataRow row in lista.Rows)
            {
                if (row["carga"].ToString() == carga)
                {
                    MostrarMsg("A carga já foi adicionada!");
                    txtCarga.Text = "";
                    txtCarga.Focus();
                    return;
                }
            }

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
            SELECT id, carga, previsao, cliorigem, cidorigem, ufcliorigem, clidestino, ciddestino, ufclidestino, status, andamento, idviagem
            FROM tbcargas
            WHERE carga = @c
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
                                DataRow linha = dt.Rows[0];

                                // Carga encontrada → adiciona na GridView
                                //DataTable lista = (DataTable)Session["Cargas"];
                                lista.Rows.Add(
                                    dt.Rows[0]["carga"],
                                    dt.Rows[0]["previsao"],
                                    dt.Rows[0]["cliorigem"],
                                    dt.Rows[0]["cidorigem"],
                                    dt.Rows[0]["ufcliorigem"],
                                    dt.Rows[0]["clidestino"],
                                    dt.Rows[0]["ciddestino"],
                                    dt.Rows[0]["ufclidestino"]
                                );

                                gvCargas.DataSource = lista;
                                gvCargas.DataBind();

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

        protected void gvCargas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "detalhes")
            {
                // Sempre converte para índice válido
                if (!int.TryParse(e.CommandArgument.ToString(), out int index))
                {
                    MostrarMsg("Erro: índice inválido.");
                    return;
                }

                DataTable lista = (DataTable)Session["Cargas"];

                // Verifica se a linha existe
                if (index < 0 || index >= lista.Rows.Count)
                {
                    MostrarMsg("Erro: índice fora do intervalo.");
                    return;
                }

                string carga = lista.Rows[index]["carga"].ToString();

                BuscarPedidos(carga);
            }
            if (e.CommandName == "Excluir")
            {
                int index = Convert.ToInt32(e.CommandArgument);

                // Recupera o DataTable da Session
                DataTable lista = (DataTable)Session["Cargas"];

                // Remove a linha
                lista.Rows.RemoveAt(index);

                // Atualiza a Session
                Session["Cargas"] = lista;

                // Rebind na Grid
                gvCargas.DataSource = lista;
                gvCargas.DataBind();
            }
        }

        private void BuscarPedidos(string carga)
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                using (SqlCommand cmd = new SqlCommand(@"
            SELECT carga, pedido, emissao, material, peso, portao, situacao, solicitante, entrega 
            FROM tbpedidos
            WHERE carga = @c
        ", conn))
                {
                    cmd.Parameters.AddWithValue("@c", carga);

                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count < 1)
                    {
                        lblCargaSel.Text =    "Carga "+ carga + " sem pedidos vinculados!";
                        lblCargaSel.ForeColor = System.Drawing.Color.Blue;
                        gvPedidos.DataSource = null;
                        gvPedidos.DataBind();
                        return;
                    }
                    else
                    {
                        lblCargaSel.Text = dt.Rows[0]["carga"].ToString();
                        gvPedidos.DataSource = dt;
                        gvPedidos.DataBind();
                    }
                }
            }
        }
        protected void btnPesquisarContato_Click(object sender, EventArgs e)
        {
            divMsg.Visible = false;
            divMsgCNH.Visible = false;
            divMsgCarreta1.Visible = false;
            divMsgCarreta2.Visible = false;
            divMsgCET.Visible = false;
            divMsgCrono.Visible = false;
            divMsgGR.Visible = false;
            divMsgLinc.Visible = false;
            divMsgVeic.Visible = false;
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
                    txtCarga.Focus();

                }
                else
                {
                    // Modal para carregar o form cadastro de contato
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "abrirModalTelefone", "abrirModalTelefone();", true);
                }

            }
        }
        protected void btnCadContato_Click(object sender, EventArgs e)
        {
            string codigoFrota = txtCodContato.Text.Trim();
            string numeroTelefone = txtCadCelular.Text;

            string connectionString = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO tbfoneveiculos (veiculo, numero)" +
                  "VALUES (@veiculo,  @numero)";
                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@veiculo", codigoFrota);
                cmd.Parameters.AddWithValue("@numero", numeroTelefone);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    int rowsInserted = cmd.ExecuteNonQuery();
                    if (rowsInserted > 0)
                    {
                        txtCodFrota.Text = codigoFrota;
                        txtFoneCorp.Text = numeroTelefone;

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
                lblDistancia.Text = "Não há distância cadastrada entre ORIGEM e DESTINO...";
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

        protected void btnSalvarColeta_Click(object sender, EventArgs e)
        {
            
            string novaCarga = novaCargaVazia.Text.Trim();
            int numCarga = int.Parse(novaCargaVazia.Text.Trim());
            string codigoOrigem = codCliInicial.Text.Trim();
            string nomeOrigem = ddlCliInicial.SelectedItem.Text.Trim().ToUpper();
            string codigoDestino = codCliFinal.Text.Trim();
            string nomeDestino = ddlCliFinal.SelectedItem.Text.Trim().ToUpper();
            string municipioOrigem = txtMunicipioOrigem.Text.Trim().ToUpper();
            string municipioDestino = txtMunicipioDestino.Text.Trim().ToUpper();
            string ufOrigem = txtUfOrigem.Text.Trim().ToUpper();
            string ufDestino = txtUfDestino.Text.Trim().ToUpper();
            double distancia = double.Parse(txtDistancia.Text.Trim());

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
                cmd.Parameters.AddWithValue("@empresa", "1111"); // ou valor padrão
                cmd.Parameters.AddWithValue("@cadastro", DateTime.Now.ToString("dd/MM/yyyy HH:mm") + " - " + Session["UsuarioLogado"].ToString());
                cmd.Parameters.AddWithValue("@distancia", distancia);

                // Abrindo a conexão e executando a query
                conn.Open();
                int rowsInserted = cmd.ExecuteNonQuery();

                if (rowsInserted > 0)
                {
                    txtCarga.Text = novaCarga;
                    
                    BuscarCargaNoBanco(novaCarga);

                    ScriptManager.RegisterStartupScript(
                        this,
                        this.GetType(),
                        "FechaModal",
                        "$('#meuModal').modal('hide');",
                        true
                    );
                }
               
                else
                {
                    string mensagem = "Falha ao cadastrar a viagem. Tente novamente.";
                    string script = $"alert('{HttpUtility.JavaScriptStringEncode(mensagem)}');";
                    ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeErro", script, true);
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
                                codCliFinal.Focus();
                                // Opcional: exibir mensagem ao usuário
                            }
                        }
                    }

                }

            }
            if (codCliInicial.Text != "" && codCliFinal.Text != "")
            {
                //string UFOrigem = txtUfOrigem.Text.Trim();
                //string Origem = ddlCliInicial.SelectedItem.Text.Trim();
                //string UFDestino = txtUfDestino.Text.Trim();
                //string Destino = ddlCliFinal.SelectedItem.Text.Trim();

                string strConn = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    SqlCommand cmd = new SqlCommand(@"
                        SELECT 
                        c1.LATITUDE  AS LatOrigem,
                        c1.LONGITUDE AS LonOrigem,
                        c2.LATITUDE  AS LatDestino,
                        c2.LONGITUDE AS LonDestino
                    FROM tbcidadesdobrasil c1
                    JOIN tbcidadesdobrasil c2 ON 1 = 1
                    WHERE 
                        c1.NOME_MUNICIPIO COLLATE Latin1_General_CI_AI  = @CidadeOrigem AND c1.UF = @UFOrigem
                    AND c2.NOME_MUNICIPIO COLLATE Latin1_General_CI_AI  = @CidadeDestino AND c2.UF = @UFDestino
                ", conn);

                    cmd.Parameters.AddWithValue("@CidadeOrigem", txtMunicipioOrigem.Text.Trim());
                    cmd.Parameters.AddWithValue("@UFOrigem", txtUfOrigem.Text.Trim());
                    cmd.Parameters.AddWithValue("@CidadeDestino", txtMunicipioDestino.Text.Trim());
                    cmd.Parameters.AddWithValue("@UFDestino", txtUfDestino.Text.Trim());

                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        double distancia = CalcularDistancia(
                            Convert.ToDouble(dr["LatOrigem"]),
                            Convert.ToDouble(dr["LonOrigem"]),
                            Convert.ToDouble(dr["LatDestino"]),
                            Convert.ToDouble(dr["LonDestino"])
                        );

                        txtDistancia.Text = $"{distancia:N2}"; //$"Distância: {distancia:N2} km";
                    }
                    else
                    {
                        lblDistancia.Text = "Cidade não encontrada.";
                    }
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


        protected void MostrarMsg(string mensagem, string tipo = "warning")
        {
            divMsg.Attributes["class"] = "alert alert-" + tipo + " alert-dismissible fade show mt-3";
            lblMsg.InnerText = mensagem;
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

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            string cs = WebConfigurationManager.ConnectionStrings["conexao"].ToString();

            using (SqlConnection conn = new SqlConnection(cs))
            {
                conn.Open();

                SqlTransaction tran = conn.BeginTransaction();

                try
                {
                    string nomeUsuario = Session["UsuarioLogado"]?.ToString() ?? "";

                    #region INSERT tbcarregamentos

                    string insert = @"
                INSERT INTO tbcarregamentos (
                    num_carregamento, codmotorista, nucleo, tipomot, valtoxicologico, venccnh, valgr, foto, nomemotorista, cpf,
                    cartaopedagio, valcartao, foneparticular, veiculo, veiculotipo, valcet, valcrlvveiculo,
                    valcrlvreboque1, valcrlvreboque2, placa, tipoveiculo, reboque1, reboque2, carreta, tecnologia, rastreamento,
                    tipocarreta, codtra, transportadora, codcontato, fonecorporativo, empresa, dtcad, usucad,
                    situacao, funcao, codtranspmotorista, nomtranspmotorista, venccronotacografo, valopacidade, emissao, numero_gr, numero_protocolo_cet
                )
                VALUES (
                    @num_carregamento, @codmotorista, @nucleo, @tipomot, @valtoxicologico, @venccnh, @valgr, @foto, @nomemotorista, @cpf,
                    @cartaopedagio, @valcartao, @foneparticular, @veiculo, @veiculotipo, @valcet, @valcrlvveiculo,
                    @valcrlvreboque1, @valcrlvreboque2, @placa, @tipoveiculo, @reboque1, @reboque2, @carreta, @tecnologia, @rastreamento,
                    @tipocarreta, @codtra, @transportadora, @codcontato, @fonecorporativo, @empresa, @dtcad, @usucad,
                    @situacao, @funcao, @codtranspmotorista, @nomtranspmotorista, @venccronotacografo, @valopacidade, @emissao, @numero_gr, @numero_protocolo_cet
                )";

                    using (SqlCommand cmd = new SqlCommand(insert, conn, tran))
                    {
                        cmd.Parameters.AddWithValue("@num_carregamento", SafeValue(novaColeta.Text));
                        cmd.Parameters.AddWithValue("@codmotorista", SafeValue(txtCodMotorista.Text));
                        cmd.Parameters.AddWithValue("@nucleo", SafeValue(txtFilialMot.Text));
                        cmd.Parameters.AddWithValue("@tipomot", SafeValue(txtTipoMot.Text));

                        cmd.Parameters.AddWithValue("@valtoxicologico", SafeDateValue(txtExameToxic.Text));
                        cmd.Parameters.AddWithValue("@venccnh", SafeDateValue(txtValCNH.Text));
                        cmd.Parameters.AddWithValue("@valgr", SafeDateValue(txtValGR.Text));

                        cmd.Parameters.AddWithValue("@foto", SafeValue(fotoMotorista));

                        cmd.Parameters.AddWithValue("@nomemotorista", ddlMotorista.SelectedItem.Text);
                        cmd.Parameters.AddWithValue("@cpf", SafeValue(txtCPF.Text));
                        cmd.Parameters.AddWithValue("@cartaopedagio", SafeValue(txtNumCartao.Text));
                        cmd.Parameters.AddWithValue("@valcartao", txtValCartao.Text);
                        cmd.Parameters.AddWithValue("@foneparticular", SafeValue(txtCelularParticular.Text));
                        cmd.Parameters.AddWithValue("@veiculo", SafeValue(txtCodVeiculo.Text));
                        cmd.Parameters.AddWithValue("@veiculotipo", SafeValue(txtVeiculoTipo.Text));

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
                        cmd.Parameters.AddWithValue("@empresa", "1111");
                        cmd.Parameters.AddWithValue("@dtcad", DateTime.Now);
                        cmd.Parameters.AddWithValue("@usucad", nomeUsuario);
                        cmd.Parameters.AddWithValue("@situacao", "EM ANDAMENTO");
                        cmd.Parameters.AddWithValue("@funcao", SafeValue(txtFuncao.Text));
                        cmd.Parameters.AddWithValue("@codtranspmotorista", SafeValue(txtCodTransportadora.Text));
                        cmd.Parameters.AddWithValue("@nomtranspmotorista", SafeValue(txtTransportadora.Text));
                        cmd.Parameters.AddWithValue("@venccronotacografo", SafeDateValue(txtCrono.Text));
                        cmd.Parameters.AddWithValue("@valopacidade", SafeDateValue(txtOpacidade.Text));
                        cmd.Parameters.AddWithValue("@emissao", DateTime.Now);
                        cmd.Parameters.AddWithValue("@numero_gr", txtLiberacaoGR.Text);
                        cmd.Parameters.AddWithValue("@numero_protocolo_cet", txtNumProtCET.Text);
                        cmd.ExecuteNonQuery();
                    }

                    #endregion

                    #region UPDATE tbcargas

                    foreach (GridViewRow row in gvCargas.Rows)
                    {
                        if (row.RowType != DataControlRowType.DataRow) continue;

                        string carga = gvCargas.DataKeys[row.RowIndex].Value.ToString();

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

                        using (SqlCommand cmd = new SqlCommand(updateCargas, conn, tran))
                        {
                            cmd.Parameters.Add("@carga", SqlDbType.VarChar).Value = carga;
                            cmd.Parameters.Add("@idviagem", SqlDbType.VarChar).Value = novaColeta.Text;
                            cmd.Parameters.Add("@codmot", SqlDbType.VarChar).Value = txtCodMotorista.Text;
                            cmd.Parameters.Add("@frota", SqlDbType.VarChar).Value = txtCodFrota.Text;
                            cmd.Parameters.Add("@status", SqlDbType.VarChar).Value = "Pendente";
                            cmd.Parameters.Add("@andamento", SqlDbType.VarChar).Value = "EM ANDAMENTO";
                            cmd.Parameters.Add("@atendimento", SqlDbType.VarChar).Value = "";
                            cmd.Parameters.Add("@funcaomot", SqlDbType.VarChar).Value = txtFuncao.Text;
                            cmd.Parameters.Add("@emissao", SqlDbType.DateTime).Value = DateTime.Now;

                            cmd.ExecuteNonQuery();
                        }
                    }

                    #endregion

                    #region UPDATE tbpedidos

                    foreach (GridViewRow row in gvPedidos.Rows)
                    {
                        if (row.RowType != DataControlRowType.DataRow) continue;

                        string carga = row.Cells[0].Text;

                        string updatePedidos = @"
                    UPDATE tbpedidos SET
                        emissao = @emissao,
                        idviagem = @idviagem,
                        status = @status,
                        andamento = @andamento
                        
                    WHERE carga = @carga";

                        using (SqlCommand cmd = new SqlCommand(updatePedidos, conn, tran))
                        {
                            cmd.Parameters.Add("@carga", SqlDbType.VarChar).Value = carga;
                            cmd.Parameters.Add("@idviagem", SqlDbType.VarChar).Value = novaColeta.Text;
                            cmd.Parameters.Add("@status", SqlDbType.VarChar).Value = "Pendente";
                            cmd.Parameters.Add("@andamento", SqlDbType.VarChar).Value = "EM ANDAMENTO";
                            cmd.Parameters.Add("@emissao", SqlDbType.DateTime).Value = DateTime.Now;

                            cmd.ExecuteNonQuery();
                        }
                    }

                    #endregion

                    tran.Commit();

                    Response.Redirect("/dist/pages/GestaoDeEntregasMatriz.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw; // ou logar e exibir mensagem controlada
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
        private object SafeDateTimeValue(string input)
        {
            DateTime dt;
            if (DateTime.TryParse(input, out dt))
                return dt.ToString("yyyy-MM-dd HH:mm:ss");
            else
                return DBNull.Value;
        }

        protected void ddlMotorista_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtCodMotorista.Text = ddlMotorista.SelectedValue;

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
            using (SqlCommand cmd = new SqlCommand(sql, conn))
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
                txtValCNH.Text = Convert.ToDateTime(dt.Rows[0]["venccnh"]).ToString("dd/MM/yyyy");
                txtValGR.Text = dt.Rows[0]["validade"].ToString();
                txtCelularParticular.Text = dt.Rows[0]["fone2"].ToString();
                txtCPF.Text = dt.Rows[0]["cpf"].ToString();
                txtNumCartao.Text = dt.Rows[0]["cartaomot"].ToString();
                txtValCartao.Text = dt.Rows[0]["venccartao"].ToString();
                txtCodTransportadora.Text = dt.Rows[0]["codtra"].ToString();
                txtTransportadora.Text = dt.Rows[0]["transp"].ToString();
                fotoMotorista = dt.Rows[0]["caminhofoto"].ToString();
                txtLiberacaoGR.Text = dt.Rows[0]["codliberacao"].ToString();
                // FOTO
                CarregarFotoMotorista(fotoMotorista);
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
                if (!DateTime.TryParse(txtValCNH.Text, out dataCNH))
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
                        txtValCNH.BackColor = System.Drawing.Color.Red;
                        txtValCNH.ForeColor = System.Drawing.Color.White;
                        txtCodMotorista.Text = "";
                        txtCodMotorista.Focus();
                    }
                    else if (diferencaCNH.TotalDays <= 30)
                    {
                        MostrarMsgCNH("Atenção! A CNH do " + ddlMotorista.SelectedItem.Text.Trim() + ", vence em " + diferencaCNH.Days + " dias.", "warning");
                        txtValCNH.BackColor = System.Drawing.Color.Khaki;
                        txtValCNH.ForeColor = System.Drawing.Color.OrangeRed;
                    }
                }

                // valida GR
                DateTime dataGR;
                if (!DateTime.TryParse(txtValGR.Text, out dataGR))
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
                        txtValGR.BackColor = System.Drawing.Color.Red;
                        txtValGR.ForeColor = System.Drawing.Color.White;
                        txtCodMotorista.Text = "";
                        txtCodMotorista.Focus();
                    }
                    else if (diferencaGR.TotalDays <= 30)
                    {
                        MostrarMsgGR("Atenção! Liberação de Risco do motorista " + ddlMotorista.SelectedItem.Text.Trim() + ", vence em " + diferencaGR.Days + " dias.", "warning");
                        txtValGR.BackColor = System.Drawing.Color.Khaki;
                        txtValGR.ForeColor = System.Drawing.Color.OrangeRed;
                    }
                }

                txtCodVeiculo.Text = dt.Rows[0]["frota"].ToString();

            }

            //Dados do veiculos
            string sqlVeiculos = @"SELECT codvei, plavei, reboque1, reboque2, tipoveiculo, tipvei, tiporeboque, tipocarreta, vencimentolaudofumaca, venclicenciamento, codtra, transp, venclicencacet, protocolocet, venccronotacografo, rastreamento, rastreador, ativo_inativo, fl_exclusao
                   FROM tbveiculos 
                   WHERE codvei = @idVeiculo AND ativo_inativo = 'ATIVO' AND fl_exclusao IS NULL";
            using (SqlCommand cmdVeiculos = new SqlCommand(sqlVeiculos, conn))
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
                txtNumProtCET.Text = dtVeiculos.Rows[0]["protocolocet"].ToString();
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
                using (SqlCommand cmdCarreta1 = new SqlCommand(sqlCarreta1, conn))
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
                using (SqlCommand cmdCarreta2 = new SqlCommand(sqlCarreta2, conn))
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

        protected void MostrarMsgCarreta2(string mensagem, string tipo = "warning")
        {
            divMsgCarreta2.Attributes["class"] = "alert alert-" + tipo + " alert-dismissible fade show mt-3";
            lblMsgCarreta2.InnerText = mensagem;
            divMsgCarreta2.Style["display"] = "block";

            string script = @"setTimeout(function() {
                        var div = document.getElementById('divMsgCarreta2');
                        if (div) div.style.display = 'none';
                      }, 5000);";

            ScriptManager.RegisterStartupScript(this, GetType(), "EscondeMsg", script, true);
        }



        // Enviar rota via WhatsApp
        protected void btnWhats_ServerClick(object sender, EventArgs e)
        {
            string telefone = "5511982300498";
            string mensagem = "Segue a rota da carga " + txtCarga.Text;

            Response.Redirect("https://wa.me/" + telefone + "?text=" + Server.UrlEncode(mensagem));
        }

    }


}