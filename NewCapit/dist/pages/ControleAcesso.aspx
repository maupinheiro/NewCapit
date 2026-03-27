<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="ControleAcesso.aspx.cs" Inherits="NewCapit.dist.pages.ControleAcesso" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
    <script type="text/javascript">
        // Esta função garante que o modal abra sempre que a página carregar 
        // e o acesso não tiver sido liberado
        function abrirModalSenha() {
            var myModal = new bootstrap.Modal(document.getElementById('modalSenhaMaster'), {
                backdrop: 'static',
                keyboard: false
            });
            myModal.show();
        }

        function fecharOverlay() {
            // Usa jQuery ou JS puro para esconder a trava branca
            $("#loadingOverlay").fadeOut();
        }
</script>
    <style type="text/css">
        #loadingOverlay {
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background-color: white; /* Cor sólida para ninguém ver o gráfico atrás */
            z-index: 9999; /* Garante que fica na frente de tudo */
            display: block;
        }
    </style>

    <div class="content-wrapper">
        <section class="content">
            <div class="container-fluid pt-3">
                <div class="row">
                    <div class="col-md-12">
                        <div class="card card-success">
                            <div class="card-header"><h3 class="card-title">Acessos por Dia</h3></div>
                            <div class="card-body"><div style="height: 300px;"><canvas id="graficoDias"></canvas></div></div>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-6">
                        <div class="card card-primary">
                            <div class="card-header"><h3 class="card-title">Top 10 Usuários (Mais Ativos)</h3></div>
                            <div class="card-body"><div style="height: 300px;"><canvas id="graficoUsuarios"></canvas></div></div>
                        </div>
                    </div>

                    <div class="col-md-6">
                        <div class="card card-info">
                            <div class="card-header"><h3 class="card-title">Acessos por Empresa</h3></div>
                            <div class="card-body"><div style="height: 300px;"><canvas id="graficoEmpresas"></canvas></div></div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
       <div class="modal fade" id="modalSenhaMaster" 
     data-bs-backdrop="static" 
     data-bs-keyboard="false" 
     tabindex="-1" aria-hidden="true">
    <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content shadow-lg border-0">
            <div class="modal-header bg-primary text-white">
                <h5 class="modal-title"><i class="fas fa-lock mr-2"></i> Acesso Restrito</h5>
            </div>
            <div class="modal-body text-center">
                <p>Esta página contém dados sensíveis. Por favor, insira a <b>Senha Master</b> para visualizar os gráficos.</p>
                <div class="form-group mt-3">
                    <asp:TextBox ID="txtSenhaMaster" runat="server" TextMode="Password" Text="" IsRequired="true"
                        CssClass="form-control form-control-lg text-center" ></asp:TextBox>
                </div>
                <asp:Label ID="lblErroSenha" runat="server" CssClass="text-danger small" Text=""></asp:Label>
            </div>
            <div class="modal-footer d-flex justify-content-between">
                <asp:LinkButton ID="btnSair" runat="server" CssClass="btn btn-outline-secondary" OnClick="btnSair_Click">
                    <i class="fas fa-home"></i> Voltar para Home
                </asp:LinkButton>
                <asp:Button ID="btnConfirmarSenha" runat="server" CssClass="btn btn-primary px-4" 
                    Text="Acessar Dashboard" OnClick="btnConfirmarSenha_Click" />
            </div>
        </div>
    </div>
</div>

<div id="loadingOverlay" style="position: fixed; top: 0; left: 0; width: 100%; height: 100%; background: #fff; z-index: 1040;"></div>
    </div>
</asp:Content>
