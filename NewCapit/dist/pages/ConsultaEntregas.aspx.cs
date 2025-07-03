using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;


namespace NewCapit.dist.pages
{
    public partial class ConsultaEntregas : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
            if(!IsPostBack)
            {
                PreencherComboStatus();
                //PreencherVeiculosCNT();
                CarregarColetas();
            }
            
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
                    ddlStatus.Items.Insert(0, new ListItem("Selecione...", "0"));
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
        //private void PreencherVeiculosCNT()
        //{
        //    // Consulta SQL que retorna os dados desejados
        //    string query = "SELECT id, descricao FROM tbtiposveiculoscnt order by descricao";

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
        //            ddlVeiculosCNT.DataSource = reader;
        //            ddlVeiculosCNT.DataTextField = "descricao";  // Campo que será mostrado no ComboBox
        //            ddlVeiculosCNT.DataValueField = "id";  // Campo que será o valor de cada item                    
        //            ddlVeiculosCNT.DataBind();  // Realiza o binding dos dados                   
        //            ddlVeiculosCNT.Items.Insert(0, new ListItem("Selecione...", "0"));
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

        private void CarregarColetas()
        {
            var novosDados = DAL.ConEntrega.FetchDataTable();

            rptCarregamento.DataSource = novosDados;
            rptCarregamento.DataBind();

            // Armazena no ViewState, se necessário para outras operações
            ViewState["rptCarregamento"] = novosDados;

            lblMensagem.Text = string.Empty;
        }

        private void CarregarColetas2()
        {
            DateTime? dtInicial = null;
            DateTime? dtFinal = null;

            string dataInicial = txtInicioData.Text.Trim();
            string dataFinal = txtFimData.Text.Trim();
            string status = (ddlStatus.SelectedValue != "0") ? ddlStatus.SelectedValue : null;
            //string veiculo = (ddlVeiculosCNT.SelectedValue != "0") ? ddlVeiculosCNT.SelectedValue : null;

            if (!string.IsNullOrWhiteSpace(dataInicial) && DateTime.TryParse(dataInicial, out DateTime parsedDataInicial))
            {
                dtInicial = parsedDataInicial;
            }

            if (!string.IsNullOrWhiteSpace(dataFinal) && DateTime.TryParse(dataFinal, out DateTime parsedDataFinal))
            {
                dtFinal = parsedDataFinal;
            }

            var novosDados = DAL.ConEntrega.FetchDataTable2(dtInicial, dtFinal, status);

            rptCarregamento.DataSource = novosDados;
            rptCarregamento.DataBind();

            ViewState["rptCarregamento"] = novosDados;
            lblMensagem.Text = string.Empty;
        }

        protected void rptCarregamento_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                // Pega o valor da carga ou do idviagem do item atual
                string idviagem = DataBinder.Eval(e.Item.DataItem, "num_carregamento").ToString();

                // Pega o repeater interno
                Repeater rptColeta = (Repeater)e.Item.FindControl("rptColeta");

                if (rptColeta != null && !string.IsNullOrEmpty(idviagem))
                {
                    // Busca os dados de coletas relacionadas àquele CVA
                    DataTable dtColetas = DAL.ConCargas.FetchDataTableColetas3(idviagem);

                    // Bind dos dados ao repeater interno
                    rptColeta.DataSource = dtColetas;
                    rptColeta.DataBind();
                }
            }
        }

        protected void rptColeta_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            string previsaoStr = DataBinder.Eval(e.Item.DataItem, "previsao")?.ToString();
            string dataHoraStr = DataBinder.Eval(e.Item.DataItem, "data_hora")?.ToString();
            string status = DataBinder.Eval(e.Item.DataItem, "status")?.ToString();

            Label lblAtendimento = (Label)e.Item.FindControl("lblAtendimento");
            HtmlTableCell tdAtendimento = (HtmlTableCell)e.Item.FindControl("tdAtendimento");

            if (lblAtendimento != null && tdAtendimento != null)
            {
                DateTime previsao, dataHora;
                DateTime agora = DateTime.Now;

                if (DateTime.TryParse(previsaoStr, out previsao) && DateTime.TryParse(dataHoraStr, out dataHora))
                {
                    DateTime dataPrevisao = previsao.Date;
                    DateTime dataHoraComparacao = new DateTime(
                        dataPrevisao.Year, dataPrevisao.Month, dataPrevisao.Day,
                        dataHora.Hour, dataHora.Minute, dataHora.Second
                    );

                    if (dataHoraComparacao < agora && (status == "Concluído" || status == "Pendente"))
                    {
                        lblAtendimento.Text = "Atrasado";
                        tdAtendimento.BgColor = "Red";
                        tdAtendimento.Attributes["style"] = "color: white; font-weight: bold;";
                    }
                    else if (dataHoraComparacao.Date == agora.Date && dataHoraComparacao.TimeOfDay <= agora.TimeOfDay
                             && (status == "Concluído" || status == "Pendente"))
                    {
                        lblAtendimento.Text = "No Prazo";
                        tdAtendimento.BgColor = "Green";
                        tdAtendimento.Attributes["style"] = "color: white; font-weight: bold;";
                    }
                    else if (dataHoraComparacao > agora && status == "Concluído")
                    {
                        lblAtendimento.Text = "Antecipado";
                        tdAtendimento.BgColor = "Orange";
                        tdAtendimento.Attributes["style"] = "color: white; font-weight: bold;";
                    }
                    else
                    {
                        lblAtendimento.Text = status;

                    }
                }
            }
        }

        protected void lnkPesquisar_Click(object sender, EventArgs e)
        {
            CarregarColetas2();
        }
        protected void lnkEditar_Command(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "Editar")
            {
                string numCarregamento = e.CommandArgument.ToString();
                string url = $"Frm_AtualizaOrdemColeta.aspx?carregamento={numCarregamento}";
                Response.Redirect(url);
            }
        }

    }
}