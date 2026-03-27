<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="ListaOS.aspx.cs" Inherits="NewCapit.dist.pages.ListaOS" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
                                <h3 class="card-title"><i class="fas fa-shipping-fast"></i>&nbsp;Manutenção - Abertura de Ordem de Serviço</h3>
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
                                    Lista Ordens de Serviço
                                </div>

                                <div class="card-body">

                                    <div class="row">

                                        <div class="col-md-2">
                                            <asp:TextBox ID="txtPesquisa" runat="server"
                                                CssClass="form-control"
                                                Placeholder="placa, motorista, fornecedor..."></asp:TextBox>
                                        </div>

                                        <div class="col-md-1">
                                            <asp:TextBox ID="txtOrdem_Servico" runat="server"
                                                CssClass="form-control"
                                                Placeholder="OS"></asp:TextBox>
                                        </div>

                                        <div class="col-md-1">
                                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                                                <asp:ListItem Value="">Status</asp:ListItem>
                                                <asp:ListItem Value="1">Aberta</asp:ListItem>
                                                <asp:ListItem Value="2">Finalizada</asp:ListItem>
                                                <asp:ListItem Value="3">Cancelada</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>

                                        <div class="col-md-1">
                                            <asp:DropDownList ID="ddlManutencao" runat="server" CssClass="form-control">
                                                <asp:ListItem Value="">Tipo OS</asp:ListItem>
                                                <asp:ListItem Value="C">Corretiva</asp:ListItem>
                                                <asp:ListItem Value="P">Preventiva</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-1">
                                            <asp:DropDownList ID="ddlServico" runat="server" CssClass="form-control">
                                                <asp:ListItem Value="">Serviço</asp:ListItem>
                                                <asp:ListItem Value="I">Interno</asp:ListItem>
                                                <asp:ListItem Value="E">Externo</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>

                                        <div class="col-md-2">
                                            <asp:TextBox ID="txtDataInicial" runat="server"
                                                CssClass="form-control"
                                                TextMode="Date"></asp:TextBox>
                                        </div>

                                        <div class="col-md-2">
                                            <asp:TextBox ID="txtDataFinal" runat="server"
                                                CssClass="form-control"
                                                TextMode="Date"></asp:TextBox>
                                        </div>

                                        <div class="col-md-1">
                                            <asp:Button ID="btnPesquisar" runat="server"
                                                Text="Pesquisar"
                                                CssClass="btn btn-primary"
                                                OnClick="PesquisarOS" />
                                        </div>
                                        <div class="col-md-1">
                                            <asp:Button ID="btnAbrirOs" runat="server"
                                                Text="Abrir O.S."
                                                CssClass="btn btn-success"
                                                 OnClick="btnAbrirOs_Click"
                                                 />
                                        </div>

                                    </div>
                                    <br />

                                    <asp:GridView ID="gvOS" runat="server"
                                        CssClass="table table-bordered table-hover"
                                        AutoGenerateColumns="False"
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
                                                        Visualizar PDF
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

                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>


</asp:Content>
