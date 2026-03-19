<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="DashboardManutencao.aspx.cs" Inherits="NewCapit.dist.pages.DashboardManutencao" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- Bootstrap 5 -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <!-- Chart.js -->
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>

    <script src="https://cdn.datatables.net/1.13.6/js/jquery.dataTables.min.js"></script>
    <link href="https://cdn.datatables.net/1.13.6/css/jquery.dataTables.min.css" rel="stylesheet" />
    <script src="https://code.jquery.com/jquery-3.7.0.min.js"></script>

    <link href="https://cdn.jsdelivr.net/npm/select2@4.1.0/dist/css/select2.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/select2@4.1.0/dist/js/select2.min.js"></script>

    <script>
        $(document).ready(function () {
            $('.select2').select2({
                width: '100%',
                placeholder: "Selecione ou pesquise..."
            });
        });
    </script>


    <div class="content-wrapper">
        <section class="content">
            <div class="container-fluid">
                <br />
                <div id="toastContainerVermelho" class="alert alert-danger alert-dismissible" style="display: none;">
                    <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                    <h5><i class="icon fas fa-exclamation-triangle"></i>Alerta!</h5>
                    Alertas
                </div>
                <div class="col-md-12">
                    <div class="card card-info">
                        <div class="card-header" style="background-color: #A020F0; font-weight: bold;">
                            <h3 class="card-title">
                                <h3 class="card-title"><i class="fas fa-shipping-fast"></i>&nbsp;Dashboard Manutenção</h3>
                            </h3>
                            <div class="card-tools">
                                <button type="button" class="btn btn-tool" data-card-widget="maximize">
                                    <i class="fas fa-expand"></i>
                                </button>
                                <button type="button" class="btn btn-tool" data-card-widget="collapse">
                                    <i class="fas fa-minus"></i>
                                </button>
                                <button type="button" class="btn btn-tool" data-card-widget="remove">
                                    <i class="fas fa-times"></i>
                                </button>
                            </div>
                            <!-- /.card-tools -->
                        </div>
                        <div class="card-body">   
                                <!-- CARDS SUPERIORES -->
                                <div class="row mb-4">

                                    <div class="col-md-3">
                                        <div class="card bg-warning text-white shadow">
                                            <div class="card-body">
                                                <h6>OS Abertas</h6>
                                                <asp:Label ID="lblAbertas" runat="server" Font-Size="Large"></asp:Label>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-md-3">
                                        <div class="card bg-success text-white shadow">
                                            <div class="card-body">
                                                <h6>OS Finalizadas (Mês)</h6>
                                                <asp:Label ID="lblFinalizadas" runat="server" Font-Size="Large"></asp:Label>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-md-3">
                                        <div class="card bg-info text-white shadow">
                                            <div class="card-body">
                                                <h6>Custo Manutenção (Mês)</h6>
                                                <asp:Label ID="lblCustoMes" runat="server" Font-Size="Large"></asp:Label>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-md-3">
                                        <div class="card bg-danger text-white shadow">
                                            <div class="card-body">
                                                <h6>Preventivas Vencidas</h6>
                                                <asp:Label ID="lblPreventivas" runat="server" Font-Size="Large"></asp:Label>
                                            </div>
                                        </div>
                                    </div>

                                </div>

                                <!-- GRÁFICOS -->
                                <div class="row">

                                    <div class="col-md-6">
                                        <div class="card shadow">
                                            <div class="card-header bg-light">
                                                Custos por Mês
                                            </div>
                                            <div class="card-body">
                                                <canvas id="chartCustos"></canvas>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-md-6">
                                        <div class="card shadow">
                                            <div class="card-header bg-light">
                                                OS por Status
                                            </div>
                                            <div class="card-body">
                                                <canvas id="chartStatus"></canvas>
                                            </div>
                                        </div>
                                    </div>

                                </div>

                                <div class="row mt-4">

                                    <div class="col-md-12">
                                        <div class="card shadow">
                                            <div class="card-header bg-light">
                                                Consumo Médio de Diesel
                                            </div>
                                            <div class="card-body">
                                                <canvas id="chartDiesel"></canvas>
                                            </div>
                                        </div>
                                    </div>

                                </div>

                                <!-- ChartJS -->
                                <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>

                                <script>

                                    var custos = <%= jsonCustos %>;
                                    var status = <%= jsonStatus %>;
                                    var diesel = <%= jsonDiesel %>;

                                    // GRÁFICO CUSTOS
                                    new Chart(document.getElementById("chartCustos"), {
                                        type: 'bar',
                                        data: {
                                            labels: custos.labels,
                                            datasets: [{
                                                label: 'R$',
                                                data: custos.valores,
                                                backgroundColor: '#007bff'
                                            }]
                                        }
                                    });

                                    // GRÁFICO STATUS
                                    new Chart(document.getElementById("chartStatus"), {
                                        type: 'doughnut',
                                        data: {
                                            labels: status.labels,
                                            datasets: [{
                                                data: status.valores,
                                                backgroundColor: ['#ffc107', '#17a2b8', '#28a745']
                                            }]
                                        }
                                    });

                                    // GRÁFICO DIESEL
                                    new Chart(document.getElementById("chartDiesel"), {
                                        type: 'line',
                                        data: {
                                            labels: diesel.labels,
                                            datasets: [{
                                                label: 'Km/L',
                                                data: diesel.valores,
                                                borderColor: '#dc3545',
                                                fill: false
                                            }]
                                        }
                                    });

                                </script>
                          
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>


</asp:Content>
