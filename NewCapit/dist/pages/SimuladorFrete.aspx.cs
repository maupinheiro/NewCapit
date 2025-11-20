using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using System.Web.Configuration;

namespace NewCapit.dist.pages
{
    public partial class SimuladorFrete : System.Web.UI.Page
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["conexao"].ToString());
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
                DateTime dataHoraAtual = DateTime.Now;
                CarregarModais();
                CarregarGrid();
            }

        }
        private void CarregarModais()
        {
            // using (SqlConnection con = new SqlConnection(conn))
            using (SqlCommand cmd = new SqlCommand("SELECT Id, Modal FROM tbmodalfrete", conn))
            {
                conn.Open();
                ddlModal.DataSource = cmd.ExecuteReader();
                ddlModal.DataTextField = "Modal";
                ddlModal.DataValueField = "Id";
                ddlModal.DataBind();
            }
        }
        protected void btnDistancia_Click(object sender, EventArgs e)
        {
            try
            {
                string origem = txtOrigem.Text.Trim();
                string destino = txtDestino.Text.Trim();
                string key = "AIzaSyApI6da0E4OJktNZ-zZHgL6A5jtk0L6Cww";

                string url = "https://routes.googleapis.com/directions/v2:computeRoutes";

                string jsonBody = $@"
        {{
            ""origin"": {{ ""address"": ""{origem}"" }},
            ""destination"": {{ ""address"": ""{destino}"" }},
            ""travelMode"": ""DRIVE""
        }}";

                using (var client = new WebClient())
                {
                    client.Headers.Add("Content-Type", "application/json");
                    client.Headers.Add("X-Goog-Api-Key", key);
                    client.Headers.Add("X-Goog-FieldMask", "routes.distanceMeters");

                    string response = client.UploadString(url, "POST", jsonBody);

                    dynamic data = JsonConvert.DeserializeObject(response);

                    double metros = data.routes[0].distanceMeters;
                    double km = metros / 1000;

                    txtDistancia.Text = km.ToString("0.##");
                }
            }
            catch
            {
                txtDistancia.Text = "0";
            }
        }
        protected void btnCalcular_Click(object sender, EventArgs e)
        {
            int modalId = int.Parse(ddlModal.SelectedValue);
            double distancia = double.Parse(txtDistancia.Text);
            double peso = double.Parse(txtPeso.Text);

            var dados = ObterModal(modalId);

            double custoKm = distancia * dados.CustoPorKm;
            double seguro = peso * (dados.SeguroPercentual / 100);
            double gris = peso * (dados.GrisPercent / 100);
            double combustivel = distancia * (dados.AdicionalCombustivel / 100);

            double total = custoKm + combustivel + seguro + gris + dados.TaxaFixa;

            if (total < dados.TakMinima)
                total = dados.TakMinima;

            lblResultado.Text = $"Frete final: <b>R$ {total:F2}</b>";
            conn.Close();
            CarregarComparativo(distancia, peso);
        }
        private void CarregarComparativo(double distancia, double peso)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Modal");
            dt.Columns.Add("Valor");

            // using (SqlConnection con = new SqlConnection(cs))
            using (SqlCommand cmd = new SqlCommand("SELECT * FROM tbmodalfrete", conn))
            {
                conn.Open();
                var r = cmd.ExecuteReader();

                while (r.Read())
                {
                    double custoKm = distancia * Convert.ToDouble(r["CustoPorKm"]);
                    double seguro = peso * (Convert.ToDouble(r["SeguroPercentual"]) / 100);
                    double gris = peso * (Convert.ToDouble(r["GrisPercent"]) / 100);
                    double combustivel = distancia * (Convert.ToDouble(r["AdicionalCombustivel"]) / 100);
                    double taxaFixa = Convert.ToDouble(r["TaxaFixa"]);
                    double minima = Convert.ToDouble(r["TakMinima"]);

                    double total = custoKm + combustivel + seguro + gris + taxaFixa;
                    if (total < minima) total = minima;

                    dt.Rows.Add(r["Modal"].ToString(), total.ToString("F2"));
                }

                gridComparativo.DataSource = dt;
                gridComparativo.DataBind();
            }
        }
        private (double CustoPorKm, double TaxaFixa, double SeguroPercentual,
                double AdicionalCombustivel, double TakMinima, double GrisPercent)
            ObterModal(int id)
        {
            // using (SqlConnection con = new SqlConnection(conn))
            using (SqlCommand cmd = new SqlCommand("SELECT * FROM tbmodalfrete WHERE Id=@id", conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();
                var r = cmd.ExecuteReader();

                r.Read();

                return (
                    Convert.ToDouble(r["CustoPorKm"]),
                    Convert.ToDouble(r["TaxaFixa"]),
                    Convert.ToDouble(r["SeguroPercentual"]),
                    Convert.ToDouble(r["AdicionalCombustivel"]),
                    Convert.ToDouble(r["TakMinima"]),
                    Convert.ToDouble(r["GrisPercent"])
                );
            }
        }
        private void CarregarGrid()
        {


            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                string sql = "SELECT Id, Modal, CustoPorKm, TaxaFixa, SeguroPercentual, AdicionalCombustivel, TakMinima, GrisPercent FROM tbmodalfrete";
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();

                GridView1.DataSource = cmd.ExecuteReader();
                GridView1.DataBind();
            }
        }
        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            CarregarGrid();
        }
        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            CarregarGrid();
        }
        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value);

            GridViewRow row = GridView1.Rows[e.RowIndex];

            //string modal = ((TextBox)row.FindControl("txtModal")).Text;
            //int quantidade = int.Parse(((TextBox)row.FindControl("txtQtd")).Text);
            decimal custoporkm = decimal.Parse(((TextBox)row.FindControl("txtCustoPorKm")).Text);
            decimal taxafixa = decimal.Parse(((TextBox)row.FindControl("txtTaxaFixa")).Text);
            decimal seguropercentual = decimal.Parse(((TextBox)row.FindControl("txtSeguroPercentual")).Text);
            decimal adicionalcombustivel = decimal.Parse(((TextBox)row.FindControl("txtAdicionalCombustivel")).Text);
            decimal takminima = decimal.Parse(((TextBox)row.FindControl("txtTakMinima")).Text);
            decimal grispercent = decimal.Parse(((TextBox)row.FindControl("txtGrisPercent")).Text);



            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                string sql = @"UPDATE tbmodalfrete
                       SET CustoPorKm = @CustoPorKm,
                           TaxaFixa = @TaxaFixa,
                           SeguroPercentual = @SeguroPercentual,
                           AdicionalCombustivel = @AdicionalCombustivel,
                           TakMinima = @TakMinima,
                           GrisPercent = @GrisPercent
                       WHERE Id = @Id";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Id", id);                
                cmd.Parameters.AddWithValue("@CustoPorKm", custoporkm);
                cmd.Parameters.AddWithValue("@TaxaFixa", taxafixa);
                cmd.Parameters.AddWithValue("@SeguroPercentual", seguropercentual);
                cmd.Parameters.AddWithValue("@AdicionalCombustivel", adicionalcombustivel);
                cmd.Parameters.AddWithValue("@TakMinima", takminima);
                cmd.Parameters.AddWithValue("@GrisPercent", grispercent);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            GridView1.EditIndex = -1;
            CarregarGrid();

            // MOSTRAR TOAST Bootstrap
            ScriptManager.RegisterStartupScript(this, GetType(),
                "popup", "showToast('Dados atualizados com sucesso!');", true);
        }
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                foreach (Button button in e.Row.Cells[0].Controls.OfType<Button>())
                {
                    if (button.CommandName == "Edit")
                        button.CssClass = "btn btn-warning btn-sm";
                    else if (button.CommandName == "Update")
                        button.CssClass = "btn btn-success btn-sm";
                    else if (button.CommandName == "Cancel")
                        button.CssClass = "btn btn-secondary btn-sm";
                }
            }
        }

    }
}