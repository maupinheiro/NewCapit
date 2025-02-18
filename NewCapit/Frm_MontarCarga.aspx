<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Frm_MontarCarga.aspx.cs" Inherits="NewCapit.Frm_MontarCarga" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid">
        <div class="d-sm-flex align-items-center justify-content-between mb-0">
            <h3 class="h3 mb-2 text-gray-800"><i class="fas fa-route"></i>CARGA</h3>
            <h3>99999999999</h3>
        </div>
        <div class="d-sm-flex align-items-center justify-content-between mb-0">
            <h6></h6>
            <h6>
                <asp:Label ID="lblDtCadastro" runat="server"></asp:Label>
            </h6>
        </div>
        <hr />
        <!-- Linha 1 -->
        <div class="row g-3">
            <div class="col-sm-1">
                <label for="codRemetente" class="form-label">Código:</label>
                <asp:TextBox ID="txtCodRemetente" runat="server" class="form-control" OnTextChanged="PreencheRemetente"></asp:TextBox>
            </div>
            <div class="col-sm-5">
                <label for="nomeRemetente" class="form-label">Remetente:</label>
                <asp:DropDownList class="single-select-field" ID="ddlRemetente" runat="server" OnSelectedIndexChanged="ddlRemetente_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
            </div>
            <div class="col-sm-1">
                <label for="ufRemetente" class="form-label">Código:</label>
                <asp:TextBox ID="txtCodExpedidor" runat="server" class="form-control" AutoPostBack="true"></asp:TextBox>
            </div>
            <div class="col-sm-5">
                <label for="nomeRemetente" class="form-label">Expedidor:</label>
                <asp:DropDownList class="single-select-field" ID="ddlExpedidor" runat="server" OnSelectedIndexChanged="ddlExpedidor_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
            </div>

        </div>
        <!-- Linha 2 -->
        <div class="row g-3">
            <div class="col-sm-1">
                <label for="codRemetente" class="form-label">Código:</label>
                <asp:TextBox ID="txtCodDestinatario" runat="server" class="form-control" AutoPostBack="true"></asp:TextBox>
            </div>
            <div class="col-sm-5">
                <label for="nomeRemetente" class="form-label">Destinatário:</label>
                <asp:DropDownList class="single-select-field" ID="ddlDestinatario" runat="server" OnSelectedIndexChanged="ddlDestinatario_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
            </div>
            <div class="col-sm-1">
                <label for="codRemetente" class="form-label">Código:</label>
                <asp:TextBox ID="txtCodRecebedor" runat="server" class="form-control" AutoPostBack="true"></asp:TextBox>
            </div>
            <div class="col-sm-5">
                <label for="nomeRemetente" class="form-label">Recebedor:</label>
                <asp:DropDownList class="single-select-field" ID="ddlRecebedor" runat="server" OnSelectedIndexChanged="ddlRecebedor_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
            </div>
        </div>
        -<!-- Linha 3 -->
        <div class="row g-3">
            <div class="col-sm-1">
                <label for="codRemetente" class="form-label">Código:</label>
                <asp:TextBox ID="txtCodConsignatario" runat="server" class="form-control" AutoPostBack="true"></asp:TextBox>
            </div>
            <div class="col-sm-5">
                <label for="nomeRemetente" class="form-label">Consignatário:</label>
                <asp:DropDownList class="single-select-field" ID="ddlConsignatario" runat="server" OnSelectedIndexChanged="ddlDestinatario_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
            </div>
            <div class="col-sm-1">
                <label for="codRemetente" class="form-label">Código:</label>
                <asp:TextBox ID="txtCodPagador" runat="server" class="form-control" AutoPostBack="true"></asp:TextBox>
            </div>
            <div class="col-sm-5">
                <label for="nomeRemetente" class="form-label">Pagador:</label>
                <asp:DropDownList class="single-select-field" ID="ddlPagador" runat="server" OnSelectedIndexChanged="ddlPagador_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
            </div>
        </div>


    </div>
    <!-- Styles -->
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/css/bootstrap.min.css" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/css/select2.min.css" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/select2-bootstrap-5-theme@1.3.0/dist/select2-bootstrap-5-theme.min.css" />



    <!-- Scripts -->
    <script src="https://cdn.jsdelivr.net/npm/jquery@3.5.0/dist/jquery.slim.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/js/bootstrap.bundle.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/js/select2.min.js"></script>
    <script> 
        $('.single-select-field').select2({
            theme: "bootstrap-5",
            width: $(this).data('width') ? $(this).data('width') : $(this).hasClass('w-100') ? '100%' : 'style',
            placeholder: $(this).data('placeholder'),
        });
    </script>



</asp:Content>
