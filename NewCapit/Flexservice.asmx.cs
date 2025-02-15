using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Web.Script.Services;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using System.Xml.Linq;
using System.Data;

namespace NewCapit
{
    /// <summary>
    /// Summary description for Flexservice
    /// </summary>
   // [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    //[System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class Flexservice : System.Web.Services.WebService
    {

        public string data1, data2, placa;
        public string codlogin, mes;

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true, ResponseFormat = ResponseFormat.Xml)]
        public XmlDocument GetData2()
        {


            int page = 1;
            if (HttpContext.Current.Request.Form["page"] != null)
            {
                page = int.Parse(HttpContext.Current.Request.Form["page"].ToString());
            }
            int rp = 1;
            if (HttpContext.Current.Request.Form["rp"] != null)
            {
                rp = int.Parse(HttpContext.Current.Request.Form["rp"].ToString());
            }


            string dt1 = HttpContext.Current.Request.QueryString["data1"].ToString();
            string hr1 = HttpContext.Current.Request.QueryString["hora1"].ToString();
            string dt2 = HttpContext.Current.Request.QueryString["data2"].ToString();
            string hr2 = HttpContext.Current.Request.QueryString["hora2"].ToString();
            string placa = HttpContext.Current.Request.QueryString["placa"].ToString();

            data1 = dt1 + " " + hr1 + ".000";
            data2 = dt2 + " " + hr2 + ".000";


            /*string sortname = "Name";
            if (HttpContext.Current.Request.Form["sortname"] != null)
            {
                sortname = HttpContext.Current.Request.Form["sortname"].ToString();
            }
            string whereCondition = "";
            if (HttpContext.Current.Request.Form["qtype"] != null &&
            HttpContext.Current.Request.Form["query"] != null &&
            HttpContext.Current.Request.Form["query"].ToString() != string.Empty)
            {
                //whereCondition = (HttpContext.Current.Request.Form["qtype"].ToString(),
                // HttpContext.Current.Request.Form["query"].ToString());
            }
            string sortorder = "asc";
            if (HttpContext.Current.Request.Form["sortorder"] != null)
            {
                sortorder = HttpContext.Current.Request.Form["sortorder"].ToString();
            }
            string sortExp = sortname + " " + sortorder;
            int start = ((page - 1) * rp);
            */

            DataAcesso dataacesso = new DataAcesso();
            List<Veiculos2> data = dataacesso.GetAllVeiculos(data1, data2, placa, page);
            XDocument xmlDoc = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement("rows",
                new XElement("page", page),
                new XElement("total", dataacesso.total.ToString()),
                             data.Select(row => new XElement("row", new XAttribute("id", row.linha),
                                                              new XElement("cell", row.nr_idveiculo),
                                                              new XElement("cell", row.ds_placa),
                                                              new XElement("cell", row.fl_bloqueio),
                                                              new XElement("cell", row.ds_cidade),
                                                              new XElement("cell", row.dt_posicao),
                                                              new XElement("cell", row.nr_dist_referencia),
                                                              new XElement("cell", row.nr_gps),
                                                              new XElement("cell", row.fl_ignicao),
                                                              new XElement("cell", row.nr_jamming),
                                                              new XElement("cell", row.ds_lat),
                                                              new XElement("cell", row.ds_long),
                                                              new XElement("cell", row.nr_odometro),
                                                              new XElement("cell", row.nr_pontoreferencia),
                                                              new XElement("cell", row.ds_rua),
                                                              new XElement("cell", row.nr_tensao),
                                                              new XElement("cell", row.nr_satelite),
                                                              new XElement("cell", row.ds_uf),
                                                              new XElement("cell", row.nr_velocidade)

                                                             )
                                           )
                                )
                    );

            XmlDocument newDoc = new XmlDocument();
            newDoc.LoadXml(xmlDoc.ToString());
            return newDoc;
        }
        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true, ResponseFormat = ResponseFormat.Xml)]
        public XmlDocument GetData()
        {


            int page2 = 1;
            if (HttpContext.Current.Request.Form["page"] != null)
            {
                page2 = int.Parse(HttpContext.Current.Request.Form["page"].ToString());
            }
            int rp = 1;
            if (HttpContext.Current.Request.Form["rp"] != null)
            {
                rp = int.Parse(HttpContext.Current.Request.Form["rp"].ToString());
            }





            /*string sortname = "Name";
            if (HttpContext.Current.Request.Form["sortname"] != null)
            {
                sortname = HttpContext.Current.Request.Form["sortname"].ToString();
            }
            string whereCondition = "";
            if (HttpContext.Current.Request.Form["qtype"] != null &&
            HttpContext.Current.Request.Form["query"] != null &&
            HttpContext.Current.Request.Form["query"].ToString() != string.Empty)
            {
                //whereCondition = (HttpContext.Current.Request.Form["qtype"].ToString(),
                // HttpContext.Current.Request.Form["query"].ToString());
            }
            string sortorder = "asc";
            if (HttpContext.Current.Request.Form["sortorder"] != null)
            {
                sortorder = HttpContext.Current.Request.Form["sortorder"].ToString();
            }
            string sortExp = sortname + " " + sortorder;
            int start = ((page - 1) * rp);
            */

            DataAcesso dataacesso = new DataAcesso();
            List<Veiculos> data = dataacesso.GetAllVeiculos2(page2);
            XDocument xmlDoc = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement("rows",
                new XElement("page", page2),
                new XElement("total", dataacesso.total2.ToString()),
                             data.Select(row => new XElement("row", new XAttribute("id", row.linha),
                                                              new XElement("cell", row.nr_idveiculo),
                                                              new XElement("cell", row.ds_placa),
                                                              new XElement("cell", row.fl_bloqueio),
                                                              new XElement("cell", row.ds_cidade),
                                                              new XElement("cell", row.dt_posicao),
                                                              new XElement("cell", row.nr_dist_referencia),
                                                              new XElement("cell", row.nr_gps),
                                                              new XElement("cell", row.fl_ignicao),
                                                              new XElement("cell", row.nr_jamming),
                                                              new XElement("cell", row.ds_lat),
                                                              new XElement("cell", row.ds_long),
                                                              new XElement("cell", row.nr_odometro),
                                                              new XElement("cell", row.nr_pontoreferencia),
                                                              new XElement("cell", row.ds_rua),
                                                              new XElement("cell", row.nr_tensao),
                                                              new XElement("cell", row.nr_satelite),
                                                              new XElement("cell", row.ds_uf),
                                                              new XElement("cell", row.nr_velocidade)

                                                             )
                                           )
                                )
                    );

            XmlDocument newDoc = new XmlDocument();
            newDoc.LoadXml(xmlDoc.ToString());
            return newDoc;
        }
        [WebMethod]
        [ScriptMethod(UseHttpGet = false, XmlSerializeString = true, ResponseFormat = ResponseFormat.Xml)]
        public XmlDocument GetData3()
        {


            int page3 = 1;
            if (HttpContext.Current.Request.Form["page"] != null)
            {
                page3 = int.Parse(HttpContext.Current.Request.Form["page"].ToString());
            }
            int rp = 1;
            if (HttpContext.Current.Request.Form["rp"] != null)
            {
                rp = int.Parse(HttpContext.Current.Request.Form["rp"].ToString());
            }

            if (HttpContext.Current.Request.QueryString["codlogin"] != "Selecione o Motorista")
            {
                codlogin = HttpContext.Current.Request.QueryString["codlogin"].ToString();
            }
            else
            {
                codlogin = "0";
            }


            if (HttpContext.Current.Request.QueryString["mes"] != "Selecione o Mês")
            {
                mes = HttpContext.Current.Request.QueryString["mes"].ToString();
            }
            else
            {
                mes = DateTime.Now.ToString("MM");
            }

            // string data1 = HttpContext.Current.Request.QueryString["data1"].ToString();
            // string data2 = HttpContext.Current.Request.QueryString["data2"].ToString();


            /*string sortname = "Name";
            if (HttpContext.Current.Request.Form["sortname"] != null)
            {
                sortname = HttpContext.Current.Request.Form["sortname"].ToString();
            }
            string whereCondition = "";
            if (HttpContext.Current.Request.Form["qtype"] != null &&
            HttpContext.Current.Request.Form["query"] != null &&
            HttpContext.Current.Request.Form["query"].ToString() != string.Empty)
            {
                //whereCondition = (HttpContext.Current.Request.Form["qtype"].ToString(),
                // HttpContext.Current.Request.Form["query"].ToString());
            }
            string sortorder = "asc";
            if (HttpContext.Current.Request.Form["sortorder"] != null)
            {
                sortorder = HttpContext.Current.Request.Form["sortorder"].ToString();
            }
            string sortExp = sortname + " " + sortorder;
            int start = ((page - 1) * rp);
            */

            DataAcesso dataacesso = new DataAcesso();
            List<Jornada> data = dataacesso.GetAllMotoristas(mes, codlogin, page3);
            XDocument xmlDoc = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement("rows",
                new XElement("page", page3),
                new XElement("total", dataacesso.total3.ToString()),
                             data.Select(row => new XElement("row", new XAttribute("id", row.linha),
                                                              new XElement("cell", row.dt_jornada),
                                                              new XElement("cell", row.cod_login),
                                                              new XElement("cell", row.hr_inicio_jornada),
                                                              new XElement("cell", row.hr_inicio_intervalo),
                                                              new XElement("cell", row.hr_fim_intervalo),
                                                              new XElement("cell", row.hr_fim_jornada),
                                                              new XElement("cell", row.vl_almoco),
                                                              new XElement("cell", row.vl_janta),
                                                              new XElement("cell", row.vl_pernoite),
                                                              new XElement("cell", row.vl_premio),
                                                              new XElement("cell", row.total)

                                                             )
                                           )
                                )
                    );

            XmlDocument newDoc = new XmlDocument();
            newDoc.LoadXml(xmlDoc.ToString());
            return newDoc;
        }
    }
}
