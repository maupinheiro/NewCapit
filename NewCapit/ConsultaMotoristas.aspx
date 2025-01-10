<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ConsultaMotoristas.aspx.cs" Inherits="NewCapit.ConsultaMotoristas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- Page Heading -->
    <div class="d-sm-flex align-items-center justify-content-between mb-4">
        <h1 class="h3 mb-2 text-gray-800">
            <i class="fas fa-address-card"></i> Consulta Motoristas</h1>
        <a href="Frm_CadTransportadoras.aspx" class="d-none d-sm-inline-block btn btn-sm btn-primary shadow-sm"><i
            class="far fa-address-card"></i> Novo Cadastro
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
                                Total de Condutores
                            </div>
                            <div class="h5 mb-0 font-weight-bold text-gray-800">
                                <asp:Label ID="TotalVeiculos" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="col-auto">
                            <img src="img/totalMot.png" width="60px" alt="" />
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
                                Colaboradores                                  
                            </div>
                            <div class="row no-gutters align-items-center">
                                <div class="col-auto">
                                    <div class="h5 mb-0 mr-3 font-weight-bold text-gray-800">
                                    </div>
                                </div>                                
                            </div>
                        </div>
                        <div class="col-auto">
                            <img src="img/totalFunc.png" width="60px" alt="" />
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
                            <img src="img/totalAgre.png" width="60px" alt="" />
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
                            <img src="img/totalTerc.png" width="60px" alt="" />
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
                                <asp:Label ID="totalAtivos" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="col-auto">
                            <img src="img/totalAtivos.png" width="60px" alt="" />
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
                            <img src="img/totalInativos.png" width="60px" alt="" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- Corpo da grid -->
    <div class="card shadow mb-4">
    <div class="card-body">
        <div class="table-responsive">
            <asp:GridView runat="server" ID="gvListAgregados" CssClass="table table-bordered dataTable1" Width="100%" AutoGenerateColumns="False" DataKeyNames="id">
                <Columns>
                    <asp:BoundField DataField="" HeaderText="Mot." />
                    <asp:BoundField DataField="codtra" HeaderText="CÓDIGO" />
                    <asp:BoundField DataField="fantra" HeaderText="NOME COMPLETO" /> 
                    <asp:BoundField DataField="cnpj" HeaderText="IDADE" />
                    <asp:BoundField DataField="filial" HeaderText="INICIO" />
                    <asp:BoundField DataField="ativa_inativa" HeaderText="TIPO" />
                    <asp:BoundField DataField="fone2" HeaderText="TEMPO DE CONTRATO" />
                    <asp:BoundField DataField="ativa_inativa" HeaderText="CARGO" />
                    <asp:BoundField DataField="ativa_inativa" HeaderText="FUNÇÃO" />
                    <asp:BoundField DataField="ativa_inativa" HeaderText="HORÁRIO" />
                    <asp:BoundField DataField="ativa_inativa" HeaderText="FROTA" />
                    <asp:BoundField DataField="ativa_inativa" HeaderText="SITUAÇÃO" />
                    <asp:TemplateField HeaderText="AÇÕES" ShowHeader="True" HeaderStyle-Width="180px">
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

</asp:Content>
