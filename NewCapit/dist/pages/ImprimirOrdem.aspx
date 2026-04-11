<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImprimirOrdem.aspx.cs" Inherits="NewCapit.dist.pages.ImprimirOrdem" %>

<html lang="pt-br">
<head runat="server">
    <meta charset="utf-8" />
    <title>Impressão - Ordem de Abastecimento</title>
    <style>
    @page { 
        size: A4; 
        margin: 5mm; 
    }
    body { 
        font-family: "Courier New", Courier, monospace; 
        font-size: 9px; 
        margin: 0; 
        padding: 0; 
    }
    .wrapper { 
        position: relative;
        width: 100%; 
        max-width: 710px; 
        margin: 0 auto; 
        border: 1px solid #000; 
        padding: 5px; 
        box-sizing: border-box;
        page-break-inside: avoid;
    }
    .header-table { width: 100%; border-bottom: 1px solid #000; }
    .content-table { width: 100%; border-collapse: collapse; margin-top: 3px; }
    .content-table td { border: 1px solid #000; padding: 2px 4px; line-height: 1; }
    .title { font-size: 11px; font-weight: bold; text-align: center; text-decoration: underline; margin: 3px 0; }
    
    /* Linha de corte */
    .linha-corte { 
        width: 100%; 
        margin: 8px auto; 
        border-top: 1px dashed #000; 
        position: relative;
    }
    .linha-corte::after {
        content: "✂ CORTE AQUI";
        font-size: 7px;
        position: absolute;
        top: -6px;
        right: 15px;
        background: #fff;
        padding: 0 5px;
    }

    /* Ajuste para REIMPRESSÃO não encavalar */
    .info-footer {
        position: relative;
        min-height: 35px; /* Reserva espaço para a etiqueta e os dados de emissão */
        margin-top: 10px;
        border-top: 1px solid #eee; /* Opcional: linha guia leve */
    }
    .reimpressao-texto {
        position: absolute;
        top: 0;
        left: 50%;
        transform: translateX(-50%);
        font-weight: bold;
        font-size: 10px;
        text-align: center;
        width: 100%;
    }
    .emissao-dados {
        margin-top: 15px; /* Garante que os dados fiquem abaixo da reimpressão */
    }

    .footer-sigs { margin-top: 10px; display: flex; justify-content: space-between; }
    .sig-box { border-top: 1px solid #000; width: 30%; text-align: center; padding-top: 2px; }

    .qrcode-container { margin-top: 5px; display: flex; justify-content: flex-end; }
    .qrcode-box { width: 150px; height: 150px; border: 1px solid #000; display: flex; align-items: center; justify-content: center; }
    .qrcode-box img { width: 140px; height: 140px; }
</style>
</head>
<body onload="window.print();">
    <form id="form1" runat="server">
        <asp:Literal ID="litConteudo" runat="server"></asp:Literal>
    </form>
</body>
</html>