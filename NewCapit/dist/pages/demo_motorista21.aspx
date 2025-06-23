<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="demo_motorista21.aspx.cs" Inherits="NewCapit.dist.pages.demo_motorista21" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <!-- Bootstrap CSS -->    
   
    <style type="text/css">
        .auto-style1 {
            width: 564px;
        }
    </style>
   
 </head>   
<body>
    <form id="form1" runat="server">
    <div>
    <table>
        <tr>
            <td>
                <img src="../../img/logo_transnovag.png" style="margin-top: -55px" />
            </td>
            <td>
             <table style="width:900px">
                 <tr>
                     <td style="text-align:center;">
                         <b><h1> DEMONSTRATIVO DE JORNADA</h1>DIÁRIO DE BORDO DO MOTORISTA</b>
                         </td>
                 </tr>
             </table>
            <table>
                <tr>
                    <td style="width:120pX"><b>CRACHÁ:</b><br />
                        <asp:Label ID="lblCracha" runat="server" Text=""></asp:Label>
                    </td>
                    <td style="width:400pX"><b>NOME:</b><br />
                        <asp:Label ID="lblNome" runat="server" Text=""></asp:Label>
                    </td>
                    <td style="width:366pX"><b>DATA:</b><br />
                        <asp:Label ID="lblData" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                  <tr>
                    <td style="width:120pX"><b>CARGO:</b></td>
                    <td style="width:400pX"><asp:Label ID="lblCargo" runat="server" Text=""></asp:Label></td>
                    <td><b>HORÁRIO DE TRABALHO:</b> <asp:Label ID="lblHorário" runat="server" Text=""></asp:Label></td>
                </tr>
            </table>
            </td>
            
        </tr>
        <tr>
            <td>

            </td>
            <td style="text-align:center;">
                &nbsp;</td>
        </tr>
    </table>
    <asp:GridView ID="grdMotoristas" runat="server"  AutoGenerateColumns="false" Width="1060px" HeaderStyle-BackColor="Silver" 
                        AlternatingRowStyle-BackColor="#e7e7e7"    Font-Size="14Px" CssClass="mGrid" DataKeyNames="cod_parada">
            <Columns>
                
                <asp:BoundField DataField="ds_macro" HeaderText="Tipo de Marcação" />
                <asp:BoundField DataField="ds_tipo" HeaderText="Descrição" />
                <asp:BoundField DataField="data" HeaderText="Data" ItemStyle-HorizontalAlign="Center"/>
                <asp:BoundField DataField="Hora1" HeaderText="Hora Inicial" ItemStyle-HorizontalAlign="Center"/>
                <asp:BoundField DataField="Hora2" HeaderText="Hora Final" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="Total" HeaderText="Total" ItemStyle-HorizontalAlign="Center"/>
                <asp:BoundField DataField="fl_marcacao" HeaderText="Registro" ItemStyle-HorizontalAlign="Center" />
               
                


            </Columns>
        </asp:GridView><br />  
                    

    </div>
    <div><br /><br />
        <table>
            <tr>
                <td>
                    <b>* I.M – Inclusão de Registro por ausência de marcação do motorista.</b>
                </td>
            </tr>
        </table>
        <br />
        <table>
            <tr>
                <td>
                    <b>Estou de pleno acordo com o que demonstram as marcações acima, sendo que representam o ocorrido neste período.</b>
                </td>
            </tr>
        </table><br /><br /><br />

        <table style="width: 1060px">
          <tr>
              <td style="text-align:center;" class="auto-style1">
                  ________________________________________________<br />
                  ASSINATURA DO EMPREGADO
              </td>
              <td>
                  <td style="text-align:center;">
                  ________________________________________________<br />
                  ASSINATURA DA ADM
              </td>
              </td>
          </tr>
        </table>
        <br /><br /><br />
          <table>
            <tr>
                 <td>
                    <b>
                        <asp:Label ID="lblAltera" runat="server" Text=""></asp:Label></b>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
