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
//using DocumentFormat.OpenXml.Drawing.Charts;
using MathNet.Numerics;

namespace NewCapit.dist.pages
{
    public partial class reajustefretes : System.Web.UI.Page
    {
        SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString());
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UsuarioLogado"] != null)
                {
                    string nomeUsuario = Session["UsuarioLogado"].ToString();
                    var lblUsuario = nomeUsuario;                    
                }
                else
                {
                    var lblUsuario = "<Usuário>";                   
                }
                               
                PreencherComboTipoVeiculos();
                PreencherComboMateriais();
                PreencherCombosClientes(cboRemetente,cboPagador);
                PreencherComboRotas();              
                CarregarCidades();

            }

        }
        private void PreencherComboRotas()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT rota, desc_rota, situacao, fl_exclusao FROM tbrotasdeentregas where fl_exclusao is null and situacao = 'ATIVO' order by desc_rota";

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
                    cboRota.DataSource = reader;
                    cboRota.DataTextField = "desc_rota";  // Campo que será mostrado no ComboBox
                    cboRota.DataValueField = "rota";  // Campo que será o valor de cada item                    
                    cboRota.DataBind();  // Realiza o binding dos dados                   
                    cboRota.Items.Insert(0, new ListItem("Selecione...", "0"));
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
        private void PreencherCombosClientes(params DropDownList[] combos)
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT DISTINCT id, razcli FROM tbclientes where fl_exclusao is not null and ativo_inativo = 'ATIVO' ORDER BY razcli", conn);

                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                cboRemetente.Items.Clear();                
                cboPagador.Items.Clear();

                cboRemetente.Items.Add(" Selecione...");               
                cboPagador.Items.Add(" Selecione...");

                while (dr.Read())
                {
                    cboRemetente.Items.Add(dr["razcli"].ToString());                    
                    cboPagador.Items.Add(dr["razcli"].ToString());
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
        private void CarregarCidades()
        {
            ddlCidade.Items.Clear();

            string sql = @"
                    SELECT DISTINCT 
                        cid_pagador + ' - ' + uf_pagador AS pagador_completo
                    FROM tbtabeladefretes
                    WHERE cid_pagador IS NOT NULL
                    ORDER BY pagador_completo";

            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                conn.Open();
                ddlCidade.DataSource = cmd.ExecuteReader();
                ddlCidade.DataTextField = "pagador_completo";
                ddlCidade.DataValueField = "pagador_completo";
                ddlCidade.DataBind();
            }

            ddlCidade.Items.Insert(0, new ListItem("Selecione...", ""));
        }
        protected void txtCodRemetente_TextChanged(object sender, EventArgs e)
        {
            if (txtCodRemetente.Text != "")
            {
                string cod = txtCodRemetente.Text;
                string sql = "SELECT codcli, razcli, cidcli, estcli, ativo_inativo, fl_exclusao,cnpj FROM tbclientes where codcli = '" + cod + "' and ativo_inativo = 'ATIVO' and fl_exclusao is null";
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
                        txtCodRemetente.Text = "";
                        txtCodRemetente.Focus();
                        return;
                    }
                    else if (dt.Rows[0][4].ToString() == "INATIVO")
                    {
                        // Acione o toast quando a página for carregada
                        string script = "<script>showToast('Cliente inativo no sistema.');</script>";
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                        txtCodRemetente.Text = "";
                        txtCodRemetente.Focus();
                        return;
                    }
                    else
                    {
                        cboRemetente.SelectedItem.Text = dt.Rows[0][1].ToString();
                        txtCNPJRemetente.Text = dt.Rows[0][6].ToString();
                        txtMunicipioRemetente.Text = dt.Rows[0][2].ToString();
                        txtUFRemetente.Text = dt.Rows[0][3].ToString();                        
                        return;
                    }

                }
                else
                {
                    // Acione o toast quando a página for carregada
                    string script = "<script>showToast('Cliente não encontrado no sistema.');</script>";
                    ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                    txtCodRemetente.Text = "";
                    txtCodRemetente.Focus();
                    return;
                }
            }

        }
        protected void cboRemetente_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idSelecionado = int.Parse(cboRemetente.SelectedValue);

            // Preencher os campos com base no valor selecionado
            if (idSelecionado > 0)
            {
                PreencherCamposRemetente(idSelecionado);
            }
            else
            {
                LimparCamposRemetente();
            }
        }
        // Função para preencher os campos com os dados do banco
        private void PreencherCamposRemetente(int id)
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                conn.Open();
                string query = "SELECT codcli, razcli, cidcli, estcli, cnpj FROM tbclientes WHERE codcli = @ID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtCodRemetente.Text = reader["codcli"].ToString();
                    txtMunicipioRemetente.Text = reader["cidcli"].ToString();
                    txtCNPJRemetente.Text = reader["cnpj"].ToString();
                    txtUFRemetente.Text = reader["estcli"].ToString();
                }
            }
        }
        // Função para limpar os campos
        private void LimparCamposRemetente()
        {
            txtCodRemetente.Text = string.Empty;
            cboRemetente.SelectedItem.Text = string.Empty;
            txtMunicipioRemetente.Text = string.Empty;
            txtUFRemetente.Text = string.Empty;
        }
        protected void txtCodPagador_TextChanged(object sender, EventArgs e)
        {
            if (txtCodPagador.Text != "")
            {
                string cod = txtCodPagador.Text;
                string sql = "SELECT codcli, razcli, cidcli, estcli, ativo_inativo, fl_exclusao, cnpj FROM tbclientes where codcli = '" + cod + "' and ativo_inativo = 'ATIVO' and fl_exclusao is null";
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
                        txtCNPJPagador.Text = dt.Rows[0][6].ToString();
                        // return;

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
                string query = "SELECT codcli, razcli, cidcli, estcli, cnpj FROM tbclientes WHERE codcli = @ID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtCodPagador.Text = reader["codcli"].ToString();
                    txtCNPJPagador.Text = reader["cnpj"].ToString();
                    txtCidPagador.Text = reader["cidcli"].ToString();
                    txtUFPagador.Text = reader["estcli"].ToString();               
                }
            }
        }
        // Função para limpar os campos
        private void LimparCamposPagador()
        {
            txtCodPagador.Text = string.Empty;
            cboPagador.SelectedItem.Text = string.Empty;
            txtCidPagador.Text = string.Empty;
            txtCNPJPagador.Text = string.Empty;
            txtUFPagador.Text = string.Empty;
        }
        protected void txtCodRota_TextChanged(object sender, EventArgs e)
        {
            if (txtCodRemetente.Text != "")
            {
                string cod = txtCodRemetente.Text;
                string sql = "SELECT rota, desc_rota, situacao, fl_exclusao FROM tbrotasdeentregas where rota = '" + cod + "' and situacao = 'ATIVO' and fl_exclusao is null";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                conn.Open();
                da.Fill(dt);
                conn.Close();

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0][3].ToString() == null)
                    {
                        // Acione o toast quando a página for carregada
                        string script = "<script>showToast('Rota deletada do sistema.');</script>";
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                        txtCodRota.Text = "";
                        txtCodRota.Focus();
                        return;
                    }
                    else if (dt.Rows[0][2].ToString() == "INATIVO")
                    {
                        // Acione o toast quando a página for carregada
                        string script = "<script>showToast('Rota inativa no sistema.');</script>";
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                        txtCodRota.Text = "";
                        txtCodRota.Focus();
                        return;
                    }
                    else
                    {
                        cboRota.SelectedItem.Text = dt.Rows[0][1].ToString();                        
                        return;
                    }

                }
                else
                {
                    // Acione o toast quando a página for carregada
                    string script = "<script>showToast('Rota não encontrada no sistema.');</script>";
                    ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                    txtCodRota.Text = "";
                    txtCodRota.Focus();
                    return;
                }
            }

        }
        protected void cboRota_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idSelecionado = int.Parse(cboRota.SelectedValue);

            // Preencher os campos com base no valor selecionado
            if (idSelecionado > 0)
            {
                PreencherCamposRota(idSelecionado);
            }
            else
            {
                LimparCamposRota();
            }
        }
        // Função para preencher os campos com os dados do banco
        private void PreencherCamposRota(int id)
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                conn.Open();
                string query = "SELECT rota, desc_rota FROM tbrotasdeentregas WHERE rota = @ID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtCodRota.Text = reader["rota"].ToString();                                      
                }
            }
        }
        // Função para limpar os campos
        private void LimparCamposRota()
        {
            txtCodRota.Text = string.Empty;
            cboRota.SelectedItem.Text = string.Empty;            
        }

    }
}