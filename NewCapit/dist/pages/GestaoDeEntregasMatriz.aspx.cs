using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using System.IO;
using System.Collections;
 
using ClosedXML.Excel;
using System.Web.UI.HtmlControls;
using System.Web.Configuration;

namespace NewCapit.dist.pages
{
    public partial class GestaoDeEntregasMatriz : System.Web.UI.Page
    {
        string conn = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
        string idViagem;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UsuarioLogado"] != null)
                {
                    string nomeUsuario = Session["UsuarioLogado"].ToString();
                }
                else
                {
                    Response.Redirect("Login.aspx");
                    return;
                }

                // 🔥 DEFINE estado inicial
                bool somenteConcluidas = false;

                // 🔥 PRIMEIRA CARGA CORRETA (SEM DUPLICAÇÃO)
                CarregarColetas(somenteConcluidas, 0);

                CarregarGridBarraPesquisa();
            }
        }
        protected void btnPostbackOcultar_Click(object sender, EventArgs e)
        {
            bool ocultar = bool.Parse(hfOcultarViagens.Value);
            FiltrarViagens(ocultar);
        }
        protected void CarregarGrid()
        {

            //string conn = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                string query = "SELECT Id, carga, emissao, peso, status, CONVERT(varchar, previsao, 103) AS previsao, cliorigem, cidorigem, clidestino, ciddestino FROM tbcargas where empresa = '1111' ";

                if (!string.IsNullOrEmpty(DataInicio.Text))
                    query += " AND previsao >= @DataInicio";

                if (!string.IsNullOrEmpty(DataFim.Text))
                    query += " AND previsao <= @DataFim";

                //if (!string.IsNullOrEmpty(ddlStatus.SelectedValue))
                //    query += " AND status = @Status";

                SqlCommand cmd = new SqlCommand(query, conn);

                if (!string.IsNullOrEmpty(DataInicio.Text))
                    cmd.Parameters.AddWithValue("@DataInicio", DateTime.Parse(DataInicio.Text));

                if (!string.IsNullOrEmpty(DataFim.Text))
                    cmd.Parameters.AddWithValue("@DataFim", DateTime.Parse(DataFim.Text));

                //if (!string.IsNullOrEmpty(ddlStatus.SelectedValue))
                //    cmd.Parameters.AddWithValue("@Status", ddlStatus.SelectedValue);
                query += @"
            GROUP BY CONVERT(VARCHAR(10), previsao, 103)
            ORDER BY CONVERT(DATE, previsao) ";


                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                //gvCargas.DataSource = dt;
                //gvCargas.DataBind();

                // Armazena os dados no ViewState para usar na exportação
                Session["Cargas"] = dt;
            }
        }
        //protected void btnFiltrar_Click(object sender, EventArgs e)
        //{
        //    bool ocultar = false;

        //    if (bool.TryParse(hfOcultarViagens.Value, out ocultar))
        //    {
        //        FiltrarViagens(ocultar);
        //    }
        //    else
        //    {
        //        // fallback de segurança
        //        CarregarColetas();
        //    }
        //}
        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            bool somenteConcluidas = false;
            bool.TryParse(hfOcultarViagens.Value, out somenteConcluidas);

            CarregarColetas(somenteConcluidas, 0);
        }


        protected void btnExportarExcel_Click(object sender, EventArgs e)
        {
            // Corrigido para a chave que você realmente usa no ViewState
            DataTable dt = Session["rptCarregamento"] as DataTable;

            if (dt == null || dt.Rows.Count == 0)
            {
                // Opcional: Avisar o usuário que não há dados
                return;
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                // Adiciona a planilha
                var ws = wb.Worksheets.Add("Relatório de Cargas");

                // Insere os dados do DataTable
                // O método InsertTable cria uma tabela formatada do Excel automaticamente
                var table = ws.Cell(1, 1).InsertTable(dt);

                // Ajusta a largura das colunas
                ws.Columns().AdjustToContents();

                // Configuração do Response para Download
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=Relatorio_Cargas.xlsx");

                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();

                    // É importante usar o HttpContext para evitar o erro de 'Thread Aborted'
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
            }
        }

        protected void Editar(object sender, EventArgs e)
        {
            //using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
            //{
            //    string id = gvCargas.DataKeys[row.RowIndex].Value.ToString();

            //    Response.Redirect("/dist/pages/Frm_AltCarga.aspx?id=" + id);
            //}
        }

        protected void gvCargas_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            //gvCargas.PageIndex = e.NewPageIndex;
            //CarregarGrid();
        }

        protected void gvCargas_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            //gvCargas.EditIndex = e.NewEditIndex;
            //CarregarGrid();
        }

        protected void gvCargas_RowCancelingEdit(object sender, System.Web.UI.WebControls.GridViewCancelEditEventArgs e)
        {
            //gvCargas.EditIndex = -1;
            //CarregarGrid();
        }

        protected void gvCargas_RowUpdating(object sender, System.Web.UI.WebControls.GridViewUpdateEventArgs e)
        {
            //GridViewRow row = gvPedidos.Rows[e.RowIndex];
            //int id = Convert.ToInt32(gvPedidos.DataKeys[e.RowIndex].Value);
            //string cliente = ((TextBox)row.Cells[1].Controls[0]).Text;
            //string data = ((TextBox)row.Cells[2].Controls[0]).Text;
            //string valor = ((TextBox)row.Cells[3].Controls[0]).Text;

            //using (SqlConnection conn = new SqlConnection(connStr))
            //{
            //    string sql = "UPDATE Pedidos SET Cliente=@Cliente, DataPedido=@DataPedido, Valor=@Valor WHERE Id=@Id";
            //    SqlCommand cmd = new SqlCommand(sql, conn);
            //    cmd.Parameters.AddWithValue("@Cliente", cliente);
            //    cmd.Parameters.AddWithValue("@DataPedido", DateTime.Parse(data));
            //    cmd.Parameters.AddWithValue("@Valor", decimal.Parse(valor));
            //    cmd.Parameters.AddWithValue("@Id", id);

            //    conn.Open();
            //    cmd.ExecuteNonQuery();
            //}

            //gvPedidos.EditIndex = -1;
            //CarregarPedidos();
        }

        //public void FiltrarViagens(bool ocultar)
        //{
        //    if (ocultar == false)
        //    {

        //        CarregarColetas();
        //    }
        //    else
        //    {
        //        CarregarColetasConcluidas();


        //    }
        //}
        public void FiltrarViagens(bool somenteConcluidas)
        {
            CarregarColetas(somenteConcluidas, 0);
        }
        //private void CarregarColetas()
        //{
        //    string busca = txtPesquisar.Text;
        //    var dados = DAL.ConEntrega.FetchDataTableEntregasMatriz(GetDataInicio(), GetDataFim(), busca);
        //    Session["rptCarregamento"] = dados;

        //    // AJUSTE: Passa os dados, página 0 e o total de linhas (3 argumentos)
        //    BindRepeaterComPaginacao(dados, 0, dados.Rows.Count);
        //}
        private void CarregarColetas(bool somenteConcluidas, int pagina = 0)
        {
            int tamanhoPagina = 75;
            string busca = txtPesquisar.Text;

            DataTable dados = DAL.ConEntrega.FetchDataTableEntregasMatrizUnificado(
                GetDataInicio(),
                GetDataFim(),
                pagina,
                tamanhoPagina,
                busca,
                somenteConcluidas
            );

            int totalRegistros = DAL.ConEntrega.GetTotalRegistrosUnificado(
                GetDataInicio(),
                GetDataFim(),
                busca,
                somenteConcluidas
            );

            Session["PaginaAtual"] = pagina;
            ViewState["PaginaAtual"] = pagina; // 👈 ADICIONE ISSO

            BindRepeaterComPaginacao(dados, pagina, totalRegistros);
        }
        private void CarregarGridBarraPesquisa()
        {
            DataTable dados = DAL.ConEntrega.FetchDataTableEntregasMatriz(GetDataInicio(), GetDataFim());
            Session["rptCarregamento"] = dados;

            // AJUSTE: Passa os dados, página 0 e o total de linhas (3 argumentos)
            BindRepeaterComPaginacao(dados, 0, dados.Rows.Count);
        }
        private void BindRepeaterComPaginacao(DataTable dados, int paginaAtual, int totalRegistros)
        {
            int tamanhoPagina = 75;
            // Calcula o total de páginas baseado no COUNT que veio do banco
            int totalPaginas = (int)Math.Ceiling((double)totalRegistros / tamanhoPagina);

            Session["PaginaAtual"] = paginaAtual;

            // Vincula as 75 linhas que vieram do SQL ao Repeater principal
            rptCarregamento.DataSource = dados;
            rptCarregamento.DataBind();

            // Monta a lista de números para o rptPaginacao
            var listaPaginas = new List<object>();
            for (int i = 0; i < totalPaginas; i++)
            {
                listaPaginas.Add(new { PageText = (i + 1), PageIndex = i });
            }

            rptPaginacao.DataSource = listaPaginas;
            rptPaginacao.DataBind();

            // Ativa/Desativa botões de navegação
            btnAnterior.Enabled = (paginaAtual > 0);
            btnProximo.Enabled = (paginaAtual < totalPaginas - 1);
        }
        //protected void Pagina_Click(object sender, EventArgs e)
        //{
        //    LinkButton btn = (LinkButton)sender;
        //    string arg = btn.CommandArgument;
        //    int paginaAtual = (int)(ViewState["PaginaAtual"] ?? 0);

        //    // Lógica para Anterior/Próximo
        //    if (arg == "Prev") paginaAtual--;
        //    else if (arg == "Next") paginaAtual++;
        //    else paginaAtual = int.Parse(arg);

        //    // Chama o carregamento que por sua vez chama o Bind
        //    CarregarColetasConcluidas(paginaAtual);
        //}

        protected void Pagina_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            string comando = btn.CommandArgument;

            int paginaAtual = Convert.ToInt32(Session["PaginaAtual"] ?? 0);

            switch (comando)
            {
                case "Prev":
                    paginaAtual--;
                    break;

                case "Next":
                    paginaAtual++;
                    break;

                default:
                    paginaAtual = Convert.ToInt32(comando);
                    break;
            }

            if (paginaAtual < 0)
                paginaAtual = 0;

            // 🔥 Recupera estado atual corretamente
            bool somenteConcluidas = false;
            bool.TryParse(hfOcultarViagens.Value, out somenteConcluidas);

            // 🔥 ESSA LINHA É A CHAVE
            CarregarColetas(somenteConcluidas, paginaAtual);
        }

        private DateTime? GetDataInicio()
        {
            return DateTime.TryParse(DataInicio.Text, out var d) ? d : (DateTime?)null;
        }
        private DateTime? GetDataFim()
        {
            return DateTime.TryParse(DataFim.Text, out var d) ? d : (DateTime?)null;
        }
        private void CarregarColetasConcluidas(int pagina = 0, string busca = "")
        {
            int tamanhoPagina = 75;

            // 1. Chama a DAL passando os 5 parâmetros agora necessários
            DataTable dados = DAL.ConEntrega.FetchDataTableEntregasMatrizConcluida(
                GetDataInicio(),
                GetDataFim(),
                pagina,
                tamanhoPagina,
                busca
            );

            // 2. Importante: O seu método de contagem também precisa da busca para a paginação bater
            // Dentro do CarregarColetasConcluidas:
            int totalRegistros = DAL.ConEntrega.GetTotalRegistrosConcluidos(GetDataInicio(), GetDataFim(), busca);

            // 3. Salva na Session para que outros métodos (como exportar ou filtros extras) tenham o dado
            Session["rptCarregamento"] = dados;

            // 4. Vincula ao Repeater e desenha a paginação
            BindRepeaterComPaginacao(dados, pagina, totalRegistros);

            lblMensagem.Text = string.IsNullOrEmpty(busca) ? "" : $"Busca por '{busca}' retornou {totalRegistros} registro(s).";
        }
        private void MontarPaginacaoNumerica(int total, int atual, int tamanho)
        {
            int totalPaginas = (int)Math.Ceiling((double)total / tamanho);
            List<dynamic> paginas = new List<dynamic>();

            for (int i = 0; i < totalPaginas; i++)
            {
                paginas.Add(new { PageText = (i + 1), PageIndex = i });
            }

            rptPaginacao.DataSource = paginas;
            rptPaginacao.DataBind();

            btnAnterior.Enabled = (atual > 0);
            btnProximo.Enabled = (atual < totalPaginas - 1);
        }


        protected void lnkEditar_Command(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "Editar")
            {
                string numCarregamento = e.CommandArgument.ToString();
                string url = $"Frm_AtualizaColetaMatriz.aspx?carregamento={numCarregamento}";
                Response.Redirect(url);
            }
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


                    Label lblStatus = e.Item.FindControl("lblStatus") as Label;
                    if (lblStatus == null) return;

                    string status = lblStatus.Text.Trim();
                    switch (status)
                    {
                        case "Pronto":
                            lblStatus.CssClass += " bg-warning text-white";
                            break;

                        case "Em Transito":
                            lblStatus.CssClass += " bg-success text-white";
                            break;

                        case "Ag. Descarga":
                            lblStatus.CssClass += " bg-danger text-white";
                            break;

                        case "Ag. Carregamento":
                            lblStatus.CssClass += " bg-red text-warning";
                            break;

                        case "Ag. Documentos":
                            lblStatus.CssClass += " bg-yellow text-dark";
                            break;

                        case "Carregando":
                            lblStatus.CssClass += " bg-purple text-white";
                            break;
                        case "Pendente":
                            lblStatus.CssClass += " bg-black text-white";
                            break;
                        case "Pernoite":
                            lblStatus.CssClass += " bg-purple text-white";
                            break;
                        case "Concluido":
                            lblStatus.CssClass += " bg-info text-white";
                            break;
                        case "Liberado Vazio":
                            lblStatus.CssClass += " bg-info text-pink";
                            break;
                        case "Veic. Quebrado":
                            lblStatus.CssClass += " bg-success text-purple";
                            break;
                        case "Cancelada":
                            lblStatus.CssClass += " bg-warning text-purple";
                            break;
                    }
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

        [System.Web.Services.WebMethod]
        //public static object BuscarDocumento(string numeroDocumento)
        //{      
        //    using (SqlConnection conn = new SqlConnection(
        //        ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
        //    {
        //        conn.Open();

        //        string sqlCte = @"
        //    SELECT chave_de_acesso, emissao_documento, empresa_emissora, idviagem
        //    FROM tbcte
        //    WHERE num_documento = @numero";

        //        SqlCommand cmd = new SqlCommand(sqlCte, conn);
        //        cmd.Parameters.AddWithValue("@numero", numeroDocumento);

        //        SqlDataReader dr = cmd.ExecuteReader();

        //        if (!dr.Read())
        //            return new { encontrado = false };

        //        string idViagem = dr["idviagem"].ToString();

        //        var resultado = new
        //        {
        //            encontrado = true,
        //            chave = dr["chave_de_acesso"].ToString(),
        //            emissao = Convert.ToDateTime(dr["emissao_documento"]).ToString("dd/MM/yyyy"),
        //            empresa = dr["empresa_emissora"].ToString(),
        //            motorista = "",
        //            destino = "",
        //            cidade = "",
        //            uf = "",
        //            dataSaida = ""
        //        };

        //        dr.Close();

        //        string sqlCar = @"
        //    SELECT nomemotorista, recebedpr. cod_recebedor, uf_recebedor, dtchegada
        //    FROM tbcarregamentos
        //    WHERE num_carregamento = @num";

        //        SqlCommand cmdCar = new SqlCommand(sqlCar, conn);
        //        cmdCar.Parameters.AddWithValue("@num", idViagem);

        //        SqlDataReader dr2 = cmdCar.ExecuteReader();

        //        if (dr2.Read())
        //        {
        //            resultado = new
        //            {
        //                encontrado = true,
        //                chave = resultado.chave,
        //                emissao = resultado.emissao,
        //                empresa = resultado.empresa,
        //                motorista = dr2["nomemotorista"].ToString(),
        //                destino = dr2["recebedor"].ToString(),
        //                cidade = dr2["cid_recebedor"].ToString(),
        //                uf = dr2["uf_recebedor"].ToString(),
        //                dataSaida = Convert.ToDateTime(dr2["dtchegada"])
        //                                .ToString("dd/MM/yyyy HH:mm")
        //            };
        //        }

        //        return resultado;
        //    }
        //}

        protected void btnSalvarBaixa_Click(object sender, EventArgs e)
        {
            if (gridRadiosCTe.Checked == true)
            {
                try
                {
                    // 1. Verificação de segurança para a Session
                    if (Session["UsuarioLogado"] == null)
                    {
                        Response.Redirect("Login.aspx");
                        return;
                    }

                    string usuario = Session["UsuarioLogado"].ToString();
                    string numeroDoc = txtNumeroDocumento.Text.Trim(); // .Trim() evita espaços vazios

                    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
                    {
                        string sql = @"
                UPDATE tbcte 
                SET baixado_por = @usuario, 
                    status_documento = @status_documento, 
                    data_baixa = GETDATE() 
                WHERE num_documento = @numero";

                        SqlCommand cmd = new SqlCommand(sql, conn);
                        cmd.Parameters.AddWithValue("@usuario", usuario);
                        cmd.Parameters.AddWithValue("@numero", numeroDoc);
                        cmd.Parameters.AddWithValue("@status_documento", "Baixado");

                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {


                            ScriptManager.RegisterStartupScript(this, GetType(), "ok", "alert('CTe " + numeroDoc + " baixado com sucesso!');", true);
                            Limpar();
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "erro", "alert('Erro: Documento não encontrado no banco.');", true);
                            Limpar();
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Remove quebras de linha e aspas simples/duplas para não quebrar o alert do JS
                    string mensagemErro = ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", " ");

                    string script = $"alert('Erro técnico: {mensagemErro}');";

                    ScriptManager.RegisterStartupScript(this, GetType(), "erro_catch", script, true);
                }
            }
            else if (gridRadiosNFSe.Checked == true)
            {
                try
                {
                    // 1. Verificação de segurança para a Session
                    if (Session["UsuarioLogado"] == null)
                    {
                        Response.Redirect("Login.aspx");
                        return;
                    }

                    string usuario = Session["UsuarioLogado"].ToString();
                    string numeroDoc = txtNumeroDocumento.Text.Trim(); // .Trim() evita espaços vazios

                    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
                    {
                        string sql = @"
                UPDATE tbnfse 
                SET baixado_por = @usuario, 
                    status_documento = @status_documento, 
                    data_baixa = GETDATE() 
                WHERE num_documento = @numero";

                        SqlCommand cmd = new SqlCommand(sql, conn);
                        cmd.Parameters.AddWithValue("@usuario", usuario);
                        cmd.Parameters.AddWithValue("@numero", numeroDoc);
                        cmd.Parameters.AddWithValue("@status_documento", "Baixado");

                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {

                            ScriptManager.RegisterStartupScript(this, GetType(), "ok", "alert('NFS-e " + numeroDoc + " baixado com sucesso!');", true);
                            Limpar();
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "erro", "alert('Erro: Documento não encontrado no banco.');", true);
                            Limpar();
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Remove quebras de linha e aspas simples/duplas para não quebrar o alert do JS
                    string mensagemErro = ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", " ");

                    string script = $"alert('Erro técnico: {mensagemErro}');";

                    ScriptManager.RegisterStartupScript(this, GetType(), "erro_catch", script, true);
                }
            }


            
        }

        public void Limpar()
        {
            txtNumeroDocumento.Text = string.Empty;
            lblChave.Text = string.Empty;
            lblCidade.Text = string.Empty;
            lblDataSaida.Text = string.Empty;
            lblDestino.Text = string.Empty;
            lblEmissao.Text = string.Empty;
            lblEmpresa.Text = string.Empty;
            lblMotorista.Text = string.Empty;

        }
        protected void btnBaixar_Click(object sender, EventArgs e)
        {



            ScriptManager.RegisterStartupScript(
                this,
                GetType(),
                "abrirModal",
                "var m = new bootstrap.Modal(document.getElementById('modalCTE')); m.show();",
                true
            );
        }

        private object SafeDateTimeValue(string input)
        {
            DateTime dt;
            if (DateTime.TryParse(input, out dt))
                return dt.ToString("yyyy-MM-dd HH:mm:ss");
            else
                return DBNull.Value;
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {

            if (gridRadiosCTe.Checked == true)
            {
                using (SqlConnection conn = new SqlConnection(
                   ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
                {
                    conn.Open();

                    string sqlCte = @"
            SELECT chave_de_acesso, emissao_documento, empresa_emissora, 
                   id_viagem, status_documento
            FROM tbcte
            WHERE num_documento = @numero";

                    SqlCommand cmd = new SqlCommand(sqlCte, conn);
                    cmd.Parameters.AddWithValue("@numero", txtNumeroDocumento.Text);

                    SqlDataReader dr = cmd.ExecuteReader();

                    // 🔍 NÃO encontrou registro
                    if (!dr.Read())
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(),
                            "ok", "alert('Não há documento cadastrado!');", true);
                        Limpar();
                        return;
                    }

                    // 📦 Encontrou, mas já está baixado
                    if (dr["status_documento"].ToString() == "Baixado")
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(),
                            "ok", "alert('Documento já foi baixado!');", true);
                        Limpar();
                        return;
                    }

                    // 📄 Encontrou e está Pendente → segue fluxo
                    idViagem = dr["id_viagem"].ToString();
                    lblChave.Text = dr["chave_de_acesso"].ToString();
                    SafeDateTimeValue(lblEmissao.Text = dr["emissao_documento"].ToString());
                    lblEmpresa.Text = dr["empresa_emissora"].ToString();

                    dr.Close();

                    string sqlCar = @"
            SELECT m.nommot, c.cid_recebedor, c.uf_recebedor, 
                   c.cheg_cliente, c.recebedor  
            FROM tbcargas AS c  
            INNER JOIN tbmotoristas AS m ON c.codmot = m.codmot 
            WHERE carga = @idviagem";

                    SqlCommand cmdCar = new SqlCommand(sqlCar, conn);
                    cmdCar.Parameters.AddWithValue("@idviagem", idViagem);

                    SqlDataReader dr2 = cmdCar.ExecuteReader();

                    if (dr2.Read())
                    {
                        lblMotorista.Text = dr2["nommot"].ToString();
                        lblDestino.Text = dr2["recebedor"].ToString();
                        lblCidade.Text = dr2["cid_recebedor"].ToString() + "/" + dr2["uf_recebedor"].ToString();
                        SafeDateTimeValue(lblDataSaida.Text = dr2["cheg_cliente"].ToString());
                    }

                    dr2.Close();
                }
            }
            else if (gridRadiosNFSe.Checked == true)
            {
                using (SqlConnection conn = new SqlConnection(
                   ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
                {
                    conn.Open();

                    string sqlCte = @"
                                SELECT num_documento, emissao_documento, 
                                       idviagem, status_documento
                                FROM tbnfse
                                WHERE num_documento =@numero";

                    SqlCommand cmd = new SqlCommand(sqlCte, conn);
                    cmd.Parameters.AddWithValue("@numero", txtNumeroDocumento.Text);

                    SqlDataReader dr = cmd.ExecuteReader();

                    // 🔍 NÃO encontrou registro
                    if (!dr.Read())
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(),
                            "ok", "alert('Não há documento cadastrado!');", true);
                        Limpar();
                        return;
                    }

                    // 📦 Encontrou, mas já está baixado
                    if (dr["status_documento"].ToString() == "Baixado")
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(),
                            "ok", "alert('Documento já foi baixado!');", true);
                        Limpar();
                        return;
                    }

                    // 📄 Encontrou e está Pendente → segue fluxo
                    idViagem = dr["idviagem"].ToString();
                    lblChave.Text = "N/A";
                    SafeDateTimeValue(lblEmissao.Text = dr["emissao_documento"].ToString());
                    lblEmpresa.Text = "N/A";

                    dr.Close();

                    string sqlCar = @"
                        SELECT m.nommot, c.cid_recebedor, c.uf_recebedor, 
                               c.cheg_cliente, c.recebedor  
                        FROM tbcargas AS c  
                        INNER JOIN tbmotoristas AS m ON c.codmot = m.codmot 
                        WHERE carga = @idviagem";

                    SqlCommand cmdCar = new SqlCommand(sqlCar, conn);
                    cmdCar.Parameters.AddWithValue("@idviagem", idViagem);

                    SqlDataReader dr2 = cmdCar.ExecuteReader();

                    if (dr2.Read())
                    {
                        lblMotorista.Text = dr2["nommot"].ToString();
                        lblDestino.Text = dr2["recebedor"].ToString();
                        lblCidade.Text = dr2["cid_recebedor"].ToString() + "/" + dr2["uf_recebedor"].ToString();
                        SafeDateTimeValue(lblDataSaida.Text = dr2["cheg_cliente"].ToString());
                    }

                    dr2.Close();
                }
            }
               
        }

        protected void btnAbrirMdfe_Click(object sender, EventArgs e)
        {
            ddlFiltroStatus.SelectedValue = "Pendente";
            CarregarMdfeFiltro();
            ReabrirModal();

        }

        protected void FiltroChanged(object sender, EventArgs e)
        {
            CarregarMdfeFiltro();
            ScriptManager.RegisterStartupScript(this, GetType(),
                "openModal", "$('#modalMdfe').modal('show');", true);
        }
        
        
        void CarregarMdfeFiltro()
        {
            string sql = @"
                    SELECT id, status, mdfe_uf, mdfe_empresa, mdfe_numero, mdfe_serie,
                           mdfe_situacao, cid_expedidor, uf_expedidor,
                           cid_recebedor, uf_recebedor, mdfe_dv,
                           mdfe_baixado, mdfe_data_baixa
                    FROM tbcargas
                    WHERE mdfe IS NOT NULL
                ";

            if (!string.IsNullOrEmpty(ddlFiltroStatus.SelectedValue))
                sql += " AND mdfe_situacao = @Status";
            if (!string.IsNullOrEmpty(txtPesquisarMDFe.Text))
            {
                // Ajustado: Coluna antes do LIKE e parênteses para isolar o OR
                sql += " AND (mdfe_numero = @Pesquisa OR mdfe_empresa = @Pesquisa OR clidestino LIKE @PesquisaLike or carga=@Pesquisa)";
            }

            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                if (!string.IsNullOrEmpty(ddlFiltroStatus.SelectedValue))
                    cmd.Parameters.AddWithValue("@Status", ddlFiltroStatus.SelectedValue);
                if (!string.IsNullOrEmpty(txtPesquisarMDFe.Text))
                {
                    cmd.Parameters.AddWithValue("@Pesquisa", txtPesquisarMDFe.Text);
                    cmd.Parameters.AddWithValue("@PesquisaLike", "%" + txtPesquisarMDFe.Text + "%");
                }
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvMdfe.DataSource = dt;
                gvMdfe.DataBind();
                //Response.Write(dt.Rows.Count);

            }
        }
        protected void btnBaixarMDFe_Click(object sender, EventArgs e)
        {
            
            string usuario = Session["UsuarioLogado"].ToString() ?? "Sistema";
            bool selecionouAlgum = false;

            // Percorre cada linha da Grid
            foreach (GridViewRow row in gvMdfe.Rows)
            {
                // Encontra o CheckBox dentro da linha pelo ID
                CheckBox chk = (CheckBox)row.FindControl("chkSelecionar");

                if (chk != null && chk.Checked)
                {
                    selecionouAlgum = true;

                    // Pega o ID da linha (definido no DataKeyNames="id" da GridView)
                    string idMdf = gvMdfe.DataKeys[row.RowIndex].Value.ToString();

                    // Se precisar de outro valor de uma coluna BoundField (ex: Número do MDF-e na coluna 3)
                    // string numero = row.Cells[3].Text;

                    AtualizarBaixaNoBanco(idMdf, usuario);
                }
            }

            if (selecionouAlgum)
            {
                // Recarrega a grid para refletir as mudanças
                CarregarMdfeFiltro(); // Chame sua função que preenche a gvMdfe

                //ScriptManager.RegisterStartupScript(this, this.GetType(), "FecharModal",
                //    "$('#modalMdfe').modal('hide'); alert('MDF-e(s) baixado(s) com sucesso!');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "FecharEDesbloquear",
                    "$('#modalMdfe').modal('hide'); $('body').removeClass('modal-open'); $('.modal-backdrop').remove();", true);
            }
            else
            {
                // Opcional: avisar que nada foi selecionado sem fechar o modal
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Aviso", "alert('Selecione ao menos um item!');", true);
            }
            ScriptManager.RegisterStartupScript(this, GetType(),
                "openModal", "$('#modalMdfe').modal('show');", true);
        }

        private void AtualizarBaixaNoBanco(string id, string usuario)
        {
            string connStr = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                // Importante: Filtramos pelo ID da linha selecionada
                string sql = @"UPDATE tbcargas 
                       SET mdfe_situacao = 'Baixado', 
                           mdfe_baixado = @usuario, 
                           mdfe_data_baixa = GETDATE() 
                       WHERE id = @id";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@usuario", usuario);
                cmd.Parameters.AddWithValue("@id", id);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        protected void btnCancelarMDFe_Click(object sender, EventArgs e)
        {
            
            ScriptManager.RegisterStartupScript(this, this.GetType(), "FecharModal",
                    "$('#modalMdfe').modal('hide');", true);
        }

        void ExecutarSql(string sql, string usuario)
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                if (usuario != null)
                    cmd.Parameters.AddWithValue("@usuario", usuario);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            CarregarMdfeFiltro();
            ScriptManager.RegisterStartupScript(this, GetType(),
                "openModal", "$('#modalMdfe').modal('show');", true);
        }
        void ReabrirModal()
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(),
                "modalMdfe",
                "var modal = new bootstrap.Modal(document.getElementById('modalMdfe')); modal.show();",
                true);
        }

        protected void gvMdfe_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "CancelarMDF")
            {
                // O CommandArgument contém o ID que passamos no Eval("id")
                string idMdf = e.CommandArgument.ToString();

                CancelarMDFNoBanco(idMdf);

                // Atualiza a Grid para mostrar que os dados sumiram/mudaram
                CarregarMdfeFiltro();

                // Alerta opcional via JS
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('MDF-e cancelado com sucesso!');", true);
            }
        }

        private void CancelarMDFNoBanco(string id)
        {
            string connStr = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                // SQL baseado no seu modelo, filtrando estritamente pelo ID da linha
                string sql = @"
            UPDATE tbcargas 
            SET mdfe = NULL, 
                mdfe_situacao = NULL, 
                mdfe_empresa = NULL, 
                mdfe_numero = NULL, 
                mdfe_serie = NULL, 
                mdfe_uf = NULL, 
                mdfe_dv = NULL, 
                mdfe_baixado = NULL, 
                mdfe_data_baixa = NULL 
            WHERE id = @id";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        protected string GetSituacaoMDFE(object obj)
        {
            string situacao = (obj == null || obj == DBNull.Value)
                ? "Pendente"
                : obj.ToString().Trim();

            string classe = "bg-warning text-dark"; // padrão = Pendente

            switch (situacao.ToUpper())
            {
                case "BAIXADO":
                    classe = "bg-success";
                    break;

                case "CANCELADO":
                    classe = "bg-danger";
                    break;

                case "ENCERRADO":
                    classe = "bg-primary";
                    break;

                case "PENDENTE":
                    classe = "bg-warning text-dark";
                    break;
            }

            return $"<span class='badge {classe}'>{situacao}</span>";
        }
    }
}