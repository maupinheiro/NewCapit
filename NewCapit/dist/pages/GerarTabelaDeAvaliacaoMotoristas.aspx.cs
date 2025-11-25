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
                string query = "SELECT id, caminhofoto, codmot, nommot, cargo, tipomot, cadmot, frota, nucleo FROM tbmotoristas";

                if (statusSelecionados != null && statusSelecionados.Length > 0)
                {
                    string filtros = string.Join(",", statusSelecionados.Select((s, i) => "@status" + i));
                    query += $" WHERE nucleo IN ({filtros}) AND status='ATIVO'";
                    txtSelecionados.Text = string.Join("_", statusSelecionados);
                }

                SqlCommand cmd = new SqlCommand(query, conn);

                if (statusSelecionados != null)
                {
                    for (int i = 0; i < statusSelecionados.Length; i++)
                        cmd.Parameters.AddWithValue("@status" + i, statusSelecionados[i]);
                }

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvMotoristas.DataSource = dt;
                gvMotoristas.DataBind();


            }
        }
        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(dataInicial.Text))
            {
                // Acione o toast quando a página for carregada
                string script = "<script>showToast('Data inicial do período, está vazia!');</script>";
                ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                dataInicial.Focus();
                return;
            }
            DateTime dataDigitadaInicial;
            bool dataValidaInicial = DateTime.TryParse(dataInicial.Text, out dataDigitadaInicial);
            if (!dataValidaInicial)
            {
                string script = "<script>showToast('Data inválida! Digite no formato correto (ex: 06/10/2025).');</script>";
                ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                dataInicial.Focus();
                return;
            }

            DateTime dataDigitadaFinal;
            bool dataValidaFinal = DateTime.TryParse(dataFinal.Text, out dataDigitadaFinal);
            if (!dataValidaFinal)
            {
                string script = "<script>showToast('Data inválida! Digite no formato correto (ex: 06/10/2025).');</script>";
                ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                dataFinal.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(dataFinal.Text))
            {
                // Acione o toast quando a página for carregada
                string script = "<script>showToast('Data final do período, está vazia!');</script>";
                ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                dataFinal.Focus();
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
                btnGerarTabela.Enabled = true;
                if (!string.IsNullOrEmpty(valores))
                {
                    DateTime dt;
                    DateTime dtInicial = DateTime.Parse(dataInicial.Text);

                    int diaInicial = dtInicial.Day;
                    int mesInicial = dtInicial.Month;
                    int anoInicial = dtInicial.Year;

                    DateTime dtFinal = DateTime.Parse(dataFinal.Text);

                    int diaFinal = dtFinal.Day;
                    int mesFinal = dtFinal.Month;
                    int anoFinal = dtFinal.Year;

                    if (DateTime.TryParse(dataFinal.Text, out dt))
                    {
                        string nomeMes = dt.ToString("MMMM", new System.Globalization.CultureInfo("pt-BR"));
                        // lblMsg.Text = "Mês: " + nomeMes;
                    }

                    // valores vêm assim: "Pendente_Aprovado"
                    txtSelecionados.Text = nomeMes + "_" + anoFinal + "_" + valores;

                }
                string[] selecionados = ddlStatus.Items.Cast<System.Web.UI.WebControls.ListItem>()
                    .Where(x => x.Selected)
                    .Select(x => x.Value)
                    .ToArray();
                CarregarMotoristas(selecionados);

            }
        }

        protected void btnGerarTabela_Click(object sender, EventArgs e)
        {
            string nomeTabela = "AvaliacaoMotorista_"+txtSelecionados.Text.Replace(" ", "");           
            // 🔴 IMPORTANTE: nunca concatene diretamente — use parâmetros
            string sqlCheck = @"
            IF EXISTS (SELECT 1 FROM sys.tables WHERE name = @tabela)
                SELECT 1
            ELSE
                SELECT 0";

            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                conn.Open();

                SqlCommand cmdCheck = new SqlCommand(sqlCheck, conn);
                cmdCheck.Parameters.AddWithValue("@tabela", nomeTabela);

                int existe = Convert.ToInt32(cmdCheck.ExecuteScalar());

                if (existe == 1)
                {
                    string script2 = "<script>showToast('Já existe avaliação para a filial selecionada!');</script>";
                    ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script2);
                    return;
                }

                // 🔹 Criar a tabela caso não exista
                string sqlCriar = $@"
                CREATE TABLE [{nomeTabela}] (
                    Id INT IDENTITY(1,1) PRIMARY KEY,   
                    cracha VARCHAR(11),
                    nome VARCHAR(50),
                    funcao VARCHAR(45),
                    admissao DATE,
                    nucleo VARCHAR(45),
                    documentos VARCHAR(4),
                    pontualidade VARCHAR(4),
                    segcarga VARCHAR(4),
                    cargaedescarga VARCHAR(4),
                    comunicacao VARCHAR(4),
                    segtransito VARCHAR(4),
                    consumocomb VARCHAR(4),
                    conservacao VARCHAR(4),
                    mes VARCHAR(8),
                    frota VARCHAR(4),
                    observacao VARCHAR(500),
                    DataCriacao DATETIME DEFAULT GETDATE()
                );";

                SqlCommand cmdCriar = new SqlCommand(sqlCriar, conn);
                cmdCriar.ExecuteNonQuery();

                string script = "<script>showToast('Arquivo para avaliação criado com sucesso!');</script>";
                ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
            }
            // COPIAR OS DADOS DA GRID PARA A NOVA TABELA
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                conn.Open();

                foreach (GridViewRow row in gvMotoristas.Rows)
                {
                    DateTime dt;
                    DateTime dtInicial = DateTime.Parse(dataInicial.Text);

                    int diaInicial = dtInicial.Day;
                    int mesInicial = dtInicial.Month;
                    int anoInicial = dtInicial.Year;

                    DateTime dtFinal = DateTime.Parse(dataFinal.Text);

                    int diaFinal = dtFinal.Day;
                    int mesFinal = dtFinal.Month;
                    int anoFinal = dtFinal.Year;                    
                    string nomeMes = mesFinal.ToString("MM", new System.Globalization.CultureInfo("pt-BR"));


                    // PEGANDO OS VALORES DA GRID
                    //string foto = row.Cells[0].Text;
                    string cracha = row.Cells[1].Text;
                    string nome = row.Cells[2].Text;
                    string funcao = row.Cells[3].Text;
                    string admissao = row.Cells[4].Text;
                    string frota = row.Cells[5].Text;
                    string nucleo = row.Cells[6].Text;
                    string mes = nomeMes + "/" + anoFinal;                    
                    string observacao = "Avaliação referente ao mês de " + mes;                    

                    // COMANDO SQL PARA INSERIR NA TABELA
                    string sql = $@"INSERT INTO [{nomeTabela}] (cracha, nome, funcao, admissao, nucleo, documentos, pontualidade, segcarga, cargaedescarga, comunicacao, segtransito, consumocomb, conservacao, mes, frota, observacao)
                                   VALUES(@cracha, @nome, @funcao, @admissao, @nucleo, @documentos, @pontualidade, @segcarga, @cargaedescarga, @comunicacao, @segtransito, @consumocomb, @conservacao, @mes, @frota, @observacao)";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        //cmd.Parameters.AddWithValue("@foto", foto);
                        cmd.Parameters.AddWithValue("@cracha", cracha);
                        cmd.Parameters.AddWithValue("@nome", nome);
                        cmd.Parameters.AddWithValue("@funcao", funcao);
                        cmd.Parameters.AddWithValue("@admissao", SafeDateValue(admissao));
                        cmd.Parameters.AddWithValue("@nucleo", nucleo);
                        cmd.Parameters.AddWithValue("@documentos", "4");
                        cmd.Parameters.AddWithValue("@pontualidade", "2");
                        cmd.Parameters.AddWithValue("@segcarga", "2");
                        cmd.Parameters.AddWithValue("@cargaedescarga", "2");
                        cmd.Parameters.AddWithValue("@comunicacao", "1");
                        cmd.Parameters.AddWithValue("@segtransito", "3");
                        cmd.Parameters.AddWithValue("@consumocomb", "3");
                        cmd.Parameters.AddWithValue("@conservacao", "3");
                        cmd.Parameters.AddWithValue("@mes", mes);
                        cmd.Parameters.AddWithValue("@frota", frota);
                        cmd.Parameters.AddWithValue("@observacao", observacao);


                        cmd.ExecuteNonQuery();
                    }
                }

                string script = "<script>showToast('Dados copiados com sucesso!');</script>";
                ClientScript.RegisterStartupScript(this.GetType(), "ShowToast", script);
                return;
            }

        }

        private object SafeDateValue(string input)
        {
            DateTime dt;
            if (DateTime.TryParse(input, out dt))
                return dt.ToString("yyyy-MM-dd");
            else
                return DBNull.Value;
        }
    }



}




