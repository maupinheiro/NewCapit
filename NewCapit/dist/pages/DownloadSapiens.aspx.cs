using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NewCapit.dist.pages
{
    public partial class DownloadSapiens : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["idCarga"] != null && Request.QueryString["numDoc"] != null && Request.QueryString["serv"] != null)
            {
                int idCarga = int.Parse(Request.QueryString["idCarga"]);
                string numeroDocumento = Request.QueryString["numDoc"];
                bool ehServico = bool.Parse(Request.QueryString["serv"]);

                // Pega o formato (txt ou xml). Se não for enviado, assume 'txt' por padrão
                string formato = Request.QueryString["formato"]?.ToLower() ?? "txt";

                try
                {
                    if (formato == "xml")
                    {
                        var srvXml = new XmlExportService();
                        string conteudoXml = ehServico ?
                            srvXml.GerarXmlNfse(idCarga, numeroDocumento) :
                            srvXml.GerarXmlCte(idCarga, numeroDocumento);

                        if (!string.IsNullOrEmpty(conteudoXml))
                        {
                            string nomeXml = $"{(ehServico ? "NFSe" : "CTe")}_{numeroDocumento}.xml";
                            // Força contentType para text/xml e codificação correta
                            DispararDownloadBruto(conteudoXml, nomeXml, "text/xml");
                        }
                    }
                    else // Padrão TXT do Sapiens
                    {
                        var srv = new SapiensIntegrationService();
                        string conteudoArquivo = srv.GerarArquivoCompleto(idCarga, numeroDocumento, ehServico);

                        if (!string.IsNullOrEmpty(conteudoArquivo))
                        {
                            string prefixo = ehServico ? "NFSe" : "CTe";
                            string nomeFinal = $"Sapiens_{prefixo}_{numeroDocumento}.txt";

                            DispararDownloadBruto(conteudoArquivo, nomeFinal, "application/octet-stream");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Response.Write("Erro ao gerar o ficheiro: " + ex.Message);
                }
            }
        }

        private void DispararDownloadBruto(string texto, string nomeArquivo, string contentType)
        {
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();

            Response.ContentType = contentType;
            Response.AddHeader("Content-Disposition", "attachment; filename=" + nomeArquivo);

            byte[] dados;
            // XML precisa obrigatoriamente ser UTF-8 por conta dos caracteres especiais e acentos do modelo nacional
            if (contentType.Contains("xml"))
            {
                dados = Encoding.UTF8.GetBytes(texto);
            }
            else
            {
                // Mantém o encoding padrão do TXT do Sapiens
                dados = Encoding.GetEncoding("ISO-8859-1").GetBytes(texto);
            }

            Response.AddHeader("Content-Length", dados.Length.ToString());
            Response.BinaryWrite(dados);
            Response.Flush();

            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
    }
}