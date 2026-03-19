<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="controlaestoque.aspx.cs" Inherits="NewCapit.dist.pages.controlaestoque" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="content-wrapper">
        <section class="content">
            <div class="container-fluid">
                <div class="col-md-12">
                    <div class="card card-info">
                        <div class="card-header" style="background-color: #A020F0; font-weight: bold;">
                            <h3 class="card-title"><i class="fas fa-shipping-fast"></i>&nbsp;Controle de Estoque</h3>
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
                        </div>

                        <div class="card-body">
                            <h4>Manutenção Ipiranga</h4>

                            <!-- Pesquisa de produto -->
                            <div class="row mb-3">
                                <div class="col-md-6">
                                    <asp:TextBox ID="txtPesquisaProduto" runat="server" CssClass="form-control" placeholder="Pesquisar produto..." />
                                </div>
                                <div class="col-md-2">
                                    <asp:Button ID="btnPesquisarProduto" runat="server" CssClass="btn btn-primary w-100" Text="Pesquisar" OnClick="btnPesquisarProduto_Click" />
                                </div>
                            </div>

                            <!-- Grid de Estoque -->
                            <div class="table-responsive mb-4">
                                <asp:GridView ID="gvEstoque" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-hover"
                                    OnRowDataBound="gvEstoque_RowDataBound">
                                    <Columns>
                                        <asp:BoundField DataField="id_peca" HeaderText="ID" />
                                        <asp:BoundField DataField="descricao_peca" HeaderText="Produto" />  
                                        <asp:BoundField DataField="unidade" HeaderText="Unidade" />
                                        <asp:BoundField DataField="estoque_peca" HeaderText="Estoque Atual" />
                                        <asp:BoundField DataField="estoque_minimo" HeaderText="Estoque Mínimo" />
                                        <asp:BoundField DataField="valor_unitario" HeaderText="Valor Unitário" DataFormatString="{0:C2}" />
                                    </Columns>
                                </asp:GridView>
                            </div>

                            <!-- Movimentação -->
                            <div class="row mb-4">
                                <div class="col-md-3">
                                    Produto:
                                    <asp:DropDownList ID="ddlProdutos" runat="server" CssClass="form-select" />
                                </div>
                                <div class="col-md-2">
                                    Tipo:
                                    <asp:DropDownList ID="ddlTipoMov" runat="server" CssClass="form-select">
                                        <asp:ListItem Value="E">Entrada</asp:ListItem>
                                        <asp:ListItem Value="S">Saída</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2">
                                    Quantidade:
                                    <asp:TextBox ID="txtQtd" runat="server" CssClass="form-control" />
                                </div>
                                <div class="col-md-3">
                                    Observação:
                                    <asp:TextBox ID="txtObs" runat="server" CssClass="form-control" />
                                </div>
                                <div class="col-md-2 d-flex align-items-end">
                                    <asp:Button ID="btnMovimentar" runat="server" CssClass="btn btn-success w-100" Text="Registrar" OnClick="btnMovimentar_Click" />
                                </div>
                            </div>

                            <!-- Histórico de Movimentações -->
                            <h5>Histórico de Movimentações</h5>
                            <div class="table-responsive">
                                <asp:GridView ID="gvHistorico" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover"
                                    OnRowDataBound="gvHistorico_RowDataBound">
                                    <Columns>
                                        <asp:BoundField DataField="idMov" HeaderText="ID" />
                                        <asp:BoundField DataField="descricao_peca" HeaderText="Produto" />
                                        <asp:TemplateField HeaderText="Tipo">
                                            <ItemTemplate>
                                                <asp:Image ID="imgTipo" runat="server" Width="20px" Height="20px" />
                                                <asp:Label ID="lblTipo" runat="server" Text='<%# Eval("tipoMov") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="quantidade" HeaderText="Quantidade" />
                                        <asp:BoundField DataField="dataMov" HeaderText="Data" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                                        <asp:BoundField DataField="responsavel" HeaderText="Responsável" />
                                        <asp:BoundField DataField="observacao" HeaderText="Observação" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>
</asp:Content>
