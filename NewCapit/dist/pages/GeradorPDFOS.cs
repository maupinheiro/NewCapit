using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using iTextSharp.text.pdf;
using iTextSharp.text;
using static NewCapit.dist.pages.AbrirOS;
using System.Drawing;
using FontPDF = iTextSharp.text.Font;
using ImagePDF = iTextSharp.text.Image;

namespace NewCapit.dist.pages
{
    public class GeradorPDFOS
    {
        public static byte[] GerarPDF(OrdemServico os)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Document doc = new Document(PageSize.A4, 20, 20, 100, 70);
                Document doc = new Document(PageSize.A4, 20, 20, 110, 150);
                PdfWriter writer = PdfWriter.GetInstance(doc, ms);

                // LOGO
                iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(
                HttpContext.Current.Server.MapPath("~/img/logo_transnovag.png"));

                logo.ScaleToFit(50, 60);

                // QR CODE

                //                BarcodeQRCode qr = new BarcodeQRCode(
                //"https://sistema.suaempresa.com/os?id=" + os.numero_os,
                //120, 120, null);

                BarcodeQRCode qr = new BarcodeQRCode(
                "OS:" + os.numero_os + "|PLACA:" + os.placa,
                120, 120, null);

                iTextSharp.text.Image imgQr = qr.GetImage();
                imgQr.ScaleAbsolute(60, 60);

                // EVENTO
                EventoOS evento = new EventoOS();
                evento.os = os;
                evento.logo = logo;
                evento.qr = imgQr;

                writer.PageEvent = evento;

                doc.Open();
                FontPDF titulo = new FontPDF(FontPDF.FontFamily.HELVETICA, 8, FontPDF.BOLD);
                FontPDF cab = new FontPDF(FontPDF.FontFamily.HELVETICA, 8, FontPDF.BOLD);
                FontPDF txt = new FontPDF(FontPDF.FontFamily.HELVETICA, 8, FontPDF.BOLD);

                // fonte tamanho 8
                iTextSharp.text.Font fonte8 = new iTextSharp.text.Font(
                    iTextSharp.text.Font.FontFamily.HELVETICA, 8);
                // fonte tamanho 10 negrito
                iTextSharp.text.Font fonte10negrito = new iTextSharp.text.Font(
    iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.BOLD);
                // título
                var fonteTitulo = new iTextSharp.text.Font(
                    iTextSharp.text.Font.FontFamily.HELVETICA, 20, iTextSharp.text.Font.BOLD);

                // VEÍCULO                
                //doc.Add(new Paragraph(" ", cab));
                // tabela veículo
                PdfPTable vei = new PdfPTable(2);
                vei.WidthPercentage = 100;
                vei.SplitRows = true;
                vei.SplitLate = false;
                vei.KeepTogether = false;                
                vei.AddCell(new Phrase("Veículo: " + os.id_veiculo + " - " + os.placa + " / " + os.tipo_veiculo, fonte8));
                vei.AddCell(new Phrase("Marca: " + os.marca + " - " + os.modelo + " / " + os.ano_modelo, fonte8));
                doc.Add(vei);
                PdfPTable veiLinha2 = new PdfPTable(2);
                veiLinha2.WidthPercentage = 100;
                veiLinha2.AddCell(new Phrase("Núcleo: " + os.nucleo_veiculo, fonte8));
                veiLinha2.AddCell(new Phrase("KM: " + os.km_abertura, fonte8));
                doc.Add(veiLinha2);

                // MOTORISTA
                doc.Add(new Paragraph(" ", cab));
                PdfPTable mot = new PdfPTable(2);
                mot.SplitRows = true;
                mot.SplitLate = false;
                mot.KeepTogether = false;
                mot.WidthPercentage = 100;
                mot.AddCell(new Phrase("Motorista: " + os.id_motorista + " - " + os.nome_motorista, fonte8));
                mot.AddCell(new Phrase("Transportadora: " + os.transp_motorista + " / " + os.nucleo_motorista, fonte8));
                doc.Add(mot);

                // TIPO OS
                doc.Add(new Paragraph(" ", cab));
                PdfPTable tipo = new PdfPTable(2);
                tipo.WidthPercentage = 100;
                tipo.SplitRows = true;
                tipo.SplitLate = false;
                tipo.KeepTogether = false;
                float[] larguraTipo = { 30f, 70f };
                tipo.SetWidths(larguraTipo);
                tipo.AddCell(new Phrase("Tipo OS: " + os.tipo_os + " - " + os.tipo_servico, fonte8));
                tipo.AddCell(new Phrase("Fornecedor: " + os.id_fornecedor + " - " + os.nome_fornecedor, fonte8));
                doc.Add(tipo);

                // MECÂNICA                
                doc.Add(new Paragraph(" ", cab));
                PdfPTable mecanica = new PdfPTable(1);
                mecanica.WidthPercentage = 100;
                mecanica.SplitRows = true;
                mecanica.SplitLate = false;
                mecanica.KeepTogether = false;
                mecanica.AddCell("1 - MECÂNICA");
                mecanica.AddCell(new Phrase("DEFEITOS APRESENTADOS:", fonte10negrito));
                mecanica.AddCell(new Phrase(os.parte_mecanica, fonte8));
                doc.Add(mecanica);

                // SERVIÇO EXECUTADO
                PdfPTable serv = new PdfPTable(1);
                serv.WidthPercentage = 100;
                serv.SplitRows = true;
                serv.SplitLate = false;
                serv.KeepTogether = false;
                serv.AddCell(new Phrase("SERVIÇO EXECUTADO:", fonte10negrito));
                for (int i = 0; i < 5; i++)
                    serv.AddCell(" ");
                doc.Add(serv);
                // PEÇAS  
                doc.Add(CriarTabelaPecas(fonte8));

                // ELETRICA                
                doc.Add(new Paragraph(" ", cab));
                PdfPTable eletrica = new PdfPTable(1);
                eletrica.WidthPercentage = 100;
                eletrica.SplitRows = true;
                eletrica.SplitLate = false;
                eletrica.KeepTogether = false;
                eletrica.AddCell("2 - ELETRICA");
                eletrica.AddCell(new Phrase("DEFEITOS APRESENTADOS:", fonte10negrito));
                eletrica.AddCell(new Phrase(os.parte_eletrica, fonte8));
                doc.Add(eletrica);
                // SERVIÇO EXECUTADO              
                doc.Add(serv);
                // PEÇAS  
                doc.Add(CriarTabelaPecas(fonte8));

                // BORRACHARIA                
                doc.Add(new Paragraph(" ", cab));
                PdfPTable borracharia = new PdfPTable(1);
                borracharia.WidthPercentage = 100;
                borracharia.SplitRows = true;
                borracharia.SplitLate = false;
                borracharia.KeepTogether = false;
                borracharia.AddCell("3 - BORRACHARIA");
                borracharia.AddCell(new Phrase("DEFEITOS APRESENTADOS:", fonte10negrito));
                borracharia.AddCell(new Phrase(os.parte_borracharia, fonte8));
                doc.Add(borracharia);
                // SERVIÇO EXECUTADO              
                doc.Add(serv);
                // PEÇAS  
                doc.Add(CriarTabelaPecas(fonte8));

                // FUNILARIA                
                doc.Add(new Paragraph(" ", cab));
                PdfPTable funilaria = new PdfPTable(1);
                funilaria.WidthPercentage = 100;
                funilaria.SplitRows = true;
                funilaria.SplitLate = false;
                funilaria.KeepTogether = false;
                funilaria.AddCell("4 - FUNILARIA");
                funilaria.AddCell(new Phrase("DEFEITOS APRESENTADOS:", fonte10negrito));
                funilaria.AddCell(new Phrase(os.parte_funilaria, fonte8));
                doc.Add(funilaria);
                // SERVIÇO EXECUTADO              
                doc.Add(serv);
                // PEÇAS  
                doc.Add(CriarTabelaPecas(fonte8));
                doc.Add(new Paragraph(" ", cab));

                // doc.Add(imgQr);
                doc.Close();

                return ms.ToArray();
            }
        }
        public static PdfPTable CriarTabelaPecas(iTextSharp.text.Font fonte8)
        {
            PdfPTable pecas = new PdfPTable(6);
            pecas.WidthPercentage = 100;

            // controle de quebra de página
            pecas.SplitRows = true;
            pecas.SplitLate = false;
            pecas.KeepTogether = false;

            // largura das colunas
            float[] largura = { 5f, 45f, 15f, 15f, 10f, 10f };
            pecas.SetWidths(largura);

            // função cabeçalho
            PdfPCell Cab(string texto)
            {
                PdfPCell c = new PdfPCell(new Phrase(texto, fonte8));
                c.HorizontalAlignment = Element.ALIGN_CENTER;
                c.VerticalAlignment = Element.ALIGN_MIDDLE;
                c.BackgroundColor = BaseColor.LIGHT_GRAY;
                return c;
            }

            // função célula normal
            PdfPCell Cel(string texto, int alinhamento)
            {
                PdfPCell c = new PdfPCell(new Phrase(texto, fonte8));
                c.HorizontalAlignment = alinhamento;
                c.VerticalAlignment = Element.ALIGN_MIDDLE;
                return c;
            }

            // cabeçalho
            pecas.AddCell(Cab("Qtd"));
            pecas.AddCell(Cab("Peça / Serviço Executado"));
            pecas.AddCell(Cab("Início"));
            pecas.AddCell(Cab("Término"));
            pecas.AddCell(Cab("Tempo"));
            pecas.AddCell(Cab("Crachá"));

            // repetir cabeçalho em nova página
            pecas.HeaderRows = 1;

            // linhas vazias (para preencher manual)
            for (int i = 0; i < 5; i++)
            {
                pecas.AddCell(Cel(" ", Element.ALIGN_CENTER));
                pecas.AddCell(Cel(" ", Element.ALIGN_LEFT));
                pecas.AddCell(Cel(" ", Element.ALIGN_CENTER));
                pecas.AddCell(Cel(" ", Element.ALIGN_CENTER));
                pecas.AddCell(Cel(" ", Element.ALIGN_CENTER));
                pecas.AddCell(Cel(" ", Element.ALIGN_CENTER));
            }

            return pecas;
        }

        static void CriarSecao(Document doc, string titulo, string texto)
        {
            FontPDF cab = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);

            doc.Add(new Paragraph(titulo, cab));

            PdfPTable tab = new PdfPTable(1);
            tab.WidthPercentage = 100;

            tab.AddCell(texto);

            doc.Add(tab);
        }

        



    }

}



