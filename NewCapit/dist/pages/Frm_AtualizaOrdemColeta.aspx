<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="Frm_AtualizaOrdemColeta.aspx.cs" Inherits="NewCapit.dist.pages.Frm_AtualizaOrdemColeta" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%-- <!-- Bootstrap CSS -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/css/bootstrap.min.css" rel="stylesheet" />
    <!-- jQuery e Bootstrap JS -->
    <script src="https://code.jquery.com/jquery-3.6.4.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/js/bootstrap.bundle.min.js"></script>
    <!-- Bootstrap CSS + JS -->
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" />
    <script src="https://code.jquery.com/jquery-3.5.1.slim.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@4.5.2/dist/js/bootstrap.bundle.min.js"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">--%>
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>

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
            let txtCadCelular = document.getElementById("<%= txtCadCelular.ClientID %>");
            if (txtCadCelular) aplicarMascara(txtCadCelular, "(00) 0 0000-0000");
        });
    </script>
    <script type="text/javascript">
        function abrirModalTelefone() {
            // Pega valor do TextBox do Web Forms            
            var codigoFrota = document.getElementById('<%= txtCodFrota.ClientID %>').value;

            // Define o valor no TextBox do modal
            document.getElementById('<%= txtCodContato.ClientID %>').value = codigoFrota;

            //$('#telefoneModal').modal('show');
            $('#telefoneModal').modal({ backdrop: 'static', keyboard: false });
        }
    </script>

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


        function mostrarErro(item, campo, mensagem) {
            const spanErro = item.querySelector('.msg-erro');
            if (spanErro) {
                spanErro.textContent = mensagem;
                spanErro.style.display = 'block';
            }

            if (campo) {
                campo.style.border = "2px solid red";
                campo.classList.add("com-erro");
                campo.focus();
            }
        }

        function limparErros(item) {
            const spanErro = item.querySelector('.msg-erro');
            if (spanErro) {
                spanErro.textContent = '';
                spanErro.style.display = 'none';
            }

            const campos = item.querySelectorAll('input');
            campos.forEach(campo => {
                campo.style.border = "";
                campo.classList.remove("com-erro");
            });
        }

        function validarDatas(item) {
            const agora = new Date();

            const chegadaOrigemInput = item.querySelector('.chegada');
            const saidaOrigemInput = item.querySelector('.saida');
            const chegadaDestinoInput = item.querySelector('.chegada-planta');
            const entradaInput = item.querySelector('.entrada-planta');
            const saidaPlantaInput = item.querySelector('.saida-planta');
            const cvaInput = item.querySelector('.cva');
            const gateInput = item.querySelector('.gate');
            const tdDataHora = item.querySelector('.data-hora');
            const dataHoraAttr = tdDataHora?.getAttribute('data-datahora');

            limparErros(item);

            // Conversões de datas
            const v1 = new Date(chegadaOrigemInput.value);
            const v2 = new Date(saidaOrigemInput.value);
            const v3 = new Date(chegadaDestinoInput.value);
            const v4 = new Date(entradaInput.value);
            const v5 = new Date(saidaPlantaInput.value);

            // Regra 1: Gate não pode ser preenchido sem CVA
            if (cvaInput && gateInput && gateInput.value && !cvaInput.value.trim()) {
                mostrarErro(item, gateInput, "Preencha o Nº CVA antes da Janela Gate.");
                gateInput.value = "";
                return;
            }

            // Regra 2: Gate não pode ser menor que data_hora
            if (dataHoraAttr && gateInput?.value) {
                const dtGate = new Date(gateInput.value);
                const dtReferencia = new Date(dataHoraAttr);

                if (dtGate < dtReferencia) {
                    mostrarErro(item, gateInput, "Janela Gate não pode ser menor que a data/hora da coleta.");
                    gateInput.value = "";
                    return;
                }
            }

            // Regras de sequência obrigatória
            if (!chegadaOrigemInput.value && saidaOrigemInput.value) {
                mostrarErro(item, saidaOrigemInput, "Preencha a chegada do fornecedor antes da saída.");
                saidaOrigemInput.value = "";
                return;
            }

            if (!saidaOrigemInput.value && chegadaDestinoInput.value) {
                mostrarErro(item, chegadaDestinoInput, "Preencha a saída do fornecedor antes da chegada na planta.");
                chegadaDestinoInput.value = "";
                return;
            }

            if (!chegadaDestinoInput.value && entradaInput.value) {
                mostrarErro(item, entradaInput, "Preencha a chegada na planta antes da entrada.");
                entradaInput.value = "";
                return;
            }

            if (!entradaInput.value && saidaPlantaInput.value) {
                mostrarErro(item, saidaPlantaInput, "Preencha a entrada na planta antes da saída.");
                saidaPlantaInput.value = "";
                return;
            }

            // Validações de ordem temporal
            if (v1 > agora) {
                mostrarErro(item, chegadaOrigemInput, "Chegada do fornecedor não pode ser no futuro.");
                return;
            }

            if (v2 < v1 || v2 > agora) {
                mostrarErro(item, saidaOrigemInput, "Saída do fornecedor não pode ser antes da chegada nem no futuro.");
                return;
            }

            if (v3 < v2 || v3 > agora) {
                mostrarErro(item, chegadaDestinoInput, "Chegada na planta não pode ser antes da saída do fornecedor nem no futuro.");
                return;
            }

            if (v4 < v3 || v4 > agora) {
                mostrarErro(item, entradaInput, "Entrada na planta não pode ser antes da chegada nem no futuro.");
                return;
            }

            if (v5 < v4 || v5 > agora) {
                mostrarErro(item, saidaPlantaInput, "Saída da planta não pode ser antes da entrada nem no futuro.");
                return;
            }

            // Tudo válido
            limparErros(item);
        }


        function bindEventos() {
            const itens = document.querySelectorAll('.item-coleta');
            itens.forEach(item => {
                const chegada = item.querySelector('.chegada');
                const saida = item.querySelector('.saida');
                const chegadaPlanta = item.querySelector('.chegada-planta');
                const entrada = item.querySelector('.entrada-planta');
                const saidaPlanta = item.querySelector('.saida-planta');
                const gate = item.querySelector('.gate');

                // Eventos de mudança (validação e cálculo)
                chegada?.addEventListener('change', () => {
                    calcularTempoAgCarreg(item);
                    validarDatas(item);
                });

                saida?.addEventListener('change', () => {
                    calcularTempoAgCarreg(item);
                    validarDatas(item);
                });

                chegadaPlanta?.addEventListener('change', () => {
                    calcularTempoEsperaGate(item);
                    validarDatas(item);
                });

                entrada?.addEventListener('change', () => {
                    calcularTempoEsperaGate(item);
                    calcularTempoDentroPlanta(item);
                    validarDatas(item);
                });

                saidaPlanta?.addEventListener('change', () => {
                    calcularTempoDentroPlanta(item);
                    validarDatas(item);
                });

                gate?.addEventListener('change', () => {
                    validarDatas(item);
                });

                // Eventos de correção (ao digitar/apagar valores)
                const inputs = item.querySelectorAll('input');
                inputs.forEach(input => {
                    input.addEventListener('input', () => {
                        if (input.classList.contains('com-erro')) {
                            input.style.border = "";
                            input.classList.remove('com-erro');
                            const erro = item.querySelector('.msg-erro');
                            if (erro) {
                                erro.textContent = '';
                                erro.style.display = 'none';
                            }
                        }
                    });
                });
            });
        }


    window.addEventListener('load', bindEventos);
    </script>
    

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="content-wrapper">
        <div class="container-fluid">
            <br/>
            <div class="card card-info">
                <div class="card-header">
                    <h3 class="card-title"><i class="fas fa-pallet"></i>&nbsp;ORDEM DE COLETA -
                        <asp:Label ID="novaColeta" runat="server"></asp:Label>
                    </h3>
                </div>
            </div>
        </div>
        <div class="card-header">
            <div class="row g-3">
                <div class="col-md-1">
                    <div class="form-group">
                        <span class="details">MOTORISTA:</span>
                        <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label>
                        <asp:TextBox ID="txtCodMotorista" runat="server" Style="text-align: center" class="form-control font-weight-bold" OnTextChanged="txtCodMotorista_TextChanged" AutoPostBack="true"></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-2">
                    <div class="form-group">
                        <span class="details">FILIAL:</span>
                        <asp:TextBox ID="txtFilialMot" runat="server" class="form-control font-weight-bold" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-2">
                    <div class="form-group">
                        <span class="details">TIPO DE MOTORISTA:</span>
                        <asp:TextBox ID="txtTipoMot" runat="server" Style="text-align: center" class="form-control font-weight-bold" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-2">
                    <div class="form-group">
                        <span class="details">FUNÇÃO:</span>
                        <asp:TextBox ID="txtFuncao" runat="server" class="form-control font-weight-bold" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-1">
                    <div class="form-group">
                        <span class="details">EX.TOXIC.:</span>
                        <div class="input-group">
                            <asp:TextBox ID="txtExameToxic" runat="server" class="form-control font-weight-bold" ReadOnly="true" Style="text-align: center"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-md-1">
                    <div class="form-group">
                        <span class="details">VAL. CNH:</span>
                        <div class="input-group">
                            <asp:TextBox ID="txtCNH" runat="server" class="form-control font-weight-bold" ReadOnly="true" Style="text-align: center"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-md-1">
                    <div class="form-group">
                        <span class="details">VAL. GR.:</span>
                        <div class="input-group">
                            <asp:TextBox ID="txtLibGR" runat="server" class="form-control font-weight-bold" ReadOnly="true" Style="text-align: center"></asp:TextBox>
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
                        <asp:TextBox ID="txtNomMot" runat="server" class="form-control font-weight-bold" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-2">
                    <div class="form-group">
                        <span class="details">CPF:</span>
                        <div class="input-group">
                            <asp:TextBox ID="txtCPF" runat="server" class="form-control font-weight-bold" ReadOnly="true" Style="text-align: center"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-md-2">
                    <div class="form-group">
                        <span class="details">CARTÃO PAMCARD:</span>
                        <div class="input-group">
                            <asp:TextBox ID="txtCartao" runat="server" class="form-control font-weight-bold" ReadOnly="true" Style="text-align: center"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-md-1">
                    <div class="form-group">
                        <span class="details">MÊS/ANO:</span>
                        <div class="input-group">
                            <asp:TextBox ID="txtValCartao" runat="server" class="form-control font-weight-bold" ReadOnly="true" Style="text-align: center"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-md-2">
                    <div class="form-group">
                        <span class="details">CELULAR:</span>
                        <div class="input-group">
                            <asp:TextBox ID="txtCelular" runat="server" class="form-control font-weight-bold" ReadOnly="true" Style="text-align: center"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row g-3">
                <div class="col-md-1">
                    <div class="form-group">
                        <span class="details">CÓD./FROTA:</span>
                        <asp:TextBox ID="txtCodVeiculo" runat="server" Style="text-align: center" class="form-control font-weight-bold"></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-1">
                    <br />
                    <asp:Button ID="btnPesquisarVeiculo" runat="server" Text="Pesquisar" CssClass="btn btn-outline-warning" OnClick="btnPesquisarVeiculo_Click" />
                </div>
                <div class="col-md-2">
                    <div class="form-group">
                        <span class="details">FILIAL:</span>
                        <asp:TextBox ID="txtFilialVeicCNT" runat="server" Style="text-align: center" class="form-control font-weight-bold" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-2">
                    <div class="form-group">
                        <span class="details">TIPO DE VEÍCULO:</span>
                        <asp:TextBox ID="txtVeiculoTipo" runat="server" Style="text-align: center" class="form-control font-weight-bold" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-1"></div>
                <div class="col-md-1">
                    <div class="form-group">
                        <span class="details">OPACIDADE:</span>
                        <div class="input-group">
                            <asp:TextBox ID="txtOpacidade" runat="server" class="form-control font-weight-bold" ReadOnly="true" Style="text-align: center"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-md-1">
                    <div class="form-group">
                        <span class="details">CET:</span>
                        <div class="input-group">
                            <asp:TextBox ID="txtCET" runat="server" class="form-control font-weight-bold" ReadOnly="true" Style="text-align: center"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-md-1">
                    <div class="form-group">
                        <span class="details">CRLV:</span>
                        <asp:TextBox ID="txtCRLVVeiculo" runat="server" class="form-control font-weight-bold" ReadOnly="true" Style="text-align: center"></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-1">
                    <div class="form-group">
                        <span class="details">CRLV REB1:</span>
                        <div class="input-group">
                            <asp:TextBox ID="txtCRLVReb1" runat="server" class="form-control font-weight-bold" ReadOnly="true" Style="text-align: center"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-md-1">
                    <div class="form-group">
                        <span class="details">CRLV REB2:</span>
                        <div class="input-group">
                            <asp:TextBox ID="txtCRLVReb2" runat="server" class="form-control font-weight-bold" ReadOnly="true" Style="text-align: center"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row g-3">
                <div class="col-md-1">
                    <div class="form-group">
                        <span class="details">PLACA:</span>
                        <asp:TextBox ID="txtPlaca" runat="server" class="form-control font-weight-bold" ReadOnly="true" MaxLength="8"></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-2">
                    <div class="form-group">
                        <span class="details">VEICULO:</span>
                        <asp:TextBox ID="txtTipoVeiculo" runat="server" class="form-control font-weight-bold" ReadOnly="true" placeholder=""></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-1">
                    <div class="form-group">
                        <span class="details">REBOQUE:</span>
                        <asp:TextBox ID="txtReboque1" runat="server" class="form-control font-weight-bold" ReadOnly="true" MaxLength="8"></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-1">
                    <div class="form-group">
                        <span class="details">REBOQUE:</span>
                        <asp:TextBox ID="txtReboque2" runat="server" class="form-control font-weight-bold" ReadOnly="true" MaxLength="8"></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-1">
                    <div class="form-group">
                        <span class="details">CARRETA(S):</span>
                        <asp:TextBox ID="txtCarreta" runat="server" class="form-control font-weight-bold" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-1">
                    <div class="form-group">
                        <span class="details">TECNOLOGIA:</span>
                        <asp:TextBox ID="txtTecnologia" runat="server" class="form-control font-weight-bold" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-1">
                    <div class="form-group">
                        <span class="details">RASTREAMENTO:</span>
                        <asp:TextBox ID="txtRastreamento" runat="server" class="form-control font-weight-bold" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <span class="details">CONJUNTO:</span>
                        <asp:TextBox ID="txtConjunto" runat="server" class="form-control font-weight-bold" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>

            </div>
            <div class="row g-3">
                <div class="col-md-1">
                    <div class="form-group">
                        <span class="details">CÓDIGO:</span>
                        <asp:TextBox ID="txtCodProprietario" runat="server" class="form-control font-weight-bold" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-7">
                    <div class="form-group">
                        <span class="details">PROPRIETÁRIO:</span>
                        <asp:TextBox ID="txtProprietario" runat="server" class="form-control font-weight-bold" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-1">
                    <div class="form-group">
                        <span class="details">CONTATO:</span>
                        <asp:TextBox ID="txtCodFrota" runat="server" class="form-control font-weight-bold"></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-2">
                    <div class="form-group">
                        <span class="details">FONE CORPORATIVO:</span>
                        <asp:TextBox ID="txtFoneCorp" runat="server" class="form-control font-weight-bold" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-1">
                    <br />
                    <asp:Button ID="btnPesquisarContato" runat="server" Text="Pesquisar" CssClass="btn btn-outline-warning" OnClick="btnPesquisarContato_Click" />
                </div>
            </div>

            <div class="row g-3">
                <div class="col-md-12">
                    <div class="card">
                        <!-- ./card-header -->
                        <div class="card-body">

                            <asp:Repeater ID="rptColetas" runat="server" OnItemDataBound="rptColetas_ItemDataBound" OnItemCommand="rptColetas_ItemCommand">
                                <HeaderTemplate>
                                    <table id="gridCargas" class="table table-bordered table-hover">
                                        <thead>
                                            <tr>
                                                <th>COLETA</th>
                                                <th>CVA</th>
                                                <th>DATA COLETA</th>
                                                <%--<th>CODIGO</th>--%>
                                                <th>LOCAL DA COLETA</th>
                                                <%--<th>CODIGO</th>--%>
                                                <th>LOCAL DA ENTREGA</th>
                                                <th>STATUS</th>
                                                <th>ATENDIMENTO</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr data-widget="expandable-table" aria-expanded="false">
                                        <td><%# Eval("carga") %></td>
                                        <td><%# Eval("cva") %></td>
                                       <td class="data-hora" data-datahora='<%# Eval("data_hora", "{0:yyyy-MM-ddTHH:mm}") %>'>  <%# Eval("data_hora", "{0:dd/MM/yyyy HH:mm}") %></td>
                                        <%--<td><%# Eval("CodigoO") %></td>--%>
                                        <td><%# Eval("cliorigem") %></td>
                                        <%--<td><%# Eval("CodigoD") %></td>--%>
                                        <td><%# Eval("clidestino") %></td>
                                        <td><%# Eval("status") %></td>
                                        <%--<td><%# Eval("atendimento") %></td>--%>
                                        <td runat="server" id="tdAtendimento">
                                            <asp:Label ID="lblAtendimento" runat="server" />
                                        </td>
                                    </tr>
                                    <tr class="expandable-body">
                                        <td colspan="12">
                                            <div class="card card-warning">
                                                <div class="card-header">
                                                    <h3 class="card-title">Dados da Coleta</h3>
                                                    <div class="card-tools">
                                                        <button type="button" class="btn btn-tool" data-card-widget="collapse">
                                                            <i class="fas fa-minus"></i>
                                                        </button>
                                                        <%--<button type="button" class="btn btn-tool" data-card-widget="remove">
                <i class="fas fa-times"></i>
            </button>--%>
                                                    </div>
                                                </div>

                                                <div class="card-body">
                                                    <div class="row g-3">
                                                        <div class="col-md-3">
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
                                                        <div class="col-md-5">
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
                                                </div>
                                                <!-- /.card-body -->
                                            </div>

                                            <div class="card card-success">
                                                <div class="card-header">
                                                    <h3 class="card-title">Atendimento da Coleta</h3>
                                                    <div class="card-tools">
                                                        <button type="button" class="btn btn-tool" data-card-widget="collapse">
                                                            <i class="fas fa-minus"></i>
                                                        </button>
                                                        <%--<button type="button" class="btn btn-tool" data-card-widget="remove">
                                                            <i class="fas fa-times"></i>
                                                        </button>--%>
                                                    </div>
                                                </div>
                                                <div class="card-body">
                                                  <div class="item-coleta">
                                                    <div class="row g-3">
                                                        <div class="col-md-2">
                                                            <div class="form-group">
                                                                <span class="details">Nº CVA:<asp:Label ID="lblMensagem" runat="server" Text=""></asp:Label></span>
                                                                <div class="input-group">
                                                                   <asp:TextBox ID="txtCVA" runat="server" CssClass="form-control cva" MaxLength="11"  Text='<%# Bind("cva") %>' Style="text-align: center"></asp:TextBox>
                                                                   

                                                                </div>
                                                                 <span class="msg-erro text-danger" style="display: none;"></span>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-2">
                                                            <div class="form-group">
                                                                <span class="details">JANELA GATE:</span>
                                                                <div class="input-group">
                                                                    <asp:TextBox ID="txtGate" runat="server" TextMode="DateTimeLocal"  Text='<%# Eval("gate","{0:yyyy-MM-ddTHH:mm}") %>' CssClass="form-control gate"  Style="text-align: center"></asp:TextBox>

                                                                </div>
                                                                <span class="msg-erro text-danger" style="display: none;"></span>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-2">
                                                            <div class="form-group">
                                                                <span class="">STATUS:</span>
                                                                <asp:HiddenField ID="hdfStatus" Value='<%# Eval("status") %>' runat="server" />
                                                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                   
                                                        <div class="row g-3">
                                                            <div class="col-md-2">
                                                                <div class="form-group">
                                                                    <span class="details">CHEGADA FORNECEDOR:</span>
                                                                    <div class="input-group">
                                                                        <asp:TextBox ID="txtChegadaOrigem" runat="server"
                                                                            CssClass="form-control chegada"
                                                                            Text='<%# Eval("chegadaorigem", "{0:yyyy-MM-ddTHH:mm}") %>'
                                                                            TextMode="DateTimeLocal"
                                                                            Style="text-align: center" onChange="validarDatas(item)"  />
                                                                    </div>
                                                                     <span class="msg-erro text-danger" style="display: none;"></span>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-2">
                                                                <div class="form-group">
                                                                    <span class="details">SAÍDA FORNECEDOR:</span>
                                                                    <div class="input-group">
                                                                        <asp:TextBox ID="txtSaidaOrigem" runat="server"
                                                                            CssClass="form-control saida"
                                                                            Text='<%# Eval("saidaorigem", "{0:yyyy-MM-ddTHH:mm}") %>'
                                                                            TextMode="DateTimeLocal"
                                                                            Style="text-align: center" onChange="validarDatas(item)"  />
                                                                    </div>
                                                                     <span class="msg-erro text-danger" style="display: none;"></span>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-1">
                                                                <div class="form-group">
                                                                    <span class="details">ESPERA:</span>
                                                                    <div class="input-group">
                                                                        <asp:TextBox ID="txtAgCarreg" runat="server"
                                                                            CssClass="form-control espera"
                                                                            Text='<%# Eval("tempoagcarreg") %>'
                                                                            Style="text-align: center" />
                                                                    </div>
                                                                     <span class="msg-erro text-danger" style="display: none;"></span>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="row g-3">
                                                            <div class="col-md-2">
                                                                <div class="form-group">
                                                                    <span class="details">CHEGADA PLANTA:</span>
                                                                    <div class="input-group">
                                                                        <asp:TextBox ID="txtChegadaDestino" runat="server"
                                                                            Text='<%# Eval("chegadadestino", "{0:yyyy-MM-ddTHH:mm}") %>'
                                                                            CssClass="form-control chegada-planta"
                                                                            TextMode="DateTimeLocal"
                                                                            Style="text-align: center" onChange="validarDatas(item)"  />

                                                                    </div>
                                                                     <span class="msg-erro text-danger" style="display: none;"></span>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-2">
                                                                <div class="form-group">
                                                                    <span class="details">ENTRADA:</span>
                                                                    <div class="input-group">
                                                                        <asp:TextBox ID="txtEntrada" runat="server"
                                                                            Text='<%# Eval("entradaplanta", "{0:yyyy-MM-ddTHH:mm}") %>'
                                                                            CssClass="form-control entrada-planta"
                                                                            TextMode="DateTimeLocal"
                                                                            Style="text-align: center" onChange="validarDatas(item)"  />

                                                                    </div>
                                                                     <span class="msg-erro text-danger" style="display: none;"></span>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-1">
                                                                <div class="form-group">
                                                                    <span class="details">ESP.GATE:</span>
                                                                    <div class="input-group">
                                                                        <asp:TextBox ID="txtEsperaGate" runat="server" Text='<%# Eval("tempoesperagate") %>'
                                                                            CssClass="form-control espera-gate"
                                                                            Style="text-align: center" />

                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-2">
                                                                <div class="form-group">
                                                                    <span class="details">SAIDA PLANTA:</span>
                                                                    <div class="input-group">
                                                                        <asp:TextBox ID="txtSaidaPlanta" runat="server"
                                                                            Text='<%# Eval("saidaplanta", "{0:yyyy-MM-ddTHH:mm}") %>'
                                                                            CssClass="form-control saida-planta"
                                                                            TextMode="DateTimeLocal"
                                                                            Style="text-align: center" onChange="validarDatas(item)"  />

                                                                    </div>
                                                                     <span class="msg-erro text-danger" style="display: none;"></span>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-1">
                                                                <div class="form-group">
                                                                    <span class="details">TEMPO:</span>
                                                                    <div class="input-group">
                                                                        <asp:TextBox ID="txtDentroPlanta" runat="server"
                                                                            Text='<%# Eval("tempodentroplanta") %>'
                                                                            CssClass="form-control dentro-planta"
                                                                            Style="text-align: center" />

                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-1">
                                                                <br />
                                                                <asp:Button ID="btnAtualizarColeta" runat="server" Text="Atualizar" CssClass="btn btn-outline-info" CommandName="Atualizar" CommandArgument='<%# Eval("carga") %>' />
                                                            </div>
                                                            <div class="col-md-1">
                                                                <br />
                                                                <asp:Button ID="WhatsApp" runat="server" Text="WhatsApp" CssClass="btn btn-outline-success" CommandName="Atualizar" CommandArgument='<%# Eval("carga") %>' />
                                                            </div>

                                                            <div class="col-md-1">
                                                                <br />
                                                                <asp:Button ID="btnAbrirModal" runat="server" Text="Ocorrência" CommandName="Ocorrencias" CommandArgument='<%# Eval("carga") %>' CssClass="btn btn-outline-danger" />

                                                            </div>
                                                              <div class="col-md-1">
                                                                  <br />
                                                                  <asp:Button ID="btnOrdemColeta" runat="server" Text="Impr. O.C." CommandName="Coletas" CommandArgument='<%# Eval("carga") %>' CssClass="btn btn-outline-warning" />

                                                              </div>


                                                        </div>
                                                    </div>
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
                    <asp:Button ID="btnSalvar1" CssClass="btn btn-outline-success  btn-lg" runat="server" OnClick="btnSalvar1_Click" Text="Atualizar Ordem de Coleta" />
                </div>
                <div class="col-md-1"></div>
                <div class="col-md-2">
                    <br />
                    <asp:Button ID="btnImprimir" CssClass="btn btn-outline-warning  btn-lg" runat="server" Text="Imprimir Ordem de Coleta" OnClick="btnImprimir_Click" />
                </div>
                <div class="col-md-1"></div>
                <div class="col-md-1">
                    <br />
                    <a href="ConsultaEntregas.aspx" class="btn btn-outline-danger btn-lg">Sair               
                    </a>
                </div>
            </div>
        </div>



    </div>
    <!-- Modal Bootstrap Cadastro de Telefone -->
    <div class="modal fade" id="telefoneModal" tabindex="-1" role="dialog" aria-labelledby="telefoneModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered" role="document">
            <div class="modal-content">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div class="modal-header">
                            <h5 class="modal-title" id="telefoneModalLabel">Cadastrar Contato</h5>
                            <button type="button" class="close" data-dismiss="modal" aria-label="Fechar">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div class="modal-body">
                            <div class="row g-3">
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <span class="details">CÓDIGO:</span>
                                        <asp:TextBox ID="txtCodContato" runat="server" class="form-control font-weight-bold"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-8">
                                    <div class="form-group">
                                        <span class="details">CELULAR:</span>
                                        <div class="input-group">
                                            <asp:TextBox ID="txtCadCelular" runat="server" class="form-control font-weight-bold"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Fechar</button>
                            <asp:Button ID="btnCadContato" runat="server" Text="Salvar" class="btn btn-primary" OnClick="btnCadContato_Click" />
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnCadContato" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>


            </div>
        </div>
    </div>


    <!-- Modal -->
    <div class="modal fade bd-example-modal-xl" id="exampleModalCenter" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered modal-xl" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalCenterTitle">Ocorrências</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="row g-3">
                        <div class="col-md-2">
                            <div class="form-group">
                                <asp:Label ID="lblColeta" runat="server" class="form-control font-weight-bold" Style="text-align: center">  
                                </asp:Label>
                            </div>
                        </div>
                        <div class="col-md-6">
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <asp:Label ID="lblStatus" runat="server" class="form-control font-weight-bold" Style="text-align: center">  
                                </asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="row g-3">
                        <div class="col-md-5">
                            <div class="form_group">
                                <span class="details">RESPONSÁVEL:</span>
                                <asp:DropDownList ID="cboResponsavel" runat="server" CssClass="form-control">
                                </asp:DropDownList><br />
                            </div>
                        </div>
                        <div class="col-md-1"></div>
                        <div class="col-md-6">
                            <div class="form_group">
                                <span class="details">OCORRÊNCIA:</span>
                                <asp:DropDownList ID="cboMotivo" runat="server" CssClass="form-control">
                                </asp:DropDownList><br />
                            </div>
                        </div>
                    </div>
                    <div class="row g-3">
                        <div class="col-md-12">
                            <div class="form_group">
                                <span class="details">OBSERVAÇÃO:</span>
                                <asp:TextBox ID="txtObservacao" runat="server" class="form-control font-weight-bold" Rows="3" TextMode="MultiLine" placeholder="Ocorrências ..."></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <br />
                    <div class="row g-3">
                        <div class="col-md-12">
                            <!-- /.card-header -->
                            <div class="card-body table-responsive p-0" style="height: 200px;">
                                <table class="table table-head-fixed text-nowrap">
                                    <asp:GridView runat="server" ID="GridViewCarga" CssClass="table table-bordered table-striped table-hover" Width="100%" AutoGenerateColumns="False">
                                        <Columns>
                                            <asp:BoundField DataField="id" HeaderText="#ID" Visible="false" />
                                            <asp:BoundField DataField="responsavel" HeaderText="RESPONSÁVEL" />
                                            <asp:BoundField DataField="motivo" HeaderText="OCORRÊNCIA" />
                                            <asp:BoundField DataField="observacao" HeaderText="OBSERVAÇÃO" />
                                            <asp:BoundField DataField="data_inclusao" HeaderText="DATA   " />
                                            <asp:BoundField DataField="usuario_inclusao" HeaderText="USUÁRIO" />

                                            <asp:TemplateField HeaderText="AÇÕES" ShowHeader="True">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkExcluir" runat="server" class="btn btn-danger"><i class="fas fa-trash-alt"></i></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </table>

                            </div>
                            <!-- /.card-body -->
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-dismiss="modal">Fechar</button>
                        <asp:Button ID="btnSalvarOcorrencia" runat="server" Text="Salvar" OnClick="btnSalvarOcorrencia_Click" class="btn btn-primary" />

                    </div>
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

</asp:Content>

