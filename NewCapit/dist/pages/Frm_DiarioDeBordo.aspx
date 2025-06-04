<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="Frm_DiarioDeBordo.aspx.cs" Inherits="NewCapit.dist.pages.Frm_DiarioDeBordo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
       <script language="javascript">

           function ConfirmMessage1() {
               var selectedvalue = confirm("Deseja inserir marcação?");
               if (selectedvalue) {
                   document.getElementById('<%=txtconformmessageValue1.ClientID %>').value = "Yes";
           } else {
               document.getElementById('<%=txtconformmessageValue1.ClientID %>').value = "No";
               }
           }

       </script>
   <script language="javascript">

       function ConfirmMessage3() {
           var selectedvalue = confirm("Deseja inserir valores?");
           if (selectedvalue) {
               document.getElementById('<%=txtconformmessageValue2.ClientID %>').value = "Yes";
           } else {
               document.getElementById('<%=txtconformmessageValue2.ClientID %>').value = "No";
           }
       }

   </script>
    <script language="javascript">

        function ConfirmMessage4() {
            var selectedvalue = confirm("Deseja excluir todos os arquivos?");
            if (selectedvalue) {
                document.getElementById('<%=txtconformmessageValue4.ClientID %>').value = "Yes";
            } else {
                document.getElementById('<%=txtconformmessageValue4.ClientID %>').value = "No";
            }
        }

    </script>
    <script language="javascript">

        function ConfirmMessage5() {
            var selectedvalue = confirm("Deseja excluir as marcações selecionadas?");
            if (selectedvalue) {
                document.getElementById('<%=txtconformmessageValue5.ClientID %>').value = "Yes";
            } else {
                document.getElementById('<%=txtconformmessageValue5.ClientID %>').value = "No";
            }
        }

    </script>
     <script language="javascript">

         function ConfirmMessage6() {
             var selectedvalue = confirm("Deseja excluir as marcações selecionadas?");
             if (selectedvalue) {
                 document.getElementById('<%=txtconformmessageValue6.ClientID %>').value = "Yes";
             } else {
                 document.getElementById('<%=txtconformmessageValue6.ClientID %>').value = "No";
             }
         }

   </script>
   <script language="javascript">
       function ConfirmMessage2() {
           var selectedvalue = alert("Senha alterada com sucesso!");

       }
   </script>
  <script>
      function ver_txt() {
          var Janela = window;
          msgWindow = Janela.open("ver_excel.aspx", "sMinuta", "width=600,height=500,scrollbars=no,resizable=no,navegation=no,status=0,location=0,top=0,left=0");
          msgWindow.moveTo(screen.width / 2 - 600 / 2, screen.height / 2 - 500 / 2 - 20);
      }
</script>
   <script>
       function ver_txt2() {
           var Janela = window;
           msgWindow = Janela.open("ver_txt4.aspx", "sMinuta", "width=600,height=500,scrollbars=no,resizable=no,navegation=no,status=0,location=0,top=0,left=0");
           msgWindow.moveTo(screen.width / 2 - 600 / 2, screen.height / 2 - 500 / 2 - 20);
       }
</script>
   <script>
       function ver_txt3() {
           var Janela = window;
           msgWindow = Janela.open("ver_txt5.aspx", "sMinuta", "width=600,height=500,scrollbars=no,resizable=no,navegation=no,status=0,location=0,top=0,left=0");
           msgWindow.moveTo(screen.width / 2 - 600 / 2, screen.height / 2 - 500 / 2 - 20);
       }
</script>
 <script>
     function ver_demo(cracha, data1,data2,hr) {
         var Janela = window;
         msgWindow = Janela.open("demo_motorista.aspx?cracha=" + cracha + "&data1=" + data1 + "&data2=" + data2 + "&hr=" + hr + "", "sMinuta", "width=1000,height=730,scrollbars=no,resizable=no,navegation=no,status=0,location=0,top=0,left=0");
         msgWindow.moveTo(screen.width / 2 - 1100 / 2, screen.height / 2 - 900 / 2 - 20);
     }
</script>
   <script>
       function ver_demo2(cracha, data1, data2, hr_1, hr_2) {
           var cva = cva;
           var evento = evento;
           var Janela = window;
           msgWindow = Janela.open("demo_motorista21.aspx?cracha=" + cracha + "&data1=" + data1 + "&data2=" + data2 + "&hr_1=" + hr_1 + "&hr_2=" + hr_2 + "", "sMinuta", "width=1000,height=730,scrollbars=no,resizable=no,navegation=no,status=0,location=0,top=0,left=0");
           msgWindow.moveTo(screen.width / 2 - 1100 / 2, screen.height / 2 - 900 / 2 - 20);
       }
   </script>
    
    <script type="text/javascript">

        function abre_relatorio(url, w, h) {
            var newW = w + 100;
            var newH = h + 100;
            var left = (screen.width - newW) / 2;
            var top = (screen.height - newH) / 2;
            var newwindow = window.open(url, 'name', 'width=' + newW + ',height=' + newH + ',left=' + left + ',top=' + top);
            newwindow.resizeTo(newW, newH);

            //posiciona o popup no centro da tela
            newwindow.moveTo(left, top);
            newwindow.focus();
            return false;
        }
    </script>
    <script language="JavaScript" type="text/javascript">
        function formatar(src, mask) {
            var i = src.value.length;
            var saida = mask.substring(0, 1);
            var texto = mask.substring(i)
            if (texto.substring(0, 1) != saida) {
                src.value += texto.substring(0, 1);
            }
        }
    </script>
<script>
    function keypressed(obj, e) {
        var tecla = (window.event) ? e.keyCode : e.which;
        var texto = document.getElementById("numeros").value
        var indexvir = texto.indexOf(",")
        var indexpon = texto.indexOf(".")

        if (tecla == 8 || tecla == 0)
            return true;
        if (tecla != 44 && tecla != 46 && tecla < 48 || tecla > 57)
            return false;
        if (tecla == 44) { if (indexvir !== -1 || indexpon !== -1) { return false } }
        if (tecla == 46) { if (indexvir !== -1 || indexpon !== -1) { return false } }
    }
</script>
    <div class="content-wrapper">
        <!-- Main content -->
        <section class="content">

            <div class="container-fluid">
                <br />
                <div class="card card-info">
                    <div class="card-header">
                        <h3 class="card-title"><i class="fas fa-clipboard-list"></i>&nbsp;JORNADA - DIARIO DE BORDO</h3>
                    </div>
                </div>
                <div class="card-header">
                    <div class="row g-3">
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="details">MOTORISTA:</span>
                                <asp:TextBox ID="txtMotorista" runat="server" Style="text-align: center" CssClass="form-control font-weight-bold" MaxLength="11"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form-group">
                                <span class="details">DATA:</span>
                                <asp:TextBox ID="txtData" TextMode="Date" runat="server" Style="text-align: center" CssClass="form-control font-weight-bold" MaxLength="10"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <br />
                            <asp:Button ID="btnBuscar" runat="server" Text="Pesquisar" CssClass="btn btn-outline-warning" ValidationGroup="Cadastro" OnClick="btnBuscar_Click" />
                        </div>
                    </div>
                    <div class="row g-3">
                        <div class="col-md-6">
                            <div class="form-group">
                                <span class="details">NOME COMPLETO:</span>
                                <asp:TextBox ID="txtNome" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="form-group">
                                <span class="details">FUNÇÃO:</span>
                                <asp:TextBox ID="txtFuncao" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="form-group">
                                <span class="details">NÚCLEO:</span>
                                <asp:TextBox ID="txtNucleo" runat="server" CssClass="form-control"></asp:TextBox>
                                  <asp:HiddenField ID="txtconformmessageValue4" runat="server" />
                                  
                            </div>
                        </div>

                    </div>
                    <div class="row g-3">
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="details">CAFÉ:</span>
                                <asp:TextBox ID="txtCafe" runat="server" CssClass="form-control" MaxLength="6"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="details">ALMOÇO:</span>
                                <asp:TextBox ID="txtAlmoco" runat="server" CssClass="form-control" MaxLength="6"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="details">JANTA:</span>
                                <asp:TextBox ID="txtJantar" runat="server" CssClass="form-control" MaxLength="6"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="details">PERNOITE:</span>
                                <asp:TextBox ID="txtPernoite" runat="server" CssClass="form-control" MaxLength="6"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="details">ENG./DES:</span>
                                <asp:TextBox ID="txtEngDesen" runat="server" CssClass="form-control" MaxLength="6"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="details">COMISSÃO:</span>
                                <asp:TextBox ID="txtComissao" runat="server" CssClass="form-control" MaxLength="6"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="details">RELATÓRIOS:</span>
                                <asp:TextBox ID="txtRel1" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="details">[2]:</span>
                                <asp:TextBox ID="txtRel2" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="details">[3]:</span>
                                <asp:TextBox ID="txtRel3" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="details">[4]:</span>
                                <asp:TextBox ID="txtRel4" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                            </div>
                        </div>
                         <div class="card-body">
   <table id="example1" class="table table-bordered table-striped table-hover table-responsive">
   <asp:GridView ID="grdCusto" runat="server"  AutoGenerateColumns="false" Width="970px"     Font-Size="12px" DataKeyNames="cod_custo" OnRowCommand="grdCusto_RowCommand" >
    <Columns>
        <asp:BoundField DataField="cafe" HeaderText="CAFÉ DA MANHÃ" ItemStyle-HorizontalAlign="Center"/>
        <asp:BoundField DataField="almoco" HeaderText="ALMOÇO"  ItemStyle-HorizontalAlign="Center"/>
        <asp:BoundField DataField="jantar" HeaderText="JANTAR" ItemStyle-HorizontalAlign="Center"/>
        <asp:BoundField DataField="pernoite" HeaderText="PERNOITE" ItemStyle-HorizontalAlign="Center"/>
        <asp:BoundField DataField="premio" HeaderText="PRÊMIO" ItemStyle-HorizontalAlign="Center"/>
        <asp:BoundField DataField="engatedes" HeaderText="ENGATE/DES" ItemStyle-HorizontalAlign="Center"/>
        <asp:BoundField DataField="ds_rel1" HeaderText="Relatório 1" ItemStyle-HorizontalAlign="Center"/>
        <asp:BoundField DataField="ds_rel2" HeaderText="Relatório 2" ItemStyle-HorizontalAlign="Center"/>
        <asp:BoundField DataField="ds_rel3" HeaderText="Relatório 3" ItemStyle-HorizontalAlign="Center"/>
        <asp:BoundField DataField="ds_rel4" HeaderText="Relatório 4" ItemStyle-HorizontalAlign="Center"/>
        <asp:BoundField DataField="total" HeaderText="TOTAL" ItemStyle-HorizontalAlign="Center"/>
        
        <asp:ButtonField ButtonType="Link" CommandName="Select" Text="Excluir"  />
        


    </Columns>
</asp:GridView>
       </table>
                        </div>
                        <div class="col-md-1">
                            <br />
                            <asp:HiddenField ID="txtconformmessageValue2" runat="server" />
                            <asp:Button ID="btnValor" runat="server" Text="Inserir" CssClass="btn btn-outline-info" OnClick="btnValor_Click" />
                        </div>
                    </div>
                    <div class="row g-3">
                        <div class="col-md-3">
                            <div class="form-group">
                                <span class="details">MACRO:</span>
                                <asp:DropDownList ID="ddlMacros" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="" Text="Selecione..."></asp:ListItem>
                                    <asp:ListItem Value="INICIO DE JORNADA" Text="INICIO DE JORNADA"></asp:ListItem>
                                    <asp:ListItem Value="INICIO JORNADA" Text="INICIO JORNADA"></asp:ListItem>
                                    <asp:ListItem Value="INICIO DE VIAGEM" Text="INICIO DE VIAGEM"></asp:ListItem>
                                    <asp:ListItem Value="REINICIO DE VIAGEM" Text="REINICIO DE VIAGEM"></asp:ListItem>
                                    <asp:ListItem Value="PARADA INTERNA" Text="PARADA INTERNA"></asp:ListItem>
                                    <asp:ListItem Value="PARADA REFEICAO" Text="PARADA REFEICAO"></asp:ListItem>
                                    <asp:ListItem Value="RETORNO REFEICAO" Text="RETORNO REFEICAO"></asp:ListItem>
                                    <asp:ListItem Value="PARADA CLIENTE/FORNECEDOR" Text="PARADA CLIENTE/FORNECEDOR"></asp:ListItem>
                                    <asp:ListItem Value="PARADA" Text="PARADA"></asp:ListItem>
                                    <asp:ListItem Value="PARADA OFICINA" Text="PARADA OFICINA"></asp:ListItem>
                                    <asp:ListItem Value="FIM DE VIAGEM" Text="FIM DE VIAGEM"></asp:ListItem>
                                    <asp:ListItem Value="FIM DE JORNADA" Text="FIM DE JORNADA"></asp:ListItem>
                                    <asp:ListItem Value="PARADA PERNOITE" Text="PARADA PERNOITE"></asp:ListItem>
                                </asp:DropDownList><br />
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="details">HORA:</span>
                                <asp:TextBox ID="txtHora" TextMode="Time" Style="text-align: center" runat="server" CssClass="form-control" MaxLength="5"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form-group">
                                <span class="">PARADA:</span>
                                <asp:DropDownList ID="ddlNumero" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="" Text="Selecione..."></asp:ListItem>
                                    <asp:ListItem Value="1" Text="1"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                    <asp:ListItem Value="3" Text="3"></asp:ListItem>
                                    <asp:ListItem Value="4" Text="4"></asp:ListItem>
                                    <asp:ListItem Value="6" Text="6"></asp:ListItem>
                                    <asp:ListItem Value="7" Text="7"></asp:ListItem>
                                    <asp:ListItem Value="9" Text="9"></asp:ListItem>
                                    <asp:ListItem Value="11" Text="11"></asp:ListItem>
                                    <asp:ListItem Value="13" Text="13"></asp:ListItem>
                                    <asp:ListItem Value="14" Text="14"></asp:ListItem>
                                    <asp:ListItem Value="15" Text="15"></asp:ListItem>
                                    <asp:ListItem Value="16" Text="16"></asp:ListItem>
                                    <asp:ListItem Value="17" Text="17"></asp:ListItem>
                                    <asp:ListItem Value="22" Text="22"></asp:ListItem>
                                    <asp:ListItem Value="23" Text="23"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="form-group">
                                <span class="details">DESCRIÇÃO:</span>
                                 <asp:HiddenField ID="txtconformmessageValue1" runat="server" />
                                <asp:TextBox ID="txtTipoMarcacao" runat="server" CssClass="form-control font-weight-bold"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <br />
                            <asp:Button ID="btnMacromanual" runat="server" CssClass="btn btn-outline-info" Text="Inserir" ValidationGroup="Cadastro" OnClientClick="javascript:ConfirmMessage1();" Width="139px" OnClick="btnMacromanual_Click" />
                           
                        </div>

                            
                            <div class="col-md-1" id="button" runat="server">
                                <br />
                            </div>
                          


           <asp:Label ID="lblBloco" runat="server" Text=""></asp:Label><br />
            <b><asp:Label ID="lblAlerta" runat="server" Text="" ForeColor="Red"></asp:Label></b> 
             <div class="card-body">
               <table id="example1" class="table table-bordered table-striped table-hover table-responsive">
               <asp:GridView ID="grdMotoristas" runat="server"  AutoGenerateColumns="false" Width="970px" HeaderStyle-BackColor="Silver" 
                AlternatingRowStyle-BackColor="#e7e7e7"    Font-Size="12px" CssClass="mGrid" DataKeyNames="cod_parada" OnRowCommand="grdMotoristas_RowCommand">
    <Columns>
         <asp:TemplateField >
            <ItemTemplate >
                <asp:CheckBox ID="chkM" runat ="server" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="ds_macro" HeaderText="Tipo de Marcação"  />
        <asp:BoundField DataField="ds_tipo" HeaderText="Descrição" />
        <asp:BoundField DataField="data" HeaderText="Data" ItemStyle-HorizontalAlign="Center" />
        <asp:BoundField DataField="Hora1" HeaderText="Hora Inicial" ItemStyle-HorizontalAlign="Center"/>
        <asp:BoundField DataField="Hora2" HeaderText="Hora Final" ItemStyle-HorizontalAlign="Center"/>
        <asp:BoundField DataField="Total" HeaderText="Total" ItemStyle-HorizontalAlign="Center"/>
        <asp:ButtonField ButtonType="Link" CommandName="Select" Text="Excluir"  />
        


    </Columns>
</asp:GridView><br />  
                </table>
                 </div>
                         <div class="col-md-1">
             <asp:HiddenField ID="txtconformmessageValue5" runat="server" />
  <asp:Button ID="btnExcluiMotoristas" runat="server" CssClass="btn btn-outline-danger" Text="Excluir"  OnClientClick="javascript:ConfirmMessage5();" Width="139px"/>
            </div>
            <br/>
             <asp:Label ID="lblTodas" runat="server" Text=""></asp:Label> 
                 <div class="card-body">
                    <table id="example1" class="table table-bordered table-striped table-hover table-responsive">
                    <asp:GridView ID="grdTodas" runat="server"  AutoGenerateColumns="false" Width="970px" HeaderStyle-BackColor="Silver" 
                                    AlternatingRowStyle-BackColor="#e7e7e7"    Font-Size="12px" CssClass="mGrid" DataKeyNames="cod_parada" OnRowCommand="grdTodas_RowCommand" >
                        <Columns>
                            <asp:TemplateField >
                                <ItemTemplate >
                                    <asp:CheckBox ID="chkT" runat ="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ds_macro" HeaderText="Tipo de Marcação"  />
                            <asp:BoundField DataField="ds_tipo" HeaderText="Descrição" />
                            <asp:BoundField DataField="data" HeaderText="Data" ItemStyle-HorizontalAlign="Center"/>
                            <asp:BoundField DataField="Hora1" HeaderText="Hora Inicial" ItemStyle-HorizontalAlign="Center"/>
                            <asp:BoundField DataField="Hora2" HeaderText="Hora Final" ItemStyle-HorizontalAlign="Center"/>
                            <asp:BoundField DataField="Total" HeaderText="Total" ItemStyle-HorizontalAlign="Center"/>
                            <asp:ButtonField ButtonType="Link" CommandName="Select" Text="Excluir"  />
        


                        </Columns>
                    </asp:GridView>
                     </table>
                     </div>
                       <br />  
            <div class="col-md-1">
              <asp:HiddenField ID="txtconformmessageValue6" runat="server" />
              <asp:Button ID="btnExcluiTodas" runat="server" CssClass="btn btn-outline-danger" Text="Excluir"  OnClientClick="javascript:ConfirmMessage6();" Width="139px" OnClick="btnExcluiTodas_Click" />
             <br />
              </div>

                    </div>




                </div>
            </div>
        </section>
        <!-- Mensagens de erro toast -->
        <div class="toast-container position-fixed top-0 end-0 p-3">
            <div id="toastNotFound" class="toast align-items-center text-bg-danger border-0" role="alert" aria-live="assertive" aria-atomic="true">
                <div class="d-flex">
                    <div class="toast-body">
                        Motorista não encontrado. Verifique o código digitado.
                    </div>
                    <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast" aria-label="Close"></button>
                </div>
            </div>
        </div>
    </div>
    <!-- /.content-wrapper -->
    <footer class="main-footer">
        <div class="float-right d-none d-sm-block">
            <b>Version</b> 3.1.0 
        </div>
        <strong>Copyright &copy; 2023-2025 <a href="#">Capit Logística</a>.</strong> Todos os direitos reservados.
    </footer>

    <script>
        function mostrarToastNaoEncontrado() {
            var toastEl = document.getElementById('toastNotFound');
            var toast = new bootstrap.Toast(toastEl);
            toast.show();
        }
    </script>
</asp:Content>
