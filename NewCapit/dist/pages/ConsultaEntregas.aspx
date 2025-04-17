<%@ Page Title="" Language="C#" MasterPageFile="Main.Master" AutoEventWireup="true" CodeBehind="ConsultaEntregas.aspx.cs" Inherits="NewCapit.dist.pages.ConsultaEntregas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        function toggleDetalhes(row) {
            const detalhesRow = row.nextElementSibling;
            if (detalhesRow && detalhesRow.classList.contains('detalhes')) {
                detalhesRow.classList.toggle('d-none');
            }
        }
    </script>
    <style>
        .d-none { display: none; }
     </style>
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    
    
    <div class="content-wrapper">
        <!-- Main content -->
        <section class="content">
            <div class="container-fluid">
                <div class="content-header">
                    <div class="d-sm-flex align-items-center justify-content-between mb-4">
                        <h1 class="h3 mb-2 text-gray-800">
                            <i class="fas fa-shipping-fast"></i>&nbsp;Acompanhamento de Coletas/Entregas</h1>
                        <a href="/dist/pages/Frm_OrdemColetaCNT.aspx" class="d-none d-sm-inline-block btn btn-sm btn-primary shadow-sm">
                            <i class="fas fa-shipping-fast"></i>&nbsp;Ordem de Coleta
                        </a>
                    </div>
                </div>
                <div class="row g-3">
                    <div class="col-md-2">
                        <div class="form-group">
                            <span class="">Coleta Inicial:</span>
                            <div class="input-group date">
                                <asp:TextBox ID="txtInicioData" CssClass="form-control" TextMode="DateTimeLocal" runat="server"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <div class="form-group">
                            <span class="">Coleta Final:</span>
                            <div class="input-group date">
                                <asp:TextBox ID="txtFimData" CssClass="form-control" TextMode="DateTimeLocal" runat="server"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <div class="form-group">
                            <span class="">Status:</span>
                            <asp:DropDownList ID="ddlStatus" ame="nomeStatus" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <div class="form_group">
                            <span class="details">VEÍCULO:</span>
                            <asp:DropDownList ID="ddlVeiculosCNT" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-1">
                        <br />
                        <asp:LinkButton ID="lnkPesquisar" runat="server" CssClass="btn btn-info" OnClick="lnkPesquisar_Click"><i class='fas fa-search' ></i> Pesquisar</asp:LinkButton>
                    </div>

                </div>
            </div>
            <!-- /.container-fluid -->
        </section>
        <!-- Main content -->
        <section class="content">
            <div class="container-fluid">
                <div class="row">
                    <div class="col-12">
                        <!-- /.col -->
                        <div class="card">
                            <!-- /.card-header -->
                            <div class="card-body">
                                <asp:Label ID="lblMensagem" runat="server" Text=""></asp:Label>
                               <asp:Repeater ID="rptCarregamento" runat="server" OnItemDataBound="rptCarregamento_ItemDataBound">
                                    <HeaderTemplate>
                                        <table class="table table-bordered table-hover">
                                            <thead>
                                                <tr>
                                                    <th>Foto</th>
                                                    <th>Motorista</th>
                                                    <th>Veículo</th>
                                                    <th>Frota</th>
                                                    <th>Placa/Reboque</th>
                                                    <th>Coleta</th>
                                                    <th>CVA</th>
                                                    <th>Situação</th>
                                                    <th>Material</th>
                                                    <th>Cadastro</th>
                                                    <th>Carregamento</th>
                                                    <th>Ação</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr onclick="toggleDetalhes(this)">
                                            <td>
                                                <img src='<%# Eval("fotos") %>' alt="Foto" style="width: 60px;" />
                                            </td>
                                            <td><%# Eval("nomemotorista") %></td>
                                            <td><%# Eval("tipoveiculo") %></td>
                                            <td><%# Eval("veiculo") %></td>
                                            <td><%# Eval("veiculo_reboques") %></td>
                                            <td><%# Eval("carga") %></td>
                                            <td><%# Eval("cva") %></td>
                                            <td><%# Eval("situacao") %></td>
                                            <td><%# Eval("material") %></td>
                                            <td><%# Eval("dtcad") %></td>
                                            <td><%# Eval("num_carregamento") %></td>
                                            <td>
                                                <asp:LinkButton ID="lnkEditar" runat="server" class="btn btn-info">
                                                    <i class="fas fa-tasks"></i>
                                                </asp:LinkButton>
                                            </td>
                                        </tr>
                                        <tr class="detalhes d-none">
                                            <td colspan="11">
                                                <asp:Repeater ID="rptColeta" OnItemDataBound="rptColeta_ItemDataBound" runat="server">
                                                    <HeaderTemplate>
                                                        <table class="table table-bordered table-hover mb-0">
                                                            <thead>
                                                                <tr>
                                                                    <th>COLETA</th>
                                                                    <th>CVA</th>
                                                                    <th>DATA COLETA</th>
                                                                    <th>CODIGO</th>
                                                                    <th>ORIGEM</th>
                                                                    <th>CODIGO</th>
                                                                    <th>DESTINO</th>
                                                                    <th>ATENDIMENTO</th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td><%# Eval("carga") %></td>
                                                            <td><%# Eval("cva") %></td>
                                                            <td><%# Eval("data_hora", "{0:dd/MM/yyyy HH:mm}") %></td>
                                                            <td><%# Eval("CodigoO") %></td>
                                                            <td><%# Eval("cliorigem") %></td>
                                                            <td><%# Eval("CodigoD") %></td>
                                                            <td><%# Eval("clidestino") %></td>
                                                            <td runat="server" id="tdAtendimento">
                                                                <asp:Label ID="lblAtendimento" runat="server" Text='<%# Eval("atendimento") %>' />
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                            </tbody>
                                                        </table>
                                                    </FooterTemplate>
                                                </asp:Repeater>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                            </tbody>
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>


                                        
                            </div>
                            <!-- /.card-body -->
                        </div>
                        <!-- /.card -->
                    </div>
                </div>
                <!-- /.row -->
            </div>
            <!-- /.container-fluid -->
        </section>
        <!-- /.content -->
    </div>
    <!-- /.content-wrapper -->
    <footer class="main-footer">
        <div class="float-right d-none d-sm-block">
            <b>Version</b> 3.1.0
 
        </div>
        <strong>Copyright &copy; 2023-2025 <a href="#">Capit Logística</a>.</strong> Todos os direitos reservados.
    </footer>

    <!-- jQuery -->

    <!-- Page specific script -->
    <script>
        $(function () {
            $("#example1").DataTable({
                "responsive": true, "lengthChange": false, "autoWidth": true,
                "buttons": ["copy", "csv", "excel", "pdf", "print", "colvis"]
            }).buttons().container().appendTo('#example1_wrapper .col-md-6:eq(0)');
            $('#example2').DataTable({
                "paging": true,
                "lengthChange": false,
                "searching": false,
                "ordering": true,
                "info": true,
                "autoWidth": false,
                "responsive": true,
            });

            new DataTable('#example1', {
                scrollX: true
            });
        });
    </script>
</asp:Content>
