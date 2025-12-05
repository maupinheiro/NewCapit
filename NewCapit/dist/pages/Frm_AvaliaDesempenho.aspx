<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Frm_AvaliaDesempenho.aspx.cs" Inherits="NewCapit.dist.pages.Frm_AvaliaDesempenho" %>

<html lang="pt-BR">

<head>
    <meta charset="UTF-8">
   
    <title>Avaliação de Desempenho e Qualidade</title>
    <style>
        * {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }

        body {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background: linear-gradient(to bottom right, #eff6ff, #dbeafe);
            padding: 20px;
            min-height: 100vh;
        }

        .container {
            max-width: 100%;
            margin: 0 auto;
        }

        /* Header */
        .header {
            background: white;
            border-radius: 8px;
            box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
            padding: 30px;
            margin-bottom: 20px;
        }

        .header-top {
            display: flex;
            justify-content: space-between;
            align-items: flex-start;
            margin-bottom: 20px;
            gap: 20px;
        }

        .header-left {
            display: flex;
            gap: 20px;
            align-items: flex-start;
        }

        .foto-placeholder {
            width: 80px;
            height: 80px;
            background-color: #e5e7eb;
            border: 2px solid #d1d5db;
            border-radius: 4px;
            display: flex;
            align-items: center;
            justify-content: center;
            color: #9ca3af;
            font-size: 12px;
            cursor: pointer;
            transition: all 0.3s ease;
        }

        .foto-placeholder:hover {
            background-color: #d1d5db;
        }

        .header-title {
            flex: 1;
        }

        .header-title h1 {
            font-size: 28px;
            font-weight: bold;
            color: #1f2937;
            margin-bottom: 5px;
        }

        .header-title p {
            font-size: 16px;
            color: #4b5563;
            font-weight: 600;
        }

        .header-buttons {
            display: flex;
            gap: 10px;
        }

        .btn {
            padding: 8px 16px;
            border: 1px solid #d1d5db;
            background: white;
            color: #374151;
            border-radius: 6px;
            cursor: pointer;
            font-size: 14px;
            font-weight: 500;
            transition: all 0.3s ease;
            display: flex;
            align-items: center;
            gap: 8px;
        }

        .btn:hover {
            background-color: #f3f4f6;
            border-color: #9ca3af;
        }

        /* Dados do Motorista */
        .motorista-info {
            background: #2563eb;
            color: white;
            padding: 20px;
            border-radius: 6px;
            margin-top: 20px;
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(150px, 1fr));
            gap: 20px;
        }

        .motorista-info-item {
            display: flex;
            flex-direction: column;
        }

        .motorista-info-label {
            font-size: 11px;
            font-weight: 700;
            letter-spacing: 0.5px;
            margin-bottom: 5px;
            opacity: 0.9;
        }

        .motorista-info-value {
            font-size: 14px;
            font-weight: bold;
        }

        /* Tabela */
        .table-container {
            background: white;
            border-radius: 8px;
            box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
            overflow: hidden;
            margin-bottom: 20px;
        }

        table {
            width: 100%;
            border-collapse: collapse;
        }

        thead {
            background: #2563eb;
            color: white;
        }

        thead tr:first-child th {
            padding: 15px;
            text-align: left;
            font-size: 13px;
            font-weight: 600;
        }

        thead tr:last-child {
            background: #1d4ed8;
        }

        thead tr:last-child th {
            padding: 10px;
            text-align: center;
            font-size: 12px;
            font-weight: 600;
        }

        tbody tr {
            border-bottom: 1px solid #e5e7eb;
        }

        tbody tr:nth-child(odd) {
            background-color: #dbeafe;
        }

        tbody tr:nth-child(even) {
            background-color: #eff6ff;
        }

        tbody tr:hover {
            background-color: #bfdbfe;
        }

        tbody td {
            padding: 15px;
            font-size: 13px;
        }

        .item-name {
            background-color: #60a5fa;
            color: white;
            font-weight: 600;
            min-width: 120px;
        }

        .item-descricao {
            font-style: italic;
            color: #374151;
        }

        .item-peso {
            text-align: center;
            font-weight: 600;
            color: #1f2937;
            min-width: 60px;
        }

        .checkbox-cell {
            text-align: center;
            min-width: 50px;
        }

        .checkbox-cell input[type="checkbox"] {
            width: 18px;
            height: 18px;
            cursor: pointer;
        }

        /* Observações */
        .observacoes-container {
            background: white;
            border-radius: 8px;
            box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
            padding: 20px;
            margin-bottom: 20px;
        }

        .observacoes-label {
            background-color: #fbbf24;
            color: #1f2937;
            padding: 8px 12px;
            border-radius: 4px;
            font-weight: 700;
            font-size: 13px;
            margin-bottom: 15px;
            display: inline-block;
        }

        .observacoes-textarea {
            width: 100%;
            height: 120px;
            padding: 12px;
            border: 2px solid #e5e7eb;
            border-radius: 6px;
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            font-size: 13px;
            resize: vertical;
            transition: border-color 0.3s ease;
        }

        .observacoes-textarea:focus {
            outline: none;
            border-color: #2563eb;
            box-shadow: 0 0 0 3px rgba(37, 99, 235, 0.1);
        }

        /* Legenda e Bônus */
        .footer-section {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
            gap: 20px;
        }

        .legenda-box {
            background: white;
            border-radius: 8px;
            padding: 20px;
            box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
        }

        .legenda-box h3 {
            font-size: 13px;
            font-weight: 700;
            color: #4b5563;
            margin-bottom: 12px;
        }

        .legenda-item {
            font-size: 12px;
            color: #374151;
            margin-bottom: 8px;
            line-height: 1.6;
        }

        .bonus-box {
            background: #2563eb;
            color: white;
            border-radius: 8px;
            padding: 20px;
            box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
            display: flex;
            flex-direction: column;
            justify-content: center;
            align-items: center;
        }

        .bonus-label {
            font-size: 14px;
            font-weight: 700;
            margin-bottom: 10px;
        }

        .bonus-value {
            font-size: 36px;
            font-weight: bold;
            color: #fbbf24;
        }

        /* Responsivo */
        @media (max-width: 768px) {
            .header-top {
                flex-direction: column;
            }

            .header-buttons {
                width: 100%;
                flex-wrap: wrap;
            }

            .motorista-info {
                grid-template-columns: repeat(2, 1fr);
                gap: 15px;
            }

            .btn {
                flex: 1;
                justify-content: center;
            }

            table {
                font-size: 12px;
            }

            tbody td {
                padding: 10px;
            }

            .footer-section {
                grid-template-columns: 1fr;
            }
        }

        @media (max-width: 480px) {
            .header {
                padding: 15px;
            }

            .header-title h1 {
                font-size: 20px;
            }

            .motorista-info {
                grid-template-columns: 1fr;
            }

            .header-buttons {
                flex-direction: column;
            }

            .btn {
                width: 100%;
            }
        }

        /* Impressão */
        @media print {
            body {
                background: white;
                padding: 0;
            }

            .header-buttons {
                display: none;
            }

            .container {
                max-width: 100%;
            }
        }
    </style>
</head>
<body>
    <form runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        
   <div class="container">
      
    <!-- Header -->
    <div class="header">
        <div class="header-top">
            <div class="header-left">
                <div class="foto-placeholder">
                    <img src="<%=fotoMotorista%>" class="rounded-circle float-center" height="80" width="80" alt="User Image">
                </div>
                <div class="header-title">
                    <h1>TRANSNOVAG</h1>
                    <p>AVALIAÇÃO DE DESEMPENHO E QUALIDADE</p>
                </div>
            </div>
            <div class="header-buttons">
                <button class="btn" onclick="window.print()">
                    <span>🖨️</span> Imprimir
                </button>
                    <asp:LinkButton ID="lnkSalvar" CssClass="btn" OnClick="lnkSalvar_Click" runat="server">Salvar</asp:LinkButton>
                <button class="btn" onclick="downloadHTML()">
                    <span>⬇️</span> Baixar
                </button>
                <
            </div>
        </div>

        <!-- Dados do Motorista -->
        <div class="motorista-info">
            <div class="motorista-info-item">
                <span class="motorista-info-label">CHAPA/RH:</span>
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
        </div>
    </div>

     <asp:UpdatePanel ID="UpdatePanel1" runat="server">
     <ContentTemplate>
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
                <td class="checkbox-cell"><asp:RadioButton ID="rb_documentacao_1" GroupName="documentacao" runat="server" AutoPostBack="true" OnCheckedChanged="Recalcular" /></td>
                <td class="checkbox-cell"><asp:RadioButton ID="rb_documentacao_2" GroupName="documentacao" runat="server" AutoPostBack="true" OnCheckedChanged="Recalcular" /></td>
                <td class="checkbox-cell"><asp:RadioButton ID="rb_documentacao_3" GroupName="documentacao" runat="server" AutoPostBack="true" OnCheckedChanged="Recalcular" /></td>
            </tr>

            <!-- PONTUALIDADE -->
            <tr>
                <td class="item-name">Pontualidade</td>
                <td class="item-descricao">Cumprimento dos horários estabelecidos...</td>
                <td class="item-peso"><asp:Label ID="lblPesoPontualidade" runat="server" Text="2%"></asp:Label></td>
                <td class="checkbox-cell"><asp:RadioButton ID="rb_pontualidade_1" GroupName="pontualidade" runat="server" AutoPostBack="true" OnCheckedChanged="Recalcular" /></td>
                <td class="checkbox-cell"><asp:RadioButton ID="rb_pontualidade_2" GroupName="pontualidade" runat="server" AutoPostBack="true" OnCheckedChanged="Recalcular" /></td>
                <td class="checkbox-cell"><asp:RadioButton ID="rb_pontualidade_3" GroupName="pontualidade" runat="server" AutoPostBack="true" OnCheckedChanged="Recalcular" /></td>
            </tr>

            <!-- SEGURANÇA DA CARGA -->
            <tr>
                <td class="item-name">Segurança da Carga</td>
                <td class="item-descricao">Distribuir, Cimar, Amarrar...</td>
                <td class="item-peso"><asp:Label ID="lblPesoSegurancaCarga" runat="server" Text="2%"></asp:Label></td>
                <td class="checkbox-cell"><asp:RadioButton ID="rb_seguranca_1" GroupName="seguranca" runat="server" AutoPostBack="true" OnCheckedChanged="Recalcular" /></td>
                <td class="checkbox-cell"><asp:RadioButton ID="rb_seguranca_2" GroupName="seguranca" runat="server" AutoPostBack="true" OnCheckedChanged="Recalcular" /></td>
                <td class="checkbox-cell"><asp:RadioButton ID="rb_seguranca_3" GroupName="seguranca" runat="server" AutoPostBack="true" OnCheckedChanged="Recalcular" /></td>
            </tr>

            <!-- CARGA E DESCARGA -->
            <tr>
                <td class="item-name">Carga e Descarga</td>
                <td class="item-descricao">Conteúr carga e descarga...</td>
                <td class="item-peso"><asp:Label ID="lblPesoCargaDescarga" runat="server" Text="2%"></asp:Label></td>
                <td class="checkbox-cell"><asp:RadioButton ID="rb_cargadescarga_1" GroupName="cargadescarga" runat="server" AutoPostBack="true" OnCheckedChanged="Recalcular" /></td>
                <td class="checkbox-cell"><asp:RadioButton ID="rb_cargadescarga_2" GroupName="cargadescarga" runat="server" AutoPostBack="true" OnCheckedChanged="Recalcular" /></td>
                <td class="checkbox-cell"><asp:RadioButton ID="rb_cargadescarga_3" GroupName="cargadescarga" runat="server" AutoPostBack="true" OnCheckedChanged="Recalcular" /></td>
            </tr>

            <!-- COMUNICAÇÃO -->
            <tr>
                <td class="item-name">Comunicação</td>
                <td class="item-descricao">Ligar para a programação...</td>
                <td class="item-peso"><asp:Label ID="lblPesoComunicacao" runat="server" Text="1%"></asp:Label></td>
                <td class="checkbox-cell"><asp:RadioButton ID="rb_comunicacao_1" GroupName="comunicacao" runat="server" AutoPostBack="true" OnCheckedChanged="Recalcular" /></td>
                <td class="checkbox-cell"><asp:RadioButton ID="rb_comunicacao_2" GroupName="comunicacao" runat="server" AutoPostBack="true" OnCheckedChanged="Recalcular" /></td>
                <td class="checkbox-cell"><asp:RadioButton ID="rb_comunicacao_3" GroupName="comunicacao" runat="server" AutoPostBack="true" OnCheckedChanged="Recalcular" /></td>
            </tr>

            <!-- SEGURANÇA NO TRÂNSITO -->
            <tr>
                <td class="item-name">Segurança no Trânsito</td>
                <td class="item-descricao">Envolvimento em acidentes...</td>
                <td class="item-peso"><asp:Label ID="lblPesoSegurancaTransito" runat="server" Text="3%"></asp:Label></td>
                <td class="checkbox-cell"><asp:RadioButton ID="rb_transito_1" GroupName="transito" runat="server" AutoPostBack="true" OnCheckedChanged="Recalcular" /></td>
                <td class="checkbox-cell"><asp:RadioButton ID="rb_transito_2" GroupName="transito" runat="server" AutoPostBack="true" OnCheckedChanged="Recalcular" /></td>
                <td class="checkbox-cell"><asp:RadioButton ID="rb_transito_3" GroupName="transito" runat="server" AutoPostBack="true" OnCheckedChanged="Recalcular" /></td>
            </tr>

            <!-- CONSUMO DE COMBUSTÍVEL -->
            <tr>
                <td class="item-name">Consumo de Combustível</td>
                <td class="item-descricao">Atingir a meta da média...</td>
                <td class="item-peso"><asp:Label ID="lblPesoConsumoCombustivel" runat="server" Text="3%"></asp:Label></td>
                <td class="checkbox-cell"><asp:RadioButton ID="rb_combustivel_1" GroupName="combustivel" runat="server" AutoPostBack="true" OnCheckedChanged="Recalcular" /></td>
                <td class="checkbox-cell"><asp:RadioButton ID="rb_combustivel_2" GroupName="combustivel" runat="server" AutoPostBack="true" OnCheckedChanged="Recalcular" /></td>
                <td class="checkbox-cell"><asp:RadioButton ID="rb_combustivel_3" GroupName="combustivel" runat="server"  AutoPostBack="true" OnCheckedChanged="Recalcular"/></td>
            </tr>

            <!-- CONSERVAÇÃO DO VEÍCULO -->
            <tr>
                <td class="item-name">Conservação do Veículo</td>
                <td class="item-descricao">Conservação dos acessórios...</td>
                <td class="item-peso"><asp:Label ID="lblPesoConservacaoVeiculo" runat="server" Text="3%"></asp:Label></td>
                <td class="checkbox-cell"><asp:RadioButton ID="rb_conservacao_1" GroupName="conservacao" runat="server" AutoPostBack="true" OnCheckedChanged="Recalcular" /></td>
                <td class="checkbox-cell"><asp:RadioButton ID="rb_conservacao_2" GroupName="conservacao" runat="server" AutoPostBack="true" OnCheckedChanged="Recalcular" /></td>
                <td class="checkbox-cell"><asp:RadioButton ID="rb_conservacao_3" GroupName="conservacao" runat="server" AutoPostBack="true" OnCheckedChanged="Recalcular" /></td>
            </tr>

        </tbody>
    </table>
</div>


    <!-- Observações -->
    <div class="observacoes-container">
        <div class="observacoes-label">OBSERVAÇÕES:</div>
        <div>
        <asp:TextBox ID="txtObs" placeholder="Digite aqui as observações sobre a avaliação..." Width="850" Height="200" TextMode="MultiLine" runat="server"></asp:TextBox>
       </div>
    </div>

    <!-- Legenda e Bônus -->
    <div class="footer-section">
        <div class="legenda-box">
            <h3>Legenda:</h3>
            <div class="legenda-item"><strong>Peso</strong></div>
            <div class="legenda-item">1 = Página 0%</div>
            <div class="legenda-item">2 = Página 50%</div>
            <div class="legenda-item">3 = Página 100%</div>
        </div>
        <div></div>
        <div></div>
        <div class="bonus-box">
            <div class="bonus-label">BÔNUS</div>
            <div class="bonus-value"><asp:Label ID="lblResultadoTotal" runat="server" Text=""></asp:Label></div>
        </div>
    </div>
               </ContentTemplate>
   </asp:UpdatePanel>
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
