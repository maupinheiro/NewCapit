using DAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.UI.WebControls;

namespace NewCapit
{
    public class PaginaBase : System.Web.UI.Page
    {
        // Propriedade protegida que armazena as ações permitidas na página atual
        protected List<int> AcoesPermitidasTela
        {
            get { return (List<int>)(ViewState["AcoesPermitidasTela"] ?? new List<int>()); }
            set { ViewState["AcoesPermitidasTela"] = value; }
        }

        // O OnInit roda antes do Page_Load de qualquer tela filha
        protected override void OnInit(EventArgs e)
        {
            if (Session["CodUsuario"] == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            int idUsuarioLogado = Convert.ToInt32(Session["CodUsuario"]);
            string telaAtual = Path.GetFileName(Request.Url.AbsolutePath);

            // Busca as permissões uma única vez no banco de dados para esta página
            this.AcoesPermitidasTela = UsersDAL.ObterPermissoesUsuario(idUsuarioLogado, telaAtual);

            // Se o usuário não tiver a ação 4 (Visualizar), barra o acesso imediatamente
            if (!this.AcoesPermitidasTela.Contains(4))
            {
                Response.Redirect("~/dist/pages/Home.aspx?erro=sem_permissao");
                return;
            }

            base.OnInit(e);
        }

        // CASO 1: Botões soltos diretamente na página (Ex: Novo Registro, Exportar Relatório)
        protected void VerificarBotoesPagina(WebControl btnInserir = null, WebControl btnAlterar = null, WebControl btnExcluir = null)
        {
            if (btnInserir != null) btnInserir.Visible = this.AcoesPermitidasTela.Contains(1); // 1 = Inserir
            if (btnAlterar != null) btnAlterar.Visible = this.AcoesPermitidasTela.Contains(2); // 2 = Alterar
            if (btnExcluir != null) btnExcluir.Visible = this.AcoesPermitidasTela.Contains(3); // 3 = Excluir
        }

        // CASO 2: Botões dentro de linhas do GridView (Para chamar no OnRowDataBound)
        protected void VerificarBotoesGrid(GridViewRowEventArgs e, string idBtnEditar, string idBtnExcluir)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton btnEditar = (LinkButton)e.Row.FindControl(idBtnEditar);
                LinkButton btnExcluir = (LinkButton)e.Row.FindControl(idBtnExcluir);

                if (btnEditar != null) btnEditar.Visible = this.AcoesPermitidasTela.Contains(2); // 2 = Alterar
                if (btnExcluir != null) btnExcluir.Visible = this.AcoesPermitidasTela.Contains(3); // 3 = Excluir
            }
        }

        // CASO 3: Botões dentro de linhas do Repeater (Para chamar no OnItemDataBound)
        protected void VerificarBotoesRepeater(RepeaterItemEventArgs e, string idBtnEditar, string idBtnExcluir)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LinkButton btnEditar = (LinkButton)e.Item.FindControl(idBtnEditar);
                LinkButton btnExcluir = (LinkButton)e.Item.FindControl(idBtnExcluir);

                if (btnEditar != null) btnEditar.Visible = this.AcoesPermitidasTela.Contains(2); // 2 = Alterar
                if (btnExcluir != null) btnExcluir.Visible = this.AcoesPermitidasTela.Contains(3); // 3 = Excluir
            }
        }
        // CASO 4: Botões automáticos do CommandField (Editar/Excluir nativos do GridView)
        // CASO 4: Botões automáticos do CommandField (Editar/Excluir nativos do GridView)
        protected void VerificarCommandFieldGrid(GridViewRowEventArgs e, int indiceColuna)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Evita erro de índice fora dos limites das colunas do GridView
                if (e.Row.Cells.Count > indiceColuna)
                {
                    // Varre os controles gerados automaticamente dentro da célula informada
                    foreach (System.Web.UI.Control ctrl in e.Row.Cells[indiceColuna].Controls)
                    {
                        // Identifica se o controle é um botão (Button, LinkButton ou ImageButton)
                        if (ctrl is IButtonControl)
                        {
                            IButtonControl btnNativo = (IButtonControl)ctrl;
                            WebControl ctrlWeb = (WebControl)ctrl;

                            // 'Edit' é o comando interno do ASP.NET para o botão "Editar"
                            if (btnNativo.CommandName == "Edit")
                            {
                                // Se o usuário NÃO tiver permissão de Alterar (Ação 2), esconde o botão "Editar"
                                if (!this.AcoesPermitidasTela.Contains(2))
                                {
                                    ctrlWeb.Visible = false;
                                }
                            }

                            // 'Delete' é o comando interno do ASP.NET para o botão "Excluir" (se você ativar no futuro)
                            if (btnNativo.CommandName == "Delete")
                            {
                                // Se o usuário NÃO tiver permissão de Excluir (Ação 3), esconde o botão "Excluir"
                                if (!this.AcoesPermitidasTela.Contains(3))
                                {
                                    ctrlWeb.Visible = false;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}