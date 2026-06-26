<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="Frm_TabelaPrecoMatriz.aspx.cs" Inherits="NewCapit.dist.pages.Frm_TabelaPrecoMatriz" %>

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

        function formatar4Casas(campo) {
            var valor = campo.value.replace(/\./g, '').replace(',', '.');

            if (!isNaN(valor) && valor !== '') {
                campo.value = parseFloat(valor).toLocaleString('pt-BR', {
                    minimumFractionDigits: 4,
                    maximumFractionDigits: 4
                });
            }
        }


        function moedaParaNumero(valor) {
            if (!valor) return 0;

            valor = valor.replace(/\./g, '');
            valor = valor.replace(',', '.');

            return parseFloat(valor) || 0;
        }

        function numeroParaMoeda(valor) {
            return valor.toLocaleString('pt-BR', {
                minimumFractionDigits: 2,
                maximumFractionDigits: 2
            });
        }

        function calcularFrete() {
            var ddlFrete = document.getElementById('<%= ddlFrete.ClientID %>').value;

            var txtFreteReceber = document.getElementById('<%= txtFreteReceber.ClientID %>');
            var txtFretePagar = document.getElementById('<%= txtFretePagar.ClientID %>');
            var txtMargem = document.getElementById('<%= txtMargem.ClientID %>');

            var freteReceber = moedaParaNumero(
                document.getElementById('<%= txtFreteReceber.ClientID %>').value
            );

            var margem = moedaParaNumero(
                document.getElementById('<%= txtMargem.ClientID %>').value
            );

            var aluguel = moedaParaNumero(
                document.getElementById('<%= txtPercentualAluguelCarreta.ClientID %>').value
            );

            var fretePagar = 0;

            if (ddlFrete == "FROTA") {

                txtMargem.value = "100,00";

                txtFretePagar.value = txtFreteReceber.value;

                return;
            }
            else if (ddlFrete == "AGREGADO") {

                fretePagar =
                    freteReceber
                    - (freteReceber * margem / 100)
                    - (freteReceber * aluguel / 100);
            }
            else if (ddlFrete == "TERCEIRO") {

                fretePagar =
                    freteReceber
                    - (freteReceber * margem / 100);
            }

            document.getElementById('<%= txtFretePagar.ClientID %>').value =
                numeroParaMoeda(fretePagar);
        }

        function calcularMargem() {

            var freteReceber = moedaParaNumero(
                document.getElementById('<%= txtFreteReceber.ClientID %>').value
            );

            var fretePagar = moedaParaNumero(
                document.getElementById('<%= txtFretePagar.ClientID %>').value
            );

            if (freteReceber <= 0)
                return;

            var margem = ((freteReceber - fretePagar) / freteReceber) * 100;
            If(ddlfrete == "FROTA")
            {
                document.getElementById('<%= txtMargem.ClientID %>').value = "100,00"
            }
            If(ddlfrete == "AGREGADO" || ddlfrete == "TERCEIRO")
            {
                document.getElementById('<%= txtMargem.ClientID %>').value =
                    margem.toFixed(2).replace('.', ',');
            }

        }

    </script>

    <script>
        function verificarCampos() {

            var centro = document.getElementById('<%= txtDistanciaCentro.ClientID %>');
            var cep = document.getElementById('<%= txtDistanciaCEP.ClientID %>');
            var rbCentro = document.getElementById('<%= rbCentro.ClientID %>');
            var rbCEP = document.getElementById('<%= rbCEP.ClientID %>');

            rbCentro.disabled = centro.value.trim() === "";
            rbCEP.disabled = cep.value.trim() === "";
        }
    </script>
    <script>
        function valorCampo(id) {
            var valor = document.getElementById(id).value;

            if (!valor) return 0;

            // Remove pontos de milhar e troca vírgula por ponto
            valor = valor.replace(/\./g, '').replace(',', '.');

            return parseFloat(valor) || 0;
        }

        function calcularTotalFrete() {
            var total =
                valorCampo('<%= txtSecCat.ClientID %>') +
                valorCampo('<%= txtDespacho.ClientID %>') +
                valorCampo('<%= txtOutros.ClientID %>') +
                valorCampo('<%= txtDespAdm.ClientID %>') +
                valorCampo('<%= txtGRIS.ClientID %>') +
                valorCampo('<%= txtColeta.ClientID %>') +
                valorCampo('<%= txtEntrega.ClientID %>') +
                valorCampo('<%= txtTDE.ClientID %>') +
                valorCampo('<%= txtTDA.ClientID %>') +
                valorCampo('<%= txtFreteReceber.ClientID %>');

            document.getElementById('<%= txtTotalFrete.ClientID %>').value =
                total.toLocaleString('pt-BR', {
                    minimumFractionDigits: 2,
                    maximumFractionDigits: 2
                });
        }
    </script>


    <script>
        document.addEventListener("DOMContentLoaded", function () {

            var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
            tooltipTriggerList.map(function (tooltipTriggerEl) {
                return new bootstrap.Tooltip(tooltipTriggerEl);
            });

            var rbCentro = document.getElementById('<%= rbCentro.ClientID %>');
            var rbCEP = document.getElementById('<%= rbCEP.ClientID %>');

            rbCentro.addEventListener("change", function () {
                if (this.checked) {
                    document.getElementById('<%= txtDistancia.ClientID %>').value =
                        document.getElementById('<%= txtDistanciaCentro.ClientID %>').value;
                }
            });

            rbCEP.addEventListener("change", function () {
                if (this.checked) {
                    document.getElementById('<%= txtDistancia.ClientID %>').value =
                        document.getElementById('<%= txtDistanciaCEP.ClientID %>').value;
                }
            });

            function aplicarMascara(input, mascara) {
                input.addEventListener("input", function () {
                    let valor = input.value.replace(/\D/g, ""); // Remove tudo que não for número
                    let resultado = "";
                    let posicao = 0;

                    for (let i = 0; i < mascara.length; i++) {
                        if (mascara[i] === "0") {
                            if (valor[posicao]) {
                                resultado += valor[posicao];
                                posicao++;
                            } else {
                                break;
                            }
                        } else {
                            resultado += mascara[i];
                        }
                    }

                    input.value = resultado;
                });
            }
            // Pegando os elementos no ASP.NET            
            let txtVigenciaInicial = document.getElementById("<%= txtVigenciaInicial.ClientID %>");
            let txtVigenciaFinal = document.getElementById("<%= txtVigenciaFinal.ClientID %>");
            if (txtVigenciaInicial) aplicarMascara(txtVigenciaInicial, "00/00/0000");
            if (txtVigenciaFinal) aplicarMascara(txtVigenciaFinal, "00/00/0000");

        });
    </script>
    <div class="content-wrapper">
        <!-- Main content -->
        <section class="content">
            <div class="container-fluid">
                <br />
                <div id="divMsg" runat="server"
                    class="alert alert-warning alert-dismissible fade show mt-3"
                    role="alert" style="display: none;">
                    <span id="lblMsg" runat="server"></span>
                    <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
                </div>
                <div class="col-xl-12 col-md-12 mb-12">
                    <div class="card card-info">
                        <div class="card-header" style="background-color: #A020F0; font-weight: bold;">
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
                                                <div class="col-md-8 d-flex justify-content-end">
                                                    <asp:HyperLink ID="lnkUrl" runat="server"
                                                        Text=""
                                                        NavigateUrl='<%# Eval("link") %>'
                                                        Target="_blank">
                                                    </asp:HyperLink>
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
                                        <!-- REMETENTE -->
                                        <div class="form-group row">
                                            <label for="inputRemetente" class="col-sm-1 col-form-label" style="text-align: right">REMETENTE:</label>
                                            <div class="col-md-1">
                                                <asp:TextBox ID="txtCodRemetente" runat="server" CssClass="form-control" OnTextChanged="txtCodRemetente_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="cboRemetente" runat="server" CssClass="form-control select2" OnSelectedIndexChanged="cboRemetente_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:TextBox ID="txtCNPJRemetente" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3">
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
                                                <asp:TextBox ID="txtCodExpedidor" runat="server" CssClass="form-control" OnTextChanged="txtCodExpedidor_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="cboExpedidor" runat="server" CssClass="form-control select2" OnSelectedIndexChanged="cboExpedidor_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:TextBox ID="txtCNPJExpedidor" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3">
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
                                                <asp:TextBox ID="txtCodDestinatario" runat="server" CssClass="form-control" OnTextChanged="txtCodDestinatario_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="cboDestinatario" runat="server" CssClass="form-control select2" OnSelectedIndexChanged="cboDestinatario_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:TextBox ID="txtCNPJDestinatario" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3">
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
                                                <asp:TextBox ID="txtCodRecebedor" runat="server" CssClass="form-control" OnTextChanged="txtCodRecebedor_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="cboRecebedor" runat="server" CssClass="form-control select2"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:TextBox ID="txtCNPJRecebedor" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3">
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
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="cboConsignatario" runat="server" CssClass="form-control select2" AutoPostBack="true" OnSelectedIndexChanged="cboConsignatario_SelectedIndexChanged"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:TextBox ID="txtCNPJConsignatario" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3">
                                                <asp:TextBox ID="txtCidConsignatario" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                            </div>
                                            <div class="col-md-1">
                                                <asp:TextBox ID="txtUFConsignatario" Style="text-align: center" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                        <!-- PAGADOR -->
                                        <div class="form-group row">
                                            <label for="inputPagador" class="col-sm-1 col-form-label" style="text-align: right">PAGADOR:</label>
                                            <div class="col-md-1">
                                                <asp:TextBox ID="txtCodPagador" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtCodPagador_TextChanged"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="cboPagador" runat="server" CssClass="form-control select2" AutoPostBack="true" OnSelectedIndexChanged="cboPagador_SelectedIndexChanged"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:TextBox ID="txtCNPJPagador" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3">
                                                <asp:TextBox ID="txtCidPagador" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                            </div>
                                            <div class="col-md-1">
                                                <asp:TextBox ID="txtUFPagador" Style="text-align: center" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="form-group row">
                                            <label for="inputFilial" class="col-sm-1 col-form-label" style="text-align: right">ROTA:</label>
                                            <div class="col-sm-1">
                                                <asp:TextBox ID="txtRota" runat="server" CssClass="form-control" Style="text-align: center" ReadOnly="true"></asp:TextBox>
                                            </div>
                                            <div class="col-md-10">
                                                <asp:TextBox ID="txtDesc_Rota" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                        <br />
                                        <div class="form-group row">
                                            <asp:RadioButton
                                                for="inputFilial"
                                                class="col-sm-5 col-form-label"
                                                Style="text-align: right"
                                                ID="rbCentro"
                                                runat="server"
                                                GroupName="tipoDistancia"
                                                Text="&nbsp;&nbsp;Distância/Tempo CENTRO/CENTRO"
                                                OnClientClick="selecionarCentro();" />

                                            <div class="col-sm-1">
                                                <asp:TextBox ID="txtDistanciaCentro" runat="server" CssClass="form-control" Style="text-align: center" ReadOnly="true" onkeyup="verificarCampos();"></asp:TextBox>
                                            </div>
                                            <div class="col-md-1">
                                                <asp:TextBox ID="txtTempoCentro" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group row">
                                            <asp:RadioButton
                                                for="inputFilial"
                                                class="col-sm-5 col-form-label"
                                                Style="text-align: right"
                                                ID="rbCEP"
                                                runat="server"
                                                GroupName="tipoDistancia"
                                                Text="&nbsp;&nbsp;Distância/Tempo CEP/CEP"
                                                OnClientClick="selecionarCEP();" />

                                            <div class="col-sm-1">
                                                <asp:TextBox ID="txtDistanciaCEP" runat="server" CssClass="form-control" Style="text-align: center" ReadOnly="true"></asp:TextBox>
                                            </div>
                                            <%--  <div class="col-md-1">
                                                 <asp:TextBox ID="txtTempoCEP" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                             </div>--%>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <!-- Frete -->
                            <div class="col-xl-12 col-md-12 mb-12">
                                <div class="card card-outline card-info collapsed-card">
                                    <div class="card-header">
                                        <h3 class="card-title"><i class="far fa-edit"></i>&nbsp;Composição do Frete</h3>
                                        <div class="card-tools">
                                            <button type="button" class="btn btn-tool" data-card-widget="collapse">
                                                <i class="fas fa-plus"></i>
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
                                                    <asp:TextBox ID="txtDeslocamento" class="form-control" runat="server" ReadOnly="true" Style="text-align: center"></asp:TextBox>
                                                </div>
                                            </div>

                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <span class="details">PAGA PEDÁGIO:</span>
                                                    <asp:DropDownList ID="ddlEmitePedagio"
                                                        runat="server"
                                                        CssClass="form-control">
                                                        <asp:ListItem Value="" Text="Selecione..."></asp:ListItem>
                                                        <asp:ListItem Value="SIM" Text="SIM"></asp:ListItem>
                                                        <asp:ListItem Value="NAO" Text="NAO"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <span class="details">PAGA HORAS PARADAS:</span>
                                                    <asp:DropDownList ID="ddlHoraParada" runat="server" CssClass="form-control">
                                                        <asp:ListItem Value="" Text="Selecione..."></asp:ListItem>
                                                        <asp:ListItem Value="SIM" Text="SIM"></asp:ListItem>
                                                        <asp:ListItem Value="NAO" Text="NAO"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <span class="details">FRANQUIA (HH:mm:ss):</span>
                                                    <asp:TextBox ID="txtFranquia" runat="server" CssClass="form-control"> 
                                                    </asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <span class="details">VALOR):</span>
                                                    <asp:TextBox ID="txtValorFranquia" runat="server" CssClass="form-control" oninput="mascaraMoeda(this);"> 
                                                    </asp:TextBox>
                                                </div>
                                            </div>

                                        </div>
                                        <div class="row g-3">
                                            <form class="form-horizontal">
                                                <div class="card-body">
                                                    <div class="form-group row">
                                                        <label for="inputFilial"
                                                            class="col-sm-1 col-form-label"
                                                            style="text-align: right"
                                                            data-bs-toggle="tooltip"
                                                            data-bs-placement="top"
                                                            title="Aliquota sobre valor total da mercadoria.">
                                                            SEGURO(%):
                                                        </label>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtAdicional"
                                                                runat="server"
                                                                CssClass="form-control"
                                                                Style="text-align: center"
                                                                onblur="formatar4Casas(this)">
</asp:TextBox>
                                                        </div>
                                                        <label for="inputFilial" class="col-sm-1 col-form-label" style="text-align: right">SEC-CAT:</label>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtSecCat" runat="server" CssClass="form-control" Style="text-align: center" oninput="mascaraMoeda(this); calcularTotalFrete();"></asp:TextBox>
                                                        </div>
                                                        <label for="inputFilial" class="col-sm-1 col-form-label" style="text-align: right">DESPACHO:</label>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtDespacho" runat="server" CssClass="form-control" Style="text-align: center" oninput="mascaraMoeda(this); calcularTotalFrete();"></asp:TextBox>
                                                        </div>
                                                        <label for="inputFilial"
                                                            class="col-sm-1 col-form-label"
                                                            style="text-align: right"
                                                            data-bs-toggle="tooltip"
                                                            data-bs-placement="top"
                                                            title="Taxa de Aluguel da Carreta.">
                                                            CARRETA(%):
                                                        </label>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtPercentualAluguelCarreta" runat="server" CssClass="form-control" Style="text-align: center" oninput="mascaraMoeda(this);"></asp:TextBox>
                                                        </div>
                                                        <label for="inputFilial" class="col-sm-1 col-form-label" style="text-align: right">OUTROS:</label>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtOutros" runat="server" CssClass="form-control" Style="text-align: center" oninput="mascaraMoeda(this); calcularTotalFrete();"></asp:TextBox>
                                                        </div>
                                                        <label for="inputFilial"
                                                            class="col-sm-1 col-form-label"
                                                            style="text-align: right"
                                                            data-bs-toggle="tooltip"
                                                            data-bs-placement="top"
                                                            title="Taxa Administrativa">
                                                            ADM.:
                                                        </label>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtDespAdm" runat="server" CssClass="form-control" Style="text-align: center" oninput="mascaraMoeda(this); calcularTotalFrete();"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </form>

                                        </div>
                                        <div class="row g-3">
                                            <div class="card-body">
                                                <div class="form-group row">
                                                    <label for="inputFilial"
                                                        class="col-sm-1 col-form-label"
                                                        style="text-align: right"
                                                        data-bs-toggle="tooltip"
                                                        data-bs-placement="top"
                                                        title="Gerenciamento de Risco">
                                                        GRIS:
                                                    </label>
                                                    <div class="col-sm-1">
                                                        <asp:TextBox ID="txtGRIS" runat="server" CssClass="form-control" Style="text-align: center" oninput="mascaraMoeda(this); calcularTotalFrete();"></asp:TextBox>
                                                    </div>
                                                    <label for="inputFilial" class="col-sm-1 col-form-label" style="text-align: right">
                                                        COLETA:
                                                    </label>
                                                    <div class="col-sm-1">
                                                        <asp:TextBox ID="txtColeta" runat="server" CssClass="form-control" Style="text-align: center" oninput="mascaraMoeda(this); calcularTotalFrete();"></asp:TextBox>
                                                    </div>
                                                    <label for="inputFilial" class="col-sm-1 col-form-label" style="text-align: right">
                                                        ENTREGA:
                                                    </label>
                                                    <div class="col-sm-1">
                                                        <asp:TextBox ID="txtEntrega" runat="server" CssClass="form-control" Style="text-align: center" oninput="mascaraMoeda(this); calcularTotalFrete();"></asp:TextBox>
                                                    </div>
                                                    <label for="inputFilial"
                                                        class="col-sm-1 col-form-label"
                                                        style="text-align: right"
                                                        data-bs-toggle="tooltip"
                                                        data-bs-placement="top"
                                                        title="Taxa por Dificuldade de Entrega">
                                                        TDE:
                                                    </label>
                                                    <div class="col-sm-1">
                                                        <asp:TextBox ID="txtTDE" runat="server" CssClass="form-control" Style="text-align: center" oninput="mascaraMoeda(this); calcularTotalFrete();"></asp:TextBox>
                                                    </div>
                                                    <label for="inputFilial"
                                                        class="col-sm-1 col-form-label"
                                                        style="text-align: right"
                                                        data-bs-toggle="tooltip"
                                                        data-bs-placement="top"
                                                        title="Taxa de Difícil Acesso">
                                                        TDA:
                                                    </label>
                                                    <div class="col-sm-1">
                                                        <asp:TextBox ID="txtTDA" runat="server" CssClass="form-control" Style="text-align: center" oninput="mascaraMoeda(this); calcularTotalFrete();"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row g-3">
                                            <form class="form-horizontal">
                                                <div class="card-body">
                                                    <div class="form-group row">
                                                        <label for="inputFilial" class="col-sm-1 col-form-label" style="text-align: right">ICMS(%):</label>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtICMS" runat="server" CssClass="form-control" Style="text-align: center" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                        <label for="inputFilial" class="col-sm-1 col-form-label" style="text-align: right">ISS(%):</label>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtISS" runat="server" CssClass="form-control" Style="text-align: center" oninput="mascaraMoeda(this);" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                        <label for="inputFilial" class="col-sm-1 col-form-label" style="text-align: right">PIS(%):</label>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtPIS" runat="server" CssClass="form-control" Style="text-align: center" oninput="mascaraMoeda(this);" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                        <label for="inputFilial" class="col-sm-1 col-form-label" style="text-align: right">COFINS(%):</label>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtCOFINS" runat="server" CssClass="form-control" Style="text-align: center" oninput="mascaraMoeda(this);" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                        <label for="inputFilial" class="col-sm-1 col-form-label" style="text-align: right">IRPJ(%):</label>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtIRPJ" runat="server" CssClass="form-control" Style="text-align: center" oninput="mascaraMoeda(this);" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                        <label for="inputFilial" class="col-sm-1 col-form-label" style="text-align: right">CSLL(%):</label>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtCSLL" runat="server" CssClass="form-control" Style="text-align: center" oninput="mascaraMoeda(this);" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </form>

                                        </div>
                                        <div class="row g-3">
                                            <div class="card-body">
                                                <div class="form-group row">
                                                    <label for="inputFilial" class="col-sm-1 col-form-label" style="text-align: right">IBS(%):</label>
                                                    <div class="col-sm-1">
                                                        <asp:TextBox ID="txtIBS" runat="server" CssClass="form-control" Style="text-align: center" oninput="mascaraMoeda(this);" ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                    <label for="inputFilial" class="col-sm-1 col-form-label" style="text-align: right">CBS(%):</label>
                                                    <div class="col-sm-1">
                                                        <asp:TextBox ID="txtCBS" runat="server" CssClass="form-control" Style="text-align: center" oninput="mascaraMoeda(this);" ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                    <label for="inputFilial" class="col-sm-2 col-form-label" style="text-align: right">SEST SENAT(%):</label>
                                                    <div class="col-sm-1">
                                                        <asp:TextBox ID="txtSestSenat" runat="server" CssClass="form-control" Style="text-align: center" oninput="mascaraMoeda(this);" ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                    <label for="inputFilial" class="col-sm-1 col-form-label" style="text-align: right">INSS(%):</label>
                                                    <div class="col-sm-1">
                                                        <asp:TextBox ID="txtINSS" runat="server" CssClass="form-control" Style="text-align: center" oninput="mascaraMoeda(this);" ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <!-- Tabela Geral de Fretes -->
                                        <div class="card card-outline card-info collapsed-card">
                                            <div class="card-header">
                                                <h3 class="card-title"><i class="far fa-edit"></i>&nbsp;Tabela Geral de Fretes</h3>
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
                                                        <div class="form-group">
                                                            <span class="details">FRETE:</span>
                                                            <asp:DropDownList ID="ddlFrete" runat="server" CssClass="form-control">
                                                                <asp:ListItem Value="" Text="Selecione..."></asp:ListItem>
                                                                <asp:ListItem Value="FROTA" Text="FROTA"></asp:ListItem>
                                                                <asp:ListItem Value="AGREGADO" Text="AGREGADO"></asp:ListItem>
                                                                <asp:ListItem Value="TERCEIRO" Text="TERCEIRO"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <div class="form-group">
                                                            <span class="details">FRETE POR:</span>
                                                            <asp:DropDownList ID="ddlTipoFrete" runat="server" CssClass="form-control">
                                                                <asp:ListItem Value="" Text="Selecione..."></asp:ListItem>
                                                                <asp:ListItem Value="TONELADA" Text="TONELADA"></asp:ListItem>
                                                                <asp:ListItem Value="FTL" Text="FTL"></asp:ListItem>

                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row g-3">
                                                    <div class="col-md-3">
                                                        <div class="form-group">
                                                            <span class="details">TIPO DE VIAGEM:</span>
                                                            <asp:DropDownList ID="cboTipoViagem" runat="server" CssClass="form-control select2">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <div class="form-group">
                                                            <span class="details">TIPO DE VEÍCULO:</span>
                                                            <asp:DropDownList ID="cboTipoVeiculo" runat="server" CssClass="form-control select2">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-1">
                                                        <div class="form-group">
                                                            <span class="details">EIXOS:</span>
                                                            <asp:DropDownList ID="ddlEixos" runat="server" CssClass="form-control">
                                                                <asp:ListItem Value="" Text="Selecione..."></asp:ListItem>
                                                                <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                                                <asp:ListItem Value="3" Text="3"></asp:ListItem>
                                                                <asp:ListItem Value="4" Text="4"></asp:ListItem>
                                                                <asp:ListItem Value="5" Text="5"></asp:ListItem>
                                                                <asp:ListItem Value="6" Text="6"></asp:ListItem>
                                                                <asp:ListItem Value="7" Text="7"></asp:ListItem>
                                                                <asp:ListItem Value="8" Text="8"></asp:ListItem>
                                                                <asp:ListItem Value="9" Text="9"></asp:ListItem>

                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <div class="form-group">
                                                            <span class="details">TIPO DE CARGA:</span>
                                                            <asp:DropDownList
                                                                ID="ddlTipoCargaANTT"
                                                                runat="server"
                                                                CssClass="form-control">
                                                                <asp:ListItem Value="" Text="Selecione..."></asp:ListItem>
                                                                <asp:ListItem Value="Granel sólido" Text="Granel sólido"></asp:ListItem>
                                                                <asp:ListItem Value="Granel líquido" Text="Granel líquido"></asp:ListItem>
                                                                <asp:ListItem Value="Frigorificada ou Aquecida" Text="Frigorificada ou Aquecida"></asp:ListItem>
                                                                <asp:ListItem Value="Conteinerizada" Text="Conteinerizada"></asp:ListItem>
                                                                <asp:ListItem Value="Carga Geral" Text="Carga Geral"></asp:ListItem>
                                                                <asp:ListItem Value="Neogranel" Text="Neogranel"></asp:ListItem>
                                                                <asp:ListItem Value="Perigosa (granel sólido)" Text="Perigosa (granel sólido)"></asp:ListItem>
                                                                <asp:ListItem Value="Perigosa (granel líquido)" Text="Perigosa (granel líquido)"></asp:ListItem>
                                                                <asp:ListItem Value="Perigosa (frigorificada ou aquecida)" Text="Perigosa (frigorificada ou aquecida)"></asp:ListItem>
                                                                <asp:ListItem Value="Perigosa (conteinerizada)" Text="Perigosa (conteinerizada)"></asp:ListItem>
                                                                <asp:ListItem Value="Perigosa (carga geral)" Text="Perigosa (carga geral)"></asp:ListItem>
                                                                <asp:ListItem Value="Carga Granel Pressurizada" Text="Carga Granel Pressurizada"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>

                                                </div>
                                                <div class="row g-3">
                                                    <div class="col-md-2">
                                                        <div class="form-group">
                                                            <span class="details">MATERIAL:</span>
                                                            <asp:DropDownList ID="cboTipoMaterial" runat="server" CssClass="form-control select2">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <div class="form-group">
                                                            <span class="details">DETALHE DO MATERIAL:</span>
                                                            <asp:TextBox ID="txtDetalheMaterial" runat="server" Style="text-align: left" CssClass="form-control" MaxLength="50"></asp:TextBox>
                                                        </div>
                                                    </div>                                                    
                                                    <div class="col-md-2">
                                                        <div class="form-group">
                                                            <span class="details">LOTAÇÃO MINIMA(KG):</span>
                                                            <asp:TextBox ID="txtPesoLotacao"
                                                                runat="server"
                                                                CssClass="form-control"
                                                                oninput="this.value=this.value.replace(/[^0-9]/g,'');">
                                                            </asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <div class="form-group">
                                                            <span class="details">VIGÊNCIA INICIAL:</span>
                                                            <asp:TextBox ID="txtVigenciaInicial" runat="server" Style="text-align: center" CssClass="form-control" MaxLength="10"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <div class="form-group">
                                                            <span class="details">VIGÊNCIA FINAL:</span>
                                                            <asp:TextBox ID="txtVigenciaFinal" runat="server" Style="text-align: center" CssClass="form-control" MaxLength="10"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                <br />
                                                <div class="row g-3">
                                                    <div class="col-md-2">
                                                        <div class="form-group">
                                                            <span class="details"><strong>TABELA ANTT:</strong></span>
                                                            <asp:DropDownList ID="ddlTabela"
                                                                runat="server" CssClass="form-control"
                                                                AutoPostBack="true"
                                                                OnSelectedIndexChanged="ddlTabela_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-2">
                                                        <span class="details"><strong>FRETE MINIMO ANTT:</strong></span>
                                                        <asp:TextBox ID="txtFreteMinimo" runat="server" CssClass="form-control"
                                                            oninput="mascaraMoeda(this);" ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                    <div class="col-sm-2">
                                                        <span class="details"><strong>FRETE A RECEBER</strong></span>
                                                        <asp:TextBox ID="txtFreteReceber"
                                                            runat="server"
                                                            CssClass="form-control"
                                                            oninput="mascaraMoeda(this); calcularTotalFrete();">
                                                        </asp:TextBox>
                                                    </div>
                                                    <div class="col-sm-2">
                                                        <span class="details"><strong>TOTAL DO FRETE</strong></span>
                                                        <asp:TextBox ID="txtTotalFrete"
                                                            runat="server"
                                                            CssClass="form-control"
                                                            ReadOnly="true">
                                                        </asp:TextBox>
                                                    </div>
                                                    <div class="col-sm-2">
                                                        <span class="details"><strong>MARGEM(%)</strong></span>
                                                        <asp:TextBox ID="txtMargem"
                                                            runat="server"
                                                            CssClass="form-control"
                                                            oninput="mascaraMoeda(this);"
                                                            onblur="calcularFrete();">
                                                        </asp:TextBox>
                                                    </div>
                                                    <div class="col-sm-2">
                                                        <span class="details"><strong>FRETE A PAGAR:</strong></span>
                                                        <asp:TextBox ID="txtFretePagar"
                                                            runat="server"
                                                            CssClass="form-control"
                                                            oninput="mascaraMoeda(this);"
                                                            onblur="calcularMargem();">
                                                        </asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="row g-3">
                                                    <div class="col-md-12">
                                                        <div class="form-group">
                                                            <label for="inputMensagem" class="col-sm-2 col-form-label">MENSAGEM NO CTE:</label>
                                                            <asp:TextBox ID="txtObservacao" class="form-control" runat="server" placeholder="Digite a mensagem que aparecerá no documento de transporte..."></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row g-3">
                                                    <label for="inputRemetente" class="col-sm-2 col-form-label" style="text-align: right">RESPONSÁVEL:</label>
                                                    <div class="col-md-3">
                                                        <asp:TextBox ID="txtResponsavel" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <asp:TextBox ID="txtData_Alteracao" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-3"></div>
                                                    <div class="col-md-2">
                                                        <asp:Button ID="btnLancarTabela" runat="server" CssClass="btn btn-outline-primary btn-lg w-100" OnClick="btnLancarTabela_Click"
                                                            Text="Gravar Frete" />
                                                    </div>
                                                </div>
                                                <br />
                                                <div class="row g-3">
                                                    <asp:GridView ID="gvFretes"
                                                        runat="server"
                                                        AutoGenerateColumns="False"
                                                        CssClass="table-sap"
                                                        DataKeyNames="id_frete"
                                                        OnRowCommand="gvFretes_RowCommand">

                                                        <Columns>
                                                            <asp:BoundField DataField="frete" HeaderText="Frete" />
                                                            <asp:BoundField DataField="medida" HeaderText="Medida" />
                                                            <asp:BoundField DataField="tipo_viagem" HeaderText="Tipo Viagem" />
                                                            <asp:BoundField DataField="tipo_veiculo" HeaderText="Veículo" />

                                                            <asp:TemplateField HeaderText="Material">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litMaterial" runat="server"
                                                                        Text='<%# Eval("material") + " <b>(" + Eval("detalhe_material") + ")</b>" %>'>
        </asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:BoundField DataField="tabela_antt" HeaderText="Tabela ANTT" />
                                                            <asp:BoundField DataField="frete_antt"
                                                                HeaderText="Frete ANTT"
                                                                DataFormatString="{0:N2}">
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:BoundField>

                                                            <asp:BoundField DataField="frete_receber"
                                                                HeaderText="A Receber"
                                                                DataFormatString="{0:N2}">
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:BoundField>

                                                            <asp:BoundField DataField="total_frete"
                                                                HeaderText="Total do Frete"
                                                                DataFormatString="{0:N2}">
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:BoundField>

                                                            <asp:BoundField DataField="margem"
                                                                HeaderText="Margem (%)"
                                                                DataFormatString="{0:N2}">
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:BoundField>

                                                            <asp:BoundField DataField="frete_pagar"
                                                                HeaderText="A Pagar"
                                                                DataFormatString="{0:N2}">
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="Início">
                                                                <ItemTemplate>
                                                                    <%# Convert.ToDateTime(Eval("vigencia_inicial")).ToString("dd/MM/yyyy") %>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Fim">
                                                                <ItemTemplate>
                                                                    <%# Eval("vigencia_final", "{0:dd/MM/yyyy}") %>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>


                                                            <asp:TemplateField HeaderText="Status">
                                                                <ItemTemplate>
                                                                    <asp:Button ID="btnStatus"
                                                                        runat="server"
                                                                        CssClass="btn btn-warning btn-sm"
                                                                        Text='<%# Eval("status") %>'
                                                                        CommandName="Status"
                                                                        CommandArgument='<%# Container.DataItemIndex %>'
                                                                        OnClientClick="return confirm('Deseja alterar status?');" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Editar">
                                                                <ItemTemplate>
                                                                    <asp:Button ID="btnEditar"
                                                                        runat="server"
                                                                        Text="Editar"
                                                                        CommandName="Editar"
                                                                        CommandArgument='<%# Eval("id_frete") %>'
                                                                        CssClass="btn btn-primary btn-sm" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                        </Columns>

                                                    </asp:GridView>
                                                </div>
                                            </div>
                                            <!-- /.card-body -->
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
                                <div class="col-md-3">
                                    <asp:Button ID="btnAlterar" runat="server" CssClass="btn btn-outline-success btn-lg w-100" OnClick="btnAlterar_Click"
                                        Text="Concluir Tabela" />
                                </div>
                                <div class="col-md-2">
                                    <a href="ConsultaFretes.aspx" class="btn btn-outline-danger btn-lg w-100">Fechar               
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
