<%@ Page Title="" Language="C#" MasterPageFile="Main.Master" AutoEventWireup="true" CodeBehind="ConsultaEntregas.aspx.cs" Inherits="NewCapit.dist.pages.ConsultaEntregas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="content-wrapper">
            <!-- Main content -->
            <section class="content">
                <div class="container-fluid">
                    <div class="content-header">
                        <div class="d-sm-flex align-items-center justify-content-between mb-4">
                            <h1 class="h3 mb-2 text-gray-800">
                                <i class="fas fa-shipping-fast"></i>&nbsp;Consulta Coletas/Entregas</h1>
                            <a href="/dist/pages/Frm_OrdemColetaCNT.aspx" class="d-none d-sm-inline-block btn btn-sm btn-primary shadow-sm">
                                <i class="fas fa-shipping-fast"></i>&nbsp;Novo
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
                    <asp:LinkButton ID="lnkPesquisar" runat="server" CssClass="btn btn-info"><i class='fas fa-search' ></i>
Pesquisar</asp:LinkButton>
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
  <table id="example1" class="table table-bordered table-striped table-hover">
    <thead>
      <tr>
        <th>Foto</th>
        <th>Veículo</th>
        <th>Placa/Reboque</th>
        <th>Motorista</th>
        <th>Remetente</th>
        <th>Destinatário</th>
        <th>Situação</th>
        <th>Material</th>
      </tr>
    </thead>
    <tbody>
      <tr>
        <td>Tri</td>
        <td>256</td>
        <td>FZY5E66/DTE9D42</td>
        <td>PAULO DOS SANTOS CALDEIRA</td>
        <td>1111 - TRANSNOVAG - MATRIZ</td>
        <td>3002 - BENTELER (CAMPINAS)</td>
        <td>BARRA </td>
        <td>CHAPA</td>
      </tr>
      
       
    </tbody>

  </table>
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
