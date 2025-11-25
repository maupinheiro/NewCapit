<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConfereArquivo.aspx.cs" Inherits="NewCapit.ConfereArquivo" %>

<!DOCTYPE html>

<html lang="pt-BR">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
   <title>Controle de Ponto RH</title>
 <style>
     * {
         margin: 0;
         padding: 0;
         box-sizing: border-box;
     }

     body {
         font-family: Arial, sans-serif;
         background-color: #ffffff;
         padding: 20px;
     }

     .container {
         max-width: 1200px;
         margin: 0 auto;
     }

     /* Título */
     .titulo {
         border: 2px solid #000;
         padding: 15px;
         margin-bottom: 20px;
         text-align: center;
     }

     .titulo h1 {
         font-size: 24px;
         font-weight: bold;
     }

     /* Seção de Motorista */
     .secao-motorista {
         border: 2px solid #000;
         padding: 15px;
         margin-bottom: 20px;
     }

     .motorista-content {
         display: grid;
         grid-template-columns: 1fr 1fr 1fr;
         gap: 15px;
         align-items: center;
     }

     .motorista-field {
         display: flex;
         align-items: center;
         gap: 10px;
     }

     .motorista-field label {
         font-weight: bold;
         white-space: nowrap;
     }

     .motorista-field input {
         flex: 1;
         border: 1px solid #000;
         padding: 8px;
         font-family: Arial, sans-serif;
     }

     .botoes-navegacao {
         display: flex;
         gap: 10px;
         justify-content: flex-end;
     }

     .botoes-navegacao button {
         border: 1px solid #999;
         background-color: #fff;
         padding: 8px 12px;
         cursor: pointer;
         font-size: 16px;
         border-radius: 4px;
         transition: background-color 0.2s;
     }

     .botoes-navegacao button:hover:not(:disabled) {
         background-color: #f0f0f0;
     }

     .botoes-navegacao button:disabled {
         opacity: 0.5;
         cursor: not-allowed;
     }

     /* Seção de Período */
     .secao-periodo {
         border: 2px solid #000;
         padding: 15px;
         margin-bottom: 20px;
     }

     .periodo-content {
         display: grid;
         grid-template-columns: 1fr 1fr 1fr;
         gap: 15px;
         align-items: center;
     }

     .periodo-field {
         display: flex;
         align-items: center;
         gap: 10px;
     }

     .periodo-field label {
         font-weight: bold;
         white-space: nowrap;
     }

     .periodo-field input {
         flex: 1;
         border: 1px solid #000;
         padding: 8px;
         font-family: Arial, sans-serif;
     }

     /* GridView */
     .gridview-container {
         border: 2px solid #000;
         overflow-x: auto;
     }

     table {
         width: 100%;
         border-collapse: collapse;
         background-color: #fff;
     }

     thead {
         background-color: #f5f5f5;
     }

     th {
         border: 1px solid #000;
         padding: 12px;
         text-align: center;
         font-weight: bold;
         background-color: #f5f5f5;
     }

     td {
         border: 1px solid #000;
         padding: 12px;
         text-align: center;
     }

     tbody tr:hover {
         background-color: #f9f9f9;
     }

     tbody tr:nth-child(even) {
         background-color: #ffffff;
     }

     /* Responsividade */
     @media (max-width: 768px) {
         .motorista-content,
         .periodo-content {
             grid-template-columns: 1fr;
         }

         .botoes-navegacao {
             justify-content: center;
         }

         table {
             font-size: 14px;
         }

         th, td {
             padding: 8px;
         }
     }
 </style>
</head>
<body>
    <form id="form1" runat="server">
         <div class="container">
     <!-- Título -->

     <div class="titulo">
         <h1>Controle de ponto RH</h1>
     </div>
             
     <!-- Seção de Motorista -->
             <asp:Panel ID="pnlMotorista" runat="server">
     <div class="secao-motorista">
         <div class="motorista-content">
             <div class="motorista-field">
                 <label for="motorista">Motorista:</label>
                <asp:Label ID="lblCod" runat="server" Text=""></asp:Label> - <asp:Label ID="lblMotorista" runat="server" Text=""></asp:Label>
             </div>
             <div></div>
        
         </div>
     </div>

     <!-- Seção de Período -->
     <div class="secao-periodo">
         <div class="periodo-content">
             <div class="periodo-field">
                 <label for="dataInicio">Período:</label>
                 <asp:TextBox ID="txtDtini" runat="server"></asp:TextBox>
             </div>
             <div class="periodo-field">
                 <asp:TextBox ID="txtDtfim" runat="server"></asp:TextBox>
             </div>
             <div></div>
         </div>
     </div>

     <!-- GridView -->


     <div class="gridview-container">
      
   
         <asp:GridView ID="gvPonto" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered">
    <Columns>

        <asp:BoundField DataField="Dia" HeaderText="Dia" />

        <asp:BoundField DataField="Marcacao" HeaderText="Marcações" />

        <asp:BoundField DataField="Status" HeaderText="Status" />

      

    </Columns>
</asp:GridView>
    
     </div>
</asp:Panel>
   <div class="botoes-navegacao">
    <asp:LinkButton ID="lnkAnterior" runat="server" OnClick="lnkAnterior_Click">&lt; Anterior</asp:LinkButton>
    <asp:Label ID="lblPaginaInfo" runat="server" Text=""></asp:Label>
    <asp:LinkButton ID="lnkProximo" runat="server" OnClick="lnkProximo_Click">Próximo &gt;</asp:LinkButton>
</div>

 </div>
      
 <script>
   
 </script>
    </form>
</body>
</html>
