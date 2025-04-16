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
            // alterado a query para verifica a coluna exclusao para itens excluídos            
            string sql = "select cva,  '../../fotos/'+ REPLACE(m.caminhofoto, '/fotos/', '') AS fotos, c.nomemotorista, c.tipoveiculo,c.veiculo, (c.placa+' / '+c.reboque1+' / '+c.reboque2) as veiculo_reboques,c.carga,c.cva,c.nomcliorigem,c.nomclidestino,c.situacao,c.material, c.dtcad, c.num_carregamento from tbcarregamentos as c inner join tbmotoristas as m on c.codmotorista=m.codmot where empresa='CNT' and situacao <> 'VIAGEM CONCLUIDA' order by c.dtcad desc ";

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
