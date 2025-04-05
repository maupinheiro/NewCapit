<%@ Page Title="" Language="C#" MasterPageFile="Main.Master" AutoEventWireup="true" CodeBehind="Frm_CadMotoristas.aspx.cs" Inherits="NewCapit.dist.pages.Frm_CadMotoristas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   

    <div class="content-wrapper">
        
            <div class="container-fluid">
                
                <div class="card card-info">
                    <div class="card-header">
                        <h3 class="card-title"><i class="fas fa-address-card"></i>&nbsp;MOTORISTAS - NOVO CADASTRO</h3>
                    </div>
                </div>
            </div>
            <div class="card-header">
                    <!-- Linha 1 do formulario -->
                    <div class="row g-3">
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="details">CÓDIGO:</span>
                                <asp:TextBox ID="txtCodMot" runat="server" class="form-control" placeholder="" MaxLength="6"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form-group">
                                <span class="">TIPO MOTORISTA:</span>
                                <asp:DropDownList ID="ddlTipoMot" runat="server" CssClass="form-control">
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
                                <asp:DropDownList ID="ddlCargo" name="descricaoCargo" runat="server" CssClass="form-control select2"></asp:DropDownList>
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
                                <asp:DropDownList ID="cbFiliais" name="nomeFiliais" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-2">
                        </div>
                        <div class="col-md-1">
                            <div class="form-group">
                                <img src="/fotos/usuario.jpg" class="rounded float-right" alt="...">
                            </div>
                        </div>

                    </div>
                    <!-- Linha 2 do formulario -->
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
                                    <input type="text" id="txtDtNasc" class="form-control" data-inputmask-alias="datetime" data-inputmask-inputformat="dd/mm/yyyy" data-mask>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form_group">
                                <span class="details">UF NASC.:</span>
                                <asp:DropDownList ID="ddlUF" name="ufNascimento" runat="server" class="form-control"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="form_group">
                                <span class="details">MUNICIPIO DE NASCIMENTO:</span>
                                <asp:DropDownList ID="ddlMunicipioNasc" class="form-control select2" OnSelectedIndexChanged="ddlMunicBrasileiros_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="details">REGIÃO DO PAIS:</span>
                                <asp:TextBox ID="txtRegiao" runat="server" class="form-control" placeholder=""></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form_group">
                                <span class="details">ADM./CAD.:</span>
                                <asp:TextBox ID="txtDtCad" runat="server" Style="text-align: left" CssClass="form-control" placeholder="" MaxLength="10"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="">SITUAÇÃO:</span>
                                <asp:DropDownList ID="ddlTipo" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="ATIVO" Text="ATIVO"></asp:ListItem>
                                    <asp:ListItem Value="INATIVO" Text="INATIVO"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <!-- Linha 3 do formulario -->
                    <div class="row g-3">
                        <div class="col-md-2">
                            <div class="form-group">
                                <span class="details">CPF:</span>
                                <div class="input-group">
                                    <input type="text" id="txtCPF" class="form-control" data-inputmask='"mask": "999.999.999-99"' data-mask>
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
                                    <input type="text" id="txtDtEmissao" class="form-control" data-inputmask-alias="datetime" data-inputmask-inputformat="dd/mm/yyyy" data-mask>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form-group">
                                <span class="details">CARTÃO PAMCARD:</span>
                                <div class="input-group">
                                    <input type="text" id="txtCartao" class="form-control" data-inputmask='"mask": "9999 9999 9999 9999"' data-mask>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="details">MÊS/ANO:</span>
                                <div class="input-group">
                                    <input type="text" id="txtValCartao" class="form-control" data-inputmask='"mask": "99/9999"' data-mask>
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
                                    <input type="text" id="txtPIS" class="form-control">
                                </div>
                            </div>
                        </div>

                    </div>
                    <!-- Linha 4 do formulário -->
                    <div class="row g-3">
                        <div class="col-md-2">
                            <div class="form-group">
                                <span class="details">Nº CNH:</span>
                                <asp:TextBox ID="txtRegCNH" runat="server" Style="text-align: center" CssClass="form-control" value=""></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form-group">
                                <span class="details">Nº FORM DA CNH:</span>
                                <asp:TextBox ID="txtFormCNH" runat="server" Style="text-align: center" CssClass="form-control" value=""></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form-group">
                                <span class="details">CÓDIGO DE SEGURANÇA:</span>
                                <asp:TextBox ID="txtCodSeguranca" runat="server" Style="text-align: center" CssClass="form-control" value=""></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="">CATEGORIA:</span>
                                <asp:DropDownList ID="ddlCat" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="AE" Text="AE"></asp:ListItem>
                                    <asp:ListItem Value="D" Text="D"></asp:ListItem>
                                    <asp:ListItem Value="E" Text="E"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="details">VALIDADE CNH:</span>
                                <input type="text" id="txtValCNH" class="form-control" data-inputmask-alias="datetime" data-inputmask-inputformat="dd/mm/yyyy" data-mask>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="details">UF CNH:</span>
                                <asp:DropDownList ID="ddlCNH" name="ufCNH" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlCNH_SelectedIndexChanged"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="form-group">
                                <span class="details">MUNICIPIO:</span>
                                <asp:DropDownList ID="ddlMunicCnh" class="form-control select2" AutoPostBack="true" runat="server"></asp:DropDownList>
                            </div>
                        </div>


                    </div>
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
                                <span class="">SEXO:</span>
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
                                <asp:TextBox ID="txtCodTra" runat="server" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="11" AutoPostBack="true"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-5">
                            <div class="form_group">
                                <span class="details">PROPRIETÁRIO/TRANSPORTADORA:</span>
                                <asp:DropDownList ID="ddlAgregados" class="form-control select2" name="nomeProprietario" runat="server" AutoPostBack="true"></asp:DropDownList>
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
                                    <input type="text" id="txtValLibRisco" class="form-control" data-inputmask-alias="datetime" data-inputmask-inputformat="dd/mm/yyyy" data-mask>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="details">VENC. EX. TOXIC.:</span>
                                <div class="input-group">
                                    <input type="text" id="txtVAlExameTox" class="form-control" data-inputmask-alias="datetime" data-inputmask-inputformat="dd/mm/yyyy" data-mask>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="details">VENC. MOOP:</span>
                                <div class="input-group">
                                    <input type="text" id="txtVAlMoop" class="form-control" data-inputmask-alias="datetime" data-inputmask-inputformat="dd/mm/yyyy" data-mask>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form-group">
                                <span class="details">FIXO:</span>
                                <div class="input-group">
                                    <input type="text" id="txtFixo" class="form-control" data-inputmask='"mask": "(99) 9999-9999"' data-mask>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form-group">
                                <span class="details">CELULAR:</span>
                                <div class="input-group">
                                    <input type="text" id="txtCelular" class="form-control" data-inputmask='"mask": "(99) 9 9999-9999"' data-mask>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-1">
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
                                <asp:TextBox ID="txtCepCli" runat="server" CssClass="form-control" Width="130px" placeholder="99999-999" MaxLength="9"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <br />
                            <asp:Button ID="btnCep" runat="server" Text="Pesquisar" CssClass="btn btn-outline-warning" />
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
                    <!-- Linha 8 do formulário -->
                    <div class="row g-3">
                        <div class="col-md-2">
                            <div class="form-group">
                                <span class="details">CADASTRADO EM:</span>
                                <asp:Label ID="lblDtCadastro" runat="server" CssClass="form-control" placeholder=""></asp:Label>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <span class="details">POR:</span>
                                <asp:TextBox ID="txtUsuCadastro" runat="server" CssClass="form-control" placeholder="" MaxLength="60"></asp:TextBox>
                            </div>
                        </div>
                  </div>
                    <!-- Linha 9 do formulário -->
                    <div class="row g-3">
                        <div class="col-md-1">                          
                            <asp:Button ID="btnSalvar1" CssClass="btn btn-outline-success  btn-lg" runat="server" Text="Salvar" />
                        </div>
                        <div class="col-md-1">                            
                            <a href="ConsultaClientes.aspx" class="btn btn-outline-danger btn-lg">Cancelar               
                            </a>
                        </div>
                    </div>
                </div>          
        
    </div>

    <footer class="main-footer">
        <div class="float-right d-none d-sm-block">
            <b>Version</b> 2.1.0  
        </div>
        <strong>Copyright &copy; 2021-2025 Capit Logística.</strong> Todos os direitos reservados.
    </footer>
  



</asp:Content>
