<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="Frm_RotaKrona.aspx.cs" Inherits="NewCapit.dist.pages.Frm_RotaKrona" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    <link href="https://cdn.jsdelivr.net/npm/select2@4.1.0/dist/css/select2.min.css" rel="stylesheet" />
    <script src="https://code.jquery.com/jquery-3.7.1.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/select2@4.1.0/dist/js/select2.min.js"></script>
    <%--<style>
        .select2-container .select2-selection--single {
            height: 38px;
            padding: 5px;
        }
    </style>--%>
    <script src="https://code.jquery.com/jquery-3.6.4.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/js/bootstrap.bundle.min.js"></script>


    <%--<script>
        function initSelect2() {
            $('.select2').select2({
                theme: 'bootstrap-5',
                width: '100%',
                placeholder: 'Selecione...',
                allowClear: true
            });
        }

        Sys.WebForms.PageRequestManager.getInstance()
            .add_endRequest(initSelect2);

        $(document).ready(initSelect2);
    </script>
    --%>
    <div class="content-wrapper">

        <!-- Main content -->
        <%--<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />--%>
        <br />
        <div class="col-md-12">
            <div class="card card-info">
                <div class="card-header" style="background-color: #A020F0; font-weight: bold;">
                    <h3 class="card-title">
                        <h3 class="card-title"><i class="fas fa-route"></i>&nbsp;GERENCIAR ROTAS KRONA</h3>
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
                <br />
                <br />
                <br />
                <br />

                <div class="container-fluid mt-4">
                    <div id="divMsg" runat="server"
                        class="alert alert-info alert-dismissible fade show mt-3"
                        role="alert" style="display: none;">
                        <span id="lblMsgGeral" runat="server"></span>
                        <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
                    </div>
                    <div class="row justify-content-center">
                        <div class="col-12 col-xxl-10">

                            <div class="card shadow-lg border-0">
                                <div class="card-header text-white py-3" style="background-color: #A020F0; font-weight: bold;">
                                    <h5 class="mb-0 fw-bold">Nova Rota Krona
                                    </h5>
                                </div>

                                <div class="card-body px-4 py-4">
                                    <div class="row mb-4">
                                        <div class="col-md-2">
                                            <label class="form-label fw-bold">ID KRONA</label>
                                            <asp:TextBox ID="txtIdKrona" runat="server"
                                                CssClass="form-control text-center"
                                                ></asp:TextBox>
                                        </div>

                                        <div class="col-md-10">
                                            <label class="form-label fw-bold">DESCRIÇÃO DA ROTA</label>
                                            <asp:TextBox ID="txtDescricaoRota" runat="server"
                                                CssClass="form-control"
                                               ></asp:TextBox>
                                        </div>
                                    </div>

                                    <hr />
                                    <h6 class="fw-bold text-secondary mb-3">ORIGEM</h6>

                                    <div class="row align-items-end mb-4">

                                        <div class="col-md-1">
                                            <label class="form-label">Código</label>
                                            <asp:TextBox ID="txtCodExpedidor" runat="server"
                                                CssClass="form-control"
                                                AutoPostBack="true"
                                                OnTextChanged="txtCodExpedidor_TextChanged"></asp:TextBox>
                                        </div>

                                        <div class="col-md-4">
                                            <label class="form-label">Expedidor</label>
                                            <asp:DropDownList ID="cboExpedidor" runat="server"
                                                CssClass="form-control select2"
                                                AutoPostBack="true"
                                                OnSelectedIndexChanged="cboExpedidor_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </div>

                                        <div class="col-md-2">
                                            <label class="form-label">CNPJ</label>
                                            <asp:TextBox ID="txtCNPJExpedidor" runat="server"
                                                CssClass="form-control"
                                                ReadOnly="true"></asp:TextBox>
                                        </div>

                                        <div class="col-md-4">
                                            <label class="form-label">Cidade</label>
                                            <asp:TextBox ID="txtCidExpedidor" runat="server"
                                                CssClass="form-control"
                                                ReadOnly="true"></asp:TextBox>
                                        </div>

                                        <div class="col-md-1">
                                            <label class="form-label">UF</label>
                                            <asp:TextBox ID="txtUFExpedidor" runat="server"
                                                CssClass="form-control text-center"
                                                ReadOnly="true"></asp:TextBox>
                                        </div>

                                    </div>
                                    <h6 class="fw-bold text-secondary mb-3">DESTINO</h6>

                                    <div class="row align-items-end mb-4">

                                        <div class="col-md-1">
                                            <label class="form-label">Código</label>
                                            <asp:TextBox ID="txtCodRecebedor" runat="server"
                                                CssClass="form-control"
                                                AutoPostBack="true"
                                                OnTextChanged="txtCodRecebedor_TextChanged"></asp:TextBox>
                                        </div>

                                        <div class="col-md-4">
                                            <label class="form-label">Recebedor</label>
                                            <asp:DropDownList ID="cboRecebedor" runat="server"
                                                CssClass="form-control select2"
                                                AutoPostBack="true"
                                                OnSelectedIndexChanged="cboRecebedor_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </div>

                                        <div class="col-md-2">
                                            <label class="form-label">CNPJ</label>
                                            <asp:TextBox ID="txtCNPJRecebedor" runat="server"
                                                CssClass="form-control"
                                                ReadOnly="true"></asp:TextBox>
                                        </div>

                                        <div class="col-md-4">
                                            <label class="form-label">Cidade</label>
                                            <asp:TextBox ID="txtCidRecebedor" runat="server"
                                                CssClass="form-control"
                                                ReadOnly="true"></asp:TextBox>
                                        </div>

                                        <div class="col-md-1">
                                            <label class="form-label">UF</label>
                                            <asp:TextBox ID="txtUFRecebedor" runat="server"
                                                CssClass="form-control text-center"
                                                ReadOnly="true"></asp:TextBox>
                                        </div>

                                    </div>

                                    <div class="row mt-4">
                                        <div class="col-md-3">
                                            <asp:Button ID="btnAtualizar" runat="server" Text="Cadastrar" CssClass="btn btn-outline-success w-100" OnClick="btnAtualizar_Click" />
                                          
                                        </div>
                                        <div class="col-md-3">
                                            <a href="GerenciarRotasKrona.aspx"
                                                class="btn btn-outline-danger w-100">Fechar
                                            </a>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
                <br />
                <br />
                <br />
                <br />
                <br />
            </div>
        </div>


    </div>

</asp:Content>