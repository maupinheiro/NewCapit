using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.Script.Serialization;
using System.Web.Configuration;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using System.Drawing;
using System.Configuration;



namespace NewCapit.dist.pages
{
    public partial class Home : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conexao"].ToString());
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregaBloco();
                CarregaGraficos();
                lblData.Text = DateTime.Now.AddDays(-7).ToString("dd/MM/yyyy") + " - " + DateTime.Now.ToString("dd/MM/yyyy");
            }
        }
        public void CarregaBloco()
        {
            string html;
            html = "<div class='col-lg-3 col-6'>";
            html += "<div class='small-box bg-info'>";
            html += "<div class='inner'>";
            html += "<h3>150</h3>";
            html += "<p>Total de Entregas</p>";
            html += "</div>";
            html += "<div class='icon'>";
            html += "<i class='fas fa-truck'></i>";
            html += "</div>";
            html += "</div>";
            html += "</div>";
            html += "<div class='col-lg-3 col-6'>";
            html += "<div class='small-box bg-success'>";
            html += "<div class='inner'>";
            html += "<h3>53</h3>";
            html += "<p>Entregas em Andamento</p>";
            html += "</div>";
            html += "<div class='icon'>";
            html += "<i class='fas fa-shipping-fast'></i>";
            html += "</div>";
            html += "</div>";
            html += "</div>";
            html += "<div class='col-lg-3 col-6'>";
            html += "<div class='small-box bg-warning'>";
            html += "<div class='inner'>";
            html += "<h3>44</h3>";
            html += "<p>Entregas Concluidas</p>";
            html += "</div>";
            html += "<div class='icon'>";
            html += "<i class='fas fa-warehouse'></i>";
            html += "</div>";
            html += "</div>";
            html += "</div>";
            html += "<div class='col-lg-3 col-6'>";
            html += "<div class='small-box bg-danger'>";
            html += "<div class='inner'>";
            html += "<h3>65</h3>";
            html += "<p>Entregas Pendentes</p>";
            html += "</div>";
            html += "<div class='icon'>";
            html += "<i class='far fa-calendar-alt'></i>";
            html += "</div>";
            html += "</div>";
            html += "</div>";
            HtmlGenericControl table = this.blocos ;
            table.InnerHtml = html;
        }
        public void CarregaGraficos()
        {
            string html = @"
                <script src='https://code.jquery.com/jquery-3.6.0.min.js'></script>
                <script src='https://cdn.jsdelivr.net/npm/chart.js'></script>
                <script>
                    $(document).ready(function () {
                        var ctx = $('#graficoDesempenhoVeiculos')[0].getContext('2d');
                        new Chart(ctx, {
                            type: 'bar',
                            data: {
                                labels: ['Frota', 'Agregados', 'Terceiros'],
                                datasets: [{
                                    data: [200, 350, 100],
                                    backgroundColor: [
                                        'rgba(255, 255, 0, 1)',
                                        'rgba(72, 209, 204, 1)',
                                        'rgba(148, 0, 211, 1)'
                                    ],
                                    borderColor: [
                                        'rgba(255, 255, 0, 1)',
                                        'rgba(72, 209, 204, 1)',
                                        'rgba(148, 0, 211, 1)'
                                    ],
                                    borderWidth: 1
                                }]
                            },
                            options: {
                                legend: { display: false },
                                scales: {
                                    yAxes: [{
                                        ticks: { beginAtZero: true }
                                    }]
                                }
                            }
                        });
                    });
                </script>";

            ClientScript.RegisterStartupScript(this.GetType(), "graficoScript", html, false);

            string html1 = @"
            <script src='https://code.jquery.com/jquery-3.6.0.min.js'></script>
            <script src='https://cdn.jsdelivr.net/npm/chart.js'></script>

            <script>
                document.addEventListener('DOMContentLoaded', function () {
                    var ctx = document.getElementById('graficoDesempenhoFrota').getContext('2d');

                    var desempenhoFrotaAreaChartData = {
                        labels: ['Domingo', 'Segunda', 'Terça', 'Quarta', 'Quinta', 'Sexta', 'Sábado'],
                        datasets: [
                            {
                                label: 'Em Operação',
                                backgroundColor: 'rgba(0,128,0,0.9)',
                                borderColor: 'rgba(0,128,0,0.8)',
                                pointRadius: false,
                                data: [28, 48, 40, 19, 86, 27, 90]
                            },
                            {
                                label: 'Na Oficina',
                                backgroundColor: 'rgba(139, 0, 0, 1)',
                                borderColor: 'rgba(139, 0, 0, 1)',
                                pointRadius: false,
                                data: [65, 59, 80, 81, 56, 55, 40]
                            }
                        ]
                    };

                    var desempenhoFrotaAreaChartOptions = {
                        maintainAspectRatio: false,
                        responsive: true,
                        plugins: {
                            legend: { display: true }
                        },
                        scales: {
                            x: { grid: { display: false } },
                            y: { grid: { display: false } }
                        }
                    };

                    new Chart(ctx, {
                        type: 'bar',
                        data: desempenhoFrotaAreaChartData,
                        options: desempenhoFrotaAreaChartOptions
                    });
                });
            </script>";

            ClientScript.RegisterStartupScript(this.GetType(), "graficoFrotaScript", html1, false);

            string sqlm = "WITH Ranking AS (SELECT nomemotorista, codmotorista, COUNT(*) AS total_entregas, RANK() OVER (ORDER BY COUNT(*) DESC) AS rank_posicao FROM tbcarregamentos WHERE MONTH(emissao) = 02 AND YEAR(emissao) = 2024  GROUP BY nomemotorista, codmotorista) ";
            sqlm += "SELECT * FROM Ranking WHERE rank_posicao <= 10 AND codmotorista NOT LIKE '%[^0-9]%'";
            SqlDataAdapter adptm = new SqlDataAdapter(sqlm, con);
            DataTable dtm = new DataTable();
            con.Open();
            adptm.Fill(dtm);
            con.Close();
            string html2 = @"
                <script src='https://code.jquery.com/jquery-3.6.0.min.js'></script>
                <script src='https://cdn.jsdelivr.net/npm/chart.js'></script>

                <script>
                    document.addEventListener('DOMContentLoaded', function () {
                        var ctx = document.getElementById('bar-chart').getContext('2d');

                        var pieData = {
                            labels: [
                                {0}
                            ],
                            datasets: [
                                {
                                    data: [{1}],
                                    backgroundColor: ['#f56954', '#00a65a', '#f39c12', '#00c0ef', '#3c8dbc', '#7401DF', '#ffff00', '#2e9afe', '#ff00ff', '#F5DEB3']
                                }
                            ]
                        };

                        var pieOptions = {
                            maintainAspectRatio: false,
                            responsive: true,
                            plugins: {
                                legend: { display: false }
                            },
                            scales: {
                                y: {
                                    beginAtZero: true
                                }
                            }
                        };

                        new Chart(ctx, {
                            type: 'bar',
                            data: pieData,
                            options: pieOptions
                        });
                    });
                </script>";
            StringBuilder sb2 = new StringBuilder(html2);
            string rankmotoristas = string.Join(", ", dtm.AsEnumerable().Select(row => $"'{row["nomemotorista"].ToString()}'")); // Adiciona aspas simples
            sb2.Replace("{0}", rankmotoristas);
            string numeroviagens = string.Join(", ", dtm.AsEnumerable().Select(row => row["total_entregas"].ToString()));
            sb2.Replace("{1}", numeroviagens); // Substitui {1} pelos valores do DataTable 'Solicitações VW'


            ClientScript.RegisterStartupScript(this.GetType(), "barChartScript", sb2.ToString(), false);

            string html3 = @"
                <script src='https://code.jquery.com/jquery-3.6.0.min.js'></script>
                <script src='https://cdn.jsdelivr.net/npm/chart.js'></script>

                <script>
                    document.addEventListener('DOMContentLoaded', function () {
                        var ctx = document.getElementById('bar-chart-veiculos').getContext('2d');

                        var pieData = {
                            labels: [
                                '256', '214', '156', '258', '163', '232', '225', '253', '257', '218'
                            ],
                            datasets: [
                                {
                                    data: [25, 100, 40, 66, 50, 20, 10, 15, 18, 110],
                                    backgroundColor: ['#f56954', '#00a65a', '#f39c12', '#00c0ef', '#3c8dbc', '#7401DF', '#ffff00', '#2e9afe', '#ff00ff', '#F5DEB3']
                                }
                            ]
                        };

                        var pieOptions = {
                            maintainAspectRatio: false,
                            responsive: true,
                            plugins: {
                                legend: {
                                    display: true,
                                    position: 'right'
                                }
                            }
                        };

                        new Chart(ctx, {
                            type: 'pie',
                            data: pieData,
                            options: pieOptions
                        });
                    });
                </script>";

            ClientScript.RegisterStartupScript(this.GetType(), "barChartVeiculosScript", html3, false);
            string sqlw = @"SELECT DATENAME(weekday, emissao) AS DiaDaSemana, COUNT(*) AS QuantidadeDeViagens FROM tbcarregamentos WHERE nomclidestino LIKE 'VW%' AND emissao BETWEEN DATEADD(week, -60, GETDATE()) AND GETDATE()  --Intervalo de uma semana
             GROUP BY DATENAME(weekday, emissao) ORDER BY  CASE DATENAME(weekday, emissao) WHEN 'Sunday' THEN 1 WHEN 'Monday' THEN 2 WHEN 'Tuesday' THEN 3 WHEN 'Wednesday' THEN 4 WHEN 'Thursday' THEN 5 WHEN 'Friday' THEN 6  WHEN 'Saturday' THEN 7 END";
            SqlDataAdapter adptTotal = new SqlDataAdapter(sqlw, con);
            DataTable dtw = new DataTable();
            con.Open();
            adptTotal.Fill(dtw);
            con.Close();
            string sqlo = @"SELECT DATENAME(weekday, emissao) AS DiaDaSemana, COUNT(*) AS QuantidadeDeViagens FROM tbcarregamentos WHERE nomclidestino NOT LIKE 'VW%' AND emissao BETWEEN DATEADD(week, -60, GETDATE()) AND GETDATE()  --Intervalo de uma semana
            GROUP BY DATENAME(weekday, emissao) ORDER BY  CASE DATENAME(weekday, emissao) WHEN 'Sunday' THEN 1 WHEN 'Monday' THEN 2 WHEN 'Tuesday' THEN 3 WHEN 'Wednesday' THEN 4 WHEN 'Thursday' THEN 5 WHEN 'Friday' THEN 6  WHEN 'Saturday' THEN 7 END";
            SqlDataAdapter adptTotalo = new SqlDataAdapter(sqlo, con);
            DataTable dto = new DataTable();
            con.Open();
            adptTotalo.Fill(dto);
            con.Close();
            string html4 = @" <script>
                    $(function () {
                        /* ChartJS
                         * -------
                         * Here we will create a few charts using ChartJS
                         */

                        //--------------
                        //- AREA CHART -
                        //--------------

                        // Get context with jQuery - using jQuery's .get() method.
                        var areaChartCanvas = $('#areaChart').get(0).getContext('2d')

                        var areaChartData = {
                            labels: ['Domingo', 'Segunda', 'Terça', 'Quarta', 'Quinta', 'Sexta', 'Sábado'],
                            datasets: [
                                {
                                    label: 'Outros',
                                    backgroundColor: 'rgba(60,141,188,0.9)',
                                    borderColor: 'rgba(60,141,188,0.8)',
                                    pointRadius: false,
                                    pointColor: '#3b8bba',
                                    pointStrokeColor: 'rgba(60,141,188,1)',
                                    pointHighlightFill: '#fff',
                                    pointHighlightStroke: 'rgba(60,141,188,1)',
                                    data: [{0}]
                                },
                                {
                                    label: 'Solicitações VW',
                                    backgroundColor: 'rgba(210, 214, 222, 1)',
                                    borderColor: 'rgba(210, 214, 222, 1)',
                                    pointRadius: false,
                                    pointColor: 'rgba(210, 214, 222, 1)',
                                    pointStrokeColor: '#c1c7d1',
                                    pointHighlightFill: '#fff',
                                    pointHighlightStroke: 'rgba(220,220,220,1)',
                                    data: [{1}]
                                },
                            ]
                        }

                        var areaChartOptions = {
                            maintainAspectRatio: false,
                            responsive: true,
                            legend: {
                                display: false
                            },
                            scales: {
                                xAxes: [{
                                    gridLines: {
                                        display: false,
                                    }
                                }],
                                yAxes: [{
                                    gridLines: {
                                        display: false,
                                    }
                                }]
                            }
                        }

                        // This will get the first returned node in the jQuery collection.
                        new Chart(areaChartCanvas, {
                            type: 'line',
                            data: areaChartData,
                            options: areaChartOptions
                        })



                        //-------------
                        //- DONUT CHART - Carregamento
                        //-------------
                        // Get context with jQuery - using jQuery's .get() method.
                        var donutChartCanvas = $('#donutChart').get(0).getContext('2d')
                        var donutData = {
                            labels: [
                                'Ag. Carreg.',
                                'Em Transito',
                                'Ag. Descarga',
                                'Concluida',
                                'Outros',
                            ],
                            datasets: [
                                {
                                    data: [700, 500, 400, 600, 300],
                                    backgroundColor: ['#f56954', '#00a65a', '#f39c12', '#00c0ef', '#3c8dbc'],
                                }
                            ]
                        }
                        var donutOptions = {
                            maintainAspectRatio: false,
                            responsive: true,
                            legend: {
                                display: true,
                                position: 'right',
                            },

                        }
                        //Create pie or douhnut chart
                        // You can switch between pie and douhnut using the method below.
                        new Chart(donutChartCanvas, {
                            type: 'doughnut',
                            data: donutData,
                            options: donutOptions
                        })

                        //-------------
                        //- PIE CHART - o
                        //-------------
                        // Get context with jQuery - using jQuery's .get() method.
                        var pieChartCanvas = $('#pieChart').get(0).getContext('2d')
                        var pieData = {
                            labels: [
                                'Matriz.',
                                'CNT',
                                'SBC',
                                'Taubaté',
                                'Ipiranga',
                                'São Carlos',
                                'PR',
                                'PE',
                                'MG',
                            ],
                            datasets: [
                                {
                                    data: [700, 500, 400, 600, 300, 200, 100, 150, 180],
                                    backgroundColor: ['#f56954', '#00a65a', '#f39c12', '#00c0ef', '#3c8dbc', '#7401DF', '#ffff00', '#2e9afe', '#ff00ff'],
                                    borderWidth: 1,
                                    borderColor: '#777',
                                    hoverBorderWidht: 2,
                                    hoverBorderColor: '#000',
                                }
                            ]
                        }
                        var pieOptions = {
                            maintainAspectRatio: false,
                            responsive: true,
                            legend: {
                                display: false
                            },
                            scales: {
                                yAxes: [{
                                    ticks: {
                                        beginAtZero: true
                                    }
                                }]

                            }
                        }
                        //Create pie or douhnut chart
                        // You can switch between pie and douhnut using the method below.
                        new Chart(pieChartCanvas, {
                            type: 'bar',
                            data: pieData,
                            options: pieOptions
                        })

                        //-------------
                        //- BAR CHART -
                        //-------------
                        var barChartCanvas = $('#barChart').get(0).getContext('2d')
                        var barChartData = $.extend(true, {}, areaChartData)
                        var temp0 = areaChartData.datasets[0]
                        var temp1 = areaChartData.datasets[1]
                        barChartData.datasets[0] = temp1
                        barChartData.datasets[1] = temp0

                        var barChartOptions = {
                            responsive: true,
                            maintainAspectRatio: false,
                            datasetFill: false
                        }

                        new Chart(barChartCanvas, {
                            type: 'bar',
                            data: barChartData,
                            options: barChartOptions
                        })
                    })
            </script>";
            StringBuilder sb = new StringBuilder(html4);
            string outrosDataValues = string.Join(", ", dto.AsEnumerable().Select(row => row["QuantidadeDeViagens"].ToString()));
            sb.Replace("{0}", outrosDataValues); // Substitui {0} pelos valores do DataTable 'Outros'
            string solicitacoesVWDataValues = string.Join(", ", dtw.AsEnumerable().Select(row => row["QuantidadeDeViagens"].ToString()));
            sb.Replace("{1}", solicitacoesVWDataValues); // Substitui {1} pelos valores do DataTable 'Solicitações VW'
            ClientScript.RegisterStartupScript(this.GetType(), "", sb.ToString(), false);


        }

        protected void lnkMapa_Click(object sender, EventArgs e)
        {
            Response.Redirect("MapaVeiculo.aspx");
        }
    }
}