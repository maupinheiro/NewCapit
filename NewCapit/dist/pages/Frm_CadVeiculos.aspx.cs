

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
                //txtCadastro.Text = dataHoraAtual.ToString("dd/MM/yyyy");
                lblDtCadastro.Text = dataHoraAtual.ToString("dd/MM/yyyy HH:mm");                
                CarregarDDLAgregados();
                PreencherComboRastreadores();
                PreencherComboFiliais();
                PreencherComboMarcasVeiculos();
                PreencherComboCoresVeiculos();
                PreencherComboRastreadores();
                PreencherComboEstados();

            }
        }
        private void PreencherComboEstados()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT id, SiglaUf FROM tbestadosbrasileiros";

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
                    ddlUfPlaca.DataSource = reader;
                    ddlUfPlaca.DataTextField = "SiglaUf";  // Campo que será mostrado no ComboBox
                    ddlUfPlaca.DataValueField = "id";  // Campo que será o valor de cada item                    
                    ddlUfPlaca.DataBind();  // Realiza o binding dos dados                   
                    ddlUfPlaca.Items.Insert(0, new ListItem("", "0"));
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
                    cbFiliais.Items.Insert(0, new ListItem("", "0"));
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
                    ddlMarca.Items.Insert(0, new ListItem("", "0"));
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
                    ddlCor.Items.Insert(0, new ListItem("", "0"));
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
                                               
                    ddlTecnologia.Items.Insert(0, new ListItem("", "0"));

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
                string retorno = "Frota: " + cod + ", já possui veículo vinculado!";
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<script type = 'text/javascript'>");
                sb.Append("window.onload=function(){");
                sb.Append("alert('");
                sb.Append(retorno);
                sb.Append("')};");
                sb.Append("</script>");
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
                txtCodVei.Text = "";
                txtCodVei.Focus();
            }
            

        }

        protected void btnSalvar1_Click(object sender, EventArgs e)
        {
           //string[] cod_transp = ddlAgregados.SelectedItem.ToString().Split('-');
            
            string sql = @"INSERT INTO tbveiculos (id, codvei, tipvei, tipoveiculo, modelo, ano, dtcvei, nucleo, ativo_inativo, plavei, rastreamento, codrastreador, rastreador, codtra, transp,usucad, dtccad, venclicenciamento, marca, renavan, cor, comunicacao, antt, ufplaca, cidplaca, dataaquisicao, comprimento, largura, altura, tacografo, modelotacografo, controlepatrimonio, chassi, terminal, codigo)
              VALUES
              (@id,@codvei, @tipvei, @tipoveiculo, @modelo, @ano, @dtcvei, @nucleo, @ativo_inativo, @plavei, @rastreamento, @codrastreador, @rastreador, @codtra, @transp, @usucad, @dtccad, @venclicenciamento, @marca, @renavan, @cor, @comunicacao, @antt, @ufplaca, @cidplaca, @dataaquisicao, @comprimento, @largura, @altura, @tacografo, @modelotacografo, @controlepatrimonio, @chassi, @terminal, @codigo)";

            try
            {
                using (SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    GerarNumero();
                    // Adicionando os parâmetros da inserção
                    cmd.Parameters.AddWithValue("@codvei", txtCodVei.Text.Trim().ToUpper());
                    cmd.Parameters.AddWithValue("@tipvei", cboTipo.SelectedValue.Trim().ToUpper());                    
                    cmd.Parameters.AddWithValue("@tipoveiculo", ddlTipo.SelectedValue.Trim().ToUpper());
                    cmd.Parameters.AddWithValue("@modelo", txtModelo.Text.Trim().ToUpper());
                    cmd.Parameters.AddWithValue("@ano", txtAno.Text.Trim());
                    cmd.Parameters.AddWithValue("@dtcvei", DateTime.Now.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@nucleo", cbFiliais.SelectedItem.ToString().Trim().ToUpper());
                    cmd.Parameters.AddWithValue("@ativo_inativo", "ATIVO");
                    cmd.Parameters.AddWithValue("@plavei", txtPlaca.Text.Trim().ToUpper());
                    cmd.Parameters.AddWithValue("@rastreamento", ddlMonitoramento.SelectedValue.Trim().ToUpper());
                    cmd.Parameters.AddWithValue("@codrastreador", txtCodRastreador.Text.Trim());
                    cmd.Parameters.AddWithValue("@rastreador", ddlTecnologia.SelectedItem.ToString().Trim().ToUpper());
                    cmd.Parameters.AddWithValue("@codtra", txtCodTra.Text.Trim().ToUpper());
                    cmd.Parameters.AddWithValue("@transp", ddlAgregados.SelectedItem.ToString().Trim().ToUpper());
                    cmd.Parameters.AddWithValue("@usucad", txtUsuCadastro.Text.Trim().ToUpper());
                    cmd.Parameters.AddWithValue("@dtccad", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));                    
                    cmd.Parameters.AddWithValue("@venclicenciamento", txtLicenciamento.Text.Trim());
                    cmd.Parameters.AddWithValue("@marca", ddlMarca.SelectedItem.ToString().Trim().ToUpper());
                    cmd.Parameters.AddWithValue("@renavan", txtRenavam.Text.Trim());
                    cmd.Parameters.AddWithValue("@cor", ddlCor.SelectedItem.ToString().Trim().ToUpper());
                    cmd.Parameters.AddWithValue("@comunicacao", ddlComunicacao.SelectedItem.ToString().Trim());
                    cmd.Parameters.AddWithValue("@antt", txtAntt.Text.Trim());
                    cmd.Parameters.AddWithValue("@ufplaca", ddlUfPlaca.SelectedItem.ToString().Trim().ToUpper());
                    cmd.Parameters.AddWithValue("@cidplaca", txtCidPlaca.Text.Trim().ToUpper());
                    cmd.Parameters.AddWithValue("@dataaquisicao", txtDataAquisicao.Text.Trim());
                    cmd.Parameters.AddWithValue("@comprimento", txtComprimento.Text.Trim());
                    cmd.Parameters.AddWithValue("@largura", txtLargura.Text.Trim());
                    cmd.Parameters.AddWithValue("@altura", txtAltura.Text.Trim());
                    cmd.Parameters.AddWithValue("@tacografo", ddlTacografo.SelectedItem.ToString().Trim().ToUpper());
                    cmd.Parameters.AddWithValue("@modelotacografo", ddlModeloTacografo.SelectedItem.ToString().Trim().ToUpper());
                    cmd.Parameters.AddWithValue("@controlepatrimonio", txtControlePatrimonio.Text.Trim().ToUpper());
                    cmd.Parameters.AddWithValue("@chassi", txtChassi.Text.Trim().ToUpper());
                    cmd.Parameters.AddWithValue("@terminal", txtId.Text.Trim());
                    cmd.Parameters.AddWithValue("@codigo", txtCodigo.Text.Trim());
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
                ddlAgregados.Items.Insert(0, new ListItem("", "0"));
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
                    //ddlAgregados.Text = reader["fantra"].ToString();
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





    }
}