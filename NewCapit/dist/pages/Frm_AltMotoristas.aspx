<%@ Page Title="" Language="C#" MasterPageFile="Main.Master" AutoEventWireup="true" CodeBehind="Frm_AltMotoristas.aspx.cs" Inherits="NewCapit.dist.pages.Frm_AltMotoristas" EnableEventValidation="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
            let txtDtNasc = document.getElementById("<%= txtDtNasc.ClientID %>");
            let txtDtEmissao = document.getElementById("<%= txtDtEmissao.ClientID %>");
            let txtValCNH = document.getElementById("<%= txtValCNH.ClientID %>");
            let txtValLibRisco = document.getElementById("<%= txtValLibRisco.ClientID %>");
            let txtVAlExameTox = document.getElementById("<%= txtVAlExameTox.ClientID %>");
            let txtVAlMoop = document.getElementById("<%= txtVAlMoop.ClientID %>");
            let txtCPF = document.getElementById("<%= txtCPF.ClientID %>");
            let txtCartao = document.getElementById("<%= txtCartao.ClientID %>");
            let txtValCartao = document.getElementById("<%= txtValCartao.ClientID %>");
            let txtFixo = document.getElementById("<%= txtFixo.ClientID %>");
            let txtCelular = document.getElementById("<%= txtCelular.ClientID %>");
            let txtDtInativacao = document.getElementById("<%= txtDtInativacao.ClientID %>");


            
            if (txtDtNasc) aplicarMascara(txtDtNasc, "00/00/0000");
            if (txtDtInativacao) aplicarMascara(txtDtInativacao, "00/00/0000");
            if (txtDtEmissao) aplicarMascara(txtDtEmissao, "00/00/0000");
            if (txtValCNH) aplicarMascara(txtValCNH, "00/00/0000");
            if (txtVAlExameTox) aplicarMascara(txtVAlExameTox, "00/00/0000");
            if (txtValLibRisco) aplicarMascara(txtValLibRisco, "00/00/0000");
            if (txtVAlMoop) aplicarMascara(txtVAlMoop, "00/00/0000");
            if (txtCPF) aplicarMascara(txtCPF, "000.000.000-00");
            if (txtCartao) aplicarMascara(txtCartao, "0000 0000 0000 0000");
            if (txtValCartao) aplicarMascara(txtValCartao, "00/0000");
            if (txtFixo) aplicarMascara(txtFixo, "(00) 0000-0000");
            if (txtFixo) aplicarMascara(txtCelular, "(00) 0 0000-0000");

        });
    </script>
    <script type="text/javascript">
        function previewImage(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();

                reader.onload = function (e) {
                    var img = document.getElementById('preview');
                    img.src = e.target.result;
                };

                reader.readAsDataURL(input.files[0]); // Lê como base64 para mostrar a imagem                

            }
        }
    </script>
    <div class="content-wrapper">
        <section class="content">
            <div class="container-fluid">
                <br />
                <div id="toastContainer" class="alert alert-warning alert-dismissible" style="display: none;">
                    <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                    <h5><i class="icon fas fa-exclamation-triangle"></i>Alerta!</h5>
                    Alertas
                </div>
                <div class="card card-info">
                    <div class="card-header">
                        <h3 class="card-title"><i class="fas fa-address-card"></i>&nbsp;MOTORISTAS - ATUALIZAÇÃO DE DADOS</h3>
                    </div>
                </div>

                <%--<form>--%>
                <%--<asp:ScriptManager ID="asm" runat="server" />--%>
                <%--<div class="card-header">--%>
                <!-- Linha 1 do formulario -->
                <div class="row g-3">
                    <div class="col-md-1">
                        <div class="form-group">
                            <span class="details">CÓDIGO:</span>
                            <asp:TextBox ID="txtCodMot" Style="text-align: center" runat="server" CssClass="form-control" placeholder="" MaxLength="6"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <div class="form-group">
                            <span class="">TIPO MOTORISTA:</span>
                            <asp:DropDownList ID="ddlTipoMot" runat="server" CssClass="form-control">
                                <asp:ListItem Value="" Text=""></asp:ListItem>
                                <asp:ListItem Value="AGREGADO" Text="AGREGADO"></asp:ListItem>
                                <asp:ListItem Value="AGREGADO FUNCIONÁRIO" Text="AGREGADO FUNCIONÁRIO"></asp:ListItem>
                                <asp:ListItem Value="FUNCIONÁRIO" Text="FUNCIONÁRIO"></asp:ListItem>
                                <asp:ListItem Value="TERCEIRO" Text="TERCEIRO"></asp:ListItem>
                                <asp:ListItem Value="FUNCIONÁRIO TERCEIRO" Text="FUNCIONÁRIO TERCEIRO"></asp:ListItem>

                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <div class="form-group">
                            <span class="">CARGO:</span>
                            <asp:DropDownList ID="ddlCargo" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <div class="form-group">
                            <span class="">FUNÇÃO:</span>
                            <asp:DropDownList ID="ddlFuncao" runat="server" CssClass="form-control">
                                <asp:ListItem Value="" Text=""></asp:ListItem>
                                <asp:ListItem Value="CARREGAMENTO" Text="CARREGAMENTO"></asp:ListItem>
                                <asp:ListItem Value="ENTREGA" Text="ENTREGA"></asp:ListItem>
                                <asp:ListItem Value="SERV. INTERNO" Text="SERV. INTERNO"></asp:ListItem>
                                <asp:ListItem Value="TERM. IPIRANGA" Text="TERM. IPIRANGA"></asp:ListItem>
                                <asp:ListItem Value="OUTRO" Text="OUTRO"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <div class="form_group">
                            <span class="details">FILIAL:</span>
                            <asp:DropDownList ID="cbFiliais" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-1">
                        <div class="form-group">
                            <span class="">STATUS:</span>
                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control" AutoPostBack="True"
                                OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">
                                <asp:ListItem Value="ATIVO" Text="ATIVO"></asp:ListItem>
                                <asp:ListItem Value="INATIVO" Text="INATIVO"></asp:ListItem>
                            </asp:DropDownList></>
                        </div>
                    </div>
                    <div class="col-md-1"></div>
                    <!-- Foto do motorista -->
                    <div class="col-md-1">
                        <!-- FileUpload oculto -->
                        <asp:FileUpload ID="FileUpload1" runat="server" Style="display: none;" onchange="previewImage(this)" />

                        <!-- Imagem clicável -->

                        <img id="preview" src="<%=fotoMotorista%>" alt="Clique para selecionar imagem"
                            onclick="document.getElementById('<%= FileUpload1.ClientID %>').click();"
                            style="cursor: pointer; width: 80px; height: 80px" />
                    </div>
                </div>
                <!-- Linha 2 do formulario -->
                <asp:UpdatePanel ID="updNasc" runat="server">
                    <ContentTemplate>
                        <div class="row g-3">
                            <div class="col-md-4">
                                <div class="form-group">
                                    <span class="details">NOME COMPLETO:</span>
                                    <asp:TextBox ID="txtNomMot" runat="server" class="form-control" placeholder="" MaxLength="50"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-1">
                                <div class="form-group">
                                    <span class="details">DATA NASC.:</span>
                                    <div class="input-group">
                                        <asp:TextBox ID="txtDtNasc" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-2">
                                <div class="form-group">
                                    <span class="details">REGIÃO DO PAIS:</span>
                                    <asp:DropDownList ID="ddlRegioes" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlRegioes_SelectedIndexChanged"></asp:DropDownList>
                                    <asp:HiddenField ID="hdfRegiao" runat="server" />
                                </div>
                            </div>
                            <div class="col-md-1">
                                <div class="form_group">
                                    <span class="details">UF NASC.:</span>
                                    <asp:DropDownList ID="ddlEstNasc" runat="server" class="form-control select2" AutoPostBack="True"
                                        OnSelectedIndexChanged="ddlEstNasc_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="form_group">
                                    <span class="details">MUNICIPIO DE NASCIMENTO:</span>
                                    <asp:DropDownList ID="ddlMunicipioNasc" class="form-control select2" runat="server"></asp:DropDownList>
                                </div>
                            </div>

                            <div class="col-md-1">
                                <div class="form_group">
                                    <span class="details">ADM./CAD.:</span>
                                    <asp:TextBox ID="txtDtCad" runat="server" Style="text-align: left" CssClass="form-control" placeholder="" MaxLength="10"></asp:TextBox>
                                </div>
                            </div>

                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlEstNasc" />
                    </Triggers>
                </asp:UpdatePanel>
                <div class="row g-3">
                    <div class="col-md-4">
                        <div class="form-group">
                            <span class="details">NOME DA MÃE:</span>
                            <asp:TextBox ID="txtNomeMae" runat="server" class="form-control" placeholder="" MaxLength="50"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group">
                            <span class="details">NOME DO PAI:</span>
                            <asp:TextBox ID="txtNomePai" runat="server" class="form-control" placeholder="" MaxLength="50"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group">
                            <div class="input-group">
                                <asp:TextBox ID="txtCaminhoFoto" runat="server" class="form-control" Visible="false"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- Linha 3 do formulario -->
                <div class="row g-3">
                    <div class="col-md-2">
                        <div class="form-group">
                            <span class="details">CPF:</span>
                            <div class="input-group">
                                <asp:TextBox ID="txtCPF" runat="server" class="form-control"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-1">
                        <div class="form-group">
                            <span class="details">RG:</span>
                            <asp:TextBox ID="txtRG" runat="server" Style="text-align: center" CssClass="form-control" value=""></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-1">
                        <div class="form-group">
                            <span class="details">EMISSOR:</span>
                            <asp:TextBox ID="txtEmissor" runat="server" Style="text-align: center" CssClass="form-control" value=""></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-1">
                        <div class="form-group">
                            <span class="details">EMISSÃO:</span>
                            <div class="input-group">
                                <asp:TextBox ID="txtDtEmissao" runat="server" class="form-control"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <div class="form-group">
                            <span class="details">CARTÃO PAMCARD:</span>
                            <div class="input-group">
                                <asp:TextBox ID="txtCartao" runat="server" Style="text-align: center" class="form-control"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-1">
                        <div class="form-group">
                            <span class="details">MÊS/ANO:</span>
                            <div class="input-group">
                                <asp:TextBox ID="txtValCartao" runat="server" class="form-control"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <div class="form-group">
                            <span class="details">Nº INSS:</span>
                            <asp:TextBox ID="txtINSS" runat="server" Style="text-align: center" CssClass="form-control" value=""></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <div class="form-group">
                            <span class="details">Nº PIS:</span>
                            <div class="input-group">
                                <asp:TextBox ID="txtPIS" runat="server" class="form-control"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                </div>
                <!-- Linha 4 do formulário -->
                <asp:UpdatePanel ID="updCnh" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <div class="row g-3">
                            <div class="col-md-2">
                                <div class="form-group">
                                    <span class="details">Nº CNH:</span>
                                    <asp:TextBox ID="txtRegCNH" runat="server" Style="text-align: center" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-2">
                                <div class="form-group">
                                    <span class="details">Nº FORM DA CNH:</span>
                                    <asp:TextBox ID="txtFormCNH" runat="server" Style="text-align: center" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-2">
                                <div class="form-group">
                                    <span class="details">CÓDIGO DE SEGURANÇA:</span>
                                    <asp:TextBox ID="txtCodSeguranca" runat="server" Style="text-align: center" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtCodSeguranca_TextChanged"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" ControlToValidate="txtCodSeguranca" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
                                </div>
                            </div>
                            <div class="col-md-1">
                                <div class="form-group">
                                    <span class="">CATEGORIA:</span>
                                    <asp:DropDownList ID="ddlCat" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="" Text=""></asp:ListItem>
                                        <asp:ListItem Value="AE" Text="AE"></asp:ListItem>
                                        <asp:ListItem Value="C" Text="C"></asp:ListItem>
                                        <asp:ListItem Value="D" Text="D"></asp:ListItem>
                                        <asp:ListItem Value="E" Text="E"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-md-1">
                                <div class="form-group">
                                    <span class="details">VALIDADE CNH:</span>
                                    <asp:TextBox ID="txtValCNH" runat="server" class="form-control"></asp:TextBox>
                                </div>
                            </div>

                            <div class="col-md-1">
                                <div class="form-group">
                                    <span class="details">UF CNH:</span>
                                    <asp:DropDownList ID="ddlCNH" name="ufCNH" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlCNH_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                    <asp:HiddenField ID="hdfCnh" runat="server" />
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="form-group">
                                    <span class="details">MUNICIPIO DA CNH:</span>
                                    <asp:DropDownList ID="ddlMunicCnh" class="form-control select2" runat="server"></asp:DropDownList>
                                </div>
                            </div>


                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlCNH" />
                    </Triggers>
                </asp:UpdatePanel>
                <!-- Linha 5 do formulário -->
                <div class="row g-3">
                    <div class="col-md-2">
                        <div class="form-group">
                            <span class="">EST. CIVIL:</span>
                            <asp:DropDownList ID="ddlEstCivil" runat="server" CssClass="form-control">
                                <asp:ListItem Value="" Text=""></asp:ListItem>
                                <asp:ListItem Value="SOLTEIRO(A)" Text="SOLTEIRO(A)"></asp:ListItem>
                                <asp:ListItem Value="CASADO(A)" Text="CASADO(A)"></asp:ListItem>
                                <asp:ListItem Value="UNIÃO ESTÁVEL" Text="UNIÃO ESTÁVEL"></asp:ListItem>
                                <asp:ListItem Value="SEPARADO(A)" Text="SEPARADO(A)"></asp:ListItem>
                                <asp:ListItem Value="DIVORCIADO(A)" Text="DIVORCIADO(A)"></asp:ListItem>
                                <asp:ListItem Value="VIUVO(A)" Text="VIUVO(A)"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <div class="form-group">
                            <span class="">GÊNERO:</span>
                            <asp:DropDownList ID="ddlSexo" runat="server" CssClass="form-control">
                                <asp:ListItem Value="" Text=""></asp:ListItem>
                                <asp:ListItem Value="MASCULINO" Text="MASCULINO"></asp:ListItem>
                                <asp:ListItem Value="FEMININO" Text="FEMININO"></asp:ListItem>
                                <asp:ListItem Value="OUTRO" Text="OUTRO"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <div class="form-group">
                            <span class="">JORNADA DE TRABALHO:</span>
                            <asp:DropDownList ID="ddlJornada" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-1">
                        <div class="form-group">
                            <span class="details">CÓDIGO:</span>
                            <asp:TextBox ID="txtCodTra" runat="server" Style="text-align: center" CssClass="form-control" placeholder="" AutoPostBack="true" OnTextChanged="txtCodTra_TextChanged"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-5">
                        <div class="form_group">
                            <span class="details">PROPRIETÁRIO/TRANSPORTADORA:</span>
                            <asp:DropDownList ID="ddlAgregados" class="form-control select2" runat="server" OnSelectedIndexChanged="ddlAgregados_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                        </div>
                    </div>
                </div>
                <!-- Linha 5 do Formulário -->
                <div class="row g-3">
                    <div class="col-md-2">
                        <div class="form-group">
                            <span class="details">LIB. RISCO:</span>
                            <asp:TextBox ID="txtCodLibRisco" runat="server" CssClass="form-control" value=""></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-1">
                        <div class="form-group">
                            <span class="details">VALIDADE:</span>
                            <div class="input-group">
                                <asp:TextBox ID="txtValLibRisco" runat="server" class="form-control"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-1">
                        <div class="form-group">
                            <span class="details">EXAME:</span>
                            <div class="input-group">
                                <asp:TextBox ID="txtVAlExameTox" runat="server" class="form-control"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-1">
                        <div class="form-group">
                            <span class="details">MOOP:</span>
                            <div class="input-group">
                                <asp:TextBox ID="txtVAlMoop" runat="server" class="form-control"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <div class="form-group">
                            <span class="details">FIXO:</span>
                            <div class="input-group">
                                <asp:TextBox ID="txtFixo" runat="server" class="form-control"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <div class="form-group">
                            <span class="details">CELULAR:</span>
                            <div class="input-group">
                                <asp:TextBox ID="txtCelular" runat="server" class="form-control"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-1">
                        <div class="form-group">
                            <span class="details">CRACHÁ:</span>
                            <asp:TextBox ID="txtCracha" runat="server" CssClass="form-control" value=""></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-1">
                        <div class="form-group">
                            <span class="details">FROTA:</span>
                            <asp:TextBox ID="txtFrota" runat="server" CssClass="form-control" value=""></asp:TextBox>
                        </div>
                    </div>

                </div>
                <!-- dados do veiculo do agregado -->
                <div class="row g-3">
                    <div class="col-md-1">
                        <div class="form-group">
                            <span class="details">PROP.:</span>
                            <asp:TextBox ID="txtCodProp" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-1">
                        <div class="form-group">
                            <span class="details">PLACA:</span>
                            <asp:TextBox ID="txtPlaca" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-1">
                        <div class="form-group">
                            <span class="details">REBOQUE 1:</span>
                            <asp:TextBox ID="txtReboque1" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-1">
                        <div class="form-group">
                            <span class="details">REBOQUE 2:</span>
                            <asp:TextBox ID="txtReboque2" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <div class="form-group">
                            <span class="details">TIPO DE VEÍCULO:</span>
                            <asp:TextBox ID="txtTipoVeiculo" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>

                </div>
                <!-- Linha 6 do formulário -->
                <div class="row g-3">
                    <div class="col-md-1">
                        <div class="form-group">
                            <span class="details">CEP:</span>
                            <asp:TextBox ID="txtCepCli" runat="server" CssClass="form-control" laceholder="99999-999" MaxLength="9"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-1">
                        <br />
                        <asp:Button ID="btnCep" runat="server" Text="Pesquisar" CssClass="btn btn-outline-warning" OnClick="btnCep_Click" UseSubmitBehavior="false" />
                    </div>
                    <div class="col-md-7">
                        <div class="form-group">
                            <span class="details">ENDEREÇO:</span>
                            <asp:TextBox ID="txtEndCli" runat="server" CssClass="form-control" MaxLength="60"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-1">
                        <div class="form-group">
                            <span class="details">Nº:</span>
                            <asp:TextBox ID="txtNumero" Style="text-align: center" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <div class="form-group">
                            <span class="details">COMPL.:</span>
                            <asp:TextBox ID="txtComplemento" Style="text-align: center" runat="server" CssClass="form-control" placeholder="" MaxLength="15"> </asp:TextBox>
                        </div>
                    </div>
                </div>
                <!-- Linha 7 do formulário -->
                <div class="row g-3">
                    <div class="col-md-5">
                        <div class="form-group">
                            <span class="details">BAIRRO:</span>
                            <asp:TextBox ID="txtBaiCli" runat="server" CssClass="form-control" placeholder="" MaxLength="60"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <span class="details">MUNICIPIO:</span>
                            <asp:TextBox ID="txtCidCli" runat="server" CssClass="form-control" placeholder="" MaxLength="60"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-1">
                        <div class="form-group">
                            <span class="details">UF:</span>
                            <asp:TextBox ID="txtEstCli" Style="text-align: center" runat="server" CssClass="form-control" placeholder="" MaxLength="2"></asp:TextBox>
                        </div>
                    </div>

                </div>
                <div class="row g-3">
                    <div class="col-md-12">
                        <div class="form-group">
                            <span class="details">HISTORICO DO MOTORISTA:</span>
                            <asp:TextBox Rows="3" ID="txtHistorico" runat="server" class="form-control" placeholder="Historico ..." TextMode="MultiLine"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row g-3">
                    <div class="col-md-11">
                        <div class="form-group">
                            <asp:Label ID="motivoInativo" runat="server" class="details">MOTIVO DA INATIVAÇÃO:</asp:Label>
                            <asp:TextBox ID="txtMotivoInativacao" runat="server" CssClass="form-control" placeholder="" MaxLength="60" ForeColor="Red"></asp:TextBox>

                        </div>
                    </div>
                    <div class="col-md-1">
                        <div class="form-group">
                            <asp:Label ID="dataInativo" runat="server" class="details">DATA:</asp:Label>
                            <asp:TextBox ID="txtDtInativacao" runat="server" CssClass="form-control" placeholder="" MaxLength="60" ForeColor="Red"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <!-- Linha 8 do formulário -->
                <div class="row g-3">
                    <div class="col-md-2">
                        <div class="form-group">
                            <span class="details">CADASTRADO EM:</span>
                            <asp:Label ID="lblDtCadastro" runat="server" CssClass="form-control" readonly="true"></asp:Label>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group">
                            <span class="details">POR:</span>
                            <asp:TextBox ID="txtUsuCadastro" runat="server" CssClass="form-control" Enabled="true" MaxLength="60"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-md-2">
                        <div class="form-group">
                            <span class="details">ATUALIZADO EM:</span>
                            <asp:Label ID="lbDtAtualizacao" runat="server" CssClass="form-control" placeholder=""></asp:Label>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group">
                            <span class="details">POR:</span>
                            <asp:TextBox ID="txtAltCad" runat="server" CssClass="form-control" placeholder="" MaxLength="60"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <!-- Linha 9 do formulário -->
                <div class="row g-3">
                    <div class="col-md-1">
                        <asp:Button ID="btnSalvar1" CssClass="btn btn-outline-success  btn-lg" runat="server" Text="Atualizar" OnClick="btnSalvar1_Click" />
                    </div>
                    <div class="col-md-1">
                        <a href="/dist/pages/ConsultaMotoristas.aspx" class="btn btn-outline-danger btn-lg">Cancelar               
                        </a>
                    </div>
                </div>
                <%--</div>--%>
                <%--</form>--%>
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


    <script>
        function mostrarToastNaoEncontrado() {
            var toastEl = document.getElementById('toastNotFound');
            var toast = new bootstrap.Toast(toastEl);
            toast.show();
        }
    </script>
   <%-- <script>
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
    </script>--%>



</asp:Content>
