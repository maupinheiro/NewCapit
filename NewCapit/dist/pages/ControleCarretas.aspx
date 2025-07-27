<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="ControleCarretas.aspx.cs" Inherits="NewCapit.dist.pages.ControleCarretas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<%--<link rel="stylesheet" href="/css/styleTabela.css">--%>

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
                        <i class="fa fa-truck"></i> &nbsp;Controle de Carretas</h1>
                    <a href="/dist/pages/Frm_CadCarreta.aspx" class="d-none d-sm-inline-block btn btn-sm btn-primary shadow-sm">
                        <i class="fas fa-shipping-fast"></i> &nbsp;Novo Cadastro  </a>
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
                                                Total de Carretas
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
                                                  <span>CNT (CC): </span> 
                                                  <span class="text-xs font-weight-bold text-success text-uppercase mb-1">
                                                    <asp:Label ID="FrotaCNT" runat="server" Text=""></asp:Label>
                                                  </span>
                                              </li>
                                              <li>
                                                  <span>ANCHIETA: </span> 
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
                                          <span>CNT (CC): </span> 
                                          <span class="text-xs font-weight-bold text-success text-uppercase mb-1">
                                            <asp:Label ID="AgCNT" runat="server" Text=""></asp:Label>
                                          </span>
                                      </li>
                                      <li>
                                          <span>ANCHIETA: </span> 
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
                                          <span>CNT (CC): </span> 
                                          <span class="text-xs font-weight-bold text-success text-uppercase mb-1">
                                            <asp:Label ID="lbTerCNT" runat="server" Text=""></asp:Label>
                                          </span>
                                      </li>
                                      <li>
                                          <span>ANCHIETA: </span> 
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
                       <asp:TextBox ID="myInput" CssClass="form-control myInput" OnTextChanged="myInput_TextChanged" placeholder="Pesquisar ..." AutoPostBack="true" runat="server" Width="100%"></asp:TextBox>
                    </div>
                    <div class="card-body">
            <div class="table-responsive" style="height: 590px;font-size:smaller;">              
                <asp:GridView runat="server" ID="gvCarretas" CssClass="table table-bordered dataTable1 table-hover" Width="100%" AutoGenerateColumns="False" DataKeyNames="idcarreta" AllowPaging="True" PageSize="10" OnPageIndexChanging="gvCarretas_PageIndexChanging" ShowHeaderWhenEmpty="True">
                        <PagerStyle HorizontalAlign="Center" CssClass="pagination-centered" />
                    <Columns>   
                        <asp:BoundField DataField="codcarreta" HeaderText="FROTA" />
                        <asp:BoundField DataField="placacarreta" HeaderText="PLACA" />
                        <asp:BoundField DataField="licenciamento" HeaderText="EXERCICIO" />
                        <asp:BoundField DataField="modelo" HeaderText="MODELO" />
                        <asp:BoundField DataField="anocarreta" HeaderText="ANO" />                        
                        <asp:BoundField DataField="tiporeboque" HeaderText="TIPO" />
                        <asp:BoundField DataField="descprop" HeaderText="TRANSPORTADORA/PROPRIETÁRIO" />
                        <asp:BoundField DataField="nucleo" HeaderText="FILIAL" />
                        <asp:BoundField DataField="frota" HeaderText="ATRELADA" />
                        <asp:BoundField DataField="placa_cavalo" HeaderText="CAVALO" />
                        <asp:BoundField DataField="ativo_inativo" HeaderText="STATUS"/>                  
                        <asp:TemplateField HeaderText="AÇÃO" ShowHeader="True">
                             <ItemTemplate > 
                                 <asp:LinkButton ID="lnkEditar" runat="server" OnClick="Editar" CssClass="btn btn-primary btn-sm"><i class="fa fa-edit"></i> Editar</asp:LinkButton>  
                             </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <asp:HiddenField ID="txtconformmessageValue" runat="server" />                   
        </div>
             </div> 
    </div>
    
   
</asp:Content>