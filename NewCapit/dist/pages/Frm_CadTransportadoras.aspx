<%@ Page Title="" Language="C#" MasterPageFile="Main.Master" AutoEventWireup="true" CodeBehind="Frm_CadTransportadoras.aspx.cs" Inherits="NewCapit.dist.pages.Frm_CadTransportadoras" %>

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

            let txtFixo = document.getElementById("<%= txtFixo.ClientID %>");
    let txtCelular = document.getElementById("<%= txtCelular.ClientID %>");

        if (txtFixo) aplicarMascara(txtFixo, "(00) 0000-0000");
        if (txtFixo) aplicarMascara(txtCelular, "(00) 0 0000-0000");
    });
    </script>
    <div class="content-wrapper">
        <section class="content">
            <div class="container-fluid">
                <br />
                <div class="card card-info">
                    <div class="card-header">
                        <h3 class="card-title">AGREGADOS/TERCEIROS - NOVO CADASTRO</h3>
                    </div>
                </div>
                <div class="card-header">
                    <div class="row g-3">
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="details">CÓDIGO:</span>
                                <asp:TextBox ID="txtCodTra" runat="server" CssClass="form-control" placeholder="" MaxLength="11"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <br />
                            <asp:Button ID="btnAgregado" runat="server" Text="Pesquisar" CssClass="btn btn-outline-warning" OnClick="btnAgregado_Click" />
                        </div>
                        <div class="col-md-7">
                            <div class="form_group">
                                <span class="details">PESSOA:</span>
                                <asp:DropDownList ID="cboPessoa" runat="server" CssClass="form-control" Width="250px" AutoPostBack="true" OnSelectedIndexChanged="OnSelectedIndexChanged">
                                    <asp:ListItem Text="Selecione..." Value="0" />
                                    <asp:ListItem Value="FÍSICA" Text="FÍSICA"></asp:ListItem>
                                    <asp:ListItem Value="JURÍDICA" Text="JURÍDICA"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="form_group">
                                <span class="details">FILIAL:</span>
                                <asp:DropDownList ID="cbFiliais" name="nomeFiliais" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="row g-3">
                        <div class="col-md-2">
                            <div class="form-group">
                                <span class="details">CPF/CNPJ:</span>
                                <asp:TextBox ID="txtCpf_Cnpj" runat="server" CssClass="form-control" placeholder=""></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <br />
                            <asp:Button ID="btnCnpj" runat="server" Text="Pesquisar" CssClass="btn btn-outline-warning" OnClick="btnCnpj_Click" />
                        </div>
                        <div class="col-md-5">
                            <div class="form-group">
                                <span class="details">PROPRIETÁRIO/RAZÃO SOCIAL:</span>
                                <asp:TextBox ID="txtRazCli" runat="server" CssClass="form-control" value=""></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form-group">
                                <span class="details">ANTT/RNTRC:</span>
                                <asp:TextBox ID="txtAntt" runat="server" CssClass="form-control" value=""></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="details">CADASTRO:</span>
                                <asp:Label ID="txtDtCadastro" runat="server" CssClass="form-control" value=""></asp:Label>
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
                    <div class="row g-3">
                        <div class="col-md-2">
                            <div class="form-group">
                                <span class="details">RG/INSC. ESTADUAL:</span>
                                <asp:TextBox ID="txtRg" runat="server" CssClass="form-control" placeholder=""></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <span class="details">NOME FANTASIA:</span>
                                <asp:TextBox ID="txtFantasia" runat="server" CssClass="form-control" placeholder=""></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form-group">
                                <span class="details">CONTATO:</span>
                                <asp:TextBox ID="txtContato" runat="server" CssClass="form-control" placeholder=""></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form-group">
                                <span class="details">FIXO:</span>
                                <asp:TextBox ID="txtFixo" runat="server" CssClass="form-control" placeholder=""></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form-group">
                                <span class="details">CELULAR:</span>
                                <asp:TextBox ID="txtCelular" runat="server" CssClass="form-control" placeholder=""></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="row g-3">
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="details">CEP:</span>
                                <asp:TextBox ID="txtCepCli" runat="server" CssClass="form-control" placeholder="99999-999" MaxLength="9"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <br />
                            <asp:Button ID="btnCep" runat="server" Text="Pesquisar" CssClass="btn btn-outline-warning" OnClick="btnCep_Click" />
                        </div>
                    </div>
                    <div class="row g-3">
                        <div class="col-md-9">
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
                                <span class="details">COMPLEMENTO:</span>
                                <asp:TextBox ID="txtComplemento" Style="text-align: center" runat="server" CssClass="form-control" placeholder="" MaxLength="15"> </asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="row g-3">
                        <div class="col-md-3">
                            <div class="form-group">
                                <span class="details">BAIRRO:</span>
                                <asp:TextBox ID="txtBaiCli" runat="server" CssClass="form-control" placeholder="" MaxLength="60"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-3">
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
                        <div class="col-md-2">
                            <div class="form-group">
                                <span class="details">CADASTRADO EM:</span>
                                <asp:TextBox ID="txtDtUsu" Style="text-align: center" runat="server" CssClass="form-control" placeholder="" MaxLength="16"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="form-group">
                                <span class="details">CADASTRADO POR:</span>
                                <asp:TextBox ID="txtUsuCadastro" Style="text-align: left" runat="server" CssClass="form-control" placeholder="" MaxLength="2"></asp:TextBox>
                            </div>
                        </div>

                    </div>
                    <div class="row g-3">
                        <div class="col-md-1">
                            <br />
                            <asp:Button ID="btnSalvar" CssClass="btn btn-outline-success  btn-lg" runat="server" Text="Cadastrar" OnClick="btnSalvar_Click" />
                        </div>
                        <div class="col-md-1">
                            <br />
                            <a href="Consulta_Agregados.aspx" class="btn btn-outline-danger btn-lg">Sair               
                            </a>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>
    <footer class="main-footer">
        <div class="float-right d-none d-sm-block">
            <b>Version</b> 2.1.0
  
        </div>
        <strong>Copyright &copy; 2021-2025 Capit Logística.</strong> Todos os direitos reservados.
    </footer>
   
    <!-- Page specific script -->
    <script>
        $(function () {
            bsCustomFileInput.init();
        });
 </script>

</asp:Content>
