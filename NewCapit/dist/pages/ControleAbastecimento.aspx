<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="ControleAbastecimento.aspx.cs" Inherits="NewCapit.dist.pages.ControleAbastecimento" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css" rel="stylesheet" />
    <div class="content-wrapper">
        <section class="content">
            <div class="container-fluid">
                <br />
                <div class="col-md-12">
                    <div class="card card-info">
                        <!-- Header -->
                        <div class="card-header" style="background-color: #A020F0; font-weight: bold;">
                            <h3 class="card-title"><i class="fas fa-shipping-fast"></i>&nbsp;Gestão de Postos
                                <br />
                                <small>Controle de Ordens de Abastecimentos</small></h3>
                        </div>
                        <br />
                        <%--HeaderStyle-CssClass="gv-header-custom"--%>
                        <div class="card-body">
                            <div class="row mb-3">

                                <div class="col-md-2">
                                    <label>Data Inicial</label>
                                    <asp:TextBox ID="txtDataInicial" runat="server" CssClass="form-control" TextMode="Date" />
                                </div>

                                <div class="col-md-2">
                                    <label>Data Final</label>
                                    <asp:TextBox ID="txtDataFinal" runat="server" CssClass="form-control" TextMode="Date" />
                                </div>

                                <div class="col-md-4">
                                    <label>Buscar</label>
                                    <asp:TextBox ID="txtBusca" runat="server" CssClass="form-control" placeholder="Motorista, placa, ordem, forta..." />
                                </div>

                                <div class="col-md-2 d-flex align-items-end">
                                    <asp:Button ID="btnFiltrar" runat="server" Text="Filtrar"
                                        CssClass="btn btn-primary w-100"
                                        OnClick="btnFiltrar_Click" />
                                </div>

                            </div>
                        </div>

                        <div class="card-body">
                            <asp:GridView ID="gvAbastecimento" runat="server"
                                AutoGenerateColumns="False"
                                CssClass="table table-hover table-bordered gv-center-pager"
                                HeaderStyle-CssClass="gv-header-custom"
                                AllowPaging="true"
                                PageSize="15"
                                OnPageIndexChanging="gvAbastecimento_PageIndexChanging"
                                OnRowCommand="gvAbastecimento_RowCommand"
                                OnRowDataBound="gvAbastecimento_RowDataBound"
                                OnDataBound="gvAbastecimento_DataBound">

                                <Columns>
                                    <asp:BoundField DataField="ordem_abastecimento" HeaderText="Ordem" />
                                    <asp:BoundField DataField="data_geracao" HeaderText="Data Emissão"
                                        DataFormatString="{0:dd/MM/yyyy HH:mm}" />

                                    <asp:TemplateField HeaderText="Situação">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStatus" runat="server"
                                                Text='<%# Eval("impressa") %>' CssClass="badge"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:BoundField DataField="nome_posto" HeaderText="Nome do Posto" />
                                    <asp:BoundField DataField="tipo_abastecimento" HeaderText="Abastecimento" />
                                    <asp:BoundField DataField="frota_agregado" HeaderText="Tipo" />
                                    <asp:BoundField DataField="filial" HeaderText="Filial" />
                                    <asp:BoundField DataField="nommot" HeaderText="Motorista" />
                                    <asp:BoundField DataField="codvei" HeaderText="Veículo" />
                                    <asp:BoundField DataField="plavei" HeaderText="Placa" />
                                    <asp:BoundField DataField="nomtra" HeaderText="Transportadora" />

                                    <asp:TemplateField HeaderText="Ações">
                                        <ItemTemplate>

                                            <asp:LinkButton ID="btnImprimir" runat="server"
                                                CommandName="Imprimir"
                                                CommandArgument='<%# Eval("ordem_abastecimento") %>'
                                                CssClass="btn btn-sm btn-primary">
                    <i class="fa fa-print"></i>
                                            </asp:LinkButton>

                                            <asp:LinkButton ID="btnConfirmar" runat="server"
                                                CommandName="Confirmar"
                                                CommandArgument='<%# Eval("ordem_abastecimento") %>'
                                                CssClass="btn btn-sm btn-success">
                    <i class="fa fa-check"></i>
                                            </asp:LinkButton>

                                            <asp:LinkButton ID="btnCancelar" runat="server"
                                                CommandName="Cancelar"
                                                CommandArgument='<%# Eval("ordem_abastecimento") %>'
                                                CssClass="btn btn-sm btn-danger">
                    <i class="fa fa-times"></i>
                                            </asp:LinkButton>

                                            <asp:LinkButton ID="btnVisualizar" runat="server"
                                                CommandName="Visualizar"
                                                CommandArgument='<%# Eval("ordem_abastecimento") %>'
                                                CssClass="btn btn-sm btn-info">
                    <i class="fa fa-eye"></i>
                                            </asp:LinkButton>

                                        </ItemTemplate>
                                    </asp:TemplateField>

                                </Columns>

                                
                                <PagerTemplate>

                                    <div class="d-flex justify-content-center align-items-center gap-2 flex-wrap">

                                        <asp:LinkButton runat="server" CommandName="Page" CommandArgument="First"
                                            CssClass="btn btn-light btn-sm">
                <i class="fa fa-angles-left"></i>
                                        </asp:LinkButton>

                                        <asp:LinkButton runat="server" CommandName="Page" CommandArgument="Prev"
                                            CssClass="btn btn-light btn-sm">
                <i class="fa fa-angle-left"></i>
                                        </asp:LinkButton>

                                        <span class="fw-bold">Página
                                            <asp:Label ID="lblPaginaAtual" runat="server" />
                                            de
                                            <asp:Label ID="lblTotalPaginas" runat="server" />
                                        </span>

                                        <asp:LinkButton runat="server" CommandName="Page" CommandArgument="Next"
                                            CssClass="btn btn-light btn-sm">
                <i class="fa fa-angle-right"></i>
                                        </asp:LinkButton>

                                        <asp:LinkButton runat="server" CommandName="Page" CommandArgument="Last"
                                            CssClass="btn btn-light btn-sm">
                <i class="fa fa-angles-right"></i>
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

                                </PagerTemplate>

                            </asp:GridView>
                        </div>
                    </div>
                </div>
        </section>
        <!-- Modal visualizar ordem de abastecimento -->
        <div class="modal fade" id="modalDetalhes" tabindex="-1">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">

                    <div class="modal-header">
                        <h5>Detalhes do Abastecimento</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                    </div>

                    <div class="modal-body">

                        <div class="row">
                            <div class="col-md-6">
                                <label>Ordem</label>
                                <asp:TextBox ID="txtOrdemModal" runat="server" CssClass="form-control" ReadOnly="true" />
                            </div>

                            <div class="col-md-6">
                                <label>Motorista</label>
                                <asp:TextBox ID="txtMotoristaModal" runat="server" CssClass="form-control" ReadOnly="true" />
                            </div>

                            <div class="col-md-6">
                                <label>Placa</label>
                                <asp:TextBox ID="txtPlacaModal" runat="server" CssClass="form-control" ReadOnly="true" />
                            </div>

                            <div class="col-md-6">
                                <label>Posto</label>
                                <asp:TextBox ID="txtPostoModal" runat="server" CssClass="form-control" ReadOnly="true" />
                            </div>

                        </div>

                    </div>

                </div>
            </div>
        </div>
    </div>
</asp:Content>
