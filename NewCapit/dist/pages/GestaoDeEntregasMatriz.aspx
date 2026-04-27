<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="GestaoDeEntregasMatriz.aspx.cs" Inherits="NewCapit.dist.pages.GestaoDeEntregasMatriz" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- Bootstrap 5 -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <!-- Chart.js -->
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>

    <script src="https://cdn.datatables.net/1.13.6/js/jquery.dataTables.min.js"></script>
    <link href="https://cdn.datatables.net/1.13.6/css/jquery.dataTables.min.css" rel="stylesheet" />
    <script src="https://code.jquery.com/jquery-3.7.0.min.js"></script>

    <link href="bootstrap.min.css" rel="stylesheet" />
    <script src="bootstrap.bundle.min.js"></script>

    <script src="https://cdn.jsdelivr.net/npm/chartjs-plugin-datalabels@2"></script>

    <link rel="stylesheet" href="https://cdn.datatables.net/1.13.6/css/jquery.dataTables.min.css">

    <script src="https://code.jquery.com/jquery-3.7.1.min.js"></script>
    <script src="https://cdn.datatables.net/1.13.6/js/jquery.dataTables.min.js"></script>

    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>

    <script>
        function renderKPI(labels, valores) {

            const container = document.getElementById("kpiContainer");
            container.innerHTML = "";

            const max = Math.max(...valores);

            labels.forEach((label, i) => {

                let valor = valores[i];

                // 🎨 cores por status
                let cor = "#6c757d";

                let s = label.toUpperCase();

                if (s.includes("PRONTO")) cor = "#ffc107";
                else if (s.includes("EM TRANSITO")) cor = "#8B008B";
                else if (s.includes("AG. DESCARGA")) cor = "#198754";
                else if (s.includes("AG. CARREGAMENTO")) cor = "#dc3545";
                else if (s.includes("AG. DOCUMENTOS")) cor = "#FFC0CB";
                else if (s.includes("CARREGANDO")) cor = "#FFFF00";
                else if (s.includes("PENDENTE")) cor = "#B0C4DE";
                else if (s.includes("PERNOITE")) cor = "#00FFFF";
                else if (s.includes("CONCLUIDO")) cor = "#20B2AA";
                else if (s.includes("LIBERADO VAZIO")) cor = "#7FFFD4";
                else if (s.includes("VEIC. QUEBRADO")) cor = "#A0522D";
                else if (s.includes("CANCELADA")) cor = "#7B68EE";

                let porcentagem = (valor / max) * 100;

                container.innerHTML += `
            <div class="kpi-item">
                <div class="kpi-label">${label}</div>
                <div class="kpi-bar">
                    <div class="kpi-fill" style="width:${porcentagem}%; background:${cor};"></div>
                </div>
                <div class="kpi-value">${valor}</div>
            </div>
        `;
            });
        }
    </script>
    <script>
        function buscarDocumento() {

            let numero = document.getElementById("txtNumeroDocumento").value;

            if (numero === "") {
                alert("Digite o número do documento");
                return;
            }

            fetch("ControleDePedagio.aspx/BuscarDocumento", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ numeroDocumento: numero })
            })
                .then(res => res.json())
                .then(res => {

                    if (!res.d || !res.d.encontrado) {
                        alert("Documento não encontrado");
                        return;
                    }

                    document.getElementById("lblChave").innerText = res.d.chave;
                    document.getElementById("lblEmissao").innerText = res.d.emissao;
                    document.getElementById("lblEmpresa").innerText = res.d.empresa;
                    document.getElementById("lblMotorista").innerText = res.d.motorista;
                    document.getElementById("lblDestino").innerText = res.d.destino;
                    document.getElementById("lblCidade").innerText = res.d.cidade + "/" + res.d.uf;
                    document.getElementById("lblDataSaida").innerText = res.d.dataSaida;
                });
        }
    </script>
    <script type="text/javascript">
        // Esta função roda sempre que o UpdatePanel termina de carregar
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            // Se houver algum backdrop travado, nós removemos
            $('.modal-backdrop').remove();
            $('body').removeClass('modal-open');
            $('body').css('overflow', 'auto');
        });

        // Função para selecionar todos os checkboxes (ajuste do seu código)
        function SelecionarTodos(headerCheckBox) {
            var grid = headerCheckBox.closest('table');
            var checkboxes = grid.querySelectorAll('input[type="checkbox"]');
            checkboxes.forEach(chk => chk.checked = headerCheckBox.checked);
        }
    </script>
    <style>
        .table-sap {
            width: 100%;
            border-collapse: collapse;
            font-family: "Segoe UI", Arial, sans-serif;
            font-size: 13px;
        }

            /* Cabeçalho SAP */
            .table-sap thead {
                background-color: #0A6ED1;
                color: #fff;
            }

                .table-sap thead th {
                    padding: 10px;
                    text-align: center;
                    border: 1px solid #d9d9d9;
                }

            /* Corpo */
            .table-sap tbody td {
                padding: 8px;
                border: 1px solid #e5e5e5;
            }

            /* Zebra */
            .table-sap tbody tr:nth-child(even) {
                background-color: #f5f7fa;
            }

            /* Hover */
            .table-sap tbody tr:hover {
                background-color: #e8f3ff;
            }

        /* Controles DataTable */
        .dataTables_wrapper .dataTables_filter input {
            border: 1px solid #ccc;
            padding: 5px;
            border-radius: 4px;
        }

        .dataTables_wrapper .dataTables_length select {
            border: 1px solid #ccc;
            padding: 4px;
        }

        /* Paginação estilo SAP */
        .dataTables_wrapper .dataTables_paginate .paginate_button {
            background: #f5f5f5;
            border: 1px solid #d9d9d9 !important;
            padding: 5px 10px;
            margin: 2px;
            border-radius: 3px;
            cursor: pointer;
        }

            .dataTables_wrapper .dataTables_paginate .paginate_button.current {
                background: #0A6ED1 !important;
                color: #fff !important;
                border: 1px solid #0A6ED1 !important;
            }

            .dataTables_wrapper .dataTables_paginate .paginate_button:hover {
                background: #e8f3ff !important;
            }
        /* Info */
        .dataTables_info {
            margin-top: 10px;
        }

        .sub-info {
            font-size: 11px;
            color: #6a6d70;
        }

        .table-sap td div {
            line-height: 16px;
        }

        .grid-sap-container {
            max-height: 450px; /* 👈 altura da grade */
            overflow-y: auto; /* 👈 scroll vertical */
            border: 1px solid #d9d9d9;
        }

        /* mantém header fixo estilo ERP */
        .gv-header-custom {
            position: sticky;
            top: 0;
            background-color: #0A6ED1;
            color: #fff;
            z-index: 10;
        }

            .gv-header-custom th {
                height: 45px; /* 👈 altura do cabeçalho */
                padding: 10px 8px; /* 👈 controle do “respiro” interno */
                line-height: 20px; /* 👈 alinhamento vertical */
                font-size: 13px;
                text-align: center;
                vertical-align: middle;
            }

        .grid-sap-container {
            max-height: 500px; /* altura da grid */
            overflow-y: auto; /* scroll vertical */
            border: 1px solid #d9d9d9;
        }

        /* Cabeçalho fixo estilo SAP */
        .gv-header-custom th {
            position: sticky;
            top: 0; /* fixa no topo */
            z-index: 100;
            background-color: #0A6ED1;
            color: #fff;
            height: 45px;
            padding: 10px 8px;
            text-align: center;
            vertical-align: middle;
            border-bottom: 2px solid #084c9e;
        }

        /* Garante que o body não sobrepõe o header */
        .table-sap {
            border-collapse: collapse;
            width: 100%;
            font-family: "Segoe UI", Arial;
            font-size: 13px;
        }

            .table-sap td {
                padding: 8px;
                border: 1px solid #e5e5e5;
            }

        .kpi-container {
            display: flex;
            flex-direction: column;
            gap: 6px;
            font-size: 12px;
        }

        .kpi-item {
            display: flex;
            align-items: center;
            gap: 10px;
        }

        .kpi-label {
            width: 130px;
        }

        .kpi-bar {
            flex: 1;
            height: 8px;
            background: #eee;
            border-radius: 5px;
            overflow: hidden;
        }

        .kpi-fill {
            height: 100%;
            border-radius: 5px;
        }

        .kpi-value {
            width: 35px;
            text-align: right;
            font-weight: bold;
            font-size: 13px;
        }
        .form-switch .form-check-input {
        appearance: checkbox !important;
        width: 1em;
        height: 1em;
        border-radius: 0;
    }
    </style>
    <script>
        document.addEventListener("DOMContentLoaded", function () {

            const chk = document.getElementById("chkOcultarConcluidos");
            const lblOcultos = document.getElementById("lblOcultos");
            const lblVisiveis = document.getElementById("lblVisiveis");
            const lblTotal = document.getElementById("lblTotal");

            if (!chk) return;

            // 🔄 carregar estado salvo
            chk.checked = localStorage.getItem("ocultarConcluidos") === "true";

            function aplicarFiltro() {

                let linhas = document.querySelectorAll("#<%= gvOrdens.ClientID %> tr");

                let ocultos = 0;
                let visiveis = 0;
                let total = 0;

                linhas.forEach((linha, index) => {

                    // ignora header
                    if (index === 0) return;

                    total++;

                    let situacao = (linha.getAttribute("data-situacao") || "").toUpperCase();

                    if (chk.checked) {
                        if (situacao.includes("VIAGEM CONCLUIDA")) {
                            linha.style.display = "none";
                            ocultos++;
                        } else {
                            linha.style.display = "";
                            visiveis++;
                        }
                    } else {
                        linha.style.display = "";
                        visiveis++;
                    }
                });

                // 📊 atualiza contadores
                lblVisiveis.innerText = "Mostrando: " + visiveis + " de " + visiveis + " registro(s).";
                lblTotal.innerText = "Total de Coletas: " + total;

                if (chk.checked && ocultos > 0) {
                    lblOcultos.innerText = ocultos + " ocultos";
                    //lblOcultos.style.fontSize = "9px";
                } else {
                    lblOcultos.innerText = "";
                }
            }

            chk.addEventListener("change", function () {
                localStorage.setItem("ocultarConcluidos", chk.checked);
                aplicarFiltro();
            });

            aplicarFiltro();
        });
    </script>
    <script>
        const chk = document.getElementById("chkOcultarConcluidos");

        // carregar estado
        chk.checked = localStorage.getItem("ocultarConcluidos") === "true";

        chk.addEventListener("change", function () {
            localStorage.setItem("ocultarConcluidos", chk.checked);
        });
    </script>
    <div class="container-fluid">
        <section class="content-wrapper">
            <section class="content">
                <br />
                <div id="toastContainerVermelho" class="alert alert-danger alert-dismissible" style="display: none;">
                    <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                    <h5><i class="icon fas fa-exclamation-triangle"></i>Alerta!</h5>
                    Alertas
                </div>
                <div class="col-md-12">
                    <div class="card card-info">
                        <div class="card-header" style="background-color: #A020F0; font-weight: bold;">
                            <h3 class="card-title"><i class="fas fa-shipping-fast"></i>&nbsp;Gestão de Coletas e Entregas
                                <br />
                                <small>Coletas e Entregas</small></h3>
                        </div>
                        <div class="card-body">
                            <div class="row mb-3">
                                <div class="col-md-2">
                                    <label>Data Inicial:</label>
                                    <asp:TextBox ID="DataInicio" runat="server" TextMode="Date" CssClass="form-control"
                                        AutoPostBack="true" OnTextChanged="FiltroPeriodo_TextChanged" />
                                </div>
                                <div class="col-md-2">
                                    <label>Data Final:</label>
                                    <asp:TextBox ID="DataFim" runat="server" TextMode="Date" CssClass="form-control"
                                        AutoPostBack="true" OnTextChanged="FiltroPeriodo_TextChanged" />
                                </div>
                                <div class="col-md-2">
                                    <label>&nbsp;</label><br />                                    
                                    <asp:Button ID="btnExcel"
                                            runat="server"
                                            Text="Exportar Excel"
                                            CssClass="btn btn-success w-100"
                                            OnClick="btnExcel_Click" />
                                </div>
                                <div class="col-md-2">
                                    <label>&nbsp;</label><br />
                                    <a href="/dist/pages/ColetasMatriz.aspx" class="d-none d-sm-inline-block btn btn-primary shadow-sm w-100"><i
                                        class="fas fa-boxes"></i>&nbsp;Abrir Carregamento
                                    </a>
                                </div>
                                <div class="col-md-2">
                                    <label>&nbsp;</label><br />
                                    <asp:Button ID="btnBaixar" runat="server" CssClass="btn btn-info w-100" Text="Baixar DOC" OnClick="btnBaixar_Click" />
                                </div>
                                <div class="col-md-2">
                                    <label>&nbsp;</label><br />
                                    <asp:Button ID="btnAbrirMdfe" runat="server"
                                        CssClass="btn btn-secondary w-100"
                                        Text="Gerenciar MDF-e"
                                        OnClick="btnAbrirMdfe_Click"
                                        PostBack="true" />
                                </div>
                            </div>
                            <div class="row mb-3">
                                <div class="col-md-1">
                                    <asp:TextBox
                                        ID="txtFrota"
                                        runat="server"
                                        CssClass="form-control"
                                        AutoPostBack="true"
                                        OnTextChanged="FiltroPeriodo_TextChanged"
                                        placeholder="Frota" />
                                </div>
                                <div class="col-md-1">
                                    <asp:TextBox
                                        ID="txtPlaca"
                                        runat="server"
                                        CssClass="form-control"
                                        AutoPostBack="true"
                                        OnTextChanged="FiltroPeriodo_TextChanged"
                                        placeholder="Placa" />
                                </div>
                                <div class="col-md-2">
                                    <asp:TextBox
                                        ID="txtMotorista"
                                        runat="server"
                                        CssClass="form-control"
                                        AutoPostBack="true"
                                        OnTextChanged="FiltroPeriodo_TextChanged"
                                        placeholder="Crachá/Nome do Motorista" />
                                </div>
                                <div class="col-md-2">
                                    <asp:TextBox
                                        ID="txtExpedidor"
                                        runat="server"
                                        CssClass="form-control"
                                        AutoPostBack="true"
                                        OnTextChanged="FiltroPeriodo_TextChanged"
                                        placeholder="Expedidor" />
                                </div>
                                <div class="col-md-2">
                                    <asp:TextBox
                                        ID="txtRecebedor"
                                        runat="server"
                                        CssClass="form-control"
                                        AutoPostBack="true"
                                        OnTextChanged="FiltroPeriodo_TextChanged"
                                        placeholder="Recebedor" />
                                </div>
                                <div class="col-md-2">
                                    <asp:TextBox
                                        ID="txtStatus"
                                        runat="server"
                                        CssClass="form-control"
                                        AutoPostBack="true"
                                        OnTextChanged="FiltroPeriodo_TextChanged"
                                        placeholder="Status " />
                                </div>
                                <div class="col-md-2">
                                    <%--<div class="form-check form-switch">
                                        <input class="form-check-input" type="checkbox" role="switch" id="chkOcultarConcluidos" runat="server" checked>
                                        <label class="form-check-label" for="chkOcultarConcluidos">
                                            Ocultar concluídas 
                                        </label>
                                    </div>
                                    <span id="lblOcultos" style="font-size: 20px; color: #666;"></span>--%>

                                    <div class="form-check form-switch">
                                        <asp:CheckBox
                                            ID="chkOcultarConcluidos"
                                            runat="server"
                                            CssClass="form-check-input"
                                            Checked="true"
                                            AutoPostBack="true"
                                            OnCheckedChanged="chkOcultarConcluidos_CheckedChanged" />
                                        <label class="form-check-label">Ocultar concluídas</label>
                                    </div>


                                </div>
                            </div>
                            <div class="row mb-3">
                                <div class="col-6">
                                    <span id="lblVisiveis"></span>
                                </div>
                                <div class="col-6">
                                    <span id="lblTotalGeral" runat="server" style="float: right;"></span>
                                </div>
                            </div>
                            <div class="table-container">
                                <div class="row">
                                    <div class="col-12">
                                        <!-- /.col -->
                                        <div class="card">
                                            <!-- /.card-header -->
                                            <div class="card-body">
                                                <div id="kpiContainer" class="kpi-container"></div>
                                                <br />
                                                <asp:UpdatePanel ID="up1" runat="server">
                                                <ContentTemplate>
                                                <asp:GridView
                                                    ID="gvOrdens"
                                                    runat="server"
                                                    AutoGenerateColumns="False"
                                                    CssClass="table-sap"
                                                    HeaderStyle-CssClass="gv-header-custom"
                                                    AllowPaging="false"
                                                    OnPageIndexChanging="gvOrdens_PageIndexChanging"
                                                    OnRowCreated="gvOrdens_RowCreated"
                                                    OnRowDataBound="gvOrdens_RowDataBound">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="#">
                                                            <ItemTemplate>
                                                                <div>
                                                                                                                <%--<asp:ImageButton ID="lnkEditar" ImageUrl='<%# Eval("foto") %>' style="width: 60px; height:60px;" runat="server" CssClass="rounded-circle"
                                                 CommandName="Editar"
                                                 CommandArgument='<%# Eval("num_carregamento") %>'
                                                 OnCommand="lnkEditar_Command"
                                                 OnClientClick="event.stopPropagation();"/> --%> 

<asp:ImageButton ID="lnkEditar"
    ImageUrl='<%# Eval("foto") %>'
    runat="server"
    style="width:60px; height:60px; border-radius:8px; object-fit:cover;"
    CommandName="Editar"
    CommandArgument='<%# Eval("num_carregamento") %>' />
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Veículo">
                                                            <ItemTemplate>
                                                                <div><%# Eval("veiculo") %></div>
                                                                <div class="sub-info"><%# Eval("tipoveiculo") %></div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Placa">
                                                            <ItemTemplate>
                                                                <div><%# Eval("placa") %></div>
                                                                <div class="sub-info"><%# Eval("reboque1")  + " - " +  Eval("reboque2") %></div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Motorista">
                                                            <ItemTemplate>
                                                                <div><%# Eval("codmotorista") + " - " + Eval("nomemotorista") %></div>
                                                                <div class="sub-info"><%# Eval("codtra") + " - " + Eval("transportadora")%></div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Expedidor/Recebedor">
                                                            <ItemTemplate>
                                                                <div><%# Eval("cod_expedidor") + " - " + Eval("expedidor") %></div>
                                                                <div class="sub-info"><%# Eval("cod_recebedor") + " - " + Eval("recebedor")%> </div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Inicio/Termino da Prestação">
                                                            <ItemTemplate>
                                                                <div><%# Eval("cid_expedidor") + "/" + Eval("uf_expedidor") %></div>
                                                                <div class="sub-info"><%# Eval("cid_recebedor") + "/" + Eval("uf_recebedor")%></div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Ordem Coleta">
                                                            <ItemTemplate>
                                                                <div><%# Eval("num_carregamento") + " ("+ Eval("carga") + ")"%></div>
                                                                <div class="sub-info"><%# Eval("emissao", "{0:dd/MM/yyyy HH:mm}") %></div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Atendimento">
                                                            <ItemTemplate>
                                                                <div class="sub-info"><%# Eval("situacao") %></div>
                                                                <div>
                                                                    <asp:Label
                                                                        ID="lblStatus"
                                                                        runat="server"
                                                                        CssClass="badge" />
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                    </Columns>


                                                </asp:GridView>
                                                <asp:Timer ID="Timer1" runat="server" Interval="1200000" OnTick="Timer1_Tick"></asp:Timer>

    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                <pagertemplate>                                                    <div class="d-flex justify-content-center align-items-center gap-2 flex-wrap">


                                                        <asp:LinkButton ID="btnPrimeiro" runat="server"
                                                            OnClick="btnPrimeiro_Click"
                                                            CssClass="btn btn-light btn-sm">
            <i class="fas fa-angle-double-left"></i>
                                                        </asp:LinkButton>


                                                        <asp:LinkButton ID="btnAnterior" runat="server"
                                                            OnClick="btnAnterior_Click"
                                                            CssClass="btn btn-light btn-sm">
            <i class="fa fa-angle-left"></i>
                                                        </asp:LinkButton>


                                                        <span class="fw-bold">Página
            <asp:Label ID="lblPaginaAtual" runat="server" />
                                                            de
            <asp:Label ID="lblTotalPaginas" runat="server" />
                                                        </span>


                                                        <asp:LinkButton ID="btnProximo" runat="server"
                                                            OnClick="btnProximo_Click"
                                                            CssClass="btn btn-light btn-sm">
            <i class="fa fa-angle-right"></i>
                                                        </asp:LinkButton>


                                                        <asp:LinkButton ID="btnUltimo" runat="server"
                                                            OnClick="btnUltimo_Click"
                                                            CssClass="btn btn-light btn-sm">
            <i class="fas fa-angle-double-right"></i>
                                                        </asp:LinkButton>


                                                        <span>Página:</span>

                                                        <asp:TextBox ID="txtIrPagina" runat="server"
                                                            CssClass="form-control form-control-sm"
                                                            Style="width: 70px;" />

                                                        <asp:LinkButton ID="btnIrPagina" runat="server"
                                                            CssClass="btn btn-primary btn-sm"
                                                            OnClick="btnIrPagina_Click">
            Buscar
                                                        </asp:LinkButton>

                                                    </div>
                                                </pagertemplate>
                                            </div>

                                        </div>
                                        <!-- /.card -->
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </section>
        </section>
        <!-- Modal -->
        <div class="modal fade" role="dialog" id="modalCTE" tabindex="-1">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="modal-dialog modal-dialog-centered modal-lg">
                        <div class="modal-content border-0 shadow">

                            <div class="modal-header bg-light">
                                <h5 class="modal-title">Baixar Documentos CT-e / NFS-e</h5>
                                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                            </div>

                            <div class="modal-body">
                                <div class="row mb-3">
                                    <div class="col-md-12 d-flex gap-4">
                                        <div class="form-check">
                                            <asp:RadioButton ID="gridRadiosCTe" runat="server" GroupName="TipoDoc" CssClass="form-check-input" Checked="true" />
                                            <label class="form-check-label" for="<%= gridRadiosCTe.ClientID %>">CT-e</label>
                                        </div>
                                        <div class="form-check">
                                            <asp:RadioButton ID="gridRadiosNFSe" runat="server" GroupName="TipoDoc" CssClass="form-check-input" />
                                            <label class="form-check-label" for="<%= gridRadiosNFSe.ClientID %>">NFS-e</label>
                                        </div>
                                    </div>
                                </div>

                                <div class="row g-2 align-items-end mb-3">
                                    <div class="col-md-6">
                                        <label class="form-label fw-bold">Número do Documento</label>
                                        <asp:TextBox ID="txtNumeroDocumento" CssClass="form-control" runat="server" placeholder="Digite o número..."></asp:TextBox>
                                    </div>
                                    <div class="col-md-3">
                                        <asp:LinkButton ID="btnBuscar" runat="server" OnClick="btnBuscar_Click" CssClass="btn btn-warning w-100">
                                    <i class="bi bi-search"></i> Pesquisar
                                        </asp:LinkButton>
                                    </div>
                                </div>

                                <hr />

                                <div class="row mt-3">
                                    <div class="col-md-6">
                                        <p>
                                            <strong>Chave:</strong>
                                            <asp:Label ID="lblChave" runat="server" CssClass="text-muted"></asp:Label>
                                        </p>
                                        <p>
                                            <strong>Emissão:</strong>
                                            <asp:Label ID="lblEmissao" runat="server"></asp:Label>
                                        </p>
                                        <p>
                                            <strong>Empresa:</strong>
                                            <asp:Label ID="lblEmpresa" runat="server"></asp:Label>
                                        </p>
                                    </div>
                                    <div class="col-md-6 border-start">
                                        <p>
                                            <strong>Motorista:</strong>
                                            <asp:Label ID="lblMotorista" runat="server"></asp:Label>
                                        </p>
                                        <p>
                                            <strong>Destino:</strong>
                                            <asp:Label ID="lblDestino" runat="server"></asp:Label>
                                        </p>
                                        <p>
                                            <strong>Cidade/UF:</strong>
                                            <asp:Label ID="lblCidade" runat="server"></asp:Label>
                                        </p>
                                        <p>
                                            <strong>Data Saída:</strong>
                                            <asp:Label ID="lblDataSaida" runat="server"></asp:Label>
                                        </p>
                                    </div>
                                </div>
                            </div>

                            <div class="modal-footer bg-light">
                                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Fechar</button>
                                <asp:Button ID="btnSalvarBaixa" runat="server"
                                    CssClass="btn btn-success px-5"
                                    Text="Baixar Documento"
                                    OnClick="btnSalvarBaixa_Click" />
                            </div>

                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnSalvarBaixa" />
                    <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <div class="modal fade" id="modalMdfe" tabindex="-1">
                    <div class="modal-dialog modal-fullscreen">
                        <div class="modal-content">

                            <div class="modal-header">
                                <h5 class="modal-title">Gerenciar MDF-e</h5>
                                <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                            </div>

                            <div class="modal-body">
                                <div class="row mb-2">
                                    <div class="col-md-6">
                                        <asp:TextBox ID="txtPesquisarMDFe" runat="server"
                                            CssClass="form-control"
                                            Placeholder="Pesquisar MDF-e..."
                                            AutoPostBack="true"
                                            OnTextChanged="FiltroChanged" />
                                    </div>

                                    <div class="col-md-3">
                                        <asp:DropDownList ID="ddlFiltroStatus" runat="server"
                                            CssClass="form-control"
                                            AutoPostBack="true"
                                            OnSelectedIndexChanged="FiltroChanged">
                                            <asp:ListItem Text="Todos" Value="" />
                                            <asp:ListItem Text="Baixados" Value="Baixado" />
                                            <asp:ListItem Text="Pendentes" Value="Pendente" />

                                        </asp:DropDownList>
                                    </div>
                                </div>

                                <asp:GridView ID="gvMdfe" runat="server" OnRowCommand="gvMdfe_RowCommand"
                                    CssClass="table table-bordered table-sm"
                                    AutoGenerateColumns="false"
                                    HtmlEncode="false"
                                    DataKeyNames="id">

                                    <Columns>
                                        <asp:BoundField DataField="mdfe_uf" HeaderText="UF" />
                                        <asp:BoundField DataField="mdfe_empresa" HeaderText="Empresa" />

                                        <asp:TemplateField HeaderText="">
                                            <HeaderTemplate>
                                                <input type="checkbox" onclick="SelecionarTodos(this)" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelecionar" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="mdfe_numero" HeaderText="Número" />
                                        <asp:BoundField DataField="mdfe_serie" HeaderText="Série" />

                                        <asp:TemplateField HeaderText="Situação MDF-e">
                                            <ItemTemplate>
                                                <span class='badge <%# 
                            (Eval("mdfe_situacao") == null || Eval("mdfe_situacao") == DBNull.Value)
                                ? "bg-warning text-dark"
                                : Eval("mdfe_situacao").ToString().Trim().ToUpper() == "BAIXADO"
                                    ? "bg-success"
                                    : "bg-warning text-dark"
                        %>'>

                                                    <%# Eval("mdfe_situacao") == null || Eval("mdfe_situacao") == DBNull.Value
                                ? "Pendente"
                                : Eval("mdfe_situacao") %>

                                                </span>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="status" HeaderText="Status" />

                                        <asp:TemplateField HeaderText="Local da Coleta">
                                            <ItemTemplate>
                                                <%# Eval("cid_expedidor") %>/<%# Eval("uf_expedidor") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Local da Entrega">
                                            <ItemTemplate>
                                                <%# Eval("cid_recebedor") %>/<%# Eval("uf_recebedor") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="mdfe_baixado" HeaderText="Baixado Por" />
                                        <asp:BoundField DataField="mdfe_data_baixa" HeaderText="Data Baixa" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                                        <asp:TemplateField HeaderText="Ações">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnCancelarLinha" runat="server"
                                                    CommandName="CancelarMDF"
                                                    CommandArgument='<%# Eval("id") %>'
                                                    CssClass="btn btn-outline-danger btn-sm"
                                                    OnClientClick="return confirm('Deseja realmente cancelar este MDF-e?');">
                                 <i class="fas fa-trash"></i> Cancelar
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>


                                </asp:GridView>
                            </div>

                            <div class="modal-footer">
                                <asp:Button ID="btnBaixarMDFe" runat="server"
                                    CssClass="btn btn-success"
                                    Text="Baixar"
                                    OnClick="btnBaixarMDFe_Click" />

                                <asp:Button ID="btnCancelarMDFe" runat="server" data-bs-dismiss="modal"
                                    CssClass="btn btn-danger"
                                    Text="Fechar"
                                    OnClick="btnCancelarMDFe_Click" />
                            </div>

                        </div>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="txtPesquisarMDFe" EventName="TextChanged" />
                <asp:AsyncPostBackTrigger ControlID="ddlFiltroStatus" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="btnBaixarMDFe" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnCancelarMDFe" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
        <!-- modal Gerenciar MDFe -->

    </div>
    <script type="text/javascript">
        // Captura o fim de qualquer atualização do UpdatePanel
        var prm = Sys.WebForms.PageRequestManager.getInstance();

        prm.add_endRequest(function () {
            // 1. Remove a div preta que escurece a tela
            $('.modal-backdrop').remove();

            // 2. Libera o scroll da página (o 'trava' vem daqui)
            $('body').removeClass('modal-open');
            $('body').css('overflow', 'auto');

            // 3. Simula o clique falso que você pediu em uma área neutra
            $('body').trigger('click');
        });
    </script>
    <script>
        setInterval(function () {
            location.reload();
        }, 600000);
    </script>
</asp:Content>
