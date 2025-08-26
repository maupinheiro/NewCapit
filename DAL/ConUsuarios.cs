using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ConUsuarios
    {
        public static DataTable FetchDataTable()
        {
            string sql = "SELECT foto_usuario, cod_usuario, nm_nome, nm_usuario, emp_usuario, fun_usuario, dep_usuario, CONVERT(varchar, dt_ultimo_acesso, 103) AS dt_ultimo_acesso, fl_tipo, fl_status FROM tb_usuario ORDER BY nm_nome";

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
            string sql = "SELECT foto_usuario, cod_usuario, nm_nome, nm_usuario, emp_usuario, fun_usuario, dep_usuario, CONVERT(varchar, dt_ultimo_acesso, 103) AS dt_ultimo_acesso," +
                " fl_tipo, fl_status FROM tb_usuario where nm_nome LIKE @searchTerm or emp_usuario LIKE @searchTerm or fun_usuario LIKE @searchTerm or nm_usuario LIKE @searchTerm or fl_tipo LIKE @searchTerm or fl_status LIKE @searchTerm ORDER BY nm_nome";

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
