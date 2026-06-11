<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="ControleAcesso2.aspx.cs" Inherits="NewCapit.dist.pages.ControleAcesso2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    <link href="https://cdn.jsdelivr.net/npm/select2@4.1.0/dist/css/select2.min.css" rel="stylesheet" />
    <script src="https://code.jquery.com/jquery-3.7.1.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/select2@4.1.0/dist/js/select2.min.js"></script>
    
    <style>
        .select2-container .select2-selection--single {
            height: 38px;
            padding: 5px;
        }
        .card-modulo {
            border-left: 4px solid #0d6efd;
            margin-bottom: 20px;
        }
        .header-modulo {
            background-color: #f8f9fa;
            font-weight: bold;
        }
        /* Alinhamento fino para manter altura padrão dos selects e botão */
        .h-38 {
            height: 38px !important;
        }
    </style>

    <script>
        function initSelect2() {
            $('.select2').select2({
                width: '100%',
                placeholder: 'Selecione um usuário...',
                allowClear: false
            });
        }

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(initSelect2);
        $(document).ready(initSelect2);

        function alternarModulo(chkModulo, containerId) {
            var mContainer = document.getElementById(containerId);
            var checkboxes = mContainer.getElementsByTagName('input');
            for (var i = 0; i < checkboxes.length; i++) {
                if (checkboxes[i].type == 'checkbox') {
                    checkboxes[i].checked = chkModulo.checked;
                }
            }
        }
    </script>   

    <div class="content-wrapper">
        <div class="container mt-4">
            
            <div id="divMsg" runat="server" class="alert alert-success alert-dismissible fade show mt-3" style="display: none;" role="alert">
                <span id="lblMsg" runat="server"></span>
                <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
            </div>

            <div class="card shadow mb-4">
                <div class="card-header bg-primary text-white">
                    <h5 class="mb-0"><i class="fas fa-user-lock"></i>&nbsp; Gestão de Permissões de Acesso</h5>
                </div>
                <div class="card-body">
                    <div class="row align-items-end g-3">
                        
                        <div class="col-md-4">
                            <label class="form-label fw-bold mb-1 text-secondary">IDENTIFICAÇÃO DO PERFIL</label>
                            <div class="p-2 border rounded bg-light">
                                <span class="text-muted small d-block">Nome completo:</span>
                                <asp:Label ID="lblNome" runat="server" CssClass="fw-bold text-dark fs-6"></asp:Label>
                                <span class="text-muted small d-block mt-1">Usuário de acesso:</span>
                                <asp:Label ID="lblUsuario" runat="server" CssClass="fw-bold text-secondary"></asp:Label>
                            </div>
                        </div>

                        <div class="col-md-2">
                            <label class="form-label fw-bold mb-1 text-secondary">STATUS:</label>
                            <asp:DropDownList ID="ddlStatusAcesso" runat="server" CssClass="form-select h-38 fw-semibold text-dark">
                                <asp:ListItem Value="A">ATIVO</asp:ListItem>
                                <asp:ListItem Value="I">INATIVO</asp:ListItem>
                            </asp:DropDownList>
                        </div>

                        <div class="col-md-3">
                            <label class="form-label fw-bold mb-1 text-secondary">TIPO DE USUÁRIO:</label>
                            <asp:DropDownList ID="ddlNivelAcesso" runat="server" CssClass="form-select h-38 fw-semibold text-dark">
                                <asp:ListItem Value="O">USUÁRIO (OPERADOR)</asp:ListItem>
                                <asp:ListItem Value="A">ADMINISTRADOR</asp:ListItem>
                            </asp:DropDownList>
                        </div>

                        <div class="col-md-3">
                            <asp:Button ID="btnSalvarPermissoes" runat="server" Text="Salvar Todas as Alterações" 
                                CssClass="btn btn-success w-100 h-38 fw-bold" OnClick="btnSalvarPermissoes_Click" Enabled="false" />
                        </div>

                    </div>
                </div>
            </div>

            <asp:Panel ID="pnlPermissoes" runat="server" Visible="false">
                
                <asp:Repeater ID="rptModulos" runat="server" OnItemDataBound="rptModulos_ItemDataBound">
                    <ItemTemplate>
                        <asp:HiddenField ID="hfIdModulo" runat="server" Value='<%# Eval("id_modulo") %>' />
                        
                        <div class="card shadow-sm card-modulo" id='modulo_<%# Eval("id_modulo") %>'>
                            <div class="card-header header-modulo d-flex justify-content-between align-items-center">
                                <div>
                                    <i class="fas fa-cubes text-secondary"></i>&nbsp;
                                    <span class="fs-5 text-dark"><%# Eval("nome_modulo") %></span>
                                </div>
                                <div class="form-check">
                                    <asp:CheckBox ID="chkMod" runat="server" CssClass="form-check-input" 
                                                  onclick='<%# "alternarModulo(this, \"telas_container_" + Eval("id_modulo") + "\")" %>' />
                                    <label class="form-check-label text-muted" runat="server" id="lblMod" for='<%# Container.FindControl("chkMod").ClientID %>'>Liberar Módulo</label>
                                </div>
                            </div>
                            
                            <div class="card-body" id='telas_container_<%# Eval("id_modulo") %>'>
                                <table class="table table-hover align-middle mb-0">
                                    <thead class="table-light">
                                        <tr>
                                            <th style="width: 40%;">Tela / Funcionalidade</th>
                                            <th class="text-center" style="width: 15%;">Visualizar</th>
                                            <th class="text-center" style="width: 15%;">Inserir</th>
                                            <th class="text-center" style="width: 15%;">Alterar</th>
                                            <th class="text-center" style="width: 15%;">Excluir</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater ID="rptTelas" runat="server">
                                            <ItemTemplate>
                                                <tr>
                                                    <td>
                                                        <asp:HiddenField ID="hfIdTela" runat="server" Value='<%# Eval("IdTela") %>' />
                                                        <span class="fw-bold text-secondary"><%# Eval("NomeTela") %></span><br />
                                                        <small class="text-muted"><%# Eval("ArquivoPhysical") %></small>
                                                    </td>
                                                    <td class="text-center">
                                                        <asp:CheckBox ID="chkVisualizar" runat="server" CssClass="form-check-input d-inline-block" />
                                                    </td>
                                                    <td class="text-center">
                                                        <asp:CheckBox ID="chkInserir" runat="server" CssClass="form-check-input d-inline-block" />
                                                    </td>
                                                    <td class="text-center">
                                                        <asp:CheckBox ID="chkAlterar" runat="server" CssClass="form-check-input d-inline-block" />
                                                    </td>
                                                    <td class="text-center">
                                                        <asp:CheckBox ID="chkExcluir" runat="server" CssClass="form-check-input d-inline-block" />
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tbody>
                                </table>
                            </div>
                        </div>

                    </ItemTemplate>
                </asp:Repeater>

            </asp:Panel>

        </div>
    </div>
</asp:Content>
