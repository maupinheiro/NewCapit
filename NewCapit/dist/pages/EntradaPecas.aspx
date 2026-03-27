<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="EntradaPecas.aspx.cs" Inherits="NewCapit.dist.pages.EntradaPecas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- Bootstrap CSS -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/css/bootstrap.min.css" rel="stylesheet" />
    <!-- jQuery e Bootstrap JS -->
    <script src="https://code.jquery.com/jquery-3.6.4.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/js/bootstrap.bundle.min.js"></script>
    <!-- Bootstrap CSS + JS -->
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" />
    <script src="https://code.jquery.com/jquery-3.5.1.slim.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@4.5.2/dist/js/bootstrap.bundle.min.js"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">

    <script>
        function formatarData(input) {
            let v = input.value.replace(/\D/g, '');

            if (v.length > 2) v = v.substring(0, 2) + '/' + v.substring(2);
            if (v.length > 5) v = v.substring(0, 5) + '/' + v.substring(5, 9);

            input.value = v;
        }
    </script>
    <script>
        function formatarMoeda(campo) {
            let valor = campo.value.replace(/\D/g, '');

            valor = (valor / 100).toFixed(2) + '';
            valor = valor.replace(".", ",");
            valor = valor.replace(/\B(?=(\d{3})+(?!\d))/g, ".");

            campo.value = valor;
        }
    </script>
    <div class="content-wrapper">
        <div class="card card-info">
            <div class="card-header" style="background-color: #A020F0; font-weight: bold;">
                <h3 class="card-title"><i class="far fa-calendar-check"></i>&nbsp;Manutenção</h3>
            </div>
        </div>
        <br />
        <br />
        <br />
        <br />
        <div class="container mt-4">
            <div id="divMsg" runat="server"
                class="alert alert-dismissible fade show mt-3"
                role="alert" visible="false">
                <asp:Label ID="lblMsgGeral" runat="server"></asp:Label>
                <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
            </div>
            <div class="card shadow">
                <div class="card-header bg-primary text-white">
                    <h5 class="mb-0">Entrada de Peças</h5>
                </div>
                <div class="card-body">
                    <div class="form-row">
                        <div class="form-group col-md-1">
                            <label>Peça:</label>
                            <asp:TextBox ID="txtIdPeca" runat="server" CssClass="form-control" ReadOnly="true" />
                        </div>
                        <div class="form-group col-md-7">
                            <label>Descrição:</label>
                            <asp:TextBox ID="txtPeca" runat="server" CssClass="form-control" ReadOnly="true" />
                        </div>
                        <div class="form-group col-md-2">
                            <label>Est. Atual:</label>
                            <asp:TextBox ID="txtEstoqueAtual" runat="server" CssClass="form-control" ReadOnly="true" />
                        </div>
                        <div class="form-group col-md-2">
                            <label>Unidade:</label>
                            <asp:TextBox ID="txtUnidade" runat="server" CssClass="form-control" ReadOnly="true" />
                        </div>

                    </div>
                    <div class="form-row">
                        <div class="form-group col-md-7">
                            <label>Chave de Acesso NF:</label>
                            <asp:TextBox ID="txtChave" runat="server" CssClass="form-control" MaxLength="44" placeholder="Digite ou Scannei a chave de acesso da nota" Style="text-align: center" AutoPostBack="true" OnTextChanged="txtChave_TextChanged" />
                        </div>
                        <div class="form-group col-md-2">
                            <label>Nota Fiscal:</label>
                            <asp:TextBox ID="txtNF" runat="server" CssClass="form-control" ReadOnly="true" Style="text-align: center" />
                        </div>
                        <div class="form-group col-md-1">
                            <label>Serie:</label>
                            <asp:TextBox ID="txtSerieNF" runat="server" CssClass="form-control" ReadOnly="true" Style="text-align: center" />
                        </div>
                        <div class="form-group col-md-2">
                            <label>Emissão:</label>
                            <asp:TextBox ID="txtEmissaoNF" runat="server" CssClass="form-control" Style="text-align: center" MaxLength="10" onkeyup="formatarData(this)" />
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-group col-md-1">
                            <label>Fornec.:</label>
                            <asp:TextBox ID="txtCodFor" runat="server" CssClass="form-control" ReadOnly="true" />
                        </div>
                        <div class="form-group col-md-7">
                            <label>Razão Social:</label>
                            <asp:TextBox ID="txtRazSocial" runat="server" CssClass="form-control" ReadOnly="true" />
                        </div>
                        <div class="form-group col-md-4">
                            <label>CNPJ:</label>
                            <asp:TextBox ID="txtCNPJ" runat="server" CssClass="form-control" ReadOnly="true" />
                        </div>

                    </div>
                    <div class="form-row">
                        <div class="form-group col-md-3">
                            <label>Tipo</label>
                            <asp:DropDownList ID="ddlTipoEntrada" runat="server" CssClass="form-control">
                                <asp:ListItem Text="Compra" />
                                <asp:ListItem Text="Devolução" />
                                <asp:ListItem Text="Ajuste" />
                            </asp:DropDownList>
                        </div>
                        <div class="form-group col-md-2">
                            <label>Quantidade:</label>
                            <asp:TextBox ID="txtQuantidade" TextMode="Number" runat="server" CssClass="form-control" />
                        </div>
                        <div class="form-group col-md-2">
                            <label>Valor Unitário:</label>
                            <asp:TextBox ID="txtValor" runat="server" CssClass="form-control" onkeyup="formatarMoeda(this)" />
                        </div>
                        <div class="form-group col-md-2" id="divTipo" runat="server" visible="false">
                            <label>Tipo:</label>
                            <asp:TextBox ID="txtTipoPeca" runat="server" CssClass="form-control"/>
                        </div>
                    </div>
                    <div class="form-row" id="divPneu" runat="server" visible="false">
                        <div class="col-md-4">
                            <label>Marca:</label>
                            <asp:DropDownList ID="ddlMarca" runat="server" CssClass="form-control" />
                        </div>

                        <div class="col-md-4">
                            <label>Modelo:</label>
                            <asp:TextBox ID="txtModelo" runat="server" CssClass="form-control" />
                        </div>

                        <div class="col-md-4">
                            <label>Medida:</label>
                            <asp:TextBox ID="txtMedida" runat="server" CssClass="form-control" placeholder="295/80 R22.5" />
                        </div>
                    </div>
                    <br />
                    <div class="row g-3">
                        <div class="col-md-3">
                            <br />
                            <asp:Button ID="btnConfirmar" CssClass="btn btn-outline-info w-100" runat="server" Text="Confirmar Entrada" OnClick="btnConfirmar_Click" />
                        </div>

                        <div class="col-md-2">
                            <br />
                            <a href="controlaestoque.aspx" class="btn btn-outline-danger w-100">Fechar   
                            </a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
