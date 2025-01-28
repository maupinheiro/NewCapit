using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using static System.Net.Mime.MediaTypeNames;

namespace NewCapit
{
    public partial class Frm_AltVeiculos : System.Web.UI.Page
    {
        string id;
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
                txtCadastro.Text = dataHoraAtual.ToString("dd/MM/yyyy");
                lblDtCadastro.Text = dataHoraAtual.ToString("dd/MM/yyyy HH:mm");

                txtTolerancia.Text = "5";
                CarregaTransportadoras();
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
            string composicao11 = "BITREM";
            string composicao12 = "TOCO";
            string composicao13 = "VEICULO 3/4";
            string composicao14 = "CAVALO SIMPLES COM PRANCHA";
            string composicao15 = "CAVALO TRUCADO COM PRANCHA";

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
        }

        protected void ddlComposicao_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (txtTara.Text != string.Empty)
            {
                cboTipoCarreta_Leave();
            }
            else
            {
                string retorno = "É necessário digitar o valor no campo Tara!";
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

        public void CarregaTransportadoras()
        {
            string id = HttpContext.Current.Request.QueryString["id"];

            // Verifica se o ID está presente na query string
            if (string.IsNullOrEmpty(id))
            {
                // Log ou mensagem indicando que o ID não foi informado
                return;
            }

            string sql = @"SELECT * FROM tbveiculos WHERE id = @id";

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
                    cboTipo.Items.Insert(0, GetValue(row, 2));
                    ddlTipo.Items.Insert(0, GetValue(row, 3));
                    txtModelo.Text = GetValue(row, 4);
                    txtAno.Text = GetValue(row, 5);
                    cbFiliais.Items.Insert(0, GetValue(row, 7));
                    status.Items.Insert(0, GetValue(row, 8));
                    txtPlaca.Text = GetValue(row, 9);
                    txtReb1.Text = GetValue(row, 10);
                    txtReb2.Text = GetValue(row, 11);
                    ddlCarreta.Items.Insert(0, GetValue(row, 12));
                    ddlComposicao.Items.Insert(0, GetValue(row, 13));
                    ddlMonitoramento.Items.Insert(0, GetValue(row, 14));
                    txtCodRastreador.Text = GetValue(row, 15);
                    ddlTecnologia.Items.Insert(0, GetValue(row, 16));
                    txtEixos.Text = GetValue(row, 19);
                    txtTara.Text = GetValue(row, 20);
                    txtTolerancia.Text = GetValue(row, 21);
                    txtCodMot.Text = GetValue(row, 27);
                    ddlMotorista.Items.Insert(0, GetValue(row, 28));
                    txtCodTra.Text = GetValue(row, 29);
                    ddlTransportadora.Items.Insert(0, GetValue(row, 30));
                    txtProtocolo.Text = GetValue(row, 37);
                    txtLicenciamento.Text = GetValue(row, 38);
                    ddlMarca.Items.Insert(0, GetValue(row, 39));
                    txtRenavam.Text = GetValue(row, 40);
                    ddlCor.Items.Insert(0, GetValue(row, 41));
                    ddlComunicacao.Items.Insert(0, GetValue(row, 42));
                    txtAntt.Text = GetValue(row, 43);

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
            string id = HttpContext.Current.Request.QueryString["id"];

            // Verifica se o ID está presente na query string
            if (string.IsNullOrEmpty(id))
            {
                // Log ou mensagem indicando que o ID não foi informado
                return;
            }

            string sql = @"
        UPDATE tbveiculos 
        SET 
            codvei = @codvei,
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
            tara = @tara,
            tolerancia = @tolerancia,
            codmot = @codmot,
            motorista = @motorista,
            codtra = @codtra,
            transp = @transp,
            usucad = @usucad,
            dtccad = @dtccad,
            usualt = @usualt,
            dtcalt = @dtcalt,
            protocolocet = @protocolocet,
            venclicenciamento = @venclicenciamento,
            marca = @marca,
            renavan = @renavan,
            cor = @cor,
            comunicacao = @comunicacao,
            antt = @antt
        WHERE id = @id";

            try
            {
                using (SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    // Parâmetros de atualização
                    cmd.Parameters.AddWithValue("@codvei", txtCodVei.Text);
                    cmd.Parameters.AddWithValue("@tipvei", cboTipo.SelectedValue);
                    cmd.Parameters.AddWithValue("@tipoveiculo", ddlTipo.SelectedValue);
                    cmd.Parameters.AddWithValue("@modelo", txtModelo.Text);
                    cmd.Parameters.AddWithValue("@ano", txtAno.Text);
                    cmd.Parameters.AddWithValue("@nucleo", cbFiliais.SelectedValue);
                    cmd.Parameters.AddWithValue("@ativo_inativo", status.SelectedValue);
                    cmd.Parameters.AddWithValue("@plavei", txtPlaca.Text);
                    cmd.Parameters.AddWithValue("@reboque1", txtReb1.Text);
                    cmd.Parameters.AddWithValue("@reboque2", txtReb2.Text);
                    cmd.Parameters.AddWithValue("@tipocarreta", ddlCarreta.SelectedValue);
                    cmd.Parameters.AddWithValue("@tiporeboque", ddlComposicao.SelectedValue);
                    cmd.Parameters.AddWithValue("@rastreamento", ddlMonitoramento.SelectedValue);
                    cmd.Parameters.AddWithValue("@codrastreador", txtCodRastreador.Text);
                    cmd.Parameters.AddWithValue("@rastreador", ddlTecnologia.SelectedValue);
                    cmd.Parameters.AddWithValue("@eixo", txtEixos.Text);
                    cmd.Parameters.AddWithValue("@tara", txtTara.Text);
                    cmd.Parameters.AddWithValue("@tolerancia", txtTolerancia.Text);
                    cmd.Parameters.AddWithValue("@codmot", txtCodMot.Text);
                    cmd.Parameters.AddWithValue("@motorista", ddlMotorista.SelectedValue);
                    cmd.Parameters.AddWithValue("@codtra", txtCodTra.Text);
                    cmd.Parameters.AddWithValue("@transp", ddlTransportadora.SelectedValue);
                    cmd.Parameters.AddWithValue("@usualt", HttpContext.Current.User.Identity.Name); // Exemplo para usuário atual
                    cmd.Parameters.AddWithValue("@dtcalt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@protocolocet", txtProtocolo.Text);
                    cmd.Parameters.AddWithValue("@venclicenciamento", txtLicenciamento.Text);
                    cmd.Parameters.AddWithValue("@marca", ddlMarca.SelectedValue);
                    cmd.Parameters.AddWithValue("@renavan", txtRenavam.Text);
                    cmd.Parameters.AddWithValue("@cor", ddlCor.SelectedValue);
                    cmd.Parameters.AddWithValue("@comunicacao", ddlComunicacao.SelectedValue);
                    cmd.Parameters.AddWithValue("@antt", txtAntt.Text);
                    cmd.Parameters.AddWithValue("@id", id);

                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        // Log ou mensagem indicando sucesso
                        string nomeUsuario = txtUsuCadastro.Text;
                        string linha1 = "Olá, " + nomeUsuario + "!";
                        string linha2 = "Código " + txtCodTra.Text + ", cadastrado com sucesso.";
                        // Concatenando as linhas com '\n' para criar a mensagem
                        string mensagem = $"{linha1}\n{linha2}";
                        string mensagemCodificada = HttpUtility.JavaScriptStringEncode(mensagem);
                        // Gerando o script JavaScript para exibir o alerta
                        string script = $"alert('{mensagemCodificada}');";
                        // Registrando o script para execução no lado do cliente
                        ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", script, true);
                        //Chama a página de consulta clientes
                        Response.Redirect("ConsultaVeiculos.aspx");
                    }
                    else
                    {
                        // Log ou mensagem indicando falha na atualização
                    }
                }
            }
            catch (Exception ex)
            {
                // Log ou tratamento do erro
                // Exemplo: ex.Message
            }
        }

        protected void ddlTransportadora_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] cod_transp = ddlTransportadora.SelectedItem.ToString().Split('-');

            txtCodTra.Text = cod_transp[0];
        }
    }
}