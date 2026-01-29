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
            string sql = "SELECT c.id, c.carga, c.cva,  data_hora, cliOrigem.codvw + '/' + cliOrigem.codcli AS CodigoO, c.cliorigem, cliDestino.codvw + '/' + cliDestino.codcli AS CodigoD, c.clidestino, c.atendimento, c.tipo_viagem, c.rota, c.veiculo, c.quant_palet, c.peso, c.pedidos, c.solicitacoes, c.estudo_rota, c.remessa, c.cva, c.gate, c.status, c.chegadaorigem, c.saidaorigem, c.tempoagcarreg, c.chegadadestino, c.entradaplanta, c.saidaplanta, c.tempodentroplanta, c.tempoesperagate, c.previsao, c.situacao,c.codmot FROM tbcargas c LEFT JOIN tbclientes cliOrigem ON cliOrigem.codvw = c.codvworigem LEFT JOIN tbclientes cliDestino ON cliDestino.codvw = c.codvwdestino WHERE c.status = 'Pendente' AND c.fl_exclusao IS NULL  AND ISDATE(data_hora) = 1 ";

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

        public static DataTable FetchDataTableCargasMatriz()
        {
            // alterado a query para verifica a coluna exclusao para itens excluídos            
            string sql = "SELECT c.id, c.carga, c.previsao, c.status, c.codorigem, c.cliorigem, c.cidorigem, c.ufcliorigem, c.coddestino, c.clidestino, c.ciddestino, c.ufclidestino, c.cod_expedidor, c.expedidor, c.cid_expedidor, c.uf_expedidor, c.cod_recebedor, c.recebedor, cc.id_recebedor, c.uf_recebedor, c.cod_consignatario, c.consignatario, c.cid_consignatario, c.uf_consignatario, ccod_pagador, c.pagador, c.cid_pagador, c.uf_pagador, c.material, c.peso, c.entrega, c.codmot FROM tbcargas c  WHERE c.status = 'Pendente' AND c.fl_exclusao IS NULL and empresa = '1111'";

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
        public static DataTable FetchDataTableCargasMatriz2(string searchTerm)
        {
            string sql = "SELECT c.id, c.carga, c.previsao, c.status, c.codorigem, c.cliorigem, c.coddestino, c.clidestino, c.material, c.peso, c.entrega, c.codmot, c.cod_expedidor, c.expedidor, c.cid_expedidor, c.uf_expedidor, c.cod_recebedor, c.recebedor, c.cid_recebedor, c.uf_recebedor, c.cod_pagador, c.pagador, c.cid_pagador, c.uf_pagador FROM tbcargas c  WHERE c.status = 'Pendente' AND c.fl_exclusao IS NULL and c.idviagem = @searchTerm";
            
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
        public static ConsultaCarga CheckCargasMatriz(ConsultaCarga carga)
        {
            // alterado a query para verifica a coluna exclusao para itens excluídos            
            string sqlQuery = "SELECT * from tbcargas WHERE (carga=@carga)";

            using (var con = ConnectionUtil.GetConnection())
            {
                return con.Query<ConsultaCarga>(sqlQuery, carga).FirstOrDefault();
            }
        }
        public static DataTable FetchDataTableColetasMatriz4(string idviagem)
        {
            // alterado a query para verifica a coluna exclusao para itens excluídos            
            string sql = "SELECT id, carga, cva, codorigem, cliorigem, cidorigem, ufcliorigem, cod_expedidor, expedidor, cid_expedidor, uf_expedidor, coddestino, clidestino, ciddestino, ufclidestino, cod_recebedor, recebedor, cid_recebedor, uf_recebedor,cod_consignatario, consignatario, cid_consignatario, uf_consignatario, cod_pagador, pagador, cid_pagador, uf_pagador, data_hora, deslocamento, distancia, emitepedagio, duracao,solicitante, gr, entrega, material, centro_custo_solicitacao,conta_debito_solicitacao, tipo_solicitacao, tipo_geracao_solicitacao, tipo_veiculo_solicitacao, ot, cva, disponivel_solicitacao, data_hora_coleta,   atendimento,tipo_viagem,rota,veiculo,quant_palet,peso,pedidos,solicitacoes,estudo_rota,remessa,gate_origem, idpedagio, valorpedagio, historicopedagio, creditopedagio, pagadorpedagioida, pagadorpedagiovolta, dtemissaopedagio, doc_pedagio, gate_destino,prev_chegada,status,chegadaorigem,saidaorigem,tempoagcarreg,chegadadestino,entradaplanta,saidaplanta,tempodentroplanta, tempoesperagate,previsao, conta_debito_solicitacao, centro_custo_solicitacao, desc_veic_vw, emissao,tempoesperagate,previsao,tempoagdescarreg, duracao_transp, rede,catraca,observacao,mdfe,(select num_documento from tbnfse where idviagem=carga) as num_nfse FROM tbcargas WHERE fl_exclusao is null and idviagem=@idviagem order by saidaorigem";

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

        public static DataTable FetchDataTableColetas()
        {
            // alterado a query para verifica a coluna exclusao para itens excluídos            
            string sql = @"SELECT 
                                c.id, c.carga, c.cva,  data_hora,
                                (co.codvw + '/' + co.codcli) AS CodigoO,
                                c.cliorigem,
                                (cd.codvw + '/' + cd.codcli) AS CodigoD,
                                c.clidestino,
                                c.atendimento, c.tipo_viagem, c.rota, c.veiculo,
                                c.quant_palet, c.peso, c.pedidos, c.solicitacoes,
                                c.estudo_rota, c.remessa, c.cva, c.gate, c.status,
                                c.chegadaorigem, c.saidaorigem, c.tempoagcarreg,
                                c.chegadadestino, c.entradaplanta, c.saidaplanta,
                                c.tempodentroplanta, c.tempoesperagate,c.frota,
                                c.previsao, c.andamento,c.codmot, cm.nommot, cv.plavei,c.mdfe
                            FROM tbcargas c
                            OUTER APPLY (
                                SELECT TOP 1 codvw, codcli FROM tbclientes WHERE codvw = c.codvworigem
                            ) co
                            OUTER APPLY (
                                SELECT TOP 1 codvw, codcli FROM tbclientes WHERE codvw = c.codvwdestino
                            ) cd 
                             OUTER APPLY (
                                SELECT TOP 1 nommot FROM tbmotoristas WHERE codmot = c.codmot
                            ) cm
                            OUTER APPLY (
                                SELECT TOP 1 plavei FROM tbveiculos WHERE codvei = c.frota
                            ) cv
                            WHERE c.empresa = 'CNT (CC)' AND c.fl_exclusao IS NULL and data_hora between @datainicial and @datafinal  AND ISDATE(data_hora) = 1 and codmot is not null
                            ";


            using (var con = ConnectionUtil.GetConnection())
            {
               
                using (var cmd = con.CreateCommand())
                {
                    DateTime dtInicial, dtFinal;
                    cmd.Parameters.AddWithValue("@datainicial", DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 00:01"));
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
        SELECT c.ID, c.carga, c.cva, c.data_hora, c.solicitacoes, c.cliorigem, c.clidestino, c.veiculo, c.tipo_viagem, c.rota, c.atendimento, c.andamento,c.codmot,c.frota, cm.nommot, cv.plavei,c.mdfe 
        FROM tbcargas c
        OUTER APPLY (
            SELECT TOP 1 nommot FROM tbmotoristas WHERE codmot = c.codmot
        ) cm
        OUTER APPLY (
            SELECT TOP 1 plavei FROM tbveiculos WHERE codvei = c.frota
        ) cv
        WHERE fl_exclusao IS NULL  AND ISDATE(data_hora) = 1 and codmot is not null");

            using (var con = ConnectionUtil.GetConnection())
            using (var cmd = con.CreateCommand())
            {
                // Filtro por searchTerm (opcional)
                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    sql.Append(@"
                AND (
                    c.data_hora LIKE @searchTerm OR 
                    c.solicitacoes LIKE @searchTerm OR 
                    c.veiculo LIKE @searchTerm OR 
                    c.tipo_viagem LIKE @searchTerm OR 
                    c.cliorigem LIKE @searchTerm OR 
                    c.clidestino LIKE @searchTerm OR 
                    c.cva LIKE @searchTerm
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
            //string sql = "SELECT id, carga,cva,CONVERT(varchar, CAST(data_hora AS datetime), 103) + ' ' + CONVERT(varchar, CAST(data_hora AS datetime), 108) AS data_hora,  (select top 1 codvw+ '/'+ codcli from tbclientes where codvw=codvworigem) as CodigoO ,cliorigem, (select top 1 codvw+ '/'+ codcli from tbclientes where codvw=codvwdestino) as CodigoD,clidestino,atendimento,tipo_viagem,rota,veiculo,quant_palet,peso,pedidos,solicitacoes,estudo_rota,remessa,cva,gate,status,chegadaorigem,saidaorigem,tempoagcarreg,chegadadestino,entradaplanta,saidaplanta,tempodentroplanta, tempoesperagate,previsao,emissao,empresa,codvworigem,codvwdestino,cadastro,tomador, atualizacao,cliorigem,cidorigem,clidestino,ciddestino, solicitante,andamento,codmot FROM tbcargas WHERE andamento = 'PENDENTE' and status = 'Pendente' and empresa = 'CNT (CC)' and fl_exclusao is null and cva=@searchTerm  AND ISDATE(data_hora) = 1";

            string sql = "SELECT id, carga,cva,data_hora,  (select top 1 codvw+ '/'+ codcli from tbclientes where codvw=codvworigem) as CodigoO ,cliorigem, (select top 1 codvw+ '/'+ codcli from tbclientes where codvw=codvwdestino) as CodigoD,clidestino,atendimento,tipo_viagem,rota,veiculo,quant_palet,peso,pedidos,solicitacoes,estudo_rota,remessa,cva,gate,status,chegadaorigem,saidaorigem,tempoagcarreg,chegadadestino,entradaplanta,saidaplanta,tempodentroplanta, tempoesperagate,previsao,emissao,empresa,codvworigem,codvwdestino,cadastro,tomador, atualizacao,cliorigem,cidorigem,clidestino,ciddestino, solicitante,andamento,codmot,mdfe FROM tbcargas WHERE andamento = 'PENDENTE' and status = 'Pendente' and empresa = 'CNT (CC)' and fl_exclusao is null and cva=@searchTerm  AND ISDATE(data_hora) = 1";

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
            string sql = "SELECT id, carga,cva, data_hora,  (select top 1  codvw+ '/' + codcli from tbclientes where codvw=codvworigem) as CodigoO ,cliorigem, (select top 1 codvw+ '/'+ codcli from tbclientes where codvw=codvwdestino) as CodigoD,clidestino,atendimento,tipo_viagem,rota,veiculo,quant_palet,peso,pedidos,solicitacoes,estudo_rota,remessa,cva,gate,status,chegadaorigem,saidaorigem,tempoagcarreg,chegadadestino,entradaplanta,saidaplanta,tempodentroplanta, tempoesperagate,previsao FROM tbcargas WHERE  empresa = 'CNT (CC)' and fl_exclusao is null and idviagem=@idviagem order by data_hora ";

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
        public static DataTable FetchDataTableColetas4(string idviagem)
        {
            // alterado a query para verifica a coluna exclusao para itens excluídos            
            string sql = "SELECT id, carga,cva, data_hora,  (select top 1  codvw+ '/' + codcli from tbclientes where codvw=codvworigem) as CodigoO ,cliorigem, (select top 1 codvw+ '/'+ codcli from tbclientes where codvw=codvwdestino) as CodigoD,clidestino,atendimento,tipo_viagem,rota,veiculo,quant_palet,peso,pedidos,solicitacoes,estudo_rota,remessa,cva,gate,status,chegadaorigem,saidaorigem,tempoagcarreg,chegadadestino,entradaplanta,saidaplanta,tempodentroplanta, tempoesperagate,previsao,tempoagdescarreg,mdfe FROM tbcargas WHERE fl_exclusao is null and idviagem=@idviagem order by data_hora ";

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
            string query = $"SELECT carga,cva, data_hora,  (select codvw+ '/'+ codcli from tbclientes where codvw=codvworigem) as CodigoO ,cliorigem, (select codvw+ '/'+ codcli from tbclientes where codvw=codvwdestino) as CodigoD,clidestino,atendimento,tipo_viagem,rota,veiculo,quant_palet,peso,pedidos,solicitacoes,estudo_rota,remessa,cva,gate,status,chegadaorigem,saidaorigem,tempoagcarreg,chegadadestino,entradaplanta,saidaplanta,tempodentroplanta, tempoesperagate,previsao,codmot,mdfe FROM tbcargas WHERE status = 'Pendente' and empresa = 'CNT (CC)' and fl_exclusao is null and carga IN ({listaCargas})  AND ISDATE(data_hora) = 1 order by data_hora ";

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
