<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ConsultaAgregados.aspx.cs" Inherits="NewCapit.ConsultaAgregados" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="javascript">
function ConfirmMessage() {
    var selectedvalue = confirm("Exclusão de Dados\n Tem certeza de que deseja excluir a informação permanentemente?");
    if (selectedvalue) {
        document.getElementById('<%=txtconformmessageValue.ClientID %>').value = "Yes";
   } else {
       document.getElementById('<%=txtconformmessageValue.ClientID %>').value = "No";
   }
}
   
    </script>
    <!-- Page Heading -->
    <div class="d-sm-flex align-items-center justify-content-between mb-4">
        <h1 class="h3 mb-2 text-gray-800">
            <i class="fas fa-truck"></i> Consulta Agregados / Terceiros</h1>
        <a href="Frm_CadTransportadoras.aspx" class="d-none d-sm-inline-block btn btn-sm btn-primary shadow-sm"><i
            class="fas fa-user-friends"></i> Novo Cadastro            
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
                            
                            <img src="img/totalagregados.png" width="60px" alt="" />
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
style="width:50%" aria-valuenow="30" aria-valuemin="0" aria-valuemax="100">
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-auto">
                            <img src="img/percentualAgregados.png" width="60px" alt="" />
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
                            <img src="img/agregados.png" width="60px" alt="" />
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
                            <img src="img/terceiros.png" width="60px" alt="" />
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
                            <img src="img/agregadoAtivo.png" width="60px" alt="" />
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
                            <img src="img/agregadoinativo.png" width="60px" alt="" />
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
                <asp:GridView runat="server" ID="gvListAgregados" CssClass="table table-bordered dataTable1" Width="100%" AutoGenerateColumns="False" DataKeyNames="id">
                    <Columns>
                        <asp:BoundField DataField="id" HeaderText="#ID" />
                        <asp:BoundField DataField="codtra" HeaderText="CÓDIGO" />
                        <asp:BoundField DataField="fantra" HeaderText="NOME FANTASIA" /> 
                        <asp:BoundField DataField="cnpj" HeaderText="CPF/CNPJ" />
                        <asp:BoundField DataField="filial" HeaderText="FILIAL" />
                        <asp:BoundField DataField="fone2" HeaderText="CELULAR" />
                        <asp:BoundField DataField="ativa_inativa" HeaderText="SITUAÇÃO" />
                        <asp:TemplateField HeaderText="AÇÕES" ShowHeader="True" HeaderStyle-Width="180px">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkEditar" runat="server" OnClick="Editar"  CssClass="btn btn-primary btn-sm"><i class="fa fa-edit"></i>Editar</asp:LinkButton>
                                <asp:LinkButton ID="lnkExcluir" runat="server" OnClick="Excluir" CssClass="btn btn-danger btn-sm" OnClientClick="javascript:ConfirmMessage();"><i class="fa fa-trash"></i></i>Excluir</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <asp:HiddenField ID="txtconformmessageValue" runat="server" />
        </div>
    </div>

    <!-- JavaScript -->
    <script src="vendor/datatables/jquery.dataTables.min.js"></script>
    <script src="src/javascript/script.js"></script>
     
</asp:Content>
