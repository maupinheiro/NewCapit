<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="Frm_GerarTXT.aspx.cs" Inherits="NewCapit.dist.pages.Frm_GerarTXT" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
     <script>
         function formatar(src, mask) {
             var i = src.value.length;
             var saida = mask.substring(0, 1);
             var texto = mask.substring(i)
             if (texto.substring(0, 1) != saida) {
                 src.value += texto.substring(0, 1);
             }
         }
     </script>
     <script language="javascript">

         function ConfirmMessage4() {
             var selectedvalue = confirm("Deseja excluir todos os arquivos?");
             if (selectedvalue) {
                 document.getElementById('<%=txtconformmessageValue4.ClientID %>').value = "Yes";
        } else {
            document.getElementById('<%=txtconformmessageValue4.ClientID %>').value = "No";
             }
         }

     </script>
    <script>
        function mostrarModalCarregando() {
            var modal = new bootstrap.Modal(document.getElementById('modalCarregando'));
            modal.show();
        }
    </script>
    <div class="content-wrapper">
        <!-- Main content -->
        <section class="content">

            <div class="container-fluid">
                <br />
                <div class="card card-info">
                    <div class="card-header">
                        <h3 class="card-title"><i class="fas fa-clipboard-list"></i>&nbsp;GERAR ARQUIVO TXT PARA DEPARTAMENTO PESSOAL</h3>
                    </div>
                </div>
                <div class="card-header">
                    <div class="row g-3">
                        <div class="col-md-2">
                            <div class="form-group">
                                <span class="details">DATA INICIAL:</span>
                                <asp:TextBox ID="txtDtInicial" OnKeyPress="formatar(this, '##/##/####')" runat="server" Style="text-align: center" CssClass="form-control font-weight-bold" MaxLength="10"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form-group">
                                <span class="details">DATA FINAL:</span>
                                <asp:TextBox ID="txtDtFinal" OnKeyPress="formatar(this, '##/##/####')" runat="server" Style="text-align: center" CssClass="form-control font-weight-bold" MaxLength="10"></asp:TextBox>
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
                            <asp:Button ID="btnPesquisarMotorista" runat="server" Text="Pesquisar" OnClick="btnPesquisarMotorista_Click" CssClass="btn btn-outline-warning" />
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
                                        
                                             <asp:CheckBox ID="chkCadiriri" runat="server" AutoPostBack="true" OnCheckedChanged="chkCadiriri_CheckedChanged"/>
                                            <label for="checkboxPrimary1">
                                                TNG MATRIZ
                                            </label>
                                        

                                    </div>
                                </div>
                            </div>
                            <!-- Diadema -->
                            <div class="row">
                                <div class="col-sm-6">
                                    <!-- checkbox -->
                                    <div class="form-group clearfix">
                                       
                                            <asp:CheckBox ID="chkDiadema"  runat="server" AutoPostBack="true" OnCheckedChanged="chkDiadema_CheckedChanged"/>
                                            <label for="checkboxDanger1">
                                                TNG DIADEMA
                                            </label>
                                       
                                    </div>
                                </div>
                            </div>
                            <!-- Ipiranga -->
                            <div class="row">
                                <div class="col-sm-6">
                                    <!-- checkbox -->
                                    <div class="form-group clearfix">
                                        
                                            <asp:CheckBox ID="chkIpiranda"  runat="server" AutoPostBack="true" OnCheckedChanged="chkIpiranda_CheckedChanged"/>
                                            <label for="checkboxSuccess1">
                                                TNG IPIRANGA
                                            </label>
                                        
                                    </div>
                                </div>
                            </div>
                            <!-- Minas -->
                            <div class="row">
                                <div class="col-sm-6">
                                    <!-- checkbox -->
                                    <div class="form-group clearfix">
                                     
                                             <asp:CheckBox ID="chkMinas" runat="server" AutoPostBack="true" OnCheckedChanged="chkMinas_CheckedChanged"/>
                                            <label for="checkboxSuccess2">
                                                TNG MINAS
                                            </label>
                                       
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- /.card-body -->
                    </div>
                    <div class="row g-3">
                        <div class="col-md-1">
                            <br />
                            <asp:HiddenField ID="txtconformmessageValue4" runat="server" />
                            <asp:Button ID="btnLimpar" runat="server" Text="Limpar Lista" CssClass="btn btn-outline-danger" OnClientClick="javascript:ConfirmMessage4();" OnClick="btnLimpar_Click" />
                        </div>
                        <div class="col-md-1">
                            <br />
                            <asp:Button ID="btnGerar" runat="server" Text="Gerar TXT" OnClick="btnGerar_Click" OnClientClick="mostrarModalCarregando();" CssClass="btn btn-outline-success" />
                        </div>
                        <div class="col-md-4">
                                <br />
                                <asp:DropDownList ID="ddlEscolherArquivo" placeholder="Selecione um arquivo..." OnSelectedIndexChanged="ddlEscolherArquivo_SelectedIndexChanged" AutoPostBack="true" runat="server" CssClass="form-control"></asp:DropDownList>
                            
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
    <div class="modal fade" id="modalCarregando" tabindex="-1" aria-hidden="true" data-bs-backdrop="static" data-bs-keyboard="false">
  <div class="modal-dialog modal-dialog-centered">
    <div class="modal-content text-center">
      <div class="modal-body">
        <div class="spinner-border text-primary" role="status">
          <span class="visually-hidden">Carregando...</span>
        </div>
        <p class="mt-3">Gerando arquivo, por favor aguarde...</p>
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
