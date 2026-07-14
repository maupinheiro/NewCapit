<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="PerfilUsuario.aspx.cs" Inherits="NewCapit.PerfilUsuario" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .card-perfil {
            max-width: 650px;
            margin: 30px auto;
            border-radius: 10px;
            box-shadow: 0 0 15px rgba(0,0,0,.15);
        }

        .foto-perfil {
            width:180px;
            height:180px;
            border-radius:50%;
            border:4px solid #0d6efd;
            object-fit:cover;
        }

        .titulo{
            font-size:22px;
            font-weight:bold;
        }

        .texto-ajuda{
            color:#666;
            font-size:13px;
        }

    </style>
    <script>

        function PreviewImagem(input) {

            if (input.files && input.files[0]) {

                var reader = new FileReader();

                reader.onload = function (e) {

                    document.getElementById('<%=imgFoto.ClientID%>').src = e.target.result;

                };

                reader.readAsDataURL(input.files[0]);

            }

        }

    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="container">

<div class="card card-perfil">

<div class="card-header bg-primary text-white">

<span class="titulo">

Alterar Foto de Perfil

</span>

</div>

<div class="card-body">

<div class="row">

<div class="col-md-4 text-center">

<asp:Image
    ID="imgFoto"
    runat="server"
    CssClass="foto-perfil"
    ImageUrl="~/fotos/usuario.jpg" />

</div>

<div class="col-md-8">

<div class="mb-3">

<label><b>Usuário</b></label>

<asp:TextBox
    ID="txtUsuario"
    runat="server"
    CssClass="form-control"
    Enabled="false">
</asp:TextBox>

</div>

<div class="mb-3">

<label><b>Selecionar Foto</b></label>

<asp:FileUpload
    ID="fuFoto"
    runat="server"
    CssClass="form-control"
    onchange="PreviewImagem(this);" />

<div class="texto-ajuda">

Somente arquivos JPG, JPEG ou PNG.

<br />

Tamanho máximo: 2 MB.

</div>

</div>

<div class="mt-4">

<asp:Button
    ID="btnSalvar"
    runat="server"
    Text="Salvar Foto"
    CssClass="btn btn-success"
    OnClick="btnSalvar_Click" />

<asp:Button
    ID="btnCancelar"
    runat="server"
    Text="Cancelar"
    CssClass="btn btn-secondary"
    PostBackUrl="~/dist/pages/home.aspx" />

</div>

<div class="mt-3">

<asp:Label
    ID="lblMensagem"
    runat="server"
    Font-Bold="true">
</asp:Label>

</div>

</div>

</div>

</div>

</div>
<br /><br /><br />
</div>

</asp:Content>