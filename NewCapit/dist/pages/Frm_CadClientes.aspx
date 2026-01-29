<%@ Page Title="" Language="C#" MasterPageFile="Main.Master" AutoEventWireup="true" CodeBehind="Frm_CadClientes.aspx.cs" Inherits="NewCapit.dist.pages.Frm_CadClientes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery.mask/1.14.16/jquery.mask.min.js"></script>
    <script>
        document.addEventListener("DOMContentLoaded", function () {
            function aplicarMascara(input, mascara) {
                input.addEventListener("input", function () {
                    let valor = input.value.replace(/\D/g, ""); // Remove tudo que não for número
                    let resultado = "";
                    let posicao = 0;

                    for (let i = 0; i < mascara.length; i++) {
                        if (mascara[i] === "0") {
                            if (valor[posicao]) {
                                resultado += valor[posicao];
                                posicao++;
                            } else {
                                break;
                            }
                        } else {
                            resultado += mascara[i];
                        }
                    }

                    input.value = resultado;
                });
            }

            // Pegando os elementos no ASP.NET
            let txtCNPJ = document.getElementById("<%= txtCnpj.ClientID %>");
            let txtCep = document.getElementById("<%= txtCepCli.ClientID %>");
            let txtTelefone = document.getElementById("<%= txtTc1Cli.ClientID %>");
            let txtCelular = document.getElementById("<%= txtTc2Cli.ClientID %>");

            if (txtCNPJ) aplicarMascara(txtCNPJ, "00.000.000/0000-00");
            if (txtCep) aplicarMascara(txtCep, "00000-000");
            if (txtTelefone) aplicarMascara(txtTelefone, "(00) 0000-0000");
            if (txtCelular) aplicarMascara(txtCelular, "(00) 0 0000-0000");
    });
    </script>
    <script>
        document.addEventListener("DOMContentLoaded", function () {
            function aplicarMascaraLatitudeLongitude(input) {
                input.addEventListener("input", function () {
                    let valor = input.value;

                    // Garante que o "-" sempre esteja no início
                    if (!valor.startsWith("-")) {
                        valor = "-" + valor.replace(/[^0-9.]/g, ""); // Remove caracteres inválidos e adiciona "-"
                    } else {
                        valor = "-" + valor.substring(1).replace(/[^0-9.]/g, ""); // Mantém o "-" e filtra o resto
                    }

                    // Remove pontos extras, mantendo apenas o primeiro
                    let partes = valor.split(".");
                    if (partes.length > 2) {
                        valor = partes[0] + "." + partes.slice(1).join(""); // Remove pontos extras
                    }

                    // Garante que tenha no máximo 2 dígitos antes do ponto
                    let match = valor.match(/^-?\d{0,2}(\.\d{0,8})?/);
                    if (match) {
                        valor = match[0];
                    }

                    input.value = valor;
                });

                // Adiciona o "-" automaticamente se o campo estiver vazio ao perder o foco
                input.addEventListener("blur", function () {
                    if (input.value === "-") {
                        input.value = "";
                    }
                });
            }

            // Pegando os elementos no ASP.NET
            let latitude = document.getElementById("<%= latitude.ClientID %>");
        let longitude = document.getElementById("<%= longitude.ClientID %>");

        if (latitude) aplicarMascaraLatitudeLongitude(latitude);
        if (longitude) aplicarMascaraLatitudeLongitude(longitude);
    });
    </script>


      <div class="content-wrapper">
      <section class="content">
          <div class="container-fluid">
              <br />
              <div class="card card-success">
                  <div class="card-header">
                      <h3 class="card-title"><i class="fas fa-warehouse"></i> &nbsp;CLIENTES - NOVO CADASTRO</h3>
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
                              <asp:DropDownList ID="cboTipo" runat="server" CssClass="form-control" >
                                  <asp:ListItem Value="" Text="Selecione..."></asp:ListItem>
                                  <asp:ListItem Value="CLIENTE" Text="CLIENTE"></asp:ListItem>
                                  <asp:ListItem Value="EMBARCADOR" Text="EMBARCADOR"></asp:ListItem>
                                  <asp:ListItem Value="FORNECEDOR" Text="FORNECEDOR"></asp:ListItem>
                                  <asp:ListItem Value="TRANSPORTADOR" Text="TRANSPORTADOR"></asp:ListItem>
                                  <asp:ListItem Value="OPERADOR LOGÍSTICO" Text="OPERADOR LOGÍSTICO"></asp:ListItem>
                                  <asp:ListItem Value="OUTROS" Text="OUTROS"></asp:ListItem>
                              </asp:DropDownList><br />
                              <asp:RequiredFieldValidator  ID="rfvcboTipo" runat="server" ControlToValidate="cboTipo" InitialValue=""  ErrorMessage="* Obrigatório" ValidationGroup="Cadastro"  Font-Size="9px" ForeColor="Red" Display="Dynamic" />
                          </div>
                      </div>
                      <div class="col-md-3">
                          <div class="form_group">
                              <span class="details">UNIDADE:</span>
                              <asp:TextBox ID="txtUnidade" runat="server" CssClass="form-control" placeholder="" MaxLength="45"></asp:TextBox>
                              <asp:RequiredFieldValidator runat="server" id="rftxtUnidade" ControlToValidate="txtUnidade" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                          </div>
                      </div>
                      <div class="col-md-2">
                          <span class="">REGIÃO DO PAÍS:</span>
                          <asp:DropDownList ID="cboRegiao" runat="server" CssClass="form-control" >
                              <asp:ListItem Value="" Text="Selecione..."></asp:ListItem>
                              <asp:ListItem Value="NORTE" Text="NORTE"></asp:ListItem>
                              <asp:ListItem Value="SUL" Text="SUL"></asp:ListItem>
                              <asp:ListItem Value="SUDESTE" Text="SUDESTE"></asp:ListItem>
                              <asp:ListItem Value="CENTRO-OESTE" Text="CENTRO-OESTE"></asp:ListItem>
                              <asp:ListItem Value="NORDESTE" Text="NORDESTE"></asp:ListItem>
                          </asp:DropDownList><br />
                          <asp:RequiredFieldValidator  ID="rfvcboRegiao" runat="server" ControlToValidate="cboRegiao" InitialValue=""  ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px"  ForeColor="Red" Display="Dynamic" />
                      </div>
                      
                      <div class="col-md-1">
                          <div class="form-group">
                              <span class="details">SAPIENS:</span>
                              <asp:TextBox ID="txtCodSapiens" runat="server" CssClass="form-control" placeholder="" MaxLength="10"></asp:TextBox>

                          </div>
                      </div>

                      <div class="col-md-1">
                          <div class="form-group">
                              <span class="details">CÓD.VW:</span>
                              <asp:TextBox ID="txtCodVw" runat="server" CssClass="form-control" placeholder="" MaxLength="10"></asp:TextBox>
                              <asp:RequiredFieldValidator runat="server" id="rfvtxtCodVw" ControlToValidate="txtCodVw" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                          </div>
                      </div>
                      <div class="col-md-1">
                          <div class="form-group">
                              <span class="">STATUS:</span>
                              <asp:DropDownList ID="status" runat="server" CssClass="form-control">
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
                              <asp:TextBox ID="txtCnpj" runat="server" class="form-control"></asp:TextBox>
                              <asp:RequiredFieldValidator runat="server" id="rfvtxtCnpj" ControlToValidate="txtCnpj" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                          </div>
                      </div>
                      <div class="col-md-1">
                          <br />
                          <asp:Button ID="btnCnpj" runat="server" Text="Pesquisar" CssClass="btn btn-outline-warning" OnClick="btnCnpj_Click" />
                      </div>
                      <div class="col-md-6">
                          <div class="form-group">
                              <span class="details">RAZÃO SOCIAL:</span>
                              <asp:TextBox ID="txtRazCli" runat="server" CssClass="form-control" ></asp:TextBox>
                              <asp:RequiredFieldValidator runat="server" id="rvftxtRazCli" ControlToValidate="txtRazCli" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                          </div>
                      </div>
                      <div class="col-md-1">
                          <div class="form-group">
                              <span class="details">TIPO:</span>
                              <asp:TextBox ID="txtTipo" runat="server" CssClass="form-control" ></asp:TextBox>
                              <asp:RequiredFieldValidator runat="server" id="rfvtxtTipo" ControlToValidate="txtTipo" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                          </div>
                      </div>
                      <div class="col-md-1">
                          <div class="form-group">
                              <span class="details">ABERTURA:</span>
                              <asp:TextBox ID="txtAbertura" runat="server" CssClass="form-control" ></asp:TextBox>
                              <asp:RequiredFieldValidator runat="server" id="rfvtxtAbertura" ControlToValidate="txtAbertura" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                          </div>
                      </div>
                      <div class="col-md-1">
                          <div class="form-group">
                              <span class="details">SITUAÇÃO:</span>
                              <asp:TextBox ID="txtSituacao" runat="server" CssClass="form-control" ></asp:TextBox>
                              <asp:RequiredFieldValidator runat="server" id="rvftxtSituacao" ControlToValidate="txtSituacao" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                          </div>
                      </div>

                  </div>
                  <div class="row g-3">
                      <div class="col-md-3">
                          <div class="form-group">
                              <span class="details">NOME FANTASIA:</span>
                              <asp:TextBox ID="txtNomCli" runat="server" CssClass="form-control" placeholder="" MaxLength="50" ></asp:TextBox>
                              <asp:RequiredFieldValidator runat="server" id="rvftxtNomCli" ControlToValidate="txtNomCli" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                          </div>
                      </div>
                      <div class="col-md-2">
                          <div class="form-group">
                              <span class="details">INSC. ESTADUAL:</span>
                              <asp:TextBox ID="txtInscEstadual" runat="server" CssClass="form-control" placeholder="" MaxLength="15"></asp:TextBox>
                              <asp:RequiredFieldValidator runat="server" id="rfvtxtInscEstadual" ControlToValidate="txtInscEstadual" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                          </div>
                      </div>
                      <div class="col-md-2">
                          <div class="form-group">
                              <span class="details">CONTATO:</span>
                              <asp:TextBox ID="txtConCli" runat="server" CssClass="form-control" placeholder="" MaxLength="30"></asp:TextBox>
                          </div>
                      </div>
                      <div class="col-md-2">
                          <div class="form-group">
                              <span class="details">FONE FIXO:</span>
                              <asp:TextBox ID="txtTc1Cli" runat="server" CssClass="form-control" MaxLength="15"></asp:TextBox>
                          </div>
                      </div>
                      <div class="col-md-1">
                          <div class="form-group">
                              <span class="details">RAMAL:</span>
                              <asp:TextBox ID="txtRamal" runat="server" CssClass="form-control" placeholder="9999" MaxLength="4"></asp:TextBox>
                          </div>
                      </div>
                      <div class="col-md-2">
                          <div class="form-group">
                              <span class="details">CELULAR:</span>
                              <asp:TextBox ID="txtTc2Cli" runat="server" CssClass="form-control" MaxLength="16"></asp:TextBox>
                          </div>
                      </div>
                  </div>
                  <div class="row g-3">
                      <div class="col-md-3">
                          <div class="form-group">
                              <span class="details">PROGRAMADORES:</span>
                              <asp:TextBox ID="txtProgramador" runat="server" CssClass="form-control" placeholder="" MaxLength="35"></asp:TextBox>
                          </div>
                      </div>
                      <div class="col-md-2">
                          <div class="form-group">
                              <span class="details">TELEFONE/RAMAL:</span>
                              <asp:TextBox ID="txtContato" runat="server" CssClass="form-control" placeholder="" MaxLength="25"></asp:TextBox>
                          </div>
                      </div>
                      <div class="col-md-7">
                          <div class="form-group">
                              <span class="details">E-MAIL(S):</span>
                              <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="" MaxLength="200"></asp:TextBox>
                          </div>
                      </div>
                  </div>
                  <div class="row g-3">
                      <div class="col-md-1">
                          <div class="form-group">
                              <span class="details">CEP:</span>
                              <asp:TextBox ID="txtCepCli" runat="server" CssClass="form-control" Width="130px" placeholder="99999-999" MaxLength="9"></asp:TextBox>
                              <asp:RequiredFieldValidator runat="server" id="rfvtxtCepCli" ControlToValidate="txtCepCli" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                          </div>
                      </div>
                      <div class="col-md-1">
                          <br />
                          <asp:Button ID="btnCep" runat="server" Text="Pesquisar" CssClass="btn btn-outline-warning" OnClick="btnCep_Click" UseSubmitBehavior="false" />
                      </div>
                      <div class="col-md-7">
                          <div class="form-group">
                              <span class="details">ENDEREÇO:</span>
                              <asp:TextBox ID="txtEndCli" runat="server" CssClass="form-control" MaxLength="60"></asp:TextBox>
                              <asp:RequiredFieldValidator runat="server" id="rfvtxtEndCli" ControlToValidate="txtEndCli" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                          </div>
                      </div>
                      <div class="col-md-1">
                          <div class="form-group">
                              <span class="details">Nº:</span>
                              <asp:TextBox ID="txtNumero" Style="text-align: center" runat="server" CssClass="form-control"></asp:TextBox>
                              <asp:RequiredFieldValidator runat="server" id="rfvtxtNumero" ControlToValidate="txtNumero" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                          </div>
                      </div>
                      <div class="col-md-2">
                          <div class="form-group">
                              <span class="details">COMPLEMENTO:</span>
                              <asp:TextBox ID="txtComplemento" Style="text-align: center" runat="server" CssClass="form-control" placeholder="" MaxLength="15"> </asp:TextBox>
                          </div>
                      </div>
                  </div>
                  <div class="row g-3">
                      <div class="col-md-2">
                          <div class="form-group">
                              <span class="details">BAIRRO:</span>
                              <asp:TextBox ID="txtBaiCli" runat="server" CssClass="form-control" placeholder="" MaxLength="60"></asp:TextBox>
                              <asp:RequiredFieldValidator runat="server" id="rfvtxtBaiCli" ControlToValidate="txtBaiCli" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                          </div>
                      </div>
                      <div class="col-md-3">
                          <div class="form-group">
                              <span class="details">MUNICIPIO:</span>
                              <asp:TextBox ID="txtCidCli" runat="server" CssClass="form-control" placeholder="" MaxLength="60"></asp:TextBox>
                              <asp:RequiredFieldValidator runat="server" id="rfvtxtCidCli" ControlToValidate="txtCidCli" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                          </div>
                      </div>
                      <div class="col-md-1">
                          <div class="form-group">
                              <span class="details">UF:</span>
                              <asp:TextBox ID="txtEstCli" Style="text-align: center" runat="server" CssClass="form-control" placeholder="" MaxLength="2"></asp:TextBox>
                              <asp:RequiredFieldValidator runat="server" id="rfvtxtEstCli" ControlToValidate="txtEstCli" ValidationGroup="Cadastro" ErrorMessage="* Obrigatório" Font-Size="9px" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                          </div>
                      </div>
                      <div class="col-md-2">
                          <div class="form-group">
                              <span class="details">LATITUDE:</span>
                              <asp:TextBox ID="latitude" Style="text-align: center" runat="server" CssClass="form-control" placeholder="" MaxLength="40"></asp:TextBox>
                          </div>
                      </div>
                      <div class="col-md-2">
                          <div class="form-group">
                              <span class="details">LONGITUDE:</span>
                              <asp:TextBox ID="longitude" Style="text-align: center" runat="server" CssClass="form-control" placeholder="" MaxLength="40"></asp:TextBox>
                          </div>
                      </div>
                      <div class="col-md-1">
                          <div class="form-group">
                              <span class="details">RAIO:</span>
                              <asp:TextBox ID="txtRaio" value="100" Style="text-align: center" runat="server" CssClass="form-control" placeholder="" min="1" max="2000"></asp:TextBox>
                          </div>
                      </div>
                      <div class="col-md-1">
                          <br />
                          <asp:Button ID="btnPesquisar" CssClass="btn btn-outline-warning" runat="server" Text="Pesquisar" OnClick="btnPesquisar_Click" />
                          
                      </div>
                  </div>
                  <div class="row g-3">
                      <div class="col-md-2">
                          <div class="form-group">
                              <span class="details">CADASTRADO EM:</span>
                              <asp:Label ID="lblDtCadastro" runat="server" CssClass="form-control" placeholder="" maxlength="20"></asp:Label>
                          </div>
                      </div>
                      <div class="col-md-10">
                          <div class="form-group">
                              <span class="details">POR:</span>
                              <asp:TextBox ID="txtUsuCadastro" runat="server" CssClass="form-control" placeholder="" MaxLength="60"></asp:TextBox>
                          </div>
                      </div>
                  </div>
                  <div class="row g-3">
                      <%--<div class="col-md-1">
                          <br />
                          <button type="button" class="btn btn-outline-info  btn-lg">Mapa </button>
                      </div>--%>
                      <div class="col-md-2">
                          <br />
                          <asp:Button ID="btnSalvar1" CssClass="btn btn-outline-success  btn-lg w-100" runat="server" ValidationGroup="Cadastro" OnClick="btnSalvar_Click" Text="Cadastrar" />
                      </div>
                      <div class="col-md-2">
                          <br />
                          <a href="/dist/pages/ConsultaClientes.aspx" class="btn btn-outline-danger btn-lg w-100">Fechar               
                          </a>
                      </div>
                  </div>
              </div>
          </div>
      </section>
   </div>
  
</asp:Content>
