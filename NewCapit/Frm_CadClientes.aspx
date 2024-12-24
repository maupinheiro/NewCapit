<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Frm_CadClientes.aspx.cs" Inherits="NewCapit.Frm_CadClientes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-5">
        <h3>..: NOVO CADASTRO :..</h3>
        <hr />
        <div class="row g-3">
            <div class="col-md-1">
                <div class="form-group">
                    <span class="details">CÓDIGO:</span>
                    <input type="text" id="txtCodCli" class="form-control" placeholder="" maxlength="11" autofocus required>
                </div> 
            </div>
            <div class="col-md-3">
                <div class="form_group">
                    <span class="details">TIPO:</span>
                    <select name="tipo" id="cboTipo" class="form-control">
                        <option value="CLIENTE">CLIENTE</option>
                        <option value="EMBARCADOR">EMBARCADOR</option>
                        <option value="TRANSPORTADOR">TRANSPORTADOR</option>
                        <option value="OPERADOR LOGÍSTICO">OPERADOR LOGÍSTICO</option>
                    </select>
                </div>
            </div>
            <div class="col-md-3">
                <div class="form_group">
                        <span class="details">UNIDADE:</span>
                        <input type="text" id="txtUnidade" class="form-control" placeholder="" maxlength="45" autofocus required>
                    </div>

            </div>
            <div class="col-md-2">
                <span class="">REGIÃO DO PAÍS:</span>
                <select name="regiao" id="cboRegiao" class="form-control">
                    <option value="NORTE">NORTE</option>
                    <option value="SUL">SUL</option>
                    <option value="SUDESTE">SUDESTE</option>
                    <option value="CENTRO-OESTE">CENTRO-OESTE</option>
                    <option value="NORDESTE">NORDESTE</option>
                </select>
            </div>
            <div class="col-md-1">
                <div class="form-group">
                    <span class="details">SAPIENS:</span>
                    <input type="text" id="txtCodSapiens" class="form-control" placeholder="" maxlength="10">
                </div> 
            </div>
            <div class="col-md-1">
                <div class="form-group">
                    <span class="details">CÓD.VW:</span>
                    <input type="text" id="txtCodVw" class="form-control" placeholder="" maxlength="10">
                </div> 
            </div>           
            <div class="col-md-1">
                <div class="form-group">
                    <span class="">SITUAÇÃO:</span>
                    <select name="status" id="status" class="form-control">
                        <option value="ATIVO">ATIVO</option>
                        <option value="INATIVO">INATIVO</option>                        
                    </select>
                </div> 
            </div>            
        </div>

        <div class="row g-3">
            <div class="col-md-9">
                <div class="form-group">
                    <span class="details">RAZÃO SOCIAL:</span>
                    <input type="text" id="txtRazCli" class="form-control" placeholder="" maxlength="50" required>
                </div>
            </div>
            
            <div class="col-md-3">
                <div class="form-group">
                    <span class="details">CNPJ:</span>
                    <input type="text" id="txtCnpj" class="form-control" placeholder="" maxlength="20" required>
                </div>
            </div>            
        </div>

        <div class="row g-3">
            <div class="col-md-9">
                <div class="form-group">
                    <span class="details">NOME FANTASIA:</span>
                    <input type="text" id="txtNomCli" class="form-control" placeholder="" maxlength="50" required>
                </div>
            </div>
    
            <div class="col-md-3">
                <div class="form-group">
                    <span class="details">INSC. ESTADUAL:</span>
                    <input type="text" id="txtInscEstadual" class="form-control" placeholder="" maxlength="15" required>
                </div>
            </div>            
        </div>

        <div class="row g-3">
            <div class="col-md-2">
                <div class="form-group">
                    <span class="details">CONTATO:</span>
                    <input type="text" id="txtConCli" class="form-control" placeholder="" maxlength="30" required>
                </div>
            </div>    
            <div class="col-md-2">
                <div class="form-group">
                    <span class="details">FONE FIXO:</span>
                    <input type="text" id="txtTc1Cli" class="form-control" placeholder="(99) 9999-9999" maxlength="17" required>
                </div>
            </div> 
            <div class="col-md-1">
                <div class="form-group">
                    <span class="details">RAMAL:</span>
                    <input type="text" id="txtRamal" class="form-control" placeholder="9999" maxlength="4" >
                </div>
            </div>
            <div class="col-md-2">
                <div class="form-group">
                    <span class="details">CELULAR:</span>
                    <input type="text" id="txtTc2Cli" class="form-control" placeholder="(99) 9 9999-9999" maxlength="16">
                </div>
            </div> 
            <div class="col-md-3">
                <div class="form-group">
                    <span class="details">PROGRAMADORES:</span>
                    <input type="text" id="txtProgramador" class="form-control" placeholder="" maxlength="35" >
                </div>
            </div> 
            <div class="col-md-2">
                <div class="form-group">
                    <span class="details">TELEFONE/RAMAL:</span>
                    <input type="text" id="txtContato" class="form-control" placeholder="" maxlength="25" >
                </div>
            </div> 
        </div>

        <div class="row g-3">
             <div class="col-md-12">
                 <div class="form-group">
                     <span class="details">E-MAIL(S):</span>
                     <input type="text" id="txtEmail" class="form-control" placeholder="" maxlength="200" >
                 </div>
             </div> 
        </div>

        <div class="row g-3">
           <div class="col-md-2">
              <div class="form-group">
                 <span class="details">CEP:</span>
                 <input type="text" id="txtCepCli" style=text-align:center class="form-control" placeholder="99999-999" maxlength="9" required>                  
              </div>
           </div> 
           <div class="col-md-1">
                <br />
                <button type="submit" id="btnCep" class="btn btn-outline-warning">Pesquisar</button>              
           </div>
           <div class="col-md-6">
               <div class="form-group">
                  <span class="details">ENDEREÇO:</span>
                  <input type="text" id="txtEndCli" class="form-control" placeholder="" maxlength="60" required>                  
               </div>
           </div> 
           <div class="col-md-1">
               <div class="form-group">
                  <span class="details">Nº:</span>
                  <input type="text" id="txtNumero" style=text-align:center class="form-control" placeholder="" maxlength="4" required>                  
               </div>
           </div> 
           <div class="col-md-2">
                <div class="form-group">
                   <span class="details">COMPLEMENTO:</span>
                   <input type="text" id="txtComplemento" style=text-align:center class="form-control" placeholder="" maxlength="15">                  
                </div>
           </div> 
    </div>

        <div class="row g-3">
            <div class="col-md-2">
                <div class="form-group">
                    <span class="details">BAIRRO:</span>
                    <input type="text" id="txtBaiCli" class="form-control" placeholder="" maxlength="60" required>
                </div>
            </div>    
            <div class="col-md-3">
                <div class="form-group">
                    <span class="details">CIDADE:</span>
                    <input type="text" id="txtCidCli" class="form-control" placeholder="" maxlength="60" required>
                </div>
            </div> 
            <div class="col-md-1">
                <div class="form-group">
                    <span class="details">UF:</span>
                    <input type="text" id="txtEstCli" style=text-align:center class="form-control" placeholder="" maxlength="2" required>
                </div>
            </div> 
            <div class="col-md-2">
                <div class="form-group">
                    <span class="details">LATITUDE:</span>
                    <input type="text" id="latitude" style=text-align:center class="form-control" placeholder="" maxlength="40">
                </div>
            </div> 
            <div class="col-md-2">
                <div class="form-group">
                    <span class="details">LONGITUDE:</span>
                    <input type="text" id="longitude" style=text-align:center class="form-control" placeholder="" maxlength="40">
                </div>
            </div>
            <div class="col-md-1">
                <div class="form-group">
                    <span class="details">RAIO:</span>
                    <input type="number" id="txtRaio" style=text-align:center class="form-control" placeholder="" min=1 max=2000>
                </div>
            </div> 
            <div class="col-md-1">
                 <br />
                 <button type="button" class="btn btn-outline-warning">Pesquisar</button>              
            </div>

        </div>

        <div class="row g-3">   
            <div class="col-md-1">
               <br />
                <button type="button" class="btn btn-outline-info  btn-lg"> Mapa </button>              
            </div>
            <div class="col-md-1">
               <br />
                <button type="button" class="btn btn-outline-success  btn-lg"> Salvar </button>              
            </div>
            <div class="col-md-1">
                <br />
                <a href="ConsultaClientes.aspx" class="btn btn-outline-danger btn-lg">
                    Cancelar               
                </a>                              
            </div>
        </div>

    </div>
    
          
  

</asp:Content>
