<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="ControleCreditoAbastecimento.aspx.cs" Inherits="NewCapit.dist.pages.ControleCreditoAbastecimento" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        .pagination-centered {
            text-align: center;
        }

            .pagination-centered table {
                margin: 0 auto; /* Isso centraliza a tabela da paginação */
            }
    </style>
    <!-- script de máscara -->
<script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/jquery.inputmask/5.0.7/jquery.inputmask.min.js"></script>

<script>
    $(function () {
        $("#txtLimiteCreditoAbastecimento").inputmask("currency", {
            prefix: "R$ ",
            groupSeparator: ".",
            radixPoint: ",",
            digits: 2,
            autoGroup: true,
            rightAlign: false
        });
    });
</script>
    <!-- Page Heading -->
    <div class="content-wrapper">
        <div class="content-header">
            <div class="d-sm-flex align-items-center justify-content-between mb-4">
                <h1 class="h3 mb-2 text-gray-800">
                    <i class="far fa-credit-card"></i>&nbsp;Controle de Limite de Crédito para Abastecimento</h1>
               <%-- <asp:Button ID="btnAbrirModal" runat="server" Text="Novo Usuário" CssClass="btn btn-primary mb-3"
                    OnClientClick="$('#modalCadastro').modal('show'); return false;" />--%>
            </div>
        </div>

        <!-- Corpo da grid -->
        <div class="card shadow mb-4">
            <div class="card-header">
                <asp:TextBox ID="myInput" CssClass="form-control myInput" OnTextChanged="myInput_TextChanged" placeholder="Pesquisar ..." AutoPostBack="true" runat="server" Width="100%"></asp:TextBox>
            </div>
            <div class="card-body">
                <div class="table-responsive">
                    <asp:GridView runat="server" ID="gvListLimiteCredito" CssClass="table table-bordered dataTable1 table-hover" Width="100%" AutoGenerateColumns="False" DataKeyNames="ID" OnRowCommand="gvListLimiteCredito_RowCommand" AllowPaging="True" PageSize="10" OnPageIndexChanging="gvListLimiteCredito_PageIndexChanging" ShowHeaderWhenEmpty="True">
                        <PagerStyle HorizontalAlign="Center" CssClass="pagination-centered" />
                        <Columns>
                            <asp:TemplateField HeaderText="PROP.">
                                <ItemTemplate>
                                    <%# Eval("codtra") %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="PROPRIETÁRIO">
                                <ItemTemplate>
                                    <%# Eval("fantra") %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="PESSOA">
                                <ItemTemplate>
                                    <%# Eval("pessoa") %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="CNPJ/CPF">
                                <ItemTemplate>
                                    <%# Eval("cnpj") %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="ESTABELECIMENTO">
                                <ItemTemplate>
                                    <%# Eval("filial") %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="LIMITE">
                                <ItemTemplate>
                                    <%# Eval("limitecreditoabastecimento") %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="SALDO">
                                <ItemTemplate>
                                    <%# Eval("saldoparaabastecimento") %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="STATUS">
                                <ItemTemplate>
                                    <%# Eval("ativa_inativa") %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="AÇÕES" ShowHeader="True">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkEditar" runat="server" CssClass="btn btn-primary btn-sm" CommandName="LimiteCredito" CommandArgument='<%# Eval("ID") %>'><i class="fas fa-credit-card"></i> Limite de Crédito</asp:LinkButton>
                                    <asp:LinkButton ID="lnkDetalhe" runat="server" OnClick="Editar" CssClass="btn btn-primary btn-sm"><i class="fa fa-edit"></i> Detalhes</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
        <!-- Modal -->
        <div class="modal fade" id="modalCadastro" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered modal-xl" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">Limite de Crédito</h5>
                      <%--  <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>--%>
                    </div>
                    <div class="modal-body" >
                        <asp:Label runat="server" ID="lblMensagem" CssClass="text-success"></asp:Label>
                        <div class="row g-3">
                            <div class="col-md-2">
                                <div class="form-group">
                                    <span class="details">CÓDIGO:</span>
                                    <asp:TextBox ID="txtCodTra" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-7">
                                <div class="form-group">
                                    <span class="details">PROPRIETÁRIO:</span>
                                    <asp:TextBox ID="txtFanTra" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="form-group">
                                    <span class="details">ESTABELECIMENTO:</span>
                                    <asp:TextBox ID="txtFilial" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row g-3">
                            <div class="col-md-2">
                                <div class="form-group">
                                    <span class="details">PESSOA:</span>
                                    <asp:TextBox ID="txtPessoa" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="form-group">
                                    <span class="details">CPF/CNPJ:</span>
                                    <asp:TextBox ID="txtCnpj" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-2">
                                <div class="form-group">
                                    <span class="details">STATUS:</span>
                                    <asp:TextBox ID="txtAtivo_Inativo" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row g-3"></div>
                        <div class="row g-3">
                            <div class="col-md-3">
                                <div class="form-group">
                                    <span class="details">LIMITE DE CRÉDITO:</span>
                                    <asp:TextBox ID="txtLimiteCreditoAbastecimento" runat="server" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-6"></div>
                            <div class="col-md-3">
                                <div class="form-group">
                                    <span class="details">SALDO:</span>
                                    <asp:TextBox ID="txtSaldoParaAbastecimento" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnSalvar" runat="server" Text="Salvar" CssClass="btn btn-success" OnClick="btnSalvar_Click" />
                        <asp:Button ID="btnSair" class="btn btn-secondary" data-bs-dismiss="modal" runat="server" Text="Sair" />
                        <asp:HiddenField ID="hdfId" runat="server" />
                    </div>
                </div>
            </div>
        </div>

    </div>
    <!-- Scripts Bootstrap -->
    <script src="https://code.jquery.com/jquery-3.5.1.slim.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@4.5.2/dist/js/bootstrap.bundle.min.js"></script>
</asp:Content>
