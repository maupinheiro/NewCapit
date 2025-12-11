using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Web.Services;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Text;


namespace NewCapit.dist.pages
{
    public partial class GerarTabelaDeAvaliacaoMotoristas : System.Web.UI.Page
    {
        SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString());
        string nomeUsuario = string.Empty;
        string nomeMes;
        
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
                CarregarNucleo();
                //   CarregarMotoristas();
                this.btnGerarPlanilha.Visible = false;
                this.btnImpmrimeMotoristas.Visible = false;
            }

        }


        private void CarregarNucleo()
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                string query = "SELECT DISTINCT nucleo FROM tbmotoristas ORDER BY nucleo";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                ddlStatus.DataSource = dr;
                ddlStatus.DataTextField = "nucleo";
                ddlStatus.DataValueField = "nucleo";
                ddlStatus.DataBind();
            }
        }
        private void CarregarMotoristas(string[] statusSelecionados = null)
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                string query = @"
                                SELECT 
                                    a.id, m.caminhofoto, m.codmot, m.nommot, m.cargo, m.tipomot, 
                                    m.cadmot, m.frota, m.nucleo, a.vl_total 
                                FROM tbmotoristas AS m
                                INNER JOIN tbavaliacaomotorista AS a ON m.codmot = a.cracha WHERE a.mes = @mes";

                if (statusSelecionados != null && statusSelecionados.Length > 0)
                {
                    string filtros = string.Join(",", statusSelecionados.Select((s, i) => "@status" + i));

                    query += $" and m.nucleo IN ({filtros})"
                          + " AND m.status = 'ATIVO'"
                          + " AND ISNUMERIC(m.codmot) = 1";
                        
                }

                SqlCommand cmd = new SqlCommand(query, conn);

                // Adiciona parâmetros dos núcleos selecionados
                if (statusSelecionados != null)
                {
                    for (int i = 0; i < statusSelecionados.Length; i++)
                        cmd.Parameters.AddWithValue("@status" + i, statusSelecionados[i]);
                }

                // Monta o valor no formato correto "MM/YYYY"
                string mesAno = ddlMes.SelectedValue.PadLeft(2, '0') + "/" + ddlAno.SelectedValue;

                // Adiciona parâmetro do mês/ano
                cmd.Parameters.AddWithValue("@mes", mesAno);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvMotoristas.DataSource = dt;
                gvMotoristas.DataBind();

                this.Botoes();

            }
        }
        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ddlMes.SelectedValue))
            {
                // Acione o toast quando a página for carregada
                string script = "<script>showToast('Período, está vazia!');</script>";
                ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                
                return;
            }
            
            if (string.IsNullOrWhiteSpace(ddlAno.SelectedValue))
            {
                // Acione o toast quando a página for carregada
                string script = "<script>showToast('Data final do período, está vazia!');</script>";
                ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                
                return;
            }


            // ddlStatus é HTML, então o valor vem assim:
            string valores = Request.Form["ddlStatus"];

            if (valores == "")
            {
                string script = "<script>showToast('Selecione pelo menos uma filial para gerar o arquivo!');</script>";
                ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                return;
            }
            else
            {
               
                if (!string.IsNullOrEmpty(valores))
                {
                    
                    txtSelecionados.Text = nomeMes + "_" + "_" + valores;

                }
                string[] selecionados = ddlStatus.Items.Cast<System.Web.UI.WebControls.ListItem>()
                    .Where(x => x.Selected)
                    .Select(x => x.Value)
                    .ToArray();
                CarregarMotoristas(selecionados);

            }
        }

        public void Botoes()
        {
            bool flag = false;
            string cmdText = " SELECT CASE WHEN EXISTS ( SELECT 1 FROM tbavaliacaomotorista  WHERE mes = @mes AND vl_total IS NULL ) THEN 0 ELSE 1 END AS TodosAvaliados";
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(cmdText, connection))
                {
                    sqlCommand.Parameters.AddWithValue("@mes", (object)"12/2025");
                    connection.Open();
                    flag = Convert.ToInt32(sqlCommand.ExecuteScalar()) == 1;
                }
            }
            if (!flag)
                return;
            this.btnGerarPlanilha.Visible = true;
            this.btnImpmrimeMotoristas.Visible = true;
        }

        private object SafeDateValue(string input)
        {
            DateTime dt;
            if (DateTime.TryParse(input, out dt))
                return dt.ToString("yyyy-MM-dd");
            else
                return DBNull.Value;
        }

        protected void Editar(object sender, EventArgs e)
        {
            using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
            {
                string id = gvMotoristas.DataKeys[row.RowIndex].Value.ToString();

                string url = "Frm_AvaliaDesempenho.aspx?id=" + id;
                string script = $"window.open('{url}', '_blank', 'width=900,height=780,scrollbars=yes,resizable=yes');";
                ClientScript.RegisterStartupScript(this.GetType(), "openWindow", script, true);

               
            }
        }

        protected void btnGerarPlanilha_Click(object sender, EventArgs e)
        {
            string selectCommandText = $"SELECT  cracha AS MATRICULA,  nome AS MOTORISTA,  funcao AS 'FUNCAO',  nucleo AS 'NUCLEO',  CASE  WHEN admissao LIKE '[1-2][0-9][0-9][0-9]-%'  THEN CONVERT(VARCHAR(10), CONVERT(DATE, admissao, 120), 103)   WHEN admissao LIKE '%/%' AND ISDATE(admissao) = 1 THEN CONVERT(VARCHAR(10), CONVERT(DATE, admissao, 103), 103) ELSE admissao END AS 'ADMISSAO', CAST(vl_total AS VARCHAR(10)) + '%' AS 'BONUS' FROM tbavaliacaomotorista  where mes='{this.ddlMes.SelectedValue.PadLeft(2, '0')}/{this.ddlAno.SelectedValue}' ";
            DataTable dataTable = new DataTable();
            using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommandText, this.conn))
                sqlDataAdapter.Fill(dataTable);
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<table border='1'>");
            stringBuilder.Append("<tr>");
            foreach (DataColumn column in (InternalDataCollectionBase)dataTable.Columns)
                stringBuilder.Append("<th>").Append(column.ColumnName).Append("</th>");
            stringBuilder.Append("</tr>");
            foreach (DataRow row in (InternalDataCollectionBase)dataTable.Rows)
            {
                stringBuilder.Append("<tr>");
                foreach (object obj in row.ItemArray)
                    stringBuilder.Append("<td>").Append(obj.ToString()).Append("</td>");
                stringBuilder.Append("</tr>");
            }
            stringBuilder.Append("</table>");
            this.Response.Clear();
            this.Response.ContentType = "application/vnd.ms-excel";
            this.Response.AddHeader("Content-Disposition", $"attachment; filename=AvaliacaoMotoristas_{this.ddlMes.SelectedItem.Text}_{this.ddlAno.SelectedItem.Text}.xls");
            this.Response.Write(stringBuilder.ToString());
            this.Response.End();
        }

        protected void btnImpmrimeMotoristas_Click(object sender, EventArgs e)
        {
            string script = $"window.open('{$"Frm_Impressao_Motorista.aspx?mes={this.ddlMes.SelectedValue.ToString()}&ano={this.ddlAno.SelectedItem.Text}"}', '_blank', 'width=900,height=780,scrollbars=yes,resizable=yes');";
            this.ClientScript.RegisterStartupScript(this.GetType(), "openWindow", script, true);
        }
    }
   
  


}




