﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="NewCapit.Home1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    

    
    <!-- Page Heading -->
    <div class="d-sm-flex align-items-left justify-content-between mb-4">
        <h1 class="h3 mb-0 text-gray-800"> Visão Geral das Filiais</h1>
        <a href="#" class="d-none d-sm-inline-block btn btn-sm btn-primary shadow-sm">
            <i class="fas fa-map-marker-alt fa-sm text-white-50"></i> Veículos no Mapa
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
    
    <div class="row">
        <div class="col-md-12 col-sm-12 col-xs-12">
            <div class="dashboard_graph">
                <div class="row x_title">
                    <div class="col-md-8">
                        <h3>Resumo de Atividades</h3>
                    </div>

                    <div class="col-md-2">
                        <div id="dataInicial" class="pull-right">
                            <div class="form-group">
                                <input type="date" class="form-control" id="input_inicial">
                            </div>
                        </div>
                    </div>
		            <div class="col-md-2">
                        <div id="dataFinal" class="pull-right">                      
		                    <div class="form-group">
                                <input type="date" class="form-control" id="input_final">
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div> 
    <div class="row">
         <!-- Content Column -->
        <div class="col-lg-3 mb-4">
            <!-- Project Card Example -->
            <div class="card shadow mb-3">
                <div class="card-header py-3">
                    <h6 class="m-0 font-weight-bold text-primary">Entregas</h6>
                </div>
                <div class="card-body">
                    <h4 class="small font-weight-bold">Matriz <span class="float-right">1220</span></h4> 
                    <div class="progress mb-3">
                        <div class="progress-bar bg-danger" role="progressbar" style="width: 20%"
                            aria-valuenow="20" aria-valuemin="0" aria-valuemax="100">
                        </div>
                    </div>
                    <h4 class="small font-weight-bold">Minas <span class="float-right">40</span></h4>
                    <div class="progress mb-4">
                        <div class="progress-bar bg-warning" role="progressbar" style="width: 40%"
                            aria-valuenow="40" aria-valuemin="0" aria-valuemax="100">
                        </div>
                    </div>
                    <h4 class="small font-weight-bold">Pernambuco <span class="float-right">60</span></h4>
                    <div class="progress mb-4">
                        <div class="progress-bar" role="progressbar" style="width: 60%"
                            aria-valuenow="60" aria-valuemin="0" aria-valuemax="100">
                        </div>
                    </div>
                    <h4 class="small font-weight-bold">CNT <span class="float-right">80</span></h4>
                    <div class="progress mb-4">
                        <div class="progress-bar bg-info" role="progressbar" style="width: 80%"
                            aria-valuenow="80" aria-valuemin="0" aria-valuemax="100">
                        </div>
                    </div>
                    <h4 class="small font-weight-bold">Taubaté <span class="float-right">20</span></h4>
                    <div class="progress">
                        <div class="progress-bar bg-success" role="progressbar" style="width: 20%"
                            aria-valuenow="100" aria-valuemin="0" aria-valuemax="100">
                        </div>
                    </div>
                    <br />
                    <h4 class="small font-weight-bold">SBC <span class="float-right">20</span></h4>
                    <div class="progress">
                         <div class="progress-bar bg-secondary" role="progressbar" style="width: 20%"
                             aria-valuenow="100" aria-valuemin="0" aria-valuemax="100">
                         </div>
                    </div>
                    <br />
                    <h4 class="small font-weight-bold">Paraná <span class="float-right">20</span></h4>
                    <div class="progress">
                         <div class="progress-bar bg-warning" role="progressbar" style="width: 20%"
                             aria-valuenow="100" aria-valuemin="0" aria-valuemax="100">
                         </div>
                    </div>
                    <br />
                    <h4 class="small font-weight-bold">São Carlos <span class="float-right">20</span></h4>
                    <div class="progress">
                         <div class="progress-bar bg-primary" role="progressbar" style="width: 20%"
                             aria-valuenow="100" aria-valuemin="0" aria-valuemax="100">
                         </div>
                    </div>
                    <!-- aqui -->
                </div>
            </div>
        </div>
        <div class="col-lg-3 mb-4">           
             <div class="card shadow mb-4">                
                 <div
                     class="card-header py-3 d-flex flex-row align-items-center justify-content-between">
                     <h6 class="m-0 font-weight-bold text-primary">Carregamento</h6>                    
                 </div>
                 <!-- Card Body -->
                 <div class="card-body">
                     <div class="chart-pie pt-4 pb-2">
                         <canvas id="myPieChart"></canvas>
                     </div>
                     <div class="mt-4 text-center small">                         
                         <span class="mr-2">
                             <i class="fas fa-circle text-primary"></i> Ag. Carreg.
                         </span>
                         <span class="mr-2">
                             <i class="fas fa-circle text-success"></i> Ag. Descarga
                         </span>
                         <span class="mr-2">
                             <i class="fas fa-circle text-info"></i> Em Transito
                         </span>
                         <br />
                         <span class="mr-2">
                             <i class="fas fa-circle text-warning"></i> Carregando
                         </span>                         
                        <span class="mr-2">
                            <i class="fas fa-circle text-danger"></i> Concluido
                        </span>
                     </div>
                 </div>
             </div>
        </div>
    </div>
    

  
</asp:Content>
