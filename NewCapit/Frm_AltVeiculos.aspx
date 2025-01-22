<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Frm_AltVeiculos.aspx.cs" Inherits="NewCapit.Frm_AltVeiculos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-5">
    <div class="d-sm-flex align-items-center justify-content-between mb-4">
        <h3 class="h3 mb-2 text-gray-800"><i class="fas fa-shipping-fast"></i> VEÍCULO </h3>
        <h3>ATUALIZA CADASTRO</h3>
    </div>
    <hr />   

    <div class="row g-3">
        <div class="col-md-1">
            <div class="form-group">
                <span class="details">ID/FROTA:</span>
                <asp:TextBox ID="txtCodVei" Style="text-align: center" runat="server" ForeColor="Blue" CssClass="form-control" placeholder="" MaxLength="9"></asp:TextBox>
            </div>
        </div>        
        <div class="col-md-1">
            <div class="form-group">
                <span class="details">PLACA:</span>
                <asp:TextBox ID="txtPlaca" Style="text-align: center" runat="server" ForeColor="Blue" CssClass="form-control" placeholder="" MaxLength="8"></asp:TextBox>
            </div>
        </div>
        <div class="col-md-3">
            <div class="form_group">
                <span class="details">TIPO DE VEÍCULO:</span>
                <asp:DropDownList ID="cboTipo" runat="server" ForeColor="Blue" CssClass="form-control" AccessKey>
                    <asp:ListItem Value="" Text="SELECIONE"></asp:ListItem>
                    <asp:ListItem Value="BITREM" Text="BITREM"></asp:ListItem>
                    <asp:ListItem Value="BITRUCK" Text="BITRUCK"></asp:ListItem>
                    <asp:ListItem Value="CAVALO SIMPLES" Text="CAVALO SIMPLES"></asp:ListItem>
                    <asp:ListItem Value="TOCO" Text="TOCO"></asp:ListItem>
                    <asp:ListItem Value="VEICULO 3/4" Text="VEICULO 3/4"></asp:ListItem>
                    <asp:ListItem Value="TRUCK" Text="TRUCK"></asp:ListItem>
                    <asp:ListItem Value="CAVALO 4 EIXOS" Text="CAVALO 4 EIXOS"></asp:ListItem>
                    <asp:ListItem Value="OUTROS" Text="OUTROS"></asp:ListItem>
                </asp:DropDownList><br />
            </div>
        </div>
        <div class="col-md-1">
            <div class="form_group">
                <span class="details">ANO/MOD.:</span>
                <asp:TextBox ID="txtAno" runat="server" data-mask="0000/0000" Style="text-align: center" ForeColor="Blue" CssClass="form-control" placeholder="" MaxLength="9"></asp:TextBox>
            </div>
        </div>        
        <div class="col-md-4">
            <div class="form_group">
                <span class="details">FILIAL:</span>
                <asp:DropDownList ID="cbFiliais" name="nomeFiliais" runat="server" ForeColor="Blue" CssClass="form-control"></asp:DropDownList>
            </div>
        </div>
        <div class="col-md-1">
            <div class="form_group">
                <span class="details">CADASTRO:</span>
                <asp:TextBox ID="txtCadastro" runat="server" ForeColor="Blue" CssClass="form-control" placeholder="99/99/9999" MaxLength="10" Style="text-align: center" Width="130px"></asp:TextBox>
            </div>
        </div>
        <div class="col-md-1">
            <div class="form-group">
                <span class="">STATUS:</span>
                <asp:DropDownList ID="status" runat="server" ForeColor="Blue" CssClass="form-control">
                    <asp:ListItem Value="ATIVO" Text="ATIVO"></asp:ListItem>
                    <asp:ListItem Value="INATIVO" Text="INATIVO"></asp:ListItem>
                </asp:DropDownList></>
            </div>
        </div>
    </div>

    <div class="row g-3">
        <div class="col-md-3">
            <div class="form_group">
                <span class="details">RENAVAM:</span>
                <asp:TextBox ID="txtRenavam" runat="server" ForeColor="Blue" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="25"></asp:TextBox>
            </div>
        </div>   
        <div class="col-md-3">
            <div class="form_group">
                <span class="details">CHASSI:</span>
                <asp:TextBox ID="txtChassi" runat="server" ForeColor="Blue" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="30"></asp:TextBox>
            </div>
        </div>   
        <div class="col-md-1">
            <div class="form_group">
                <span class="details">LICENCIAMENTO:</span>
                <asp:TextBox ID="txtLicenciamento" runat="server" ForeColor="Blue" CssClass="form-control" placeholder="" MaxLength="10" data-mask="00/00/0000" Width="130px" Style="text-align: center"></asp:TextBox>
            </div>
        </div>   
        <div class="col-md-2">
            <div class="form_group">
                <span class="details">PROT. CET:</span>
                <asp:TextBox ID="txtProtocolo" runat="server" ForeColor="Blue" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="10" ></asp:TextBox>
            </div>
        </div> 
        <div class="col-md-1">
            <div class="form_group">
                <span class="details">VALIDADE:</span>
                <asp:TextBox ID="txtValCET" runat="server" ForeColor="Blue" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="10" Width="130px" data-mask="00/00/0000"></asp:TextBox>
            </div>
        </div> 
        <div class="col-md-2">
            <div class="form_group">
                <span class="details">OPACIDADE:</span>
                <asp:TextBox ID="txtOpacidade" runat="server" ForeColor="Blue" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="10" data-mask="00/00/0000"></asp:TextBox>
            </div>
        </div>
    </div><br />

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
                <asp:TextBox ID="txtModelo" runat="server" ForeColor="Blue" CssClass="form-control" placeholder="" maxlength="40"></asp:TextBox>
            </div>
        </div>
        <div class="col-md-2">
            <div class="form_group">
                <span class="details">COR:</span>
                <asp:DropDownList ID="ddlCor" name="nomeFiliais" runat="server" ForeColor="Blue" CssClass="form-control"></asp:DropDownList>
            </div>
        </div>
    </div>
    <div class="row g-3">
        <div class="col-md-2">
            <div class="form-group">
                <span class="">MONITORAMENTO:</span>
                <asp:DropDownList ID="ddlMonitoramento" runat="server" ForeColor="Blue" CssClass="form-control">
                    <asp:ListItem Value="" Text="SELECIONE"></asp:ListItem>
                    <asp:ListItem Value="MONITORADO" Text="MONITORADO"></asp:ListItem>
                    <asp:ListItem Value="RASTREADO" Text="RASTREADO"></asp:ListItem>
                    <asp:ListItem Value="TELEMONITORADO" Text="TELEMONITORADO"></asp:ListItem>
                </asp:DropDownList></>
            </div>
        </div>
        <div class="col-md-1">
            <div class="form-group">
                <span class="details">CÓDIGO:</span>
                <asp:TextBox ID="txtCodRastreador" runat="server" Style="text-align: center" ForeColor="Blue" CssClass="form-control" placeholder="" MaxLength="4"></asp:TextBox>
            </div>
        </div>
        <div class="col-md-4">
            <div class="form_group">
                <span class="details">TECNOLOGIA/RASTREADOR:</span>
                <asp:DropDownList ID="ddlTecnologia" name="tecnologia" runat="server" ForeColor="Blue" CssClass="form-control"></asp:DropDownList>
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
                    <asp:ListItem Value="" Text="SELECIONE"></asp:ListItem>
                    <asp:ListItem Value="GPS/DUPLO GPS" Text="GPS/DUPLO GPS"></asp:ListItem>
                    <asp:ListItem Value="GPS/CRPS" Text="GPS/CRPS"></asp:ListItem>
                    <asp:ListItem Value="GPS/GPRS GLOBAL" Text="GPS/GPRS GLOBAL"></asp:ListItem>
                    <asp:ListItem Value="GPS/GPRS+SATÉLITE" Text="GPS/GPRS+SATÉLITE"></asp:ListItem>
                    <asp:ListItem Value="GPS/SATÉLITE" Text="GPS/SATÉLITE"></asp:ListItem>
                    <asp:ListItem Value="NÃO TEM" Text="NÃO TEM"></asp:ListItem>
                    <asp:ListItem Value="RF/GPS/GPRS" Text="RF/GPS/GPRS"></asp:ListItem>
                    <asp:ListItem Value="OUTROS" Text="OUTROS"></asp:ListItem>
                </asp:DropDownList></>
            </div>
        </div>
    </div>
    
    <div class="row g-3">
        <div class="col-md-1">
            <div class="form-group">
                <span class="details">CÓDIGO:</span>
                <asp:TextBox ID="txtCodTra" runat="server" ForeColor="Blue" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="4"></asp:TextBox>
            </div>
        </div>
        <div class="col-md-4">
            <div class="form_group">
                <span class="details">PROPRIETÁRIO/TRANSPORTADORA:</span>
                <asp:DropDownList ID="ddlTransportadora" name="nomeProprietario" runat="server" ForeColor="Blue" CssClass="form-control"></asp:DropDownList>
            </div>
        </div>
        <div class="col-md-2">
            <div class="form-group">
                <span class="details">ANTT/RNTRC:</span>
                <asp:TextBox ID="txtAntt" runat="server" Style="text-align: center" ForeColor="Blue" CssClass="form-control" placeholder="" MaxLength="11"></asp:TextBox>
            </div>
        </div>
        <div class="col-md-1">
            <div class="form-group">
                <span class="details">CÓDIGO:</span>
                <asp:TextBox ID="txtCodMot" runat="server" ForeColor="Blue" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="4"></asp:TextBox>
            </div>
        </div>
        <div class="col-md-4">
            <div class="form_group">
                <span class="details">MOTORISTA:</span>
                <asp:DropDownList ID="ddlMotorista" name="nomeMotorista" runat="server" ForeColor="Blue" CssClass="form-control"></asp:DropDownList>
            </div>
        </div>
    </div>
    <div class="row g-3">
        <div class="col-md-2">
            <div class="form-group">
                <span class="">TIPO:</span>
                <asp:DropDownList ID="ddlTipo" runat="server" ForeColor="Blue" CssClass="form-control">
                    <asp:ListItem Value="" Text="SELECIONE"></asp:ListItem>
                    <asp:ListItem Value="AGREGADO" Text="AGREGADO"></asp:ListItem>
                    <asp:ListItem Value="FROTA" Text="FROTA"></asp:ListItem>
                    <asp:ListItem Value="TERCEIRO" Text="TERCEIRO"></asp:ListItem>                    
                </asp:DropDownList></>
            </div>
        </div>
        <div class="col-md-2">
            <div class="form-group">
                <span class="">CARRETA:</span>
                <asp:DropDownList ID="ddlCarreta" runat="server" ForeColor="Blue" CssClass="form-control">
                    <asp:ListItem Value="" Text="SELECIONE"></asp:ListItem>
                    <asp:ListItem Value="PROPRIA" Text="PROPRIA"></asp:ListItem>
                    <asp:ListItem Value="TRANSNOVAG" Text="TRANSNOVAG"></asp:ListItem> 
                </asp:DropDownList></>
            </div>
        </div>
<<<<<<< HEAD
       
=======
        <div class="col-md-1">
            <div class="form-group">
                <span id="numeroReb1" class="details">REB 999999:</span>
                <asp:TextBox ID="txtReb1" runat="server" ForeColor="Blue" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="8"></asp:TextBox>
            </div>
        </div>
        <div class="col-md-1">
            <div class="form-group">
                <span id="numeroReb2" class="details">REB 999999:</span>
                <asp:TextBox ID="txtReb2" runat="server" ForeColor="Blue" CssClass="form-control" Style="text-align: center" placeholder="" MaxLength="8"></asp:TextBox>
            </div>
        </div>        
        <div class="col-md-6">
            <div class="form-group">
                <span class="">COMPOSIÇÃO:</span>
                <asp:DropDownList ID="ddlComposicao" runat="server" ForeColor="Blue" CssClass="form-control">
                     <asp:ListItem Value="" Text="SELECIONE"></asp:ListItem>
                    <asp:ListItem Value="CAVALO SIMPLES COM CARRETA VANDERLEIA ABERTA" Text="SELECIONE">CAVALO SIMPLES COM CARRETA VANDERLEIA ABERTA</asp:ListItem>
                    <asp:ListItem Value="CAVALO SIMPLES COM CARRETA SIMPLES TOTAL SIDER" Text="">CAVALO SIMPLES COM CARRETA SIMPLES TOTAL SIDER</asp:ListItem>
                    <asp:ListItem Value="CAVALO SIMPLES COM CARRETA SIMPLES(LS) ABERTA" Text="">CAVALO SIMPLES COM CARRETA SIMPLES(LS) ABERTA</asp:ListItem>
                    <asp:ListItem Value="CAVALO SIMPLES COM CARRETA VANDERLEIA TOTAL SIDER" Text="">CAVALO SIMPLES COM CARRETA VANDERLEIA TOTAL SIDER</asp:ListItem>
                    <asp:ListItem Value="CAVALO TRUCADO COM CARRETA VANDERLEIA ABERTA" Text="">CAVALO TRUCADO COM CARRETA VANDERLEIA ABERTA</asp:ListItem>
                    <asp:ListItem Value="CAVALO TRUCADO COM CARRETA SIMPLES TOTAL SIDER" Text="">CAVALO TRUCADO COM CARRETA SIMPLES TOTAL SIDER</asp:ListItem>
                    <asp:ListItem Value="CAVALO TRUCADO COM CARRETA SIMPLES(LS) ABERTA" Text="">CAVALO TRUCADO COM CARRETA SIMPLES(LS) ABERTA</asp:ListItem>
                    <asp:ListItem Value="CAVALO TRUCADO COM CARRETA VANDERLEIA TOTAL SIDER" Text="">CAVALO TRUCADO COM CARRETA VANDERLEIA TOTAL SIDER</asp:ListItem>
                    <asp:ListItem Value="TRUCK" Text="">TRUCK</asp:ListItem>
                    <asp:ListItem Value="BITRUCK" Text="">BITRUCK</asp:ListItem>
                    <asp:ListItem Value="BITREM" Text="">BITREM</asp:ListItem>
                    <asp:ListItem Value="TOCO" Text="">TOCO</asp:ListItem>
                    <asp:ListItem Value="VEICULO 3/4" Text="">VEICULO 3/4</asp:ListItem>
                    <asp:ListItem Value="CAVALO SIMPLES COM PRANCHA" Text="">CAVALO SIMPLES COM PRANCHA</asp:ListItem>
                    <asp:ListItem Value="CAVALO TRUCADO COM PRANCHA" Text="">CAVALO TRUCADO COM PRANCHA</asp:ListItem>
                    <asp:ListItem Value="CAVALO TRUCADO COM CARRETA LS TOTAL SIDER PRANCHA" Text="">CAVALO TRUCADO COM CARRETA LS TOTAL SIDER LISA</asp:ListItem>
                    <asp:ListItem Value="CAVALO TRUCADO COM CARRETA LS TOTAL SIDER PRANCHA" Text="">CAVALO SIMPLES COM CARRETA LS TOTAL SIDER LISA</asp:ListItem>                                    
                </asp:DropDownList></>
            </div>
        </div>

    </div>
    <div class="row g-3">
        <div class="col-md-1">
            <div class="form-group">
                <span class="details">EIXOS:</span>
                <asp:TextBox ID="txtEixos" runat="server" ForeColor="Blue" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="6"></asp:TextBox>
            </div>
        </div>
        <div class="col-md-1">
            <div class="form-group">
                <span class="details">CAPACIDADE:</span>
                <asp:TextBox ID="txtCap" runat="server" ForeColor="Blue" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="6"></asp:TextBox>
            </div>
        </div>
>>>>>>> 32a0be2c431546afa6d0b3ca80eaa50750c28b3e
        <div class="col-md-1">
            <div class="form-group">
                <span class="details">TARA:</span>
                <asp:TextBox ID="txtTara" runat="server" ForeColor="Blue" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="6"></asp:TextBox>
            </div>
        </div>
        <div class="col-md-6">
            <div class="form-group">
                <span class="">COMPOSIÇÃO:</span>
                <asp:DropDownList ID="ddlComposicao" runat="server" ForeColor="Blue" CssClass="form-control" OnSelectedIndexChanged="ddlComposicao_SelectedIndexChanged" AutoPostBack="true">
                     <asp:ListItem Value="" Text="SELECIONE"></asp:ListItem>
                    <asp:ListItem Value="CAVALO SIMPLES COM CARRETA VANDERLEIA ABERTA" Text="SELECIONE">CAVALO SIMPLES COM CARRETA VANDERLEIA ABERTA</asp:ListItem>
                    <asp:ListItem Value="CAVALO SIMPLES COM CARRETA SIMPLES TOTAL SIDER" Text="">CAVALO SIMPLES COM CARRETA SIMPLES TOTAL SIDER</asp:ListItem>
                    <asp:ListItem Value="CAVALO SIMPLES COM CARRETA SIMPLES(LS) ABERTA" Text="">CAVALO SIMPLES COM CARRETA SIMPLES(LS) ABERTA</asp:ListItem>
                    <asp:ListItem Value="CAVALO SIMPLES COM CARRETA VANDERLEIA TOTAL SIDER" Text="">CAVALO SIMPLES COM CARRETA VANDERLEIA TOTAL SIDER</asp:ListItem>
                    <asp:ListItem Value="CAVALO TRUCADO COM CARRETA VANDERLEIA ABERTA" Text="">CAVALO TRUCADO COM CARRETA VANDERLEIA ABERTA</asp:ListItem>
                    <asp:ListItem Value="CAVALO TRUCADO COM CARRETA SIMPLES TOTAL SIDER" Text="">CAVALO TRUCADO COM CARRETA SIMPLES TOTAL SIDER</asp:ListItem>
                    <asp:ListItem Value="CAVALO TRUCADO COM CARRETA SIMPLES(LS) ABERTA" Text="">CAVALO TRUCADO COM CARRETA SIMPLES(LS) ABERTA</asp:ListItem>
                    <asp:ListItem Value="CAVALO TRUCADO COM CARRETA VANDERLEIA TOTAL SIDER" Text="">CAVALO TRUCADO COM CARRETA VANDERLEIA TOTAL SIDER</asp:ListItem>
                    <asp:ListItem Value="TRUCK" Text="">TRUCK</asp:ListItem>
                    <asp:ListItem Value="BITRUCK" Text="">BITRUCK</asp:ListItem>
                    <asp:ListItem Value="BITREM 7 EIXOS" Text="">BITREM 7 EIXOS</asp:ListItem>
                    <asp:ListItem Value="TOCO" Text="">TOCO</asp:ListItem>
                    <asp:ListItem Value="VEICULO 3/4" Text="">VEICULO 3/4</asp:ListItem>
                    <asp:ListItem Value="CAVALO SIMPLES COM PRANCHA" Text="">CAVALO SIMPLES COM PRANCHA</asp:ListItem>
                    <asp:ListItem Value="CAVALO TRUCADO COM PRANCHA" Text="">CAVALO TRUCADO COM PRANCHA</asp:ListItem>
                    <asp:ListItem Value="CAVALO TRUCADO COM CARRETA LS TOTAL SIDER PRANCHA" Text="">CAVALO TRUCADO COM CARRETA LS TOTAL SIDER LISA</asp:ListItem>
                    <asp:ListItem Value="CAVALO TRUCADO COM CARRETA LS TOTAL SIDER PRANCHA" Text="">CAVALO SIMPLES COM CARRETA LS TOTAL SIDER LISA</asp:ListItem>                                    
                </asp:DropDownList></>
            </div>
        </div>
        <div class="col-md-1">
        <div class="form-group">
            <span class="details">EIXOS:</span>
            <asp:TextBox ID="txtEixos" runat="server" ForeColor="Blue" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="6"></asp:TextBox>
        </div>
</div>

    </div>
    <div class="row g-3">
        
        <div class="col-md-1">
                 <div class="form-group">
                     <span id="numeroReb1" class="details">REB 999999:</span>
                     <asp:TextBox ID="txtReb1" runat="server" ForeColor="Blue" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="8"></asp:TextBox>
                 </div>
        </div>
        <div class="col-md-1">
                 <div class="form-group">
                     <span id="numeroReb2" class="details">REB 999999:</span>
                     <asp:TextBox ID="txtReb2" runat="server" ForeColor="Blue" CssClass="form-control" Style="text-align: center" placeholder="" MaxLength="8"></asp:TextBox>
                 </div>
        </div>  
        <div class="col-md-1">
            <div class="form-group">
                <span class="details">CAPACIDADE:</span>
                <asp:TextBox ID="txtCap" runat="server" ForeColor="Blue" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="6"></asp:TextBox>
            </div>
        </div>
        
        <div class="col-md-1">
            <div class="form-group">
                <span class="details">TOL. %:</span>
                <asp:TextBox ID="txtTolerancia" runat="server" ForeColor="Blue" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="6"></asp:TextBox>
            </div>
        </div>
        <div class="col-md-1">
            <div class="form-group">
                <span class="details">CARGA LIQ.:</span>
                <asp:TextBox ID="txtPBT" runat="server" ForeColor="Blue" Style="text-align: center" CssClass="form-control" placeholder="" MaxLength="6"></asp:TextBox>
            </div>
        </div>
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
    </div>

    <div class="row g-3">        
        <div class="col-md-1">
            
            <asp:Button ID="btnSalvar1" CssClass="btn btn-outline-success  btn-lg" runat="server"  Text="Cadastra" />
        </div>
        <div class="col-md-1">
            
            <a href="ConsultaVeiculos.aspx" class="btn btn-outline-danger btn-lg">Cancelar               
            </a>
        </div>
        <div class="col-md-1">
           
            <button type="button" class="btn btn-outline-info  btn-lg">Mapa </button>
        </div>
    </div>

</div>

<script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.5.0/jquery.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/jquery.mask/1.14.16/jquery.mask.js"></script>
</asp:Content>
