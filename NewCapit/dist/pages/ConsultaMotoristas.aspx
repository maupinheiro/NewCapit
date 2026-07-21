<%@ Page Title="" Language="C#" MasterPageFile="Main.Master" AutoEventWireup="true" CodeBehind="ConsultaMotoristas.aspx.cs" Inherits="NewCapit.ConsultaMotoristas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 
<div class="content-wrapper">
    <section class="content">
    <div class="container-fluid">
        <div class="content-header">
         <div class="d-sm-flex align-items-center justify-content-between mb-4">
             <h1 class="h3 mb-2 text-gray-800">
                 <i class="fas fa-address-card"></i> Gestão de Motoristas</h1>
             <asp:LinkButton ID="lnkNovoCadastro" OnClick="lnkNovoCadastro_Click" CssClass="d-none d-sm-inline-block btn btn-sm btn-primary shadow-sm" runat="server"><i class="fas fa-user-plus"></i> Novo Cadastro</asp:LinkButton>             
         </div>
         <!-- Content Graficos -->
         <div class="row">
             <!-- Total de colaboradores frota/agregados/terceiros -->
             <div class="col-xl-3 col-md-6 mb-4">
                    <div class="info-box">
                        <span class="info-box-icon bg-primary"><img src="/img/totalMot.png" width="60px" alt="" /></span>
                        <div class="info-box-content">
                            <span class="info-box-text">TOTAL DE MOTORISTAS</span>
                            <span class="info-box-number">
                                <asp:Label ID="TotalCondutores" runat="server" Text=""></asp:Label></span>
                        </div>
                        <!-- /.info-box-content -->
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
                    </div>                    
             </div>             
             <!-- Total de colaboradores -->
             <div class="col-xl-3 col-md-6 mb-4">
               <div class="info-box">
                   <span class="info-box-icon bg-info"><img src="/img/totalFunc.png" width="60px" alt="" /></span>
                   <div class="info-box-content">
                       <span class="info-box-text">FUNCIONÁRIOS</span>
                       <span class="info-box-number">
                           <asp:Label ID="lblTotalEmpregados" runat="server" Text=""></asp:Label>
                   </div>
                   <!-- / Por filiais -->
                   <div class="col-auto">                            
                     <ul class="nav navbar-nav navbar-rigth">
                     <li class="">                            
                     <class="user-profile dropdown-toggle" data-toggle="dropdown" aria-expanded="false">      
                     <span class=" fa fa-angle-down"></span>                               
                     <ul class="dropdown-menu dropdown-usermenu pull-rigth">  
                     </li>
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
                  </div>
               </div>                    
             </div>  
             <!-- Total de agregados -->
             <div class="col-xl-3 col-md-6 mb-4">
              <div class="info-box">
                  <span class="info-box-icon bg-secondary"><img src="/img/totalAgre.png" width="60px" alt="" /></span>
                  <div class="info-box-content">
                      <span class="info-box-text">AGREGADOS</span>
                      <span class="info-box-number">
                          <asp:Label ID="Agregados" runat="server" Text=""></asp:Label>
                  </div>
                  <!-- / Por filiais -->
                  <div class="col-auto">                            
                    <ul class="nav navbar-nav navbar-rigth">
                    <li class="">                            
                    <class="user-profile dropdown-toggle" data-toggle="dropdown" aria-expanded="false">      
                    <span class=" fa fa-angle-down"></span>                               
                    <ul class="dropdown-menu dropdown-usermenu pull-rigth">  
                    </li>
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
                     <li><span>PARANÁ: </span>
                           <span class="text-xs font-weight-bold text-dark text-uppercase mb-1">
                              <asp:Label ID="AgPR" runat="server" Text=""></asp:Label>
                           </span>
                     </li>
                     <li><span>PERNAMBUCO: </span>
                      <span class="text-xs font-weight-bold text-secondary text-uppercase mb-1">
                          <asp:Label ID="AgPE" runat="server" Text=""></asp:Label>
                      </span>
                     </li>
                     <li><span>MINAS GERAIS: </span>
                        <span class="text-xs font-weight-bold text-danger text-uppercase mb-1">
                            <asp:Label ID="AgMG" runat="server" Text=""></asp:Label>
                        </span>
                     </li>
                   </ul>
                 </div>
              </div>                    
             </div>            
             <!-- Total de terceiros -->
             <div class="col-xl-3 col-md-6 mb-4">
                 <div class="info-box">
                     <span class="info-box-icon bg-warning"><img src="/img/totalTerc.png" width="60px" alt="" /></span>
                     <div class="info-box-content">
                         <span class="info-box-text">TERCEIROS</span>
                         <span class="info-box-number">
                             <asp:Label ID="Terceiros" runat="server" Text=""></asp:Label>
                     </div>
                     <!-- / Por filiais -->
                     <div class="col-auto">                            
                       <ul class="nav navbar-nav navbar-rigth">
                       <li class="">                            
                       <class="user-profile dropdown-toggle" data-toggle="dropdown" aria-expanded="false">      
                       <span class=" fa fa-angle-down"></span>                               
                       <ul class="dropdown-menu dropdown-usermenu pull-rigth">  
                       </li>
                        <li>
                     <span>CNT (CC): </span> 
                     <span class="text-xs font-weight-bold text-success text-uppercase mb-1">
                       <asp:Label ID="TCNT" runat="server" Text=""></asp:Label>
                     </span>
                 </li>
                 <li>
                     <span>ANCHIETA: </span> 
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
                 <li><span>PARANÁ: </span>
                        <span class="text-xs font-weight-bold text-dark text-uppercase mb-1">
                           <asp:Label ID="TPR" runat="server" Text=""></asp:Label>
                        </span>
                 </li>
                 <li><span>PERNAMBUCO: </span>
                   <span class="text-xs font-weight-bold text-secondary text-uppercase mb-1">
                       <asp:Label ID="TPE" runat="server" Text=""></asp:Label>
                   </span>
                 </li>
                 <li><span>MINAS GERAIS: </span>
                     <span class="text-xs font-weight-bold text-danger text-uppercase mb-1">
                         <asp:Label ID="TMG" runat="server" Text=""></asp:Label>
                     </span>
                 </li>
                      </ul>
                    </div>
                 </div>                    
             </div>
         </div>
    </div> 
    
        <!-- Corpo da grid -->
        <%--<div class="card shadow mb-3"> --%>
         <div class="card shadow-sm">
        <div class="card-body">  
            <%--<div class="row g-2">--%>
                <div class="row gy-0 mt-1">
                    <div class="col-md-3">
                        <asp:TextBox
                            ID="txtMotorista"
                            CssClass="form-control"
                            placeholder="Código, Nome do Motorista..."
                            runat="server"
                            AutoPostBack="true"
                            OnTextChanged="txtMotorista_TextChanged">
                        </asp:TextBox>
                    </div>
                    <div class="col-md-2">
                         <div class="form-group">                         
                             <asp:DropDownList ID="ddlTipoMot" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlTipoMot_SelectedIndexChanged">
                                 <asp:ListItem Value="" Text="Tipo de Motorista..."></asp:ListItem>
                                 <asp:ListItem Value="AGREGADO" Text="AGREGADO"></asp:ListItem>
                                 <asp:ListItem Value="AGREGADO FUNCIONÁRIO" Text="AGREGADO FUNCIONÁRIO"></asp:ListItem>
                                 <asp:ListItem Value="FUNCIONÁRIO" Text="FUNCIONÁRIO"></asp:ListItem>
                                 <asp:ListItem Value="TERCEIRO" Text="TERCEIRO"></asp:ListItem>
                                 <asp:ListItem Value="FUNCIONÁRIO TERCEIRO" Text="FUNCIONÁRIO TERCEIRO" ></asp:ListItem>
                             </asp:DropDownList>                        
                         </div>
                    </div>
                    <div class="col-md-2">
                         <div class="form-group">                         
                             <asp:DropDownList ID="ddlCargo" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlCargo_SelectedIndexChanged"> 
                             </asp:DropDownList>                        
                         </div>
                    </div>
                    <div class="col-md-3">
                        <asp:TextBox
                            ID="txtTransportadora"
                            CssClass="form-control"
                            placeholder="Código, Nome do Proprietário/Transp..."
                            runat="server"
                            AutoPostBack="true"
                            OnTextChanged="txtTransportadora_TextChanged">
                        </asp:TextBox>
                    </div>
                    <div class="col-md-2">
                         <div class="form-group">                         
                             <asp:DropDownList ID="ddlSituacao" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlSituacao_SelectedIndexChanged">
                                 <asp:ListItem Value="ATIVO" Text="ATIVO"></asp:ListItem>
                                 <asp:ListItem Value="INATIVO" Text="INATIVO"></asp:ListItem>
                                 <asp:ListItem Value="TODOS" Text="TODOS"></asp:ListItem>                             
                             </asp:DropDownList>                        
                         </div>
                    </div> 
                </div>
                <div class="row gy-0 mt-1">
                        <div class="col-3">
                            <asp:TextBox
                                ID="txtFilial"
                                CssClass="form-control"
                                placeholder="Filial..."
                                runat="server"
                                AutoPostBack="true"
                                OnTextChanged="txtFilial_TextChanged">
                            </asp:TextBox>
                        </div>
                        <div class="col-3">
                            <span id="lblVisiveis"></span>
                        </div>
                        <div class="col-6">
                            <span id="lblTotalGeral" runat="server" style="float: right;"></span>
                        </div>
                    </div>            
           <%-- </div>--%>
            <br />
            <div class="row g-3">
                <div class="table-responsive">                
                <asp:GridView 
                    ID="gvListMotoristas"                    
                    runat="server"
                    CssClass="grid-sap"
                    AutoGenerateColumns="False"
                    GridLines="None"
                    ShowHeaderWhenEmpty="True"
                    DataKeyNames="id"      
                    OnPageIndexChanging="gvListMotoristas_PageIndexChanging">                    
                <Columns>
                    <%--tamanho da foto 45x45--%>
                    <asp:ImageField DataImageUrlField="caminhofoto" HeaderText="Mot." ControlStyle-Width="50" ItemStyle-Width="50" ControlStyle-CssClass="rounded-circle" ItemStyle-HorizontalAlign="Center" />
                    
                    <asp:TemplateField HeaderText="MOTORISTA">
                        <ItemTemplate>
                             <%--<%# Eval("codmot") + " - " + Eval("nommot")%> --%>
                            <%# Eval("codmot") %>
                            <br>
                            <%# Eval("nommot") %>
                            </br>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="CARGO">
                        <ItemTemplate>                                
                            <%# Eval("tipomot") %>
                            <br>
                            <%# Eval("cargo") %>
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

                    <asp:TemplateField HeaderText="CELULAR">
                        <ItemTemplate>
                            <%# Eval("fone2")%>
                            <%--</br>
                            <asp:Label ID="lblIdade" runat="server"></asp:Label>
                            </br>--%>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="CADASTRO">
                        <ItemTemplate>
                            <%# Eval("cadmot") + " - " + Eval("status")%> 
                            <%--<br>
                            <asp:Label ID="lblTempoContrato" runat="server"></asp:Label>
                            </br> --%>                                
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="AÇÕES" ShowHeader="True" >
                        <ItemTemplate>
                            <br>
                            <asp:LinkButton ID="lnkEditar" runat="server" OnClick="Editar" CssClass="btn btn-primary btn-sm"><i class="fa fa-edit"></i> Editar</asp:LinkButton>
                          <%--  <a class="btn btn-danger btn-sm" href="#">
                                <i class="fa fa-trash"></i>
                                
                            </a>--%>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
                <pagertemplate>
                  <div class="d-flex justify-content-center align-items-center gap-2 flex-wrap">
                       <asp:LinkButton ID="btnPrimeiro" runat="server"
                                        OnClick="btnPrimeiro_Click"
                                        CssClass="btn btn-light btn-sm">
<i class="fas fa-angle-double-left"></i>
                                    </asp:LinkButton>
                       <asp:LinkButton ID="btnAnterior" runat="server"
                                        OnClick="btnAnterior_Click"
                                        CssClass="btn btn-light btn-sm">
<i class="fa fa-angle-left"></i>
                                    </asp:LinkButton>
                       <span class="fw-bold">Página
                           <asp:Label ID="lblPaginaAtual" runat="server" />
                            de
                           <asp:Label ID="lblTotalPaginas" runat="server" />
                       </span>
                      <asp:LinkButton
                          ID="btnProximo" runat="server"
                          OnClick="btnProximo_Click"
                          CssClass="btn btn-light btn-sm">
                          <i class="fa fa-angle-right"></i>
                      </asp:LinkButton>
                      <asp:LinkButton
                          ID="btnUltimo" runat="server"
                          OnClick="btnUltimo_Click"
                          CssClass="btn btn-light btn-sm">
                          <i class="fas fa-angle-double-right"></i>
                      </asp:LinkButton>
                      <span>Página:</span>
                         <asp:TextBox
                             ID="txtIrPagina" runat="server"
                             CssClass="form-control form-control-sm"
                             Style="width: 70px;" />
                         <asp:LinkButton 
                              ID="btnIrPagina" runat="server"
                              CssClass="btn btn-primary btn-sm"
                              OnClick="btnIrPagina_Click">
                              Buscar
                          </asp:LinkButton>
                  </div>
                </pagertemplate>
            </div>
            </div>
        </div>
    </div>
    </div>
    </section>
</div>
   
</asp:Content>
