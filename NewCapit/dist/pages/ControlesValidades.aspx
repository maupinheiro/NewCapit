<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="ControlesValidades.aspx.cs" Inherits="NewCapit.dist.pages.ControlesValidades" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- Bootstrap 5 -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <!-- Chart.js -->
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>

    <script src="https://cdn.datatables.net/1.13.6/js/jquery.dataTables.min.js"></script>
    <link href="https://cdn.datatables.net/1.13.6/css/jquery.dataTables.min.css" rel="stylesheet" />
    <script src="https://code.jquery.com/jquery-3.7.0.min.js"></script>


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
                                <h3 class="card-title"><i class="fas fa-shipping-fast"></i>&nbsp;CONTROLA VALIDADES</h3>
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
                            <div class="row mb-3">
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtPesquisa" runat="server"
                                        CssClass="form-control"
                                        Placeholder="Pesquisar por núcleo, código ou placa" />
                                </div>
                                <div class="col-md-2">
                                    <asp:Button ID="btnPesquisar" runat="server"
                                        Text="Pesquisar"
                                        CssClass="btn btn-primary w-100"
                                        OnClick="btnPesquisar_Click" />
                                    <!--OnClick="btnPesquisar_Click" -->
                                </div>
                            </div>

                            <!-- GRID -->
                            <asp:GridView ID="gvVeiculos" runat="server"
                                CssClass="table table-bordered table-hover"
                                AutoGenerateColumns="False"
                                DataKeyNames="id"
                                OnRowEditing="gvVeiculos_RowEditing"
                                OnRowCancelingEdit="gvVeiculos_RowCancelingEdit"
                                OnRowUpdating="gvVeiculos_RowUpdating"
                                OnRowDataBound="gvVeiculos_RowDataBound">

                                <Columns>

                                    <asp:BoundField DataField="nucleo" HeaderText="Núcleo" ReadOnly="true"/>
                                    <asp:BoundField DataField="codvei" HeaderText="Frota" ReadOnly="true"/>
                                    <asp:BoundField DataField="tipvei" HeaderText="Tipo" ReadOnly="true" />
                                    <asp:BoundField DataField="plavei" HeaderText="Placa" ReadOnly="true" />

                                    <asp:TemplateField HeaderText="Licenciamento"
                                         ItemStyle-HorizontalAlign="Center"
                                         HeaderStyle-HorizontalAlign="Center">
                                       
                                        <EditItemTemplate>                                            
                                            <asp:TextBox ID="txtVencLicenciamento" runat="server"
                                                Text='<%# Bind("venclicenciamento","{0:dd/MM/yyyy}") %>'
                                                CssClass="form-control text-center"
                                                Enabled="false"/>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <%# Eval("venclicenciamento","{0:dd/MM/yyyy}") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Cronotacografo"
                                        ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-HorizontalAlign="Center">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtVencCronotacografo" runat="server"
                                                Text='<%# Bind("venccronotacografo","{0:dd/MM/yyyy}") %>'
                                                CssClass="form-control text-center"
                                                Enabled="false"/>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <%# Eval("venccronotacografo","{0:dd/MM/yyyy}") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Opacidade"
                                        ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-HorizontalAlign="Center">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtVencLaudoFumaca" runat="server"
                                                Text='<%# Bind("vencimentolaudofumaca","{0:dd/MM/yyyy}") %>'
                                                CssClass="form-control text-center"
                                                Enabled="false"/>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <%# Eval("vencimentolaudofumaca","{0:dd/MM/yyyy}") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="CET"
                                        ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-HorizontalAlign="Center">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtVencLicencaCet" runat="server"
                                                Text='<%# Bind("venclicencacet","{0:dd/MM/yyyy}") %>'
                                                CssClass="form-control text-center"
                                                Enabled="false"/>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <%# Eval("venclicencacet","{0:dd/MM/yyyy}") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                  
                                    <asp:TemplateField HeaderText="Protocolo"
                                        ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="txtProtocoloCet" runat="server"
                                                Text='<%# Eval("protocolocet") %>' />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtProtocoloCet" runat="server"
                                                Text='<%# Eval("protocolocet") %>'
                                                CssClass="form-control text-center" 
                                                Enabled="false"/>
                                        </EditItemTemplate>
                                    </asp:TemplateField>

                                    <asp:BoundField DataField="ativo_inativo" HeaderText="Status" ReadOnly="true" />

                                    <asp:TemplateField HeaderText="Ações">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnEditar" runat="server"
                                                CommandName="Edit"
                                                CssClass="btn btn-sm btn-primary"
                                                ToolTip="Editar">
            <i class="fas fa-pencil-alt"></i>
                                            </asp:LinkButton>
                                        </ItemTemplate>

                                        <EditItemTemplate>
                                            <asp:LinkButton ID="btnAtualizar" runat="server"
                                                CommandName="Update"
                                                CssClass="btn btn-sm btn-success me-1"
                                                ToolTip="Salvar">
            <i class="fa fa-save"></i>
                                            </asp:LinkButton>

                                            <asp:LinkButton ID="btnCancelar" runat="server"
                                                CommandName="Cancel"
                                                CssClass="btn btn-sm btn-danger"
                                                ToolTip="Cancelar">
            <i class="fas fa-reply-all"></i>
                                            </asp:LinkButton>
                                        </EditItemTemplate>
                                    </asp:TemplateField>


                                </Columns>
                            </asp:GridView>

                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>


</asp:Content>
