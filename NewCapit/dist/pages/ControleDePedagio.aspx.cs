using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;



namespace NewCapit.dist.pages
{
    public partial class ControleDePedagio : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString());
        string nomeUsuario;
        string tipoMotorista;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                
            if (Session["UsuarioLogado"] != null)
            {
                string nomeUsuario = Session["UsuarioLogado"].ToString();
                var lblUsuario = nomeUsuario;
                txtCreditadoPor.Text = nomeUsuario;
            }
            else
            {
                var lblUsuario = "<Usuário>";
                Response.Redirect("Login.aspx");
            }
            CarregarGrid();

        }
        void CarregarGrid()
        {
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString());
            {
                string sql = @"SELECT nomemotorista, cpf, transportadora,
                       cpf_cnpj_proprietario, placa, tipoveiculo, eixos,
                       tomadorservico, expedidor, cid_expedidor, uf_expedidor,
                       recebedor, cid_recebedor, uf_recebedor,
                       idpedagio, valorpedagio, id
                       FROM tbcarregamentos
                       WHERE pedagio = 'SIM'
                          AND NOT (
                                tomadorservico LIKE 'VOLKS%'
                                AND (cva IS NULL OR LTRIM(RTRIM(cva)) = '')
                              )
                               AND pedagiofeito='NÃO'
                        ORDER BY nomemotorista
                        ";

                SqlCommand cmd = new SqlCommand(sql, con);   
                con.Open();
                rpCarregamentos.DataSource = cmd.ExecuteReader();
                rpCarregamentos.DataBind();
            }
        }
        protected void rpCarregamentos_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Editar")
            {
               // hdIdCarregamento.Value = e.CommandArgument.ToString();

                int id = Convert.ToInt32(e.CommandArgument);
                hdIdCarregamento.Value = id.ToString();

                CarregarDadosModal(id);


                ScriptManager.RegisterStartupScript(
                    this,
                    this.GetType(),
                    "abrirModal",
                    "var modal = new bootstrap.Modal(document.getElementById('modalPedagio')); modal.show();",
                    true
                );
            }
        }
        protected void btnSalvarPedagio_Click(object sender, EventArgs e)
        {
            lblErro.Text = "";

            // 🔒 Validação
            if (string.IsNullOrWhiteSpace(txtValorPedagio.Text))
            {
                lblErro.Text = "Informe o valor do pedágio.";
                ReabrirModal();
                return;
            }

            if (!decimal.TryParse(
                txtValorPedagio.Text.Replace(".", "").Replace(",", "."),
                System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture,
                out decimal valorPedagio))
            {
                lblErro.Text = "Valor do pedágio inválido.";
                ReabrirModal();
                return;
            }

            using (SqlConnection conn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                conn.Open();

                // 🔐 Segurança: não salvar se já estiver SIM
                SqlCommand check = new SqlCommand(@"
            SELECT pedagiofeito, idpedagio, valorpedagio, historicopedagio, creditopedagio, pagadorpedagioida, pagadorpedagiovolta, dtemissaopedagio 
            FROM tbcarregamentos 
            WHERE id = @id", conn);

                check.Parameters.AddWithValue("@id", hdIdCarregamento.Value);

                if (Convert.ToString(check.ExecuteScalar()) == "SIM")
                {
                    lblErro.Text = "Este pedágio já foi salvo.";
                    ReabrirModal();
                    return;
                }

                // 💾 Salvar pedágio
                SqlCommand cmd = new SqlCommand(@"
            UPDATE tbcarregamentos
            SET
                idpedagio = @idpedagio,
                valorpedagio = @valorpedagio,
                dtemissaopedagio = @dtemissaopedagio, 
                historicopedagio = @historicopedagio,
                creditopedagio = @creditopedagio,
                pagadorpedagioida=@pagadorpedagioida,
                pagadorpedagiovolta = @pagadorpedagiovolta,
                pedagiofeito = 'SIM'
            WHERE id = @id", conn);

                cmd.Parameters.AddWithValue("@id", hdIdCarregamento.Value);
                cmd.Parameters.AddWithValue("@idpedagio", txtDocumentoPedagio.Text.Trim());
                cmd.Parameters.AddWithValue("@valorpedagio", valorPedagio);
                cmd.Parameters.AddWithValue("@dtemissaopedagio", DateTime.Now);
                cmd.Parameters.AddWithValue("@historicopedagio", txtObservacaoPedagio.Text.Trim());
                cmd.Parameters.AddWithValue("@creditopedagio", txtCreditadoPor.Text.Trim());
                cmd.Parameters.AddWithValue("@pagadorpedagioida", txtTomadorServicoPedagio.Text.Trim());
                cmd.Parameters.AddWithValue("@pagadorpedagiovolta", txtPagadorPedagioVolta.Text.Trim());
                cmd.Parameters.AddWithValue("@pedagiofeito", "SIM");

                cmd.ExecuteNonQuery();
            }

            // 🔄 Atualiza grid
            CarregarGrid();

            // ❌ Fecha modal
            FecharModal();
        }

        void ReabrirModal()
        {
            ScriptManager.RegisterStartupScript(
                this, GetType(), "abrirModal",
                "var modal = new bootstrap.Modal(document.getElementById('modalPedagio')); modal.show();",
                true);
        }

        void FecharModal()
        {
            ScriptManager.RegisterStartupScript(
                this, GetType(), "fecharModal",
                "var modalEl = document.getElementById('modalPedagio');" +
                "var modal = bootstrap.Modal.getInstance(modalEl);" +
                "if(modal){ modal.hide(); }",
                true);
        }

        void CarregarDadosModal(int idCarregamento)
        {
            using (SqlConnection conn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                string sql = @"
            SELECT 
                num_carregamento,
                expedidor,
                cid_expedidor,
                uf_expedidor,
                recebedor,
                cid_recebedor,
                uf_recebedor,
                nomemotorista,
                cpf,
                transportadora,
                cpf_cnpj_proprietario,
                placa,
                tipoveiculo,
                eixos,
                tomadorservico, pedagio, pedagiofeito, valorpedagio, idpedagio,historicopedagio,creditopedagio,dtemissaopedagio, pagadorpedagioida, pagadorpedagiovolta, tipomot,cva
            FROM tbcarregamentos
            WHERE id = @id";

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", idCarregamento);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    //txtComprovante.Text = dr["comprovante"]?.ToString();
                    txtNum_Carregamento.Text = dr["num_carregamento"]?.ToString();
                    txtExpedidorPedagio.Text = dr["expedidor"]?.ToString();
                    txtCid_ExpedidorPedagio.Text = dr["cid_expedidor"]?.ToString();
                    txtUf_ExpedidorPedagio.Text = dr["uf_expedidor"]?.ToString();
                    txtRecebedorPedagio.Text = dr["recebedor"]?.ToString();
                    txtCid_RecebedorPedagio.Text = dr["cid_recebedor"]?.ToString();
                    txtUf_RecebedorPedagio.Text = dr["uf_recebedor"]?.ToString();
                    txtMotoristaPedagio.Text = dr["cpf"]?.ToString() + " - " + dr["nomemotorista"]?.ToString();
                    txtProprietarioPedagio.Text = dr["cpf_cnpj_proprietario"]?.ToString() + " - " + dr["transportadora"]?.ToString();
                    txtVeiculoPedagio.Text = dr["placa"]?.ToString() + " - " + dr["tipoveiculo"]?.ToString() + " - " + dr["eixos"]?.ToString() + " eixos";
                    txtTomadorServicoPedagio.Text = dr["tomadorservico"]?.ToString() + " - SEM PARAR";
                    tipoMotorista = dr["uf_expedidor"]?.ToString();
                    if (tipoMotorista == "Terceiro")
                    {
                        txtPagadorPedagioVolta.Text = "Pedágio só de IDA";
                    }
                    else
                    {
                        txtPagadorPedagioVolta.Text = "TRANSNOVAG - SEM PARAR";
                    }
                    if (dr["tomadorservico"]?.ToString() == "VOLKSWAGEN")
                    {
                        txtDocumentoPedagio.Text = "CVA" + dr["cva"]?.ToString();
                    }
                    else
                    {
                        txtDocumentoPedagio.Text = "OC" + dr["num_carregamento"]?.ToString();
                    }
                    txtValorPedagio.Text = dr["valorpedagio"] != DBNull.Value
                        ? Convert.ToDecimal(dr["valorpedagio"]).ToString("N2")
                        : "";

                    txtEmissaoPedagio.Text = dr["dtemissaopedagio"] != DBNull.Value
                        ? Convert.ToDateTime(dr["dtemissaopedagio"]).ToString("dd/MM/yyyy HH:mm")
                        : DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                }
            }
        }




    }

}