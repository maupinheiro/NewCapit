using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ConCarretas
    {
        public static DataTable FetchDataTable()
        {
            string sql = "SELECT idcarreta, codcarreta, placacarreta, licenciamento, modelo, anocarreta, tiporeboque, descprop, nucleo, frota, placa, ativo_inativo FROM tbcarretas where fl_exclusao is null";

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
            string sql = "SELECT idcarreta, codcarreta, placacarreta, licenciamento, modelo, anocarreta, tiporeboque, descprop, nucleo, frota, placa_cavalo, ativo_inativo FROM tbcarretas where fl_exclusao is null and placacarreta LIKE @searchTerm or codcarreta LIKE @searchTerm or nucleo LIKE @searchTerm or descprop LIKE @searchTerm or licenciamento LIKE @searchTerm or modelo LIKE @searchTerm or tiporeboque LIKE @searchTerm ORDER BY codcarreta";

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
