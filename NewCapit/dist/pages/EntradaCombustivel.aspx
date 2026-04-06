<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="EntradaCombustivel.aspx.cs" Inherits="NewCapit.dist.pages.EntradaCombustivel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- Bootstrap CSS -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/css/bootstrap.min.css" rel="stylesheet" />
    <!-- jQuery e Bootstrap JS -->
    <script src="https://code.jquery.com/jquery-3.6.4.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/js/bootstrap.bundle.min.js"></script>
    <!-- Bootstrap CSS + JS -->
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" />
    <script src="https://code.jquery.com/jquery-3.5.1.slim.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@4.5.2/dist/js/bootstrap.bundle.min.js"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
    <script>
        function formatarLitros(campo) {

            let valor = campo.value;

            if (!valor) return;

            valor = valor
                .replace(/\./g, "")
                .replace(",", ".");

            let numero = parseFloat(valor);

            if (!isNaN(numero)) {
                campo.value = numero.toLocaleString('pt-BR', {
                    minimumFractionDigits: 3,
                    maximumFractionDigits: 3
                });
            }
        }
</script>
    <script>
        function limparMoeda(valor) {
            if (!valor) return 0;

            return parseFloat(
                valor
                    .replace("R$", "")
                    .replace(/\s/g, "")
                    .replace(/\./g, "")
                    .replace(",", ".")
            ) || 0;
        }

        function formatarMoeda(campo) {
            let valor = limparMoeda(campo.value);

            campo.value = valor.toLocaleString('pt-BR', {
                style: 'currency',
                currency: 'BRL'
            });
        }

        function calcularValorUnitario() {

            let total = limparMoeda(document.getElementById('<%= txtValorTotalEntrada.ClientID %>').value);
            let litros = limparMoeda(document.getElementById('<%= txtLitrosEntrada.ClientID %>').value);

            if (litros > 0) {
                let unitario = total / litros;

                // Formata como número pt-BR com 3 casas decimais, sem R$
                document.getElementById('<%= txtValorUnitario.ClientID %>').value =
                    unitario.toLocaleString('pt-BR', {
                        minimumFractionDigits: 3,
                        maximumFractionDigits: 3
                    });
            }
        }
    </script>
    <script>
        function validarEstoqueEntrada() {
            let litrosEntrada = parseFloat(document.getElementById('<%= txtLitrosEntrada.ClientID %>').value.replace(",", ".")) || 0;
            let estoqueAtual = parseFloat(document.getElementById('<%= txtEstoqueAtual.ClientID %>').value.replace(",", ".")) || 0;
            let capacidade = parseFloat(document.getElementById('<%= txtCapTotal.ClientID %>').value.replace(",", ".")) || 0;

            if ((estoqueAtual + litrosEntrada) > capacidade) {
                alert(`⚠️ Não é possível registrar. Estoque atual (${estoqueAtual} L) + entrada (${litrosEntrada} L) ultrapassa a capacidade total do tanque (${capacidade} L).`);
                return false;
            }
            return true;
        }
    </script>
    <script>
        function formatarData(input) {
            let v = input.value.replace(/\D/g, '');

            if (v.length > 2) v = v.substring(0, 2) + '/' + v.substring(2);
            if (v.length > 5) v = v.substring(0, 5) + '/' + v.substring(5, 9);

            input.value = v;
        }
    </script>
    <script>
        function formatarMoeda(campo) {
            let valor = campo.value.replace(/\D/g, '');

            valor = (valor / 100).toFixed(2) + '';
            valor = valor.replace(".", ",");
            valor = valor.replace(/\B(?=(\d{3})+(?!\d))/g, ".");

            campo.value = valor;
        }        
    </script>
    <script>
        function abrirModalTanque() {
            var modal = new bootstrap.Modal(document.getElementById('modalTanque'));
            modal.show();
        }
    </script>
    <div class="content-wrapper">
        <div class="card card-info">
            <div class="card-header" style="background-color: #A020F0; font-weight: bold;">
                <h3 class="card-title"><i class="far fa-calendar-check"></i>&nbsp;Gestão de Postos
                   <br />
                   <small>Entrada de Combustível</small></h3>                
            </div>
        </div>
        <br />
        <br />
        <br />
        <br />
        <div class="container mt-4">
            <div id="divMsg" runat="server"
                class="alert alert-dismissible fade show mt-3"
                role="alert" visible="false">
                <asp:Label ID="lblMsgGeral" runat="server"></asp:Label>
                <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
            </div>
            <div class="card shadow">
                <div class="card-header bg-primary text-white">
                    <h5 class="mb-0">Entrada de Combustível</h5>
                </div>
                <div class="card-body">
                    <div class="form-row">
                        <div class="form-group col-md-7">
                            <label>Chave de Acesso NF:</label>
                            <asp:TextBox ID="txtChave" runat="server" CssClass="form-control" MaxLength="44" placeholder="Digite ou Scannei a chave de acesso da nota" Style="text-align: center" AutoPostBack="true" OnTextChanged="txtChave_TextChanged" />
                        </div>
                        <div class="form-group col-md-2">
                            <label>Emissão:</label>
                            <asp:TextBox ID="txtEmissaoNF" runat="server" CssClass="form-control" Style="text-align: center" MaxLength="10" onkeyup="formatarData(this)" />
                        </div>
                        <div class="form-group col-md-2">
                            <label>Nota Fiscal:</label>
                            <asp:TextBox ID="txtNF" runat="server" CssClass="form-control" ReadOnly="true" Style="text-align: center" />
                        </div>
                        <div class="form-group col-md-1">
                            <label>Serie:</label>
                            <asp:TextBox ID="txtSerieNF" runat="server" CssClass="form-control" ReadOnly="true" Style="text-align: center" />
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-group col-md-1">
                            <label>Fornec.:</label>
                            <asp:TextBox ID="txtCodFor" runat="server" CssClass="form-control" ReadOnly="true" />
                        </div>
                        <div class="form-group col-md-7">
                            <label>Razão Social:</label>
                            <asp:TextBox ID="txtRazSocial" runat="server" CssClass="form-control" ReadOnly="true" />
                        </div>
                        <div class="form-group col-md-4">
                            <label>CNPJ:</label>
                            <asp:TextBox ID="txtCNPJ" runat="server" CssClass="form-control" ReadOnly="true" />
                        </div>

                    </div>
                    <div class="form-row">
                        <div class="form-group col-md-1">
                            <label>Tanque:</label>
                            <asp:TextBox ID="txtIdTanque" runat="server" CssClass="form-control" OnTextChanged="txtIdTanque_TextChanged" AutoPostBack="true"/>
                        </div>
                        <div class="col-md-6">
                            <label>&nbsp;</label><br />
                            <asp:DropDownList ID="ddlTanqueEntrada" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlTanqueEntrada_SelectedIndexChanged" />
                        </div>
                        <div class="col-md-3">
                            <label>&nbsp;</label><br />
                            <asp:Button ID="btnAbrirModal" CssClass="btn btn-outline-success w-100" runat="server" Text="Cadastrar" OnClientClick="abrirModalTanque(); return false;" />
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-group col-md-1">
                            <label>Código:</label>
                            <asp:TextBox ID="txtIdCombustivel" runat="server" CssClass="form-control" ReadOnly="true" />
                        </div>
                        <div class="form-group col-md-7">
                            <label>Combustível:</label>
                            <asp:TextBox ID="txtDescCombustivel" runat="server" CssClass="form-control" ReadOnly="true" />
                        </div>
                        <div class="form-group col-md-2">
                            <label>Est. Atual(Litros):</label>
                            <asp:TextBox ID="txtEstoqueAtual" runat="server" CssClass="form-control" ReadOnly="true" />
                        </div>
                        <div class="form-group col-md-2">
                            <label>Capacidade(Litros):</label>
                            <asp:TextBox ID="txtCapTotal" runat="server" CssClass="form-control" ReadOnly="true" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-2">
                            <label>Data Entrada:</label>
                            <asp:TextBox ID="txtDataEntrada" runat="server" CssClass="form-control" MaxLength="10" onkeyup="formatarData(this)" />
                        </div>
                        <div class="col-md-2">
                            <label>Litros:</label>
                            <asp:TextBox ID="txtLitrosEntrada" runat="server" CssClass="form-control" onblur="formatarLitros(this)" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group col-md-2">
                            <label>Valor Total:</label>
                            <asp:TextBox ID="txtValorTotalEntrada" runat="server" CssClass="form-control" onkeyup="formatarMoeda(this)" onblur="calcularValorUnitario()" />
                        </div>
                        <div class="form-group col-md-2">
                            <label>Valor Unitário:</label>
                            <asp:TextBox ID="txtValorUnitario" runat="server" CssClass="form-control" />
                        </div>
                    </div>

                    <div class="row g-3">
                        <div class="col-md-3">
                            <br />
                            <asp:Button ID="btnSalvarEntrada" CssClass="btn btn-outline-info w-100" runat="server" Text="Confirmar Entrada" OnClick="btnSalvarEntrada_Click" OnClientClick="return validarEstoqueEntrada();" />
                        </div>

                        <div class="col-md-2">
                            <br />
                            <a href="controlaestoque.aspx" class="btn btn-outline-danger w-100">Fechar   
                            </a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade" id="modalTanque" tabindex="-1">
    <div class="modal-dialog">
        <div class="modal-content">
            <div id="divMsg1" runat="server"
                class="alert alert-dismissible fade show mt-3"
                role="alert" visible="false">
                <asp:Label ID="lblMsgGeral1" runat="server"></asp:Label>
                <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
            </div>
            <div class="modal-header bg-success text-white">
                <h5 class="modal-title">Cadastrar Tanque</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
            </div>

            <div class="modal-body">

                <div class="mb-2">
                    <label>Tanque:</label>
                    <asp:TextBox ID="txtNomeTanque" runat="server" CssClass="form-control" />
                </div>

                <div class="mb-2">
                    <label>Combustível:</label>
                    <asp:DropDownList ID="ddlCombustivel" runat="server" CssClass="form-control" />
                </div>

                <div class="mb-2">
                    <label>Capacidade (Litros):</label>
                    <asp:TextBox ID="txtCapacidadeTotal" runat="server" CssClass="form-control" onblur="formatarLitros(this)"/>
                </div>

            </div>

            <div class="modal-footer">
                <asp:Button ID="btnSalvarModal"
                    runat="server"
                    Text="Salvar"
                    CssClass="btn btn-success"
                    OnClick="btnSalvarTanque_Click" />

                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">
                    Fechar
                </button>
            </div>

        </div>
    </div>
</div>
    </div>
    
</asp:Content>
