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

namespace NewCapit.dist.pages
{
    public partial class GestaoDeMultas : System.Web.UI.Page
    {
        SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString());

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlStatus.SelectedValue = "Pendente"; // padrão
                CarregarGrid("Pendente");
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
                            ISNULL(NULLIF(foto,''), '/fotos/usuario.jpg') AS foto
                           FROM tbmultas_veiculos WHERE 1 = 1 ";


                if (status != "Todos")
                {
                    sql += " AND status = @status ";
                }

                if (!string.IsNullOrWhiteSpace(pesquisa))
                {
                    sql += " AND (placa LIKE @pesq OR nome LIKE @pesq OR processo LIKE @pesq) ";
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
                    string fotoPadrao = "/fotos/usuario.jpg";
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

                    txtLancamento.BackColor = System.Drawing.Color.LightYellow;
                    txtLancamento.ForeColor = System.Drawing.Color.Blue;
                    txtLancamento.Text = Convert.ToDateTime(dr["lancamento"]).ToString("dd/MM/yyyy HH:mm");

                    txtId.BackColor = System.Drawing.Color.Magenta;
                    txtId.ForeColor = System.Drawing.Color.White;
                    txtId.Text = dr["id"].ToString();

                    txtFrota.Text = dr["frota"].ToString();
                    txtPlaca.Text = dr["placa"].ToString();
                    txtEquipamento.Text = dr["equipamento"].ToString();
                    txtCodMot.Text = dr["codmot"].ToString();
                    txtNome.Text = dr["nome"].ToString();
                    txtCodigo_Infracao.Text = dr["codigo_infracao"].ToString();

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
                }
            }
        }
        private void LimparCampos()
        {
            txtProcessoModal.Text = "";
            txtNome.Text = "";
            txtData.Text = "";
            txtAIT.Text = "";
        }

        private void HabilitarCampos(bool habilitar)
        {
            txtNome.Enabled = habilitar;
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
                        "INSERT INTO tbmultas_veiculos (processo, ait, dthoranot, dia, codigo_infracao, frota, placa, codmot, nome, nucleo, artigo, pontos, valorsd, valorcd, vencimento, desc_multa, localmulta, providencia, envio_transp, envio_dpessoal, envio_dcp, recebido_por, baixado_por, lancamento, gravidade, infrator,competencia, equipamento, transportadora, foto, data_pesquisa, pesquisado_por, status) " +
                        "VALUES (@processo, @ait, @dthoranot, @dia, @codigo_infracao, @frota, @placa, @codmot, @nome, @nucleo, @artigo,@pontos, @valorsd, @valorcd, @vencimento, @desc_multa, @localmulta, @providencia, @envio_transp, @envio_dpessoal, @envio_dcp, @recebido_por, @baixado_por, @lancamento, @gravidade, @infrator, @competencia, @equipamento, @transportadora, @foto, @data_pesquisa, @pesquisado_por, @status)", con);
                }
                else
                {
                    cmd = new SqlCommand(
                        "UPDATE tbmultas_veiculos SET processo=@processo, ait=@ait, dthoranot=@dthoranot, dia=@dia, codigo_infracao=@codigo_infracao, frota=@frota, placa=@placa, codmot=@codmot, nome=@nome, nucleo=@nucleo, artigo=@artigo, pontos=@pontos, valorsd=@valorsd, valorcd=@valorcd, vencimento=@vencimento, desc_multa=@desc_multa, localmulta=@localmulta, providencia=@providencia, envio_transp=@envio_transp, envio_dpessoal=@envio_dpessoal, envio_dcp=@envio_dcp, recebido_por=@recebido_por, baixado_por=@baixado_por, lancamento=@lancamento, gravidade=@gravidade, infrator=@infrator, competencia=@competencia, equipamento=@equipamento, transportadora=@transportadora, foto=@foto, data_pesquisa=@data_pesquisa, pesquisado_por=@pesquisado_por, status=@status WHERE processo=@processo", con);
                }

                cmd.Parameters.AddWithValue("@processo", txtProcessoModal.Text);
                cmd.Parameters.AddWithValue("@nome", txtNome.Text);
                cmd.Parameters.AddWithValue("@dthoranot", txtData.Text);
                cmd.Parameters.AddWithValue("@ait", txtAIT.Text);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        protected void txtData_TextChanged(object sender, EventArgs e)
        {

            if (!DateTime.TryParse(txtData.Text,
                new CultureInfo("pt-BR"),
                DateTimeStyles.None,
                out DateTime data))
            {
                txtDia.Text = "";
                return;
            }

            if (data.ToString("dddd", new CultureInfo("pt-BR")) == "domingo")
            {
                txtDia.BackColor = System.Drawing.Color.LightCoral;
                txtDia.ForeColor = System.Drawing.Color.White;
                txtDia.Text = data.ToString("dddd", new CultureInfo("pt-BR"));
            }
            else
            {
                txtDia.BackColor = System.Drawing.Color.Khaki;
                txtDia.ForeColor = System.Drawing.Color.OrangeRed;
                txtDia.Text = data.ToString("dddd", new CultureInfo("pt-BR"));
            }


        }



    }
}