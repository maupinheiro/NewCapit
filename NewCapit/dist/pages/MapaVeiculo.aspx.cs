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
using System.Globalization;

namespace NewCapit.dist.pages
{
    public partial class MapaVeiculo : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString());
        string hora, placa, lat, lon, ignicao, velocidade, rua, uf;

        protected void btnPlaca_Click(object sender, EventArgs e)
        {
            string sql = "Select t.nr_idveiculo, v.ds_placa, t.ds_cidade, t.dt_posicao, t.nr_dist_referencia, t.fl_ignicao,t.ds_lat,t.ds_long,t.nr_velocidade, t.ds_rua, t.ds_uf   ";
            sql += " from tb_transmissao as t inner join tb_veiculo_sascar as v on t.nr_idveiculo=v.nr_idveiculo where v.ds_placa=@placa";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@placa", txtPlaca.Text.Trim());
            SqlDataAdapter adpt = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            adpt.Fill(dt);
            con.Close();

            if(dt.Rows.Count > 0)
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

                txtPlaca.Text = "";
                txtPlaca.Focus();
            }

           
        }


        GInfoWindow window;
        protected void Page_Load(object sender, EventArgs e)
        {
            

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
                CarregaMap();
            }
        }
        public void CarregaMap()
        {
            // 1. Configurações Iniciais do Mapa
            GLatLng centroInicial = new GLatLng(-23.5505, -46.6333); // Ex: Centro de SP ou sua preferência
            GMap1.setCenter(centroInicial, 5);

            // Configura as opções de interface apenas UMA VEZ (fora do loop)
            GMapUIOptions options = new GMapUIOptions
            {
                maptypes_hybrid = true,
                keyboard = true,
                maptypes_physical = true,
                zoom_scrollwheel = true,
                controls_scalecontrol = true,
                controls_maptypecontrol = true,
                controls_menumaptypecontrol = true
            };
            GMap1.Add(new GMapUI(options));

            // 2. Busca de Dados
            // Importante: Verifique se a conexão 'con' está instanciada corretamente no topo da classe
            string sql = @"SELECT t.nr_idveiculo, v.ds_placa, t.ds_cidade, t.dt_posicao, 
                          t.fl_ignicao, t.ds_lat, t.ds_long, t.nr_direcao 
                   FROM tb_transmissao AS t 
                   INNER JOIN tb_veiculo_sascar AS v ON t.nr_idveiculo = v.nr_idveiculo 
                   WHERE YEAR(t.dt_posicao) = YEAR(GETDATE())";

            DataTable dt = new DataTable();

            try
            {
                using (SqlDataAdapter adpt = new SqlDataAdapter(sql, con))
                {
                    con.Open();
                    adpt.Fill(dt);
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                // Logar erro se necessário: ex.Message
                if (con.State == ConnectionState.Open) con.Close();
                return;
            }

            // 3. Renderização dos Marcadores
            // Corrigido: r < dt.Rows.Count para não pular o último registro
            for (int r = 0; r < dt.Rows.Count; r++)
            {
                try
                {
                    // Conversão segura de coordenadas
                    double latitude = Convert.ToDouble(dt.Rows[r]["ds_lat"].ToString(), CultureInfo.InvariantCulture);
                    double longitude = Convert.ToDouble(dt.Rows[r]["ds_long"].ToString(), CultureInfo.InvariantCulture);
                    GLatLng posicaoVeiculo = new GLatLng(latitude, longitude);

                    // Definição do Ícone
                    GIcon ico = new GIcon();
                    ico.iconAnchor = new GPoint(25, 10);

                    // Corrigido: Pegando o valor da linha ATUAL [r] e não sempre da [0]
                    string direcaoStr = dt.Rows[r]["nr_direcao"]?.ToString() ?? "";
                    string iconPath = "../img/ico_truck.png"; // Default

                    if (!string.IsNullOrEmpty(direcaoStr))
                    {
                        string prefixo = direcaoStr.Substring(0, 1);
                        // Mapeia o prefixo para a imagem correspondente
                        iconPath = $"../img/ico_truck{prefixo}.png";

                        // Caso o prefixo seja "0", usamos a imagem padrão sem número
                        if (prefixo == "0") iconPath = "../img/ico_truck.png";
                    }

                    ico.image = iconPath;

                    // Opções do Marcador
                    GMarkerOptions mOpts = new GMarkerOptions
                    {
                        clickable = true,
                        icon = ico,
                        draggable = false,
                        title = dt.Rows[r]["ds_placa"].ToString() // Tooltip com a placa
                    };

                    GMarker marker = new GMarker(posicaoVeiculo, mOpts);

                    // Adiciona o marcador ao mapa
                    GMap1.Add(marker);
                }
                catch
                {
                    // Se uma linha falhar (ex: lat/long inválido), pula para a próxima
                    continue;
                }
            }
        }


        protected string GMap1_MarkerClick(object s, GAjaxServerEventArgs e)
        {
            // 1. Criar o comando com Parâmetros para evitar o erro de conversão (Arithmetic Overflow)
            string sql = @"SELECT t.nr_idveiculo, v.ds_placa, t.ds_cidade, t.dt_posicao, 
                          t.nr_dist_referencia, t.fl_ignicao, t.ds_lat, t.ds_long, 
                          t.nr_velocidade, t.ds_rua, t.ds_uf 
                   FROM tb_transmissao as t 
                   INNER JOIN tb_veiculo_sascar as v ON t.nr_idveiculo = v.nr_idveiculo 
                   WHERE t.ds_lat = @lat AND t.ds_long = @lon 
                   AND YEAR(t.dt_posicao) = YEAR(GETDATE())";

            DataTable dt = new DataTable();

            try
            {
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    // O segredo está aqui: o ADO.NET trata a conversão numérica para você
                    cmd.Parameters.AddWithValue("@lat", e.point.lat);
                    cmd.Parameters.AddWithValue("@lon", e.point.lng);

                    if (con.State == ConnectionState.Closed) con.Open();

                    using (SqlDataAdapter adpt = new SqlDataAdapter(cmd))
                    {
                        adpt.Fill(dt);
                    }
                    con.Close();
                }

                if (dt.Rows.Count > 0)
                {
                    // 2. Extração dos dados usando nomes de colunas (mais seguro que índices)
                    DataRow row = dt.Rows[0];

                    hora = Convert.ToDateTime(row["dt_posicao"]).ToString("dd/MM/yyyy - HH:mm:ss");
                    placa = row["ds_placa"].ToString();
                    lat = row["ds_lat"].ToString();
                    lon = row["ds_long"].ToString();
                    ignicao = row["fl_ignicao"].ToString() == "1" ? "Ligada" : "Desligada";
                    velocidade = row["nr_velocidade"].ToString() + " Km/h";
                    rua = row["ds_rua"].ToString();
                    uf = row["ds_uf"].ToString();

                    // 3. Criar a Janela de Informações
                    GLatLng latlng = new GLatLng(e.point.lat, e.point.lng);

                    string conteudo = string.Format(
                        "<b>Informações:</b><br />" +
                        "Horário: {0}<br/>Placa: {1}<br/>Lat: {2}<br/>Long: {3}<br/>" +
                        "End: {4}<br/>UF: {5}<br/>Ignição: {6}<br/>Velocidade: {7}",
                        hora, placa, lat, lon, rua, uf, ignicao, velocidade);

                    window = new GInfoWindow(latlng, conteudo, true);

                    // Adiciona ao mapa para que o componente saiba o que renderizar
                    GMap1.Add(window);

                    return window.ToString(e.map);
                }
            }
            catch (Exception ex)
            {
                // Se houver erro, logue ou trate, mas não deixe a página quebrar
                // CarregaMap(); // Opcional
            }
            finally
            {
                if (con.State == ConnectionState.Open) con.Close();
            }

            return "";
        }
    }
}