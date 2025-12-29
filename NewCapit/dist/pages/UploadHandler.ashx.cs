using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace NewCapit.dist.pages
{
    /// <summary>
    /// Descrição resumida de UploadHandler
    /// </summary>
    public class UploadHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var files = context.Request.Files;

            string pastaDestino = context.Server.MapPath("~/ImportacaoTemp/");
            if (!Directory.Exists(pastaDestino))
                Directory.CreateDirectory(pastaDestino);

            foreach (string key in files)
            {
                var file = files[key];

                string nome = Path.GetFileName(file.FileName);

                // 🔎 filtro: começa com SG e termina com .txt
                if (!nome.StartsWith("SG", StringComparison.OrdinalIgnoreCase) ||
                    !nome.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
                {
                    continue; // pula o arquivo
                }

                file.SaveAs(Path.Combine(pastaDestino, nome));
            }
        }


        public bool IsReusable => false;
    }
}