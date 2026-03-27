using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Configuration;
using System.Web.UI;

namespace NewCapit.dist.pages
{
    public partial class ControleAcesso : System.Web.UI.Page
    {
        // Variáveis que alimentam o Javascript
        public string LabelsAcessos = "[]";
        public string ValoresAcessos = "[]";
        string senhaMasterCorreta = "superadmin123";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //CarregarDadosDoBanco();
                txtSenhaMaster.Text = string.Empty;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "abrirModalSenha();", true);
            }
        }

        private void CarregarDadosDoBanco()
        {
            string constr = WebConfigurationManager.ConnectionStrings["conexao"].ToString();
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(constr))
            {
                string query = @"SELECT dt_login, nm_nome, emp_usuario 
                         FROM LogSessoes AS s 
                         INNER JOIN tb_usuario u ON s.cod_usuario = u.cod_usuario 
                         WHERE dt_logout IS NOT NULL";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd)) { sda.Fill(dt); }
                }
            }

            var listaDados = dt.AsEnumerable();

            // 1. Agrupamento por DATA
            var dadosDias = listaDados
                .GroupBy(r => Convert.ToDateTime(r["dt_login"]).Date)
                .Select(g => new { Lab = g.Key.ToString("dd/MM"), Val = g.Count() })
                .OrderBy(x => x.Lab).ToList();

            // 2. Agrupamento por USUÁRIO (Top 10)
            var dadosUsuarios = listaDados
                .GroupBy(r => r["nm_nome"].ToString())
                .Select(g => new { Lab = g.Key, Val = g.Count() })
                .OrderByDescending(x => x.Val).Take(10).ToList();

            // 3. Agrupamento por EMPRESA
            var dadosEmpresas = listaDados
                .GroupBy(r => r["emp_usuario"].ToString())
                .Select(g => new { Lab = g.Key, Val = g.Count() }).ToList();

            // Formatação de Strings para o JS
            string jsLabelsDias = "['" + string.Join("','", dadosDias.Select(d => d.Lab)) + "']";
            string jsValoresDias = "[" + string.Join(",", dadosDias.Select(d => d.Val)) + "]";

            string jsLabelsUser = "['" + string.Join("','", dadosUsuarios.Select(d => d.Lab)) + "']";
            string jsValoresUser = "[" + string.Join(",", dadosUsuarios.Select(d => d.Val)) + "]";

            string jsLabelsEmp = "['" + string.Join("','", dadosEmpresas.Select(d => d.Lab)) + "']";
            string jsValoresEmp = "[" + string.Join(",", dadosEmpresas.Select(d => d.Val)) + "]";

            // Injeção do Script
            string script = $@"
        function criarGrafico(id, tipo, labels, dados, labelGrafico, cor) {{
            new Chart(document.getElementById(id).getContext('2d'), {{
                type: tipo,
                data: {{
                    labels: labels,
                    datasets: [{{ label: labelGrafico, data: dados, backgroundColor: cor }}]
                }},
                options: {{ responsive: true, maintainAspectRatio: false }}
            }});
        }}

        criarGrafico('graficoDias', 'line', {jsLabelsDias}, {jsValoresDias}, 'Acessos/Dia', '#28a745');
        criarGrafico('graficoUsuarios', 'bar', {jsLabelsUser}, {jsValoresUser}, 'Acessos/Usuário', '#007bff');
        criarGrafico('graficoEmpresas', 'pie', {jsLabelsEmp}, {jsValoresEmp}, 'Empresas', ['#17a2b8','#ffc107','#dc3545','#6f42c1','#e83e8c']);
    ";

            ClientScript.RegisterStartupScript(this.GetType(), "drawCharts", script, true);
        }

        protected void btnConfirmarSenha_Click(object sender, EventArgs e)
        {
            

            string senhaDigitada = txtSenhaMaster.Text?.Trim();

            if (string.IsNullOrWhiteSpace(senhaDigitada) || senhaDigitada != senhaMasterCorreta)
            {
                lblErroSenha.Text = "Senha obrigatória para acessar os dados.";
                txtSenhaMaster.Text = "";

                ScriptManager.RegisterStartupScript(
                    this, this.GetType(),
                    "ManterModal",
                    "abrirModalSenha();",
                    true
                );
            }
            else
            {
                lblErroSenha.Text = "";

                ScriptManager.RegisterStartupScript(
                    this, this.GetType(),
                    "LiberarPagina",
                    "fecharOverlay();",
                    true
                );

                CarregarDadosDoBanco();
            }
        }

        protected void btnSair_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/dist/pages/Home.aspx");
        }
    }
}