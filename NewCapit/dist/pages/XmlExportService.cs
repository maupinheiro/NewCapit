using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace NewCapit
{
    public class XmlExportService
    {
        // MÉTODO RESTAURADO: GerarXmlNfse
        public string GerarXmlNfse(int idCarga, string numeroDoc)
        {
            var serviceSapiens = new dist.pages.SapiensIntegrationService();
            var notas = serviceSapiens.ObterNotasDaCarga(idCarga);
            var motorista = serviceSapiens.ObterMotoristaDaCarga(idCarga);

            var retorno = new RetornoConsultaXml
            {
                NFe = new NFeXml
                {
                    NumeroNFe = numeroDoc,
                    DataEmissaoNFe = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"),
                    CPFCNPJPrestador = new Identificacao { CNPJ = "55890016000109" },
                    RazaoSocialPrestador = "TRANSNOVAG TRANSPORTES SA",
                    ValorServicos = "278.88",
                    Discriminacao = $"PRESTACAO DE SERVICO - NF: {string.Join(", ", notas.Select(x => x.NumeroNfe))} - MOT: {motorista?.Nome} - CPF: {motorista?.CPF}"
                }
            };
            return Serializar(retorno);
        }

        public string GerarXmlCte(int idCarga, string numeroDoc)
        {
            var serviceSapiens = new dist.pages.SapiensIntegrationService();
            var notas = serviceSapiens.ObterNotasDaCarga(idCarga);
            var motorista = serviceSapiens.ObterMotoristaDaCarga(idCarga);
            var veiculos = serviceSapiens.ObterVeiculosDaCarga(idCarga);

            var proc = new cteProc
            {
                CTe = new CTe
                {
                    infCte = new infCte
                    {
                        Id = "CTe3526035589001600010957001000" + numeroDoc.PadLeft(9, '0') + "1",
                        ide = new ide { nCT = numeroDoc, cCT = numeroDoc.PadLeft(8, '0'), dhEmi = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz") },
                        emit = new emit { CNPJ = "55890016000109", xNome = "TRANSNOVAG TRANSPORTES SA", IE = "111501336118" },
                        vPrest = new vPrest { vTPrest = "3100.80", vRec = "3100.80" },
                        infCTeNorm = new infCTeNorm
                        {
                            infDoc = new infDoc
                            {
                                infNFe = notas.Select(n => new infNFe { chave = n.ChaveAcesso }).ToList()
                            },
                            infModal = new infModal
                            {
                                rodo = new rodo { RNTRC = "00109548" }
                            }
                        },
                        compl = new compl
                        {
                            xObs = $"MOT: {motorista?.Nome}-CPF: {motorista?.CPF}/PLACA: {veiculos.FirstOrDefault()?.Placa}"
                        }
                    }
                }
            };
            return Serializar(proc);
        }

        private string Serializar<T>(T objeto)
        {
            var settings = new XmlWriterSettings { Indent = true, Encoding = Encoding.UTF8 };
            using (var sw = new StringWriter())
            {
                using (var writer = XmlWriter.Create(sw, settings))
                {
                    var ns = new XmlSerializerNamespaces();
                    // Define o namespace padrão conforme o modelo oficial
                    ns.Add("", "http://www.portalfiscal.inf.br/cte");
                    ns.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
                    new XmlSerializer(typeof(T)).Serialize(writer, objeto, ns);
                }
                return sw.ToString();
            }
        }
    }

    // --- MODELOS PARA NFSE ---
    [XmlRoot("RetornoConsulta")]
    public class RetornoConsultaXml { public NFeXml NFe { get; set; } }
    public class NFeXml
    {
        public Identificacao CPFCNPJPrestador { get; set; }
        public string RazaoSocialPrestador, NumeroNFe, DataEmissaoNFe, ValorServicos, Discriminacao;
    }
    public class Identificacao { public string CNPJ; }

    // --- MODELOS PARA CTE (PADRÃO OFICIAL) ---
    [XmlRoot("cteProc", Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class cteProc
    {
        [XmlAttribute] public string versao = "4.00";
        public CTe CTe { get; set; }
    }
    public class CTe { public infCte infCte { get; set; } }
    public class infCte
    {
        [XmlAttribute] public string Id;
        [XmlAttribute] public string versao = "4.00";
        public ide ide { get; set; }
        public compl compl { get; set; }
        public emit emit { get; set; }
        public vPrest vPrest { get; set; }
        public infCTeNorm infCTeNorm { get; set; }
    }
    public class ide { public string cUF = "35", cCT, nCT, dhEmi, mod = "57", serie = "1", tpImp = "1", tpEmis = "1", tpAmb = "1", tpCTe = "0", procEmi = "0", verProc = "1.0", modal = "01", tpServ = "0"; }
    public class compl { public string xObs { get; set; } }
    public class emit { public string CNPJ, IE, xNome; }
    public class vPrest { public string vTPrest, vRec; }
    public class infCTeNorm { public infDoc infDoc { get; set; } public infModal infModal { get; set; } }
    public class infDoc { public List<infNFe> infNFe { get; set; } }
    public class infNFe { public string chave { get; set; } }
    public class infModal { [XmlAttribute] public string versaoModal = "4.00"; public rodo rodo { get; set; } }
    public class rodo { public string RNTRC { get; set; } }
}