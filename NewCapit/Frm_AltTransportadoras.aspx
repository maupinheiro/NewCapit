<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Frm_AltTransportadoras.aspx.cs" Inherits="NewCapit.Frm_AltTransportadoras" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container mt-5">
        <div class="d-sm-flex align-items-center justify-content-between mb-4">
            <h3 class="h3 mb-2 text-gray-800"><i class="fas fa-user-friends"></i> TRANSPORTADOR
            </h3>
            <h3>ATUALIZA CADASTRO</h3>
        </div>

        <hr />
        <div class="row g-3">
            <div class="col-md-1">
                <div class="form-group">
                    <span class="details">CÓDIGO:</span>
                    <asp:TextBox ID="txtCodTra" runat="server" ForeColor="Blue" CssClass="form-control" placeholder="" MaxLength="11"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-1">
                <br />
                <asp:Button ID="btnAgregado" runat="server" Text="Pesquisar" CssClass="btn btn-outline-warning" OnClick="btnAgregado_Click" />
            </div>
            <div class="col-md-2">
                <div class="form_group">
                    <span class="details">PESSOA:</span>
                    <asp:DropDownList ID="cboPessoa" runat="server" ForeColor="Blue" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="OnSelectedIndexChanged"> 
                        <asp:ListItem Value="" Text="SELECIONE"></asp:ListItem>
                        <asp:ListItem Value="FÍSICA" Text="FÍSICA"></asp:ListItem>
                        <asp:ListItem Value="JURÍDICA" Text="JURÍDICA"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-md-5">
                <div class="form_group">
                    <span class="details">TIPO:</span>
                    <asp:DropDownList ID="ddlTipo" runat="server" ForeColor="Blue" CssClass="form-control" Width="250px">      
                        <asp:ListItem Value="" Text="SELECIONE"></asp:ListItem>
                        <asp:ListItem Value="AGREGADO" Text="AGREGADO"></asp:ListItem>
                        <asp:ListItem Value="TERCEIRO" Text="TERCEIRO"></asp:ListItem>
                        <asp:ListItem Value="EMPRESA" Text="EMPRESA"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-md-3">
                <div class="form_group">
                    <span class="details">FILIAL:</span>
                    <asp:DropDownList ID="cbFiliais" runat="server" ForeColor="Blue" CssClass="form-control"></asp:DropDownList>
                </div>
            </div>
        </div>

        <div class="row g-3">
            <div class="col-md-2">
                <div class="form-group">
                    <span class="details">CPF/CNPJ:</span>
                    <asp:TextBox ID="txtCpf_Cnpj" runat="server" ForeColor="Blue" CssClass="form-control" placeholder=""></asp:TextBox>
                </div>
            </div>
            <div class="col-md-1">
                <br />
                <asp:Button ID="btnCnpj" runat="server" Text="Pesquisar" CssClass="btn btn-outline-warning" OnClick="btnCnpj_Click" />
            </div>
            <div class="col-md-5">
                <div class="form-group">
                    <span class="details">PROPRIETÁRIO/RAZÃO SOCIAL:</span>
                    <asp:TextBox ID="txtRazCli" runat="server" ForeColor="Blue" CssClass="form-control" value=""></asp:TextBox>
                </div>
            </div>
            <div class="col-md-2">
                <div class="form-group">
                    <span class="details">ANTT/RNTRC:</span>
                    <asp:TextBox ID="txtAntt" runat="server" ForeColor="Blue" CssClass="form-control" value=""></asp:TextBox>
                </div>
            </div>
            <div class="col-md-1">
                <div class="form-group">
                    <span class="details">CADASTRO:</span>
                    <asp:Label ID="txtDtCadastro" runat="server" ForeColor="Blue"  width="120px" CssClass="form-control" value=""></asp:Label>
                </div>
            </div>
            <div class="col-md-1">
                <div class="form_group">
                    <span class="details">SITUAÇÃO:</span>
                    <asp:DropDownList ID="ddlSituacao" runat="server" ForeColor="Blue" CssClass="form-control">
                        <asp:ListItem Value="ATIVO" Text="ATIVO"></asp:ListItem>
                        <asp:ListItem Value="INATIVO" Text="INATIVO"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
        </div>
        <div class="row g-3">
            <div class="col-md-2">
                <div class="form-group">
                    <span class="details">RG/INSC. ESTADUAL:</span>
                    <asp:TextBox ID="txtRg" runat="server" ForeColor="Blue" CssClass="form-control" placeholder=""></asp:TextBox>
                </div>
            </div>
            <div class="col-md-4">
                <div class="form-group">
                    <span class="details">NOME FANTASIA:</span>
                    <asp:TextBox ID="txtFantasia" runat="server" ForeColor="Blue" CssClass="form-control" placeholder=""></asp:TextBox>
                </div>
            </div>
            <div class="col-md-2">
                <div class="form-group">
                    <span class="details">CONTATO:</span>
                    <asp:TextBox ID="txtContato" runat="server" ForeColor="Blue" CssClass="form-control" placeholder=""></asp:TextBox>
                </div>
            </div>
            <div class="col-md-2">
                <div class="form-group">
                    <span class="details">FIXO:</span>
                    <asp:TextBox ID="txtFixo" Text="" runat="server" data-mask="(00) 0000-0000" ForeColor="Blue" CssClass="form-control" placeholder=""></asp:TextBox>
                </div>
            </div>
            <div class="col-md-2">
                <div class="form-group">
                    <span class="details">CELULAR:</span>
                    <asp:TextBox ID="txtCelular" runat="server" data-mask="(00) 0 0000-0000" ForeColor="Blue" CssClass="form-control" placeholder=""></asp:TextBox>
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
                <asp:Button ID="btnCep" runat="server" Text="Pesquisar" CssClass="btn btn-outline-warning" OnClick="btnCep_Click" />
            </div>
            <div class="col-md-6">
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
            <div class="col-md-3">
                <div class="form-group">
                    <span class="details">COMPLEMENTO:</span>
                    <asp:TextBox ID="txtComplemento" Style="text-align: center" runat="server" ForeColor="Blue" CssClass="form-control" placeholder="" MaxLength="15"> </asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row g-3">
            <div class="col-md-5">
                <div class="form-group">
                    <span class="details">BAIRRO:</span>
                    <asp:TextBox ID="txtBaiCli" runat="server" ForeColor="Blue" CssClass="form-control" placeholder="" MaxLength="60"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-6">
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
                    <asp:TextBox ID="txtDtCad" Style="text-align: center" runat="server" ForeColor="Blue" CssClass="form-control" placeholder="" MaxLength="16"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-4">
                <div class="form-group">
                    <span class="details">CADASTRADO POR:</span>
                    <asp:TextBox ID="txtUsuCad" Style="text-align: left" runat="server" ForeColor="Blue" CssClass="form-control" placeholder="" MaxLength="2"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-2">
                <div class="form-group">
                    <span class="details">ÚLTIMA ATUALIZAÇÃO::</span>
                    <asp:TextBox ID="txtAltDtUsu" Style="text-align: center" runat="server" ForeColor="Blue" CssClass="form-control" placeholder="" MaxLength="16"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-4">
                <div class="form-group">
                    <span class="details">ATUALIZADO POR:</span>
                    <asp:TextBox ID="txtUsuAltCadastro" Style="text-align: left" runat="server" ForeColor="Blue" CssClass="form-control" placeholder="" MaxLength="2"></asp:TextBox>
                </div>
            </div>

        </div>

        <div class="row g-3">
            <br />
            <div class="col-md-1">
                <asp:Button ID="btnAtualizar" CssClass="btn btn-outline-success btn-lg" runat="server" Text="Atualizar" OnClick="btnSalvar_Click" />
            </div>           
            <div class="col-md-1">                
                <a href="ConsultaAgregados.aspx" class="btn btn-outline-danger btn-lg">Cancelar               
                </a>
            </div>
        </div>

    </div>

    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.5.0/jquery.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery.mask/1.14.16/jquery.mask.min.js"></script>



</asp:Content>
