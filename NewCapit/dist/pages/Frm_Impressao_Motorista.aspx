<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Frm_Impressao_Motorista.aspx.cs" Inherits="NewCapit.dist.pages.Frm_Impressao_Motorista" Async="true" %>

<html lang="pt-BR">

<head>
    <meta charset="UTF-8">
   
    <title>Avaliação de Desempenho e Qualidade</title>
    <style>
    * { margin: 0; padding: 0; box-sizing: border-box; }

    body {
        font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
        background: linear-gradient(to bottom right, #eff6ff, #dbeafe);
        padding: 20px;
        min-height: 100vh;
    }

    .container { max-width: 100%; margin: 0 auto; }

    /* Header */
    .header, .table-container, .observacoes-container, .legenda-box {
        background: white;
        border-radius: 8px;
        box-shadow: 0 2px 8px rgba(0,0,0,0.1);
        padding: 20px;
        margin-bottom: 20px;
    }

    .header { padding: 30px; }
    .header-top { display: flex; justify-content: space-between; gap: 20px; margin-bottom: 20px; }
    .header-left { display: flex; gap: 20px; }
    .foto-placeholder {
        width: 80px; height: 80px;
        background: #e5e7eb; border: 2px solid #d1d5db; border-radius: 4px;
        display: flex; align-items: center; justify-content: center;
        color: #9ca3af; font-size: 12px; cursor: pointer;
        transition: 0.3s;
    }
    .foto-placeholder:hover { background: #d1d5db; }

    .header-title h1 { font-size: 28px; font-weight: 700; color: #1f2937; margin-bottom: 5px; }
    .header-title p { font-size: 16px; color: #4b5563; font-weight: 600; }

    .header-buttons { display: flex; gap: 10px; }

    .btn {
        padding: 8px 16px; border: 1px solid #d1d5db; background: white;
        border-radius: 6px; font-size: 14px; cursor: pointer;
        color: #374151; font-weight: 500; display: flex; gap: 8px;
        transition: 0.3s; align-items: center;
    }
    .btn:hover { background: #f3f4f6; border-color: #9ca3af; }

    /* Motorista Info */
    .motorista-info {
        background: #2563eb; color: white; padding: 20px; border-radius: 6px;
        margin-top: 20px; gap: 20px;
        display: grid; grid-template-columns: repeat(auto-fit, minmax(150px,1fr));
    }
    .motorista-info-label { font-size: 11px; font-weight: 700; margin-bottom: 5px; opacity: .9; }
    .motorista-info-value { font-size: 14px; font-weight: 700; }

    /* Tabela */
    table { width: 100%; border-collapse: collapse; }
    thead { background: #2563eb; color: white; }
    thead tr:first-child th { padding: 15px; font-size: 13px; }
    thead tr:last-child { background: #1d4ed8; }
    thead tr:last-child th { padding: 10px; font-size: 12px; text-align: center; }

    tbody tr:nth-child(odd) { background: #dbeafe; }
    tbody tr:nth-child(even) { background: #eff6ff; }
    tbody tr:hover { background: #bfdbfe; }
    tbody td { padding: 15px; font-size: 13px; }

    .item-name {
        background: #60a5fa; color: white; font-weight: 600; min-width: 120px;
    }
    .item-descricao { font-style: italic; color: #374151; }
    .item-peso { font-weight: 700; text-align: center; min-width: 60px; color: #1f2937; }
    .checkbox-cell { text-align: center; min-width: 50px; }
    .checkbox-cell input { width: 18px; height: 18px; cursor: pointer; }

    /* Observações */
    .observacoes-label {
        background: #fbbf24; color: #1f2937; padding: 8px 12px;
        border-radius: 4px; font-size: 13px; font-weight: 700; margin-bottom: 15px; display: inline-block;
    }
    .observacoes-textarea {
        width: 100%; height: 120px; padding: 12px;
        border: 2px solid #e5e7eb; border-radius: 6px;
        font-size: 13px; resize: vertical; transition: .3s;
        font-family: inherit;
    }
    .observacoes-textarea:focus {
        outline: none; border-color: #2563eb;
        box-shadow: 0 0 0 3px rgba(37,99,235,0.1);
    }

    /* Legenda e Bônus */
    .footer-section {
        display: grid; gap: 20px;
        grid-template-columns: repeat(auto-fit, minmax(200px,1fr));
    }
    .legenda-box h3 { font-size: 13px; font-weight: 700; margin-bottom: 12px; color: #4b5563; }
    .legenda-item { font-size: 12px; color: #374151; margin-bottom: 8px; line-height: 1.6; }

    .bonus-box {
        background: #2563eb; color: white; border-radius: 8px; padding: 20px;
        display: flex; align-items: center; justify-content: center; flex-direction: column;
        box-shadow: 0 2px 8px rgba(0,0,0,0.1);
    }
    .bonus-label { font-size: 14px; font-weight: 700; margin-bottom: 10px; }
    .bonus-value { font-size: 36px; font-weight: 700; color: #fbbf24; }

    /* Responsivo */
    @media(max-width:768px){
        .header-top{ flex-direction: column; }
        .header-buttons{ flex-wrap: wrap; width: 100%; }
        .motorista-info{ grid-template-columns: repeat(2,1fr); }
        .btn{ flex: 1; justify-content: center; }
        tbody td{ padding: 10px; }
        .footer-section{ grid-template-columns: 1fr; }
    }

    @media(max-width:480px){
        .header{ padding: 15px; }
        .header-title h1{ font-size: 20px; }
        .motorista-info{ grid-template-columns: 1fr; }
        .header-buttons{ flex-direction: column; }
        .btn{ width: 100%; }
    }

    /* Impressão */
   @media print {

    @page {
        size: A4 portrait;
        margin: 8mm; /* margem mínima segura */
    }

    body {
        background: white !important;
        padding: 0 !important;
        margin: 0 !important;
        -webkit-print-color-adjust: exact;
        print-color-adjust: exact;
        zoom: 89%; /* 🔥 A CHAVE PARA CABER EM 1 PÁGINA */
    }

    .header,
    .table-container,
    .observacoes-container,
    .legenda-box,
    .bonus-box {
        box-shadow: none !important;
        padding: 10px !important;
        margin-bottom: 10px !important;
    }

    /* Reduz altura da área do motorista */
    .motorista-info {
        padding: 10px !important;
        gap: 10px !important;
        grid-template-columns: repeat(3, 1fr) !important;
    }

    .motorista-info-label {
        font-size: 9px !important;
    }

    .motorista-info-value {
        font-size: 12px !important;
    }

    table {
        font-size: 10px !important;
    }

    thead th {
        padding: 6px !important;
        font-size: 10px !important;
    }

    tbody td {
        padding: 7px !important;
    }

    .observacoes-textarea {
        height: 60px !important;
    }

    /* Bônus */
    .bonus-value {
        font-size: 24px !important;
    }

    /* Botões e elementos desnecessários */
    .header-buttons,
    .btn,
    .no-print {
        display: none !important;
    }
    .assinatura-container {
    width: 100%;
    text-align: center;
    margin-top: 40px;
        }

        .linha-assinatura {
            width: 300px;          /* tamanho da linha */
            border-bottom: 1px solid #000;
            margin: 0 auto;        /* centraliza */
            height: 40px;          /* cria espaço para “assinar” */
        }

        .assinatura-nome {
            margin-top: 5px;
            font-weight: bold;
            font-size: 14px;
        }
    }
</style>
    
</head>
<body>
    <form runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        
   <div class="container">
       <asp:Repeater ID="Repeater1" OnItemDataBound="Repeater1_ItemDataBound" runat="server">
           <ItemTemplate>
    <!-- Header -->
    <div class="header">
        <div class="header-top">
            <div class="header-left">
              <%--  <div class="foto-placeholder">
                    <img src="<%=fotoMotorista%>" class="rounded-circle float-center" height="80" width="80" alt="User Image">
                </div>--%>
                <div class="header-title">
                    <h1>TRANSNOVAG</h1>
                    <p>AVALIAÇÃO DE DESEMPENHO E QUALIDADE</p>
                </div>
            </div>
            <div class="header-buttons">
                <button class="btn" onclick="window.print()">
                    <span>🖨️</span> Imprimir
                </button>
                   
                      
              
                
            </div>
        </div>
       
        <!-- Dados do Motorista -->
    <div class="motorista-info">
              <div class="foto-placeholder">
                  <asp:Image ID="imgMotorista" runat="server" CssClass="rounded-circle float-center"
           Height="80" Width="80" AlternateText="User Image" />
              </div>
            <div class="motorista-info-item">
                <span class="motorista-info-label">CHAPA/RE:</span>
                <span class="motorista-info-value"><asp:Label ID="lblCracha" runat="server" Text=""></asp:Label></span>
            </div>
            <div class="motorista-info-item">
                <span class="motorista-info-label">NOME DO MOTORISTA:</span>
                <span class="motorista-info-value"><asp:Label ID="lblNomeMot" runat="server" Text=""></asp:Label></span>
            </div>
            <div class="motorista-info-item">
                <span class="motorista-info-label">FUNÇÃO:</span>
                <span class="motorista-info-value"><asp:Label ID="lblFuncao" runat="server" Text=""></asp:Label></span>
            </div>
            <div class="motorista-info-item">
                <span class="motorista-info-label">ADMISSÃO:</span>
                <span class="motorista-info-value"><asp:Label ID="lblDtAdmissao" runat="server" Text=""></asp:Label></span>
            </div>
            <div class="motorista-info-item">
                <span class="motorista-info-label">NÚCLEO:</span>
                <span class="motorista-info-value"><asp:Label ID="lblNucleo" runat="server" Text=""></asp:Label></span>
            </div>
            <div class="motorista-info-item">
                <span class="motorista-info-label">FROTA:</span>
                <span class="motorista-info-value"><asp:Label ID="lblFrota" runat="server" Text=""></asp:Label></span>
            </div>
            <div class="motorista-info-item">
                <span class="motorista-info-label">MÊS:</span>
                <span class="motorista-info-value"><asp:Label ID="lblMes" runat="server" Text=""></asp:Label></span>
            </div>
            <div class="motorista-info-item">
                <span class="motorista-info-label">AVALIADO POR:</span>
                <span class="motorista-info-value"><asp:Label ID="lblUsuario" runat="server" Text=""></asp:Label></span>
        </div>
        </div>
    </div>

    
    <!-- Tabela de Avaliação -->
    <div runat="server" class="table-container">
    <table>
        <thead>
            <tr>
                <th>ITEM</th>
                <th>DESCRIÇÃO</th>
                <th>PESO</th>
                <th colspan="3" style="text-align: right;">AVALIAÇÃO</th>
            </tr>
            <tr>
                <th colspan="2"></th>
                <th>PESO</th>
                <th>1</th>
                <th>2</th>
                <th>3</th>
            </tr>
        </thead>
        <tbody>

            <!-- DOCUMENTAÇÃO -->
            <tr>
                <td class="item-name">Documentação</td>
                <td class="item-descricao">Documentos preenchidos corretamente...</td>
                <td class="item-peso"><asp:Label ID="lblPesoDocumentacao" runat="server" Text="4%"></asp:Label></td>
                <td class="checkbox-cell"><asp:RadioButton ID="rb_documentacao_1" GroupName="documentacao" runat="server" Enabled="false" /></td>
                <td class="checkbox-cell"><asp:RadioButton ID="rb_documentacao_2" GroupName="documentacao" runat="server" Enabled="false" /></td>
                <td class="checkbox-cell"><asp:RadioButton ID="rb_documentacao_3" GroupName="documentacao" runat="server" Enabled="false" /></td>
            </tr>

            <!-- PONTUALIDADE -->
            <tr>
                <td class="item-name">Pontualidade</td>
                <td class="item-descricao">Cumprimento dos horários estabelecidos...</td>
                <td class="item-peso"><asp:Label ID="lblPesoPontualidade" runat="server" Text="2%"></asp:Label></td>
                <td class="checkbox-cell"><asp:RadioButton ID="rb_pontualidade_1" GroupName="pontualidade" runat="server" Enabled="false" /></td>
                <td class="checkbox-cell"><asp:RadioButton ID="rb_pontualidade_2" GroupName="pontualidade" runat="server" Enabled="false" /></td>
                <td class="checkbox-cell"><asp:RadioButton ID="rb_pontualidade_3" GroupName="pontualidade" runat="server" Enabled="false" /></td>
            </tr>

            <!-- SEGURANÇA DA CARGA -->
            <tr>
                <td class="item-name">Segurança da Carga</td>
                <td class="item-descricao">Distribuir, Cimar, Amarrar...</td>
                <td class="item-peso"><asp:Label ID="lblPesoSegurancaCarga" runat="server" Text="2%"></asp:Label></td>
                <td class="checkbox-cell"><asp:RadioButton ID="rb_seguranca_1" GroupName="seguranca" runat="server" Enabled="false" /></td>
                <td class="checkbox-cell"><asp:RadioButton ID="rb_seguranca_2" GroupName="seguranca" runat="server" Enabled="false" /></td>
                <td class="checkbox-cell"><asp:RadioButton ID="rb_seguranca_3" GroupName="seguranca" runat="server" Enabled="false" /></td>
            </tr>

            <!-- CARGA E DESCARGA -->
            <tr>
                <td class="item-name">Carga e Descarga</td>
                <td class="item-descricao">Conteúr carga e descarga...</td>
                <td class="item-peso"><asp:Label ID="lblPesoCargaDescarga" runat="server" Text="2%"></asp:Label></td>
                <td class="checkbox-cell"><asp:RadioButton ID="rb_cargadescarga_1" GroupName="cargadescarga" runat="server" Enabled="false" /></td>
                <td class="checkbox-cell"><asp:RadioButton ID="rb_cargadescarga_2" GroupName="cargadescarga" runat="server" Enabled="false" /></td>
                <td class="checkbox-cell"><asp:RadioButton ID="rb_cargadescarga_3" GroupName="cargadescarga" runat="server" Enabled="false" /></td>
            </tr>

            <!-- COMUNICAÇÃO -->
            <tr>
                <td class="item-name">Comunicação</td>
                <td class="item-descricao">Ligar para a programação...</td>
                <td class="item-peso"><asp:Label ID="lblPesoComunicacao" runat="server" Text="1%"></asp:Label></td>
                <td class="checkbox-cell"><asp:RadioButton ID="rb_comunicacao_1" GroupName="comunicacao" runat="server" Enabled="false" /></td>
                <td class="checkbox-cell"><asp:RadioButton ID="rb_comunicacao_2" GroupName="comunicacao" runat="server" Enabled="false" /></td>
                <td class="checkbox-cell"><asp:RadioButton ID="rb_comunicacao_3" GroupName="comunicacao" runat="server" Enabled="false" /></td>
            </tr>

            <!-- SEGURANÇA NO TRÂNSITO -->
            <tr>
                <td class="item-name">Segurança no Trânsito</td>
                <td class="item-descricao">Envolvimento em acidentes...</td>
                <td class="item-peso"><asp:Label ID="lblPesoSegurancaTransito" runat="server" Text="3%"></asp:Label></td>
                <td class="checkbox-cell"><asp:RadioButton ID="rb_transito_1" GroupName="transito" runat="server" Enabled="false" /></td>
                <td class="checkbox-cell"><asp:RadioButton ID="rb_transito_2" GroupName="transito" runat="server" Enabled="false" /></td>
                <td class="checkbox-cell"><asp:RadioButton ID="rb_transito_3" GroupName="transito" runat="server" Enabled="false" /></td>
            </tr>

            <!-- CONSUMO DE COMBUSTÍVEL -->
            <tr>
                <td class="item-name">Consumo de Combustível</td>
                <td class="item-descricao">Atingir a meta da média...</td>
                <td class="item-peso"><asp:Label ID="lblPesoConsumoCombustivel" runat="server" Text="3%"></asp:Label></td>
                <td class="checkbox-cell"><asp:RadioButton ID="rb_combustivel_1" GroupName="combustivel" runat="server" Enabled="false" /></td>
                <td class="checkbox-cell"><asp:RadioButton ID="rb_combustivel_2" GroupName="combustivel" runat="server" Enabled="false" /></td>
                <td class="checkbox-cell"><asp:RadioButton ID="rb_combustivel_3" GroupName="combustivel" runat="server"  Enabled="false"/></td>
            </tr>

            <!-- CONSERVAÇÃO DO VEÍCULO -->
            <tr>
                <td class="item-name">Conservação do Veículo</td>
                <td class="item-descricao">Conservação dos acessórios...</td>
                <td class="item-peso"><asp:Label ID="lblPesoConservacaoVeiculo" runat="server" Text="3%"></asp:Label></td>
                <td class="checkbox-cell"><asp:RadioButton ID="rb_conservacao_1" GroupName="conservacao" runat="server" Enabled="false" /></td>
                <td class="checkbox-cell"><asp:RadioButton ID="rb_conservacao_2" GroupName="conservacao" runat="server" Enabled="false" /></td>
                <td class="checkbox-cell"><asp:RadioButton ID="rb_conservacao_3" GroupName="conservacao" runat="server" Enabled="false" /></td>
            </tr>

        </tbody>
    </table>
</div>


    <!-- Observações -->
    <div class="observacoes-container">
        <div class="observacoes-label">OBSERVAÇÕES:</div>
        <div>
        <asp:TextBox ID="txtObs" placeholder="Digite aqui as observações sobre a avaliação..." CssClass="observacoes-textarea" TextMode="MultiLine" runat="server"></asp:TextBox>
       </div>
    </div>

    <!-- Legenda e Bônus -->
    <div class="footer-section">
        <div class="legenda-box">
            <h3>Legenda:</h3>
            <div class="legenda-item"><strong>Peso</strong></div>
            <div class="legenda-item">1 = Paga 0%</div>
            <div class="legenda-item">2 = Paga 50%</div>
            <div class="legenda-item">3 = Paga 100%</div>
        </div>
        <div></div>
        <div></div>
        <div class="bonus-box">
            <div class="bonus-label">BÔNUS</div>
            <div class="bonus-value"><asp:Label ID="lblResultadoTotal" runat="server" Text=""></asp:Label></div>
        </div>
    </div>

    <div class="assinatura-container">
                    <div class="linha-assinatura"></div>
                    <div class="assinatura-nome"><asp:Label ID="lblNomeAss" runat="server" Text=""></asp:Label></div>
    </div>
               </ItemTemplate>
          </asp:Repeater>    
</div>
         
    </form>
<script>
    function downloadHTML() {
        const html = document.documentElement.outerHTML;
        const blob = new Blob([html], { type: 'text/html' });
        const url = URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = 'avaliacao_desempenho.html';
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
        URL.revokeObjectURL(url);
    }

    // Funcionalidade de checkboxes exclusivos (apenas um por linha)
    document.querySelectorAll('tbody tr').forEach(row => {
        const checkboxes = row.querySelectorAll('input[type="checkbox"]');
        checkboxes.forEach(checkbox => {
            checkbox.addEventListener('change', function() {
                if (this.checked) {
                    checkboxes.forEach(cb => {
                        if (cb !== this) cb.checked = false;
                    });
                }
            });
        });
    });
</script>
</body>
</html>
