using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp.text.pdf;
using iTextSharp.text;
using static NewCapit.dist.pages.AbrirOS;

namespace NewCapit.dist.pages
{
    public class EventoOS : PdfPageEventHelper
    {
        public OrdemServico os;
        public Image qr;
        public Image logo;

        public override void OnEndPage(PdfWriter writer, Document doc)
        {
            Font fonte8 = new Font(Font.FontFamily.HELVETICA, 8);
            Font fonteTitulo = new Font(Font.FontFamily.HELVETICA, 20, Font.BOLD);

            // CABEÇALHO
            PdfPTable cabecalho = new PdfPTable(3);
            cabecalho.TotalWidth = doc.PageSize.Width - 40;
            cabecalho.SetWidths(new float[] { 25f, 50f, 25f });

            PdfPCell cellLogo = new PdfPCell(logo);
            cellLogo.Border = 0;

            PdfPCell titulo = new PdfPCell(new Phrase("ORDEM DE SERVIÇO", fonteTitulo));
            titulo.HorizontalAlignment = Element.ALIGN_CENTER;
            titulo.VerticalAlignment = Element.ALIGN_MIDDLE;
            titulo.Border = 0;

            Barcode128 barcode = new Barcode128();
            barcode.Code = os.numero_os.ToString();

            Image imgBar = barcode.CreateImageWithBarcode(writer.DirectContent, null, null);

            PdfPCell cellBar = new PdfPCell(imgBar);
            cellBar.Border = 0;
            cellBar.HorizontalAlignment = Element.ALIGN_RIGHT;

            cabecalho.AddCell(cellLogo);
            cabecalho.AddCell(titulo);
            cabecalho.AddCell(cellBar);

            cabecalho.WriteSelectedRows(
                0, -1,
                20,
                doc.PageSize.Height - 10,
                writer.DirectContent);

            // RODAPÉ
            PdfPTable rodape = new PdfPTable(3);
            rodape.TotalWidth = doc.PageSize.Width - 40;
            rodape.SetWidths(new float[] { 40f, 20f, 40f });

            PdfPCell vazio = new PdfPCell(new Phrase(""));
            vazio.Border = 0;

            PdfPCell pagina = new PdfPCell(
                new Phrase("Página " + writer.PageNumber, fonte8));
            pagina.Border = 0;
            pagina.HorizontalAlignment = Element.ALIGN_CENTER;

            PdfPCell cellQr = new PdfPCell(qr);
            cellQr.Border = 0;
            cellQr.HorizontalAlignment = Element.ALIGN_RIGHT;

            rodape.AddCell(vazio);
            rodape.AddCell(pagina);
            rodape.AddCell(cellQr);

            rodape.WriteSelectedRows(
                0, -1,
                20,
                30,
                writer.DirectContent);
        }
    }
}