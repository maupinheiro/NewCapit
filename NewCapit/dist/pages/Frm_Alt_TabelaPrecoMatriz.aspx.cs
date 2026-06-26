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
using System.Globalization;
using DocumentFormat.OpenXml.Wordprocessing;
using DAL;
using DocumentFormat.OpenXml.Office.Word;
namespace NewCapit.dist.pages
{
    public partial class Frm_Alt_TabelaPrecoMatriz : System.Web.UI.Page
    {
        SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString());
        string nomeUsuario = string.Empty;
        DateTime dataHoraAtual = DateTime.Now;
        string lotacaomin;
        string radioSim;
        string radioNao;
        string customRadioFrota;
        string customRadioAgregado;
        string id;
        string rota;
        string resLink;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UsuarioLogado"] != null)
                {
                    string nomeUsuario = Session["UsuarioLogado"].ToString();
                    var lblUsuario = nomeUsuario;
                    txtAltCad.Text = lblUsuario.Trim().ToUpper();
                }
                PreencherComboTipoVeiculos();
                PreencherComboMateriais();
                PreencherComboTipoViagens();
                PreencherComboConsignario();
                PreencherComboPagador();
                PreencherTabelaANTT();
                PreencherNumTabelaDeFrete();                
                ViewState["idTabela"] = novaTabelaDeFrete.Text.Trim();
                int idTabela = Convert.ToInt32(ViewState["idTabela"]);                
                CarregarFretes(idTabela);
            }
            
            //DateTime dataHoraAtual = DateTime.Now;
            //txtAltCad.Text = dataHoraAtual.ToString("dd/MM/yyyy HH:mm");
            lbDtAtualizacao.Text = dataHoraAtual.ToString("dd/MM/yyyy HH:mm");
            //txtStatusRota.Text = "ATIVO";
            decimal total =
            Converter(txtSecCat.Text) +
            Converter(txtDespacho.Text) +
            Converter(txtOutros.Text) +
            Converter(txtDespAdm.Text) +
            Converter(txtGRIS.Text) +
            Converter(txtColeta.Text) +
            Converter(txtEntrega.Text) +
            Converter(txtTDE.Text) +
            Converter(txtTDA.Text) +
            Converter(txtFreteReceber.Text);
            
            txtTotalFrete.Text = total.ToString("N2");
        }
        private decimal Converter(string valor)
        {
            decimal resultado;
            decimal.TryParse(valor,
                System.Globalization.NumberStyles.Any,
                new System.Globalization.CultureInfo("pt-BR"),
                out resultado);

            return resultado;
        }
        private void PreencherComboMateriais()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT codigo, descricao FROM tbtipodematerial order by descricao";

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
                    cboTipoMaterial.DataSource = reader;
                    cboTipoMaterial.DataTextField = "descricao";  // Campo que será mostrado no ComboBox
                    cboTipoMaterial.DataValueField = "codigo";  // Campo que será o valor de cada item                    
                    cboTipoMaterial.DataBind();  // Realiza o binding dos dados                   
                    cboTipoMaterial.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Selecione...", "0"));
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
        private void PreencherComboTipoVeiculos()
        {
            // Consulta SQL que retorna os dados desejados
            string query = @"
            SELECT DISTINCT RTRIM(LTRIM(descricao_tng)) AS descricao_tng
            FROM tbtipoveic
            WHERE descricao_tng IS NOT NULL
            ORDER BY descricao_tng";

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
                    cboTipoVeiculo.DataSource = reader;
                    cboTipoVeiculo.DataTextField = "descricao_tng";  // Campo que será mostrado no ComboBox
                    cboTipoVeiculo.DataValueField = "descricao_tng";  // Campo que será o valor de cada item                    
                    cboTipoVeiculo.DataBind();  // Realiza o binding dos dados                   
                    cboTipoVeiculo.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Selecione...", "0"));
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
        private void PreencherComboTipoViagens()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT codigo, descricao FROM tbtiposdeviagem order by descricao";

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
                    cboTipoViagem.DataSource = reader;
                    cboTipoViagem.DataTextField = "descricao";  // Campo que será mostrado no ComboBox
                    cboTipoViagem.DataValueField = "codigo";  // Campo que será o valor de cada item                    
                    cboTipoViagem.DataBind();  // Realiza o binding dos dados                   
                    cboTipoViagem.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Selecione...", "0"));
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
        private void PreencherComboConsignario()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT codcli, razcli, cidcli, estcli, ativo_inativo, fl_exclusao FROM tbclientes order by razcli";

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
                    cboConsignatario.DataSource = reader;
                    cboConsignatario.DataTextField = "razcli";  // Campo que será mostrado no ComboBox
                    cboConsignatario.DataValueField = "codcli";  // Campo que será o valor de cada item                    
                    cboConsignatario.DataBind();  // Realiza o binding dos dados                   
                    //cboConsignatario.Items.Insert(0, new ListItem("Selecione...", "0"));
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
        private void PreencherComboPagador()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT codcli, razcli, cidcli, estcli, ativo_inativo, fl_exclusao FROM tbclientes order by razcli";

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
                    cboPagador.DataSource = reader;
                    cboPagador.DataTextField = "razcli";  // Campo que será mostrado no ComboBox
                    cboPagador.DataValueField = "codcli";  // Campo que será o valor de cada item                    
                    cboPagador.DataBind();  // Realiza o binding dos dados                   
                   // cboPagador.Items.Insert(0, new ListItem("Selecione...", "0"));
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
        private void PreencherNumTabelaDeFrete()
        {
            if (HttpContext.Current.Request.QueryString["id"].ToString() != "")
            {
                id = HttpContext.Current.Request.QueryString["id"].ToString();
            }
            novaTabelaDeFrete.Text = id;
            CarregarDadosFrete(id);
        }
        private void CarregarDadosFrete(string id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = "SELECT * FROM tbtabeladefretes WHERE cod_frete = @cod_frete";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@cod_frete", id);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    // 🔹 Campos simples
                    txtFrete.Text = dr["cod_frete"].ToString();
                    txtDesc_Frete.Text = dr["desc_frete"].ToString();
                    txtCodPagador.Text = dr["cod_pagador"].ToString();
                    txtCodExpedidor.Text = dr["cod_expedidor"].ToString();
                    cboExpedidor.Text = dr["expedidor"].ToString();
                    txtCidExpedidor.Text = dr["cid_expedidor"].ToString();
                    txtUFExpedidor.Text = dr["uf_expedidor"].ToString();
                    txtCodRecebedor.Text = dr["cod_recebedor"].ToString();
                    txtCodRemetente.Text = dr["cod_remetente"].ToString();
                    cboRemetente.Text = dr["remetente"].ToString();
                    cboRecebedor.Text = dr["recebedor"].ToString();
                    txtCidRecebedor.Text = dr["cid_recebedor"].ToString();
                    txtUFRecebedor.Text = dr["uf_recebedor"].ToString();
                    txtRota.Text = dr["rota"].ToString();
                    cboRotas.Text = dr["desc_rota"].ToString();
                    txtDuracao.Text = dr["Tempo"].ToString();
                    txtObservacao.Text = dr["observacao"].ToString();
                    txtUsuCadastro.Text = dr["cadastro_usuario"].ToString();
                    txtStatusRota.Text = dr["situacao"].ToString();
                    txtCodDestinatario.Text = dr["cod_destinatario"].ToString();
                    cboDestinatario.Text = dr["destinatario"].ToString();
                    txtMunicipioDestinatario.Text = dr["cid_destinatario"].ToString();
                    txtUFDestinatario.Text = dr["uf_destinatario"].ToString();
                    txtMunicipioRemetente.Text = dr["cid_remetente"].ToString();
                    txtUFRemetente.Text = dr["uf_remetente"].ToString();
                    txtCadastro.Text = dr["data_cadastro"].ToString();
                    txtCNPJRemetente.Text = dr["cnpj_remetente"].ToString();
                    txtCNPJExpedidor.Text = dr["cnpj_expedidor"].ToString();    
                    txtCNPJDestinatario.Text = dr["cnpj_destinatario"].ToString();
                    txtCNPJRecebedor.Text = dr["cnpj_recebedor"].ToString();
                    txtCNPJConsignatario.Text = dr["cnpj_consignatario"].ToString();
                    txtCNPJPagador.Text = dr["cnpj_pagador"].ToString();

                    // 🔹 Campos numéricos / decimais
                    txtDistancia.Text = Convert.ToDecimal(dr["distancia"]).ToString("N2"); 
                    txtAdicional.Text = Convert.ToDecimal(dr["adicional_sobrenf"]).ToString("N4");
                    txtSecCat.Text = Convert.ToDecimal(dr["sec_cat"]).ToString("N2");
                    txtDespacho.Text = Convert.ToDecimal(dr["despacho"]).ToString("N2"); 
                    txtOutros.Text = Convert.ToDecimal(dr["outros"]).ToString("N2");
                    txtPercentualAluguelCarreta.Text = Convert.ToDecimal(dr["aluguel_carreta"]).ToString("N2");
                    txtDespAdm.Text = Convert.ToDecimal(dr["despesa_adm"]).ToString("N2");  
                    lnkUrl.Text = dr["resolucao_vigente"].ToString();
                    lnkUrl.NavigateUrl = dr["endereco_resolucao"].ToString();
                    txtGRIS.Text = Convert.ToDecimal(dr["gris"]).ToString("N2", new CultureInfo("pt-BR"));
                    txtColeta.Text = Convert.ToDecimal(dr["coleta"]).ToString("N2", new CultureInfo("pt-BR"));
                    txtEntrega.Text = Convert.ToDecimal(dr["entrega"]).ToString("N2", new CultureInfo("pt-BR"));
                    txtTDE.Text = Convert.ToDecimal(dr["tde"]).ToString("N2", new CultureInfo("pt-BR"));
                    txtTDA.Text = Convert.ToDecimal(dr["tda"]).ToString("N2", new CultureInfo("pt-BR"));
                    txtTotalFrete.Text = Convert.ToDecimal(dr["total_frete"]).ToString("N2", new CultureInfo("pt-BR"));
                    txtCodConsignatario.Text = dr["cod_consignatario"].ToString();
                    cboConsignatario.SelectedItem.Text = dr["consignatario"].ToString();
                    txtCidConsignatario.Text = dr["cid_consignatario"].ToString();
                    txtUFConsignatario.Text = dr["uf_consignatario"].ToString();
                    cboPagador.SelectedItem.Text = dr["pagador"].ToString();
                    txtCidPagador.Text = dr["cid_pagador"].ToString();
                    txtUFPagador.Text = dr["uf_pagador"].ToString(); 
                    txtDeslocamento.Text =  dr["deslocamento"].ToString();
                    ddlEmitePedagio.SelectedItem.Text =  dr["emitepedagio"].ToString();   
                    lblDtCadastro.Text = dr["data_cadastro"].ToString();
                    //ddlHoraParada.SelectedItem.Text=
                    rota = dr["rota"].ToString();                    
                    ddlHoraParada.SelectedItem.Text = dr["cobra_hora_parada"].ToString();
                    if (dr["franquia_hora_parada"] != DBNull.Value)
                    {
                        TimeSpan hora = (TimeSpan)dr["franquia_hora_parada"];
                        txtFranquia.Text = hora.ToString(@"hh\:mm\:ss");
                    }                    
                    txtValorFranquia.Text = GetDecimal(dr["valor_hora_parada"]);
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "NaoEncontrado",
                        "<script>alert('⚠️ Frete não encontrado.');</script>");
                }

                dr.Close();
            }
            
        }
        private void SetSelectedValue(DropDownList ddl, string value)
        {
            if (ddl.Items.FindByValue(value) != null)
            {
                ddl.ClearSelection(); // Limpa qualquer seleção anterior
                ddl.SelectedValue = value;
            }
            // Opcional: Se o valor não for encontrado, você pode querer definir um padrão
            // else { ddl.SelectedIndex = 0; } 
        }
        string GetDecimal(object valor)
        {
            return valor == DBNull.Value
                ? "0,00"
                : Convert.ToDecimal(valor).ToString("N2");
        }
        protected void txtCodConsignatario_TextChanged(object sender, EventArgs e)
        {
            if (txtCodConsignatario.Text != "")
            {
                string cod = txtCodConsignatario.Text;
                string sql = "SELECT codcli, razcli, cidcli, estcli, ativo_inativo, fl_exclusao FROM tbclientes where codcli = '" + cod + "' and ativo_inativo = 'ATIVO' and fl_exclusao is null";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                conn.Open();
                da.Fill(dt);
                conn.Close();

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0][5].ToString() == null)
                    {
                        // Acione o toast quando a página for carregada
                        string script = "<script>showToast('Cliente deletado do sistema.');</script>";
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                        txtCodConsignatario.Text = "";
                        txtCodConsignatario.Focus();
                        return;
                    }
                    else if (dt.Rows[0][4].ToString() == "INATIVO")
                    {
                        // Acione o toast quando a página for carregada
                        string script = "<script>showToast('Cliente inativo no sistema.');</script>";
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                        txtCodConsignatario.Text = "";
                        txtCodConsignatario.Focus();
                        return;
                    }
                    else
                    {
                        cboConsignatario.SelectedItem.Text = dt.Rows[0][1].ToString();
                        txtCidConsignatario.Text = dt.Rows[0][2].ToString();
                        txtUFConsignatario.Text = dt.Rows[0][3].ToString();
                        return;
                    }

                }
                else
                {
                    // Acione o toast quando a página for carregada
                    string script = "<script>showToast('Cliente não encontrado no sistema.');</script>";
                    ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                    txtCodConsignatario.Text = "";
                    txtCodConsignatario.Focus();
                    return;
                }
            }

        }
        protected void cboConsignatario_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idSelecionado = int.Parse(cboConsignatario.SelectedValue);

            // Preencher os campos com base no valor selecionado
            if (idSelecionado > 0)
            {
                PreencherCamposConsignatario(idSelecionado);
            }
            else
            {
                LimparCamposConsignatario();
            }
        }
        // Função para preencher os campos com os dados do banco
        private void PreencherCamposConsignatario(int id)
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                conn.Open();
                string query = "SELECT codcli, razcli, cidcli, estcli FROM tbclientes WHERE codcli = @ID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtCodConsignatario.Text = reader["codcli"].ToString();
                    txtCidConsignatario.Text = reader["cidcli"].ToString();
                    txtUFConsignatario.Text = reader["estcli"].ToString();
                }
            }
        }
        // Função para limpar os campos
        private void LimparCamposConsignatario()
        {
            txtCodConsignatario.Text = string.Empty;
            txtCidConsignatario.Text = string.Empty;
            txtUFConsignatario.Text = string.Empty;
        }
        protected void txtCodPagador_TextChanged(object sender, EventArgs e)
        {
            if (txtCodPagador.Text != "")
            {
                string cod = txtCodPagador.Text;
                string sql = "SELECT codcli, razcli, cidcli, estcli, ativo_inativo, fl_exclusao FROM tbclientes where codcli = '" + cod + "' and ativo_inativo = 'ATIVO' and fl_exclusao is null";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                conn.Open();
                da.Fill(dt);
                conn.Close();

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0][5].ToString() == null)
                    {
                        // Acione o toast quando a página for carregada
                        string script = "<script>showToast('Cliente deletado do sistema.');</script>";
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                        txtCodPagador.Text = "";
                        txtCodPagador.Focus();
                        return;
                    }
                    else if (dt.Rows[0][4].ToString() == "INATIVO")
                    {
                        // Acione o toast quando a página for carregada
                        string script = "<script>showToast('Cliente inativo no sistema.');</script>";
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                        txtCodPagador.Text = "";
                        txtCodPagador.Focus();
                        return;
                    }
                    else
                    {
                        cboPagador.SelectedItem.Text = dt.Rows[0][1].ToString();
                        txtCidPagador.Text = dt.Rows[0][2].ToString();
                        txtUFPagador.Text = dt.Rows[0][3].ToString();
                        return;
                    }

                }
                else
                {
                    // Acione o toast quando a página for carregada
                    string script = "<script>showToast('Cliente não encontrado no sistema.');</script>";
                    ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                    txtCodPagador.Text = "";
                    txtCodPagador.Focus();
                    return;
                }
            }

        }
        protected void cboPagador_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idSelecionado = int.Parse(cboPagador.SelectedValue);

            // Preencher os campos com base no valor selecionado
            if (idSelecionado > 0)
            {
                PreencherCamposPagador(idSelecionado);
            }
            else
            {
                LimparCamposPagador();
            }
        }
        // Função para preencher os campos com os dados do banco
        private void PreencherCamposPagador(int id)
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                conn.Open();
                string query = "SELECT codcli, razcli, cidcli, estcli FROM tbclientes WHERE codcli = @ID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtCodPagador.Text = reader["codcli"].ToString();
                    txtCidPagador.Text = reader["cidcli"].ToString();
                    txtUFPagador.Text = reader["estcli"].ToString();
                }
            }
        }
        // Função para limpar os campos
        private void LimparCamposPagador()
        {
            txtCodPagador.Text = string.Empty;
            txtCidPagador.Text = string.Empty;
            txtUFPagador.Text = string.Empty;
        }
        public void CarregaRotas(string rota)
        {
            if (txtRota.Text != "")
            {
                using (SqlConnection conn = new SqlConnection(
     WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
                {
                    string cod = txtRota.Text;
                    string sql = "SELECT * FROM tbrotasdeentregas where rota = '" + cod + "' and situacao = 'ATIVO' and fl_exclusao is null";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id", cod);

                    conn.Open();

                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        txtRota.Text = dr["rota"].ToString();
                        cboRotas.Text = dr["desc_rota"].ToString();
                        txtCodRemetente.Text = dr["cod_remetente"].ToString();
                        cboRemetente.Text = dr["remetente"].ToString();
                        txtMunicipioRemetente.Text = dr["cid_remetente,"].ToString();
                        txtUFRemetente.Text = dr["uf_remetente"].ToString();

                        txtCodExpedidor.Text = dr["codigo_expedidor"].ToString();
                        cboExpedidor.Text = dr["nome_expedidor"].ToString();
                        txtCidExpedidor.Text = dr["cidade_expedidor"].ToString();
                        txtUFExpedidor.Text = dr["uf_expedidor"].ToString();

                        txtCodDestinatario.Text = dr["codigo_destinatario"].ToString();
                        cboDestinatario.Text = dr["nome_destinatario"].ToString();
                        txtMunicipioDestinatario.Text = dr["cidade_destinatario"].ToString();
                        txtUFDestinatario.Text = dr["uf_destinatario"].ToString();
                        txtCodRecebedor.Text = dr["codigo_recebedor"].ToString();
                        cboRecebedor.Text = dr["nome_recebedor"].ToString();

                        txtCidRecebedor.Text = dr["cidade_recebedor"].ToString();
                        txtUFRecebedor.Text = dr["uf_recebedor"].ToString();
                        txtDistancia.Text = dr["distancia"].ToString();
                        txtDuracao.Text = dr["tempo"].ToString();
                        txtDeslocamento.Text = dr["deslocamento"].ToString();
                        ViewState["id_frete"] = id;
                    }
                }

            }
        }
        protected void btnAlterar_Click(object sender, EventArgs e)
        {
            // 🔹 Verifica se campos obrigatórios (que compõem descr_frete) estão preenchidos
            if (string.IsNullOrWhiteSpace(txtCodPagador.Text) ||
                cboPagador.SelectedItem == null ||
                string.IsNullOrWhiteSpace(txtCodExpedidor.Text) ||
                cboExpedidor.Text == null ||
                string.IsNullOrWhiteSpace(txtCidExpedidor.Text) ||
                string.IsNullOrWhiteSpace(txtUFExpedidor.Text) ||
                string.IsNullOrWhiteSpace(txtCodRecebedor.Text) ||
                cboRecebedor.Text == null ||
                string.IsNullOrWhiteSpace(txtCidRecebedor.Text) ||
                string.IsNullOrWhiteSpace(txtUFRecebedor.Text) ||
                cboTipoMaterial.SelectedItem == null ||
                cboTipoVeiculo.SelectedItem == null)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "CamposNulos",
                    "<script>alert('⚠️ Campos obrigatórios para gerar a descrição do frete não podem estar vazios.');</script>");
                return;
            }

            string usuario = Session["UsuarioLogado"] != null
                ? Session["UsuarioLogado"].ToString()
                : "SISTEMA";

            // 🔹 Monta o campo descr_frete
            string[] pagador = cboPagador.SelectedItem.Text.Split(' ');
            string[] expedidor = cboExpedidor.Text.Split(' ');
            string[] recebedor = cboRecebedor.Text.Split(' ');

            string descr_frete = $"{txtCodPagador.Text} - {pagador[0]} - Inicio Prestação: {txtCidExpedidor.Text}/{txtUFExpedidor.Text} - Term. Prestação: {txtCidRecebedor.Text}/{txtUFRecebedor.Text}";

            string connectionString = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = @"
                UPDATE tbtabeladefretes SET                    
                    cod_remetente=@cod_remetente, 
                    remetente=@remetente,
                    cid_remetente=@cid_remetente,
                    uf_remetente=@uf_remetente, 
                    cod_expedidor=@cod_expedidor,
                    expedidor=@expedidor,
                    cid_expedidor=@cid_expedidor, 
                    uf_expedidor=@uf_expedidor,
                    cod_destinatario=@cod_destinatario,
                    destinatario=@destinatario,
                    cid_destinatario=@cid_destinatario,
                    uf_destinatario=@uf_destinatario,
                    cod_recebedor=@cod_recebedor, 
                    recebedor=@recebedor,
                    cid_recebedor=@cid_recebedor, 
                    uf_recebedor=@uf_recebedor,  
                    cod_consignatario=@cod_consignatario,
                    consignatario=@consignatario, 
                    cid_consignatario=@cid_consignatario,
                    uf_consignatario=@uf_consignatario,
                    cod_pagador=@cod_pagador,
                    pagador=@pagador, 
                    cid_pagador=@cid_pagador,
                    uf_pagador=@uf_pagador,
                    distancia=@distancia,
                    Tempo=@Tempo,
                    adicional_sobrenf=@adicional_sobrenf,
                    sec-cat=@sec_cat,
                    despacho=@despacho, 
                    outros=@outros,
                    data_cadastro=@data_cadastro, 
                    situacao=@situacao,
                    deslocamento=@deslocamento,
                    aluguel_carreta=@aluguel_carreta, 
                    cadastro_usuario=@cadastro_usuario,
                    emitepedagio=@emitepedagio,
                    despesa_adm=@despesa_adm, 
                    cobra_hora_parada=@cobra_hora_parada,                     valor_hora_parada=@valor_hora_parada,franquia_hora_parada=@franquia_hora_parada, resolucao_vigente=@resolucao_vigente, endereco_resolucao=@endereco_resolucao, valor_icms=@valor_icms, valor_iss=@valor_iss, valor_pis=@valor_pis, valor_cofins=@valor_cofins, valor_irpj=@valor_irpj, valor_csll=@valor_csll, valor_ibs=@valor_ibs, valor_cbs=@valor_cbs, valor_sestsenat=@valor_sestsenat, valor_inss=@valor_inss, alteracao_usuario=@alteracao_usuario, alteracao_data=GETDATE(),
                      cnpj_remetente=@remetente,
                      cnpj_expedidor=@expedidor,
                      cnpj_destinatario=@destinatario,
                      cnpj_recebedor=@recebedor,
                      cnpj_consignatario=@consignatario,
                      cnpj_pagador=@pagador,
                      gris=@gris, coleta=@coleta, entrega=@entrega, tde=@tde, tda=@tda, total_frete=@total_frete 
                WHERE cod_frete = @cod_frete";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    if (HttpContext.Current.Request.QueryString["id"].ToString() != "")
                    {
                        id = HttpContext.Current.Request.QueryString["id"].ToString();
                    }
                    // Mesmo mapeamento de parâmetros do seu UPDATE
                    cmd.Parameters.Add("@cod_frete", SqlDbType.Int).Value = Convert.ToInt32(id);                    
                    cmd.Parameters.Add("@cod_remetente", SqlDbType.Int).Value = Convert.ToInt32(txtCodRemetente.Text);
                    cmd.Parameters.Add("@remetente", SqlDbType.NVarChar).Value = cboRemetente.Text;
                    cmd.Parameters.Add("@cid_remetente", SqlDbType.NVarChar).Value = txtMunicipioRemetente.Text;
                    cmd.Parameters.Add("@uf_remetente", SqlDbType.NVarChar).Value = txtUFRemetente.Text;
                    cmd.Parameters.Add("@cod_expedidor", SqlDbType.Int).Value = Convert.ToInt32(txtCodExpedidor.Text);
                    cmd.Parameters.Add("@expedidor", SqlDbType.NVarChar).Value = cboExpedidor.Text;
                    cmd.Parameters.Add("@cid_expedidor", SqlDbType.NVarChar).Value = txtCidExpedidor.Text;
                    cmd.Parameters.Add("@uf_expedidor", SqlDbType.NVarChar).Value = txtUFExpedidor.Text;
                    cmd.Parameters.Add("@cod_destinatario", SqlDbType.Int).Value = Convert.ToInt32(txtCodDestinatario.Text);
                    cmd.Parameters.Add("@destinatario", SqlDbType.NVarChar).Value = cboDestinatario.Text;
                    cmd.Parameters.Add("@cid_destinatario", SqlDbType.NVarChar).Value = txtMunicipioDestinatario.Text;
                    cmd.Parameters.Add("@uf_destinatario", SqlDbType.NVarChar).Value = txtUFDestinatario.Text;
                    cmd.Parameters.Add("@cod_recebedor", SqlDbType.Int).Value = Convert.ToInt32(txtCodRecebedor.Text);
                    cmd.Parameters.Add("@recebedor", SqlDbType.NVarChar).Value = cboRecebedor.Text;
                    cmd.Parameters.Add("@cid_recebedor", SqlDbType.NVarChar).Value = txtCidRecebedor.Text;
                    cmd.Parameters.Add("@uf_recebedor", SqlDbType.NVarChar).Value = txtUFRecebedor.Text;
                    cmd.Parameters.Add("@cod_consignatario", SqlDbType.Int).Value = DbParse.Int(txtCodConsignatario.Text);
                    cmd.Parameters.Add("@consignatario", SqlDbType.NVarChar).Value = DbParse.String(cboConsignatario.SelectedItem.Text);
                    cmd.Parameters.Add("@cid_consignatario", SqlDbType.NVarChar).Value = DbParse.String(txtCidConsignatario.Text);
                    cmd.Parameters.Add("@uf_consignatario", SqlDbType.NVarChar).Value = DbParse.String(txtUFConsignatario.Text);
                    cmd.Parameters.Add("@cod_pagador", SqlDbType.Int).Value = Convert.ToInt32(txtCodPagador.Text);
                    cmd.Parameters.Add("@pagador", SqlDbType.NVarChar).Value = cboPagador.SelectedItem.Text;
                    cmd.Parameters.Add("@cid_pagador", SqlDbType.NVarChar).Value = txtCidPagador.Text;
                    cmd.Parameters.Add("@uf_pagador", SqlDbType.NVarChar).Value = txtUFPagador.Text;
                    //cmd.Parameters.Add("@nucleo", SqlDbType.NVarChar).Value = cboFilial.SelectedItem.Text;
                    cmd.Parameters.Add("@Tempo", SqlDbType.NVarChar).Value = txtDuracao.Text;
                    cmd.Parameters.Add("@situacao", SqlDbType.NVarChar).Value = txtStatusRota.Text;
                    cmd.Parameters.Add("@deslocamento", SqlDbType.NVarChar).Value = txtDeslocamento.Text;
                    cmd.Parameters.Add("@alteracao_usuario", SqlDbType.NVarChar).Value = usuario; 
                    cmd.Parameters.Add("@emitepedagio", SqlDbType.NVarChar).Value = ddlEmitePedagio.SelectedItem.Text;
                    cmd.Parameters.Add("@distancia", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtDistancia.Text);
                    cmd.Parameters.Add("@adicional_sobrenf", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtAdicional.Text);
                    cmd.Parameters.Add("@sec_cat", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtSecCat.Text);
                    cmd.Parameters.Add("@despacho", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtDespacho.Text);                   
                    cmd.Parameters.Add("@outros", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtOutros.Text);
                    //cmd.Parameters.Add("@total_frete", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtFreteTNG.Text);
                    cmd.Parameters.Add("@aluguel_carreta", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtPercentualAluguelCarreta.Text);
                    cmd.Parameters.Add("@despesa_adm", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtDespAdm.Text);
                    cmd.Parameters.Add("@cobra_hora_parada", SqlDbType.NVarChar).Value = ddlHoraParada.SelectedValue;
                    cmd.Parameters.Add("@valor_hora_parada", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtValorFranquia.Text);
                    TimeSpan franquiaHora;
                    if (!TimeSpan.TryParseExact(
                            txtFranquia.Text,
                            @"hh\:mm\:ss",
                            CultureInfo.InvariantCulture,
                            out franquiaHora))
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "ErroFranquia",
                            "<script>alert('❌ Use o formato HH:mm:ss');</script>");
                        return;
                    }

                    cmd.Parameters.Add("@franquia_hora_parada", SqlDbType.Time).Value = franquiaHora;
                    cmd.Parameters.Add("@resolucao_vigente", SqlDbType.NVarChar).Value = lnkUrl.Text.ToString().Trim();
                    cmd.Parameters.Add("@endereco_resolucao", SqlDbType.NVarChar).Value = lnkUrl.NavigateUrl;
                    cmd.Parameters.Add("@valor_icms", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtICMS.Text);
                    cmd.Parameters.Add("@valor_iss", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtISS.Text);
                    cmd.Parameters.Add("@valor_pis", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtPIS.Text);
                    cmd.Parameters.Add("@valor_cofins", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtCOFINS.Text);
                    cmd.Parameters.Add("@valor_irpj", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtIRPJ.Text);
                    cmd.Parameters.Add("@valor_csll", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtCSLL.Text);
                    cmd.Parameters.Add("@valor_ibs", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtIBS.Text);
                    cmd.Parameters.Add("@valor_cbs", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtCBS.Text);
                    cmd.Parameters.Add("@valor_sestsenat", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtSestSenat.Text);
                    cmd.Parameters.Add("@valor_inss", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtINSS.Text);
                    cmd.Parameters.Add("@cnpj_remetente", SqlDbType.NVarChar).Value = txtCNPJRemetente.Text.Trim();
                    cmd.Parameters.Add("@cnpj_expedidor", SqlDbType.NVarChar).Value = txtCNPJExpedidor.Text.Trim();
                    cmd.Parameters.Add("@cnpj_destinatario", SqlDbType.NVarChar).Value = txtCNPJDestinatario.Text.Trim();
                    cmd.Parameters.Add("@cnpj_recebedor", SqlDbType.NVarChar).Value = txtCNPJRecebedor.Text.Trim();
                    cmd.Parameters.Add("@cnpj_consignatario", SqlDbType.NVarChar).Value = txtCNPJConsignatario.Text.Trim();
                    cmd.Parameters.Add("@cnpj_pagador", SqlDbType.NVarChar).Value = txtCNPJPagador.Text.Trim();
                    cmd.Parameters.Add("@gris", SqlDbType.Decimal).Value =
                        LimparMascaraMoeda(txtGRIS.Text);
                    cmd.Parameters.Add("@coleta", SqlDbType.Decimal).Value =
                        LimparMascaraMoeda(txtColeta.Text);
                    cmd.Parameters.Add("@entrega", SqlDbType.Decimal).Value =
                        LimparMascaraMoeda(txtEntrega.Text);
                    cmd.Parameters.Add("@tde", SqlDbType.Decimal).Value =
                        LimparMascaraMoeda(txtTDE.Text);
                    cmd.Parameters.Add("@tda", SqlDbType.Decimal).Value =
                        LimparMascaraMoeda(txtTDA.Text);
                    cmd.Parameters.Add("@total_frete", SqlDbType.Decimal).Value =
                        LimparMascaraMoeda(txtTotalFrete.Text);

                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();

                        ClientScript.RegisterStartupScript(this.GetType(), "Sucesso",
                            "<script>alert('✅ Frete atualizado com sucesso!'); window.location.href='/dist/pages/ConsultaFretes.aspx';</script>");
                    }
                    catch (Exception ex)
                    {
                        // ✅ BOA PRÁTICA: Logar o erro para depuração
                        // Logger.LogError(ex); // Exemplo de como você poderia logar o erro
                        ClientScript.RegisterStartupScript(this.GetType(), "Erro",
                            $"<script>alert('❌ Ocorreu um erro ao atualizar o frete: {ex.Message.Replace("'", "\\'")}')</script>");
                    }
                }

                //ClientScript.RegisterStartupScript(this.GetType(), "Sucesso",
                //    "<script>alert('✅ Frete atualizado com sucesso!');</script>");
            }
            Response.Redirect("/dist/pages/ConsultaFretes.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }
        public static class DbParse
        {
            public static object Int(string v)
                => int.TryParse(v, out int i) ? i : (object)DBNull.Value;

            public static object Decimal(string v)
                => decimal.TryParse(v, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal d)
                   ? d : (object)DBNull.Value;

            public static object Date(string v)
                => DateTime.TryParse(v, out DateTime dt) ? dt : (object)DBNull.Value;

            public static object String(string v)
                => string.IsNullOrWhiteSpace(v) ? (object)DBNull.Value : v.Trim();
        }
        private object SafeDateValue(string input)
        {
            DateTime dt;
            if (DateTime.TryParse(input, out dt))
                return dt.ToString("yyyy-MM-dd");
            else
                return DBNull.Value;
        }
        private decimal LimparMascaraMoeda(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
            {
                return 0m;
            }
            // Remove pontos de milhar e substitui vírgula decimal por ponto
            string valorLimpo = valor.Replace(".", "").Replace(",", ".");
            if (decimal.TryParse(valorLimpo, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal resultado))
            {
                return resultado;
            }
            return 0m;
        }       
        protected void ddlTabela_SelectedIndexChanged(object sender, EventArgs e)                       
        {
            // 🔎 Verifica distância
            if (string.IsNullOrWhiteSpace(txtDistancia.Text))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg",
                    "alert('Informe a distância primeiro.');", true);
                ddlTabela.SelectedIndex = 0;
                return;
            }

            // 🔎 Verifica eixos
            if (ddlEixos.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg",
                    "alert('Selecione a quantidade de eixos.');", true);
                ddlTabela.SelectedIndex = 0;
                return;
            }

            // ✅ Converter valores
            decimal distancia;
            if (!decimal.TryParse(txtDistancia.Text, out distancia))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg",
                    "alert('Distância inválida.');", true);
                return;
            }

            int eixos = Convert.ToInt32(ddlEixos.SelectedValue);
            decimal valorKm = 0;
            decimal ccd = 0;

            string coluna = "_" + eixos + "Eixos";
            string cargaDescarga = "Valor" + eixos + "Eixos";

            using (SqlConnection conn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                string sql = $@"
            SELECT {coluna}, {cargaDescarga}, resolucao, link
            FROM tbresolucoesantt
            WHERE RTRIM(vigente) = 'SIM'
            AND Tabela = @tabela
            AND TipoCarga = @tipoCarga";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@tabela", ddlTabela.SelectedValue);
                cmd.Parameters.AddWithValue("@tipoCarga", ddlTipoCargaANTT.Text);
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    if (dr[coluna] != DBNull.Value)
                        valorKm = Convert.ToDecimal(dr[coluna]);

                    if (dr[cargaDescarga] != DBNull.Value)
                        ccd = Convert.ToDecimal(dr[cargaDescarga]);

                    if (dr["resolucao"] != DBNull.Value)
                        lnkUrl.Text = dr["resolucao"].ToString();
                        lnkUrl.NavigateUrl = dr["link"].ToString().Trim();
                        string resLink = dr["link"].ToString().Trim();
                }
            }

            // ✅ AGORA SIM a variável existe
            decimal frete = (distancia * valorKm) + ccd;

            txtFreteMinimo.Text = frete.ToString("N2");            
        }

        protected void MostrarMsg(string mensagem, string tipo = "warning")
        {
            divMsg.Attributes["class"] = "alert alert-" + tipo + " alert-dismissible fade show mt-3";
            lblMsg.InnerText = mensagem;
            divMsg.Style["display"] = "block";

            string script = @"setTimeout(function() {
                        var div = document.getElementById('divMsg');
                        if (div) div.style.display = 'none';
                      }, 5000);";

            ScriptManager.RegisterStartupScript(this, GetType(), "EscondeMsg", script, true);
        }
        //protected void btnLancarTabela_Click(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrEmpty(ddlFrete.SelectedValue))
        //    {
        //        MostrarMsg("Escolha o Frete: Frota/Agregado/Terceiro.", "warning");
        //        ddlFrete.Focus();
        //        return;
        //    }

        //    if (ddlTipoFrete.SelectedIndex == 0)
        //    {
        //        MostrarMsg("Escolha o Tipo de frete por Tonelada/Quilo ou FTL.", "warning");
        //        ddlTipoFrete.Focus();
        //        return;
        //    }
        //    if (string.IsNullOrEmpty(cboTipoViagem.SelectedValue))
        //    {
        //        MostrarMsg("Escolha o tipo de viagem.", "warning");
        //        cboTipoViagem.Focus();
        //        return;
        //    }
        //    if (string.IsNullOrEmpty(cboTipoVeiculo.SelectedValue))
        //    {
        //        MostrarMsg("Escolha o tipo de veículo.", "warning");
        //        cboTipoViagem.Focus();
        //        return;
        //    }

        //    if (string.IsNullOrEmpty(ddlEixos.SelectedItem.Text))
        //    {
        //        MostrarMsg("Escolha a quantidade de eixos.", "warning");
        //        ddlEixos.Focus();
        //        return;
        //    }

        //    if (string.IsNullOrEmpty(ddlTabela.SelectedValue))
        //    {
        //        MostrarMsg("Escolha a tabela ANTT.", "warning");
        //        ddlTabela.Focus();
        //        return;
        //    }

        //    if (string.IsNullOrEmpty(ddlTipoCargaANTT.SelectedItem.Text))
        //    {
        //        MostrarMsg("Escolha o tipo de carga ANTT.", "warning");
        //        ddlTipoCargaANTT.Focus();
        //        return;
        //    }

        //    if (string.IsNullOrEmpty(cboTipoMaterial.SelectedValue))
        //    {
        //        MostrarMsg("Escolha o tipo de material.", "warning");
        //        cboTipoMaterial.Focus();
        //        return;
        //    }            

        //    // CONVERSÕES SEGURAS            
        //    DateTime vigenciaInicial;
        //    DateTime vigenciaFinal;

        //    CultureInfo cultura = new CultureInfo("pt-BR");
        //    decimal freteANTT;
        //    decimal freteReceber;
        //    decimal fretePagar;
        //    decimal freteMargem;
        //    decimal freteTotal;


        //    if (string.IsNullOrWhiteSpace(txtVigenciaInicial.Text) ||
        //        string.IsNullOrWhiteSpace(txtVigenciaFinal.Text))
        //    {
        //        MostrarMsg("As datas de vigência não podem estar vazias. Verifique.", "danger");
        //        return;
        //    }

        //    // tenta converter inicial
        //    if (!DateTime.TryParse(txtVigenciaInicial.Text, new CultureInfo("pt-BR"), DateTimeStyles.None, out vigenciaInicial))
        //    {
        //        MostrarMsg("Data inicial vazia ou inválida.", "danger");
        //        txtVigenciaInicial.Focus();
        //        return;
        //    }

        //    //tenta converter final
        //    if (!DateTime.TryParse(txtVigenciaFinal.Text, new CultureInfo("pt-BR"), DateTimeStyles.None, out vigenciaFinal))
        //    {
        //        MostrarMsg("Data final vazia ou inválida.", "danger");
        //        txtVigenciaFinal.Focus();
        //        return;
        //    }

        //    // valida regra de negócio
        //    if (vigenciaInicial > vigenciaFinal)
        //    {
        //        MostrarMsg("A vigência inicial não pode ser maior que a vigência final.", "warning");
        //        txtVigenciaFinal.Focus();
        //        return;
        //    }

        //    // converte inicial
        //    if (!DateTime.TryParse(txtVigenciaInicial.Text, new CultureInfo("pt-BR"), DateTimeStyles.None, out vigenciaInicial))
        //    {
        //        MostrarMsg("Data inicial inválida.", "danger");
        //        txtVigenciaInicial.Focus();
        //        return;
        //    }

        //    // converte final
        //    if (!DateTime.TryParse(txtVigenciaFinal.Text, new CultureInfo("pt-BR"), DateTimeStyles.None, out vigenciaFinal))
        //    {
        //        MostrarMsg("Data final inválida.", "danger");
        //        txtVigenciaFinal.Focus();
        //        return;
        //    }

        //    // regra de negócio
        //    if (vigenciaFinal < vigenciaInicial)
        //    {
        //        MostrarMsg("A data final não pode ser menor que a data inicial.", "warning");
        //        txtVigenciaFinal.Focus();
        //        return;
        //    }
        //    // valida vazio
        //    if (string.IsNullOrWhiteSpace(txtFreteMinimo.Text) ||
        //        string.IsNullOrWhiteSpace(txtFreteReceber.Text) ||
        //        string.IsNullOrWhiteSpace(txtFretePagar.Text) ||
        //        string.IsNullOrWhiteSpace(txtMargem.Text))
        //    {
        //        MostrarMsg("Todos os campos de frete devem ser preenchidos.", "warning");
        //        return;
        //    }

        //    // conversão segura
        //    if (!decimal.TryParse(txtFreteMinimo.Text, NumberStyles.Any, cultura, out freteANTT))
        //    {
        //        MostrarMsg("Frete ANTT inválido.", "danger");
        //        txtFreteMinimo.Focus();
        //        return;
        //    }


        //    if (!decimal.TryParse(txtFreteReceber.Text, NumberStyles.Any, cultura, out freteReceber))
        //    {
        //        MostrarMsg("Frete a receber inválido.", "danger");
        //        txtFreteReceber.Focus();
        //        return;
        //    }


        //    if (!decimal.TryParse(txtFretePagar.Text, NumberStyles.Any, cultura, out fretePagar))
        //    {
        //        MostrarMsg("Frete a pagar inválido.", "danger");
        //        txtFretePagar.Focus();
        //        return;
        //    }

        //    if (!decimal.TryParse(txtTotalFrete.Text, NumberStyles.Any, cultura, out freteTotal))
        //    {
        //        MostrarMsg("Frete a pagar inválido.", "danger");
        //        txtTotalFrete.Focus();
        //        return;
        //    }

        //    if (!decimal.TryParse(txtMargem.Text, NumberStyles.Any, cultura, out freteMargem))
        //    {
        //        MostrarMsg("Margem inválida.", "danger");
        //        txtMargem.Focus();
        //        return;
        //    }

        //    string usuario = Session["UsuarioLogado"] != null
        //        ? Session["UsuarioLogado"].ToString()
        //        : "SISTEMA";

        //    int idRota;
        //    if (!int.TryParse(txtRota.Text, out idRota))
        //    {
        //        MostrarMsg("ID da rota inválido!", "danger");
        //        return;
        //    }

        //    int idTabela;
        //    if (!int.TryParse(novaTabelaDeFrete.Text, out idTabela))
        //    {
        //        MostrarMsg("ID da tabela inválido!", "danger");
        //        return;
        //    }

        //    //int pesoLotacao;
        //    //if (!int.TryParse(txtPesoLotacao.Text, out pesoLotacao))
        //    //{
        //    //    MostrarMsg("Verifique o peso digistado!", "warning");
        //    //    txtPesoLotacao.Focus();
        //    //    return;
        //    //}

        //    if (!int.TryParse(txtPesoLotacao.Text, out int pesoLotacao))
        //    {
        //        MostrarMsg("Peso de lotação inválido.", "danger");
        //        txtPesoLotacao.Focus();
        //        return;
        //    }


        //    // Frete Receber não pode ser menor que o mínimo ANTT
        //    if (freteReceber < freteANTT)
        //    {
        //        MostrarMsg("O frete a Receber não pode ser menor que o Frete Mínimo ANTT!", "warning");
        //        txtFreteReceber.Focus();
        //        return;
        //    }

        //    // Frete Pagar não pode ser menor que o mínimo ANTT
        //    if (fretePagar < freteANTT && ddlFrete.SelectedValue == "Terceiro")
        //    {
        //        MostrarMsg("O Frete a Pagar não pode ser menor que o Frete Minimo!", "warning");
        //        txtFretePagar.Focus();
        //        return;
        //    }

        //    // Frete Pagar não pode ser maior que o Frete a Receber
        //    if (fretePagar > freteReceber)
        //    {
        //        MostrarMsg("Frete a Pagar não pode ser maior que o Frete a Receber!", "warning");
        //        txtFretePagar.Focus();
        //        return;
        //    }
        //    using (SqlConnection conn = new SqlConnection(
        //        WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
        //    {
        //        conn.Open();
        //        string sql = @"
        //        IF EXISTS (
        //            SELECT 1
        //            FROM tbfretes
        //            WHERE cod_frete = @cod_frete
        //              AND frete = @frete
        //              AND tipo_veiculo = @tipo_veiculo
        //              AND fl_exclusao IS NULL
        //        )
        //        BEGIN
        //        UPDATE tbfretes
        //        SET
        //            medida = @medida,
        //            tipo_viagem = @tipo_viagem,
        //            eixos = @eixos,
        //            tabela_antt = @tabela_antt,
        //            tipo_carga = @tipo_carga,
        //            material = @material,
        //            detalhe_material = @detalhe_material,                    
        //            lotacao_peso = @lotacao_peso,
        //            vigencia_inicial = @vigencia_inicial,
        //            vigencia_final = @vigencia_final,
        //            frete_antt = @frete_antt,
        //            frete_receber = @frete_receber,
        //            frete_pagar = @frete_pagar,
        //            margem = @margem,
        //            mensagem = @mensagem,
        //            status = @status,
        //            gris=@gris,
        //            coleta=@coleta,
        //            entrega=@entrega,
        //            tde=@tde,
        //            tda=@tda,
        //            total_frete=@totalfrete,
        //            despesa_adm=@despesa_adm,
        //            sec_cat=@sec_cat,
        //            despacho=@despacho,
        //            outros=@outros,
        //            responsavel = @responsavel,
        //            data_alteracao = GETDATE()
        //        WHERE cod_frete = @cod_frete
        //          AND frete = @frete
        //          AND tipo_veiculo = @tipo_veiculo
        //          AND fl_exclusao IS NULL
        //        END
        //        ELSE
        //        BEGIN
        //            INSERT INTO tbfretes
        //            (
        //                cod_frete,
        //                id_rota,
        //                frete,
        //                medida,
        //                tipo_viagem,
        //                tipo_veiculo,
        //                eixos,
        //                tabela_antt,
        //                tipo_carga,
        //                material,
        //                detalhe_material,
        //                lotacao_peso,
        //                vigencia_inicial,
        //                vigencia_final,
        //                frete_antt,
        //                frete_receber,
        //                frete_pagar,
        //                margem,
        //                mensagem,
        //                gris,
        //                coleta,
        //                entrega,
        //                tde,
        //                tda,
        //                total_frete,
        //                despesa_adm,
        //                sec_cat,
        //                despacho,
        //                outros,
        //                status,
        //                responsavel,
        //                data_alteracao
        //            )
        //            VALUES
        //            (
        //                @cod_frete,
        //                @id_rota,
        //                @frete,
        //                @medida,
        //                @tipo_viagem,
        //                @tipo_veiculo,
        //                @eixos,
        //                @tabela_antt,
        //                @tipo_carga,
        //                @material,
        //                @detalhe_material,                        
        //                @lotacao_peso,
        //                @vigencia_inicial,
        //                @vigencia_final,
        //                @frete_antt,
        //                @frete_receber,
        //                @frete_pagar,
        //                @margem,
        //                @mensagem,
        //                @status,
        //                @gris,
        //                @coleta,
        //                @entrega,
        //                @tde,
        //                @tda,
        //                @totalfrete,
        //                @despesa_adm,
        //                @sec_cat,
        //                @despacho,
        //                @outros,
        //                @responsavel,
        //                GETDATE()
        //            )
        //        END";

        //        SqlCommand cmd2 = new SqlCommand(sql, conn);

        //        // PARÂMETROS
        //        cmd2.Parameters.AddWithValue("@cod_frete", idTabela);
        //        cmd2.Parameters.AddWithValue("@id_rota", idRota);
        //        cmd2.Parameters.AddWithValue("@frete", ddlFrete.SelectedValue);
        //        cmd2.Parameters.AddWithValue("@medida", ddlTipoFrete.SelectedValue);
        //        cmd2.Parameters.AddWithValue("@tipo_viagem", cboTipoViagem.SelectedItem.Text.Trim());
        //        cmd2.Parameters.AddWithValue("@tipo_veiculo", cboTipoVeiculo.SelectedItem.Text.Trim());
        //        cmd2.Parameters.AddWithValue("@eixos", ddlEixos.SelectedValue);
        //        cmd2.Parameters.AddWithValue("@tabela_antt", ddlTabela.SelectedItem.Text.Trim());
        //        cmd2.Parameters.AddWithValue("@tipo_carga", ddlTipoCargaANTT.SelectedItem.Text.Trim());
        //        cmd2.Parameters.AddWithValue("@material", cboTipoMaterial.SelectedItem.Text.Trim());
        //        cmd2.Parameters.AddWithValue("@detalhe_material", txtDetalheMaterial.Text.Trim().ToUpper()); 
        //        cmd2.Parameters.AddWithValue("@lotacao_peso", pesoLotacao);
        //        cmd2.Parameters.AddWithValue("@vigencia_inicial", vigenciaInicial);
        //        cmd2.Parameters.AddWithValue("@vigencia_final", vigenciaFinal);
        //        cmd2.Parameters.Add("@frete_antt", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtFreteMinimo.Text);
        //        cmd2.Parameters.Add("@frete_receber", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtFreteReceber.Text);
        //        cmd2.Parameters.Add("@frete_pagar", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtFretePagar.Text);
        //        cmd2.Parameters.Add("@margem", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtMargem.Text);
        //        cmd2.Parameters.Add("@gris", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtGRIS.Text);
        //        cmd2.Parameters.Add("@coleta", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtColeta.Text);
        //        cmd2.Parameters.Add("@entrega", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtEntrega.Text);
        //        cmd2.Parameters.Add("@tde", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtTDE.Text);
        //        cmd2.Parameters.Add("@tda", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtTDA.Text);
        //        cmd2.Parameters.Add("@totalfrete", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtTotalFrete.Text);
        //        cmd2.Parameters.Add("@despesa_adm", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtDespAdm.Text);
        //        cmd2.Parameters.Add("@sec_cat", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtSecCat.Text);
        //        cmd2.Parameters.Add("@despacho", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtDespacho.Text);
        //        cmd2.Parameters.Add("@outros", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtOutros.Text);
        //        cmd2.Parameters.AddWithValue("@status", "ATIVO");
        //        cmd2.Parameters.AddWithValue("@mensagem", txtObservacao.Text.Trim().ToUpper());
        //        cmd2.Parameters.AddWithValue("@responsavel", usuario);

        //        cmd2.ExecuteNonQuery();

        //    }
        //    CarregarFretes(idTabela);
        //    MostrarMsg("Frete atualizado com sucesso!", "success");
        //    return;
        //}

        protected void btnLancarTabela_Click(object sender, EventArgs e)
        {
            if (ddlFrete.SelectedIndex == 0)
            {
                MostrarMsg("Escolha o Frete: Frota/Agregado/Terceiro.", "warning");
                ddlFrete.Focus();
                return;
            }

            if (ddlTipoFrete.SelectedIndex == 0)
            {
                MostrarMsg("Escolha o Tipo de frete por Tonelada/Quilo ou FTL.", "warning");
                ddlTipoFrete.Focus();
                return;
            }

            if (cboTipoViagem.SelectedIndex == 0)
            {
                MostrarMsg("Escolha o tipo de viagem.", "warning");
                cboTipoViagem.Focus();
                return;
            }

            if (cboTipoVeiculo.SelectedIndex == 0)
            {
                MostrarMsg("Escolha o tipo de veículo.", "warning");
                cboTipoVeiculo.Focus();
                return;
            }

            if (ddlEixos.SelectedIndex == 0)
            {
                MostrarMsg("Escolha a quantidade de eixos.", "warning");
                ddlEixos.Focus();
                return;
            }

            if (ddlTabela.SelectedIndex == 0)
            {
                MostrarMsg("Escolha a tabela ANTT.", "warning");
                ddlTabela.Focus();
                return;
            }

            if (ddlTipoCargaANTT.SelectedIndex == 0)
            {
                MostrarMsg("Escolha o tipo de carga ANTT.", "warning");
                ddlTipoCargaANTT.Focus();
                return;
            }

            if (cboTipoMaterial.SelectedIndex == 0)
            {
                MostrarMsg("Escolha o tipo de material.", "warning");
                cboTipoMaterial.Focus();
                return;
            }


            // CONVERSÕES SEGURAS            
            DateTime vigenciaInicial;
            DateTime vigenciaFinal;

            CultureInfo cultura = new CultureInfo("pt-BR");
            decimal freteANTT;
            decimal freteReceber;
            decimal fretePagar;
            decimal freteMargem;

            if (string.IsNullOrWhiteSpace(txtVigenciaInicial.Text) ||
                string.IsNullOrWhiteSpace(txtVigenciaFinal.Text))
            {
                MostrarMsg("As datas de vigência não podem estar vazias. Verifique.", "danger");
                return;
            }

            // tenta converter inicial
            if (!DateTime.TryParse(txtVigenciaInicial.Text, new CultureInfo("pt-BR"), DateTimeStyles.None, out vigenciaInicial))
            {
                MostrarMsg("Data inicial vazia ou inválida.", "danger");
                txtVigenciaInicial.Focus();
                return;
            }

            //tenta converter final
            if (!DateTime.TryParse(txtVigenciaFinal.Text, new CultureInfo("pt-BR"), DateTimeStyles.None, out vigenciaFinal))
            {
                MostrarMsg("Data final vazoa ou inválida.", "danger");
                txtVigenciaFinal.Focus();
                return;
            }

            // valida regra de negócio
            if (vigenciaInicial > vigenciaFinal)
            {
                MostrarMsg("A vigência inicial não pode ser maior que a vigência final.", "warning");
                txtVigenciaFinal.Focus();
                return;
            }

            // converte inicial
            if (!DateTime.TryParse(txtVigenciaInicial.Text, new CultureInfo("pt-BR"), DateTimeStyles.None, out vigenciaInicial))
            {
                MostrarMsg("Data inicial inválida.", "danger");
                txtVigenciaInicial.Focus();
                return;
            }

            // converte final
            if (!DateTime.TryParse(txtVigenciaFinal.Text, new CultureInfo("pt-BR"), DateTimeStyles.None, out vigenciaFinal))
            {
                MostrarMsg("Data final inválida.", "danger");
                txtVigenciaFinal.Focus();
                return;
            }

            // regra de negócio
            if (vigenciaFinal < vigenciaInicial)
            {
                MostrarMsg("A data final não pode ser menor que a data inicial.", "warning");
                txtVigenciaFinal.Focus();
                return;
            }
            // valida vazio
            if (string.IsNullOrWhiteSpace(txtFreteMinimo.Text) ||
                string.IsNullOrWhiteSpace(txtFreteReceber.Text) ||
                string.IsNullOrWhiteSpace(txtFretePagar.Text) ||
                string.IsNullOrWhiteSpace(txtMargem.Text))
            {
                MostrarMsg("Todos os campos de frete devem ser preenchidos.", "warning");
                return;
            }

            // conversão segura
            if (!decimal.TryParse(txtFreteMinimo.Text, NumberStyles.Any, cultura, out freteANTT))
            {
                MostrarMsg("Frete ANTT inválido.", "danger");
                txtFreteMinimo.Focus();
                return;
            }


            if (!decimal.TryParse(txtFreteReceber.Text, NumberStyles.Any, cultura, out freteReceber))
            {
                MostrarMsg("Frete a receber inválido.", "danger");
                txtFreteReceber.Focus();
                return;
            }


            if (!decimal.TryParse(txtFretePagar.Text, NumberStyles.Any, cultura, out fretePagar))
            {
                MostrarMsg("Frete a pagar inválido.", "danger");
                txtFretePagar.Focus();
                return;
            }

            if (!decimal.TryParse(txtMargem.Text, NumberStyles.Any, cultura, out freteMargem))
            {
                MostrarMsg("Margem inválida.", "danger");
                txtMargem.Focus();
                return;
            }


            string usuario = Session["UsuarioLogado"] != null
                ? Session["UsuarioLogado"].ToString()
                : "SISTEMA";

            int idRota;
            if (!int.TryParse(txtRota.Text, out idRota))
            {
                MostrarMsg("ID da rota inválido!", "danger");
                return;
            }

            int idTabela;
            if (!int.TryParse(novaTabelaDeFrete.Text, out idTabela))
            {
                MostrarMsg("ID da tabela inválido!", "danger");
                return;
            }

            int pesoLotacao;
            if (!int.TryParse(txtPesoLotacao.Text, out pesoLotacao))
            {
                MostrarMsg("Verifique o peso digistado!", "warning");
                txtPesoLotacao.Focus();
                return;
            }

            // Frete Receber não pode ser menor que o mínimo ANTT
            if (freteReceber < freteANTT)
            {
                MostrarMsg("O frete a Receber não pode ser menor que o Frete Mínimo ANTT!", "warning");
                txtFreteReceber.Focus();
                return;
            }

            // Frete Pagar não pode ser menor que o mínimo ANTT
            if (fretePagar < freteANTT && ddlFrete.SelectedValue == "Terceiro")
            {
                MostrarMsg("O Frete a Pagar não pode ser menor que o Frete Minimo!", "warning");
                txtFretePagar.Focus();
                return;
            }

            // Frete Pagar não pode ser maior que o Frete a Receber
            if (fretePagar > freteReceber)
            {
                MostrarMsg("Frete a Pagar não pode ser maior que o Frete a Receber!", "warning");
                txtFretePagar.Focus();
                return;
            }

            // 🔹 Monta o campo descr_frete
            string[] pagador = cboPagador.SelectedItem.Text.Split(' ');
            string[] remetente = cboRemetente.Text.Split(' ');
            string[] expedidor = cboExpedidor.Text.Split(' ');
            string[] destinatario = cboDestinatario.Text.Split(' ');
            string[] recebedor = cboRecebedor.Text.Split(' ');

            string descr_frete = $"{txtCodPagador.Text} - {pagador[0]} - Inicio Prestação: {txtCidExpedidor.Text}/{txtUFExpedidor.Text} - Term. Prestação: {txtCidRecebedor.Text}/{txtUFRecebedor.Text}";
            string sqlVerifica = @"
            SELECT TOP 1 cod_frete
            FROM tbtabeladefretes
            WHERE desc_frete = @desc_frete";

            using (SqlCommand cmd = new SqlCommand(sqlVerifica, conn))
            {
                conn.Close();
                // Inserir novo registro
                using (SqlConnection conn = new SqlConnection(
           WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
                {
                    conn.Open();
                    string sql = @"
                IF EXISTS (
                    SELECT 1
                    FROM tbfretes
                    WHERE cod_frete = @cod_frete
                      AND frete = @frete
                      AND tipo_veiculo = @tipo_veiculo
                      AND fl_exclusao IS NULL
                )
                BEGIN
                UPDATE tbfretes
                SET
                    medida = @medida,
                    tipo_viagem = @tipo_viagem,
                    eixos = @eixos,
                    tabela_antt = @tabela_antt,
                    tipo_carga = @tipo_carga,
                    material = @material,
                    detalhe_material = @detalhe_material,                   
                    lotacao_peso = @lotacao_peso,
                    vigencia_inicial = @vigencia_inicial,
                    vigencia_final = @vigencia_final,
                    frete_antt = @frete_antt,
                    frete_receber = @frete_receber,
                    frete_pagar = @frete_pagar,
                    margem = @margem,
                    mensagem = @mensagem,
                    status = @status,
                    gris=@gris,
                    coleta=@coleta,
                    entrega=@entrega,
                    tde=@tde,
                    tda=@tda,
                    total_frete=@totalfrete,
                    despesa_adm=@despesa_adm,
                    sec_cat=@sec_cat,
                    despacho=@despacho,
                    outros=@outros,
                    responsavel = @responsavel,
                    data_alteracao = GETDATE()
                WHERE cod_frete = @cod_frete
                  AND frete = @frete
                  AND tipo_veiculo = @tipo_veiculo
                  AND fl_exclusao IS NULL
                END
                ELSE
                BEGIN
                    INSERT INTO tbfretes
                    (
                        cod_frete,
                        id_rota,
                        frete,
                        medida,
                        tipo_viagem,
                        tipo_veiculo,
                        eixos,
                        tabela_antt,
                        tipo_carga,
                        material,
                        detalhe_material,                        
                        lotacao_peso,
                        vigencia_inicial,
                        vigencia_final,
                        frete_antt,
                        frete_receber,
                        frete_pagar,
                        margem,
                        mensagem,
                        status,
                        gris,
                        coleta,
                        entrega,
                        tde,
                        tda,
                        total_frete,
                        responsavel,
                        despesa_adm,
                        sec_cat,
                        despacho,
                        outros,
                        data_alteracao
                    )
                    VALUES
                    (
                        @cod_frete,
                        @id_rota,
                        @frete,
                        @medida,
                        @tipo_viagem,
                        @tipo_veiculo,
                        @eixos,
                        @tabela_antt,
                        @tipo_carga,
                        @material,
                        @detalhe_material,                       
                        @lotacao_peso,
                        @vigencia_inicial,
                        @vigencia_final,
                        @frete_antt,
                        @frete_receber,
                        @frete_pagar,
                        @margem,
                        @mensagem,
                        @status,
                        @gris,
                        @coleta,
                        @entrega,
                        @tde,
                        @tda,
                        @totalfrete,
                        @responsavel,
                        @despesa_adm,
                        @sec_cat,
                        @despacho,
                        @outros,
                        GETDATE()
                    )
                END";

                    SqlCommand cmd2 = new SqlCommand(sql, conn);

                    // PARÂMETROS
                    cmd2.Parameters.AddWithValue("@cod_frete", idTabela);
                    cmd2.Parameters.AddWithValue("@id_rota", idRota);
                    cmd2.Parameters.AddWithValue("@frete", ddlFrete.SelectedValue);
                    cmd2.Parameters.AddWithValue("@medida", ddlTipoFrete.SelectedValue);
                    cmd2.Parameters.AddWithValue("@tipo_viagem", cboTipoViagem.SelectedItem.Text.Trim());
                    cmd2.Parameters.AddWithValue("@tipo_veiculo", cboTipoVeiculo.SelectedItem.Text.Trim());
                    cmd2.Parameters.AddWithValue("@eixos", ddlEixos.SelectedValue);
                    cmd2.Parameters.AddWithValue("@tabela_antt", ddlTabela.SelectedItem.Text.Trim());
                    cmd2.Parameters.AddWithValue("@tipo_carga", ddlTipoCargaANTT.SelectedItem.Text.Trim());
                    cmd2.Parameters.AddWithValue("@material", cboTipoMaterial.SelectedItem.Text.Trim());
                    cmd2.Parameters.AddWithValue("@detalhe_material", txtDetalheMaterial.Text.Trim().ToUpper());
                    cmd2.Parameters.AddWithValue("@lotacao_peso", pesoLotacao);

                    cmd2.Parameters.AddWithValue("@vigencia_inicial", vigenciaInicial);
                    cmd2.Parameters.AddWithValue("@vigencia_final", vigenciaFinal);

                    cmd2.Parameters.Add("@frete_antt", SqlDbType.Decimal).Value =
                        LimparMascaraMoeda(txtFreteMinimo.Text);

                    cmd2.Parameters.Add("@frete_receber", SqlDbType.Decimal).Value =
                        LimparMascaraMoeda(txtFreteReceber.Text);

                    cmd2.Parameters.Add("@frete_pagar", SqlDbType.Decimal).Value =
                        LimparMascaraMoeda(txtFretePagar.Text);

                    cmd2.Parameters.Add("@margem", SqlDbType.Decimal).Value =
                        LimparMascaraMoeda(txtMargem.Text);

                    cmd2.Parameters.Add("@gris", SqlDbType.Decimal).Value =
                        LimparMascaraMoeda(txtGRIS.Text);
                    cmd2.Parameters.Add("@coleta", SqlDbType.Decimal).Value =
                        LimparMascaraMoeda(txtColeta.Text);
                    cmd2.Parameters.Add("@entrega", SqlDbType.Decimal).Value =
                        LimparMascaraMoeda(txtEntrega.Text);
                    cmd2.Parameters.Add("@tde", SqlDbType.Decimal).Value =
                        LimparMascaraMoeda(txtTDE.Text);
                    cmd2.Parameters.Add("@tda", SqlDbType.Decimal).Value =
                        LimparMascaraMoeda(txtTDA.Text);
                    cmd2.Parameters.Add("@totalfrete", SqlDbType.Decimal).Value =
                        LimparMascaraMoeda(txtTotalFrete.Text);
                    cmd2.Parameters.Add("@despesa_adm", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtDespAdm.Text);
                    cmd2.Parameters.Add("@sec_cat", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtSecCat.Text);
                    cmd2.Parameters.Add("@despacho", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtDespacho.Text);
                    cmd2.Parameters.Add("@outros", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtOutros.Text);
                    cmd2.Parameters.AddWithValue("@status", "ATIVO");
                    cmd2.Parameters.AddWithValue("@mensagem", txtObservacao.Text.Trim().ToUpper());
                    cmd2.Parameters.AddWithValue("@responsavel", usuario);

                    cmd2.ExecuteNonQuery();

                }
                CarregarFretes(idTabela);
                MostrarMsg("Frete atualizado com sucesso!", "success");
                return;
            }
        }


        private void CarregarFretes(int idTabela)
        {
            using (SqlConnection conn = new SqlConnection(
                WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                string sql = @"
                SELECT 
                    id_frete,
                    cod_frete,
                    frete,
                    medida,
                    tipo_viagem,
                    tipo_veiculo,
                    material,
                    detalhe_material,
                    tabela_antt,
                    frete_antt,
                    frete_receber,
                    frete_pagar,
                    margem, 
                    gris,
                    coleta, 
                    entrega,
                    tde,
                    tda,
                    total_frete,
                    vigencia_inicial,
                    vigencia_final,
                    status
                FROM tbfretes
                WHERE cod_frete = @cod_frete
                  AND fl_exclusao IS NULL
                ORDER BY id_frete ASC";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@cod_frete", idTabela);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvFretes.DataSource = dt;
                gvFretes.DataBind();
            }
        }
        protected void gvFretes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Status")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                int id_frete = Convert.ToInt32(gvFretes.DataKeys[index].Value);

                using (SqlConnection conn = new SqlConnection(
                    WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
                {
                    conn.Open();

                    string sql = @"
                    UPDATE tbfretes
                    SET status = CASE 
                                    WHEN status = 'ATIVO' THEN 'INATIVO'
                                    ELSE 'ATIVO'
                                 END
                    WHERE id_frete = @id_frete";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id_frete", id_frete);
                    cmd.ExecuteNonQuery();
                }
                ViewState["idTabela"] = Convert.ToInt32(novaTabelaDeFrete.Text);
                int idTabela = Convert.ToInt32(ViewState["idTabela"]);
                CarregarFretes(idTabela);
            }
            if (e.CommandName == "Editar")
            {
                CarregarRegistro(Convert.ToInt32(e.CommandArgument));
            }
        }
        private void CarregarRegistro(int id)
        {
            using (SqlConnection conn = new SqlConnection(
                WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                string sql = "SELECT * FROM tbfretes WHERE id_frete=@id";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);

                conn.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    ddlFrete.SelectedValue = dr["frete"].ToString();
                    ddlTipoFrete.SelectedValue = dr["medida"].ToString();
                    cboTipoViagem.SelectedItem.Text = dr["tipo_viagem"].ToString();
                    cboTipoVeiculo.SelectedItem.Text = dr["tipo_veiculo"].ToString();
                    cboTipoMaterial.SelectedItem.Text = dr["material"].ToString();
                    txtDetalheMaterial.Text = dr["detalhe_material"].ToString();
                    ddlTabela.SelectedItem.Text = dr["tabela_antt"].ToString();                    
                    txtPesoLotacao.Text = dr["lotacao_peso"].ToString();
                    ddlEixos.SelectedItem.Text = dr["eixos"].ToString();
                    ddlTipoCargaANTT.SelectedItem.Text = dr["tipo_carga"].ToString();

                    txtFreteMinimo.Text = Convert.ToDecimal(dr["frete_antt"]).ToString("N2", new CultureInfo("pt-BR"));
                    txtFreteReceber.Text = Convert.ToDecimal(dr["frete_receber"]).ToString("N2", new CultureInfo("pt-BR")); 
                    txtFretePagar.Text = Convert.ToDecimal(dr["frete_pagar"]).ToString("N2", new CultureInfo("pt-BR"));
                    txtMargem.Text = Convert.ToDecimal(dr["margem"]).ToString("N2", new CultureInfo("pt-BR"));
                    txtVigenciaInicial.Text = Convert.ToDateTime(dr["vigencia_inicial"]).ToString("dd/MM/yyyy");
                    txtVigenciaFinal.Text = Convert.ToDateTime(dr["vigencia_final"]).ToString("dd/MM/yyyy");
                    txtGRIS.Text = Convert.ToDecimal(dr["gris"]).ToString("N2", new CultureInfo("pt-BR"));
                    txtColeta.Text = Convert.ToDecimal(dr["coleta"]).ToString("N2", new CultureInfo("pt-BR"));
                    txtEntrega.Text = Convert.ToDecimal(dr["entrega"]).ToString("N2", new CultureInfo("pt-BR"));
                    txtTDE.Text = Convert.ToDecimal(dr["tde"]).ToString("N2", new CultureInfo("pt-BR"));
                    txtTDA.Text = Convert.ToDecimal(dr["tda"]).ToString("N2", new CultureInfo("pt-BR"));
                    txtTotalFrete.Text = Convert.ToDecimal(dr["total_frete"]).ToString("N2", new CultureInfo("pt-BR"));
                    txtObservacao.Text = dr["mensagem"].ToString();
                    txtResponsavel.Text = dr["responsavel"].ToString();
                    txtData_Alteracao.Text = Convert.ToDateTime(dr["data_alteracao"]).ToString("dd/MM/yyyy HH:mm");
                    ViewState["id_frete"] = id;
                }
            }

        }
        private void PreencherTabelaANTT()
        {
            // Consulta SQL que retorna os dados desejados
            string query = @"
            SELECT tabela
            FROM tbresolucoesantt
            WHERE vigente = 'SIM'
            GROUP BY tabela
            ORDER BY tabela";

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
                    ddlTabela.DataSource = reader;
                    ddlTabela.DataTextField = "tabela";  // Campo que será mostrado no ComboBox
                    ddlTabela.DataValueField = "tabela";  // Campo que será o valor de cada item                    
                    ddlTabela.DataBind();  // Realiza o binding dos dados                   
                    ddlTabela.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Selecione...", "0"));                    
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