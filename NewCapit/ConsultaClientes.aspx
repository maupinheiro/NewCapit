<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ConsultaClientes.aspx.cs" Inherits="NewCapit.ConsultaClientes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- Page Heading -->
    <div class="d-sm-flex align-items-center justify-content-between mb-4">
        <h1 class="h3 mb-2 text-gray-800">Consulta Clientes</h1>
        <a href="Frm_CadClientes.aspx" class="d-none d-sm-inline-block btn btn-sm btn-primary shadow-sm"><i
            class="fas fa-industry"></i> Novo Cadastro
        </a>
    </div>
    <!-- Content Graficos -->
    <div class="row">
        <!-- Total de Clientes -->
        <div class="col-xl-2 col-md-6 mb-4">
            <div class="card border-left-primary shadow h-100 py-2">
                <div class="card-body">
                    <div class="row no-gutters align-items-center">
                        <div class="col mr-2">
                            <div class="text-xs font-weight-bold text-primary text-uppercase mb-1">
                                Total de Clientes
                            </div>
                            <div class="h5 mb-0 font-weight-bold text-gray-800">
                               <asp:Label ID="TotalClientes" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="col-auto">
                            <img src="img/brasil.jpg" width="60px" alt="" />
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Total de Clientes por Região -->
        <div class="col-xl-2 col-md-6 mb-4">
            <div class="card border-left-info shadow h-100 py-2">
                <div class="card-body">
                    <div class="row no-gutters align-items-center">
                        <div class="col mr-2">
                            <div class="text-xs font-weight-bold text-info text-uppercase mb-1">
                                Região Norte
                            </div>
                            <div class="h5 mb-0 font-weight-bold text-gray-800">
                                <asp:Label ID="Norte" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="col-auto">
                            <img src="img/regiao-norte.jpg" width="60px" alt="" />
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Nordeste -->
        <div class="col-xl-2 col-md-6 mb-4">
            <div class="card border-left-danger shadow h-100 py-2">
                <div class="card-body">
                    <div class="row no-gutters align-items-center">
                        <div class="col mr-2">
                            <div class="text-xs font-weight-bold text-danger text-uppercase mb-1">
                                Região Nordeste
                            </div>
                            <div class="h5 mb-0 font-weight-bold text-gray-800">
                                <asp:Label ID="Nordeste" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="col-auto">
                            <img src="img/regiao-nordeste.jpg" width="60px" alt="" />
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Sul -->
        <div class="col-xl-2 col-md-6 mb-4">
            <div class="card border-left-warning shadow h-100 py-2">
                <div class="card-body">
                    <div class="row no-gutters align-items-center">
                        <div class="col mr-2">
                            <div class="text-xs font-weight-bold text-warning text-uppercase mb-1">
                                Região Sul                                       
                            </div>
                            <div class="row no-gutters align-items-center">
                                <div class="col-auto">
                                    <div class="h5 mb-0 mr-3 font-weight-bold text-gray-800">
                                        <asp:Label ID="Sul" runat="server" Text=""></asp:Label>
                                     </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-auto">
                            <img src="img/regiao-sul.jpg" width="60px" alt="" />
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Sudeste -->
        <div class="col-xl-2 col-md-6 mb-4">
            <div class="card border-left-success shadow h-100 py-2">
                <div class="card-body">
                    <div class="row no-gutters align-items-center">
                        <div class="col mr-2">
                            <div class="text-xs font-weight-bold text-success text-uppercase mb-1">
                                Região Sudeste
                            </div>
                            <div class="h5 mb-0 font-weight-bold text-gray-800">
                                <asp:Label ID="Sudeste" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="col-auto">
                            <img src="img/regiao-sudeste.jpg" width="60px" alt="" />
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Centro-Oeste -->
        <div class="col-xl-2 col-md-6 mb-4">
            <div class="card border-left-warning shadow h-100 py-2">
                <div class="card-body">
                    <div class="row no-gutters align-items-center">
                        <div class="col mr-2">
                            <div class="text-xs font-weight-bold text-warning text-uppercase mb-1">
                                Região Centro-Oeste
                            </div>
                            <div class="h5 mb-0 font-weight-bold text-gray-800">
                                <asp:Label ID="CentroOeste" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="col-auto">
                            <img src="img/regiao-centro-oeste.jpg" width="60px" alt="" />
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
                <asp:GridView runat="server" ID="gvList" CssClass="table table-bordered dataTable1" Width="100%" AutoGenerateColumns="False">
                    <Columns>                        
                        <asp:BoundField DataField="id" HeaderText="#ID" />
                        <asp:BoundField DataField="codcli" HeaderText="CÓDIGO" />
                        <asp:BoundField DataField="tipo" HeaderText="TIPO" />
                        <asp:BoundField DataField="nomcli" HeaderText="NOME FANTASIA" />
                        <asp:BoundField DataField="unidade" HeaderText="UNIDADE" />
                        <asp:BoundField DataField="regiao" HeaderText="REGIÃO   " />
                        <asp:BoundField DataField="cidcli" HeaderText="CIDADE" />
                        <asp:BoundField DataField="estcli" HeaderText="UF" />                        
                        <asp:BoundField DataField="ativo_inativo" HeaderText="SITUAÇÃO" ControlStyle-BackColor="Yellow" /> 
                         
                        <asp:TemplateField HeaderText="AÇÕES" ShowHeader="True" HeaderStyle-Width="230px">
                             <ItemTemplate >                                 
                                 <a class="btn btn-primary btn-sm" href="Frm_AltClientes.aspx?id=">
                                    <i class="fa fa-edit"></i>
                                    Editar
                                 </a> 
                                 <a class="btn btn-info btn-sm" href="Frm_AltClientes.aspx?id=">
                                    <i class="fas fa-map-marker-alt"></i>                                    
                                    Mapa
                                 </a> 
                                 <a class="btn btn-danger btn-sm" href="Frm_AltClientes.aspx?id=">
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
