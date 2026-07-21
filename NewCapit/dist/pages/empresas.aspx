<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="empresas.aspx.cs" Inherits="NewCapit.dist.pages.empresas" %>

<asp:Content ID="Content1"
    ContentPlaceHolderID="head"
    runat="server">

    <link href="../css/erp.css" rel="stylesheet" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        function PreviewLogo(input) {

            if (input.files && input.files[0]) {

                var reader = new FileReader();

                reader.onload = function (e) {

                    document.getElementById('<%= imgLogo.ClientID %>').src = e.target.result;

                };

                reader.readAsDataURL(input.files[0]);

            }

        }
    </script>
    <div class="content-wrapper">
        <section class="content">
            <div class="container-fluid">
                <br />
                <asp:UpdatePanel ID="upEmpresa" runat="server">
                    <ContentTemplate>
                        <!-- CABEÇALHO -->
                        <div class="erp-card">
                            <div class="erp-card-header">
                                <i class="fas fa-building"></i>
                                Cadastro de Empresas       
                            </div>
                            <div class="erp-card-body">
                                <div class="row">
                                    <!-- LOGO -->
                                    <div class="col-md-2 text-center">
                                        <div class="erp-logo-box">
                                            <asp:Image
                                                ID="imgLogo"
                                                runat="server"
                                                ImageUrl="~/dist/img/no-image.png"
                                                CssClass="img-fluid" />
                                        </div>  
                                        <br />
                                        <asp:FileUpload
                                            ID="fuLogo"
                                            runat="server"
                                            CssClass="form-control"
                                            onchange="PreviewImagem(this,'<%= imgLogo.ClientID %>');" />

                                    </div>
                                    <!-- DADOS -->
                                    <div class="col-md-10">
                                        <h5 class="erp-section-title">Dados da Empresa</h5>
                                        <div class="row">                                           
                                            <div class="col-md-1">
                                                <label>Empresa:</label>
                                               <asp:TextBox
                                                    ID="txtCodigo"
                                                    runat="server"
                                                    CssClass="form-control"
                                                    AutoPostBack="true"
                                                    OnTextChanged="txtCodigo_TextChanged">
                                               </asp:TextBox>
                                            </div>
                                            <div class="col-md-2">
                                                 <label>CNPJ:</label>
                                                 <asp:TextBox
                                                     ID="txtCNPJ"
                                                     runat="server"
                                                     CssClass="form-control mask-cnpj text-center text-blue" />
                                            </div>
                                            <div class="col-md-2 d-flex align-items-end">
                                                 <asp:Button
                                                     ID="btnConsultarReceita"
                                                     runat="server"
                                                     Text="Consultar"
                                                     CssClass="btn btn-primary w-100" 
                                                     OnClick="btnCnpj_Click"/>
                                            </div>
                                            <div class="col-md-2">
                                                <label>Status:</label>
                                                <asp:DropDownList
                                                    ID="ddlStatus"
                                                    runat="server"
                                                    CssClass="form-select">
                                                    <asp:ListItem>ATIVO</asp:ListItem>
                                                    <asp:ListItem>INATIVO</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-2">
                                                <label>Cadastro:</label>
                                                <asp:TextBox
                                                    ID="txtAbertura"
                                                    runat="server" 
                                                    CssClass="form-control text-center" />
                                            </div>
                                           
                                            
                                        </div>                                      
                                        <div class="row">
                                            <div class="col-md-6">
                                                <label>Razão Social:</label>
                                                <asp:TextBox
                                                    ID="txtRazaoSocial"
                                                    runat="server"
                                                    CssClass="form-control" />
                                            </div>
                                            <div class="col-md-4">
                                                <label>Nome Fantasia:</label>
                                                <asp:TextBox
                                                    ID="txtNomeFantasia"
                                                    runat="server"
                                                    CssClass="form-control" />
                                            </div>
                                            <div class="col-md-2">
                                                  <label>Tipo:</label>
                                                  <asp:TextBox
                                                      ID="txtTipo"
                                                      runat="server" 
                                                      CssClass="form-control text-center" />
                                            </div>
                                                                                      
                                        </div>                                         
                                        <div class="row">  
                                            <div class="col-md-8">
                                                <label>Atividade Principal:</label>
                                                <asp:TextBox
                                                    ID="txtAtividade_Principal"
                                                    runat="server" 
                                                    CssClass="form-control text-left" />
                                            </div>  
                                            <div class="col-md-2">
                                                <label>Abertura:</label>
                                                <asp:TextBox
                                                    ID="txtDtAbertura"
                                                    runat="server" 
                                                    CssClass="form-control text-center" />
                                            </div>
                                            <div class="col-md-2">
                                                <label>Situação:</label>
                                                <asp:TextBox
                                                    ID="txtSituacao"
                                                    runat="server" 
                                                    CssClass="form-control text-center" />
                                            </div>
                                        </div>

                                </div>

                            </div>

                        </div>
                        <!-- ========================================================= -->
                        <!-- ENDEREÇO -->
                        <!-- ========================================================= -->
                        <div class="erp-card">
                            <div class="erp-card-header">
                                <i class="fas fa-map-marker-alt"></i>
                                Endereço
                            </div>
                            <div class="erp-card-body">
                                <div class="row">
                                    <div class="col-md-1">
                                        <label>CEP:</label>
                                        <asp:TextBox
                                            ID="txtCEP"
                                            runat="server"
                                            CssClass="form-control mask-cep text-center" />
                                    </div>
                                    <div class="col-md-8">
                                        <label>Endereço:</label>
                                        <asp:TextBox
                                            ID="txtEndereco"
                                            runat="server"
                                            CssClass="form-control" />
                                    </div>
                                    <div class="col-md-1">
                                        <label>Número:</label>
                                        <asp:TextBox
                                            ID="txtNumero"
                                            runat="server"
                                            CssClass="form-control" />
                                    </div>
                                     <div class="col-md-2">
                                         <label>Complemento:</label>
                                         <asp:TextBox
                                             ID="txtComplemento"
                                             runat="server"
                                             CssClass="form-control" />
                                     </div>
                                </div>
                                <div class="row"> 
                                    <div class="col-md-4">
                                        <label>Bairro:</label>
                                        <asp:TextBox
                                            ID="txtBairro"
                                            runat="server"
                                            CssClass="form-control" />
                                    </div>
                                    <div class="col-md-4">
                                        <label>Município:</label>
                                        <asp:TextBox
                                            ID="txtMunicipio"
                                            runat="server"
                                            CssClass="form-control" />
                                    </div>
                                    <div class="col-md-1">
                                        <label>UF:</label>
                                        <asp:DropDownList
                                            ID="ddlUF"
                                            runat="server"
                                            CssClass="form-select">
                                            <asp:ListItem Value="">Selecione</asp:ListItem>
                                            <asp:ListItem>AC</asp:ListItem>
                                            <asp:ListItem>AL</asp:ListItem>
                                            <asp:ListItem>AP</asp:ListItem>
                                            <asp:ListItem>AM</asp:ListItem>
                                            <asp:ListItem>BA</asp:ListItem>
                                            <asp:ListItem>CE</asp:ListItem>
                                            <asp:ListItem>DF</asp:ListItem>
                                            <asp:ListItem>ES</asp:ListItem>
                                            <asp:ListItem>GO</asp:ListItem>
                                            <asp:ListItem>MA</asp:ListItem>
                                            <asp:ListItem>MT</asp:ListItem>
                                            <asp:ListItem>MS</asp:ListItem>
                                            <asp:ListItem>MG</asp:ListItem>
                                            <asp:ListItem>PA</asp:ListItem>
                                            <asp:ListItem>PB</asp:ListItem>
                                            <asp:ListItem>PR</asp:ListItem>
                                            <asp:ListItem>PE</asp:ListItem>
                                            <asp:ListItem>PI</asp:ListItem>
                                            <asp:ListItem>RJ</asp:ListItem>
                                            <asp:ListItem>RN</asp:ListItem>
                                            <asp:ListItem>RS</asp:ListItem>
                                            <asp:ListItem>RO</asp:ListItem>
                                            <asp:ListItem>RR</asp:ListItem>
                                            <asp:ListItem>SC</asp:ListItem>
                                            <asp:ListItem>SP</asp:ListItem>
                                            <asp:ListItem>SE</asp:ListItem>
                                            <asp:ListItem>TO</asp:ListItem>

                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-3">
                                        <label>Nome da UF:</label>
                                        <asp:TextBox
                                            ID="txtUFNome"
                                            runat="server"
                                            CssClass="form-control" />
                                    </div>
                                </div>
                            </div>

                        </div>
                        <!-- ========================================================= -->
                        <!-- FISCAL -->
                        <!-- ========================================================= -->
                        <div class="erp-card">
                            <div class="erp-card-header">
                                <i class="fas fa-file-invoice"></i>
                                Dados Fiscais
                            </div>
                            <div class="erp-card-body">
                                <div class="row">
                                    <div class="col-md-3">
                                        <label>Inscrição Estadual:</label>
                                        <asp:TextBox
                                            ID="txtInscricaoEstadual"
                                            runat="server"
                                            CssClass="form-control" />
                                    </div>
                                    <div class="col-md-3">
                                        <label>Código Município:</label>
                                        <asp:TextBox
                                            ID="txtCodigoMunicipio"
                                            runat="server"
                                            CssClass="form-control" />
                                    </div>
                                    <div class="col-md-3">
                                        <label>RNTRC:</label>
                                        <asp:TextBox
                                            ID="txtRNTRC"
                                            runat="server"
                                            CssClass="form-control" />
                                    </div>
                                    <div class="col-md-3">
                                        <label>Modal:</label>
                                        <asp:DropDownList
                                            ID="ddlModal"
                                            runat="server"
                                            CssClass="form-select">
                                            <asp:ListItem Value="">Selecione</asp:ListItem>
                                            <asp:ListItem>Rodoviário</asp:ListItem>
                                            <asp:ListItem>Ferroviário</asp:ListItem>
                                            <asp:ListItem>Aéreo</asp:ListItem>
                                            <asp:ListItem>Marítimo</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>

                        </div>
                        <!-- ===================================================== -->
                        <!-- CONTATOS -->
                        <!-- ===================================================== -->
                        <div class="erp-card">
                            <div class="erp-card-header">
                                <i class="fas fa-phone"></i>
                                Contatos
                            </div>
                            <div class="erp-card-body">
                                <div class="row">
                                    <div class="col-md-2">
                                        <label>Telefone:</label>
                                        <asp:TextBox
                                            ID="txtTelefone"
                                            runat="server"
                                            CssClass="form-control mask-telefone" />
                                    </div>
                                    <div class="col-md-5">
                                        <label>E-mail:</label>
                                        <asp:TextBox
                                            ID="txtEmail"
                                            runat="server"
                                            CssClass="form-control"
                                            TextMode="Email" />
                                    </div>
                                    <div class="col-md-5">
                                        <label>Site:</label>
                                        <asp:TextBox
                                            ID="txtSite"
                                            runat="server"
                                            CssClass="form-control" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="text-center mt-4 mb-4">
                            <asp:Button
                                ID="btnNovo"
                                runat="server"
                                Text="Novo"
                                CssClass="btn btn-secondary btn-lg me-2"
                                OnClick="btnNovo_Click" />
                            <asp:Button
                                ID="btnSalvar"
                                runat="server"
                                Text="Salvar"
                                CssClass="btn btn-success btn-lg me-2"
                                OnClick="btnSalvar_Click" />
                            <asp:Button
                                ID="btnCancelar"
                                runat="server"
                                Text="Cancelar"
                                CssClass="btn btn-danger btn-lg"
                                OnClick="btnCancelar_Click" />
                        </div>
                        <div class="erp-card">
                            <div class="erp-card-header">
                                <i class="fas fa-search"></i>
                                Empresas Cadastradas
                            </div>
                            <div class="erp-card-body">
                                <div class="row mb-3">
                                    <div class="col-md-4">
                                        <asp:TextBox
                                            ID="txtPesquisar"
                                            runat="server"
                                            CssClass="form-control"
                                            placeholder="Pesquisar..." />

                                    </div>
                                </div>
                                <table id="tblEmpresasCadastradas"
                                    class="table table-bordered table-hover table-striped erp-grid">
                                    <thead>
                                        <tr>
                                            <th>Empresa</th>
                                            <th>Razão Social</th>
                                            <th>Fantasia</th>
                                            <th>CNPJ</th>
                                            <th>Status</th>
                                            <th width="70">Editar</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater
                                            ID="rpEmpresas"
                                            runat="server">
                                            <ItemTemplate>
                                                <tr>
                                                    <td><%# Eval("codigo_empresa") %></td>
                                                    <td><%# Eval("razao_social") %></td>
                                                    <td><%# Eval("nome_fantasia") %></td>
                                                    <td><%# Eval("cnpj") %></td>
                                                    <td>
                                                        <span class='<%# Eval("status").ToString()=="ATIVO" ? "status-ativo" : "status-inativo" %>'>

                                                            <%# Eval("status") %>

                                                        </span>

                                                    </td>

                                                    <td class="text-center">

                                                        <asp:LinkButton
                                                            ID="btnEditar"
                                                            runat="server"
                                                            CssClass="btn btn-primary btn-grid"
                                                            CommandArgument='<%# Eval("codigo_empresa") %>'
                                                            OnClick="btnEditar_Click">

                            <i class="fas fa-edit"></i>

                                                        </asp:LinkButton>

                                                    </td>

                                                </tr>

                                            </ItemTemplate>

                                        </asp:Repeater>

                                    </tbody>

                                </table>
                                <table id="tblEmpresas"
                                    class="table table-bordered table-hover table-striped erp-grid">
                                    <thead>
                                        <tr>                                           
                                            <th>Empresa</th>
                                            <th>Descrição</th>
                                            <th>Razão Social</th>
                                            <th>Fantasia</th>
                                            <th>CNPJ</th>
                                            <th>Status</th>
                                            <th width="70">Editar</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater
                                            ID="Repeater1"
                                            runat="server">
                                            <ItemTemplate>
                                                <tr>                                                    
                                                    <td><%# Eval("codigo_empresa") %></td>
                                                    <td><%# Eval("descricao") %></td>
                                                    <td><%# Eval("razao_social") %></td>
                                                    <td><%# Eval("nome_fantasia") %></td>
                                                    <td><%# Eval("cnpj") %></td>
                                                    <td>
                                                        <span class='<%# Eval("status").ToString()=="ATIVO" ? "status-ativo" : "status-inativo" %>'>

                                                            <%# Eval("status") %>

                                                        </span>

                                                    </td>

                                                    <td class="text-center">

                                                        <asp:LinkButton
                                                            ID="btnEditar"
                                                            runat="server"
                                                            CssClass="btn btn-primary btn-grid"
                                                            CommandArgument='<%# Eval("codigo") %>'
                                                            OnClick="btnEditar_Click">

                            <i class="fas fa-edit"></i>

                                                        </asp:LinkButton>

                                                    </td>

                                                </tr>

                                            </ItemTemplate>

                                        </asp:Repeater>

                                    </tbody>

                                </table>


                            </div>
                            </section>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </section>
    </div>
</asp:Content>
