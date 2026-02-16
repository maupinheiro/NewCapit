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
    public partial class Frm_EditarRotaKrona : System.Web.UI.Page
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["conexao"].ToString());
        string rota;
        string id;
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
                CarregarRota();
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
        //private void CarregarCombos()
        //{
        //    cboExpedidor.DataSource = BuscarClientes();
        //    cboExpedidor.DataTextField = "nome";
        //    cboExpedidor.DataValueField = "codigo";
        //    cboExpedidor.DataBind();

        //    cboRecebedor.DataSource = BuscarClientes();
        //    cboRecebedor.DataTextField = "nome";
        //    cboRecebedor.DataValueField = "codigo";
        //    cboRecebedor.DataBind();

        //    cboExpedidor.Items.Insert(0, new ListItem("-- selecione --", ""));
        //    cboRecebedor.Items.Insert(0, new ListItem("-- selecione --", ""));
        //}
        private void CarregarRota()
        {
            string idQuery = Request.QueryString["id_rota"];

            if (!int.TryParse(idQuery, out int id_rota))
                return;

            string strConn = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(strConn))
            {
                string query = @"
            SELECT id, id_rota, descricao_rota,
                   cod_expedidor_rota, expedidor_rota, cnpj_expedidor,
                   cid_expedidor, uf_expedidor,
                   cod_recebedor_rota, recebedor_rota, cnpj_recebedor,
                   cid_recebedor, uf_recebedor
            FROM tbrotaskrona
            WHERE id_rota = @id_rota";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@id_rota", SqlDbType.Int).Value = id_rota;

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtIdKrona.Text = reader["id_rota"].ToString();
                            txtDescricaoRota.Text = reader["descricao_rota"].ToString();

                            txtCodExpedidor.Text = reader["cod_expedidor_rota"].ToString();
                            cboExpedidor.SelectedValue = reader["cod_expedidor_rota"].ToString();
                            txtCNPJExpedidor.Text = reader["cnpj_expedidor"].ToString();
                            txtCidExpedidor.Text = reader["cid_expedidor"].ToString();
                            txtUFExpedidor.Text = reader["uf_expedidor"].ToString();

                            txtCodRecebedor.Text = reader["cod_recebedor_rota"].ToString();
                            cboRecebedor.SelectedValue = reader["cod_recebedor_rota"].ToString();
                            txtCNPJRecebedor.Text = reader["cnpj_recebedor"].ToString();
                            txtCidRecebedor.Text = reader["cid_recebedor"].ToString();
                            txtUFRecebedor.Text = reader["uf_recebedor"].ToString();
                        }
                    }
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
        
    }
}