using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Net.Mail;
using System.Text;
using GMaps;
using GMaps.Classes;
using Subgurim;
using Subgurim.Controles;
using Subgurim.Controls;
using Subgurim.Maps;
using Subgurim.Web;
using System.Drawing;
using System.Web.UI.HtmlControls;
using System.Globalization;

namespace NewCapit.dist.pages
{
    public partial class demo_motorista : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString());
        protected void Page_Load(object sender, EventArgs e)
        {
            Carrega();
        }
        public void Carrega()
        {
            string cracha, data1, data2, hr;
            if (HttpContext.Current.Request.QueryString["cracha"].ToString() != "" || HttpContext.Current.Request.QueryString["data1"].ToString() != "" || HttpContext.Current.Request.QueryString["data2"].ToString() != "")
            {
                cracha = string.Empty;
                data1 = string.Empty;
                data2 = string.Empty;
                hr = string.Empty;
                cracha = HttpContext.Current.Request.QueryString["cracha"].ToString();
                data1 = DateTime.Parse(HttpContext.Current.Request.QueryString["data1"].ToString()).ToString("yyyy-MM-dd");
                data2 = HttpContext.Current.Request.QueryString["data2"].ToString();
                hr = HttpContext.Current.Request.QueryString["hr"].ToString();

                string sql1 = "exec sp_tempo_total '" + DateTime.Parse(data1).ToString("yyyy-MM-dd") + "','" + DateTime.Parse(data1).ToString("yyyy-MM-dd") + "'," + cracha + "";
                SqlDataAdapter adtp1 = new SqlDataAdapter(sql1, con);
                DataTable dt = new DataTable();
                con.Open();
                adtp1.Fill(dt);
                con.Close();

                //string sql4 = "exec sp_tempo_total_noturno '" + data1 + "','" + data2 + "'," + cracha + "";
                //SqlDataAdapter adtp4 = new SqlDataAdapter(sql4, con);
                //DataTable dt4 = new DataTable();
                //con.Open();
                //adtp4.Fill(dt4);
                //con.Close();
                int dia;
                int mes;
                int ano;

                CultureInfo culture = new CultureInfo("pt-BR");
                DateTimeFormatInfo dtfi = culture.DateTimeFormat;

                //dia = Convert.ToInt32(data1.Substring(0, 2));
                //mes = Convert.ToInt32(data1.Substring(3, 2));
                //ano = Convert.ToInt32(data1.Substring(6, 4));
                dia = Convert.ToInt32(data1.Substring(8, 2));
                mes = Convert.ToInt32(data1.Substring(5, 2));
                ano = Convert.ToInt32(data1.Substring(0, 4));

                DateTime data = new DateTime(ano, mes, dia);

                string sql2 = "select codmot, nommot, cargo, horario from tbmotoristas where codmot='" + cracha + "'";
                SqlDataAdapter adtp2 = new SqlDataAdapter(sql2, con);
                DataTable dt2 = new DataTable();
                con.Open();
                adtp2.Fill(dt2);
                con.Close();

                string sql3 = "select top 1 * from tb_parada where cod_cracha='" + cracha + "' and ds_macro='FIM DE JORNADA' and dt_posicao_parada='" + DateTime.Parse(data1).ToString("yyyy-MM-dd") + "' and fl_deletado is null OR cod_cracha='" + cracha + "' and ds_macro='PARADA PERNOITE' and dt_posicao_parada='" + DateTime.Parse(data1).ToString("yyyy-MM-dd") + "' and fl_deletado is null OR cod_cracha='" + cracha + "' and ds_macro='INICIO DE JORNADA' and dt_posicao_parada='" + DateTime.Parse(data1).ToString("yyyy-MM-dd") + "' and fl_deletado is null OR cod_cracha='" + cracha + "' and ds_macro='REINICIO DE VIAGEM' and dt_posicao_parada='" + DateTime.Parse(data1).ToString("yyyy-MM-dd") + "' and fl_deletado is null order by hr_posicao";
                SqlDataAdapter adtp3 = new SqlDataAdapter(sql3, con);
                DataTable dt3 = new DataTable();
                con.Open();
                adtp3.Fill(dt3);
                con.Close();

                string[] horario = dt2.Rows[0][3].ToString().Trim().Split('-');

                lblCracha.Text = cracha;
                lblData.Text = data1 + " - " + dtfi.GetDayName(data.DayOfWeek);
                lblNome.Text = dt2.Rows[0][1].ToString();
                lblCargo.Text = dt2.Rows[0][2].ToString();
                lblHorário.Text = DateTime.Parse(horario[0]).ToString("HH:mm") + " - " + DateTime.Parse(horario[1]).ToString("HH:mm");
                if (dt.Rows.Count > 0)
                {

                    if (HttpContext.Current.Request.QueryString["hr"].ToString() != "undefined")
                    {
                        //string sqlx = "select * from tb_parada where cod_cracha='" + login + "' and dt_posicao_parada='" + DateTime.Parse(txtData.Text).ToString("dd/MM/yyyy") + "' and fl_deletado is null and hr_posicao > '"+dt3.Rows[0][4].ToString()+"' order by hr_posicao ";



                        string sqlx = "exec sp_tempo_total_dif '" + DateTime.Parse(data1).ToString("yyyy-MM-dd") + "','" + DateTime.Parse(data1).ToString("yyyy-MM-dd") + "','" + hr + "'," + cracha + "";
                        SqlDataAdapter adtpx = new SqlDataAdapter(sqlx, con);
                        DataTable dtx = new DataTable();
                        con.Open();
                        adtpx.Fill(dtx);
                        con.Close();

                        grdMotoristas.DataSource = dtx;
                        grdMotoristas.DataBind();
                    }
                    else
                    {
                        grdMotoristas.DataSource = dt;
                        grdMotoristas.DataBind();
                    }
                }
                string sql6 = "select nm_usuario from tb_exclusao_macro where dt_macro='" + data1 + "' and cod_cracha=" + cracha + " group by nm_usuario";
                SqlDataAdapter apt6 = new SqlDataAdapter(sql6, con);
                DataTable dt6 = new DataTable();
                con.Open();
                apt6.Fill(dt6);
                con.Close();

                string[] usuarios; // Declare without initialization for clarity

                if (dt6.Rows.Count > 0)
                {
                    usuarios = new string[dt6.Rows.Count];
                    for (int i = 0; i < dt6.Rows.Count; i++)
                    {
                        usuarios[i] = dt6.Rows[i]["nm_usuario"].ToString();
                    }
                }
                else
                {
                    usuarios = new string[0]; // Empty array if no results
                }

                // Concatenate usuarios into a comma-separated string using StringBuilder
                StringBuilder sb = new StringBuilder();
                foreach (string usuario in usuarios)
                {
                    if (sb.Length > 0)
                    {
                        sb.Append(",");
                    }
                    sb.Append(usuario);
                }

                string usuariosSeparadosPorVirgula = sb.ToString();

                if (usuariosSeparadosPorVirgula.Length == 0)
                {

                }
                else
                {
                    lblAltera.Text = "Relatório Alterado por: " + usuariosSeparadosPorVirgula;

                }
            }
        }
    }
}