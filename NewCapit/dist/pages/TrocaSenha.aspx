<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="TrocaSenha.aspx.cs" Inherits="NewCapit.dist.pages.TrocaSenha" %>
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
    </style>
     <script src="https://code.jquery.com/jquery-3.6.4.min.js"></script>
 <script src="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/js/bootstrap.bundle.min.js"></script>

    <!-- Google Maps -->
    <script src="https://maps.googleapis.com/maps/api/js?key=AIzaSyApI6da0E4OJktNZ-zZHgL6A5jtk0L6Cww"></script>

    <script>
       function initSelect2() {
           $('.select2').select2({
               theme: 'bootstrap-5',
               width: '100%',
               placeholder: 'Selecione...',
               allowClear: true
           });
       }

       Sys.WebForms.PageRequestManager.getInstance()
           .add_endRequest(initSelect2);

       $(document).ready(initSelect2);
    </script>   
   <script type="text/javascript">
       function abrirModal() {
           $('#meuModal').modal({
               backdrop: 'static',
               keyboard: false
           });
       }
   </script>
    <div class="content-wrapper">
        <div class="container mt-4">
    <div id="divMsgSenha" runat="server" class="alert alert-info alert-dismissible fade show mt-3" role="alert" style="display: none;">
        <span id="lblMsgSenha" runat="server"></span>
        <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
    </div>

    <div class="card shadow">
        <div class="card-header bg-primary text-white">
            <h5 class="mb-0"><i class="fas fa-lock"></i>&nbsp; Alterar Senha de Acesso</h5>
        </div>

        <div class="card-body">
            <div class="row">
                <div class="col-md-12 mb-3">
                    <label class="form-label">Senha Atual</label>
                    <asp:TextBox ID="txtSenhaAtual" runat="server" CssClass="form-control" TextMode="Password" placeholder="Digite sua senha atual"></asp:TextBox>
                </div>

                <div class="col-md-6 mb-3">
                    <label class="form-label">Nova Senha</label>
                    <asp:TextBox ID="txtNovaSenha" runat="server" CssClass="form-control" TextMode="Password" placeholder="Nova senha"></asp:TextBox>
                </div>

                <div class="col-md-6 mb-3">
                    <label class="form-label">Confirmar Nova Senha</label>
                    <asp:TextBox ID="txtConfirmarSenha" runat="server" CssClass="form-control" TextMode="Password" placeholder="Repita a nova senha"></asp:TextBox>
                </div>
            </div>

            <hr />

            <div class="row">
                <div class="col-md-6">
                    <asp:Button ID="btnSalvarSenha" runat="server" Text="Atualizar Senha" 
                        CssClass="btn btn-outline-success px-4 w-100" 
                         OnClick="btnSalvarSenha_Click" />
                </div>
                <div class="col-md-6">
                    <a href="Home.aspx" class="btn btn-outline-secondary px-4 w-100">Cancelar</a>
                </div>
            </div>
        </div>
    </div>
</div>
        </div>
</asp:Content>
