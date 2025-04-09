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

        public static DataTable FetchDataTableColetas2(string searchTerm)
        {
            // alterado a query para verifica a coluna exclusao para itens excluídos            
            string sql = "SELECT carga,cva,data_hora,  (select codvw+ '/'+ codcli from tbclientes where codvw=codvworigem) as CodigoO ,cliorigem, (select codvw+ '/'+ codcli from tbclientes where codvw=codvwdestino) as CodigoD,clidestino,atendimento,tipo_viagem,rota,veiculo,quant_palet,peso,pedidos,solicitacoes,estudo_rota,remessa,cva,gate,status,chegadaorigem,saidaorigem,tempoagcarreg,chegadadestino,entradaplanta,saidaplanta,tempodentroplanta, tempoesperagate FROM tbcargas WHERE status = 'Pendente' and empresa = 'CNT' and fl_exclusao is null and carga=@searchTerm";

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
            string query = $"SELECT carga,cva,data_hora,  (select codvw+ '/'+ codcli from tbclientes where codvw=codvworigem) as CodigoO ,cliorigem, (select codvw+ '/'+ codcli from tbclientes where codvw=codvwdestino) as CodigoD,clidestino,atendimento,tipo_viagem,rota,veiculo,quant_palet,peso,pedidos,solicitacoes,estudo_rota,remessa,cva,gate,status,chegadaorigem,saidaorigem,tempoagcarreg,chegadadestino,entradaplanta,saidaplanta,tempodentroplanta, tempoesperagate FROM tbcargas WHERE status = 'Pendente' and empresa = 'CNT' and fl_exclusao is null and carga IN ({listaCargas})";

            using (var con = ConnectionUtil.GetConnection())
            {
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

    }
}
