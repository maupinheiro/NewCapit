<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="Indicadores.aspx.cs" Inherits="NewCapit.dist.pages.Indicadores" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="content-wrapper">
        <section class="content-header">
            <div class="container-fluid">
                <div class="card card-primary card-outline">
                    <div class="card-body">
                        <div class="row">
                            <div class="col-md-3">
                                <label>Início:</label>
                                <asp:TextBox ID="txtDataInicio" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                            </div>
                            <div class="col-md-3">
                                <label>Fim:</label>
                                <asp:TextBox ID="txtDataFim" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <label>Empresa (Núcleo):</label>
                                <asp:DropDownList ID="ddlEmpresa" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <label>&nbsp;</label>
                                <asp:LinkButton ID="btnFiltrar" runat="server" CssClass="btn btn-primary btn-block" OnClick="btnFiltrar_Click">
                                    <i class="fas fa-sync"></i> Atualizar
                                </asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-lg-2 col-6">
                        <div class="small-box bg-success"><div class="inner"><h3><asp:Literal ID="litRealizadas" runat="server" /></h3><p>Realizadas</p></div></div>
                    </div>
                    <div class="col-lg-2 col-6">
                        <div class="small-box bg-warning"><div class="inner"><h3><asp:Literal ID="litPendentes" runat="server" /></h3><p>Pendentes</p></div></div>
                    </div>
                    <div class="col-lg-2 col-6">
                        <div class="small-box bg-info"><div class="inner"><h3><asp:Literal ID="litAndamento" runat="server" /></h3><p>Em Andamento</p></div></div>
                    </div>
                    <div class="col-lg-3 col-6">
                        <div class="small-box bg-danger"><div class="inner"><h3><asp:Literal ID="litAtrasadas" runat="server" /></h3><p>Atrasadas</p></div></div>
                    </div>
                    <div class="col-lg-3 col-6">
                        <div class="small-box bg-dark"><div class="inner"><h3><asp:Literal ID="litFaturamentoTotal" runat="server" /></h3><p>Faturamento Total</p></div></div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-6">
                        <div class="card card-primary"><div class="card-header"><h3 class="card-title">Ranking Faturamento por Veículo</h3></div>
                        <div class="card-body"><canvas id="chartFatVeiculo" style="height:300px;"></canvas></div></div>
                    </div>
                    <div class="col-md-6">
                        <div class="card card-success"><div class="card-header"><h3 class="card-title">Top 10 Motoristas (Cargas)</h3></div>
                        <div class="card-body"><canvas id="chartMotoristas" style="height:300px;"></canvas></div></div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-4">
                        <div class="card card-info"><div class="card-header"><h3 class="card-title">Tipo de Veículo</h3></div>
                        <div class="card-body"><canvas id="chartTipoVeiculo" style="height:300px;"></canvas></div></div>
                    </div>
                    <div class="col-md-4">
                        <div class="card card-warning"><div class="card-header"><h3 class="card-title">Principais Rotas</h3></div>
                        <div class="card-body"><canvas id="chartRotasFull" style="height:300px;"></canvas></div></div>
                    </div>
                    <div class="col-md-4">
                        <div class="card card-secondary"><div class="card-header"><h3 class="card-title">Cidades Destino</h3></div>
                        <div class="card-body"><canvas id="chartCidades" style="height:300px;"></canvas></div></div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-6">
                        <div class="card card-outline card-primary"><div class="card-header"><h3 class="card-title">Top Recebedores</h3></div>
                        <div class="card-body"><canvas id="chartRecebedor" style="height:300px;"></canvas></div></div>
                    </div>
                    <div class="col-md-6">
                        <div class="card card-outline card-success"><div class="card-header"><h3 class="card-title">Top Pagadores</h3></div>
                        <div class="card-body"><canvas id="chartPagador" style="height:300px;"></canvas></div></div>
                    </div>
                </div>
            </div>
        </section>
    </div>
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
</asp:Content>

