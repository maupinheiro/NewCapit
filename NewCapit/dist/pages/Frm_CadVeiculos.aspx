<%@ Page Title="" Language="C#" MasterPageFile="Main.Master" AutoEventWireup="true" CodeBehind="Frm_CadVeiculos.aspx.cs" Inherits="NewCapit.Frm_CadVeiculos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="content-wrapper">
        <section class="content">
            <div class="container-fluid">
                <br />
                <div class="card card-warning">
                    <div class="card-header">
                        <h3 class="card-title"><i class="fas fa-shipping-fast"></i>VEÍCULO - NOVO CADASTRO</h3>
                    </div>
                </div>
                <div class="card-header">        
                      <!-- linha 1 -->
                      <div class="row g-3">
            <div class="col-md-1">
                <div class="form-group">
                    <span class="details">FROTA:</span>
                    <asp:TextBox ID="txtCodVei" Style="text-align: center" runat="server" CssClass="form-control" placeholder="" MaxLength="9"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-1">
                <br />
                <asp:Button ID="btnVeiculo" runat="server" Text="Pesquisar" CssClass="btn btn-outline-warning" OnClick="btnVeiculo_Click" />
            </div>
            <div class="col-md-2">
                <div class="form_group">
                    <span class="details">TIPO DE VEÍCULO:</span>
                    <asp:DropDownList ID="cboTipo" runat="server" CssClass="form-control" Width="250px">
                        <asp:ListItem Value="" Text=""></asp:ListItem>
                        <asp:ListItem Value="BITREM" Text="BITREM"></asp:ListItem>
                        <asp:ListItem Value="BITRUCK" Text="BITRUCK"></asp:ListItem>
                        <asp:ListItem Value="CAVALO SIMPLES" Text="CAVALO SIMPLES"></asp:ListItem>
                        <asp:ListItem Value="CAVALO TRUCADO" Text="CAVALO TRUCADO"></asp:ListItem>
                        <asp:ListItem Value="CAVALO 4 EIXOS" Text="CAVALO 4 EIXOS"></asp:ListItem>
                        <asp:ListItem Value="TOCO" Text="TOCO"></asp:ListItem>
                        <asp:ListItem Value="TRUCK" Text="TRUCK"></asp:ListItem>
                        <asp:ListItem Value="VEICULO 3/4" Text="VEICULO 3/4"></asp:ListItem>
                        <asp:ListItem Value="OUTROS" Text="OUTROS"></asp:ListItem>
                    </asp:DropDownList><br />
                </div>
            </div>
            <div class="col-md-1">
                <div class="form-group">
                    <span class="details">PLACA:</span>
                    <asp:TextBox ID="txtPlaca" Style="text-align: center" runat="server" CssClass="form-control" placeholder="" MaxLength="8"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-1">
                <div class="form-group">
                    <span class="details">UF:</span>
                    <asp:DropDownList ID="ddlUfPlaca" runat="server" class="form-control select2">                       
                    </asp:DropDownList>
                </div>
            </div>          
            <div class="col-md-4">
                <div class="form_group">
                    <span class="details">MUNICIPIO:</span>
                    <asp:TextBox ID="txtCidPlaca" runat="server" Style="text-align: left" CssClass="form-control" placeholder="" MaxLength="40"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-2">
                <div class="form_group">
                    <span class="details">FILIAL:</span>
                    <asp:DropDownList ID="cbFiliais" name="nomeFiliais" runat="server" CssClass="form-control"></asp:DropDownList>
                </div>
            </div>            
            </div>
            <!-- linha 2 -->
                      <div class="row g-3">
            <div class="col-md-1">
                 <div class="form_group">
                    <span class="details">FAB/MOD.:</span>
                    <asp:TextBox ID="txtAno" runat="server"  Style="text-align: center" ForeColor="Blue" CssClass="form-control" placeholder="0000/0000" MaxLength="9"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-1">
                  <div class="form_group">
                     <span class="details">AQUISIÇÃO:</span>
                     <asp:TextBox ID="txtDataAquisicao" runat="server" data-mask="00/00/0000" Style="text-align: center" ForeColor="Blue" CssClass="form-control" placeholder="00/00/0000" MaxLength="10" Width="130px"></asp:TextBox>
                 </div>
            </div>
            <div class="col-md-2">
                <div class="form_group">
                    <span class="details">RENAVAM:</span>
                    <asp:TextBox ID="txtRenavam" runat="server" ForeColor="Blue" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="25"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-2">
                <div class="form_group">
                    <span class="details">CHASSI:</span>
                    <asp:TextBox ID="txtChassi" runat="server" ForeColor="Blue" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="30"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-1">
                <div class="form_group">
                    <span class="details">LICENCIAMENTO:</span>
                    <asp:TextBox ID="txtLicenciamento" runat="server" ForeColor="Blue" CssClass="form-control" placeholder="00/00/0000" MaxLength="10" data-mask="00/00/0000" Width="130px" Style="text-align: center"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-1">
                <div class="form_group">
                    <span class="details">TACÓGRAFO:</span>
                    <asp:DropDownList ID="ddlTacografo" runat="server" ForeColor="Blue" CssClass="form-control" width="130px">
                        <asp:ListItem Value="" Text=""></asp:ListItem>
                        <asp:ListItem Value="DIARIO" Text="DIARIO"></asp:ListItem>
                        <asp:ListItem Value="SEMANAL" Text="SEMANAL"></asp:ListItem>
                        <asp:ListItem Value="FITA" Text="FITA"></asp:ListItem>    
                        <asp:ListItem Value="OUTROS" Text="OUTROS"></asp:ListItem>
                    </asp:DropDownList><br />
                </div>
            </div>
            <div class="col-md-1">
                <div class="form_group">
                    <span class="details">MODELO:</span>
                    <asp:DropDownList ID="ddlModeloTacografo" runat="server" ForeColor="Blue" CssClass="form-control" Width="135px">
                        <asp:ListItem Value="" Text=""></asp:ListItem>
                        <asp:ListItem Value="MECANICO" Text="MECANICO"></asp:ListItem>
                        <asp:ListItem Value="ELETRONICO" Text="ELETRONICO"></asp:ListItem>                          
                        <asp:ListItem Value="OUTROS" Text="OUTROS"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-md-1">
                <div class="form_group">
                    <span class="details">COMPRIMENTO:</span>
                    <asp:TextBox ID="txtComprimento" runat="server" ForeColor="Blue" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="10"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-1">
                <div class="form_group">
                    <span class="details">LARGURA:</span>
                    <asp:TextBox ID="txtLargura" runat="server" ForeColor="Blue" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="10" ></asp:TextBox>
                </div>
            </div>
            <div class="col-md-1">
                <div class="form_group">
                    <span class="details">ALTURA:</span>
                    <asp:TextBox ID="txtAltura" runat="server" ForeColor="Blue" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="10" ></asp:TextBox>
                </div>
            </div>
        </div>       
                      <!-- linha 3 -->
                      <div class="row g-3">
            <div class="col-md-5">
                <div class="form_group">
                    <span class="details">MARCA:</span>
                    <asp:DropDownList ID="ddlMarca" name="nomeFiliais" runat="server" ForeColor="Blue" CssClass="form-control"></asp:DropDownList>
                </div>
            </div>
            <div class="col-md-5">
                <div class="form-group">
                    <span class="details">MODELO:</span>
                    <asp:TextBox ID="txtModelo" runat="server" ForeColor="Blue" CssClass="form-control" placeholder="" MaxLength="40"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-2">
                <div class="form_group">
                    <span class="details">COR:</span>
                    <asp:DropDownList ID="ddlCor" name="nomeFiliais" runat="server" ForeColor="Blue" CssClass="form-control"></asp:DropDownList>
                </div>
            </div>
        </div>
                      <!-- linha 4 -->
                      <div class="row g-3">
            <div class="col-md-2">
                <div class="form-group">
                    <span class="">MONITORAMENTO:</span>
                    <asp:DropDownList ID="ddlMonitoramento" runat="server" ForeColor="Blue" CssClass="form-control">
                        <asp:ListItem Value="" Text=""></asp:ListItem>
                        <asp:ListItem Value="MONITORADO" Text="MONITORADO"></asp:ListItem>
                        <asp:ListItem Value="RASTREADO" Text="RASTREADO"></asp:ListItem>
                        <asp:ListItem Value="TELEMONITORADO" Text="TELEMONITORADO"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-md-1">
                <div class="form-group">
                    <span class="details">CÓDIGO:</span>
                    <asp:TextBox ID="txtCodRastreador" runat="server" Style="text-align: center" ForeColor="Blue" CssClass="form-control select2+-" placeholder="" MaxLength="4"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-4">
                <div class="form_group">
                    <span class="details">TECNOLOGIA/RASTREADOR:</span>
                    <asp:DropDownList ID="ddlTecnologia" name="tecnologia" runat="server" ForeColor="Blue" CssClass="form-control select2" OnSelectedIndexChanged="ddlTecnologia_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                </div>
            </div>
            <div class="col-md-2">
                <div class="form-group">
                    <span class="details">ID:</span>
                    <asp:TextBox ID="txtId" runat="server" ForeColor="Blue" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="11"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-3">
                <div class="form-group">
                    <span class="">COMUNICAÇÃO:</span>
                    <asp:DropDownList ID="ddlComunicacao" runat="server" ForeColor="Blue" CssClass="form-control">
                        <asp:ListItem Value="" Text=""></asp:ListItem>
                        <asp:ListItem Value="GPS/DUPLO GPS" Text="GPS/DUPLO GPS"></asp:ListItem>
                        <asp:ListItem Value="GPS/CRPS" Text="GPS/CRPS"></asp:ListItem>
                        <asp:ListItem Value="GPS/GPRS GLOBAL" Text="GPS/GPRS GLOBAL"></asp:ListItem>
                        <asp:ListItem Value="GPS/GPRS+SATÉLITE" Text="GPS/GPRS+SATÉLITE"></asp:ListItem>
                        <asp:ListItem Value="GPS/SATÉLITE" Text="GPS/SATÉLITE"></asp:ListItem>
                        <asp:ListItem Value="NÃO TEM" Text="NÃO TEM"></asp:ListItem>
                        <asp:ListItem Value="RF/GPS/GPRS" Text="RF/GPS/GPRS"></asp:ListItem>
                        <asp:ListItem Value="OUTROS" Text="OUTROS"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
        </div>
                      <!-- linha 5 -->
                      <div class="row g-3">
            <div class="col-md-2">
                <div class="form-group">
                    <span class="">TIPO:</span>
                    <asp:DropDownList ID="ddlTipo" runat="server" CssClass="form-control">
                        <asp:ListItem Value="" Text=""></asp:ListItem>
                        <asp:ListItem Value="AGREGADO" Text="AGREGADO"></asp:ListItem>
                        <asp:ListItem Value="FROTA" Text="FROTA"></asp:ListItem>
                        <asp:ListItem Value="TERCEIRO" Text="TERCEIRO"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-md-1">
                <div class="form-group">
                    <span class="details">CÓDIGO:</span>
                    <asp:TextBox ID="txtCodTra" runat="server" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="11" AutoPostBack="true"></asp:TextBox>

                </div>
            </div>

            <div class="col-md-6">
                <div class="form_group">
                    <span class="details">PROPRIETÁRIO/TRANSPORTADORA:</span>
                    <asp:DropDownList ID="ddlAgregados" class="form-control select2" name="nomeProprietario" runat="server"   OnSelectedIndexChanged="ddlAgregados_SelectedIndexChanged" AutoPostBack="true" ></asp:DropDownList>
                </div>
            </div>

            <div class="col-md-3">
                <div class="form-group">
                    <span class="details">ANTT/RNTRC:</span>
                    <asp:TextBox ID="txtAntt" runat="server" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="11"></asp:TextBox>
                </div>
            </div>
        </div>
                      <!-- linha 6 -->
                      <div class="row g-3">
            <div class="col-md-2">
                <div class="form-group">
                    <span class="details">CADASTRADO EM:</span>
                    <asp:Label ID="lblDtCadastro" runat="server" Style="text-align: center" ForeColor="Blue" CssClass="form-control" placeholder="" maxlength="20"></asp:Label>
                </div>
            </div>
            <div class="col-md-5">
                <div class="form-group">
                    <span class="details">POR:</span>
                    <asp:TextBox ID="txtUsuCadastro" runat="server" ForeColor="Blue" CssClass="form-control" placeholder="" MaxLength="60"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-2">
                <div class="form-group">
                    <span class="details">CÓDIGO:</span>
                    <asp:TextBox ID="txtCodigo" runat="server" Style="text-align: center" ForeColor="Blue" CssClass="form-control" placeholder="" maxlength="10"></asp:TextBox>
                </div>
            </div>
                        <div class="col-md-3">
                <div class="form-group">
                    <span class="details">PATRIMÔNIO:</span>
                    <asp:TextBox ID="txtControlePatrimonio" runat="server" Style="text-align: center" ForeColor="Blue" CssClass="form-control" placeholder="" maxlength="20"></asp:TextBox>
                </div>
            </div>
        </div>
                      <!-- linha 7 -->
                      <div class="row g-3">
            <div class="col-md-1">

                <asp:Button ID="btnSalvar1" CssClass="btn btn-outline-success  btn-lg" runat="server" OnClick="btnSalvar1_Click" Text="Cadastrar" />
            </div>
            <div class="col-md-2">
                &nbsp;&nbsp;&nbsp;
                <a href="ConsultaVeiculos.aspx" class="btn btn-outline-danger btn-lg"> Cancelar               
                </a>
            </div>
        </div>
                 </div>
             </div>
        </section>

    </div>


    <footer class="main-footer">
        <div class="float-right d-none d-sm-block">
            <b>Version</b> 2.1.0
 
        </div>
        <strong>Copyright &copy; 2021-2025 Capit Logística.</strong> Todos os direitos reservados.
    </footer>
    <!-- Page specific script -->
    <script>
        $(function () {
            //Initialize Select2 Elements
            $('.select2').select2()

            //Initialize Select2 Elements
            $('.select2bs4').select2({
                theme: 'bootstrap4'
            })

            //Datemask dd/mm/yyyy
            $('#datemask').inputmask('dd/mm/yyyy', { 'placeholder': 'dd/mm/yyyy' })
            //Datemask2 mm/dd/yyyy
            $('#datemask2').inputmask('mm/dd/yyyy', { 'placeholder': 'mm/dd/yyyy' })
            //Money Euro
            $('[data-mask]').inputmask()

            //Date picker
            $('#reservationdate').datetimepicker({
                format: 'L'
            });


            //Date range as a button
            $('#daterange-btn').daterangepicker(
                {
                    ranges: {
                        'Today': [moment(), moment()],
                        'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
                        'Last 7 Days': [moment().subtract(6, 'days'), moment()],
                        'Last 30 Days': [moment().subtract(29, 'days'), moment()],
                        'This Month': [moment().startOf('month'), moment().endOf('month')],
                        'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
                    },
                    startDate: moment().subtract(29, 'days'),
                    endDate: moment()
                },
                function (start, end) {
                    $('#reportrange span').html(start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'))
                }
            )


        })


    </script>

</asp:Content>
