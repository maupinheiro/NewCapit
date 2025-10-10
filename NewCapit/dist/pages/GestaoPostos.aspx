<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="GestaoPostos.aspx.cs" Inherits="NewCapit.dist.pages.GestaoPostos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link rel="stylesheet" href="/css/styleTabela.css">
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
                            <asp:GridView ID="gvListPostos" CssClass="table table-bordered dataTable1 table-hover" Width="100%" AutoGenerateColumns="False" DataKeyNames="id" runat="server" AllowPaging="True" PageSize="10" OnPageIndexChanging="gvListPostos_PageIndexChanging" ShowHeaderWhenEmpty="True">
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
                                           <asp:LinkButton ID="LnkReajuste" runat="server" OnClick="Reajuste" CssClass="btn btn-primary btn-sm"><i class="fa fa-edit"></i>&nbsp;Reajuste de Preço</asp:LinkButton>
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
</asp:Content>
