<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="ColetasMatriz.aspx.cs" Inherits="NewCapit.dist.pages.ColetasMatriz" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery.mask/1.14.16/jquery.mask.min.js"></script>

    <!-- Bootstrap CSS + JS -->
    <script src="https://code.jquery.com/jquery-3.5.1.slim.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@4.5.2/dist/js/bootstrap.bundle.min.js"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/html2pdf.js/0.9.2/html2pdf.bundle.min.js"></script>


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



    <%-- <script type="text/javascript">
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
    </script>--%>

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
                          
                                <div class="container mt-4">

                                    <div class="input-group mb-3">
                                        <asp:TextBox ID="txtCarga" CssClass="form-control" runat="server" placeholder="Digite o número da carga"></asp:TextBox>
                                        <button class="btn btn-primary" type="button" onclick="buscarCarga()">Buscar</button>
                                    </div>

                                    <!-- Spinner -->
                                    <div id="spinner" class="text-center my-3" style="display: none;">
                                        <div class="spinner-border text-primary"></div>
                                        <p>Carregando...</p>
                                    </div>

                                    <!-- Collapse CARGA (ABERTO AUTOMATICAMENTE) -->
                                    <div class="accordion-item mb-3">
                                        <h2 class="accordion-header" id="headingCarga">
                                            <button class="accordion-button" type="button" data-bs-toggle="collapse"
                                                data-bs-target="#collapseCarga" aria-expanded="true">
                                                Dados da Carga
                                            </button>
                                        </h2>
                                        <div id="collapseCarga" class="accordion-collapse collapse show" data-bs-parent="#accordionExample">
                                            <div class="accordion-body">

                                                <!-- MAPA -->
                                                <div id="map" style="width: 100%; height: 300px;" class="mb-3 border"></div>

                                                <div class="row mb-3">
                                                    <div class="col-md-4">
                                                        <asp:Label ID="lblMotorista" CssClass="fw-bold" runat="server" Text="Motorista: -"></asp:Label>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:Label ID="lblStatus" CssClass="fw-bold" runat="server" Text="Status: -"></asp:Label>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:Label ID="lblVeiculo" CssClass="fw-bold" runat="server" Text="Veículo: -"></asp:Label>
                                                    </div>
                                                </div>

                                                <!-- TELEMETRIA -->
                                                <div class="alert alert-info">
                                                    <asp:Label ID="lblTelemetria" runat="server" Text="Telemetria: -"></asp:Label>
                                                </div>

                                                <!-- GRID CARGA -->
                                                <asp:GridView ID="gvCarga" CssClass="table table-striped table-bordered"
                                                    AutoGenerateColumns="true" runat="server">
                                                </asp:GridView>

                                            </div>
                                        </div>
                                    </div>

                                    <!-- Collapse PEDIDOS (FECHADO) -->
                                    <div class="accordion-item">
                                        <h2 class="accordion-header" id="headingPedidos">
                                            <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse"
                                                data-bs-target="#collapsePedidos">
                                                Pedidos da Carga
                                            </button>
                                        </h2>

                                        <div id="collapsePedidos" class="accordion-collapse collapse">
                                            <div class="accordion-body">

                                                <asp:Label ID="lblTotalPedidos" CssClass="fw-bold mb-2" runat="server" Text=""></asp:Label><br />
                                                <asp:Label ID="lblSomaValores" CssClass="fw-bold text-success mb-3" runat="server" Text=""></asp:Label>

                                                <asp:GridView ID="gvPedidos" CssClass="table table-hover table-bordered"
                                                    AutoGenerateColumns="true" runat="server">
                                                </asp:GridView>

                                            </div>
                                        </div>
                                    </div>

                                    <!-- Botão enviar rota para WhatsApp -->
                                    <button runat="server" id="btnWhats" class="btn btn-success mt-3" onserverclick="btnWhats_ServerClick">
                                        Enviar rota para WhatsApp
                                    </button>
                                </div>

                                <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>

                                <script>
                                    function buscarCarga() {
                                        document.getElementById("spinner").style.display = "block";
                                        __doPostBack('<%= btnBuscar.ClientID %>', '');
                                    }
                                </script>

                                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" Style="display: none;" OnClick="btnBuscar_Click" />
                           

                        </div>
                    </div>
                </div>

            </div>
            <!-- Modal Bootstrap Cadastro de Telefone -->
            <%--<div class="modal fade" id="telefoneModal" tabindex="-1" role="dialog" aria-labelledby="telefoneModalLabel" aria-hidden="true">
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
                            <asp:Button ID="btnCadContato" runat="server" Text="Salvar" class="btn btn-primary" />
                        </div>
                    </div>
                </div>
            </div>--%>
            <!-- Modal Bootstrap Incluir Coleta Vazia -->
            <%--<div class="modal fade" id="meuModal" tabindex="-1" role="dialog" aria-labelledby="meuModalLabel" aria-hidden="true">
                <div class="modal-dialog modal-dialog-centered modal-xl" role="document">
                    <div class="modal-content">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <div class="modal-header">
                                    <h5 class="modal-title" id="meuModalLabel">Inclusão de Viagem Veículo Vazio</h5>
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
                                                <asp:DropDownList ID="ddlCliInicial" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCliInicial_TextChanged" class="form-control select2"></asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <span class="details">VIAGEM:</span>
                                                <asp:TextBox ID="novaCarga" runat="server" Style="text-align: center" class="form-control font-weight-bold" ReadOnly="true" placeholder=""></asp:TextBox>

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
                                                <asp:DropDownList ID="ddlCliFinal" runat="server" AutoPostBack="True" class="form-control select2" OnSelectedIndexChanged="ddlCliFinal_TextChanged"></asp:DropDownList>
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
                                    <asp:Button ID="btnSalvarColeta" runat="server" Text="Salvar" class="btn btn-primary" />
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnSalvarColeta" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>


                    </div>
                </div>
            </div>--%>
        </section>
    </div>

    </span>
</asp:Content>
