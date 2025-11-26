<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="SimuladorFrete.aspx.cs" Inherits="NewCapit.dist.pages.SimuladorFrete" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        function showToast(msg) {
            document.getElementById("toastBody").innerText = msg;
            var toast = new bootstrap.Toast(document.getElementById("toastMsg"));
            toast.show();
        }
    </script>

    <div class="content-wrapper">
        <section class="content">
            <div class="container-fluid">
                <br />
                <!-- Toast Bootstrap -->
                <div aria-live="polite" aria-atomic="true" class="position-relative">
                    <div class="toast-container position-fixed bottom-0 end-0 p-3">

                        <div id="toastMsg" class="toast text-bg-success border-0" role="alert" aria-live="assertive" aria-atomic="true">
                            <div class="toast-header">
                                <strong class="me-auto">Mensagem</strong>
                                <small>Agora</small>
                                <button type="button" class="btn-close" data-bs-dismiss="toast"></button>
                            </div>
                            <div class="toast-body" id="toastBody">
                                Operação realizada com sucesso.
                            </div>
                        </div>

                    </div>
                </div>

                <div class="col-md-12">
                    <div class="card card-info">
                        <div class="card-header">
                            <h3 class="card-title">
                                <h3 class="card-title"><i class="fas fa-shipping-fast"></i>&nbsp;SIMULADOR DE FRETE</h3>
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
                            <div class="row">
                                <div class="col-md-12">
                                    <div id="cardComposicao" runat="server" class="card collapsed-card">

                                        <div class="card-header">
                                            <h3 class="card-title">Composição do Frete</h3>

                                            <div class="card-tools">
                                                <button type="button" class="btn btn-tool" data-card-widget="collapse">
                                                    <i class="fas fa-plus"></i>
                                                </button>
                                            </div>
                                            <!-- /.card-tools -->
                                        </div>
                                        <!-- /.card-header -->
                                        <div class="card-body">
                                            <asp:GridView ID="GridView1" runat="server" CssClass="table table-bordered table-striped table-hover"
                                                AutoGenerateColumns="False"
                                                DataKeyNames="Id"
                                                OnRowEditing="GridView1_RowEditing"
                                                OnRowCancelingEdit="GridView1_RowCancelingEdit"
                                                OnRowUpdating="GridView1_RowUpdating"
                                                OnRowDataBound="GridView1_RowDataBound">

                                                <Columns>

                                                    <asp:BoundField DataField="Id" HeaderText="ID" ReadOnly="True" Visible="false" />

                                                    <asp:BoundField DataField="Modal" HeaderText="Modal" ReadOnly="True" />
                                                    <%--<asp:TemplateField HeaderText="Modal" ReadOnly="True">
                                                        <ItemTemplate>
                                                            <%# Eval("Modal") %>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtModal" runat="server" CssClass="form-control"
                                                                Text='<%# Bind("Modal") %>' />
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>--%>

                                                    <asp:TemplateField HeaderText="Custo por KM">
                                                        <ItemTemplate>
                                                            <%# String.Format("{0:N2}", Eval("CustoPorKm")) %>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtCustoPorKm" runat="server" CssClass="form-control"
                                                                Text='<%# Bind("CustoPorKm") %>' />
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Taxa Fixa">
                                                        <ItemTemplate>
                                                            <%# String.Format("{0:N2}", Eval("TaxaFixa")) %>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtTaxaFixa" runat="server" CssClass="form-control"
                                                                Text='<%# Bind("TaxaFixa") %>' />
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="% Seguro">
                                                        <ItemTemplate>
                                                            <%# String.Format("{0:N2}", Eval("SeguroPercentual")) %>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtSeguroPercentual" runat="server" CssClass="form-control"
                                                                Text='<%# Bind("SeguroPercentual") %>' />
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Adicional Combustível">
                                                        <ItemTemplate>
                                                            <%# String.Format("{0:N2}", Eval("AdicionalCombustivel")) %>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtAdicionalCombustivel" runat="server" CssClass="form-control"
                                                                Text='<%# Bind("AdicionalCombustivel") %>' />
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Taxa Minima">
                                                        <ItemTemplate>
                                                            <%# String.Format("{0:N2}", Eval("TakMinima")) %>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtTakMinima" runat="server" CssClass="form-control"
                                                                Text='<%# Bind("TakMinima") %>' />
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="% G.Risco">
                                                        <ItemTemplate>
                                                            <%# String.Format("{0:N2}", Eval("GrisPercent")) %>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtGrisPercent" runat="server" CssClass="form-control"
                                                                Text='<%# Bind("GrisPercent") %>' />
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:CommandField ShowEditButton="True"
                                                        EditText="Editar"
                                                        UpdateText="Salvar"
                                                        CancelText="Cancelar"
                                                        ControlStyle-CssClass="btn btn-primary btn-sm"
                                                        ButtonType="Button" />

                                                </Columns>

                                            </asp:GridView>
                                        </div>
                                        <!-- /.card-body -->
                                    </div>
                                    <!-- /.card -->
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-md-12">
                                    <div id="cardComposicao2" runat="server" class="card collapsed-card">
                                        <div class="card-header">
                                            <h3 class="card-title">Dados do Frete</h3>

                                            <div class="card-tools">
                                                <button type="button" class="btn btn-tool" data-card-widget="collapse">
                                                    <i class="fas fa-plus"></i>
                                                </button>
                                            </div>
                                            <!-- /.card-tools -->
                                        </div>
                                        <!-- /.card-header -->
                                        <div class="card-body">
                                            <div class="row mb-3">
                                                <div class="col-md-5">
                                                    <label>Origem:</label>
                                                    <asp:TextBox ID="txtOrigem" runat="server" CssClass="form-control"></asp:TextBox>
                                                </div>
                                                <div class="col-md-5">
                                                    <label>Destino:</label>
                                                    <asp:TextBox ID="txtDestino" runat="server" CssClass="form-control"></asp:TextBox>
                                                </div>
                                                <div class="col-md-1">
                                                    <label>&nbsp;</label>
                                                    <asp:Button ID="btnDistancia" runat="server" Text="Calcular Distância" CssClass="btn btn-primary" OnClick="btnDistancia_Click" />

                                                </div>

                                            </div>
                                            <div class="row mb-3">
                                                <div class="col-md-2">
                                                    <label>Distância (Km):</label>
                                                    <asp:TextBox ID="txtDistancia" runat="server" CssClass="form-control"></asp:TextBox>
                                                </div>
                                                <div class="col-md-2">
                                                    <label>Peso (Kg):</label>
                                                    <asp:TextBox ID="txtPeso" runat="server" CssClass="form-control"></asp:TextBox>
                                                </div>
                                                <div class="col-md-2">
                                                    <label>Modal:</label>
                                                    <asp:DropDownList ID="ddlModal" runat="server" CssClass="form-control"></asp:DropDownList>
                                                </div>
                                                <div class="col-md-1">
                                                    <label>&nbsp;</label>
                                                    <asp:Button ID="btnCalcular" runat="server" Text="Calcular Frete" CssClass="btn btn-primary" OnClick="btnCalcular_Click" />
                                                </div>
                                            </div>
                                            <div class="row mb-3">
                                                <h4>Resultado:</h4>
                                                <asp:Label ID="lblResultado" runat="server" />

                                                <h4>Comparativo entre Modais</h4>
                                                <asp:GridView ID="gridComparativo" runat="server" AutoGenerateColumns="true" CssClass="table table-bordered table-striped table-hover" />
                                            </div>
                                        </div>
                                        <!-- /.card-body -->
                                    </div>
                                    <!-- /.card -->
                                </div>
                            </div>
                            <asp:HiddenField ID="hfCollapse" runat="server" />

                        </div>
                    </div>
                </div>
            </div>
        </section>
        <script>
            // Quando um collapse for aberto, salva o ID
            document.addEventListener('shown.bs.collapse', function (e) {
                document.getElementById("<%= hfCollapse.ClientID %>").value = e.target.id;
            });

            // Quando for fechado, limpa a memória
            document.addEventListener('hidden.bs.collapse', function (e) {
                if (document.getElementById("<%= hfCollapse.ClientID %>").value === e.target.id)
                    document.getElementById("<%= hfCollapse.ClientID %>").value = "";
            });
        </script>
    </div>
</asp:Content>
