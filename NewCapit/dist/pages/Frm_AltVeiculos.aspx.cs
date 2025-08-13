using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using NPOI.SS.Formula.Functions;
using static System.Net.Mime.MediaTypeNames;
using static NPOI.HSSF.Util.HSSFColor;

namespace NewCapit
{
    public partial class Frm_AltVeiculos : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conexao"].ToString());
        string id;
        string id_uf;  
        DateTime dataHoraAtual = DateTime.Now;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                if (Session["UsuarioLogado"] != null)
                {
                    string nomeUsuario = Session["UsuarioLogado"].ToString();
                    var lblUsuario = nomeUsuario;

                    // txtAlteradoPor.Text = nomeUsuario;
                    txtUsuarioAtual.Text = nomeUsuario;
                }
                else
                {
                    var lblUsuario = "<Usuário>";
                    // txtAlteradoPor.Text = lblUsuario;
                }
                PreencherComboEstados();                
                PreencherComboComposicao();
                CarregarDDLAgregados();
                PreencherComboFiliais();
                PreencherComboMarcasVeiculos();
                PreencherComboCoresVeiculos();
                PreencherComboRastreadores();
                PreencherComboMotoristas();
                PreencherComboCboTipo();

                CarregaDadosDoVeiculo();


                //DateTime dataHoraAtual = DateTime.Now;
                // txtDtAlteracao.Text = dataHoraAtual.ToString("dd/MM/yyyy HH:mm");

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
        private void PreencherComboMarcasVeiculos()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT id, marca FROM tbmarcasveiculos";

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
                    ddlMarca.DataSource = reader;
                    ddlMarca.DataTextField = "marca";  // Campo que será mostrado no ComboBox
                    ddlMarca.DataValueField = "id";  // Campo que será o valor de cada item                    
                    ddlMarca.DataBind();  // Realiza o binding dos dados                   

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
        private void PreencherComboCoresVeiculos()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT id, cor FROM tbcores";

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
                    ddlCor.DataSource = reader;
                    ddlCor.DataTextField = "cor";  // Campo que será mostrado no ComboBox
                    ddlCor.DataValueField = "id";  // Campo que será o valor de cada item                    
                    ddlCor.DataBind();  // Realiza o binding dos dados                   

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
        private void CarregarCidades(string uf)
        {
            string strConn = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                string query = "SELECT cod_municipio, nome_municipio FROM tbmunicipiosbrasileiros WHERE uf = @UF";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UF", uf);
                conn.Open();
                ddlCidades.DataSource = cmd.ExecuteReader();
                ddlCidades.DataTextField = "nome_municipio";
                ddlCidades.DataValueField = "cod_municipio"; // valor único
                ddlCidades.DataBind();

                //ddlCidades.Items.Insert(0, new ListItem("-- Selecione uma cidade --", "0"));
            }
        }
        protected void ddlEstados_SelectedIndexChanged(object sender, EventArgs e)
        {
            string uf = ddlEstados.SelectedValue;
            CarregarCidades(uf);

            // Restaurar cidade se estiver em ViewState
            if (ViewState["CidadeSelecionada"] != null)
            {
                string cidadeId = ViewState["CidadeSelecionada"].ToString();
                if (ddlCidades.Items.FindByValue(cidadeId) != null)
                {
                    ddlCidades.SelectedValue = cidadeId;
                }
            }
        }
        private void PreencherComboRastreadores()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT codRastreador, nomRastreador, codBuonny FROM tbrastreadores";

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
                    ddlTecnologia.DataSource = reader;
                    ddlTecnologia.DataTextField = "nomRastreador";  // Campo que será mostrado no ComboBox
                    ddlTecnologia.DataValueField = "codRastreador";  // Campo que será o valor de cada item                    
                    ddlTecnologia.DataBind();  // Realiza o binding dos dados                   

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
        private void PreencherComboCboTipo()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT id, descricao FROM tbtipos_veiculos order by descricao";

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
                    cboTipo.DataSource = reader;
                    cboTipo.DataTextField = "descricao";  // Campo que será mostrado no ComboBox
                    cboTipo.DataValueField = "id";  // Campo que será o valor de cada item                    
                    cboTipo.DataBind();  // Realiza o binding dos dados                   

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
        private void PreencherComboEstados()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT Uf, SiglaUf FROM tbestadosbrasileiros";

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
                    ddlEstados.DataSource = reader;
                    ddlEstados.DataTextField = "SiglaUf";  // Campo que será mostrado no ComboBox
                    ddlEstados.DataValueField = "Uf";  // Campo que será o valor de cada item                    
                    ddlEstados.DataBind();  // Realiza o binding dos dados                   
                                            //ddlEstados.Items.Insert(0, new ListItem("", "0"));
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
        protected void ddlTecnologia_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtCodRastreador.Text = ddlTecnologia.SelectedValue.ToString();
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
                    // Trate exceções
                    Response.Write("Erro: " + ex.Message);
                }
            }
        }
        //private void cboTipoCarreta_Leave()
        //{
        //    string composicao1 = "CAVALO SIMPLES COM CARRETA VANDERLEIA ABERTA";
        //    string composicao2 = "CAVALO SIMPLES COM CARRETA SIMPLES TOTAL SIDER";
        //    string composicao3 = "CAVALO SIMPLES COM CARRETA SIMPLES(LS) ABERTA";
        //    string composicao4 = "CAVALO SIMPLES COM CARRETA VANDERLEIA TOTAL SIDER";
        //    string composicao5 = "CAVALO TRUCADO COM CARRETA VANDERLEIA ABERTA";
        //    string composicao6 = "CAVALO TRUCADO COM CARRETA SIMPLES TOTAL SIDER";
        //    string composicao7 = "CAVALO TRUCADO COM CARRETA SIMPLES(LS) ABERTA";
        //    string composicao8 = "CAVALO TRUCADO COM CARRETA VANDERLEIA TOTAL SIDER";
        //    string composicao9 = "TRUCK";
        //    string composicao10 = "BITRUCK";
        //    string composicao11 = "BITREM";
        //    string composicao12 = "TOCO";
        //    string composicao13 = "VUC OU 3/4";
        //    string composicao14 = "CAVALO SIMPLES COM PRANCHA";
        //    string composicao15 = "CAVALO TRUCADO COM PRANCHA";
        //    string composicao16 = "UTILITÁRIO/FURGÃO";
        //    string composicao17 = "FIORINO";

        //    string selectedValue = ddlComposicao.SelectedItem.ToString().Trim();
        //    string tipoComposicao = selectedValue;
        //    string tara = txtTara.Text.Trim();
        //    int nTara = 0;
        //    if (tipoComposicao.Equals(composicao1))
        //    {
        //        txtEixos.Text = "05";
        //        txtLotacao.Text = "46000";
        //        txtTolerancia.Text = "5";
        //        int nCapacidade = 46000;
        //        nTara = int.Parse(txtTara.Text);
        //        int nPesoLiquido = nCapacidade - nTara;
        //        int nPesoTolerancia = (nPesoLiquido * 5) / 100;
        //        int nTotalCarga = nPesoLiquido + nPesoTolerancia;
        //        txtPBT.Text = nTotalCarga.ToString();
        //    }
        //    else if (tipoComposicao.Equals(composicao2))
        //    {
        //        txtEixos.Text = "05";
        //        txtLotacao.Text = "46000";
        //        txtTolerancia.Text = "5";
        //        int nCapacidade = 41500;
        //        nTara = int.Parse(txtTara.Text);
        //        int nPesoLiquido = nCapacidade - nTara;
        //        int nPesoTolerancia = (nPesoLiquido * 5) / 100;
        //        int nTotalCarga = nPesoLiquido + nPesoTolerancia;
        //        txtPBT.Text = nTotalCarga.ToString();
        //    }
        //    else if (tipoComposicao.Equals(composicao3))
        //    {
        //        txtEixos.Text = "05";
        //        txtLotacao.Text = "41500";
        //        txtTolerancia.Text = "5";
        //        int nCapacidade = 46000;
        //        nTara = int.Parse(txtTara.Text);
        //        int nPesoLiquido = nCapacidade - nTara;
        //        int nPesoTolerancia = (nPesoLiquido * 5) / 100;
        //        int nTotalCarga = nPesoLiquido + nPesoTolerancia;
        //        txtPBT.Text = nTotalCarga.ToString();
        //    }
        //    else if (tipoComposicao.Equals(composicao4))
        //    {
        //        txtEixos.Text = "05";
        //        txtLotacao.Text = "46000";
        //        txtTolerancia.Text = "5";
        //        int nCapacidade = 46000;
        //        nTara = int.Parse(txtTara.Text);
        //        int nPesoLiquido = nCapacidade - nTara;
        //        int nPesoTolerancia = (nPesoLiquido * 5) / 100;
        //        int nTotalCarga = nPesoLiquido + nPesoTolerancia;
        //        txtPBT.Text = nTotalCarga.ToString();
        //    }
        //    else if (tipoComposicao.Equals(composicao5))
        //    {

        //        txtEixos.Text = "06";
        //        txtLotacao.Text = "53000";
        //        txtTolerancia.Text = "5";
        //        int nCapacidade = 53000;
        //        nTara = Int32.Parse(tara);
        //        int nPesoLiquido = nCapacidade - nTara;
        //        int nPesoTolerancia = (nPesoLiquido * 5) / 100;

        //    }
        //    else if (tipoComposicao.Equals(composicao6))
        //    {
        //        txtEixos.Text = "06";
        //        txtLotacao.Text = "48500";
        //        txtTolerancia.Text = "5";
        //        int nCapacidade = 48500;
        //        nTara = int.Parse(txtTara.Text);
        //        int nPesoLiquido = nCapacidade - nTara;
        //        int nPesoTolerancia = (nPesoLiquido * 5) / 100;

        //    }
        //    else if (tipoComposicao.Equals(composicao7))
        //    {
        //        txtEixos.Text = "06";
        //        txtLotacao.Text = "48500";
        //        txtTolerancia.Text = "5";
        //        int nCapacidade = 48500;
        //        nTara = int.Parse(txtTara.Text);
        //        int nPesoLiquido = nCapacidade - nTara;
        //        int nPesoTolerancia = (nPesoLiquido * 5) / 100;

        //    }
        //    else if (tipoComposicao.Equals(composicao8))
        //    {
        //        txtEixos.Text = "06";
        //        txtLotacao.Text = "53000";
        //        txtTolerancia.Text = "5";
        //        int nCapacidade = 53000;
        //        nTara = int.Parse(txtTara.Text);
        //        int nPesoLiquido = nCapacidade - nTara;
        //        int nPesoTolerancia = (nPesoLiquido * 5) / 100;

        //    }
        //    else if (tipoComposicao.Equals(composicao9))
        //    {
        //        txtEixos.Text = "03";
        //        txtLotacao.Text = "23000";
        //        txtTolerancia.Text = "5";
        //        int nCapacidade = 23000;
        //        nTara = int.Parse(txtTara.Text);
        //        int nPesoLiquido = nCapacidade - nTara;
        //        int nPesoTolerancia = (nPesoLiquido * 5) / 100;

        //    }
        //    else if (tipoComposicao.Equals(composicao10))
        //    {
        //        txtEixos.Text = "04";
        //        txtLotacao.Text = "29000";
        //        txtTolerancia.Text = "5";
        //        int nCapacidade = 29000;
        //        nTara = int.Parse(txtTara.Text);
        //        int nPesoLiquido = nCapacidade - nTara;
        //        int nPesoTolerancia = (nPesoLiquido * 5) / 100;

        //    }
        //    else if (tipoComposicao.Equals(composicao11))
        //    {
        //        txtEixos.Text = "07";
        //        txtLotacao.Text = "57000";
        //        txtTolerancia.Text = "5";
        //        int nCapacidade = 57000;
        //        nTara = int.Parse(txtTara.Text);
        //        int nPesoLiquido = nCapacidade - nTara;
        //        int nPesoTolerancia = (nPesoLiquido * 5) / 100;

        //    }
        //    else if (tipoComposicao.Equals(composicao12))
        //    {
        //        txtEixos.Text = "02";
        //        txtLotacao.Text = "16000";
        //        txtTolerancia.Text = "5";
        //        int nCapacidade = 16000;
        //        nTara = int.Parse(txtTara.Text);
        //        int nPesoLiquido = nCapacidade - nTara;
        //        int nPesoTolerancia = (nPesoLiquido * 5) / 100;

        //    }
        //    else if (tipoComposicao.Equals(composicao13))
        //    {
        //        txtEixos.Text = "06";
        //        txtLotacao.Text = "3000";
        //        txtTolerancia.Text = "5";
        //        int nCapacidade = 3000;
        //        nTara = int.Parse(txtTara.Text);
        //        int nPesoLiquido = nCapacidade - nTara;
        //        int nPesoTolerancia = (nPesoLiquido * 5) / 100;

        //    }
        //    else if (tipoComposicao.Equals(composicao14))
        //    {
        //        txtEixos.Text = "05";
        //        txtLotacao.Text = "23000";
        //        txtTolerancia.Text = "5";
        //        int nCapacidade = 23000;
        //        nTara = int.Parse(txtTara.Text);
        //        int nPesoLiquido = nCapacidade - nTara;
        //        int nPesoTolerancia = (nPesoLiquido * 5) / 100;

        //    }
        //    else if (tipoComposicao.Equals(composicao15))
        //    {
        //        txtEixos.Text = "06";
        //        txtLotacao.Text = "23000";
        //        txtTolerancia.Text = "5";
        //        int nCapacidade = 23000;
        //        nTara = int.Parse(txtTara.Text);
        //        int nPesoLiquido = nCapacidade - nTara;
        //        int nPesoTolerancia = (nPesoLiquido * 5) / 100;

        //    }
        //    else if (tipoComposicao.Equals(composicao16))
        //    {
        //        txtEixos.Text = "02";
        //        txtLotacao.Text = "1200";
        //        txtTolerancia.Text = "5";
        //        int nCapacidade = 1200;
        //        nTara = int.Parse(txtTara.Text);
        //        int nPesoLiquido = nCapacidade - nTara;
        //        int nPesoTolerancia = (nPesoLiquido * 5) / 100;

        //    }
        //    else if (tipoComposicao.Equals(composicao17))
        //    {
        //        txtEixos.Text = "02";
        //        txtLotacao.Text = "630";
        //        txtTolerancia.Text = "5";
        //        int nCapacidade = 630;
        //        nTara = int.Parse(txtTara.Text);
        //        int nPesoLiquido = nCapacidade - nTara;
        //        int nPesoTolerancia = (nPesoLiquido * 5) / 100;

        //    }
        //}
        //protected void ddlComposicao_SelectedIndexChanged2(object sender, EventArgs e)
        //{
        //    string composicao1 = "CAVALO SIMPLES COM CARRETA VANDERLEIA ABERTA";
        //    string composicao2 = "CAVALO SIMPLES COM CARRETA SIMPLES TOTAL SIDER";
        //    string composicao3 = "CAVALO SIMPLES COM CARRETA SIMPLES(LS) ABERTA";
        //    string composicao4 = "CAVALO SIMPLES COM CARRETA VANDERLEIA TOTAL SIDER";
        //    string composicao5 = "CAVALO TRUCADO COM CARRETA VANDERLEIA ABERTA";
        //    string composicao6 = "CAVALO TRUCADO COM CARRETA SIMPLES TOTAL SIDER";
        //    string composicao7 = "CAVALO TRUCADO COM CARRETA SIMPLES(LS) ABERTA";
        //    string composicao8 = "CAVALO TRUCADO COM CARRETA VANDERLEIA TOTAL SIDER";
        //    string composicao9 = "TRUCK";
        //    string composicao10 = "BITRUCK";
        //    string composicao11 = "BITREM";
        //    string composicao12 = "TOCO";
        //    string composicao13 = "VUC OU 3/4";
        //    string composicao14 = "CAVALO SIMPLES COM PRANCHA";
        //    string composicao15 = "CAVALO TRUCADO COM PRANCHA";
        //    string composicao16 = "UTILITÁRIO/FURGÃO";
        //    string composicao17 = "FIORINO";

        //    string selectedValue = ddlComposicao.SelectedItem.ToString().Trim();
        //    string tipoComposicao = selectedValue;
        //    string tara = txtTara.Text;

        //    int nTara = 0;
        //    if (tipoComposicao.Equals(composicao1))
        //    {
        //        txtEixos.Text = "05";
        //        txtLotacao.Text = "46000";
        //        txtTolerancia.Text = "5";
        //        int nCapacidade = 46000;
        //        nTara = int.Parse(txtTara.Text);
        //        int nPesoLiquido = nCapacidade - nTara;
        //        int nPesoTolerancia = (nPesoLiquido * 5) / 100;
        //        int nTotalCarga = nPesoLiquido + nPesoTolerancia;
        //        txtPBT.Text = nTotalCarga.ToString();
        //    }
        //    else if (tipoComposicao.Equals(composicao2))
        //    {
        //        txtEixos.Text = "05";
        //        txtLotacao.Text = "46000";
        //        txtTolerancia.Text = "5";
        //        int nCapacidade = 41500;
        //        nTara = int.Parse(txtTara.Text);
        //        int nPesoLiquido = nCapacidade - nTara;
        //        int nPesoTolerancia = (nPesoLiquido * 5) / 100;
        //        int nTotalCarga = nPesoLiquido + nPesoTolerancia;
        //        txtPBT.Text = nTotalCarga.ToString();
        //    }
        //    else if (tipoComposicao.Equals(composicao3))
        //    {
        //        txtEixos.Text = "05";
        //        txtLotacao.Text = "41500";
        //        txtTolerancia.Text = "5";
        //        int nCapacidade = 46000;
        //        nTara = int.Parse(txtTara.Text);
        //        int nPesoLiquido = nCapacidade - nTara;
        //        int nPesoTolerancia = (nPesoLiquido * 5) / 100;
        //        int nTotalCarga = nPesoLiquido + nPesoTolerancia;
        //        txtPBT.Text = nTotalCarga.ToString();
        //    }
        //    else if (tipoComposicao.Equals(composicao4))
        //    {
        //        txtEixos.Text = "05";
        //        txtLotacao.Text = "46000";
        //        txtTolerancia.Text = "5";
        //        int nCapacidade = 46000;
        //        nTara = int.Parse(txtTara.Text);
        //        int nPesoLiquido = nCapacidade - nTara;
        //        int nPesoTolerancia = (nPesoLiquido * 5) / 100;
        //        int nTotalCarga = nPesoLiquido + nPesoTolerancia;
        //        txtPBT.Text = nTotalCarga.ToString();
        //    }
        //    else if (tipoComposicao.Equals(composicao5))
        //    {
        //        tara = txtTara.Text.Trim();
        //        txtEixos.Text = "06";
        //        txtLotacao.Text = "53000";
        //        txtTolerancia.Text = "5";
        //        int nCapacidade = 53000;
        //        nTara = int.Parse(tara);
        //        int nPesoLiquido = nCapacidade - nTara;
        //        int nPesoTolerancia = (nPesoLiquido * 5) / 100;

        //    }
        //    else if (tipoComposicao.Equals(composicao6))
        //    {
        //        txtEixos.Text = "06";
        //        txtLotacao.Text = "48500";
        //        txtTolerancia.Text = "5";
        //        int nCapacidade = 48500;
        //        nTara = int.Parse(txtTara.Text);
        //        int nPesoLiquido = nCapacidade - nTara;
        //        int nPesoTolerancia = (nPesoLiquido * 5) / 100;

        //    }
        //    else if (tipoComposicao.Equals(composicao7))
        //    {
        //        txtEixos.Text = "06";
        //        txtLotacao.Text = "48500";
        //        txtTolerancia.Text = "5";
        //        int nCapacidade = 48500;
        //        nTara = int.Parse(txtTara.Text);
        //        int nPesoLiquido = nCapacidade - nTara;
        //        int nPesoTolerancia = (nPesoLiquido * 5) / 100;

        //    }
        //    else if (tipoComposicao.Equals(composicao8))
        //    {
        //        txtEixos.Text = "06";
        //        txtLotacao.Text = "53000";
        //        txtTolerancia.Text = "5";
        //        int nCapacidade = 53000;
        //        nTara = int.Parse(txtTara.Text);
        //        int nPesoLiquido = nCapacidade - nTara;
        //        int nPesoTolerancia = (nPesoLiquido * 5) / 100;

        //    }
        //    else if (tipoComposicao.Equals(composicao9))
        //    {
        //        txtEixos.Text = "03";
        //        txtLotacao.Text = "23000";
        //        txtTolerancia.Text = "5";
        //        int nCapacidade = 23000;
        //        nTara = int.Parse(txtTara.Text);
        //        int nPesoLiquido = nCapacidade - nTara;
        //        int nPesoTolerancia = (nPesoLiquido * 5) / 100;

        //    }
        //    else if (tipoComposicao.Equals(composicao10))
        //    {
        //        txtEixos.Text = "04";
        //        txtLotacao.Text = "29000";
        //        txtTolerancia.Text = "5";
        //        int nCapacidade = 29000;
        //        nTara = int.Parse(txtTara.Text);
        //        int nPesoLiquido = nCapacidade - nTara;
        //        int nPesoTolerancia = (nPesoLiquido * 5) / 100;

        //    }
        //    else if (tipoComposicao.Equals(composicao11))
        //    {
        //        txtEixos.Text = "07";
        //        txtLotacao.Text = "57000";
        //        txtTolerancia.Text = "5";
        //        int nCapacidade = 57000;
        //        nTara = int.Parse(txtTara.Text);
        //        int nPesoLiquido = nCapacidade - nTara;
        //        int nPesoTolerancia = (nPesoLiquido * 5) / 100;

        //    }
        //    else if (tipoComposicao.Equals(composicao12))
        //    {
        //        txtEixos.Text = "02";
        //        txtLotacao.Text = "16000";
        //        txtTolerancia.Text = "5";
        //        int nCapacidade = 16000;
        //        nTara = int.Parse(txtTara.Text);
        //        int nPesoLiquido = nCapacidade - nTara;
        //        int nPesoTolerancia = (nPesoLiquido * 5) / 100;

        //    }
        //    else if (tipoComposicao.Equals(composicao13))
        //    {
        //        txtEixos.Text = "02";
        //        txtLotacao.Text = "3000";
        //        txtTolerancia.Text = "5";
        //        int nCapacidade = 3000;
        //        nTara = int.Parse(txtTara.Text);
        //        int nPesoLiquido = nCapacidade - nTara;
        //        int nPesoTolerancia = (nPesoLiquido * 5) / 100;

        //    }
        //    else if (tipoComposicao.Equals(composicao14))
        //    {
        //        txtEixos.Text = "05";
        //        txtLotacao.Text = "23000";
        //        txtTolerancia.Text = "5";
        //        int nCapacidade = 23000;
        //        nTara = int.Parse(txtTara.Text);
        //        int nPesoLiquido = nCapacidade - nTara;
        //        int nPesoTolerancia = (nPesoLiquido * 5) / 100;

        //    }
        //    else if (tipoComposicao.Equals(composicao15))
        //    {
        //        txtEixos.Text = "06";
        //        txtLotacao.Text = "23000";
        //        txtTolerancia.Text = "5";
        //        int nCapacidade = 23000;
        //        nTara = int.Parse(txtTara.Text);
        //        int nPesoLiquido = nCapacidade - nTara;
        //        int nPesoTolerancia = (nPesoLiquido * 5) / 100;

        //    }
        //    else if (tipoComposicao.Equals(composicao16))
        //    {
        //        txtEixos.Text = "02";
        //        txtLotacao.Text = "1200";
        //        txtTolerancia.Text = "5";
        //        int nCapacidade = 1200;
        //        nTara = int.Parse(txtTara.Text);
        //        int nPesoLiquido = nCapacidade - nTara;
        //        int nPesoTolerancia = (nPesoLiquido * 5) / 100;

        //    }
        //    else if (tipoComposicao.Equals(composicao17))
        //    {
        //        txtEixos.Text = "02";
        //        txtLotacao.Text = "630";
        //        txtTolerancia.Text = "5";
        //        int nCapacidade = 630;
        //        nTara = int.Parse(txtTara.Text);
        //        int nPesoLiquido = nCapacidade - nTara;
        //        int nPesoTolerancia = (nPesoLiquido * 5) / 100;

        //    }

        //}
        private void PreencherComboComposicao()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT ID, composicao, eixos, pbt, tolerancia FROM tbcomposicaoveiculo";

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
                    ddlComposicao.DataSource = reader;
                    ddlComposicao.DataTextField = "composicao";  // Campo que será mostrado no ComboBox
                    ddlComposicao.DataValueField = "ID";  // Campo que será o valor de cada item                    
                    ddlComposicao.DataBind();  // Realiza o binding dos dados                   
                    ddlComposicao.Items.Insert(0, new ListItem("Selecione...", "0"));
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
        public void CarregaDadosDoVeiculo()
        {

            if (HttpContext.Current.Request.QueryString["id"].ToString() != "")
            {
                id = HttpContext.Current.Request.QueryString["id"].ToString();
            }

            string sql = "SELECT * FROM tbveiculos WHERE id = @id";

            try
            {
                using (SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    DataTable dt = new DataTable();

                    using (SqlDataAdapter adpt = new SqlDataAdapter(cmd))
                    {
                        con.Open();
                        adpt.Fill(dt);
                    }

                    // Verifica se encontrou resultados
                    if (dt.Rows.Count == 0)
                    {
                        // Log ou mensagem indicando que não encontrou o registro
                        return;
                    }

                    DataRow row = dt.Rows[0];

                    // Método auxiliar para evitar exceções de valores nulos
                    string GetValue(DataRow r, int index) => r[index] == DBNull.Value ? string.Empty : r[index].ToString();

                    txtCodVei.Text = GetValue(row, 1); 
                    if (dt.Rows[0][2].ToString() != "")
                    {                        
                        //cboTipo.Items.Insert(0, new ListItem(GetValue(row, 2)));
                        cboTipo.SelectedItem.Text = GetValue(row, 2);
                        if (cboTipo.SelectedItem.Text.Trim() == "TRUCK" || cboTipo.SelectedItem.Text.Trim() == "BITRUCK" || cboTipo.SelectedItem.Text.Trim() == "UTILITÁRIO/FURGÃO" || cboTipo.SelectedItem.Text.Trim() == "LEVE" || cboTipo.SelectedItem.Text.Trim() == "FIORINO" || cboTipo.SelectedItem.Text.Trim() == "TOCO" || cboTipo.SelectedItem.Text.Trim() == "VUC OU 3/4")
                        {
                            carreta.Visible = false;
                            pnlDivReboque1.Visible = false;
                            pnlDivReboque2.Visible = false;
                            
                        }
                        if (cboTipo.SelectedItem.Text.Trim() == "BITREM")
                        {
                            carreta.Visible = true;
                            pnlDivReboque1.Visible = true;
                            pnlDivReboque2.Visible = true;
                            
                        }
                        if (cboTipo.SelectedItem.Text.Trim() == "CAVALO SIMPLES" || cboTipo.SelectedItem.Text.Trim() == "CAVALO TRUCADO" || cboTipo.SelectedItem.Text.Trim() == "CAVALO 4 EIXOS")
                        {
                            carreta.Visible = true;
                            pnlDivReboque1.Visible = true;
                            pnlDivReboque2.Visible = false;
                            
                        }

                    }

                    if (dt.Rows[0][3].ToString() != string.Empty)
                    {
                        ddlTipo.Items.Insert(0, GetValue(row, 3));
                    }
                    if (dt.Rows[0][4].ToString() != string.Empty)
                    {
                        txtModelo.Text = GetValue(row, 4);
                    }
                    if (dt.Rows[0][5].ToString() != string.Empty)
                    {
                        txtAno.Text = GetValue(row, 5);
                    }
                    if (dt.Rows[0][6].ToString() != string.Empty)
                    {
                        txtDtcVei.Text = DateTime.Parse(dt.Rows[0][6].ToString()).ToString("dd/MM/yyyy");
                    }
                    if (dt.Rows[0][7].ToString() != string.Empty)
                    {
                        cbFiliais.Items.Insert(0, GetValue(row, 7));
                    }
                    if (dt.Rows[0][8].ToString() != string.Empty)
                    {
                        ddlSituacao.Items.Insert(0, GetValue(row, 8));
                    }
                    if (dt.Rows[0][9].ToString() != string.Empty)
                    {
                        txtPlaca.Text = GetValue(row, 9);
                    }
                    if (dt.Rows[0][10].ToString() != string.Empty)
                    {
                        txtReb1.Text = GetValue(row, 10);
                    }
                    if (dt.Rows[0][11].ToString() != string.Empty)
                    {
                        txtReb2.Text = GetValue(row, 11);
                    }

                    if (dt.Rows[0][12].ToString() != string.Empty)
                    {
                        ddlComposicao.Items.Insert(0, GetValue(row, 12));
                    }
                    if (dt.Rows[0][13].ToString() != string.Empty)
                    {
                        ddlCarreta.Items.Insert(0, GetValue(row, 13));
                    }
                    if (dt.Rows[0][14].ToString() != string.Empty)
                    {
                        ddlMonitoramento.Items.Insert(0, GetValue(row, 14));
                    }
                    if (dt.Rows[0][15].ToString() != string.Empty)
                    {
                        txtCodRastreador.Text = GetValue(row, 15);
                    }
                    if (dt.Rows[0][16].ToString() != string.Empty)
                    {
                        ddlTecnologia.Items.Insert(0, GetValue(row, 16));
                    }
                    if (dt.Rows[0][17].ToString() != string.Empty)
                    {
                        txtId.Text = GetValue(row, 17);
                    }
                    if (dt.Rows[0][18].ToString() != string.Empty)
                    {
                        txtCargaLiq.Text = GetValue(row, 18);
                    }
                    if (dt.Rows[0][19].ToString() != string.Empty)
                    {
                        txtEixos.Text = GetValue(row, 19);
                    }
                    if (dt.Rows[0][20].ToString() != string.Empty)
                    {
                        txtTara.Text = GetValue(row, 20);
                    }
                    if (dt.Rows[0][21].ToString() != string.Empty)
                    {
                        txtTolerancia.Text = GetValue(row, 21);
                    }
                    if (dt.Rows[0][22].ToString() != string.Empty)
                    {
                        txtPBT.Text = GetValue(row, 22);
                    }
                    if (dt.Rows[0][27].ToString() != string.Empty)
                    {
                        txtCodMot.Text = GetValue(row, 27);
                        txtMotAnterior.Text = GetValue(row, 27);
                    }
                    if (dt.Rows[0][28].ToString() != string.Empty)
                    {
                        ddlMotorista.Items.Insert(0, GetValue(row, 28));
                    }
                    if (dt.Rows[0][29].ToString() != string.Empty)
                    {
                        txtCodTra.Text = GetValue(row, 29);
                    }
                    if (dt.Rows[0][30].ToString() != string.Empty)
                    {
                        ddlAgregados.Items.Insert(0, GetValue(row, 30));
                    }

                    if (dt.Rows[0][31].ToString() == string.Empty)
                    {
                        txtOpacidade.BackColor = System.Drawing.Color.Red;
                        txtOpacidade.ForeColor = System.Drawing.Color.White;
                        txtOpacidade.Text = "Verifique";
                    }
                    else
                    {
                        txtOpacidade.Text = DateTime.Parse(GetValue(row, 31).ToString()).ToString("dd/MM/yyyy");
                        DateTime dataHoje = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                        DateTime dataLicenciamento = Convert.ToDateTime(txtOpacidade.Text);

                        TimeSpan diferencaLicenciamento = dataLicenciamento - dataHoje;
                        // comparar a diferença
                        if (diferencaLicenciamento.TotalDays < 30 && diferencaLicenciamento.TotalDays >= 1)
                        {
                            string diasLicenciamento = diferencaLicenciamento.TotalDays.ToString();
                            txtOpacidade.Text = DateTime.Parse(GetValue(row, 31).ToString()).ToString("dd/MM/yyyy");
                            txtOpacidade.BackColor = System.Drawing.Color.Khaki;
                            txtOpacidade.ForeColor = System.Drawing.Color.OrangeRed;
                        }
                        else if (diferencaLicenciamento.TotalDays <= 0)
                        {
                            txtOpacidade.Text = DateTime.Parse(GetValue(row, 31).ToString()).ToString("dd/MM/yyyy");
                            txtOpacidade.BackColor = System.Drawing.Color.Red;
                            txtOpacidade.ForeColor = System.Drawing.Color.White;
                        }
                    }
                    if (dt.Rows[0][32].ToString() != string.Empty)
                    {
                        txtCadastradoPor.Text = GetValue(row, 32);
                    }
                    if (dt.Rows[0][33].ToString() != string.Empty)
                    {
                        txtDtCadastro.Text = GetValue(row, 33);
                    }
                    if (dt.Rows[0][34].ToString() != string.Empty)
                    {
                        txtAlteradoPor.Text = GetValue(row, 34);
                    }
                    if (dt.Rows[0][35].ToString() != string.Empty)
                    {
                        txtDtAlteracao.Text = GetValue(row, 35);
                    }
                    if (dt.Rows[0][36].ToString() == string.Empty)
                    {
                        txtVencCET.BackColor = System.Drawing.Color.Red;
                        txtVencCET.ForeColor = System.Drawing.Color.White;
                        txtVencCET.Text = "Verifique";
                    }
                    else
                    {                        
                        txtVencCET.Text = DateTime.Parse(GetValue(row, 36).ToString()).ToString("dd/MM/yyyy");
                        DateTime dataHoje = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                        DateTime dataLicenciamento = Convert.ToDateTime(txtVencCET.Text);

                        TimeSpan diferencaLicenciamento = dataLicenciamento - dataHoje;
                        // comparar a diferença
                        if (diferencaLicenciamento.TotalDays < 30 && diferencaLicenciamento.TotalDays >= 1)
                        {
                            string diasLicenciamento = diferencaLicenciamento.TotalDays.ToString();
                            txtVencCET.Text = DateTime.Parse(GetValue(row, 36).ToString()).ToString("dd/MM/yyyy");
                            txtVencCET.Text = txtVencCET.Text + " (" + diasLicenciamento + " dias)";
                            txtVencCET.BackColor = System.Drawing.Color.Khaki;
                            txtVencCET.ForeColor = System.Drawing.Color.OrangeRed;
                        }
                        else if (diferencaLicenciamento.TotalDays <= 0)
                        {
                            txtVencCET.Text = DateTime.Parse(GetValue(row, 36).ToString()).ToString("dd/MM/yyyy");
                            txtVencCET.BackColor = System.Drawing.Color.Red;
                            txtVencCET.ForeColor = System.Drawing.Color.White;
                            txtVencCET.Text = txtVencCET.Text + " (Vencida)";
                        }
                    }



                    if (dt.Rows[0][37].ToString() != string.Empty)
                    {
                        txtProtocoloCET.Text = GetValue(row, 37);
                    }
                    if (dt.Rows[0][38].ToString() == string.Empty)
                    {
                        txtLicenciamento.BackColor = System.Drawing.Color.Red;
                        txtLicenciamento.ForeColor = System.Drawing.Color.White;
                        txtLicenciamento.Text = "Verifique";
                    }
                    else
                    {
                        txtLicenciamento.Text = DateTime.Parse(GetValue(row, 38).ToString()).ToString("dd/MM/yyyy");
                        DateTime dataHoje = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                        DateTime dataLicenciamento = Convert.ToDateTime(txtLicenciamento.Text);

                        TimeSpan diferencaLicenciamento = dataLicenciamento - dataHoje;
                        // comparar a diferença
                        if (diferencaLicenciamento.TotalDays < 30 && diferencaLicenciamento.TotalDays >= 1)
                        {
                            string diasLicenciamento = diferencaLicenciamento.TotalDays.ToString();
                            txtLicenciamento.Text = DateTime.Parse(GetValue(row, 38).ToString()).ToString("dd/MM/yyyy");
                            txtLicenciamento.BackColor = System.Drawing.Color.Khaki;
                            txtLicenciamento.ForeColor = System.Drawing.Color.OrangeRed;
                        }
                        else if (diferencaLicenciamento.TotalDays <= 0)
                        {
                            txtLicenciamento.Text = DateTime.Parse(GetValue(row, 38).ToString()).ToString("dd/MM/yyyy");
                            txtLicenciamento.BackColor = System.Drawing.Color.Red;
                            txtLicenciamento.ForeColor = System.Drawing.Color.White;
                        }
                    }
                    if (dt.Rows[0][39].ToString() == string.Empty)
                    {
                        txtCronotacografo.BackColor = System.Drawing.Color.Red;
                        txtCronotacografo.ForeColor = System.Drawing.Color.White;
                        txtCronotacografo.Text = "Verifique";
                    }
                    else
                    {
                        txtCronotacografo.Text = DateTime.Parse(GetValue(row, 39).ToString()).ToString("dd/MM/yyyy");
                        DateTime dataHoje = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                        DateTime dataLicenciamento = Convert.ToDateTime(txtCronotacografo.Text);

                        TimeSpan diferencaLicenciamento = dataLicenciamento - dataHoje;
                        // comparar a diferença
                        if (diferencaLicenciamento.TotalDays < 30 && diferencaLicenciamento.TotalDays >= 1)
                        {
                            string diasLicenciamento = diferencaLicenciamento.TotalDays.ToString();
                            txtCronotacografo.Text = DateTime.Parse(GetValue(row, 39).ToString()).ToString("dd/MM/yyyy");
                            txtCronotacografo.BackColor = System.Drawing.Color.Khaki;
                            txtCronotacografo.ForeColor = System.Drawing.Color.OrangeRed;
                        }
                        else if (diferencaLicenciamento.TotalDays <= 0)
                        {
                            txtCronotacografo.Text = DateTime.Parse(GetValue(row, 39).ToString()).ToString("dd/MM/yyyy");
                            txtCronotacografo.BackColor = System.Drawing.Color.Red;
                            txtCronotacografo.ForeColor = System.Drawing.Color.White;
                        }
                    }
                    if (dt.Rows[0][40].ToString() != string.Empty)
                    {
                        ddlMarca.Items.Insert(0, GetValue(row, 40));
                    }
                    if (dt.Rows[0][41].ToString() != string.Empty)
                    {
                        txtRenavam.Text = GetValue(row, 41);
                    }
                    if (dt.Rows[0][42].ToString() != string.Empty)
                    {
                        ddlCor.Items.Insert(0, GetValue(row, 42));
                    }
                    if (dt.Rows[0][43].ToString() != string.Empty)
                    {
                        ddlComunicacao.Items.Insert(0, GetValue(row, 43));
                    }
                    if (dt.Rows[0][44].ToString() != string.Empty)
                    {
                        txtAntt.Text = GetValue(row, 44);
                    }
                    if (dt.Rows[0][47].ToString() != string.Empty)
                    {
                        ddlEstados.Items.Insert(0,GetValue(row, 47));
                    }
                    if (dt.Rows[0][48].ToString() != string.Empty)
                    {
                        ddlCidades.Items.Insert(0, GetValue(row, 48));
                    }
                    if (dt.Rows[0][49].ToString() != string.Empty)
                    {
                        txtLotacao.Text = GetValue(row, 49);
                    }
                    if (dt.Rows[0][50].ToString() != string.Empty)
                    {
                        txtComprimento.Text = GetValue(row, 50);
                    }
                    if (dt.Rows[0][51].ToString() != string.Empty)
                    {
                        txtLargura.Text = GetValue(row, 51);
                    }
                    if (dt.Rows[0][52].ToString() != string.Empty)
                    {
                        txtAltura.Text = GetValue(row, 52);
                    }
                    if (dt.Rows[0][53].ToString() != string.Empty)
                    {
                        txtPlacaAnt.Text = GetValue(row, 53);
                    }
                    if (dt.Rows[0][54].ToString() != string.Empty)
                    {
                        txtCodigo.Text = GetValue(row, 54);
                    }
                    if (dt.Rows[0][55].ToString() != string.Empty)
                    {
                        txtTipoSeguro.Text = GetValue(row, 55);
                    }
                    if (dt.Rows[0][56].ToString() != string.Empty)
                    {
                        txtSeguradora.Text = GetValue(row, 56);
                    }
                    if (dt.Rows[0][57].ToString() != string.Empty)
                    {
                        txtApolice.Text = GetValue(row, 57);
                    }
                    if (dt.Rows[0][58].ToString() == string.Empty)
                    {
                        txtTipoSeguro.BackColor = System.Drawing.Color.Red;
                        txtTipoSeguro.ForeColor = System.Drawing.Color.White;
                        txtTipoSeguro.Text = "Sem Seguro Cadastrado";
                    }
                    else
                    {
                        txtValidadeApolice.Text = DateTime.Parse(GetValue(row, 58).ToString()).ToString("dd/MM/yyyy");
                        DateTime dataHoje = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                        DateTime dataLicenciamento = Convert.ToDateTime(txtValidadeApolice.Text);

                        TimeSpan diferencaLicenciamento = dataLicenciamento - dataHoje;
                        // comparar a diferença
                        if (diferencaLicenciamento.TotalDays < 30 && diferencaLicenciamento.TotalDays >= 1)
                        {
                            string diasLicenciamento = diferencaLicenciamento.TotalDays.ToString();
                            txtValidadeApolice.Text = DateTime.Parse(GetValue(row, 58).ToString()).ToString("dd/MM/yyyy");
                            txtValidadeApolice.BackColor = System.Drawing.Color.Khaki;
                            txtValidadeApolice.ForeColor = System.Drawing.Color.OrangeRed;
                        }
                        else if (diferencaLicenciamento.TotalDays <= 0)
                        {
                            txtValidadeApolice.Text = DateTime.Parse(GetValue(row, 58).ToString()).ToString("dd/MM/yyyy");
                            txtValidadeApolice.BackColor = System.Drawing.Color.Red;
                            txtValidadeApolice.ForeColor = System.Drawing.Color.White;
                        }
                    }
                    if (dt.Rows[0][59].ToString() != string.Empty)
                    {
                        txtValorFranquia.Text = GetValue(row, 59);
                    }
                    if (dt.Rows[0][60].ToString() != string.Empty)
                    {
                        ddlTacografo.Items.Insert(0, GetValue(row, 60));
                    }
                    if (dt.Rows[0][61].ToString() != string.Empty)
                    {
                        ddlModeloTacografo.Items.Insert(0, GetValue(row, 61));
                    }
                    if (dt.Rows[0][62].ToString() != string.Empty)
                    {
                        txtDataAquisicao.Text = GetValue(row, 62);
                    }
                    if (dt.Rows[0][63].ToString() != string.Empty)
                    {
                        txtControlePatrimonio.Text = GetValue(row, 63);
                    }
                    if (dt.Rows[0][64].ToString() != string.Empty)
                    {
                        txtChassi.Text = GetValue(row, 64);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log ou tratamento do erro
                // Exemplo: ex.Message
            }
        }
        
        protected void btnSalvar1_Click(object sender, EventArgs e)
        {           
            if (txtCodMot.Text != "")
            {
                string codigoMotoristaNovo = txtCodMot.Text.Trim();
                string strConnMotNovo = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(strConnMotNovo))
                {
                    string queryMotoristaNovo = "SELECT codprop, placa, reboque1, reboque2, tipoveiculo  FROM tbmotoristas where codmot = @Codigo";

                    using (SqlCommand cmdMotoristaNovo = new SqlCommand(queryMotoristaNovo, conn))
                    {
                        cmdMotoristaNovo.Parameters.AddWithValue("@Codigo", codigoMotoristaNovo);
                        conn.Open();

                        using (SqlDataReader reader = cmdMotoristaNovo.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                conn.Close();
                                // Achou o motorista limpa
                                string queryAtualizaMotorista = @"UPDATE tbmotoristas SET codprop=@codprop, placa=@placa, reboque1=@reboque1, reboque2=@reboque2, tipoveiculo=@tipoveiculo WHERE codmot = '" + codigoMotoristaNovo + "'";
                                SqlCommand AtualizaMotorista = new SqlCommand(queryAtualizaMotorista, conn);
                                AtualizaMotorista.Parameters.AddWithValue("@placa", txtPlaca.Text.Trim().ToUpper());
                                AtualizaMotorista.Parameters.AddWithValue("@codvei", txtCodVei.Text.Trim().ToUpper());
                                AtualizaMotorista.Parameters.AddWithValue("@frota", txtCodVei.Text.Trim().ToUpper());
                                AtualizaMotorista.Parameters.AddWithValue("@codprop", txtCodTra.Text.Trim().ToUpper());
                                AtualizaMotorista.Parameters.AddWithValue("@reboque1", txtReb1.Text.Trim().ToUpper());
                                AtualizaMotorista.Parameters.AddWithValue("@reboque2", txtReb2.Text.Trim().ToUpper());
                                AtualizaMotorista.Parameters.AddWithValue("@tipoveiculo", cboTipo.SelectedItem.Text);
                                conn.Open();
                                AtualizaMotorista.ExecuteNonQuery();
                                conn.Close();
                            }
                            else
                            {
                                // motorista não está atrelado a nenhum veículo.
                            }
                        }
                    }

                }


            }

            string id = HttpContext.Current.Request.QueryString["id"];
            // Verifica se o ID foi fornecido e é um número válido
            if (string.IsNullOrEmpty(id) || !int.TryParse(id, out int idConvertido))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", "alert('ID inválido ou não fornecido.');", true);
                return;
            }

            string sql = @"UPDATE tbveiculos SET         
                                tipvei = @tipvei,
                                tipoveiculo = @tipoveiculo,
                                modelo = @modelo,
                                ano = @ano,
                                nucleo = @nucleo,
                                ativo_inativo = @ativo_inativo,
                                plavei = @plavei,
                                reboque1 = @reboque1,
                                reboque2 = @reboque2,
                                tipocarreta = @tipocarreta,
                                tiporeboque = @tiporeboque,
                                rastreamento = @rastreamento,
                                codrastreador = @codrastreador,
                                eixos = @eixos,
                                cap = @cap, 
                                tara = @tara,
                                tolerancia = @tolerancia,
                                pbt = @pbt,                            
                                codmot = @codmot,
                                motorista = @motorista,
                                codtra = @codtra,
                                transp = @transp,
                                vencimentolaudofumaca = @vencimentolaudofumaca,
                                usualt = @usualt,
                                dtcalt = @dtcalt,
                                protocolocet = @protocolocet,
                                venclicencacet = @venclicencacet,
                                venclicenciamento = @venclicenciamento,
                                venccronotacografo = @venccronotacografo,
                                marca = @marca,
                                renavan = @renavan,
                                cor = @cor,
                                comunicacao = @comunicacao,
                                antt = @antt,
                                ufplaca = @ufplaca,
                                cidplaca = @cidplaca,  
                                lotacao = @lotacao,
                                comprimento = @comprimento,
                                largura = @largura,
                                altura = @altura, 
                                placaant = @placaant,
                                tacografo = @tacografo,
                                modelotacografo = @modelotacografo,
                                dataaquisicao = @dataaquisicao, 
                                controlepatrimonio = @controlepatrimonio,
                                chassi = @chassi
                            WHERE id = @id";
            //try
            //{
                using (SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    // Adiciona os parâmetros                    
                    cmd.Parameters.AddWithValue("@tipvei", cboTipo.SelectedItem.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@tipoveiculo", ddlTipo.SelectedItem.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@modelo", txtModelo.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@ano", txtAno.Text);
                    cmd.Parameters.AddWithValue("@nucleo", cbFiliais.SelectedItem.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@ativo_inativo", ddlSituacao.SelectedItem.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@plavei", txtPlaca.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@reboque1", string.IsNullOrEmpty(txtReb1.Text.ToUpper()) ? (object)DBNull.Value : txtReb1.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@reboque2", string.IsNullOrEmpty(txtReb2.Text.ToUpper()) ? (object)DBNull.Value : txtReb2.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@tiporeboque", ddlCarreta.SelectedItem.Text.ToUpper());
                    if (ddlComposicao.SelectedItem.Text != "")
                    {
                        cmd.Parameters.AddWithValue("@tipocarreta", ddlComposicao.SelectedItem.Text.ToUpper());
                    }
                    cmd.Parameters.AddWithValue("@rastreamento", ddlMonitoramento.SelectedItem.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@codrastreador", txtCodRastreador.Text);
                    cmd.Parameters.AddWithValue("@eixos", txtEixos.Text);
                    cmd.Parameters.AddWithValue("@cap", txtCargaLiq.Text);
                    cmd.Parameters.AddWithValue("@tara", txtTara.Text);
                    cmd.Parameters.AddWithValue("@tolerancia", txtTolerancia.Text);
                    cmd.Parameters.AddWithValue("@pbt", txtPBT.Text);
                    cmd.Parameters.AddWithValue("@codmot", txtCodMot.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@motorista", ddlMotorista.SelectedItem.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@codtra", txtCodTra.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@transp", ddlAgregados.SelectedItem.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@vencimentolaudofumaca", txtOpacidade.Text);
                    cmd.Parameters.AddWithValue("@usualt", txtUsuarioAtual.Text.Trim().ToUpper()); // Usuário atual
                    cmd.Parameters.AddWithValue("@dtcalt", dataHoraAtual.ToString("dd/MM/yyyy HH:mm")); // Corrigido para DateTime
                    cmd.Parameters.AddWithValue("@protocolocet", txtProtocoloCET.Text);
                    cmd.Parameters.AddWithValue("@venclicencacet", string.IsNullOrEmpty(txtVencCET.Text) ? (object)DBNull.Value : txtVencCET.Text);
                    cmd.Parameters.AddWithValue("@venclicenciamento", string.IsNullOrEmpty(txtLicenciamento.Text) ? (object)DBNull.Value : txtLicenciamento.Text);
                    cmd.Parameters.AddWithValue("@venccronotacografo", string.IsNullOrWhiteSpace(txtCronotacografo.Text) ? (object)DBNull.Value : DateTime.Parse(txtCronotacografo.Text));
                    //cmd.Parameters.AddWithValue("@venccronotacografo", DateTime.Parse(txtCronotacografo.Text).ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@marca", ddlMarca.SelectedItem.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@renavan", txtRenavam.Text);
                    cmd.Parameters.AddWithValue("@cor", ddlCor.SelectedItem.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@comunicacao", ddlComunicacao.SelectedItem.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@antt", txtAntt.Text);
                    //cmd.Parameters.AddWithValue("@codreb1", numeroReb1.Text);
                    //cmd.Parameters.AddWithValue("@codreb2", numeroReb2.Text);
                    cmd.Parameters.AddWithValue("@ufplaca", ddlEstados.SelectedItem.Text);
                    cmd.Parameters.AddWithValue("@cidplaca", ddlCidades.SelectedItem.Text);
                    //if (ddlCidades.SelectedItem.Text != null)
                    //{
                    //    cmd.Parameters.AddWithValue("@cidplaca", string.IsNullOrEmpty(ddlCidades.SelectedItem.Text.ToUpper()) ? (object)DBNull.Value : ddlCidades.SelectedItem.Text.ToUpper());
                    //}
                    cmd.Parameters.AddWithValue("@lotacao", txtLotacao.Text);
                    cmd.Parameters.AddWithValue("@comprimento", txtComprimento.Text);
                    cmd.Parameters.AddWithValue("@largura", txtLargura.Text);
                    cmd.Parameters.AddWithValue("@altura", txtAltura.Text);
                    cmd.Parameters.AddWithValue("@placaant", txtPlacaAnt.Text);
                    cmd.Parameters.AddWithValue("@tacografo", ddlTacografo.SelectedItem.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@modelotacografo", ddlModeloTacografo.SelectedItem.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@dataaquisicao", txtDataAquisicao.Text);
                    cmd.Parameters.AddWithValue("@controlepatrimonio", txtControlePatrimonio.Text);
                    cmd.Parameters.AddWithValue("@chassi", txtChassi.Text);
                    cmd.Parameters.AddWithValue("@id", idConvertido);

                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        //string mensagem = $"Olá, {txtAlteradoPor.Text}! Código {txtCodTra.Text} atualizado com sucesso.";
                        //string script = $"alert('{HttpUtility.JavaScriptStringEncode(mensagem)}');";
                        //ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", script, true);
                        ExibirToastErro("Dados atualizados com sucesso.");
                        Thread.Sleep(5000);

                        Response.Redirect("ConsultaVeiculos.aspx");
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", "alert('Nenhum registro foi atualizado.');", true);
                    }
                }
            //}
            //catch (SqlException ex)
            //{
            //    //string mensagemErro = $"Erro ao atualizar: {HttpUtility.JavaScriptStringEncode(ex.Message)}";
            //    //string script = $"alert('{mensagemErro}');";
            //    //ClientScript.RegisterStartupScript(this.GetType(), "Erro", script, true);

            //    string erroDetalhado = "Erro ao salvar registro no banco de dados: " + ex.Message;

            //    // Tentar identificar qual parâmetro pode ter causado o erro
            //    erroDetalhado += "\nValores enviados:";
            //    erroDetalhado += $"\ntipvei = {cboTipo.SelectedValue.ToUpper()}";
            //    erroDetalhado += $"\ntipoveiculo = {ddlTipo.SelectedItem.Text.ToUpper()}";
            //    erroDetalhado += $"\nmodelo = {txtModelo.Text.ToUpper()}";
            //    erroDetalhado += $"\nano = {txtAno.Text}";
            //    erroDetalhado += $"\nnucleo = {cbFiliais.SelectedItem.Text.ToUpper()}";
            //    erroDetalhado += $"\nativo_inativo = {ddlSituacao.SelectedItem.Text.ToUpper()}";
            //    erroDetalhado += $"\nplavei = {txtPlaca.Text.ToUpper()}";
            //    erroDetalhado += $"\nreboque1 = {txtReb1.Text.ToUpper()}";
            //    erroDetalhado += $"\nreboque2 = {txtReb2.Text.ToUpper()}";
            //    erroDetalhado += $"\ntipocarreta = {ddlCarreta.SelectedItem.Text.ToUpper()}";
            //    erroDetalhado += $"\ntiporeboque = {ddlComposicao.SelectedItem.Text.ToUpper()}";
            //    erroDetalhado += $"\nrastreamento = {ddlMonitoramento.SelectedItem.Text.ToUpper()}";
            //    erroDetalhado += $"\ncodrastreador = {txtCodRastreador.Text}";
            //    erroDetalhado += $"\neixos = {txtEixos.Text.Trim()}";
            //    erroDetalhado += $"\ncap = {txtLotacao.Text}";
            //    erroDetalhado += $"\ntara = {txtTara.Text}";
            //    erroDetalhado += $"\ntolerancia = {txtTolerancia.Text}";
            //    erroDetalhado += $"\npbt = {txtPBT.Text}";
            //    erroDetalhado += $"\ncodmot = {txtCodMot.Text.ToUpper()}";
            //    erroDetalhado += $"\nmotorista = {ddlMotorista.SelectedItem.Text.ToUpper()}";
            //    erroDetalhado += $"\ncodtra = {txtCodTra.Text.ToUpper()}";
            //    erroDetalhado += $"\ntransp = {ddlAgregados.SelectedItem.Text.ToUpper()}";
            //    erroDetalhado += $"\nvencimentolaudofumaca = {txtOpacidade.Text}";
            //    erroDetalhado += $"\nusualt = {txtAlteradoPor.Text.Trim().ToUpper()}"; // Usuário atual
            //    erroDetalhado += $"\ndtcalt = {txtDtAlteracao.Text}"; // Corrigido para DateTime
            //    erroDetalhado += $"\nprotocolocet = {txtProtocoloCET.Text}";
            //    erroDetalhado += $"\nvenclicencacet = {txtVencCET.Text}";
            //    erroDetalhado += $"\nvenclicenciamento = {txtLicenciamento.Text}";
            //    erroDetalhado += $"\nvenccronotacografo = {txtCronotacografo.Text}";
            //    erroDetalhado += $"\nmarca = {ddlMarca.SelectedItem.Text.ToUpper()}";
            //    erroDetalhado += $"\nrenavan = {txtRenavam.Text}";
            //    erroDetalhado += $"\ncor = {ddlCor.SelectedItem.Text.ToUpper()}";
            //    erroDetalhado += $"\ncomunicacao = {ddlComunicacao.SelectedItem.Text.ToUpper()}";
            //    erroDetalhado += $"\nantt = {txtAntt.Text}";

            //    erroDetalhado += $"\nufplaca = {ddlEstados.SelectedItem.Text}";
            //    erroDetalhado += $"\ncidplaca = {ddlCidades.SelectedItem.Text}";
            //    erroDetalhado += $"\nlotacao = {txtLotacao.Text}";
            //    erroDetalhado += $"\ncomprimento = {txtComprimento.Text}";
            //    erroDetalhado += $"\nlargura = {txtLargura.Text}";
            //    erroDetalhado += $"\naltura = {txtAltura.Text}";
            //    erroDetalhado += $"\nplacaant = {txtPlacaAnt.Text}";
            //    erroDetalhado += $"\ntacografo = {ddlTacografo.SelectedItem.Text.ToUpper()}";
            //    erroDetalhado += $"\nmodelotacografo = {ddlModeloTacografo.SelectedItem.Text.ToUpper()}";
            //    erroDetalhado += $"\ndataaquisicao = {txtDataAquisicao.Text}";
            //    erroDetalhado += $"\ncontrolepatrimonio = {txtControlePatrimonio.Text}";
            //    erroDetalhado += $"\nchassi = {txtChassi.Text}";
            //    erroDetalhado += $"\nid = {idConvertido}";

            //    //// Aciona o Toast via JavaScript
            //    //ScriptManager.RegisterStartupScript(this, GetType(), "toastNaoEncontrado", "mostrarToastNaoEncontrado();", true);


            //    //string script = $"alert('{erroDetalhado}');";
            //    //ClientScript.RegisterStartupScript(this.GetType(), "Erro", script, true);




            //    // Exibir no log ou em uma Label (por exemplo)

            //    //if (ex.Number == 8152)
            //    //{
            //    //    lblErro.Text = "Erro: Valor duplicado em campo único.";
            //    //    lblErro.Text = erroDetalhado;
            //    //}
            //    miDiv.Visible = true;
            //    lblErro.Text = erroDetalhado;

            //}
        }
        private void PreencherComboAgregados(string filtroCodTra = null)
        {
            string query = "SELECT codtra, (CAST(codtra AS VARCHAR) + '-' + fantra) AS Nome FROM tbtransportadoras WHERE fl_exclusao IS NULL AND ativa_inativa = 'ATIVO'";

            if (!string.IsNullOrEmpty(filtroCodTra))
            {
                query += " AND LTRIM(RTRIM(codtra)) = @codtra";
            }

            query += " ORDER BY fantra";

            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                try
                {
                    // Abra a conexão com o banco de dados
                    conn.Open();

                    // Crie o comando SQL
                    SqlCommand cmd = new SqlCommand(query, conn);

                    if (!string.IsNullOrEmpty(filtroCodTra))
                    {
                        cmd.Parameters.AddWithValue("@codtra", filtroCodTra);
                    }

                    SqlDataReader reader = cmd.ExecuteReader();

                    // Preencher o ComboBox com os dados do DataReader
                    ddlAgregados.DataSource = reader;
                    ddlAgregados.DataTextField = "Nome";
                    ddlAgregados.DataValueField = "codtra";
                    ddlAgregados.DataBind();

                    //ddlAgregados.Items.Insert(0, "Selecione...");

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
        protected void ddlMotorista_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtCodMot.Text = ddlMotorista.SelectedValue;

            using (SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                string sql = "SELECT * FROM tbveiculos WHERE codmot = @codmot";
                con.Open();

                SqlDataAdapter da = new SqlDataAdapter(sql, con);
                da.SelectCommand.Parameters.AddWithValue("@codmot", txtCodMot.Text);

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
                                string nomeUsuario = txtAlteradoPor.Text;
                                string mensagem = $"Olá, {nomeUsuario}!\nCódigo {txtCodMot.Text}, foi desvinculado do veículo com sucesso.";
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
                        //txtCodMot.Text = string.Empty;
                    }



                }
            }
        }
        // Função para carregar o DropDownList com dados dos agregados
        private void CarregarDDLAgregados()
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                conn.Open();
                string query = "SELECT ID, fantra FROM tbtransportadoras WHERE fl_exclusao is null AND ativa_inativa = 'ATIVO' ORDER BY fantra";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                ddlAgregados.DataSource = reader;
                ddlAgregados.DataTextField = "fantra";  // Campo a ser exibido
                ddlAgregados.DataValueField = "ID";  // Valor associado ao item
                ddlAgregados.DataBind();

                // Adicionar o item padrão
                ddlAgregados.Items.Insert(0, new ListItem("Selecione...", "0"));
            }
        }
        // Evento disparado quando o item do DropDownList é alterado
        protected void ddlAgregados_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idSelecionado = int.Parse(ddlAgregados.SelectedValue);

            // Preencher os campos com base no valor selecionado
            if (idSelecionado > 0)
            {
                PreencherCampos(idSelecionado);
            }
            else
            {
                LimparCampos();
            }
        }
        // Função para preencher os campos com os dados do banco
        private void PreencherCampos(int id)
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                conn.Open();
                string query = "SELECT codtra, fantra, antt FROM tbtransportadoras WHERE ID = @ID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtCodTra.Text = reader["codtra"].ToString();
                    //   ddlAgregados.DataValueField = reader["ID"].ToString();
                    //    ddlAgregados.Text = reader["fantra"].ToString();

                    txtAntt.Text = reader["antt"].ToString();
                }
            }
        }
        // Função para limpar os campos
        private void LimparCampos()
        {
            txtCodTra.Text = string.Empty;
            txtAntt.Text = string.Empty;
        }
        protected void ddlComposicao_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idSelecionado = int.Parse(ddlComposicao.SelectedValue);

            // Preencher os campos com base no valor selecionado
            if (idSelecionado > 0)
            {
                PreencherCamposComposicao(idSelecionado);
            }
            else
            {
                LimparCamposComposicao();
            }
        }
        private void PreencherCamposComposicao(int id)
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                conn.Open();
                string query = "SELECT composicao, eixos, pbt, tolerancia FROM tbcomposicaoveiculo WHERE ID = @ID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtEixos.Text = reader["eixos"].ToString();
                    txtPBT.Text = reader["pbt"].ToString();
                    txtTolerancia.Text = reader["tolerancia"].ToString();

                    double sPBT, sTara;
                    if (double.TryParse(txtPBT.Text, out sPBT) && double.TryParse(txtTara.Text, out sTara))
                    {
                        double resultado = sPBT - sTara;
                        txtLotacao.Text = resultado.ToString("N0"); // casas decimais
                    }

                    double sTolerancia, sLotacao;
                    if (double.TryParse(txtLotacao.Text, out sLotacao) && double.TryParse(txtTolerancia.Text, out sTolerancia))
                    {
                        double resultado2 = ((sLotacao * 5) / 100) + sLotacao;

                        txtCargaLiq.Text = resultado2.ToString("N0"); // N0 = casas decimais 
                    }

                }
            }
        }
        private void LimparCamposComposicao()
        {
            txtTolerancia.Text = string.Empty;
            txtPBT.Text = string.Empty;
            txtLotacao.Text = string.Empty;
            txtEixos.Text = string.Empty;
        }
        protected void txtCodTra_TextChanged(object sender, EventArgs e)
        {
            if (txtCodTra.Text != "")
            {
                string cod = txtCodTra.Text;
                string sql = "SELECT codtra, fantra, ativa_inativa, fl_exclusao, antt FROM tbtransportadoras where codtra = '" + cod + "'";
                SqlDataAdapter da = new SqlDataAdapter(sql, con);
                DataTable dt = new DataTable();
                con.Open();
                da.Fill(dt);
                con.Close();

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0][3].ToString() == null)
                    {
                        ExibirToastErro("Código: " + txtCodTra.Text.Trim() + ", deletado do sistema.");
                        Thread.Sleep(5000);
                        txtCodVei.Text = "";
                        txtCodVei.Focus();
                        return;
                    }
                    else if (dt.Rows[0][2].ToString() == "INATIVO")
                    {
                        ExibirToastErro("Código: " + txtCodTra.Text.Trim() + ", inativo no sistema.");
                        Thread.Sleep(5000);
                        txtCodVei.Text = "";
                        txtCodVei.Focus();
                        return;
                    }
                    else
                    {
                        ddlAgregados.SelectedItem.Text = dt.Rows[0][1].ToString();
                        txtAntt.Text = dt.Rows[0][4].ToString();
                        return;
                    }

                }
                else
                {
                    ExibirToastErro("Código: " + txtCodTra.Text.Trim() + ", não encontrado no sistema.");
                    Thread.Sleep(5000);
                    txtCodTra.Text = "";
                    txtCodTra.Focus();
                }
            }

        }
        protected void txtCodMot_TextChanged(object sender, EventArgs e)
        {
            if (txtCodMot.Text != "")
            {


                string codigoMotorista = txtCodMot.Text.Trim();
                string strConn = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    string query = "SELECT codmot, nommot, codtra, transp, status, fl_exclusao, placa, codvei FROM tbmotoristas WHERE codmot = @Codigo";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Codigo", codigoMotorista);
                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtCodMot.Text = reader["codmot"].ToString();
                                ddlMotorista.SelectedItem.Text = reader["nommot"].ToString();
                                if (reader["codtra"].ToString() != txtCodTra.Text.Trim())
                                {
                                    ExibirToastErro("Motorista: " + reader["nommot"].ToString().Trim() + ", não pertence a transportadora do veículo. Não pode dirigir este veículo.");
                                    Thread.Sleep(5000);
                                    txtCodMot.Text = "";
                                    ddlMotorista.SelectedItem.Text = "";
                                    txtCodMot.Focus();
                                    return;
                                }
                                else if (reader["status"].ToString() == "INATIVO")
                                {
                                    ExibirToastErro("Motorista: " + reader["nommot"].ToString().Trim() + ", inativo no sistema.");
                                    Thread.Sleep(5000);
                                    txtCodMot.Text = "";
                                    ddlMotorista.SelectedItem.Text = "";
                                    txtCodMot.Focus();
                                    return;
                                }
                                else if (reader["fl_exclusao"].ToString() == "S")
                                {
                                    ExibirToastErro("Motorista: " + reader["nommot"].ToString().Trim() + ", deletado do sistema.");
                                    Thread.Sleep(5000);
                                    txtCodMot.Text = "";
                                    ddlMotorista.SelectedItem.Text = "";
                                    txtCodMot.Focus();
                                    return;
                                }
                                // Verifica se o motorista está atrelado
                                else if (reader["placa"].ToString() != "" || reader["placa"].ToString() != null)
                                {
                                    txtMotoristaAtrelado.Text = reader["codmot"].ToString() + "/" + reader["nommot"].ToString();
                                    txtPlacaAtrelada.Text = reader["codvei"].ToString() + "/" + reader["placa"].ToString();

                                    // Dispara o script para abrir o modal via JavaScript
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "abrirTrocaMotorista", "abrirTrocaMotorista();", true);                                    
                                    
                                }
                                else if (reader["codtra"].ToString() == txtCodTra.Text.Trim() && reader["status"].ToString() == "ATIVO" && reader["fl_exclusao"].ToString() != null)
                                {
                                    ddlMotorista.SelectedItem.Text = reader["nommot"].ToString();
                                }
                            }
                            else
                            {
                                ExibirToastErro("Código: " + txtCodMot.Text.Trim() + ", não encontrado no sistema.");
                                Thread.Sleep(5000);
                                txtCodMot.Text = "";
                                txtCodMot.Focus();
                                // Opcional: exibir mensagem ao usuário
                            }

                        }
                    }

                }

            }
        }
        protected void txtCodRastreador_TextChanged(object sender, EventArgs e)
        {
            if (txtCodRastreador.Text != "")
            {

                string codigoRastreador = txtCodRastreador.Text.Trim();
                string strConn = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    string query = "SELECT codRastreador, nomRastreador FROM tbrastreadores WHERE codRastreador = @Codigo";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Codigo", codigoRastreador);
                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                ddlTecnologia.SelectedItem.Text = reader["nomRastreador"].ToString();
                            }
                            else
                            {
                                ExibirToastErro("Código: " + txtCodRastreador.Text.Trim() + ", não encontrado no sistema.");
                                Thread.Sleep(5000);
                                txtCodRastreador.Text = "";
                                txtCodRastreador.Focus();
                            }

                        }
                    }

                }

            }

        }
        protected void txtPlaca_TextChanged(object sender, EventArgs e)
        {
            if (txtPlaca.Text != "")
            {
                string termo = txtPlaca.Text.ToUpper();
                string strConn = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    string query = "SELECT TOP 1 plavei,codvei FROM tbveiculos WHERE plavei LIKE @termo";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@termo", "%" + termo + "%");
                    conn.Open();

                    object res = cmd.ExecuteScalar();
                    if (res != null)
                    {
                        ExibirToast("Placa: " + txtPlaca.Text.Trim() + ", já cadastrada no sistema.");
                        Thread.Sleep(5000);
                        txtPlaca.Text = "";
                        txtPlaca.Focus();
                    }
                }


                txtPlaca.Text.ToUpper();
                ddlEstados.Focus();
            }
        }
        protected void ExibirToast(string mensagem)
        {
            string script = $@"
            <script>
                document.getElementById('toastMessage').innerText = '{mensagem.Replace("'", "\\'")}';
                var toastEl = document.getElementById('myToast');
                var toast = new bootstrap.Toast(toastEl);
                toast.show();
            </script>";

            ClientScript.RegisterStartupScript(this.GetType(), "toastScript", script, false);
        }
        protected void ExibirToastCadastro(string mensagem)
        {
            string script = $@"
            <script>
                document.getElementById('toastMessage2').innerText = '{mensagem.Replace("'", "\\'")}';
                var toastEl = document.getElementById('myToast2');
                var toast = new bootstrap.Toast(toastEl);
                toast.show();
            </script>";

            ClientScript.RegisterStartupScript(this.GetType(), "toastScript", script, false);
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
        protected void txtReb1_TextChanged(object sender, EventArgs e)
        {
            if (txtReb1.Text != "")
            {
                string placaReboque = txtReb1.Text.Trim();
                string strConn = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    string query = "SELECT codcarreta, placacarreta, tiporeboque, codprop, descprop, frota, placa_cavalo, ativo_inativo, fl_exclusao FROM tbcarretas where placacarreta = @Codigo";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Codigo", placaReboque);
                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                //txtCodMot.Text = reader["codmot"].ToString();
                                //ddlMotorista.SelectedItem.Text = reader["nommot"].ToString();
                                if (reader["tiporeboque"].ToString() == "FROTA" && reader["codprop"].ToString() != "1111")
                                {
                                    ExibirToastErro("Placa: " + reader["placacarreta"].ToString().Trim() + ", não pode se atrelada a veículo da frota.");
                                    Thread.Sleep(5000);
                                    txtReb1.Text = "";
                                    txtReb1.Focus();
                                    return;
                                }
                                else if (reader["ativo_inativo"].ToString() == "INATIVO")
                                {
                                    ExibirToastErro("Placa: " + reader["placacarreta"].ToString().Trim() + ", inativa no sistema.");
                                    Thread.Sleep(5000);
                                    txtReb1.Text = "";
                                    txtReb1.Focus();
                                    return;
                                }
                                else if (reader["fl_exclusao"].ToString() == "S")
                                {
                                    ExibirToastErro("Placa: " + reader["placacarreta"].ToString().Trim() + ", deletada do sistema.");
                                    Thread.Sleep(5000);
                                    txtReb1.Text = "";
                                    txtReb1.Focus();
                                    return;
                                }
                                // Verifica se a carreta está atrelada
                                else if (reader["placa_cavalo"].ToString() != "" || reader["placa_cavalo"].ToString() != null)
                                {
                                    txtFrotaAtrelado.Text = reader["codcarreta"].ToString() + "/" + reader["placacarreta"].ToString().ToUpper();
                                    txtFrotaCavalo.Text = reader["frota"].ToString() + "/" + reader["placa_cavalo"].ToString().ToUpper();
                                    txtTransportadora.Text = reader["descprop"].ToString();

                                    // Dispara o script para abrir o modal via JavaScript
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "modalConfirmacaoCarreta", "abrirConfirmacaoCarreta();", true);
                                }
                                else if (reader["placa_cavalo"].ToString() != txtReb1.Text.Trim() && reader["ativo_inativo"].ToString() == "ATIVO" && reader["fl_exclusao"].ToString() != null)
                                {
                                    txtReb1.Text = reader["placacarreta"].ToString().Trim().ToUpper();
                                }
                            }
                            else
                            {
                                ExibirToastErro("Código: " + txtReb1.Text.Trim() + ", não encontrado no sistema.");
                                Thread.Sleep(5000);
                                txtReb1.Text = "";
                                txtReb1.Focus();
                                // Opcional: exibir mensagem ao usuário
                            }

                        }
                    }

                }

            }
        }
        protected void txtReb2_TextChanged(object sender, EventArgs e)
        {
            if (txtReb2.Text != "")
            {
                string placaReboque = txtReb2.Text.Trim();
                string strConn = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    string query = "SELECT codcarreta, placacarreta, tiporeboque, codprop, descprop, frota, placa_cavalo, ativo_inativo, fl_exclusao FROM tbcarretas where placacarreta = @Codigo";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Codigo", placaReboque);
                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                //txtCodMot.Text = reader["codmot"].ToString();
                                //ddlMotorista.SelectedItem.Text = reader["nommot"].ToString();
                                if (reader["tiporeboque"].ToString() == "FROTA" && reader["codprop"].ToString() != "1111")
                                {
                                    ExibirToastErro("Placa: " + reader["placacarreta"].ToString().Trim() + ", não pode se atrelada a veículo da frota.");
                                    Thread.Sleep(5000);
                                    txtReb2.Text = "";
                                    txtReb2.Focus();
                                    return;
                                }
                                else if (reader["ativo_inativo"].ToString() == "INATIVO")
                                {
                                    ExibirToastErro("Placa: " + reader["placacarreta"].ToString().Trim() + ", inativa no sistema.");
                                    Thread.Sleep(5000);
                                    txtReb2.Text = "";
                                    txtReb2.Focus();
                                    return;
                                }
                                else if (reader["fl_exclusao"].ToString() == "S")
                                {
                                    ExibirToastErro("Placa: " + reader["placacarreta"].ToString().Trim() + ", deletada do sistema.");
                                    Thread.Sleep(5000);
                                    txtReb2.Text = "";
                                    txtReb2.Focus();
                                    return;
                                }
                                // Verifica se a carreta está atrelada
                                else if (reader["placa_cavalo"].ToString() != "" || reader["placa_cavalo"].ToString() != null)
                                {
                                    txtFrotaAtrelado.Text = reader["codcarreta"].ToString() + "/" + reader["placacarreta"].ToString().ToUpper();
                                    txtFrotaCavalo.Text = reader["frota"].ToString() + "/" + reader["placa_cavalo"].ToString().ToUpper();
                                    txtTransportadora.Text = reader["descprop"].ToString();

                                    // Dispara o script para abrir o modal via JavaScript
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "modalConfirmacaoCarreta", "abrirConfirmacaoCarreta();", true);
                                }
                                else if (reader["placa_cavalo"].ToString() != txtReb2.Text.Trim() && reader["ativo_inativo"].ToString() == "ATIVO" && reader["fl_exclusao"].ToString() != null)
                                {
                                    txtReb2.Text = reader["placacarreta"].ToString().Trim().ToUpper();
                                }
                            }
                            else
                            {
                                ExibirToastErro("Placa: " + txtReb2.Text.Trim() + ", não encontrada no sistema.");
                                Thread.Sleep(5000);
                                txtReb2.Text = "";
                                txtReb2.Focus();
                                // Opcional: exibir mensagem ao usuário
                            }

                        }
                    }

                }

            }
        }
        protected void cboTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string tipoVeiculo = cboTipo.SelectedValue;
            CarregarComposicao(tipoVeiculo);

            // Restaurar cidade se estiver em ViewState
            if (ViewState["ComposicaoSelecionada"] != null)
            {
                string composicaoId = ViewState["ComposicaoSelecionada"].ToString();
                if (ddlComposicao.Items.FindByValue(composicaoId) != null)
                {
                    ddlComposicao.SelectedValue = composicaoId;
                }
            }
            if (cboTipo.SelectedItem.Text == "TRUCK" || cboTipo.SelectedItem.Text == "BITRUCK" || cboTipo.SelectedItem.Text == "UTILITÁRIO/FURGÃO" || cboTipo.SelectedItem.Text == "LEVE" || cboTipo.SelectedItem.Text == "FIORINO" || cboTipo.SelectedItem.Text == "TOCO" || cboTipo.SelectedItem.Text == "VUC OU 3/4")
            {
                pnlDivReboque1.Visible = false;
                pnlDivReboque2.Visible = false;
                ddlCarreta.Visible = false;
                carreta.Visible = false;
            }
            else if (cboTipo.SelectedItem.Text == "BITREM")
            {
                pnlDivReboque1.Visible = true;
                pnlDivReboque2.Visible = true;
                ddlCarreta.Visible = true;
                carreta.Visible = true;
            }
            else if (cboTipo.SelectedItem.Text == "CAVALO SIMPLES" || cboTipo.SelectedItem.Text == "CAVALO TRUCADO" || cboTipo.SelectedItem.Text == "CAVALO 4 EIXOS")
            {
                pnlDivReboque1.Visible = true;
                pnlDivReboque2.Visible = false;
                ddlCarreta.Visible = true;
                carreta.Visible = true;
            }
        }
        private void CarregarComposicao(string tipoVeiculo)
        {
            string strConn = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                string query = "SELECT ID, composicao, eixos, pbt, tolerancia, tipo_veiculo FROM tbcomposicaoveiculo WHERE tipo_veiculo = @ID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", tipoVeiculo);
                conn.Open();
                ddlComposicao.DataSource = cmd.ExecuteReader();
                ddlComposicao.DataTextField = "composicao";
                ddlComposicao.DataValueField = "ID"; // valor único
                ddlComposicao.DataBind();

                //ddlComposicao.Items.Insert(0, new ListItem("-- Selecione a composição --", "0"));
            }
        }
        protected void btnSalvarTrocaMotorista_Click(object sender, EventArgs e)
        {
            if (txtCodMot.Text != "")
            {
                string codigoMotorista = txtCodMot.Text.Trim();
                string strConn = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    string query = "SELECT codmot, motorista FROM tbveiculos where codmot = @Codigo";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Codigo", codigoMotorista);
                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                conn.Close();
                                // Achou o motorista limpa
                                string queryLimpaMotorista = @"UPDATE tbveiculos SET codmot=@codmot, motorista=@motorista WHERE codmot = '" + codigoMotorista + "'";
                                SqlCommand LimpaMotorista = new SqlCommand(queryLimpaMotorista, conn);
                                LimpaMotorista.Parameters.AddWithValue("@codmot", "");
                                LimpaMotorista.Parameters.AddWithValue("@motorista", "");
                                conn.Open();
                                LimpaMotorista.ExecuteNonQuery();
                                conn.Close();                               
                            }
                            else
                            {
                                // motorista não está atrelado a nenhum veículo.
                                if (txtMotAnterior.Text != "")
                                {
                                    string codigoMotoristaAnterior = txtMotAnterior.Text.Trim();
                                    string strConnMotAnterior = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
                                    using (SqlConnection conn2 = new SqlConnection(strConnMotAnterior))
                                    {
                                        string queryMotoristaAnterior2 = "SELECT codmot, codprop, placa, reboque1, reboque2, tipoveiculo FROM tbmotoristas where codmot = @Codigo";

                                        using (SqlCommand cmdMotoristaAnterior = new SqlCommand(queryMotoristaAnterior2, conn2))
                                        {
                                            cmdMotoristaAnterior.Parameters.AddWithValue("@Codigo", codigoMotoristaAnterior);
                                            conn2.Open();

                                            using (SqlDataReader reader2 = cmdMotoristaAnterior.ExecuteReader())
                                            {
                                                if (reader2.Read())
                                                {
                                                    conn2.Close();
                                                    // Achou o motorista limpa
                                                    string queryLimpaMotoristaAnterior2 = @"UPDATE tbmotoristas SET codprop=@codprop, placa=@placa, reboque1=@reboque1, reboque2=@reboque2, tipoveiculo=@tipoveiculo WHERE codmot = '" + codigoMotoristaAnterior + "'";
                                                    SqlCommand LimpaMotoristaAnterior2 = new SqlCommand(queryLimpaMotoristaAnterior2, conn2);
                                                    LimpaMotoristaAnterior2.Parameters.AddWithValue("@codprop", "");
                                                    LimpaMotoristaAnterior2.Parameters.AddWithValue("@placa", "");
                                                    LimpaMotoristaAnterior2.Parameters.AddWithValue("@reboque1", "");
                                                    LimpaMotoristaAnterior2.Parameters.AddWithValue("@reboque2", "");
                                                    LimpaMotoristaAnterior2.Parameters.AddWithValue("@tipoveiculo", "");
                                                    conn2.Open();
                                                    LimpaMotoristaAnterior2.ExecuteNonQuery();
                                                    conn2.Close();
                                                }
                                                else
                                                {
                                                    // motorista não está atrelado a nenhum veículo.
                                                }
                                            }
                                        }

                                    }

                                }
                            }

                        }
                    }

                }

                if (txtMotAnterior.Text != "")
                {
                    string codigoMotoristaAnterior = txtMotAnterior.Text.Trim();
                    string strConnMotAnterior = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
                    using (SqlConnection conn = new SqlConnection(strConnMotAnterior))
                    {
                        string queryMotoristaAnterior = "SELECT codmot, codprop, placa, reboque1, reboque2, tipoveiculo FROM tbmotoristas where codmot = @Codigo";

                        using (SqlCommand cmdMotoristaAnterior = new SqlCommand(queryMotoristaAnterior, conn))
                        {
                            cmdMotoristaAnterior.Parameters.AddWithValue("@Codigo", codigoMotoristaAnterior);
                            conn.Open();

                            using (SqlDataReader reader = cmdMotoristaAnterior.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    conn.Close();
                                    // Achou o motorista limpa
                                    string queryLimpaMotoristaAnterior = @"UPDATE tbmotoristas SET codprop=@codprop, placa=@placa, reboque1=@reboque1, reboque2=@reboque2, tipoveiculo=@tipoveiculo WHERE codmot = '" + codigoMotoristaAnterior + "'";
                                    SqlCommand LimpaMotoristaAnterior = new SqlCommand(queryLimpaMotoristaAnterior, conn);
                                    LimpaMotoristaAnterior.Parameters.AddWithValue("@codprop", "");
                                    LimpaMotoristaAnterior.Parameters.AddWithValue("@placa", "");
                                    LimpaMotoristaAnterior.Parameters.AddWithValue("@reboque1", "");
                                    LimpaMotoristaAnterior.Parameters.AddWithValue("@reboque2", "");
                                    LimpaMotoristaAnterior.Parameters.AddWithValue("@tipoveiculo", "");
                                    conn.Open();
                                    LimpaMotoristaAnterior.ExecuteNonQuery();
                                    conn.Close();



                                }
                                else
                                {
                                    // motorista não está atrelado a nenhum veículo.
                                }
                            }
                        }

                    }

                }

                // Script para fechar o modal após salvar
                ScriptManager.RegisterStartupScript(this, this.GetType(), "FecharModal", "$('#ModalTrocarMotorista').modal('hide');", true);
            }

            
        }
    }
}


