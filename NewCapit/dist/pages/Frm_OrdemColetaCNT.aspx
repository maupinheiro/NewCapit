<%@ Page Title="" Language="C#" MasterPageFile="Main.Master" AutoEventWireup="true" CodeBehind="Frm_OrdemColetaCNT.aspx.cs" Inherits="NewCapit.dist.pages.Frm_OrdemColetaCNT" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--<script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery.mask/1.14.16/jquery.mask.min.js"></script>--%>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    <script>
        function calcularTempoAgCarreg(item) {
            const chegada = item.querySelector('.chegada').value;
            const saida = item.querySelector('.saida').value;
            const espera = item.querySelector('.espera');

            if (chegada && saida) {
                const dtChegada = new Date(chegada);
                const dtSaida = new Date(saida);

                if (!isNaN(dtChegada) && !isNaN(dtSaida)) {
                    const diffMs = dtSaida - dtChegada;

                    if (diffMs < 0) {
                        espera.value = "Inválido";
                        return;
                    }

                    const diffMin = Math.floor(diffMs / 60000);
                    const horas = Math.floor(diffMin / 60);
                    const minutos = diffMin % 60;

                    espera.value = `${horas}h ${minutos}min`;
                } else {
                    espera.value = '';
                }
            }
        }

        function calcularTempoEsperaGate(item) {
            const chegadaPlanta = item.querySelector('.chegada-planta').value;
            const entrada = item.querySelector('.entrada-planta').value;
            const esperaGate = item.querySelector('.espera-gate');

            if (chegadaPlanta && entrada) {
                const dtChegadaPlanta = new Date(chegadaPlanta);
                const dtEntrada = new Date(entrada);

                if (!isNaN(dtChegadaPlanta) && !isNaN(dtEntrada)) {
                    const diffMs = dtEntrada - dtChegadaPlanta;

                    if (diffMs < 0) {
                        esperaGate.value = "Inválido";
                        return;
                    }

                    const diffMin = Math.floor(diffMs / 60000);
                    const horas = Math.floor(diffMin / 60);
                    const minutos = diffMin % 60;

                    esperaGate.value = `${horas}h ${minutos}min`;
                } else {
                    esperaGate.value = '';
                }
            }
        }
        function calcularTempoDentroPlanta(item) {
            const entrada = item.querySelector('.entrada-planta')?.value;
            const saida = item.querySelector('.saida-planta')?.value;
            const dentro = item.querySelector('.dentro-planta');

            if (entrada && saida) {
                const dtEntrada = new Date(entrada);
                const dtSaida = new Date(saida);

                if (!isNaN(dtEntrada) && !isNaN(dtSaida)) {
                    const diffMs = dtSaida - dtEntrada;

                    if (diffMs < 0) {
                        dentro.value = "Inválido";
                        return;
                    }

                    const diffMin = Math.floor(diffMs / 60000);
                    const horas = Math.floor(diffMin / 60);
                    const minutos = diffMin % 60;

                    dentro.value = `${horas}h ${minutos}min`;
                } else {
                    dentro.value = '';
                }
            }
        }


        function bindEventos() {
            const itens = document.querySelectorAll('.item-coleta');
            itens.forEach(item => {
                // Parte 1: Espera fornecedor
                const chegada = item.querySelector('.chegada');
                const saida = item.querySelector('.saida');
                chegada?.addEventListener('change', () => calcularTempoAgCarreg(item));
                saida?.addEventListener('change', () => calcularTempoAgCarreg(item));

                // Parte 2: Espera Gate
                const chegadaPlanta = item.querySelector('.chegada-planta');
                const entrada = item.querySelector('.entrada-planta');
                chegadaPlanta?.addEventListener('change', () => calcularTempoEsperaGate(item));
                entrada?.addEventListener('change', () => {
                    calcularTempoEsperaGate(item);
                    calcularTempoDentroPlanta(item); // atualiza também dentro da planta ao alterar entrada
                });

                // Parte 3: Dentro da Planta
                const saidaPlanta = item.querySelector('.saida-planta');
                saidaPlanta?.addEventListener('change', () => calcularTempoDentroPlanta(item));
            });
        }


        window.addEventListener('load', bindEventos);
    </script>


    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <div class="container-xxl">
        <div class="content-wrapper">
            <div class="container-fluid">
                <div class="card card-info">
                    <div class="card-header">
                        <div class="d-sm-flex align-items-left justify-content-between mb-3">
                            <h3 class="card-title"><i class="fas fa-pallet"></i>&nbsp;ORDEM DE COLETA - 
                                <asp:Label ID="novaColeta" runat="server"></asp:Label></h3>
                            <%--<h3 class="card-title"><i class="fas fa-shipping-fast"></i>&nbsp;--%>
                            <%--<asp:Label ID="txtFilial" runat="server">CNT</asp:Label></h3>--%>
                        </div>
                    </div>
                </div>
            </div>
            <div class="card-header">
                <div class="row g-3">
                    <div class="col-md-1">
                        <div class="form-group">
                            <span class="details">MOTORISTA:</span>
                            <asp:TextBox ID="txtCodMotorista" runat="server" font-weight="bold" Style="text-align: center" class="form-control" placeholder=""></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-1">
                        <br />
                        <asp:Button ID="btnPesquisarMotorista" runat="server" Text="Pesquisar" CssClass="btn btn-outline-warning" OnClick="btnPesquisarMotorista_Click" />
                    </div>
                    <div class="col-md-2">
                        <div class="form-group">
                            <span class="details">FILIAL:</span>
                            <asp:TextBox ID="txtFilialMot" runat="server" class="form-control" placeholder=""></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <div class="form-group">
                            <span class="details">TIPO DE MOTORISTA:</span>
                            <asp:TextBox ID="txtTipoMot" runat="server" class="form-control" placeholder=""></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <div class="form-group">
                            <span class="details">FUNÇÃO:</span>
                            <asp:TextBox ID="txtFuncao" runat="server" class="form-control" placeholder=""></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-1">
                        <div class="form-group">
                            <span class="details">EX.TOXIC.:</span>
                            <div class="input-group">
                                <asp:TextBox ID="txtExameToxic" runat="server" class="form-control" Style="text-align: center"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-1">
                        <div class="form-group">
                            <span class="details">VAL. CNH:</span>
                            <div class="input-group">
                                <asp:TextBox ID="txtCNH" runat="server" class="form-control" Style="text-align: center"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-1">
                        <div class="form-group">
                            <span class="details">VAL. GR.:</span>
                            <div class="input-group">
                                <asp:TextBox ID="txtLibGR" runat="server" class="form-control" Style="text-align: center"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-1">
                        <div class="form-group">
                            <img src="<%=fotoMotorista%>" class="rounded float-right" height="80" width="80" alt="User Image">
                        </div>
                    </div>
                </div>
                <div class="row g-3">
                    <div class="col-md-5">
                        <div class="form-group">
                            <span class="details">NOME COMPLETO:</span>
                            <asp:TextBox ID="txtNomMot" runat="server" class="form-control" placeholder=""></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <div class="form-group">
                            <span class="details">CPF:</span>
                            <div class="input-group">
                                <asp:TextBox ID="txtCPF" runat="server" class="form-control" Style="text-align: center"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <div class="form-group">
                            <span class="details">CARTÃO PAMCARD:</span>
                            <div class="input-group">
                                <asp:TextBox ID="txtCartao" runat="server" class="form-control" Style="text-align: center"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-1">
                        <div class="form-group">
                            <span class="details">MÊS/ANO:</span>
                            <div class="input-group">
                                <asp:TextBox ID="txtValCartao" runat="server" class="form-control" Style="text-align: center"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <div class="form-group">
                            <span class="details">CELULAR:</span>
                            <div class="input-group">
                                <asp:TextBox ID="txtCelular" runat="server" class="form-control" Style="text-align: center"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row g-3">
                    <div class="col-md-1">
                        <div class="form-group">
                            <span class="details">CÓD./FROTA:</span>
                            <asp:TextBox ID="txtCodVeiculo" runat="server" Style="text-align: center" class="form-control" placeholder=""></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-1">
                        <br />
                        <asp:Button ID="btnPesquisarVeiculo" runat="server" Text="Pesquisar" CssClass="btn btn-outline-warning" OnClick="btnPesquisarVeiculo_Click" />
                    </div>
                    <div class="col-md-2">
                        <div class="form-group">
                            <span class="details">FILIAL:</span>
                            <asp:TextBox ID="txtFilialVeicCNT" runat="server" Style="text-align: center" class="form-control" placeholder=""></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <div class="form-group">
                            <span class="details">TIPO DE VEÍCULO:</span>
                            <asp:TextBox ID="txtVeiculoTipo" runat="server" Style="text-align: center" class="form-control" placeholder=""></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-1"></div>
                    <div class="col-md-1">
                        <div class="form-group">
                            <span class="details">OPACIDADE:</span>
                            <div class="input-group">
                                <asp:TextBox ID="txtOpacidade" runat="server" class="form-control" Style="text-align: center"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-1">
                        <div class="form-group">
                            <span class="details">LICENÇA CET:</span>
                            <div class="input-group">
                                <asp:TextBox ID="txtCET" runat="server" class="form-control" Style="text-align: center"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-1">
                        <div class="form-group">
                            <span class="details">VAL. CRLV:</span>
                            <asp:TextBox ID="txtCRLVVeiculo" runat="server" class="form-control" Style="text-align: center"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-1">
                        <div class="form-group">
                            <span class="details">VAL.CRLVREB1.:</span>
                            <div class="input-group">
                                <asp:TextBox ID="txtCRLVReb1" runat="server" class="form-control" Style="text-align: center"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-1">
                        <div class="form-group">
                            <span class="details">VAL.CRLVREB2.:</span>
                            <div class="input-group">
                                <asp:TextBox ID="txtCRLVReb2" runat="server" class="form-control" Style="text-align: center"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row g-3">
                    <div class="col-md-1">
                        <div class="form-group">
                            <span class="details">PLACA:</span>
                            <asp:TextBox ID="txtPlaca" runat="server" class="form-control" placeholder="" MaxLength="8"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <div class="form-group">
                            <span class="details">VEICULO:</span>
                            <asp:TextBox ID="txtTipoVeiculo" runat="server" class="form-control" placeholder=""></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-1">
                        <div class="form-group">
                            <span class="details">REBOQUE:</span>
                            <asp:TextBox ID="txtReboque1" runat="server" class="form-control" placeholder="" MaxLength="8"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-1">
                        <div class="form-group">
                            <span class="details">REBOQUE:</span>
                            <asp:TextBox ID="txtReboque2" runat="server" class="form-control" placeholder="" MaxLength="8"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-1">
                        <div class="form-group">
                            <span class="details">CARRETA(S):</span>
                            <asp:TextBox ID="txtCarreta" runat="server" class="form-control" placeholder=""></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-1">
                        <div class="form-group">
                            <span class="details">TECNOLOGIA:</span>
                            <asp:TextBox ID="txtTecnologia" runat="server" class="form-control" placeholder=""></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-1">
                        <div class="form-group">
                            <span class="details">RASTREAMENTO:</span>
                            <asp:TextBox ID="txtRastreamento" runat="server" class="form-control" placeholder=""></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group">
                            <span class="details">CONJUNTO:</span>
                            <asp:TextBox ID="txtConjunto" runat="server" class="form-control" placeholder=""></asp:TextBox>
                        </div>
                    </div>

                </div>
                <div class="row g-3">
                    <div class="col-md-1">
                        <div class="form-group">
                            <span class="details">CÓDIGO:</span>
                            <asp:TextBox ID="txtCodProprietario" runat="server" class="form-control" placeholder=""></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-7">
                        <div class="form-group">
                            <span class="details">PROPRIETÁRIO:</span>
                            <asp:TextBox ID="txtProprietario" runat="server" class="form-control" placeholder=""></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-1">
                        <div class="form-group">
                            <span class="details">CONTATO:</span>
                            <asp:TextBox ID="txtCodFrota" runat="server" class="form-control" placeholder=""></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <div class="form-group">
                            <span class="details">FONE CORPORATIVO:</span>
                            <asp:TextBox ID="txtFoneCorp" runat="server" class="form-control" placeholder=""></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-1">
                        <br />
                        <asp:Button ID="btnPesquisarContato" runat="server" Text="Pesquisar" CssClass="btn btn-outline-warning" OnClick="btnPesquisarContato_Click" />
                    </div>
                </div>
                <%--<div class="row g-3">
                    <div class="col-md-1">
                        <div class="form-group">
                            <span class="details">CÓDIGO:</span>
                            <asp:TextBox ID="codCliInicial" runat="server" class="form-control" OnTextChanged="codCliInicial_TextChanged" AutoPostBack="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <div class="form-group">
                            <span class="details">INICIO DA PRESTAÇÃO:</span>
                            <asp:DropDownList ID="ddlCliInicial" runat="server" OnTextChanged="ddlCliInicial_TextChanged" AutoPostBack="True" class="form-control select2"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-1">
                        <div class="form-group">
                            <span class="details">CÓDIGO:</span>
                            <asp:TextBox ID="codCliFinal" runat="server" class="form-control" OnTextChanged="codCliFinal_TextChanged" AutoPostBack="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <div class="form-group">
                            <span class="details">TERMINO DA PRESTAÇÃO:</span>
                            <asp:DropDownList ID="ddlCliFinal" runat="server" OnTextChanged="ddlCliFinal_TextChanged" AutoPostBack="True" class="form-control select2"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <div class="form_group">
                            <span class="details">Tipo de Veículo:</span>
                            <asp:DropDownList ID="ddlVeiculosCNT" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-1">
                        <div class="form-group">
                            <span class="details">Distância:</span>
                            <asp:TextBox ID="txtDistancia" runat="server" class="form-control" placeholder=""></asp:TextBox>
                            <asp:Label ID="lblDistancia" runat="server" Text="" ForeColor="Red" Font-Size="XX-Small"></asp:Label>
                        </div>
                    </div>

                </div>--%>

                <div class="row g-3">
                    <div class="col-md-1">
                        <div class="form-group">
                            <span class="details">COLETA:</span>
                            <asp:TextBox ID="txtColeta" runat="server" Style="text-align: center" class="form-control" placeholder=""></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-1">
                        <br />
                        <asp:Button ID="bntPesquisaColeta" runat="server" Text="Pesquisar" CssClass="btn btn-outline-warning" OnClick="bntPesquisaColeta_Click" />
                    </div>
                </div>
                <div class="row g-3">
                    <div class="col-md-12">
                        <div class="card">
                            <!-- ./card-header -->
                            <div class="card-body">
                                <asp:Label ID="lblMensagem" runat="server" Text=""></asp:Label>
                                <asp:UpdatePanel ID="updColetas" runat="server">
                                    <ContentTemplate>
                                        <asp:Repeater ID="rptColetas" runat="server" OnItemDataBound="rptColetas_ItemDataBound" OnItemCommand="rptColetas_ItemCommand">
                                            <HeaderTemplate>
                                                <table class="table table-bordered table-hover">
                                                    <thead>
                                                        <tr>
                                                            <th>COLETA</th>
                                                            <th>CVA</th>
                                                            <th>DATA COLETA</th>
                                                            <th>CODIGO</th>
                                                            <th>ORIGEM</th>
                                                            <th>CODIGO</th>
                                                            <th>DESTINO</th>
                                                            <th>ATENDIMENTO</th>
                                                            <th>AÇÕES</th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr data-widget="expandable-table" aria-expanded="false">
                                                    <td>
                                                        <asp:Label ID="lblCarga" runat="server" Text='<%# Eval("carga") %>' /></td>
                                                    <td><%# Eval("cva") %></td>
                                                    <td><%# Eval("data_hora", "{0:dd/MM/yyyy HH:mm}") %></td>
                                                    <td><%# Eval("CodigoO") %></td>
                                                    <td><%# Eval("cliorigem") %></td>
                                                    <td><%# Eval("CodigoD") %></td>
                                                    <td><%# Eval("clidestino") %></td>
                                                    <td runat="server" id="tdAtendimento">
                                                        <asp:Label ID="lblAtendimento" runat="server" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnRemoverColeta" runat="server" Text="Remover" CssClass="btn btn-outline-danger" CommandName="Remover" CommandArgument='<%# Eval("carga") %>' />
                                                    </td>
                                                </tr>
                                                <tr class="expandable-body">
                                                    <td colspan="12">
                                                        <div class="row g-3">
                                                            <div class="col-md-2">
                                                                <h3 class="card-title">TIPO DE VIAGEM:
                                                        <asp:Label ID="lblTipoViagem" runat="server" Text='<%# Eval("tipo_viagem") %>' ForeColor="Blue" /></h3>
                                                            </div>
                                                            <div class="col-md-1">
                                                                <h3 class="card-title">ROTA:
                                                        <asp:Label ID="lblRota" runat="server" Text='<%# Eval("rota") %>' ForeColor="Blue" /></h3>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <h3 class="card-title">VEICULO:
                                                        <asp:Label ID="lblVeiculo" runat="server" Text='<%# Eval("veiculo") %>' ForeColor="Blue" /></h3>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <h3 class="card-title">QUANT./PALLET´S:
                                                        <asp:Label ID="lblQuant" runat="server" Text='<%# Eval("quant_palet") %>' ForeColor="Blue" /></h3>
                                                            </div>
                                                        </div>
                                                        <div class="row g-3">
                                                            <div class="col-md-2">
                                                                <h3 class="card-title">PESO:
                                                        <asp:Label ID="lblPeso" runat="server" Text='<%# Eval("peso") %>' ForeColor="Blue" /></h3>
                                                            </div>
                                                            <div class="col-md-1">
                                                                <h3 class="card-title">M<sup>3</sup>:
                                                        <asp:Label ID="lblMetragem" runat="server" Text='<%# Eval("pedidos") %>' ForeColor="Blue" /></h3>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <h3 class="card-title">SOLICITAÇÃO:
                                                        <asp:Label ID="lblSolicitacao" runat="server" Text='<%# Eval("solicitacoes") %>' ForeColor="Blue" /></h3>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <h3 class="card-title">ESTUDO DE ROTA:
                                                        <asp:Label ID="lblEstRota" runat="server" Text='<%# Eval("estudo_rota") %>' ForeColor="Blue" /></h3>
                                                            </div>
                                                            <div class="col-md-2">
                                                                <h3 class="card-title">REMESSA:
                                                        <asp:Label ID="lblRemessa" runat="server" Text='<%# Eval("remessa") %>' ForeColor="Blue" /></h3>
                                                            </div>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                </tbody>
                                    </table>
                                            </FooterTemplate>
                                        </asp:Repeater>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="rptColetas" EventName="ItemCommand" />
                                    </Triggers>
                                </asp:UpdatePanel>


                            </div>
                            <!-- /.card-body -->
                        </div>
                    </div>
                </div>
                <div class="row g-3">
                    <div class="col-md-2">
                        <div class="form-group">
                            <span class="details">CADASTRADO EM:</span>
                            <asp:Label ID="lblDtCadastro" runat="server" CssClass="form-control" placeholder="" maxlength="20"></asp:Label>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group">
                            <span class="details">POR:</span>
                            <asp:TextBox ID="txtUsuCadastro" runat="server" CssClass="form-control" placeholder="" MaxLength="60"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <div class="form-group">
                            <span class="details">ATUALIZADO EM:</span>
                            <asp:Label ID="lblAtualizadoEm" runat="server" CssClass="form-control" placeholder="" maxlength="20"></asp:Label>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group">
                            <span class="details">POR:</span>
                            <asp:TextBox ID="txtAtualizadoPor" runat="server" CssClass="form-control" placeholder="" MaxLength="60"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row g-3">
                    <div class="col-md-2">
                        <br />
                        <asp:Button ID="btnSalvar1" CssClass="btn btn-outline-success  btn-lg" runat="server" Text="Salvar" OnClick="btnSalvar1_Click" />

                    </div>

                    <div class="col-md-1">
                        <br />
                        <a href="ConsultaEntregas.aspx" class="btn btn-outline-danger btn-lg">Sair               
                        </a>
                    </div>
                </div>
            </div>
        </div>
        <!-- Mensagens de erro toast -->
        <div class="toast-container position-fixed top-0 end-0 p-3">
            <div id="toastNotFound" class="toast align-items-center text-bg-danger border-0" role="alert" aria-live="assertive" aria-atomic="true">
                <div class="d-flex">
                    <div class="toast-body">
                        Cliente não encontrado. Verifique o código digitado.
                    </div>
                    <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast" aria-label="Close"></button>
                </div>
            </div>
        </div>
    </div>
    <footer class="main-footer">
        <div class="float-right d-none d-sm-block">
            <b>Version</b> 3.1.0 
        </div>
        <strong>Copyright &copy; 2023-2025 <a href="#">Capit Logística</a>.</strong> Todos os direitos reservados.
    </footer>

    <script>
        function mostrarToastNaoEncontrado() {
            var toastEl = document.getElementById('toastNotFound');
            var toast = new bootstrap.Toast(toastEl);
            toast.show();
        }
    </script>
    <script>
        $(function () {
            //Initialize Select2 Elements
            $('.select2').select2()

            //Initialize Select2 Elements
            $('.select2bs4').select2({
                theme: 'bootstrap4'
            })



        })


    </script>

</asp:Content>
