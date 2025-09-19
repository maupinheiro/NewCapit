<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="Frm_EditarColeta.aspx.cs" Inherits="NewCapit.dist.pages.Frm_EditarColeta" %>

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
                <div class="card card-primary">
                    <div class="card-header">
                        <h3 class="card-title"><i class="fas fa-clipboard-list"></i>&nbsp;COLETAS - ATUALIZAÇÃO DE DADOS&nbsp;<asp:Label ID="lblNumerocva" runat="server" Text=""></asp:Label> </h3>
                    </div>
                </div>
                <div class="card-header">
                    <div class="row g-3">
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="details">COLETA:</span>
                                <asp:TextBox ID="txtColeta" runat="server" CssClass="form-control" placeholder="" MaxLength="11"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form_group">
                                <span class="details">FILIAL:</span>
                                <asp:DropDownList ID="cbFiliais" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form_group">
                                <span class="details">PLANTA:</span>
                                <asp:DropDownList ID="ddlPlanta" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="form_group">
                                <span class="details">TIPO DE VEÍCULO:</span>
                                <asp:DropDownList ID="ddlTipoVeiculo" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>
                        </div>                        
                        <div class="col-md-1">  
                            <asp:TextBox ID="txtAlterado" runat="server" CssClass="form-control" placeholder="" Visible="false"></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            <div class="form-group">
                                <span class="details">SITUAÇÃO:</span>
                                <asp:TextBox ID="txtSituacao" runat="server" CssClass="form-control" placeholder=""></asp:TextBox>
                            </div>
                        </div>
                        </div>
                    <div class="row g-3">
                                <div class="col-md-1">
                                    <div class="form-group">
                                        <span class="details">CÓDIGO:</span>
                                        <asp:TextBox ID="txtCodRemetente" runat="server" CssClass="form-control" placeholder="" MaxLength="10" OnTextChanged="txtCodRemetente_TextChanged" AutoPostBack="True"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-5">
                                    <div class="form-group">
                                        <span class="details">REMETENTE:</span>
                                        <asp:TextBox ID="txtNomRemetente" runat="server" CssClass="form-control" placeholder="" MaxLength="60"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-5">
                                    <div class="form-group">
                                        <span class="details">MUNICIPIO:</span>
                                        <asp:TextBox ID="txtMunicipioRemetente" runat="server" CssClass="form-control" placeholder="" MaxLength="60"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-1">
                                    <div class="form-group">
                                        <span class="details">UF:</span>
                                        <asp:TextBox ID="txtUFRemetente" Style="text-align: center" runat="server" CssClass="form-control" placeholder="" MaxLength="2"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                    <div class="row g-3">
                                <div class="col-md-1">
                                    <div class="form-group">
                                        <span class="details">CÓDIGO:</span>
                                        <asp:TextBox ID="txtCodDestinatario" runat="server" CssClass="form-control" placeholder="" MaxLength="10" OnTextChanged="txtCodDestinatario_TextChanged" AutoPostBack="true"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-5">
                                    <div class="form-group">
                                        <span class="details">DESTINATÁRIO:</span>
                                        <asp:TextBox ID="txtNomDestinatario" runat="server" CssClass="form-control" placeholder="" MaxLength="60"></asp:TextBox>
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
                                <div class="col-md-2">
                                    <div class="form-group">
                                        <span class="details">DATA COLETA:</span>
                                        <asp:TextBox ID="txtDataColeta" class="form-control" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div class="form-group">
                                        <span class="details">SOLICITAÇÃO(ÕES):</span>
                                        <asp:TextBox ID="txtSolicitacoes" runat="server" CssClass="form-control" value=""></asp:TextBox>
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
                                        <span class="details">M<sup>3</sup>:</span>
                                        <asp:TextBox ID="txtMetragem" runat="server" CssClass="form-control"> </asp:TextBox>
                                    </div>
                                </div>
                        
                            </div>
                    <div class="row g-3">
                                <div class="col-md-3">
                                    <div class="form-group">
                                        <span class="details">VIAGEM:</span>
                                        <asp:TextBox ID="txtViagem" runat="server" CssClass="form-control" placeholder=""></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div class="form-group">
                                        <span class="details">ROTA:</span>
                                        <asp:TextBox ID="txtRota" runat="server" CssClass="form-control" placeholder=""></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-2">
                                    <div class="form-group">
                                        <span class="details">ESTUDO DE ROTA:</span>
                                        <asp:TextBox ID="txtEstudoRota" runat="server" CssClass="form-control" placeholder=""></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-2">
                                    <div class="form-group">
                                        <span class="details">REMESSA:</span>
                                        <asp:TextBox ID="txtRemessa" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                    <div class="row g-3">
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <span class="details">QUANTIDADES/PALET´S:</span>
                                        <asp:TextBox Rows="5" ID="txtHistorico" runat="server" class="form-control" placeholder="Quant. / Palet´s ..." TextMode="MultiLine"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                    <div class="row g-3">
                                <div class="col-md-2">
                                    <div class="form-group">
                                        <span class="details">CADASTRADO EM:</span>
                                        <asp:Label ID="lblDtCadastro" runat="server" CssClass="form-control" placeholder="" maxlength="20"></asp:Label>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <span class="details">POR:</span>
                                        <asp:TextBox ID="txtUsuCadastro" runat="server" CssClass="form-control" placeholder="" MaxLength="60"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-md-2">
                                    <div class="form-group">
                                        <span class="details">ÚLTIMA ATUALIZAÇÃO:</span>
                                        <asp:Label ID="lblDtAlteracao" runat="server" CssClass="form-control" placeholder="" maxlength="20"></asp:Label>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <span class="details">POR:</span>
                                        <asp:TextBox ID="txtUsuAlteracao" runat="server" CssClass="form-control" placeholder=""></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                    <div class="row g-3">
                                <div class="col-md-1">
                                    <asp:Button ID="btnAlterar" runat="server" CssClass="btn btn-outline-success btn-lg" Text="Atualizar" OnClick="btnAlterar_Click" />
                                </div>
                                <div class="col-md-1">
                                    <a href="ConsultaColetasCNT.aspx" class="btn btn-outline-danger btn-lg">Sair               
                                    </a>
                                </div>
                            </div>
                </div>
            </div>
        </section>
        <!-- Mensagens de erro toast -->
        <div class="toast-container position-fixed top-0 end-0 p-3">
            <div id="toastNotFound" class="toast align-items-center text-bg-danger border-0" role="alert" aria-live="assertive" aria-atomic="true">
                <div class="d-flex">
                    <div class="toast-body">
                        Código não encontrado. Verifique o código digitado.
                    </div>
                    <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast" aria-label="Close"></button>
                </div>
            </div>
        </div>
    </div>
    <!-- /.content-wrapper -->   
    
    <script>
    function mostrarToastNaoEncontrado() {
        var toastEl = document.getElementById('toastNotFound');
        var toast = new bootstrap.Toast(toastEl);
        toast.show();
    }
    </script>
</asp:Content>
