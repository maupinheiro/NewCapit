<%@ Page Title="" Language="C#" MasterPageFile="Main.Master" AutoEventWireup="true" CodeBehind="ConsultaColetasCNT.aspx.cs" Inherits="NewCapit.dist.pages.ConsultaColetasCNT" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- Bootstrap CSS -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/css/bootstrap.min.css" rel="stylesheet" />
    <!-- jQuery e Bootstrap JS -->
    <script src="https://code.jquery.com/jquery-3.6.4.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/js/bootstrap.bundle.min.js"></script>
    <!-- Bootstrap CSS + JS -->
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" />
    <script src="https://code.jquery.com/jquery-3.5.1.slim.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@4.5.2/dist/js/bootstrap.bundle.min.js"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">

    <div class="content-wrapper">
        <!-- Main content -->
        <section class="content">
            <div class="container-fluid">
                <div class="content-header">
                    <div class="row g-3">
                        <div class="col-md-9">
                            <h1 class="h3 mb-2 text-gray-800">
                                <i class="fas fa-shipping-fast"></i>&nbsp;Coletas</h1>
                        </div>
                        <%-- <div class="col-md-1 text-center">
                            <input type="text" class="knob" value="70" data-width="90" data-height="90" data-fgcolor="#f56954">
                            <div class="knob-label
                                ">
                                Atrasadas
                            </div>
                        </div>
                        <div class="col-md-1 text-center">
                            <input type="text" class="knob" value="80" data-width="90" data-height="90" data-fgcolor="#3c8dbc">
                            <div class="knob-label">No Prazo</div>
                        </div>
                        <div class="col-1 text-center">
                            <input type="text" class="knob" value="90" data-width="90" data-height="90" data-fgcolor="#932ab6">
                            <div class="knob-label">Antecipadas</div>
                        </div>--%>
                    </div>
                </div>

                <div class="row g-3">
                    <div class="col-md-2">
                        <div class="form-group">
                            <span class="">Coleta Inicial:</span>
                            <div class="input-group date">
                                <asp:TextBox ID="txtInicioData" CssClass="form-control" TextMode="DateTimeLocal" runat="server"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <div class="form-group">
                            <span class="">Coleta Final:</span>
                            <div class="input-group date">
                                <asp:TextBox ID="txtFimData" CssClass="form-control" TextMode="DateTimeLocal" runat="server"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <%-- <div class="col-md-2">
                        <div class="form-group">
                            <span class="">Status:</span>
                            <asp:DropDownList ID="ddlStatus" name="nomeStatus" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <div class="form_group">
                            <span class="details">VEÍCULO:</span>
                            <asp:DropDownList ID="ddlVeiculos" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>--%>
                    <div class="col-md-2">
                        <br />
                        <asp:LinkButton ID="lnkPesquisar" runat="server" CssClass="btn btn-warning" OnClick="lnkPesquisar_Click"><i class='fas fa-search' ></i>  Pesquisar</asp:LinkButton>
                    </div>

                </div>
                <div class="row g-3">
                    <div class="card-header">
                    <asp:TextBox ID="myInput" CssClass="form-control myInput" OnTextChanged="myInput_TextChanged" placeholder="Pesquisar ..." AutoPostBack="true" runat="server"></asp:TextBox>
                </div>
                </div>
            </div>
            <!-- /.container-fluid -->
        </section>
        <!-- Main content -->

        <section class="content">
            
            <div class="container-fluid">
                <div class="row">
                    <div class="col-12">
                        <!-- /.col -->
                        <div class="card">
                            <!-- /.card-header -->
                            <div class="card-body table-responsive p-0" style="height: 590px;font-size:smaller;">
                                <table class="table table-head-fixed text-nowrap">
                                    <asp:GridView runat="server" ID="gvListCargas" CssClass="table table-bordered table-striped table-hover" Width="100%" AutoGenerateColumns="False" DataKeyNames="carga" OnRowDataBound="gvListCargas_RowDataBound" OnRowCommand="gvListCargas_RowCommand">

                                        <Columns>
                                            <asp:BoundField DataField="id" HeaderText="ID" Visible="false" />
                                            <asp:BoundField DataField="carga" HeaderText="COLETA" Visible="false" />
                                            <asp:BoundField DataField="cva" HeaderText="CVA" />
                                            <asp:BoundField DataField="data_hora" HeaderText="DATA/HORA" />
                                            <asp:BoundField DataField="solicitacoes" HeaderText="SOLICITAÇÃO(ES)" />
                                            <asp:BoundField DataField="atendimento" HeaderText="ATENDIMENTO" />
                                            <asp:BoundField DataField="cliorigem" HeaderText="LOCAL DA COLETA" />
                                            <asp:BoundField DataField="clidestino" HeaderText="DESTINO" />
                                            <asp:BoundField DataField="veiculo" HeaderText="VEICULO" />
                                            <asp:BoundField DataField="tipo_viagem" HeaderText="VIAGEM" />
                                            <asp:BoundField DataField="rota" HeaderText="ROTA" />
                                            <asp:BoundField DataField="andamento" HeaderText="SITUAÇÃO" Visible="false"/>

                                            <asp:TemplateField HeaderText="AÇÕES" ShowHeader="True">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="btnDetalhes" runat="server" Text="" CommandName="MostrarDetalhes" CommandArgument='<%# Eval("Id") %>' class="btn btn-info"><i class="fas fa-list"></i></asp:LinkButton>
                                                    <asp:LinkButton ID="lnkEditar" runat="server" OnClick="Editar" CssClass="btn btn-primary"><i class="fas fa-edit"></i></asp:LinkButton>
                                                    <asp:LinkButton ID="lnkExcluir" runat="server" class="btn btn-danger" data-toggle="modal" data-target="#modal-xl"><i class="fas fa-trash-alt"></i></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </table>
                                <asp:HiddenField ID="txtconformmessageValue" runat="server" />
                            </div>
                            <!-- /.card-body -->
                        </div>
                        <!-- /.card -->
                    </div>
                </div>
                <!-- /.row -->
            </div>
            <!-- Modal Detalhes da Coleta -->
            <div class="modal fade" id="infoModal" tabindex="-1" role="dialog">
                <div class="modal-dialog modal-xl" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title">Detalhes da Coleta</h5>
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                        </div>
                        <div class="modal-body">
                            <!-- Linha 1 do formulario modal -->
                            <div class="row g-3">
                                <div class="col-md-10">
                                </div>
                                <div class="col-md-2">
                                    <div class="form-group">
                                        <asp:Label ID="lblStatus" runat="server" class="form-control font-weight-bold" Style="text-align: center">  
                                        </asp:Label>
                                    </div>
                                </div>
                            </div>
                            <ul class="nav nav-tabs" id="tabMenu" visible="false">
                                <li class="nav-item">
                                    <a class="nav-link active" data-toggle="tab" href="#tab1">Dados da Coleta</a>
                                </li>
                                <li class="nav-item" id="liTab2" runat="server">
                                    <a class="nav-link" data-toggle="tab" href="#tab2">Dados do Motorista e Veículo</a>
                                </li>
                                <li class="nav-item" id="liTab3" runat="server">
                                    <a class="nav-link" data-toggle="tab" href="#tab3">Dados do Carregamento</a>
                                </li>
                                <li class="nav-item">
                                    <a class="nav-link" data-toggle="tab" href="#tab4">Ocorrências</a>
                                </li>
                            </ul>
                            <div class="tab-content mt-4">
                                <!-- aba 1 dados da coleta ok -->
                                <div class="tab-pane container active" id="tab1">
                                    <!-- Linha 2 do formulario modal -->
                                    <div class="row g-3">
                                        <div class="col-md-2">
                                            <div class="form_group">
                                                <span class="details">COLETA:</span>
                                                <asp:Label ID="txtColeta" runat="server" CssClass="form-control font-weight-bold"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <span class="">FILIAL:</span>
                                                <asp:Label ID="txtFilial" runat="server" class="form-control font-weight-bold">  
                                                </asp:Label>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <span class="">PLANTA:</span>
                                                <asp:Label ID="txtPlanta" runat="server" CssClass="form-control font-weight-bold"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <span class="details">TIPO DE VEÍCULO:</span>
                                                <asp:TextBox ID="txtTipoVeiculo" runat="server" class="form-control font-weight-bold"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <!-- Linha 3 do formulario modal -->
                                    <div class="row g-3">
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <span class="details">CÓDIGO:</span>
                                                <asp:TextBox ID="txtCodCliOrigem" runat="server" class="form-control font-weight-bold"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form_group">
                                                <span class="details">ORIGEM:</span>
                                                <asp:TextBox ID="txtRemetente" runat="server" class="form-control font-weight-bold"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-5">
                                            <div class="form-group">
                                                <span class="details">MUNICÍPIO:</span>
                                                <asp:TextBox ID="txtMunicOrigem" runat="server" class="form-control font-weight-bold"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-1">
                                            <div class="form-group">
                                                <span class="details">UF:</span>
                                                <asp:TextBox ID="txtUFOrigem" runat="server" class="form-control font-weight-bold" Style="text-align: center"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <!-- Linha 4 do formulario modal -->
                                    <div class="row g-3">
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <span class="details">CÓDIGO:</span>
                                                <asp:TextBox ID="txtCodCliDestino" runat="server" class="form-control font-weight-bold"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form_group">
                                                <span class="details">DESTINO:</span>
                                                <asp:TextBox ID="txtDestinatario" runat="server" class="form-control font-weight-bold"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-5">
                                            <div class="form-group">
                                                <span class="details">MUNICÍPIO:</span>
                                                <asp:TextBox ID="txtMunicDestinatario" runat="server" class="form-control font-weight-bold"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-1">
                                            <div class="form-group">
                                                <span class="details">UF:</span>
                                                <asp:TextBox ID="txtUFDestinatario" runat="server" class="form-control font-weight-bold" Style="text-align: center"></asp:TextBox>
                                            </div>
                                        </div>

                                    </div>
                                    <!-- Linha 5 do formulário modal -->
                                    <div class="row g-3">
                                        <div class="col-md-3">
                                            <div class="form_group">
                                                <span class="details">DATA COLETA:</span>
                                                <asp:TextBox ID="lblDataColeta" runat="server" CssClass="form-control font-weight-bold" Style="text-align: center">
                                                </asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-5">
                                            <div class="form_group">
                                                <span class="details">SOLICITAÇÃO(ÕES):</span>
                                                <asp:TextBox ID="lblSolicitacoes" runat="server" CssClass="form-control font-weight-bold">
                                                </asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form_group">
                                                <span class="details">VIAGEM:</span>
                                                <asp:TextBox ID="lblTipoViagem" runat="server" CssClass="form-control font-weight-bold">
                                                </asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <!-- Linha 6 do formulario modal -->
                                    <div class="row g-3">
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <span class="details">ROTA:</span>
                                                <asp:TextBox ID="txtRota" runat="server" class="form-control font-weight-bold"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <span class="details">PESO:</span>
                                                <asp:TextBox ID="txtPeso" runat="server" class="form-control font-weight-bold"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form_group">
                                                <span class="details">MET<sup>3</sup>:</span>
                                                <asp:TextBox ID="lblMetragem" runat="server" CssClass="form-control font-weight-bold">    
                                                </asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <span class="details">ESTUDO ROTA:</span>
                                                <asp:TextBox ID="txtEstudoRota" runat="server" class="form-control font-weight-bold"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <span class="details">REMESSA:</span>
                                                <asp:TextBox ID="txtRemessa" runat="server" class="form-control font-weight-bold"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <!-- Linha 7 do formulário modal -->
                                    <div class="row g-3">
                                        <div class="col-md-12">
                                            <div class="form_group">
                                                <span class="details">QUANTIDADE/PALLET´S:</span>
                                                <asp:TextBox ID="quantPallet" runat="server" class="form-control font-weight-bold" Rows="4" TextMode="MultiLine" placeholder="Quant./Pallet´s"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <!-- aba 2 dados do motorista e veiculo -->
                                <div class="tab-pane container fade" id="tab2">
                                    <div class="row g-3">
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <span class="details">ORDEM COLETA:</span>
                                                <asp:TextBox ID="txtOrdemColeta" runat="server" Style="text-align: center" class="form-control font-weight-bold"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <span class="details">FILIAL:</span>
                                                <asp:TextBox ID="txtFilialMot" runat="server" Style="text-align: center" class="form-control font-weight-bold"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <span class="details">TIPO:</span>
                                                <asp:TextBox ID="txtTipoMot" runat="server" Style="text-align: center" class="form-control font-weight-bold"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row g-3">
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <span class="details">MOTORISTA:</span>
                                                <asp:TextBox ID="txtCodMotorista" runat="server" Style="text-align: center" class="form-control font-weight-bold"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <span class="details">NOME COMPLETO:</span>
                                                <asp:TextBox ID="txtNomMot" runat="server" class="form-control font-weight-bold"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <span class="details">TRANSP.:</span>
                                                <asp:TextBox ID="txtCodTra" runat="server" Style="text-align: center" class="form-control font-weight-bold"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <span class="details">NOME/RAZÃO SOCIAL:</span>
                                                <asp:TextBox ID="txtTransp" runat="server" class="form-control font-weight-bold"></asp:TextBox>
                                            </div>
                                        </div>

                                    </div>
                                    <div class="row g-3">
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <span class="details">VEÍCULO USADO:</span>
                                                <asp:TextBox ID="txtVeiculoTipo" runat="server" Style="text-align: center" class="form-control font-weight-bold"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row g-3">
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <span class="details">VEICULO:</span>
                                                <asp:TextBox ID="txtCodVeiculo" runat="server" Style="text-align: center" class="form-control font-weight-bold"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <span class="details">PLACA:</span>
                                                <asp:TextBox ID="txtPlaca" runat="server" class="form-control font-weight-bold"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <span class="details">REBOQUE:</span>
                                                <asp:TextBox ID="txtReboque1" runat="server" class="form-control font-weight-bold"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <span class="details">REBOQUE:</span>
                                                <asp:TextBox ID="txtReboque2" runat="server" class="form-control font-weight-bold"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <span class="details">CARRETA(S):</span>
                                                <asp:TextBox ID="txtCarreta" runat="server" class="form-control font-weight-bold"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row g-3"></div>
                                </div>
                                <!-- aba 3 dados do carregamento -->
                                <div class="tab-pane container fade" id="tab3">
                                    <div class="row g-3">
                                    </div>
                                </div>
                                <!-- aba 4 ocorrencias ok -->
                                <div class="tab-pane container fade" id="tab4">
                                    <div class="row g-3">
                                        <div class="col-md-12">
                                            <div class="form_group">
                                                <span class="details">OCORRÊNCIAS:</span>
                                                <asp:TextBox ID="txtObservacao" runat="server" class="form-control font-weight-bold" Rows="8" TextMode="MultiLine" placeholder="Ocorrências ..."></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <!-- Linha 8 do formulário modal botão fechar -->
                            <div class="modal-footer justify-content-between">
                                <button type="button" class="btn btn-info" data-dismiss="modal">Fechar</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>
    <%--</div>

    </section>
        <!-- /.content -->
    </div>--%>
    <!-- /.content-wrapper -->

</asp:Content>
