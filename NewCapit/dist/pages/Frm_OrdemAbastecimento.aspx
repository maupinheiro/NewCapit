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

    <script type="text/javascript">
        function calcularTotal() {
            var litros = parseFloat(document.getElementById('<%= txtLitros.ClientID %>').value.replace(',', '.')) || 0;
            var preco = parseFloat(document.getElementById('<%= txtPreco.ClientID %>').value.replace(',', '.')) || 0;
            var limite = parseFloat(document.getElementById('<%= txtLimiteCredito.ClientID %>').value.replace(',', '.')) || 0;

            var total = litros * preco;

            document.getElementById('<%= txtValorTotal.ClientID %>').value = total.toFixed(2).replace('.', ',');

            if (total > limite) {
                Mensagem("danger", "Valor total excede o limite de crédito!");
                document.getElementById('<%= txtValorTotal.ClientID %>').value = "";
            }
        }
    </script>
    <div class="content-wrapper">
        <section class="content">
            <div class="container-fluid">
                <br />
                <div id="divMsg" runat="server"
                    class="alert alert-dismissible fade show mt-3"
                    role="alert" visible="false">
                    <asp:Label ID="lblMsgGeral" runat="server"></asp:Label>
                    <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
                </div>
                <div class="card card-success">
                    <div class="card-header">
                        <h3 class="card-title"><i class="fas fa-gas-pump"></i>&nbsp;Gestão de Postos
                         <br />
                            <small>Gerar Ordem de Abastecimento</small></h3>
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
                                    <asp:TextBox ID="txtCodFor" runat="server" CssClass="form-control direct-chat-name float-left font-weight-bold" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-5">
                                <div class="form-group">
                                    <span class="details">POSTO:</span>
                                    <asp:TextBox ID="txtNomFor" runat="server" CssClass="form-control font-weight-bold" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="form-group">
                                    <span class="direct-chat-name float-left">PRODUTO:</span>
                                    <asp:DropDownList ID="ddlCombustivel" runat="server" CssClass="form-control font-weight-bold" AutoPostBack="true" OnSelectedIndexChanged="ddlCombustivel_SelectedIndexChanged"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvCombustivel" runat="server"
                                        ControlToValidate="ddlCombustivel"
                                        InitialValue=""
                                        ErrorMessage="Selecione o combustível."
                                        ForeColor="Red"
                                        ValidationGroup="Cadastro"
                                        Display="Dynamic">*</asp:RequiredFieldValidator>

                                </div>
                            </div>
                            <div class="col-md-2">
                                <div class="form-group">
                                    <span class="direct-chat-name float-left">PREÇO UNIT.:</span>
                                    <asp:TextBox ID="txtPreco" runat="server" CssClass="form-control font-weight-bold" ReadOnly="true"></asp:TextBox>
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
                                <div class="form-check">
                                    <asp:RadioButton ID="customRadioAgregado"
                                        runat="server"
                                        GroupName="tipoMotorista"
                                        AutoPostBack="true"
                                        OnCheckedChanged="customRadioAgregado_CheckedChanged" />
                                    <label class="form-check-label">AGREGADO</label>
                                </div>
                            </div>
                            <div class="col-sm-1">
                                <div class="form-check">
                                    <asp:RadioButton ID="customRadioFrota"
                                        runat="server"
                                        GroupName="tipoMotorista"
                                        AutoPostBack="true"
                                        OnCheckedChanged="customRadioFrota_CheckedChanged" />
                                    <label class="form-check-label">FROTA</label>
                                </div>
                            </div>
                            <div class="col-sm-2"></div>
                            <div class="col-sm-1">
                                <div class="form-check">
                                    <asp:RadioButton ID="customRadioCTe"
                                        runat="server"
                                        GroupName="tipoDocumento"
                                        AutoPostBack="true"
                                        OnCheckedChanged="customRadioCTe_CheckedChanged" />
                                    <label class="form-check-label">CT-e</label>
                                </div>
                            </div>
                            <div class="col-sm-1">
                                <div class="form-check">
                                    <asp:RadioButton ID="customRadioNFSe"
                                        runat="server"
                                        GroupName="tipoDocumento"
                                        AutoPostBack="true"
                                        OnCheckedChanged="customRadioNFSe_CheckedChanged" />
                                    <label class="form-check-label">NFS-e</label>
                                </div>
                            </div>

                        </div>
                        <div class="row g-3">
                            <div class="col-md-1">
                                <div class="form-group">
                                    <span class="direct-chat-name float-left">DOCUMENTO:</span>
                                    <asp:TextBox ID="txtDocumento"
                                        runat="server"
                                        CssClass="form-control font-weight-bold"
                                        ReadOnly="true"
                                        AutoPostBack="true"
                                        OnTextChanged="txtDocumento_TextChanged" />
                                </div>
                            </div>
                            <div class="col-md-2">
                                <div class="form-group">
                                    <span class="direct-chat-name float-left">EMISSÃO:</span>
                                    <asp:TextBox ID="txtEmissao" runat="server" CssClass="form-control font-weight-bold" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-3" id="divFilial" runat="server" visible="true">
                                <div class="form_group">
                                    <span class="details">FILIAL:</span>
                                    <asp:TextBox ID="txtFilial" runat="server" CssClass="form-control font-weight-bold" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row g-3">
                            <div class="col-md-1">
                                <div class="form-group">
                                    <span class="direct-chat-name float-left">MOTORISTA:</span>
                                    <asp:TextBox ID="txtCodMot" runat="server" CssClass="form-control direct-chat-name float-left font-weight-bold" ReadOnly="true" OnTextChanged="txtCodMot_TextChanged" AutoPostBack="true"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-5">
                                <div class="form-group">
                                    <span class="details">NOME COMPLETO:</span>
                                    <asp:TextBox ID="txtNomMot" runat="server" CssClass="form-control font-weight-bold" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-2">
                                <div class="form-group">
                                    <span class="details">C.P.F.:</span>
                                    <asp:TextBox ID="txtCPF" runat="server" CssClass="form-control font-weight-bold" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row g-3">
                            <div class="col-md-1">
                                <div class="form-group">
                                    <span class="direct-chat-name float-left">VEÍCULO:</span>
                                    <asp:TextBox ID="txtCodVei" runat="server" CssClass="form-control direct-chat-name float-left font-weight-bold" ReadOnly="true" OnTextChanged="txtCodVei_TextChanged" AutoPostBack="true"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-1">
                                <div class="form-group">
                                    <span class="details">PLACA:</span>
                                    <asp:TextBox ID="txtPlaca" runat="server" CssClass="form-control font-weight-bold" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group">
                                    <span class="details">DESCRIÇÃO:</span>
                                    <asp:TextBox ID="txtModelo" runat="server" CssClass="form-control font-weight-bold" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-2">
                                <div class="form-group">
                                    <span class="details">&emsp;</span><br />
                                    <asp:TextBox ID="txtFilialVeic" runat="server" CssClass="form-control font-weight-bold" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row g-3">
                            <div class="col-md-1">
                                <div class="form-group">
                                    <span class="direct-chat-name float-left">PROP.:</span>
                                    <asp:TextBox ID="txtCodProp" runat="server" CssClass="form-control direct-chat-name float-left font-weight-bold" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-5">
                                <div class="form-group">
                                    <span class="details">PROPRIETÁRIO.:</span>
                                    <asp:TextBox ID="txtTransp" runat="server" CssClass="form-control font-weight-bold" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-2">
                                <div class="form-group">
                                    <span class="details">C.P.F./C.N.P.J.:</span>
                                    <asp:TextBox ID="txtCNPJ" runat="server" CssClass="form-control font-weight-bold" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>

                        </div>
                        <div class="row g-3">
                            <div class="col-md-1">
                                <div class="form-group">
                                    <span class="direct-chat-name float-left">LITROS:</span>
                                    <asp:TextBox ID="txtLitros"
                                        runat="server"
                                        CssClass="form-control font-weight-bold"
                                        MaxLength="4"
                                        onkeyup="calcularTotal()"
                                        AutoPostBack="true"
                                        OnTextChanged="txtLitros_TextChanged"></asp:TextBox>
                                </div>
                                <asp:RequiredFieldValidator ID="rfvLitros" runat="server"
                                    ControlToValidate="txtLitros"
                                    ErrorMessage="Informe a quantidade de litros."
                                    ForeColor="Red"
                                    ValidationGroup="Cadastro"
                                    Display="Dynamic">*</asp:RequiredFieldValidator>

                            </div>
                            <div class="col-md-1">
                                <div class="form-group">
                                    <span class="direct-chat-name float-left">VALOR TOTAL:</span>
                                    <asp:TextBox ID="txtValorTotal" runat="server" CssClass="form-control font-weight-bold" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-1">
                                <div class="form-group">
                                    <span class="direct-chat-name float-left">LIMITE DE CRÉDITO:</span>
                                    <asp:TextBox ID="txtLimiteCredito" runat="server" CssClass="form-control direct-chat-name float-left font-weight-bold" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>
                <div class="card card-success card-outline direct-chat direct-chat-success shadow-none">
                    <div class="card-header">
                        <br />
                        <div class="row g-3">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <span class="details">GERADA POR:</span>
                                    <asp:TextBox ID="txtUsuCadastro" runat="server" CssClass="form-control" placeholder="" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <span class="details">REIMPRESSA POR:</span>
                                    <asp:TextBox ID="txtAlteradoPor" runat="server" CssClass="form-control" placeholder="" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="card card-success card-outline direct-chat direct-chat-success shadow-none">
                    <div class="card-header">
                        <div class="row g-3">
                            <div class="col-md-2">
                                <br />
                                <asp:Button ID="btnSalvar" CssClass="btn btn-outline-success btn-lg w-100"
                                    runat="server" Text="Confirmar"
                                    OnClick="btnSalvar_Click" />

                            </div>
                            <%--//OnClientClick="this.disabled=true; this.value='Gerando...';"--%>
                            <div class="col-md-2">
                                <br />
                                <a href="/dist/pages/ControleAbastecimento.aspx" class="btn btn-outline-danger btn-lg w-100">Fechar               
                                </a>
                            </div>
                        </div>
                        <br>
                    </div>
                </div>

            </div>


        </section>
    </div>
</asp:Content>
