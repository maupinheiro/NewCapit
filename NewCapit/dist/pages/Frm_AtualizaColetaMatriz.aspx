<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="Frm_AtualizaColetaMatriz.aspx.cs" Inherits="NewCapit.dist.pages.Frm_AtualizaColetaMatriz" %>
<%@ Register Assembly="GMaps" Namespace="Subgurim.Controles" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- Bootstrap CSS -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/css/bootstrap.min.css" rel="stylesheet" />
    <!-- jQuery e Bootstrap JS -->
    <script src="https://code.jquery.com/jquery-3.6.4.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/js/bootstrap.bundle.min.js"></script>
    <!-- Bootstrap CSS + JS -->
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" />
    <script src="https://code.jquery.com/jquery-3.5.1.slim.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@4.5.2/dist/js/bootstrap.bundle.min.js"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">






    <!-- Script para fechar modal -->
    <script type="text/javascript">
        function fecharModalOcorrencia() {
            $('#modalOcorrencia').modal('hide');
        }
    </script>
    <style>
        .fonte-menor {
            font-size: 10px;
        }
    </style>
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
        // Função para converter o formato "Xh Ymin" para minutos totais.
        function converterTempoParaMinutos(tempoStr) {
            if (!tempoStr || typeof tempoStr !== 'string' || tempoStr.includes("Inválido")) {
                return 0;
            }
            const partesHoras = tempoStr.split('h');
            const horas = parseInt(partesHoras[0], 10) || 0;
            const partesMinutos = (partesHoras[1] || '').split('min');
            const minutos = parseInt(partesMinutos[0], 10) || 0;
            return (horas * 60) + minutos;
        }

        // NOVA FUNÇÃO: Atualiza o status e o estilo dos campos de tempo.
        function atualizarStatusECores(item) {
            const ddlStatus = item.querySelector('.ddlStatus');
            if (!ddlStatus) return;

            const txtChegadaOrigem = item.querySelector('.chegada');
            const txtSaidaOrigem = item.querySelector('.saida');
            const txtChegadaDestino = item.querySelector('.chegada-planta');
            const txtSaidaPlanta = item.querySelector('.saida-planta');

            const txtAgCarreg = item.querySelector('.espera');
            const txtEsperaGate = item.querySelector('.espera-gate');
            const txtDentroPlanta = item.querySelector('.dentro-planta');

            // 1. Atualiza o Status baseado no preenchimento dos campos
            if (txtSaidaPlanta && txtSaidaPlanta.value) {
                ddlStatus.value = 'Concluido';
            } else if (txtChegadaDestino && txtChegadaDestino.value) {
                ddlStatus.value = 'Ag. Descarga';
            } else if (txtSaidaOrigem && txtSaidaOrigem.value) {
                ddlStatus.value = 'Em Transito';
            } else if (txtChegadaOrigem && txtChegadaOrigem.value) {
                ddlStatus.value = 'Ag. Carreg.';
            }

            // 2. Verifica o tempo e aplica o estilo (fundo vermelho, letra branca)
            // Limite de 90 minutos (1h 30min)
            const limiteMinutos = 90;

            // Verifica txtAgCarreg
            if (txtAgCarreg) {
                const tempoAgCarreg = converterTempoParaMinutos(txtAgCarreg.value);
                if (tempoAgCarreg > limiteMinutos) {
                    txtAgCarreg.style.backgroundColor = 'red';
                    txtAgCarreg.style.color = 'white';
                } else {
                    txtAgCarreg.style.backgroundColor = '';
                    txtAgCarreg.style.color = '';
                }
            }

            // Verifica txtEsperaGate
            if (txtEsperaGate) {
                const tempoEsperaGate = converterTempoParaMinutos(txtEsperaGate.value);
                if (tempoEsperaGate > limiteMinutos) {
                    txtEsperaGate.style.backgroundColor = 'red';
                    txtEsperaGate.style.color = 'white';
                } else {
                    txtEsperaGate.style.backgroundColor = '';
                    txtEsperaGate.style.color = '';
                }
            }

            // Verifica txtDentroPlanta
            if (txtDentroPlanta) {
                const tempoDentroPlanta = converterTempoParaMinutos(txtDentroPlanta.value);
                if (tempoDentroPlanta > limiteMinutos) {
                    txtDentroPlanta.style.backgroundColor = 'red';
                    txtDentroPlanta.style.color = 'white';
                } else {
                    txtDentroPlanta.style.backgroundColor = '';
                    txtDentroPlanta.style.color = '';
                }
            }
        }

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
            // Chama a função de atualização após o cálculo
            atualizarStatusECores(item);
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
            // Chama a função de atualização após o cálculo
            atualizarStatusECores(item);
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
            // Chama a função de atualização após o cálculo
            atualizarStatusECores(item);
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

            const v1 = new Date(chegadaOrigemInput.value);
            const v2 = new Date(saidaOrigemInput.value);
            const v3 = new Date(chegadaDestinoInput.value);
            const v4 = new Date(entradaInput.value);
            const v5 = new Date(saidaPlantaInput.value);

            if (cvaInput && gateInput && gateInput.value && !cvaInput.value.trim()) {
                mostrarErro(item, gateInput, "Preencha o Nº CVA antes da Janela Gate.");
                gateInput.value = "";
                return;
            }

            if (dataHoraAttr && gateInput?.value) {
                const dtGate = new Date(gateInput.value);
                const dtReferencia = new Date(dataHoraAttr);

                if (dtGate < dtReferencia) {
                    mostrarErro(item, gateInput, "Janela Gate não pode ser menor que a data/hora da coleta.");
                    gateInput.value = "";
                    return;
                }
            }

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

            limparErros(item);
            // Chama a função de atualização após a validação bem-sucedida
            atualizarStatusECores(item);
        }

        function bindEventos() {
            const itens = document.querySelectorAll('.item-coleta');
            itens.forEach(item => {
                // MODIFICAÇÃO: Adiciona a classe 'ddlStatus' ao DropDownList para facilitar a seleção.
                const ddl = item.querySelector('select[id*="ddlStatus"]');
                if (ddl) {
                    ddl.classList.add('ddlStatus');
                }

                const inputsDeData = item.querySelectorAll('.chegada, .saida, .chegada-planta, .entrada-planta, .saida-planta, .gate');

                inputsDeData.forEach(input => {
                    input.addEventListener('change', () => {
                        // Centraliza as chamadas de função
                        calcularTempoAgCarreg(item);
                        calcularTempoEsperaGate(item);
                        calcularTempoDentroPlanta(item);
                        validarDatas(item); // validarDatas já chama a atualização no final
                    });
                });

                const inputsGerais = item.querySelectorAll('input');
                inputsGerais.forEach(input => {
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

                // Executa a verificação inicial quando a página carrega
                atualizarStatusECores(item);
            });
        }

        window.addEventListener('load', bindEventos);
    </script>
    <script>
        $(document).ready(function () {
            // Escuta mudança em qualquer txtCVA dentro do Repeater
            $(document).on('blur', '.cva', function () {
                var txt = $(this);
                var valor = txt.val().trim();

                if (valor.length > 0) {
                    $.ajax({
                        type: "POST",
                        url: '<%= ResolveUrl("Frm_AtualizaOrdemColeta.aspx/VerificarCVA") %>', // ajuste para o nome da sua página
                        data: JSON.stringify({ numeroCVA: valor }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            if (response.d === true) {
                                alert("Já existe uma carga com esse número de CVA!");
                                txt.val(""); // limpa o campo
                                txt.focus();
                            }
                        },
                        error: function (err) {
                            console.error("Erro na verificação do CVA", err);
                        }
                    });
                }
            });
        });
    </script>

    <div class="content-wrapper">
        <section class="content">
            <div class="container-fluid">
                <br />
                <div class="col-md-12">
                    <div class="card card-info">
                        <div class="card-header">
                            <h3 class="card-title">
                                <h3 class="card-title"><i class="fas fa-shipping-fast"></i>&nbsp;ORDEM DE COLETA/ENTREGA - &nbsp;<asp:Label ID="novaColeta" runat="server"></asp:Label></h3>
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
                            <!-- linha 1 -->
                            <div class="row g-3">
                                <div class="col-md-1">
                                    <div class="form-group">
                                        <img src="<%=fotoMotorista%>"  class="rounded-circle float-center" height="80" width="80" alt="User Image">
                                    </div>
                                </div>
                                <div class="col-md-1">
                                    <div class="form-group">
                                        <span class="details">MOTORISTA:</span>
                                        <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label>
                                        <asp:TextBox ID="txtCodMotorista" runat="server" Style="text-align: center" class="form-control font-weight-bold" OnTextChanged="txtCodMotorista_TextChanged" AutoPostBack="true"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div class="form-group">
                                        <span class="details">NOME COMPLETO:</span>
                                        <asp:DropDownList ID="ddlMotorista" runat="server" class="form-control font-weight-bold select2" OnSelectedIndexChanged="ddlMotorista_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                    </div>
                                    <asp:HiddenField ID="txtconformmessageValue" runat="server" />
                                </div>
                                <div class="col-md-1">
                                    <div class="form-group">
                                        <span class="details">CONTATO:</span>
                                        <asp:TextBox ID="txtCodFrota" runat="server" class="form-control font-weight-bold" AutoPostBack="true" OnTextChanged="btnPesquisarContato_Click"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-2">
                                    <div class="form-group">
                                        <span class="details">FONE CORPORATIVO:</span>
                                        <asp:TextBox ID="txtFoneCorp" runat="server" class="form-control font-weight-bold" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-1">
                                    <div class="form-group">
                                        <span class="details">CÓD./FROTA:</span>
                                        <asp:TextBox ID="txtCodVeiculo" runat="server" Style="text-align: center" ReadOnly="true" class="form-control font-weight-bold" AutoPostBack="true" OnTextChanged="btnPesquisarVeiculo_Click"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-1">
                                    <div class="form-group">
                                        <span class="details">PLACA:</span>
                                        <asp:TextBox ID="txtPlaca" runat="server" class="form-control font-weight-bold" ReadOnly="true" MaxLength="8"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-1" id="reboque1" runat="server">
                                    <div class="form-group">
                                        <span class="details">REBOQUE:</span>
                                        <asp:TextBox ID="txtReboque1" runat="server" class="form-control font-weight-bold" ReadOnly="true" MaxLength="8"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-1" id="reboque2" runat="server" visible="false">
                                    <div class="form-group">
                                        <span class="details">REBOQUE:</span>
                                        <asp:TextBox ID="txtReboque2" runat="server" class="form-control font-weight-bold" ReadOnly="true" MaxLength="8"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <!-- dados do motorista -->
                            <div class="card card-outline card-info collapsed-card">
                                <div class="card-header">
                                    <h3 class="card-title"><i class="far fa-edit"></i>&nbsp;Dados do Motorista</h3>
                                    <div class="card-tools">
                                        <button type="button" class="btn btn-tool" data-card-widget="collapse">
                                            <i class="fas fa-plus"></i>
                                        </button>
                                    </div>
                                    <!-- /.card-tools -->
                                </div>
                                <!-- /.card-header -->
                                <div class="card-body">
                                    <div class="row g-3">
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <span class="details">FILIAL:</span>
                                                <asp:TextBox ID="txtFilialMot" runat="server" class="form-control font-weight-bold" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <span class="details">TIPO DE MOTORISTA:</span>
                                                <asp:TextBox ID="txtTipoMot" runat="server" class="form-control font-weight-bold" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <span class="details">FUNÇÃO:</span>
                                                <asp:TextBox ID="txtFuncao" runat="server" class="form-control font-weight-bold" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-2" id="ETI" runat="server">
                                            <div class="form-group">
                                                <span class="details">VALIDADE E.T.I.:</span>
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtExameToxic" runat="server" class="form-control font-weight-bold" ReadOnly="true" Style="text-align: center"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <span class="details">VALIDADE CNH:</span>
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtCNH" runat="server" class="form-control font-weight-bold" ReadOnly="true" Style="text-align: center"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <span class="details">VALIDADE GR.:</span>
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtLibGR" runat="server" class="form-control font-weight-bold" ReadOnly="true" Style="text-align: center"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                    <div class="row g-3">
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <span class="details">CELULAR PARTICULAR:</span>
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtCelular" runat="server" class="form-control font-weight-bold" ReadOnly="true" Style="text-align: center"></asp:TextBox>
                                                </div>
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

                                        <div class="col-md-1">
                                            <div class="form-group">
                                                <span class="details">CÓDIGO:</span>
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtCodTransportadora" runat="server" class="form-control font-weight-bold" ReadOnly="true" Style="text-align: center"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <span class="details">TRANSPORTADORA:</span>
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtTransportadora" runat="server" class="form-control font-weight-bold" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <span class="details">CAFÉ:</span>
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtCafe" runat="server" class="form-control font-weight-bold" ReadOnly="true" Style="text-align: center"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <span class="details">ALMOÇO:</span>
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtAlmoco" runat="server" class="form-control font-weight-bold" ReadOnly="true" Style="text-align: center"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <span class="details">JANTA:</span>
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtJanta" runat="server" class="form-control font-weight-bold" ReadOnly="true" Style="text-align: center"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <span class="details">PERNOITE:</span>
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtPernoite" runat="server" class="form-control font-weight-bold" ReadOnly="true" Style="text-align: center"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <span class="details">COMISSÃO:</span>
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtComissao" runat="server" class="form-control font-weight-bold" ReadOnly="true" Style="text-align: center"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <span class="details">DESENGATE:</span>
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtDesengate" runat="server" class="form-control font-weight-bold" ReadOnly="true" Style="text-align: center"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                </div>
                                <!-- /.card-body -->
                            </div>
                            <!-- dados do veiculo -->
                            <div class="card card-outline card-info collapsed-card">
                                <div class="card-header">
                                    <h3 class="card-title"><i class="far fa-edit"></i>&nbsp;Dados do Veículo</h3>
                                    <div class="card-tools">
                                        <button type="button" class="btn btn-tool" data-card-widget="collapse">
                                            <i class="fas fa-plus"></i>
                                        </button>
                                    </div>
                                    <!-- /.card-tools -->
                                </div>
                                <!-- /.card-header -->
                                <div class="card-body">
                                    <div class="row g-3">
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
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <span class="details">VEICULO:</span>
                                                <asp:TextBox ID="txtTipoVeiculo" runat="server" class="form-control font-weight-bold" ReadOnly="true" placeholder=""></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-2" id="carretas" runat="server">
                                            <div class="form-group">
                                                <span class="details">CARRETA(S):</span>
                                                <asp:TextBox ID="txtCarreta" runat="server" class="form-control font-weight-bold" ReadOnly="true"></asp:TextBox>
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
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <span class="details">VALIDADE OPACIDADE:</span>
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtOpacidade" runat="server" class="form-control font-weight-bold" ReadOnly="true" Style="text-align: center"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <span class="details">VALIDADE LIC. CET:</span>
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtCET" runat="server" class="form-control font-weight-bold" ReadOnly="true" Style="text-align: center"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <span class="details">LICENCIMAENTO:</span>
                                                <asp:TextBox ID="txtCRLVVeiculo" runat="server" class="form-control font-weight-bold" ReadOnly="true" Style="text-align: center"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-2" id="reb1" runat="server" visible="false">
                                            <div class="form-group">
                                                <span class="details">VALIDADE REBOQUE:</span>
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtCRLVReb1" runat="server" class="form-control font-weight-bold" ReadOnly="true" Style="text-align: center"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-2" id="reb2" runat="server" visible="false">
                                            <div class="form-group">
                                                <span class="details">VALIDADE REBOQUE:</span>
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtCRLVReb2" runat="server" class="form-control font-weight-bold" ReadOnly="true" Style="text-align: center"></asp:TextBox>
                                                </div>
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
                                        <div class="col-md-5">
                                            <div class="form-group">
                                                <span class="details">PROPRIETÁRIO:</span>
                                                <asp:TextBox ID="txtProprietario" runat="server" class="form-control font-weight-bold" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-1"></div>
                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <span class="details">TECNOLOGIA:</span>
                                                <asp:TextBox ID="txtTecnologia" runat="server" class="form-control font-weight-bold" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <span class="details">RASTREAMENTO:</span>
                                                <asp:TextBox ID="txtRastreamento" runat="server" class="form-control font-weight-bold" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <!-- /.card-body -->
                            </div>
                            <div class="card card-outline card-info collapsed-card">
                                <div class="card-header">
                                    <h3 class="card-title"><i class="fas fa-map-marker-alt"></i>&nbsp;Rastreamento do Veículo</h3>
                                    <div class="card-tools">
                                        <button type="button" class="btn btn-tool" data-card-widget="collapse">
                                            <i class="fas fa-plus"></i>
                                        </button>

                                    </div>
                                    <!-- /.card-tools -->
                                </div>
                                <!-- /.card-header -->
                                <div class="card-body">
                                        <asp:UpdatePanel ID="updMapa" runat="server">
                                            <ContentTemplate>
                                                <asp:Timer ID="tmAtualizaMapa" runat="server" Interval="60000" OnTick="tmAtualizaMapa_Tick" />
                                                <cc1:GMap ID="GMap1" runat="server" Width="100%" Height="570px" Key="AIzaSyApI6da0E4OJktNZ-zZHgL6A5jtk0L6Cww" enableServerEvents="True" />
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="tmAtualizaMapa" EventName="Tick" />
                                            </Triggers>
                                        </asp:UpdatePanel>

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
                                                                <th>CARGA</th>
                                                                <th>REMETENTE/EXPEDIDOR</th>
                                                                <th>DESTINATÁRIO/RECEBEDOR</th>                                                                
                                                                <th>INICIO VIAGEM</th>
                                                                <th>PREVISÃO CHEG.</th>
                                                                <th>CHEGADA</th>
                                                                <th>FIM DE VIAGEM</th>
                                                                <th>STATUS</th>
                                                                <th>ATENDIMENTO</th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <tr data-widget="expandable-table" aria-expanded="false">
                                                        <td><%# Eval("carga") %></td>
                                                        <td><%# Eval("cliorigem") %></td>
                                                       <td><%# Eval("clidestino") %></td>
                                                        <td class="data-hora" data-datahora='<%# Eval("data_hora", "{0:yyyy-MM-ddTHH:mm}") %>'><%# Eval("data_hora", "{0:dd/MM/yyyy HH:mm}") %></td>
                                                         <td class="data-hora" data-datahora='<%# Eval("data_hora", "{0:yyyy-MM-ddTHH:mm}") %>'><%# Eval("data_hora", "{0:dd/MM/yyyy HH:mm}") %></td>
                                                         <td class="data-hora" data-datahora='<%# Eval("data_hora", "{0:yyyy-MM-ddTHH:mm}") %>'><%# Eval("data_hora", "{0:dd/MM/yyyy HH:mm}") %></td>
                                                         <td class="data-hora" data-datahora='<%# Eval("data_hora", "{0:yyyy-MM-ddTHH:mm}") %>'><%# Eval("data_hora", "{0:dd/MM/yyyy HH:mm}") %></td>
                                                        <td><%# Eval("status") %></td>
                                                        <%--<td><%# Eval("atendimento") %></td>--%>
                                                        <td runat="server" id="tdAtendimento">
                                                            <asp:Label ID="lblAtendimento" runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr class="expandable-body">
                                                        <td colspan="12">
                                                            <div class="card card-outline card-info collapsed-card">
                                                                <div class="card-header">
                                                                    <h3 class="card-title"><i class="far fa-edit"></i>&nbsp;Dados da Coleta</h3>
                                                                    <div class="card-tools">
                                                                        <button type="button" class="btn btn-tool" data-card-widget="collapse">
                                                                            <i class="fas fa-plus"></i>
                                                                        </button>
                                                                    </div>
                                                                    <!-- /.card-tools -->
                                                                </div>
                                                                <!-- /.card-header -->
                                                                <div class="card-body">
                                                                    <div class="row g-3">
                                                                        <div class="col-md-3">
                                                                            <div class="form-group">
                                                                                <span class="details">TIPO DE VIAGEM:</span>
                                                                                <div class="input-group">
                                                                                    <asp:TextBox ID="lblTipoViagem" runat="server" class="form-control" ReadOnly="true" Style="text-align: center" Text='<%# Eval("tipo_viagem") %>'></asp:TextBox>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-md-1">
                                                                            <div class="form-group">
                                                                                <span class="details">ROTA:</span>
                                                                                <div class="input-group">
                                                                                    <asp:TextBox ID="lblRota" runat="server" class="form-control" ReadOnly="true" Style="text-align: center" Text='<%# Eval("rota") %>'></asp:TextBox>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-md-3">
                                                                            <div class="form-group">
                                                                                <span class="details">VEÍCULO:</span>
                                                                                <div class="input-group">
                                                                                    <asp:TextBox ID="lblVeiculo" runat="server" class="form-control" ReadOnly="true" Style="text-align: center" Text='<%# Eval("veiculo") %>'></asp:TextBox>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-md-5">
                                                                            <div class="form-group">
                                                                                <span class="details">QUANT./PALLET´S:</span>
                                                                                <div class="input-group">
                                                                                    <asp:TextBox ID="lblQuant" runat="server" class="form-control" ReadOnly="true" Style="text-align: center" Text='<%# Eval("quant_palet") %>'></asp:TextBox>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row g-3">
                                                                        <div class="col-md-2">
                                                                            <div class="form-group">
                                                                                <span class="details">PESO:</span>
                                                                                <div class="input-group">
                                                                                    <asp:TextBox ID="lblPeso" runat="server" class="form-control" ReadOnly="true" Style="text-align: center" Text='<%# Eval("peso") %>'></asp:TextBox>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-md-1">
                                                                            <div class="form-group">
                                                                                <span class="details">M<sup>3</span>
                                                                                <div class="input-group">
                                                                                    <asp:TextBox ID="lblMetragem" runat="server" class="form-control" ReadOnly="true" Style="text-align: center" Text='<%# Eval("pedidos") %>'></asp:TextBox>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-md-4">
                                                                            <div class="form-group">
                                                                                <span class="details">SOLICITAÇÕES:</span>
                                                                                <div class="input-group">
                                                                                    <asp:TextBox ID="lblSolicitacao" runat="server" class="form-control" ReadOnly="true" Style="text-align: center" Text='<%# Eval("solicitacoes") %>'></asp:TextBox>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-md-3">
                                                                            <div class="form-group">
                                                                                <span class="details">ESTUDO DE ROTA:</span>
                                                                                <div class="input-group">
                                                                                    <asp:TextBox ID="lblEstRota" runat="server" class="form-control" ReadOnly="true" Style="text-align: center" Text='<%# Eval("estudo_rota") %>'></asp:TextBox>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-md-2">
                                                                            <div class="form-group">
                                                                                <span class="details">REMESSA:</span>
                                                                                <div class="input-group">
                                                                                    <asp:TextBox ID="lblRemessa" runat="server" class="form-control" ReadOnly="true" Style="text-align: center" Text='<%# Eval("remessa") %>'></asp:TextBox>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <!-- /.card-body -->
                                                            </div>

                                                            <div class="card card-outline card-info">
                                                                <div class="card-header">
                                                                    <h3 class="card-title"><i class="far fa-edit"></i>&nbsp;Dados do Atendimento</h3>
                                                                    <div class="card-tools">
                                                                        <button type="button" class="btn btn-tool" data-card-widget="collapse">
                                                                            <i class="fas fa-minus"></i>
                                                                        </button>
                                                                    </div>
                                                                    <!-- /.card-tools -->
                                                                </div>
                                                                <div class="card-body">
                                                                    <div class="item-coleta">
                                                                        <div class="row g-3">
                                                                            <div class="col-md-2">
                                                                                <div class="form-group">
                                                                                    <span class="details">Nº CVA:<asp:Label ID="lblMensagem" runat="server" Text=""></asp:Label></span>
                                                                                    <div class="input-group">
                                                                                        <asp:TextBox ID="txtCVA" runat="server" CssClass="form-control cva" MaxLength="11" Text='<%# Bind("cva") %>' Style="text-align: center"></asp:TextBox>
                                                                                    </div>
                                                                                    <span class="msg-erro text-danger" style="display: none;"></span>
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-md-2">
                                                                                <div class="form-group">
                                                                                    <span class="details">JANELA GATE:</span>
                                                                                    <div class="input-group">
                                                                                        <asp:TextBox ID="txtGate" runat="server" TextMode="DateTimeLocal" Text='<%# Eval("gate","{0:yyyy-MM-ddTHH:mm}") %>' CssClass="form-control gate" Style="text-align: center"></asp:TextBox>
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
                                                                        <div class="row g-3">
                                                                            <div class="col-md-6">
                                                                                <div class="card card-outline card-success">
                                                                                    <div class="card-header">
                                                                                        <h3 class="card-title"><i class="far fa-edit"></i>&nbsp;Local da Coleta</h3>
                                                                                        <div class="card-tools">
                                                                                            <button type="button" class="btn btn-tool" data-card-widget="collapse">
                                                                                                <i class="fas fa-minus"></i>
                                                                                            </button>
                                                                                        </div>
                                                                                        <!-- /.card-tools -->
                                                                                    </div>
                                                                                    <div class="card-body">
                                                                                        <div class="row g-3">
                                                                                            <div class="col-md-6">
                                                                                                <div class="form-group">
                                                                                                    <span class="details">CHEGADA:</span>
                                                                                                    <div class="input-group">
                                                                                                        <asp:TextBox ID="txtChegadaOrigem" runat="server"
                                                                                                            CssClass="form-control chegada"
                                                                                                            Text='<%# Bind("chegadaorigem", "{0:yyyy-MM-ddTHH:mm}") %>'
                                                                                                            TextMode="DateTimeLocal"
                                                                                                            Style="text-align: center" onChange="validarDatas(item)" />
                                                                                                    </div>
                                                                                                    <span class="msg-erro text-danger" style="display: none;"></span>
                                                                                                </div>
                                                                                            </div>
                                                                                            <div class="col-md-6">
                                                                                                <div class="form-group">
                                                                                                    <span class="details">SAÍDA:</span>
                                                                                                    <div class="input-group">
                                                                                                        <asp:TextBox ID="txtSaidaOrigem" runat="server"
                                                                                                            CssClass="form-control saida"
                                                                                                            Text='<%# Bind("saidaorigem", "{0:yyyy-MM-ddTHH:mm}") %>'
                                                                                                            TextMode="DateTimeLocal"
                                                                                                            Style="text-align: center" onChange="validarDatas(item)" />
                                                                                                    </div>
                                                                                                    <span class="msg-erro text-danger" style="display: none;"></span>
                                                                                                </div>
                                                                                            </div>

                                                                                        </div>
                                                                                        <div class="row g-3">
                                                                                            <div class="col-md-12">
                                                                                                <div class="form-group">
                                                                                                    <span class="details" style="text-align: center">TEMPO DE ESPERA:</span>
                                                                                                    <div class="input-group">
                                                                                                        <asp:TextBox ID="txtAgCarreg" runat="server"
                                                                                                            CssClass="form-control espera"
                                                                                                            Text='<%# Bind("tempoagcarreg") %>'
                                                                                                            Style="text-align: center" onkeydown="return false;" />
                                                                                                    </div>
                                                                                                    <span class="msg-erro text-danger" style="display: none;"></span>
                                                                                                </div>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-md-6">
                                                                                <div class="card card-outline card-warning">
                                                                                    <div class="card-header">
                                                                                        <h3 class="card-title"><i class="far fa-edit"></i>&nbsp;Local de Entrega</h3>
                                                                                        <div class="card-tools">
                                                                                            <button type="button" class="btn btn-tool" data-card-widget="collapse">
                                                                                                <i class="fas fa-minus"></i>
                                                                                            </button>
                                                                                        </div>
                                                                                        <!-- /.card-tools -->
                                                                                    </div>
                                                                                    <div class="card-body">
                                                                                        <div class="row g-3">
                                                                                            <div class="col-md-4">
                                                                                                <div class="form-group">
                                                                                                    <span class="details">CHEGADA:</span>
                                                                                                    <div class="input-group">
                                                                                                        <asp:TextBox ID="txtChegadaDestino" runat="server"
                                                                                                            Text='<%# Bind("chegadadestino", "{0:yyyy-MM-ddTHH:mm}") %>'
                                                                                                            CssClass="form-control chegada-planta"
                                                                                                            TextMode="DateTimeLocal"
                                                                                                            Style="text-align: center" onChange="validarDatas(item)" />

                                                                                                    </div>
                                                                                                    <span class="msg-erro text-danger" style="display: none;"></span>
                                                                                                </div>
                                                                                            </div>
                                                                                            <div class="col-md-4">
                                                                                                <div class="form-group">
                                                                                                    <span class="details">ENTRADA:</span>
                                                                                                    <div class="input-group">
                                                                                                        <asp:TextBox ID="txtEntrada" runat="server"
                                                                                                            Text='<%# Bind("entradaplanta", "{0:yyyy-MM-ddTHH:mm}") %>'
                                                                                                            CssClass="form-control entrada-planta"
                                                                                                            TextMode="DateTimeLocal"
                                                                                                            Style="text-align: center" onChange="validarDatas(item)" />

                                                                                                    </div>
                                                                                                    <span class="msg-erro text-danger" style="display: none;"></span>
                                                                                                </div>
                                                                                            </div>
                                                                                            <div class="col-md-4">
                                                                                                <div class="form-group">
                                                                                                    <span class="details">ESPERA GATE:</span>
                                                                                                    <div class="input-group">
                                                                                                        <asp:TextBox ID="txtEsperaGate" runat="server" Text='<%# Bind("tempoesperagate") %>'
                                                                                                            CssClass="form-control espera-gate"
                                                                                                            Style="text-align: center" onkeydown="return false;" />

                                                                                                    </div>
                                                                                                </div>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="row g-3">
                                                                                            <div class="col-md-6">
                                                                                                <div class="form-group">
                                                                                                    <span class="details">SAIDA:</span>
                                                                                                    <div class="input-group">
                                                                                                        <asp:TextBox ID="txtSaidaPlanta" runat="server"
                                                                                                            Text='<%# Bind("saidaplanta", "{0:yyyy-MM-ddTHH:mm}") %>'
                                                                                                            CssClass="form-control saida-planta"
                                                                                                            TextMode="DateTimeLocal"
                                                                                                            Style="text-align: center" onChange="validarDatas(item)" />

                                                                                                    </div>
                                                                                                    <span class="msg-erro text-danger" style="display: none;"></span>
                                                                                                </div>
                                                                                            </div>
                                                                                            <div class="col-md-6">
                                                                                                <div class="form-group">
                                                                                                    <span class="details">TEMPO DE ESPERA:</span>
                                                                                                    <div class="input-group">
                                                                                                        <asp:TextBox ID="txtDentroPlanta" runat="server"
                                                                                                            Text='<%# Bind("tempodentroplanta") %>'
                                                                                                            CssClass="form-control dentro-planta"
                                                                                                            Style="text-align: center" onkeydown="return false;" />

                                                                                                    </div>
                                                                                                </div>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
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
                                        <asp:Label ID="lblDtCadastro" runat="server" CssClass="form-control" placeholder="" maxlength="20" readonly="true"></asp:Label>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <span class="details">POR:</span>
                                        <asp:TextBox ID="txtUsuCadastro" runat="server" CssClass="form-control" placeholder="" MaxLength="60" Enabled="false"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-2">
                                    <div class="form-group">
                                        <span class="details">ATUALIZADO EM:</span>
                                        <asp:Label ID="lblAtualizadoEm" runat="server" CssClass="form-control" placeholder="" maxlength="20" readonly="true"></asp:Label>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <span class="details">POR:</span>
                                        <asp:TextBox ID="txtAtualizadoPor" runat="server" CssClass="form-control" placeholder="" MaxLength="60" Enabled="false"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="row g-3">
                                <div class="col-md-1">
                                    <br />
                                    <asp:Button ID="btnSalvar1" CssClass="btn btn-outline-success  btn-lg" runat="server" OnClick="btnSalvar1_Click" Text="Atualizar"/>
                                </div>
                               
                                <div class="col-md-1">
                                    <br />
                                    <asp:Button ID="btnEncerrar" CssClass="btn btn-outline-danger  btn-lg" runat="server" Text="Concluir" OnClick="btnEncerrar_Click" />
                                </div>
                                
                                <div class="col-md-1">
                                    <br />
                                    <a href="GestaoDeEntregasMatriz.aspx" class="btn btn-outline-info btn-lg">Fechar               
                                    </a>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- Modal Bootstrap Cadastro de Telefone -->
                <div class="modal fade" id="telefoneModal" tabindex="-1" role="dialog" aria-labelledby="telefoneModalLabel" aria-hidden="true">
                    <div class="modal-dialog modal-dialog-centered" role="document">
                        <div class="modal-content">
                            <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>--%>
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
                            <%--</ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnCadContato" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>--%>
                        </div>
                    </div>
                </div>
                <!-- Modal Ocorrências -->
                <div class="modal fade bd-example-modal-xl" id="modalEditar" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true">
                    <div class="modal-dialog modal-dialog-centered modal-xl" role="document">
                        <div class="modal-content">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <div class="modal-header">
                                        <h5 class="modal-title" id="exampleModalCenterTitle">Atualizar Coleta/Entrega</h5>
                                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                            <span aria-hidden="true">&times;</span>
                                        </button>
                                    </div>
                                    <div class="modal-body">
                                        <div class="row g-3">
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <asp:Label ID="lblCVA" runat="server" class="form-control font-weight-bold" Style="text-align: center">  
                                                    </asp:Label>
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <asp:Label ID="lblColeta" runat="server" class="form-control font-weight-bold" Style="text-align: center">  
                                                    </asp:Label>
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <asp:Label ID="lblStatus" runat="server" class="form-control font-weight-bold" Style="text-align: center">  
                                                    </asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row g-3">

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
                                            <button type="button" id="btnFechar" runat="server" class="btn btn-secondary" data-dismiss="modal" onclick="btnFechar_Click">Fechar</button>
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
