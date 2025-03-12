<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Mapa.aspx.cs" Inherits="NewCapit.dist.pages.Mapa" EnableEventValidation="true" %>
<%@ register assembly="GMaps" namespace="Subgurim.Controles" tagprefix="cc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<link rel="stylesheet" type="text/css" href="flexgrid/css/flexigrid.css"/>
<script type="text/javascript" src="flexgrid/js/flexigrid.js"></script>
 <script type="text/javascript" src="flexgrid/js/flexigrid.pack.js"></script>
<!-- FlexGrid-->
  <link rel="stylesheet" 
href="http://code.jquery.com/ui/1.10.3/themes/smoothness/jquery-ui.css" 
/>
<!--End FlexGrid-->
    
    <script type="text/javascript">
       
        var $j = jQuery.noConflict();
        var $a = jQuery.noConflict();
        $j(document).ready(function () {
            var obj;
            $j.ajax({
                type: "POST",
                contentType: 'application/xml; charset=utf-8',
                url: "Flexservice.asmx/GetData",
                data: '{}',
                dataType: 'xml',
                success: function (result) {
                    obj = $.parseJSON(result.d);
                    // add data
                    $a("#FlexTable1").flexAddData(formatEmployeeResults(obj));
                }
            });

            // init flexigrid
            
            $a("#FlexTable").flexigrid({
                url: "Flexservice.asmx/GetData",
                dataType: 'xml',
                colModel: [{
                    display: 'ID Veiculo',
                    name: 'nr_veiculo',
                    width: 70,
                    sortable: true,
                    align: 'center'
                    
                }, {
                    display: 'Placa',
                    name: 'ds_placa',
                    width: 70,
                    sortable: true,
                    align: 'left'
                }, {
                    display: 'Bloqueio',
                    name: 'fl_bloqueio',
                    width: 50,
                    sortable: true,
                    align: 'left'
                }, {
                    display: 'Cidade',
                    name: 'ds_cidade',
                    width: 100,
                    sortable: true,
                    align: 'left'
                }, {
                    display: 'Posicao',
                    name: 'dt_posicao',
                    width: 120,
                    sortable: true,
                    align: 'left'
                }, {
                    display: 'Dist. Referencia',
                    name: 'nr_dist_referencia',
                    width: 120,
                    sortable: true,
                    align: 'left'
                }, {
                    display: 'GPS',
                    name: 'nr_gps',
                    width: 50,
                    sortable: true,
                    align: 'left'
                }, {
                    display: 'Ignicao',
                    name: 'fl_ignicao',
                    width: 50,
                    sortable: true,
                    align: 'left'
                }, {
                    display: 'Jamming',
                    name: 'nr_jamming',
                    width: 50,
                    sortable: true,
                    align: 'left'
                },
                {
                    display: 'Latitude',
                    name: 'ds_lat',
                    width: 120,
                    sortable: true,
                    align: 'left'
                },
                {
                    display: 'Longitude',
                    name: 'ds_long',
                    width: 120,
                    sortable: true,
                    align: 'left'
                },
                {
                    display: 'Odomero',
                    name: 'nr_odometro',
                    width: 50,
                    sortable: true,
                    align: 'left'
                },
                {
                    display: 'Ponto Referencia',
                    name: 'nr_pontoreferencia',
                    width: 120,
                    sortable: true,
                    align: 'left'
                },
                {
                    display: 'Rua',
                    name: 'ds_rua',
                    width: 120,
                    sortable: true,
                    align: 'left'
                },
                {
                    display: 'Tensao',
                    name: 'nr_tensao',
                    width: 50,
                    sortable: true,
                    align: 'left'
                },
                {
                    display: 'Satelite',
                    name: 'nr_satelite',
                    width: 50,
                    sortable: true,
                    align: 'left'
                },
                {
                    display: 'UF',
                    name: 'ds_uf',
                    width: 30,
                    sortable: true,
                    align: 'left'
                },
                {
                    display: 'Velocidade',
                    name: 'nr_velocidade',
                    width: 60,
                    sortable: true,
                    align: 'left'
                }],


                searchitems: [{
                    display: 'ID Veículo',
                    name: 'nr_idveiculo'
                }, {
                    display: 'Placa',
                    name: 'ds_placa',
                    isdefault: true
                }],
                sortname: "nr_idveiculo",
                              
                sortorder: "asc",
                usepager: true,
                title: 'Grid',
                useRp: false,
                rp: 50,
                showTableToggleBtn: true,
                width: 1240,
                height: 200

            });

            function formatEmployeeResults(Veiculos) {

                var rows = Array();

                for (i = 0; i < Veiculos.length; i++) {
                    var item = Veiculos[i];
                    item.page,
                    item.total,
                    rows.push({
                        cell: [item.nr_idveiculo,
                               item.ds_placa,
                               item.fl_bloqueio,
                               item.ds_cidade,
                               item.dt_posicao,
                               item.nr_dist_referencia,
                               item.nr_gps,
                               item.fl_ignicao,
                               item.nr_jamming,
                               item.ds_lat,
                               item.ds_long,
                               item.nr_odometro,
                               item.nr_pontoreferencia,
                               item.ds_rua,
                               item.nr_tensao,
                               item.nr_satelite,
                               item.ds_uf,
                               item.nr_velocidade
                        ]

                    });
                }
                return {
                    total: Veiculos.length,
                    page: 1,
                    rows: rows
                }
            }
        });
</script>
 <script type="text/javascript">jQuery.noConflict();</script>
<script type="text/javascript">
    //<![CDATA[
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
    <script src="//maps.googleapis.com/maps/api/js?key=AIzaSyApI6da0E4OJktNZ-zZHgL6A5jtk0L6Cww"></script>
<script type="text/javascript">

    function abre_relatorio(url, w, h) {
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
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div id="flexme1" ondblclick="return abre_mapa('mapa.aspx',1400,700);">
    
                             
             <cc1:GMap ID="GMap1" runat="server" Width="100%" Height="700px" enableServerEvents="True"  OnMarkerClick="GMap1_MarkerClick" />
    
         </div>
              <table id="FlexTable1" style="display:none"></table>
        </div>
    </form>
</body>
</html>
