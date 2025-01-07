<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Frm_CadAgregados.aspx.cs" Inherits="NewCapit.Frm_CadAgregados" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-5">
        <h3>..: NOVO CADASTRO :..</h3>
        <hr />
        <div class="row g-3">
            <div class="col-md-3">
                <div class="form-group">
                    <span class="">FILIAL:</span>
                    <asp:DropDownList ID="ddlCombo" runat="server" ForeColor="Blue" CssClass="form-control">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-md-1">
                <div class="form-group">
                    <span class="details">CÓDIGO:</span>
                    <asp:TextBox ID="txtCodCli" runat="server" ForeColor="Blue" CssClass="form-control" placeholder="" MaxLength="11"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-1">
                <br />
                <asp:Button ID="btnCliente" runat="server" Text="Pesquisar" CssClass="btn btn-outline-warning" />
            </div>
            <div class="col-md-4">
                <div class="form-group">
                    <span class="">PESSOA:</span>
                    <asp:DropDownList ID="DropDownList1" runat="server" ForeColor="Blue" Width="260px" CssClass="form-control">                        
                        <asp:ListItem Value="FÍSICA" Text="FÍSICA"></asp:ListItem>
                        <asp:ListItem Value="JURÍDICA" Text="JURÍDICA"></asp:ListItem>
                    </asp:DropDownList></>
                </div>
            </div>
            <div class="col-md-1">
                <div class="form-group">
                    <span class="details">CADASTRO:</span>
                    <asp:Label ID="lblDataAtual" runat="server" ForeColor="Blue" width="130px" CssClass="form-control" placeholder="" MaxLength="10"></asp:Label>
                </div>
            </div>

            <div class="col-md-2">
                <div class="form-group">
                    <span class="">STATUS:</span>
                    <asp:DropDownList ID="status" runat="server" ForeColor="Blue" CssClass="form-control">
                        <asp:ListItem Value="ATIVO" Text="ATIVO"></asp:ListItem>
                        <asp:ListItem Value="INATIVO" Text="INATIVO"></asp:ListItem>
                    </asp:DropDownList></>
                </div>
            </div>
        </div>

        <div class="row g-3">
            <div class="col-md-2">
                <div class="form-group">
                    <span class="details" id="cpfoucnpj">CPF:</span>
                    <asp:TextBox ID="txtCnpjCpf" data-mask="000.000.000-00" ForeColor="Blue" runat="server" class="form-control"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-1">
                <br />
                <asp:Button ID="btnCnpj" runat="server" Text="Pesquisar" CssClass="btn btn-outline-warning" />
            </div>
            <div class="col-md-6">
                <div class="form-group">
                    <span class="details">RAZÃO SOCIAL:</span>
                    <asp:TextBox ID="txtRazCli" runat="server" ForeColor="Blue" CssClass="form-control" value=""></asp:TextBox>
                </div>
            </div>
            <div class="col-md-1">
                <div class="form-group">
                    <span class="details">TIPO:</span>
                    <asp:TextBox ID="txtTipo" runat="server" ForeColor="Blue" CssClass="form-control" value=""></asp:TextBox>
                </div>
            </div>
            <div class="col-md-1">
                <div class="form-group">
                    <span class="details">ABERTURA:</span>
                    <asp:TextBox ID="txtAbertura" runat="server" ForeColor="Blue" CssClass="form-control" Width="130px" value=""></asp:TextBox>
                </div>
            </div>
            <div class="col-md-1">
                <div class="form-group">
                    <span class="details">SITUAÇÃO:</span>
                    <asp:TextBox ID="txtSituacao" runat="server" ForeColor="Blue" CssClass="form-control" value=""></asp:TextBox>
                </div>
            </div>

        </div>

        <div class="row g-3">
            <div class="col-md-3">
                <div class="form-group">
                    <span class="details">NOME FANTASIA:</span>
                    <asp:TextBox ID="txtNomCli" runat="server" CssClass="form-control" placeholder="" MaxLength="50" ForeColor="Blue"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-2">
                <div class="form-group">
                    <span class="details">INSC. ESTADUAL:</span>
                    <asp:TextBox ID="txtInscEstadual" runat="server" ForeColor="Blue" CssClass="form-control" placeholder="" MaxLength="15"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-2">
                <div class="form-group">
                    <span class="details">CONTATO:</span>
                    <asp:TextBox ID="txtConCli" runat="server" ForeColor="Blue" CssClass="form-control" placeholder="" MaxLength="30"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-2">
                <div class="form-group">
                    <span class="details">FONE FIXO:</span>
                    <asp:TextBox ID="txtTc1Cli" runat="server" ForeColor="Blue" CssClass="form-control" data-mask="(00) 0000-0000" MaxLength="15"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-2">
                <div class="form-group">
                    <span class="details">CELULAR:</span>
                    <asp:TextBox ID="txtTc2Cli" runat="server" ForeColor="Blue" CssClass="form-control" data-mask="(00) 0 0000-0000" MaxLength="16"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row g-3">

            <div class="col-md-7">
                <div class="form-group">
                    <span class="details">E-MAIL(S):</span>
                    <asp:TextBox ID="txtEmail" runat="server" ForeColor="Blue" CssClass="form-control" placeholder="" MaxLength="200"></asp:TextBox>
                </div>
            </div>
        </div>
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
            <div class="col-md-7">
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
            <div class="col-md-2">
                <div class="form-group">
                    <span class="details">COMPLEMENTO:</span>
                    <asp:TextBox ID="txtComplemento" Style="text-align: center" runat="server" ForeColor="Blue" CssClass="form-control" placeholder="" MaxLength="15"> </asp:TextBox>
                </div>
            </div>
        </div>

        <div class="row g-3">
            <div class="col-md-2">
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
        <div class="row g-3">
            <div class="col-md-2">
                <div class="form-group">
                    <span class="details">CADASTRADO EM:</span>
                    <asp:Label ID="lblDtCadastro" runat="server" ForeColor="Blue" CssClass="form-control" placeholder="" maxlength="20"></asp:Label>
                </div>
            </div>
            <div class="col-md-10">
                <div class="form-group">
                    <span class="details">POR:</span>
                    <asp:TextBox ID="txtUsuCadastro" runat="server" ForeColor="Blue" CssClass="form-control" placeholder="" MaxLength="60"></asp:TextBox>
                </div>
            </div>
        </div>

        <div class="row g-3">
            <div class="col-md-1">
                <br />
                <asp:Button ID="btnSalvar1" CssClass="btn btn-outline-success  btn-lg" runat="server" Text="Salvar" />
            </div>
            <div class="col-md-1">
                <br />
                <a href="ConsultaAgregados.aspx" class="btn btn-outline-danger btn-lg">Cancelar               
                </a>
            </div>
        </div>

    </div>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.5.0/jquery.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery.mask/1.14.16/jquery.mask.js"></script>

</asp:Content>
