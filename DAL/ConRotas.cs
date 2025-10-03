using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ConRotas
    {

        public static DataTable FetchDataTable()
        {
            string sql = "SELECT rota, desc_rota, deslocamento, distancia, tempo, situacao FROM tbrotasdeentregas where fl_exclusao is null ORDER BY desc_rota";

            using (var con = ConnectionUtil.GetConnection())
            {
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = sql;
                    using (var reader = cmd.ExecuteReader())
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);
                        return dataTable;
                    }

                }
            }
        }

        public static DataTable FetchDataTableLimite()
        {
            string sql = "SELECT rota, desc_rota, deslocamento, distancia, tempo, situacao FROM tbrotasdeentregas where fl_exclusao is null ORDER BY desc_rota";

            using (var con = ConnectionUtil.GetConnection())
            {
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = sql;
                    using (var reader = cmd.ExecuteReader())
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);
                        return dataTable;
                    }

                }
            }
        }


        public static DataTable FetchDataTable2(string searchTerm)
        {
            string sql = "SELECT rota, desc_rota, deslocamento, distancia, tempo, situacao FROM tbrotasdeentregas where fl_exclusao is null and rota LIKE @searchTerm OR desc_rota LIKE @searchTerm OR deslocamento LIKE @searchTerm ORDER BY desc_rota";

            using (var con = ConnectionUtil.GetConnection())
            {
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");

                    using (var reader = cmd.ExecuteReader())
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);
                        return dataTable;
                    }
                }
            }
        }

    }
       
}
