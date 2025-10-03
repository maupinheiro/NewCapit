<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="Frm_Coleta.aspx.cs" Inherits="NewCapit.dist.pages.Frm_Coleta" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        function apenasNumeros(e) {
            var charCode = (e.which) ? e.which : e.keyCode;
            // Permite backspace, delete, setas e números (48-57)
            if (
                charCode > 31 && (charCode < 48 || charCode > 57)
            ) {
                return false;
            }
            return true;
        }
    </script>
    <script>
        document.addEventListener("DOMContentLoaded", function () {
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
            let txtJanOrigem = document.getElementById("<%= txtJanOrigem.ClientID %>");
            let txtJanDestino = document.getElementById("<%= txtJanDestino.ClientID %>");
            let txtSaida = document.getElementById("<%= txtSaida.ClientID %>");
            let txtPrevisaoChegada = document.getElementById("<%= txtPrevisaoChegada.ClientID %>");
            let txtChegada = document.getElementById("<%= txtChegada.ClientID %>");
            let txtConclusao = document.getElementById("<%= txtConclusao.ClientID %>");
            let txtCPF = document.getElementById("<%= txtCPF.ClientID %>");

            if (txtJanOrigem) aplicarMascara(txtJanOrigem, "00/00/0000 00:00");
            if (txtJanDestino) aplicarMascara(txtJanDestino, "00/00/0000 00:00");
            if (txtSaida) aplicarMascara(txtSaida, "00/00/0000 00:00");
            if (txtPrevisaoChegada) aplicarMascara(txtPrevisaoChegada, "00/00/0000 00:00");
            if (txtChegada) aplicarMascara(txtChegada, "00/00/0000 00:00");
            if (txtConclusao) aplicarMascara(txtConclusao, "00/00/0000 00:00");
            if (txtCPF) aplicarMascara(txtCPF, "000.000.000-00");


        });
    </script>
    <script type="text/javascript">
        function validarDataHora() {
            var txt = document.getElementById('<%= txtJanOrigem.ClientID %>').value;
            var txt = document.getElementById('<%= txtJanDestino.ClientID %>').value;
            // Regex para validar dd/MM/yyyy HH:mm
            var regex = /^([0-2][0-9]|(3)[0-1])\/(0[1-9]|1[0-2])\/\d{4} ([0-1][0-9]|2[0-3]):([0-5][0-9])$/;

            if (!regex.test(txt)) {
                alert("Data e hora inválidas. Use o formato dd/MM/yyyy HH:mm");
                return false;
            }
            return true;
        }
    </script>
    <script type="text/javascript">
        function checkAll(source) {
            var checkboxes = document.querySelectorAll('[id*="chkSelect"]');
            for (var i = 0; i < checkboxes.length; i++) {
                if (checkboxes[i].type == "checkbox") {
                    checkboxes[i].checked = source.checked;
                }
            }
        }
    </script>
    <div class="content-wrapper">
        <section class="content">
            <div class="container-fluid">
                <br />
                <div id="toastContainer" class="alert alert-warning alert-dismissible" style="display: none;">
                    <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                    <h5><i class="icon fas fa-exclamation-triangle"></i>Alerta!</h5>
                    Alertas
                </div>

                <div class="col-md-12">
                    <div class="card card-info">
                        <div class="card-header">
                            <h3 class="card-title">
                                <h3 class="card-title"><i class="fas fa-shipping-fast"></i>&nbsp;ORDEM DE COLETA - &nbsp;<asp:Label ID="novaColeta" runat="server"></asp:Label></h3>
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
                                    <span class="info-box-icon bg-info">
                                        <%--tamanho da foto 39x39 60--%>
                                        <img src="<%=fotoMotorista%>" class="rounded-circle float-center" width="60px" alt="" />
                                    </span>
                                    <div class="info-box-content">
                                        <span class="info-box-number">
                                            <div class="row g-3">
                                                <div class="col-md-1">
                                                    <div class="form-group">
                                                        <span class="details">CÓDIGO:</span>
                                                        <asp:TextBox ID="txtCodMot" Style="text-align: center" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtCodMot_TextChanged"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvtxtCodMot" runat="server" ControlToValidate="txtCodMot" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
                                                    </div>
                                                </div>
                                                <div class="col-md-4">
                                                    <div class="form-group">
                                                        <span class="details">NOME DO MOTORISTA:</span>
                                                        <asp:DropDownList ID="cboMotorista" runat="server" CssClass="form-control select2"></asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="col-md-2">
                                                    <div class="form-group">
                                                        <span class="details">CPF:</span>
                                                        <asp:TextBox ID="txtCPF" Style="text-align: center" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-2">
                                                    <div class="form-group">
                                                        <span class="details">CARTÃO PAMCARD:</span>
                                                        <asp:TextBox ID="txtCartaoPamcard" Style="text-align: center" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-2">
                                                    <div class="form-group">
                                                        <span class="details">CONT. PARTICULAR:</span>
                                                        <asp:TextBox ID="txtFoneParticular" Style="text-align: center" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-12">
                                <div class="card card-outline card-info">
                                    <div class="card-header">
                                        <h3 class="card-title"><i class="far fa-edit"></i>&nbsp;Dados do Veículo</h3>
                                        <div class="card-tools">
                                            <button type="button" class="btn btn-tool" data-card-widget="collapse">
                                                <i class="fas fa-minus"></i>
                                            </button>
                                        </div>
                                        <!-- /.card-tools -->
                                    </div>
                                    <!-- /.card-header -->
                                    <div class="card-body">
                                        <div class="row g-3">
                                            <div class="col-md-1">
                                                <div class="form-group">
                                                    <span class="details">VEÍCULO:</span>
                                                    <asp:TextBox ID="txtCodVei" Style="text-align: center" runat="server" CssClass="form-control"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtCodVei" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <span class="details">TIPO DE VEÍCULO:</span>
                                                    <asp:TextBox ID="txtTipoVeiculo" Style="text-align: center" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-1">
                                                <div class="form-group">
                                                    <span class="details">PLACA:</span>
                                                    <asp:TextBox ID="txtPlaca" Style="text-align: center" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-1">
                                                <div class="form-group">
                                                    <span class="details">REBOQUE1:</span>
                                                    <asp:TextBox ID="txtReboque1" Style="text-align: center" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-1">
                                                <div class="form-group">
                                                    <span class="details">REBOQUE2:</span>
                                                    <asp:TextBox ID="txtReboque2" Style="text-align: center" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <span class="details">TECNOLOGIA:</span>
                                                    <asp:TextBox ID="txtTecnologia" Style="text-align: center" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <span class="details">MONITORAMENTO:</span>
                                                    <asp:TextBox ID="txtMonitoramento" Style="text-align: center" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <span class="details">ESTABELECIMENTO:</span>
                                                    <asp:TextBox ID="txtNucleo" Style="text-align: center" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row g-3">
                                            <div class="col-md-1">
                                                <div class="form-group">
                                                    <span class="details">CONTATO:</span>
                                                    <asp:TextBox ID="txtCodContato" Style="text-align: center" runat="server" CssClass="form-control"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtCodMot" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <span class="details">FONE CORPORATIVO:</span>
                                                    <asp:TextBox ID="txtFoneCorporativo" Style="text-align: center" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <span class="details">TRANSPORTADOR:</span>
                                                    <asp:TextBox ID="txtTransportadora" Style="text-align: center" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <span class="details">TIPO DE CONJUNTO DO VEÍCULO:</span>
                                                    <asp:TextBox ID="txtTipoConjunto" Style="text-align: center" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-1">
                                                <div class="form-group">
                                                    <span class="details">CARRETA:</span>
                                                    <asp:TextBox ID="txtCarreta" Style="text-align: center" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <!-- /.card-body -->
                                </div>
                            </div>
                            <div class="col-md-12">
                                <div class="card card-outline card-info">
                                    <div class="card-header">
                                        <h3 class="card-title"><i class="far fa-edit"></i>&nbsp;Dados da Coleta/Entrega</h3>
                                        <div class="card-tools">
                                            <button type="button" class="btn btn-tool" data-card-widget="collapse">
                                                <i class="fas fa-minus"></i>
                                            </button>
                                        </div>
                                        <!-- /.card-tools -->
                                    </div>
                                    <div class="card-body">
                                        <div class="row g-3">
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <span class="details">ABERTURA:</span>
                                                    <asp:TextBox ID="txtCadastro" Style="text-align: center" runat="server" CssClass="form-control"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <span class="details">CARGA/SOLICITAÇÃO:</span>
                                                    <asp:TextBox ID="txtCarga" Style="text-align: center" onkeypress="return apenasNumeros(event);" runat="server" CssClass="form-control" OnTextChanged="txtCarga_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <span class="details">STATUS:</span>
                                                    <asp:DropDownList ID="cboStatus" runat="server" CssClass="form-control select2" ReadOnly="true"></asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <span class="details">SITUAÇÃO:</span>
                                                    <asp:TextBox ID="txtSituacao" Style="text-align: center" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>

                                        </div>
                                        <div class="row g-3">
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <span class="details">SOLICITANTE:</span>
                                                    <asp:TextBox ID="txtSolicitante" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <span class="details">GERENCIADORA DE RISCO(GR):</span>
                                                    <asp:TextBox ID="txtGR" Style="text-align: center" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <span class="details">MATERIAL:</span>
                                                    <asp:DropDownList ID="cboMaterial" Style="text-align: center" runat="server" CssClass="form-control select2"></asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-md-1">
                                                <div class="form-group">
                                                    <span class="details">PESO:</span>
                                                    <asp:TextBox ID="txtPesoCarga" Style="text-align: center" onkeypress="return apenasNumeros(event);" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-1">
                                                <div class="form-group">
                                                    <span class="details">CAP.CARGA:</span>
                                                    <asp:TextBox ID="txtCapCarga" Style="text-align: center" runat="server" CssClass="form-control" ReadOnly="true" ForeColor="Red"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-1">
                                                <div class="form-group">
                                                    <span class="details">CINTAS:</span>
                                                    <asp:TextBox ID="txtCintas" Style="text-align: center" onkeypress="return apenasNumeros(event);" runat="server" CssClass="form-control"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-1">
                                                <div class="form-group">
                                                    <span class="details">CATRACAS:</span>
                                                    <asp:TextBox ID="txtCatracas" Style="text-align: center" onkeypress="return apenasNumeros(event);" runat="server" CssClass="form-control"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group row">
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
                                            <!-- PAGADOR -->
                                            <div class="form-group row">
                                                <label for="inputPagador" class="col-sm-1 col-form-label" style="text-align: right">PAGADOR:</label>
                                                <div class="col-md-1">
                                                    <asp:TextBox ID="txtCodPagador" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                                </div>
                                                <div class="col-md-5">
                                                    <asp:TextBox ID="txtPagador" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:TextBox ID="txtCidPagador" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                                </div>
                                                <div class="col-md-1">
                                                    <asp:TextBox ID="txtUFPagador" Style="text-align: center" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-12">
                                <div class="card card-outline card-info">
                                    <div class="card-header">
                                        <h3 class="card-title"><i class="far fa-edit"></i>&nbsp;Dados Diversos</h3>
                                        <div class="card-tools">
                                            <button type="button" class="btn btn-tool" data-card-widget="collapse">
                                                <i class="fas fa-minus"></i>
                                            </button>
                                        </div>
                                        <!-- /.card-tools -->
                                    </div>
                                    <div class="card-body">
                                        <!-- Abas -->
                                        <div class="col-12 col-sm-12">
                                            <div class="card card-info card-outline card-outline-tabs">
                                                <div class="card-header p-0 border-bottom-0">
                                                    <ul class="nav nav-tabs" id="custom-tabs-four-tab" role="tablist">
                                                        <li class="nav-item">
                                                            <a class="nav-link active" id="custom-tabs-four-atendimento-tab" data-toggle="pill" href="#custom-tabs-four-atendimento" role="tab" aria-controls="custom-tabs-four-atendimento" aria-selected="true">Atendimento</a>
                                                        </li>
                                                        <li class="nav-item">
                                                            <a class="nav-link" id="custom-tabs-four-pedidos-tab" data-toggle="pill" href="#custom-tabs-four-profile" role="tab" aria-controls="custom-tabs-four-profile" aria-selected="false">Pedidos</a>
                                                        </li>
                                                        <li class="nav-item">
                                                            <a class="nav-link" id="custom-tabs-four-documentos-tab" data-toggle="pill" href="#custom-tabs-four-messages" role="tab" aria-controls="custom-tabs-four-messages" aria-selected="false">CT-e/RPS</a>
                                                        </li>
                                                        <li class="nav-item">
                                                            <a class="nav-link" id="custom-tabs-four-notasfiscais-tab" data-toggle="pill" href="#custom-tabs-four-messages" role="tab" aria-controls="custom-tabs-four-messages" aria-selected="false">Notas Fiscais</a>
                                                        </li>
                                                        <li class="nav-item">
                                                            <a class="nav-link" id="custom-tabs-four-pedagio-tab" data-toggle="pill" href="#custom-tabs-four-settings" role="tab" aria-controls="custom-tabs-four-settings" aria-selected="false">Emissão de Pedágio</a>
                                                            <li class="nav-item">
                                                                <a class="nav-link" id="custom-tabs-four-krona-tab" data-toggle="pill" href="#custom-tabs-four-mensagem" role="tab" aria-controls="custom-tabs-four-settings" aria-selected="false">SM - Krona</a>
                                                            </li>
                                                            <li class="nav-item">
                                                                <a class="nav-link" id="custom-tabs-four-pernoite-tab" data-toggle="pill" href="#custom-tabs-four-mensagem" role="tab" aria-controls="custom-tabs-four-settings" aria-selected="false">Pernoite</a>
                                                            </li>
                                                            <li class="nav-item">
                                                                <a class="nav-link" id="custom-tabs-four-historico-tab" data-toggle="pill" href="#custom-tabs-four-mensagem" role="tab" aria-controls="custom-tabs-four-settings" aria-selected="false">Histórico da Viagem</a>
                                                            </li>
                                                        </li>
                                                    </ul>
                                                </div>
                                                <div class="card-body">
                                                    <div class="tab-content" id="custom-tabs-four-tabContent">
                                                        <!-- ATENDIMENTO -->
                                                        <div class="tab-pane fade show active" id="custom-tabs-four-home" role="tabpanel" aria-labelledby="custom-tabs-four-atendimento-tab">
                                                            <div class="row g-3">
                                                                <div class="col-md-1">
                                                                    <div class="form-group">
                                                                        <span class="details">ADD. DOC.:</span>
                                                                        <asp:TextBox ID="txtDocumentos" Style="text-align: center" onkeypress="return apenasNumeros(event);" runat="server" CssClass="form-control"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                                <div class="col-sm-1">
                                                                    <br />
                                                                    <div class="form-group">
                                                                        <div class="custom-control custom-radio">
                                                                            <input class="custom-control-input custom-control-input-info custom-control-input-outline" type="radio" id="customRadioCTe" name="customRadioTipo">
                                                                            <label for="customRadioCTe" class="custom-control-label">CT-e</label>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="col-sm-1">
                                                                    <br />
                                                                    <div class="form-group">
                                                                        <div class="custom-control custom-radio">
                                                                            <input class="custom-control-input custom-control-input-info custom-control-input-outline" type="radio" id="customRadioMDFe" name="customRadioTipo">
                                                                            <label for="customRadioMDFe" class="custom-control-label">MDF-e</label>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="col-sm-1">
                                                                    <br />
                                                                    <div class="form-group">
                                                                        <div class="custom-control custom-radio">
                                                                            <input class="custom-control-input custom-control-input-info custom-control-input-outline" type="radio" id="customRadioNV" name="customRadioTipo">
                                                                            <label for="customRadioNV" class="custom-control-label">NV</label>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="col-md-4" id="tipoSolicitacao">
                                                                    <div class="form-group">
                                                                        <span class="details">TIPO DE SOLICITAÇÃO:</span>
                                                                        <asp:TextBox ID="txtTipoSolicitacao" Style="text-align: center" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-4" id="tipoVeiculo">
                                                                    <div class="form-group">
                                                                        <span class="details">TIPO DE VEÍCULO:</span>
                                                                        <asp:TextBox ID="txtTipoDeVeiculo" Style="text-align: center" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                                                    </div>
                                                                </div>

                                                            </div>
                                                            <div class="row g-3">
                                                                <div class="col-md-2">
                                                                    <div class="form-group">
                                                                        <span class="details">JANELA ORIGEM:</span>
                                                                        <asp:TextBox ID="txtJanOrigem" Style="text-align: center" runat="server" CssClass="form-control"></asp:TextBox>
                                                                        <asp:RegularExpressionValidator
                                                                            ID="revJanOrigem"
                                                                            runat="server"
                                                                            ControlToValidate="txtJanOrigem"
                                                                            ErrorMessage="Corrija a data."
                                                                            ValidationExpression="^([0-2][0-9]|(3)[0-1])/(0[1-9]|1[0-2])/\d{4} ([0-1][0-9]|2[0-3]):([0-5][0-9])$"
                                                                            ForeColor="Red" />
                                                                        <asp:Label ID="lblResultado" runat="server" />
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-2">
                                                                    <div class="form-group">
                                                                        <span class="details">JANELA DESTINO:</span>
                                                                        <asp:TextBox ID="txtJanDestino" Style="text-align: center" runat="server" CssClass="form-control" OnClientClick="return validarDataHora();"></asp:TextBox>
                                                                        <asp:Label ID="Label1" runat="server" />
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-2">
                                                                    <div class="form-group">
                                                                        <span class="details">SAIDA:</span>
                                                                        <asp:TextBox ID="txtSaida" Style="text-align: center" runat="server" CssClass="form-control"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-2">
                                                                    <div class="form-group">
                                                                        <span class="details">PREVISÃO CHEGADA:</span>
                                                                        <asp:TextBox ID="txtPrevisaoChegada" Style="text-align: center" runat="server" CssClass="form-control"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-2">
                                                                    <div class="form-group">
                                                                        <span class="details">CHEGADA:</span>
                                                                        <asp:TextBox ID="txtChegada" Style="text-align: center" runat="server" CssClass="form-control"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-2">
                                                                    <div class="form-group">
                                                                        <span class="details">CONCLUSÃO:</span>
                                                                        <asp:TextBox ID="txtConclusao" Style="text-align: center" runat="server" CssClass="form-control"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="row g-3">
                                                                <div class="col-md-2">
                                                                    <div class="form-group">
                                                                        <span class="details">Nº CARREG. FERROLENE:</span>
                                                                        <asp:TextBox ID="txtContFerrolene" Style="text-align: center" runat="server" CssClass="form-control"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-2">
                                                                    <div class="form-group">
                                                                        <span class="details">Nº CVA:</span>
                                                                        <asp:TextBox ID="txtCVA" Style="text-align: center" runat="server" CssClass="form-control"></asp:TextBox>
                                                                    </div>
                                                                </div>

                                                                <div class="col-md-2">
                                                                    <div class="form-group">
                                                                        <span class="details">AG. CARREG.:</span>
                                                                        <asp:TextBox ID="txtAgCarreg" Style="text-align: center" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-2">
                                                                    <div class="form-group">
                                                                        <span class="details">AG. DESCARGA:</span>
                                                                        <asp:TextBox ID="txtAgDescarga" Style="text-align: center" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-2">
                                                                    <div class="form-group">
                                                                        <span class="details">TT NO TRANSP.:</span>
                                                                        <asp:TextBox ID="txtTempoTotal" Style="text-align: center" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                            </div>

                                                        </div>
                                                        <!-- Pedidos -->
                                                        <div class="tab-pane fade" id="custom-tabs-four-profile" role="tabpanel" aria-labelledby="custom-tabs-four-profile-tab">
                                                            <div class="card-body table-responsive p-0" style="height: 260px;">
                                                                <table class="table table-head-fixed text-nowrap">
                                                                    <%-- <asp:GridView ID="gvPedidos" runat="server" AutoGenerateColumns="False" CssClass="table table-striped" EmptyDataText="Nenhum pedido encontrado." OnRowEditing="gvPedidos_RowEditing"
                                                                        OnRowUpdating="gvPedidos_RowUpdating" OnRowCancelingEdit="gvPedidos_RowCancelingEdit">--%>
                                                                    <asp:GridView ID="gvPedidos" runat="server" CssClass="table table-striped"
                                                                        AutoGenerateColumns="False"
                                                                        DataKeyNames="carga"
                                                                        OnRowEditing="gvPedidos_RowEditing"
                                                                        OnRowUpdating="gvPedidos_RowUpdating"
                                                                        OnRowCancelingEdit="gvPedidos_RowCancelingEdit">
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="Del" ShowHeader="True" ItemStyle-Width="9">
                                                                                <ItemTemplate>
                                                                                    <asp:LinkButton ID="lnkExcluir" runat="server" Style="text-align: center" CssClass="btn btn-danger btn-sm"><i class="fa fa-trash"></i></i>
                                                                                    </asp:LinkButton>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:BoundField DataField="pedido" HeaderText="Pedido" ReadOnly="True" />
                                                                            <asp:BoundField DataField="portao" HeaderText="Deposito" ReadOnly="True" />
                                                                            <asp:BoundField DataField="peso" HeaderText="Peso" ReadOnly="True" />
                                                                            <asp:BoundField DataField="material" HeaderText="Material" ReadOnly="True" />

                                                                            <asp:TemplateField>
                                                                                <HeaderTemplate>
                                                                                    <asp:CheckBox ID="chkHeader" runat="server" onclick="checkAll(this);" />
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                                                                </ItemTemplate>

                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Motorista Carregamento">
                                                                                <ItemTemplate>
                                                                                    <%# Eval("motcar") %>
                                                                                </ItemTemplate>
                                                                                <EditItemTemplate>
                                                                                    <asp:DropDownList ID="ddlMotoristas" runat="server" />
                                                                                </EditItemTemplate>
                                                                            </asp:TemplateField>

                                                                            <asp:TemplateField HeaderText="Inicio Carreg.">
                                                                                <ItemTemplate>
                                                                                    <%# Eval("iniciocar", "{0:dd/MM/yyyy HH:mm}") %>
                                                                                </ItemTemplate>
                                                                                <EditItemTemplate>
                                                                                    <asp:TextBox ID="txtDataHoraCarreg" runat="server" Text='<%# Bind("iniciocar", "{0:yyyy-MM-dd HH:mm}") %>' />
                                                                                    <asp:RegularExpressionValidator ID="revDataHoraCarreg" runat="server"
                                                                                        ControlToValidate="txtDataHoraCarreg"
                                                                                        ValidationExpression="^\d{4}-\d{2}-\d{2} \d{2}:\d{2}$"
                                                                                        ErrorMessage="Data inválida."
                                                                                        ForeColor="Red" />
                                                                                </EditItemTemplate>
                                                                            </asp:TemplateField>

                                                                            <asp:TemplateField HeaderText="Termino Carreg.">
                                                                                <ItemTemplate>
                                                                                    <%# Eval("termcar", "{0:dd/MM/yyyy HH:mm}") %>
                                                                                </ItemTemplate>
                                                                                <EditItemTemplate>
                                                                                    <asp:TextBox ID="txtDataHoraFimCarreg" runat="server" Text='<%# Bind("termcar", "{0:yyyy-MM-dd HH:mm}") %>' />
                                                                                    <asp:RegularExpressionValidator ID="revDataHoraFimCarreg" runat="server"
                                                                                        ControlToValidate="txtDataHoraFimCarreg"
                                                                                        ValidationExpression="^\d{4}-\d{2}-\d{2} \d{2}:\d{2}$"
                                                                                        ErrorMessage="Data inválida."
                                                                                        ForeColor="Red" />
                                                                                </EditItemTemplate>
                                                                            </asp:TemplateField>

                                                                            <asp:TemplateField HeaderText="Duração">
                                                                                <%--<ItemTemplate>
                                                                                    <%# GetDuracaoHoras(Eval("iniciocar"), Eval("termcar")) %>
                                                                                </ItemTemplate>--%>
                                                                            </asp:TemplateField>

                                                                            <asp:TemplateField HeaderText="Salvar" ShowHeader="True" ItemStyle-Width="9">
                                                                                <ItemTemplate>
                                                                                    <asp:LinkButton ID="lnkSalvar" runat="server" Style="text-align: center" CssClass="btn btn-success btn-sm"><i class="fas fa-redo-alt"></i></i>
                                                                                    </asp:LinkButton>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>

                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </table>
                                                            </div>
                                                        </div>
                                                        <!-- frete Agregado/Terceiro -->
                                                        <div class="tab-pane fade" id="custom-tabs-four-messages" role="tabpanel" aria-labelledby="custom-tabs-four-messages-tab">
                                                            <div class="form-group row">
                                                                <label for="inputFilial" class="col-sm-2 col-form-label" style="text-align: right">VALOR DO FRETE:</label>
                                                                <div class="col-sm-1">
                                                                    <asp:TextBox ID="txtFreteAgregado" runat="server" CssClass="form-control" Style="text-align: center" placeholder="000,00"></asp:TextBox>
                                                                </div>
                                                                <label for="inputFilial" class="col-sm-1 col-form-label" style="text-align: right">FRETE FIXO:</label>
                                                                <div class="col-sm-2">
                                                                    <asp:DropDownList ID="ddlFixoAgregado" runat="server" CssClass="form-control">
                                                                        <asp:ListItem Value="" Text="Selecione..."></asp:ListItem>
                                                                        <asp:ListItem Value="SIM" Text="SIM"></asp:ListItem>
                                                                        <asp:ListItem Value="NÃO" Text="NÃO"></asp:ListItem>
                                                                        <asp:ListItem Value="KILO" Text="KILO"></asp:ListItem>
                                                                        <asp:ListItem Value="TONELADA" Text="TONELADA"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </div>
                                                                <label for="inputFilial" class="col-sm-3 col-form-label" style="text-align: right">ALUGUEL DE CARRETA TNG (%):</label>
                                                                <div class="col-sm-1">
                                                                    <asp:TextBox ID="txtPercentualAluguelCarreta" runat="server" CssClass="form-control" Style="text-align: center" placeholder="000,00"></asp:TextBox>
                                                                </div>
                                                                <label for="inputFilial" class="col-sm-1 col-form-label" style="text-align: right">% FRETE TNG:</label>
                                                                <div class="col-sm-1">
                                                                    <asp:TextBox ID="txtPercTNGAgregado" runat="server" CssClass="form-control" Style="text-align: center" placeholder="000,00"></asp:TextBox>
                                                                </div>
                                                            </div>

                                                        </div>
                                                        <!-- frete Especial -->
                                                        <div class="tab-pane fade" id="custom-tabs-four-settings" role="tabpanel" aria-labelledby="custom-tabs-four-settings-tab">
                                                            <div class="form-group row">
                                                                <label for="inputFilial" class="col-sm-2 col-form-label" style="text-align: right">AGREGADO/TERCEIRO:</label>
                                                                <div class="col-sm-1">
                                                                    <asp:TextBox ID="txtCodAgregado" runat="server" CssClass="form-control" Style="text-align: center"></asp:TextBox>
                                                                </div>
                                                                <div class="col-sm-6">
                                                                    <asp:TextBox ID="txtNomAgregado" runat="server" CssClass="form-control" Style="text-align: left"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="form-group row">
                                                                <label for="inputFilial" class="col-sm-2 col-form-label" style="text-align: right">VALOR DO FRETE:</label>
                                                                <div class="col-sm-1">
                                                                    <asp:TextBox ID="txtFreteEspecial" runat="server" CssClass="form-control" Style="text-align: center" placeholder="000,00"></asp:TextBox>
                                                                </div>
                                                                <label for="inputFilial" class="col-sm-1 col-form-label" style="text-align: right">FRETE FIXO:</label>
                                                                <div class="col-sm-2">
                                                                    <asp:DropDownList ID="ddlEspecial" runat="server" CssClass="form-control">
                                                                        <asp:ListItem Value="" Text="Selecione..."></asp:ListItem>
                                                                        <asp:ListItem Value="SIM" Text="SIM"></asp:ListItem>
                                                                        <asp:ListItem Value="NÃO" Text="NÃO"></asp:ListItem>
                                                                        <asp:ListItem Value="KILO" Text="KILO"></asp:ListItem>
                                                                        <asp:ListItem Value="TONELADA" Text="TONELADA"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </div>
                                                                <label for="inputFilial" class="col-sm-3 col-form-label" style="text-align: right">ALUGUEL DE CARRETA TNG (%):</label>
                                                                <div class="col-sm-1">
                                                                    <asp:TextBox ID="txtAluguelCarretaEspecial" runat="server" CssClass="form-control" Style="text-align: center" placeholder="000,00"></asp:TextBox>
                                                                </div>
                                                                <label for="inputFilial" class="col-sm-1 col-form-label" style="text-align: right">% FRETE TNG:</label>
                                                                <div class="col-sm-1">
                                                                    <asp:TextBox ID="txtPercTNGEspecial" runat="server" CssClass="form-control" Style="text-align: center" placeholder="000,00"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <!-- frete Mensagem -->
                                                        <div class="tab-pane fade" id="custom-tabs-four-mensagem" role="tabpanel" aria-labelledby="custom-tabs-four-mensagem-tab">
                                                            <div class="row g-3">
                                                                <div class="col-md-12">
                                                                    <div class="form-group">
                                                                        <label for="inputMensagem" class="col-sm-2 col-form-label">MENSAGEM NO CTE:</label>
                                                                        <asp:TextBox ID="txtMensagem" class="form-control" runat="server" placeholder="Digite a mensagem que aparecerá no documento de transporte..."></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <!-- /.card -->
                                            </div>
                                        </div>


                                    </div>
                                </div>



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
                                            <asp:TextBox ID="txtUsuCadastro" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <!-- Linha 9 do formulário -->
                                <div class="row g-3">
                                    <div class="col-md-1">
                                        <asp:Button ID="btnSalvar" CssClass="btn btn-outline-success  btn-lg" runat="server" Text="Cadastrar" />
                                    </div>
                                    <div class="col-md-1">
                                        <a href="/dist/pages/ConsultaMotoristas.aspx" class="btn btn-outline-danger btn-lg">Sair               
                                        </a>
                                    </div>
                                </div>

                            </div>
                            <!-- /.card-body -->
                        </div>
                        <!-- /.card -->
                    </div>





                </div>
        </section>
    </div>
</asp:Content>
