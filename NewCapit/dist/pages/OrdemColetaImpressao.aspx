<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrdemColetaImpressao.aspx.cs" Inherits="NewCapit.dist.pages.OrdemColetaImpressao" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Impressão da Ordem de Coleta</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            font-size: 12px;
            margin: 10px;
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
            max-height: 60px;
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
            height: 60px;
        }

        .auto-style1 {
            width: 60%;
        }

        .form-wrapper {
            margin: 0;
            padding: 0;
            page-break-inside: avoid;
        }

        .form-container {
            width: 100%;
            padding: 10px 0;
            box-sizing: border-box;
            page-break-inside: avoid;
        }

        .form-divider {
            border-top: 1px dashed #333;
            margin: 10px 0;
            height: 1px;
        }

        .page-break {
            display: none;
        }

        @media print {
            body {
                margin: 5mm;
            }

            .form-wrapper {
                page-break-inside: avoid;
            }

            .page-break {
                display: none !important;
            }

            .form-container {
                page-break-inside: avoid;
            }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Repeater ID="rptFormularios" runat="server">
            <ItemTemplate>
                <div class="form-wrapper">
                    <asp:Repeater ID="rptInterno" runat="server" DataSource='<%# Eval("Formularios") %>'>
                        <ItemTemplate>
                            <div class="form-container">
                                <table class="header">
                                    <tr>
                                        <td class="auto-style1">
                                            <div class="logo-wrapper">
                                                <img src="../../img/logo_transnovag.png" alt="Logo" class="logo-img" />
                                                <div class="info-texto">
                                                    Fone: (0xx11) 4071-9087<br />
                                                    C.N.P.J: 55.890.016/0003-70<br />
                                                    INSC. EST.: 28635877211<br />
                                                    <%--INSC. MUNICIPAL: 3.431.650-7--%>
                                                </div>
                                            </div>
                                            <div class="info-empresa-endereco">
                                                Av. Fukuichi Nakata, 138 - <strong>Piraporinha</strong> - CEP 09950-400 - Diadema - SP
                                            </div>
                                        </td>
                                        <td class="center" style="width: 60%;">
                                            <div class="titulo center">ORDEM DE COLETA DE CARGA</div>
                                            <div><strong>MOD. 20</strong></div>
                                            <div><strong>SÉRIE B-1</strong></div>
                                            <div><strong>Nº <span style="color: red;"><%# Eval("Numero") %></span></strong></div>
                                            <div>1ª Via (Destinatário) - 2ª Via (Remetente)</div>
                                        </td>
                                    </tr>
                                </table>

                                <table class="detalhes">
                                    <tr>
                                        <td colspan="2"><strong>Nome do Remetente:</strong> <%# Eval("Remetente") %></td>
                                        <td><strong>Inscr. Estadual:</strong> <%# Eval("IE") %></td>
                                    </tr>
                                    <tr>
                                        <td colspan="3"><strong>Endereço:</strong> <%# Eval("Endereco") %></td>
                                    </tr>
                                    <tr>
                                        <td colspan="3"><strong>CNPJ:</strong> <%# Eval("CNPJ") %></td>
                                    </tr>
                                </table>

                                <table class="detalhes">
                                    <tr class="bold center">
                                        <td style="width: 15%;">QUANT. DO VOLUME</td>
                                        <td>DESCRIÇÃO DA CARGA A SER COLETADA<br />ESPÉCIE DO VOLUME OU MERCADORIA</td>
                                        <td style="width: 25%;">Nº E DATA DOC. FISCAL</td>
                                    </tr>
                                    <tr class="espaco">
                                        <td><%# Eval("Pedido") %></td>
                                        <td><%# Eval("QuanPallet") %></td>
                                        <td><%# Eval("Controle") %></td>
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
                                        <td><%# Eval("Motorista") %></td>
                                        <td><%# Eval("Placa") %></td>
                                        <td>&nbsp;</td>
                                        <td><%# Eval("Contato") %></td>
                                        <td><%# Eval("Controle") %></td>
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
                            </div>
                            <div class="form-divider"></div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </form>
</body>
</html>



