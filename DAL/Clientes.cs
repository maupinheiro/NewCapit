using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


namespace DAL
{
    public class codCliente
    {
        public static DataTable FetchDataTable()
        {
            string sql = "SELECT id, codcli, tipo, nomcli, unidade, regiao, cidcli, estcli, CONVERT(varchar, dtccli, 103) AS dtccli, ativo_inativo FROM tbclientes";
           
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
        public static ConsultaCliente CheckCliente(ConsultaCliente obj)
        {
            string sqlQuery = "SELECT codcli FROM tbclientes WHERE (codcli = @codcli)";

            using (var con = ConnectionUtil.GetConnection())
            {
                return con.Query<ConsultaCliente>(sqlQuery, obj).FirstOrDefault();
            }
        }

    }
}