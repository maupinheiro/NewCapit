using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Newtonsoft.Json;
using System.Web.Services;

namespace NewCapit
{
    public partial class Frm_MontarCarga : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DateTime dataHoraAtual = DateTime.Now;
                lblDtCadastro.Text = dataHoraAtual.ToString("dd/MM/yyyy HH:mm");
                CarregarRemetente();
                CarregarExpedidor();
                CarregarDestinatario();
                CarregarRecebedor();
                CarregarConsignatario();
                CarregarPagador();
            }
               
            
        }

        // Função para carregar o DropDownList com dados do remetente
        private void CarregarRemetente()
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                conn.Open();
                string query = "SELECT id, codcli, razcli FROM tbclientes WHERE fl_exclusao is null AND ativo_inativo = 'ATIVO' ORDER BY razcli";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                ddlRemetente.DataSource = reader;
                ddlRemetente.DataTextField = "razcli";  // Campo a ser exibido
                ddlRemetente.DataValueField = "id";  // Valor associado ao item
                ddlRemetente.DataBind();

                // Adicionar o item padrão
                ddlRemetente.Items.Insert(0, new ListItem("", "0"));

            }
        }

        // Evento disparado quando o item do DropDownList é alterado
        protected void ddlRemetente_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idSelecionado = int.Parse(ddlRemetente.SelectedValue);

            // Preencher os campos com base no valor selecionado
            if (idSelecionado > 0)
            {
                PreencherCamposRemetente(idSelecionado);
            }
            else
            {
                LimparCamposRemetente();
            }
        }

        // Função para preencher os campos com os dados do banco
        private void PreencherCamposRemetente(int id)
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                conn.Open();
                string query = "SELECT codcli, razcli, cidcli, estcli FROM tbclientes WHERE id = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtCodRemetente.Text = reader["codcli"].ToString();                   
                }
            }
        }

        // Função para limpar os campos
        private void LimparCamposRemetente()
        {
            txtCodRemetente.Text = string.Empty;                     
        }

        private void PreencheRemetente(string codcli)
        {
            if (txtCodRemetente.Text.Trim() != "")
            {
                using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
                {
                    conn.Open();
                    string query = "SELECT codcli, razcli, cidcli, estcli FROM tbclientes WHERE codcli = @codcli";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@codcli", codcli);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        ddlRemetente.SelectedValue = reader["razcli"].ToString();
                    }
                    else
                    {
                        string nomeUsuario = "Genildo"; // txtUsuCadastro.Text;

                        string linha1 = "Olá, " + nomeUsuario + "!";
                        string linha2 = "Código digitado, não foi encontrado no sistema.";

                        // Concatenando as linhas com '\n' para criar a mensagem
                        string mensagem = $"{linha1}\n{linha2}";

                        string mensagemCodificada = HttpUtility.JavaScriptStringEncode(mensagem);
                        // Gerando o script JavaScript para exibir o alerta
                        string script = $"alert('{mensagemCodificada}');";

                        // Registrando o script para execução no lado do cliente
                        ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", script, true);
                        txtCodRemetente.Focus();
                    }

                }

            }
        }

        // Função para carregar o DropDownList com dados do expedidor
        private void CarregarExpedidor()
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                conn.Open();
                string query = "SELECT id, codcli, razcli FROM tbclientes WHERE fl_exclusao is null AND ativo_inativo = 'ATIVO' ORDER BY razcli";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                ddlExpedidor.DataSource = reader;
                ddlExpedidor.DataTextField = "razcli";  // Campo a ser exibido
                ddlExpedidor.DataValueField = "id";  // Valor associado ao item
                ddlExpedidor.DataBind();

                // Adicionar o item padrão
                ddlExpedidor.Items.Insert(0, new ListItem("", "0"));
            }
        }

        // Evento disparado quando o item do DropDownList é alterado
        protected void ddlExpedidor_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idSelecionado = int.Parse(ddlExpedidor.SelectedValue);

            // Preencher os campos com base no valor selecionado
            if (idSelecionado > 0)
            {
                PreencherCamposExpedidor(idSelecionado);
            }
            else
            {
                LimparCamposExpedidor();
            }
        }

        // Função para preencher os campos com os dados do banco
        private void PreencherCamposExpedidor(int id)
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                conn.Open();
                string query = "SELECT codcli, razcli, cidcli, estcli FROM tbclientes WHERE id = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtCodExpedidor.Text = reader["codcli"].ToString();                    
                }
            }
        }

        // Função para limpar os campos
        private void LimparCamposExpedidor()
        {
            txtCodExpedidor.Text = string.Empty;           
        }


        // Função para carregar o DropDownList com dados do destinatario
        private void CarregarDestinatario()
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                conn.Open();
                string query = "SELECT id, codcli, razcli FROM tbclientes WHERE fl_exclusao is null AND ativo_inativo = 'ATIVO' ORDER BY razcli";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                ddlDestinatario.DataSource = reader;
                ddlDestinatario.DataTextField = "razcli";  // Campo a ser exibido
                ddlDestinatario.DataValueField = "id";  // Valor associado ao item
                ddlDestinatario.DataBind();

                // Adicionar o item padrão
                ddlDestinatario.Items.Insert(0, new ListItem("", "0"));
            }
        }

        // Evento disparado quando o item do DropDownList é alterado
        protected void ddlDestinatario_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idSelecionado = int.Parse(ddlDestinatario.SelectedValue);

            // Preencher os campos com base no valor selecionado
            if (idSelecionado > 0)
            {
                PreencherCamposDestinatario(idSelecionado);
            }
            else
            {
                LimparCamposDestinatario();
            }
        }

        // Função para preencher os campos com os dados do banco
        private void PreencherCamposDestinatario(int id)
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                conn.Open();
                string query = "SELECT codcli, razcli, cidcli, estcli FROM tbclientes WHERE id = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtCodDestinatario.Text = reader["codcli"].ToString();                    
                }
            }
        }

        // Função para limpar os campos
        private void LimparCamposDestinatario()
        {
            txtCodDestinatario.Text = string.Empty;            
        }

        // Função para carregar o DropDownList com dados do destinatario
        private void CarregarRecebedor()
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                conn.Open();
                string query = "SELECT id, codcli, razcli FROM tbclientes WHERE fl_exclusao is null AND ativo_inativo = 'ATIVO' ORDER BY razcli";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                ddlRecebedor.DataSource = reader;
                ddlRecebedor.DataTextField = "razcli";  // Campo a ser exibido
                ddlRecebedor.DataValueField = "id";  // Valor associado ao item
                ddlRecebedor.DataBind();

                // Adicionar o item padrão
                ddlRecebedor.Items.Insert(0, new ListItem("", "0"));
            }
        }

        // Evento disparado quando o item do DropDownList é alterado
        protected void ddlRecebedor_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idSelecionado = int.Parse(ddlRecebedor.SelectedValue);

            // Preencher os campos com base no valor selecionado
            if (idSelecionado > 0)
            {
                PreencherCamposRecebedor(idSelecionado);
            }
            else
            {
                LimparCamposRecebedor();
            }
        }

        // Função para preencher os campos com os dados do banco
        private void PreencherCamposRecebedor(int id)
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                conn.Open();
                string query = "SELECT codcli, razcli, cidcli, estcli FROM tbclientes WHERE id = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtCodRecebedor.Text = reader["codcli"].ToString();
                }
            }
        }

        // Função para limpar os campos
        private void LimparCamposRecebedor()
        {
            txtCodRecebedor.Text = string.Empty;
        }

        // Função para carregar o DropDownList com dados do consignatario
        private void CarregarConsignatario()
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                conn.Open();
                string query = "SELECT id, codcli, razcli FROM tbclientes WHERE fl_exclusao is null AND ativo_inativo = 'ATIVO' ORDER BY razcli";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                ddlConsignatario.DataSource = reader;
                ddlConsignatario.DataTextField = "razcli";  // Campo a ser exibido
                ddlConsignatario.DataValueField = "id";  // Valor associado ao item
                ddlConsignatario.DataBind();

                // Adicionar o item padrão
                ddlConsignatario.Items.Insert(0, new ListItem("", "0"));
            }
        }

        // Evento disparado quando o item do DropDownList é alterado
        protected void ddlConsignatario_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idSelecionado = int.Parse(ddlConsignatario.SelectedValue);

            // Preencher os campos com base no valor selecionado
            if (idSelecionado > 0)
            {
                PreencherCamposConsignatario(idSelecionado);
            }
            else
            {
                LimparCamposConsignatario();
            }
        }

        // Função para preencher os campos com os dados do banco
        private void PreencherCamposConsignatario(int id)
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                conn.Open();
                string query = "SELECT codcli, razcli, cidcli, estcli FROM tbclientes WHERE id = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtCodConsignatario.Text = reader["codcli"].ToString();
                }
            }
        }

        // Função para limpar os campos
        private void LimparCamposConsignatario()
        {
            txtCodConsignatario.Text = string.Empty;
        }

        // Função para carregar o DropDownList com dados do consignatario
        private void CarregarPagador()
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                conn.Open();
                string query = "SELECT id, codcli, razcli FROM tbclientes WHERE fl_exclusao is null AND ativo_inativo = 'ATIVO' ORDER BY razcli";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                ddlPagador.DataSource = reader;
                ddlPagador.DataTextField = "razcli";  // Campo a ser exibido
                ddlPagador.DataValueField = "id";  // Valor associado ao item
                ddlPagador.DataBind();

                // Adicionar o item padrão
                ddlPagador.Items.Insert(0, new ListItem("", "0"));
            }
        }

        // Evento disparado quando o item do DropDownList é alterado
        protected void ddlPagador_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idSelecionado = int.Parse(ddlPagador.SelectedValue);

            // Preencher os campos com base no valor selecionado
            if (idSelecionado > 0)
            {
                PreencherCamposPagador(idSelecionado);
            }
            else
            {
                LimparCamposPagador();
            }
        }

        // Função para preencher os campos com os dados do banco
        private void PreencherCamposPagador(int id)
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                conn.Open();
                string query = "SELECT codcli, razcli, cidcli, estcli FROM tbclientes WHERE id = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtCodPagador.Text = reader["codcli"].ToString();
                }
            }
        }

        // Função para limpar os campos
        private void LimparCamposPagador()
        {
            txtCodPagador.Text = string.Empty;
        }
    }

}