<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="GestaoDeMultas.aspx.cs" Inherits="NewCapit.dist.pages.GestaoDeMultas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- Bootstrap 5 -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>

    <!-- Chart.js -->
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>

    <script src="https://cdn.datatables.net/1.13.6/js/jquery.dataTables.min.js"></script>
    <link href="https://cdn.datatables.net/1.13.6/css/jquery.dataTables.min.css" rel="stylesheet" />
    <script src="https://code.jquery.com/jquery-3.7.0.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery.mask/1.14.16/jquery.mask.min.js"></script>
    <script>
        document.addEventListener("DOMContentLoaded", function () {
            function aplicarMascara(input, mascara) {
                input.addEventListener("input", function () {
                    let valor = input.value.replace(/\D/g, ""); // Remove tudo que não for número
                    let resultado = "";
                    let posicao = 0;

                    for (let i = 0; i < mascara.length; i++) {
                        if (mascara[i] === "0") {
                            if (valor[posicao]) {
                                resultado += valor[posicao];
                                posicao++;
                            } else {
                                break;
                            }
                        } else {
                            resultado += mascara[i];
                        }
                    }

                    input.value = resultado;
                });
            }

            // Pegando os elementos no ASP.NET
            let txtData = document.getElementById("<%= txtData.ClientID %>");
            if (txtData) aplicarMascara(txtData, "00/00/0000 00:00");
        });
    </script>   
    <script>
        function abrirModalProcesso() {
            var modalEl = document.getElementById('mdlProcesso');
            var modal = new bootstrap.Modal(modalEl);
            modal.show();
        }
        function mascaraDataHora(campo) {
            let v = campo.value.replace(/\D/g, '');

            if (v.length >= 3)
                v = v.replace(/(\d{2})(\d)/, '$1/$2');
            if (v.length >= 6)
                v = v.replace(/(\d{2})\/(\d{2})(\d)/, '$1/$2/$3');
            if (v.length >= 11)
                v = v.replace(/(\d{4})(\d)/, '$1 $2');
            if (v.length >= 14)
                v = v.replace(/(\d{2})(\d)/, '$1:$2');

            campo.value = v;
        }

    </script>
   
    <div class="content-wrapper">
        <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server" />
        <section class="content">
            <div class="container-fluid">
                <br />
                <div id="toastContainerVermelho" class="alert alert-danger alert-dismissible" style="display: none;">
                    <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                    <h5><i class="icon fas fa-exclamation-triangle"></i>Alerta!</h5>
                    Alertas
                </div>
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
                                            placeholder="Pesquisar por veículo ou motorista ou processo" />
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
                    <div class="modal-dialog modal-xl modal-dialog-centered"">
                        <div class="modal-content">                            
                            <div class="modal-header">
                                <h5 class="modal-title">Multa/Processo</h5>
                                <button type="button" class="btn-close"
                                    data-bs-dismiss="modal">
                                </button>
                            </div>

                            <!-- UPDATEPANEL SÓ NO CONTEÚDO -->
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>

                                    <div class="modal-body">

                                        <asp:HiddenField ID="hfStatus" runat="server" />
                                        <div class="col-xl-12 col-md-12 mb-12">
                                            <div class="row g-3">
                                                     <div class="col-md-1">
                                                         <%--<label>&nbsp;</label><br /> --%>
                                                         <asp:Image ID="imgFoto" runat="server"
                                                             CssClass="img-thumbnail"
                                                             Style="width:80px;height:80px;object-fit:cover;"
                                                             AlternateText="Foto"
                                                             ImageUrl="/fotos/usuario.jpg" />
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
                                                    <asp:TextBox ID="txtData" runat="server" CssClass="form-control"  AutoPostBack="true" OnTextChanged="txtData_TextChanged"  />
                                                </div>
                                                <div class="col-md-2">
                                                    <label>&nbsp;</label><br />                                                   
                                                    <asp:TextBox ID="txtDia" runat="server" CssClass="form-control" Enabled="false" Style="text-align: center" />
                                                </div>                                                
                                                <div class="col-md-2">
                                                    <label>Lançamento:</label>
                                                    <asp:TextBox ID="txtLancamento" runat="server" CssClass="form-control" Enabled="false" Style="text-align: center" />
                                                </div>
                                                <div class="col-md-1">
                                                    <label>Id:</label>
                                                    <asp:TextBox ID="txtId" runat="server" CssClass="form-control" Enabled="false" Style="text-align: center" />
                                                </div>
                                            </div>
                                           
                                            <br />
                                            <div class="row g-3">
                                                <div class="col-md-1">
                                                     <label>Código:</label>
                                                     <asp:TextBox ID="txtCodMot" runat="server" CssClass="form-control" />
                                                </div>
                                                <div class="col-md-6">
                                                     <label>Nome do Motorista:</label>
                                                     <asp:TextBox ID="txtNome"
                                                         runat="server"
                                                         CssClass="form-control" />
                                                </div>
                                                <div class="col-md-1">
                                                    <label>Frota:</label>
                                                    <asp:TextBox ID="txtFrota" runat="server" CssClass="form-control" />
                                                </div>
                                                <div class="col-md-2">
                                                    <label>Placa:</label>
                                                    <asp:TextBox ID="txtPlaca" runat="server" CssClass="form-control" />
                                                </div>
                                                <div class="col-md-2">
                                                    <label>Equip:</label>
                                                    <asp:TextBox ID="txtEquipamento" runat="server" CssClass="form-control" />
                                                </div>
                                                
                                            </div>
                                            
                                            <br />
                                            <div class="row g-3">
                                                <div class="col-md-1">
                                                    <label>Infração:</label>
                                                    <asp:TextBox ID="txtCodigo_Infracao" runat="server" CssClass="form-control" />
                                                </div>
                                            </div>



                                       

                                        

                                        

                                    </div>
                                    <br />
                                    <div class="modal-footer">
                                        <asp:Button ID="btnPesquisarModal"
                                            runat="server"
                                            Text="Pesquisar"
                                            CssClass="btn btn-info"
                                            OnClick="btnPesquisarModal_Click"
                                            UseSubmitBehavior="false" />

                                        <asp:Button ID="btnSalvar"
                                            runat="server"
                                            Text="Salvar"
                                            CssClass="btn btn-success"
                                            OnClick="btnSalvar_Click"
                                            UseSubmitBehavior="false" />

                                        <button type="button"
                                            class="btn btn-secondary"
                                            data-bs-dismiss="modal">
                                            Fechar
                                        </button>
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
