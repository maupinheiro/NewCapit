<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="Frm_DistanciaEntreCidades.aspx.cs" Inherits="NewCapit.dist.pages.Frm_DistanciaEntreCidades" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- Bootstrap 5 -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>

    <script src="https://code.jquery.com/jquery-3.7.1.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery.mask/1.14.16/jquery.mask.min.js"></script>

    <!-- Chart.js -->
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>

    <script src="https://cdn.datatables.net/1.13.6/js/jquery.dataTables.min.js"></script>
    <link href="https://cdn.datatables.net/1.13.6/css/jquery.dataTables.min.css" rel="stylesheet" />
       
  


    <script>
        function abrirModalProcesso() {
            var modalEl = document.getElementById('mdlProcesso');
            var modal = new bootstrap.Modal(modalEl);
            modal.show();
        }
        function mascaraDataHora(campo) {
            let v = campo.value.replace(/\D/g, '');

            if (v.length > 12)
                v = v.substring(0, 12);

            if (v.length >= 9) {
                campo.value =
                    v.substring(0, 2) + '/' +
                    v.substring(2, 4) + '/' +
                    v.substring(4, 8) + ' ' +
                    v.substring(8, 10) +
                    (v.length >= 11 ? ':' + v.substring(10, 12) : '');
            }
            else if (v.length >= 5) {
                campo.value =
                    v.substring(0, 2) + '/' +
                    v.substring(2, 4) + '/' +
                    v.substring(4, 8);
            }
            else if (v.length >= 3) {
                campo.value =
                    v.substring(0, 2) + '/' +
                    v.substring(2, 4);
            }
            else {
                campo.value = v;
            }
        }


    </script>
    <script>
        function formatarMoeda(campo) {
            let valor = campo.value.replace(/\D/g, "");
            if (valor === "") return;

            valor = (valor / 100).toFixed(2) + "";
            valor = valor.replace(".", ",");
            valor = valor.replace(/\B(?=(\d{3})+(?!\d))/g, ".");
            campo.value = "R$ " + valor;
        }
    </script>
   
    <div class="content-wrapper">
        <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server" />
        <section class="content">
            <div class="container-fluid">               
                <br />
                <div class="col-md-12">
                    <div class="card card-info">
                        <div class="card-header">
                            <h3 class="card-title">
                                <h3 class="card-title"><i class="fas fa-route"></i>&nbsp;DISTÂNCIA ENTRE CIDADES</h3>
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

                                <div class="row mb-3">

                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtPesquisar" runat="server"
                                            CssClass="form-control"
                                            placeholder="Pesquisar ciddade, estado... " />
                                    </div>

                                    <%--<div class="col-md-3">
                                        <asp:DropDownList ID="ddlStatus" runat="server"
                                            CssClass="form-select"
                                            AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">
                                            <asp:ListItem Text="Origem" Value="Origem" />
                                            <asp:ListItem Text="Destino" Value="Destino" />
                                            <asp:ListItem Text="Todos" Value="Todos" />
                                        </asp:DropDownList>
                                    </div>--%>

                                    <div class="col-md-2">
                                        <asp:Button ID="btnPesquisar" runat="server"
                                            CssClass="btn btn-warning w-100"
                                            Text="Pesquisar"
                                            OnClick="btnPesquisar_Click" />
                                    </div>

                                    <%--<div class="col-md-2">
                                        <asp:Button ID="btnAbrirModal" runat="server"
                                            Text="Abrir Processo"
                                            CssClass="btn btn-primary w-100"
                                            OnClick="btnAbrirModal_Click" />
                                    </div>--%>
                                </div>

                                <asp:GridView ID="gvCidades" runat="server"
                                    CssClass="table table-bordered table-striped table-hover"
                                    AutoGenerateColumns="False"
                                    EmptyDataText="Nenhum registro encontrado.">

                                    <Columns>                                        

                                        <asp:TemplateField HeaderText="Cidade de Origem">
                                            <ItemTemplate>
                                                <%# Eval("origem")%>
                                               
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="UF">
                                            <ItemTemplate>
                                                <%# Eval("uf_origem")%>   
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Cidade de Destino">
                                            <ItemTemplate>
                                                <%# Eval("Destino")%>   
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="UF">
                                            <ItemTemplate>
                                                <%# Eval("uf_destino")%>   
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Distância (km)">
                                            <ItemTemplate>
                                                <%# Eval("Distancia")%>   
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- form INCLUIR e CONSULTAR processo -->
               <%-- <div class="modal fade" id="mdlProcesso" tabindex="-1">
                    <div class="modal-dialog modal-xl modal-dialog-centered"">
                        <div class="modal-content">                            
                            <div class="modal-header">
                                <h5 class="modal-title">Multa/Processo</h5>
                                <button type="button" class="btn-close"
                                    data-bs-dismiss="modal">
                                </button>
                            </div>
                            <!-- UPDATEPANEL SÓ NO CONTEÚDO -->
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">                                
                                <ContentTemplate>                                                                   
                                    <div class="modal-body">                                       
                                        <asp:HiddenField ID="hfStatus" runat="server" />
                                        <div class="col-xl-12 col-md-12 mb-12">

                                            <div id="divMsg" runat="server"
                                                class="alert alert-info alert-dismissible fade show mt-3"
                                                role="alert" style="display: none;">
                                                <span id="lblMsg" runat="server"></span>
                                                <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
                                            </div>
                                            <div class="row g-3">
                                                     <div class="col-md-1">                                                         
                                                         <asp:Image ID="imgFoto" runat="server"
                                                             CssClass="img-thumbnail"
                                                             Style="width:80px;height:80px;object-fit:cover;"
                                                             AlternateText="Foto"
                                                             ImageUrl="/fotos/motoristasemfoto.jpg" />
                                                     </div>
                                                <div class="col-md-2">
                                                    <label>Nº Processo:</label>
                                                    <asp:TextBox ID="txtProcessoModal" runat="server" CssClass="form-control" />
                                                </div>
                                                <div class="col-md-2">
                                                    <label>Nº AIT:</label>
                                                    <asp:TextBox ID="txtAIT" runat="server" CssClass="form-control" />
                                                </div>
                                                <div class="col-md-2">
                                                    <label>Data</label>
                                                    <asp:TextBox 
                                                            ID="txtData"
                                                            runat="server"
                                                            CssClass="form-control"
                                                            AutoPostBack="true"
                                                            OnTextChanged="txtData_TextChanged"
                                                            oninput="mascaraDataHora(this);" />
                                                </div>
                                                <div class="col-md-2">
                                                    <label>&nbsp;</label><br />                                                   
                                                    <asp:TextBox ID="txtDia" runat="server" CssClass="form-control" ReadOnly="true" Style="text-align: center" />
                                                </div>                                                
                                                <div class="col-md-2">
                                                    <label>Lançamento:</label>
                                                    <asp:TextBox ID="txtLancamento" runat="server" CssClass="form-control" Enabled="false" Style="text-align: center" />
                                                </div>
                                                <div class="col-md-1" id="colunaID" runat="server" visible="false">
                                                    <label>Id:</label>
                                                    <asp:TextBox ID="txtId" runat="server" CssClass="form-control" Enabled="false" Style="text-align: center" />
                                                </div>
                                            </div>
                                            <br />
                                            <div class="row g3">
                                                <div class="col-md-2">
                                                    <label>Código:</label>
                                                    <asp:TextBox ID="txtCodigo_Infracao" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtCodigo_Infracao_TextChanged" />
                                                </div>
                                                <div class="col-md-2">
                                                    <div class="form-group">
                                                        <label>Fundamento CTB:</label>
                                                        <asp:DropDownList ID="ddlArtigo" runat="server" CssClass="form-control"></asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="col-md-1">
                                                    <div class="form-group">
                                                        <label>Pontos:</label>
                                                        <asp:TextBox ID="txtPontos" runat="server" CssClass="form-control" Style="text-align: center" Enabled="false"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-2">
                                                    <div class="form-group">
                                                        <%--<span class="details">Gravidade:</span>--%>
                                                       <%-- <label>Gravidade:</label>
                                                        <asp:TextBox ID="txtGravidade" runat="server" CssClass="form-control" Style="text-align: center" Enabled="false"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-2">
                                                    <div class="form-group">
                                                        <label>Infrator:</label>
                                                        <asp:TextBox ID="txtInfrator" runat="server" CssClass="form-control" Style="text-align: center" Enabled="false"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-3">
                                                    <div class="form-group">
                                                        <label>Competência:</label>
                                                        <asp:TextBox ID="txtCompetencia" runat="server" CssClass="form-control" Style="text-align: center" Enabled="false"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>                                            
                                            <div class="row g-3">
                                                <div class="col-md-12">
                                                     <div class="form-group">
                                                        <label>Descrição da Multa/Motivo:</label>
                                                        <asp:TextBox ID="txtdesc_multa" runat="server" CssClass="form-control" Enabled="false" TextMode="MultiLine" Rows="2"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row g-3">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label>Local de Multa:</label>
                                                        <asp:TextBox ID="txtLocalMulta" runat="server" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row g-3">
                                                <div class="col-md-12">
                                                <div class="form-group">
                                                    <label>Providência Tomada:</label>
                                                    <asp:TextBox ID="txtProvidencia" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2"></asp:TextBox>
                                                </div>
                                            </div>
                                            </div>
                                            <div class="row-3">
                                                <div class="card-header">
                                                    <form class="form-horizontal">
                                                        <div class="card-body">                                                            
                                                            <div class="form-group row">
                                                                <label for="inputRemetente" class="col-sm-2 col-form-label" style="text-align: right"> Valor Sem Indicação:</label>
                                                                <div class="col-md-2">
                                                                    <asp:TextBox ID="txtValorsd" runat="server" CssClass="form-control"  onblur="formatarMoeda(this)" ></asp:TextBox>
                                                                </div>

                                                                <label for="inputRemetente" class="col-sm-2 col-form-label" style="text-align: right">Valor Com Indicação:</label>
                                                                <div class="col-md-2">
                                                                    <asp:TextBox ID="txtValorcd" runat="server" CssClass="form-control"  onblur="formatarMoeda(this)" ></asp:TextBox>
                                                                </div>
                                                                <label for="inputRemetente" class="col-sm-2 col-form-label" style="text-align: right">Indicar Até:</label>
                                                                <div class="col-md-2">
                                                                    <asp:TextBox ID="txtVencimento" runat="server" CssClass="form-control" ></asp:TextBox>
                                                                </div> 
                                                            </div>                                                            
                                                        </div>
                                                    </form>
                                                </div>
                                            </div>                                           
                                           
                                            <div class="row g-3">
                                                <div class="col-md-2">
                                                     <label>Código:</label>
                                                     <asp:TextBox ID="txtCodMot" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtCodMot_TextChanged" />
                                                </div>
                                                <div class="col-md-7">
                                                     <label>Nome do Motorista:</label>
                                                     <asp:TextBox ID="txtNome"
                                                         runat="server"
                                                         CssClass="form-control" Enabled="false" />
                                                </div>
                                                
                                                <div class="col-md-3">
                                                     <label>Núcleo:</label>
                                                     <asp:TextBox ID="txtNucleo"
                                                         runat="server"
                                                         CssClass="form-control" Enabled="false" />
                                                </div>
                                            </div>
                                            
                                            <div class="row g-3">
                                                <div class="col-md-2">
                                                    <label>Frota:</label>
                                                    <asp:TextBox ID="txtFrota" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtFrota_TextChanged" />
                                                </div>
                                                <div class="col-md-2">
                                                    <label>Placa:</label>
                                                    <asp:TextBox ID="txtPlaca" runat="server" CssClass="form-control" Enabled="false"/>
                                                </div>
                                                <div class="col-md-2">
                                                    <label>Equip:</label>
                                                    <asp:TextBox ID="txtEquipamento" runat="server" CssClass="form-control" Enabled="false" />
                                                </div>
                                                <div class="col-md-6">
                                                     <label>Transportadora:</label>
                                                     <asp:TextBox ID="txtTransportadora"
                                                         runat="server"
                                                         CssClass="form-control" Enabled="false" />
                                                </div>
                                            </div>                                            
                                            <br />
                                            <div class="row g-3">
                                                <div class="card-header">
                                                    <form class="form-horizontal">
                                                        <div class="card-body">                                                            
                                                            <div class="form-group row">
                                                                <label for="inputRemetente" class="col-sm-4 col-form-label" style="text-align: right"> Entregue ao Dep. de Transporte Em:</label>
                                                                <div class="col-md-2">
                                                                    <asp:TextBox ID="txtEnvio_transp" runat="server" CssClass="form-control" ></asp:TextBox>
                                                                </div>

                                                                <label for="inputRemetente" class="col-sm-2 col-form-label" style="text-align: right">Recebido Por:</label>
                                                                <div class="col-md-4">
                                                                    <asp:TextBox ID="txtRecebido_Por" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                                </div>
                                                            </div>                                                            
                                                            <div class="form-group row">
                                                                <label for="inputRemetente" class="col-sm-4 col-form-label" style="text-align: right"> Entregue ao Controle Em:</label>
                                                                <div class="col-md-2">
                                                                    <asp:TextBox ID="txtEnvio_dcp" runat="server" CssClass="form-control" ></asp:TextBox>
                                                                </div>

                                                                <label for="inputRemetente" class="col-sm-2 col-form-label" style="text-align: right">Baixado Por:</label>
                                                                <div class="col-md-4">
                                                                    <asp:TextBox ID="txtBaixado_por" runat="server" CssClass="form-control" Enabled="false" ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                           
                                                        </div>
                                                    </form>
                                                </div>
                                            </div>
                                    </div>                                    
                                    
                                    <div class="modal-footer">

                                        <div class="container-fluid">
                                            <div class="row align-items-center">

                                                <!-- ESQUERDA -->
                                                <div class="col-auto">
                                                    <asp:TextBox ID="txtStatus"
                                                        runat="server"
                                                        CssClass="form-control form-control-sm" Style="text-align: center"
                                                         />                                                       
                                                </div>

                                                <!-- ESPAÇO FLEXÍVEL -->
                                                <div class="col"></div>

                                                <!-- DIREITA -->
                                                <div class="col-auto text-end">
                                                    <asp:Button ID="btnPesquisarModal"
                                                        runat="server"
                                                        Text="Pesquisar"
                                                        CssClass="btn btn-info btn-sm ms-2"
                                                        OnClick="btnPesquisarModal_Click"
                                                        UseSubmitBehavior="false" />

                                                    <asp:Button ID="btnSalvar"
                                                        runat="server"
                                                        Text="Salvar"
                                                        CssClass="btn btn-success btn-sm ms-2"
                                                        OnClick="btnSalvar_Click"
                                                        UseSubmitBehavior="false" />

                                                    <asp:Button ID="btnFechar"
                                                        runat="server"
                                                        Text="Fechar"
                                                        CssClass="btn btn-secondary btn-sm ms-2"
                                                        data-bs-dismiss="modal" />
                                                </div>

                                            </div>
                                        </div>

                                    </div>

                                </ContentTemplate>
                            </asp:UpdatePanel>

                        </div>
                    </div>
                </div>--%>
            </div>
        </section>
    </div>
</asp:Content>

