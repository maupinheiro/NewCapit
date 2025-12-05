using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Sql;
using System.Data.SqlClient;
using System.IO;
using System.Data;
using System.Text;
using System.Configuration;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using NPOI.OpenXmlFormats.Wordprocessing;
using DocumentFormat.OpenXml.Office2010.Excel;
using NPOI.HSSF.Record.Chart;
using DocumentFormat.OpenXml.Office2010.Drawing;

namespace NewCapit
{
		public partial class ConfereArquivo : System.Web.UI.Page
		{
			string cracha, hora;
			string sql4;
			string tipo, arquivo, arquivo1, pf;
			int c, d;
			int inicio, reinicio, fim, pernoite;
			int iniciob, reiniciob, fimb, pernoiteb;
			PagedDataSource pds = new PagedDataSource();

			string usuarioLogado;
			public string fotoMotorista;
			string codmot, caminhofoto;
			int dia;
			SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString());
			string filial;
			string dtinicial, dtfinal;
			DataTable dtDados = new DataTable("DataTable");
			protected void Page_Load(object sender, EventArgs e)
			{
				if (!IsPostBack)
				{
					if (Session["DataTable"] == null)
					{
						dtDados.Columns.Add("Dia", Type.GetType("System.String"));
						dtDados.Columns.Add("Marcacao", Type.GetType("System.String"));
						//dtDados.Columns.Add("Status", Type.GetType("System.String"));
				   


						Session["DataTable"] = dtDados;



					}
				//else
				//{
				//    Session["DataTable"] = null;
				//}
				PaginaAtual = 1;


				//gvPonto.DataSource = dtDados;
				//gvPonto.DataBind();
				Txt5e();
				}
			
			}
        public int PaginaAtual
        {
            get
            {
                if (ViewState["PaginaAtual"] == null)
                    return 1;   // ← ajuste aqui!!
                return (int)ViewState["PaginaAtual"];
            }
            set
            {
                ViewState["PaginaAtual"] = value;
            }
        }

        protected void lnkAnterior_Click(object sender, EventArgs e)
		{
			// Diminui a página e recarrega
			PaginaAtual--;
			Txt5e();
		}

		protected void lnkProximo_Click(object sender, EventArgs e)
		{
			// Aumenta a página e recarrega
			PaginaAtual++;
			Txt5e();
		}

        // Evento para esconder/mostrar botões (ex: esconder "Anterior" na primeira página)

        public void Txt5e()
        {

            if (HttpContext.Current.Request.QueryString["dataini"].ToString() != "")
            {
                dtinicial = HttpContext.Current.Request.QueryString["dataini"].ToString();
            }
            if (HttpContext.Current.Request.QueryString["datafim"].ToString() != "")
            {
                dtfinal = HttpContext.Current.Request.QueryString["datafim"].ToString();
            }
            if (HttpContext.Current.Request.QueryString["filial"].ToString() != "")
            {
                filial = HttpContext.Current.Request.QueryString["filial"].ToString();
            }

            //try
            //{
            // string sql1 = "select nr_cracha from tb_motorista_txt";
            string sql1 = "SELECT * FROM (SELECT id, codmot, nommot, DENSE_RANK() OVER (ORDER BY id DESC) AS NUMBER FROM tbmotoristas WHERE ISNUMERIC(codmot)=1 and status='ATIVO' ";


            if (filial == "Diadema")
            {
                sql1 += " and nucleo in('CNT (CC)') ";
                //sql1 += " and cod_login in (330,912,1027,1058,958,1091,1099,1116,1219,1290,1338,1352,1274,1403,1421,1446,1395,1526,1532,1534,1544,1545,1543, ";
                //sql1 += " 1564,1570,1571,1584,482,999,820,989,1591,1590) ";
                //sql1 += " and cod_login in (636,906,1047,1064,1070,1075,1178,1231,1388,1412,1463,1464,1478,1485,1488,1490,1491,1492,1495,1496,1497,1498,1499,1500,1537,1539,1548,1549,1563,1567,1568,1569,1572,";
                //sql1 += " 1573,1574,1575,1576,1580,1581,1583,1585,1587,1588,10,21) ";
            }
            else if (filial == "Matriz")
            {
                sql1 += " and nucleo in('MATRIZ') ";
                //sql1 += " and cod_login in (330,912,1027,1058,958,1091,1099,1116,1219,1290,1338,1352,1274,1403,1421,1446,1395,1526,1532,1534,1544,1545,1543, ";
                //sql1 += " 1564,1570,1571,1584,482,999,820,989,1591,1590) ";
                //sql1 += " and cod_login in (636,906,1047,1064,1070,1075,1178,1231,1388,1412,1463,1464,1478,1485,1488,1490,1491,1492,1495,1496,1497,1498,1499,1500,1537,1539,1548,1549,1563,1567,1568,1569,1572,";
                //sql1 += " 1573,1574,1575,1576,1580,1581,1583,1585,1587,1588,10,21) ";
            }
            else if (filial == "MinasGerais")
            {
                sql1 += " and nucleo='MINAS GERAIS' ";

            }
            //else if (chkBahia.Checked)
            //{
            //    sql1 += " and ds_nucleo='TNG BAHIA' ";
            //}
            else if (filial == "Ipiranga")
            {
                sql1 += " and nucleo='IPIRANGA' ";
            }
            else if (filial == "Taubate")
            {
                sql1 += " and nucleo='TAUBATE' ";
            }
            else if (filial == "SaoCarlos")
            {
                sql1 += " and nucleo='SAO CARLOS' ";
            }
            else
            {

                sql1 += " and codmot='" + filial + "' ";


            }


            sql1 += " ) AS TBL WHERE NUMBER BETWEEN ((" + PaginaAtual + "-1)*1)+1 AND (" + PaginaAtual + "*1) ORDER BY codmot";
            SqlDataAdapter adtp1 = new SqlDataAdapter(sql1, con);
            DataTable dt1 = new DataTable();

            try
            {
                con.Open();
                adtp1.Fill(dt1);
                con.Close();
            }
            catch (Exception ed)
            {
                //MessageBox.Show(ed.ToString(), "Erro1");
            }
            string sqlt = "select codmot, nommot  from tbmotoristas where ISNUMERIC(codmot)=1 and status='ATIVO' ";

            if (filial == "Diadema")
            {
                sqlt += " and nucleo in('CNT (CC)') ";

            }
            else if (filial == "Matriz")
            {
                sqlt += " and nucleo in('MATRIZ') ";

            }
            else if (filial == "MinasGerais")
            {
                sqlt += " and nucleo='MINAS GERAIS' ";

            }
            //else if (chkBahia.Checked)
            //{
            //    sql1 += " and ds_nucleo='TNG BAHIA' ";
            //}
            else if (filial == "Ipiranga")
            {
                sqlt += " and nucleo='IPIRANGA' ";
            }
            else if (filial == "Taubate")
            {
                sqlt += " and nucleo='TAUBATE' ";
            }
            else if (filial == "SaoCarlos")
            {
                sqlt += " and nucleo='SAO CARLOS' ";
            }
            else
            {

                sqlt += " and codmot='" + filial + "' ";


            }

            SqlDataAdapter adtpt = new SqlDataAdapter(sqlt, con);
            DataTable dtt = new DataTable();

            try
            {
                con.Open();
                adtpt.Fill(dtt);
                con.Close();
            }
            catch (Exception ed)
            {
                //MessageBox.Show(ed.ToString(), "Erro1");
            }


            //string arquivo2 = "Transmot" + DateTime.Parse(txtDtInicial.Text).ToString("ddMM") + "002.txt";
            DataTable dt = dt1;

            // Configuração da Paginação
            int totalMotoristas = dtt.Rows.Count;

            // 1. Ajuste de Limites
            if (PaginaAtual < 0) PaginaAtual = 1;
            if (PaginaAtual >= totalMotoristas) PaginaAtual = totalMotoristas > 0 ? totalMotoristas - 1 : 0;

            // Esconde/mostra a seção se não houver dados
            pnlMotorista.Visible = totalMotoristas > 0;

            if (totalMotoristas == 0)
            {
                lblPaginaInfo.Text = "Nenhum motorista encontrado.";
                lnkAnterior.Enabled = lnkProximo.Enabled = false;
                return;
            }

            // 2. PEGA O MOTORISTA DA PÁGINA ATUAL

            string codigoMotorista = dt1.Rows[0][1].ToString();
            string nomeMotorista = dt1.Rows[0][2].ToString();
            txtDtini.Text = dtinicial;
            txtDtfim.Text = dtfinal;

            // 3. ATUALIZA CONTROLES DO CABEÇALHO
            lblCod.Text = codigoMotorista;
            lblMotorista.Text = nomeMotorista;

            // 4. ATUALIZA GRIDVIEW
            //DataTable dtPonto = ObterDadosPontoPorMotorista(codigoMotorista); // Use o método do exemplo anterior
            //gvPonto.DataSource = dtPonto;
            //gvPonto.DataBind();

            // 5. ATUALIZA BOTÕES DE NAVEGAÇÃO
            lnkAnterior.Enabled = (PaginaAtual >= 0);
            lnkProximo.Enabled = (PaginaAtual < totalMotoristas - 1);
            lblPaginaInfo.Text = $"Motorista {PaginaAtual} de {totalMotoristas}";


            if (PaginaAtual == 1)
            {
                lnkAnterior.Enabled = false;

            }
            else
            {
                lnkAnterior.Enabled = true;

            }




            string caminho1 = Server.MapPath("/TXT/");

            string filepath1 = caminho1 + arquivo1;

            FileInfo file1 = new FileInfo(filepath1);
            if (file1.Exists)
            {
                file1.Delete();
            }









            string sql_e = "select dt_posicao_parada from tb_parada where dt_posicao_parada between '" + DateTime.Parse(txtDtini.Text).ToString("yyyy-MM-dd") + "' and '" + DateTime.Parse(txtDtfim.Text).ToString("yyyy-MM-dd") + "' and cod_cracha='" + lblCod.Text + "' and fl_deletado is null group by dt_posicao_parada  order by dt_posicao_parada asc";
            SqlDataAdapter adtpe = new SqlDataAdapter(sql_e, con);
            DataTable dte = new DataTable();
            try
            {
                con.Open();
                adtpe.Fill(dte);
                con.Close();
            }
            catch (Exception ex)
            {
                var message2 = new JavaScriptSerializer().Serialize(ex.Message.ToString());
                string retorno = "Erro! Contate o administrador. Detalhes do erro 50586: " + message2;
                string caminho = Server.MapPath("/TXT/");
                StreamWriter write = new StreamWriter(caminho + "Erro.txt", true);
                write.WriteLine(retorno);
                write.Flush();
                write.Close();


            }
            finally
            {
                con.Close();
            }

            dtDados = (DataTable)Session["DataTable"];
            List<string> marcacoes = new List<string>();
            int nrmarcacoes;

            for (int f = 0; f < dte.Rows.Count; f++)
            {
                DataRow dtRow = dtDados.NewRow();

                string sql3 = "select * from tb_parada where cod_cracha='" + lblCod.Text + "' and dt_posicao_parada ='" + DateTime.Parse(dte.Rows[f][0].ToString()).ToString("yyyy-MM-dd") + "' AND (ds_macro IN ('INICIO DE JORNADA', 'INICIO JORNADA CAMINHAO', 'PARADA REFEICAO', 'FIM DE JORNADA','REINICIO DE VIAGEM','FIM DE VIAGEM', 'PARADA','PARADA INTERNA','INICIO DE VIAGEM','PARADA INTERNA','RETORNO REFEICAO','PARADA CLIENTE / FORNECEDOR','PARADA PERNOITE','RETORNO PERNOITE') OR cod_ref_parada in (0,2,3,4,14,22)) AND fl_deletado IS NULL   ORDER BY dt_posicao_parada, hr_posicao";
                SqlDataAdapter adtp3 = new SqlDataAdapter(sql3, con);
                DataTable dt3 = new DataTable();
                try
                {
                    if (con.State != ConnectionState.Open)
                        con.Open();
                    adtp3.Fill(dt3);
                    con.Close();
                }
                catch (Exception ex)
                {
                    var message2 = new JavaScriptSerializer().Serialize(ex.Message.ToString());
                    string retorno = "Erro! Contate o administrador. Detalhes do erro 50613: " + message2 + " || Data:" + DateTime.Parse(dte.Rows[f][0].ToString()).ToString();
                    string caminho = Server.MapPath("/TXT/");
                    StreamWriter write = new StreamWriter(caminho + "Erro.txt", true);
                    write.WriteLine(retorno);
                    write.Flush();
                    write.Close();


                }
                finally
                {
                    con.Close();
                }






                if (dt3.Rows.Count > 0)
                {

                    if (dt3.Rows[0][6].ToString() == "INICIO DE JORNADA")
                    {

                        //VERIFICA SE A ULTIMA MARCAÇÃO DO DIA É PARADA PERNOITE OU FIM DE JORNADA
                        string sqlv = "select TOP 1 ds_macro  from tb_parada where cod_cracha=" + lblCod.Text + " and dt_posicao_parada='" + DateTime.Parse(dte.Rows[f][0].ToString()).ToString("yyyy-MM-dd") + "' and fl_deletado is null order by hr_posicao  DESC ";
                        SqlDataAdapter adtpv = new SqlDataAdapter(sqlv, con);
                        DataTable dtv = new DataTable();
                        con.Open();
                        adtpv.Fill(dtv);
                        con.Close();


                        if (dtv.Rows[0][0].ToString() == "FIM DE JORNADA" || dtv.Rows[0][0].ToString() == "PARADA PERNOITE")
                        {
                            dtRow["Dia"] = DateTime.Parse(dte.Rows[f][0].ToString()).ToString("dd");
                            //JORNADA NORMAL INICIO DE JORNADA
                            #region Jornada Normal
                            //string arquivo = "Transmot" + DateTime.Parse(txtDtInicial.Text).ToString("ddMM") + "001.txt";

                            sql4 = "select dt_posicao_parada, hr_posicao, ds_macro,cod_transmissao, cod_cracha, isnull(cod_ref_parada,0) as cod_ref_parada, cod_idveiculo from tb_parada where fl_deletado is null and cod_cracha=" + lblCod.Text + " and dt_posicao_parada='" + DateTime.Parse(dte.Rows[f][0].ToString()).ToString("yyyy-MM-dd") + "' AND (ds_macro IN ('INICIO DE JORNADA', 'INICIO JORNADA CAMINHAO', 'PARADA REFEICAO', 'FIM DE JORNADA','REINICIO DE VIAGEM','FIM DE VIAGEM', 'PARADA','PARADA INTERNA','INICIO DE VIAGEM','RETORNO REFEICAO','PARADA CLIENTE / FORNECEDOR','PARADA PERNOITE','RETORNO PERNOITE') OR cod_ref_parada in (0,2,3,4,14,22)) AND fl_deletado IS NULL ORDER BY dt_posicao_parada, hr_posicao";
                            // string sql4 = "select dt_posicao_parada, hr_posicao, ds_macro,cod_transmissao, cod_cracha, isnull(cod_ref_parada,0) as cod_ref_parada, cod_idveiculo from tb_parada where fl_deletado is null and cod_cracha=" + lblCod.Text + " and dt_posicao_parada between '" + txtDtInicial.Text + "' and '" + txtDtFinal.Text + "' order by dt_posicao_parada, hr_posicao";
                            SqlDataAdapter adtp4 = new SqlDataAdapter(sql4, con);
                            DataTable dt4 = new DataTable();

                            try
                            {
                                con.Open();
                                adtp4.Fill(dt4);
                                con.Close();
                            }
                            catch (Exception ed)
                            {
                                // MessageBox.Show(ed.ToString(), "Erro3");
                            }

                            c = 0;

                            for (int y = 0; y < dt4.Rows.Count; y++)
                            {
                                if (dt4.Rows[y][2].ToString() == "FIM DE JORNADA" && dt4.Rows[y][6].ToString() == "725405" || dt4.Rows[y][2].ToString() == "FIM DE JORNADA" && dt4.Rows[y][6].ToString() == "725407" || dt4.Rows[y][2].ToString() == "FIM DE JORNADA" && dt4.Rows[y][6].ToString() == "184525")
                                {
                                    c = c + 1;
                                }

                            }
                            string caminho = Server.MapPath("/TXT/");

                            string filepath = caminho + arquivo;

                            FileInfo file = new FileInfo(filepath);

                            //if (file.Exists)
                            //{
                            //    file.Delete();
                            //}


                            for (int w = 0; w < dt4.Rows.Count; w++)
                            {


                                string codigo = lblCod.Text;
                                string[] codigosEspeciais = { "330", "482", "820", "958", "989" };

                                if (codigo.StartsWith("312"))
                                {
                                    cracha = codigo.Insert(3, "0");
                                }
                                else if ((codigosEspeciais.Contains(codigo) || codigo.Length <= 4))
                                {
                                    cracha = "30" + codigo;
                                }
                                else if (codigo.Length <= 3)
                                {
                                    cracha = "300" + codigo;
                                }

                                else
                                {
                                    cracha = codigo;
                                }




                                int t = 0;
                                t = w - 1;

                                string data = DateTime.Parse(dt4.Rows[w][0].ToString()).ToString("dd/MM");
                                string horas = DateTime.Parse(dt4.Rows[w][1].ToString()).ToString("HH:mm");
                                if (t >= 0)
                                {
                                    if (DateTime.Parse(dt4.Rows[w][1].ToString()).ToString("HH:mm") == DateTime.Parse(dt4.Rows[t][1].ToString()).ToString("HH:mm"))
                                    {
                                        hora = DateTime.Parse(horas).AddMinutes(1).ToString("HH:mm");
                                    }
                                    else
                                    {
                                        hora = DateTime.Parse(horas).ToString("HH:mm");
                                    }
                                }
                                else
                                {
                                    hora = DateTime.Parse(horas).ToString("HH:mm");
                                }
                                //tipo = dt4.Rows[w][2].ToString();
                                string transmissao = dt4.Rows[w][3].ToString();


                                //if (dt4.Rows[w][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[w][2].ToString() == "TRECHO ALMOÇO" || dt4.Rows[w][2].ToString() == "FIM DE JORNADA")
                                //{
                                //    tipo = "2";
                                //}
                                //else  if (dt4.Rows[w][2].ToString() == "PARADA FORNECEDOR" || dt4.Rows[w][2].ToString() == "REINICIO DE VIAGEM")
                                //{
                                //    tipo = "21";
                                //}











                                #region BLOCO INICIO JORNADA
                                //BLOCO INICIO JORNADA
                                if (dt4.Rows[w][2].ToString() == "INICIO DE JORNADA")
                                {
                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0102");
                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01020000");
                                    marcacoes.Add(hora);



                                }

                                if (dt4.Rows[w][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[w][2].ToString() == "INICIO JORNADA")
                                {

                                    int a = 0;
                                    a = w - 1;
                                    if (a >= 0)
                                    {
                                        if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA")
                                        {
                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");

                                        }
                                        //else
                                        //{
                                        //	//write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0102");
                                        //	//write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01020000");
                                        //	marcacoes.Add(hora);



                                        //}
                                    }
                                    else
                                    {
                                        marcacoes.Add(hora);
                                    }


                                }
                                //FIM
                                #endregion

                                #region BLOCO INICIO JORNADA CAMINHAO
                                //BLOCO INICIO JORNADA CAMINHAO

                                if (dt4.Rows[w][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[w][2].ToString() == "INICIO JORNADA")
                                {

                                    int a = 0;
                                    a = w - 1;
                                    if (a >= 0)
                                    {
                                        if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA")
                                        {
                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");

                                        }
                                        else
                                        {
                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0102");
                                            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01020000");
                                            marcacoes.Add(hora);


                                        }
                                    }
                                    else
                                    {
                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                    }


                                }
                                //FIM
                                #endregion

                                #region PARADA INTERNA
                                //BLOCO PARADA INTERNA
                                if (dt4.Rows[w][2].ToString() == "PARADA INTERNA")
                                {
                                    if (dt4.Rows[w][5].ToString() == "3" || dt4.Rows[w][5].ToString() == "2")
                                    {
                                        int a = 0;
                                        a = w - 1;
                                        if (a >= 0)
                                        {
                                            if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[a][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[a][2].ToString() == "INICIO JORNADA")
                                            {
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");
                                            }
                                            else if (dt4.Rows[a][5].ToString() == "14")
                                            {
                                                //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");
                                                marcacoes.Add(hora);
                                            }
                                            else if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2" || dt4.Rows[a][5].ToString() == "1")
                                            {
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                            }
                                            else if (dt4.Rows[a][2].ToString() == "PARADA REFEICAO")
                                            {
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");
                                            }
                                            else if (dt4.Rows[a][2].ToString() == "INICIO DE VIAGEM")
                                            {
                                                //////write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                ////write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                ////write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");
                                            }
                                            else if (dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                            {
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;94");
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");
                                            }
                                            else if (dt4.Rows[a][2].ToString() == "FIM DE VIAGEM")
                                            {
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                            }
                                            else
                                            {
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");
                                            }

                                        }

                                    }
                                    else if (dt4.Rows[w][5].ToString() == "14")
                                    {
                                        int a = 0;
                                        a = w - 1;
                                        if (a >= 0)
                                        {
                                            if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[a][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[a][2].ToString() == "INICIO JORNADA")
                                            {
                                                marcacoes.Add(hora);
                                            }
                                            if (dt4.Rows[a][5].ToString() == "14")
                                            {
                                                //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                marcacoes.Add(hora);
                                            }
                                            else if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2" || dt4.Rows[a][5].ToString() == "1")
                                            {
                                                //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                marcacoes.Add(hora);
                                            }
                                            else if (dt4.Rows[a][2].ToString() == "PARADA REFEICAO")
                                            {
                                                // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");
                                            }
                                            else if (dt4.Rows[a][2].ToString() == "INICIO DE VIAGEM" || dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                            {
                                                //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                marcacoes.Add(hora);
                                            }
                                            else if (dt4.Rows[a][2].ToString() == "FIM DE VIAGEM")
                                            {

                                                //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                marcacoes.Add(hora);

                                            }
                                            if (dt4.Rows[a][5].ToString() == "22")
                                            {
                                                //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                marcacoes.Add(hora);
                                            }
                                            //else
                                            //{
                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93: 71489");
                                            //    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                            //}

                                        }
                                    }
                                    else if (dt4.Rows[w][5].ToString() == "22")
                                    {
                                        int a = 0;
                                        a = w - 1;
                                        if (a >= 0)
                                        {

                                            if (dt4.Rows[a][5].ToString() == "14")
                                            {
                                                //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");
                                                marcacoes.Add(hora);
                                            }


                                        }
                                    }
                                    else
                                    {
                                        int a = 0;
                                        a = w - 1;
                                        if (a >= 0)
                                        {
                                            if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[a][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[a][2].ToString() == "INICIO JORNADA")
                                            {
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;95");
                                            }
                                            if (dt4.Rows[a][5].ToString() == "14")
                                            {
                                                //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");
                                                marcacoes.Add(hora);

                                            }

                                            else if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2" || dt4.Rows[a][5].ToString() == "1")
                                            {
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;95");
                                            }
                                            else if (dt4.Rows[a][2].ToString() == "PARADA REFEICAO")
                                            {
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                                //// write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");
                                            }
                                            else if (dt4.Rows[a][2].ToString() == "INICIO DE VIAGEM" || dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                            {

                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;94");
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;95");
                                            }
                                            else if (dt4.Rows[a][2].ToString() == "FIM DE VIAGEM" || dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                            {

                                                //// write.WriteLine(cracha + ";" + data + ";" + hora + ";1;94");
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;95");

                                            }

                                        }
                                    }

                                }
                                //FIM 
                                #endregion

                                #region BLOCO PARADA REFEIÇÃO
                                //BLOCO PARADA REFEIÇÃO
                                if (dt4.Rows[w][2].ToString() == "PARADA REFEICAO")
                                {
                                    int a = 0;
                                    a = w - 1;
                                    if (a >= 0)
                                    {
                                        //if (dt4.Rows[a][2].ToString() == "PARADA INTERNA")
                                        //{

                                        //    if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2")
                                        //    {
                                        //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                        //        write.WriteLine(cracha + ";" + data + ";" + hora + ";2;93");
                                        //    }
                                        //    else
                                        //    {
                                        //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");
                                        //        write.WriteLine(cracha + ";" + data + ";" + hora + ";2;93");
                                        //    }
                                        //}
                                        if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[a][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[a][2].ToString() == "INICIO JORNADA")
                                        {
                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0193");
                                            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                            marcacoes.Add(hora);

                                        }
                                        else
                                        {
                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0193");
                                            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                            marcacoes.Add(hora);
                                        }

                                    }
                                    //else
                                    //{
                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");
                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;93");
                                    //}
                                }
                                //FIM
                                #endregion

                                #region RETORNO REFEIÇAO
                                //BLOCO RETORNO REFEIÇAO
                                if (dt4.Rows[w][2].ToString() == "RETORNO REFEICAO")
                                {

                                    int a = 0;
                                    int b = 0;
                                    a = w - 1;
                                    b = w + 1;
                                    if (a >= 0)
                                    {

                                        if (dt4.Rows[a][2].ToString() == "PARADA REFEICAO")
                                        {

                                            // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0293");
                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;0102");
                                            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");
                                            marcacoes.Add(hora);

                                        }
                                        //if (dt4.Rows[b][2].ToString() == "FIM DE JORNADA")
                                        //{
                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                        //}
                                        //else
                                        //{
                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                        //    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");

                                        //}
                                    }


                                }
                                //FIM
                                #endregion

                                #region INICIO DE VIAGEM
                                //BLOCO REINICIO / INICIO DE VIAGEM
                                if (dt4.Rows[w][2].ToString() == "INICIO DE VIAGEM")
                                {
                                    int a = 0;
                                    a = w - 1;
                                    if (a >= 0)
                                    {
                                        //if (dt4.Rows[a][2].ToString() == "PARADA INTERNA")
                                        //{
                                        //    if (dt4.Rows[a][5].ToString() == "14")
                                        //    {
                                        //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                        //        write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");

                                        //    }
                                        //    else
                                        //        if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2")
                                        //    {
                                        //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                        //        write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");

                                        //    }
                                        //    else
                                        //    {
                                        //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");
                                        //        write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                        //    }
                                        //}
                                        //if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[a][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[a][2].ToString() == "INICIO JORNADA")
                                        //{
                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                        //}
                                        //else if (dt4.Rows[a][2].ToString() == "PARADA PERNOITE")
                                        //{
                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;22");
                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                        //}
                                        //else if (dt4.Rows[a][2].ToString() == "FIM DE VIAGEM")
                                        //{
                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;94");
                                        //    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                        //}

                                        if (dt4.Rows[a][2].ToString() == "PARADA CLIENTE / FORNECEDOR")
                                        {


                                            if (dt4.Rows[a][5].ToString() == "14")
                                            {

                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0193");
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                marcacoes.Add(hora);



                                            }
                                            //else if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "1" || dt4.Rows[a][5].ToString() == "2" || dt4.Rows[a][5].ToString() == "4")
                                            //{
                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                            //}



                                        }
                                        else if (dt4.Rows[a][2].ToString() == "PARADA")
                                        {


                                            if (dt4.Rows[a][5].ToString() == "14")
                                            {

                                                // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0193");
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                marcacoes.Add(hora);
                                            }
                                            //else
                                            //{
                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");
                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");



                                            //}



                                        }
                                        //else if (dt4.Rows[a][2].ToString() == "RETORNO REFEICAO")
                                        //{
                                        //    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0293");
                                        //    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                        //    write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");

                                        //}

                                    }
                                    //else
                                    //{
                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                    //}



                                }
                                //FIM
                                #endregion

                                #region PARADA CLIENTE / FORNECEDOR
                                if (dt4.Rows[w][2].ToString() == "PARADA CLIENTE / FORNECEDOR")
                                {
                                    if (dt4.Rows[w][5].ToString() == "3" || dt4.Rows[w][5].ToString() == "2" || dt4.Rows[w][5].ToString() == "4")
                                    {
                                        int a = 0;
                                        a = w - 1;
                                        if (a >= 0)
                                        {
                                            //if (dt4.Rows[a][2].ToString() == "INICIO DE VIAGEM" || dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM" || dt4.Rows[a][2].ToString() == "FIM DE VIAGEM")
                                            //{
                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;94");
                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");
                                            //}
                                            if (dt4.Rows[a][2].ToString() == "PARADA CLIENTE / FORNECEDOR")
                                            {
                                                if (dt4.Rows[a][5].ToString() == "14")
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0293");
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;0102");
                                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");
                                                    marcacoes.Add(hora);


                                                }
                                                //else if (dt4.Rows[a][5].ToString() == "15")
                                                //{
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;94");
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");

                                                //}
                                                //else
                                                //{
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");
                                                //}

                                            }
                                        }

                                    }
                                    else if (dt4.Rows[w][5].ToString() == "14")
                                    {
                                        int a = 0;
                                        a = w - 1;
                                        if (a >= 0)
                                        {
                                            if (dt4.Rows[a][2].ToString() == "INICIO DE VIAGEM" || dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM" || dt4.Rows[a][2].ToString() == "FIM DE VIAGEM")
                                            {
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0193");
                                                //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                marcacoes.Add(hora);
                                            }
                                            else if (dt4.Rows[a][2].ToString() == "PARADA CLIENTE / FORNECEDOR")
                                            {
                                                if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2" || dt4.Rows[a][5].ToString() == "4")
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0193");
                                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                    marcacoes.Add(hora);
                                                }
                                                else if (dt4.Rows[a][5].ToString() == "14")
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0193");
                                                    marcacoes.Add(hora);

                                                }


                                            }
                                            else
                                            {
                                                //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                marcacoes.Add(hora);
                                            }

                                        }


                                    }


                                }
                                #endregion

                                #region REINICIO
                                //BLOCO REINICIO / INICIO DE VIAGEM
                                if (dt4.Rows[w][2].ToString() == "REINICIO DE VIAGEM" || dt4.Rows[w][2].ToString() == "RETORNO PERNOITE")
                                {
                                    int a = 0;
                                    a = w - 1;
                                    if (a >= 0)
                                    {
                                        if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[a][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[a][2].ToString() == "INICIO JORNADA")
                                        {

                                        }
                                        else if (dt4.Rows[a][2].ToString() == "PARADA PERNOITE")
                                        {
                                            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01020000");
                                            marcacoes.Add(hora);
                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                        }

                                        else if (dt4.Rows[a][2].ToString() == "PARADA CLIENTE / FORNECEDOR")
                                        {

                                            if (a >= 0)
                                            {

                                                if (dt4.Rows[a][2].ToString() == "PARADA CLIENTE / FORNECEDOR")
                                                {
                                                    //if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2")
                                                    //{
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                    //}
                                                    if (dt4.Rows[a][5].ToString() == "14")
                                                    {

                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0293");
                                                        marcacoes.Add(hora);
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0102");

                                                    }
                                                    //else if (dt4.Rows[a][5].ToString() == "15")
                                                    //{

                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");

                                                    //}
                                                    //else
                                                    //{
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                    //}
                                                }
                                                //else if (dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                                //{
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                //}


                                            }



                                        }
                                        else if (dt4.Rows[a][2].ToString() == "PARADA")
                                        {
                                            if (dt4.Rows[a][5].ToString() == "14")
                                            {
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0293");
                                                //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0102");
                                                marcacoes.Add(hora);
                                            }
                                            //else
                                            //{
                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");
                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                            //}




                                        }
                                        //else if (dt4.Rows[a][2].ToString() == "OFICINA / MANUTENCAO")
                                        //{

                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");
                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");


                                        //}

                                    }
                                    else
                                    {
                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0102");
                                        marcacoes.Add(hora);
                                        // write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                    }



                                }
                                //FIM
                                #endregion

                                #region BLOCO PARADA
                                // BLOCO PARADA
                                if (dt4.Rows[w][2].ToString() == "PARADA")
                                {
                                    int a = 0;
                                    a = w - 1;


                                    if (dt4.Rows[w][5].ToString() == "14")
                                    {
                                        if (dt4.Rows.Count >= a)
                                        {
                                            if (dt4.Rows[a][2].ToString() == "INICIO DE VIAGEM" || dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                            {
                                                // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0193");
                                                marcacoes.Add(hora);


                                            }

                                        }
                                    }
                                    //else
                                    //{
                                    //    if (dt4.Rows.Count >= a)
                                    //    {
                                    //        if (dt4.Rows[a][2].ToString() == "INICIO DE VIAGEM" || dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                    //        {
                                    //            write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                    //            //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;95");

                                    //        }
                                    //        //else
                                    //        //{
                                    //        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                    //        //    // write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                    //        //}
                                    //    }
                                    //}



                                    //  write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");

                                }
                                //FIM
                                #endregion

                                #region FIM DE VIAGEM
                                //BLOCO FIM DE VIAGEM
                                if (dt4.Rows[w][2].ToString() == "FIM DE VIAGEM")
                                {
                                    int a = 0;
                                    int g = 0;
                                    a = w - 1;
                                    g = w - 2;
                                    if (a >= 0)
                                    {
                                        if (dt4.Rows[a][2].ToString() == "PARADA CLIENTE / FORNECEDOR")
                                        {


                                            if (dt4.Rows[a][5].ToString() == "14")
                                            {

                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0293");
                                                //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;0102");
                                                marcacoes.Add(hora);

                                            }

                                            //else if (dt4.Rows[a][5].ToString() == "14")
                                            //{
                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                            //    // write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                            //}

                                            //else
                                            //{
                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                            //    // write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                            //}



                                        }
                                        else if (dt4.Rows[a][2].ToString() == "PARADA")
                                        {


                                            if (dt4.Rows[a][5].ToString() == "14")
                                            {
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0293");
                                                //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");
                                                marcacoes.Add(hora);
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;0102");

                                            }

                                            //else
                                            //{
                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                            //    // write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");

                                            //}


                                        }

                                        else if (dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                        {
                                            if (g >= 0)
                                            {
                                                //if (dt4.Rows[g][2].ToString() == "PARADA PERNOITE")
                                                //{
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                //    write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                //}
                                                //else
                                                //{
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                //}
                                            }
                                            //else
                                            //{
                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                            //}


                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                        }


                                    }
                                    //else
                                    //{
                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                    //}

                                }

                                //
                                #endregion

                                #region FIM DE JORNADA
                                //BLOCO FIM DE JORMADA
                                if (c > 0)
                                {
                                    if (dt4.Rows[w][2].ToString() == "FIM DE JORNADA" && dt4.Rows[w][6].ToString() == "725405" || dt4.Rows[w][2].ToString() == "FIM DE JORNADA" && dt4.Rows[w][6].ToString() == "725407" || dt4.Rows[w][2].ToString() == "FIM DE JORNADA" && dt4.Rows[w][6].ToString() == "184525" && dt4.Rows[w][6].ToString() == "184522")
                                    {
                                        int a = 0;
                                        a = w - 1;

                                        if (a >= 0)
                                        {
                                            if (dt4.Rows[a][2].ToString() == "RETORNO REFEICAO")
                                            {
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02020000");
                                                //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                marcacoes.Add(hora);

                                            }
                                            else if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA")
                                            {

                                                //WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02020000");
                                                marcacoes.Add(hora);


                                            }
                                            else if (dt4.Rows[a][2].ToString() == "RETORNO REFEICAO")
                                            {

                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02020000");
                                                marcacoes.Add(hora);

                                            }
                                            else if (dt4.Rows[a][2].ToString() == "FIM DE VIAGEM")
                                            {
                                                int b = 0;
                                                b = a - 1;

                                                if (b >= 0)
                                                {
                                                    if (dt4.Rows[b][2].ToString() == "INICIO DE VIAGEM" || dt4.Rows[b][2].ToString() == "REINICIO DE VIAGEM")
                                                    {
                                                        int d = 0;
                                                        d = b - 1;
                                                        if (d >= 0)
                                                        {

                                                            if (dt4.Rows[d][2].ToString() == "PARADA")
                                                            {
                                                                if (dt4.Rows[d][5].ToString() == "14")
                                                                {
                                                                    // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02020000");
                                                                    //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                                    marcacoes.Add(hora);
                                                                }
                                                                else
                                                                {
                                                                    // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02020000");
                                                                    // write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                                    marcacoes.Add(hora);
                                                                }




                                                            }
                                                            else
                                                            {
                                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02020000");
                                                                // write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                                marcacoes.Add(hora);
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                        //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02020000");
                                                        // write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                        marcacoes.Add(hora);
                                                    }

                                                }
                                                else
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                    marcacoes.Add(hora);
                                                    //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");

                                                }

                                            }
                                            else if (dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                            {
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02020000");
                                                //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                marcacoes.Add(hora);
                                            }
                                            else if (dt4.Rows[a][2].ToString() == "PARADA")
                                            {
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                marcacoes.Add(hora);
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                            }
                                            else if (dt4.Rows[a][2].ToString() == "PARADA INTERNA")
                                            {
                                                if (dt4.Rows[a][5].ToString() == "14")
                                                {
                                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");
                                                    marcacoes.Add(hora);
                                                }
                                                else
                                                {
                                                    marcacoes.Add(hora);
                                                }
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");

                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                            }
                                            else if (dt4.Rows[a][2].ToString() == "FIM DE JORNADA")
                                            {
                                                int m = 0;
                                                m = a - 1;

                                                if (m >= 0)
                                                {
                                                    if (dt4.Rows[m][2].ToString() == "RETORNO REFEICAO" || dt4.Rows[m][2].ToString() == "INICIO DE JORNADA")
                                                    {
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                        marcacoes.Add(hora);
                                                    }
                                                    else if (dt4.Rows[m][2].ToString() == "FIM DE VIAGEM")
                                                    {
                                                        int b = 0;
                                                        b = m - 1;

                                                        if (b >= 0)
                                                        {
                                                            if (dt4.Rows[b][2].ToString() == "INICIO DE VIAGEM" || dt4.Rows[b][2].ToString() == "REINICIO DE VIAGEM")
                                                            {
                                                                int d = 0;
                                                                d = b - 1;
                                                                if (d >= 0)
                                                                {

                                                                    if (dt4.Rows[d][2].ToString() == "PARADA")
                                                                    {
                                                                        if (dt4.Rows[d][5].ToString() == "14")
                                                                        {
                                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                            marcacoes.Add(hora);
                                                                            //.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                                        }
                                                                        else
                                                                        {
                                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                            marcacoes.Add(hora);
                                                                            //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");

                                                                        }


                                                                    }
                                                                    else
                                                                    {
                                                                        // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                        marcacoes.Add(hora);
                                                                        //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                marcacoes.Add(hora);
                                                                //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                            }

                                                        }


                                                    }
                                                    //else if (dt4.Rows[m][2].ToString() == "REINICIO DE VIAGEM")
                                                    //{
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;94");
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                    //}
                                                    //else if (dt4.Rows[m][2].ToString() == "PARADA INTERNA")
                                                    //{
                                                    //    if (dt4.Rows[m][5].ToString() == "3" || dt4.Rows[m][5].ToString() == "1" || dt4.Rows[m][5].ToString() == "2")
                                                    //    {
                                                    //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");

                                                    //    }
                                                    //    else if (dt4.Rows[m][5].ToString() == "22")
                                                    //    {
                                                    //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");
                                                    //    }

                                                    //    else if (dt4.Rows[m][2].ToString() == "FIM DE JORNADA")
                                                    //    {
                                                    //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                    //        write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                    //    }

                                                    //}
                                                    else if (dt4.Rows[m][2].ToString() == "PARADA")
                                                    {
                                                        // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                        marcacoes.Add(hora);
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                    }
                                                    //else if (dt4.Rows[m][2].ToString() == "PARADA INTERNA")
                                                    //{
                                                    //    if (dt4.Rows[m][5].ToString() != "3" || dt4.Rows[m][5].ToString() != "1" || dt4.Rows[m][5].ToString() != "2")
                                                    //    {
                                                    //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");
                                                    //        write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                    //        write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");

                                                    //    }

                                                    //}

                                                    else
                                                    {
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                        marcacoes.Add(hora);
                                                        //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                    }


                                                }
                                                else
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                }
                                            }
                                            else
                                            {
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                marcacoes.Add(hora);
                                                //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                            }


                                        }
                                        else
                                        {
                                            // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                            marcacoes.Add(hora);
                                            //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                        }
                                    }

                                }
                                else
                                {
                                    if (dt4.Rows[w][2].ToString() == "FIM DE JORNADA")
                                    {
                                        int q = 0;
                                        q = w + 1;

                                        if (q < dt4.Rows.Count)
                                        {
                                            if (dt4.Rows[q][2].ToString() == "FIM DE JORNADA")
                                            {
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                marcacoes.Add(hora);
                                                //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");

                                            }
                                            else
                                            {
                                                int a = 0;
                                                a = w - 1;

                                                if (a >= 0)
                                                {

                                                    if (dt4.Rows[a][2].ToString() == "FIM DE VIAGEM" || dt4.Rows[a][2].ToString() == "RETORNO REFEICAO" || dt4.Rows[a][2].ToString() == "INICIO DE JORNADA")
                                                    {
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                        marcacoes.Add(hora);
                                                        // write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");

                                                    }
                                                    //else if (dt4.Rows[a][2].ToString() == "PARADA INTERNA")
                                                    //{
                                                    //    if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2")
                                                    //    {
                                                    //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                    //        // write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                    //        // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                    //    }
                                                    //    else if (dt4.Rows[a][5].ToString() == "14")
                                                    //    {
                                                    //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                    //        write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                    //    }
                                                    //    else
                                                    //    {
                                                    //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");


                                                    //    }
                                                    //}
                                                    else if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[a][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[a][2].ToString() == "INICIO JORNADA")
                                                    {

                                                    }
                                                    else if (dt4.Rows[a][2].ToString() == "FIM DE JORNADA")
                                                    {
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                        marcacoes.Add(hora);
                                                        //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                    }
                                                    else
                                                    {
                                                        // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                        marcacoes.Add(hora);
                                                        // write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                    }
                                                }
                                            }

                                        }
                                        else
                                        {
                                            int a = 0;
                                            a = w - 1;

                                            if (a >= 0)
                                            {

                                                if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA")
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                    marcacoes.Add(hora);

                                                }
                                                else if (dt4.Rows[a][2].ToString() == "FIM DE VIAGEM" || dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                    marcacoes.Add(hora);
                                                    //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");

                                                }
                                                else if (dt4.Rows[a][2].ToString() == "RETORNO REFEICAO")
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                    marcacoes.Add(hora);

                                                }
                                                else if (dt4.Rows[a][2].ToString() == "PARADA INTERNA")
                                                {
                                                    if (dt4.Rows[a][5].ToString() == "14")
                                                    {
                                                        marcacoes.Add(hora);
                                                    }
                                                    else
                                                    {
                                                        marcacoes.Add(hora);
                                                    }
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");

                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                }
                                                //else if (dt4.Rows[a][2].ToString() == "PARADA INTERNA")
                                                //{
                                                //    if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2")
                                                //    {
                                                //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                //        // write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                //        // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                //    }
                                                //    else if (dt4.Rows[a][5].ToString() == "14")
                                                //    {
                                                //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                //        write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                //    }
                                                //    else
                                                //    {
                                                //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");

                                                //    }
                                                //}
                                                else if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[a][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[a][2].ToString() == "INICIO JORNADA")
                                                {

                                                }
                                                else if (dt4.Rows[a][2].ToString() == "FIM DE JORNADA")
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                    marcacoes.Add(hora);

                                                    //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                }
                                                else
                                                {
                                                    // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                    //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                }
                                            }
                                            else
                                            {
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                marcacoes.Add(hora);
                                                //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                            }
                                        }
                                    }





                                }


                                #endregion

                                #region PARADA PERNOITE

                                if (dt4.Rows[w][2].ToString() == "PARADA PERNOITE")
                                {
                                    int a = 0;
                                    a = w - 1;


                                    if (a >= 0)
                                    {
                                        if (dt4.Rows[a][2].ToString() == "PARADA CLIENTE / FORNECEDOR")
                                        {


                                            if (dt4.Rows[a][5].ToString() == "14")
                                            {

                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0293");
                                                marcacoes.Add(hora);
                                                //WriteLine(cracha + ";" + data + ";" + hora + ";1;0102");
                                                //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;0102");
                                                marcacoes.Add(DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm").Replace(":", ""));



                                            }
                                            else if (dt4.Rows[a][5].ToString() == "15")
                                            {

                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                marcacoes.Add(hora);
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;22");


                                            }
                                            //else if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2")
                                            //{
                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                            //    write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;22");
                                            //    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                            //}
                                            else
                                            {
                                                marcacoes.Add(hora);
                                            }



                                        }
                                        else if (dt4.Rows[a][2].ToString() == "PARADA")
                                        {
                                            //if (dt4.Rows[a][5].ToString() == "7")
                                            //{
                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                            //    write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;22");
                                            //}
                                            if (dt4.Rows[a][5].ToString() == "14")
                                            {

                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0293");
                                                marcacoes.Add(hora);
                                                //WriteLine(cracha + ";" + data + ";" + hora + ";1;0102");
                                                //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;0102");
                                                marcacoes.Add(DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm").Replace(":", ""));


                                            }
                                            else
                                            {
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                marcacoes.Add(hora);
                                                //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                            }

                                        }
                                        else if (dt4.Rows[a][2].ToString() == "INICIO DE VIAGEM")
                                        {
                                            marcacoes.Add(hora);
                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                            //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                            //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";2;22");
                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";3;22");
                                        }
                                        else if (dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                        {
                                            marcacoes.Add(hora);
                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                            //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");

                                            // write.WriteLine(cracha + ";" + data + ";" + hora + ";3;22");
                                        }
                                        else if (dt4.Rows[a][2].ToString() == "RETORNO REFEICAO")
                                        {
                                            marcacoes.Add(hora);
                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                            //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");

                                            // write.WriteLine(cracha + ";" + data + ";" + hora + ";3;22");
                                        }
                                        //else if (dt4.Rows[a][2].ToString() == "PARADA INTERNA")
                                        //{


                                        //    if (dt4.Rows[a][5].ToString() == "14")
                                        //    {

                                        //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                        //        //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;22");


                                        //    }
                                        //    //else if (dt4.Rows[a][5].ToString() == "15")
                                        //    //{

                                        //    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                        //    //    write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;22");


                                        //    //}
                                        //    else if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2")
                                        //    {
                                        //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                        //        //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;22");
                                        //        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                        //    }
                                        //    //else
                                        //    //{
                                        //    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;22");
                                        //    //}



                                        //}
                                        //else
                                        //{
                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;22");
                                        //}

                                    }
                                    //else
                                    //{
                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;22");
                                    //    write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;22");

                                    //}

                                }

                                #endregion

                                #region RETORNO PERNOITE
                                //BLOCO REINICIO / INICIO DE VIAGEM
                                if (dt4.Rows[w][2].ToString() == "RETORNO PERNOITE")
                                {
                                    int a = 0;
                                    a = w - 1;
                                    if (a >= 0)
                                    {
                                        if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[a][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[a][2].ToString() == "INICIO JORNADA")
                                        {

                                        }
                                        else if (dt4.Rows[a][2].ToString() == "PARADA PERNOITE")
                                        {
                                            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01020000");
                                            marcacoes.Add(hora);
                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                        }


                                    }
                                    else
                                    {
                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0102");
                                        marcacoes.Add(hora);
                                        // write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                    }



                                }
                                //FIM
                                #endregion

                                //dtDados.Rows.Add(dtRow);



                            }

                            //FIM

                            #endregion
                        }
                        else
                        {

                            //JORNADA PERNOITE INICIO DE JORNADA


                            string sqli = "select top 1  hr_posicao from tb_parada where cod_cracha=" + lblCod.Text + " and ds_macro = 'INICIO DE JORNADA' and dt_posicao_parada='" + DateTime.Parse(dte.Rows[f][0].ToString()).ToString("yyyy-MM-dd") + "' and fl_deletado is null order by hr_posicao";
                            SqlDataAdapter adtpi = new SqlDataAdapter(sqli, con);
                            DataTable dti = new DataTable();
                            con.Open();
                            adtpi.Fill(dti);
                            con.Close();

                            string sqlf = "select top 1 hr_posicao from tb_parada where cod_cracha=" + lblCod.Text + " and ds_macro = 'FIM DE JORNADA' and dt_posicao_parada='" + DateTime.Parse(dte.Rows[f][0].ToString()).AddDays(1).ToString("yyyy-MM-dd") + "' and fl_deletado is null OR cod_cracha=" + lblCod.Text + " and ds_macro='PARADA PERNOITE' and dt_posicao_parada='" + DateTime.Parse(dte.Rows[f][0].ToString()).AddDays(1).ToString("yyyy-MM-dd") + "' and fl_deletado is null order by hr_posicao";
                            SqlDataAdapter adtpf = new SqlDataAdapter(sqlf, con);
                            DataTable dtf = new DataTable();
                            con.Open();
                            adtpf.Fill(dtf);
                            con.Close();


                            if (dti.Rows.Count > 0 && dtf.Rows.Count > 0)
                            {
                                sql4 = "select dt_posicao_parada, hr_posicao, ds_macro,cod_transmissao, cod_cracha, isnull(cod_ref_parada,0) as cod_ref_parada, cod_idveiculo from tb_parada where fl_deletado is null and cod_cracha=" + lblCod.Text + " and dt_posicao_parada='" + DateTime.Parse(dte.Rows[f][0].ToString()).ToString("yyyy-MM-dd") + "' and hr_posicao >='" + dti.Rows[0][0].ToString() + "' or cod_cracha=" + lblCod.Text + " and dt_posicao_parada='" + DateTime.Parse(dte.Rows[f][0].ToString()).AddDays(1).ToString("yyyy-MM-dd") + "' and hr_posicao <= '" + dtf.Rows[0][0].ToString() + "' AND (ds_macro IN ('INICIO DE JORNADA', 'INICIO JORNADA CAMINHAO', 'PARADA REFEICAO', 'FIM DE JORNADA','REINICIO DE VIAGEM','FIM DE VIAGEM', 'PARADA','PARADA INTERNA','INICIO DE VIAGEM','PARADA INTERNA','RETORNO REFEICAO','PARADA CLIENTE / FORNECEDOR','PARADA PERNOITE','RETORNO PERNOITE') OR cod_ref_parada in (0,2,3,4,14,22)) AND fl_deletado IS NULL ORDER BY dt_posicao_parada, hr_posicao";
                                SqlDataAdapter adtp4 = new SqlDataAdapter(sql4, con);
                                DataTable dt4 = new DataTable();

                                dtRow["Dia"] = DateTime.Parse(dte.Rows[f][0].ToString()).ToString("dd");


                                #region Jornada Noturna




                                try
                                {
                                    con.Open();
                                    adtp4.Fill(dt4);
                                    con.Close();
                                }
                                catch (Exception ed)
                                {
                                    //MessageBox.Show(ed.ToString(), "Erro3");
                                }

                                c = 0;

                                for (int y = 0; y < dt4.Rows.Count; y++)
                                {
                                    if (dt4.Rows[y][2].ToString() == "FIM DE JORNADA" && dt4.Rows[y][6].ToString() == "725405" || dt4.Rows[y][2].ToString() == "FIM DE JORNADA" && dt4.Rows[y][6].ToString() == "725407" || dt4.Rows[y][2].ToString() == "FIM DE JORNADA" && dt4.Rows[y][6].ToString() == "184525")
                                    {
                                        c = c + 1;
                                    }

                                }
                                string caminho = Server.MapPath("/TXT/");

                                string filepath = caminho + arquivo;

                                FileInfo file = new FileInfo(filepath);
                                //if(file.Exists)
                                //{
                                //    file.Delete();
                                //}



                                for (int w = 0; w < dt4.AsEnumerable().Count(); w++)
                                {

                                    string codigo = lblCod.Text;
                                    string[] codigosEspeciais = { "330", "482", "820", "958", "989" };

                                    if (codigo.StartsWith("312"))
                                    {
                                        cracha = codigo.Insert(3, "0");
                                    }
                                    else if ((codigosEspeciais.Contains(codigo) || codigo.Length <= 4))
                                    {
                                        cracha = "30" + codigo;
                                    }
                                    else if (codigo.Length <= 3)
                                    {
                                        cracha = "300" + codigo;
                                    }

                                    else
                                    {
                                        cracha = codigo;
                                    }

                                    int t = 0;
                                    t = w - 1;

                                    string data = DateTime.Parse(dt4.Rows[w][0].ToString()).ToString("dd/MM");
                                    string horas = DateTime.Parse(dt4.Rows[w][1].ToString()).ToString("HH:mm");
                                    if (t >= 0)
                                    {
                                        if (DateTime.Parse(dt4.Rows[w][1].ToString()).ToString("HH:mm") == DateTime.Parse(dt4.Rows[t][1].ToString()).ToString("HH:mm"))
                                        {
                                            hora = DateTime.Parse(horas).AddMinutes(1).ToString("HH:mm");
                                        }
                                        else
                                        {
                                            hora = DateTime.Parse(horas).ToString("HH:mm");
                                        }
                                    }
                                    else
                                    {
                                        hora = DateTime.Parse(horas).ToString("HH:mm");
                                    }
                                    //tipo = dt4.Rows[w][2].ToString();
                                    string transmissao = dt4.Rows[w][3].ToString();


                                    //if (dt4.Rows[w][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[w][2].ToString() == "TRECHO ALMOÇO" || dt4.Rows[w][2].ToString() == "FIM DE JORNADA")
                                    //{
                                    //    tipo = "2";
                                    //}
                                    //else  if (dt4.Rows[w][2].ToString() == "PARADA FORNECEDOR" || dt4.Rows[w][2].ToString() == "REINICIO DE VIAGEM")
                                    //{
                                    //    tipo = "21";
                                    //}





                                    #region BLOCO INICIO JORNADA
                                    //BLOCO INICIO JORNADA
                                    if (dt4.Rows[w][2].ToString() == "INICIO DE JORNADA")
                                    {
                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0102");
                                        //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01020000");
                                        marcacoes.Add(hora);


                                    }


                                    //FIM
                                    #endregion

                                    #region BLOCO INICIO JORNADA CAMINHAO
                                    //BLOCO INICIO JORNADA CAMINHAO

                                    if (dt4.Rows[w][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[w][2].ToString() == "INICIO JORNADA")
                                    {

                                        int a = 0;
                                        a = w - 1;
                                        if (a >= 0)
                                        {
                                            if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA")
                                            {
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");

                                            }
                                            else
                                            {
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0102");
                                                //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01020000");
                                                marcacoes.Add(hora);
                                            }
                                        }
                                        else
                                        {
                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                        }


                                    }
                                    //FIM
                                    #endregion

                                    #region PARADA INTERNA
                                    //BLOCO PARADA INTERNA
                                    if (dt4.Rows[w][2].ToString() == "PARADA INTERNA")
                                    {
                                        if (dt4.Rows[w][5].ToString() == "3" || dt4.Rows[w][5].ToString() == "2")
                                        {
                                            int a = 0;
                                            a = w - 1;
                                            if (a >= 0)
                                            {
                                                if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[a][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[a][2].ToString() == "INICIO JORNADA")
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");
                                                }
                                                else if (dt4.Rows[a][5].ToString() == "14")
                                                {
                                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");
                                                    marcacoes.Add(hora);
                                                }
                                                else if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2" || dt4.Rows[a][5].ToString() == "1")
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                }
                                                else if (dt4.Rows[a][2].ToString() == "PARADA REFEICAO")
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");
                                                }
                                                else if (dt4.Rows[a][2].ToString() == "INICIO DE VIAGEM")
                                                {
                                                    //////write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                    ////write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                    ////write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");
                                                }
                                                else if (dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;94");
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");
                                                }
                                                else if (dt4.Rows[a][2].ToString() == "FIM DE VIAGEM")
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                }
                                                else
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");
                                                }

                                            }

                                        }
                                        else if (dt4.Rows[w][5].ToString() == "14")
                                        {
                                            int a = 0;
                                            a = w - 1;
                                            if (a >= 0)
                                            {
                                                if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[a][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[a][2].ToString() == "INICIO JORNADA")
                                                {
                                                    marcacoes.Add(hora);
                                                }
                                                if (dt4.Rows[a][5].ToString() == "14")
                                                {
                                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                    marcacoes.Add(hora);
                                                }
                                                else if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2" || dt4.Rows[a][5].ToString() == "1")
                                                {
                                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                    marcacoes.Add(hora);
                                                }
                                                else if (dt4.Rows[a][2].ToString() == "PARADA REFEICAO")
                                                {
                                                    // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");
                                                }
                                                else if (dt4.Rows[a][2].ToString() == "INICIO DE VIAGEM" || dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                                {
                                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                    marcacoes.Add(hora);
                                                }
                                                else if (dt4.Rows[a][2].ToString() == "FIM DE VIAGEM")
                                                {

                                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                    marcacoes.Add(hora);

                                                }
                                                if (dt4.Rows[a][5].ToString() == "22")
                                                {
                                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                    marcacoes.Add(hora);
                                                }
                                                //else
                                                //{
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93: 71489");
                                                //    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                                //}

                                            }
                                        }
                                        else if (dt4.Rows[w][5].ToString() == "22")
                                        {
                                            int a = 0;
                                            a = w - 1;
                                            if (a >= 0)
                                            {

                                                if (dt4.Rows[a][5].ToString() == "14")
                                                {
                                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");
                                                    marcacoes.Add(hora);
                                                }


                                            }
                                        }
                                        else
                                        {
                                            int a = 0;
                                            a = w - 1;
                                            if (a >= 0)
                                            {
                                                if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[a][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[a][2].ToString() == "INICIO JORNADA")
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;95");
                                                }
                                                if (dt4.Rows[a][5].ToString() == "14")
                                                {
                                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");
                                                    marcacoes.Add(hora);

                                                }

                                                else if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2" || dt4.Rows[a][5].ToString() == "1")
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;95");
                                                }
                                                else if (dt4.Rows[a][2].ToString() == "PARADA REFEICAO")
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                                    //// write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");
                                                }
                                                else if (dt4.Rows[a][2].ToString() == "INICIO DE VIAGEM" || dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                                {

                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;94");
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;95");
                                                }
                                                else if (dt4.Rows[a][2].ToString() == "FIM DE VIAGEM" || dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                                {

                                                    //// write.WriteLine(cracha + ";" + data + ";" + hora + ";1;94");
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;95");

                                                }

                                            }
                                        }

                                    }
                                    //FIM 
                                    #endregion

                                    #region BLOCO PARADA REFEIÇÃO
                                    //BLOCO PARADA REFEIÇÃO
                                    if (dt4.Rows[w][2].ToString() == "PARADA REFEICAO")
                                    {
                                        int a = 0;
                                        a = w - 1;
                                        if (a >= 0)
                                        {
                                            //if (dt4.Rows[a][2].ToString() == "PARADA INTERNA")
                                            //{

                                            //    if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2")
                                            //    {
                                            //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                            //        write.WriteLine(cracha + ";" + data + ";" + hora + ";2;93");
                                            //    }
                                            //    else
                                            //    {
                                            //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");
                                            //        write.WriteLine(cracha + ";" + data + ";" + hora + ";2;93");
                                            //    }
                                            //}
                                            if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[a][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[a][2].ToString() == "INICIO JORNADA")
                                            {
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0193");
                                                //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                marcacoes.Add(hora);

                                            }
                                            else
                                            {
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0193");
                                                //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                marcacoes.Add(hora);
                                            }

                                        }
                                        //else
                                        //{
                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");
                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;93");
                                        //}
                                    }
                                    //FIM
                                    #endregion

                                    #region RETORNO REFEIÇAO
                                    //BLOCO RETORNO REFEIÇAO
                                    if (dt4.Rows[w][2].ToString() == "RETORNO REFEICAO")
                                    {

                                        int a = 0;
                                        int b = 0;
                                        a = w - 1;
                                        b = w + 1;
                                        if (a >= 0)
                                        {

                                            if (dt4.Rows[a][2].ToString() == "PARADA REFEICAO")
                                            {

                                                // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0293");
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;0102");
                                                //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");
                                                marcacoes.Add(hora);

                                            }
                                            //if (dt4.Rows[b][2].ToString() == "FIM DE JORNADA")
                                            //{
                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                            //}
                                            //else
                                            //{
                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                            //    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");

                                            //}
                                        }


                                    }
                                    //FIM
                                    #endregion

                                    #region INICIO DE VIAGEM
                                    //BLOCO REINICIO / INICIO DE VIAGEM
                                    if (dt4.Rows[w][2].ToString() == "INICIO DE VIAGEM")
                                    {
                                        int a = 0;
                                        a = w - 1;
                                        if (a >= 0)
                                        {
                                            //if (dt4.Rows[a][2].ToString() == "PARADA INTERNA")
                                            //{
                                            //    if (dt4.Rows[a][5].ToString() == "14")
                                            //    {
                                            //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                            //        write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");

                                            //    }
                                            //    else
                                            //        if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2")
                                            //    {
                                            //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                            //        write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");

                                            //    }
                                            //    else
                                            //    {
                                            //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");
                                            //        write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                            //    }
                                            //}
                                            //if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[a][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[a][2].ToString() == "INICIO JORNADA")
                                            //{
                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                            //}
                                            //else if (dt4.Rows[a][2].ToString() == "PARADA PERNOITE")
                                            //{
                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;22");
                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                            //}
                                            //else if (dt4.Rows[a][2].ToString() == "FIM DE VIAGEM")
                                            //{
                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;94");
                                            //    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                            //}

                                            if (dt4.Rows[a][2].ToString() == "PARADA CLIENTE / FORNECEDOR")
                                            {


                                                if (dt4.Rows[a][5].ToString() == "14")
                                                {

                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0193");
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                    marcacoes.Add(hora);



                                                }
                                                //else if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "1" || dt4.Rows[a][5].ToString() == "2" || dt4.Rows[a][5].ToString() == "4")
                                                //{
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                //}



                                            }
                                            else if (dt4.Rows[a][2].ToString() == "PARADA")
                                            {


                                                if (dt4.Rows[a][5].ToString() == "14")
                                                {

                                                    // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0193");
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                    marcacoes.Add(hora);
                                                }
                                                //else
                                                //{
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");



                                                //}



                                            }
                                            //else if (dt4.Rows[a][2].ToString() == "RETORNO REFEICAO")
                                            //{
                                            //    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0293");
                                            //    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                            //    write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");

                                            //}

                                        }
                                        //else
                                        //{
                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                        //}



                                    }
                                    //FIM
                                    #endregion

                                    #region PARADA CLIENTE / FORNECEDOR
                                    if (dt4.Rows[w][2].ToString() == "PARADA CLIENTE / FORNECEDOR")
                                    {
                                        if (dt4.Rows[w][5].ToString() == "3" || dt4.Rows[w][5].ToString() == "2" || dt4.Rows[w][5].ToString() == "4")
                                        {
                                            int a = 0;
                                            a = w - 1;
                                            if (a >= 0)
                                            {
                                                //if (dt4.Rows[a][2].ToString() == "INICIO DE VIAGEM" || dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM" || dt4.Rows[a][2].ToString() == "FIM DE VIAGEM")
                                                //{
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;94");
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");
                                                //}
                                                if (dt4.Rows[a][2].ToString() == "PARADA CLIENTE / FORNECEDOR")
                                                {
                                                    if (dt4.Rows[a][5].ToString() == "14")
                                                    {
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0293");
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;0102");
                                                        //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");
                                                        marcacoes.Add(hora);


                                                    }
                                                    //else if (dt4.Rows[a][5].ToString() == "15")
                                                    //{
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;94");
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");

                                                    //}
                                                    //else
                                                    //{
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");
                                                    //}

                                                }
                                            }

                                        }
                                        else if (dt4.Rows[w][5].ToString() == "14")
                                        {
                                            int a = 0;
                                            a = w - 1;
                                            if (a >= 0)
                                            {
                                                if (dt4.Rows[a][2].ToString() == "INICIO DE VIAGEM" || dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM" || dt4.Rows[a][2].ToString() == "FIM DE VIAGEM")
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                    // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0193");
                                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                    marcacoes.Add(hora);
                                                }
                                                else if (dt4.Rows[a][2].ToString() == "PARADA CLIENTE / FORNECEDOR")
                                                {
                                                    if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2" || dt4.Rows[a][5].ToString() == "4")
                                                    {
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0193");
                                                        //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                        marcacoes.Add(hora);
                                                    }
                                                    else if (dt4.Rows[a][5].ToString() == "14")
                                                    {
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0193");
                                                        marcacoes.Add(hora);

                                                    }


                                                }
                                                else
                                                {
                                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                    marcacoes.Add(hora);
                                                }

                                            }


                                        }


                                    }
                                    #endregion

                                    #region REINICIO
                                    //BLOCO REINICIO / INICIO DE VIAGEM
                                    if (dt4.Rows[w][2].ToString() == "REINICIO DE VIAGEM" || dt4.Rows[w][2].ToString() == "RETORNO PERNOITE")
                                    {
                                        int a = 0;
                                        a = w - 1;
                                        if (a >= 0)
                                        {
                                            if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[a][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[a][2].ToString() == "INICIO JORNADA")
                                            {

                                            }
                                            else if (dt4.Rows[a][2].ToString() == "PARADA PERNOITE")
                                            {
                                                //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01020000");
                                                marcacoes.Add(hora);
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                            }

                                            else if (dt4.Rows[a][2].ToString() == "PARADA CLIENTE / FORNECEDOR")
                                            {

                                                if (a >= 0)
                                                {

                                                    if (dt4.Rows[a][2].ToString() == "PARADA CLIENTE / FORNECEDOR")
                                                    {
                                                        //if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2")
                                                        //{
                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                        //}
                                                        if (dt4.Rows[a][5].ToString() == "14")
                                                        {

                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0293");
                                                            marcacoes.Add(hora);
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0102");

                                                        }
                                                        //else if (dt4.Rows[a][5].ToString() == "15")
                                                        //{

                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");

                                                        //}
                                                        //else
                                                        //{
                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                        //}
                                                    }
                                                    //else if (dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                                    //{
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                    //}


                                                }



                                            }
                                            else if (dt4.Rows[a][2].ToString() == "PARADA")
                                            {
                                                if (dt4.Rows[a][5].ToString() == "14")
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0293");
                                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0102");
                                                    marcacoes.Add(hora);
                                                }
                                                //else
                                                //{
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                //}




                                            }
                                            //else if (dt4.Rows[a][2].ToString() == "OFICINA / MANUTENCAO")
                                            //{

                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");
                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");


                                            //}

                                        }
                                        else
                                        {
                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0102");
                                            marcacoes.Add(hora);
                                            // write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                        }



                                    }
                                    //FIM
                                    #endregion

                                    #region BLOCO PARADA
                                    // BLOCO PARADA
                                    if (dt4.Rows[w][2].ToString() == "PARADA")
                                    {
                                        int a = 0;
                                        a = w - 1;


                                        if (dt4.Rows[w][5].ToString() == "14")
                                        {
                                            if (dt4.Rows.Count >= a)
                                            {
                                                if (dt4.Rows[a][2].ToString() == "INICIO DE VIAGEM" || dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                                {
                                                    // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0193");
                                                    marcacoes.Add(hora);


                                                }

                                            }
                                        }
                                        //else
                                        //{
                                        //    if (dt4.Rows.Count >= a)
                                        //    {
                                        //        if (dt4.Rows[a][2].ToString() == "INICIO DE VIAGEM" || dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                        //        {
                                        //            write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                        //            //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;95");

                                        //        }
                                        //        //else
                                        //        //{
                                        //        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                        //        //    // write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                        //        //}
                                        //    }
                                        //}



                                        //  write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");

                                    }
                                    //FIM
                                    #endregion

                                    #region FIM DE VIAGEM
                                    //BLOCO FIM DE VIAGEM
                                    if (dt4.Rows[w][2].ToString() == "FIM DE VIAGEM")
                                    {
                                        int a = 0;
                                        int g = 0;
                                        a = w - 1;
                                        g = w - 2;
                                        if (a >= 0)
                                        {
                                            if (dt4.Rows[a][2].ToString() == "PARADA CLIENTE / FORNECEDOR")
                                            {


                                                if (dt4.Rows[a][5].ToString() == "14")
                                                {

                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0293");
                                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;0102");
                                                    marcacoes.Add(hora);

                                                }

                                                //else if (dt4.Rows[a][5].ToString() == "14")
                                                //{
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                                //    // write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                //}

                                                //else
                                                //{
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                //    // write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                //}



                                            }
                                            else if (dt4.Rows[a][2].ToString() == "PARADA")
                                            {


                                                if (dt4.Rows[a][5].ToString() == "14")
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0293");
                                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");
                                                    marcacoes.Add(hora);
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;0102");

                                                }

                                                //else
                                                //{
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                //    // write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");

                                                //}


                                            }

                                            else if (dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                            {
                                                if (g >= 0)
                                                {
                                                    //if (dt4.Rows[g][2].ToString() == "PARADA PERNOITE")
                                                    //{
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                    //    write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                    //}
                                                    //else
                                                    //{
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                    //}
                                                }
                                                //else
                                                //{
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                //}


                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                            }


                                        }
                                        //else
                                        //{
                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                        //}

                                    }

                                    //
                                    #endregion

                                    #region FIM DE JORNADA
                                    //BLOCO FIM DE JORMADA
                                    if (c > 0)
                                    {
                                        if (dt4.Rows[w][2].ToString() == "FIM DE JORNADA" && dt4.Rows[w][6].ToString() == "725405" || dt4.Rows[w][2].ToString() == "FIM DE JORNADA" && dt4.Rows[w][6].ToString() == "725407" || dt4.Rows[w][2].ToString() == "FIM DE JORNADA" && dt4.Rows[w][6].ToString() == "184525" && dt4.Rows[w][6].ToString() == "184522")
                                        {
                                            int a = 0;
                                            a = w - 1;

                                            if (a >= 0)
                                            {
                                                if (dt4.Rows[a][2].ToString() == "RETORNO REFEICAO")
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02020000");
                                                    //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                    marcacoes.Add(hora);

                                                }
                                                else if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA")
                                                {

                                                    //WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02020000");
                                                    marcacoes.Add(hora);


                                                }
                                                else if (dt4.Rows[a][2].ToString() == "RETORNO REFEICAO")
                                                {

                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02020000");
                                                    marcacoes.Add(hora);

                                                }
                                                else if (dt4.Rows[a][2].ToString() == "FIM DE VIAGEM")
                                                {
                                                    int b = 0;
                                                    b = a - 1;

                                                    if (b >= 0)
                                                    {
                                                        if (dt4.Rows[b][2].ToString() == "INICIO DE VIAGEM" || dt4.Rows[b][2].ToString() == "REINICIO DE VIAGEM")
                                                        {
                                                            int d = 0;
                                                            d = b - 1;
                                                            if (d >= 0)
                                                            {

                                                                if (dt4.Rows[d][2].ToString() == "PARADA")
                                                                {
                                                                    if (dt4.Rows[d][5].ToString() == "14")
                                                                    {
                                                                        // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                        //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02020000");
                                                                        //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                                        marcacoes.Add(hora);
                                                                    }
                                                                    else
                                                                    {
                                                                        // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                        //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02020000");
                                                                        // write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                                        marcacoes.Add(hora);
                                                                    }




                                                                }
                                                                else
                                                                {
                                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02020000");
                                                                    // write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                                    marcacoes.Add(hora);
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02020000");
                                                            // write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                            marcacoes.Add(hora);
                                                        }

                                                    }
                                                    else
                                                    {
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                        marcacoes.Add(hora);
                                                        //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");

                                                    }

                                                }
                                                else if (dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02020000");
                                                    //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                    marcacoes.Add(hora);
                                                }
                                                else if (dt4.Rows[a][2].ToString() == "PARADA")
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                    marcacoes.Add(hora);
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                }
                                                else if (dt4.Rows[a][2].ToString() == "PARADA INTERNA")
                                                {
                                                    if (dt4.Rows[a][5].ToString() == "14")
                                                    {
                                                        //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");
                                                        marcacoes.Add(hora);
                                                    }
                                                    else
                                                    {
                                                        marcacoes.Add(hora);
                                                    }
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");

                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                }
                                                else if (dt4.Rows[a][2].ToString() == "FIM DE JORNADA")
                                                {
                                                    int m = 0;
                                                    m = a - 1;

                                                    if (m >= 0)
                                                    {
                                                        if (dt4.Rows[m][2].ToString() == "RETORNO REFEICAO" || dt4.Rows[m][2].ToString() == "INICIO DE JORNADA")
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                            marcacoes.Add(hora);
                                                        }
                                                        else if (dt4.Rows[m][2].ToString() == "FIM DE VIAGEM")
                                                        {
                                                            int b = 0;
                                                            b = m - 1;

                                                            if (b >= 0)
                                                            {
                                                                if (dt4.Rows[b][2].ToString() == "INICIO DE VIAGEM" || dt4.Rows[b][2].ToString() == "REINICIO DE VIAGEM")
                                                                {
                                                                    int d = 0;
                                                                    d = b - 1;
                                                                    if (d >= 0)
                                                                    {

                                                                        if (dt4.Rows[d][2].ToString() == "PARADA")
                                                                        {
                                                                            if (dt4.Rows[d][5].ToString() == "14")
                                                                            {
                                                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                                marcacoes.Add(hora);
                                                                                //.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                                            }
                                                                            else
                                                                            {
                                                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                                marcacoes.Add(hora);
                                                                                //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");

                                                                            }


                                                                        }
                                                                        else
                                                                        {
                                                                            // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                            marcacoes.Add(hora);
                                                                            //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                    marcacoes.Add(hora);
                                                                    //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                                }

                                                            }


                                                        }
                                                        //else if (dt4.Rows[m][2].ToString() == "REINICIO DE VIAGEM")
                                                        //{
                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;94");
                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                        //}
                                                        //else if (dt4.Rows[m][2].ToString() == "PARADA INTERNA")
                                                        //{
                                                        //    if (dt4.Rows[m][5].ToString() == "3" || dt4.Rows[m][5].ToString() == "1" || dt4.Rows[m][5].ToString() == "2")
                                                        //    {
                                                        //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");

                                                        //    }
                                                        //    else if (dt4.Rows[m][5].ToString() == "22")
                                                        //    {
                                                        //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");
                                                        //    }

                                                        //    else if (dt4.Rows[m][2].ToString() == "FIM DE JORNADA")
                                                        //    {
                                                        //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                        //        write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                        //    }

                                                        //}
                                                        else if (dt4.Rows[m][2].ToString() == "PARADA")
                                                        {
                                                            // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                            marcacoes.Add(hora);
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                        }
                                                        //else if (dt4.Rows[m][2].ToString() == "PARADA INTERNA")
                                                        //{
                                                        //    if (dt4.Rows[m][5].ToString() != "3" || dt4.Rows[m][5].ToString() != "1" || dt4.Rows[m][5].ToString() != "2")
                                                        //    {
                                                        //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");
                                                        //        write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                        //        write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");

                                                        //    }

                                                        //}

                                                        else
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                            marcacoes.Add(hora);
                                                            //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                        }


                                                    }
                                                    else
                                                    {
                                                        //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                    }
                                                }
                                                else
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                    marcacoes.Add(hora);
                                                    //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                }


                                            }
                                            else
                                            {
                                                // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                marcacoes.Add(hora);
                                                //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                            }
                                        }

                                    }
                                    else
                                    {
                                        if (dt4.Rows[w][2].ToString() == "FIM DE JORNADA")
                                        {
                                            int q = 0;
                                            q = w + 1;

                                            if (q < dt4.Rows.Count)
                                            {
                                                if (dt4.Rows[q][2].ToString() == "FIM DE JORNADA")
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                    marcacoes.Add(hora);
                                                    //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");

                                                }
                                                else
                                                {
                                                    int a = 0;
                                                    a = w - 1;

                                                    if (a >= 0)
                                                    {

                                                        if (dt4.Rows[a][2].ToString() == "FIM DE VIAGEM" || dt4.Rows[a][2].ToString() == "RETORNO REFEICAO" || dt4.Rows[a][2].ToString() == "INICIO DE JORNADA")
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                            marcacoes.Add(hora);
                                                            // write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");

                                                        }
                                                        //else if (dt4.Rows[a][2].ToString() == "PARADA INTERNA")
                                                        //{
                                                        //    if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2")
                                                        //    {
                                                        //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                        //        // write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                        //        // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                        //    }
                                                        //    else if (dt4.Rows[a][5].ToString() == "14")
                                                        //    {
                                                        //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                        //        write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                        //    }
                                                        //    else
                                                        //    {
                                                        //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");


                                                        //    }
                                                        //}
                                                        else if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[a][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[a][2].ToString() == "INICIO JORNADA")
                                                        {

                                                        }
                                                        else if (dt4.Rows[a][2].ToString() == "FIM DE JORNADA")
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                            marcacoes.Add(hora);
                                                            //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                        }
                                                        else
                                                        {
                                                            // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                            marcacoes.Add(hora);
                                                            // write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                        }
                                                    }
                                                }

                                            }
                                            else
                                            {
                                                int a = 0;
                                                a = w - 1;

                                                if (a >= 0)
                                                {

                                                    if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA")
                                                    {
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                        marcacoes.Add(hora);

                                                    }
                                                    else if (dt4.Rows[a][2].ToString() == "FIM DE VIAGEM" || dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                                    {
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                        marcacoes.Add(hora);
                                                        //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");

                                                    }
                                                    else if (dt4.Rows[a][2].ToString() == "RETORNO REFEICAO")
                                                    {
                                                        //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                        marcacoes.Add(hora);

                                                    }
                                                    else if (dt4.Rows[a][2].ToString() == "PARADA INTERNA")
                                                    {
                                                        if (dt4.Rows[a][5].ToString() == "14")
                                                        {
                                                            marcacoes.Add(hora);
                                                        }
                                                        else
                                                        {
                                                            marcacoes.Add(hora);
                                                        }
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");

                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                    }
                                                    //else if (dt4.Rows[a][2].ToString() == "PARADA INTERNA")
                                                    //{
                                                    //    if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2")
                                                    //    {
                                                    //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                    //        // write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                    //        // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                    //    }
                                                    //    else if (dt4.Rows[a][5].ToString() == "14")
                                                    //    {
                                                    //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                    //        write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                    //    }
                                                    //    else
                                                    //    {
                                                    //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");

                                                    //    }
                                                    //}
                                                    else if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[a][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[a][2].ToString() == "INICIO JORNADA")
                                                    {

                                                    }
                                                    else if (dt4.Rows[a][2].ToString() == "FIM DE JORNADA")
                                                    {
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                        marcacoes.Add(hora);

                                                        //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                    }
                                                    else
                                                    {
                                                        // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                        //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                    }
                                                }
                                                else
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                    marcacoes.Add(hora);
                                                    //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                }
                                            }
                                        }





                                    }


                                    #endregion

                                    #region PARADA PERNOITE

                                    if (dt4.Rows[w][2].ToString() == "PARADA PERNOITE")
                                    {
                                        int a = 0;
                                        a = w - 1;


                                        if (a >= 0)
                                        {
                                            if (dt4.Rows[a][2].ToString() == "PARADA CLIENTE / FORNECEDOR")
                                            {


                                                if (dt4.Rows[a][5].ToString() == "14")
                                                {

                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0293");
                                                    marcacoes.Add(hora);
                                                    //WriteLine(cracha + ";" + data + ";" + hora + ";1;0102");
                                                    //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;0102");
                                                    marcacoes.Add(DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm").Replace(":", ""));



                                                }
                                                else if (dt4.Rows[a][5].ToString() == "15")
                                                {

                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                    marcacoes.Add(hora);
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;22");


                                                }
                                                //else if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2")
                                                //{
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                //    write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;22");
                                                //    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                //}
                                                else
                                                {
                                                    marcacoes.Add(hora);
                                                }



                                            }
                                            else if (dt4.Rows[a][2].ToString() == "PARADA")
                                            {
                                                //if (dt4.Rows[a][5].ToString() == "7")
                                                //{
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                //    write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;22");
                                                //}
                                                if (dt4.Rows[a][5].ToString() == "14")
                                                {

                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0293");
                                                    marcacoes.Add(hora);
                                                    //WriteLine(cracha + ";" + data + ";" + hora + ";1;0102");
                                                    //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;0102");
                                                    marcacoes.Add(DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm").Replace(":", ""));


                                                }
                                                else
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                    marcacoes.Add(hora);
                                                    //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                }

                                            }
                                            else if (dt4.Rows[a][2].ToString() == "INICIO DE VIAGEM")
                                            {
                                                marcacoes.Add(hora);
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";2;22");
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";3;22");
                                            }
                                            else if (dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                            {
                                                marcacoes.Add(hora);
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");

                                                // write.WriteLine(cracha + ";" + data + ";" + hora + ";3;22");
                                            }
                                            else if (dt4.Rows[a][2].ToString() == "RETORNO REFEICAO")
                                            {
                                                marcacoes.Add(hora);
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");

                                                // write.WriteLine(cracha + ";" + data + ";" + hora + ";3;22");
                                            }
                                            //else if (dt4.Rows[a][2].ToString() == "PARADA INTERNA")
                                            //{


                                            //    if (dt4.Rows[a][5].ToString() == "14")
                                            //    {

                                            //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                            //        //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;22");


                                            //    }
                                            //    //else if (dt4.Rows[a][5].ToString() == "15")
                                            //    //{

                                            //    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                            //    //    write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;22");


                                            //    //}
                                            //    else if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2")
                                            //    {
                                            //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                            //        //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;22");
                                            //        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                            //    }
                                            //    //else
                                            //    //{
                                            //    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;22");
                                            //    //}



                                            //}
                                            //else
                                            //{
                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;22");
                                            //}

                                        }
                                        //else
                                        //{
                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;22");
                                        //    write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;22");

                                        //}

                                    }

                                    #endregion

                                    #region RETORNO PERNOITE
                                    //BLOCO REINICIO / INICIO DE VIAGEM
                                    if (dt4.Rows[w][2].ToString() == "RETORNO PERNOITE")
                                    {
                                        int a = 0;
                                        a = w - 1;
                                        if (a >= 0)
                                        {
                                            if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[a][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[a][2].ToString() == "INICIO JORNADA")
                                            {

                                            }
                                            else if (dt4.Rows[a][2].ToString() == "PARADA PERNOITE")
                                            {
                                                //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01020000");
                                                marcacoes.Add(hora);
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                            }


                                        }
                                        else
                                        {
                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0102");
                                            marcacoes.Add(hora);
                                            // write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                        }



                                    }
                                    //FIM
                                    #endregion


                                }

                                //FIM

                                #endregion

                            }
                        }

                    }
                    else if (dt3.Rows[0][6].ToString() == "REINICIO DE VIAGEM" || dt3.Rows[0][6].ToString() == "RETORNO PERNOITE")
                    {
                        //VERIFICA SE A ULTIMA MARCAÇÃO DO DIA ANTERIOR É PARADA PERNOITE OU FIM DE VIAGEM
                        string sqlv = "select TOP 1 ds_macro from tb_parada where cod_cracha=" + lblCod.Text + " and dt_posicao_parada='" + DateTime.Parse(dte.Rows[f][0].ToString()).AddDays(-1).ToString("yyyy-MM-dd") + "' and fl_deletado is null order by hr_posicao  DESC ";
                        SqlDataAdapter adtpv = new SqlDataAdapter(sqlv, con);
                        DataTable dtv = new DataTable();
                        con.Open();
                        adtpv.Fill(dtv);
                        con.Close();

                        if (dtv.Rows.Count > 0)
                        {
                            if (dtv.Rows[0][0].ToString() == "PARADA PERNOITE" || dtv.Rows[0][0].ToString() == "FIM DE JORNADA")
                            {
                                //JORNADA NORMAL REINICIO DE VIAGEM

                                //VERIFICA SE A ULTIMA MARCAÇÃO DO DIA É PARADA PERNOITE OU FIM DE VIAGEM
                                string sqlu = "select ds_macro from tb_parada where cod_cracha=" + lblCod.Text + " and dt_posicao_parada='" + DateTime.Parse(dte.Rows[f][0].ToString()).ToString("yyyy-MM-dd") + "' and fl_deletado is null order by hr_posicao  DESC";
                                SqlDataAdapter adtpu = new SqlDataAdapter(sqlu, con);
                                DataTable dtu = new DataTable();
                                con.Open();
                                adtpu.Fill(dtu);
                                con.Close();


                                if (dtu.Rows[0][0].ToString() == "FIM DE JORNADA" || dtu.Rows[0][0].ToString() == "PARADA PERNOITE")
                                {
                                    //JORNADA NORMAL REINICIO DE VIAGEM
                                    dtRow["Dia"] = DateTime.Parse(dte.Rows[f][0].ToString()).ToString("dd");
                                    #region Jornada Normal
                                    //string arquivo = "Transmot" + DateTime.Parse(txtDtInicial.Text).ToString("ddMM") + "001.txt";

                                    //sql4 = "select dt_posicao_parada, hr_posicao, ds_macro,cod_transmissao, cod_cracha, isnull(cod_ref_parada,0) as cod_ref_parada, cod_idveiculo from tb_parada where fl_deletado is null and cod_cracha=" + lblCod.Text + " and dt_posicao_parada='" + DateTime.Parse(dte.Rows[f][0].ToString()).ToString("yyyy-MM-dd") + "' AND (ds_macro IN ('INICIO DE JORNADA', 'INICIO JORNADA CAMINHAO', 'PARADA REFEICAO', 'FIM DE JORNADA','REINICIO DE VIAGEM','FIM DE VIAGEM', 'PARADA','PARADA INTERNA','INICIO DE VIAGEM','RETORNO REFEICAO','PARADA CLIENTE / FORNECEDOR','PARADA PERNOITE','RETORNO PERNOITE') OR cod_ref_parada in (0,2,3,4,14,22)) AND fl_deletado IS NULL ORDER BY dt_posicao_parada, hr_posicao";
                                    string sql4 = "select dt_posicao_parada, hr_posicao, ds_macro,cod_transmissao, cod_cracha, isnull(cod_ref_parada,0) as cod_ref_parada, cod_idveiculo from tb_parada where fl_deletado is null and cod_cracha=" + lblCod.Text + " and dt_posicao_parada between '" + DateTime.Parse(dte.Rows[f][0].ToString()).ToString("yyyy-MM-dd") + "' and '" + DateTime.Parse(dte.Rows[f][0].ToString()).ToString("yyyy-MM-dd") + "' order by dt_posicao_parada, hr_posicao";
                                    SqlDataAdapter adtp4 = new SqlDataAdapter(sql4, con);
                                    DataTable dt4 = new DataTable();

                                    try
                                    {
                                        con.Open();
                                        adtp4.Fill(dt4);
                                        con.Close();
                                    }
                                    catch (Exception ed)
                                    {
                                        // MessageBox.Show(ed.ToString(), "Erro3");
                                    }

                                   

                                    c = 0;

                                    for (int y = 0; y < dt4.Rows.Count; y++)
                                    {
                                        if (dt4.Rows[y][2].ToString() == "FIM DE JORNADA" && dt4.Rows[y][6].ToString() == "725405" || dt4.Rows[y][2].ToString() == "FIM DE JORNADA" && dt4.Rows[y][6].ToString() == "725407" || dt4.Rows[y][2].ToString() == "FIM DE JORNADA" && dt4.Rows[y][6].ToString() == "184525")
                                        {
                                            c = c + 1;
                                        }

                                    }
                                    string caminho = Server.MapPath("/TXT/");

                                    string filepath = caminho + arquivo;

                                    FileInfo file = new FileInfo(filepath);

                                    //if (file.Exists)
                                    //{
                                    //    file.Delete();
                                    //}


                                    for (int w = 0; w < dt4.Rows.Count; w++)
                                    {


                                        string codigo = lblCod.Text;
                                        string[] codigosEspeciais = { "330", "482", "820", "958", "989" };

                                        if (codigo.StartsWith("312"))
                                        {
                                            cracha = codigo.Insert(3, "0");
                                        }
                                        else if ((codigosEspeciais.Contains(codigo) || codigo.Length <= 4))
                                        {
                                            cracha = "30" + codigo;
                                        }
                                        else if (codigo.Length <= 3)
                                        {
                                            cracha = "300" + codigo;
                                        }

                                        else
                                        {
                                            cracha = codigo;
                                        }




                                        int t = 0;
                                        t = w - 1;

                                        string data = DateTime.Parse(dt4.Rows[w][0].ToString()).ToString("dd/MM");
                                        string horas = DateTime.Parse(dt4.Rows[w][1].ToString()).ToString("HH:mm");
                                        if (t >= 0)
                                        {
                                            if (DateTime.Parse(dt4.Rows[w][1].ToString()).ToString("HH:mm") == DateTime.Parse(dt4.Rows[t][1].ToString()).ToString("HH:mm"))
                                            {
                                                hora = DateTime.Parse(horas).AddMinutes(1).ToString("HH:mm");
                                            }
                                            else
                                            {
                                                hora = DateTime.Parse(horas).ToString("HH:mm");
                                            }
                                        }
                                        else
                                        {
                                            hora = DateTime.Parse(horas).ToString("HH:mm");
                                        }
                                        //tipo = dt4.Rows[w][2].ToString();
                                        string transmissao = dt4.Rows[w][3].ToString();


                                        //if (dt4.Rows[w][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[w][2].ToString() == "TRECHO ALMOÇO" || dt4.Rows[w][2].ToString() == "FIM DE JORNADA")
                                        //{
                                        //    tipo = "2";
                                        //}
                                        //else  if (dt4.Rows[w][2].ToString() == "PARADA FORNECEDOR" || dt4.Rows[w][2].ToString() == "REINICIO DE VIAGEM")
                                        //{
                                        //    tipo = "21";
                                        //}











                                        #region BLOCO INICIO JORNADA
                                        //BLOCO INICIO JORNADA
                                        if (dt4.Rows[w][2].ToString() == "INICIO DE JORNADA")
                                        {
                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0102");
                                            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01020000");
                                            marcacoes.Add(hora);



                                        }

                                        if (dt4.Rows[w][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[w][2].ToString() == "INICIO JORNADA")
                                        {

                                            int a = 0;
                                            a = w - 1;
                                            if (a >= 0)
                                            {
                                                if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA")
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");

                                                }
                                                //else
                                                //{
                                                //	//write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0102");
                                                //	//write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01020000");
                                                //	marcacoes.Add(hora);



                                                //}
                                            }
                                            else
                                            {
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                            }


                                        }
                                        //FIM
                                        #endregion

                                        #region BLOCO INICIO JORNADA CAMINHAO
                                        //BLOCO INICIO JORNADA CAMINHAO

                                        if (dt4.Rows[w][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[w][2].ToString() == "INICIO JORNADA")
                                        {

                                            int a = 0;
                                            a = w - 1;
                                            if (a >= 0)
                                            {
                                                if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA")
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");

                                                }
                                                else
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0102");
                                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01020000");
                                                    marcacoes.Add(hora);
                                                }
                                            }
                                            else
                                            {
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                            }


                                        }
                                        //FIM
                                        #endregion

                                        #region PARADA INTERNA
                                        //BLOCO PARADA INTERNA
                                        if (dt4.Rows[w][2].ToString() == "PARADA INTERNA")
                                        {
                                            if (dt4.Rows[w][5].ToString() == "3" || dt4.Rows[w][5].ToString() == "2")
                                            {
                                                int a = 0;
                                                a = w - 1;
                                                if (a >= 0)
                                                {
                                                    if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[a][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[a][2].ToString() == "INICIO JORNADA")
                                                    {
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");
                                                    }
                                                    else if (dt4.Rows[a][5].ToString() == "14")
                                                    {
                                                        //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");
                                                        marcacoes.Add(hora);
                                                    }
                                                    else if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2" || dt4.Rows[a][5].ToString() == "1")
                                                    {
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                    }
                                                    else if (dt4.Rows[a][2].ToString() == "PARADA REFEICAO")
                                                    {
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");
                                                    }
                                                    else if (dt4.Rows[a][2].ToString() == "INICIO DE VIAGEM")
                                                    {
                                                        //////write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                        ////write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                        ////write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");
                                                    }
                                                    else if (dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                                    {
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;94");
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");
                                                    }
                                                    else if (dt4.Rows[a][2].ToString() == "FIM DE VIAGEM")
                                                    {
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                    }
                                                    else
                                                    {
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");
                                                    }

                                                }

                                            }
                                            else if (dt4.Rows[w][5].ToString() == "14")
                                            {
                                                int a = 0;
                                                a = w - 1;
                                                if (a >= 0)
                                                {
                                                    if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[a][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[a][2].ToString() == "INICIO JORNADA")
                                                    {
                                                        marcacoes.Add(hora);
                                                    }
                                                    if (dt4.Rows[a][5].ToString() == "14")
                                                    {
                                                        //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                        marcacoes.Add(hora);
                                                    }
                                                    else if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2" || dt4.Rows[a][5].ToString() == "1")
                                                    {
                                                        //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                        marcacoes.Add(hora);
                                                    }
                                                    else if (dt4.Rows[a][2].ToString() == "PARADA REFEICAO")
                                                    {
                                                        // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");
                                                    }
                                                    else if (dt4.Rows[a][2].ToString() == "INICIO DE VIAGEM" || dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                                    {
                                                        //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                        marcacoes.Add(hora);
                                                    }
                                                    else if (dt4.Rows[a][2].ToString() == "FIM DE VIAGEM")
                                                    {

                                                        //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                        marcacoes.Add(hora);

                                                    }
                                                    if (dt4.Rows[a][5].ToString() == "22")
                                                    {
                                                        //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                        marcacoes.Add(hora);
                                                    }
                                                    //else
                                                    //{
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93: 71489");
                                                    //    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                                    //}

                                                }
                                            }
                                            else if (dt4.Rows[w][5].ToString() == "22")
                                            {
                                                int a = 0;
                                                a = w - 1;
                                                if (a >= 0)
                                                {

                                                    if (dt4.Rows[a][5].ToString() == "14")
                                                    {
                                                        //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");
                                                        marcacoes.Add(hora);
                                                    }


                                                }
                                            }
                                            else
                                            {
                                                int a = 0;
                                                a = w - 1;
                                                if (a >= 0)
                                                {
                                                    if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[a][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[a][2].ToString() == "INICIO JORNADA")
                                                    {
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;95");
                                                    }
                                                    if (dt4.Rows[a][5].ToString() == "14")
                                                    {
                                                        //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");
                                                        marcacoes.Add(hora);

                                                    }

                                                    else if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2" || dt4.Rows[a][5].ToString() == "1")
                                                    {
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;95");
                                                    }
                                                    else if (dt4.Rows[a][2].ToString() == "PARADA REFEICAO")
                                                    {
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                                        //// write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");
                                                    }
                                                    else if (dt4.Rows[a][2].ToString() == "INICIO DE VIAGEM" || dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                                    {

                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;94");
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;95");
                                                    }
                                                    else if (dt4.Rows[a][2].ToString() == "FIM DE VIAGEM" || dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                                    {

                                                        //// write.WriteLine(cracha + ";" + data + ";" + hora + ";1;94");
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;95");

                                                    }

                                                }
                                            }

                                        }
                                        //FIM 
                                        #endregion

                                        #region BLOCO PARADA REFEIÇÃO
                                        //BLOCO PARADA REFEIÇÃO
                                        if (dt4.Rows[w][2].ToString() == "PARADA REFEICAO")
                                        {
                                            int a = 0;
                                            a = w - 1;
                                            if (a >= 0)
                                            {
                                                //if (dt4.Rows[a][2].ToString() == "PARADA INTERNA")
                                                //{

                                                //    if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2")
                                                //    {
                                                //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                //        write.WriteLine(cracha + ";" + data + ";" + hora + ";2;93");
                                                //    }
                                                //    else
                                                //    {
                                                //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");
                                                //        write.WriteLine(cracha + ";" + data + ";" + hora + ";2;93");
                                                //    }
                                                //}
                                                if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[a][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[a][2].ToString() == "INICIO JORNADA")
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0193");
                                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                    marcacoes.Add(hora);

                                                }
                                                else
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0193");
                                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                    marcacoes.Add(hora);
                                                }

                                            }
                                            //else
                                            //{
                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");
                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;93");
                                            //}
                                        }
                                        //FIM
                                        #endregion

                                        #region RETORNO REFEIÇAO
                                        //BLOCO RETORNO REFEIÇAO
                                        if (dt4.Rows[w][2].ToString() == "RETORNO REFEICAO")
                                        {

                                            int a = 0;
                                            int b = 0;
                                            a = w - 1;
                                            b = w + 1;
                                            if (a >= 0)
                                            {

                                                if (dt4.Rows[a][2].ToString() == "PARADA REFEICAO")
                                                {

                                                    // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0293");
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;0102");
                                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");
                                                    marcacoes.Add(hora);

                                                }
                                                //if (dt4.Rows[b][2].ToString() == "FIM DE JORNADA")
                                                //{
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                //}
                                                //else
                                                //{
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                                //    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");

                                                //}
                                            }


                                        }
                                        //FIM
                                        #endregion

                                        #region INICIO DE VIAGEM
                                        //BLOCO REINICIO / INICIO DE VIAGEM
                                        if (dt4.Rows[w][2].ToString() == "INICIO DE VIAGEM")
                                        {
                                            int a = 0;
                                            a = w - 1;
                                            if (a >= 0)
                                            {
                                                //if (dt4.Rows[a][2].ToString() == "PARADA INTERNA")
                                                //{
                                                //    if (dt4.Rows[a][5].ToString() == "14")
                                                //    {
                                                //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                                //        write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");

                                                //    }
                                                //    else
                                                //        if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2")
                                                //    {
                                                //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                //        write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");

                                                //    }
                                                //    else
                                                //    {
                                                //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");
                                                //        write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                //    }
                                                //}
                                                //if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[a][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[a][2].ToString() == "INICIO JORNADA")
                                                //{
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                //}
                                                //else if (dt4.Rows[a][2].ToString() == "PARADA PERNOITE")
                                                //{
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;22");
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                //}
                                                //else if (dt4.Rows[a][2].ToString() == "FIM DE VIAGEM")
                                                //{
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;94");
                                                //    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                //}

                                                if (dt4.Rows[a][2].ToString() == "PARADA CLIENTE / FORNECEDOR")
                                                {


                                                    if (dt4.Rows[a][5].ToString() == "14")
                                                    {

                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0193");
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                        //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                        marcacoes.Add(hora);



                                                    }
                                                    //else if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "1" || dt4.Rows[a][5].ToString() == "2" || dt4.Rows[a][5].ToString() == "4")
                                                    //{
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                    //}



                                                }
                                                else if (dt4.Rows[a][2].ToString() == "PARADA")
                                                {


                                                    if (dt4.Rows[a][5].ToString() == "14")
                                                    {

                                                        // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0193");
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                        //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                        marcacoes.Add(hora);
                                                    }
                                                    //else
                                                    //{
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");



                                                    //}



                                                }
                                                //else if (dt4.Rows[a][2].ToString() == "RETORNO REFEICAO")
                                                //{
                                                //    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0293");
                                                //    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                //    write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");

                                                //}

                                            }
                                            //else
                                            //{
                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                            //}



                                        }
                                        //FIM
                                        #endregion

                                        #region PARADA CLIENTE / FORNECEDOR
                                        if (dt4.Rows[w][2].ToString() == "PARADA CLIENTE / FORNECEDOR")
                                        {
                                            if (dt4.Rows[w][5].ToString() == "3" || dt4.Rows[w][5].ToString() == "2" || dt4.Rows[w][5].ToString() == "4")
                                            {
                                                int a = 0;
                                                a = w - 1;
                                                if (a >= 0)
                                                {
                                                    //if (dt4.Rows[a][2].ToString() == "INICIO DE VIAGEM" || dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM" || dt4.Rows[a][2].ToString() == "FIM DE VIAGEM")
                                                    //{
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;94");
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");
                                                    //}
                                                    if (dt4.Rows[a][2].ToString() == "PARADA CLIENTE / FORNECEDOR")
                                                    {
                                                        if (dt4.Rows[a][5].ToString() == "14")
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0293");
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;0102");
                                                            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");
                                                            marcacoes.Add(hora);


                                                        }
                                                        //else if (dt4.Rows[a][5].ToString() == "15")
                                                        //{
                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;94");
                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");

                                                        //}
                                                        //else
                                                        //{
                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");
                                                        //}

                                                    }
                                                }

                                            }
                                            else if (dt4.Rows[w][5].ToString() == "14")
                                            {
                                                int a = 0;
                                                a = w - 1;
                                                if (a >= 0)
                                                {
                                                    if (dt4.Rows[a][2].ToString() == "INICIO DE VIAGEM" || dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM" || dt4.Rows[a][2].ToString() == "FIM DE VIAGEM")
                                                    {
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                        // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0193");
                                                        //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                        marcacoes.Add(hora);
                                                    }
                                                    else if (dt4.Rows[a][2].ToString() == "PARADA CLIENTE / FORNECEDOR")
                                                    {
                                                        if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2" || dt4.Rows[a][5].ToString() == "4")
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0193");
                                                            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                            marcacoes.Add(hora);
                                                        }
                                                        else if (dt4.Rows[a][5].ToString() == "14")
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0193");
                                                            marcacoes.Add(hora);

                                                        }


                                                    }
                                                    else
                                                    {
                                                        //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                        marcacoes.Add(hora);
                                                    }

                                                }


                                            }


                                        }
                                        #endregion

                                        #region REINICIO
                                        //BLOCO REINICIO / INICIO DE VIAGEM
                                        if (dt4.Rows[w][2].ToString() == "REINICIO DE VIAGEM" || dt4.Rows[w][2].ToString() == "RETORNO PERNOITE")
                                        {
                                            int a = 0;
                                            a = w - 1;
                                            if (a >= 0)
                                            {
                                                if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[a][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[a][2].ToString() == "INICIO JORNADA")
                                                {

                                                }
                                                else if (dt4.Rows[a][2].ToString() == "PARADA PERNOITE")
                                                {
                                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01020000");
                                                    marcacoes.Add(hora);
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                }

                                                else if (dt4.Rows[a][2].ToString() == "PARADA CLIENTE / FORNECEDOR")
                                                {

                                                    if (a >= 0)
                                                    {

                                                        if (dt4.Rows[a][2].ToString() == "PARADA CLIENTE / FORNECEDOR")
                                                        {
                                                            //if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2")
                                                            //{
                                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                            //}
                                                            if (dt4.Rows[a][5].ToString() == "14")
                                                            {

                                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0293");
                                                                marcacoes.Add(hora);
                                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0102");

                                                            }
                                                            //else if (dt4.Rows[a][5].ToString() == "15")
                                                            //{

                                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");

                                                            //}
                                                            //else
                                                            //{
                                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                            //}
                                                        }
                                                        //else if (dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                                        //{
                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                        //}


                                                    }



                                                }
                                                else if (dt4.Rows[a][2].ToString() == "PARADA")
                                                {
                                                    if (dt4.Rows[a][5].ToString() == "14")
                                                    {
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0293");
                                                        //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0102");
                                                        marcacoes.Add(hora);
                                                    }
                                                    //else
                                                    //{
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                    //}




                                                }
                                                //else if (dt4.Rows[a][2].ToString() == "OFICINA / MANUTENCAO")
                                                //{

                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");


                                                //}

                                            }
                                            else
                                            {
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0102");
                                                marcacoes.Add(hora);
                                                // write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                            }



                                        }
                                        //FIM
                                        #endregion

                                        #region BLOCO PARADA
                                        // BLOCO PARADA
                                        if (dt4.Rows[w][2].ToString() == "PARADA")
                                        {
                                            int a = 0;
                                            a = w - 1;


                                            if (dt4.Rows[w][5].ToString() == "14")
                                            {
                                                if (dt4.Rows.Count >= a)
                                                {
                                                    if (dt4.Rows[a][2].ToString() == "INICIO DE VIAGEM" || dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                                    {
                                                        // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0193");
                                                        marcacoes.Add(hora);


                                                    }

                                                }
                                            }
                                            //else
                                            //{
                                            //    if (dt4.Rows.Count >= a)
                                            //    {
                                            //        if (dt4.Rows[a][2].ToString() == "INICIO DE VIAGEM" || dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                            //        {
                                            //            write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                            //            //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;95");

                                            //        }
                                            //        //else
                                            //        //{
                                            //        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                            //        //    // write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                            //        //}
                                            //    }
                                            //}



                                            //  write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");

                                        }
                                        //FIM
                                        #endregion

                                        #region FIM DE VIAGEM
                                        //BLOCO FIM DE VIAGEM
                                        if (dt4.Rows[w][2].ToString() == "FIM DE VIAGEM")
                                        {
                                            int a = 0;
                                            int g = 0;
                                            a = w - 1;
                                            g = w - 2;
                                            if (a >= 0)
                                            {
                                                if (dt4.Rows[a][2].ToString() == "PARADA CLIENTE / FORNECEDOR")
                                                {


                                                    if (dt4.Rows[a][5].ToString() == "14")
                                                    {

                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0293");
                                                        //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;0102");
                                                        marcacoes.Add(hora);

                                                    }

                                                    //else if (dt4.Rows[a][5].ToString() == "14")
                                                    //{
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                                    //    // write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                    //}

                                                    //else
                                                    //{
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                    //    // write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                    //}



                                                }
                                                else if (dt4.Rows[a][2].ToString() == "PARADA")
                                                {


                                                    if (dt4.Rows[a][5].ToString() == "14")
                                                    {
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0293");
                                                        //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");
                                                        marcacoes.Add(hora);
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;0102");

                                                    }

                                                    //else
                                                    //{
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                    //    // write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");

                                                    //}


                                                }

                                                else if (dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                                {
                                                    if (g >= 0)
                                                    {
                                                        //if (dt4.Rows[g][2].ToString() == "PARADA PERNOITE")
                                                        //{
                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                        //    write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                        //}
                                                        //else
                                                        //{
                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                        //}
                                                    }
                                                    //else
                                                    //{
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                    //}


                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                }


                                            }
                                            //else
                                            //{
                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                            //}

                                        }

                                        //
                                        #endregion

                                        #region FIM DE JORNADA
                                        //BLOCO FIM DE JORMADA
                                        if (c > 0)
                                        {
                                            if (dt4.Rows[w][2].ToString() == "FIM DE JORNADA" && dt4.Rows[w][6].ToString() == "725405" || dt4.Rows[w][2].ToString() == "FIM DE JORNADA" && dt4.Rows[w][6].ToString() == "725407" || dt4.Rows[w][2].ToString() == "FIM DE JORNADA" && dt4.Rows[w][6].ToString() == "184525" && dt4.Rows[w][6].ToString() == "184522")
                                            {
                                                int a = 0;
                                                a = w - 1;

                                                if (a >= 0)
                                                {
                                                    if (dt4.Rows[a][2].ToString() == "RETORNO REFEICAO")
                                                    {
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                        //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02020000");
                                                        //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                        marcacoes.Add(hora);

                                                    }
                                                    else if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA")
                                                    {

                                                        //WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                        //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02020000");
                                                        marcacoes.Add(hora);


                                                    }
                                                    else if (dt4.Rows[a][2].ToString() == "RETORNO REFEICAO")
                                                    {

                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                        //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02020000");
                                                        marcacoes.Add(hora);

                                                    }
                                                    else if (dt4.Rows[a][2].ToString() == "FIM DE VIAGEM")
                                                    {
                                                        int b = 0;
                                                        b = a - 1;

                                                        if (b >= 0)
                                                        {
                                                            if (dt4.Rows[b][2].ToString() == "INICIO DE VIAGEM" || dt4.Rows[b][2].ToString() == "REINICIO DE VIAGEM")
                                                            {
                                                                int d = 0;
                                                                d = b - 1;
                                                                if (d >= 0)
                                                                {

                                                                    if (dt4.Rows[d][2].ToString() == "PARADA")
                                                                    {
                                                                        if (dt4.Rows[d][5].ToString() == "14")
                                                                        {
                                                                            // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02020000");
                                                                            //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                                            marcacoes.Add(hora);
                                                                        }
                                                                        else
                                                                        {
                                                                            // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02020000");
                                                                            // write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                                            marcacoes.Add(hora);
                                                                        }




                                                                    }
                                                                    else
                                                                    {
                                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                        //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02020000");
                                                                        // write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                                        marcacoes.Add(hora);
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02020000");
                                                                // write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                                marcacoes.Add(hora);
                                                            }

                                                        }
                                                        else
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                            marcacoes.Add(hora);
                                                            //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");

                                                        }

                                                    }
                                                    else if (dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                                    {
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                        //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02020000");
                                                        //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                        marcacoes.Add(hora);
                                                    }
                                                    else if (dt4.Rows[a][2].ToString() == "PARADA")
                                                    {
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                        marcacoes.Add(hora);
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                    }
                                                    else if (dt4.Rows[a][2].ToString() == "PARADA INTERNA")
                                                    {
                                                        if (dt4.Rows[a][5].ToString() == "14")
                                                        {
                                                            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");
                                                            marcacoes.Add(hora);
                                                        }
                                                        else
                                                        {
                                                            marcacoes.Add(hora);
                                                        }
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");

                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                    }
                                                    else if (dt4.Rows[a][2].ToString() == "FIM DE JORNADA")
                                                    {
                                                        int m = 0;
                                                        m = a - 1;

                                                        if (m >= 0)
                                                        {
                                                            if (dt4.Rows[m][2].ToString() == "RETORNO REFEICAO" || dt4.Rows[m][2].ToString() == "INICIO DE JORNADA")
                                                            {
                                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                marcacoes.Add(hora);
                                                            }
                                                            else if (dt4.Rows[m][2].ToString() == "FIM DE VIAGEM")
                                                            {
                                                                int b = 0;
                                                                b = m - 1;

                                                                if (b >= 0)
                                                                {
                                                                    if (dt4.Rows[b][2].ToString() == "INICIO DE VIAGEM" || dt4.Rows[b][2].ToString() == "REINICIO DE VIAGEM")
                                                                    {
                                                                        int d = 0;
                                                                        d = b - 1;
                                                                        if (d >= 0)
                                                                        {

                                                                            if (dt4.Rows[d][2].ToString() == "PARADA")
                                                                            {
                                                                                if (dt4.Rows[d][5].ToString() == "14")
                                                                                {
                                                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                                    marcacoes.Add(hora);
                                                                                    //.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                                                }
                                                                                else
                                                                                {
                                                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                                    marcacoes.Add(hora);
                                                                                    //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");

                                                                                }


                                                                            }
                                                                            else
                                                                            {
                                                                                // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                                marcacoes.Add(hora);
                                                                                //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                                            }
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                        marcacoes.Add(hora);
                                                                        //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                                    }

                                                                }


                                                            }
                                                            //else if (dt4.Rows[m][2].ToString() == "REINICIO DE VIAGEM")
                                                            //{
                                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;94");
                                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                            //}
                                                            //else if (dt4.Rows[m][2].ToString() == "PARADA INTERNA")
                                                            //{
                                                            //    if (dt4.Rows[m][5].ToString() == "3" || dt4.Rows[m][5].ToString() == "1" || dt4.Rows[m][5].ToString() == "2")
                                                            //    {
                                                            //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");

                                                            //    }
                                                            //    else if (dt4.Rows[m][5].ToString() == "22")
                                                            //    {
                                                            //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");
                                                            //    }

                                                            //    else if (dt4.Rows[m][2].ToString() == "FIM DE JORNADA")
                                                            //    {
                                                            //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                            //        write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                            //    }

                                                            //}
                                                            else if (dt4.Rows[m][2].ToString() == "PARADA")
                                                            {
                                                                // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                marcacoes.Add(hora);
                                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                            }
                                                            //else if (dt4.Rows[m][2].ToString() == "PARADA INTERNA")
                                                            //{
                                                            //    if (dt4.Rows[m][5].ToString() != "3" || dt4.Rows[m][5].ToString() != "1" || dt4.Rows[m][5].ToString() != "2")
                                                            //    {
                                                            //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");
                                                            //        write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                            //        write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");

                                                            //    }

                                                            //}

                                                            else
                                                            {
                                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                marcacoes.Add(hora);
                                                                //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                            }


                                                        }
                                                        else
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                        marcacoes.Add(hora);
                                                        //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                    }


                                                }
                                                else
                                                {
                                                    // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                    marcacoes.Add(hora);
                                                    //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                }
                                            }

                                        }
                                        else
                                        {
                                            if (dt4.Rows[w][2].ToString() == "FIM DE JORNADA")
                                            {
                                                int q = 0;
                                                q = w + 1;

                                                if (q < dt4.Rows.Count)
                                                {
                                                    if (dt4.Rows[q][2].ToString() == "FIM DE JORNADA")
                                                    {
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                        marcacoes.Add(hora);
                                                        //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");

                                                    }
                                                    else
                                                    {
                                                        int a = 0;
                                                        a = w - 1;

                                                        if (a >= 0)
                                                        {

                                                            if (dt4.Rows[a][2].ToString() == "FIM DE VIAGEM" || dt4.Rows[a][2].ToString() == "RETORNO REFEICAO" || dt4.Rows[a][2].ToString() == "INICIO DE JORNADA")
                                                            {
                                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                marcacoes.Add(hora);
                                                                // write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");

                                                            }
                                                            //else if (dt4.Rows[a][2].ToString() == "PARADA INTERNA")
                                                            //{
                                                            //    if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2")
                                                            //    {
                                                            //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                            //        // write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                            //        // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                            //    }
                                                            //    else if (dt4.Rows[a][5].ToString() == "14")
                                                            //    {
                                                            //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                            //        write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                            //    }
                                                            //    else
                                                            //    {
                                                            //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");


                                                            //    }
                                                            //}
                                                            else if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[a][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[a][2].ToString() == "INICIO JORNADA")
                                                            {

                                                            }
                                                            else if (dt4.Rows[a][2].ToString() == "FIM DE JORNADA")
                                                            {
                                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                marcacoes.Add(hora);
                                                                //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                            }
                                                            else
                                                            {
                                                                // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                marcacoes.Add(hora);
                                                                // write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                            }
                                                        }
                                                    }

                                                }
                                                else
                                                {
                                                    int a = 0;
                                                    a = w - 1;

                                                    if (a >= 0)
                                                    {

                                                        if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA")
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                            marcacoes.Add(hora);

                                                        }
                                                        else if (dt4.Rows[a][2].ToString() == "FIM DE VIAGEM" || dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                            marcacoes.Add(hora);
                                                            //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");

                                                        }
                                                        else if (dt4.Rows[a][2].ToString() == "RETORNO REFEICAO")
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                            marcacoes.Add(hora);

                                                        }
                                                        else if (dt4.Rows[a][2].ToString() == "PARADA INTERNA")
                                                        {
                                                            if (dt4.Rows[a][5].ToString() == "14")
                                                            {
                                                                marcacoes.Add(hora);
                                                            }
                                                            else
                                                            {
                                                                marcacoes.Add(hora);
                                                            }
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");

                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                        }
                                                        //else if (dt4.Rows[a][2].ToString() == "PARADA INTERNA")
                                                        //{
                                                        //    if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2")
                                                        //    {
                                                        //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                        //        // write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                        //        // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                        //    }
                                                        //    else if (dt4.Rows[a][5].ToString() == "14")
                                                        //    {
                                                        //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                        //        write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                        //    }
                                                        //    else
                                                        //    {
                                                        //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");

                                                        //    }
                                                        //}
                                                        else if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[a][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[a][2].ToString() == "INICIO JORNADA")
                                                        {

                                                        }
                                                        else if (dt4.Rows[a][2].ToString() == "FIM DE JORNADA")
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                            marcacoes.Add(hora);

                                                            //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                        }
                                                        else
                                                        {
                                                            // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                            //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                        marcacoes.Add(hora);
                                                        //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                    }
                                                }
                                            }





                                        }


                                        #endregion

                                        #region PARADA PERNOITE

                                        if (dt4.Rows[w][2].ToString() == "PARADA PERNOITE")
                                        {
                                            int a = 0;
                                            a = w - 1;


                                            if (a >= 0)
                                            {
                                                if (dt4.Rows[a][2].ToString() == "PARADA CLIENTE / FORNECEDOR")
                                                {


                                                    if (dt4.Rows[a][5].ToString() == "14")
                                                    {

                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0293");
                                                        marcacoes.Add(hora);
                                                        //WriteLine(cracha + ";" + data + ";" + hora + ";1;0102");
                                                        //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;0102");
                                                        marcacoes.Add(DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm").Replace(":", ""));



                                                    }
                                                    else if (dt4.Rows[a][5].ToString() == "15")
                                                    {

                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                        marcacoes.Add(hora);
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;22");


                                                    }
                                                    //else if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2")
                                                    //{
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                    //    write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;22");
                                                    //    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                    //}
                                                    else
                                                    {
                                                        marcacoes.Add(hora);
                                                    }



                                                }
                                                else if (dt4.Rows[a][2].ToString() == "PARADA")
                                                {
                                                    //if (dt4.Rows[a][5].ToString() == "7")
                                                    //{
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                    //    write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;22");
                                                    //}
                                                    if (dt4.Rows[a][5].ToString() == "14")
                                                    {

                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0293");
                                                        marcacoes.Add(hora);
                                                        //WriteLine(cracha + ";" + data + ";" + hora + ";1;0102");
                                                        //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;0102");
                                                        marcacoes.Add(DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm").Replace(":", ""));


                                                    }
                                                    else
                                                    {
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                        marcacoes.Add(hora);
                                                        //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                    }

                                                }
                                                else if (dt4.Rows[a][2].ToString() == "INICIO DE VIAGEM")
                                                {
                                                    marcacoes.Add(hora);
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                    //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                    //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";2;22");
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";3;22");
                                                }
                                                else if (dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                                {
                                                    marcacoes.Add(hora);
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                    //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");

                                                    // write.WriteLine(cracha + ";" + data + ";" + hora + ";3;22");
                                                }
                                                else if (dt4.Rows[a][2].ToString() == "RETORNO REFEICAO")
                                                {
                                                    marcacoes.Add(hora);
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                    //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");

                                                    // write.WriteLine(cracha + ";" + data + ";" + hora + ";3;22");
                                                }
                                                //else if (dt4.Rows[a][2].ToString() == "PARADA INTERNA")
                                                //{


                                                //    if (dt4.Rows[a][5].ToString() == "14")
                                                //    {

                                                //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                                //        //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;22");


                                                //    }
                                                //    //else if (dt4.Rows[a][5].ToString() == "15")
                                                //    //{

                                                //    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                //    //    write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;22");


                                                //    //}
                                                //    else if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2")
                                                //    {
                                                //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                //        //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;22");
                                                //        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                //    }
                                                //    //else
                                                //    //{
                                                //    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;22");
                                                //    //}



                                                //}
                                                //else
                                                //{
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;22");
                                                //}

                                            }
                                            //else
                                            //{
                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;22");
                                            //    write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;22");

                                            //}

                                        }

                                        #endregion

                                        //#region RETORNO PERNOITE
                                        ////BLOCO REINICIO / INICIO DE VIAGEM
                                        //if (dt4.Rows[w][2].ToString() == "RETORNO PERNOITE")
                                        //{
                                        //    int a = 0;
                                        //    a = w - 1;
                                        //    if (a >= 0)
                                        //    {
                                        //        if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[a][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[a][2].ToString() == "INICIO JORNADA")
                                        //        {

                                        //        }
                                        //        else if (dt4.Rows[a][2].ToString() == "PARADA PERNOITE")
                                        //        {
                                        //            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01020000");
                                        //            marcacoes.Add(hora);
                                        //            //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                        //        }


                                        //    }
                                        //    else
                                        //    {
                                        //        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0102");
                                        //        marcacoes.Add(hora);
                                        //        // write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                        //    }



                                        //}
                                        ////FIM
                                        //#endregion






                                    }

                                    //FIM

                                    #endregion

                                }
                                else
                                {
                                    //JORNADA PERNOITE REINICIO DE VIAGEM
                                    string sqli = "select top 1  hr_posicao from tb_parada where cod_cracha=" + lblCod.Text + " and ds_macro = 'REINICIO DE VIAGEM' and dt_posicao_parada='" + DateTime.Parse(dte.Rows[f][0].ToString()).ToString("yyyy-MM-dd") + "' and fl_deletado is null OR ds_macro = 'RETORNO PERNOITE' and dt_posicao_parada='" + DateTime.Parse(dte.Rows[f][0].ToString()).ToString("yyyy-MM-dd") + "' and fl_deletado is null order by hr_posicao";
                                    SqlDataAdapter adtpi = new SqlDataAdapter(sqli, con);
                                    DataTable dti = new DataTable();
                                    con.Open();
                                    adtpi.Fill(dti);
                                    con.Close();

                                    string sqlf = "select top 1 hr_posicao from tb_parada where cod_cracha=" + lblCod.Text + " and ds_macro = 'FIM DE JORNADA' and dt_posicao_parada='" + DateTime.Parse(dte.Rows[f][0].ToString()).AddDays(1).ToString("yyyy-MM-dd") + "' and fl_deletado is null OR cod_cracha=" + lblCod.Text + " and ds_macro='PARADA PERNOITE' and dt_posicao_parada='" + DateTime.Parse(dte.Rows[f][0].ToString()).AddDays(1).ToString("yyyy-MM-dd") + "' and fl_deletado is null order by hr_posicao";
                                    SqlDataAdapter adtpf = new SqlDataAdapter(sqlf, con);
                                    DataTable dtf = new DataTable();
                                    con.Open();
                                    adtpf.Fill(dtf);
                                    con.Close();



                                    if (dti.Rows.Count > 0 && dtf.Rows.Count > 0)
                                    {
                                        dtRow["Dia"] = DateTime.Parse(dte.Rows[f][0].ToString()).ToString("dd");
                                        #region Jornada Noturna


                                        //sql4 = "select dt_posicao_parada, hr_posicao, ds_macro,cod_transmissao, cod_cracha, isnull(cod_ref_parada,0) as cod_ref_parada, cod_idveiculo from tb_parada where fl_deletado is null and cod_cracha=" + lblCod.Text + " and dt_posicao_parada='" + txtDtInicial.Text + "' order by dt_posicao_parada, hr_posicao";
                                        sql4 = "select dt_posicao_parada, hr_posicao, ds_macro,cod_transmissao, cod_cracha, isnull(cod_ref_parada,0) as cod_ref_parada, cod_idveiculo from tb_parada where fl_deletado is null and cod_cracha=" + lblCod.Text + " and dt_posicao_parada='" + DateTime.Parse(dte.Rows[f][0].ToString()).ToString("yyyy-MM-dd") + "' and hr_posicao >='" + dti.Rows[0][0].ToString() + "' or cod_cracha=" + lblCod.Text + " and dt_posicao_parada='" + DateTime.Parse(dte.Rows[f][0].ToString()).AddDays(1).ToString("yyyy-MM-dd") + "' and hr_posicao <= '" + dtf.Rows[0][0].ToString() + "' AND (ds_macro IN ('INICIO DE JORNADA', 'INICIO JORNADA CAMINHAO', 'PARADA REFEICAO', 'FIM DE JORNADA','REINICIO DE VIAGEM','FIM DE VIAGEM', 'PARADA','PARADA INTERNA','INICIO DE VIAGEM','PARADA INTERNA','RETORNO REFEICAO','PARADA CLIENTE / FORNECEDOR','PARADA PERNOITE','RETORNO PERNOITE') OR cod_ref_parada in (0,2,3,4,14,22)) AND fl_deletado IS NULL ORDER BY dt_posicao_parada, hr_posicao";
                                        // string sql4 = "select dt_posicao_parada, hr_posicao, ds_macro,cod_transmissao, cod_cracha, isnull(cod_ref_parada,0) as cod_ref_parada, cod_idveiculo from tb_parada where fl_deletado is null and cod_cracha=" + lblCod.Text + " and dt_posicao_parada between '" + txtDtInicial.Text + "' and '" + txtDtFinal.Text + "' order by dt_posicao_parada, hr_posicao";
                                        SqlDataAdapter adtp4 = new SqlDataAdapter(sql4, con);
                                        DataTable dt4 = new DataTable();

                                        try
                                        {
                                            con.Open();
                                            adtp4.Fill(dt4);
                                            con.Close();
                                        }
                                        catch (Exception ed)
                                        {
                                            //MessageBox.Show(ed.ToString(), "Erro3");
                                        }

                                        c = 0;

                                        for (int y = 0; y < dt4.Rows.Count; y++)
                                        {
                                            if (dt4.Rows[y][2].ToString() == "FIM DE JORNADA" && dt4.Rows[y][6].ToString() == "725405" || dt4.Rows[y][2].ToString() == "FIM DE JORNADA" && dt4.Rows[y][6].ToString() == "725407" || dt4.Rows[y][2].ToString() == "FIM DE JORNADA" && dt4.Rows[y][6].ToString() == "184525")
                                            {
                                                c = c + 1;
                                            }

                                        }
                                        string caminho = Server.MapPath("/TXT/");

                                        string filepath = caminho + arquivo;

                                        FileInfo file = new FileInfo(filepath);
                                        //if(file.Exists)
                                        //{
                                        //    file.Delete();
                                        //}



                                        for (int w = 0; w < dt4.Rows.Count; w++)
                                        {

                                            string codigo = lblCod.Text;
                                            string[] codigosEspeciais = { "330", "482", "820", "958", "989" };

                                            if (codigo.StartsWith("312"))
                                            {
                                                cracha = codigo.Insert(3, "0");
                                            }
                                            else if ((codigosEspeciais.Contains(codigo) || codigo.Length <= 4))
                                            {
                                                cracha = "30" + codigo;
                                            }
                                            else if (codigo.Length <= 3)
                                            {
                                                cracha = "300" + codigo;
                                            }

                                            else
                                            {
                                                cracha = codigo;
                                            }

                                            int t = 0;
                                            t = w - 1;

                                            string data = DateTime.Parse(dt4.Rows[w][0].ToString()).ToString("dd/MM");
                                            string horas = DateTime.Parse(dt4.Rows[w][1].ToString()).ToString("HH:mm");
                                            if (t >= 0)
                                            {
                                                if (DateTime.Parse(dt4.Rows[w][1].ToString()).ToString("HH:mm") == DateTime.Parse(dt4.Rows[t][1].ToString()).ToString("HH:mm"))
                                                {
                                                    hora = DateTime.Parse(horas).AddMinutes(1).ToString("HH:mm");
                                                }
                                                else
                                                {
                                                    hora = DateTime.Parse(horas).ToString("HH:mm");
                                                }
                                            }
                                            else
                                            {
                                                hora = DateTime.Parse(horas).ToString("HH:mm");
                                            }
                                            //tipo = dt4.Rows[w][2].ToString();
                                            string transmissao = dt4.Rows[w][3].ToString();


                                            //if (dt4.Rows[w][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[w][2].ToString() == "TRECHO ALMOÇO" || dt4.Rows[w][2].ToString() == "FIM DE JORNADA")
                                            //{
                                            //    tipo = "2";
                                            //}
                                            //else  if (dt4.Rows[w][2].ToString() == "PARADA FORNECEDOR" || dt4.Rows[w][2].ToString() == "REINICIO DE VIAGEM")
                                            //{
                                            //    tipo = "21";
                                            //}





                                            #region BLOCO INICIO JORNADA
                                            //BLOCO INICIO JORNADA
                                            if (dt4.Rows[w][2].ToString() == "INICIO DE JORNADA")
                                            {
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0102");
                                                //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01020000");
                                                marcacoes.Add(hora);



                                            }

                                            if (dt4.Rows[w][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[w][2].ToString() == "INICIO JORNADA")
                                            {

                                                int a = 0;
                                                a = w - 1;
                                                if (a >= 0)
                                                {
                                                    if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA")
                                                    {
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");

                                                    }
                                                    //else
                                                    //{
                                                    //	//write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0102");
                                                    //	//write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01020000");
                                                    //	marcacoes.Add(hora);



                                                    //}
                                                }
                                                else
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                }


                                            }
                                            //FIM
                                            #endregion

                                            #region BLOCO INICIO JORNADA CAMINHAO
                                            //BLOCO INICIO JORNADA CAMINHAO

                                            if (dt4.Rows[w][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[w][2].ToString() == "INICIO JORNADA")
                                            {

                                                int a = 0;
                                                a = w - 1;
                                                if (a >= 0)
                                                {
                                                    if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA")
                                                    {
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");

                                                    }
                                                    else
                                                    {
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0102");
                                                        //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01020000");
                                                        marcacoes.Add(hora);
                                                    }
                                                }
                                                else
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                }


                                            }
                                            //FIM
                                            #endregion

                                            #region PARADA INTERNA
                                            //BLOCO PARADA INTERNA
                                            if (dt4.Rows[w][2].ToString() == "PARADA INTERNA")
                                            {
                                                if (dt4.Rows[w][5].ToString() == "3" || dt4.Rows[w][5].ToString() == "2")
                                                {
                                                    int a = 0;
                                                    a = w - 1;
                                                    if (a >= 0)
                                                    {
                                                        if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[a][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[a][2].ToString() == "INICIO JORNADA")
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");
                                                        }
                                                        else if (dt4.Rows[a][5].ToString() == "14")
                                                        {
                                                            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");
                                                            marcacoes.Add(hora);
                                                        }
                                                        else if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2" || dt4.Rows[a][5].ToString() == "1")
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                        }
                                                        else if (dt4.Rows[a][2].ToString() == "PARADA REFEICAO")
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");
                                                        }
                                                        else if (dt4.Rows[a][2].ToString() == "INICIO DE VIAGEM")
                                                        {
                                                            //////write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                            ////write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                            ////write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");
                                                        }
                                                        else if (dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;94");
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");
                                                        }
                                                        else if (dt4.Rows[a][2].ToString() == "FIM DE VIAGEM")
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                        }
                                                        else
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");
                                                        }

                                                    }

                                                }
                                                else if (dt4.Rows[w][5].ToString() == "14")
                                                {
                                                    int a = 0;
                                                    a = w - 1;
                                                    if (a >= 0)
                                                    {
                                                        if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[a][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[a][2].ToString() == "INICIO JORNADA")
                                                        {
                                                            marcacoes.Add(hora);
                                                        }
                                                        if (dt4.Rows[a][5].ToString() == "14")
                                                        {
                                                            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                            marcacoes.Add(hora);
                                                        }
                                                        else if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2" || dt4.Rows[a][5].ToString() == "1")
                                                        {
                                                            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                            marcacoes.Add(hora);
                                                        }
                                                        else if (dt4.Rows[a][2].ToString() == "PARADA REFEICAO")
                                                        {
                                                            // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");
                                                        }
                                                        else if (dt4.Rows[a][2].ToString() == "INICIO DE VIAGEM" || dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                                        {
                                                            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                            marcacoes.Add(hora);
                                                        }
                                                        else if (dt4.Rows[a][2].ToString() == "FIM DE VIAGEM")
                                                        {

                                                            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                            marcacoes.Add(hora);

                                                        }
                                                        if (dt4.Rows[a][5].ToString() == "22")
                                                        {
                                                            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                            marcacoes.Add(hora);
                                                        }
                                                        //else
                                                        //{
                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93: 71489");
                                                        //    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                                        //}

                                                    }
                                                }
                                                else if (dt4.Rows[w][5].ToString() == "22")
                                                {
                                                    int a = 0;
                                                    a = w - 1;
                                                    if (a >= 0)
                                                    {

                                                        if (dt4.Rows[a][5].ToString() == "14")
                                                        {
                                                            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");
                                                            marcacoes.Add(hora);
                                                        }


                                                    }
                                                }
                                                else
                                                {
                                                    int a = 0;
                                                    a = w - 1;
                                                    if (a >= 0)
                                                    {
                                                        if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[a][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[a][2].ToString() == "INICIO JORNADA")
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;95");
                                                        }
                                                        if (dt4.Rows[a][5].ToString() == "14")
                                                        {
                                                            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");
                                                            marcacoes.Add(hora);

                                                        }

                                                        else if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2" || dt4.Rows[a][5].ToString() == "1")
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;95");
                                                        }
                                                        else if (dt4.Rows[a][2].ToString() == "PARADA REFEICAO")
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                                            //// write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");
                                                        }
                                                        else if (dt4.Rows[a][2].ToString() == "INICIO DE VIAGEM" || dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                                        {

                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;94");
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;95");
                                                        }
                                                        else if (dt4.Rows[a][2].ToString() == "FIM DE VIAGEM" || dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                                        {

                                                            //// write.WriteLine(cracha + ";" + data + ";" + hora + ";1;94");
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;95");

                                                        }

                                                    }
                                                }

                                            }
                                            //FIM 
                                            #endregion

                                            #region BLOCO PARADA REFEIÇÃO
                                            //BLOCO PARADA REFEIÇÃO
                                            if (dt4.Rows[w][2].ToString() == "PARADA REFEICAO")
                                            {
                                                int a = 0;
                                                a = w - 1;
                                                if (a >= 0)
                                                {
                                                    //if (dt4.Rows[a][2].ToString() == "PARADA INTERNA")
                                                    //{

                                                    //    if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2")
                                                    //    {
                                                    //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                    //        write.WriteLine(cracha + ";" + data + ";" + hora + ";2;93");
                                                    //    }
                                                    //    else
                                                    //    {
                                                    //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");
                                                    //        write.WriteLine(cracha + ";" + data + ";" + hora + ";2;93");
                                                    //    }
                                                    //}
                                                    if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[a][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[a][2].ToString() == "INICIO JORNADA")
                                                    {
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0193");
                                                        //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                        marcacoes.Add(hora);

                                                    }
                                                    else
                                                    {
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0193");
                                                        //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                        marcacoes.Add(hora);
                                                    }

                                                }
                                                //else
                                                //{
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;93");
                                                //}
                                            }
                                            //FIM
                                            #endregion

                                            #region RETORNO REFEIÇAO
                                            //BLOCO RETORNO REFEIÇAO
                                            if (dt4.Rows[w][2].ToString() == "RETORNO REFEICAO")
                                            {

                                                int a = 0;
                                                int b = 0;
                                                a = w - 1;
                                                b = w + 1;
                                                if (a >= 0)
                                                {

                                                    if (dt4.Rows[a][2].ToString() == "PARADA REFEICAO")
                                                    {

                                                        // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0293");
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;0102");
                                                        //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");
                                                        marcacoes.Add(hora);

                                                    }
                                                    //if (dt4.Rows[b][2].ToString() == "FIM DE JORNADA")
                                                    //{
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                    //}
                                                    //else
                                                    //{
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                                    //    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");

                                                    //}
                                                }


                                            }
                                            //FIM
                                            #endregion

                                            #region INICIO DE VIAGEM
                                            //BLOCO REINICIO / INICIO DE VIAGEM
                                            if (dt4.Rows[w][2].ToString() == "INICIO DE VIAGEM")
                                            {
                                                int a = 0;
                                                a = w - 1;
                                                if (a >= 0)
                                                {
                                                    //if (dt4.Rows[a][2].ToString() == "PARADA INTERNA")
                                                    //{
                                                    //    if (dt4.Rows[a][5].ToString() == "14")
                                                    //    {
                                                    //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                                    //        write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");

                                                    //    }
                                                    //    else
                                                    //        if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2")
                                                    //    {
                                                    //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                    //        write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");

                                                    //    }
                                                    //    else
                                                    //    {
                                                    //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");
                                                    //        write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                    //    }
                                                    //}
                                                    //if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[a][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[a][2].ToString() == "INICIO JORNADA")
                                                    //{
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                    //}
                                                    //else if (dt4.Rows[a][2].ToString() == "PARADA PERNOITE")
                                                    //{
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;22");
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                    //}
                                                    //else if (dt4.Rows[a][2].ToString() == "FIM DE VIAGEM")
                                                    //{
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;94");
                                                    //    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                    //}

                                                    if (dt4.Rows[a][2].ToString() == "PARADA CLIENTE / FORNECEDOR")
                                                    {


                                                        if (dt4.Rows[a][5].ToString() == "14")
                                                        {

                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0193");
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                            marcacoes.Add(hora);



                                                        }
                                                        //else if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "1" || dt4.Rows[a][5].ToString() == "2" || dt4.Rows[a][5].ToString() == "4")
                                                        //{
                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                        //}



                                                    }
                                                    else if (dt4.Rows[a][2].ToString() == "PARADA")
                                                    {


                                                        if (dt4.Rows[a][5].ToString() == "14")
                                                        {

                                                            // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0193");
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                            marcacoes.Add(hora);
                                                        }
                                                        //else
                                                        //{
                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");
                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");



                                                        //}



                                                    }
                                                    //else if (dt4.Rows[a][2].ToString() == "RETORNO REFEICAO")
                                                    //{
                                                    //    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0293");
                                                    //    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                    //    write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");

                                                    //}

                                                }
                                                //else
                                                //{
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                //}



                                            }
                                            //FIM
                                            #endregion

                                            #region PARADA CLIENTE / FORNECEDOR
                                            if (dt4.Rows[w][2].ToString() == "PARADA CLIENTE / FORNECEDOR")
                                            {
                                                if (dt4.Rows[w][5].ToString() == "3" || dt4.Rows[w][5].ToString() == "2" || dt4.Rows[w][5].ToString() == "4")
                                                {
                                                    int a = 0;
                                                    a = w - 1;
                                                    if (a >= 0)
                                                    {
                                                        //if (dt4.Rows[a][2].ToString() == "INICIO DE VIAGEM" || dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM" || dt4.Rows[a][2].ToString() == "FIM DE VIAGEM")
                                                        //{
                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;94");
                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");
                                                        //}
                                                        if (dt4.Rows[a][2].ToString() == "PARADA CLIENTE / FORNECEDOR")
                                                        {
                                                            if (dt4.Rows[a][5].ToString() == "14")
                                                            {
                                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0293");
                                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;0102");
                                                                //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");
                                                                marcacoes.Add(hora);


                                                            }
                                                            //else if (dt4.Rows[a][5].ToString() == "15")
                                                            //{
                                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;94");
                                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");

                                                            //}
                                                            //else
                                                            //{
                                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");
                                                            //}

                                                        }
                                                    }

                                                }
                                                else if (dt4.Rows[w][5].ToString() == "14")
                                                {
                                                    int a = 0;
                                                    a = w - 1;
                                                    if (a >= 0)
                                                    {
                                                        if (dt4.Rows[a][2].ToString() == "INICIO DE VIAGEM" || dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM" || dt4.Rows[a][2].ToString() == "FIM DE VIAGEM")
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                            // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0193");
                                                            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                            marcacoes.Add(hora);
                                                        }
                                                        else if (dt4.Rows[a][2].ToString() == "PARADA CLIENTE / FORNECEDOR")
                                                        {
                                                            if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2" || dt4.Rows[a][5].ToString() == "4")
                                                            {
                                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0193");
                                                                //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                                marcacoes.Add(hora);
                                                            }
                                                            else if (dt4.Rows[a][5].ToString() == "14")
                                                            {
                                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0193");
                                                                marcacoes.Add(hora);

                                                            }


                                                        }
                                                        else
                                                        {
                                                            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                            marcacoes.Add(hora);
                                                        }

                                                    }


                                                }


                                            }
                                            #endregion

                                            #region REINICIO
                                            //BLOCO REINICIO / INICIO DE VIAGEM
                                            if (dt4.Rows[w][2].ToString() == "REINICIO DE VIAGEM" || dt4.Rows[w][2].ToString() == "RETORNO PERNOITE")
                                            {
                                                int a = 0;
                                                a = w - 1;
                                                if (a >= 0)
                                                {
                                                    if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[a][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[a][2].ToString() == "INICIO JORNADA")
                                                    {

                                                    }
                                                    else if (dt4.Rows[a][2].ToString() == "PARADA PERNOITE")
                                                    {
                                                        //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01020000");
                                                        marcacoes.Add(hora);
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                    }

                                                    else if (dt4.Rows[a][2].ToString() == "PARADA CLIENTE / FORNECEDOR")
                                                    {

                                                        if (a >= 0)
                                                        {

                                                            if (dt4.Rows[a][2].ToString() == "PARADA CLIENTE / FORNECEDOR")
                                                            {
                                                                //if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2")
                                                                //{
                                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                                //}
                                                                if (dt4.Rows[a][5].ToString() == "14")
                                                                {

                                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0293");
                                                                    marcacoes.Add(hora);
                                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0102");

                                                                }
                                                                //else if (dt4.Rows[a][5].ToString() == "15")
                                                                //{

                                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");

                                                                //}
                                                                //else
                                                                //{
                                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                                //}
                                                            }
                                                            //else if (dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                                            //{
                                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                            //}


                                                        }



                                                    }
                                                    else if (dt4.Rows[a][2].ToString() == "PARADA")
                                                    {
                                                        if (dt4.Rows[a][5].ToString() == "14")
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0293");
                                                            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0102");
                                                            marcacoes.Add(hora);
                                                        }
                                                        //else
                                                        //{
                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");
                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                        //}




                                                    }
                                                    //else if (dt4.Rows[a][2].ToString() == "OFICINA / MANUTENCAO")
                                                    //{

                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");


                                                    //}

                                                }
                                                else
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0102");
                                                    marcacoes.Add(hora);
                                                    // write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                }



                                            }
                                            //FIM
                                            #endregion

                                            #region BLOCO PARADA
                                            // BLOCO PARADA
                                            if (dt4.Rows[w][2].ToString() == "PARADA")
                                            {
                                                int a = 0;
                                                a = w - 1;


                                                if (dt4.Rows[w][5].ToString() == "14")
                                                {
                                                    if (dt4.Rows.Count >= a)
                                                    {
                                                        if (dt4.Rows[a][2].ToString() == "INICIO DE VIAGEM" || dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                                        {
                                                            // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0193");
                                                            marcacoes.Add(hora);


                                                        }

                                                    }
                                                }
                                                //else
                                                //{
                                                //    if (dt4.Rows.Count >= a)
                                                //    {
                                                //        if (dt4.Rows[a][2].ToString() == "INICIO DE VIAGEM" || dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                                //        {
                                                //            write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                //            //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;95");

                                                //        }
                                                //        //else
                                                //        //{
                                                //        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                //        //    // write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                //        //}
                                                //    }
                                                //}



                                                //  write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");

                                            }
                                            //FIM
                                            #endregion

                                            #region FIM DE VIAGEM
                                            //BLOCO FIM DE VIAGEM
                                            if (dt4.Rows[w][2].ToString() == "FIM DE VIAGEM")
                                            {
                                                int a = 0;
                                                int g = 0;
                                                a = w - 1;
                                                g = w - 2;
                                                if (a >= 0)
                                                {
                                                    if (dt4.Rows[a][2].ToString() == "PARADA CLIENTE / FORNECEDOR")
                                                    {


                                                        if (dt4.Rows[a][5].ToString() == "14")
                                                        {

                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0293");
                                                            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;0102");
                                                            marcacoes.Add(hora);

                                                        }

                                                        //else if (dt4.Rows[a][5].ToString() == "14")
                                                        //{
                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                                        //    // write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                        //}

                                                        //else
                                                        //{
                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                        //    // write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                        //}



                                                    }
                                                    else if (dt4.Rows[a][2].ToString() == "PARADA")
                                                    {


                                                        if (dt4.Rows[a][5].ToString() == "14")
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0293");
                                                            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");
                                                            marcacoes.Add(hora);
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;0102");

                                                        }

                                                        //else
                                                        //{
                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                        //    // write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");

                                                        //}


                                                    }

                                                    else if (dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                                    {
                                                        if (g >= 0)
                                                        {
                                                            //if (dt4.Rows[g][2].ToString() == "PARADA PERNOITE")
                                                            //{
                                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                            //    write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                            //}
                                                            //else
                                                            //{
                                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                            //}
                                                        }
                                                        //else
                                                        //{
                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                        //}


                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                    }


                                                }
                                                //else
                                                //{
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                //}

                                            }

                                            //
                                            #endregion

                                            #region FIM DE JORNADA
                                            //BLOCO FIM DE JORMADA
                                            if (c > 0)
                                            {
                                                if (dt4.Rows[w][2].ToString() == "FIM DE JORNADA" && dt4.Rows[w][6].ToString() == "725405" || dt4.Rows[w][2].ToString() == "FIM DE JORNADA" && dt4.Rows[w][6].ToString() == "725407" || dt4.Rows[w][2].ToString() == "FIM DE JORNADA" && dt4.Rows[w][6].ToString() == "184525" && dt4.Rows[w][6].ToString() == "184522")
                                                {
                                                    int a = 0;
                                                    a = w - 1;

                                                    if (a >= 0)
                                                    {
                                                        if (dt4.Rows[a][2].ToString() == "RETORNO REFEICAO")
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02020000");
                                                            //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                            marcacoes.Add(hora);

                                                        }
                                                        else if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA")
                                                        {

                                                            //WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02020000");
                                                            marcacoes.Add(hora);


                                                        }
                                                        else if (dt4.Rows[a][2].ToString() == "RETORNO REFEICAO")
                                                        {

                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02020000");
                                                            marcacoes.Add(hora);

                                                        }
                                                        else if (dt4.Rows[a][2].ToString() == "FIM DE VIAGEM")
                                                        {
                                                            int b = 0;
                                                            b = a - 1;

                                                            if (b >= 0)
                                                            {
                                                                if (dt4.Rows[b][2].ToString() == "INICIO DE VIAGEM" || dt4.Rows[b][2].ToString() == "REINICIO DE VIAGEM")
                                                                {
                                                                    int d = 0;
                                                                    d = b - 1;
                                                                    if (d >= 0)
                                                                    {

                                                                        if (dt4.Rows[d][2].ToString() == "PARADA")
                                                                        {
                                                                            if (dt4.Rows[d][5].ToString() == "14")
                                                                            {
                                                                                // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                                //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02020000");
                                                                                //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                                                marcacoes.Add(hora);
                                                                            }
                                                                            else
                                                                            {
                                                                                // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                                //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02020000");
                                                                                // write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                                                marcacoes.Add(hora);
                                                                            }




                                                                        }
                                                                        else
                                                                        {
                                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02020000");
                                                                            // write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                                            marcacoes.Add(hora);
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02020000");
                                                                    // write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                                    marcacoes.Add(hora);
                                                                }

                                                            }
                                                            else
                                                            {
                                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                marcacoes.Add(hora);
                                                                //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");

                                                            }

                                                        }
                                                        else if (dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02020000");
                                                            //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                            marcacoes.Add(hora);
                                                        }
                                                        else if (dt4.Rows[a][2].ToString() == "PARADA")
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                            marcacoes.Add(hora);
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                        }
                                                        else if (dt4.Rows[a][2].ToString() == "PARADA INTERNA")
                                                        {
                                                            if (dt4.Rows[a][5].ToString() == "14")
                                                            {
                                                                //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");
                                                                marcacoes.Add(hora);
                                                            }
                                                            else
                                                            {
                                                                marcacoes.Add(hora);
                                                            }
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");

                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                        }
                                                        else if (dt4.Rows[a][2].ToString() == "FIM DE JORNADA")
                                                        {
                                                            int m = 0;
                                                            m = a - 1;

                                                            if (m >= 0)
                                                            {
                                                                if (dt4.Rows[m][2].ToString() == "RETORNO REFEICAO" || dt4.Rows[m][2].ToString() == "INICIO DE JORNADA")
                                                                {
                                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                    marcacoes.Add(hora);
                                                                }
                                                                else if (dt4.Rows[m][2].ToString() == "FIM DE VIAGEM")
                                                                {
                                                                    int b = 0;
                                                                    b = m - 1;

                                                                    if (b >= 0)
                                                                    {
                                                                        if (dt4.Rows[b][2].ToString() == "INICIO DE VIAGEM" || dt4.Rows[b][2].ToString() == "REINICIO DE VIAGEM")
                                                                        {
                                                                            int d = 0;
                                                                            d = b - 1;
                                                                            if (d >= 0)
                                                                            {

                                                                                if (dt4.Rows[d][2].ToString() == "PARADA")
                                                                                {
                                                                                    if (dt4.Rows[d][5].ToString() == "14")
                                                                                    {
                                                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                                        marcacoes.Add(hora);
                                                                                        //.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                                        marcacoes.Add(hora);
                                                                                        //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");

                                                                                    }


                                                                                }
                                                                                else
                                                                                {
                                                                                    // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                                    marcacoes.Add(hora);
                                                                                    //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                                                }
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                            marcacoes.Add(hora);
                                                                            //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                                        }

                                                                    }


                                                                }
                                                                //else if (dt4.Rows[m][2].ToString() == "REINICIO DE VIAGEM")
                                                                //{
                                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;94");
                                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                                //}
                                                                //else if (dt4.Rows[m][2].ToString() == "PARADA INTERNA")
                                                                //{
                                                                //    if (dt4.Rows[m][5].ToString() == "3" || dt4.Rows[m][5].ToString() == "1" || dt4.Rows[m][5].ToString() == "2")
                                                                //    {
                                                                //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");

                                                                //    }
                                                                //    else if (dt4.Rows[m][5].ToString() == "22")
                                                                //    {
                                                                //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");
                                                                //    }

                                                                //    else if (dt4.Rows[m][2].ToString() == "FIM DE JORNADA")
                                                                //    {
                                                                //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                                //        write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                                //    }

                                                                //}
                                                                else if (dt4.Rows[m][2].ToString() == "PARADA")
                                                                {
                                                                    // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                    marcacoes.Add(hora);
                                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                                }
                                                                //else if (dt4.Rows[m][2].ToString() == "PARADA INTERNA")
                                                                //{
                                                                //    if (dt4.Rows[m][5].ToString() != "3" || dt4.Rows[m][5].ToString() != "1" || dt4.Rows[m][5].ToString() != "2")
                                                                //    {
                                                                //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");
                                                                //        write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                                //        write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");

                                                                //    }

                                                                //}

                                                                else
                                                                {
                                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                    marcacoes.Add(hora);
                                                                    //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                                }


                                                            }
                                                            else
                                                            {
                                                                //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                            }
                                                        }
                                                        else
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                            marcacoes.Add(hora);
                                                            //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                        }


                                                    }
                                                    else
                                                    {
                                                        // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                        marcacoes.Add(hora);
                                                        //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                    }
                                                }

                                            }
                                            else
                                            {
                                                if (dt4.Rows[w][2].ToString() == "FIM DE JORNADA")
                                                {
                                                    int q = 0;
                                                    q = w + 1;

                                                    if (q < dt4.Rows.Count)
                                                    {
                                                        if (dt4.Rows[q][2].ToString() == "FIM DE JORNADA")
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                            marcacoes.Add(hora);
                                                            //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");

                                                        }
                                                        else
                                                        {
                                                            int a = 0;
                                                            a = w - 1;

                                                            if (a >= 0)
                                                            {

                                                                if (dt4.Rows[a][2].ToString() == "FIM DE VIAGEM" || dt4.Rows[a][2].ToString() == "RETORNO REFEICAO" || dt4.Rows[a][2].ToString() == "INICIO DE JORNADA")
                                                                {
                                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                    marcacoes.Add(hora);
                                                                    // write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");

                                                                }
                                                                //else if (dt4.Rows[a][2].ToString() == "PARADA INTERNA")
                                                                //{
                                                                //    if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2")
                                                                //    {
                                                                //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                                //        // write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                                //        // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                                //    }
                                                                //    else if (dt4.Rows[a][5].ToString() == "14")
                                                                //    {
                                                                //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                                //        write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                                //    }
                                                                //    else
                                                                //    {
                                                                //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");


                                                                //    }
                                                                //}
                                                                else if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[a][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[a][2].ToString() == "INICIO JORNADA")
                                                                {

                                                                }
                                                                else if (dt4.Rows[a][2].ToString() == "FIM DE JORNADA")
                                                                {
                                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                    marcacoes.Add(hora);
                                                                    //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                                }
                                                                else
                                                                {
                                                                    // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                    marcacoes.Add(hora);
                                                                    // write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                                }
                                                            }
                                                        }

                                                    }
                                                    else
                                                    {
                                                        int a = 0;
                                                        a = w - 1;

                                                        if (a >= 0)
                                                        {

                                                            if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA")
                                                            {
                                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                marcacoes.Add(hora);

                                                            }
                                                            else if (dt4.Rows[a][2].ToString() == "FIM DE VIAGEM" || dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                                            {
                                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                marcacoes.Add(hora);
                                                                //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");

                                                            }
                                                            else if (dt4.Rows[a][2].ToString() == "RETORNO REFEICAO")
                                                            {
                                                                //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                marcacoes.Add(hora);

                                                            }
                                                            else if (dt4.Rows[a][2].ToString() == "PARADA INTERNA")
                                                            {
                                                                if (dt4.Rows[a][5].ToString() == "14")
                                                                {
                                                                    marcacoes.Add(hora);
                                                                }
                                                                else
                                                                {
                                                                    marcacoes.Add(hora);
                                                                }
                                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");

                                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                            }
                                                            //else if (dt4.Rows[a][2].ToString() == "PARADA INTERNA")
                                                            //{
                                                            //    if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2")
                                                            //    {
                                                            //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                            //        // write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                            //        // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                            //    }
                                                            //    else if (dt4.Rows[a][5].ToString() == "14")
                                                            //    {
                                                            //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                            //        write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                            //    }
                                                            //    else
                                                            //    {
                                                            //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");

                                                            //    }
                                                            //}
                                                            else if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[a][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[a][2].ToString() == "INICIO JORNADA")
                                                            {

                                                            }
                                                            else if (dt4.Rows[a][2].ToString() == "FIM DE JORNADA")
                                                            {
                                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                marcacoes.Add(hora);

                                                                //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                            }
                                                            else
                                                            {
                                                                // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                            }
                                                        }
                                                        else
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                            marcacoes.Add(hora);
                                                            //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                        }
                                                    }
                                                }





                                            }


                                            #endregion

                                            #region PARADA PERNOITE

                                            if (dt4.Rows[w][2].ToString() == "PARADA PERNOITE")
                                            {
                                                int a = 0;
                                                a = w - 1;


                                                if (a >= 0)
                                                {
                                                    if (dt4.Rows[a][2].ToString() == "PARADA CLIENTE / FORNECEDOR")
                                                    {


                                                        if (dt4.Rows[a][5].ToString() == "14")
                                                        {

                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0293");
                                                            marcacoes.Add(hora);
                                                            //WriteLine(cracha + ";" + data + ";" + hora + ";1;0102");
                                                            //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;0102");
                                                            marcacoes.Add(DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm").Replace(":", ""));



                                                        }
                                                        else if (dt4.Rows[a][5].ToString() == "15")
                                                        {

                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                            marcacoes.Add(hora);
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;22");


                                                        }
                                                        //else if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2")
                                                        //{
                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                        //    write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;22");
                                                        //    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                        //}
                                                        else
                                                        {
                                                            marcacoes.Add(hora);
                                                        }



                                                    }
                                                    else if (dt4.Rows[a][2].ToString() == "PARADA")
                                                    {
                                                        //if (dt4.Rows[a][5].ToString() == "7")
                                                        //{
                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                        //    write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;22");
                                                        //}
                                                        if (dt4.Rows[a][5].ToString() == "14")
                                                        {

                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0293");
                                                            marcacoes.Add(hora);
                                                            //WriteLine(cracha + ";" + data + ";" + hora + ";1;0102");
                                                            //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;0102");
                                                            marcacoes.Add(DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm").Replace(":", ""));


                                                        }
                                                        else
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                            marcacoes.Add(hora);
                                                            //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                        }

                                                    }
                                                    else if (dt4.Rows[a][2].ToString() == "INICIO DE VIAGEM")
                                                    {
                                                        marcacoes.Add(hora);
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                        //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                        //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";2;22");
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";3;22");
                                                    }
                                                    else if (dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                                    {
                                                        marcacoes.Add(hora);
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                        //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");

                                                        // write.WriteLine(cracha + ";" + data + ";" + hora + ";3;22");
                                                    }
                                                    else if (dt4.Rows[a][2].ToString() == "RETORNO REFEICAO")
                                                    {
                                                        marcacoes.Add(hora);
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                        //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");

                                                        // write.WriteLine(cracha + ";" + data + ";" + hora + ";3;22");
                                                    }
                                                    //else if (dt4.Rows[a][2].ToString() == "PARADA INTERNA")
                                                    //{


                                                    //    if (dt4.Rows[a][5].ToString() == "14")
                                                    //    {

                                                    //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                                    //        //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;22");


                                                    //    }
                                                    //    //else if (dt4.Rows[a][5].ToString() == "15")
                                                    //    //{

                                                    //    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                    //    //    write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;22");


                                                    //    //}
                                                    //    else if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2")
                                                    //    {
                                                    //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                    //        //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;22");
                                                    //        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                    //    }
                                                    //    //else
                                                    //    //{
                                                    //    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;22");
                                                    //    //}



                                                    //}
                                                    //else
                                                    //{
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;22");
                                                    //}

                                                }
                                                //else
                                                //{
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;22");
                                                //    write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;22");

                                                //}

                                            }

                                            #endregion

                                            #region RETORNO PERNOITE
                                            //BLOCO REINICIO / INICIO DE VIAGEM
                                            if (dt4.Rows[w][2].ToString() == "RETORNO PERNOITE")
                                            {
                                                int a = 0;
                                                a = w - 1;
                                                if (a >= 0)
                                                {
                                                    if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[a][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[a][2].ToString() == "INICIO JORNADA")
                                                    {

                                                    }
                                                    else if (dt4.Rows[a][2].ToString() == "PARADA PERNOITE")
                                                    {
                                                        //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01020000");
                                                        marcacoes.Add(hora);
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                    }


                                                }
                                                else
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0102");
                                                    marcacoes.Add(hora);
                                                    // write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                }



                                            }
                                            //FIM
                                            #endregion



                                        }

                                        //FIM

                                        #endregion

                                    }



                                }

                            }
                            else
                            {
                                //JORNADA DIFERENCIA DE COMEÇO

                                //VERIFICA SE A ULTIMA MARCAÇÃO DO DIA É PARADA PERNOITE OU FIM DE VIAGEM
                                string sqlz = "select TOP 1 ds_macro from tb_parada where cod_cracha=" + lblCod.Text + " and dt_posicao_parada='" + DateTime.Parse(dte.Rows[f][0].ToString()).ToString("yyyy-MM-dd") + "' and fl_deletado is null order by hr_posicao  DESC ";
                                SqlDataAdapter adtpz = new SqlDataAdapter(sqlz, con);
                                DataTable dtz = new DataTable();
                                con.Open();
                                adtpz.Fill(dtz);
                                con.Close();




                                if (dtz.Rows[0][0].ToString() == "FIM DE JORNADA" || dtz.Rows[0][0].ToString() == "PARADA PERNOITE")
                                {
                                    //JORNADA NORMAL COM DIFERENÇA NO COMEÇO
                                    string sqld = "select TOP 1  ds_macro, hr_posicao from tb_parada  where  cod_cracha=" + lblCod.Text + " and fl_deletado is null and dt_posicao_parada='" + DateTime.Parse(dte.Rows[f][0].ToString()).ToString("yyyy-MM-dd") + "'  and hr_posicao > (select top 1 hr_posicao from tb_parada where cod_cracha=" + lblCod.Text + " and fl_deletado is null and dt_posicao_parada='" + DateTime.Parse(dte.Rows[f][0].ToString()).ToString("yyyy-MM-dd") + "'  and ds_macro='FIM DE JORNADA'  OR cod_cracha=" + lblCod.Text + " and fl_deletado is null and dt_posicao_parada='" + DateTime.Parse(dte.Rows[f][0].ToString()).ToString("yyyy-MM-dd") + "'  and ds_macro='PARADA PERNOITE' order by hr_posicao) order by hr_posicao ";
                                    SqlDataAdapter adtpd = new SqlDataAdapter(sqld, con);
                                    DataTable dtd = new DataTable();
                                    con.Open();
                                    adtpd.Fill(dtd);
                                    con.Close();

                                    if (dtd.Rows.Count > 0)
                                    {
                                        dtRow["Dia"] = DateTime.Parse(dte.Rows[f][0].ToString()).ToString("dd");
                                        #region Jornada Normal
                                        //string arquivo = "Transmot" + DateTime.Parse(txtDtInicial.Text).ToString("ddMM") + "001.txt";

                                        sql4 = "select dt_posicao_parada, hr_posicao, ds_macro,cod_transmissao, cod_cracha, isnull(cod_ref_parada,0) as cod_ref_parada, cod_idveiculo from tb_parada where fl_deletado is null and cod_cracha=" + lblCod.Text + " and dt_posicao_parada='" + DateTime.Parse(dte.Rows[f][0].ToString()).ToString("yyyy-MM-dd") + "' and fl_deletado is null and hr_posicao >= '" + dtd.Rows[0][1].ToString() + "' AND (ds_macro IN ('INICIO DE JORNADA', 'INICIO JORNADA CAMINHAO', 'PARADA REFEICAO', 'FIM DE JORNADA','REINICIO DE VIAGEM','FIM DE VIAGEM', 'PARADA','PARADA INTERNA','INICIO DE VIAGEM','PARADA INTERNA','RETORNO REFEICAO','PARADA CLIENTE / FORNECEDOR','PARADA PERNOITE','RETORNO PERNOITE') OR cod_ref_parada in (2,3,4,14,22)) AND fl_deletado IS NULL ORDER BY dt_posicao_parada, hr_posicao";
                                        // string sql4 = "select dt_posicao_parada, hr_posicao, ds_macro,cod_transmissao, cod_cracha, isnull(cod_ref_parada,0) as cod_ref_parada, cod_idveiculo from tb_parada where fl_deletado is null and cod_cracha=" + lblCod.Text + " and dt_posicao_parada between '" + txtDtInicial.Text + "' and '" + txtDtFinal.Text + "' order by dt_posicao_parada, hr_posicao";
                                        SqlDataAdapter adtp4 = new SqlDataAdapter(sql4, con);
                                        DataTable dt4 = new DataTable();

                                        try
                                        {
                                            con.Open();
                                            adtp4.Fill(dt4);
                                            con.Close();
                                        }
                                        catch (Exception ed)
                                        {
                                            // MessageBox.Show(ed.ToString(), "Erro3");
                                        }

                                        c = 0;

                                        for (int y = 0; y < dt4.Rows.Count; y++)
                                        {
                                            if (dt4.Rows[y][2].ToString() == "FIM DE JORNADA" && dt4.Rows[y][6].ToString() == "725405" || dt4.Rows[y][2].ToString() == "FIM DE JORNADA" && dt4.Rows[y][6].ToString() == "725407" || dt4.Rows[y][2].ToString() == "FIM DE JORNADA" && dt4.Rows[y][6].ToString() == "184525")
                                            {
                                                c = c + 1;
                                            }

                                        }
                                        string caminho = Server.MapPath("/TXT/");

                                        string filepath = caminho + arquivo;

                                        FileInfo file = new FileInfo(filepath);

                                        //if (file.Exists)
                                        //{
                                        //    file.Delete();
                                        //}


                                        for (int w = 0; w < dt4.Rows.Count; w++)
                                        {
                                            //if (chkDiadema.Checked || chkCadiriri.Checked)
                                            //{


                                            //        if (lblCod.Text.Length < 4)
                                            //        {
                                            //            cracha = "300" + lblCod.Text;
                                            //        }
                                            //        else if (lblCod.Text.Length >= 4 || lblCod.Text.Length <= 5)
                                            //        {
                                            //            cracha = "30" + lblCod.Text;
                                            //        }
                                            //        else
                                            //        {
                                            //            cracha = lblCod.Text;
                                            //        }




                                            //}
                                            //else
                                            //{
                                            //    if (lblCod.Text.Length < 4)
                                            //    {
                                            //        cracha = "300" + lblCod.Text;
                                            //    }
                                            //    else if (lblCod.Text.Length >= 4 || lblCod.Text.Length <= 5)
                                            //    {
                                            //        cracha = "30" + lblCod.Text;
                                            //    }
                                            //    else
                                            //    {
                                            //        cracha = lblCod.Text;
                                            //    }
                                            //}
                                            string codigo = lblCod.Text;
                                            string[] codigosEspeciais = { "330", "482", "820", "958", "989" };

                                            if (codigo.StartsWith("312"))
                                            {
                                                cracha = codigo.Insert(3, "0");
                                            }
                                            else if ((codigosEspeciais.Contains(codigo) || codigo.Length <= 4))
                                            {
                                                cracha = "30" + codigo;
                                            }
                                            else if (codigo.Length <= 3)
                                            {
                                                cracha = "300" + codigo;
                                            }

                                            else
                                            {
                                                cracha = codigo;
                                            }





                                            //  cracha = "30" + lblCod.Text;
                                            int t = 0;
                                            t = w - 1;

                                            string data = DateTime.Parse(dt4.Rows[w][0].ToString()).ToString("dd/MM");
                                            string horas = DateTime.Parse(dt4.Rows[w][1].ToString()).ToString("HH:mm");
                                            if (t >= 0)
                                            {
                                                if (DateTime.Parse(dt4.Rows[w][1].ToString()).ToString("HH:mm") == DateTime.Parse(dt4.Rows[t][1].ToString()).ToString("HH:mm"))
                                                {
                                                    hora = DateTime.Parse(horas).AddMinutes(1).ToString("HH:mm");
                                                }
                                                else
                                                {
                                                    hora = DateTime.Parse(horas).ToString("HH:mm");
                                                }
                                            }
                                            else
                                            {
                                                hora = DateTime.Parse(horas).ToString("HH:mm");
                                            }
                                            //tipo = dt4.Rows[w][2].ToString();
                                            string transmissao = dt4.Rows[w][3].ToString();


                                            //if (dt4.Rows[w][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[w][2].ToString() == "TRECHO ALMOÇO" || dt4.Rows[w][2].ToString() == "FIM DE JORNADA")
                                            //{
                                            //    tipo = "2";
                                            //}
                                            //else  if (dt4.Rows[w][2].ToString() == "PARADA FORNECEDOR" || dt4.Rows[w][2].ToString() == "REINICIO DE VIAGEM")
                                            //{
                                            //    tipo = "21";
                                            //}









                                            #region BLOCO INICIO JORNADA
                                            //BLOCO INICIO JORNADA
                                            if (dt4.Rows[w][2].ToString() == "INICIO DE JORNADA")
                                            {
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0102");
                                                //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01020000");
                                                marcacoes.Add(hora);



                                            }

                                            if (dt4.Rows[w][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[w][2].ToString() == "INICIO JORNADA")
                                            {

                                                int a = 0;
                                                a = w - 1;
                                                if (a >= 0)
                                                {
                                                    if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA")
                                                    {
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");

                                                    }
                                                    //else
                                                    //{
                                                    //	//write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0102");
                                                    //	//write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01020000");
                                                    //	marcacoes.Add(hora);



                                                    //}
                                                }
                                                else
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                }


                                            }
                                            //FIM
                                            #endregion

                                            #region BLOCO INICIO JORNADA CAMINHAO
                                            //BLOCO INICIO JORNADA CAMINHAO

                                            if (dt4.Rows[w][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[w][2].ToString() == "INICIO JORNADA")
                                            {

                                                int a = 0;
                                                a = w - 1;
                                                if (a >= 0)
                                                {
                                                    if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA")
                                                    {
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");

                                                    }
                                                    else
                                                    {
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0102");
                                                        //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01020000");
                                                        marcacoes.Add(hora);
                                                    }
                                                }
                                                else
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                }


                                            }
                                            //FIM
                                            #endregion

                                            #region PARADA INTERNA
                                            //BLOCO PARADA INTERNA
                                            if (dt4.Rows[w][2].ToString() == "PARADA INTERNA")
                                            {
                                                if (dt4.Rows[w][5].ToString() == "3" || dt4.Rows[w][5].ToString() == "2")
                                                {
                                                    int a = 0;
                                                    a = w - 1;
                                                    if (a >= 0)
                                                    {
                                                        if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[a][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[a][2].ToString() == "INICIO JORNADA")
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");
                                                        }
                                                        else if (dt4.Rows[a][5].ToString() == "14")
                                                        {
                                                            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");
                                                            marcacoes.Add(hora);
                                                        }
                                                        else if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2" || dt4.Rows[a][5].ToString() == "1")
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                        }
                                                        else if (dt4.Rows[a][2].ToString() == "PARADA REFEICAO")
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");
                                                        }
                                                        else if (dt4.Rows[a][2].ToString() == "INICIO DE VIAGEM")
                                                        {
                                                            //////write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                            ////write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                            ////write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");
                                                        }
                                                        else if (dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;94");
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");
                                                        }
                                                        else if (dt4.Rows[a][2].ToString() == "FIM DE VIAGEM")
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                        }
                                                        else
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");
                                                        }

                                                    }

                                                }
                                                else if (dt4.Rows[w][5].ToString() == "14")
                                                {
                                                    int a = 0;
                                                    a = w - 1;
                                                    if (a >= 0)
                                                    {
                                                        if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[a][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[a][2].ToString() == "INICIO JORNADA")
                                                        {
                                                            marcacoes.Add(hora);
                                                        }
                                                        if (dt4.Rows[a][5].ToString() == "14")
                                                        {
                                                            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                            marcacoes.Add(hora);
                                                        }
                                                        else if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2" || dt4.Rows[a][5].ToString() == "1")
                                                        {
                                                            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                            marcacoes.Add(hora);
                                                        }
                                                        else if (dt4.Rows[a][2].ToString() == "PARADA REFEICAO")
                                                        {
                                                            // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");
                                                        }
                                                        else if (dt4.Rows[a][2].ToString() == "INICIO DE VIAGEM" || dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                                        {
                                                            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                            marcacoes.Add(hora);
                                                        }
                                                        else if (dt4.Rows[a][2].ToString() == "FIM DE VIAGEM")
                                                        {

                                                            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                            marcacoes.Add(hora);

                                                        }
                                                        if (dt4.Rows[a][5].ToString() == "22")
                                                        {
                                                            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                            marcacoes.Add(hora);
                                                        }
                                                        //else
                                                        //{
                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93: 71489");
                                                        //    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                                        //}

                                                    }
                                                }
                                                else if (dt4.Rows[w][5].ToString() == "22")
                                                {
                                                    int a = 0;
                                                    a = w - 1;
                                                    if (a >= 0)
                                                    {

                                                        if (dt4.Rows[a][5].ToString() == "14")
                                                        {
                                                            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");
                                                            marcacoes.Add(hora);
                                                        }


                                                    }
                                                }
                                                else
                                                {
                                                    int a = 0;
                                                    a = w - 1;
                                                    if (a >= 0)
                                                    {
                                                        if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[a][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[a][2].ToString() == "INICIO JORNADA")
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;95");
                                                        }
                                                        if (dt4.Rows[a][5].ToString() == "14")
                                                        {
                                                            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");
                                                            marcacoes.Add(hora);

                                                        }

                                                        else if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2" || dt4.Rows[a][5].ToString() == "1")
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;95");
                                                        }
                                                        else if (dt4.Rows[a][2].ToString() == "PARADA REFEICAO")
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                                            //// write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");
                                                        }
                                                        else if (dt4.Rows[a][2].ToString() == "INICIO DE VIAGEM" || dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                                        {

                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;94");
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;95");
                                                        }
                                                        else if (dt4.Rows[a][2].ToString() == "FIM DE VIAGEM" || dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                                        {

                                                            //// write.WriteLine(cracha + ";" + data + ";" + hora + ";1;94");
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;95");

                                                        }

                                                    }
                                                }

                                            }
                                            //FIM 
                                            #endregion

                                            #region BLOCO PARADA REFEIÇÃO
                                            //BLOCO PARADA REFEIÇÃO
                                            if (dt4.Rows[w][2].ToString() == "PARADA REFEICAO")
                                            {
                                                int a = 0;
                                                a = w - 1;
                                                if (a >= 0)
                                                {
                                                    //if (dt4.Rows[a][2].ToString() == "PARADA INTERNA")
                                                    //{

                                                    //    if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2")
                                                    //    {
                                                    //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                    //        write.WriteLine(cracha + ";" + data + ";" + hora + ";2;93");
                                                    //    }
                                                    //    else
                                                    //    {
                                                    //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");
                                                    //        write.WriteLine(cracha + ";" + data + ";" + hora + ";2;93");
                                                    //    }
                                                    //}
                                                    if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[a][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[a][2].ToString() == "INICIO JORNADA")
                                                    {
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0193");
                                                        //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                        marcacoes.Add(hora);

                                                    }
                                                    else
                                                    {
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0193");
                                                        //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                        marcacoes.Add(hora);
                                                    }

                                                }
                                                //else
                                                //{
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;93");
                                                //}
                                            }
                                            //FIM
                                            #endregion

                                            #region RETORNO REFEIÇAO
                                            //BLOCO RETORNO REFEIÇAO
                                            if (dt4.Rows[w][2].ToString() == "RETORNO REFEICAO")
                                            {

                                                int a = 0;
                                                int b = 0;
                                                a = w - 1;
                                                b = w + 1;
                                                if (a >= 0)
                                                {

                                                    if (dt4.Rows[a][2].ToString() == "PARADA REFEICAO")
                                                    {

                                                        // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0293");
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;0102");
                                                        //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");
                                                        marcacoes.Add(hora);

                                                    }
                                                    //if (dt4.Rows[b][2].ToString() == "FIM DE JORNADA")
                                                    //{
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                    //}
                                                    //else
                                                    //{
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                                    //    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");

                                                    //}
                                                }


                                            }
                                            //FIM
                                            #endregion

                                            #region INICIO DE VIAGEM
                                            //BLOCO REINICIO / INICIO DE VIAGEM
                                            if (dt4.Rows[w][2].ToString() == "INICIO DE VIAGEM")
                                            {
                                                int a = 0;
                                                a = w - 1;
                                                if (a >= 0)
                                                {
                                                    //if (dt4.Rows[a][2].ToString() == "PARADA INTERNA")
                                                    //{
                                                    //    if (dt4.Rows[a][5].ToString() == "14")
                                                    //    {
                                                    //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                                    //        write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");

                                                    //    }
                                                    //    else
                                                    //        if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2")
                                                    //    {
                                                    //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                    //        write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");

                                                    //    }
                                                    //    else
                                                    //    {
                                                    //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");
                                                    //        write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                    //    }
                                                    //}
                                                    //if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[a][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[a][2].ToString() == "INICIO JORNADA")
                                                    //{
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                    //}
                                                    //else if (dt4.Rows[a][2].ToString() == "PARADA PERNOITE")
                                                    //{
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;22");
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                    //}
                                                    //else if (dt4.Rows[a][2].ToString() == "FIM DE VIAGEM")
                                                    //{
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;94");
                                                    //    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                    //}

                                                    if (dt4.Rows[a][2].ToString() == "PARADA CLIENTE / FORNECEDOR")
                                                    {


                                                        if (dt4.Rows[a][5].ToString() == "14")
                                                        {

                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0193");
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                            marcacoes.Add(hora);



                                                        }
                                                        //else if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "1" || dt4.Rows[a][5].ToString() == "2" || dt4.Rows[a][5].ToString() == "4")
                                                        //{
                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                        //}



                                                    }
                                                    else if (dt4.Rows[a][2].ToString() == "PARADA")
                                                    {


                                                        if (dt4.Rows[a][5].ToString() == "14")
                                                        {

                                                            // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0193");
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                            marcacoes.Add(hora);
                                                        }
                                                        //else
                                                        //{
                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");
                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");



                                                        //}



                                                    }
                                                    //else if (dt4.Rows[a][2].ToString() == "RETORNO REFEICAO")
                                                    //{
                                                    //    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0293");
                                                    //    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                    //    write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");

                                                    //}

                                                }
                                                //else
                                                //{
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                //}



                                            }
                                            //FIM
                                            #endregion

                                            #region PARADA CLIENTE / FORNECEDOR
                                            if (dt4.Rows[w][2].ToString() == "PARADA CLIENTE / FORNECEDOR")
                                            {
                                                if (dt4.Rows[w][5].ToString() == "3" || dt4.Rows[w][5].ToString() == "2" || dt4.Rows[w][5].ToString() == "4")
                                                {
                                                    int a = 0;
                                                    a = w - 1;
                                                    if (a >= 0)
                                                    {
                                                        //if (dt4.Rows[a][2].ToString() == "INICIO DE VIAGEM" || dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM" || dt4.Rows[a][2].ToString() == "FIM DE VIAGEM")
                                                        //{
                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;94");
                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");
                                                        //}
                                                        if (dt4.Rows[a][2].ToString() == "PARADA CLIENTE / FORNECEDOR")
                                                        {
                                                            if (dt4.Rows[a][5].ToString() == "14")
                                                            {
                                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0293");
                                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;0102");
                                                                //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");
                                                                marcacoes.Add(hora);


                                                            }
                                                            //else if (dt4.Rows[a][5].ToString() == "15")
                                                            //{
                                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;94");
                                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");

                                                            //}
                                                            //else
                                                            //{
                                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");
                                                            //}

                                                        }
                                                    }

                                                }
                                                else if (dt4.Rows[w][5].ToString() == "14")
                                                {
                                                    int a = 0;
                                                    a = w - 1;
                                                    if (a >= 0)
                                                    {
                                                        if (dt4.Rows[a][2].ToString() == "INICIO DE VIAGEM" || dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM" || dt4.Rows[a][2].ToString() == "FIM DE VIAGEM")
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                            // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0193");
                                                            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                            marcacoes.Add(hora);
                                                        }
                                                        else if (dt4.Rows[a][2].ToString() == "PARADA CLIENTE / FORNECEDOR")
                                                        {
                                                            if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2" || dt4.Rows[a][5].ToString() == "4")
                                                            {
                                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0193");
                                                                //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                                marcacoes.Add(hora);
                                                            }
                                                            else if (dt4.Rows[a][5].ToString() == "14")
                                                            {
                                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0193");
                                                                marcacoes.Add(hora);

                                                            }


                                                        }
                                                        else
                                                        {
                                                            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                            marcacoes.Add(hora);
                                                        }

                                                    }


                                                }


                                            }
                                            #endregion

                                            #region REINICIO
                                        //BLOCO REINICIO / INICIO DE VIAGEM
                                        if (dt4.Rows[w][2].ToString() == "REINICIO DE VIAGEM" || dt4.Rows[w][2].ToString() == "RETORNO PERNOITE")
                                        {
                                            int a = 0;
                                            a = w - 1;
                                            if (a >= 0)
                                            {
                                                if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[a][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[a][2].ToString() == "INICIO JORNADA")
                                                {

                                                }
                                                else if (dt4.Rows[a][2].ToString() == "PARADA PERNOITE")
                                                {
                                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01020000");
                                                    marcacoes.Add(hora);
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                }

                                                else if (dt4.Rows[a][2].ToString() == "PARADA CLIENTE / FORNECEDOR")
                                                {

                                                    if (a >= 0)
                                                    {

                                                        if (dt4.Rows[a][2].ToString() == "PARADA CLIENTE / FORNECEDOR")
                                                        {
                                                            //if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2")
                                                            //{
                                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                            //}
                                                            if (dt4.Rows[a][5].ToString() == "14")
                                                            {

                                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0293");
                                                                marcacoes.Add(hora);
                                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0102");

                                                            }
                                                            //else if (dt4.Rows[a][5].ToString() == "15")
                                                            //{

                                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");

                                                            //}
                                                            //else
                                                            //{
                                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                            //}
                                                        }
                                                        //else if (dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                                        //{
                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                        //}


                                                    }



                                                }
                                                else if (dt4.Rows[a][2].ToString() == "PARADA")
                                                {
                                                    if (dt4.Rows[a][5].ToString() == "14")
                                                    {
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0293");
                                                        //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0102");
                                                        marcacoes.Add(hora);
                                                    }
                                                    //else
                                                    //{
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                    //}




                                                }
                                                //else if (dt4.Rows[a][2].ToString() == "OFICINA / MANUTENCAO")
                                                //{

                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");


                                                //}

                                            }
                                            else
                                            {
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0102");
                                                marcacoes.Add(hora);
                                                // write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                            }



                                        }
                                        //FIM
                                        #endregion

                                            #region BLOCO PARADA
                                            // BLOCO PARADA
                                            if (dt4.Rows[w][2].ToString() == "PARADA")
                                            {
                                                int a = 0;
                                                a = w - 1;


                                                if (dt4.Rows[w][5].ToString() == "14")
                                                {
                                                    if (dt4.Rows.Count >= a)
                                                    {
                                                        if (dt4.Rows[a][2].ToString() == "INICIO DE VIAGEM" || dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                                        {
                                                            // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0193");
                                                            marcacoes.Add(hora);


                                                        }

                                                    }
                                                }
                                                //else
                                                //{
                                                //    if (dt4.Rows.Count >= a)
                                                //    {
                                                //        if (dt4.Rows[a][2].ToString() == "INICIO DE VIAGEM" || dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                                //        {
                                                //            write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                //            //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;95");

                                                //        }
                                                //        //else
                                                //        //{
                                                //        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                //        //    // write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                //        //}
                                                //    }
                                                //}



                                                //  write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");

                                            }
                                            //FIM
                                            #endregion

                                            #region FIM DE VIAGEM
                                            //BLOCO FIM DE VIAGEM
                                            if (dt4.Rows[w][2].ToString() == "FIM DE VIAGEM")
                                            {
                                                int a = 0;
                                                int g = 0;
                                                a = w - 1;
                                                g = w - 2;
                                                if (a >= 0)
                                                {
                                                    if (dt4.Rows[a][2].ToString() == "PARADA CLIENTE / FORNECEDOR")
                                                    {


                                                        if (dt4.Rows[a][5].ToString() == "14")
                                                        {

                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0293");
                                                            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;0102");
                                                            marcacoes.Add(hora);

                                                        }

                                                        //else if (dt4.Rows[a][5].ToString() == "14")
                                                        //{
                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                                        //    // write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                        //}

                                                        //else
                                                        //{
                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                        //    // write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                        //}



                                                    }
                                                    else if (dt4.Rows[a][2].ToString() == "PARADA")
                                                    {


                                                        if (dt4.Rows[a][5].ToString() == "14")
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0293");
                                                            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");
                                                            marcacoes.Add(hora);
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;0102");

                                                        }

                                                        //else
                                                        //{
                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                        //    // write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");

                                                        //}


                                                    }

                                                    else if (dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                                    {
                                                        if (g >= 0)
                                                        {
                                                            //if (dt4.Rows[g][2].ToString() == "PARADA PERNOITE")
                                                            //{
                                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                            //    write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                            //}
                                                            //else
                                                            //{
                                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                            //}
                                                        }
                                                        //else
                                                        //{
                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                        //}


                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                    }


                                                }
                                                //else
                                                //{
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                //}

                                            }

                                            //
                                            #endregion

                                            #region FIM DE JORNADA
                                            //BLOCO FIM DE JORMADA
                                            if (c > 0)
                                            {
                                                if (dt4.Rows[w][2].ToString() == "FIM DE JORNADA" && dt4.Rows[w][6].ToString() == "725405" || dt4.Rows[w][2].ToString() == "FIM DE JORNADA" && dt4.Rows[w][6].ToString() == "725407" || dt4.Rows[w][2].ToString() == "FIM DE JORNADA" && dt4.Rows[w][6].ToString() == "184525" && dt4.Rows[w][6].ToString() == "184522")
                                                {
                                                    int a = 0;
                                                    a = w - 1;

                                                    if (a >= 0)
                                                    {
                                                        if (dt4.Rows[a][2].ToString() == "RETORNO REFEICAO")
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02020000");
                                                            //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                            marcacoes.Add(hora);

                                                        }
                                                        else if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA")
                                                        {

                                                            //WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02020000");
                                                            marcacoes.Add(hora);


                                                        }
                                                        else if (dt4.Rows[a][2].ToString() == "RETORNO REFEICAO")
                                                        {

                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02020000");
                                                            marcacoes.Add(hora);

                                                        }
                                                        else if (dt4.Rows[a][2].ToString() == "FIM DE VIAGEM")
                                                        {
                                                            int b = 0;
                                                            b = a - 1;

                                                            if (b >= 0)
                                                            {
                                                                if (dt4.Rows[b][2].ToString() == "INICIO DE VIAGEM" || dt4.Rows[b][2].ToString() == "REINICIO DE VIAGEM")
                                                                {
                                                                    int d = 0;
                                                                    d = b - 1;
                                                                    if (d >= 0)
                                                                    {

                                                                        if (dt4.Rows[d][2].ToString() == "PARADA")
                                                                        {
                                                                            if (dt4.Rows[d][5].ToString() == "14")
                                                                            {
                                                                                // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                                //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02020000");
                                                                                //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                                                marcacoes.Add(hora);
                                                                            }
                                                                            else
                                                                            {
                                                                                // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                                //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02020000");
                                                                                // write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                                                marcacoes.Add(hora);
                                                                            }




                                                                        }
                                                                        else
                                                                        {
                                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02020000");
                                                                            // write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                                            marcacoes.Add(hora);
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02020000");
                                                                    // write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                                    marcacoes.Add(hora);
                                                                }

                                                            }
                                                            else
                                                            {
                                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                marcacoes.Add(hora);
                                                                //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");

                                                            }

                                                        }
                                                        else if (dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02020000");
                                                            //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                            marcacoes.Add(hora);
                                                        }
                                                        else if (dt4.Rows[a][2].ToString() == "PARADA")
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                            marcacoes.Add(hora);
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                        }
                                                        else if (dt4.Rows[a][2].ToString() == "PARADA INTERNA")
                                                        {
                                                            if (dt4.Rows[a][5].ToString() == "14")
                                                            {
                                                                //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");
                                                                marcacoes.Add(hora);
                                                            }
                                                            else
                                                            {
                                                                marcacoes.Add(hora);
                                                            }
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");

                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                        }
                                                        else if (dt4.Rows[a][2].ToString() == "FIM DE JORNADA")
                                                        {
                                                            int m = 0;
                                                            m = a - 1;

                                                            if (m >= 0)
                                                            {
                                                                if (dt4.Rows[m][2].ToString() == "RETORNO REFEICAO" || dt4.Rows[m][2].ToString() == "INICIO DE JORNADA")
                                                                {
                                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                    marcacoes.Add(hora);
                                                                }
                                                                else if (dt4.Rows[m][2].ToString() == "FIM DE VIAGEM")
                                                                {
                                                                    int b = 0;
                                                                    b = m - 1;

                                                                    if (b >= 0)
                                                                    {
                                                                        if (dt4.Rows[b][2].ToString() == "INICIO DE VIAGEM" || dt4.Rows[b][2].ToString() == "REINICIO DE VIAGEM")
                                                                        {
                                                                            int d = 0;
                                                                            d = b - 1;
                                                                            if (d >= 0)
                                                                            {

                                                                                if (dt4.Rows[d][2].ToString() == "PARADA")
                                                                                {
                                                                                    if (dt4.Rows[d][5].ToString() == "14")
                                                                                    {
                                                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                                        marcacoes.Add(hora);
                                                                                        //.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                                        marcacoes.Add(hora);
                                                                                        //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");

                                                                                    }


                                                                                }
                                                                                else
                                                                                {
                                                                                    // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                                    marcacoes.Add(hora);
                                                                                    //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                                                }
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                            marcacoes.Add(hora);
                                                                            //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                                        }

                                                                    }


                                                                }
                                                                //else if (dt4.Rows[m][2].ToString() == "REINICIO DE VIAGEM")
                                                                //{
                                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;94");
                                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                                //}
                                                                //else if (dt4.Rows[m][2].ToString() == "PARADA INTERNA")
                                                                //{
                                                                //    if (dt4.Rows[m][5].ToString() == "3" || dt4.Rows[m][5].ToString() == "1" || dt4.Rows[m][5].ToString() == "2")
                                                                //    {
                                                                //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");

                                                                //    }
                                                                //    else if (dt4.Rows[m][5].ToString() == "22")
                                                                //    {
                                                                //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");
                                                                //    }

                                                                //    else if (dt4.Rows[m][2].ToString() == "FIM DE JORNADA")
                                                                //    {
                                                                //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                                //        write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                                //    }

                                                                //}
                                                                else if (dt4.Rows[m][2].ToString() == "PARADA")
                                                                {
                                                                    // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                    marcacoes.Add(hora);
                                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                                }
                                                                //else if (dt4.Rows[m][2].ToString() == "PARADA INTERNA")
                                                                //{
                                                                //    if (dt4.Rows[m][5].ToString() != "3" || dt4.Rows[m][5].ToString() != "1" || dt4.Rows[m][5].ToString() != "2")
                                                                //    {
                                                                //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");
                                                                //        write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                                //        write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");

                                                                //    }

                                                                //}

                                                                else
                                                                {
                                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                    marcacoes.Add(hora);
                                                                    //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                                }


                                                            }
                                                            else
                                                            {
                                                                //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                            }
                                                        }
                                                        else
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                            marcacoes.Add(hora);
                                                            //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                        }


                                                    }
                                                    else
                                                    {
                                                        // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                        marcacoes.Add(hora);
                                                        //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                    }
                                                }

                                            }
                                            else
                                            {
                                                if (dt4.Rows[w][2].ToString() == "FIM DE JORNADA")
                                                {
                                                    int q = 0;
                                                    q = w + 1;

                                                    if (q < dt4.Rows.Count)
                                                    {
                                                        if (dt4.Rows[q][2].ToString() == "FIM DE JORNADA")
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                            marcacoes.Add(hora);
                                                            //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");

                                                        }
                                                        else
                                                        {
                                                            int a = 0;
                                                            a = w - 1;

                                                            if (a >= 0)
                                                            {

                                                                if (dt4.Rows[a][2].ToString() == "FIM DE VIAGEM" || dt4.Rows[a][2].ToString() == "RETORNO REFEICAO" || dt4.Rows[a][2].ToString() == "INICIO DE JORNADA")
                                                                {
                                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                    marcacoes.Add(hora);
                                                                    // write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");

                                                                }
                                                                //else if (dt4.Rows[a][2].ToString() == "PARADA INTERNA")
                                                                //{
                                                                //    if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2")
                                                                //    {
                                                                //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                                //        // write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                                //        // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                                //    }
                                                                //    else if (dt4.Rows[a][5].ToString() == "14")
                                                                //    {
                                                                //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                                //        write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                                //    }
                                                                //    else
                                                                //    {
                                                                //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");


                                                                //    }
                                                                //}
                                                                else if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[a][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[a][2].ToString() == "INICIO JORNADA")
                                                                {

                                                                }
                                                                else if (dt4.Rows[a][2].ToString() == "FIM DE JORNADA")
                                                                {
                                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                    marcacoes.Add(hora);
                                                                    //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                                }
                                                                else
                                                                {
                                                                    // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                    marcacoes.Add(hora);
                                                                    // write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                                }
                                                            }
                                                        }

                                                    }
                                                    else
                                                    {
                                                        int a = 0;
                                                        a = w - 1;

                                                        if (a >= 0)
                                                        {

                                                            if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA")
                                                            {
                                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                marcacoes.Add(hora);

                                                            }
                                                            else if (dt4.Rows[a][2].ToString() == "FIM DE VIAGEM" || dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                                            {
                                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                marcacoes.Add(hora);
                                                                //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");

                                                            }
                                                            else if (dt4.Rows[a][2].ToString() == "RETORNO REFEICAO")
                                                            {
                                                                //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                marcacoes.Add(hora);

                                                            }
                                                            else if (dt4.Rows[a][2].ToString() == "PARADA INTERNA")
                                                            {
                                                                if (dt4.Rows[a][5].ToString() == "14")
                                                                {
                                                                    marcacoes.Add(hora);
                                                                }
                                                                else
                                                                {
                                                                    marcacoes.Add(hora);
                                                                }
                                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");

                                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                            }
                                                            //else if (dt4.Rows[a][2].ToString() == "PARADA INTERNA")
                                                            //{
                                                            //    if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2")
                                                            //    {
                                                            //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                            //        // write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                            //        // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                            //    }
                                                            //    else if (dt4.Rows[a][5].ToString() == "14")
                                                            //    {
                                                            //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                            //        write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                            //    }
                                                            //    else
                                                            //    {
                                                            //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");

                                                            //    }
                                                            //}
                                                            else if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[a][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[a][2].ToString() == "INICIO JORNADA")
                                                            {

                                                            }
                                                            else if (dt4.Rows[a][2].ToString() == "FIM DE JORNADA")
                                                            {
                                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                marcacoes.Add(hora);

                                                                //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                            }
                                                            else
                                                            {
                                                                // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                            }
                                                        }
                                                        else
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                            marcacoes.Add(hora);
                                                            //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                        }
                                                    }
                                                }





                                            }


                                            #endregion

                                            #region PARADA PERNOITE

                                            if (dt4.Rows[w][2].ToString() == "PARADA PERNOITE")
                                            {
                                                int a = 0;
                                                a = w - 1;


                                                if (a >= 0)
                                                {
                                                    if (dt4.Rows[a][2].ToString() == "PARADA CLIENTE / FORNECEDOR")
                                                    {


                                                        if (dt4.Rows[a][5].ToString() == "14")
                                                        {

                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0293");
                                                            marcacoes.Add(hora);
                                                            //WriteLine(cracha + ";" + data + ";" + hora + ";1;0102");
                                                            //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;0102");
                                                            marcacoes.Add(DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm").Replace(":", ""));



                                                        }
                                                        else if (dt4.Rows[a][5].ToString() == "15")
                                                        {

                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                            marcacoes.Add(hora);
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;22");


                                                        }
                                                        //else if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2")
                                                        //{
                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                        //    write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;22");
                                                        //    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                        //}
                                                        else
                                                        {
                                                            marcacoes.Add(hora);
                                                        }



                                                    }
                                                    else if (dt4.Rows[a][2].ToString() == "PARADA")
                                                    {
                                                        //if (dt4.Rows[a][5].ToString() == "7")
                                                        //{
                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                        //    write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;22");
                                                        //}
                                                        if (dt4.Rows[a][5].ToString() == "14")
                                                        {

                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0293");
                                                            marcacoes.Add(hora);
                                                            //WriteLine(cracha + ";" + data + ";" + hora + ";1;0102");
                                                            //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;0102");
                                                            marcacoes.Add(DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm").Replace(":", ""));


                                                        }
                                                        else
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                            marcacoes.Add(hora);
                                                            //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                        }

                                                    }
                                                    else if (dt4.Rows[a][2].ToString() == "INICIO DE VIAGEM")
                                                    {
                                                        marcacoes.Add(hora);
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                        //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                        //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";2;22");
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";3;22");
                                                    }
                                                    else if (dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                                    {
                                                        marcacoes.Add(hora);
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                        //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");

                                                        // write.WriteLine(cracha + ";" + data + ";" + hora + ";3;22");
                                                    }
                                                    else if (dt4.Rows[a][2].ToString() == "RETORNO REFEICAO")
                                                    {
                                                        marcacoes.Add(hora);
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                        //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");

                                                        // write.WriteLine(cracha + ";" + data + ";" + hora + ";3;22");
                                                    }
                                                    //else if (dt4.Rows[a][2].ToString() == "PARADA INTERNA")
                                                    //{


                                                    //    if (dt4.Rows[a][5].ToString() == "14")
                                                    //    {

                                                    //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                                    //        //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;22");


                                                    //    }
                                                    //    //else if (dt4.Rows[a][5].ToString() == "15")
                                                    //    //{

                                                    //    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                    //    //    write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;22");


                                                    //    //}
                                                    //    else if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2")
                                                    //    {
                                                    //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                    //        //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;22");
                                                    //        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                    //    }
                                                    //    //else
                                                    //    //{
                                                    //    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;22");
                                                    //    //}



                                                    //}
                                                    //else
                                                    //{
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;22");
                                                    //}

                                                }
                                                //else
                                                //{
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;22");
                                                //    write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;22");

                                                //}

                                            }

                                            #endregion

                                            #region RETORNO PERNOITE
                                            //BLOCO REINICIO / INICIO DE VIAGEM
                                            if (dt4.Rows[w][2].ToString() == "RETORNO PERNOITE")
                                            {
                                                int a = 0;
                                                a = w - 1;
                                                if (a >= 0)
                                                {
                                                    if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[a][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[a][2].ToString() == "INICIO JORNADA")
                                                    {

                                                    }
                                                    else if (dt4.Rows[a][2].ToString() == "PARADA PERNOITE")
                                                    {
                                                        //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01020000");
                                                        marcacoes.Add(hora);
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                    }


                                                }
                                                else
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0102");
                                                    marcacoes.Add(hora);
                                                    // write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                }



                                            }
                                            //FIM
                                            #endregion


                                        }

                                        //FIM

                                        #endregion
                                    }
                                    else
                                    {

                                    }

                                }
                                else
                                {
                                    //JORNADA PERNOITE COM DIFERENÇA NO COMEÇO
                                    string sqli = "select TOP 1  hr_posicao from tb_parada  where  cod_cracha=" + lblCod.Text + " and fl_deletado is null and dt_posicao_parada='" + DateTime.Parse(dte.Rows[f][0].ToString()).ToString("yyyy-MM-dd") + "'  and hr_posicao > (select top 1 hr_posicao from tb_parada where cod_cracha=" + lblCod.Text + " and fl_deletado is null and dt_posicao_parada='" + DateTime.Parse(dte.Rows[f][0].ToString()).ToString("yyyy-MM-dd") + "'  and ds_macro='FIM DE JORNADA'  OR cod_cracha=" + lblCod.Text + " and fl_deletado is null and dt_posicao_parada='" + DateTime.Parse(dte.Rows[f][0].ToString()).ToString("yyyy-MM-dd") + "'  and ds_macro='PARADA PERNOITE' order by hr_posicao) order by hr_posicao ";
                                    SqlDataAdapter adtpi = new SqlDataAdapter(sqli, con);
                                    DataTable dti = new DataTable();
                                    con.Open();
                                    adtpi.Fill(dti);
                                    con.Close();

                                    string sqlf = "select top 1 hr_posicao from tb_parada where cod_cracha=" + lblCod.Text + " and ds_macro = 'FIM DE JORNADA' and dt_posicao_parada='" + DateTime.Parse(dte.Rows[f][0].ToString()).AddDays(1).ToString("yyyy-MM-dd") + "' and fl_deletado is null OR cod_cracha=" + lblCod.Text + " and ds_macro='PARADA PERNOITE' and dt_posicao_parada='" + DateTime.Parse(dte.Rows[f][0].ToString()).AddDays(1).ToString("yyyy-MM-dd") + "' and fl_deletado is null order by hr_posicao";
                                    SqlDataAdapter adtpf = new SqlDataAdapter(sqlf, con);
                                    DataTable dtf = new DataTable();
                                    con.Open();
                                    adtpf.Fill(dtf);
                                    con.Close();



                                    if (dti.Rows.Count > 0 && dtf.Rows.Count > 0)
                                    {
                                        dtRow["Dia"] = DateTime.Parse(dte.Rows[f][0].ToString()).ToString("dd");
                                        #region Jornada Noturna


                                        //sql4 = "select dt_posicao_parada, hr_posicao, ds_macro,cod_transmissao, cod_cracha, isnull(cod_ref_parada,0) as cod_ref_parada, cod_idveiculo from tb_parada where fl_deletado is null and cod_cracha=" + lblCod.Text + " and dt_posicao_parada='" + txtDtInicial.Text + "' order by dt_posicao_parada, hr_posicao";
                                        sql4 = "select dt_posicao_parada, hr_posicao, ds_macro,cod_transmissao, cod_cracha, isnull(cod_ref_parada,0) as cod_ref_parada, cod_idveiculo from tb_parada where fl_deletado is null and cod_cracha=" + lblCod.Text + " and dt_posicao_parada='" + DateTime.Parse(dte.Rows[f][0].ToString()).ToString("yyyy-MM-dd") + "' and hr_posicao >='" + dti.Rows[0][0].ToString() + "' or cod_cracha=" + lblCod.Text + " and dt_posicao_parada='" + DateTime.Parse(dte.Rows[f][0].ToString()).AddDays(1).ToString("yyyy-MM-dd") + "' and hr_posicao <= '" + dtf.Rows[0][0].ToString() + "' AND (ds_macro IN ('INICIO DE JORNADA', 'INICIO JORNADA CAMINHAO', 'PARADA REFEICAO', 'FIM DE JORNADA','REINICIO DE VIAGEM','FIM DE VIAGEM', 'PARADA','INICIO DE VIAGEM','PARADA INTERNA','RETORNO REFEICAO','PARADA CLIENTE / FORNECEDOR','PARADA PERNOITE','RETORNO PERNOITE') OR cod_ref_parada in (2,3,4,14,22)) AND fl_deletado IS NULL ORDER BY dt_posicao_parada, hr_posicao";
                                        // string sql4 = "select dt_posicao_parada, hr_posicao, ds_macro,cod_transmissao, cod_cracha, isnull(cod_ref_parada,0) as cod_ref_parada, cod_idveiculo from tb_parada where fl_deletado is null and cod_cracha=" + lblCod.Text + " and dt_posicao_parada between '" + txtDtInicial.Text + "' and '" + txtDtFinal.Text + "' order by dt_posicao_parada, hr_posicao";
                                        SqlDataAdapter adtp4 = new SqlDataAdapter(sql4, con);
                                        DataTable dt4 = new DataTable();

                                        try
                                        {
                                            con.Open();
                                            adtp4.Fill(dt4);
                                            con.Close();
                                        }
                                        catch (Exception ed)
                                        {
                                            //MessageBox.Show(ed.ToString(), "Erro3");
                                        }

                                        c = 0;

                                        for (int y = 0; y < dt4.Rows.Count; y++)
                                        {
                                            if (dt4.Rows[y][2].ToString() == "FIM DE JORNADA" && dt4.Rows[y][6].ToString() == "725405" || dt4.Rows[y][2].ToString() == "FIM DE JORNADA" && dt4.Rows[y][6].ToString() == "725407" || dt4.Rows[y][2].ToString() == "FIM DE JORNADA" && dt4.Rows[y][6].ToString() == "184525")
                                            {
                                                c = c + 1;
                                            }

                                        }
                                        string caminho = Server.MapPath("/TXT/");

                                        string filepath = caminho + arquivo;

                                        FileInfo file = new FileInfo(filepath);
                                        //if(file.Exists)
                                        //{
                                        //    file.Delete();
                                        //}



                                        for (int w = 0; w < dt4.Rows.Count; w++)
                                        {

                                            string codigo = lblCod.Text;
                                            string[] codigosEspeciais = { "330", "482", "820", "958", "989" };

                                            if (codigo.StartsWith("312"))
                                            {
                                                cracha = codigo.Insert(3, "0");
                                            }
                                            else if ((codigosEspeciais.Contains(codigo) || codigo.Length <= 4))
                                            {
                                                cracha = "30" + codigo;
                                            }
                                            else if (codigo.Length <= 3)
                                            {
                                                cracha = "300" + codigo;
                                            }

                                            else
                                            {
                                                cracha = codigo;
                                            }

                                            // cracha = "30" + lblCod.Text;
                                            int t = 0;
                                            t = w - 1;
                                            string data = DateTime.Parse(dt4.Rows[w][0].ToString()).ToString("dd/MM");
                                            string horas = DateTime.Parse(dt4.Rows[w][1].ToString()).ToString("HH:mm");
                                            if (t >= 0)
                                            {
                                                if (DateTime.Parse(dt4.Rows[w][1].ToString()).ToString("HH:mm") == DateTime.Parse(dt4.Rows[t][1].ToString()).ToString("HH:mm"))
                                                {
                                                    hora = DateTime.Parse(horas).AddMinutes(1).ToString("HH:mm");
                                                }
                                                else
                                                {
                                                    hora = DateTime.Parse(horas).ToString("HH:mm");
                                                }
                                            }
                                            else
                                            {
                                                hora = DateTime.Parse(horas).ToString("HH:mm");
                                            }
                                            //tipo = dt4.Rows[w][2].ToString();
                                            string transmissao = dt4.Rows[w][3].ToString();


                                            //if (dt4.Rows[w][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[w][2].ToString() == "TRECHO ALMOÇO" || dt4.Rows[w][2].ToString() == "FIM DE JORNADA")
                                            //{
                                            //    tipo = "2";
                                            //}
                                            //else  if (dt4.Rows[w][2].ToString() == "PARADA FORNECEDOR" || dt4.Rows[w][2].ToString() == "REINICIO DE VIAGEM")
                                            //{
                                            //    tipo = "21";
                                            //}





                                            #region BLOCO INICIO JORNADA
                                            //BLOCO INICIO JORNADA
                                            if (dt4.Rows[w][2].ToString() == "INICIO DE JORNADA")
                                            {
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0102");
                                                //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01020000");
                                                marcacoes.Add(hora);



                                            }

                                            if (dt4.Rows[w][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[w][2].ToString() == "INICIO JORNADA")
                                            {

                                                int a = 0;
                                                a = w - 1;
                                                if (a >= 0)
                                                {
                                                    if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA")
                                                    {
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");

                                                    }
                                                    //else
                                                    //{
                                                    //	//write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0102");
                                                    //	//write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01020000");
                                                    //	marcacoes.Add(hora);



                                                    //}
                                                }
                                                else
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                }


                                            }
                                            //FIM
                                            #endregion

                                            #region BLOCO INICIO JORNADA CAMINHAO
                                            //BLOCO INICIO JORNADA CAMINHAO

                                            if (dt4.Rows[w][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[w][2].ToString() == "INICIO JORNADA")
                                            {

                                                int a = 0;
                                                a = w - 1;
                                                if (a >= 0)
                                                {
                                                    if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA")
                                                    {
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");

                                                    }
                                                    else
                                                    {
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0102");
                                                        //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01020000");
                                                        marcacoes.Add(hora);
                                                    }
                                                }
                                                else
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                }


                                            }
                                            //FIM
                                            #endregion

                                            #region PARADA INTERNA
                                            //BLOCO PARADA INTERNA
                                            if (dt4.Rows[w][2].ToString() == "PARADA INTERNA")
                                            {
                                                if (dt4.Rows[w][5].ToString() == "3" || dt4.Rows[w][5].ToString() == "2")
                                                {
                                                    int a = 0;
                                                    a = w - 1;
                                                    if (a >= 0)
                                                    {
                                                        if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[a][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[a][2].ToString() == "INICIO JORNADA")
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");
                                                        }
                                                        else if (dt4.Rows[a][5].ToString() == "14")
                                                        {
                                                            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");
                                                            marcacoes.Add(hora);
                                                        }
                                                        else if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2" || dt4.Rows[a][5].ToString() == "1")
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                        }
                                                        else if (dt4.Rows[a][2].ToString() == "PARADA REFEICAO")
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");
                                                        }
                                                        else if (dt4.Rows[a][2].ToString() == "INICIO DE VIAGEM")
                                                        {
                                                            //////write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                            ////write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                            ////write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");
                                                        }
                                                        else if (dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;94");
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");
                                                        }
                                                        else if (dt4.Rows[a][2].ToString() == "FIM DE VIAGEM")
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                        }
                                                        else
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");
                                                        }

                                                    }

                                                }
                                                else if (dt4.Rows[w][5].ToString() == "14")
                                                {
                                                    int a = 0;
                                                    a = w - 1;
                                                    if (a >= 0)
                                                    {
                                                        if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[a][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[a][2].ToString() == "INICIO JORNADA")
                                                        {
                                                            marcacoes.Add(hora);
                                                        }
                                                        if (dt4.Rows[a][5].ToString() == "14")
                                                        {
                                                            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                            marcacoes.Add(hora);
                                                        }
                                                        else if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2" || dt4.Rows[a][5].ToString() == "1")
                                                        {
                                                            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                            marcacoes.Add(hora);
                                                        }
                                                        else if (dt4.Rows[a][2].ToString() == "PARADA REFEICAO")
                                                        {
                                                            // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");
                                                        }
                                                        else if (dt4.Rows[a][2].ToString() == "INICIO DE VIAGEM" || dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                                        {
                                                            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                            marcacoes.Add(hora);
                                                        }
                                                        else if (dt4.Rows[a][2].ToString() == "FIM DE VIAGEM")
                                                        {

                                                            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                            marcacoes.Add(hora);

                                                        }
                                                        if (dt4.Rows[a][5].ToString() == "22")
                                                        {
                                                            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                            marcacoes.Add(hora);
                                                        }
                                                        //else
                                                        //{
                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93: 71489");
                                                        //    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                                        //}

                                                    }
                                                }
                                                else if (dt4.Rows[w][5].ToString() == "22")
                                                {
                                                    int a = 0;
                                                    a = w - 1;
                                                    if (a >= 0)
                                                    {

                                                        if (dt4.Rows[a][5].ToString() == "14")
                                                        {
                                                            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");
                                                            marcacoes.Add(hora);
                                                        }


                                                    }
                                                }
                                                else
                                                {
                                                    int a = 0;
                                                    a = w - 1;
                                                    if (a >= 0)
                                                    {
                                                        if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[a][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[a][2].ToString() == "INICIO JORNADA")
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;95");
                                                        }
                                                        if (dt4.Rows[a][5].ToString() == "14")
                                                        {
                                                            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");
                                                            marcacoes.Add(hora);

                                                        }

                                                        else if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2" || dt4.Rows[a][5].ToString() == "1")
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;95");
                                                        }
                                                        else if (dt4.Rows[a][2].ToString() == "PARADA REFEICAO")
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                                            //// write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");
                                                        }
                                                        else if (dt4.Rows[a][2].ToString() == "INICIO DE VIAGEM" || dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                                        {

                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;94");
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;95");
                                                        }
                                                        else if (dt4.Rows[a][2].ToString() == "FIM DE VIAGEM" || dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                                        {

                                                            //// write.WriteLine(cracha + ";" + data + ";" + hora + ";1;94");
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;95");

                                                        }

                                                    }
                                                }

                                            }
                                            //FIM 
                                            #endregion

                                            #region BLOCO PARADA REFEIÇÃO
                                            //BLOCO PARADA REFEIÇÃO
                                            if (dt4.Rows[w][2].ToString() == "PARADA REFEICAO")
                                            {
                                                int a = 0;
                                                a = w - 1;
                                                if (a >= 0)
                                                {
                                                    //if (dt4.Rows[a][2].ToString() == "PARADA INTERNA")
                                                    //{

                                                    //    if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2")
                                                    //    {
                                                    //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                    //        write.WriteLine(cracha + ";" + data + ";" + hora + ";2;93");
                                                    //    }
                                                    //    else
                                                    //    {
                                                    //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");
                                                    //        write.WriteLine(cracha + ";" + data + ";" + hora + ";2;93");
                                                    //    }
                                                    //}
                                                    if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[a][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[a][2].ToString() == "INICIO JORNADA")
                                                    {
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0193");
                                                        //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                        marcacoes.Add(hora);

                                                    }
                                                    else
                                                    {
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0193");
                                                        //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                        marcacoes.Add(hora);
                                                    }

                                                }
                                                //else
                                                //{
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;93");
                                                //}
                                            }
                                            //FIM
                                            #endregion

                                            #region RETORNO REFEIÇAO
                                            //BLOCO RETORNO REFEIÇAO
                                            if (dt4.Rows[w][2].ToString() == "RETORNO REFEICAO")
                                            {

                                                int a = 0;
                                                int b = 0;
                                                a = w - 1;
                                                b = w + 1;
                                                if (a >= 0)
                                                {

                                                    if (dt4.Rows[a][2].ToString() == "PARADA REFEICAO")
                                                    {

                                                        // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0293");
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;0102");
                                                        //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");
                                                        marcacoes.Add(hora);

                                                    }
                                                    //if (dt4.Rows[b][2].ToString() == "FIM DE JORNADA")
                                                    //{
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                    //}
                                                    //else
                                                    //{
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                                    //    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");

                                                    //}
                                                }


                                            }
                                            //FIM
                                            #endregion

                                            #region INICIO DE VIAGEM
                                            //BLOCO REINICIO / INICIO DE VIAGEM
                                            if (dt4.Rows[w][2].ToString() == "INICIO DE VIAGEM")
                                            {
                                                int a = 0;
                                                a = w - 1;
                                                if (a >= 0)
                                                {
                                                    //if (dt4.Rows[a][2].ToString() == "PARADA INTERNA")
                                                    //{
                                                    //    if (dt4.Rows[a][5].ToString() == "14")
                                                    //    {
                                                    //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                                    //        write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");

                                                    //    }
                                                    //    else
                                                    //        if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2")
                                                    //    {
                                                    //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                    //        write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");

                                                    //    }
                                                    //    else
                                                    //    {
                                                    //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");
                                                    //        write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                    //    }
                                                    //}
                                                    //if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[a][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[a][2].ToString() == "INICIO JORNADA")
                                                    //{
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                    //}
                                                    //else if (dt4.Rows[a][2].ToString() == "PARADA PERNOITE")
                                                    //{
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;22");
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                    //}
                                                    //else if (dt4.Rows[a][2].ToString() == "FIM DE VIAGEM")
                                                    //{
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;94");
                                                    //    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                    //}

                                                    if (dt4.Rows[a][2].ToString() == "PARADA CLIENTE / FORNECEDOR")
                                                    {


                                                        if (dt4.Rows[a][5].ToString() == "14")
                                                        {

                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0193");
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                            marcacoes.Add(hora);



                                                        }
                                                        //else if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "1" || dt4.Rows[a][5].ToString() == "2" || dt4.Rows[a][5].ToString() == "4")
                                                        //{
                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                        //}



                                                    }
                                                    else if (dt4.Rows[a][2].ToString() == "PARADA")
                                                    {


                                                        if (dt4.Rows[a][5].ToString() == "14")
                                                        {

                                                            // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0193");
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                            marcacoes.Add(hora);
                                                        }
                                                        //else
                                                        //{
                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");
                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");



                                                        //}



                                                    }
                                                    //else if (dt4.Rows[a][2].ToString() == "RETORNO REFEICAO")
                                                    //{
                                                    //    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0293");
                                                    //    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                    //    write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");

                                                    //}

                                                }
                                                //else
                                                //{
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                //}



                                            }
                                            //FIM
                                            #endregion

                                            #region PARADA CLIENTE / FORNECEDOR
                                            if (dt4.Rows[w][2].ToString() == "PARADA CLIENTE / FORNECEDOR")
                                            {
                                                if (dt4.Rows[w][5].ToString() == "3" || dt4.Rows[w][5].ToString() == "2" || dt4.Rows[w][5].ToString() == "4")
                                                {
                                                    int a = 0;
                                                    a = w - 1;
                                                    if (a >= 0)
                                                    {
                                                        //if (dt4.Rows[a][2].ToString() == "INICIO DE VIAGEM" || dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM" || dt4.Rows[a][2].ToString() == "FIM DE VIAGEM")
                                                        //{
                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;94");
                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");
                                                        //}
                                                        if (dt4.Rows[a][2].ToString() == "PARADA CLIENTE / FORNECEDOR")
                                                        {
                                                            if (dt4.Rows[a][5].ToString() == "14")
                                                            {
                                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0293");
                                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;0102");
                                                                //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");
                                                                marcacoes.Add(hora);


                                                            }
                                                            //else if (dt4.Rows[a][5].ToString() == "15")
                                                            //{
                                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;94");
                                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");

                                                            //}
                                                            //else
                                                            //{
                                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");
                                                            //}

                                                        }
                                                    }

                                                }
                                                else if (dt4.Rows[w][5].ToString() == "14")
                                                {
                                                    int a = 0;
                                                    a = w - 1;
                                                    if (a >= 0)
                                                    {
                                                        if (dt4.Rows[a][2].ToString() == "INICIO DE VIAGEM" || dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM" || dt4.Rows[a][2].ToString() == "FIM DE VIAGEM")
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                            // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0193");
                                                            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                            marcacoes.Add(hora);
                                                        }
                                                        else if (dt4.Rows[a][2].ToString() == "PARADA CLIENTE / FORNECEDOR")
                                                        {
                                                            if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2" || dt4.Rows[a][5].ToString() == "4")
                                                            {
                                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0193");
                                                                //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                                marcacoes.Add(hora);
                                                            }
                                                            else if (dt4.Rows[a][5].ToString() == "14")
                                                            {
                                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0193");
                                                                marcacoes.Add(hora);

                                                            }


                                                        }
                                                        else
                                                        {
                                                            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                            marcacoes.Add(hora);
                                                        }

                                                    }


                                                }


                                            }
                                            #endregion

                                            #region REINICIO
                                            //BLOCO REINICIO / INICIO DE VIAGEM
                                            if (dt4.Rows[w][2].ToString() == "REINICIO DE VIAGEM")
                                            {
                                                int a = 0;
                                                a = w - 1;
                                                if (a >= 0)
                                                {
                                                    if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[a][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[a][2].ToString() == "INICIO JORNADA")
                                                    {

                                                    }
                                                    else if (dt4.Rows[a][2].ToString() == "PARADA PERNOITE")
                                                    {
                                                        //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01020000");
                                                        marcacoes.Add(hora);
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                    }

                                                    else if (dt4.Rows[a][2].ToString() == "PARADA CLIENTE / FORNECEDOR")
                                                    {

                                                        if (a >= 0)
                                                        {

                                                            if (dt4.Rows[a][2].ToString() == "PARADA CLIENTE / FORNECEDOR")
                                                            {
                                                                //if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2")
                                                                //{
                                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                                //}
                                                                if (dt4.Rows[a][5].ToString() == "14")
                                                                {

                                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0293");
                                                                    marcacoes.Add(hora);
                                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0102");

                                                                }
                                                                //else if (dt4.Rows[a][5].ToString() == "15")
                                                                //{

                                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");

                                                                //}
                                                                //else
                                                                //{
                                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                                //}
                                                            }
                                                            //else if (dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                                            //{
                                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                            //}


                                                        }



                                                    }
                                                    else if (dt4.Rows[a][2].ToString() == "PARADA")
                                                    {
                                                        if (dt4.Rows[a][5].ToString() == "14")
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0293");
                                                            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0102");
                                                            marcacoes.Add(hora);
                                                        }
                                                        //else
                                                        //{
                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");
                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                        //}




                                                    }
                                                    //else if (dt4.Rows[a][2].ToString() == "OFICINA / MANUTENCAO")
                                                    //{

                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");


                                                    //}

                                                }
                                                else
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0102");
                                                    marcacoes.Add(hora);
                                                    // write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                }



                                            }
                                            //FIM
                                            #endregion

                                            #region BLOCO PARADA
                                            // BLOCO PARADA
                                            if (dt4.Rows[w][2].ToString() == "PARADA")
                                            {
                                                int a = 0;
                                                a = w - 1;


                                                if (dt4.Rows[w][5].ToString() == "14")
                                                {
                                                    if (dt4.Rows.Count >= a)
                                                    {
                                                        if (dt4.Rows[a][2].ToString() == "INICIO DE VIAGEM" || dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                                        {
                                                            // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0193");
                                                            marcacoes.Add(hora);


                                                        }

                                                    }
                                                }
                                                //else
                                                //{
                                                //    if (dt4.Rows.Count >= a)
                                                //    {
                                                //        if (dt4.Rows[a][2].ToString() == "INICIO DE VIAGEM" || dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                                //        {
                                                //            write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                //            //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;95");

                                                //        }
                                                //        //else
                                                //        //{
                                                //        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                //        //    // write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                //        //}
                                                //    }
                                                //}



                                                //  write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");

                                            }
                                            //FIM
                                            #endregion

                                            #region FIM DE VIAGEM
                                            //BLOCO FIM DE VIAGEM
                                            if (dt4.Rows[w][2].ToString() == "FIM DE VIAGEM")
                                            {
                                                int a = 0;
                                                int g = 0;
                                                a = w - 1;
                                                g = w - 2;
                                                if (a >= 0)
                                                {
                                                    if (dt4.Rows[a][2].ToString() == "PARADA CLIENTE / FORNECEDOR")
                                                    {


                                                        if (dt4.Rows[a][5].ToString() == "14")
                                                        {

                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0293");
                                                            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;0102");
                                                            marcacoes.Add(hora);

                                                        }

                                                        //else if (dt4.Rows[a][5].ToString() == "14")
                                                        //{
                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                                        //    // write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                        //}

                                                        //else
                                                        //{
                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                        //    // write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                        //}



                                                    }
                                                    else if (dt4.Rows[a][2].ToString() == "PARADA")
                                                    {


                                                        if (dt4.Rows[a][5].ToString() == "14")
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0293");
                                                            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");
                                                            marcacoes.Add(hora);
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;0102");

                                                        }

                                                        //else
                                                        //{
                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                        //    // write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");

                                                        //}


                                                    }

                                                    else if (dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                                    {
                                                        if (g >= 0)
                                                        {
                                                            //if (dt4.Rows[g][2].ToString() == "PARADA PERNOITE")
                                                            //{
                                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                            //    write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                            //}
                                                            //else
                                                            //{
                                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                            //}
                                                        }
                                                        //else
                                                        //{
                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                        //}


                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                    }


                                                }
                                                //else
                                                //{
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                //}

                                            }

                                            //
                                            #endregion

                                            #region FIM DE JORNADA
                                            //BLOCO FIM DE JORMADA
                                            if (c > 0)
                                            {
                                                if (dt4.Rows[w][2].ToString() == "FIM DE JORNADA" && dt4.Rows[w][6].ToString() == "725405" || dt4.Rows[w][2].ToString() == "FIM DE JORNADA" && dt4.Rows[w][6].ToString() == "725407" || dt4.Rows[w][2].ToString() == "FIM DE JORNADA" && dt4.Rows[w][6].ToString() == "184525" && dt4.Rows[w][6].ToString() == "184522")
                                                {
                                                    int a = 0;
                                                    a = w - 1;

                                                    if (a >= 0)
                                                    {
                                                        if (dt4.Rows[a][2].ToString() == "RETORNO REFEICAO")
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02020000");
                                                            //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                            marcacoes.Add(hora);

                                                        }
                                                        else if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA")
                                                        {

                                                            //WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02020000");
                                                            marcacoes.Add(hora);


                                                        }
                                                        else if (dt4.Rows[a][2].ToString() == "RETORNO REFEICAO")
                                                        {

                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02020000");
                                                            marcacoes.Add(hora);

                                                        }
                                                        else if (dt4.Rows[a][2].ToString() == "FIM DE VIAGEM")
                                                        {
                                                            int b = 0;
                                                            b = a - 1;

                                                            if (b >= 0)
                                                            {
                                                                if (dt4.Rows[b][2].ToString() == "INICIO DE VIAGEM" || dt4.Rows[b][2].ToString() == "REINICIO DE VIAGEM")
                                                                {
                                                                    int d = 0;
                                                                    d = b - 1;
                                                                    if (d >= 0)
                                                                    {

                                                                        if (dt4.Rows[d][2].ToString() == "PARADA")
                                                                        {
                                                                            if (dt4.Rows[d][5].ToString() == "14")
                                                                            {
                                                                                // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                                //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02020000");
                                                                                //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                                                marcacoes.Add(hora);
                                                                            }
                                                                            else
                                                                            {
                                                                                // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                                //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02020000");
                                                                                // write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                                                marcacoes.Add(hora);
                                                                            }




                                                                        }
                                                                        else
                                                                        {
                                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02020000");
                                                                            // write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                                            marcacoes.Add(hora);
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02020000");
                                                                    // write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                                    marcacoes.Add(hora);
                                                                }

                                                            }
                                                            else
                                                            {
                                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                marcacoes.Add(hora);
                                                                //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");

                                                            }

                                                        }
                                                        else if (dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02020000");
                                                            //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                            marcacoes.Add(hora);
                                                        }
                                                        else if (dt4.Rows[a][2].ToString() == "PARADA")
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                            marcacoes.Add(hora);
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                        }
                                                        else if (dt4.Rows[a][2].ToString() == "PARADA INTERNA")
                                                        {
                                                            if (dt4.Rows[a][5].ToString() == "14")
                                                            {
                                                                //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");
                                                                marcacoes.Add(hora);
                                                            }
                                                            else
                                                            {
                                                                marcacoes.Add(hora);
                                                            }
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");

                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                        }
                                                        else if (dt4.Rows[a][2].ToString() == "FIM DE JORNADA")
                                                        {
                                                            int m = 0;
                                                            m = a - 1;

                                                            if (m >= 0)
                                                            {
                                                                if (dt4.Rows[m][2].ToString() == "RETORNO REFEICAO" || dt4.Rows[m][2].ToString() == "INICIO DE JORNADA")
                                                                {
                                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                    marcacoes.Add(hora);
                                                                }
                                                                else if (dt4.Rows[m][2].ToString() == "FIM DE VIAGEM")
                                                                {
                                                                    int b = 0;
                                                                    b = m - 1;

                                                                    if (b >= 0)
                                                                    {
                                                                        if (dt4.Rows[b][2].ToString() == "INICIO DE VIAGEM" || dt4.Rows[b][2].ToString() == "REINICIO DE VIAGEM")
                                                                        {
                                                                            int d = 0;
                                                                            d = b - 1;
                                                                            if (d >= 0)
                                                                            {

                                                                                if (dt4.Rows[d][2].ToString() == "PARADA")
                                                                                {
                                                                                    if (dt4.Rows[d][5].ToString() == "14")
                                                                                    {
                                                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                                        marcacoes.Add(hora);
                                                                                        //.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                                        marcacoes.Add(hora);
                                                                                        //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");

                                                                                    }


                                                                                }
                                                                                else
                                                                                {
                                                                                    // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                                    marcacoes.Add(hora);
                                                                                    //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                                                }
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                            marcacoes.Add(hora);
                                                                            //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                                        }

                                                                    }


                                                                }
                                                                //else if (dt4.Rows[m][2].ToString() == "REINICIO DE VIAGEM")
                                                                //{
                                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;94");
                                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                                //}
                                                                //else if (dt4.Rows[m][2].ToString() == "PARADA INTERNA")
                                                                //{
                                                                //    if (dt4.Rows[m][5].ToString() == "3" || dt4.Rows[m][5].ToString() == "1" || dt4.Rows[m][5].ToString() == "2")
                                                                //    {
                                                                //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");

                                                                //    }
                                                                //    else if (dt4.Rows[m][5].ToString() == "22")
                                                                //    {
                                                                //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");
                                                                //    }

                                                                //    else if (dt4.Rows[m][2].ToString() == "FIM DE JORNADA")
                                                                //    {
                                                                //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                                //        write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                                //    }

                                                                //}
                                                                else if (dt4.Rows[m][2].ToString() == "PARADA")
                                                                {
                                                                    // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                    marcacoes.Add(hora);
                                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                                }
                                                                //else if (dt4.Rows[m][2].ToString() == "PARADA INTERNA")
                                                                //{
                                                                //    if (dt4.Rows[m][5].ToString() != "3" || dt4.Rows[m][5].ToString() != "1" || dt4.Rows[m][5].ToString() != "2")
                                                                //    {
                                                                //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");
                                                                //        write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                                //        write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");

                                                                //    }

                                                                //}

                                                                else
                                                                {
                                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                    marcacoes.Add(hora);
                                                                    //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                                }


                                                            }
                                                            else
                                                            {
                                                                //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                            }
                                                        }
                                                        else
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                            marcacoes.Add(hora);
                                                            //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                        }


                                                    }
                                                    else
                                                    {
                                                        // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                        marcacoes.Add(hora);
                                                        //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                    }
                                                }

                                            }
                                            else
                                            {
                                                if (dt4.Rows[w][2].ToString() == "FIM DE JORNADA")
                                                {
                                                    int q = 0;
                                                    q = w + 1;

                                                    if (q < dt4.Rows.Count)
                                                    {
                                                        if (dt4.Rows[q][2].ToString() == "FIM DE JORNADA")
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                            marcacoes.Add(hora);
                                                            //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");

                                                        }
                                                        else
                                                        {
                                                            int a = 0;
                                                            a = w - 1;

                                                            if (a >= 0)
                                                            {

                                                                if (dt4.Rows[a][2].ToString() == "FIM DE VIAGEM" || dt4.Rows[a][2].ToString() == "RETORNO REFEICAO" || dt4.Rows[a][2].ToString() == "INICIO DE JORNADA")
                                                                {
                                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                    marcacoes.Add(hora);
                                                                    // write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");

                                                                }
                                                                //else if (dt4.Rows[a][2].ToString() == "PARADA INTERNA")
                                                                //{
                                                                //    if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2")
                                                                //    {
                                                                //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                                //        // write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                                //        // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                                //    }
                                                                //    else if (dt4.Rows[a][5].ToString() == "14")
                                                                //    {
                                                                //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                                //        write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                                //    }
                                                                //    else
                                                                //    {
                                                                //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");


                                                                //    }
                                                                //}
                                                                else if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[a][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[a][2].ToString() == "INICIO JORNADA")
                                                                {

                                                                }
                                                                else if (dt4.Rows[a][2].ToString() == "FIM DE JORNADA")
                                                                {
                                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                    marcacoes.Add(hora);
                                                                    //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                                }
                                                                else
                                                                {
                                                                    // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                    marcacoes.Add(hora);
                                                                    // write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                                }
                                                            }
                                                        }

                                                    }
                                                    else
                                                    {
                                                        int a = 0;
                                                        a = w - 1;

                                                        if (a >= 0)
                                                        {

                                                            if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA")
                                                            {
                                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                marcacoes.Add(hora);

                                                            }
                                                            else if (dt4.Rows[a][2].ToString() == "FIM DE VIAGEM" || dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                                            {
                                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                marcacoes.Add(hora);
                                                                //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");

                                                            }
                                                            else if (dt4.Rows[a][2].ToString() == "RETORNO REFEICAO")
                                                            {
                                                                //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                marcacoes.Add(hora);

                                                            }
                                                            else if (dt4.Rows[a][2].ToString() == "PARADA INTERNA")
                                                            {
                                                                if (dt4.Rows[a][5].ToString() == "14")
                                                                {
                                                                    marcacoes.Add(hora);
                                                                }
                                                                else
                                                                {
                                                                    marcacoes.Add(hora);
                                                                }
                                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");

                                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                            }
                                                            //else if (dt4.Rows[a][2].ToString() == "PARADA INTERNA")
                                                            //{
                                                            //    if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2")
                                                            //    {
                                                            //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                            //        // write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                            //        // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                            //    }
                                                            //    else if (dt4.Rows[a][5].ToString() == "14")
                                                            //    {
                                                            //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                            //        write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                            //    }
                                                            //    else
                                                            //    {
                                                            //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");

                                                            //    }
                                                            //}
                                                            else if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[a][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[a][2].ToString() == "INICIO JORNADA")
                                                            {

                                                            }
                                                            else if (dt4.Rows[a][2].ToString() == "FIM DE JORNADA")
                                                            {
                                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                marcacoes.Add(hora);

                                                                //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                            }
                                                            else
                                                            {
                                                                // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                            }
                                                        }
                                                        else
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                            marcacoes.Add(hora);
                                                            //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                        }
                                                    }
                                                }





                                            }


                                            #endregion

                                            #region PARADA PERNOITE

                                            if (dt4.Rows[w][2].ToString() == "PARADA PERNOITE")
                                            {
                                                int a = 0;
                                                a = w - 1;


                                                if (a >= 0)
                                                {
                                                    if (dt4.Rows[a][2].ToString() == "PARADA CLIENTE / FORNECEDOR")
                                                    {


                                                        if (dt4.Rows[a][5].ToString() == "14")
                                                        {

                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0293");
                                                            marcacoes.Add(hora);
                                                            //WriteLine(cracha + ";" + data + ";" + hora + ";1;0102");
                                                            //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;0102");
                                                            marcacoes.Add(DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm").Replace(":", ""));



                                                        }
                                                        else if (dt4.Rows[a][5].ToString() == "15")
                                                        {

                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                            marcacoes.Add(hora);
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;22");


                                                        }
                                                        //else if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2")
                                                        //{
                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                        //    write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;22");
                                                        //    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                        //}
                                                        else
                                                        {
                                                            marcacoes.Add(hora);
                                                        }



                                                    }
                                                    else if (dt4.Rows[a][2].ToString() == "PARADA")
                                                    {
                                                        //if (dt4.Rows[a][5].ToString() == "7")
                                                        //{
                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                        //    write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;22");
                                                        //}
                                                        if (dt4.Rows[a][5].ToString() == "14")
                                                        {

                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0293");
                                                            marcacoes.Add(hora);
                                                            //WriteLine(cracha + ";" + data + ";" + hora + ";1;0102");
                                                            //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;0102");
                                                            marcacoes.Add(DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm").Replace(":", ""));


                                                        }
                                                        else
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                            marcacoes.Add(hora);
                                                            //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                        }

                                                    }
                                                    else if (dt4.Rows[a][2].ToString() == "INICIO DE VIAGEM")
                                                    {
                                                        marcacoes.Add(hora);
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                        //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                        //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";2;22");
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";3;22");
                                                    }
                                                    else if (dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                                    {
                                                        marcacoes.Add(hora);
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                        //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");

                                                        // write.WriteLine(cracha + ";" + data + ";" + hora + ";3;22");
                                                    }
                                                    else if (dt4.Rows[a][2].ToString() == "RETORNO REFEICAO")
                                                    {
                                                        marcacoes.Add(hora);
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                        //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");

                                                        // write.WriteLine(cracha + ";" + data + ";" + hora + ";3;22");
                                                    }
                                                    //else if (dt4.Rows[a][2].ToString() == "PARADA INTERNA")
                                                    //{


                                                    //    if (dt4.Rows[a][5].ToString() == "14")
                                                    //    {

                                                    //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                                    //        //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;22");


                                                    //    }
                                                    //    //else if (dt4.Rows[a][5].ToString() == "15")
                                                    //    //{

                                                    //    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                    //    //    write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;22");


                                                    //    //}
                                                    //    else if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2")
                                                    //    {
                                                    //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                    //        //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;22");
                                                    //        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                    //    }
                                                    //    //else
                                                    //    //{
                                                    //    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;22");
                                                    //    //}



                                                    //}
                                                    //else
                                                    //{
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;22");
                                                    //}

                                                }
                                                //else
                                                //{
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;22");
                                                //    write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;22");

                                                //}

                                            }

                                            #endregion

                                            #region RETORNO PERNOITE
                                            //BLOCO REINICIO / INICIO DE VIAGEM
                                            if (dt4.Rows[w][2].ToString() == "RETORNO PERNOITE")
                                            {
                                                int a = 0;
                                                a = w - 1;
                                                if (a >= 0)
                                                {
                                                    if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[a][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[a][2].ToString() == "INICIO JORNADA")
                                                    {

                                                    }
                                                    else if (dt4.Rows[a][2].ToString() == "PARADA PERNOITE")
                                                    {
                                                        //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01020000");
                                                        marcacoes.Add(hora);
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                    }


                                                }
                                                else
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0102");
                                                    marcacoes.Add(hora);
                                                    // write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                }



                                            }
                                            //FIM
                                            #endregion



                                        }

                                        //FIM

                                        #endregion
                                    }
                                }
                            }

                        }

                    }
                    else
                    {
                        //JORNADA DIFERENCIA DE COMEÇO

                        //VERIFICA SE A ULTIMA MARCAÇÃO DO DIA É PARADA PERNOITE OU FIM DE VIAGEM
                        string sqlz = "select TOP 1 ds_macro from tb_parada where cod_cracha=" + lblCod.Text + " and dt_posicao_parada='" + DateTime.Parse(dte.Rows[f][0].ToString()).ToString("yyyy-MM-dd") + "' and fl_deletado is null order by hr_posicao  DESC ";
                        SqlDataAdapter adtpz = new SqlDataAdapter(sqlz, con);
                        DataTable dtz = new DataTable();
                        con.Open();
                        adtpz.Fill(dtz);
                        con.Close();




                        if (dtz.Rows[0][0].ToString() == "FIM DE JORNADA" || dtz.Rows[0][0].ToString() == "PARADA PERNOITE")
                        {
                            //JORNADA NORMAL COM DIFERENÇA NO COMEÇO
                            string sqld = "select TOP 1  ds_macro, hr_posicao from tb_parada  where  cod_cracha=" + lblCod.Text + " and fl_deletado is null and dt_posicao_parada='" + DateTime.Parse(dte.Rows[f][0].ToString()).ToString("yyyy-MM-dd") + "'  and hr_posicao > (select top 1 hr_posicao from tb_parada where cod_cracha=" + lblCod.Text + " and fl_deletado is null and dt_posicao_parada='" + DateTime.Parse(dte.Rows[f][0].ToString()).ToString("yyyy-MM-dd") + "'  and ds_macro='FIM DE JORNADA'  OR cod_cracha=" + lblCod.Text + " and fl_deletado is null and dt_posicao_parada='" + DateTime.Parse(dte.Rows[f][0].ToString()).ToString("yyyy-MM-dd") + "'  and ds_macro='PARADA PERNOITE' order by hr_posicao) order by hr_posicao ";
                            SqlDataAdapter adtpd = new SqlDataAdapter(sqld, con);
                            DataTable dtd = new DataTable();
                            con.Open();
                            adtpd.Fill(dtd);
                            con.Close();

                            if (dtd.Rows.Count > 0)
                            {
                                dtRow["Dia"] = DateTime.Parse(dte.Rows[f][0].ToString()).ToString("dd");
                                #region Jornada Normal
                                //string arquivo = "Transmot" + DateTime.Parse(txtDtInicial.Text).ToString("ddMM") + "001.txt";

                                sql4 = "select dt_posicao_parada, hr_posicao, ds_macro,cod_transmissao, cod_cracha, isnull(cod_ref_parada,0) as cod_ref_parada, cod_idveiculo from tb_parada where fl_deletado is null and cod_cracha=" + lblCod.Text + " and dt_posicao_parada='" + DateTime.Parse(dte.Rows[f][0].ToString()).ToString("yyyy-MM-dd") + "' and fl_deletado is null and hr_posicao >= '" + dtd.Rows[0][1].ToString() + "' AND (ds_macro IN ('INICIO DE JORNADA', 'INICIO JORNADA CAMINHAO', 'PARADA REFEICAO', 'FIM DE JORNADA','REINICIO DE VIAGEM','FIM DE VIAGEM', 'PARADA','PARADA INTERNA','INICIO DE VIAGEM','PARADA INTERNA','RETORNO REFEICAO','PARADA CLIENTE / FORNECEDOR','PARADA PERNOITE','RETORNO PERNOITE') OR cod_ref_parada in (2,3,4,14,22)) AND fl_deletado IS NULL ORDER BY dt_posicao_parada, hr_posicao";
                                // string sql4 = "select dt_posicao_parada, hr_posicao, ds_macro,cod_transmissao, cod_cracha, isnull(cod_ref_parada,0) as cod_ref_parada, cod_idveiculo from tb_parada where fl_deletado is null and cod_cracha=" + lblCod.Text + " and dt_posicao_parada between '" + txtDtInicial.Text + "' and '" + txtDtFinal.Text + "' order by dt_posicao_parada, hr_posicao";
                                SqlDataAdapter adtp4 = new SqlDataAdapter(sql4, con);
                                DataTable dt4 = new DataTable();

                                try
                                {
                                    con.Open();
                                    adtp4.Fill(dt4);
                                    con.Close();
                                }
                                catch (Exception ed)
                                {
                                    // MessageBox.Show(ed.ToString(), "Erro3");
                                }

                                c = 0;

                                for (int y = 0; y < dt4.Rows.Count; y++)
                                {
                                    if (dt4.Rows[y][2].ToString() == "FIM DE JORNADA" && dt4.Rows[y][6].ToString() == "725405" || dt4.Rows[y][2].ToString() == "FIM DE JORNADA" && dt4.Rows[y][6].ToString() == "725407" || dt4.Rows[y][2].ToString() == "FIM DE JORNADA" && dt4.Rows[y][6].ToString() == "184525")
                                    {
                                        c = c + 1;
                                    }

                                }
                                string caminho = Server.MapPath("/TXT/");

                                string filepath = caminho + arquivo;

                                FileInfo file = new FileInfo(filepath);

                                //if (file.Exists)
                                //{
                                //    file.Delete();
                                //}


                                for (int w = 0; w < dt4.Rows.Count; w++)
                                {
                                    //if (chkDiadema.Checked || chkCadiriri.Checked)
                                    //{


                                    //        if (lblCod.Text.Length < 4)
                                    //        {
                                    //            cracha = "300" + lblCod.Text;
                                    //        }
                                    //        else if (lblCod.Text.Length >= 4 || lblCod.Text.Length <= 5)
                                    //        {
                                    //            cracha = "30" + lblCod.Text;
                                    //        }
                                    //        else
                                    //        {
                                    //            cracha = lblCod.Text;
                                    //        }




                                    //}
                                    //else
                                    //{
                                    //    if (lblCod.Text.Length < 4)
                                    //    {
                                    //        cracha = "300" + lblCod.Text;
                                    //    }
                                    //    else if (lblCod.Text.Length >= 4 || lblCod.Text.Length <= 5)
                                    //    {
                                    //        cracha = "30" + lblCod.Text;
                                    //    }
                                    //    else
                                    //    {
                                    //        cracha = lblCod.Text;
                                    //    }
                                    //}
                                    string codigo = lblCod.Text;
                                    string[] codigosEspeciais = { "330", "482", "820", "958", "989" };

                                    if (codigo.StartsWith("312"))
                                    {
                                        cracha = codigo.Insert(3, "0");
                                    }
                                    else if ((codigosEspeciais.Contains(codigo) || codigo.Length <= 4))
                                    {
                                        cracha = "30" + codigo;
                                    }
                                    else if (codigo.Length <= 3)
                                    {
                                        cracha = "300" + codigo;
                                    }

                                    else
                                    {
                                        cracha = codigo;
                                    }





                                    //  cracha = "30" + lblCod.Text;
                                    int t = 0;
                                    t = w - 1;

                                    string data = DateTime.Parse(dt4.Rows[w][0].ToString()).ToString("dd/MM");
                                    string horas = DateTime.Parse(dt4.Rows[w][1].ToString()).ToString("HH:mm");
                                    if (t >= 0)
                                    {
                                        if (DateTime.Parse(dt4.Rows[w][1].ToString()).ToString("HH:mm") == DateTime.Parse(dt4.Rows[t][1].ToString()).ToString("HH:mm"))
                                        {
                                            hora = DateTime.Parse(horas).AddMinutes(1).ToString("HH:mm");
                                        }
                                        else
                                        {
                                            hora = DateTime.Parse(horas).ToString("HH:mm");
                                        }
                                    }
                                    else
                                    {
                                        hora = DateTime.Parse(horas).ToString("HH:mm");
                                    }
                                    //tipo = dt4.Rows[w][2].ToString();
                                    string transmissao = dt4.Rows[w][3].ToString();


                                    //if (dt4.Rows[w][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[w][2].ToString() == "TRECHO ALMOÇO" || dt4.Rows[w][2].ToString() == "FIM DE JORNADA")
                                    //{
                                    //    tipo = "2";
                                    //}
                                    //else  if (dt4.Rows[w][2].ToString() == "PARADA FORNECEDOR" || dt4.Rows[w][2].ToString() == "REINICIO DE VIAGEM")
                                    //{
                                    //    tipo = "21";
                                    //}









                                    #region BLOCO INICIO JORNADA
                                    //BLOCO INICIO JORNADA
                                    if (dt4.Rows[w][2].ToString() == "INICIO DE JORNADA")
                                    {
                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0102");
                                        //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01020000");
                                        marcacoes.Add(hora);



                                    }

                                    if (dt4.Rows[w][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[w][2].ToString() == "INICIO JORNADA")
                                    {

                                        int a = 0;
                                        a = w - 1;
                                        if (a >= 0)
                                        {
                                            if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA")
                                            {
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");

                                            }
                                            //else
                                            //{
                                            //	//write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0102");
                                            //	//write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01020000");
                                            //	marcacoes.Add(hora);



                                            //}
                                        }
                                        else
                                        {
                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                        }


                                    }
                                    //FIM
                                    #endregion

                                    #region BLOCO INICIO JORNADA CAMINHAO
                                    //BLOCO INICIO JORNADA CAMINHAO

                                    if (dt4.Rows[w][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[w][2].ToString() == "INICIO JORNADA")
                                    {

                                        int a = 0;
                                        a = w - 1;
                                        if (a >= 0)
                                        {
                                            if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA")
                                            {
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");

                                            }
                                            else
                                            {
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0102");
                                                //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01020000");
                                                marcacoes.Add(hora);
                                            }
                                        }
                                        else
                                        {
                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                        }


                                    }
                                    //FIM
                                    #endregion

                                    #region PARADA INTERNA
                                    //BLOCO PARADA INTERNA
                                    if (dt4.Rows[w][2].ToString() == "PARADA INTERNA")
                                    {
                                        if (dt4.Rows[w][5].ToString() == "3" || dt4.Rows[w][5].ToString() == "2")
                                        {
                                            int a = 0;
                                            a = w - 1;
                                            if (a >= 0)
                                            {
                                                if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[a][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[a][2].ToString() == "INICIO JORNADA")
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");
                                                }
                                                else if (dt4.Rows[a][5].ToString() == "14")
                                                {
                                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");
                                                    marcacoes.Add(hora);
                                                }
                                                else if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2" || dt4.Rows[a][5].ToString() == "1")
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                }
                                                else if (dt4.Rows[a][2].ToString() == "PARADA REFEICAO")
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");
                                                }
                                                else if (dt4.Rows[a][2].ToString() == "INICIO DE VIAGEM")
                                                {
                                                    //////write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                    ////write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                    ////write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");
                                                }
                                                else if (dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;94");
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");
                                                }
                                                else if (dt4.Rows[a][2].ToString() == "FIM DE VIAGEM")
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                }
                                                else
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");
                                                }

                                            }

                                        }
                                        else if (dt4.Rows[w][5].ToString() == "14")
                                        {
                                            int a = 0;
                                            a = w - 1;
                                            if (a >= 0)
                                            {
                                                if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[a][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[a][2].ToString() == "INICIO JORNADA")
                                                {
                                                    marcacoes.Add(hora);
                                                }
                                                if (dt4.Rows[a][5].ToString() == "14")
                                                {
                                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                    marcacoes.Add(hora);
                                                }
                                                else if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2" || dt4.Rows[a][5].ToString() == "1")
                                                {
                                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                    marcacoes.Add(hora);
                                                }
                                                else if (dt4.Rows[a][2].ToString() == "PARADA REFEICAO")
                                                {
                                                    // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");
                                                }
                                                else if (dt4.Rows[a][2].ToString() == "INICIO DE VIAGEM" || dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                                {
                                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                    marcacoes.Add(hora);
                                                }
                                                else if (dt4.Rows[a][2].ToString() == "FIM DE VIAGEM")
                                                {

                                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                    marcacoes.Add(hora);

                                                }
                                                if (dt4.Rows[a][5].ToString() == "22")
                                                {
                                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                    marcacoes.Add(hora);
                                                }
                                                //else
                                                //{
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93: 71489");
                                                //    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                                //}

                                            }
                                        }
                                        else if (dt4.Rows[w][5].ToString() == "22")
                                        {
                                            int a = 0;
                                            a = w - 1;
                                            if (a >= 0)
                                            {

                                                if (dt4.Rows[a][5].ToString() == "14")
                                                {
                                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");
                                                    marcacoes.Add(hora);
                                                }


                                            }
                                        }
                                        else
                                        {
                                            int a = 0;
                                            a = w - 1;
                                            if (a >= 0)
                                            {
                                                if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[a][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[a][2].ToString() == "INICIO JORNADA")
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;95");
                                                }
                                                if (dt4.Rows[a][5].ToString() == "14")
                                                {
                                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");
                                                    marcacoes.Add(hora);

                                                }

                                                else if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2" || dt4.Rows[a][5].ToString() == "1")
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;95");
                                                }
                                                else if (dt4.Rows[a][2].ToString() == "PARADA REFEICAO")
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                                    //// write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");
                                                }
                                                else if (dt4.Rows[a][2].ToString() == "INICIO DE VIAGEM" || dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                                {

                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;94");
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;95");
                                                }
                                                else if (dt4.Rows[a][2].ToString() == "FIM DE VIAGEM" || dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                                {

                                                    //// write.WriteLine(cracha + ";" + data + ";" + hora + ";1;94");
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;95");

                                                }

                                            }
                                        }

                                    }
                                    //FIM 
                                    #endregion

                                    #region BLOCO PARADA REFEIÇÃO
                                    //BLOCO PARADA REFEIÇÃO
                                    if (dt4.Rows[w][2].ToString() == "PARADA REFEICAO")
                                    {
                                        int a = 0;
                                        a = w - 1;
                                        if (a >= 0)
                                        {
                                            //if (dt4.Rows[a][2].ToString() == "PARADA INTERNA")
                                            //{

                                            //    if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2")
                                            //    {
                                            //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                            //        write.WriteLine(cracha + ";" + data + ";" + hora + ";2;93");
                                            //    }
                                            //    else
                                            //    {
                                            //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");
                                            //        write.WriteLine(cracha + ";" + data + ";" + hora + ";2;93");
                                            //    }
                                            //}
                                            if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[a][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[a][2].ToString() == "INICIO JORNADA")
                                            {
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0193");
                                                //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                marcacoes.Add(hora);

                                            }
                                            else
                                            {
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0193");
                                                //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                marcacoes.Add(hora);
                                            }

                                        }
                                        //else
                                        //{
                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");
                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;93");
                                        //}
                                    }
                                    //FIM
                                    #endregion

                                    #region RETORNO REFEIÇAO
                                    //BLOCO RETORNO REFEIÇAO
                                    if (dt4.Rows[w][2].ToString() == "RETORNO REFEICAO")
                                    {

                                        int a = 0;
                                        int b = 0;
                                        a = w - 1;
                                        b = w + 1;
                                        if (a >= 0)
                                        {

                                            if (dt4.Rows[a][2].ToString() == "PARADA REFEICAO")
                                            {

                                                // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0293");
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;0102");
                                                //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");
                                                marcacoes.Add(hora);

                                            }
                                            //if (dt4.Rows[b][2].ToString() == "FIM DE JORNADA")
                                            //{
                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                            //}
                                            //else
                                            //{
                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                            //    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");

                                            //}
                                        }


                                    }
                                    //FIM
                                    #endregion

                                    #region INICIO DE VIAGEM
                                    //BLOCO REINICIO / INICIO DE VIAGEM
                                    if (dt4.Rows[w][2].ToString() == "INICIO DE VIAGEM")
                                    {
                                        int a = 0;
                                        a = w - 1;
                                        if (a >= 0)
                                        {
                                            //if (dt4.Rows[a][2].ToString() == "PARADA INTERNA")
                                            //{
                                            //    if (dt4.Rows[a][5].ToString() == "14")
                                            //    {
                                            //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                            //        write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");

                                            //    }
                                            //    else
                                            //        if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2")
                                            //    {
                                            //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                            //        write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");

                                            //    }
                                            //    else
                                            //    {
                                            //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");
                                            //        write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                            //    }
                                            //}
                                            //if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[a][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[a][2].ToString() == "INICIO JORNADA")
                                            //{
                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                            //}
                                            //else if (dt4.Rows[a][2].ToString() == "PARADA PERNOITE")
                                            //{
                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;22");
                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                            //}
                                            //else if (dt4.Rows[a][2].ToString() == "FIM DE VIAGEM")
                                            //{
                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;94");
                                            //    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                            //}

                                            if (dt4.Rows[a][2].ToString() == "PARADA CLIENTE / FORNECEDOR")
                                            {


                                                if (dt4.Rows[a][5].ToString() == "14")
                                                {

                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0193");
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                    marcacoes.Add(hora);



                                                }
                                                //else if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "1" || dt4.Rows[a][5].ToString() == "2" || dt4.Rows[a][5].ToString() == "4")
                                                //{
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                //}



                                            }
                                            else if (dt4.Rows[a][2].ToString() == "PARADA")
                                            {


                                                if (dt4.Rows[a][5].ToString() == "14")
                                                {

                                                    // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0193");
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                    marcacoes.Add(hora);
                                                }
                                                //else
                                                //{
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");



                                                //}



                                            }
                                            //else if (dt4.Rows[a][2].ToString() == "RETORNO REFEICAO")
                                            //{
                                            //    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0293");
                                            //    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                            //    write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");

                                            //}

                                        }
                                        //else
                                        //{
                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                        //}



                                    }
                                    //FIM
                                    #endregion

                                    #region PARADA CLIENTE / FORNECEDOR
                                    if (dt4.Rows[w][2].ToString() == "PARADA CLIENTE / FORNECEDOR")
                                    {
                                        if (dt4.Rows[w][5].ToString() == "3" || dt4.Rows[w][5].ToString() == "2" || dt4.Rows[w][5].ToString() == "4")
                                        {
                                            int a = 0;
                                            a = w - 1;
                                            if (a >= 0)
                                            {
                                                //if (dt4.Rows[a][2].ToString() == "INICIO DE VIAGEM" || dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM" || dt4.Rows[a][2].ToString() == "FIM DE VIAGEM")
                                                //{
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;94");
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");
                                                //}
                                                if (dt4.Rows[a][2].ToString() == "PARADA CLIENTE / FORNECEDOR")
                                                {
                                                    if (dt4.Rows[a][5].ToString() == "14")
                                                    {
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0293");
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;0102");
                                                        //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");
                                                        marcacoes.Add(hora);


                                                    }
                                                    //else if (dt4.Rows[a][5].ToString() == "15")
                                                    //{
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;94");
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");

                                                    //}
                                                    //else
                                                    //{
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");
                                                    //}

                                                }
                                            }

                                        }
                                        else if (dt4.Rows[w][5].ToString() == "14")
                                        {
                                            int a = 0;
                                            a = w - 1;
                                            if (a >= 0)
                                            {
                                                if (dt4.Rows[a][2].ToString() == "INICIO DE VIAGEM" || dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM" || dt4.Rows[a][2].ToString() == "FIM DE VIAGEM")
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                    // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0193");
                                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                    marcacoes.Add(hora);
                                                }
                                                else if (dt4.Rows[a][2].ToString() == "PARADA CLIENTE / FORNECEDOR")
                                                {
                                                    if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2" || dt4.Rows[a][5].ToString() == "4")
                                                    {
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0193");
                                                        //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                        marcacoes.Add(hora);
                                                    }
                                                    else if (dt4.Rows[a][5].ToString() == "14")
                                                    {
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0193");
                                                        marcacoes.Add(hora);

                                                    }


                                                }
                                                else
                                                {
                                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                    marcacoes.Add(hora);
                                                }

                                            }


                                        }


                                    }
                                    #endregion

                                    #region REINICIO
                                    //BLOCO REINICIO / INICIO DE VIAGEM
                                    if (dt4.Rows[w][2].ToString() == "REINICIO DE VIAGEM")
                                    {
                                        int a = 0;
                                        a = w - 1;
                                        if (a >= 0)
                                        {
                                            if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[a][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[a][2].ToString() == "INICIO JORNADA")
                                            {

                                            }
                                            else if (dt4.Rows[a][2].ToString() == "PARADA PERNOITE")
                                            {
                                                //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01020000");
                                                marcacoes.Add(hora);
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                            }

                                            else if (dt4.Rows[a][2].ToString() == "PARADA CLIENTE / FORNECEDOR")
                                            {

                                                if (a >= 0)
                                                {

                                                    if (dt4.Rows[a][2].ToString() == "PARADA CLIENTE / FORNECEDOR")
                                                    {
                                                        //if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2")
                                                        //{
                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                        //}
                                                        if (dt4.Rows[a][5].ToString() == "14")
                                                        {

                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0293");
                                                            marcacoes.Add(hora);
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0102");

                                                        }
                                                        //else if (dt4.Rows[a][5].ToString() == "15")
                                                        //{

                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");

                                                        //}
                                                        //else
                                                        //{
                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                        //}
                                                    }
                                                    //else if (dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                                    //{
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                    //}


                                                }



                                            }
                                            else if (dt4.Rows[a][2].ToString() == "PARADA")
                                            {
                                                if (dt4.Rows[a][5].ToString() == "14")
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0293");
                                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0102");
                                                    marcacoes.Add(hora);
                                                }
                                                //else
                                                //{
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                //}




                                            }
                                            //else if (dt4.Rows[a][2].ToString() == "OFICINA / MANUTENCAO")
                                            //{

                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");
                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");


                                            //}

                                        }
                                        else
                                        {
                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0102");
                                            marcacoes.Add(hora);
                                            // write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                        }



                                    }
                                    //FIM
                                    #endregion

                                    #region BLOCO PARADA
                                    // BLOCO PARADA
                                    if (dt4.Rows[w][2].ToString() == "PARADA")
                                    {
                                        int a = 0;
                                        a = w - 1;


                                        if (dt4.Rows[w][5].ToString() == "14")
                                        {
                                            if (dt4.Rows.Count >= a)
                                            {
                                                if (dt4.Rows[a][2].ToString() == "INICIO DE VIAGEM" || dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                                {
                                                    // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0193");
                                                    marcacoes.Add(hora);


                                                }

                                            }
                                        }
                                        //else
                                        //{
                                        //    if (dt4.Rows.Count >= a)
                                        //    {
                                        //        if (dt4.Rows[a][2].ToString() == "INICIO DE VIAGEM" || dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                        //        {
                                        //            write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                        //            //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;95");

                                        //        }
                                        //        //else
                                        //        //{
                                        //        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                        //        //    // write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                        //        //}
                                        //    }
                                        //}



                                        //  write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");

                                    }
                                    //FIM
                                    #endregion

                                    #region FIM DE VIAGEM
                                    //BLOCO FIM DE VIAGEM
                                    if (dt4.Rows[w][2].ToString() == "FIM DE VIAGEM")
                                    {
                                        int a = 0;
                                        int g = 0;
                                        a = w - 1;
                                        g = w - 2;
                                        if (a >= 0)
                                        {
                                            if (dt4.Rows[a][2].ToString() == "PARADA CLIENTE / FORNECEDOR")
                                            {


                                                if (dt4.Rows[a][5].ToString() == "14")
                                                {

                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0293");
                                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;0102");
                                                    marcacoes.Add(hora);

                                                }

                                                //else if (dt4.Rows[a][5].ToString() == "14")
                                                //{
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                                //    // write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                //}

                                                //else
                                                //{
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                //    // write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                //}



                                            }
                                            else if (dt4.Rows[a][2].ToString() == "PARADA")
                                            {


                                                if (dt4.Rows[a][5].ToString() == "14")
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0293");
                                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");
                                                    marcacoes.Add(hora);
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;0102");

                                                }

                                                //else
                                                //{
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                //    // write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");

                                                //}


                                            }

                                            else if (dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                            {
                                                if (g >= 0)
                                                {
                                                    //if (dt4.Rows[g][2].ToString() == "PARADA PERNOITE")
                                                    //{
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                    //    write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                    //}
                                                    //else
                                                    //{
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                    //}
                                                }
                                                //else
                                                //{
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                //}


                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                            }


                                        }
                                        //else
                                        //{
                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                        //}

                                    }

                                    //
                                    #endregion

                                    #region FIM DE JORNADA
                                    //BLOCO FIM DE JORMADA
                                    if (c > 0)
                                    {
                                        if (dt4.Rows[w][2].ToString() == "FIM DE JORNADA" && dt4.Rows[w][6].ToString() == "725405" || dt4.Rows[w][2].ToString() == "FIM DE JORNADA" && dt4.Rows[w][6].ToString() == "725407" || dt4.Rows[w][2].ToString() == "FIM DE JORNADA" && dt4.Rows[w][6].ToString() == "184525" && dt4.Rows[w][6].ToString() == "184522")
                                        {
                                            int a = 0;
                                            a = w - 1;

                                            if (a >= 0)
                                            {
                                                if (dt4.Rows[a][2].ToString() == "RETORNO REFEICAO")
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02020000");
                                                    //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                    marcacoes.Add(hora);

                                                }
                                                else if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA")
                                                {

                                                    //WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02020000");
                                                    marcacoes.Add(hora);


                                                }
                                                else if (dt4.Rows[a][2].ToString() == "RETORNO REFEICAO")
                                                {

                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02020000");
                                                    marcacoes.Add(hora);

                                                }
                                                else if (dt4.Rows[a][2].ToString() == "FIM DE VIAGEM")
                                                {
                                                    int b = 0;
                                                    b = a - 1;

                                                    if (b >= 0)
                                                    {
                                                        if (dt4.Rows[b][2].ToString() == "INICIO DE VIAGEM" || dt4.Rows[b][2].ToString() == "REINICIO DE VIAGEM")
                                                        {
                                                            int d = 0;
                                                            d = b - 1;
                                                            if (d >= 0)
                                                            {

                                                                if (dt4.Rows[d][2].ToString() == "PARADA")
                                                                {
                                                                    if (dt4.Rows[d][5].ToString() == "14")
                                                                    {
                                                                        // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                        //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02020000");
                                                                        //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                                        marcacoes.Add(hora);
                                                                    }
                                                                    else
                                                                    {
                                                                        // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                        //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02020000");
                                                                        // write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                                        marcacoes.Add(hora);
                                                                    }




                                                                }
                                                                else
                                                                {
                                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02020000");
                                                                    // write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                                    marcacoes.Add(hora);
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02020000");
                                                            // write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                            marcacoes.Add(hora);
                                                        }

                                                    }
                                                    else
                                                    {
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                        marcacoes.Add(hora);
                                                        //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");

                                                    }

                                                }
                                                else if (dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02020000");
                                                    //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                    marcacoes.Add(hora);
                                                }
                                                else if (dt4.Rows[a][2].ToString() == "PARADA")
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                    marcacoes.Add(hora);
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                }
                                                else if (dt4.Rows[a][2].ToString() == "PARADA INTERNA")
                                                {
                                                    if (dt4.Rows[a][5].ToString() == "14")
                                                    {
                                                        //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");
                                                        marcacoes.Add(hora);
                                                    }
                                                    else
                                                    {
                                                        marcacoes.Add(hora);
                                                    }
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");

                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                }
                                                else if (dt4.Rows[a][2].ToString() == "FIM DE JORNADA")
                                                {
                                                    int m = 0;
                                                    m = a - 1;

                                                    if (m >= 0)
                                                    {
                                                        if (dt4.Rows[m][2].ToString() == "RETORNO REFEICAO" || dt4.Rows[m][2].ToString() == "INICIO DE JORNADA")
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                            marcacoes.Add(hora);
                                                        }
                                                        else if (dt4.Rows[m][2].ToString() == "FIM DE VIAGEM")
                                                        {
                                                            int b = 0;
                                                            b = m - 1;

                                                            if (b >= 0)
                                                            {
                                                                if (dt4.Rows[b][2].ToString() == "INICIO DE VIAGEM" || dt4.Rows[b][2].ToString() == "REINICIO DE VIAGEM")
                                                                {
                                                                    int d = 0;
                                                                    d = b - 1;
                                                                    if (d >= 0)
                                                                    {

                                                                        if (dt4.Rows[d][2].ToString() == "PARADA")
                                                                        {
                                                                            if (dt4.Rows[d][5].ToString() == "14")
                                                                            {
                                                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                                marcacoes.Add(hora);
                                                                                //.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                                            }
                                                                            else
                                                                            {
                                                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                                marcacoes.Add(hora);
                                                                                //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");

                                                                            }


                                                                        }
                                                                        else
                                                                        {
                                                                            // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                            marcacoes.Add(hora);
                                                                            //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                    marcacoes.Add(hora);
                                                                    //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                                }

                                                            }


                                                        }
                                                        //else if (dt4.Rows[m][2].ToString() == "REINICIO DE VIAGEM")
                                                        //{
                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;94");
                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                        //}
                                                        //else if (dt4.Rows[m][2].ToString() == "PARADA INTERNA")
                                                        //{
                                                        //    if (dt4.Rows[m][5].ToString() == "3" || dt4.Rows[m][5].ToString() == "1" || dt4.Rows[m][5].ToString() == "2")
                                                        //    {
                                                        //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");

                                                        //    }
                                                        //    else if (dt4.Rows[m][5].ToString() == "22")
                                                        //    {
                                                        //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");
                                                        //    }

                                                        //    else if (dt4.Rows[m][2].ToString() == "FIM DE JORNADA")
                                                        //    {
                                                        //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                        //        write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                        //    }

                                                        //}
                                                        else if (dt4.Rows[m][2].ToString() == "PARADA")
                                                        {
                                                            // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                            marcacoes.Add(hora);
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                        }
                                                        //else if (dt4.Rows[m][2].ToString() == "PARADA INTERNA")
                                                        //{
                                                        //    if (dt4.Rows[m][5].ToString() != "3" || dt4.Rows[m][5].ToString() != "1" || dt4.Rows[m][5].ToString() != "2")
                                                        //    {
                                                        //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");
                                                        //        write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                        //        write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");

                                                        //    }

                                                        //}

                                                        else
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                            marcacoes.Add(hora);
                                                            //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                        }


                                                    }
                                                    else
                                                    {
                                                        //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                    }
                                                }
                                                else
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                    marcacoes.Add(hora);
                                                    //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                }


                                            }
                                            else
                                            {
                                                // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                marcacoes.Add(hora);
                                                //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                            }
                                        }

                                    }
                                    else
                                    {
                                        if (dt4.Rows[w][2].ToString() == "FIM DE JORNADA")
                                        {
                                            int q = 0;
                                            q = w + 1;

                                            if (q < dt4.Rows.Count)
                                            {
                                                if (dt4.Rows[q][2].ToString() == "FIM DE JORNADA")
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                    marcacoes.Add(hora);
                                                    //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");

                                                }
                                                else
                                                {
                                                    int a = 0;
                                                    a = w - 1;

                                                    if (a >= 0)
                                                    {

                                                        if (dt4.Rows[a][2].ToString() == "FIM DE VIAGEM" || dt4.Rows[a][2].ToString() == "RETORNO REFEICAO" || dt4.Rows[a][2].ToString() == "INICIO DE JORNADA")
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                            marcacoes.Add(hora);
                                                            // write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");

                                                        }
                                                        //else if (dt4.Rows[a][2].ToString() == "PARADA INTERNA")
                                                        //{
                                                        //    if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2")
                                                        //    {
                                                        //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                        //        // write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                        //        // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                        //    }
                                                        //    else if (dt4.Rows[a][5].ToString() == "14")
                                                        //    {
                                                        //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                        //        write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                        //    }
                                                        //    else
                                                        //    {
                                                        //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");


                                                        //    }
                                                        //}
                                                        else if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[a][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[a][2].ToString() == "INICIO JORNADA")
                                                        {

                                                        }
                                                        else if (dt4.Rows[a][2].ToString() == "FIM DE JORNADA")
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                            marcacoes.Add(hora);
                                                            //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                        }
                                                        else
                                                        {
                                                            // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                            marcacoes.Add(hora);
                                                            // write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                        }
                                                    }
                                                }

                                            }
                                            else
                                            {
                                                int a = 0;
                                                a = w - 1;

                                                if (a >= 0)
                                                {

                                                    if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA")
                                                    {
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                        marcacoes.Add(hora);

                                                    }
                                                    else if (dt4.Rows[a][2].ToString() == "FIM DE VIAGEM" || dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                                    {
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                        marcacoes.Add(hora);
                                                        //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");

                                                    }
                                                    else if (dt4.Rows[a][2].ToString() == "RETORNO REFEICAO")
                                                    {
                                                        //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                        marcacoes.Add(hora);

                                                    }
                                                    else if (dt4.Rows[a][2].ToString() == "PARADA INTERNA")
                                                    {
                                                        if (dt4.Rows[a][5].ToString() == "14")
                                                        {
                                                            marcacoes.Add(hora);
                                                        }
                                                        else
                                                        {
                                                            marcacoes.Add(hora);
                                                        }
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");

                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                    }
                                                    //else if (dt4.Rows[a][2].ToString() == "PARADA INTERNA")
                                                    //{
                                                    //    if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2")
                                                    //    {
                                                    //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                    //        // write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                    //        // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                    //    }
                                                    //    else if (dt4.Rows[a][5].ToString() == "14")
                                                    //    {
                                                    //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                    //        write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                    //    }
                                                    //    else
                                                    //    {
                                                    //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");

                                                    //    }
                                                    //}
                                                    else if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[a][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[a][2].ToString() == "INICIO JORNADA")
                                                    {

                                                    }
                                                    else if (dt4.Rows[a][2].ToString() == "FIM DE JORNADA")
                                                    {
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                        marcacoes.Add(hora);

                                                        //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                    }
                                                    else
                                                    {
                                                        // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                        //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                    }
                                                }
                                                else
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                    marcacoes.Add(hora);
                                                    //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                }
                                            }
                                        }





                                    }


                                    #endregion

                                    #region PARADA PERNOITE

                                    if (dt4.Rows[w][2].ToString() == "PARADA PERNOITE")
                                    {
                                        int a = 0;
                                        a = w - 1;


                                        if (a >= 0)
                                        {
                                            if (dt4.Rows[a][2].ToString() == "PARADA CLIENTE / FORNECEDOR")
                                            {


                                                if (dt4.Rows[a][5].ToString() == "14")
                                                {

                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0293");
                                                    marcacoes.Add(hora);
                                                    //WriteLine(cracha + ";" + data + ";" + hora + ";1;0102");
                                                    //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;0102");
                                                    marcacoes.Add(DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm").Replace(":", ""));



                                                }
                                                else if (dt4.Rows[a][5].ToString() == "15")
                                                {

                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                    marcacoes.Add(hora);
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;22");


                                                }
                                                //else if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2")
                                                //{
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                //    write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;22");
                                                //    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                //}
                                                else
                                                {
                                                    marcacoes.Add(hora);
                                                }



                                            }
                                            else if (dt4.Rows[a][2].ToString() == "PARADA")
                                            {
                                                //if (dt4.Rows[a][5].ToString() == "7")
                                                //{
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                //    write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;22");
                                                //}
                                                if (dt4.Rows[a][5].ToString() == "14")
                                                {

                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0293");
                                                    marcacoes.Add(hora);
                                                    //WriteLine(cracha + ";" + data + ";" + hora + ";1;0102");
                                                    //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;0102");
                                                    marcacoes.Add(DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm").Replace(":", ""));


                                                }
                                                else
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                    marcacoes.Add(hora);
                                                    //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                }

                                            }
                                            else if (dt4.Rows[a][2].ToString() == "INICIO DE VIAGEM")
                                            {
                                                marcacoes.Add(hora);
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";2;22");
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";3;22");
                                            }
                                            else if (dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                            {
                                                marcacoes.Add(hora);
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");

                                                // write.WriteLine(cracha + ";" + data + ";" + hora + ";3;22");
                                            }
                                            else if (dt4.Rows[a][2].ToString() == "RETORNO REFEICAO")
                                            {
                                                marcacoes.Add(hora);
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");

                                                // write.WriteLine(cracha + ";" + data + ";" + hora + ";3;22");
                                            }
                                            //else if (dt4.Rows[a][2].ToString() == "PARADA INTERNA")
                                            //{


                                            //    if (dt4.Rows[a][5].ToString() == "14")
                                            //    {

                                            //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                            //        //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;22");


                                            //    }
                                            //    //else if (dt4.Rows[a][5].ToString() == "15")
                                            //    //{

                                            //    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                            //    //    write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;22");


                                            //    //}
                                            //    else if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2")
                                            //    {
                                            //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                            //        //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;22");
                                            //        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                            //    }
                                            //    //else
                                            //    //{
                                            //    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;22");
                                            //    //}



                                            //}
                                            //else
                                            //{
                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;22");
                                            //}

                                        }
                                        //else
                                        //{
                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;22");
                                        //    write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;22");

                                        //}

                                    }

                                    #endregion

                                    #region RETORNO PERNOITE
                                    //BLOCO REINICIO / INICIO DE VIAGEM
                                    if (dt4.Rows[w][2].ToString() == "RETORNO PERNOITE")
                                    {
                                        int a = 0;
                                        a = w - 1;
                                        if (a >= 0)
                                        {
                                            if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[a][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[a][2].ToString() == "INICIO JORNADA")
                                            {

                                            }
                                            else if (dt4.Rows[a][2].ToString() == "PARADA PERNOITE")
                                            {
                                                //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01020000");
                                                marcacoes.Add(hora);
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                            }


                                        }
                                        else
                                        {
                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0102");
                                            marcacoes.Add(hora);
                                            // write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                        }



                                    }
                                    //FIM
                                    #endregion



                                }

                                //FIM

                                #endregion
                            }
                            else
                            {

                            }

                        }
                        else
                        {
                            //JORNADA PERNOITE COM DIFERENÇA NO COMEÇO
                            string sqli = "select TOP 1  hr_posicao from tb_parada  where  cod_cracha=" + lblCod.Text + " and fl_deletado is null and dt_posicao_parada='" + DateTime.Parse(dte.Rows[f][0].ToString()).ToString("yyyy-MM-dd") + "'  and hr_posicao > (select top 1 hr_posicao from tb_parada where cod_cracha=" + lblCod.Text + " and fl_deletado is null and dt_posicao_parada='" + DateTime.Parse(dte.Rows[f][0].ToString()).ToString("yyyy-MM-dd") + "'  and ds_macro='FIM DE JORNADA'  OR cod_cracha=" + lblCod.Text + " and fl_deletado is null and dt_posicao_parada='" + DateTime.Parse(dte.Rows[f][0].ToString()).ToString("yyyy-MM-dd") + "'  and ds_macro='PARADA PERNOITE' order by hr_posicao) order by hr_posicao ";
                            SqlDataAdapter adtpi = new SqlDataAdapter(sqli, con);
                            DataTable dti = new DataTable();
                            con.Open();
                            adtpi.Fill(dti);
                            con.Close();

                            string sqlf = "select top 1 hr_posicao from tb_parada where cod_cracha=" + lblCod.Text + " and ds_macro = 'FIM DE JORNADA' and dt_posicao_parada='" + DateTime.Parse(dte.Rows[f][0].ToString()).AddDays(1).ToString("yyyy-MM-dd") + "' and fl_deletado is null OR cod_cracha=" + lblCod.Text + " and ds_macro='PARADA PERNOITE' and dt_posicao_parada='" + DateTime.Parse(dte.Rows[f][0].ToString()).AddDays(1).ToString("yyyy-MM-dd") + "' and fl_deletado is null order by hr_posicao";
                            SqlDataAdapter adtpf = new SqlDataAdapter(sqlf, con);
                            DataTable dtf = new DataTable();
                            con.Open();
                            adtpf.Fill(dtf);
                            con.Close();



                            if (dti.Rows.Count > 0 && dtf.Rows.Count > 0)
                            {
                                dtRow["Dia"] = DateTime.Parse(dte.Rows[f][0].ToString()).ToString("dd");
                                #region Jornada Noturna


                                //sql4 = "select dt_posicao_parada, hr_posicao, ds_macro,cod_transmissao, cod_cracha, isnull(cod_ref_parada,0) as cod_ref_parada, cod_idveiculo from tb_parada where fl_deletado is null and cod_cracha=" + lblCod.Text + " and dt_posicao_parada='" + txtDtInicial.Text + "' order by dt_posicao_parada, hr_posicao";
                                sql4 = "select dt_posicao_parada, hr_posicao, ds_macro,cod_transmissao, cod_cracha, isnull(cod_ref_parada,0) as cod_ref_parada, cod_idveiculo from tb_parada where fl_deletado is null and cod_cracha=" + lblCod.Text + " and dt_posicao_parada='" + DateTime.Parse(dte.Rows[f][0].ToString()).ToString("yyyy-MM-dd") + "' and hr_posicao >='" + dti.Rows[0][0].ToString() + "' or cod_cracha=" + lblCod.Text + " and dt_posicao_parada='" + DateTime.Parse(dte.Rows[f][0].ToString()).AddDays(1).ToString("yyyy-MM-dd") + "' and hr_posicao <= '" + dtf.Rows[0][0].ToString() + "' AND (ds_macro IN ('INICIO DE JORNADA', 'INICIO JORNADA CAMINHAO', 'PARADA REFEICAO', 'FIM DE JORNADA','REINICIO DE VIAGEM','FIM DE VIAGEM', 'PARADA','INICIO DE VIAGEM','PARADA INTERNA','RETORNO REFEICAO','PARADA CLIENTE / FORNECEDOR','PARADA PERNOITE','RETORNO PERNOITE') OR cod_ref_parada in (2,3,4,14,22)) AND fl_deletado IS NULL ORDER BY dt_posicao_parada, hr_posicao";
                                // string sql4 = "select dt_posicao_parada, hr_posicao, ds_macro,cod_transmissao, cod_cracha, isnull(cod_ref_parada,0) as cod_ref_parada, cod_idveiculo from tb_parada where fl_deletado is null and cod_cracha=" + lblCod.Text + " and dt_posicao_parada between '" + txtDtInicial.Text + "' and '" + txtDtFinal.Text + "' order by dt_posicao_parada, hr_posicao";
                                SqlDataAdapter adtp4 = new SqlDataAdapter(sql4, con);
                                DataTable dt4 = new DataTable();

                                try
                                {
                                    con.Open();
                                    adtp4.Fill(dt4);
                                    con.Close();
                                }
                                catch (Exception ed)
                                {
                                    //MessageBox.Show(ed.ToString(), "Erro3");
                                }

                                c = 0;

                                for (int y = 0; y < dt4.Rows.Count; y++)
                                {
                                    if (dt4.Rows[y][2].ToString() == "FIM DE JORNADA" && dt4.Rows[y][6].ToString() == "725405" || dt4.Rows[y][2].ToString() == "FIM DE JORNADA" && dt4.Rows[y][6].ToString() == "725407" || dt4.Rows[y][2].ToString() == "FIM DE JORNADA" && dt4.Rows[y][6].ToString() == "184525")
                                    {
                                        c = c + 1;
                                    }

                                }
                                string caminho = Server.MapPath("/TXT/");

                                string filepath = caminho + arquivo;

                                FileInfo file = new FileInfo(filepath);
                                //if(file.Exists)
                                //{
                                //    file.Delete();
                                //}



                                for (int w = 0; w < dt4.Rows.Count; w++)
                                {

                                    string codigo = lblCod.Text;
                                    string[] codigosEspeciais = { "330", "482", "820", "958", "989" };

                                    if (codigo.StartsWith("312"))
                                    {
                                        cracha = codigo.Insert(3, "0");
                                    }
                                    else if ((codigosEspeciais.Contains(codigo) || codigo.Length <= 4))
                                    {
                                        cracha = "30" + codigo;
                                    }
                                    else if (codigo.Length <= 3)
                                    {
                                        cracha = "300" + codigo;
                                    }

                                    else
                                    {
                                        cracha = codigo;
                                    }

                                    // cracha = "30" + lblCod.Text;
                                    int t = 0;
                                    t = w - 1;
                                    string data = DateTime.Parse(dt4.Rows[w][0].ToString()).ToString("dd/MM");
                                    string horas = DateTime.Parse(dt4.Rows[w][1].ToString()).ToString("HH:mm");
                                    if (t >= 0)
                                    {
                                        if (DateTime.Parse(dt4.Rows[w][1].ToString()).ToString("HH:mm") == DateTime.Parse(dt4.Rows[t][1].ToString()).ToString("HH:mm"))
                                        {
                                            hora = DateTime.Parse(horas).AddMinutes(1).ToString("HH:mm");
                                        }
                                        else
                                        {
                                            hora = DateTime.Parse(horas).ToString("HH:mm");
                                        }
                                    }
                                    else
                                    {
                                        hora = DateTime.Parse(horas).ToString("HH:mm");
                                    }
                                    //tipo = dt4.Rows[w][2].ToString();
                                    string transmissao = dt4.Rows[w][3].ToString();


                                    //if (dt4.Rows[w][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[w][2].ToString() == "TRECHO ALMOÇO" || dt4.Rows[w][2].ToString() == "FIM DE JORNADA")
                                    //{
                                    //    tipo = "2";
                                    //}
                                    //else  if (dt4.Rows[w][2].ToString() == "PARADA FORNECEDOR" || dt4.Rows[w][2].ToString() == "REINICIO DE VIAGEM")
                                    //{
                                    //    tipo = "21";
                                    //}





                                    #region BLOCO INICIO JORNADA
                                    //BLOCO INICIO JORNADA
                                    if (dt4.Rows[w][2].ToString() == "INICIO DE JORNADA")
                                    {
                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0102");
                                        //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01020000");
                                        marcacoes.Add(hora);



                                    }

                                    if (dt4.Rows[w][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[w][2].ToString() == "INICIO JORNADA")
                                    {

                                        int a = 0;
                                        a = w - 1;
                                        if (a >= 0)
                                        {
                                            if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA")
                                            {
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");

                                            }
                                            //else
                                            //{
                                            //	//write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0102");
                                            //	//write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01020000");
                                            //	marcacoes.Add(hora);



                                            //}
                                        }
                                        else
                                        {
                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                        }


                                    }
                                    //FIM
                                    #endregion

                                    #region BLOCO INICIO JORNADA CAMINHAO
                                    //BLOCO INICIO JORNADA CAMINHAO

                                    if (dt4.Rows[w][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[w][2].ToString() == "INICIO JORNADA")
                                    {

                                        int a = 0;
                                        a = w - 1;
                                        if (a >= 0)
                                        {
                                            if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA")
                                            {
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");

                                            }
                                            else
                                            {
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0102");
                                                //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01020000");
                                                marcacoes.Add(hora);
                                            }
                                        }
                                        else
                                        {
                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                        }


                                    }
                                    //FIM
                                    #endregion

                                    #region PARADA INTERNA
                                    //BLOCO PARADA INTERNA
                                    if (dt4.Rows[w][2].ToString() == "PARADA INTERNA")
                                    {
                                        if (dt4.Rows[w][5].ToString() == "3" || dt4.Rows[w][5].ToString() == "2")
                                        {
                                            int a = 0;
                                            a = w - 1;
                                            if (a >= 0)
                                            {
                                                if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[a][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[a][2].ToString() == "INICIO JORNADA")
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");
                                                }
                                                else if (dt4.Rows[a][5].ToString() == "14")
                                                {
                                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");
                                                    marcacoes.Add(hora);
                                                }
                                                else if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2" || dt4.Rows[a][5].ToString() == "1")
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                }
                                                else if (dt4.Rows[a][2].ToString() == "PARADA REFEICAO")
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");
                                                }
                                                else if (dt4.Rows[a][2].ToString() == "INICIO DE VIAGEM")
                                                {
                                                    //////write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                    ////write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                    ////write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");
                                                }
                                                else if (dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;94");
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");
                                                }
                                                else if (dt4.Rows[a][2].ToString() == "FIM DE VIAGEM")
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                }
                                                else
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");
                                                }

                                            }

                                        }
                                        else if (dt4.Rows[w][5].ToString() == "14")
                                        {
                                            int a = 0;
                                            a = w - 1;
                                            if (a >= 0)
                                            {
                                                if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[a][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[a][2].ToString() == "INICIO JORNADA")
                                                {
                                                    marcacoes.Add(hora);
                                                }
                                                if (dt4.Rows[a][5].ToString() == "14")
                                                {
                                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                    marcacoes.Add(hora);
                                                }
                                                else if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2" || dt4.Rows[a][5].ToString() == "1")
                                                {
                                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                    marcacoes.Add(hora);
                                                }
                                                else if (dt4.Rows[a][2].ToString() == "PARADA REFEICAO")
                                                {
                                                    // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");
                                                }
                                                else if (dt4.Rows[a][2].ToString() == "INICIO DE VIAGEM" || dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                                {
                                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                    marcacoes.Add(hora);
                                                }
                                                else if (dt4.Rows[a][2].ToString() == "FIM DE VIAGEM")
                                                {

                                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                    marcacoes.Add(hora);

                                                }
                                                if (dt4.Rows[a][5].ToString() == "22")
                                                {
                                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                    marcacoes.Add(hora);
                                                }
                                                //else
                                                //{
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93: 71489");
                                                //    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                                //}

                                            }
                                        }
                                        else if (dt4.Rows[w][5].ToString() == "22")
                                        {
                                            int a = 0;
                                            a = w - 1;
                                            if (a >= 0)
                                            {

                                                if (dt4.Rows[a][5].ToString() == "14")
                                                {
                                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");
                                                    marcacoes.Add(hora);
                                                }


                                            }
                                        }
                                        else
                                        {
                                            int a = 0;
                                            a = w - 1;
                                            if (a >= 0)
                                            {
                                                if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[a][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[a][2].ToString() == "INICIO JORNADA")
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;95");
                                                }
                                                if (dt4.Rows[a][5].ToString() == "14")
                                                {
                                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");
                                                    marcacoes.Add(hora);

                                                }

                                                else if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2" || dt4.Rows[a][5].ToString() == "1")
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;95");
                                                }
                                                else if (dt4.Rows[a][2].ToString() == "PARADA REFEICAO")
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                                    //// write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");
                                                }
                                                else if (dt4.Rows[a][2].ToString() == "INICIO DE VIAGEM" || dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                                {

                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;94");
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;95");
                                                }
                                                else if (dt4.Rows[a][2].ToString() == "FIM DE VIAGEM" || dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                                {

                                                    //// write.WriteLine(cracha + ";" + data + ";" + hora + ";1;94");
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;95");

                                                }

                                            }
                                        }

                                    }
                                    //FIM 
                                    #endregion

                                    #region BLOCO PARADA REFEIÇÃO
                                    //BLOCO PARADA REFEIÇÃO
                                    if (dt4.Rows[w][2].ToString() == "PARADA REFEICAO")
                                    {
                                        int a = 0;
                                        a = w - 1;
                                        if (a >= 0)
                                        {
                                            //if (dt4.Rows[a][2].ToString() == "PARADA INTERNA")
                                            //{

                                            //    if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2")
                                            //    {
                                            //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                            //        write.WriteLine(cracha + ";" + data + ";" + hora + ";2;93");
                                            //    }
                                            //    else
                                            //    {
                                            //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");
                                            //        write.WriteLine(cracha + ";" + data + ";" + hora + ";2;93");
                                            //    }
                                            //}
                                            if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[a][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[a][2].ToString() == "INICIO JORNADA")
                                            {
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0193");
                                                //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                marcacoes.Add(hora);

                                            }
                                            else
                                            {
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0193");
                                                //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                marcacoes.Add(hora);
                                            }

                                        }
                                        //else
                                        //{
                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");
                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;93");
                                        //}
                                    }
                                    //FIM
                                    #endregion

                                    #region RETORNO REFEIÇAO
                                    //BLOCO RETORNO REFEIÇAO
                                    if (dt4.Rows[w][2].ToString() == "RETORNO REFEICAO")
                                    {

                                        int a = 0;
                                        int b = 0;
                                        a = w - 1;
                                        b = w + 1;
                                        if (a >= 0)
                                        {

                                            if (dt4.Rows[a][2].ToString() == "PARADA REFEICAO")
                                            {

                                                // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0293");
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;0102");
                                                //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");
                                                marcacoes.Add(hora);

                                            }
                                            //if (dt4.Rows[b][2].ToString() == "FIM DE JORNADA")
                                            //{
                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                            //}
                                            //else
                                            //{
                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                            //    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");

                                            //}
                                        }


                                    }
                                    //FIM
                                    #endregion

                                    #region INICIO DE VIAGEM
                                    //BLOCO REINICIO / INICIO DE VIAGEM
                                    if (dt4.Rows[w][2].ToString() == "INICIO DE VIAGEM")
                                    {
                                        int a = 0;
                                        a = w - 1;
                                        if (a >= 0)
                                        {
                                            //if (dt4.Rows[a][2].ToString() == "PARADA INTERNA")
                                            //{
                                            //    if (dt4.Rows[a][5].ToString() == "14")
                                            //    {
                                            //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                            //        write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");

                                            //    }
                                            //    else
                                            //        if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2")
                                            //    {
                                            //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                            //        write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");

                                            //    }
                                            //    else
                                            //    {
                                            //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");
                                            //        write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                            //    }
                                            //}
                                            //if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[a][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[a][2].ToString() == "INICIO JORNADA")
                                            //{
                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                            //}
                                            //else if (dt4.Rows[a][2].ToString() == "PARADA PERNOITE")
                                            //{
                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;22");
                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                            //}
                                            //else if (dt4.Rows[a][2].ToString() == "FIM DE VIAGEM")
                                            //{
                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;94");
                                            //    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                            //}

                                            if (dt4.Rows[a][2].ToString() == "PARADA CLIENTE / FORNECEDOR")
                                            {


                                                if (dt4.Rows[a][5].ToString() == "14")
                                                {

                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0193");
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                    marcacoes.Add(hora);



                                                }
                                                //else if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "1" || dt4.Rows[a][5].ToString() == "2" || dt4.Rows[a][5].ToString() == "4")
                                                //{
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                //}



                                            }
                                            else if (dt4.Rows[a][2].ToString() == "PARADA")
                                            {


                                                if (dt4.Rows[a][5].ToString() == "14")
                                                {

                                                    // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0193");
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                    marcacoes.Add(hora);
                                                }
                                                //else
                                                //{
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");



                                                //}



                                            }
                                            //else if (dt4.Rows[a][2].ToString() == "RETORNO REFEICAO")
                                            //{
                                            //    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0293");
                                            //    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                            //    write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");

                                            //}

                                        }
                                        //else
                                        //{
                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                        //}



                                    }
                                    //FIM
                                    #endregion

                                    #region PARADA CLIENTE / FORNECEDOR
                                    if (dt4.Rows[w][2].ToString() == "PARADA CLIENTE / FORNECEDOR")
                                    {
                                        if (dt4.Rows[w][5].ToString() == "3" || dt4.Rows[w][5].ToString() == "2" || dt4.Rows[w][5].ToString() == "4")
                                        {
                                            int a = 0;
                                            a = w - 1;
                                            if (a >= 0)
                                            {
                                                //if (dt4.Rows[a][2].ToString() == "INICIO DE VIAGEM" || dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM" || dt4.Rows[a][2].ToString() == "FIM DE VIAGEM")
                                                //{
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;94");
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");
                                                //}
                                                if (dt4.Rows[a][2].ToString() == "PARADA CLIENTE / FORNECEDOR")
                                                {
                                                    if (dt4.Rows[a][5].ToString() == "14")
                                                    {
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0293");
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;0102");
                                                        //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");
                                                        marcacoes.Add(hora);


                                                    }
                                                    //else if (dt4.Rows[a][5].ToString() == "15")
                                                    //{
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;94");
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");

                                                    //}
                                                    //else
                                                    //{
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;21");
                                                    //}

                                                }
                                            }

                                        }
                                        else if (dt4.Rows[w][5].ToString() == "14")
                                        {
                                            int a = 0;
                                            a = w - 1;
                                            if (a >= 0)
                                            {
                                                if (dt4.Rows[a][2].ToString() == "INICIO DE VIAGEM" || dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM" || dt4.Rows[a][2].ToString() == "FIM DE VIAGEM")
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                    // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0193");
                                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                    marcacoes.Add(hora);
                                                }
                                                else if (dt4.Rows[a][2].ToString() == "PARADA CLIENTE / FORNECEDOR")
                                                {
                                                    if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2" || dt4.Rows[a][5].ToString() == "4")
                                                    {
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0193");
                                                        //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                        marcacoes.Add(hora);
                                                    }
                                                    else if (dt4.Rows[a][5].ToString() == "14")
                                                    {
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0193");
                                                        marcacoes.Add(hora);

                                                    }


                                                }
                                                else
                                                {
                                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01930000");
                                                    marcacoes.Add(hora);
                                                }

                                            }


                                        }


                                    }
                                    #endregion

                                    #region REINICIO
                                    //BLOCO REINICIO / INICIO DE VIAGEM
                                    if (dt4.Rows[w][2].ToString() == "REINICIO DE VIAGEM")
                                    {
                                        int a = 0;
                                        a = w - 1;
                                        if (a >= 0)
                                        {
                                            if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[a][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[a][2].ToString() == "INICIO JORNADA")
                                            {

                                            }
                                            else if (dt4.Rows[a][2].ToString() == "PARADA PERNOITE")
                                            {
                                                //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01020000");
                                                marcacoes.Add(hora);
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                            }

                                            else if (dt4.Rows[a][2].ToString() == "PARADA CLIENTE / FORNECEDOR")
                                            {

                                                if (a >= 0)
                                                {

                                                    if (dt4.Rows[a][2].ToString() == "PARADA CLIENTE / FORNECEDOR")
                                                    {
                                                        //if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2")
                                                        //{
                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                        //}
                                                        if (dt4.Rows[a][5].ToString() == "14")
                                                        {

                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0293");
                                                            marcacoes.Add(hora);
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0102");

                                                        }
                                                        //else if (dt4.Rows[a][5].ToString() == "15")
                                                        //{

                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");

                                                        //}
                                                        //else
                                                        //{
                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                        //}
                                                    }
                                                    //else if (dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                                    //{
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                    //}


                                                }



                                            }
                                            else if (dt4.Rows[a][2].ToString() == "PARADA")
                                            {
                                                if (dt4.Rows[a][5].ToString() == "14")
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0293");
                                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0102");
                                                    marcacoes.Add(hora);
                                                }
                                                //else
                                                //{
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                //}




                                            }
                                            //else if (dt4.Rows[a][2].ToString() == "OFICINA / MANUTENCAO")
                                            //{

                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");
                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");


                                            //}

                                        }
                                        else
                                        {
                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0102");
                                            marcacoes.Add(hora);
                                            // write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                        }



                                    }
                                    //FIM
                                    #endregion

                                    #region BLOCO PARADA
                                    // BLOCO PARADA
                                    if (dt4.Rows[w][2].ToString() == "PARADA")
                                    {
                                        int a = 0;
                                        a = w - 1;


                                        if (dt4.Rows[w][5].ToString() == "14")
                                        {
                                            if (dt4.Rows.Count >= a)
                                            {
                                                if (dt4.Rows[a][2].ToString() == "INICIO DE VIAGEM" || dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                                {
                                                    // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0193");
                                                    marcacoes.Add(hora);


                                                }

                                            }
                                        }
                                        //else
                                        //{
                                        //    if (dt4.Rows.Count >= a)
                                        //    {
                                        //        if (dt4.Rows[a][2].ToString() == "INICIO DE VIAGEM" || dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                        //        {
                                        //            write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                        //            //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;95");

                                        //        }
                                        //        //else
                                        //        //{
                                        //        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                        //        //    // write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                        //        //}
                                        //    }
                                        //}



                                        //  write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");

                                    }
                                    //FIM
                                    #endregion

                                    #region FIM DE VIAGEM
                                    //BLOCO FIM DE VIAGEM
                                    if (dt4.Rows[w][2].ToString() == "FIM DE VIAGEM")
                                    {
                                        int a = 0;
                                        int g = 0;
                                        a = w - 1;
                                        g = w - 2;
                                        if (a >= 0)
                                        {
                                            if (dt4.Rows[a][2].ToString() == "PARADA CLIENTE / FORNECEDOR")
                                            {


                                                if (dt4.Rows[a][5].ToString() == "14")
                                                {

                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0293");
                                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;0102");
                                                    marcacoes.Add(hora);

                                                }

                                                //else if (dt4.Rows[a][5].ToString() == "14")
                                                //{
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                                //    // write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                //}

                                                //else
                                                //{
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                //    // write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                                //}



                                            }
                                            else if (dt4.Rows[a][2].ToString() == "PARADA")
                                            {


                                                if (dt4.Rows[a][5].ToString() == "14")
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0293");
                                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");
                                                    marcacoes.Add(hora);
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;0102");

                                                }

                                                //else
                                                //{
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                //    // write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");

                                                //}


                                            }

                                            else if (dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                            {
                                                if (g >= 0)
                                                {
                                                    //if (dt4.Rows[g][2].ToString() == "PARADA PERNOITE")
                                                    //{
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                    //    write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                    //}
                                                    //else
                                                    //{
                                                    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                    //}
                                                }
                                                //else
                                                //{
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                //}


                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                            }


                                        }
                                        //else
                                        //{
                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                        //}

                                    }

                                    //
                                    #endregion

                                    #region FIM DE JORNADA
                                    //BLOCO FIM DE JORMADA
                                    if (c > 0)
                                    {
                                        if (dt4.Rows[w][2].ToString() == "FIM DE JORNADA" && dt4.Rows[w][6].ToString() == "725405" || dt4.Rows[w][2].ToString() == "FIM DE JORNADA" && dt4.Rows[w][6].ToString() == "725407" || dt4.Rows[w][2].ToString() == "FIM DE JORNADA" && dt4.Rows[w][6].ToString() == "184525" && dt4.Rows[w][6].ToString() == "184522")
                                        {
                                            int a = 0;
                                            a = w - 1;

                                            if (a >= 0)
                                            {
                                                if (dt4.Rows[a][2].ToString() == "RETORNO REFEICAO")
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02020000");
                                                    //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                    marcacoes.Add(hora);

                                                }
                                                else if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA")
                                                {

                                                    //WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02020000");
                                                    marcacoes.Add(hora);


                                                }
                                                else if (dt4.Rows[a][2].ToString() == "RETORNO REFEICAO")
                                                {

                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02020000");
                                                    marcacoes.Add(hora);

                                                }
                                                else if (dt4.Rows[a][2].ToString() == "FIM DE VIAGEM")
                                                {
                                                    int b = 0;
                                                    b = a - 1;

                                                    if (b >= 0)
                                                    {
                                                        if (dt4.Rows[b][2].ToString() == "INICIO DE VIAGEM" || dt4.Rows[b][2].ToString() == "REINICIO DE VIAGEM")
                                                        {
                                                            int d = 0;
                                                            d = b - 1;
                                                            if (d >= 0)
                                                            {

                                                                if (dt4.Rows[d][2].ToString() == "PARADA")
                                                                {
                                                                    if (dt4.Rows[d][5].ToString() == "14")
                                                                    {
                                                                        // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                        //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02020000");
                                                                        //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                                        marcacoes.Add(hora);
                                                                    }
                                                                    else
                                                                    {
                                                                        // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                        //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02020000");
                                                                        // write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                                        marcacoes.Add(hora);
                                                                    }




                                                                }
                                                                else
                                                                {
                                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02020000");
                                                                    // write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                                    marcacoes.Add(hora);
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                            //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02020000");
                                                            // write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                            marcacoes.Add(hora);
                                                        }

                                                    }
                                                    else
                                                    {
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                        marcacoes.Add(hora);
                                                        //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");

                                                    }

                                                }
                                                else if (dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                    //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02020000");
                                                    //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                    marcacoes.Add(hora);
                                                }
                                                else if (dt4.Rows[a][2].ToString() == "PARADA")
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                    marcacoes.Add(hora);
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                }
                                                else if (dt4.Rows[a][2].ToString() == "PARADA INTERNA")
                                                {
                                                    if (dt4.Rows[a][5].ToString() == "14")
                                                    {
                                                        //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "02930000");
                                                        marcacoes.Add(hora);
                                                    }
                                                    else
                                                    {
                                                        marcacoes.Add(hora);
                                                    }
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");

                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                }
                                                else if (dt4.Rows[a][2].ToString() == "FIM DE JORNADA")
                                                {
                                                    int m = 0;
                                                    m = a - 1;

                                                    if (m >= 0)
                                                    {
                                                        if (dt4.Rows[m][2].ToString() == "RETORNO REFEICAO" || dt4.Rows[m][2].ToString() == "INICIO DE JORNADA")
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                            marcacoes.Add(hora);
                                                        }
                                                        else if (dt4.Rows[m][2].ToString() == "FIM DE VIAGEM")
                                                        {
                                                            int b = 0;
                                                            b = m - 1;

                                                            if (b >= 0)
                                                            {
                                                                if (dt4.Rows[b][2].ToString() == "INICIO DE VIAGEM" || dt4.Rows[b][2].ToString() == "REINICIO DE VIAGEM")
                                                                {
                                                                    int d = 0;
                                                                    d = b - 1;
                                                                    if (d >= 0)
                                                                    {

                                                                        if (dt4.Rows[d][2].ToString() == "PARADA")
                                                                        {
                                                                            if (dt4.Rows[d][5].ToString() == "14")
                                                                            {
                                                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                                marcacoes.Add(hora);
                                                                                //.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                                            }
                                                                            else
                                                                            {
                                                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                                marcacoes.Add(hora);
                                                                                //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");

                                                                            }


                                                                        }
                                                                        else
                                                                        {
                                                                            // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                            marcacoes.Add(hora);
                                                                            //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                                    marcacoes.Add(hora);
                                                                    //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                                }

                                                            }


                                                        }
                                                        //else if (dt4.Rows[m][2].ToString() == "REINICIO DE VIAGEM")
                                                        //{
                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;94");
                                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                        //}
                                                        //else if (dt4.Rows[m][2].ToString() == "PARADA INTERNA")
                                                        //{
                                                        //    if (dt4.Rows[m][5].ToString() == "3" || dt4.Rows[m][5].ToString() == "1" || dt4.Rows[m][5].ToString() == "2")
                                                        //    {
                                                        //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");

                                                        //    }
                                                        //    else if (dt4.Rows[m][5].ToString() == "22")
                                                        //    {
                                                        //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");
                                                        //    }

                                                        //    else if (dt4.Rows[m][2].ToString() == "FIM DE JORNADA")
                                                        //    {
                                                        //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                        //        write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                        //    }

                                                        //}
                                                        else if (dt4.Rows[m][2].ToString() == "PARADA")
                                                        {
                                                            // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                            marcacoes.Add(hora);
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                        }
                                                        //else if (dt4.Rows[m][2].ToString() == "PARADA INTERNA")
                                                        //{
                                                        //    if (dt4.Rows[m][5].ToString() != "3" || dt4.Rows[m][5].ToString() != "1" || dt4.Rows[m][5].ToString() != "2")
                                                        //    {
                                                        //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");
                                                        //        write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                        //        write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");

                                                        //    }

                                                        //}

                                                        else
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                            marcacoes.Add(hora);
                                                            //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                        }


                                                    }
                                                    else
                                                    {
                                                        //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                    }
                                                }
                                                else
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                    marcacoes.Add(hora);
                                                    //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                }


                                            }
                                            else
                                            {
                                                // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                marcacoes.Add(hora);
                                                //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                            }
                                        }

                                    }
                                    else
                                    {
                                        if (dt4.Rows[w][2].ToString() == "FIM DE JORNADA")
                                        {
                                            int q = 0;
                                            q = w + 1;

                                            if (q < dt4.Rows.Count)
                                            {
                                                if (dt4.Rows[q][2].ToString() == "FIM DE JORNADA")
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                    marcacoes.Add(hora);
                                                    //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");

                                                }
                                                else
                                                {
                                                    int a = 0;
                                                    a = w - 1;

                                                    if (a >= 0)
                                                    {

                                                        if (dt4.Rows[a][2].ToString() == "FIM DE VIAGEM" || dt4.Rows[a][2].ToString() == "RETORNO REFEICAO" || dt4.Rows[a][2].ToString() == "INICIO DE JORNADA")
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                            marcacoes.Add(hora);
                                                            // write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");

                                                        }
                                                        //else if (dt4.Rows[a][2].ToString() == "PARADA INTERNA")
                                                        //{
                                                        //    if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2")
                                                        //    {
                                                        //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                        //        // write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                        //        // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                        //    }
                                                        //    else if (dt4.Rows[a][5].ToString() == "14")
                                                        //    {
                                                        //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                        //        write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                        //    }
                                                        //    else
                                                        //    {
                                                        //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");


                                                        //    }
                                                        //}
                                                        else if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[a][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[a][2].ToString() == "INICIO JORNADA")
                                                        {

                                                        }
                                                        else if (dt4.Rows[a][2].ToString() == "FIM DE JORNADA")
                                                        {
                                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                            marcacoes.Add(hora);
                                                            //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                        }
                                                        else
                                                        {
                                                            // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                            marcacoes.Add(hora);
                                                            // write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                        }
                                                    }
                                                }

                                            }
                                            else
                                            {
                                                int a = 0;
                                                a = w - 1;

                                                if (a >= 0)
                                                {

                                                    if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA")
                                                    {
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                        marcacoes.Add(hora);

                                                    }
                                                    else if (dt4.Rows[a][2].ToString() == "FIM DE VIAGEM" || dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                                    {
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                        marcacoes.Add(hora);
                                                        //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");

                                                    }
                                                    else if (dt4.Rows[a][2].ToString() == "RETORNO REFEICAO")
                                                    {
                                                        //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                        marcacoes.Add(hora);

                                                    }
                                                    else if (dt4.Rows[a][2].ToString() == "PARADA INTERNA")
                                                    {
                                                        if (dt4.Rows[a][5].ToString() == "14")
                                                        {
                                                            marcacoes.Add(hora);
                                                        }
                                                        else
                                                        {
                                                            marcacoes.Add(hora);
                                                        }
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");

                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                    }
                                                    //else if (dt4.Rows[a][2].ToString() == "PARADA INTERNA")
                                                    //{
                                                    //    if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2")
                                                    //    {
                                                    //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                    //        // write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                                    //        // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                    //    }
                                                    //    else if (dt4.Rows[a][5].ToString() == "14")
                                                    //    {
                                                    //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                    //        write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                    //    }
                                                    //    else
                                                    //    {
                                                    //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;95");

                                                    //    }
                                                    //}
                                                    else if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[a][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[a][2].ToString() == "INICIO JORNADA")
                                                    {

                                                    }
                                                    else if (dt4.Rows[a][2].ToString() == "FIM DE JORNADA")
                                                    {
                                                        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                        marcacoes.Add(hora);

                                                        //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                    }
                                                    else
                                                    {
                                                        // write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                        //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                    }
                                                }
                                                else
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                    marcacoes.Add(hora);
                                                    //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                }
                                            }
                                        }





                                    }


                                    #endregion

                                    #region PARADA PERNOITE

                                    if (dt4.Rows[w][2].ToString() == "PARADA PERNOITE")
                                    {
                                        int a = 0;
                                        a = w - 1;


                                        if (a >= 0)
                                        {
                                            if (dt4.Rows[a][2].ToString() == "PARADA CLIENTE / FORNECEDOR")
                                            {


                                                if (dt4.Rows[a][5].ToString() == "14")
                                                {

                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0293");
                                                    marcacoes.Add(hora);
                                                    //WriteLine(cracha + ";" + data + ";" + hora + ";1;0102");
                                                    //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;0102");
                                                    marcacoes.Add(DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm").Replace(":", ""));



                                                }
                                                else if (dt4.Rows[a][5].ToString() == "15")
                                                {

                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                    marcacoes.Add(hora);
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;22");


                                                }
                                                //else if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2")
                                                //{
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;21");
                                                //    write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;22");
                                                //    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                //}
                                                else
                                                {
                                                    marcacoes.Add(hora);
                                                }



                                            }
                                            else if (dt4.Rows[a][2].ToString() == "PARADA")
                                            {
                                                //if (dt4.Rows[a][5].ToString() == "7")
                                                //{
                                                //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                                //    write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;22");
                                                //}
                                                if (dt4.Rows[a][5].ToString() == "14")
                                                {

                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0293");
                                                    marcacoes.Add(hora);
                                                    //WriteLine(cracha + ";" + data + ";" + hora + ";1;0102");
                                                    //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;0102");
                                                    marcacoes.Add(DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm").Replace(":", ""));


                                                }
                                                else
                                                {
                                                    //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                    marcacoes.Add(hora);
                                                    //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                }

                                            }
                                            else if (dt4.Rows[a][2].ToString() == "INICIO DE VIAGEM")
                                            {
                                                marcacoes.Add(hora);
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");
                                                //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";2;22");
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";3;22");
                                            }
                                            else if (dt4.Rows[a][2].ToString() == "REINICIO DE VIAGEM")
                                            {
                                                marcacoes.Add(hora);
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");

                                                // write.WriteLine(cracha + ";" + data + ";" + hora + ";3;22");
                                            }
                                            else if (dt4.Rows[a][2].ToString() == "RETORNO REFEICAO")
                                            {
                                                marcacoes.Add(hora);
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0202");
                                                //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;2");

                                                // write.WriteLine(cracha + ";" + data + ";" + hora + ";3;22");
                                            }
                                            //else if (dt4.Rows[a][2].ToString() == "PARADA INTERNA")
                                            //{


                                            //    if (dt4.Rows[a][5].ToString() == "14")
                                            //    {

                                            //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;93");
                                            //        //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;22");


                                            //    }
                                            //    //else if (dt4.Rows[a][5].ToString() == "15")
                                            //    //{

                                            //    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                            //    //    write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;22");


                                            //    //}
                                            //    else if (dt4.Rows[a][5].ToString() == "3" || dt4.Rows[a][5].ToString() == "2")
                                            //    {
                                            //        write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                            //        //write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;22");
                                            //        //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;2");
                                            //    }
                                            //    //else
                                            //    //{
                                            //    //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;22");
                                            //    //}



                                            //}
                                            //else
                                            //{
                                            //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;22");
                                            //}

                                        }
                                        //else
                                        //{
                                        //    write.WriteLine(cracha + ";" + data + ";" + hora + ";1;22");
                                        //    write.WriteLine(cracha + ";" + data + ";" + DateTime.Parse(hora).AddMinutes(1).ToString("HH:mm") + ";1;22");

                                        //}

                                    }

                                    #endregion

                                    #region RETORNO PERNOITE
                                    //BLOCO REINICIO / INICIO DE VIAGEM
                                    if (dt4.Rows[w][2].ToString() == "RETORNO PERNOITE")
                                    {
                                        int a = 0;
                                        a = w - 1;
                                        if (a >= 0)
                                        {
                                            if (dt4.Rows[a][2].ToString() == "INICIO DE JORNADA" || dt4.Rows[a][2].ToString() == "INICIO JORNADA CAMINHAO" || dt4.Rows[a][2].ToString() == "INICIO JORNADA")
                                            {

                                            }
                                            else if (dt4.Rows[a][2].ToString() == "PARADA PERNOITE")
                                            {
                                                //write.WriteLine("+00" + hora.Replace(":", "") + data.Replace("/", "") + cracha.PadLeft(10, '0') + "01020000");
                                                marcacoes.Add(hora);
                                                //write.WriteLine(cracha + ";" + data + ";" + hora + ";2;94");
                                            }


                                        }
                                        else
                                        {
                                            //write.WriteLine(cracha + ";" + data + ";" + hora + ";1;0102");
                                            marcacoes.Add(hora);
                                            // write.WriteLine(cracha + ";" + data + ";" + hora + ";2;2");
                                        }



                                    }
                                    //FIM
                                    #endregion


                                }

                                //FIM

                                #endregion
                            }
                        }

                    }

                }



                else
                {

                    string retorno = "Erro! Contate o administrador. Detalhes do erro 60340: Não há marcações";
                    System.Text.StringBuilder sb2 = new System.Text.StringBuilder();
                    sb2.Append("<script type = 'text/javascript'>");
                    sb2.Append("window.onload=function(){");
                    sb2.Append("alert('");
                    sb2.Append(retorno);
                    sb2.Append("')};");
                    sb2.Append("</script>");
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb2.ToString());
                }
                dtRow["Marcacao"] = string.Join(" ", marcacoes);
                nrmarcacoes = marcacoes.Count;
                //if (nrmarcacoes == 2)
                //{
                //	dtRow["Status"] = "Verificar saída antecipada ou jornada sem refeiç!";
                //}
                //else if (nrmarcacoes % 2 != 0)
                //{
                //	dtRow["Status"] = "Marcação incorreta, verificar!";
                //}
                //else if (nrmarcacoes == 0)
                //{
                //	dtRow["Status"] = "";
                //}
                //else
                //{
                //	dtRow["Status"] = "OK!";
                //}

                dtDados.Rows.Add(dtRow);
                marcacoes.Clear();
                //nrmarcacoes = 0;



            }

            Session["DataTable"] = dtDados;
            gvPonto.DataSource = dtDados;
            gvPonto.DataBind();
            dtDados.Rows.Clear();





        }

        }
}