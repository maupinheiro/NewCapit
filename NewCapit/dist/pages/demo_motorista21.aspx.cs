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
    public partial class demo_motorista21 : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString());
        protected void Page_Load(object sender, EventArgs e)
        {
            Carrega();
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
            }
        }

        public void Carrega()
        {
            string cracha, data1, data2, hr_1, hr_2;
            if (HttpContext.Current.Request.QueryString["cracha"].ToString() != "" || HttpContext.Current.Request.QueryString["data1"].ToString() != "" || HttpContext.Current.Request.QueryString["data2"].ToString() != "" || HttpContext.Current.Request.QueryString["hr_1"].ToString() != "" || HttpContext.Current.Request.QueryString["hr_2"].ToString() != "")
            {
                cracha = string.Empty;
                data1 = string.Empty;
                data2 = string.Empty;
                hr_1 = string.Empty;
                hr_2 = string.Empty;
                cracha = HttpContext.Current.Request.QueryString["cracha"].ToString();
                data1 = HttpContext.Current.Request.QueryString["data1"].ToString();
                data2 = HttpContext.Current.Request.QueryString["data2"].ToString();
                hr_1 = HttpContext.Current.Request.QueryString["hr_1"].ToString();
                hr_2 = HttpContext.Current.Request.QueryString["hr_2"].ToString();

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

                string sql4 = "exec sp_tempo_total_noturno '" + data1 + "','" + data2 + "','" + hr_1 + "','" + hr_2 + "', " + cracha + "";
                SqlDataAdapter adtp4 = new SqlDataAdapter(sql4, con);
                DataTable dt4 = new DataTable();
                con.Open();
                adtp4.Fill(dt4);
                con.Close();

                string[] horario = dt2.Rows[0][3].ToString().Trim().Split('-');

                lblCracha.Text = cracha;
                lblData.Text = data1 + " - " + dtfi.GetDayName(data.DayOfWeek);
                lblNome.Text = dt2.Rows[0][1].ToString();
                lblCargo.Text = dt2.Rows[0][2].ToString();
                lblHorário.Text = DateTime.Parse(horario[0]).ToString("HH:mm") + " - " + DateTime.Parse(horario[1]).ToString("HH:mm");

                grdMotoristas.DataSource = dt4;
                grdMotoristas.DataBind();

                // 1. Criamos a query unificada com UNION e parâmetros (@data e @cracha)
                string sql6 = @"
                            SELECT nm_usuario FROM tb_exclusao_macro 
                            WHERE dt_macro = @data AND cod_cracha = @cracha 
                            GROUP BY nm_usuario

                            UNION

                            SELECT u.nm_usuario FROM tb_usuario AS u 
                            INNER JOIN tb_parada AS p ON u.cod_usuario = p.cod_usuario_alt 
                            WHERE p.dt_posicao_parada = @data AND p.cod_cracha = @cracha 
                            GROUP BY u.nm_usuario";

                SqlCommand cmd6 = new SqlCommand(sql6, con);
                // Passando os parâmetros corretamente
                cmd6.Parameters.AddWithValue("@data", data1);
                cmd6.Parameters.AddWithValue("@cracha", cracha);

                SqlDataAdapter apt6 = new SqlDataAdapter(cmd6);
                DataTable dt6 = new DataTable();

                con.Open();
                apt6.Fill(dt6);
                con.Close();

                string[] usuarios;

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
                    usuarios = new string[0];
                }

                // Junta os usuários separados por vírgula
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

                if (usuariosSeparadosPorVirgula.Length > 0)
                {
                    lblAltera.Text = "Relatório Alterado por: " + usuariosSeparadosPorVirgula;
                }
                else
                {
                    // Opcional: limpar o label ou tratar caso não haja alterações
                    lblAltera.Text = "";
                }

            }

        }
    }
}