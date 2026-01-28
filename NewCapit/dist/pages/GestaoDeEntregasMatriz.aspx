<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="GestaoDeEntregasMatriz.aspx.cs" Inherits="NewCapit.dist.pages.GestaoDeEntregasMatriz" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- Bootstrap 5 -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <!-- Chart.js -->
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>

    <script src="https://cdn.datatables.net/1.13.6/js/jquery.dataTables.min.js"></script>
    <link href="https://cdn.datatables.net/1.13.6/css/jquery.dataTables.min.css" rel="stylesheet" />
    <script src="https://code.jquery.com/jquery-3.7.0.min.js"></script>
   
    <link href="bootstrap.min.css" rel="stylesheet" />
    <script src="bootstrap.bundle.min.js"></script>
    <script>
        function buscarDocumento() {

            let numero = document.getElementById("txtNumeroDocumento").value;

            if (numero === "") {
                alert("Digite o número do documento");
                return;
            }

            fetch("ControleDePedagio.aspx/BuscarDocumento", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ numeroDocumento: numero })
            })
                .then(res => res.json())
                .then(res => {

                    if (!res.d || !res.d.encontrado) {
                        alert("Documento não encontrado");
                        return;
                    }

                    document.getElementById("lblChave").innerText = res.d.chave;
                    document.getElementById("lblEmissao").innerText = res.d.emissao;
                    document.getElementById("lblEmpresa").innerText = res.d.empresa;
                    document.getElementById("lblMotorista").innerText = res.d.motorista;
                    document.getElementById("lblDestino").innerText = res.d.destino;
                    document.getElementById("lblCidade").innerText = res.d.cidade + "/" + res.d.uf;
                    document.getElementById("lblDataSaida").innerText = res.d.dataSaida;
                });
        }
    </script>

     <div class="container-fluid">
    <div class="content-wrapper">        
        <section class="content">           
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
                                <h3 class="card-title"><i class="fas fa-shipping-fast"></i>&nbsp;GESTÃO DE COLETAS E ENTREGAS</h3>
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
                            <div class="row mb-3">
                                <div class="col-md-2">
                                    <label>Data Inicial:</label>
                                    <asp:TextBox ID="DataInicio" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <label>Data Final:</label>
                                    <asp:TextBox ID="DataFim" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                                </div>                               
                                <div class="col-md-1">
                                    <label>&nbsp;</label><br />
                                    <asp:Button ID="btnFiltrar" runat="server" CssClass="btn btn-warning w-100" Text="Filtrar" OnClick="btnFiltrar_Click" />
                                </div>
                                <div class="col-md-2">
                                    <label>&nbsp;</label><br />
                                    <asp:Button ID="btnExportarExcel" runat="server" CssClass="btn btn-success w-100" Text="Exportar para Excel" OnClick="btnExportarExcel_Click" />
                                </div>                              
                                <div class="col-md-2">
                                    <label>&nbsp;</label><br />
                                    <a href="/dist/pages/ColetasMatriz.aspx" class="d-none d-sm-inline-block btn btn-primary shadow-sm w-100"><i
                                        class="fas fa-boxes"></i>&nbsp;Abrir Carregamento
                                    </a>
                                </div>
                                <div class="col-md-1">
                                    <label>&nbsp;</label><br />
                                    <asp:Button ID="btnBaixar" runat="server" CssClass="btn btn-info w-100" Text="Baixar DOC" OnClick="btnBaixar_Click" />
                                </div>
                                <div class="col-md-2">
                                    <div class="form-group">
                                      <div class="custom-control custom-switch custom-switch-off-primary custom-switch-on-success">
                                        <input type="checkbox" class="custom-control-input" id="chkOcultarViagens" 
                                               <%= hfOcultarViagens.Value.ToLower() == "true" ? "checked" : "" %>
                                               onchange="document.getElementById('<%= hfOcultarViagens.ClientID %>').value = this.checked; document.getElementById('<%= btnPostbackOcultar.ClientID %>').click();">
    
                                        <label class="custom-control-label" for="chkOcultarViagens">Visualizar Todas as Viagens</label>

                                        <asp:HiddenField ID="hfOcultarViagens" runat="server" Value="false" />
    
                                        <%-- Botão invisível para processar o evento no servidor --%>
                                        <asp:Button ID="btnPostbackOcultar" runat="server" style="display:none;" OnClick="btnPostbackOcultar_Click" />
                                    </div>

                                    </div>
                                </div>
                            </div>
                            <div class="row mb-3">
                                <div class="col-md-12">
                                        <asp:TextBox 
                                            ID="txtPesquisar"
                                            runat="server"
                                            AutoPostBack="true"
                                            OnTextChanged="txtPesquisar_TextChanged"
                                            CssClass="form-control"
                                            placeholder="Pesquisar..." />
                                </div>
                            </div>
                            <div class="container-fluid">                                
                                <div class="row">
                                    <div class="col-12">
                                        <!-- /.col -->
                                        <div class="card">
                                            <!-- /.card-header -->
                                            <div class="card-body">
                                                <asp:Label ID="lblMensagem" runat="server" Text=""></asp:Label>
                                                <asp:Repeater ID="rptCarregamento" runat="server" OnItemDataBound="rptCarregamento_ItemDataBound">
                                                    <HeaderTemplate>
                                                        <table class="table table-striped table-bordere table-hover" AutoGenerateColumns="false">
                                                            <thead>
                                                                <tr text-align: center;>
                                                                    <th>#</th>
                                                                    <th>Veículo</th>
                                                                    <th>Placa</th>   
                                                                    <th>Motorista</th>
                                                                    <th>Expedidor/Recebedor</th>
                                                                    <th>Inicio/Termino da Prestação</th>
                                                                    <th>Ordem Coleta</th>
                                                                    <th>Atendimento</th> 
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                    </HeaderTemplate>
                                                    <ItemTemplate> 
                                                        <tr onclick="toggleDetalhes(this)">
                                                            <td style="text-align: center; vertical-align: middle;">
                                                                <asp:ImageButton ID="lnkEditar" ImageUrl='<%# Eval("fotos") %>' style="width: 60px; height:60px;" runat="server"
                                                                     CommandName="Editar"
                                                                     CommandArgument='<%# Eval("num_carregamento") %>'
                                                                     OnCommand="lnkEditar_Command"
                                                                     OnClientClick="event.stopPropagation();"/>  
                                                            </td>
                                                            <td>
                                                                <%# Eval("veiculo") %>
                                                                <br />
                                                                <%# Eval("tipoveiculo") %>
                                                            </td>
                                                            <td>
                                                                <%# Eval("placa") %>
                                                                <br />
                                                                <%# Eval("reboque1")  + " - " +  Eval("reboque2") %>

                                                            </td>   
                                                            <td>
                                                                <%# Eval("codmotorista") + " - " + Eval("nomemotorista") %>
                                                                <br />
                                                                <%# Eval("codtra") + " - " + Eval("transportadora")%>  
                                                            </td>
                                                            <td>
                                                                <%# Eval("cod_expedidor") + " - " + Eval("expedidor") %>
                                                                <br />
                                                                <%# Eval("cod_recebedor") + " - " + Eval("recebedor")%>  
                                                            </td>
                                                            <td>
                                                                <%# Eval("cid_expedidor") + "/" + Eval("uf_expedidor") %>
                                                                <br />
                                                                <%# Eval("cid_recebedor") + "/" + Eval("uf_recebedor")%>  
                                                            </td>
                                                            <td style="text-align: center; vertical-align: middle;">
                                                                <%# Eval("num_carregamento") + " ("
 + Eval("carga") + ")"  %>                                                                <br />
                                                                <%# Eval("emissao", "{0:dd/MM/yyyy HH:mm}") %>
                                                            </td>
                                                            <td>
                                                                <%--<%# Eval("situacao") %>
                                                                <br />
                                                                <%# Eval("status")%>  --%>
                                                                <%# Eval("situacao") %>
                                                                <br />
                                                                <asp:Label 
                                                                    ID="lblStatus"
                                                                    runat="server"
                                                                    Text='<%# Eval("status") %>'
                                                                    CssClass="badge" />
                                                            </td>                                                           
                                                        </tr>
                                                        <tr class="detalhes d-none">
                                                            <td colspan="11">
                                                                <asp:Repeater ID="rptColeta" OnItemDataBound="rptColeta_ItemDataBound" runat="server">
                                                                    <HeaderTemplate>
                                                                        <table class="table table-striped table-bordered table-hover" AutoGenerateColumns="false">
                                                                            <thead>
                                                                                <tr>
                                                                                    <th>Carga</th>
                                                                                    <th>Status</th>
                                                                                    <th>Remetente/Expedidor</th>
                                                                                    <th>Destinatário/Recebedor</th>
                                                                                    <th>Saída</th>
                                                                                    <th>Prev.Chegada</th>
                                                                                    <th>Chegada</th>
                                                                                    <th>Conclusão</th>
                                                                                </tr>
                                                                            </thead>
                                                                            <tbody>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <tr>
                                                                            <td>
                                                                                <%# Eval("carga") %>
                                                                                <br />
                                                                                <%# Eval("emissao", "{0:dd/MM/yyyy HH:mm}") %>
                                                                            </td>
                                                                            <td>
                                                                                <%# Eval("status") %>
                                                                                <br />
                                                                                02h30min
                                                                            </td>
                                                                            <td>
                                                                                <%# Eval("codorigem") %> + " - " + <%# Eval("cliorigem") %>
                                                                                <br />
                                                                                <%# Eval("cod_expedidor") %> + " - " + <%# Eval("expedidor") %>
                                                                            </td>
                                                                            <td>
                                                                                <%# Eval("coddestino") %> + " - " + <%# Eval("clidestino") %>
                                                                                <br />
                                                                                <%# Eval("cod_recebedor") %> + " - " + <%# Eval("recebedor") %>
                                                                            </td>
                                                                            <td><%# Eval("inicio_viagem", "{0:dd/MM/yyyy HH:mm}") %></td>
                                                                            <td><%# Eval("prev_chegada", "{0:dd/MM/yyyy HH:mm}") %></td>
                                                                            <td><%# Eval("cheg_cliente", "{0:dd/MM/yyyy HH:mm}") %></td>
                                                                            <td><%# Eval("fim_viagem", "{0:dd/MM/yyyy HH:mm}") %></td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        </tbody>
                                        </table>
                                                                    </FooterTemplate>
                                                                </asp:Repeater>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        </tbody>
                        </table>
                                                    </FooterTemplate>
                                                </asp:Repeater>



                                            </div>
                                            <!-- /.card-body -->
                                        </div>
                                        <!-- /.card -->
                                    </div>
                                </div>



                            </div>
                        </div>
                    </div>
                </div>
            </div>
         <!-- Modal -->
         <div class="modal fade" id="modalCTE" tabindex="-1">

             <asp:UpdatePanel ID="UpdatePanel1" runat="server">
             <ContentTemplate>
                          <div class="modal-dialog modal-dialog-centered modal-lg">
    <div class="modal-content">

        <div class="modal-header">
            <h5 class="modal-title">Baixar Documentos CT-e / NFS-e</h5>
            <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
        </div>

        <div class="modal-body">
            <div class="row g-3">
               <div class="col-md-2">
                   <div class="form-check">
                      <input
                        class="form-check-input"
                        type="radio"
                        name="gridRadios"
                        id="gridRadiosCTe"
                        value="option1"
                        checked
                      />
                      <label class="form-check-label" for="gridRadiosCTe"> CT-e</label>
                   </div>
               </div>
               <div class="col-md-2">
                   <div class="form-check">
                       <input
                         class="form-check-input"
                         type="radio"
                         name="gridRadios"
                         id="gridRadiosNFSe"
                         value="option2"                         
                       />
                       <label class="form-check-label" for="gridRadiosNFSe"> NFS-e</label>
                   </div>
               </div>
            </div>
            <div class="row g-3">
               <div class="col-md-4">                 
                   <asp:TextBox ID="txtNumeroDocumento" CssClass="form-control" runat="server"></asp:TextBox> 
               </div>
                
              <div class="col-md-3">                  
                  <asp:Button ID="btnBuscar" runat="server" Text="Pesquisar" OnClick="btnBuscar_Click" CssClass="btn btn-warning w-100" />
                  
             </div>
            </div>
            <hr />

            <b>Chave:</b> <asp:Label ID="lblChave" runat="server" Text=""></asp:Label> <br />
            <b>Emissão:</b> <asp:Label ID="lblEmissao" runat="server" Text=""></asp:Label> <br />
            <b>Empresa:</b> <asp:Label ID="lblEmpresa" runat="server" Text=""></asp:Label> <br />

            <hr />

            <b>Motorista:</b> <asp:Label ID="lblMotorista" runat="server" Text=""></asp:Label> <br />
            <b>Destino:</b> <asp:Label ID="lblDestino" runat="server" Text=""></asp:Label> <br />
            <b>Cidade/UF:</b> <asp:Label ID="lblCidade" runat="server" Text=""></asp:Label><br />
            <b>Data Saída:</b> <asp:Label ID="lblDataSaida" runat="server" Text=""></asp:Label> 
        </div>

        <div class="modal-footer">
            <asp:Button ID="btnSalvarBaixa" runat="server"
                CssClass="btn btn-success w-100"
                Text="Baixar"
                OnClick="btnSalvarBaixa_Click" />
        </div>

    </div>
</div>
             </ContentTemplate>    
             </asp:UpdatePanel>
             
          </div>


        </section>
           
    </div>
   



</asp:Content>
