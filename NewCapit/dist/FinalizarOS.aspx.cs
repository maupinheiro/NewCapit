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
    public partial class FinalizarOS : System.Web.UI.Page
    {
        SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString());
        string nomeUsuario = null;
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
                CarregarPecas();
                CarregarMecanicos();
                CalcularTotais();
                

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

                    if(dr["status"].ToString() == "1")
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
                        lblStatus.Text = "Finalizada" ;
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
                ddlPeca.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Selecione", ""));

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
                string sql = "SELECT cracha, nome FROM tbprofissional_manutencao";

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

            Mensagem("success", "OS finalizada com sucesso.");
            LimparCampos();
            txtOS.Focus();
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

            DateTime inicio = Convert.ToDateTime(txtInicioBorracharia.Text);
            DateTime termino = Convert.ToDateTime(txtFimBorracharia.Text);

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
    }
}