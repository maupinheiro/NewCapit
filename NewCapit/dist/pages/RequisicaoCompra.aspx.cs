using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Net;
using System.Web.Configuration;
using System.Configuration;
using System.Web.Services;
using System.IO;

namespace NewCapit.dist.pages
{
    public partial class RequisicaoCompra : System.Web.UI.Page
    {
        SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString());
        DateTime dataHoraAtual = DateTime.Now;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UsuarioLogado"] == null)
                {
                    Response.Redirect("Login.aspx");
                    return;
                }

                CarregarDescricao();                
                Session["ItensReq"] = CriarTabelaItens();

            }            
            CarregarGrid();
            txtData.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
        }
        private void CarregarDescricao()
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                conn.Open();
                string query = "SELECT id_peca, descricao_peca FROM tbestoque_pecas WHERE fl_exclusao is null AND status = 'ATIVO' ORDER BY descricao_peca";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                ddlDescricao.DataSource = reader;
                ddlDescricao.DataTextField = "descricao_peca";  // Campo a ser exibido
                ddlDescricao.DataValueField = "id_peca";  // Valor associado ao item
                ddlDescricao.DataBind();

                // Adicionar o item padrão
                ddlDescricao.Items.Insert(0, new ListItem("", "0"));
            }
        }
        private void PreencherDescricao(int id)
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                conn.Open();
                string query = "SELECT id_peca, descricao_peca, unidade, estoque_peca FROM tbestoque_pecas WHERE id_peca = @ID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtCodigo.Text = reader["id_peca"].ToString();
                    ddlDescricao.SelectedItem.Text = reader["descricao_peca"].ToString();
                    txtUnidade.Text = reader["unidade"].ToString();
                    txtEstoque.Text = reader["estoque_peca"].ToString();
                    txtQtd.Focus();
                }
            }
        }
        protected void txtCodigo_TextChanged(object sender, EventArgs e)
        {
            if (txtCodigo.Text != "")
            {
                string codigoDescricao = txtCodigo.Text.Trim();
                string strConn = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    string query = "SELECT id_peca, descricao_peca, unidade, estoque_peca FROM tbestoque_pecas WHERE id_peca = @Codigo";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Codigo", codigoDescricao);
                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtCodigo.Text = reader["id_peca"].ToString();
                                ddlDescricao.SelectedItem.Text = reader["descricao_peca"].ToString();
                                txtUnidade.Text = reader["unidade"].ToString();
                                txtEstoque.Text = reader["estoque_peca"].ToString();
                                txtQtd.Focus();
                            }
                            else
                            {
                                Mensagem("danger", "Código do produto, não encontrado. Verifique o código " + txtCodigo.Text.Trim() + " digitado.");
                                ddlDescricao.ClearSelection();
                                txtCodigo.Text = string.Empty;                                
                                txtCodigo.Focus();                                
                            }
                        }
                    }

                }

            }
        }
        protected void Mensagem(string tipo, string texto)
        {
            divMsg.Visible = true;

            divMsg.Attributes["class"] =
                "alert alert-" + tipo + " alert-dismissible fade show mt-3";

            lblMsgGeral.Text = texto;
        }
        protected void ddlDescricao_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idSelecionado = int.Parse(ddlDescricao.SelectedValue);

            // Preencher os campos com base no valor selecionado
            if (idSelecionado > 0)
            {
                PreencherDescricao(idSelecionado);
            }
            else
            {
                LimparCampos();
            }
        }
        private void LimparCampos()
        {
            txtCodigo.Text = string.Empty;
            txtUnidade.Text = string.Empty;
            txtEstoque.Text = string.Empty;
            ddlDescricao.SelectedItem.Text = string.Empty;
            txtAplicacao.Text = string.Empty;
        }
        protected void btnAddItem_Click(object sender, EventArgs e)
        {
            string codigo = txtCodigo.Text.Trim();
            string descricao = ddlDescricao.SelectedItem.Text;
            string unidade = txtUnidade.Text.Trim();
            string quantidade = txtQtd.Text.Trim();
            string aplicacao = txtAplicacao.Text.Trim();

            if (string.IsNullOrEmpty(codigo))
                return;

            DataTable dt = Session["ItensReq"] as DataTable;

            // 🔴 VERIFICAR DUPLICIDADE
            bool existe = dt.AsEnumerable()
                            .Any(row => row["Codigo"].ToString() == codigo);

            if (existe)
            {                      
                Mensagem("warning", "Produto já inserido na requisição " + txtCodigo.Text.Trim() + ".");
                txtCodigo.Text = "";
                txtCodigo.Focus();
                return;
            }

            // ✔️ ADICIONAR ITEM
            DataRow dr = dt.NewRow();
            dr["Codigo"] = codigo;
            dr["Descricao"] = descricao;
            dr["Unidade"] = unidade;
            dr["Quantidade"] = quantidade;
            dr["Aplicacao"] = aplicacao;
            dt.Rows.Add(dr);

            // Atualiza Session
            Session["ItensReq"] = dt;

            // Atualiza Grid
            gvItens.DataSource = dt;
            gvItens.DataBind();

            txtCodigo.Text = "";
            ddlDescricao.SelectedItem.Text = "";
            txtUnidade.Text = "";
            txtQtd.Text = "";
            txtAplicacao.Text = "";
        }
        //protected void btnExcel_Click(object sender, EventArgs e)
        //{
        //    AtualizarItensGrid();

        //    Response.Clear();
        //    Response.Buffer = true;
        //    Response.AddHeader("content-disposition", "attachment;filename=Requisicao.xls");
        //    Response.ContentType = "application/vnd.ms-excel";

        //    StringWriter sw = new StringWriter();

        //    sw.Write("<html><head>");
        //    sw.Write("<style>");
        //    sw.Write("table {border-collapse: collapse;}");
        //    sw.Write("td, th {border: 1px solid black; padding: 5px;}");
        //    sw.Write("th {background-color: #eeeeee;}");
        //    sw.Write("</style>");
        //    sw.Write("</head><body>");

        //    // CABEÇALHO
        //    sw.Write("<h2>REQUISIÇÃO DE COMPRA</h2>");
        //    sw.Write("<b>Número:</b> " + txtNumero.Text + "<br/>");
        //    sw.Write("<b>Usuário:</b> " + Session["UsuarioLogado"] + "<br/>");
        //    sw.Write("<b>Data:</b> " + DateTime.Now.ToString("dd/MM/yyyy") + "<br/><br/>");

        //    // TABELA
        //    sw.Write("<table>");
        //    sw.Write("<tr><th>Produto</th><th>Quantidade</th><th>Observação</th></tr>");

        //    foreach (DataRow row in Itens.Rows)
        //    {
        //        sw.Write("<tr>");
        //        sw.Write("<td>" + row["produto"] + "</td>");
        //        sw.Write("<td>" + row["quantidade"] + "</td>");
        //        sw.Write("<td>" + row["observacao"] + "</td>");
        //        sw.Write("</tr>");
        //    }

        //    sw.Write("</table>");

        //    sw.Write("</body></html>");

        //    Response.Write(sw.ToString());
        //    Response.End();
        //}        
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            //AtualizarItensGrid();

            using (SqlConnection conn = new SqlConnection(
                WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();

                try
                {
                    // GERAR NÚMERO
                    string sql = @"
                    DECLARE @ano VARCHAR(4) = YEAR(GETDATE());
                    DECLARE @seq INT;

                    SELECT @seq = ISNULL(MAX(CAST(RIGHT(numero,5) AS INT)), 0) + 1
                    FROM tbRequisicaoCompra WITH (UPDLOCK, HOLDLOCK)
                    WHERE numero LIKE 'REQ-' + @ano + '-%';

                    INSERT INTO tbRequisicaoCompra 
                    (numero, data_criacao, usuario)
                    OUTPUT INSERTED.id_requisicao, INSERTED.numero
                    VALUES (
                        'REQ-' + @ano + '-' + RIGHT('00000' + CAST(@seq AS VARCHAR), 5),
                        GETDATE(),
                        @usuario
                    );
                    ";

                    SqlCommand cmd = new SqlCommand(sql, conn, trans);
                    cmd.Parameters.AddWithValue("@usuario", Session["UsuarioLogado"]);

                    SqlDataReader dr = cmd.ExecuteReader();

                    int idReq = 0;
                    string numero = "";

                    if (dr.Read())
                    {
                        idReq = Convert.ToInt32(dr["id_requisicao"]);
                        numero = dr["numero"].ToString();
                    }
                    dr.Close();

                    // SALVAR ITENS
                    //foreach (DataRow row in Itens.Rows)
                    //{
                    //    if (!string.IsNullOrWhiteSpace(row["produto"].ToString()))
                    //    {
                    //        string sqlItem = @"
                    //INSERT INTO tbRequisicaoCompraItem
                    //(id_requisicao, produto, quantidade, observacao)
                    //VALUES (@id, @produto, @qtd, @obs)
                    //";

                    //        SqlCommand cmdItem = new SqlCommand(sqlItem, conn, trans);

                    //        cmdItem.Parameters.AddWithValue("@id", idReq);
                    //        cmdItem.Parameters.AddWithValue("@produto", row["produto"]);
                    //        cmdItem.Parameters.AddWithValue("@qtd", Convert.ToDecimal(row["quantidade"]));
                    //        cmdItem.Parameters.AddWithValue("@obs", row["observacao"]);

                    //        cmdItem.ExecuteNonQuery();
                    //    }
                    //}

                    trans.Commit();

                    txtNumero.Text = numero;

                    ScriptManager.RegisterStartupScript(this, GetType(), "ok",
                        "alert('Requisição salva: " + numero + "');", true);
                }
                catch (Exception ex)
                {
                    trans.Rollback();

                    ScriptManager.RegisterStartupScript(this, GetType(), "erro",
                        "alert('Erro: " + ex.Message.Replace("'", "") + "');", true);
                }
            }
        }        
        private DataTable CriarTabelaItens()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Codigo");
            dt.Columns.Add("Descricao");
            dt.Columns.Add("Unidade");
            dt.Columns.Add("Quantidade");
            dt.Columns.Add("Aplicacao");

            return dt;
        }
        protected void gvItens_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Excluir")
            {
                int index = Convert.ToInt32(e.CommandArgument);

                DataTable dt = Session["ItensReq"] as DataTable;

                if (dt.Rows.Count > index)
                {
                    dt.Rows.RemoveAt(index);
                }

                Session["ItensReq"] = dt;

                CarregarGrid();
            }
        }
        private void CarregarGrid()
        {
            DataTable dt = Session["ItensReq"] as DataTable;

            gvItens.DataSource = dt;
            gvItens.DataBind();
        }                        
        private void ExcluirAnexo(int id)
        {
            string caminhoArquivo = "";

            using (SqlConnection conn = new SqlConnection(
                WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                conn.Open();

                // Buscar caminho do arquivo
                SqlCommand cmdSelect = new SqlCommand(
                    "SELECT CaminhoArquivo FROM tbAnexosRequisicaoCompras WHERE Id = @Id", conn);
                cmdSelect.Parameters.AddWithValue("@Id", id);

                object result = cmdSelect.ExecuteScalar();
                if (result != null)
                {
                    caminhoArquivo = Server.MapPath(result.ToString());
                }

                // Excluir do banco
                SqlCommand cmdDelete = new SqlCommand(
                    "DELETE FROM tbAnexosRequisicaoCompras WHERE Id = @Id", conn);
                cmdDelete.Parameters.AddWithValue("@Id", id);
                cmdDelete.ExecuteNonQuery();
            }

            // Excluir arquivo físico
            if (!string.IsNullOrEmpty(caminhoArquivo) && File.Exists(caminhoArquivo))
            {
                File.Delete(caminhoArquivo);
            }
        }        
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            List<string> ok = new List<string>();
            List<string> erro = new List<string>();

            if (fileUploadAnexo.HasFiles)
            {
                foreach (HttpPostedFile arquivo in fileUploadAnexo.PostedFiles)
                {
                    string nomeOriginal = Path.GetFileName(arquivo.FileName);

                    // tamanho
                    if (arquivo.ContentLength > 5 * 1024 * 1024)
                    {
                        erro.Add(nomeOriginal + " grande");
                        continue;
                    }

                    string extensao = Path.GetExtension(nomeOriginal).ToLower();

                    if (!new[] { ".pdf", ".jpg", ".png", ".jpeg" }.Contains(extensao))
                    {
                        erro.Add(nomeOriginal + " inválido");
                        continue;
                    }

                    string nomeArquivo = Guid.NewGuid() + "_" + nomeOriginal;

                    string pasta = Server.MapPath("~/Anexos/");
                    if (!Directory.Exists(pasta))
                        Directory.CreateDirectory(pasta);

                    string caminho = Path.Combine(pasta, nomeArquivo);
                    arquivo.SaveAs(caminho);

                    using (SqlConnection conn = new SqlConnection(
                        WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
                    {
                        string sql = @"INSERT INTO tbAnexosRequisicaoCompras
                (NumeroRequisicao, NomeArquivo, NomeOriginal, CaminhoArquivo, DataUpload, Usuario)
                VALUES (@Numero, @Nome, @NomeOriginal, @Caminho, GETDATE(), @Usuario)";

                        SqlCommand cmd = new SqlCommand(sql, conn);
                        cmd.Parameters.AddWithValue("@Numero", txtNumero.Text);
                        cmd.Parameters.AddWithValue("@Nome", nomeArquivo);
                        cmd.Parameters.AddWithValue("@NomeOriginal", nomeOriginal);
                        cmd.Parameters.AddWithValue("@Caminho", "~/Anexos/" + nomeArquivo);
                        cmd.Parameters.AddWithValue("@Usuario", Session["UsuarioLogado"].ToString());

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }

                    ok.Add(nomeOriginal);
                }

                if (ok.Count > 0)
                    Mensagem("success", "OK: " + string.Join(", ", ok));

                if (erro.Count > 0)
                    Mensagem("warning", "Erro: " + string.Join(", ", erro));

                CarregarAnexos();
            }
        }
        protected void gvAnexos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Excluir")
            {
                int id = Convert.ToInt32(e.CommandArgument);

                using (SqlConnection conn = new SqlConnection(
                    WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT CaminhoArquivo FROM tbAnexosRequisicaoCompras WHERE Id=@Id", conn);
                    cmd.Parameters.AddWithValue("@Id", id);

                    string caminho = Server.MapPath(cmd.ExecuteScalar().ToString());

                    SqlCommand del = new SqlCommand("DELETE FROM tbAnexosRequisicaoCompras WHERE Id=@Id", conn);
                    del.Parameters.AddWithValue("@Id", id);
                    del.ExecuteNonQuery();

                    if (File.Exists(caminho))
                        File.Delete(caminho);
                }

                CarregarAnexos();
            }
        }
        private void CarregarAnexos()
        {
            using (SqlConnection conn = new SqlConnection(
                WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                string sql = "SELECT * FROM tbAnexosRequisicaoCompras WHERE NumeroRequisicao=@Numero";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Numero", txtNumero.Text);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvAnexos.DataSource = dt;
                gvAnexos.DataBind();
            }
        }








        protected void btnSalvarItem_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)Session["itens"];

            DataRow dr = dt.NewRow();
            dr["Produto"] = ddlDescricao.SelectedItem.Text;
            dr["Quantidade"] = txtQuantidade.Text;
            dr["Estoque"] = BuscarEstoque(ddlDescricao.SelectedItem.Text);

            dt.Rows.Add(dr);

            gvItens.DataSource = dt;
            gvItens.DataBind();
        }
        private string BuscarEstoque(string produto)
        {
            string estoque = "0";

            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT estoque_peca FROM tbestoque_pecas WHERE descricao_peca = @produto", conn);
                cmd.Parameters.AddWithValue("@produto", produto);

                var result = cmd.ExecuteScalar();
                if (result != null)
                    estoque = result.ToString();
            }

            return estoque;
        }        
        protected void btnEnviar_Click(object sender, EventArgs e)
        {
            //PENDENTE, APROVADO, REJEITADO, LIBERADO
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(@"
                INSERT INTO tbRequisicaoCompra (solicitante, setor, status, observacao)
                OUTPUT INSERTED.ID
                VALUES (@sol, @setor, 'PENDENTE', @obs)", conn);

                cmd.Parameters.AddWithValue("@sol", txtSolicitante.Text);
                cmd.Parameters.AddWithValue("@setor", txtSetor.Text);
                cmd.Parameters.AddWithValue("@obs", txtObservacao.Text);

                int id = (int)cmd.ExecuteScalar();

                Session["idReq"] = id;

                SalvarItens(id);
                EnviarEmailGerente();
            }
        }
        private void SalvarItens(int id)
        {
            DataTable dt = (DataTable)Session["itens"];

            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                conn.Open();

                foreach (DataRow dr in dt.Rows)
                {
                    SqlCommand cmd = new SqlCommand(@"
            INSERT INTO tbRequisicaoItens (id_requisicao, produto, quantidade, estoque_atual)
            VALUES (@id, @prod, @qtd, @est)", conn);

                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@prod", dr["Produto"]);
                    cmd.Parameters.AddWithValue("@qtd", dr["Quantidade"]);
                    cmd.Parameters.AddWithValue("@est", dr["Estoque"]);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        private void EnviarEmailGerente()
        {
            MailMessage mail = new MailMessage();
            mail.To.Add("gsaprevi@gmail.com");
            mail.Subject = "Nova Requisição";
            mail.Body = "Existe uma requisição pendente de aprovação.";

            SmtpClient smtp = new SmtpClient("smtp.office365.com", 587);
            smtp.Credentials = new NetworkCredential("progtrans2@transnovag.com.br", "Sascar@2007");
            smtp.EnableSsl = true;
            smtp.Send(mail);
        }
        protected void btnAprovar_Click(object sender, EventArgs e)
        {
            string base64 = hfAssinatura.Value;
            byte[] assinatura = Convert.FromBase64String(base64.Replace("data:image/png;base64,", ""));

            using (SqlConnection conn = new SqlConnection("SUA_CONEXAO"))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(@"
                UPDATE tbRequisicaoCompra
                SET status = 'LIBERADO',
                    assinatura = @assinatura,
                    aprovado_por = @usuario,
                    data_aprovacao = GETDATE()
                WHERE id = @id", conn);

                cmd.Parameters.AddWithValue("@assinatura", assinatura);
                cmd.Parameters.AddWithValue("@usuario", "GERENTE");
                cmd.Parameters.AddWithValue("@id", Request.QueryString["id"]);

                cmd.ExecuteNonQuery();
            }
        }

        
    }
}