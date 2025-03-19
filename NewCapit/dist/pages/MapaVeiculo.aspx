<%@ Page Title="" Language="C#" MasterPageFile="Main.Master" AutoEventWireup="true" CodeBehind="MapaVeiculo.aspx.cs" Inherits="NewCapit.dist.pages.MapaVeiculo" %>
<%@ register assembly="GMaps" namespace="Subgurim.Controles" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<script type="text/javascript">

    function abre_mapa(url, w, h) {
        var newW = w + 100;
        var newH = h + 100;
        var left = (screen.width - newW) / 2;
        var top = (screen.height - newH) / 2;
        var newwindow = window.open(url, 'name', 'width=' + newW + ',height=' + newH + ',left=' + left + ',top=' + top);
        newwindow.resizeTo(newW, newH);

        //posiciona o popup no centro da tela
        newwindow.moveTo(left, top);
        newwindow.focus();
        return false;
    }
</script>
<div class="content-wrapper">
<section class="content">
    <div class="container-fluid">
        <br />
        <div class="card card-success">
            <div class="card-header">
                <h3 class="card-title">MAPA - BUSCA DE VEÍCULOS</h3>
            </div>
        </div>
        <div class="card-header">
            <div class="row g-3">
                <div class="col-md-1">
                    <div class="form-group">
                        <span class="details">PLACA:</span>
                        <asp:TextBox ID="txtPlaca" runat="server" CssClass="form-control" placeholder="" MaxLength="11" ></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-1">
                    <br />
                    <asp:Button ID="btnPlaca" runat="server" Text="Pesquisar" CssClass="btn btn-outline-warning" OnClick="btnPlaca_Click"  />
                </div>
                
           
            </div>
             <div class="row">
                 <div class="col-md-12" ondblclick="return abre_mapa('mapa.aspx',1400,700);">
                     <cc1:GMap ID="GMap1" runat="server" Width="100%" Height="700px" Key="AIzaSyApI6da0E4OJktNZ-zZHgL6A5jtk0L6Cww" enableServerEvents="True"  OnMarkerClick="GMap1_MarkerClick" />
                      
                     </div>
                 </div>
            </div>
        </div>
    </section>
    </div>
    

</asp:Content>
