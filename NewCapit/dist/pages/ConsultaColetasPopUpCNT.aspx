<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConsultaColetasPopUpCNT.aspx.cs" Inherits="NewCapit.dist.pages.ConsultaColetasPopUp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>NewCapit - Consulta Coletas</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server" class="p-3">
        <asp:GridView ID="GVColetas" runat="server" CssClass="table table-bordered table-striped" AutoGenerateColumns="true"></asp:GridView>
        <columns>
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

            <asp:TemplateField HeaderText="AÇÕES" ShowHeader="True">
                <itemtemplate>
                    <asp:LinkButton ID="btnDetalhes" runat="server" Text="" CommandName="MostrarDetalhes" CommandArgument='<%# Eval("Id") %>' class="btn btn-info"><i class="fas fa-list"></i></asp:LinkButton>
                    <asp:LinkButton ID="lnkEditar" runat="server" CssClass="btn btn-primary"><i class="fas fa-edit"></i></asp:LinkButton>                   
                </itemtemplate>
            </asp:TemplateField>
        </columns>
    </form>
</body>
</html>
