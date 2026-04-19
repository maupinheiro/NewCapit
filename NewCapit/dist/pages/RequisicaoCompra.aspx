<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="RequisicaoCompra.aspx.cs" Inherits="NewCapit.dist.pages.RequisicaoCompra" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/css/select2.min.css" rel="stylesheet" />

    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/js/select2.min.js"></script>

    <script>
        //function debounce(fn, delay) {
        //    let timer;
        //    return function () {
        //        clearTimeout(timer);
        //        timer = setTimeout(() => fn.apply(this, arguments), delay);
        //    };
        //}

        $(document).ready(function () {           

            $('#ddlProduto').select2({
                placeholder: "Buscar produto...",
                minimumInputLength: 2,
                ajax: {
                    url: 'RequisicaoCompra.aspx/BuscarProdutos',
                    type: 'POST',
                    dataType: 'json',
                    contentType: "application/json; charset=utf-8",
                    delay: 250,
                    data: function (params) {
                        return JSON.stringify({
                            termo: params.term
                        });
                    },
                    processResults: function (data) {
                        return {
                            results: data.d
                        };
                    }
                }
            });

        });
    </script>
    <script>
        function salvarAssinatura() {
            var canvas = document.getElementById("canvasAssinatura");
            var dataUrl = canvas.toDataURL();
            document.getElementById("<%= hfAssinatura.ClientID %>").value = dataUrl;
        }
    </script>
    <script>
        function abrirModalItem() {
            $('#modalItem').modal('show');
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
                            <%-- <div class="row mb-3">

                                <div class="col-md-2">
                                    <label>Data Inicial</label>
                                    <asp:TextBox ID="txtDataInicial" runat="server" CssClass="form-control" TextMode="Date" />
                                </div>

                                <div class="col-md-2">
                                    <label>Data Final</label>
                                    <asp:TextBox ID="txtDataFinal" runat="server" CssClass="form-control" TextMode="Date" />
                                </div>

                                <div class="col-md-4">
                                    <label>Buscar</label>
                                    <asp:TextBox ID="txtBusca" runat="server" CssClass="form-control" placeholder="Motorista, placa, ordem, forta..." />
                                </div>

                                <div class="col-md-2 d-flex align-items-end">
                                    <asp:Button ID="btnFiltrar" runat="server" Text="Filtrar"
                                        CssClass="btn btn-primary w-100"
                                        OnClick="btnFiltrar_Click" />
                                </div>

                            </div>--%>

                            <div class="container">

                                <asp:TextBox ID="txtSolicitante" runat="server" CssClass="form-control" placeholder="Solicitante"></asp:TextBox>

                                <asp:TextBox ID="txtSetor" runat="server" CssClass="form-control" placeholder="Setor"></asp:TextBox>

                                <asp:TextBox ID="txtObservacao" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>

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
                                        <asp:Button ID="btnSalvarItem" runat="server" Text="Salvar" CssClass="btn btn-success"  />
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
