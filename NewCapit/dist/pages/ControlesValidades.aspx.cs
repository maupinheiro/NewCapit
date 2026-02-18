using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Configuration;

namespace NewCapit.dist.pages
{
    public partial class ControlesValidades : System.Web.UI.Page
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["conexao"].ToString());
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                CarregarGrid();
        }
        private void CarregarGrid()
        {

            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))

            {
                SqlCommand cmd = new SqlCommand(@"
            SELECT *
            FROM tbveiculos
            WHERE tipoveiculo = 'FROTA'
            AND ativo_inativo = 'ATIVO'
            AND codvei LIKE '%' + @pesquisa + '%'
                OR plavei LIKE '%' + @pesquisa + '%'
                OR nucleo LIKE '%' + @pesquisa + '%'
                OR protocolocet LIKE '%' + @pesquisa + '%'            
            ", conn);

                cmd.Parameters.AddWithValue("@pesquisa", txtPesquisa.Text.Trim());

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvVeiculos.DataSource = dt;
                gvVeiculos.DataBind();
            }
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            CarregarGrid();
        }
        protected void gvVeiculos_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvVeiculos.EditIndex = e.NewEditIndex;          
            CarregarGrid();
        }

        protected void gvVeiculos_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvVeiculos.EditIndex = -1;
            CarregarGrid();
        }

        protected void gvVeiculos_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int id = Convert.ToInt32(gvVeiculos.DataKeys[e.RowIndex].Value);
            GridViewRow row = gvVeiculos.Rows[e.RowIndex];

            TextBox txtVencLic = (TextBox)row.FindControl("txtVencLicenciamento");
            TextBox txtCrono = (TextBox)row.FindControl("txtVencCronotacografo");
            TextBox txtLaudo = (TextBox)row.FindControl("txtVencLaudoFumaca");
            TextBox txtCet = (TextBox)row.FindControl("txtVencLicencaCet");
            TextBox txtProtocolo = (TextBox)row.FindControl("txtProtocoloCet");

            using (SqlConnection conn = new SqlConnection(
                WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                SqlCommand cmd = new SqlCommand(@"
        UPDATE tbveiculos SET                
            venclicenciamento = @venclicenciamento,
            venccronotacografo = @venccronotacografo,
            vencimentolaudofumaca = @vencimentolaudofumaca,
            venclicencacet = @venclicencacet,
            protocolocet = @protocolocet 
        WHERE id = @id", conn);

                cmd.Parameters.AddWithValue("@id", id);

                cmd.Parameters.AddWithValue("@venclicenciamento",
                    string.IsNullOrWhiteSpace(txtVencLic.Text)
                        ? (object)DBNull.Value
                        : DateTime.ParseExact(txtVencLic.Text, "dd/MM/yyyy",
                            System.Globalization.CultureInfo.InvariantCulture));

                cmd.Parameters.AddWithValue("@venccronotacografo",
                    string.IsNullOrWhiteSpace(txtCrono.Text)
                        ? (object)DBNull.Value
                        : DateTime.ParseExact(txtCrono.Text, "dd/MM/yyyy",
                            System.Globalization.CultureInfo.InvariantCulture));

                cmd.Parameters.AddWithValue("@vencimentolaudofumaca",
                    string.IsNullOrWhiteSpace(txtLaudo.Text)
                        ? (object)DBNull.Value
                        : DateTime.ParseExact(txtLaudo.Text, "dd/MM/yyyy",
                            System.Globalization.CultureInfo.InvariantCulture));

                cmd.Parameters.AddWithValue("@venclicencacet",
                    string.IsNullOrWhiteSpace(txtCet.Text)
                        ? (object)DBNull.Value
                        : DateTime.ParseExact(txtCet.Text, "dd/MM/yyyy",
                            System.Globalization.CultureInfo.InvariantCulture));

                cmd.Parameters.AddWithValue("@protocolocet",
                    string.IsNullOrWhiteSpace(txtProtocolo.Text)
                        ? (object)DBNull.Value
                        : txtProtocolo.Text);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            gvVeiculos.EditIndex = -1;
            CarregarGrid();

        }

        private void HabilitarSeVencido(GridViewRow row, string idTextBox, string nomeCampo, DateTime hoje)
        {
            TextBox txt = (TextBox)row.FindControl(idTextBox);

            object dataObj = DataBinder.Eval(row.DataItem, nomeCampo);

            if (dataObj != DBNull.Value && dataObj != null)
            {
                DateTime data = Convert.ToDateTime(dataObj);

                // Vencido OU vencendo em 30 dias
                if (data <= hoje.AddDays(30))
                {
                    txt.Enabled = true;
                    txt.BackColor = System.Drawing.Color.MistyRose; // opcional destaque
                }
            }
        }


        protected void gvVeiculos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow &&
        (e.Row.RowState & DataControlRowState.Edit) > 0)
            {
                DateTime hoje = DateTime.Today;

                DateTime? lic = DataBinder.Eval(e.Row.DataItem, "venclicenciamento") as DateTime?;
                DateTime? crono = DataBinder.Eval(e.Row.DataItem, "venccronotacografo") as DateTime?;
                DateTime? laudo = DataBinder.Eval(e.Row.DataItem, "vencimentolaudofumaca") as DateTime?;
                DateTime? cet = DataBinder.Eval(e.Row.DataItem, "venclicencacet") as DateTime?;

                HabilitarSeVencendo(e.Row, "txtVencLicenciamento", lic, hoje);
                HabilitarSeVencendo(e.Row, "txtVencCronotacografo", crono, hoje);
                HabilitarSeVencendo(e.Row, "txtVencLaudoFumaca", laudo, hoje);

                bool cetVencido = EstaVencendo(cet, hoje);

                // CET
                HabilitarSeVencendo(e.Row, "txtVencLicencaCet", cet, hoje);

                // 🔥 PROTOCOLO só habilita se CET estiver vencido
                TextBox txtProtocolo = (TextBox)e.Row.FindControl("txtProtocoloCet");
                if (txtProtocolo != null)
                {
                    txtProtocolo.Enabled = cetVencido;
                }
            }

            if (e.Row.RowType == DataControlRowType.DataRow &&
        (e.Row.RowState & DataControlRowState.Edit) == 0)
            {
                DateTime hoje = DateTime.Today;

                DateTime? lic = DataBinder.Eval(e.Row.DataItem, "venclicenciamento") as DateTime?;
                DateTime? crono = DataBinder.Eval(e.Row.DataItem, "venccronotacografo") as DateTime?;
                DateTime? laudo = DataBinder.Eval(e.Row.DataItem, "vencimentolaudofumaca") as DateTime?;
                DateTime? cet = DataBinder.Eval(e.Row.DataItem, "venclicencacet") as DateTime?;

                bool temVencido =
                    EstaVencendo(lic, hoje) ||
                    EstaVencendo(crono, hoje) ||
                    EstaVencendo(laudo, hoje) ||
                    EstaVencendo(cet, hoje);

                LinkButton btnEditar = (LinkButton)e.Row.FindControl("btnEditar");

                if (!temVencido && btnEditar != null)
                {
                    btnEditar.Visible = false;
                }
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                object dataObjLicenciamento = DataBinder.Eval(e.Row.DataItem, "venclicenciamento"); //4
                object dataObjCrono = DataBinder.Eval(e.Row.DataItem, "venccronotacografo"); //5
                object dataObjOpacidade = DataBinder.Eval(e.Row.DataItem, "vencimentolaudofumaca"); //6
                object dataObjCet = DataBinder.Eval(e.Row.DataItem, "venclicencacet"); //7

                if (dataObjLicenciamento != DBNull.Value && dataObjLicenciamento != null)
                {
                    DateTime dataVencLicenciamento = Convert.ToDateTime(dataObjLicenciamento);
                    int dias = (dataVencLicenciamento - DateTime.Now).Days;

                    if (dias <= 30 && dias >= 0)
                    {
                        e.Row.Cells[4].BackColor = System.Drawing.Color.Orange;
                    }

                    if (dias < 0)
                    {
                        e.Row.Cells[4].BackColor = System.Drawing.Color.Red;
                        e.Row.Cells[4].ForeColor = System.Drawing.Color.White;
                    }
                }
                if (dataObjCrono != DBNull.Value && dataObjCrono != null)
                {
                    DateTime dataVencCrono = Convert.ToDateTime(dataObjCrono);
                    int dias = (dataVencCrono - DateTime.Now).Days;

                    if (dias <= 30 && dias >= 0)
                    {
                        e.Row.Cells[5].BackColor = System.Drawing.Color.Orange;
                    }

                    if (dias < 0)
                    {
                        e.Row.Cells[5].BackColor = System.Drawing.Color.Red;
                        e.Row.Cells[5].ForeColor = System.Drawing.Color.White;
                    }
                }
                if (dataObjOpacidade != DBNull.Value && dataObjOpacidade != null)
                {
                    DateTime dataVencOpacidade = Convert.ToDateTime(dataObjOpacidade);
                    int dias = (dataVencOpacidade - DateTime.Now).Days;

                    if (dias <= 30 && dias >= 0)
                    {
                        e.Row.Cells[6].BackColor = System.Drawing.Color.Orange;
                    }

                    if (dias < 0)
                    {
                        e.Row.Cells[6].BackColor = System.Drawing.Color.Red;
                        e.Row.Cells[6].ForeColor = System.Drawing.Color.White;
                    }
                }
                if (dataObjCet != DBNull.Value && dataObjCet != null)
                {
                    DateTime dataVencCet = Convert.ToDateTime(dataObjCet);
                    int dias = (dataVencCet - DateTime.Now).Days;

                    if (dias <= 30 && dias >= 0)
                    {
                        e.Row.Cells[7].BackColor = System.Drawing.Color.Orange;
                    }

                    if (dias < 0)
                    {
                        e.Row.Cells[7].BackColor = System.Drawing.Color.Red;
                        e.Row.Cells[7].ForeColor = System.Drawing.Color.White;
                    }
                }
            }
        }
       
        private bool EstaVencendo(DateTime? data, DateTime hoje)
        {
            if (!data.HasValue)
                return false;

            return data.Value <= hoje.AddDays(30);
        }

        private void HabilitarSeVencendo(GridViewRow row, string idTextBox, DateTime? data, DateTime hoje)
        {
            TextBox txt = (TextBox)row.FindControl(idTextBox);

            if (txt == null)
                return;

            txt.Enabled = false; // padrão desabilitado

            if (EstaVencendo(data, hoje))
            {
                txt.Enabled = true;
                txt.BackColor = System.Drawing.Color.MistyRose; // opcional
            }
        }




    }
}