<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="ConsultaColetasCNT.aspx.cs" Inherits="NewCapit.dist.pages.ConsultaColetasCNT" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="content-wrapper">
        <!-- Main content -->
        <section class="content">
            <div class="container-fluid">                
               <div class="content-header">
                    <div class="row g-3">
                       <div class="col-md-10">                          
                             <h1 class="h3 mb-2 text-gray-800">
                               <i class="fas fa-shipping-fast"></i>&nbsp;Coletas</h1> 
                       </div>
                       <div class="col-md-1 text-center">
                            <input type="text" class="knob" value="70" data-width="90" data-height="90" data-fgcolor="#f56954">
                            <div class="knob-label">Atrasadas</div>
                       </div>
                       <div class="col-md-1 text-center">
                          <input type="text" class="knob" value="80" data-width="90" data-height="90" data-fgColor="#3c8dbc">
                          <div class="knob-label">No Prazo</div>
                       </div>
                        
                  </div>
               </div>
                
                <div class="row g-3">
                    <div class="col-md-2">
                        <div class="form-group">
                            <span class="">Coleta Inicial:</span>
                            <div class="input-group date">
                                <asp:TextBox ID="txtInicioData" CssClass="form-control" TextMode="Date" runat="server"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <div class="form-group">
                            <span class="">Coleta Final:</span>
                            <div class="input-group date">
                                <asp:TextBox ID="txtFimData" CssClass="form-control" TextMode="Date" runat="server"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <div class="form-group">
                            <span class="">Status:</span>
                            <asp:DropDownList ID="ddlStatus" ame="nomeStatus" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <div class="form_group">
                            <span class="details">VEÍCULO:</span>
                            <asp:DropDownList ID="ddlVeiculos" runat="server" CssClass="form-control"> 
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-1">
                        <br />
                        <asp:LinkButton ID="lnkPesquisar" runat="server" CssClass="btn btn-info" OnClick="lnkPesquisar_Click"><i class='fas fa-search' ></i>
    Pesquisar</asp:LinkButton>
                    </div>
                   
                </div>
            </div>
            <!-- /.container-fluid -->
        </section>
        <!-- Main content -->

        <section class="content">
            <div class="container-fluid">
                <div class="row">
                    <div class="col-12">
                        <!-- /.col -->
                        <div class="card">
                            <!-- /.card-header -->
                            <div class="card-body table-responsive p-0" style="height: 640px;">
                                <table class="table table-head-fixed text-nowrap">
                                    <asp:GridView runat="server" ID="gvListCargas" CssClass="table table-bordered table-striped table-hover" Width="100%" AutoGenerateColumns="False" DataKeyNames="id" AllowPaging="True" PageSize="10" OnPageIndexChanging="gvListCargas_PageIndexChanging" ShowHeaderWhenEmpty="True">
                                        <PagerStyle HorizontalAlign="Center" CssClass="pagination-centered" />
                                        <Columns>
                                            <asp:BoundField DataField="id" HeaderText="COLETA" />
                                            <asp:BoundField DataField="data_hora" HeaderText="DATA/HORA" />
                                            <asp:BoundField DataField="solicitacoes" HeaderText="SOLICITAÇÃO(ES)" />
                                            <asp:BoundField DataField="" HeaderText="ATENDIMENTO" />
                                            <asp:BoundField DataField="cliorigem" HeaderText="LOCAL DA COLETA" />
                                            <asp:BoundField DataField="clidestino" HeaderText="DESTINO" />
                                            <asp:BoundField DataField="veiculo" HeaderText="VEICULO" />
                                            <asp:BoundField DataField="tipo_viagem" HeaderText="VIAGEM" />
                                            <asp:BoundField DataField="rota" HeaderText="ROTA" />
                                            <asp:BoundField DataField="andamento" HeaderText="SITUAÇÃO" />

                                            <asp:TemplateField HeaderText="AÇÕES" ShowHeader="True">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkEditar" runat="server" CssClass="btn btn-primary btn-sm"><i class="fas fa-tasks"></i></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </table>
                                <asp:HiddenField ID="txtconformmessageValue" runat="server" />
                            </div>
                            <!-- /.card-body -->
                        </div>
                        <!-- /.card -->
                    </div>
                </div>
                <!-- /.row -->
            </div>
            <!-- /.container-fluid -->
        </section>
        <!-- /.content -->
    </div>
    <!-- /.content-wrapper -->
    <footer class="main-footer">
        <div class="float-right d-none d-sm-block">
            <b>Version</b> 3.1.0 
        </div>
        <strong>Copyright &copy; 2023-2025 <a href="#">Capit Logística</a>.</strong> Todos os direitos reservados.
    </footer>

</asp:Content>
