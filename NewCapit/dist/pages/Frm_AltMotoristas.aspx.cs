
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using NPOI.SS.Formula.Functions;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using static NPOI.HSSF.Util.HSSFColor;

namespace NewCapit.dist.pages
{
    public partial class Frm_AltMotoristas : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conexao"].ToString());
        string id;
        string id_uf;
        public string fotoMotorista;
        DateTime dataHoraAtual = DateTime.Now;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UsuarioLogado"] != null)
                {
                    string nomeUsuario = Session["UsuarioLogado"].ToString();
                    var lblUsuario = nomeUsuario;

                    //  txtAlteradoPor.Text = nomeUsuario;

                }
                else
                {
                    var lblUsuario = "<Usuário>";
                    //txtAlteradoPor.Text = lblUsuario;
                }
                
                PreencherComboCargo();
                PreencherComboFiliais();              
                PreencherComboJornada();
                CarregarDDLAgregados();
                CarregarRegioes();
                CarregarEstadosNascimento();
                CarregarEstCNH();              
                CarregaDadosMotorista();
                
            }
              

            fotoMotorista = txtCaminhoFoto.Text.Trim();
            if (ddlStatus.SelectedItem.Text == "ATIVO")
            {
                motivoInativo.Visible = false;
                dataInativo.Visible = false;    
                txtMotivoInativacao.Visible = false;
                txtDtInativacao.Visible = false;
            }
            else
            {
                motivoInativo.Visible = true;
                dataInativo.Visible = true;
                txtMotivoInativacao.Visible = true;
                txtDtInativacao.Visible = true;
            }
        }
        private void CarregarRegioes()
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                SqlCommand cmd = new SqlCommand("SELECT id, regiao FROM tbregioesdopais", conn);
                conn.Open();
                ddlRegioes.DataSource = cmd.ExecuteReader();
                ddlRegioes.DataTextField = "regiao";
                ddlRegioes.DataValueField = "id";
                ddlRegioes.DataBind();

                if (hdfRegiao.Value != string.Empty)
                {
                    SqlCommand cmde = new SqlCommand("SELECT id, regiao FROM tbregioesdopais where regiao='" + hdfRegiao.Value + "'", conn);
                    conn.Open();
                    ddlRegioes.DataSource = cmde.ExecuteReader();
                    ddlRegioes.Items.Insert(0, new ListItem("regiao", "id"));
                }
                


            }
        }
        private void CarregarEstadosNascimento()
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                SqlCommand cmd = new SqlCommand("SELECT Uf, IdRegiao, SiglaUf FROM tbestadosbrasileiros WHERE IdRegiao = @RegiaoId", conn);
                cmd.Parameters.AddWithValue("@RegiaoId", ddlRegioes.SelectedValue);
                conn.Open();
                ddlEstNasc.DataSource = cmd.ExecuteReader();
                ddlEstNasc.DataTextField = "SiglaUf";
                ddlEstNasc.DataValueField = "Uf";
                ddlEstNasc.DataBind();
                //ddlEstNasc.Items.Insert(0, new ListItem("Selecione", "0"));
            }
        }
        private void CarregarMunicipioNasc()
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {              



                SqlCommand cmd = new SqlCommand("SELECT nome_municipio FROM tbmunicipiosbrasileiros WHERE IdRegiao = @RegiaoId AND Uf = @Uf", conn);
                cmd.Parameters.AddWithValue("@RegiaoId", ddlRegioes.SelectedValue);
                cmd.Parameters.AddWithValue("@Uf", ddlEstNasc.SelectedValue);
                conn.Open();
                ddlMunicipioNasc.DataSource = cmd.ExecuteReader();
                ddlMunicipioNasc.DataTextField = "nome_municipio";
                ddlMunicipioNasc.DataValueField = "nome_municipio";
                ddlMunicipioNasc.DataBind();
                ddlMunicipioNasc.Items.Insert(0, new ListItem("Selecione", "0"));
            }
        }
        protected void ddlRegioes_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarEstadosNascimento(); // Atualiza subcategorias com base na categoria
            CarregarMunicipioNasc();         // Atualiza itens com base na nova subcategoria
        }
        protected void ddlEstNasc_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarMunicipioNasc(); // Atualiza itens com base na nova subcategoria
        }
        private void CarregarEstCNH()
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                SqlCommand cmd = new SqlCommand("SELECT Uf, SiglaUf FROM tbestadosbrasileiros", conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                ddlCNH.DataSource = reader;
                ddlCNH.DataTextField = "SiglaUf";
                ddlCNH.DataValueField = "Uf";
                ddlCNH.DataBind();


                if (hdfCnh.Value != string.Empty)
                {
                    SqlCommand cmdc = new SqlCommand("SELECT Uf, SiglaUf FROM tbestadosbrasileiros where SiglaUf=@SiglaUf", conn);
                    cmd.Parameters.AddWithValue("@SiglaUf", hdfCnh.Value);
                    conn.Open();
                    SqlDataReader readerc = cmdc.ExecuteReader();

                    ddlCNH.Items.Insert(0, new ListItem("SiglaUf", "Uf"));
                }
                


                
            }
        }
        protected void ddlCNH_SelectedIndexChanged(object sender, EventArgs e)
        {
            //int cidadeId = int.Parse(ddlCNH.SelectedValue);
            // CarregarMunicipioCNH();
            //updCnh.Update();
            string uf = ddlCNH.SelectedValue;
            CarregarMunicipioCNH(uf);

            // Restaurar cidade se estiver em ViewState
            if (ViewState["CidadeSelecionada"] != null)
            {
                string cidadeId = ViewState["CidadeSelecionada"].ToString();
                if (ddlMunicCnh.Items.FindByValue(cidadeId) != null)
                {
                    ddlMunicCnh.SelectedValue = cidadeId;
                }
            }


        }
        private void CarregarMunicipioCNH(string uf)
        {            
            string strConn = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                string query = "SELECT cod_municipio, nome_municipio FROM tbmunicipiosbrasileiros WHERE uf = @UF";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UF", uf);
                conn.Open();
                ddlMunicCnh.DataSource = cmd.ExecuteReader();
                ddlMunicCnh.DataTextField = "nome_municipio";
                ddlMunicCnh.DataValueField = "cod_municipio"; // valor único
                ddlMunicCnh.DataBind();

                ddlMunicCnh.Items.Insert(0, new ListItem("-- Selecione uma cidade --", "0"));
            }



        }
        public void CarregaDadosMotorista()
        {
            if (HttpContext.Current.Request.QueryString["id"].ToString() != "")
            {
                id = HttpContext.Current.Request.QueryString["id"].ToString();
            }
            string sql = "SELECT codmot, nommot, status, CONVERT(varchar, emissaorg, 103) as emissaorg, numrg, cargo, nucleo, orgaorg, cpf, numregcnh, codsegurancacnh, catcnh, CONVERT(varchar, venccnh, 103) as venccnh, codliberacao, numpis, endmot, baimot, cidmot, ufmot, cepmot, fone3, fone2, validade, CONVERT(varchar, dtnasc, 103) as dtnasc, estcivil, sexo, horario, nomepai, nomemae, codtra, transp, CONVERT(varchar, cadmot, 103) as cadmot, inativo, CONVERT(varchar, dtinativo, 103) as dtinativo, historico, alterado, dataalteracao, cartaomot, naturalmot, numero, complemento, tipomot, codprop, placa, reboque1, reboque2, tipoveiculo, venccartao, horario, funcao, frota, usucad, CONVERT(varchar, dtccad, 103) as dtccad, venceti, codvei, ufnascimento, formulariocnh, ufcnh, municipiocnh, vencmoop, cracha, regiao, numinss, caminhofoto FROM tbmotoristas WHERE id = " + id;

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
                    txtCodMot.Text = dt.Rows[0][0].ToString();
                }
                txtNomMot.Text = dt.Rows[0][1].ToString();
                ddlStatus.SelectedValue = dt.Rows[0][2].ToString();
                txtDtEmissao.Text = dt.Rows[0][3].ToString();
                txtRG.Text = dt.Rows[0][4].ToString();
                ddlCargo.SelectedItem.Text = dt.Rows[0][5].ToString();
                cbFiliais.SelectedItem.Text = dt.Rows[0][6].ToString();
                txtEmissor.Text = dt.Rows[0][7].ToString();
                txtCPF.Text = dt.Rows[0][8].ToString();
                txtRegCNH.Text = dt.Rows[0][9].ToString();
                txtCodSeguranca.Text = dt.Rows[0][10].ToString();
                ddlCat.SelectedItem.Text = dt.Rows[0][11].ToString();
                txtValCNH.Text = dt.Rows[0][12].ToString();
                txtCodLibRisco.Text = dt.Rows[0][13].ToString();
                txtPIS.Text = dt.Rows[0][14].ToString();
                txtEndCli.Text = dt.Rows[0][15].ToString();
                txtBaiCli.Text = dt.Rows[0][16].ToString();
                txtCidCli.Text = dt.Rows[0][17].ToString();
                txtEstCli.Text = dt.Rows[0][18].ToString();
                txtCepCli.Text = dt.Rows[0][19].ToString();
                txtFixo.Text = dt.Rows[0][20].ToString();
                txtCelular.Text = dt.Rows[0][21].ToString();
                txtValLibRisco.Text = dt.Rows[0][22].ToString();
                txtDtNasc.Text = dt.Rows[0][23].ToString();
                ddlEstCivil.SelectedItem.Text = dt.Rows[0][24].ToString();
                ddlSexo.SelectedItem.Text = dt.Rows[0][25].ToString();
                ddlJornada.SelectedItem.Text = dt.Rows[0][26].ToString();                
                txtNomePai.Text = dt.Rows[0][27].ToString();
                txtNomeMae.Text = dt.Rows[0][28].ToString();
                txtCodTra.Text = dt.Rows[0][29].ToString();
                ddlAgregados.Items.Insert(0, new ListItem(dt.Rows[0][30].ToString(), "0"));
                txtDtCad.Text = DateTime.Parse(dt.Rows[0][31].ToString()).ToString("dd/MM/yyyy");
                if (dt.Rows[0][32].ToString() != string.Empty)
                {
                    txtMotivoInativacao.Text = dt.Rows[0][32].ToString();
                    txtDtInativacao.Text = dt.Rows[0][33].ToString();
                }
                if (dt.Rows[0][34].ToString() != string.Empty)
                {
                    txtHistorico.Text = dt.Rows[0][34].ToString();
                }
                if (dt.Rows[0][35].ToString() != string.Empty)
                {
                    txtAltCad.Text = dt.Rows[0][35].ToString();
                }
                else
                {
                    txtAltCad.Text = Session["UsuarioLogado"].ToString();
                }

                if(dt.Rows[0][36].ToString() != string.Empty)
                {
                    lbDtAtualizacao.Text = DateTime.Parse(dt.Rows[0][36].ToString()).ToString("dd/MM/yyyy HH:mm");
                }
                else
                {
                    lbDtAtualizacao.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                }
                
                txtCartao.Text = dt.Rows[0][37].ToString();
                ddlMunicipioNasc.Items.Insert(0, new ListItem(dt.Rows[0][38].ToString(), "0"));
                txtNumero.Text = dt.Rows[0][39].ToString();
                txtComplemento.Text = dt.Rows[0][40].ToString();
                ddlTipoMot.SelectedItem.Text = dt.Rows[0][41].ToString();
                txtCodProp.Text = dt.Rows[0][42].ToString() + "/" + dt.Rows[0][54].ToString();
                txtPlaca.Text = dt.Rows[0][43].ToString();
                txtReboque1.Text = dt.Rows[0][44].ToString();
                txtReboque2.Text = dt.Rows[0][45].ToString();
                txtTipoVeiculo.Text = dt.Rows[0][46].ToString();
                txtValCartao.Text = dt.Rows[0][47].ToString();
                //ddlJornada.SelectedItem.Text = dt.Rows[0][48].ToString();
                ddlFuncao.SelectedItem.Text = dt.Rows[0][49].ToString();
                txtFrota.Text = dt.Rows[0][50].ToString();
                txtUsuCadastro.Text = dt.Rows[0][51].ToString();
                lblDtCadastro.Text = dt.Rows[0][52].ToString();
                txtVAlExameTox.Text = dt.Rows[0][53].ToString();
                hdfRegiao.Value = dt.Rows[0][55].ToString();
                ddlEstNasc.Items.Insert(0, new ListItem(dt.Rows[0][55].ToString()));
                txtFormCNH.Text = dt.Rows[0][56].ToString();
                ddlCNH.Items.Insert(0, new ListItem(dt.Rows[0][57].ToString()));
                //hdfCnh.Value = dt.Rows[0][57].ToString();
                ddlMunicCnh.Items.Insert(0, new ListItem(dt.Rows[0][58].ToString()));
                txtVAlMoop.Text = dt.Rows[0][59].ToString();
                txtCracha.Text = dt.Rows[0][60].ToString();
                ddlRegioes.SelectedItem.Text = dt.Rows[0][61].ToString();
                txtINSS.Text = dt.Rows[0][62].ToString();
                txtCaminhoFoto.Text = dt.Rows[0][63].ToString();
                fotoMotorista = dt.Rows[0][63].ToString();
                

                //SALVAR A FOTO DO MOTORISTA
                // aspx
                //< asp:FileUpload ID = "FileUpload1" runat = "server" />
                //< asp:Button ID = "btnSalvar" runat = "server" Text = "Salvar Imagem" OnClick = "btnSalvar_Click" />
                //< asp:Label ID = "lblMensagem" runat = "server" ForeColor = "Green" />

                // csharp
                //        protected void btnSalvar_Click(object sender, EventArgs e)
                //{
                //    if (FileUpload1.HasFile)
                //    {
                //        try
                //        {
                //            // Nome original
                //            string originalName = Path.GetFileName(FileUpload1.FileName);

                //            // Novo nome (por exemplo, com timestamp)
                //            string novoNome = "img_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + Path.GetExtension(originalName);

                //            // Caminho da nova pasta (por exemplo: ~/FotosSalvas/)
                //            string pastaDestino = Server.MapPath("~/FotosSalvas/");

                //            // Cria a pasta se não existir
                //            if (!Directory.Exists(pastaDestino))
                //            {
                //                Directory.CreateDirectory(pastaDestino);
                //            }

                //            // Caminho completo para salvar a cópia
                //            string caminhoCompleto = Path.Combine(pastaDestino, novoNome);

                //            // Salva o arquivo com novo nome na nova pasta
                //            FileUpload1.SaveAs(caminhoCompleto);

                //            lblMensagem.Text = "Imagem salva com sucesso como " + novoNome;
                //        }
                //        catch (Exception ex)
                //        {
                //            lblMensagem.ForeColor = System.Drawing.Color.Red;
                //            lblMensagem.Text = "Erro ao salvar: " + ex.Message;
                //        }
                //    }
                //    else
                //    {
                //        lblMensagem.ForeColor = System.Drawing.Color.Red;
                //        lblMensagem.Text = "Nenhuma imagem selecionada.";
                //    }
                //}
            }
        }
        protected void btnSalvar1_Click(object sender, EventArgs e)
        {
            string id = HttpContext.Current.Request.QueryString["id"];

            // Verifica se o ID foi fornecido e é um número válido
            if (string.IsNullOrEmpty(id) || !int.TryParse(id, out int idConvertido))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", "alert('ID inválido ou não fornecido.');", true);
                return;
            }

            string sql = @"UPDATE tbmotoristas SET         
                         nommot = @nommot,
                         status = @status,
                         emissaorg = @emissaorg,
                         numrg = @numrg,
                         cargo = @cargo,
                         nucleo = @nucleo,
                         orgaorg = @orgaorg,
                         cpf = @cpf,
                         numregcnh = @numregcnh,
                         codsegurancacnh = @codsegurancacnh,
                         venccnh = @venccnh,
                         codliberacao = @codliberacao,
                         numpis = @numpis,
                         endmot = @endmot,
                         baimot = @baimot,
                         cidmot = @cidmot,
                         ufmot = @ufmot,
                         cepmot = @cepmot,
                         fone3 = @fone3,
                         fone2 = @fone2,
                         validade = @validade,
                         dtnasc = @dtnasc,
                         estcivil = @estcivil,
                         sexo = @sexo,
                         horario = @horario,
                         nomepai = @nomepai,
                         nomemae = @nomemae,
                         codtra = @codtra,
                         transp = @transp,
                         cadmot = @cadmot,
                         inativo = @inativo,
                         dtinativo = @dtinativo,
                         historico = @historico,
                         alterado = @alterado,
                         dataalteracao = @dataalteracao,
                         cartaomot = @cartaomot,
                         naturalmot = @naturalmot,
                         numero = @numero,
                         complemento = @complemento,
                         tipomot = @tipomot,
                         codprop = @codprop,
                         placa = @placa,
                         reboque1 = @reboque1,
                         reboque2 = @reboque2,
                         tipoveiculo = @tipoveiculo,
                         venccartao = @venccartao,
                         funcao = @funcao,
                         frota = @frota,
                         venceti = @venceti,
                         codvei = @codvei,
                         ufnascimento = @ufnascimento,
                         formulariocnh = @formulariocnh,
                         ufcnh = @ufcnh,
                         municipiocnh = @municipiocnh,
                         vencmoop = @vencmoop,
                         cracha = @cracha,
                         regiao = @regiao,
                         numinss = @numinss,
                         caminhofoto = @caminhofoto
                        WHERE id = @id";
            try
            {
                using (SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    // Adiciona os parâmetros
                   
                    cmd.Parameters.AddWithValue("@nommot", txtNomMot.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@status", ddlStatus.SelectedValue.ToUpper());
                    cmd.Parameters.AddWithValue("@emissaorg", DateTime.Parse(txtDtEmissao.Text).ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@numrg", txtRG.Text);
                    cmd.Parameters.AddWithValue("@cargo", ddlCargo.SelectedItem.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@nucleo", cbFiliais.SelectedItem.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@orgaorg", txtEmissor.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@cpf", txtCPF.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@numregcnh", txtRegCNH.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@codsegurancacnh", txtCodSeguranca.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@venccnh", DateTime.Parse(txtValCNH.Text.ToUpper()).ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@codliberacao", txtCodLibRisco.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@numpis", txtPIS.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@endmot", txtEndCli.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@baimot", txtBaiCli.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@cidmot", txtCidCli.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@ufmot", txtEstCli.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@cepmot", txtCepCli.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@fone3", txtFixo.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@fone2", txtCelular.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@validade", txtValLibRisco.Text);
                    cmd.Parameters.AddWithValue("@dtnasc", DateTime.Parse(txtDtNasc.Text).ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@estcivil", ddlEstCivil.SelectedItem.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@sexo", ddlSexo.SelectedItem.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@horario", ddlJornada.SelectedItem.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@nomepai", txtNomePai.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@nomemae", txtNomeMae.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@codtra", txtCodTra.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@transp", ddlAgregados.SelectedItem.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@cadmot", DateTime.Parse(txtDtCad.Text).ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@inativo", txtMotivoInativacao.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@dtinativo", txtDtInativacao.Text);
                    cmd.Parameters.AddWithValue("@historico", txtHistorico.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@alterado", Session["UsuarioLogado"].ToString().ToUpper());
                    cmd.Parameters.Add("@dataalteracao", SqlDbType.DateTime2).Value = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                    cmd.Parameters.AddWithValue("@cartaomot", txtCartao.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@naturalmot", ddlMunicipioNasc.SelectedItem.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@numero", txtNumero.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@complemento", txtComplemento.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@tipomot", ddlTipoMot.SelectedItem.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@codprop", txtCodProp.Text.Split('/')[0].ToUpper());
                    cmd.Parameters.AddWithValue("@placa", txtPlaca.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@reboque1", string.IsNullOrEmpty(txtReboque1.Text.ToUpper()) ? (object)DBNull.Value : txtReboque1.Text);
                    cmd.Parameters.AddWithValue("@reboque2", string.IsNullOrEmpty(txtReboque2.Text.ToUpper()) ? (object)DBNull.Value : txtReboque2.Text);
                    cmd.Parameters.AddWithValue("@tipoveiculo", txtTipoVeiculo.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@venccartao", txtValCartao.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@funcao", ddlFuncao.SelectedItem.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@frota", txtFrota.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@venceti", txtVAlExameTox.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@codvei", txtFrota.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@ufnascimento", ddlEstNasc.SelectedItem.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@formulariocnh", txtFormCNH.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@ufcnh", ddlCNH.SelectedItem.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@municipiocnh", ddlMunicCnh.SelectedItem.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@vencmoop", txtVAlMoop.Text);
                    cmd.Parameters.AddWithValue("@cracha", txtCracha.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@regiao", ddlRegioes.SelectedItem.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@numinss", txtINSS.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@id", idConvertido);
                    if (FileUpload1.HasFile)
                    {
                        try
                        {
                            // Nome original
                            string originalName = Path.GetFileName(FileUpload1.FileName);

                            // Novo nome (por exemplo, com timestamp)
                            //string novoNome = "img_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + Path.GetExtension(originalName);
                            string novoNome = txtCodMot.Text.Trim().ToUpper() + Path.GetExtension(originalName);

                            // Caminho da nova pasta (por exemplo: ~/FotosSalvas/)
                            string pastaDestino = Server.MapPath("~/fotos/");

                            // Cria a pasta se não existir
                            if (!Directory.Exists(pastaDestino))
                            {
                                Directory.CreateDirectory(pastaDestino);
                            }

                            // Caminho completo para salvar a cópia
                            string caminhoCompleto = Path.Combine(pastaDestino, novoNome);

                            // Salva o arquivo com novo nome na nova pasta
                            FileUpload1.SaveAs(caminhoCompleto);



                            //lblMensagem.Text = "Imagem salva com sucesso como " + novoNome;
                        }
                        catch (Exception ex)
                        {
                            Response.Write("Erro ao salvar foto: " + ex.Message);
                        }
                    }
                    else
                    {
                        //cmd.Parameters.AddWithValue("@caminhofoto", "/fotos/");
                    }
                    cmd.Parameters.AddWithValue("@caminhofoto", "/fotos/" + txtCodMot.Text.Trim().ToUpper() + ".jpg");

                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        //string mensagem = $"Olá, {txtAltCad.Text}! Código {txtCodMot.Text} atualizado com sucesso. {ddlMunicCnh.SelectedItem.Text.ToUpper()}";
                        //string script = $"alert('{HttpUtility.JavaScriptStringEncode(mensagem)}');";
                        //ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", script, true);

                        Response.Redirect("/dist/pages/ConsultaMotoristas.aspx");
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", "alert('Nenhum registro foi atualizado.');", true);
                    }
                }
            }
            catch (Exception ex)
            {
                string mensagemErro = $"Erro ao atualizar: {HttpUtility.JavaScriptStringEncode(ex.Message)}";
                string script = $"alert('{mensagemErro}');";
                ClientScript.RegisterStartupScript(this.GetType(), "Erro", script, true);
            }
        }
        private void PreencherComboJornada()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT id, descricao FROM tbhorarios";

            // Crie uma conexão com o banco de dados
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                try
                {
                    // Abra a conexão com o banco de dados
                    conn.Open();

                    // Crie o comando SQL
                    SqlCommand cmd = new SqlCommand(query, conn);

                    // Execute o comando e obtenha os dados em um DataReader
                    SqlDataReader reader = cmd.ExecuteReader();

                    // Preencher o ComboBox com os dados do DataReader
                    ddlJornada.DataSource = reader;
                    ddlJornada.DataTextField = "descricao";  // Campo que será mostrado no ComboBox
                    ddlJornada.DataValueField = "id";  // Campo que será o valor de cada item                    
                    ddlJornada.DataBind();  // Realiza o binding dos dados                   
                    ddlJornada.Items.Insert(0, new ListItem("", "0"));
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
        private void CarregarDDLAgregados()
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                conn.Open();
                string query = "SELECT ID, codtra, fantra FROM tbtransportadoras WHERE fl_exclusao is null AND ativa_inativa = 'ATIVO' ORDER BY fantra";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                ddlAgregados.DataSource = reader;
                ddlAgregados.DataTextField = "fantra";  // Campo a ser exibido
                ddlAgregados.DataValueField = "ID";  // Valor associado ao item
                ddlAgregados.DataBind();

                // Adicionar o item padrão
                ddlAgregados.Items.Insert(0, new ListItem("", "0"));
            }
        }
        protected void ddlAgregados_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idSelecionado = int.Parse(ddlAgregados.SelectedValue);

            // Preencher os campos com base no valor selecionado
            if (idSelecionado > 0)
            {
                PreencherCampos(idSelecionado);
            }
            else
            {
                LimparCampos();
            }
        }
        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Preencher os campos com base no valor selecionado
            if (ddlStatus.SelectedItem.Text == "ATIVO")
            {
                motivoInativo.Visible = false;
                dataInativo.Visible = false;
                txtMotivoInativacao.Visible = false;
                txtDtInativacao.Visible = false;
            }
            else
            {
                motivoInativo.Visible = true;
                dataInativo.Visible = true;
                txtMotivoInativacao.Visible = true;
                txtDtInativacao.Visible = true;
            }
        }
        private void PreencherCampos(int id)
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                conn.Open();
                string query = "SELECT codtra, fantra FROM tbtransportadoras WHERE ID = @ID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtCodTra.Text = reader["codtra"].ToString();

                }
            }
        }
        private void LimparCampos()
        {
            txtCodTra.Text = string.Empty;
        }
        private void CarregarCargos()
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                conn.Open();
                string query = "SELECT cod_funcao, nm_funcao FROM tb_funcao ORDER BY nm_funcao";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                ddlCargo.DataSource = reader;
                ddlCargo.DataTextField = "nm_funcao";  // Campo a ser exibido
                ddlCargo.DataValueField = "cod_funcao";  // Valor associado ao item
                ddlCargo.DataBind();

                // Adicionar o item padrão
                ddlCargo.Items.Insert(0, new ListItem("", "0"));
            }
        }       
        protected void ddlCargo_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idSelecionado = int.Parse(ddlCargo.SelectedValue);

            // Preencher os campos com base no valor selecionado
            if (idSelecionado > 0)
            {
                // PreencherCamposCargo(idSelecionado);
            }
            else
            {
                // LimparCamposCidades();
            }
        }
        private void PreencherComboCargo()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT cod_funcao, nm_funcao FROM tb_funcao ORDER BY nm_funcao";

            // Crie uma conexão com o banco de dados
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                try
                {
                    // Abra a conexão com o banco de dados
                    conn.Open();

                    // Crie o comando SQL
                    SqlCommand cmd = new SqlCommand(query, conn);

                    // Execute o comando e obtenha os dados em um DataReader
                    SqlDataReader reader = cmd.ExecuteReader();

                    // Preencher o ComboBox com os dados do DataReader
                    ddlCargo.DataSource = reader;
                    ddlCargo.DataTextField = "nm_funcao";  // Campo que será mostrado no ComboBox
                    ddlCargo.DataValueField = "cod_funcao";  // Campo que será o valor de cada item                    
                    ddlCargo.DataBind();  // Realiza o binding dos dados                   
                                          // Adicionar o item padrão
                    ddlCargo.Items.Insert(0, new ListItem("", "0"));
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
        private void LimparCamposCargo()
        {
            ddlCargo.Text = string.Empty;
        }
        private void PreencherComboFiliais()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT codigo, descricao FROM tbempresa";

            // Crie uma conexão com o banco de dados
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                try
                {
                    // Abra a conexão com o banco de dados
                    conn.Open();

                    // Crie o comando SQL
                    SqlCommand cmd = new SqlCommand(query, conn);

                    // Execute o comando e obtenha os dados em um DataReader
                    SqlDataReader reader = cmd.ExecuteReader();

                    // Preencher o ComboBox com os dados do DataReader
                    cbFiliais.DataSource = reader;
                    cbFiliais.DataTextField = "descricao";  // Campo que será mostrado no ComboBox
                    cbFiliais.DataValueField = "codigo";  // Campo que será o valor de cada item                    
                    cbFiliais.DataBind();  // Realiza o binding dos dados                   
                    cbFiliais.Items.Insert(0, new ListItem("", "0"));
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
        // Função para preencher os campos com os dados do banco
        private void PreencherCamposProp(int id)
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                conn.Open();
                string query = "SELECT codtra, fantra, antt FROM tbtransportadoras WHERE ID = @ID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtCodTra.Text = reader["codtra"].ToString();
                    //ddlAgregados.Text = reader["fantra"].ToString();
                    //txtAntt.Text = reader["antt"].ToString();
                }
            }
        }

        // Função para limpar os campos
        private void LimparCamposProp()
        {
            txtCodTra.Text = string.Empty;
            //txtAntt.Text = string.Empty;
        }

        protected void txtCodTra_TextChanged(object sender, EventArgs e)
        {
            if (txtCodTra.Text != "")
            {

                string codigoRemetente = txtCodTra.Text.Trim();
                string strConn = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    string query = "SELECT codtra, fantra, antt FROM tbtransportadoras WHERE codtra = @Codigo";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Codigo", codigoRemetente);
                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                ddlAgregados.SelectedItem.Text = reader["fantra"].ToString();                                
                                txtCodMot.Focus();
                            }
                            else
                            {
                                ddlAgregados.ClearSelection();                                
                                txtCodTra.Text = string.Empty;
                                // Aciona o Toast via JavaScript

                                ScriptManager.RegisterStartupScript(this, GetType(), "toastNaoEncontrado", "mostrarToastNaoEncontrado();", true);
                                txtCodTra.Focus();
                                // Opcional: exibir mensagem ao usuário
                            }
                        }
                    }

                }

            }
        }
        protected void btnCep_Click(object sender, EventArgs e)
        {
            WebCEP cep = new WebCEP(txtCepCli.Text);
            txtBaiCli.Text = cep.Bairro.ToString();
            txtCidCli.Text = cep.Cidade.ToString();
            txtEndCli.Text = cep.TipoLagradouro.ToString() + " " + cep.Lagradouro.ToString();
            txtEstCli.Text = cep.UF.ToString();
            txtNumero.Focus();
        }

        protected void txtPlaca_TextChanged(object sender, EventArgs e)
        {
            if (txtPlaca.Text != "")
            {

                string codigoPlaca = txtPlaca.Text.ToUpper().Trim();
                string strConn = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    string query = "SELECT codvei, tipvei, plavei, motorista FROM tbveiculos WHERE plavei = @Codigo";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Codigo", codigoPlaca);
                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                if (reader["motorista"].ToString() == string.Empty)
                                {
                                    txtTipoVeiculo.Text = reader["tipvei"].ToString();
                                    txtFrota.Text = reader["codvei"].ToString();
                                    txtReboque1.Focus();
                                }
                                else 
                                {
                                    string nomeUsuario = txtUsuCadastro.Text;

                                    string linha1 = "Olá, " + nomeUsuario + "!";
                                    string linha2 = "Veículo com o motorista: " + reader["tipvei"].ToString() + ".";
                                    string linha3 = "Escolha outro veículo, ou retire o motorista atrelado.";
                                    //string linha4 = "Unidade: " + unidade + ". Por favor, verifique.";

                                    // Concatenando as linhas com '\n' para criar a mensagem
                                    string mensagem = $"{linha1}\n{linha2}\n{linha3}";

                                    string mensagemCodificada = HttpUtility.JavaScriptStringEncode(mensagem);
                                    //// Gerando o script JavaScript para exibir o alerta
                                    string script = $"alert('{mensagemCodificada}');";

                                    //// Registrando o script para execução no lado do cliente
                                    ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", script, true);

                                    txtPlaca.Text = "";
                                    txtPlaca.Focus();
                                }

                            }
                            else
                            {
                                txtPlaca.Text = string.Empty;
                                // Aciona o Toast via JavaScript

                                ScriptManager.RegisterStartupScript(this, GetType(), "toastNaoEncontrado", "mostrarToastNaoEncontrado();", true);
                                txtCodTra.Focus();
                                // Opcional: exibir mensagem ao usuário
                            }
                        }
                    }

                }

            }
        }
    }
}
