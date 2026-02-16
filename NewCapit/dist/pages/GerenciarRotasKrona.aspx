<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="GerenciarRotasKrona.aspx.cs" Inherits="NewCapit.dist.pages.GerenciarRotasKrona" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <link href="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/css/select2.min.css" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/js/select2.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
    <script>
        $(document).on("click", ".btnRemover", function () {

            var id = $(this).data("id");

            Swal.fire({
                title: 'Tem certeza?',
                text: "Essa rota será removida definitivamente!",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#d33',
                confirmButtonText: 'Sim, remover!',
                cancelButtonText: 'Cancelar'
            }).then((result) => {

                if (result.isConfirmed) {

                    $.ajax({
                        type: "POST",
                        url: "Rotas.aspx/ExcluirRota",
                        data: JSON.stringify({ id: id }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function () {

                            Swal.fire({
                                icon: 'success',
                                title: 'Removido!',
                                text: 'Rota excluída com sucesso.',
                                timer: 2000,
                                showConfirmButton: false
                            });

                            // Recarrega grid sem atualizar página
                            setTimeout(function () {
                                location.reload();
                            }, 1500);
                        }
                    });
                }
            });
        });
    </script>
    <div class="content-wrapper">
        <section class="content">
            <div class="container-fluid">
                <br />
                <!-- Alertas -->
                <div id="divMsg" runat="server"
                    class="alert alert-warning alert-dismissible fade show mt-3"
                    role="alert" style="display: none;">
                    <span id="lblMsgGeral" runat="server"></span>
                    <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
                </div>
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
                        <div class="card-body">
                            <div class="row mb-3">
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtPesquisa" runat="server" CssClass="form-control"
                                        placeholder="Pesquisar descrição, código ou recebedor..." />
                                </div>
                                <div class="col-md-2">
                                    <asp:Button ID="btnPesquisar" runat="server" Text="Pesquisar"
                                        CssClass="btn btn-primary w-100"
                                        OnClick="btnPesquisar_Click" />
                                </div>
                                <div class="col-md-2">
                                    <button class="btn btn-primary">Nova Rota</button>
                                </div>
                                <div class="col-md-3">
    <a href="GerenciarRotasKrona.aspx"
        class="btn btn-outline-danger w-100">Fechar
    </a>
</div>
                            </div>
                            <asp:GridView ID="gvRotas"
                                runat="server"
                                CssClass="table table-bordered table-hover dataTable1"
                                Width="100%"
                                AutoGenerateColumns="False"
                                DataKeyNames="id_rota"
                                AllowPaging="True"
                                PageSize="75"
                                OnRowCommand="gvRotas_RowCommand"
                                OnPageIndexChanging="gvRotas_PageIndexChanging"
                                ShowHeaderWhenEmpty="True"
                                EmptyDataText="Nenhuma rota encontrada.">

                                <Columns>
                                    <asp:BoundField DataField="id_rota" HeaderText="ID" />
                                    <asp:BoundField DataField="descricao_rota" HeaderText="Descrição da Rota" />
                                    <asp:TemplateField HeaderText="Expedidor">
                                        <ItemTemplate>
                                            <strong><%# Eval("cod_expedidor_rota") %></strong> -
                                            <%# Eval("expedidor_rota") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Recebedor">
                                        <ItemTemplate>
                                            <strong><%# Eval("cod_recebedor_rota") %></strong> -
                                            <%# Eval("recebedor_rota") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Ações">
                                        <ItemTemplate>

                                           <%-- <asp:LinkButton ID="btnEditar"
                                                runat="server"
                                                Text="Editar"
                                                CssClass="btn btn-sm btn-outline-primary"
                                                CommandName="Editar"
                                                CommandArgument='<%# Eval("id_rota") %>' />--%>

                                            <asp:LinkButton ID="lnkEditar" runat="server" OnClick="Editar"  CssClass="btn btn-outline-primary btn-sm"><i class="fa fa-edit"></i>Editar</asp:LinkButton>

                                            <button type="button"
                                                class="btn btn-sm btn-outline-danger btnRemover"
                                                data-id='<%# Eval("id_rota") %>'>
                                                Remover
       
                                            </button>

                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>

                            </asp:GridView>

                        </div>
                    </div>
                </div>
            </div>
            <div class="modal fade" id="modalRotaKrona" tabindex="-1">
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">

                        <div class="modal-header bg-primary text-white">
                            <h5 class="modal-title" id="tituloModal">Nova Rota</h5>
                            <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                        </div>

                        <div class="modal-body">

                            <input type="hidden" id="hfIdRota" />

                            <div class="mb-3">
                                <label>Descrição</label>
                                <input type="text" id="txtDescricao" class="form-control" />
                            </div>

                            <div class="row">
                                <div class="col-md-4">
                                    <label>Expedidor</label>
                                    <select id="ddlExpedidor" class="form-select select2"></select>
                                </div>

                                <div class="col-md-4">
                                    <label>Recebedor</label>
                                    <select id="ddlRecebedor" class="form-select select2"></select>
                                </div>
                            </div>

                        </div>

                        <div class="modal-footer">
                            <button class="btn btn-success" onclick="salvarRota()">Salvar</button>
                            <button class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                        </div>

                    </div>
                </div>
            </div>
        </section>
    </div>
    <div class="toast position-fixed bottom-0 end-0 p-3" id="toastSucesso">
        <div class="toast-body bg-success text-white">
            <span id="msgToast"></span>
        </div>
    </div>
</asp:Content>
