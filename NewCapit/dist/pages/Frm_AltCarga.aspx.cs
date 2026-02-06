using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using static NewCapit.Main;
using static NewCapit.dist.pages.Frm_TabelaPrecoMatriz;

namespace NewCapit.dist.pages
{
    public partial class Frm_AltCarga : System.Web.UI.Page
    {
        SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString());
        string nomeUsuario = string.Empty;
        DateTime dataHoraAtual = DateTime.Now;
        int totalQuantidade = 0;
        //int totalLinhas = 0;
        string totalPesoCarga;
        string id;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UsuarioLogado"] != null)
                {
                    string nomeUsuario = Session["UsuarioLogado"].ToString();
                    var lblUsuario = nomeUsuario;
                    //txtUsuCadastro.Text = nomeUsuario;
                    //txtUsuAlteracao.Text = DateTime.Now.ToString();
                }
                else
                {
                    var lblUsuario = "<Usuário>";
                    Response.Redirect("Login.aspx");

                }
                //PreencherNumCarga();
                CarregarClientes(cboRemetente, cboExpedidor, cboDestinatario, cboRecebedor, txtConsignatario, txtPagador);
                PreencherComboSolicitantes();                
                PreencherComboMateriais();
                PreencherComboDeposito();

                CarregaDados();
            }
        }
        private void CarregarClientes(params DropDownList[] combos)
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT codcli, razcli, ativo_inativo, fl_exclusao FROM tbclientes where ativo_inativo = 'ATIVO' and fl_exclusao is null ORDER BY razcli";

            // Crie uma conexão com o banco de dados
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                DataTable dt = new DataTable();
                dt.Load(dr);

                foreach (DropDownList ddl in combos)
                {
                    ddl.DataSource = dt;
                    ddl.DataTextField = "razcli";   // texto exibido
                    ddl.DataValueField = "codcli";  // valor
                    ddl.DataBind();

                    ddl.Items.Insert(0, new ListItem("Selecione...", ""));
                }
            }



        }
        public void CarregaDados()
        {
            if (HttpContext.Current.Request.QueryString["id"].ToString() != "")
            {
                id = HttpContext.Current.Request.QueryString["id"].ToString();
            }

            string sql = "SELECT * from tbcargas where id = " + id;
            SqlDataAdapter adpt = new SqlDataAdapter(sql, conn);
            DataTable dt = new DataTable();
            conn.Open();
            adpt.Fill(dt);
            conn.Close();

            if (dt.Rows.Count > 0)
            {
                // Preenchendo os TextBoxes com valores do DataTable
                txtID.Text = dt.Rows[0][0].ToString();
                txtID.BackColor = System.Drawing.Color.Purple;
                txtID.ForeColor = System.Drawing.Color.White;
                novaCarga.Text = dt.Rows[0][1].ToString();
                txtCadastro.Text = DateTime.Parse(dt.Rows[0][2].ToString()).ToString("dd/MM/yyyy HH:mm");

                txtCodRemetente.Text = dt.Rows[0][12].ToString();
                cboRemetente.SelectedItem.Text = dt.Rows[0][13].ToString();
                txtCNPJRemetente.Text = dt.Rows[0][117].ToString();
                txtMunicipioRemetente.Text = dt.Rows[0][23].ToString();
                txtUFRemetente.Text = dt.Rows[0][19].ToString();

                txtCodExpedidor.Text = dt.Rows[0][64].ToString();
                cboExpedidor.SelectedItem.Text = dt.Rows[0][65].ToString();
                txtCNPJExpedidor.Text = dt.Rows[0][118].ToString();
                txtCidExpedidor.Text = dt.Rows[0][66].ToString();
                txtUFExpedidor.Text = dt.Rows[0][67].ToString();

                txtCodDestinatario.Text = dt.Rows[0][14].ToString();
                cboDestinatario.SelectedItem.Text = dt.Rows[0][15].ToString();
                txtCNPJDestinatario.Text = dt.Rows[0][119].ToString();
                txtMunicipioDestinatario.Text = dt.Rows[0][24].ToString();
                txtUFDestinatario.Text = dt.Rows[0][20].ToString();
                txtUsuCadastro.Text = dt.Rows[0][26].ToString();

                txtCodRecebedor.Text = dt.Rows[0][68].ToString();
                cboRecebedor.SelectedItem.Text = dt.Rows[0][69].ToString();
                txtCNPJRecebedor.Text = dt.Rows[0][120].ToString();
                txtCidRecebedor.Text = dt.Rows[0][70].ToString();
                txtUFRecebedor.Text = dt.Rows[0][71].ToString();

                txtCodConsignatario.Text = dt.Rows[0][72].ToString();
                txtConsignatario.SelectedItem.Text = dt.Rows[0][73].ToString();
                txtCNPJConsignatario.Text = dt.Rows[0][121].ToString();
                txtCidConsignatario.Text = dt.Rows[0][74].ToString();
                txtUFConsignatario.Text = dt.Rows[0][75].ToString();

                txtCodPagador.Text = dt.Rows[0][76].ToString();
                txtPagador.SelectedItem.Text = dt.Rows[0][77].ToString();
                txtCNPJPagador.Text = dt.Rows[0][122].ToString();
                txtCidPagador.Text = dt.Rows[0][78].ToString();
                txtUFPagador.Text = dt.Rows[0][79].ToString();

                cbSolicitantes.Items.Insert(0, new ListItem(dt.Rows[0][30].ToString(), ""));
                cboGR.Text =dt.Rows[0][28].ToString();
                //cboFrete.Items.Insert(0, new ListItem(dt.Rows[0][4].ToString(), ""));

                txtFilial.Text = dt.Rows[0][80].ToString();
                txtTipoVeiculo.Text = dt.Rows[0][83].ToString();
                txtDistancia.Text = dt.Rows[0][62].ToString();
                txtDeslocamento.Text = dt.Rows[0][84].ToString();
                txtDuracao.Text = dt.Rows[0][81].ToString();
                txtPedagio.Text = dt.Rows[0][63].ToString();
                txtObservacao.Text = dt.Rows[0][16].ToString();
                txtRota.Text = dt.Rows[0][123].ToString();



                //txtFrete.Text = dt.Rows[0][82].ToString();
                if (novaCarga.Text != "")
                {
                    string numeroCarga = novaCarga.Text.Trim();
                    CarregarGrid(numeroCarga);
                }
            }
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
            string query = "SELECT id, nome FROM tbsolicitantes where status = 'Ativo' order by nome";

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

        private void PreencherComboMateriais()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT id, descricao FROM tbtipomaterial";

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
                    cboMaterial.DataValueField = "id";  // Campo que será o valor de cada item                    
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
        //private void PreencherComboFretes()
        //{
        //    // Consulta SQL que retorna os dados desejados
        //    string query = "SELECT cod_frete, desc_frete, fl_exclusao FROM tbtabeladefretes where fl_exclusao is null  and situacao = 'ATIVO' order by desc_frete";

        //    // Crie uma conexão com o banco de dados
        //    using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
        //    {
        //        try
        //        {
        //            // Abra a conexão com o banco de dados
        //            conn.Open();

        //            // Crie o comando SQL
        //            SqlCommand cmd = new SqlCommand(query, conn);

        //            // Execute o comando e obtenha os dados em um DataReader
        //            SqlDataReader reader = cmd.ExecuteReader();

        //            // Preencher o ComboBox com os dados do DataReader
        //            cboFrete.DataSource = reader;
        //            cboFrete.DataTextField = "desc_frete";  // Campo que será mostrado no ComboBox
        //            cboFrete.DataValueField = "cod_frete";  // Campo que será o valor de cada item                    
        //            cboFrete.DataBind();  // Realiza o binding dos dados                   
        //            cboFrete.Items.Insert(0, new ListItem("Selecione...", "0"));
        //            // Feche o reader
        //            reader.Close();
        //        }
        //        catch (Exception ex)
        //        {
        //            // Trate exceções
        //            Response.Write("Erro: " + ex.Message);
        //        }
        //    }
        //}
        //protected void txtFrete_TextChanged(object sender, EventArgs e)
        //{
        //    if (txtFrete.Text != "")
        //    {
        //        string cod = txtFrete.Text;
        //        string sql = "SELECT  cod_frete, desc_frete, cod_remetente, remetente, cid_remetente, uf_remetente, cod_expedidor, expedidor, cid_expedidor, uf_expedidor, cod_destinatario, destinatario, cid_destinatario, uf_destinatario,cod_recebedor, recebedor, cid_recebedor, uf_recebedor, cod_consignatario, consignatario, cid_consignatario, uf_consignatario, cod_pagador, pagador, cid_pagador, uf_pagador, nucleo, tipo_veiculo,  tempo, deslocamento, distancia, emitepedagio, fl_exclusao, situacao FROM tbtabeladefretes where cod_frete = '" + cod + "' and situacao = 'ATIVO' and fl_exclusao is null";
        //        SqlDataAdapter da = new SqlDataAdapter(sql, conn);
        //        DataTable dt = new DataTable();
        //        conn.Open();
        //        da.Fill(dt);
        //        conn.Close();

        //        if (dt.Rows.Count > 0)
        //        {
        //            if (dt.Rows[0][17].ToString() == null)
        //            {
        //                // Acione o toast quando a página for carregada
        //                string script = "<script>showToast('Tabela de frete deletada do sistema.');</script>";
        //                ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
        //                txtFrete.Text = "";
        //                txtFrete.Focus();
        //                return;
        //            }
        //            else if (dt.Rows[0][18].ToString() == "INATIVO")
        //            {
        //                // Acione o toast quando a página for carregada
        //                string script = "<script>showToast('Tabela de frete inativa no sistema.');</script>";
        //                ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
        //                txtFrete.Text = "";
        //                txtFrete.Focus();
        //                return;
        //            }
        //            else
        //            {
        //                txtFrete.Text = dt.Rows[0][0].ToString();
        //                cboFrete.SelectedItem.Text = dt.Rows[0][1].ToString();
        //                txtCodRemetente.Text = dt.Rows[0][2].ToString();
        //                cboRemetente.Text = dt.Rows[0][3].ToString();
        //                txtMunicipioRemetente.Text = dt.Rows[0][4].ToString();
        //                txtUFRemetente.Text = dt.Rows[0][5].ToString();

        //                txtCodExpedidor.Text = dt.Rows[0][6].ToString();
        //                cboExpedidor.Text = dt.Rows[0][7].ToString();
        //                txtCidExpedidor.Text = dt.Rows[0][8].ToString();
        //                txtUFExpedidor.Text = dt.Rows[0][9].ToString();

        //                txtCodDestinatario.Text = dt.Rows[0][10].ToString();
        //                cboDestinatario.Text = dt.Rows[0][11].ToString();
        //                txtMunicipioDestinatario.Text = dt.Rows[0][12].ToString();
        //                txtUFDestinatario.Text = dt.Rows[0][13].ToString();

        //                txtCodRecebedor.Text = dt.Rows[0][14].ToString();
        //                cboRecebedor.Text = dt.Rows[0][15].ToString();
        //                txtCidRecebedor.Text = dt.Rows[0][16].ToString();
        //                txtUFRecebedor.Text = dt.Rows[0][17].ToString();

        //                txtCodConsignatario.Text = dt.Rows[0][18].ToString();
        //                txtConsignatario.Text = dt.Rows[0][19].ToString();
        //                txtCidConsignatario.Text = dt.Rows[0][20].ToString();
        //                txtUFConsignatario.Text = dt.Rows[0][21].ToString();

        //                txtCodPagador.Text = dt.Rows[0][22].ToString();
        //                txtPagador.Text = dt.Rows[0][23].ToString();
        //                txtCidPagador.Text = dt.Rows[0][24].ToString();
        //                txtUFPagador.Text = dt.Rows[0][25].ToString();

        //                txtFilial.Text = dt.Rows[0][26].ToString();
        //                txtTipoVeiculo.Text = dt.Rows[0][27].ToString();
        //                txtDuracao.Text = dt.Rows[0][28].ToString();
        //                txtDeslocamento.Text = dt.Rows[0][29].ToString();
        //                txtDistancia.Text = dt.Rows[0][30].ToString();
        //                txtPedagio.Text = dt.Rows[0][31].ToString();
        //                return;
        //            }

        //        }
        //        else
        //        {
        //            // Acione o toast quando a página for carregada
        //            string script = "<script>showToast('Tabela de frete não encontrada no sistema.');</script>";
        //            ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
        //            txtFrete.Text = "";
        //            txtFrete.Focus();
        //            return;
        //        }
        //    }

        //}
        //protected void cboFrete_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    int idSelecionado = int.Parse(cboFrete.SelectedValue);

        //    // Preencher os campos com base no valor selecionado
        //    if (idSelecionado > 0)
        //    {
        //        PreencherCamposFrete(idSelecionado);
        //    }
        //    else
        //    {
        //        LimparCamposFrete();
        //    }
        //}
        //// Função para preencher os campos com os dados do banco
        //private void PreencherCamposFrete(int id)
        //{
        //    using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
        //    {
        //        conn.Open();
        //        string query = "SELECT cod_frete, desc_frete, cod_remetente, remetente, cid_remetente, uf_remetente, cod_expedidor, expedidor, cid_expedidor, uf_expedidor, cod_destinatario, destinatario, cid_destinatario, uf_destinatario,cod_recebedor, recebedor, cid_recebedor, uf_recebedor, cod_consignatario, consignatario, cid_consignatario, uf_consignatario, cod_pagador, pagador, cid_pagador, uf_pagador, nucleo, tipo_veiculo,  tempo, deslocamento, distancia, emitepedagio, fl_exclusao, situacao FROM tbtabeladefretes WHERE cod_frete = @ID";
        //        SqlCommand cmd = new SqlCommand(query, conn);
        //        cmd.Parameters.AddWithValue("@ID", id);
        //        SqlDataReader reader = cmd.ExecuteReader();

        //        if (reader.Read())
        //        {
        //            txtFrete.Text = reader["cod_frete"].ToString();
        //            cboFrete.SelectedItem.Text = reader["desc_frete"].ToString();
        //            txtCodRemetente.Text = reader["cod_remetente"].ToString();
        //            cboRemetente.Text = reader["remetente"].ToString();
        //            txtMunicipioRemetente.Text = reader["cid_remetente"].ToString();
        //            txtUFRemetente.Text = reader["uf_remetente"].ToString();

        //            txtCodExpedidor.Text = reader["cod_expedidor"].ToString();
        //            cboExpedidor.Text = reader["expedidor"].ToString();
        //            txtCidExpedidor.Text = reader["cid_expedidor"].ToString();
        //            txtUFExpedidor.Text = reader["uf_expedidor"].ToString();

        //            txtCodDestinatario.Text = reader["cod_destinatario"].ToString();
        //            cboDestinatario.Text = reader["destinatario"].ToString();
        //            txtMunicipioDestinatario.Text = reader["cid_destinatario"].ToString();
        //            txtUFDestinatario.Text = reader["uf_destinatario"].ToString();

        //            txtCodRecebedor.Text = reader["cod_recebedor"].ToString();
        //            cboRecebedor.Text = reader["recebedor"].ToString();
        //            txtCidRecebedor.Text = reader["cid_recebedor"].ToString();
        //            txtUFRecebedor.Text = reader["uf_recebedor"].ToString();

        //            txtCodConsignatario.Text = reader["cod_consignatario"].ToString();
        //            txtConsignatario.Text = reader["consignatario"].ToString();
        //            txtCidConsignatario.Text = reader["cid_consignatario"].ToString();
        //            txtUFConsignatario.Text = reader["uf_consignatario"].ToString();

        //            txtCodPagador.Text = reader["cod_pagador"].ToString();
        //            txtPagador.Text = reader["pagador"].ToString();
        //            txtCidPagador.Text = reader["cid_pagador"].ToString();
        //            txtUFPagador.Text = reader["uf_pagador"].ToString();

        //            txtFilial.Text = reader["nucleo"].ToString();
        //            txtTipoVeiculo.Text = reader["tipo_veiculo"].ToString();
        //            txtDuracao.Text = reader["tempo"].ToString();
        //            txtDeslocamento.Text = reader["deslocamento"].ToString();
        //            txtDistancia.Text = reader["distancia"].ToString();
        //            txtPedagio.Text = reader["emitepedagio"].ToString();
        //            return;

        //        }

        //    }
        //}
        //// Função para limpar os campos
        //private void LimparCamposFrete()
        //{
        //    //txtCodPagador.Text = string.Empty;
        //    //txtCidPagador.Text = string.Empty;
        //    //txtUFPagador.Text = string.Empty;
        //}
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
                            string sql = @"UPDATE tbpedidos SET carga = @carga, material = @material, peso = @peso, portao = @portao, situacao = @situacao, previsao = @previsao, entrega = @entrega, controledocliente = @controledocliente, observacao = @observacao, atualizacao = @atualizacao, gr = @gr, tomador = @tomador, solicitante=@solicitante WHERE pedido = @pedido";
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
                                    cmdUpdate.Parameters.AddWithValue("@gr", cboGR.Text);
                                    cmdUpdate.Parameters.AddWithValue("@atualizacao", dataHoraAtual.ToString("dd/MM/yyyy HH:mm") + " - " + nomeUsuario.ToUpper());
                                    cmdUpdate.Parameters.AddWithValue("@tomador", txtPagador.SelectedItem.Text);
                                    cmdUpdate.Parameters.AddWithValue("@pedido", txtNumPedido.Text);
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
                            string sqlSalvarPedido = "insert into tbpedidos " + "(pedido, carga, emissao, status, solicitante, entrega, peso, material, portao, situacao, previsao, codorigem, cliorigem, coddestino, clidestino, observacao, andamento, ufcliorigem, ufclidestino, tomador, cidorigem, ciddestino, gr, cadastro)" +
              "values" + "(@pedido, @carga, @emissao, @status, @solicitante, @entrega, @peso, @material, @portao, @situacao, @previsao, @codorigem, @cliorigem, @coddestino, @clidestino, @observacao, @andamento, @ufcliorigem, @ufclidestino, @tomador, @cidorigem, @ciddestino, @gr, @cadastro)";

                            SqlCommand comando = new SqlCommand(sqlSalvarPedido, conn);
                            comando.Parameters.AddWithValue("@pedido", txtNumPedido.Text);
                            comando.Parameters.AddWithValue("@carga", novaCarga.Text);
                            comando.Parameters.AddWithValue("@emissao", txtCadastro.Text);
                            comando.Parameters.AddWithValue("@status", "Pendente");
                            comando.Parameters.AddWithValue("@solicitante", cboSituacao.SelectedItem.Text);
                            comando.Parameters.AddWithValue("@entrega", cboEntrega.SelectedItem.Text);
                            comando.Parameters.AddWithValue("@peso", txtPeso.Text);
                            comando.Parameters.AddWithValue("@material", cboMaterial.SelectedItem.Text);
                            comando.Parameters.AddWithValue("@portao", cboDeposito.SelectedItem.Text);
                            comando.Parameters.AddWithValue("@situacao", cboSituacao.SelectedItem.Text);
                            comando.Parameters.AddWithValue("@previsao", DateTime.Parse(txtPrevEntrega.Text).ToString("yyyy-MM-dd"));
                            comando.Parameters.AddWithValue("@codorigem", txtCodRemetente.Text);
                            comando.Parameters.AddWithValue("@cliorigem", cboRemetente.Text);
                            comando.Parameters.AddWithValue("@coddestino", txtCodDestinatario.Text);
                            comando.Parameters.AddWithValue("@clidestino", cboDestinatario.Text);
                            comando.Parameters.AddWithValue("@observacao", txtObservacao.Text.ToUpper());
                            comando.Parameters.AddWithValue("@andamento", "PENDENTE");
                            comando.Parameters.AddWithValue("@ufcliorigem", txtUFRemetente.Text);
                            comando.Parameters.AddWithValue("@ufclidestino", txtUFDestinatario.Text);
                            comando.Parameters.AddWithValue("@tomador", txtPagador.SelectedItem.Text);
                            comando.Parameters.AddWithValue("@cidorigem", txtMunicipioRemetente.Text);
                            comando.Parameters.AddWithValue("@ciddestino", txtMunicipioDestinatario.Text);
                            comando.Parameters.AddWithValue("@gr", cboGR.Text);
                            comando.Parameters.AddWithValue("@cadastro", dataHoraAtual.ToString("dd/MM/yyyy HH:mm") + " - " + nomeUsuario.ToUpper());

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
                else
                {
                    conn.Close();
                    decimal totalPesoCarga = 0;
                    if (ViewState["TotalPesoCarga"] != null)
                        totalPesoCarga = Convert.ToDecimal(ViewState["TotalPesoCarga"]);
                    string sqlSalvarCarga = @"
                    UPDATE tbcargas SET
                        carga = @carga,
                        tomador = @tomador,
                        entrega = @entrega,
                        peso = @peso,
                        material = @material,
                        portao = @portao,
                        situacao = @situacao,
                        codorigem = @codorigem,
                        cliorigem = @cliorigem,
                        coddestino = @coddestino,
                        clidestino = @clidestino,
                        observacao = @observacao,
                        ufcliorigem = @ufcliorigem,
                        ufclidestino = @ufclidestino,
                        pedidos = @pedidos,
                        gr = @gr,
                        ot = @ot,
                        solicitante = @solicitante,
                        empresa = @empresa,
                        andamento = @andamento,
                        distancia = @distancia,
                        emitepedagio = @emitepedagio,
                        cidorigem = @cidorigem,
                        ciddestino = @ciddestino,
                        nucleo = @nucleo,
                        cod_expedidor = @cod_expedidor,
                        expedidor = @expedidor,
                        cid_expedidor = @cid_expedidor,
                        uf_expedidor = @uf_expedidor,
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
                        duracao = @duracao,
                        cod_tomador = @cod_tomador,
                        tipo_veiculo = @tipo_veiculo,
                        deslocamento = @deslocamento,
                        atualizacao = @atualizacao
                    WHERE id = @id";


                    try
                    {
                        using (SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
                        using (SqlCommand cmd = new SqlCommand(sqlSalvarCarga, con))
                        {
                            cmd.Parameters.Add("@id", SqlDbType.Int).Value = SafeConvert.SafeInt(txtID.Text);

                            cmd.Parameters.Add("@carga", SqlDbType.NVarChar, 50).Value = novaCarga.Text;
                            cmd.Parameters.Add("@tomador", SqlDbType.NVarChar, 170).Value =
                                txtPagador.SelectedItem.Text;

                            cmd.Parameters.Add("@entrega", SqlDbType.NVarChar, 20).Value = cboEntrega.SelectedItem.Text;
                            cmd.Parameters.Add("@peso", SqlDbType.Decimal).Value = totalPesoCarga;
                            cmd.Parameters.Add("@material", SqlDbType.NVarChar, 50).Value = cboMaterial.SelectedItem.Text;
                            cmd.Parameters.Add("@portao", SqlDbType.NVarChar, 20).Value = cboDeposito.SelectedItem.Text;
                            cmd.Parameters.Add("@situacao", SqlDbType.NVarChar, 20).Value = cboSituacao.SelectedItem.Text;

                            cmd.Parameters.Add("@codorigem", SqlDbType.Int).Value = SafeConvert.SafeInt(txtCodRemetente.Text);
                            cmd.Parameters.Add("@cliorigem", SqlDbType.NVarChar, 150).Value = cboRemetente.SelectedItem.Text;

                            cmd.Parameters.Add("@coddestino", SqlDbType.Int).Value = SafeConvert.SafeInt(txtCodDestinatario.Text);
                            cmd.Parameters.Add("@clidestino", SqlDbType.NVarChar, 150).Value = cboDestinatario.SelectedItem.Text;

                            cmd.Parameters.Add("@observacao", SqlDbType.NVarChar).Value = txtObservacao.Text.ToUpper();
                            cmd.Parameters.Add("@ufcliorigem", SqlDbType.NVarChar, 2).Value = txtUFRemetente.Text;
                            cmd.Parameters.Add("@ufclidestino", SqlDbType.NVarChar, 2).Value = txtUFDestinatario.Text;

                            cmd.Parameters.Add("@pedidos", SqlDbType.NVarChar).Value = gvPedidos.Rows.Count.ToString();
                            cmd.Parameters.Add("@gr", SqlDbType.NVarChar, 50).Value = cboGR.Text;
                            cmd.Parameters.Add("@ot", SqlDbType.NVarChar, 50).Value = txtControleCliente.Text;
                            cmd.Parameters.Add("@solicitante", SqlDbType.NVarChar, 50).Value = cbSolicitantes.SelectedItem.Text;
                            cmd.Parameters.Add("@empresa", SqlDbType.NVarChar, 50).Value = "1111";
                            cmd.Parameters.Add("@andamento", SqlDbType.NVarChar, 50).Value = "PENDENTE";

                            cmd.Parameters.Add("@distancia", SqlDbType.Decimal).Value =
                                SafeConvert.SafeDecimal(txtDistancia.Text);

                            cmd.Parameters.Add("@emitepedagio", SqlDbType.NChar, 3).Value = txtPedagio.Text;
                            cmd.Parameters.Add("@cidorigem", SqlDbType.NVarChar, 50).Value = txtMunicipioRemetente.Text;
                            cmd.Parameters.Add("@ciddestino", SqlDbType.NVarChar, 50).Value = txtMunicipioDestinatario.Text;
                            cmd.Parameters.Add("@nucleo", SqlDbType.NVarChar, 50).Value = txtFilial.Text;

                            cmd.Parameters.Add("@cod_expedidor", SqlDbType.Int).Value = SafeConvert.SafeInt(txtCodExpedidor.Text);
                            cmd.Parameters.Add("@expedidor", SqlDbType.NVarChar, 150).Value = cboExpedidor.SelectedItem.Text;
                            cmd.Parameters.Add("@cid_expedidor", SqlDbType.NVarChar, 50).Value = txtCidExpedidor.Text;
                            cmd.Parameters.Add("@uf_expedidor", SqlDbType.NVarChar, 2).Value = txtUFExpedidor.Text;
                            cmd.Parameters.Add("@cod_recebedor", SqlDbType.Int).Value = SafeConvert.SafeInt(txtCodRecebedor.Text);
                            cmd.Parameters.Add("@recebedor", SqlDbType.NVarChar, 150).Value = cboRecebedor.SelectedItem.Text;
                            cmd.Parameters.Add("@cid_recebedor", SqlDbType.NVarChar, 50).Value = txtCidRecebedor.Text;
                            cmd.Parameters.Add("@uf_recebedor", SqlDbType.NVarChar, 2).Value = txtUFRecebedor.Text;
                            cmd.Parameters.Add("@cod_consignatario", SqlDbType.Int).Value =
                                SafeConvert.SafeIntNullable(txtCodConsignatario.Text) ?? (object)DBNull.Value;
                            cmd.Parameters.Add("@consignatario", SqlDbType.NVarChar, 150).Value = txtConsignatario.SelectedItem.Text;
                            cmd.Parameters.Add("@cid_consignatario", SqlDbType.NVarChar, 50).Value = txtCidConsignatario.Text;
                            cmd.Parameters.Add("@uf_consignatario", SqlDbType.NVarChar, 2).Value = txtUFConsignatario.Text;
                            cmd.Parameters.Add("@cod_pagador", SqlDbType.Int).Value = SafeConvert.SafeInt(txtCodPagador.Text);
                            cmd.Parameters.Add("@pagador", SqlDbType.NVarChar, 150).Value = txtPagador.SelectedItem.Text;
                            cmd.Parameters.Add("@cid_pagador", SqlDbType.NVarChar, 50).Value = txtCidPagador.Text;
                            cmd.Parameters.Add("@uf_pagador", SqlDbType.NVarChar, 2).Value = txtUFPagador.Text;
                            cmd.Parameters.Add("@duracao", SqlDbType.NVarChar, 15).Value = txtDuracao.Text;                            
                            cmd.Parameters.Add("@tipo_veiculo", SqlDbType.NVarChar, 50).Value = txtTipoVeiculo.Text;
                            cmd.Parameters.Add("@deslocamento", SqlDbType.NVarChar, 30).Value = txtDeslocamento.Text;

                            cmd.Parameters.Add("@atualizacao", SqlDbType.NVarChar, 80).Value =
                                $"{DateTime.Now:dd/MM/yyyy HH:mm} - {nomeUsuario.ToUpper()}";

                            con.Open();
                            cmd.ExecuteNonQuery();
                            Response.Redirect("/dist/pages/GestaoDeCargasMatriz.aspx", false);
                            Context.ApplicationInstance.CompleteRequest();
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
        public RotaEntrega ObterRotaPorDescricao(string nomeRota)
        {
            const string sql = @"
            SELECT 
                rota,
                desc_rota,
                distancia,
                tempo,
                deslocamento,
                pedagio
            FROM tbrotasdeentregas
            WHERE desc_rota COLLATE Latin1_General_CI_AI LIKE '%' + @nomeRota + '%'
        ";

            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add("@nomeRota", SqlDbType.VarChar).Value = nomeRota;
                conn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (!dr.Read())
                        return null;

                    return new RotaEntrega
                    {
                        Rota = DbSafe.ToString(dr["rota"]),
                        Descricao = DbSafe.ToString(dr["desc_rota"]),
                        Distancia = DbSafe.ToDecimal(dr["distancia"]),
                        Tempo = DbSafe.ToTimeSpan(dr["tempo"]),
                        EmitePedagio = DbSafe.ToString(dr["pedagio"]),
                        Percurso = DbSafe.ToString(dr["deslocamento"])
                    };
                }
            }
        }
        private void PreencherCamposRota(RotaEntrega rota)
        {
            // txtRota.Text = rota.Rota;           
            txtDistancia.Text = rota.Distancia.ToString("N2");
            txtDuracao.Text = rota.Tempo.ToString(@"hh\:mm");
            txtRota.Text = rota.Rota + " - " + rota.Descricao;
            txtDeslocamento.Text = rota.Percurso;
            txtPedagio.Text = rota.EmitePedagio;
        }
        protected void BuscarRota(string nomeRota)
        {
            var rota = ObterRotaPorDescricao(nomeRota);

            if (rota == null)
            {
                // LimparCamposRota();
                return;
            }

            PreencherCamposRota(rota);
        }

        protected void txtCodRemetente_TextChanged(object sender, EventArgs e)
        {
            if (txtCodRemetente.Text != "")
            {
                string cod = txtCodRemetente.Text;
                string sql = "SELECT  codcli, razcli, cnpj, cidcli, estcli, ativo_inativo, fl_exclusao, codvw, codsapiens FROM tbclientes where codcli = '" + cod + "' OR codvw = '" + cod + "' OR codsapiens = '" + cod + "' and ativo_inativo = 'ATIVO' and fl_exclusao is null";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                conn.Open();
                da.Fill(dt);
                conn.Close();

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0][6].ToString() == null)
                    {
                        // Acione o toast quando a página for carregada
                        string script = "<script>showToast(' - Remetente deletado do sistema.');</script>";
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                        txtCodRemetente.Text = "";
                        txtCodRemetente.Focus();
                        return;
                    }
                    else if (dt.Rows[0][5].ToString() == "INATIVO")
                    {
                        // Acione o toast quando a página for carregada
                        string script = "<script>showToast(' - Remetente inativo no sistema.');</script>";
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                        txtCodRemetente.Text = "";
                        txtCodRemetente.Focus();
                        return;
                    }
                    else
                    {
                        txtCodRemetente.Text = dt.Rows[0][0].ToString();
                        cboRemetente.SelectedItem.Text = dt.Rows[0][1].ToString();
                        txtCNPJRemetente.Text = dt.Rows[0][2].ToString();
                        txtMunicipioRemetente.Text = dt.Rows[0][3].ToString();
                        txtUFRemetente.Text = dt.Rows[0][4].ToString();
                        txtCodExpedidor.Focus();
                        return;
                    }

                }
                else
                {
                    // Acione o toast quando a página for carregada
                    string script = "<script>showToast(' - Remetente não encontrado no sistema.');</script>";
                    ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                    txtCodRemetente.Text = "";
                    txtCodRemetente.Focus();
                    return;
                }
            }

        }
        protected void txtCodExpedidor_TextChanged(object sender, EventArgs e)
        {
            if (txtCodExpedidor.Text != "")
            {
                string cod = txtCodExpedidor.Text;
                string sql = "SELECT  codcli, razcli, cnpj, cidcli, estcli, ativo_inativo, fl_exclusao, codvw, codsapiens FROM tbclientes where codcli = '" + cod + "' OR codvw = '" + cod + "' OR codsapiens = '" + cod + "' and ativo_inativo = 'ATIVO' and fl_exclusao is null";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                conn.Open();
                da.Fill(dt);
                conn.Close();

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0][6].ToString() == null)
                    {
                        // Acione o toast quando a página for carregada
                        string script = "<script>showToast(' - Expedidor deletado do sistema.');</script>";
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                        txtCodExpedidor.Text = "";
                        txtCodExpedidor.Focus();
                        return;
                    }
                    else if (dt.Rows[0][5].ToString() == "INATIVO")
                    {
                        // Acione o toast quando a página for carregada
                        string script = "<script>showToast(' - Expedidor inativo no sistema.');</script>";
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                        txtCodExpedidor.Text = "";
                        txtCodExpedidor.Focus();
                        return;
                    }
                    else
                    {
                        txtCodExpedidor.Text = dt.Rows[0][0].ToString();
                        cboExpedidor.SelectedItem.Text = dt.Rows[0][1].ToString();
                        txtCNPJExpedidor.Text = dt.Rows[0][2].ToString();
                        txtCidExpedidor.Text = dt.Rows[0][3].ToString();
                        txtUFExpedidor.Text = dt.Rows[0][4].ToString();
                        txtCodDestinatario.Focus();
                        return;
                    }

                }
                else
                {
                    // Acione o toast quando a página for carregada
                    string script = "<script>showToast(' - Expedidor não encontrado no sistema.');</script>";
                    ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                    txtCodExpedidor.Text = "";
                    txtCodExpedidor.Focus();
                    return;
                }
            }

        }
        protected void txtCodDestinatario_TextChanged(object sender, EventArgs e)
        {
            if (txtCodDestinatario.Text != "")
            {
                string cod = txtCodDestinatario.Text;
                string sql = "SELECT  codcli, razcli, cnpj, cidcli, estcli, ativo_inativo, fl_exclusao, codvw, codsapiens FROM tbclientes where codcli = '" + cod + "' OR codvw = '" + cod + "' OR codsapiens = '" + cod + "' and ativo_inativo = 'ATIVO' and fl_exclusao is null";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                conn.Open();
                da.Fill(dt);
                conn.Close();

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0][6].ToString() == null)
                    {
                        // Acione o toast quando a página for carregada
                        string script = "<script>showToast(' - Destinatário deletado do sistema.');</script>";
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                        txtCodDestinatario.Text = "";
                        txtCodDestinatario.Focus();
                        return;
                    }
                    else if (dt.Rows[0][5].ToString() == "INATIVO")
                    {
                        // Acione o toast quando a página for carregada
                        string script = "<script>showToast(' - Destinatário inativo no sistema.');</script>";
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                        txtCodDestinatario.Text = "";
                        txtCodDestinatario.Focus();
                        return;
                    }
                    else
                    {
                        txtCodDestinatario.Text = dt.Rows[0][0].ToString();
                        cboDestinatario.SelectedItem.Text = dt.Rows[0][1].ToString();
                        txtCNPJDestinatario.Text = dt.Rows[0][2].ToString();
                        txtMunicipioDestinatario.Text = dt.Rows[0][3].ToString();
                        txtUFDestinatario.Text = dt.Rows[0][4].ToString();
                        txtObservacao.Text = "CODIGOS - Reflection: " + dt.Rows[0][0].ToString() + " VW: " + dt.Rows[0][7].ToString() + " Sapiens: " + dt.Rows[0][8].ToString();
                        txtCodRecebedor.Focus();
                        return;
                    }

                }
                else
                {
                    // Acione o toast quando a página for carregada
                    string script = "<script>showToast(' - Destinatário não encontrado no sistema.');</script>";
                    ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                    txtCodDestinatario.Text = "";
                    txtCodDestinatario.Focus();
                    return;
                }
            }

        }
        protected void txtCodRecebedor_TextChanged(object sender, EventArgs e)
        {
            if (txtCodRecebedor.Text != "")
            {
                string cod = txtCodRecebedor.Text;
                string sql = "SELECT  codcli, razcli, cnpj, cidcli, estcli, ativo_inativo, fl_exclusao, codvw, codsapiens FROM tbclientes where codcli = '" + cod + "' OR codvw = '" + cod + "' OR codsapiens = '" + cod + "' and ativo_inativo = 'ATIVO' and fl_exclusao is null";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                conn.Open();
                da.Fill(dt);
                conn.Close();

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0][6].ToString() == null)
                    {
                        // Acione o toast quando a página for carregada
                        string script = "<script>showToast(' - Destinatário deletado do sistema.');</script>";
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                        txtCodRecebedor.Text = "";
                        txtCodRecebedor.Focus();
                        return;
                    }
                    else if (dt.Rows[0][5].ToString() == "INATIVO")
                    {
                        // Acione o toast quando a página for carregada
                        string script = "<script>showToast(' - Recebedor inativo no sistema.');</script>";
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                        txtCodRecebedor.Text = "";
                        txtCodRecebedor.Focus();
                        return;
                    }
                    else
                    {
                        txtCodRecebedor.Text = dt.Rows[0][0].ToString();
                        cboRecebedor.SelectedItem.Text = dt.Rows[0][1].ToString();
                        txtCNPJRecebedor.Text = dt.Rows[0][2].ToString();
                        txtCidRecebedor.Text = dt.Rows[0][3].ToString();
                        txtUFRecebedor.Text = dt.Rows[0][4].ToString();
                        txtCodConsignatario.Focus();
                        return;
                    }

                }
                else
                {
                    // Acione o toast quando a página for carregada
                    string script = "<script>showToast(' - Recebedor não encontrado no sistema.');</script>";
                    ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                    txtCodRecebedor.Text = "";
                    txtCodRecebedor.Focus();
                    return;
                }
            }

        }
        protected void txtCodConsignatario_TextChanged(object sender, EventArgs e)
        {
            if (txtCodConsignatario.Text != "")
            {
                string cod = txtCodConsignatario.Text;
                string sql = "SELECT  codcli, razcli, cnpj, cidcli, estcli, ativo_inativo, fl_exclusao, codvw, codsapiens FROM tbclientes where codcli = '" + cod + "' OR codvw = '" + cod + "' OR codsapiens = '" + cod + "' and ativo_inativo = 'ATIVO' and fl_exclusao is null";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                conn.Open();
                da.Fill(dt);
                conn.Close();

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0][6].ToString() == null)
                    {
                        // Acione o toast quando a página for carregada
                        string script = "<script>showToast(' - Consignatário deletado do sistema.');</script>";
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                        txtCodConsignatario.Text = "";
                        txtCodConsignatario.Focus();
                        return;
                    }
                    else if (dt.Rows[0][5].ToString() == "INATIVO")
                    {
                        // Acione o toast quando a página for carregada
                        string script = "<script>showToast(' - Consignatário inativo no sistema.');</script>";
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                        txtCodConsignatario.Text = "";
                        txtCodConsignatario.Focus();
                        return;
                    }
                    else
                    {
                        txtCodConsignatario.Text = dt.Rows[0][0].ToString();
                        txtConsignatario.SelectedItem.Text = dt.Rows[0][1].ToString();
                        txtCNPJConsignatario.Text = dt.Rows[0][2].ToString();
                        txtCidConsignatario.Text = dt.Rows[0][3].ToString();
                        txtUFConsignatario.Text = dt.Rows[0][4].ToString();
                        txtCodPagador.Focus();
                        return;
                    }

                }
                else
                {
                    // Acione o toast quando a página for carregada
                    string script = "<script>showToast(' - Consignatário não encontrado no sistema.');</script>";
                    ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                    txtCodConsignatario.Text = "";
                    txtCodConsignatario.Focus();
                    return;
                }
            }

        }
        protected void txtCodPagador_TextChanged(object sender, EventArgs e)
        {
            if (txtCodPagador.Text != "")
            {
                string cod = txtCodPagador.Text;
                string sql = "SELECT  codcli, razcli, cnpj, cidcli, estcli, ativo_inativo, fl_exclusao, codvw, codsapiens FROM tbclientes where codcli = '" + cod + "' OR codvw = '" + cod + "' OR codsapiens = '" + cod + "' and ativo_inativo = 'ATIVO' and fl_exclusao is null";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                conn.Open();
                da.Fill(dt);
                conn.Close();

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0][6].ToString() == null)
                    {
                        // Acione o toast quando a página for carregada
                        string script = "<script>showToast(' - Pagador deletado do sistema.');</script>";
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                        txtCodPagador.Text = "";
                        txtCodPagador.Focus();
                        return;
                    }
                    else if (dt.Rows[0][5].ToString() == "INATIVO")
                    {
                        // Acione o toast quando a página for carregada
                        string script = "<script>showToast(' - Pagador inativo no sistema.');</script>";
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                        txtCodPagador.Text = "";
                        txtCodPagador.Focus();
                        return;
                    }
                    else
                    {
                        txtCodPagador.Text = dt.Rows[0][0].ToString();
                        txtPagador.SelectedItem.Text = dt.Rows[0][1].ToString();
                        txtCNPJPagador.Text = dt.Rows[0][2].ToString();
                        txtCidPagador.Text = dt.Rows[0][3].ToString();
                        txtUFPagador.Text = dt.Rows[0][4].ToString();

                        if (txtCodRemetente.Text == "")
                        {
                            // MostrarMsg("Digite o Remetente, por favor!", "danger");
                            // Acione o toast quando a página for carregada
                            string script = "<script>showToast(' - Digite o Remetente, por favor!');</script>";
                            ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                            txtCodRemetente.Text = string.Empty;
                            txtCodRemetente.Focus();
                            return;
                        }
                        if (txtCodExpedidor.Text == "")
                        {
                            // MostrarMsg("Digite o Expedidor, por favor!", "danger");
                            // Acione o toast quando a página for carregada
                            string script = "<script>showToast(' - Digite o Expedidor, por favor!');</script>";
                            ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                            txtCodExpedidor.Text = string.Empty;
                            txtCodExpedidor.Focus();
                            return;
                        }
                        if (txtCodDestinatario.Text == "")
                        {
                            //MostrarMsg("Digite o Destinatário, por favor!", "danger");
                            // Acione o toast quando a página for carregada
                            string script = "<script>showToast(' - Digite o Destinatário, por favor!');</script>";
                            ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                            txtCodDestinatario.Text = string.Empty;
                            txtCodDestinatario.Focus();
                            return;
                        }
                        if (txtCodRecebedor.Text == "")
                        {
                            //MostrarMsg("Digite o Recebedor, por favor!", "danger");
                            // Acione o toast quando a página for carregada
                            string script = "<script>showToast(' - Digite o Recebedor, por favor!');</script>";
                            ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                            txtCodRecebedor.Text = string.Empty;
                            txtCodRecebedor.Focus();
                            return;
                        }
                        if (txtCodPagador.Text == "")
                        {
                            //MostrarMsg("Digite o Pagador, por favor!", "danger");
                            // Acione o toast quando a página for carregada
                            string script = "<script>showToast(' - Digite o Pagador, por favor!');</script>";
                            ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                            txtCodPagador.Text = string.Empty;
                            txtCodPagador.Focus();
                            return;
                        }
                        string nomeRota = txtCidExpedidor.Text.Trim() + "/" + txtUFExpedidor.Text.Trim() + " X " + txtCidRecebedor.Text.Trim() + "/" + txtUFRecebedor.Text.Trim();

                        BuscarRota(nomeRota);



                        return;
                    }

                }
                else
                {
                    // Acione o toast quando a página for carregada
                    string script = "<script>showToast(' - Pagador não encontrado no sistema.');</script>";
                    ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                    txtCodPagador.Text = "";
                    txtCodPagador.Focus();
                    return;
                }
            }

        }

        protected void cboRemetente_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtCodRemetente.Text = cboRemetente.SelectedValue;
            if (txtCodRemetente.Text != "")
            {
                string cod = txtCodRemetente.Text;
                string sql = "SELECT  codcli, razcli, cnpj, cidcli, estcli, ativo_inativo, fl_exclusao, codvw, codsapiens FROM tbclientes where codcli = '" + cod + "' OR codvw = '" + cod + "' OR codsapiens = '" + cod + "' and ativo_inativo = 'ATIVO' and fl_exclusao is null";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                conn.Open();
                da.Fill(dt);
                conn.Close();

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0][6].ToString() == null)
                    {
                        // Acione o toast quando a página for carregada
                        string script = "<script>showToast(txtCodRemetente.Text.Trim() + ' - Remetente deletado do sistema.');</script>";
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                        txtCodRemetente.Text = "";
                        txtCodRemetente.Focus();
                        return;
                    }
                    else if (dt.Rows[0][5].ToString() == "INATIVO")
                    {
                        // Acione o toast quando a página for carregada
                        string script = "<script>showToast(' - Remetente inativo no sistema.');</script>";
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                        txtCodRemetente.Text = "";
                        txtCodRemetente.Focus();
                        return;
                    }
                    else
                    {
                        txtCodRemetente.Text = dt.Rows[0][0].ToString();
                        cboRemetente.SelectedItem.Text = dt.Rows[0][1].ToString();
                        txtCNPJRemetente.Text = dt.Rows[0][2].ToString();
                        txtMunicipioRemetente.Text = dt.Rows[0][3].ToString();
                        txtUFRemetente.Text = dt.Rows[0][4].ToString();
                        txtCodExpedidor.Focus();
                        return;
                    }

                }
                else
                {
                    // Acione o toast quando a página for carregada
                    string script = "<script>showToast(' - Remetente não encontrado no sistema.');</script>";
                    ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                    txtCodRemetente.Text = "";
                    txtCodRemetente.Focus();
                    return;
                }
            }

        }
        protected void cboExpedidor_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtCodExpedidor.Text = cboExpedidor.SelectedValue;
            if (txtCodExpedidor.Text != "")
            {
                string cod = txtCodExpedidor.Text;
                string sql = "SELECT  codcli, razcli, cnpj, cidcli, estcli, ativo_inativo, fl_exclusao, codvw, codsapiens FROM tbclientes where codcli = '" + cod + "' OR codvw = '" + cod + "' OR codsapiens = '" + cod + "' and ativo_inativo = 'ATIVO' and fl_exclusao is null";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                conn.Open();
                da.Fill(dt);
                conn.Close();

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0][6].ToString() == null)
                    {
                        // Acione o toast quando a página for carregada
                        string script = "<script>showToast(' - Expedidor deletado do sistema.');</script>";
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                        txtCodExpedidor.Text = "";
                        txtCodExpedidor.Focus();
                        return;
                    }
                    else if (dt.Rows[0][5].ToString() == "INATIVO")
                    {
                        // Acione o toast quando a página for carregada
                        string script = "<script>showToast(' - Expedidor inativo no sistema.');</script>";
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                        txtCodExpedidor.Text = "";
                        txtCodExpedidor.Focus();
                        return;
                    }
                    else
                    {
                        txtCodExpedidor.Text = dt.Rows[0][0].ToString();
                        cboExpedidor.SelectedItem.Text = dt.Rows[0][1].ToString();
                        txtCNPJExpedidor.Text = dt.Rows[0][2].ToString();
                        txtCidExpedidor.Text = dt.Rows[0][3].ToString();
                        txtUFExpedidor.Text = dt.Rows[0][4].ToString();
                        txtCodDestinatario.Focus();
                        return;
                    }

                }
                else
                {
                    // Acione o toast quando a página for carregada
                    string script = "<script>showToast(' - Expedidor não encontrado no sistema.');</script>";
                    ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                    txtCodExpedidor.Text = "";
                    txtCodExpedidor.Focus();
                    return;
                }
            }

        }
        protected void cboDestinatario_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtCodDestinatario.Text = cboDestinatario.SelectedValue;
            if (txtCodDestinatario.Text != "")
            {
                string cod = txtCodDestinatario.Text;
                string sql = "SELECT  codcli, razcli, cnpj, cidcli, estcli, ativo_inativo, fl_exclusao, codvw, codsapiens FROM tbclientes where codcli = '" + cod + "' OR codvw = '" + cod + "' OR codsapiens = '" + cod + "' and ativo_inativo = 'ATIVO' and fl_exclusao is null";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                conn.Open();
                da.Fill(dt);
                conn.Close();

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0][6].ToString() == null)
                    {
                        // Acione o toast quando a página for carregada
                        string script = "<script>showToast(' - Destinatário deletado do sistema.');</script>";
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                        txtCodDestinatario.Text = "";
                        txtCodDestinatario.Focus();
                        return;
                    }
                    else if (dt.Rows[0][5].ToString() == "INATIVO")
                    {
                        // Acione o toast quando a página for carregada
                        string script = "<script>showToast(' - Destinatário inativo no sistema.');</script>";
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                        txtCodDestinatario.Text = "";
                        txtCodDestinatario.Focus();
                        return;
                    }
                    else
                    {
                        txtCodDestinatario.Text = dt.Rows[0][0].ToString();
                        cboDestinatario.SelectedItem.Text = dt.Rows[0][1].ToString();
                        txtCNPJDestinatario.Text = dt.Rows[0][2].ToString();
                        txtMunicipioDestinatario.Text = dt.Rows[0][3].ToString();
                        txtUFDestinatario.Text = dt.Rows[0][4].ToString();
                        txtObservacao.Text = "CODIGOS - Reflection: " + dt.Rows[0][0].ToString() + " VW: " + dt.Rows[0][7].ToString() + " Sapiens: " + dt.Rows[0][8].ToString();
                        txtCodRecebedor.Focus();
                        return;
                    }

                }
                else
                {
                    // Acione o toast quando a página for carregada
                    string script = "<script>showToast(' - Destinatario não encontrado no sistema.');</script>";
                    ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                    txtCodDestinatario.Text = "";
                    txtCodDestinatario.Focus();
                    return;
                }
            }

        }
        protected void cboRecebedor_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtCodRecebedor.Text = cboRecebedor.SelectedValue;
            if (txtCodRecebedor.Text != "")
            {
                string cod = txtCodRecebedor.Text;
                string sql = "SELECT  codcli, razcli, cnpj, cidcli, estcli, ativo_inativo, fl_exclusao, codvw, codsapiens FROM tbclientes where codcli = '" + cod + "' OR codvw = '" + cod + "' OR codsapiens = '" + cod + "' and ativo_inativo = 'ATIVO' and fl_exclusao is null";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                conn.Open();
                da.Fill(dt);
                conn.Close();

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0][6].ToString() == null)
                    {
                        // Acione o toast quando a página for carregada
                        string script = "<script>showToast(' - Recebedor deletado do sistema.');</script>";
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                        txtCodRecebedor.Text = "";
                        txtCodRecebedor.Focus();
                        return;
                    }
                    else if (dt.Rows[0][5].ToString() == "INATIVO")
                    {
                        // Acione o toast quando a página for carregada
                        string script = "<script>showToast(' - Recebedor inativo no sistema.');</script>";
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                        txtCodRecebedor.Text = "";
                        txtCodRecebedor.Focus();
                        return;
                    }
                    else
                    {
                        txtCodRecebedor.Text = dt.Rows[0][0].ToString();
                        cboRecebedor.SelectedItem.Text = dt.Rows[0][1].ToString();
                        txtCNPJRecebedor.Text = dt.Rows[0][2].ToString();
                        txtCidRecebedor.Text = dt.Rows[0][3].ToString();
                        txtUFRecebedor.Text = dt.Rows[0][4].ToString();
                        txtCodConsignatario.Focus();
                        return;
                    }

                }
                else
                {
                    // Acione o toast quando a página for carregada
                    string script = "<script>showToast(' - Recebedor não encontrado no sistema.');</script>";
                    ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                    txtCodRecebedor.Text = "";
                    txtCodRecebedor.Focus();
                    return;
                }
            }

        }
        protected void txtConsignatario_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtCodConsignatario.Text = txtConsignatario.SelectedValue;
            if (txtCodConsignatario.Text != "")
            {
                string cod = txtCodConsignatario.Text;
                string sql = "SELECT  codcli, razcli, cnpj, cidcli, estcli, ativo_inativo, fl_exclusao, codvw, codsapiens FROM tbclientes where codcli = '" + cod + "' OR codvw = '" + cod + "' OR codsapiens = '" + cod + "' and ativo_inativo = 'ATIVO' and fl_exclusao is null";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                conn.Open();
                da.Fill(dt);
                conn.Close();

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0][6].ToString() == null)
                    {
                        // Acione o toast quando a página for carregada
                        string script = "<script>showToast(' - Consignatário deletado do sistema.');</script>";
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                        txtCodConsignatario.Text = "";
                        txtCodConsignatario.Focus();
                        return;
                    }
                    else if (dt.Rows[0][5].ToString() == "INATIVO")
                    {
                        // Acione o toast quando a página for carregada
                        string script = "<script>showToast(' - Consignatário inativo no sistema.');</script>";
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                        txtCodConsignatario.Text = "";
                        txtCodConsignatario.Focus();
                        return;
                    }
                    else
                    {
                        txtCodConsignatario.Text = dt.Rows[0][0].ToString();
                        txtConsignatario.SelectedItem.Text = dt.Rows[0][1].ToString();
                        txtCNPJConsignatario.Text = dt.Rows[0][2].ToString();
                        txtCidConsignatario.Text = dt.Rows[0][3].ToString();
                        txtUFConsignatario.Text = dt.Rows[0][4].ToString();
                        txtCodPagador.Focus();
                        return;
                    }

                }
                else
                {
                    // Acione o toast quando a página for carregada
                    string script = "<script>showToast(' - Consignatário não encontrado no sistema.');</script>";
                    ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                    txtCodConsignatario.Text = "";
                    txtCodConsignatario.Focus();
                    return;
                }
            }

        }
        protected void txtPagador_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtCodPagador.Text = txtPagador.SelectedValue;
            if (txtCodPagador.Text != "")
            {
                string cod = txtCodPagador.Text;
                string sql = "SELECT  codcli, razcli, cnpj, cidcli, estcli, ativo_inativo, fl_exclusao, codvw, codsapiens FROM tbclientes where codcli = '" + cod + "' OR codvw = '" + cod + "' OR codsapiens = '" + cod + "' and ativo_inativo = 'ATIVO' and fl_exclusao is null";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                conn.Open();
                da.Fill(dt);
                conn.Close();

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0][6].ToString() == null)
                    {
                        // Acione o toast quando a página for carregada
                        string script = "<script>showToast(' - Pagador deletado do sistema.');</script>";
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                        txtCodPagador.Text = "";
                        txtCodPagador.Focus();
                        return;
                    }
                    else if (dt.Rows[0][5].ToString() == "INATIVO")
                    {
                        // Acione o toast quando a página for carregada
                        string script = "<script>showToast(' - Pagador inativo no sistema.');</script>";
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                        txtCodPagador.Text = "";
                        txtCodPagador.Focus();
                        return;
                    }
                    else
                    {
                        txtCodPagador.Text = dt.Rows[0][0].ToString();
                        txtPagador.SelectedItem.Text = dt.Rows[0][1].ToString();
                        txtCNPJPagador.Text = dt.Rows[0][2].ToString();
                        txtCidPagador.Text = dt.Rows[0][3].ToString();
                        txtUFPagador.Text = dt.Rows[0][4].ToString();
                        if (txtCodRemetente.Text == "")
                        {
                            // MostrarMsg("Digite o Remetente, por favor!", "danger");
                            // Acione o toast quando a página for carregada
                            string script = "<script>showToast(' - Digite o Remetente, por favor!');</script>";
                            ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                            txtCodRemetente.Text = string.Empty;
                            txtCodRemetente.Focus();
                            return;
                        }
                        if (txtCodExpedidor.Text == "")
                        {
                            // MostrarMsg("Digite o Expedidor, por favor!", "danger");
                            // Acione o toast quando a página for carregada
                            string script = "<script>showToast(' - Digite o Expedidor, por favor!');</script>";
                            ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                            txtCodExpedidor.Text = string.Empty;
                            txtCodExpedidor.Focus();
                            return;
                        }
                        if (txtCodDestinatario.Text == "")
                        {
                            //MostrarMsg("Digite o Destinatário, por favor!", "danger");
                            // Acione o toast quando a página for carregada
                            string script = "<script>showToast(' - Digite o Destinatário, por favor!');</script>";
                            ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                            txtCodDestinatario.Text = string.Empty;
                            txtCodDestinatario.Focus();
                            return;
                        }
                        if (txtCodRecebedor.Text == "")
                        {
                            //MostrarMsg("Digite o Recebedor, por favor!", "danger");
                            // Acione o toast quando a página for carregada
                            string script = "<script>showToast(' - Digite o Recebedor, por favor!');</script>";
                            ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                            txtCodRecebedor.Text = string.Empty;
                            txtCodRecebedor.Focus();
                            return;
                        }
                        if (txtCodPagador.Text == "")
                        {
                            //MostrarMsg("Digite o Pagador, por favor!", "danger");
                            // Acione o toast quando a página for carregada
                            string script = "<script>showToast(' - Digite o Pagador, por favor!');</script>";
                            ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                            txtCodPagador.Text = string.Empty;
                            txtCodPagador.Focus();
                            return;
                        }
                        string nomeRota = txtCidExpedidor.Text.Trim() + "/" + txtUFExpedidor.Text.Trim() + " X " + txtCidRecebedor.Text.Trim() + "/" + txtUFRecebedor.Text.Trim();

                        BuscarRota(nomeRota);



                        return;
                    }

                }
                else
                {
                    // Acione o toast quando a página for carregada
                    string script = "<script>showToast(' - Pagador não encontrado no sistema.');</script>";
                    ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                    txtCodPagador.Text = "";
                    txtCodPagador.Focus();
                    return;
                }
            }

        }

        protected void cbSolicitantes_SelectedIndexChanged(object sender, EventArgs e)
        {
            string idSolicitante = cbSolicitantes.SelectedValue;
            if (idSolicitante != "")
            {
                string cod = idSolicitante;
                string sql = "SELECT  id, nome, gr, status FROM tbsolicitantes where id = '" + cod + "' and status = 'Ativo' ";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                conn.Open();
                da.Fill(dt);
                conn.Close();

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0][3].ToString() == "Inativo")
                    {
                        // Acione o toast quando a página for carregada
                        string script = "<script>showToast(' - Solicitante inativo no sistema.');</script>";
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                        //txtCodPagador.Text = "";
                        //txtCodPagador.Focus();
                        return;
                    }
                    else
                    {
                        cbSolicitantes.SelectedItem.Text = dt.Rows[0][1].ToString();
                        cboGR.Text = dt.Rows[0][2].ToString();
                        return;
                    }

                }
                else
                {
                    // Acione o toast quando a página for carregada
                    string script = "<script>showToast(' - Solicitante não encontrado no sistema.');</script>";
                    ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                    return;
                }
            }

        }
    }
}