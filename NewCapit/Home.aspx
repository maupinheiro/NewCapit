<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="NewCapit.Home1" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- Bootstrap -->
<link href="vendor/bootstrap/dist/css/bootstrap.min.css" rel="stylesheet">
<!-- Font Awesome -->
<!-- NProgres -->
<!-- iCheck -->

<!-- bootstrap-progressbar -->
<!-- JQVMap -->

<!-- bootstrap-daterangepicker -->
   

<!-- Custom Theme Style -->
<link href="build/css/custom.min.css" rel="stylesheet">
    <!-- Page Heading -->
    <div class="d-sm-flex align-items-left justify-content-between mb-4">
        <h1 class="h3 mb-0 text-gray-800">Visão Geral das Filiais</h1>
        <a href="#" class="d-none d-sm-inline-block btn btn-sm btn-primary shadow-sm">
            <i class="fas fa-map-marker-alt fa-sm text-white-50"></i>&nbsp;Veículos no Mapa
        </a>
    </div>
    <!-- Content Row -->
    <div class="row">
        <!-- Total Entregas -->
        <div class="col-xl-3 col-md-6 mb-4">
            <div class="card border-left-primary shadow h-100 py-2">
                <div class="card-body">
                    <div class="row no-gutters align-items-center">
                        <div class="col mr-2">
                            <div class="text-xs font-weight-bold text-primary text-uppercase mb-1">
                                Total de Entregas
                            </div>
                            <div class="h5 mb-0 font-weight-bold text-gray-800">30</div>
                        </div>
                        <div class="col-auto">
                            <i class="fas fa-truck fa-2x text-gray-300"></i>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Entregas em Andamento -->
        <div class="col-xl-3 col-md-6 mb-4">
            <div class="card border-left-success shadow h-100 py-2">
                <div class="card-body">
                    <div class="row no-gutters align-items-center">
                        <div class="col mr-2">
                            <div class="text-xs font-weight-bold text-success text-uppercase mb-1">
                                Entregas em Andamento
                            </div>
                            <div class="h5 mb-0 font-weight-bold text-gray-800">12</div>
                        </div>
                        <div class="col-auto">
                            <i class="fas fa-calendar fa-2x text-gray-300"></i>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Entregas Concluidas -->
        <div class="col-xl-3 col-md-6 mb-4">
            <div class="card border-left-info shadow h-100 py-2">
                <div class="card-body">
                    <div class="row no-gutters align-items-center">
                        <div class="col mr-2">
                            <div class="text-xs font-weight-bold text-info text-uppercase mb-1">
                                Entregas Concluidas
                                           
                            </div>
                            <div class="row no-gutters align-items-center">
                                <div class="col-auto">
                                    <div class="h5 mb-0 mr-3 font-weight-bold text-gray-800">40%</div>
                                </div>
                                <div class="col">
                                    <div class="progress progress-sm mr-2">
                                        <div class="progress-bar bg-info" role="progressbar"
                                            style="width: 40%" aria-valuenow="40" aria-valuemin="0"
                                            aria-valuemax="100">
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-auto">
                            <i class="fas fa-industry fa-2x text-gray-300"></i>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Entregas Pendentes -->
        <div class="col-xl-3 col-md-6 mb-4">
            <div class="card border-left-warning shadow h-100 py-2">
                <div class="card-body">
                    <div class="row no-gutters align-items-center">
                        <div class="col mr-2">
                            <div class="text-xs font-weight-bold text-warning text-uppercase mb-1">
                                Entregas Pendentes
                            </div>
                            <div class="h5 mb-0 font-weight-bold text-gray-800">15</div>
                        </div>
                        <div class="col-auto">
                            <i class="fas fa-clock fa-2x text-gray-300"></i>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- Graficos -->
    <div class="col-md-12 col-sm-12 col-xs-12">
      <div class="row">
      <div class="col-md-12 col-sm-12 col-xs-12">
          <div class="dashboard_graph">

              <div class="row x_title">
                  <div class="col-md-8">
                      <h3>Resumo Atividades <small>TRANSNOVAG TRANSPORTES</small></h3>
                  </div>

                  <div class="col-md-2">
                      <div id="data" class="pull-right">
                          <div class="form-group">
                              <input type="date" class="form-control" id="input_inicial" ali>
                          </div>
                      </div>
                  </div>
                  <div class="col-md-2">
                      <div id="data" class="pull-right">
                          <div class="form-group">
                              <input type="date" class="form-control" id="input_final" ">
                          </div>
                      </div>
                  </div>
              </div>

              <!--  graficos de entregas por dias -->
              <div class="col-md-12 col-sm-12 col-xs-12">
                  <div id="chart_plot_02" class="demo-placeholder"></div>
              </div>
              <br>

          </div>
          <br />

          <!-- Peformance das Filiais percentual do total de entregas concluidas de todas filiais e quantidade de entregas concluidas de cada filial -->
          <div class="col-md-4 col-sm-4 col-xs-12">
              <div class="x_panel tile fixed_height_450">
                  <div class="x_title">
                      <h2>Peformance das Filiais:</h2>
                      <ul class="nav navbar-right panel_toolbox">
                          <li>
                              <a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                          </li>
                          <li>
                              <a class="close-link"><i class="fa fa-close"></i></a>
                          </li>
                      </ul>
                      <div class="clearfix"></div>
                  </div>

                  <!-- Matriz -->
                  <div class="x_content">
                      <div class="widget_summary">
                          <div class="w_left w_25">
                              <span>Matriz</span>
                          </div>
                          <div class="w_center w_55">
                              <div class="progress">
                                  <div class="progress-bar bg-PaleTurquoise" role="progressbar" aria-valuenow="60" aria-valuemin="0" aria-valuemax="100" style="width: 100%;">
                                      <span class="sr-only">100% Complete</span>
                                  </div>
                              </div>
                          </div>
                          <div class="w_right w_20">
                              <span>1.230</span>
                          </div>
                          <div class="clearfix"></div>
                      </div>

                      <!-- Minas -->
                      <div class="widget_summary">
                          <div class="w_left w_25">
                              <span>Minas</span>
                          </div>
                          <div class="w_center w_55">
                              <div class="progress">
                                  <div class="progress-bar bg-Yellow" role="progressbar" aria-valuenow="10" aria-valuemin="0" aria-valuemax="100" style="width: 10%;">
                                      <span class="sr-only">10% Complete</span>
                                  </div>
                              </div>
                          </div>
                          <div class="w_right w_20">
                              <span>53</span>
                          </div>
                          <div class="clearfix"></div>
                      </div>

                      <!-- Pernambuco -->
                      <div class="widget_summary">
                          <div class="w_left w_25">
                              <span>Pernambuco</span>
                          </div>
                          <div class="w_center w_55">
                              <div class="progress">
                                  <div class="progress-bar bg-PaleTurquoise" role="progressbar" aria-valuenow="35" aria-valuemin="0" aria-valuemax="100" style="width: 35%;">
                                      <span class="sr-only">35% Complete</span>
                                  </div>
                              </div>
                          </div>
                          <div class="w_right w_20">
                              <span>35</span>
                          </div>
                          <div class="clearfix"></div>
                      </div>

                      <!-- CNT -->
                      <div class="widget_summary">
                          <div class="w_left w_25">
                              <span>CNT</span>
                          </div>
                          <div class="w_center w_55">
                              <div class="progress">
                                  <div class="progress-bar bg-paleturquoise" " role="progressbar" aria-valuenow="60" aria-valuemin="0" aria-valuemax="100" style="width: 5%;">
                                      <span class="sr-only">60% Complete</span>
                                  </div>
                              </div>
                          </div>
                          <div class="w_right w_20">
                              <span>3k</span>
                          </div>
                          <div class="clearfix"></div>
                      </div>

                      <!-- Taubaté -->
                      <div class="widget_summary">
                          <div class="w_left w_25">
                              <span>Taubaté</span>
                          </div>
                          <div class="w_center w_55">
                              <div class="progress">
                                  <div class="progress-bar bg-PaleTurquoise" " role="progressbar" aria-valuenow="60" aria-valuemin="0" aria-valuemax="100" style="width: 2%;">
                                      <span class="sr-only">60% Complete</span>
                                  </div>
                              </div>
                          </div>
                          <div class="w_right w_20">
                              <span>1k</span>
                          </div>
                          <div class="clearfix"></div>
                      </div>

                      <!-- SBC -->
                      <div class="widget_summary">
                          <div class="w_left w_25">
                              <span>SBC</span>
                          </div>
                          <div class="w_center w_55">
                              <div class="progress">
                                  <div class="progress-bar bg-PaleTurquoise" " role="progressbar" aria-valuenow="60" aria-valuemin="0" aria-valuemax="100" style="width: 2%;">
                                      <span class="sr-only">60% Complete</span>
                                  </div>
                              </div>
                          </div>
                          <div class="w_right w_20">
                              <span>1k</span>
                          </div>
                          <div class="clearfix"></div>
                      </div>

                      <!-- Paraná -->
                      <div class="widget_summary">
                          <div class="w_left w_25">
                              <span>Paraná</span>
                          </div>
                          <div class="w_center w_55">
                              <div class="progress">
                                  <div class="progress-bar bg-PaleTurquoise" " role="progressbar" aria-valuenow="60" aria-valuemin="0" aria-valuemax="100" style="width: 25%;">
                                      <span class="sr-only">60% Complete</span>
                                  </div>
                              </div>
                          </div>
                          <div class="w_right w_20">
                              <span>1k</span>
                          </div>
                          <div class="clearfix"></div>
                      </div>

                      <!-- São Carlos -->
                      <div class="widget_summary">
                          <div class="w_left w_25">
                              <span>São Carlos</span>
                          </div>
                          <div class="w_center w_55">
                              <div class="progress">
                                  <div class="progress-bar bg-PaleTurquoise" " role="progressbar" aria-valuenow="60" aria-valuemin="0" aria-valuemax="100" style="width: 32%;">
                                      <span class="sr-only">60% Complete</span>
                                  </div>
                              </div>
                          </div>
                          <div class="w_right w_20">
                              <span>1k</span>
                          </div>
                          <div class="clearfix"></div>
                      </div>
                  </div>
              </div>
          </div>

          <!-- Grafico Operacional -->
          <div class="col-md-4 col-sm-4 col-xs-12">
              <div class="x_panel tile fixed_height_350 overflow_hidden">
                  <div class="x_title">
                      <h2>Operação:</h2>
                      <ul class="nav navbar-right panel_toolbox">
                          <li>
                              <a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                          </li>
                          <li>
                              <a class="close-link"><i class="fa fa-close"></i></a>
                          </li>
                      </ul>
                      <div class="clearfix"></div>
                  </div>

                  <div class="x_content">
                      <table class="" style="width:100%">
                          <tr>
                              <th style="width:37%;">
                                  <p>Viagens </p>
                              </th>
                              <th>
                                  <div class="col-lg-7 col-md-7 col-sm-7 col-xs-7">
                                      <p class="">Status</p>
                                  </div>
                                  <div class="col-lg-5 col-md-5 col-sm-5 col-xs-5">
                                      <p class="">Progresso</p>
                                  </div>
                              </th>
                          </tr>
                          <tr>
                              <td>
                                  <canvas class="canvasDoughnut" height="140" width="140" style="margin: 15px 10px 10px 0"></canvas>
                              </td>
                              <td>
                                  <table class="tile_info">
                                      <tr>
                                          <td>
                                              <p><i class="fa fa-square blue"></i>Ag. Carreg. </p>
                                          </td>
                                          <td>30%</td>
                                      </tr>
                                      <tr>
                                          <td>
                                              <p><i class="fa fa-square green"></i>Ag. Descarga </p>
                                          </td>
                                          <td>10%</td>
                                      </tr>
                                      <tr>
                                          <td>
                                              <p><i class="fa fa-square purple"></i>Em Transito </p>
                                          </td>
                                          <td>20%</td>
                                      </tr>
                                      <tr>
                                          <td>
                                              <p><i class="fa fa-square aero"></i>Carregando </p>
                                          </td>
                                          <td>15%</td>
                                      </tr>
                                      <tr>
                                          <td>
                                              <p><i class="fa fa-square red"></i>Concluido </p>
                                          </td>
                                          <td>30%</td>
                                      </tr>
                                  </table>
                              </td>
                          </tr>
                      </table>
                      <tr>

                          <!-- rendimento dos veículos FROTA/AGREGADO/TERCEIRO -->
                          <!-- Frota -->
                          <div class="row">
                              <div class="progress_title">
                                  <br>
                                  <span class="left"><p>Veículos</p></span>
                                  <div class="clearfix"></div>
                              </div>

                              <div class="col-xs-2">
                                  <span>Frota</span>
                              </div>
                              <div class="col-xs-8">
                                  <div class="progress progress_sm">
                                      <div class="progress-bar bg-green" role="progressbar" data-transitiongoal="89"></div>
                                  </div>
                              </div>
                              <div class="col-xs-2 more_info">
                                  <span>89%</span>
                              </div>
                          </div>

                          <!-- Agregados -->
                          <div class="row">
                              <div class="col-xs-2">
                                  <span>Agregados</span>
                              </div>
                              <div class="col-xs-8">
                                  <div class="progress progress_sm">
                                      <div class="progress-bar bg-yellow" role="progressbar" data-transitiongoal="79"></div>
                                  </div>
                              </div>
                              <div class="col-xs-2 more_info">
                                  <span>79%</span>
                              </div>
                          </div>

                          <!-- Terceiros -->
                          <div class="row">
                              <div class="col-xs-2">
                                  <span>Terceiros</span>
                              </div>
                              <div class="col-xs-8">
                                  <div class="progress progress_sm">
                                      <div class="progress-bar bg-green" role="progressbar" data-transitiongoal="69"></div>
                                  </div>
                              </div>
                              <div class="col-xs-2 more_info">
                                  <span>69%</span>
                              </div>
                      </tr>
                  </div>
              </div>
              <!-- aqui funciona -->


          </div>
      </div>


      <div class="col-md-4 col-sm-4 col-xs-12">
          <div class="x_panel tile fixed_height_400">
              <div class="x_title">
                  <h2>VW Solicitações:</h2>
                  <ul class="nav navbar-right panel_toolbox">
                      <li>
                          <a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                      </li>
                      <li>
                          <a class="close-link"><i class="fa fa-close"></i></a>
                      </li>
                  </ul>
                  <div class="clearfix"></div>
              </div>

              <div class="col-xs-12 bg-white progress_summary">
                  <!-- Interplantas -->
                  <div class="row">
                      <div class="col-xs-6">
                          <span>Ferramentas/Chapas/Disp.</span>
                      </div>
                      <div class="col-xs-5">
                          <div class="progress progress_sm">
                              <div class="progress-bar bg-PaleTurquoise" " role="progressbar" data-transitiongoal="89"></div>
                          </div>
                      </div>
                      <div class="col-xs-1 more_info">
                          <span>89%</span>
                      </div>
                  </div>

                  <!-- distribuição de aço -->
                  <div class="row">
                      <div class="col-xs-6">
                          <span>Consignado CNT</span>
                      </div>
                      <div class="col-xs-5">
                          <div class="progress progress_sm">
                              <div class="progress-bar bg-green" role="progressbar" data-transitiongoal="79"></div>
                          </div>
                      </div>
                      <div class="col-xs-1 more_info">
                          <span>79%</span>
                      </div>
                  </div>

                  <!-- Terceiros -->
                  <div class="row">
                      <div class="col-xs-6">
                          <span>Inclusão na Janela do dia</span>
                      </div>
                      <div class="col-xs-5">
                          <div class="progress progress_sm">
                              <div class="progress-bar bg-PaleTurquoise" " role="progressbar" data-transitiongoal="69"></div>
                          </div>
                      </div>
                      <div class="col-xs-1 more_info">
                          <span>69%</span>
                      </div>
                  </div>
                  <!-- Terceiros -->
                  <div class="row">
                      <div class="col-xs-6">
                          <span>Emergencial</span>
                      </div>
                      <div class="col-xs-5">
                          <div class="progress progress_sm">
                              <div class="progress-bar bg-green" role="progressbar" data-transitiongoal="69"></div>
                          </div>
                      </div>
                      <div class="col-xs-1 more_info">
                          <span>69%</span>
                      </div>
                  </div>

                  <!-- Terceiros -->
                  <div class="row">
                      <div class="col-xs-6">
                          <span>Kanban</span>
                      </div>
                      <div class="col-xs-5">
                          <div class="progress progress_sm">
                              <div class="progress-bar bg-PaleTurquoise" " role="progressbar" data-transitiongoal="69"></div>
                          </div>
                      </div>
                      <div class="col-xs-1 more_info">
                          <span>69%</span>
                      </div>
                  </div>
                  <!-- Terceiros -->
                  <div class="row">
                      <div class="col-xs-6">
                          <span>Adicional na Janela Progr.</span>
                      </div>
                      <div class="col-xs-5">
                          <div class="progress progress_sm">
                              <div class="progress-bar bg-green" role="progressbar" data-transitiongoal="69"></div>
                          </div>
                      </div>
                      <div class="col-xs-1 more_info">
                          <span>69%</span>
                      </div>
                  </div>
                  <!-- Terceiros -->
                  <div class="row">
                      <div class="col-xs-6">
                          <span>Retorno de Embalagem</span>
                      </div>
                      <div class="col-xs-5">
                          <div class="progress progress_sm">
                              <div class="progress-bar bg-PaleTurquoise" " role="progressbar" data-transitiongoal="69"></div>
                          </div>
                      </div>
                      <div class="col-xs-1 more_info">
                          <span>69%</span>
                      </div>
                  </div>
                  <!-- Terceiros -->
                  <div class="row">
                      <div class="col-xs-6">
                          <span>Feriado/Domingo</span>
                      </div>
                      <div class="col-xs-5">
                          <div class="progress progress_sm">
                              <div class="progress-bar bg-green" role="progressbar" data-transitiongoal="69"></div>
                          </div>
                      </div>
                      <div class="col-xs-1 more_info">
                          <span>69%</span>
                      </div>
                  </div>
                  <!-- Terceiros -->
                  <div class="row">
                      <div class="col-xs-6">
                          <span>Sábado Adicional</span>
                      </div>
                      <div class="col-xs-5">
                          <div class="progress progress_sm">
                              <div class="progress-bar bg-PaleTurquoise" " role="progressbar" data-transitiongoal="69"></div>
                          </div>
                      </div>
                      <div class="col-xs-1 more_info">
                          <span>69%</span>
                      </div>
                  </div>

              </div>

          </div>
      </div>

  </div>
    </div>
    <script src="plugin/bower_components/jquery/dist/jquery.min.js"></script>
 
<!-- Menu Plugin JavaScript -->
<script src="plugin/bower_components/sidebar-nav/dist/sidebar-nav.min.js"></script>
<!--slimscroll JavaScript -->
<script src="js/jquery.slimscroll.js"></script>
<!--Wave Effects -->
 
<!--Counter js -->
<script src="plugin/bower_components/waypoints/lib/jquery.waypoints.js"></script>
<script src="plugin/bower_components/counterup/jquery.counterup.min.js"></script>
<!--Morris JavaScript -->
<script src="plugin/bower_components/raphael/raphael-min.js"></script>
<script src="plugin/bower_components/morrisjs/morris.js"></script>
<!-- chartist chart -->
<script src="plugin/bower_components/chartist-js/dist/chartist.min.js"></script>
<script src="plugin/bower_components/chartist-plugin-tooltip-master/dist/chartist-plugin-tooltip.min.js"></script>
<!-- Calendar JavaScript -->
<script src="plugin/bower_components/moment/moment.js"></script>
<script src='plugin/bower_components/calendar/dist/fullcalendar.min.js'></script>
<script src="plugin/bower_components/calendar/dist/cal-init.js"></script>
<!-- Custom Theme JavaScript -->
<script src="js/custom.min.js"></script>
<script src="js/dashboard1.js"></script>
<!-- Custom tab JavaScript -->
<script src="js/cbpFWTabs.js"></script>
<script type="text/javascript">
    (function () {
        [].slice.call(document.querySelectorAll('.sttabs')).forEach(function (el) {
            new CBPFWTabs(el);
        });
    })();
</script>
<script src="plugin/bower_components/toast-master/js/jquery.toast.js"></script>
<!--Style Switcher -->
<script src="plugin/bower_components/styleswitcher/jQuery.style.switcher.js"></script>

 <script src="plugin/bower_components/Minimal-Gauge-chart/js/cmGauge.js"></script>
    
    <!-- Fim dos Graficos



    <!--
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
    <script>
        const ctx = document.getElementById('myChart2');

        new Chart(ctx, {
            type: 'bar',
            data: {
                labels: ['Red', 'Blue', 'Yellow', 'Green', 'Purple', 'Orange'],
                datasets: [{
                    label: '# of Votes',
                    data: [12, 19, 3, 5, 2, 3],
                    borderWidth: 1
                }]
            },
            options: {
                scales: {
                    y: {
                        beginAtZero: true
                    }
                }
            }
        });
    </script>

 -->
     <!-- jQuery -->
 <script src="vendor/jquery/dist/jquery.min.js"></script>
 <!-- Bootstrap -->
 <script src="vendor/bootstrap/dist/js/bootstrap.min.js"></script>
 <!-- FastClick -->
 <script src="vendor/fastclick/lib/fastclick.js"></script>
 <!-- NProgress -->
 <script src="vendor/nprogress/nprogress.js"></script>
 <!-- Chart.js -->
 <script src="vendor/Chart.js/dist/Chart.min.js"></script>
 <!-- gauge.js -->
 <script src="vendor/gauge.js/dist/gauge.min.js"></script>
 <!-- bootstrap-progressbar -->
 <script src="vendor/bootstrap-progressbar/bootstrap-progressbar.min.js"></script>
 <!-- iCheck -->
 <script src="vendor/iCheck/icheck.min.js"></script>
 <!-- Skycons -->
 <script src="vendor/skycons/skycons.js"></script>
 <!-- Flot -->
 <script src="vendor/Flot/jquery.flot.js"></script>
 <script src="vendor/Flot/jquery.flot.pie.js"></script>
 <script src="vendor/Flot/jquery.flot.time.js"></script>
 <script src="vendor/Flot/jquery.flot.stack.js"></script>
 <script src="vendor/Flot/jquery.flot.resize.js"></script>
 <!-- Flot pluins -->
 <script src="vendor/flot.orderbars/js/jquery.flot.orderBars.js"></script>
 <script src="vendor/flot-spline/js/jquery.flot.spline.min.js"></script>
 <script src="vendor/flot.curvedlines/curvedLines.js"></script>
 <!-- DateJS -->
 <script src="vendor/DateJS/build/date.js"></script>
 <!-- JQVMap -->
 <script src="vendor/jqvmap/dist/jquery.vmap.js"></script>
 <script src="vendor/jqvmap/dist/maps/jquery.vmap.world.js"></script>
 <script src="vendor/jqvmap/examples/js/jquery.vmap.sampledata.js"></script>
 <!-- bootstrap-daterangepicker -->
 <script src="vendor/moment/min/moment.min.js"></script>
 <script src="vendor/bootstrap-daterangepicker/daterangepicker.js"></script>

 <!-- Custom Theme Scripts -->
 <script src="build/js/custom.min.js"></script>

</asp:Content>
