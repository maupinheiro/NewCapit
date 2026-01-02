<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="ControleDePedagio.aspx.cs" Inherits="NewCapit.dist.pages.ControleDePedagio" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- Bootstrap 5 -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>

    <script src="https://code.jquery.com/jquery-3.7.1.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery.mask/1.14.16/jquery.mask.min.js"></script>

    <!-- Chart.js -->
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>

    <script src="https://cdn.datatables.net/1.13.6/js/jquery.dataTables.min.js"></script>
    <link href="https://cdn.datatables.net/1.13.6/css/jquery.dataTables.min.css" rel="stylesheet" />
       
  


    <script>
        function abrirModalProcesso() {
            var modalEl = document.getElementById('mdlProcesso');
            var modal = new bootstrap.Modal(modalEl);
            modal.show();
        }
        function mascaraDataHora(campo) {
            let v = campo.value.replace(/\D/g, '');

            if (v.length > 12)
                v = v.substring(0, 12);

            if (v.length >= 9) {
                campo.value =
                    v.substring(0, 2) + '/' +
                    v.substring(2, 4) + '/' +
                    v.substring(4, 8) + ' ' +
                    v.substring(8, 10) +
                    (v.length >= 11 ? ':' + v.substring(10, 12) : '');
            }
            else if (v.length >= 5) {
                campo.value =
                    v.substring(0, 2) + '/' +
                    v.substring(2, 4) + '/' +
                    v.substring(4, 8);
            }
            else if (v.length >= 3) {
                campo.value =
                    v.substring(0, 2) + '/' +
                    v.substring(2, 4);
            }
            else {
                campo.value = v;
            }
        }


    </script>
    <script>
        function formatarMoeda(campo) {
            let valor = campo.value.replace(/\D/g, "");
            if (valor === "") return;

            valor = (valor / 100).toFixed(2) + "";
            valor = valor.replace(".", ",");
            valor = valor.replace(/\B(?=(\d{3})+(?!\d))/g, ".");
            campo.value = "R$ " + valor;
        }
    </script>
   
    <div class="content-wrapper">
        <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server" />
        <section class="content">
            <div class="container-fluid">               
                <br />
                <div class="col-md-12">
                    <div class="card card-info">
                        <div class="card-header">
                            <h3 class="card-title">
                                <h3 class="card-title"><i class="fas fa-map-marker-alt"></i>&nbsp;CONTROLE DE PEDÁGIOS</h3>
                            </h3>
                            <div class="card-tools">
                                <button type="button" class="btn btn-tool" data-card-widget="maximize">
                                    <i class="fas fa-expand"></i>
                                </button>
                                <button type="button" class="btn btn-tool" data-card-widget="collapse">
                                    <i class="fas fa-minus"></i>
                                </button>
                                <button type="button" class="btn btn-tool" data-card-widget="remove">
                                    <i class="fas fa-times"></i>
                                </button>
                            </div>
                            <!-- /.card-tools -->
                        </div>
                        <div class="card-body">
                            <div class="container-fluid">

                               
                            </div>
                        </div>
                    </div>
                </div>
                <!-- form Pedagio -->
             
                
            </div>
        </section>
    </div>
</asp:Content>
