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


namespace NewCapit.dist.pages
{
    public partial class WebForm1 : System.Web.UI.Page
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
               


                PreencherComboRotas();
                PreencherComboFiliais();
                PreencherComboTipoVeiculos();
                PreencherComboMateriais();
                PreencherComboTipoViagens();
                PreencherComboConsignario();
                PreencherComboPagador();
                PreencherComboMotorista();
                PreencherNumTabelaDeFrete();
            }

            //DateTime dataHoraAtual = DateTime.Now;
            txtAltCad.Text = dataHoraAtual.ToString("dd/MM/yyyy HH:mm");
            lbDtAtualizacao.Text = dataHoraAtual.ToString("dd/MM/yyyy HH:mm");
            txtStatusRota.Text = "ATIVO";
        }
        private void PreencherComboFiliais()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT codigo, descricao FROM tbempresa order by descricao";

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
                    cboFilial.DataSource = reader;
                    cboFilial.DataTextField = "descricao";  // Campo que será mostrado no ComboBox
                    cboFilial.DataValueField = "codigo";  // Campo que será o valor de cada item                    
                    cboFilial.DataBind();  // Realiza o binding dos dados                   
                    cboFilial.Items.Insert(0, new ListItem("Selecione...", "0"));
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
                    cboTipoMaterial.Items.Insert(0, new ListItem("Selecione...", "0"));
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
        private void PreencherComboMotorista()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT id, codmot, nommot, codtra, transp FROM tbmotoristas where fl_exclusao is null and status = 'ATIVO' and tipomot != 'FUNCIONÁRIO' order by nommot";

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
                    cboNomAgregado.DataSource = reader;
                    cboNomAgregado.DataTextField = "nommot";  // Campo que será mostrado no ComboBox
                    cboNomAgregado.DataValueField = "id";  // Campo que será o valor de cada item                    
                    cboNomAgregado.DataBind();  // Realiza o binding dos dados                   
                    cboNomAgregado.Items.Insert(0, new ListItem("Selecione...", "0"));
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
            string query = "SELECT codigo, descricao FROM tbtipoveic order by descricao";

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
                    cboTipoVeiculo.DataTextField = "descricao";  // Campo que será mostrado no ComboBox
                    cboTipoVeiculo.DataValueField = "codigo";  // Campo que será o valor de cada item                    
                    cboTipoVeiculo.DataBind();  // Realiza o binding dos dados                   
                    cboTipoVeiculo.Items.Insert(0, new ListItem("Selecione...", "0"));
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
                    cboTipoViagem.Items.Insert(0, new ListItem("Selecione...", "0"));
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
            string query = "SELECT codcli, nomcli, cidcli, estcli, ativo_inativo, fl_exclusao FROM tbclientes order by nomcli";

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
                    cboConsignatario.DataTextField = "nomcli";  // Campo que será mostrado no ComboBox
                    cboConsignatario.DataValueField = "codcli";  // Campo que será o valor de cada item                    
                    cboConsignatario.DataBind();  // Realiza o binding dos dados                   
                    cboConsignatario.Items.Insert(0, new ListItem("Selecione...", "0"));
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
            string query = "SELECT codcli, nomcli, cidcli, estcli, ativo_inativo, fl_exclusao FROM tbclientes order by nomcli";

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
                    cboPagador.DataTextField = "nomcli";  // Campo que será mostrado no ComboBox
                    cboPagador.DataValueField = "codcli";  // Campo que será o valor de cada item                    
                    cboPagador.DataBind();  // Realiza o binding dos dados                   
                    cboPagador.Items.Insert(0, new ListItem("Selecione...", "0"));
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
        private void PreencherComboRotas()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT rota, desc_rota, fl_exclusao FROM tbrotasdeentregas where fl_exclusao is null  and situacao = 'ATIVO' order by desc_rota";

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
                    cboRotas.DataSource = reader;
                    cboRotas.DataTextField = "desc_rota";  // Campo que será mostrado no ComboBox
                    cboRotas.DataValueField = "rota";  // Campo que será o valor de cada item                    
                    cboRotas.DataBind();  // Realiza o binding dos dados                   
                    cboRotas.Items.Insert(0, new ListItem("Selecione...", "0"));
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
        private void CarregarDadosFrete(string codFrete)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = "SELECT * FROM tbtabeladefretes WHERE cod_frete = @cod_frete";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@cod_frete", codFrete);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    // 🔹 Campos simples
                    txtCodPagador.Text = dr["cod_pagador"].ToString();
                    txtCodExpedidor.Text = dr["cod_expedidor"].ToString();
                    txtCidExpedidor.Text = dr["cid_expedidor"].ToString();
                    txtUFExpedidor.Text = dr["uf_expedidor"].ToString();
                    txtCodRecebedor.Text = dr["cod_recebedor"].ToString();
                    txtCidRecebedor.Text = dr["cid_recebedor"].ToString();
                    txtUFRecebedor.Text = dr["uf_recebedor"].ToString();
                    txtRota.Text = dr["rota"].ToString();
                    txtDuracao.Text = dr["Tempo"].ToString();
                    txtObservacao.Text = dr["observacao"].ToString();
                    txtUsuCadastro.Text = dr["cadastro_usuario"].ToString();
                    txtStatusRota.Text = dr["situacao"].ToString();

                    // 🔹 Campos numéricos / decimais
                    txtDistancia.Text = Convert.ToDecimal(dr["distancia"]).ToString("N2");
                    txtFreteTNG.Text = Convert.ToDecimal(dr["frete_tng"]).ToString("N2");
                    txtFreteAgregado.Text = Convert.ToDecimal(dr["frete_agregado"]).ToString("N2");
                    txtFreteAgregadoComDesconto.Text = Convert.ToDecimal(dr["frete_agregado_com_desc_carreta"]).ToString("N2");
                    txtFreteTerceiro.Text = Convert.ToDecimal(dr["frete_terceiro"]).ToString("N2");
                    txtAdicional.Text = Convert.ToDecimal(dr["adicional_sobrenf"]).ToString("N2");
                    txtSecCat.Text = Convert.ToDecimal(dr["sec_cat"]).ToString("N2");
                    txtDespacho.Text = Convert.ToDecimal(dr["despacho"]).ToString("N2");
                    txtPedagio.Text = Convert.ToDecimal(dr["pedagio"]).ToString("N2");
                    txtOutros.Text = Convert.ToDecimal(dr["outros"]).ToString("N2");
                    txtPercentualAluguelCarreta.Text = Convert.ToDecimal(dr["aluguel_carreta"]).ToString("N2");
                    txtAluguelCarretaEspecial.Text = Convert.ToDecimal(dr["desc_carreta"]).ToString("N2");
                    txtFreteEspecial.Text = Convert.ToDecimal(dr["valor_especial"]).ToString("N2");
                    txtFreteEspecialComDesconto.Text = Convert.ToDecimal(dr["valor_com_desconto_especial"]).ToString("N2");
                    txtDespAdm.Text = Convert.ToDecimal(dr["despesa_adm"]).ToString("N2");
                    txtPercTNGAgregado.Text = Convert.ToDecimal(dr["perc_frete_agregado"]).ToString("N2");
                    txtPercTngTerceiro.Text = Convert.ToDecimal(dr["perc_frete_terceiro"]).ToString("N2");
                    txtPercTNGEspecial.Text = Convert.ToDecimal(dr["perc_frete_especial"]).ToString("N2");

                    // 🔹 Datas (com verificação)
                    txtVigenciaInicial.Text = dr["vigencia_inicial"] != DBNull.Value ? Convert.ToDateTime(dr["vigencia_inicial"]).ToString("yyyy-MM-dd") : "";
                    txtVigenciaFinal.Text = dr["vigencia_final"] != DBNull.Value ? Convert.ToDateTime(dr["vigencia_final"]).ToString("yyyy-MM-dd") : "";
                    txtVigenciaAgregadoInicial.Text = dr["vigencia_inicial_agregado"] != DBNull.Value ? Convert.ToDateTime(dr["vigencia_inicial_agregado"]).ToString("yyyy-MM-dd") : "";
                    txtVigenciaAgregadoFinal.Text = dr["vigencia_final_agregado"] != DBNull.Value ? Convert.ToDateTime(dr["vigencia_final_agregado"]).ToString("yyyy-MM-dd") : "";
                    txtVigenciaTerceiroInicial.Text = dr["vigencia_inicial_terceiro"] != DBNull.Value ? Convert.ToDateTime(dr["vigencia_inicial_terceiro"]).ToString("yyyy-MM-dd") : "";
                    txtVigenciaTerceiroFinal.Text = dr["vigencia_final_terceiro"] != DBNull.Value ? Convert.ToDateTime(dr["vigencia_final_terceiro"]).ToString("yyyy-MM-dd") : "";
                    txtCodConsignatario.Text = dr["cod_consignatario"].ToString();
                    cboConsignatario.SelectedItem.Text = dr["consignatario"].ToString();
                    txtCidConsignatario.Text = dr["cid_consignatario"].ToString();
                    txtUFConsignatario.Text = dr["uf_consignatario"].ToString();
                    cboPagador.SelectedItem.Text = dr["pagador"].ToString();
                    txtCidPagador.Text = dr["cid_pagador"].ToString();
                    txtUFPagador.Text = dr["uf_pagador"].ToString();
                    // 🔹 DropDownLists e combos (usa SelectedValue se possível)
                    cboFilial.SelectedItem.Text= dr["nucleo"].ToString();
                    cboTipoMaterial.SelectedItem.Text =  dr["tipo_material"].ToString();
                    cboTipoVeiculo.SelectedItem.Text= dr["tipo_veiculo"].ToString();
                    cboTipoViagem.SelectedItem.Text =  dr["tipo_viagem"].ToString();
                    cboDeslocamento.Text =  dr["deslocamento"].ToString();
                    ddlTerceiro.SelectedItem.Text= dr["valor_fixo_terceiro"].ToString();
                    ddlValorFixoTng.SelectedItem.Text =  dr["valor_fixo_tng"].ToString();
                    ddlEmitePedagio.Items.Insert(0, new ListItem(dr["emitepedagio"].ToString())); 
                    cboNomAgregado.SelectedItem.Text= dr["nommot_especial"].ToString();
                    txtCadastro.Text = dr["cadastro_usuario"].ToString();
                    lblDtCadastro.Text = dr["data_cadastro"].ToString();
                    //ddlHoraParada.SelectedItem.Text=
                    rota = dr["rota"].ToString();
                    txtCodAgregado.Text = dr["codmot_especial"].ToString();
                    txtCodTra.Text = dr["codtra_especial"].ToString();
                    txtTransp.Text = dr["transp_especial"].ToString();
                    CarregaRotas(rota);
                    // 🔹 RadioButton customizado (lotação mínima)
                    string lotacao = dr["lotacao_minima"].ToString();
                    ScriptManager.RegisterStartupScript(this, GetType(), "SetRadio",
                        $"document.querySelector('input[name=\"customRadioTipo\"][value=\"{lotacao}\"]').checked = true;", true);
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "NaoEncontrado",
                        "<script>alert('⚠️ Frete não encontrado.');</script>");
                }

                dr.Close();
            }
        }

        protected void txtCodConsignatario_TextChanged(object sender, EventArgs e)
        {
            if (txtCodConsignatario.Text != "")
            {
                string cod = txtCodConsignatario.Text;
                string sql = "SELECT codcli, nomcli, cidcli, estcli, ativo_inativo, fl_exclusao FROM tbclientes where codcli = '" + cod + "' and ativo_inativo = 'ATIVO' and fl_exclusao is null";
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
                string query = "SELECT codcli, nomcli, cidcli, estcli FROM tbclientes WHERE codcli = @ID";
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
                string sql = "SELECT codcli, nomcli, cidcli, estcli, ativo_inativo, fl_exclusao FROM tbclientes where codcli = '" + cod + "' and ativo_inativo = 'ATIVO' and fl_exclusao is null";
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
                string query = "SELECT codcli, nomcli, cidcli, estcli FROM tbclientes WHERE codcli = @ID";
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

        protected void txtRota_TextChanged(object sender, EventArgs e)
        {
            if (txtRota.Text != "")
            {
                string cod = txtRota.Text;
                string sql = "SELECT rota, desc_rota, codigo_remetente, nome_remetente, cidade_remetente, uf_remetente, codigo_expedidor, nome_expedidor, cidade_expedidor, uf_expedidor, codigo_destinatario, nome_destinatario, cidade_destinatario, uf_destinatario,codigo_recebedor, nome_recebedor, cidade_recebedor, uf_recebedor, distancia, tempo, deslocamento, fl_exclusao, situacao FROM tbrotasdeentregas where rota = '" + cod + "' and situacao = 'ATIVO' and fl_exclusao is null";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                conn.Open();
                da.Fill(dt);
                conn.Close();

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0][17].ToString() == null)
                    {
                        // Acione o toast quando a página for carregada
                        string script = "<script>showToast('Rota deletado do sistema.');</script>";
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                        txtRota.Text = "";
                        txtRota.Focus();
                        return;
                    }
                    else if (dt.Rows[0][18].ToString() == "INATIVO")
                    {
                        // Acione o toast quando a página for carregada
                        string script = "<script>showToast('Rota inativa no sistema.');</script>";
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                        txtRota.Text = "";
                        txtRota.Focus();
                        return;
                    }
                    else
                    {
                        txtRota.Text = dt.Rows[0][0].ToString();
                        cboRotas.SelectedItem.Text = dt.Rows[0][1].ToString();
                        txtCodRemetente.Text = dt.Rows[0][2].ToString();
                        cboRemetente.Text = dt.Rows[0][3].ToString();
                        txtMunicipioRemetente.Text = dt.Rows[0][4].ToString();
                        txtUFRemetente.Text = dt.Rows[0][5].ToString();

                        txtCodExpedidor.Text = dt.Rows[0][6].ToString();
                        cboExpedidor.Text = dt.Rows[0][7].ToString();
                        txtCidExpedidor.Text = dt.Rows[0][8].ToString();
                        txtUFExpedidor.Text = dt.Rows[0][9].ToString();

                        txtCodDestinatario.Text = dt.Rows[0][10].ToString();
                        cboDestinatario.Text = dt.Rows[0][11].ToString();
                        txtMunicipioDestinatario.Text = dt.Rows[0][12].ToString();
                        txtUFDestinatario.Text = dt.Rows[0][13].ToString();

                        txtCodRecebedor.Text = dt.Rows[0][14].ToString();
                        cboRecebedor.Text = dt.Rows[0][15].ToString();
                        txtCidRecebedor.Text = dt.Rows[0][16].ToString();
                        txtUFRecebedor.Text = dt.Rows[0][17].ToString();

                        txtDistancia.Text = dt.Rows[0][18].ToString();
                        txtDuracao.Text = dt.Rows[0][19].ToString();
                        cboDeslocamento.Text = dt.Rows[0][20].ToString();
                        return;
                    }

                }
                else
                {
                    // Acione o toast quando a página for carregada
                    string script = "<script>showToast('Rota não encontrada no sistema.');</script>";
                    ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                    txtRota.Text = "";
                    txtRota.Focus();
                    return;
                }
            }

        }
        public void CarregaRotas(string rota)
        {
            if (txtRota.Text != "")
            {
                string cod = txtRota.Text;
                string sql = "SELECT rota, desc_rota, codigo_remetente, nome_remetente, cidade_remetente, uf_remetente, codigo_expedidor, nome_expedidor, cidade_expedidor, uf_expedidor, codigo_destinatario, nome_destinatario, cidade_destinatario, uf_destinatario,codigo_recebedor, nome_recebedor, cidade_recebedor, uf_recebedor, distancia, tempo, deslocamento, fl_exclusao, situacao FROM tbrotasdeentregas where rota = '" + cod + "' and situacao = 'ATIVO' and fl_exclusao is null";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                conn.Open();
                da.Fill(dt);
                conn.Close();

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0][17].ToString() == null)
                    {
                        // Acione o toast quando a página for carregada
                        string script = "<script>showToast('Rota deletado do sistema.');</script>";
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                        txtRota.Text = "";
                        txtRota.Focus();
                        return;
                    }
                    else if (dt.Rows[0][18].ToString() == "INATIVO")
                    {
                        // Acione o toast quando a página for carregada
                        string script = "<script>showToast('Rota inativa no sistema.');</script>";
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                        txtRota.Text = "";
                        txtRota.Focus();
                        return;
                    }
                    else
                    {
                        txtRota.Text = dt.Rows[0][0].ToString();
                        cboRotas.SelectedItem.Text = dt.Rows[0][1].ToString();
                        txtCodRemetente.Text = dt.Rows[0][2].ToString();
                        cboRemetente.Text = dt.Rows[0][3].ToString();
                        txtMunicipioRemetente.Text = dt.Rows[0][4].ToString();
                        txtUFRemetente.Text = dt.Rows[0][5].ToString();

                        txtCodExpedidor.Text = dt.Rows[0][6].ToString();
                        cboExpedidor.Text = dt.Rows[0][7].ToString();
                        txtCidExpedidor.Text = dt.Rows[0][8].ToString();
                        txtUFExpedidor.Text = dt.Rows[0][9].ToString();

                        txtCodDestinatario.Text = dt.Rows[0][10].ToString();
                        cboDestinatario.Text = dt.Rows[0][11].ToString();
                        txtMunicipioDestinatario.Text = dt.Rows[0][12].ToString();
                        txtUFDestinatario.Text = dt.Rows[0][13].ToString();

                        txtCodRecebedor.Text = dt.Rows[0][14].ToString();
                        cboRecebedor.Text = dt.Rows[0][15].ToString();
                        txtCidRecebedor.Text = dt.Rows[0][16].ToString();
                        txtUFRecebedor.Text = dt.Rows[0][17].ToString();

                        txtDistancia.Text = dt.Rows[0][18].ToString();
                        txtDuracao.Text = dt.Rows[0][19].ToString();
                        cboDeslocamento.Text = dt.Rows[0][20].ToString();
                        return;
                    }

                }
                else
                {
                    // Acione o toast quando a página for carregada
                    string script = "<script>showToast('Rota não encontrada no sistema.');</script>";
                    ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                    txtRota.Text = "";
                    txtRota.Focus();
                    return;
                }
            }
        }
        protected void cboRotas_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idSelecionado = int.Parse(cboRotas.SelectedValue);

            // Preencher os campos com base no valor selecionado
            if (idSelecionado > 0)
            {
                PreencherCamposRotas(idSelecionado);
            }
            else
            {
                LimparCamposRotas();
            }
        }
        // Função para preencher os campos com os dados do banco
        private void PreencherCamposRotas(int id)
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                conn.Open();
                string query = "SELECT rota, desc_rota, codigo_remetente, nome_remetente, cidade_remetente, uf_remetente, codigo_expedidor, nome_expedidor, cidade_expedidor, uf_expedidor, codigo_destinatario, nome_destinatario, cidade_destinatario, uf_destinatario, codigo_recebedor, nome_recebedor, cidade_recebedor, uf_recebedor, distancia, tempo, deslocamento FROM tbrotasdeentregas WHERE rota = @ID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtRota.Text = reader["rota"].ToString();
                    cboRotas.SelectedItem.Text = reader["desc_rota"].ToString();
                    txtCodRemetente.Text = reader["codigo_remetente"].ToString();
                    cboRemetente.Text = reader["nome_remetente"].ToString();
                    txtMunicipioRemetente.Text = reader["cidade_remetente"].ToString();
                    txtUFRemetente.Text = reader["uf_remetente"].ToString();

                    txtCodExpedidor.Text = reader["codigo_expedidor"].ToString();
                    cboExpedidor.Text = reader["nome_expedidor"].ToString();
                    txtCidExpedidor.Text = reader["cidade_expedidor"].ToString();
                    txtUFExpedidor.Text = reader["uf_expedidor"].ToString();

                    txtCodDestinatario.Text = reader["codigo_destinatario"].ToString();
                    cboDestinatario.Text = reader["nome_destinatario"].ToString();
                    txtMunicipioDestinatario.Text = reader["cidade_destinatario"].ToString();
                    txtUFDestinatario.Text = reader["uf_destinatario"].ToString();

                    txtCodRecebedor.Text = reader["codigo_recebedor"].ToString();
                    cboRecebedor.Text = reader["nome_recebedor"].ToString();
                    txtCidRecebedor.Text = reader["cidade_recebedor"].ToString();
                    txtUFRecebedor.Text = reader["uf_recebedor"].ToString();

                    txtDistancia.Text = reader["distancia"].ToString();
                    txtDuracao.Text = reader["tempo"].ToString();
                    cboDeslocamento.Text = reader["deslocamento"].ToString();
                    return;

                }
            }
        }
        // Função para limpar os campos
        private void LimparCamposRotas()
        {
            //txtCodPagador.Text = string.Empty;
            //txtCidPagador.Text = string.Empty;
            //txtUFPagador.Text = string.Empty;
        }

        protected void cboNomAgregado_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idSelecionado = int.Parse(cboNomAgregado.SelectedValue);

            // Preencher os campos com base no valor selecionado
            if (idSelecionado > 0)
            {
                PreencherCamposMotorista(idSelecionado);
            }
            else
            {
                LimparCamposMotorista();
            }
        }
        private void PreencherCamposMotorista(int id)
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                conn.Open();
                string query = "SELECT codmot, nommot, codtra, transp FROM tbmotoristas WHERE id = @ID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtCodAgregado.Text = reader["codmot"].ToString();
                    cboNomAgregado.SelectedItem.Text = reader["nommot"].ToString();
                    txtCodTra.Text = reader["codtra"].ToString();
                    txtTransp.Text = reader["transp"].ToString();
                    return;
                }
            }
        }
        // Função para limpar os campos
        private void LimparCamposMotorista()
        {
            //txtCodPagador.Text = string.Empty;
            //txtCidPagador.Text = string.Empty;
            //txtUFPagador.Text = string.Empty;
        }

        protected void txtCodAgregado_TextChanged(object sender, EventArgs e)
        {
            if (txtCodAgregado.Text != "")
            {
                string cod = txtCodAgregado.Text;
                string sql = "SELECT * FROM tbmotoristas where codmot = '" + cod + "' and status = 'ATIVO' and tipomot != 'FUNCIONÁRIO' and fl_exclusao is null";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                conn.Open();
                da.Fill(dt);
                conn.Close();

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0][68].ToString() == null)
                    {
                        // Acione o toast quando a página for carregada
                        string script = "<script>showToast('Agregado/Terceiro deletado do sistema.');</script>";
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                        txtCodAgregado.Text = "";
                        txtCodAgregado.Focus();
                        return;
                    }
                    else if (dt.Rows[0][3].ToString() == "INATIVO")
                    {
                        // Acione o toast quando a página for carregada
                        string script = "<script>showToast('Agregado/Terceiro inativa no sistema.');</script>";
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                        txtCodAgregado.Text = "";
                        cboNomAgregado.SelectedItem.Text = "";
                        txtCodTra.Text = "";
                        txtTransp.Text = "";
                        txtCodAgregado.Text = "";
                        txtCodAgregado.Focus();
                        return;
                    }
                    else if (dt.Rows[0][44].ToString() == "FUNCIONÁRIO")
                    {
                        // Acione o toast quando a página for carregada
                        string script = "<script>showToast('Motorista inválido, é funcionário.');</script>";
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                        txtCodAgregado.Text = "";
                        cboNomAgregado.SelectedItem.Text = "";
                        txtCodTra.Text = "";
                        txtTransp.Text = "";
                        txtCodAgregado.Text = "";
                        txtCodAgregado.Focus();
                        return;
                    }
                    else
                    {
                        txtCodAgregado.Text = dt.Rows[0][1].ToString();
                        cboNomAgregado.SelectedItem.Text = dt.Rows[0][2].ToString();
                        txtCodTra.Text = dt.Rows[0][29].ToString();
                        txtTransp.Text = dt.Rows[0][30].ToString();
                        return;
                    }

                }
                else
                {
                    // Acione o toast quando a página for carregada
                    string script = "<script>showToast('Agregado/Terceiro não encontrado no sistema.');</script>";
                    ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                    txtCodAgregado.Text = "";
                    cboNomAgregado.SelectedItem.Text = "";
                    txtCodTra.Text = "";
                    txtTransp.Text = "";

                    txtCodAgregado.Text = "";
                    txtCodAgregado.Focus();
                    return;
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

            string valor = Request.Form["customRadioTipo"];
            string lotacaomin = string.IsNullOrEmpty(valor) ? "NÃO" : valor;

            // 🔹 Monta o campo descr_frete
            string[] pagador = cboPagador.SelectedItem.Text.Split(' ');
            string[] expedidor = cboExpedidor.Text.Split(' ');
            string[] recebedor = cboRecebedor.Text.Split(' ');

            string descr_frete = $"{txtCodPagador.Text} - {pagador[0]} - Exped./Recb.: {txtCodExpedidor.Text} - {expedidor[0]}({txtCidExpedidor.Text}/{txtUFExpedidor.Text})x {txtCodRecebedor.Text} - {recebedor[0]}({txtCidRecebedor.Text}/{txtUFRecebedor.Text}) - Material: {cboTipoMaterial.SelectedItem.Text} - Veículo: {cboTipoVeiculo.SelectedItem.Text}";

            string connectionString = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = @"
                                            UPDATE tbtabeladefretes SET
                                                desc_frete = @desc_frete,
                                                rota = @rota,
                                                desc_rota = @desc_rota,
                                                cod_remetente = @cod_remetente,
                                                remetente = @remetente,
                                                cid_remetente = @cid_remetente,
                                                uf_remetente = @uf_remetente,
                                                cod_expedidor = @cod_expedidor,
                                                expedidor = @expedidor,
                                                cid_expedidor = @cid_expedidor,
                                                uf_expedidor = @uf_expedidor,
                                                cod_destinatario = @cod_destinatario,
                                                destinatario = @destinatario,
                                                cid_destinatario = @cid_destinatario,
                                                uf_destinatario = @uf_destinatario,
                                                cod_recebedor = @cod_recebedor,
                                                recebedor = @recebedor,
                                                cid_recebedor = @cid_recebedor,
                                                uf_recebedor = @uf_recebedor,
                                                cod_consignatario = @cod_consignatario,
                                                consignatario = @consignatario,
                                                cid_consignatario = @cid_consignatario,
                                                uf_consignatario = @uf_consignatario,
                                                cod_pagador = @cod_pagador,
                                                pagador = @pagador,
                                                cid_pagador = @cid_pagador,
                                                uf_pagador = @uf_pagador,
                                                nucleo = @nucleo,
                                                distancia = @distancia,
                                                Tempo = @Tempo,
                                                frete_tng = @frete_tng,
                                                frete_agregado = @frete_agregado,
                                                frete_agregado_com_desc_carreta = @frete_agregado_com_desc_carreta,
                                                frete_terceiro = @frete_terceiro,
                                                adicional_sobrenf = @adicional_sobrenf,
                                                sec_cat = @sec_cat,
                                                despacho = @despacho,
                                                pedagio = @pedagio,
                                                outros = @outros,
                                                total_frete = @total_frete,
                                                tipo_veiculo = @tipo_veiculo,
                                                tipo_material = @tipo_material,
                                                situacao = @situacao,
                                                tipo_viagem = @tipo_viagem,
                                                deslocamento = @deslocamento,
                                                vigencia_inicial = @vigencia_inicial,
                                                vigencia_final = @vigencia_final,
                                                lotacao_minima = @lotacao_minima,
                                                valor_fixo_terceiro = @valor_fixo_terceiro,
                                                aluguel_carreta = @aluguel_carreta,
                                                desc_carreta = @desc_carreta,
                                                valor_fixo_tng = @valor_fixo_tng,
                                                valor_especial = @valor_especial,
                                                desc_especial = @desc_especial,
                                                valor_com_desconto_especial = @valor_com_desconto_especial,
                                                observacao = @observacao,
                                                alteracao_usuario = @alteracao_usuario,
                                                alteracao_data = @alteracao_data,
                                                emitepedagio = @emitepedagio,
                                                vigencia_inicial_agregado = @vigencia_inicial_agregado,
                                                vigencia_final_agregado = @vigencia_final_agregado,
                                                vigencia_inicial_terceiro = @vigencia_inicial_terceiro,
                                                vigencia_final_terceiro = @vigencia_final_terceiro,
                                                despesa_adm = @despesa_adm,
                                                codmot_especial = @codmot_especial,
                                                nommot_especial = @nommot_especial,
                                                codtra_especial = @codtra_especial,
                                                transp_especial = @transp_especial,
                                                perc_frete_agregado = @perc_frete_agregado,
                                                perc_frete_terceiro = @perc_frete_terceiro,
                                                perc_frete_especial = @perc_frete_especial
                                            WHERE cod_frete = @cod_frete";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    // Mesmo mapeamento de parâmetros do seu INSERT
                    cmd.Parameters.Add("@cod_frete", SqlDbType.Int).Value = Convert.ToInt32(novaTabelaDeFrete.Text);
                    cmd.Parameters.Add("@desc_frete", SqlDbType.NVarChar).Value = descr_frete;
                    cmd.Parameters.Add("@rota", SqlDbType.Int).Value = Convert.ToInt32(txtRota.Text);
                    cmd.Parameters.Add("@desc_rota", SqlDbType.NVarChar).Value = cboRotas.SelectedItem.Text;
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
                    cmd.Parameters.Add("@cod_consignatario", SqlDbType.Int).Value = Convert.ToInt32(txtCodConsignatario.Text);
                    cmd.Parameters.Add("@consignatario", SqlDbType.NVarChar).Value = cboConsignatario.SelectedItem.Text;
                    cmd.Parameters.Add("@cid_consignatario", SqlDbType.NVarChar).Value = txtCidConsignatario.Text;
                    cmd.Parameters.Add("@uf_consignatario", SqlDbType.NVarChar).Value = txtUFConsignatario.Text;
                    cmd.Parameters.Add("@cod_pagador", SqlDbType.Int).Value = Convert.ToInt32(txtCodPagador.Text);
                    cmd.Parameters.Add("@pagador", SqlDbType.NVarChar).Value = cboPagador.SelectedItem.Text;
                    cmd.Parameters.Add("@cid_pagador", SqlDbType.NVarChar).Value = txtCidPagador.Text;
                    cmd.Parameters.Add("@uf_pagador", SqlDbType.NVarChar).Value = txtUFPagador.Text;
                    cmd.Parameters.Add("@nucleo", SqlDbType.NVarChar).Value = cboFilial.SelectedItem.Text;
                    cmd.Parameters.Add("@Tempo", SqlDbType.NVarChar).Value = txtDuracao.Text;
                    cmd.Parameters.Add("@tipo_veiculo", SqlDbType.NVarChar).Value = cboTipoVeiculo.SelectedItem.Text;
                    cmd.Parameters.Add("@tipo_material", SqlDbType.NVarChar).Value = cboTipoMaterial.SelectedItem.Text;
                    cmd.Parameters.Add("@situacao", SqlDbType.NVarChar).Value = txtStatusRota.Text;
                    cmd.Parameters.Add("@tipo_viagem", SqlDbType.NVarChar).Value = cboTipoViagem.SelectedItem.Text;
                    cmd.Parameters.Add("@deslocamento", SqlDbType.NVarChar).Value = cboDeslocamento.Text;
                    cmd.Parameters.Add("@lotacao_minima", SqlDbType.NVarChar).Value = lotacaomin;
                    cmd.Parameters.Add("@valor_fixo_terceiro", SqlDbType.NVarChar).Value = ddlTerceiro.SelectedItem.Text;
                    cmd.Parameters.Add("@valor_fixo_tng", SqlDbType.NVarChar).Value = ddlValorFixoTng.SelectedItem.Text;
                    cmd.Parameters.Add("@observacao", SqlDbType.NVarChar).Value = txtObservacao.Text;
                    cmd.Parameters.Add("@alteracao_usuario", SqlDbType.NVarChar).Value = txtUsuCadastro.Text;
                    cmd.Parameters.Add("@alteracao_data", SqlDbType.NVarChar).Value = lbDtAtualizacao.Text;
                    cmd.Parameters.Add("@emitepedagio", SqlDbType.NVarChar).Value = ddlEmitePedagio.SelectedValue;
                    cmd.Parameters.Add("@codmot_especial", SqlDbType.NChar).Value = txtCodAgregado.Text;
                    cmd.Parameters.Add("@nommot_especial", SqlDbType.NVarChar).Value = cboNomAgregado.SelectedItem.Text;
                    cmd.Parameters.Add("@codtra_especial", SqlDbType.NChar).Value = txtCodTra.Text;
                    cmd.Parameters.Add("@transp_especial", SqlDbType.NVarChar).Value = txtTransp.Text;
                    cmd.Parameters.Add("@vigencia_inicial", SqlDbType.Date).Value = SafeDateValue(txtVigenciaInicial.Text);
                    cmd.Parameters.Add("@vigencia_final", SqlDbType.Date).Value = SafeDateValue(txtVigenciaFinal.Text);
                    cmd.Parameters.Add("@vigencia_inicial_agregado", SqlDbType.Date).Value = SafeDateValue(txtVigenciaAgregadoInicial.Text);
                    cmd.Parameters.Add("@vigencia_final_agregado", SqlDbType.Date).Value = SafeDateValue(txtVigenciaAgregadoFinal.Text);
                    cmd.Parameters.Add("@vigencia_inicial_terceiro", SqlDbType.Date).Value = SafeDateValue(txtVigenciaTerceiroInicial.Text);
                    cmd.Parameters.Add("@vigencia_final_terceiro", SqlDbType.Date).Value = SafeDateValue(txtVigenciaTerceiroFinal.Text);
                    cmd.Parameters.Add("@distancia", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtDistancia.Text);
                    cmd.Parameters.Add("@frete_tng", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtFreteTNG.Text);
                    cmd.Parameters.Add("@frete_agregado", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtFreteAgregado.Text);
                    cmd.Parameters.Add("@frete_agregado_com_desc_carreta", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtFreteAgregadoComDesconto.Text);
                    cmd.Parameters.Add("@frete_terceiro", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtFreteTerceiro.Text);
                    cmd.Parameters.Add("@adicional_sobrenf", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtAdicional.Text);
                    cmd.Parameters.Add("@sec_cat", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtSecCat.Text);
                    cmd.Parameters.Add("@despacho", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtDespacho.Text);
                    cmd.Parameters.Add("@pedagio", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtPedagio.Text);
                    cmd.Parameters.Add("@outros", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtOutros.Text);
                    cmd.Parameters.Add("@total_frete", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtFreteTNG.Text);
                    cmd.Parameters.Add("@aluguel_carreta", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtPercentualAluguelCarreta.Text);
                    cmd.Parameters.Add("@desc_carreta", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtAluguelCarretaEspecial.Text);
                    cmd.Parameters.Add("@valor_especial", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtFreteEspecial.Text);
                    cmd.Parameters.Add("@desc_especial", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtAluguelCarretaEspecial.Text);
                    cmd.Parameters.Add("@valor_com_desconto_especial", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtFreteEspecialComDesconto.Text);
                    cmd.Parameters.Add("@despesa_adm", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtDespAdm.Text);
                    cmd.Parameters.Add("@perc_frete_agregado", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtPercTNGAgregado.Text);
                    cmd.Parameters.Add("@perc_frete_terceiro", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtPercTngTerceiro.Text);
                    cmd.Parameters.Add("@perc_frete_especial", SqlDbType.Decimal).Value = LimparMascaraMoeda(txtPercTNGEspecial.Text);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                ClientScript.RegisterStartupScript(this.GetType(), "Sucesso",
                    "<script>alert('✅ Frete atualizado com sucesso!');</script>");
            }
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
    }
}