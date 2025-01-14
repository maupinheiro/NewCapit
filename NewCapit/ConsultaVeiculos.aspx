<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ConsultaVeiculos.aspx.cs" Inherits="NewCapit.ConsultaVeiculos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- Page Heading -->
    <div class="d-sm-flex align-items-left justify-content-between mb-4">
        <h1 class="h3 mb-0 text-gray-800">
            <i class="fas fa-shipping-fast"></i>Consulta Veículos</h1>
        <a href="#" class="d-none d-sm-inline-block btn btn-sm btn-primary shadow-sm">
            <i class="fas fa-shipping-fast"></i>Novo Cadastro
        </a>
    </div>

    <!-- Content Row -->
    <div class="row">
        <!-- Total Veículos -->        
        <div class="col-xl-2 col-md-6 mb-4">
            <div class="card border-left-primary shadow h-100 py-2">
                <div class="card-body">
                    <div class="row no-gutters align-items-center">
                        <div class="col mr-2">
                            <div class="text-xs font-weight-bold text-primary text-uppercase mb-1">
                                Total de Veículos
                            </div>
                            <div class="h5 mb-0 font-weight-bold text-gray-800">
                                <asp:Label ID="TotalVeiculos" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="col-auto">
                            <i class="fas fa-shipping-fast fa-2x text-gray-300"></i>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Frota -->
        <div class="col-xl-2 col-md-6 mb-4">
            <div class="card border-left-success shadow h-100 py-2">
                <div class="card-body">
                    <div class="row no-gutters align-items-center">
                        <div class="col mr-2">
                            <div class="text-xs font-weight-bold text-success text-uppercase mb-1">
                                Frota
                            </div>
                            <div class="h5 mb-0 font-weight-bold text-gray-800">
                                <asp:Label ID="TotalFrota" runat="server" Text=""></asp:Label>
                            </div>
                            <!--
                            <span class="text-xs font-weight-bold text-success text-uppercase mb-1">MATRIZ:</span>
                            <span class="text-xs font-weight-bold text-dark text-uppercase mb-1">55</span>
                            <span class="text-xs font-weight-bold text-success text-uppercase mb-1">MINAS:</span>
                            <span class="text-xs font-weight-bold text-dark text-uppercase mb-1">55</span>
                            <span class="text-xs font-weight-bold text-success text-uppercase mb-1">CNT:</span>
                            <span class="text-xs font-weight-bold text-dark text-uppercase mb-1">55</span>
                            -->
                        </div>
                        <div class="col-auto">
                            <i class="fas fa-truck fa-2x text-gray-300"></i>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Agregados Matriz -->
        <div class="col-xl-2 col-md-6 mb-4">
            <div class="card border-left-info shadow h-100 py-2">
                <div class="card-body">
                    <div class="row no-gutters align-items-center">
                        <div class="col mr-2">
                            <div class="text-xs font-weight-bold text-info text-uppercase mb-1">
                                Agregados -  Matriz                             
                                 <div class="h5 mb-0 font-weight-bold text-gray-800">
                                     <asp:Label ID="TotalAgregadoMatriz" runat="server" Text=""></asp:Label>
                                 </div>
                            </div>
                        </div>
                        <div class="col-auto">
                            <i class="fas fa-truck fa-2x text-gray-300"></i>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Terceiros -->
        <div class="col-xl-2 col-md-6 mb-4">
            <div class="card border-left-warning shadow h-100 py-2">
                <div class="card-body">
                    <div class="row no-gutters align-items-center">
                        <div class="col mr-2">
                            <div class="text-xs font-weight-bold text-warning text-uppercase mb-1">
                                Terceiros - Matriz
                            </div>
                            <div class="h5 mb-0 font-weight-bold text-gray-800">
                                <asp:Label ID="TotalTerceirosMatriz" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="col-auto">
                            <i class="fas fa-truck fa-2x text-gray-300"></i>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-- Terceiros CNT -->
        <div class="col-xl-2 col-md-6 mb-4">
            <div class="card border-left-danger shadow h-100 py-2">
                <div class="card-body">
                    <div class="row no-gutters align-items-center">
                        <div class="col mr-2">
                            <div class="text-xs font-weight-bold text-danger text-uppercase mb-1">
                                Terceiros - CNTi
                            </div>
                            <div class="h5 mb-0 font-weight-bold text-gray-800">15</div>
                        </div>
                        <div class="col-auto">
                            <i class="fas fa-truck fa-2x text-gray-300"></i>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-- Agregados MINAS -->
        <div class="col-xl-2 col-md-6 mb-4">
            <div class="card border-left-secondary shadow h-100 py-2">
                <div class="card-body">
                    <div class="row no-gutters align-items-center">
                        <div class="col mr-2">
                            <div class="text-xs font-weight-bold text-secondary text-uppercase mb-1">
                                Agregados - CNTi
                            </div>
                            <div class="h5 mb-0 font-weight-bold text-gray-800">15</div>
                        </div>
                        <div class="col-auto">
                            <i class="fas fa-truck fa-2x text-gray-300"></i>
                        </div>
                    </div>
                </div>
            </div>
        </div>
       
        
    </div>


</asp:Content>
