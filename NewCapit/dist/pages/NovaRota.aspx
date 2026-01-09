<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="NovaRota.aspx.cs" Inherits="NewCapit.dist.pages.NovaRota" %>

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
    <script>
    <%--function calcularDistanciaBanco() {

        var ufOrigem = $('#<%= ddlUfOrigem.ClientID %>').val();
        var cidadeOrigem = $('#<%= ddlCidadeOrigem.ClientID %>').val();
        var ufDestino = $('#<%= ddlUfDestino.ClientID %>').val();
        var cidadeDestino = $('#<%= ddlCidadeDestino.ClientID %>').val();

        if (!cidadeOrigem || !cidadeDestino) {
            alert("Selecione origem e destino.");
           // MostrarMsg("Selecione origem e destino.", "info");
            return;
        }

        PageMethods.BuscarDistancia(
            ufOrigem, cidadeOrigem, ufDestino, cidadeDestino,
            function (ret) {

                if (!ret.encontrado) {
                    alert("Distância não cadastrada para este trajeto.");
                    //MostrarMsg("Selecione origem e destino.", "info");
                    return;
                }

                $('#lblDistancia').text(ret.distancia + " km");
                $('#lblTempo').text(ret.tempo + " min");
            }
        );
    }--%>
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
       
        <!-- Main content -->
        <%--<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />--%>
    <div class="card card-info">
    <div class="card-header">
         <h3 class="card-title"><i class="fas fa-map-marker-alt"></i>&nbsp;ROTAS - NOVO CADASTRO</h3>
    </div>
    </div>  
        <br /><br /><br /><br />
    <div class="container mt-4">  
       <div id="divMsg" runat="server"
             class="alert alert-info alert-dismissible fade show mt-3"
             role="alert" style="display: none;">
             <span id="lblMsgGeral" runat="server"></span>
             <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
       </div>
       <div class="card shadow">
        <div class="card-header bg-primary text-white">
            <h5 class="mb-0">Cadastro de Rota</h5>
        </div>

        <div class="card-body">

            <div class="row">
                <div class="col-md-6">
                    <h6>Origem</h6>

                    <label class="form-label">UF</label>
                    <asp:DropDownList ID="ddlUfOrigem" runat="server"
                        CssClass="form-select select2"
                        AutoPostBack="true"
                        OnSelectedIndexChanged="ddlUfOrigem_SelectedIndexChanged" />

                    <label class="form-label mt-2">Cidade</label>
                    <asp:DropDownList ID="ddlCidadeOrigem" runat="server"
                        CssClass="form-select select2" OnSelectedIndexChanged="AtualizarDeslocamento" />
                </div>

                <div class="col-md-6">
                    <h6>Destino</h6>

                    <label class="form-label">UF</label>
                    <asp:DropDownList ID="ddlUfDestino" runat="server"
                        CssClass="form-select select2"
                        AutoPostBack="true"
                        OnSelectedIndexChanged="ddlUfDestino_SelectedIndexChanged" />

                    <label class="form-label mt-2">Cidade</label>
                    <asp:DropDownList ID="ddlCidadeDestino" runat="server"
                        CssClass="form-select select2" OnSelectedIndexChanged="AtualizarDeslocamento"  />
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <label class="form-label mt-2">PEDAGIADA</label>
                    <asp:DropDownList ID="ddlPedagio" runat="server"
                        CssClass="form-select select2">
                        <asp:ListItem Text="SIM" Value="SIM"></asp:ListItem>
                        <asp:ListItem Text="NÃO" Value="NÃO"></asp:ListItem>
                        </asp:DropDownList>
                </div>
                <div class="col-md-6">
                 <label class="form-label mt-2">DESLOCAMENTO</label><br />
                    <asp:Label ID="lblDeslocamento" runat="server" Text=""></asp:Label>
                </div>
               </div> <br />
            <div class="row">
              <div class="col-md-6">
                   <asp:Button 
                     ID="btnCalcular" 
                     runat="server" 
                     Text="Calcular Distância"
                     CssClass="btn btn-success px-4"
                     OnClick="btnCalcular_Click" />
              </div>
                <div class="col-md-6">
                    <asp:Button ID="btnCadastrarRota" runat="server" Text="Cadastrar" CssClass="btn btn-warning px-4" OnClick="btnCadastrarRota_Click" />
                </div>
               
            </div>

            <div class="row mt-4 text-center">
                <div class="col-md-6">
                    <div class="alert alert-info">
                        <strong>Distância</strong><br />
                        <span id="lblDistancia" runat="server">-</span>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="alert alert-warning">
                        <strong>Tempo</strong><br />
                        <span id="lblTempo" runat="server">-</span>
                    </div>
                </div>
            </div>

        </div>
    </div>
    </div>

        <div class="modal fade" id="meuModal" tabindex="-1" role="dialog" aria-labelledby="meuModalLabel" aria-hidden="true">
    <div class="modal-dialog modal-dialog-centered modal-xl" role="document">
        <div class="modal-content">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
 <div class="row">
     <div class="col-md-12">
         <div class="card shadow"  runat="server">
             <div class="card-header bg-primary text-white">
                 <h5 class="mb-0">Cadastrar Nova Rota</h5>
                     </div>
             <!-- /.card-header -->
             <div class="card-body">
                 <div class="row mb-3">
                     <div class="col-md-5">
                         <label>Origem:</label>
                         <asp:TextBox ID="txtOrigem" runat="server" placeholder="Ex: São Paulo - SP" CssClass="form-control"  ></asp:TextBox>
                     </div>
                     <div class="col-md-5">
                         <label>Destino:</label>
                         <asp:TextBox ID="txtDestino" runat="server" placeholder="Ex: São Paulo - SP" CssClass="form-control"  ></asp:TextBox>
                     </div>
                     <div class="col-md-1">
                         <label>&nbsp;</label>
                         <asp:Button ID="btnDistancia" runat="server" Text="Calcular Distância" CssClass="btn btn-primary" OnClick="btnDistancia_Click" />

                     </div>

                 </div>
                 <div class="row mb-3">
                     <div class="col-md-2">
                         <label>Distância (Km):</label>
                         <asp:TextBox ID="txtDistancia" runat="server" CssClass="form-control" ></asp:TextBox>
                     </div>
                     <div class="col-md-2">
                        <label>Tempo (Min):</label>
                        <asp:TextBox ID="txtTempo" runat="server" CssClass="form-control" ></asp:TextBox>
                    </div>
                   <div class="col-md-2">
                       <label>Pedagiada</label>
                    <asp:DropDownList ID="ddlPedagioNovo" runat="server"
                        CssClass="form-select select2">
                        <asp:ListItem Text="SIM" Value="SIM"></asp:ListItem>
                        <asp:ListItem Text="NÃO" Value="NÃO"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <label>Deslocamento</label>
                        <asp:Label ID="lblDeslocamentoNovo" runat="server" Text=""></asp:Label>
                    </div>

                 </div>
                 <div class="row mb-3">
                     <div class="col-md-2">
                         <asp:Button ID="btnCadastrar" runat="server" Text="Cadastrar" CssClass="btn btn-success" OnClick="btnCadastrar_Click"/>
                     </div>
                      <div class="col-md-2">
                        <asp:Button 
                                ID="btnFechar" 
                                runat="server" 
                                Text="Fechar"
                                CssClass="btn btn-danger"
                                 />
                         </div>
                 </div>
                
             </div>
             <!-- /.card-body -->
         </div>
         <!-- /.card -->
     </div>
 </div>
 <asp:HiddenField ID="hfCollapse" runat="server" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnDistancia" />
                    <asp:PostBackTrigger ControlID="btnFechar" />
                    <asp:PostBackTrigger ControlID="btnCadastrar" />
                </Triggers>
            </asp:UpdatePanel>


        </div>
    </div>
</div>
       
    </div>

</asp:Content>
