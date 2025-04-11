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
            string sql = "SELECT id, carga,cva,data_hora,  (select codvw+ '/'+ codcli from tbclientes where codvw=codvworigem) as CodigoO ,cliorigem, (select codvw+ '/'+ codcli from tbclientes where codvw=codvwdestino) as CodigoD,clidestino,atendimento,tipo_viagem,rota,veiculo,quant_palet,peso,pedidos,solicitacoes,estudo_rota,remessa,cva,gate,status,chegadaorigem,saidaorigem,tempoagcarreg,chegadadestino,entradaplanta,saidaplanta,tempodentroplanta, tempoesperagate,previsao FROM tbcargas WHERE status = 'Pendente' and fl_exclusao is null ";

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
            string sql = "SELECT c.id, c.carga, c.cva, c.data_hora, (co.codvw + '/' + co.codcli) AS CodigoO, c.cliorigem, (cd.codvw + '/' + cd.codcli) AS CodigoD, c.clidestino, c.atendimento, c.tipo_viagem, c.rota, c.veiculo, c.quant_palet, c.peso, c.pedidos, c.solicitacoes, c.estudo_rota, c.remessa, c.cva, c.gate, c.status, c.chegadaorigem, c.saidaorigem, c.tempoagcarreg, c.chegadadestino, c.entradaplanta, c.saidaplanta, c.tempodentroplanta, c.tempoesperagate, c.previsao,c.andamento FROM tbcargas c LEFT JOIN tbclientes co ON co.codvw = c.codvworigem LEFT JOIN tbclientes cd ON cd.codvw = c.codvwdestino WHERE c.status = 'Pendente' AND c.empresa = 'CNT' AND c.fl_exclusao IS NULL";


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

        public static DataTable FetchDataTableColetas2(string searchTerm)
        {
            // alterado a query para verifica a coluna exclusao para itens excluídos            
            string sql = "SELECT id, carga,cva,data_hora,  (select codvw+ '/'+ codcli from tbclientes where codvw=codvworigem) as CodigoO ,cliorigem, (select codvw+ '/'+ codcli from tbclientes where codvw=codvwdestino) as CodigoD,clidestino,atendimento,tipo_viagem,rota,veiculo,quant_palet,peso,pedidos,solicitacoes,estudo_rota,remessa,cva,gate,status,chegadaorigem,saidaorigem,tempoagcarreg,chegadadestino,entradaplanta,saidaplanta,tempodentroplanta, tempoesperagate,previsao FROM tbcargas WHERE status = 'Pendente' and empresa = 'CNT' and fl_exclusao is null and carga=@searchTerm";

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

        public static DataTable FetchDataTableColetasPorCargas(List<string> cargas)
        {
            string listaCargas = string.Join(",", cargas.Select(c => $"'{c}'"));
            string query = $"SELECT carga,cva,data_hora,  (select codvw+ '/'+ codcli from tbclientes where codvw=codvworigem) as CodigoO ,cliorigem, (select codvw+ '/'+ codcli from tbclientes where codvw=codvwdestino) as CodigoD,clidestino,atendimento,tipo_viagem,rota,veiculo,quant_palet,peso,pedidos,solicitacoes,estudo_rota,remessa,cva,gate,status,chegadaorigem,saidaorigem,tempoagcarreg,chegadadestino,entradaplanta,saidaplanta,tempodentroplanta, tempoesperagate,previsao FROM tbcargas WHERE status = 'Pendente' and empresa = 'CNT' and fl_exclusao is null and carga IN ({listaCargas})";

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
