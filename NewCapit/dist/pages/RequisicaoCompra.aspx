<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="RequisicaoCompra.aspx.cs" Inherits="NewCapit.dist.pages.RequisicaoCompra" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.0/css/all.min.css" />
    <script>
        function salvarAssinatura() {
            var canvas = document.getElementById("canvasAssinatura");
            var dataUrl = canvas.toDataURL();
            document.getElementById("<%= hfAssinatura.ClientID %>").value = dataUrl;
        }
    </script>
    <script>
        function somenteNumeros(e) {
            var tecla = (window.event) ? event.keyCode : e.which;

            // Permite apenas números (0-9)
            if (tecla >= 48 && tecla <= 57) {
                return true;
            }
            return false;
        }
    </script>
    <style>
        .btn-remover {
            padding: 4px 6px;
            border-radius: 4px;
        }

            .btn-remover:hover {
                background-color: #fde7e9;
            }

        .btn-remover {
            color: #d13438;
            font-size: 16px;
            cursor: pointer;
            text-decoration: none;
            display: inline-block;
        }

            .btn-remover:hover {
                color: #a80000;
                transform: scale(1.1);
            }

        .grid-sap {
            width: 100%;
            border-collapse: collapse;
            font-family: Arial;
            font-size: 13px;
        }

            /* Cabeçalho azul SAP */
            .grid-sap th {
                background: linear-gradient(to bottom, #0a6ed1, #0854a0);
                color: white;
                padding: 8px;
                text-align: left;
                border: 1px solid #d9d9d9;
            }

            /* Linhas */
            .grid-sap td {
                padding: 6px;
                border: 1px solid #e0e0e0;
            }

            /* Hover estilo SAP */
            .grid-sap tr:hover td {
                background-color: #f5faff;
            }

        /* Inputs */
        .input-grid {
            width: 100%;
            border: 1px solid #ccc;
            padding: 4px;
            border-radius: 3px;
        }

        /* Quantidade menor */
        .qtd {
            width: 80px;
        }

        /* Botão remover */
        /*.btn-remover {
            color: #d13438;
            font-weight: bold;
            text-decoration: none;
            cursor: pointer;
        }

            .btn-remover:hover {
                color: #a80000;
            }*/
    </style>
    
    <div class="content-wrapper">
        <section class="content">
            <div class="container-fluid">
                <br />
                <div class="col-md-12">
                    <div class="card card-info">
                        <!-- Header -->
                        <div class="card-header" style="background-color: #A020F0; font-weight: bold;">
                            <h3 class="card-title"><i class="far fa-credit-card"></i>&nbsp;Controle de Compras
                                <br />
                                <small>Requisição de Compras</small></h3>
                        </div>
                        <br />
                        <%--HeaderStyle-CssClass="gv-header-custom"--%>

                        <div class="card-body">
                            <div id="divMsg" runat="server"
                                class="alert alert-dismissible fade show mt-3"
                                role="alert" visible="false">
                                <asp:Label ID="lblMsgGeral" runat="server"></asp:Label>
                                <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
                            </div>
                            <div class="container">
                                <div class="row mb-3">
                                    <div class="col-md-3">
                                        <label>SOLICITANTE:</label>
                                        <asp:TextBox ID="txtSolicitante" runat="server" CssClass="form-control" placeholder="Solicitante"></asp:TextBox>
                                    </div>
                                    <div class="col-md-5">
                                        <label>SETOR:</label>
                                        <asp:TextBox ID="txtSetor" runat="server" CssClass="form-control" placeholder="Setor"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                        <label>DATA:</label>
                                        <asp:TextBox ID="txtData" runat="server" CssClass="form-control" placeholder="Data"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                        <label>REQUISIÇÃO:</label>
                                        <asp:TextBox ID="txtNumero" runat="server" CssClass="form-control" placeholder="REQ-9999-000000" ReadOnly="true" Style="text-align: center"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row mb-3">
                                    <div class="col-md-12">
                                        <label>OBSERVAÇÃO:</label>
                                        <asp:TextBox ID="txtObservacao" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row mb-3">
                                    <div class="col-md-12">
                                        <div class="card p-3">

    <!-- DRAG & DROP -->
    <div id="dropArea" class="border p-4 text-center mb-3" style="cursor:pointer;">
        Arraste arquivos aqui ou clique para selecionar
        <asp:FileUpload ID="fileUploadAnexo" runat="server" AllowMultiple="true" style="display:none;" />
    </div>

    <!-- PREVIEW -->
    <div id="preview" class="mb-3"></div>

    <!-- BOTÃO -->
    <asp:Button ID="btnUpload" runat="server" Text="Enviar Arquivos"
        CssClass="btn btn-primary" OnClick="btnUpload_Click" />

</div>

<!-- GRID -->
<asp:GridView ID="gvAnexos" runat="server" AutoGenerateColumns="false"
    OnRowCommand="gvAnexos_RowCommand" CssClass="table">
    <Columns>

        <asp:BoundField DataField="NomeOriginal" HeaderText="Arquivo" />

        <asp:TemplateField HeaderText="Download">
            <ItemTemplate>
                <a href='<%# Eval("CaminhoArquivo") %>' target="_blank">Baixar</a>
            </ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="Ações">
            <ItemTemplate>
                <asp:LinkButton ID="btnExcluir" runat="server"
                    CommandName="Excluir"
                    CommandArgument='<%# Eval("Id") %>'
                    CssClass="btn btn-danger btn-sm">
                    Excluir
                </asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>

    </Columns>
</asp:GridView>
                                    </div>
                                </div>
                                <%--<div class="row mb-3">
                                    <div class="col-md-12">
                                        <asp:FileUpload ID="fileUploadAnexo" runat="server" CssClass="form-control" AllowMultiple="true" />

                                        <asp:Button
                                            ID="btnUpload"
                                            runat="server"
                                            Text="Anexar Arquivo"
                                            CssClass="btn btn-success"                                            
                                            OnClick="btnUpload_Click" />
                                    </div>
                                </div>--%>
                                <div class="row mb-3">
                                    <div class="col-md-1">
                                        <label>CODIGO:</label>
                                        <asp:TextBox ID="txtCodigo" runat="server" CssClass="form-control" onkeypress="return somenteNumeros(event)" placeholder="" AutoPostBack="true" OnTextChanged="txtCodigo_TextChanged"></asp:TextBox>
                                    </div>
                                    <div class="col-md-4">
                                        <label>DESCRIÇÃO DO PRODUTO:</label>
                                        <div class="form_group">
                                            <asp:DropDownList ID="ddlDescricao" runat="server" CssClass="form-control select2" AutoPostBack="true" OnSelectedIndexChanged="ddlDescricao_SelectedIndexChanged"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-1">
                                        <label>UN:</label>
                                        <asp:TextBox ID="txtUnidade" runat="server" CssClass="form-control" placeholder=""></asp:TextBox>
                                    </div>
                                    <div class="col-md-1">
                                        <label>EST.:</label>
                                        <asp:TextBox ID="txtEstoque" runat="server" CssClass="form-control" placeholder=""></asp:TextBox>
                                    </div>
                                    <div class="col-md-1">
                                        <label>QTD:</label>
                                        <asp:TextBox ID="txtQtd" runat="server" CssClass="form-control" onkeypress="return somenteNumeros(event)" placeholder=""></asp:TextBox>
                                    </div>
                                    <div class="col-md-4">
                                        <label>APLICAÇÃO/DESTINO:</label>
                                        <asp:TextBox ID="txtAplicacao" runat="server" CssClass="form-control" placeholder=""></asp:TextBox>
                                    </div>
                                </div>

                                <div class="row mb-3">
                                    <br />
                                    <asp:GridView ID="gvItens" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-striped table-hover"
                                        HeaderStyle-CssClass="gv-header-custom"
                                        OnRowCommand="gvItens_RowCommand">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Produto">
                                                <ItemTemplate>
                                                    <div><%# Eval("codigo")%></div>
                                                    <%--<div class="sub-info"><%# Eval("emissao", "{0:dd/MM/yyyy HH:mm}") %></div>--%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Descrição">
                                                <ItemTemplate>
                                                    <div><%# Eval("descricao")%></div>
                                                    <%--<div class="sub-info"><%# Eval("emissao", "{0:dd/MM/yyyy HH:mm}") %></div>--%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Un.">
                                                <ItemTemplate>
                                                    <div><%# Eval("unidade")%></div>
                                                    <%--<div class="sub-info"><%# Eval("emissao", "{0:dd/MM/yyyy HH:mm}") %></div>--%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Qtd.">
                                                <ItemTemplate>
                                                    <div><%# Eval("quantidade")%></div>
                                                    <%--<div class="sub-info"><%# Eval("emissao", "{0:dd/MM/yyyy HH:mm}") %></div>--%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Aplicação/Destino">
                                                <ItemTemplate>
                                                    <div><%# Eval("aplicacao")%></div>
                                                    <%--<div class="sub-info"><%# Eval("emissao", "{0:dd/MM/yyyy HH:mm}") %></div>--%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ações">

                                                <ItemStyle HorizontalAlign="Center" />
                                                <HeaderStyle HorizontalAlign="Center" />

                                                <ItemTemplate>
                                                    <asp:LinkButton
                                                        ID="btnExcluir"
                                                        runat="server"
                                                        CommandName="Excluir"
                                                        CommandArgument='<%# Container.DataItemIndex %>'
                                                        CssClass="btn btn-danger btn-sm"
                                                        OnClientClick="return confirm('Deseja excluir este item?');">

            <i class="fa fa-trash"></i>

        </asp:LinkButton>
                                                </ItemTemplate>

                                            </asp:TemplateField>

                                        </Columns>

                                    </asp:GridView>


                                </div>

                                <%--<div class="row mb-3">
                                    <br />
                                    <asp:GridView
                                        ID="gvAnexos"
                                        runat="server"
                                        AutoGenerateColumns="false"
                                        OnRowCommand="gvAnexos_RowCommand"
                                        CssClass="table table-bordered table-striped table-hover"
                                        HeaderStyle-CssClass="gv-header-custom">

                                        <Columns>

                                            <asp:BoundField DataField="NomeArquivo" HeaderText="Arquivo" />

                                            <asp:TemplateField HeaderText="Download">
                                                <ItemTemplate>
                                                    <a href='<%# Eval("CaminhoArquivo") %>' target="_blank">Baixar</a>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Ações">
                                                <ItemTemplate>
                                                    <asp:LinkButton
                                                        ID="btnExcluir"
                                                        runat="server"
                                                        CommandName="Excluir"
                                                        CommandArgument='<%# Eval("Id") %>'
                                                        CssClass="btn btn-danger btn-sm"
                                                        OnClientClick="return confirm('Deseja excluir este anexo?');">
                    <i class="fa fa-trash"></i>
                </asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                        </Columns>

                                    </asp:GridView>


                                </div>--%>

                                <div class="row mb-3">
                                    <div class="col-md-2">
                                        <asp:Button ID="btnAddItem" runat="server" Text="Adicionar Item" CssClass="btn btn-primary w-100" OnClick="btnAddItem_Click" />
                                    </div>
                                    <div class="col-md-3">
                                        <asp:Button ID="btnSalvar" runat="server" Text="Salvar Requisição" CssClass="btn btn-success w-100" OnClick="btnSalvar_Click" />
                                    </div>
                                    <div class="col-md-3">
                                        <asp:Button ID="btnEnviar" runat="server" Text="Enviar para Aprovação" CssClass="btn btn-warning w-100" OnClick="btnEnviar_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal fade" id="modalItem" tabindex="-1">
                            <div class="modal-dialog">
                                <div class="modal-content">

                                    <div class="modal-header">
                                        <h5>Adicionar Item</h5>
                                    </div>

                                    <div class="modal-body">
                                        <%--<asp:TextBox ID="txtProduto" runat="server" CssClass="form-control" placeholder="Produto"></asp:TextBox>--%>
                                        <%-- <select id="ddlProduto" runat="server" class="form-control select2-erp"></select>--%>
                                        <select id="ddlProduto" class="form-control select2-erp"></select>
                                        <asp:TextBox ID="txtQuantidade" runat="server" CssClass="form-control" placeholder="Quantidade"></asp:TextBox>


                                        <canvas id="canvasAssinatura" width="400" height="150" style="border: 1px solid #000;"></canvas>

                                        <br />

                                        <button type="button" onclick="salvarAssinatura()">Salvar Assinatura</button>

                                        <asp:HiddenField ID="hfAssinatura" runat="server" />
                                    </div>

                                    <div class="modal-footer">
                                        <asp:Button ID="btnSalvarItem" runat="server" Text="Salvar" CssClass="btn btn-success" />
                                        <%-- OnClick="btnSalvarItem_Click"--%>
                                    </div>

                                </div>
                            </div>
                        </div>

                    </div>
                </div>
        </section>
    </div>
    <script>
        const dropArea = document.getElementById("dropArea");
        const fileInput = document.getElementById("<%= fileUploadAnexo.ClientID %>");

        // clicar na área abre o seletor
        dropArea.addEventListener("click", () => {
            fileInput.click();
        });

        // arrastar arquivo
        dropArea.addEventListener("dragover", (e) => {
            e.preventDefault();
            dropArea.style.background = "#eee";
        });

        dropArea.addEventListener("dragleave", () => {
            dropArea.style.background = "";
        });

        // soltar arquivo
        dropArea.addEventListener("drop", (e) => {
            e.preventDefault();
            dropArea.style.background = "";

            fileInput.files = e.dataTransfer.files;
        });
    </script>
</asp:Content>
