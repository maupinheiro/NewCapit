using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using static NewCapit.dist.pages.Frm_AtualizaColetaMatriz;

namespace NewCapit
{
    public class MeuDanfeService
    {
        private readonly string token =
            ConfigurationManager.AppSettings["MeuDanfeToken"];

        public async Task<NFeResponse> ConsultarNFe(string chave)
        {
            string url = $"https://api.meudanfe.com.br/v1/nfe/{chave}";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                    throw new Exception("Erro ao consultar NF-e");

                string json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<NFeResponse>(json);
            }
        }
    }
}