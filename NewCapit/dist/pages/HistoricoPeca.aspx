<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="HistoricoPeca.aspx.cs" Inherits="NewCapit.dist.pages.HistoricoPeca" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- Bootstrap CSS -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/css/bootstrap.min.css" rel="stylesheet" />
    <!-- jQuery e Bootstrap JS -->
    <script src="https://code.jquery.com/jquery-3.6.4.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/js/bootstrap.bundle.min.js"></script>
    <!-- Bootstrap CSS + JS -->
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" />
    <script src="https://code.jquery.com/jquery-3.5.1.slim.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@4.5.2/dist/js/bootstrap.bundle.min.js"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
        
    <div class="content-wrapper">
        <div class="card card-info">
            <div class="card-header" style="background-color: #A020F0; font-weight: bold;">
                <h3 class="card-title"><i class="far fa-calendar-check"></i>&nbsp;Manutenção</h3>
            </div>
        </div>
        <br />
        <br />
        <br />
        <br />
        <div class="container mt-4">
            <div id="divMsg" runat="server"
                class="alert alert-dismissible fade show mt-3"
                role="alert" visible="false">
                <asp:Label ID="lblMsgGeral" runat="server"></asp:Label>
                <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
            </div>
            <div class="card shadow">
                <div class="card-header bg-primary text-white">
                    <h5 class="mb-0">Historico de Compras da Peça</h5>
                </div>
                <div class="card-body">                    
                   <div class="form-row">
                            <div class="form-group col-md-1">
                                <label>Peça:</label>
                                <asp:TextBox ID="txtIdPeca" runat="server" CssClass="form-control" ReadOnly="true" />
                            </div>
                            <div class="form-group col-md-7">
                                <label>Descrição:</label>
                                <asp:TextBox ID="txtPeca" runat="server" CssClass="form-control" ReadOnly="true" />
                            </div>
                            <div class="form-group col-md-2">
                                <label>Est. Atual:</label>
                                <asp:TextBox ID="txtEstoqueAtual" runat="server" CssClass="form-control" ReadOnly="true" />
                            </div>
                            <div class="form-group col-md-2">
                                <label>Unidade:</label>
                                <asp:TextBox ID="txtUnidade" runat="server" CssClass="form-control" ReadOnly="true" />
                            </div>

                        </div>                   
                   <div class="form-row">
                            <div class="form-group col-md-3">
                                <label>Últimos:</label>
                                <asp:DropDownList ID="ddlPeriodo" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlPeriodo_SelectedIndexChanged">
                                    <asp:ListItem Text="30 dias" Value="30" />
                                    <asp:ListItem Text="60 dias" Value="60" />
                                    <asp:ListItem Text="90 dias" Value="90" />
                                    <asp:ListItem Text="120 dias" Value="120" />
                                    <asp:ListItem Text="01 ano" Value="365" />
                                    <asp:ListItem Text="02 anos" Value="730" />
                                    <asp:ListItem Text="Todos" Value="0" />
                                </asp:DropDownList>  
                             </div>   
                        </div>                   
                   <div class="form-row">
                            <div class="form-group col-md-12">
                             <asp:GridView ID="gvHistorico" runat="server"
                                CssClass="table table-bordered"
                                 HeaderStyle-CssClass="gv-header-custom"
                                AutoGenerateColumns="false">
                                <Columns>
                                    <asp:BoundField DataField="fornecedor" HeaderText="Fornecedor" />
                                    <asp:BoundField DataField="nota_fiscal" HeaderText="Nota Fiscal" />
                                    <asp:BoundField DataField="emissao_nf" HeaderText="Data Compra" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField DataField="quantidade" HeaderText="Qtd" />
                                    <asp:BoundField DataField="valor_unitario" HeaderText="Vlr Unit." DataFormatString="{0:C}" />
                                    <asp:BoundField DataField="valor_total" HeaderText="Total" DataFormatString="{0:C}" />
                                </Columns>

                            </asp:GridView>
                            </div>
                        </div>
                   <br />
                   <div class="row g-3">
                        <%--<div class="col-md-3">
                            <br />
                            <asp:Button ID="btnConfirmar" CssClass="btn btn-outline-info w-100" runat="server" Text="Confirmar Entrada" OnClick="btnConfirmar_Click"/>
                        </div>--%>

                        <div class="col-md-2">
                            <br />
                            <a href="controlaestoque.aspx" class="btn btn-outline-danger w-100">Fechar   
                            </a>
                        </div>
                    </div>
                </div>
            </div>
        
        </div>
    </div>
</asp:Content>

