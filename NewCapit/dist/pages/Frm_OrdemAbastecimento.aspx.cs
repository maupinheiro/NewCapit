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
using NPOI.SS.Formula.Functions;
using static Microsoft.AspNetCore.Razor.Language.TagHelperMetadata;
using System.Numerics;
using System.Globalization;

namespace NewCapit.dist.pages
{
    public partial class Frm_OrdemAbastecimento : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString());
        string id;
        string abastecimentoFrotaAgregado;
        string tipoDocumento;
        string filial;
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
                CarregaDadosFornecedor();
                CarregaCombustivel();
            }
        }       
        protected void ddlCombustivel_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sCodFor = txtCodFor.Text.Trim();
            string idSelecionado = ddlCombustivel.SelectedItem.Text;

            if (string.IsNullOrEmpty(idSelecionado))
            {
                txtPreco.Text = "";
                return;
            }

            // string connectionString = "sua_connection_string_aqui";
            string query = "SELECT valor FROM tbprecocombustivel WHERE combustivel = @combustivel AND codposto=@codfor AND status = 'ATIVO'";

            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@combustivel", idSelecionado);
                cmd.Parameters.AddWithValue("@codfor", sCodFor);                
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
        public void CarregaDadosFornecedor()
        {
            string id = HttpContext.Current.Request.QueryString["codfor"];
            if (HttpContext.Current.Request.QueryString["codfor"].ToString() != "")
            {
                id = HttpContext.Current.Request.QueryString["codfor"].ToString();
            }            
            string sql = "SELECT codfor, fantasia FROM tbfornecedores WHERE codfor = " + id;

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

                    //CarregaCombustivel();

                }

                //CarregaCombustivel();
            }
        }
        public void CarregaCombustivel()
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                string query = "SELECT id, combustivel FROM tbprecocombustivel WHERE codposto = @codposto AND status = 'ATIVO'";

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
                txtLitros.Enabled = false;

            }           
        }
        protected void customRadioAgregado_CheckedChanged(object sender, EventArgs e)
        {     
            //if (customRadioAgregado.Checked)
            //{
                customRadioCTe.Enabled = true;
                customRadioNFSe.Enabled = true;

            //}           
        }
        protected void customRadioCTe_CheckedChanged(object sender, EventArgs e)
        {
            txtDocumento.ReadOnly = customRadioCTe.Checked;
           

            if (customRadioCTe.Checked)
            {
                txtDocumento.ReadOnly = false;
                divFilial.Visible = true;
            }
        }
        protected void customRadioNFSe_CheckedChanged(object sender, EventArgs e)
        {
            txtDocumento.ReadOnly = customRadioNFSe.Checked;

            if (customRadioNFSe.Checked)
            {
                txtDocumento.ReadOnly = false;
                divFilial.Visible = false;
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
                                Mensagem("info", dr["codmot"].ToString() + " - " + dr["nommot"].ToString() + ", não é funcionário.");
                                txtCodMot.Text = "";
                                txtCodMot.Focus();
                                return;
                            }

                        }
                        else
                        {
                            Mensagem("danger", dr["codmot"].ToString() + " - " + dr["nommot"].ToString() + ", motorista está INATIVO.");  
                            txtCodMot.Text = "";
                            txtCodMot.Focus();
                            return;
                        }
                    }
                    else
                    {
                        Mensagem("warning", txtCodMot.Text.Trim() + ", motorista não encontrado."); 
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
            txtFilialVeic.Text = "";
            txtCNPJ.Text = "";
            if (string.IsNullOrEmpty(txtCodVei.Text))
                return;

            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                string sql = @"SELECT *
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
                                //txtModelo.Text = dr["tipvei"].ToString() + " - " + dr["modelo"].ToString();
                                txtModelo.Text = dr["tipvei"].ToString() + " - " + dr["modelo"].ToString() + " - " + dr["ano"].ToString() + " - " + dr["tipoveiculo"].ToString();
                                txtFilialVeic.Text = dr["nucleo"].ToString();
                                txtCodProp.Text = "1111";
                                txtTransp.Text = "TRANSNOVAG TRANSPORTES S/A";
                                txtCNPJ.Text = "55.890.016/0001-09";
                                return;
                            }
                            else
                            {
                                Mensagem("info", dr["codvei"].ToString() + " - " + dr["plavei"].ToString() + ", não corresponde a veículo da frota.");                               
                                txtCodVei.Text = "";
                                txtCodVei.Focus();
                                return;
                            }

                        }
                        else
                        {
                            Mensagem("danger", dr["codvei"].ToString() + " - " + dr["plavei"].ToString() + ", frota encontrada, porém está INATIVA.");                           
                            txtCodVei.Text = "";
                            txtCodVei.Focus();
                            return;
                        }
                    }
                    else
                    {
                        Mensagem("warning", txtCodVei.Text.Trim() + ", frota não encontrada."); 
                        txtCodVei.Text = "";
                        txtCodVei.Focus();
                        return;
                        
                    }
                }
            }
        }
        protected void txtDocumento_TextChanged(object sender, EventArgs e)
        {            
            string documento = txtDocumento.Text.Trim();

            if (customRadioCTe.Checked)
            {
                using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
                {
                    conn.Open();
                    string query = @"SELECT 
                        cte.num_documento, cte.emissao_documento, cte.ordem_abastecimento, cte.id_viagem, cte.empresa_emissora,
                        cg.carga,
                        mot.codmot, mot.nommot, mot.cpf,
                        vei.codvei, vei.plavei, vei.codtra, vei.tipvei, vei.tipoveiculo, vei.modelo, vei.ano, vei.nucleo, 
                        pr.codtra, pr.fantra, pr.cnpj, pr.limitecreditoabastecimento
                    FROM tbcte cte
                    LEFT JOIN tbcargas cg ON cg.carga = cte.id_viagem
                    LEFT JOIN tbmotoristas mot ON mot.codmot = cg.codmot
                    LEFT JOIN tbveiculos vei ON vei.codvei = cg.frota
                    LEFT JOIN tbtransportadoras pr ON pr.codtra = vei.codtra
                    WHERE cte.num_documento = @num";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@num", txtDocumento.Text);
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        if (dr["ordem_abastecimento"].ToString() == "")
                        {
                            txtEmissao.Text = dr["emissao_documento"] != DBNull.Value
                                ? Convert.ToDateTime(dr["emissao_documento"]).ToString("dd/MM/yyyy HH:mm")
                                : "";
                            txtFilial.Text = dr["empresa_emissora"].ToString();
                            txtCodMot.Text = dr["codmot"].ToString();
                            txtNomMot.Text = dr["nommot"].ToString();
                            txtCPF.Text = dr["cpf"].ToString();
                            txtCodVei.Text = dr["codvei"].ToString();
                            txtPlaca.Text = dr["plavei"].ToString();
                            txtModelo.Text = dr["tipvei"].ToString() + " - " + dr["modelo"].ToString() + " - " + dr["ano"].ToString() + " - " + dr["tipoveiculo"].ToString();
                            txtFilialVeic.Text = dr["nucleo"].ToString();
                            txtCodProp.Text = dr["codtra"].ToString();
                            txtTransp.Text = dr["fantra"].ToString();
                            txtCNPJ.Text = dr["cnpj"].ToString();
                            txtLimiteCredito.Text = dr["limitecreditoabastecimento"].ToString();
                            txtLitros.Focus();
                        }
                        else
                        {
                            Mensagem("info", "Documento: "+ txtDocumento.Text.Trim() + " - já tem a Ordem de Abastecimento: " + dr["ordem_abastecimento"].ToString() + " gerada.");
                            LimparCamposMotorista();
                            txtDocumento.Text = "";
                            txtDocumento.Focus();
                            return;

                        }
                        
                    }
                    else
                    {
                        // Limpa se não encontrou
                        Mensagem("danger", txtDocumento.Text.Trim() + " - Documento não encontrado.Verifique o número digitado!");
                        LimparCamposMotorista();
                        txtDocumento.Text = "";
                        txtDocumento.Focus();
                        return;
                    }

                    dr.Close();
                }
            }
            else if (customRadioNFSe.Checked)
            {
                // Lógica para NFSe, se necessário
                using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
                {
                    conn.Open();
                    string query = @"SELECT 
                        cte.num_documento, cte.emissao_documento, cte.ordem_abastecimento, cte.idviagem, 
                        cg.carga,
                        mot.codmot, mot.nommot, mot.cpf,
                        vei.codvei, vei.plavei, vei.codtra, vei.tipvei, vei.tipoveiculo, vei.modelo, vei.ano, vei.nucleo, 
                        pr.codtra, pr.fantra, pr.cnpj, pr.limitecreditoabastecimento
                    FROM tbnfse cte
                    LEFT JOIN tbcargas cg ON cg.carga = cte.idviagem
                    LEFT JOIN tbmotoristas mot ON mot.codmot = cg.codmot
                    LEFT JOIN tbveiculos vei ON vei.codvei = cg.frota
                    LEFT JOIN tbtransportadoras pr ON pr.codtra = vei.codtra
                    WHERE cte.num_documento = @num";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@num", txtDocumento.Text);
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        if (dr["ordem_abastecimento"].ToString() == "")
                        {
                            txtEmissao.Text = dr["emissao_documento"] != DBNull.Value
                                ? Convert.ToDateTime(dr["emissao_documento"]).ToString("dd/MM/yyyy HH:mm")
                                : "";                            
                            txtCodMot.Text = dr["codmot"].ToString();
                            txtNomMot.Text = dr["nommot"].ToString();
                            txtCPF.Text = dr["cpf"].ToString();
                            txtCodVei.Text = dr["codvei"].ToString();
                            txtPlaca.Text = dr["plavei"].ToString();
                            txtModelo.Text = dr["tipvei"].ToString() + " - " + dr["modelo"].ToString() + " - " + dr["ano"].ToString() + " - " + dr["tipoveiculo"].ToString();
                            txtFilialVeic.Text = dr["nucleo"].ToString();
                            txtCodProp.Text = dr["codtra"].ToString();
                            txtTransp.Text = dr["fantra"].ToString();
                            txtCNPJ.Text = dr["cnpj"].ToString();
                            txtLimiteCredito.Text = dr["limitecreditoabastecimento"].ToString();
                            txtLitros.Focus();
                        }
                        else
                        {
                            Mensagem("info", "Documento: " + txtDocumento.Text.Trim() + " - já tem a Ordem de Abastecimento: " + dr["ordem_abastecimento"].ToString() + " gerada.");
                            LimparCamposMotorista();
                            txtDocumento.Text = "";
                            txtDocumento.Focus();
                            return;

                        }

                    }
                    else
                    {
                        // Limpa se não encontrou
                        Mensagem("danger", txtDocumento.Text.Trim() + " - Documento não encontrado.Verifique o número digitado!");
                        LimparCamposMotorista();
                        txtDocumento.Text = "";
                        txtDocumento.Focus();
                        return;
                    }

                    dr.Close();
                }

            }
        }
        protected void txtLitros_TextChanged(object sender, EventArgs e)
        {
            decimal litros = 0;
            decimal preco = 0;
            decimal limite = 0;
            
            decimal.TryParse(txtLitros.Text.Replace(",", "."), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out litros);
            decimal.TryParse(txtPreco.Text.Replace(",", "."), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out preco);
            decimal.TryParse(txtLimiteCredito.Text.Replace(",", "."), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out limite);

            decimal total = litros * preco;
            decimal limiteLitros = limite / preco;
            txtValorTotal.Text = total.ToString("N2");

            if (total > limite)
            {
                Mensagem("info","Limite de crédito,libera até " + limiteLitros.ToString("N0") + " litros para abastecimento." );   
                txtValorTotal.Text = "";
                txtLitros.Focus();
                return;
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
        protected void Mensagem(string tipo, string texto)
        {
            divMsg.Visible = true;

            divMsg.Attributes["class"] =
                "alert alert-" + tipo + " alert-dismissible fade show mt-3";

            lblMsgGeral.Text = texto;
        }
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            // Tipos
            string abastecimentoFrotaAgregado = customRadioFrota.Checked ? "FROTA" : (customRadioAgregado.Checked ? "AGREGADO" : "");
            string tipoDocumento = customRadioCTe.Checked ? "CTe" : (customRadioNFSe.Checked ? "NFSe" : "");

            // Valores decimais
            decimal litros = 0, valorUnitario = 0, valorTotal = 0;
            decimal.TryParse(txtLitros.Text.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out litros);            
           
            decimal.TryParse(txtPreco.Text, NumberStyles.Number, new CultureInfo("pt-BR"), out valorUnitario);

            // Se quiser calcular o total
            valorTotal = litros * valorUnitario;

            // Usuário
            string usuario = Session["UsuarioLogado"].ToString();
            string lancadoPor = $"{DateTime.Now:dd/MM/yyyy HH:mm} - {usuario}";

            // Data emissão
            DateTime dataEmissao;
            if (!DateTime.TryParseExact(txtEmissao.Text, "dd/MM/yyyy HH:mm", CultureInfo.GetCultureInfo("pt-BR"), DateTimeStyles.None, out dataEmissao))
            {
                dataEmissao = DateTime.Now;
            }

            if (abastecimentoFrotaAgregado == "FROTA" || txtFilial.Text == "")
            {
               txtFilial.Text = txtFilialVeic.Text;
            }


            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();

                try
                {
                    string insertSql = @"
                        INSERT INTO tbsaida_combustivel
                        (cod_posto, nome_posto, tipo_abastecimento, frota_agregado, filial, cod_combustivel, combustivel,
                         litros, valor_unitario, valor_total, numero_documento, tipo_documento, data_emissao, codmot,
                         nommot, cpf, codvei, plavei, descricao_veiculo, codtra, nomtra, cnpj_cpf, lancado_por, impressa, data_geracao)   
                        OUTPUT INSERTED.ordem_abastecimento
                        VALUES
                        (@cod_posto, @nome_posto, @tipo_abastecimento, @frota_agregado, @filial, @cod_combustivel, @combustivel,
                         @litros, @valor_unitario, @valor_total, @numero_documento, @tipo_documento, @data_emissao, @codmot,
                         @nommot, @cpf, @codvei, @plavei, @descricao_veiculo, @codtra, @nomtra, @cnpj_cpf, @lancado_por, 'PENDENTE', GETDATE())";

                    SqlCommand cmdInsert = new SqlCommand(insertSql, conn, trans);
                    cmdInsert.Parameters.AddWithValue("@cod_posto", txtCodFor.Text.Trim());
                    cmdInsert.Parameters.AddWithValue("@nome_posto", txtNomFor.Text.Trim());
                    cmdInsert.Parameters.AddWithValue("@tipo_abastecimento", txtExterno.Text.Trim());
                    cmdInsert.Parameters.AddWithValue("@frota_agregado", abastecimentoFrotaAgregado);
                    cmdInsert.Parameters.AddWithValue("@filial", txtFilial.Text.Trim());
                    cmdInsert.Parameters.AddWithValue("@cod_combustivel", ddlCombustivel.SelectedValue);
                    cmdInsert.Parameters.AddWithValue("@combustivel", ddlCombustivel.SelectedItem.Text.Trim());
                    cmdInsert.Parameters.AddWithValue("@litros", litros);
                    cmdInsert.Parameters.AddWithValue("@valor_unitario", valorUnitario);
                    cmdInsert.Parameters.AddWithValue("@valor_total", valorTotal);
                    cmdInsert.Parameters.AddWithValue("@numero_documento", txtDocumento.Text.Trim());
                    cmdInsert.Parameters.AddWithValue("@tipo_documento", tipoDocumento);
                    cmdInsert.Parameters.AddWithValue("@data_emissao", dataEmissao);
                    cmdInsert.Parameters.AddWithValue("@codmot", txtCodMot.Text.Trim());
                    cmdInsert.Parameters.AddWithValue("@nommot", txtNomMot.Text.Trim());
                    cmdInsert.Parameters.AddWithValue("@cpf", txtCPF.Text.Trim());
                    cmdInsert.Parameters.AddWithValue("@codvei", txtCodVei.Text.Trim());
                    cmdInsert.Parameters.AddWithValue("@plavei", txtPlaca.Text.Trim());
                    cmdInsert.Parameters.AddWithValue("@descricao_veiculo", txtModelo.Text.Trim());
                    cmdInsert.Parameters.AddWithValue("@codtra", txtCodProp.Text.Trim());
                    cmdInsert.Parameters.AddWithValue("@nomtra", txtTransp.Text.Trim());
                    cmdInsert.Parameters.AddWithValue("@cnpj_cpf", txtCNPJ.Text.Trim());
                    cmdInsert.Parameters.AddWithValue("@lancado_por", lancadoPor);
                    
                    // Captura a ordem gerada
                    string ordemAbastecimento = cmdInsert.ExecuteScalar().ToString();

                    // UPDATE tbcte/tbnfse
                    if (tipoDocumento == "CTe")
                    {
                        string updateSql = "UPDATE tbcte SET ordem_abastecimento=@ordem WHERE num_documento=@numero_documento";
                        SqlCommand cmdUpdate = new SqlCommand(updateSql, conn, trans);
                        cmdUpdate.Parameters.AddWithValue("@ordem", ordemAbastecimento);
                        cmdUpdate.Parameters.AddWithValue("@numero_documento", txtDocumento.Text.Trim());
                        cmdUpdate.ExecuteNonQuery();

                    }
                    else if (tipoDocumento == "NFSe")
                    {
                        string updateSql = "UPDATE tbnfse SET ordem_abastecimento=@ordem WHERE num_documento=@numero_documento";
                        SqlCommand cmdUpdate = new SqlCommand(updateSql, conn, trans);
                        cmdUpdate.Parameters.AddWithValue("@ordem", ordemAbastecimento);
                        cmdUpdate.Parameters.AddWithValue("@numero_documento", txtDocumento.Text.Trim());
                        cmdUpdate.ExecuteNonQuery();
                    }                    

                    trans.Commit();

                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "ok", $"alert('Salvo com sucesso! Ordem: {ordemAbastecimento}');", true);
                    Mensagem("info", $"Ordem de Abastecimento {ordemAbastecimento} gerada com sucesso!");
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "erro", $"alert('Erro ao salvar: {ex.Message.Replace("'", "")}');", true);
                    Mensagem("danger", $"Erro ao salvar: {ex.Message.Replace("'", "")}");
                }
            }
        }
    }
}