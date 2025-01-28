

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
                

            }
            PreencherComboFiliais();
            PreencherComboMarcasVeiculos();
            PreencherComboCoresVeiculos();
            PreencherComboRastreadores();
            


           



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
            string sql = @"
        INSERT INTO tbveiculos (
            codvei, tipvei, tipoveiculo, modelo, ano, nucleo, ativo_inativo, plavei, 
            rastreamento, codrastreador, rastreador
            codmot, motorista, codtra, transp, 
            usucad, dtccad, protocolocet, venclicenciamento, marca, renavan, cor, 
            comunicacao, antt
        ) VALUES (
            @codvei, @tipvei, @tipoveiculo, @modelo, @ano, @nucleo, @ativo_inativo, @plavei, 
            @rastreamento, @codrastreador,,@rastreador 
            @codmot, @motorista, @codtra, @transp, 
            @usucad, @dtccad, @protocolocet, @venclicenciamento, @marca, @renavan, @cor, 
            @comunicacao, @antt
        )";

            //try
            //{
                using (SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    // Adicionando os parâmetros da inserção
                    cmd.Parameters.AddWithValue("@codvei", txtCodVei.Text);
                    cmd.Parameters.AddWithValue("@tipvei", cboTipo.SelectedValue);
                    cmd.Parameters.AddWithValue("@tipoveiculo", ddlTipo.SelectedValue);
                    cmd.Parameters.AddWithValue("@modelo", txtModelo.Text);
                    cmd.Parameters.AddWithValue("@ano", txtAno.Text);
                    cmd.Parameters.AddWithValue("@nucleo", cbFiliais.SelectedValue);
                    cmd.Parameters.AddWithValue("@ativo_inativo", status.SelectedValue);
                    cmd.Parameters.AddWithValue("@plavei", txtPlaca.Text);
                    //cmd.Parameters.AddWithValue("@reboque1", txtReb1.Text);
                    //cmd.Parameters.AddWithValue("@reboque2", txtReb2.Text);
                    //cmd.Parameters.AddWithValue("@tipocarreta", ddlCarreta.SelectedValue);
                    //cmd.Parameters.AddWithValue("@tiporeboque", ddlComposicao.SelectedValue);
                    cmd.Parameters.AddWithValue("@rastreamento", ddlMonitoramento.SelectedValue);
                    cmd.Parameters.AddWithValue("@codrastreador", txtCodRastreador.Text);
                    cmd.Parameters.AddWithValue("@rastreador", ddlTecnologia.SelectedValue);
                    //cmd.Parameters.AddWithValue("@cap", txtEixos.Text);
                    //cmd.Parameters.AddWithValue("@eixos", txtEixos.Text);
                    //cmd.Parameters.AddWithValue("@tara", txtTara.Text);
                    //cmd.Parameters.AddWithValue("@tolerancia", txtTolerancia.Text);
                    //cmd.Parameters.AddWithValue("@codmot", txtCodMot.Text);
                    //cmd.Parameters.AddWithValue("@motorista", ddlMotorista.SelectedValue);
                    cmd.Parameters.AddWithValue("@codtra", txtCodTra.Text);
                    cmd.Parameters.AddWithValue("@transp", ddlTransportadora.SelectedValue);
                    cmd.Parameters.AddWithValue("@usucad", txtUsuCadastro.Text);
                    cmd.Parameters.AddWithValue("@dtccad", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@protocolocet", txtProtocolo.Text);
                    cmd.Parameters.AddWithValue("@venclicenciamento", txtLicenciamento.Text);
                    cmd.Parameters.AddWithValue("@marca", ddlMarca.SelectedValue);
                    cmd.Parameters.AddWithValue("@renavan", txtRenavam.Text);
                    cmd.Parameters.AddWithValue("@cor", ddlCor.SelectedValue);
                    cmd.Parameters.AddWithValue("@comunicacao", ddlComunicacao.SelectedValue);
                    cmd.Parameters.AddWithValue("@antt", txtAntt.Text);

                    con.Open();
                    int rowsInserted = cmd.ExecuteNonQuery();

                    if (rowsInserted > 0)
                    {
                        // Log ou mensagem indicando sucesso na inserção
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
                        // Log ou mensagem indicando falha na inserção
                    }
                }
            //}
            //catch (Exception ex)
            //{
            //    // Log ou tratamento do erro
            //    // Exemplo: ex.Message
            //}
        }

        protected void ddlTransportadora_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] cod_transp = ddlTransportadora.SelectedItem.ToString().Split('-');

            txtCodTra.Text = cod_transp[0];
    }
    
        protected void ddlTecnologia_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtCodRastreador.Text = ddlTecnologia.SelectedValue.ToString();
        }
    }
    }