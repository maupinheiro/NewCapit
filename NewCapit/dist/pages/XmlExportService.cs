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

            string cnpjEmitente = "55890016000109";
            string serie = "001";
            string nCTFormatado = int.Parse(numeroDoc).ToString("D9");
            string cCT = int.Parse(numeroDoc).ToString("D8");
            string tpEmis = "1";
            string cDV = "0";
            string idChaveCte = $"352603{cnpjEmitente}57{serie}{nCTFormatado}{tpEmis}{cCT}{cDV}";

            var cte = new CTe
            {
                infCte = new infCte
                {
                    Id = "CTe" + idChaveCte,
                    ide = new ide
                    {
                        cCT = cCT,
                        nCT = numeroDoc,
                        dhEmi = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                        cMunEnv = "3550308",
                        xMunEnv = "SAO PAULO",
                        UFEnv = "SP",
                        cMunIni = "3550308",
                        xMunIni = "SAO PAULO",
                        UFIni = "SP",
                        cMunFim = "3545209",
                        xMunFim = "SALTO",
                        UFFim = "SP"
                    },
                    compl = new compl
                    {
                        xObs = $"MOT: {motorista?.Nome}-CPF: {motorista?.CPF}/PLACA: {veiculos.FirstOrDefault()?.Placa}"
                    },
                    emit = new emit
                    {
                        CNPJ = cnpjEmitente,
                        IE = "111501336118",
                        xNome = "TRANSNOVAG TRANSPORTES SA",
                        xFant = "MATRIZ",
                        enderEmit = new enderEmit
                        {
                            xLgr = "RUA CADIRIRI",
                            nro = "629",
                            xBairro = "PARQUE DA MOOCA",
                            cMun = "3550308",
                            xMun = "SAO PAULO",
                            CEP = "03109040",
                            UF = "SP"
                        }
                    },
                    dest = new dest
                    {
                        CNPJ = "00000000000000", // Preencha com o CNPJ real do destinatário se disponível
                        xNome = "CONSIGNATARIO OU DESTINATARIO DA CARGA LTDA",
                        enderDest = new enderDest
                        {
                            xLgr = "AVENIDA PRINCIPAL",
                            nro = "100",
                            xBairro = "CENTRO",
                            cMun = "3545209",
                            xMun = "SALTO",
                            CEP = "13320000",
                            UF = "SP"
                        }
                    },
                    vPrest = new vPrest { vTPrest = "3100.80", vRec = "3100.80" },
                    imp = new imp
                    {
                        ICMS = new ICMS
                        {
                            ICMS00 = new ICMS00 { CST = "00", vBC = "3100.80", pICMS = "12.00", vICMS = "372.10" }
                        }
                    },
                    infCTeNorm = new infCTeNorm
                    {
                        infCarga = new infCarga
                        {
                            vCarga = notas.Sum(x => x.ValorNf).ToString("F2", System.Globalization.CultureInfo.InvariantCulture),
                            proPred = "CHAPA",
                            infQ = new infQ { qCarga = notas.Sum(x => x.Peso).ToString("F4", System.Globalization.CultureInfo.InvariantCulture) }
                        },
                        infDoc = new infDoc
                        {
                            infNFe = notas.Select(n => new infNFe { chave = n.ChaveAcesso.Trim() }).ToList()
                        },
                        infModal = new infModal
                        {
                            rodo = new rodo { RNTRC = "00109548" }
                        }
                    }
                }
            };

            return Serializar(cte);
        }

        private string Serializar<T>(T objeto)
        {
            var settings = new XmlWriterSettings { Indent = true, Encoding = Encoding.UTF8 };
            using (var sw = new Utf8StringWriter())
            {
                using (var writer = XmlWriter.Create(sw, settings))
                {
                    var ns = new XmlSerializerNamespaces();
                    ns.Add("", "http://www.portalfiscal.inf.br/cte");
                    ns.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
                    // Removido o namespace "ds" da assinatura digital daqui
                    new XmlSerializer(typeof(T)).Serialize(writer, objeto, ns);
                }
                return sw.ToString();
            }
        }
    }

    public class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding => Encoding.UTF8;
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

    // --- MODELO DO CTE REFORMATADO (SEM TAGS DE ASSINATURA) ---
    [XmlRoot("CTe", Namespace = "http://www.portalfiscal.inf.br/cte")]
    public class CTe
    {
        // Agora possui APENAS a tag de negócio. 
        [XmlElement(Order = 0)] public infCte infCte { get; set; }
    }

    public class infCte
    {
        [XmlAttribute] public string Id;
        [XmlAttribute] public string versao = "4.00";

        [XmlElement(Order = 0)] public ide ide { get; set; }
        [XmlElement(Order = 1)] public compl compl { get; set; }
        [XmlElement(Order = 2)] public emit emit { get; set; }
        [XmlElement(Order = 3)] public dest dest { get; set; }
        [XmlElement(Order = 4)] public vPrest vPrest { get; set; }
        [XmlElement(Order = 5)] public imp imp { get; set; }
        [XmlElement(Order = 6)] public infCTeNorm infCTeNorm { get; set; }
    }

    public class ide
    {
        [XmlElement(Order = 0)] public string cUF = "35";
        [XmlElement(Order = 1)] public string cCT;
        [XmlElement(Order = 2)] public string CFOP = "5352";
        [XmlElement(Order = 3)] public string natOp = "PRESTACAO SERVICOS DE TRANSPORTES";
        [XmlElement(Order = 4)] public string mod = "57";
        [XmlElement(Order = 5)] public string serie = "1";
        [XmlElement(Order = 6)] public string nCT;
        [XmlElement(Order = 7)] public string dhEmi;
        [XmlElement(Order = 8)] public string tpImp = "1";
        [XmlElement(Order = 9)] public string tpEmis = "1";
        [XmlElement(Order = 10)] public string cDV = "0";
        [XmlElement(Order = 11)] public string tpAmb = "1";
        [XmlElement(Order = 12)] public string tpCTe = "0";
        [XmlElement(Order = 13)] public string procEmi = "0";
        [XmlElement(Order = 14)] public string verProc = "1.0";
        [XmlElement(Order = 15)] public string cMunEnv;
        [XmlElement(Order = 16)] public string xMunEnv;
        [XmlElement(Order = 17)] public string UFEnv;
        [XmlElement(Order = 18)] public string modal = "01";
        [XmlElement(Order = 19)] public string tpServ = "0";
        [XmlElement(Order = 20)] public string cMunIni;
        [XmlElement(Order = 21)] public string xMunIni;
        [XmlElement(Order = 22)] public string UFIni;
        [XmlElement(Order = 23)] public string cMunFim;
        [XmlElement(Order = 24)] public string xMunFim;
        [XmlElement(Order = 25)] public string UFFim;
        [XmlElement(Order = 26)] public string retira = "1";
        [XmlElement(Order = 27)] public string indIEToma = "1";
        [XmlElement(Order = 28)] public toma3 toma3 = new toma3();
    }

    public class toma3 { [XmlElement(Order = 0)] public string toma = "0"; }
    public class compl { [XmlElement(Order = 0)] public string xObs { get; set; } }

    public class emit
    {
        [XmlElement(Order = 0)] public string CNPJ;
        [XmlElement(Order = 1)] public string IE;
        [XmlElement(Order = 2)] public string xNome;
        [XmlElement(Order = 3)] public string xFant;
        [XmlElement(Order = 4)] public enderEmit enderEmit { get; set; }
        [XmlElement(Order = 5)] public string CRT = "3";
    }

    public class enderEmit
    {
        [XmlElement(Order = 0)] public string xLgr;
        [XmlElement(Order = 1)] public string nro;
        [XmlElement(Order = 2)] public string xBairro;
        [XmlElement(Order = 3)] public string cMun;
        [XmlElement(Order = 4)] public string xMun;
        [XmlElement(Order = 5)] public string CEP;
        [XmlElement(Order = 6)] public string UF;
    }

    public class dest
    {
        [XmlElement(Order = 0)] public string CNPJ { get; set; }
        [XmlElement(Order = 1)] public string xNome { get; set; }
        [XmlElement(Order = 2)] public enderDest enderDest { get; set; }
    }

    public class enderDest
    {
        [XmlElement(Order = 0)] public string xLgr { get; set; }
        [XmlElement(Order = 1)] public string nro { get; set; }
        [XmlElement(Order = 2)] public string xBairro { get; set; }
        [XmlElement(Order = 3)] public string cMun { get; set; }
        [XmlElement(Order = 4)] public string xMun { get; set; }
        [XmlElement(Order = 5)] public string CEP { get; set; }
        [XmlElement(Order = 6)] public string UF { get; set; }
    }

    public class vPrest
    {
        [XmlElement(Order = 0)] public string vTPrest;
        [XmlElement(Order = 1)] public string vRec;
    }

    public class imp { [XmlElement(Order = 0)] public ICMS ICMS { get; set; } }
    public class ICMS { [XmlElement(Order = 0)] public ICMS00 ICMS00 { get; set; } }
    public class ICMS00 { [XmlElement(Order = 0)] public string CST; [XmlElement(Order = 1)] public string vBC; [XmlElement(Order = 2)] public string pICMS; [XmlElement(Order = 3)] public string vICMS; }

    public class infCTeNorm
    {
        [XmlElement(Order = 0)] public infCarga infCarga { get; set; }
        [XmlElement(Order = 1)] public infDoc infDoc { get; set; }
        [XmlElement(Order = 2)] public infModal infModal { get; set; }
    }

    public class infCarga
    {
        [XmlElement(Order = 0)] public string vCarga;
        [XmlElement(Order = 1)] public string proPred;
        [XmlElement(Order = 2)] public infQ infQ { get; set; }
    }
    public class infQ
    {
        [XmlElement(Order = 0)] public string cUnid = "01";
        [XmlElement(Order = 1)] public string tpMed = "PESO";
        [XmlElement(Order = 2)] public string qCarga;
    }

    public class infDoc { [XmlElement(Order = 0)] public List<infNFe> infNFe { get; set; } }
    public class infNFe { [XmlElement(Order = 0)] public string chave { get; set; } }

    public class infModal
    {
        [XmlAttribute] public string versaoModal = "4.00";
        [XmlElement(Order = 0)] public rodo rodo { get; set; }
    }
    public class rodo { [XmlElement(Order = 0)] public string RNTRC { get; set; } }
}