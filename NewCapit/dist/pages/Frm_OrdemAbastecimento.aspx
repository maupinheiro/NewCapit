<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="Frm_OrdemAbastecimento.aspx.cs" Inherits="NewCapit.dist.pages.Frm_OrdemAbastecimento" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery.mask/1.14.16/jquery.mask.min.js"></script>

    <script>
        document.addEventListener("DOMContentLoaded", function () {
            function aplicarMascaraLatitudeLongitude(input) {
                input.addEventListener("input", function () {
                    let valor = input.value;

                    // Garante que o "-" sempre esteja no início
                    if (!valor.startsWith("-")) {
                        valor = "-" + valor.replace(/[^0-9.]/g, ""); // Remove caracteres inválidos e adiciona "-"
                    } else {
                        valor = "-" + valor.substring(1).replace(/[^0-9.]/g, ""); // Mantém o "-" e filtra o resto
                    }

                    // Remove pontos extras, mantendo apenas o primeiro
                    let partes = valor.split(".");
                    if (partes.length > 2) {
                        valor = partes[0] + "." + partes.slice(1).join(""); // Remove pontos extras
                    }

                    // Garante que tenha no máximo 2 dígitos antes do ponto
                    let match = valor.match(/^-?\d{0,2}(\.\d{0,8})?/);
                    if (match) {
                        valor = match[0];
                    }

                    input.value = valor;
                });

                // Adiciona o "-" automaticamente se o campo estiver vazio ao perder o foco
                input.addEventListener("blur", function () {
                    if (input.value === "-") {
                        input.value = "";
                    }
                });
            }

        });
    </script>
    <script>
        $(function () {
            $("#txtS500").inputmask("currency", {
                prefix: "R$ ",
                groupSeparator: ".",
                radixPoint: ",",
                digits: 2,
                autoGroup: true,
                rightAlign: false
            });
        });
    </script>
    <script>
        $(function () {
            $("#txtS10").inputmask("currency", {
                prefix: "R$ ",
                groupSeparator: ".",
                radixPoint: ",",
                digits: 2,
                autoGroup: true,
                rightAlign: false
            });
        });
    </script>
    <script>
        $(function () {
            $("#txtEtanol").inputmask("currency", {
                prefix: "R$ ",
                groupSeparator: ".",
                radixPoint: ",",
                digits: 2,
                autoGroup: true,
                rightAlign: false
            });
        });
    </script>
    <script>
        $(function () {
            $("#txtGasolina").inputmask("currency", {
                prefix: "R$ ",
                groupSeparator: ".",
                radixPoint: ",",
                digits: 2,
                autoGroup: true,
                rightAlign: false
            });
        });
    </script>
    <script>
        $(function () {
            $("#txtArla").inputmask("currency", {
                prefix: "R$ ",
                groupSeparator: ".",
                radixPoint: ",",
                digits: 2,
                autoGroup: true,
                rightAlign: false
            });
        });
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
                <div class="card card-success">
                    <div class="card-header">
                        <h3 class="card-title"><i class="fas fa-gas-pump"></i>&nbsp;ORDEM DE ABASTECIMENTO</h3>
                    </div>
                </div>
                <br />
                <div class="card card-success card-outline direct-chat direct-chat-success shadow-none">
                    <div class="card-header">
                        <br />
                        <div class="row g-3">
                            <div class="col-md-1">
                                <div class="form-group">                                    
                                    <asp:TextBox ID="txtExterno" runat="server" Style="text-align: center" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row g-3">
                            <div class="col-md-1">
                                <div class="form-group">
                                    <span class="direct-chat-name float-left">FORNECEDOR:</span>
                                    <asp:TextBox ID="txtCodFor" runat="server" CssClass="form-control direct-chat-name float-left" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-5">
                                <div class="form-group">
                                    <span class="details">POSTO:</span>
                                    <asp:TextBox ID="txtNomFor" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="form-group">
                                    <span class="direct-chat-name float-left">COMBUSTÍVEL:</span>
                                   <%-- <asp:TextBox ID="txtCombustivel" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>--%>
                                    <asp:DropDownList ID="ddlFrutas" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlFrutas_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-md-2">
                                <div class="form-group">
                                    <span class="direct-chat-name float-left">PREÇO UNIT.:</span>
                                    <asp:TextBox ID="txtPreco" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>

            </div>
            </div>
                </div>
                <div class="card card-success card-outline direct-chat direct-chat-success shadow-none">
                    <div class="card-header">
                        <br />
                        <div class="row g-3">
                            <div class="col-sm-2">
                                <div class="form-group">
                                    <div class="custom-control custom-radio">
                                        <input class="custom-control-input custom-control-input-success custom-control-input-outline" type="radio" id="customRadioAgregado" name="customRadioTipo">
                                        <label for="customRadioAgregado" class="custom-control-label">AGREGADO</label>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-1">
                                <div class="form-group">
                                    <div class="custom-control custom-radio">
                                        <input class="custom-control-input custom-control-input-success custom-control-input-outline" type="radio" id="customRadioFrota" name="customRadioTipo">
                                        <label for="customRadioFrota" class="custom-control-label">FROTA</label>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-2"></div>
                            <div class="col-sm-1">
                                <div class="form-group">
                                    <div class="custom-control custom-radio">
                                        <input class="custom-control-input custom-control-input-success custom-control-input-outline" type="radio" id="customRadioCTE" name="customRadioDocumento">
                                        <label for="customRadioCTE" class="custom-control-label">CT-e/NF-e</label>
                                    </div>
                                </div>
                            </div>
                            <%--<div class="col-sm-1">
                                <div class="form-group">
                                    <div class="custom-control custom-radio">
                                        <input class="custom-control-input custom-control-input-success custom-control-input-outline" type="radio" id="customRadioCVA" name="customRadioDocumento">
                                        <label for="customRadioCVA" class="custom-control-label">C.V.A.</label>
                                    </div>
                                </div>
                            </div>--%>
                            <div class="col-sm-2">
                                <div class="form-group">
                                    <div class="custom-control custom-radio">
                                        <input class="custom-control-input custom-control-input-success custom-control-input-outline" type="radio" id="customRadioOC" name="customRadioDocumento">
                                        <label for="customRadioOC" class="custom-control-label">ORDEM DE COLETA</label>
                                    </div>
                                </div>
                            </div>

                        </div>
                        <div class="row g-3">
                            <div class="col-md-3">
                                <div class="form_group">
                                    <span class="details">FILIAL:</span>
                                    <asp:DropDownList ID="cbFiliais" name="nomeFiliais" runat="server" CssClass="form-control select2"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="cbFiliais" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
                                </div>
                            </div>
                            <div class="col-md-2">
                                <div class="form-group">
                                    <span class="direct-chat-name float-left">DOCUMENTO:</span>
                                    <asp:TextBox ID="txtDocumento" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-2">
                                <div class="form-group">
                                    <span class="direct-chat-name float-left">EMISSÃO:</span>
                                    <asp:TextBox ID="txtEmissao" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row g-3">
                            <div class="col-md-1">
                                <div class="form-group">
                                    <span class="direct-chat-name float-left">MOTORISTA:</span>
                                    <asp:TextBox ID="txtCodMot" runat="server" CssClass="form-control direct-chat-name float-left" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-5">
                                <div class="form-group">
                                    <span class="details">NOME COMPLETO:</span>
                                    <asp:TextBox ID="txtNomMot" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-2">
                                <div class="form-group">
                                    <span class="details">C.P.F.:</span>
                                    <asp:TextBox ID="txtCPF" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row g-3">
                            <div class="col-md-1">
                                <div class="form-group">
                                    <span class="direct-chat-name float-left">VEÍCULO:</span>
                                    <asp:TextBox ID="txtCodVei" runat="server" CssClass="form-control direct-chat-name float-left" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-2">
                                <div class="form-group">
                                    <span class="details">PLACA:</span>
                                    <asp:TextBox ID="txtPlaca" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-5">
                                <div class="form-group">
                                    <span class="details">DESCRIÇÃO:</span>
                                    <asp:TextBox ID="txtModelo" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row g-3">
                            <div class="col-md-1">
                                <div class="form-group">
                                    <span class="direct-chat-name float-left">PROP.:</span>
                                    <asp:TextBox ID="txtCodProp" runat="server" CssClass="form-control direct-chat-name float-left" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-5">
                                <div class="form-group">
                                    <span class="details">PROPRIETÁRIO.:</span>
                                    <asp:TextBox ID="txtTransp" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-2">
                                <div class="form-group">
                                    <span class="details">C.P.F./C.N.P.J.:</span>
                                    <asp:TextBox ID="txtCNPJ" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>

                        </div>
                        <div class="row g-3">
                            <div class="col-md-2">
                                <div class="form-group">
                                    <span class="direct-chat-name float-left">LITROS:</span>
                                    <asp:TextBox ID="txtLitros" runat="server" CssClass="form-control" MaxLength="4"></asp:TextBox>
                                </div>
                                <asp:RegularExpressionValidator
                                    ID="revNumero"
                                    runat="server"
                                    ControlToValidate="txtLitros"
                                    ValidationExpression="^\d+$"
                                    ErrorMessage="Digite apenas números"
                                    ForeColor="Red"
                                    Display="Dynamic" />

                            </div>
                            <div class="col-md-2">
                                <div class="form-group">
                                    <span class="direct-chat-name float-left">VALOR TOTAL:</span>
                                    <asp:TextBox ID="txtValorTotal" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-2">
                                <div class="form-group">
                                    <span class="direct-chat-name float-left">LIMITE DE CRÉDITO:</span>
                                    <asp:TextBox ID="txtLimiteCredito" runat="server" CssClass="form-control direct-chat-name float-left" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>
                <div class="card card-success card-outline direct-chat direct-chat-success shadow-none">
                    <div class="card-header">
                        <br />
                        <div class="row g-3">
                            <div class="col-md-2">
                                <div class="form-group">
                                    <span class="details">GERADA EM:</span>
                                    <asp:Label ID="lblDtCadastro" runat="server" CssClass="form-control" placeholder="" readonly="true"></asp:Label>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group">
                                    <span class="details">POR:</span>
                                    <asp:TextBox ID="txtUsuCadastro" runat="server" CssClass="form-control" placeholder="" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>

                            <div class="col-md-2">
                                <div class="form-group">
                                    <span class="details">REIMPRESSO EM:</span>
                                    <asp:Label ID="lblDataAlteracao" runat="server" CssClass="form-control" placeholder="" readonly="true"></asp:Label>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group">
                                    <span class="details">POR:</span>
                                    <asp:TextBox ID="txtAlteradoPor" runat="server" CssClass="form-control" placeholder="" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="card card-success card-outline direct-chat direct-chat-success shadow-none">
                    <div class="card-header">
                        <div class="row g-3">
                            <div class="col-md-1">
                                <br />
                                <asp:Button ID="btnSalvar" CssClass="btn btn-outline-success  btn-lg" runat="server" ValidationGroup="Cadastro" Text="Confirmar" />
                            </div>
                            <div class="col-md-1">
                                <br />
                                <a href="/dist/pages/GestaoPostos.aspx" class="btn btn-outline-danger btn-lg">Fechar               
                                </a>
                            </div>
                        </div>
                        <br>
                    </div>
                </div>
        </section>
    </div>
</asp:Content>
