<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="NewCapit.dist.pages.WebForm1" %>

<!-- modelo de form padrão -->
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <div class="content-wrapper">
        <section class="content">
            <div class="container-fluid">
                <br />
                <div id="toastContainerVermelho" class="alert alert-danger alert-dismissible" style="display: none;">
                    <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                    <h5><i class="icon fas fa-exclamation-triangle"></i>Alerta!</h5>
                    Alertas
                </div>
                <div class="col-md-12">
                    <div class="card card-info">
                        <div class="card-header" style="background-color: #A020F0; font-weight: bold;">
                            <h3 class="card-title">
                                <h3 class="card-title"><i class="fas fa-shipping-fast"></i>&nbsp;Manutenção - Abertura de Ordem de Serviço</h3>
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
                                
                          
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>


</asp:Content>

<!-- acordion aberto -->
<div class="card card-outline card-success">
    <div class="card-header">
        <h3 class="card-title"><i class="far fa-edit"></i>&nbsp;Local da Coleta</h3>
        <div class="card-tools">
            <button type="button" class="btn btn-tool" data-card-widget="collapse">
                <i class="fas fa-minus"></i>
            </button>
        </div>
        <!-- /.card-tools -->
    </div>
    <div class="card-body">
        <p>ACORDION ABERTO</p>
    </div>
</div>
<!-- Acordion fechado -->
<div class="card card-outline card-info collapsed-card">
    <div class="card-header">
        <h3 class="card-title"><i class="far fa-edit"></i>&nbsp;Mecânica</h3>
        <div class="card-tools">
            <button type="button" class="btn btn-tool" data-card-widget="collapse">
                <i class="fas fa-plus"></i>
            </button>
        </div>

    </div>

    <div class="card-body">
        <div class="form-group">
            <label>Descrição do Problema</label>
            <asp:TextBox ID="txtDescricao" runat="server"
                TextMode="MultiLine"
                Rows="4"
                CssClass="form-control"></asp:TextBox>
        </div>
        <br />
    </div>

</div>
