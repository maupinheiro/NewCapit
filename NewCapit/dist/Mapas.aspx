<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Mapas.aspx.cs" Inherits="NewCapit.dist.Mapas" %>
<%@ register assembly="GMaps" namespace="Subgurim.Controles" tagprefix="cc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">

                                           
                                            
    <cc1:GMap ID="GMap2" runat="server" Width="100%" Height="870px" enableServerEvents="True" OnMarkerClick="GMap2_MarkerClick" />
                   
                 
   
</form>
</body>
</html>
