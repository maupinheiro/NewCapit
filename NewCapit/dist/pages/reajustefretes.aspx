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

    <link href="https://cdn.jsdelivr.net/npm/select2@4.1.0/dist/css/select2.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/select2@4.1.0/dist/js/select2.min.js"></script>

    <script>
        $(document).ready(function () {
            $('.select2').select2({
                width: '100%',
                placeholder: "Selecione ou pesquise..."
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
                                    <h5>Reajuste Avançado de Frete</h5>
                                </div>

                                <div class="card-body">

                                    <div class="row">

                                        <!-- CLIENTE -->
                                        <div class="form-group row">
                                            <label for="inputRemetente" class="col-sm-1 col-form-label" style="text-align: right">CLIENTE:</label>
                                            <div class="col-md-1">
                                                <asp:TextBox ID="txtCodRemetente" runat="server" CssClass="form-control" OnTextChanged="txtCodRemetente_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="cboRemetente" runat="server" CssClass="form-control select2" OnSelectedIndexChanged="cboRemetente_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:TextBox ID="txtCNPJRemetente" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3">
                                                <asp:TextBox ID="txtMunicipioRemetente" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                            </div>
                                            <div class="col-md-1">
                                                <asp:TextBox ID="txtUFRemetente" Style="text-align: center" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                            </div>

                                        </div>
                                        <!-- PAGADOR -->
                                        <div class="form-group row">
                                            <label for="inputPagador" class="col-sm-1 col-form-label" style="text-align: right">PAGADOR:</label>
                                            <div class="col-md-1">
                                                <asp:TextBox ID="txtCodPagador" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtCodPagador_TextChanged"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="cboPagador" runat="server" CssClass="form-control select2" AutoPostBack="true" OnSelectedIndexChanged="cboPagador_SelectedIndexChanged"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:TextBox ID="txtCNPJPagador" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3">
                                                <asp:TextBox ID="txtCidPagador" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                            </div>
                                            <div class="col-md-1">
                                                <asp:TextBox ID="txtUFPagador" Style="text-align: center" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                        <!-- ROTA -->
                                        <div class="form-group row">
                                            <label for="inputRota" class="col-sm-1 col-form-label" style="text-align: right">ROTA:</label>
                                            <div class="col-md-1">
                                                <asp:TextBox ID="txtCodRota" runat="server" CssClass="form-control" OnTextChanged="txtCodRota_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="cboRota" runat="server" CssClass="form-control select2" AutoPostBack="true" OnSelectedIndexChanged="cboRota_SelectedIndexChanged"></asp:DropDownList>
                                            </div>
                                            <label for="inputRecebedor" class="col-sm-1 col-form-label" style="text-align: right">MATERIAL:</label>
                                            <div class="col-md-2">
                                                <asp:DropDownList ID="cboTipoMaterial" runat="server" CssClass="form-control select2"></asp:DropDownList>
                                            </div>
                                            <label for="inputRecebedor" class="col-sm-1 col-form-label" style="text-align: right">VEÍCULO:</label>
                                            <div class="col-md-2">
                                                <asp:DropDownList ID="cboTipoVeiculo" runat="server" CssClass="form-control select2"></asp:DropDownList>
                                            </div>

                                        </div>
                                        <!-- MATERIAL / VEICULO -->
                                        <div class="form-group row">
                                            <label for="inputRecebedor" class="col-sm-1 col-form-label" style="text-align: right">CIDADE:</label>
                                            <div class="col-md-5">
                                                <asp:DropDownList ID="ddlCidade" runat="server" CssClass="form-control select2"></asp:DropDownList>
                                            </div>                                            
                                            <label for="inputRecebedor" class="col-sm-1 col-form-label" style="text-align: right">FAIXA DE KM:</label>
                                            <div class="col-md-1">
                                                <asp:TextBox ID="txtKMInicial" runat="server" CssClass="form-control" placeholder="Km inicial" TextMode="Number"></asp:TextBox>
                                            </div>
                                            <div class="col-md-1">
                                                <asp:TextBox ID="txtKMFinal" runat="server" CssClass="form-control" placeholder="Km final" TextMode="Number"></asp:TextBox>
                                            </div>

                                        </div>
                                    </div>

                                    <div class="row mt-3">

                                        <div class="col-md-2">
                                            <label>Reajuste (%)</label>
                                            <asp:TextBox ID="txtPercentual" runat="server" CssClass="form-control" />
                                        </div>

                                        <div class="col-md-2">
                                            <label>Aplicar Para</label>
                                            <asp:DropDownList ID="ddlAplicarPara" runat="server" CssClass="form-control">
                                                <asp:ListItem Text="Selecione..." Value="" />
                                                <asp:ListItem Text="Transnovag" Value="frete_tng" />
                                                <asp:ListItem Text="Agregado" Value="frete_agregado" />
                                                <asp:ListItem Text="Terceiro" Value="frete_terceiro" />
                                            </asp:DropDownList>
                                        </div>

                                        <div class="col-md-2 d-flex align-items-end">
                                            <asp:Button ID="btnSimular" runat="server"
                                                Text="Simular"
                                                CssClass="btn btn-info w-100" />
                                            <%--OnClick="btnSimular_Click"--%>
                                        </div>

                                    </div>

                                    <hr />




                                    <hr />
                                    <asp:GridView ID="gvSimulacao"
                                        runat="server"
                                        CssClass="table table-bordered"
                                        AutoGenerateColumns="true" />

                                    <div class="text-right mt-3">
                                        <asp:Button ID="btnAplicar"
                                            runat="server"
                                            Text="Aplicar Reajuste"
                                            CssClass="btn btn-success btn-lg" />
                                        <%--OnClick="btnAplicar_Click"--%>
                                    </div>
                                    <div class="text-right mt-3">
                                        <a href="ConsultaFretes.aspx" class="btn btn-outline-danger btn-lg">Fechar               
                                        </a>
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
