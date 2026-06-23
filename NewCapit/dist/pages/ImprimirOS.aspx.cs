using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Web;
using iTextSharp.text.pdf.qrcode;

namespace NewCapit.dist.pages
{
    public partial class ImprimirOS : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string idOS = Request.QueryString["id"];

                if (!string.IsNullOrEmpty(idOS))
                {
                    GerarPDFOS(idOS);
                }
            }
        }
        private void GerarPDFOS(string idOS)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                //Document doc = new Document(PageSize.A4, 20, 20, 20, 20);
                //PdfWriter writer = PdfWriter.GetInstance(doc, ms);

                //doc.Open();
                Document doc = new Document(PageSize.A4, 20, 20, 20, 60);

                //MemoryStream ms = new MemoryStream();

                PdfWriter writer = PdfWriter.GetInstance(doc, ms);

                // Adiciona QR Code e número da página no rodapé
                writer.PageEvent = new RodapeComQRCode("OS:" + idOS);

                doc.Open();

                Font titulo = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14);
                Font subtitulo = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 11);
                Font normal = FontFactory.GetFont(FontFactory.HELVETICA, 10);
                Font normal8 = FontFactory.GetFont(FontFactory.HELVETICA, 8);
                Font fonteCabecalho = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 8);

                #region CABEÇALHO

                PdfPTable cabecalho = new PdfPTable(3);
                cabecalho.WidthPercentage = 100;
                cabecalho.SetWidths(new float[] { 20, 60, 20 });

                // LOGO
                PdfPCell celLogo;

                string caminhoLogo = Server.MapPath("~/img/logo_transnovag.png");

                if (File.Exists(caminhoLogo))
                {
                    iTextSharp.text.Image logo =
                        iTextSharp.text.Image.GetInstance(caminhoLogo);

                    logo.ScaleToFit(60f, 60f);

                    celLogo = new PdfPCell(logo);
                }
                else
                {
                    celLogo = new PdfPCell(new Phrase(""));
                }

                celLogo.Border = Rectangle.NO_BORDER;
                celLogo.HorizontalAlignment = Element.ALIGN_LEFT;

                // EMPRESA  
                Paragraph pEmpresa = new Paragraph();
                pEmpresa.Alignment = Element.ALIGN_CENTER;

                pEmpresa.Add(new Chunk("TRANSNOVAG TRANSPORTES\n", titulo));
                pEmpresa.Add(new Chunk("Ordem de Serviço", subtitulo));

                PdfPCell celEmpresa = new PdfPCell();
                celEmpresa.AddElement(pEmpresa);

                celEmpresa.Border = Rectangle.NO_BORDER;
                celEmpresa.HorizontalAlignment = Element.ALIGN_CENTER;
                celEmpresa.VerticalAlignment = Element.ALIGN_MIDDLE;

                // CÓDIGO DE BARRAS
                Barcode128 barcode = new Barcode128();
                barcode.Code = idOS;

                iTextSharp.text.Image imgBarCode =
                    barcode.CreateImageWithBarcode(
                        writer.DirectContent,
                        null,
                        null);

                imgBarCode.ScalePercent(80);

                PdfPCell celCodigo = new PdfPCell(imgBarCode);
                celCodigo.Border = Rectangle.NO_BORDER;
                celCodigo.HorizontalAlignment = Element.ALIGN_RIGHT;

                cabecalho.AddCell(celLogo);
                cabecalho.AddCell(celEmpresa);
                cabecalho.AddCell(celCodigo);

                doc.Add(cabecalho);

                #endregion

                string sql = @"SELECT 
                o.id_os,
                o.placa,
                o.tipo_veiculo, 
                o.ano_modelo,
                o.marca,
                o.modelo,
                o.parte_mecanica,
                o.parte_eletrica,
                o.parte_borracharia,
                o.parte_funilaria,
                o.servico_executado_mecanica,
                o.servico_executado_eletrica,
                o.servico_executado_borracharia,
                o.servico_executado_funilaria,
                o.resp_abertura,
                o.km_abertura,
                o.nucleo_veiculo,
                o.transp_motorista,
                o.nome_fornecedor,
                CASE 
                    WHEN o.tipo_veiculo = 'CARRETA'
                        THEN o.id_carreta
                    ELSE o.id_veiculo
                END AS codigo_veiculo,
                CASE 
                    WHEN o.interno_externo = 'I' THEN 'Interno'
                    WHEN o.interno_externo = 'E' THEN 'Externo'    
                END AS servico_interno_externo,
                CASE 
                    WHEN o.tipo_os = 'C' THEN 'Corretiva'
                    WHEN o.tipo_os = 'P' THEN 'Preventiva'    
                END AS os_preventiva_corretiva,
                o.nucleo_veiculo,
                o.nome_motorista,
                o.tipo_os,
                o.data_abertura,                
                DATEDIFF(DAY, o.data_abertura,
                    CASE 
                        WHEN o.data_fechamento IS NOT NULL THEN o.data_fechamento
                        ELSE GETDATE()
                    END
                ) AS dias_aberta,
                CASE 
                    WHEN o.status = '1' THEN 'Aberta'
                    WHEN o.status = '2' THEN 'Finalizada'
                    WHEN o.status = '3' THEN 'Cancelada'
                END AS status_texto
                FROM tbordem_servico o
                WHERE o.id_os = @id";

                using (SqlConnection conn = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id", idOS);

                    conn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            //doc.Add(new Paragraph("Nº O.S.: " + dr["id_os"], normal));
                            //doc.Add(new Paragraph("Placa: " + dr["placa"], normal));
                            doc.Add(new Paragraph(
                                    "Veic./Placa: " + dr["codigo_veiculo"] + " - " + dr["placa"] +
                                    "   |   Tipo: " + dr["tipo_veiculo"] +
                                    "   |   Ano: " + dr["ano_modelo"] +
                                    "   |   Marca: " + dr["marca"] +
                                    "   |   Núcleo: " + dr["nucleo_veiculo"],
                                    normal));
                            if (dr["data_abertura"] != DBNull.Value)
                            {
                                doc.Add(new Paragraph(
                                    "Data Abertura: " +
                                    Convert.ToDateTime(dr["data_abertura"])
                                    .ToString("dd/MM/yyyy HH:mm") +
                                    "   |   Status: " + dr["status_texto"] +
                                    "   |   Aberta por: " + dr["resp_abertura"] +
                                    "   |   Km: " + dr["km_abertura"],
                                    normal));
                            }
                            doc.Add(new Paragraph("Motorista: " + dr["nome_motorista"] +
                                    "   |   Transportadora: " + dr["transp_motorista"], normal));
                            doc.Add(new Paragraph("Tipo de O.S.: " + dr["os_preventiva_corretiva"] +
                                    "   |   Serviço: " + dr["servico_interno_externo"] +
                                    "   |   Fornecedor: " + dr["nome_fornecedor"], normal));

                            // MECÂNICA
                            AdicionarSecao(
                                doc,
                                "DEFEITOS MECÂNICOS",
                                dr["parte_mecanica"].ToString(),
                                subtitulo,
                                normal8);

                            AdicionarSecao(
                                doc,
                                "SERVIÇOS EXECUTADOS MECÂNICA",
                                dr["servico_executado_mecanica"].ToString(),
                                subtitulo,
                                normal8);
                            AdicionarPecasPorTipo(
                                doc,
                                idOS,
                                "MECANICA",
                                subtitulo,
                                fonteCabecalho,
                                normal8);

                            // ELÉTRICA
                            AdicionarSecao(
                                 doc,
                                 "DEFEITOS ELÉTRICOS",
                                 dr["parte_eletrica"].ToString(),
                                 subtitulo,
                                 normal8);

                            AdicionarSecao(
                                doc,
                                "SERVIÇOS EXECUTADOS ELÉTRICA",
                                dr["servico_executado_eletrica"].ToString(),
                                subtitulo,
                                normal8);

                            AdicionarPecasPorTipo(
                                doc,
                                idOS,
                                "ELETRICA",
                                subtitulo,
                                fonteCabecalho,
                                normal8);

                            // BORRACHARIA
                            AdicionarSecao(
                               doc,
                               "DEFEITOS BORRACHARIA",
                               dr["parte_borracharia"].ToString(),
                               subtitulo,
                               normal8);

                            AdicionarSecao(
                                doc,
                                "SERVIÇOS EXECUTADOS BORRACHARIA",
                                dr["servico_executado_borracharia"].ToString(),
                                subtitulo,
                                normal8);

                            AdicionarPecasPorTipo(
                                doc,
                                idOS,
                                "BORRACHARIA",
                                subtitulo,
                                fonteCabecalho,
                                normal8);

                            // FUNILARIA
                            AdicionarSecao(
                                doc,
                                "DEFEITOS FUNILARIA",
                                dr["parte_funilaria"].ToString(),
                                subtitulo,
                                normal8);

                            AdicionarSecao(
                                doc,
                                "SERVIÇOS EXECUTADOS FUNILARIA",
                                dr["servico_executado_funilaria"].ToString(),
                                subtitulo,
                                normal8);

                            AdicionarPecasPorTipo(
                                doc,
                                idOS,
                                "FUNILARIA",
                                subtitulo,
                                fonteCabecalho,
                                normal8);
                        }
                    }
                }

                doc.Add(new Paragraph(" "));
                //doc.Add(new Paragraph(" "));

                doc.Close();

                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.AddHeader(
                    "Content-Disposition",
                    $"inline; filename=OS_{idOS}.pdf");

                Response.BinaryWrite(ms.ToArray());
                Response.Flush();
                Response.End();
            }
        }
        public class RodapeComQRCode : PdfPageEventHelper
        {
            private readonly string _textoQr;
            private readonly Font fonteRodape =
                FontFactory.GetFont(FontFactory.HELVETICA, 8);

            public RodapeComQRCode(string textoQr)
            {
                _textoQr = textoQr;
            }

            public override void OnEndPage(PdfWriter writer, Document document)
            {
                PdfContentByte cb = writer.DirectContent;

                // QR Code
                BarcodeQRCode qr = new BarcodeQRCode(_textoQr, 50, 50, null);
                Image imgQr = qr.GetImage();
                imgQr.ScaleAbsolute(40f, 40f);

                imgQr.SetAbsolutePosition(
                    document.Right - 40,
                    document.Bottom - 35);

                cb.AddImage(imgQr);

                // Número da página
                string textoPagina = "Página " + writer.PageNumber;

                ColumnText.ShowTextAligned(
                    cb,
                    Element.ALIGN_CENTER,
                    new Phrase(textoPagina, fonteRodape),
                    (document.Left + document.Right) / 2,
                    document.Bottom - 20,
                    0);
            }
        }
        private void AdicionarTabelaPecas(
        Document doc,
        string idOS,
        string tipo,
        Font subtitulo,
        Font fonteCabecalho,
        Font normal8)
        {
            PdfPTable tabela = new PdfPTable(6);
            tabela.WidthPercentage = 100;
            tabela.SetWidths(new float[] { 35, 10, 15, 20, 20, 20 });

            // TÍTULO DA TABELA
            PdfPCell titulo = new PdfPCell(
                new Phrase("PEÇAS UTILIZADAS - " + tipo, subtitulo));

            titulo.Colspan = 6;
            titulo.HorizontalAlignment = Element.ALIGN_CENTER;
            titulo.VerticalAlignment = Element.ALIGN_MIDDLE;
            titulo.BackgroundColor = BaseColor.LIGHT_GRAY;
            titulo.Padding = 5f;

            tabela.AddCell(titulo);

            // CABEÇALHOS
            tabela.AddCell(new PdfPCell(new Phrase("Descrição", fonteCabecalho)));
            tabela.AddCell(new PdfPCell(new Phrase("Qtd", fonteCabecalho)));
            tabela.AddCell(new PdfPCell(new Phrase("Crachá", fonteCabecalho)));
            tabela.AddCell(new PdfPCell(new Phrase("Nome", fonteCabecalho)));
            tabela.AddCell(new PdfPCell(new Phrase("Início", fonteCabecalho)));
            tabela.AddCell(new PdfPCell(new Phrase("Término", fonteCabecalho)));

            bool possuiRegistros = false;

            string sql = @"
            SELECT descricao,
                   quant,
                   cracha,
                   nome,
                   inicio,
                   termino
            FROM tbos_pecas
            WHERE id_os = @id
              AND tipo = @tipo
            ORDER BY inicio";

            using (SqlConnection conn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", idOS);
                cmd.Parameters.AddWithValue("@tipo", tipo);

                conn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        possuiRegistros = true;

                        tabela.AddCell(new PdfPCell(
                            new Phrase(dr["descricao"].ToString(), normal8)));

                        tabela.AddCell(new PdfPCell(
                            new Phrase(dr["quant"].ToString(), normal8)));

                        tabela.AddCell(new PdfPCell(
                            new Phrase(dr["cracha"].ToString(), normal8)));

                        tabela.AddCell(new PdfPCell(
                            new Phrase(dr["nome"].ToString(), normal8)));

                        tabela.AddCell(new PdfPCell(
                            new Phrase(
                                dr["inicio"] == DBNull.Value
                                    ? ""
                                    : Convert.ToDateTime(dr["inicio"])
                                        .ToString("dd/MM/yyyy HH:mm"),
                                normal8)));

                        tabela.AddCell(new PdfPCell(
                            new Phrase(
                                dr["termino"] == DBNull.Value
                                    ? ""
                                    : Convert.ToDateTime(dr["termino"])
                                        .ToString("dd/MM/yyyy HH:mm"),
                                normal8)));
                    }
                }
            }

            if (possuiRegistros)
            {                
                doc.Add(tabela);
                doc.Add(new Paragraph(" "));
            }
        }
        private void AdicionarSecao(
        Document doc,
        string titulo,
        string conteudo,
        Font fonteTitulo,
        Font fonteConteudo)
        {
            if (string.IsNullOrWhiteSpace(conteudo))
                return;

            PdfPTable tabela = new PdfPTable(1);
            tabela.WidthPercentage = 100;
            tabela.SpacingBefore = 3f;
            tabela.SpacingAfter = 5f;

            PdfPCell cabecalho = new PdfPCell(
                new Phrase(titulo, fonteTitulo));

            cabecalho.BackgroundColor = BaseColor.LIGHT_GRAY;
            cabecalho.HorizontalAlignment = Element.ALIGN_CENTER;
            cabecalho.Padding = 4f;

            tabela.AddCell(cabecalho);

            PdfPCell corpo = new PdfPCell(
                new Phrase(conteudo, fonteConteudo));

            corpo.Padding = 5f;
            corpo.MinimumHeight = 30f;

            tabela.AddCell(corpo);

            doc.Add(tabela);
        }
        private void AdicionarPecasPorTipo(
        Document doc,
        string idOS,
        string tipo,
        Font subtitulo,
        Font fonteCabecalho,
        Font normal8)
        {
            PdfPTable tabela = new PdfPTable(6);
            tabela.WidthPercentage = 100;
            tabela.SetWidths(new float[] { 35, 10, 15, 20, 20, 20 });

            tabela.AddCell(new PdfPCell(new Phrase("Descrição", fonteCabecalho)));
            tabela.AddCell(new PdfPCell(new Phrase("Qtd", fonteCabecalho)));
            tabela.AddCell(new PdfPCell(new Phrase("Crachá", fonteCabecalho)));
            tabela.AddCell(new PdfPCell(new Phrase("Nome", fonteCabecalho)));
            tabela.AddCell(new PdfPCell(new Phrase("Início", fonteCabecalho)));
            tabela.AddCell(new PdfPCell(new Phrase("Término", fonteCabecalho)));

            bool encontrou = false;

            string sql = @"
            SELECT descricao,
                   quant,
                   cracha,
                   nome,
                   inicio,
                   termino
            FROM tbos_pecas
            WHERE id_os = @id
              AND tipo = @tipo";

            using (SqlConnection conn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", idOS);
                cmd.Parameters.AddWithValue("@tipo", tipo);

                conn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        encontrou = true;

                        tabela.AddCell(new Phrase(dr["descricao"].ToString(), normal8));
                        tabela.AddCell(new Phrase(dr["quant"].ToString(), normal8));
                        tabela.AddCell(new Phrase(dr["cracha"].ToString(), normal8));
                        tabela.AddCell(new Phrase(dr["nome"].ToString(), normal8));

                        tabela.AddCell(new Phrase(
                            dr["inicio"] == DBNull.Value
                            ? ""
                            : Convert.ToDateTime(dr["inicio"])
                                .ToString("dd/MM/yyyy HH:mm"),
                            normal8));

                        tabela.AddCell(new Phrase(
                            dr["termino"] == DBNull.Value
                            ? ""
                            : Convert.ToDateTime(dr["termino"])
                                .ToString("dd/MM/yyyy HH:mm"),
                            normal8));
                    }
                }
            }

            if (encontrou)
            {
                doc.Add(new Paragraph(
                    "PEÇAS UTILIZADAS",
                    subtitulo));

                doc.Add(tabela);
            }
        }


        //private void GerarPDFOS(string idOS)
        //{
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        //Document doc = new Document(PageSize.A4, 20, 20, 20, 20);
        //        //PdfWriter writer = PdfWriter.GetInstance(doc, ms);

        //        //doc.Open();
        //        Document doc = new Document(PageSize.A4, 20, 20, 80, 60);

        //        //MemoryStream ms = new MemoryStream();

        //        PdfWriter writer = PdfWriter.GetInstance(doc, ms);

        //        // Adiciona QR Code e número da página no rodapé
        //        writer.PageEvent = new RodapeComQRCode("OS:" + idOS);

        //        doc.Open();

        //        Font titulo = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14);
        //        Font subtitulo = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 11);
        //        Font normal = FontFactory.GetFont(FontFactory.HELVETICA, 10);
        //        Font normal8 = FontFactory.GetFont(FontFactory.HELVETICA, 8);
        //        Font fonteCabecalho = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 8);

        //        #region CABEÇALHO

        //        PdfPTable cabecalho = new PdfPTable(3);
        //        cabecalho.WidthPercentage = 100;
        //        cabecalho.SetWidths(new float[] { 20, 60, 20 });

        //        // LOGO
        //        PdfPCell celLogo;

        //        string caminhoLogo = Server.MapPath("~/img/logo_transnovag.png");

        //        if (File.Exists(caminhoLogo))
        //        {
        //            iTextSharp.text.Image logo =
        //                iTextSharp.text.Image.GetInstance(caminhoLogo);

        //            logo.ScaleToFit(60f, 60f);

        //            celLogo = new PdfPCell(logo);
        //        }
        //        else
        //        {
        //            celLogo = new PdfPCell(new Phrase(""));
        //        }

        //        celLogo.Border = Rectangle.NO_BORDER;
        //        celLogo.HorizontalAlignment = Element.ALIGN_LEFT;

        //        // EMPRESA  
        //        Paragraph pEmpresa = new Paragraph();
        //        pEmpresa.Alignment = Element.ALIGN_CENTER;

        //        pEmpresa.Add(new Chunk("TRANSNOVAG TRANSPORTES\n", titulo));
        //        pEmpresa.Add(new Chunk("Ordem de Serviço", subtitulo));

        //        PdfPCell celEmpresa = new PdfPCell();
        //        celEmpresa.AddElement(pEmpresa);

        //        celEmpresa.Border = Rectangle.NO_BORDER;
        //        celEmpresa.HorizontalAlignment = Element.ALIGN_CENTER;
        //        celEmpresa.VerticalAlignment = Element.ALIGN_MIDDLE;

        //        // CÓDIGO DE BARRAS
        //        Barcode128 barcode = new Barcode128();
        //        barcode.Code = idOS;

        //        iTextSharp.text.Image imgBarCode =
        //            barcode.CreateImageWithBarcode(
        //                writer.DirectContent,
        //                null,
        //                null);

        //        imgBarCode.ScalePercent(80);

        //        PdfPCell celCodigo = new PdfPCell(imgBarCode);
        //        celCodigo.Border = Rectangle.NO_BORDER;
        //        celCodigo.HorizontalAlignment = Element.ALIGN_RIGHT;

        //        cabecalho.AddCell(celLogo);
        //        cabecalho.AddCell(celEmpresa);
        //        cabecalho.AddCell(celCodigo);

        //        doc.Add(cabecalho);

        //        #endregion

        //        string sql = @"SELECT 
        //        o.id_os,
        //        o.placa,
        //        o.tipo_veiculo, 
        //        o.ano_modelo,
        //        o.marca,
        //        o.modelo,
        //        o.parte_mecanica,
        //        o.parte_eletrica,
        //        o.parte_borracharia,
        //        o.parte_funilaria,
        //        o.servico_executado_mecanica,
        //        o.servico_executado_eletrica,
        //        o.servico_executado_borracharia,
        //        o.servico_executado_funilaria,
        //        o.resp_abertura,
        //        o.km_abertura,
        //        o.nucleo_veiculo,
        //        o.transp_motorista,
        //        o.nome_fornecedor,
        //        CASE 
        //            WHEN o.tipo_veiculo = 'CARRETA'
        //                THEN o.id_carreta
        //            ELSE o.id_veiculo
        //        END AS codigo_veiculo,
        //        CASE 
        //            WHEN o.interno_externo = 'I' THEN 'Interno'
        //            WHEN o.interno_externo = 'E' THEN 'Externo'    
        //        END AS servico_interno_externo,
        //        CASE 
        //            WHEN o.tipo_os = 'C' THEN 'Corretiva'
        //            WHEN o.tipo_os = 'P' THEN 'Preventiva'    
        //        END AS os_preventiva_corretiva,
        //        o.nucleo_veiculo,
        //        o.nome_motorista,
        //        o.tipo_os,
        //        o.data_abertura,                
        //        DATEDIFF(DAY, o.data_abertura,
        //            CASE 
        //                WHEN o.data_fechamento IS NOT NULL THEN o.data_fechamento
        //                ELSE GETDATE()
        //            END
        //        ) AS dias_aberta,
        //        CASE 
        //            WHEN o.status = '1' THEN 'Aberta'
        //            WHEN o.status = '2' THEN 'Finalizada'
        //            WHEN o.status = '3' THEN 'Cancelada'
        //        END AS status_texto
        //        FROM tbordem_servico o
        //        WHERE o.id_os = @id";

        //        using (SqlConnection conn = new SqlConnection(
        //            ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
        //        {
        //            SqlCommand cmd = new SqlCommand(sql, conn);
        //            cmd.Parameters.AddWithValue("@id", idOS);

        //            conn.Open();

        //            using (SqlDataReader dr = cmd.ExecuteReader())
        //            {
        //                if (dr.Read())
        //                {
        //                    //doc.Add(new Paragraph("Nº O.S.: " + dr["id_os"], normal));
        //                    //doc.Add(new Paragraph("Placa: " + dr["placa"], normal));
        //                    doc.Add(new Paragraph(
        //                            "Veic./Placa: " + dr["codigo_veiculo"] + " - " + dr["placa"] +
        //                            "   |   Tipo: " + dr["tipo_veiculo"] +
        //                            "   |   Ano: " + dr["ano_modelo"] +
        //                            "   |   Marca: " + dr["marca"] +
        //                            "   |   Núcleo: " + dr["nucleo_veiculo"],
        //                            normal));
        //                    if (dr["data_abertura"] != DBNull.Value)
        //                    {
        //                        doc.Add(new Paragraph(
        //                            "Data Abertura: " +
        //                            Convert.ToDateTime(dr["data_abertura"])
        //                            .ToString("dd/MM/yyyy HH:mm") +
        //                            "   |   Status: " + dr["status_texto"] +
        //                            "   |   Aberta por: " + dr["resp_abertura"] +
        //                            "   |   Km: " + dr["km_abertura"],
        //                            normal));
        //                    }
        //                    doc.Add(new Paragraph("Motorista: " + dr["nome_motorista"] +
        //                            "   |   Transportadora: " + dr["transp_motorista"], normal));
        //                    doc.Add(new Paragraph("Tipo de O.S.: " + dr["os_preventiva_corretiva"] +
        //                            "   |   Serviço: " + dr["servico_interno_externo"] +
        //                            "   |   Fornecedor: " + dr["nome_fornecedor"], normal));
        //                    //doc.Add(new Paragraph(" "));
        //                    doc.Add(new Paragraph("DEFEITOS MECÂNICOS", subtitulo));
        //                    doc.Add(new Paragraph(
        //                        dr["parte_mecanica"].ToString(),
        //                        normal8));

        //                    //doc.Add(new Paragraph(" "));

        //                    doc.Add(new Paragraph("SERVIÇOS EXECUTADOS", subtitulo));
        //                    doc.Add(new Paragraph(
        //                        dr["servico_executado_mecanica"].ToString(),
        //                        normal8));
        //                }
        //            }
        //        }

        //        //doc.Add(new Paragraph(" "));

        //        #region PEÇAS

        //        PdfPTable tabela = new PdfPTable(6);
        //        tabela.WidthPercentage = 100;

        //        tabela.SetWidths(new float[] { 35, 10, 15, 20, 20, 20 });

        //        tabela.AddCell(new PdfPCell(new Phrase("Descrição", fonteCabecalho)));
        //        tabela.AddCell(new PdfPCell(new Phrase("Qtd", fonteCabecalho)));
        //        tabela.AddCell(new PdfPCell(new Phrase("Crachá", fonteCabecalho)));
        //        tabela.AddCell(new PdfPCell(new Phrase("Nome", fonteCabecalho)));
        //        tabela.AddCell(new PdfPCell(new Phrase("Início", fonteCabecalho)));
        //        tabela.AddCell(new PdfPCell(new Phrase("Término", fonteCabecalho)));

        //        string sqlPecas = @"
        //        SELECT descricao,
        //               quant,
        //               cracha,
        //               nome,
        //               inicio,
        //               termino
        //        FROM tbos_pecas
        //        WHERE id_os = @id";

        //        using (SqlConnection conn = new SqlConnection(
        //            ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
        //        {
        //            SqlCommand cmd = new SqlCommand(sqlPecas, conn);
        //            cmd.Parameters.AddWithValue("@id", idOS);

        //            conn.Open();

        //            using (SqlDataReader dr = cmd.ExecuteReader())
        //            {
        //                while (dr.Read())
        //                {
        //                    tabela.AddCell(new PdfPCell(new Phrase(dr["descricao"].ToString(), normal8)));
        //                    tabela.AddCell(new PdfPCell(new Phrase(dr["quant"].ToString(), normal8)));
        //                    tabela.AddCell(new PdfPCell(new Phrase(dr["cracha"].ToString(), normal8)));
        //                    tabela.AddCell(new PdfPCell(new Phrase(dr["nome"].ToString(), normal8)));

        //                    tabela.AddCell(new PdfPCell(new Phrase(
        //                        dr["inicio"] == DBNull.Value
        //                            ? ""
        //                            : Convert.ToDateTime(dr["inicio"]).ToString("dd/MM/yyyy HH:mm"),
        //                        normal8)));

        //                    tabela.AddCell(new PdfPCell(new Phrase(
        //                        dr["termino"] == DBNull.Value
        //                            ? ""
        //                            : Convert.ToDateTime(dr["termino"]).ToString("dd/MM/yyyy HH:mm"),
        //                        normal8)));
        //                }
        //            }
        //        }

        //        doc.Add(new Paragraph("PEÇAS UTILIZADAS", subtitulo));
        //        doc.Add(tabela);

        //        #endregion


        //        #region ELETRICA

        //        PdfPTable tabelaEletrica = new PdfPTable(6);
        //        tabelaEletrica.WidthPercentage = 100;

        //        tabelaEletrica.SetWidths(new float[] { 35, 10, 15, 20, 20, 20 });

        //        tabelaEletrica.AddCell(new PdfPCell(new Phrase("Descrição", fonteCabecalho)));
        //        tabelaEletrica.AddCell(new PdfPCell(new Phrase("Qtd", fonteCabecalho)));
        //        tabelaEletrica.AddCell(new PdfPCell(new Phrase("Crachá", fonteCabecalho)));
        //        tabelaEletrica.AddCell(new PdfPCell(new Phrase("Nome", fonteCabecalho)));
        //        tabelaEletrica.AddCell(new PdfPCell(new Phrase("Início", fonteCabecalho)));
        //        tabelaEletrica.AddCell(new PdfPCell(new Phrase("Término", fonteCabecalho)));

        //        string sqlPecasEletrica = @"
        //        SELECT descricao,
        //               quant,
        //               cracha,
        //               nome,
        //               inicio,
        //               termino
        //        FROM tbos_pecas
        //        WHERE id_os = @id";

        //        using (SqlConnection conn = new SqlConnection(
        //            ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
        //        {
        //            SqlCommand cmdEletrica = new SqlCommand(sqlPecasEletrica, conn);
        //            cmdEletrica.Parameters.AddWithValue("@id", idOS);

        //            conn.Open();

        //            using (SqlDataReader drEletrica = cmdEletrica.ExecuteReader())
        //            {
        //                while (drEletrica.Read())
        //                {
        //                    tabelaEletrica.AddCell(new PdfPCell(new Phrase(drEletrica["descricao"].ToString(), normal8)));
        //                    tabelaEletrica.AddCell(new PdfPCell(new Phrase(drEletrica["quant"].ToString(), normal8)));
        //                    tabelaEletrica.AddCell(new PdfPCell(new Phrase(drEletrica["cracha"].ToString(), normal8)));
        //                    tabelaEletrica.AddCell(new PdfPCell(new Phrase(drEletrica["nome"].ToString(), normal8)));

        //                    tabelaEletrica.AddCell(new PdfPCell(new Phrase(
        //                        drEletrica["inicio"] == DBNull.Value
        //                            ? ""
        //                            : Convert.ToDateTime(drEletrica["inicio"]).ToString("dd/MM/yyyy HH:mm"),
        //                        normal8)));

        //                    tabelaEletrica.AddCell(new PdfPCell(new Phrase(
        //                        drEletrica["termino"] == DBNull.Value
        //                            ? ""
        //                            : Convert.ToDateTime(drEletrica["termino"]).ToString("dd/MM/yyyy HH:mm"),
        //                        normal8)));
        //                }
        //            }
        //        }

        //        doc.Add(new Paragraph("PEÇAS UTILIZADAS", subtitulo));
        //        doc.Add(tabelaEletrica);

        //        #endregion

        //        doc.Add(new Paragraph(" "));
        //        doc.Add(new Paragraph(" "));

        //        //#region QR CODE

        //        //string textoQr = "OS:" + idOS;

        //        //BarcodeQRCode qr =
        //        //    new BarcodeQRCode(textoQr, 120, 120, null);

        //        //iTextSharp.text.Image imgQr = qr.GetImage();

        //        //imgQr.Alignment = Element.ALIGN_RIGHT;

        //        //doc.Add(imgQr);

        //        //#endregion

        //        doc.Close();

        //        Response.Clear();
        //        Response.ContentType = "application/pdf";
        //        Response.AddHeader(
        //            "Content-Disposition",
        //            $"inline; filename=OS_{idOS}.pdf");

        //        Response.BinaryWrite(ms.ToArray());
        //        Response.Flush();
        //        Response.End();
        //    }
        //}
    }

}
