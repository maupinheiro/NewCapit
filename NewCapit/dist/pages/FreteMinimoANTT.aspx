<%@ Page Title="" Language="C#" MasterPageFile="~/dist/pages/Main.Master" AutoEventWireup="true" CodeBehind="FreteMinimoANTT.aspx.cs" Inherits="NewCapit.dist.pages.FreteMinimoANTT" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- Bootstrap e jQuery -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>

    <!-- Plugin Bootstrap Multiselect -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap4-multiselect/dist/css/bootstrap-multiselect.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap4-multiselect/dist/js/bootstrap-multiselect.min.js"></script>

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
                        <div class="card-header">
                            <h3 class="card-title">
                                <h3 class="card-title"><i class="fas fa-chart-line"></i>&nbsp;Frete Mínimo ANTT</h3>
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
                                <center><span style="color: blue;">Calcular Piso Mínimo de Frete</span></center>
                            </h3>
                            <div class="row g-3">
                                <div class="col-md-6">
                                    <div class="card card-default">
                                        <div class="card-header">
                                            <h3 class="card-title">
                                                <i class="fas fa-comments-dollar"></i>
                                                Dados do Frete
                                             </h3>
                                        </div>
                                        <!-- /.card-header -->
                                        <div class="card-body">
                                            <div class="callout callout-info">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <span class="details">Tipo de Carga:</span>
                                                        <asp:DropDownList ID="ddlTipoCarga" runat="server" CssClass="form-control">
                                                            <asp:ListItem Value="" Text="Selecione..."></asp:ListItem>
                                                            <asp:ListItem Value="Granel sólido" Text="Granel sólido"></asp:ListItem>
                                                            <asp:ListItem Value="Granel líquido" Text="Granel líquido"></asp:ListItem>
                                                            <asp:ListItem Value="Frigorificada ou Aquecida" Text="Frigorificada ou Aquecida"></asp:ListItem>
                                                            <asp:ListItem Value="Conteinerizada" Text="Conteinerizada"></asp:ListItem>
                                                            <asp:ListItem Value="Carga Geral" Text="Carga Geral"></asp:ListItem>
                                                            <asp:ListItem Value="Neogranel" Text="Neogranel"></asp:ListItem>
                                                            <asp:ListItem Value="Perigosa (granel sólido)" Text="Perigosa (granel sólido)"></asp:ListItem>
                                                            <asp:ListItem Value="Perigosa (granel líquido)" Text="Perigosa (granel líquido)"></asp:ListItem>
                                                            <asp:ListItem Value="Perigosa (frigorificada ou aquecida)" Text="Perigosa (frigorificada ou aquecida)"></asp:ListItem>
                                                            <asp:ListItem Value="Perigosa (conteinerizada)" Text="Perigosa (conteinerizada)"></asp:ListItem>
                                                            <asp:ListItem Value="Perigosa (carga geral)" Text="Perigosa (carga geral)"></asp:ListItem>
                                                            <asp:ListItem Value="Carga Granel Pressurizada" Text="Carga Granel Pressurizada"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <!-- /.card-body -->
                                    </div>
                                    <!-- /.card -->
                                </div>

                                <div class="col-md-6">
                                    <div class="card card-default">
                                        <div class="card-header">
                                            <h3 class="card-title">
                                                <i class="fas fa-chart-pie"></i>
                                                Tabela ANTT Oficial
                                            </h3>
                                        </div>
                                        <!-- /.card-header -->
                                        <div class="card-body">
                                            <div class="callout callout-success">
                                                <h5>I am a success callout!</h5>

                                                <p>This is a green callout.</p>
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
