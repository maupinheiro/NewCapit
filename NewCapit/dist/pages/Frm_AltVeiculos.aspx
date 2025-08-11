<%@ Page Title="" Language="C#" MasterPageFile="Main.Master" AutoEventWireup="true" CodeBehind="Frm_AltVeiculos.aspx.cs" Inherits="NewCapit.Frm_AltVeiculos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
    <script type="text/javascript">
        function fecharModalMotorista() {
            $('#ModalTrocarMotorista').modal('hide');
        }
    </script>
    <script type="text/javascript">
        function abrirConfirmacaoCarreta() {
            var myModal = new bootstrap.Modal(document.getElementById('modalConfirmacaoCarreta'));
            myModal.show();
        }
    </script>
     <script type="text/javascript">
         function abrirTrocaMotorista() {
             var myModal = new bootstrap.Modal(document.getElementById('ModalTrocarMotorista'));
             myModal.show();
         }
     </script>
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
    <%--<script language="javascript">
        function ConfirmMessage() {
            var selectedvalue = confirm("Esse motorista já possui vinculo com um veículo. Deseja desvincular o veículo?");
            if (selectedvalue) {
                document.getElementById('<%=txtconformmessageValue.ClientID %>').value = "Yes";
            } else {
                document.getElementById('<%=txtconformmessageValue.ClientID %>').value = "No";
            }
        }
    </script>--%>
    <div class="content-wrapper">
        <section id="formAltVei" class="content">
            <div class="container-fluid">
                <br />
                <div class="card card-info">
                    <div class="card-header">
                        <h3 class="card-title"><i class="fas fa-shipping-fast"></i>&nbsp;VEÍCULO - ATUALIZAÇÃO</h3>
                    </div>
                </div>
                <div class="card card-danger" id="miDiv" runat="server" visible="false">
                    <div class="card-header">
                        <h3 class="card-title"><i class="fas fa-shipping-fast"></i>&nbsp;<asp:Label ID="lblErro" runat="server"></asp:Label></h3>
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
                                <asp:DropDownList ID="cboTipo" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="cboTipo_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvcboTipo" runat="server" ControlToValidate="cboTipo" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
                            </div>
                        </div>
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
                                <asp:DropDownList ID="ddlEstados" runat="server" AutoPostBack="True" class="form-control select2" OnSelectedIndexChanged="ddlEstados_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlEstados" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="form_group">
                                <span class="details">MUNICIPIO:</span>
                                <asp:DropDownList ID="ddlCidades" runat="server" class="form-control select2">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlCidades" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form_group">
                                <span class="details">FILIAL:</span>
                                <asp:DropDownList ID="cbFiliais" name="nomeFiliais" runat="server" CssClass="form-control select2" AutoPostBack="true"></asp:DropDownList>
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
                                <span class="details">LICEN.:</span>
                                <asp:TextBox ID="txtLicenciamento" runat="server" CssClass="form-control" placeholder="00/00/0000" MaxLength="10" Style="text-align: center"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator18" ControlToValidate="txtLicenciamento" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
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
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlTacografo" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
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
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddlModeloTacografo" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form_group">
                                <span class="details">COMP.:</span>
                                <asp:TextBox ID="txtComprimento" runat="server" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="10"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator19" ControlToValidate="txtComprimento" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form_group">
                                <span class="details">LARG.:</span>
                                <asp:TextBox ID="txtLargura" runat="server" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="10"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator20" ControlToValidate="txtLargura" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form_group">
                                <span class="details">ALT.:</span>
                                <asp:TextBox ID="txtAltura" runat="server" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="10"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator21" ControlToValidate="txtAltura" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form_group">
                                <span class="details">PROT.CET:</span>
                                <asp:TextBox ID="txtProtocoloCET" runat="server" CssClass="form-control" MaxLength="25" Style="text-align: center"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator22" ControlToValidate="txtProtocoloCET" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form_group">
                                <span class="details">VALIDADE:</span>
                                <asp:TextBox ID="txtVencCET" runat="server" CssClass="form-control" placeholder="00/00/0000" MaxLength="10" Style="text-align: center"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator23" ControlToValidate="txtVencCET" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form_group">
                                <span class="details">OPACIDADE:</span>
                                <asp:TextBox ID="txtOpacidade" runat="server" CssClass="form-control" placeholder="00/00/0000" MaxLength="10" Style="text-align: center"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator24" ControlToValidate="txtOpacidade" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form_group">
                                <span class="details">CRONO:</span>
                                <asp:TextBox ID="txtCronotacografo" runat="server" CssClass="form-control" placeholder="00/00/0000" MaxLength="10" Style="text-align: center"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator25" ControlToValidate="txtCronotacografo" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                    <!-- linha 3 -->
                    <div class="row g-3">
                        <div class="col-md-5">
                            <div class="form_group">
                                <span class="details">MARCA:</span>
                                <asp:DropDownList ID="ddlMarca" name="nomeFiliais" runat="server" CssClass="form-control select2" AutoPostBack="true"></asp:DropDownList>
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
                                <asp:DropDownList ID="ddlCor" runat="server" CssClass="form-control select2" AutoPostBack="true"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="ddlCor" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
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
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="ddlMonitoramento" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
                            </div>
                        </div>
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
                        <div class="col-md-2">
                            <div class="form-group">
                                <span class="">TIPO:</span>
                                <asp:DropDownList ID="ddlTipo" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="AGREGADO" Text="AGREGADO"></asp:ListItem>
                                    <asp:ListItem Value="FROTA" Text="FROTA"></asp:ListItem>
                                    <asp:ListItem Value="TERCEIRO" Text="TERCEIRO"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="ddlTipo" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
                            </div>
                        </div>
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
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="details">MOTORISTA:</span>
                                <asp:TextBox ID="txtCodMot" runat="server" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="9" OnTextChanged="txtCodMot_TextChanged" AutoPostBack="true"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator32" ControlToValidate="txtCodMot" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="col-md-5">
                            <div class="form_group">
                                <span class="details">NOME COMPLETO:</span>
                                <asp:DropDownList ID="ddlMotorista" runat="server" class="form-control select2" OnSelectedIndexChanged="ddlMotorista_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="ddlMotorista" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
                            </div>
                            <asp:HiddenField ID="txtconformmessageValue" runat="server" />
                        </div>

                        <div class="col-md-2"></div>

                        <div class="col-md-2" id="carreta" runat="server" visible="false">
                            <div class="form-group">
                                <span class="">CARRETA:</span>
                                <asp:DropDownList ID="ddlCarreta" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="PROPRIA" Text="PROPRIA"></asp:ListItem>
                                    <asp:ListItem Value="TRANSNOVAG" Text="TRANSNOVAG"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="ddlCarreta" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
                            </div>
                        </div>
                        <%--<asp:Panel ID="pnlDivReboque1" runat="server" Visible="false">--%>
                        <div class="col-md-1" id="pnlDivReboque1" runat="server" visible="false">
                            <div class="form-group">
                                <asp:Label ID="numeroReb1" runat="server" class="details">REBOQUE 1:</asp:Label>
                                <asp:TextBox ID="txtReb1" runat="server" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="8" AutoPostBack="true" OnTextChanged="txtReb1_TextChanged"></asp:TextBox>
                            </div>
                        </div>
                        <%--</asp:Panel>--%>
                        <%--<asp:Panel ID="pnlDivReboque2" runat="server" Visible="false">--%>
                        <div class="col-md-1" id="pnlDivReboque2" runat="server" visible="false">
                            <div class="form-group">
                                <asp:Label ID="numeroReb2" runat="server" class="details">REBOQUE 2:</asp:Label>
                                <asp:TextBox ID="txtReb2" runat="server" CssClass="form-control" Style="text-align: center" placeholder="" MaxLength="8" AutoPostBack="true" OnTextChanged="txtReb2_TextChanged"></asp:TextBox>
                            </div>
                        </div>
                        <%--</asp:Panel>--%>
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
                            <asp:DropDownList ID="ddlComposicao" runat="server" CssClass="form-control select2" OnSelectedIndexChanged="ddlComposicao_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
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
                                <asp:TextBox ID="txtCargaLiq" runat="server" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="6" Enabled="true"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-1" id="motAnterior" runat="server">
                            <div class="form-group">
                                <span class="details">MOT. ANTERIOR</span>
                                <asp:TextBox ID="txtMotAnterior" runat="server" Style="text-align: center" CssClass="form-control" Enabled="false"></asp:TextBox>
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
        <!-- modal motorista atrelado -->
        <div class="modal fade" id="ModalTrocarMotorista" tabindex="-1" role="dialog">
            <div class="modal-dialog" role="document">
                <asp:UpdatePanel ID="UpdatePanelTrocaMot" runat="server">
                    <ContentTemplate>
                        <div class="modal-content">
                            <div class="modal-header">
                                <h5 class="modal-title">Transferir Motorista</h5>
                                <button type="button" class="close" data-dismiss="modal">&times;</button>
                            </div>
                            <div class="modal-body">
                                Motorista:
                        <asp:Label ID="txtMotoristaAtrelado" runat="server" CssClass="form-control"></asp:Label>Atrelado ao veículo:
                        <asp:Label ID="txtPlacaAtrelada" runat="server" CssClass="form-control"></asp:Label>Deseja transferi-lo para o novo veículo?
                            </div>
                            <div class="modal-footer">
                                <asp:Button ID="btnSalvarTrocaMotorista" runat="server" Text="Sim" CssClass="btn btn-success" OnClick="btnSalvarTrocaMotorista_Click" UseSubmitBehavior="false" />
                                <button type="button" class="btn btn-secondary" data-dismiss="modal">Não</button>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <!-- modal carreta atrelada -->
        <div class="modal fade" id="modalConfirmacaoCarreta" tabindex="-1" aria-labelledby="modalConfirmacaoLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="modalConfirmacaoLabelCarreta">Confirmação</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Fechar"></button>
                    </div>
                    <div class="modal-body">
                        Frota/Carreta:
                        <asp:Label ID="txtFrotaAtrelado" runat="server" CssClass="form-control"></asp:Label>
                        Atrelada a Frota/Cavalo:
                        <asp:Label ID="txtFrotaCavalo" runat="server" CssClass="form-control"></asp:Label>
                        Transportadora/Agregado:
                        <asp:Label ID="txtTransportadora" runat="server" CssClass="form-control"></asp:Label>
                        Deseja transferi-la para o novo veículo?
                    </div>
                    <div class="modal-footer">
                        <button type="button" id="carretaNao" class="btn btn-secondary" data-bs-dismiss="modal" onclick="carretaNao">Não</button>
                        <button type="button" class="btn btn-primary">Sim</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
   
</asp:Content>
