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
        public static ConsultaFornecedor CheckFornecedor(ConsultaFornecedor obj)
        {
            string sqlQuery = "SELECT * FROM tbfornecedores WHERE (codfor = @codfor)";

            using (var con = ConnectionUtil.GetConnection())
            {
                return con.Query<ConsultaFornecedor>(sqlQuery, obj).FirstOrDefault();
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

        public static int RegistrarLogin(int codUsuario)
        {
            int idGerado = 0;

            // 1. Insere o log na tabela de sessões
            // 2. Atualiza a coluna dt_ultimo_login na tabela tb_usuario com a data/hora atual
            // 3. Retorna o ID gerado da sessão
            string sql = @"
        INSERT INTO LogSessoes (cod_usuario, dt_login) VALUES (@cod, GETDATE());
        
        UPDATE tb_usuario 
        SET dt_ultimo_acesso= GETDATE() 
        WHERE cod_usuario = @cod;

        SELECT SCOPE_IDENTITY();";

            using (var con = ConnectionUtil.GetConnection())
            {
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.Add("@cod", SqlDbType.Int).Value = codUsuario;

                if (con.State == ConnectionState.Closed) con.Open();

                object result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    idGerado = Convert.ToInt32(result);
                }
            }
            return idGerado;
        }

        public static void RegistrarLogout(object idLog)
        {
            if (idLog == null) return;

            // Use try-catch aqui para não deixar o erro passar em branco
            try
            {
                using (var con = ConnectionUtil.GetConnection())
                {
                    string sql = "UPDATE LogSessoes SET dt_logout = GETDATE() WHERE id_log = @idLog";
                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@idLog", idLog);

                    if (con.State == ConnectionState.Closed) con.Open();
                    int linhasAfetadas = cmd.ExecuteNonQuery();

                    // Se linhasAfetadas for 0, o ID passado não existe na tabela
                }
            }
            catch (Exception ex)
            {
                // Se der erro de banco, ele vai estourar aqui
                throw new Exception("Falha no Banco: " + ex.Message);
            }
        }

        public static List<int> ObterPermissoesUsuario(int idUsuario, string arquivoPhysical)
        {
            List<int> acoes = new List<int>();

            // Query que cruza a tabela de permissões com a tabela de cadastro de telas
            string sql = @"SELECT up.IdAcao 
                   FROM Usuario_permissao up
                   INNER JOIN Cad_Telas t ON up.IdTela = t.IdTela
                   WHERE up.IdUsuario = @IdUsuario 
                     AND t.ArquivoPhysical = @ArquivoPhysical";

            using (SqlConnection conn = ConnectionUtil.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = idUsuario;
                    cmd.Parameters.Add("@ArquivoPhysical", SqlDbType.VarChar, 100).Value = arquivoPhysical;

                    try
                    {
                        // CORREÇÃO CRÍTICA: Só abre se o estado atual for Fechado (Closed)
                        if (conn.State == ConnectionState.Closed)
                        {
                            conn.Open();
                        }

                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                acoes.Add(Convert.ToInt32(rdr["IdAcao"]));
                            }
                        }

                        // Nota: O método conn.Close() não é estritamente obrigatório aqui dentro 
                        // porque o 'using' da SqlConnection vai descartar/fechar ela automaticamente ao sair.
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Erro ao obter permissões do usuário: " + ex.Message);
                    }
                }
            }

            return acoes;
        }
    }
}
   

