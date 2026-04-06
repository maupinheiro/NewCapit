<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="GestaoPostos.aspx.cs" Inherits="NewCapit.dist.pages.GestaoPostos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link rel="stylesheet" href="/css/styleTabela.css">
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/css/bootstrap.min.css">
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/js/bootstrap.bundle.min.js"></script>

    <script language="javascript">
        function ConfirmMessage() {
            var selectedvalue = confirm("Exclusão de Dados\n Tem certeza de que deseja excluir a informação permanentemente?");
            if (selectedvalue) {
                document.getElementById('<%=txtconformmessageValue.ClientID %>').value = "Yes";

            } else {
                document.getElementById('<%=txtconformmessageValue.ClientID %>').value = "No";

            }

    </script>
    <style>
        .pagination-centered {
            text-align: center;
        }

            .pagination-centered table {
                margin: 0 auto; /* Isso centraliza a tabela da paginação */
            }
    </style>
    <!-- Page Heading -->
    <div class="content-wrapper">
        <div class="content-header">
            <div class="d-sm-flex align-items-center justify-content-between mb-4">
                <h1 class="h3 mb-2 text-gray-800">
                    <i class="fas fa-gas-pump"></i>&nbsp;Gestão de Postos de Combustível</h1>
                <%--<a href="Frm_CadFornecedores.aspx" class="d-none d-sm-inline-block btn btn-sm btn-primary shadow-sm"><i
                    class="fas fa-building"></i>&nbsp;Novo Cadastro            
                </a>--%>
            </div>
        </div>
        <div class="card shadow mb-4">
            <div class="card-body">
                <div class="card shadow mb-4">
                    <div class="card-header">
                        <asp:TextBox ID="myInput" CssClass="form-control myInput" OnTextChanged="myInput_TextChanged" placeholder="Pesquisar ..." AutoPostBack="true" runat="server"></asp:TextBox>
                    </div>
                    <div class="card-body">
                        <div class="table-responsive">
                            <asp:GridView ID="gvListPostos" CssClass="table table-bordered dataTable1 table-hover" Width="100%" AutoGenerateColumns="False" DataKeyNames="codfor" runat="server" HeaderStyle-CssClass="gv-header-custom" AllowPaging="True" PageSize="15" OnPageIndexChanging="gvListPostos_PageIndexChanging" ShowHeaderWhenEmpty="True">
                                <PagerStyle HorizontalAlign="Center" CssClass="pagination-centered" />
                                <Columns>
                                    <asp:BoundField DataField="id" HeaderText="#ID" Visible="false" />
                                    <asp:BoundField DataField="codfor" HeaderText="CÓDIGO" />
                                    <asp:BoundField DataField="fantasia" HeaderText="NOME FANTASIA" />
                                    <asp:BoundField DataField="combustivel_S500" HeaderText="DIESEL S500" />
                                    <asp:BoundField DataField="combustivel_S10" HeaderText="DIESEL S10" />
                                    <asp:BoundField DataField="combustivel_Etanol" HeaderText="ETANOL" />
                                    <asp:BoundField DataField="combustivel_gasolina" HeaderText="GASOLINA" />
                                    <asp:BoundField DataField="combustivel_arla" HeaderText="ARLA 32 (l)" />
                                    <asp:BoundField DataField="cidade" HeaderText="MUNICIPIO" />
                                    <asp:BoundField DataField="estado" HeaderText="UF" />

                                    <asp:TemplateField HeaderText="AÇÕES" ShowHeader="True">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkEditar" runat="server" OnClick="Editar" CssClass="btn btn-primary btn-sm"><i class="fas fa-gas-pump"></i>&nbsp;Ordem de Abastecimento</asp:LinkButton>
                                            <asp:LinkButton ID="LnkReajuste" runat="server" OnClick="Reajuste" CssClass="btn btn-danger btn-sm"><i class="fa fa-edit"></i>&nbsp;Reajuste de Preço</asp:LinkButton>
                                            <asp:LinkButton ID="lnkHistorico" runat="server" OnClick="lnkHistorico_Click" CssClass="btn btn-info btn-sm">
                                                <i class="fa fa-history"></i>&nbsp;Histórico
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>

                        </div>
                        <!--<asp:HiddenField ID="txtconformmessageValue" runat="server" />-->
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- Modal Historico -->
    <asp:Panel ID="pnlHistorico" runat="server" CssClass="modal fade" Style="display: none;">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Histórico de Preços</h5>
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-3">
                            <label>Fornecedor:</label>
                            <asp:TextBox
                                ID="txtCodFor"
                                runat="server"
                                CssClass="form-control font-weight-bold"
                                ReadOnly="true"> 
                            </asp:TextBox>
                        </div>
                        <div class="col-md-9">
                            <label>&nbsp;</label><br />
                            <asp:TextBox
                                ID="txtFornecedor"
                                runat="server"
                                class="form-control font-weight-bold"
                                ReadOnly="true">
                            </asp:TextBox>
                        </div>   
                    </div>
                    <div class="form-group">
                        <div class="col-md-4">
                            <label>Últimos:</label>
                            <asp:DropDownList ID="ddlPeriodo" runat="server" AutoPostBack="true" CssClass="form-control" OnSelectedIndexChanged="ddlPeriodo_SelectedIndexChanged">
                                <asp:ListItem Text="30 dias" Value="30" />
                                <asp:ListItem Text="60 dias" Value="60" />
                                <asp:ListItem Text="90 dias" Value="90" />
                                <asp:ListItem Text="6 meses" Value="180" />
                                <asp:ListItem Text="1 ano" Value="365" />
                                <asp:ListItem Text="2 anos" Value="730" />
                                <asp:ListItem Text="5 anos" Value="1825" />
                            </asp:DropDownList>
                        </div>
                    </div>

                    <asp:GridView ID="gvHistorico" runat="server" CssClass="table table-bordered table-hover" HeaderStyle-CssClass="gv-header-custom" AutoGenerateColumns="False" AllowSorting="true" OnSorting="gvHistorico_Sorting">
                        <Columns>
                            <asp:BoundField DataField="combustivel" HeaderText="Combustível" />
                            <asp:BoundField DataField="valor" HeaderText="Valor" DataFormatString="{0:N2}" HtmlEncode="false" />
                            <asp:BoundField DataField="dtinicio" HeaderText="Data Início" DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:BoundField DataField="dtreajuste" HeaderText="Reajuste" DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:BoundField DataField="reajustadopor" HeaderText="Responsável" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
