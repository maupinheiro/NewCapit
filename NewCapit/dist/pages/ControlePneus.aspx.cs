using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace NewCapit.dist.pages
{
    public partial class ControlePneus : System.Web.UI.Page
    {
        string conexao = WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
        DateTime dataHoraAtual = DateTime.Now;
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
                    Response.Redirect("Login.aspx");
                }
                //txtAdmissao.Text = dataHoraAtual.ToString("dd/MM/yyyy");
                CarregarGrid();

            }

        }
        void CarregarGrid()
        {
            using (SqlConnection conn = new SqlConnection(conexao))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbPneus WHERE fl_exclusao IS NULL", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvPneus.DataSource = dt;
                gvPneus.DataBind();
            }
        }
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            bool erro = false;

            if (string.IsNullOrEmpty(txtNumero.Text)) { txtNumero.CssClass += " erro"; erro = true; }
            if (string.IsNullOrEmpty(txtMarca.Text)) { txtMarca.CssClass += " erro"; erro = true; }
            if (string.IsNullOrEmpty(txtKM.Text)) { txtKM.CssClass += " erro"; erro = true; }

            if (erro)
            {
                lblErro.Text = "Preencha os campos obrigatórios";
                ScriptManager.RegisterStartupScript(this, GetType(), "modal", "abrirModal();", true);
                return;
            }
            string numero = txtNumero.Text.Trim();
            using (SqlConnection conn = new SqlConnection(conexao))
            {
                conn.Open();

                // 1. Verifica se já existe
                SqlCommand cmdVerifica = new SqlCommand(
                    "SELECT COUNT(*) FROM tbpneus WHERE numero = @numero", conn);

                cmdVerifica.Parameters.AddWithValue("@numero", numero);

                int existe = (int)cmdVerifica.ExecuteScalar();

                if (existe > 0)
                {
                    // Já existe → bloqueia
                    lblErro.Text = "Este número já está cadastrado!";
                    lblErro.ForeColor = System.Drawing.Color.Red;
                    txtNumero.Focus();
                    return;
                }


                SqlCommand cmd;

                if (string.IsNullOrEmpty(hfId.Value))
                {
                    cmd = new SqlCommand("INSERT INTO tbPneus (Numero,Marca,Modelo,Medida,KMAtual,Status) VALUES (@n,@m,@mo,@me,@km,@s)", conn);
                }
                else
                {
                    cmd = new SqlCommand("UPDATE tbPneus SET Numero=@n,Marca=@m,Modelo=@mo,Medida=@me,KMAtual=@km,Status=@s WHERE Id=@id", conn);
                    cmd.Parameters.AddWithValue("@id", hfId.Value);
                }

                cmd.Parameters.AddWithValue("@n", txtNumero.Text);
                cmd.Parameters.AddWithValue("@m", txtMarca.Text);
                cmd.Parameters.AddWithValue("@mo", txtModelo.Text);
                cmd.Parameters.AddWithValue("@me", txtMedida.Text);
                cmd.Parameters.AddWithValue("@km", txtKM.Text);
                cmd.Parameters.AddWithValue("@s", ddlStatus.SelectedValue);

                cmd.ExecuteNonQuery();
            }

            Limpar();
            CarregarGrid();
        }
        protected void gvPneus_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "Editar")
            {
                using (SqlConnection conn = new SqlConnection(conexao))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM tbPneus WHERE Id=@id", conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        hfId.Value = dr["Id"].ToString();
                        txtNumero.Text = dr["Numero"].ToString();
                        txtMarca.Text = dr["Marca"].ToString();
                        txtModelo.Text = dr["Modelo"].ToString();
                        txtMedida.Text = dr["Medida"].ToString();
                        txtKM.Text = dr["KMAtual"].ToString();
                        ddlStatus.SelectedValue = dr["Status"].ToString();
                    }
                }

                ScriptManager.RegisterStartupScript(this, GetType(), "modal", "abrirModal();", true);
            }
            else if (e.CommandName == "Excluir")
            {
                using (SqlConnection conn = new SqlConnection(conexao))
                {
                    conn.Open();
                    //SqlCommand cmd = new SqlCommand("DELETE FROM tbPneus WHERE Id=@id", conn);
                    //cmd.Parameters.AddWithValue("@id", id);
                    //cmd.ExecuteNonQuery();
                    string sql = @"UPDATE tbpneus SET
                            resp_exclusao=@resp_exclusao, status=@status
                            WHERE id=@id";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id", e.CommandArgument);
                    cmd.Parameters.AddWithValue("@resp_exclusao", dataHoraAtual.ToString("dd/MM/yyyy HH:mm") + " - " + Session["UsuarioLogado"]);                    
                    cmd.Parameters.AddWithValue("@status", "Descartado");
                    cmd.ExecuteNonQuery();
                }

                CarregarGrid();
            }
            else if (e.CommandName == "Movimentar")
            {
                // Abre modal de movimentação
                hfPneuMov.Value = id.ToString();

                // Carrega veículos no dropdown
                using (SqlConnection conn = new SqlConnection(conexao))
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT id, codvei, plavei FROM tbVeiculos", conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    ddlVeiculo.DataSource = dt;
                    ddlVeiculo.DataTextField = "plavei";
                    ddlVeiculo.DataValueField = "id";
                    ddlVeiculo.DataBind();
                }

                ScriptManager.RegisterStartupScript(this, GetType(), "modalMov", "var modal = new bootstrap.Modal(document.getElementById('modalMov')); modal.show();", true);
            }


        }       
        void Limpar()
        {
            hfId.Value = "";
            txtNumero.Text = "";
            txtMarca.Text = "";
            txtModelo.Text = "";
            txtMedida.Text = "";
            txtKM.Text = "";
            ddlStatus.SelectedIndex = 0;
            lblErro.Text = "";
        }
        protected void Mensagem(string tipo, string texto)
        {
            divMsg.Visible = true;

            divMsg.Attributes["class"] =
                "alert alert-" + tipo + " alert-dismissible fade show mt-3";

            lblMsgGeral.Text = texto;
        }
        protected void SalvarMovimentacao(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(conexao))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(@"INSERT INTO tbMovimentacaoPneu
        (IdPneu,IdVeiculo,Posicao,TipoMovimentacao,KM)
        VALUES (@p,@v,@pos,'Instalação',@km)", conn);

                cmd.Parameters.AddWithValue("@p", hfPneuMov.Value);
                cmd.Parameters.AddWithValue("@v", ddlVeiculo.SelectedValue);
                cmd.Parameters.AddWithValue("@pos", ddlPosicao.SelectedValue);
                cmd.Parameters.AddWithValue("@km", txtKMMov.Text);

                cmd.ExecuteNonQuery();

                // atualiza KM do pneu
                SqlCommand cmd2 = new SqlCommand("UPDATE tbPneus SET KMAtual=@km WHERE Id=@id", conn);
                cmd2.Parameters.AddWithValue("@km", txtKMMov.Text);
                cmd2.Parameters.AddWithValue("@id", hfPneuMov.Value);
                cmd2.ExecuteNonQuery();
            }
        }
        protected void SalvarRecapagem(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(conexao))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(@"INSERT INTO tbRecapagem
        (IdPneu,Custo,KM,DataRecapagem)
        VALUES (@p,@c,@km,GETDATE())", conn);

                cmd.Parameters.AddWithValue("@p", hfPneuRecap.Value);
                cmd.Parameters.AddWithValue("@c", txtCusto.Text);
                cmd.Parameters.AddWithValue("@km", txtKMRecap.Text);

                cmd.ExecuteNonQuery();
            }
        }

    }
}