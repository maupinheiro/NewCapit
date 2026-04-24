<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="RequisicaoCompra.aspx.cs" Inherits="NewCapit.dist.pages.RequisicaoCompra" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    <script>
        function salvarAssinatura() {
            var canvas = document.getElementById("canvasAssinatura");
            var dataUrl = canvas.toDataURL();
            document.getElementById("<%= hfAssinatura.ClientID %>").value = dataUrl;
        }
    </script>
    <script>
        function somenteNumeros(e) {
            var tecla = (window.event) ? event.keyCode : e.which;

            // Permite apenas números (0-9)
            if (tecla >= 48 && tecla <= 57) {
                return true;
            }
            return false;
        }
    </script>
    <div class="content-wrapper">
        <section class="content">
            <div class="container-fluid">
                <br />
                <div class="col-md-12">
                    <div class="card card-info">
                        <!-- Header -->
                        <div class="card-header" style="background-color: #A020F0; font-weight: bold;">
                            <h3 class="card-title"><i class="far fa-credit-card"></i>&nbsp;Controle de Compras
                                <br />
                                <small>Requisição de Compras</small></h3>
                        </div>
                        <br />
                        <%--HeaderStyle-CssClass="gv-header-custom"--%>

                        <div class="card-body">
                            <div id="divMsg" runat="server"
                                class="alert alert-dismissible fade show mt-3"
                                role="alert" visible="false">
                                <asp:Label ID="lblMsgGeral" runat="server"></asp:Label>
                                <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
                            </div>
                            <div class="container">
                                <div class="row mb-3">
                                    <div class="col-md-5">
                                        <label>SOLICITANTE:</label>
                                        <asp:TextBox ID="txtSolicitante" runat="server" CssClass="form-control" placeholder="Solicitante"></asp:TextBox>
                                    </div>
                                    <div class="col-md-5">
                                        <label>SETOR:</label>
                                        <asp:TextBox ID="txtSetor" runat="server" CssClass="form-control" placeholder="Setor"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                        <label>DATA:</label>
                                        <asp:TextBox ID="txtData" runat="server" CssClass="form-control" placeholder="Data"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row mb-3">
                                    <div class="col-md-12">
                                        <label>OBSERVAÇÃO:</label>
                                        <asp:TextBox ID="txtObservacao" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row mb-3">
                                    <div class="col-md-1">
                                        <label>CODIGO:</label>
                                        <asp:TextBox ID="txtCodigo" runat="server" CssClass="form-control" onkeypress="return somenteNumeros(event)" placeholder="" AutoPostBack="true" OnTextChanged="txtCodigo_TextChanged"></asp:TextBox>
                                    </div>
                                    <div class="col-md-4">
                                        <label>DESCRIÇÃO DO PRODUTO:</label>
                                        <div class="form_group">
                                            <asp:DropDownList ID="ddlDescricao" runat="server" CssClass="form-control select2" AutoPostBack="true" OnSelectedIndexChanged="ddlDescricao_SelectedIndexChanged"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-1">
                                        <label>UN:</label>
                                        <asp:TextBox ID="txtUnidade" runat="server" CssClass="form-control" placeholder=""></asp:TextBox>
                                    </div>
                                    <div class="col-md-1">
                                        <label>EST.:</label>
                                        <asp:TextBox ID="txtEstoque" runat="server" CssClass="form-control" placeholder=""></asp:TextBox>
                                    </div>
                                    <div class="col-md-1">
                                        <label>QTD:</label>
                                        <asp:TextBox ID="txtQtd" runat="server" CssClass="form-control" onkeypress="return somenteNumeros(event)" placeholder=""></asp:TextBox>
                                    </div>
                                    <div class="col-md-4">
                                        <label>APLICAÇÃO/DESTINO:</label>
                                        <asp:TextBox ID="txtAplicacao" runat="server" CssClass="form-control" placeholder=""></asp:TextBox>
                                    </div>
                                </div>
                                <br />
                                <asp:Button ID="btnAdicionarItem" runat="server" Text="Adicionar Item" CssClass="btn btn-primary" OnClientClick="abrirModalItem(); return false;" />

                                <asp:Button ID="btnEnviar" runat="server" Text="Enviar para Aprovação" CssClass="btn btn-success" OnClick="btnEnviar_Click" />

                            </div>

                            <asp:GridView ID="gvItens" runat="server" AutoGenerateColumns="false" CssClass="table">
                                <Columns>
                                    <asp:BoundField DataField="Produto" HeaderText="Produto" />
                                    <asp:BoundField DataField="Quantidade" HeaderText="Quantidade" />
                                    <asp:BoundField DataField="Estoque" HeaderText="Estoque Atual" />
                                </Columns>
                            </asp:GridView>

                        </div>
                        <div class="modal fade" id="modalItem" tabindex="-1">
                            <div class="modal-dialog">
                                <div class="modal-content">

                                    <div class="modal-header">
                                        <h5>Adicionar Item</h5>
                                    </div>

                                    <div class="modal-body">
                                        <%--<asp:TextBox ID="txtProduto" runat="server" CssClass="form-control" placeholder="Produto"></asp:TextBox>--%>
                                        <%-- <select id="ddlProduto" runat="server" class="form-control select2-erp"></select>--%>
                                        <select id="ddlProduto" class="form-control select2-erp"></select>
                                        <asp:TextBox ID="txtQuantidade" runat="server" CssClass="form-control" placeholder="Quantidade"></asp:TextBox>
                                        <asp:FileUpload ID="fileOrcamento" runat="server" CssClass="form-control" />
                                        <asp:Button ID="btnUpload" runat="server" Text="Anexar Orçamento" CssClass="btn btn-info" OnClick="btnUpload_Click" />

                                        <canvas id="canvasAssinatura" width="400" height="150" style="border: 1px solid #000;"></canvas>

                                        <br />

                                        <button type="button" onclick="salvarAssinatura()">Salvar Assinatura</button>

                                        <asp:HiddenField ID="hfAssinatura" runat="server" />
                                    </div>

                                    <div class="modal-footer">
                                        <asp:Button ID="btnSalvarItem" runat="server" Text="Salvar" CssClass="btn btn-success" />
                                        <%-- OnClick="btnSalvarItem_Click"--%>
                                    </div>

                                </div>
                            </div>
                        </div>

                    </div>
                </div>
        </section>
    </div>
</asp:Content>
