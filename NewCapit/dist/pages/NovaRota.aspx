<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="NovaRota.aspx.cs" Inherits="NewCapit.dist.pages.NovaRota" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    <style>
        #map {
            height: 500px;
            width: 100%;
        }

        .info-rota {
            margin-top: 10px;
            font-weight: bold;
        }
    </style>

    <!-- Google Maps -->
    <script src="https://maps.googleapis.com/maps/api/js?key=AIzaSyApI6da0E4OJktNZ-zZHgL6A5jtk0L6Cww"></script>

    <div class="content-wrapper">
        <!-- Main content -->
        <section class="content">

            <div class="container-fluid">
                <br />
                <div id="divMsg" runat="server"
                    class="alert alert-warning alert-dismissible fade show mt-3"
                    role="alert" style="display: none;">
                    <span id="lblMsgGeral" runat="server"></span>
                    <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
                </div>
                <div class="card card-info">
                    <div class="card-header">
                        <h3 class="card-title"><i class="fas fa-map-marker-alt"></i>&nbsp;ROTAS - NOVO CADASTRO</h3>
                    </div>
                </div>
                <div class="card-body">

                   <%-- <asp:ScriptManager runat="server" EnablePageMethods="true" />--%>

                    <h4>Origem</h4>
                    <asp:DropDownList ID="ddlUfOrigem" runat="server"
                        AutoPostBack="true"
                        OnSelectedIndexChanged="ddlUfOrigem_SelectedIndexChanged" />

                    <asp:DropDownList ID="ddlCidadeOrigem" runat="server" />

                    <hr />

                    <h4>Destino</h4>
                    <asp:DropDownList ID="ddlUfDestino" runat="server"
                        AutoPostBack="true"
                        OnSelectedIndexChanged="ddlUfDestino_SelectedIndexChanged" />

                    <asp:DropDownList ID="ddlCidadeDestino" runat="server" />

                    <br />
                    <br />

                    <asp:Button ID="btnCalcular" runat="server"
                        Text="Mostrar Rota"
                        OnClientClick="calcularRota(); return false;" />

                    <asp:Button ID="btnSalvar" runat="server"
                        Text="Salvar no Banco"
                        OnClientClick="salvarNoBanco(); return false;" />

                    <br />
                    <br />

                    Distância: <span id="lblDistancia">-</span><br />
                    Tempo: <span id="lblTempo">-</span>

                    <div id="map" style="height: 500px;"></div>







                    <div class="row g-3">
                        <div class="col-md-2">
                            <div class="form-group">
                                <span class="details">CADASTRADO EM:</span>
                                <asp:Label ID="lblDtCadastro" runat="server" CssClass="form-control" readonly="true"></asp:Label>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <span class="details">POR:</span>
                                <asp:TextBox ID="txtUsuCadastro" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-md-2">
                            <div class="form-group">
                                <span class="details">ATUALIZADO EM:</span>
                                <asp:Label ID="lbDtAtualizacao" runat="server" CssClass="form-control" placeholder="" readonly="true"></asp:Label>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <span class="details">POR:</span>
                                <asp:TextBox ID="txtAltCad" runat="server" CssClass="form-control" placeholder="" ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="row g-3">
                        <div class="col-md-2">
                            <asp:Button ID="btnCadastrar" runat="server" CssClass="btn btn-outline-success btn-lg w-100" Text="Cadastrar" />
                        </div>
                        <div class="col-md-2">
                            <a href="ConsultaRotas.aspx" class="btn btn-outline-danger btn-lg w-100">Fechar      
                            </a>
                        </div>
                    </div>
                </div>
            </div>
    </section>
    </div>

</asp:Content>
