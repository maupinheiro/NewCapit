<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="Frm_CadFornecedores.aspx.cs" Inherits="NewCapit.dist.pages.Frm_CadFornecedores" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery.mask/1.14.16/jquery.mask.min.js"></script>
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
            let txtCNPJ = document.getElementById("<%= txtCnpj.ClientID %>");
            let txtCep = document.getElementById("<%= txtCepFor.ClientID %>");
            let txtTc1For = document.getElementById("<%= txtTc1For.ClientID %>");
            let txtTc2For = document.getElementById("<%= txtTc2For.ClientID %>");

            if (txtCNPJ) aplicarMascara(txtCNPJ, "00.000.000/0000-00");
            if (txtCep) aplicarMascara(txtCep, "00000-000");
            if (txtTc1For) aplicarMascara(txtTc1For, "(00) 0000-0000");
            if (txtTc2For) aplicarMascara(txtTc2For, "(00) 0 0000-0000");
        });
    </script>
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
                        <h3 class="card-title"><i class="fas fa-users"></i>&nbsp;FORNECEDORES - NOVO CADASTRO</h3>
                    </div>
                </div>
                <div class="card-header">
                    <div class="row g-3">
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="details">CÓDIGO:</span>
                                <asp:TextBox ID="txtCodFor" runat="server" CssClass="form-control" MaxLength="10" OnTextChanged="btnFornecedor_Click" AutoPostBack="true"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form-group">
                                <span class="details">CNPJ:</span>
                                <asp:TextBox ID="txtCnpj" runat="server" class="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="rfvtxtCnpj" ControlToValidate="txtCnpj" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <br />
                            <asp:Button ID="btnCnpj" runat="server" Text="Pesquisar" CssClass="btn btn-outline-warning" OnClick="btnCnpj_Click" />
                        </div>
                        <div class="col-md-3"></div>
                        <div class="col-md-2">
                            <div class="form-group">
                                <span class="details">TIPO DE EMPRESA:</span>
                                <asp:TextBox ID="txtTipo" runat="server" CssClass="form-control" MaxLength="30"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="rfvtxtTipo" ControlToValidate="txtTipo" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="details">ABERTURA:</span>
                                <asp:TextBox ID="txtAbertura" runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="rfvtxtAbertura" ControlToValidate="txtAbertura" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form-group">
                                <span class="details">SITUAÇÃO NA RFB:</span>
                                <asp:TextBox ID="txtSituacao" runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="rvftxtSituacao" ControlToValidate="txtSituacao" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                    <div class="row g-3">
                        <div class="col-md-5">
                            <div class="form-group">
                                <span class="details">RAZÃO SOCIAL:</span>
                                <asp:TextBox ID="txtRazFor" runat="server" CssClass="form-control" MaxLength="50"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="rvftxtRazFor" ControlToValidate="txtRazFor" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="col-md-5">
                            <div class="form-group">
                                <span class="details">NOME FANTASIA:</span>
                                <asp:TextBox ID="txtNomFor" runat="server" CssClass="form-control" placeholder="" MaxLength="50"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="rvftxtNomFor" ControlToValidate="txtNomFor" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form-group">
                                <span class="">TIPO DE FORNECEDOR:</span>
                                <asp:DropDownList ID="ddlTipoFornecedor" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="POSTO COMBUSTÍVEL" Text="POSTO COMBUSTÍVEL"></asp:ListItem>
                                    <asp:ListItem Value="COMBUSTÍVEL" Text="COMBUSTÍVEL"></asp:ListItem>
                                    <asp:ListItem Value="OFICINA EXTERNA" Text="OFICINA EXTERNA"></asp:ListItem>
                                    <asp:ListItem Value="PEÇAS" Text="PEÇAS"></asp:ListItem>
                                    <asp:ListItem Value="LAVAGEM VEÍCULO" Text="LAVAGEM VEÍCULO"></asp:ListItem>
                                    <asp:ListItem Value="TERCEIRIZADA" Text="TERCEIRIZADA"></asp:ListItem>
                                    <asp:ListItem Value="OUTROS" Text="OUTROS"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                       <%-- <div class="col-md-1">
                            <div class="form-group">
                                <span class="">POSTO:</span>
                                <asp:DropDownList ID="ddlTipoPosto" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="EXTERNO" Text="EXTERNO"></asp:ListItem>
                                    <asp:ListItem Value="INTERNO" Text="INTERNO"></asp:ListItem> 
                                </asp:DropDownList>
                            </div>
                        </div>--%>

                    </div>
                    <div class="row g-3">
                        <div class="col-md-2">
                            <div class="form-group">
                                <span class="details">INSC. ESTADUAL:</span>
                                <asp:TextBox ID="txtInscEstadual" runat="server" CssClass="form-control" placeholder="" MaxLength="15"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="rfvtxtInscEstadual" ControlToValidate="txtInscEstadual" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form-group">
                                <span class="details">INSC. C.C.M.:</span>
                                <asp:TextBox ID="txtInscCCM" runat="server" CssClass="form-control" placeholder="" MaxLength="30"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form-group">
                                <span class="details">PAÍS:</span>
                                <asp:DropDownList ID="ddlPaises" runat="server" AutoPostBack="True" class="form-control select2">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form-group">
                                <span class="details">CONTATO:</span>
                                <asp:TextBox ID="txtConFor" runat="server" CssClass="form-control" placeholder="" MaxLength="30"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form-group">
                                <span class="details">FONE FIXO:</span>
                                <asp:TextBox ID="txtTc1For" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form-group">
                                <span class="details">CELULAR:</span>
                                <asp:TextBox ID="txtTc2For" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="row g-3">
                        <div class="col-md-6">
                            <div class="form-group">
                                <span class="details">E-MAIL(S):</span>
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="" MaxLength="200"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <span class="details">SITE:</span>
                                <asp:TextBox ID="txtSite" runat="server" CssClass="form-control" placeholder="" MaxLength="200"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="row g-3">
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="details">CEP:</span>
                                <asp:TextBox ID="txtCepFor" runat="server" CssClass="form-control" Width="130px" placeholder="99999-999" MaxLength="9"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="rfvtxtCepFor" ControlToValidate="txtCepFor" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <br />
                            <asp:Button ID="btnCep" runat="server" Text="Pesquisar" CssClass="btn btn-outline-warning" OnClick="btnCep_Click" UseSubmitBehavior="false" />
                        </div>
                    </div>
                    <div class="row g-3">
                        <div class="col-md-9">
                            <div class="form-group">
                                <span class="details">ENDEREÇO:</span>
                                <asp:TextBox ID="txtEndFor" runat="server" CssClass="form-control" MaxLength="60"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="rfvtxtEndFor" ControlToValidate="txtEndFor" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="details">Nº:</span>
                                <asp:TextBox ID="txtNumero" Style="text-align: center" runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="rfvtxtNumero" ControlToValidate="txtNumero" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form-group">
                                <span class="details">COMPLEMENTO:</span>
                                <asp:TextBox ID="txtComplemento" Style="text-align: center" runat="server" CssClass="form-control" placeholder="" MaxLength="15"> </asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="row g-3">
                        <div class="col-md-6">
                            <div class="form-group">
                                <span class="details">BAIRRO:</span>
                                <asp:TextBox ID="txtBaiFor" runat="server" CssClass="form-control" placeholder="" MaxLength="60"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="rfvtxtBaiFor" ControlToValidate="txtBaiFor" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="col-md-5">
                            <div class="form-group">
                                <span class="details">MUNICIPIO:</span>
                                <asp:TextBox ID="txtCidFor" runat="server" CssClass="form-control" placeholder="" MaxLength="60"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="rfvtxtCidFor" ControlToValidate="txtCidFor" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="details">UF:</span>
                                <asp:TextBox ID="txtEstFor" Style="text-align: center" runat="server" CssClass="form-control" placeholder="" MaxLength="2"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="rfvtxtEstFor" ControlToValidate="txtEstFor" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>

                    <div class="row g-3">
                        <div class="col-md-2">
                            <div class="form-group">
                                <span class="details">CADASTRADO EM:</span>
                                <asp:Label ID="lblDtCadastro" runat="server" CssClass="form-control" placeholder="" readonly="true"></asp:Label>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <span class="details">POR:</span>
                                <asp:TextBox ID="txtUsuCadastro" runat="server" CssClass="form-control" placeholder="" ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="row g-3">
                        <div class="col-md-1">
                            <br />
                            <asp:Button ID="btnSalvar" CssClass="btn btn-outline-success  btn-lg" runat="server" ValidationGroup="Cadastro" OnClick="btnSalvar_Click" Text="Cadastrar" />
                        </div>
                        <div class="col-md-1">
                            <br />
                            <a href="/dist/pages/ConsultaFornecedores.aspx" class="btn btn-outline-danger btn-lg">Fechar               
                            </a>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>
</asp:Content>
