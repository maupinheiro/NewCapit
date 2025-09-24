<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="Frm_TabelaPrecoMatriz.aspx.cs" Inherits="NewCapit.dist.pages.Frm_TabelaPrecoMatriz" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>

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
                <div class="card card-info">
                    <div class="card-header">
                        <h3 class="card-title"><i class="fas fa-hand-holding-usd"></i>&nbsp;TABELA DE FRETES - NOVO CADASTRO</h3>
                    </div>
                </div>
                <div class="card-header">
                    <form class="form-horizontal">
                        <div class="card-body">
                            <div class="form-group row">
                                <label for="inputFilial" class="col-sm-1 col-form-label" style="text-align: right">FILIAL:</label>
                                <div class="col-sm-3">
                                    <asp:DropDownList ID="cboFilial" runat="server" CssClass="form-control select2"></asp:DropDownList>
                                </div>
                                <div class="col-md-2"></div>
                                <label for="inputFilial" class="col-sm-1 col-form-label" style="text-align: right">SITUAÇÃO:</label>
                                <div class="col-sm-2">
                                    <asp:TextBox ID="txtStatusRota" runat="server" CssClass="form-control" Style="text-align: center" ReadOnly="true"></asp:TextBox>
                                </div>
                                <label for="inputFilial" class="col-sm-1 col-form-label" style="text-align: right">CADASTRO:</label>
                                <div class="col-sm-2">
                                    <asp:TextBox ID="txtCadastro" runat="server" CssClass="form-control" Style="text-align: center" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group row">
                                <label for="inputFilial" class="col-sm-1 col-form-label" style="text-align: right">ROTA:</label>
                                <div class="col-sm-1">
                                    <asp:TextBox ID="txtRota" runat="server" CssClass="form-control" Style="text-align: center" OnTextChanged="txtRota_TextChanged" AutoPostBack="true"></asp:TextBox>
                                </div>
                                <div class="col-md-10">
                                    <asp:DropDownList ID="cboRotas" runat="server" CssClass="form-control select2" AutoPostBack="true" OnSelectedIndexChanged="cboRotas_SelectedIndexChanged" ></asp:DropDownList>
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
                                <label for="inputDestinatario" class="col-sm-1 col-form-label" style="text-align: right">DESTINATÁRIO:</label>
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
                                    <asp:TextBox ID="txtCodPagador" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="cboPagador_SelectedIndexChanged"></asp:TextBox>
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
                    </form>
                    <div class="row g-3">
                        <div class="col-md-2">
                            <div class="form-group">
                                <label for="inputDistancia" class="col-sm-2 col-form-label">DISTÂNCIA(KM):</label>
                                <asp:TextBox ID="txtDistancia" class="form-control" runat="server" ReadOnly="true" Style="text-align: center"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form-group">
                                <label for="inputDuracao" class="col-sm-1 col-form-label">DURAÇÃO:</label>
                                <asp:TextBox ID="txtDuracao" class="form-control" runat="server" placeholder="00:00" ReadOnly="true" Style="text-align: center"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form-group">
                                <label for="inputDistancia" class="col-sm-2 col-form-label">DESLOCAMENTO:</label>
                                <asp:DropDownList ID="cboDeslocamento" runat="server" CssClass="form-control" ReadOnly="true">
                                    <asp:ListItem Value="" Text="Selecione..."></asp:ListItem>
                                    <asp:ListItem Value="MUNICIPAL" Text="MUNICIPAL"></asp:ListItem>
                                    <asp:ListItem Value="INTERMUNICIAPAL" Text="INTERMUNICIAPAL"></asp:ListItem>
                                    <asp:ListItem Value="INTERESTADUAL" Text="INTERESTADUAL"></asp:ListItem>
                                    <asp:ListItem Value="INTERNACIONAL" Text="INTERNACIONAL"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="form-group">
                                <label for="inputDistancia" class="col-sm-3 col-form-label">TIPO_DE_VIAGEM:</label>
                                <asp:DropDownList ID="cboTipoViagem" runat="server" CssClass="form-control select2">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label for="inputDistancia" class="col-sm-4 col-form-label">TIPO_DE_VEICULO:</label>
                                <asp:DropDownList ID="cboTipoVeiculo" runat="server" CssClass="form-control select2">
                                </asp:DropDownList>
                            </div>
                        </div>

                    </div>
                    <!-- Abas -->
                    <div class="col-12 col-sm-12">
                        <div class="card card-info card-outline card-outline-tabs">
                            <div class="card-header p-0 border-bottom-0">
                                <ul class="nav nav-tabs" id="custom-tabs-four-tab" role="tablist">
                                    <li class="nav-item">
                                        <a class="nav-link active" id="custom-tabs-four-home-tab" data-toggle="pill" href="#custom-tabs-four-home" role="tab" aria-controls="custom-tabs-four-home" aria-selected="true">Dados do Frete</a>
                                    </li>
                                    <li class="nav-item">
                                        <a class="nav-link" id="custom-tabs-four-profile-tab" data-toggle="pill" href="#custom-tabs-four-profile" role="tab" aria-controls="custom-tabs-four-profile" aria-selected="false">Frete Transnovag</a>
                                    </li>
                                    <li class="nav-item">
                                        <a class="nav-link" id="custom-tabs-four-messages-tab" data-toggle="pill" href="#custom-tabs-four-messages" role="tab" aria-controls="custom-tabs-four-messages" aria-selected="false">Frete Agregado/Terceiro</a>
                                    </li>
                                    <li class="nav-item">
                                        <a class="nav-link" id="custom-tabs-four-settings-tab" data-toggle="pill" href="#custom-tabs-four-settings" role="tab" aria-controls="custom-tabs-four-settings" aria-selected="false">Frete Especial</a>
                                        <li class="nav-item">
                                            <a class="nav-link" id="custom-tabs-four-mensagem-tab" data-toggle="pill" href="#custom-tabs-four-mensagem" role="tab" aria-controls="custom-tabs-four-settings" aria-selected="false">Mensagem no CTE</a>
                                        </li>
                                    </li>
                                </ul>
                            </div>
                            <div class="card-body">
                                <div class="tab-content" id="custom-tabs-four-tabContent">
                                    <!-- dados do frete -->
                                    <div class="tab-pane fade show active" id="custom-tabs-four-home" role="tabpanel" aria-labelledby="custom-tabs-four-home-tab">
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
                                            <label for="inputFilial" class="col-sm-2 col-form-label" style="text-align: right">LOTAÇÃO (KG):</label>
                                            <div class="col-sm-1">
                                                <asp:TextBox ID="txtLotacao" runat="server" CssClass="form-control" Style="text-align: center" placeholder="00000"></asp:TextBox>
                                            </div>
                                            <label for="inputFilial" class="col-sm-3 col-form-label" style="text-align: right">MATERIAL:</label>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="cboTipoMaterial" runat="server" CssClass="form-control select2">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="row g-3">
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <span class="details">VIGÊNCIA INICIAL:</span>
                                                    <asp:TextBox ID="txtVigenciaInicial" TextMode="Date" runat="server" Style="text-align: center" CssClass="form-control font-weight-bold" MaxLength="10"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <span class="details">VICÊNCIA FINAL:</span>
                                                    <asp:TextBox ID="txtVigenciaFinal" TextMode="Date" runat="server" Style="text-align: center" CssClass="form-control font-weight-bold" MaxLength="10"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <span class="details">TOTAL DO FRETE:</span>
                                                    <asp:TextBox ID="txtTotalFrete" runat="server" Style="text-align: center" CssClass="form-control font-weight-bold" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <form class="form-horizontal">
                                            <div class="card-body">
                                                <div class="form-group row">
                                                    <label for="inputFilial" class="col-sm-1 col-form-label" style="text-align: right">ADIC.(%):</label>
                                                    <div class="col-sm-1">
                                                        <asp:TextBox ID="txtAdicional" runat="server" CssClass="form-control" Style="text-align: center" placeholder="000,00"></asp:TextBox>
                                                    </div>
                                                    <label for="inputFilial" class="col-sm-1 col-form-label" style="text-align: right">SEC-CAT:</label>
                                                    <div class="col-sm-1">
                                                        <asp:TextBox ID="txtSecCat" runat="server" CssClass="form-control" Style="text-align: center" placeholder="000,00"></asp:TextBox>
                                                    </div>
                                                    <label for="inputFilial" class="col-sm-1 col-form-label" style="text-align: right">DESPACHO:</label>
                                                    <div class="col-sm-1">
                                                        <asp:TextBox ID="txtDespacho" runat="server" CssClass="form-control" Style="text-align: center" placeholder="000,00"></asp:TextBox>
                                                    </div>
                                                    <label for="inputFilial" class="col-sm-1 col-form-label" style="text-align: right">PEDÁGIO:</label>
                                                    <div class="col-sm-1">
                                                        <asp:TextBox ID="txtPedagio" runat="server" CssClass="form-control" Style="text-align: center" placeholder="000,00"></asp:TextBox>
                                                    </div>
                                                    <label for="inputFilial" class="col-sm-1 col-form-label" style="text-align: right">OUTROS:</label>
                                                    <div class="col-sm-1">
                                                        <asp:TextBox ID="txtOutros" runat="server" CssClass="form-control" Style="text-align: center" placeholder="000,00"></asp:TextBox>
                                                    </div>
                                                    <label for="inputFilial" class="col-sm-1 col-form-label" style="text-align: right">STATUS:</label>
                                                    <div class="col-sm-1">
                                                        <asp:DropDownList ID="DropDownList1" runat="server" CssClass="form-control">
                                                            <asp:ListItem Value="" Text="Selecione..."></asp:ListItem>
                                                            <asp:ListItem Value="ATIVO" Text="ATIVO"></asp:ListItem>
                                                            <asp:ListItem Value="INATIVO" Text="INATIVO"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </form>
                                    </div>
                                    <!-- frete TNG -->
                                    <div class="tab-pane fade" id="custom-tabs-four-profile" role="tabpanel" aria-labelledby="custom-tabs-four-profile-tab">
                                        <div class="form-group row">
                                            <label for="inputFilial" class="col-sm-2 col-form-label" style="text-align: right">VALOR DO FRETE:</label>
                                            <div class="col-sm-1">
                                                <asp:TextBox ID="txtFreteTNG" runat="server" CssClass="form-control" Style="text-align: center" placeholder="000,00"></asp:TextBox>
                                            </div>
                                            <label for="inputFilial" class="col-sm-1 col-form-label" style="text-align: right">FRETE FIXO:</label>
                                            <div class="col-sm-2">
                                                <asp:DropDownList ID="ddlValorFixoTng" runat="server" CssClass="form-control">
                                                    <asp:ListItem Value="" Text="Selecione..."></asp:ListItem>
                                                    <asp:ListItem Value="SIM" Text="SIM"></asp:ListItem>
                                                    <asp:ListItem Value="NÃO" Text="NÃO"></asp:ListItem>
                                                    <asp:ListItem Value="KILO" Text="KILO"></asp:ListItem>
                                                    <asp:ListItem Value="TONELADA" Text="TONELADA"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
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
                            <asp:Button ID="btnAlterar" runat="server" CssClass="btn btn-outline-success btn-lg" Text="Atualizar" />
                        </div>
                        <div class="col-md-1">
                            <a href="ConsultaFretes.aspx" class="btn btn-outline-danger btn-lg">Sair               
                            </a>
                        </div>
                    </div>
                </div>
        </section>
    </div>

</asp:Content>
