<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="Frm_ImpSolVWMatriz.aspx.cs" Inherits="NewCapit.dist.pages.Frm_ImpSolVWMatriz" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- Bootstrap e jQuery -->
    <%--  <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>--%>

    <!-- Plugin Bootstrap Multiselect -->
    <%--<link href="https://cdn.jsdelivr.net/npm/bootstrap4-multiselect/dist/css/bootstrap-multiselect.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap4-multiselect/dist/js/bootstrap-multiselect.min.js"></script>--%>


    <script>
        function somenteNumeros(e) {
            var charCode = e.which || e.keyCode;

            // Permitir: backspace, delete, tab, setas
            if (charCode == 8 || charCode == 9 || charCode == 46 || (charCode >= 37 && charCode <= 40)) {
                return true;
            }

            // Permitir apenas números (48–57)
            if (charCode < 48 || charCode > 57) {
                return false;
            }
            return true;
        }
    </script>
    <%--<script>

        function showToast(msg) {
            document.getElementById("toastMsg").innerHTML = msg;
            var toast = new bootstrap.Toast(document.getElementById("toastErro"));
            toast.show();
        }

        function validarCampos() {
            var erro = false;

            var campos = [
                { id: "<%= ddlTipoCarga.ClientID %>", nome: "Informe um tipo de carga." },
                { id: "<%= ddlEixos.ClientID %>", nome: "Informe a quantidade de eixos." },
                { id: "<%= txtDistancia.ClientID %>", nome: "Informe a distância a ser percorrida." }
            ];

            campos.forEach(function (c) {
                var valor = document.getElementById(c.id).value.trim();
                if (valor === "") {
                    showToast("O campo <b>" + c.nome + "</b> está vazio!");
                    erro = true;
                }
            });

            return !erro;  // Se tiver erro, cancela o postback
        }

    </script>--%>
    <script>
        function enviarPasta() {
            const input = document.getElementById("folderInput");
            const files = input.files;

            if (!files || files.length === 0) {
                alert("Selecione uma pasta");
                return;
            }

            const formData = new FormData();
            let totalValidos = 0;

            for (let i = 0; i < files.length; i++) {
                const nome = files[i].name.toUpperCase();

                if (nome.startsWith("SG") && nome.endsWith(".TXT")) {
                    totalValidos++;
                    formData.append("files", files[i]);
                }
            }

            if (totalValidos === 0) {
                alert("Nenhum arquivo SG*.txt encontrado");
                return;
            }

            alert('Serão importados ${totalValidos} arquivos SG*.txt');


            if (!encontrouArquivoValido) {
                alert("Nenhum arquivo SG*.txt encontrado na pasta selecionada");
                return;
            }

            fetch("UploadHandler.ashx", {
                method: "POST",
                body: formData
            }).then(() => {
                // segue seu fluxo normal
                iniciar();
            }).catch(err => {
                console.error(err);
                alert("Erro ao enviar arquivos");
            });
        }
    </script>
  <script>
      function abrirPasta() {
          document.getElementById("folderInput").click();
      }

      document.getElementById("folderInput").addEventListener("change", function () {
          const files = this.files;
          let totalValidos = 0;
          const formData = new FormData();

          for (let file of files) {
              const nome = file.name.toUpperCase();
              if (nome.startsWith("SG") && nome.endsWith(".TXT")) {
                  totalValidos++;
                  formData.append("files", file);
              }
          }

          if (totalValidos === 0) {
              document.getElementById("lblResumo").innerText =
                  "Nenhum arquivo SG*.txt encontrado na pasta selecionada";
              return;
          }

          document.getElementById("lblResumo").innerText =
              `${totalValidos} arquivos SG*.txt encontrados`;

          fetch("UploadHandler.ashx", {
              method: "POST",
              body: formData
          })
              .then(resp => {
                  if (!resp.ok) throw new Error("Erro no upload");
                  // 👉 AGORA sim
                  iniciar();                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        
              })
              .catch(err => {
                  console.error(err);
                  alert("Erro ao enviar arquivos");
              });
      });
  </script>



    <script>
        function iniciar() {

            fetch('Frm_ImpSolVWMatriz.aspx/IniciarImportacao', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' }
            });

            let timer = setInterval(() => {

                fetch('Frm_ImpSolVWMatriz.aspx/Progresso', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' }
                })
                    .then(r => r.json())
                    .then(r => {
                        let d = r.d;

                        let bar = document.getElementById('<%= barProgresso.ClientID %>');
            bar.style.width = d.percentual + '%';
            bar.innerText = d.percentual + '%';

            document.getElementById('<%= lblStatus.ClientID %>')
                .innerText = `Processando ${d.atual} de ${d.total}`;

            if (d.concluido) {
                clearInterval(timer);
                document.getElementById('<%= lblStatus.ClientID %>')
                    .innerText = '✅ Concluído';
            }
        });

    }, 500);
        }
    </script>


    <div class="content-wrapper">
        <section class="content">
            <div class="container-fluid">
                <br />
                <%--<asp:ScriptManager runat="server" />--%>
                <div class="col-md-12">
                    <div class="card card-info">
                        <div class="card-header">
                            <h3 class="card-title">
                                <h3 class="card-title"><i class="fas fa-chart-line"></i>&nbsp;Importação de Frete Mínimo ANTT</h3>
                            </h3>
                            <div class="card-tools">
                                <button type="button" class="btn btn-tool" data-card-widget="maximize">
                                    <i class="fas fa-expand"></i>
                                </button>
                                <button type="button" class="btn btn-tool" data-card-widget="collapse">
                                    <i class="fas fa-minus"></i>
                                </button>
                                <button type="button" class="btn btn-tool" data-card-widget="remove">
                                    <i class="fas fa-times"></i>
                                </button>
                            </div>
                            <!-- /.card-tools -->
                        </div>
                        <div class="card-body">
                            <h3>
                                <center><span style="color: blue;">Solicitações Volkswagen</span></center>
                            </h3>
                            <div class="row g-3">
                                <div class="col-md-12">
                                    <div class="card card-default">
                                        <div class="card-header">
                                            <h3 class="card-title">
                                                <i class="fas fa-dolly-flatbed"></i>
                                                Importar Solicitações VW
                                            </h3>
                                        </div>
                                        <!-- /.card-header -->
                                        <div class="card-body">
                                            <div class="callout callout-info">
                                                <div class="row g-3">
                                                    <div class="col-md-12">

                                                      

                                                          <asp:UpdatePanel ID="upd" runat="server" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <div class="col-md-3">
                                                                            <input
                                                                           type="file"
                                                                           id="folderInput"
                                                                           webkitdirectory
                                                                           style="position:absolute; left:-9999px; width:1px; height:1px; opacity:0;"/>
                                                                            <button type="button" class="btn btn-primary" onclick="abrirPasta()">
                                                                            Selecionar pasta
                                                                        </button>
                                                                            <div id="lblResumo" class="mt-2"></div>
                                                                        </div>
                                                                          <div class="col-md-3">
                                                                               <asp:Button ID="btnImportar" runat="server"
                                                                                 Text="Importar Arquivos"
                                                                                 CssClass="btn btn-primary"
                                                                                 OnClientClick="enviarPasta(); return false;" />
                                                                          </div>
                                                                     
                                                                         

                                                                            

                                                                            


                                                                       


                                                                        <br /><br />
                                                                       
                                                                        <div class="progress">
                                                                            <div id="barProgresso" runat="server"
                                                                                class="progress-bar progress-bar-striped progress-bar-animated"
                                                                                role="progressbar"
                                                                                style="width: 0%">
                                                                                
                                                                            </div>
                                                                        </div>

                                                                        <br />

                                                                        <asp:Label ID="lblStatus" runat="server"
                                                                            CssClass="alert alert-info" />

                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>



                                                    </div>
                                                </div>
                                                <!-- botao /.row -->
                                                <div class="row g-3">
                                                    <div class="col-md-3">
                                                        <div class="form-group">
                                                            <span class="details">&nbsp;</span>
                                                            <asp:TextBox ID="txtUsuario" runat="server" class="form-control font-weight-bold" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-7">
                                                        <div class="form-group">
                                                            <span class="details">&nbsp;</span>                                                            
                                                        </div>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <div class="form-group">
                                                            <asp:Button ID="btnSair" runat="server" CssClass="btn btn-outline-info float-right"
                                                                Text="Sair" />
                                                        </div>
                                                    </div>
                                                </div>

                                            </div>
                                        </div>
                                        <!-- /.card-body -->
                                    </div>
                                    <!-- /.card -->
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>

</asp:Content>
