<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="ConsultaFretes.aspx.cs" Inherits="NewCapit.dist.pages.ConsultaFretes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link rel="stylesheet" href="/css/styleTabela.css">

    <style>
        .pagination-centered {
            text-align: center;
        }

            .pagination-centered table {
                margin: 0 auto; /* Isso centraliza a tabela da paginação */
            }
    </style>
    <!-- Page Heading -->
    <div class="content-wrapper">
        <div class="content-header">
            <div class="d-sm-flex align-items-center justify-content-between mb-4">
                <h1 class="h3 mb-2 text-gray-800">
                    <i class="fas fa-file-invoice-dollar"></i>&nbsp;Gestão de Fretes</h1>
                <a href="Frm_TabelaPrecoMatriz.aspx" class="d-none d-sm-inline-block btn btn-sm btn-primary shadow-sm"><i
                    class="fas fa-file-invoice-dollar"></i>&nbsp;Novo Cadastro            
                </a>

            </div>
        </div>
        <!-- DataTales Example -->

        <div class="card shadow mb-4">
            <div class="card-body">
                <%--<input type="text" id="myInput" onkeyup="myFunction()" placeholder="Search for names..">--%>
                <div class="card-header">
                    <asp:TextBox ID="myInput" CssClass="form-control myInput" OnTextChanged="myInput_TextChanged" placeholder="Pesquisar ..." AutoPostBack="true" runat="server"></asp:TextBox>
                </div>
                <div class="card shadow mb-4">


                    <div class="card-body">
                        <div class="table-responsive">
                            <asp:GridView runat="server" ID="gvListFretes" CssClass="table table-bordered dataTable1 table-hover" Width="100%" AutoGenerateColumns="False" DataKeyNames="cod_frete" AllowPaging="True" PageSize="75" OnPageIndexChanging="gvListFretes_PageIndexChanging" ShowHeaderWhenEmpty="True">
                                <PagerStyle HorizontalAlign="Center" CssClass="pagination-centered" />
                                <Columns>
                                    <asp:TemplateField HeaderText="#">
                                        <ItemTemplate>
                                            <%# Eval("cod_frete") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Pagador/Tipo de Veículo">
                                        <ItemTemplate>
                                            <span><%# Eval("cod_pagador") + " - " + Eval("pagador") %></span>
                                            <br>
                                            <span class="negrito"><%# Eval("tipo_veiculo") %></span>
                                            <%--<%# Eval("tipo_veiculo") %> --%>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Expedidor">
                                        <ItemTemplate>
                                            <%# Eval("cod_expedidor") + " - " + Eval("expedidor") %>
                                            <br>
                                            <%# Eval("cid_expedidor") + "/" + Eval("uf_expedidor") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Recebedor">
                                        <ItemTemplate>
                                            <%# Eval("cod_recebedor") + " - " + Eval("recebedor") %>
                                            <br>
                                            <%# Eval("cid_recebedor") + "/" + Eval("uf_recebedor") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Frete TNG">
                                        <ItemTemplate>
                                            <%# Eval("frete_tng")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Agregado">
                                        <ItemTemplate>
                                            <%# Eval("frete_agregado")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Terceiro">
                                        <ItemTemplate>
                                            <%# Eval("frete_terceiro")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Vigência">
                                        <ItemTemplate>
                                            <%# Eval("vigencia_inicial") %>
                                            <br>
                                            <%# Eval("vigencia_final") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Situação">
                                        <ItemTemplate>
                                            <%# Eval("situacao") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="" ShowHeader="True">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkEditar" runat="server" OnClick="Editar" CssClass="btn btn-primary btn-sm"><i class="fa fa-edit"></i> Editar</asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>

                </div>
        </div>
    </div>
    </div>
</asp:Content>
