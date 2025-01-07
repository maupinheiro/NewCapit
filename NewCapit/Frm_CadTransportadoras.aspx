<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Frm_CadTransportadoras.aspx.cs" Inherits="NewCapit.Frm_CadTransportadoras" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div class="container mt-5">
        <div class="d-sm-flex align-items-center justify-content-between mb-4">
            <h3 class="h3 mb-2 text-gray-800">                
                ..: <i class="fas fa-user-friends"></i>NOVO CADASTRO :..
            </h3>
            <div class="form-group">
                <asp:DropDownList ID="cbFiliais" name="nomeFiliais" runat="server" Width="350px" ForeColor="Blue"></asp:DropDownList> 
            </div>
        </div>
        
        <hr />
        <div class="row g-3">
            <div class="col-md-3">
                <div class="form-group">
                    <span class="details">FILIAL:</span>
                    <asp:DropDownList ID="cbFiliais2" name="nomeFiliais2" runat="server" Width="350px" ForeColor="Blue"></asp:DropDownList>
                </div>
            </div>
            <div class="col-md-1">
                <div class="form-group">
                    <span class="details">CÓDIGO:</span>
                    <asp:TextBox ID="txtCodCli" runat="server" ForeColor="Blue" CssClass="form-control" placeholder="" MaxLength="11"></asp:TextBox>
                </div>
            </div>

            <div class="col-md-8">
                <div class="form_group">
                    <span class="details">TIPO DE CLIENTE:</span>
                    <asp:DropDownList ID="cboTipo" runat="server" ForeColor="Blue" CssClass="form-control" Width="250px">
                        <asp:ListItem Value="" Text="SELECIONE"></asp:ListItem>
                        <asp:ListItem Value="CLIENTE" Text="CLIENTE"></asp:ListItem>
                        <asp:ListItem Value="EMBARCADOR" Text="EMBARCADOR"></asp:ListItem>
                        <asp:ListItem Value="TRANSPORTADOR" Text="TRANSPORTADOR"></asp:ListItem>
                        <asp:ListItem Value="OPERADOR LOGÍSTICO" Text="OPERADOR LOGÍSTICO"></asp:ListItem>
                        <asp:ListItem Value="OUTROS" Text="OUTROS"></asp:ListItem>                       
                    </asp:DropDownList><br />
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
                <a href="ConsultaAgregados.aspx" class="btn btn-outline-danger btn-lg">Cancelar               
                </a>
            </div>
        </div>

    </div>

           <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.5.0/jquery.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/jquery.mask/1.14.16/jquery.mask.js"></script>

<script src="https://code.jquery.com/jquery-3.7.1.min.js" integrity="sha256-/JqT3SQfawRcv/BIHPThkBvs0OEvtFFmqPF/lYI/Cxo=" crossorigin="anonymous"></script>
<script src="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/js/select2.min.js"></script> 
    <script>
        (document).ready(function () {
       $('#cboTipo').select2();
        });
    </script>

</asp:Content>
