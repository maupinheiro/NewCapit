using Dapper;
using Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
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
            string sql = "SELECT c.id, c.carga, c.cva, CONVERT(varchar, CAST(c.data_hora AS datetime), 103) + ' ' + CONVERT(varchar, CAST(c.data_hora AS datetime), 108) AS c.data_hora, cliOrigem.codvw + '/' + cliOrigem.codcli AS CodigoO, c.cliorigem, cliDestino.codvw + '/' + cliDestino.codcli AS CodigoD, c.clidestino, c.atendimento, c.tipo_viagem, c.rota, c.veiculo, c.quant_palet, c.peso, c.pedidos, c.solicitacoes, c.estudo_rota, c.remessa, c.cva, c.gate, c.status, c.chegadaorigem, c.saidaorigem, c.tempoagcarreg, c.chegadadestino, c.entradaplanta, c.saidaplanta, c.tempodentroplanta, c.tempoesperagate, c.previsao, c.situacao FROM tbcargas c LEFT JOIN tbclientes cliOrigem ON cliOrigem.codvw = c.codvworigem LEFT JOIN tbclientes cliDestino ON cliDestino.codvw = c.codvwdestino WHERE c.status = 'Pendente' AND c.fl_exclusao IS NULL ";

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
            string sql = @"SELECT 
                                c.id, c.carga, c.cva, CONVERT(varchar, CAST(c.data_hora AS datetime), 103) + ' ' + CONVERT(varchar, CAST(c.data_hora AS datetime), 108) AS data_hora,
                                (co.codvw + '/' + co.codcli) AS CodigoO,
                                c.cliorigem,
                                (cd.codvw + '/' + cd.codcli) AS CodigoD,
                                c.clidestino,
                                c.atendimento, c.tipo_viagem, c.rota, c.veiculo,
                                c.quant_palet, c.peso, c.pedidos, c.solicitacoes,
                                c.estudo_rota, c.remessa, c.cva, c.gate, c.status,
                                c.chegadaorigem, c.saidaorigem, c.tempoagcarreg,
                                c.chegadadestino, c.entradaplanta, c.saidaplanta,
                                c.tempodentroplanta, c.tempoesperagate,
                                c.previsao, c.andamento
                            FROM tbcargas c
                            OUTER APPLY (
                                SELECT TOP 1 codvw, codcli FROM tbclientes WHERE codvw = c.codvworigem
                            ) co
                            OUTER APPLY (
                                SELECT TOP 1 codvw, codcli FROM tbclientes WHERE codvw = c.codvwdestino
                            ) cd
                            WHERE c.empresa = 'CNT (CC)' AND c.fl_exclusao IS NULL and data_hora between @datainicial and @datafinal
                            ";


            using (var con = ConnectionUtil.GetConnection())
            {
               
                using (var cmd = con.CreateCommand())
                {
                    DateTime dtInicial, dtFinal;
                    cmd.Parameters.AddWithValue("@datainicial", DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 06:00"));
                    cmd.Parameters.AddWithValue("@datafinal", DateTime.Now.ToString("yyyy-MM-dd 23:59"));
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
        public static DataTable FetchDataTablePesquisa(string searchTerm, string datainicial, string datafinal)
        {
            var sql = new StringBuilder(@"
        SELECT ID, carga, cva, CONVERT(varchar, CAST(data_hora AS datetime), 103) + ' ' + CONVERT(varchar, CAST(data_hora AS datetime), 108) AS data_hora, solicitacoes, cliorigem, clidestino, veiculo, tipo_viagem, rota, atendimento, andamento 
        FROM tbcargas 
        WHERE fl_exclusao IS NULL");

            using (var con = ConnectionUtil.GetConnection())
            using (var cmd = con.CreateCommand())
            {
                // Filtro por searchTerm (opcional)
                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    sql.Append(@"
                AND (
                    data_hora LIKE @searchTerm OR 
                    solicitacoes LIKE @searchTerm OR 
                    veiculo LIKE @searchTerm OR 
                    tipo_viagem LIKE @searchTerm OR 
                    cliorigem LIKE @searchTerm OR 
                    clidestino LIKE @searchTerm OR 
                    cva LIKE @searchTerm
                )");

                    cmd.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");
                }

                // Filtro por data (opcional)
                DateTime dtInicial, dtFinal;

                if (!string.IsNullOrWhiteSpace(datainicial) && DateTime.TryParse(datainicial, out dtInicial) &&
                    !string.IsNullOrWhiteSpace(datafinal) && DateTime.TryParse(datafinal, out dtFinal))
                {
                    sql.Append(" AND previsao BETWEEN @dataInicial AND @dataFinal");
                    cmd.Parameters.Add("@dataInicial", SqlDbType.DateTime).Value = dtInicial;
                    cmd.Parameters.Add("@dataFinal", SqlDbType.DateTime).Value = dtFinal;
                }
                else if (!string.IsNullOrWhiteSpace(datainicial) && DateTime.TryParse(datainicial, out dtInicial))
                {
                    sql.Append(" AND previsao >= @dataInicial");
                    cmd.Parameters.Add("@dataInicial", SqlDbType.DateTime).Value = dtInicial;
                }
                else if (!string.IsNullOrWhiteSpace(datafinal) && DateTime.TryParse(datafinal, out dtFinal))
                {
                    sql.Append(" AND previsao <= @dataFinal");
                    cmd.Parameters.Add("@dataFinal", SqlDbType.DateTime).Value = dtFinal;
                }

                // Finaliza e executa
                cmd.CommandText = sql.ToString();

                using (var reader = cmd.ExecuteReader())
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    return dt;
                }
            }
        }


        public static DataTable FetchDataTableColetas2(string searchTerm)
        {
            // alterado a query para verifica a coluna exclusao para itens excluídos            
            string sql = "SELECT id, carga,cva,CONVERT(varchar, CAST(data_hora AS datetime), 103) + ' ' + CONVERT(varchar, CAST(data_hora AS datetime), 108) AS data_hora,  (select top 1 codvw+ '/'+ codcli from tbclientes where codvw=codvworigem) as CodigoO ,cliorigem, (select top 1 codvw+ '/'+ codcli from tbclientes where codvw=codvwdestino) as CodigoD,clidestino,atendimento,tipo_viagem,rota,veiculo,quant_palet,peso,pedidos,solicitacoes,estudo_rota,remessa,cva,gate,status,chegadaorigem,saidaorigem,tempoagcarreg,chegadadestino,entradaplanta,saidaplanta,tempodentroplanta, tempoesperagate,previsao,emissao,empresa,codvworigem,codvwdestino,cadastro,tomador, atualizacao,cliorigem,cidorigem,clidestino,ciddestino, solicitante FROM tbcargas WHERE codmot is null and status = 'Pendente' and empresa = 'CNT (CC)' and fl_exclusao is null and cva=@searchTerm";

            using (var con = ConnectionUtil.GetConnection())
            {
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@searchTerm", searchTerm);

                    using (var reader = cmd.ExecuteReader())
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);
                        return dataTable;
                    }
                }
            }
        }
        public static DataTable FetchDataTableColetas3(string idviagem)
        {
            // alterado a query para verifica a coluna exclusao para itens excluídos            
            string sql = "SELECT id, carga,cva,CONVERT(varchar, CAST(data_hora AS datetime), 103) + ' ' + CONVERT(varchar, CAST(data_hora AS datetime), 108) AS data_hora,  (select top 1  codvw+ '/' + codcli from tbclientes where codvw=codvworigem) as CodigoO ,cliorigem, (select top 1 codvw+ '/'+ codcli from tbclientes where codvw=codvwdestino) as CodigoD,clidestino,atendimento,tipo_viagem,rota,veiculo,quant_palet,peso,pedidos,solicitacoes,estudo_rota,remessa,cva,gate,status,chegadaorigem,saidaorigem,tempoagcarreg,chegadadestino,entradaplanta,saidaplanta,tempodentroplanta, tempoesperagate,previsao FROM tbcargas WHERE  empresa = 'CNT (CC)' and fl_exclusao is null and idviagem=@idviagem";

            using (var con = ConnectionUtil.GetConnection())
            {
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@idviagem", idviagem);

                    using (var reader = cmd.ExecuteReader())
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);
                        return dataTable;
                    }
                }
            }
        }
        public static DataTable FetchDataTableColetasPorCargas(List<string> cargas)
        {
            string listaCargas = string.Join(",", cargas.Select(c => $"'{c}'"));
            string query = $"SELECT carga,cva,CONVERT(varchar, CAST(data_hora AS datetime), 103) + ' ' + CONVERT(varchar, CAST(data_hora AS datetime), 108) AS data_hora,  (select codvw+ '/'+ codcli from tbclientes where codvw=codvworigem) as CodigoO ,cliorigem, (select codvw+ '/'+ codcli from tbclientes where codvw=codvwdestino) as CodigoD,clidestino,atendimento,tipo_viagem,rota,veiculo,quant_palet,peso,pedidos,solicitacoes,estudo_rota,remessa,cva,gate,status,chegadaorigem,saidaorigem,tempoagcarreg,chegadadestino,entradaplanta,saidaplanta,tempodentroplanta, tempoesperagate,previsao FROM tbcargas WHERE status = 'Pendente' and empresa = 'CNT (CC)' and fl_exclusao is null and carga IN ({listaCargas})";

            using (var con = ConnectionUtil.GetConnection())
            {
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public static ConsultaCarga CheckColetas(ConsultaCarga carga)
        {
            // alterado a query para verifica a coluna exclusao para itens excluídos            
            string sqlQuery = "SELECT * from tbcargas WHERE (cva=@carga)";

            using (var con = ConnectionUtil.GetConnection())
            {
                return con.Query<ConsultaCarga>(sqlQuery, carga).FirstOrDefault();
            }
        }

    }
}
