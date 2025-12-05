using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Configuration;
using System.Web.UI.HtmlControls;
using System.Collections;
using Org.BouncyCastle.Asn1.Cmp;
using NPOI.SS.Formula.Functions;
using Domain;
using static NPOI.HSSF.Util.HSSFColor;
using System.Threading;
using System.Diagnostics.Eventing.Reader;
using System.Web.Services.Description;
using NPOI.SS.UserModel;
using ICSharpCode.SharpZipLib.Zip;
using MathNet.Numerics.Providers.SparseSolver;
using System.Drawing.Drawing2D;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using System.Runtime.InteropServices.ComTypes;
using System.Globalization;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Drawing;
using System.Web.Script.Serialization;
using System.Text;


namespace NewCapit.dist.pages
{
    public partial class ColetasMatriz : System.Web.UI.Page
    {
        SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString());
        public string fotoMotorista;
        string codmot, caminhofoto;
        string num_coleta;
        DateTime dataHoraAtual = DateTime.Now;
        string sDuracao, sPercurso;
        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (!IsPostBack)
            {
                if (Session["UsuarioLogado"] != null)
                {
                    string nomeUsuario = Session["UsuarioLogado"].ToString();
                    var lblUsuario = nomeUsuario;
                    //txtUsuCadastro.Text = nomeUsuario;
                }
                else
                {
                    var lblUsuario = "<Usuário>";
                    //  txtUsuCadastro.Text = lblUsuario;
                    Response.Redirect("Login.aspx");
                }

                // lblDtCadastro.Text = dataHoraAtual.ToString("dd/MM/yyyy HH:mm");
                // txtCadastro.Text = dataHoraAtual.ToString("dd/MM/yyyy HH:mm");

                //  PreencherComboMotoristas();
                //CarregarGrid();
                //  PreencherNumColeta();
                fotoMotorista = "/img/totalFunc.png";
            }
            //CarregaFoto();
           

            

        }
        //protected void btnBuscar_Click(object sender, EventArgs e)
        //{
        //    string carga = txtCarga.Text.Trim();

        //    CarregarCarga(carga);
        //    CarregarPedidosAccordion(carga);
        //    //using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))

        //}
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            string carga = txtCarga.Text.Trim();

            carregarDadosCarga(carga);
            carregarPedidos(carga);
            carregarInformacoesExtras(carga);
        }

        void carregarDadosCarga(string carga)
        {
            SqlDataAdapter da = new SqlDataAdapter(
                "SELECT * FROM tbcargas WHERE carga = @c", conn);

            da.SelectCommand.Parameters.AddWithValue("@c", carga);

            DataTable dt = new DataTable();
            da.Fill(dt);

            gvCarga.DataSource = dt;
            gvCarga.DataBind();
        }

        void carregarPedidos(string carga)
        {
            SqlDataAdapter da = new SqlDataAdapter(
                "SELECT * FROM tbpedidos WHERE carga = @c", conn);

            da.SelectCommand.Parameters.AddWithValue("@c", carga);

            DataTable dt = new DataTable();
            da.Fill(dt);

            gvPedidos.DataSource = dt;
            gvPedidos.DataBind();

            lblTotalPedidos.Text = "Total de Pedidos: " + dt.Rows.Count;

            decimal soma = 0;
            foreach (DataRow r in dt.Rows)
                soma += Convert.ToDecimal(r["peso"]);

            lblSomaValores.Text = "Soma dos Valores: " + soma.ToString("N2");
        }

        void carregarInformacoesExtras(string carga)
        {
            SqlCommand cmd = new SqlCommand(
                @"SELECT Motorista, Veiculo, StatusTransporte, Latitude, Longitude, 
                     DistanciaKm, TempoEstimado, TelemetriaDados 
              FROM Rastreamento WHERE NumeroCarga = @c", conn);

            cmd.Parameters.AddWithValue("@c", carga);

            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                lblMotorista.Text = "Motorista: " + dr["Motorista"].ToString();
                lblVeiculo.Text = "Veículo: " + dr["Veiculo"].ToString();
                lblStatus.Text = "Status: " + dr["StatusTransporte"].ToString();
                lblTelemetria.Text = "Telemetria: " + dr["TelemetriaDados"].ToString();

                // Enviar coordenadas para JavaScript renderizar o mapa
                ClientScript.RegisterStartupScript(this.GetType(), "mapa",
                    "initMap(" + dr["Latitude"] + "," + dr["Longitude"] + ");", true);
            }

            conn.Close();
        }

        // Enviar rota via WhatsApp
        protected void btnWhats_ServerClick(object sender, EventArgs e)
        {
            string telefone = "55999999999";
            string mensagem = "Segue a rota da carga " + txtCarga.Text;

            Response.Redirect("https://wa.me/" + telefone + "?text=" + Server.UrlEncode(mensagem));
        }
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public static object GetRota(string num)
        {
            // pega Origem, Destino, Paradas
            using (var cn = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
            {
                string sql = "SELECT cliorigem, clidestino, paradas FROM tbcargas WHERE carga = @num";
                using (var cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@num", num);
                    cn.Open();
                    var r = cmd.ExecuteReader();
                    if (r.Read())
                    {
                        var paradasRaw = r["paradas"]?.ToString() ?? "";
                        return new
                        {
                            Origem = r["cliorigem"]?.ToString(),
                            Destino = r["clidestino"]?.ToString(),
                            Paradas = paradasRaw == "" ? new string[0] : paradasRaw.Split(';')
                        };
                    }
                }
            }
            return null;
        }

    }


}