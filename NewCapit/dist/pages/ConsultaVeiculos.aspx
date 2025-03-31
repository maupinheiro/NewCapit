<%@ Page Title="" Language="C#" MasterPageFile="Main.Master" AutoEventWireup="true" CodeBehind="ConsultaVeiculos.aspx.cs" Inherits="NewCapit.ConsultaVeiculos" %>

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
 <div class="content-wrapper">
     <div class="content-header">
         <div class="d-sm-flex align-items-center justify-content-between mb-4">
            <h1 class="h3 mb-2 text-gray-800">
                <i class="fas fa-shipping-fast"></i> &nbsp;Consulta Veículos</h1>
            <a href="/dist/pages/Frm_CadVeiculos.aspx" class="d-none d-sm-inline-block btn btn-sm btn-primary shadow-sm">
                <i class="fas fa-shipping-fast"></i> &nbsp;Novo Cadastro
            </a>
        </div> 
         <!-- Content Row -->
         <div class="row">
              <!-- Total Veículos -->        
              <div class="col-xl-3 col-md-6 mb-4">
                    <div class="card border-left-primary shadow h-100 py-2">
                        <div class="card-body">
                            <div class="row no-gutters align-items-center">
                                <div class="col mr-2">
                                    <div class="text-xs font-weight-bold text-primary text-uppercase mb-1">
                                        Total de Veículos
                                    </div>
                                    <div class="h5 mb-0 font-weight-bold text-gray-800">
                                        <asp:Label ID="TotalVeiculos" runat="server" Text=""></asp:Label>
                                    </div>
                                </div>
                                <!-- sub menu total de veiculos -->
                                <div class="col-auto">                            
                                     <ul class="nav navbar-nav navbar-right">
                                         <li>
                                             <span>ATIVOS: </span> 
                                             <span class="text-xs font-weight-bold text-primary text-uppercase mb-1">
                                             <asp:Label ID="LbAtivos" runat="server" Text=""></asp:Label></span>
                                         </li>
                                         <li><span>INATIVOS: </span>
                                             <span class="text-xs font-weight-bold text-danger text-uppercase mb-1">
                                             <asp:Label ID="LbInativos" runat="server" Text=""></asp:Label>
                                             </span>
                                         </li>
                                     </ul>
                                </div>
                                <!-- fim -->
                            </div>
                        </div>
                    </div>                  
              </div>
              <!-- Frota -->
              <div class="col-xl-3 col-md-6 mb-4">
                 <div class="card border-left-success shadow h-100 py-2">
                    <div class="card-body">
                    <div class="row no-gutters align-items-center">
                        <div class="col mr-2">
                            <div class="text-xs font-weight-bold text-success text-uppercase mb-1">
                                Frota Ativa
                            </div>
                            <div class="h5 mb-0 font-weight-bold text-gray-800">
                                <asp:Label ID="TotalFrota" runat="server" Text=""></asp:Label>
                            </div>                           
                        </div>
                        <!-- Sub menu frota -->
                        <div class="col-auto">                            
                            <ul class="nav navbar-nav navbar-right">
                                <li class="">                            
                                    <class="user-profile dropdown-toggle" data-toggle="dropdown" aria-expanded="false">
                                    <i class="fas fa-truck fa-2x text-gray-300"></i>
                                    <span class=" fa fa-angle-down"></span>
                                  </>                               
                                  <ul class="dropdown-menu dropdown-usermenu pull-right">
                                      <li>
                                          <span>CNT: </span> 
                                          <span class="text-xs font-weight-bold text-success text-uppercase mb-1">
                                            <asp:Label ID="FrotaCNT" runat="server" Text=""></asp:Label>
                                          </span>
                                      </li>
                                      <li>
                                          <span>SBC: </span> 
                                          <span class="text-xs font-weight-bold text-danger text-uppercase mb-1">
                                            <asp:Label ID="FrotaSBC" runat="server" Text=""></asp:Label>
                                          </span>
                                      </li>
                                      <li><span>MATRIZ: </span>
                                            <span class="text-xs font-weight-bold text-info text-uppercase mb-1">
                                                <asp:Label ID="FrotaMATRIZ" runat="server" Text=""></asp:Label>
                                            </span>
                                      </li>
                                      <li><span>TAUBATÉ: </span>
                                            <span class="text-xs font-weight-bold text-warning text-uppercase mb-1">
                                                <asp:Label ID="FrotaTaubate" runat="server" Text=""></asp:Label>
                                            </span>
                                      </li>
                                      <li><span>IPIRANGA: </span>
                                         <span class="text-xs font-weight-bold text-danger text-uppercase mb-1">
                                            <asp:Label ID="FrotaIpiranga" runat="server" Text=""></asp:Label>
                                         </span>
                                      </li>
                                      <li><span>SÃO CARLOS: </span>
                                             <span class="text-xs font-weight-bold text-success text-uppercase mb-1">
                                                <asp:Label ID="FrotaSC" runat="server" Text=""></asp:Label>
                                             </span>
                                       </li>
                                      <li><span>PR - PARANÁ: </span>
                                             <span class="text-xs font-weight-bold text-dark text-uppercase mb-1">
                                                <asp:Label ID="FrotaPR" runat="server" Text=""></asp:Label>
                                             </span>
                                      </li>
                                      <li><span>PE - PERNAMBUCO: </span>
                                        <span class="text-xs font-weight-bold text-secondary text-uppercase mb-1">
                                            <asp:Label ID="FrotaPE" runat="server" Text=""></asp:Label>
                                        </span>
                                      </li>
                                      <li><span>MG - MINAS GERAIS: </span>
                                          <span class="text-xs font-weight-bold text-danger text-uppercase mb-1">
                                              <asp:Label ID="FrotaMINAS" runat="server" Text=""></asp:Label>
                                          </span>
                                      </li>
                                  </ul>
                                </li>
                            </ul>
                        </div>
                        <!-- Fim do sub menu frota -->
                    </div>
                </div>
                 </div>
              </div>
              <!-- Agregados  -->
              <div class="col-xl-3 col-md-6 mb-4">
                 <div class="card border-left-warning shadow h-100 py-2">
                 <div class="card-body">
            <div class="row no-gutters align-items-center">
                <div class="col mr-2">
                    <div class="text-xs font-weight-bold text-warning text-uppercase mb-1">
                        Agregados Ativos
                    </div>
                    <div class="h5 mb-0 font-weight-bold text-gray-800">
                        <asp:Label ID="TotalAgregados" runat="server" Text=""></asp:Label>
                    </div>                           
                </div>
                <!-- Sub menu agregados -->
                <div class="col-auto">                            
                    <ul class="nav navbar-nav navbar-right">
                        <li class="">                            
                            <class="user-profile dropdown-toggle" data-toggle="dropdown" aria-expanded="false">
                            <i class="fas fa-truck fa-2x text-gray-300"></i>
                            <span class=" fa fa-angle-down"></span>
                          </>                               
                          <ul class="dropdown-menu dropdown-usermenu pull-right">
                              <li>
                                  <span>CNT: </span> 
                                  <span class="text-xs font-weight-bold text-success text-uppercase mb-1">
                                    <asp:Label ID="AgCNT" runat="server" Text=""></asp:Label>
                                  </span>
                              </li>
                              <li>
                                  <span>SBC: </span> 
                                  <span class="text-xs font-weight-bold text-danger text-uppercase mb-1">
                                    <asp:Label ID="AgSBC" runat="server" Text=""></asp:Label>
                                  </span>
                              </li>
                              <li><span>MATRIZ: </span>
                                    <span class="text-xs font-weight-bold text-info text-uppercase mb-1">
                                        <asp:Label ID="AgMatriz" runat="server" Text=""></asp:Label>
                                    </span>
                              </li>
                              <li><span>TAUBATÉ: </span>
                                    <span class="text-xs font-weight-bold text-warning text-uppercase mb-1">
                                        <asp:Label ID="AgTaubate" runat="server" Text=""></asp:Label>
                                    </span>
                              </li>
                              <li><span>IPIRANGA: </span>
                                 <span class="text-xs font-weight-bold text-danger text-uppercase mb-1">
                                    <asp:Label ID="AgIpiranga" runat="server" Text=""></asp:Label>
                                 </span>
                              </li>
                              <li><span>SÃO CARLOS: </span>
                                     <span class="text-xs font-weight-bold text-success text-uppercase mb-1">
                                        <asp:Label ID="AgSC" runat="server" Text=""></asp:Label>
                                     </span>
                               </li>
                              <li><span>PR - PARANÁ: </span>
                                     <span class="text-xs font-weight-bold text-dark text-uppercase mb-1">
                                        <asp:Label ID="AgPR" runat="server" Text=""></asp:Label>
                                     </span>
                              </li>
                              <li><span>PE - PERNAMBUCO: </span>
                                <span class="text-xs font-weight-bold text-secondary text-uppercase mb-1">
                                    <asp:Label ID="AgPE" runat="server" Text=""></asp:Label>
                                </span>
                              </li>
                              <li><span>MG - MINAS GERAIS: </span>
                                  <span class="text-xs font-weight-bold text-danger text-uppercase mb-1">
                                      <asp:Label ID="AgMG" runat="server" Text=""></asp:Label>
                                  </span>
                              </li>
                          </ul>
                        </li>
                    </ul>
                </div>
                <!-- Fim do sub menu frota -->
            </div>
        </div>
            </div>
              </div>       
              <!-- Terceiros  -->
              <div class="col-xl-3 col-md-6 mb-4">
                 <div class="card border-left-danger shadow h-100 py-2">
                 <div class="card-body">
            <div class="row no-gutters align-items-center">
                <div class="col mr-2">
                    <div class="text-xs font-weight-bold text-danger text-uppercase mb-1">
                        Terceiros Ativos
                    </div>
                    <div class="h5 mb-0 font-weight-bold text-gray-800">
                        <asp:Label ID="TotalTerceiros" runat="server" Text=""></asp:Label>
                    </div>                           
                </div>
                <!-- Sub menu terceiros -->
                <div class="col-auto">                            
                    <ul class="nav navbar-nav navbar-right">
                        <li class="">                            
                            <class="user-profile dropdown-toggle" data-toggle="dropdown" aria-expanded="false">
                            <i class="fas fa-truck fa-2x text-gray-300"></i>
                            <span class=" fa fa-angle-down"></span>
                          </>                               
                          <ul class="dropdown-menu dropdown-usermenu pull-right">
                              <li>
                                  <span>CNT: </span> 
                                  <span class="text-xs font-weight-bold text-success text-uppercase mb-1">
                                    <asp:Label ID="lbTerCNT" runat="server" Text=""></asp:Label>
                                  </span>
                              </li>
                              <li>
                                  <span>SBC: </span> 
                                  <span class="text-xs font-weight-bold text-danger text-uppercase mb-1">
                                    <asp:Label ID="lbTerSBC" runat="server" Text=""></asp:Label>
                                  </span>
                              </li>
                              <li><span>MATRIZ: </span>
                                    <span class="text-xs font-weight-bold text-info text-uppercase mb-1">
                                        <asp:Label ID="lbTerMatriz" runat="server" Text=""></asp:Label>
                                    </span>
                              </li>
                              <li><span>TAUBATÉ: </span>
                                    <span class="text-xs font-weight-bold text-warning text-uppercase mb-1">
                                        <asp:Label ID="lbTerTaubate" runat="server" Text=""></asp:Label>
                                    </span>
                              </li>
                              <li><span>IPIRANGA: </span>
                                 <span class="text-xs font-weight-bold text-danger text-uppercase mb-1">
                                    <asp:Label ID="lbTerIpiranga" runat="server" Text=""></asp:Label>
                                 </span>
                              </li>
                              <li><span>SÃO CARLOS: </span>
                                     <span class="text-xs font-weight-bold text-success text-uppercase mb-1">
                                        <asp:Label ID="lbTerSCarlos" runat="server" Text=""></asp:Label>
                                     </span>
                               </li>
                              <li><span>PR - PARANÁ: </span>
                                     <span class="text-xs font-weight-bold text-dark text-uppercase mb-1">
                                        <asp:Label ID="lbTerPR" runat="server" Text=""></asp:Label>
                                     </span>
                              </li>
                              <li><span>PE - PERNAMBUCO: </span>
                                <span class="text-xs font-weight-bold text-secondary text-uppercase mb-1">
                                    <asp:Label ID="lbTerPE" runat="server" Text=""></asp:Label>
                                </span>
                              </li>
                              <li><span>MG - MINAS GERAIS: </span>
                                  <span class="text-xs font-weight-bold text-danger text-uppercase mb-1">
                                      <asp:Label ID="lbTerMG" runat="server" Text=""></asp:Label>
                                  </span>
                              </li>
                          </ul>
                        </li>
                    </ul>
                </div>
                <!-- Fim do sub menu frota -->
            </div>
        </div>
           </div>
              </div>        
        </div>
     </div>
     <!-- Grid -->
     <div class="card shadow mb-4"> 
        <div class="card-header">
           <asp:TextBox ID="myInput" CssClass="" OnTextChanged="myInput_TextChanged" placeholder="Pesquisar ..." AutoPostBack="true" runat="server" Width="100%"></asp:TextBox>
        </div>
        <div class="card-body">
            <div class="table-responsive">              
                <asp:GridView runat="server" ID="gvVeiculos" CssClass="table table-bordered dataTable1 table-hover" Width="100%" AutoGenerateColumns="False" DataKeyNames="id" AllowPaging="True" PageSize="10" OnPageIndexChanging="gvVeiculos_PageIndexChanging" ShowHeaderWhenEmpty="True">
                        <PagerStyle HorizontalAlign="Center" CssClass="pagination-centered" />
                    <Columns>                        
                        <asp:BoundField DataField="id" HeaderText="#ID" />
                        <asp:BoundField DataField="codvei" HeaderText="FROTA" />
                        <asp:BoundField DataField="tipvei" HeaderText="VEÍCULO" />
                        <asp:BoundField DataField="plavei" HeaderText="PLACA" />
                        <asp:BoundField DataField="reboque1" HeaderText="REBOQUE" />
                        <asp:BoundField DataField="tipoveiculo" HeaderText="TIPO" />
                        <asp:BoundField DataField="nucleo" HeaderText="FILIAL" />
                        <asp:BoundField DataField="transp" HeaderText="TRANSPORTADORA/PROPRIETÁRIO" />                             
                        <asp:BoundField DataField="ativo_inativo" HeaderText="STATUS"/> 
                 
                        <asp:TemplateField HeaderText="AÇÕES" ShowHeader="True">
                             <ItemTemplate >                              
                                 <asp:LinkButton ID="lnkEditar" runat="server" OnClick="Editar" CssClass="btn btn-primary btn-sm"><i class="fa fa-edit"></i> 
                                     </asp:LinkButton>
                               <%--<a class="btn btn-primary btn-sm" href="Frm_AltClientes.aspx?=<%# Eval("codcli") %>">
                                    <i class="fa fa-edit"></i>
                                    Editar
                                 </a> --%>
                         
                                 <a class="btn btn-info btn-sm" href="Frm_AltClientes.aspx?id=">
                                    <i class="fas fa-map-marker-alt"></i>
                                 </a> 
                                  <asp:LinkButton ID="lnkExcluir" runat="server" OnClick="Excluir" CssClass="btn btn-danger btn-sm" OnClientClick="ConfirmMessage();" ><i class="fa fa-trash"></i></i>
                                      </asp:LinkButton>
                        
                                 <%-- <a class="btn btn-danger btn-sm" href="Frm_AltClientes.aspx?id=">
                                    <i class="fa fa-trash"></i>                                    
                                    Excluir
                                 </a> --%>
                         
                             </ItemTemplate>
                        </asp:TemplateField>

               
                    </Columns>
                </asp:GridView>
         
            </div>
            <asp:HiddenField ID="txtconformmessageValue" runat="server" />
        </div>
    </div>
 </div>    
 
    <!-- JavaScript 
    <script src="vendor/datatables/jquery.dataTables.min.js"></script>
    <script src="src/javascript/script.js"></script>
    <!--
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/5.0.0-alpha1/css/bootstrap.min.css"     crossorigin="anonymous" />
    
        <script src="https://stackpath.bootstrapcdn.com/bootstrap/5.0.0-alpha1/js/bootstrap.min.js"        crossorigin="anonymous"></script>
    -->
<footer class="main-footer">
   <div class="float-right d-none d-sm-block">
     <b>Version</b> 2.1.0
   </div>
   <strong>Copyright &copy; 2021-2024 Capit Logística.</strong> Todos os direitos reservados.
</footer>

<!-- Page specific script -->
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