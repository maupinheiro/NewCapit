<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="ControlePneus.aspx.cs" Inherits="NewCapit.dist.pages.ControlePneus" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery.mask/1.14.16/jquery.mask.min.js"></script>
    <script>
        function formatarData(input) {
            let v = input.value.replace(/\D/g, '');

            if (v.length > 2) v = v.substring(0, 2) + '/' + v.substring(2);
            if (v.length > 5) v = v.substring(0, 5) + '/' + v.substring(5, 9);

            input.value = v;
        }
    </script>

    <div class="content-wrapper">
        <section class="content">
            <div class="container-fluid">
                <br />
                <div class="col-md-12">
                    <div class="card card-info">
                        <!-- Header -->
                        <div class="card-header" style="background-color: #A020F0; font-weight: bold;">
                            <h3 class="card-title"><i class="fas fa-shipping-fast"></i>&nbsp;Manutenção</h3>
                        </div>

                        <!-- Controle de Pneus -->
                        <div class="card-body">
                            <div class="card-header bg-secondary text-white">
                                Controle de Pneus
                            </div>
                            <div id="divMsg" runat="server" class="alert alert-dismissible fade show mt-3" role="alert" visible="false">
                                <asp:Label ID="lblMsgGeral" runat="server"></asp:Label>
                                <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
                            </div>                            
                            <asp:GridView 
                                ID="gvPneus" 
                                runat="server" 
                                AutoGenerateColumns="false"
                                HeaderStyle-CssClass="gv-header-custom"
                                CssClass="table table-bordered"
                                OnRowCommand="gvPneus_RowCommand">
                                <Columns>
                                    <asp:BoundField DataField="Id" HeaderText="ID" />
                                    <asp:BoundField DataField="Numero" HeaderText="Número" />
                                    <asp:BoundField DataField="descricao" HeaderText="Descrição" />
                                    <asp:BoundField DataField="Marca" HeaderText="Marca" />
                                    <asp:BoundField DataField="Modelo" HeaderText="Modelo" />
                                    <asp:BoundField DataField="Medida" HeaderText="Medida" />
                                    <asp:BoundField DataField="KMAtual" HeaderText="KM Atual" />
                                    <asp:BoundField DataField="Status" HeaderText="Status" />
                                    <asp:BoundField DataField="placa" HeaderText="Placa" />
                                    <asp:BoundField DataField="datainstalacao" HeaderText="Instalação" DataFormatString="{0:dd/MM/yyyy}" />
                                     <asp:BoundField DataField="posicao" HeaderText="Posição" />

                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" CommandName="Editar" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-warning btn-sm">Editar</asp:LinkButton>
                                            <asp:LinkButton runat="server" CommandName="Excluir" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-danger btn-sm">Descartar</asp:LinkButton>
                                            <asp:LinkButton runat="server" CommandName="Movimentar" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-info btn-sm">Movimentar</asp:LinkButton>

                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>

                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>
   
    <!-- MODAL MOVIMENTAÇÃO -->
    <div class="modal fade" id="modalMov" tabindex="-1">
        <div class="modal-dialog">
            <div class="modal-content">

                <div class="modal-header">
                    <h5>Movimentar Pneu</h5>
                </div>

                <div class="modal-body">
                    <asp:HiddenField ID="hfPneuMov" runat="server" />

                    <label>Veículo</label>
                    <asp:DropDownList ID="ddlVeiculo" runat="server" CssClass="form-control" />

                    <label class="mt-2">Posição</label>
                    <asp:DropDownList ID="ddlPosicao" runat="server" CssClass="form-control">
                        <asp:ListItem>Dianteiro Esquerdo</asp:ListItem>
                        <asp:ListItem>Dianteiro Direito</asp:ListItem>
                        <asp:ListItem>Tração</asp:ListItem>
                        <asp:ListItem>Carreta</asp:ListItem>
                    </asp:DropDownList>

                    <label class="mt-2">KM</label>
                    <asp:TextBox ID="txtKMMov" runat="server" CssClass="form-control" />
                </div>

                <div class="modal-footer">
                    <asp:Button ID="btnSalvarMov" runat="server" Text="Salvar" CssClass="btn btn-success" OnClick="SalvarMovimentacao" />
                </div>
            </div>
        </div>
    </div>

    <!-- MODAL RECAPAGEM -->
    <div class="modal fade" id="modalRecap" tabindex="-1">
        <div class="modal-dialog">
            <div class="modal-content">

                <div class="modal-header">
                    <h5>Recapagem</h5>
                </div>

                <div class="modal-body">
                    <asp:HiddenField ID="hfPneuRecap" runat="server" />

                    <label>Custo</label>
                    <asp:TextBox ID="txtCusto" runat="server" CssClass="form-control" />

                    <label class="mt-2">KM</label>
                    <asp:TextBox ID="txtKMRecap" runat="server" CssClass="form-control" />
                </div>

                <div class="modal-footer">
                    <asp:Button ID="btnSalvarRecap" runat="server" Text="Salvar" CssClass="btn btn-warning" OnClick="SalvarRecapagem" />
                </div>
            </div>
        </div>
    </div>
     <!-- MODAL CADASTRO COMPLETO -->
 <div class="modal fade" id="modalPneu" tabindex="-1">
     <div class="modal-dialog">
         <div class="modal-content">
             <div class="modal-header">
                 <h5 class="modal-title">Cadastro Completo</h5>
                 <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
             </div>
             <div class="modal-body">

                 <asp:HiddenField ID="hfId" runat="server" />

                 <div class="row">
                     <div class="col-md-4">
                         <label>Número*</label>
                         <asp:TextBox ID="txtNumero" runat="server" CssClass="form-control" />
                     </div>

                     <div class="col-md-4">
                         <label>Marca*</label>
                         <asp:TextBox ID="txtMarca" runat="server" CssClass="form-control" />
                     </div>

                     <div class="col-md-4">
                         <label>Modelo</label>
                         <asp:TextBox ID="txtModelo" runat="server" CssClass="form-control" />
                     </div>

                     <div class="col-md-4 mt-2">
                         <label>Medida</label>
                         <asp:TextBox ID="txtMedida" runat="server" CssClass="form-control" placeholder="295/80 R22.5" />
                     </div>

                     <div class="col-md-4 mt-2">
                         <label>KM Atual*</label>
                         <asp:TextBox ID="txtKM" runat="server" CssClass="form-control" />
                     </div>

                     <div class="col-md-4 mt-2">
                         <label>Status</label>
                         <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                             <asp:ListItem>Estoque</asp:ListItem>
                             <asp:ListItem>Em Uso</asp:ListItem>
                             <asp:ListItem>Reforma</asp:ListItem>
                             <asp:ListItem>Descartado</asp:ListItem>
                         </asp:DropDownList>
                     </div>
                 </div>

                 <asp:Label ID="lblErro" runat="server" ForeColor="Red" />

             </div>
             <div class="modal-footer">
                 <asp:Button ID="btnSalvar" runat="server" Text="Salvar" CssClass="btn btn-success" OnClick="btnSalvar_Click" />
                 <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Fechar</button>
             </div>
         </div>
     </div>
 </div>
 <script>
     function abrirModal() {
         var modal = new bootstrap.Modal(document.getElementById('modalPneu'));
         modal.show();
     }
     </script>


</asp:Content>
