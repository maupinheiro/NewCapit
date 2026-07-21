<%@ Control Language="C#" CodeBehind="ucCadastro.ascx.cs" Inherits="NewCapit.DynamicData.FieldTemplates.ucCadastro" %>

<asp:Literal runat="server" ID="Literal1" Text="<%# FieldValueString %>" />

<div class="erp-container">


    <!-- CABEÇALHO -->

    <div class="erp-card">


        <div class="erp-card-header d-flex justify-content-between align-items-center">


            <div>

                <i class="fas fa-folder-open"></i>

                <asp:Label 
                    ID="lblTitulo"
                    runat="server"
                    Text="Cadastro">
                </asp:Label>

            </div>



            <div>


                <asp:Button
                    ID="btnNovo"
                    runat="server"
                    Text="Novo"
                    CssClass="btn btn-light erp-btn"
                    OnClick="btnNovo_Click" />


                <asp:Button
                    ID="btnSalvar"
                    runat="server"
                    Text="Salvar"
                    CssClass="btn btn-success erp-btn"
                    OnClick="btnSalvar_Click" />


                <asp:Button
                    ID="btnCancelar"
                    runat="server"
                    Text="Cancelar"
                    CssClass="btn btn-danger erp-btn"
                    OnClick="btnCancelar_Click" />


            </div>


        </div>



        <div class="erp-card-body">


            <div class="row">


                <!-- MENU LATERAL -->


                <div class="col-md-2">


                    <div class="erp-menu">


                        <asp:LinkButton
                            ID="lnkEmpresa"
                            runat="server"
                            CssClass="erp-menu-item active"
                            OnClick="Menu_Click">

                            <i class="fas fa-building"></i>
                            Empresa

                        </asp:LinkButton>



                        <asp:LinkButton
                            ID="lnkEndereco"
                            runat="server"
                            CssClass="erp-menu-item"
                            OnClick="Menu_Click">

                            <i class="fas fa-map-marker-alt"></i>
                            Endereço

                        </asp:LinkButton>



                        <asp:LinkButton
                            ID="lnkFiscal"
                            runat="server"
                            CssClass="erp-menu-item"
                            OnClick="Menu_Click">


                            <i class="fas fa-file-invoice"></i>
                            Fiscal

                        </asp:LinkButton>




                        <asp:LinkButton
                            ID="lnkContato"
                            runat="server"
                            CssClass="erp-menu-item"
                            OnClick="Menu_Click">


                            <i class="fas fa-phone"></i>
                            Contatos


                        </asp:LinkButton>



                    </div>



                </div>




                <!-- CONTEUDO -->


                <div class="col-md-10">



                    <asp:Panel
                        ID="pnlConteudo"
                        runat="server">


                        <asp:PlaceHolder
                            ID="phConteudo"
                            runat="server">
                        </asp:PlaceHolder>


                    </asp:Panel>



                </div>


            </div>


        </div>


    </div>






    <!-- PESQUISA + GRID -->


    <div class="erp-card mt-3">


        <div class="erp-card-header">


            <i class="fas fa-search"></i>

            Pesquisa


        </div>



        <div class="erp-card-body">


            <div class="row mb-3">


                <div class="col-md-6">


                    <asp:TextBox
                        ID="txtPesquisa"
                        runat="server"
                        CssClass="form-control"
                        placeholder="Digite para pesquisar..." />



                </div>


            </div>




            <asp:PlaceHolder
                ID="phGrid"
                runat="server">
            </asp:PlaceHolder>



        </div>


    </div>


</div>
