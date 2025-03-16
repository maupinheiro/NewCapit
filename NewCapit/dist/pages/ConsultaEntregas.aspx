<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ConsultaEntregas.aspx.cs" Inherits="NewCapit.dist.pages.ConsultaEntregas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- Google Font: Source Sans Pro -->
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,400i,700&display=fallback">
    <!-- Font Awesome -->
    <link rel="stylesheet" href="../../plugins/fontawesome-free/css/all.min.css">
    <!-- DataTables -->
    <link rel="stylesheet" href="../../plugins/datatables-bs4/css/dataTables.bootstrap4.min.css">
    <link rel="stylesheet" href="../../plugins/datatables-responsive/css/responsive.bootstrap4.min.css">
    <link rel="stylesheet" href="../../plugins/datatables-buttons/css/buttons.bootstrap4.min.css">
    <!-- Theme style -->
    <link rel="stylesheet" href="../../dist/css/adminlte.min.css">


    <body class="hold-transition sidebar-mini">
        <div class="content-wrapper">
            <!-- Main content -->
            <section class="content">
                <div class="container-fluid">
                    <div class="content-header">
                        <div class="d-sm-flex align-items-center justify-content-between mb-4">
                            <h1 class="h3 mb-2 text-gray-800">
                                <i class="fas fa-shipping-fast"></i>&nbsp;Consulta Coletas/Entregas</h1>
                            <a href="/dist/pages/Frm_CadCargas.aspx" class="d-none d-sm-inline-block btn btn-sm btn-primary shadow-sm">
                                <i class="fas fa-shipping-fast"></i>&nbsp;Novo
                            </a>
                        </div>
                    </div>
                    <div class="row g-3">
                        <div class="col-md-2">
                            <div class="form-group">
                                <span class="">Previsão Inicial:</span>
                                <div class="input-group date" id="prevInicial" data-target-input="nearest">
                                    <input type="text" class="form-control datetimepicker-input" data-target="#prevInicial" />
                                    <div class="input-group-append" data-target="#prevInicial" data-toggle="datetimepicker">
                                        <div class="input-group-text"><i class="fa fa-calendar"></i></div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-2">
                            <div class="form-group">
                                <span class="">Previsão Final:</span>
                                <div class="input-group date" id="prevFinal" data-target-input="nearest">
                                    <input type="text" class="form-control datetimepicker-input" data-target="#prevFinal" />
                                    <div class="input-group-append" data-target="#prevFinal" data-toggle="datetimepicker">
                                        <div class="input-group-text"><i class="fa fa-calendar"></i></div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-2">
                            <div class="form-group">
                                <span class="">Status:</span>
                                <asp:DropDownList ID="ddlStatus" ame="nomeStatus" runat="server" CssClass="form-control"></asp:DropDownList>
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
    </body>
<!-- jQuery -->
<script src="../../plugins/jquery/jquery.min.js"></script>
<!-- Bootstrap 4 -->
<script src="../../plugins/bootstrap/js/bootstrap.bundle.min.js"></script>
<!-- DataTables  & Plugins -->
<script src="../../plugins/datatables/jquery.dataTables.min.js"></script>
<script src="../../plugins/datatables-bs4/js/dataTables.bootstrap4.min.js"></script>
<script src="../../plugins/datatables-responsive/js/dataTables.responsive.min.js"></script>
<script src="../../plugins/datatables-responsive/js/responsive.bootstrap4.min.js"></script>
<script src="../../plugins/datatables-buttons/js/dataTables.buttons.min.js"></script>
<script src="../../plugins/datatables-buttons/js/buttons.bootstrap4.min.js"></script>
<script src="../../plugins/jszip/jszip.min.js"></script>
<script src="../../plugins/pdfmake/pdfmake.min.js"></script>
<script src="../../plugins/pdfmake/vfs_fonts.js"></script>
<script src="../../plugins/datatables-buttons/js/buttons.html5.min.js"></script>
<script src="../../plugins/datatables-buttons/js/buttons.print.min.js"></script>
<script src="../../plugins/datatables-buttons/js/buttons.colVis.min.js"></script>
<!-- AdminLTE App -->
<script src="../../dist/js/adminlte.min.js"></script>
<!-- AdminLTE for demo purposes -->
<script src="../../dist/js/demo.js"></script>
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
