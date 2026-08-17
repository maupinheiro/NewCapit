using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static NewCapit.dist.pages.Frm_AtualizaColetaMatriz;
using System.Web.UI.WebControls;

namespace NewCapit.dist.pages
{
    public class CVAArquivo
    {
        public List<CVAProduto> Produtos { get; set; } = new List<CVAProduto>();

        public List<CVAEmbalagem> Embalagens { get; set; } = new List<CVAEmbalagem>();

        public List<CVAQuantidade> Quantidades { get; set; } = new List<CVAQuantidade>();
    }






}
   