using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Web.Script.Serialization;
using System.Configuration;

namespace NewCapit.dist.pages
{
    public partial class ConsultaCargas : System.Web.UI.Page
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["conexao"].ToString());
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DateTime dataHoraAtual = DateTime.Now;
                PreencherComboStatus();
                PreencherTabela();
            }
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
        }
        private void PreencherTabela()
        {

            var dataTable = DAL.ConCargas.FetchDataTable();            
            if (dataTable.Rows.Count <= 0)
            {
                return;
            }
            gvListCargas.DataSource = dataTable;
            gvListCargas.DataBind();

            //gvListCargas.UseAccessibleHeader = true;
            //gvListCargas.HeaderRow.TableSection = TableRowSection.TableHeader;
            //gvListCargas.FooterRow.TableSection = TableRowSection.TableFooter;
        }
        private void PreencherComboStatus()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT cod_status, ds_status FROM tb_status";

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
                    ddlStatus.DataSource = reader;
                    ddlStatus.DataTextField = "ds_status";  // Campo que será mostrado no ComboBox
                    ddlStatus.DataValueField = "cod_status";  // Campo que será o valor de cada item                    
                    ddlStatus.DataBind();  // Realiza o binding dos dados                   
                    ddlStatus.Items.Insert(0, new ListItem("", "0"));
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

        //Método que faz a "exclusão" do dado deixando ele com o status de invisivel
        protected void Excluir(object sender, EventArgs e)
        {
            if (txtconformmessageValue.Value == "Yes")
            {
                using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
                {
                    string id = gvListCargas.DataKeys[row.RowIndex].Value.ToString();

                    string sql = "update tbcargas set fl_exclusao='S' where id=@id";
                    SqlCommand comando = new SqlCommand(sql, conn);
                    comando.Parameters.AddWithValue("@id", id);
                    try
                    {
                        conn.Open();
                        comando.ExecuteNonQuery();
                        conn.Close();
                        PreencherTabela();
                        string retorno = "Registro excluído com sucesso!";
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append("<script type = 'text/javascript'>");
                        sb.Append("window.onload=function(){");
                        sb.Append("alert('");
                        sb.Append(retorno);
                        sb.Append("')};");
                        sb.Append("</script>");
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());

                    }
                    catch (Exception ex)
                    {
                        var message = new JavaScriptSerializer().Serialize(ex.Message.ToString());
                        string retorno = "Erro! Contate o administrador. Detalhes do erro: " + message;
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append("<script type = 'text/javascript'>");
                        sb.Append("window.onload=function(){");
                        sb.Append("alert('");
                        sb.Append(retorno);
                        sb.Append("')};");
                        sb.Append("</script>");
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
                        //Chama a página de consulta clientes
                        Response.Redirect("ConsultaClientes.aspx");
                    }

                    finally
                    {
                        conn.Close();
                    }
                }
            }

            //Método que faz a "exclusão" do dado deixando ele com o status de invisivel


        }

        protected void lnkPesquisar_Click(object sender, EventArgs e)
        {
            string sql = @"SELECT id, carga, peso, status, cliorigem, clidestino, 
                   CONVERT(varchar, previsao, 103) AS previsao, situacao 
                   FROM tbcargas 
                   WHERE fl_exclusao IS NULL";

            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                try
                {
                    conn.Open();

                    using (var cmd = conn.CreateCommand())
                    {
                        DateTime? dtInicial = null;
                        DateTime? dtFinal = null;

                        string dataInicial = txtInicioData.Text.Trim();
                        string dataFinal = txtFimData.Text.Trim();
                        string status = ddlStatus.SelectedItem.Text;

                        if (!string.IsNullOrWhiteSpace(dataInicial) && DateTime.TryParse(dataInicial, out DateTime parsedDataInicial))
                        {
                            dtInicial = parsedDataInicial;
                        }

                        if (!string.IsNullOrWhiteSpace(dataFinal) && DateTime.TryParse(dataFinal, out DateTime parsedDataFinal))
                        {
                            dtFinal = parsedDataFinal;
                        }

                        if (dtInicial.HasValue && dtFinal.HasValue)
                        {
                            sql += " AND previsao BETWEEN @dataInicial AND @dataFinal";
                            cmd.Parameters.Add("@dataInicial", SqlDbType.DateTime).Value = dtInicial.Value;
                            cmd.Parameters.Add("@dataFinal", SqlDbType.DateTime).Value = dtFinal.Value;
                        }
                        else if (dtInicial.HasValue)
                        {
                            sql += " AND previsao >= @dataInicial";
                            cmd.Parameters.Add("@dataInicial", SqlDbType.DateTime).Value = dtInicial.Value;
                        }
                        else if (dtFinal.HasValue)
                        {
                            sql += " AND previsao <= @dataFinal";
                            cmd.Parameters.Add("@dataFinal", SqlDbType.DateTime).Value = dtFinal.Value;
                        }

                        if (!string.IsNullOrEmpty(status) && status != "0") // Supondo que "0" seja a opção "Todos"
                        {
                            sql += " AND status = @status";
                            cmd.Parameters.Add("@status", SqlDbType.VarChar, 50).Value = status;
                        }

                        cmd.CommandText = sql;

                        using (var reader = cmd.ExecuteReader())
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Load(reader);
                            gvListCargas.DataSource = dataTable;
                            gvListCargas.DataBind();
                        }
                    }
                }
                catch (Exception ex)
                {
                    lblMensagem.Text = "Erro ao buscar cargas: " + ex.Message;
                    lblMensagem.ForeColor = System.Drawing.Color.Red;
                }
            }
        }

        protected void gvListCargas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvListCargas.PageIndex = e.NewPageIndex;
            PreencherTabela();  // Método para recarregar os dados no GridView
        }

    }
}
