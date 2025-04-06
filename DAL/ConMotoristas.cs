using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ConMotoristas
    {
        public static DataTable FetchDataTable()
        {
            string sql = "SELECT caminhofoto, codmot, nommot, cargo, tipomot, funcao, horario, codtra, transp, nucleo, CONVERT(varchar, dtnasc, 103) AS dtnasc, cpf, CONVERT(varchar, cadmot, 103) AS cadmot, status, cartaomot, venccartao, CONVERT(varchar, venccnh, 103) AS venccnh, fone2, validade,  id FROM tbmotoristas where fl_exclusao is null ORDER BY nommot";

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
            string sql = "SELECT caminhofoto, codmot, nommot, cargo, tipomot, funcao, horario, codtra, transp, nucleo, CONVERT(varchar, dtnasc, 103) AS dtnasc, cpf, CONVERT(varchar, cadmot, 103) AS cadmot, status, id FROM tbmotoristas where fl_exclusao is null and and codmot LIKE @searchTerm or nommot LIKE @searchTerm  ORDER BY nommot";

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
