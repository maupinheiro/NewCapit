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
</head>
<body>

    <form id="form1" runat="server" class="p-3">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <asp:Timer ID="timerAtualiza" runat="server" OnTick="timerAtualiza_Tick" Interval="30000"></asp:Timer>
        <br />
        <div>
            <asp:Button ID="btnAtualizar" runat="server" CssClass="btn btn-success" Text="Atualizar" OnClick="btnAtualizar_Click" />
        </div>
        <br />
        <%--<asp:GridView ID="GVColetas" runat="server" CssClass="table table-bordered table-striped" AutoGenerateColumns="false" OnRowCommand="GVColetas_RowCommand">--%>
        <asp:GridView ID="GVColetas" runat="server" CssClass="table table-bordered table-striped table-hover" Width="100%" AutoGenerateColumns="False" OnRowCommand="GVColetas_RowCommand">
            <Columns>
                <asp:BoundField DataField="id" HeaderText="ID" Visible="false" />
                <asp:BoundField DataField="carga" HeaderText="COLETA" />
                <asp:BoundField DataField="data_hora" HeaderText="DATA/HORA" />
                <asp:BoundField DataField="" HeaderText="ATENDIMENTO" />
                <asp:BoundField DataField="cliorigem" HeaderText="LOCAL DA COLETA" />
                <asp:BoundField DataField="clidestino" HeaderText="DESTINO" />
                <asp:BoundField DataField="veiculo" HeaderText="VEICULO" />
                <asp:BoundField DataField="tipo_viagem" HeaderText="VIAGEM" />
                <asp:BoundField DataField="rota" HeaderText="ROTA" />
                <asp:BoundField DataField="andamento" HeaderText="SITUAÇÃO" />

                <asp:TemplateField HeaderText="AÇÃO" ShowHeader="True">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnDetalhes" runat="server" Text="" CommandName="MostrarDetalhes" CommandArgument='<%# Eval("Id") %>' class="btn btn-danger"><i class="fas fa-list"></i></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </form>
</body>
</html>
