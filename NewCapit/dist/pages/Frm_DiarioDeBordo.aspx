<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="Frm_DiarioDeBordo.aspx.cs" Inherits="NewCapit.dist.pages.Frm_DiarioDeBordo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>

    <div class="content-wrapper">
        <!-- Main content -->
        <section class="content">

            <div class="container-fluid">
                <br />
                <div class="card card-info">
                    <div class="card-header">
                        <h3 class="card-title"><i class="fas fa-clipboard-list"></i>&nbsp;JORNADA - DIARIO DE BORDO</h3>
                    </div>
                </div>
                <div class="card-header">
                    <div class="row g-3">
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="details">MOTORISTA:</span>
                                <asp:TextBox ID="txtColeta" runat="server" Style="text-align: center" CssClass="form-control font-weight-bold" MaxLength="11"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form-group">
                                <span class="details">DATA:</span>
                                <asp:TextBox ID="txtDia" TextMode="Date" runat="server" Style="text-align: center" CssClass="form-control font-weight-bold" MaxLength="10"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <br />
                            <asp:Button ID="btnPesquisarMotorista" runat="server" Text="Pesquisar" CssClass="btn btn-outline-warning" />
                        </div>
                    </div>
                    <div class="row g-3">
                        <div class="col-md-6">
                            <div class="form-group">
                                <span class="details">NOME COMPLETO:</span>
                                <asp:TextBox ID="txtNome" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="form-group">
                                <span class="details">FUNÇÃO:</span>
                                <asp:TextBox ID="txtFuncao" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="form-group">
                                <span class="details">NÚCLEO:</span>
                                <asp:TextBox ID="txtNucleo" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>

                    </div>
                    <div class="row g-3">
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="details">CAFÉ:</span>
                                <asp:TextBox ID="txtCafe" runat="server" CssClass="form-control" MaxLength="6"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="details">ALMOÇO:</span>
                                <asp:TextBox ID="txtAlmoco" runat="server" CssClass="form-control" MaxLength="6"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="details">JANTA:</span>
                                <asp:TextBox ID="txtJanta" runat="server" CssClass="form-control" MaxLength="6"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="details">PERNOITE:</span>
                                <asp:TextBox ID="txtPernoite" runat="server" CssClass="form-control" MaxLength="6"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="details">ENG./DES:</span>
                                <asp:TextBox ID="txtEngate" runat="server" CssClass="form-control" MaxLength="6"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="details">COMISSÃO:</span>
                                <asp:TextBox ID="txtComissao" runat="server" CssClass="form-control" MaxLength="6"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="details">RELATÓRIOS:</span>
                                <asp:TextBox ID="txtRel1" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="details">[2]:</span>
                                <asp:TextBox ID="txtRel2" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="details">[3]:</span>
                                <asp:TextBox ID="txtRel3" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="details">[4]:</span>
                                <asp:TextBox ID="txtRel4" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <br />
                            <asp:Button ID="btnInserirValores" runat="server" Text="Inserir" CssClass="btn btn-outline-info" />
                        </div>
                    </div>
                    <div class="row g-3">
                        <div class="col-md-3">
                            <div class="form-group">
                                <span class="details">MACRO:</span>
                                <asp:DropDownList ID="ddlMacro" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="" Text="Selecione..."></asp:ListItem>
                                    <asp:ListItem Value="INICIO DE JORNADA" Text="INICIO DE JORNADA"></asp:ListItem>
                                    <asp:ListItem Value="INICIO JORNADA" Text="INICIO JORNADA"></asp:ListItem>
                                    <asp:ListItem Value="INICIO DE VIAGEM" Text="INICIO DE VIAGEM"></asp:ListItem>
                                    <asp:ListItem Value="REINICIO DE VIAGEM" Text="REINICIO DE VIAGEM"></asp:ListItem>
                                    <asp:ListItem Value="PARADA INTERNA" Text="PARADA INTERNA"></asp:ListItem>
                                    <asp:ListItem Value="PARADA REFEICAO" Text="PARADA REFEICAO"></asp:ListItem>
                                    <asp:ListItem Value="RETORNO REFEICAO" Text="RETORNO REFEICAO"></asp:ListItem>
                                    <asp:ListItem Value="PARADA CLIENTE/FORNECEDOR" Text="PARADA CLIENTE/FORNECEDOR"></asp:ListItem>
                                    <asp:ListItem Value="PARADA" Text="PARADA"></asp:ListItem>
                                    <asp:ListItem Value="PARADA OFICINA" Text="PARADA OFICINA"></asp:ListItem>
                                    <asp:ListItem Value="FIM DE VIAGEM" Text="FIM DE VIAGEM"></asp:ListItem>
                                    <asp:ListItem Value="FIM DE JORNADA" Text="FIM DE JORNADA"></asp:ListItem>
                                    <asp:ListItem Value="PARADA PERNOITE" Text="PARADA PERNOITE"></asp:ListItem>
                                </asp:DropDownList><br />
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="details">HORA:</span>
                                <asp:TextBox ID="txtHora" TextMode="Time" Style="text-align: center" runat="server" CssClass="form-control" MaxLength="5"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="">PARADA:</span>
                                <asp:DropDownList ID="ddlParada" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="" Text="Selecione..."></asp:ListItem>
                                    <asp:ListItem Value="1" Text="1"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                    <asp:ListItem Value="3" Text="3"></asp:ListItem>
                                    <asp:ListItem Value="4" Text="4"></asp:ListItem>
                                    <asp:ListItem Value="6" Text="6"></asp:ListItem>
                                    <asp:ListItem Value="7" Text="7"></asp:ListItem>
                                    <asp:ListItem Value="9" Text="9"></asp:ListItem>
                                    <asp:ListItem Value="11" Text="11"></asp:ListItem>
                                    <asp:ListItem Value="13" Text="13"></asp:ListItem>
                                    <asp:ListItem Value="14" Text="14"></asp:ListItem>
                                    <asp:ListItem Value="15" Text="15"></asp:ListItem>
                                    <asp:ListItem Value="16" Text="16"></asp:ListItem>
                                    <asp:ListItem Value="17" Text="17"></asp:ListItem>
                                    <asp:ListItem Value="22" Text="22"></asp:ListItem>
                                    <asp:ListItem Value="23" Text="23"></asp:ListItem>
                                </asp:DropDownList></>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="form-group">
                                <span class="details">DESCRIÇÃO:</span>
                                <asp:TextBox ID="txtDescricao" runat="server" CssClass="form-control font-weight-bold"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <br />
                            <asp:Button ID="btnInserirMarcacao" runat="server" Text="Inserir" CssClass="btn btn-outline-info" />
                        </div>
                    </div>

                </div>
            </div>
        </section>
        <!-- Mensagens de erro toast -->
        <div class="toast-container position-fixed top-0 end-0 p-3">
            <div id="toastNotFound" class="toast align-items-center text-bg-danger border-0" role="alert" aria-live="assertive" aria-atomic="true">
                <div class="d-flex">
                    <div class="toast-body">
                        Motorista não encontrado. Verifique o código digitado.
                    </div>
                    <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast" aria-label="Close"></button>
                </div>
            </div>
        </div>
    </div>
    <!-- /.content-wrapper -->
    <footer class="main-footer">
        <div class="float-right d-none d-sm-block">
            <b>Version</b> 3.1.0 
        </div>
        <strong>Copyright &copy; 2023-2025 <a href="#">Capit Logística</a>.</strong> Todos os direitos reservados.
    </footer>

    <script>
        function mostrarToastNaoEncontrado() {
            var toastEl = document.getElementById('toastNotFound');
            var toast = new bootstrap.Toast(toastEl);
            toast.show();
        }
    </script>
</asp:Content>
