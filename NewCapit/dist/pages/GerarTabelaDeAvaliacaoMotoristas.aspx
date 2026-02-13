<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="GerarTabelaDeAvaliacaoMotoristas.aspx.cs" Inherits="NewCapit.dist.pages.GerarTabelaDeAvaliacaoMotoristas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- Bootstrap e jQuery -->
    <%-- <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>--%>

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>

    <!-- Plugin Bootstrap Multiselect -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap4-multiselect/dist/css/bootstrap-multiselect.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap4-multiselect/dist/js/bootstrap-multiselect.min.js"></script>

    <style>
        .btn-full {
            width: 100% !important;
            display: block;
            text-align: center;
        }
    </style>
    <script>
        $(document).ready(function () {
            $('#ddlStatus').multiselect({
                includeSelectAllOption: true,
                onChange: function () { atualizarTextbox(); },
                onSelectAll: function () { atualizarTextbox(); },
                onDeselectAll: function () { atualizarTextbox(); }
            });
        });

        function atualizarTextbox() {
            let selecionados = $('#ddlStatus').val(); // array de valores selecionados

            // junta os valores com traço
            let texto = selecionados ? selecionados.join("_") : "";

            // escreve na TextBox ASPX
            $('#<%= txtSelecionados.ClientID %>').val(texto);
        }

    </script>

    <div class="content-wrapper">
        <section class="content">
            <div class="container-fluid">
                <div class="col-md-12">
                    <div class="card card-info">
                        <div class="card-header">
                            <h3 class="card-title">
                                <h3 class="card-title"><i class="fas fa-balance-scale"></i>&nbsp;Gerar Tabela Mensal para Avaliação do Motorista</h3>
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
                        <div class="col-md-12">
                            <div id="toastContainer" class="alert alert-warning alert-dismissible" style="display: none;">
                                <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                                <h5><i class="fas fa-exclamation-triangle"></i>Alerta!</h5>
                                Alertas
                            </div>
                        </div>
                        <div class="card-body">
                            <div class="row mb-3">
                                <div class="col-md-2">
                                    <label>Período de avaliação:</label>
                                    <asp:DropDownList ID="ddlMes" runat="server" CssClass="form-control">
                                        <asp:ListItem Text="Selecione o Mês" Value=""></asp:ListItem>
                                        <asp:ListItem Text="JAN" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="FEV" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="MAR" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="ABR" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="MAI" Value="5"></asp:ListItem>
                                        <asp:ListItem Text="JUN" Value="6"></asp:ListItem>
                                        <asp:ListItem Text="JUL" Value="7"></asp:ListItem>
                                        <asp:ListItem Text="AGO" Value="8"></asp:ListItem>
                                        <asp:ListItem Text="SET" Value="9"></asp:ListItem>
                                        <asp:ListItem Text="OUT" Value="10"></asp:ListItem>
                                        <asp:ListItem Text="NOV" Value="11"></asp:ListItem>
                                        <asp:ListItem Text="DEZ" Value="12"></asp:ListItem>
                                    </asp:DropDownList>
                                    
                                </div>
                                <div class="col-md-2">
                                    <label>&nbsp;</label>
                                    <asp:DropDownList ID="ddlAno" runat="server" CssClass="form-control">
                                        <asp:ListItem Text="Selecione o Ano" Value=""></asp:ListItem>
                                        <asp:ListItem Text="2025" Value="2025"></asp:ListItem>
                                        <asp:ListItem Text="2026" Value="2026"></asp:ListItem>
                                        <asp:ListItem Text="2027" Value="2027"></asp:ListItem>
                                        <asp:ListItem Text="2028" Value="2028"></asp:ListItem>
                                        <asp:ListItem Text="2029" Value="2029"></asp:ListItem>
                                        <asp:ListItem Text="2030" Value="2030"></asp:ListItem>
                                        <asp:ListItem Text="2031" Value="2031"></asp:ListItem>
                                        <asp:ListItem Text="2032" Value="2032"></asp:ListItem>
                                        <asp:ListItem Text="2033" Value="2033"></asp:ListItem>
                                        <asp:ListItem Text="2034" Value="2034"></asp:ListItem>
                                        <asp:ListItem Text="2035" Value="2035"></asp:ListItem>
                                        <asp:ListItem Text="2036" Value="2036"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-3">
                                    <!-- TextBox onde será exibido o valor -->
                                    <label>&nbsp;</label>
                                    <asp:TextBox ID="txtSelecionados" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row mb-3">
                                <div class="col-md-8">
                                    <div class="select2-purple">
                                        <label>Filial:</label>
                                        <asp:ListBox ID="ddlStatus" runat="server" SelectionMode="Multiple" CssClass="form-control select2"></asp:ListBox>
                                    </div>
                                </div>                               
                                <div class="col-md-1">
                                     <label>&nbsp;</label><br />
                                     <asp:Button ID="btnPesquisar" runat="server" Text="Filtrar" CssClass="btn btn-success" OnClick="btnPesquisar_Click" />   
                                </div>
                                 <div class="col-md-1">
                                  <label>&nbsp;</label><br />
                                  <asp:Button ID="btnGerarPlanilha" runat="server" Text="Gerar Planilha" CssClass="btn btn-warning" OnClick="btnGerarPlanilha_Click" />   
                             </div>
                            <div class="col-md-1">
                                     <label>&nbsp;</label><br />
                                     <asp:Button ID="btnImpmrimeMotoristas" runat="server" Text="Imprimir Relatórios" OnClick="btnImpmrimeMotoristas_Click" CssClass="btn btn-info" />   
                                </div>
                              <%--  <div class="col-md-2">
                                    <label>&nbsp;</label><br />
                                    <asp:Button ID="btnGerarTabela" runat="server" Text="Gerar Arquivo" CssClass="btn btn-success" Enabled="false" OnClick="btnGerarTabela_Click" />
                                </div>--%>
                            </div>
                            <%--</div>--%>
                            <div class="container-fluid">
                                <div class="row">
                                    <div class="col-12">
                                        <!-- /.col -->
                                        <div class="card">
                                            <!-- /.card-header -->
                                            <div class="card-body">
                                                <div class="table-responsive">
                                                    <asp:GridView runat="server" ID="gvMotoristas" CssClass="table table-bordered dataTable1 table-hover" Width="100%" AutoGenerateColumns="False" DataKeyNames="id" AllowPaging="True" PageSize="75" ShowHeaderWhenEmpty="True">
                                                        <PagerStyle HorizontalAlign="Center" CssClass="pagination-centered" />
                                                        <Columns>
                                                            <%--tamanho da foto 45x45--%>
                                                            <asp:ImageField DataImageUrlField="caminhofoto" HeaderText="FOTO" ControlStyle-Width="39" ItemStyle-Width="39" ControlStyle-CssClass="rounded-circle" ItemStyle-HorizontalAlign="Center" />
                                                            <asp:TemplateField HeaderText="CRACHÁ">
                                                                <ItemTemplate>
                                                                    <%# Eval("codmot") %>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="NOME DO MOTORISTA">
                                                                <ItemTemplate>
                                                                    <%# Eval("nommot") %>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="CARGO">
                                                                <ItemTemplate>
                                                                    <%# Eval("cargo") %>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="ADMISSÃO">
                                                                <ItemTemplate>
                                                                    <%# Eval("cadmot", "{0:dd/MM/yyyy}") %>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="FROTA">
                                                                <ItemTemplate>
                                                                    <%# Eval("frota") %>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="FILIAL">
                                                                <ItemTemplate>
                                                                    <%# Eval("nucleo") %>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="BONUS">
                                                                <ItemTemplate>
                                                                    <%# Eval("vl_total") %>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                    
                                                    <asp:TemplateField HeaderText="AÇÕES" ShowHeader="True" >
                                                        <ItemTemplate>
                                                            <br>
                                                            <asp:LinkButton ID="lnkEditar" runat="server" OnClick="Editar" CssClass="btn btn-primary btn-sm"><i class="fa fa-edit"></i> Avaliar</asp:LinkButton>
                                                          <%--  <a class="btn btn-danger btn-sm" href="#">
                                                                <i class="fa fa-trash"></i>
                                
                                                            </a>--%>
                                                        </ItemTemplate>
                                                         </asp:TemplateField>
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
