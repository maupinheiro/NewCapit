<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="Frm_EditarColetaMatriz.aspx.cs" Inherits="NewCapit.dist.pages.Frm_EditarColetaMatriz" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery.mask/1.14.16/jquery.mask.min.js"></script>

    <!-- Bootstrap CSS + JS -->
    <script src="https://code.jquery.com/jquery-3.5.1.slim.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@4.5.2/dist/js/bootstrap.bundle.min.js"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">

    <script type="text/javascript">
        function abrirModalTelefone() {
            // Pega valor do TextBox do Web Forms            
            var codigoFrota = document.getElementById('<%= txtCodFrota.ClientID %>').value;

            // Define o valor no TextBox do modal
            document.getElementById('<%= txtCodFrota.ClientID %>').value = codigoFrota;

            //$('#telefoneModal').modal('show');
            $('#telefoneModal').modal({ backdrop: 'static', keyboard: false });
        }
    </script>
    <style>
        hr {
            height: 10px; /* Define a espessura da linha em 5 pixels */
            background-color: #45aab8; /* Define a cor da linha como preta */
            border: none; /* Remove a borda padrão do navegador */
            margin: 1px 0; /* Adiciona margem acima e abaixo da linha */
        }
    </style>
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
            let txtCadCelular = document.getElementById("<%= txtCadCelular.ClientID %>");

            if (txtJanOrigem) aplicarMascara(txtJanOrigem, "00/00/0000 00:00");
            if (txtJanDestino) aplicarMascara(txtJanDestino, "00/00/0000 00:00");
            if (txtSaida) aplicarMascara(txtSaida, "00/00/0000 00:00");
            if (txtPrevisaoChegada) aplicarMascara(txtPrevisaoChegada, "00/00/0000 00:00");
            if (txtChegada) aplicarMascara(txtChegada, "00/00/0000 00:00");
            if (txtConclusao) aplicarMascara(txtConclusao, "00/00/0000 00:00");
            if (txtCPF) aplicarMascara(txtCPF, "000.000.000-00");
            if (txtCadCelular) aplicarMascara(txtCadCelular, "(00) 0 0000-0000");

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
                                        <%--<img src="<%=fotoMotorista%>" class="rounded-circle float-center" width="60px" alt="" />--%>
                                        
                                    </span>
                                    <div class="info-box-content">
                                        <span class="info-box-number">
                                            <div class="row g-3">
                                                <div class="col-md-1">
                                                    <div class="form-group">
                                                        <span class="details">CÓDIGO:</span>
                                                        <asp:TextBox ID="txtCodMotorista" Style="text-align: center" runat="server" CssClass="form-control font-weight-bold" AutoPostBack="true" OnTextChanged="txtCodMotorista_TextChanged"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvtxtCodMot" runat="server" ControlToValidate="txtCodMotorista" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
                                                    </div>
                                                </div>
                                                <div class="col-md-4">
                                                    <div class="form-group">
                                                        <span class="details">NOME DO MOTORISTA:</span>
                                                        <asp:DropDownList ID="ddlMotorista" runat="server" CssClass="form-control select2"></asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="col-md-1">
                                                    <div class="form-group">
                                                        <span class="details">CONTATO:</span>
                                                        <asp:TextBox ID="txtCodFrota" runat="server" class="form-control font-weight-bold" AutoPostBack="true" OnTextChanged="txtCodFrota_TextChanged"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-2">
                                                    <div class="form-group">
                                                        <span class="details">FONE CORPORATIVO:</span>
                                                        <asp:TextBox ID="txtFoneCorp" runat="server" class="form-control font-weight-bold" ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-1">
                                                    <div class="form-group">
                                                        <span class="details">CÓD./FROTA:</span>
                                                        <asp:TextBox ID="txtCodVeiculo" runat="server" Style="text-align: center" class="form-control font-weight-bold" AutoPostBack="true" OnTextChanged="txtCodVeiculo_TextChanged"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-1">
                                                    <div class="form-group">
                                                        <span class="details">PLACA:</span>
                                                        <asp:TextBox ID="txtPlaca" runat="server" class="form-control font-weight-bold" ReadOnly="true" MaxLength="8"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-1" id="reboque1" runat="server" visible="false">
                                                    <div class="form-group">
                                                        <span class="details">REBOQUE:</span>
                                                        <asp:TextBox ID="txtReboque1" runat="server" class="form-control font-weight-bold" ReadOnly="true" MaxLength="8"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-1" id="reboque2" runat="server" visible="false">
                                                    <div class="form-group">
                                                        <span class="details">REBOQUE:</span>
                                                        <asp:TextBox ID="txtReboque2" runat="server" class="form-control font-weight-bold" ReadOnly="true" MaxLength="8"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-12">
                                <div class="card card-outline card-info">
                                    <div class="card-header">
                                        <h3 class="card-title"><i class="far fa-edit"></i>&nbsp;Dados do Motorista/Veículo</h3>
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
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <span class="details">FILIAL:</span>
                                                    <asp:TextBox ID="txtFilialMot" runat="server" class="form-control font-weight-bold" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <span class="details">TIPO DE MOTORISTA:</span>
                                                    <asp:TextBox ID="txtTipoMot" runat="server" class="form-control font-weight-bold" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <span class="details">FUNÇÃO:</span>
                                                    <asp:TextBox ID="txtFuncao" runat="server" class="form-control font-weight-bold" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-2" id="ETI" runat="server">
                                                <div class="form-group">
                                                    <span class="details">VALIDADE E.T.I.:</span>
                                                    <div class="input-group">
                                                        <asp:TextBox ID="txtExameToxic" runat="server" class="form-control font-weight-bold" ReadOnly="true" Style="text-align: center"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <span class="details">VALIDADE CNH:</span>
                                                    <div class="input-group">
                                                        <asp:TextBox ID="txtCNH" runat="server" class="form-control font-weight-bold" ReadOnly="true" Style="text-align: center"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <span class="details">VALIDADE GR.:</span>
                                                    <div class="input-group">
                                                        <asp:TextBox ID="txtLibGR" runat="server" class="form-control font-weight-bold" ReadOnly="true" Style="text-align: center"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row g-3">
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <span class="details">CELULAR PARTICULAR:</span>
                                                    <div class="input-group">
                                                        <asp:TextBox ID="txtCelular" runat="server" class="form-control font-weight-bold" ReadOnly="true" Style="text-align: center"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <span class="details">CPF:</span>
                                                    <div class="input-group">
                                                        <asp:TextBox ID="txtCPF" runat="server" class="form-control font-weight-bold" ReadOnly="true" Style="text-align: center"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <span class="details">CARTÃO PAMCARD:</span>
                                                    <div class="input-group">
                                                        <asp:TextBox ID="txtCartao" runat="server" class="form-control font-weight-bold" ReadOnly="true" Style="text-align: center"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-1">
                                                <div class="form-group">
                                                    <span class="details">MÊS/ANO:</span>
                                                    <div class="input-group">
                                                        <asp:TextBox ID="txtValCartao" runat="server" class="form-control font-weight-bold" ReadOnly="true" Style="text-align: center"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-1">
                                                <div class="form-group">
                                                    <span class="details">CÓDIGO:</span>
                                                    <div class="input-group">
                                                        <asp:TextBox ID="txtCodTransportadora" runat="server" class="form-control font-weight-bold" ReadOnly="true" Style="text-align: center"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <span class="details">TRANSPORTADORA:</span>
                                                    <div class="input-group">
                                                        <asp:TextBox ID="txtTransportadora" runat="server" class="form-control font-weight-bold" ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>



                                        </div>
                                        <div>
                                            <hr />
                                        </div>
                                        <div class="row g-3">
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <span class="details">TIPO DE VEÍCULO:</span>
                                                    <asp:TextBox ID="txtVeiculoTipo" runat="server" Style="text-align: center" class="form-control font-weight-bold" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <span class="details">VEICULO:</span>
                                                    <asp:TextBox ID="txtTipoVeiculo" runat="server" class="form-control font-weight-bold" ReadOnly="true" placeholder=""></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-2" id="carretas" runat="server">
                                                <div class="form-group">
                                                    <span class="details">CARRETA(S):</span>
                                                    <asp:TextBox ID="txtCarreta" runat="server" class="form-control font-weight-bold" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <span class="details">CONJUNTO:</span>
                                                    <asp:TextBox ID="txtConjunto" runat="server" class="form-control font-weight-bold" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row g-3">
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <span class="details">VALIDADE OPACIDADE:</span>
                                                    <div class="input-group">
                                                        <asp:TextBox ID="txtOpacidade" runat="server" class="form-control font-weight-bold" ReadOnly="true" Style="text-align: center"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-2" id="valCET" runat="server">
                                                <div class="form-group">
                                                    <span class="details">VALIDADE LIC. CET:</span>
                                                    <div class="input-group">
                                                        <asp:TextBox ID="txtCET" runat="server" class="form-control font-weight-bold" ReadOnly="true" Style="text-align: center"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <span class="details">LICENCIMAENTO:</span>
                                                    <asp:TextBox ID="txtCRLVVeiculo" runat="server" class="form-control font-weight-bold" ReadOnly="true" Style="text-align: center"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-2" id="reb1" runat="server" visible="false">
                                                <div class="form-group">
                                                    <span class="details">VALIDADE REBOQUE:</span>
                                                    <div class="input-group">
                                                        <asp:TextBox ID="txtCRLVReb1" runat="server" class="form-control font-weight-bold" ReadOnly="true" Style="text-align: center"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-2" id="reb2" runat="server" visible="false">
                                                <div class="form-group">
                                                    <span class="details">VALIDADE REBOQUE:</span>
                                                    <div class="input-group">
                                                        <asp:TextBox ID="txtCRLVReb2" runat="server" class="form-control font-weight-bold" ReadOnly="true" Style="text-align: center"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-2" id="crono" runat="server">
                                                <div class="form-group">
                                                    <span class="details">VAL. CRONOTACOGRAFO:</span>
                                                    <div class="input-group">
                                                        <asp:TextBox ID="txtCrono" runat="server" class="form-control font-weight-bold" ReadOnly="true" Style="text-align: center"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row g-3">
                                            <div class="col-md-1">
                                                <div class="form-group">
                                                    <span class="details">CÓDIGO:</span>
                                                    <asp:TextBox ID="txtCodProprietario" runat="server" class="form-control font-weight-bold" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-5">
                                                <div class="form-group">
                                                    <span class="details">PROPRIETÁRIO:</span>
                                                    <asp:TextBox ID="txtProprietario" runat="server" class="form-control font-weight-bold" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-1"></div>
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <span class="details">TECNOLOGIA:</span>
                                                    <asp:TextBox ID="txtTecnologia" runat="server" class="form-control font-weight-bold" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <span class="details">RASTREAMENTO:</span>
                                                    <asp:TextBox ID="txtRastreamento" runat="server" class="form-control font-weight-bold" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
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
                                                    <asp:DropDownList ID="cboStatus" runat="server" CssClass="form-control" ReadOnly="true"></asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-md-5">
                                                <div class="form-group">
                                                    <span class="details">SITUAÇÃO:</span>
                                                    <asp:TextBox ID="txtSituacao" Style="text-align: center" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-1">
                                                <div class="form-group">
                                                    <span class="details">FERROLENE:</span>
                                                    <asp:TextBox ID="txtContFerrolene" Style="text-align: center" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
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
                                                    <asp:DropDownList ID="cboMaterial" runat="server" CssClass="form-control"></asp:DropDownList>
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
                                            <div class="col-md-1">
                                                <div class="form-group">
                                                    <span class="details">O.T. CLIENTE:</span>
                                                    <asp:TextBox ID="txtControleCli" Style="text-align: center" ReadOnly="true" runat="server" CssClass="form-control"></asp:TextBox>
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
                                                            <a class="nav-link" id="custom-tabs-four-pedidos-tab" data-toggle="pill" href="#custom-tabs-four-pedidos" role="tab" aria-controls="custom-tabs-four-pedidos" aria-selected="false">Pedidos</a>
                                                        </li>
                                                        <li class="nav-item">
                                                            <a class="nav-link" id="custom-tabs-four-documentos-tab" data-toggle="pill" href="#custom-tabs-four-documentos" role="tab" aria-controls="custom-tabs-four-documentos" aria-selected="false">CTe/RPS/MDFe</a>
                                                        </li>
                                                        <li class="nav-item">
                                                            <a class="nav-link" id="custom-tabs-four-notasfiscais-tab" data-toggle="pill" href="#custom-tabs-four-messages" role="tab" aria-controls="custom-tabs-four-notasfiscais" aria-selected="false">Notas Fiscais</a>
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
                                                        <div class="tab-pane fade show active" id="custom-tabs-four-atendimento" role="tabpanel" aria-labelledby="custom-tabs-four-atendimento-tab">
                                                            <div class="row g-3">
                                                                <div class="col-md-4">
                                                                    <div class="form-group">
                                                                        <span class="details">ADD. DOC.:</span>
                                                                        <asp:TextBox ID="txtDocumentos" Style="text-align: center" onkeypress="return apenasNumeros(event);" runat="server" MaxLength="44" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtDocumentos_TextChanged"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                                <div class="col-sm-1">
                                                                    <br />
                                                                    <div class="form-check">
                                                                        <input class="form-check-input" type="radio" name="opcao" id="rbCTe" value="1" runat="server">
                                                                        <label class="form-check-label" for="rboCTe">
                                                                            CT-e
                                                                        </label>
                                                                    </div>
                                                                </div>
                                                                <div class="col-sm-1">
                                                                    <br />
                                                                    <div class="form-check">
                                                                        <input class="form-check-input" type="radio" name="opcao" id="rbMDFe" value="2" runat="server">
                                                                        <label class="form-check-label" for="rbMDFe">
                                                                            MDF-e
                                                                        </label>
                                                                    </div>
                                                                </div>
                                                                <div class="col-sm-1">
                                                                    <br />
                                                                    <div class="form-check">
                                                                        <input class="form-check-input" type="radio" name="opcao" id="rbRPS" value="3" runat="server">
                                                                        <label class="form-check-label" for="rbRPS">
                                                                            RPS-e
                                                                        </label>
                                                                    </div>
                                                                </div>
                                                                <div class="col-sm-2">
                                                                    <br />
                                                                    <div class="form-check">
                                                                        <input class="form-check-input" type="radio" name="opcao" id="rbNFe" value="4" runat="server">
                                                                        <label class="form-check-label" for="rbNFe">
                                                                            Nota Fiscal
                                                                        </label>
                                                                    </div>
                                                                </div>

                                                            </div>
                                                            <div class="row g-3">
                                                                <div class="col-md-1">
                                                                    <div class="form-group">
                                                                        <span class="details">EMBARQUE:</span>
                                                                        <asp:TextBox ID="txtEmbarqueNum" Style="text-align: center" runat="server" CssClass="form-control"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-1">
                                                                    <div class="form-group">
                                                                        <span class="details">MDF-e/Série:</span>
                                                                        <asp:TextBox ID="txtMDFeNum" Style="text-align: center" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-1">
                                                                    <div class="form-group">
                                                                        <span class="details">EMISSÃO:</span>
                                                                        <asp:TextBox ID="txtEmisaoMDFe" Style="text-align: center" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-4">
                                                                    <div class="form-group">
                                                                        <span class="details">CHAVE DE ACESSO DO MDFe:</span>
                                                                        <asp:TextBox ID="txtChaveAcessoMDFe" Style="text-align: center" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-1">
                                                                    <div class="form-group">
                                                                        <span class="details">SITUAÇÃO:</span>
                                                                        <asp:TextBox ID="txtSituacaoMDFe" Style="text-align: center" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-1" id="botaoMDFe">
                                                                    <br />
                                                                    <button type="button" class="btn btn-info">Encerrar</button>
                                                                </div>
                                                                <div class="col-md-3">
                                                                    <div class="form-group">
                                                                        <span class="details">RESPONSÁVEL:</span>
                                                                        <asp:TextBox ID="txtResponsavelBaixaMDFe" Style="text-align: left" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
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
                                                        <!-- PEDIDOS -->
                                                        <div class="tab-pane fade" id="custom-tabs-four-pedidos" role="tabpanel" aria-labelledby="custom-tabs-four-pedidos-tab">                                                            
                                                            <div class="card-body table-responsive p-0" style="height: 150px;">
                                                                <table class="table table-head-fixed text-nowrap">
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
                                                        <!-- DOCUMENTOS CTE/RPS -->
                                                        <div class="tab-pane fade" id="custom-tabs-four-documentos" role="tabpanel" aria-labelledby="custom-tabs-four-pedidos-tab">

                                                            <div class="card-body table-responsive p-0" style="height: 150px;">
                                                                <table class="table table-head-fixed text-nowrap">
                                                                    <asp:GridView ID="GridView1" runat="server" CssClass="table table-striped"
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
                                                        <!-- NOTAS FISCAIS -->
                                                        <div class="tab-pane fade" id="custom-tabs-four-messages" role="tabpanel" aria-labelledby="custom-tabs-four-messages-tab">
                                                            <div class="card-body table-responsive p-0" style="height: 150px;">
                                                                <table class="table table-head-fixed text-nowrap">
                                                                    <asp:GridView ID="GridView2" runat="server" CssClass="table table-striped"
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
                                                        <!-- DADOS PEDÁGIO -->
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
                                                        <!-- DADOS DA SM KRONA -->
                                                        <div class="tab-pane fade" id="custom-tabs-four-mensagem" role="tabpanel" aria-labelledby="custom-tabs-four-mensagem-tab">
                                                            <div class="row g-3">
                                                                <div class="col-md-12">
                                                                    <div class="col-md-6" id="tipoSolicitacao">
                                                                        <div class="form-group">
                                                                            <span class="details">TIPO DE SOLICITAÇÃO:</span>
                                                                            <asp:TextBox ID="txtTipoSolicitacao" Style="text-align: center" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-6" id="tipoVeiculo">
                                                                        <div class="form-group">
                                                                            <span class="details">TIPO DE VEÍCULO:</span>
                                                                            <asp:TextBox ID="txtTipoDeVeiculo" Style="text-align: center" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                                                        </div>
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
                                    <asp:Button ID="btnSalvar" CssClass="btn btn-outline-success  btn-lg" runat="server" Text="Cadastrar" OnClick="btnSalvar_Click" />
                                </div>
                                <div class="col-md-1">
                                    <a href="/dist/pages/ConsultaMotoristas.aspx" class="btn btn-outline-danger btn-lg">Sair               
                                    </a>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>

            </div>
            <!-- Modal Bootstrap Cadastro de Telefone -->
            <div class="modal fade" id="telefoneModal" tabindex="-1" role="dialog" aria-labelledby="telefoneModalLabel" aria-hidden="true">
                <div class="modal-dialog modal-dialog-centered" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title" id="telefoneModalLabel">Cadastrar Contato</h5>
                            <button type="button" class="close" data-dismiss="modal" aria-label="Fechar">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div class="modal-body">
                            <div class="row g-3">
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <span class="details">CÓDIGO:</span>
                                        <asp:TextBox ID="txtCodContato" runat="server" class="form-control font-weight-bold" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-8">
                                    <div class="form-group">
                                        <span class="details">CELULAR:</span>
                                        <div class="input-group">
                                            <asp:TextBox ID="txtCadCelular" runat="server" CssClass="form-control font-weight-bold"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Fechar</button>
                            <asp:Button ID="btnCadContato" runat="server" Text="Salvar" class="btn btn-primary" OnClick="btnCadContato_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>

</asp:Content>