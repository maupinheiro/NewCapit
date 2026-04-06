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

        public static DataTable FetchDataTableEntregasMatriz(DateTime? dataInicio, DateTime? dataFim, string busca = "")
        {
            var sql = @"
                        SELECT 
                            c.veiculo, c.tipoveiculo, c.placa, c.reboque1, c.reboque2,
                            '../../fotos/' + REPLACE(m.caminhofoto, '/fotos/', '') AS fotos,
                            c.codmotorista, c.nomemotorista, c.codtra, c.transportadora,
                            c.cod_expedidor, c.expedidor, c.cid_expedidor, c.uf_expedidor,
                            c.cod_recebedor, c.recebedor, c.cid_recebedor, c.uf_recebedor,
                            c.num_carregamento, c.emissao, c.situacao, c.status, c.carga
                        FROM tbcarregamentos c
                        INNER JOIN tbmotoristas m ON c.codmotorista = m.codmot
                        WHERE empresa = '1111' AND c.situacao <> 'VIAGEM CONCLUIDA' ";

            if (dataInicio.HasValue) sql += " AND c.emissao >= @DataInicio";
            if (dataFim.HasValue) sql += " AND c.emissao <= @DataFim";

            // NOVO: Filtro de busca global no SQL
            sql += @"
                            AND (
                                @Busca IS NULL

                                OR c.veiculo LIKE @Busca
                                OR c.nomemotorista LIKE @Busca
                                OR c.placa LIKE @Busca
                                OR c.transportadora LIKE @Busca
                                OR c.num_carregamento LIKE @Busca

                                OR (@CodMotorista IS NOT NULL AND c.codmotorista = @CodMotorista)
                            )";

            sql += " ORDER BY c.veiculo, c.emissao ASC";

            using (var con = ConnectionUtil.GetConnection())
            using (var cmd = con.CreateCommand())
            {
                cmd.CommandText = sql;
                if (dataInicio.HasValue) cmd.Parameters.AddWithValue("@DataInicio", dataInicio.Value.Date);
                if (dataFim.HasValue) cmd.Parameters.AddWithValue("@DataFim", dataFim.Value.Date.AddDays(1).AddSeconds(-1));
                if (!string.IsNullOrWhiteSpace(busca))
                {
                    cmd.Parameters.AddWithValue("@Busca", "%" + busca + "%");

                    if (int.TryParse(busca, out int cod))
                        cmd.Parameters.AddWithValue("@CodMotorista", cod);
                    else
                        cmd.Parameters.AddWithValue("@CodMotorista", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Busca", DBNull.Value);
                    cmd.Parameters.AddWithValue("@CodMotorista", DBNull.Value);
                }

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
    DateTime? dataFim,
    int pagina = 0,
    int registrosPorPagina = 75,
    string busca = "")
        {
            int inicio = (pagina * registrosPorPagina) + 1;
            int fim = (pagina + 1) * registrosPorPagina;

            string sql = @"
                        WITH ResultadoPaginado AS (
                            SELECT 
                                c.veiculo, c.tipoveiculo, c.placa, c.reboque1, c.reboque2,
                                '../../fotos/' + REPLACE(m.caminhofoto, '/fotos/', '') AS fotos,
                                c.codmotorista, c.nomemotorista, c.codtra, c.transportadora,
                                c.cod_expedidor, c.expedidor, c.cid_expedidor, c.uf_expedidor,
                                c.cod_recebedor, c.recebedor, c.cid_recebedor, c.uf_recebedor,
                                c.num_carregamento, c.emissao, c.situacao, c.status, c.carga,
                                ROW_NUMBER() OVER (ORDER BY c.emissao DESC, c.veiculo ASC) AS RowNum
                            FROM tbcarregamentos c
                            INNER JOIN tbmotoristas m ON c.codmotorista = m.codmot
                            WHERE c.empresa = '1111' 
                              AND c.situacao = 'VIAGEM CONCLUIDA'
                        ";

                                    if (dataInicio.HasValue)
                                        sql += " AND c.emissao >= @DataInicio";

                                    if (dataFim.HasValue)
                                        sql += " AND c.emissao <= @DataFim";

                                    // 🔥 Inteligência na busca
                                    sql += @"
                            AND (
                                @Busca IS NULL

                                OR c.veiculo LIKE @Busca
                                OR c.nomemotorista LIKE @Busca
                                OR c.placa LIKE @Busca
                                OR c.transportadora LIKE @Busca
                                OR c.num_carregamento LIKE @Busca

                                OR (@CodMotorista IS NOT NULL AND c.codmotorista = @CodMotorista)
                            )";

                                    sql += @")
                        SELECT * FROM ResultadoPaginado
                        WHERE RowNum BETWEEN @Inicio AND @Fim";

            using (var con = ConnectionUtil.GetConnection())
            using (var cmd = con.CreateCommand())
            {
                cmd.CommandText = sql;

                cmd.Parameters.AddWithValue("@Inicio", inicio);
                cmd.Parameters.AddWithValue("@Fim", fim);

                if (dataInicio.HasValue)
                    cmd.Parameters.AddWithValue("@DataInicio", dataInicio.Value.Date);

                if (dataFim.HasValue)
                    cmd.Parameters.AddWithValue("@DataFim", dataFim.Value.Date.AddDays(1).AddSeconds(-1));

                // 🎯 Processamento inteligente da busca
                if (!string.IsNullOrWhiteSpace(busca))
                {
                    cmd.Parameters.AddWithValue("@Busca", "%" + busca + "%");

                    if (int.TryParse(busca, out int cod))
                        cmd.Parameters.AddWithValue("@CodMotorista", cod);
                    else
                        cmd.Parameters.AddWithValue("@CodMotorista", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Busca", DBNull.Value);
                    cmd.Parameters.AddWithValue("@CodMotorista", DBNull.Value);
                }

                using (var reader = cmd.ExecuteReader())
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    return dt;
                }
            }
        }
        public static int GetTotalRegistrosConcluidos(
    DateTime? dataInicio,
    DateTime? dataFim,
    string busca = "")
        {
            string sql = @"
                            SELECT COUNT(*) 
                            FROM tbcarregamentos c
                            WHERE c.empresa = '1111'
                              AND c.situacao = 'VIAGEM CONCLUIDA'
                            ";

                                        if (dataInicio.HasValue)
                                            sql += " AND c.emissao >= @DataInicio";

                                        if (dataFim.HasValue)
                                            sql += " AND c.emissao <= @DataFim";

                                        sql += @"
                            AND (
                                @Busca IS NULL

                                OR c.veiculo LIKE @Busca
                                OR c.nomemotorista LIKE @Busca
                                OR c.placa LIKE @Busca
                                OR c.transportadora LIKE @Busca
                                OR c.num_carregamento LIKE @Busca

                                OR (@CodMotorista IS NOT NULL AND c.codmotorista = @CodMotorista)
                            )";

            using (var con = ConnectionUtil.GetConnection())
            using (var cmd = con.CreateCommand())
            {
                cmd.CommandText = sql;

                if (dataInicio.HasValue)
                    cmd.Parameters.AddWithValue("@DataInicio", dataInicio.Value.Date);

                if (dataFim.HasValue)
                    cmd.Parameters.AddWithValue("@DataFim", dataFim.Value.Date.AddDays(1).AddSeconds(-1));

                if (!string.IsNullOrWhiteSpace(busca))
                {
                    cmd.Parameters.AddWithValue("@Busca", "%" + busca + "%");

                    if (int.TryParse(busca, out int cod))
                        cmd.Parameters.AddWithValue("@CodMotorista", cod);
                    else
                        cmd.Parameters.AddWithValue("@CodMotorista", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Busca", DBNull.Value);
                    cmd.Parameters.AddWithValue("@CodMotorista", DBNull.Value);
                }

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public static DataTable FetchDataTableEntregasMatrizUnificado(
     DateTime? dataInicio,
     DateTime? dataFim,
     int pagina,
     int registrosPorPagina,
     string busca,
     bool somenteConcluidas)
        {
            int inicio = (pagina * registrosPorPagina) + 1;
            int fim = (pagina + 1) * registrosPorPagina;

            string sql = @"
                                WITH ResultadoPaginado AS (
                                    SELECT 
                                        c.veiculo, c.tipoveiculo, c.placa, c.reboque1, c.reboque2,
                                        '../../fotos/' + REPLACE(m.caminhofoto, '/fotos/', '') AS fotos,
                                        c.codmotorista, c.nomemotorista, c.codtra, c.transportadora,
                                        c.cod_expedidor, c.expedidor, c.cid_expedidor, c.uf_expedidor,
                                        c.cod_recebedor, c.recebedor, c.cid_recebedor, c.uf_recebedor,
                                        c.num_carregamento, c.emissao, c.situacao, c.status, c.carga,

                                        ROW_NUMBER() OVER (ORDER BY c.emissao DESC, c.veiculo ASC) AS RowNum

                                    FROM tbcarregamentos c
                                    INNER JOIN tbmotoristas m ON c.codmotorista = m.codmot

                                    WHERE c.empresa = '1111'

                                    AND (
                                        @SomenteConcluidas = 0
                                        OR c.situacao = 'VIAGEM CONCLUIDA'
                                    )
                                ";

                                            if (dataInicio.HasValue)
                                                sql += " AND c.emissao >= @DataInicio";

                                            if (dataFim.HasValue)
                                                sql += " AND c.emissao <= @DataFim";

                                            sql += @"
                                    AND (
                                        @Busca IS NULL

                                        OR c.veiculo LIKE @Busca
                                        OR c.nomemotorista LIKE @Busca
                                        OR c.placa LIKE @Busca
                                        OR c.transportadora LIKE @Busca

                                        -- 👇 BLINDAGEM PARA CAMPOS NUMÉRICOS
                                        OR CAST(c.num_carregamento AS VARCHAR(50)) LIKE @Busca

                                        -- 👇 AGORA STRING COM STRING (SEM ERRO)
                                        OR c.codmotorista LIKE @Busca
                                    )
                                )
                                SELECT * FROM ResultadoPaginado
                                WHERE RowNum BETWEEN @Inicio AND @Fim
                                ";

            using (var con = ConnectionUtil.GetConnection())
            using (var cmd = con.CreateCommand())
            {
                cmd.CommandText = sql;

                // 📄 Paginação
                cmd.Parameters.Add("@Inicio", SqlDbType.Int).Value = inicio;
                cmd.Parameters.Add("@Fim", SqlDbType.Int).Value = fim;

                // 🔘 Filtro
                cmd.Parameters.Add("@SomenteConcluidas", SqlDbType.Bit).Value = somenteConcluidas;

                // 📅 Datas
                if (dataInicio.HasValue)
                    cmd.Parameters.Add("@DataInicio", SqlDbType.DateTime).Value = dataInicio.Value.Date;

                if (dataFim.HasValue)
                    cmd.Parameters.Add("@DataFim", SqlDbType.DateTime).Value = dataFim.Value.Date.AddDays(1).AddSeconds(-1);

                // 🔍 Busca (SEM risco de conversão)
                if (!string.IsNullOrWhiteSpace(busca))
                {
                    busca = busca.Trim();
                    cmd.Parameters.Add("@Busca", SqlDbType.NVarChar).Value = "%" + busca + "%";
                }
                else
                {
                    cmd.Parameters.Add("@Busca", SqlDbType.NVarChar).Value = DBNull.Value;
                }

                using (var reader = cmd.ExecuteReader())
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    return dt;
                }
            }
        }
        public static int GetTotalRegistrosUnificado(
    DateTime? dataInicio,
    DateTime? dataFim,
    string busca,
    bool somenteConcluidas)
        {
            string sql = @"
                            SELECT COUNT(*)
                            FROM tbcarregamentos c
                            WHERE c.empresa = '1111'

                            AND (
                                @SomenteConcluidas = 0
                                OR c.situacao = 'VIAGEM CONCLUIDA'
                            )
                            ";

                                    if (dataInicio.HasValue)
                                        sql += " AND c.emissao >= @DataInicio";

                                    if (dataFim.HasValue)
                                        sql += " AND c.emissao <= @DataFim";

                                    sql += @"
                            AND (
                                @Busca IS NULL

                                OR c.veiculo LIKE @Busca
                                OR c.nomemotorista LIKE @Busca
                                OR c.placa LIKE @Busca
                                OR c.transportadora LIKE @Busca

                                -- 🔥 CAMPOS NUMÉRICOS PROTEGIDOS
                                OR CAST(c.num_carregamento AS VARCHAR(50)) LIKE @Busca
                                OR CAST(c.codtra AS VARCHAR(50)) LIKE @Busca
                                OR CAST(c.cod_expedidor AS VARCHAR(50)) LIKE @Busca
                                OR CAST(c.cod_recebedor AS VARCHAR(50)) LIKE @Busca

                                -- 🔥 VARCHAR NORMAL
                                OR c.codmotorista LIKE @Busca
                            )
                            ";

            using (var con = ConnectionUtil.GetConnection())
            using (var cmd = con.CreateCommand())
            {
                cmd.CommandText = sql;

                // 🔘 Filtro
                cmd.Parameters.Add("@SomenteConcluidas", SqlDbType.Bit).Value = somenteConcluidas;

                // 📅 Datas
                if (dataInicio.HasValue)
                    cmd.Parameters.Add("@DataInicio", SqlDbType.DateTime).Value = dataInicio.Value.Date;

                if (dataFim.HasValue)
                    cmd.Parameters.Add("@DataFim", SqlDbType.DateTime).Value = dataFim.Value.Date.AddDays(1).AddSeconds(-1);

                // 🔍 Busca
                if (!string.IsNullOrWhiteSpace(busca))
                {
                    busca = busca.Trim();
                    cmd.Parameters.Add("@Busca", SqlDbType.NVarChar).Value = "%" + busca + "%";
                }
                else
                {
                    cmd.Parameters.Add("@Busca", SqlDbType.NVarChar).Value = DBNull.Value;
                }

                return Convert.ToInt32(cmd.ExecuteScalar());
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
