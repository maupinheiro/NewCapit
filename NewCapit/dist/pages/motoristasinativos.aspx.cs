using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;


namespace NewCapit.dist.pages
{
    public partial class motoristasinativos : System.Web.UI.Page
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["conexao"].ToString());
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
                txtDias.Text = "30";
                CarregarFiliais();
            }
        }
        private void CarregarFiliais()
        {
            ddlFilial.Items.Clear();
            ddlFilial.Items.Add(new ListItem("Todos", ""));
            using (SqlConnection conn = new SqlConnection(
               ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                string sql = @"SELECT descricao
                       FROM tbempresa where status = 'ATIVA'
                       ORDER BY descricao";
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    ddlFilial.Items.Add(
                        new ListItem(
                            dr["descricao"].ToString(),
                            dr["descricao"].ToString()
                     ));
                }
            }
        }
        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            PesquisarMotoristas();
        }
        private void PesquisarMotoristas()
        {
            using (SqlConnection conn = new SqlConnection(
               ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(@"
                    SELECT
                    m.codmot,
                    m.nommot,
                    m.tipomot,
                    m.nucleo,
                    MAX(v.data) UltimaViagem,
                    ISNULL(DATEDIFF(DAY,MAX(v.data),GETDATE()),99999) Dias
                    FROM tbmotoristas m
                    LEFT JOIN tbmotoristas_viagens v
                    ON m.codmot=v.cod_motorista
                    WHERE
                    LTRIM(RTRIM(ISNULL(m.status,''))) = 'ATIVO'
                    AND LTRIM(RTRIM(ISNULL(m.tipomot,''))) <> 'FUNCIONÁRIO'
                    ");
                if (ddlFilial.SelectedIndex > 0)
                {
                    sql.Append(" AND m.nucleo=@filial ");
                }
                sql.Append(@"
                    GROUP BY
                    m.codmot,
                    m.nommot,
                    m.tipomot,
                    m.nucleo
                    HAVING
                    ISNULL(DATEDIFF(DAY,MAX(v.data),GETDATE()),99999)>=@dias
                    ORDER BY
                    Dias DESC
                    ");
                SqlDataAdapter da = new SqlDataAdapter(sql.ToString(), conn);
                da.SelectCommand.Parameters.AddWithValue("@dias",
                Convert.ToInt32(txtDias.Text));
                if (ddlFilial.SelectedIndex > 0)
                {
                    da.SelectCommand.Parameters.AddWithValue("@filial",
                    ddlFilial.SelectedValue);
                }
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvMotoristas.DataSource = dt;
                gvMotoristas.DataBind();
                lblQuantidade.Text = "Motoristas encontrados : " + dt.Rows.Count;

            }
        }
        protected void btnInativar_Click(object sender, EventArgs e)
        {
            InativarMotoristas();           
        } 
        private void InativarMotoristas()
        {
            DataTable dtMotoristas = new DataTable();
            dtMotoristas.Columns.Add("codmot", typeof(string));
            dtMotoristas.Columns.Add("dias", typeof(int));
            foreach (GridViewRow row in gvMotoristas.Rows)
            {
                CheckBox chk =
                    (CheckBox)row.FindControl("chkSelecionar");
                if (chk != null && chk.Checked)
                {
                    string codmot =
                        gvMotoristas.DataKeys[row.RowIndex]
                        .Values["codmot"]
                        .ToString();
                    int dias =
                        Convert.ToInt32(
                            gvMotoristas.DataKeys[row.RowIndex]
                            .Values["Dias"]);
                    dtMotoristas.Rows.Add(
                        codmot,
                        dias);
                }
            }
            if (dtMotoristas.Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(
                    this,
                    GetType(),
                    "msg",
                    "alert('Selecione pelo menos um motorista.');",
                    true);
                return;
            }
            ExecutarInativacao(dtMotoristas);
        }

        private void ExecutarInativacao(DataTable dtMotoristas)
        {
            using (SqlConnection conn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                conn.Open();
                SqlTransaction trans =
                    conn.BeginTransaction();
                try
                {
                    SqlCommand cmd =
                        new SqlCommand(
                            "sp_InativarMotoristas",
                            conn,
                            trans);
                    cmd.CommandType =
                        CommandType.StoredProcedure;
                    SqlParameter parametro =
                        cmd.Parameters.AddWithValue(
                            "@Motoristas",
                            dtMotoristas);
                    parametro.SqlDbType =
                        SqlDbType.Structured;
                    parametro.TypeName =
                        "dbo.ListaMotoristas";
                    cmd.Parameters.AddWithValue(
                        "@Usuario",
                        Session["UsuarioLogado"].ToString());
                    int total =
                        Convert.ToInt32(
                            cmd.ExecuteScalar());
                    trans.Commit();
                    ScriptManager.RegisterStartupScript(
                        this,
                        GetType(),
                        "ok",
                        $"alert('{total} motorista(s) inativado(s) com sucesso.');",
                        true);
                    PesquisarMotoristas();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    ScriptManager.RegisterStartupScript(
                        this,
                        GetType(),
                        "erro",
                        $"alert('{ex.Message.Replace("'", "")}');",
                        true);
                }

            }

        }
    }
}

