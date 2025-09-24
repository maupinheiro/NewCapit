using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ConFretes
    {
        public static DataTable FetchDataTable()
        {
            string sql = "SELECT cod_frete, pagador, expedidor, recebedor, frete_tng, frete_agregado, frete_terceiro, vigencia_inicial, vigencia_final, situacao FROM tbtabeladefretes where fl_exclusao is null ORDER BY pagador";

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
            string sql = "SELECT cod_frete, pagador, expedidor, recebedor, frete_tng, frete_agregado, frete_terceiro, vigencia_inicial, vigencia_final, situacao FROM tbtabeladefretes";

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
            string sql = "SELECT cod_frete, pagador, expedidor, recebedor, frete_tng, frete_agregado, frete_terceiro, vigencia_inicial, vigencia_final, situacao FROM tbtabeladefretes where fl_exclusao is null and pagador LIKE @searchTerm OR cod_frete LIKE @searchTerm OR expedidor LIKE @searchTerm OR recebedor LIKE @searchTerm ORDER BY pagador";

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
