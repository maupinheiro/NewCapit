

. <%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="NewCapit.Login" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta charset="UTF-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>...:: CAPIT Logística ::...</title>
    <link href="https://unpkg.com/boxicons@2.1.4/css/boxicons.min.css" rel="stylesheet"/>
    <link rel="stylesheet" href="../css/styleLogin.css"/>
</head>
<body>
<div class="container">
    <div class="form-box login">
       <form id="formlogin" runat="server">
            <h3>Entre com sua conta</h3>
            <div class="input-box">
                <i class="bx bxs-user"></i>
                <asp:TextBox required="" ID="txtUsuario" placeholder="Usuário..." runat="server" ></asp:TextBox>                
            </div>
            <div class="input-box">
                <i class="bx bxs-lock-alt"></i>
                <asp:TextBox required="" ID="txtSenha" placeholder="Senha..." runat="server" TextMode="Password"></asp:TextBox>
            </div>
            <div class="forgot-link">
                <br />
                <a href="#">Esqueci minha senha.</a>
            </div>
          
            <asp:Button ID="btnEntrar" Text="Entrar" CssClass="btn btn-primary btn-user btn-block" runat="server" OnCLick="btnLogin_Click"/>
           <br />
           <br />
           <br />
           <asp:Label Text="" ID="lblError" ForeColor="Yellow" Font-Bold="true" runat="server" />
       
           </form>
    </div>
   
    <div class="toggle-box">
        <div class="toggle-panel toggle-left">
            <img src="/img/logo.png" alt="" width="100" />
	        <h1>CAPIT LOGÍSTICA</h1>
            <p>TRANSNOVAG TRANSPORTES</p>                
        </div>            
    </div>
</div>  

    <!-- Bootstrap core JavaScript-->
    <script src="vendors/jquery/jquery.min.js"></script>
    <script src="vendors/bootstrap/js/bootstrap.bundle.min.js"></script>

    <!-- Core plugin JavaScript-->
    <script src="vendors/jquery-easing/jquery.easing.min.js"></script>

    <!-- Custom scripts for all pages-->
    <script src="js/sb-admin-2.min.js"></script>

    <!-- Logo -->
    <script src="script/script.js"></script>
    
</body>
</html>
