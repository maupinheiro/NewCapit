<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="ConsultaUsuarios.aspx.cs" Inherits="NewCapit.dist.pages.ConsultaUsuarios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" />
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
                    <i class="fas fa-address-card"></i>Controle de Usuários</h1>
                <%--<a href="$('#modalCadastro').modal('show'); return false;" class="d-none d-sm-inline-block btn btn-sm btn-primary shadow-sm"><i
                    class="fas fa-user-plus"></i>Novo Cadastro
                </a>--%>
                <asp:Button ID="btnAbrirModal" runat="server" Text="Novo Usuário" CssClass="btn btn-primary mb-3"
                    OnClientClick="$('#modalCadastro').modal('show'); return false;" />
            </div>
        </div>

        <!-- Corpo da grid -->
        <div class="card shadow mb-4">
            <div class="card-header">
                <asp:TextBox ID="myInput" CssClass="form-control myInput" OnTextChanged="myInput_TextChanged" placeholder="Pesquisar ..." AutoPostBack="true" runat="server" Width="100%"></asp:TextBox>
            </div>
            <div class="card-body">
                <div class="table-responsive">
                    <asp:GridView runat="server" ID="gvListUsuarios" CssClass="table table-bordered dataTable1 table-hover" Width="100%" AutoGenerateColumns="False" DataKeyNames="cod_usuario" AllowPaging="True" PageSize="10" OnPageIndexChanging="gvListUsuarios_PageIndexChanging" ShowHeaderWhenEmpty="True">
                        <PagerStyle HorizontalAlign="Center" CssClass="pagination-centered" />
                        <Columns>
                            <asp:ImageField DataImageUrlField="foto_usuario" HeaderText="#" ControlStyle-Width="45" ItemStyle-Width="45" ControlStyle-CssClass="rounded-circle" ItemStyle-HorizontalAlign="Center" />
                            <asp:TemplateField HeaderText="NOME COMPLETO/EMPRESA">
                                <ItemTemplate>
                                    <%# Eval("nm_nome") %>
                                    <br>
                                        <%# Eval("emp_usuario") %>
                                    </br>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="CARGO/DEPARTAMENTO">
                                <ItemTemplate>
                                    <%# Eval("fun_usuario") %>
                                    <br>
                                        <%# Eval("dep_usuario") %>
                                    </br>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="TIPO/STATUS">
                                <ItemTemplate>
                                    <%# Eval("fl_tipo") %>
                                    <br>
                                        <%# Eval("fl_status") %>
                                    </br>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="LOGIN/ÚLTIMO ACESSO">
                                <ItemTemplate>
                                    <%# Eval("nm_usuario") %>
                                    <br>
                                        <%# Eval("dt_ultimo_acesso")%>                                
                                    </br>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="AÇÕES" ShowHeader="True">
                                <ItemTemplate>
                                    <br>
                                    <asp:LinkButton ID="lnkEditar" runat="server" OnClick="Editar" CssClass="btn btn-primary btn-sm"><i class="fa fa-edit"></i> Editar</asp:LinkButton>
                                    <%--  <a class="btn btn-danger btn-sm" href="#">
                                <i class="fa fa-trash"></i>
                                
                            </a>--%>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
        <!-- Modal -->
        <div class="modal fade" id="modalCadastro" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered modal-xl" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">Cadastrar Usuário</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        <asp:Label runat="server" ID="lblMensagem" CssClass="text-success"></asp:Label>
                        <div class="row g-3">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <span class="details">NOME COMPLETO:</span>
                                    <asp:TextBox ID="txtNm_Nome" runat="server" CssClass="form-control" placeholder="" MaxLength="60"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group">
                                    <span class="details">USUÁRIO:</span>
                                    <asp:TextBox ID="txtNm_Usuario" runat="server" CssClass="form-control" placeholder="" MaxLength="60"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-2">
                                <div class="form-group">
                                    <span class="details">SENHA:</span>
                                    <asp:TextBox ID="txtDs_Senha" runat="server" CssClass="form-control" placeholder="" MaxLength="16"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row g-3">
                            <div class="col-md-2">
                                <div class="form-group">
                                    <span class="details">STATUS:</span>
                                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="">Selecione..</asp:ListItem>
                                        <asp:ListItem Value="A">ATIVO</asp:ListItem>
                                        <asp:ListItem Value="I">INATIVO</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="form-group">
                                    <span class="details">NÍVEL:</span>
                                    <asp:DropDownList ID="ddlNivel" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="">Selecione...</asp:ListItem>
                                        <asp:ListItem Value="A">ADMINISTRADOR</asp:ListItem>
                                        <asp:ListItem Value="O">OPERADOR</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-md-7">
                                <div class="form-group">
                                    <span class="details">E-MAIL:</span>
                                    <asp:TextBox ID="txtDs_Email" runat="server" CssClass="form-control" placeholder="" MaxLength="40"></asp:TextBox>
                                </div>
                            </div>

                        </div>
                        <div class="row g-3">
                            <div class="col-md-4">
                                <div class="form-group">
                                    <span class="details">FUNÇÃO:</span>
                                    <asp:DropDownList ID="ddlFun_Usuario" runat="server" class="form-control" ReadOnly="true" placeholder=""></asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group">
                                    <span class="details">DEPARTAMENTO:</span>
                                    <asp:DropDownList ID="ddlDep_Usuario" runat="server" class="form-control" ReadOnly="true" placeholder=""></asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group">
                                    <span class="details">FILIAL:</span>
                                    <asp:DropDownList ID="ddlEmp_Usuario" runat="server" class="form-control" ReadOnly="true"></asp:DropDownList>
                                </div>
                            </div>
                        </div>


                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnSalvar" runat="server" Text="Salvar" CssClass="btn btn-success" OnClick="btnSalvar_Click" />
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Sair</button>
                    </div>
                </div>
            </div>
        </div>

    </div>
    <!-- Scripts Bootstrap -->
    <script src="https://code.jquery.com/jquery-3.5.1.slim.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@4.5.2/dist/js/bootstrap.bundle.min.js"></script>
</asp:Content>
