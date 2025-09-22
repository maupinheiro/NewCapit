<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="Frm_CadRotas.aspx.cs" Inherits="NewCapit.dist.pages.Frm_CadRotas" %>

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
            let txtDuracao = document.getElementById("<%= txtDuracao.ClientID %>");
            if (txtDuracao) aplicarMascara(txtDuracao, "000:00:00");
        });
    </script>
    <div class="content-wrapper">
        <!-- Main content -->
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
                        <h3 class="card-title"><i class="fas fa-map-marker-alt"></i>&nbsp;ROTAS - NOVO CADASTRO</h3>
                    </div>
                </div>
                <div class="card-header">
                    <form class="form-horizontal">
                        <div class="card-body">
                            <div class="form-group row">
                                <label for="inputFilial" class="col-sm-1 col-form-label" style="text-align: right">ROTA:</label>
                                <div class="col-sm-1">
                                    <asp:TextBox ID="txtRota" runat="server" CssClass="form-control" Style="text-align: center" ReadOnly="true"></asp:TextBox>
                                </div>
                                <%--<div class="col-sm-4">
                                </div>--%>
                                <label for="inputFilial" class="col-sm-2 col-form-label" style="text-align: right">DESCRIÇÃO DA ROTA:</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtDesc_Rota" runat="server" CssClass="form-control" Style="text-align: left" ReadOnly="true"></asp:TextBox>
                                </div>

                            </div>
                            <!-- REMETENTE -->
                            <div class="form-group row">
                                <label for="inputRemetente" class="col-sm-1 col-form-label" style="text-align: right">REMETENTE:</label>
                                <div class="col-md-1">
                                    <asp:TextBox ID="txtCodRemetente" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtCodRemetente_TextChanged"></asp:TextBox>
                                </div>
                                <div class="col-md-5">
                                    <asp:DropDownList ID="cboRemetente" runat="server" CssClass="form-control select2" AutoPostBack="true" OnSelectedIndexChanged="cboRemetente_SelectedIndexChanged"></asp:DropDownList>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtMunicipioRemetente" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="col-md-1">
                                    <asp:TextBox ID="txtUFRemetente" Style="text-align: center" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>

                            </div>
                            <!-- EXPEDIDOR -->
                            <div class="form-group row">
                                <label for="inputExpedidor" class="col-sm-1 col-form-label" style="text-align: right">EXPEDIDOR:</label>
                                <div class="col-md-1">
                                    <asp:TextBox ID="txtCodExpedidor" runat="server" CssClass="form-control" OnTextChanged="txtCodExpedidor_TextChanged" AutoPostBack="true"></asp:TextBox>
                                </div>
                                <div class="col-md-5">
                                    <asp:DropDownList ID="cboExpedidor" runat="server" CssClass="form-control select2" OnSelectedIndexChanged="cboExpedidor_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtCidExpedidor" runat="server" CssClass="form-control" ReadOnly ="true"></asp:TextBox>
                                </div>
                                <div class="col-md-1">
                                    <asp:TextBox ID="txtUFExpedidor" Style="text-align: center" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <!-- DESTINATARIO -->
                            <div class="form-group row">
                                <label for="inputDestinatario" class="col-sm-1 col-form-label" style="text-align: right">DESTINATÁRIO:</label>
                                <div class="col-md-1">
                                    <asp:TextBox ID="txtCodDestinatario" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtCodDestinatario_TextChanged"></asp:TextBox>
                                </div>
                                <div class="col-md-5">
                                    <asp:DropDownList ID="cboDestinatario" runat="server" CssClass="form-control select2" AutoPostBack="true" OnSelectedIndexChanged="cboDestinatario_SelectedIndexChanged"></asp:DropDownList>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtMunicipioDestinatario" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="col-md-1">
                                    <asp:TextBox ID="txtUFDestinatario" Style="text-align: center" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <!-- RECEBEDOR -->
                            <div class="form-group row">
                                <label for="inputRecebedor" class="col-sm-1 col-form-label" style="text-align: right">RECEBEDOR:</label>
                                <div class="col-md-1">
                                    <asp:TextBox ID="txtCodRecebedor" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtCodRecebedor_TextChanged"></asp:TextBox>
                                </div>
                                <div class="col-md-5">
                                    <asp:DropDownList ID="cboRecebedor" runat="server" CssClass="form-control select2" AutoPostBack="true" OnSelectedIndexChanged="cboRecebedor_SelectedIndexChanged"></asp:DropDownList>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtCidRecebedor" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="col-md-1">
                                    <asp:TextBox ID="txtUFRecebedor" Style="text-align: center" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </form>
                    <div class="row g-3">
                        <div class="col-md-2">
                            <div class="form-group">
                                <label for="inputDistancia" class="col-sm-2 col-form-label">DISTÂNCIA(KM):</label>
                                <asp:TextBox ID="txtDistancia" class="form-control" Style="text-align: center" runat="server" ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form-group">
                                <label for="inputDuracao" class="col-sm-1 col-form-label">DURAÇÃO:</label>
                                <asp:TextBox ID="txtDuracao" class="form-control" Style="text-align: center" runat="server" placeholder="000:00:00"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form-group">
                                <label for="inputDistancia" class="col-sm-2 col-form-label">DESLOCAMENTO:</label>
                                <asp:DropDownList ID="cboDeslocamento" runat="server" CssClass="form-control" ReadOnly="true">
                                    <asp:ListItem Value="" Text="Selecione..."></asp:ListItem>
                                    <asp:ListItem Value="MUNICIPAL" Text="MUNICIPAL"></asp:ListItem>
                                    <asp:ListItem Value="INTERMUNICIAPAL" Text="INTERMUNICIAPAL"></asp:ListItem>
                                    <asp:ListItem Value="INTERESTADUAL" Text="INTERESTADUAL"></asp:ListItem>
                                    <asp:ListItem Value="INTERNACIONAL" Text="INTERNACIONAL"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form-group">
                                <label for="inputFilial" class="col-sm-1 col-form-label" style="text-align: right">STATUS:</label>
                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control" ReadOnly="true">
                                    <asp:ListItem Value="ATIVA" Text="ATIVO"></asp:ListItem>
                                    <asp:ListItem Value="INATIVA" Text="INATIVO"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form-group">
                                <label for="inputFilial" class="col-sm-1 col-form-label" style="text-align: right">CADASTRO:</label>
                                <asp:TextBox ID="txtCadastro" runat="server" CssClass="form-control" Style="text-align: center" ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>


                    </div>
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
                                <asp:TextBox ID="txtUsuCadastro" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-md-2">
                            <div class="form-group">
                                <span class="details">ATUALIZADO EM:</span>
                                <asp:Label ID="lbDtAtualizacao" runat="server" CssClass="form-control" placeholder="" readonly="true"></asp:Label>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <span class="details">POR:</span>
                                <asp:TextBox ID="txtAltCad" runat="server" CssClass="form-control" placeholder="" ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="row g-3">
                        <div class="col-md-1">
                            <asp:Button ID="btnAlterar" runat="server" CssClass="btn btn-outline-success btn-lg" Text="Atualizar" />
                        </div>
                        <div class="col-md-1">
                            <a href="ConsultaRotas.aspx" class="btn btn-outline-danger btn-lg">Sair               
                            </a>
                        </div>
                    </div>
                </div>
        </section>
    </div>

</asp:Content>
