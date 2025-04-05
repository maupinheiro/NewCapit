<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="ImportarPlanejamento.aspx.cs" Inherits="NewCapit.dist.pages.ImportarPlanejamento" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
              <style>
    .scrollable-gridview {
        max-height: 400px;
        overflow-y: auto;
    }
</style>
            <div class="content-wrapper">
        <section class="content">
            <div class="container-fluid">
                <div class="content-header">
                    <div class="d-sm-flex align-items-center justify-content-between mb-4">
                        <h1 class="h3 mb-2 text-gray-800">
                            <i class="fas fa-boxes nav-icon"></i>&nbsp;Importar Planejamento
                        </h1>
                    </div>
                </div>
                <div class="row g-3">
                    <div class="col-md-8">
                        <div class="form-group">
                            <div class="input-group date"> 
                                <asp:FileUpload ID="FileUpload1" runat="server" CssClass="form-control" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-1">
                        
                        <asp:LinkButton ID="lnkCarregar" runat="server" CssClass="btn btn-info" OnClick="lnkCarregar_Click">
                            <i class='fas fa-search'></i> Carregar
                        </asp:LinkButton>   
                    </div>  
                </div>  
            </div>
        </section>
        
        <section class="content">
            <div class="container-fluid">
                <div class="row">
                    <div class="col-12">
                        <div class="card">
                            
                            <div class="card-body">
                                 <div class="scrollable-gridview">
                                <asp:GridView runat="server" ID="gvListCargas" CssClass="table table-bordered table-striped table-hover" Width="100%" AutoGenerateColumns="True">
                                </asp:GridView>
                                
                                     </div>
                                <br />
                                <asp:Label ID="lblMensagem" runat="server" Text=""></asp:Label>
                                <br />
                                <asp:Button ID="btnSalvar" runat="server" CssClass="btn btn-success" Text="Gravar Informações" OnClick="btnSalvar_Click" />
                                     
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>
        <!-- /.content-wrapper -->
        <footer class="main-footer">
            <div class="float-right d-none d-sm-block">
                <b>Version</b> 3.1.0
 
            </div>
            <strong>Copyright &copy; 2023-2025 <a href="#">Capit Logística</a>.</strong> Todos os direitos reservados.
        </footer>
   

    <!-- JavaScript -->

</asp:Content>
