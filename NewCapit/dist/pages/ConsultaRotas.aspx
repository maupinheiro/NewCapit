<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="ConsultaRotas.aspx.cs" Inherits="NewCapit.dist.pages.ConsultaRotas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link rel="stylesheet" href="/css/styleTabela.css">
    <script language="javascript">
        function ConfirmMessage() {
            var selectedvalue = confirm("Exclusão de Dados\n Tem certeza de que deseja excluir a informação permanentemente?");
            if (selectedvalue) {
                document.getElementById('<%=txtconformmessageValue.ClientID %>').value = "Yes";

        } else {
            document.getElementById('<%=txtconformmessageValue.ClientID %>').value = "No";

            }

    </script>
    <style>
        .pagination-centered {
            text-align: center;
        }

            .pagination-centered table {
                margin: 0 auto; /* Isso centraliza a tabela da paginação */
            }
    </style>
    <!-- Page Heading -->
    <div class="content-wrapper">
        <div class="content-header">
            <div class="d-sm-flex align-items-center justify-content-between mb-4">
                <h1 class="h3 mb-2 text-gray-800">
                    <i class="fas fa-map-marker-alt"></i>&nbsp;Gestão de Rotas</h1>
                <a href="NovaRota.aspx" class="d-none d-sm-inline-block btn btn-sm btn-primary shadow-sm"><i
                    class="fas fa-directions"></i>&nbsp;Novo Cadastro            
                </a>

            </div>            
        </div>
        <!-- DataTales Example -->

         <div class="card shadow mb-4">
              <div class="card-body">
               <%--<input type="text" id="myInput" onkeyup="myFunction()" placeholder="Search for names..">--%> 
               <div class="card-header">
                 <asp:TextBox id="myInput" CssClass="form-control myInput" OnTextChanged="myInput_TextChanged" placeholder="Pesquisar ..." AutoPostBack="true" runat="server"></asp:TextBox>
               </div>
        <div class="card shadow mb-4">              
              <div class="card-body">
                 <div class="table-responsive">
                     <asp:GridView ID="gvListRotas" CssClass="table table-bordered dataTable1 table-hover" Width="100%" AutoGenerateColumns="False" DataKeyNames="rota" runat="server" AllowPaging="True" PageSize="75" OnPageIndexChanging="gvListRotas_PageIndexChanging" ShowHeaderWhenEmpty="True">
                        <PagerStyle HorizontalAlign="Center" CssClass="pagination-centered" />
                     <Columns>
                        <asp:BoundField DataField="rota" HeaderText="#ID"/>
                        <asp:BoundField DataField="desc_rota" HeaderText="ROTA"/>                         
                        <asp:BoundField DataField="deslocamento" HeaderText="DESLOCAMENTO" />
                        <asp:BoundField DataField="distancia" HeaderText="DISTÂNCIA" /> 
                        <asp:BoundField DataField="tempo" HeaderText="DURAÇÃO" />
                        <asp:BoundField DataField="situacao" HeaderText="STATUS" /> 
                        <asp:TemplateField HeaderText="AÇÕES" ShowHeader="True">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkEditar" runat="server" OnClick="Editar"  CssClass="btn btn-primary btn-sm"><i class="fa fa-edit"></i>Editar</asp:LinkButton>
                                <%--<asp:LinkButton ID="lnkExcluir" runat="server" OnClick="Excluir" CssClass="btn btn-danger btn-sm" OnClientClick="javascript:ConfirmMessage();"><i class="fa fa-trash"></i></i></asp:LinkButton>--%>
                            </ItemTemplate>
                        </asp:TemplateField>
                     </Columns>
                    </asp:GridView>

                 </div>
                 <!--<asp:HiddenField ID="txtconformmessageValue" runat="server" />-->
              </div>
         </div>
    </div>             
         </div>
        </div>
</asp:Content>
