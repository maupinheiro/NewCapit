<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="motoristasinativos.aspx.cs" Inherits="NewCapit.dist.pages.motoristasinativos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<style>

body{
    font-family:Segoe UI;
}

.card-sap{

    border-radius:4px;
    border:1px solid #cfd8dc;
    box-shadow:0 1px 4px rgba(0,0,0,.15);

}

.card-header-sap{

    background:#0a6ed1;
    color:white;
    font-size:18px;
    font-weight:600;

}

.table-sap{

    font-size:13px;

}

.table-sap thead th{

    background:#0a6ed1;
    color:white;
    text-align:center;
    vertical-align:middle;

}

.table-sap tbody tr:hover{

    background:#E8F4FD;

}

.table-sap td{

    vertical-align:middle;

}

.text-center{

    text-align:center;

}

.btn-sap{

    background:#0a6ed1;
    color:white;
    border:none;

}

.btn-sap:hover{

    background:#085caf;
    color:white;

}

.badge-dias{

    background:#dc3545;
    color:white;
    padding:4px 10px;
    border-radius:20px;

}

.badge-normal{

    background:#198754;
    color:white;
    padding:4px 10px;
    border-radius:20px;

}

.grid{

    max-height:600px;
    overflow:auto;

}

</style>

<script>

function SelecionarTodos(chk){

    var grid=document.getElementById('<%=gvMotoristas.ClientID%>');

    var checks=grid.querySelectorAll("input[type='checkbox']");

    for(var i=1;i<checks.length;i++){

        checks[i].checked=chk.checked;

    }

}

function Confirmar(){

    return confirm("Deseja realmente inativar os motoristas selecionados?");

}

</script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <div class="content-wrapper">
     <section class="content">
        <div class="container-fluid mt-3">
    <br />
    <div id="divMsg" runat="server"
        class="alert alert-warning alert-dismissible fade show mt-3"
        role="alert" style="display: none;">
        <span id="lblMsgGeral" runat="server"></span>
        <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
    </div>
    <br />
    <div class="card card-sap">
         <div class="card-header card-header-sap">
            <i class="fa fa-user-times"></i>
            Inativação de Motoristas
         </div>
         <div class="card-body">
            <div class="row">
              <div class="col-md-2">
                <label>Quantidade de Dias</label>
                <asp:TextBox
                    ID="txtDias"
                    runat="server"
                    CssClass="form-control"
                    TextMode="Number">
                </asp:TextBox>
              </div>
              <div class="col-md-4">
                <label>Filial</label>
                <asp:DropDownList
                    ID="ddlFilial"
                    runat="server"
                    CssClass="form-control">
                </asp:DropDownList>
              </div>
              <div class="col-md-2">
                <label>&nbsp;</label>
                <asp:Button
                    ID="btnPesquisar"
                    runat="server"
                    Text="Pesquisar"
                    CssClass="btn btn-primary btn-block"
                    OnClick="btnPesquisar_Click"/>
              </div>
            </div>
            <hr />
            <div class="grid">
                <asp:GridView
                    ID="gvMotoristas"
                    runat="server"
                    AutoGenerateColumns="False"
                    CssClass="table table-bordered table-hover table-sap"
                    DataKeyNames="codmot,Dias"
                    Width="100%">
                <Columns>
                   <asp:TemplateField HeaderStyle-Width="40">
                       <HeaderTemplate>
                            <asp:CheckBox
                            ID="chkTodos"
                            runat="server"
                            onclick="SelecionarTodos(this)" />
                       </HeaderTemplate>
                       <ItemTemplate>
                           <asp:CheckBox
                            ID="chkSelecionar"
                            runat="server"/>
                       </ItemTemplate>
                   </asp:TemplateField>
                   <asp:BoundField
                        DataField="codmot"
                        HeaderText="Código"
                        ItemStyle-HorizontalAlign="Center"
                        HeaderStyle-HorizontalAlign="Center"/>
                   <asp:BoundField
                        DataField="nommot"
                        HeaderText="Motorista"/>
                   <asp:BoundField
                        DataField="tipomot"
                        HeaderText="Tipo de Motorista"/>
                   <asp:BoundField
                        DataField="nucleo"
                        HeaderText="Filial"/>
                   <asp:TemplateField HeaderText="Última Viagem">
                        <ItemStyle HorizontalAlign="Center" />
                        <ItemTemplate>
                            <%# Eval("UltimaViagem") == DBNull.Value
                                ? "Nunca"
                                : Convert.ToDateTime(Eval("UltimaViagem")).ToString("dd/MM/yyyy") %>
                        </ItemTemplate>
                   </asp:TemplateField>
                   <asp:TemplateField HeaderText="Há">
                        <ItemStyle HorizontalAlign="Center" />
                        <ItemTemplate>
                            <span class="badge-dias">
                                <%# Eval("Dias") %> Dias
                            </span>
                        </ItemTemplate>
                   </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    <div class="alert alert-warning">
                         Nenhum motorista encontrado.
                    </div>
                </EmptyDataTemplate>
                </asp:GridView>
            </div>
            <br />
            <div class="row">
                <div class="col-md-6">
                    <asp:Label
                    ID="lblQuantidade"
                    runat="server"
                    Font-Bold="true"
                    ForeColor="#0a6ed1">
                    </asp:Label>
                </div>
                <div class="col-md-6 text-right">
                    <asp:Button
                    ID="btnInativar"
                    runat="server"
                    Text="Inativar Selecionados"
                    CssClass="btn btn-danger"
                    OnClick="btnInativar_Click"
                    OnClientClick="return Confirmar();" />
                </div>
            </div>
         </div>
    </div>
</div>
     </section>
 </div>
</asp:Content>
