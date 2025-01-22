using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NewCapit
{
    public partial class Frm_AltVeiculos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack) {
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
                txtCadastro.Text = dataHoraAtual.ToString("dd/MM/yyyy");
                lblDtCadastro.Text = dataHoraAtual.ToString("dd/MM/yyyy HH:mm");
                txtTolerancia.Text = "5";
            }
            PreencherComboFiliais();
            PreencherComboMarcasVeiculos();
            PreencherComboCoresVeiculos();
            PreencherComboRastreadores();
            PreencherComboMotoristas();
            
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

        private void PreencherComboMotoristas()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT codmot, nommot FROM tbmotoristas where fl_exclusao is null and status = 'ATIVO' ORDER BY nommot";

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
                    ddlTransportadora.DataSource = reader;
                    ddlTransportadora.DataTextField = "nommot";  // Campo que será mostrado no ComboBox
                    ddlTransportadora.DataValueField = "codmot";  // Campo que será o valor de cada item             
                    ddlTransportadora.DataBind();  // Realiza o binding dos dados                   

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

        private void cboTipoCarreta_Leave()
        {
            string composicao1 = "CAVALO SIMPLES COM CARRETA VANDERLEIA ABERTA";
            string composicao2 = "CAVALO SIMPLES COM CARRETA SIMPLES TOTAL SIDER";
            string composicao3 = "CAVALO SIMPLES COM CARRETA SIMPLES(LS) ABERTA";
            string composicao4 = "CAVALO SIMPLES COM CARRETA VANDERLEIA TOTAL SIDER";
            string composicao5 = "CAVALO TRUCADO COM CARRETA VANDERLEIA ABERTA";
            string composicao6 = "CAVALO TRUCADO COM CARRETA SIMPLES TOTAL SIDER";
            string composicao7 = "CAVALO TRUCADO COM CARRETA SIMPLES(LS) ABERTA";
            string composicao8 = "CAVALO TRUCADO COM CARRETA VANDERLEIA TOTAL SIDER";
            string composicao9 = "TRUCK";
            string composicao10 = "BITRUCK";
            string composicao11 = "BITREM 7 EIXOS";
            string composicao12 = "TOCO";
            string composicao13 = "VEICULO 3/4";
            string composicao14 = "CAVALO SIMPLES COM PRANCHA";
            string composicao15 = "CAVALO TRUCADO COM PRANCHA";
            string composicao16 = "CAVALO TRUCADO COM CARRETA LS TOTAL SIDER LISA";
            string composicao17 = "CAVALO SIMPLES COM CARRETA LS TOTAL SIDER LISA";
            

            string selectedValue = ddlComposicao.SelectedItem.ToString().Trim();
            string tipoComposicao = selectedValue;

            if (tipoComposicao.Equals(composicao1))
            {
                txtEixos.Text = "05";
                txtCap.Text = "46000";
                txtTolerancia.Text = "5";
                int nCapacidade = 46000;
                int nTara = int.Parse(txtTara.Text);
                int nPesoLiquido = nCapacidade - nTara;
                int nPesoTolerancia = (nPesoLiquido * 5) / 100;
                int nTotalCarga = nPesoLiquido + nPesoTolerancia;
                txtPBT.Text = nTotalCarga.ToString();
            }
            else if (tipoComposicao.Equals(composicao2))
            {
                txtEixos.Text = "05";
                txtCap.Text = "46000";
                txtTolerancia.Text = "5";
                int nCapacidade = 41500;
                int nTara = int.Parse(txtTara.Text);
                int nPesoLiquido = nCapacidade - nTara;
                int nPesoTolerancia = (nPesoLiquido * 5) / 100;
                int nTotalCarga = nPesoLiquido + nPesoTolerancia;
                txtPBT.Text = nTotalCarga.ToString();
            }
            else if (tipoComposicao.Equals(composicao3))
            {
                txtEixos.Text = "05";
                txtCap.Text = "41500";
                txtTolerancia.Text = "5";
                int nCapacidade = 46000;
                int nTara = int.Parse(txtTara.Text);
                int nPesoLiquido = nCapacidade - nTara;
                int nPesoTolerancia = (nPesoLiquido * 5) / 100;
                int nTotalCarga = nPesoLiquido + nPesoTolerancia;
                txtPBT.Text = nTotalCarga.ToString();
            }
            else if (tipoComposicao.Equals(composicao4))
            {
                txtEixos.Text = "05";
                txtCap.Text = "46000";
                txtTolerancia.Text = "5";
                int nCapacidade = 46000;
                int nTara = int.Parse(txtTara.Text);
                int nPesoLiquido = nCapacidade - nTara;
                int nPesoTolerancia = (nPesoLiquido * 5) / 100;
                int nTotalCarga = nPesoLiquido + nPesoTolerancia;
                txtPBT.Text = nTotalCarga.ToString();
            }
            else if (tipoComposicao.Equals(composicao5))
            {
                txtEixos.Text = "06";
                txtCap.Text = "53000";
                txtTolerancia.Text = "5";
                int nCapacidade = 53000;
                int nTara = int.Parse(txtTara.Text);
                int nPesoLiquido = nCapacidade - nTara;
                int nPesoTolerancia = (nPesoLiquido * 5) / 100;
            }
            else if (tipoComposicao.Equals(composicao6))
            {
                //"CAVALO TRUCADO COM CARRETA SIMPLES TOTAL SIDER";
                txtEixos.Text = "06";
                txtCap.Text = "48500";
                txtTolerancia.Text = "5";
                int nCapacidade = 48500;
                int nTara = int.Parse(txtTara.Text);
                int nPesoLiquido = nCapacidade - nTara;
                int nPesoTolerancia = (nPesoLiquido * 5) / 100;
                int nTotalCarga = nPesoLiquido + nPesoTolerancia;
                txtPBT.Text = nTotalCarga.ToString();
            }
            else if (tipoComposicao.Equals(composicao7))
            {
                //"CAVALO TRUCADO COM CARRETA SIMPLES(LS) ABERTA";
                txtEixos.Text = "06";
                txtCap.Text = "48500";
                txtTolerancia.Text = "5";
                int nCapacidade = 48500;
                int nTara = int.Parse(txtTara.Text);
                int nPesoLiquido = nCapacidade - nTara;
                int nPesoTolerancia = (nPesoLiquido * 5) / 100;
                int nTotalCarga = nPesoLiquido + nPesoTolerancia;
                txtPBT.Text = nTotalCarga.ToString();
            }
            else if (tipoComposicao.Equals(composicao8))
            {
                //"CAVALO TRUCADO COM CARRETA VANDERLEIA TOTAL SIDER";
                txtEixos.Text = "06";
                txtCap.Text = "53000";
                txtTolerancia.Text = "5";
                int nCapacidade = 53000;
                int nTara = int.Parse(txtTara.Text);
                int nPesoLiquido = nCapacidade - nTara;
                int nPesoTolerancia = (nPesoLiquido * 5) / 100;
                int nTotalCarga = nPesoLiquido + nPesoTolerancia;
                txtPBT.Text = nTotalCarga.ToString();
            }
            else if (tipoComposicao.Equals(composicao9))
            {
                // "TRUCK";
                txtEixos.Text = "03";
                txtCap.Text = "23000";
                txtTolerancia.Text = "5";
                int nCapacidade = 23000;
                int nTara = int.Parse(txtTara.Text);
                int nPesoLiquido = nCapacidade - nTara;
                int nPesoTolerancia = (nPesoLiquido * 5) / 100;
                int nTotalCarga = nPesoLiquido + nPesoTolerancia;
                txtPBT.Text = nTotalCarga.ToString();
            }
            else if (tipoComposicao.Equals(composicao10))
            {
                // BITRUCK
                txtEixos.Text = "04";
                txtCap.Text = "29000";
                txtTolerancia.Text = "5";
                int nCapacidade = 29000;
                int nTara = int.Parse(txtTara.Text);
                int nPesoLiquido = nCapacidade - nTara;
                int nPesoTolerancia = (nPesoLiquido * 5) / 100;
                int nTotalCarga = nPesoLiquido + nPesoTolerancia;
                txtPBT.Text = nTotalCarga.ToString();
            }
            else if (tipoComposicao.Equals(composicao11))
            {
                // BITREM
                txtEixos.Text = "07";
                txtCap.Text = "57000";
                txtTolerancia.Text = "5";
                int nCapacidade = 57000;
                int nTara = int.Parse(txtTara.Text);
                int nPesoLiquido = nCapacidade - nTara;
                int nPesoTolerancia = (nPesoLiquido * 5) / 100;
                int nTotalCarga = nPesoLiquido + nPesoTolerancia;
                txtPBT.Text = nTotalCarga.ToString();

            }
            else if (tipoComposicao.Equals(composicao12))
            {
                //TOCO
                txtEixos.Text = "2";
                txtCap.Text = "16000";
                txtTolerancia.Text = "5";
                int nCapacidade = 16000;
                int nTara = int.Parse(txtTara.Text);
                int nPesoLiquido = nCapacidade - nTara;
                int nPesoTolerancia = (nPesoLiquido * 5) / 100;
                int nTotalCarga = nPesoLiquido + nPesoTolerancia;
                txtPBT.Text = nTotalCarga.ToString();
            }
            else if (tipoComposicao.Equals(composicao13))
            {
                // 3/4
                txtEixos.Text = "02";
                txtCap.Text = "3000";
                txtTolerancia.Text = "5";
                int nCapacidade = 3000;
                int nTara = int.Parse(txtTara.Text);
                int nPesoLiquido = nCapacidade - nTara;
                int nPesoTolerancia = (nPesoLiquido * 5) / 100;
                int nTotalCarga = nPesoLiquido + nPesoTolerancia;
                txtPBT.Text = nTotalCarga.ToString();
            }
            else if (tipoComposicao.Equals(composicao14))
            {
                // CAVALO SIMPLES COM PRANCHA
                txtEixos.Text = "05";
                txtCap.Text = "23000";
                txtTolerancia.Text = "5";
                int nCapacidade = 23000;
                int nTara = int.Parse(txtTara.Text);
                int nPesoLiquido = nCapacidade - nTara;
                int nPesoTolerancia = (nPesoLiquido * 5) / 100;
                int nTotalCarga = nPesoLiquido + nPesoTolerancia;
                txtPBT.Text = nTotalCarga.ToString();
            }
            else if (tipoComposicao.Equals(composicao15))
            {
                // CAVALO SIMPLES COM PRANCHA
                txtEixos.Text = "06";
                txtCap.Text = "23000";
                txtTolerancia.Text = "5";
                int nCapacidade = 23000;
                int nTara = int.Parse(txtTara.Text);
                int nPesoLiquido = nCapacidade - nTara;
                int nPesoTolerancia = (nPesoLiquido * 5) / 100;
                int nTotalCarga = nPesoLiquido + nPesoTolerancia;
                txtPBT.Text = nTotalCarga.ToString();
            }
            else if (tipoComposicao.Equals(composicao16))
            {
                //"CAVALO TRUCADO COM CARRETA SIMPLES TOTAL SIDER";
                txtEixos.Text = "06";
                txtCap.Text = "48500";
                txtTolerancia.Text = "5";
                int nCapacidade = 48500;
                int nTara = int.Parse(txtTara.Text);
                int nPesoLiquido = nCapacidade - nTara;
                int nPesoTolerancia = (nPesoLiquido * 5) / 100;
                int nTotalCarga = nPesoLiquido + nPesoTolerancia;
                txtPBT.Text = nTotalCarga.ToString();
            }
            else if (tipoComposicao.Equals(composicao17))
            {
                txtEixos.Text = "05";
                txtCap.Text = "46000";
                txtTolerancia.Text = "5";
                int nCapacidade = 41500;
                int nTara = int.Parse(txtTara.Text);
                int nPesoLiquido = nCapacidade - nTara;
                int nPesoTolerancia = (nPesoLiquido * 5) / 100;
                int nTotalCarga = nPesoLiquido + nPesoTolerancia;
                txtPBT.Text = nTotalCarga.ToString();
            }
        }

        

       

        protected void ddlComposicao_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(txtTara.Text !=string.Empty)
            {
                cboTipoCarreta_Leave();
            }
            else
            {
                string retorno = "É necessário preencher o campo Tara!";
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<script type = 'text/javascript'>");
                sb.Append("window.onload=function(){");
                sb.Append("alert('");
                sb.Append(retorno);
                sb.Append("')};");
                sb.Append("</script>");
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
                ddlComposicao.SelectedValue = "";
            }
            
        }
    }
}