using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using NPOI.SS.Formula;
using static NewCapit.dist.pages.Frm_TabelaPrecoMatriz;
using static NewCapit.Main;

namespace NewCapit.dist.pages
{
    public partial class Frm_CadPedidos : System.Web.UI.Page
    {
        SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString());
        string nomeUsuario = string.Empty;
        DateTime dataHoraAtual = DateTime.Now;
        decimal totalQuantidade = 0;
        //int totalLinhas = 0;
        string totalPesoCarga;
        string newPedidoAvuldo;
        string cTipoVeiculo;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UsuarioLogado"] != null)
                {
                    string nomeUsuario = Session["UsuarioLogado"].ToString();
                    var lblUsuario = nomeUsuario;

                    txtUsuCadastro.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm") + " - " + nomeUsuario.ToUpper();
                    
                }
                else
                {
                    var lblUsuario = "<Usuário>";
                    Response.Redirect("Login.aspx");

                }
                CarregarClientes(cboRemetente, cboExpedidor, cboDestinatario, cboRecebedor, txtConsignatario, txtPagador);
                PreencherNumCarga();
                PreencherComboSolicitantes();   
                PreencherComboMateriais();
                PreencherComboDeposito();
            }
            //DateTime dataHoraAtual = DateTime.Now;
            txtCadastro.Text = dataHoraAtual.ToString("dd/MM/yyyy HH:mm");
            txtFilial.Text = "MATRIZ";
        }
        private void PreencherNumCarga()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT (carga + incremento) as ProximaCarga, (pedido + incremento) as ProximoPedidoAvulso FROM tbcontadores";
            
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
                                newPedidoAvuldo = reader["ProximoPedidoAvulso"].ToString();
                                novaCarga.Text = reader["ProximaCarga"].ToString();
                                lblNewPedidoAvulso.Text = "Prox.Ped.: " + reader["ProximoPedidoAvulso"].ToString();
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
                    string sql = @"UPDATE tbcontadores SET carga = @carga, pedido = @pedido WHERE id = @id";
                    try
                    {
                        using (SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
                        using (SqlCommand cmd = new SqlCommand(sql, con))
                        {
                            cmd.Parameters.AddWithValue("@carga", novaCarga.Text);
                            cmd.Parameters.AddWithValue("@pedido", newPedidoAvuldo);
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
        //private void PreencherComboGR()
        //{
        //    // Consulta SQL que retorna os dados desejados
        //    string query = "SELECT id, nomgr FROM tbgr";

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
        //            cboGR.DataSource = reader;
        //            cboGR.DataTextField = "nomgr";  // Campo que será mostrado no ComboBox
        //            cboGR.DataValueField = "id";  // Campo que será o valor de cada item                    
        //            cboGR.DataBind();  // Realiza o binding dos dados                   
        //            cboGR.Items.Insert(0, new ListItem("Selecione...", "0"));
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
        //                //txtTipoVeiculo.Text = dt.Rows[0][27].ToString();
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
        //            cboRemetente.SelectedItem.Text = reader["remetente"].ToString();
        //            txtMunicipioRemetente.Text = reader["cid_remetente"].ToString();
        //            txtUFRemetente.Text = reader["uf_remetente"].ToString();

        //            txtCodExpedidor.Text = reader["cod_expedidor"].ToString();
        //            cboExpedidor.SelectedItem.Text = reader["expedidor"].ToString();
        //            txtCidExpedidor.Text = reader["cid_expedidor"].ToString();
        //            txtUFExpedidor.Text = reader["uf_expedidor"].ToString();

        //            txtCodDestinatario.Text = reader["cod_destinatario"].ToString();
        //            cboDestinatario.SelectedItem.Text = reader["destinatario"].ToString();
        //            txtMunicipioDestinatario.Text = reader["cid_destinatario"].ToString();
        //            txtUFDestinatario.Text = reader["uf_destinatario"].ToString();

        //            txtCodRecebedor.Text = reader["cod_recebedor"].ToString();
        //            cboRecebedor.SelectedItem.Text = reader["recebedor"].ToString();
        //            txtCidRecebedor.Text = reader["cid_recebedor"].ToString();
        //            txtUFRecebedor.Text = reader["uf_recebedor"].ToString();

        //            txtCodConsignatario.Text = reader["cod_consignatario"].ToString();
        //            txtConsignatario.SelectedItem.Text = reader["consignatario"].ToString();
        //            txtCidConsignatario.Text = reader["cid_consignatario"].ToString();
        //            txtUFConsignatario.Text = reader["uf_consignatario"].ToString();

        //            txtCodPagador.Text = reader["cod_pagador"].ToString();
        //            txtPagador.SelectedItem.Text = reader["pagador"].ToString();
        //            txtCidPagador.Text = reader["cid_pagador"].ToString();
        //            txtUFPagador.Text = reader["uf_pagador"].ToString();

        //            txtFilial.Text = reader["nucleo"].ToString();
        //            //txtTipoVeiculo.Text = reader["tipo_veiculo"].ToString();
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
            //else
            //{
            //    // Acione o toast quando a página for carregada
            //    string script = "<script>showToast('Escolha uma rota válida, para continuar...');</script>";
            //    ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
            //    txtFrete.Text = "";
            //    txtFrete.Focus();
            //    return;
            //}

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
                    decimal peso = decimal.Parse(txtPeso.Text, new CultureInfo("pt-BR"));
                    string cod = txtNumPedido.Text;
                    using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
                    {
                        string nomeCompleto = txtPagador.Text;
                        string primeiroNome = nomeCompleto.Split(' ')[0];
                        conn.Open();
                        string query = "SELECT pedido FROM tbpedidos WHERE pedido = @ID";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@ID", cod);
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            // encontrou update
                            reader.Close();                           
                            string sql = @"
                            UPDATE tbpedidos 
                            SET 
                                carga = @carga,
                                material = @material,
                                peso = @peso,
                                portao = @portao,
                                situacao = @situacao,
                                previsao = @previsao,
                                entrega = @entrega,
                                controledocliente = @controledocliente,
                                observacao = @observacao,
                                atualizacao = @atualizacao,
                                gr = @gr,
                                tomador = @tomador,
                                lotacao = @lotacao      
                            WHERE pedido = @pedido";


                            try
                            {
                                using (SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
                            using (SqlCommand cmdUpdate = new SqlCommand(sql, con))
                            {
                                
                                cmdUpdate.Parameters.Add("@carga", SqlDbType.NVarChar).Value = novaCarga.Text;
                                cmdUpdate.Parameters.Add("@material", SqlDbType.NVarChar).Value = cboMaterial.SelectedItem.Text;
                                    //decimal peso;

                                    //if (!decimal.TryParse(
                                    //        txtPeso.Text.Replace(",", "."),
                                    //        System.Globalization.NumberStyles.Any,
                                    //        System.Globalization.CultureInfo.InvariantCulture,
                                    //        out peso))
                                    //{
                                    //    throw new Exception("Peso inválido.");
                                    //}
                                   
                                    cmdUpdate.Parameters.AddWithValue("@peso",  peso); // decimal convertido
                                cmdUpdate.Parameters.Add("@portao", SqlDbType.NVarChar).Value = cboDeposito.SelectedItem.Text;
                                cmdUpdate.Parameters.Add("@situacao", SqlDbType.NVarChar).Value = cboSituacao.SelectedItem.Text;
                                cmdUpdate.Parameters.Add("@previsao", SqlDbType.Date).Value = DateTime.Parse(txtPrevEntrega.Text);
                                cmdUpdate.Parameters.Add("@entrega", SqlDbType.NVarChar).Value = cboEntrega.SelectedItem.Text;
                                cmdUpdate.Parameters.Add("@controledocliente", SqlDbType.NVarChar).Value = txtControleCliente.Text;
                                cmdUpdate.Parameters.Add("@observacao", SqlDbType.NVarChar).Value = txtObservacao.Text.ToUpper();
                                cmdUpdate.Parameters.Add("@gr", SqlDbType.NVarChar).Value = cboGR.Text;
                                cmdUpdate.Parameters.Add("@atualizacao", SqlDbType.NVarChar).Value =
                                    dataHoraAtual.ToString("dd/MM/yyyy HH:mm") + " - " + nomeUsuario.ToUpper();
                                cmdUpdate.Parameters.Add("@tomador", SqlDbType.NVarChar).Value = primeiroNome;
                                cmdUpdate.Parameters.Add("@pedido", SqlDbType.Int).Value = int.Parse(txtNumPedido.Text);
                                cmdUpdate.Parameters.Add("@lotacao", SqlDbType.NVarChar).Value = txtLotacao.Text;

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
                            comando.Parameters.AddWithValue("@peso", peso);
                            comando.Parameters.AddWithValue("@material", cboMaterial.SelectedItem.Text);
                            comando.Parameters.AddWithValue("@portao", cboDeposito.SelectedItem.Text);
                            comando.Parameters.AddWithValue("@situacao", cboSituacao.SelectedItem.Text);
                            comando.Parameters.AddWithValue("@previsao", SafeDateValue(txtPrevEntrega.Text));
                            comando.Parameters.AddWithValue("@codorigem", txtCodRemetente.Text);
                            comando.Parameters.AddWithValue("@cliorigem", cboRemetente.SelectedItem.Text);
                            comando.Parameters.AddWithValue("@coddestino", txtCodDestinatario.Text);
                            comando.Parameters.AddWithValue("@clidestino", cboDestinatario.SelectedItem.Text);
                            comando.Parameters.AddWithValue("@observacao", txtObservacao.Text.ToUpper());
                            comando.Parameters.AddWithValue("@andamento", "PENDENTE");
                            comando.Parameters.AddWithValue("@ufcliorigem", txtUFRemetente.Text);
                            comando.Parameters.AddWithValue("@ufclidestino", txtUFDestinatario.Text);
                            comando.Parameters.AddWithValue("@tomador", txtPagador.SelectedItem.Text);
                            comando.Parameters.AddWithValue("@cidorigem", txtMunicipioRemetente.Text);
                            comando.Parameters.AddWithValue("@ciddestino", txtMunicipioDestinatario.Text);
                            comando.Parameters.AddWithValue("@gr", cboGR.Text);
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
        decimal ToDecimal(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                return 0;

            return Convert.ToDecimal(valor.Replace(".", "").Replace(",", "."),
                System.Globalization.CultureInfo.InvariantCulture);
        }

        protected void gvPedidos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                
                decimal quantidade = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Peso"), new CultureInfo("pt-BR"));
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
                    string nomeCompleto = txtPagador.Text;
                    string primeiroNome = nomeCompleto.Split(' ')[0];
                    conn.Close();
                    decimal totalPesoCarga = 0;
                    if (ViewState["TotalPesoCarga"] != null)
                        totalPesoCarga = Convert.ToDecimal(ViewState["TotalPesoCarga"]);
                    if (totalPesoCarga <= 12000)
                    {
                        txtTipoVeiculo.Text = "TRUCK";
                        cTipoVeiculo = txtTipoVeiculo.Text;
                    }
                    if (totalPesoCarga > 12000 && totalPesoCarga <= 25000)
                    {
                        txtTipoVeiculo.Text = "CAVALO SIMPLES";
                        cTipoVeiculo = txtTipoVeiculo.Text;
                    }
                    if (totalPesoCarga > 25000)
                    {
                        txtTipoVeiculo.Text = "CAVALO TRUCADO";
                        cTipoVeiculo = txtTipoVeiculo.Text;
                    }
                    string sqlSalvarPedido = "insert into tbcargas " + "(carga, emissao, status, tomador, entrega, peso, material, portao, situacao, previsao, codorigem, cliorigem, coddestino, clidestino, observacao, ufcliorigem, ufclidestino, pedidos, gr, ot, solicitante, empresa, andamento,cadastro, distancia, emitepedagio, cidorigem, ciddestino, nucleo, cod_expedidor, expedidor, cid_expedidor, uf_expedidor, cod_recebedor, recebedor, cid_recebedor, uf_recebedor, cod_consignatario, consignatario, cid_consignatario, uf_consignatario, cod_pagador, pagador, cid_pagador, uf_pagador, duracao, tipo_veiculo, deslocamento, cnpj_remetente, cnpj_expedidor, cnpj_destinatario, cnpj_recebedor, cnpj_consignatario, cnpj_pagador, rota_entrega)" +
                    "values" + "(@carga, @emissao, @status, @tomador, @entrega, @peso, @material, @portao, @situacao, @previsao, @codorigem, @cliorigem, @coddestino, @clidestino, @observacao, @ufcliorigem, @ufclidestino, @pedidos, @gr, @ot, @solicitante, @empresa, @andamento, @cadastro, @distancia, @emitepedagio, @cidorigem, @ciddestino, @nucleo, @cod_expedidor, @expedidor, @cid_expedidor, @uf_expedidor, @cod_recebedor, @recebedor, @cid_recebedor, @uf_recebedor, @cod_consignatario, @consignatario, @cid_consignatario, @uf_consignatario, @cod_pagador, @pagador, @cid_pagador, @uf_pagador, @duracao, @tipo_veiculo, @deslocamento, @cnpj_remetente, @cnpj_expedidor, @cnpj_destinatario, @cnpj_recebedor, @cnpj_consignatario, @cnpj_pagador, @rota_entrega)";
                    SqlCommand comando = new SqlCommand(sqlSalvarPedido, conn);

                    // nvarchar
                    comando.Parameters.Add("@carga", SqlDbType.NVarChar, 50).Value = novaCarga.Text;
                    comando.Parameters.Add("@status", SqlDbType.NVarChar, 50).Value = "Pendente";
                    comando.Parameters.Add("@tomador", SqlDbType.NVarChar, 170).Value = primeiroNome;
                    comando.Parameters.Add("@entrega", SqlDbType.NVarChar, 20).Value = cboEntrega.SelectedItem.Text;
                    comando.Parameters.Add("@material", SqlDbType.NVarChar, 50).Value = cboMaterial.SelectedItem.Text;
                    comando.Parameters.Add("@portao", SqlDbType.NVarChar, 20).Value = cboDeposito.SelectedItem.Text;
                    comando.Parameters.Add("@situacao", SqlDbType.NVarChar, 20).Value = cboSituacao.SelectedItem.Text;
                    comando.Parameters.Add("@cliorigem", SqlDbType.NVarChar, 150).Value = cboRemetente.SelectedItem.Text;
                    comando.Parameters.Add("@cnpj_remetente", SqlDbType.NVarChar, 50).Value = txtCNPJRemetente.Text;
                    comando.Parameters.Add("@clidestino", SqlDbType.NVarChar, 150).Value = cboDestinatario.SelectedItem.Text;
                    comando.Parameters.Add("@cnpj_destinatario", SqlDbType.NVarChar, 50).Value = txtCNPJDestinatario.Text;
                    comando.Parameters.Add("@observacao", SqlDbType.NVarChar).Value = txtObservacao.Text.ToUpper();
                    comando.Parameters.Add("@rota_entrega", SqlDbType.NVarChar).Value = txtRota.Text.ToUpper();
                    comando.Parameters.Add("@ufcliorigem", SqlDbType.NVarChar, 2).Value = txtUFRemetente.Text;
                    comando.Parameters.Add("@ufclidestino", SqlDbType.NVarChar, 2).Value = txtUFDestinatario.Text;
                    comando.Parameters.Add("@gr", SqlDbType.NVarChar, 50).Value = cboGR.Text;
                    comando.Parameters.Add("@ot", SqlDbType.NVarChar, 50).Value = txtControleCliente.Text;
                    comando.Parameters.Add("@solicitante", SqlDbType.NVarChar, 50).Value = cbSolicitantes.SelectedItem.Text;
                    comando.Parameters.Add("@empresa", SqlDbType.NVarChar, 50).Value = "1111";
                    comando.Parameters.Add("@andamento", SqlDbType.NVarChar, 50).Value = "PENDENTE";
                    comando.Parameters.Add("@cadastro", SqlDbType.NVarChar, 80).Value =
                        $"{dataHoraAtual:dd/MM/yyyy HH:mm} - {nomeUsuario.ToUpper()}";

                    // datetime / date
                    comando.Parameters.Add("@emissao", SqlDbType.DateTime).Value = SafeDateTimeValue(txtCadastro.Text);
                    comando.Parameters.Add("@previsao", SqlDbType.Date).Value = SafeDateValue(txtPrevEntrega.Text);

                    // decimal
                    comando.Parameters.Add("@peso", SqlDbType.Decimal).Value = Convert.ToDecimal(totalPesoCarga);
                    comando.Parameters.Add("@distancia", SqlDbType.Decimal).Value =
                        string.IsNullOrWhiteSpace(txtDistancia.Text) ? 0 : Convert.ToDecimal(txtDistancia.Text.Replace(",", "."));

                    // int
                    comando.Parameters.Add("@codorigem", SqlDbType.Int).Value = Convert.ToInt32(txtCodRemetente.Text);
                    comando.Parameters.Add("@coddestino", SqlDbType.Int).Value = Convert.ToInt32(txtCodDestinatario.Text);
                    comando.Parameters.Add("@cod_expedidor", SqlDbType.Int).Value = Convert.ToInt32(txtCodExpedidor.Text);
                    comando.Parameters.Add("@cod_recebedor", SqlDbType.Int).Value = Convert.ToInt32(txtCodRecebedor.Text);
                    comando.Parameters.Add("@cod_consignatario", SqlDbType.Int).Value = SafeConvert.SafeIntNullable(txtCodConsignatario.Text);
                    comando.Parameters.Add("@cod_pagador", SqlDbType.Int).Value = Convert.ToInt32(txtCodPagador.Text);
                    //comando.Parameters.Add("@cod_tomador", SqlDbType.Int).Value = Convert.ToInt32(txtFrete.Text);

                    // outros
                    comando.Parameters.Add("@pedidos", SqlDbType.NVarChar).Value = gvPedidos.Rows.Count.ToString();
                    comando.Parameters.Add("@emitepedagio", SqlDbType.NChar, 3).Value = txtPedagio.Text;
                    comando.Parameters.Add("@cidorigem", SqlDbType.NVarChar, 50).Value = txtMunicipioRemetente.Text;
                    comando.Parameters.Add("@ciddestino", SqlDbType.NVarChar, 50).Value = txtMunicipioDestinatario.Text;
                    comando.Parameters.Add("@nucleo", SqlDbType.NVarChar, 50).Value = txtFilial.Text;
                    comando.Parameters.Add("@duracao", SqlDbType.NVarChar, 15).Value = txtDuracao.Text;
                    comando.Parameters.Add("@tipo_veiculo", SqlDbType.NVarChar, 50).Value = cTipoVeiculo;
                    comando.Parameters.Add("@deslocamento", SqlDbType.NVarChar, 30).Value = txtDeslocamento.Text;

                    // EXPEDIDOR
                    comando.Parameters.Add("@expedidor", SqlDbType.NVarChar, 150).Value = cboExpedidor.SelectedItem.Text;
                    comando.Parameters.Add("@cnpj_expedidor", SqlDbType.NVarChar, 50).Value = txtCNPJExpedidor.Text;
                    comando.Parameters.Add("@cid_expedidor", SqlDbType.NVarChar, 50).Value = txtCidExpedidor.Text;
                    comando.Parameters.Add("@uf_expedidor", SqlDbType.NVarChar, 2).Value = txtUFExpedidor.Text;

                    // RECEBEDOR
                    comando.Parameters.Add("@recebedor", SqlDbType.NVarChar, 150).Value = cboRecebedor.SelectedItem.Text;
                    comando.Parameters.Add("@cnpj_recebedor", SqlDbType.NVarChar, 50).Value = txtCNPJRecebedor.Text;
                    comando.Parameters.Add("@cid_recebedor", SqlDbType.NVarChar, 50).Value = txtCidRecebedor.Text;
                    comando.Parameters.Add("@uf_recebedor", SqlDbType.NVarChar, 2).Value = txtUFRecebedor.Text;

                    // CONSIGNATÁRIO
                    comando.Parameters.Add("@consignatario", SqlDbType.NVarChar, 150).Value = txtConsignatario.SelectedItem.Text;
                    comando.Parameters.Add("@cnpj_consignatario", SqlDbType.NVarChar, 50).Value = txtCNPJConsignatario.Text;
                    comando.Parameters.Add("@cid_consignatario", SqlDbType.NVarChar, 50).Value = txtCidConsignatario.Text;
                    comando.Parameters.Add("@uf_consignatario", SqlDbType.NVarChar, 2).Value = txtUFConsignatario.Text;

                    // PAGADOR
                    comando.Parameters.Add("@pagador", SqlDbType.NVarChar, 150).Value = txtPagador.SelectedItem.Text;
                    comando.Parameters.Add("@cnpj_pagador", SqlDbType.NVarChar, 50).Value = txtCNPJPagador.Text;
                    comando.Parameters.Add("@cid_pagador", SqlDbType.NVarChar, 50).Value = txtCidPagador.Text;
                    comando.Parameters.Add("@uf_pagador", SqlDbType.NVarChar, 2).Value = txtUFPagador.Text;



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