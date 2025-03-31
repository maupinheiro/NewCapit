<%@ Page Title="" Language="C#" MasterPageFile="Main.Master" AutoEventWireup="true" CodeBehind="Frm_AltVeiculos.aspx.cs" Inherits="NewCapit.Frm_AltVeiculos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- Google Font: Source Sans Pro -->
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,400i,700&display=fallback">
    <!-- Font Awesome -->
    <link rel="stylesheet" href="../../plugins/fontawesome-free/css/all.min.css">
    <!-- daterange picker -->
    <link rel="stylesheet" href="../../plugins/daterangepicker/daterangepicker.css">
    <!-- iCheck for checkboxes and radio inputs -->
    <link rel="stylesheet" href="../../plugins/icheck-bootstrap/icheck-bootstrap.min.css">
    <!-- Bootstrap Color Picker -->
    <link rel="stylesheet" href="../../plugins/bootstrap-colorpicker/css/bootstrap-colorpicker.min.css">
    <!-- Tempusdominus Bootstrap 4 -->
    <link rel="stylesheet" href="../../plugins/tempusdominus-bootstrap-4/css/tempusdominus-bootstrap-4.min.css">
    <!-- Select2 -->
    <link rel="stylesheet" href="../../plugins/select2/css/select2.min.css">
    <link rel="stylesheet" href="../../plugins/select2-bootstrap4-theme/select2-bootstrap4.min.css">
    <!-- Bootstrap4 Duallistbox -->
    <link rel="stylesheet" href="../../plugins/bootstrap4-duallistbox/bootstrap-duallistbox.min.css">
    <!-- BS Stepper -->
    <link rel="stylesheet" href="../../plugins/bs-stepper/css/bs-stepper.min.css">
    <!-- dropzonejs -->
    <link rel="stylesheet" href="../../plugins/dropzone/min/dropzone.min.css">
    <!-- Theme style -->
    <link rel="stylesheet" href="../../dist/css/adminlte.min.css">
    <script language="javascript">
        function ConfirmMessage() {
            var selectedvalue = confirm("Esse motorista já possui vinculo com um veículo. Deseja desvincular o veículo?");
            if (selectedvalue) {
                document.getElementById('<%=txtconformmessageValue.ClientID %>').value = "Yes";
            } else {
                document.getElementById('<%=txtconformmessageValue.ClientID %>').value = "No";
            }
        }
    </script>
    <div class="content-wrapper">
        <section class="content">
            <div class="container-fluid">
                <br />
                <div class="card card-warning">
                    <div class="card-header">
                        <h3 class="card-title"><i class="fas fa-shipping-fast"></i>VEÍCULOS - ATUALIZAÇÃO DE DADOS</h3>
                    </div>
                </div>
            </div>
            <div class="card-header">
                <!-- Linha 1 do formulario -->
                <div class="row g-3">
                    <div class="col-md-1">
                        <div class="form-group">
                            <span class="details">FROTA:</span>
                            <asp:TextBox ID="txtCodVei" Style="text-align: center" runat="server" CssClass="form-control" placeholder="" MaxLength="9"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <div class="form_group">
                            <span class="details">TIPO DE VEÍCULO:</span>
                            <asp:DropDownList ID="cboTipo" runat="server" CssClass="form-control" Width="250px">
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
                            <asp:TextBox ID="txtPlaca" Style="text-align: center" runat="server" CssClass="form-control" placeholder="" MaxLength="8"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-1">
                        <div class="form-group">
                            <span class="details">UF:</span>
                            <asp:DropDownList ID="ddlUfPlaca" runat="server" class="form-control select2"
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
                    <div class="col-md-3">
                        <div class="form_group">
                            <span class="details">MUNICIPIO:</span>
                            <asp:TextBox ID="txtCidPlaca" runat="server" Style="text-align: left" CssClass="form-control" placeholder="" MaxLength="40"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <div class="form_group">
                            <span class="details">FILIAL:</span>
                            <asp:DropDownList ID="cbFiliais" name="nomeFiliais" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-1">
                        <div class="form_group">
                            <span class="details">CADASTRO:</span>
                            <asp:TextBox ID="txtCadastro" runat="server" class="form-control" data-inputmask-inputformat="dd/mm/yyyy" MaxLength="10" Style="text-align: center" Width="130px"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-1">
                        <div class="form-group">
                            <span class="">STATUS:</span>
                            <asp:DropDownList ID="status" runat="server" CssClass="form-control">
                                <asp:ListItem Value="ATIVO" Text="ATIVO"></asp:ListItem>
                                <asp:ListItem Value="INATIVO" Text="INATIVO"></asp:ListItem>
                            </asp:DropDownList></>
                        </div>
                    </div>

                </div>
                <!-- Linha 2 do formulario -->
                <div class="row g-3">
                    <div class="col-md-1">
                        <div class="form_group">
                            <span class="details">FAB/MOD.:</span>
                            <asp:TextBox ID="txtAno" runat="server" data-mask="0000/0000" Style="text-align: center" CssClass="form-control" placeholder="0000/0000" MaxLength="9"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-1">
                        <div class="form_group">
                            <span class="details">AQUISIÇÃO:</span>
                            <asp:TextBox ID="txtDataAquisicao" runat="server" data-mask="00/00/0000" Style="text-align: center" CssClass="form-control" placeholder="00/00/0000" MaxLength="10" Width="130px"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <div class="form_group">
                            <span class="details">RENAVAM:</span>
                            <asp:TextBox ID="txtRenavam" runat="server" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="25"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <div class="form_group">
                            <span class="details">CHASSI:</span>
                            <asp:TextBox ID="txtChassi" runat="server" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="30"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-1">
                        <div class="form_group">
                            <span class="details">LICENCIAMENTO:</span>
                            <asp:TextBox ID="txtLicenciamento" runat="server" CssClass="form-control" placeholder="00/00/0000" MaxLength="10" data-mask="00/00/0000" Style="text-align: center"></asp:TextBox>
                        </div>
                    </div>
                    <!-- LINHA -->
                    <div class="row g-3">
                        <div class="col-md-2">
                            <div class="form_group">
                                <span class="details">TACÓGRAFO:</span>
                                <asp:DropDownList ID="ddlTacografo" runat="server" CssClass="form-control" >
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="DIARIO" Text="DIARIO"></asp:ListItem>
                                    <asp:ListItem Value="SEMANAL" Text="SEMANAL"></asp:ListItem>
                                    <asp:ListItem Value="FITA" Text="FITA"></asp:ListItem>
                                    <asp:ListItem Value="OUTROS" Text="OUTROS"></asp:ListItem>
                                </asp:DropDownList><br />
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form_group">
                                <span class="details">MODELO:</span>
                                <asp:DropDownList ID="ddlModeloTacografo" runat="server" CssClass="form-control" >
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="MECANICO" Text="MECANICO"></asp:ListItem>
                                    <asp:ListItem Value="ELETRONICO" Text="ELETRONICO"></asp:ListItem>
                                    <asp:ListItem Value="OUTROS" Text="OUTROS"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form_group">
                                <span class="details">COMPRIMENTO:</span>
                                <asp:TextBox ID="txtComprimento" runat="server" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="10"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form_group">
                                <span class="details">LARGURA:</span>
                                <asp:TextBox ID="txtLargura" runat="server" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="10"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form_group">
                                <span class="details">ALTURA:</span>
                                <asp:TextBox ID="txtAltura" runat="server" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="10"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                </div>
                <!-- Linha 3 do formulario -->
                <div class="row g-3">
                    <div class="col-md-3">
                        <div class="form_group">
                            <span class="details">MARCA:</span>
                            <asp:DropDownList ID="ddlMarca" name="nomeFiliais" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <div class="form-group">
                            <span class="details">MODELO:</span>
                            <asp:TextBox ID="txtModelo" runat="server" CssClass="form-control" placeholder="" MaxLength="40"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <div class="form_group">
                            <span class="details">COR:</span>
                            <asp:DropDownList ID="ddlCor" name="nomeFiliais" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <div class="form_group">
                            <span class="details">PROT.CET:</span>
                            <asp:TextBox ID="txtProtocolo" runat="server" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="10"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-1">
                        <div class="form_group">
                            <span class="details">VALIDADE:</span>
                            <asp:TextBox ID="txtValCET" runat="server" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="10" data-mask="00/00/0000"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-1">
                        <div class="form_group">
                            <span class="details">OPACIDADE:</span>
                            <asp:TextBox ID="txtOpacidade" runat="server" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="10" data-mask="00/00/0000"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <!-- Linha 4 do formulario -->
                <div class="row g-3">
                    <div class="col-md-2">
                        <div class="form-group">
                            <span class="">MONITORAMENTO:</span>
                            <asp:DropDownList ID="ddlMonitoramento" runat="server" CssClass="form-control">
                                <asp:ListItem Value="" Text="SELECIONE"></asp:ListItem>
                                <asp:ListItem Value="MONITORADO" Text="MONITORADO"></asp:ListItem>
                                <asp:ListItem Value="RASTREADO" Text="RASTREADO"></asp:ListItem>
                                <asp:ListItem Value="TELEMONITORADO" Text="TELEMONITORADO"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-1">
                        <div class="form-group">
                            <span class="details">CÓDIGO:</span>
                            <asp:TextBox ID="txtCodRastreador" runat="server" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="4"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form_group">
                            <span class="details">TECNOLOGIA/RASTREADOR:</span>
                            <asp:DropDownList ID="ddlTecnologia" name="tecnologia" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlTecnologia_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <div class="form-group">
                            <span class="details">ID:</span>
                            <asp:TextBox ID="txtId" runat="server" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="11"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <div class="form-group">
                            <span class="">COMUNICAÇÃO:</span>
                            <asp:DropDownList ID="ddlComunicacao" runat="server" CssClass="form-control">
                                <asp:ListItem Value="" Text="Selecione..."></asp:ListItem>
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
                <!-- linha 5 do formulario -->
                <div class="row g-3">
                    <div class="col-md-2">
                        <div class="form-group">
                            <span class="">TIPO:</span>
                            <asp:DropDownList ID="DropDownList1" runat="server" CssClass="form-control">
                                <asp:ListItem Value="" Text="Selecione..."></asp:ListItem>
                                <asp:ListItem Value="AGREGADO" Text="AGREGADO"></asp:ListItem>
                                <asp:ListItem Value="FROTA" Text="FROTA"></asp:ListItem>
                                <asp:ListItem Value="TERCEIRO" Text="TERCEIRO"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-1">
                        <div class="form-group">
                            <span class="details">CÓDIGO:</span>
                            <asp:TextBox ID="txtCodTra" runat="server" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="4" AutoPostBack="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <div class="form_group">
                            <span class="details">PROPRIETÁRIO/TRANSPORTADORA:</span>
                            <asp:DropDownList ID="ddlAgregados" class="form-control select2" name="nomeProprietario" runat="server" OnSelectedIndexChanged="ddlAgregados_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-1">
                        <div class="form-group">
                            <span class="details">ANTT/RNTRC:</span>
                            <asp:TextBox ID="txtAntt" runat="server" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="11"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-1">
                        <div class="form-group">
                            <span class="details">CÓDIGO:</span>
                            <asp:TextBox ID="txtCodMot" runat="server" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="9"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form_group">
                            <span class="details">MOTORISTA:</span>
                            <asp:DropDownList ID="ddlMotorista" runat="server" class="form-control select2" OnSelectedIndexChanged="ddlMotorista_SelectedIndexChanged" Width="520px" AutoPostBack="true"></asp:DropDownList>

                        </div>
                        <asp:HiddenField ID="txtconformmessageValue" runat="server" />
                    </div>

                </div>
                <!-- linha 6 do formulario -->
                <div class="row g-3">
                    <div class="col-md-1">
                        <div class="form-group">
                            <span class="details">EIXOS:</span>
                            <asp:TextBox ID="txtEixos" runat="server" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="6"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-1">
                        <div class="form-group">
                            <span class="details">LOTAÇÃO:</span>
                            <asp:TextBox ID="txtCap" runat="server" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="6"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-md-1">
                        <div class="form-group">
                            <span class="details">TARA:</span>
                            <asp:TextBox ID="txtTara" runat="server" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="6"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-1">
                        <div class="form-group">
                            <span class="details">TOL. %:</span>
                            <asp:TextBox ID="txtTolerancia" runat="server" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="6"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-1">
                        <div class="form-group">
                            <span class="details">CARGA LIQ.:</span>
                            <asp:TextBox ID="txtPBT" runat="server" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="6"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-1">
                        <div class="form-group">
                            <span class="details">TIPO SEGURO:</span>
                            <asp:TextBox ID="txtTipoSeguro" runat="server" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="30" disabled></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <div class="form-group">
                            <span class="details">COMPANHIA:</span>
                            <asp:TextBox ID="txtSeguradora" runat="server" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="50" disabled></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-1">
                        <div class="form-group">
                            <span class="details">APOLICE:</span>
                            <asp:TextBox ID="txtApolice" runat="server" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="30" disabled></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-1">
                        <div class="form-group">
                            <span class="details">VALIDADE:</span>
                            <asp:TextBox ID="txtValidadeApolice" runat="server" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="10" disabled></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-1">
                        <div class="form-group">
                            <span class="details">FRANQUIA:</span>
                            <asp:TextBox ID="txtValorFranquia" runat="server" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="11" disabled></asp:TextBox>
                        </div>
                    </div>

                </div>
                <!-- Linha 7 do formulario -->
                <div class="row g-3">
                    <div class="col-md-2">
                        <div class="form-group">
                            <span class="">TIPO:</span>
                            <asp:DropDownList ID="ddlTipo" runat="server" CssClass="form-control">
                                <asp:ListItem Value="" Text="Selecione..."></asp:ListItem>
                                <asp:ListItem Value="AGREGADO" Text="AGREGADO"></asp:ListItem>
                                <asp:ListItem Value="FROTA" Text="FROTA"></asp:ListItem>
                                <asp:ListItem Value="TERCEIRO" Text="TERCEIRO"></asp:ListItem>
                            </asp:DropDownList></>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <div class="form-group">
                            <span class="">CARRETA:</span>
                            <asp:DropDownList ID="ddlCarreta" runat="server" CssClass="form-control">
                                <asp:ListItem Value="" Text="Selecione..."></asp:ListItem>
                                <asp:ListItem Value="PROPRIA" Text="PROPRIA"></asp:ListItem>
                                <asp:ListItem Value="TRANSNOVAG" Text="TRANSNOVAG"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>

                    <div class="col-md-1">
                        <div class="form-group">
                            <span id="numeroReb1" class="details">REBOQUE 1:</span>
                            <asp:TextBox ID="txtReb1" runat="server" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="8"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-1">
                        <div class="form-group">
                            <span id="numeroReb2" class="details">REBOQUE 2:</span>
                            <asp:TextBox ID="txtReb2" runat="server" CssClass="form-control" Style="text-align: center" placeholder="" MaxLength="8"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <span class="">COMPOSIÇÃO:</span>
                            <asp:DropDownList ID="ddlComposicao" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlComposicao_SelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Value="" Text="SELECIONE"></asp:ListItem>
                                <asp:ListItem Value="CAVALO SIMPLES COM CARRETA VANDERLEIA ABERTA" Text="SELECIONE">CAVALO SIMPLES COM CARRETA VANDERLEIA ABERTA</asp:ListItem>
                                <asp:ListItem Value="CAVALO SIMPLES COM CARRETA SIMPLES TOTAL SIDER" Text="">CAVALO SIMPLES COM CARRETA SIMPLES TOTAL SIDER</asp:ListItem>
                                <asp:ListItem Value="CAVALO SIMPLES COM CARRETA SIMPLES(LS) ABERTA" Text="">CAVALO SIMPLES COM CARRETA SIMPLES(LS) ABERTA</asp:ListItem>
                                <asp:ListItem Value="CAVALO SIMPLES COM CARRETA VANDERLEIA TOTAL SIDER" Text="">CAVALO SIMPLES COM CARRETA VANDERLEIA TOTAL SIDER</asp:ListItem>
                                <asp:ListItem Value="CAVALO TRUCADO COM CARRETA VANDERLEIA ABERTA" Text="">CAVALO TRUCADO COM CARRETA VANDERLEIA ABERTA</asp:ListItem>
                                <asp:ListItem Value="CAVALO TRUCADO COM CARRETA SIMPLES TOTAL SIDER" Text="">CAVALO TRUCADO COM CARRETA SIMPLES TOTAL SIDER</asp:ListItem>
                                <asp:ListItem Value="CAVALO TRUCADO COM CARRETA SIMPLES(LS) ABERTA" Text="">CAVALO TRUCADO COM CARRETA SIMPLES(LS) ABERTA</asp:ListItem>
                                <asp:ListItem Value="CAVALO TRUCADO COM CARRETA VANDERLEIA TOTAL SIDER" Text="">CAVALO TRUCADO COM CARRETA VANDERLEIA TOTAL SIDER</asp:ListItem>
                                <asp:ListItem Value="TRUCK" Text="">TRUCK</asp:ListItem>
                                <asp:ListItem Value="BITRUCK" Text="">BITRUCK</asp:ListItem>
                                <asp:ListItem Value="BITREM" Text="">BITREM</asp:ListItem>
                                <asp:ListItem Value="TOCO" Text="">TOCO</asp:ListItem>
                                <asp:ListItem Value="VEICULO 3/4" Text="">VEICULO 3/4</asp:ListItem>
                                <asp:ListItem Value="CAVALO SIMPLES COM PRANCHA" Text="">CAVALO SIMPLES COM PRANCHA</asp:ListItem>
                                <asp:ListItem Value="CAVALO TRUCADO COM PRANCHA" Text="">CAVALO TRUCADO COM PRANCHA</asp:ListItem>
                                <asp:ListItem Value="CAVALO TRUCADO COM CARRETA LS TOTAL SIDER PRANCHA" Text="">CAVALO TRUCADO COM CARRETA LS TOTAL SIDER LISA</asp:ListItem>
                                <asp:ListItem Value="CAVALO TRUCADO COM CARRETA LS TOTAL SIDER PRANCHA" Text="">CAVALO SIMPLES COM CARRETA LS TOTAL SIDER LISA</asp:ListItem>
                            </asp:DropDownList></>
                        </div>
                    </div>

                </div>
                <!-- Linha 8 do formulario -->
                <div class="row g-3">
                    <div class="col-md-2">
                        <div class="form-group">
                            <span class="details">CADASTRADO EM:</span>
                            <asp:Label ID="txtDtCadastro" runat="server" Style="text-align: center" CssClass="form-control" placeholder="" maxlength="20"></asp:Label>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group">
                            <span class="details">POR:</span>
                            <asp:TextBox ID="txtCadastradoPor" runat="server" CssClass="form-control" placeholder="" MaxLength="60"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <div class="form-group">
                            <span class="details">ATUALIZADO EM:</span>
                            <asp:TextBox ID="txtDtAlteracao" runat="server" Style="text-align: center" ForeColor="Blue" CssClass="form-control" placeholder="" MaxLength="10"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group">
                            <span class="details">POR:</span>
                            <asp:TextBox ID="txtAlteradoPor" runat="server" Style="text-align: left" ForeColor="Blue" CssClass="form-control" placeholder="" MaxLength="20"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <!-- linha 9 -->
                <div class="row g-3">
                    <div class="col-md-1">

                        <asp:Button ID="btnSalvar1" CssClass="btn btn-outline-success  btn-lg" runat="server" Text="Atualiza" OnClick="btnSalvar1_Click" />
                    </div>
                    <div class="col-md-1">
                        <a href="ConsultaVeiculos.aspx" class="btn btn-outline-danger btn-lg">Cancelar               
                        </a>
                    </div>
                    <div class="col-md-1">
                        <button type="button" class="btn btn-outline-info  btn-lg">Mapa </button>
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
    <!-- Select2 -->
    <script src="../../plugins/select2/js/select2.full.min.js"></script>
    <!-- Bootstrap4 Duallistbox -->
    <script src="../../plugins/bootstrap4-duallistbox/jquery.bootstrap-duallistbox.min.js"></script>
    <!-- InputMask -->
    <script src="../../plugins/moment/moment.min.js"></script>
    <script src="../../plugins/inputmask/jquery.inputmask.min.js"></script>
    <!-- date-range-picker -->
    <script src="../../plugins/daterangepicker/daterangepicker.js"></script>
    <!-- bootstrap color picker -->
    <script src="../../plugins/bootstrap-colorpicker/js/bootstrap-colorpicker.min.js"></script>
    <!-- Tempusdominus Bootstrap 4 -->
    <script src="../../plugins/tempusdominus-bootstrap-4/js/tempusdominus-bootstrap-4.min.js"></script>
    <!-- Bootstrap Switch -->
    <script src="../../plugins/bootstrap-switch/js/bootstrap-switch.min.js"></script>
    <!-- BS-Stepper -->
    <script src="../../plugins/bs-stepper/js/bs-stepper.min.js"></script>
    <!-- dropzonejs -->
    <script src="../../plugins/dropzone/min/dropzone.min.js"></script>
    <!-- AdminLTE App -->
    <script src="../../dist/js/adminlte.min.js"></script>
    <!-- AdminLTE for demo purposes -->
    <script src="../../dist/js/demo.js"></script>
    <!-- Page specific script -->
    <script>
        $(function () {
            //Initialize Select2 Elements
            $('.select2').select2()

            //Initialize Select2 Elements
            $('.select2bs4').select2({
                theme: 'bootstrap4'
            })

            //Datemask dd/mm/yyyy
            $('#datemask').inputmask('dd/mm/yyyy', { 'placeholder': 'dd/mm/yyyy' })
            //Datemask2 mm/dd/yyyy
            $('#datemask2').inputmask('mm/dd/yyyy', { 'placeholder': 'mm/dd/yyyy' })
            //Money Euro
            $('[data-mask]').inputmask()

            //Date picker
            $('#reservationdate').datetimepicker({
                format: 'L'
            });

            //Date and time picker
            $('#reservationdatetime').datetimepicker({ icons: { time: 'far fa-clock' } });

            //Date range picker
            $('#reservation').daterangepicker()
            //Date range picker with time picker
            $('#reservationtime').daterangepicker({
                timePicker: true,
                timePickerIncrement: 30,
                locale: {
                    format: 'MM/DD/YYYY hh:mm A'
                }
            })
            //Date range as a button
            $('#daterange-btn').daterangepicker(
                {
                    ranges: {
                        'Today': [moment(), moment()],
                        'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
                        'Last 7 Days': [moment().subtract(6, 'days'), moment()],
                        'Last 30 Days': [moment().subtract(29, 'days'), moment()],
                        'This Month': [moment().startOf('month'), moment().endOf('month')],
                        'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
                    },
                    startDate: moment().subtract(29, 'days'),
                    endDate: moment()
                },
                function (start, end) {
                    $('#reportrange span').html(start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'))
                }
            )

            //Timepicker
            $('#timepicker').datetimepicker({
                format: 'LT'
            })

            //Bootstrap Duallistbox
            $('.duallistbox').bootstrapDualListbox()

            //Colorpicker
            $('.my-colorpicker1').colorpicker()
            //color picker with addon
            $('.my-colorpicker2').colorpicker()

            $('.my-colorpicker2').on('colorpickerChange', function (event) {
                $('.my-colorpicker2 .fa-square').css('color', event.color.toString());
            })

            $("input[data-bootstrap-switch]").each(function () {
                $(this).bootstrapSwitch('state', $(this).prop('checked'));
            })

        })
        // BS-Stepper Init
        document.addEventListener('DOMContentLoaded', function () {
            window.stepper = new Stepper(document.querySelector('.bs-stepper'))
        })

        // DropzoneJS Demo Code Start
        Dropzone.autoDiscover = false

        // Get the template HTML and remove it from the doumenthe template HTML and remove it from the doument
        var previewNode = document.querySelector("#template")
        previewNode.id = ""
        var previewTemplate = previewNode.parentNode.innerHTML
        previewNode.parentNode.removeChild(previewNode)

        var myDropzone = new Dropzone(document.body, { // Make the whole body a dropzone
            url: "/target-url", // Set the url
            thumbnailWidth: 80,
            thumbnailHeight: 80,
            parallelUploads: 20,
            previewTemplate: previewTemplate,
            autoQueue: false, // Make sure the files aren't queued until manually added
            previewsContainer: "#previews", // Define the container to display the previews
            clickable: ".fileinput-button" // Define the element that should be used as click trigger to select files.
        })

        myDropzone.on("addedfile", function (file) {
            // Hookup the start button
            file.previewElement.querySelector(".start").onclick = function () { myDropzone.enqueueFile(file) }
        })

        // Update the total progress bar
        myDropzone.on("totaluploadprogress", function (progress) {
            document.querySelector("#total-progress .progress-bar").style.width = progress + "%"
        })

        myDropzone.on("sending", function (file) {
            // Show the total progress bar when upload starts
            document.querySelector("#total-progress").style.opacity = "1"
            // And disable the start button
            file.previewElement.querySelector(".start").setAttribute("disabled", "disabled")
        })

        // Hide the total progress bar when nothing's uploading anymore
        myDropzone.on("queuecomplete", function (progress) {
            document.querySelector("#total-progress").style.opacity = "0"
        })

        // Setup the buttons for all transfers
        // The "add files" button doesn't need to be setup because the config
        // `clickable` has already been specified.
        document.querySelector("#actions .start").onclick = function () {
            myDropzone.enqueueFiles(myDropzone.getFilesWithStatus(Dropzone.ADDED))
        }
        document.querySelector("#actions .cancel").onclick = function () {
            myDropzone.removeAllFiles(true)
        }
        // DropzoneJS Demo Code End
    </script>

</asp:Content>
