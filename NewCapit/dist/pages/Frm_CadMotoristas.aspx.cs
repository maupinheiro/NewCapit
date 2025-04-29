using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using Domain;

namespace NewCapit.dist.pages
{
    
    public partial class Frm_CadMotoristas : System.Web.UI.Page
    {
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
                PreencherComboFiliais();
                PreencherComboCargo();
                PreencherComboUFCNH();                
                PreencherComboJornada();
                CarregarDDLAgregados();

            }
            DateTime dataHoraAtual = DateTime.Now;
            txtDtCad.Text = dataHoraAtual.ToString("dd/MM/yyyy");
            lblDtCadastro.Text = dataHoraAtual.ToString("dd/MM/yyyy HH:mm");


        }
        protected void ddlRegiao_SelectedIndexChanged(object sender, EventArgs e)
        {
            int regiaoId = int.Parse(ddlRegiao.SelectedValue);
            CarregarEstados(regiaoId);
        }
        private void CarregarEstados(int regiaoId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT Uf,idRegiao, SiglaUf FROM tbestadosbrasileiros WHERE IdRegiao = @CategoriaId", conn);
                cmd.Parameters.AddWithValue("@CategoriaId", regiaoId);
                conn.Open();
                ddlUF.DataSource = cmd.ExecuteReader();
                ddlUF.DataTextField = "SiglaUf";
                ddlUF.DataValueField = "Uf";
                ddlUF.DataBind();

                ddlUF.Items.Insert(0, new ListItem("Selecione", "0"));
            }
        }
        protected void ddlUF_SelectedIndexChanged(object sender, EventArgs e)
        {
            int municipioId = int.Parse(ddlUF.SelectedValue);
            CarregarMunicipios(municipioId); 
        }
        
        private void CarregarMunicipios(int municipioId)
        {            
            string connectionString = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT id, Uf,IdRegiao, Sigla, nome_municipio FROM tbmunicipiosbrasileiros WHERE Uf = @MunicipioId", conn);
                cmd.Parameters.AddWithValue("@MunicipioId", municipioId);
                conn.Open();
                ddlMunicipioNasc.DataSource = cmd.ExecuteReader();


                //ddlMunicipioNasc.Items.Insert(0, new ListItem("Selecione", "0"));

                string cidadeSelecionada = ddlMunicipioNasc.SelectedValue;
                if (ViewState["cidadeSelecionada"] != null && ddlMunicipioNasc.Items.Count > 0)
                {
                    
                    ddlMunicipioNasc.SelectedValue = ViewState["cidadeSelecionada"].ToString();
                }
                else
                {
                    ddlMunicipioNasc.DataTextField = "nome_municipio";
                    ddlMunicipioNasc.DataValueField = "Uf";
                    ddlMunicipioNasc.DataBind();
                    ddlMunicipioNasc.Items.Insert(0, new ListItem("Selecione", "0"));                    
                }



            }
        }
        protected void ddlMunicipioNasc_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idSelecionado = int.Parse(ddlMunicipioNasc.SelectedValue);

            // Preencher os campos com base no valor selecionado
            if (idSelecionado > 0)
            {
                PreencherCamposCidNasc(idSelecionado);
            }
            else
            {
                LimparCamposCidNasc();
            }
        }
        private void PreencherCamposCidNasc(int id)
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                conn.Open();
                string query = "SELECT id, nome_municipio FROM tbmunicipiosbrasileiros WHERE id = @ID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    ddlMunicipioNasc.Text = reader["nome_municipio"].ToString();                    

                }
            }
        }
        // Função para limpar os campos
        private void LimparCamposCidNasc()
        {
            ddlMunicipioNasc.Text = "Selecione";
        }




        protected void txtCodMot_TextChanged(object sender, EventArgs e)
        {
            string termo = txtCodMot.Text.ToUpper();
            string strConn = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(strConn))
            {
                string query = "SELECT TOP 1 codmot FROM tbmotoristas WHERE codmot LIKE @termo";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@termo", "%" + termo + "%");
                conn.Open();

                object res = cmd.ExecuteScalar();
                if (res != null)
                {
                    //resultado = "Resultado: " + res.ToString();
                    string retorno = "Código: " + res.ToString() + ", já cadastrado. ";
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append("<script type = 'text/javascript'>");
                    sb.Append("window.onload=function(){");
                    sb.Append("alert('");
                    sb.Append(retorno);
                    sb.Append("')};");
                    sb.Append("</script>");
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
                    txtCodMot.Text = "";
                    txtCodMot.Focus();
                }
            }

            //lblResultado.Text = resultado;
            txtCodMot.Text.ToUpper();
           // ddlTipoMot.Focus();
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
                string query = "SELECT codtra, fantra FROM tbtransportadoras WHERE ID = @ID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtCodTra.Text = reader["codtra"].ToString();
                    //ddlAgregados.Text = reader["fantra"].ToString();
                   
                }
            }
        }
        // Função para limpar os campos
        private void LimparCampos()
        {
            txtCodTra.Text = string.Empty;
            
        }
        private void PreencherComboUFCNH()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT Uf, SiglaUf FROM tbestadosbrasileiros ORDER BY SiglaUf";

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
                    ddlCNH.DataSource = reader;
                    ddlCNH.DataTextField = "SiglaUf";  // Campo que será mostrado no ComboBox
                    ddlCNH.DataValueField = "Uf";  // Campo que será o valor de cada item                    
                    ddlCNH.DataBind();  // Realiza o binding dos dados                   
                    ddlCNH.Items.Insert(0, new ListItem("", "0"));
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
        private void CarregarMunicipiosCNH(int municipioCNHId)
        {           
            string connectionString = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT Uf,IdRegiao, Sigla, nome_municipio FROM tbmunicipiosbrasileiros WHERE Uf = @MunicipioCNHId", conn);
                cmd.Parameters.AddWithValue("@MunicipioCNHId", municipioCNHId);
                conn.Open();
                ddlMunicCnh.DataSource = cmd.ExecuteReader();
                ddlMunicCnh.DataTextField = "nome_municipio";
                ddlMunicCnh.DataValueField = "Uf";
                ddlMunicCnh.DataBind();

                ddlMunicCnh.Items.Insert(0, new ListItem("Selecione", "0"));
            }
        }
        protected void ddlCNH_SelectedIndexChanged(object sender, EventArgs e)
        {
            int municipioCNHId = int.Parse(ddlCNH.SelectedValue);
            CarregarMunicipiosCNH(municipioCNHId);
        }              
        private void CarregarCargos()
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                conn.Open();
                string query = "SELECT cod_funcao, nm_funcao FROM tb_funcao ORDER BY nm_funcao";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                ddlCargo.DataSource = reader;
                ddlCargo.DataTextField = "nm_funcao";  // Campo a ser exibido
                ddlCargo.DataValueField = "cod_funcao";  // Valor associado ao item
                ddlCargo.DataBind();

                // Adicionar o item padrão
                ddlCargo.Items.Insert(0, new ListItem("", "0"));
            }
        }
        // Evento disparado quando o item do DropDownList é alterado
        protected void ddlCargo_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idSelecionado = int.Parse(ddlCargo.SelectedValue);

            // Preencher os campos com base no valor selecionado
            if (idSelecionado > 0)
            {
               // PreencherCamposCargo(idSelecionado);
            }
            else
            {
                // LimparCamposCidades();
            }
        }
        // Função para preencher os campos com os dados do banco
        private void PreencherComboCargo()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT cod_funcao, nm_funcao FROM tb_funcao";

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
                    ddlCargo.DataSource = reader;
                    ddlCargo.DataTextField = "nm_funcao";  // Campo que será mostrado no ComboBox
                    ddlCargo.DataValueField = "cod_funcao";  // Campo que será o valor de cada item                    
                    ddlCargo.DataBind();  // Realiza o binding dos dados                   
                                          // Adicionar o item padrão
                    ddlCargo.Items.Insert(0, new ListItem("", "0"));
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
        // Função para limpar os campos
        private void LimparCamposCargo()
        {
            ddlCargo.Text = string.Empty;
        }
        // Função para preencher a combo Filial
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
        private void PreencherComboJornada()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT id, descricao FROM tbhorarios";

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
                    ddlJornada.DataSource = reader;
                    ddlJornada.DataTextField = "descricao";  // Campo que será mostrado no ComboBox
                    ddlJornada.DataValueField = "id";  // Campo que será o valor de cada item                    
                    ddlJornada.DataBind();  // Realiza o binding dos dados                   
                    ddlJornada.Items.Insert(0, new ListItem("", "0"));
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

    }
}