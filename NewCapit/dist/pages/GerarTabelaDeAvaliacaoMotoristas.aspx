<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="GerarTabelaDeAvaliacaoMotoristas.aspx.cs" Inherits="NewCapit.dist.pages.GerarTabelaDeAvaliacaoMotoristas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- Bootstrap e jQuery -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>

    <!-- Plugin Bootstrap Multiselect -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap4-multiselect/dist/css/bootstrap-multiselect.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap4-multiselect/dist/js/bootstrap-multiselect.min.js"></script>

    <script>
        $(document).ready(function () {
            $('#<%= ddlStatus.ClientID %>').multiselect({
                includeSelectAllOption: true,
                nonSelectedText: 'Selecione status',
                nSelectedText: 'selecionados',
                allSelectedText: 'Todos selecionados'
            });
        });
    </script>

    <div class="content-wrapper">
        <section class="content">
            <div class="container-fluid">
                <br />
                <div id="toastContainerVermelho" class="alert alert-danger alert-dismissible" style="display: none;">
                    <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                    <h5><i class="icon fas fa-exclamation-triangle"></i>Alerta!</h5>
                    Alertas
                </div>
                <div class="col-md-12">
                    <div class="card card-info">
                        <div class="card-header">
                            <h3 class="card-title">
                                <h3 class="card-title"><i class="fas fa-book"></i>&nbsp;GERAR TABELA MENSAL PARA AVALIAÇÃO DO MOTORISTA</h3>
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
                                <div class="col-md-2">
                                    <label>Data Inicial:</label>
                                    <asp:TextBox ID="DataInicio" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <label>Data Final:</label>
                                    <asp:TextBox ID="DataFim" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                                </div>
                                <div class="col-md-5">
                                    <div class="select2-purple">
                                        <label>Filial:</label>
                                        <asp:DropDownList ID="ddlStatus" multiple="multiple" runat="server" CssClass="form-control select2"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-1">
                                    <label>&nbsp;</label><br />
                                    <asp:Button ID="btnPesquisar" runat="server" Text="Selecionar" CssClass="btn btn-primary mb-3" OnClick="btnPesquisar_Click" />
                                    <%--<asp:Button ID="btnFiltrar" runat="server" CssClass="btn btn-warning" Text="Filtrar" OnClick="btnPesquisar_Click" />--%>
                                </div> 
                                <%-- <div class="col-md-1">
                                    <label>&nbsp;</label><br />
                                    <asp:Button ID="btnFiltrar" runat="server" CssClass="btn btn-warning" Text="Filtrar" OnClick="btnFiltrar_Click" />
                                </div>--%>
                            </div>
                            <div class="container-fluid">
                                <div class="row">
                                    <div class="col-12">
                                        <!-- /.col -->
                                        <div class="card">
                                            <!-- /.card-header -->
                                            <div class="card-body">

                                                <!-- testando aqui -->
                                                <%-- <div class="mb-3">
                                                    <asp:ListBox ID="ddlStatus" runat="server" SelectionMode="Multiple" CssClass="form-control"></asp:ListBox>
                                                </div>--%>


                                               <%-- <div class="col-md-5">
                                                    <asp:ListBox ID="ddlStatus" runat="server" SelectionMode="Multiple" CssClass="form-control"></asp:ListBox>
                                                </div>--%>

                                                <%--<div class="col-md-5">
     <div class="select2-purple">
         <label>Filial:</label>
         <asp:DropDownList ID="ddlStatus" multiple="multiple" runat="server" CssClass="form-control select2"></asp:DropDownList>
     </div>
 </div>--%>

                                                

                                                 <div class="table-responsive">
                                                    <asp:GridView runat="server" ID="gvPedidos" CssClass="table table-bordered dataTable1 table-hover" Width="100%" AutoGenerateColumns="False" DataKeyNames="id" AllowPaging="True" PageSize="50"  ShowHeaderWhenEmpty="True">
                                                        <PagerStyle HorizontalAlign="Center" CssClass="pagination-centered" />
                                                        <Columns>
                                                            <%--tamanho da foto 45x45--%>
                                                <asp:ImageField DataImageUrlField="caminhofoto" HeaderText="FOTO" ControlStyle-Width="39" ItemStyle-Width="39" ControlStyle-CssClass="rounded-circle" ItemStyle-HorizontalAlign="Center" />
                                                <asp:TemplateField HeaderText="CRACHÁ">
                                                    <itemtemplate>
                                                        <%# Eval("codmot") %>
                                                    </itemtemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="NOME DO MOTORISTA">
                                                    <itemtemplate>
                                                        <%# Eval("nommot") %>
                                                    </itemtemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="CARGO">
                                                    <itemtemplate>
                                                        <%# Eval("cargo") %>
                                                    </itemtemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="ADMISSÃO">
                                                    <itemtemplate>
                                                        <%# Eval("cadmot", "{0:dd/MM/yyyy}") %>
                                                    </itemtemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="FROTA">
                                                    <itemtemplate>
                                                        <%# Eval("frota") %>
                                                    </itemtemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="FILIAL">
                                                    <itemtemplate>
                                                        <%# Eval("nucleo") %>
                                                    </itemtemplate>
                                                </asp:TemplateField>
                                                <%-- Mês
                                                <asp:TemplateField HeaderText="MêS">
                                                    <itemtemplate>
                                                       <asp:Label ID="lblMes" runat="server"></asp:Label>
                                                    </itemtemplate>
                                                </asp:TemplateField>--%>
                                                </Columns>
                                                    </asp:GridView>
                                            </div>
                                            </div>
                                            <!-- /.card-body -->
                                        </div>
                                        <!-- /.card -->
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>
</asp:Content>
