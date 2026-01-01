using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using NPOI.SS.Formula;

namespace NewCapit.dist.pages
{
    public partial class Frm_CadPedidos : System.Web.UI.Page
    {
        SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString());
        string nomeUsuario = string.Empty;
        DateTime dataHoraAtual = DateTime.Now;
        int totalQuantidade = 0;
        //int totalLinhas = 0;
        string totalPesoCarga;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UsuarioLogado"] != null)
                {
                    string nomeUsuario = Session["UsuarioLogado"].ToString();
                    var lblUsuario = nomeUsuario;

                    txtUsuCadastro.Text = nomeUsuario;
                    txtUsuAlteracao.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                }
                else
                {
                    var lblUsuario = "<Usuário>";
                    Response.Redirect("Login.aspx");

                }
                PreencherNumCarga();
                PreencherComboSolicitantes();
                PreencherComboGR();
                PreencherComboFretes();
                PreencherComboMateriais();
                PreencherComboDeposito();
            }
            //DateTime dataHoraAtual = DateTime.Now;
            txtCadastro.Text = dataHoraAtual.ToString("dd/MM/yyyy HH:mm");
        }
        private void PreencherNumCarga()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT (carga + incremento) as ProximaCarga FROM tbcontadores";

            // Crie uma conexão com o banco de dados
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Crie o comando SQL
                        //SqlCommand cmd = new SqlCommand(query, conn);

                        // Execute o comando e obtenha os dados em um DataReader
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                // Preencher o TextBox com o nome encontrado 
                                novaCarga.Text = reader["ProximaCarga"].ToString();
                            }
                        }

                    }
                    string id = "1";

                    // Verifica se o ID foi fornecido e é um número válido
                    if (string.IsNullOrEmpty(id) || !int.TryParse(id, out int idConvertido))
                    {
                        // Acione o toast quando a página for carregada
                        string script = "<script>showToast('ID invalido ou não fornecido.');</script>";
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                        return;
                    }
                    string sql = @"UPDATE tbcontadores SET carga = @carga WHERE id = @id";
                    try
                    {
                        using (SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
                        using (SqlCommand cmd = new SqlCommand(sql, con))
                        {
                            cmd.Parameters.AddWithValue("@carga", novaCarga.Text);
                            cmd.Parameters.AddWithValue("@id", idConvertido);

                            con.Open();
                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                // atualiza  
                            }
                            else
                            {
                                // Acione o toast quando a página for carregada
                                string script = "<script>showToast('Erro ao atualizar o número da carga.');</script>";
                                ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                            }

                        }


                    }
                    catch (Exception ex)
                    {
                        string mensagemErro = $"Erro ao atualizar: {HttpUtility.JavaScriptStringEncode(ex.Message)}";
                        string script = $"alert('{mensagemErro}');";
                        ClientScript.RegisterStartupScript(this.GetType(), "Erro", script, true);
                    }
                }
                catch (Exception ex)
                {
                    //Tratar erro
                    //txtResultado.Text = "Erro: " + ex.Message;
                }
            }
        }
        private void PreencherComboSolicitantes()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT id, nome FROM tbsolicitantes";

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
                    cbSolicitantes.DataSource = reader;
                    cbSolicitantes.DataTextField = "nome";  // Campo que será mostrado no ComboBox
                    cbSolicitantes.DataValueField = "id";  // Campo que será o valor de cada item                    
                    cbSolicitantes.DataBind();  // Realiza o binding dos dados                   
                    cbSolicitantes.Items.Insert(0, new ListItem("Selecione...", "0"));
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
        private void PreencherComboGR()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT id, nomgr FROM tbgr";

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
                    cboGR.DataSource = reader;
                    cboGR.DataTextField = "nomgr";  // Campo que será mostrado no ComboBox
                    cboGR.DataValueField = "id";  // Campo que será o valor de cada item                    
                    cboGR.DataBind();  // Realiza o binding dos dados                   
                    cboGR.Items.Insert(0, new ListItem("Selecione...", "0"));
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
            string query = "SELECT codigo, descricao, lotacao FROM tbtipodematerial where status = 'ATIVO' ORDER BY descricao";

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
                    cboMaterial.DataSource = reader;
                    cboMaterial.DataTextField = "descricao";  // Campo que será mostrado no ComboBox
                    cboMaterial.DataValueField = "codigo";  // Campo que será o valor de cada item                    
                    cboMaterial.DataBind();  // Realiza o binding dos dados                   
                    cboMaterial.Items.Insert(0, new ListItem("Selecione...", "0"));
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
        protected void cboMaterial_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cboMaterial.SelectedItem.Text))
            {
                txtLotacao.Text = "";
                return;
            }

            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                string sql = @"SELECT lotacao 
                       FROM tbtipodematerial 
                       WHERE descricao = @descricao";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@descricao", cboMaterial.SelectedItem.Text);

                    conn.Open();
                    object result = cmd.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                        txtLotacao.Text = result.ToString();
                    else
                        txtLotacao.Text = "";
                }
            }
        }

        private void PreencherComboDeposito()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT id, descricao FROM tblocalcarregamento";

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
                    cboDeposito.DataSource = reader;
                    cboDeposito.DataTextField = "descricao";  // Campo que será mostrado no ComboBox
                    cboDeposito.DataValueField = "id";  // Campo que será o valor de cada item                    
                    cboDeposito.DataBind();  // Realiza o binding dos dados                   
                    cboDeposito.Items.Insert(0, new ListItem("Selecione...", "0"));
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
        private void PreencherComboFretes()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT cod_frete, desc_frete, fl_exclusao FROM tbtabeladefretes where fl_exclusao is null  and situacao = 'ATIVO' order by desc_frete";

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
                    cboFrete.DataSource = reader;
                    cboFrete.DataTextField = "desc_frete";  // Campo que será mostrado no ComboBox
                    cboFrete.DataValueField = "cod_frete";  // Campo que será o valor de cada item                    
                    cboFrete.DataBind();  // Realiza o binding dos dados                   
                    cboFrete.Items.Insert(0, new ListItem("Selecione...", "0"));
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
        protected void txtFrete_TextChanged(object sender, EventArgs e)
        {
            if (txtFrete.Text != "")
            {
                string cod = txtFrete.Text;
                string sql = "SELECT  cod_frete, desc_frete, cod_remetente, remetente, cid_remetente, uf_remetente, cod_expedidor, expedidor, cid_expedidor, uf_expedidor, cod_destinatario, destinatario, cid_destinatario, uf_destinatario,cod_recebedor, recebedor, cid_recebedor, uf_recebedor, cod_consignatario, consignatario, cid_consignatario, uf_consignatario, cod_pagador, pagador, cid_pagador, uf_pagador, nucleo, tipo_veiculo,  tempo, deslocamento, distancia, emitepedagio, fl_exclusao, situacao FROM tbtabeladefretes where cod_frete = '" + cod + "' and situacao = 'ATIVO' and fl_exclusao is null";
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
                        string script = "<script>showToast('Tabela de frete deletada do sistema.');</script>";
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                        txtFrete.Text = "";
                        txtFrete.Focus();
                        return;
                    }
                    else if (dt.Rows[0][18].ToString() == "INATIVO")
                    {
                        // Acione o toast quando a página for carregada
                        string script = "<script>showToast('Tabela de frete inativa no sistema.');</script>";
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                        txtFrete.Text = "";
                        txtFrete.Focus();
                        return;
                    }
                    else
                    {
                        txtFrete.Text = dt.Rows[0][0].ToString();
                        cboFrete.SelectedItem.Text = dt.Rows[0][1].ToString();
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

                        txtCodConsignatario.Text = dt.Rows[0][18].ToString();
                        txtConsignatario.Text = dt.Rows[0][19].ToString();
                        txtCidConsignatario.Text = dt.Rows[0][20].ToString();
                        txtUFConsignatario.Text = dt.Rows[0][21].ToString();

                        txtCodPagador.Text = dt.Rows[0][22].ToString();
                        txtPagador.Text = dt.Rows[0][23].ToString();
                        txtCidPagador.Text = dt.Rows[0][24].ToString();
                        txtUFPagador.Text = dt.Rows[0][25].ToString();

                        txtFilial.Text = dt.Rows[0][26].ToString();
                        txtTipoVeiculo.Text = dt.Rows[0][27].ToString();
                        txtDuracao.Text = dt.Rows[0][28].ToString();
                        txtDeslocamento.Text = dt.Rows[0][29].ToString();
                        txtDistancia.Text = dt.Rows[0][30].ToString();
                        txtPedagio.Text = dt.Rows[0][31].ToString();
                        return;
                    }

                }
                else
                {
                    // Acione o toast quando a página for carregada
                    string script = "<script>showToast('Tabela de frete não encontrada no sistema.');</script>";
                    ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                    txtFrete.Text = "";
                    txtFrete.Focus();
                    return;
                }
            }

        }
        protected void cboFrete_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idSelecionado = int.Parse(cboFrete.SelectedValue);

            // Preencher os campos com base no valor selecionado
            if (idSelecionado > 0)
            {
                PreencherCamposFrete(idSelecionado);
            }
            else
            {
                LimparCamposFrete();
            }
        }
        // Função para preencher os campos com os dados do banco
        private void PreencherCamposFrete(int id)
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                conn.Open();
                string query = "SELECT cod_frete, desc_frete, cod_remetente, remetente, cid_remetente, uf_remetente, cod_expedidor, expedidor, cid_expedidor, uf_expedidor, cod_destinatario, destinatario, cid_destinatario, uf_destinatario,cod_recebedor, recebedor, cid_recebedor, uf_recebedor, cod_consignatario, consignatario, cid_consignatario, uf_consignatario, cod_pagador, pagador, cid_pagador, uf_pagador, nucleo, tipo_veiculo,  tempo, deslocamento, distancia, emitepedagio, fl_exclusao, situacao FROM tbtabeladefretes WHERE cod_frete = @ID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtFrete.Text = reader["cod_frete"].ToString();
                    cboFrete.SelectedItem.Text = reader["desc_frete"].ToString();
                    txtCodRemetente.Text = reader["cod_remetente"].ToString();
                    cboRemetente.Text = reader["remetente"].ToString();
                    txtMunicipioRemetente.Text = reader["cid_remetente"].ToString();
                    txtUFRemetente.Text = reader["uf_remetente"].ToString();

                    txtCodExpedidor.Text = reader["cod_expedidor"].ToString();
                    cboExpedidor.Text = reader["expedidor"].ToString();
                    txtCidExpedidor.Text = reader["cid_expedidor"].ToString();
                    txtUFExpedidor.Text = reader["uf_expedidor"].ToString();

                    txtCodDestinatario.Text = reader["cod_destinatario"].ToString();
                    cboDestinatario.Text = reader["destinatario"].ToString();
                    txtMunicipioDestinatario.Text = reader["cid_destinatario"].ToString();
                    txtUFDestinatario.Text = reader["uf_destinatario"].ToString();

                    txtCodRecebedor.Text = reader["cod_recebedor"].ToString();
                    cboRecebedor.Text = reader["recebedor"].ToString();
                    txtCidRecebedor.Text = reader["cid_recebedor"].ToString();
                    txtUFRecebedor.Text = reader["uf_recebedor"].ToString();

                    txtCodConsignatario.Text = reader["cod_consignatario"].ToString();
                    txtConsignatario.Text = reader["consignatario"].ToString();
                    txtCidConsignatario.Text = reader["cid_consignatario"].ToString();
                    txtUFConsignatario.Text = reader["uf_consignatario"].ToString();

                    txtCodPagador.Text = reader["cod_pagador"].ToString();
                    txtPagador.Text = reader["pagador"].ToString();
                    txtCidPagador.Text = reader["cid_pagador"].ToString();
                    txtUFPagador.Text = reader["uf_pagador"].ToString();

                    txtFilial.Text = reader["nucleo"].ToString();
                    txtTipoVeiculo.Text = reader["tipo_veiculo"].ToString();
                    txtDuracao.Text = reader["tempo"].ToString();
                    txtDeslocamento.Text = reader["deslocamento"].ToString();
                    txtDistancia.Text = reader["distancia"].ToString();
                    txtPedagio.Text = reader["emitepedagio"].ToString();
                    return;

                }

            }
        }
        // Função para limpar os campos
        private void LimparCamposFrete()
        {
            //txtCodPagador.Text = string.Empty;
            //txtCidPagador.Text = string.Empty;
            //txtUFPagador.Text = string.Empty;
        }
        protected void txtNumPedido_TextChanged(object sender, EventArgs e)
        {
            string numeroCarga = novaCarga.Text.Trim();
            if (txtCodRemetente.Text != "" && txtCodDestinatario.Text != "")
            {
                if (txtNumPedido.Text != "")
                {
                    string cod = txtNumPedido.Text;
                    using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
                    {
                        conn.Open();
                        string query = "SELECT pedido, carga, status, entrega, peso, portao, material, situacao, CONVERT(varchar, previsao, 103) AS previsao, codorigem, cliorigem, coddestino, clidestino, controledocliente FROM tbpedidos WHERE pedido = @ID";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@ID", cod);
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            if (reader["status"].ToString() == "Pendente")
                            {
                                if (txtCodRemetente.Text == reader["codorigem"].ToString() && txtCodDestinatario.Text == reader["coddestino"].ToString())
                                {
                                    txtNumPedido.Text = reader["pedido"].ToString();
                                    cboMaterial.SelectedItem.Text = reader["material"].ToString();
                                    txtPeso.Text = reader["peso"].ToString();
                                    cboDeposito.SelectedItem.Text = reader["portao"].ToString();
                                    cboSituacao.SelectedItem.Text = reader["situacao"].ToString();
                                    txtPrevEntrega.Text = reader["previsao"].ToString();
                                    txtControleCliente.Text = reader["controledocliente"].ToString();
                                    cboEntrega.SelectedItem.Text = reader["entrega"].ToString();
                                    btnAdicionar.Text = "Atualizar";
                                    return;
                                }
                                else
                                {
                                    // Acione o toast quando a página for carregada
                                    string script = "<script>showToast('Pedido diferente da rota. Por favor, verifique a rota digitada.');</script>";
                                    ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                                    txtNumPedido.Text = "";
                                    txtNumPedido.Focus();
                                    return;
                                }

                            }
                            else
                            {
                                string carga = reader["carga"].ToString();
                                if (carga != numeroCarga)
                                {
                                    // Acione o toast quando a página for carregada
                                    string script = "<script>showToast('Pedido carregado ou em andamento.');</script>";
                                    ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                                    txtNumPedido.Text = "";
                                    txtNumPedido.Focus();
                                    return;
                                }
                                else
                                {
                                    // Acione o toast quando a página for carregada
                                    string script = "<script>showToast('Pedido já lançado na carga!');</script>";
                                    ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                                    txtNumPedido.Text = "";
                                    txtNumPedido.Focus();
                                    return;
                                }
                            }
                        }
                        else
                        {
                            btnAdicionar.Text = "Adicionar";
                            cboMaterial.Focus();
                            return;
                        }
                    }

                }
            }
            else
            {
                // Acione o toast quando a página for carregada
                string script = "<script>showToast('Escolha uma rota válida, para continuar...');</script>";
                ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                txtFrete.Text = "";
                txtFrete.Focus();
                return;
            }

        }
        private void CarregarGrid(string numeroCarga)
        {
            string connStr = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT carga, pedido, portao, peso, material, cliorigem, clidestino, situacao, previsao FROM tbpedidos WHERE carga = @NumeroCarga";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@NumeroCarga", numeroCarga);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        if (dt.Rows.Count > 0)
                        {
                            gvPedidos.DataSource = dt;
                            gvPedidos.DataBind();
                        }
                        else
                        {
                            gvPedidos.DataSource = null;
                            gvPedidos.DataBind();
                            // Acione o toast quando a página for carregada
                            string script = "<script>showToast('Pedido não encontrado!');</script>";
                            ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                            txtNumPedido.Text = "";
                            txtNumPedido.Focus();
                            return;
                        }
                    }
                }
            }
        }
        protected void btnAdicionar_Click(object sender, EventArgs e)
        {
            if (txtNumPedido.Text == "")
            {
                // Acione o toast quando a página for carregada
                string script = "<script>showToast('Preencha o número do pedido!');</script>";
                ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                txtNumPedido.Focus();
            }
            else
            {
                if (cbSolicitantes.SelectedItem.Value == "0")
                {
                    // Acione o toast quando a página for carregada
                    string script = "<script>showToast('Preenchimento obrigatório para o Solicitante!');</script>";
                    ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                    cbSolicitantes.Focus();
                }
                else if (cboGR.SelectedItem.Value == "0")
                {
                    // Acione o toast quando a página for carregada
                    string script = "<script>showToast('Preenchimento obrigatório para a Gerenciadora de Risco (GR)!');</script>";
                    ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                    cboGR.Focus();
                }
                else
                {
                    string cod = txtNumPedido.Text;
                    using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
                    {
                        conn.Open();
                        string query = "SELECT pedido FROM tbpedidos WHERE pedido = @ID";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@ID", cod);
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            // encontrou update
                            reader.Close();
                            string sql = @"UPDATE tbpedidos SET carga = @carga, material = @material, peso = REPLACE(@peso, ',', '.'), portao = @portao, situacao = @situacao, previsao = @previsao, entrega = @entrega, controledocliente = @controledocliente, observacao = @observacao, atualizacao = @atualizacao, gr = @gr, tomador = @tomador WHERE pedido = @pedido, lotacao=@lotacao";
                            try
                            {
                                using (SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
                                using (SqlCommand cmdUpdate = new SqlCommand(sql, con))
                                {
                                    cmdUpdate.Parameters.AddWithValue("@carga", novaCarga.Text);
                                    cmdUpdate.Parameters.AddWithValue("@material", cboMaterial.SelectedItem.Text);
                                    cmdUpdate.Parameters.AddWithValue("@peso", txtPeso.Text);
                                    cmdUpdate.Parameters.AddWithValue("@portao", cboDeposito.SelectedItem.Text);
                                    cmdUpdate.Parameters.AddWithValue("@situacao", cboSituacao.SelectedItem.Text);
                                    cmdUpdate.Parameters.AddWithValue("@previsao", DateTime.Parse(txtPrevEntrega.Text).ToString("yyyy-MM-dd"));
                                    cmdUpdate.Parameters.AddWithValue("@entrega", cboEntrega.SelectedItem.Text);
                                    cmdUpdate.Parameters.AddWithValue("@controledocliente", txtControleCliente.Text);
                                    cmdUpdate.Parameters.AddWithValue("@observacao", txtObservacao.Text.ToUpper());
                                    cmdUpdate.Parameters.AddWithValue("@solicitante", cbSolicitantes.SelectedItem.Text);
                                    cmdUpdate.Parameters.AddWithValue("@gr", cboGR.SelectedItem.Text);
                                    cmdUpdate.Parameters.AddWithValue("@atualizacao", dataHoraAtual.ToString("dd/MM/yyyy HH:mm") + " - " + nomeUsuario.ToUpper());
                                    cmdUpdate.Parameters.AddWithValue("@tomador", txtCodPagador.Text.Trim() + " - " + txtPagador.Text.Trim() + "(" + txtFrete.Text.Trim() + ")");
                                    cmdUpdate.Parameters.AddWithValue("@pedido", txtNumPedido.Text);
                                    cmdUpdate.Parameters.AddWithValue("@lotacao", txtLotacao.Text);
                                    con.Open();
                                    int rowsAffected = cmdUpdate.ExecuteNonQuery();
                                    if (rowsAffected > 0)
                                    {
                                        // Acione o toast quando a página for carregada
                                        string script = "<script>showToast('Pedido atualizado com sucesso!');</script>";
                                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                                        // atualiza  
                                        txtNumPedido.Text = "";
                                        txtNumPedido.Focus();
                                    }
                                    else
                                    {
                                        // Acione o toast quando a página for carregada
                                        string script = "<script>showToast('Erro ao atualizar o pedido.');</script>";
                                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                                    }

                                }
                            }
                            catch (Exception ex)
                            {
                                string mensagemErro = $"Erro ao atualizar: {HttpUtility.JavaScriptStringEncode(ex.Message)}";
                                string script = $"alert('{mensagemErro}');";
                                ClientScript.RegisterStartupScript(this.GetType(), "Erro", script, true);
                            }
                        }
                        else
                        {
                            conn.Close();
                            string sqlSalvarPedido = "insert into tbpedidos " + "(pedido, carga, emissao, status, solicitante, entrega, peso, material, portao, situacao, previsao, codorigem, cliorigem, coddestino, clidestino, observacao, andamento, ufcliorigem, ufclidestino, tomador, cidorigem, ciddestino, gr, cadastro, lotacao)" +
              "values" + "(@pedido, @carga, @emissao, @status, @solicitante, @entrega, @peso, @material, @portao, @situacao, @previsao, @codorigem, @cliorigem, @coddestino, @clidestino, @observacao, @andamento, @ufcliorigem, @ufclidestino, @tomador, @cidorigem, @ciddestino, @gr, @cadastro, @lotacao)";

                            SqlCommand comando = new SqlCommand(sqlSalvarPedido, conn);
                            comando.Parameters.AddWithValue("@pedido", txtNumPedido.Text);
                            comando.Parameters.AddWithValue("@carga", novaCarga.Text);
                            comando.Parameters.AddWithValue("@emissao", SafeDateTimeValue(txtCadastro.Text));
                            comando.Parameters.AddWithValue("@status", "Pendente");
                            comando.Parameters.AddWithValue("@solicitante", cbSolicitantes.SelectedItem.Text);
                            comando.Parameters.AddWithValue("@entrega", cboEntrega.SelectedItem.Text);
                            comando.Parameters.AddWithValue("@peso", txtPeso.Text);
                            comando.Parameters.AddWithValue("@material", cboMaterial.SelectedItem.Text);
                            comando.Parameters.AddWithValue("@portao", cboDeposito.SelectedItem.Text);
                            comando.Parameters.AddWithValue("@situacao", cboSituacao.SelectedItem.Text);
                            comando.Parameters.AddWithValue("@previsao", SafeDateValue(txtPrevEntrega.Text));
                            comando.Parameters.AddWithValue("@codorigem", txtCodRemetente.Text);
                            comando.Parameters.AddWithValue("@cliorigem", cboRemetente.Text);
                            comando.Parameters.AddWithValue("@coddestino", txtCodDestinatario.Text);
                            comando.Parameters.AddWithValue("@clidestino", cboDestinatario.Text);
                            comando.Parameters.AddWithValue("@observacao", txtObservacao.Text.ToUpper());
                            comando.Parameters.AddWithValue("@andamento", "PENDENTE");
                            comando.Parameters.AddWithValue("@ufcliorigem", txtUFRemetente.Text);
                            comando.Parameters.AddWithValue("@ufclidestino", txtUFDestinatario.Text);
                            comando.Parameters.AddWithValue("@tomador", txtCodPagador.Text.Trim() + " - " + txtPagador.Text.Trim() + "(" + txtFrete.Text.Trim() + ")");
                            comando.Parameters.AddWithValue("@cidorigem", txtMunicipioRemetente.Text);
                            comando.Parameters.AddWithValue("@ciddestino", txtMunicipioDestinatario.Text);
                            comando.Parameters.AddWithValue("@gr", cboGR.SelectedItem.Text);
                            comando.Parameters.AddWithValue("@cadastro", dataHoraAtual.ToString("dd/MM/yyyy HH:mm") + " - " + nomeUsuario.ToUpper());
                            comando.Parameters.AddWithValue("@lotacao", txtLotacao.Text);
                            try
                            {
                                conn.Open();
                                comando.ExecuteNonQuery();
                                conn.Close();
                                // Acione o toast quando a página for carregada
                                       string script = "<script>showToast('Pedido cadastrado com sucesso!');</script>";
                                       ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                                // atualiza  
                                        txtNumPedido.Text = "";
                                       txtNumPedido.Focus();

                            }
                            catch (Exception ex)
                            {
                                string mensagemErro = $"Erro ao cadastrar: {HttpUtility.JavaScriptStringEncode(ex.Message)}";
                                string script = $"alert('{mensagemErro}');";
                                ClientScript.RegisterStartupScript(this.GetType(), "Erro", script, true);
                            }

                            finally
                            {
                                conn.Close();
                            }
                        }
                    }
                }
                string numeroCarga = novaCarga.Text.Trim();
                CarregarGrid(numeroCarga);
            }
        }
        protected void gvPedidos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int quantidade = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "Peso"));
                totalQuantidade += quantidade;
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[1].Text = "Total:";
                e.Row.Cells[2].Text = totalQuantidade.ToString();
                e.Row.Cells[2].Font.Bold = true;

                // Armazena o total no ViewState
                ViewState["TotalPesoCarga"] = totalQuantidade;

                int totalLinhas = gvPedidos.Rows.Count;
                e.Row.Cells[4].Text = "Total de Pedidos:";
                e.Row.Cells[5].Text = totalLinhas.ToString();
                e.Row.Font.Bold = true;
            }
        }

        protected void lnkExcluir_Click(object sender, EventArgs e)
        {
            using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
            {
                string id = gvPedidos.DataKeys[row.RowIndex].Value.ToString();
                try
                {
                    string sql = "delete tbpedidos where pedido=@pedido";
                using (SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
                    using (SqlCommand cmdUpdate = new SqlCommand(sql, con))
                    {
                        cmdUpdate.Parameters.AddWithValue("@pedido", id);

                        con.Open();
                        int rowsAffected = cmdUpdate.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            // Acione o toast quando a página for carregada
                            string script = "<script>showToast('Pedido excluído com sucesso!');</script>";
                            ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                            // atualiza  
                            txtNumPedido.Text = "";
                            txtNumPedido.Focus();
                        }
                        else
                        {
                            // Acione o toast quando a página for carregada
                            string script = "<script>showToast('Erro ao excluir o pedido.');</script>";
                            ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                        }

                    }
                }
                catch (Exception ex)
                {
                    string mensagemErro = $"Erro ao atualizar: {HttpUtility.JavaScriptStringEncode(ex.Message)}";
                    string script = $"alert('{mensagemErro}');";
                    ClientScript.RegisterStartupScript(this.GetType(), "Erro", script, true);
                }

            }
           
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if (gvPedidos.Rows.Count == 0)
            {
                // Acione o toast quando a página for carregada
                string script = "<script>showToast('Nenhum pedido adicionado para salvar a carga!');</script>";
                ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                txtNumPedido.Focus();
                return;
            }
            else
            {
                if (cbSolicitantes.SelectedItem.Value == "0")
                {
                    // Acione o toast quando a página for carregada
                    string script = "<script>showToast('Preenchimento obrigatório para o Solicitante!');</script>";
                    ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                    cbSolicitantes.Focus();
                }
                else if (cboGR.SelectedItem.Value == "0")
                {
                    // Acione o toast quando a página for carregada
                    string script = "<script>showToast('Preenchimento obrigatório para a Gerenciadora de Risco (GR)!');</script>";
                    ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                    cboGR.Focus();
                }
                else
                {
                    conn.Close();
                    int totalPesoCarga = 0;
                    if (ViewState["TotalPesoCarga"] != null)
                        totalPesoCarga = Convert.ToInt32(ViewState["TotalPesoCarga"]);
                    string sqlSalvarPedido = "insert into tbcargas " + "(carga, emissao, status, tomador, entrega, peso, material, portao, situacao, previsao, codorigem, cliorigem, coddestino, clidestino, observacao, ufcliorigem, ufclidestino, pedidos, gr, ot, solicitante, empresa, andamento,cadastro, distancia, emitepedagio, cidorigem, ciddestino, nucleo, cod_expedidor, expedidor, cid_expedidor, uf_expedidor, cod_recebedor, recebedor, cid_recebedor, uf_recebedor, cod_consignatario, consignatario, cid_consignatario, uf_consignatario, cod_pagador, pagador, cid_pagador, uf_pagador, duracao, cod_tomador, tipo_veiculo, deslocamento)" +
                    "values" + "(@carga, @emissao, @status, @tomador, @entrega, @peso, @material, @portao, @situacao, @previsao, @codorigem, @cliorigem, @coddestino, @clidestino, @observacao, @ufcliorigem, @ufclidestino, @pedidos, @gr, @ot, @solicitante, @empresa, @andamento, @cadastro, @distancia, @emitepedagio, @cidorigem, @ciddestino, @nucleo, @cod_expedidor, @expedidor, @cid_expedidor, @uf_expedidor, @cod_recebedor, @recebedor, @cid_recebedor, @uf_recebedor, @cod_consignatario, @consignatario, @cid_consignatario, @uf_consignatario, @cod_pagador, @pagador, @cid_pagador, @uf_pagador, @duracao, @cod_tomador, @tipo_veiculo, @deslocamento)";

                    SqlCommand comando = new SqlCommand(sqlSalvarPedido, conn);
                    comando.Parameters.AddWithValue("@carga", novaCarga.Text);
                    comando.Parameters.AddWithValue("@emissao", SafeDateTimeValue(txtCadastro.Text));
                    comando.Parameters.AddWithValue("@status", "Pendente");
                    //comando.Parameters.AddWithValue("@tomador", txtCodPagador.Text.Trim() + " - " + txtPagador.Text.Trim() + "(" + txtFrete.Text.Trim() + ")");
                    comando.Parameters.AddWithValue("@tomador", cboFrete.SelectedItem.Text);
                    comando.Parameters.AddWithValue("@entrega", cboEntrega.SelectedItem.Text);
                    comando.Parameters.AddWithValue("@peso", totalPesoCarga);
                    comando.Parameters.AddWithValue("@material", cboMaterial.SelectedItem.Text);
                    comando.Parameters.AddWithValue("@portao", cboDeposito.SelectedItem.Text);
                    comando.Parameters.AddWithValue("@situacao", cboSituacao.SelectedItem.Text);
                    comando.Parameters.AddWithValue("@previsao", SafeDateValue(txtPrevEntrega.Text));
                    comando.Parameters.AddWithValue("@codorigem", txtCodRemetente.Text);
                    comando.Parameters.AddWithValue("@cliorigem", cboRemetente.Text);
                    comando.Parameters.AddWithValue("@coddestino", txtCodDestinatario.Text);
                    comando.Parameters.AddWithValue("@clidestino", cboDestinatario.Text);
                    comando.Parameters.AddWithValue("@observacao", txtObservacao.Text.ToUpper());
                    comando.Parameters.AddWithValue("@ufcliorigem", txtUFRemetente.Text);
                    comando.Parameters.AddWithValue("@ufclidestino", txtUFDestinatario.Text);
                    comando.Parameters.AddWithValue("@pedidos", gvPedidos.Rows.Count);
                    comando.Parameters.AddWithValue("@gr", cboGR.SelectedItem.Text);
                    comando.Parameters.AddWithValue("@ot", txtControleCliente.Text);
                    comando.Parameters.AddWithValue("@solicitante", cbSolicitantes.SelectedItem.Text);
                    comando.Parameters.AddWithValue("@empresa", "1111");
                    comando.Parameters.AddWithValue("@andamento", "PENDENTE");
                    comando.Parameters.AddWithValue("@cadastro", dataHoraAtual.ToString("dd/MM/yyyy HH:mm") + " - " + nomeUsuario.ToUpper());
                    comando.Parameters.AddWithValue("@distancia", txtDistancia.Text);
                    comando.Parameters.AddWithValue("@emitepedagio", txtPedagio.Text);
                    comando.Parameters.AddWithValue("@cidorigem", txtMunicipioRemetente.Text);
                    comando.Parameters.AddWithValue("@ciddestino", txtMunicipioDestinatario.Text);
                    comando.Parameters.AddWithValue("@nucleo", txtFilial.Text);
                    comando.Parameters.AddWithValue("@cod_expedidor", txtCodExpedidor.Text);
                    comando.Parameters.AddWithValue("@expedidor", cboExpedidor.Text);
                    comando.Parameters.AddWithValue("@cid_expedidor", txtCidExpedidor.Text);
                    comando.Parameters.AddWithValue("@uf_expedidor", txtUFExpedidor.Text);
                    comando.Parameters.AddWithValue("@cod_recebedor", txtCodRecebedor.Text);
                    comando.Parameters.AddWithValue("@recebedor", cboRecebedor.Text);
                    comando.Parameters.AddWithValue("@cid_recebedor", txtCidRecebedor.Text);
                    comando.Parameters.AddWithValue("@uf_recebedor", txtUFRecebedor.Text);
                    comando.Parameters.AddWithValue("@cod_consignatario", txtCodConsignatario.Text);
                    comando.Parameters.AddWithValue("@consignatario", txtConsignatario.Text);
                    comando.Parameters.AddWithValue("@cid_consignatario", txtCidConsignatario.Text);
                    comando.Parameters.AddWithValue("@uf_consignatario", txtUFConsignatario.Text);
                    comando.Parameters.AddWithValue("@cod_pagador", txtCodPagador.Text);
                    comando.Parameters.AddWithValue("@pagador", txtPagador.Text);
                    comando.Parameters.AddWithValue("@cid_pagador", txtCidPagador.Text);
                    comando.Parameters.AddWithValue("@uf_pagador", txtUFPagador.Text);
                    comando.Parameters.AddWithValue("@duracao", txtDuracao.Text);
                    comando.Parameters.AddWithValue("@cod_tomador", txtFrete.Text);
                    comando.Parameters.AddWithValue("@tipo_veiculo", txtTipoVeiculo.Text);
                    comando.Parameters.AddWithValue("@deslocamento", txtDeslocamento.Text);

                    try
                    {
                        conn.Open();
                        comando.ExecuteNonQuery();
                        conn.Close();
                        // Acione o toast quando a página for carregada
                        string script = "<script>showToast('Carga cadastrado com sucesso!');</script>";
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                        // atualiza  
                        Response.Redirect("/dist/pages/GestaoDeCargasMatriz.aspx");                     
                    }
                    catch (Exception ex)
                    {
                        string mensagemErro = $"Erro ao cadastrar: {HttpUtility.JavaScriptStringEncode(ex.Message)}";
                        string script = $"alert('{mensagemErro}');";
                        ClientScript.RegisterStartupScript(this.GetType(), "Erro", script, true);
                    }

                    finally
                    {
                        conn.Close();
                    }


                }
            }
        }

        private object SafeDateTimeValue(string input)
        {
            DateTime dt;
            if (DateTime.TryParse(input, out dt))
                return dt.ToString("yyyy-MM-dd HH:mm:ss");
            else
                return DBNull.Value;
        }
        private object SafeDateValue(string input)
        {
            DateTime dt;
            if (DateTime.TryParse(input, out dt))
                return dt.ToString("yyyy-MM-dd");
            else
                return DBNull.Value;
        }
        protected void txtPrevEntrega_TextChanged(object sender, EventArgs e)
        {
            if (txtPrevEntrega.Text != "")
            {
                DateTime data;
                if (DataValida(txtPrevEntrega.Text, out data))
                {
                    //lblMensagem.Text = "Data válida: " + data.ToString("dd/MM/yyyy");
                    //lblMensagem.ForeColor = System.Drawing.Color.Green;
                }
                else
                {
                    // Acione o toast quando a página for carregada
                    string script = "<script>showToast('Data inválida!');</script>";
                    ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                    txtPrevEntrega.Focus();
                }

            }
           
        }
        public static bool DataValida(string dataTexto, out DateTime data)
        {
            string formato = "dd/MM/yyyy";
            return DateTime.TryParseExact(
                dataTexto,
                formato,
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out data
            );
        }
    }
}