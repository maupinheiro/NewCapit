﻿<%@ Page Title="" Language="C#" MasterPageFile="Main.Master" AutoEventWireup="true" CodeBehind="Frm_CadVeiculos.aspx.cs" Inherits="NewCapit.Frm_CadVeiculos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
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

            if (txtAno) aplicarMascara(txtAno, "0000/0000");
            if (txtDataAquisicao) aplicarMascara(txtDataAquisicao, "00/00/0000");
            if (txtLicenciamento) aplicarMascara(txtLicenciamento, "00/00/0000");
            if (txtOpacidade) aplicarMascara(txtOpacidade, "00/00/0000");
            if (txtCronotacografo) aplicarMascara(txtCronotacografo, "00/00/0000");
        });
    </script>

    <div class="content-wrapper">
        <section class="content">
            <div class="container-fluid">
                <br />
                <div class="card card-warning">
                    <div class="card-header">
                        <h3 class="card-title"><i class="fas fa-shipping-fast"></i>&nbsp;VEÍCULO - NOVO CADASTRO</h3>
                    </div>
                </div>
                <div class="card-header">
                    <!-- linha 1 -->
                    <div class="row g-3">
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="details">FROTA:</span>
                                <asp:TextBox ID="txtCodVei" Style="text-align: center" runat="server" CssClass="form-control" placeholder="" MaxLength="9" OnTextChanged="txtCodVei_TextChanged" AutoPostBack="true"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator27" ControlToValidate="txtPlaca" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                               
                            </div>
                        </div>
                        <div class="col-md-1">
                        </div>
                        <div class="col-md-2">
                            <div class="form_group">
                                <span class="details">TIPO DE VEÍCULO:</span>
                                <asp:DropDownList ID="cboTipo" runat="server" CssClass="form-control select2" AutoPostBack="true" OnSelectedIndexChanged="cboTipo_SelectedIndexChanged">  
                                </asp:DropDownList><br />
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
                                <asp:RequiredFieldValidator ID="rvfddlEstados" runat="server" ControlToValidate="ddlEstados" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form_group">
                                <span class="details">MUNICIPIO:</span>
                                <asp:DropDownList ID="ddlCidades" runat="server" OnSelectedIndexChanged="ddlCidades_SelectedIndexChanged" class="form-control select2">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rvfddlCidades" runat="server" ControlToValidate="ddlCidades" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form_group">
                                <span class="details">FILIAL:</span>
                                <asp:DropDownList ID="cbFiliais" name="nomeFiliais" runat="server" CssClass="form-control select2"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rvfcbFiliais" runat="server" ControlToValidate="cbFiliais" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
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
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator12" ControlToValidate="txtAno" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form_group">
                                <span class="details">AQUISIÇÃO:</span>
                                <asp:TextBox ID="txtDataAquisicao" runat="server" Style="text-align: center" CssClass="form-control" placeholder="00/00/0000" MaxLength="10"></asp:TextBox>
                                 <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator11" ControlToValidate="txtAno" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form_group">
                                <span class="details">RENAVAM:</span>
                                <asp:TextBox ID="txtRenavam" runat="server" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="25"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator13" ControlToValidate="txtRenavam" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="form_group">
                                <span class="details">CHASSI:</span>
                                <asp:TextBox ID="txtChassi" runat="server" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="30"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator14" ControlToValidate="txtChassi" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form_group">
                                <span class="details">PROX. LICEN.:</span>
                                <asp:TextBox ID="txtLicenciamento" runat="server" CssClass="form-control" placeholder="00/00/0000" MaxLength="10" Style="text-align: center"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator15" ControlToValidate="txtLicenciamento" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                    <div class="row g-3">
                        <div class="col-md-2">
                            <div class="form_group">
                                <span class="details">TACÓGRAFO:</span>
                                <asp:DropDownList ID="ddlTacografo" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="" Text="Selecione..."></asp:ListItem>
                                    <asp:ListItem Value="DIARIO" Text="DIARIO"></asp:ListItem>
                                    <asp:ListItem Value="SEMANAL" Text="SEMANAL"></asp:ListItem>
                                    <asp:ListItem Value="FITA" Text="FITA"></asp:ListItem>
                                    <asp:ListItem Value="OUTROS" Text="OUTROS"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddlTacografo" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form_group">
                                <span class="details">MODELO:</span>
                                <asp:DropDownList ID="ddlModeloTacografo" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="" Text="Selecione..."></asp:ListItem>
                                    <asp:ListItem Value="MECANICO" Text="MECANICO"></asp:ListItem>
                                    <asp:ListItem Value="ELETRONICO" Text="ELETRONICO"></asp:ListItem>
                                    <asp:ListItem Value="OUTROS" Text="OUTROS"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="ddlModeloTacografo" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form_group">
                                <span class="details">COMP.:</span>
                                <asp:TextBox ID="txtComprimento" runat="server" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="10"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator16" ControlToValidate="txtComprimento" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form_group">
                                <span class="details">LARG.:</span>
                                <asp:TextBox ID="txtLargura" runat="server" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="10"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator17" ControlToValidate="txtLargura" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form_group">
                                <span class="details">ALT.:</span>
                                <asp:TextBox ID="txtAltura" runat="server" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="10"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator29" runat="server" ControlToValidate="txtAntt" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form_group">
                                <span class="details">PROT. CET:</span>
                                <asp:TextBox ID="txtProtocoloCET" runat="server" CssClass="form-control" MaxLength="25" Style="text-align: center"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator28" runat="server" ControlToValidate="txtAntt" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form_group">
                                <span class="details">VALIDADE:</span>
                                <asp:TextBox ID="txtVencCET" runat="server" CssClass="form-control" placeholder="00/00/0000" MaxLength="10" Style="text-align: center"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator18" ControlToValidate="txtVencCET" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form_group">
                                <span class="details">OPACIDADE:</span>
                                <asp:TextBox ID="txtOpacidade" runat="server" CssClass="form-control" placeholder="00/00/0000" MaxLength="10" Style="text-align: center"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator19" ControlToValidate="txtOpacidade" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form_group">
                                <span class="details">CRONO:</span>
                                <asp:TextBox ID="txtCronotacografo" runat="server" CssClass="form-control" placeholder="00/00/0000" MaxLength="10" Style="text-align: center"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator20" ControlToValidate="txtCronotacografo" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                    <!-- linha 3 -->
                    <div class="row g-3">
                        <div class="col-md-5">
                            <div class="form_group">
                                <span class="details">MARCA:</span>
                                <asp:DropDownList ID="ddlMarca" name="nomeFiliais" runat="server" CssClass="form-control select2"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="ddlMarca" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
                            </div>
                        </div>
                        <div class="col-md-5">
                            <div class="form-group">
                                <span class="details">MODELO:</span>
                                <asp:TextBox ID="txtModelo" runat="server" CssClass="form-control" placeholder="" MaxLength="40"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator21" ControlToValidate="txtModelo" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form_group">
                                <span class="details">COR:</span>
                                <asp:DropDownList ID="ddlCor" name="nomeFiliais" runat="server" CssClass="form-control select2"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator22" runat="server" ControlToValidate="ddlCor" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
                            </div>
                        </div>
                    </div>
                    <!-- linha 4 -->
                    <div class="row g-3">
                        <div class="col-md-2">
                            <div class="form-group">
                                <span class="">MONITORAMENTO:</span>
                                <asp:DropDownList ID="ddlMonitoramento" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="" Text="Selecione..."></asp:ListItem>
                                    <asp:ListItem Value="MONITORADO" Text="MONITORADO"></asp:ListItem>
                                    <asp:ListItem Value="RASTREADO" Text="RASTREADO"></asp:ListItem>
                                    <asp:ListItem Value="TELEMONITORADO" Text="TELEMONITORADO"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="ddlMonitoramento" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="details">CÓDIGO:</span>
                                <asp:TextBox ID="txtCodRastreador" runat="server" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="4" OnTextChanged="txtCodRastreador_TextChanged" AutoPostBack="true" ></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator24" runat="server" ControlToValidate="txtCodRastreador" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form_group">
                                <span class="details">TECNOLOGIA/RASTREADOR:</span>
                                <asp:DropDownList ID="ddlTecnologia" name="tecnologia" runat="server" CssClass="form-control select2" OnSelectedIndexChanged="ddlTecnologia_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator23" runat="server" ControlToValidate="ddlTecnologia" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form-group">
                                <span class="details">ID:</span>
                                <asp:TextBox ID="txtId" runat="server" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="11"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator25" runat="server" ControlToValidate="txtId" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
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
                                    <asp:ListItem Value="" Text="Selecione..."></asp:ListItem>
                                    <asp:ListItem Value="AGREGADO" Text="AGREGADO"></asp:ListItem>
                                    <asp:ListItem Value="FROTA" Text="FROTA"></asp:ListItem>
                                    <asp:ListItem Value="TERCEIRO" Text="TERCEIRO"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="ddlTipo" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="details">CÓDIGO:</span>
                                <asp:TextBox ID="txtCodTra" runat="server" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="11" AutoPostBack="true" OnTextChanged="txtCodTra_TextChanged"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator26" runat="server" ControlToValidate="txtCodTra" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />

                            </div>
                        </div>

                        <div class="col-md-6">
                            <div class="form_group">
                                <span class="details">PROPRIETÁRIO/TRANSPORTADORA:</span>
                                <asp:DropDownList ID="ddlAgregados" class="form-control select2" name="nomeProprietario" runat="server" OnSelectedIndexChanged="ddlAgregados_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="ddlAgregados" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
                            </div>
                        </div>

                        <div class="col-md-3">
                            <div class="form-group">
                                <span class="details">ANTT/RNTRC:</span>
                                <asp:TextBox ID="txtAntt" runat="server" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="11"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvtxtAntt" runat="server" ControlToValidate="txtAntt" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
                            </div>
                        </div>
                    </div>
                    <!-- linha 6 -->
                    <div class="row g-3">
                        <div class="col-md-2">
                            <div class="form-group">
                                <span class="details">CADASTRADO EM:</span>
                                <asp:Label ID="lblDtCadastro" runat="server" Style="text-align: center" CssClass="form-control" placeholder="" maxlength="20"></asp:Label>
                            </div>
                        </div>
                        <div class="col-md-5">
                            <div class="form-group">
                                <span class="details">POR:</span>
                                <asp:TextBox ID="txtUsuCadastro" runat="server" CssClass="form-control" placeholder="" MaxLength="60"></asp:TextBox>
                            </div>
                        </div>

                    </div>
                    <!-- linha 7 -->
                    <div class="row g-3">
                        <div class="col-md-1">

                            <asp:Button ID="btnSalvar1" CssClass="btn btn-outline-success  btn-lg" runat="server" ValidationGroup="Cadastro" OnClick="btnSalvar1_Click" Text="Cadastrar" />
                        </div>
                        <div class="col-md-2">
                            &nbsp;&nbsp;&nbsp;
                            <a href="ConsultaVeiculos.aspx" class="btn btn-outline-danger btn-lg">Cancelar               
                            </a>
                        </div>
                    </div>
                </div>
            </div>
        </section>
         <!-- Bootstrap Toast Já Cadastrado -->
        
    </div>    
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

</asp:Content>
