using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NewCapit
{
    public partial class Frm_CadMotoristas : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DateTime dataHoraAtual = DateTime.Now;
            txtDtCad.Text = dataHoraAtual.ToString("dd/MM/yyyy");
        }
    }
}