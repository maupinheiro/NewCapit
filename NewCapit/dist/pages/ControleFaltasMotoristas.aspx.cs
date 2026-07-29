using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Domain;
using System.Data.SqlClient;
using System.Text;
using DAL;
using Microsoft.Extensions.Primitives;
using System.Web.Configuration;

namespace NewCapit.dist.pages
{
    public partial class ControleFaltasMotoristas : System.Web.UI.Page
    {
        private readonly ControleFaltasDAL controleDAL = new ControleFaltasDAL();
        private readonly MotoristaDAL motoristaDAL = new MotoristaDAL();
        private readonly string conexao = WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
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
                txtData.Text = DateTime.Today.ToString("yyyy-MM-dd");
                txtDataIni.Text = DateTime.Today.AddDays(-30).ToString("yyyy-MM-dd");
                txtDataFim.Text = DateTime.Today.ToString("yyyy-MM-dd");
                txtQuantidade.Text = "1";
                CarregarMotoristas();
                CarregarNucleos();
                CarregarGrid();
            }
        }
        private void CarregarMotoristas()
        {
            DataTable dt = motoristaDAL.ListarMotoristas();

            ddlMotorista.DataSource = dt;
            ddlMotorista.DataTextField = "Motorista";
            ddlMotorista.DataValueField = "codmot";
            ddlMotorista.DataBind();

            ddlMotorista.Items.Insert(0,
                new ListItem("Selecione...", ""));

            ddlFiltroMotorista.DataSource = dt;
            ddlFiltroMotorista.DataTextField = "Motorista";
            ddlFiltroMotorista.DataValueField = "codmot";
            ddlFiltroMotorista.DataBind();

            ddlFiltroMotorista.Items.Insert(0,
                new ListItem("Todos", ""));
        }
        private void CarregarGrid()
        {
            DateTime dataInicial = Convert.ToDateTime(txtDataIni.Text);
            DateTime dataFinal = Convert.ToDateTime(txtDataFim.Text);

            string codmot = "0";

            if (!string.IsNullOrWhiteSpace(ddlFiltroMotorista.SelectedValue))
                codmot = ddlFiltroMotorista.SelectedValue;

            string nucleo = "";

            if (!string.IsNullOrWhiteSpace(ddlFiltroNucleo.SelectedValue))
                nucleo = ddlFiltroNucleo.SelectedValue;

            DataTable dt = controleDAL.Listar(
                dataInicial,
                dataFinal,
                codmot,
                nucleo);

            gvFaltas.DataSource = dt;
            gvFaltas.DataBind();

            CarregarIndicadores(dt);
        }
        private void CarregarIndicadores(DataTable dt)
        {
            lblTotalOcorrencias.Text =
                dt.Rows.Count.ToString();

            int faltas = 0;
            int atestados = 0;
            int ferias = 0;
            int dsr = 0;

            foreach (DataRow row in dt.Rows)
            {
                string motivo =
                    row["motivo"].ToString();

                switch (motivo)
                {
                    case "Falta":

                        faltas++;

                        break;

                    case "Atestado":

                        atestados++;

                        break;

                    case "Férias":

                        ferias++;

                        break;

                    case "DSR":

                        dsr++;

                        break;
                }
            }

            lblFaltas.Text =
                faltas.ToString();

            lblAtestados.Text =
                atestados.ToString();

            lblFerias.Text =
                ferias.ToString();

            lblDSR.Text =
                dsr.ToString();
        }
        private void LimparCadastro()
        {
            txtCodMot.Text = "";

            ddlMotorista.SelectedIndex = 0;

            txtFuncao.Text = "";

            txtNucleo.Text = "";

            txtQuantidade.Text = "1";

            txtObservacao.Text = "";

            txtData.Text =
                DateTime.Today.ToString("yyyy-MM-dd");

            ddlMotivo.SelectedIndex = 0;
        }
        private void MostrarMensagem(string mensagem,bool sucesso)
        {
            lblMensagem.Visible = true;


            lblMensagem.Text = mensagem;


            if (sucesso)
            {
                lblMensagem.CssClass =
                    "alert alert-success alert-dismissible fade show";
            }
            else
            {
                lblMensagem.CssClass =
                    "alert alert-danger alert-dismissible fade show";
            }


            ScriptManager.RegisterStartupScript(
                this,
                GetType(),
                "Mensagem",
                "window.scrollTo(0,0);",
                true);
        }       
        protected void txtCodMot_TextChanged(object sender, EventArgs e)
        {
            string codigo = txtCodMot.Text.Trim().ToUpper();

            if (string.IsNullOrWhiteSpace(codigo))
            {
                MostrarMensagem("Informe o código do motorista.", false);
                txtCodMot.Focus();
                return;
            }

            DataRow dr = motoristaDAL.ObterMotorista(codigo);

            if (dr == null)
            {
                MostrarMensagem("Motorista não encontrado.", false);

                ddlMotorista.SelectedIndex = 0;
                txtFuncao.Text = "";
                txtNucleo.Text = "";

                return;
            }

            ddlMotorista.SelectedValue = codigo;

            txtFuncao.Text = dr["funcao"].ToString();
            txtNucleo.Text = dr["nucleo"].ToString();
        }
        protected void ddlMotorista_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ddlMotorista.SelectedValue))
                return;
            
            string codigo = ddlMotorista.SelectedValue.Trim();

            DataRow dr = motoristaDAL.ObterMotorista(codigo);

            if (dr == null)
                return;

            txtCodMot.Text = codigo.ToString();

            txtFuncao.Text = dr["funcao"].ToString();

            txtNucleo.Text = dr["nucleo"].ToString();
        }
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            lblMensagem.Visible = false;

            if (!ValidarCadastro())
                return;

            int gravados = 0;

            List<string> duplicados = new List<string>();

            int quantidade = Convert.ToInt32(txtQuantidade.Text);

            DateTime dataInicial = Convert.ToDateTime(txtData.Text);

            DataRow motorista =
                motoristaDAL.ObterMotorista(txtCodMot.Text);

            List<ControleFaltasDTO> lista = new List<ControleFaltasDTO>();   

            for (int i = 0; i < quantidade; i++)
            {
                lista.Add(new ControleFaltasDTO
                {
                    CodMot = txtCodMot.Text,
                    NomMot = motorista["nommot"].ToString(),
                    Funcao = motorista["funcao"].ToString(),
                    Nucleo = motorista["nucleo"].ToString(),
                    DataFalta = dataInicial.AddDays(i),
                    Motivo = ddlMotivo.SelectedValue,
                    Observacao = txtObservacao.Text,
                    Usuario = Session["UsuarioLogado"].ToString()
                });
            }

            ResultadoLancamentoDTO resultado = controleDAL.SalvarPeriodo(lista);

            StringBuilder sb = new StringBuilder();
            
            sb.Append("<b>Registros gravados:</b> ");
            sb.Append(resultado.Gravados + "&nbsp;&nbsp;&nbsp;&nbsp;");
            

            if (resultado.DatasDuplicadas.Count > 0)
            {
                sb.Append("<br/><br/><br/>");               
                foreach (ControleFaltasDTO item in resultado.DatasDuplicadas)
                {
                    //sb.Append("<div style='padding-left:15px;padding-right:15px;'>");
                    sb.Append("<span style='color:red'>");
                    sb.Append("&nbsp;&nbsp;&nbsp;<b>Data:</b> ");
                    sb.Append(item.DataFalta.ToString("dd/MM/yyyy"));
                    sb.Append(" - <b>Motivo:</b> ");
                    sb.Append(item.Motivo);
                    sb.Append("</span><br/>");
                    
                }
                sb.Append("</span>");
            }

            MostrarMensagem(sb.ToString(), true);

            CarregarGrid();

            LimparCadastro();
            
        }
        private bool ValidarCadastro()
        {
            if (string.IsNullOrWhiteSpace(txtCodMot.Text))
            {
                MostrarMensagem("Informe o motorista.", false);
                txtCodMot.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtData.Text))
            {
                MostrarMensagem("Informe a data.", false);
                txtData.Focus();
                return false;
            }

            int quantidade;

            if (!int.TryParse(txtQuantidade.Text, out quantidade))
            {
                MostrarMensagem("Quantidade inválida.", false);
                txtQuantidade.Focus();
                return false;
            }

            if (quantidade <= 0)
            {
                MostrarMensagem("Quantidade deve ser maior que zero.", false);
                txtQuantidade.Focus();
                return false;
            }

            if (quantidade > 365)
            {
                MostrarMensagem("Quantidade máxima permitida é 365 dias.", false);
                txtQuantidade.Focus();
                return false;
            }

            return true;
        }
        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            gvFaltas.PageIndex = 0;

            CarregarGrid();
        }
        protected void gvFaltas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvFaltas.PageIndex = e.NewPageIndex;

            CarregarGrid();
        }
        protected void gvFaltas_RowCommand(object sender, GridViewCommandEventArgs e)
        {            
            if (e.CommandName == "Excluir")
            {
                int id = Convert.ToInt32(e.CommandArgument);


                controleDAL.Excluir(id);


                CarregarGrid();


                MostrarMensagem(
                    "Registro excluído com sucesso.",
                    true);
            }
        }                
        protected void ExcluirFalta(int id)
        {
            try
            {
                controleDAL.Excluir(id);

                CarregarGrid();

                MostrarMensagem(
                    "Registro excluído.",
                    true);
            }
            catch (Exception ex)
            {
                MostrarMensagem(
                    "Erro ao excluir: " + ex.Message,
                    false);
            }
        }        
        protected void ddlFiltroNucleo_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarGrid();
        }
        private void CarregarNucleos()
        {
            ddlFiltroNucleo.Items.Clear();

            ddlFiltroNucleo.Items.Add(new ListItem("Todos", ""));

            using (SqlConnection con = new SqlConnection(conexao))
            {
                string sql = @"
                SELECT DISTINCT nucleo
                FROM tbmotoristas
                WHERE nucleo IS NOT NULL
                  AND LTRIM(RTRIM(nucleo)) <> ''
                ORDER BY nucleo";

                SqlCommand cmd = new SqlCommand(sql, con);

                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    ddlFiltroNucleo.Items.Add(
                        new ListItem(
                            dr["nucleo"].ToString(),
                            dr["nucleo"].ToString()));
                }
            }
        }
    }
}
