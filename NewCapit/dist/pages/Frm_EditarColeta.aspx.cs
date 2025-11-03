using DAL;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using static NPOI.HSSF.Util.HSSFColor;

namespace NewCapit.dist.pages
{
    public partial class Frm_EditarColeta : System.Web.UI.Page
    {
        DateTime dataHoraAtual = DateTime.Now;
        string id;
        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString());
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UsuarioLogado"] != null)
                {
                    string nomeUsuario = Session["UsuarioLogado"].ToString();
                    var lblUsuario = nomeUsuario;

                    txtAlterado.Text = nomeUsuario;

                }
                else
                {
                    var lblUsuario = "<Usuário>";
                    //txtAlteradoPor.Text = lblUsuario;
                    Response.Redirect("Login.aspx");
                }

                
                PreencherComboFiliais();
                PreencherComboPlantas();
                PreencherComboVeicuos();
                CarregaDados();

            }        
           

        }
        private void PreencherComboFiliais()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT codigo, descricao FROM tbempresa";

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
                    cbFiliais.DataSource = reader;
                    cbFiliais.DataTextField = "descricao";  // Campo que será mostrado no ComboBox
                    cbFiliais.DataValueField = "codigo";  // Campo que será o valor de cada item                    
                    cbFiliais.DataBind();  // Realiza o binding dos dados                   
                    cbFiliais.Items.Insert(0, new ListItem("Selecione ...", "0"));
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
        private void PreencherComboPlantas()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT codigo, descricao FROM tbPlantavw";

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
                    ddlPlanta.DataSource = reader;
                    ddlPlanta.DataTextField = "descricao";  // Campo que será mostrado no ComboBox
                    ddlPlanta.DataValueField = "codigo";  // Campo que será o valor de cada item                    
                    ddlPlanta.DataBind();  // Realiza o binding dos dados                   
                    ddlPlanta.Items.Insert(0, new ListItem("Selecione ...", "0"));
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
        private void PreencherComboVeicuos()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT ID, descricao FROM tbtiposveiculoscnt";

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
                    ddlTipoVeiculo.DataSource = reader;
                    ddlTipoVeiculo.DataTextField = "descricao";  // Campo que será mostrado no ComboBox
                    ddlTipoVeiculo.DataValueField = "ID";  // Campo que será o valor de cada item                    
                    ddlTipoVeiculo.DataBind();  // Realiza o binding dos dados                   
                    ddlTipoVeiculo.Items.Insert(0, new ListItem("Selecione ...", "0"));
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
        protected void txtCodRemetente_TextChanged(object sender, EventArgs e)
        {
            if (txtCodRemetente.Text != "") 
            {

                string codigoRemetente = txtCodRemetente.Text.Trim();
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
                                txtCodRemetente.Text = reader["codcli"].ToString();
                                txtNomRemetente.Text = reader["nomcli"].ToString();
                                txtMunicipioRemetente.Text = reader["cidcli"].ToString();
                                txtUFRemetente.Text = reader["estcli"].ToString();
                                txtCodDestinatario.Focus();
                            }
                            else
                            {
                                txtNomRemetente.Text = "";
                                txtMunicipioRemetente.Text = "";
                                txtUFRemetente.Text = "";
                                txtCodRemetente.Text = "";
                                // Aciona o Toast via JavaScript
                                ScriptManager.RegisterStartupScript(this, GetType(), "toastNaoEncontrado", "mostrarToastNaoEncontrado();", true);
                                txtCodRemetente.Focus();
                                // Opcional: exibir mensagem ao usuário
                            }
                        }
                    }

                }

            }
            
            
        }
        protected void txtCodDestinatario_TextChanged(object sender, EventArgs e)
        {
            if (txtCodDestinatario.Text != "")
            {

                string codigoRemetente = txtCodDestinatario.Text.Trim();
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
                                txtCodDestinatario.Text = reader["codcli"].ToString();
                                txtNomDestinatario.Text = reader["nomcli"].ToString();
                                txtMunicipioDestinatario.Text = reader["cidcli"].ToString();
                                txtUFDestinatario.Text = reader["estcli"].ToString();
                                txtDataColeta.Focus();
                            }
                            else
                            {
                                txtNomDestinatario.Text = "";
                                txtMunicipioDestinatario.Text = "";
                                txtUFDestinatario.Text = "";
                                txtCodDestinatario.Text = "";
                                // Aciona o Toast via JavaScript
                                ScriptManager.RegisterStartupScript(this, GetType(), "toastNaoEncontrado", "mostrarToastNaoEncontrado();", true);
                                txtCodDestinatario.Focus();
                                
                            }
                        }
                    }

                }

            }


        }

        public void CarregaDados()
        {
            string nomeUsuario = Session["UsuarioLogado"].ToString();
            if (HttpContext.Current.Request.QueryString["id"].ToString() != "")
            {
                id = HttpContext.Current.Request.QueryString["id"].ToString();
            }
            DataTable dt = ConCargas.FetchDataTableColetas2(id);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                SqlDataAdapter adpto = new SqlDataAdapter("SELECT nomcli, cidcli, estcli, codcli FROM tbclientes WHERE codvw = @codvw", con);
                adpto.SelectCommand.Parameters.AddWithValue("@codvw", row["codvworigem"].ToString());
                DataTable dto = new DataTable();
                adpto.Fill(dto);

                SqlDataAdapter adptd = new SqlDataAdapter("SELECT nomcli, cidcli, estcli, codcli FROM tbclientes WHERE codvw = @codvw", con);
                adptd.SelectCommand.Parameters.AddWithValue("@codvw", row["codvwdestino"].ToString());
                DataTable dtd = new DataTable();
                adptd.Fill(dtd);

               
                txtColeta.Text = row["carga"].ToString();
                txtSituacao.Text = row["status"].ToString(); 
                txtCodRemetente.Text = dto.Rows[0][3].ToString();
                txtNomRemetente.Text = dto.Rows[0][0].ToString();
                txtMunicipioRemetente.Text = dto.Rows[0][1].ToString();
                txtUFRemetente.Text = dto.Rows[0][2].ToString();

                txtCodDestinatario.Text = dtd.Rows[0][3].ToString();
                txtNomDestinatario.Text = dtd.Rows[0][0].ToString();
                txtMunicipioDestinatario.Text = dtd.Rows[0][1].ToString();
                txtUFDestinatario.Text = dtd.Rows[0][2].ToString();

                txtDataColeta.Text = row["emissao"].ToString();
                txtSolicitacoes.Text = row["solicitacoes"].ToString();
                txtPeso.Text = row["peso"].ToString();
                txtMetragem.Text = row["pedidos"].ToString();

                txtViagem.Text = row["tipo_viagem"].ToString();
                txtRota.Text = row["rota"].ToString();
                txtEstudoRota.Text = row["estudo_rota"].ToString();
                txtRemessa.Text = row["remessa"].ToString();

                txtHistorico.Text = row["quant_palet"].ToString();
                lblNumerocva.Text = "- CVA Nº "+ row["cva"].ToString();



                // Labels
                string[] partes = row["cadastro"].ToString().Split(new string[] { " - " }, StringSplitOptions.None);

                if (partes.Length == 2)
                {
                    string dataHora = partes[0]; // "08/05/2025 20:22"
                    string nome = partes[1];     // "Genildo Santos"

                    txtUsuCadastro.Text = nome;
                    lblDtCadastro.Text = DateTime.Now.ToString(dataHora);
                    
                }
                if(row["atualizacao"].ToString() != string.Empty)
                {
                    string[] partes2 = row["atualizacao"].ToString().Split(new string[] { " - " }, StringSplitOptions.None);

                    if (partes.Length == 2)
                    {
                        string dataHora2 = partes[0]; // "08/05/2025 20:22"
                        string nome2 = partes[1];     // "Genildo Santos"

                        txtUsuAlteracao.Text = nome2;
                        lblDtAlteracao.Text = DateTime.Now.ToString(dataHora2);

                    }
                }
                else
                {
                    txtUsuAlteracao.Text = nomeUsuario;
                    lblDtAlteracao.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                }
                

                // DropDownLists (exemplo com dados fictícios)
               
                cbFiliais.SelectedItem.Text= row["empresa"].ToString();
                

                
                ddlPlanta.SelectedItem.Text = row["tomador"].ToString();
                //ddlPlanta.Items.Add(new ListItem("Planta A", "A"));
                //ddlPlanta.Items.Add(new ListItem("Planta B", "B"));


                ddlTipoVeiculo.SelectedItem.Text = row["veiculo"].ToString();
               
            }

        }

        protected void btnAlterar_Click(object sender, EventArgs e)
        {
            string nomeUsuario = Session["UsuarioLogado"].ToString();
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                string query = @"
            UPDATE tbcargas SET
                status = @status,
                codorigem = @codRemetente,
                cliorigem = @nomRemetente,
                cidorigem = @municipioRemetente,
                coddestino = @codDestinatario,
                clidestino = @nomDestinatario,
                ciddestino = @municipioDestinatario,
                emissao = @dataColeta,
                solicitacoes = @solicitacoes,
                peso = @peso,
                pedidos = @metragem,
                tipo_viagem = @tipoViagem,
                rota = @rota,
                estudo_rota = @estudoRota,
                remessa = @remessa,
                quant_palet = @historico,
                empresa = @empresa,
                tomador = @tomador,
                veiculo = @veiculo,
                codvworigem =@codvworigem,
                codvwdestino=@codvwdestino,
                atualizacao = @atualizacao
            WHERE carga = @carga";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    SqlDataAdapter adpto = new SqlDataAdapter("SELECT nomcli, cidcli, estcli, codvw FROM tbclientes WHERE codcli = @codcliorigem", con);
                    adpto.SelectCommand.Parameters.AddWithValue("@codcliorigem", txtCodRemetente.Text.Trim());
                    DataTable dto = new DataTable();
                    adpto.Fill(dto);

                    SqlDataAdapter adptd = new SqlDataAdapter("SELECT nomcli, cidcli, estcli, codvw FROM tbclientes WHERE codcli = @codclidestino", con);
                    adptd.SelectCommand.Parameters.AddWithValue("@codclidestino", txtCodDestinatario.Text.Trim());
                    DataTable dtd = new DataTable();
                    adptd.Fill(dtd);
                    cmd.Parameters.AddWithValue("@status", txtSituacao.Text);
                    cmd.Parameters.AddWithValue("@codRemetente", txtCodRemetente.Text.Trim());
                    cmd.Parameters.AddWithValue("@nomRemetente", dto.Rows[0][0].ToString());
                    cmd.Parameters.AddWithValue("@municipioRemetente", dto.Rows[0][1].ToString());
                    cmd.Parameters.AddWithValue("@codvworigem", dto.Rows[0][3].ToString());

                    cmd.Parameters.AddWithValue("@codDestinatario", txtCodDestinatario.Text.Trim());
                    cmd.Parameters.AddWithValue("@nomDestinatario", dtd.Rows[0][0].ToString());
                    cmd.Parameters.AddWithValue("@municipioDestinatario", dtd.Rows[0][1].ToString());
                    cmd.Parameters.AddWithValue("@codvwdestino", dtd.Rows[0][3].ToString());

                    cmd.Parameters.AddWithValue("@dataColeta", txtDataColeta.Text);
                    cmd.Parameters.AddWithValue("@solicitacoes", txtSolicitacoes.Text);
                    cmd.Parameters.AddWithValue("@peso", txtPeso.Text);
                    cmd.Parameters.AddWithValue("@metragem", txtMetragem.Text);

                    cmd.Parameters.AddWithValue("@tipoViagem", txtViagem.Text);
                    cmd.Parameters.AddWithValue("@rota", txtRota.Text);
                    cmd.Parameters.AddWithValue("@estudoRota", txtEstudoRota.Text);
                    cmd.Parameters.AddWithValue("@remessa", txtRemessa.Text);

                    cmd.Parameters.AddWithValue("@historico", txtHistorico.Text);

                    cmd.Parameters.AddWithValue("@empresa", cbFiliais.SelectedItem.Text);
                    cmd.Parameters.AddWithValue("@tomador", ddlPlanta.SelectedItem.Text);
                    cmd.Parameters.AddWithValue("@veiculo", ddlTipoVeiculo.SelectedItem.Text);

                    // Atualização: nome + data/hora atual
                    string atualizacao = DateTime.Now.ToString("dd/MM/yyyy HH:mm") + " - " + nomeUsuario;
                    cmd.Parameters.AddWithValue("@atualizacao", atualizacao);

                    cmd.Parameters.AddWithValue("@carga", txtColeta.Text);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    Response.Redirect("ConsultaColetasCNT.aspx");
                }
            }
        }
    }

}