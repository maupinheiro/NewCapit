using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp.text.pdf;
using iTextSharp.text;
using static NewCapit.dist.pages.AbrirOS;
using DocumentFormat.OpenXml.EMMA;

namespace NewCapit
{
    public class EventoOS : PdfPageEventHelper
    {
        public OrdemServico os;
        public iTextSharp.text.Image logo;
        public iTextSharp.text.Image qr;

        public override void OnEndPage(PdfWriter writer, Document doc)
        {
            Font fonte8 = new Font(Font.FontFamily.HELVETICA, 8);
            Font titulo = new Font(Font.FontFamily.HELVETICA, 18, Font.BOLD);

            /* ---------- LINHA 1 CABEÇALHO ---------- */

            PdfPTable cab1 = new PdfPTable(3);
            cab1.TotalWidth = doc.PageSize.Width - 40;
            cab1.SetWidths(new float[] { 20f, 60f, 20f });

            PdfPCell c1 = new PdfPCell(logo);
            c1.Border = 0;

            PdfPCell c2 = new PdfPCell(new Phrase("ORDEM DE SERVIÇO", titulo));
            c2.HorizontalAlignment = Element.ALIGN_CENTER;
            c2.VerticalAlignment = Element.ALIGN_MIDDLE;
            c2.Border = 0;

            Barcode128 barcode = new Barcode128();
            barcode.Code = os.numero_os.ToString();
            var imgBar = barcode.CreateImageWithBarcode(writer.DirectContent, null, null);

            PdfPCell c3 = new PdfPCell(imgBar);
            c3.HorizontalAlignment = Element.ALIGN_RIGHT;
            c3.Border = 0;

            cab1.AddCell(c1);
            cab1.AddCell(c2);
            cab1.AddCell(c3);

            cab1.WriteSelectedRows(0, -1, 20, doc.PageSize.Height - 10, writer.DirectContent);

            /* ---------- LINHA 2 CABEÇALHO ---------- */

            PdfPTable cab2 = new PdfPTable(3);
            cab2.TotalWidth = doc.PageSize.Width - 40;

            PdfPCell usuario = new PdfPCell(
                new Phrase("Usuário: " + os.resp_abertura, fonte8));
            usuario.Border = 0;

            PdfPCell data = new PdfPCell(
                new Phrase("Data: " + os.data_abertura + " - Aberta", fonte8));
            data.HorizontalAlignment = Element.ALIGN_CENTER;
            data.Border = 0;

            PdfPCell status = new PdfPCell(
                new Phrase(" ", fonte8));
            status.HorizontalAlignment = Element.ALIGN_RIGHT;
            status.Border = 0;

            cab2.AddCell(usuario);
            cab2.AddCell(data);
            cab2.AddCell(status);

            cab2.WriteSelectedRows(0, -1, 20, doc.PageSize.Height - 35, writer.DirectContent);

            /* ---------- RODAPÉ ---------- */

            PdfPTable rod = new PdfPTable(4);
            rod.TotalWidth = doc.PageSize.Width - 40;
            rod.SetWidths(new float[] { 40f, 25f, 25f, 10f });

            PdfPCell assinatura = new PdfPCell(
                new Phrase("\n\n\nAssinatura Motorista\n" + os.nome_motorista, fonte8));
            assinatura.HorizontalAlignment = Element.ALIGN_CENTER;

            PdfPCell pagina = new PdfPCell(
                new Phrase("\n\n\nLiberada por\nPágina " + writer.PageNumber, fonte8));
            pagina.HorizontalAlignment = Element.ALIGN_CENTER;

            PdfPCell baixa = new PdfPCell(
                new Phrase("\n\n\nBaixada por", fonte8));
            baixa.HorizontalAlignment = Element.ALIGN_CENTER;

            PdfPCell cellQr = new PdfPCell(qr);
            cellQr.HorizontalAlignment = Element.ALIGN_RIGHT;

            rod.AddCell(assinatura);
            rod.AddCell(pagina);
            rod.AddCell(baixa);
            rod.AddCell(cellQr);

            rod.WriteSelectedRows(0, -1, 20, 70, writer.DirectContent);            
        }
        
    }
}