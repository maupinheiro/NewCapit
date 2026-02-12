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
using System.Windows.Media.Media3D;


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
        double distancia;
        string sDuracao, sPercurso;
        string sOTCliente;       
        
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
                
                PreencherClienteInicial();
                PreencherClienteFinal();
                
                PreencherComboMotoristas();

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
                if (dt.Rows[0]["venceti"] != DBNull.Value)
                {
                    txtExameToxic.Text = Convert.ToDateTime(dt.Rows[0]["venceti"])
                                         .ToString("dd/MM/yyyy");
                }
                else
                {
                    txtExameToxic.Text = "";
                }

                if (dt.Rows[0]["venccnh"] != DBNull.Value)
                {
                    txtValCNH.Text = Convert.ToDateTime(dt.Rows[0]["venccnh"])
                                     .ToString("dd/MM/yyyy");
                }
                else
                {
                    txtValCNH.Text = "";
                }
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
                //CarregarFotoMotorista(fotoMotorista);
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
            string sqlVeiculos = @"SELECT codvei, plavei, reboque1, reboque2, tipoveiculo, tipvei, tiporeboque, tipocarreta, vencimentolaudofumaca, venclicenciamento, codtra, transp, venclicencacet, protocolocet, venccronotacografo, rastreamento, rastreador, ativo_inativo, fl_exclusao, eixos
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
                txtTecnologia.Text = dtVeiculos.Rows[0]["rastreador"].ToString();
                txtRastreamento.Text = dtVeiculos.Rows[0]["rastreamento"].ToString();

                txtOpacidade.Text = dtVeiculos.Rows[0]["vencimentolaudofumaca"].ToString();
                txtCET.Text = dtVeiculos.Rows[0]["venclicencacet"].ToString();
                txtNumProtCET.Text = dtVeiculos.Rows[0]["protocolocet"].ToString();
                txtCRLVVeiculo.Text = dtVeiculos.Rows[0]["venclicenciamento"].ToString();
                txtCrono.Text = dtVeiculos.Rows[0]["venccronotacografo"].ToString();
                txtEixos.Text = dtVeiculos.Rows[0]["eixos"].ToString();


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

            //Dados do proprietario
            string sqlProprietario = @"SELECT codtra, nomtra, cnpj
                   FROM tbtransportadoras 
                   WHERE codtra = @codtra";
            using (SqlCommand cmdProprietario = new SqlCommand(sqlProprietario, conn))
            {
                cmdProprietario.Parameters.AddWithValue("@codtra", txtCodProprietario.Text.Trim());

                DataTable dtProprietario = new DataTable();
                SqlDataAdapter daProprietario = new SqlDataAdapter(cmdProprietario);

                daProprietario.Fill(dtProprietario);

                if (dtProprietario.Rows.Count == 0)
                {
                    txtCPF_CNPJ.Text = "00.000.000/0000-00";
                }
                else
                {
                    txtCPF_CNPJ.Text = dtProprietario.Rows[0]["cnpj"].ToString();
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
            SELECT id, carga, previsao, cliorigem, cidorigem, ufcliorigem, clidestino, ciddestino, ufclidestino, status, andamento, idviagem, emitepedagio, cod_expedidor, expedidor, cid_expedidor, uf_expedidor, cod_recebedor, recebedor, cid_recebedor, uf_recebedor, pagador, ot, data_hora
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
                            //string sEmitePedagio = ViewState["emitepedagio"]?.ToString();
                            txtPedagio.Text = dt.Rows[0]["emitepedagio"].ToString();
                            txtCod_Expedidor.Text = dt.Rows[0]["cod_expedidor"].ToString();
                            txtExpedidor.Text = dt.Rows[0]["expedidor"].ToString();
                            txtCid_Expedidor.Text = dt.Rows[0]["cid_expedidor"].ToString();
                            txtUf_Expedidor.Text = dt.Rows[0]["uf_expedidor"].ToString();
                            txtCod_Recebedor.Text = dt.Rows[0]["cod_recebedor"].ToString();
                            txtRecebedor.Text = dt.Rows[0]["recebedor"].ToString();
                            txtCid_Recebedor.Text = dt.Rows[0]["cid_recebedor"].ToString();
                            txtUf_Recebedor.Text = dt.Rows[0]["uf_recebedor"].ToString();
                            
                            string nomeCompleto = dt.Rows[0]["pagador"].ToString().Trim();
                            string sOTCliente = dt.Rows[0]["ot"].ToString().Trim();
                            string primeiroNome = nomeCompleto.Split(' ')[0];
                            txtPagador.Text = primeiroNome;
                            if (dt.Rows[0][10].ToString() == "Pendente" || dt.Rows[0][10].ToString() == "PENDENTE")
                            {                                
                                // ✔ Agora é seguro acessar dt.Rows[0]
                                DataRow linha = dt.Rows[0];

                                // Carga encontrada → adiciona na GridView
                                DataTable listaCarga = (DataTable)Session["Cargas"];
                                listaCarga.Rows.Add(
                                    dt.Rows[0]["carga"],
                                    dt.Rows[0]["previsao"],
                                    dt.Rows[0]["cliorigem"],
                                    dt.Rows[0]["cidorigem"],
                                    dt.Rows[0]["ufcliorigem"],
                                    dt.Rows[0]["clidestino"],
                                    dt.Rows[0]["ciddestino"],
                                    dt.Rows[0]["ufclidestino"]
                                );   

                                gvCargas.DataSource = listaCarga;
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
            if (codCliInicial.Text != "")
            {

                string codigoRemetente = codCliInicial.Text.Trim();
                string strConn = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    string query = "SELECT codcli, razcli, cidcli, estcli FROM tbclientes WHERE codcli = @Codigo OR codvw=@Codigo";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Codigo", codigoRemetente);
                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                codCliInicial.Text = reader["codcli"].ToString();
                                ddlCliInicial.SelectedItem.Text = reader["razcli"].ToString();
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
        protected void ddlCliFinal_TextChanged(object sender, EventArgs e)
        {
            codCliFinal.Text = ddlCliFinal.SelectedValue;
            if (codCliFinal.Text != "")
            {


                string codigoRemetente = codCliFinal.Text.Trim();
                string strConn = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    string query = "SELECT codcli, razcli, cidcli, estcli FROM tbclientes WHERE codcli = @Codigo OR codvw=@Codigo";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Codigo", codigoRemetente);
                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                codCliFinal.Text = reader["codcli"].ToString();
                                ddlCliFinal.SelectedItem.Text = reader["razcli"].ToString();
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
                string UFOrigem = txtUfOrigem.Text.Trim();
                string Origem = ddlCliInicial.SelectedItem.Text.Trim();
                string UFDestino = txtUfDestino.Text.Trim();
                string Destino = ddlCliFinal.SelectedItem.Text.Trim();

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
                    string query = "SELECT codcli, razcli, cidcli, estcli FROM tbclientes WHERE codcli = @Codigo OR codvw=@Codigo";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Codigo", codigoRemetente);
                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                codCliInicial.Text = reader["codcli"].ToString();
                                ddlCliInicial.SelectedItem.Text = reader["razcli"].ToString();
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
                    string query = "SELECT codcli, razcli, cidcli, estcli FROM tbclientes WHERE codcli = @Codigo OR codvw=@Codigo";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Codigo", codigoRemetente);
                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                codCliFinal.Text = reader["codcli"].ToString();
                                ddlCliFinal.SelectedItem.Text = reader["razcli"].ToString();
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
                string UFOrigem = txtUfOrigem.Text.Trim();
                string Origem = ddlCliInicial.SelectedItem.Text.Trim();
                string UFDestino = txtUfDestino.Text.Trim();
                string Destino = ddlCliFinal.SelectedItem.Text.Trim();

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
            }
        }
        
        // Salvando coleta vazia
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
            if (codCliInicial.Text != string.Empty || codCliFinal.Text != string.Empty || txtPesoVazio.Text != string.Empty || txtCod_PagadorVazio.Text != string.Empty)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO tbcargas (carga, emissao, status, entrega, peso, material, portao, situacao, previsao, codorigem, cliorigem, coddestino, clidestino, ufcliorigem, ufclidestino, cidorigem, ciddestino, empresa, cadastro, tomador, andamento, cod_expedidor, expedidor, cid_expedidor, uf_expedidor, cod_recebedor, recebedor, cid_recebedor, uf_recebedor, cod_pagador, pagador, cid_pagador, uf_pagador, duracao)" +
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
                    cmd.Parameters.AddWithValue("@andamento", "Pendente");
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
                        // txtCarga.Text = novaCargaVazia.Text.Trim();
                        //ScriptManager.RegisterStartupScript(
                        //    this,
                        //    this.GetType(),
                        //    "FechaModal",
                        //    "$('#meuModal').modal('hide');",
                        //    true
                        //);
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
                MostrarMsg2("Informar campos obrigatórios da carga!");
            }

        }
        public void MostrarMsg2(string mensagem)
        {
            // Substitua o alert por um Toastr ou SweetAlert se preferir, mas o RegisterStartupScript é essencial
            string script = $"alert('{mensagem.Replace("'", "")}');";
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", script, true);
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
        object DbInt(object v)
        {
            if (v == null || v == DBNull.Value)
                return DBNull.Value;

            if (v is int i)
                return i;

            return int.TryParse(v.ToString(), out int r)
                ? r
                : (object)DBNull.Value;
        }
        protected void btnSalvar_Click(object sender, EventArgs e)
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
            bool temCargas = gvCargas.Rows
                .Cast<GridViewRow>()
                .Any(r => r.RowType == DataControlRowType.DataRow);

            if (!temCargas)
            {
                MostrarMsg("Inclua ao menos uma carga antes de salvar.");
                return;
                              
            }
            if (txtCodMotorista.Text == string.Empty)
            {
                MostrarMsg("É necessário incluir um motorista antes de salvar.");
                return;
            }
            if (txtCodFrota.Text == string.Empty)
            {
                MostrarMsg("É necessário incluir o cóodigo de frota antes de salvar.");
                return;
            }

            object DbString(string v) =>
                string.IsNullOrWhiteSpace(v) ? (object)DBNull.Value : v.Trim();

            object DbInt(string v) =>
                int.TryParse(v, out var i) ? i : (object)DBNull.Value;

            object DbDate(string v) =>
                DateTime.TryParse(v, out var d) ? d : (object)DBNull.Value;
            object DbDateTime(string v) =>
                DateTime.TryParse(v, out var d) ? d : (object)DBNull.Value;

            object DbDecimal(string v) =>
                decimal.TryParse(v, out var d) ? d : (object)DBNull.Value;
            string nomeCompleto = txtPagador.Text;
            string primeiroNome = nomeCompleto.Split(' ')[0];
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
                    situacao, funcao, codtranspmotorista, nomtranspmotorista, venccronotacografo, valopacidade, emissao, numero_gr, numero_protocolo_cet, cpf_cnpj_proprietario, pedagio, eixos, pedagiofeito, cod_expedidor, expedidor, cid_expedidor, uf_expedidor, cod_recebedor, recebedor, cid_recebedor, uf_recebedor, tomadorservico, dtottu)
                VALUES (
                    @num_carregamento, @codmotorista, @nucleo, @tipomot, @valtoxicologico, @venccnh, @valgr, @foto, @nomemotorista, @cpf,
                    @cartaopedagio, @valcartao, @foneparticular, @veiculo, @veiculotipo, @valcet, @valcrlvveiculo,
                    @valcrlvreboque1, @valcrlvreboque2, @placa, @tipoveiculo, @reboque1, @reboque2, @carreta, @tecnologia, @rastreamento,
                    @tipocarreta, @codtra, @transportadora, @codcontato, @fonecorporativo, @empresa, @dtcad, @usucad,
                    @situacao, @funcao, @codtranspmotorista, @nomtranspmotorista, @venccronotacografo, @valopacidade, @emissao, @numero_gr, @numero_protocolo_cet, @cpf_cnpj_proprietario, @pedagio, @eixos, @pedagiofeito, @cod_expedidor, @expedidor, @cid_expedidor, @uf_expedidor, @cod_recebedor, @recebedor, @cid_recebedor, @uf_recebedor, @tomadorservico, @dtottu)";

                    using (SqlCommand cmd = new SqlCommand(insert, conn, tran))
                    {
                        // PRINCIPAIS
                        cmd.Parameters.Add("@num_carregamento", SqlDbType.VarChar, 20).Value = DbString(novaColeta.Text);
                        cmd.Parameters.Add("@codmotorista", SqlDbType.VarChar, 10).Value = DbString(txtCodMotorista.Text);
                        cmd.Parameters.Add("@nucleo", SqlDbType.VarChar, 25).Value = DbString(txtFilialMot.Text);
                        cmd.Parameters.Add("@tipomot", SqlDbType.VarChar, 25).Value = DbString(txtTipoMot.Text);

                        // DATAS MOTORISTA
                        cmd.Parameters.Add("@valtoxicologico", SqlDbType.Date).Value = DbDate(txtExameToxic.Text);
                        cmd.Parameters.Add("@venccnh", SqlDbType.Date).Value = DbDate(txtValCNH.Text);
                        cmd.Parameters.Add("@valgr", SqlDbType.Date).Value = DbDate(txtValGR.Text);

                        // FOTO
                        cmd.Parameters.Add("@foto", SqlDbType.VarChar).Value = DbString(fotoMotorista);

                        // DADOS MOTORISTA
                        cmd.Parameters.Add("@nomemotorista", SqlDbType.VarChar, 150).Value = DbString(ddlMotorista.SelectedItem.Text);
                        cmd.Parameters.Add("@cpf", SqlDbType.VarChar, 14).Value = DbString(txtCPF.Text);
                        cmd.Parameters.Add("@cartaopedagio", SqlDbType.VarChar, 30).Value = DbString(txtNumCartao.Text);
                        cmd.Parameters.Add("@valcartao", SqlDbType.Decimal).Value = DbDecimal(txtValCartao.Text);
                        cmd.Parameters.Add("@foneparticular", SqlDbType.VarChar, 20).Value = DbString(txtCelularParticular.Text);

                        // VEÍCULO
                        cmd.Parameters.Add("@veiculo", SqlDbType.VarChar, 20).Value = DbString(txtCodVeiculo.Text);
                        cmd.Parameters.Add("@veiculotipo", SqlDbType.VarChar, 20).Value = DbString(txtVeiculoTipo.Text);

                        cmd.Parameters.Add("@valcet", SqlDbType.Date).Value = DbDate(txtCET.Text);
                        cmd.Parameters.Add("@valcrlvveiculo", SqlDbType.Date).Value = DbDate(txtCRLVVeiculo.Text);
                        cmd.Parameters.Add("@valcrlvreboque1", SqlDbType.Date).Value = DbDate(txtCRLVReb1.Text);
                        cmd.Parameters.Add("@valcrlvreboque2", SqlDbType.Date).Value = DbDate(txtCRLVReb2.Text);

                        cmd.Parameters.Add("@placa", SqlDbType.VarChar, 10).Value = DbString(txtPlaca.Text);
                        cmd.Parameters.Add("@tipoveiculo", SqlDbType.VarChar, 20).Value = DbString(txtTipoVeiculo.Text);
                        cmd.Parameters.Add("@reboque1", SqlDbType.VarChar, 20).Value = DbString(txtReboque1.Text);
                        cmd.Parameters.Add("@reboque2", SqlDbType.VarChar, 20).Value = DbString(txtReboque2.Text);
                        cmd.Parameters.Add("@carreta", SqlDbType.VarChar, 20).Value = DbString(txtCarreta.Text);

                        cmd.Parameters.Add("@tecnologia", SqlDbType.VarChar, 30).Value = DbString(txtTecnologia.Text);
                        cmd.Parameters.Add("@rastreamento", SqlDbType.VarChar, 30).Value = DbString(txtRastreamento.Text);

                        cmd.Parameters.Add("@tipocarreta", SqlDbType.VarChar, 80).Value = DbString(txtConjunto.Text);

                        // PROPRIETÁRIO / TRANSPORTADORA
                        cmd.Parameters.Add("@codtra", SqlDbType.VarChar, 20).Value = DbString(txtCodProprietario.Text);
                        cmd.Parameters.Add("@transportadora", SqlDbType.VarChar, 150).Value = DbString(txtProprietario.Text);
                        cmd.Parameters.Add("@codcontato", SqlDbType.VarChar, 20).Value = DbString(txtCodFrota.Text);
                        cmd.Parameters.Add("@fonecorporativo", SqlDbType.VarChar, 20).Value = DbString(txtFoneCorp.Text);

                        // CONTROLE
                        cmd.Parameters.Add("@empresa", SqlDbType.VarChar, 10).Value = "1111";
                        cmd.Parameters.Add("@dtcad", SqlDbType.DateTime).Value = DateTime.Now;
                        cmd.Parameters.Add("@usucad", SqlDbType.VarChar, 50).Value = DbString(nomeUsuario);
                        cmd.Parameters.Add("@situacao", SqlDbType.VarChar, 20).Value = "PROGRAMADA";
                        cmd.Parameters.Add("@dtottu", SqlDbType.VarChar, 20).Value = (object)sOTCliente ?? DBNull.Value;
                        cmd.Parameters.Add("@funcao", SqlDbType.VarChar, 30).Value = DbString(txtFuncao.Text);
                        cmd.Parameters.Add("@tomadorservico", SqlDbType.VarChar, 50).Value = DbString(primeiroNome);

                        // TRANSPORTADORA DO MOTORISTA
                        cmd.Parameters.Add("@codtranspmotorista", SqlDbType.VarChar, 20).Value = DbString(txtCodTransportadora.Text);
                        cmd.Parameters.Add("@nomtranspmotorista", SqlDbType.VarChar, 150).Value = DbString(txtTransportadora.Text);

                        // OUTRAS DATAS
                        cmd.Parameters.Add("@venccronotacografo", SqlDbType.Date).Value = DbDate(txtCrono.Text);
                        cmd.Parameters.Add("@valopacidade", SqlDbType.Date).Value = DbDate(txtOpacidade.Text);
                        
                        // DOCUMENTOS
                        cmd.Parameters.Add("@emissao", SqlDbType.DateTime).Value = DateTime.Parse(txtCadastro.Text).ToString("yyyy-MM-dd HH:mm");
                        cmd.Parameters.Add("@numero_gr", SqlDbType.VarChar, 30).Value = DbString(txtLiberacaoGR.Text);
                        cmd.Parameters.Add("@numero_protocolo_cet", SqlDbType.VarChar, 30).Value = DbString(txtNumProtCET.Text);
                        cmd.Parameters.Add("@cpf_cnpj_proprietario", SqlDbType.VarChar, 20).Value = DbString(txtCPF_CNPJ.Text);

                        // PEDÁGIO / EIXOS
                        cmd.Parameters.Add("@pedagio", SqlDbType.VarChar, 3).Value = DbString(txtPedagio.Text);
                        cmd.Parameters.Add("@pedagiofeito", SqlDbType.VarChar, 15).Value = DbString("Pendente");
                        cmd.Parameters.Add("@eixos", SqlDbType.Int).Value = DbInt(txtEixos.Text);

                        // EXPEDIDOR
                        cmd.Parameters.Add("@cod_expedidor", SqlDbType.Int).Value = DbInt(txtCod_Expedidor.Text);
                        cmd.Parameters.Add("@expedidor", SqlDbType.VarChar, 150).Value = DbString(txtExpedidor.Text);
                        cmd.Parameters.Add("@cid_expedidor", SqlDbType.VarChar, 50).Value = DbString(txtCid_Expedidor.Text);
                        cmd.Parameters.Add("@uf_expedidor", SqlDbType.VarChar, 2).Value = DbString(txtUf_Expedidor.Text);

                        // RECEBEDOR
                        cmd.Parameters.Add("@cod_recebedor", SqlDbType.Int).Value = DbInt(txtCod_Recebedor.Text);
                        cmd.Parameters.Add("@recebedor", SqlDbType.VarChar, 150).Value = DbString(txtRecebedor.Text);
                        cmd.Parameters.Add("@cid_recebedor", SqlDbType.VarChar, 50).Value = DbString(txtCid_Recebedor.Text);
                        cmd.Parameters.Add("@uf_recebedor", SqlDbType.VarChar, 2).Value = DbString(txtUf_Recebedor.Text);

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
                            cmd.Parameters.Add("@andamento", SqlDbType.VarChar).Value = "Programada";
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
        protected void btnPesquisarVeiculo_Click(object sender, EventArgs e)
        {
            if (txtCodMotorista.Text.Trim() == "")
            {
                //string nomeUsuario = txtUsuCadastro.Text;

                string linha1 = "Olá, " + Session["UsuarioLogado"].ToString() + "!";
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
                            //string nomeUsuario = txtUsuCadastro.Text;
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

                                //string nomeUsuario = txtUsuCadastro.Text;
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
                                        //string nomeUsuario = txtUsuCadastro.Text;
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
                                        // string nomeUsuario = txtUsuCadastro.Text;
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
                                        //string nomeUsuario = txtUsuCadastro.Text;
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
                                    // string nomeUsuario = txtUsuCadastro.Text;
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
                                    // string nomeUsuario = txtUsuCadastro.Text;
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
                                    // string nomeUsuario = txtUsuCadastro.Text;
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

                        // string nomeUsuario = txtUsuCadastro.Text;

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
                //CarregarFotoMotorista(fotoMotorista);
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
                txtTecnologia.Text = dtVeiculos.Rows[0]["rastreador"].ToString();
                txtRastreamento.Text = dtVeiculos.Rows[0]["rastreamento"].ToString();

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

            //Dados do proprietario
            string sqlProprietario = @"SELECT codtra, nomtra, cnpj
                   FROM tbtransportadoras 
                   WHERE codtra = @codtra";
            using (SqlCommand cmdProprietario = new SqlCommand(sqlProprietario, conn))
            {
                cmdProprietario.Parameters.AddWithValue("@codtra", txtCodProprietario.Text.Trim());

                DataTable dtProprietario = new DataTable();
                SqlDataAdapter daProprietario = new SqlDataAdapter(cmdProprietario);

                daProprietario.Fill(dtProprietario);

                if (dtProprietario.Rows.Count == 0)
                {
                    txtCPF_CNPJ.Text = "00.000.000/0000-00";
                }
                else
                {
                    txtCPF_CNPJ.Text = dtProprietario.Rows[0]["cnpj"].ToString();
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