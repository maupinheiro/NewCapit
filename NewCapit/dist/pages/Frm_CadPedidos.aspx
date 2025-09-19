<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="Frm_CadPedidos.aspx.cs" Inherits="NewCapit.dist.pages.Frm_CadPedidos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>

    <div class="content-wrapper">
        <!-- Main content -->
        <section class="content">

            <div class="container-fluid">
                <br />
                <div id="toastContainer" class="alert alert-warning alert-dismissible" style="display: none;">
                    <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                    <h5><i class="icon fas fa-exclamation-triangle"></i>Alerta!</h5>
                    Alertas
                </div>
                <div class="card card-primary">
                    <div class="card-header">
                        <h3 class="card-title"><i class="fas fa-clipboard-list"></i>&nbsp;CARGAS - NOVA CARGA</h3>
                    </div>
                </div>
                <div class="card-header">
                    <div class="row g-3">
                        <div class="col-md-2">
                            <div class="form_group">
                                <span class="details">SOLICITANTE/PROGRAMADOR:</span>
                                <asp:DropDownList ID="cbSolicitantes" runat="server" CssClass="form-control select2"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="form_group">
                                <span class="details">TOMADOR DO SERVIÇO/PAGADOR:</span>
                                <asp:DropDownList ID="cboPagador" runat="server" CssClass="form-control select2" AutoPostBack="True" OnSelectedIndexChanged="cboPagador_SelectedIndexChanged"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form-group">
                                <span class="details">GERENCIADORA DE RISCO:</span>
                                <asp:TextBox ID="txtGR" runat="server" CssClass="form-control" placeholder=""></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form_group">
                                <span class="details">FILIAL:</span>
                                <asp:DropDownList ID="cboFilial" runat="server" CssClass="form-control select2"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-1"></div>
                        <div class="col-md-2">
                            <div class="form-group">
                                <span class="details">CADASTRO:</span>
                                <asp:TextBox ID="txtCadastro" runat="server" CssClass="form-control" placeholder=""></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="row g-3">
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="details">CÓDIGO:</span>
                                <asp:TextBox ID="txtCodRemetente" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtCodRemetente_TextChanged"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-5">
                            <div class="form-group">
                                <span class="details">REMETENTE:</span>
                                <asp:DropDownList ID="cboRemetente" runat="server" CssClass="form-control select2"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-5">
                            <div class="form-group">
                                <span class="details">MUNICIPIO:</span>
                                <asp:TextBox ID="txtMunicipioRemetente" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="details">UF:</span>
                                <asp:TextBox ID="txtUFRemetente" Style="text-align: center" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="row g-3">
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="details">CÓDIGO:</span>
                                <asp:TextBox ID="txtCodDestinatario" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-5">
                            <div class="form-group">
                                <span class="details">DESTINATÁRIO:</span>
                                <asp:DropDownList ID="cboDestinatario" runat="server" CssClass="form-control select2"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-5">
                            <div class="form-group">
                                <span class="details">MUNICIPIO:</span>
                                <asp:TextBox ID="txtMunicipioDestinatario" runat="server" CssClass="form-control" placeholder="" MaxLength="60"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="details">UF:</span>
                                <asp:TextBox ID="txtUFDestinatario" Style="text-align: center" runat="server" CssClass="form-control" placeholder="" MaxLength="2"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="row g-3">
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="details">PEDIDO:</span>
                                <asp:TextBox ID="txtNumPedido" class="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form-group">
                                <span class="details">MATERIAL:</span>
                                <asp:DropDownList ID="cboMaterial" runat="server" CssClass="form-control select2"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="details">PESO:</span>
                                <asp:TextBox ID="txtPeso" runat="server" CssClass="form-control" value=""></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="details">DEPOSITO:</span>
                                <asp:DropDownList ID="cboDeposito" runat="server" CssClass="form-control select2"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="details">STATUS:</span>
                                <asp:DropDownList ID="cboStatus" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="PRONTO" Text="PRONTO"></asp:ListItem>
                                    <asp:ListItem Value="EM PROCESSO" Text="EM PROCESSO"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="details">CTRL.CLIENTE:</span>
                                <asp:TextBox ID="txtControleCliente" runat="server" CssClass="form-control" value=""></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form-group">
                                <span class="details">PREV. ENTREGA:</span>
                                <asp:TextBox ID="txtPrevEntrega" TextMode="Date" runat="server" Style="text-align: center" CssClass="form-control" value=""></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="details">ENTREGA:</span>
                                <asp:DropDownList ID="cboEntrega" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="NORMAL" Text="NORMAL"></asp:ListItem>
                                    <asp:ListItem Value="IMEDIATA" Text="IMEDIATA"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>

                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="details">Nº PED.:</span>
                                <asp:TextBox ID="txtNumPedidos" runat="server" CssClass="form-control" value=""></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="details">PESO TOTAL:</span>
                                <asp:TextBox ID="txtPesoTotalCarga" runat="server" CssClass="form-control" value=""></asp:TextBox>
                            </div>
                        </div>
                    </div>

                    <div class="row g-3">
                        <div class="col-md-1">
                            <asp:Button ID="btnAlterar" runat="server" CssClass="btn btn-outline-success btn-lg" Text="Atualizar" />
                        </div>
                        <div class="col-md-1">
                            <a href="ConsultaColetasCNT.aspx" class="btn btn-outline-danger btn-lg">Sair               
                            </a>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>
</asp:Content>

