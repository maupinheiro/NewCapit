using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ConVeiculos
    {
        public static DataTable FetchDataTable()
        {
            string sql = "SELECT id, codvei, tipvei, plavei, reboque1, tipoveiculo, nucleo, transp, ativo_inativo FROM tbveiculos where fl_exclusao is null";

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
            string sql = "SELECT id, codvei, tipvei, plavei, reboque1, tipoveiculo, nucleo, transp, ativo_inativo FROM tbveiculos where fl_exclusao is null and plavei LIKE @searchTerm ";

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
