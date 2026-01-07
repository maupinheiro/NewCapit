<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="Frm_Alt_TabelaPrecoMatriz.aspx.cs" Inherits="NewCapit.dist.pages.Frm_Alt_TabelaPrecoMatriz" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>


  

    <!-- Bibliotecas necessárias -->
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery.mask/1.14.16/jquery.mask.min.js"></script>

    <script>
        function mascaraMoeda(campo) {
            let valor = campo.value.replace(/\D/g, "");
            valor = (valor / 100).toFixed(2) + "";
            valor = valor.replace(".", ",");
            valor = valor.replace(/\B(?=(\d{3})+(?!\d))/g, ".");
            campo.value = valor;
        }

        function mascaraPercentual(campo) {
            let valor = campo.value.replace(/\D/g, "");
            if (valor.length > 3) valor = valor.substring(0, 3); // limite 100%
            campo.value = valor;
        }

        function moedaParaNumero(valor) {
            if (!valor) return 0;
            return parseFloat(valor.replace(/\./g, "").replace(",", "."));
        }

        function calcularFrete() {
            const freteCampo = document.getElementById("<%= txtFreteAgregado.ClientID %>");
            const percentualCampo = document.getElementById("<%= txtPercentualAluguelCarreta.ClientID %>");
            const totalCampo = document.getElementById("<%= txtFreteAgregadoComDesconto.ClientID %>");
            const freteTNGCampo = document.getElementById("<%= txtFreteTNG.ClientID %>");
            const percTNGCampo = document.getElementById("<%= txtPercTNGAgregado.ClientID %>");

            const frete = moedaParaNumero(freteCampo.value);
            const perc = parseFloat(percentualCampo.value) || 0;
            const freteTNG = moedaParaNumero(freteTNGCampo.value);

            // 1️⃣ Calcula o frete com desconto
            const total = frete - (frete * (perc / 100));
            totalCampo.value = total.toLocaleString("pt-BR", { minimumFractionDigits: 2 });

            // 2️⃣ Calcula o percentual TNG
            if (freteTNG > 0) {
                const percTNG = 100 - ((frete / freteTNG) * 100);
                percTNGCampo.value = percTNG.toFixed(2).replace(".", ",");
            } else {
                percTNGCampo.value = "";
            }
        }
        function calcularFreteEspecial() {
            const freteCampoEspecial = document.getElementById("<%= txtFreteEspecial.ClientID %>");
            const percentualCampoEspecial = document.getElementById("<%= txtAluguelCarretaEspecial.ClientID %>");
            const totalCampoEspecial = document.getElementById("<%= txtFreteEspecialComDesconto.ClientID %>");
            const freteTNGCampo = document.getElementById("<%= txtFreteTNG.ClientID %>");
            const percTNGCampoEspecial = document.getElementById("<%= txtPercTNGEspecial.ClientID %>");

            const freteEspecial = moedaParaNumero(freteCampoEspecial.value);
            const percEspecial = parseFloat(percentualCampoEspecial.value) || 0;
            const freteTNG = moedaParaNumero(freteTNGCampo.value);

            // 1️⃣ Calcula o frete com desconto
            const total = freteEspecial - (freteEspecial * (percEspecial / 100));
            totalCampoEspecial.value = total.toLocaleString("pt-BR", { minimumFractionDigits: 2 });

            // 2️⃣ Calcula o percentual TNG
            if (freteTNG > 0) {
                const percTNGEspecial = 100 - ((freteEspecial / freteTNG) * 100);
                percTNGCampoEspecial.value = percTNGEspecial.toFixed(2).replace(".", ",");
            } else {
                percTNGCampoEspecial.value = "";
            }
        }
        function calcularFreteTerceiro() {
            const freteCampoTerceiro = document.getElementById("<%= txtFreteTerceiro.ClientID %>");
            const freteTNGCampo = document.getElementById("<%= txtFreteTNG.ClientID %>");
            const percTNGCampoTerceiro = document.getElementById("<%= txtPercTngTerceiro.ClientID %>");

            const freteTerceiro = moedaParaNumero(freteCampoTerceiro.value);
            const freteTNG = moedaParaNumero(freteTNGCampo.value);


            // 2️⃣ Calcula o percentual TNG
            if (freteTNG > 0) {
                const percTNGT = 100 - ((freteTerceiro / freteTNG) * 100);
                percTNGCampoTerceiro.value = percTNGT.toFixed(2).replace(".", ",");
            } else {
                percTNGCampo.value = "";
            }
        }
        document.addEventListener("DOMContentLoaded", calcularFrete);
        document.addEventListener("DOMContentLoaded", calcularFreteTerceiro);
        document.addEventListener("DOMContentLoaded", alcularFreteEspecial);
    </script>




    <div class="content-wrapper">
        <!-- Main content -->
        <section class="content">
            <div class="container-fluid">
                <br />
                <div id="toastContainer" class="alert alert-warning alert-dismissible" style="display: none;">
                    <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                    <h5><i class="icon fas fa-exclamation-triangle"></i>Alerta!</h5>
                    Alertas
                </div>

                <div class="col-xl-12 col-md-12 mb-12">
                    <div class="card card-info">
                        <div class="card-header">
                            <h3 class="card-title">
                                <h3 class="card-title"><i class="fas fa-donate"></i>&nbsp;GESTÃO DE FRETES - NOVA TABELA DE FRETE NÚMERO:&nbsp;<asp:Label ID="novaTabelaDeFrete" runat="server"></asp:Label></h3>
                            </h3>
                            <div class="card-tools">
                                <button type="button" class="btn btn-tool" data-card-widget="maximize">
                                    <i class="fas fa-expand"></i>
                                </button>
                                <button type="button" class="btn btn-tool" data-card-widget="collapse">
                                    <i class="fas fa-minus"></i>
                                </button>
                                <button type="button" class="btn btn-tool" data-card-widget="remove">
                                    <i class="fas fa-times"></i>
                                </button>
                            </div>
                            <!-- /.card-tools -->
                        </div>
                        <!-- /.card-header -->
                        <div class="card-body">
                            <div class="col-xl-12 col-md-12 mb-12">
                                <div class="info-box">
                                    <span class="info-box-icon bg-info"></span>
                                    <div class="info-box-content">
                                        <span class="info-box-number">
                                            <div class="row g-3">
                                                <div class="col-md-3">
                                                    <div class="form-group">
                                                        <span class="details">FILIAL:</span>
                                                        <asp:DropDownList ID="cboFilial" runat="server" CssClass="form-control select2"></asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="col-md-2">
                                                    <div class="form-group">
                                                        <span class="details">SITUAÇÃO:</span>
                                                        <asp:TextBox ID="txtStatusRota" runat="server" CssClass="form-control" Style="text-align: center" ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-2">
                                                    <div class="form-group">
                                                        <span class="details">CADASTRO:</span>
                                                        <asp:TextBox ID="txtCadastro" runat="server" CssClass="form-control" Style="text-align: center" ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-xl-12 col-md-12 mb-12">
                                <div class="card card-outline card-info">
                                    <div class="card-header">
                                        <h3 class="card-title"><i class="far fa-edit"></i>&nbsp;Dados do Cliente</h3>
                                        <div class="card-tools">
                                            <button type="button" class="btn btn-tool" data-card-widget="collapse">
                                                <i class="fas fa-minus"></i>
                                            </button>
                                        </div>
                                        <!-- /.card-tools -->
                                    </div>
                                    <!-- /.card-header -->
                                    <div class="card-body">
                                        <div class="form-group row">
                                            <label for="inputFilial" class="col-sm-1 col-form-label" style="text-align: right">ROTA:</label>
                                            <div class="col-sm-1">
                                                <asp:TextBox ID="txtRota" runat="server" CssClass="form-control" Style="text-align: center" OnTextChanged="txtRota_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            </div>
                                            <div class="col-md-10">
                                                <asp:DropDownList ID="cboRotas" runat="server" CssClass="form-control select2" AutoPostBack="true" OnSelectedIndexChanged="cboRotas_SelectedIndexChanged"></asp:DropDownList>
                                            </div>
                                        </div>
                                        <!-- REMETENTE -->
                                        <div class="form-group row">
                                            <label for="inputRemetente" class="col-sm-1 col-form-label" style="text-align: right">REMETENTE:</label>
                                            <div class="col-md-1">
                                                <asp:TextBox ID="txtCodRemetente" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                            </div>
                                            <div class="col-md-5">
                                                <asp:TextBox ID="cboRemetente" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="txtMunicipioRemetente" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                            </div>
                                            <div class="col-md-1">
                                                <asp:TextBox ID="txtUFRemetente" Style="text-align: center" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                            </div>

                                        </div>
                                        <!-- EXPEDIDOR -->
                                        <div class="form-group row">
                                            <label for="inputExpedidor" class="col-sm-1 col-form-label" style="text-align: right">EXPEDIDOR:</label>
                                            <div class="col-md-1">
                                                <asp:TextBox ID="txtCodExpedidor" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                            </div>
                                            <div class="col-md-5">
                                                <asp:TextBox ID="cboExpedidor" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="txtCidExpedidor" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                            </div>
                                            <div class="col-md-1">
                                                <asp:TextBox ID="txtUFExpedidor" Style="text-align: center" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                        <!-- DESTINATARIO -->
                                        <div class="form-group row">
                                            <label for="inputDestinatario" class="col-sm-1 col-form-label" style="text-align: right">DEST.:</label>
                                            <div class="col-md-1">
                                                <asp:TextBox ID="txtCodDestinatario" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                            </div>
                                            <div class="col-md-5">
                                                <asp:TextBox ID="cboDestinatario" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="txtMunicipioDestinatario" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                            </div>
                                            <div class="col-md-1">
                                                <asp:TextBox ID="txtUFDestinatario" Style="text-align: center" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                        <!-- RECEBEDOR -->
                                        <div class="form-group row">
                                            <label for="inputRecebedor" class="col-sm-1 col-form-label" style="text-align: right">RECEBEDOR:</label>
                                            <div class="col-md-1">
                                                <asp:TextBox ID="txtCodRecebedor" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                            </div>
                                            <div class="col-md-5">
                                                <asp:TextBox ID="cboRecebedor" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="txtCidRecebedor" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                            </div>
                                            <div class="col-md-1">
                                                <asp:TextBox ID="txtUFRecebedor" Style="text-align: center" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                        <!-- CONSIGNATARIO -->
                                        <div class="form-group row">
                                            <label for="inputConsignatario" class="col-sm-1 col-form-label" style="text-align: right">CONSIG.:</label>
                                            <div class="col-md-1">
                                                <asp:TextBox ID="txtCodConsignatario" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtCodConsignatario_TextChanged"></asp:TextBox>
                                            </div>
                                            <div class="col-md-5">
                                                <asp:DropDownList ID="cboConsignatario" runat="server" CssClass="form-control select2" AutoPostBack="true" OnSelectedIndexChanged="cboConsignatario_SelectedIndexChanged"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="txtCidConsignatario" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                            <div class="col-md-1">
                                                <asp:TextBox ID="txtUFConsignatario" Style="text-align: center" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                        <!-- PAGADOR -->
                                        <div class="form-group row">
                                            <label for="inputPagador" class="col-sm-1 col-form-label" style="text-align: right">PAGADOR:</label>
                                            <div class="col-md-1">
                                                <asp:TextBox ID="txtCodPagador" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtCodPagador_TextChanged"></asp:TextBox>
                                            </div>
                                            <div class="col-md-5">
                                                <asp:DropDownList ID="cboPagador" runat="server" CssClass="form-control select2" AutoPostBack="true" OnSelectedIndexChanged="cboPagador_SelectedIndexChanged"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="txtCidPagador" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                            <div class="col-md-1">
                                                <asp:TextBox ID="txtUFPagador" Style="text-align: center" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>

                                    </div>
                                </div>
                            </div>
                            <!-- Frete -->
                            <div class="col-xl-12 col-md-12 mb-12">
                                <div class="card card-outline card-info">
                                    <div class="card-header">
                                        <h3 class="card-title"><i class="far fa-edit"></i>&nbsp;Frete</h3>
                                        <div class="card-tools">
                                            <button type="button" class="btn btn-tool" data-card-widget="collapse">
                                                <i class="fas fa-minus"></i>
                                            </button>
                                        </div>
                                        <!-- /.card-tools -->
                                    </div>
                                    <div class="card-body">
                                        <div class="row g-3">
                                            <div class="col-md-1">
                                                <div class="form-group">
                                                    <span class="details">DIST.(KM):</span>
                                                    <asp:TextBox ID="txtDistancia" class="form-control" runat="server" ReadOnly="true" Style="text-align: center"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-1">
                                                <div class="form-group">
                                                    <span class="details">DURAÇÃO:</span>
                                                    <asp:TextBox ID="txtDuracao" class="form-control" runat="server" placeholder="00:00" ReadOnly="true" Style="text-align: center"></asp:TextBox>
                                                </div>
                                            </div>

                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <span class="details">PERCURSO:</span>
                                                    <asp:TextBox ID="cboDeslocamento" class="form-control" runat="server" ReadOnly="true" Style="text-align: center"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <span class="details">TIPO DE VIAGEM:</span>
                                                    <asp:DropDownList ID="cboTipoViagem" runat="server" CssClass="form-control select2">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <span class="details">TIPO DE VEÍCULO:</span>
                                                    <asp:DropDownList ID="cboTipoVeiculo" runat="server" CssClass="form-control select2">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <!-- Dados do Frete -->
                                        <div class="card card-outline card-info collapsed-card">
                                            <div class="card-header">
                                                <h3 class="card-title"><i class="far fa-edit"></i>&nbsp;Dados do Frete</h3>
                                                <div class="card-tools">
                                                    <button type="button" class="btn btn-tool" data-card-widget="collapse">
                                                        <i class="fas fa-plus"></i>
                                                    </button>
                                                </div>
                                                <!-- /.card-tools -->
                                            </div>
                                            <!-- /.card-header -->
                                            <div class="card-body">
                                                <div class="row g-3">
                                                    <div class="col-md-3">
                                                        <div class="form_group">
                                                            <span class="details">LOTAÇÃO MÍNIMA:</span>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row g-3">
                                                    <div class="col-sm-1">
                                                        <div class="form-group">
                                                            <div class="custom-control custom-radio">
                                                                <input class="custom-control-input custom-control-input-info custom-control-input-outline" type="radio" id="customRadioAgregado" name="customRadioTipo">
                                                                <label for="customRadioAgregado" class="custom-control-label">SIM</label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-1">
                                                        <div class="form-group">
                                                            <div class="custom-control custom-radio">
                                                                <input class="custom-control-input custom-control-input-info custom-control-input-outline" type="radio" id="customRadioFrota" name="customRadioTipo">
                                                                <label for="customRadioFrota" class="custom-control-label">NÃO</label>
                                                            </div>
                                                        </div>
                                                    </div>                                                  

                                                    <label for="inputFilial" class="col-sm-1 col-form-label" style="text-align: right">MATERIAL:</label>
                                                    <div class="col-md-4">
                                                        <asp:DropDownList ID="cboTipoMaterial" runat="server" CssClass="form-control select2">
                                                        </asp:DropDownList>
                                                    </div>

                                                    <div class="col-sm-2">
                                                        <div class="form-group">
                                                            <!-- Button trigger modal -->
                                                            <button type="button" class="btn btn-success" data-bs-toggle="modal" data-bs-target="#modalGeraLotacao">
                                                                Tabela Lotação
                                                            </button>
                                                        </div>
                                                    </div>




                                                </div>
                                                <div class="row g-3">
                                                    <div class="col-md-2">
                                                        <div class="form-group">
                                                            <span class="details">VIGÊNCIA INICIAL:</span>
                                                            <asp:TextBox ID="txtVigenciaInicial" TextMode="Date" runat="server" Style="text-align: center" CssClass="form-control" MaxLength="10"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <div class="form-group">
                                                            <span class="details">VICÊNCIA FINAL:</span>
                                                            <asp:TextBox ID="txtVigenciaFinal" TextMode="Date" runat="server" Style="text-align: center" CssClass="form-control" MaxLength="10"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <div class="form-group">
                                                            <span class="details">EMITE PEDÁGIO:</span>
                                                            <asp:DropDownList ID="ddlEmitePedagio" runat="server" CssClass="form-control"> 
                                                                <asp:ListItem Value="" Text="SELECIONE"></asp:ListItem>
                                                                <asp:ListItem Value="SIM" Text="SIM"></asp:ListItem>
                                                                <asp:ListItem Value="NÃO" Text="NÃO"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <div class="form-group">
                                                            <span class="details">COBRA HORAS PARADAS:</span>
                                                            <asp:DropDownList ID="ddlHoraParada" runat="server" CssClass="form-control">
                                                                <asp:ListItem Value="" Text="SELECIONE"></asp:ListItem>
                                                                <asp:ListItem Value="SIM" Text="SIM"></asp:ListItem>
                                                                <asp:ListItem Value="NÃO" Text="NÃO"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <div class="form-group">
                                                            <span class="details">FRANQUIA (HH:mm):</span>
                                                            <asp:TextBox ID="txtFranquia" runat="server" CssClass="form-control"> 
                                                            </asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <div class="form-group">
                                                            <span class="details">VALOR (HH:mm):</span>
                                                            <asp:TextBox ID="txtValorFranquia" runat="server" CssClass="form-control" oninput="mascaraMoeda(this);"> 
                                                            </asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                <form class="form-horizontal">
                                                    <div class="card-body">
                                                        <div class="form-group row">
                                                            <label for="inputFilial" class="col-sm-1 col-form-label" style="text-align: right">ADIC NF.(%):</label>
                                                            <div class="col-sm-1">
                                                                <asp:TextBox ID="txtAdicional" runat="server" CssClass="form-control" Style="text-align: center"></asp:TextBox>
                                                            </div>
                                                            <label for="inputFilial" class="col-sm-1 col-form-label" style="text-align: right">SEC-CAT:</label>
                                                            <div class="col-sm-1">
                                                                <asp:TextBox ID="txtSecCat" runat="server" CssClass="form-control" Style="text-align: center" oninput="mascaraMoeda(this);"></asp:TextBox>
                                                            </div>
                                                            <label for="inputFilial" class="col-sm-1 col-form-label" style="text-align: right">DESPACHO:</label>
                                                            <div class="col-sm-1">
                                                                <asp:TextBox ID="txtDespacho" runat="server" CssClass="form-control" Style="text-align: center"></asp:TextBox>
                                                            </div>
                                                            <label for="inputFilial" class="col-sm-1 col-form-label" style="text-align: right">PEDÁGIO:</label>
                                                            <div class="col-sm-1">
                                                                <asp:TextBox ID="txtPedagio" runat="server" CssClass="form-control" Style="text-align: center" oninput="mascaraMoeda(this);"></asp:TextBox>
                                                            </div>
                                                            <label for="inputFilial" class="col-sm-1 col-form-label" style="text-align: right">OUTROS:</label>
                                                            <div class="col-sm-1">
                                                                <asp:TextBox ID="txtOutros" runat="server" CssClass="form-control" Style="text-align: center" oninput="mascaraMoeda(this);"></asp:TextBox>
                                                            </div>
                                                            <label for="inputFilial" class="col-sm-1 col-form-label" style="text-align: right">ADM.(%):</label>
                                                            <div class="col-sm-1">
                                                                <asp:TextBox ID="txtDespAdm" runat="server" CssClass="form-control" Style="text-align: center"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </form>

                                            </div>
                                            <!-- /.card-body -->
                                        </div>
                                        <!-- Frete Transnovag -->
                                        <div class="card card-outline card-info collapsed-card">
                                            <div class="card-header">
                                                <h3 class="card-title"><i class="far fa-edit"></i>&nbsp;Frete Transnovag</h3>
                                                <div class="card-tools">
                                                    <button type="button" class="btn btn-tool" data-card-widget="collapse">
                                                        <i class="fas fa-plus"></i>
                                                    </button>
                                                </div>
                                                <!-- /.card-tools -->
                                            </div>
                                            <!-- /.card-header -->
                                            <div class="card-body">
                                                <div class="form-group row">
                                                    <label for="inputFilial" class="col-sm-2 col-form-label" style="text-align: right">VALOR DO FRETE:</label>
                                                    <div class="col-sm-1">
                                                        <asp:TextBox ID="txtFreteTNG" runat="server" CssClass="form-control"
                                                            oninput="mascaraMoeda(this);"></asp:TextBox>
                                                    </div>
                                                    <label for="inputFilial" class="col-sm-1 col-form-label" style="text-align: right">FRETE FIXO:</label>
                                                    <div class="col-sm-2">
                                                        <asp:DropDownList ID="ddlValorFixoTng" runat="server" CssClass="form-control">
                                                            <asp:ListItem Value="" Text="SELECIONE"></asp:ListItem>
                                                            <asp:ListItem Value="SIM" Text="SIM"></asp:ListItem>
                                                            <asp:ListItem Value="NÃO" Text="NÃO"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <!-- /.card-body -->
                                        </div>
                                        <!-- Frete Agregado -->
                                        <div class="card card-outline card-info collapsed-card">
                                            <div class="card-header">
                                                <h3 class="card-title"><i class="far fa-edit"></i>&nbsp;Frete Agregado</h3>
                                                <div class="card-tools">
                                                    <button type="button" class="btn btn-tool" data-card-widget="collapse">
                                                        <i class="fas fa-plus"></i>
                                                    </button>
                                                </div>
                                            </div>

                                            <div class="card-body">
                                                <div class="form-group row">
                                                    <label class="col-sm-2 col-form-label text-right">VALOR DO FRETE:</label>
                                                    <div class="col-sm-1">
                                                        <asp:TextBox ID="txtFreteAgregado" runat="server" CssClass="form-control"
                                                            Style="text-align: center"
                                                            oninput="mascaraMoeda(this); calcularFrete();" />
                                                    </div>

                                                    <label class="col-sm-1 col-form-label text-right">FRETE FIXO:</label>
                                                    <div class="col-sm-2">
                                                        <asp:DropDownList ID="ddlFixoAgregado" runat="server" CssClass="form-control">
                                                            
                                                            <asp:ListItem Value="SIM" Text="SIM"></asp:ListItem>
                                                            <asp:ListItem Value="NÃO" Text="NÃO"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>

                                                    <label class="col-sm-2 col-form-label text-right">ALUGUEL TNG (%):</label>
                                                    <div class="col-sm-1">
                                                        <asp:TextBox ID="txtPercentualAluguelCarreta" runat="server" CssClass="form-control"
                                                            Style="text-align: center"
                                                            oninput="mascaraPercentual(this); calcularFrete();" />
                                                    </div>

                                                    <label class="col-sm-2 col-form-label text-right">FRETE AGREGADO:</label>
                                                    <div class="col-sm-1">
                                                        <asp:TextBox ID="txtFreteAgregadoComDesconto" runat="server" CssClass="form-control"
                                                            Style="text-align: center" ReadOnly="true" />
                                                    </div>
                                                </div>

                                                <div class="form-group row">
                                                    <label class="col-sm-2 col-form-label text-right">VIGÊNCIA INICIAL:</label>
                                                    <div class="col-sm-2">
                                                        <asp:TextBox ID="txtVigenciaAgregadoInicial" TextMode="Date" runat="server" CssClass="form-control"></asp:TextBox>
                                                    </div>

                                                    <label class="col-sm-2 col-form-label text-right">VIGÊNCIA FINAL:</label>
                                                    <div class="col-sm-2">
                                                        <asp:TextBox ID="txtVigenciaAgregadoFinal" TextMode="Date" runat="server" CssClass="form-control"></asp:TextBox>
                                                    </div>

                                                    <label class="col-sm-1 col-form-label text-right">(%) TNG:</label>
                                                    <div class="col-sm-1">
                                                        <asp:TextBox ID="txtPercTNGAgregado" runat="server" CssClass="form-control"
                                                            Style="text-align: center" ReadOnly="true" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <!-- Frete Terceiro -->
                                        <div class="card card-outline card-info collapsed-card">
                                            <div class="card-header">
                                                <h3 class="card-title"><i class="far fa-edit"></i>&nbsp;Frete Terceiro</h3>
                                                <div class="card-tools">
                                                    <button type="button" class="btn btn-tool" data-card-widget="collapse">
                                                        <i class="fas fa-plus"></i>
                                                    </button>
                                                </div>
                                                <!-- /.card-tools -->
                                            </div>
                                            <!-- /.card-header -->
                                            <div class="card-body">
                                                <div class="form-group row">
                                                    <label for="inputFilial" class="col-sm-2 col-form-label" style="text-align: right">VALOR DO FRETE:</label>
                                                    <div class="col-sm-1">
                                                        <asp:TextBox ID="txtFreteTerceiro" oninput="mascaraMoeda(this); calcularFreteTerceiro();" runat="server" CssClass="form-control" Style="text-align: center"></asp:TextBox>
                                                    </div>
                                                    <label for="inputFilial" class="col-sm-1 col-form-label" style="text-align: right">FRETE FIXO:</label>
                                                    <div class="col-sm-2">
                                                        <asp:DropDownList ID="ddlTerceiro" runat="server" CssClass="form-control">
                                                            
                                                            <asp:ListItem Value="SIM" Text="SIM"></asp:ListItem>
                                                            <asp:ListItem Value="NÃO" Text="NÃO"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                    <label for="inputFilial" class="col-sm-1 col-form-label" style="text-align: right">(%) TNG:</label>
                                                    <div class="col-sm-1">
                                                        <asp:TextBox ID="txtPercTngTerceiro" ReadOnly="true" runat="server" CssClass="form-control" Style="text-align: center"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label for="inputFilial" class="col-sm-2 col-form-label" style="text-align: right">VIGÊNCIA INICIAL:</label>
                                                    <div class="col-sm-2">
                                                        <asp:TextBox ID="txtVigenciaTerceiroInicial" TextMode="Date" runat="server" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                    <label for="inputFilial" class="col-sm-2 col-form-label" style="text-align: right">VIGÊNCIA FINAL:</label>
                                                    <div class="col-sm-2">
                                                        <asp:TextBox ID="txtVigenciaTerceiroFinal" TextMode="Date" runat="server" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <!-- /.card-body -->
                                        </div>
                                        <!-- Frete Especial -->
                                        <div class="card card-outline card-info collapsed-card">
                                            <div class="card-header">
                                                <h3 class="card-title"><i class="far fa-edit"></i>&nbsp;Frete Especial</h3>
                                                <div class="card-tools">
                                                    <button type="button" class="btn btn-tool" data-card-widget="collapse">
                                                        <i class="fas fa-plus"></i>
                                                    </button>
                                                </div>
                                                <!-- /.card-tools -->
                                            </div>
                                            <!-- /.card-header -->
                                            <div class="card-body">
                                                <div class="form-group row">
                                                    <label for="inputFilial" class="col-sm-3 col-form-label" style="text-align: right">AGREGADO/TERCEIRO:</label>
                                                    <div class="col-sm-1">
                                                        <asp:TextBox ID="txtCodAgregado" runat="server" AutoPostBack="true" OnTextChanged="txtCodAgregado_TextChanged" CssClass="form-control" Style="text-align: center"></asp:TextBox>
                                                    </div>
                                                    <div class="col-sm-6">
                                                        <asp:DropDownList ID="cboNomAgregado" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cboNomAgregado_SelectedIndexChanged" CssClass="form-control select2"></asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label for="inputFilial" class="col-sm-3 col-form-label" style="text-align: right">AGREGADO/TRANPORTADORA:</label>
                                                    <div class="col-sm-1">
                                                        <asp:TextBox ID="txtCodTra" runat="server" CssClass="form-control" Style="text-align: center"></asp:TextBox>
                                                    </div>
                                                    <div class="col-sm-6">
                                                        <asp:TextBox ID="txtTransp" runat="server" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label for="inputFilial" class="col-sm-2 col-form-label" style="text-align: right">VALOR DO FRETE:</label>
                                                    <div class="col-sm-1">
                                                        <asp:TextBox ID="txtFreteEspecial" runat="server" CssClass="form-control" oninput="mascaraMoeda(this); calcularFreteEspecial();" Style="text-align: center"></asp:TextBox>
                                                    </div>
                                                    <label for="inputFilial" class="col-sm-1 col-form-label" style="text-align: right">FRETE FIXO:</label>
                                                    <div class="col-sm-1">
                                                        <asp:DropDownList ID="ddlEspecial" runat="server" CssClass="form-control">
                                                            
                                                            <asp:ListItem Value="SIM" Text="SIM"></asp:ListItem>
                                                            <asp:ListItem Value="NÃO" Text="NÃO"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                    <label for="inputFilial" class="col-sm-2 col-form-label" style="text-align: right">ALUGUEL TNG (%):</label>
                                                    <div class="col-sm-1">
                                                        <asp:TextBox ID="txtAluguelCarretaEspecial" oninput="mascaraPercentual(this); calcularFreteEspecial();" runat="server" CssClass="form-control" Style="text-align: center"></asp:TextBox>
                                                    </div>


                                                </div>
                                                <div class="form-group row">
                                                    <label for="inputFilial" class="col-sm-2 col-form-label" style="text-align: right">(%) TNG:</label>
                                                    <div class="col-sm-2">
                                                        <asp:TextBox ID="txtPercTNGEspecial" runat="server" ReadOnly="true" CssClass="form-control" Style="text-align: center"></asp:TextBox>
                                                    </div>
                                                    <label class="col-sm-2 col-form-label text-right">FRETE ESPECIAL:</label>
                                                    <div class="col-sm-2">
                                                        <asp:TextBox ID="txtFreteEspecialComDesconto" runat="server" CssClass="form-control"
                                                            Style="text-align: center" ReadOnly="true" />
                                                    </div>
                                                </div>
                                            </div>
                                            <!-- /.card-body -->
                                        </div>
                                        <!-- Mensagem no Documento -->
                                        <div class="card card-outline card-info collapsed-card">
                                            <div class="card-header">
                                                <h3 class="card-title"><i class="far fa-edit"></i>&nbsp;Mensagem no Documento de Transporte</h3>
                                                <div class="card-tools">
                                                    <button type="button" class="btn btn-tool" data-card-widget="collapse">
                                                        <i class="fas fa-plus"></i>
                                                    </button>
                                                </div>
                                                <!-- /.card-tools -->
                                            </div>
                                            <!-- /.card-header -->
                                            <div class="card-body">
                                                <div class="row g-3">
                                                    <div class="col-md-12">
                                                        <div class="form-group">
                                                            <label for="inputMensagem" class="col-sm-2 col-form-label">MENSAGEM NO CTE:</label>
                                                            <asp:TextBox ID="txtObservacao" class="form-control" runat="server" placeholder="Digite a mensagem que aparecerá no documento de transporte..."></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <!-- /.card-body -->
                                        </div>
                                        <div class="col-xl-12 col-md-12 mb-12">
                                            <div class="row g-3">
                                                <div class="col-sm-2">
                                                    <div class="form-group">
                                                        <!-- Button trigger modal -->
                                                        <button type="button" class="btn btn-outline-primary" data-bs-toggle="modal" data-bs-target="#modalGeraLotacao">
                                                            Cadastrar Rota
                                                        </button>
                                                    </div>
                                                </div>
                                                <div class="col-sm-2">
                                                    <div class="form-group">
                                                        <!-- Button trigger modal -->
                                                        <button type="button" class="btn btn-outline-primary" data-bs-toggle="modal" data-bs-target="#modalGeraLotacao">
                                                            Tipo de Viagem
                                                        </button>
                                                    </div>
                                                </div>

                                                <div class="col-sm-2">
                                                    <div class="form-group">
                                                        <!-- Button trigger modal -->
                                                        <button type="button" class="btn btn-outline-primary" data-bs-toggle="modal" data-bs-target="#modalGeraLotacao">
                                                            Tipo de Veículo
                                                        </button>
                                                    </div>
                                                </div>
                                                <div class="col-sm-1">
                                                    <div class="form-group">
                                                        <!-- Button trigger modal -->
                                                        <button type="button" class="btn btn-outline-primary" data-bs-toggle="modal" data-bs-target="#modalGeraLotacao">
                                                            Material
                                                        </button>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-xl-12 col-md-12 mb-12">
                <div class="info-box">
                    <span class="info-box-icon bg-info">
                        <%--tamanho da foto 39x39 60--%>
                        <%--<img src="<%=fotoMotorista%>" class="rounded-circle float-center" width="60px" alt="" />--%>
                    </span>
                    <div class="info-box-content">
                        <span class="info-box-number">
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
                                <div class="col-md-1">
                                    <asp:Button ID="btnAlterar" runat="server" CssClass="btn btn-outline-success btn-lg" OnClick="btnAlterar_Click"
                                        Text="Atualizar" />
                                </div>
                                <div class="col-md-1">
                                    <a href="ConsultaFretes.aspx" class="btn btn-outline-danger btn-lg">Fechar             
                                    </a>
                                </div>
                            </div>
                    </div>
                </div>
            </div>
        </section>
        <!-- Modal Gerar Lotação -->
        <div class="modal fade" id="modalGeraLotacao" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered">
                <div class="modal-content">
                    <div class="modal-header">
                        <h1 class="modal-title fs-5" id="exampleModalLabel">Tabela de Lotação</h1>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        ...
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Fechar</button>
                        <button type="button" class="btn btn-primary">Salvar</button>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>

