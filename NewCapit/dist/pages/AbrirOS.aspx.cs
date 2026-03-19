using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DocumentFormat.OpenXml.Wordprocessing;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using PDFDocument = iTextSharp.text.Document;
using PDFPageSize = iTextSharp.text.PageSize;
using PDFFont = iTextSharp.text.Font;
using PDFFontFactory = iTextSharp.text.FontFactory;
using PDFTable = iTextSharp.text.pdf.PdfPTable;
using PDFParagraph = iTextSharp.text.Paragraph;
using PDFImage = iTextSharp.text.Image;

namespace NewCapit.dist.pages
{
    public partial class AbrirOS : System.Web.UI.Page
    {
        public string sReboque;
        public string nomeUsuario;
        
        SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString());
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                divMsg.Visible = false;
                if (Session["UsuarioLogado"] != null)
                {
                    string nomeUsuario = Session["UsuarioLogado"].ToString();
                    var lblUsuario = nomeUsuario;
                }
                else
                {
                    var lblUsuario = "<Usuário>";
                }
                PreencherComboMotoristas();
                CarregarFornecedores();

            }
        }
        protected void txtCodVeiculo_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCodVeiculo.Text))
                return;

            using (SqlConnection conn = new SqlConnection(
                WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                conn.Open();

                bool encontrou = false;

                // 🔎 1️⃣ TENTAR BUSCAR EM TBVEICULOS
                string sqlVeiculo = @"SELECT codvei, plavei, marca, modelo, ano, tipvei, nucleo, ativo_inativo
                              FROM tbveiculos
                              WHERE ISNUMERIC(codvei)=1 and codvei = @id";

                SqlCommand cmd = new SqlCommand(sqlVeiculo, conn);
                cmd.Parameters.AddWithValue("@id", txtCodVeiculo.Text);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    if (dr["ativo_inativo"].ToString() == "INATIVO")
                    {
                        Mensagem("warning", "Veículo / Carreta: " + txtCodVeiculo.Text.Trim() + ", está inativo no sistema. Verifique!");
                        LimparCampos();
                        txtCodVeiculo.Text = "";
                        txtCodVeiculo.Focus();
                        return;
                    }
                    divMsg.Visible = false;
                    txtCodVeiculo.Text = dr["codvei"].ToString();
                    txtPlaca.Text = dr["plavei"].ToString();
                    txtTipVei.Text = dr["tipvei"].ToString();
                    txtMarca.Text = dr["marca"].ToString();
                    txtModelo.Text = dr["modelo"].ToString();
                    txtAno.Text = dr["ano"].ToString();
                    txtNucleo.Text = dr["nucleo"].ToString();

                    encontrou = true;
                    txtKm.Focus();
                }

                dr.Close();

                // 🔎 2️⃣ SE NÃO ENCONTROU, BUSCAR EM TBCARRETAS
                if (!encontrou)
                {
                    string sqlCarreta = @"SELECT codcarreta, placacarreta, marca, modelo, anocarreta, nucleo, ativo_inativo
                                  FROM tbcarretas
                                  WHERE ISNUMERIC(codcarreta)=1 and codcarreta = @id";

                    SqlCommand cmdCarreta = new SqlCommand(sqlCarreta, conn);
                    cmdCarreta.Parameters.AddWithValue("@id", txtCodVeiculo.Text);

                    SqlDataReader drCarreta = cmdCarreta.ExecuteReader();

                    if (drCarreta.Read())
                    {
                        if (dr["ativo_inativo"].ToString() == "INATIVO")
                        {
                            Mensagem("warning", "Veículo / Carreta: " + txtCodVeiculo.Text.Trim() + ", está inativo no sistema. Verifique!");
                            LimparCampos();
                            txtCodVeiculo.Text = "";
                            txtCodVeiculo.Focus();
                            return;
                        }
                        divMsg.Visible = false;
                        txtCodVeiculo.Text = drCarreta["codcarreta"].ToString();
                        txtPlaca.Text = drCarreta["placacarreta"].ToString();
                        txtTipVei.Text = "CARRETA";
                        txtMarca.Text = drCarreta["marca"].ToString();
                        txtModelo.Text = drCarreta["modelo"].ToString();
                        txtAno.Text = drCarreta["anocarreta"].ToString();
                        txtNucleo.Text = drCarreta["nucleo"].ToString();

                        encontrou = true;
                        txtKm.Focus();
                    }

                    drCarreta.Close();
                }

                // ❌ Se não encontrou em nenhuma
                if (!encontrou)
                {
                    Mensagem("warning", "Veículo / Carreta: " + txtCodVeiculo.Text.Trim() + ", não cadastrado no sistema. Verifique!");
                    LimparCampos();
                    txtCodVeiculo.Text = "";
                    txtCodVeiculo.Focus();
                    return;
                }

                // 🔎 3️⃣ BUSCAR ÚLTIMA OS
                string sqlOS = @"SELECT TOP 1 id_os, data_abertura, status
                         FROM tbordem_servico
                         WHERE id_veiculo = @id
                         ORDER BY data_abertura DESC";

                SqlCommand cmdOS = new SqlCommand(sqlOS, conn);
                cmdOS.Parameters.AddWithValue("@id", txtCodVeiculo.Text);

                SqlDataReader drOS = cmdOS.ExecuteReader();

                if (drOS.Read())
                {
                    txtUltimaOS.Text = drOS["id_os"].ToString();
                    txtDataUltimaOS.Text =
                        Convert.ToDateTime(drOS["data_abertura"])
                        .ToString("dd/MM/yyyy HH:mm");
                    if (drOS["status"].ToString() == "1")
                    {
                        txtStatusUltOS.Text = "Aberta";
                        Mensagem("warning", "Veículo / Carreta: " + txtCodVeiculo.Text.Trim() + ", tem a Ordem de Serviço " + drOS["id_os"].ToString() + ", Aberta em " + txtDataUltimaOS.Text);
                        LimparCampos();
                        txtCodVeiculo.Text = "";
                        txtCodVeiculo.Focus();
                        return;


                    }
                    else if (drOS["status"].ToString() == "2")
                    {
                        txtStatusUltOS.Text = "Baixada";
                    }
                    
                }
                else
                {
                    txtUltimaOS.Text = "Nenhuma OS";
                    txtDataUltimaOS.Text = "";
                    txtKm.Focus();
                }

                drOS.Close();
            }
        }
        private void LimparCampos()
        {
            txtPlaca.Text = "";
            txtKm.Text = "";
            txtTipVei.Text = "";
            txtMarca.Text = "";
            txtModelo.Text = "";
            txtAno.Text = "";
            txtNucleo.Text = "";
            txtUltimaOS.Text = "";
            txtDataUltimaOS.Text = "";
            txtStatusUltOS.Text = "";
        }
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos())
                return;

            SalvarOS();
            
        }
        protected void ddlTipoServico_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTipoServico.SelectedValue == "E")
            {
                divFornecedor.Visible = true;
                CarregarFornecedores();
            }
            else
            {
                divFornecedor.Visible = false;
                cboFornecedores.Items.Clear();
            }
        }
        private void CarregarFornecedores()
        {
            cboFornecedores.Items.Clear();

            string sql = @"SELECT codfor, fantasia
                   FROM tbfornecedores
                   WHERE tipofornecedor = 'OFICINA EXTERNA'
                   ORDER BY fantasia"
            ;

            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();

                cboFornecedores.DataSource = dr;
                cboFornecedores.DataTextField = "fantasia";
                cboFornecedores.DataValueField = "codfor";
                cboFornecedores.DataBind();
            }

            cboFornecedores.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Selecione...", ""));
        }
        protected void ddlNome_Motorista_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtTransp_Motorista.Text = "";
            txtNucleo_Motorista.Text = "";
            txtCodVeiculo.Text = "";
            txtPlaca.Text = "";
            txtTipVei.Text = "";
            txtMarca.Text = "";
            txtModelo.Text = "";
            txtAno.Text = "";
            txtNucleo.Text = "";
            txtUltimaOS.Text = "";
            txtDataUltimaOS.Text = "";
            txtStatusUltOS.Text = "";
            ddlTipo.SelectedItem.Text = "Selecione...";
            ddlTipoServico.SelectedItem.Text = "Selecione...";
            txtParteMecanica.Text = "";
            txtParteEletrica.Text = "";
            txtParteBorracharia.Text = "";
            txtParteFunilaria.Text = "";

            txtId_Motorista.Text = ddlNome_Motorista.SelectedValue;
            string sql = @"SELECT codmot, nommot, transp, nucleo, status, fl_exclusao, reboque1
                        FROM tbmotoristas 
                        WHERE codmot = @id AND status = 'ATIVO' AND fl_exclusao IS NULL";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id", txtId_Motorista.Text);

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    Mensagem("warning", "Motorista: " + txtId_Motorista.Text.Trim() + ", não cadastrado no sistema. Verifique!");
                    txtId_Motorista.Text = "";
                    txtId_Motorista.Focus();
                    return;
                }
                if (dt.Rows[0]["status"].ToString() == "INATIVO")
                {
                    Mensagem("warning", "Motorista: " + txtId_Motorista.Text.Trim() + ", está inativo no sistema. Verifique!");
                    txtId_Motorista.Text = "";
                    txtId_Motorista.Focus();
                    return;
                }

                if (dt.Rows[0]["fl_exclusao"].ToString() == "S")
                {
                    Mensagem("warning", "Motorista: " + txtId_Motorista.Text.Trim() + ", deletado do sistema. Verifique!");
                    txtId_Motorista.Text = "";
                    txtId_Motorista.Focus();
                    return;
                }
                divMsg.Visible = false;
                txtId_Motorista.Text = dt.Rows[0]["codmot"].ToString();
                ddlNome_Motorista.SelectedItem.Text = dt.Rows[0]["nommot"].ToString();
                txtTransp_Motorista.Text = dt.Rows[0]["transp"].ToString();
                txtNucleo_Motorista.Text = dt.Rows[0]["nucleo"].ToString();
                sReboque = dt.Rows[0]["reboque1"].ToString();

                string valor = txtId_Motorista.Text.Trim().ToUpper();

                // 🔎 Verifica se começa com T
                if (valor.StartsWith("T"))
                {
                    BuscarCarreta(valor);
                }
                txtCodVeiculo.Focus();
            }

        }
        protected void txtId_Motorista_TextChanged(object sender, EventArgs e)
        {
            if (txtId_Motorista.Text != "")
            {
                ddlNome_Motorista.SelectedItem.Text = "Selecione...";
                txtTransp_Motorista.Text = "";
                txtNucleo_Motorista.Text = "";
                txtCodVeiculo.Text = "";
                txtPlaca.Text = "";
                txtTipVei.Text = "";
                txtMarca.Text = "";
                txtModelo.Text = "";
                txtAno.Text = "";
                txtNucleo.Text = "";
                txtUltimaOS.Text = "";
                txtDataUltimaOS.Text = "";
                txtStatusUltOS.Text = "";
                ddlTipo.SelectedItem.Text = "Selecione...";
                ddlTipoServico.SelectedItem.Text = "Selecione...";
                txtParteMecanica.Text = "";
                txtParteEletrica.Text = "";
                txtParteBorracharia.Text = "";
                txtParteFunilaria.Text = "";

                string sql = @"SELECT codmot, nommot, transp, nucleo, status, fl_exclusao, reboque1
                        FROM tbmotoristas 
                        WHERE codmot = @id";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", txtId_Motorista.Text);

                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    da.Fill(dt);

                    if (dt.Rows.Count == 0)
                    {
                        Mensagem("warning", "Motorista: " + txtId_Motorista.Text.Trim() + ", não cadastrado no sistema. Verifique!");
                        txtId_Motorista.Text = "";
                        txtId_Motorista.Focus();
                        return;
                    }
                    if (dt.Rows[0]["status"].ToString() == "INATIVO")
                    {
                        Mensagem("warning", "Motorista: " + txtId_Motorista.Text.Trim() + ", está inativo no sistema. Verifique!");
                        txtId_Motorista.Text = "";
                        txtId_Motorista.Focus();
                        return;
                    }

                    if (dt.Rows[0]["fl_exclusao"].ToString() == "S")
                    {
                        Mensagem("warning", "Motorista: " + txtId_Motorista.Text.Trim() + ", deletado do sistema. Verifique!");
                        txtId_Motorista.Text = "";
                        txtId_Motorista.Focus();
                        return;
                    }
                    divMsg.Visible = false;
                    txtId_Motorista.Text = dt.Rows[0]["codmot"].ToString();
                    ddlNome_Motorista.SelectedItem.Text = dt.Rows[0]["nommot"].ToString();
                    txtTransp_Motorista.Text = dt.Rows[0]["transp"].ToString();
                    txtNucleo_Motorista.Text = dt.Rows[0]["nucleo"].ToString();
                    sReboque = dt.Rows[0]["reboque1"].ToString();

                    string valor = txtId_Motorista.Text.Trim().ToUpper();

                    // 🔎 Verifica se começa com T
                    if (valor.StartsWith("T"))
                    {
                        BuscarCarreta(valor);
                    }
                    txtCodVeiculo.Focus();

                }

            }

        }
        private void BuscarCarreta(string placa)
        {
            placa = sReboque;

            using (SqlConnection conn = new SqlConnection(
                WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                conn.Open();

                string sql = @"SELECT codcarreta, placacarreta, marca, modelo, anocarreta, nucleo
                   FROM tbcarretas
                   WHERE placacarreta = @placa";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@placa", placa);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    string codVeiculo = dr["codcarreta"].ToString();

                    txtCodVeiculo.Text = codVeiculo;
                    txtPlaca.Text = dr["placacarreta"].ToString();
                    txtTipVei.Text = "CARRETA";
                    txtMarca.Text = dr["marca"].ToString();
                    txtModelo.Text = dr["modelo"].ToString();
                    txtAno.Text = dr["anocarreta"].ToString();
                    txtNucleo.Text = dr["nucleo"].ToString();

                    dr.Close();

                    if (sReboque != "")
                    {
                        string sqlOS = @"SELECT TOP 1 id_os, data_abertura, status
                             FROM tbordem_servico
                             WHERE id_veiculo = @id
                             ORDER BY data_abertura DESC";

                        SqlCommand cmdOS = new SqlCommand(sqlOS, conn);
                        cmdOS.Parameters.AddWithValue("@id", codVeiculo);

                        SqlDataReader drOS = cmdOS.ExecuteReader();

                        if (drOS.Read())
                        {
                            txtUltimaOS.Text = drOS["id_os"].ToString();
                            txtDataUltimaOS.Text =
                                Convert.ToDateTime(drOS["data_abertura"])
                                .ToString("dd/MM/yyyy HH:mm");

                            if (drOS["status"].ToString() == "1")
                            {
                                txtStatusUltOS.Text = "Aberta";
                                Mensagem("warning", "Veículo / Carreta: " + txtCodVeiculo.Text.Trim() + ", tem a Ordem de Serviço " + drOS["id_os"].ToString() + ", Aberta em " + txtDataUltimaOS.Text);
                                LimparCampos();
                                txtCodVeiculo.Text = "";
                                txtCodVeiculo.Focus();
                                return;


                            }
                            else if (drOS["status"].ToString() == "2")
                            {
                                txtStatusUltOS.Text = "Baixada";
                            }
                        }
                        else
                        {
                            txtUltimaOS.Text = "Nenhuma OS";
                            txtDataUltimaOS.Text = "";
                            txtKm.Focus();
                        }

                        drOS.Close();
                    }
                }
                else
                {
                    dr.Close();
                    txtCodVeiculo.Focus();
                }
            }
        }
        private void PreencherComboMotoristas()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT codmot, nommot FROM tbmotoristas WHERE fl_exclusao is null AND status = 'ATIVO' ORDER BY nommot";

            // Crie uma conexão com o banco de dados
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                try
                {
                    // Abra a conexão com o banco de dados
                    conn.Open();

                    // Crie o comando SQL
                    SqlCommand cmd = new SqlCommand(query, conn);



                    SqlDataReader reader = cmd.ExecuteReader();

                    // Preencher o ComboBox com os dados do DataReader
                    ddlNome_Motorista.DataSource = reader;
                    ddlNome_Motorista.DataTextField = "nommot";
                    ddlNome_Motorista.DataValueField = "codmot";
                    ddlNome_Motorista.DataBind();
                    ddlNome_Motorista.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Selecione...", ""));

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
        protected void Mensagem(string tipo, string texto)
        {
            divMsg.Visible = true;

            divMsg.Attributes["class"] =
                "alert alert-" + tipo + " alert-dismissible fade show mt-3";

            lblMsgGeral.Text = texto;
        }
        protected void txtPlaca_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPlaca.Text))
                return;

            using (SqlConnection conn = new SqlConnection(
                WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                conn.Open();

                bool encontrou = false;

                // 🔎 1️⃣ TENTAR BUSCAR EM TBVEICULOS
                string sqlVeiculo = @"SELECT codvei, plavei, marca, modelo, ano, tipvei, nucleo, ativo_inativo
                              FROM tbveiculos
                              WHERE plavei = @PlacaVei";

                SqlCommand cmd = new SqlCommand(sqlVeiculo, conn);
                cmd.Parameters.AddWithValue("@PlacaVei", txtPlaca.Text);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    if (dr["ativo_inativo"].ToString() == "INATIVO")
                    {
                        Mensagem("warning", "Veículo / Carreta: " + txtPlaca.Text.Trim() + ", está inativo no sistema. Verifique!");
                        LimparCampos();
                        txtPlaca.Text = "";
                        txtPlaca.Focus();
                        return;
                    }
                    divMsg.Visible = false;
                    txtCodVeiculo.Text = dr["codvei"].ToString();
                    txtPlaca.Text = dr["plavei"].ToString();
                    txtTipVei.Text = dr["tipvei"].ToString();
                    txtMarca.Text = dr["marca"].ToString();
                    txtModelo.Text = dr["modelo"].ToString();
                    txtAno.Text = dr["ano"].ToString();
                    txtNucleo.Text = dr["nucleo"].ToString();

                    encontrou = true;
                    txtKm.Focus();
                }

                dr.Close();

                // 🔎 2️⃣ SE NÃO ENCONTROU, BUSCAR EM TBCARRETAS
                if (!encontrou)
                {
                    string sqlCarreta = @"SELECT codcarreta, placacarreta, marca, modelo, anocarreta, nucleo, ativo_inativo
                                  FROM tbcarretas
                                  WHERE placacarreta = @PlacaCarreta";

                    SqlCommand cmdCarreta = new SqlCommand(sqlCarreta, conn);
                    cmdCarreta.Parameters.AddWithValue("@PlacaCarreta", txtPlaca.Text);

                    SqlDataReader drCarreta = cmdCarreta.ExecuteReader();

                    if (drCarreta.Read())
                    {
                        if (dr["ativo_inativo"].ToString() == "INATIVO")
                        {
                            Mensagem("warning", "Veículo / Carreta: " + txtPlaca.Text.Trim() + ", está inativo no sistema. Verifique!");
                            LimparCampos();
                            txtPlaca.Text = "";
                            txtPlaca.Focus();
                            return;
                        }
                        divMsg.Visible = false;
                        txtCodVeiculo.Text = drCarreta["codcarreta"].ToString();
                        txtPlaca.Text = drCarreta["placacarreta"].ToString();
                        txtTipVei.Text = "CARRETA";
                        txtMarca.Text = drCarreta["marca"].ToString();
                        txtModelo.Text = drCarreta["modelo"].ToString();
                        txtAno.Text = drCarreta["anocarreta"].ToString();
                        txtNucleo.Text = drCarreta["nucleo"].ToString();

                        encontrou = true;
                        txtKm.Focus();
                    }

                    drCarreta.Close();
                }

                // ❌ Se não encontrou em nenhuma
                if (!encontrou)
                {
                    Mensagem("warning", "Veículo / Carreta: " + txtPlaca.Text.Trim() + ", não cadastrado no sistema. Verifique!");
                    LimparCampos();
                    txtPlaca.Text = "";
                    txtPlaca.Focus();
                    return;
                }

                // 🔎 3️⃣ BUSCAR ÚLTIMA OS
                string sqlOS = @"SELECT TOP 1 id_os, data_abertura, status
                         FROM tbordem_servico
                         WHERE id_veiculo = @id
                         ORDER BY data_abertura DESC";

                SqlCommand cmdOS = new SqlCommand(sqlOS, conn);
                cmdOS.Parameters.AddWithValue("@id", txtCodVeiculo.Text);

                SqlDataReader drOS = cmdOS.ExecuteReader();

                if (drOS.Read())
                {
                    txtUltimaOS.Text = drOS["id_os"].ToString();
                    txtDataUltimaOS.Text =
                        Convert.ToDateTime(drOS["data_abertura"])
                        .ToString("dd/MM/yyyy HH:mm");
                    if (drOS["status"].ToString() == "1")
                    {
                        txtStatusUltOS.Text = "Aberta";
                        Mensagem("warning", "Veículo / Carreta: " + txtCodVeiculo.Text.Trim() + ", tem a Ordem de Serviço " + drOS["id_os"].ToString() + ", Aberta em " + txtDataUltimaOS.Text);
                        LimparCampos();
                        txtCodVeiculo.Text = "";
                        txtCodVeiculo.Focus();
                        return;


                    }
                    else if (drOS["status"].ToString() == "2")
                    {
                        txtStatusUltOS.Text = "Baixada";
                    }
                }
                else
                {
                    txtUltimaOS.Text = "Nenhuma OS";
                    txtDataUltimaOS.Text = "";
                    txtKm.Focus();
                }

                drOS.Close();
            }

        }
        private void MarcarErro(WebControl campo)
        {
            campo.CssClass += " campo-erro";
            ScriptManager.RegisterStartupScript(this, GetType(),
                    "scroll", "window.scrollTo(0,0);", true);
        }
        private void LimparErros()
        {
            txtId_Motorista.CssClass = txtId_Motorista.CssClass.Replace("campo-erro", "");
            txtCodVeiculo.CssClass = txtCodVeiculo.CssClass.Replace("campo-erro", "");
            txtKm.CssClass = txtKm.CssClass.Replace("campo-erro", "");
        }
        private bool ValidarCampos()
        {
            LimparErros();

            bool valido = true;

            if (string.IsNullOrWhiteSpace(txtId_Motorista.Text))
            {
                MarcarErro(txtId_Motorista);
                valido = false;
            }

            if (string.IsNullOrWhiteSpace(txtCodVeiculo.Text))
            {
                MarcarErro(txtCodVeiculo);
                valido = false;
            }

            if (string.IsNullOrWhiteSpace(txtKm.Text))
            {
                MarcarErro(txtKm);
                valido = false;
            }

            //if (ddlTipo.SelectedIndex == 0)
            //{
            //    MarcarErro(ddlTipo);
            //    valido = false;
            //}

            //if (ddlTipoServico.SelectedIndex == 0)
            //{
            //    MarcarErro(ddlTipoServico);
            //    valido = false;
            //}

            if (!ValidarDropDown(ddlTipo)) valido = false;
            if (!ValidarDropDown(ddlTipoServico)) valido = true;

            if (string.IsNullOrWhiteSpace(txtParteMecanica.Text) &&
                string.IsNullOrWhiteSpace(txtParteEletrica.Text) &&
                string.IsNullOrWhiteSpace(txtParteBorracharia.Text) &&
                string.IsNullOrWhiteSpace(txtParteFunilaria.Text))
            {
                Mensagem("warning", "Informe pelo menos uma descrição do problema.");
                valido = false;
            }

            if (!valido)
                Mensagem("danger", "Preencha os campos obrigatórios.");

            return valido;
        }
        bool ValidarDropDown(DropDownList ddl)
        {
            if (ddl.SelectedValue == "0")
            {
                ddl.BorderColor = System.Drawing.Color.Red;
                return false;
            }
            else
            {
                ddl.BorderColor = System.Drawing.Color.Empty;
                return true;
            }
        }        
        private int SalvarOS()
        {
            string idVeiculo = null;
            string idCarreta = null;

            using (SqlConnection conn = new SqlConnection(
                WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                conn.Open();

                // verifica veiculo
                string sqlVeiculo = "SELECT codvei FROM tbveiculos WHERE codvei = @id";

                SqlCommand cmd = new SqlCommand(sqlVeiculo, conn);
                cmd.Parameters.AddWithValue("@id", txtCodVeiculo.Text);

                var result = cmd.ExecuteScalar();

                if (result != null)
                {
                    idVeiculo = result.ToString();
                }
                else
                {
                    // verifica carreta
                    string sqlCarreta = "SELECT codcarreta FROM tbcarretas WHERE codcarreta = @id";

                    SqlCommand cmd2 = new SqlCommand(sqlCarreta, conn);
                    cmd2.Parameters.AddWithValue("@id", txtCodVeiculo.Text);

                    var result2 = cmd2.ExecuteScalar();

                    if (result2 != null)
                        idCarreta = result2.ToString();
                }
            }

            int numeroOS = 0;

            using (SqlConnection conn = new SqlConnection(
                WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                conn.Open();

                string sql = @"INSERT INTO tbordem_servico
                       (
                           data_abertura,
                           id_motorista,
                           nome_motorista,
                           transp_motorista,
                           nucleo_motorista,
                           id_veiculo,
                           id_carreta,
                           placa,
                           tipo_veiculo,
                           marca,
                           modelo,
                           ano_modelo,
                           nucleo_veiculo,                          
                           km_abertura,
                           tipo_os,
                           interno_externo,
                           parte_mecanica,
                           parte_eletrica,
                           parte_borracharia,
                           parte_funilaria,
                           id_fornecedor,
                           nome_fornecedor,
                           resp_abertura,
                           status
                       )
                       VALUES
                       (
                          @data_abertura,
                          @id_motorista,
                          @nome_motorista,
                          @transp_motorista,
                          @nucleo_motorista,
                          @id_veiculo,
                          @id_carreta,
                          @placa,
                          @tipo_veiculo,
                          @marca,
                          @modelo,
                          @ano_modelo,
                          @nucleo_veiculo,                          
                          @km_abertura,
                          @tipo_os,
                          @interno_externo,
                          @parte_mecanica,
                          @parte_eletrica,
                          @parte_borracharia,
                          @parte_funilaria,
                          @id_fornecedor,
                          @nome_fornecedor,
                          @resp_abertura,
                          1
                       );

                       SELECT SCOPE_IDENTITY();";

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@data_abertura", DateTime.Now);
                cmd.Parameters.AddWithValue("@id_motorista", txtId_Motorista.Text);
                cmd.Parameters.AddWithValue("@nome_motorista", ddlNome_Motorista.SelectedItem.Text);
                cmd.Parameters.AddWithValue("@transp_motorista", txtTransp_Motorista.Text);
                cmd.Parameters.AddWithValue("@nucleo_motorista", txtNucleo_Motorista.Text);
                cmd.Parameters.AddWithValue("@id_veiculo", (object)idVeiculo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@id_carreta", (object)idCarreta ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@placa", txtPlaca.Text);
                cmd.Parameters.AddWithValue("@tipo_veiculo", txtTipVei.Text);
                cmd.Parameters.AddWithValue("@marca", txtMarca.Text);
                cmd.Parameters.AddWithValue("@modelo", txtModelo.Text);
                cmd.Parameters.AddWithValue("@ano_modelo", txtAno.Text);
                cmd.Parameters.AddWithValue("@km_abertura", txtKm.Text);
                cmd.Parameters.AddWithValue("@nucleo_veiculo", txtNucleo.Text);
                cmd.Parameters.AddWithValue("@tipo_os", ddlTipo.SelectedValue);
                cmd.Parameters.AddWithValue("@interno_externo", ddlTipoServico.SelectedValue);
                if (cboFornecedores.SelectedIndex <= 0)
                {
                    cmd.Parameters.AddWithValue("@id_fornecedor", DBNull.Value);
                    cmd.Parameters.AddWithValue("@nome_fornecedor", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@id_fornecedor", cboFornecedores.SelectedValue);
                    cmd.Parameters.AddWithValue("@nome_fornecedor", cboFornecedores.SelectedItem.Text);
                }

                cmd.Parameters.AddWithValue("@parte_mecanica", txtParteMecanica.Text);
                cmd.Parameters.AddWithValue("@parte_eletrica", txtParteEletrica.Text);
                cmd.Parameters.AddWithValue("@parte_borracharia", txtParteBorracharia.Text);
                cmd.Parameters.AddWithValue("@parte_funilaria", txtParteFunilaria.Text);
                cmd.Parameters.AddWithValue("@resp_abertura", Session["UsuarioLogado"].ToString());

                numeroOS = Convert.ToInt32(cmd.ExecuteScalar());
            }

            Mensagem("success", "Ordem de serviço criada com sucesso. Nº " + numeroOS);

            // gerar PDF automático
            //GerarPDF(numeroOS);

            OrdemServico os = new OrdemServico();

            os.numero_os = numeroOS;
            os.data_abertura = DateTime.Now;

            // dados do usuario
            os.resp_abertura = Session["UsuarioLogado"].ToString();
            // dados do veiculo
            os.id_veiculo = txtCodVeiculo.Text;
            os.placa = txtPlaca.Text;
            os.tipo_veiculo = txtTipVei.Text;
            os.marca = txtMarca.Text;
            os.modelo = txtModelo.Text;
            os.ano_modelo =  txtAno.Text;
            os.nucleo_veiculo = txtNucleo.Text;
            os.km_abertura = txtKm.Text;

            os.id_motorista = txtId_Motorista.Text;
            os.nome_motorista = ddlNome_Motorista.SelectedItem.Text;
            os.transp_motorista = txtTransp_Motorista.Text;
            os.nucleo_motorista = txtNucleo_Motorista.Text;

            os.tipo_os = ddlTipo.SelectedItem.Text;
            os.tipo_servico = ddlTipoServico.SelectedItem.Text;
            
            if (os.tipo_servico == "Externo")
            {
                os.id_fornecedor = cboFornecedores.SelectedValue;
                os.nome_fornecedor = cboFornecedores.SelectedItem.Text;
            }
            else
            {
                os.id_fornecedor = "6424";
                os.nome_fornecedor = "MANUTENÇÃO - INTERNA";
            }
            os.parte_mecanica = txtParteMecanica.Text;
            os.parte_eletrica = txtParteEletrica.Text;
            os.parte_borracharia = txtParteBorracharia.Text;
            os.parte_funilaria = txtParteFunilaria.Text;  

            byte[] pdf = GeradorPDFOS.GerarPDF(os);

            //Response.Clear();
            //Response.ContentType = "application/pdf";
            //Response.AddHeader("content-disposition", "attachment;filename=OS_" + numeroOS + ".pdf");
            //Response.BinaryWrite(pdf);
            //Response.End();

            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=OS_" + numeroOS + ".pdf");

            Response.BinaryWrite(pdf);
            Response.Flush();
            HttpContext.Current.ApplicationInstance.CompleteRequest();

            return numeroOS;
        }
        public class OrdemServico
        {
            public int numero_os { get; set; }
            public DateTime data_abertura { get; set; }
            public string resp_abertura { get; set; }

            public string id_motorista { get; set; }
            public string nome_motorista { get; set; }
            public string transp_motorista { get; set; }
            public string nucleo_motorista { get; set; }

            public string id_veiculo { get; set; }
            public string placa { get; set; }
            public string tipo_veiculo { get; set; }
            public string marca { get; set; }
            public string modelo { get; set; }
            public string ano_modelo { get; set; }
            public string nucleo_veiculo { get; set; }

            public string km_abertura { get; set; }

            public string tipo_os { get; set; }
            public string tipo_servico { get; set; }

            public string parte_mecanica { get; set; }
            public string parte_eletrica { get; set; }
            public string parte_borracharia { get; set; }
            public string parte_funilaria { get; set; }

            public string id_fornecedor { get; set; }
            public string nome_fornecedor { get; set; }

            public string status { get; set; }
        }


       
    }
}