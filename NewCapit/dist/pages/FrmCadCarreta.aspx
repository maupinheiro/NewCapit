<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="FrmCadCarreta.aspx.cs" Inherits="NewCapit.dist.pages.FrmCadCarreta" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

 
 <div class="content-wrapper">
     <section class="content">
         <div class="container-fluid">
             <br />
             <div class="card card-info">
                 <div class="card-header">
                     <h3 class="card-title"><i class="fas fa-shipping-fast"></i>&nbsp;CARRETAS - ATUALIZAÇÃO</h3>
                 </div>
             </div>
             <div class="card-header">
                 <!-- linha 1 -->
                 <div class="row g-3">
                     <div class="col-md-1">
                         <div class="form-group">
                             <span class="details">CÓDIGO:</span>
                             <asp:TextBox ID="txtCodVei" Style="text-align: center" runat="server" CssClass="form-control" placeholder="" MaxLength="9"></asp:TextBox>
                         </div>
                     </div>
                     <div class="col-md-2">
                         <div class="form-group">
                             <span class="">TIPO DE CARRETA:</span>
                             <asp:DropDownList ID="ddlTipo" runat="server" CssClass="form-control">
                                 <asp:ListItem Value="" Text=""></asp:ListItem>
                                 <asp:ListItem Value="AGREGADO" Text="AGREGADO"></asp:ListItem>
                                 <asp:ListItem Value="FROTA" Text="FROTA"></asp:ListItem>
                                 <asp:ListItem Value="TERCEIRO" Text="TERCEIRO"></asp:ListItem>
                             </asp:DropDownList>
                             <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="ddlTipo" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
                         </div>
                     </div>
                     <div class="col-md-2">
                         <div class="form_group">
                             <span class="details">FILIAL:</span>
                             <asp:DropDownList ID="cbFiliais" name="nomeFiliais" runat="server" CssClass="form-control" AutoPostBack="true"></asp:DropDownList>
                         </div>
                     </div>
                     <div class="col-md-3"></div>
                     <div class="col-md-2">
                         <div class="form-group">
                             <span class="details">CADASTRO:</span>
                             <asp:Label ID="txtDtcVei" runat="server" CssClass="form-control" value=""></asp:Label>
                         </div>
                     </div>
                     <div class="col-md-2">
                         <div class="form_group">
                             <span class="details">SITUAÇÃO:</span>
                             <asp:DropDownList ID="ddlSituacao" runat="server" CssClass="form-control">
                                 <asp:ListItem Value="ATIVO" Text="ATIVO"></asp:ListItem>
                                 <asp:ListItem Value="INATIVO" Text="INATIVO"></asp:ListItem>
                             </asp:DropDownList>
                         </div>
                     </div>
                 </div>
                 <!-- linha 2 -->
                 <div class="row g-3">
                     <div class="col-md-1">
                         <div class="form-group">
                             <span class="details">PLACA:</span>
                             <asp:TextBox ID="txtPlaca" Style="text-align: center" runat="server" CssClass="form-control" placeholder="" MaxLength="8" AutoPostBack="True" OnTextChanged="txtPlaca_TextChanged"></asp:TextBox>
                             <asp:RequiredFieldValidator runat="server" ID="rvftxtPlaca" ControlToValidate="txtPlaca" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                         </div>
                     </div>
                     <div class="col-md-1">
                         <div class="form-group">
                             <span class="details">UF:</span>
                             <asp:DropDownList ID="ddlEstados" runat="server" AutoPostBack="True" class="form-control" OnSelectedIndexChanged="ddlEstados_SelectedIndexChanged">
                             </asp:DropDownList>
                             <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlEstados" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
                         </div>
                     </div>
                     <div class="col-md-3">
                         <div class="form_group">
                             <span class="details">MUNICIPIO:</span>
                             <asp:DropDownList ID="ddlCidades" runat="server" class="form-control">
                             </asp:DropDownList>
                             <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlCidades" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
                         </div>
                     </div>
                 </div>
                 <!-- linha 3 -->
                 <div class="row g-3">
                     <div class="col-md-1">
                         <div class="form-group">
                             <span class="details">PATRIMÔNIO:</span>
                             <asp:TextBox ID="txtControlePatrimonio" runat="server" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="20"></asp:TextBox>
                         </div>
                     </div>
                     <div class="col-md-1">
                         <div class="form_group">
                             <span class="details">FAB/MOD.:</span>
                             <asp:TextBox ID="txtAno" runat="server" Style="text-align: center" CssClass="form-control" placeholder="0000/0000" MaxLength="9"></asp:TextBox>
                             <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator14" ControlToValidate="txtAno" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                         </div>
                     </div>
                     <div class="col-md-1">
                         <div class="form_group">
                             <span class="details">AQUISIÇÃO:</span>
                             <asp:TextBox ID="txtDataAquisicao" runat="server" Style="text-align: center" CssClass="form-control" placeholder="00/00/0000" MaxLength="10"></asp:TextBox>
                             <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator15" ControlToValidate="txtDataAquisicao" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                         </div>
                     </div>
                     <div class="col-md-2">
                         <div class="form_group">
                             <span class="details">RENAVAM:</span>
                             <asp:TextBox ID="txtRenavam" runat="server" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="25"></asp:TextBox>
                             <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator16" ControlToValidate="txtRenavam" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                         </div>
                     </div>
                     <div class="col-md-2">
                         <div class="form_group">
                             <span class="details">CHASSI:</span>
                             <asp:TextBox ID="txtChassi" runat="server" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="30"></asp:TextBox>
                             <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator17" ControlToValidate="txtChassi" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                         </div>
                     </div>
                     <div class="col-md-1">
                         <div class="form_group">
                             <span class="details">LICENCIAMENTO:</span>
                             <asp:TextBox ID="txtLicenciamento" runat="server" CssClass="form-control" placeholder="00/00/0000" MaxLength="10" Style="text-align: center"></asp:TextBox>
                             <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator18" ControlToValidate="txtLicenciamento" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                         </div>
                     </div>
                 </div>
                 <div class="row g-3">
                     <div class="col-md-1">
                         <div class="form_group">
                             <span class="details">COMPRIMENTO:</span>
                             <asp:TextBox ID="txtComprimento" runat="server" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="10"></asp:TextBox>
                             <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator19" ControlToValidate="txtComprimento" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                         </div>
                     </div>
                     <div class="col-md-1">
                         <div class="form_group">
                             <span class="details">LARGURA:</span>
                             <asp:TextBox ID="txtLargura" runat="server" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="10"></asp:TextBox>
                             <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator20" ControlToValidate="txtLargura" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                         </div>
                     </div>
                     <div class="col-md-1">
                         <div class="form_group">
                             <span class="details">ALTURA:</span>
                             <asp:TextBox ID="txtAltura" runat="server" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="10"></asp:TextBox>
                             <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator21" ControlToValidate="txtAltura" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                         </div>
                     </div>
                     <div class="col-md-1">
                         <div class="form_group">
                             <span class="details">ODOMETRO:</span>
                             <asp:TextBox ID="txtCronotacografo" runat="server" CssClass="form-control" placeholder="km" MaxLength="10" Style="text-align: center"></asp:TextBox>
                             <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator25" ControlToValidate="txtCronotacografo" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                         </div>
                     </div>
                 </div>
                 <!-- linha 3 -->
                 <div class="row g-3">
                     <div class="col-md-5">
                         <div class="form_group">
                             <span class="details">MARCA:</span>
                             <asp:DropDownList ID="ddlMarca" name="nomeMarca" runat="server" CssClass="form-control" AutoPostBack="true"></asp:DropDownList>
                             <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="ddlMarca" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
                         </div>
                     </div>
                     <div class="col-md-5">
                         <div class="form-group">
                             <span class="details">MODELO:</span>
                             <asp:TextBox ID="txtModelo" runat="server" CssClass="form-control" placeholder="" MaxLength="40"></asp:TextBox>
                             <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator26" ControlToValidate="txtModelo" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                         </div>
                     </div>
                     <div class="col-md-2">
                         <div class="form_group">
                             <span class="details">COR:</span>
                             <asp:DropDownList ID="ddlCor" runat="server" CssClass="form-control" AutoPostBack="true"></asp:DropDownList>
                             <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="ddlCor" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
                         </div>
                     </div>
                 </div>
                 <!-- linha 4 -->
                 <div class="row g-3">
                     <div class="col-md-1">
                         <div class="form-group">
                             <span class="details">CÓD.TEC.:</span>
                             <asp:TextBox ID="txtCodRastreador" runat="server" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="4" OnTextChanged="txtCodRastreador_TextChanged" AutoPostBack="true"></asp:TextBox>
                             <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator27" ControlToValidate="txtCodRastreador" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                         </div>
                     </div>
                     <div class="col-md-4">
                         <div class="form_group">
                             <span class="details">TECNOLOGIA/RASTREADOR:</span>
                             <asp:DropDownList ID="ddlTecnologia" name="tecnologia" runat="server" CssClass="form-control select2" OnSelectedIndexChanged="ddlTecnologia_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                             <asp:RequiredFieldValidator ID="RequiredFieldValidator28" runat="server" ControlToValidate="ddlTecnologia" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
                         </div>
                     </div>
                     <div class="col-md-2">
                         <div class="form-group">
                             <span class="details">ID:</span>
                             <asp:TextBox ID="txtId" runat="server" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="11"></asp:TextBox>
                             <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator29" ControlToValidate="txtId" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                         </div>
                     </div>
                     <div class="col-md-3">
                         <div class="form-group">
                             <span class="">COMUNICAÇÃO:</span>
                             <asp:DropDownList ID="ddlComunicacao" runat="server" CssClass="form-control">
                                 <asp:ListItem Value="" Text=""></asp:ListItem>
                                 <asp:ListItem Value="GPS/DUPLO GPS" Text="GPS/DUPLO GPS"></asp:ListItem>
                                 <asp:ListItem Value="GPS/CRPS" Text="GPS/CRPS"></asp:ListItem>
                                 <asp:ListItem Value="GPS/GPRS GLOBAL" Text="GPS/GPRS GLOBAL"></asp:ListItem>
                                 <asp:ListItem Value="GPS/GPRS+SATÉLITE" Text="GPS/GPRS+SATÉLITE"></asp:ListItem>
                                 <asp:ListItem Value="GPS/SATÉLITE" Text="GPS/SATÉLITE"></asp:ListItem>
                                 <asp:ListItem Value="NÃO TEM" Text="NÃO TEM"></asp:ListItem>
                                 <asp:ListItem Value="RF/GPS/GPRS" Text="RF/GPS/GPRS"></asp:ListItem>
                                 <asp:ListItem Value="OUTROS" Text="OUTROS"></asp:ListItem>
                             </asp:DropDownList>
                             <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="ddlComunicacao" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
                         </div>
                     </div>
                 </div>
                 <!-- linha 5 -->
                 <div class="row g-3">
                     <div class="col-md-1">
                         <div class="form-group">
                             <span class="details">CÓD.PROP.:</span>
                             <asp:TextBox ID="txtCodTra" runat="server" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="11" AutoPostBack="true" OnTextChanged="txtCodTra_TextChanged"></asp:TextBox>
                             <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator30" ControlToValidate="txtCodTra" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>

                         </div>
                     </div>

                     <div class="col-md-6">
                         <div class="form_group">
                             <span class="details">PROPRIETÁRIO/TRANSPORTADORA:</span>
                             <asp:DropDownList ID="ddlAgregados" class="form-control select2" runat="server" OnSelectedIndexChanged="ddlAgregados_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                             <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="ddlAgregados" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
                         </div>
                     </div>

                     <div class="col-md-3">
                         <div class="form-group">
                             <span class="details">ANTT/RNTRC:</span>
                             <asp:TextBox ID="txtAntt" runat="server" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="15"></asp:TextBox>
                             <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator31" ControlToValidate="txtAntt" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                         </div>
                     </div>
                 </div>
                 <!-- linha 6 -->
                 <div class="row g-3">

                     <div class="col-md-2">
                         <div class="form-group">
                             <span class="">CARRETA:</span>
                             <asp:DropDownList ID="ddlCarreta" runat="server" CssClass="form-control">
                                 <asp:ListItem Value="" Text=""></asp:ListItem>
                                 <asp:ListItem Value="PROPRIA" Text="PROPRIA"></asp:ListItem>
                                 <asp:ListItem Value="ALUGADA" Text="ALUGADA"></asp:ListItem>
                             </asp:DropDownList>
                             <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="ddlCarreta" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
                         </div>
                     </div>
                 </div>
                 <!-- linha 7 do formulario -->
                 <div class="row g-3">
                     <div class="col-md-1">
                         <div class="form-group">
                             <span class="details">TARA:</span>
                             <asp:TextBox ID="txtTara" runat="server" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="6"></asp:TextBox>
                         </div>
                     </div>
                 </div>

                 <!-- Linha 8 do formulario -->
                 <div class="row g-3">
                     <div class="col-md-2">
                         <div class="form-group">
                             <span class="details">CADASTRADO EM:</span>
                             <asp:TextBox ID="txtDataCadastro" runat="server" Style="text-align: center" CssClass="form-control" placeholder=""></asp:TextBox>
                         </div>
                     </div>
                     <div class="col-md-4">
                         <div class="form-group">
                             <span class="details">POR:</span>
                             <asp:TextBox ID="txtCadastradoPor" runat="server" Style="text-align: left" CssClass="form-control" placeholder="" MaxLength="60"></asp:TextBox>
                         </div>
                     </div>
                 </div>
                 <!-- linha 9 -->
                 <div class="row g-3">
                     <div class="col-md-1">
                         <asp:Button ID="btnSalvarCarreta" CssClass="btn btn-outline-success  btn-lg" runat="server" Text="Cadastrar" OnClick="btnSalvarCarreta_Click" />
                     </div>
                     <div class="col-md-1">
                         <a href="ConsultaCarretas.aspx" class="btn btn-outline-danger btn-lg">Cancelar               
                         </a>
                     </div>
                 </div>
             </div>
         </div>
     </section>
     <!-- Mensagens de erro toast -->
     <div class="toast-container position-fixed top-0 end-0 p-3">
         <div id="toastNotFound" class="toast align-items-center text-bg-danger border-0" role="alert" aria-live="assertive" aria-atomic="true">
             <div class="d-flex">
                 <div class="toast-body" id="mensagem">
                     Código, não encontrado no sistema. Verifique o número digitado. 
                 </div>
                 <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast" aria-label="Close"></button>
             </div>
         </div>
     </div>

 </div>


 <footer class="main-footer">
     <div class="float-right d-none d-sm-block">
         <b>Version</b> 3.1.0 
     </div>
     <strong>Copyright &copy; 2023-2025 <a href="#">Capit Logística</a>.</strong> Todos os direitos reservados.
 </footer>
 <!-- Page specific script -->

</asp:Content>
