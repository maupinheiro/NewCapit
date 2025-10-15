<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="Frm_CadPedidos.aspx.cs" Inherits="NewCapit.dist.pages.Frm_CadPedidos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    <script type="text/javascript">
        function apenasNumeros(e) {
            var charCode = (e.which) ? e.which : e.keyCode;
            // Permite backspace, delete, setas e números (48-57)
            if (
                charCode > 31 && (charCode < 48 || charCode > 57)
            ) {
                return false;
            }
            return true;
        }
    </script>
    <script>
        function formatarData(campo, e) {
            var tecla = e.keyCode;
            var valor = campo.value.replace(/\D/g, '');

            if (valor.length >= 2 && valor.length <= 4)
                campo.value = valor.substring(0, 2) + '/' + valor.substring(2);
            else if (valor.length > 4)
                campo.value = valor.substring(0, 2) + '/' + valor.substring(2, 4) + '/' + valor.substring(4, 8);
        }
    </script>

    <div class="content-wrapper">
        <!-- Main content -->
        <section class="content">

            <div class="container-fluid">
                <br />
                <div id="toastContainerVermelho" class="alert alert-danger alert-dismissible" style="display: none;">
                    <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                    <h5><i class="icon fas fa-exclamation-triangle"></i>Alerta!</h5>
                    Alertas
                </div>
                <div id="toastContainer" class="alert alert-warning alert-dismissible" style="display: none;">
                    <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                    <h5><i class="icon fas fa-exclamation-triangle"></i>Alerta!</h5>
                    Alertas
                </div>
                <div class="card card-info">
                    <div class="card-header">
                        <h3 class="card-title"><i class="fas fa-clipboard-list"></i>&nbsp;CARGAS - NOVA CARGA:
                            <asp:Label ID="novaCarga" runat="server"></asp:Label></h3>
                    </div>
                </div>
                <div class="card-header">
                    <div class="form-group row">
                        <label for="inputFilial" class="col-sm-1 col-form-label" style="text-align: right">SOLICITANTE:</label>
                        <div class="col-sm-3">
                            <asp:DropDownList ID="cbSolicitantes" runat="server" CssClass="form-control select2"></asp:DropDownList>
                        </div>

                        <label for="inputFilial" class="col-sm-1 col-form-label" style="text-align: right">GR:</label>
                        <div class="col-sm-2">
                            <asp:DropDownList ID="cboGR" runat="server" CssClass="form-control select2"></asp:DropDownList>
                        </div>

                        <label for="inputFilial" class="col-sm-2 col-form-label" style="text-align: right">CADASTRO:</label>
                        <div class="col-md-2">
                            <div class="form-group">
                                <asp:TextBox ID="txtCadastro" runat="server" CssClass="form-control" Style="text-align: center"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="form-group row">
                        <label for="inputFilial" class="col-sm-1 col-form-label" style="text-align: right">PAGADOR/ROTA:</label>
                        <div class="col-sm-1">
                            <asp:TextBox ID="txtFrete" runat="server" CssClass="form-control" Style="text-align: center" OnTextChanged="txtFrete_TextChanged" AutoPostBack="true"></asp:TextBox>
                        </div>
                        <div class="col-md-10">
                            <asp:DropDownList ID="cboFrete" runat="server" CssClass="form-control select2" AutoPostBack="true" OnSelectedIndexChanged="cboFrete_SelectedIndexChanged"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="card card-outline card-info">
                        <div class="card-header">
                            <h3 class="card-title"><i class="far fa-edit"></i>&nbsp;Dados da Coleta/Entrega</h3>
                            <div class="card-tools">
                                <button type="button" class="btn btn-tool" data-card-widget="collapse">
                                    <i class="fas fa-minus"></i>
                                </button>
                            </div>
                            <!-- /.card-tools -->
                        </div>
                        <div class="card-body">
                            <div class="item-coleta">
                                <!-- REMETENTE -->
                                <div class="form-group row">
                                    <label for="inputRemetente" class="col-sm-1 col-form-label" style="text-align: right">REMETENTE:</label>
                                    <div class="col-md-1">
                                        <asp:TextBox ID="txtCodRemetente" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <div class="col-md-5">
                                        <asp:TextBox ID="cboRemetente" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtMunicipioRemetente" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <div class="col-md-1">
                                        <asp:TextBox ID="txtUFRemetente" Style="text-align: center" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>

                                </div>
                                <!-- EXPEDIDOR -->
                                <div class="form-group row">
                                    <label for="inputExpedidor" class="col-sm-1 col-form-label" style="text-align: right">EXPEDIDOR:</label>
                                    <div class="col-md-1">
                                        <asp:TextBox ID="txtCodExpedidor" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <div class="col-md-5">
                                        <asp:TextBox ID="cboExpedidor" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtCidExpedidor" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <div class="col-md-1">
                                        <asp:TextBox ID="txtUFExpedidor" Style="text-align: center" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>
                                <!-- DESTINATARIO -->
                                <div class="form-group row">
                                    <label for="inputDestinatario" class="col-sm-1 col-form-label" style="text-align: right">DEST.:</label>
                                    <div class="col-md-1">
                                        <asp:TextBox ID="txtCodDestinatario" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <div class="col-md-5">
                                        <asp:TextBox ID="cboDestinatario" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtMunicipioDestinatario" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <div class="col-md-1">
                                        <asp:TextBox ID="txtUFDestinatario" Style="text-align: center" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>
                                <!-- RECEBEDOR -->
                                <div class="form-group row">
                                    <label for="inputRecebedor" class="col-sm-1 col-form-label" style="text-align: right">RECEBEDOR:</label>
                                    <div class="col-md-1">
                                        <asp:TextBox ID="txtCodRecebedor" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <div class="col-md-5">
                                        <asp:TextBox ID="cboRecebedor" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtCidRecebedor" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <div class="col-md-1">
                                        <asp:TextBox ID="txtUFRecebedor" Style="text-align: center" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>
                                <!-- CONSIGNATARIO -->
                                <div class="form-group row">
                                    <label for="inputConsignatario" class="col-sm-1 col-form-label" style="text-align: right">CONSIG.:</label>
                                    <div class="col-md-1">
                                        <asp:TextBox ID="txtCodConsignatario" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <div class="col-md-5">
                                        <asp:TextBox ID="txtConsignatario" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtCidConsignatario" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <div class="col-md-1">
                                        <asp:TextBox ID="txtUFConsignatario" Style="text-align: center" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>
                                <!-- PAGADOR -->
                                <div class="form-group row">
                                    <label for="inputPagador" class="col-sm-1 col-form-label" style="text-align: right">PAGADOR:</label>
                                    <div class="col-md-1">
                                        <asp:TextBox ID="txtCodPagador" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <div class="col-md-5">
                                        <asp:TextBox ID="txtPagador" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtCidPagador" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <div class="col-md-1">
                                        <asp:TextBox ID="txtUFPagador" Style="text-align: center" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row g-3">
                        <div class="col-md-2">
                            <label for="inputFilial" style="text-align: right">FILIAL:</label>
                            <div class="form-group">
                                <asp:TextBox ID="txtFilial" runat="server" CssClass="form-control" Style="text-align: left" ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <label for="inputFilial" style="text-align: right">TIPO DE VEÍCULO:</label>
                            <div class="form-group">
                                <asp:TextBox ID="txtTipoVeiculo" runat="server" CssClass="form-control" Style="text-align: left" ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <label for="inputFilial" style="text-align: right">DIST.(KM):</label>
                            <div class="form-group">
                                <asp:TextBox ID="txtDistancia" runat="server" CssClass="form-control" Style="text-align: center" ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <label for="inputFilial" style="text-align: right">DESLOCAMENTO:</label>
                            <div class="form-group">
                                <asp:TextBox ID="txtDeslocamento" runat="server" CssClass="form-control" Style="text-align: center" ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <label for="inputFilial" style="text-align: right">TEMPO:</label>
                            <div class="form-group">
                                <asp:TextBox ID="txtDuracao" runat="server" CssClass="form-control" Style="text-align: center" ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <label for="inputFilial" style="text-align: right">EMITE PEDAGIO:</label>
                            <div class="form-group">
                                <asp:TextBox ID="txtPedagio" runat="server" CssClass="form-control" Style="text-align: center" ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="row g-3">
                        <div class="col-md-12">
                            <label for="inputFilial" style="text-align: right">OBSERVAÇÃO:</label>
                            <div class="form-group">
                                <asp:TextBox ID="txtObservacao" runat="server" CssClass="form-control" Style="text-align: left"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="row g-3">
                        <div class="col-md-1">
                            <div class="form-group">
                                <label for="inputFilial" style="text-align: right">PEDIDO:</label>
                                <asp:TextBox ID="txtNumPedido" onkeypress="return apenasNumeros(event);" class="form-control" runat="server" OnTextChanged="txtNumPedido_TextChanged" AutoPostBack="true"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form-group">
                                <label for="inputFilial" style="text-align: right">MATERIAL:</label>
                                <asp:DropDownList ID="cboMaterial" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form-group">
                                <label for="inputFilial" style="text-align: right">PESO:</label>
                                <asp:TextBox ID="txtPeso" runat="server" onkeypress="return apenasNumeros(event);" CssClass="form-control" value=""></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form-group">
                                <label for="inputFilial" style="text-align: right">DEPOSITO:</label>
                                <asp:DropDownList ID="cboDeposito" runat="server" CssClass="form-control select2"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form-group">
                                <label for="inputFilial" style="text-align: right">SITUAÇÃO:</label>
                                <asp:DropDownList ID="cboSituacao" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="PRONTO" Text="PRONTO"></asp:ListItem>
                                    <asp:ListItem Value="EM PROCESSO" Text="EM PROCESSO"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form-group">
                                <label for="inputFilial" style="text-align: right">DT/OT/TU:</label>
                                <asp:TextBox ID="txtControleCliente" runat="server" CssClass="form-control" value=""></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form-group">
                                <label for="inputFilial" style="text-align: right">PREV. ENTREGA:</label>
                                <asp:TextBox ID="txtPrevEntrega" runat="server" Style="text-align: center" CssClass="form-control" MaxLength="10" onkeyup="formatarData(this, event);"></asp:TextBox>
                                <asp:RegularExpressionValidator
                                    ID="revData"
                                    runat="server"
                                    ControlToValidate="txtPrevEntrega"
                                    ErrorMessage="Data inválida"
                                    ValidationExpression="^(0[1-9]|[12][0-9]|3[01])/(0[1-9]|1[0-2])/\d{4}$"
                                    ForeColor="Red"
                                    Display="Dynamic" />
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form-group">
                                <label for="inputFilial" style="text-align: right">ENTREGA:</label>
                                <asp:DropDownList ID="cboEntrega" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="NORMAL" Text="NORMAL"></asp:ListItem>
                                    <asp:ListItem Value="IMEDIATA" Text="IMEDIATA"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <br />
                        <br />
                        <div class="col-md-1">
                            <div class="form-group">
                                <label for="inputFilial" style="text-align: right">&emsp;</label>
                                <asp:Button ID="btnAdicionar" runat="server" CssClass="btn btn-primary btn-sm" Text="Adicionar" OnClick="btnAdicionar_Click" />
                            </div>
                        </div>
                    </div>


                    <div class="row">
                        <div class="col-12">
                            <div class="card">
                                <div class="card-header">
                                    <h3 class="card-title">PEDIDOS:</h3>
                                </div>
                                <!-- /.card-header -->
                                <div class="card-body table-responsive p-0" style="height: 260px;">
                                    <table class="table table-head-fixed text-nowrap">
                                        <asp:GridView ID="gvPedidos" runat="server" DataKeyNames="pedido" AutoGenerateColumns="False" CssClass="table table-striped" EmptyDataText="Nenhum pedido encontrado." OnRowDataBound="gvPedidos_RowDataBound" ShowFooter="True">
                                            <Columns>
                                                <asp:TemplateField HeaderText="" ShowHeader="True" ItemStyle-Width="9">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkExcluir" runat="server" OnClick="lnkExcluir_Click" CssClass="btn btn-danger btn-sm"><i class="fa fa-trash"></i></i>
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="pedido" HeaderText="Pedido" />
                                                <asp:BoundField DataField="portao" HeaderText="Deposito" />
                                                <asp:BoundField DataField="peso" HeaderText="Peso" />
                                                <asp:BoundField DataField="material" HeaderText="Material" />
                                                <asp:BoundField DataField="cliorigem" HeaderText="Remetente" />
                                                <asp:BoundField DataField="clidestino" HeaderText="Destinatário" />
                                                <asp:BoundField DataField="situacao" HeaderText="Status" />
                                                <asp:BoundField DataField="previsao" HeaderText="Prev. Entrega" DataFormatString="{0:dd/MM/yyyy}" />
                                                <%--<asp:BoundField DataField="ValorTotal" HeaderText="Valor Total" DataFormatString="{0:C}" />--%>
                                            </Columns>
                                        </asp:GridView>
                                    </table>
                                </div>
                                <!-- /.card-body -->
                            </div>
                            <!-- /.card -->
                        </div>
                    </div>

                    <div class="row g-3">
                        <div class="col-md-6">
                            <div class="form-group">
                                <span class="details">CADASTRADO POR:</span>
                                <asp:TextBox ID="txtUsuCadastro" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-md-6">
                            <div class="form-group">
                                <span class="details">ÚLTIMA ATUALIZAÇÃO:</span>
                                <asp:TextBox ID="txtUsuAlteracao" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                    <div class="row g-3">
                        <div class="col-md-1">
                            <asp:Button ID="btnSalvar" runat="server" CssClass="btn btn-outline-success btn-lg" Text="Salvar" OnClick="btnSalvar_Click" />
                        </div>
                        <div class="col-md-1">
                            <a href="ConsultaColetasCNT.aspx" class="btn btn-outline-danger btn-lg">Fechar               
                            </a>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>
</asp:Content>

