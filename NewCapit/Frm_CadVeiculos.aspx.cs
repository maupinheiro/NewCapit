

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Newtonsoft.Json;
using System.Web.Services;


namespace NewCapit
{
    public partial class Frm_CadVeiculos : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString());
        int sequencia;
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
                PreencherComboAgregados();
                PreencherComboRastreadores();
                PreencherComboFiliais();
                PreencherComboMarcasVeiculos();
                PreencherComboCoresVeiculos();
                PreencherComboRastreadores();

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
                    ddlTransportadora.DataSource = reader;
                    ddlTransportadora.DataTextField = "Nome";
                    ddlTransportadora.DataValueField = "codtra";
                    ddlTransportadora.DataBind();

                    ddlTransportadora.Items.Insert(0, "Selecione Proprietário/Transportadora");

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


        protected void btnVeiculo_Click(object sender, EventArgs e)
        {
            string cod = txtCodVei.Text;
            string sql = "select * from tbveiculos where codvei='" + cod + "'";
            SqlDataAdapter da = new SqlDataAdapter(sql, con);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                string retorno = "o código digitado já possui veículo vinculado!";
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<script type = 'text/javascript'>");
                sb.Append("window.onload=function(){");
                sb.Append("alert('");
                sb.Append(retorno);
                sb.Append("')};");
                sb.Append("</script>");
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());

            }
            
        }

        protected void btnSalvar1_Click(object sender, EventArgs e)
        {
            

            string[] cod_transp = ddlTransportadora.SelectedItem.ToString().Split('-');

            
            string sql = @"
                            INSERT INTO tbveiculos (
                            id, codvei, tipvei, tipoveiculo, modelo, ano, nucleo, ativo_inativo, plavei, 
                            rastreamento, codrastreador, rastreador, codtra, transp, 
                            usucad, dtccad, protocolocet, venclicenciamento, marca, renavan, cor, 
                            comunicacao, antt
                        ) VALUES (
                             @id,@codvei, @tipvei, @tipoveiculo, @modelo, @ano, @nucleo, @ativo_inativo, @plavei, 
                             @rastreamento, @codrastreador, @rastreador, @codtra, @transp, 
                             @usucad, @dtccad, @protocolocet, @venclicenciamento, @marca, @renavan, @cor, 
                             @comunicacao, @antt
                        )";

            try
            {
                using (SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    GerarNumero();
                    // Adicionando os parâmetros da inserção
                    cmd.Parameters.AddWithValue("@codvei", txtCodVei.Text);
                    cmd.Parameters.AddWithValue("@tipvei", cboTipo.SelectedValue);
                    cmd.Parameters.AddWithValue("@tipoveiculo", ddlTipo.SelectedValue);
                    cmd.Parameters.AddWithValue("@modelo", txtModelo.Text);
                    cmd.Parameters.AddWithValue("@ano", txtAno.Text);
                    cmd.Parameters.AddWithValue("@nucleo", cbFiliais.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@ativo_inativo", status.SelectedValue);
                    cmd.Parameters.AddWithValue("@plavei", txtPlaca.Text);
                    cmd.Parameters.AddWithValue("@rastreamento", ddlMonitoramento.SelectedValue);
                    cmd.Parameters.AddWithValue("@codrastreador", txtCodRastreador.Text);
                    cmd.Parameters.AddWithValue("@rastreador", ddlTecnologia.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@codtra", txtCodTra.Text);
                    cmd.Parameters.AddWithValue("@transp", cod_transp[1]);
                    cmd.Parameters.AddWithValue("@usucad", txtUsuCadastro.Text);
                    cmd.Parameters.AddWithValue("@dtccad", DateTime.Now.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@protocolocet", txtProtocolo.Text);
                    cmd.Parameters.AddWithValue("@venclicenciamento", txtLicenciamento.Text);
                    cmd.Parameters.AddWithValue("@marca", ddlMarca.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@renavan", txtRenavam.Text);
                    cmd.Parameters.AddWithValue("@cor", ddlCor.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@comunicacao", ddlComunicacao.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@antt", txtAntt.Text);
                    cmd.Parameters.AddWithValue("@id", sequencia);

                    // Abrindo a conexão e executando a query
                    con.Open();
                    int rowsInserted = cmd.ExecuteNonQuery();

                    if (rowsInserted > 0)
                    {
                        string nomeUsuario = txtUsuCadastro.Text;
                        string mensagem = $"Olá, {nomeUsuario}!\nVeículo com código {txtCodVei.Text} cadastrado com sucesso.";
                        string script = $"alert('{HttpUtility.JavaScriptStringEncode(mensagem)}');";
                        ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", script, true);

                        // Redirecionar para a página de consulta
                        Response.Redirect("ConsultaVeiculos.aspx");
                    }
                    else
                    {
                        string mensagem = "Falha ao cadastrar o veículo. Tente novamente.";
                        string script = $"alert('{HttpUtility.JavaScriptStringEncode(mensagem)}');";
                        ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeErro", script, true);
                    }
                }
            }
            catch (Exception ex)
            {
                string mensagemErro = "Erro ao cadastrar o veículo: " + ex.Message;
                string scriptErro = $"alert('{HttpUtility.JavaScriptStringEncode(mensagemErro)}');";
                ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeErro", scriptErro, true);
            }
        }


        protected void ddlTransportadora_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] cod_transp = ddlTransportadora.SelectedItem.ToString().Split('-');

            txtCodTra.Text = cod_transp[0];

            string query = "SELECT antt FROM tbtransportadoras WHERE fl_exclusao IS NULL AND ativa_inativa = 'ATIVO'";

          
                query += " AND LTRIM(RTRIM(codtra)) ='" + cod_transp[0]+"'";
           

            query += " ORDER BY fantra";

            using (SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                

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

                txtAntt.Text = GetValue(row, 0);
                

            }
        }
    
        protected void ddlTecnologia_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtCodRastreador.Text = ddlTecnologia.SelectedValue.ToString();
        }
        public void GerarNumero()
        {
            string sql_sequncia = " select isnull(max(id+1),1) as id from tbveiculos";

            con.Open();

            SqlDataAdapter da = new SqlDataAdapter(sql_sequncia, con);

            DataTable dt2 = new DataTable();

            da.Fill(dt2);

            sequencia = int.Parse(dt2.Rows[0][0].ToString());

            con.Close();
        }
    }
}