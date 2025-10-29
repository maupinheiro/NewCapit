﻿<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="GestaoDeCargasMatriz.aspx.cs" Inherits="NewCapit.dist.pages.GestaoDeCargas" %>

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

    <script>
        $(document).ready(function () {
            $('#<%= gvCargas.ClientID %>').DataTable({
                language: {
                    url: "//cdn.datatables.net/plug-ins/1.13.6/i18n/pt-BR.json"
                }
            });
        });
    </script>

    <div class="content-wrapper">
        <section class="content">
            <div class="container-fluid">
                <br />
                <div id="toastContainerVermelho" class="alert alert-danger alert-dismissible" style="display: none;">
                    <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                    <h5><i class="icon fas fa-exclamation-triangle"></i>Alerta!</h5>
                    Alertas
                </div>
                <div class="col-md-12">
                    <div class="card card-info">
                        <div class="card-header">
                            <h3 class="card-title">
                                <h3 class="card-title"><i class="fas fa-shipping-fast"></i>&nbsp;GESTÃO DE CARGAS</h3>
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
                            <div class="row mb-3">
                                <div class="col-md-2">
                                    <label>Data Inicial:</label>
                                    <asp:TextBox ID="DataInicio" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <label>Data Final:</label>
                                    <asp:TextBox ID="DataFim" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <label>Status:</label>
                                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                                        <asp:ListItem Text="Todas" Value="" />
                                        <asp:ListItem Text="Pendente" Value="Pendente" />
                                        <asp:ListItem Text="Concluído" Value="Concluído" />
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-1">
                                    <label>&nbsp;</label><br />
                                    <asp:Button ID="btnFiltrar" runat="server" CssClass="btn btn-warning" Text="Filtrar" OnClick="btnFiltrar_Click" />
                                </div>
                                <div class="col-md-2">
                                    <label>&nbsp;</label><br />
                                    <asp:Button ID="btnExportarExcel" runat="server" CssClass="btn btn-success" Text="Exportar para Excel" OnClick="btnExportarExcel_Click" />
                                </div>
                                <div class="col-md-2">
                                    <label>&nbsp;</label><br />
                                    <a href="/dist/pages/Frm_CadPedidosMatriz.aspx" class="d-none d-sm-inline-block btn btn-primary shadow-sm"><i
                                        class="fas fa-boxes"></i>&nbsp;Nova Carga
                                    </a>
                                </div>
                            </div>
                            <div class="container-fluid">

                                <%--<asp:GridView ID="gvCargas" runat="server" CssClass="table table-striped" AutoGenerateColumns="False" AllowPaging="True" PageSize="25" OnPageIndexChanging="gvCargas_PageIndexChanging" OnRowEditing="gvCargas_RowEditing" OnRowCancelingEdit="gvCargas_RowCancelingEdit"
                                    OnRowUpdating="gvCargas_RowUpdating">--%>
                                <table id="example1" class="table table-bordered table-striped table-hover table-responsive">
    <asp:GridView runat="server" ID="gvCargas" CssClass="table table-bordered table-striped table-hover" Width="100%" AutoGenerateColumns="False" DataKeyNames="id" AllowPaging="True" PageSize="25" OnPageIndexChanging="gvCargas_PageIndexChanging" ShowHeaderWhenEmpty="True">
        <PagerStyle HorizontalAlign="Center" CssClass="pagination-centered" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" ShowHeader="True" ItemStyle-Width="9">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkEditar" runat="server" OnClick="Editar" CssClass="btn btn-info btn-sm"><i class="fas fa-edit"></i></i>
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Id" HeaderText="ID" Visible="false" />
                                        <asp:BoundField DataField="carga" HeaderText="Carga" />
                                        <asp:BoundField DataField="emissao" HeaderText="Emissão" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                                        <asp:BoundField DataField="peso" HeaderText="Peso" />
                                        <asp:BoundField DataField="status" HeaderText="Status" />
                                        <asp:BoundField DataField="previsao" HeaderText="Previsão" DataFormatString="{0:dd/MM/yyyy}" />
                                        <%--<asp:BoundField DataField="Valor" HeaderText="Valor" DataFormatString="{0:C}" />--%>
                                        <asp:BoundField DataField="cliorigem" HeaderText="Remetente" />
                                        <asp:BoundField DataField="cidorigem" HeaderText="Município" />
                                        <asp:BoundField DataField="clidestino" HeaderText="Destinatário" />
                                        <asp:BoundField DataField="ciddestino" HeaderText="Município" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
        </section>
    </div>


</asp:Content>
