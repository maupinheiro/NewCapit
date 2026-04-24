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

                Session["itens"] = new DataTable();
                CriarTabela();
                CarregarDescricao();
            } 
            txtData.Text = dataHoraAtual.ToString("dd/MM/yyyy HH:mm");
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
        }



        private void CriarTabela()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Produto");
            dt.Columns.Add("Quantidade");
            dt.Columns.Add("Estoque");

            Session["itens"] = dt;
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
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (fileOrcamento.HasFile)
            {
                byte[] arquivo = fileOrcamento.FileBytes;

                using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO tbRequisicaoAnexos (id_requisicao, nome_arquivo, arquivo) VALUES (@id, @nome, @arquivo)", conn);

                    cmd.Parameters.AddWithValue("@id", Session["idReq"]);
                    cmd.Parameters.AddWithValue("@nome", fileOrcamento.FileName);
                    cmd.Parameters.AddWithValue("@arquivo", arquivo);

                    cmd.ExecuteNonQuery();
                }
            }
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
        //public class ProdutoERP
        //{
        //    public string id_peca { get; set; }
        //    public string text { get; set; } // Select2 usa isso            
        //    public string descricao_peca { get; set; }
        //    public string unidade { get; set; }
        //}
        
        //[WebMethod]
        //private static DataTable BuscarNoBanco(string termo)
        //{
        //    DataTable dt = new DataTable();

        //    string connStr = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;

        //    using (SqlConnection conn = new SqlConnection(connStr))
        //    {
        //        string sql = @"
        //    SELECT TOP 20 id_peca, descricao_peca
        //    FROM tbestoque_pecas
        //    WHERE descricao_peca LIKE @termo
        //    ORDER BY descricao_peca";

        //        using (SqlCommand cmd = new SqlCommand(sql, conn))
        //        {
        //            cmd.Parameters.AddWithValue("@termo", "%" + termo + "%");

        //            SqlDataAdapter da = new SqlDataAdapter(cmd);
        //            da.Fill(dt);
        //        }
        //    }

        //    return dt;
        //}
        //public static List<object> BuscarProdutos(string termo)
        //{
        //    var lista = new List<object>();

        //    DataTable dt = BuscarNoBanco(termo);

        //    foreach (DataRow r in dt.Rows)
        //    {
        //        lista.Add(new
        //        {
        //            id = r["id_peca"].ToString(),
        //            text = r["descricao_peca"].ToString()
        //        });
        //    }

        //    return lista;
        //}


        //public static object BuscarProdutosERP(string termo, int pagina)
        //{
        //    int pageSize = 20;
        //    int offset = (pagina - 1) * pageSize;

        //    List<ProdutoERP> lista = new List<ProdutoERP>();

        //    string cs = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;

        //    using (SqlConnection conn = new SqlConnection(cs))
        //    {
        //        string sql = @"
        //        SELECT 
        //            id_peca,                    
        //            descricao_peca,
        //            unidade
        //        FROM tbestoque_pecas
        //        WHERE 
        //            id_peca LIKE @termo
        //            OR descricao_peca LIKE @termo
        //        ORDER BY descricao_peca
        //        OFFSET @offset ROWS
        //        FETCH NEXT @pageSize ROWS ONLY;
        //         ";

        //        SqlCommand cmd = new SqlCommand(sql, conn);
        //        cmd.Parameters.AddWithValue("@termo", "%" + termo + "%");
        //        cmd.Parameters.AddWithValue("@offset", offset);
        //        cmd.Parameters.AddWithValue("@pageSize", pageSize);

        //        conn.Open();

        //        SqlDataReader dr = cmd.ExecuteReader();

        //        while (dr.Read())
        //        {
        //            lista.Add(new ProdutoERP
        //            {
        //                id_peca = dr["id_peca"].ToString(),
        //                text = dr["id_peca"] + " - " + dr["descricao_peca"],
        //                descricao_peca = dr["descricao_peca"].ToString(),
        //                unidade = dr["unidade"].ToString()
        //            });
        //        }
        //    }

        //    return new
        //    {
        //        itens = lista,
        //        mais = lista.Count == pageSize
        //    };
        //}
    }
}