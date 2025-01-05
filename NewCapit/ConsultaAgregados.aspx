<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ConsultaAgregados.aspx.cs" Inherits="NewCapit.ConsultaAgregados" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- Page Heading -->
    <div class="d-sm-flex align-items-center justify-content-between mb-4">
        <h1 class="h3 mb-2 text-gray-800">Consulta Agregados / Terceiros</h1>
        <a href="Frm_CadClientes.aspx" class="d-none d-sm-inline-block btn btn-sm btn-primary shadow-sm"><i
            class="fas fa-industry"></i>Novo Cadastro
        </a>
    </div>
    <!-- Content Graficos -->
    <div class="row">
        <!-- Total de veiculos frota/agregados/terceiros -->
        <div class="col-xl-2 col-md-6 mb-4">
            <div class="card border-left-primary shadow h-100 py-2">
                <div class="card-body">
                    <div class="row no-gutters align-items-center">
                        <div class="col mr-2">
                            <div class="text-xs font-weight-bold text-primary text-uppercase mb-1">
                                Total de Agregados / Terceiros
                            </div>
                            <div class="h5 mb-0 font-weight-bold text-gray-800">
                                <asp:Label ID="TotalVeiculos" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="col-auto">
                            <img src="img/caminhoes.png" width="60px" alt="" />
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Total de frota -->
        <div class="col-xl-2 col-md-6 mb-4">
            <div class="card border-left-info shadow h-100 py-2">
                <div class="card-body">
                    <div class="row no-gutters align-items-center">
                        <div class="col mr-2">
                            <div class="text-xs font-weight-bold text-info text-uppercase mb-1">
                                Em Operação                                   
                            </div>
                            <div class="row no-gutters align-items-center">
                                <div class="col-auto">
                                    <div class="h5 mb-0 mr-3 font-weight-bold text-gray-800">
                                      
                                    </div>
                                </div>
                                <div class="col">
                                    <div class="progress progress-sm mr-2">
                                         
                                        <div class="progress-bar bg-info" role="progressbar"
                                            style="width:percentualAtivos%" aria-valuenow=percentualAtivos aria-valuemin="0"
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

        <!-- Total de agregados -->
        <div class="col-xl-2 col-md-6 mb-4">
            <div class="card border-left-danger shadow h-100 py-2">
                <div class="card-body">
                    <div class="row no-gutters align-items-center">
                        <div class="col mr-2">
                            <div class="text-xs font-weight-bold text-danger text-uppercase mb-1">
                                Agregados
                            </div>
                            <div class="h5 mb-0 font-weight-bold text-gray-800">
                                <asp:Label ID="Agregados" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="col-auto">
                            <img src="img/truck6.png" width="60px" alt="" />
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Total de terceiros -->
        <div class="col-xl-2 col-md-6 mb-4">
            <div class="card border-left-warning shadow h-100 py-2">
                <div class="card-body">
                    <div class="row no-gutters align-items-center">
                        <div class="col mr-2">
                            <div class="text-xs font-weight-bold text-warning text-uppercase mb-1">
                                Terceiros                                       
                            </div>
                            <div class="row no-gutters align-items-center">
                                <div class="col-auto">
                                    <div class="h5 mb-0 mr-3 font-weight-bold text-gray-800">
                                        <asp:Label ID="Terceiros" runat="server" Text=""></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-auto">
                            <img src="img/truck7.png" width="60px" alt="" />
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Total de veiculos em atividade -->
        <div class="col-xl-2 col-md-6 mb-4">
            <div class="card border-left-success shadow h-100 py-2">
                <div class="card-body">
                    <div class="row no-gutters align-items-center">
                        <div class="col mr-2">
                            <div class="text-xs font-weight-bold text-success text-uppercase mb-1">
                                Ativos
                            </div>
                            <div class="h5 mb-0 font-weight-bold text-gray-800">
                                <asp:Label ID="veiculosAtivos" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="col-auto">
                            <img src="img/veiculoAtivo.png" width="60px" alt="" />
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Total de veiculos inativos -->
        <div class="col-xl-2 col-md-6 mb-4">
            <div class="card border-left-warning shadow h-100 py-2">
                <div class="card-body">
                    <div class="row no-gutters align-items-center">
                        <div class="col mr-2">
                            <div class="text-xs font-weight-bold text-warning text-uppercase mb-1">
                                Inativos
                            </div>
                            <div class="h5 mb-0 font-weight-bold text-gray-800">
                                <asp:Label ID="veiculosInativos" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="col-auto">
                            <img src="img/veiculoInativo.png" width="60px" alt="" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- DataTales Example -->

    <div class="card shadow mb-4">
        <div class="card-body">
            <div class="table-responsive">
                <asp:GridView runat="server" ID="gvList" CssClass="table table-bordered dataTable1" Width="100%" AutoGenerateColumns="False" DataKeyNames="id">
                    <Columns>
                        <asp:BoundField DataField="id" HeaderText="#ID" />
                        <asp:BoundField DataField="codtra" HeaderText="CÓDIGO" />
                        <asp:BoundField DataField="fantra" HeaderText="NOME FANTASIA" />
                        <asp:BoundField DataField="pessoa" HeaderText="PESSOA" />
                        <asp:BoundField DataField="filial" HeaderText="LOCAL DE PRESTAÇÃO SERVIÇO" />
                        <asp:BoundField DataField="contra" HeaderText="CONTATO" />
                        <asp:BoundField DataField="fone1" HeaderText="FONE FIXO" />
                        <asp:BoundField DataField="fone2" HeaderText="CELULAR" />
                        <asp:BoundField DataField="ativa_inativa" HeaderText="SITUAÇÃO" />

                        <asp:TemplateField HeaderText="AÇÕES" ShowHeader="True" HeaderStyle-Width="230px">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkEditar" runat="server" CssClass="btn btn-primary btn-sm"><i class="fa fa-edit"></i>Editar</asp:LinkButton>
                                <a class="btn btn-danger btn-sm" href="#">
                                    <i class="fa fa-trash"></i>
                                    Excluir
                                </a>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>

    <!-- JavaScript -->
    <script src="vendor/datatables/jquery.dataTables.min.js"></script>
    <script src="src/javascript/script.js"></script>
     <style>
         .progress-bar-container {
             width: 100%;
             background-color: #f3f3f3;
             border-radius: 5px;
             height: 30px;
             box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
         }

         .progress-bar {
             height: 100%;
             background-color: #4caf50;
             text-align: center;
             color: white;
             line-height: 30px; /* Alinha o texto verticalmente */
             width: 0%;
             border-radius: 5px;
             transition: width 0.5s ease-in-out;
         }
     </style>
</asp:Content>
