using Dapper;
using Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ConEntrega
    {
        public static DataTable FetchDataTable()
        {
            // alterado a query para verificar a coluna exclusao para itens excluídos            
            string sql = "select cva,  '../../fotos/'+ REPLACE(m.caminhofoto, '/fotos/', '') AS fotos, c.nomemotorista, c.transportadora, c.veiculo, c.placa, c.carga,c.cva,c.nomcliorigem,c.nomclidestino,c.situacao, c.veiculotipo, c.num_carregamento from tbcarregamentos as c inner join tbmotoristas as m on c.codmotorista=m.codmot where empresa='CNT' and situacao <> 'VIAGEM CONCLUIDA' order by c.dtcad desc ";

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
        public static DataTable FetchDataTable2(DateTime? dataInicio, DateTime? dataFim, string status, string veiculo)
        {
            string sql = @"
        SELECT 
            cva, '../../fotos/' + REPLACE(m.caminhofoto, '/fotos/', '') AS fotos,
            c.nomemotorista, c.tipoveiculo, c.veiculo,
            (c.placa + ' / ' + c.reboque1 + ' / ' + c.reboque2) AS veiculo_reboques,
            c.carga, c.cva, c.nomcliorigem, c.nomclidestino,
            c.situacao, c.material, c.dtcad, c.num_carregamento
        FROM tbcarregamentos AS c
        INNER JOIN tbmotoristas AS m ON c.codmotorista = m.codmot
        WHERE empresa = 'CNT' AND situacao <> 'VIAGEM CONCLUIDA'";

            if (dataInicio.HasValue && dataFim.HasValue)
            {
                sql += " AND c.dtcad BETWEEN @dataInicio AND @dataFim";
               
            }

            if (!string.IsNullOrEmpty(status))
                sql += " AND c.situacao = @status";
            if (!string.IsNullOrEmpty(veiculo))
                sql += " AND c.veiculo = @veiculo";

            sql += " ORDER BY c.dtcad DESC";

            using (var con = ConnectionUtil.GetConnection())
            using (var cmd = con.CreateCommand())
            {
                cmd.CommandText = sql;

                if (dataInicio.HasValue && dataFim.HasValue)
                {
                    cmd.Parameters.Add("@dataInicio", SqlDbType.DateTime).Value = dataInicio.Value.ToString("yyyy-MM-dd HH:mm");
                    cmd.Parameters.Add("@dataFim", SqlDbType.DateTime).Value = dataFim.Value.ToString("yyyy-MM-dd HH:mm");
                }
                
                if (!string.IsNullOrEmpty(status))
                    cmd.Parameters.AddWithValue("@status", status);
                if (!string.IsNullOrEmpty(veiculo))
                    cmd.Parameters.AddWithValue("@veiculo", veiculo);

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
