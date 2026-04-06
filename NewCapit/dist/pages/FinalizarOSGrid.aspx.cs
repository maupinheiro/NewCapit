using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Data;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Globalization;


namespace NewCapit.dist.pages
{
    public partial class FinalizarOSGrid : System.Web.UI.Page
    {
        SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString());
        string nomeUsuario = null;
        string numero_os;
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
                    Response.Redirect("Login.aspx");
                }

                CarregaDados();
                AtualizarNumeroPneu();
                PreencherDestino();
                PreencherPosicao();
                CarregarPecas();
                CarregarMecanicos();
                CalcularTotais();                
            }
        }
        public void CarregaDados()
        {
            if (HttpContext.Current.Request.QueryString["os"].ToString() != "")
            {
                numero_os = HttpContext.Current.Request.QueryString["os"].ToString();
            }
            using (SqlConnection conn = new SqlConnection(
                WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                conn.Open();

                bool encontrou = false;

                
                string sqlVeiculo = @"SELECT id_os,id_motorista,nome_motorista,transp_motorista,nucleo_motorista,tipo_os,           id_veiculo,id_carreta,placa,tipo_veiculo,marca	,modelo	,ano_modelo,nucleo_veiculo,data_abertura,km_abertura,resp_abertura,status,	
                    parte_mecanica,parte_eletrica,parte_borracharia,parte_funilaria,interno_externo,id_fornecedor,nome_fornecedor
                    id_profissional,previsao_entrega,status,data_fechamento,km_fechamento,resp_abertura,resp_baixa,DATEDIFF(DAY, data_abertura, GETDATE()) AS dias_aberta
                              FROM tbordem_servico
                              WHERE id_os = @id";

                SqlCommand cmd = new SqlCommand(sqlVeiculo, conn);
                cmd.Parameters.AddWithValue("@id", numero_os);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    divMsg.Visible = false;
                    txtOS.Text = dr["id_os"].ToString();
                    txtId_Motorista.Text = dr["id_motorista"].ToString();
                    txtNome_Motorista.Text = dr["nome_motorista"].ToString();
                    txtRespAbertura.Text = dr["resp_abertura"].ToString();
                    txtTransp_Motorista.Text = dr["transp_motorista"].ToString();
                    txtNucleo_Motorista.Text = dr["nucleo_motorista"].ToString();

                    txtPlaca.Text = dr["placa"].ToString();
                    txtTipVei.Text = dr["tipo_veiculo"].ToString();                    
                    txtMarca.Text = dr["marca"].ToString();
                    txtModelo.Text = dr["modelo"].ToString();
                    txtAno.Text = dr["ano_modelo"].ToString();
                    txtNucleo.Text = dr["nucleo_veiculo"].ToString();
                    txtKm.Text = dr["km_abertura"].ToString();

                    txtParteFunilaria.Text = dr["parte_funilaria"].ToString();
                    txtParteBorracharia.Text = dr["parte_borracharia"].ToString();
                    txtParteEletrica.Text = dr["parte_eletrica"].ToString();
                    txtParteMecanica.Text = dr["parte_mecanica"].ToString();
                    txtAbertura.Text = DateTime.Parse(dr["data_abertura"].ToString()).ToString("dd/MM/yyyy HH:mm");

                    if (dr["tipo_veiculo"].ToString() == "CARRETA")
                    {
                        txtCodVeiculo.Text = dr["id_carreta"].ToString();
                    }
                    else
                    {
                        txtCodVeiculo.Text = dr["id_veiculo"].ToString();
                    }
                    caminhaoParado.Visible = true;

                    int diasAberta = Convert.ToInt32(dr["dias_aberta"]);

                    if (diasAberta == 0)
                    {
                        txtTempoParado.Text = "Veículo/Carreta na Oficina a " + diasAberta + " dia(s)";
                        txtTempoParado.BackColor = System.Drawing.Color.GreenYellow;
                        txtTempoParado.ForeColor = System.Drawing.Color.White;
                    }

                    if (diasAberta == 1)
                    {
                        txtTempoParado.Text = "Veículo/Carreta na Oficina a " + diasAberta + " dia(s)";
                        txtTempoParado.BackColor = System.Drawing.Color.Khaki;
                        txtTempoParado.ForeColor = System.Drawing.Color.OrangeRed;
                    }

                    if (diasAberta >= 2)
                    {
                        txtTempoParado.Text = "Veículo/Carreta na Oficina a " + diasAberta + " dia(s)";
                        txtTempoParado.BackColor = System.Drawing.Color.Red;
                        txtTempoParado.ForeColor = System.Drawing.Color.White;
                    }

                    if (dr["status"].ToString() == "1")
                    {
                        situacao.Visible = false;
                        fechamento.Visible = false;
                        responsavel.Visible = false;
                        txtStatus.Text = "Aberta";
                        txtStatus.BackColor = System.Drawing.Color.Green;
                        txtStatus.ForeColor = System.Drawing.Color.White;
                    }
                    if (dr["status"].ToString() == "2")
                    {
                        txtStatus.Text = "Finalizada";
                        txtStatus.BackColor = System.Drawing.Color.Purple;
                        txtStatus.ForeColor = System.Drawing.Color.White;
                        situacao.Visible = true;
                        fechamento.Visible = true;
                        responsavel.Visible = true;
                        lblStatus.Text = "Finalizada";
                        lblFechamento.Text = DateTime.Parse(dr["data_fechamento"].ToString()).ToString("dd/MM/yyyy HH:mm");
                        lblResp_fechamento.Text = dr["resp_baixa"].ToString();
                        btnFinalizar.Enabled = false;
                    }
                    if (dr["status"].ToString() == "3")
                    {
                        txtStatus.Text = "Cancelada";
                        txtStatus.BackColor = System.Drawing.Color.Red;
                        txtStatus.ForeColor = System.Drawing.Color.White;
                        situacao.Visible = true;
                        fechamento.Visible = true;
                        responsavel.Visible = true;
                        lblStatus.Text = "Cancelada";
                        lblFechamento.Text = DateTime.Parse(dr["data_fechamento"].ToString()).ToString("dd/MM/yyyy HH:mm");
                        lblResp_fechamento.Text = dr["resp_baixa"].ToString();
                        btnFinalizar.Enabled = false;
                    }
                    CarregarGrid();
                    CarregarGridEletrica();
                    CarregarGridBorracharia();
                    //PreencherPosicao();
                   
                    CarregarGridFunilaria();
                    CalcularTotais();
                    encontrou = true;
                }

                dr.Close();

                // ❌ Se não encontrou em nenhuma
                if (!encontrou)
                {
                    Mensagem("warning", "Ordem de Serviço: " + txtOS.Text.Trim() + ", não cadastrada no sistema. Verifique!");
                    LimparCampos();
                    txtOS.Focus();
                    return;
                }
            } 
        }
        protected void txtOS_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtOS.Text))
                return;

            using (SqlConnection conn = new SqlConnection(
                WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                conn.Open();

                bool encontrou = false;

                // 🔎 1️⃣ TENTAR BUSCAR EM TBVEICULOS
                string sqlVeiculo = @"SELECT id_os,id_motorista,nome_motorista,transp_motorista,nucleo_motorista,tipo_os,           id_veiculo,id_carreta,placa,tipo_veiculo,marca	,modelo	,ano_modelo,nucleo_veiculo,data_abertura,km_abertura,resp_abertura,status,	
                    parte_mecanica,parte_eletrica,parte_borracharia,parte_funilaria,interno_externo,id_fornecedor,nome_fornecedor
                    id_profissional,previsao_entrega,status,data_fechamento,km_fechamento,resp_abertura,resp_baixa,DATEDIFF(DAY, data_abertura, GETDATE()) AS dias_aberta
                              FROM tbordem_servico
                              WHERE id_os = @id";

                SqlCommand cmd = new SqlCommand(sqlVeiculo, conn);
                cmd.Parameters.AddWithValue("@id", txtOS.Text);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    divMsg.Visible = false;
                    txtOS.Text = dr["id_os"].ToString();
                    txtId_Motorista.Text = dr["id_motorista"].ToString();
                    txtNome_Motorista.Text = dr["nome_motorista"].ToString();
                    txtRespAbertura.Text = dr["resp_abertura"].ToString();
                    txtTransp_Motorista.Text = dr["transp_motorista"].ToString();
                    txtNucleo_Motorista.Text = dr["nucleo_motorista"].ToString();

                    txtPlaca.Text = dr["placa"].ToString();
                    txtTipVei.Text = dr["tipo_veiculo"].ToString();
                    txtMarca.Text = dr["marca"].ToString();
                    txtModelo.Text = dr["modelo"].ToString();
                    txtAno.Text = dr["ano_modelo"].ToString();
                    txtNucleo.Text = dr["nucleo_veiculo"].ToString();
                    txtKm.Text = dr["km_abertura"].ToString();

                    txtParteFunilaria.Text = dr["parte_funilaria"].ToString();
                    txtParteBorracharia.Text = dr["parte_borracharia"].ToString();
                    txtParteEletrica.Text = dr["parte_eletrica"].ToString();
                    txtParteMecanica.Text = dr["parte_mecanica"].ToString();
                    txtAbertura.Text = DateTime.Parse(dr["data_abertura"].ToString()).ToString("dd/MM/yyyy HH:mm");

                    if (dr["tipo_veiculo"].ToString() == "CARRETA")
                    {
                        txtCodVeiculo.Text = dr["id_carreta"].ToString();
                    }
                    else
                    {
                        txtCodVeiculo.Text = dr["id_veiculo"].ToString();
                    }
                    caminhaoParado.Visible = true;

                    int diasAberta = Convert.ToInt32(dr["dias_aberta"]);

                    if (diasAberta == 0)
                    {
                        txtTempoParado.Text = "Veículo/Carreta na Oficina a " + diasAberta + " dia(s)";
                        txtTempoParado.BackColor = System.Drawing.Color.GreenYellow;
                        txtTempoParado.ForeColor = System.Drawing.Color.White;
                    }

                    if (diasAberta == 1)
                    {
                        txtTempoParado.Text = "Veículo/Carreta na Oficina a " + diasAberta + " dia(s)";
                        txtTempoParado.BackColor = System.Drawing.Color.Khaki;
                        txtTempoParado.ForeColor = System.Drawing.Color.OrangeRed;
                    }

                    if (diasAberta >= 2)
                    {
                        txtTempoParado.Text = "Veículo/Carreta na Oficina a " + diasAberta + " dia(s)";
                        txtTempoParado.BackColor = System.Drawing.Color.Red;
                        txtTempoParado.ForeColor = System.Drawing.Color.White;
                    }

                    if (dr["status"].ToString() == "1")
                    {
                        situacao.Visible = false;
                        fechamento.Visible = false;
                        responsavel.Visible = false;
                        txtStatus.Text = "Aberta";
                        txtStatus.BackColor = System.Drawing.Color.Green;
                        txtStatus.ForeColor = System.Drawing.Color.White;
                    }
                    if (dr["status"].ToString() == "2")
                    {
                        txtStatus.Text = "Finalizada";
                        txtStatus.BackColor = System.Drawing.Color.Purple;
                        txtStatus.ForeColor = System.Drawing.Color.White;
                        situacao.Visible = true;
                        fechamento.Visible = true;
                        responsavel.Visible = true;
                        lblStatus.Text = "Finalizada";
                        lblFechamento.Text = DateTime.Parse(dr["data_fechamento"].ToString()).ToString("dd/MM/yyyy HH:mm");
                        lblResp_fechamento.Text = dr["resp_baixa"].ToString();
                        btnFinalizar.Enabled = false;
                    }
                    if (dr["status"].ToString() == "3")
                    {
                        txtStatus.Text = "Cancelada";
                        txtStatus.BackColor = System.Drawing.Color.Red;
                        txtStatus.ForeColor = System.Drawing.Color.White;
                        situacao.Visible = true;
                        fechamento.Visible = true;
                        responsavel.Visible = true;
                        lblStatus.Text = "Cancelada";
                        lblFechamento.Text = DateTime.Parse(dr["data_fechamento"].ToString()).ToString("dd/MM/yyyy HH:mm");
                        lblResp_fechamento.Text = dr["resp_baixa"].ToString();
                        btnFinalizar.Enabled = false;
                    }
                    CarregarGrid();
                    CarregarGridEletrica();
                    CarregarGridBorracharia();
                    CarregarGridFunilaria();
                    CalcularTotais();
                    encontrou = true;
                }

                dr.Close();

                // ❌ Se não encontrou em nenhuma
                if (!encontrou)
                {
                    Mensagem("warning", "Ordem de Serviço: " + txtOS.Text.Trim() + ", não cadastrada no sistema. Verifique!");
                    LimparCampos();
                    txtOS.Focus();
                    return;
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
        private void LimparCampos()
        {
            txtOS.Text = "";
            txtPlaca.Text = "";
            txtKm.Text = "";
            txtTipVei.Text = "";
            txtMarca.Text = "";
            txtModelo.Text = "";
            txtAno.Text = "";
            txtNucleo.Text = "";
            txtId_Motorista.Text = "";
            txtNome_Motorista.Text = "";
            txtTransp_Motorista.Text = "";
            txtNucleo_Motorista.Text = "";
            txtStatus.Text = "";
            txtRespAbertura.Text = "";
            lblCustoTotal.Text = "";
            lblTempoTotal.Text = "";
            lblTotalPecas.Text = "";
            caminhaoParado.Visible = false;

        }
        void CarregarPecas()
        {
            using (SqlConnection conn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                string sql = "SELECT id_peca, descricao_peca FROM tbestoque_pecas";

                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlPeca.DataSource = dt;
                ddlPeca.DataTextField = "descricao_peca";
                ddlPeca.DataValueField = "id_peca";
                ddlPeca.DataBind();
                ddlPeca.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Selecione...", ""));

                ddlParteEletrica.DataSource = dt;
                ddlParteEletrica.DataTextField = "descricao_peca";
                ddlParteEletrica.DataValueField = "id_peca";
                ddlParteEletrica.DataBind();
                ddlParteEletrica.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Selecione...", ""));

                ddlParteBorracharia.DataSource = dt;
                ddlParteBorracharia.DataTextField = "descricao_peca";
                ddlParteBorracharia.DataValueField = "id_peca";
                ddlParteBorracharia.DataBind();
                ddlParteBorracharia.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Selecione...", ""));

                ddlParteFunilaria.DataSource = dt;
                ddlParteFunilaria.DataTextField = "descricao_peca";
                ddlParteFunilaria.DataValueField = "id_peca";
                ddlParteFunilaria.DataBind();
                ddlParteFunilaria.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Selecione...", ""));
            }
        }
        void CarregarMecanicos()
        {
            using (SqlConnection conn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                string sql = "SELECT cracha, nome FROM tbprofissional_manutencao WHERE fl_exclusao IS NULL AND status = 'ATIVO'";

                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlMecanico.DataSource = dt;
                ddlMecanico.DataTextField = "nome";
                ddlMecanico.DataValueField = "cracha";
                ddlMecanico.DataBind();
                ddlMecanico.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Selecione", ""));

                ddlEletricista.DataSource = dt;
                ddlEletricista.DataTextField = "nome";
                ddlEletricista.DataValueField = "cracha";
                ddlEletricista.DataBind();
                ddlEletricista.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Selecione", ""));

                ddlBorracheiro.DataSource = dt;
                ddlBorracheiro.DataTextField = "nome";
                ddlBorracheiro.DataValueField = "cracha";
                ddlBorracheiro.DataBind();
                ddlBorracheiro.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Selecione", ""));

                ddlFunileiro.DataSource = dt;
                ddlFunileiro.DataTextField = "nome";
                ddlFunileiro.DataValueField = "cracha";
                ddlFunileiro.DataBind();
                ddlFunileiro.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Selecione", ""));
            }
        }

        protected void CarregarGrid()
        {
            string sTipo = txtTipo.Text.ToString();
            using (SqlConnection conn = new SqlConnection(
                WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                conn.Open();

                string sql = @"
                SELECT 
                p.id,
                pe.descricao_peca,
                p.quant,
                p.valor_unitario,
                p.valor_total,
                m.cracha,
                m.nome,
                p.inicio,
                p.termino,
                p.tempo_minutos,
                p.tipo
                FROM tbos_pecas p
                INNER JOIN tbestoque_pecas pe ON pe.id_peca = p.id_peca
                INNER JOIN tbprofissional_manutencao m ON m.cracha = p.cracha
                WHERE p.id_os = @os
                AND p.tipo = @tipo
                ORDER BY p.inicio";

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@os", txtOS.Text);
                cmd.Parameters.AddWithValue("@tipo", sTipo);

                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();

                da.Fill(dt);

                gridPecas.DataSource = dt;
                gridPecas.DataBind();


            }
        }
        protected void CarregarGridEletrica()
        {
            string sTipo = txtTipoEletrica.Text.ToString();
            using (SqlConnection conn = new SqlConnection(
                WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                conn.Open();

                string sql = @"
                SELECT 
                p.id,
                pe.descricao_peca,
                p.quant,
                p.valor_unitario,
                p.valor_total,
                m.cracha,
                m.nome,
                p.inicio,
                p.termino,
                p.tempo_minutos,
                p.tipo
                FROM tbos_pecas p
                INNER JOIN tbestoque_pecas pe ON pe.id_peca = p.id_peca
                INNER JOIN tbprofissional_manutencao m ON m.cracha = p.cracha
                WHERE p.id_os = @os
                AND p.tipo = @tipo
                ORDER BY p.inicio";

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@os", txtOS.Text);
                cmd.Parameters.AddWithValue("@tipo", sTipo);

                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();

                da.Fill(dt);

                gridPecasEletrica.DataSource = dt;
                gridPecasEletrica.DataBind();
            }
        }
        protected void CarregarGridBorracharia()
        {
            string sTipo = txtTipoBorracharia.Text.ToString();
            using (SqlConnection conn = new SqlConnection(
                WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                conn.Open();

                string sql = @"
                SELECT 
                p.id,
                pe.descricao_peca,
                p.quant,
                p.valor_unitario,
                p.valor_total,
                m.cracha,
                m.nome,
                p.inicio,
                p.termino,
                p.tempo_minutos,
                p.tipo
                FROM tbos_pecas p
                INNER JOIN tbestoque_pecas pe ON pe.id_peca = p.id_peca
                INNER JOIN tbprofissional_manutencao m ON m.cracha = p.cracha
                WHERE p.id_os = @os
                AND p.tipo = @tipo
                ORDER BY p.inicio";

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@os", txtOS.Text);
                cmd.Parameters.AddWithValue("@tipo", sTipo);

                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();

                da.Fill(dt);

                gridPecasBorracharia.DataSource = dt;
                gridPecasBorracharia.DataBind();
            }
        }
        protected void CarregarGridFunilaria()
        {
            string sTipo = txtTipoFunilaria.Text.ToString();
            using (SqlConnection conn = new SqlConnection(
                WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                conn.Open();

                string sql = @"
                SELECT 
                p.id,
                pe.descricao_peca,
                p.quant,
                p.valor_unitario,
                p.valor_total,
                m.cracha,
                m.nome,
                p.inicio,
                p.termino,
                p.tempo_minutos,
                p.tipo
                FROM tbos_pecas p
                INNER JOIN tbestoque_pecas pe ON pe.id_peca = p.id_peca
                INNER JOIN tbprofissional_manutencao m ON m.cracha = p.cracha
                WHERE p.id_os = @os
                AND p.tipo = @tipo
                ORDER BY p.inicio";

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@os", txtOS.Text);
                cmd.Parameters.AddWithValue("@tipo", sTipo);

                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();

                da.Fill(dt);

                gridPecasFunilaria.DataSource = dt;
                gridPecasFunilaria.DataBind();
            }
        }

        protected void gridPecas_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Excluir")
            {
                if (txtStatus.Text != "Aberta")
                {
                    Mensagem("danger", "Status da Ordem de Serviço, não permite excluir peça.");
                    return;
                }
                int id = Convert.ToInt32(e.CommandArgument);

                using (SqlConnection conn = new SqlConnection(
                WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
                {
                    conn.Open();

                    string sql = @"

            DECLARE @id_peca INT
            DECLARE @quant INT

            SELECT 
            @id_peca = id_peca,
            @quant = quant
            FROM tbos_pecas
            WHERE id=@id

            DELETE FROM tbos_pecas
            WHERE id=@id

            UPDATE tbestoque_pecas
            SET estoque_peca = estoque_peca + @quant
            WHERE id_peca = @id_peca
            ";

                    SqlCommand cmd = new SqlCommand(sql, conn);

                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }
                divMsg.Visible = false;
                CarregarGrid();
                CarregarGridEletrica();
                CalcularTotais();
            }

            if (e.CommandName == "ExcluirEletrica")
            {
                if (txtStatus.Text != "Aberta")
                {
                    Mensagem("danger", "Status da Ordem de Serviço, não permite excluir peça.");
                    return;
                }
                int id = Convert.ToInt32(e.CommandArgument);

                using (SqlConnection conn = new SqlConnection(
                WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
                {
                    conn.Open();

                    string sql = @"

            DECLARE @id_peca INT
            DECLARE @quant INT

            SELECT 
            @id_peca = id_peca,
            @quant = quant
            FROM tbos_pecas
            WHERE id=@id

            DELETE FROM tbos_pecas
            WHERE id=@id

            UPDATE tbestoque_pecas
            SET estoque_peca = estoque_peca + @quant
            WHERE id_peca = @id_peca
            ";

                    SqlCommand cmd = new SqlCommand(sql, conn);

                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }
                divMsg.Visible = false;
                CarregarGridEletrica();
                CalcularTotais();
            }
        }
        protected void gridPecasEletrica_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

            if (e.CommandName == "ExcluirEletrica")
            {
                if (txtStatus.Text != "Aberta")
                {
                    Mensagem("danger", "Status da Ordem de Serviço, não permite excluir peça.");
                    return;
                }
                int id = Convert.ToInt32(e.CommandArgument);

                using (SqlConnection conn = new SqlConnection(
                WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
                {
                    conn.Open();

                    string sql = @"

            DECLARE @id_peca INT
            DECLARE @quant INT

            SELECT 
            @id_peca = id_peca,
            @quant = quant
            FROM tbos_pecas
            WHERE id=@id

            DELETE FROM tbos_pecas
            WHERE id=@id

            UPDATE tbestoque_pecas
            SET estoque_peca = estoque_peca + @quant
            WHERE id_peca = @id_peca
            ";

                    SqlCommand cmd = new SqlCommand(sql, conn);

                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }
                divMsg.Visible = false;
                CarregarGridEletrica();
                CalcularTotais();
            }
        }
        protected void gridPecasBorracharia_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

            if (e.CommandName == "ExcluirBorracharia")
            {
                if (txtStatus.Text != "Aberta")
                {
                    Mensagem("danger", "Status da Ordem de Serviço, não permite excluir peça.");
                    return;
                }
                int id = Convert.ToInt32(e.CommandArgument);

                using (SqlConnection conn = new SqlConnection(
                WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
                {
                    conn.Open();

                    string sql = @"

            DECLARE @id_peca INT
            DECLARE @quant INT

            SELECT 
            @id_peca = id_peca,
            @quant = quant
            FROM tbos_pecas
            WHERE id=@id

            DELETE FROM tbos_pecas
            WHERE id=@id

            UPDATE tbestoque_pecas
            SET estoque_peca = estoque_peca + @quant
            WHERE id_peca = @id_peca
            ";

                    SqlCommand cmd = new SqlCommand(sql, conn);

                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }
                divMsg.Visible = false;
                CarregarGridBorracharia();
                CalcularTotais();
            }
        }
        protected void gridPecasFunilaria_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

            if (e.CommandName == "ExcluirFunilaria")
            {
                if (txtStatus.Text != "Aberta")
                {
                    Mensagem("danger", "Status da Ordem de Serviço, não permite excluir peça.");
                    return;
                }
                int id = Convert.ToInt32(e.CommandArgument);

                using (SqlConnection conn = new SqlConnection(
                WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
                {
                    conn.Open();

                    string sql = @"

                    DECLARE @id_peca INT
                    DECLARE @quant INT

                    SELECT 
                    @id_peca = id_peca,
                    @quant = quant
                    FROM tbos_pecas
                    WHERE id=@id

                    DELETE FROM tbos_pecas
                    WHERE id=@id

                    UPDATE tbestoque_pecas
                    SET estoque_peca = estoque_peca + @quant
                    WHERE id_peca = @id_peca
                    ";

                    SqlCommand cmd = new SqlCommand(sql, conn);

                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }
                divMsg.Visible = false;
                CarregarGridFunilaria();
                CalcularTotais();
            }
        }

        decimal totalOS = 0;
        protected void CalcularTotais()
        {
            string sTipo = txtTipo.Text.ToString();
            using (SqlConnection conn = new SqlConnection(
WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                conn.Open();

                string sql = @"

                SELECT 
                ISNULL(SUM(tempo_minutos),0) tempo_total,
                ISNULL(SUM(quant),0) total_pecas,
                ISNULL(SUM(quant * valor_unitario),0) custo_total
                FROM tbos_pecas
                WHERE id_os=@os
                ";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@os", txtOS.Text);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    divMsg.Visible = false;
                    lblTempoTotal.Text = dr["tempo_total"].ToString() + " min";

                    lblTotalPecas.Text = dr["total_pecas"].ToString();

                    lblCustoTotal.Text =
                    Convert.ToDecimal(dr["custo_total"]).ToString("C");
                }
            }
        }
        protected void btnFinalizar_Click(object sender, EventArgs e)
        {
            int os = Convert.ToInt32(txtOS.Text);
            string responsavel_baixa = Session["UsuarioLogado"].ToString();

            using (SqlConnection conn = new SqlConnection(
            WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                conn.Open();

                string sql = @"UPDATE tbordem_servico
                       SET status = '2',
                       resp_baixa = @responsavel_baixa,
                       data_fechamento = GETDATE()
                       WHERE id_os = @os";

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@os", os);
                cmd.Parameters.AddWithValue("@responsavel_baixa", responsavel_baixa); // FALTAVA

                cmd.ExecuteNonQuery();
            }

            Response.Redirect("listaOS.aspx");
        }
        protected void btnFinalizarTroca_Click(object sender, EventArgs e)
        {
           
            if (ddlPeca.SelectedIndex == 0 ||
                string.IsNullOrWhiteSpace(txtQuant.Text) ||
                string.IsNullOrWhiteSpace(txtInicio.Text) ||
                string.IsNullOrWhiteSpace(txtTerm.Text) ||
                ddlMecanico.SelectedIndex == 0)
            {
                Mensagem("warning", "Preencha todos os campos para lançamento da peça.");
                return;
            }

            if (txtStatus.Text != "Aberta")
            {
                Mensagem("danger", "Status da Ordem de Serviço, não permite lançamento de peça.");
                return;
            }

            DateTime dataAbertura, dataInicio, dataFim;

            // 🔹 Validar Data de Abertura
            if (!DateTime.TryParseExact(txtAbertura.Text,
                "dd/MM/yyyy HH:mm",
                new CultureInfo("pt-BR"),
                DateTimeStyles.None,
                out dataAbertura))
            {
                Mensagem("info", "Informe uma data de abertura válida!");                
                return;
            }

            // 🔹 Validar Data de Início
            if (!DateTime.TryParseExact(txtInicio.Text,
                "dd/MM/yyyy HH:mm",
                new CultureInfo("pt-BR"),
                DateTimeStyles.None,
                out dataInicio))
            {
                Mensagem("info", "Informe uma data de início do serviço válida!");                
                return;
            }

            // 🔹 Validar Data Fim
            if (!DateTime.TryParseExact(txtTerm.Text,
                 "dd/MM/yyyy HH:mm",
                new CultureInfo("pt-BR"),
                DateTimeStyles.None,
                out dataFim))
            {
                Mensagem("info", "Informe uma data de termino do serviço válida!");                
                return;
            }

            // 🔹 Regra 1: Data início não pode ser menor que abertura
            if (dataInicio < dataAbertura)
            {
                Mensagem("info", "A data de início do serviço não pode ser menor que a data de abertura da OS!");                
                return;
            }

            // 🔹 Regra 2: Data fim não pode ser menor que início
            if (dataFim < dataInicio)
            {
                Mensagem("info", "A data de termino não pode ser menor que a data de início do serviço!");                
                return;
            }

            int os = Convert.ToInt32(txtOS.Text);
            int peca = Convert.ToInt32(ddlPeca.SelectedValue);
            string desc_peca = ddlPeca.SelectedItem.Text;
            int qtde = Convert.ToInt32(txtQuant.Text);
            int mecanico = Convert.ToInt32(ddlMecanico.SelectedValue);
            string nome_mecanico = ddlMecanico.SelectedItem.Text;
            string tipo = txtTipo.Text;

            DateTime inicio = Convert.ToDateTime(txtInicio.Text);
            DateTime termino = Convert.ToDateTime(txtTerm.Text);

            int tempo = Convert.ToInt32((termino - inicio).TotalMinutes);

            using (SqlConnection conn = new SqlConnection(
    WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                conn.Open();   // <<< obrigatório

                string sqlConsultaEstoque = "SELECT estoque_peca FROM tbestoque_pecas WHERE id_peca=@peca";

                SqlCommand cmdConsultaEstoque = new SqlCommand(sqlConsultaEstoque, conn);
                cmdConsultaEstoque.Parameters.AddWithValue("@peca", peca);

                int estoqueAtual = Convert.ToInt32(cmdConsultaEstoque.ExecuteScalar());
                conn.Close();
                if (qtde > estoqueAtual)
                {
                    Mensagem("warning", "Estoque insuficiente. Disponível: " + estoqueAtual);
                    return;
                }

                conn.Open();

                // busca valor da peça
                string sqlValor = "SELECT valor_unitario FROM tbestoque_pecas WHERE id_peca=@peca";

                SqlCommand cmdValor = new SqlCommand(sqlValor, conn);
                cmdValor.Parameters.AddWithValue("@peca", peca);

                decimal valor = Convert.ToDecimal(cmdValor.ExecuteScalar());

                decimal total = valor * qtde;

                // insere peça na OS
                string sqlInsert = @"INSERT INTO tbos_pecas
        (id_os,id_peca,descricao,quant,valor_unitario,valor_total,
        cracha,nome,inicio,termino,tempo_minutos,tipo)
        VALUES
        (@os,@peca,@descricao,@qtde,@valor,@total,@mecanico,@nome,@inicio,@termino,@tempo,@tipo)";

                SqlCommand cmd = new SqlCommand(sqlInsert, conn);

                cmd.Parameters.AddWithValue("@os", os);
                cmd.Parameters.AddWithValue("@peca", peca);
                cmd.Parameters.AddWithValue("@descricao", desc_peca);
                cmd.Parameters.AddWithValue("@qtde", qtde);
                cmd.Parameters.AddWithValue("@valor", valor);
                cmd.Parameters.AddWithValue("@total", total);
                cmd.Parameters.AddWithValue("@mecanico", mecanico);
                cmd.Parameters.AddWithValue("@nome", nome_mecanico);
                cmd.Parameters.AddWithValue("@inicio", inicio);
                cmd.Parameters.AddWithValue("@termino", termino);
                cmd.Parameters.AddWithValue("@tempo", tempo);
                cmd.Parameters.AddWithValue("@tipo", tipo);

                cmd.ExecuteNonQuery();

                // BAIXA ESTOQUE
                string sqlEstoque = @"UPDATE tbestoque_pecas
                              SET estoque_peca = estoque_peca - @qtde
                              WHERE id_peca = @peca";

                SqlCommand cmdEstoque = new SqlCommand(sqlEstoque, conn);

                cmdEstoque.Parameters.AddWithValue("@qtde", qtde);
                cmdEstoque.Parameters.AddWithValue("@peca", peca);

                cmdEstoque.ExecuteNonQuery();
            }

            ddlPeca.SelectedIndex = 0;
            txtQuant.Text = "";
            ddlMecanico.SelectedIndex = 0;
            txtInicio.Text = "";
            txtTerm.Text = "";

            CarregarGrid();
            CalcularTotais();
        }
        protected void btnTrocarEletrica_Click(object sender, EventArgs e)
        {
            if (ddlParteEletrica.SelectedIndex == 0 ||
                string.IsNullOrWhiteSpace(txtQuantEletrica.Text) ||
                string.IsNullOrWhiteSpace(txtInicioEletrica.Text) ||
                string.IsNullOrWhiteSpace(txtFimEletrica.Text) ||
                ddlEletricista.SelectedIndex == 0)
            {
                Mensagem("warning", "Preencha todos os campos para lançamento da peça.");
                return;
            }

            if (txtStatus.Text != "Aberta")
            {
                Mensagem("danger", "Status da Ordem de Serviço, não permite lançamento de peça.");
                return;
            }

            DateTime dataAbertura, dataInicio, dataFim;

            // 🔹 Validar Data de Abertura
            if (!DateTime.TryParseExact(txtAbertura.Text,
                "dd/MM/yyyy HH:mm",
                new CultureInfo("pt-BR"),
                DateTimeStyles.None,
                out dataAbertura))
            {
                Mensagem("info", "Informe uma data de abertura válida!");
                return;
            }

            // 🔹 Validar Data de Início
            if (!DateTime.TryParseExact(txtInicio.Text,
                "dd/MM/yyyy HH:mm",
                new CultureInfo("pt-BR"),
                DateTimeStyles.None,
                out dataInicio))
            {
                Mensagem("info", "Informe uma data de início do serviço válida!");
                return;
            }

            // 🔹 Validar Data Fim
            if (!DateTime.TryParseExact(txtTerm.Text,
                 "dd/MM/yyyy HH:mm",
                new CultureInfo("pt-BR"),
                DateTimeStyles.None,
                out dataFim))
            {
                Mensagem("info", "Informe uma data de termino do serviço válida!");
                return;
            }

            // 🔹 Regra 1: Data início não pode ser menor que abertura
            if (dataInicio < dataAbertura)
            {
                Mensagem("info", "A data de início do serviço não pode ser menor que a data de abertura da OS!");
                return;
            }

            // 🔹 Regra 2: Data fim não pode ser menor que início
            if (dataFim < dataInicio)
            {
                Mensagem("info", "A data de termino não pode ser menor que a data de início do serviço!");
                return;
            }


            int os = Convert.ToInt32(txtOS.Text);
            int peca = Convert.ToInt32(ddlParteEletrica.SelectedValue);
            string desc_peca = ddlParteEletrica.SelectedItem.Text;
            int qtde = Convert.ToInt32(txtQuantEletrica.Text);
            int mecanico = Convert.ToInt32(ddlEletricista.SelectedValue);
            string nome_mecanico = ddlEletricista.SelectedItem.Text;
            string tipo = txtTipoEletrica.Text;

            DateTime inicio = Convert.ToDateTime(txtInicioEletrica.Text);
            DateTime termino = Convert.ToDateTime(txtFimEletrica.Text);

            int tempo = Convert.ToInt32((termino - inicio).TotalMinutes);

            using (SqlConnection conn = new SqlConnection(
    WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                conn.Open();   // <<< obrigatório

                string sqlConsultaEstoque = "SELECT estoque_peca FROM tbestoque_pecas WHERE id_peca=@peca";

                SqlCommand cmdConsultaEstoque = new SqlCommand(sqlConsultaEstoque, conn);
                cmdConsultaEstoque.Parameters.AddWithValue("@peca", peca);

                int estoqueAtual = Convert.ToInt32(cmdConsultaEstoque.ExecuteScalar());
                conn.Close();
                if (qtde > estoqueAtual)
                {
                    Mensagem("warning", "Estoque insuficiente. Disponível: " + estoqueAtual);
                    return;
                }

                conn.Open();

                // busca valor da peça
                string sqlValor = "SELECT valor_unitario FROM tbestoque_pecas WHERE id_peca=@peca";

                SqlCommand cmdValor = new SqlCommand(sqlValor, conn);
                cmdValor.Parameters.AddWithValue("@peca", peca);

                decimal valor = Convert.ToDecimal(cmdValor.ExecuteScalar());

                decimal total = valor * qtde;

                // insere peça na OS
                string sqlInsert = @"INSERT INTO tbos_pecas
        (id_os,id_peca,descricao,quant,valor_unitario,valor_total,
        cracha,nome,inicio,termino,tempo_minutos,tipo)
        VALUES
        (@os,@peca,@descricao,@qtde,@valor,@total,@mecanico,@nome,@inicio,@termino,@tempo,@tipo)";

                SqlCommand cmd = new SqlCommand(sqlInsert, conn);

                cmd.Parameters.AddWithValue("@os", os);
                cmd.Parameters.AddWithValue("@peca", peca);
                cmd.Parameters.AddWithValue("@descricao", desc_peca);
                cmd.Parameters.AddWithValue("@qtde", qtde);
                cmd.Parameters.AddWithValue("@valor", valor);
                cmd.Parameters.AddWithValue("@total", total);
                cmd.Parameters.AddWithValue("@mecanico", mecanico);
                cmd.Parameters.AddWithValue("@nome", nome_mecanico);
                cmd.Parameters.AddWithValue("@inicio", inicio);
                cmd.Parameters.AddWithValue("@termino", termino);
                cmd.Parameters.AddWithValue("@tempo", tempo);
                cmd.Parameters.AddWithValue("@tipo", tipo);

                cmd.ExecuteNonQuery();

                // BAIXA ESTOQUE
                string sqlEstoque = @"UPDATE tbestoque_pecas
                              SET estoque_peca = estoque_peca - @qtde
                              WHERE id_peca = @peca";

                SqlCommand cmdEstoque = new SqlCommand(sqlEstoque, conn);

                cmdEstoque.Parameters.AddWithValue("@qtde", qtde);
                cmdEstoque.Parameters.AddWithValue("@peca", peca);

                cmdEstoque.ExecuteNonQuery();
            }

            ddlParteEletrica.SelectedIndex = 0;
            txtQuantEletrica.Text = "";
            ddlEletricista.SelectedIndex = 0;
            txtInicioEletrica.Text = "";
            txtFimEletrica.Text = "";

            CarregarGridEletrica();
            CalcularTotais();
        }
        protected void btnTrocarBorracharia_Click(object sender, EventArgs e)
        {
            if (ddlParteBorracharia.SelectedIndex == 0 ||
                string.IsNullOrWhiteSpace(txtQuantBorracharia.Text) ||
                string.IsNullOrWhiteSpace(txtInicioBorracharia.Text) ||
                string.IsNullOrWhiteSpace(txtFimBorracharia.Text) ||
                ddlBorracheiro.SelectedIndex == 0)
            {
                Mensagem("warning", "Preencha todos os campos para lançamento da peça.");
                return;
            }

            if (txtStatus.Text != "Aberta")
            {
                Mensagem("danger", "Status da Ordem de Serviço, não permite lançamento de peça.");
                return;
            }
            DateTime dataAbertura, dataInicio, dataFim;

            // 🔹 Validar Data de Abertura
            if (!DateTime.TryParseExact(txtAbertura.Text,
                "dd/MM/yyyy HH:mm",
                new CultureInfo("pt-BR"),
                DateTimeStyles.None,
                out dataAbertura))
            {
                Mensagem("info", "Informe uma data de abertura válida!");
                return;
            }

            // 🔹 Validar Data de Início
            if (!DateTime.TryParseExact(txtInicio.Text,
                "dd/MM/yyyy HH:mm",
                new CultureInfo("pt-BR"),
                DateTimeStyles.None,
                out dataInicio))
            {
                Mensagem("info", "Informe uma data de início do serviço válida!");
                return;
            }

            // 🔹 Validar Data Fim
            if (!DateTime.TryParseExact(txtTerm.Text,
                 "dd/MM/yyyy HH:mm",
                new CultureInfo("pt-BR"),
                DateTimeStyles.None,
                out dataFim))
            {
                Mensagem("info", "Informe uma data de termino do serviço válida!");
                return;
            }

            // 🔹 Regra 1: Data início não pode ser menor que abertura
            if (dataInicio < dataAbertura)
            {
                Mensagem("info", "A data de início do serviço não pode ser menor que a data de abertura da OS!");
                return;
            }

            // 🔹 Regra 2: Data fim não pode ser menor que início
            if (dataFim < dataInicio)
            {
                Mensagem("info", "A data de termino não pode ser menor que a data de início do serviço!");
                return;
            }


            int os = Convert.ToInt32(txtOS.Text);
            int peca = Convert.ToInt32(ddlParteBorracharia.SelectedValue);
            string desc_peca = ddlParteBorracharia.SelectedItem.Text;
            int qtde = Convert.ToInt32(txtQuantBorracharia.Text);
            int mecanico = Convert.ToInt32(ddlBorracheiro.SelectedValue);
            string nome_mecanico = ddlBorracheiro.SelectedItem.Text;
            string tipo = txtTipoBorracharia.Text;
            
            // Informações do pneu
            string retirada_instalacao = ddlSituacao.SelectedItem.Text;
            int num_pneu = Convert.ToInt32(ddlNumeroPneu.SelectedValue);
            string destino_pneu = ddlDestino.SelectedItem.Text;
            string posicao_pneu = ddlPosicao.SelectedItem.Text;
            int km_inicial_pneu = Convert.ToInt32(txtKmInicial.Text);
            int km_final_pneu = Convert.ToInt32(txtKMFinal.Text);

            DateTime inicio = Convert.ToDateTime(txtInicioBorracharia.Text);
            DateTime termino = Convert.ToDateTime(txtFimBorracharia.Text);

            int tempo = Convert.ToInt32((termino - inicio).TotalMinutes);

            using (SqlConnection conn = new SqlConnection(
    WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                conn.Open();  

                string sqlConsultaEstoque = "SELECT estoque_peca FROM tbestoque_pecas WHERE id_peca=@peca";

                SqlCommand cmdConsultaEstoque = new SqlCommand(sqlConsultaEstoque, conn);
                cmdConsultaEstoque.Parameters.AddWithValue("@peca", peca);

                int estoqueAtual = Convert.ToInt32(cmdConsultaEstoque.ExecuteScalar());
                conn.Close();
                if (qtde > estoqueAtual)
                {
                    Mensagem("warning", "Estoque insuficiente. Disponível: " + estoqueAtual);
                    return;
                }

                conn.Open();

                // busca valor da peça
                string sqlValor = "SELECT valor_unitario FROM tbestoque_pecas WHERE id_peca=@peca";

                SqlCommand cmdValor = new SqlCommand(sqlValor, conn);
                cmdValor.Parameters.AddWithValue("@peca", peca);

                decimal valor = Convert.ToDecimal(cmdValor.ExecuteScalar());

                decimal total = valor * qtde;

                // insere peça na OS
                string sqlInsert = @"INSERT INTO tbos_pecas
        (id_os,id_peca,descricao,quant,valor_unitario,valor_total,
        cracha,nome,inicio,termino,tempo_minutos,tipo,retirada_instalacao,num_pneu,destino_pneu,posicao_pneu,km_inicial_pneu,km_final_pneu)
        VALUES
        (@os,@peca,@descricao,@qtde,@valor,@total,@mecanico,@nome,@inicio,@termino,@tempo,@tipo,@retirada_instalacao,@num_pneu,@destino_pneu,@posicao_pneu,@km_inicial_pneu,@km_final_pneu)";

                SqlCommand cmd = new SqlCommand(sqlInsert, conn);

                cmd.Parameters.AddWithValue("@os", os);
                cmd.Parameters.AddWithValue("@peca", peca);
                cmd.Parameters.AddWithValue("@descricao", desc_peca);
                cmd.Parameters.AddWithValue("@qtde", qtde);
                cmd.Parameters.AddWithValue("@valor", valor);
                cmd.Parameters.AddWithValue("@total", total);
                cmd.Parameters.AddWithValue("@mecanico", mecanico);
                cmd.Parameters.AddWithValue("@nome", nome_mecanico);
                cmd.Parameters.AddWithValue("@inicio", inicio);
                cmd.Parameters.AddWithValue("@termino", termino);
                cmd.Parameters.AddWithValue("@tempo", tempo);
                cmd.Parameters.AddWithValue("@tipo", tipo);
                cmd.Parameters.AddWithValue("@retirada_instalacao", retirada_instalacao);
                cmd.Parameters.AddWithValue("@num_pneu", num_pneu);
                cmd.Parameters.AddWithValue("@destino_pneu", destino_pneu);
                cmd.Parameters.AddWithValue("@posicao_pneu", posicao_pneu);
                cmd.Parameters.AddWithValue("@km_inicial_pneu", km_inicial_pneu);
                cmd.Parameters.AddWithValue("@km_final_pneu", km_final_pneu);   
                cmd.ExecuteNonQuery();

                // BAIXA ESTOQUE
                string sqlEstoque = @"UPDATE tbestoque_pecas
                              SET estoque_peca = estoque_peca - @qtde
                              WHERE id_peca = @peca";

                SqlCommand cmdEstoque = new SqlCommand(sqlEstoque, conn);

                cmdEstoque.Parameters.AddWithValue("@qtde", qtde);
                cmdEstoque.Parameters.AddWithValue("@peca", peca);

                cmdEstoque.ExecuteNonQuery();

                // ATUALIZA PNEU
                string sqlPneu = @"UPDATE tbpneus
                SET placa=@placa, DataInstalacao=GETDATE(), ordem_servico=@ordemServico, Status=@status, KmAtual=@kmAtual, posicao=@posicao 
                WHERE numero = @num_pneu";

                SqlCommand cmdPneu = new SqlCommand(sqlPneu, conn);

                cmdPneu.Parameters.AddWithValue("@placa", txtPlaca.Text.Trim());
                cmdPneu.Parameters.AddWithValue("@ordemServico", os);
                cmdPneu.Parameters.AddWithValue("@status", destino_pneu);
                cmdPneu.Parameters.AddWithValue("@KmAtual", km_inicial_pneu);
                cmdPneu.Parameters.AddWithValue("@posicao", posicao_pneu);
                cmdPneu.ExecuteNonQuery();

                // INSERE A MOVIMENTAÇÃO
                string sqlMovimentacao = @"INSERT INTO tbmovimentacaopneu                (Num_pneu,IdVeiculo,Placa,Posicao,TipoMovimentacao,retirada_instalacao,KM,Km_final,DataMovimentacao,OrdemServico)
                  VALUES
                  (@num_pneu,@IdVeiculo,@Placa,@Posicao,@TipoMovimentacao,@retirada_instalacao,@KM,@Km_final,GETDATE(),@OrdemServico)";

                SqlCommand cmdMovimentacao = new SqlCommand(sqlMovimentacao, conn);
                cmdMovimentacao.Parameters.AddWithValue("@num_pneu", num_pneu);
                cmdMovimentacao.Parameters.AddWithValue("@IdVeiculo", txtCodVeiculo.Text.Trim());
                cmdMovimentacao.Parameters.AddWithValue("@Placa", txtPlaca.Text.Trim());
                cmdMovimentacao.Parameters.AddWithValue("@Posicao", posicao_pneu);
                cmdMovimentacao.Parameters.AddWithValue("@TipoMovimentacao", destino_pneu);
                cmdMovimentacao.Parameters.AddWithValue("@retirada_instalacao", retirada_instalacao);
                cmdMovimentacao.Parameters.AddWithValue("@km", km_inicial_pneu);
                cmdMovimentacao.Parameters.AddWithValue("@km_final", km_final_pneu);
                cmdMovimentacao.Parameters.AddWithValue("@OrdemServico", os);
                cmdMovimentacao.ExecuteNonQuery();
            }

            ddlParteBorracharia.SelectedIndex = 0;
            txtQuantBorracharia.Text = "";
            ddlBorracheiro.SelectedIndex = 0;
            txtInicioBorracharia.Text = "";
            txtFimBorracharia.Text = "";

            CarregarGridBorracharia();
            CalcularTotais();
        }
        protected void btnTrocarFunilaria_Click(object sender, EventArgs e)
        {
            if (ddlParteFunilaria.SelectedIndex == 0 ||
                string.IsNullOrWhiteSpace(txtQuantFunilaria.Text) ||
                string.IsNullOrWhiteSpace(txtInicioFunilaria.Text) ||
                string.IsNullOrWhiteSpace(txtFimFunilaria.Text) ||
                ddlFunileiro.SelectedIndex == 0)
            {
                Mensagem("warning", "Preencha todos os campos para lançamento da peça.");
                return;
            }

            if (txtStatus.Text != "Aberta")
            {
                Mensagem("danger", "Status da Ordem de Serviço, não permite lançamento de peça.");
                return;
            }
            DateTime dataAbertura, dataInicio, dataFim;

            // 🔹 Validar Data de Abertura
            if (!DateTime.TryParseExact(txtAbertura.Text,
                "dd/MM/yyyy HH:mm",
                new CultureInfo("pt-BR"),
                DateTimeStyles.None,
                out dataAbertura))
            {
                Mensagem("info", "Informe uma data de abertura válida!");
                return;
            }

            // 🔹 Validar Data de Início
            if (!DateTime.TryParseExact(txtInicio.Text,
                "dd/MM/yyyy HH:mm",
                new CultureInfo("pt-BR"),
                DateTimeStyles.None,
                out dataInicio))
            {
                Mensagem("info", "Informe uma data de início do serviço válida!");
                return;
            }

            // 🔹 Validar Data Fim
            if (!DateTime.TryParseExact(txtTerm.Text,
                 "dd/MM/yyyy HH:mm",
                new CultureInfo("pt-BR"),
                DateTimeStyles.None,
                out dataFim))
            {
                Mensagem("info", "Informe uma data de termino do serviço válida!");
                return;
            }

            // 🔹 Regra 1: Data início não pode ser menor que abertura
            if (dataInicio < dataAbertura)
            {
                Mensagem("info", "A data de início do serviço não pode ser menor que a data de abertura da OS!");
                return;
            }

            // 🔹 Regra 2: Data fim não pode ser menor que início
            if (dataFim < dataInicio)
            {
                Mensagem("info", "A data de termino não pode ser menor que a data de início do serviço!");
                return;
            }


            int os = Convert.ToInt32(txtOS.Text);
            int peca = Convert.ToInt32(ddlParteFunilaria.SelectedValue);
            string desc_peca = ddlParteFunilaria.SelectedItem.Text;
            int qtde = Convert.ToInt32(txtQuantFunilaria.Text);
            int mecanico = Convert.ToInt32(ddlFunileiro.SelectedValue);
            string nome_mecanico = ddlFunileiro.SelectedItem.Text;
            string tipo = txtTipoFunilaria.Text;

            DateTime inicio = Convert.ToDateTime(txtInicioFunilaria.Text);
            DateTime termino = Convert.ToDateTime(txtFimFunilaria.Text);

            int tempo = Convert.ToInt32((termino - inicio).TotalMinutes);

            using (SqlConnection conn = new SqlConnection(
    WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                conn.Open();   // <<< obrigatório

                string sqlConsultaEstoque = "SELECT estoque_peca FROM tbestoque_pecas WHERE id_peca=@peca";

                SqlCommand cmdConsultaEstoque = new SqlCommand(sqlConsultaEstoque, conn);
                cmdConsultaEstoque.Parameters.AddWithValue("@peca", peca);

                int estoqueAtual = Convert.ToInt32(cmdConsultaEstoque.ExecuteScalar());
                conn.Close();
                if (qtde > estoqueAtual)
                {
                    Mensagem("warning", "Estoque insuficiente. Disponível: " + estoqueAtual);
                    return;
                }

                conn.Open();

                // busca valor da peça
                string sqlValor = "SELECT valor_unitario FROM tbestoque_pecas WHERE id_peca=@peca";

                SqlCommand cmdValor = new SqlCommand(sqlValor, conn);
                cmdValor.Parameters.AddWithValue("@peca", peca);

                decimal valor = Convert.ToDecimal(cmdValor.ExecuteScalar());

                decimal total = valor * qtde;

                // insere peça na OS
                string sqlInsert = @"INSERT INTO tbos_pecas
        (id_os,id_peca,descricao,quant,valor_unitario,valor_total,
        cracha,nome,inicio,termino,tempo_minutos,tipo)
        VALUES
        (@os,@peca,@descricao,@qtde,@valor,@total,@mecanico,@nome,@inicio,@termino,@tempo,@tipo)";

                SqlCommand cmd = new SqlCommand(sqlInsert, conn);

                cmd.Parameters.AddWithValue("@os", os);
                cmd.Parameters.AddWithValue("@peca", peca);
                cmd.Parameters.AddWithValue("@descricao", desc_peca);
                cmd.Parameters.AddWithValue("@qtde", qtde);
                cmd.Parameters.AddWithValue("@valor", valor);
                cmd.Parameters.AddWithValue("@total", total);
                cmd.Parameters.AddWithValue("@mecanico", mecanico);
                cmd.Parameters.AddWithValue("@nome", nome_mecanico);
                cmd.Parameters.AddWithValue("@inicio", inicio);
                cmd.Parameters.AddWithValue("@termino", termino);
                cmd.Parameters.AddWithValue("@tempo", tempo);
                cmd.Parameters.AddWithValue("@tipo", tipo);

                cmd.ExecuteNonQuery();

                // BAIXA ESTOQUE
                string sqlEstoque = @"UPDATE tbestoque_pecas
                              SET estoque_peca = estoque_peca - @qtde
                              WHERE id_peca = @peca";

                SqlCommand cmdEstoque = new SqlCommand(sqlEstoque, conn);

                cmdEstoque.Parameters.AddWithValue("@qtde", qtde);
                cmdEstoque.Parameters.AddWithValue("@peca", peca);

                cmdEstoque.ExecuteNonQuery();
            }

            ddlParteFunilaria.SelectedIndex = 0;
            txtQuantFunilaria.Text = "";
            ddlFunileiro.SelectedIndex = 0;
            txtInicioFunilaria.Text = "";
            txtFimFunilaria.Text = "";

            CarregarGridFunilaria();
            CalcularTotais();
        }

        [System.Web.Services.WebMethod]
        public static List<string> BuscarPneus(string placa, string situacao)
        {
            List<string> lista = new List<string>();

            using (SqlConnection conn = new SqlConnection(
                WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                conn.Open();

                string sql = "";

                if (situacao == "Retirada")
                {
                    sql = "SELECT numero FROM tbpneus WHERE placa = @placa";
                }
                else if (situacao == "Instalação")
                {
                    sql = "SELECT numero FROM tbpneus WHERE status = 'Estoque' AND numero <> ''";
                }

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@placa", placa ?? "");

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    lista.Add(dr["numero"].ToString());
                }
            }

            return lista;
        }
        protected void AtualizarNumeroPneu2()
        {
            string tipoServico = ddlTipoServico.SelectedValue;
            string situacao = ddlSituacao.SelectedValue;
            string placa = txtPlaca.Text.Trim();

            if (tipoServico == "Pneu" && situacao == "Retirada")
            {
                using (SqlConnection conn = new SqlConnection(
     WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
                {
                    string query = @"SELECT *
                             FROM tbpneus 
                             WHERE placa = @placa";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@placa", placa);

                    conn.Open();

                    SqlDataReader reader = cmd.ExecuteReader();

                    ddlNumeroPneu.Items.Clear();
                    ddlNumeroPneu.Items.Add(new System.Web.UI.WebControls.ListItem("Selecione", ""));

                    while (reader.Read())
                    {
                        ddlNumeroPneu.Items.Add(new System.Web.UI.WebControls.ListItem(
                            reader["numero"].ToString(),
                            reader["numero"].ToString()
                        ));
                    }
                }
            }
            else if (tipoServico == "Pneu" && situacao == "Instalação")
            {       
                using (SqlConnection conn = new SqlConnection(
     WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
                {
                    string query = @"SELECT * 
                             FROM tbpneus 
                             WHERE status = 'Estoque' AND numero <> ''";

                    SqlCommand cmd = new SqlCommand(query, conn);                 

                    conn.Open();

                    SqlDataReader reader = cmd.ExecuteReader();

                    ddlNumeroPneu.Items.Clear();
                    // Primeiro item (placeholder)
                    ddlNumeroPneu.Items.Add(new System.Web.UI.WebControls.ListItem("Selecione...", ""));

                    while (reader.Read())
                    {
                        System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem();
                        item.Text = reader["numero"].ToString();
                        item.Value = reader["numero"].ToString();

                        // atributos personalizados
                        item.Attributes["data-parte"] = reader["id_peca"].ToString();
                        item.Attributes["data-status"] = reader["status"].ToString();
                        item.Attributes["data-km"] = reader["kmatual"].ToString();
                        
                        ddlNumeroPneu.Items.Add(item);
                    }
                    txtStatus.ReadOnly = true;
                    txtKMFinal.ReadOnly = true;
                    txtQuantBorracharia.Text = "1";
                    txtQuantBorracharia.ReadOnly = true;

                    //ddlNumeroPneu.Items.Clear();
                    //ddlNumeroPneu.Items.Add(new System.Web.UI.WebControls.ListItem("Selecione", ""));

                    //while (reader.Read())
                    //{
                    //    ddlNumeroPneu.Items.Add(new System.Web.UI.WebControls.ListItem(
                    //        reader["numero"].ToString(),
                    //        reader["numero"].ToString()                           
                    //    ));
                    //    ddlParteBorracharia.SelectedItem.Text = reader["descricao"].ToString();
                    //    txtStatusPneu.Text = reader["status"].ToString();
                    //    txtKmInicial.Text = reader["kmatual"].ToString();
                    //}
                }
            }
            else
            {
                ddlNumeroPneu.Items.Clear();
                ddlNumeroPneu.Items.Add(new System.Web.UI.WebControls.ListItem("Selecione", ""));
            }
        }
        protected void AtualizarNumeroPneu()
        {
            string tipoServico = ddlTipoServico.SelectedValue;
            string situacao = ddlSituacao.SelectedValue;
            string placa = txtPlaca.Text.Trim();

            if (tipoServico == "Pneu" && situacao == "Retirada")
            {               
                    string query = @"SELECT * FROM tbpneus WHERE placa = @placa";
                    
                    // Crie uma conexão com o banco de dados
                    using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
                    {
                        try
                        {
                            // Abra a conexão com o banco de dados
                            conn.Open();

                            SqlCommand cmd = new SqlCommand(query, conn);
                            cmd.Parameters.AddWithValue("@placa", placa);
                            // Crie o comando SQL                          

                            // Execute o comando e obtenha os dados em um DataReader
                            SqlDataReader reader = cmd.ExecuteReader();

                            // Preencher o ComboBox com os dados do DataReader
                            ddlNumeroPneu.DataSource = reader;
                            ddlNumeroPneu.DataTextField = "numero";  // Campo que será mostrado no ComboBox
                            ddlNumeroPneu.DataValueField = "numero";  // Campo que será o valor de cada item                    
                            ddlNumeroPneu.DataBind();  // Realiza o binding dos dados                   
                            ddlNumeroPneu.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Selecione...", "0"));
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
            else if (tipoServico == "Pneu" && situacao == "Instalação")
            {
                string query = @"SELECT * FROM tbpneus WHERE status = 'Estoque' AND numero <> ''";

                // Crie uma conexão com o banco de dados
                using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
                {
                    try
                    {
                        // Abra a conexão com o banco de dados
                        conn.Open();

                        SqlCommand cmd = new SqlCommand(query, conn);
                        //cmd.Parameters.AddWithValue("@placa", placa);
                        // Crie o comando SQL                          

                        // Execute o comando e obtenha os dados em um DataReader
                        SqlDataReader reader = cmd.ExecuteReader();

                        // Preencher o ComboBox com os dados do DataReader
                        ddlNumeroPneu.DataSource = reader;
                        ddlNumeroPneu.DataTextField = "numero";  // Campo que será mostrado no ComboBox
                        ddlNumeroPneu.DataValueField = "numero";  // Campo que será o valor de cada item                    
                        ddlNumeroPneu.DataBind();  // Realiza o binding dos dados                   
                        ddlNumeroPneu.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Selecione...", "0"));
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


        }
        protected void ddlSituacao_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            AtualizarNumeroPneu();
            PreencherDestino();
        }
        public void PreencherDestino()
        {
            ddlDestino.Items.Clear();
            ddlDestino.Items.Add(new System.Web.UI.WebControls.ListItem("Selecione...", ""));
            
            string situacao = ddlSituacao.SelectedValue;
            if (situacao == "Instalação")
            {
                ddlPosicao.Items.Add(new System.Web.UI.WebControls.ListItem("Em Uso"));   
            }
            else
            {
                ddlPosicao.Items.Add(new System.Web.UI.WebControls.ListItem("Estoque"));
                ddlPosicao.Items.Add(new System.Web.UI.WebControls.ListItem("Conserto"));
                ddlPosicao.Items.Add(new System.Web.UI.WebControls.ListItem("Reforma"));
                ddlPosicao.Items.Add(new System.Web.UI.WebControls.ListItem("Descarte"));  
            }
        }
        public void PreencherPosicao()
        {
            ddlPosicao.Items.Clear();
            ddlPosicao.Items.Add(new System.Web.UI.WebControls.ListItem("Selecione...", ""));

            string tipoVei = txtTipVei.Text.Trim().ToUpper();

            if (tipoVei == "CARRETA")
            {
                ddlPosicao.Items.Add(new System.Web.UI.WebControls.ListItem("Carreta 1º Eixo LED"));
                ddlPosicao.Items.Add(new System.Web.UI.WebControls.ListItem("Carreta 1º Eixo LEF"));
                ddlPosicao.Items.Add(new System.Web.UI.WebControls.ListItem("Carreta 2º Eixo LED"));
                ddlPosicao.Items.Add(new System.Web.UI.WebControls.ListItem("Carreta 2º Eixo LEF"));
                ddlPosicao.Items.Add(new System.Web.UI.WebControls.ListItem("Carreta 3º Eixo LED"));
                ddlPosicao.Items.Add(new System.Web.UI.WebControls.ListItem("Carreta 3º Eixo LEF"));
                ddlPosicao.Items.Add(new System.Web.UI.WebControls.ListItem("Carreta 1º Eixo LDD"));
                ddlPosicao.Items.Add(new System.Web.UI.WebControls.ListItem("Carreta 1º Eixo LDF"));
                ddlPosicao.Items.Add(new System.Web.UI.WebControls.ListItem("Carreta 2º Eixo LDD"));
                ddlPosicao.Items.Add(new System.Web.UI.WebControls.ListItem("Carreta 2º Eixo LDF"));
                ddlPosicao.Items.Add(new System.Web.UI.WebControls.ListItem("Carreta 3º Eixo LDD"));
                ddlPosicao.Items.Add(new System.Web.UI.WebControls.ListItem("Carreta 3º Eixo LDF"));
            }
            else
            {
                ddlPosicao.Items.Add(new System.Web.UI.WebControls.ListItem("Dianteiro Esquerdo"));
                ddlPosicao.Items.Add(new System.Web.UI.WebControls.ListItem("Dianteiro Direito"));
                ddlPosicao.Items.Add(new System.Web.UI.WebControls.ListItem("Traseiro Esquerdo Dentro"));
                ddlPosicao.Items.Add(new System.Web.UI.WebControls.ListItem("Traseiro Esquerdo Fora"));
                ddlPosicao.Items.Add(new System.Web.UI.WebControls.ListItem("Traseiro Direito Dentro"));
                ddlPosicao.Items.Add(new System.Web.UI.WebControls.ListItem("Traseiro Direito Fora"));
                ddlPosicao.Items.Add(new System.Web.UI.WebControls.ListItem("Tração Esquerdo Dentro"));
                ddlPosicao.Items.Add(new System.Web.UI.WebControls.ListItem("Tração Esquerdo Fora"));
                ddlPosicao.Items.Add(new System.Web.UI.WebControls.ListItem("Tração Direito Dentro"));
                ddlPosicao.Items.Add(new System.Web.UI.WebControls.ListItem("Tração Direito Fora"));               
            }            
        }

        protected void ddlDestino_SelectedIndexChanged(object sender, EventArgs e)
        {
            PreencherDestino();
        }

        //   protected void ddlNumeroPneu_SelectedIndexChanged(object sender, EventArgs e)
        //   {
        //       string tipoServico = ddlTipoServico.SelectedValue;
        //       string situacao = ddlSituacao.SelectedValue;
        //       string placa = txtPlaca.Text.Trim();
        //       string numero = ddlNumeroPneu.SelectedValue;

        //       using (SqlConnection conn = new SqlConnection(
        //WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
        //       {
        //           string query = @"SELECT * FROM tbpneus WHERE numero = @numero";

        //           SqlCommand cmd = new SqlCommand(query, conn);
        //           cmd.Parameters.AddWithValue("@numero", numero);
        //           conn.Open();
        //           SqlDataReader reader = cmd.ExecuteReader();
        //           while (reader.Read())
        //           {
        //               if (tipoServico == "Pneu" && situacao == "Retirada")
        //               {
        //                   // ddlParteBorracharia.SelectedItem.Text = reader["descricao"].ToString(); 
        //                   // ddlDestino.SelectedItem.Text = reader["status"].ToString();
        //                   // ddlPosicao.SelectedItem.Text = reader["posicao"].ToString();
        //                   //CarregarPecas();
        //                   ddlParteBorracharia.SelectedValue = reader["descricao"].ToString();
        //                   txtStatus.Text = reader["status"].ToString();
        //                   PreencherDestino();
        //                   ddlDestino.SelectedValue = reader["status"].ToString();
        //                   txtKmInicial.Text = reader["kmatual"].ToString();
        //                   PreencherPosicao();
        //                   ddlPosicao.SelectedValue = reader["posicao"].ToString();

        //               }
        //               else if (tipoServico == "Pneu" && situacao == "Instalação")
        //               {
        //                   //ddlParteBorracharia.SelectedItem.Text = reader["descricao"].ToString();
        //                   //CarregarPecas();
        //                   ddlParteBorracharia.SelectedValue = reader["descricao"].ToString();
        //                   txtStatus.Text = reader["status"].ToString();
        //               }
        //           }
        //       }
        //   }
        protected void ddlNumeroPneu_SelectedIndexChanged(object sender, EventArgs e)
        {
            string tipoServico = ddlTipoServico.SelectedValue;
            string situacao = ddlSituacao.SelectedValue;
            string numero = ddlNumeroPneu.SelectedValue;

            using (SqlConnection conn = new SqlConnection(
                WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                string query = "SELECT * FROM tbpneus WHERE numero = @numero";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@numero", numero);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    string descricao = reader["descricao"].ToString();
                    string status = reader["status"].ToString();
                    string km = reader["kmatual"].ToString();
                    string posicao = reader["posicao"].ToString();

                    // 🔥 PRIMEIRO carrega os dropdowns
                    PreencherDestino();
                    PreencherPosicao();
                    //CarregarPecas(); // se necessário

                    if (tipoServico == "Pneu" && situacao == "Retirada")
                    {
                        if (ddlParteBorracharia.Items.FindByValue(descricao) != null)
                            ddlParteBorracharia.SelectedValue = descricao;

                        txtStatus.Text = status;

                        if (ddlDestino.Items.FindByValue(status) != null)
                            ddlDestino.SelectedValue = status;

                        txtKmInicial.Text = km;

                        if (ddlPosicao.Items.FindByValue(posicao) != null)
                            ddlPosicao.SelectedValue = posicao;
                    }
                    else if (tipoServico == "Pneu" && situacao == "Instalação")
                    {
                        if (ddlParteBorracharia.Items.FindByValue(descricao) != null)
                            ddlParteBorracharia.SelectedValue = descricao;

                        txtStatus.Text = status;
                    }
                }
            }
        }
    }
}