<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Frm_CadVeiculos.aspx.cs" Inherits="NewCapit.Frm_CadVeiculos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div class="container mt-5">
        <!-- cabeçalho -->
        <div class="d-sm-flex align-items-center justify-content-between mb-4">
            <h3 class="h3 mb-2 text-gray-800"><i class="fas fa-shipping-fast"></i>VEÍCULO </h3>
            <h3>NOVO CADASTRO</h3>
        </div>
        <hr />
        <!-- linha 1 -->
        <div class="row g-3">
            <div class="col-md-1">
                <div class="form-group">
                    <span class="details">FROTA:</span>
                    <asp:TextBox ID="txtCodVei" Style="text-align: center" runat="server" ForeColor="Blue" CssClass="form-control" placeholder="" MaxLength="9"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-1">
                <br />
                <asp:Button ID="btnVeiculo" runat="server" Text="Pesquisar" CssClass="btn btn-outline-warning" OnClick="btnVeiculo_Click" />
            </div>
            <div class="col-md-2">
                <div class="form_group">
                    <span class="details">TIPO DE VEÍCULO:</span>
                    <asp:DropDownList ID="cboTipo" runat="server" ForeColor="Blue" CssClass="form-control" Width="250px">
                        <asp:ListItem Value="" Text=""></asp:ListItem>
                        <asp:ListItem Value="BITREM" Text="BITREM"></asp:ListItem>
                        <asp:ListItem Value="BITRUCK" Text="BITRUCK"></asp:ListItem>
                        <asp:ListItem Value="CAVALO SIMPLES" Text="CAVALO SIMPLES"></asp:ListItem>
                        <asp:ListItem Value="CAVALO TRUCADO" Text="CAVALO TRUCADO"></asp:ListItem>
                        <asp:ListItem Value="CAVALO 4 EIXOS" Text="CAVALO 4 EIXOS"></asp:ListItem>
                        <asp:ListItem Value="TOCO" Text="TOCO"></asp:ListItem>
                        <asp:ListItem Value="TRUCK" Text="TRUCK"></asp:ListItem>
                        <asp:ListItem Value="VEICULO 3/4" Text="VEICULO 3/4"></asp:ListItem>
                        <asp:ListItem Value="OUTROS" Text="OUTROS"></asp:ListItem>
                    </asp:DropDownList><br />
                </div>
            </div>
            <div class="col-md-1">
                <div class="form-group">
                    <span class="details">PLACA:</span>
                    <asp:TextBox ID="txtPlaca" Style="text-align: center" runat="server" ForeColor="Blue" CssClass="form-control" placeholder="" MaxLength="8"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-1">
                <div class="form-group">
                    <span class="details">UF:</span>
                    <asp:DropDownList ID="ddlUfPlaca" runat="server" ForeColor="Blue" class="js-example-basic-single"
 Width="100px">
                        <asp:ListItem Value=""></asp:ListItem>
                        <asp:ListItem Value="AC">AC</asp:ListItem>
                        <asp:ListItem Value="AL">AL</asp:ListItem>
                        <asp:ListItem Value="AP">AP</asp:ListItem>
                        <asp:ListItem Value="AM">AM</asp:ListItem>
                        <asp:ListItem Value="BA">BA</asp:ListItem>
                        <asp:ListItem Value="CE">CE</asp:ListItem>
                        <asp:ListItem Value="DF">DF</asp:ListItem>
                        <asp:ListItem Value="ES">ES</asp:ListItem>
                        <asp:ListItem Value="GO">GO</asp:ListItem>
                        <asp:ListItem Value="MA">MA</asp:ListItem>
                        <asp:ListItem Value="MT">MT</asp:ListItem>
                        <asp:ListItem Value="MS">MS</asp:ListItem>
                        <asp:ListItem Value="MG">MG</asp:ListItem>
                        <asp:ListItem Value="PA">PA</asp:ListItem>
                        <asp:ListItem Value="PB">PB</asp:ListItem>
                        <asp:ListItem Value="PR">PR</asp:ListItem>
                        <asp:ListItem Value="PE">PE</asp:ListItem>
                        <asp:ListItem Value="PI">PI</asp:ListItem>
                        <asp:ListItem Value="RJ">RJ</asp:ListItem>
                        <asp:ListItem Value="RN">RN</asp:ListItem>
                        <asp:ListItem Value="RS">RS</asp:ListItem>
                        <asp:ListItem Value="RO">RO</asp:ListItem>
                        <asp:ListItem Value="RR">RR</asp:ListItem>
                        <asp:ListItem Value="SC">SC</asp:ListItem>
                        <asp:ListItem Value="SP">SP</asp:ListItem>
                        <asp:ListItem Value="SE">SE</asp:ListItem>
                        <asp:ListItem Value="TO">TO</asp:ListItem>
                        <asp:ListItem Value="EX">EX</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-md-4">
                <div class="form_group">
                    <span class="details">MUNICIPIO:</span>
                    <asp:TextBox ID="txtCidPlaca" runat="server" Style="text-align: left" ForeColor="Blue" CssClass="form-control" placeholder="" MaxLength="40"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-2">
                <div class="form_group">
                    <span class="details">FILIAL:</span>
                    <asp:DropDownList ID="cbFiliais" name="nomeFiliais" runat="server" ForeColor="Blue" CssClass="form-control"></asp:DropDownList>
                </div>
            </div>            
        </div>
        <!-- linha 2 -->
        <div class="row g-3">
            <div class="col-md-1">
                 <div class="form_group">
                    <span class="details">FAB/MOD.:</span>
                    <asp:TextBox ID="txtAno" runat="server" data-mask="0000/0000" Style="text-align: center" ForeColor="Blue" CssClass="form-control" placeholder="0000/0000" MaxLength="9"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-1">
                  <div class="form_group">
                     <span class="details">AQUISIÇÃO:</span>
                     <asp:TextBox ID="txtDataAquisicao" runat="server" data-mask="00/00/0000" Style="text-align: center" ForeColor="Blue" CssClass="form-control" placeholder="00/00/0000" MaxLength="10" Width="130px"></asp:TextBox>
                 </div>
            </div>
            <div class="col-md-2">
                <div class="form_group">
                    <span class="details">RENAVAM:</span>
                    <asp:TextBox ID="txtRenavam" runat="server" ForeColor="Blue" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="25"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-2">
                <div class="form_group">
                    <span class="details">CHASSI:</span>
                    <asp:TextBox ID="txtChassi" runat="server" ForeColor="Blue" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="30"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-1">
                <div class="form_group">
                    <span class="details">LICENCIAMENTO:</span>
                    <asp:TextBox ID="txtLicenciamento" runat="server" ForeColor="Blue" CssClass="form-control" placeholder="00/00/0000" MaxLength="10" data-mask="00/00/0000" Width="130px" Style="text-align: center"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-1">
                <div class="form_group">
                    <span class="details">TACÓGRAFO:</span>
                    <asp:DropDownList ID="ddlTacografo" runat="server" ForeColor="Blue" CssClass="form-control" width="130px">
                        <asp:ListItem Value="" Text=""></asp:ListItem>
                        <asp:ListItem Value="DIARIO" Text="DIARIO"></asp:ListItem>
                        <asp:ListItem Value="SEMANAL" Text="SEMANAL"></asp:ListItem>
                        <asp:ListItem Value="FITA" Text="FITA"></asp:ListItem>    
                        <asp:ListItem Value="OUTROS" Text="OUTROS"></asp:ListItem>
                    </asp:DropDownList><br />
                </div>
            </div>
            <div class="col-md-1">
                <div class="form_group">
                    <span class="details">MODELO:</span>
                    <asp:DropDownList ID="ddlModeloTacografo" runat="server" ForeColor="Blue" CssClass="form-control" Width="135px">
                        <asp:ListItem Value="" Text=""></asp:ListItem>
                        <asp:ListItem Value="MECANICO" Text="MECANICO"></asp:ListItem>
                        <asp:ListItem Value="ELETRONICO" Text="ELETRONICO"></asp:ListItem>                          
                        <asp:ListItem Value="OUTROS" Text="OUTROS"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-md-1">
                <div class="form_group">
                    <span class="details">COMPRIMENTO:</span>
                    <asp:TextBox ID="txtComprimento" runat="server" ForeColor="Blue" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="10"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-1">
                <div class="form_group">
                    <span class="details">LARGURA:</span>
                    <asp:TextBox ID="txtLargura" runat="server" ForeColor="Blue" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="10" ></asp:TextBox>
                </div>
            </div>
            <div class="col-md-1">
                <div class="form_group">
                    <span class="details">ALTURA:</span>
                    <asp:TextBox ID="txtAltura" runat="server" ForeColor="Blue" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="10" ></asp:TextBox>
                </div>
            </div>
        </div>
       
        <!-- linha 3 -->
        <div class="row g-3">
            <div class="col-md-5">
                <div class="form_group">
                    <span class="details">MARCA:</span>
                    <asp:DropDownList ID="ddlMarca" name="nomeFiliais" runat="server" ForeColor="Blue" CssClass="form-control"></asp:DropDownList>
                </div>
            </div>
            <div class="col-md-5">
                <div class="form-group">
                    <span class="details">MODELO:</span>
                    <asp:TextBox ID="txtModelo" runat="server" ForeColor="Blue" CssClass="form-control" placeholder="" MaxLength="40"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-2">
                <div class="form_group">
                    <span class="details">COR:</span>
                    <asp:DropDownList ID="ddlCor" name="nomeFiliais" runat="server" ForeColor="Blue" CssClass="form-control"></asp:DropDownList>
                </div>
            </div>
        </div>
        <!-- linha 4 -->
        <div class="row g-3">
            <div class="col-md-2">
                <div class="form-group">
                    <span class="">MONITORAMENTO:</span>
                    <asp:DropDownList ID="ddlMonitoramento" runat="server" ForeColor="Blue" CssClass="form-control">
                        <asp:ListItem Value="" Text=""></asp:ListItem>
                        <asp:ListItem Value="MONITORADO" Text="MONITORADO"></asp:ListItem>
                        <asp:ListItem Value="RASTREADO" Text="RASTREADO"></asp:ListItem>
                        <asp:ListItem Value="TELEMONITORADO" Text="TELEMONITORADO"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-md-1">
                <div class="form-group">
                    <span class="details">CÓDIGO:</span>
                    <asp:TextBox ID="txtCodRastreador" runat="server" Style="text-align: center" ForeColor="Blue" CssClass="form-control" placeholder="" MaxLength="4"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-4">
                <div class="form_group">
                    <span class="details">TECNOLOGIA/RASTREADOR:</span>
                    <asp:DropDownList ID="ddlTecnologia" name="tecnologia" runat="server" ForeColor="Blue" CssClass="form-control" OnSelectedIndexChanged="ddlTecnologia_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                </div>
            </div>
            <div class="col-md-2">
                <div class="form-group">
                    <span class="details">ID:</span>
                    <asp:TextBox ID="txtId" runat="server" ForeColor="Blue" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="11"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-3">
                <div class="form-group">
                    <span class="">COMUNICAÇÃO:</span>
                    <asp:DropDownList ID="ddlComunicacao" runat="server" ForeColor="Blue" CssClass="form-control">
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
                </div>
            </div>
        </div>
        <!-- linha 5 -->
        <div class="row g-3">
            <div class="col-md-2">
                <div class="form-group">
                    <span class="">TIPO:</span>
                    <asp:DropDownList ID="ddlTipo" runat="server" ForeColor="Blue" CssClass="form-control">
                        <asp:ListItem Value="" Text=""></asp:ListItem>
                        <asp:ListItem Value="AGREGADO" Text="AGREGADO"></asp:ListItem>
                        <asp:ListItem Value="FROTA" Text="FROTA"></asp:ListItem>
                        <asp:ListItem Value="TERCEIRO" Text="TERCEIRO"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-md-1">
                <div class="form-group">
                    <span class="details">CÓDIGO:</span>
                    <asp:TextBox ID="txtCodTra" runat="server" ForeColor="Blue" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="4" AutoPostBack="true"></asp:TextBox>
                </div>
            </div>

            <div class="col-md-6">
                <div class="form_group">
                    <span class="details">PROPRIETÁRIO/TRANSPORTADORA:</span>
                    <asp:DropDownList ID="ddlAgregados" class="js-example-basic-single" name="nomeProprietario" runat="server" ForeColor="Blue" Width="750px"  OnSelectedIndexChanged="ddlAgregados_SelectedIndexChanged" AutoPostBack="true" ></asp:DropDownList>
                </div>
            </div>


            <div class="col-md-3">
                <div class="form-group">
                    <span class="details">ANTT/RNTRC:</span>
                    <asp:TextBox ID="txtAntt" runat="server" Style="text-align: center" ForeColor="Blue" CssClass="form-control" placeholder="" MaxLength="11"></asp:TextBox>
                </div>
            </div>
        </div>
        <!-- linha 6 -->
        <div class="row g-3">
            <div class="col-md-2">
                <div class="form-group">
                    <span class="details">CADASTRADO EM:</span>
                    <asp:Label ID="lblDtCadastro" runat="server" Style="text-align: center" ForeColor="Blue" CssClass="form-control" placeholder="" maxlength="20"></asp:Label>
                </div>
            </div>
            <div class="col-md-5">
                <div class="form-group">
                    <span class="details">POR:</span>
                    <asp:TextBox ID="txtUsuCadastro" runat="server" ForeColor="Blue" CssClass="form-control" placeholder="" MaxLength="60"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-2">
                <div class="form-group">
                    <span class="details">CÓDIGO:</span>
                    <asp:TextBox ID="txtCodigo" runat="server" Style="text-align: center" ForeColor="Blue" CssClass="form-control" placeholder="" maxlength="10"></asp:TextBox>
                </div>
            </div>
                        <div class="col-md-3">
                <div class="form-group">
                    <span class="details">PATRIMÔNIO:</span>
                    <asp:TextBox ID="txtControlePatrimonio" runat="server" Style="text-align: center" ForeColor="Blue" CssClass="form-control" placeholder="" maxlength="20"></asp:TextBox>
                </div>
            </div>
        </div>
        <!-- linha 7 -->
        <div class="row g-3">
            <div class="col-md-1">

                <asp:Button ID="btnSalvar1" CssClass="btn btn-outline-success  btn-lg" runat="server" OnClick="btnSalvar1_Click" Text="Cadastrar" />
            </div>
            <div class="col-md-2">
                &nbsp;&nbsp;&nbsp;
                <a href="ConsultaVeiculos.aspx" class="btn btn-outline-danger btn-lg"> Cancelar               
                </a>
            </div>
        </div>

    </div>    
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.5.0/jquery.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery.mask/1.14.16/jquery.mask.js"></script>

    <link href="assets/plugins/global/plugins.bundle.css" rel="stylesheet" type="text/css"/>
    <script src="assets/plugins/global/plugins.bundle.js"></script>


    <!--
    <link href="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/css/select2.min.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap/5.3.3/js/bootstrap.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/js/select2.min.js"></script>
    -->
    <script>
        $(document).ready(function () {
            $('.js-example-basic-single').select2();
        });
    </script>
    
</asp:Content>
