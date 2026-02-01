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
using System.Text.RegularExpressions;
using Org.BouncyCastle.Asn1.Cmp;
using DocumentFormat.OpenXml.Office.Word;

namespace NewCapit.dist.pages
{
    public partial class Frm_OrdemAbastecimento : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString());
        string id;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PreencherComboFiliais();
                CarregaDadosFornecedor();
                // Carrega apenas na primeira vez
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
            }
            else
            {
                // Se você precisa recarregar o ddlCombustivel em postbacks por algum motivo,
                // certifique-se de que a seleção do usuário seja restaurada APÓS o DataBind.
                // No entanto, a melhor prática é carregar apenas uma vez se os dados não mudam.
                // Se os dados mudam, você pode precisar de uma lógica mais complexa para preservar a seleção.
                // Por exemplo, você pode salvar o valor selecionado em ViewState antes do DataBind
                // e restaurá-lo depois.

                // Exemplo (se CarregaCombustivel() precisar ser chamado em postbacks):
                // string selectedValue = ddlCombustivel.SelectedValue;
                // CarregaCombustivel();
                 
            }



        }
        
        protected void ddlCombustivel_SelectedIndexChanged(object sender, EventArgs e)
        {
            string idSelecionado = ddlCombustivel.SelectedItem.Text;

            if (string.IsNullOrEmpty(idSelecionado))
            {
                txtPreco.Text = "";
                return;
            }

           // string connectionString = "sua_connection_string_aqui";
            string query = "SELECT valor FROM tbprecocombustivel WHERE combustivel = @combustivel";

            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@combustivel", idSelecionado);
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    // Exemplo: mostrar cor e sabor juntos
                    string valorCombustivel = reader["valor"].ToString();
                    //string sabor = reader["Sabor"].ToString();

                    txtPreco.Text = $"{valorCombustivel}";
                }

                reader.Close();
            }
        }

        private void PreencherComboFiliais()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT codigo, descricao FROM tbempresa";

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
                    cbFiliais.DataSource = reader;
                    cbFiliais.DataTextField = "descricao";  // Campo que será mostrado no ComboBox
                    cbFiliais.DataValueField = "codigo";  // Campo que será o valor de cada item                    
                    cbFiliais.DataBind();  // Realiza o binding dos dados                   
                    cbFiliais.Items.Insert(0, new ListItem("-- Origem do documento --", "0"));
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
        public void CarregaDadosFornecedor()
        {
            if (HttpContext.Current.Request.QueryString["id"].ToString() != "")
            {
                id = HttpContext.Current.Request.QueryString["id"].ToString();
            }
            string sql = "SELECT codfor, fantasia FROM tbfornecedores WHERE id = " + id;

            SqlDataAdapter adpt = new SqlDataAdapter(sql, con);
            DataTable dt = new DataTable();
            con.Open();
            adpt.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                // Preenchendo os TextBoxes com valores do DataTable
                if (dt.Rows[0][0].ToString() != string.Empty)
                {
                    txtCodFor.Text = dt.Rows[0][0].ToString(); 
                    txtNomFor.Text = dt.Rows[0][1].ToString();

                    string textoCompleto = dt.Rows[0][1].ToString();

                    // Verifica se o texto tem pelo menos 5 caracteres
                    string primeiras10Letras = textoCompleto.Length >= 10
                        ? textoCompleto.Substring(0, 10)
                        : textoCompleto; // Se tiver menos de 10, pega tudo

                    if (primeiras10Letras == "TRANSNOVAG")
                    {
                        txtExterno.BackColor = System.Drawing.Color.Purple;
                        txtExterno.ForeColor = System.Drawing.Color.White;
                        txtExterno.Text = "INTERNO";

                    }
                    else
                    {
                        txtExterno.BackColor = System.Drawing.Color.Purple;
                        txtExterno.ForeColor = System.Drawing.Color.White;
                        txtExterno.Text = "EXTERNO";
                    }

                    CarregaCombustivel();

                }

                //CarregaCombustivel();
            }
        }
        public void CarregaCombustivel()
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                string query = "SELECT id, combustivel FROM tbprecocombustivel WHERE codposto = @codposto";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    int codPosto;
                    if (!int.TryParse(txtCodFor.Text.Trim(), out codPosto))
                    {
                        return;
                    }

                    cmd.Parameters.AddWithValue("@codposto", codPosto);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    // Salva o valor selecionado antes do DataBind, se houver um postback
                    string selectedValue = string.Empty;
                    if (IsPostBack && ddlCombustivel.SelectedItem != null)
                    {
                        selectedValue = ddlCombustivel.SelectedValue;
                    }

                    ddlCombustivel.DataSource = reader;
                    ddlCombustivel.DataTextField = "combustivel";
                    ddlCombustivel.DataValueField = "id";
                    ddlCombustivel.DataBind();

                    // Insere o item padrão. É importante que ele tenha um DataValueField único, como uma string vazia.
                    ddlCombustivel.Items.Insert(0, new ListItem("-- Produto --", ""));

                    // Tenta restaurar a seleção após o DataBind e a inserção do item padrão
                    if (IsPostBack && !string.IsNullOrEmpty(selectedValue))
                    {
                        try
                        {
                            ddlCombustivel.SelectedValue = selectedValue;
                        }
                        catch (Exception ex)
                        {
                            // Log ou trate o erro se o valor selecionado não for encontrado na nova lista
                            // Isso pode acontecer se a lista de combustíveis mudar dinamicamente
                            System.Diagnostics.Debug.WriteLine("Erro ao restaurar seleção do ddlCombustivel: " + ex.Message);
                        }
                    }

                    reader.Close();
                    conn.Close();
                }
            }
        }
        protected void customRadioFrota_CheckedChanged(object sender, EventArgs e)
        {
            txtCodMot.ReadOnly = customRadioFrota.Checked;
            txtCodMot.ReadOnly = false;
            txtCodMot.Text = "";
            txtNomMot.Text = "";
            txtCPF.Text = "";          

            if (customRadioFrota.Checked)
            {
                customRadioCTe.Enabled = false;
                customRadioNFSe.Enabled = false;
                cbFiliais.Enabled = false;
                txtLitros.Enabled = false;

            }
        }
        protected void customRadioAgregado_CheckedChanged(object sender, EventArgs e)
        {     
            if (customRadioFrota.Checked)
            {
                customRadioCTe.Enabled = true;
                customRadioNFSe.Enabled = true;

            }
        }
        protected void customRadioCTe_CheckedChanged(object sender, EventArgs e)
        {
            txtDocumento.ReadOnly = customRadioCTe.Checked;
           

            if (customRadioCTe.Checked)
            {
                txtDocumento.ReadOnly = false;
            }
        }
        protected void customRadioNFSe_CheckedChanged(object sender, EventArgs e)
        {
            txtDocumento.ReadOnly = customRadioNFSe.Checked;

            if (customRadioNFSe.Checked)
            {
                txtDocumento.ReadOnly = false;
            }
        }
        protected void txtCodMot_TextChanged(object sender, EventArgs e)
        {            
            txtNomMot.Text = "";
            txtCPF.Text = "";

            if (string.IsNullOrEmpty(txtCodMot.Text))
                return;

            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                string sql = @"SELECT codmot, nommot, cpf, status, tipomot
                       FROM tbmotoristas
                       WHERE codmot = @codmot";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@codmot", txtCodMot.Text.Trim());

                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        if (dr["status"].ToString().ToUpper() == "ATIVO")
                        {
                            if (dr["tipomot"].ToString().ToUpper() == "FUNCIONÁRIO")
                            {
                                txtCodMot.Text = dr["codmot"].ToString();
                                txtNomMot.Text = dr["nommot"].ToString();
                                txtCPF.Text = dr["cpf"].ToString();
                                txtCodVei.ReadOnly = false;
                                txtCodVei.Focus();
                                return;
                            }
                            else
                            {
                                MostrarMsg(dr["codmot"].ToString() + " - " + dr["nommot"].ToString() + ", não é funcionário.", "info");                                
                                txtCodMot.Text = "";
                                txtCodMot.Focus();
                                return;
                            }

                        }
                        else
                        {
                            MostrarMsg(dr["codmot"].ToString() + " - " + dr["nommot"].ToString() + ", motorista encontrado, porém está INATIVO.", "danger");                           
                            txtCodMot.Text = "";
                            txtCodMot.Focus();
                            return;
                        }
                    }
                    else
                    {
                        MostrarMsg(txtCodMot.Text.Trim() + ", motorista não encontrado.", "warning"); 
                        txtCodMot.Text = "";
                        txtCodMot.Focus();
                        return;
                    }
                }
            }
        }
        protected void txtCodVei_TextChanged(object sender, EventArgs e)
        {           
            txtPlaca.Text = "";
            txtModelo.Text = "";
            txtCodProp.Text = "";
            txtTransp.Text = "";
            txtCNPJ.Text = "";
            if (string.IsNullOrEmpty(txtCodVei.Text))
                return;

            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                string sql = @"SELECT codvei, plavei, tipvei, modelo, ativo_inativo, tipoveiculo
                       FROM tbveiculos
                       WHERE codvei = @codvei";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@codvei", txtCodVei.Text.Trim());

                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        if (dr["ativo_inativo"].ToString().ToUpper() == "ATIVO")
                        {
                            if (dr["tipoveiculo"].ToString().ToUpper() == "FROTA")
                            {
                                txtCodVei.Text = dr["codvei"].ToString();
                                txtPlaca.Text = dr["plavei"].ToString();
                                txtModelo.Text = dr["tipvei"].ToString() + " - " + dr["modelo"].ToString();
                                txtCodProp.Text = "1111";
                                txtTransp.Text = "TRANSNOVAG TRANSPORTES S/A";
                                txtCNPJ.Text = "55.890.016/0001-09";
                                return;
                            }
                            else
                            {
                                MostrarMsg(dr["codvei"].ToString() + " - " + dr["plavei"].ToString() + ", não corresponde a veículo da frota.", "info");                               
                                txtCodVei.Text = "";
                                txtCodVei.Focus();
                                return;
                            }

                        }
                        else
                        {
                            MostrarMsg(dr["codvei"].ToString() + " - " + dr["plavei"].ToString() + ", frota encontrada, porém está INATIVA.", "danger");                           
                            txtCodVei.Text = "";
                            txtCodVei.Focus();
                            return;
                        }
                    }
                    else
                    {
                        MostrarMsg(txtCodVei.Text.Trim() + ", frota não encontrada.", "warning"); 
                        txtCodVei.Text = "";
                        txtCodVei.Focus();
                        return;
                        
                    }
                }
            }
        }
        protected void txtDocumento_TextChanged(object sender, EventArgs e)
        {
            string empresa = "TRANSNOVAG - " + cbFiliais.SelectedItem.Text.Trim(); 
            string documento = txtDocumento.Text.Trim();

            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                conn.Open();
                string sql = @"
                SELECT
                    car.tipomot,
                    car.codmotorista,
                    car.nomemotorista,
                    car.cpf,
                    car.veiculo,
                    car.placa,
                    car.codtra,
                    car.transportadora,
                    car.cpf_cnpj_proprietario
                FROM tbcte cte
                INNER JOIN tbcargas cg
                    ON cg.idviagem = cte.id_viagem
                INNER JOIN tbcarregamentos car
                    ON car.num_carregamento = cg.idviagem
                WHERE
                    cte.empresa_emissora = @empresa
                AND cte.num_documento = @documento";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@empresa", SqlDbType.VarChar).Value = empresa;   // MATRIZ
                    cmd.Parameters.Add("@documento", SqlDbType.VarChar).Value = documento;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (!dr.HasRows)
                        {
                            MostrarMsg(txtDocumento.Text.Trim() + " - Documento não encontrado ou sem carregamento vinculado", "warning");
                            return;
                        }

                        while (dr.Read())
                        {
                            string tipoMot = dr["tipomot"].ToString();

                            // 🚫 FUNCIONÁRIO
                            if (tipoMot == "FUNCIONÁRIO")
                            {
                                MostrarMsg(txtDocumento.Text.Trim() + " - Documento inválido para emissão da autorização", "danger");
                                LimparCamposMotorista();
                                return;
                            }

                            // ✅ AGREGADO ou TERCEIRO
                            txtCodMot.Text = dr["codmotorista"].ToString();
                            txtNomMot.Text = dr["nommotorista"].ToString();
                            txtCPF.Text = dr["cpf"].ToString();
                            txtCodVei.Text = dr["veiculo"].ToString();
                            txtPlaca.Text = dr["placa"].ToString();
                            txtCodProp.Text = dr["codtra"].ToString();
                            txtTransp.Text = dr["transportadora"].ToString();
                            txtCNPJ.Text = dr["cpf_cnpj_proprietario"].ToString();

                            break; // usa o primeiro válido
                        }
                    }
                }

            }
        }
        private void LimparCamposMotorista()
        {
            txtCodMot.Text = "";
            txtNomMot.Text = "";
            txtCPF.Text = "";
            txtCodVei.Text = "";
            txtPlaca.Text = "";
            txtCodProp.Text = "";
            txtTransp.Text = "";
            txtCNPJ.Text = "";
        }

        protected void MostrarMsg(string mensagem, string tipo = "warning")
        {
            // limpa antes de mostrar
            LimparMsg();

            divMsg.Attributes["class"] = $"alert alert-{tipo} alert-dismissible fade show mt-3";
            lblMsgGeral.InnerText = mensagem;
            divMsg.Style["display"] = "block";

            string script = @"
        setTimeout(function () {
            var div = document.getElementById('divMsg');
            if (div) div.style.display = 'none';
        }, 4000);";

            ScriptManager.RegisterStartupScript(
                this,
                GetType(),
                Guid.NewGuid().ToString(),
                script,
                true
            );
        }
        //protected void MostrarToast(string mensagem, string tipo = "warning")
        //{
        //    string script = $@"
        //var toastEl = document.getElementById('toastMsg');
        //toastEl.className = 'toast align-items-center text-bg-{tipo} border-0';
        //document.getElementById('toastBody').innerText = '{mensagem}';
        //var toast = new bootstrap.Toast(toastEl);
        //toast.show();";

        //    ScriptManager.RegisterStartupScript(
        //        this,
        //        GetType(),
        //        Guid.NewGuid().ToString(),
        //        script,
        //        true
        //    );
        //}
        protected void LimparMsg()
        {
            lblMsgGeral.InnerText = "";
            divMsg.Style["display"] = "none";
        }


    }
}