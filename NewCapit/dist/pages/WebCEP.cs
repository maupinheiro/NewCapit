using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace NewCapit.dist.pages
{
    //public class WebCEP
    //{
    //    #region "Váriavies"
    //    string _uf;
    //    string _cidade;
    //    string _bairro;
    //    string _tipo_lagradouro;
    //    string _lagradouro;
    //    string _ibge;
    //    string _regiao;
    //    string _resultado;
    //    string _resultato_txt;
    //    #endregion

    //    #region "Propiedades"
    //    public string UF
    //    {
    //        get { return _uf; }
    //    }
    //    public string Cidade
    //    {
    //        get { return _cidade; }
    //    }
    //    public string IBGE
    //    {
    //        get { return _ibge; }
    //    }
    //    public string Regiao
    //    {
    //        get { return _regiao; }
    //    }

    //    public string Bairro
    //    {
    //        get { return _bairro; }
    //    }
    //    public string TipoLagradouro
    //    {
    //        get { return _tipo_lagradouro; }
    //    }
    //    public string Lagradouro
    //    {
    //        get { return _lagradouro; }
    //    }
    //    public string Resultado
    //    {
    //        get { return _resultado; }
    //    }
    //    public string ResultadoTXT
    //    {
    //        get { return _resultato_txt; }
    //    }
    //    #endregion
    //    #region "Construtor"
    //    /// <summary>  
    //    /// WebService para Busca de CEP  
    //    ///  </summary>  
    //    /// <param  name="CEP"></param>  
    //    public WebCEP(string CEP)
    //    {
    //        _uf = "";
    //        _cidade = "";
    //        _bairro = "";
    //        _ibge = "";
    //        _regiao = "";
    //        _tipo_lagradouro = "";
    //        _lagradouro = "";
    //        _resultado = "0";
    //        _resultato_txt = "CEP não encontrado";

    //        //Cria um DataSet  baseado no retorno do XML  
    //        DataSet ds = new DataSet();
    //        ds.ReadXml("http://cep.republicavirtual.com.br/web_cep.php?cep=" + CEP.Replace("-", "").Trim() + "&formato=xml");

    //        if (ds != null)
    //        {
    //            if (ds.Tables[0].Rows.Count > 0)
    //            {
    //                _resultado = ds.Tables[0].Rows[0]["resultado"].ToString();
    //                switch (_resultado)
    //                {
    //                    case "1":
    //                        _uf = ds.Tables[0].Rows[0]["uf"].ToString().Trim();
    //                        _cidade = ds.Tables[0].Rows[0]["cidade"].ToString().Trim();
    //                        _bairro = ds.Tables[0].Rows[0]["bairro"].ToString().Trim();
    //                        _ibge = ds.Tables[0].Rows[0]["ibge"].ToString().Trim();
    //                        _regiao = ds.Tables[0].Rows[0]["regiao"].ToString().Trim();
    //                        _tipo_lagradouro = ds.Tables[0].Rows[0]["tipo_logradouro"].ToString().Trim();
    //                        _lagradouro = ds.Tables[0].Rows[0]["logradouro"].ToString().Trim();
    //                        _resultato_txt = "CEP completo";
    //                        break;
    //                    case "2":
    //                        _uf = ds.Tables[0].Rows[0]["uf"].ToString().Trim();
    //                        _cidade = ds.Tables[0].Rows[0]["cidade"].ToString().Trim();
    //                        _bairro = "";
    //                        _ibge = "";
    //                        _regiao = "";
    //                        _tipo_lagradouro = "";
    //                        _lagradouro = "";
    //                        _resultato_txt = "CEP  único";
    //                        break;
    //                    default:
    //                        _uf = "";
    //                        _cidade = "";
    //                        _bairro = "";
    //                        _ibge = "";
    //                        _regiao = "";
    //                        _tipo_lagradouro = "";
    //                        _lagradouro = "";
    //                        _resultato_txt = "CEP não  encontrado";
    //                        break;
    //                        #endregion
    //                }
    //            }
    //        }
    //    }
    //}

    using System;
    using System.IO;
    using System.Net;
    using Newtonsoft.Json;

    public class WebCEP
    {
        #region Variáveis

        private string _uf;
        private string _cidade;
        private string _bairro;
        private string _tipo_lagradouro;
        private string _lagradouro;
        private string _ibge;
        private string _regiao;
        private string _resultado;
        private string _resultato_txt;

        #endregion

        #region Propriedades

        public string UF => _uf;
        public string Cidade => _cidade;
        public string Bairro => _bairro;
        public string TipoLagradouro => _tipo_lagradouro;
        public string Lagradouro => _lagradouro;
        public string IBGE => _ibge;
        public string Regiao => _regiao;
        public string Resultado => _resultado;
        public string ResultadoTXT => _resultato_txt;

        #endregion

        #region Construtor

        public WebCEP(string cep)
        {
            _uf = "";
            _cidade = "";
            _bairro = "";
            _tipo_lagradouro = "";
            _lagradouro = "";
            _ibge = "";
            _regiao = "";
            _resultado = "0";
            _resultato_txt = "CEP não encontrado";

            try
            {
                cep = cep.Replace("-", "").Trim();

                var request = (HttpWebRequest)WebRequest.Create($"https://viacep.com.br/ws/{cep}/json/");
                request.Method = "GET";

                using (var response = (HttpWebResponse)request.GetResponse())
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    string json = reader.ReadToEnd();

                    ViaCepDTO dados = JsonConvert.DeserializeObject<ViaCepDTO>(json);

                    if (dados != null && dados.erro != true)
                    {
                        _uf = dados.uf ?? "";
                        _cidade = dados.localidade ?? "";
                        _bairro = dados.bairro ?? "";
                        _lagradouro = dados.logradouro ?? "";
                        _tipo_lagradouro = ObterTipoLogradouro(dados.logradouro);
                        _ibge = dados.ibge ?? "";
                        _regiao = ObterRegiao(_uf);

                        _resultado = "1";
                        _resultato_txt = "CEP completo";
                    }
                }
            }
            catch
            {
                _resultado = "0";
                _resultato_txt = "Erro ao consultar o CEP";
            }
        }

        #endregion

        #region Métodos

        private string ObterTipoLogradouro(string logradouro)
        {
            if (string.IsNullOrWhiteSpace(logradouro))
                return "";

            string[] partes = logradouro.Split(' ');

            if (partes.Length > 0)
                return partes[0];

            return "";
        }

        private string ObterRegiao(string uf)
        {
            switch (uf.ToUpper())
            {
                case "AC":
                case "AM":
                case "AP":
                case "PA":
                case "RO":
                case "RR":
                case "TO":
                    return "NORTE";

                case "AL":
                case "BA":
                case "CE":
                case "MA":
                case "PB":
                case "PE":
                case "PI":
                case "RN":
                case "SE":
                    return "NORDESTE";

                case "DF":
                case "GO":
                case "MT":
                case "MS":
                    return "CENTRO-OESTE";

                case "ES":
                case "MG":
                case "RJ":
                case "SP":
                    return "SUDESTE";

                case "PR":
                case "RS":
                case "SC":
                    return "SUL";

                default:
                    return "";
            }
        }

        #endregion
    }

    public class ViaCepDTO
    {
        public string cep { get; set; }
        public string logradouro { get; set; }
        public string complemento { get; set; }
        public string bairro { get; set; }
        public string localidade { get; set; }
        public string uf { get; set; }
        public string ibge { get; set; }
        public string gia { get; set; }
        public string ddd { get; set; }
        public string siafi { get; set; }
        public bool erro { get; set; }
    }
}