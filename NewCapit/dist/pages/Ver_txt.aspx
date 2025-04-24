<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Ver_txt.aspx.cs" Inherits="NewCapit.dist.pages.Ver_txt" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
       <meta charset="utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <meta name="description" content="Creative - Bootstrap 3 Responsive Admin Template"/>
    <meta name="author" content="GeeksLabs"/>
    <meta name="keyword" content="Creative, Dashboard, Admin, Template, Theme, Bootstrap, Responsive, Retina, Minimal"/>
    <link rel="shortcut icon" href="img/favicon.png"/>

    <title>.:CAPit - Logística:.</title>

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

<!--End FlexGrid-->
    <script src="js/jquery.mask.js" type="text/javascript"></script>
  <script>
    var $j = jQuery.noConflict();
        $j(document).ready(function () {
            $j('#txtDtchegadaprevista').mask('00/00/0000');
            $j('#txtHrchegadaprevista').mask('00:00:00');
            
        });
</script>

    <script>
        function close() {
            windoows.close();
            }
        
    </script>

    <script>
        function formatar(src, mask) {
            var i = src.value.length;
            var saida = mask.substring(0, 1);
            var texto = mask.substring(i)
            if (texto.substring(0, 1) != saida) {
                src.value += texto.substring(0, 1);
            }
        }
    </script>
    


</head>
    
<body>
    <form id="sform" runat="server">
    <div style="margin-left:10px;">
        <section id="main-content" style="margin-left:10px;">
          <section class="wrapper">
		  <div class="row">
				<div class="col-lg-12">
					<h3 class="page-header"><i class="fa fa-files-o"></i> Gerar Arquivos TXT</h3>
					
				</div>
			</div>
              <!-- Form validations -->              
              <div class="row" style="width:400px;">
                  <div class="col-lg-12">
                      <section class="panel">
                          <header class="panel-heading">
                           
                          </header>
                          <div class="panel-body">
                             
                              <div class="form">

             
            <div>
        <b>Data Inicial</b><br />
        <asp:TextBox ID="txtDtInicial" CssClass="form-control" runat="server" OnKeyPress="formatar(this, '##/##/####')" MaxLength="10"></asp:TextBox>
    </div>                                     
            <div>
        <b>Data Final</b><br />
        <asp:TextBox ID="txtDtFinal" CssClass="form-control" runat="server" OnKeyPress="formatar(this, '##/##/####')" MaxLength="10"></asp:TextBox>
    </div>
       
        <div style="width: 135px">
            <asp:CheckBox ID="chkDiadema" runat="server" Text="TNG CNT" AutoPostBack="true" OnCheckedChanged="chkDiadema_CheckedChanged"/>

        </div>
          <div style="width: 124px">
            <asp:CheckBox ID="chkCadiriri" runat="server" Text="TNG MATRIZ" AutoPostBack="true" OnCheckedChanged="chkCadiriri_CheckedChanged"/>
        </div>
        <div style="width: 124px">
            <asp:CheckBox ID="chkIpiranda" runat="server" Text="TNG IPIRANGA" AutoPostBack="true" OnCheckedChanged="chkIpiranda_CheckedChanged"/>
        </div>
        <div style="width: 124px">
            <asp:CheckBox ID="chkMinas" runat="server" Text="TNG MINAS" AutoPostBack="true" OnCheckedChanged="chkMinas_CheckedChanged"/>
        </div>
         <div style="width: 124px">
            <asp:CheckBox ID="chkBahia" runat="server" Text="TNG BAHIA" AutoPostBack="true" OnCheckedChanged="chkBahia_CheckedChanged"/>
        </div><br />
         
        <div>
            <asp:Button ID="btnGeraArquivo" CssClass="btn btn-outline-success btn-lg" runat="server" Text="Gerar" OnClick="btnGeraArquivo_Click" />
        </div>
             <br />
         <div>
             <asp:DropDownList ID="DropDownList1" CssClass="form-control" runat="server"  OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged"  AutoPostBack="true"></asp:DropDownList>
        </div>
       
                                  
               <p></p>
            

        
           
           
                              </div>

                          </div>
                      </section>
                  </div>
              </div>
        
              
              <!-- page end-->
          </section>
      </section>
    </div>
    </form>
     <!-- container section end -->

   <!-- container section end -->
    <!-- javascripts -->
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

<!-- jQuery Knob -->
<script src="../../plugins/jquery-knob/jquery.knob.min.js"></script>
<!-- Sparkline -->
<script src="../../plugins/sparklines/sparkline.js"></script>
<!-- AdminLTE for demo purposes -->
<script src="../../dist/js/demo.js"></script>
<!-- AdminLTE App -->
<script src="../../dist/js/adminlte.min.js"></script>
</body>
</html>
