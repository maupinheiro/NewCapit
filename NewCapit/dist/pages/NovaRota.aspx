<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="NovaRota.aspx.cs" Inherits="NewCapit.dist.pages.NovaRota" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    <link href="https://cdn.jsdelivr.net/npm/select2@4.1.0/dist/css/select2.min.css" rel="stylesheet" />
    <script src="https://code.jquery.com/jquery-3.7.1.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/select2@4.1.0/dist/js/select2.min.js"></script>
   <style>
    .select2-container .select2-selection--single {
        height: 38px;
        padding: 5px;
    }
    </style>


    <!-- Google Maps -->
    <script src="https://maps.googleapis.com/maps/api/js?key=AIzaSyApI6da0E4OJktNZ-zZHgL6A5jtk0L6Cww"></script>

    <script>
       function initSelect2() {
           $('.select2').select2({
               theme: 'bootstrap-5',
               width: '100%',
               placeholder: 'Selecione...',
               allowClear: true
           });
       }

       Sys.WebForms.PageRequestManager.getInstance()
           .add_endRequest(initSelect2);

       $(document).ready(initSelect2);
    </script>
    <script>
    <%--function calcularDistanciaBanco() {

        var ufOrigem = $('#<%= ddlUfOrigem.ClientID %>').val();
        var cidadeOrigem = $('#<%= ddlCidadeOrigem.ClientID %>').val();
        var ufDestino = $('#<%= ddlUfDestino.ClientID %>').val();
        var cidadeDestino = $('#<%= ddlCidadeDestino.ClientID %>').val();

        if (!cidadeOrigem || !cidadeDestino) {
            alert("Selecione origem e destino.");
           // MostrarMsg("Selecione origem e destino.", "info");
            return;
        }

        PageMethods.BuscarDistancia(
            ufOrigem, cidadeOrigem, ufDestino, cidadeDestino,
            function (ret) {

                if (!ret.encontrado) {
                    alert("Distância não cadastrada para este trajeto.");
                    //MostrarMsg("Selecione origem e destino.", "info");
                    return;
                }

                $('#lblDistancia').text(ret.distancia + " km");
                $('#lblTempo').text(ret.tempo + " min");
            }
        );
    }--%>
    </script>

    <div class="content-wrapper">
        <!-- Main content -->
        <%--<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />--%>
    <div class="card card-info">
    <div class="card-header">
         <h3 class="card-title"><i class="fas fa-map-marker-alt"></i>&nbsp;ROTAS - NOVO CADASTRO</h3>
    </div>
    </div>  
        <br /><br /><br /><br />
    <div class="container mt-4">  
       <div id="divMsg" runat="server"
             class="alert alert-info alert-dismissible fade show mt-3"
             role="alert" style="display: none;">
             <span id="lblMsgGeral" runat="server"></span>
             <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
       </div>
       <div class="card shadow">
        <div class="card-header bg-primary text-white">
            <h5 class="mb-0">Cadastro de Rota</h5>
        </div>

        <div class="card-body">

            <div class="row">
                <div class="col-md-6">
                    <h6>Origem</h6>

                    <label class="form-label">UF</label>
                    <asp:DropDownList ID="ddlUfOrigem" runat="server"
                        CssClass="form-select select2"
                        AutoPostBack="true"
                        OnSelectedIndexChanged="ddlUfOrigem_SelectedIndexChanged" />

                    <label class="form-label mt-2">Cidade</label>
                    <asp:DropDownList ID="ddlCidadeOrigem" runat="server"
                        CssClass="form-select select2" />
                </div>

                <div class="col-md-6">
                    <h6>Destino</h6>

                    <label class="form-label">UF</label>
                    <asp:DropDownList ID="ddlUfDestino" runat="server"
                        CssClass="form-select select2"
                        AutoPostBack="true"
                        OnSelectedIndexChanged="ddlUfDestino_SelectedIndexChanged" />

                    <label class="form-label mt-2">Cidade</label>
                    <asp:DropDownList ID="ddlCidadeDestino" runat="server"
                        CssClass="form-select select2" />
                </div>
            </div>

            <div class="mt-4 text-center">
                <%--<button class="btn btn-success px-4" onclick="calcularDistanciaBanco()">
                    Calcular Distância
                </button>--%>
                <asp:Button 
                    ID="btnCalcular" 
                    runat="server" 
                    Text="Calcular Distância"
                    CssClass="btn btn-success px-4"
                    OnClick="btnCalcular_Click" />
            </div>

            <div class="row mt-4 text-center">
                <div class="col-md-6">
                    <div class="alert alert-info">
                        <strong>Distância</strong><br />
                        <span id="lblDistancia" runat="server">-</span>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="alert alert-warning">
                        <strong>Tempo</strong><br />
                        <span id="lblTempo" runat="server">-</span>
                    </div>
                </div>
            </div>

        </div>
    </div>
    </div>

        <%--<section class="content">

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
        </section>--%>
    </div>

</asp:Content>
