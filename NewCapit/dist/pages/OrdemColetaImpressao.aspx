<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrdemColetaImpressao.aspx.cs" Inherits="NewCapit.dist.pages.OrdemColetaImpressao" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Ordem de Coleta de Carga</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            font-size: 12px;
            margin: 30px;
        }

        .titulo {
            font-weight: bold;
            font-size: 16px;
            text-align: right;
        }

        .header, .detalhes, .rodape {
            width: 100%;
            border: 1px solid black;
            border-collapse: separate;
            border-spacing: 0;
            margin-bottom: 10px;
            border-radius: 6px;
            overflow: hidden;
        }

        .header td, .detalhes td, .rodape td {
            border: 1px solid black;
            padding: 4px;
            vertical-align: top;
        }

        .logo-img {
            max-height: 70px;
        }

        .logo-wrapper {
            display: flex;
            justify-content: space-between;
            align-items: flex-start;
        }

        .info-texto {
            font-size: 11px;
            text-align: right;
        }

        .info-empresa-endereco {
            margin-top: 5px;
            font-size: 11px;
            text-align: center;
        }

        .center {
            text-align: center;
        }

        .bold {
            font-weight: bold;
        }

        .espaco {
            height: 100px;
        }

        .auto-style1 {
            width: 60%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <table class="header">
            <tr>
                <td class="auto-style1">
                    <div class="logo-wrapper">
                        <img src="../../img/logo_transnovag.png" alt="Logo" class="logo-img" />
                        <div class="info-texto">
                            Fone: (0xx11) 2126-3982<br />
                            C.N.P.J: 55.890.016/0008-85<br />
                            INSC. EST.: 117.096.586.112<br />
                            INSC. MUNICIPAL: 3.431.650-7
                        </div>
                    </div>
                    <div class="info-empresa-endereco">
                        Av. Santa Marina, 325 - <strong>Água Branca</strong> - CEP 05036-000 - São Paulo - SP
                    </div>
                </td>
                <td class="center" style="width: 60%;">
                    <div class="titulo center">ORDEM DE COLETA DE CARGA</div>
                    <div><strong>MOD. 20</strong></div>
                    <div><strong>SÉRIE B-1</strong></div>
                    <div><strong>Nº <span style="color: red;"><asp:Label ID="lblCarregamento" runat="server" Text=""></asp:Label></span></strong></div>
                    <div>1ª Via (Destinatário) - 2ª Via  (Remetente) - 3ª Via (Fisc.)</div>
                </td>
            </tr>
        </table>

        <table class="detalhes">
            <tr>
                <td colspan="2"><strong>Nome do Remetente:</strong> <asp:Label ID="lblRemetente" runat="server" Text=""></asp:Label></td>
                <td><strong>Inscr. Estadual:</strong> <asp:Label ID="lblIe" runat="server" Text=""></asp:Label></td>
            </tr>
            <tr>
                <td colspan="3"><strong>Endereço:</strong> <asp:Label ID="lblEndereco" runat="server" Text=""></asp:Label></td>
            </tr>
            <tr>
                <td colspan="3"><strong>CNPJ:</strong> <asp:Label ID="lblCnpj" runat="server" Text=""></asp:Label></td>
            </tr>
        </table>

        <table class="detalhes">
            <tr class="bold center">
                <td style="width: 15%;">QUANT. DO VOLUME</td>
                <td>DESCRIÇÃO DA CARGA A SER COLETADA<br />ESPÉCIE DO VOLUME OU MERCADORIA</td>
                <td style="width: 25%;">Nº E DATA DOC. FISCAL</td>
            </tr>
            <tr class="espaco">
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
        </table>

        <table class="detalhes">
            <tr class="bold center">
                <td style="width: 25%;">MOTORISTA</td>
                <td style="width: 10%;">PLACA</td>
                <td style="width: 15%;">ENC.</td>
                <td>CONTATO</td>
            </tr>
            <tr>
                <td><asp:Label ID="lblMotorista" runat="server" Text=""></asp:Label></td>
                <td><asp:Label ID="lblPlaca" runat="server" Text=""></asp:Label></td>
                <td>&nbsp;</td>
                <td><asp:Label ID="lblContato" runat="server" Text=""></asp:Label></td>
            </tr>
        </table>

        <table class="rodape">
            <tr class="bold center">
                <td>LOCAL</td>
                <td style="width: 15%;">DATA</td>
                <td style="width: 30%;">ASS. DO RECEBEDOR</td>
                <td style="width: 20%;">Nº DE CONTROLE DO FORMULÁRIO</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
        </table>
    </form>
</body>
</html>


