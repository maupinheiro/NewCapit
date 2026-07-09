<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="tabelasauxiliares.aspx.cs" Inherits="NewCapit.dist.pages.tabelasauxiliares" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
  <meta charset="utf-8">
  <meta http-equiv="X-UA-Compatible" content="IE=edge">  
  <!-- Tell the browser to be responsive to screen width -->
  <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport">
  <!-- Google Font -->
  <link rel="stylesheet"
        href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,600,700,300italic,400italic,600italic">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  
    <div class="content-wrapper">
    <section class="content">
        <div class="container-fluid">
    
    




    <div class="card shadow-sm">
        <br />
        <div class="card-header bg-primary text-white d-flex justify-content-between align-items-center">
            <h5 class="mb-0">Tipos de Viagem</h5>
            <asp:Button ID="btnNovo"
                runat="server"
                Text="+ Novo Tipo de Viagem"
                CssClass="btn btn-success"
                OnClick="btnNovo_Click" />
        </div>

        <div class="card-body">

            <asp:GridView ID="gvTiposViagem"
                runat="server"
                AutoGenerateColumns="False"
                CssClass="table table-bordered table-hover table-striped"
                DataKeyNames="codigo"
                OnRowCommand="gvTiposViagem_RowCommand">

                <Columns>

                    <asp:BoundField DataField="descricao"
                        HeaderText="Descrição" />

                    <asp:BoundField DataField="abreviacao"
                        HeaderText="Abreviação" />

                    <asp:BoundField DataField="interplantas"
                        HeaderText="Interplantas" />

                    <asp:BoundField DataField="cnti"
                        HeaderText="CNTI" />

                    <asp:BoundField DataField="codigo_recebedor_interplantas"
                        HeaderText="Cód. Recebedor Inter." />

                    <asp:BoundField DataField="nome_recebedor_interplantas"
                        HeaderText="Recebedor Inter." />

                    <asp:BoundField DataField="codigo_recebedor_cnti"
                        HeaderText="Cód. Recebedor CNTI" />

                    <asp:BoundField DataField="nome_recebedor_cnti"
                        HeaderText="Recebedor CNTI" />

                    <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>

                            <asp:Label ID="lblStatus"
                                runat="server"
                                Text='<%# Eval("status") %>'
                                CssClass='<%# Eval("status").ToString().Trim()=="ATIVO" ? "badge bg-success" : "badge bg-danger" %>' />

                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Ações" ItemStyle-Width="180">
                        <ItemTemplate>

                            <asp:LinkButton ID="btnEditar"
                                runat="server"
                                CssClass="btn btn-sm btn-primary"
                                CommandName="Editar"
                                CommandArgument='<%# Eval("codigo") %>'>
                            <i class="fa fa-edit"></i> Editar
                            </asp:LinkButton>

                            <asp:LinkButton ID="btnInativar"
                                runat="server"
                                CssClass="btn btn-sm btn-danger"
                                CommandName="Inativar"
                                CommandArgument='<%# Eval("codigo") %>'
                                OnClientClick="return confirm('Deseja realmente inativar este registro?');">
                            <i class="fa fa-times"></i> Inativar
                            </asp:LinkButton>

                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>

            </asp:GridView>

        </div>
    </div>
          </div>
     </section>
   </div>

</asp:Content>
