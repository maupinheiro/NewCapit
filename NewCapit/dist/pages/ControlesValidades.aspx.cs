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
            WHERE tipvei = 'FROTA'
            AND 
                codvei LIKE '%' + @pesquisa + '%'
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

            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))

            {
                SqlCommand cmd = new SqlCommand(@"
            UPDATE tbveiculos SET
                venclicencacet = @venclicencacet,
                venclicenciamento = @venclicenciamento,
                venccronotacografo = @venccronotacografo,
                protocolocet = @protocolocet,       
                ativo_inativo = @ativo
            WHERE id = @id", conn);

                cmd.Parameters.AddWithValue("@id", id);
                //cmd.Parameters.AddWithValue("@nucleo", ((TextBox)row.Cells[0].Controls[0]).Text);
                //cmd.Parameters.AddWithValue("@codvei", ((TextBox)row.Cells[1].Controls[0]).Text);
                //cmd.Parameters.AddWithValue("@plavei", ((TextBox)row.Cells[3].Controls[0]).Text);
                cmd.Parameters.AddWithValue("@venclicencacet", DateTime.Parse(((TextBox)row.Cells[0].Controls[0]).Text));
                cmd.Parameters.AddWithValue("@protocolocet", NovoTexto(row, "txtProtocoloCet") ?? (object)DBNull.Value);

                cmd.Parameters.AddWithValue("@venclicenciamento", DateTime.Parse(((TextBox)row.Cells[2].Controls[0]).Text));
                cmd.Parameters.AddWithValue("@venccronotacografo", DateTime.Parse(((TextBox)row.Cells[3].Controls[0]).Text));
               
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            gvVeiculos.EditIndex = -1;
            CarregarGrid();
        }

        protected void gvVeiculos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
                return;

            object dataItem = e.Row.DataItem;

            // Datas vindas direto do banco
            DateTime? vencCet = GetData(dataItem, "venclicencacet");
            DateTime? vencLic = GetData(dataItem, "venclicenciamento");
            DateTime? vencTac = GetData(dataItem, "venccronotacografo");
            

            // 👉 MODO VISUAL (cores / ícones)
            PintarData(e.Row, vencCet, "lblIconCet", "lblDataCet");
            PintarData(e.Row, vencLic, "lblIconLic", "lblDataLic");
            PintarData(e.Row, vencTac, "lblIconTac", "lblDataTac");
            

            // 👉 MODO EDIÇÃO (habilitar/desabilitar)
            if ((e.Row.RowState & DataControlRowState.Edit) > 0)
            {
                ControlarEdicaoData(e.Row, vencCet, "txtDataCet");
                ControlarEdicaoData(e.Row, vencLic, "txtDataLic");
                ControlarEdicaoData(e.Row, vencTac, "txtDataTac");
                
                ControlarEdicaoProtocoloCet(e.Row, vencCet);
            }
        }

        private bool DataVencendoOuVencida(DateTime data)
        {
            int dias = (data - DateTime.Today).Days;
            return dias <= 30;
        }
        private DateTime? GetData(object dataItem, string campo)
        {
            object valor = DataBinder.Eval(dataItem, campo);
            if (valor == null || valor == DBNull.Value)
                return null;

            return Convert.ToDateTime(valor);
        }
        private void PintarData(GridViewRow row, DateTime? data,
    string lblIconId, string lblDataId)
        {
            if (!data.HasValue) return;

            DateTime hoje = DateTime.Today;
            DateTime venc = data.Value.Date;

            int dias = (venc - hoje).Days;

            Label lblIcon = row.FindControl(lblIconId) as Label;
            Label lblData = row.FindControl(lblDataId) as Label;

            if (lblIcon == null || lblData == null) return;

            TableCell cell = lblData.NamingContainer as TableCell;
            if (cell == null) return;

            if (dias < 0)
            {
                lblIcon.Text = "<i class='fa fa-times-circle text-danger'></i> ";
                cell.BackColor = System.Drawing.Color.LightCoral;
            }
            else if (dias <= 30)
            {
                lblIcon.Text = "<i class='fa fa-exclamation-triangle text-warning'></i> ";
                cell.BackColor = System.Drawing.Color.Khaki;
            }
        }

        private bool DataVencendoOuVencida(DateTime? data)
        {
            if (!data.HasValue) return false;
            return (data.Value - DateTime.Today).Days <= 30;
        }

        private void ControlarEdicaoData(GridViewRow row, DateTime? data, string txtId)
        {
            TextBox txt = row.FindControl(txtId) as TextBox;
            if (txt == null) return;

            txt.Enabled = DataVencendoOuVencida(data);
            txt.CssClass = txt.Enabled
                ? "form-control"
                : "form-control bg-light";
        }
        private void ControlarEdicaoProtocoloCet(GridViewRow row, DateTime? vencCet)
        {
            TextBox txt = row.FindControl("txtProtocoloCet") as TextBox;
            if (txt == null) return;

            txt.Enabled = DataVencendoOuVencida(vencCet);
            txt.CssClass = txt.Enabled
                ? "form-control"
                : "form-control bg-light";
        }




        private void ControlarEdicaoData(GridViewRow row, string lblId, string txtId)
        {
            Label lbl = row.FindControl(lblId) as Label;
            TextBox txt = row.FindControl(txtId) as TextBox;

            if (lbl == null || txt == null)
            {
                return;
            }

            if (!DateTime.TryParse(lbl.Text, out DateTime venc))
            {
                txt.Enabled = false;
                return;
            }

            txt.Enabled = DataVencendoOuVencida(venc);
            txt.CssClass = txt.Enabled
                ? "form-control"
                : "form-control bg-light";
        }

        private void ControlarEdicaoProtocoloCet(GridViewRow row)
        {
            Label lblDataCet = row.FindControl("lblDataCet") as Label;
            TextBox txtProtocolo = row.FindControl("txtProtocoloCet") as TextBox;

            if (lblDataCet == null || txtProtocolo == null)
                return;

            if (!DateTime.TryParse(lblDataCet.Text, out DateTime vencCet))
            {
                txtProtocolo.Enabled = false;
                return;
            }

            txtProtocolo.Enabled = DataVencendoOuVencida(vencCet);
            txtProtocolo.CssClass = txtProtocolo.Enabled
                ? "form-control"
                : "form-control bg-light";
        }
        string NovoTexto(GridViewRow row, string txtId)
        {
            TextBox txt = row.FindControl(txtId) as TextBox;
            return (txt != null && txt.Enabled)
                ? txt.Text
                : null;
        }



        //private void TratarVencimento(GridViewRow row, string lblDataId, string lblIconId)
        //{
        //    Label lblData = row.FindControl(lblDataId) as Label;
        //    Label lblIcon = row.FindControl(lblIconId) as Label;

        //    if (lblData == null || lblIcon == null) return;
        //    if (!DateTime.TryParse(lblData.Text, out DateTime venc)) return;

        //    int dias = (venc - DateTime.Today).Days;

        //    if (dias < 0)
        //    {
        //        lblIcon.Text = "❌ ";
        //        row.BackColor = System.Drawing.Color.LightCoral;
        //        lblIcon.ToolTip = "Documento vencido";
        //    }
        //    else if (dias <= 30)
        //    {
        //        lblIcon.Text = "⚠️ ";
        //        row.BackColor = System.Drawing.Color.Khaki;
        //        lblIcon.ToolTip = "Vencimento próximo";
        //    }
        //}




    }
}