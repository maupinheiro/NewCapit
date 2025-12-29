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

            string pastaDestino = context.Server.MapPath("../../ImportacaoTemp/");
            if (!Directory.Exists(pastaDestino))
                Directory.CreateDirectory(pastaDestino);

            for (int i = 0; i < files.Count; i++)
            {
                var file = files[i];
                string nome = Path.GetFileName(file.FileName).ToUpper();

                // trava de segurança
                if (!nome.StartsWith("SG") || !nome.EndsWith(".TXT"))
                    continue;

                file.SaveAs(Path.Combine(pastaDestino, nome));
            }

            context.Response.StatusCode = 200;
            context.Response.Write("OK");
        }

        public bool IsReusable => false;
    }
}