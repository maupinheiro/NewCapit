using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class UsersDAL
    {
        public static Users CheckLogin(Users obj) 
        {
            string sqlQuery = "SELECT * FROM tb_usuario WHERE (nm_usuario = @nm_usuario) AND (ds_senha = @ds_senha)";

            using (var con = ConnectionUtil.GetConnection())
            {
                return con.Query<Users>(sqlQuery, obj).FirstOrDefault();
            }
        }

        public static DataTable FetchDataTable()
        {
            string sql = "SELECT id, codcli, tipo, nomcli, unidade, regiao, cidcli, estcli, dtccli, ativo_inativo FROM tbclientes";
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
        
        public static ConsultaCliente CheckCliente(ConsultaCliente obj)
        {
            string sqlQuery = "SELECT * FROM tbclientes WHERE (codcli = @codcli)";

            using (var con = ConnectionUtil.GetConnection())
            {
                return con.Query<ConsultaCliente>(sqlQuery, obj).FirstOrDefault();
            }
        }
        public static ConsultaContato CheckContato(ConsultaContato obj)
        {
            string sqlQuery = "SELECT * FROM tbfoneveiculos WHERE (veiculo = @veiculo)";

            using (var con = ConnectionUtil.GetConnection())
            {
                return con.Query<ConsultaContato>(sqlQuery, obj).FirstOrDefault();
            }
        }
        public static ConsultaMotorista CheckMotorista(ConsultaMotorista obj)
        {
            string sqlQuery = "SELECT * FROM tbmotoristas WHERE (codmot = @codmot)";

            using (var con = ConnectionUtil.GetConnection())
            {
                return con.Query<ConsultaMotorista>(sqlQuery, obj).FirstOrDefault();
            }
        }

        public static ConsultaPedido CheckPedido(ConsultaPedido obj)
        {
            string sqlQuery = "SELECT * FROM tbpedidos WHERE (pedido = @pedido)";

            using (var con = ConnectionUtil.GetConnection())
            {
                return con.Query<ConsultaPedido>(sqlQuery, obj).FirstOrDefault();
            }
        }



        public static ConsultaAgregado CheckAgregado(ConsultaAgregado obj)
        {
            string sqlQuery = "SELECT * FROM tbtransportadoras WHERE (codtra = @codtra)";

            using (var con = ConnectionUtil.GetConnection())
            {
                return con.Query<ConsultaAgregado>(sqlQuery, obj).FirstOrDefault();
            }
        }
        public static ConsultaVeiculo CheckVeiculo(ConsultaVeiculo obj)
        {
            string sqlQuery = "SELECT * FROM tbveiculos WHERE (codvei = @codvei)";

            using (var con = ConnectionUtil.GetConnection())
            {
                return con.Query<ConsultaVeiculo>(sqlQuery, obj).FirstOrDefault();
            }
        }
        public static ConsultaReboque CheckReboque(ConsultaReboque objReboque)
        {
            string sqlQuery = "SELECT * FROM tbcarretas WHERE (placacarreta = @placaCarreta)";

            using (var con = ConnectionUtil.GetConnection())
            {
                return con.Query<ConsultaReboque>(sqlQuery, objReboque).FirstOrDefault();
            }
        }
    }
   
}
