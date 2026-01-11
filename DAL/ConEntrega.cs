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
            string sql = "select cva,  '../../fotos/'+ REPLACE(m.caminhofoto, '/fotos/', '') AS fotos, c.codmotorista,c.nomemotorista,c.codtra, c.transportadora, c.veiculo, c.placa, c.reboque1, c.reboque2, c.status, c.carga,c.cva,c.nomcliorigem,c.nomclidestino,c.situacao, c.veiculotipo, c.tipoveiculo, c.num_carregamento from tbcarregamentos as c inner join tbmotoristas as m on c.codmotorista=m.codmot where empresa='CNT (CC)' and situacao <> 'VIAGEM CONCLUIDA' order by c.dtcad desc ";

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

        public static DataTable FetchDataTableEntregasMatriz(
    DateTime? dataInicio,
    DateTime? dataFim)
        {
            var sql = @"
        SELECT 
            c.veiculo, c.tipoveiculo, c.placa, c.reboque1, c.reboque2,
            '../../fotos/' + REPLACE(m.caminhofoto, '/fotos/', '') AS fotos,
            c.codmotorista, c.nomemotorista, c.codtra, c.transportadora,
            c.cod_expedidor, c.expedidor, c.cid_expedidor, c.uf_expedidor,
            c.cod_recebedor, c.recebedor, c.cid_recebedor, c.uf_recebedor,
            c.num_carregamento, c.emissao, c.situacao, c.status
        FROM tbcarregamentos c
        INNER JOIN tbmotoristas m ON c.codmotorista = m.codmot
        WHERE empresa = '1111' AND c.situacao <> 'VIAGEM CONCLUIDA'
    ";

            if (dataInicio.HasValue)
                sql += " AND c.emissao >= @DataInicio";

            if (dataFim.HasValue)
                sql += " AND c.emissao <= @DataFim";

            sql += " ORDER BY c.veiculo, c.emissao ASC";

            using (var con = ConnectionUtil.GetConnection())
            using (var cmd = con.CreateCommand())
            {
                cmd.CommandText = sql;

                if (dataInicio.HasValue)
                    cmd.Parameters.AddWithValue("@DataInicio", dataInicio.Value.Date);

                if (dataFim.HasValue)
                    cmd.Parameters.AddWithValue(
                        "@DataFim",
                        dataFim.Value.Date.AddDays(1).AddSeconds(-1)
                    );

                using (var reader = cmd.ExecuteReader())
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    return dt;
                }
            }
        }


        public static DataTable FetchDataTableEntregasMatrizConcluida(
    DateTime? dataInicio,
    DateTime? dataFim)
        {
            string sql = @"
        SELECT 
            c.veiculo, c.tipoveiculo, c.placa, c.reboque1, c.reboque2,
            '../../fotos/' + REPLACE(m.caminhofoto, '/fotos/', '') AS fotos,
            c.codmotorista, c.nomemotorista, c.codtra, c.transportadora,
            c.cod_expedidor, c.expedidor, c.cid_expedidor, c.uf_expedidor,
            c.cod_recebedor, c.recebedor, c.cid_recebedor, c.uf_recebedor,
            c.num_carregamento, c.emissao, c.situacao, c.status
        FROM tbcarregamentos c
        INNER JOIN tbmotoristas m ON c.codmotorista = m.codmot
        WHERE empresa = '1111'
          
    ";

           

            if (dataFim.HasValue)
                sql += " AND c.emissao <= @DataFim";

            sql += " ORDER BY c.veiculo, c.emissao ASC";

            using (var con = ConnectionUtil.GetConnection())
            using (var cmd = con.CreateCommand())
            {
                cmd.CommandText = sql;

                if (dataInicio.HasValue)
                    cmd.Parameters.AddWithValue("@DataInicio", dataInicio.Value.Date);

                if (dataFim.HasValue)
                    cmd.Parameters.AddWithValue(
                        "@DataFim",
                        dataFim.Value.Date.AddDays(1).AddSeconds(-1)
                    );

                using (var reader = cmd.ExecuteReader())
                {
                    DataTable dataTable = new DataTable();
                    dataTable.Load(reader);
                    return dataTable;
                }
            }
        }

        public static DataTable FetchDataTable2(DateTime? dataInicio, DateTime? dataFim)
        {
            string sql = @"
        SELECT 
            cva, '../../fotos/' + REPLACE(m.caminhofoto, '/fotos/', '') AS fotos, c.codtra,c.transportadora,
            c.codmotorista,c.nomemotorista, c.tipoveiculo, c.veiculo,c.placa, c.reboque1, c.reboque2, c.status,
            (c.placa + ' / ' + c.reboque1 + ' / ' + c.reboque2) AS veiculo_reboques,
            c.carga, c.cva, c.nomcliorigem, c.nomclidestino,
            c.situacao, c.material, c.dtcad, c.num_carregamento
        FROM tbcarregamentos AS c
        INNER JOIN tbmotoristas AS m ON c.codmotorista = m.codmot
        WHERE empresa = 'CNT (CC)' AND situacao <> 'VIAGEM CONCLUIDA'";

            if (dataInicio.HasValue && dataFim.HasValue)
            {
                sql += " AND c.dtcad BETWEEN @dataInicio AND @dataFim";
               
            }

          
          

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
