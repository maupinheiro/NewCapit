<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="controlaestoque.aspx.cs" Inherits="NewCapit.dist.pages.controlaestoque" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link rel="stylesheet" href="~/plugins/bootstrap/css/bootstrap.min.css" />
    <link rel="stylesheet" href="~/dist/css/adminlte.min.css" />
    <script src="~/plugins/jquery/jquery.min.js"></script>
    <script src="~/plugins/bootstrap/js/bootstrap.bundle.min.js"></script>
    <style>
        /* Cabeçalho da GridView */
        .gv-header-custom th {
            background-color: #cce5ff !important; /* azul clarinho */
            color: #000000 !important; /* texto preto */
            font-weight: bold;
            text-align: center;
        }
    </style>
    <style>
        .estoque-baixo {
            background-color: #FFF176 !important; /* amarelo */
            color: #000000 !important; /* texto preto */
            font-weight: bold;
        }
    </style>
    <script>
        function validarCampos() {
            var descricao = document.getElementById('<%= txtDescricaoPecaModal.ClientID %>').value;
            var unidade = document.getElementById('<%= ddlUnidadeModal.ClientID %>').value;
            var estoque = document.getElementById('<%= txtEstoqueMinimoModal.ClientID %>').value;

            if (descricao === "" || unidade === "") {
                Mensagem("info", "Preencha todos os campos corretamente!");
                return false; // NÃO faz postback
            }

            return true;
        }
    </script>
    <script>
        function fecharModal() {
            $('#modalCadastrarPeca').modal('hide');
        }
    </script>

    <div class="content-wrapper">
        <section class="content">
            <div class="container-fluid">
                <br />
                <div class="col-md-12">
                    <div class="card card-info">
                        <div class="card-header" style="background-color: #A020F0; font-weight: bold;">
                            <h3 class="card-title"><i class="fas fa-shipping-fast"></i>&nbsp;Manutenção</h3>
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
                        </div>
                        <br />

                        <div class="card-body">
                            <div class="card-header bg-secondary text-white">
                                Controle de Estoque
                            </div>

                            <br />
                            <div class="row">
                                <div class="col-md-8">
                                    <asp:TextBox ID="txtPesquisa" runat="server"
                                        CssClass="form-control"
                                        Placeholder="Pesquisa por ID, Peça..."></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <asp:Button ID="btnPesquisaPeca" runat="server"
                                        Text="Atualizar"
                                        CssClass="btn btn-warning w-100"
                                        OnClick="PesquisaPeca" />
                                </div>

                                <div class="col-md-2">
                                    <button type="button" class="btn btn-primary w-100" data-toggle="modal" data-target="#modalCadastrarPeca">
                                        Cadastro
                                    </button>
                                </div>

                            </div>
                            <br />
                            <div class="row">
                                <asp:GridView ID="gvEstoque" runat="server"
                                    CssClass="table table-bordered table-hover"
                                    AutoGenerateColumns="False"
                                    DataKeyNames="id_peca"
                                    OnRowDataBound="gvEstoque_RowDataBound"
                                    OnRowCommand="gvEstoque_RowCommand"
                                    HeaderStyle-CssClass="gv-header-custom">
                                    <Columns>
                                        <asp:BoundField DataField="id_peca" HeaderText="ID" />
                                        <asp:BoundField DataField="descricao_peca" HeaderText="PEÇA" />
                                        <asp:BoundField DataField="unidade" HeaderText="UNIDADE" />
                                        <asp:BoundField DataField="estoque_peca" HeaderText="ESTOQUE ATUAL" />
                                        <asp:BoundField DataField="estoque_minimo" HeaderText="ESTOQUE MINIMO" />
                                        <asp:BoundField DataField="tipo_peca" HeaderText="TIPO DE FORNECEDOR" />
                                        <asp:BoundField
                                            DataField="valor_unitario"
                                            HeaderText="VALOR UNIT."
                                            DataFormatString="{0:C}"
                                            HtmlEncode="false" />

                                        <asp:TemplateField HeaderText="AÇÕES">

                                            <ItemTemplate>

                                                <asp:LinkButton ID="btnEntrada"
                                                    runat="server"
                                                    CommandName="entrada"
                                                    CommandArgument='<%# Eval("id_peca") %>'
                                                    CssClass="btn btn-sm btn-success">
                                                Entrada
                                                </asp:LinkButton>

                                                <asp:LinkButton ID="btnHistorico"
                                                    runat="server"
                                                    CommandName="historico"
                                                    CommandArgument='<%# Eval("id_peca") %>'
                                                    CssClass="btn btn-sm btn-warning">
                                                 Ver Histórico
                                                </asp:LinkButton>
                                            </ItemTemplate>

                                        </asp:TemplateField>

                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Modal Bootstrap Cadastro de Peça -->
                <div class="modal fade" id="modalCadastrarPeca" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
                    <div class="modal-dialog modal-dialog-centered modal-lg" role="document">
                        <div class="modal-content">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <div class="modal-header bg-info text-white">
                                        <h5 class="modal-title" id="meuModalLabel">Cadastro de Peças</h5>
                                        <button type="button" class="close" data-dismiss="modal" aria-label="Fechar">
                                            <span aria-hidden="true">&times;</span>
                                        </button>
                                    </div>
                                    <div class="modal-body">
                                        <asp:Label ID="lblMsgModal" runat="server" CssClass="text-danger"></asp:Label>
                                        <div id="divMsg" runat="server"
                                            class="alert alert-dismissible fade show mt-3"
                                            role="alert" visible="false">
                                            <asp:Label ID="lblMsgGeral" runat="server"></asp:Label>
                                            <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
                                        </div>
                                        <!-- Tipo -->
                                        <div class="form-group">
                                            <label>Produto:</label>
                                            <asp:DropDownList ID="ddlTipo" runat="server" CssClass="form-control">
                                                <asp:ListItem Text="Selecione..." Value="" />
                                                <asp:ListItem Text="PNEU" Value="PNEU" />
                                                <asp:ListItem Text="COMBUSTIVEL" Value="COMBUSTIVEL" />
                                                <asp:ListItem Text="OUTRO" Value="OUTRO" /> 
                                            </asp:DropDownList>
                                        </div>
                                        <!-- Descrição da peça -->
                                        <div class="form-group">
                                            <label>Descrição da Peça</label>
                                            <asp:TextBox ID="txtDescricaoPecaModal" runat="server" CssClass="form-control" />
                                        </div>
                                        <!-- Unidade (DropDownList para limitar opções) -->
                                        <div class="form-group">
                                            <label>Unidade</label>
                                            <asp:DropDownList ID="ddlUnidadeModal" runat="server" CssClass="form-control">
                                                <asp:ListItem Text="Selecione..." Value="" />
                                                <asp:ListItem Text="UN" Value="UN" />
                                                <asp:ListItem Text="PC" Value="PC" />
                                                <asp:ListItem Text="LITRO" Value="LITRO" />
                                                <asp:ListItem Text="GALÃO" Value="GALÃO" />
                                                <asp:ListItem Text="BARRICA" Value="BARRICA" />
                                                <asp:ListItem Text="CAIXA" Value="CAIXA" />
                                                <asp:ListItem Text="OUTROS" Value="OUTROS" />
                                            </asp:DropDownList>
                                        </div>

                                        <!-- Estoque mínimo -->
                                        <div class="form-group">
                                            <label>Estoque Mínimo</label>
                                            <asp:TextBox ID="txtEstoqueMinimoModal" runat="server" CssClass="form-control" MaxLength="5"
                                                onkeypress="return event.charCode >= 48 && event.charCode <= 57" />
                                        </div>

                                    </div>
                                    <div class="modal-footer">
                                        <asp:Button ID="btnSalvarPecaModal" runat="server" Text="Salvar" class="btn btn-primary" OnClick="btnSalvarPecaModal_Click" OnClientClick="return validarCampos();" />
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>

</asp:Content>
