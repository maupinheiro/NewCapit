<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="Frm_Rotas.aspx.cs" Inherits="NewCapit.dist.pages.Frm_Rotas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    <link href="https://cdn.jsdelivr.net/npm/select2@4.1.0/dist/css/select2.min.css" rel="stylesheet" />
    <script src="https://code.jquery.com/jquery-3.7.1.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/select2@4.1.0/dist/js/select2.min.js"></script>
    <script src="https://code.jquery.com/jquery-3.6.4.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/js/bootstrap.bundle.min.js"></script>
    <script>
        function mascaraDuracao(campo) {
            let valor = campo.value.replace(/\D/g, "");

            // limita tamanho se quiser (ex: até 9999 horas = 4 dígitos + 4 de mmss)
            if (valor.length > 8)
                valor = valor.substring(0, 8);

            if (valor.length <= 2) {
                campo.value = valor;
            }
            else if (valor.length <= 4) {
                campo.value = valor.substring(0, valor.length - 2) + ":" + valor.substring(valor.length - 2);
            }
            else {
                campo.value = valor.substring(0, valor.length - 4) + ":" +
                    valor.substring(valor.length - 4, valor.length - 2) + ":" +
                    valor.substring(valor.length - 2);
            }
        }
    </script>


    <div class="content-wrapper">

        <!-- Main content -->
        <%--<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />--%>
        <br />
        <div class="col-md-12">
            <div class="card card-info">
                <div class="card-header" style="background-color: #A020F0; font-weight: bold;">
                    <h3 class="card-title">
                        <h3 class="card-title"><i class="fas fa-route"></i>&nbsp;GERENCIAR ROTAS</h3>
                    </h3>
                    <div class="card-tools">
                        <button type="button" class="btn btn-tool" data-card-widget="maximize">
                            <i class="fas fa-expand"></i>
                        </button>
                        <button type="button" class="btn btn-tool" data-card-widget="collapse">
                            <i class="fas fa-minus"></i>
                        </button>
                      
                    </div>
                    <!-- /.card-tools -->
                </div>
                <br />
                <br />
                <br />
                <br />

                <div class="container-fluid mt-4">
                    <div id="divMsg" runat="server"
                        class="alert alert-info alert-dismissible fade show mt-3"
                        role="alert" style="display: none;">
                        <span id="lblMsgGeral" runat="server"></span>
                        <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
                    </div>
                    <div class="row justify-content-center">
                        <div class="col-12 col-xxl-10">

                            <div class="card shadow-lg border-0">
                                <div class="card-header text-white py-3" style="background-color: #A020F0; font-weight: bold;">
                                    <h5 class="mb-0 fw-bold">Atualizar Rota
                                    </h5>
                                </div>

                                <div class="card-body px-4 py-4">
                                    <div class="form-group row">
                                        <label for="inputFilial" class="col-sm-1 col-form-label" style="text-align: right">ROTA:</label>
                                        <div class="col-sm-1">
                                            <asp:TextBox ID="txtRota" runat="server" CssClass="form-control" Style="text-align: center" ReadOnly="true"></asp:TextBox>
                                        </div>

                                        <label for="inputFilial" class="col-sm-2 col-form-label" style="text-align: right">DESCRIÇÃO DA ROTA:</label>
                                        <div class="col-sm-3">
                                            <asp:TextBox ID="txtDesc_Rota" runat="server" CssClass="form-control" Style="text-align: left" ReadOnly="true"></asp:TextBox>
                                        </div>

                                        <label for="inputFilial" class="col-sm-1 col-form-label" style="text-align: right">CADASTRO:</label>
                                        <div class="col-sm-2">
                                            <asp:TextBox ID="txtCadastro" runat="server" CssClass="form-control" Style="text-align: center" ReadOnly="true"></asp:TextBox>
                                        </div>

                                        <label for="inputFilial" class="col-sm-1 col-form-label" style="text-align: right">STATUS:</label>
                                        <div class="col-sm-1">
                                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                                                <asp:ListItem Value="ATIVA" Text="ATIVO"></asp:ListItem>
                                                <asp:ListItem Value="INATIVA" Text="INATIVO"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>



                                    </div>

                                    <hr />
                                    <h6 class="fw-bold text-secondary mb-3">DADOS:</h6>
                                    <div class="row align-items-end mb-12">
                                        <div class="form-group row">
                                            <label for="inputExpedidor" class="col-sm-1 col-form-label" style="text-align: right">Deslocamento:</label>
                                            <div class="col-md-2">
                                                <asp:DropDownList ID="cboDeslocamento" runat="server" CssClass="form-control">
                                                    <asp:ListItem Value="" Text="Selecione..."></asp:ListItem>
                                                    <asp:ListItem Value="MUNICIPAL" Text="MUNICIPAL"></asp:ListItem>
                                                    <asp:ListItem Value="INTERMUNICIAPAL" Text="INTERMUNICIAPAL"></asp:ListItem>
                                                    <asp:ListItem Value="INTERESTADUAL" Text="INTERESTADUAL"></asp:ListItem>
                                                    <asp:ListItem Value="INTERNACIONAL" Text="INTERNACIONAL"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row align-items-end mb-12">
                                        <div class="form-group row">
                                            <label for="inputExpedidor" class="col-sm-1 col-form-label" style="text-align: right">Distância(KM):</label>
                                            <div class="col-md-2">
                                                <asp:TextBox ID="txtDistancia" class="form-control" Style="text-align: center" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row align-items-end mb-12">
                                        <div class="form-group row">
                                            <label for="inputExpedidor" class="col-sm-1 col-form-label" style="text-align: right">Transit Time:</label>
                                            <div class="col-md-2">
                                                <asp:TextBox ID="txtDuracao" 
                                                    CssClass="form-control" 
                                                    Style="text-align: center"
                                                    runat="server"
                                                    onkeyup="mascaraDuracao(this)"
                                                    onkeypress="return event.charCode >= 48 && event.charCode <= 57">
                                                </asp:TextBox>

                                            </div>
                                        </div>
                                    </div>
                                    <div class="row align-items-end mb-12">
                                        <div class="form-group row">
                                            <label for="inputExpedidor" class="col-sm-1 col-form-label" style="text-align: right">Pedagiada:</label>
                                            <div class="col-md-2">
                                                <asp:DropDownList ID="ddlPedagio" runat="server" CssClass="form-control">
                                                    <asp:ListItem Value="SIM" Text="SIM"></asp:ListItem>
                                                    <asp:ListItem Value="NÃO" Text="NÃO"></asp:ListItem>
                                                </asp:DropDownList>
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
                                    <hr />
                                    <div class="row g-3">
                                    <div class="col-md-2">
                                        <asp:Button ID="btnAlterar" runat="server" OnClick="btnAlterar_Click" CssClass="btn btn-outline-success btn-lg w-100" Text="Atualizar" />
                                    </div>
                                    <div class="col-md-2">
                                        <a href="ConsultaRotas.aspx" class="btn btn-outline-danger btn-lg w-100">Fechar               
                                        </a>
                                    </div>
                                </div>
                                </div>
                            </div>
                            
                        </div>
                    </div>
                </div>
            </div>
        </div>       
    </div>   
</asp:Content>
