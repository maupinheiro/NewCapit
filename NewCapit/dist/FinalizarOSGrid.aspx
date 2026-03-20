<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="FinalizarOSGrid.aspx.cs" Inherits="NewCapit.dist.pages.FinalizarOSGrid" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script>

        function calcularTempo(inicio, termino) {
            var i = inicio.split(":");
            var t = termino.split(":");

            var data1 = new Date(0, 0, 0, i[0], i[1]);
            var data2 = new Date(0, 0, 0, t[0], t[1]);

            var diff = (data2 - data1) / 60000;

            return diff;
        }

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
            let txtInicio = document.getElementById("<%= txtInicio.ClientID %>");
            let txtTerm = document.getElementById("<%= txtTerm.ClientID %>");

            if (txtInicio) aplicarMascara(txtInicio, "00/00/0000 00:00");
            if (txtTerm) aplicarMascara(txtTerm, "00/00/0000 00:00");

            let txtInicioEletrica = document.getElementById("<%= txtInicioEletrica.ClientID %>");
            let txtFimEletrica = document.getElementById("<%= txtFimEletrica.ClientID %>");

            if (txtInicioEletrica) aplicarMascara(txtInicioEletrica, "00/00/0000 00:00");
            if (txtFimEletrica) aplicarMascara(txtFimEletrica, "00/00/0000 00:00");

            let txtInicioBorracharia = document.getElementById("<%= txtInicioBorracharia.ClientID %>");
            let txtFimBorracharia = document.getElementById("<%= txtFimBorracharia.ClientID %>");

            if (txtInicioBorracharia) aplicarMascara(txtInicioBorracharia, "00/00/0000 00:00");
            if (txtFimBorracharia) aplicarMascara(txtFimBorracharia, "00/00/0000 00:00");

            let txtInicioFunilaria = document.getElementById("<%= txtInicioFunilaria.ClientID %>");
            let txtFimFunilaria = document.getElementById("<%= txtFimFunilaria.ClientID %>");

            if (txtInicioFunilaria) aplicarMascara(txtInicioFunilaria, "00/00/0000 00:00");
            if (txtFimFunilaria) aplicarMascara(txtFimFunilaria, "00/00/0000 00:00");


        });
    </script>




    <div class="content-wrapper">
        <section class="content">
            <div class="container-fluid">
                <br />
                <div id="toastContainerVermelho" class="alert alert-danger alert-dismissible" style="display: none;">
                    <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                    <h5><i class="icon fas fa-exclamation-triangle"></i>Alerta!</h5>
                    Alertas
                </div>
                <div class="col-md-12">
                    <div class="card card-info">
                        <div class="card-header" style="background-color: #A020F0; font-weight: bold;">
                            <h3 class="card-title">
                                <h3 class="card-title"><i class="fas fa-shipping-fast"></i>&nbsp;Manutenção - Finalizar de Ordem de Serviço</h3>
                            </h3>
                            <div class="card-tools">
                                <button type="button" class="btn btn-tool" data-card-widget="maximize">
                                    <i class="fas fa-expand"></i>
                                </button>
                                <button type="button" class="btn btn-tool" data-card-widget="collapse">
                                    <i class="fas fa-minus"></i>
                                </button>
                                <button type="button" class="btn btn-tool" data-card-widget="remove">
                                    <i class="fas fa-times"></i>
                                </button>
                            </div>
                            <!-- /.card-tools -->
                        </div>
                        <div class="card-body">
                            <div class="card">

                                <div class="card-header bg-secondary text-white">
                                    Finalização da Ordem de Serviço
                                </div>

                                <div class="card-body">
                                    <br />
                                    <div id="divMsg" runat="server"
                                        class="alert alert-dismissible fade show mt-3"
                                        role="alert" visible="false">

                                        <asp:Label ID="lblMsgGeral" runat="server"></asp:Label>

                                        <button type="button" class="btn-close" data-bs-dismiss="alert"></button>

                                    </div>

                                    <div class="form-group row">
                                        <label for="inputRemetente" class="col-sm-1 col-form-label" style="text-align: right">Núm. OS:</label>
                                        <div class="col-md-1">
                                            <asp:TextBox
                                                ID="txtOS"
                                                runat="server"
                                                CssClass="form-control"
                                                Style="text-align: center"
                                                OnTextChanged="txtOS_TextChanged"
                                                AutoPostBack="true"></asp:TextBox>
                                        </div>
                                        <label for="inputRemetente" class="col-sm-1 col-form-label" style="text-align: right">Abertura:</label>
                                        <div class="col-md-2">
                                            <asp:TextBox
                                                ID="txtAbertura"
                                                runat="server"
                                                CssClass="form-control"
                                                Style="text-align: center">
                                            </asp:TextBox>
                                        </div>
                                        <div class="col-md-3" id="caminhaoParado" runat="server" visible="false">
                                            <asp:TextBox
                                                ID="txtTempoParado"
                                                runat="server"
                                                Style="text-align: center"
                                                CssClass="form-control">
                                            </asp:TextBox>
                                        </div>
                                        <label for="inputRemetente" class="col-sm-1 col-form-label" style="text-align: right">Responsável:</label>
                                        <div class="col-md-2">
                                            <asp:TextBox
                                                ID="txtRespAbertura"
                                                runat="server"
                                                CssClass="form-control"
                                                Style="text-align: left"
                                                ReadOnly="true">
                                            </asp:TextBox>
                                        </div>
                                        <div class="col-md-1">
                                            <asp:TextBox
                                                ID="txtStatus"
                                                runat="server"
                                                CssClass="form-control"
                                                Style="text-align: center"
                                                ReadOnly="true">
                                            </asp:TextBox>
                                        </div>

                                    </div>


                                    <div class="row">
                                        <div class="col-md-1">
                                            <label>Motorista:</label>
                                            <asp:TextBox
                                                ID="txtId_Motorista"
                                                runat="server"
                                                CssClass="form-control"
                                                ReadOnly="true">   

                                            </asp:TextBox>
                                            <%--Style="text-align: center"--%>
                                            <!-- negrito na class form-control  font-weight-bold -->
                                        </div>

                                        <div class="col-md-4">
                                            <label>Nome Completo:</label>
                                            <asp:TextBox
                                                ID="txtNome_Motorista"
                                                runat="server"
                                                class="form-control"
                                                ReadOnly="true">
                                            </asp:TextBox>
                                        </div>

                                        <div class="col-md-4">
                                            <label>Transportadora:</label>
                                            <asp:TextBox ID="txtTransp_Motorista"
                                                runat="server"
                                                CssClass="form-control"
                                                ReadOnly="true">
                                            </asp:TextBox>
                                        </div>

                                        <div class="col-md-3">
                                            <label>Núcleo:</label>
                                            <asp:TextBox
                                                ID="txtNucleo_Motorista"
                                                runat="server"
                                                CssClass="form-control"
                                                ReadOnly="true" />
                                        </div>

                                    </div>
                                    <hr />

                                    <div class="row">

                                        <div class="col-md-1">
                                            <label>Código:</label>
                                            <asp:TextBox ID="txtCodVeiculo"
                                                runat="server"
                                                CssClass="form-control"
                                                ReadOnly="true">
                                            </asp:TextBox>
                                        </div>

                                        <div class="col-md-1">
                                            <label>Placa:</label>
                                            <asp:TextBox
                                                ID="txtPlaca"
                                                runat="server"
                                                CssClass="form-control"
                                                ReadOnly="true">
                                            </asp:TextBox>
                                        </div>

                                        <div class="col-md-1">
                                            <label>Km Atual:</label>
                                            <asp:TextBox ID="txtKm"
                                                runat="server"
                                                ReadOnly="true"
                                                CssClass="form-control">
                                            </asp:TextBox>
                                        </div>

                                        <div class="col-md-2">
                                            <label>Tipo:</label>
                                            <asp:TextBox ID="txtTipVei" runat="server" CssClass="form-control" ReadOnly="true" />
                                        </div>

                                        <div class="col-md-2">
                                            <label>Marca</label>
                                            <asp:TextBox ID="txtMarca" runat="server" CssClass="form-control" ReadOnly="true" />
                                        </div>

                                        <div class="col-md-2">
                                            <label>Modelo:</label>
                                            <asp:TextBox ID="txtModelo" runat="server" CssClass="form-control" ReadOnly="true" />
                                        </div>

                                        <div class="col-md-1">
                                            <label>Ano:</label>
                                            <asp:TextBox ID="txtAno" runat="server" CssClass="form-control" ReadOnly="true" />
                                        </div>

                                        <div class="col-md-2">
                                            <label>Núcleo:</label>
                                            <asp:TextBox ID="txtNucleo" runat="server" CssClass="form-control" ReadOnly="true" />
                                        </div>

                                    </div>

                                    <hr />
                                    <br />
                                    <div class="card card-outline card-info collapsed-card">
                                        <div class="card-header">
                                            <h3 class="card-title"><i class="far fa-edit"></i>&nbsp;Mecânica</h3>
                                            <div class="card-tools">
                                                <button type="button" class="btn btn-tool" data-card-widget="collapse">
                                                    <i class="fas fa-plus"></i>
                                                </button>
                                            </div>

                                        </div>

                                        <div class="card-body">
                                            <asp:TextBox ID="txtTipo"
                                                runat="server"
                                                Visible="false"
                                                Text="Mecanica"
                                                CssClass="form-control">
                                            </asp:TextBox>
                                            <div class="form-group">
                                                <label>Descrição do Problema:</label>
                                                <asp:TextBox ID="txtParteMecanica" runat="server"
                                                    TextMode="MultiLine"
                                                    Rows="3"
                                                    CssClass="form-control"  ReadOnly="true"></asp:TextBox>
                                            </div>
                                            <div class="form-group">
                                                <label>Serviço Executado:</label>
                                                <asp:TextBox ID="txtServExecMec" runat="server"
                                                    TextMode="MultiLine"
                                                    Rows="3"
                                                    CssClass="form-control"></asp:TextBox>
                                            </div>
                                            <div class="row">
                                                <label>
                                                    <h3>Peças Trocadas</h3>
                                                    <hr />
                                                </label>

                                                <div class="col-md-3">
                                                    <label>Peça / Serviço Executado:</label>
                                                    <asp:DropDownList
                                                        ID="ddlPeca"
                                                        runat="server"
                                                        CssClass="form-control select2">
                                                    </asp:DropDownList>
                                                </div>

                                                <div class="col-md-1">
                                                    <label>Qtd.:</label>
                                                    <asp:TextBox ID="txtQuant"
                                                        runat="server"
                                                        CssClass="form-control"
                                                        Style="text-align: center">
                                                    </asp:TextBox>
                                                </div>

                                                <div class="col-md-2">
                                                    <label>Inicio:</label>
                                                    <asp:TextBox ID="txtInicio"
                                                        runat="server"
                                                        CssClass="form-control"
                                                        Style="text-align: center"
                                                        placeholder="dd/mm/aaaa hh:mm">
                                                    </asp:TextBox>
                                                </div>

                                                <div class="col-md-2">
                                                    <label>Termino:</label>
                                                    <asp:TextBox ID="txtTerm"
                                                        runat="server"
                                                        CssClass="form-control"
                                                        Style="text-align: center"
                                                        placeholder="dd/mm/aaaa hh:mm">
                                                    </asp:TextBox>
                                                </div>

                                                <div class="col-md-3">
                                                    <label>Mecânico:</label>
                                                    <asp:DropDownList ID="ddlMecanico"
                                                        runat="server"
                                                        CssClass="form-control select2">
                                                    </asp:DropDownList>
                                                </div>

                                                <div class="col-md-1">
                                                    <label>&nbsp;</label><br />
                                                    <asp:Button ID="btnFinalizarTroca"
                                                        runat="server"
                                                        Text="Lançar Peça"
                                                        CssClass="btn btn-success w-100"
                                                        OnClick="btnFinalizarTroca_Click" />
                                                </div>

                                            </div>
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <br />
                                                    <asp:Repeater ID="gridPecas" runat="server" OnItemCommand="gridPecas_ItemCommand">

                                                        <HeaderTemplate>

                                                            <table class="table table-striped table-hover table-bordered">
                                                                <thead class="table-dark">
                                                                    <tr>
                                                                        <th>Peça</th>
                                                                        <th>Qtd.</th>
                                                                        <th>Unidade</th>
                                                                        <th>Total</th>
                                                                        <th>Crachá</th>
                                                                        <th>Mecânico</th>
                                                                        <th>Inicio</th>
                                                                        <th>Término</th>
                                                                        <th>Tempo (min)</th>
                                                                        <th>Ação</th>
                                                                    </tr>
                                                                </thead>

                                                                <tbody>
                                                        </HeaderTemplate>

                                                        <ItemTemplate>

                                                            <tr>

                                                                <td><%# Eval("descricao_peca") %></td>
                                                                <td><%# Eval("quant") %></td>
                                                                <td><%# Eval("valor_unitario", "{0:C}") %></td>
                                                                <td><%# Eval("valor_total", "{0:C}") %></td>
                                                                <td><%# Eval("cracha") %></td>
                                                                <td><%# Eval("nome") %></td>
                                                                <td><%# Eval("inicio", "{0:dd/MM/yyyy HH:mm}") %></td>
                                                                <td><%# Eval("termino", "{0:dd/MM/yyyy HH:mm}") %></td>
                                                                <td><%# Eval("tempo_minutos") %></td>

                                                                <td>

                                                                    <asp:LinkButton
                                                                        ID="btnExcluir"
                                                                        runat="server"
                                                                        CommandName="Excluir"
                                                                        CommandArgument='<%# Eval("id") %>'
                                                                        CssClass="btn btn-danger btn-sm"
                                                                        OnClientClick="return confirm('Deseja excluir esta peça?');">

Excluir

                                                                    </asp:LinkButton>

                                                                </td>

                                                            </tr>

                                                        </ItemTemplate>

                                                        <FooterTemplate>
                                                            </tbody>
</table>

                                                        </FooterTemplate>

                                                    </asp:Repeater>

                                                </div>
                                            </div>

                                        </div>

                                    </div>
                                    <br />
                                    <div class="card card-outline card-info collapsed-card">
                                        <div class="card-header">
                                            <h3 class="card-title"><i class="far fa-edit"></i>&nbsp;Eletrica</h3>
                                            <div class="card-tools">
                                                <button type="button" class="btn btn-tool" data-card-widget="collapse">
                                                    <i class="fas fa-plus"></i>
                                                </button>
                                            </div>

                                        </div>

                                        <div class="card-body">
                                            <asp:TextBox ID="txtTipoEletrica"
                                                runat="server"
                                                Visible="false"
                                                Text="Eletrica"
                                                CssClass="form-control">
                                            </asp:TextBox>
                                            <div class="form-group">
                                                <label>Descrição do Problema:</label>
                                                <asp:TextBox ID="txtParteEletrica" runat="server"
                                                    TextMode="MultiLine"
                                                    Rows="3"
                                                    CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                            </div>
                                            <div class="form-group">
                                                <label>Serviço Executado:</label>
                                                <asp:TextBox ID="txtEletricaExecutada" runat="server"
                                                    TextMode="MultiLine"
                                                    Rows="3"
                                                    CssClass="form-control"></asp:TextBox>
                                            </div>
                                            <div class="row">
                                                <label>
                                                    <h3>Peças Trocadas</h3>
                                                    <hr />
                                                </label>

                                                <div class="col-md-3">
                                                    <label>Peça / Serviço Executado:</label>
                                                    <asp:DropDownList
                                                        ID="ddlParteEletrica"
                                                        runat="server"
                                                        CssClass="form-control select2">
                                                    </asp:DropDownList>
                                                </div>

                                                <div class="col-md-1">
                                                    <label>Qtd.:</label>
                                                    <asp:TextBox ID="txtQuantEletrica"
                                                        runat="server"
                                                        CssClass="form-control"
                                                        Style="text-align: center">
                                                    </asp:TextBox>
                                                </div>

                                                <div class="col-md-2">
                                                    <label>Inicio:</label>
                                                    <asp:TextBox ID="txtInicioEletrica"
                                                        runat="server"
                                                        CssClass="form-control"
                                                        Style="text-align: center"
                                                        placeholder="dd/mm/aaaa hh:mm">
                                                    </asp:TextBox>
                                                </div>

                                                <div class="col-md-2">
                                                    <label>Termino:</label>
                                                    <asp:TextBox ID="txtFimEletrica"
                                                        runat="server"
                                                        CssClass="form-control"
                                                        Style="text-align: center"
                                                        placeholder="dd/mm/aaaa hh:mm">
                                                    </asp:TextBox>
                                                </div>

                                                <div class="col-md-3">
                                                    <label>Mecânico:</label>
                                                    <asp:DropDownList ID="ddlEletricista"
                                                        runat="server"
                                                        CssClass="form-control select2">
                                                    </asp:DropDownList>
                                                </div>

                                                <div class="col-md-1">
                                                    <label>&nbsp;</label><br />
                                                    <asp:Button ID="btnTrocarEletrica"
                                                        runat="server"
                                                        Text="Lançar Peça"
                                                        CssClass="btn btn-success w-100"
                                                        OnClick="btnTrocarEletrica_Click" />
                                                </div>

                                            </div>
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <br />
                                                    <asp:Repeater ID="gridPecasEletrica" runat="server" OnItemCommand="gridPecasEletrica_ItemCommand">

                                                        <HeaderTemplate>

                                                            <table class="table table-striped table-hover table-bordered">
                                                                <thead class="table-dark">
                                                                    <tr>
                                                                        <th>Peça</th>
                                                                        <th>Qtd.</th>
                                                                        <th>Unidade</th>
                                                                        <th>Total</th>
                                                                        <th>Crachá</th>
                                                                        <th>Mecânico</th>
                                                                        <th>Inicio</th>
                                                                        <th>Término</th>
                                                                        <th>Tempo (min)</th>
                                                                        <th>Ação</th>
                                                                    </tr>
                                                                </thead>

                                                                <tbody>
                                                        </HeaderTemplate>

                                                        <ItemTemplate>

                                                            <tr>

                                                                <td><%# Eval("descricao_peca") %></td>
                                                                <td><%# Eval("quant") %></td>
                                                                <td><%# Eval("valor_unitario", "{0:C}") %></td>
                                                                <td><%# Eval("valor_total", "{0:C}") %></td>
                                                                <td><%# Eval("cracha") %></td>
                                                                <td><%# Eval("nome") %></td>
                                                                <td><%# Eval("inicio", "{0:dd/MM/yyyy HH:mm}") %></td>
                                                                <td><%# Eval("termino", "{0:dd/MM/yyyy HH:mm}") %></td>
                                                                <td><%# Eval("tempo_minutos") %></td>

                                                                <td>

                                                                    <asp:LinkButton
                                                                        ID="btnExcluirEletrica"
                                                                        runat="server"
                                                                        CommandName="ExcluirEletrica"
                                                                        CommandArgument='<%# Eval("id") %>'
                                                                        CssClass="btn btn-danger btn-sm"
                                                                        OnClientClick="return confirm('Deseja excluir esta peça?');">

Excluir

                                                                    </asp:LinkButton>

                                                                </td>

                                                            </tr>

                                                        </ItemTemplate>

                                                        <FooterTemplate>
                                                            </tbody>
</table>

                                                        </FooterTemplate>

                                                    </asp:Repeater>

                                                </div>
                                            </div>

                                        </div>

                                    </div>
                                    <br />
                                    <div class="card card-outline card-info collapsed-card">
                                        <div class="card-header">
                                            <h3 class="card-title"><i class="far fa-edit"></i>&nbsp;Borracharia</h3>
                                            <div class="card-tools">
                                                <button type="button" class="btn btn-tool" data-card-widget="collapse">
                                                    <i class="fas fa-plus"></i>
                                                </button>
                                            </div>

                                        </div>

                                        <div class="card-body">
                                            <asp:TextBox ID="txtTipoBorracharia"
                                                runat="server"
                                                Visible="false"
                                                Text="Borracharia"
                                                CssClass="form-control">
                                            </asp:TextBox>
                                            <div class="form-group">
                                                <label>Descrição do Problema:</label>
                                                <asp:TextBox ID="txtParteBorracharia" runat="server"
                                                    TextMode="MultiLine"
                                                    Rows="3"
                                                    CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                            </div>
                                            <div class="form-group">
                                                <label>Serviço Executado:</label>
                                                <asp:TextBox ID="txtServExecBorracharia" runat="server"
                                                    TextMode="MultiLine"
                                                    Rows="3"
                                                    CssClass="form-control"></asp:TextBox>
                                            </div>
                                            <div class="row">
                                                <label>
                                                    <h3>Peças Trocadas</h3>
                                                    <hr />
                                                </label>

                                                <div class="col-md-3">
                                                    <label>Peça / Serviço Executado:</label>
                                                    <asp:DropDownList
                                                        ID="ddlParteBorracharia"
                                                        runat="server"
                                                        CssClass="form-control select2">
                                                    </asp:DropDownList>
                                                </div>

                                                <div class="col-md-1">
                                                    <label>Qtd.:</label>
                                                    <asp:TextBox ID="txtQuantBorracharia"
                                                        runat="server"
                                                        CssClass="form-control"
                                                        Style="text-align: center">
                                                    </asp:TextBox>
                                                </div>

                                                <div class="col-md-2">
                                                    <label>Inicio:</label>
                                                    <asp:TextBox ID="txtInicioBorracharia"
                                                        runat="server"
                                                        CssClass="form-control"
                                                        Style="text-align: center"
                                                        placeholder="dd/mm/aaaa hh:mm">
                                                    </asp:TextBox>
                                                </div>

                                                <div class="col-md-2">
                                                    <label>Termino:</label>
                                                    <asp:TextBox ID="txtFimBorracharia"
                                                        runat="server"
                                                        CssClass="form-control"
                                                        Style="text-align: center"
                                                        placeholder="dd/mm/aaaa hh:mm">
                                                    </asp:TextBox>
                                                </div>

                                                <div class="col-md-3">
                                                    <label>Mecânico:</label>
                                                    <asp:DropDownList ID="ddlBorracheiro"
                                                        runat="server"
                                                        CssClass="form-control select2">
                                                    </asp:DropDownList>
                                                </div>

                                                <div class="col-md-1">
                                                    <label>&nbsp;</label><br />
                                                    <asp:Button ID="btnTrocarBorracharia"
                                                        runat="server"
                                                        Text="Lançar Peça"
                                                        CssClass="btn btn-success w-100"
                                                        OnClick="btnTrocarBorracharia_Click" />
                                                </div>

                                            </div>
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <br />
                                                    <asp:Repeater ID="gridPecasBorracharia" runat="server" OnItemCommand="gridPecasBorracharia_ItemCommand">

                                                        <HeaderTemplate>

                                                            <table class="table table-striped table-hover table-bordered">
                                                                <thead class="table-dark">
                                                                    <tr>
                                                                        <th>Peça</th>
                                                                        <th>Qtd.</th>
                                                                        <th>Unidade</th>
                                                                        <th>Total</th>
                                                                        <th>Crachá</th>
                                                                        <th>Mecânico</th>
                                                                        <th>Inicio</th>
                                                                        <th>Término</th>
                                                                        <th>Tempo (min)</th>
                                                                        <th>Ação</th>
                                                                    </tr>
                                                                </thead>

                                                                <tbody>
                                                        </HeaderTemplate>

                                                        <ItemTemplate>

                                                            <tr>

                                                                <td><%# Eval("descricao_peca") %></td>
                                                                <td><%# Eval("quant") %></td>
                                                                <td><%# Eval("valor_unitario", "{0:C}") %></td>
                                                                <td><%# Eval("valor_total", "{0:C}") %></td>
                                                                <td><%# Eval("cracha") %></td>
                                                                <td><%# Eval("nome") %></td>
                                                                <td><%# Eval("inicio", "{0:dd/MM/yyyy HH:mm}") %></td>
                                                                <td><%# Eval("termino", "{0:dd/MM/yyyy HH:mm}") %></td>
                                                                <td><%# Eval("tempo_minutos") %></td>

                                                                <td>

                                                                    <asp:LinkButton
                                                                        ID="btnExcluirBorracharia"
                                                                        runat="server"
                                                                        CommandName="ExcluirBorracharia"
                                                                        CommandArgument='<%# Eval("id") %>'
                                                                        CssClass="btn btn-danger btn-sm"
                                                                        OnClientClick="return confirm('Deseja excluir esta peça?');">

Excluir

                                                                    </asp:LinkButton>

                                                                </td>

                                                            </tr>

                                                        </ItemTemplate>

                                                        <FooterTemplate>
                                                            </tbody>
</table>

                                                        </FooterTemplate>

                                                    </asp:Repeater>

                                                </div>
                                            </div>

                                        </div>

                                    </div>
                                    <br />
                                    <div class="card card-outline card-info collapsed-card">
                                        <div class="card-header">
                                            <h3 class="card-title"><i class="far fa-edit"></i>&nbsp;Funilaria/Carroceria/Sider/Total Sider</h3>
                                            <div class="card-tools">
                                                <button type="button" class="btn btn-tool" data-card-widget="collapse">
                                                    <i class="fas fa-plus"></i>
                                                </button>
                                            </div>

                                        </div>

                                        <div class="card-body">
                                            <asp:TextBox ID="txtTipoFunilaria"
                                                runat="server"
                                                Visible="false"
                                                Text="Funilaria"
                                                CssClass="form-control">
                                            </asp:TextBox>
                                            <div class="form-group">
                                                <label>Descrição do Problema:</label>
                                                <asp:TextBox ID="txtParteFunilaria" runat="server"
                                                    TextMode="MultiLine"
                                                    Rows="3"
                                                    CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                            </div>
                                            <div class="form-group">
                                                <label>Serviço Executado:</label>
                                                <asp:TextBox ID="txtServExecFunilaria" runat="server"
                                                    TextMode="MultiLine"
                                                    Rows="3"
                                                    CssClass="form-control"></asp:TextBox>
                                            </div>
                                            <div class="row">
                                                <label>
                                                    <h3>Peças Trocadas</h3>
                                                    <hr />
                                                </label>

                                                <div class="col-md-3">
                                                    <label>Peça / Serviço Executado:</label>
                                                    <asp:DropDownList
                                                        ID="ddlParteFunilaria"
                                                        runat="server"
                                                        CssClass="form-control select2">
                                                    </asp:DropDownList>
                                                </div>

                                                <div class="col-md-1">
                                                    <label>Qtd.:</label>
                                                    <asp:TextBox ID="txtQuantFunilaria"
                                                        runat="server"
                                                        CssClass="form-control"
                                                        Style="text-align: center">
                                                    </asp:TextBox>
                                                </div>

                                                <div class="col-md-2">
                                                    <label>Inicio:</label>
                                                    <asp:TextBox ID="txtInicioFunilaria"
                                                        runat="server"
                                                        CssClass="form-control"
                                                        Style="text-align: center"
                                                        placeholder="dd/mm/aaaa hh:mm">
                                                    </asp:TextBox>
                                                </div>

                                                <div class="col-md-2">
                                                    <label>Termino:</label>
                                                    <asp:TextBox ID="txtFimFunilaria"
                                                        runat="server"
                                                        CssClass="form-control"
                                                        Style="text-align: center"
                                                        placeholder="dd/mm/aaaa hh:mm">
                                                    </asp:TextBox>
                                                </div>

                                                <div class="col-md-3">
                                                    <label>Mecânico:</label>
                                                    <asp:DropDownList ID="ddlFunileiro"
                                                        runat="server"
                                                        CssClass="form-control select2">
                                                    </asp:DropDownList>
                                                </div>

                                                <div class="col-md-1">
                                                    <label>&nbsp;</label><br />
                                                    <asp:Button ID="btnTrocarFunilaria"
                                                        runat="server"
                                                        Text="Lançar Peça"
                                                        CssClass="btn btn-success w-100"
                                                        OnClick="btnTrocarFunilaria_Click" />
                                                </div>

                                            </div>
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <br />
                                                    <asp:Repeater ID="gridPecasFunilaria" runat="server" OnItemCommand="gridPecasFunilaria_ItemCommand">

                                                        <HeaderTemplate>

                                                            <table class="table table-striped table-hover table-bordered">
                                                                <thead class="table-dark">
                                                                    <tr>
                                                                        <th>Peça</th>
                                                                        <th>Qtd.</th>
                                                                        <th>Unidade</th>
                                                                        <th>Total</th>
                                                                        <th>Crachá</th>
                                                                        <th>Mecânico</th>
                                                                        <th>Inicio</th>
                                                                        <th>Término</th>
                                                                        <th>Tempo (min)</th>
                                                                        <th>Ação</th>
                                                                    </tr>
                                                                </thead>

                                                                <tbody>
                                                        </HeaderTemplate>

                                                        <ItemTemplate>

                                                            <tr>

                                                                <td><%# Eval("descricao_peca") %></td>
                                                                <td><%# Eval("quant") %></td>
                                                                <td><%# Eval("valor_unitario", "{0:C}") %></td>
                                                                <td><%# Eval("valor_total", "{0:C}") %></td>
                                                                <td><%# Eval("cracha") %></td>
                                                                <td><%# Eval("nome") %></td>
                                                                <td><%# Eval("inicio", "{0:dd/MM/yyyy HH:mm}") %></td>
                                                                <td><%# Eval("termino", "{0:dd/MM/yyyy HH:mm}") %></td>
                                                                <td><%# Eval("tempo_minutos") %></td>

                                                                <td>

                                                                    <asp:LinkButton
                                                                        ID="btnExcluirFunilaria"
                                                                        runat="server"
                                                                        CommandName="ExcluirFunilaria"
                                                                        CommandArgument='<%# Eval("id") %>'
                                                                        CssClass="btn btn-danger btn-sm"
                                                                        OnClientClick="return confirm('Deseja excluir esta peça?');">

Excluir

                                                                    </asp:LinkButton>

                                                                </td>

                                                            </tr>

                                                        </ItemTemplate>

                                                        <FooterTemplate>
                                                            </tbody>
                                                            </table>

                                                        </FooterTemplate>

                                                    </asp:Repeater>

                                                </div>
                                            </div>

                                        </div>

                                    </div>

                                    <hr>

                                    <div class="row">

                                        <div class="col-md-2">
                                            Tempo Total OS
                                            <asp:Label ID="lblTempoTotal" runat="server"
                                                CssClass="form-control font-weight-bold" ReadOnly="true"></asp:Label>
                                        </div>

                                        <div class="col-md-2">
                                            Total Peças
                                            <asp:Label ID="lblTotalPecas" runat="server"
                                                CssClass="form-control font-weight-bold" ReadOnly="true"></asp:Label>
                                        </div>

                                        <div class="col-md-2">
                                            Custo Total
                                            <asp:Label ID="lblCustoTotal" runat="server"
                                                CssClass="form-control font-weight-bold"
                                                ReadOnly="true"></asp:Label>
                                        </div>
                                        <div class="col-md-1" id="situacao" runat="server" visible="false">
                                            Situação
                                            <asp:Label ID="lblStatus"
                                                runat="server"
                                                CssClass="form-control font-weight-bold"
                                                ReadOnly="true"></asp:Label>
                                        </div>
                                        <div class="col-md-2" id="fechamento" runat="server" visible="false">
                                            Fechamento
                                            <asp:Label ID="lblFechamento" runat="server"
                                                CssClass="form-control font-weight-bold"
                                                ReadOnly="true"></asp:Label>
                                        </div>
                                        <div class="col-md-3" id="responsavel" runat="server" visible="false">
                                            Responsável
                                            <asp:Label ID="lblResp_fechamento" runat="server"
                                                CssClass="form-control font-weight-bold"
                                                ReadOnly="true"></asp:Label>
                                        </div>

                                    </div>

                                    <br>
                                    <div class="row g-3">


                                        <div class="col-md-3">
                                            <br />
                                            <asp:Button ID="btnFinalizar" CssClass="btn btn-outline-info w-100" runat="server" Text="Finalizar Ordem de Serviço" OnClick="btnFinalizar_Click" />
                                        </div>

                                        <div class="col-md-1">
                                            <br />
                                            <a href="ListaOS.aspx" class="btn btn-outline-danger w-100">Fechar               
                                            </a>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>


</asp:Content>
