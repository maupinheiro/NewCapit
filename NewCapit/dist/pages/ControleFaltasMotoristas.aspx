<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="ControleFaltasMotoristas.aspx.cs"
    Inherits="NewCapit.dist.pages.ControleFaltasMotoristas"
    MasterPageFile="~/dist/pages/Main.Master" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .erp-card{
            background:#fff;
            border-radius:8px;
            box-shadow:0 2px 8px rgba(0,0,0,.15);
            margin-bottom:20px;
        }
        .erp-card-header{
            background:#0A6ED1;
            color:#fff;
            padding:12px;
            font-size:18px;
            font-weight:600;
            border-radius:8px 8px 0 0;
        }
        .erp-card-body{
            padding:20px;
        }
        .table-sap{
            font-size:13px;
        }
        .table-sap thead th{
            background:#0A6ED1;
            color:#fff;
            text-align:center;
            vertical-align:middle;
            position:sticky;
            top:0;
            z-index:10;
        }
        .table-sap tbody td{
            vertical-align:middle;
        }
        .table-sap tbody tr:hover{
            background:#EAF3FC;
        }
        .sap-total{
            background:#F5F6F7;
            border-left:5px solid #0A6ED1;
            padding:10px;
            margin-bottom:10px;
            font-weight:bold;
        }
        .btn-sap{
            background:#0A6ED1;
            color:white;
        }
        .btn-sap:hover{
            background:#0854A0;
            color:white;
        }
        .form-control{
            border-radius:4px;
        }
        .select2-container{
            width:100%!important;
        }
        .badge-falta{
            background:#dc3545;
        }
        .badge-atestado{
            background:#ffc107;
            color:#000;
        }
        .badge-ferias{
            background:#28a745;
        }
        .badge-DSR{
            background:#17a2b8;
        }
        .pagination-ys table{
        margin:auto;
        }
        .pagination-ys td{

            padding:6px;

        }
        .pagination-ys a{

            padding:8px 12px;

            background:#0A6ED1;

            color:white;

            border-radius:4px;

            text-decoration:none;

        }
        .pagination-ys span{

    padding:8px 12px;

    background:#0854A0;

    color:white;

    border-radius:4px;

}
        .grid-container-sap{
            max-height:500px;
            overflow-y:auto;
        }
        .grid-container-sap table thead th{
            position:sticky;
            top:0;
            z-index:5;
        }
    </style>
    
    <script>
        function InicializarSelect2() {

            $('.select2').select2({

                width: '100%',
                allowClear: true

            });

            $('.select2').select2({
                width: '100%',                
                allowClear: true
            });

        }


        $(document).ready(function () {

            InicializarSelect2();

        });


        Sys.WebForms.PageRequestManager
            .getInstance()
            .add_endRequest(function () {

                InicializarSelect2();

            });
    </script>
   
    
    <div class="content-wrapper">
        <section class="content">
            <div class="container-fluid">
               <br />
              <%-- <div id="divMsg" runat="server"
                    class="alert alert-dismissible fade show mt-3"
                    role="alert" visible="false">
                    <asp:Label ID="lblMsgGeral" runat="server"></asp:Label>
                    <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
                </div>--%>
               <asp:UpdatePanel ID="upPrincipal" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="container-fluid">
                       <div class="erp-card">
                            <div class="erp-card-header">
                                <i class="fas fa-user-clock"></i>
                                Controle de Faltas dos Motoristas
                            </div>
                            <div class="erp-card-body">
                                <div class="row">
                                    <!-- Código -->
                                    <div class="col-md-2">
                                        <label>Código</label>
                                        <asp:TextBox
                                            ID="txtCodMot"
                                            runat="server"
                                            CssClass="form-control"
                                            AutoPostBack="true"
                                            OnTextChanged="txtCodMot_TextChanged">
                                        </asp:TextBox>
                                    </div>

                                    <!-- Motorista -->
                                    <div class="col-md-5">

                                        <label>Motorista</label>

                                        <asp:DropDownList
                                            ID="ddlMotorista"
                                            runat="server"
                                            CssClass="form-control select2"
                                            AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlMotorista_SelectedIndexChanged">
                                        </asp:DropDownList>

                                    </div>

                                    <!-- Função -->
                                    <div class="col-md-2">

                                        <label>Função</label>

                                        <asp:TextBox
                                            ID="txtFuncao"
                                            runat="server"
                                            CssClass="form-control"
                                            ReadOnly="true">
                                        </asp:TextBox>

                                    </div>

                                    <!-- Núcleo -->
                                    <div class="col-md-3">

                                        <label>Núcleo</label>

                                        <asp:TextBox
                                            ID="txtNucleo"
                                            runat="server"
                                            CssClass="form-control"
                                            ReadOnly="true">
                                        </asp:TextBox>

                                    </div>

                                </div>
                                <hr />
                                <div class="row">

                                    <!-- Data -->
                                    <div class="col-md-2">

                                        <label>Data Inicial</label>

                                        <asp:TextBox
                                            ID="txtData"
                                            runat="server"
                                            CssClass="form-control"
                                            TextMode="Date">
                                        </asp:TextBox>

                                    </div>

                                    <!-- Quantidade -->
                                    <div class="col-md-2">

                                        <label>Quantidade</label>

                                        <asp:TextBox
                                            ID="txtQuantidade"
                                            runat="server"
                                            CssClass="form-control"
                                            Text="1">
                                        </asp:TextBox>

                                    </div>

                                    <!-- Motivo -->
                                    <div class="col-md-2">

                                        <label>Motivo</label>
                                        <asp:DropDownList
                                            ID="ddlMotivo"
                                            runat="server"
                                            CssClass="form-control">
                                            <asp:ListItem Value="Falta">Falta</asp:ListItem>
                                            <asp:ListItem Value="Atestado">Atestado</asp:ListItem>
                                            <asp:ListItem Value="DSR">DSR</asp:ListItem>
                                            <asp:ListItem Value="Férias">Férias</asp:ListItem>
                                        </asp:DropDownList>

                                    </div>

                                    <!-- Observação -->
                                    <div class="col-md-4">

                                        <label>Observação</label>

                                        <asp:TextBox
                                            ID="txtObservacao"
                                            runat="server"
                                            CssClass="form-control"
                                            MaxLength="300">
                                        </asp:TextBox>

                                    </div>

                                    <!-- Botão -->
                                    <div class="col-md-2">

                                        <label>&nbsp;</label>

                                        <asp:Button
                                            ID="btnSalvar"
                                            runat="server"
                                            Text="Salvar"
                                            CssClass="btn btn-sap btn-block"
                                            OnClick="btnSalvar_Click" />

                                    </div>

                                </div>
                                <br />
                                <div class="row">

                                    <div class="col-md-12">

                                        <asp:Label
                                            ID="lblMensagem"
                                            runat="server"
                                            CssClass="alert alert-info"
                                            Visible="false">
                                        </asp:Label>

                                    </div>

                                </div>
                            </div>
                       </div>
                       <!-- =================== CONSULTA =================== -->
                       <div class="erp-card">
                           <div class="erp-card-header">
                                <i class="fas fa-search"></i>
                                Consulta de Ocorrências
                            </div>
                           <div class="erp-card-body">
        <div class="row">
            <div class="col-md-2">
                <label>Data Inicial</label>
                <asp:TextBox
                    ID="txtDataIni"
                    runat="server"
                    CssClass="form-control"
                    TextMode="Date" />
            </div>
            <div class="col-md-2">
                <label>Data Final</label>
                <asp:TextBox
                    ID="txtDataFim"
                    runat="server"
                    CssClass="form-control"
                    TextMode="Date" />
            </div>
            <div class="col-md-4">
                <label>Motorista</label>
                <asp:DropDownList
                    ID="ddlFiltroMotorista"
                    runat="server"
                    CssClass="form-control select2">
                </asp:DropDownList>
            </div>
            <div class="col-md-2">
                <label>Núcleo</label>

                <asp:DropDownList ID="ddlFiltroNucleo"
                    runat="server"
                    CssClass="form-select select2"
                    AutoPostBack="true"
                    OnSelectedIndexChanged="ddlFiltroNucleo_SelectedIndexChanged">
                </asp:DropDownList>
            </div>
            <div class="col-md-2">
                <label>&nbsp;</label>
                <asp:Button
                    ID="btnPesquisar"
                    runat="server"
                    Text="Pesquisar"
                    CssClass="btn btn-sap btn-block"
                    OnClick="btnPesquisar_Click" />
            </div>
        </div>
        <hr />
                    </div>  
                           <div class="container-fluid">
                           <div class="row">
                                <div class="col-md-4">
                                    <div class="sap-total">
                                        Total de Ocorrências
                                        <br />
                                        <asp:Label
                                            ID="lblTotalOcorrencias"
                                            runat="server"
                                            Font-Size="XX-Large"
                                            Font-Bold="true"
                                            Text="0" />
                                    </div>

                                </div>
                                <div class="col-md-2">
                                    <div class="sap-total">
                                        Faltas                                                                                    <br />
                                        <asp:Label
                                            ID="lblFaltas"
                                            runat="server"
                                            CssClass="badge badge-falta"
                                            Font-Size="Large"
                                            Text="0" />
                                    </div>
                                </div>
                                <div class="col-md-2">
                                    <div class="sap-total">
                                        Atestados
                                        <br />
                                        <asp:Label
                                            ID="lblAtestados"
                                            runat="server"
                                            CssClass="badge badge-atestado"
                                            Font-Size="Large"
                                            Text="0" />
                                    </div>

                                </div>
                                <div class="col-md-2">
                                    <div class="sap-total">
                                        Férias
                                        <br />
                                        <asp:Label
                                            ID="lblFerias"
                                            runat="server"
                                            CssClass="badge badge-ferias"
                                            Font-Size="Large"
                                            Text="0" />
                                    </div>
                                </div>
                                <div class="col-md-2">
                                 <div class="sap-total">
                                    DSR
                                    <br />
                                    <asp:Label
                                        ID="lblDSR"
                                        runat="server"
                                        CssClass="badge badge-DSR"
                                        Font-Size="Large"
                                        Text="0" />

                                </div>
                                </div>
                           </div>
                           </div>
                           <br />
                           <div class="container-fluid">
                           <div class="row">
                           <div class="table-responsive grid-container-sap">
                                <asp:GridView ID="gvFaltas" runat="server"
                                    AutoGenerateColumns="False"
                                    AllowPaging="True"
                                    PageSize="30"
                                    CssClass="table table-bordered table-hover table-sap"
                                    GridLines="None"
                                    DataKeyNames="id"
                                    OnPageIndexChanging="gvFaltas_PageIndexChanging"
                                    OnRowCommand="gvFaltas_RowCommand">
                                    <Columns>
                                        <asp:BoundField
                                            HeaderText="Data"
                                            DataField="data_falta"
                                            DataFormatString="{0:dd/MM/yyyy}" />

                                        <asp:BoundField
                                            HeaderText="Código"
                                            DataField="codmot" />

                                        <asp:BoundField
                                            HeaderText="Motorista"
                                            DataField="nommot" />

                                        <asp:BoundField
                                            HeaderText="Função"
                                            DataField="funcao" />

                                        <asp:BoundField
                                            HeaderText="Núcleo"
                                            DataField="nucleo" />

                                        <asp:BoundField
                                            HeaderText="Motivo"
                                            DataField="motivo" />

                                        <asp:BoundField
                                            HeaderText="Observação"
                                            DataField="observacao" />

                                        <asp:BoundField
                                            HeaderText="Usuário"
                                            DataField="usuario" />
                                        <asp:TemplateField
                                            HeaderText="Ações"
                                            ItemStyle-HorizontalAlign="Center"
                                            >

                                            <ItemTemplate>

                                            <asp:LinkButton
                                                ID="btnExcluir"
                                                runat="server"
                                                CssClass="btn btn-danger btn-sm"
                                                CommandName="Excluir"
                                                CommandArgument='<%# Eval("id") %>'
                                                OnClientClick="return confirm('Deseja excluir este registro?');"
                                                ToolTip="Excluir">
                                                <i class="fas fa-trash"></i>
                                            </asp:LinkButton>

                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerStyle
                                        HorizontalAlign="Center"
                                        CssClass="pagination-ys" />
                                </asp:GridView>
                               
                           </div>
                           </div>
                           </div>
                       </div>
                    </div>
                </ContentTemplate>
               </asp:UpdatePanel>
            </div>
        </section>
    </div>


</asp:Content>
