<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="FreteMinimoANTT.aspx.cs" Inherits="NewCapit.dist.pages.FreteMinimoANTT" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        /* Campo com erro */
        .campo-erro {
            border: 2px solid #dc3545 !important;
            border-radius: 6px;
            animation: shake 0.3s;
        }

        /* Campo válido */
        .campo-ok {
            border: 2px solid #198754 !important; /* Verde */
            border-radius: 6px;
        }

        /* Mensagem de erro */
        .msg-erro {
            color: #dc3545;
            font-size: 13px;
            margin-top: 3px;
        }

        /* Animação de erro (shake) */
        @keyframes shake {
            0% {
                transform: translateX(0);
            }

            25% {
                transform: translateX(-4px);
            }

            50% {
                transform: translateX(4px);
            }

            75% {
                transform: translateX(-4px);
            }

            100% {
                transform: translateX(0);
            }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- Bootstrap e jQuery -->
  <%--  <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>--%>

    <!-- Plugin Bootstrap Multiselect -->
    <%--<link href="https://cdn.jsdelivr.net/npm/bootstrap4-multiselect/dist/css/bootstrap-multiselect.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap4-multiselect/dist/js/bootstrap-multiselect.min.js"></script>--%>


    <script>
        function somenteNumeros(e) {
            var charCode = e.which || e.keyCode;

            // Permitir: backspace, delete, tab, setas
            if (charCode == 8 || charCode == 9 || charCode == 46 || (charCode >= 37 && charCode <= 40)) {
                return true;
            }

            // Permitir apenas números (48–57)
            if (charCode < 48 || charCode > 57) {
                return false;
            }
            return true;
        }
    </script>
    <%--<script>

        function showToast(msg) {
            document.getElementById("toastMsg").innerHTML = msg;
            var toast = new bootstrap.Toast(document.getElementById("toastErro"));
            toast.show();
        }

        function validarCampos() {
            var erro = false;

            var campos = [
                { id: "<%= ddlTipoCarga.ClientID %>", nome: "Informe um tipo de carga." },
                { id: "<%= ddlEixos.ClientID %>", nome: "Informe a quantidade de eixos." },
                { id: "<%= txtDistancia.ClientID %>", nome: "Informe a distância a ser percorrida." }
            ];

            campos.forEach(function (c) {
                var valor = document.getElementById(c.id).value.trim();
                if (valor === "") {
                    showToast("O campo <b>" + c.nome + "</b> está vazio!");
                    erro = true;
                }
            });

            return !erro;  // Se tiver erro, cancela o postback
        }

    </script>--%>

    <div class="content-wrapper">
        <section class="content">
            <div class="container-fluid">
                <br />
                <div class="col-md-12">
                    <div class="card card-info">
                        <div class="card-header">
                            <h3 class="card-title">
                                <h3 class="card-title"><i class="fas fa-chart-line"></i>&nbsp;Frete Mínimo ANTT</h3>
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
                            <h3>
                                <center><span style="color: blue;">Calcular Piso Mínimo de Frete</span></center>
                            </h3>
                            <div class="row g-3">
                                <div class="col-md-6">
                                    <div class="card card-default">
                                        <div class="card-header">
                                            <h3 class="card-title">
                                                <i class="fas fa-comments-dollar"></i>
                                                Dados do Frete
                                            </h3>
                                        </div>
                                        <!-- /.card-header -->
                                        <div class="card-body">
                                            <div class="callout callout-info">

                                                <div class="row g-3">
                                                    <div class="col-md-12">
                                                        <div class="form-group">
                                                            <span class="details">Tipo de Carga:</span>
                                                            <asp:DropDownList ID="ddlTipoCarga" runat="server" CssClass="form-control" onkeyup="limparErro('<%= ddlTipoCarga.ClientID %>')">
                                                                <asp:ListItem Value="" Text="Selecione..."></asp:ListItem>
                                                                <asp:ListItem Value="Granel sólido" Text="Granel sólido"></asp:ListItem>
                                                                <asp:ListItem Value="Granel líquido" Text="Granel líquido"></asp:ListItem>
                                                                <asp:ListItem Value="Frigorificada ou Aquecida" Text="Frigorificada ou Aquecida"></asp:ListItem>
                                                                <asp:ListItem Value="Conteinerizada" Text="Conteinerizada"></asp:ListItem>
                                                                <asp:ListItem Value="Carga Geral" Text="Carga Geral"></asp:ListItem>
                                                                <asp:ListItem Value="Neogranel" Text="Neogranel"></asp:ListItem>
                                                                <asp:ListItem Value="Perigosa (granel sólido)" Text="Perigosa (granel sólido)"></asp:ListItem>
                                                                <asp:ListItem Value="Perigosa (granel líquido)" Text="Perigosa (granel líquido)"></asp:ListItem>
                                                                <asp:ListItem Value="Perigosa (frigorificada ou aquecida)" Text="Perigosa (frigorificada ou aquecida)"></asp:ListItem>
                                                                <asp:ListItem Value="Perigosa (conteinerizada)" Text="Perigosa (conteinerizada)"></asp:ListItem>
                                                                <asp:ListItem Value="Perigosa (carga geral)" Text="Perigosa (carga geral)"></asp:ListItem>
                                                                <asp:ListItem Value="Carga Granel Pressurizada" Text="Carga Granel Pressurizada"></asp:ListItem>
                                                            </asp:DropDownList>
                                                            <span id="msg_<%= ddlTipoCarga.ClientID %>" class="msg-erro"></span>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row g-3">
                                                    <div class="col-md-6">
                                                        <div class="form-group">
                                                            <span class="details">Número de Eixos:</span>
                                                            <asp:DropDownList ID="ddlEixos" runat="server" CssClass="form-control" onkeyup="limparErro('<%= ddlEixos.ClientID %>')">
                                                                <asp:ListItem Value="" Text="Selecione..."></asp:ListItem>
                                                                <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                                                <asp:ListItem Value="3" Text="3"></asp:ListItem>
                                                                <asp:ListItem Value="4" Text="4"></asp:ListItem>
                                                                <asp:ListItem Value="5" Text="5"></asp:ListItem>
                                                                <asp:ListItem Value="6" Text="6"></asp:ListItem>
                                                                <asp:ListItem Value="7" Text="7"></asp:ListItem>
                                                                <asp:ListItem Value="9" Text="9"></asp:ListItem>
                                                            </asp:DropDownList>
                                                            <span id="msg_<%= ddlEixos.ClientID %>" class="msg-erro"></span>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <div class="form-group">
                                                            <span class="details">Distância:</span>
                                                            <asp:TextBox ID="txtDistancia" runat="server" CssClass="form-control"
                                                                onkeyup="validarInstantaneo('<%= txtDistancia.ClientID %>', 'Valor 1')"
                                                                onblur="validarInstantaneo('<%= txtDistancia.ClientID %>', 'Valor 1')"></asp:TextBox>
                                                            <span id="msg_<%= txtDistancia.ClientID %>" class="msg-erro"></span>

                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row g-3">
                                                    <div class="col-md-10">
                                                        <div class="form-group">
                                                            <span class="details">É composição veicular?
                                                                <br />
                                                                <small>(veículo automotor + implemento ou caminhão simples)</small>
                                                            </span>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <div class="form-group" style="text-align: right;">
                                                            <span class="details">&nbsp;</span><br />
                                                            <input type="checkbox" name="my-checkbox" data-bootstrap-switch data-off-text="Não" data-off-color="danger" data-on-text="Sim" data-on-color="success">
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row g-3">
                                                    <div class="col-md-10">
                                                        <div class="form-group">
                                                            <span class="details">É Alto Desempenho?</span>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <div class="form-group" style="text-align: right;">
                                                            <span class="details">&nbsp;</span>
                                                            <input type="checkbox" name="my-checkbox" data-bootstrap-switch data-off-text="Não" data-off-color="danger" data-on-text="Sim" data-on-color="success">
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row g-3">
                                                    <div class="col-md-10">
                                                        <div class="form-group">
                                                            <span class="details">Retorno Vazio?</span>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <div class="form-group" style="text-align: right;">
                                                            <span class="details">&nbsp;</span>
                                                            <input type="checkbox" name="my-checkbox" data-bootstrap-switch data-off-text="Não" data-off-color="danger" data-on-text="Sim" data-on-color="success">
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row g-3">
                                                    <div class="col-md-10">
                                                        <div class="form-group">
                                                            <span class="details">&nbsp;</span>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <div class="form-group">
                                                            <asp:Button ID="btnCalcular" runat="server" CssClass="btn btn-outline-info float-right"
                                                                Text="Calcular" OnClientClick="return validarCampos();" />
                                                        </div>
                                                    </div>
                                                </div>

                                            </div>
                                        </div>
                                        <!-- /.card-body -->
                                    </div>
                                    <!-- /.card -->
                                </div>

                                <!-- Resposta da pesquisa -->
                                <div class="col-md-6">
                                    <div class="card card-default">
                                        <div class="card-header">
                                            <h3 class="card-title">
                                                <i class="fas fa-chart-pie"></i>
                                                Tabela ANTT Oficial
                                            </h3>
                                        </div>
                                        <!-- /.card-header -->
                                        <div class="card-body">
                                            <div class="callout callout-success">
                                                <h5>I am a success callout!</h5>

                                                <p>This is a green callout.</p>
                                            </div>
                                        </div>
                                        <!-- /.card-body -->
                                    </div>
                                    <!-- /.card -->
                                </div>
                            </div>


                        </div>
                    </div>
                </div>
            </div>
        </section>
        <%--<div class="toast-container position-fixed top-0 end-0 p-3">

            <div id="toastErro" class="toast text-bg-danger border-0">
                <div class="d-flex">
                    <div class="toast-body" id="toastMsg"></div>
                </div>
            </div>

        </div>--%>
    </div>
    <!-- jQuery + Bootstrap (JS) -->
    <script src="https://code.jquery.com/jquery-3.6.4.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>

    <script>
        // Valida tudo ao clicar
        function validarCampos() {
            let campos = [
                { id: "<%= ddlTipoCarga.ClientID %>", nome: "tipo de carga" },
                { id: "<%= ddlEixos.ClientID %>", nome: "número de eixos" },
                { id: "<%= txtDistancia.ClientID %>", nome: "distância" }
            ];

            let erro = false;
            campos.forEach(c => {
                if (!validarCampo(c.id, c.nome)) erro = true;
            });

            // se erro = true, cancela o postback
            return !erro;
        }

        // Valida um campo específico: aplica classes e mensagens
        function validarCampo(idCampo, nomeCampo) {
            let campo = document.getElementById(idCampo);
            // span com id "msg_<clientid>"
            let msg = document.getElementById("msg_" + idCampo);

            // limpa classes/messages antes
            campo.classList.remove("campo-erro", "campo-ok");
            if (msg) msg.innerHTML = "";

            if (campo.value.trim() === "") {
                campo.classList.add("campo-erro");
                if (msg) msg.innerHTML = "O campo " + nomeCampo + " é obrigatório.";
                // Força reflow para a animação shake reaplicar em chamadas consecutivas
                campo.offsetWidth;
                return false;
            } else {
                campo.classList.add("campo-ok");
                if (msg) msg.innerHTML = "";
                return true;
            }
        }

        // Chamado ao digitar / perder foco
        function validarInstantaneo(idCampo, nomeCampo) {
            validarCampo(idCampo, nomeCampo);
        }
    </script>

</asp:Content>
