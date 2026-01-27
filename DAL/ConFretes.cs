using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ConFretes
    {
        public static DataTable FetchDataTable()
        {
            string sql = "SELECT cod_frete, cod_pagador, pagador, cod_expedidor, expedidor, cod_recebedor, recebedor, frete_tng, frete_agregado, frete_terceiro, CONVERT(varchar, vigencia_inicial, 103) as vigencia_inicial, CONVERT(varchar, vigencia_final, 103) as vigencia_final, situacao, tipo_veiculo, cid_expedidor, uf_expedidor, cid_recebedor, uf_recebedor FROM tbtabeladefretes where fl_exclusao is null ORDER BY pagador";

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

        public static DataTable FetchDataTableLimite()
        {
            string sql = "SELECT cod_frete, cod_pagador, pagador, cod_expedidor, expedidor, cod_recebedor, recebedor, frete_tng, frete_agregado, frete_terceiro, CONVERT(varchar, vigencia_inicial, 103) as vigencia_inicial, CONVERT(varchar, vigencia_final, 103) as vigencia_final, situacao, tipo_veiculo, cid_expedidor, uf_expedidor, cid_recebedor, uf_recebedor FROM tbtabeladefretes";

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


        public static DataTable FetchDataTable2(string searchTerm)
        {
            string sql = "SELECT cod_frete, cod_pagador, pagador, cod_expedidor, expedidor, cod_recebedor, recebedor, frete_tng, frete_agregado, frete_terceiro, CONVERT(varchar, vigencia_inicial, 103) as vigencia_inicial, CONVERT(varchar, vigencia_final, 103) as vigencia_final, situacao, tipo_veiculo, cid_expedidor, uf_expedidor, cid_recebedor, uf_recebedor FROM tbtabeladefretes where fl_exclusao is null and pagador LIKE @searchTerm OR cod_frete LIKE @searchTerm OR expedidor LIKE @searchTerm OR recebedor LIKE @searchTerm OR cod_pagador LIKE @searchTerm OR cod_frete LIKE @searchTerm ORDER BY pagador";

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
    }
}
