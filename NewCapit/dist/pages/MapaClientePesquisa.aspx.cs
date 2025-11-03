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
    public partial class MapaClientePesquisa : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString());
        string cidade, empresa, lat, lon, ignicao, bairro, rua, uf,id;
        GInfoWindow window;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                CarregaMap();

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
      

        public void CarregaMap()
        {
            if (HttpContext.Current.Request.QueryString["lat"].ToString() != "" && HttpContext.Current.Request.QueryString["long"].ToString() != "")
            {
                lat = HttpContext.Current.Request.QueryString["lat"].ToString();
                lon = HttpContext.Current.Request.QueryString["long"].ToString();
                try
                {





                    GLatLng latlng1 = new GLatLng(Convert.ToDouble(lat, CultureInfo.InvariantCulture), Convert.ToDouble(lon, CultureInfo.InvariantCulture));
                    window = new GInfoWindow(latlng1, string.Format(@"<b>Informações:Lat: {0}<br/>Long: {1}",

                    lat, lon), true);
                    GIcon ico = new GIcon();
                    ico.image = "../img/icon_loc.png";
                    GMap1.Add(new GMarker(latlng1));
                    GMap1.Add(window);

                    // Focar o veículo no mapa com zoom adequado
                    GMap1.setCenter(latlng1, 18, GMapType.GTypes.Satellite);
                    GMap1.GZoom = 18;
                    //GMap1; // Ajuste conforme necessário
                }
                catch
                {
                    CarregaMap();
                }
            }
            else
            {

            }
            

               



                //return "document.getElementById('Msg1').innerHTML="+ e.point.lat.ToString() + ";" + "document.getElementById('Msg2').innerHTML="+ e.point.lng.ToString();

                //return window.ToString(e.map);
        }
           




    }



        
    
}