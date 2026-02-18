using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using MathNet.Numerics;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace NewCapit.dist.pages
{
    public partial class Frm_RotaKrona : System.Web.UI.Page
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["conexao"].ToString());
        string rota;
        string id;
        string idQuery;
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
                    Response.Redirect("Login.aspx");
                }
                CarregarClientes(cboExpedidor, cboRecebedor);
               

                //CarregarCombos();

            }

        }
        private void CarregarClientes(params DropDownList[] combos)
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT codcli, nomcli, cnpj, cidcli, estcli, ativo_inativo, fl_exclusao FROM tbclientes where ativo_inativo = 'ATIVO' and fl_exclusao is null ORDER BY nomcli";

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
                    ddl.DataTextField = "nomcli";   // texto exibido
                    ddl.DataValueField = "codcli";  // valor
                    ddl.DataBind();

                    ddl.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Selecione...", ""));
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
                        //txtCodDestinatario.Focus();
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
        protected void MostrarMsg(string mensagem, string tipo = "warning")
        {
            divMsg.Attributes["class"] = "alert alert-" + tipo + " alert-dismissible fade show mt-3";
            lblMsgGeral.InnerText = mensagem;
            divMsg.Style["display"] = "block";

            string script = @"setTimeout(function() {
                        var div = document.getElementById('divMsg');
                        if (div) div.style.display = 'none';
                      }, 5000);";

            ScriptManager.RegisterStartupScript(this, GetType(), "EscondeMsg", script, true);
        }

        protected void btnAtualizar_Click(object sender, EventArgs e)
        {
            // Validação visual simples antes de ir ao banco
            if (string.IsNullOrEmpty(txtIdKrona.Text) || string.IsNullOrEmpty(txtDescricaoRota.Text))
            {
                MostrarMsg("Por favor, preencha o ID e a Descrição.");
                return;
            }

            string resultado = InserirRota();

            if (resultado == "Sucesso")
            {
                MostrarMsg("Rota cadastrada com sucesso!","success");
                // LimparCampos(); // Opcional: criar um método para limpar a tela
            }
            else
            {
                // Exibe a mensagem de erro ou de duplicidade retornada pelo método
                MostrarMsg(resultado,"warning");
            }
        }

        private string InserirRota()
        {
            string strConn = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(strConn))
            {
                conn.Open();

                // 1. Validação de Duplicidade (id_rota ou descricao_rota)
                string sqlCheck = "SELECT COUNT(*) FROM tbrotaskrona WHERE id_rota = @id_rota OR descricao_rota = @descricao";
                using (SqlCommand cmdCheck = new SqlCommand(sqlCheck, conn))
                {
                    cmdCheck.Parameters.Add("@id_rota", SqlDbType.Int).Value = int.Parse(txtIdKrona.Text);
                    cmdCheck.Parameters.Add("@descricao", SqlDbType.VarChar).Value = txtDescricaoRota.Text;

                    int existe = (int)cmdCheck.ExecuteScalar();
                    if (existe > 0)
                    {
                        return "Já existe uma rota cadastrada com este ID ou Descrição.";
                    }
                }

                // 2. Query de Insert
                string query = @"
            INSERT INTO tbrotaskrona (
                id_rota, descricao_rota, cod_expedidor_rota, expedidor_rota, cnpj_expedidor, 
                cid_expedidor, uf_expedidor, cod_recebedor_rota, recebedor_rota, 
                cnpj_recebedor, cid_recebedor, uf_recebedor
            ) VALUES (
                @id_rota, @descricao, @codExp, @expedidor, @cnpjExp, 
                @cidExp, @ufExp, @codRec, @recebedor, 
                @cnpjRec, @cidRec, @ufRec
            )";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // Mapeamento dos parâmetros
                    cmd.Parameters.Add("@id_rota", SqlDbType.Int).Value = int.Parse(txtIdKrona.Text);
                    cmd.Parameters.Add("@descricao", SqlDbType.VarChar).Value = txtDescricaoRota.Text;

                    cmd.Parameters.Add("@codExp", SqlDbType.VarChar).Value = txtCodExpedidor.Text;
                    cmd.Parameters.Add("@expedidor", SqlDbType.VarChar).Value = cboExpedidor.SelectedItem.Text;
                    cmd.Parameters.Add("@cnpjExp", SqlDbType.VarChar).Value = txtCNPJExpedidor.Text;
                    cmd.Parameters.Add("@cidExp", SqlDbType.VarChar).Value = txtCidExpedidor.Text;
                    cmd.Parameters.Add("@ufExp", SqlDbType.VarChar).Value = txtUFExpedidor.Text;

                    cmd.Parameters.Add("@codRec", SqlDbType.VarChar).Value = txtCodRecebedor.Text;
                    cmd.Parameters.Add("@recebedor", SqlDbType.VarChar).Value = cboRecebedor.SelectedItem.Text;
                    cmd.Parameters.Add("@cnpjRec", SqlDbType.VarChar).Value = txtCNPJRecebedor.Text;
                    cmd.Parameters.Add("@cidRec", SqlDbType.VarChar).Value = txtCidRecebedor.Text;
                    cmd.Parameters.Add("@ufRec", SqlDbType.VarChar).Value = txtUFRecebedor.Text;

                    cmd.ExecuteNonQuery();
                    return "Sucesso";
                }
            }
        }
    }
}