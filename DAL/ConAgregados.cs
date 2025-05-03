using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ConAgregados
    {
        public static DataTable FetchDataTable()
        {
            string sql = "SELECT ID, codtra, fantra, pessoa, cnpj, filial, fone2, ativa_inativa FROM tbtransportadoras where fl_exclusao is null ORDER BY fantra";

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
            string sql = "SELECT ID, codtra, fantra, pessoa, cnpj, filial, fone2, ativa_inativa FROM tbtransportadoras " 
                + "where fl_exclusao is null and codtra LIKE @searchTerm OR fantra LIKE @searchTerm OR cnpj LIKE @searchTerm  ORDER BY fantra";

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
