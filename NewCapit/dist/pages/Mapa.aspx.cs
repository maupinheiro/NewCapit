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
    public partial class Mapa : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString());
        string hora, placa, lat, lon, ignicao, velocidade, rua, uf;
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
            GLatLng latlng = new GLatLng(-23, -48);

            GMap2.setCenter(latlng, 5);



            string sql = "Select t.nr_idveiculo, v.ds_placa, t.ds_cidade, t.dt_posicao, t.nr_dist_referencia, t.fl_ignicao,t.ds_lat,t.ds_long, nr_direcao  ";
            sql += " from tb_transmissao as t inner join tb_veiculo_sascar as v on t.nr_idveiculo=v.nr_idveiculo";
            SqlDataAdapter adpt = new SqlDataAdapter(sql, con);
            DataTable dt = new DataTable();
            con.Open();
            adpt.Fill(dt);
            con.Close();

            for (int r = 0; r < dt.Rows.Count - 1; r++)
            {
                //GMap1.Add(new GMapUI());
                GMapUIOptions options = new GMapUIOptions();
                options.maptypes_hybrid = true;
                options.keyboard = true;
                options.maptypes_physical = true;
                options.zoom_scrollwheel = true;
                options.controls_scalecontrol = true;
                options.controls_maptypecontrol = true;
                options.controls_menumaptypecontrol = true;




                GMap2.Add(new GMapUI(options));

                GLatLng latlng1 = new GLatLng(Convert.ToDouble(dt.Rows[r][6].ToString(), CultureInfo.InvariantCulture), Convert.ToDouble(dt.Rows[r][7].ToString(), CultureInfo.InvariantCulture));
                /* GIcon ico = new GIcon();
                 ico.image = "/img/ico_truck.png";
                 GMap1.Add(new GMarker(latlng1));*/
                // GLatLng latlng1 = new GLatLng(-23.679611666666666, -46.592063333333336);

                GIcon ico = new GIcon();


                ico.iconAnchor = new GPoint(25, 10);
                if (dt.Rows[r][8].ToString() == "0")
                {
                    ico.image = "img/ico_truck.png";
                }
                if (dt.Rows[r][8].ToString() == "1")
                {
                    ico.image = "img/ico_truck1.png";
                }
                if (dt.Rows[r][8].ToString() == "2")
                {
                    ico.image = "img/ico_truck2.png";
                }
                if (dt.Rows[r][8].ToString() == "3")
                {
                    ico.image = "img/ico_truck3.png";
                }
                if (dt.Rows[r][8].ToString() == "4")
                {
                    ico.image = "img/ico_truck4.png";
                }
                if (dt.Rows[r][8].ToString() == "5")
                {
                    ico.image = "img/ico_truck5.png";
                }
                if (dt.Rows[r][8].ToString() == "6")
                {
                    ico.image = "img/ico_truck6.png";
                }
                if (dt.Rows[r][8].ToString() == "7")
                {
                    ico.image = "img/ico_truck7.png";
                }

                GMarkerOptions mOpts = new GMarkerOptions();
                mOpts.clickable = true;
                mOpts.icon = ico;
                mOpts.draggable = false;

                GMarker marker = new GMarker(latlng1, mOpts);

                /* GInfoWindow window2 = new GInfoWindow(marker, latlng1.ToString(), false, GListener.Event.mouseover);
                 GMap1.Add(window2);*/

                GMap2.Add(marker);






            }





        }



        protected string GMap2_MarkerClick(object s, GAjaxServerEventArgs e)
        {
            string sql = "Select t.nr_idveiculo, v.ds_placa, t.ds_cidade, t.dt_posicao, t.nr_dist_referencia, t.fl_ignicao,t.ds_lat,t.ds_long,t.nr_velocidade, t.ds_rua, t.ds_uf   ";
            sql += " from tb_transmissao as t inner join tb_veiculo_sascar as v on t.nr_idveiculo=v.nr_idveiculo where t.ds_lat=" + e.point.lat.ToString().Replace(",", ".") + " and t.ds_long=" + e.point.lng.ToString().Replace(",", ".");
            SqlDataAdapter adpt = new SqlDataAdapter(sql, con);
            DataTable dt = new DataTable();
            con.Open();
            adpt.Fill(dt);
            con.Close();

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

                GLatLng latlng1 = new GLatLng(Convert.ToDouble(dt.Rows[0][6].ToString(), CultureInfo.InvariantCulture), Convert.ToDouble(dt.Rows[0][7].ToString(), CultureInfo.InvariantCulture));
                GLatLng latlng2 = new GLatLng(Convert.ToDouble(dt.Rows[0][6].ToString(), CultureInfo.InvariantCulture), Convert.ToDouble(dt.Rows[0][7].ToString(), CultureInfo.InvariantCulture));

                window = new GInfoWindow(latlng1, string.Format(@"<b>Informações:</b><br />Horário: {0}<br/>Placa: {1}<br/>Lat: {2}<br/>Long: {3}<br/>End: {4}<br/>UF: {5}<br/>Ignição: {6}<br/>Velocidade: {7}",
                                                 hora, placa, lat, lon, rua, uf, ignicao, velocidade), true);



                GMap2.Add(window);


                //return "document.getElementById('Msg1').innerHTML="+ e.point.lat.ToString() + ";" + "document.getElementById('Msg2').innerHTML="+ e.point.lng.ToString();



            }
            catch
            {
                CarregaMap();

            }
            return window.ToString(e.map);

        }
    }
}