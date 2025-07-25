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
    public class ConCargas
    {
        public static DataTable FetchDataTable()
        {
            // alterado a query para verifica a coluna exclusao para itens excluídos            
            string sql = "SELECT c.id, c.carga, c.cva, c.data_hora, cliOrigem.codvw + '/' + cliOrigem.codcli AS CodigoO, c.cliorigem, cliDestino.codvw + '/' + cliDestino.codcli AS CodigoD, c.clidestino, c.atendimento, c.tipo_viagem, c.rota, c.veiculo, c.quant_palet, c.peso, c.pedidos, c.solicitacoes, c.estudo_rota, c.remessa, c.cva, c.gate, c.status, c.chegadaorigem, c.saidaorigem, c.tempoagcarreg, c.chegadadestino, c.entradaplanta, c.saidaplanta, c.tempodentroplanta, c.tempoesperagate, c.previsao, c.situacao FROM tbcargas c LEFT JOIN tbclientes cliOrigem ON cliOrigem.codvw = c.codvworigem LEFT JOIN tbclientes cliDestino ON cliDestino.codvw = c.codvwdestino WHERE c.status = 'Pendente' AND c.fl_exclusao IS NULL ";

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
                                c.id, c.carga, c.cva, c.data_hora,
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
                            WHERE c.empresa = 'CNT (CC)' AND c.fl_exclusao IS NULL
                            ";


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
        public static DataTable FetchDataTablePesquisa(string searchTerm)
        {
            string sql = "SELECT ID, carga, cva, data_hora, solicitacoes,  cliorigem, clidestino, veiculo, tipo_viagem, rota,atendimento, andamento FROM tbcargas "
                + "where fl_exclusao is null and data_hora LIKE @searchTerm OR solicitacoes LIKE @searchTerm OR veiculo LIKE @searchTerm OR tipo_viagem LIKE @searchTerm OR cliorigem LIKE @searchTerm OR clidestino LIKE @searchTerm OR cva LIKE @searchTerm ORDER BY data_hora";

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

        public static DataTable FetchDataTableColetas2(string searchTerm)
        {
            // alterado a query para verifica a coluna exclusao para itens excluídos            
            string sql = "SELECT id, carga,cva,data_hora,  (select top 1 codvw+ '/'+ codcli from tbclientes where codvw=codvworigem) as CodigoO ,cliorigem, (select top 1 codvw+ '/'+ codcli from tbclientes where codvw=codvwdestino) as CodigoD,clidestino,atendimento,tipo_viagem,rota,veiculo,quant_palet,peso,pedidos,solicitacoes,estudo_rota,remessa,cva,gate,status,chegadaorigem,saidaorigem,tempoagcarreg,chegadadestino,entradaplanta,saidaplanta,tempodentroplanta, tempoesperagate,previsao,emissao,empresa,codvworigem,codvwdestino,cadastro,tomador, atualizacao,cliorigem,cidorigem,clidestino,ciddestino, solicitante FROM tbcargas WHERE codmot is null and status = 'Pendente' and empresa = 'CNT (CC)' and fl_exclusao is null and carga=@searchTerm";

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
            string sql = "SELECT id, carga,cva,data_hora,  (select top 1  codvw+ '/' + codcli from tbclientes where codvw=codvworigem) as CodigoO ,cliorigem, (select top 1 codvw+ '/'+ codcli from tbclientes where codvw=codvwdestino) as CodigoD,clidestino,atendimento,tipo_viagem,rota,veiculo,quant_palet,peso,pedidos,solicitacoes,estudo_rota,remessa,cva,gate,status,chegadaorigem,saidaorigem,tempoagcarreg,chegadadestino,entradaplanta,saidaplanta,tempodentroplanta, tempoesperagate,previsao FROM tbcargas WHERE  empresa = 'CNT (CC)' and fl_exclusao is null and idviagem=@idviagem";

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
            string query = $"SELECT carga,cva,data_hora,  (select codvw+ '/'+ codcli from tbclientes where codvw=codvworigem) as CodigoO ,cliorigem, (select codvw+ '/'+ codcli from tbclientes where codvw=codvwdestino) as CodigoD,clidestino,atendimento,tipo_viagem,rota,veiculo,quant_palet,peso,pedidos,solicitacoes,estudo_rota,remessa,cva,gate,status,chegadaorigem,saidaorigem,tempoagcarreg,chegadadestino,entradaplanta,saidaplanta,tempodentroplanta, tempoesperagate,previsao FROM tbcargas WHERE status = 'Pendente' and empresa = 'CNT (CC)' and fl_exclusao is null and carga IN ({listaCargas})";

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
            string sqlQuery = "SELECT * from tbcargas WHERE (carga=@carga)";

            using (var con = ConnectionUtil.GetConnection())
            {
                return con.Query<ConsultaCarga>(sqlQuery, carga).FirstOrDefault();
            }
        }

    }
}
