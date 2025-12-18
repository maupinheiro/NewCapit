<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="ColetasMatriz.aspx.cs" Inherits="NewCapit.dist.pages.ColetasMatriz" EnableEventValidation="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery.mask/1.14.16/jquery.mask.min.js"></script>

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/js/bootstrap.bundle.min.js"></script>

    <!-- Bootstrap CSS + JS -->
    <%--  <script src="https://code.jquery.com/jquery-3.5.1.slim.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@4.5.2/dist/js/bootstrap.bundle.min.js"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/html2pdf.js/0.9.2/html2pdf.bundle.min.js"></script>--%>


    <style>
        /* Estilo extra para visual mais moderno */
        .card-custom {
            border-radius: .75rem;
            box-shadow: 0 4px 12px rgba(0,0,0,.08);
        }

        .accordion-button:not(.collapsed) {
            color: #0d6efd;
        }

        .small-table td, .small-table th {
            padding: .35rem .5rem;
            font-size: .9rem;
        }

        .export-btn {
            margin-left: .5rem;
        }
    </style>
    <script type="text/javascript">
        function abrirModalTelefone() {
            // Pega valor do TextBox do Web Forms            
            var codigoFrota = document.getElementById('<%= txtCodFrota.ClientID %>').value;

            // Define o valor no TextBox do modal
            document.getElementById('<%= txtCodContato.ClientID %>').value = codigoFrota;

            //$('#telefoneModal').modal('show');
            $('#telefoneModal').modal({ backdrop: 'static', keyboard: false });
        }
    </script>
    <script type="text/javascript">
        function abrirModal() {
            //$('#meuModal').modal('show');
            $('#meuModal').modal({ backdrop: 'static', keyboard: false });
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
    <script type="text/javascript">
        // Função para re-inicializar componentes Bootstrap
        function reInitializeBootstrap() {
            // Re-inicializa todos os alertas para que o botão de fechar funcione
            $('.alert').alert();

            // Re-inicializa modais (se necessário)
            $('.modal').modal({ show: false });
        }

        // Obtém a instância do PageRequestManager
        var prm = Sys.WebForms.PageRequestManager.getInstance();

        if (prm) {
            // Adiciona um manipulador de eventos para o final de cada postback assíncrono
            prm.add_endRequest(function (sender, args) {
                reInitializeBootstrap();
            });
        }

        // Executa a função também no carregamento inicial da página
        $(document).ready(function () {
            reInitializeBootstrap();
        });
    </script>


    <div class="content-wrapper">
        <section class="content">
            <asp:UpdatePanel ID="UpdatePanel3"  runat="server">
    <ContentTemplate>
            <div class="container-fluid">
                <br />
                <!-- ALERTA BOOTSTRAP -->
                
                <div id="divMsg" runat="server"
                    class="alert alert-warning alert-dismissible fade show mt-3"
                    role="alert" style="display: none;">
                    <span id="lblMsg" runat="server"></span>
                    <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
                </div>
                <div id="divMsgCNH" runat="server"
                    class="alert alert-warning alert-dismissible fade show mt-3"
                    role="alert" style="display: none;">
                    <span id="lblMsgCNH" runat="server"></span>
                    <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
                </div>
                <div id="divMsgGR" runat="server"
                    class="alert alert-warning alert-dismissible fade show mt-3"
                    role="alert" style="display: none;">
                    <span id="lblMsgGR" runat="server"></span>
                    <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
                </div>

                <div id="divMsgVeic" runat="server"
                    class="alert alert-warning alert-dismissible fade show mt-3"
                    role="alert" style="display: none;">
                    <span id="lblMsgVeic" runat="server"></span>
                    <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
                </div>
                <div id="divMsgCET" runat="server"
                    class="alert alert-warning alert-dismissible fade show mt-3"
                    role="alert" style="display: none;">
                    <span id="lblMsgCET" runat="server"></span>
                    <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
                </div>
                <div id="divMsgLinc" runat="server"
                    class="alert alert-warning alert-dismissible fade show mt-3"
                    role="alert" style="display: none;">
                    <span id="lblMsgLinc" runat="server"></span>
                    <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
                </div>
                <div id="divMsgCrono" runat="server"
                    class="alert alert-warning alert-dismissible fade show mt-3"
                    role="alert" style="display: none;">
                    <span id="lblMsgCrono" runat="server"></span>
                    <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
                </div>
                <div id="divMsgCarreta1" runat="server"
                    class="alert alert-warning alert-dismissible fade show mt-3"
                    role="alert" style="display: none;">
                    <span id="lblMsgCarreta1" runat="server"></span>
                    <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
                </div>
                <div id="divMsgCarreta2" runat="server"
                    class="alert alert-warning alert-dismissible fade show mt-3"
                    role="alert" style="display: none;">
                    <span id="lblMsgCarreta2" runat="server"></span>
                    <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
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
                        <div class="col-xl-12 col-md-12 mb-12">
                            <br />
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
                                                    <asp:TextBox ID="txtCodMotorista" Style="text-align: center" runat="server" CssClass="form-control font-weight-bold" OnTextChanged="btnBuscarMotorista_Click" AutoPostBack="true"></asp:TextBox>

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
                                                    <asp:TextBox ID="txtCodFrota" runat="server" class="form-control font-weight-bold" AutoPostBack="true" OnTextChanged="btnPesquisarContato_Click"></asp:TextBox>
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
                                                    <span class="details">FROTA:</span>
                                                    <asp:TextBox ID="txtCodVeiculo" runat="server" Style="text-align: center" class="form-control font-weight-bold"></asp:TextBox>
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
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <span class="details">CPF:</span>
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtCPF" runat="server" class="form-control font-weight-bold" ReadOnly="true" Style="text-align: center"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <span class="details">CARTÃO PAMCARD:</span>
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtNumCartao" runat="server" class="form-control font-weight-bold" ReadOnly="true" Style="text-align: center"></asp:TextBox>
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
                                    </div>
                                    <!-- colunas invisiveis -->
                                    <div class="row g-3" id="validarMotorista" runat="server" visible="false">
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
                                                    <asp:TextBox ID="txtValCNH" runat="server" class="form-control font-weight-bold" ReadOnly="true" Style="text-align: center"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <span class="details">VALIDADE GR.:</span>
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtValGR" runat="server" class="form-control font-weight-bold" ReadOnly="true" Style="text-align: center"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!-- fim das colunas invisiveis -->
                                    <div class="row g-3">
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <span class="details">CELULAR PARTICULAR:</span>
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtCelularParticular" runat="server" class="form-control font-weight-bold" ReadOnly="true" Style="text-align: center"></asp:TextBox>
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
                                        <div class="col-md-5">
                                            <div class="form-group">
                                                <span class="details">TRANSPORTADORA:</span>
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtTransportadora" runat="server" class="form-control font-weight-bold" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <%-- Dados do veículo --%>
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
                                    <!-- colunas invisiveis -->
                                    <div class="row g-3" id="validarVeiculo" runat="server" visible="false">
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
                                    <!-- fim das colunas invisiveis -->
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
                        <!-- /.Cargas e Pedidos -->
                        <div class="card-body">
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
                                            <asp:TextBox ID="txtCarga" Style="text-align: center" onkeypress="return apenasNumeros(event);" runat="server" CssClass="form-control" OnTextChanged="btnAdd_Click" AutoPostBack="true"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <hr />
                            <h4>Carga(s):</h4>
                            <asp:GridView ID="gvCargas" runat="server" AutoGenerateColumns="False"
                                CssClass="table table-bordered"
                                DataKeyNames="carga"
                                OnRowCommand="gvCargas_RowCommand">

                                <Columns>
                                    <asp:TemplateField HeaderText="AÇÃO" ShowHeader="True">
                                        <ItemTemplate>

                                            <asp:LinkButton ID="lnkRemover" runat="server" CommandName="Excluir"
                                                CommandArgument='<%# Container.DataItemIndex %>' CssClass="btn btn-danger btn-sm"><i class="far fa-trash-alt"></i> Remover</asp:LinkButton>


                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="carga" HeaderText="CARGA" />
                                    <asp:BoundField DataField="previsao" HeaderText="PREVISÃO" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField DataField="cliorigem" HeaderText="ORIGEM" />
                                    <asp:BoundField DataField="cidorigem" HeaderText="MUNICIPIO" />
                                    <asp:BoundField DataField="ufcliorigem" HeaderText="UF" />
                                    <asp:BoundField DataField="clidestino" HeaderText="DESTINO" />
                                    <asp:BoundField DataField="ciddestino" HeaderText="MUNICIPIO" />
                                    <asp:BoundField DataField="ufclidestino" HeaderText="UF" />

                                    <asp:ButtonField CommandName="detalhes"
                                        ButtonType="Button" Text="Pedido(s)" ControlStyle-CssClass="btn btn-info btn-sm" />
                                </Columns>
                            </asp:GridView>
                            <hr />
                            <h4>Pedido(s) da carga: &nbsp;<asp:Label ID="lblCargaSel" runat="server"></asp:Label></h4>
                            <asp:GridView ID="gvPedidos" runat="server" AutoGenerateColumns="false" CssClass="table table-striped">
                                <Columns>

                                    <asp:BoundField DataField="carga" HeaderText="Carga" Visible="false" />
                                    <asp:BoundField DataField="pedido" HeaderText="PEDIDO" />
                                    <asp:BoundField DataField="emissao" HeaderText="EMISSÃO" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                                    <asp:BoundField DataField="material" HeaderText="MATERIAL" />
                                    <asp:BoundField DataField="peso" HeaderText="PESO" />
                                    <asp:BoundField DataField="portao" HeaderText="LOCAL" />
                                    <asp:BoundField DataField="situacao" HeaderText="SITUAÇÃO" />
                                    <asp:BoundField DataField="solicitante" HeaderText="SOLICITANTE" />
                                    <asp:BoundField DataField="entrega" HeaderText="ENTREGA" />
                                </Columns>
                            </asp:GridView>
                        </div>

                        <div class="col-md-12">
                            <div class="row g-3">
                                <div class="col-md-1">
                                    <asp:Button ID="btnSalvar" CssClass="btn btn-outline-success  btn-lg" runat="server" Text="Cadastrar" />
                                </div>
                                <div class="col-md-1">
                                    <a href="/dist/pages/GestaoDeEntregasMatriz.aspx" class="btn btn-outline-danger btn-lg">Fechar               
                                    </a>
                                </div>
                            </div>
                        </div>
                        <br />
                    </div>
                         
                </div>

            </div>
            </ContentTemplate>               
                <Triggers>
                    <asp:PostBackTrigger ControlID="txtCodMotorista" />
                    <asp:PostBackTrigger ControlID="txtCodFrota" />
                    <asp:PostBackTrigger ControlID="txtCarga" />
                </Triggers>
                </asp:UpdatePanel>

            <!-- Modal Bootstrap Cadastro de Telefone -->
            <div class="modal fade" id="telefoneModal" tabindex="-1" role="dialog" aria-labelledby="telefoneModalLabel" aria-hidden="true">
                <div class="modal-dialog modal-dialog-centered" role="document">
                    <div class="modal-content">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
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
                                        <asp:TextBox ID="txtCodContato" runat="server" class="form-control font-weight-bold"></asp:TextBox>
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
                                </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btnCadContato" />
                            </Triggers>
                            </asp:UpdatePanel>
                    </div>
                </div>
            </div>
            <!-- Modal Bootstrap Incluir Coleta Vazia -->
            <div class="modal fade" id="meuModal" tabindex="-1" role="dialog" aria-labelledby="meuModalLabel" aria-hidden="true"> 
                <div class="modal-dialog modal-dialog-centered modal-xl" role="document">
                    <div class="modal-content">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <div class="modal-header">
                                    <h5 class="modal-title" id="meuModalLabel">Inclusão de Viagem</h5>
                                    <button type="button" class="close" data-dismiss="modal" aria-label="Fechar">
                                        <span aria-hidden="true">&times;</span>
                                    </button>
                                </div>
                                <div class="modal-body">
                                    <div class="row g-3">
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <span class="details">CÓDIGO:</span>
                                                <asp:TextBox ID="codCliInicial" runat="server" class="form-control" AutoPostBack="true" OnTextChanged="codCliInicial_TextChanged"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-8">
                                            <div class="form-group">
                                                <span class="details">ORIGEM:</span>
                                                <asp:DropDownList ID="ddlCliInicial" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCliInicial_TextChanged" CssClass="form-control select2"></asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <span class="details">VIAGEM:</span>
                                                <asp:TextBox ID="novaCargaVazia" runat="server" Style="text-align: center" class="form-control font-weight-bold" ReadOnly="true" placeholder=""></asp:TextBox>

                                            </div>
                                        </div>
                                    </div>
                                    <!-- colunas ocultas -->
                                    <div class="row g-3">
                                        <div class="col-md-10">
                                            <div class="form-group">
                                                <asp:TextBox ID="txtMunicipioOrigem" runat="server" Style="text-align: center" class="form-control font-weight-bold" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <asp:TextBox ID="txtUfOrigem" runat="server" Style="text-align: center" class="form-control font-weight-bold" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <!-- fim das colunas ocultas -->
                                    <div class="row g-3">
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <span class="details">CÓDIGO:</span>
                                                <asp:TextBox ID="codCliFinal" runat="server" class="form-control" AutoPostBack="true" OnTextChanged="codCliFinal_TextChanged"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-8">
                                            <div class="form-group">
                                                <span class="details">DESTINO:</span>
                                                <asp:DropDownList ID="ddlCliFinal" runat="server" AutoPostBack="True" CssClass="form-control select2" OnSelectedIndexChanged="ddlCliFinal_TextChanged"></asp:DropDownList>
                                                <asp:Label ID="lblDistancia" runat="server" Text="" ForeColor="Red" Font-Size="XX-Small"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <span class="details">PERCURSO:</span>
                                                <asp:TextBox ID="txtDistancia" runat="server" ReadOnly="true" Style="text-align: center" class="form-control font-weight-bold" placeholder=""></asp:TextBox>
                                            </div>
                                        </div>

                                    </div>
                                    <!-- colunas ocultas -->
                                    <div class="row g-3">
                                        <div class="col-md-10">
                                            <div class="form-group">
                                                <asp:TextBox ID="txtMunicipioDestino" runat="server" Style="text-align: center" class="form-control font-weight-bold" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <asp:TextBox ID="txtUfDestino" runat="server" Style="text-align: center" class="form-control font-weight-bold" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <!-- fim das colunas ocultas -->
                                </div>
                                <div class="modal-footer">
                                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Fechar</button>
                                    <asp:Button ID="btnSalvarColeta" runat="server" Text="Salvar" class="btn btn-primary" OnClick="btnSalvarColeta_Click" />
                                </div>
                            </ContentTemplate>
                            <Triggers>
                               <asp:PostBackTrigger ControlID="btnSalvarColeta" />
                            </Triggers>
                        </asp:UpdatePanel>


                    </div>
                </div>
            </div>
        </section>
    </div>
</asp:Content>
