﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ConsultaMotoristas.aspx.cs" Inherits="NewCapit.ConsultaMotoristas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- Page Heading -->
    <div class="d-sm-flex align-items-center justify-content-between mb-4">
        <h1 class="h3 mb-2 text-gray-800">
            <i class="fas fa-address-card"></i> Consulta Motoristas</h1>
        <a href="Frm_CadMotoristas.aspx" class="d-none d-sm-inline-block btn btn-sm btn-primary shadow-sm"><i
            class="fas fa-user-plus"></i> Novo Cadastro
        </a>
    </div>
    <!-- Content Graficos -->
    <div class="row">
        <!-- Total de colaboradores frota/agregados/terceiros -->
        <div class="col-xl-3 col-md-6 mb-4">
            <div class="card border-left-primary shadow h-100 py-2">
                <div class="card-body">
                    <div class="row no-gutters align-items-center">
                        <div class="col mr-2">
                            <div class="text-xs font-weight-bold text-primary text-uppercase mb-1">
                                Total de Condutores
                            </div>
                            <div class="h5 mb-0 font-weight-bold text-gray-800">
                                <asp:Label ID="TotalCondutores" runat="server" Text=""></asp:Label>
                            </div>
                        </div>                        
                         <!-- sub menu total de condutores -->
                         <div class="col-auto">                            
                              <ul class="nav navbar-nav navbar-rigth">
                              <li class="">                            
                              <class="user-profile dropdown-toggle" data-toggle="dropdown" aria-expanded="false">      
                              <span class=" fa fa-angle-down"></span>                               
                              <ul class="dropdown-menu dropdown-usermenu pull-rigth">                             
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
                         <div class="col-auto">
                             <img src="img/totalMot.png" width="60px" alt="" />
                         </div>
                         <!-- fim -->
                    </div>
                </div>
            </div>
        </div>

        <!-- Total de colaboradores -->
        <div class="col-xl-3 col-md-6 mb-4">
            <div class="card border-left-info shadow h-100 py-2">
                <div class="card-body">
                    <div class="row no-gutters align-items-center">
                        <div class="col mr-2">
                            <div class="text-xs font-weight-bold text-info text-uppercase mb-1">
                                Colaboradores                                  
                            </div>
                            <div class="row no-gutters align-items-center">
                                <div class="col-auto">
                                    <div class="h5 mb-0 mr-3 font-weight-bold text-gray-800">
                                        <asp:Label ID="lblTotalEmpregados" runat="server" Text=""></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- Sub menu colaboradores -->
                        <div class="col-auto">                            
                            <ul class="nav navbar-nav navbar-right">
                                <li class="">                            
                                    <class="user-profile dropdown-toggle" data-toggle="dropdown" aria-expanded="false">            
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
                        <div class="col-auto">
                            <img src="img/totalFunc.png" width="60px" alt="" />
                        </div>
                    </div>
                </div>

            </div>
        </div>

        <!-- Total de agregados -->
        <div class="col-xl-3 col-md-6 mb-4">
            <div class="card border-left-danger shadow h-100 py-2">
                <div class="card-body">
                    <div class="row no-gutters align-items-center">
                        <div class="col mr-2">
                            <div class="text-xs font-weight-bold text-danger text-uppercase mb-1">
                                Agregados
                            </div>
                            <div class="h5 mb-0 font-weight-bold text-gray-800">
                                <asp:Label ID="Agregados" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <!-- Sub menu agregados -->
                        <div class="col-auto">                            
                            <ul class="nav navbar-nav navbar-right">
                                <li class="">                            
                                    <class="user-profile dropdown-toggle" data-toggle="dropdown" aria-expanded="false">            
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
                        <div class="col-auto">
                            <img src="img/totalAgre.png" width="60px" alt="" />
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Total de terceiros -->
        <div class="col-xl-3 col-md-6 mb-4">
            <div class="card border-left-warning shadow h-100 py-2">
                <div class="card-body">
                    <div class="row no-gutters align-items-center">
                        <div class="col mr-2">
                            <div class="text-xs font-weight-bold text-warning text-uppercase mb-1">
                                Terceiros                                       
                            </div>
                            <div class="row no-gutters align-items-center">
                                <div class="col-auto">
                                    <div class="h5 mb-0 mr-3 font-weight-bold text-gray-800">
                                        <asp:Label ID="Terceiros" runat="server" Text=""></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- Sub menu terceiros -->
                        <div class="col-auto">                            
                             <ul class="nav navbar-nav navbar-right">
                                 <li class="">                            
                                     <class="user-profile dropdown-toggle" data-toggle="dropdown" aria-expanded="false">             
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
                        <div class="col-auto">
                            <img src="img/totalTerc.png" width="60px" alt="" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
       
    </div>
   
    <!-- Corpo da grid -->
    <div class="card shadow mb-4">
        <div class="card-body">
            <div class="table-responsive">
                <asp:GridView runat="server" ID="gvListMotoristas" CssClass="table table-bordered dataTable1 table-hover" Width="100%" AutoGenerateColumns="False" DataKeyNames="id">
                    <Columns>
                        <asp:ImageField DataImageUrlField="caminhofoto" HeaderText="Mot." ControlStyle-Width="45" ItemStyle-Width="45" ControlStyle-CssClass="rounded-circle" ItemStyle-HorizontalAlign="Center" />
                        <asp:TemplateField HeaderText="MOTORISTA">
                            <ItemTemplate>
                                <!-- <%# Eval("codmot") + " - " + Eval("nommot")%> -->
                                <%# Eval("codmot") %>
                                <br>
                                <%# Eval("nommot") %>
                                </br>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="CARGO" HeaderStyle-Width="1">
                            <ItemTemplate>
                                <%# Eval("cargo") %>
                                <br>
                                <%# Eval("tipomot") %>
                                </br>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="FUNÇÃO">
                            <ItemTemplate>
                                <%# Eval("funcao") %>
                                <br>
                                <%# Eval("horario") %>
                                </br>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="FILIAL">
                            <ItemTemplate>
                                <%# Eval("nucleo") %>
                                <br>
                                <%# Eval("codtra") + " - " + Eval("transp")%>                                
                                </br>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="CPF">
                            <ItemTemplate>
                                <%# Eval("cpf")%>
                                <br>
                                <asp:Label ID="lblIdade" runat="server"></asp:Label>
                                </br>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="CADASTRO">
                            <ItemTemplate>
                                <%# Eval("cadmot") + " - " + Eval("status")%> 
                                <br>
                                <asp:Label ID="lblTempoContrato" runat="server"></asp:Label>
                                </br>
                            </ItemTemplate>
                        </asp:TemplateField>



                        <asp:TemplateField HeaderText="AÇÕES" ShowHeader="True" HeaderStyle-Width="140px">
                            <ItemTemplate>
                                <br>
                                <asp:LinkButton ID="lnkEditar" runat="server" CssClass="btn btn-primary btn-sm"><i class="fa fa-edit"></i>Editar</asp:LinkButton>
                                <a class="btn btn-danger btn-sm" href="#">
                                    <i class="fa fa-trash"></i>
                                    Excluir
                                </a>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>

        </div>
    </div>

    <!-- JavaScript -->
    <script src="vendor/datatables/jquery.dataTables.min.js"></script>
    <script src="src/javascript/script.js"></script>


</asp:Content>
