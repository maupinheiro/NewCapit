<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="ControleDePedagio.aspx.cs" Inherits="NewCapit.dist.pages.ControleDePedagio" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- Bootstrap 5 -->
<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
<!-- Chart.js -->
<script src="https://cdn.jsdelivr.net/npm/chart.js"></script>

<script src="https://cdn.datatables.net/1.13.6/js/jquery.dataTables.min.js"></script>
<link href="https://cdn.datatables.net/1.13.6/css/jquery.dataTables.min.css" rel="stylesheet" />
<script src="https://code.jquery.com/jquery-3.7.0.min.js"></script>
    <div class="content-wrapper">
        <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server" />
        <section class="content">
            <div class="container-fluid">               
                <br />
                <div class="col-md-12">
                    <div class="card card-info">
                        <div class="card-header">
                            <h3 class="card-title">
                                <h3 class="card-title"><i class="fas fa-map-marker-alt"></i>&nbsp;CONTROLE DE PEDÁGIOS</h3>
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
                            <div class="container-fluid">
                                <asp:UpdatePanel ID="upLista" runat="server">
                                 <ContentTemplate>
                                        <table class="table table-striped table-hover table-bordered">
                                            <thead class="table-dark">
                                                <tr>
                                                    <th>Motorista</th> 
                                                    <th>Transportadora</th>
                                                    <th>Placa</th>                                                     
                                                    <th>Local de Coleta</th>
                                                    <th>Local de Entrega</th> 
                                                    <th>Ação</th>
                                                </tr>
                                            </thead>

                                            <tbody>
                                                <asp:Repeater ID="rpCarregamentos" runat="server"
                                                    OnItemCommand="rpCarregamentos_ItemCommand">

                                                    <ItemTemplate>
                                                        <tr>
                                                            <td>
                                                                <%# Eval("nomemotorista") %>
                                                                <br />
                                                                <%# Eval("cpf") %>
                                                            </td>
                                                            
                                                            <td>
                                                                <%# Eval("transportadora") %>
                                                                <br />
                                                                <%# Eval("cpf_cnpj_proprietario") %>
                                                            </td>
                                                            <td>
                                                                <%# Eval("placa")  + " (" + Eval("eixos") + ") eixos" %>
                                                                <br />
                                                                <%# Eval("tipoveiculo") %>   
                                                            </td>  
                                                            <td>
                                                                <%# Eval("expedidor") %>
                                                                <br />
                                                                <%# Eval("cid_expedidor") + "/" + Eval("uf_expedidor") %>
                                                            </td>
                                                            <td>
                                                                <%# Eval("recebedor") %>
                                                                <br />
                                                                <%# Eval("cid_recebedor") + "/" + Eval("uf_recebedor") %>
                                                            </td>

                                                            <td>
                                                                <asp:LinkButton runat="server"
                                                                    CommandName="Editar"
                                                                    CommandArgument='<%# Eval("id") %>'
                                                                    CssClass="btn btn btn-primary">
                                                                    <i class="bi bi-pencil"></i>
                                                                </asp:LinkButton>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>

                                                </asp:Repeater>
                                            </tbody>
                                        </table>

                                    </ContentTemplate>
                                </asp:UpdatePanel>
                               
                            </div>
                        </div>
                    </div>
                </div>
                <!-- form Pedagio -->
             <div class="modal fade" id="modalPedagio" tabindex="-1">
                <div class="modal-dialog modal-xl modal-dialog-centered">
                    <div class="modal-content">

                        <div class="modal-header bg-primary text-white">
                            <h5 class="modal-title">
                                <i class="bi bi-cash-coin"></i> Emissão de Pedágio
                            </h5>
                            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>

                        </div>

                        <div class="modal-body">
                            <asp:HiddenField ID="hdIdCarregamento" runat="server" />
                            <div class="row g-3">
                                <div class="col-md-2">
                                    <div class="form-group">
                                        <span class="details">Ordem Coleta:</span>
                                        <asp:TextBox ID="txtNum_Carregamento" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>                                        
                                    </div>
                                </div>
                                <div class="col-md-2">
                                    <div class="form-group">
                                        <span class="details">Emissão:</span>
                                        <asp:TextBox ID="txtEmissaoPedagio" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>                                        
                                    </div>
                                </div>
                                <div class="col-2"></div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <span class="details">Responsável:</span>
                                        <asp:TextBox ID="txtCreditadoPor" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>                                        
                                    </div>
                                </div>

                            </div>
                            <div class="row g-3">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <span class="details">Origem:</span>
                                        <asp:TextBox ID="txtExpedidorPedagio" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>                                        
                                    </div>
                                </div>
                                <div class="col-md-5">
                                    <div class="form-group">
                                        <span class="details">Cidade:</span>
                                        <asp:TextBox ID="txtCid_ExpedidorPedagio" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>                                        
                                    </div>
                                </div>
                                <div class="col-md-1">
                                    <div class="form-group">
                                        <span class="details">UF:</span>
                                        <asp:TextBox ID="txtUf_ExpedidorPedagio" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>                                        
                                    </div>
                                </div>

                            </div>
                            <div class="row g-3">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <span class="details">Destino:</span>
                                        <asp:TextBox ID="txtRecebedorPedagio" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>                                        
                                    </div>
                                </div>
                                <div class="col-md-5">
                                    <div class="form-group">
                                        <span class="details">Cidade:</span>
                                        <asp:TextBox ID="txtCid_RecebedorPedagio" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>                                        
                                    </div>
                                </div>
                                <div class="col-md-1">
                                    <div class="form-group">
                                        <span class="details">UF:</span>
                                        <asp:TextBox ID="txtUf_RecebedorPedagio" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>                                        
                                    </div>
                                </div>

                            </div>
                            <div class="row g-3">
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <span class="details">Motorista:</span>
                                        <asp:TextBox ID="txtMotoristaPedagio" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>                                        
                                    </div>
                                </div>
                            </div>
                            <div class="row g-3">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <span class="details">Proprietário:</span>
                                        <asp:TextBox ID="txtProprietarioPedagio" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>                                        
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <span class="details">Veículos:</span>
                                        <asp:TextBox ID="txtVeiculoPedagio" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>                                        
                                    </div>
                                </div>

                            </div>
                            <div class="row g-3">                                
                                 <div class="col-md-6">
                                     <div class="form-group">
                                         <span class="details">Pagador Pedágio de Ida:</span>
                                         <asp:TextBox ID="txtTomadorServicoPedagio" runat="server" class="form-control"></asp:TextBox>                                        
                                     </div>
                                 </div>
                                 <div class="col-md-6">
                                     <div class="form-group">
                                         <span class="details">Pagador Pedágio de Volta:</span>
                                         <asp:TextBox ID="txtPagadorPedagioVolta" runat="server" class="form-control"></asp:TextBox>                                        
                                     </div>
                                </div>
                             </div>
                            <div class="row g-3">
                                <div class="col-md-2">
                                    <div class="form-group">
                                        <span class="details">Documento</span>
                                        <asp:TextBox ID="txtDocumentoPedagio" runat="server" class="form-control"></asp:TextBox>                                        
                                    </div>
                                </div>
                                <div class="col-md-2">
                                    <div class="form-group">
                                        <span class="details">Comprovante:</span>
                                        <asp:TextBox ID="txtComprovantePedagio" runat="server" class="form-control"></asp:TextBox>                                        
                                    </div>
                                </div>
                                <div class="col-md-2">
                                    <div class="form-group">
                                        <span class="details">Valor Creditado:</span>
                                        <asp:TextBox ID="txtValorPedagio" runat="server" class="form-control"></asp:TextBox>                                        
                                    </div>
                               </div>
                               <div class="col-md-6">
                                     <div class="form-group">
                                         <span class="details">Observação:</span>
                                         <asp:TextBox ID="txtObservacaoPedagio" runat="server" class="form-control"></asp:TextBox>                                        
                                     </div>
                               </div>
                            </div>
                            

                            <asp:Label ID="lblErro" runat="server"
                                CssClass="text-danger fw-bold" />
                        </div>

                        <div class="modal-footer">
                            <asp:Button ID="btnSalvarPedagio" runat="server"
                                Text="Salvar"
                                CssClass="btn btn-success"
                                OnClick="btnSalvarPedagio_Click" />
                        </div>

                    </div>
                </div>
            </div>                
            </div>
        </section>
    </div>
</asp:Content>
