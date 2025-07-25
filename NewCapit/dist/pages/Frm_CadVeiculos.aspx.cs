

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
using System.Configuration;
using System.Diagnostics.Eventing.Reader;
using NPOI.SS.Formula.Functions;
using static NPOI.HSSF.Util.HSSFColor;
using System.Threading;


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
                lblDtCadastro.Text = dataHoraAtual.ToString("dd/MM/yyyy HH:mm");

                CarregarDDLAgregados();
                PreencherComboRastreadores();
                PreencherComboFiliais();
                PreencherComboMarcasVeiculos();
                PreencherComboCoresVeiculos();
                PreencherComboRastreadores();
                PreencherComboEstados();
                PreencherComboCboTipo();
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
                    string query = "SELECT TOP 1 plavei FROM tbveiculos WHERE plavei LIKE @termo";
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

                //lblResultado.Text = resultado;
                txtPlaca.Text.ToUpper();
                ddlEstados.Focus();
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

                ddlCidades.Items.Insert(0, new ListItem("-- Selecione uma cidade --", "0"));
            }
        }
        private void PreencherComboEstados()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT uf, SiglaUf FROM tbestadosbrasileiros";

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
                    ddlEstados.DataValueField = "uf";  // Campo que será o valor de cada item                    
                    ddlEstados.DataBind();  // Realiza o binding dos dados                   
                    ddlEstados.Items.Insert(0, new ListItem("", "0"));
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
                    cbFiliais.Items.Insert(0, new ListItem("Selecione...", "0"));
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
                    ddlMarca.Items.Insert(0, new ListItem("Selecione...", "0"));
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
                    ddlCor.Items.Insert(0, new ListItem("Selecione...", "0"));
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

                    ddlTecnologia.Items.Insert(0, new ListItem("Selecione...", "0"));

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
       
        protected void btnSalvar1_Click(object sender, EventArgs e)
        {
            //string[] cod_transp = ddlAgregados.SelectedItem.ToString().Split('-');

            string sql = @"INSERT INTO tbveiculos (codvei, tipvei, tipoveiculo, modelo, ano, dtcvei, nucleo, ativo_inativo, plavei, rastreamento, codrastreador, rastreador, codtra, transp,usucad, dtccad, venclicenciamento, marca, renavan, cor, comunicacao, antt, ufplaca, cidplaca, dataaquisicao, comprimento, largura, altura, tacografo, modelotacografo, controlepatrimonio, chassi, terminal, codigo, venccronotacografo, vencimentolaudofumaca)
              VALUES
              (@codvei, @tipvei, @tipoveiculo, @modelo, @ano, @dtcvei, @nucleo, @ativo_inativo, @plavei, @rastreamento, @codrastreador, @rastreador, @codtra, @transp, @usucad, @dtccad, @venclicenciamento, @marca, @renavan, @cor, @comunicacao, @antt, @ufplaca, @cidplaca, @dataaquisicao, @comprimento, @largura, @altura, @tacografo, @modelotacografo, @controlepatrimonio, @chassi, @terminal, @codigo, @venccronotacografo, @vencimentolaudofumaca)";

            try
            {
                using (SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {

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
                    cmd.Parameters.AddWithValue("@ufplaca", ddlEstados.SelectedItem.ToString().Trim().ToUpper());
                    cmd.Parameters.AddWithValue("@cidplaca", ddlCidades.SelectedItem.ToString().Trim().ToUpper());
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
                    cmd.Parameters.AddWithValue("@venccronotacografo", DateTime.Parse(txtCronotacografo.Text.Trim()).ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@vencimentolaudofumaca", txtOpacidade.Text.Trim());


                    // Abrindo a conexão e executando a query
                    con.Open();
                    int rowsInserted = cmd.ExecuteNonQuery();

                    if (rowsInserted > 0)
                    {
                       
                        ExibirToastCadastro("Os dados foram salvos com sucesso!");
                        Thread.Sleep(5000);
                        // Redirecionar para a página de consulta
                        Response.Redirect("ConsultaVeiculos.aspx");
                    }
                    else
                    {
                        ExibirToastErro("Erro ao salvar os dados. Tente novamente.");
                        Thread.Sleep(5000);
                    }
                }
            }
            catch (Exception ex)
            {

                ExibirToastErro("Erro ao salvar os dados do veículo. " + ex.Message);
                Thread.Sleep(5000);
                Response.Redirect("ConsultaVeiculos.aspx");

            }
        }
        protected void ddlTecnologia_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtCodRastreador.Text = ddlTecnologia.SelectedValue.ToString();
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
        protected void ddlCidades_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["CidadeSelecionada"] = ddlCidades.SelectedValue;
        }        
        protected void txtCodVei_TextChanged(object sender, EventArgs e)
        {
            if (txtCodVei.Text != "")
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

                    ExibirToast("Código: " + txtCodVei.Text.Trim() + ", já cadastrado no sistema.");
                    Thread.Sleep(5000);
                    txtCodVei.Text = "";
                    txtCodVei.Focus();
                }
            }            
        }
        protected void txtCodRastreador_TextChanged(object sender, EventArgs e)
        {
            if (txtCodRastreador.Text != "")
            {
                string cod = txtCodRastreador.Text;
                string sql = "SELECT codRastreador, nomRastreador, codBuonny FROM tbrastreadores where codRastreador='" + cod + "'";
                SqlDataAdapter da = new SqlDataAdapter(sql, con);
                DataTable dt = new DataTable();
                con.Open();
                da.Fill(dt);
                con.Close();

                if (dt.Rows.Count > 0)
                {
                    ddlTecnologia.SelectedItem.Text = dt.Rows[0][1].ToString();
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
                    }
                    else if (dt.Rows[0][2].ToString() == "INATIVO")
                    {
                        ExibirToastErro("Código: " + txtCodTra.Text.Trim() + ", inativo no sistema.");
                        Thread.Sleep(5000);
                        txtCodVei.Text = "";
                        txtCodVei.Focus();                        
                    }
                    else
                    {
                        ddlAgregados.SelectedItem.Text = dt.Rows[0][1].ToString();
                        txtAntt.Text = dt.Rows[0][4].ToString();
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
        private void PreencherComboCboTipo()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT id, descricao FROM tbtipos_veiculos";

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
        protected void cboTipo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}