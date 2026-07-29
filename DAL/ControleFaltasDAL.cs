using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Data;
using System.Data.SqlClient;
using Domain;

namespace DAL
{
    public class ControleFaltasDAL
    {
        private readonly string conexao = WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString;
        public ResultadoLancamentoDTO SalvarPeriodo(List<ControleFaltasDTO> lista)
        {
            ResultadoLancamentoDTO resultado = new ResultadoLancamentoDTO();

            using (SqlConnection conn = new SqlConnection(conexao))
            {
                conn.Open();

                SqlTransaction trans = conn.BeginTransaction();

                try
                {
                    foreach (ControleFaltasDTO dto in lista)
                    {
                        string motivoExistente;
                        if (Existe(conn, trans, dto.CodMot, dto.DataFalta, out motivoExistente))
                        {
                            resultado.DatasDuplicadas.Add(new ControleFaltasDTO
                            {
                                DataFalta = dto.DataFalta,
                                Motivo = motivoExistente
                            });

                            continue;
                        }

                        string sql = @"

                        INSERT INTO tbControleFaltas
                        (
                            codmot,
                            nommot,
                            funcao,
                            nucleo,
                            data_falta,
                            motivo,
                            observacao,
                            usuario,
                            data_cadastro
                        )
                        VALUES
                        (
                            @codmot,
                            @nommot,
                            @funcao,
                            @nucleo,
                            @data,
                            @motivo,
                            @obs,
                            @usuario,
                            GETDATE()
                        )";

                        SqlCommand cmd = new SqlCommand(sql, conn, trans);

                        cmd.Parameters.AddWithValue("@codmot", dto.CodMot);
                        cmd.Parameters.AddWithValue("@nommot", dto.NomMot);
                        cmd.Parameters.AddWithValue("@funcao", dto.Funcao);
                        cmd.Parameters.AddWithValue("@nucleo", dto.Nucleo);
                        cmd.Parameters.AddWithValue("@data", dto.DataFalta.Date);
                        cmd.Parameters.AddWithValue("@motivo", dto.Motivo);
                        cmd.Parameters.AddWithValue("@obs", dto.Observacao ?? "");
                        cmd.Parameters.AddWithValue("@usuario", dto.Usuario ?? "");

                        cmd.ExecuteNonQuery();

                        string sqlHistorico = @"
                        UPDATE tbmotoristas
                        SET historico =
                        CASE
                            WHEN historico IS NULL OR LTRIM(RTRIM(historico)) = ''
                                THEN @texto
                            ELSE historico + CHAR(13) + CHAR(10) + @texto
                        END
                        WHERE codmot = @codmot";
                        SqlCommand cmdHistorico = new SqlCommand(sqlHistorico, conn, trans);
                        cmdHistorico.Parameters.Add("@texto", SqlDbType.VarChar).Value =
                            dto.DataFalta.ToString("dd/MM/yyyy") + " - " + dto.Motivo + ".";
                        cmdHistorico.Parameters.Add("@codmot", SqlDbType.NVarChar).Value =
                            dto.CodMot;
                        cmdHistorico.ExecuteNonQuery();

                        resultado.Gravados++;
                    }

                    trans.Commit();
                }
                catch
                {
                    trans.Rollback();
                    throw;
                }
            }

            return resultado;
        }
        private bool Existe(
        SqlConnection conn,
        SqlTransaction trans,
        string codmot,
        DateTime data,
        out string motivoExistente)
        {
            motivoExistente = "";

            string sql = @"
            SELECT motivo
            FROM tbControleFaltas
            WHERE codmot = @codmot
            AND data_falta = @data";

            SqlCommand cmd = new SqlCommand(sql, conn, trans);

            cmd.Parameters.Add("@codmot", SqlDbType.NVarChar, 10).Value = codmot;
            cmd.Parameters.Add("@data", SqlDbType.Date).Value = data.Date;

            object retorno = cmd.ExecuteScalar();

            if (retorno != null)
            {
                motivoExistente = retorno.ToString();
                return true;
            }

            return false;
        }
        public void Excluir(int id)
        {
            using (SqlConnection conn = new SqlConnection(conexao))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM tbControleFaltas WHERE id=@id",
                        conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }
        public void Atualizar(ControleFaltasDTO dto)
        {
            using (SqlConnection conn = new SqlConnection(conexao))
            {
                conn.Open();
                string sql = @"
                UPDATE tbControleFaltas
                SET
                motivo=@motivo,
                observacao=@observacao,
                usuario=@usuario
                WHERE id=@id";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", dto.Id);
                cmd.Parameters.AddWithValue("@motivo", dto.Motivo);
                cmd.Parameters.AddWithValue("@observacao", dto.Observacao ?? "");
                cmd.Parameters.AddWithValue("@usuario", dto.Usuario ?? "");
                cmd.ExecuteNonQuery();
            }
        }
        public ControleFaltasDTO ObterPorId(int id)
        {
            using (SqlConnection conn =
                new SqlConnection(conexao))
            {
                conn.Open();

                string sql =
                    "SELECT * FROM tbControleFaltas WHERE id=@id";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader dr = cmd.ExecuteReader();
                if (!dr.Read())
                    return null;

                ControleFaltasDTO dto = new ControleFaltasDTO();
                dto.Id = Convert.ToInt32(dr["id"]);
                dto.CodMot = dr["codmot"].ToString();
                dto.NomMot = dr["nommot"].ToString();
                dto.Funcao = dr["funcao"].ToString();
                dto.Nucleo = dr["nucleo"].ToString();
                dto.DataFalta = Convert.ToDateTime(dr["data_falta"]);
                dto.Motivo = dr["motivo"].ToString();
                dto.Observacao = dr["observacao"].ToString();
                dto.Usuario = dr["usuario"].ToString();
                return dto;
            }
        }
        public DataTable Listar(DateTime dataInicial,
                        DateTime dataFinal,
                        string codmot,
                        string nucleo)
        {
            using (SqlConnection conn = new SqlConnection(conexao))
            {
                StringBuilder sql = new StringBuilder();

                sql.Append(@"
            SELECT
                id,
                data_falta,
                codmot,
                nommot,
                funcao,
                nucleo,
                motivo,
                observacao,
                usuario
            FROM tbControleFaltas
            WHERE data_falta BETWEEN @inicio AND @fim
        ");

                if (!string.IsNullOrWhiteSpace(codmot) && codmot != "0")
                {
                    sql.Append(" AND codmot = @codmot");
                }

                if (!string.IsNullOrWhiteSpace(nucleo))
                {
                    sql.Append(" AND nucleo = @nucleo");
                }

                sql.Append(" ORDER BY data_falta, nommot");

                SqlDataAdapter da = new SqlDataAdapter(sql.ToString(), conn);

                da.SelectCommand.Parameters.Add("@inicio", SqlDbType.Date).Value = dataInicial.Date;
                da.SelectCommand.Parameters.Add("@fim", SqlDbType.Date).Value = dataFinal.Date;

                if (!string.IsNullOrWhiteSpace(codmot) && codmot != "0")
                {
                    da.SelectCommand.Parameters.Add("@codmot", SqlDbType.NVarChar, 10).Value = codmot;
                }

                if (!string.IsNullOrWhiteSpace(nucleo))
                {
                    da.SelectCommand.Parameters.Add("@nucleo", SqlDbType.NVarChar, 30).Value = nucleo;
                }

                DataTable dt = new DataTable();
                da.Fill(dt);

                return dt;
            }
        }


    }
}
