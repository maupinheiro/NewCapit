<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="ConsultaFretes.aspx.cs" Inherits="NewCapit.dist.pages.ConsultaFretes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link rel="stylesheet" href="/css/styleTabela.css">
    <!-- Page Heading -->
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
        color: #00050a;
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

    @keyframes flickerAnimation {
        0% {
            opacity: 1;
            transform: scale(1);
        }

        50% {
            opacity: 0.3;
            transform: scale(1.1);
            color: #ff0000;
        }

        100% {
            opacity: 1;
            transform: scale(1);
        }
    }

    .animate-flicker {
        animation: flickerAnimation 1.2s infinite;
        display: inline-block;
    }

    .btn-alerta-sirene {
        background: none;
        border: none;
        padding: 0;
        cursor: pointer;
    }
</style>
    <div class="content-wrapper">

        <div class="content-header">
            <div class="row g-3">
                <div class="col-md-3">
                    <div class="d-sm-flex align-items-center justify-content-between mb-4">
                        <h1 class="h3 mb-2 text-gray-800">
                            <i class="fas fa-file-invoice-dollar"></i>&nbsp;Gestão de Fretes</h1>
                    </div>
                </div>
                <div class="col-md-5">
                </div>
                <div class="col-md-2">
                    <a href="Frm_TabelaPrecoMatriz.aspx" class="d-none d-lg-inline-block btn btn-primary shadow-lg w-100"><i
                        class="fas fa-file-invoice-dollar"></i>&nbsp;Novo Cadastro            
                    </a>
                </div>
                <div class="col-md-2">
                    <a href="reajustefretes.aspx" class="d-none d-lg-inline-block btn btn-success shadow-lg  w-100"><i
                        class="fas fa-file-invoice-dollar"></i>&nbsp;Reajustes          
                    </a>
                </div>
            </div>
        </div>
        <div class="card shadow mb-4">
            <div class="card-body">
                <div class="card shadow mb-4">
                    <div class="card-body">
                        <div class="row g-3">
                            <div class="col-md-1">
                                <asp:TextBox
                                    ID="txtCodigo"
                                    CssClass="form-control"
                                    placeholder="Código..."
                                    runat="server"
                                    AutoPostBack="true"
                                    OnTextChanged="txtCodigo_TextChanged">
                                </asp:TextBox>
                            </div>
                            <div class="col-md-3">
                                <asp:TextBox
                                    ID="txtPagador"
                                    CssClass="form-control"
                                    placeholder="Pagador..."
                                    runat="server"
                                    AutoPostBack="true"
                                    OnTextChanged="txtPagador_TextChanged">
                                </asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox
                                    ID="txtExpedidor"
                                    CssClass="form-control"
                                    placeholder="Expedidor..."
                                    runat="server"
                                    AutoPostBack="true"
                                    OnTextChanged="txtExpedidor_TextChanged">
                                </asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtRecebedor"
                                    CssClass="form-control"
                                    placeholder="Recebedor..."
                                    runat="server"
                                    AutoPostBack="true"
                                    OnTextChanged="txtRecebedor_TextChanged">
                                </asp:TextBox>
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
                        <br />
                        <div class="table-responsive">
                            <asp:GridView 
                                ID="gvListFretes"
                                runat="server"
                                AutoGenerateColumns="False"
                                CssClass="table-sap"
                                HeaderStyle-CssClass="gv-header-custom"
                                AllowPaging="false"
                                DataKeyNames="cod_frete"      
                                OnPageIndexChanging="gvListFretes_PageIndexChanging">                               
                                <Columns>
                                    <asp:TemplateField HeaderText="Código">
                                        <ItemTemplate>
                                            <%# Eval("cod_frete") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Pagador">
                                        <ItemTemplate>
                                            <span><%# Eval("cod_pagador") + " - " + Eval("pagador") %></span>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Expedidor">
                                        <ItemTemplate>
                                            <%# Eval("cod_expedidor") + " - " + Eval("expedidor") + 
    " <b> (" + Eval("cid_expedidor") + "/" + Eval("uf_expedidor") + ")</b>" %>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Recebedor">
                                        <ItemTemplate>
                                            <%# Eval("cod_recebedor") + " - " + Eval("recebedor") + "<b> (" + Eval("cid_recebedor") + "/" + Eval("uf_recebedor") +")</b>" %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Situação">
                                        <ItemTemplate>
                                            <span class='<%# Eval("situacao").ToString().ToUpper() == "ATIVO"
                                                ? "badge bg-success"
                                                : "badge bg-danger" %>'
                                                style="font-size: 15px;">
                                                <%# Eval("situacao") %>
                                            </span>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="" ShowHeader="True">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkEditar" runat="server" OnClick="Editar" CssClass="btn btn-primary btn-sm"><i class="fa fa-edit"></i> Editar</asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <pagertemplate>
                                <div class="d-flex justify-content-center align-items-center gap-2 flex-wrap">
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

                </div>
            </div>
        </div>
    </div>
</asp:Content>
