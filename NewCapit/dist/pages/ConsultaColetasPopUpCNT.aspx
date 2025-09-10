<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConsultaColetasPopUpCNT.aspx.cs" Inherits="NewCapit.dist.pages.ConsultaColetasPopUp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>NewCapit - Consulta Solicitações</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />

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

    <!-- Script para fechar modal -->
    <script type="text/javascript">
        function fecharModalOcorrencia() {
            $('#modalOcorrencia').modal('hide');
        }
    </script>
</head>
<body>

    <form id="form1" runat="server" class="p-3">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <%--<asp:Timer ID="timerAtualiza" runat="server" OnTick="timerAtualiza_Tick" Interval="30000"></asp:Timer>--%>
        <br />
        <div class="row g-3">
            <div class="card-header">
            <asp:TextBox ID="myInput" CssClass="form-control myInput" OnTextChanged="myInput_TextChanged" placeholder="Pesquisar ..." AutoPostBack="true" runat="server"></asp:TextBox>
        </div>
           <%-- <div>
                <asp:Button ID="btnAtualizar" runat="server" CssClass="btn btn-success" Text="Atualizar" OnClick="btnAtualizar_Click" />
            </div>--%>
        </div>
        
        <br />
        <%--<asp:GridView ID="GVColetas" runat="server" CssClass="table table-bordered table-striped" AutoGenerateColumns="false" OnRowCommand="GVColetas_RowCommand">--%>
        <asp:GridView ID="GVColetas" runat="server" CssClass="table table-bordered table-striped table-hover" Width="100%" AutoGenerateColumns="False" OnRowCommand="GVColetas_RowCommand" OnRowDataBound="GVColetas_RowDataBound">
            <Columns>
                <asp:BoundField DataField="id" HeaderText="ID" Visible="false" />
                <asp:BoundField DataField="carga" HeaderText="COLETA" Visible="false" />
                <asp:BoundField DataField="cva" HeaderText="CVA" />
                <asp:BoundField DataField="data_hora" HeaderText="DATA/HORA" />
                <asp:BoundField DataField="atendimento" HeaderText="ATENDIMENTO" />
                <asp:BoundField DataField="cliorigem" HeaderText="LOCAL DA COLETA" />
                <asp:BoundField DataField="clidestino" HeaderText="DESTINO" />
                <asp:BoundField DataField="veiculo" HeaderText="VEICULO" />
                <asp:BoundField DataField="tipo_viagem" HeaderText="VIAGEM" />
                <asp:BoundField DataField="solicitacoes" HeaderText="SOLICITAÇÕES" />
                <asp:BoundField DataField="peso" HeaderText="PESO" />
                <asp:BoundField DataField="pedidos" HeaderText="METRAGEM" />
                <asp:BoundField DataField="andamento" HeaderText="SITUAÇÃO" Visible="false" />

                <asp:TemplateField HeaderText="AÇÃO" ShowHeader="True">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnDetalhes" runat="server" Text="Ocorrencias" CommandName="Ocorrencias" CommandArgument='<%# Eval("carga") %>' class="btn btn-danger"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Motorista" ShowHeader="True">
                     <ItemTemplate>
                         <asp:LinkButton ID="btnAtualizar" runat="server" Text="Atualizar" CommandName="Motoristas" CommandArgument='<%# Eval("carga") %>' class="btn btn-success"></asp:LinkButton>
                     </ItemTemplate>
                 </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <!-- Modal Ocorrências -->
        <div class="modal fade bd-example-modal-xl" id="modalOcorrencia" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered modal-xl" role="document">
                <div class="modal-content">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div class="modal-header">
                                <h5 class="modal-title" id="exampleModalCenterTitle">Ocorrências</h5>
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                            </div>
                            <div class="modal-body">
                                <div class="row g-3">
                                    <div class="col-md-2">
                                        <div class="form-group">
                                            <asp:Label ID="lblCVA" runat="server" class="form-control font-weight-bold" Style="text-align: center">  
                                            </asp:Label>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <asp:Label ID="lblColeta" runat="server" class="form-control font-weight-bold" Style="text-align: center">  
                                            </asp:Label>
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <asp:Label ID="lblStatus" runat="server" class="form-control font-weight-bold" Style="text-align: center">  
                                            </asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <div class="row g-3">
                                    <div class="col-md-5">
                                        <div class="form_group">
                                            <span class="details">RESPONSÁVEL:</span>
                                            <asp:DropDownList ID="cboResponsavel" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="cboResponsavel_SelectedIndexChanged">
                                            </asp:DropDownList><br />
                                        </div>
                                    </div>
                                    <div class="col-md-1"></div>
                                    <div class="col-md-6">
                                        <div class="form_group">
                                            <span class="details">OCORRÊNCIA:</span>
                                            <asp:DropDownList ID="cboMotivo" runat="server" CssClass="form-control">
                                            </asp:DropDownList><br />
                                        </div>
                                    </div>
                                </div>
                                <div class="row g-3">
                                    <div class="col-md-12">
                                        <div class="form_group">
                                            <span class="details">OBSERVAÇÃO:</span>
                                            <asp:TextBox ID="txtObservacao" runat="server" class="form-control font-weight-bold" Rows="3" TextMode="MultiLine" placeholder="Ocorrências ..."></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <br />
                                <div class="row g-3">
                                    <div class="col-md-12">
                                        <!-- /.card-header -->
                                        <div class="card-body table-responsive p-0" style="height: 200px;">
                                            <table class="table table-head-fixed text-nowrap">
                                                <asp:GridView runat="server" ID="GridViewCarga" CssClass="table table-bordered table-striped table-hover" Width="100%" AutoGenerateColumns="False">
                                                    <Columns>
                                                        <asp:BoundField DataField="id" HeaderText="#ID" Visible="false" />
                                                        <asp:BoundField DataField="responsavel" HeaderText="RESPONSÁVEL" />
                                                        <asp:BoundField DataField="motivo" HeaderText="OCORRÊNCIA" />
                                                        <asp:BoundField DataField="observacao" HeaderText="OBSERVAÇÃO" />
                                                        <asp:BoundField DataField="data_inclusao" HeaderText="DATA   " />
                                                        <asp:BoundField DataField="usuario_inclusao" HeaderText="USUÁRIO" />

                                                        <asp:TemplateField HeaderText="AÇÕES" ShowHeader="True">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkExcluir" runat="server" class="btn btn-danger"><i class="fas fa-trash-alt"></i></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </table>

                                        </div>
                                        <!-- /.card-body -->
                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <button type="button" id="btnFechar" runat="server" class="btn btn-secondary" data-dismiss="modal" onclick="btnFechar_Click">Fechar</button>
                                    <asp:Button ID="btnSalvarOcorrencia" runat="server" Text="Salvar" OnClick="btnSalvarOcorrencia_Click" class="btn btn-primary" />
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>

        <footer class="main-footer">
            <div class="float-right d-none d-sm-block">
                <b>Version</b> 3.1.0 
            </div>
            <strong>Copyright &copy; 2023-2025 <a href="#">Capit Logística</a>.</strong> Todos os direitos reservados.
        </footer>


    </form>
</body>
</html>
