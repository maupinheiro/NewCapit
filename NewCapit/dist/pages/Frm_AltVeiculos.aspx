<%@ Page Title="" Language="C#" MasterPageFile="Main.Master" AutoEventWireup="true" CodeBehind="Frm_AltVeiculos.aspx.cs" Inherits="NewCapit.Frm_AltVeiculos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery.mask/1.14.16/jquery.mask.min.js"></script>

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>

    <script>
        document.addEventListener("DOMContentLoaded", function () {
            function aplicarMascara(input, mascara) {
                input.addEventListener("input", function () {
                    let valor = input.value.replace(/\D/g, ""); // Remove tudo que não for número
                    let resultado = "";
                    let posicao = 0;

                    for (let i = 0; i < mascara.length; i++) {
                        if (mascara[i] === "0") {
                            if (valor[posicao]) {
                                resultado += valor[posicao];
                                posicao++;
                            } else {
                                break;
                            }
                        } else {
                            resultado += mascara[i];
                        }
                    }

                    input.value = resultado;
                });
            }

            // Pegando os elementos no ASP.NET
            let txtAno = document.getElementById("<%= txtAno.ClientID %>");
            let txtDataAquisicao = document.getElementById("<%= txtDataAquisicao.ClientID %>");
            let txtLicenciamento = document.getElementById("<%= txtLicenciamento.ClientID %>");
            let txtOpacidade = document.getElementById("<%= txtOpacidade.ClientID %>");
            let txtCronotacografo = document.getElementById("<%= txtCronotacografo.ClientID %>");
            let txtVencCET = document.getElementById("<%= txtVencCET.ClientID %>");


            if (txtAno) aplicarMascara(txtAno, "0000/0000");
            if (txtDataAquisicao) aplicarMascara(txtDataAquisicao, "00/00/0000");
            if (txtLicenciamento) aplicarMascara(txtLicenciamento, "00/00/0000");
            if (txtOpacidade) aplicarMascara(txtOpacidade, "00/00/0000");
            if (txtCronotacografo) aplicarMascara(txtCronotacografo, "00/00/0000");
            if (txtVencCET) aplicarMascara(txtVencCET, "00/00/0000");
        });
    </script>
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
                <div class="card card-info">
                    <div class="card-header">
                        <h3 class="card-title"><i class="fas fa-shipping-fast"></i>&nbsp;VEÍCULO - ATUALIZAÇÃO</h3>
                    </div>
                </div>
                <div class="card card-danger" id="miDiv" runat="server" visible="false">
                    <div class="card-header">
                        <h3 class="card-title"><i class="fas fa-shipping-fast"></i>&nbsp;<asp:Label ID="lblErro" runat="server" ></asp:Label></h3>
                    </div>
                </div>
                <div class="card-header">
                    <!-- linha 1 -->
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
                                <asp:DropDownList ID="cboTipo" runat="server" CssClass="form-control" AutoPostBack="true">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="BITREM" Text="BITREM"></asp:ListItem>
                                    <asp:ListItem Value="BITRUCK" Text="BITRUCK"></asp:ListItem>
                                    <asp:ListItem Value="CAVALO SIMPLES" Text="CAVALO SIMPLES"></asp:ListItem>
                                    <asp:ListItem Value="CAVALO TRUCADO" Text="CAVALO TRUCADO"></asp:ListItem>
                                    <asp:ListItem Value="CAVALO 4 EIXOS" Text="CAVALO 4 EIXOS"></asp:ListItem>
                                    <asp:ListItem Value="FURGAO" Text="FURGAO"></asp:ListItem>
                                    <asp:ListItem Value="LEVE" Text="LEVE"></asp:ListItem>
                                    <asp:ListItem Value="SAVEIRO" Text="SAVEIRO"></asp:ListItem>
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
                                <asp:DropDownList ID="ddlEstados" runat="server" AutoPostBack="True" class="form-control" OnSelectedIndexChanged="ddlEstados_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="form_group">
                                <span class="details">MUNICIPIO:</span>
                                <asp:DropDownList ID="ddlCidades" runat="server" class="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form_group">
                                <span class="details">FILIAL:</span>
                                <asp:DropDownList ID="cbFiliais" name="nomeFiliais" runat="server" CssClass="form-control" AutoPostBack="true"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="details">CADASTRO:</span>
                                <asp:Label ID="txtDtcVei" runat="server" CssClass="form-control" value=""></asp:Label>
                            </div>
                        </div>
                        <div class="col-md-1">
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
                                <span class="details">CÓDIGO:</span>
                                <asp:TextBox ID="txtCodigo" runat="server" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="10"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="details">PLACA ANT.:</span>
                                <asp:TextBox ID="txtPlacaAnt" Style="text-align: center" runat="server" CssClass="form-control" placeholder="" MaxLength="8"></asp:TextBox>
                            </div>
                        </div>
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
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form_group">
                                <span class="details">AQUISIÇÃO:</span>
                                <asp:TextBox ID="txtDataAquisicao" runat="server" Style="text-align: center" CssClass="form-control" placeholder="00/00/0000" MaxLength="10"></asp:TextBox>
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
                                <asp:TextBox ID="txtLicenciamento" runat="server" CssClass="form-control" placeholder="00/00/0000" MaxLength="10" Style="text-align: center"></asp:TextBox>
                            </div>
                        </div>


                    </div>
                    <div class="row g-3">
                        <div class="col-md-2">
                            <div class="form_group">
                                <span class="details">TACÓGRAFO:</span>
                                <asp:DropDownList ID="ddlTacografo" runat="server" CssClass="form-control">
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
                                <asp:DropDownList ID="ddlModeloTacografo" runat="server" CssClass="form-control">
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
                                <asp:TextBox ID="txtComprimento" runat="server" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="10"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form_group">
                                <span class="details">LARGURA:</span>
                                <asp:TextBox ID="txtLargura" runat="server" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="10"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form_group">
                                <span class="details">ALTURA:</span>
                                <asp:TextBox ID="txtAltura" runat="server" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="10"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form_group">
                                <span class="details">PROT.CET:</span>
                                <asp:TextBox ID="txtProtocoloCET" runat="server" CssClass="form-control" MaxLength="25" Style="text-align: center"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form_group">
                                <span class="details">VENCIMENTO:</span>
                                <asp:TextBox ID="txtVencCET" runat="server" CssClass="form-control" placeholder="00/00/0000" MaxLength="10" Style="text-align: center"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form_group">
                                <span class="details">OPACIDADE:</span>
                                <asp:TextBox ID="txtOpacidade" runat="server" CssClass="form-control" placeholder="00/00/0000" MaxLength="10" Style="text-align: center"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form_group">
                                <span class="details">CRONO:</span>
                                <asp:TextBox ID="txtCronotacografo" runat="server" CssClass="form-control" placeholder="00/00/0000" MaxLength="10" Style="text-align: center"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <!-- linha 3 -->
                    <div class="row g-3">
                        <div class="col-md-5">
                            <div class="form_group">
                                <span class="details">MARCA:</span>
                                <asp:DropDownList ID="ddlMarca" name="nomeFiliais" runat="server" CssClass="form-control" AutoPostBack="true"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-5">
                            <div class="form-group">
                                <span class="details">MODELO:</span>
                                <asp:TextBox ID="txtModelo" runat="server" CssClass="form-control" placeholder="" MaxLength="40"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form_group">
                                <span class="details">COR:</span>
                                <asp:DropDownList ID="ddlCor" runat="server" CssClass="form-control" AutoPostBack="true"></asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <!-- linha 4 -->
                    <div class="row g-3">
                        <div class="col-md-2">
                            <div class="form-group">
                                <span class="">MONITORAMENTO:</span>
                                <asp:DropDownList ID="ddlMonitoramento" runat="server" CssClass="form-control" AutoPostBack="true">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="MONITORADO" Text="MONITORADO"></asp:ListItem>
                                    <asp:ListItem Value="RASTREADO" Text="RASTREADO"></asp:ListItem>
                                    <asp:ListItem Value="TELEMONITORADO" Text="TELEMONITORADO"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="details">CÓD.TEC.:</span>
                                <asp:TextBox ID="txtCodRastreador" runat="server" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="4" OnTextChanged="txtCodRastreador_TextChanged" AutoPostBack="true"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form_group">
                                <span class="details">TECNOLOGIA/RASTREADOR:</span>
                                <asp:DropDownList ID="ddlTecnologia" name="tecnologia" runat="server" CssClass="form-control select2" OnSelectedIndexChanged="ddlTecnologia_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
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
                                <asp:DropDownList ID="ddlTipo" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="AGREGADO" Text="AGREGADO"></asp:ListItem>
                                    <asp:ListItem Value="FROTA" Text="FROTA"></asp:ListItem>
                                    <asp:ListItem Value="TERCEIRO" Text="TERCEIRO"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="details">CÓD.PROP.:</span>
                                <asp:TextBox ID="txtCodTra" runat="server" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="11" AutoPostBack="true" OnTextChanged="txtCodTra_TextChanged"></asp:TextBox>

                            </div>
                        </div>

                        <div class="col-md-6">
                            <div class="form_group">
                                <span class="details">PROPRIETÁRIO/TRANSPORTADORA:</span>
                                <asp:DropDownList ID="ddlAgregados" class="form-control select2" runat="server" OnSelectedIndexChanged="ddlAgregados_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            </div>
                        </div>

                        <div class="col-md-3">
                            <div class="form-group">
                                <span class="details">ANTT/RNTRC:</span>
                                <asp:TextBox ID="txtAntt" runat="server" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="15"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <!-- linha 6 -->
                    <div class="row g-3">
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="details">MOTORISTA:</span>
                                <asp:TextBox ID="txtCodMot" runat="server" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="9" OnTextChanged="txtCodMot_TextChanged" AutoPostBack="true"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-5">
                            <div class="form_group">
                                <span class="details">NOME COMPLETO:</span>
                                <asp:DropDownList ID="ddlMotorista" runat="server" class="form-control select2" OnSelectedIndexChanged="ddlMotorista_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            </div>
                            <asp:HiddenField ID="txtconformmessageValue" runat="server" />
                        </div>

                        <div class="col-md-2"></div>

                        <div class="col-md-2">
                            <div class="form-group">
                                <span class="">CARRETA:</span>
                                <asp:DropDownList ID="ddlCarreta" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="PROPRIA" Text="PROPRIA"></asp:ListItem>
                                    <asp:ListItem Value="TRANSNOVAG" Text="TRANSNOVAG"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form-group">
                                <asp:Label ID="numeroReb1" runat="server" class="details">REBOQUE 1:</asp:Label>
                                <asp:TextBox ID="txtReb1" runat="server" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="8"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form-group">
                                <asp:Label ID="numeroReb2" runat="server" class="details">REBOQUE 2:</asp:Label>
                                <asp:TextBox ID="txtReb2" runat="server" CssClass="form-control" Style="text-align: center" placeholder="" MaxLength="8"></asp:TextBox>
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
                        <div class="col-md-5">
                            <span class="">COMPOSIÇÃO:</span>
                            <asp:DropDownList ID="ddlComposicao" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlComposicao_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                        </div>
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="details">EIXOS:</span>
                                <asp:TextBox ID="txtEixos" runat="server" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="6"></asp:TextBox>
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
                                <span class="details">PBT:</span>
                                <asp:TextBox ID="txtPBT" runat="server" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="6"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="details">LOTAÇÃO:</span>
                                <asp:TextBox ID="txtLotacao" runat="server" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="6"></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="details">CARGA LIQ.:</span>
                                <asp:TextBox ID="txtCargaLiq" runat="server" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="6"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <!-- linha 8 -->
                    <div class="row g-3">

                        <div class="col-md-2">
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
                        <div class="col-md-2">
                            <div class="form-group">
                                <span class="details">APOLICE:</span>
                                <asp:TextBox ID="txtApolice" runat="server" Style="text-align: center" CssClass="form-control" placeholder="" disabled></asp:TextBox>
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


                        <div class="col-md-1">
                            <div class="form-group">
                                <asp:Label ID="Label1" runat="server" class="details" Visible="false">usuario</asp:Label>
                                <asp:TextBox ID="txtUsuarioAtual" runat="server" CssClass="form-control" Visible="false"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <!-- Linha 8 do formulario -->
                    <div class="row g-3">
                        <div class="col-md-2">
                            <div class="form-group">
                                <span class="details">CADASTRADO EM:</span>
                                <asp:Label ID="txtDtCadastro" runat="server" Style="text-align: center" CssClass="form-control" placeholder=""></asp:Label>
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
                                <asp:TextBox ID="txtDtAlteracao" runat="server" Style="text-align: center" CssClass="form-control" placeholder=""></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <span class="details">POR:</span>
                                <asp:TextBox ID="txtAlteradoPor" runat="server" Style="text-align: left" CssClass="form-control" placeholder="" MaxLength="60"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <!-- linha 9 -->
                    <div class="row g-3">
                        <div class="col-md-1">

                            <asp:Button ID="btnSalvar1" CssClass="btn btn-outline-success  btn-lg" runat="server" Text="Atualizar" OnClick="btnSalvar1_Click" />
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


        })


    </script>
    <script>
        function mostrarToastNaoEncontrado() {
            var toastEl = document.getElementById('toastNotFound');
            var toast = new bootstrap.Toast(toastEl);
            toast.show();
        }
    </script>

</asp:Content>
