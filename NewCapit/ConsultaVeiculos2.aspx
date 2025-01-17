<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ConsultaVeiculos2.aspx.cs" Inherits="NewCapit.ConsultaVeiculos2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- Page Heading -->
    <div class="d-sm-flex align-items-left justify-content-between mb-4">
        <h1 class="h3 mb-0 text-gray-800">
            <i class="fas fa-shipping-fast"></i> Consulta Veículos</h1>
        <a href="#" class="d-none d-sm-inline-block btn btn-sm btn-primary shadow-sm">
            <i class="fas fa-shipping-fast"></i> Novo Cadastro
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
                             <li class="">                            
                             <class="user-profile dropdown-toggle" data-toggle="dropdown" aria-expanded="false">
                             <i class="fas fa-shipping-fast fa-2x text-gray-300"></i>
                             <span class=" fa fa-angle-down"></span>                               
                             <ul class="dropdown-menu dropdown-usermenu pull-right">                             
                             </li>    
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
                                    <asp:Label ID="TCNT" runat="server" Text=""></asp:Label>
                                  </span>
                              </li>
                              <li>
                                  <span>SBC: </span> 
                                  <span class="text-xs font-weight-bold text-danger text-uppercase mb-1">
                                    <asp:Label ID="TSBC" runat="server" Text=""></asp:Label>
                                  </span>
                              </li>
                              <li><span>MATRIZ: </span>
                                    <span class="text-xs font-weight-bold text-info text-uppercase mb-1">
                                        <asp:Label ID="TMatriz" runat="server" Text=""></asp:Label>
                                    </span>
                              </li>
                              <li><span>TAUBATÉ: </span>
                                    <span class="text-xs font-weight-bold text-warning text-uppercase mb-1">
                                        <asp:Label ID="TTaubate" runat="server" Text=""></asp:Label>
                                    </span>
                              </li>
                              <li><span>IPIRANGA: </span>
                                 <span class="text-xs font-weight-bold text-danger text-uppercase mb-1">
                                    <asp:Label ID="TIpiranga" runat="server" Text=""></asp:Label>
                                 </span>
                              </li>
                              <li><span>SÃO CARLOS: </span>
                                     <span class="text-xs font-weight-bold text-success text-uppercase mb-1">
                                        <asp:Label ID="TSC" runat="server" Text=""></asp:Label>
                                     </span>
                               </li>
                              <li><span>PR - PARANÁ: </span>
                                     <span class="text-xs font-weight-bold text-dark text-uppercase mb-1">
                                        <asp:Label ID="TPR" runat="server" Text=""></asp:Label>
                                     </span>
                              </li>
                              <li><span>PE - PERNAMBUCO: </span>
                                <span class="text-xs font-weight-bold text-secondary text-uppercase mb-1">
                                    <asp:Label ID="TPE" runat="server" Text=""></asp:Label>
                                </span>
                              </li>
                              <li><span>MG - MINAS GERAIS: </span>
                                  <span class="text-xs font-weight-bold text-danger text-uppercase mb-1">
                                      <asp:Label ID="TMG" runat="server" Text=""></asp:Label>
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


</asp:Content>
