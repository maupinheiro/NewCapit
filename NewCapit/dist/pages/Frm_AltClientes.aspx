<%@ Page Title="" Language="C#" MasterPageFile="Main.Master" AutoEventWireup="true" CodeBehind="Frm_AltClientes.aspx.cs" Inherits="NewCapit.dist.pages.Frm_AltClientes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- Google Font: Source Sans Pro -->
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,400i,700&display=fallback">
    <!-- Font Awesome -->
    <link rel="stylesheet" href="../../plugins/fontawesome-free/css/all.min.css">
    <!-- Theme style -->
    <link rel="stylesheet" href="../../dist/css/adminlte.min.css">
    <div class="content-wrapper">
        <!-- Main content -->
        <section class="content">
            <div class="container-fluid">
                <br />
                <div class="card card-primary">
                    <div class="card-header">
                        <h3 class="card-title">CLIENTES - ATUALIZAÇÃO DE DADOS</h3>
                    </div>
                </div>                
                <div class="card-header">
                     <div class="row g-3">
                            <div class="col-md-1">
                                <div class="form-group">
                                    <span class="details">CÓDIGO:</span>
                                    <asp:TextBox ID="txtCodCli" runat="server" ForeColor="Blue" CssClass="form-control" placeholder="" MaxLength="11"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-2">
                                <div class="form_group">
                                    <span class="details">TIPO DE CLIENTE:</span>
                                    <asp:DropDownList ID="cboTipo" runat="server" ForeColor="Blue" CssClass="form-control" Width="250px">
                                        <asp:ListItem Value="" Text="SELECIONE"></asp:ListItem>
                                        <asp:ListItem Value="CLIENTE" Text="CLIENTE"></asp:ListItem>
                                        <asp:ListItem Value="EMBARCADOR" Text="EMBARCADOR"></asp:ListItem>
                                        <asp:ListItem Value="TRANSPORTADOR" Text="TRANSPORTADOR"></asp:ListItem>
                                        <asp:ListItem Value="OPERADOR LOGÍSTICO" Text="OPERADOR LOGÍSTICO"></asp:ListItem>
                                        <asp:ListItem Value="OUTROS" Text="OUTROS"></asp:ListItem>
                                    </asp:DropDownList><br />
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form_group">
                                    <span class="details">UNIDADE:</span>
                                    <asp:TextBox ID="txtUnidade" runat="server" ForeColor="Blue" CssClass="form-control" placeholder="" MaxLength="45"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-2">
                                <span class="">REGIÃO DO PAÍS:</span>
                                <asp:DropDownList ID="cboRegiao" runat="server" ForeColor="Blue" CssClass="form-control" Width="250px">
                                    <asp:ListItem Value="" Text="SELECIONE"></asp:ListItem>
                                    <asp:ListItem Value="NORTE" Text="NORTE"></asp:ListItem>
                                    <asp:ListItem Value="SUL" Text="SUL"></asp:ListItem>
                                    <asp:ListItem Value="SUDESTE" Text="SUDESTE"></asp:ListItem>
                                    <asp:ListItem Value="CENTRO-OESTE" Text="CENTRO-OESTE"></asp:ListItem>
                                    <asp:ListItem Value="NORDESTE" Text="NORDESTE"></asp:ListItem>
                                </asp:DropDownList><br />
                            </div>

                            <div class="col-md-1">
                                <div class="form-group">
                                    <span class="details">SAPIENS:</span>
                                    <asp:TextBox ID="txtCodSapiens" runat="server" ForeColor="Blue" CssClass="form-control" placeholder="" MaxLength="10"></asp:TextBox>
                                </div>
                            </div>

                            <div class="col-md-1">
                                <div class="form-group">
                                    <span class="details">CÓD.VW:</span>
                                    <asp:TextBox ID="txtCodVw" runat="server" ForeColor="Blue" CssClass="form-control" placeholder="" MaxLength="10"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-1">
                                <div class="form-group">
                                    <span class="">STATUS:</span>
                                    <asp:DropDownList ID="ddlStatus" runat="server" ForeColor="Blue" CssClass="form-control">
                                        <asp:ListItem Value="ATIVO" Text="ATIVO"></asp:ListItem>
                                        <asp:ListItem Value="INATIVO" Text="INATIVO"></asp:ListItem>
                                    </asp:DropDownList></>
                                </div>
                            </div>
                        </div>
                     <div class="row g-3">
                            <div class="col-md-2">
                                <div class="form-group">
                                    <span class="details">CNPJ:</span>
                                    <asp:TextBox ID="txtCnpj" class="form-control" data-inputmask='"mask": "99.999.999/9999-99"' data-mask ForeColor="Blue" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-1">
                                <br />
                                <asp:Button ID="btnCnpj" runat="server" Text="Pesquisar" CssClass="btn btn-outline-warning" OnClick="btnCnpj_Click" />
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <span class="details">RAZÃO SOCIAL:</span>
                                    <asp:TextBox ID="txtRazCli" runat="server" ForeColor="Blue" CssClass="form-control" value=""></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-1">
                                <div class="form-group">
                                    <span class="details">TIPO:</span>
                                    <asp:TextBox ID="txtTipo" runat="server" ForeColor="Blue" CssClass="form-control" value=""></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-1">
                                <div class="form-group">
                                    <span class="details">ABERTURA:</span>
                                    <asp:TextBox ID="txtAbertura" runat="server" ForeColor="Blue" CssClass="form-control" Width="130px" value=""></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-1">
                                <div class="form-group">
                                    <span class="details">SITUAÇÃO:</span>
                                    <asp:TextBox ID="txtSituacao" runat="server" ForeColor="Blue" CssClass="form-control" value=""></asp:TextBox>
                                </div>
                            </div>
                        </div>
                     <div class="row g-3">
                            <div class="col-md-3">
                                <div class="form-group">
                                    <span class="details">NOME FANTASIA:</span>
                                    <asp:TextBox ID="txtNomCli" runat="server" CssClass="form-control" placeholder="" MaxLength="50" ForeColor="Blue"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-2">
                                <div class="form-group">
                                    <span class="details">INSC. ESTADUAL:</span>
                                    <asp:TextBox ID="txtInscEstadual" runat="server" ForeColor="Blue" CssClass="form-control" placeholder="" MaxLength="15"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-2">
                                <div class="form-group">
                                    <span class="details">CONTATO:</span>
                                    <asp:TextBox ID="txtConCli" runat="server" ForeColor="Blue" CssClass="form-control" placeholder="" MaxLength="30"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-2">
                                <div class="form-group">
                                    <span class="details">FONE FIXO:</span>
                                    <asp:TextBox ID="txtTc1Cli" runat="server" ForeColor="Blue" CssClass="form-control" data-mask="(00) 0000-0000" MaxLength="15"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-1">
                                <div class="form-group">
                                    <span class="details">RAMAL:</span>
                                    <asp:TextBox ID="txtRamal" runat="server" ForeColor="Blue" CssClass="form-control" placeholder="9999" MaxLength="4"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-2">
                                <div class="form-group">
                                    <span class="details">CELULAR:</span>
                                    <asp:TextBox ID="txtTc2Cli" runat="server" ForeColor="Blue" CssClass="form-control" data-mask="(00) 0 0000-0000" MaxLength="16"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                     <div class="row g-3">
                            <div class="col-md-3">
                                <div class="form-group">
                                    <span class="details">PROGRAMADORES:</span>
                                    <asp:TextBox ID="txtProgramador" runat="server" ForeColor="Blue" CssClass="form-control" placeholder="" MaxLength="35"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-2">
                                <div class="form-group">
                                    <span class="details">TELEFONE/RAMAL:</span>
                                    <asp:TextBox ID="txtContato" runat="server" ForeColor="Blue" CssClass="form-control" placeholder="" MaxLength="25"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-7">
                                <div class="form-group">
                                    <span class="details">E-MAIL(S):</span>
                                    <asp:TextBox ID="txtEmail" runat="server" ForeColor="Blue" CssClass="form-control" placeholder="" MaxLength="200"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                     <div class="row g-3">
                            <div class="col-md-1">
                                <div class="form-group">
                                    <span class="details">CEP:</span>
                                    <asp:TextBox ID="txtCepCli" runat="server" ForeColor="Blue" CssClass="form-control" Width="130px" placeholder="99999-999" MaxLength="9"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-1">
                                <br />
                                <asp:Button ID="btnCep" runat="server" Text="Pesquisar" CssClass="btn btn-outline-warning" OnClick="btnCep_Click" />
                            </div>
                            <div class="col-md-7">
                                <div class="form-group">
                                    <span class="details">ENDEREÇO:</span>
                                    <asp:TextBox ID="txtEndCli" runat="server" ForeColor="Blue" CssClass="form-control" MaxLength="60"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-1">
                                <div class="form-group">
                                    <span class="details">Nº:</span>
                                    <asp:TextBox ID="txtNumero" Style="text-align: center" runat="server" ForeColor="Blue" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-2">
                                <div class="form-group">
                                    <span class="details">COMPLEMENTO:</span>
                                    <asp:TextBox ID="txtComplemento" Style="text-align: center" runat="server" ForeColor="Blue" CssClass="form-control" placeholder="" MaxLength="15"> </asp:TextBox>
                                </div>
                            </div>
                        </div>
                     <div class="row g-3">
                            <div class="col-md-2">
                                <div class="form-group">
                                    <span class="details">BAIRRO:</span>
                                    <asp:TextBox ID="txtBaiCli" runat="server" ForeColor="Blue" CssClass="form-control" placeholder="" MaxLength="60"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="form-group">
                                    <span class="details">MUNICIPIO:</span>
                                    <asp:TextBox ID="txtCidCli" runat="server" ForeColor="Blue" CssClass="form-control" placeholder="" MaxLength="60"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-1">
                                <div class="form-group">
                                    <span class="details">UF:</span>
                                    <asp:TextBox ID="txtEstCli" Style="text-align: center" runat="server" ForeColor="Blue" CssClass="form-control" placeholder="" MaxLength="2"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-2">
                                <div class="form-group">
                                    <span class="details">LATITUDE:</span>
                                    <asp:TextBox ID="latitude" Style="text-align: center" runat="server" ForeColor="Blue" CssClass="form-control" placeholder="" MaxLength="40"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-2">
                                <div class="form-group">
                                    <span class="details">LONGITUDE:</span>
                                    <asp:TextBox ID="longitude" Style="text-align: center" runat="server" ForeColor="Blue" CssClass="form-control" placeholder="" MaxLength="40"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-1">
                                <div class="form-group">
                                    <span class="details">RAIO:</span>
                                    <asp:TextBox ID="txtRaio" value="100" Style="text-align: center" runat="server" ForeColor="Blue" CssClass="form-control" placeholder="" min="1" max="2000"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-1">
                                <br />
                                <button type="button" class="btn btn-outline-warning">Pesquisar</button>
                            </div>
                        </div>
                     <div class="row g-3">
                            <div class="col-md-2">
                                <div class="form-group">
                                    <span class="details">CADASTRADO EM:</span>
                                    <asp:Label ID="lblDtCadastro" runat="server" ForeColor="Blue" CssClass="form-control" placeholder="" maxlength="20"></asp:Label>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group">
                                    <span class="details">POR:</span>
                                    <asp:TextBox ID="txtUsuCadastro" runat="server" ForeColor="Blue" CssClass="form-control" placeholder="" MaxLength="60"></asp:TextBox>
                                </div>
                            </div>

                            <div class="col-md-2">
                                <div class="form-group">
                                    <span class="details">ÚLTIMA ATUALIZAÇÃO:</span>
                                    <asp:Label ID="lblDtAlteracao" runat="server" ForeColor="Blue" CssClass="form-control" placeholder="" maxlength="20"></asp:Label>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group">
                                    <span class="details">POR:</span>
                                    <asp:TextBox ID="txtUsuAlteracao" runat="server" ForeColor="Blue" CssClass="form-control" placeholder=""></asp:TextBox>
                                </div>
                            </div>
                        </div>
                     <div class="row g-3">
                            <div class="col-md-1">
                                <asp:Button ID="btnMapa" runat="server" CssClass="btn btn-outline-info btn-lg" Text="Mapa" />
                            </div>
                            <div class="col-md-1">
                                <asp:Button ID="btnAlterar" runat="server" CssClass="btn btn-outline-success btn-lg" Text="Atualizar" OnClick="btnAlterar_Click" />
                            </div>
                            <div class="col-md-1">
                                <a href="ConsultaClientes.aspx" class="btn btn-outline-danger btn-lg">Cancelar               
                                </a>
                            </div>
                        </div>                     
                </div>                              
            </div>
        </section>
    </div>
    <footer class="main-footer">
       <div class="float-right d-none d-sm-block">
         <b>Version</b> 2.1.0
       </div>
       <strong>Copyright &copy; 2021-2025 Capit Logística.</strong> Todos os direitos reservados.
    </footer>    
    <!-- jQuery -->
    <script src="../../plugins/jquery/jquery.min.js"></script>
    <!-- Bootstrap 4 -->
    <script src="../../plugins/bootstrap/js/bootstrap.bundle.min.js"></script>
    <!-- bs-custom-file-input -->
    <script src="../../plugins/bs-custom-file-input/bs-custom-file-input.min.js"></script>
    <!-- AdminLTE App -->
    <script src="../../dist/js/adminlte.min.js"></script>
    <!-- AdminLTE for demo purposes -->
    <script src="../../dist/js/demo.js"></script>
    <!-- Page specific script -->
    <script>
        $(function () {
            bsCustomFileInput.init();
        });
    </script>

</asp:Content>
