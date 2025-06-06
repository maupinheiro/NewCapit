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
            string sql = "SELECT id, codvei, tipvei, plavei, reboque1, venclicenciamento, venclicencacet, vencimentolaudofumaca, reboque2, rastreamento, rastreador, tipoveiculo, nucleo, codtra, transp,tiporeboque, tipocarreta, ativo_inativo FROM tbveiculos where fl_exclusao is null";

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
            string sql = "SELECT id, codvei, tipvei, plavei, reboque1, reboque2, tipoveiculo, nucleo, transp, ativo_inativo FROM tbveiculos where fl_exclusao is null and plavei LIKE @searchTerm or codvei LIKE @searchTerm or nucleo LIKE @searchTerm or transp LIKE @searchTerm or tipvei LIKE @searchTerm or reboque1 LIKE @searchTerm or reboque2 LIKE @searchTerm ORDER BY codvei";

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
