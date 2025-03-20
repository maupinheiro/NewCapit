﻿<%@ Page Title="" Language="C#" MasterPageFile="Main.Master" AutoEventWireup="true" CodeBehind="Frm_CadClientes.aspx.cs" Inherits="NewCapit.dist.pages.Frm_CadClientes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <div class="content-wrapper">
      <section class="content">
          <div class="container-fluid">
              <br />
              <div class="card card-success">
                  <div class="card-header">
                      <h3 class="card-title">CLIENTES - NOVO CADASTRO</h3>
                  </div>
              </div>
              <div class="card-header">
                  <div class="row g-3">
                      <div class="col-md-1">
                          <div class="form-group">
                              <span class="details">CÓDIGO:</span>
                              <asp:TextBox ID="txtCodCli" runat="server" CssClass="form-control" placeholder="" MaxLength="11" ></asp:TextBox>
                          </div>
                      </div>
                      <div class="col-md-1">
                          <br />
                          <asp:Button ID="btnCliente" runat="server" Text="Pesquisar" CssClass="btn btn-outline-warning" OnClick="btnCliente_Click"  />
                      </div>
                      <div class="col-md-2">
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
                      <div class="col-md-3">
                          <div class="form_group">
                              <span class="details">UNIDADE:</span>
                              <asp:TextBox ID="txtUnidade" runat="server" ForeColor="Blue" CssClass="form-control" placeholder="" MaxLength="45"></asp:TextBox>
                          </div>
                      </div>
                      <div class="col-md-2">
                          <span class="">REGIÃO DO PAÍS:</span>
                          <asp:DropDownList ID="cboRegiao" runat="server" ForeColor="Blue" CssClass="form-control" Width="250px">
                              <asp:ListItem Value="" Text="SELECIONE"></asp:ListItem>
                              <asp:ListItem Value="NORTE" Text="NORTE"></asp:ListItem>
                              <asp:ListItem Value="SUL" Text="SUL"></asp:ListItem>
                              <asp:ListItem Value="SUDESTE" Text="SUDESTE"></asp:ListItem>
                              <asp:ListItem Value="CENTRO-OESTE" Text="CENTRO-OESTE"></asp:ListItem>
                              <asp:ListItem Value="NORDESTE" Text="NORDESTE"></asp:ListItem>
                          </asp:DropDownList><br />
                      </div>

                      <div class="col-md-1">
                          <div class="form-group">
                              <span class="details">SAPIENS:</span>
                              <asp:TextBox ID="txtCodSapiens" runat="server" ForeColor="Blue" CssClass="form-control" placeholder="" MaxLength="10"></asp:TextBox>
                          </div>
                      </div>

                      <div class="col-md-1">
                          <div class="form-group">
                              <span class="details">CÓD.VW:</span>
                              <asp:TextBox ID="txtCodVw" runat="server" ForeColor="Blue" CssClass="form-control" placeholder="" MaxLength="10"></asp:TextBox>
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
                      <div class="col-md-2">
                          <div class="form-group">
                              <span class="details">CNPJ:</span>
                              <asp:TextBox ID="txtCnpj" data-mask="00.000.000/0000-00" ForeColor="Blue" runat="server" class="form-control"></asp:TextBox>
                          </div>
                      </div>
                      <div class="col-md-1">
                          <br />
                          <asp:Button ID="btnCnpj" runat="server" Text="Pesquisar" CssClass="btn btn-outline-warning" OnClick="btnCnpj_Click" />
                      </div>
                      <div class="col-md-6">
                          <div class="form-group">
                              <span class="details">RAZÃO SOCIAL:</span>
                              <asp:TextBox ID="txtRazCli" runat="server" ForeColor="Blue" CssClass="form-control" value=""></asp:TextBox>
                          </div>
                      </div>
                      <div class="col-md-1">
                          <div class="form-group">
                              <span class="details">TIPO:</span>
                              <asp:TextBox ID="txtTipo" runat="server" ForeColor="Blue" CssClass="form-control" value=""></asp:TextBox>
                          </div>
                      </div>
                      <div class="col-md-1">
                          <div class="form-group">
                              <span class="details">ABERTURA:</span>
                              <asp:TextBox ID="txtAbertura" runat="server" ForeColor="Blue" CssClass="form-control" Width="130px" value=""></asp:TextBox>
                          </div>
                      </div>
                      <div class="col-md-1">
                          <div class="form-group">
                              <span class="details">SITUAÇÃO:</span>
                              <asp:TextBox ID="txtSituacao" runat="server" ForeColor="Blue" CssClass="form-control" value=""></asp:TextBox>
                          </div>
                      </div>

                  </div>
                  <div class="row g-3">
                      <div class="col-md-3">
                          <div class="form-group">
                              <span class="details">NOME FANTASIA:</span>
                              <asp:TextBox ID="txtNomCli" runat="server" CssClass="form-control" placeholder="" MaxLength="50" ForeColor="Blue"></asp:TextBox>
                          </div>
                      </div>
                      <div class="col-md-2">
                          <div class="form-group">
                              <span class="details">INSC. ESTADUAL:</span>
                              <asp:TextBox ID="txtInscEstadual" runat="server" ForeColor="Blue" CssClass="form-control" placeholder="" MaxLength="15"></asp:TextBox>
                          </div>
                      </div>
                      <div class="col-md-2">
                          <div class="form-group">
                              <span class="details">CONTATO:</span>
                              <asp:TextBox ID="txtConCli" runat="server" ForeColor="Blue" CssClass="form-control" placeholder="" MaxLength="30"></asp:TextBox>
                          </div>
                      </div>
                      <div class="col-md-2">
                          <div class="form-group">
                              <span class="details">FONE FIXO:</span>
                              <asp:TextBox ID="txtTc1Cli" runat="server" ForeColor="Blue" CssClass="form-control" data-inputmask='"mask": "(99) 9999-9999"' data-mask MaxLength="15"></asp:TextBox>
                          </div>
                      </div>
                      <div class="col-md-1">
                          <div class="form-group">
                              <span class="details">RAMAL:</span>
                              <asp:TextBox ID="txtRamal" runat="server" ForeColor="Blue" CssClass="form-control" placeholder="9999" MaxLength="4"></asp:TextBox>
                          </div>
                      </div>
                      <div class="col-md-2">
                          <div class="form-group">
                              <span class="details">CELULAR:</span>
                              <asp:TextBox ID="txtTc2Cli" runat="server" ForeColor="Blue" CssClass="form-control" data-mask="(00) 0 0000-0000" MaxLength="16"></asp:TextBox>
                          </div>
                      </div>
                  </div>
                  <div class="row g-3">
                      <div class="col-md-3">
                          <div class="form-group">
                              <span class="details">PROGRAMADORES:</span>
                              <asp:TextBox ID="txtProgramador" runat="server" ForeColor="Blue" CssClass="form-control" placeholder="" MaxLength="35"></asp:TextBox>
                          </div>
                      </div>
                      <div class="col-md-2">
                          <div class="form-group">
                              <span class="details">TELEFONE/RAMAL:</span>
                              <asp:TextBox ID="txtContato" runat="server" ForeColor="Blue" CssClass="form-control" placeholder="" MaxLength="25"></asp:TextBox>
                          </div>
                      </div>
                      <div class="col-md-7">
                          <div class="form-group">
                              <span class="details">E-MAIL(S):</span>
                              <asp:TextBox ID="txtEmail" runat="server" ForeColor="Blue" CssClass="form-control" placeholder="" MaxLength="200"></asp:TextBox>
                          </div>
                      </div>
                  </div>
                  <div class="row g-3">
                      <div class="col-md-1">
                          <div class="form-group">
                              <span class="details">CEP:</span>
                              <asp:TextBox ID="txtCepCli" runat="server" ForeColor="Blue" CssClass="form-control" Width="130px" placeholder="99999-999" MaxLength="9"></asp:TextBox>
                          </div>
                      </div>
                      <div class="col-md-1">
                          <br />
                          <asp:Button ID="btnCep" runat="server" Text="Pesquisar" CssClass="btn btn-outline-warning" OnClick="btnCep_Click" UseSubmitBehavior="false" />
                      </div>
                      <div class="col-md-7">
                          <div class="form-group">
                              <span class="details">ENDEREÇO:</span>
                              <asp:TextBox ID="txtEndCli" runat="server" ForeColor="Blue" CssClass="form-control" MaxLength="60"></asp:TextBox>
                          </div>
                      </div>
                      <div class="col-md-1">
                          <div class="form-group">
                              <span class="details">Nº:</span>
                              <asp:TextBox ID="txtNumero" Style="text-align: center" runat="server" ForeColor="Blue" CssClass="form-control"></asp:TextBox>
                          </div>
                      </div>
                      <div class="col-md-2">
                          <div class="form-group">
                              <span class="details">COMPLEMENTO:</span>
                              <asp:TextBox ID="txtComplemento" Style="text-align: center" runat="server" ForeColor="Blue" CssClass="form-control" placeholder="" MaxLength="15"> </asp:TextBox>
                          </div>
                      </div>
                  </div>
                  <div class="row g-3">
                      <div class="col-md-2">
                          <div class="form-group">
                              <span class="details">BAIRRO:</span>
                              <asp:TextBox ID="txtBaiCli" runat="server" ForeColor="Blue" CssClass="form-control" placeholder="" MaxLength="60"></asp:TextBox>
                          </div>
                      </div>
                      <div class="col-md-3">
                          <div class="form-group">
                              <span class="details">MUNICIPIO:</span>
                              <asp:TextBox ID="txtCidCli" runat="server" ForeColor="Blue" CssClass="form-control" placeholder="" MaxLength="60"></asp:TextBox>
                          </div>
                      </div>
                      <div class="col-md-1">
                          <div class="form-group">
                              <span class="details">UF:</span>
                              <asp:TextBox ID="txtEstCli" Style="text-align: center" runat="server" ForeColor="Blue" CssClass="form-control" placeholder="" MaxLength="2"></asp:TextBox>
                          </div>
                      </div>
                      <div class="col-md-2">
                          <div class="form-group">
                              <span class="details">LATITUDE:</span>
                              <asp:TextBox ID="latitude" Style="text-align: center" runat="server" ForeColor="Blue" CssClass="form-control" placeholder="" MaxLength="40"></asp:TextBox>
                          </div>
                      </div>
                      <div class="col-md-2">
                          <div class="form-group">
                              <span class="details">LONGITUDE:</span>
                              <asp:TextBox ID="longitude" Style="text-align: center" runat="server" ForeColor="Blue" CssClass="form-control" placeholder="" MaxLength="40"></asp:TextBox>
                          </div>
                      </div>
                      <div class="col-md-1">
                          <div class="form-group">
                              <span class="details">RAIO:</span>
                              <asp:TextBox ID="txtRaio" value="100" Style="text-align: center" runat="server" ForeColor="Blue" CssClass="form-control" placeholder="" min="1" max="2000"></asp:TextBox>
                          </div>
                      </div>
                      <div class="col-md-1">
                          <br />
                          <button type="button" class="btn btn-outline-warning">Pesquisar</button>
                      </div>
                  </div>
                  <div class="row g-3">
                      <div class="col-md-2">
                          <div class="form-group">
                              <span class="details">CADASTRADO EM:</span>
                              <asp:Label ID="lblDtCadastro" runat="server" ForeColor="Blue" CssClass="form-control" placeholder="" maxlength="20"></asp:Label>
                          </div>
                      </div>
                      <div class="col-md-10">
                          <div class="form-group">
                              <span class="details">POR:</span>
                              <asp:TextBox ID="txtUsuCadastro" runat="server" ForeColor="Blue" CssClass="form-control" placeholder="" MaxLength="60"></asp:TextBox>
                          </div>
                      </div>
                  </div>
                  <div class="row g-3">
                      <div class="col-md-1">
                          <br />
                          <button type="button" class="btn btn-outline-info  btn-lg">Mapa </button>
                      </div>
                      <div class="col-md-1">
                          <br />
                          <asp:Button ID="btnSalvar1" CssClass="btn btn-outline-success  btn-lg" runat="server" OnClick="btnSalvar_Click" Text="Salvar" />
                      </div>
                      <div class="col-md-1">
                          <br />
                          <a href="ConsultaClientes.aspx" class="btn btn-outline-danger btn-lg">Cancelar               
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
 

</asp:Content>
