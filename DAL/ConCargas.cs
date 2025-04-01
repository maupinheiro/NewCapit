using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ConCargas
    {
        public static DataTable FetchDataTable()
        {
            // alterado a query para verifica a coluna exclusao para itens excluídos            
            string sql = "SELECT id, carga, peso, status, cliorigem, clidestino, CONVERT(varchar, previsao, 103) AS previsao, situacao,rota, andamento, data_hora, veiculo, tipo_viagem FROM tbcargas WHERE status = 'Pendente' and fl_exclusao is null";

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
        public static DataTable FetchDataTableColetas()
        {
            // alterado a query para verifica a coluna exclusao para itens excluídos            
            string sql = "SELECT id, peso, status, cliorigem, clidestino, CONVERT(varchar, previsao, 103) AS previsao, situacao,rota, andamento, data_hora, veiculo, tipo_viagem, solicitacoes FROM tbcargas WHERE status = 'Pendente' and empresa = 'CNT' and fl_exclusao is null";

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

    }
}
