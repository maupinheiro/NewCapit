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

    <%--<script>
        $(document).ready(function () {
            $('#<%= gvCargas.ClientID %>').DataTable({
                language: {
                    url: "//cdn.datatables.net/plug-ins/1.13.6/i18n/pt-BR.json"
                }
            });
        });
    </script>--%>

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
                                <h3 class="card-title"><i class="fas fa-shipping-fast"></i>&nbsp;GESTÃO DE COLETAS E ENTREGAS</h3>
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
                                <%--<div class="col-md-2">
                                    <label>Status:</label>
                                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                                        <asp:ListItem Text="Todas" Value="" />
                                        <asp:ListItem Text="Pendente" Value="Pendente" />
                                        <asp:ListItem Text="Concluído" Value="Concluído" />
                                    </asp:DropDownList>
                                </div>--%>
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
                                    <a href="/dist/pages/Frm_ColetaMatriz.aspx" class="d-none d-sm-inline-block btn btn-primary shadow-sm"><i
                                        class="fas fa-boxes"></i>&nbsp;Nova Coleta/Entrega
                                    </a>
                                </div>
                                <div class="col-md-2">
                                    <label>&nbsp;</label><br />
                                    <a href="/dist/pages/ColetasMatriz.aspx" class="d-none d-sm-inline-block btn btn-primary shadow-sm"><i
                                        class="fas fa-boxes"></i>&nbsp;Coleta
                                    </a>
                                </div>
                                <div class="col-md-3">
                                    <div class="form-group">
                                        <div class="custom-control custom-switch custom-switch-off-primary custom-switch-on-success">
                                            <input type="checkbox" class="custom-control-input" id="customSwitch3">
                                            <label class="custom-control-label" style="text-align: center" for="customSwitch3">Ocultar viagens CONCLUÍDAS</label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="container-fluid">
                                <%--<table id="example1" class="table table-bordered table-striped table-hover table-responsive">
                                    <asp:GridView runat="server" ID="gvCargas" CssClass="table table-bordered table-striped table-hover" Width="100%" AutoGenerateColumns="False" DataKeyNames="id" AllowPaging="True" PageSize="40" OnPageIndexChanging="gvCargas_PageIndexChanging" ShowHeaderWhenEmpty="True">
                                        <PagerStyle HorizontalAlign="Center" CssClass="pagination-centered" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" ShowHeader="True" ItemStyle-Width="9">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkEditar" runat="server" OnClick="Editar" CssClass="btn btn-info btn-sm"><i class="fas fa-edit"></i></i>
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Id" HeaderText="ID" Visible="false" />
                                            <asp:BoundField DataField="veiculo" HeaderText="Veículo" />                                            
                                            <asp:BoundField DataField="placa" HeaderText="Placa" />
                                            <asp:BoundField DataField="num_carregamento" HeaderText="Carga" />
                                            <asp:BoundField DataField="" HeaderText="foto" />
                                            <asp:BoundField DataField="codmotorista" HeaderText="Código"/>
                                            <asp:BoundField DataField="nomemotorista" HeaderText="Nome do Motorista"/>
                                            <asp:BoundField DataField="carga" HeaderText="Remetente/Expedidor" />
                                            <asp:BoundField DataField="carga" HeaderText="Destinatário/Recebedor"/>
                                            <asp:BoundField DataField="emissao" HeaderText="Abertura" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                                            <asp:BoundField DataField="peso" HeaderText="Situacao" />
                                            <asp:BoundField DataField="" HeaderText="Status" />
                                            <asp:BoundField DataField="previsao" HeaderText="Previsão" DataFormatString="{0:dd/MM/yyyy}" />
                                            <%--<asp:BoundField DataField="Valor" HeaderText="Valor" DataFormatString="{0:C}" />--%>
                                <%--<asp:BoundField DataField="cliorigem" HeaderText="Remetente" />
                                            <asp:BoundField DataField="cidorigem" HeaderText="Município" />
                                            <asp:BoundField DataField="clidestino" HeaderText="Destinatário" />
                                            <asp:BoundField DataField="ciddestino" HeaderText="Município" />
                                        </Columns>
                                    </asp:GridView>--%>

                                <div class="row">
                                    <div class="col-12">
                                        <!-- /.col -->
                                        <div class="card">
                                            <!-- /.card-header -->
                                            <div class="card-body">
                                                <asp:Label ID="lblMensagem" runat="server" Text=""></asp:Label>
                                                <asp:Repeater ID="rptCarregamento" runat="server" OnItemDataBound="rptCarregamento_ItemDataBound">
                                                    <HeaderTemplate>
                                                        <table class="table table-bordered table-hover">
                                                            <thead>
                                                                <tr>
                                                                    <th>AÇÃO</th>
                                                                    <th>VEÍCULO</th>
                                                                    <th>PLACA</th>
                                                                    <th>MOTORISTA</th>
                                                                    <th>ORDEM COLETA</th>
                                                                    <th>ATENDIMENTO</th>
                                                                    <th>PROGRESSO</th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <tr onclick="toggleDetalhes(this)">
                                                            <td style="text-align: center; vertical-align: middle;">
                                                                <asp:ImageButton ID="lnkEditar" ImageUrl='<%# Eval("fotos") %>' style="width: 45px;"  CommandName="Editar"
                                                                 CommandArgument='<%# Eval("num_carregamento") %>'
                                                                 OnCommand="lnkEditar_Command"
                                                                 OnClientClick="event.stopPropagation();"  runat="server" />

                                                              
                                                            </td>
                                                            <td>
                                                                <%# Eval("veiculo") %>
                                                                <br />
                                                                <%# Eval("tipoveiculo") %>
                                                            </td>
                                                            <td>
                                                                <%# Eval("placa") %>
                                                                <br />
                                                                <%# Eval("reboque1")  + " - " +  Eval("reboque2") %>

                                                            </td>
                                                          
                                                            <td>
                                                                <%# Eval("codmotorista") + " - " + Eval("nomemotorista") %>
                                                                <br />
                                                                <%# Eval("codtra") + " - " + Eval("transportadora")%>  
                                                            </td>
                                                            <td style="text-align: center; vertical-align: middle;">
                                                                <%# Eval("num_carregamento") %>
                                                                <br />
                                                                <%# Eval("emissao", "{0:dd/MM/yyyy HH:mm}") %>
                                                            </td>
                                                            <td>
                                                                <%# Eval("situacao") %>
                                                                <br />
                                                                <%# Eval("status")%>  
                                                            </td>
                                                            <td>
                                                                <div class="progress progress-xs">
                                                                    <div class="progress-bar progress-bar-danger" style="width: 55%"></div>
                                                                </div>
                                                                <span class="badge bg-danger">55%</span>
                                                            </td>

                                                        </tr>
                                                        <tr class="detalhes d-none">
                                                            <td colspan="11">
                                                                <asp:Repeater ID="rptColeta" OnItemDataBound="rptColeta_ItemDataBound" runat="server">
                                                                    <HeaderTemplate>
                                                                        <table class="table table-bordered table-hover mb-0">
                                                                            <thead>
                                                                                <tr>
                                                                                    <th>Carga</th>
                                                                                    <th>Status</th>
                                                                                    <th>Remetente/Expedidor</th>
                                                                                    <th>Destinatário/Recebedor</th>
                                                                                    <th>Saída</th>
                                                                                    <th>Prev.Chegada</th>
                                                                                    <th>Chegada</th>
                                                                                    <th>Conclusão</th>
                                                                                </tr>
                                                                            </thead>
                                                                            <tbody>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <tr>
                                                                            <td>
                                                                                <%# Eval("carga") %>
                                                                                <br />
                                                                                <%# Eval("emissao", "{0:dd/MM/yyyy HH:mm}") %>
                                                                            </td>
                                                                            <td>
                                                                                <%# Eval("status") %>
                                                                                <br />
                                                                                02h30min
                                                                            </td>
                                                                            <td>
                                                                                <%# Eval("codorigem") %> + " - " + <%# Eval("cliorigem") %>
                                                                                <br />
                                                                                <%# Eval("cod_expedidor") %> + " - " + <%# Eval("expedidor") %>
                                                                            </td>
                                                                            <td>
                                                                                <%# Eval("coddestino") %> + " - " + <%# Eval("clidestino") %>
                                                                                <br />
                                                                                <%# Eval("cod_recebedor") %> + " - " + <%# Eval("recebedor") %>
                                                                            </td>
                                                                            <td><%# Eval("inicio_viagem", "{0:dd/MM/yyyy HH:mm}") %></td>
                                                                            <td><%# Eval("prev_chegada", "{0:dd/MM/yyyy HH:mm}") %></td>
                                                                            <td><%# Eval("cheg_cliente", "{0:dd/MM/yyyy HH:mm}") %></td>
                                                                            <td><%# Eval("fim_viagem", "{0:dd/MM/yyyy HH:mm}") %></td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        </tbody>
                                        </table>
                                                                    </FooterTemplate>
                                                                </asp:Repeater>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        </tbody>
                        </table>
                                                    </FooterTemplate>
                                                </asp:Repeater>



                                            </div>
                                            <!-- /.card-body -->
                                        </div>
                                        <!-- /.card -->
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
