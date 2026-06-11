using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NewCapit.dist.pages
{
    public partial class ControleAcesso2 : System.Web.UI.Page
    {
        string id;
        private SqlConnection ObterConexao()
        {
            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["conexAO"].ConnectionString;
            return new SqlConnection(connString);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregarUsuarioPorQueryString();
            }
        }

        private void CarregarUsuarioPorQueryString()
        {
            // 1. Valida de forma segura se a QueryString "id" existe e não está vazia
            if (Request.QueryString["id"] == null || string.IsNullOrEmpty(Request.QueryString["id"].ToString()))
            {
                pnlPermissoes.Visible = false;
                btnSalvarPermissoes.Enabled = false;
                return;
            }

            string idUsuarioUrl = Request.QueryString["id"].ToString();

            using (SqlConnection conn = ObterConexao())
            {
                // 2. Query usando parâmetros para evitar SQL Injection
                string query = "SELECT cod_usuario, nm_usuario,nm_nome, fl_status, fl_tipo FROM tb_usuario WHERE cod_usuario = @IdUsuario";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@IdUsuario", idUsuarioUrl);

                conn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        // Preenche a Label com o nome do usuário encontrado
                        lblUsuario.Text = dr["nm_usuario"].ToString();
                        lblNome.Text = dr["nm_nome"].ToString().ToUpper();
                        // Salva o ID no ViewState para podermos usar depois no clique do botão Salvar
                        ViewState["IdUsuarioPermissao"] = dr["cod_usuario"].ToString();
                        ddlNivelAcesso.SelectedValue = dr["fl_tipo"].ToString();
                        ddlStatusAcesso.SelectedValue = dr["fl_status"].ToString();
                        // 3. Ativa a tela e carrega as permissões do usuário
                        pnlPermissoes.Visible = true;
                        btnSalvarPermissoes.Enabled = true;

                        // Dispara o método que varre os módulos e preenche as tabelas/checkboxes
                        CarregarModulosETelas();
                    }
                    else
                    {
                        // Caso o ID passado na URL não exista na tabela tb_usuario
                        pnlPermissoes.Visible = false;
                        btnSalvarPermissoes.Enabled = false;
                        lblUsuario.Text = "Usuário não encontrado";

                        lblMsg.InnerText = "O usuário informado não foi encontrado no sistema.";
                        divMsg.Attributes["class"] = "alert alert-warning alert-dismissible fade show mt-3";
                        divMsg.Style["display"] = "block";
                    }
                }
            }
        }


        private void CarregarModulosETelas()
        {
            using (SqlConnection conn = ObterConexao())
            {
                string query = "SELECT id_modulo, nome_modulo FROM tb_modulo ORDER BY nome_modulo";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dtModulos = new DataTable();
                da.Fill(dtModulos);

                rptModulos.DataSource = dtModulos;
                rptModulos.DataBind();
            }
        }

        // Esse evento roda para CADA módulo. Ele vai buscar as telas pertencentes àquele módulo específico
        protected void rptModulos_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                // 1. Captura o id_modulo que está a ser renderizado neste Card
                DataRowView rowModulo = (DataRowView)e.Item.DataItem;
                int idModulo = Convert.ToInt32(rowModulo["id_modulo"]);

                // Encontra o checkbox/switch do módulo
                CheckBox chkMod = (CheckBox)e.Item.FindControl("chkMod");

                // 2. Captura o ID do usuário que está a ser editado. 
                // (Substitua 'txtIdUsuario.Text' ou a variável que guarda o ID do usuário que você está editando)
                int idUsuarioEditado = Convert.ToInt32(Request.QueryString["id"] ?? "0");
                // Se você guarda o ID num HiddenField ou TextBox, use por exemplo: Convert.ToInt32(hfIdUsuario.Value);

                // 3. Vamos criar tabelas em memória para guardar o que vem do banco
                DataTable dtTelasDoModulo = new DataTable();
                DataTable dtPermissoesAtuais = new DataTable();

                // Substitua 'ObterConexao()' pelo seu método real de conexão (ex: seu SqlConnection)
                using (SqlConnection conn = ObterConexao())
                {
                    conn.Open();

                    // CONSULTA 1: Trazer as telas pertencentes a ESTE módulo específico
                    string sqlTelas = "SELECT IdTela, NomeTela, ArquivoPhysical FROM Cad_Telas WHERE id_modulo = @id_modulo";
                    using (SqlCommand cmdTelas = new SqlCommand(sqlTelas, conn))
                    {
                        cmdTelas.Parameters.AddWithValue("@id_modulo", idModulo);
                        using (SqlDataAdapter da = new SqlDataAdapter(cmdTelas))
                        {
                            da.Fill(dtTelasDoModulo);
                        }
                    }

                    // CONSULTA 2: Trazer todas as permissões que esse usuário JÁ TEM salvas para este módulo
                    string sqlPermissoes = @"SELECT up.IdTela, up.IdAcao 
                                     FROM Usuario_permissao up
                                     INNER JOIN Cad_Telas t ON up.IdTela = t.IdTela
                                     WHERE up.IdUsuario = @IdUsuario AND t.id_modulo = @id_modulo";
                    using (SqlCommand cmdPerm = new SqlCommand(sqlPermissoes, conn))
                    {
                        cmdPerm.Parameters.AddWithValue("@IdUsuario", idUsuarioEditado);
                        cmdPerm.Parameters.AddWithValue("@id_modulo", idModulo);
                        using (SqlDataAdapter da = new SqlDataAdapter(cmdPerm))
                        {
                            da.Fill(dtPermissoesAtuais);
                        }
                    }
                }

                // 4. Se o usuário já tiver qualquer permissão gravada para este módulo, ativamos o switch do Módulo
                if (dtPermissoesAtuais.Rows.Count > 0)
                {
                    chkMod.Checked = true;
                }

                // 5. Vincula o Repeater de Telas interno com as telas do módulo atual
                Repeater rptTelas = (Repeater)e.Item.FindControl("rptTelas");
                if (rptTelas != null)
                {
                    rptTelas.DataSource = dtTelasDoModulo;
                    rptTelas.DataBind();

                    // 6. Agora varremos as linhas que o rptTelas acabou de criar para marcar os Checkboxes
                    foreach (RepeaterItem itemTela in rptTelas.Items)
                    {
                        HiddenField hfIdTela = (HiddenField)itemTela.FindControl("hfIdTela");
                        int idTela = Convert.ToInt32(hfIdTela.Value);

                        CheckBox chkVisualizar = (CheckBox)itemTela.FindControl("chkVisualizar");
                        CheckBox chkInserir = (CheckBox)itemTela.FindControl("chkInserir");
                        CheckBox chkAlterar = (CheckBox)itemTela.FindControl("chkAlterar");
                        CheckBox chkExcluir = (CheckBox)itemTela.FindControl("chkExcluir");

                        // Varre o DataTable de permissões procurando se existe a combinação (IdTela e IdAcao)
                        // Inserir = 1, Alterar = 2, Excluir = 3, Visualizar = 4
                        chkInserir.Checked = dtPermissoesAtuais.AsEnumerable().Any(r => r.Field<int>("IdTela") == idTela && r.Field<int>("IdAcao") == 1);
                        chkAlterar.Checked = dtPermissoesAtuais.AsEnumerable().Any(r => r.Field<int>("IdTela") == idTela && r.Field<int>("IdAcao") == 2);
                        chkExcluir.Checked = dtPermissoesAtuais.AsEnumerable().Any(r => r.Field<int>("IdTela") == idTela && r.Field<int>("IdAcao") == 3);
                        chkVisualizar.Checked = dtPermissoesAtuais.AsEnumerable().Any(r => r.Field<int>("IdTela") == idTela && r.Field<int>("IdAcao") == 4);
                    }
                }
            }
        }

        private DataTable ObterTelasEPermissoesDoModulo(int idModulo, int idUsuario)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = ObterConexao())
            {
                // Traz as telas do módulo e faz um LEFT JOIN com as permissões salvas do usuário atual
                string query = @"
                SELECT 
                    t.IdTela, 
                    t.NomeTela, 
                    t.ArquivoPhysical,
                    MAX(CASE WHEN p.IdAcao = 1 THEN 1 ELSE 0 END) AS Inserir,
                    MAX(CASE WHEN p.IdAcao = 2 THEN 1 ELSE 0 END) AS Alterar,
                    MAX(CASE WHEN p.IdAcao = 3 THEN 1 ELSE 0 END) AS Excluir,
                    MAX(CASE WHEN p.IdAcao = 4 THEN 1 ELSE 0 END) AS Visualizar
                FROM Cad_Telas t
                LEFT JOIN Usuario_permissao p ON t.IdTela = p.IdTela AND p.IdUsuario = @IdUsuario
                WHERE t.id_modulo = @IdModulo
                GROUP BY t.IdTela, t.NomeTela, t.ArquivoPhysical
                ORDER BY t.NomeTela";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                cmd.Parameters.AddWithValue("@IdModulo", idModulo);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }

        private void PreencherCheckboxesPermissoes(Repeater rptTelas, DataTable dtTelas)
        {
            for (int i = 0; i < rptTelas.Items.Count; i++)
            {
                RepeaterItem item = rptTelas.Items[i];
                DataRow row = dtTelas.Rows[i];

                CheckBox chkInserir = (CheckBox)item.FindControl("chkInserir");
                CheckBox chkAlterar = (CheckBox)item.FindControl("chkAlterar");
                CheckBox chkExcluir = (CheckBox)item.FindControl("chkExcluir");
                CheckBox chkVisualizar = (CheckBox)item.FindControl("chkVisualizar");

                chkInserir.Checked = Convert.ToInt32(row["Inserir"]) == 1;
                chkAlterar.Checked = Convert.ToInt32(row["Alterar"]) == 1;
                chkExcluir.Checked = Convert.ToInt32(row["Excluir"]) == 1;
                chkVisualizar.Checked = Convert.ToInt32(row["Visualizar"]) == 1;
            }
        }

        protected void btnSalvarPermissoes_Click(object sender, EventArgs e)
        {
            int idUsuario = Convert.ToInt32(ViewState["IdUsuarioPermissao"]);

            // Criamos uma coleção para armazenar os IDs dos módulos selecionados sem duplicados
            HashSet<int> modulosSelecionados = new HashSet<int>();

            using (SqlConnection conn = ObterConexao())
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                SqlTransaction trans = conn.BeginTransaction();

                try
                {
                    // ============================================================================
                    // PASSO 1: Deletar todas as permissões antigas do usuário dentro da transação
                    // ============================================================================
                    string deleteQuery = "DELETE FROM Usuario_permissao WHERE IdUsuario = @IdUsuario";
                    using (SqlCommand cmdDelete = new SqlCommand(deleteQuery, conn, trans))
                    {
                        cmdDelete.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = idUsuario;
                        cmdDelete.ExecuteNonQuery();
                    }

                    // ============================================================================
                    // PASSO 2: Preparar o comando de inserção para o que estiver selecionado
                    // ============================================================================
                    string insertQuery = "INSERT INTO Usuario_permissao (IdUsuario, IdTela, IdAcao) VALUES (@IdUsuario, @IdTela, @IdAcao)";
                    SqlCommand cmdInsert = new SqlCommand(insertQuery, conn, trans);
                    cmdInsert.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = idUsuario;
                    cmdInsert.Parameters.Add("@IdTela", SqlDbType.Int);
                    cmdInsert.Parameters.Add("@IdAcao", SqlDbType.Int);

                    // Varre os Repeaters capturando o que está checado em tela
                    foreach (RepeaterItem itemModulo in rptModulos.Items)
                    {
                        // Captura o id_modulo direto do DataKeys ou de um HiddenField do Módulo se houver.
                        // Caso não tenha mapeado, garanta que existe um HiddenField com o id_modulo no layout do rptModulos:
                        HiddenField hfIdModulo = (HiddenField)itemModulo.FindControl("hfIdModulo");
                        int idModulo = Convert.ToInt32(hfIdModulo.Value);

                        CheckBox chkMod = (CheckBox)itemModulo.FindControl("chkMod");
                        bool moduloTeveSelecao = false;

                        Repeater rptTelas = (Repeater)itemModulo.FindControl("rptTelas");
                        if (rptTelas != null)
                        {
                            foreach (RepeaterItem itemTela in rptTelas.Items)
                            {
                                HiddenField hfIdTela = (HiddenField)itemTela.FindControl("hfIdTela");
                                int idTela = Convert.ToInt32(hfIdTela.Value);

                                CheckBox chkInserir = (CheckBox)itemTela.FindControl("chkInserir");
                                CheckBox chkAlterar = (CheckBox)itemTela.FindControl("chkAlterar");
                                CheckBox chkExcluir = (CheckBox)itemTela.FindControl("chkExcluir");
                                CheckBox chkVisualizar = (CheckBox)itemTela.FindControl("chkVisualizar");

                                cmdInsert.Parameters["@IdTela"].Value = idTela;

                                // Só vai inserir no banco o que permanecer checado pelo administrador
                                if (chkInserir.Checked) { cmdInsert.Parameters["@IdAcao"].Value = 1; cmdInsert.ExecuteNonQuery(); moduloTeveSelecao = true; }
                                if (chkAlterar.Checked) { cmdInsert.Parameters["@IdAcao"].Value = 2; cmdInsert.ExecuteNonQuery(); moduloTeveSelecao = true; }
                                if (chkExcluir.Checked) { cmdInsert.Parameters["@IdAcao"].Value = 3; cmdInsert.ExecuteNonQuery(); moduloTeveSelecao = true; }
                                if (chkVisualizar.Checked) { cmdInsert.Parameters["@IdAcao"].Value = 4; cmdInsert.ExecuteNonQuery(); moduloTeveSelecao = true; }
                            }
                        }

                        // Regra opcional/segura: Se o Switch do módulo estiver ligado OU alguma tela dele foi marcada, adiciona na lista
                        if ((chkMod != null && chkMod.Checked) || moduloTeveSelecao)
                        {
                            modulosSelecionados.Add(idModulo);
                        }
                    }

                    // ============================================================================
                    // PASSO 3: Juntar os IDs por vírgula e atualizar a tabela tb_usuario
                    // ============================================================================
                    string stringModulos = string.Join(",", modulosSelecionados);

                    string updateUsuarioQuery = "UPDATE tb_usuario SET fl_permissao = @FlPermissao WHERE cod_usuario = @IdUsuario";
                    using (SqlCommand cmdUpdate = new SqlCommand(updateUsuarioQuery, conn, trans))
                    {
                        cmdUpdate.Parameters.Add("@FlPermissao", SqlDbType.VarChar, 250).Value = stringModulos;
                        cmdUpdate.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = idUsuario;
                        cmdUpdate.ExecuteNonQuery();
                    }

                    // Se tudo correu bem, confirma as deleções, inserções e o update juntos
                    trans.Commit();

                    lblMsg.InnerText = "Permissões e módulos atualizados com sucesso!";
                    divMsg.Attributes["class"] = "alert alert-success alert-dismissible fade show mt-3";
                    divMsg.Style["display"] = "block";
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    lblMsg.InnerText = "Erro ao salvar alterações: " + ex.Message;
                    divMsg.Attributes["class"] = "alert alert-danger alert-dismissible fade show mt-3";
                    divMsg.Style["display"] = "block";
                }
            }
        }

        public void AlterarUsuatio()
        {
            using (SqlConnection conn = ObterConexao())
            {
                conn.Open();


                if (Request.QueryString["id"] == null || string.IsNullOrEmpty(Request.QueryString["id"].ToString()))
                {
                    
                    return;
                }

                string idUsuarioUrl = Request.QueryString["id"].ToString();

                string sql = @"UPDATE tb_usuario SET
                            fl_status=@status, fl_tipo=@fl_tipo
                            WHERE cod_usuario=@id";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id", idUsuarioUrl);
                    cmd.Parameters.AddWithValue("@status", ddlStatusAcesso.SelectedValue);
                    cmd.Parameters.AddWithValue("@fl_tipo", ddlNivelAcesso.SelectedValue);
                    cmd.ExecuteNonQuery();
                }
            }
        }
}