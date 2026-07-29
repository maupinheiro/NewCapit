using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace DAL
{
    public class MotoristaDAL
    {
        private readonly string conexao = WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
        public DataTable ListarMotoristas()
        {
            using (SqlConnection conn = new SqlConnection(conexao))
            {
                string sql = @"
                SELECT
                codmot,
                nommot,
                CAST(codmot AS NVARCHAR(11))
                + ' - '
                + nommot AS Motorista
                FROM tbmotoristas
                WHERE status = 'ATIVO' AND tipomot='FUNCIONÁRIO'
                ORDER BY nommot
                ";


                SqlDataAdapter da =
                    new SqlDataAdapter(sql, conn);


                DataTable dt =
                    new DataTable();


                da.Fill(dt);


                return dt;
            }
        }
        public DataRow ObterMotorista(string codmot)
        {
            using (SqlConnection conn =
                new SqlConnection(conexao))
            {

                string sql = @"
                SELECT
                codmot,
                nommot,
                funcao,
                nucleo
                FROM tbmotoristas
                WHERE codmot=@codmot AND tipomot='FUNCIONÁRIO'
                ";


                SqlDataAdapter da =
                    new SqlDataAdapter(sql, conn);


                da.SelectCommand.Parameters.AddWithValue(
                    "@codmot",
                    codmot);


                DataTable dt =
                    new DataTable();


                da.Fill(dt);


                if (dt.Rows.Count == 0)
                    return null;


                return dt.Rows[0];
            }
        }



    }
}
