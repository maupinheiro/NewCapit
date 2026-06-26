<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="ConsultaUsuarios.aspx.cs" Inherits="NewCapit.dist.pages.ConsultaUsuarios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>

    <style>
        /* Centralização e estilização da paginação nativa do GridView */
        .pagination-centered {
            text-align: center;
            padding-top: 15px;
        }

            .pagination-centered table {
                margin: 0 auto;
            }

                .pagination-centered table table td {
                    border: none;
                    padding: 0 4px;
                }

            .pagination-centered a, .pagination-centered span {
                display: inline-block;
                padding: 6px 12px;
                border: 1px solid #dee2e6;
                border-radius: 4px;
                text-decoration: none;
                color: #0d6efd;
                background-color: #fff;
            }

            .pagination-centered span {
                color: #fff;
                background-color: #0d6efd;
                border-color: #0d6efd;
                font-weight: bold;
            }

            .pagination-centered a:hover {
                background-color: #e9ecef;
            }

        /* Ajustes finos de design para o GridView */
        .table-usuarios th {
            background-color: #f8f9fa !important;
            color: #495057 !important;
            font-weight: 600;
            text-transform: uppercase;
            font-size: 0.82rem;
            letter-spacing: 0.5px;
            border-bottom: 2px solid #dee2e6 !important;
        }

        .table-usuarios td {
            font-size: 0.9rem;
            vertical-align: middle !important;
        }

        .text-sub-info {
            font-size: 0.78rem;
            color: #6c757d;
            display: block;
            margin-top: 2px;
        }

        .details-label {
            font-size: 0.8rem;
            font-weight: 600;
            color: #6c757d;
            text-transform: uppercase;
            margin-bottom: 4px;
            display: block;
        }
    </style>

    <script>
        function fecharModalCadastro() {
            var modalEl = document.getElementById('modalCadastro');
            var modal = bootstrap.Modal.getInstance(modalEl);

            if (modal) {
                modal.hide();
            }
        }
</script>
    <div class="content-wrapper">
        <div class="content-header px-3">
            <div class="container-fluid">
                <div class="d-sm-flex align-items-center justify-content-between my-4">
                    <h1 class="h4 mb-0 text-gray-800 fw-bold">
                        <i class="fas fa-address-card text-primary me-2"></i>Controle de Usuários
                    </h1>
                    <asp:Button ID="btnAbrirModal" runat="server" Text="Novo Usuário" CssClass="btn btn-primary shadow-sm px-4"
                        OnClientClick="$('#modalCadastro').modal('show'); return false;" />
                </div>
            </div>
        </div>

        <div class="content px-3">
            <div class="container-fluid">

                <div class="card shadow-sm border-0 rounded-3 mb-4">
                    <div class="card-header bg-white py-3 border-bottom-0">
                        <div class="row">
                            <div class="col-md-4 ms-auto">
                                <div class="input-group shadow-sm">
                                    <span class="input-group-text bg-light border-end-0"><i class="fas fa-search text-muted"></i></span>
                                    <asp:TextBox ID="myInput" CssClass="form-control bg-light border-start-0 myInput" OnTextChanged="myInput_TextChanged" placeholder="Pesquisar..." AutoPostBack="true" runat="server"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="card-body p-0">
                        <div class="table-responsive">
                            <asp:GridView runat="server" ID="gvListUsuarios" CssClass="table table-striped table-hover table-usuarios mb-0" Width="100%" AutoGenerateColumns="False" DataKeyNames="cod_usuario" AllowPaging="True" PageSize="10" OnPageIndexChanging="gvListUsuarios_PageIndexChanging" ShowHeaderWhenEmpty="True" GridLines="None">
                                <PagerStyle HorizontalAlign="Center" CssClass="pagination-centered" />
                                <Columns>

                                    <asp:ImageField DataImageUrlField="foto_usuario" HeaderText="#" ControlStyle-Width="38" ItemStyle-Width="50" ControlStyle-CssClass="rounded-circle border shadow-sm" ItemStyle-HorizontalAlign="Center" />

                                    <asp:TemplateField HeaderText="Nome Completo / Empresa">
                                        <ItemTemplate>
                                            <span class="fw-bold text-dark"><%# Eval("nm_nome") %></span>
                                            <span class="text-sub-info"><i class="fas fa-building me-1"></i><%# Eval("emp_usuario") %></span>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Cargo / Departamento">
                                        <ItemTemplate>
                                            <span class="text-secondary"><%# Eval("fun_usuario") %></span>
                                            <span class="text-sub-info"><i class="fas fa-folder me-1"></i><%# Eval("dep_usuario") %></span>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Nível / Status">
                                        <ItemTemplate>
                                            <span class="d-block mb-1">
                                                <%# Eval("fl_tipo").ToString().ToUpper() == "A" ? "<span class='badge bg-primary-subtle text-primary border border-primary-subtle'>ADMINISTRADOR</span>" : "<span class='badge bg-secondary-subtle text-secondary border border-secondary-subtle'>OPERADOR</span>" %>
                                            </span>

                                            <span class="d-block">
                                                <%# Eval("fl_status").ToString().ToUpper().Contains("ATIVO") || Eval("fl_status").ToString().ToUpper() == "A" ? "<span class='badge bg-success-subtle text-success border border-success-subtle'>ATIVO</span>" : "<span class='badge bg-danger-subtle text-danger border border-danger-subtle'>INATIVO</span>" %>
                                            </span>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Login / Último Acesso">
                                        <ItemTemplate>
                                            <span class="text-dark fw-semibold"><%# Eval("nm_usuario") %></span>
                                            <span class="text-sub-info"><i class="far fa-clock me-1"></i><%# Eval("dt_ultimo_acesso") %></span>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                                                           <asp:TemplateField HeaderText="Ações" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="180">
                                            <ItemTemplate>
                                                <div class="d-flex justify-content-center gap-2">
                                                    <asp:LinkButton ID="lnkEditar" runat="server" OnClick="Editar" CausesValidation="false" CssClass="btn btn-outline-primary btn-sm rounded-2">
                                                        <i class="fa fa-edit"></i>
                                                    </asp:LinkButton>
            
                                                    <asp:LinkButton ID="lnkResetar" runat="server" OnClick="ResetarSenha_Click" 
                                                        CommandArgument='<%# Eval("cod_usuario") %>' 
                                                        OnClientClick="return confirm('Tem certeza que deseja resetar a senha deste usuário para \'mudar123\'?');"
                                                        CssClass="btn btn-outline-warning btn-sm rounded-2">
                                                        <i class="fa fa-key"></i> Resetar
                                                    </asp:LinkButton>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>

            </div>
        </div>

        <div class="modal fade" id="modalCadastro" tabindex="-1" role="dialog" aria-labelledby="modalCadastroLabel" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered modal-xl" role="document">
                <div class="modal-content border-0 shadow-lg rounded-3">
                    <div class="modal-header bg-primary text-white py-3">
                        <h5 class="modal-title fw-bold" id="modalCadastroLabel">
                            <i class="fas fa-user-plus me-2"></i>Cadastrar Usuário
                        </h5>
                        <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body p-4">
                        <asp:Label runat="server" ID="lblMensagem" CssClass="d-block text-success mb-3 fw-semibold"></asp:Label>

                        <div class="row g-3 mb-3">
                            <div class="col-md-6">
                                <label class="details-label">Nome Completo:</label>
                                <asp:TextBox ID="txtNm_Nome" runat="server" CssClass="form-control shadow-sm" placeholder="Ex: Maurício Silva" MaxLength="60"></asp:TextBox>
                            </div>
                            <div class="col-md-6">
                                <label class="details-label">Usuário:</label>
                                <asp:TextBox ID="txtNm_Usuario" runat="server" CssClass="form-control shadow-sm" placeholder="Ex: mauricio.silva" MaxLength="60"></asp:TextBox>
                            </div>
                           
                        </div>

                        <div class="row g-3 mb-3">
                            <div class="col-md-2">
                                <label class="details-label">Status:</label>
                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-select shadow-sm">
                                    <asp:ListItem Value="">Selecione..</asp:ListItem>
                                    <asp:ListItem Value="A">ATIVO</asp:ListItem>
                                    <asp:ListItem Value="I">INATIVO</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-3">
                                <label class="details-label">Nível:</label>
                                <asp:DropDownList ID="ddlNivel" runat="server" CssClass="form-select shadow-sm">
                                    <asp:ListItem Value="">Selecione...</asp:ListItem>
                                    <asp:ListItem Value="A">ADMINISTRADOR</asp:ListItem>
                                    <asp:ListItem Value="O">OPERADOR</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-7">
                                <label class="details-label">E-mail:</label>
                                <asp:TextBox ID="txtDs_Email" runat="server" CssClass="form-control shadow-sm" placeholder="nome@empresa.com.br" MaxLength="40"></asp:TextBox>
                            </div>
                        </div>

                        <%--<div class="row g-3"> --%>
                        <asp:UpdatePanel ID="updSetorFuncao" runat="server">
                            <ContentTemplate>
                                <div class="row g-3">
                                    <div class="col-md-4">
                                        <label class="details-label">Departamento:</label>
                                        <asp:DropDownList ID="ddlDep_Usuario"
                                            runat="server"
                                            AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlDep_Usuario_SelectedIndexChanged"
                                            CssClass="form-select shadow-sm">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-4">
                                        <label class="details-label">Função:</label>
                                        <asp:DropDownList ID="ddlFun_Usuario"
                                            runat="server"
                                            CssClass="form-select shadow-sm">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-4">
                                        <label class="details-label">Filial:</label>
                                        <asp:DropDownList ID="ddlEmp_Usuario" runat="server" CssClass="form-select shadow-sm" ReadOnly="true"></asp:DropDownList>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                        <%--</div>--%>
                    </div>
                    <div class="modal-footer bg-light py-3">
                        <%--<button type="button" class="btn btn-outline-secondary px-4" data-bs-dismiss="modal">Sair</button>--%>
                        <button type="button"
                            class="btn btn-outline-secondary px-4"
                            onclick="fecharModalCadastro();">
                            Sair
                        </button>
                        <asp:Button ID="btnSalvar" runat="server" Text="Salvar" CssClass="btn btn-success px-4" OnClick="btnSalvar_Click" />
                    </div>
                </div>
            </div>
        </div>

    </div>

    <script src="https://code.jquery.com/jquery-3.6.4.min.js"></script>
</asp:Content>
