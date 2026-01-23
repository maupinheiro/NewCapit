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

        /* =========================
           FUNÇÕES GLOBAIS (MODAIS)
        ========================= */
        function fecharModalOcorrencia() {
            $('#modalOcorrencia').modal('hide');
        }

        function abrirModalTelefone() {
            var codigoFrota = document.getElementById('<%= txtCodFrota.ClientID %>').value;
            document.getElementById('<%= txtCodContato.ClientID %>').value = codigoFrota;

            $('#telefoneModal').modal({ backdrop: 'static', keyboard: false });
        }

        /* =========================
           MÁSCARA CELULAR
        ========================= */
        function aplicarMascaraTelefone() {
            const input = document.getElementById("<%= txtCadCelular.ClientID %>");
            if (!input) return;

            input.addEventListener("input", function () {
                let valor = this.value.replace(/\D/g, "");
                let resultado = "";
                let pos = 0;
                const mascara = "(00) 0 0000-0000";

                for (let i = 0; i < mascara.length; i++) {
                    if (mascara[i] === "0") {
                        if (valor[pos]) resultado += valor[pos++];
                        else break;
                    } else {
                        resultado += mascara[i];
                    }
                }
                this.value = resultado;
            });
        }

        /* =========================
           SELECT2 (SEGURO)
        ========================= */
       

        /* =========================
           TEMPOS E STATUS
        ========================= */
        function converterTempoParaMinutos(str) {
            if (!str || str.includes("Inválido")) return 0;
            let h = parseInt(str.split('h')[0]) || 0;
            let m = parseInt((str.split('h')[1] || '').replace('min', '')) || 0;
            return h * 60 + m;
        }

        function atualizarStatusECores(item) {
            const ddl = item.querySelector('.ddlStatus');
            if (!ddl) return;

            const chegada = item.querySelector('.chegada')?.value;
            const saida = item.querySelector('.saida')?.value;
            const chegadaPlanta = item.querySelector('.chegada-planta')?.value;
            const saidaPlanta = item.querySelector('.saida-planta')?.value;

            if (saidaPlanta) ddl.value = 'Concluido';
            else if (chegadaPlanta) ddl.value = 'Ag. Descarga';
            else if (saida) ddl.value = 'Em Transito';
            else if (chegada) ddl.value = 'Ag. Carregamento';

            const limite = 90;
            ['espera', 'espera-gate', 'dentro-planta'].forEach(cls => {
                const el = item.querySelector('.' + cls);
                if (!el) return;

                const tempo = converterTempoParaMinutos(el.value);
                el.style.backgroundColor = tempo > limite ? 'red' : '';
                el.style.color = tempo > limite ? 'white' : '';
            });
        }

        /* =========================
           CÁLCULOS
        ========================= */
        function calcularTempo(item, iniCls, fimCls, destinoCls) {
            const ini = item.querySelector(iniCls)?.value;
            const fim = item.querySelector(fimCls)?.value;
            const destino = item.querySelector(destinoCls);

            if (!ini || !fim || !destino) return;

            const d1 = new Date(ini);
            const d2 = new Date(fim);
            if (isNaN(d1) || isNaN(d2) || d2 < d1) {
                destino.value = "Inválido";
                return;
            }

            const min = Math.floor((d2 - d1) / 60000);
            destino.value = `${Math.floor(min / 60)}h ${min % 60}min`;

            atualizarStatusECores(item);
        }

        /* =========================
           VALIDAÇÕES
        ========================= */
        function validarDatas(item) {
            // 1. Identifica o container da linha
            let container = item.closest('.expandable-body');
            if (!container) return;

            // --- SELEÇÃO DE CAMPOS ---
            const txtSaidaOrigem = container.querySelector('input[id*="txtSaidaOrigem"]');
            const txtDuracao = container.querySelector('input[id*="txtDuracao"]');
            const txtPrevisaoChegada = container.querySelector('input[id*="txtPrevisaoChegada"]');
            const txtChegadaDestino = container.querySelector('input[id*="txtChegadaDestino"]');
            const txtSaidaPlanta = container.querySelector('input[id*="txtSaidaPlanta"]');
            const txtAgCarreg = container.querySelector('input[id*="txtAgCarreg"]');
            const txtAgDescarga = container.querySelector('input[id*="txtAgDescarga"]');
            const txtDurTransp = container.querySelector('input[id*="txtDurTransp"]');

            // Busca o HiddenField hdEmissao (pode estar no container ou na linha principal do Repeater)
            let hdEmissao = container.querySelector('input[id*="hdEmissao"]');
            if (!hdEmissao) {
                const trPrincipal = container.previousElementSibling;
                if (trPrincipal) hdEmissao = trPrincipal.querySelector('input[id*="hdEmissao"]');
            }

            // --- FUNÇÃO AUXILIAR PARA DIFERENÇA (HHh mmmin) ---
            function formatarDiferenca(inicio, fim) {
                if (!inicio || !fim) return "";
                const d1 = new Date(inicio);
                const d2 = new Date(fim);
                if (isNaN(d1) || isNaN(d2)) return "";
                if (d2 < d1) return "Inválido";

                const diffMs = d2 - d1;
                const totalMinutos = Math.floor(diffMs / 60000);
                const horas = Math.floor(totalMinutos / 60);
                const minutos = totalMinutos % 60;
                return `${horas}h ${minutos}min`;
            }

            // --- 1. CÁLCULO: Previsão de Chegada (Saída + Duração) ---
            if (txtSaidaOrigem && txtDuracao && txtPrevisaoChegada) {
                const valorSaida = txtSaidaOrigem.value;
                const valorDuracao = txtDuracao.value;
                if (valorSaida && valorDuracao) {
                    let dataPrevisao = new Date(valorSaida);
                    const partes = valorDuracao.split(':');
                    dataPrevisao.setHours(dataPrevisao.getHours() + (parseInt(partes[0]) || 0));
                    dataPrevisao.setMinutes(dataPrevisao.getMinutes() + (parseInt(partes[1]) || 0));
                    dataPrevisao.setSeconds(dataPrevisao.getSeconds() + (parseInt(partes[2]) || 0));

                    if (!isNaN(dataPrevisao.getTime())) {
                        const ano = dataPrevisao.getFullYear();
                        const mes = String(dataPrevisao.getMonth() + 1).padStart(2, '0');
                        const dia = String(dataPrevisao.getDate()).padStart(2, '0');
                        const hora = String(dataPrevisao.getHours()).padStart(2, '0');
                        const min = String(dataPrevisao.getMinutes()).padStart(2, '0');
                        txtPrevisaoChegada.value = `${ano}-${mes}-${dia}T${hora}:${min}`;
                    }
                }
            }

            // --- 2. CÁLCULO: Duração da Viagem (Chegada Destino - Saída Origem) ---
            if (txtChegadaDestino && txtSaidaOrigem && txtAgCarreg) {
                txtAgCarreg.value = formatarDiferenca(txtSaidaOrigem.value, txtChegadaDestino.value);
            }

            // --- 3. CÁLCULO: Tempo de Descarga (Saída Planta - Chegada Destino) ---
            if (txtSaidaPlanta && txtChegadaDestino && txtAgDescarga) {
                txtAgDescarga.value = formatarDiferenca(txtChegadaDestino.value, txtSaidaPlanta.value);
            }

            // --- 4. CÁLCULO: Tempo Total da Viagem (Saída Planta - Emissão) ---
            if (txtSaidaPlanta && hdEmissao && txtDurTransp) {
                txtDurTransp.value = formatarDiferenca(hdEmissao.value, txtSaidaPlanta.value);
            }

            // Atualiza cores/status se a função existir
            if (typeof atualizarStatusECores === "function") {
                atualizarStatusECores(container);
            }
        }
        /* =========================
           PEDIDOS (TAB)
        ========================= */
        function carregarPedidos(idCarga) {
            fetch("Carga.aspx/GetPedidos", {
                method: "POST",
                headers: { "Content-Type": "application/json; charset=utf-8" },
                body: JSON.stringify({ idCarga })
            })
                .then(r => r.json())
                .then(d => {
                    if (d && d.d !== undefined) {
                        document.getElementById("conteudoPedidos").innerHTML = d.d;
                    }
                })
                .catch(console.error);
        }

        /* =========================
           BIND GERAL (UMA VEZ)
        ========================= */
        function bindEventos() {

          
            aplicarMascaraTelefone();

            document.querySelectorAll('.item-coleta').forEach(item => {

                if (item.dataset.bound) return;
                item.dataset.bound = "true";

                const ddl = item.querySelector('select[id*="ddlStatus"]');
                if (ddl) ddl.classList.add('ddlStatus');

                item.querySelectorAll('.chegada, .saida, .chegada-planta, .entrada-planta, .saida-planta')
                    .forEach(i => {
                        i.addEventListener('change', () => {
                            calcularTempo(item, '.chegada', '.saida', '.espera');
                            calcularTempo(item, '.chegada-planta', '.entrada-planta', '.espera-gate');
                            calcularTempo(item, '.entrada-planta', '.saida-planta', '.dentro-planta');
                            validarDatas(item);
                        });
                    });

                atualizarStatusECores(item);
            });
        }

        /* =========================
           INICIALIZAÇÃO
        ========================= */
       Sys.Application.add_load(function () {
    // 1. Restaura as abas
    inicializarAbas();
    
    // 2. Aplica o Select2 nos campos visíveis e recém-criados
    aplicarPluginsDinamicos();
    
    // 3. Recalcula os tempos da grid
    recalcularTodosOsTempos();
    
    // 4. Aplica máscaras de telefone se houver
    if (typeof aplicarMascaraTelefone === "function") aplicarMascaraTelefone();
});

    </script>
  <%--  <script>
        function inicializarAbas() {
            console.log("Inicializando abas...");

            // 1. Restaurar a aba salva após o postback parcial
            document.querySelectorAll('.hf-aba-ativa').forEach(hf => {
                const targetId = hf.value;
                if (targetId) {
                    console.log("Restaurando aba:", targetId);
                    const btnAba = document.querySelector(`button[data-bs-target="${targetId}"]`);

                    if (btnAba) {
                        // Usamos a API do Bootstrap para mostrar a aba corretamente
                        const tab = new bootstrap.Tab(btnAba);
                        tab.show();
                    }
                }
            });

            // 2. Escutar o evento de troca de aba para salvar no HiddenField
            // Usamos o evento 'shown.bs.tab' que é disparado quando a animação termina
            document.querySelectorAll('button[data-bs-toggle="tab"]').forEach(btn => {
                btn.addEventListener('shown.bs.tab', function (event) {
                    const target = event.target.getAttribute('data-bs-target');
                    // Busca o HiddenField dentro do mesmo container do botão clicado
                    const container = event.target.closest('.upd-tabs-container');
                    if (container) {
                        const hf = container.querySelector('.hf-aba-ativa');
                        if (hf) {
                            hf.value = target;
                            console.log("Aba salva no HiddenField:", target);
                        }
                    }
                });
            });
        }

        // Carregamento inicial (apenas quando a página carrega inteira)
        document.addEventListener('DOMContentLoaded', inicializarAbas);

        // Pós-Postback do UpdatePanel (apenas se o Sys do ASP.NET existir)
        if (typeof Sys !== 'undefined') {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                console.log("Postback detectado, reinicializando...");
                inicializarAbas();
            });
        }

        // Essencial para UpdatePanel: Re-executa após cada postback parcial
        if (typeof Sys !== 'undefined') {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                inicializarAbas();
            });
        }
    </script>--%>
    <script>
                 function inicializarAbas() {
                     console.log("Iniciando limpeza e restauração de abas...");

                     // 1. Restaurar o estado das abas salvas nos HiddenFields
                     document.querySelectorAll('.hf-aba-ativa').forEach(hf => {
                         const targetId = hf.value; // Ex: #tabNotas_0 ou tabNotas_0
                         if (!targetId) return;

                         // Garante que o ID comece com # para o seletor CSS
                         const selector = targetId.startsWith('#') ? targetId : '#' + targetId;
                         const btnAba = document.querySelector(`button[data-bs-target="${targetId}"], button[data-bs-target="${selector}"]`);

                         if (btnAba) {
                             console.log("Ativando aba e conteúdo para:", selector);

                             // A. Forçar ativação do Botão
                             const container = btnAba.closest('.upd-tabs-container');
                             if (container) {
                                 // Remove active de todos os botões do grupo
                                 container.querySelectorAll('.nav-link').forEach(b => b.classList.remove('active'));
                                 btnAba.classList.add('active');

                                 // B. Forçar ativação do Painel de Conteúdo
                                 // O ID do painel é o targetId sem o #
                                 const paneId = selector.replace('#', '');
                                 const pane = document.getElementById(paneId);

                                 if (pane) {
                                     // Remove show/active de todos os painéis do grupo
                                     container.querySelectorAll('.tab-pane').forEach(p => p.classList.remove('show', 'active'));
                                     pane.classList.add('show');
                                     pane.classList.add('active');
                                 } else {
                                     console.error("Painel não encontrado:", paneId);
                                 }
                             }
                         }
                         configurarFocoScanner();
                     });

                     // 2. Configurar o evento de clique (sem usar o evento do Bootstrap que pode falhar)
                     document.querySelectorAll('button[data-bs-toggle="tab"]').forEach(btn => {
                         btn.onclick = function () {
                             const target = this.getAttribute('data-bs-target');
                             const container = this.closest('.upd-tabs-container');
                             if (container) {
                                 const hf = container.querySelector('.hf-aba-ativa');
                                 if (hf) {
                                     hf.value = target;
                                     console.log("Novo estado salvo:", target);
                                 }
                             }
                         };
                     });
                 }
                    function configurarFocoScanner() {
                        document.querySelectorAll('.chave-cte').forEach(input => {
                            input.addEventListener('keydown', function (e) {
                                if (e.key === 'Enter') {
                                    // Impede que o Enter envie o formulário inteiro
                                    // O AutoPostBack="true" já vai cuidar de disparar o TextChanged
                                }
                            });
                        });
                    }

        // Chame isso dentro da sua função inicializarAbas()
                 // Carregamento inicial
                 document.addEventListener('DOMContentLoaded', inicializarAbas);

                 // Pós-Postback do UpdatePanel
                 if (typeof Sys !== 'undefined') {
                     var prm = Sys.WebForms.PageRequestManager.getInstance();
                     prm.add_endRequest(function () {
                         // Um pequeno delay de 10ms ajuda o DOM a "assentar" no UpdatePanel
                         setTimeout(inicializarAbas, 10);
                     });
                 }
    </script>


    <script type="text/javascript">
        function abrirModal() {
            //$('#meuModal').modal('show');
            $('#meuModal').modal({ backdrop: 'static', keyboard: false });
        }

    </script>

   
   
    <script>
        function aplicarPluginsDinamicos() {
    console.log("Aplicando Select2 e Máscaras...");

    $('.select2').each(function () {
        // Se já existir um select2 no elemento, destrói para evitar bugs
        if ($(this).hasClass("select2-hidden-accessible")) {
            $(this).select2('destroy');
        }
        
        $(this).select2({
            width: '100%',
            // Se estiver dentro de um modal, precisa desta linha:
            // dropdownParent: $(this).closest('.modal').length ? $(this).closest('.modal') : $(document.body)
        });
    });

    // Sincroniza o ID da Rota Krona manualmente
    $('#ddlRotaKrona').off('change').on('change', function () {
        $('#txtId_Rota').val($(this).val());
    });
}
    </script>
    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm) {
            prm.add_endRequest(function (sender, args) {
                recalcularTodosOsTempos();
            });
        }

        $(document).ready(function () {
            recalcularTodosOsTempos();
        });

        // Evento disparado quando você altera a data no calendário do navegador
        $(document).on('change', '.hora-inicio, .hora-fim', function () {
            calcularDiferenca($(this).closest('tr'));
        });

        function recalcularTodosOsTempos() {
            $('.hora-total').each(function () {
                calcularDiferenca($(this).closest('tr'));
            });
        }

        function calcularDiferenca(row) {
            var valInicio = row.find('.hora-inicio').val(); // Formato: yyyy-MM-ddTHH:mm
            var valFim = row.find('.hora-fim').val();

            if (valInicio && valFim) {
                var dataIni = new Date(valInicio);
                var dataFim = new Date(valFim);

                if (dataFim > dataIni) {
                    var diffMs = dataFim - dataIni;
                    var totalMinutos = Math.floor(diffMs / 60000);
                    var horas = Math.floor(totalMinutos / 60);
                    var minutos = totalMinutos % 60;

                    var resultado = (horas < 10 ? "0" + horas : horas) + ":" + (minutos < 10 ? "0" + minutos : minutos);
                    row.find('.hora-total').val(resultado);
                } else {
                    row.find('.hora-total').val("00:00");
                }
            }
        }
    </script>  
    

    <div class="content-wrapper">
        <section class="content">
            <div class="container-fluid">
                <br />
                <!-- ALERTA BOOTSTRAP -->

                <div id="divMsg" runat="server"
                    class="alert alert-warning alert-dismissible fade show mt-3"
                    role="alert" style="display: none;">
                    <span id="lblMsgGeral" runat="server"></span>
                    <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
                </div>
                <div id="divMsgCNH" runat="server"
                    class="alert alert-warning alert-dismissible fade show mt-3"
                    role="alert" style="display: none;">
                    <span id="lblMsgCNH" runat="server"></span>
                    <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
                </div>
                <div id="divMsgGR" runat="server"
                    class="alert alert-warning alert-dismissible fade show mt-3"
                    role="alert" style="display: none;">
                    <span id="lblMsgGR" runat="server"></span>
                    <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
                </div>
                <div id="divMsgVeic" runat="server"
                    class="alert alert-warning alert-dismissible fade show mt-3"
                    role="alert" style="display: none;">
                    <span id="lblMsgVeic" runat="server"></span>
                    <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
                </div>
                <div id="divMsgCET" runat="server"
                    class="alert alert-warning alert-dismissible fade show mt-3"
                    role="alert" style="display: none;">
                    <span id="lblMsgCET" runat="server"></span>
                    <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
                </div>
                <div id="divMsgLinc" runat="server"
                    class="alert alert-warning alert-dismissible fade show mt-3"
                    role="alert" style="display: none;">
                    <span id="lblMsgLinc" runat="server"></span>
                    <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
                </div>
                <div id="divMsgCrono" runat="server"
                    class="alert alert-warning alert-dismissible fade show mt-3"
                    role="alert" style="display: none;">
                    <span id="lblMsgCrono" runat="server"></span>
                    <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
                </div>
                <div id="divMsgCarreta1" runat="server"
                    class="alert alert-warning alert-dismissible fade show mt-3"
                    role="alert" style="display: none;">
                    <span id="lblMsgCarreta1" runat="server"></span>
                    <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
                </div>
                <div id="divMsgCarreta2" runat="server"
                    class="alert alert-warning alert-dismissible fade show mt-3"
                    role="alert" style="display: none;">
                    <span id="lblMsgCarreta2" runat="server"></span>
                    <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
                </div>

                <div class="col-md-12">
                    <div class="card card-info">
                        <div class="card-header">

                            <h3 class="card-title"><i class="fas fa-shipping-fast"></i>&nbsp;ORDEM DE COLETA/ENTREGA - &nbsp;<asp:Label ID="novaColeta" runat="server"></asp:Label></h3>

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
                            <div class="info-box">
                                <%--rounded-circle border--%>
                                <span class="info-box-icon bg-info">
                                    <img src="<%=fotoMotorista%>" class="mg-thumbnail float-center" width="75" height="77" alt="" />
                                </span>
                                <div class="info-box-content">
                                    <span class="info-box-number" />
                                    <div class="row g-3">
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
                                                <span class="details">CÓD./FROTA:</span>
                                                <asp:TextBox ID="txtCodVeiculo" runat="server" Style="text-align: center" class="form-control font-weight-bold" AutoPostBack="true" OnTextChanged="btnPesquisarVeiculo_Click"></asp:TextBox>
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

                                         <div class="col-md-1">
                                             <div class="form-group">
                                                 <span class="details">CONTATO:</span>
                                                 <asp:TextBox ID="txtCodFrota" runat="server" class="form-control font-weight-bold"  Style="text-align: center" AutoPostBack="true" OnTextChanged="btnPesquisarContato_Click"></asp:TextBox>
                                             </div>
                                         </div>
                                         <div class="col-md-2">
                                             <div class="form-group">
                                                 <span class="details">FONE CORPORATIVO:</span>
                                                 <asp:TextBox ID="txtFoneCorp" runat="server" class="form-control font-weight-bold"  Style="text-align: center" ReadOnly="true"></asp:TextBox>
                                             </div>
                                         </div>
                                        <div class="col-md-1">
                                            <br />
                                            <asp:Button ID="btnSalvar1" CssClass="btn btn-outline-success w-100" runat="server" OnClick="btnSalvar1_Click" Text="Atualizar" />
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row g-3">

                                <!-- dados do motorista -->
                                <div class="card card-outline card-info collapsed-card">
                                    <div class="card-header">
                                        <h3 class="card-title"><i class="far fa-edit"></i>&nbsp;Dados do Motorista</h3>
                                        <div class="card-tools">
                                            <button type="button" class="btn btn-tool" data-card-widget="collapse">
                                                <i class="fas fa-plus"></i>
                                            </button>
                                        </div>

                                    </div>

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
                                                    <span class="details">VAL. E.T.I.:</span>
                                                    <div class="input-group">
                                                        <asp:TextBox ID="txtExameToxic" runat="server" class="form-control font-weight-bold" ReadOnly="true" Style="text-align: center"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-2">
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
                                                    <span class="details">LIB.RISCO:</span>
                                                    <div class="input-group">
                                                        <asp:TextBox ID="txtLiberacao" runat="server" class="form-control font-weight-bold" ReadOnly="true" Style="text-align: center;"></asp:TextBox>
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
                                            <div class="col-md-8">
                                                <div class="form-group">
                                                    <span class="details">CONJUNTO:</span>
                                                    <asp:TextBox ID="txtConjunto" runat="server" class="form-control font-weight-bold" ReadOnly="true" MaxLength="80"></asp:TextBox>
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
                                                    <span class="details">PROTOCOLO:</span>
                                                    <div class="input-group">
                                                        <asp:TextBox ID="txtProtocoloCET" runat="server" class="form-control font-weight-bold" ReadOnly="true" Style="text-align: center"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-1">
                                                <div class="form-group">
                                                    <span class="details">LICENC.:</span>
                                                    <asp:TextBox ID="txtCRLVVeiculo" runat="server" class="form-control font-weight-bold" ReadOnly="true" Style="text-align: center"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-1">
                                                <div class="form-group">
                                                    <span class="details">CRONO:</span>
                                                    <asp:TextBox ID="txtCrono" runat="server" class="form-control font-weight-bold" ReadOnly="true" Style="text-align: center"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-2" id="reb1" runat="server" visible="false">
                                                <div class="form-group">
                                                    <span class="details">REBOQUE:</span>
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
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <span class="details">CPF/CNPJ:</span>
                                                    <asp:TextBox ID="txtCPF_CNPJ" runat="server" class="form-control font-weight-bold" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>

                                            <div class="col-md-2">
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
                                <!-- Monitoramento -->
                                <div class="card card-outline card-info collapsed-card">
                                    <div class="card-header">
                                        <h3 class="card-title"><i class="fas fa-map-marker-alt"></i>&nbsp;Rastreamento do Veículo</h3>
                                        <div class="card-tools">
                                            <button type="button" class="btn btn-tool" data-card-widget="collapse">
                                                <i class="fas fa-plus"></i>
                                            </button>
                                        </div>
                                    </div>
                                    <!-- /.card-header -->
                                    <div class="card-body">
                                        <div id="divMsgMapa" runat="server"
                                            class="alert alert-info alert-dismissible fade show mt-3"
                                            role="alert" style="display: none;">
                                            <span id="lblMsgMapa" runat="server"></span>
                                            <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
                                        </div>
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
                                                <div class="col-md-2">
                                                    <div class="form-group">
                                                        <span class="details">CARGA/SOLICITAÇÃO:</span>
                                                        <asp:TextBox ID="txtCarga" Style="text-align: center" onkeypress="return apenasNumeros(event);" runat="server" CssClass="form-control" OnTextChanged="btnAdd_Click" AutoPostBack="true"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <asp:Repeater ID="rptColetas" runat="server" OnItemDataBound="rptColetas_ItemDataBound" OnItemCommand="rptColetas_ItemCommand">
                                                            <HeaderTemplate>

                                                                <table id="gridCargas" class="table table-bordered table-hover">

                                                                    <thead>
                                                                        <tr>
                                                                            <th>CARGA</th>
                                                                            <th>LOCAL DE COLETA</th>
                                                                            <th>LOCAL DE ENTREGA</th>
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
                                                                <asp:HiddenField ID="hdIdCarga" runat="server"
                                                                    Value='<%# Eval("carga") %>' />

                                                                <tr data-widget="expandable-table" aria-expanded="false">
                                                                    <td><%# Eval("carga") %></td>
                                                                    <td><%# Eval("cod_expedidor") + " - " + Eval("expedidor") %></td>
                                                                    <td><%# Eval("cod_recebedor") + " - " + Eval("recebedor") %></td>
                                                                    <td><%# Eval("saidaorigem", "{0:dd/MM/yyyy HH:mm}") %></td>
                                                                    <td><%# Eval("prev_chegada", "{0:dd/MM/yyyy HH:mm}") %></td>
                                                                    <td><%# Eval("chegadadestino", "{0:dd/MM/yyyy HH:mm}") %></td>
                                                                    <td><%# Eval("saidaplanta", "{0:dd/MM/yyyy HH:mm}") %></td>

                                                                    <td><%# Eval("status") %></td>
                                                                    <td runat="server" id="tdAtendimento">
                                                                        <asp:Label ID="lblAtendimento" runat="server" Text=""></asp:Label>
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
                                                                                <!-- REMETENTE -->
                                                                                <div class="form-group row">
                                                                                    <label for="inputRemetente" class="col-sm-1 col-form-label" style="text-align: right">REMETENTE:</label>
                                                                                    <div class="col-md-1">
                                                                                        <asp:TextBox ID="txtCodRemetente" runat="server" CssClass="form-control" ReadOnly="true" Text='<%# Eval("codorigem") %>'></asp:TextBox>
                                                                                    </div>
                                                                                    <div class="col-md-5">
                                                                                        <asp:TextBox ID="cboRemetente" runat="server" CssClass="form-control" ReadOnly="true" Text='<%# Eval("cliorigem") %>'></asp:TextBox>
                                                                                    </div>
                                                                                    <div class="col-md-4">
                                                                                        <asp:TextBox ID="txtMunicipioRemetente" runat="server" CssClass="form-control" ReadOnly="true" Text='<%# Eval("cidorigem") %>'></asp:TextBox>
                                                                                    </div>
                                                                                    <div class="col-md-1">
                                                                                        <asp:TextBox ID="txtUFRemetente" Style="text-align: center" runat="server" CssClass="form-control" ReadOnly="true" Text='<%# Eval("ufcliorigem") %>'></asp:TextBox>
                                                                                    </div>
                                                                                </div>
                                                                                <!-- EXPEDIDOR -->
                                                                                <div class="form-group row">
                                                                                    <label for="inputExpedidor" class="col-sm-1 col-form-label" style="text-align: right">EXPEDIDOR:</label>
                                                                                    <div class="col-md-1">
                                                                                        <asp:TextBox ID="txtCodExpedidor" runat="server" CssClass="form-control" ReadOnly="true" Text='<%# Eval("cod_expedidor") %>'></asp:TextBox>
                                                                                    </div>
                                                                                    <div class="col-md-5">
                                                                                        <asp:TextBox ID="cboExpedidor" runat="server" CssClass="form-control" ReadOnly="true" Text='<%# Eval("expedidor") %>'></asp:TextBox>
                                                                                    </div>
                                                                                    <div class="col-md-4">
                                                                                        <asp:TextBox ID="txtCidExpedidor" runat="server" CssClass="form-control" ReadOnly="true" Text='<%# Eval("cid_expedidor") %>'></asp:TextBox>
                                                                                    </div>
                                                                                    <div class="col-md-1">
                                                                                        <asp:TextBox ID="txtUFExpedidor" Style="text-align: center" runat="server" CssClass="form-control" ReadOnly="true" Text='<%# Eval("uf_expedidor") %>'></asp:TextBox>
                                                                                    </div>
                                                                                </div>
                                                                                <!-- DESTINATARIO -->
                                                                                <div class="form-group row">
                                                                                    <label for="inputDestinatario" class="col-sm-1 col-form-label" style="text-align: right">DEST.:</label>
                                                                                    <div class="col-md-1">
                                                                                        <asp:TextBox ID="txtCodDestinatario" runat="server" CssClass="form-control" ReadOnly="true" Text='<%# Eval("coddestino") %>'></asp:TextBox>
                                                                                    </div>
                                                                                    <div class="col-md-5">
                                                                                        <asp:TextBox ID="cboDestinatario" runat="server" CssClass="form-control" ReadOnly="true" Text='<%# Eval("clidestino") %>'></asp:TextBox>
                                                                                    </div>
                                                                                    <div class="col-md-4">
                                                                                        <asp:TextBox ID="txtMunicipioDestinatario" runat="server" CssClass="form-control" ReadOnly="true" Text='<%# Eval("ciddestino") %>'></asp:TextBox>
                                                                                    </div>
                                                                                    <div class="col-md-1">
                                                                                        <asp:TextBox ID="txtUFDestinatario" Style="text-align: center" runat="server" CssClass="form-control" ReadOnly="true" Text='<%# Eval("ufclidestino") %>'></asp:TextBox>
                                                                                    </div>
                                                                                </div>
                                                                                <!-- RECEBEDOR -->
                                                                                <div class="form-group row">
                                                                                    <label for="inputRecebedor" class="col-sm-1 col-form-label" style="text-align: right">RECEBEDOR:</label>
                                                                                    <div class="col-md-1">
                                                                                        <asp:TextBox ID="txtCodRecebedor" runat="server" CssClass="form-control" ReadOnly="true" Text='<%# Eval("cod_recebedor") %>'></asp:TextBox>
                                                                                    </div>
                                                                                    <div class="col-md-5">
                                                                                        <asp:TextBox ID="cboRecebedor" runat="server" CssClass="form-control" ReadOnly="true" Text='<%# Eval("recebedor") %>'></asp:TextBox>
                                                                                    </div>
                                                                                    <div class="col-md-4">
                                                                                        <asp:TextBox ID="txtCidRecebedor" runat="server" CssClass="form-control" ReadOnly="true" Text='<%# Eval("cid_recebedor") %>'></asp:TextBox>
                                                                                    </div>
                                                                                    <div class="col-md-1">
                                                                                        <asp:TextBox ID="txtUFRecebedor" Style="text-align: center" runat="server" CssClass="form-control" ReadOnly="true" Text='<%# Eval("uf_recebedor") %>'></asp:TextBox>
                                                                                    </div>
                                                                                </div>
                                                                                <!-- CONSIGNATARIO -->
                                                                                <div class="form-group row">
                                                                                    <label for="inputConsignatario" class="col-sm-1 col-form-label" style="text-align: right">CONSIG.:</label>
                                                                                    <div class="col-md-1">
                                                                                        <asp:TextBox ID="txtCodConsignatario" runat="server" CssClass="form-control" ReadOnly="true" Text='<%# Eval("cod_consignatario") %>'></asp:TextBox>
                                                                                    </div>
                                                                                    <div class="col-md-5">
                                                                                        <asp:TextBox ID="txtConsignatario" runat="server" CssClass="form-control" ReadOnly="true" Text='<%# Eval("consignatario") %>'></asp:TextBox>
                                                                                    </div>
                                                                                    <div class="col-md-4">
                                                                                        <asp:TextBox ID="txtCidConsignatario" runat="server" CssClass="form-control" ReadOnly="true" Text='<%# Eval("cid_consignatario") %>'></asp:TextBox>
                                                                                    </div>
                                                                                    <div class="col-md-1">
                                                                                        <asp:TextBox ID="txtUFConsignatario" Style="text-align: center" runat="server" CssClass="form-control" ReadOnly="true" Text='<%# Eval("uf_consignatario") %>'></asp:TextBox>
                                                                                    </div>
                                                                                </div>
                                                                                <!-- PAGADOR -->
                                                                                <div class="form-group row">
                                                                                    <label for="inputPagador" class="col-sm-1 col-form-label" style="text-align: right">PAGADOR:</label>
                                                                                    <div class="col-md-1">
                                                                                        <asp:TextBox ID="txtCodPagador" runat="server" CssClass="form-control" ReadOnly="true" Text='<%# Eval("cod_pagador") %>'></asp:TextBox>
                                                                                    </div>
                                                                                    <div class="col-md-5">
                                                                                        <asp:TextBox ID="txtPagador" runat="server" CssClass="form-control" ReadOnly="true" Text='<%# Eval("pagador") %>'></asp:TextBox>
                                                                                    </div>
                                                                                    <div class="col-md-4">
                                                                                        <asp:TextBox ID="txtCidPagador" runat="server" CssClass="form-control" ReadOnly="true" Text='<%# Eval("cid_pagador") %>'></asp:TextBox>
                                                                                    </div>
                                                                                    <div class="col-md-1">
                                                                                        <asp:TextBox ID="txtUFPagador" Style="text-align: center" runat="server" CssClass="form-control" ReadOnly="true" Text='<%# Eval("uf_pagador") %>'></asp:TextBox>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="row g-3">
                                                                                    <div class="col-md-2">
                                                                                        <div class="form-group">
                                                                                            <span class="details">Tipo de Viagem:</span>
                                                                                            <div class="input-group">
                                                                                                <asp:TextBox ID="lblTipoViagem" runat="server" class="form-control" ReadOnly="true" Style="text-align: center" Text='<%# Eval("deslocamento") %>'></asp:TextBox>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="col-md-1">
                                                                                        <div class="form-group">
                                                                                            <span class="details">Distância:</span>
                                                                                            <div class="input-group">
                                                                                                <asp:TextBox ID="txtDistancia" runat="server" class="form-control" ReadOnly="true" Style="text-align: center" Text='<%# Eval("distancia") %>'></asp:TextBox>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="col-md-1">
                                                                                        <div class="form-group">
                                                                                            <span class="details">Duração:</span>
                                                                                            <div class="input-group">
                                                                                                <asp:TextBox ID="txtDuracao" runat="server" class="form-control" ReadOnly="true" Style="text-align: center" Text='<%# Eval("duracao") %>'></asp:TextBox>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="col-md-1">
                                                                                        <div class="form-group">
                                                                                            <span class="details">Pedágio:</span>
                                                                                            <div class="input-group">
                                                                                                <asp:TextBox ID="lblRota" runat="server" class="form-control" ReadOnly="true" Style="text-align: center" Text='<%# Eval("emitepedagio") %>'></asp:TextBox>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="col-md-2" id="divComprovante" runat="server" visible="false">
                                                                                        <div class="form-group">
                                                                                            <span class="details">Comprovante:</span>
                                                                                            <div class="input-group">
                                                                                                <asp:TextBox ID="lblVeiculo" runat="server" class="form-control" ReadOnly="true" Style="text-align: center" Text=""></asp:TextBox>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="col-md-1" id="divValorPedagio" runat="server" visible="false">
                                                                                        <div class="form-group">
                                                                                            <span class="details">Valor Pedágio:</span>
                                                                                            <div class="input-group">
                                                                                                <asp:TextBox ID="lblQuant" runat="server" class="form-control" ReadOnly="true" Style="text-align: center" Text=""></asp:TextBox>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="row g-3">
                                                                                    <div class="col-md-1">
                                                                                        <div class="form-group">
                                                                                            <span class="details">Peso:</span>
                                                                                            <div class="input-group">
                                                                                                <asp:TextBox ID="lblPeso" runat="server" class="form-control" ReadOnly="true" Style="text-align: center" Text='<%# Eval("peso") %>'></asp:TextBox>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="col-md-1">
                                                                                        <div class="form-group">
                                                                                            <span class="details">Entrega:</span>
                                                                                            <div class="input-group">
                                                                                                <asp:TextBox ID="txtEntrega" runat="server" class="form-control" ReadOnly="true" Style="text-align: center" Text='<%# Eval("entrega") %>'></asp:TextBox>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="col-md-2">
                                                                                        <div class="form-group">
                                                                                            <span class="details">Solicitante:</span>
                                                                                            <div class="input-group">
                                                                                                <asp:TextBox ID="lblSolicitacao" runat="server" class="form-control" ReadOnly="true" Style="text-align: center" Text='<%# Eval("solicitante") %>'></asp:TextBox>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="col-md-2">
                                                                                        <div class="form-group">
                                                                                            <span class="details">Material:</span>
                                                                                            <div class="input-group">
                                                                                                <asp:TextBox ID="lblEstRota" runat="server" class="form-control" ReadOnly="true" Style="text-align: center" Text='<%# Eval("material") %>'></asp:TextBox>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="col-md-2">
                                                                                        <div class="form-group">
                                                                                            <span class="details">GR:</span>
                                                                                            <div class="input-group">
                                                                                                <asp:TextBox ID="lblRemessa" runat="server" class="form-control" ReadOnly="true" Style="text-align: center" Text='<%# Eval("gr") %>'></asp:TextBox>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="col-md-1">
                                                                                        <div class="form-group">
                                                                                            <span class="details">Rede:</span>
                                                                                            <div class="input-group">
                                                                                                <asp:TextBox ID="txtRedes" runat="server" Text='<%# Eval("rede") %>' class="form-control" Style="text-align: center"></asp:TextBox>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="col-md-1">
                                                                                        <div class="form-group">
                                                                                            <span class="details">Catraca:</span>
                                                                                            <div class="input-group">
                                                                                                <asp:TextBox ID="txtCatracas" runat="server" Text='<%# Bind("catraca") %>' class="form-control" Style="text-align: center"></asp:TextBox>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="col-md-1">
                                                                                        <div class="form-group">
                                                                                            <span class="details">Conta Débito:</span>
                                                                                            <div class="input-group">
                                                                                                <asp:TextBox ID="txtConta_Debito_Solicitacao" runat="server" class="form-control" Style="text-align: center" ReadOnly="true" Text='<%# Eval("conta_debito_solicitacao") %>'></asp:TextBox>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="col-md-1">
                                                                                        <div class="form-group">
                                                                                            <span class="details">Centro Custo:</span>
                                                                                            <div class="input-group">
                                                                                                <asp:TextBox ID="txtCento_Custo_Solicitacao" runat="server" class="form-control" Style="text-align: center" ReadOnly="true" Text='<%# Eval("centro_custo_solicitacao") %>'></asp:TextBox>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="row g-3">
                                                                                    <div class="col-md-2">
                                                                                        <div class="form-group">
                                                                                            <span class="details">Tipo de Solicitação:</span>
                                                                                            <div class="input-group">
                                                                                                <asp:TextBox ID="txtTipo_Solicitacao" runat="server" class="form-control" ReadOnly="true" Style="text-align: center" Text='<%# Eval("tipo_solicitacao") %>'></asp:TextBox>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="col-md-2">
                                                                                        <div class="form-group">
                                                                                            <span class="details">Tipo de Geração:</span>
                                                                                            <div class="input-group">
                                                                                                <asp:TextBox ID="txtTipo_Geracao_Solicitacao" runat="server" class="form-control" ReadOnly="true" Style="text-align: center" Text='<%# Eval("tipo_geracao_solicitacao") %>'></asp:TextBox>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="col-md-4">
                                                                                        <div class="form-group">
                                                                                            <span class="details">Tipo de Veículo:</span>
                                                                                            <div class="input-group">
                                                                                                <asp:TextBox ID="txtTipo_Veiculo_Solicitacao" runat="server" class="form-control" ReadOnly="true" Style="text-align: center" Text='<%# Eval("tipo_veiculo_solicitacao") %>'></asp:TextBox>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="col-md-2">
                                                                                        <div class="form-group">
                                                                                            <span class="details">Ctrl.Cliente:</span>
                                                                                            <div class="input-group">
                                                                                                <asp:TextBox ID="txtOT" runat="server" Text='<%# Bind("ot") %>' class="form-control" Style="text-align: center"></asp:TextBox>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="col-md-1">
                                                                                            <br />
                                                                                            <asp:Button ID="btnSalvarDadosColeta" runat="server" Text="Atualizar" CssClass="btn btn-outline-info w-100" CommandName="AtualizarColeta" CommandArgument='<%# Eval("carga") %>' />
                                                                                        </div>

                                                                                </div>


                                                                            </div>
                                                                        </div>

<div class="card card-outline card-info collapsed-card">
<div class="card-header">
<h3 class="card-title"><i class="far fa-edit"></i>&nbsp;Detalhes da Coleta</h3>
<div class="card-tools">
<button type="button" class="btn btn-tool" data-card-widget="collapse">
<i class="fas fa-plus"></i>
</button>
</div>
<!-- /.card-tools -->
</div>
<!-- /.card-header -->
<div class="card-body">
<asp:UpdatePanel ID="updTabs" runat="server">
<ContentTemplate>
    <div class="upd-tabs-container">
<input type="hidden" id="hfAbaAtiva" runat="server" class="hf-aba-ativa" />
<!-- COLE AS ABAS AQUI -->
<ul class="nav nav-tabs" role="tablist">
<li class="nav-item">
<button class="nav-link active" data-bs-toggle="tab" data-bs-target='<%# "tabPedidos_" + ((RepeaterItem)Container).ItemIndex %>'>
📦 Pedidos
</button>
</li>
<li class="nav-item active">
<button class="nav-link" data-bs-toggle="tab" data-bs-target='<%# "tabNotas_" + ((RepeaterItem)Container).ItemIndex %>'>
🧾 Notas Fiscais
</button>
</li>
<li class="nav-item">
<button class="nav-link" data-bs-toggle="tab" data-bs-target='<%# "tabCte_" + ((RepeaterItem)Container).ItemIndex %>'>
🧾 CT-e / NFS-e / MDF-e
</button>
</li>
<li class="nav-item">
<button class="nav-link" data-bs-toggle="tab" data-bs-target='<%# "tabPedagio_" + ((RepeaterItem)Container).ItemIndex %>'>
Pedágio
</button>
</li>
<li class="nav-item">
<button class="nav-link" data-bs-toggle="tab" data-bs-target='<%# "tabKrona_" + ((RepeaterItem)Container).ItemIndex %>'>
Krona
</button>
</li>
<li class="nav-item">
<button class="nav-link" data-bs-toggle="tab" data-bs-target='<%# "tabDespesa_" + ((RepeaterItem)Container).ItemIndex %>'>
Despesa Motorista
</button>
</li>
<li class="nav-item">
<button class="nav-link" data-bs-toggle="tab" data-bs-target='<%# "tabHistorico_" + ((RepeaterItem)Container).ItemIndex %>'>
Histórico
</button>
</li>
<li class="nav-item">
<button class="nav-link" data-bs-toggle="tab" data-bs-target='<%# "tabAlteracao_" + ((RepeaterItem)Container).ItemIndex %>'>
Alterações
</button>
</li>
</ul>
    
<div class="tab-content border border-top-0 p-3">
<!-- ABA PEDIDOS -->
<div class="tab-pane fade show active" id='<%# "tabPedidos_" + ((RepeaterItem)Container).ItemIndex %>'>
<asp:GridView ID="gvPedidos" runat="server" CssClass="table table-sm table-striped" DataKeyNames="pedido" AutoGenerateColumns="False" OnRowDataBound="gvPedidos_RowDataBound">
<Columns>
<asp:BoundField DataField="pedido" HeaderText="Pedido" />
<asp:BoundField DataField="emissao"
HeaderText="Emissão"
DataFormatString="{0:dd/MM/yyyy}" />
<asp:BoundField DataField="peso" HeaderText="Peso" />
<asp:BoundField DataField="material" HeaderText="Material" />
<asp:BoundField DataField="portao" HeaderText="Portão" />
<asp:TemplateField HeaderText="Motorista">
<ItemTemplate>                                                                                                           
    <asp:DropDownList ID="ddlMotCar" runat="server" CssClass="form-select select2">
    </asp:DropDownList>                                                                                                 

</ItemTemplate>
  </asp:TemplateField>
<asp:TemplateField HeaderText="Início">
    <ItemTemplate>
        <asp:TextBox ID="txtInicioCar" TextMode="DateTimeLocal" runat="server" 
            CssClass="form-control hora-inicio" 
            Text='<%# Eval("iniciocar") != DBNull.Value ? Convert.ToDateTime(Eval("iniciocar")).ToString("yyyy-MM-ddTHH:mm") : "" %>' ></asp:TextBox>
    </ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Fim">
    <ItemTemplate>
        <asp:TextBox ID="txtTermCar" TextMode="DateTimeLocal" runat="server" 
            CssClass="form-control hora-fim" 
            Text='<%# Eval("termcar") != DBNull.Value ? Convert.ToDateTime(Eval("termcar")).ToString("yyyy-MM-ddTHH:mm") : "" %>'></asp:TextBox>
    </ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Tempo">
    <ItemTemplate>
        <asp:TextBox ID="txtTempoTotal" runat="server" CssClass="form-control hora-total" Text='<%# Eval("duracao")%>'  tabIndex="-1"></asp:TextBox>
    </ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView>
</div>

<div class="tab-pane fade" id='<%# "tabNotas_" + ((RepeaterItem)Container).ItemIndex %>'>
    <!-- Conteúdo Notas Fiscais -->
</div>

<div class="tab-pane fade" id='<%# "tabCte_" + ((RepeaterItem)Container).ItemIndex %>'> 
<!-- Conteúdo CT-e / NFS-e -->
<div class="form-group row">
   <div class="col-md-4">
       <asp:TextBox ID="txtChaveCte" CssClass="form-control chave-cte" OnTextChanged="txtChaveCte_TextChanged"  placeholder="Chave de Acesso do CT-e / RPS-e" runat="server" maxlength="44" AutoPostBack="true"></asp:TextBox>
   </div>
   
  <%-- <div class="col-md-6">--%>
           <label for="inputmdfe" class="col-sm-3 col-form-label" style="text-align: right">MDF-e:</label>
          <div class="col-md-4">
               <asp:TextBox ID="txtMDFe" runat="server" CssClass="form-control" Text='<%# Eval("mdfe")%>' maxlength="44"></asp:TextBox>
           </div>
   <%--</div>--%>


   </div>

</br>
<div class="row g-3">
    <asp:HiddenField ID="hdflIdviagem" Value='<%# Eval("carga") %>' runat="server" />
    <asp:GridView ID="gvCte" runat="server" 
        CssClass="table table-sm table-bordered mt-2" 
        AutoGenerateColumns="False" 
        GridLines="None">
        <Columns>
         <asp:BoundField DataField="uf_emissor" HeaderText="Estado" />
        <asp:BoundField DataField="empresa_emissora" HeaderText="Filial" />
        <asp:BoundField DataField="num_documento" HeaderText="Nº CT-e" />
        <asp:BoundField DataField="serie_documento" HeaderText="Série" />
        <asp:BoundField DataField="mes_ano_documento" HeaderText="Emissão" />
        <asp:TemplateField HeaderText="Status">
                <ItemTemplate>
                    <span class='<%# Eval("Status").ToString().Contains("Lido") ? "badge bg-warning" : "badge bg-success" %>'>
                        <%# Eval("Status") %>
                    </span>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <EmptyDataTemplate>
            <div class="alert alert-info mt-2">Nenhum documento lido para esta carga.</div>
        </EmptyDataTemplate>
    </asp:GridView>
</div>
</div>

<div class="tab-pane fade" id='<%# "tabPedagio_" + ((RepeaterItem)Container).ItemIndex %>'>
<!-- Conteúdo da aba Pedágio -->
<div class="row g-3">
<div class="col-md-2">
<div class="form-group">
<span class="details">IdViagem/Comprovante:</span>
<asp:TextBox ID="txtIdPedagio" class="form-control" runat="server" Text='<%# Eval("idpedagio") %>' ReadOnly="true"></asp:TextBox>
</div>
</div>
 <div class="col-md-2">
    <div class="form-group">
    <span class="details">Valor Creditado:</span>
    <asp:TextBox ID="txtValorPedagio" class="form-control" runat="server" Text='<%# Eval("valorpedagio", "{0:C2}") %>' ReadOnly="true"></asp:TextBox>
    </div>
 </div>
 <div class="col-md-2">
   <div class="form-group">
   <span class="details">Emissão:</span>
   <asp:TextBox ID="txtDtemissaoPedagio" class="form-control" runat="server" Text='<%# Eval("dtemissaopedagio", "{0:dd/MM/yyyy HH:mm}") %>' ReadOnly="true" ></asp:TextBox>
   </div>
 </div>
 <div class="col-md-3">
   <div class="form-group">
   <span class="details">Emitido Por:</span>
   <asp:TextBox ID="txtCreditoPedagio" class="form-control" runat="server" Text='<%# Eval("creditopedagio") %>' ReadOnly="true"></asp:TextBox>
   </div>
 </div>
</div>
<div class="row g-3">
  <div class="col-md-12">
     <div class="form-group">
        <span class="details">Observações:</span>
        <asp:TextBox ID="txtHistoricoPedagio" TextMode="MultiLine" Rows="3" class="form-control" Text='<%# Eval("historicopedagio") %>' runat="server" ReadOnly="true"></asp:TextBox>
     </div>
  </div>
</div>
</div>

<div class="tab-pane fade" id='<%# "tabKrona_" + ((RepeaterItem)Container).ItemIndex %>'>
<!-- Conteúdo Krona -->
<div class="row g-3">
    <div class="col-md-2">
        <div class="form-group">
        <span class="details">Num. SM:</span>
        <asp:TextBox ID="txtSM" class="form-control" runat="server"></asp:TextBox>
        </div>
    </div>
    <div class="col-md-2">
        <div class="form-group">
        <span class="details">Percurso:</span>
        <asp:DropDownList 
            ID="ddlPercurso" 
            runat="server"
            CssClass="form-select">
    
            <asp:ListItem Text="Selecione..." Value="" />
            <asp:ListItem Text="Urbano" Value="Urbano" />
            <asp:ListItem Text="Rodoviário" Value="Rodoriário" />
        </asp:DropDownList>
    </div>
</div>
    <div class="col-md-2">
        <div class="form-group">
        <span class="details">Peso Total:</span>
        <asp:TextBox ID="txtPeso" class="form-control" runat="server"></asp:TextBox>
        </div>
    </div>
    <div class="col-md-2">
        <div class="form-group">
        <span class="details">Valor Total:</span>
        <asp:TextBox ID="txtValorTotal" class="form-control" runat="server"></asp:TextBox>
        </div>
    </div>
    <div class="col-md-2">
        <div class="form-group">
        <span class="details">Previsão Inicio:</span>
        <asp:TextBox ID="txtPrevisaoInicio" class="form-control" runat="server"></asp:TextBox>
        </div>
    </div>
    <div class="col-md-2">
        <div class="form-group">
        <span class="details">Previsão Termino:</span>
        <asp:TextBox ID="txtPrevisaoTermino" class="form-control" runat="server"></asp:TextBox>
        </div>
    </div>
</div>
<div class="row g-3">
    <div class="col-md-2">
        <%--<div class="form-group">
        <span class="details">Id Rota:</span>
        <asp:TextBox ID="txtIdRotaKrona" class="form-control" runat="server" ReadOnly="true"></asp:TextBox>
        </div>--%>
        <asp:TextBox ID="txtId_Rota"
    runat="server"
    CssClass="form-control mt-2"
    ClientIDMode="Static"
    ReadOnly="true">
</asp:TextBox>
    </div>
    <div class="col-md-4">
        <%--<div class="form-group">
        <span class="details">Descrição da Rota:</span>
        <asp:DropDownList 
            ID="ddlRotaKrona" 
            runat="server"
            CssClass="form-select select2"
            AutoPostBack="true"
            OnSelectedIndexChanged="ddlRotaKrona_SelectedIndexChanged">
        </asp:DropDownList>
        </div>--%>
        <asp:DropDownList ID="ddlRotaKrona"
    runat="server"
    CssClass="form-control select2"
    ClientIDMode="Static">
</asp:DropDownList>

    </div>
    <div class="col-md-4">
        <div class="form-group">
        <span class="details">Enviada Por:</span>
        <asp:TextBox ID="txtSmEnviadaPor" class="form-control" runat="server"></asp:TextBox>
        </div>
    </div>
    <div class="col-md-2">
        <br />
        <asp:Button ID="btnEnviarSM" CssClass="btn btn-outline-success w-100" runat="server" Text="Enviar SM" />
    </div>
</div>
</div>

<div class="tab-pane fade" id='<%# "tabDespesas_" + ((RepeaterItem)Container).ItemIndex %>'>
    <!-- Conteúdo Despesa Motorista -->
</div>

<div class="tab-pane fade" id='<%# "tabHistorico_" + ((RepeaterItem)Container).ItemIndex %>'>
    <!-- Conteúdo Histórico -->
    <div class="row g-3">
        <div class="col-md-12">
            <div class="form-group">
                <span class="details">Observações:</span>
                <asp:TextBox ID="txtHistoricoObservacao" Text='<%# Eval("observacao") %>' TextMode="MultiLine" Rows="4" class="form-control" runat="server"></asp:TextBox>
            </div>
        </div>
</div>

    
</div>

<div class="tab-pane fade" id='<%# "tabAlteracoes_" + ((RepeaterItem)Container).ItemIndex %>'>
    <!-- Conteúdo Alterações -->
</div>
                                                                                        
</div>
                                                                                                <div class="row g-3">
                                                                                        <div class="col-md-10"></div>
                                                                                        <div class="col-md-2">
    <br />
    <asp:Button ID="Button1" runat="server" Text="Atualizar" CssClass="btn btn-outline-info w-100" CommandName="AtualizarAbas" CommandArgument='<%# Eval("carga") %>' />
</div></div></div>
</div>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>




                                                                        <!-- /.card-body -->
                                                                        </div>

                                                                 <asp:HiddenField ID="hdEmissao" runat="server"
                                                                     Value='<%#Eval("emissao", "{0:yyyy-MM-ddTHH:mm:ss}") %>' />
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
                                                                                                <span class="details">Janela Gate Origem:<asp:Label ID="lblMensagem" runat="server" Text=""></asp:Label></span>
                                                                                                <div class="input-group">
                                                                                                    <div class="input-group">
                                                                                                        <asp:TextBox ID="txtGateOrigem" runat="server" TextMode="DateTimeLocal" Text='<%# Eval("gate_origem","{0:yyyy-MM-ddTHH:mm}") %>' CssClass="form-control gate" Style="text-align: center" ReadOnly="true"></asp:TextBox>
                                                                                                    </div>

                                                                                                </div>
                                                                                                <span class="msg-erro text-danger" style="display: none;"></span>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="col-md-2">
                                                                                            <div class="form-group">
                                                                                                <span class="details">Janela Gate Destino:</span>
                                                                                                <div class="input-group">
                                                                                                    <asp:TextBox ID="txtGateDestino" runat="server" TextMode="DateTimeLocal" Text='<%# Eval("gate_destino","{0:yyyy-MM-ddTHH:mm}") %>' CssClass="form-control gate" Style="text-align: center" ReadOnly="true"></asp:TextBox>
                                                                                                </div>
                                                                                                <span class="msg-erro text-danger" style="display: none;"></span>
                                                                                            </div>
                                                                                        </div>                                                                                        
                                                                                        <div class="col-md-2">
    <div class="form-group">
        <span class="details">Data e Hora da Coleta:<asp:Label ID="Label2" runat="server" Text=""></asp:Label></span>
        <div class="input-group">
            <div class="input-group">
                <asp:TextBox ID="txtDataHoraColeta" runat="server" TextMode="DateTimeLocal" Text='<%# Eval("data_hora_coleta","{0:yyyy-MM-ddTHH:mm}") %>' CssClass="form-control gate" Style="text-align: center" ReadOnly="true"></asp:TextBox>
            </div>

        </div>
        <span class="msg-erro text-danger" style="display: none;"></span>
    </div>
                                                                                            </div>
                                                                                        <div class="col-md-2">
    <div class="form-group">
        <span class="details">Núm. CVA:</span>
        <div class="input-group">
            <asp:TextBox ID="txtCVA" runat="server" Text='<%# Bind("cva") %>' class="form-control" Style="text-align: center"></asp:TextBox>
        </div>
    </div>
</div>
                                                                                                                        

                                                            <div class="col-md-2">
    <div class="form-group">
        <span class="details">Veículo Disponível:<asp:Label ID="Label3" runat="server" Text=""></asp:Label></span>
        <div class="input-group">
            <div class="input-group">
                <asp:TextBox ID="txtVeiculoDisponivel" runat="server" TextMode="DateTimeLocal" Text='<%# Eval("disponivel_solicitacao","{0:yyyy-MM-ddTHH:mm}") %>' CssClass="form-control gate" Style="text-align: center"></asp:TextBox>
            </div>

        </div>
        <span class="msg-erro text-danger" style="display: none;"></span>
    </div>
</div>

                                                                                        <div class="col-md-2">
                                                                                            <div class="form-group">
                                                                                                <span class="">Status:</span>
                                                                                                <asp:HiddenField ID="hdfStatus" Value='<%# Eval("status") %>' runat="server" />
                                                                                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                                                                                                </asp:DropDownList>
                                                                                            </div>
                                                                                        </div>

                                                                                        
                                                                                    </div>

                                                                                    <div class="row g-3">
                                                                                        <div class="col-md-12">
                                                                                            <div class="card card-outline card-success">
                                                                                                <div class="card-header">
                                                                                                    <h3 class="card-title"><i class="far fa-edit"></i>&nbsp;Deslocamento</h3>
                                                                                                    <div class="card-tools">
                                                                                                        <button type="button" class="btn btn-tool" data-card-widget="collapse">
                                                                                                            <i class="fas fa-minus"></i>
                                                                                                        </button>
                                                                                                    </div>
                                                                                                    <!-- /.card-tools -->
                                                                                                </div>
                                                                                                <div class="card-body">
                                                                                                    <div class="row g-3">
                                                                                                        <div class="col-md-3">
                                                                                                            <div class="form-group">
                                                                                                                <span class="details">Inicio de Viagem:</span>
                                                                                                                <div class="input-group">
                                                                                                                    <asp:TextBox ID="txtSaidaOrigem" runat="server" CssClass="form-control saida" Text='<%# Bind("saidaorigem", "{0:yyyy-MM-ddTHH:mm}") %>' TextMode="DateTimeLocal" Style="text-align: center" onChange="validarDatas(item)" />
                                                                                                                </div>
                                                                                                                <span class="msg-erro text-danger" style="display: none;"></span>
                                                                                                            </div>
                                                                                                        </div>
                                                                                                        <div class="col-md-3">
                                                                                                            <div class="form-group">
                                                                                                                <span class="details">Previsão de Chegada:</span>
                                                                                                                <div class="input-group">
                                                                                                                    <asp:TextBox ID="txtPrevisaoChegada" runat="server" CssClass="form-control saida" Text='<%# Bind("prev_chegada", "{0:yyyy-MM-ddTHH:mm}") %>' TextMode="DateTimeLocal" Style="text-align: center" onChange="validarDatas(item)" />
                                                                                                                </div>
                                                                                                                <span class="msg-erro text-danger" style="display: none;"></span>
                                                                                                            </div>
                                                                                                        </div>
                                                                                                        <div class="col-md-3">
                                                                                                            <div class="form-group">
                                                                                                                <span class="details">Chegada no Cliente:</span>
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
                                                                                                        <div class="col-md-3">
                                                                                                            <div class="form-group">
                                                                                                                <span class="details">Fim de Viagem:</span>
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
                                                                                                    </div>
                                                                                                    <div class="row g-3">
                                                                                                        <div class="col-md-4">
                                                                                                            <div class="form-group">
                                                                                                                <span class="details" style="text-align: center">DURAÇÃO DA VIAGEM:</span>
                                                                                                                <div class="input-group">
                                                                                                                    <asp:TextBox ID="txtAgCarreg" runat="server" CssClass="form-control espera" Text='<%# Bind("tempoagcarreg") %>' Style="text-align: center" onkeydown="return false;" />
                                                                                                                </div>
                                                                                                                <span class="msg-erro text-danger" style="display: none;"></span>
                                                                                                            </div>
                                                                                                        </div>
                                                                                                        <div class="col-md-4">
                                                                                                            <div class="form-group">
                                                                                                                <span class="details" style="text-align: center">AG. DESCARREGAMENTO:</span>
                                                                                                                <div class="input-group">
                                                                                                                    <asp:TextBox ID="txtAgDescarga" runat="server" CssClass="form-control espera" Text='<%# Bind("tempoagdescarreg") %>' Style="text-align: center" onkeydown="return false;" />
                                                                                                                </div>
                                                                                                                <span class="msg-erro text-danger" style="display: none;"></span>
                                                                                                            </div>
                                                                                                        </div>
                                                                                                        <div class="col-md-4">
                                                                                                            <div class="form-group">
                                                                                                                <span class="details" style="text-align: center">DURAÇÃO DO TRANSPORTE:</span>
                                                                                                                <div class="input-group">
                                                                                                                    <asp:TextBox ID="txtDurTransp" runat="server" CssClass="form-control espera" Text='<%# Bind("duracao_transp") %>' Style="text-align: center" onkeydown="return false;" />
                                                                                                                </div>
                                                                                                                <span class="msg-erro text-danger" style="display: none;"></span>
                                                                                                            </div>
                                                                                                        </div>
                                                                                                    </div>
                                                                                                </div>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="row g-3">
                                                                                        <div class="col-md-10"></div>
                                                                                        <div class="col-md-2">
    <br />
    <asp:Button ID="btnAtualizarColeta" runat="server" Text="Atualizar" CssClass="btn btn-outline-info w-100" CommandName="Atualizar" CommandArgument='<%# Eval("carga") %>' />
</div>
<%--<div class="col-md-1">
    <br />
    <asp:Button ID="WhatsApp" runat="server" Text="WhatsApp" CssClass="btn btn-outline-success w-100" CommandName="Atualizar" CommandArgument='<%# Eval("carga") %>' />
</div>                                                                                                                                                                             <div class="col-md-1">
    <br />
    <asp:Button ID="btnOrdemColeta" runat="server" Text="Impr. O.C." CommandName="Coletas" CommandArgument='<%# Eval("carga") %>' CssClass="btn btn-outline-warning w-100" />

</div>
  --%>                                                                                  </div>
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
                                                </div>
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
                                    

                                    <div class="col-md-3">
                                        <br />
                                        <asp:Button ID="btnEncerrar" CssClass="btn btn-outline-info w-100" runat="server" Text="Finalizar Ordem de Coleta" OnClick="btnEncerrar_Click" />
                                    </div>

                                    <div class="col-md-1">
                                        <br />
                                        <a href="GestaoDeEntregasMatriz.aspx" class="btn btn-outline-danger w-100">Fechar               
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

                    <!-- Modal Bootstrap Incluir Coleta Vazia -->
                    <div class="modal fade" id="meuModal" tabindex="-1" role="dialog" aria-labelledby="meuModalLabel" aria-hidden="true">
                        <div class="modal-dialog modal-dialog-centered modal-xl" role="document">
                            <div class="modal-content">
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <div class="modal-header">
                                            <h5 class="modal-title" id="meuModalLabel">Inclusão de Viagem</h5>
                                            <button type="button" class="close" data-dismiss="modal" aria-label="Fechar">
                                                <span aria-hidden="true">&times;</span>
                                            </button>
                                        </div>
                                        <div class="modal-body">
                                            <div class="row g-3">
                                                <div class="col-md-2">
                                                    <div class="form-group">
                                                        <span class="details">Código:</span>
                                                        <asp:TextBox ID="codCliInicial" runat="server" class="form-control" AutoPostBack="true" OnTextChanged="codCliInicial_TextChanged"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-8">
                                                    <div class="form-group">
                                                        <span class="details">Origem:</span>
                                                        <asp:DropDownList ID="ddlCliInicial" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCliInicial_TextChanged" class="form-control select2"></asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="col-md-2">
                                                    <div class="form-group">
                                                        <span class="details">Carga:</span>
                                                        <asp:TextBox ID="novaCargaVazia" runat="server" Style="text-align: center" class="form-control font-weight-bold" ReadOnly="true" placeholder=""></asp:TextBox>
                                                        <asp:HiddenField ID="hdNovaCarga" runat="server" />
                                                    </div>
                                                </div>
                                            </div>
                                            <!-- colunas ocultas -->
                                            <div class="row g-3">
                                                <div class="col-md-10">
                                                    <div class="form-group">
                                                        <asp:TextBox ID="txtMunicipioOrigem" runat="server" Style="text-align: center" class="form-control font-weight-bold" ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-2">
                                                    <div class="form-group">
                                                        <asp:TextBox ID="txtUfOrigem" runat="server" Style="text-align: center" class="form-control font-weight-bold" ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <!-- fim das colunas ocultas -->
                                            <div class="row g-3">
                                                <div class="col-md-2">
                                                    <div class="form-group">
                                                        <span class="details">Código:</span>
                                                        <asp:TextBox ID="codCliFinal" runat="server" class="form-control" AutoPostBack="true" OnTextChanged="codCliFinal_TextChanged"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-8">
                                                    <div class="form-group">
                                                        <span class="details">Destin:</span>
                                                        <asp:DropDownList ID="ddlCliFinal" runat="server" AutoPostBack="True" class="form-control select2" OnSelectedIndexChanged="ddlCliFinal_TextChanged"></asp:DropDownList>
                                                        <asp:Label ID="lblDistancia" runat="server" Text="" ForeColor="Red" Font-Size="XX-Small"></asp:Label>
                                                    </div>
                                                </div>
                                                <div class="col-md-2">
                                                    <div class="form-group">
                                                        <span class="details">Percurso:</span>
                                                        <asp:TextBox ID="txtDistancia" runat="server" ReadOnly="true" Style="text-align: center" class="form-control font-weight-bold" placeholder=""></asp:TextBox>
                                                    </div>
                                                </div>

                                            </div>
                                            <!-- colunas ocultas -->
                                            <div class="row g-3">
                                                <div class="col-md-10">
                                                    <div class="form-group">
                                                        <asp:TextBox ID="txtMunicipioDestino" runat="server" Style="text-align: center" class="form-control font-weight-bold" ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-2">
                                                    <div class="form-group">
                                                        <asp:TextBox ID="txtUfDestino" runat="server" Style="text-align: center" class="form-control font-weight-bold" ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <!-- fim das colunas ocultas -->
                                            <div class="row g-3">
                                                <div class="col-md-2">
                                                    <div class="form-group">
                                                        <span class="">Viagem:</span>
                                                        <asp:DropDownList ID="ddlTipoMaterial" runat="server" CssClass="form-control">
                                                            <asp:ListItem Value="Almoxarifado" Text="Almoxarifado"></asp:ListItem>
                                                            <asp:ListItem Value="Vazio" Text="Vazio"></asp:ListItem>
                                                            <asp:ListItem Value="Embalagem" Text="Embalagem"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="col-md-1">
                                                    <div class="form-group">
                                                        <span class="details">Peso:</span>
                                                        <asp:TextBox ID="txtPesoVazio" runat="server" Style="text-align: center" class="form-control font-weight-bold"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-1">
                                                    <div class="form-group">
                                                        <span class="details">TT:</span>
                                                        <asp:TextBox ID="txtDuracaoVazio" runat="server" Style="text-align: center" class="form-control font-weight-bold"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-2">
                                                    <div class="form-group">
                                                        <span class="details">Código:</span>
                                                        <asp:TextBox ID="txtCod_PagadorVazio" runat="server" class="form-control" AutoPostBack="true" OnTextChanged="txtCod_PagadorVazio_TextChanged"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <span class="details">Pagador:</span>
                                                        <asp:TextBox ID="txtPagadorVazio" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <asp:Label ID="Label1" runat="server" Text="" ForeColor="Red" Font-Size="XX-Small"></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                            <!-- colunas ocultas -->
                                            <div class="row g-3" id="linhaPagadorCidadeUF" runat="server" visible="false">
                                                <div class="col-md-10">
                                                    <div class="form-group">
                                                        <asp:TextBox ID="txtCid_PagadorVazio" runat="server" Style="text-align: center" class="form-control font-weight-bold" ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-2">
                                                    <div class="form-group">
                                                        <asp:TextBox ID="txtUf_PagadorVazio" runat="server" Style="text-align: center" class="form-control font-weight-bold" ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <!-- fim das colunas ocultas -->
                                        </div>
                                        <div class="modal-footer">
                                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Fechar</button>
                                            <asp:Button ID="btnSalvarColeta" runat="server" Text="Salvar" class="btn btn-primary" OnClick="btnSalvarColeta_Click" />
                                        </div>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="btnSalvarColeta" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>
</asp:Content>

