<%@ Page Title="" Language="C#" MasterPageFile="Main.Master" AutoEventWireup="true" CodeBehind="Consulta_Agregados.aspx.cs" Inherits="NewCapit.dist.pages.Consulta_Agregados" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link rel="stylesheet" href="/css/styleTabela.css">
    <script language="javascript">
        function ConfirmMessage() {
            var selectedvalue = confirm("Exclusão de Dados\n Tem certeza de que deseja excluir a informação permanentemente?");
        if (selectedvalue) {
                document.getElementById('<%=txtconformmessageValue.ClientID %>').value = "Yes";

        } else {
            document.getElementById('<%=txtconformmessageValue.ClientID %>').value = "No";

        }
        
    </script>
    <style>
    .pagination-centered {
     text-align: center;
    }

    .pagination-centered table {
    margin: 0 auto; /* Isso centraliza a tabela da paginação */
    }

</style>
    <!-- Page Heading -->
    <div class="content-wrapper">
        <div class="content-header">
            <div class="d-sm-flex align-items-center justify-content-between mb-4">
                <h1 class="h3 mb-2 text-gray-800">
                    <i class="fas fa-users"></i>&nbsp;Consulta Agregados / Terceiros</h1>
                <a href="Frm_CadTransportadoras.aspx" class="d-none d-sm-inline-block btn btn-sm btn-primary shadow-sm"><i
                    class="fas fa-user-plus"></i>&nbsp;Novo Cadastro            
                </a>

            </div>
            <!-- Content Graficos -->
            <div class="row">
                <!-- Total de veiculos frota/agregados/terceiros -->
                <div class="col-md-3 col-sm-6 col-12">
                  <div class="info-box shadow-none">
                    <span class="info-box-icon bg-info"><i class="fas fa-users"></i></span>
                    <div class="info-box-content">
                      <span class="info-box-text"> Agregados/Terceiros</span>                      
                      <asp:Label id="TotalVeiculos"  class="info-box-number" runat="server">0</asp:Label>
                    </div>
                    <!-- /.info-box-content -->
                  </div>
                  <!-- /.info-box -->
                </div>

                <!-- agregados/terceiros em operação -->
                <div class="col-md-3 col-sm-6 col-12">
                  <div class="info-box shadow-none">
                    <span class="info-box-icon bg-info"><i class="fas fa-street-view"></i></span>
                    <div class="info-box-content">
                      <span class="info-box-text"> Em Operação</span>
                      <asp:Label id="Operacao" class="info-box-number" runat="server">0</asp:Label>
                      <div class="progress">
                        <div class="progress-bar bg-danger" style="width: 70%"></div>
                      </div>
                    </div>                    
                    <!-- /.info-box-content -->
                  </div>
                  <!-- /.info-box -->
                </div>
                <!-- Total de veiculos agregados -->
                <div class="col-md-3 col-sm-6 col-12">
                  <div class="info-box shadow-none">
                    <span class="info-box-icon bg-info"><i class="fas fa-user-friends"></i></span>
                    <div class="info-box-content">
                      <span class="info-box-text"> Agregados</span>
                      <asp:Label id="Agregados" class="info-box-number" runat="server">0</asp:Label>
                    </div>
                    <!-- /.info-box-content -->
                      <!-- sub menu total de condutores -->
                      <div class="col-auto">                            
                           <ul class="nav navbar-nav navbar-rigth">
                               <li class="">                            
                                   <class="user-profile dropdown-toggle" data-toggle="dropdown" aria-expanded="false">      
                                   <span class=" fa fa-angle-down"></span>  
                               </li>    
                               <li>
                               <span> Ativos: </span> 
                               <span class="text-xs font-weight-bold text-primary text-uppercase mb-1">
                               <asp:Label ID="veiculosAtivos" runat="server" Text=""></asp:Label></span>
                               </li>
                               <li><span> Inativos: </span>
                               <span class="text-xs font-weight-bold text-danger text-uppercase mb-1">
                               <asp:Label ID="veiculosInativos" runat="server" Text=""></asp:Label>
                               </span>
                               </li>
                           </ul>
                    </div>
                  </div>
                  <!-- /.info-box -->                  
                </div>
                <!-- Total de veiculos agregados -->
                <div class="col-md-3 col-sm-6 col-12">
                  <div class="info-box shadow-none">
                    <span class="info-box-icon bg-info"><i class="fas fa-user-friends"></i></span>
                    <div class="info-box-content">
                      <span class="info-box-text"> Terceiros</span>
                      <asp:Label id="Terceiros" class="info-box-number" runat="server">0</asp:Label>
                    </div>
                    <!-- /.info-box-content -->
                    <!-- sub menu total de condutores -->
                    <div class="col-auto">                            
                         <ul class="nav navbar-nav navbar-rigth">
                             <li class="">                            
                                 <class="user-profile dropdown-toggle" data-toggle="dropdown" aria-expanded="false">      
                                 <span class=" fa fa-angle-down"></span>   
                             </li>    
                             <li>
                                <span> Ativos: </span> 
                                <span class="text-xs font-weight-bold text-primary text-uppercase mb-1">
                                <asp:Label ID="lbAtivosTerceiros" runat="server" Text=""></asp:Label></span>
                                </li>
                                <li><span> Inativos: </span>
                                <span class="text-xs font-weight-bold text-danger text-uppercase mb-1">
                                <asp:Label ID="lbInativosTerceiros" runat="server" Text=""></asp:Label>
                                </span>
                             </li>
                         </ul>
                    </div>
                  </div>
                  <!-- /.info-box -->                  
                </div>                
            </div>
        </div>
        <!-- DataTales Example -->

         <div class="card shadow mb-4">
              <div class="card-body">
                
        <div class="card shadow mb-4">
              <div class="card-header">
                 <asp:TextBox ID="myInput" CssClass="" OnTextChanged="myInput_TextChanged" placeholder="Pesquisar ..." AutoPostBack="true" runat="server" Width="100%"></asp:TextBox>
              </div>
              <div class="card-body">
                 <div class="table-responsive">
                     <asp:GridView ID="gvListAgregados" CssClass="table table-bordered dataTable1 table-hover" Width="100%" AutoGenerateColumns="False" DataKeyNames="id" runat="server" AllowPaging="True" PageSize="10" OnPageIndexChanging="gvListAgregados_PageIndexChanging" ShowHeaderWhenEmpty="True">
                        <PagerStyle HorizontalAlign="Center" CssClass="pagination-centered" />
                     <Columns>
                        <asp:BoundField DataField="id" HeaderText="#ID" Visible="false" />
                        <asp:BoundField DataField="codtra" HeaderText="CÓDIGO" />
                        <asp:BoundField DataField="fantra" HeaderText="NOME FANTASIA" /> 
                        <asp:BoundField DataField="pessoa" HeaderText="PESSOA" />
                        <asp:BoundField DataField="cnpj" HeaderText="CPF/CNPJ" />
                        <asp:BoundField DataField="filial" HeaderText="FILIAL" />
                        <asp:BoundField DataField="fone2" HeaderText="CELULAR" />
                        <asp:BoundField DataField="ativa_inativa" HeaderText="SITUAÇÃO" />
                        <asp:TemplateField HeaderText="AÇÕES" ShowHeader="True" HeaderStyle-Width="180px">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkEditar" runat="server" OnClick="Editar"  CssClass="btn btn-primary btn-sm"><i class="fa fa-edit"></i>Editar</asp:LinkButton>
                                <%--<asp:LinkButton ID="lnkExcluir" runat="server" OnClick="Excluir" CssClass="btn btn-danger btn-sm" OnClientClick="javascript:ConfirmMessage();"><i class="fa fa-trash"></i></i></asp:LinkButton>--%>
                            </ItemTemplate>
                        </asp:TemplateField>
                     </Columns>
                    </asp:GridView>

                 </div>
                 <asp:HiddenField ID="txtconformmessageValue" runat="server" />
              </div>
         </div>


    </div>
             
         </div>
        </div>
    <!-- jQuery -->
    
    <script>
        $(function () {
            $("#example1").DataTable({
                "responsive": true, "lengthChange": false, "autoWidth": false,
                "buttons": ["copy", "csv", "excel", "pdf", "print", "colvis"]
            }).buttons().container().appendTo('#example1_wrapper .col-md-6:eq(0)');
            $('#example2').DataTable({
                "paging": true,
                "lengthChange": false,
                "searching": false,
                "ordering": true,
                "info": true,
                "autoWidth": false,
                "responsive": true,
            });
        });

</script>

   

</asp:Content>
