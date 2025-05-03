<%@ Page Title="" Language="C#" MasterPageFile="Main.Master" AutoEventWireup="true" CodeBehind="ConsultaClientes.aspx.cs" Inherits="NewCapit.ConsultaClientes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link rel="stylesheet" href="/css/styleTabela.css">
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

    <style>
        .pagination-centered {
         text-align: center;
        }

        .pagination-centered table {
        margin: 0 auto; /* Isso centraliza a tabela da paginação */
        }

    </style>
    <!-- Page Heading -->
    <div class="content-wrapper">
        <div class="content-header">
            <div class="d-sm-flex align-items-center justify-content-between mb-4">
                <h1 class="h3 mb-2 text-gray-800">
                    <i class="fas fa-warehouse"></i> &nbsp;Consulta Clientes</h1>
                <a href="/dist/pages/Frm_CadClientes.aspx" class="d-none d-sm-inline-block btn btn-sm btn-primary shadow-sm">
                    <i class="fas fa-warehouse"></i> &nbsp;Novo Cadastro
                </a>
            </div>
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
                                    <img src="/img/brasil.jpg" width="60px" alt="" />
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
                                    <img src="/img/regiao-norte.jpg" width="60px" alt="" />
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
                                    <img src="/img/regiao-nordeste.jpg" width="60px" alt="" />
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
                                    <img src="/img/regiao-sul.jpg" width="60px" alt="" />
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
                                    <img src="/img/regiao-sudeste.jpg" width="60px" alt="" />
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
                                    <img src="/img/regiao-centro-oeste.jpg" width="60px" alt="" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- DataTales Grid -->
        <div class="card shadow mb-4">
            <div class="card-header">
                <asp:TextBox ID="myInput" CssClass="form-control myInput" OnTextChanged="myInput_TextChanged" placeholder="Pesquisar ..." AutoPostBack="true" runat="server" Width="100%"></asp:TextBox>
              <%--<input type="text" id="myInput" onkeyup="myFunction()" placeholder="Pesquisar ...">--%>
            </div>
            <div class="card-body">
                <table id="example1" class="table table-bordered table-striped table-hover table-responsive">
                    <asp:GridView runat="server" ID="gvList" CssClass="table table-bordered table-striped table-hover" Width="100%" AutoGenerateColumns="False" DataKeyNames="id" AllowPaging="True" PageSize="10" OnPageIndexChanging="gvList_PageIndexChanging" ShowHeaderWhenEmpty="True">
                        <PagerStyle HorizontalAlign="Center" CssClass="pagination-centered" />
                        <Columns>
                            <asp:BoundField DataField="id" HeaderText="#ID" Visible="false" />
                            <asp:BoundField DataField="codcli" HeaderText="CÓDIGO" />
                            <asp:BoundField DataField="tipo" HeaderText="TIPO" />
                            <asp:BoundField DataField="nomcli" HeaderText="NOME FANTASIA" />
                            <asp:BoundField DataField="unidade" HeaderText="UNIDADE" />
                            <asp:BoundField DataField="regiao" HeaderText="REGIÃO   " />
                            <asp:BoundField DataField="cidcli" HeaderText="CIDADE" />
                            <asp:BoundField DataField="estcli" HeaderText="UF" />
                            <asp:BoundField DataField="ativo_inativo" HeaderText="SITUAÇÃO" />

                            <asp:TemplateField HeaderText="AÇÕES" ShowHeader="True" >
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkEditar" runat="server" OnClick="Editar" CssClass="btn btn-primary btn-sm"><i class="fa fa-edit"></i> Editar 
                                 </asp:LinkButton>
                                       <asp:LinkButton ID="lnkMapa" runat="server" OnClick="Mapa" CssClass="btn btn-info btn-sm"> <i class="fas fa-map-marker-alt"></i> Mapa 
                                           </asp:LinkButton>
                                    <%--<a class="btn btn-primary btn-sm" href="Frm_AltClientes.aspx?=<%# Eval("codcli") %>">
                                <i class="fa fa-edit"></i>
                                Editar
                             </a> --%>

                                  <%--  <a class="btn btn-info btn-sm">
                                        <i class="fas fa-map-marker-alt"></i>
                                        
                                    </a>--%>
                                   <%-- <asp:LinkButton ID="lnkExcluir" runat="server" OnClick="Excluir" CssClass="btn btn-danger btn-sm" OnClientClick="javascript:ConfirmMessage();"><i class="fa fa-trash"></i></i>
                                  </asp:LinkButton>--%>

                                    <%-- <a class="btn btn-danger btn-sm" href="Frm_AltClientes.aspx?id=">
                                <i class="fa fa-trash"></i>                                    
                                Excluir
                             </a> --%>
                                </ItemTemplate>
                            </asp:TemplateField>


                        </Columns>
                    </asp:GridView>

                </table>
                <asp:HiddenField ID="txtconformmessageValue" runat="server" />
            </div>
        </div>
    </div>
    <footer class="main-footer">
       <div class="float-right d-none d-sm-block">
         <b>Version</b> 2.1.0
       </div>
       <strong>Copyright &copy; 2021-2025 Capit Logística.</strong> Todos os direitos reservados.
    </footer>
  
    <script>
        function myFunction() {
            // Declare variables
            var input, filter, table, tr, td, i, txtValue;
            input = document.getElementById("myInput");
            filter = input.value.toUpperCase();
            table = document.getElementById("example1");
            tr = table.getElementsByTagName("tr");

            // Loop through all table rows, and hide those who don't match the search query
            for (i = 0; i < tr.length; i++) {
                td = tr[i].getElementsByTagName("td")[0];
                if (td) {
                    txtValue = td.textContent || td.innerText;
                    if (txtValue.toUpperCase().indexOf(filter) > -1) {
                        tr[i].style.display = "";
                    } else {
                        tr[i].style.display = "none";
                    }
                }
            }
        }
    </script>


</asp:Content>
