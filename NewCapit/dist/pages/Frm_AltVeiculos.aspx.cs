﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using NPOI.SS.Formula.Functions;
using static System.Net.Mime.MediaTypeNames;
using static NPOI.HSSF.Util.HSSFColor;

namespace NewCapit
{
    public partial class Frm_AltVeiculos : System.Web.UI.Page
    {       
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conexao"].ToString());
        string id;
        string id_uf;        
        DateTime dataHoraAtual = DateTime.Now;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                if (Session["UsuarioLogado"] != null)
                {
                    string nomeUsuario = Session["UsuarioLogado"].ToString();
                    var lblUsuario = nomeUsuario;

                    txtAlteradoPor.Text = nomeUsuario;
                    txtUsuarioAtual.Text = nomeUsuario;
                }
                else
                {
                    var lblUsuario = "<Usuário>";
                    txtAlteradoPor.Text = lblUsuario;
                }

                PreencherComboComposicao();
                CarregarDDLAgregados();
                PreencherComboFiliais();
                PreencherComboMarcasVeiculos();
                PreencherComboCoresVeiculos();
                PreencherComboRastreadores();
                PreencherComboMotoristas();
                PreencherComboEstados();
                CarregaDadosDoVeiculo();

                DateTime dataHoraAtual = DateTime.Now;
                txtDtAlteracao.Text = dataHoraAtual.ToString("dd/MM/yyyy HH:mm");

            }

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

        private void PreencherComboMarcasVeiculos()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT id, marca FROM tbmarcasveiculos";

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
                    ddlMarca.DataSource = reader;
                    ddlMarca.DataTextField = "marca";  // Campo que será mostrado no ComboBox
                    ddlMarca.DataValueField = "id";  // Campo que será o valor de cada item                    
                    ddlMarca.DataBind();  // Realiza o binding dos dados                   

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

        private void PreencherComboCoresVeiculos()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT id, cor FROM tbcores";

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
                    ddlCor.DataSource = reader;
                    ddlCor.DataTextField = "cor";  // Campo que será mostrado no ComboBox
                    ddlCor.DataValueField = "id";  // Campo que será o valor de cada item                    
                    ddlCor.DataBind();  // Realiza o binding dos dados                   

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
        private void CarregarCidades(string uf)
        {
            string strConn = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                string query = "SELECT cod_municipio, nome_municipio FROM tbmunicipiosbrasileiros WHERE uf = @UF";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UF", uf);
                conn.Open();
                ddlCidades.DataSource = cmd.ExecuteReader();
                ddlCidades.DataTextField = "nome_municipio";
                ddlCidades.DataValueField = "cod_municipio"; // valor único
                ddlCidades.DataBind();

                ddlCidades.Items.Insert(0, new ListItem("-- Selecione uma cidade --", "0"));
            }
        }
        protected void ddlEstados_SelectedIndexChanged(object sender, EventArgs e)
        {
            string uf = ddlEstados.SelectedValue;
            CarregarCidades(uf);

            // Restaurar cidade se estiver em ViewState
            if (ViewState["CidadeSelecionada"] != null)
            {
                string cidadeId = ViewState["CidadeSelecionada"].ToString();
                if (ddlCidades.Items.FindByValue(cidadeId) != null)
                {
                    ddlCidades.SelectedValue = cidadeId;
                }
            }
        }
        private void PreencherComboRastreadores()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT codRastreador, nomRastreador, codBuonny FROM tbrastreadores";

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
                    ddlTecnologia.DataSource = reader;
                    ddlTecnologia.DataTextField = "nomRastreador";  // Campo que será mostrado no ComboBox
                    ddlTecnologia.DataValueField = "codRastreador";  // Campo que será o valor de cada item                    
                    ddlTecnologia.DataBind();  // Realiza o binding dos dados                   

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
        private void PreencherComboEstados()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT Uf, SiglaUf FROM tbestadosbrasileiros";

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
                    ddlEstados.DataSource = reader;
                    ddlEstados.DataTextField = "SiglaUf";  // Campo que será mostrado no ComboBox
                    ddlEstados.DataValueField = "Uf";  // Campo que será o valor de cada item                    
                    ddlEstados.DataBind();  // Realiza o binding dos dados                   
                    //ddlEstados.Items.Insert(0, new ListItem("", "0"));
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
        protected void ddlTecnologia_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtCodRastreador.Text = ddlTecnologia.SelectedValue.ToString();
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
                    ddlMotorista.DataSource = reader;
                    ddlMotorista.DataTextField = "nommot";
                    ddlMotorista.DataValueField = "codmot";
                    ddlMotorista.DataBind();

                    ddlMotorista.Items.Insert(0, "");

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
        private void cboTipoCarreta_Leave()
        {
            string composicao1 = "CAVALO SIMPLES COM CARRETA VANDERLEIA ABERTA";
            string composicao2 = "CAVALO SIMPLES COM CARRETA SIMPLES TOTAL SIDER";
            string composicao3 = "CAVALO SIMPLES COM CARRETA SIMPLES(LS) ABERTA";
            string composicao4 = "CAVALO SIMPLES COM CARRETA VANDERLEIA TOTAL SIDER";
            string composicao5 = "CAVALO TRUCADO COM CARRETA VANDERLEIA ABERTA";
            string composicao6 = "CAVALO TRUCADO COM CARRETA SIMPLES TOTAL SIDER";
            string composicao7 = "CAVALO TRUCADO COM CARRETA SIMPLES(LS) ABERTA";
            string composicao8 = "CAVALO TRUCADO COM CARRETA VANDERLEIA TOTAL SIDER";
            string composicao9 = "TRUCK";
            string composicao10 = "BITRUCK";
            string composicao11 = "BITREM";
            string composicao12 = "TOCO";
            string composicao13 = "VUC OU 3/4";
            string composicao14 = "CAVALO SIMPLES COM PRANCHA";
            string composicao15 = "CAVALO TRUCADO COM PRANCHA";
            string composicao16 = "UTILITÁRIO/FURGÃO";
            string composicao17 = "FIORINO";            

            string selectedValue = ddlComposicao.SelectedItem.ToString().Trim();
            string tipoComposicao = selectedValue;
            string tara = txtTara.Text.Trim();
            int nTara = 0;
            if (tipoComposicao.Equals(composicao1))
            {
                txtEixos.Text = "05";
                txtLotacao.Text = "46000";
                txtTolerancia.Text = "5";
                int nCapacidade = 46000;
                nTara = int.Parse(txtTara.Text);
                int nPesoLiquido = nCapacidade - nTara;
                int nPesoTolerancia = (nPesoLiquido * 5) / 100;
                int nTotalCarga = nPesoLiquido + nPesoTolerancia;
                txtPBT.Text = nTotalCarga.ToString();
            }
            else if (tipoComposicao.Equals(composicao2))
            {
                txtEixos.Text = "05";
                txtLotacao.Text = "46000";
                txtTolerancia.Text = "5";
                int nCapacidade = 41500;
                nTara = int.Parse(txtTara.Text);
                int nPesoLiquido = nCapacidade - nTara;
                int nPesoTolerancia = (nPesoLiquido * 5) / 100;
                int nTotalCarga = nPesoLiquido + nPesoTolerancia;
                txtPBT.Text = nTotalCarga.ToString();
            }
            else if (tipoComposicao.Equals(composicao3))
            {
                txtEixos.Text = "05";
                txtLotacao.Text = "41500";
                txtTolerancia.Text = "5";
                int nCapacidade = 46000;
                nTara = int.Parse(txtTara.Text);
                int nPesoLiquido = nCapacidade - nTara;
                int nPesoTolerancia = (nPesoLiquido * 5) / 100;
                int nTotalCarga = nPesoLiquido + nPesoTolerancia;
                txtPBT.Text = nTotalCarga.ToString();
            }
            else if (tipoComposicao.Equals(composicao4))
            {
                txtEixos.Text = "05";
                txtLotacao.Text = "46000";
                txtTolerancia.Text = "5";
                int nCapacidade = 46000;
                nTara = int.Parse(txtTara.Text);
                int nPesoLiquido = nCapacidade - nTara;
                int nPesoTolerancia = (nPesoLiquido * 5) / 100;
                int nTotalCarga = nPesoLiquido + nPesoTolerancia;
                txtPBT.Text = nTotalCarga.ToString();
            }
            else if (tipoComposicao.Equals(composicao5))
            {
                
                txtEixos.Text = "06";
                txtLotacao.Text = "53000";
                txtTolerancia.Text = "5";
                int nCapacidade = 53000;
                nTara = Int32.Parse(tara);
                int nPesoLiquido = nCapacidade - nTara;
                int nPesoTolerancia = (nPesoLiquido * 5) / 100;             
            
            }
            else if (tipoComposicao.Equals(composicao6))
            {
                txtEixos.Text = "06";
                txtLotacao.Text = "48500";
                txtTolerancia.Text = "5";
                int nCapacidade = 48500;
                nTara = int.Parse(txtTara.Text);
                int nPesoLiquido = nCapacidade - nTara;
                int nPesoTolerancia = (nPesoLiquido * 5) / 100;

            }
            else if (tipoComposicao.Equals(composicao7))
            {
                txtEixos.Text = "06";
                txtLotacao.Text = "48500";
                txtTolerancia.Text = "5";
                int nCapacidade = 48500;
                nTara = int.Parse(txtTara.Text);
                int nPesoLiquido = nCapacidade - nTara;
                int nPesoTolerancia = (nPesoLiquido * 5) / 100;

            }
            else if (tipoComposicao.Equals(composicao8))
            {
                txtEixos.Text = "06";
                txtLotacao.Text = "53000";
                txtTolerancia.Text = "5";
                int nCapacidade = 53000;
                nTara = int.Parse(txtTara.Text);
                int nPesoLiquido = nCapacidade - nTara;
                int nPesoTolerancia = (nPesoLiquido * 5) / 100;

            }
            else if (tipoComposicao.Equals(composicao9))
            {
                txtEixos.Text = "03";
                txtLotacao.Text = "23000";
                txtTolerancia.Text = "5";
                int nCapacidade = 23000;
                nTara = int.Parse(txtTara.Text);
                int nPesoLiquido = nCapacidade - nTara;
                int nPesoTolerancia = (nPesoLiquido * 5) / 100;

            }
            else if (tipoComposicao.Equals(composicao10))
            {
                txtEixos.Text = "04";
                txtLotacao.Text = "29000";
                txtTolerancia.Text = "5";
                int nCapacidade = 29000;
                nTara = int.Parse(txtTara.Text);
                int nPesoLiquido = nCapacidade - nTara;
                int nPesoTolerancia = (nPesoLiquido * 5) / 100;

            }
            else if (tipoComposicao.Equals(composicao11))
            {
                txtEixos.Text = "07";
                txtLotacao.Text = "57000";
                txtTolerancia.Text = "5";
                int nCapacidade = 57000;
                nTara = int.Parse(txtTara.Text);
                int nPesoLiquido = nCapacidade - nTara;
                int nPesoTolerancia = (nPesoLiquido * 5) / 100;

            }
            else if (tipoComposicao.Equals(composicao12))
            {
                txtEixos.Text = "02";
                txtLotacao.Text = "16000";
                txtTolerancia.Text = "5";
                int nCapacidade = 16000;
                nTara = int.Parse(txtTara.Text);
                int nPesoLiquido = nCapacidade - nTara;
                int nPesoTolerancia = (nPesoLiquido * 5) / 100;

            }
            else if (tipoComposicao.Equals(composicao13))
            {
                txtEixos.Text = "06";
                txtLotacao.Text = "3000";
                txtTolerancia.Text = "5";
                int nCapacidade = 3000;
                nTara = int.Parse(txtTara.Text);
                int nPesoLiquido = nCapacidade - nTara;
                int nPesoTolerancia = (nPesoLiquido * 5) / 100;

            }
            else if (tipoComposicao.Equals(composicao14))
            {
                txtEixos.Text = "05";
                txtLotacao.Text = "23000";
                txtTolerancia.Text = "5";
                int nCapacidade = 23000;
                nTara = int.Parse(txtTara.Text);
                int nPesoLiquido = nCapacidade - nTara;
                int nPesoTolerancia = (nPesoLiquido * 5) / 100;

            }
            else if (tipoComposicao.Equals(composicao15))
            {
                txtEixos.Text = "06";
                txtLotacao.Text = "23000";
                txtTolerancia.Text = "5";
                int nCapacidade = 23000;
                nTara = int.Parse(txtTara.Text);
                int nPesoLiquido = nCapacidade - nTara;
                int nPesoTolerancia = (nPesoLiquido * 5) / 100;

            }
            else if (tipoComposicao.Equals(composicao16))
            {
                txtEixos.Text = "02";
                txtLotacao.Text = "1200";
                txtTolerancia.Text = "5";
                int nCapacidade = 1200;
                nTara = int.Parse(txtTara.Text);
                int nPesoLiquido = nCapacidade - nTara;
                int nPesoTolerancia = (nPesoLiquido * 5) / 100;

            }
            else if (tipoComposicao.Equals(composicao17))
            {
                txtEixos.Text = "02";
                txtLotacao.Text = "630";
                txtTolerancia.Text = "5";
                int nCapacidade = 630;
                nTara = int.Parse(txtTara.Text);
                int nPesoLiquido = nCapacidade - nTara;
                int nPesoTolerancia = (nPesoLiquido * 5) / 100;

            }
        }
        protected void ddlComposicao_SelectedIndexChanged2(object sender, EventArgs e)
        {
            //if (txtTara.Text != string.Empty)
            //{
            //    cboTipoCarreta_Leave();
            //}
            //else
            //{
            //    string retorno = "É necessário digitar o valor no campo Tara!";
            //    System.Text.StringBuilder sb = new System.Text.StringBuilder();
            //    sb.Append("<script type = 'text/javascript'>");
            //    sb.Append("window.onload=function(){");
            //    sb.Append("alert('");
            //    sb.Append(retorno);
            //    sb.Append("')};");
            //    sb.Append("</script>");
            //    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
            //    ddlComposicao.SelectedValue = "";
            //}
            //cboTipoCarreta_Leave();

            string composicao1 = "CAVALO SIMPLES COM CARRETA VANDERLEIA ABERTA";
            string composicao2 = "CAVALO SIMPLES COM CARRETA SIMPLES TOTAL SIDER";
            string composicao3 = "CAVALO SIMPLES COM CARRETA SIMPLES(LS) ABERTA";
            string composicao4 = "CAVALO SIMPLES COM CARRETA VANDERLEIA TOTAL SIDER";
            string composicao5 = "CAVALO TRUCADO COM CARRETA VANDERLEIA ABERTA";
            string composicao6 = "CAVALO TRUCADO COM CARRETA SIMPLES TOTAL SIDER";
            string composicao7 = "CAVALO TRUCADO COM CARRETA SIMPLES(LS) ABERTA";
            string composicao8 = "CAVALO TRUCADO COM CARRETA VANDERLEIA TOTAL SIDER";
            string composicao9 = "TRUCK";
            string composicao10 = "BITRUCK";
            string composicao11 = "BITREM";
            string composicao12 = "TOCO";
            string composicao13 = "VUC OU 3/4";
            string composicao14 = "CAVALO SIMPLES COM PRANCHA";
            string composicao15 = "CAVALO TRUCADO COM PRANCHA";
            string composicao16 = "UTILITÁRIO/FURGÃO";
            string composicao17 = "FIORINO";

            string selectedValue = ddlComposicao.SelectedItem.ToString().Trim();
            string tipoComposicao = selectedValue;
            string tara = txtTara.Text;
            
            int nTara = 0;
            if (tipoComposicao.Equals(composicao1))
            {
                txtEixos.Text = "05";
                txtLotacao.Text = "46000";
                txtTolerancia.Text = "5";
                int nCapacidade = 46000;
                nTara = int.Parse(txtTara.Text);
                int nPesoLiquido = nCapacidade - nTara;
                int nPesoTolerancia = (nPesoLiquido * 5) / 100;
                int nTotalCarga = nPesoLiquido + nPesoTolerancia;
                txtPBT.Text = nTotalCarga.ToString();
            }
            else if (tipoComposicao.Equals(composicao2))
            {
                txtEixos.Text = "05";
                txtLotacao.Text = "46000";
                txtTolerancia.Text = "5";
                int nCapacidade = 41500;
                nTara = int.Parse(txtTara.Text);
                int nPesoLiquido = nCapacidade - nTara;
                int nPesoTolerancia = (nPesoLiquido * 5) / 100;
                int nTotalCarga = nPesoLiquido + nPesoTolerancia;
                txtPBT.Text = nTotalCarga.ToString();
            }
            else if (tipoComposicao.Equals(composicao3))
            {
                txtEixos.Text = "05";
                txtLotacao.Text = "41500";
                txtTolerancia.Text = "5";
                int nCapacidade = 46000;
                nTara = int.Parse(txtTara.Text);
                int nPesoLiquido = nCapacidade - nTara;
                int nPesoTolerancia = (nPesoLiquido * 5) / 100;
                int nTotalCarga = nPesoLiquido + nPesoTolerancia;
                txtPBT.Text = nTotalCarga.ToString();
            }
            else if (tipoComposicao.Equals(composicao4))
            {
                txtEixos.Text = "05";
                txtLotacao.Text = "46000";
                txtTolerancia.Text = "5";
                int nCapacidade = 46000;
                nTara = int.Parse(txtTara.Text);
                int nPesoLiquido = nCapacidade - nTara;
                int nPesoTolerancia = (nPesoLiquido * 5) / 100;
                int nTotalCarga = nPesoLiquido + nPesoTolerancia;
                txtPBT.Text = nTotalCarga.ToString();
            }
            else if (tipoComposicao.Equals(composicao5))
            {
                tara = txtTara.Text.Trim();
                txtEixos.Text = "06";
                txtLotacao.Text = "53000";
                txtTolerancia.Text = "5";
                int nCapacidade = 53000;
                nTara = int.Parse(tara);
                int nPesoLiquido = nCapacidade - nTara;
                int nPesoTolerancia = (nPesoLiquido * 5) / 100;

            }
            else if (tipoComposicao.Equals(composicao6))
            {
                txtEixos.Text = "06";
                txtLotacao.Text = "48500";
                txtTolerancia.Text = "5";
                int nCapacidade = 48500;
                nTara = int.Parse(txtTara.Text);
                int nPesoLiquido = nCapacidade - nTara;
                int nPesoTolerancia = (nPesoLiquido * 5) / 100;

            }
            else if (tipoComposicao.Equals(composicao7))
            {
                txtEixos.Text = "06";
                txtLotacao.Text = "48500";
                txtTolerancia.Text = "5";
                int nCapacidade = 48500;
                nTara = int.Parse(txtTara.Text);
                int nPesoLiquido = nCapacidade - nTara;
                int nPesoTolerancia = (nPesoLiquido * 5) / 100;

            }
            else if (tipoComposicao.Equals(composicao8))
            {
                txtEixos.Text = "06";
                txtLotacao.Text = "53000";
                txtTolerancia.Text = "5";
                int nCapacidade = 53000;
                nTara = int.Parse(txtTara.Text);
                int nPesoLiquido = nCapacidade - nTara;
                int nPesoTolerancia = (nPesoLiquido * 5) / 100;

            }
            else if (tipoComposicao.Equals(composicao9))
            {
                txtEixos.Text = "03";
                txtLotacao.Text = "23000";
                txtTolerancia.Text = "5";
                int nCapacidade = 23000;
                nTara = int.Parse(txtTara.Text);
                int nPesoLiquido = nCapacidade - nTara;
                int nPesoTolerancia = (nPesoLiquido * 5) / 100;

            }
            else if (tipoComposicao.Equals(composicao10))
            {
                txtEixos.Text = "04";
                txtLotacao.Text = "29000";
                txtTolerancia.Text = "5";
                int nCapacidade = 29000;
                nTara = int.Parse(txtTara.Text);
                int nPesoLiquido = nCapacidade - nTara;
                int nPesoTolerancia = (nPesoLiquido * 5) / 100;

            }
            else if (tipoComposicao.Equals(composicao11))
            {
                txtEixos.Text = "07";
                txtLotacao.Text = "57000";
                txtTolerancia.Text = "5";
                int nCapacidade = 57000;
                nTara = int.Parse(txtTara.Text);
                int nPesoLiquido = nCapacidade - nTara;
                int nPesoTolerancia = (nPesoLiquido * 5) / 100;

            }
            else if (tipoComposicao.Equals(composicao12))
            {
                txtEixos.Text = "02";
                txtLotacao.Text = "16000";
                txtTolerancia.Text = "5";
                int nCapacidade = 16000;
                nTara = int.Parse(txtTara.Text);
                int nPesoLiquido = nCapacidade - nTara;
                int nPesoTolerancia = (nPesoLiquido * 5) / 100;

            }
            else if (tipoComposicao.Equals(composicao13))
            {
                txtEixos.Text = "02";
                txtLotacao.Text = "3000";
                txtTolerancia.Text = "5";
                int nCapacidade = 3000;
                nTara = int.Parse(txtTara.Text);
                int nPesoLiquido = nCapacidade - nTara;
                int nPesoTolerancia = (nPesoLiquido * 5) / 100;

            }
            else if (tipoComposicao.Equals(composicao14))
            {
                txtEixos.Text = "05";
                txtLotacao.Text = "23000";
                txtTolerancia.Text = "5";
                int nCapacidade = 23000;
                nTara = int.Parse(txtTara.Text);
                int nPesoLiquido = nCapacidade - nTara;
                int nPesoTolerancia = (nPesoLiquido * 5) / 100;

            }
            else if (tipoComposicao.Equals(composicao15))
            {
                txtEixos.Text = "06";
                txtLotacao.Text = "23000";
                txtTolerancia.Text = "5";
                int nCapacidade = 23000;
                nTara = int.Parse(txtTara.Text);
                int nPesoLiquido = nCapacidade - nTara;
                int nPesoTolerancia = (nPesoLiquido * 5) / 100;

            }
            else if (tipoComposicao.Equals(composicao16))
            {
                txtEixos.Text = "02";
                txtLotacao.Text = "1200";
                txtTolerancia.Text = "5";
                int nCapacidade = 1200;
                nTara = int.Parse(txtTara.Text);
                int nPesoLiquido = nCapacidade - nTara;
                int nPesoTolerancia = (nPesoLiquido * 5) / 100;

            }
            else if (tipoComposicao.Equals(composicao17))
            {
                txtEixos.Text = "02";
                txtLotacao.Text = "630";
                txtTolerancia.Text = "5";
                int nCapacidade = 630;
                nTara = int.Parse(txtTara.Text);
                int nPesoLiquido = nCapacidade - nTara;
                int nPesoTolerancia = (nPesoLiquido * 5) / 100;

            }

        }
        private void PreencherComboComposicao()
        {
            // Consulta SQL que retorna os dados desejados
            string query = "SELECT ID, composicao, eixos, pbt, tolerancia FROM tbcomposicaoveiculo";

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
                    ddlComposicao.DataSource = reader;
                    ddlComposicao.DataTextField = "composicao";  // Campo que será mostrado no ComboBox
                    ddlComposicao.DataValueField = "ID";  // Campo que será o valor de cada item                    
                    ddlComposicao.DataBind();  // Realiza o binding dos dados                   

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
        public void CarregaDadosDoVeiculo()
        {
           
            if (HttpContext.Current.Request.QueryString["id"].ToString() != "")
            {
                id = HttpContext.Current.Request.QueryString["id"].ToString();
            }

            string sql = "SELECT * FROM tbveiculos WHERE id = @id";

            try
            {
                using (SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    DataTable dt = new DataTable();

                    using (SqlDataAdapter adpt = new SqlDataAdapter(cmd))
                    {
                        con.Open();
                        adpt.Fill(dt);
                    }

                    // Verifica se encontrou resultados
                    if (dt.Rows.Count == 0)
                    {
                        // Log ou mensagem indicando que não encontrou o registro
                        return;
                    }

                    DataRow row = dt.Rows[0];

                    // Método auxiliar para evitar exceções de valores nulos
                    string GetValue(DataRow r, int index) => r[index] == DBNull.Value ? string.Empty : r[index].ToString();

                    txtCodVei.Text = GetValue(row, 1);
                    cboTipo.Items.Insert(0, GetValue(row, 2));
                    ddlTipo.Items.Insert(0, GetValue(row, 3));
                    txtModelo.Text = GetValue(row, 4);
                    txtAno.Text = GetValue(row, 5);                    
                    txtDtcVei.Text = DateTime.Parse(dt.Rows[0][6].ToString()).ToString("dd/MM/yyyy");
                    cbFiliais.Items.Insert(0, GetValue(row, 7));
                    ddlSituacao.Items.Insert(0, GetValue(row, 8));
                    txtPlaca.Text = GetValue(row, 9);
                    txtReb1.Text = GetValue(row, 10);
                    txtReb2.Text = GetValue(row, 11);
                    //ddlComposicao.Items.Insert(0, GetValue(row, 12));
                    ddlComposicao.Items.Insert(0, GetValue(row, 12));
                    ddlCarreta.Items.Insert(0, GetValue(row, 13));                    
                    ddlMonitoramento.Items.Insert(0, GetValue(row, 14));
                    txtCodRastreador.Text = GetValue(row, 15);
                    ddlTecnologia.Items.Insert(0, GetValue(row, 16));
                    txtId.Text = GetValue(row, 17);
                    txtCargaLiq.Text = GetValue(row, 18);
                    txtEixos.Text = GetValue(row, 19);
                    txtTara.Text = GetValue(row, 20);
                    txtTolerancia.Text = GetValue(row, 21);
                    txtPBT.Text = GetValue(row, 22);
                    txtCodMot.Text = GetValue(row, 27);
                    ddlMotorista.Items.Insert(0, GetValue(row, 28));
                    txtCodTra.Text = GetValue(row, 29);
                    ddlAgregados.Items.Insert(0, GetValue(row, 30));
                    txtOpacidade.Text = GetValue(row, 31);
                    txtCadastradoPor.Text = GetValue(row, 32);
                    txtDtCadastro.Text = GetValue(row, 33);
                    txtVencCET.Text = GetValue(row, 36);
                    txtProtocoloCET.Text = GetValue(row, 37);
                    txtLicenciamento.Text = GetValue(row, 38);
                    txtCronotacografo.Text = DateTime.Parse(dt.Rows[0][39].ToString()).ToString("dd/MM/yyyy");
                    ddlMarca.Items.Insert(0, GetValue(row, 40));
                    txtRenavam.Text = GetValue(row, 41);
                    ddlCor.Items.Insert(0, GetValue(row, 42));
                    ddlComunicacao.Items.Insert(0, GetValue(row, 43));
                    txtAntt.Text = GetValue(row, 44);
                    // numeroReb1.Text = GetValue(row, 45);
                    // numeroReb2.Text = GetValue(row, 46); 
                    //ddlEstados.Items.Insert(0, new ListItem(dt.Rows[0][47].ToString(),""));
                    //ddlCidades.Items.Insert(0, new ListItem(dt.Rows[0][48].ToString(),""));
                    ddlEstados.Items.Insert(0, GetValue(row, 47));
                    ddlCidades.Items.Insert(0, GetValue(row, 48));
                    txtLotacao.Text = GetValue(row, 49);
                    txtComprimento.Text = GetValue(row, 50);
                    txtLargura.Text = GetValue(row, 51);
                    txtAltura.Text = GetValue(row, 52);
                    txtPlacaAnt.Text = GetValue(row, 53);
                    txtCodigo.Text = GetValue(row, 54);
                    txtTipoSeguro.Text = GetValue(row, 55);
                    txtSeguradora.Text = GetValue(row, 56);
                    txtApolice.Text = GetValue(row, 57);
                    txtValidadeApolice.Text = GetValue(row, 58);
                    txtValorFranquia.Text = GetValue(row, 59);
                    ddlTacografo.Items.Insert(0, GetValue(row, 60));
                    ddlModeloTacografo.Items.Insert(0, GetValue(row, 61));
                    txtDataAquisicao.Text = GetValue(row, 62);
                    txtControlePatrimonio.Text = GetValue(row, 63);
                    txtChassi.Text = GetValue(row, 64);
                    

                }
            }
            catch (Exception ex)
            {
                // Log ou tratamento do erro
                // Exemplo: ex.Message
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

            string sql = @"UPDATE tbveiculos SET         
                            tipvei = @tipvei,
                            tipoveiculo = @tipoveiculo,
                            modelo = @modelo,
                            ano = @ano,
                            nucleo = @nucleo,
                            ativo_inativo = @ativo_inativo,
                            plavei = @plavei,
                            reboque1 = @reboque1,
                            reboque2 = @reboque2,
                            tipocarreta = @tipocarreta,
                            tiporeboque = @tiporeboque,
                            rastreamento = @rastreamento,
                            codrastreador = @codrastreador,
                            eixos = @eixos,
                            cap = @cap, 
                            tara = @tara,
                            tolerancia = @tolerancia,
                            pbt = @pbt,                            
                            codmot = @codmot,
                            motorista = @motorista,
                            codtra = @codtra,
                            transp = @transp,
                            vencimentolaudofumaca = @vencimentolaudofumaca,
                            usualt = @usualt,
                            dtcalt = @dtcalt,
                            protocolocet = @protocolocet,
                            venclicencacet = @venclicencacet,
                            venclicenciamento = @venclicenciamento,
                            venccronotacografo = @venccronotacografo,
                            marca = @marca,
                            renavan = @renavan,
                            cor = @cor,
                            comunicacao = @comunicacao,
                            antt = @antt,
                            ufplaca = @ufplaca,
                            cidplaca = @cidplaca,  
                            lotacao = @lotacao,
                            comprimento = @comprimento,
                            largura = @largura,
                            altura = @altura, 
                            placaant = @placaant,
                            tacografo = @tacografo,
                            modelotacografo = @modelotacografo,
                            dataaquisicao = @dataaquisicao, 
                            controlepatrimonio = @controlepatrimonio,
                            chassi = @chassi
                        WHERE id = @id";
            try
            {
                using (SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    // Adiciona os parâmetros                    
                    cmd.Parameters.AddWithValue("@tipvei", cboTipo.SelectedValue.ToUpper());
                    cmd.Parameters.AddWithValue("@tipoveiculo", ddlTipo.SelectedItem.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@modelo", txtModelo.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@ano", txtAno.Text);
                    cmd.Parameters.AddWithValue("@nucleo", cbFiliais.SelectedItem.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@ativo_inativo", ddlSituacao.SelectedItem.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@plavei", txtPlaca.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@reboque1", string.IsNullOrEmpty(txtReb1.Text.ToUpper()) ? (object)DBNull.Value : txtReb1.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@reboque2", string.IsNullOrEmpty(txtReb2.Text.ToUpper()) ? (object)DBNull.Value : txtReb2.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@tiporeboque", ddlCarreta.SelectedItem.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@tipocarreta", ddlComposicao.SelectedItem.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@rastreamento", ddlMonitoramento.SelectedItem.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@codrastreador", txtCodRastreador.Text);
                    cmd.Parameters.AddWithValue("@eixos", txtEixos.Text);
                    cmd.Parameters.AddWithValue("@cap", txtCargaLiq.Text);
                    cmd.Parameters.AddWithValue("@tara", txtTara.Text);
                    cmd.Parameters.AddWithValue("@tolerancia", txtTolerancia.Text);
                    cmd.Parameters.AddWithValue("@pbt", txtPBT.Text);
                    cmd.Parameters.AddWithValue("@codmot", txtCodMot.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@motorista", ddlMotorista.SelectedItem.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@codtra", txtCodTra.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@transp", ddlAgregados.SelectedItem.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@vencimentolaudofumaca", txtOpacidade.Text);
                    cmd.Parameters.AddWithValue("@usualt", txtAlteradoPor.Text.Trim().ToUpper()); // Usuário atual
                    cmd.Parameters.AddWithValue("@dtcalt", txtDtAlteracao.Text); // Corrigido para DateTime
                    cmd.Parameters.AddWithValue("@protocolocet", txtProtocoloCET.Text);
                    cmd.Parameters.AddWithValue("@venclicencacet", string.IsNullOrEmpty(txtVencCET.Text) ? (object)DBNull.Value : txtVencCET.Text);
                    cmd.Parameters.AddWithValue("@venclicenciamento", string.IsNullOrEmpty(txtLicenciamento.Text) ? (object)DBNull.Value : txtLicenciamento.Text);
                    cmd.Parameters.AddWithValue("@venccronotacografo", string.IsNullOrWhiteSpace(txtCronotacografo.Text) ? (object)DBNull.Value : DateTime.Parse(txtCronotacografo.Text));
                    //cmd.Parameters.AddWithValue("@venccronotacografo", DateTime.Parse(txtCronotacografo.Text).ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@marca", ddlMarca.SelectedItem.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@renavan", txtRenavam.Text);
                    cmd.Parameters.AddWithValue("@cor", ddlCor.SelectedItem.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@comunicacao", ddlComunicacao.SelectedItem.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@antt", txtAntt.Text);
                    //cmd.Parameters.AddWithValue("@codreb1", numeroReb1.Text);
                    //cmd.Parameters.AddWithValue("@codreb2", numeroReb2.Text);
                    cmd.Parameters.AddWithValue("@ufplaca", ddlEstados.SelectedItem.Text);
                    if (ddlCidades.SelectedItem.Text != null)
                    {
                        cmd.Parameters.AddWithValue("@cidplaca", string.IsNullOrEmpty(ddlCidades.SelectedItem.Text.ToUpper()) ? (object)DBNull.Value : ddlCidades.SelectedItem.Text.ToUpper());
                    }                    
                    cmd.Parameters.AddWithValue("@lotacao", txtLotacao.Text);
                    cmd.Parameters.AddWithValue("@comprimento", txtComprimento.Text);
                    cmd.Parameters.AddWithValue("@largura", txtLargura.Text);
                    cmd.Parameters.AddWithValue("@altura", txtAltura.Text);
                    cmd.Parameters.AddWithValue("@placaant", txtPlacaAnt.Text);
                    cmd.Parameters.AddWithValue("@tacografo", ddlTacografo.SelectedItem.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@modelotacografo", ddlModeloTacografo.SelectedItem.Text.ToUpper());
                    cmd.Parameters.AddWithValue("@dataaquisicao", txtDataAquisicao.Text);
                    cmd.Parameters.AddWithValue("@controlepatrimonio", txtControlePatrimonio.Text);
                    cmd.Parameters.AddWithValue("@chassi", txtChassi.Text);                    
                    cmd.Parameters.AddWithValue("@id", idConvertido);

                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        string mensagem = $"Olá, {txtAlteradoPor.Text}! Código {txtCodTra.Text} atualizado com sucesso.";
                        string script = $"alert('{HttpUtility.JavaScriptStringEncode(mensagem)}');";
                        ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", script, true);

                        Response.Redirect("ConsultaVeiculos.aspx");
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", "alert('Nenhum registro foi atualizado.');", true);
                    }
                }
            }
            catch (SqlException ex)
            {
                //string mensagemErro = $"Erro ao atualizar: {HttpUtility.JavaScriptStringEncode(ex.Message)}";
                //string script = $"alert('{mensagemErro}');";
                //ClientScript.RegisterStartupScript(this.GetType(), "Erro", script, true);

                string erroDetalhado = "Erro ao salvar registro no banco de dados: " + ex.Message;

                // Tentar identificar qual parâmetro pode ter causado o erro
                erroDetalhado += "\nValores enviados:";               
                erroDetalhado += $"\ntipvei = {cboTipo.SelectedValue.ToUpper()}";
                erroDetalhado += $"\ntipoveiculo = {ddlTipo.SelectedItem.Text.ToUpper()}";
                erroDetalhado += $"\nmodelo = {txtModelo.Text.ToUpper()}";
                erroDetalhado += $"\nano = {txtAno.Text}";
                erroDetalhado += $"\nnucleo = {cbFiliais.SelectedItem.Text.ToUpper()}";
                erroDetalhado += $"\nativo_inativo = {ddlSituacao.SelectedItem.Text.ToUpper()}";
                erroDetalhado += $"\nplavei = {txtPlaca.Text.ToUpper()}";
                erroDetalhado += $"\nreboque1 = {txtReb1.Text.ToUpper()}";
                erroDetalhado += $"\nreboque2 = {txtReb2.Text.ToUpper()}";
                erroDetalhado += $"\ntipocarreta = {ddlCarreta.SelectedItem.Text.ToUpper()}";
                erroDetalhado += $"\ntiporeboque = {ddlComposicao.SelectedItem.Text.ToUpper()}";
                erroDetalhado += $"\nrastreamento = {ddlMonitoramento.SelectedItem.Text.ToUpper()}";
                erroDetalhado += $"\ncodrastreador = {txtCodRastreador.Text}";
                erroDetalhado += $"\neixos = {txtEixos.Text.Trim()}";
                erroDetalhado += $"\ncap = {txtLotacao.Text}";
                erroDetalhado += $"\ntara = {txtTara.Text}";
                erroDetalhado += $"\ntolerancia = {txtTolerancia.Text}";
                erroDetalhado += $"\npbt = {txtPBT.Text}";
                erroDetalhado += $"\ncodmot = {txtCodMot.Text.ToUpper()}";
                erroDetalhado += $"\nmotorista = {ddlMotorista.SelectedItem.Text.ToUpper()}";
                erroDetalhado += $"\ncodtra = {txtCodTra.Text.ToUpper()}";
                erroDetalhado += $"\ntransp = {ddlAgregados.SelectedItem.Text.ToUpper()}";
                erroDetalhado += $"\nvencimentolaudofumaca = {txtOpacidade.Text}";
                erroDetalhado += $"\nusualt = {txtAlteradoPor.Text.Trim().ToUpper()}"; // Usuário atual
                erroDetalhado += $"\ndtcalt = {txtDtAlteracao.Text}"; // Corrigido para DateTime
                erroDetalhado += $"\nprotocolocet = {txtProtocoloCET.Text}";
                erroDetalhado += $"\nvenclicencacet = {txtVencCET.Text}";
                erroDetalhado += $"\nvenclicenciamento = {txtLicenciamento.Text}";
                erroDetalhado += $"\nvenccronotacografo = {DateTime.Parse(txtCronotacografo.Text).ToString("yyyy-MM-dd")}";
                erroDetalhado += $"\nmarca = {ddlMarca.SelectedItem.Text.ToUpper()}";
                erroDetalhado += $"\nrenavan = {txtRenavam.Text}";
                erroDetalhado += $"\ncor = {ddlCor.SelectedItem.Text.ToUpper()}";
                erroDetalhado += $"\ncomunicacao = {ddlComunicacao.SelectedItem.Text.ToUpper()}";
                erroDetalhado += $"\nantt = {txtAntt.Text}";
               
                erroDetalhado += $"\nufplaca = {ddlEstados.SelectedItem.Text}";
                erroDetalhado += $"\ncidplaca = {ddlCidades.SelectedItem.Text}";
                erroDetalhado += $"\nlotacao = {txtLotacao.Text}";
                erroDetalhado += $"\ncomprimento = {txtComprimento.Text}";
                erroDetalhado += $"\nlargura = {txtLargura.Text}";
                erroDetalhado += $"\naltura = {txtAltura.Text}";
                erroDetalhado += $"\nplacaant = {txtPlacaAnt.Text}";
                erroDetalhado += $"\ntacografo = {ddlTacografo.SelectedItem.Text.ToUpper()}";
                erroDetalhado += $"\nmodelotacografo = {ddlModeloTacografo.SelectedItem.Text.ToUpper()}";
                erroDetalhado += $"\ndataaquisicao = {txtDataAquisicao.Text}";
                erroDetalhado += $"\ncontrolepatrimonio = {txtControlePatrimonio.Text}";
                erroDetalhado += $"\nchassi = {txtChassi.Text}";
                erroDetalhado += $"\nid = {idConvertido}";

                //// Aciona o Toast via JavaScript
                //ScriptManager.RegisterStartupScript(this, GetType(), "toastNaoEncontrado", "mostrarToastNaoEncontrado();", true);


                //string script = $"alert('{erroDetalhado}');";
                //ClientScript.RegisterStartupScript(this.GetType(), "Erro", script, true);




                // Exibir no log ou em uma Label (por exemplo)
                
                //if (ex.Number == 8152)
                //{
                //    lblErro.Text = "Erro: Valor duplicado em campo único.";
                //    lblErro.Text = erroDetalhado;
                //}
                miDiv.Visible = true;
                lblErro.Text = erroDetalhado;

            }
        }
        private void PreencherComboAgregados(string filtroCodTra = null)
        {
            string query = "SELECT codtra, (CAST(codtra AS VARCHAR) + '-' + fantra) AS Nome FROM tbtransportadoras WHERE fl_exclusao IS NULL AND ativa_inativa = 'ATIVO'";

            if (!string.IsNullOrEmpty(filtroCodTra))
            {
                query += " AND LTRIM(RTRIM(codtra)) = @codtra";
            }

            query += " ORDER BY fantra";

            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                try
                {
                    // Abra a conexão com o banco de dados
                    conn.Open();

                    // Crie o comando SQL
                    SqlCommand cmd = new SqlCommand(query, conn);

                    if (!string.IsNullOrEmpty(filtroCodTra))
                    {
                        cmd.Parameters.AddWithValue("@codtra", filtroCodTra);
                    }

                    SqlDataReader reader = cmd.ExecuteReader();

                    // Preencher o ComboBox com os dados do DataReader
                    ddlAgregados.DataSource = reader;
                    ddlAgregados.DataTextField = "Nome";
                    ddlAgregados.DataValueField = "codtra";
                    ddlAgregados.DataBind();

                    ddlAgregados.Items.Insert(0, "");

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
        protected void ddlMotorista_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtCodMot.Text = ddlMotorista.SelectedValue;

            using (SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                string sql = "SELECT * FROM tbveiculos WHERE codmot = @codmot";
                con.Open();

                SqlDataAdapter da = new SqlDataAdapter(sql, con);
                da.SelectCommand.Parameters.AddWithValue("@codmot", txtCodMot.Text);

                DataTable dt2 = new DataTable();
                da.Fill(dt2);

                if (dt2.Rows.Count > 0)
                {
                    // Registra o script de confirmação no lado do cliente
                    string script = "ConfirmMessage();";
                    ClientScript.RegisterStartupScript(this.GetType(), "ConfirmMessage", script, true);

                    // Verifica a resposta do usuário via txtconformmessageValue
                    if (txtconformmessageValue.Value == "Yes")
                    {
                        string updateSql = "UPDATE tbveiculos SET codmot = NULL, motorista = NULL WHERE id = @id";
                        using (SqlCommand cmd = new SqlCommand(updateSql, con))
                        {
                            cmd.Parameters.AddWithValue("@id", dt2.Rows[0][0].ToString());

                            int rowsAffected = cmd.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                // Exibe mensagem de sucesso
                                string nomeUsuario = txtAlteradoPor.Text;
                                string mensagem = $"Olá, {nomeUsuario}!\nCódigo {txtCodMot.Text}, foi desvinculado do veículo com sucesso.";
                                string mensagemCodificada = HttpUtility.JavaScriptStringEncode(mensagem);
                                string successScript = $"alert('{mensagemCodificada}');";
                                ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeSucesso", successScript, true);
                            }
                            else
                            {
                                // Log ou mensagem indicando falha na atualização
                                string failScript = "alert('Falha ao desvincular o veículo.');";
                                ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeErro", failScript, true);
                            }
                        }
                    }
                    else
                    {
                        ddlMotorista.ClearSelection();
                        txtCodMot.Text=string.Empty;
                    }
                }
            }
        }

        // Função para carregar o DropDownList com dados dos agregados
        private void CarregarDDLAgregados()
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                conn.Open();
                string query = "SELECT ID, fantra FROM tbtransportadoras WHERE fl_exclusao is null AND ativa_inativa = 'ATIVO' ORDER BY fantra";
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

        // Evento disparado quando o item do DropDownList é alterado
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

        // Função para preencher os campos com os dados do banco
        private void PreencherCampos(int id)
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
                 //   ddlAgregados.DataValueField = reader["ID"].ToString();
                //    ddlAgregados.Text = reader["fantra"].ToString();
                    
                    txtAntt.Text = reader["antt"].ToString();
                }
            }
        }

        // Função para limpar os campos
        private void LimparCampos()
        {
            txtCodTra.Text = string.Empty;
            txtAntt.Text = string.Empty;
        }

       

        protected void ddlComposicao_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idSelecionado = int.Parse(ddlComposicao.SelectedValue);

            // Preencher os campos com base no valor selecionado
            if (idSelecionado > 0)
            {
                PreencherCamposComposicao(idSelecionado);
            }
            else
            {
                LimparCamposComposicao();
            }
        }

        private void PreencherCamposComposicao(int id)
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                conn.Open();
                string query = "SELECT composicao, eixos, pbt, tolerancia FROM tbcomposicaoveiculo WHERE ID = @ID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {                    
                    txtEixos.Text = reader["eixos"].ToString();
                    txtPBT.Text = reader["pbt"].ToString();
                    txtTolerancia.Text = reader["tolerancia"].ToString();

                    double sPBT, sTara;
                    if (double.TryParse(txtPBT.Text, out sPBT) && double.TryParse(txtTara.Text, out sTara))
                    {
                        double resultado = sPBT - sTara;
                        txtLotacao.Text = resultado.ToString("N0"); // casas decimais
                    }                    

                    double sTolerancia, sLotacao;
                    if (double.TryParse(txtLotacao.Text, out sLotacao) && double.TryParse(txtTolerancia.Text, out sTolerancia))
                    {
                        double resultado2 = ((sLotacao * 5) / 100) + sLotacao;
                       
                        txtCargaLiq.Text = resultado2.ToString("N0"); // N0 = casas decimais 
                    }
                    
                }
            }
        }

        // Função para limpar os campos
        private void LimparCamposComposicao()
        {
            txtTolerancia.Text = string.Empty;
            txtPBT.Text = string.Empty;
            txtLotacao.Text = string.Empty;
            txtEixos.Text = string.Empty;
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
                                txtAntt.Text = reader["antt"].ToString();
                                txtCodMot.Focus();
                            }
                            else
                            {
                                ddlAgregados.ClearSelection();
                                txtAntt.Text = string.Empty;
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

        protected void txtCodMot_TextChanged(object sender, EventArgs e)
        {
            if (txtCodMot.Text != "")
            {

                string codigoMotorista = txtCodMot.Text.Trim();
                string strConn = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    string query = "SELECT codmot, nommot FROM tbmotoristas WHERE codmot = @Codigo";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Codigo", codigoMotorista);
                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                ddlMotorista.SelectedItem.Text = reader["nommot"].ToString();
                            }
                            else
                            {
                                ddlMotorista.ClearSelection();
                                txtCodMot.Text = string.Empty;
                                // Aciona o Toast via JavaScript

                                ScriptManager.RegisterStartupScript(this, GetType(), "toastNaoEncontrado", "mostrarToastNaoEncontrado();", true);
                                txtCodMot.Focus();
                                // Opcional: exibir mensagem ao usuário
                            }                            
                            
                        }
                    }

                }

            }
        }
    }
}