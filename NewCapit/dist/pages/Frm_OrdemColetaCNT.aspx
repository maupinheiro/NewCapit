<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="Frm_OrdemColetaCNT.aspx.cs" Inherits="NewCapit.dist.pages.Frm_OrdemColetaCNT" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-xxl">
        <div class="content-wrapper">
            <div class="container-fluid">
                <div class="card card-info">
                    <div class="card-header">
                        <h3 class="card-title"><i class="fas fa-pallet"></i>&nbsp;ORDEM DE COLETA</h3>
                    </div>
                </div>
            </div>
            <div class="card-header">
                <div class="row g-3">
                    <div class="col-md-1">
                        <div class="form-group">
                            <span class="details">MOTORISTA:</span>
                            <asp:TextBox ID="txtCodMotorista" runat="server" Style="text-align: center" class="form-control" placeholder=""></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-1">
                        <br />
                        <asp:Button ID="btnPesquisarMotorista" runat="server" Text="Pesquisar" CssClass="btn btn-outline-warning" OnClick="btnPesquisarMotorista_Click" />
                    </div>
                    <div class="col-md-2">
                        <div class="form-group">
                            <span class="details">FILIAL:</span>
                            <asp:TextBox ID="txtFilialMot" runat="server" Style="text-align: center" class="form-control" placeholder=""></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-4"></div>
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
                            <img src="/fotos/usuario.jpg" class="rounded float-right" alt="...">
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
                        <asp:Button ID="btnPesquisarVeiculo" runat="server" Text="Pesquisar" CssClass="btn btn-outline-warning" />
                    </div>
                    <div class="col-md-2">
                        <div class="form-group">
                            <span class="details">FILIAL:</span>
                            <asp:TextBox ID="txtFilialVeicCNT" runat="server" Style="text-align: center" class="form-control" placeholder=""></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-4"></div>
                    <div class="col-md-1">
                        <div class="form-group">
                            <span class="details">LICENÇA CET:</span>
                            <div class="input-group">
                                <input type="text" id="txtCET" class="form-control" style="text-align: center">
                            </div>
                        </div>
                    </div>
                    <div class="col-md-1">
                        <div class="form-group">
                            <span class="details">VAL. CRLV:</span>
                            <div class="input-group">
                                <input type="text" id="txtCRLVVeiculo" class="form-control" style="text-align: center">
                            </div>
                        </div>
                    </div>
                    <div class="col-md-1">
                        <div class="form-group">
                            <span class="details">VAL. CRLV REB.:</span>
                            <div class="input-group">
                                <input type="text" id="txtCRLVReb1" class="form-control" style="text-align: center">
                            </div>
                        </div>
                    </div>
                    <div class="col-md-1">
                        <div class="form-group">
                            <span class="details">VAL. CRLV REB.:</span>
                            <div class="input-group">
                                <input type="text" id="txtCRLVReb2" class="form-control" style="text-align: center">
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
                    <div class="col-md-8">
                        <div class="form-group">
                            <span class="details">PROPRIETÁRIO:</span>
                            <asp:TextBox ID="txtProprietario" runat="server" class="form-control" placeholder=""></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-1">
                        <div class="form-group">
                            <span class="details">CÓDIGO:</span>
                            <asp:TextBox ID="txtCodFrota" runat="server" class="form-control" placeholder=""></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <div class="form-group">
                            <span class="details">FONE CORP.:</span>
                            <asp:TextBox ID="txtFoneCorp" runat="server" class="form-control" placeholder=""></asp:TextBox>
                        </div>
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
                        <asp:Button ID="bntColeta" runat="server" Text="Pesquisar" CssClass="btn btn-outline-warning" />
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
                                        <tr data-widget="expandable-table" aria-expanded="false">
                                            <td>382545</td>
                                            <td>1013520</td>
                                            <td>04/04/2025 11:00</td>
                                            <td>3118/E81</td>
                                            <td>GESTAMP (TAUBATE)</td>
                                            <td>1020/11</td>
                                            <td>VW ANCHIETA</td>
                                            <td>NO PRAZO</td>
                                        </tr>
                                        <tr class="expandable-body">
                                            <td colspan="12">
                                                <div class="row g-3">
                                                    <div class="col-md-2">
                                                        <h3 class="card-title">TIPO DE VIAGEM:
                                                            <asp:Label ID="lblTipoViagem" runat="server" ForeColor="Blue">FTL</asp:Label></h3>
                                                    </div>
                                                    <div class="col-md-1">
                                                        <h3 class="card-title">ROTA:
                                                            <asp:Label ID="lblRota" runat="server" ForeColor="Blue">20.62</asp:Label></h3>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <h3 class="card-title">VEICULO:
                                                <asp:Label ID="lblVeiculo" runat="server" ForeColor="Blue">CARRETA REBAIXADA 2 EIXOS</asp:Label></h3>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <h3 class="card-title">QUANT./PALLET´S:
                                                <asp:Label ID="lblQuant" runat="server" ForeColor="Blue">1 PALLET 6 RAD0137 6 RBA0137</asp:Label></h3>
                                                    </div>
                                                </div>
                                                <div class="row g-3">
                                                    <div class="col-md-2">
                                                        <h3 class="card-title">PESO:
                                                            <asp:Label ID="lblPeso" runat="server" ForeColor="Blue">18000</asp:Label></h3>
                                                    </div>
                                                    <div class="col-md-1">
                                                        <h3 class="card-title">M<sup>3</sup>:
                                                            <asp:Label ID="lblMetragem" runat="server" ForeColor="Blue">19</asp:Label></h3>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <h3 class="card-title">SOLICITAÇÃO:
                                        <asp:Label ID="lblSolicitacao" runat="server" ForeColor="Blue">+2541252+8965896+87456989</asp:Label></h3>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <h3 class="card-title">ESTUDO DE ROTA:
                                        <asp:Label ID="lblEstRota" runat="server" ForeColor="Blue">11FTL554</asp:Label></h3>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <h3 class="card-title">REMESSA:
                                                <asp:Label ID="lblRemessa" runat="server" ForeColor="Blue">12112221</asp:Label></h3>
                                                    </div>
                                                </div>
                                                <div class="row g-3">
                                                    <div class="col-md-2">
                                                        <div class="form-group">
                                                            <span class="details">Nº CVA:</span>
                                                            <div class="input-group">
                                                                <input type="text" id="txtCVA" maxlength="11" class="form-control" style="text-align: center">
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <div class="form-group">
                                                            <span class="details">JANELA GATE:</span>
                                                            <div class="input-group">
                                                                <input type="text" id="txtGate" class="form-control" style="text-align: center" data-inputmask-alias="datetime" data-inputmask-inputformat="dd/MM/yyyy HH:mm" data-mask>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <div class="form-group">
                                                            <span class="">STATUS:</span>
                                                            <asp:DropDownList ID="ddlStatus" ame="nomeStatus" runat="server" CssClass="form-control"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                 </div>
                                                    <div class="row g-3">
                                                        <div class="col-md-2">
                                                            <div class="form-group">
                                                                <span class="details">CHEGADA FORNECEDOR:</span>
                                                                <div class="input-group">
                                                                    <input type="text" id="txtChegadaOrigem" class="form-control" style="text-align: center" textmode="DateTimeLocal">
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-2">
                                                            <div class="form-group">
                                                                <span class="details">SAIDA FORNECEDOR:</span>
                                                                <div class="input-group">
                                                                    <input type="text" id="txtSaidaOrigem" class="form-control" style="text-align: center" textmode="DateTimeLocal">
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-1">
                                                            <div class="form-group">
                                                                <span class="details">ESPERA:</span>
                                                                <div class="input-group">
                                                                    <input type="text" id="txtAgCarreg" class="form-control" style="text-align: center">
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row g-3">
                                                        <div class="col-md-2">
                                                            <div class="form-group">
                                                                <span class="details">CHEGADA PLANTA:</span>
                                                                <div class="input-group">
                                                                    <input type="text" id="txtChegadaDestino" class="form-control" style="text-align: center" textmode="DateTimeLocal">
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-2">
                                                            <div class="form-group">
                                                                <span class="details">ENTRADA:</span>
                                                                <div class="input-group">
                                                                    <input type="text" id="txtEntrada" class="form-control" style="text-align: center" textmode="DateTimeLocal">
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-1">
                                                            <div class="form-group">
                                                                <span class="details">ESP.GATE:</span>
                                                                <div class="input-group">
                                                                    <input type="text" id="txtEsperaGate" class="form-control" style="text-align: center">
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-2">
                                                            <div class="form-group">
                                                                <span class="details">SAIDA PLANTA:</span>
                                                                <div class="input-group">
                                                                    <input type="text" id="txtSaidaPlanta" class="form-control" style="text-align: center" textmode="DateTimeLocal">
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-1">
                                                            <div class="form-group">
                                                                <span class="details">TEMPO:</span>
                                                                <div class="input-group">
                                                                    <input type="text" id="txtDentroPlanta" class="form-control" style="text-align: center" textmode="DateTimeLocal">
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-1">
                                                            <br />
                                                            <asp:Button ID="btnAtualizarColeta" runat="server" Text="Atualizar" CssClass="btn btn-outline-info" />
                                                        </div>
                                                    </div>
                                            </td>
                                        </tr>

                                    </tbody>
                                </table>
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
                    <div class="col-md-1">
                        <br />
                        <asp:Button ID="btnSalvar1" CssClass="btn btn-outline-success  btn-lg" runat="server" Text="Salvar" />
                    </div>
                    <div class="col-md-1">
                        <br />
                        <a href="ConsultaClientes.aspx" class="btn btn-outline-danger btn-lg">Sair               
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
