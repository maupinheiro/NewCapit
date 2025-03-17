<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="~/dist/pages/Mapa.aspx.cs" Inherits="NewCapit.dist.pages.Mapa" EnableEventValidation="true" %>
<%@ register assembly="GMaps" namespace="Subgurim.Controles" tagprefix="cc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <!-- Google Font: Source Sans Pro -->
 
 <!-- Theme style -->
 <link rel="stylesheet" href="../../dist/css/adminlte.min.css"/>
    <script>
    <![CDATA[
    jQuery(document).ready(function ($) {
        $('#flexme1').flexigrid({

            title: 'Mapa',
            useRp: true,
            rp: 5,
            showTableToggleBtn: true,
            width: 1240,
            height: 300
        });
    });
    //]]>
</script>
 <style type="text/css">
.box {
    border: 1px solid black;
    background: yellow;
    padding: 5px;
}
</style>

</head>
<body>
    <form id="form1" runat="server">
       

       <div id="flexme1">
    
    
<cc1:GMap ID="GMap1" runat="server" Width="100%" Height="700px" Key="AIzaSyApI6da0E4OJktNZ-zZHgL6A5jtk0L6Cww" enableServerEvents="True"  OnMarkerClick="GMap1_MarkerClick" />
    
    </div>
    
    
       
       
        <div>
           
             
        </div>
    </form>
</body>




</html>
