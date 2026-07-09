using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NewCapit.dist.pages
{
    public partial class ConsultaFretes : PaginaBase
    {
        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString());
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
                CarregarGridFretes();
            }
            
        }

        protected void gvListFretes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvListFretes.PageIndex = e.NewPageIndex;
            CarregarGridFretes();  // Método para recarregar os dados no GridView
        }
        protected void Editar(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;

            string id = gvListFretes.DataKeys[row.RowIndex].Value.ToString();

            Response.Redirect("Frm_Alt_TabelaPrecoMatriz.aspx?id=" + id);
        }       

        protected void CarregarGridFretes()
        {
            int pageSize = 35;
            int paginaAtual = Session["Pagina"] != null ? (int)Session["Pagina"] : 1;

            string connStr = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string where = " WHERE fl_exclusao IS NULL ";

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;

                // FILTRO: cod_frete
                if (!string.IsNullOrEmpty(txtCodigo.Text))
                {
                    where += " AND cod_frete = @Codigo";
                    cmd.Parameters.AddWithValue("@Codigo", txtCodigo.Text.Trim());
                }

                // FILTRO: pagador
                if (!string.IsNullOrEmpty(txtPagador.Text))
                {
                    where += " AND (cod_pagador = @codPagador OR pagador LIKE @nomePagador)";
                    cmd.Parameters.AddWithValue("@codPagador", txtPagador.Text.Trim());
                    cmd.Parameters.AddWithValue("@nomePagador", "%" + txtPagador.Text.Trim().ToUpper() + "%");
                }

                // FILTRO: expedidor
                if (!string.IsNullOrEmpty(txtExpedidor.Text))
                {
                    where += " AND (cod_expedidor = @codExpedidor OR expedidor LIKE @nomeExpedidor)";
                    cmd.Parameters.AddWithValue("@codExpedidor", txtExpedidor.Text.Trim());
                    cmd.Parameters.AddWithValue("@nomeExpedidor", "%" + txtExpedidor.Text.Trim().ToUpper() + "%");
                }

                // FILTRO: recebedor
                if (!string.IsNullOrEmpty(txtRecebedor.Text))
                {
                    where += " AND (cod_recebedor = @codRecebedor OR recebedor LIKE @nomeRecebedor)";
                    cmd.Parameters.AddWithValue("@codRecebedor", txtRecebedor.Text.Trim());
                    cmd.Parameters.AddWithValue("@nomeRecebedor", "%" + txtRecebedor.Text.Trim().ToUpper() + "%");
                }

                // =========================
                // 1. TOTAL REGISTROS
                // =========================
                string sqlCount = "SELECT COUNT(*) FROM tbtabeladefretes " + where;

                SqlCommand cmdTotal = new SqlCommand(sqlCount, conn);
                foreach (SqlParameter p in cmd.Parameters)
                    cmdTotal.Parameters.AddWithValue(p.ParameterName, p.Value);

                conn.Open();

                int totalRegistros = (int)cmdTotal.ExecuteScalar();
                int totalPaginas = (int)Math.Ceiling((double)totalRegistros / pageSize);

                lblTotalGeral.InnerText = $"Página {paginaAtual} de {totalPaginas} | Total: {totalRegistros}";
                lblPaginaAtual.Text = paginaAtual.ToString();
                lblTotalPaginas.Text = totalPaginas.ToString();
                Session["TotalPaginas"] = totalPaginas;

                // =========================
                // 2. PAGINAÇÃO GRID
                // =========================
                string sql = @"
                WITH Dados AS (
                    SELECT 
                        cod_frete, cod_pagador, pagador,
                        cod_expedidor, expedidor, cid_expedidor, uf_expedidor,
                        cod_recebedor, recebedor, cid_recebedor, uf_recebedor,
                        situacao,
                        ROW_NUMBER() OVER(ORDER BY pagador ASC) AS RowNum
                    FROM tbtabeladefretes
                " + where + @"
                )
                SELECT *
                FROM Dados
                WHERE RowNum BETWEEN ((@pagina - 1) * @pageSize + 1)
                      AND (@pagina * @pageSize)";

                SqlCommand cmdFretes = new SqlCommand(sql, conn);

                foreach (SqlParameter p in cmd.Parameters)
                    cmdFretes.Parameters.AddWithValue(p.ParameterName, p.Value);

                cmdFretes.Parameters.AddWithValue("@pagina", paginaAtual);
                cmdFretes.Parameters.AddWithValue("@pageSize", pageSize);

                SqlDataAdapter da = new SqlDataAdapter(cmdFretes);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvListFretes.DataSource = dt;
                gvListFretes.DataBind();
            }

        }        
        protected void txtCodigo_TextChanged(object sender, EventArgs e)
        {
            CarregarGridFretes();
        }
        protected void txtPagador_TextChanged(object sender, EventArgs e)
        {
            CarregarGridFretes();
        }
        protected void txtExpedidor_TextChanged(object sender, EventArgs e)
        {
            CarregarGridFretes();
        }
        protected void txtRecebedor_TextChanged(object sender, EventArgs e)
        {
            CarregarGridFretes();
        }
        protected void btnPrimeiro_Click(object sender, EventArgs e)
        {
            Session["Pagina"] = 1;
            CarregarGridFretes();
        }
        protected void btnAnterior_Click(object sender, EventArgs e)
        {
            int pagina = (int)Session["Pagina"];
            if (pagina > 1)
                Session["Pagina"] = pagina - 1;

            CarregarGridFretes();
        }
        protected void btnProximo_Click(object sender, EventArgs e)
        {
            int pagina = (int)Session["Pagina"];
            int total = (int)Session["TotalPaginas"];

            if (pagina < total)
                Session["Pagina"] = pagina + 1;

            CarregarGridFretes();
        }
        protected void btnUltimo_Click(object sender, EventArgs e)
        {
            Session["Pagina"] = (int)Session["TotalPaginas"];
            CarregarGridFretes();
        }
        protected void btnIrPagina_Click(object sender, EventArgs e)
        {
            int paginaDigitada;

            if (int.TryParse(txtIrPagina.Text, out paginaDigitada))
            {
                int totalPaginas = Session["TotalPaginas"] != null ? (int)Session["TotalPaginas"] : 1;

                // 🔒 limita entre 1 e totalPaginas
                if (paginaDigitada < 1)
                    paginaDigitada = 1;

                if (paginaDigitada > totalPaginas)
                    paginaDigitada = totalPaginas;

                Session["Pagina"] = paginaDigitada;
                CarregarGridFretes();
            }
        }
        protected void lnkNovoCadastro_Click(object sender, EventArgs e)
        {
            Response.Redirect("Frm_TabelaPrecoMatriz.aspx");
        }
        protected void gvListFretes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            VerificarBotoesGrid(e, idBtnEditar: "lnkEditar", idBtnExcluir: "lnkRemover");
        }
    }
}