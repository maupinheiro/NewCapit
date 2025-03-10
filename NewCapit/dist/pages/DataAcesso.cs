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
using System.Xml.Linq;
using System.Data;
using System.Web.UI.WebControls;


namespace NewCapit.dist.pages
{
    public class DataAcesso
    {
        public int page;
        public int total;
        public int page2;
        public int total2;
        public int page3;
        public int total3;
        string data2;
      

        public List<Veiculos2> GetAllVeiculos(string data1, string data2, string placa,int page)
        {
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString());
            List<Veiculos2> lst2 = new List<Veiculos2>();

            SqlCommand cmd = new SqlCommand("sp_grid_historico", con);
            

            //  SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@dt_posicao1", data1);
            cmd.Parameters.AddWithValue("@dt_posicao2",data2);
            cmd.Parameters.AddWithValue("@ds_placa", placa);
            cmd.Parameters.AddWithValue("@PageNumber", page);
            cmd.Parameters.AddWithValue("@RowspPage", 50);
          
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            con.Open();
            SqlDataReader dataReader = cmd.ExecuteReader();




            while (dataReader.Read())
            {

                Veiculos2 e1 = new Veiculos2();
                e1.linha = Convert.ToInt32(dataReader["NUMBER"]);
                e1.nr_idveiculo = Convert.ToInt32(dataReader["nr_idveiculo"]);
                e1.ds_placa = dataReader["ds_placa"].ToString();
                e1.fl_bloqueio = Convert.ToInt32(dataReader["fl_bloqueio"]);
                e1.ds_cidade = dataReader["ds_cidade"].ToString();
                e1.dt_posicao = dataReader["dt_posicao"].ToString();
                e1.nr_dist_referencia = Convert.ToInt32(dataReader["nr_dist_referencia"]);
                e1.nr_gps = Convert.ToInt32(dataReader["nr_gps"]);
                e1.fl_ignicao = Convert.ToInt32(dataReader["fl_ignicao"]);
                e1.nr_jamming = Convert.ToInt32(dataReader["nr_jamming"]);
                e1.ds_lat = dataReader["ds_lat"].ToString();
                e1.ds_long = dataReader["ds_long"].ToString();
                e1.nr_odometro = Convert.ToInt32(dataReader["nr_odometro"]);
                e1.nr_pontoreferencia = dataReader["nr_pontoreferencia"].ToString();
                e1.ds_rua = dataReader["ds_rua"].ToString();
                e1.nr_tensao = Convert.ToInt32(dataReader["nr_tensao"]);
                e1.nr_satelite = Convert.ToInt32(dataReader["nr_satelite"]);
                e1.ds_uf = dataReader["ds_uf"].ToString();
                e1.nr_velocidade = Convert.ToInt32(dataReader["nr_velocidade"]);
                lst2.Add(e1);

                
            }

            

            con.Close();

            //string dataini = "08/07/2015 10:00:00.000";
            //string datafim = "14/07/2015 23:59:59.000";
            //string placa1 = "DBC1530";
            string sql2 = "SELECT COUNT(*) ";
            sql2 += "FROM  tb_transmissao_historico as t inner join tb_veiculo_sascar as v on t.nr_idveiculo=v.nr_idveiculo ";
            sql2 += "where v.ds_placa='" + placa + "' and t.dt_posicao between '" + data1 + "' and '" + data2 + "'";
           
            // sql2 += "where v.ds_placa='DBC1530' and t.dt_posicao between '10/07/2015 10:00:00.000' and '14/07/2015 20:00:00.000'";
            SqlDataAdapter adpt = new SqlDataAdapter(sql2, con);
            DataTable dt = new DataTable();
            con.Open();
            adpt.Fill(dt);
            con.Close();

            total = int.Parse(dt.Rows[0][0].ToString());
           
            
            
            return lst2;

            
           
        
        }

        public List<Veiculos> GetAllVeiculos2(int page2)
        {
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString());
            List<Veiculos> lst = new List<Veiculos>();

            SqlCommand cmd = new SqlCommand("sp_utlimo_local", con);


            //  SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@PageNumber", page2);
            cmd.Parameters.AddWithValue("@RowspPage", 50);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            con.Open();
            SqlDataReader dataReader = cmd.ExecuteReader();




            while (dataReader.Read())
            {

                Veiculos e = new Veiculos();
                e.linha = Convert.ToInt32(dataReader["NUMBER"]);
                e.nr_idveiculo = Convert.ToInt32(dataReader["nr_idveiculo"]);
                e.ds_placa = dataReader["ds_placa"].ToString();
                e.fl_bloqueio = Convert.ToInt32(dataReader["fl_bloqueio"]);
                e.ds_cidade = dataReader["ds_cidade"].ToString();
                e.dt_posicao = dataReader["dt_posicao"].ToString();
                e.nr_dist_referencia = Convert.ToInt32(dataReader["nr_dist_referencia"]);
                e.nr_gps = Convert.ToInt32(dataReader["nr_gps"]);
                e.fl_ignicao = Convert.ToInt32(dataReader["fl_ignicao"]);
                e.nr_jamming = Convert.ToInt32(dataReader["nr_jamming"]);
                e.ds_lat = dataReader["ds_lat"].ToString();
                e.ds_long = dataReader["ds_long"].ToString();
                e.nr_odometro = Convert.ToInt32(dataReader["nr_odometro"]);
                e.nr_pontoreferencia = dataReader["nr_pontoreferencia"].ToString();
                e.ds_rua = dataReader["ds_rua"].ToString();
                e.nr_tensao = Convert.ToInt32(dataReader["nr_tensao"]);
                e.nr_satelite = Convert.ToInt32(dataReader["nr_satelite"]);
                e.ds_uf = dataReader["ds_uf"].ToString();
                e.nr_velocidade = Convert.ToInt32(dataReader["nr_velocidade"]);
                lst.Add(e);


            }



            con.Close();

            //string dataini = "08/07/2015 10:00:00.000";
            //string datafim = "14/07/2015 23:59:59.000";
            //string placa1 = "DBC1530";
            string sql2 = "SELECT COUNT(*) ";
            sql2 += "FROM  tb_transmissao as t inner join tb_veiculo_sascar as v on t.nr_idveiculo=v.nr_idveiculo ";
           

            // sql2 += "where v.ds_placa='DBC1530' and t.dt_posicao between '10/07/2015 10:00:00.000' and '14/07/2015 20:00:00.000'";
            SqlDataAdapter adpt = new SqlDataAdapter(sql2, con);
            DataTable dt = new DataTable();
            con.Open();
            adpt.Fill(dt);
            con.Close();

            total2 = int.Parse(dt.Rows[0][0].ToString());



            return lst;




        }
        public List<Jornada> GetAllMotoristas(string mes, string codlogin, int page3)
        {
            
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString());
            List<Jornada> lst3 = new List<Jornada>();
            
            string ano = DateTime.Now.Year.ToString();
            string data1 = "01/" + mes + "/" + ano;
            
            if(mes == "01" || mes =="03"||mes =="05"||mes =="07"||mes =="08"||mes =="10"|| mes=="12")
            {
                 data2 = "31/" + mes + "/" + ano;
            }
            else
            {
                 data2 = "30/" + mes + "/" + ano;
            }

            
            SqlCommand cmd = new SqlCommand("sp_consulta_jornada", con);


            //  SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@dt_posicao1",data1);
            cmd.Parameters.AddWithValue("@dt_posicao2", data2);
            cmd.Parameters.AddWithValue("@cod_login", codlogin);
            cmd.Parameters.AddWithValue("@PageNumber", page3);
            cmd.Parameters.AddWithValue("@RowspPage", 50);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            con.Open();



            SqlDataReader dataReader = cmd.ExecuteReader();

            

            while (dataReader.Read())
            {

                Jornada e1 = new Jornada();
                e1.linha = Convert.ToInt32(dataReader["NUMBER"]);
                e1.dt_jornada = dataReader["dt_jornada"].ToString();
                e1.cod_login = Convert.ToInt32(dataReader["cod_login"]);
                e1.hr_inicio_jornada = dataReader["hr_inicio_jornada"].ToString();
                e1.hr_inicio_intervalo = dataReader["hr_inicio_intervalo"].ToString();
                e1.hr_fim_intervalo = dataReader["hr_fim_intervalo"].ToString();
                e1.hr_fim_jornada = dataReader["hr_fim_jornada"].ToString();
                e1.vl_almoco = dataReader["vl_almoco"].ToString();
                e1.vl_janta = dataReader["vl_janta"].ToString();
                e1.vl_pernoite = dataReader["vl_pernoite"].ToString();
                e1.vl_premio = dataReader["vl_premio"].ToString();
                e1.total = dataReader["total"].ToString();
                lst3.Add(e1);


            }



            con.Close();


            string sql2 = "SELECT COUNT(*) FROM  tb_jornada where cod_login=" + codlogin + " and dt_jornada between '"+data1+"' and '"+data2+"'";
         //  string  sql2 = "SELECT COUNT(*) FROM  tb_jornada";
         //   sql2 += "where cod_login=1053 ";
     
                SqlDataAdapter adpt = new SqlDataAdapter(sql2, con);
                DataTable dt = new DataTable();
                con.Open();
                adpt.Fill(dt);
                con.Close();
           
           
             

            total3 = int.Parse(dt.Rows[0][0].ToString());

         

            return lst3;




        }
    }
}