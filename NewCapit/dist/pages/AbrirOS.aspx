<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="AbrirOS.aspx.cs" Inherits="NewCapit.dist.pages.AbrirOS" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .campo-erro {
            border: 1px solid #dc3545 !important;
            background-color: #fff5f5;
        }
    </style>

    <script>
        function somenteNumeros(e) {
            var tecla = (window.event) ? event.keyCode : e.which;

            // Permite backspace, delete, setas
            if (tecla == 8 || tecla == 37 || tecla == 39 || tecla == 46)
                return true;

            // Permite apenas números 0–9
            if (tecla >= 48 && tecla <= 57)
                return true;

            return false;
        }
    </script>
    <div class="content-wrapper">
        <section class="content">
            <div class="container-fluid">
                <div class="col-md-12">
                    <div class="card card-info">
                        <div class="card-header" style="background-color: #A020F0; font-weight: bold;">
                            <h3 class="card-title">
                                <h3 class="card-title"><i class="fas fa-shipping-fast"></i>&nbsp;Manutenção</h3>
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
                            <div class="card shadow">
                                <div class="card-header bg-secondary text-white">
                                    <h5>Abrir Ordem de Serviço</h5>
                                </div>
                                <div class="card-body">
                                    <br />
                                    <div id="divMsg" runat="server"
                                        class="alert alert-dismissible fade show mt-3"
                                        role="alert" visible="false">

                                        <asp:Label ID="lblMsgGeral" runat="server"></asp:Label>

                                        <button type="button" class="btn-close" data-bs-dismiss="alert"></button>

                                    </div>

                                    <div class="row">
                                        <div class="col-md-1">
                                            <label>Motorista:</label>
                                            <asp:TextBox
                                                ID="txtId_Motorista"
                                                runat="server"
                                                CssClass="form-control"
                                                AutoPostBack="true"
                                                OnTextChanged="txtId_Motorista_TextChanged">                                       </asp:TextBox>
                                            <%--Style="text-align: center"--%>
                                            <!-- negrito na class form-control  font-weight-bold -->
                                        </div>

                                        <div class="col-md-4">
                                            <label>Nome Completo:</label>
                                            <asp:DropDownList
                                                ID="ddlNome_Motorista"
                                                runat="server"
                                                class="form-control font-weight-bold select2"
                                                OnSelectedIndexChanged="ddlNome_Motorista_SelectedIndexChanged"
                                                AutoPostBack="true">
                                            </asp:DropDownList>
                                        </div>

                                        <div class="col-md-4">
                                            <label>Transportadora:</label>
                                            <asp:TextBox ID="txtTransp_Motorista"
                                                runat="server"
                                                CssClass="form-control"
                                                ReadOnly="true">
                                            </asp:TextBox>
                                        </div>

                                        <div class="col-md-3">
                                            <label>Núcleo:</label>
                                            <asp:TextBox ID="txtNucleo_Motorista" runat="server" CssClass="form-control" ReadOnly="true" />
                                        </div>

                                    </div>
                                    <hr />

                                    <div class="row">

                                        <div class="col-md-1">
                                            <label>Código:</label>
                                            <asp:TextBox ID="txtCodVeiculo"
                                                runat="server"
                                                CssClass="form-control"
                                                AutoPostBack="true"
                                                OnTextChanged="txtCodVeiculo_TextChanged">
                                            </asp:TextBox>
                                        </div>

                                        <div class="col-md-1">
                                            <label>Placa:</label>
                                            <asp:TextBox
                                                ID="txtPlaca"
                                                runat="server"
                                                CssClass="form-control"
                                                AutoPostBack="true"
                                                OnTextChanged="txtPlaca_TextChanged">
                                            </asp:TextBox>
                                        </div>

                                        <div class="col-md-1">
                                            <label>Km Atual:</label>
                                            <asp:TextBox ID="txtKm"
                                                runat="server"
                                                CssClass="form-control"
                                                onkeypress="return somenteNumeros(event)"
                                                MaxLength="6"
                                                >
                                            </asp:TextBox>
                                        </div>

                                        <div class="col-md-2">
                                            <label>Tipo:</label>
                                            <asp:TextBox ID="txtTipVei" runat="server" CssClass="form-control" ReadOnly="true" />
                                        </div>

                                        <div class="col-md-2">
                                            <label>Marca</label>
                                            <asp:TextBox ID="txtMarca" runat="server" CssClass="form-control" ReadOnly="true" />
                                        </div>

                                        <div class="col-md-2">
                                            <label>Modelo:</label>
                                            <asp:TextBox ID="txtModelo" runat="server" CssClass="form-control" ReadOnly="true" />
                                        </div>

                                        <div class="col-md-1">
                                            <label>Ano:</label>
                                            <asp:TextBox ID="txtAno" runat="server" CssClass="form-control" ReadOnly="true" />
                                        </div>

                                        <div class="col-md-2">
                                            <label>Núcleo:</label>
                                            <asp:TextBox ID="txtNucleo" runat="server" CssClass="form-control" ReadOnly="true" />
                                        </div>

                                    </div>

                                    <hr />

                                    <div class="row">

                                        <div class="col-md-2">
                                            <label>Última OS:</label>
                                            <asp:TextBox ID="txtUltimaOS" runat="server" CssClass="form-control" ReadOnly="true" Style="text-align: center" />
                                        </div>

                                        <div class="col-md-2">
                                            <label>Emissão:</label>
                                            <asp:TextBox ID="txtDataUltimaOS" runat="server" CssClass="form-control" ReadOnly="true" Style="text-align: center" />
                                        </div>

                                        <div class="col-md-2">
                                            <label>Status:</label>
                                            <asp:TextBox ID="txtStatusUltOS" runat="server" CssClass="form-control" ReadOnly="true" Style="text-align: center" />
                                        </div>

                                        <div class="col-md-1">
                                            <label>Tipo OS</label>
                                            <asp:DropDownList ID="ddlTipo" runat="server" CssClass="form-control">
                                                <asp:ListItem Value="0">Selecione...</asp:ListItem>
                                                <asp:ListItem Value="C">Corretiva</asp:ListItem>
                                                <asp:ListItem Value="P">Preventiva</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>

                                        <div class="col-md-1">
                                            <label>Tipo Serviço</label>
                                            <asp:DropDownList ID="ddlTipoServico"
                                                runat="server"
                                                CssClass="form-control"
                                                AutoPostBack="true"
                                                OnSelectedIndexChanged="ddlTipoServico_SelectedIndexChanged">
                                                <asp:ListItem Text="Selecione..." Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Interno" Value="I"></asp:ListItem>
                                                <asp:ListItem Text="Externo" Value="E"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>

                                        <div class="col-md-4" id="divFornecedor" runat="server" visible="false">
                                            <label>Fornecedor:</label>
                                            <asp:DropDownList ID="cboFornecedores"
                                                runat="server"
                                                CssClass="form-control select2">
                                            </asp:DropDownList>
                                        </div>
                                    </div>

                                    <br />
                                    <div class="card card-outline card-info collapsed-card">
                                        <div class="card-header">
                                            <h3 class="card-title"><i class="far fa-edit"></i>&nbsp;Mecânica</h3>
                                            <div class="card-tools">
                                                <button type="button" class="btn btn-tool" data-card-widget="collapse">
                                                    <i class="fas fa-plus"></i>
                                                </button>
                                            </div>

                                        </div>

                                        <div class="card-body">
                                            <div class="form-group">
                                                <label>Descrição do Problema:</label>
                                                <asp:TextBox ID="txtParteMecanica" runat="server"
                                                    TextMode="MultiLine"
                                                    Rows="5"
                                                    Placeholder="
                                                    Descreva os problemas enumerando. Ex.:
                                                    1 - Regular freios, próxima linha
                                                    2 - Vazamento de água no radiador ..."
                                                    CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>

                                    </div>
                                    <br />
                                    <div class="card card-outline card-info collapsed-card">
                                        <div class="card-header">
                                            <h3 class="card-title"><i class="far fa-edit"></i>&nbsp;Eletrica</h3>
                                            <div class="card-tools">
                                                <button type="button" class="btn btn-tool" data-card-widget="collapse">
                                                    <i class="fas fa-plus"></i>
                                                </button>
                                            </div>

                                        </div>

                                        <div class="card-body">
                                            <div class="form-group">
                                                <label>Descrição do Problema:</label>
                                                <asp:TextBox ID="txtParteEletrica" runat="server"
                                                    TextMode="MultiLine"
                                                    Rows="5"
                                                    Placeholder="
                                                    Descreva os problemas enumerando. Ex.: 
                                                    1 - Trocar farol lado direito, próxima linha
                                                    2 - Lampada do freio lado direito queimada ..."
                                                    CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>

                                    </div>
                                    <br />
                                    <div class="card card-outline card-info collapsed-card">
                                        <div class="card-header">
                                            <h3 class="card-title"><i class="far fa-edit"></i>&nbsp;Borracharia</h3>
                                            <div class="card-tools">
                                                <button type="button" class="btn btn-tool" data-card-widget="collapse">
                                                    <i class="fas fa-plus"></i>
                                                </button>
                                            </div>

                                        </div>

                                        <div class="card-body">
                                            <div class="form-group">
                                                <label>Descrição do Problema:</label>
                                                <asp:TextBox ID="txtParteBorracharia" runat="server"
                                                    TextMode="MultiLine"
                                                    Rows="5"
                                                    Placeholder="
                                                    Descreva os problemas enumerando. Ex.:
                                                    1 - Pneu do truck lado direito dentro furado, próxima linha
                                                    2 - Lampada do freio lado direito queimada ..."
                                                    CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>

                                    </div>
                                    <br />
                                    <div class="card card-outline card-info collapsed-card">
                                        <div class="card-header">
                                            <h3 class="card-title"><i class="far fa-edit"></i>&nbsp;Funilaria/Carroceria/Sider/Total Sider</h3>
                                            <div class="card-tools">
                                                <button type="button" class="btn btn-tool" data-card-widget="collapse">
                                                    <i class="fas fa-plus"></i>
                                                </button>
                                            </div>

                                        </div>

                                        <div class="card-body">
                                            <div class="form-group">
                                                <label>Descrição do Problema:</label>
                                                <asp:TextBox ID="txtParteFunilaria" runat="server"
                                                    TextMode="MultiLine"
                                                    Rows="5"
                                                    Placeholder="Descreva os problemas enumerando. Ex.: 
                                                                     1 - Rasgo lado direito próximo a letra N no sider, próxima linha 
                                                                     2 - Roldana lado direito escapando ..."
                                                    CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>

                                    </div>

                                    <br />
                                    <div class="row">
                                        <div class="col-md-2">
                                            <asp:Button ID="btnSalvar"
                                                runat="server"
                                                Text="Salvar OS"
                                                CssClass="btn btn-outline-success w-100"
                                                OnClick="btnSalvar_Click" />
                                        </div>
                                        <div class="col-md-2">                                            
                                            <a href="ListaOS.aspx" class="btn btn-outline-danger w-100">Fechar
                                            </a>
                                        </div>
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
