using DAL;
using Domain;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NewCapit.dist.pages
{
    public partial class Frm_Impressao_Motorista : System.Web.UI.Page
    {
        private SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conexao"].ToString());
        public string fotoMotorista;
        private string mes;
        private string ano;
        private string nomeUsuario;
        protected System.Web.UI.ScriptManager ScriptManager1;
        protected Repeater Repeater1;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.IsPostBack)
                return;
            if (this.Session["UsuarioLogado"] != null)
            {
                this.nomeUsuario = this.Session["UsuarioLogado"].ToString();
                string nomeUsuario = this.nomeUsuario;
            }
            this.CarregarDados();
        }

        private void CarregarDados()
        {
            string mes = this.Request.QueryString["mes"];
            string ano = this.Request.QueryString["ano"];
            string nucleosStr = this.Request.QueryString["nucleos"]; // "A,B,C"

            string cmdText = @"SELECT cracha, nome, funcao, admissao, nucleo, mes, frota, documentos, pontualidade,
                              segcarga, cargaedescarga, comunicacao, segtransito, consumocomb, conservacao, 
                              observacao, vl_total, dt_avaliacao, nm_usuario  
                       FROM tbavaliacaomotorista   
                       WHERE mes = @mes ";

            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("@mes", $"{mes}/{ano}"));

            if (!string.IsNullOrWhiteSpace(nucleosStr))
            {
                string[] nucleos = nucleosStr.Split(',');

                string filtros = string.Join(",", nucleos.Select((s, i) => "@n" + i));
                cmdText += $" AND nucleo IN ({filtros})";

                for (int i = 0; i < nucleos.Length; i++)
                    parametros.Add(new SqlParameter("@n" + i, nucleos[i]));
            }
            cmdText += " ORDER BY CAST(cracha AS INT)";

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            using (SqlCommand selectCommand = new SqlCommand(cmdText, connection))
            {
                selectCommand.Parameters.AddRange(parametros.ToArray());

                DataTable dataTable = new DataTable();
                new SqlDataAdapter(selectCommand).Fill(dataTable);

                this.Repeater1.DataSource = dataTable;
                this.Repeater1.DataBind();
            }
        }


        protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;
            DataRowView dataItem = (DataRowView)e.Item.DataItem;
            ((Label)e.Item.FindControl("lblCracha")).Text = dataItem["cracha"].ToString();
            ((Label)e.Item.FindControl("lblNomeMot")).Text = dataItem["nome"].ToString();
            ((Label)e.Item.FindControl("lblFuncao")).Text = dataItem["funcao"].ToString();
            ((Label)e.Item.FindControl("lblNucleo")).Text = dataItem["nucleo"].ToString();
            ((Label)e.Item.FindControl("lblMes")).Text = dataItem["mes"].ToString();
            ((Label)e.Item.FindControl("lblFrota")).Text = dataItem["frota"].ToString();
            ((Label)e.Item.FindControl("lblResultadoTotal")).Text = dataItem["vl_total"]?.ToString() + "%";
            ((TextBox)e.Item.FindControl("txtObs")).Text = dataItem["observacao"].ToString();
            ((Label)e.Item.FindControl("lblNomeAss")).Text = dataItem["nome"].ToString().ToUpper();
            DateTime result;
            ((Label)e.Item.FindControl("lblDtAdmissao")).Text = DateTime.TryParse(dataItem["admissao"].ToString(), out result) ? result.ToString("dd/MM/yyyy") : "";
            ((Label)e.Item.FindControl("lblUsuario")).Text = dataItem["nm_usuario"].ToString().ToUpper();
            ((Image)e.Item.FindControl("imgMotorista")).ImageUrl = this.ObterFoto(dataItem["cracha"].ToString());
            this.SelecionarRadioItem(dataItem["documentos"].ToString(), e, "rb_documentacao_1", "rb_documentacao_2", "rb_documentacao_3");
            this.SelecionarRadioItem(dataItem["pontualidade"].ToString(), e, "rb_pontualidade_1", "rb_pontualidade_2", "rb_pontualidade_3");
            this.SelecionarRadioItem(dataItem["segcarga"].ToString(), e, "rb_seguranca_1", "rb_seguranca_2", "rb_seguranca_3");
            this.SelecionarRadioItem(dataItem["cargaedescarga"].ToString(), e, "rb_cargadescarga_1", "rb_cargadescarga_2", "rb_cargadescarga_3");
            this.SelecionarRadioItem(dataItem["comunicacao"].ToString(), e, "rb_comunicacao_1", "rb_comunicacao_2", "rb_comunicacao_3");
            this.SelecionarRadioItem(dataItem["segtransito"].ToString(), e, "rb_transito_1", "rb_transito_2", "rb_transito_3");
            this.SelecionarRadioItem(dataItem["consumocomb"].ToString(), e, "rb_combustivel_1", "rb_combustivel_2", "rb_combustivel_3");
            this.SelecionarRadioItem(dataItem["conservacao"].ToString(), e, "rb_conservacao_1", "rb_conservacao_2", "rb_conservacao_3");
        }

        private string ObterFoto(string cracha)
        {
            ConsultaMotorista consultaMotorista = UsersDAL.CheckMotorista(new ConsultaMotorista()
            {
                codmot = cracha
            });
            string str = "../../fotos/motoristasemfoto.jpg";
            if (consultaMotorista != null)
            {
                if (!string.Equals(consultaMotorista.status?.Trim(), "INATIVO", StringComparison.OrdinalIgnoreCase))
                {
                    if (!string.IsNullOrWhiteSpace(consultaMotorista.caminhofoto))
                    {
                        string path = "../.." + consultaMotorista.caminhofoto.Trim();
                        str = File.Exists(this.Server.MapPath(path)) ? path : "../../fotos/motoristasemfoto.jpg";
                    }
                }
                else
                    str = "../../fotos/inativo.jpg";
            }
            return str;
        }

        private void SelecionarRadioItem(
          string valor,
          RepeaterItemEventArgs e,
          string id1,
          string id2,
          string id3)
        {
            ((CheckBox)e.Item.FindControl(id1)).Checked = valor == "1";
            ((CheckBox)e.Item.FindControl(id2)).Checked = valor == "2";
            ((CheckBox)e.Item.FindControl(id3)).Checked = valor == "3";
        }

        private void EnviarPdfParaCliente(string caminhoArquivo, string nomeArquivo)
        {
            HttpResponse response = HttpContext.Current.Response;
            response.Clear();
            response.ContentType = "application/pdf";
            response.AddHeader("Content-Disposition", "attachment; filename=" + nomeArquivo);
            response.WriteFile(caminhoArquivo);
            response.End();
        }
    }
}