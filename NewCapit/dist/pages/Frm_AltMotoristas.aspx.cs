using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using NPOI.SS.Formula.Functions;
using System.IO;
using NPOI.SS.UserModel;

namespace NewCapit.dist.pages
{
    public partial class Frm_AltMotoristas : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conexao"].ToString());
        string id;
        public string fotoMotorista;
        DateTime dataHoraAtual = DateTime.Now;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UsuarioLogado"] != null)
                {
                    string nomeUsuario = Session["UsuarioLogado"].ToString();
                    var lblUsuario = nomeUsuario;

                    //  txtAlteradoPor.Text = nomeUsuario;

                }
                else
                {
                    var lblUsuario = "<Usuário>";
                    //txtAlteradoPor.Text = lblUsuario;
                }
                
                PreencherComboCargo();
                PreencherComboFiliais();
                CarregarDDLEstadosBrasileiros();
                PreencherComboUFCNH();


                PreencherComboJornada();
                CarregarDDLAgregados();
                CarregaDadosMotorista();
            }
            
        }
        public void CarregaDadosMotorista()
        {
            if (HttpContext.Current.Request.QueryString["id"].ToString() != "")
            {
                id = HttpContext.Current.Request.QueryString["id"].ToString();
            }
            string sql = "SELECT codmot, nommot, status, CONVERT(varchar, emissaorg, 103) as emissaorg, numrg, cargo, nucleo, orgaorg, cpf, numregcnh, codsegurancacnh, catcnh, CONVERT(varchar, venccnh, 103) as venccnh, codliberacao, numpis, endmot, baimot, cidmot, ufmot, cepmot, fone3, fone2, validade, CONVERT(varchar, dtnasc, 103) as dtnasc, estcivil, sexo, horario, nomepai, nomemae, codtra, transp, CONVERT(varchar, cadmot, 103) as cadmot, inativo, CONVERT(varchar, dtinativo, 103) as dtinativo, historico, alterado, dataalteracao, cartaomot, naturalmot, numero, complemento, tipomot, codprop, placa, reboque1, reboque2, tipoveiculo, venccartao, horario, funcao, frota, usucad, dtccad, venceti, codvei, ufnascimento, formulariocnh, ufcnh, municipiocnh, vencmoop, cracha, regiao, numinss, caminhofoto FROM tbmotoristas WHERE id = " + id;

            SqlDataAdapter adpt = new SqlDataAdapter(sql, con);
            DataTable dt = new DataTable();
            con.Open();
            adpt.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                // Preenchendo os TextBoxes com valores do DataTable
                if (dt.Rows[0][0].ToString() != string.Empty)
                {
                    txtCodMot.Text = dt.Rows[0][0].ToString();
                }
                txtNomMot.Text = dt.Rows[0][1].ToString();
                ddlStatus.SelectedValue = dt.Rows[0][2].ToString();
                txtDtEmissao.Text = dt.Rows[0][3].ToString();
                txtRG.Text = dt.Rows[0][4].ToString();
                ddlCargo.SelectedItem.Text = dt.Rows[0][5].ToString();
                cbFiliais.SelectedItem.Text = dt.Rows[0][6].ToString();
                txtEmissor.Text = dt.Rows[0][7].ToString();
                txtCPF.Text = dt.Rows[0][8].ToString();
                txtRegCNH.Text = dt.Rows[0][9].ToString();
                txtCodSeguranca.Text = dt.Rows[0][10].ToString();
                ddlCat.SelectedItem.Text = dt.Rows[0][11].ToString();
                txtValCNH.Text = dt.Rows[0][12].ToString();
                txtCodLibRisco.Text = dt.Rows[0][13].ToString();
                txtPIS.Text = dt.Rows[0][14].ToString();
                txtEndCli.Text = dt.Rows[0][15].ToString();
                txtBaiCli.Text = dt.Rows[0][16].ToString();
                txtCidCli.Text = dt.Rows[0][17].ToString();
                txtEstCli.Text = dt.Rows[0][18].ToString();
                txtCepCli.Text = dt.Rows[0][19].ToString();
                txtFixo.Text = dt.Rows[0][20].ToString();
                txtCelular.Text = dt.Rows[0][21].ToString();
                txtValLibRisco.Text = dt.Rows[0][22].ToString();
                txtDtNasc.Text = dt.Rows[0][23].ToString();
                ddlEstCivil.SelectedItem.Text = dt.Rows[0][24].ToString();
                ddlSexo.SelectedItem.Text = dt.Rows[0][25].ToString();
                ddlJornada.SelectedItem.Text = dt.Rows[0][26].ToString();
                txtNomeMae.Text = dt.Rows[0][27].ToString();
                txtNomePai.Text = dt.Rows[0][28].ToString();
                txtCodTra.Text = dt.Rows[0][29].ToString();
                //ddlAgregados.SelectedValue = dt.Rows[0][30].ToString();
                txtDtCad.Text = dt.Rows[0][31].ToString();
                if (dt.Rows[0][32].ToString() != string.Empty)
                {
                    txtMotivoInativo.Text = dt.Rows[0][32].ToString();
                    txtDtInativacao.Text = dt.Rows[0][33].ToString();
                }
                if (dt.Rows[0][34].ToString() != string.Empty)
                {
                    txtHistorico.Text = dt.Rows[0][34].ToString();
                }
                txtAltCad.Text = dt.Rows[0][35].ToString();
                lbDtAtualizacao.Text = dt.Rows[0][36].ToString();
                txtCartao.Text = dt.Rows[0][37].ToString();
                ddlMunicipioNasc.Items.Insert(0, new ListItem(dt.Rows[0][38].ToString(), "0"));
                //ddlMunicipioNasc.SelectedItem.Text = dt.Rows[0][38].ToString();
                txtNumero.Text = dt.Rows[0][39].ToString();
                txtComplemento.Text = dt.Rows[0][40].ToString();
                ddlTipoMot.SelectedItem.Text = dt.Rows[0][41].ToString();
                txtCodProp.Text = dt.Rows[0][42].ToString() + "/" + dt.Rows[0][54].ToString();
                txtPlaca.Text = dt.Rows[0][43].ToString();
                txtReboque1.Text = dt.Rows[0][44].ToString();
                txtReboque2.Text = dt.Rows[0][45].ToString();
                txtTipoVeiculo.Text = dt.Rows[0][46].ToString();
                txtValCartao.Text = dt.Rows[0][47].ToString();
                ddlJornada.SelectedItem.Text = dt.Rows[0][48].ToString();
                ddlFuncao.SelectedItem.Text = dt.Rows[0][49].ToString();
                txtFrota.Text = dt.Rows[0][50].ToString();
                txtUsuCadastro.Text = dt.Rows[0][51].ToString();
                lblDtCadastro.Text = dt.Rows[0][52].ToString();
                txtVAlExameTox.Text = dt.Rows[0][53].ToString();



                ddlUF.SelectedItem.Text = dt.Rows[0][55].ToString();
                txtFormCNH.Text = dt.Rows[0][56].ToString();
                ddlCNH.Items.Insert(0, new ListItem(dt.Rows[0][57].ToString(), "0"));
                ddlMunicCnh.Items.Insert(0, new ListItem(dt.Rows[0][58].ToString(), "0"));
                txtVAlMoop.Text = dt.Rows[0][59].ToString();
                txtCracha.Text = dt.Rows[0][60].ToString();
                ddlRegiao.SelectedItem.Text = dt.Rows[0][61].ToString();
                txtINSS.Text = dt.Rows[0][62].ToString();
                txtCaminhoFoto.Text = dt.Rows[0][63].ToString();
                fotoMotorista = dt.Rows[0][63].ToString();
                

                //SALVAR A FOTO DO MOTORISTA
                // aspx
                //< asp:FileUpload ID = "FileUpload1" runat = "server" />
                //< asp:Button ID = "btnSalvar" runat = "server" Text = "Salvar Imagem" OnClick = "btnSalvar_Click" />
                //< asp:Label ID = "lblMensagem" runat = "server" ForeColor = "Green" />

                // csharp
                //        protected void btnSalvar_Click(object sender, EventArgs e)
                //{
                //    if (FileUpload1.HasFile)
                //    {
                //        try
                //        {
                //            // Nome original
                //            string originalName = Path.GetFileName(FileUpload1.FileName);

                //            // Novo nome (por exemplo, com timestamp)
                //            string novoNome = "img_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + Path.GetExtension(originalName);

                //            // Caminho da nova pasta (por exemplo: ~/FotosSalvas/)
                //            string pastaDestino = Server.MapPath("~/FotosSalvas/");

                //            // Cria a pasta se não existir
                //            if (!Directory.Exists(pastaDestino))
                //            {
                //                Directory.CreateDirectory(pastaDestino);
                //            }

                //            // Caminho completo para salvar a cópia
                //            string caminhoCompleto = Path.Combine(pastaDestino, novoNome);

                //            // Salva o arquivo com novo nome na nova pasta
                //            FileUpload1.SaveAs(caminhoCompleto);

                //            lblMensagem.Text = "Imagem salva com sucesso como " + novoNome;
                //        }
                //        catch (Exception ex)
                //        {
                //            lblMensagem.ForeColor = System.Drawing.Color.Red;
                //            lblMensagem.Text = "Erro ao salvar: " + ex.Message;
                //        }
                //    }
                //    else
                //    {
                //        lblMensagem.ForeColor = System.Drawing.Color.Red;
                //        lblMensagem.Text = "Nenhuma imagem selecionada.";
                //    }
                //}











    }
}

        // Função para carregar os Estados Brasileiro
        private void CarregarDDLEstadosBrasileiros()
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
                    ddlUF.DataSource = reader;
                    ddlUF.DataTextField = "SiglaUf";  // Campo que será mostrado no ComboBox
                    ddlUF.DataValueField = "Uf";  // Campo que será o valor de cada item                    
                    ddlUF.DataBind();  // Realiza o binding dos dados 
                    ddlUF.Items.Insert(0, new ListItem("", "0"));

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
        private void CarregarMunicipiosDoEstado(int ufId)
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                string query = "SELECT Uf, nome_municipio, regiao FROM tbmunicipiosbrasileiros WHERE Uf = @Uf";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Uf", ufId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlMunicipioNasc.DataSource = dt;
                ddlMunicipioNasc.DataTextField = "nome_municipio";
                ddlMunicipioNasc.DataValueField = "Uf";
                ddlMunicipioNasc.DataBind();

                // Adiciona uma opção "Selecione" como primeira opção
                ddlMunicipioNasc.Items.Insert(0, new ListItem("Selecione o item", ""));
            }
        }
        protected void ddlRegiao_SelectedIndexChanged(object sender, EventArgs e)
        {
            int regiaoId = int.Parse(ddlRegiao.SelectedValue);
           // CarregarEstados(regiaoId);
        }
        private void CarregarEstados()
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
                    cbFiliais.DataSource = reader;
                    cbFiliais.DataTextField = "SiglaUf";  // Campo que será mostrado no ComboBox
                    cbFiliais.DataValueField = "Uf";  // Campo que será o valor de cada item                    
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
        protected void ddlUF_SelectedIndexChanged(object sender, EventArgs e)
        {
            int municipioId = int.Parse(ddlUF.SelectedValue);
            CarregarMunicipios(municipioId);
        }

        protected void ddlCNH_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlCNH.SelectedValue))
            {
                CarregarMunicipiosDoEstado(int.Parse(ddlCNH.SelectedValue));
            }

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
        // Função para carregar os municipios Brasileiro
        private void CarregarDDLCidades()
{
    using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
    {
        conn.Open();
        string query = "SELECT Id, Uf, nome_municipio, regiao FROM tbmunicipiosbrasileiros where Uf=" + ddlUF.SelectedValue.ToString() + " ORDER BY nome_municipio";
        SqlCommand cmd = new SqlCommand(query, conn);
        SqlDataReader reader = cmd.ExecuteReader();

        ddlMunicipioNasc.DataSource = reader;
        ddlMunicipioNasc.DataTextField = "nome_municipio";  // Campo a ser exibido
        ddlMunicipioNasc.DataValueField = "Id";  // Valor associado ao item
        ddlMunicipioNasc.DataBind();

        // Adicionar o item padrão
        ddlMunicipioNasc.Items.Insert(0, new ListItem("", "0"));
    }
}
// Evento disparado quando o item do DropDownList é alterado
protected void ddlMunicBrasileiros_SelectedIndexChanged(object sender, EventArgs e)
{
    int idSelecionado = int.Parse(ddlMunicipioNasc.SelectedValue);

    // Preencher os campos com base no valor selecionado
    if (idSelecionado > 0)
    {
        PreencherCamposCidades(idSelecionado);
    }
    else
    {
        LimparCamposCidades();
    }
}
// Função para preencher os campos com os dados do banco
private void PreencherCamposCidades(int id)
{
    using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
    {
        conn.Open();
        string query = "SELECT Id, Uf, nome_municipios, regiao FROM tbmunicipiosbrasileiros WHERE Id = @ID";
        SqlCommand cmd = new SqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@ID", id);
        SqlDataReader reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            // preencher os campos vindos da tabela  txtCodTra.Text = reader["codtra"].ToString();                   
        }
    }
}
// Função para limpar os campos
private void LimparCamposCidades()
{
    ddlUF.Text = string.Empty;
    ddlMunicipioNasc.Text = string.Empty;
}

// Função para carregar os municipios Brasileiro
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
    string query = "SELECT cod_funcao, nm_funcao FROM tb_funcao ORDER BY nm_funcao";

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
private void CarregarDDLAgregados()
{
    using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
    {
        conn.Open();
        string query = "SELECT ID, codtra, fantra FROM tbtransportadoras WHERE fl_exclusao is null AND ativa_inativa = 'ATIVO' ORDER BY fantra";
        SqlCommand cmd = new SqlCommand(query, conn);
        SqlDataReader reader = cmd.ExecuteReader();

        ddlAgregados.DataSource = reader;
        ddlAgregados.DataTextField = "fantra";  // Campo a ser exibido
        ddlAgregados.DataValueField = "codtra";  // Valor associado ao item
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

        }
    }
}

// Função para limpar os campos
private void LimparCampos()
{
    txtCodTra.Text = string.Empty;
}
    }
}