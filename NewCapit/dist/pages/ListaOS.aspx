<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="ListaOS.aspx.cs" Inherits="NewCapit.dist.pages.ListaOS" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        function abrirModal() {
            var myModal = new bootstrap.Modal(document.getElementById('modalOrdem'));
            myModal.show();
        }

        function fecharModal() {
            var modalEl = document.getElementById('modalOrdem');
            var modal = bootstrap.Modal.getInstance(modalEl);
            modal.hide();
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

            let txtDataInicial = document.getElementById("<%= txtDataInicial.ClientID %>");
            let txtDataFinal = document.getElementById("<%= txtDataFinal.ClientID %>");

            if (txtDataInicial) aplicarMascara(txtDataInicial, "00/00/0000");
            if (txtDataFinal) aplicarMascara(txtDataFinal, "00/00/0000");
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
                                <h3 class="card-title"><i class="fas fa-shipping-fast"></i>&nbsp;Manutenção</h3>
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
                                    Controle de Ordem de Serviço
                                </div>

                                <div class="card-body">

                                    <div class="row">

                                        <div class="col-md-1">
                                            <asp:TextBox ID="txtFrotaPlaca" runat="server"
                                                CssClass="form-control"
                                                Placeholder="Placa"
                                                AutoPostBack="true" OnTextChanged="txtFrotaPlaca_TextChanged"></asp:TextBox>
                                        </div>

                                        <div class="col-md-2">
                                            <asp:TextBox ID="txtCrachaMotorista" runat="server"
                                                CssClass="form-control"
                                                Placeholder="Motorista"
                                                AutoPostBack="true" OnTextChanged="txtCrachaMotorista_TextChanged"></asp:TextBox>
                                        </div>

                                        <div class="col-md-2">
                                            <asp:TextBox ID="txtFornecedor" runat="server"
                                                CssClass="form-control"
                                                Placeholder="Fornecedor"
                                                AutoPostBack="true" OnTextChanged="txtFornecedor_TextChanged"></asp:TextBox>
                                        </div>

                                        <div class="col-md-1">
                                            <asp:TextBox ID="txtOrdem_Servico" runat="server"
                                                CssClass="form-control"
                                                Placeholder="OS"
                                                AutoPostBack="true"
                                                OnTextChanged="txtOrdem_Servico_TextChanged"></asp:TextBox>
                                        </div>

                                        <div class="col-md-1">
                                            <asp:DropDownList ID="ddlStatus" runat="server"
                                                AutoPostBack="true"
                                                OnTextChanged="ddlStatus_TextChanged"
                                                CssClass="form-control">
                                                <asp:ListItem Value="">Status</asp:ListItem>
                                                <asp:ListItem Value="1">Aberta</asp:ListItem>
                                                <asp:ListItem Value="2">Finalizada</asp:ListItem>
                                                <asp:ListItem Value="3">Cancelada</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>

                                        <div class="col-md-1">
                                            <asp:DropDownList ID="ddlManutencao" runat="server"
                                                AutoPostBack="true"
                                                OnTextChanged="ddlManutencao_TextChanged"
                                                CssClass="form-control">
                                                <asp:ListItem Value="">Tipo OS</asp:ListItem>
                                                <asp:ListItem Value="C">Corretiva</asp:ListItem>
                                                <asp:ListItem Value="P">Preventiva</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-1">
                                            <asp:DropDownList ID="ddlServico" runat="server"
                                                AutoPostBack="true"
                                                OnTextChanged="ddlServico_TextChanged"
                                                CssClass="form-control">
                                                <asp:ListItem Value="">Serviço</asp:ListItem>
                                                <asp:ListItem Value="I">Interno</asp:ListItem>
                                                <asp:ListItem Value="E">Externo</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>

                                        <div class="col-md-1">
                                            <asp:TextBox ID="txtDataInicial" runat="server"
                                                AutoPostBack="true"
                                                OnTextChanged="txtDataInicial_TextChanged"
                                                CssClass="form-control"></asp:TextBox>
                                        </div>

                                        <div class="col-md-1">
                                            <asp:TextBox ID="txtDataFinal" runat="server"
                                                AutoPostBack="true"
                                                OnTextChanged="txtDataFinal_TextChanged"
                                                CssClass="form-control"></asp:TextBox>
                                        </div>

                                        <div class="col-md-1">
                                            <asp:Button ID="btnAbrirOs" runat="server"
                                                Text="Abrir O.S."
                                                CssClass="btn btn-success w-100"
                                                OnClick="btnAbrirOs_Click" />
                                        </div>

                                    </div>
                                    <br />
                                    <div class="row mb-3">
                                        <div class="col-6">
                                            <span id="lblVisiveis"></span>
                                        </div>
                                        <div class="col-6">
                                            <span id="lblTotalGeral" runat="server" style="float: right;"></span>
                                        </div>
                                    </div>
                                    <asp:GridView ID="gvOS" runat="server"
                                        CssClass="table table-bordered table-hover"
                                        AutoGenerateColumns="False"
                                        AllowPaging="True"
                                        PageSize="10"
                                        OnPageIndexChanging="gvOS_PageIndexChanging"
                                        HeaderStyle-CssClass="gv-header-custom"
                                        OnRowCommand="gvOS_RowCommand"
                                        OnRowDataBound="gvOS_RowDataBound">

                                        <Columns>

                                            <asp:BoundField DataField="id_os" HeaderText="OS" />
                                            <asp:TemplateField HeaderText="Dias">

                                                <ItemStyle HorizontalAlign="Center" />

                                                <ItemTemplate>
                                                    <span class='<%# CorDias(Convert.ToInt32(Eval("dias_aberta"))) %>'>
                                                        <%# Eval("dias_aberta") %>
                                                    </span>
                                                </ItemTemplate>

                                            </asp:TemplateField>


                                            <asp:BoundField DataField="os_preventiva_corretiva" HeaderText="Manutenção" />
                                            <asp:BoundField DataField="codigo_veiculo" HeaderText="Frota" />
                                            <asp:BoundField DataField="placa" HeaderText="Placa" />
                                            <asp:BoundField DataField="tipo_veiculo" HeaderText="Tipo" />
                                            <asp:BoundField DataField="nome_motorista" HeaderText="Motorista" />
                                            <asp:BoundField DataField="servico_interno_externo" HeaderText="Serviço" />
                                            <asp:BoundField DataField="nome_fornecedor" HeaderText="Fornecedor" />
                                            <asp:BoundField DataField="nucleo_veiculo" HeaderText="Núcleo" />
                                            <asp:BoundField DataField="data_abertura" HeaderText="Abertura"
                                                DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                                            <%--<asp:BoundField DataField="status_texto" HeaderText="Status" />--%>

                                            <%-- <asp:TemplateField HeaderText="Status">
                                                <ItemTemplate>

                                                    <asp:Label ID="lblStatus" runat="server"
                                                        Text='<%# Eval("status_texto") %>'></asp:Label>

                                                </ItemTemplate>
                                            </asp:TemplateField>--%>

                                            <asp:TemplateField HeaderText="Status">
                                                <ItemTemplate>
                                                    <asp:Image ID="imgStatus" runat="server" Width="20px" Height="20px" ToolTip='<%# Eval("status_texto") %>' />
                                                    <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("status_texto") %>'></asp:Label>
                                                    <%-- <asp:Image ID="imgStatus" runat="server" ToolTip='<%# Eval("status_texto") %>' />--%>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Ações">

                                                <ItemTemplate>

                                                    <asp:LinkButton ID="btnPDF"
                                                        runat="server"
                                                        CommandName="pdf"
                                                        CommandArgument='<%# Eval("id_os") %>'
                                                        CssClass="btn btn-sm btn-info">
                                                        Visualizar
                                                    </asp:LinkButton>

                                                    <asp:LinkButton
                                                        ID="btnFinalizar"
                                                        runat="server"
                                                        CssClass='<%# Eval("status_texto").ToString() == "Finalizado" ? "btn btn-secondary btn-sm disabled" : "btn btn-success btn-sm" %>'
                                                        CommandName="Finalizar"
                                                        CommandArgument='<%# Eval("id_os") %>'
                                                        Enabled='<%# Eval("status_texto").ToString() != "Finalizado" %>'>
                                                        Finalizar
                                                    </asp:LinkButton>
                                                </ItemTemplate>

                                            </asp:TemplateField>

                                        </Columns>
                                    </asp:GridView>
                                                                        <pagertemplate>                                                    <div class="d-flex justify-content-center align-items-center gap-2 flex-wrap">


                                            <asp:LinkButton ID="btnPrimeiro" runat="server"
                                                OnClick="btnPrimeiro_Click"
                                                CssClass="btn btn-light btn-sm">
<i class="fas fa-angle-double-left"></i>
                                            </asp:LinkButton>


                                            <asp:LinkButton ID="btnAnterior" runat="server"
                                                OnClick="btnAnterior_Click"
                                                CssClass="btn btn-light btn-sm">
<i class="fa fa-angle-left"></i>
                                            </asp:LinkButton>


                                            <span class="fw-bold">Página
<asp:Label ID="lblPaginaAtual" runat="server" />
                                                de
<asp:Label ID="lblTotalPaginas" runat="server" />
                                            </span>


                                            <asp:LinkButton ID="btnProximo" runat="server"
                                                OnClick="btnProximo_Click"
                                                CssClass="btn btn-light btn-sm">
<i class="fa fa-angle-right"></i>
                                            </asp:LinkButton>


                                            <asp:LinkButton ID="btnUltimo" runat="server"
                                                OnClick="btnUltimo_Click"
                                                CssClass="btn btn-light btn-sm">
<i class="fas fa-angle-double-right"></i>
                                            </asp:LinkButton>


                                            <span>Página:</span>

                                            <asp:TextBox ID="txtIrPagina" runat="server"
                                                CssClass="form-control form-control-sm"
                                                Style="width: 70px;" />

                                            <asp:LinkButton ID="btnIrPagina" runat="server"
                                                CssClass="btn btn-primary btn-sm"
                                                OnClick="btnIrPagina_Click">
Buscar
                                            </asp:LinkButton>

                                        </div>
                                    </pagertemplate>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
        </section>
        <!-- modal visualizar OS -->



        <div class="modal fade" id="modalOrdem" tabindex="-1">
            <div class="modal-dialog modal-xl">
                <div class="modal-content">

                    <div class="modal-header">
                        <h5 class="modal-title">Ordem de Serviço</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                    </div>

                    <div class="modal-body">
                        <div class="row g-3">
                            <div class="col-md-10"></div>
                            <div class="col-md-2">
                                <div class="form-group">
                                    <span class="details"></span>
                                    <asp:TextBox ID="txtStatus" Style="text-align: center" runat="server" CssClass="form-control font-weight-bold" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row g-3">
                            <div class="col-md-1">
                                <asp:Label ID="lblOS" runat="server" />
                            </div>
                            <div class="col-md-1">
                                <asp:Label ID="lblTipo_Os" runat="server" />
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblEmissao" runat="server" />
                            </div>
                            <div class="col-md-1">
                                <asp:Label ID="lblInternoExterno" runat="server" />
                            </div>
                            <div class="col-md-7">
                                <asp:Label ID="lblPrestador" runat="server" />
                            </div>
                        </div>
                        <hr />
                        <div class="row g-3">
                            <div class="col-md-12">
                                <asp:Label ID="lblMotorista" runat="server" />
                            </div>
                        </div>
                        <hr />
                        <div class="row g-3">
                            <div class="col-md-12">
                                <asp:Label ID="lblVeiculo" runat="server" /><br />
                            </div>
                        </div>
                        <!-- MECANICA -->
                        <hr />
                        <div class="row g-3">
                            <%-- <label>SERVIÇOS MECÂNICOS:</label>       --%>
                            <div class="col-md-12">
                                <asp:Label ID="lblDefeitosMecanicos" runat="server" /><br />
                            </div>
                        </div>
                        <div class="row g-3">
                            <%-- <label>SERVIÇOS EXECUTADOS MECÂNICOS:</label>       --%>
                            <div class="col-md-12">
                                <asp:Label ID="lblServicoExecutadoMecanica" runat="server" /><br />
                            </div>
                        </div>
                        <div class="row g-3">
                            <asp:GridView ID="gvPecasMecanica" runat="server"
                                AutoGenerateColumns="false"
                                CssClass="table table-bordered table-striped">
                                <Columns>
                                    <asp:BoundField DataField="descricao" HeaderText="Peça/Serviço" />
                                    <asp:BoundField DataField="quant" HeaderText="Qtd." />
                                    <asp:BoundField DataField="cracha" HeaderText="Crachá" />
                                    <asp:BoundField DataField="nome" HeaderText="Colaborador" />
                                    <asp:BoundField DataField="inicio" HeaderText="Início" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                                    <asp:BoundField DataField="termino" HeaderText="Término" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                                    <asp:BoundField DataField="tempo_minutos" HeaderText="Tempo (min)" />
                                </Columns>
                            </asp:GridView>
                        </div>
                        <!-- ELETRICA -->
                        <hr />
                        <div class="row g-3">
                            <div class="col-md-12">
                                <asp:Label ID="lblDefeitosEletricos" runat="server" /><br />
                            </div>
                        </div>
                        <div class="row g-3">
                            <div class="col-md-12">
                                <asp:Label ID="lblServicoExecutadoEletrico" runat="server" /><br />
                            </div>
                        </div>
                        <div class="row g-3">
                            <asp:GridView ID="gvPecasEletrica" runat="server"
                                AutoGenerateColumns="false"
                                CssClass="table table-bordered table-striped">
                                <Columns>
                                    <asp:BoundField DataField="descricao" HeaderText="Peça/Serviço" />
                                    <asp:BoundField DataField="quant" HeaderText="Qtd." />
                                    <asp:BoundField DataField="cracha" HeaderText="Crachá" />
                                    <asp:BoundField DataField="nome" HeaderText="Colaborador" />
                                    <asp:BoundField DataField="inicio" HeaderText="Início" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                                    <asp:BoundField DataField="termino" HeaderText="Término" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                                    <asp:BoundField DataField="tempo_minutos" HeaderText="Tempo (min)" />
                                </Columns>
                            </asp:GridView>
                        </div>
                        <!-- BORRACHARIA -->
                        <hr />
                        <div class="row g-3">
                            <div class="col-md-12">
                                <asp:Label ID="lblDefeitosBorracharia" runat="server" /><br />
                            </div>
                        </div>
                        <div class="row g-3">
                            <div class="col-md-12">
                                <asp:Label ID="lblServicoExecutadoBorracharia" runat="server" /><br />
                            </div>
                        </div>
                        <div class="row g-3">
                            <asp:GridView ID="gvPecasBorracharia" runat="server"
                                AutoGenerateColumns="false"
                                CssClass="table table-bordered table-striped">
                                <Columns>
                                    <asp:BoundField DataField="descricao" HeaderText="Peça/Serviço" />
                                    <asp:BoundField DataField="quant" HeaderText="Qtd." />
                                    <asp:BoundField DataField="cracha" HeaderText="Crachá" />
                                    <asp:BoundField DataField="nome" HeaderText="Colaborador" />
                                    <asp:BoundField DataField="inicio" HeaderText="Início" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                                    <asp:BoundField DataField="termino" HeaderText="Término" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                                    <asp:BoundField DataField="tempo_minutos" HeaderText="Tempo (min)" />
                                </Columns>
                            </asp:GridView>
                        </div>
                        <!-- FUNILARIA -->
                        <hr />
                        <div class="row g-3">
                            <div class="col-md-12">
                                <asp:Label ID="lblDefeitosFunilaria" runat="server" /><br />
                            </div>
                        </div>
                        <div class="row g-3">
                            <div class="col-md-12">
                                <asp:Label ID="lblServicoExecutadoFunilaria" runat="server" /><br />
                            </div>
                        </div>
                        <div class="row g-3">
                            <asp:GridView ID="gvPecasFunilaria" runat="server"
                                AutoGenerateColumns="false"
                                CssClass="table table-bordered table-striped">
                                <Columns>
                                    <asp:BoundField DataField="descricao" HeaderText="Peça/Serviço" />
                                    <asp:BoundField DataField="quant" HeaderText="Qtd." />
                                    <asp:BoundField DataField="cracha" HeaderText="Crachá" />
                                    <asp:BoundField DataField="nome" HeaderText="Colaborador" />
                                    <asp:BoundField DataField="inicio" HeaderText="Início" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                                    <asp:BoundField DataField="termino" HeaderText="Término" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                                    <asp:BoundField DataField="tempo_minutos" HeaderText="Tempo (min)" />
                                </Columns>
                            </asp:GridView>
                        </div>

                    </div>

                    <div class="modal-footer">
                        <asp:Button ID="btnFechar" runat="server" Text="Fechar" CssClass="btn btn-secondary" OnClientClick="fecharModal(); return false;" />
                        <%--<asp:Button ID="btnImprimir" runat="server" Text="Imprimir Ordem" CssClass="btn btn-primary" OnClick="btnImprimir_Click" />--%>
                        <asp:LinkButton ID="btnImprimir"
                            runat="server"
                            CssClass="btn btn-primary"
                            OnClick="btnImprimir_Click">
    Imprimir OS
</asp:LinkButton>
                    </div>

                </div>
            </div>
        </div>
    </div>


</asp:Content>
