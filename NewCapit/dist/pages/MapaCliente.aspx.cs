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
    public partial class MapaCliente : System.Web.UI.Page
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
            if (HttpContext.Current.Request.QueryString["id"].ToString() != "")
            {
                id = HttpContext.Current.Request.QueryString["id"].ToString();
            }
            string sql = "select razcli,latitude,longitude,raio, endcli,baicli,cidcli,estcli from tbclientes where id=" + id;
           
            SqlDataAdapter adpt = new SqlDataAdapter(sql, con);
            DataTable dt = new DataTable();
            con.Open();
            adpt.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                try
                {
                   

                    empresa = dt.Rows[0][0].ToString();
                    lat = dt.Rows[0][1].ToString();
                    lon = dt.Rows[0][2].ToString();
                    rua = dt.Rows[0][4].ToString(); ;
                    cidade = dt.Rows[0][6].ToString();
                    bairro = dt.Rows[0][5].ToString();
                    uf = dt.Rows[0][7].ToString();

                    //if (dt.Rows[0][5].ToString() == "1")
                    //{
                    //    ignicao = "Ligada";
                    //}
                    //else
                    //{
                    //    ignicao = "Desligada";
                    //}

                    //velocidade = dt.Rows[0][8].ToString() + " Km/h";
                    //rua = dt.Rows[0][9].ToString();
                    //uf = dt.Rows[0][10].ToString();

                    GLatLng latlng1 = new GLatLng(Convert.ToDouble(lat, CultureInfo.InvariantCulture), Convert.ToDouble(lon, CultureInfo.InvariantCulture));
                    window = new GInfoWindow(latlng1, string.Format(@"<b>Informações:</b><br />Empresa: {0}<br/>Lat: {1}<br/>Long: {2}<br/>End: {3}<br/>Bairro: {4}<br/>Cidade: {5}<br/>UF: {6}",

                                                      empresa, lat, lon, rua, bairro, cidade, uf), true);
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