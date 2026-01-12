<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="GestaoDeMultas.aspx.cs" Inherits="NewCapit.dist.pages.GestaoDeMultas" %>

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
        function mascaraData(campo) {
            let v = campo.value.replace(/\D/g, '');

            if (v.length > 12)
                v = v.substring(0, 12);

            if (v.length >= 9) {
                campo.value =
                    v.substring(0, 2) + '/' +
                    v.substring(2, 4) + '/' +
                    v.substring(4, 8) + ' ' +
                    v.substring(8, 10);
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
   <%-- <script>
    $(function () {
        $('#<%= chkBaixado.ClientID %>').change(function () {
            if (this.checked) {
                $('#divDataBaixa').slideDown();
                $('#divBaixadoPor').slideDown();
            } else {
                $('#divDataBaixa').slideUp();
                 $('#divBaixadoPor').slideUp();
            }
        });
    });
    </script>--%>

    <style>
    .modal-xxl {
        max-width: 80%;
    }
    </style>
    <script>
    function ReabrirModal() {
        var modal = new bootstrap.Modal(
            document.getElementById('mdlProcesso')
        );
        modal.show();
    }
    </script>

    <div class="content-wrapper">
        <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server" />
        <section class="content">
            <div class="container-fluid">               
                
                <div class="col-md-12">
                    <div class="card card-info">
                        <div class="card-header">
                            <h3 class="card-title">
                                <h3 class="card-title"><i class="fas fa-shipping-fast"></i>&nbsp;GESTÃO DE MULTAS</h3>
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
                                            placeholder="Pesquisar placa, por codigo, nome do motorista ou processo ... " />
                                    </div>

                                    <div class="col-md-3">
                                        <asp:DropDownList ID="ddlStatus" runat="server"
                                            CssClass="form-select"
                                            AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">
                                            <asp:ListItem Text="Pendentes" Value="Pendente" />
                                            <asp:ListItem Text="Baixados" Value="Baixado" />
                                            <asp:ListItem Text="Todos" Value="Todos" />
                                        </asp:DropDownList>
                                    </div>

                                    <div class="col-md-2">
                                        <asp:Button ID="btnPesquisar" runat="server"
                                            CssClass="btn btn-warning w-100"
                                            Text="Pesquisar"
                                            OnClick="btnPesquisar_Click" />
                                    </div>

                                    <div class="col-md-2">
                                        <asp:Button ID="btnAbrirModal" runat="server"
                                            Text="Abrir Processo"
                                            CssClass="btn btn-primary w-100"
                                            OnClick="btnAbrirModal_Click" />
                                    </div>
                                </div>

                                <asp:GridView ID="gvProcessos" runat="server"
                                    CssClass="table table-bordered table-striped table-hover"
                                    AutoGenerateColumns="False"
                                    EmptyDataText="Nenhum registro encontrado.">

                                    <Columns>

                                        <asp:TemplateField HeaderText="Foto">
                                            <ItemTemplate>
                                                <asp:Image ID="imgFoto" runat="server"
                                                    Width="55"
                                                    CssClass="img-thumbnail"
                                                    ImageUrl='<%# Eval("foto") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Motorista/Transportadora">
                                            <ItemTemplate>
                                                <%# Eval("codmot") + " - " + Eval("nome") %>
                                                <br />
                                                <%# Eval("transportadora")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Processo/AIT">
                                            <ItemTemplate>
                                                <%# Eval("processo")%>
                                                <br />
                                                <%# Eval("ait")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Data Autuação">
                                            <ItemTemplate>
                                                <%# String.Format("{0:dd/MM/yyyy HH:mm}", Eval("dthoranot")) %>
                                                <br />
                                                <%# Eval("dia")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Placa">
                                            <ItemTemplate>
                                                <%# Eval("placa")%>
                                                <br />
                                                <%# Eval("equipamento")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderText="Status">
                                            <ItemTemplate>
                                                <span class='<%# Eval("status").ToString() == "Pendente" ? "badge bg-warning text-dark" : "badge bg-success" %>'>
                                                    <%# Eval("status") %>
                                                </span>
                                                <br />
                                                <%# Eval("baixado_por") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- form INCLUIR e CONSULTAR processo -->
                <div class="modal fade" id="mdlProcesso" tabindex="-1">
                    <div class="modal-dialog modal-xxl modal-dialog-centered"">
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
                                                    <asp:TextBox ID="txtProcessoModal" runat="server" CssClass="form-control" OnTextChanged="btnPesquisarModal_Click" AutoPostBack="true" />
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
                                                        <label>Gravidade:</label>
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
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label>Local de Multa:</label>
                                                        <asp:TextBox ID="txtLocalMulta" runat="server" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-2">
                                                    <div class="form-group">
                                                        <label>Indicar Até:</label>
                                                       <asp:TextBox ID="txtVencimento" runat="server" CssClass="form-control" oninput="mascaraData(this);" ></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-2">
                                                    <div class="form-group">
                                                        <label>R$ Sem Indicação:</label>
                                                        <asp:TextBox ID="txtValorsd" runat="server" CssClass="form-control"  onblur="formatarMoeda(this)" ></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-2">
                                                    <div class="form-group">
                                                        <label>R$ Com Indicação:</label>
                                                        <asp:TextBox ID="txtValorcd" runat="server" CssClass="form-control"  onblur="formatarMoeda(this)" ></asp:TextBox>
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
                                            <div class="row g-3">
                                                <div class="col-md-2">
                                                     <label>Código:</label>
                                                     <asp:TextBox ID="txtCodMot" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtCodMot_TextChanged" />
                                                </div>
                                                <div class="col-md-7">
                                                     <label>Nome do Motorista:</label>
                                                     <asp:TextBox ID="txtNome"
                                                         runat="server"
                                                         CssClass="form-control" ReadyOnly="true" />
                                                </div>
                                                
                                                <div class="col-md-3">
                                                     <label>Núcleo:</label>
                                                     <asp:TextBox ID="txtNucleo"
                                                         runat="server"
                                                         CssClass="form-control" ReadyOnly="true" />
                                                </div>
                                            </div>
                                            
                                            <div class="row g-3">
                                                <div class="col-md-2">
                                                    <label>Frota:</label>
                                                    <asp:TextBox ID="txtFrota" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtFrota_TextChanged" />
                                                </div>
                                                <div class="col-md-2">
                                                    <label>Placa:</label>
                                                    <asp:TextBox ID="txtPlaca" runat="server" CssClass="form-control" ReadyOnly="true"/>
                                                </div>
                                                <div class="col-md-2">
                                                    <label>Equip:</label>
                                                    <asp:TextBox ID="txtEquipamento" runat="server" CssClass="form-control" ReadyOnly="true" />
                                                </div>
                                                <div class="col-md-6">
                                                     <label>Transportadora:</label>
                                                     <asp:TextBox ID="txtTransportadora"
                                                         runat="server"
                                                         CssClass="form-control" ReadyOnly="true" />
                                                </div>
                                            </div>                                            
                                            <br />
                                            <div class="row g-3">
                                                <div class="col-md-2">
                                                     <label>Recebido:</label>
                                                      <asp:TextBox ID="txtEnvio_transp" runat="server" CssClass="form-control" oninput="mascaraDataHora(this);"  ></asp:TextBox>
                                                </div>
                                                <div class="col-md-4">
                                                     <label>Por:</label>
                                                     <asp:TextBox ID="txtRecebido_Por" runat="server" CssClass="form-control" ReadyOnly="true"></asp:TextBox>
                                                </div>
                                                
                                                <div class="col-md-2" id="divDataBaixa" >
                                                     <label>Baixa:</label>
                                                     <asp:TextBox ID="txtEnvio_dcp" runat="server" CssClass="form-control" onkeyup="mascaraDataHora(this);" ></asp:TextBox>
                                                </div>
                                                <div class="col-md-4" id="divBaixadoPor" >
                                                     <label>Por:</label>
                                                     <asp:TextBox ID="txtBaixado_por" runat="server" CssClass="form-control" ReadyOnly="true" ></asp:TextBox>
                                                </div>
                                            </div>
                                    </div>                                    
                                    <br />
                                    <div class="modal-footer">

                                        <div class="container-fluid">
                                            <div class="row align-items-center">

                                                <!-- ESQUERDA -->
                                                <div class="col-auto">
                                                    <asp:TextBox ID="txtStatus" Text=""
                                                        runat="server"
                                                        CssClass="form-control form-control-sm" Style="text-align: center"
                                                         />                                                       
                                                </div>
                                                <div class="col-auto">
                                                    <asp:CheckBox
                                                        ID="chkBaixado"
                                                        runat="server"
                                                        AutoPostBack="true"
                                                        OnCheckedChanged="chkBaixado_CheckedChanged"
                                                        CssClass="form-check-input" />
                                                    <label class="form-check-label" for="<%= chkBaixado.ClientID %>">
                                                        Baixar Multa
                                                    </label>
                                                </div>

                                                <!-- ESPAÇO FLEXÍVEL -->
                                                <div class="col"></div>

                                                <!-- DIREITA -->
                                                <div class="col-auto text-end">
                                                   <%-- <asp:Button ID="btnPesquisarModal"
                                                        runat="server"
                                                        Text="Pesquisar"
                                                        CssClass="btn btn-info btn-sm ms-2"
                                                        OnClick="btnPesquisarModal_Click"
                                                        UseSubmitBehavior="false" />--%>

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
                </div>
            </div>
        </section>
    </div>
</asp:Content>

