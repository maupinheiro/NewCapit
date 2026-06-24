<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="ConsultaFretes.aspx.cs" Inherits="NewCapit.dist.pages.ConsultaFretes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link rel="stylesheet" href="/css/styleTabela.css">
    <!-- Page Heading -->
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
