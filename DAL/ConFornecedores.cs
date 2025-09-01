using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ConFornecedores
    {
        public static DataTable FetchDataTable()
        {
            string sql = "SELECT id, codfor, fantasia, tipofornecedor, contato, fonefixo, fonecelular, status, cidade, estado FROM tbfornecedores where fl_exclusao is null ORDER BY fantasia";

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
            string sql = "SELECT id, codfor, fantasia, tipofornecedor, contato, fonefixo, fonecelular, status, cidade, estado FROM tbfornecedores where fl_exclusao is null ORDER BY fantasia";

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
            string sql = "SELECT id, codfor, fantasia, tipofornecedor, contato, fonefixo, fonecelular, status, cidade, estado FROM tbfornecedores "
                + "where fl_exclusao is null and codfor LIKE @searchTerm OR fantasia LIKE @searchTerm OR tipofornecedor LIKE @searchTerm OR status LIKE @searchTerm OR cidade LIKE @searchTerm  ORDER BY fantasia";

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
