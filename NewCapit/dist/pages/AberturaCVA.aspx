<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="AberturaCVA.aspx.cs" Inherits="NewCapit.dist.pages.AberturaCVA" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

  <style>

        /* =========================================================
           TELA
        ========================================================= */

        .solicitacao-page {
            padding: 15px;
        }

        /* =========================================================
           CARD SAP
        ========================================================= */

        .erp-card {
            background: #ffffff;
            border: 1px solid #d9e2ec;
            border-radius: 6px;
            margin-bottom: 15px;
            box-shadow: 0 2px 5px rgba(0,0,0,.08);
        }

        .erp-card-header {
            background: #0a6ed1;
            color: #ffffff;
            padding: 10px 15px;
            font-weight: 600;
            font-size: 14px;
            border-radius: 6px 6px 0 0;
        }

        .erp-card-header i {
            margin-right: 7px;
        }

        .erp-card-body {
            padding: 15px;
        }

        /* =========================================================
           LABEL
        ========================================================= */

        .form-label-sap {
            font-size: 12px;
            font-weight: 600;
            color: #43536a;
            margin-bottom: 3px;
        }

        /* =========================================================
           INPUT
        ========================================================= */

        .form-control-sap {
            width: 100%;
            height: 34px;
            border: 1px solid #b8c4d1;
            border-radius: 4px;
            padding: 5px 9px;
            font-size: 13px;
            color: #263238;
            background: #ffffff;
            box-sizing: border-box;
        }

        .form-control-sap:focus {
            border-color: #0a6ed1;
            outline: none;
            box-shadow: 0 0 0 2px rgba(10,110,209,.12);
        }

        .form-control-sap[readonly] {
            background: #f5f7f9;
            color: #4c5967;
        }

        /* =========================================================
           CAMPO DE CÓDIGO
        ========================================================= */

        .codigo-input {
            background: #f5f7f9;
            font-weight: 600;
        }

        /* =========================================================
           BOTÕES
        ========================================================= */

        .btn-sap {
            height: 34px;
            border: none;
            border-radius: 4px;
            padding: 0 18px;
            font-size: 13px;
            font-weight: 600;
            cursor: pointer;
        }

        .btn-sap-primary {
            background: #0a6ed1;
            color: #ffffff;
        }

        .btn-sap-primary:hover {
            background: #085caf;
        }

        .btn-sap-secondary {
            background: #e7edf3;
            color: #263238;
        }

        .btn-sap-secondary:hover {
            background: #d5dee7;
        }

        /* =========================================================
           ESPAÇAMENTO
        ========================================================= */

        .campo {
            margin-bottom: 10px;
        }

        /* =========================================================
           TÍTULO
        ========================================================= */

        .page-title {
            font-size: 20px;
            font-weight: 600;
            color: #263238;
            margin-bottom: 15px;
        }

        .page-title i {
            color: #0a6ed1;
            margin-right: 8px;
        }

        /* =========================================================
           STATUS
        ========================================================= */

        .status-box {
            padding: 8px 12px;
            border-radius: 4px;
            font-size: 13px;
            margin-top: 10px;
            display: none;
        }

        /* =========================================================
           RESPONSIVO
        ========================================================= */

        @media (max-width: 768px) {

            .solicitacao-page {
                padding: 8px;
            }

            .erp-card-body {
                padding: 10px;
            }

        }
        /* =================================================================
            FORMULARIO HORIZONTAL
        ===================================================================*/
        .sap-form-label {
            font-size: 13px;
            font-weight: 600;
            color: #354a5f;
            text-align: right;
            padding-right: 8px;
        }

        .sap-form-control {
            height: 32px;
            border: 1px solid #89919a;
            border-radius: 2px;
            font-size: 13px;
            background-color: #f7f7f7;
        }

    </style>
  <div class="content-wrapper">
    <section class="content">
        <div class="container-fluid">
            <br />
     <div class="solicitacao-page">

        <!-- =====================================================
             TÍTULO
        ====================================================== -->

        <div class="page-title">
            <i class="fas fa-file-alt"></i>
            Gerar CVA Interplantas/CNTi
        </div>
        <!-- =====================================================
             PESQUISA
        ====================================================== -->
        <div class="erp-card">
            <div class="erp-card-header">
                <i class="fas fa-search"></i>
                Solicitação:
            </div>
            <div class="erp-card-body"> 
                <div class="row mb-2">
                     <div class="col-md-12">
                         <div class="form-group row align-items-center justify-content-end">
                             <label for="<%= txtNumCVA.ClientID %>"
                                    class="col-md-1 col-form-label text-end">
                                 CVA:
                             </label>
                             <div class="col-md-2">
                                 <asp:TextBox
                                     ID="txtNumCVA"
                                     runat="server"
                                     CssClass="form-control form-control-sm text-center"
                                     ReadOnly="true">
                                 </asp:TextBox>
                             </div>
                             <label for="<%= txtSitCVA.ClientID %>"
                                    class="col-md-1 col-form-label text-end">
                                 SITUAÇÃO:
                             </label>
                             <div class="col-md-2">
                                 <asp:TextBox
                                     ID="txtSitCVA"
                                     runat="server"
                                     CssClass="form-control form-control-sm text-center"
                                     ReadOnly="true">
                                 </asp:TextBox>
                             </div>
                         </div>
                     </div>
                 </div>

                <div class="d-flex justify-content-end align-items-center flex-nowrap gap-2 mt-1">
                    <!-- TIPO -->
                    <label for="<%= ddlTipoCVA.ClientID %>" class="mb-0 text-nowrap">
                        TIPO:
                    </label>
                    <asp:DropDownList
                        ID="ddlTipoCVA"
                        runat="server"
                        CssClass="form-control form-control-sm"
                        Style="width: 200px;"
                        AutoPostBack="true">
                        <asp:ListItem Value="1" Text="INTERPLANTAS"></asp:ListItem>
                        <asp:ListItem Value="2" Text="CNTI"></asp:ListItem>
                    </asp:DropDownList>
                    <!-- NÚCLEO -->
                    <label for="<%= ddlEstabelecimentoCVA.ClientID %>"
                           class="mb-0 text-nowrap ms-2">
                        NÚCLEO:
                    </label>
                    <asp:DropDownList
                        ID="ddlEstabelecimentoCVA"
                        runat="server"
                        CssClass="form-control form-control-sm"
                        Style="width: 250px;"
                        AutoPostBack="true">
                    </asp:DropDownList>
                    <!-- Nº SOLICITAÇÃO -->
                    <label for="<%= txtSolicitacao.ClientID %>"
                           class="mb-0 text-nowrap ms-2">
                        Nº SOLICITAÇÃO:
                    </label>
                    <asp:TextBox
                        ID="txtSolicitacao"
                        runat="server"
                        CssClass="form-control form-control-sm text-center"
                        Style="width: 100px;">
                    </asp:TextBox>
                    <!-- BOTÕES -->
                    <asp:Button
                        ID="btnPesquisar"
                        runat="server"
                        Text="Pesquisar"
                        CssClass="btn-sap btn-sap-primary ms-2"
                        OnClick="btnPesquisar_Click" />

                    <asp:Button
                        ID="btnLimpar"
                        runat="server"
                        Text="Limpar"
                        CssClass="btn-sap btn-sap-secondary"
                        CausesValidation="false"
                        OnClick="btnLimpar_Click" />

                    <asp:Button
                        ID="btnGerarCVA"
                        runat="server"
                        Text="Gerar CVA"
                        CssClass="btn-sap btn-outline-success ms-2"
                        OnClick="btnGerarCVA_Click" />
                </div>  
                <br />
                <div class="row gy-0 mt-1 align-items-center">
                    <!-- DATA DA ENTREGA -->
                    <label for="<%= txtDataHoraEntregaCVA.ClientID %>"
                           class="col-md-2 col-form-label text-end">
                        DATA DA ENTREGA:
                    </label>
                    <div class="col-md-2">
                        <asp:TextBox
                            ID="txtDataHoraEntregaCVA"
                            runat="server"
                            CssClass="form-control form-control-sm text-center"
                            Style="width: 150px;"
                            MaxLength="16"
                            placeholder="dd/mm/yyyy hh:mm">
                        </asp:TextBox>
                    </div>
                    <!-- VIAGEM COM RETORNO -->
                    <label for="<%= txtComRetornoCVA.ClientID %>"
                           class="col-md-2 col-form-label text-end">
                        VIAGEM COM RETORNO:
                    </label>
                    <div class="col-md-1">
                        <asp:TextBox
                            ID="txtComRetornoCVA"
                            runat="server"
                            CssClass="form-control form-control-sm text-center"
                            ReadOnly="true">
                        </asp:TextBox>
                    </div>
                    <!-- DATA RETORNO -->                   
                    <div class="col-md-2">
                        <asp:TextBox
                            ID="txtDtRetorno"
                            runat="server"
                            CssClass="form-control form-control-sm text-center"
                            Style="width: 150px;"
                            MaxLength="16"
                            placeholder="dd/mm/yyyy hh:mm">
                        </asp:TextBox>
                    </div>
                    <!-- DEVOLUÇÃO -->
                    <label for="<%= ddlDevolucaoPecaCVA.ClientID %>"
                           class="col-md-2 col-form-label text-end">
                        APENAS DEVOLUÇÃO:
                    </label>
                    <div class="col-md-1">
                        <asp:DropDownList
                            ID="ddlDevolucaoPecaCVA"
                            runat="server"
                            CssClass="form-control form-control-sm"
                            AutoPostBack="true">

                            <asp:ListItem Value="NAO" Text="NAO"></asp:ListItem>
                            <asp:ListItem Value="SIM" Text="SIM"></asp:ListItem>

                        </asp:DropDownList>
                    </div>
                </div>
            </div>
        </div>


        <!-- =====================================================
             DADOS DA SOLICITAÇÃO
        ====================================================== -->

        <div class="erp-card">
            <div class="erp-card-header" style="background-color:#354a5f; color:white; font-size:13px; font-weight:600;">
                <i class="fas fa-file-invoice"></i>
                Dados da Solicitação:
            </div>
            <div class="erp-card-body">
                <div class="row">
                    <!-- EMISSÃO -->
                    <div class="col-md-1">
                        <div class="campo">
                            <label class="form-label-sap">
                                CADASTRO:
                            </label>
                            <asp:TextBox
                                ID="txtEmissao"
                                runat="server"
                                CssClass="form-control-sap"
                                ReadOnly="true">
                            </asp:TextBox>
                        </div>
                    </div>
                    <!-- COLETA -->
                    <div class="col-md-1">
                        <div class="campo">
                            <label class="form-label-sap">
                                COLETA:
                            </label>
                            <asp:TextBox
                                ID="txtColeta"
                                runat="server"
                                CssClass="form-control-sap"
                                ReadOnly="true">
                            </asp:TextBox>
                        </div>
                    </div>
                    <!-- CENTRO DE CUSTO -->
                    <div class="col-md-1">
                        <div class="campo">
                            <label class="form-label-sap">
                                CENTRO CUSTO:
                            </label>
                            <asp:TextBox
                                ID="txtCentroCusto"
                                runat="server"
                                CssClass="form-control-sap"
                                ReadOnly="true">
                            </asp:TextBox>
                        </div>
                    </div>
                    <!-- CONTA -->
                    <div class="col-md-1">
                        <div class="campo">
                            <label class="form-label-sap">
                                CONTA:
                            </label>
                            <asp:TextBox
                                ID="txtConta"
                                runat="server"
                                CssClass="form-control-sap"
                                ReadOnly="true">
                            </asp:TextBox>
                        </div>
                    </div>
                    <!-- TIPO VEÍCULO -->
                    <div class="col-md-1">
                        <div class="campo">
                            <label class="form-label-sap">
                                CÓDIGO:
                            </label>
                            <asp:TextBox
                                ID="txtTipoVeiculo"
                                runat="server"
                                CssClass="form-control-sap codigo-input"
                                ReadOnly="true">
                            </asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="campo">
                            <label class="form-label-sap">
                                TIPO DE VEÍCULO:
                            </label>
                            <asp:TextBox
                                ID="txtTipoVeiculoDescricao"
                                runat="server"
                                CssClass="form-control-sap"
                                ReadOnly="true">
                            </asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <!-- TIPO GERAÇÃO -->
                    <div class="col-md-1">
                        <div class="campo">
                            <label class="form-label-sap">
                                CÓDIGO:
                            </label>
                            <asp:TextBox
                                ID="txtCodTipoGeracao"
                                runat="server"
                                CssClass="form-control-sap codigo-input"
                                ReadOnly="true">
                            </asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="campo">
                            <label class="form-label-sap">
                                TIPO DE GERAÇÃO:
                            </label>
                            <asp:TextBox
                                ID="txtTipoGeracao"
                                runat="server"
                                CssClass="form-control-sap"
                                ReadOnly="true">
                            </asp:TextBox>
                        </div>
                    </div>
                    <!-- TIPO SOLICITAÇÃO -->
                    <div class="col-md-1">
                        <div class="campo">
                            <label class="form-label-sap">
                                CÓDIGO:
                            </label>
                            <asp:TextBox
                                ID="txtTipoSolicitacao"
                                runat="server"
                                CssClass="form-control-sap codigo-input"
                                ReadOnly="true">
                            </asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="campo">
                            <label class="form-label-sap">
                                TIPO DE SOLICITAÇÃO:
                            </label>
                            <asp:TextBox
                                ID="txtTipoSolicitacaoDescricao"
                                runat="server"
                                CssClass="form-control-sap"
                                ReadOnly="true">
                            </asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-1">
                        <div class="campo">
                            <label class="form-label-sap">
                                <i class="fas fa-warehouse"></i>
                                CÓDIGO:
                            </label>
                            <asp:TextBox
                                ID="txtCodRemetente"
                                runat="server"
                                CssClass="form-control-sap codigo-input"
                                ReadOnly="true">
                            </asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="campo">
                            <label class="form-label-sap">
                                REMETENTE:
                            </label>
                            <asp:TextBox
                                ID="txtRemetente"
                                runat="server"
                                CssClass="form-control-sap"
                                ReadOnly="true">
                            </asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <div class="campo">
                            <label class="form-label-sap">
                                CNPJ:
                            </label>
                            <asp:TextBox
                                ID="txtCNPJRemetente"
                                runat="server"
                                CssClass="form-control-sap"
                                ReadOnly="true">
                            </asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <div class="campo">
                            <label class="form-label-sap">
                                MUNICIPIO:
                            </label>
                            <asp:TextBox
                                ID="txtCidRemetente"
                                runat="server"
                                CssClass="form-control-sap"
                                ReadOnly="true">
                            </asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-1">
                        <div class="campo">
                            <label class="form-label-sap">
                                UF:
                            </label>
                            <asp:TextBox
                                ID="txtUfRemetente"
                                runat="server"
                                CssClass="form-control-sap"
                                ReadOnly="true">
                            </asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-1">
                        <div class="campo">
                            <label class="form-label-sap">
                                <i class="fas fa-industry"></i>
                                CÓDIGO:
                            </label>
                            <asp:TextBox
                                ID="txtCodExpedidor"
                                runat="server"
                                CssClass="form-control-sap codigo-input"
                                ReadOnly="true">
                            </asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="campo">
                            <label class="form-label-sap">
                                EXPEDIDOR:
                            </label>
                            <asp:TextBox
                                ID="txtExpedidor"
                                runat="server"
                                CssClass="form-control-sap"
                                ReadOnly="true">
                            </asp:TextBox>

                        </div>
                    </div>
                    <div class="col-md-2">
                        <div class="campo">
                            <label class="form-label-sap">
                                CNPJ:
                            </label>
                            <asp:TextBox
                                ID="txtCNPJExpedidor"
                                runat="server"
                                CssClass="form-control-sap"
                                ReadOnly="true">
                            </asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <div class="campo">
                            <label class="form-label-sap">
                                LOCAL DE COLETA:
                            </label>
                            <asp:TextBox
                                ID="txtCidExpedidor"
                                runat="server"
                                CssClass="form-control-sap"
                                ReadOnly="true">
                            </asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-1">
                        <div class="campo">
                            <label class="form-label-sap">
                                UF:
                            </label>
                            <asp:TextBox
                                ID="txtUfExpedidor"
                                runat="server"
                                CssClass="form-control-sap"
                                ReadOnly="true">
                            </asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-1">
                        <div class="campo">
                            <label class="form-label-sap">
                                <i class="fas fa-map-marker-alt"></i>
                                CÓDIGO:
                            </label>
                            <asp:TextBox
                                ID="txtCodDestinatario"
                                runat="server"
                                CssClass="form-control-sap codigo-input"
                                ReadOnly="true">
                            </asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="campo">
                            <label class="form-label-sap">
                                DESTINATÁRIO:
                            </label>
                            <asp:TextBox
                                ID="txtDestinatario"
                                runat="server"
                                CssClass="form-control-sap"
                                ReadOnly="true">
                            </asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <div class="campo">
                            <label class="form-label-sap">
                                CNPJ:
                            </label>
                            <asp:TextBox
                                ID="txtCNPJDestinatario"
                                runat="server"
                                CssClass="form-control-sap"
                                ReadOnly="true">
                            </asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <div class="campo">
                            <label class="form-label-sap">
                                MUNICIPIO:
                            </label>
                            <asp:TextBox
                                ID="txtCidDestinatario"
                                runat="server"
                                CssClass="form-control-sap"
                                ReadOnly="true">
                            </asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-1">
                        <div class="campo">
                            <label class="form-label-sap">
                                UF:
                            </label>
                            <asp:TextBox
                                ID="txtUfDestinatario"
                                runat="server"
                                CssClass="form-control-sap"
                                ReadOnly="true">
                            </asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row">
                     <div class="col-md-1">
                         <div class="campo">
                             <label class="form-label-sap">
                                 <i class="fas fa-sign-in-alt"></i>
                                 CÓDIGO:
                             </label>
                             <asp:TextBox
                                 ID="txtCodRecebedor"
                                 runat="server"
                                 CssClass="form-control-sap codigo-input"
                                 ReadOnly="true">
                             </asp:TextBox>
                         </div>
                     </div>
                     <div class="col-md-4">
                         <div class="campo">
                             <label class="form-label-sap">
                                 RECEBEDOR:
                             </label>
                             <asp:TextBox
                                 ID="txtRecebedor"
                                 runat="server"
                                 CssClass="form-control-sap"
                                 ReadOnly="true">
                             </asp:TextBox>
                         </div>
                     </div>
                     <div class="col-md-2">
                         <div class="campo">
                             <label class="form-label-sap">
                                 CNPJ:
                             </label>
                             <asp:TextBox
                                 ID="txtCNPJRecebedor"
                                 runat="server"
                                 CssClass="form-control-sap"
                                 ReadOnly="true">
                             </asp:TextBox>
                         </div>
                     </div>
                     <div class="col-md-3">
                         <div class="campo">
                             <label class="form-label-sap">
                                 LOCAL DE ENTREGA:
                             </label>
                             <asp:TextBox
                                 ID="txtCidRecebedor"
                                 runat="server"
                                 CssClass="form-control-sap"
                                 ReadOnly="true">
                             </asp:TextBox>
                         </div>
                     </div>
                     <div class="col-md-1">
                         <div class="campo">
                             <label class="form-label-sap">
                                 UF:
                             </label>
                             <asp:TextBox
                                 ID="txtUfRecebedor"
                                 runat="server"
                                 CssClass="form-control-sap"
                                 ReadOnly="true">
                             </asp:TextBox>
                         </div>
                     </div>
                 </div>
            </div>
        </div>
        <!-- =====================================================
             MOTORISTA
        ====================================================== -->
        <div class="erp-card">
            <div class="erp-card-header" style="background-color:#354a5f; color:white; font-size:13px; font-weight:600;">
                <i class="fas fa-user"></i>
                Dados do Motorista:
            </div>
            <div class="erp-card-body">
                <div class="row">
                    <div class="col-md-1">
                        <div class="campo">
                            <label class="form-label-sap">
                                CÓDIGO:
                            </label>
                            <asp:TextBox
                                ID="txtCodMot"
                                runat="server"
                                CssClass="form-control-sap codigo-input"
                                ReadOnly="true">
                            </asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="campo">
                            <label class="form-label-sap">
                                NOME COMPLETO:
                            </label>
                            <asp:TextBox
                                ID="txtNome"
                                runat="server"
                                CssClass="form-control-sap"
                                ReadOnly="true">
                            </asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <div class="campo">
                            <label class="form-label-sap">
                                CPF:
                            </label>
                            <asp:TextBox
                                ID="txtCPF"
                                runat="server"
                                CssClass="form-control-sap"
                                ReadOnly="true">
                            </asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <div class="campo">
                            <label class="form-label-sap">
                                RG:
                            </label>
                            <asp:TextBox
                                ID="txtRG"
                                runat="server"
                                CssClass="form-control-sap"
                                ReadOnly="true">
                            </asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <div class="campo">
                            <label class="form-label-sap">
                                TRANSPORTADORA:
                            </label>
                            <asp:TextBox
                                ID="txtTranspMotorista"
                                runat="server"
                                CssClass="form-control-sap"
                                ReadOnly="true">
                            </asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-- =====================================================
             VEÍCULO
        ====================================================== -->
        <div class="erp-card">
            <div class="erp-card-header" style="background-color:#354a5f; color:white; font-size:13px; font-weight:600;">
                <i class="fas fa-truck"></i>
                Dados do Veículo:
            </div>
            <div class="erp-card-body">
                <div class="row">
                    <div class="col-md-1">
                        <div class="campo">
                            <label class="form-label-sap">
                                CÓDIGO:
                            </label>
                            <asp:TextBox
                                ID="txtCodVei"
                                runat="server"
                                CssClass="form-control-sap codigo-input"
                                ReadOnly="true">
                            </asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <div class="campo">
                            <label class="form-label-sap">
                                TIPO DE VEÍCULO:
                            </label>
                            <asp:TextBox
                                ID="txtTipVei"
                                runat="server"
                                CssClass="form-control-sap"
                                ReadOnly="true">
                            </asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-1">
                        <div class="campo">
                            <label class="form-label-sap">
                                <i class="fas fa-truck"></i>
                                PLACA:
                            </label>
                            <asp:TextBox
                                ID="txtPlaca"
                                runat="server"
                                CssClass="form-control-sap"
                                ReadOnly="true">
                            </asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-1">
                        <div class="campo">
                            <label class="form-label-sap">
                                CAP.:
                            </label>
                            <asp:TextBox
                                ID="txtCapacidade"
                                runat="server"
                                CssClass="form-control-sap"
                                ReadOnly="true">
                            </asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-5">
                        <div class="campo">
                            <label class="form-label-sap">
                                TRANSPORTADORA:
                            </label>
                            <asp:TextBox
                                ID="txtTranspVeiculo"
                                runat="server"
                                CssClass="form-control-sap"
                                ReadOnly="true">
                            </asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-1">
                         <div class="campo">
                             <label class="form-label-sap">
                                 <i class="fas fa-trailer"></i>
                                 Reboque 1
                             </label>
                             <asp:TextBox
                                 ID="txtReboque1"
                                 runat="server"
                                 CssClass="form-control-sap"
                                 ReadOnly="true">
                             </asp:TextBox>
                         </div>
                    </div>
                    <div class="col-md-1">
                         <div class="campo">
                             <label class="form-label-sap">
                                 <i class="fas fa-trailer"></i>
                                 Reboque 2
                             </label>
                             <asp:TextBox
                                 ID="txtPlacaReboque2"
                                 runat="server"
                                 CssClass="form-control-sap"
                                 ReadOnly="true">
                             </asp:TextBox>
                         </div>
                  </div>
                </div>
            </div>
        </div>
        <!-- =====================================================
             PRODUTOS DA SOLICITAÇÃO
        ====================================================== -->
        <div class="row mt-2">
            <div class="col-md-12">
                <div class="card shadow-sm">
                    <div class="card-header"
                         style="background-color:#354a5f; color:white; font-size:13px; font-weight:600;">
                        <i class="fas fa-box"></i>
                        Produtos da Solicitação:
                    </div>
                    <div class="card-body p-0">
                        <asp:GridView
                            ID="gvProdutosSolicitacao"
                            runat="server"
                            AutoGenerateColumns="False"
                            CssClass="table table-sm table-bordered table-hover mb-0"
                            EmptyDataText="Nenhum produto encontrado para esta solicitação."
                            GridLines="None">
                            <Columns>                               
                                <asp:BoundField
                                    DataField="CodigoProduto"
                                    HeaderText="CÓDIGO PRODUTO">
                                    <ItemStyle HorizontalAlign="Center" />
                                    <HeaderStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField
                                    DataField="NumeroSolicitacao"
                                    HeaderText="SOLICITAÇÃO">
                                    <ItemStyle HorizontalAlign="Center" />
                                    <HeaderStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField
                                    DataField="Quantidade"
                                    HeaderText="QUANTIDADE">
                                    <ItemStyle HorizontalAlign="Center" />
                                    <HeaderStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField
                                    DataField="Quantidade"
                                    HeaderText="CONFIRMADA">
                                    <ItemStyle HorizontalAlign="Center" />
                                    <HeaderStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                            </Columns>
                            <HeaderStyle
                                BackColor="#e5e5e5"
                                ForeColor="#354a5f"
                                Font-Bold="true" />
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>        
    </div>
    </div>
    </section>
  </div>
  <script>
    function mascaraDataHora(campo) {
        let valor = campo.value.replace(/\D/g, '');

        if (valor.length > 12)
            valor = valor.substring(0, 12);

        if (valor.length > 0)
            valor = valor.substring(0, 2) + '/' + valor.substring(2);

        if (valor.length > 5)
            valor = valor.substring(0, 5) + '/' + valor.substring(5);

        if (valor.length > 10)
            valor = valor.substring(0, 10) + ' ' + valor.substring(10);

        if (valor.length > 13)
            valor = valor.substring(0, 13) + ':' + valor.substring(13);

        campo.value = valor;
    }

    document.addEventListener('DOMContentLoaded', function () {

        const campos = [
            '<%= txtDataHoraEntregaCVA.ClientID %>',
            '<%= txtDtRetorno.ClientID %>'
        ];

        campos.forEach(function (id) {

            const campo = document.getElementById(id);

            if (campo) {
                campo.addEventListener('input', function () {
                    mascaraDataHora(this);
                });
            }

        });

    });
</script>

</asp:Content>

