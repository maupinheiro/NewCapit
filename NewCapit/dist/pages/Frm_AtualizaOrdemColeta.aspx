﻿<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="Frm_AtualizaOrdemColeta.aspx.cs" Inherits="NewCapit.dist.pages.Frm_AtualizaOrdemColeta" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-xxl">
        <div class="content-wrapper">
            <div class="container-fluid">
                <div class="card card-info">
                    <div class="card-header">
                        <div class="d-sm-flex align-items-left justify-content-between mb-3">
                            <h3 class="card-title"><i class="fas fa-pallet"></i>&nbsp;ORDEM DE COLETA -
    <asp:Label ID="novaColeta" runat="server"></asp:Label></h3>
                            <h3 class="card-title"><i class="fas fa-shipping-fast"></i>&nbsp;
                                <asp:Label ID="txtFilial" runat="server">CNT</asp:Label></h3>
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
                    <div class="col-md-3">
                        <div class="form-group">
                            <span class="details">FILIAL:</span>
                            <asp:TextBox ID="txtFilialMot" runat="server" Style="text-align: center" class="form-control" placeholder=""></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <div class="form-group">
                            <span class="details">TIPO:</span>
                            <asp:TextBox ID="txtTipoMot" runat="server" Style="text-align: center" class="form-control" placeholder=""></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-1"></div>
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
                            <span class="details">VEICULO:</span>
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
                            <span class="details">TIPO:</span>
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
                            <span class="details">VAL. CRLV REB.:</span>
                            <div class="input-group">
                                <asp:TextBox ID="txtCRLVReb1" runat="server" class="form-control" Style="text-align: center"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-1">
                        <div class="form-group">
                            <span class="details">VAL. CRLV REB.:</span>
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
                            <span class="details">TIPO VEICULO:</span>
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
                            <span class="details">VEÍCULO:</span>
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
                    <div class="col-md-1">
                        <br />
                        <asp:Button ID="btnSalvarColeta" runat="server" Text="Salvar Coleta" CssClass="btn btn-outline-success" />
                    </div>
                </div>
                <div class="row g-3">
                    <div class="col-md-12">
                        <div class="card">
                            <!-- ./card-header -->
                            <div class="card-body">

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
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr data-widget="expandable-table" aria-expanded="false">
                                            <td><%# Eval("carga") %></td>
                                            <td><%# Eval("cva") %></td>
                                            <td><%# Eval("data_hora", "{0:dd/MM/yyyy HH:mm}") %></td>
                                            <td><%# Eval("CodigoO") %></td>
                                            <td><%# Eval("cliorigem") %></td>
                                            <td><%# Eval("CodigoD") %></td>
                                            <td><%# Eval("clidestino") %></td>
                                            <td><%# Eval("atendimento") %></td>
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
                                                <div class="row g-3">
                                                    <div class="col-md-2">
                                                        <div class="form-group">
                                                            <span class="details">Nº CVA:</span>
                                                            <div class="input-group">
                                                                <asp:TextBox ID="txtCVA" runat="server" Text='<%# Eval("cva") %>' CssClass="form-control" MaxLength="11" Style="text-align: center"></asp:TextBox>

                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <div class="form-group">
                                                            <span class="details">JANELA GATE:</span>
                                                            <div class="input-group">
                                                                <asp:TextBox ID="txtGate" runat="server" Text='<%# Eval("gate") %>' CssClass="form-control" Style="text-align: center"></asp:TextBox>

                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <div class="form-group">
                                                            <span class="">STATUS:</span>
                                                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row g-3">
                                                    <div class="col-md-2">
                                                        <div class="form-group">
                                                            <span class="details">CHEGADA FORNECEDOR:</span>
                                                            <div class="input-group">
                                                                <asp:TextBox ID="txtChegadaOrigem" runat="server" Text='<%# Eval("chegadaorigem") %>' TextMode="DateTimeLocal" CssClass="form-control" Style="text-align: center"></asp:TextBox>

                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <div class="form-group">
                                                            <span class="details">SAIDA FORNECEDOR:</span>
                                                            <div class="input-group">
                                                                <asp:TextBox ID="txtSaidaOrigem" runat="server" Text='<%# Eval("saidaorigem") %>' CssClass="form-control" TextMode="DateTimeLocal" Style="text-align: center"></asp:TextBox>

                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-1">
                                                        <div class="form-group">
                                                            <span class="details">ESPERA:</span>
                                                            <div class="input-group">
                                                                <asp:TextBox ID="txtAgCarreg" runat="server" Text='<%# Eval("tempoagcarreg") %>' CssClass="form-control" Style="text-align: center"></asp:TextBox>

                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row g-3">
                                                    <div class="col-md-2">
                                                        <div class="form-group">
                                                            <span class="details">CHEGADA PLANTA:</span>
                                                            <div class="input-group">
                                                                <asp:TextBox ID="txtChegadaDestino" runat="server" Text='<%# Eval("chegadadestino") %>' CssClass="form-control" TextMode="DateTimeLocal" Style="text-align: center"></asp:TextBox>

                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <div class="form-group">
                                                            <span class="details">ENTRADA:</span>
                                                            <div class="input-group">
                                                                <asp:TextBox ID="txtEntrada" runat="server" Text='<%# Eval("entradaplanta") %>' CssClass="form-control" Style="text-align: center" TextMode="DateTimeLocal"></asp:TextBox>

                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-1">
                                                        <div class="form-group">
                                                            <span class="details">ESP.GATE:</span>
                                                            <div class="input-group">
                                                                <asp:TextBox ID="txtEsperaGate" runat="server" Text="" CssClass="form-control" Style="text-align: center"></asp:TextBox>

                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <div class="form-group">
                                                            <span class="details">SAIDA PLANTA:</span>
                                                            <div class="input-group">
                                                                <asp:TextBox ID="txtSaidaPlanta" runat="server" Text='<%# Eval("saidaplanta") %>' CssClass="form-control" TextMode="DateTimeLocal" Style="text-align: center"></asp:TextBox>

                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-1">
                                                        <div class="form-group">
                                                            <span class="details">TEMPO:</span>
                                                            <div class="input-group">
                                                                <asp:TextBox ID="txtDentroPlanta" runat="server" Text='<%# Eval("tempodentroplanta") %>' CssClass="form-control" Style="text-align: center"></asp:TextBox>

                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-1">
                                                        <br />
                                                        <asp:Button ID="btnAtualizarColeta" runat="server" Text="Atualizar" CssClass="btn btn-outline-info" CommandName="Atualizar" CommandArgument='<%# Eval("carga") %>' />
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
                        <asp:Button ID="btnSalvar1" CssClass="btn btn-outline-success  btn-lg" runat="server" Text="Atualizar Ordem de Coleta" />
                    </div>
                    <div class="col-md-1">
                        <br />
                        <a href="ConsultaColetasCNT.aspx" class="btn btn-outline-danger btn-lg">Sair               
                        </a>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <footer class="main-footer">
        <div class="float-right d-none d-sm-block">
            <b>Version</b> 2.1.0 
        </div>
        <strong>Copyright &copy; 2021-2025 Capit Logística.</strong> Todos os direitos reservados.
    </footer>
</asp:Content>

