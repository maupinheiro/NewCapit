<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="reajustefretes.aspx.cs" Inherits="NewCapit.dist.pages.reajustefretes" %>

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
                        <div class="card-header" style="background-color: #A020F0; font-weight: bold;">
                            <h3 class="card-title">
                                <h3 class="card-title"><i class="fas fa-shipping-fast"></i>&nbsp;REAJUSTES DE FRETES</h3>
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
                            <div class="card shadow">
                                <div class="card-header bg-warning">
                                    <h5>Reajuste de Frete Avançado</h5>
                                </div>

                                <div class="card-body">

                                    <div class="row">

                                        <div class="col-md-3">
                                            <label>Cliente</label>
                                            <asp:DropDownList ID="ddlCliente" runat="server" CssClass="form-control" />
                                        </div>

                                        <div class="col-md-3">
                                            <label>Rota</label>
                                            <asp:DropDownList ID="ddlRota" runat="server" CssClass="form-control" />
                                        </div>

                                        <div class="col-md-3">
                                            <label>Pagador</label>
                                            <asp:DropDownList ID="ddlPagador" runat="server" CssClass="form-control" />
                                        </div>

                                        <div class="col-md-3">
                                            <label>Tipo Veículo</label>
                                            <asp:DropDownList ID="ddlTipoVeiculo" runat="server" CssClass="form-control" />
                                        </div>

                                    </div>

                                    <div class="row mt-3">

                                        <div class="col-md-3">
                                            <label>Tipo Material</label>
                                            <asp:DropDownList ID="ddlMaterial" runat="server" CssClass="form-control" />
                                        </div>

                                        <div class="col-md-2">
                                            <label>% Reajuste</label>
                                            <asp:TextBox ID="txtPercentual" runat="server" CssClass="form-control" />
                                        </div>

                                        <div class="col-md-2">
                                            <label>Tipo</label>
                                            <asp:DropDownList ID="ddlTipoReajuste" runat="server" CssClass="form-control">
                                                <asp:ListItem Text="Percentual" Value="P" />
                                                <asp:ListItem Text="Valor Fixo +" Value="V" />
                                            </asp:DropDownList>
                                        </div>

                                        <div class="col-md-2 d-flex align-items-end">
                                            <asp:Button ID="btnSimular"
                                                runat="server"
                                                Text="Simular"
                                                CssClass="btn btn-info w-100"
                                                OnClick="btnSimular_Click" />
                                        </div>

                                    </div>

                                    <hr />

                                    <asp:GridView ID="gvSimulacao"
                                        runat="server"
                                        CssClass="table table-bordered"
                                        AutoGenerateColumns="true" />

                                    <div class="text-right mt-3">
                                        <asp:Button ID="btnAplicar"
                                            runat="server"
                                            Text="Aplicar Reajuste"
                                            CssClass="btn btn-success"
                                            OnClick="btnAplicar_Click" />
                                    </div>

                                </div>
                            </div>


                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>


</asp:Content>
