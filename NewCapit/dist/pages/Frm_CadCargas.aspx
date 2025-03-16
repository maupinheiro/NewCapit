<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Frm_CadCargas.aspx.cs" Inherits="NewCapit.dist.pages.FrmCadCargas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <!-- Google Font: Source Sans Pro -->
 <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,400i,700&display=fallback">
 <!-- Font Awesome -->
 <link rel="stylesheet" href="../../plugins/fontawesome-free/css/all.min.css">
 <!-- daterange picker -->
 <link rel="stylesheet" href="../../plugins/daterangepicker/daterangepicker.css">
 <!-- iCheck for checkboxes and radio inputs -->
 <link rel="stylesheet" href="../../plugins/icheck-bootstrap/icheck-bootstrap.min.css">
 <!-- Bootstrap Color Picker -->
 <link rel="stylesheet" href="../../plugins/bootstrap-colorpicker/css/bootstrap-colorpicker.min.css">
 <!-- Tempusdominus Bootstrap 4 -->
 <link rel="stylesheet" href="../../plugins/tempusdominus-bootstrap-4/css/tempusdominus-bootstrap-4.min.css">
 <!-- Select2 -->
 <link rel="stylesheet" href="../../plugins/select2/css/select2.min.css">
 <link rel="stylesheet" href="../../plugins/select2-bootstrap4-theme/select2-bootstrap4.min.css">
 <!-- Bootstrap4 Duallistbox -->
 <link rel="stylesheet" href="../../plugins/bootstrap4-duallistbox/bootstrap-duallistbox.min.css">
 <!-- BS Stepper -->
 <link rel="stylesheet" href="../../plugins/bs-stepper/css/bs-stepper.min.css">
 <!-- dropzonejs -->
 <link rel="stylesheet" href="../../plugins/dropzone/min/dropzone.min.css">
 <!-- Theme style -->
 <link rel="stylesheet" href="../../dist/css/adminlte.min.css">
    <script type="text/javascript">
        function preencherTextBox() {
            var comboBox = document.getElementById('<%= ddlTomador.ClientID %>');
            var textBox = document.getElementById('<%= txtGr.ClientID %>');
            textBox.value = comboBox.options[comboBox.selectedIndex].text;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   

    <div class="content-wrapper">
        <section class="content">
            <div class="container-fluid">
                <dciv class="card card-danger">
                    <div class="card-header">
                        <h3 class="card-title">CARGAS - NOVA CARGA</h3>
                    </div>
                </dciv>
            </div>
      
                <div class="card-header">
                    <!-- Linha 1 do formulario -->
                    <div class="col-md-1">
                        <div class="form-group">
                            <span class="">
                                <h1>000001</h1>
                            </span>
                        </div>
                    </div>
                    <!-- Linha 2 do formulario -->
                    <div class="row g-3">
                        <div class="col-md-2">
                            <div class="form_group">
                                <span class="details">FILIAL:</span>
                                <asp:DropDownList ID="cbFiliais" name="nomeFiliais" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="form-group">
                                <span class="">SOLICITANTE:</span>
                                 <asp:DropDownList ID="ddlSolicitante" runat="server" class="form-control" AutoPostBack="True" 
                              OnSelectedIndexChanged="DropDownListProdutos_SelectedIndexChanged">
                <asp:ListItem Text="Selecione uma categoria" Value="0"></asp:ListItem>
            </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <span class="">TOMADOR DO SERVIÇO:</span>
                                <asp:DropDownList ID="ddlTomador" runat="server"  CssClass="form-control"  ></asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form-group">
                                <span class="details">GERENCIADORA DE RISCO:</span>
                                <asp:TextBox ID="txtGr" runat="server" class="form-control" placeholder="" MaxLength="50"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="details">CADASTRO:</span>
                                <div class="input-group">
                                    <asp:Label ID="lblDtCadCarga" runat="server" Style="text-align: center" class="form-control" data-inputmask-alias="datetime" data-inputmask-inputformat="dd/mm/yyyy" data-mask></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                    <!-- Linha 3 do formulario -->
                    <div class="row g-3">
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="details">CÓDIGO:</span>
                                <asp:TextBox ID="txtCodCliOrigem" runat="server" class="form-control" placeholder="" MaxLength="5"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-5">
                            <div class="form_group">
                                <span class="details">REMETENTE:</span>
                                <asp:DropDownList ID="ddlRemetente" class="form-control select2" name="nomeRemetente" runat="server" AutoPostBack="true"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-5">
                            <div class="form-group">
                                <span class="details">INICIO DA PRESTAÇÃO:</span>
                                <asp:TextBox ID="txtMunicOrigem" runat="server" class="form-control" placeholder="" MaxLength="45"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="details">UF:</span>
                                <asp:TextBox ID="txtUFOrigem" runat="server" class="form-control" Style="text-align: center" placeholder="" MaxLength="2"></asp:TextBox>
                            </div>
                        </div>

                    </div>
                    <!-- Linha 4 do formulario -->
                    <div class="row g-3">
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="details">CÓDIGO:</span>
                                <asp:TextBox ID="txtCodCliDestino" runat="server" class="form-control" placeholder="" MaxLength="5"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-5">
                            <div class="form_group">
                                <span class="details">DESTINATÁRIO:</span>
                                <asp:DropDownList ID="ddlDestinatario" class="form-control select2" name="nomeRemetente" runat="server" AutoPostBack="true"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-5">
                            <div class="form-group">
                                <span class="details">TERMINO DA PRESTAÇÃO:</span>
                                <asp:TextBox ID="txtMunicDestinatario" runat="server" class="form-control" placeholder="" MaxLength="45"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="details">UF:</span>
                                <asp:TextBox ID="txtUFDestinatario" runat="server" class="form-control" Style="text-align: center" placeholder="" MaxLength="2"></asp:TextBox>
                            </div>
                        </div>

                    </div>
                    <!-- Linha 5 do formulário -->
                    <div class="row g-3">
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="details">PEDIDO:</span>
                                <asp:TextBox ID="txtNumPedido" runat="server" class="form-control" placeholder="" MaxLength="11"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <br />
                            <asp:Button ID="btnCliente" runat="server" Text="Pesquisar" CssClass="btn btn-outline-warning"  />
                        </div>
                        <div class="col-md-2">
                            <div class="form_group">
                                <span class="details">MATERIAL:</span>
                                <asp:DropDownList ID="ddlMaterial" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="SERV. FERROLENE" Text="SERV. FERROLENE"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="details">PESO:</span>
                                <asp:TextBox ID="txtPeso" runat="server" class="form-control" placeholder="" MaxLength="5"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form_group">
                                <span class="details">DEPOSITO:</span>
                                <asp:DropDownList ID="ddlLocalCarreg" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="SERV. FERROLENE" Text="SERV. FERROLENE"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form_group">
                                <span class="details">SITUAÇÃO:</span>
                                <asp:DropDownList ID="ddlSituacaoMaterial" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="EM PROCESSO" Text="EM PROCESSO"></asp:ListItem>
                                    <asp:ListItem Value="PRONTO" Text="PRONTO"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="details">CONT. CLIENTE:</span>
                                <asp:TextBox ID="txtControleCliente" runat="server" class="form-control" placeholder="" MaxLength="11"></asp:TextBox>
                            </div>
                        </div>                        
                        <div class="col-md-2">
                            <div class="form-group">
                                <span class="details">PREV. ENTREGA:</span>
                                <div class="input-group">
                                    <div class="input-group date" id="reservationdate" data-target-input="nearest">
                                        <input type="text" class="form-control datetimepicker-input" data-target="#reservationdate" />
                                        <div class="input-group-append" data-target="#reservationdate" data-toggle="datetimepicker">
                                            <div class="input-group-text"><i class="fa fa-calendar"></i></div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form_group">
                                <span class="details">ENTREGA:</span>
                                <asp:DropDownList ID="ddlEntrega" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="NORMAL" Text="NORMAL"></asp:ListItem>
                                    <asp:ListItem Value="IMEDIATA" Text="IMEDIATA"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <!-- Linha 5 do formulário -->
                    <div class="row g-3">
                        <div class="col-md-12">
                            <div class="form_group">
                                <span class="details">OBSERVAÇÕES NA CARGA:</span>
                                <textarea class="form-control" rows="4" placeholder="Observações ..."></textarea>
                            </div>
                        </div>
                    </div>
                    <!-- Linha 6 do Formulário -->
                    <div class="row g-3">
                        <div class="col-md-12">
                            <div class="form_group">

                            </div>
                        </div>
                    </div>
                    <!-- Linha 7 do formulário -->
                    <div class="row g-3">
                        <div class="col-md-2">
                            <div class="form-group">
                                <span class="details">CADASTRADO EM:</span>
                                <asp:Label ID="lblDtCadastro" runat="server" CssClass="form-control" placeholder=""></asp:Label>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <span class="details">POR:</span>
                                <asp:TextBox ID="txtUsuCadastro" runat="server" CssClass="form-control" placeholder="" MaxLength="60"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <!-- Linha 9 do formulário -->
                    <div class="row g-3">
                        <div class="col-md-1">
                            <asp:Button ID="btnSalvar1" CssClass="btn btn-outline-success  btn-lg" runat="server" Text="Salvar" />
                        </div>
                        <div class="col-md-1">
                            <a href="ConsultaClientes.aspx" class="btn btn-outline-danger btn-lg">Cancelar               
                            </a>
                        </div>
                    </div>
                </div>
          
        </section>
    </div>

    <footer class="main-footer">
        <div class="float-right d-none d-sm-block">
            <b>Version</b> 2.1.0  
        </div>
        <strong>Copyright &copy; 2021-2025 Capit Logística.</strong> Todos os direitos reservados.
    </footer>
    <!-- jQuery -->
    <script src="../../plugins/jquery/jquery.min.js"></script>
    <!-- Bootstrap 4 -->
    <script src="../../plugins/bootstrap/js/bootstrap.bundle.min.js"></script>
    <!-- Select2 -->
    <script src="../../plugins/select2/js/select2.full.min.js"></script>
    <!-- Bootstrap4 Duallistbox -->
    <script src="../../plugins/bootstrap4-duallistbox/jquery.bootstrap-duallistbox.min.js"></script>
    <!-- InputMask -->
    <script src="../../plugins/moment/moment.min.js"></script>
    <script src="../../plugins/inputmask/jquery.inputmask.min.js"></script>
    <!-- date-range-picker -->
    <script src="../../plugins/daterangepicker/daterangepicker.js"></script>
    <!-- bootstrap color picker -->
    <script src="../../plugins/bootstrap-colorpicker/js/bootstrap-colorpicker.min.js"></script>
    <!-- Tempusdominus Bootstrap 4 -->
    <script src="../../plugins/tempusdominus-bootstrap-4/js/tempusdominus-bootstrap-4.min.js"></script>
    <!-- Bootstrap Switch -->
    <script src="../../plugins/bootstrap-switch/js/bootstrap-switch.min.js"></script>
    <!-- BS-Stepper -->
    <script src="../../plugins/bs-stepper/js/bs-stepper.min.js"></script>
    <!-- dropzonejs -->
    <script src="../../plugins/dropzone/min/dropzone.min.js"></script>
    <!-- AdminLTE App -->
    <script src="../../dist/js/adminlte.min.js"></script>
    <!-- AdminLTE for demo purposes -->
    <script src="../../dist/js/demo.js"></script>
    <!-- Page specific script -->

    <script>
        $(function () {
            //Initialize Select2 Elements
            $('.select2').select2()

            //Initialize Select2 Elements
            $('.select2bs4').select2({
                theme: 'bootstrap4'
            })

            //Datemask dd/mm/yyyy
            $('#datemask').inputmask('dd/mm/yyyy', { 'placeholder': 'dd/mm/yyyy' })
            //Datemask2 mm/dd/yyyy
            $('#datemask2').inputmask('dd/mm/yyyy', { 'placeholder': 'dd/mm/yyyy' })
            //Money Euro
            $('[data-mask]').inputmask()

            //Date picker
            $('#reservationdate').datetimepicker({
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
                    format: 'DD/MM/YYYY hh:mm A'
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

            //Bootstrap Duallistbox
            $('.duallistbox').bootstrapDualListbox()

            //Colorpicker
            $('.my-colorpicker1').colorpicker()
            //color picker with addon
            $('.my-colorpicker2').colorpicker()

            $('.my-colorpicker2').on('colorpickerChange', function (event) {
                $('.my-colorpicker2 .fa-square').css('color', event.color.toString());
            })

            $("input[data-bootstrap-switch]").each(function () {
                $(this).bootstrapSwitch('state', $(this).prop('checked'));
            })

        })
        // BS-Stepper Init
        document.addEventListener('DOMContentLoaded', function () {
            window.stepper = new Stepper(document.querySelector('.bs-stepper'))
        })

        // DropzoneJS Demo Code Start
        Dropzone.autoDiscover = false

        // Get the template HTML and remove it from the doumenthe template HTML and remove it from the doument
        var previewNode = document.querySelector("#template")
        previewNode.id = ""
        var previewTemplate = previewNode.parentNode.innerHTML
        previewNode.parentNode.removeChild(previewNode)

        var myDropzone = new Dropzone(document.body, { // Make the whole body a dropzone
            url: "/target-url", // Set the url
            thumbnailWidth: 80,
            thumbnailHeight: 80,
            parallelUploads: 20,
            previewTemplate: previewTemplate,
            autoQueue: false, // Make sure the files aren't queued until manually added
            previewsContainer: "#previews", // Define the container to display the previews
            clickable: ".fileinput-button" // Define the element that should be used as click trigger to select files.
        })

        myDropzone.on("addedfile", function (file) {
            // Hookup the start button
            file.previewElement.querySelector(".start").onclick = function () { myDropzone.enqueueFile(file) }
        })

        // Update the total progress bar
        myDropzone.on("totaluploadprogress", function (progress) {
            document.querySelector("#total-progress .progress-bar").style.width = progress + "%"
        })

        myDropzone.on("sending", function (file) {
            // Show the total progress bar when upload starts
            document.querySelector("#total-progress").style.opacity = "1"
            // And disable the start button
            file.previewElement.querySelector(".start").setAttribute("disabled", "disabled")
        })

        // Hide the total progress bar when nothing's uploading anymore
        myDropzone.on("queuecomplete", function (progress) {
            document.querySelector("#total-progress").style.opacity = "0"
        })

        // Setup the buttons for all transfers
        // The "add files" button doesn't need to be setup because the config
        // `clickable` has already been specified.
        document.querySelector("#actions .start").onclick = function () {
            myDropzone.enqueueFiles(myDropzone.getFilesWithStatus(Dropzone.ADDED))
        }
        document.querySelector("#actions .cancel").onclick = function () {
            myDropzone.removeAllFiles(true)
        }
        // DropzoneJS Demo Code End
    </script>



</asp:Content>
