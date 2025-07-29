<%@ Page Title="" Language="C#" MasterPageFile="Main.Master" AutoEventWireup="true" CodeBehind="Frm_CadCarreta.aspx.cs" Inherits="NewCapit.Frm_CadCarreta" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <style>
    /* Cor padrão das abas */
    .nav-tabs .nav-link {
        background-color: #35BDB4; /* #f0f0f0 cinza claro */
        color: white;               /* cor do texto 333 */
        border: 1px solid #ccc;
        margin-right: 4px;
    }

    /* Aba ativa */
    .nav-tabs .nav-link.active {
        background-color: #007bff; /* azul Bootstrap */
        color: white;
        border-color: #007bff #007bff #fff;
    }

    /* Hover */
    .nav-tabs .nav-link:hover {
        background-color: #e0e0e0;
        color: #000;
    }
</style>


    <script>
        // Salvar aba ativa no localStorage
        document.addEventListener("DOMContentLoaded", function () {
            var triggerTabList = [].slice.call(document.querySelectorAll('#myTab button'))
            triggerTabList.forEach(function (triggerEl) {
                triggerEl.addEventListener('shown.bs.tab', function (event) {
                    localStorage.setItem('activeTab', event.target.getAttribute('data-bs-target'));
                });
            });

            // Restaurar aba ativa
            var activeTab = localStorage.getItem('activeTab');
            if (activeTab) {
                var someTabTriggerEl = document.querySelector('[data-bs-target="' + activeTab + '"]');
                if (someTabTriggerEl) {
                    var tab = new bootstrap.Tab(someTabTriggerEl);
                    tab.show();
                }
            }
        });
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

            let txtCronotacografo = document.getElementById("<%= txtCronotacografo.ClientID %>");


            if (txtAno) aplicarMascara(txtAno, "0000/0000");
            if (txtDataAquisicao) aplicarMascara(txtDataAquisicao, "00/00/0000");
            if (txtLicenciamento) aplicarMascara(txtLicenciamento, "00/00/0000");

            if (txtCronotacografo) aplicarMascara(txtCronotacografo, "00/00/0000");

        });
    </script>
    <div class="content-wrapper">
        <section class="content">
            <div class="container-fluid">
                <br />
                <div class="card card-info">
                    <div class="card-header">
                        <h3 class="card-title"><i class="fas fa-shipping-fast"></i>&nbsp;CARRETAS - NOVO CADASTRO</h3>
                    </div>
                </div>
                <div class="card-header">
                    <!-- linha 1 -->
                    <div class="row g-3">
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="details">CÓDIGO:</span>
                                <asp:TextBox ID="txtCodVei" Style="text-align: center" runat="server" CssClass="form-control" placeholder="" MaxLength="9" AutoPostBack="true" OnTextChanged="txtCodVei_TextChanged"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form-group">
                                <span class="">TIPO DE CARRETA:</span>
                                <asp:DropDownList ID="ddlTipo" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="" Text="Selecione..."></asp:ListItem>
                                    <asp:ListItem Value="AGREGADO" Text="AGREGADO"></asp:ListItem>
                                    <asp:ListItem Value="FROTA" Text="FROTA"></asp:ListItem>
                                    <asp:ListItem Value="TERCEIRO" Text="TERCEIRO"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="ddlTipo" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
                            </div>
                        </div>
                        <div class="col-md-2" id="alugada" runat="server" visible="false">
                            <div class="form-group">
                                <span class="">CARRETA:</span>
                                <asp:DropDownList ID="DropDownList1" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="ALUGADA" Text="ALUGADA"></asp:ListItem>
                                    <asp:ListItem Value="PROPRIA" Text="PROPRIA"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form_group">
                                <span class="details">FILIAL:</span>
                                <asp:DropDownList ID="cbFiliais" name="nomeFiliais" runat="server" CssClass="form-control select2" AutoPostBack="true"></asp:DropDownList>
                            </div>
                        </div>                       
                        
                    </div>

                    <!-- Abas -->
                    <ul class="nav nav-tabs" id="myTab" role="tablist">
                        <li class="nav-item" role="presentation">
                            <button class="nav-link active" id="tab1-tab" data-bs-toggle="tab" data-bs-target="#tab1" type="button" role="tab">
                                Dados da Carreta
                            </button>
                        </li>
                        <li class="nav-item" role="presentation">
                            <button class="nav-link" id="tab2-tab" data-bs-toggle="tab" data-bs-target="#tab2" type="button" role="tab">
                                Dados do Proprietário
                            </button>
                        </li>
                        <li class="nav-item" role="presentation">
                            <button class="nav-link" id="tab3-tab" data-bs-toggle="tab" data-bs-target="#tab3" type="button" role="tab">
                                Rastreamento
                            </button>
                        </li>
                        <li class="nav-item" role="presentation">
                            <button class="nav-link" id="tab4-tab" data-bs-toggle="tab" data-bs-target="#tab4" type="button" role="tab">
                                Seguro
                            </button>
                        </li>
                    </ul>

                    <!-- Conteúdo das Abas -->
                    <div class="tab-content" id="myTabContent">
                        <!-- aba 1 -->
                        <div class="tab-pane fade show active p-3 border border-top-0" id="tab1" role="tabpanel">
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
                                        <span class="details">VENC.LIC:</span>
                                        <asp:TextBox ID="txtLicenciamento" runat="server" CssClass="form-control" placeholder="00/00/0000" MaxLength="10" Style="text-align: center"></asp:TextBox>
                                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator18" ControlToValidate="txtLicenciamento" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>
                            <div class="row g-3">
                                <div class="col-md-1">
                                    <div class="form-group">
                                        <span class="details">TARA:</span>
                                        <asp:TextBox ID="txtTara" runat="server" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="6"></asp:TextBox>
                                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ControlToValidate="txtTara" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
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
                                        <asp:DropDownList ID="ddlMarca" name="nomeMarca" runat="server" CssClass="form-control select2" AutoPostBack="true"></asp:DropDownList>
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

                        </div>
                        <!-- aba 2 dados do proprietario -->
                        <div class="tab-pane fade p-3 border border-top-0" id="tab2" role="tabpanel">
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
                                <div class="col-md-2">
                                    <div class="form-group">
                                        <span class="details">ANTT/RNTRC:</span>
                                        <asp:TextBox ID="txtAntt" runat="server" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="15"></asp:TextBox>
                                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator31" ControlToValidate="txtAntt" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>
                            <!-- linha 5 se a carreta for alugada pela TNG, mostra o proprietario -->
                            <div class="row g-3">
                                <div class="col-md-2" id="cnpj" runat="server" visible="false">
                                    <div class="form-group">
                                        <span class="details">CNPJ:</span>
                                        <asp:TextBox ID="txtCNPJ" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-3" id="contrato" runat="server" visible="false">
                                    <div class="form_group">
                                        <span class="details">CONTRATO:</span>
                                        <asp:TextBox ID="txtRazaoSocial" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-1" id="inicio" runat="server" visible="false">
                                    <div class="form_group">
                                        <span class="details">INICIO:</span>
                                        <asp:TextBox ID="txtInicioContrato" runat="server" Style="text-align: center" CssClass="form-control" placeholder="00/00/0000" MaxLength="10"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-1" id="termino" runat="server" visible="false">
                                    <div class="form_group">
                                        <span class="details">TERMINO:</span>
                                        <asp:TextBox ID="txtTerminoContrato" runat="server" Style="text-align: center" CssClass="form-control" placeholder="00/00/0000" MaxLength="10"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- aba 3 dados do rastreador -->
                        <div class="tab-pane fade p-3 border border-top-0" id="tab3" role="tabpanel">                            
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
                        </div>
                        <!-- aba 4 -->
                        <div class="tab-pane fade p-3 border border-top-0" id="tab4" role="tabpanel">
                            
                        </div>

                    </div>



                   
                    <!-- linha 6 -->
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
                            <a href="ControleCarretas.aspx" class="btn btn-outline-danger btn-lg">Sair               
                            </a>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>
</asp:Content>
