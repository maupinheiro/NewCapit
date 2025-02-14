<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Frm_AltVeiculos.aspx.cs" Inherits="NewCapit.Frm_AltVeiculos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
    <div class="container mt-5">
        <div class="d-sm-flex align-items-center justify-content-between mb-4">
            <h3 class="h3 mb-2 text-gray-800"><i class="fas fa-shipping-fast"></i>VEÍCULO </h3>
            <h3>ATUALIZA CADASTRO</h3>
        </div>
        <hr />
        <!-- Linha 1 do formulario -->
        <div class="row g-3">
            <div class="col-md-1">
                <div class="form-group">
                    <span class="details">FROTA:</span>
                    <asp:TextBox ID="txtCodVei" Style="text-align: center" runat="server" ForeColor="Blue" CssClass="form-control" placeholder="" MaxLength="9"></asp:TextBox>
                </div>
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
            <div class="col-md-3">
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
            <div class="col-md-1">
                <div class="form_group">
                    <span class="details">CADASTRO:</span>
                    <asp:TextBox ID="txtCadastro" runat="server" ForeColor="Blue" CssClass="form-control" placeholder="99/99/9999" MaxLength="10" Style="text-align: center" Width="130px"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-1">
                <div class="form-group">
                    <span class="">STATUS:</span>
                    <asp:DropDownList ID="status" runat="server" ForeColor="Blue" CssClass="form-control">
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
                    <asp:DropDownList ID="ddlTacografo" runat="server" ForeColor="Blue" CssClass="form-control" Width="130px">
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
                    <asp:TextBox ID="txtLargura" runat="server" ForeColor="Blue" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="10"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-1">
                <div class="form_group">
                    <span class="details">ALTURA:</span>
                    <asp:TextBox ID="txtAltura" runat="server" ForeColor="Blue" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="10"></asp:TextBox>
                </div>
            </div>
        </div>
        <!-- Linha 3 do formulario -->
        <div class="row g-3">
            <div class="col-md-3">
                <div class="form_group">
                    <span class="details">MARCA:</span>
                    <asp:DropDownList ID="ddlMarca" name="nomeFiliais" runat="server" ForeColor="Blue" CssClass="form-control"></asp:DropDownList>
                </div>
            </div>
            <div class="col-md-3">
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
            <div class="col-md-2">
                <div class="form_group">
                    <span class="details">PROT.CET:</span>
                    <asp:TextBox ID="txtProtocolo" runat="server" ForeColor="Blue" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="10"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-1">
                <div class="form_group">
                    <span class="details">VALIDADE:</span>
                    <asp:TextBox ID="txtValCET" runat="server" ForeColor="Blue" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="10" data-mask="00/00/0000"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-1">
                <div class="form_group">
                    <span class="details">OPACIDADE:</span>
                    <asp:TextBox ID="txtOpacidade" runat="server" ForeColor="Blue" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="10" data-mask="00/00/0000"></asp:TextBox>
                </div>
            </div>
        </div>
        <!-- Linha 4 do formulario -->
        <div class="row g-3">
            <div class="col-md-2">
                <div class="form-group">
                    <span class="">MONITORAMENTO:</span>
                    <asp:DropDownList ID="ddlMonitoramento" runat="server" ForeColor="Blue" CssClass="form-control">
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
                        <asp:ListItem Value="" Text="SELECIONE"></asp:ListItem>
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
                    <asp:DropDownList ID="DropDownList1" runat="server" ForeColor="Blue" CssClass="form-control">
                        <asp:ListItem Value="" Text="SELECIONE"></asp:ListItem>
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
            <div class="col-md-3">
                <div class="form_group">
                    <span class="details">PROPRIETÁRIO/TRANSPORTADORA:</span>
                    <asp:DropDownList ID="ddlAgregados" class="js-example-basic-single" name="nomeProprietario" runat="server" ForeColor="Blue" Width="400px" OnSelectedIndexChanged="ddlAgregados_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                </div>
            </div>
            <div class="col-md-1">
                <div class="form-group">
                    <span class="details">ANTT/RNTRC:</span>
                    <asp:TextBox ID="txtAntt" runat="server" Style="text-align: center" ForeColor="Blue" CssClass="form-control" placeholder="" MaxLength="11"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-1">
                <div class="form-group">
                    <span class="details">CÓDIGO:</span>
                    <asp:TextBox ID="txtCodMot" runat="server" ForeColor="Blue" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="9"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-4">
                <div class="form_group">
                    <span class="details">MOTORISTA:</span>
                    <asp:DropDownList ID="ddlMotorista" runat="server" ForeColor="Blue" class="js-example-basic-single" OnSelectedIndexChanged="ddlMotorista_SelectedIndexChanged" Width="520px" AutoPostBack="true"></asp:DropDownList>

                </div>
                <asp:HiddenField ID="txtconformmessageValue" runat="server" />
            </div>

        </div>
        <!-- linha 6 do formulario -->
        <div class="row g-3">
            <div class="col-md-1">
                <div class="form-group">
                    <span class="details">EIXOS:</span>
                    <asp:TextBox ID="txtEixos" runat="server" ForeColor="Blue" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="6"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-1">
                <div class="form-group">
                    <span class="details">LOTAÇÃO:</span>
                    <asp:TextBox ID="txtCap" runat="server" ForeColor="Blue" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="6"></asp:TextBox>
                </div>
            </div>

            <div class="col-md-1">
                <div class="form-group">
                    <span class="details">TARA:</span>
                    <asp:TextBox ID="txtTara" runat="server" ForeColor="Blue" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="6"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-1">
                <div class="form-group">
                    <span class="details">TOL. %:</span>
                    <asp:TextBox ID="txtTolerancia" runat="server" ForeColor="Blue" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="6"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-1">
                <div class="form-group">
                    <span class="details">CARGA LIQ.:</span>
                    <asp:TextBox ID="txtPBT" runat="server" ForeColor="Blue" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="6"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-1">
                <div class="form-group">
                    <span class="details">TIPO SEGURO:</span>
                    <asp:TextBox ID="txtTipoSeguro" runat="server" ForeColor="Blue" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="30" disabled></asp:TextBox>
                </div>
            </div>
            <div class="col-md-3">
                <div class="form-group">
                    <span class="details">COMPANHIA:</span>
                    <asp:TextBox ID="txtSeguradora" runat="server" ForeColor="Blue" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="50" disabled></asp:TextBox>
                </div>
            </div>
            <div class="col-md-1">
                <div class="form-group">
                    <span class="details">APOLICE:</span>
                    <asp:TextBox ID="txtApolice" runat="server" ForeColor="Blue" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="30" disabled></asp:TextBox>
                </div>
            </div>
            <div class="col-md-1">
                <div class="form-group">
                    <span class="details">VALIDADE:</span>
                    <asp:TextBox ID="txtValidadeApolice" runat="server" ForeColor="Blue" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="10" disabled></asp:TextBox>
                </div>
            </div>
            <div class="col-md-1">
                <div class="form-group">
                    <span class="details">FRANQUIA:</span>
                    <asp:TextBox ID="txtValorFranquia" runat="server" ForeColor="Blue" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="11" disabled></asp:TextBox>
                </div>
            </div>

        </div>
        <!-- Linha 7 do formulario -->
        <div class="row g-3">
            <div class="col-md-2">
                <div class="form-group">
                    <span class="">TIPO:</span>
                    <asp:DropDownList ID="ddlTipo" runat="server" ForeColor="Blue" CssClass="form-control">
                        <asp:ListItem Value="" Text=""></asp:ListItem>
                        <asp:ListItem Value="AGREGADO" Text="AGREGADO"></asp:ListItem>
                        <asp:ListItem Value="FROTA" Text="FROTA"></asp:ListItem>
                        <asp:ListItem Value="TERCEIRO" Text="TERCEIRO"></asp:ListItem>
                    </asp:DropDownList></>
                </div>
            </div>
            <div class="col-md-2">
                <div class="form-group">
                    <span class="">CARRETA:</span>
                    <asp:DropDownList ID="ddlCarreta" runat="server" ForeColor="Blue" CssClass="form-control">
                        <asp:ListItem Value="" Text=""></asp:ListItem>
                        <asp:ListItem Value="PROPRIA" Text="PROPRIA"></asp:ListItem>
                        <asp:ListItem Value="TRANSNOVAG" Text="TRANSNOVAG"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>

            <div class="col-md-1">
                <div class="form-group">
                    <span id="numeroReb1" class="details">REB 999999:</span>
                    <asp:TextBox ID="txtReb1" runat="server" ForeColor="Blue" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="8"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-1">
                <div class="form-group">
                    <span ID="numeroReb2" class="details">REB 999999:</span>
                    <asp:TextBox ID="txtReb2" runat="server" ForeColor="Blue" CssClass="form-control" Style="text-align: center" placeholder="" MaxLength="8"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-6">
                <div class="form-group">
                    <span class="">COMPOSIÇÃO:</span>
                    <asp:DropDownList ID="ddlComposicao" runat="server" ForeColor="Blue" CssClass="form-control" OnSelectedIndexChanged="ddlComposicao_SelectedIndexChanged" AutoPostBack="true">
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
                    <asp:Label ID="txtDtCadastro" runat="server" Style="text-align: center" ForeColor="Blue" CssClass="form-control" placeholder="" maxlength="20"></asp:Label>
                </div>
            </div>
            <div class="col-md-4">
                <div class="form-group">
                    <span class="details">POR:</span>
                    <asp:TextBox ID="txtCadastradoPor" runat="server" ForeColor="Blue" CssClass="form-control" placeholder="" MaxLength="60"></asp:TextBox>
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

    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.5.0/jquery.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery.mask/1.14.16/jquery.mask.js"></script>



    <link href="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/css/select2.min.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap/5.3.3/js/bootstrap.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/js/select2.min.js"></script>
    <script>
        $(document).ready(function () {
            $('.js-example-basic-single').select2();
        });
    </script>


</asp:Content>
