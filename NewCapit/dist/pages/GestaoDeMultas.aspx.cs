using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.IO;
using System.Configuration;

namespace NewCapit.dist.pages
{
    public partial class GestaoDeMultas : System.Web.UI.Page
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
                ddlStatus.SelectedValue = "Pendente"; // padrão
                CarregarGrid("Pendente");
                CarregarArtigosCTB();
                divMsg.Visible = false;
                txtBaixado_por.Text = nomeUsuario;
            }

        }

        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarGrid(ddlStatus.SelectedValue);
        }

        void CarregarGrid(string status, string pesquisa = "")
        {

            using (SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                string sql = @"SELECT 
                            codmot,
                            nome,
                            baixado_por,
                            dthoranot,
                            dia,
                            placa,
                            equipamento,
                            transportadora,
                            processo,
                            ait,
                            status,
                            ISNULL(NULLIF(foto,''), '/fotos/motoristasemfoto.jpg') AS foto
                           FROM tbmultas_veiculos WHERE 1 = 1 ";


                if (status != "Todos")
                {
                    sql += " AND status = @status ";
                }

                if (!string.IsNullOrWhiteSpace(pesquisa))
                {
                    sql += " AND (placa LIKE @pesq OR nome LIKE @pesq OR processo LIKE @pesq OR codmot LIKE @pesq) ";
                }

                SqlCommand cmd = new SqlCommand(sql, conn);
                if (status != "Todos")
                    cmd.Parameters.AddWithValue("@status", status);

                if (!string.IsNullOrWhiteSpace(pesquisa))
                    cmd.Parameters.AddWithValue("@pesq", "%" + pesquisa + "%");


                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvProcessos.DataSource = dt;
                gvProcessos.DataBind();
            }
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            CarregarGrid(ddlStatus.SelectedValue, txtPesquisar.Text.Trim());
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

        protected void btnPesquisarModal_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT id, processo, ait, dthoranot, dia, codigo_infracao, frota, placa, codmot, nome, nucleo, artigo, pontos, valorsd, valorcd, vencimento, desc_multa, localmulta, providencia, envio_transp, envio_dpessoal, envio_dcp, recebido_por, baixado_por, lancamento, gravidade, infrator,competencia, equipamento, transportadora, foto, data_pesquisa, pesquisado_por, status FROM tbmultas_veiculos WHERE processo = @processo", con);

                cmd.Parameters.AddWithValue("@processo", txtProcessoModal.Text);
                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    colunaID.Visible = true;
                    string fotoPadrao = "/fotos/motoristasemfoto.jpg";
                    txtData.Text = Convert.ToDateTime(dr["dthoranot"]).ToString("dd/MM/yyyy HH:mm");
                    txtAIT.Text = dr["ait"].ToString();
                    if (dr["dia"].ToString() == "domingo")
                    {
                        txtDia.BackColor = System.Drawing.Color.LightCoral;
                        txtDia.ForeColor = System.Drawing.Color.White;
                        txtDia.Text = dr["dia"].ToString();
                    }
                    else
                    {
                        txtDia.BackColor = System.Drawing.Color.Khaki;
                        txtDia.ForeColor = System.Drawing.Color.OrangeRed;
                        txtDia.Text = dr["dia"].ToString();
                    }
                    if (dr["status"].ToString() == "Baixado")
                    {
                        txtStatus.BackColor = System.Drawing.Color.LightCoral;
                        txtStatus.ForeColor = System.Drawing.Color.White;
                        txtStatus.Text = dr["status"].ToString();
                    }
                    else
                    {
                        txtStatus.BackColor = System.Drawing.Color.Khaki;
                        txtStatus.ForeColor = System.Drawing.Color.OrangeRed;
                        txtStatus.Text = dr["status"].ToString();
                    }


                    txtLancamento.BackColor = System.Drawing.Color.LightYellow;
                    txtLancamento.ForeColor = System.Drawing.Color.Blue;
                    txtLancamento.Text = Convert.ToDateTime(dr["lancamento"]).ToString("dd/MM/yyyy HH:mm");

                    txtId.BackColor = System.Drawing.Color.Magenta;
                    txtId.ForeColor = System.Drawing.Color.White;
                    txtId.Text = dr["id"].ToString();
                    txtCodigo_Infracao.Text = dr["codigo_infracao"].ToString();
                    ddlArtigo.SelectedItem.Text = dr["artigo"].ToString();
                    txtPontos.Text = dr["pontos"].ToString();
                    txtGravidade.Text = dr["gravidade"].ToString();
                    txtInfrator.Text = dr["infrator"].ToString();
                    txtCompetencia.Text = dr["competencia"].ToString();
                    txtdesc_multa.Text = dr["desc_multa"].ToString();
                    txtLocalMulta.Text = dr["localmulta"].ToString();
                    txtProvidencia.Text = dr["providencia"].ToString();
                    if (dr["valorsd"] != DBNull.Value)
                    {
                        decimal valorSemDesconto = Convert.ToDecimal(dr["valorsd"]);
                        txtValorsd.Text = valorSemDesconto.ToString("N2", new CultureInfo("pt-BR"));
                    }
                    else
                    {
                        txtValorsd.Text = "0,00";
                    }
                    if (dr["valorcd"] != DBNull.Value)
                    {
                        decimal valorComDesconto = Convert.ToDecimal(dr["valorcd"]);
                        txtValorcd.Text = valorComDesconto.ToString("N2", new CultureInfo("pt-BR"));
                    }
                    else
                    {
                        txtValorcd.Text = "0,00";
                    }
                    txtCodMot.Text = dr["codmot"].ToString();
                    txtNome.Text = dr["nome"].ToString();
                    txtNucleo.Text = dr["nucleo"].ToString();
                    if (dr["vencimento"] != DBNull.Value)
                    {
                        txtVencimento.Text = Convert.ToDateTime(dr["vencimento"])
                            .ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        txtVencimento.Text = "";
                    }
                    txtTransportadora.Text = dr["transportadora"].ToString();

                    if (dr["envio_transp"] != DBNull.Value)
                    {
                        txtEnvio_transp.Text = Convert.ToDateTime(dr["envio_transp"])
                            .ToString("dd/MM/yyyy HH:mm");
                    }
                    else
                    {
                        txtEnvio_transp.Text = "";
                    }

                    if (dr["envio_dcp"] != DBNull.Value)
                    {
                        txtEnvio_dcp.Text = Convert.ToDateTime(dr["envio_dcp"])
                            .ToString("dd/MM/yyyy HH:mm");
                    }
                    else
                    {
                        txtEnvio_dcp.Text = "";
                    }
                    txtRecebido_Por.Text = dr["recebido_por"].ToString();
                    txtBaixado_por.Text = dr["baixado_por"].ToString();

                    txtFrota.Text = dr["frota"].ToString();
                    txtPlaca.Text = dr["placa"].ToString();
                    txtEquipamento.Text = dr["equipamento"].ToString();

                    if (dr["foto"].ToString() != null && !string.IsNullOrWhiteSpace(dr["foto"].ToString()))
                    {
                        string caminhoFoto = dr["foto"].ToString();

                        // verifica se o arquivo realmente existe
                        string caminhoFisico = Server.MapPath(caminhoFoto);

                        if (File.Exists(caminhoFisico))
                            imgFoto.ImageUrl = ResolveUrl(caminhoFoto);
                        else
                            imgFoto.ImageUrl = ResolveUrl(fotoPadrao);
                    }
                    else
                    {
                        imgFoto.ImageUrl = ResolveUrl(fotoPadrao);
                    }

                    hfStatus.Value = dr["status"].ToString();

                    if (hfStatus.Value == "Pendente")
                    {
                        HabilitarCampos(true);
                        btnSalvar.Visible = true;
                    }
                    else // BAIXADO
                    {
                        HabilitarCampos(false);
                        btnSalvar.Visible = false;
                    }
                }
                else
                {
                    // não encontrado → novo
                    HabilitarCampos(true);
                    btnSalvar.Visible = true;
                    hfStatus.Value = "NOVO";
                    MostrarMsg("Processo " + txtProcessoModal.Text + " não encontrado. Você pode cadastrar uma nova multa.", "info");
                    txtNome.Text = "";
                    txtData.Text = "";
                    txtAIT.Text = "";
                    txtDia.Text = "";
                    txtId.Text = "";
                    txtCodigo_Infracao.Text = "";
                    ddlArtigo.SelectedIndex = 0;
                    txtPontos.Text = "";
                    txtGravidade.Text = "";
                    txtInfrator.Text = "";
                    txtCompetencia.Text = "";
                    txtdesc_multa.Text = "";
                    txtLocalMulta.Text = "";
                    txtProvidencia.Text = "";
                    txtValorsd.Text = "";
                    txtValorcd.Text = "";
                    txtCodMot.Text = "";
                    txtNucleo.Text = "";
                    txtVencimento.Text = "";
                    txtTransportadora.Text = "";
                    txtEnvio_transp.Text = "";
                    txtEnvio_dcp.Text = "";
                    txtBaixado_por.Text = "";
                    txtFrota.Text = "";
                    txtPlaca.Text = "";
                    txtEquipamento.Text = "";
                    imgFoto.ImageUrl = ResolveUrl("/fotos/motoristasemfoto.jpg");



                    txtLancamento.BackColor = System.Drawing.Color.LightYellow;
                    txtLancamento.ForeColor = System.Drawing.Color.Blue;
                    txtLancamento.Text = dataHoraAtual.ToString("dd/MM/yyyy HH:mm");
                    txtStatus.BackColor = System.Drawing.Color.Khaki;
                    txtStatus.ForeColor = System.Drawing.Color.OrangeRed;
                    txtStatus.Text = "Pendente";

                }
            }
        }
        private void LimparCampos()
        {
            txtProcessoModal.Text = "";
            txtNome.Text = "";
            txtData.Text = "";
            txtAIT.Text = "";
            txtDia.Text = "";
            txtId.Text = "";
            txtCodigo_Infracao.Text = "";
            ddlArtigo.SelectedItem.Text = "";
            txtPontos.Text = "";
            txtGravidade.Text = "";
            txtInfrator.Text = "";
            txtCompetencia.Text = "";
            txtdesc_multa.Text = "";
            txtLocalMulta.Text = "";
            txtProvidencia.Text = "";
            txtValorsd.Text = "";
            txtValorcd.Text = "";
            txtCodMot.Text = "";
            txtNucleo.Text = "";
            txtVencimento.Text = "";
            txtTransportadora.Text = "";
            txtEnvio_transp.Text = "";
            txtEnvio_dcp.Text = "";
            txtBaixado_por.Text = "";
            txtFrota.Text = "";
            txtPlaca.Text = "";
            txtEquipamento.Text = "";
            txtStatus.Text = "";
            txtRecebido_Por.Text = "";
            imgFoto.ImageUrl = ResolveUrl("/fotos/motoristasemfoto.jpg");
        }
        private void HabilitarCampos(bool habilitar)
        {
            //txtNome.Enabled = habilitar;
            txtData.Enabled = habilitar;
            txtAIT.Enabled = habilitar;

        }
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if (hfStatus.Value == "Baixado")
                return;

            using (SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                SqlCommand cmd;

                if (hfStatus.Value == "NOVO")
                {
                    cmd = new SqlCommand(
                        "INSERT INTO tbmultas_veiculos (processo, ait, dthoranot, dia, codigo_infracao, frota, placa, codmot, nome, nucleo, artigo, pontos, valorsd, valorcd, vencimento, desc_multa, localmulta, providencia, envio_transp, envio_dcp, recebido_por, baixado_por, lancamento, gravidade, infrator,competencia, equipamento, transportadora, foto, data_pesquisa, status) " +
                        "VALUES (@processo, @ait, @dthoranot, @dia, @codigo_infracao, @frota, @placa, @codmot, @nome, @nucleo, @artigo,@pontos, @valorsd, @valorcd, @vencimento, @desc_multa, @localmulta, @providencia, @envio_transp, @envio_dcp, @recebido_por, @baixado_por, @lancamento, @gravidade, @infrator, @competencia, @equipamento, @transportadora, @foto, @data_pesquisa, @status, @foto)", con);
                }
                else
                {
                    cmd = new SqlCommand(
                        "UPDATE tbmultas_veiculos SET processo=@processo, ait=@ait, dthoranot=@dthoranot, dia=@dia, codigo_infracao=@codigo_infracao, frota=@frota, placa=@placa, codmot=@codmot, nome=@nome, nucleo=@nucleo, artigo=@artigo, pontos=@pontos, valorsd=@valorsd, valorcd=@valorcd, vencimento=@vencimento, desc_multa=@desc_multa, localmulta=@localmulta, providencia=@providencia, envio_transp=@envio_transp, envio_dcp=@envio_dcp, recebido_por=@recebido_por, baixado_por=@baixado_por, lancamento=@lancamento, gravidade=@gravidade, infrator=@infrator, competencia=@competencia, equipamento=@equipamento, transportadora=@transportadora, foto=@foto, data_pesquisa=@data_pesquisa, status=@status WHERE processo=@processo", con);
                }

                cmd.Parameters.AddWithValue("@processo", txtProcessoModal.Text.Trim());
                cmd.Parameters.AddWithValue("@nome", txtNome.Text.Trim());
                cmd.Parameters.AddWithValue("@dthoranot", SafeDateTimeValue(txtData.Text));
                cmd.Parameters.AddWithValue("@ait", txtAIT.Text.Trim());
                cmd.Parameters.AddWithValue("@dia", txtDia.Text.Trim());
                cmd.Parameters.AddWithValue("@lancamento", SafeDateTimeValue(txtLancamento.Text));
                cmd.Parameters.AddWithValue("@codigo_infracao", txtCodigo_Infracao.Text.Trim());
                cmd.Parameters.AddWithValue("@artigo", ddlArtigo.SelectedItem.Text.Trim());
                cmd.Parameters.AddWithValue("@pontos", txtPontos.Text.Trim());
                cmd.Parameters.AddWithValue("@gravidade", txtGravidade.Text.Trim());
                cmd.Parameters.AddWithValue("@infrator", txtInfrator.Text.Trim());
                cmd.Parameters.AddWithValue("@competencia", txtCompetencia.Text.Trim());
                cmd.Parameters.AddWithValue("@desc_multa", txtdesc_multa.Text.Trim());
                cmd.Parameters.AddWithValue("@localmulta", txtLocalMulta.Text.Trim());
                cmd.Parameters.AddWithValue("@providencia", txtProvidencia.Text.Trim());
                cmd.Parameters.AddWithValue("@valorsd", Convert.ToDecimal(txtValorsd.Text.Trim()));
                cmd.Parameters.AddWithValue("@valorcd", Convert.ToDecimal(txtValorcd.Text.Trim()));
                cmd.Parameters.AddWithValue("@codmot", txtCodMot.Text.Trim());
                cmd.Parameters.AddWithValue("@nucleo", txtNucleo.Text.Trim());
                cmd.Parameters.AddWithValue("@vencimento", SafeDateValue(txtVencimento.Text));
                cmd.Parameters.AddWithValue("@frota", txtFrota.Text.Trim());
                cmd.Parameters.AddWithValue("@placa", txtPlaca.Text.Trim());
                cmd.Parameters.AddWithValue("@equipamento", txtEquipamento.Text.Trim());
                cmd.Parameters.AddWithValue("@transportadora", txtTransportadora.Text.Trim());
                cmd.Parameters.AddWithValue("@envio_transp", SafeDateTimeValue(txtEnvio_transp.Text));
                cmd.Parameters.AddWithValue("@envio_dcp", SafeDateTimeValue(txtEnvio_dcp.Text));
                cmd.Parameters.AddWithValue("@recebido_por", txtRecebido_Por.Text.Trim());
                cmd.Parameters.AddWithValue("@data_pesquisa", dataHoraAtual);
                //cmd.Parameters.AddWithValue("@pesquisado_por", Session["usuario_nome"].ToString());
                if (string.IsNullOrWhiteSpace(txtEnvio_dcp.Text))
                {
                    cmd.Parameters.AddWithValue("@status", "Pendente");
                    cmd.Parameters.AddWithValue("@baixado_por", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@status", "Baixado");
                    cmd.Parameters.AddWithValue("@baixado_por", txtBaixado_por.Text.Trim());
                }
                cmd.Parameters.AddWithValue("@foto", imgFoto.ImageUrl);
                con.Open();
                cmd.ExecuteNonQuery();
                LimparCampos();
                txtProcessoModal.Focus();
            }
        }
        protected void txtData_TextChanged(object sender, EventArgs e)
        {
            DateTime data;

            if (!DateTime.TryParse(
                txtData.Text,
                new CultureInfo("pt-BR"),
                DateTimeStyles.None,
                out data))
            {
                txtDia.Text = "";
                return;
            }

            string diaSemana = data.ToString("dddd", new CultureInfo("pt-BR"));

            if (diaSemana.Equals("domingo", StringComparison.OrdinalIgnoreCase))
            {
                txtDia.BackColor = System.Drawing.Color.LightCoral;
                txtDia.ForeColor = System.Drawing.Color.White;
            }
            else
            {
                txtDia.BackColor = System.Drawing.Color.Khaki;
                txtDia.ForeColor = System.Drawing.Color.OrangeRed;
            }

            txtDia.Text = diaSemana;
        }

        protected void MostrarMsg(string mensagem, string tipo = "info")
        {
            divMsg.Attributes["class"] = "alert alert-" + tipo + " alert-dismissible fade show mt-3";
            lblMsg.InnerText = mensagem;
            divMsg.Style["display"] = "block";

            string script = @"setTimeout(function() {
                        var div = document.getElementById('divMsg');
                        if (div) div.style.display = 'none';
                      }, 5000);";

            ScriptManager.RegisterStartupScript(this, GetType(), "EscondeMsg", script, true);
        }
        private void CarregarArtigosCTB()
        {
            using (SqlConnection conn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT id, artigo_inciso_CTB FROM tbcodigoevalordasinfracoes ORDER BY artigo_inciso_CTB", conn);
                conn.Open();
                ddlArtigo.DataSource = cmd.ExecuteReader();
                ddlArtigo.DataTextField = "artigo_inciso_CTB";
                ddlArtigo.DataValueField = "id";
                ddlArtigo.DataBind();
            }

            ddlArtigo.Items.Insert(0, new ListItem("...", ""));
        }
        protected void txtCodMot_TextChanged(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT codmot, nommot, transp, nucleo, caminhofoto FROM tbmotoristas WHERE codmot = @codmot", con);

                cmd.Parameters.AddWithValue("@codmot", txtCodMot.Text.Trim());
                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    string fotoPadrao = "/fotos/motoristasemfoto.jpg";

                    txtCodMot.Text = dr["codmot"].ToString();
                    txtNome.Text = dr["nommot"].ToString();
                    txtNucleo.Text = dr["nucleo"].ToString();
                    txtTransportadora.Text = dr["transp"].ToString();

                    if (dr["caminhofoto"].ToString() != null && !string.IsNullOrWhiteSpace(dr["caminhofoto"].ToString()))
                    {
                        string caminhoFoto = dr["caminhofoto"].ToString();

                        // verifica se o arquivo realmente existe
                        string caminhoFisico = Server.MapPath(caminhoFoto);

                        if (File.Exists(caminhoFisico))
                            imgFoto.ImageUrl = ResolveUrl(caminhoFoto);
                        else
                            imgFoto.ImageUrl = ResolveUrl(fotoPadrao);
                    }
                    else
                    {
                        imgFoto.ImageUrl = ResolveUrl(fotoPadrao);
                    }
                    txtFrota.Focus();

                }
                else
                {
                    // não encontrado → novo
                    divMsg.Visible = true;
                    MostrarMsg(txtCodMot.Text + " - Código não encontrado. Verifique a digitação!", "info");
                    txtCodMot.Text = "";
                    txtNome.Text = "";
                    txtNucleo.Text = "";
                    txtTransportadora.Text = "";
                    txtCodMot.Focus();
                    return;
                }
            }

        }
        protected void txtFrota_TextChanged(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT  codvei, plavei, tipvei FROM tbveiculos WHERE codvei = @codvei", con);

                cmd.Parameters.AddWithValue("@codvei", txtFrota.Text.Trim());
                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    txtFrota.Text = dr["codvei"].ToString();
                    txtPlaca.Text = dr["plavei"].ToString();
                    txtEquipamento.Text = dr["tipvei"].ToString();
                }
                else
                {
                    SqlCommand cmdCarreta = new SqlCommand(
                    "SELECT  codcarreta, placacarreta FROM tbcarretas WHERE codcodcarreta = @codcarreta", con);

                    cmdCarreta.Parameters.AddWithValue("@codcarreta", txtFrota.Text.Trim());
                    con.Open();

                    SqlDataReader drCarreta = cmdCarreta.ExecuteReader();
                    if (drCarreta.Read())
                    {
                        txtFrota.Text = dr["codcarreta"].ToString();
                        txtPlaca.Text = dr["placacarreta"].ToString();
                        txtEquipamento.Text = "Reboque";
                    }
                    else
                    {
                        // não encontrado → novo
                        MostrarMsg(txtFrota.Text + " - Frota não encontrada. Verifique a digitação!", "info");
                        txtFrota.Text = "";
                        txtPlaca.Text = "";
                        txtEquipamento.Text = "";
                        return;
                    }
                }
            }

        }
        protected void txtCodigo_Infracao_TextChanged(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT artigo_inciso_CTB, pontos, codigo, descricao, gravidade, responsavel, autuador, valorsemindicacao, valorcomindicacao FROM tbcodigoevalordasinfracoes WHERE codigo = @codigo", con);

                cmd.Parameters.AddWithValue("@codigo", txtCodigo_Infracao.Text.Trim());
                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {

                    txtCodigo_Infracao.Text = dr["codigo"].ToString();
                    ddlArtigo.SelectedItem.Text = dr["artigo_inciso_CTB"].ToString();
                    txtPontos.Text = dr["pontos"].ToString();
                    txtGravidade.Text = dr["gravidade"].ToString();
                    txtInfrator.Text = dr["responsavel"].ToString();
                    txtCompetencia.Text = dr["autuador"].ToString();
                    txtdesc_multa.Text = dr["descricao"].ToString();


                    txtLocalMulta.Focus();

                }
                else
                {
                    // não encontrado → novo
                    MostrarMsg(txtCodigo_Infracao.Text + " - Código não encontrado. Verifique a digitação!", "info");
                    txtCodigo_Infracao.Text = "";
                    ddlArtigo.SelectedItem.Text = "";
                    txtPontos.Text = "";
                    txtGravidade.Text = "";
                    txtInfrator.Text = "";
                    txtCompetencia.Text = "";
                    txtdesc_multa.Text = "";
                    txtCodigo_Infracao.Focus();

                }
            }

        }
        private object SafeDateValue(string input)
        {
            DateTime dt;
            if (DateTime.TryParse(input, out dt))
                return dt.ToString("yyyy-MM-dd");
            else
                return DBNull.Value;
        }
        private object SafeDateTimeValue(string input)
        {
            DateTime dt;
            if (DateTime.TryParse(input, out dt))
                return dt.ToString("yyyy-MM-dd HH:mm:ss");
            else
                return DBNull.Value;
        }
    }
}
