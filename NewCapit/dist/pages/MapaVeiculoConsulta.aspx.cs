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
using System.Globalization;

namespace NewCapit.dist.pages
{
    public partial class MapaVeiculoConsulta : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString());
        string cidade, empresa, lat, lon, ignicao, bairro, rua, uf,id, placa, hora,velocidade;
        GInfoWindow window;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                CarregaMap();
                
            }


        }
      

        public void CarregaMap()
        {
            if (HttpContext.Current.Request.QueryString["placa"].ToString() != "")
            {
                id = HttpContext.Current.Request.QueryString["placa"].ToString();
            }
            string sql = "Select t.nr_idveiculo, v.ds_placa, t.ds_cidade, t.dt_posicao, t.nr_dist_referencia, t.fl_ignicao,t.ds_lat,t.ds_long,t.nr_velocidade, t.ds_rua, t.ds_uf, t.nr_direcao   ";
            sql += " from tb_transmissao as t inner join tb_veiculo_sascar as v on t.nr_idveiculo=v.nr_idveiculo where v.ds_placa='" + id + "'";
            SqlDataAdapter adpt = new SqlDataAdapter(sql, con);
            DataTable dt = new DataTable();
            con.Open();
            adpt.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                try
                {
                    hora = DateTime.Parse(dt.Rows[0][3].ToString()).ToString("dd/MM/yyyy - HH:mm:ss");

                    placa = dt.Rows[0][1].ToString();
                    lat = dt.Rows[0][6].ToString();
                    lon = dt.Rows[0][7].ToString();

                    if (dt.Rows[0][5].ToString() == "1")
                    {
                        ignicao = "Ligada";
                    }
                    else
                    {
                        ignicao = "Desligada";
                    }

                    velocidade = dt.Rows[0][8].ToString() + " Km/h";
                    rua = dt.Rows[0][9].ToString();
                    uf = dt.Rows[0][10].ToString();

                    GLatLng latlng1 = new GLatLng(Convert.ToDouble(lat, CultureInfo.InvariantCulture), Convert.ToDouble(lon, CultureInfo.InvariantCulture));

                    window = new GInfoWindow(latlng1, string.Format(@"<b>Informações:</b><br />Horário: {0}<br/>Placa: {1}<br/>Lat: {2}<br/>Long: {3}<br/>End: {4}<br/>UF: {5}<br/>Ignição: {6}<br/>Velocidade: {7}",
                                                      hora, placa, lat, lon, rua, uf, ignicao, velocidade), true);

                    GMap1.Add(window);

                    // Focar o veículo no mapa com zoom adequado
                    GMap1.setCenter(latlng1);
                    GMap1.GZoom = 18;
                    //GMap1; // Ajuste conforme necessário
                    GIcon ico = new GIcon();


                    ico.iconAnchor = new GPoint(25, 10);
                    if (dt.Rows[0][11].ToString() == "0")
                    {
                        ico.image = "../img/ico_truck.png";
                    }
                    if (dt.Rows[0][11].ToString() == "1")
                    {
                        ico.image = "../img/ico_truck1.png";
                    }
                    if (dt.Rows[0][11].ToString() == "2")
                    {
                        ico.image = "../img/ico_truck2.png";
                    }
                    if (dt.Rows[0][11].ToString() == "3")
                    {
                        ico.image = "../img/ico_truck3.png";
                    }
                    if (dt.Rows[0][11].ToString() == "4")
                    {
                        ico.image = "../img/ico_truck4.png";
                    }
                    if (dt.Rows[0][11].ToString() == "5")
                    {
                        ico.image = "../img/ico_truck5.png";
                    }
                    if (dt.Rows[0][11].ToString() == "6")
                    {
                        ico.image = "../img/ico_truck6.png";
                    }
                    if (dt.Rows[0][11].ToString() == "7")
                    {
                        ico.image = "../img/ico_truck7.png";
                    }
                    GMarkerOptions mOpts = new GMarkerOptions();
                    mOpts.clickable = true;
                    mOpts.icon = ico;
                    mOpts.draggable = false;

                    GMarker marker = new GMarker(latlng1, mOpts);

                    /* GInfoWindow window2 = new GInfoWindow(marker, latlng1.ToString(), false, GListener.Event.mouseover);
                     GMap1.Add(window2);*/

                    GMap1.Add(marker);
                }
                catch
                {
                    CarregaMap();
                }



                //return "document.getElementById('Msg1').innerHTML="+ e.point.lat.ToString() + ";" + "document.getElementById('Msg2').innerHTML="+ e.point.lng.ToString();

                //return window.ToString(e.map);
            }
            else
            {
                string linha1 = "Placa não encontrada no sistema.";


                // Concatenando as linhas com '\n' para criar a mensagem
                string mensagem = $"{linha1}";

                string mensagemCodificada = HttpUtility.JavaScriptStringEncode(mensagem);
                // Gerando o script JavaScript para exibir o alerta
                string script = $"alert('{mensagemCodificada}');";

                // Registrando o script para execução no lado do cliente
                ClientScript.RegisterStartupScript(this.GetType(), "MensagemDeAlerta", script, true);

               
            }




        }



        
    }
}