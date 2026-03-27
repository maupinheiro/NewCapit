<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="ColaboradoresManutencao.aspx.cs" Inherits="NewCapit.dist.pages.ColaboradoresManutencao" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery.mask/1.14.16/jquery.mask.min.js"></script>

    <script>
        function aplicarMascaras() {
            // CPF
            var cpf = $('#<%= txtCPF.ClientID %>');
            if (cpf.length) {
                cpf.mask('000.000.000-00');
            }

            // Telefone
            var telefone = $('#<%= txtTelefone.ClientID %>');
            if (telefone.length) {
                var behavior = function (val) {
                    val = val.replace(/\D/g, '');
                    return val.length === 11 ? '(00) 0 0000-0000' : '(00) 0000-0000';
                };
                var options = {
                    onKeyPress: function (val, e, field, options) {
                        field.mask(behavior.apply({}, arguments), options);
                    }
                };
                telefone.mask(behavior, options);
            }
        }

        // Aplica máscaras no carregamento da página normal
        $(document).ready(function () {
            aplicarMascaras();
        });

        // Se você usa UpdatePanel, aplica máscaras após cada postback parcial
        if (typeof (Sys) !== "undefined") {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                aplicarMascaras();
            });
        }

        // Preview da foto
        function previewImagem(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#<%= imgPreview.ClientID %>').attr('src', e.target.result).show();
                }
                reader.readAsDataURL(input.files[0]);
                $('#<%= hfFoto.ClientID %>').val(input.value);
            }
        }
    </script>
    <script>
        function formatarData(input) {
            let v = input.value.replace(/\D/g, '');

            if (v.length > 2) v = v.substring(0, 2) + '/' + v.substring(2);
            if (v.length > 5) v = v.substring(0, 5) + '/' + v.substring(5, 9);

            input.value = v;
        }
    </script>
    <script>
        function controlarStatus() {
            var status = document.getElementById('<%= ddlStatus.ClientID %>').value;

            var div = document.getElementById('<%= divDemissao.ClientID %>');
            var demissao = document.getElementById('<%= txtDemissao.ClientID %>');
            var motivo = document.getElementById('<%= txtMotivo.ClientID %>');

            if (status === "INATIVO") {
                div.style.display = "flex";

                // aplica vermelho se vazio
                if (demissao.value === "") {
                    demissao.classList.add("texto-vermelho");
                }
                if (motivo.value === "") {
                    motivo.classList.add("texto-vermelho");
                }
            } else {
                div.style.display = "none";

                demissao.value = "";
                motivo.value = "";

                demissao.classList.remove("texto-vermelho");
                motivo.classList.remove("texto-vermelho");
            }
        }

        function validarCamposDemissao() {
            var status = document.getElementById('<%= ddlStatus.ClientID %>').value;

            var demissao = document.getElementById('<%= txtDemissao.ClientID %>');
            var motivo = document.getElementById('<%= txtMotivo.ClientID %>');

            let valido = true;

            if (status === "INATIVO") {
                if (demissao.value === "") {
                    demissao.classList.add("texto-vermelho");
                    valido = false;
                } else {
                    demissao.classList.remove("texto-vermelho");
                }

                if (motivo.value === "") {
                    motivo.classList.add("texto-vermelho");
                    valido = false;
                } else {
                    motivo.classList.remove("texto-vermelho");
                }
            }

            return valido;
        }
    </script>
    <script>
        function removerErro(campo) {
            campo.classList.remove("texto-vermelho");
        }
    </script>
    <div class="content-wrapper">
        <section class="content">
            <div class="container-fluid">
                <br />
                <div class="col-md-12">
                    <div class="card card-info">
                        <!-- Header -->
                        <div class="card-header" style="background-color: #A020F0; font-weight: bold;">
                            <h3 class="card-title"><i class="fas fa-shipping-fast"></i>&nbsp;Manutenção</h3>
                        </div>

                        <!-- Formulário de cadastro -->
                        <div class="card-body">
                            <div class="card-header bg-secondary text-white">
                                Cadastra Colaborador da Manutenção
                            </div>

                            <asp:HiddenField ID="hfId" runat="server" />
                            <asp:HiddenField ID="hfFoto" runat="server" />

                            <div id="divMsg" runat="server" class="alert alert-dismissible fade show mt-3" role="alert" visible="false">
                                <asp:Label ID="lblMsgGeral" runat="server"></asp:Label>
                                <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
                            </div>

                            <!-- Preview da foto -->
                            <div class="row g-3 mb-3">
                                <div class="col-md-1 text-center">
                                    <img id="imgPreview" runat="server" src="" style="width: 80px; display: none;" class="rounded-circle" />
                                </div>
                            </div>

                            <!-- Campos -->
                            <div class="row g-3">
                                <div class="col-md-1">
                                    <label>Crachá:</label>
                                    <asp:TextBox ID="txtCracha" TextMode="Number" runat="server" CssClass="form-control" OnTextChanged="txtCracha_TextChanged" AutoPostBack="true" />
                                </div>
                                <div class="col-md-3">
                                    <label>Nome:</label>
                                    <asp:TextBox ID="txtNome" runat="server" CssClass="form-control" />
                                </div>
                                <div class="col-md-2">
                                    <label>CPF:</label>
                                    <asp:TextBox ID="txtCPF" runat="server" CssClass="form-control" />
                                </div>
                                <div class="col-md-2">
                                    <label>Função:</label>
                                    <asp:DropDownList ID="ddlCargo" runat="server" CssClass="form-control select2" />
                                </div>
                                <div class="col-md-2">
                                    <label>Horário:</label>
                                    <asp:DropDownList ID="ddlJornada" runat="server" CssClass="form-control select2" />
                                </div>
                                <div class="col-md-2">
                                    <label>Filial:</label>
                                    <asp:DropDownList ID="cbFiliais" runat="server" CssClass="form-control select2" />
                                </div>
                            </div>

                            <div class="row g-3 mt-2">
                                <div class="col-md-1">
                                    <label>Admissão:</label>
                                    <asp:TextBox ID="txtAdmissao" runat="server" CssClass="form-control" MaxLength="10" onkeyup="formatarData(this)" />
                                </div>
                                <div class="col-md-1">
                                    <label>Status:</label>
                                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control"
                                        AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">   
                                        <asp:ListItem Text="" />
                                        <asp:ListItem Text="ATIVO" />
                                        <asp:ListItem Text="INATIVO" />
                                    </asp:DropDownList>                                   
                                </div>
                                <div class="col-md-2">
                                    <label>Telefone:</label>
                                    <asp:TextBox ID="txtTelefone" runat="server" CssClass="form-control" />
                                </div>
                                <div class="col-md-4">
                                    <label>Email:</label>
                                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" />
                                </div>
                                <div class="col-md-3">
                                    <label>Foto:</label>
                                    <asp:FileUpload ID="fuFoto" runat="server" CssClass="form-control" onchange="previewImagem(this)" />
                                </div>
                                <div class="col-md-1 d-flex align-items-end">
                                    <asp:Button ID="btnSalvar" runat="server" Text="Salvar" CssClass="btn btn-success w-100" OnClick="btnSalvar_Click" />
                                </div>
                            </div>
                            <div id="divDemissao" runat="server" class="row g-3 mt-2" visible="false">
                                <div class="col-md-1">
                                    <label>Demissão:</label>
                                    <asp:TextBox ID="txtDemissao" runat="server"
                                        CssClass="form-control"
                                        onkeyup="formatarData(this); removerErro(this)" />
                                </div>
                                <div class="col-md-11">
                                    <label>Motivo:</label>
                                    <asp:TextBox ID="txtMotivo" runat="server"
                                        CssClass="form-control"
                                        onkeyup="removerErro(this)" />
                                </div>
                            </div>
                        </div>

                        <!-- Grid de colaboradores -->
                        <div class="card-body">
                            <div class="card-header bg-secondary text-white">
                                Lista de Colaboradores da Manutenção       
                            </div>
                            <br />
                            <div class="row mb-3">
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtBusca" runat="server" CssClass="form-control" placeholder="Buscar por nome..." AutoPostBack="true" OnTextChanged="txtBusca_TextChanged" />
                                </div>
                            </div>

                            <asp:GridView ID="gvProfissionais" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-striped table-hover"
                                HeaderStyle-CssClass="gv-header-custom"
                                OnRowCommand="gvProfissionais_RowCommand">
                                <Columns>
                                    <asp:TemplateField HeaderText="Foto">
                                        <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Image ID="imgFoto" runat="server" ImageUrl='<%# Eval("foto") %>' CssClass="rounded-circle" Width="50px" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="id" HeaderText="Id" Visible="false">
                                        <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="cracha" HeaderText="Crachá">
                                        <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="nome" HeaderText="Nome Completo" />

                                    <asp:BoundField DataField="funcao" HeaderText="Função" />

                                    <asp:BoundField DataField="horario" HeaderText="Horário de Trabalho">
                                        <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="filial" HeaderText="Filial" />

                                    <asp:BoundField DataField="telefone" HeaderText="Celular">
                                        <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="admissao" HeaderText="Admissão" DataFormatString="{0:dd/MM/yyyy}" HtmlEncode="false">
                                        <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="status" HeaderText="Status">
                                        <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Ações">
                                        <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" CommandName="Editar" CommandArgument='<%# Eval("id") %>' CssClass="btn btn-sm btn-primary">Editar</asp:LinkButton>
                                            <asp:LinkButton runat="server" CommandName="Excluir" CommandArgument='<%# Eval("id") %>' CssClass="btn btn-sm btn-danger"
                                                OnClientClick="return confirm('Deseja excluir?');">
                                                Excluir</asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
        </section>
    </div>
</asp:Content>
