<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Frm_CadMotoristas.aspx.cs" Inherits="NewCapit.Frm_CadMotoristas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-5">
        <div class="d-sm-flex align-items-center justify-content-between mb-4">
            <h3 class="h3 mb-2 text-gray-800"><i class="fas fa-user-plus"></i>&nbsp;MOTORISTA</h3>
            <form>
                <div class="form-group">
                    <img src="../fotos/usuario.jpg" class="rounded float-right" alt="...">
                    <!--input type="file" class="form-control-file" id="exampleFormControlFile1"-->
                </div>
            </form>
        </div>
        <hr />
        <!-- Linha 1 do formulario -->
        <div class="row g-3">
            <div class="col-md-1">
                <div class="form-group">
                    <span class="details">CÓDIGO:</span>
                    <asp:TextBox ID="txtCodMot" runat="server" ForeColor="Blue" CssClass="form-control" placeholder="" MaxLength="11"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-1">
                <br />
                <asp:Button ID="btnMotorista" runat="server" Text="Pesquisar" CssClass="btn btn-outline-warning" />
            </div>
            <div class="col-md-3">
                <div class="form-group">
                    <span class="details">NOME COMPLETO:</span>
                    <asp:TextBox ID="txtNomMot" runat="server" ForeColor="Blue" CssClass="form-control" placeholder="" MaxLength="45"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-1">
                <div class="form-group">
                    <span class="details">DT. NASC.:</span>
                    <asp:TextBox ID="txtDtCadastro" runat="server" data-mask="00/00/0000" ForeColor="Blue" CssClass="form-control" value=""></asp:TextBox>
                </div>
            </div>
            <div class="col-md-1">
                <div class="form-group">
                    <span class="details">UF NASC.:</span>
                    <asp:DropDownList ID="ddlUfNasc" runat="server" ForeColor="Blue" class="js-example-basic-single"
                        Width="110px">
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
                    <span class="details">MUNICIPIO DE NASCIMENTO:</span>
                    <asp:TextBox ID="txtCidNasc" runat="server" Style="text-align: left" ForeColor="Blue" CssClass="form-control" placeholder="" MaxLength="40"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-1">
                <div class="form_group">
                    <span class="details">CADASTRO:</span>
                    <asp:TextBox ID="txtDtCad" runat="server" Style="text-align: left" ForeColor="Blue" CssClass="form-control" placeholder="" MaxLength="10"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-1">
                <div class="form-group">
                    <span class="">SITUAÇÃO:</span>
                    <asp:DropDownList ID="ddlTipo" runat="server" ForeColor="Blue" CssClass="form-control">
                        <asp:ListItem Value="ATIVO" Text="ATIVO"></asp:ListItem>
                        <asp:ListItem Value="INATIVO" Text="INATIVO"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>


        </div>
        <!-- Linha 2 do formulario -->
        <div class="row g-3">
            <div class="col-md-2">
                <div class="form-group">
                    <span class="details">CPF:</span>
                    <asp:TextBox ID="txtCPF" runat="server" data-mask="000.000.000-00" Style="text-align: center" ForeColor="Blue" CssClass="form-control" value=""></asp:TextBox>
                </div>
            </div>
            <div class="col-md-1">
                <div class="form-group">
                    <span class="details">RG:</span>
                    <asp:TextBox ID="txtRG" runat="server" Style="text-align: center" ForeColor="Blue" CssClass="form-control" value=""></asp:TextBox>
                </div>
            </div>
            <div class="col-md-1">
                <div class="form-group">
                    <span class="details">EMISSOR:</span>
                    <asp:TextBox ID="txtEmissor" runat="server" Style="text-align: center" ForeColor="Blue" CssClass="form-control" value=""></asp:TextBox>
                </div>
            </div>
            <div class="col-md-1">
                <div class="form-group">
                    <span class="details">EMISSÃO:</span>
                    <asp:TextBox ID="txtDtEmissao" runat="server" data-mask="00/00/0000" Style="text-align: center" ForeColor="Blue" CssClass="form-control" value=""></asp:TextBox>
                </div>
            </div>
            <div class="col-md-2">
                <div class="form-group">
                    <span class="details">CARTÃO PAMCARD:</span>
                    <asp:TextBox ID="txtCartao" runat="server" data-mask="0000 0000 0000 0000" Style="text-align: center" ForeColor="Blue" CssClass="form-control" value=""></asp:TextBox>
                </div>
            </div>
            <div class="col-md-1">
                <div class="form-group">
                    <span class="details">MÊS/ANO:</span>
                    <asp:TextBox ID="txtValCartao" runat="server" data-mask="00/0000" Style="text-align: center" ForeColor="Blue" CssClass="form-control" value=""></asp:TextBox>
                </div>
            </div>
            <div class="col-md-2">
                <div class="form-group">
                    <span class="details">INSS:</span>
                    <asp:TextBox ID="txtINSS" runat="server" Style="text-align: center" ForeColor="Blue" CssClass="form-control" value=""></asp:TextBox>
                </div>
            </div>
            <div class="col-md-2">
                <div class="form-group">
                    <span class="details">PIS:</span>
                    <asp:TextBox ID="txtPIS" runat="server" data-mask="000.00000.00.0" Style="text-align: center" ForeColor="Blue" CssClass="form-control" value=""></asp:TextBox>
                </div>
            </div>

        </div>
        <!-- Linha 3 do formulário -->
        <div class="row g-3">
            <div class="col-md-2">
                <div class="form-group">
                    <span class="details">CNH:</span>
                    <asp:TextBox ID="txtRegCNH" runat="server" Style="text-align: center" ForeColor="Blue" CssClass="form-control" value=""></asp:TextBox>
                </div>
            </div>
            <div class="col-md-2">
                <div class="form-group">
                    <span class="details">FORM CNH:</span>
                    <asp:TextBox ID="txtFormCNH" runat="server" Style="text-align: center" ForeColor="Blue" CssClass="form-control" value=""></asp:TextBox>
                </div>
            </div>
            <div class="col-md-2">
                <div class="form-group">
                    <span class="details">CÓDIGO SEGURANÇA:</span>
                    <asp:TextBox ID="txtCodSeguranca" runat="server" Style="text-align: center" ForeColor="Blue" CssClass="form-control" value=""></asp:TextBox>
                </div>
            </div>
            <div class="col-md-1">
                <div class="form-group">
                    <span class="">CAT.:</span>
                    <asp:DropDownList ID="ddlCat" runat="server" ForeColor="Blue" CssClass="form-control">
                        <asp:ListItem Value="" Text=""></asp:ListItem>
                        <asp:ListItem Value="AE" Text="AE"></asp:ListItem>
                        <asp:ListItem Value="D" Text="D"></asp:ListItem>
                        <asp:ListItem Value="E" Text="E"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-md-1">
                <div class="form-group">
                    <span class="details">VALIDADE:</span>
                    <asp:TextBox ID="txtValCNH" runat="server" data-mask="00/00/0000" Style="text-align: center" ForeColor="Blue" CssClass="form-control" value=""></asp:TextBox>
                </div>
            </div>
            <div class="col-md-1">
                <div class="form-group">
                    <span class="details">UF CNH:</span>
                    <asp:DropDownList ID="ddlCNH" runat="server" ForeColor="Blue" class="js-example-basic-single"
                        Width="110px">
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
                <div class="form-group">
                    <span class="details">MUNICIPIO:</span>
                    <asp:TextBox ID="txtMunicipioCNH" runat="server" Style="text-align: center" ForeColor="Blue" CssClass="form-control" value=""></asp:TextBox>
                </div>
            </div>


        </div>
        <!-- Linha 4 do formulário -->
        <div class="row g-3">
            <div class="col-md-2">
                <div class="form-group">
                    <span class="">EST. CIVIL:</span>
                    <asp:DropDownList ID="ddlEstCivil" runat="server" ForeColor="Blue" CssClass="form-control">
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
                    <span class="">SEXO:</span>
                    <asp:DropDownList ID="ddlSexo" runat="server" ForeColor="Blue" CssClass="form-control">
                        <asp:ListItem Value="" Text=""></asp:ListItem>
                        <asp:ListItem Value="MASCULINO" Text="MASCULINO"></asp:ListItem>
                        <asp:ListItem Value="FEMININO" Text="FEMININO"></asp:ListItem>
                        <asp:ListItem Value="OUTRO" Text="OUTRO"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-md-2">
                <div class="form_group">
                    <span class="details">FILIAL:</span>
                    <asp:DropDownList ID="cbFiliais" name="nomeFiliais" runat="server" ForeColor="Blue" CssClass="form-control"></asp:DropDownList>
                </div>
            </div>
            <div class="col-md-2">
                <div class="form-group">
                    <span class="">TIPO MOTORISTA:</span>
                    <asp:DropDownList ID="ddlTipoMot" runat="server" ForeColor="Blue" CssClass="form-control">
                        <asp:ListItem Value="" Text=""></asp:ListItem>
                        <asp:ListItem Value="AGREGADO" Text="AGREGADO"></asp:ListItem>
                        <asp:ListItem Value="FUNCIONÁRIO" Text="FUNCIONÁRIO"></asp:ListItem>
                        <asp:ListItem Value="TERCEIRO" Text="TERCEIRO"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-md-2">
                <div class="form-group">
                    <span class="">CARGO:</span>
                    <asp:DropDownList ID="ddlCargo" runat="server" ForeColor="Blue" CssClass="form-control">
                        <asp:ListItem Value="" Text=""></asp:ListItem>
                        <asp:ListItem Value="CARRETEIRO" Text="CARRETEIRO"></asp:ListItem>
                        <asp:ListItem Value="MOTORISTA" Text="MOTORISTA"></asp:ListItem>
                        <asp:ListItem Value="CARRETEIRO N I" Text="CARRETEIRO N I"></asp:ListItem>
                        <asp:ListItem Value="CARRETEIRO N II" Text="CARRETEIRO N II"></asp:ListItem>
                        <asp:ListItem Value="CARRETEIRO N III" Text="CARRETEIRO N III"></asp:ListItem>
                        <asp:ListItem Value="CARRETEIRO BITREM N I" Text="CARRETEIRO BITREM N I"></asp:ListItem>
                        <asp:ListItem Value="CARRETEIRO BITREM N II" Text="CARRETEIRO BITREM N II"></asp:ListItem>
                        <asp:ListItem Value="CARRETEIRO BITREM N III" Text="CARRETEIRO BITREM N III"></asp:ListItem>
                        <asp:ListItem Value="MOTORISTA N I" Text="MOTORISTA N I"></asp:ListItem>
                        <asp:ListItem Value="MOTORISTA N II" Text="MOTORISTA N II"></asp:ListItem>
                        <asp:ListItem Value="MOTORISTA N III" Text="MOTORISTA N III"></asp:ListItem>
                        <asp:ListItem Value="MOTORISTA BITRUCK N I" Text="MOTORISTA BITRUCK N I"></asp:ListItem>
                        <asp:ListItem Value="MOTORISTA BITRUCK N II" Text="MOTORISTA BITRUCK N II"></asp:ListItem>
                        <asp:ListItem Value="MOTORISTA BITRUCK N III" Text="MOTORISTA BITRUCK N III"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-md-2">
                <div class="form-group">
                    <span class="">FUNÇÃO:</span>
                    <asp:DropDownList ID="ddlFuncao" runat="server" ForeColor="Blue" CssClass="form-control">
                        <asp:ListItem Value="" Text=""></asp:ListItem>
                        <asp:ListItem Value="CARREGAMENTO" Text="CARREGAMENTO"></asp:ListItem>
                        <asp:ListItem Value="ENTREGA" Text="ENTREGA"></asp:ListItem>
                        <asp:ListItem Value="SERV. INTERNO" Text="SERV. INTERNO"></asp:ListItem>
                        <asp:ListItem Value="TERM. IPIRANGA" Text="TERM. IPIRANGA"></asp:ListItem>
                        <asp:ListItem Value="OUTRO" Text="OUTRO"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>

        </div>
        <!-- Linha 5 do Formulário -->
        <div class="row g-3">
            <div class="col-md-2">
                <div class="form-group">
                    <span class="">JORNADA:</span>
                    <asp:DropDownList ID="ddlJornada" runat="server" ForeColor="Blue" CssClass="form-control">
                        <asp:ListItem Value="" Text=""></asp:ListItem>
                        <asp:ListItem Value="INTEGRAL" Text="INTEGRAL"></asp:ListItem>
                        <asp:ListItem Value="06:00 - 14:48" Text="06:00 - 14:48"></asp:ListItem>
                        <asp:ListItem Value="06:00 - 15:48" Text="06:00 - 15:48"></asp:ListItem>
                        <asp:ListItem Value="06:00 - 18:00 12x36" Text="06:00 - 18:00 12x36"></asp:ListItem>
                        <asp:ListItem Value="09:00 - 18:48" Text="09:00 - 18:48"></asp:ListItem>
                        <asp:ListItem Value="12:00 - 21:48" Text="12:00 - 21:48"></asp:ListItem>
                        <asp:ListItem Value="13:25 - 22:11" Text="13:25 - 22:11"></asp:ListItem>
                        <asp:ListItem Value="18:00 - 06:00 12x36" Text="18:00 - 06:00 12x36"></asp:ListItem>
                        <asp:ListItem Value="21:12 - 06:00" Text="21:12 - 06:00"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-md-1">
                <div class="form-group">
                    <span class="details">LIB. RISCO:</span>
                    <asp:TextBox ID="txtCodLibRisco" runat="server" ForeColor="Blue" CssClass="form-control" value=""></asp:TextBox>
                </div>
            </div>
            <div class="col-md-1">
                <div class="form-group">
                    <span class="details">VALIDADE:</span>
                    <asp:TextBox ID="txtValLibRisco" runat="server" data-mask="00/00/0000" ForeColor="Blue" CssClass="form-control" value=""></asp:TextBox>
                </div>
            </div>
            <div class="col-md-1">
                <div class="form-group">
                    <span class="details">VENC.TOXIC.:</span>
                    <asp:TextBox ID="txtVAlExameTox" runat="server" data-mask="00/00/0000" ForeColor="Blue" CssClass="form-control" value=""></asp:TextBox>
                </div>
            </div>
            <div class="col-md-1">
                <div class="form-group">
                    <span class="details">VENC. MOOP:</span>
                    <asp:TextBox ID="TextBox1" runat="server" data-mask="00/00/0000" ForeColor="Blue" CssClass="form-control" value=""></asp:TextBox>
                </div>
            </div>
            <div class="col-md-2">
                <div class="form-group">
                    <span class="details">FIXO:</span>
                    <asp:TextBox ID="txtFixo" runat="server" data-mask="(00) 0000-0000" ForeColor="Blue" CssClass="form-control" placeholder=""></asp:TextBox>
                </div>
            </div>
            <div class="col-md-2">
                <div class="form-group">
                    <span class="details">CELULAR:</span>
                    <asp:TextBox ID="txtCelular" runat="server" data-mask="(00) 0 0000-0000" ForeColor="Blue" CssClass="form-control" placeholder=""></asp:TextBox>
                </div>
            </div>
            <div class="col-md-2">
                <div class="form-group">
                    <span class="details">CRACHÁ:</span>
                    <asp:TextBox ID="txtCracha" runat="server" ForeColor="Blue" CssClass="form-control" value=""></asp:TextBox>
                </div>
            </div>

        </div>
        <!-- Linha 6 do formulário -->
        <div class="row g-3">
            <div class="col-md-1">
                <div class="form-group">
                    <span class="details">CEP:</span>
                    <asp:TextBox ID="txtCepCli" runat="server" ForeColor="Blue" CssClass="form-control" Width="130px" placeholder="99999-999" MaxLength="9"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-1">
                <br />
                <asp:Button ID="btnCep" runat="server" Text="Pesquisar" CssClass="btn btn-outline-warning" />
            </div>
        </div>
        <!-- Linha 7 do formulário -->
        <div class="row g-3">
            <div class="col-md-3">
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
            <div class="col-md-1">
                <div class="form-group">
                    <span class="details">COMPL.:</span>
                    <asp:TextBox ID="txtComplemento" Style="text-align: center" runat="server" ForeColor="Blue" CssClass="form-control" placeholder="" MaxLength="15"> </asp:TextBox>
                </div>
            </div>
            <div class="col-md-3">
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

        </div>
        <!-- Linha 8 do formulário -->
        <div class="row g-3">
            <div class="col-md-1">
                <div class="form-group">
                    <span class="details">CÓDIGO:</span>
                    <asp:TextBox ID="txtCodTra" runat="server" ForeColor="Blue" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="11" AutoPostBack="true"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-5">
                <div class="form_group">
                    <span class="details">PROPRIETÁRIO/TRANSPORTADORA:</span>
                    <asp:DropDownList ID="ddlAgregados" class="js-example-basic-single" name="nomeProprietario" runat="server" Width="520px" AutoPostBack="true"></asp:DropDownList>
                </div>
            </div>
            <div class="col-md-2">
                <div class="form-group">
                    <span class="details">CADASTRADO EM:</span>
                    <asp:Label ID="lblDtCadastro" runat="server" ForeColor="Blue" CssClass="form-control" placeholder=""></asp:Label>
                </div>
            </div>
            <div class="col-md-4">
                <div class="form-group">
                    <span class="details">CADASTRADO POR:</span>
                    <asp:TextBox ID="txtUsuCadastro" runat="server" ForeColor="Blue" CssClass="form-control" placeholder="" MaxLength="60"></asp:TextBox>
                </div>
            </div>
        </div>
        <!-- Linha 9 do formulário -->
        <div class="row g-3">            
            <div class="col-md-1">
                <br />
                <asp:Button ID="btnSalvar1" CssClass="btn btn-outline-success  btn-lg" runat="server" Text="Salvar" />
            </div>
            <div class="col-md-1">
                <br />
                <a href="ConsultaClientes.aspx" class="btn btn-outline-danger btn-lg">Cancelar               
                </a>
            </div>
        </div>
     </div>   
     <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.5.0/jquery.js"></script>
     <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery.mask/1.14.16/jquery.mask.js"></script>

     <link href="assets/plugins/global/plugins.bundle.css" rel="stylesheet" type="text/css" />
     <script src="assets/plugins/global/plugins.bundle.js"></script>

     <link href="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/css/select2.min.css" rel="stylesheet" />
     <script src="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/js/select2.min.js"></script>

     <script>
        $(document).ready(function () {
           $('.js-example-basic-single').select2();
        });
     </script>
</asp:Content>
