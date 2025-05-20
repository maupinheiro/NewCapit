<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="Frm_GerarTXT.aspx.cs" Inherits="NewCapit.dist.pages.Frm_GerarTXT" %>

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
                        <h3 class="card-title"><i class="fas fa-clipboard-list"></i>&nbsp;JORNADA - GERAR ARQUIVO TXT PARA DEPARTAMENTO PESSOAL</h3>
                    </div>
                </div>
                <div class="card-header">
                    <div class="row g-3">
                        <div class="col-md-2">
                            <div class="form-group">
                                <span class="details">DATA INICIAL:</span>
                                <asp:TextBox ID="txtDataInicila" TextMode="Date" runat="server" Style="text-align: center" CssClass="form-control font-weight-bold" MaxLength="10"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form-group">
                                <span class="details">DATA FINAL:</span>
                                <asp:TextBox ID="txtDataFinal" TextMode="Date" runat="server" Style="text-align: center" CssClass="form-control font-weight-bold" MaxLength="10"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="row g-3">
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="details">MOTORISTA:</span>
                                <asp:TextBox ID="txtCodMot" runat="server" CssClass="form-control font-weight-bold" MaxLength="11"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <span class="details">NOME COMPLETO:</span>
                                <asp:TextBox ID="txtNomMot" runat="server" CssClass="form-control font-weight-bold"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <br />
                            <asp:Button ID="btnPesquisarMotorista" runat="server" Text="Pesquisar" CssClass="btn btn-outline-warning" />
                        </div>
                    </div>
                    <br />
                    <div class="card card-success">
                        <div class="card-header">
                            <h3 class="card-title">Filiais:</h3>
                        </div>
                        <div class="card-body">
                            <!-- Matriz -->
                            <div class="row">
                                <div class="col-sm-6">
                                    <!-- checkbox -->
                                    <div class="form-group clearfix">
                                        <div class="icheck-primary d-inline">
                                            <input type="checkbox" id="checkboxPrimary1">
                                            <label for="checkboxPrimary1">
                                                TNG MATRIZ
                                            </label>
                                        </div>

                                    </div>
                                </div>
                            </div>
                            <!-- Diadema -->
                            <div class="row">
                                <div class="col-sm-6">
                                    <!-- checkbox -->
                                    <div class="form-group clearfix">
                                        <div class="icheck-danger d-inline">
                                            <input type="checkbox" id="checkboxDanger1">
                                            <label for="checkboxDanger1">
                                                TNG DIADEMA
                                            </label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <!-- Ipiranga -->
                            <div class="row">
                                <div class="col-sm-6">
                                    <!-- checkbox -->
                                    <div class="form-group clearfix">
                                        <div class="icheck-success d-inline">
                                            <input type="checkbox" id="checkboxSuccess1">
                                            <label for="checkboxSuccess1">
                                                TNG IPIRANGA
                                            </label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <!-- Minas -->
                            <div class="row">
                                <div class="col-sm-6">
                                    <!-- checkbox -->
                                    <div class="form-group clearfix">
                                        <div class="icheck-warning d-inline">
                                            <input type="checkbox" id="checkboxSuccess2">
                                            <label for="checkboxSuccess2">
                                                TNG MINAS
                                            </label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- /.card-body -->
                    </div>
                    <div class="row g-3">
                        <div class="col-md-1">
                            <br />
                            <asp:Button ID="btnLimpar" runat="server" Text="Limpar Lista" CssClass="btn btn-outline-danger" />
                        </div>
                        <div class="col-md-1">
                            <br />
                            <asp:Button ID="btnGerar" runat="server" Text="Gerar TXT" CssClass="btn btn-outline-success" />
                        </div>
                        <div class="col-md-4">
                                <br />
                                <asp:DropDownList ID="ddlEscolherArquivo" placeholder="Selecione um arquivo..." runat="server" CssClass="form-control"></asp:DropDownList>
                            
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
