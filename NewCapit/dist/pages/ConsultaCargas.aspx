<%@ Page Title="" Language="C#" MasterPageFile="Main.Master" AutoEventWireup="true" CodeBehind="ConsultaCargas.aspx.cs" Inherits="NewCapit.dist.pages.ConsultaCargas" %>

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
                                <i class="fas fa-shipping-fast"></i>&nbsp;Consulta Cargas</h1>
                            <a href="/dist/pages/Frm_CadCargas.aspx" class="d-none d-sm-inline-block btn btn-sm btn-primary shadow-sm">
                                <i class="fas fa-shipping-fast"></i>&nbsp;Nova Carga
                            </a>
                        </div>                       
                    </div>
                    <div class="row g-3">
                       <div class="col-md-2">
                            <div class="form-group">
                                <span class="">Prev. Inicial:</span>
                                <div class="input-group date" id="prevInicial" data-target-input="nearest">
                                    <input type="text" class="form-control datetimepicker-input" data-target="#prevInicial"/>
                                    <div class="input-group-append" data-target="#prevInicial" data-toggle="datetimepicker">
                                        <div class="input-group-text"><i class="fa fa-calendar"></i></div>
                                    </div>
                                </div>
                            </div>
                       </div>
                      
                       <div class="col-md-2">
                            <div class="form-group">
                                <span class="">Prev. Final:</span>
                                <div class="input-group date" id="prevFinal" data-target-input="nearest">
                                    <input type="text" class="form-control datetimepicker-input" data-target="#prevFinal"/>
                                    <div class="input-group-append" data-target="#prevFinal" data-toggle="datetimepicker">
                                        <div class="input-group-text"><i class="fa fa-calendar"></i></div>
                                    </div>
                                </div>
                            </div>
                       </div>   
                        
                       <div class="col-md-2">
                         <div class="form-group">
                             <span class="">Status:</span>
                             <asp:DropDownList ID="ddlStatus" ame="nomeStatus"  runat="server" CssClass="form-control"></asp:DropDownList>
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
                           <table id="example1" class="table table-bordered table-striped table-hover table-responsive">
                              <asp:GridView runat="server" ID="gvListCargas" CssClass="table table-bordered table-striped table-hover" Width="100%" AutoGenerateColumns="False" DataKeyNames="id">
                                <Columns>
                                   <asp:BoundField DataField="id" HeaderText="#ID" />
                                   <asp:BoundField DataField="carga" HeaderText="CARGA" />
                                   <asp:BoundField DataField="peso" HeaderText="PESO" />
                                   <asp:BoundField DataField="status" HeaderText="STATUS" />
                                   <asp:BoundField DataField="cliorigem" HeaderText="REMETENTE" />
                                   <asp:BoundField DataField="clidestino" HeaderText="DESTINATÁRIO" />
                                   <asp:BoundField DataField="previsao" HeaderText="PREV.ENTREGA" />
                                   <asp:BoundField DataField="situacao" HeaderText="SITUAÇÃO" />   

                                   <asp:TemplateField HeaderText="AÇÕES" ShowHeader="True" >
                                   <ItemTemplate>
                                   <asp:LinkButton ID="lnkEditar" runat="server" CssClass="btn btn-primary btn-sm"><i class="fa fa-edit"></i></asp:LinkButton>                                                     
                                      <asp:LinkButton ID="lnkExcluir" runat="server" OnClick="Excluir" CssClass="btn btn-danger btn-sm" OnClientClick="javascript:ConfirmMessage();"><i class="fa fa-trash"></i></i>
</asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                  </Columns>
                              </asp:GridView>
                           </table>
                             <asp:HiddenField ID="txtconformmessageValue" runat="server" />
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
   

    <!-- JavaScript -->
<script src="vendor/datatables/jquery.dataTables.min.js"></script>
    
    <script>
        $(function () {
            //Datemask dd/mm/yyyy
            $('#datemask').inputmask('dd/mm/yyyy', { 'placeholder': 'dd/mm/yyyy' })
            //Datemask2 mm/dd/yyyy
            $('#datemask2').inputmask('mm/dd/yyyy', { 'placeholder': 'mm/dd/yyyy' })
            //Money Euro
            $('[data-mask]').inputmask()

            //Date picker
            $('#prevInicial').datetimepicker({
                format: 'L'
            });

            //Date picker
            $('#prevFinal').datetimepicker({
                format: 'L'
            });

            //Date and time picker
            $('#reservationdatetime').datetimepicker({ icons: { time: 'far fa-clock' } });

            //Date range picker
            $('#reservation').daterangepicker()
            //Date range picker with time picker
            $('#reservationtime').daterangepicker({
                timePicker: true,
                timePickerIncrement: 30,
                locale: {
                    format: 'MM/DD/YYYY hh:mm A'
                }
            })
            //Date range as a button
            $('#daterange-btn').daterangepicker(
                {
                    ranges: {
                        'Today': [moment(), moment()],
                        'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
                        'Last 7 Days': [moment().subtract(6, 'days'), moment()],
                        'Last 30 Days': [moment().subtract(29, 'days'), moment()],
                        'This Month': [moment().startOf('month'), moment().endOf('month')],
                        'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
                    },
                    startDate: moment().subtract(29, 'days'),
                    endDate: moment()
                },
                function (start, end) {
                    $('#reportrange span').html(start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'))
                }
            )

            //Timepicker
            $('#timepicker').datetimepicker({
                format: 'LT'
            })
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
        });

        function ConfirmMessage() {
            var selectedvalue = confirm("Exclusão de Dados\nTem certeza de que deseja excluir a carga permanentemente?");
            if (selectedvalue) {
                document.getElementById('<%=txtconformmessageValue.ClientID %>').value = "Yes";
    } else {
                document.getElementById('<%=txtconformmessageValue.ClientID %>').value = "No";
            }
        }


</script>
</asp:Content>
