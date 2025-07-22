<%@ Page Title="" Language="C#" MasterPageFile="Main.Master" AutoEventWireup="true" CodeBehind="Frm_CadMotoristas.aspx.cs" Inherits="NewCapit.dist.pages.Frm_CadMotoristas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
            let txtDtNasc = document.getElementById("<%= txtDtNasc.ClientID %>");
          let txtDtEmissao = document.getElementById("<%= txtDtEmissao.ClientID %>");
          let txtValCNH = document.getElementById("<%= txtValCNH.ClientID %>");
          let txtValLibRisco = document.getElementById("<%= txtValLibRisco.ClientID %>");
          let txtVAlExameTox = document.getElementById("<%= txtVAlExameTox.ClientID %>");
          let txtVAlMoop = document.getElementById("<%= txtVAlMoop.ClientID %>");
          let txtCPF = document.getElementById("<%= txtCPF.ClientID %>");
          let txtCartao = document.getElementById("<%= txtCartao.ClientID %>");
          let txtValCartao = document.getElementById("<%= txtValCartao.ClientID %>");
          let txtFixo = document.getElementById("<%= txtFixo.ClientID %>");
          let txtCelular = document.getElementById("<%= txtCelular.ClientID %>");



          if (txtVAlExameTox) aplicarMascara(txtVAlExameTox, "00/00/0000");
          if (txtDtNasc) aplicarMascara(txtDtNasc, "00/00/0000");
          if (txtDtEmissao) aplicarMascara(txtDtEmissao, "00/00/0000");
          if (txtValCNH) aplicarMascara(txtValCNH, "00/00/0000");
          if (txtValLibRisco) aplicarMascara(txtValLibRisco, "00/00/0000");
          if (txtVAlMoop) aplicarMascara(txtVAlMoop, "00/00/0000");
          if (txtCPF) aplicarMascara(txtCPF, "000.000.000-00");
          if (txtCartao) aplicarMascara(txtCartao, "0000 0000 0000 0000");
          if (txtValCartao) aplicarMascara(txtValCartao, "00/0000");
          if (txtFixo) aplicarMascara(txtFixo, "(00) 0000-0000");
          if (txtFixo) aplicarMascara(txtCelular, "(00) 0 0000-0000");
      });
    </script>
    <script type="text/javascript">
        function previewImage(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();

                reader.onload = function (e) {
                    var img = document.getElementById('preview');
                    img.src = e.target.result;

                    // Salva o base64 no campo hidden
                    document.getElementById('<%= hiddenImage.ClientID %>').value = e.target.result;
                };

                reader.readAsDataURL(input.files[0]);
            }
        }
    </script>
    <script>
        document.addEventListener("DOMContentLoaded", function () {
            const fileInput = document.getElementById('<%= FileUpload1.ClientID %>');
        const preview = document.getElementById('preview');
        const hidden = document.getElementById('<%= hiddenImage.ClientID %>');
        const maxSize = 1 * 1024 * 1024; // 1MB

        // Intercepta a mudança no input
        fileInput.addEventListener("change", function (e) {
            const file = fileInput.files[0];

            if (!file) return;

            if (file.size > maxSize) {
                alert("Imagem muito grande. Tamanho máximo permitido: 1MB.");
                fileInput.value = "";
                hidden.value = "";
                preview.src = '<%= ResolveUrl("/fotos/usuario.jpg") %>';
              e.preventDefault(); // <--- bloqueia envio (só por segurança extra)
              return false;
          }

          const reader = new FileReader();
          reader.onload = function (evt) {
              const base64 = evt.target.result;
              hidden.value = base64;
              preview.src = base64;
          };
          reader.readAsDataURL(file);
      });
    });
    </script>
    <div class="content-wrapper">
        <section class="content">
        <div class="container-fluid">
            <br />
            <div class="card card-info">
                <div class="card-header">
                    <h3 class="card-title"><i class="fas fa-address-card"></i>&nbsp;MOTORISTAS - NOVO CADASTRO</h3>
                </div>
            </div>       
            <!-- Linha 1 do formulario -->
            <div class="row g-3">
                <div class="col-md-1">
                    <div class="form-group">
                        <span class="details">CÓDIGO:</span>
                        <asp:TextBox ID="txtCodMot" Style="text-align: center" runat="server" CssClass="form-control" placeholder="" MaxLength="6" AutoPostBack="True" OnTextChanged="txtCodMot_TextChanged"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvtxtCodMot" runat="server" ControlToValidate="txtCodMot" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
                    </div>
                </div>
                <div class="col-md-2">
                    <div class="form-group">
                        <span class="">TIPO MOTORISTA:</span>
                        <asp:DropDownList ID="ddlTipoMot" runat="server" CssClass="form-control">
                            <asp:ListItem Value="" Text="Selecione..."></asp:ListItem>
                            <asp:ListItem Value="AGREGADO" Text="AGREGADO"></asp:ListItem>
                            <asp:ListItem Value="AGREGADO FUNCIONÁRIO" Text="AGREGADO FUNCIONÁRIO"></asp:ListItem>
                            <asp:ListItem Value="FUNCIONÁRIO" Text="FUNCIONÁRIO"></asp:ListItem>
                            <asp:ListItem Value="TERCEIRO" Text="TERCEIRO"></asp:ListItem>
                            <asp:ListItem Value="FUNCIONÁRIO TERCEIRO" Text="FUNCIONÁRIO TERCEIRO"></asp:ListItem>

                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfddlTipoMot" runat="server" ControlToValidate="ddlTipoMot" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
                    </div>
                </div>
                <div class="col-md-2">
                    <div class="form-group">
                        <span class="">CARGO:</span>
                        <asp:DropDownList ID="ddlCargo" name="descricaoCargo" runat="server" CssClass="form-control"></asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlCargo" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
                    </div>
                </div>
                <div class="col-md-2">
                    <div class="form-group">
                        <span class="">FUNÇÃO:</span>
                        <asp:DropDownList ID="ddlFuncao" runat="server" CssClass="form-control">
                            <asp:ListItem Value="" Text="Selecione..."></asp:ListItem>
                            <asp:ListItem Value="CARREGAMENTO" Text="CARREGAMENTO"></asp:ListItem>
                            <asp:ListItem Value="ENTREGA" Text="ENTREGA"></asp:ListItem>
                            <asp:ListItem Value="SERV. INTERNO" Text="SERV. INTERNO"></asp:ListItem>
                            <asp:ListItem Value="TERM. IPIRANGA" Text="TERM. IPIRANGA"></asp:ListItem>
                            <asp:ListItem Value="OUTRO" Text="OUTRO"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvddlFuncao" runat="server" ControlToValidate="ddlFuncao" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
                    </div>
                </div>
                <div class="col-md-2">
                    <div class="form_group">
                        <span class="details">FILIAL:</span>
                        <asp:DropDownList ID="cbFiliais" name="nomeFiliais" runat="server" CssClass="form-control select2"></asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="cbFiliais" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
                    </div>
                </div>
                <div class="col-md-2"></div>
                <div class="col-md-1">
                    <!-- FileUpload oculto -->
                    <asp:FileUpload ID="FileUpload1" runat="server" Style="display: none;" onchange="previewImage(this)" />

                    <!-- Imagem clicável -->
                    <asp:FileUpload ID="FileUpload2" runat="server" Style="display: none;" />
                    <asp:HiddenField ID="hiddenImage" runat="server" />
                    <img id="preview"
                        src='<%= ResolveUrl("/fotos/usuario.jpg") %>'
                        alt="Selecione a foto"
                        onclick="document.getElementById('<%= FileUpload1.ClientID %>').click();"
                        style="cursor: pointer; width: 80px; height: 80px;" />                    
                </div>
            </div>
            <!-- Linha 2 do formulario -->
            <div class="row g-3">
                <div class="col-md-4">
                    <div class="form-group">
                        <span class="details">NOME COMPLETO:</span>
                        <asp:TextBox ID="txtNomMot" runat="server" class="form-control" placeholder="" MaxLength="50"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtNomMot" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
                    </div>
                </div>
                <div class="col-md-1">
                    <div class="form-group">
                        <span class="details">DATA NASC.:</span>
                        <div class="input-group">
                            <asp:TextBox ID="txtDtNasc" runat="server" class="form-control"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtDtNasc" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
                        </div>
                    </div>
                </div>
                <div class="col-md-2">
                    <div class="form-group">
                        <div class="form-group">
                            <span class="details">REGIÃO DO PAIS:</span>
                            <asp:DropDownList ID="ddlRegioes" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlRegioes_SelectedIndexChanged"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="ddlRegioes" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
                        </div>
                    </div>
                </div>
                <div class="col-md-1">
                    <div class="form_group">
                        <span class="details">UF NASC.:</span>
                        <asp:DropDownList ID="ddlEstNasc" runat="server" class="form-control select2" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlEstNasc_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="ddlEstNasc" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="form_group">
                        <span class="details">MUNICIPIO DE NASCIMENTO:</span>
                        <asp:DropDownList ID="ddlMunicipioNasc" runat="server" CssClass="form-control select2"></asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="ddlMunicipioNasc" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
                    </div>
                </div>
                <div class="col-md-1">
                    <div class="form_group">
                        <span class="details">ADM./CAD.:</span>
                        <asp:TextBox ID="txtDtCad" runat="server" Style="text-align: left" CssClass="form-control" placeholder="" MaxLength="10"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtDtCad" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
                    </div>
                </div>

            </div>
            <div class="row g-3">
                <div class="col-md-4">
                    <div class="form-group">
                        <span class="details">NOME DA MÃE:</span>
                        <asp:TextBox ID="txtNomeMae" runat="server" class="form-control" placeholder="" MaxLength="50"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="txtNomeMae" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <span class="details">NOME DO PAI:</span>
                        <asp:TextBox ID="txtNomePai" runat="server" class="form-control" placeholder="" MaxLength="50"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="txtNomePai" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
                    </div>
                </div>

            </div>
            <!-- Linha 3 do formulario -->
            <div class="row g-3">
                <div class="col-md-2">
                    <div class="form-group">
                        <span class="details">CPF:</span>
                        <div class="input-group">
                            <asp:TextBox ID="txtCPF" runat="server" class="form-control"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="txtCPF" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
                        </div>
                    </div>
                </div>
                <div class="col-md-1">
                    <div class="form-group">
                        <span class="details">RG:</span>
                        <asp:TextBox ID="txtRG" runat="server" Style="text-align: center" CssClass="form-control" value=""></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="txtRG" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
                    </div>
                </div>
                <div class="col-md-1">
                    <div class="form-group">
                        <span class="details">EMISSOR:</span>
                        <asp:TextBox ID="txtEmissor" runat="server" Style="text-align: center" CssClass="form-control" value=""></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="txtEmissor" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
                    </div>
                </div>
                <div class="col-md-1">
                    <div class="form-group">
                        <span class="details">EMISSÃO:</span>
                        <div class="input-group">
                            <asp:TextBox ID="txtDtEmissao" runat="server" class="form-control"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="txtDtEmissao" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
                        </div>
                    </div>
                </div>
                <div class="col-md-2">
                    <div class="form-group">
                        <span class="details">CARTÃO PAMCARD:</span>
                        <div class="input-group">
                            <asp:TextBox ID="txtCartao" runat="server" class="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-md-1">
                    <div class="form-group">
                        <span class="details">MÊS/ANO:</span>
                        <div class="input-group">
                            <asp:TextBox ID="txtValCartao" runat="server" class="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-md-2">
                    <div class="form-group">
                        <span class="details">Nº INSS:</span>
                        <asp:TextBox ID="txtINSS" runat="server" Style="text-align: center" CssClass="form-control" value=""></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-2">
                    <div class="form-group">
                        <span class="details">Nº PIS:</span>
                        <div class="input-group">
                            <asp:TextBox ID="txtPIS" runat="server" class="form-control"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ControlToValidate="txtPIS" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
                        </div>
                    </div>
                </div>

            </div>
            <!-- Linha 4 do formulário -->
            <div class="row g-3">
                <div class="col-md-2">
                    <div class="form-group">
                        <span class="details">Nº CNH:</span>
                        <asp:TextBox ID="txtRegCNH" runat="server" Style="text-align: center" CssClass="form-control" value=""></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ControlToValidate="txtRegCNH" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
                    </div>
                </div>
                <div class="col-md-2">
                    <div class="form-group">
                        <span class="details">Nº FORM DA CNH:</span>
                        <asp:TextBox ID="txtFormCNH" runat="server" Style="text-align: center" CssClass="form-control" value=""></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ControlToValidate="txtFormCNH" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
                    </div>
                </div>
                <div class="col-md-2">
                    <div class="form-group">
                        <span class="details">CÓDIGO DE SEGURANÇA:</span>
                        <asp:TextBox ID="txtCodSeguranca" runat="server" Style="text-align: center" CssClass="form-control" value=""></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" ControlToValidate="txtCodSeguranca" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
                    </div>
                </div>
                <div class="col-md-1">
                    <div class="form-group">
                        <span class="">CATEGORIA:</span>
                        <asp:DropDownList ID="ddlCat" runat="server" CssClass="form-control">
                            <asp:ListItem Value="" Text="Selecione..."></asp:ListItem>
                            <asp:ListItem Value="AE" Text="AE"></asp:ListItem>
                            <asp:ListItem Value="C" Text="C"></asp:ListItem>
                            <asp:ListItem Value="D" Text="D"></asp:ListItem>
                            <asp:ListItem Value="E" Text="E"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator20" runat="server" ControlToValidate="ddlCat" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
                    </div>
                </div>
                <div class="col-md-1">
                    <div class="form-group">
                        <span class="details">VALIDADE CNH:</span>
                        <asp:TextBox ID="txtValCNH" runat="server" class="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator21" runat="server" ControlToValidate="txtValCNH" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
                    </div>
                </div>
                <div class="col-md-1">
                    <div class="form-group">
                        <span class="details">UF CNH:</span>
                        <asp:DropDownList ID="ddlCNH" name="ufCNH" runat="server" CssClass="form-control select2" OnSelectedIndexChanged="ddlCNH_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator22" runat="server" ControlToValidate="ddlCNH" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="form-group">
                        <span class="details">MUNICIPIO DA CNH:</span>
                        <asp:DropDownList ID="ddlMunicCnh" class="form-control select2" runat="server"></asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator23" runat="server" ControlToValidate="ddlMunicCnh" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
                    </div>
                </div>


            </div>
            <!-- Linha 5 do formulário -->
            <div class="row g-3">
                <div class="col-md-2">
                    <div class="form-group">
                        <span class="">EST. CIVIL:</span>
                        <asp:DropDownList ID="ddlEstCivil" runat="server" CssClass="form-control">
                            <asp:ListItem Value="" Text="Selecione..."></asp:ListItem>
                            <asp:ListItem Value="SOLTEIRO(A)" Text="SOLTEIRO(A)"></asp:ListItem>
                            <asp:ListItem Value="CASADO(A)" Text="CASADO(A)"></asp:ListItem>
                            <asp:ListItem Value="UNIÃO ESTÁVEL" Text="UNIÃO ESTÁVEL"></asp:ListItem>
                            <asp:ListItem Value="SEPARADO(A)" Text="SEPARADO(A)"></asp:ListItem>
                            <asp:ListItem Value="DIVORCIADO(A)" Text="DIVORCIADO(A)"></asp:ListItem>
                            <asp:ListItem Value="VIUVO(A)" Text="VIUVO(A)"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator24" runat="server" ControlToValidate="ddlEstCivil" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
                    </div>
                </div>
                <div class="col-md-2">
                    <div class="form-group">
                        <span class="">GÊNERO:</span>
                        <asp:DropDownList ID="ddlSexo" runat="server" CssClass="form-control">
                            <asp:ListItem Value="" Text="Selecione..."></asp:ListItem>
                            <asp:ListItem Value="MASCULINO" Text="MASCULINO"></asp:ListItem>
                            <asp:ListItem Value="FEMININO" Text="FEMININO"></asp:ListItem>
                            <asp:ListItem Value="OUTRO" Text="OUTRO"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator25" runat="server" ControlToValidate="ddlSexo" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
                    </div>
                </div>
                <div class="col-md-2">
                    <div class="form-group">
                        <span class="">JORNADA DE TRABALHO:</span>
                        <asp:DropDownList ID="ddlJornada" runat="server" CssClass="form-control"></asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator26" runat="server" ControlToValidate="ddlJornada" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
                    </div>
                </div>
                <div class="col-md-1">
                    <div class="form-group">
                        <span class="details">CÓDIGO:</span>
                        <asp:TextBox ID="txtCodTra" runat="server" Style="text-align: center" CssClass="form-control" placeholder="" AutoPostBack="true"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator27" runat="server" ControlToValidate="txtCodTra" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
                    </div>
                </div>
                <div class="col-md-5">
                    <div class="form_group">
                        <span class="details">PROPRIETÁRIO/TRANSPORTADORA:</span>
                        <asp:DropDownList ID="ddlAgregados" class="form-control select2" runat="server" OnSelectedIndexChanged="ddlAgregados_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    </div>
                </div>
            </div>
            <!-- Linha 5 do Formulário -->
            <div class="row g-3">
                <div class="col-md-2">
                    <div class="form-group">
                        <span class="details">LIB. RISCO:</span>
                        <asp:TextBox ID="txtCodLibRisco" runat="server" CssClass="form-control" value=""></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-1">
                    <div class="form-group">
                        <span class="details">VALIDADE:</span>
                        <div class="input-group">
                            <asp:TextBox ID="txtValLibRisco" runat="server" class="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-md-1">
                    <div class="form-group">
                        <span class="details">EXAME TOX.:</span>
                        <div class="input-group">
                            <asp:TextBox ID="txtVAlExameTox" runat="server" class="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-md-1">
                    <div class="form-group">
                        <span class="details">MOOP:</span>
                        <div class="input-group">
                            <asp:TextBox ID="txtVAlMoop" runat="server" class="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-md-2">
                    <div class="form-group">
                        <span class="details">FIXO:</span>
                        <div class="input-group">
                            <asp:TextBox ID="txtFixo" runat="server" class="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-md-2">
                    <div class="form-group">
                        <span class="details">CELULAR:</span>
                        <div class="input-group">
                            <asp:TextBox ID="txtCelular" runat="server" class="form-control"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator28" runat="server" ControlToValidate="txtCelular" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
                        </div>
                    </div>
                </div>
                <div class="col-md-1">
                    <div class="form-group">
                        <span class="details">CRACHÁ:</span>
                        <asp:TextBox ID="txtCracha" runat="server" CssClass="form-control" value=""></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-1">
                    <div class="form-group">
                        <span class="details">FROTA:</span>
                        <asp:TextBox ID="txtFrota" runat="server" CssClass="form-control" value=""></asp:TextBox>
                    </div>
                </div>

            </div>
            <!-- Linha 6 do formulário -->
            <div class="row g-3">
                <div class="col-md-1">
                    <div class="form-group">
                        <span class="details">CEP:</span>
                        <asp:TextBox ID="txtCepCli" runat="server" CssClass="form-control" Width="130px" placeholder="99999-999" MaxLength="9"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator29" runat="server" ControlToValidate="txtCepCli" InitialValue="" ErrorMessage="* Obrigatório" ValidationGroup="Cadastro" Font-Size="9px" ForeColor="Red" Display="Dynamic" />
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
                    </div>
                </div>
                <div class="col-md-1">
                    <div class="form-group">
                        <span class="details">Nº:</span>
                        <asp:TextBox ID="txtNumero" Style="text-align: center" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-2">
                    <div class="form-group">
                        <span class="details">COMPL.:</span>
                        <asp:TextBox ID="txtComplemento" Style="text-align: center" runat="server" CssClass="form-control" placeholder="" MaxLength="15"> </asp:TextBox>
                    </div>
                </div>
            </div>
            <!-- Linha 7 do formulário -->
            <div class="row g-3">
                <div class="col-md-5">
                    <div class="form-group">
                        <span class="details">BAIRRO:</span>
                        <asp:TextBox ID="txtBaiCli" runat="server" CssClass="form-control" placeholder="" MaxLength="60"></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <span class="details">MUNICIPIO:</span>
                        <asp:TextBox ID="txtCidCli" runat="server" CssClass="form-control" placeholder="" MaxLength="60"></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-1">
                    <div class="form-group">
                        <span class="details">UF:</span>
                        <asp:TextBox ID="txtEstCli" Style="text-align: center" runat="server" CssClass="form-control" placeholder="" MaxLength="2"></asp:TextBox>
                    </div>
                </div>

            </div>
            <!-- Linha 8 do formulário -->
            <div class="row g-3">
                <div class="col-md-2">
                    <div class="form-group">
                        <span class="details">CADASTRADO EM:</span>
                        <asp:Label ID="lblDtCadastro" runat="server" CssClass="form-control" placeholder=""></asp:Label>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <span class="details">POR:</span>
                        <asp:TextBox ID="txtUsuCadastro" runat="server" CssClass="form-control" placeholder="" MaxLength="60"></asp:TextBox>
                    </div>
                </div>
            </div>
            <!-- Linha 9 do formulário -->
            <div class="row g-3">
                <div class="col-md-1">
                    <asp:Button ID="btnSalvar1" CssClass="btn btn-outline-success  btn-lg" runat="server" Text="Cadastrar" OnClick="btnSalvar1_Click" />
                </div>
                <div class="col-md-1">
                    <a href="/dist/pages/ConsultaMotoristas.aspx" class="btn btn-outline-danger btn-lg">Sair               
                    </a>
                </div>
            </div>
        </div>
        </section>
    </div>
</asp:Content>
