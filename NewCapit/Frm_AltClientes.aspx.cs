using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls.WebParts;


namespace NewCapit
{
    public partial class Frm_AltClientes : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conexao"].ToString());
        string id;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string id = Request.QueryString["codcli"];
                if (!string.IsNullOrEmpty(id))
                {
                    // Use o ID para carregar os detalhes
                    txtCodCli.Text = id;
                }
            }
        }



    }
    
}