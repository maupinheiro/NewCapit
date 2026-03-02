<%@ Page Title="" Language="C#" MasterPageFile="Main.Master" AutoEventWireup="true" CodeBehind="Frm_AltTransportadoras.aspx.cs" Inherits="NewCapit.dist.pages.Frm_AltTransportadoras" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="https://cdn.jsdelivr.net/npm/select2@4.1.0/dist/css/select2.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/select2@4.1.0/dist/js/select2.min.js"></script>

    <script>
        $(document).ready(function () {

            $('#ddlBanco').select2({
                placeholder: "Digite o nome ou código",
                minimumInputLength: 1,
                ajax: {
                    url: 'SuaPagina.aspx/BuscarBancos',
                    type: 'POST',
                    contentType: "application/json; charset=utf-8",
                    dataType: 'json',
                    delay: 250,
                    data: function (params) {
                        return JSON.stringify({ termo: params.term });
                    },
                    processResults: function (data) {
                        return {
                            results: data.d
                        };
                    }
                }
            });

            // Quando selecionar no Select2
            $('#ddlBanco').on('select2:select', function (e) {
                let codigo = e.params.data.id;
                $('#<%= txtCodigoBanco.ClientID %>').val(codigo);
        });

    });
    </script>
    <script>
        function buscarBancoPorCodigo() {

            let codigo = $('#<%= txtCodigoBanco.ClientID %>').val();

    if (codigo === "")
        return;

    $.ajax({
        type: "POST",
        url: "SuaPagina.aspx/BuscarBancos",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ termo: codigo }),
        success: function (response) {

            let dados = response.d;

            if (dados.length > 0) {

                let banco = dados[0];

                let option = new Option(banco.text, banco.id, true, true);
                $('#ddlBanco').append(option).trigger('change');
            }
            else {
                alert("Código não encontrado");
                $('#<%= txtCodigoBanco.ClientID %>').val("");
            }
        }
    });
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

            let txtCpf_Cnpj = document.getElementById("<%= txtCpf_Cnpj.ClientID %>");
            let txtFixo = document.getElementById("<%= txtFixo.ClientID %>");
            let txtCelular = document.getElementById("<%= txtCelular.ClientID %>");
            let txtCep = document.getElementById("<%= txtCepCli.ClientID %>");
            if (txtFixo) aplicarMascara(txtFixo, "(00) 0000-0000");
            if (txtFixo) aplicarMascara(txtCelular, "(00) 0 0000-0000");
            if (txtCep) aplicarMascara(txtData, "00000-000");
            if (cboPessoa.SelectValue = "JURÍDICA") {
                if (txtCpf_Cnpj) aplicarMascara(txtData, "00.000.000/0000-00");
            }
            else if (cboPessoa.SelectValue = "FÍSICA") {
                if (txtCpf_Cnpj) aplicarMascara(txtData, "000.000.000-00");
            }
        });
    </script>
    <script>
        function formatarMoeda(campo) {
            var valor = campo.value.replace(/\D/g, "");
            valor = (valor / 100).toFixed(2) + "";
            valor = valor.replace(".", ",");
            valor = valor.replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1.");
            campo.value = "R$ " + valor;
        }
    </script>
    <script>

        window.cartaoValido = false;

        function validarCartao(campo) {

            let numero = campo.value.replace(/\D/g, '');
            numero = numero.substring(0, 16);

            campo.value = numero.replace(/(\d{4})(?=\d)/g, '$1 ');

            if (numero.length === 16 && validarLuhn(numero)) {
                document.getElementById("msgCartao").innerText = "";
                window.cartaoValido = true;
            //document.getElementById("<%= btnSalvar.ClientID %>").disabled = false;
            } else {
                window.cartaoValido = false;
        ///document.getElementById("<%= btnSalvar.ClientID %>").disabled = true;
            }
        }

        function validarAoSair(campo) {

            let numero = campo.value.replace(/\D/g, '');

            if (numero.length === 0)
                return;

            if (!validarLuhn(numero) || numero.length !== 16) {

                document.getElementById("msgCartao").innerText = "Número de cartão inválido";

                campo.focus(); // volta o foco
        //document.getElementById("<%= btnSalvar.ClientID %>").disabled = true;

                return false;
            }
        }

        function validarLuhn(numero) {
            let soma = 0;
            let alternar = false;

            for (let i = numero.length - 1; i >= 0; i--) {
                let n = parseInt(numero.charAt(i));

                if (alternar) {
                    n *= 2;
                    if (n > 9) n -= 9;
                }

                soma += n;
                alternar = !alternar;
            }

            return (soma % 10 === 0);
        }

    </script>

    <div class="content-wrapper">
        <section class="content">
            <div class="container-fluid">
                <br />
                <div id="toastContainerVermelho" class="alert alert-danger alert-dismissible" style="display: none;">
                    <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                    <h5><i class="icon fas fa-exclamation-triangle"></i>Alerta!</h5>
                    Alertas
                </div>
                <div class="col-md-12">
                    <div class="card card-info">
                        <div class="card-header" style="background-color: #A020F0; font-weight: bold;">
                            <h3 class="card-title">
                                <h3 class="card-title"><i class="fas fa-shipping-fast"></i>&nbsp;ATUALIZA DADOS DO PROPRIETÁRIO/TRANSPORTADORA</h3>
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
                        <div class="card-body">
                            <div class="card-header">
                                <div class="row g-3">
                                    <div class="col-md-1">
                                        <div class="form-group">
                                            <span class="details">CÓDIGO:</span>
                                            <asp:TextBox ID="txtCodTra" runat="server" CssClass="form-control" placeholder="" MaxLength="11"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-2">
                                        <div class="form_group">
                                            <span class="details">PESSOA:</span>
                                            <asp:DropDownList ID="cboPessoa" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="OnSelectedIndexChanged">
                                                <asp:ListItem Value="" Text="Selecione..."></asp:ListItem>
                                                <asp:ListItem Value="FÍSICA" Text="FÍSICA"></asp:ListItem>
                                                <asp:ListItem Value="JURÍDICA" Text="JURÍDICA"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvcboPessoa" runat="server" ControlToValidate="cboPessoa" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
                                        </div>
                                    </div>
                                    <div class="col-md-2">
                                        <div class="form_group">
                                            <span class="details">TIPO:</span>
                                            <asp:DropDownList ID="ddlTipo" runat="server" CssClass="form-control">
                                                <asp:ListItem Value="" Text="Selecione..."></asp:ListItem>
                                                <asp:ListItem Value="AGREGADO" Text="AGREGADO"></asp:ListItem>
                                                <asp:ListItem Value="TERCEIRO" Text="TERCEIRO"></asp:ListItem>
                                                <asp:ListItem Value="EMPRESA" Text="EMPRESA"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlTipo" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form_group">
                                            <span class="details">FILIAL:</span>
                                            <asp:DropDownList ID="cbFiliais" runat="server" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>

                                <div class="row g-3">
                                    <div class="col-md-2">
                                        <div class="form-group">
                                            <span class="details">CPF/CNPJ:</span>
                                            <asp:TextBox ID="txtCpf_Cnpj" runat="server" CssClass="form-control" placeholder=""></asp:TextBox>
                                            <asp:RequiredFieldValidator runat="server" ID="rftxtCpf_Cnpj" ControlToValidate="txtCpf_Cnpj" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="col-md-1">
                                        <br />
                                        <asp:Button ID="btnCnpj" runat="server" Text="Pesquisar" CssClass="btn btn-outline-warning w-100" OnClick="btnCnpj_Click" />
                                    </div>
                                    <div class="col-md-5">
                                        <div class="form-group">
                                            <span class="details">PROPRIETÁRIO/RAZÃO SOCIAL:</span>
                                            <asp:TextBox ID="txtRazCli" runat="server" CssClass="form-control" value=""></asp:TextBox>
                                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="txtRazCli" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="col-md-2">
                                        <div class="form-group">
                                            <span class="details">ANTT/RNTRC:</span>
                                            <asp:TextBox ID="txtAntt" runat="server" CssClass="form-control" value=""></asp:TextBox>
                                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ControlToValidate="txtAntt" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="col-md-1">
                                        <div class="form-group">
                                            <span class="details">CADASTRO:</span>
                                            <asp:Label ID="txtDtCadastro" runat="server" CssClass="form-control" value=""></asp:Label>
                                        </div>
                                    </div>
                                    <div class="col-md-1">
                                        <div class="form_group">
                                            <span class="details">SITUAÇÃO:</span>
                                            <asp:DropDownList ID="ddlSituacao" runat="server" CssClass="form-control">
                                                <asp:ListItem Value="ATIVO" Text="ATIVO"></asp:ListItem>
                                                <asp:ListItem Value="INATIVO" Text="INATIVO"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="row g-3">
                                    <div class="col-md-2">
                                        <div class="form-group">
                                            <span class="details">RG/INSC. ESTADUAL:</span>
                                            <asp:TextBox ID="txtRg" runat="server" CssClass="form-control" placeholder=""></asp:TextBox>
                                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator4" ControlToValidate="txtRg" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <span class="details">NOME FANTASIA:</span>
                                            <asp:TextBox ID="txtFantasia" runat="server" CssClass="form-control" placeholder=""></asp:TextBox>
                                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator5" ControlToValidate="txtFantasia" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="col-md-2">
                                        <div class="form-group">
                                            <span class="details">CONTATO:</span>
                                            <asp:TextBox ID="txtContato" runat="server" CssClass="form-control" placeholder=""></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-2">
                                        <div class="form-group">
                                            <span class="details">FIXO:</span>
                                            <asp:TextBox ID="txtFixo" Text="" runat="server" data-mask="(00) 0000-0000" CssClass="form-control" placeholder=""></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-2">
                                        <div class="form-group">
                                            <span class="details">CELULAR:</span>
                                            <asp:TextBox ID="txtCelular" runat="server" data-mask="(00) 0 0000-0000" CssClass="form-control" placeholder=""></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row g-3">
                                    <div class="col-md-1">
                                        <div class="form-group">
                                            <span class="details">CEP:</span>
                                            <asp:TextBox ID="txtCepCli" runat="server" CssClass="form-control" placeholder="99999-999" MaxLength="9"></asp:TextBox>
                                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator6" ControlToValidate="txtCepCli" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="col-md-1">
                                        <br />
                                        <asp:Button ID="btnCep" runat="server" Text="Pesquisar" CssClass="btn btn-outline-warning w-100" OnClick="btnCep_Click" />
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <span class="details">ENDEREÇO:</span>
                                            <asp:TextBox ID="txtEndCli" runat="server" CssClass="form-control" MaxLength="60"></asp:TextBox>
                                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator7" ControlToValidate="txtEndCli" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="col-md-1">
                                        <div class="form-group">
                                            <span class="details">Nº:</span>
                                            <asp:TextBox ID="txtNumero" Style="text-align: center" runat="server" CssClass="form-control"></asp:TextBox>
                                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator8" ControlToValidate="txtNumero" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <span class="details">COMPLEMENTO:</span>
                                            <asp:TextBox ID="txtComplemento" Style="text-align: center" runat="server" CssClass="form-control" placeholder="" MaxLength="15"> </asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row g-3">
                                    <div class="col-md-5">
                                        <div class="form-group">
                                            <span class="details">BAIRRO:</span>
                                            <asp:TextBox ID="txtBaiCli" runat="server" CssClass="form-control" placeholder="" MaxLength="60"></asp:TextBox>
                                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator9" ControlToValidate="txtBaiCli" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <span class="details">MUNICIPIO:</span>
                                            <asp:TextBox ID="txtCidCli" runat="server" CssClass="form-control" placeholder="" MaxLength="60"></asp:TextBox>
                                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator10" ControlToValidate="txtCidCli" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="col-md-1">
                                        <div class="form-group">
                                            <span class="details">UF:</span>
                                            <asp:TextBox ID="txtEstCli" Style="text-align: center" runat="server" CssClass="form-control" placeholder="" MaxLength="2"></asp:TextBox>
                                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator11" ControlToValidate="txtEstCli" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </div>
                                <br />
                                <h3>Dados Bancários e Integração:</h3>
                                <hr />
                                <br />
                                <div class="form-group row">
                                    <label for="inputRemetente" class="col-sm-2 col-form-label" style="text-align: right">Integração RUBI/SAPIENS:</label>
                                    <div class="col-md-2">
                                        <asp:TextBox ID="txtCodRubi_Sapiens" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                    <label for="inputRemetente" class="col-sm-2 col-form-label" style="text-align: right">Gera CIOT:</label>
                                    <div class="col-md-2">
                                        <asp:DropDownList ID="ddlGeraCIOT" runat="server" CssClass="form-control">
                                            <asp:ListItem Value="" Text="Selecione..."></asp:ListItem>
                                            <asp:ListItem Value="Sim" Text="Sim"></asp:ListItem>
                                            <asp:ListItem Value="Não" Text="Não"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <label for="inputRemetente" class="col-sm-1 col-form-label" style="text-align: right">Valor CIOT:</label>
                                    <div class="col-md-2">
                                        <asp:TextBox ID="txtValorCIOT" runat="server" CssClass="form-control" onkeyup="formatarMoeda(this);"></asp:TextBox>
                                    </div>

                                </div>
                                <div class="row g-3">
                                    <div class="col-md-2">
                                        <div class="form_group">
                                            <span class="details">TIPO DE PAGAMENTO:</span>
                                            <asp:DropDownList ID="ddlTipoPagamento" runat="server" CssClass="form-control">
                                                <asp:ListItem Value="" Text="Selecione..."></asp:ListItem>
                                                <asp:ListItem Value="Quinzenal" Text="Quinzenal"></asp:ListItem>
                                                <asp:ListItem Value="Mensal" Text="Mensal"></asp:ListItem>
                                                <asp:ListItem Value="A Vista" Text="A Vista"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-2">
                                        <div class="form_group">
                                            <span class="details">FORMA DE PAGAMENTO:</span>
                                            <asp:DropDownList ID="ddlFormaPagamento" runat="server" CssClass="form-control">
                                                <asp:ListItem Value="" Text="Selecione..."></asp:ListItem>
                                                <asp:ListItem Value="Cartao" Text="Cartao"></asp:ListItem>
                                                <asp:ListItem Value="Deposito em Conta" Text="Deposito em Conta"></asp:ListItem>
                                                <asp:ListItem Value="PIX" Text="PIX"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-2">
                                        <div class="form_group">
                                            <span class="details">NÚMEDO DO CARTÃO:</span>
                                            <div style="position: relative;">
                                                <asp:TextBox
                                                    ID="txtCartao"
                                                    runat="server"
                                                    CssClass="form-control"
                                                    MaxLength="19"
                                                    onkeyup="validarCartao(this)"
                                                    onblur="validarAoSair(this)"
                                                    autocomplete="off">
                                                </asp:TextBox>
                                            </div>

                                            <small id="msgCartao" style="color: red;"></small>
                                        </div>
                                    </div>
                                    <div class="row">

                                        <div class="col-md-1">
                                            <label>Código</label>
                                            <asp:TextBox
                                                ID="txtCodigoBanco"
                                                runat="server"
                                                CssClass="form-control"
                                                onblur="buscarBancoPorCodigo()" />
                                        </div>

                                        <div class="col-md-6">
                                            <label>Banco</label>
                                            <select id="ddlBanco" style="width: 100%"></select>
                                        </div>

                                    </div>

                                    <br />
                                    <h3>Dados Pessoa Física:</h3>
                                    <hr />
                                    <br />


                                    <div class="row g-3">
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <span class="details">CADASTRADO EM:</span>
                                                <asp:TextBox ID="txtDtCad" Style="text-align: center" runat="server" CssClass="form-control" placeholder="" MaxLength="16" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <span class="details">CADASTRADO POR:</span>
                                                <asp:TextBox ID="txtUsuCad" Style="text-align: left" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <span class="details">ÚLTIMA ATUALIZAÇÃO::</span>
                                                <asp:TextBox ID="txtAltDtUsu" Style="text-align: center" runat="server" CssClass="form-control" placeholder="" MaxLength="16" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <span class="details">ATUALIZADO POR:</span>
                                                <asp:TextBox ID="txtUsuAltCadastro" Style="text-align: left" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row g-3">
                                        <br />
                                        <div class="col-md-2">
                                            <asp:Button ID="btnSalvar" CssClass="btn btn-outline-success btn-lg w-100" runat="server" ValidationGroup="Cadastro" OnClick="btnSalvar_Click" Text="Atualizar" />
                                        </div>
                                        <div class="col-md-2">
                                            <a href="/dist/pages/Consulta_Agregados.aspx" class="btn btn-outline-danger btn-lg w-100">Fechar               
                                            </a>
                                        </div>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>
        </section>
    </div>


    <!-- Page specific script -->
    <script>
        $(function () {
            bsCustomFileInput.init();
        });
    </script>


</asp:Content>
