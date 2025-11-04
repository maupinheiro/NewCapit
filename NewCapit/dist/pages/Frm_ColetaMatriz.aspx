<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="Frm_ColetaMatriz.aspx.cs" Inherits="NewCapit.dist.pages.Frm_ColetaMatriz" %>

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

            let txtCPF = document.getElementById("<%= txtCPF.ClientID %>");
            let txtCadCelular = document.getElementById("<%= txtCadCelular.ClientID %>");

            if (txtCPF) aplicarMascara(txtCPF, "000.000.000-00");
            if (txtCadCelular) aplicarMascara(txtCadCelular, "(00) 0 0000-0000");

        });
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
                                                    <span class="details">LICENCIAMENTO:</span>
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
                                                    <span class="details">VAL. CRONO:</span>
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
                                                    <span class="details">MONITORAMENTO:</span>
                                                    <asp:TextBox ID="txtRastreamento" runat="server" class="form-control font-weight-bold" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                </div>
                            </div>
                            <div class="col-md-12">
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
                                </div>
                            </div>
                            <div class="row g-3">
                                <div class="col-md-12">
                                    <div class="card">
                                        <!-- ./card-header -->
                                        <div class="card-body">
                                            <asp:Label ID="lblMensagem" runat="server" Text=""></asp:Label>
                                            <asp:UpdatePanel ID="updColetas" runat="server">
                                                <ContentTemplate>
                                                    <asp:Repeater ID="rptColetas" runat="server" OnItemDataBound="rptColetas_ItemDataBound" OnItemCommand="rptColetas_ItemCommand">
                                                        <HeaderTemplate>
                                                            <table class="table table-bordered table-hover">
                                                                <thead>
                                                                    <tr>
                                                                        <th>CARGA</th>
                                                                        <th>REMETENTE</th>
                                                                        <th>PRESTAÇÃO DO SERVIÇO</th>
                                                                        <th>DESTINATÁRIO</th>
                                                                        <th>TERMINO DO SERVIÇO</th>
                                                                        <th>PESO</th>
                                                                        <th>AÇÕES</th>
                                                                    </tr>
                                                                </thead>
                                                                <tbody>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <tr data-widget="expandable-table" aria-expanded="false">
                                                                <td>
                                                                    <asp:Label ID="lblCarga" runat="server" Text='<%# Eval("carga") %>' /></td>
                                                                <td><%# Eval("cliorigem") %></td>
                                                                <td><%# Eval("cidcliorigem") %></td>
                                                                <td><%# Eval("clidestino") %></td>
                                                                <td><%# Eval("cidclidestino") %></td>
                                                                <td><%# Eval("peso") %></td>

                                                                <td>
                                                                    <asp:Button ID="btnRemoverColeta" runat="server" Text="Remover" CssClass="btn btn-outline-danger" CommandName="Remover" CommandArgument='<%# Eval("carga") %>' />
                                                                </td>
                                                            </tr>
                                                            <tr class="expandable-body">
                                                                <td colspan="12">
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
                                                                                <div class="col-md-2" id="DivPercurso" runat="server">
                                                                                    <div class="form-group">
                                                                                        <span class="details">PERCURSO:</span>
                                                                                        <div class="input-group">
                                                                                            <asp:TextBox ID="txtPercurso" runat="server" class="form-control font-weight-bold" ReadOnly="true" Style="text-align: center"></asp:TextBox>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="col-md-1" runat="server">
                                                                                    <div class="form-group">
                                                                                        <span class="details">DISTÂNCIA:</span>
                                                                                        <div class="input-group">
                                                                                            <asp:TextBox ID="txtDistancia" runat="server" class="form-control font-weight-bold" ReadOnly="true" Style="text-align: center"></asp:TextBox>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="col-md-1" runat="server">
                                                                                    <div class="form-group">
                                                                                        <span class="details">T.TIME:</span>
                                                                                        <div class="input-group">
                                                                                            <asp:TextBox ID="txtDuracao" runat="server" class="form-control font-weight-bold" ReadOnly="true" Style="text-align: center"></asp:TextBox>
                                                                                        </div>
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
                                                                                <div class="col-md-1">
                                                                                    <div class="form-group">
                                                                                        <span class="details">PEDAGIO:</span>
                                                                                        <asp:TextBox ID="txtPedagio" Style="text-align: center" ReadOnly="true" runat="server" CssClass="form-control"></asp:TextBox>
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
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            </tbody>
                    </table>
                                                        </FooterTemplate>
                                                    </asp:Repeater>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="rptColetas" EventName="ItemCommand" />
                                                </Triggers>
                                            </asp:UpdatePanel>


                                        </div>
                                        <!-- /.card-body -->
                                    </div>
                                </div>
                            </div>





                            <%-- <div class="col-md-12">
                                                           </div>--%>

                            <%--<div class="col-md-12">
                                <div class="card card-outline card-info">
                                    <div class="card-header">
                                        <h3 class="card-title"><i class="far fa-edit"></i>&nbsp;Pedido(s)</h3>
                                        <div class="card-tools">
                                            <button type="button" class="btn btn-tool" data-card-widget="collapse">
                                                <i class="fas fa-minus"></i>
                                            </button>
                                        </div>                                          
                                    </div>
                                    <div class="card-body">
                                        <!-- PEDIDOS -->
                                        <div class="card-body table-responsive p-0" style="height: 140px;">
                                            <table class="table table-head-fixed text-nowrap table-hover">
                                                <asp:GridView ID="gvPedidos" runat="server" CssClass="table table-striped"
                                                    AutoGenerateColumns="False"
                                                    DataKeyNames="carga"
                                                    OnRowEditing="gvPedidos_RowEditing"
                                                    OnRowUpdating="gvPedidos_RowUpdating"
                                                    OnRowCancelingEdit="gvPedidos_RowCancelingEdit">
                                                    <Columns>
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
                                </div>
                            </div>--%>

                            <div class="col-md-12">
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
                            </div>
                            <div class="col-md-12">
                                <div class="row g-3">
                                    <div class="col-md-1">
                                        <asp:Button ID="btnSalvar" CssClass="btn btn-outline-success  btn-lg" runat="server" Text="Cadastrar" OnClick="btnSalvar_Click" />
                                    </div>
                                    <div class="col-md-1">
                                        <a href="/dist/pages/ConsultaMotoristas.aspx" class="btn btn-outline-danger btn-lg">Fechar               
                                        </a>
                                    </div>
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

    </span>
</asp:Content>
