<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="NumerarPneu.aspx.cs" Inherits="NewCapit.dist.pages.NumerarPneu" %>

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
                    <h5 class="mb-0">Numerar Pneu</h5>
                </div>
                <div class="card-body">
                    <div class="row">
                        <div class="col-md-10">
                            <label>Descrição:</label>
                            <asp:TextBox ID="txtDescricao" runat="server" CssClass="form-control" ReadOnly="true" />
                        </div>
                        <div class="col-md-2">
                            <label>ID:</label>
                            <asp:TextBox ID="txtID" runat="server"
                                CssClass="form-control"
                                ReadOnly="true"
                                Style="background-color: #fff9c4; color: #000;">
                            </asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-2">
                            <label>Compra:</label>
                            <asp:TextBox ID="txtDtCompra" runat="server" CssClass="form-control" ReadOnly="true" />
                        </div>
                        <div class="col-md-2">
                            <label>Valor:</label>
                            <asp:TextBox ID="txtValor" runat="server" CssClass="form-control" ReadOnly="true" />
                        </div>
                        <div class="col-md-8">
                            <label>Responsável:</label>
                            <asp:TextBox ID="txtResp_entrada" runat="server" CssClass="form-control" ReadOnly="true" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-2">
                            <label>Número*:</label>
                            <asp:TextBox ID="txtNumero" runat="server" CssClass="form-control" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <label>Marca:</label>
                            <asp:TextBox ID="txtMarca" runat="server" CssClass="form-control" ReadOnly="true" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-5">
                            <label>Modelo:</label>
                            <asp:TextBox ID="txtModelo" runat="server" CssClass="form-control" ReadOnly="true" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <label>Medida:</label>
                            <asp:TextBox ID="txtMedida" runat="server" CssClass="form-control" placeholder="295/80 R22.5" ReadOnly="true" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-3">
                            <label>Status:</label>
                            <asp:TextBox ID="txtStatus" runat="server" CssClass="form-control" ReadOnly="true" />
                        </div>
                    </div>
                    <br />
                    <div class="row g-3">
                        <div class="col-md-3">
                            <br />
                            <asp:Button ID="btnSalvar" CssClass="btn btn-outline-info w-100" runat="server" Text="Confirmar Numeração" OnClick="btnSalvar_Click" />
                        </div>
                        <div class="col-md-2">
                            <br />
                            <a href="ControlePneus.aspx" class="btn btn-outline-danger w-100">Fechar   
                            </a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
