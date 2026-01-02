using System.Configuration;
using System;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.Configuration;
using System.Web.UI;
using System.Data;

namespace NewCapit.dist.pages
{
    public partial class Frm_DistanciaEntreCidades : System.Web.UI.Page
    {
        SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString());
        DateTime dataHoraAtual = DateTime.Now;
        public string caminhoFoto;
        string nomeUsuario = "";
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
                //ddlStatus.SelectedValue = "Todos"; // padrão
                CarregarGrid("Todos");
               // CarregarArtigosCTB();
               // divMsg.Visible = false;
            }

        }

        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
           // CarregarGrid(ddlStatus.SelectedValue);
        }

        void CarregarGrid(string status, string pesquisa = "")
        {

            using (SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                string sql = @"SELECT 
                            uf_origem,
                            origem,
                            uf_destino,
                            destino,
                            distancia
                           FROM tbdistanciapremio";


                //if (status != "Todos")
                //{
                //    sql += " AND status = @status ";
                //}

                if (!string.IsNullOrWhiteSpace(pesquisa))
                {
                    sql += " WHERE (origem COLLATE Latin1_General_CI_AI LIKE @pesq OR destino COLLATE Latin1_General_CI_AI LIKE @pesq OR uf_origem COLLATE Latin1_General_CI_AI LIKE @pesq OR uf_destino COLLATE Latin1_General_CI_AI LIKE @pesq) ";
                }

                SqlCommand cmd = new SqlCommand(sql, conn);
                //if (status != "Todos")
                //    cmd.Parameters.AddWithValue("@status", status);

                if (!string.IsNullOrWhiteSpace(pesquisa))
                    cmd.Parameters.AddWithValue("@pesq", "%" + pesquisa + "%");


                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvCidades.DataSource = dt;
                gvCidades.DataBind();
            }
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
          //  CarregarGrid(ddlStatus.SelectedValue, txtPesquisar.Text.Trim());
        }

        protected void btnAbrirModal_Click(object sender, EventArgs e)
        {
            LimparCampos();
            AbrirModal();
        }

        private void AbrirModal()
        {
            ScriptManager.RegisterStartupScript(this, GetType(),
                "abrirModal",
                "abrirModalProcesso();", true);
        }

       
        private void LimparCampos()
        {
            //txtProcessoModal.Text = "";
            //txtNome.Text = "";
            //txtData.Text = "";
            //txtAIT.Text = "";
            //txtDia.Text = "";           
        }
       
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            //if (hfStatus.Value == "Baixado")
            //    return;

            //using (SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            //{
            //    SqlCommand cmd;

            //    if (hfStatus.Value == "NOVO")
            //    {
            //        cmd = new SqlCommand(
            //            "INSERT INTO tbmultas_veiculos (processo, ait, dthoranot, dia, codigo_infracao, frota, placa, codmot, nome, nucleo, artigo, pontos, valorsd, valorcd, vencimento, desc_multa, localmulta, providencia, envio_transp, envio_dcp, recebido_por, baixado_por, lancamento, gravidade, infrator,competencia, equipamento, transportadora, foto, data_pesquisa, status) " +
            //            "VALUES (@processo, @ait, @dthoranot, @dia, @codigo_infracao, @frota, @placa, @codmot, @nome, @nucleo, @artigo,@pontos, @valorsd, @valorcd, @vencimento, @desc_multa, @localmulta, @providencia, @envio_transp, @envio_dcp, @recebido_por, @baixado_por, @lancamento, @gravidade, @infrator, @competencia, @equipamento, @transportadora, @foto, @data_pesquisa, @status, @foto)", con);
            //    }
            //    else
            //    {
            //        cmd = new SqlCommand(
            //            "UPDATE tbmultas_veiculos SET processo=@processo, ait=@ait, dthoranot=@dthoranot, dia=@dia, codigo_infracao=@codigo_infracao, frota=@frota, placa=@placa, codmot=@codmot, nome=@nome, nucleo=@nucleo, artigo=@artigo, pontos=@pontos, valorsd=@valorsd, valorcd=@valorcd, vencimento=@vencimento, desc_multa=@desc_multa, localmulta=@localmulta, providencia=@providencia, envio_transp=@envio_transp, envio_dcp=@envio_dcp, recebido_por=@recebido_por, baixado_por=@baixado_por, lancamento=@lancamento, gravidade=@gravidade, infrator=@infrator, competencia=@competencia, equipamento=@equipamento, transportadora=@transportadora, foto=@foto, data_pesquisa=@data_pesquisa, status=@status WHERE processo=@processo", con);
            //    }

            //    cmd.Parameters.AddWithValue("@processo", txtProcessoModal.Text.Trim());
            //    cmd.Parameters.AddWithValue("@nome", txtNome.Text.Trim());
            //    cmd.Parameters.AddWithValue("@dthoranot", SafeDateTimeValue(txtData.Text));
            //    cmd.Parameters.AddWithValue("@ait", txtAIT.Text.Trim());
            //    cmd.Parameters.AddWithValue("@dia", txtDia.Text.Trim());
            //    cmd.Parameters.AddWithValue("@lancamento", SafeDateTimeValue(txtLancamento.Text));
            //    cmd.Parameters.AddWithValue("@codigo_infracao", txtCodigo_Infracao.Text.Trim());
            //    cmd.Parameters.AddWithValue("@artigo", ddlArtigo.SelectedItem.Text.Trim());
            //    cmd.Parameters.AddWithValue("@pontos", txtPontos.Text.Trim());
            //    cmd.Parameters.AddWithValue("@gravidade", txtGravidade.Text.Trim());
            //    cmd.Parameters.AddWithValue("@infrator", txtInfrator.Text.Trim());
            //    cmd.Parameters.AddWithValue("@competencia", txtCompetencia.Text.Trim());
            //    cmd.Parameters.AddWithValue("@desc_multa", txtdesc_multa.Text.Trim());
            //    cmd.Parameters.AddWithValue("@localmulta", txtLocalMulta.Text.Trim());
            //    cmd.Parameters.AddWithValue("@providencia", txtProvidencia.Text.Trim());
            //    cmd.Parameters.AddWithValue("@valorsd", Convert.ToDecimal(txtValorsd.Text.Trim()));
            //    cmd.Parameters.AddWithValue("@valorcd", Convert.ToDecimal(txtValorcd.Text.Trim()));
            //    cmd.Parameters.AddWithValue("@codmot", txtCodMot.Text.Trim());
            //    cmd.Parameters.AddWithValue("@nucleo", txtNucleo.Text.Trim());
            //    cmd.Parameters.AddWithValue("@vencimento", SafeDateValue(txtVencimento.Text));
            //    cmd.Parameters.AddWithValue("@frota", txtFrota.Text.Trim());
            //    cmd.Parameters.AddWithValue("@placa", txtPlaca.Text.Trim());
            //    cmd.Parameters.AddWithValue("@equipamento", txtEquipamento.Text.Trim());
            //    cmd.Parameters.AddWithValue("@transportadora", txtTransportadora.Text.Trim());
            //    cmd.Parameters.AddWithValue("@envio_transp", SafeDateTimeValue(txtEnvio_transp.Text));
            //    cmd.Parameters.AddWithValue("@envio_dcp", SafeDateTimeValue(txtEnvio_dcp.Text));
            //    cmd.Parameters.AddWithValue("@recebido_por", txtRecebido_Por.Text.Trim());
            //    cmd.Parameters.AddWithValue("@data_pesquisa", dataHoraAtual);
            //    //cmd.Parameters.AddWithValue("@pesquisado_por", Session["usuario_nome"].ToString());
            //    if (string.IsNullOrWhiteSpace(txtEnvio_dcp.Text))
            //    {
            //        cmd.Parameters.AddWithValue("@status", "Pendente");
            //        cmd.Parameters.AddWithValue("@baixado_por", "");
            //    }
            //    else
            //    {
            //        cmd.Parameters.AddWithValue("@status", "Baixado");
            //        cmd.Parameters.AddWithValue("@baixado_por", txtBaixado_por.Text.Trim());
            //    }
            //    cmd.Parameters.AddWithValue("@foto", imgFoto.ImageUrl);
            //    con.Open();
            //    cmd.ExecuteNonQuery();
            //    LimparCampos();
            //    txtProcessoModal.Focus();
            //}
        }
        
        protected void MostrarMsg(string mensagem, string tipo = "info")
        {
            //divMsg.Attributes["class"] = "alert alert-" + tipo + " alert-dismissible fade show mt-3";
            //lblMsg.InnerText = mensagem;
            //divMsg.Style["display"] = "block";

            //string script = @"setTimeout(function() {
            //            var div = document.getElementById('divMsg');
            //            if (div) div.style.display = 'none';
            //          }, 5000);";

            //ScriptManager.RegisterStartupScript(this, GetType(), "EscondeMsg", script, true);
        }
        private void CarregarArtigosCTB()
        {
            //using (SqlConnection conn = new SqlConnection(
            //    ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            //{
            //    SqlCommand cmd = new SqlCommand(
            //        "SELECT id, artigo_inciso_CTB FROM tbcodigoevalordasinfracoes ORDER BY artigo_inciso_CTB", conn);
            //    conn.Open();
            //    ddlArtigo.DataSource = cmd.ExecuteReader();
            //    ddlArtigo.DataTextField = "artigo_inciso_CTB";
            //    ddlArtigo.DataValueField = "id";
            //    ddlArtigo.DataBind();
            //}

           // ddlArtigo.Items.Insert(0, new ListItem("...", ""));
        }
    }
}