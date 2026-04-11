<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UploadNF.aspx.cs" Inherits="NewCapit.dist.pages.UploadNF" %>

<!DOCTYPE html>
<html lang="pt-br">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Upload de Nota Fiscal - TRANSNOVAG</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        body { background-color: #f8f9fa; }
        .upload-card { max-width: 500px; margin: 50px auto; border-radius: 15px; box-shadow: 0 4px 15px rgba(0,0,0,0.1); }
        .btn-primary { background-color: #004a99; border: none; }
        .btn-primary:hover { background-color: #003366; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="card upload-card">
                <div class="card-header bg-primary text-white text-center py-3">
                    <h4 class="mb-0">Comprovante de Abastecimento</h4>
                </div>
                <div class="card-body p-4 text-center">
                    <p class="text-muted">Ordem: <strong><asp:Literal ID="litOrdem" runat="server" /></strong></p>
                    
                    <div class="mb-4">
                        <label for="FileUpload1" class="form-label fw-bold">Selecione a foto da Nota Fiscal</label>
                        <asp:FileUpload ID="FileUpload1" runat="server" CssClass="form-control" />
                    </div>

                    <asp:Button ID="btnEnviar" runat="server" Text="Enviar Nota Fiscal" 
                        CssClass="btn btn-primary btn-lg w-100 mb-3" OnClick="btnEnviar_Click" />
                    
                    <asp:Label ID="lblStatus" runat="server" CssClass="d-block mt-2 fw-bold"></asp:Label>
                </div>
                <div class="card-footer text-center text-muted small">
                    TRANSNOVAG TRANSPORTES S/A
                </div>
            </div>
        </div>
    </form>
</body>
</html>
