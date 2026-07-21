using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class EmpresaDAL
    {
        private readonly string conexao =
            ConfigurationManager.ConnectionStrings["Conexao"].ConnectionString;
        public DataTable Listar()
        {
            DataTable dt = new DataTable();

            using (SqlConnection cn = new SqlConnection(conexao))
            {
                string sql = @"

                SELECT                   
                    codigo_empresa,
                    descricao,
                    razao_social,
                    nome_fantasia,
                    cnpj,
                    status

                FROM tbempresa

                ORDER BY descricao";

                using (SqlDataAdapter da = new SqlDataAdapter(sql, cn))
                {
                    da.Fill(dt);
                }
            }

            return dt;
        }
        public Domain.EmpresaDTO Obter(int codigo)
        {
            Domain.EmpresaDTO empresa = new Domain.EmpresaDTO();

            using (SqlConnection cn = new SqlConnection(conexao))
            {
                cn.Open();
                string sql = @"
                SELECT *
                FROM tbempresa
                WHERE codigo_empresa=@codigo";

                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.Parameters.Add("@codigo", SqlDbType.Int).Value = codigo;
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {                    
                    empresa.CodigoEmpresa = dr["codigo_empresa"].ToString();                   
                    empresa.RazaoSocial = dr["razao_social"].ToString();
                    empresa.NomeFantasia = dr["nome_fantasia"].ToString();
                    empresa.CNPJ = dr["cnpj"].ToString();
                    empresa.InscricaoEstadual = dr["inscricao_estadual"].ToString();
                    empresa.CodigoMunicipal = dr["codigo_municipal"].ToString();
                    empresa.Endereco = dr["endereco"].ToString();
                    empresa.CEP = dr["cep"].ToString();
                    empresa.Bairro = dr["bairro"].ToString();
                    empresa.Municipio = dr["municipio"].ToString();
                    empresa.UF = dr["uf"].ToString();
                    empresa.UFNome = dr["uf_nome"].ToString();
                    empresa.Telefone = dr["telefone"].ToString();
                    empresa.Modal = dr["modal"].ToString();
                    empresa.Numero = dr["numero"].ToString();
                    empresa.RNTRC = dr["rntrc"].ToString();
                    empresa.Logo = dr["logo"].ToString();
                    empresa.Status = dr["status"].ToString();

                    if (dr["abertura"] != DBNull.Value)
                        empresa.Abertura = Convert.ToDateTime(dr["abertura"]);
                }

                dr.Close();
            }

            return empresa;
        }
        public bool EmpresaExiste(int codigo)
        {
            bool existe = false;

            string sql = @"
            SELECT COUNT(1)
            FROM tbempresa
            WHERE codigo_empresa = @codigo";
            using (SqlConnection cn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@codigo", codigo);
                    cn.Open();
                    existe = Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                }
            }

            return existe;
        }
        public bool ExisteCNPJ(string cnpj, int codigo)
        {
            using (SqlConnection cn = new SqlConnection(conexao))
            {
                cn.Open();

                string sql = @"
                SELECT COUNT(*)
                FROM tbempresa
                WHERE cnpj = @cnpj
                  AND codigo_empresa <> @codigo";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.Add("@cnpj", SqlDbType.VarChar).Value = cnpj;
                    cmd.Parameters.Add("@codigo", SqlDbType.Int).Value = codigo;

                    return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                }
            }
        }
        public void Excluir(int codigo)
        {
            using (SqlConnection cn = new SqlConnection(conexao))
            {
                cn.Open();

                string sql = "DELETE FROM tbempresa WHERE codigo_empresa=@codigo";

                SqlCommand cmd = new SqlCommand(sql, cn);

                cmd.Parameters.Add("@codigo", SqlDbType.Int).Value = codigo;

                cmd.ExecuteNonQuery();
            }
        }
        public int Salvar(Domain.EmpresaDTO empresa)
        {
            using (SqlConnection cn = new SqlConnection(conexao))
            {
                cn.Open();

                SqlTransaction trans = cn.BeginTransaction();

                try
                {
                    SqlCommand cmd = new SqlCommand();

                    cmd.Connection = cn;
                    cmd.Transaction = trans;

                    if (empresa.Codigo == 0)
                    {
                        cmd.CommandText = @"

                        INSERT INTO tbempresa
                        (
                            codigo_empresa,                            
                            razao_social,
                            nome_fantasia,
                            cnpj,
                            inscricao_estadual,
                            codigo_municipal,
                            endereco,
                            cep,
                            bairro,
                            municipio,
                            uf,
                            uf_nome,
                            telefone,
                            modal,
                            numero,
                            rntrc,
                            logo,
                            abertura,
                            status
                        )

                        VALUES
                        (
                            @codigo_empresa,                            
                            @razao_social,
                            @nome_fantasia,
                            @cnpj,
                            @inscricao_estadual,
                            @codigo_municipal,
                            @endereco,
                            @cep,
                            @bairro,
                            @municipio,
                            @uf,
                            @uf_nome,
                            @telefone,
                            @modal,
                            @numero,
                            @rntrc,
                            @logo,
                            @abertura,
                            @status
                        );

                        SELECT SCOPE_IDENTITY();
                        ";
                    }
                    else
                    {
                        cmd.CommandText = @"
                        UPDATE tbempresa SET
                        codigo_empresa=@codigo_empresa,  
                        razao_social=@razao_social,
                        nome_fantasia=@nome_fantasia,
                        cnpj=@cnpj,
                        inscricao_estadual=@inscricao_estadual,
                        codigo_municipal=@codigo_municipal,
                        endereco=@endereco,
                        cep=@cep,
                        bairro=@bairro,
                        municipio=@municipio,
                        uf=@uf,
                        uf_nome=@uf_nome,
                        telefone=@telefone,
                        modal=@modal,
                        numero=@numero,
                        rntrc=@rntrc,
                        logo=@logo,
                        abertura=@abertura,
                        status=@status
                        WHERE codigo_empresa=@codigo;
                        ";
                    }

                    AdicionarParametros(cmd, empresa);

                    if (empresa.Codigo == 0)
                    {
                        empresa.Codigo = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    else
                    {
                        cmd.ExecuteNonQuery();
                    }

                    trans.Commit();

                    return empresa.Codigo;
                }
                catch
                {
                    trans.Rollback();
                    throw;
                }
            }
        }
        private void AdicionarParametros(SqlCommand cmd, Domain.EmpresaDTO empresa)
        {
            cmd.Parameters.Clear();
            cmd.Parameters.Add("@codigo_empresa", SqlDbType.VarChar).Value =
                (object)empresa.CodigoEmpresa ?? DBNull.Value;            
            cmd.Parameters.Add("@razao_social", SqlDbType.VarChar).Value =
                (object)empresa.RazaoSocial ?? DBNull.Value;
            cmd.Parameters.Add("@nome_fantasia", SqlDbType.VarChar).Value =
                (object)empresa.NomeFantasia ?? DBNull.Value;
            cmd.Parameters.Add("@cnpj", SqlDbType.VarChar).Value =
                (object)empresa.CNPJ ?? DBNull.Value;
            cmd.Parameters.Add("@inscricao_estadual", SqlDbType.VarChar).Value =
                (object)empresa.InscricaoEstadual ?? DBNull.Value;
            cmd.Parameters.Add("@codigo_municipal", SqlDbType.VarChar).Value =
                (object)empresa.CodigoMunicipal ?? DBNull.Value;
            cmd.Parameters.Add("@endereco", SqlDbType.VarChar).Value =
                (object)empresa.Endereco ?? DBNull.Value;
            cmd.Parameters.Add("@cep", SqlDbType.VarChar).Value =
                (object)empresa.CEP ?? DBNull.Value;
            cmd.Parameters.Add("@bairro", SqlDbType.VarChar).Value =
                (object)empresa.Bairro ?? DBNull.Value;
            cmd.Parameters.Add("@municipio", SqlDbType.VarChar).Value =
                (object)empresa.Municipio ?? DBNull.Value;
            cmd.Parameters.Add("@uf", SqlDbType.VarChar).Value =
                (object)empresa.UF ?? DBNull.Value;
            cmd.Parameters.Add("@uf_nome", SqlDbType.VarChar).Value =
                (object)empresa.UFNome ?? DBNull.Value;
            cmd.Parameters.Add("@telefone", SqlDbType.VarChar).Value =
                (object)empresa.Telefone ?? DBNull.Value;
            cmd.Parameters.Add("@modal", SqlDbType.VarChar).Value =
                (object)empresa.Modal ?? DBNull.Value;
            cmd.Parameters.Add("@numero", SqlDbType.VarChar).Value =
                (object)empresa.Numero ?? DBNull.Value;
            cmd.Parameters.Add("@rntrc", SqlDbType.VarChar).Value =
                (object)empresa.RNTRC ?? DBNull.Value;
            cmd.Parameters.Add("@logo", SqlDbType.VarChar).Value =
                (object)empresa.Logo ?? DBNull.Value;
            cmd.Parameters.Add("@abertura", SqlDbType.Date).Value =
                empresa.Abertura == DateTime.MinValue
                    ? DBNull.Value
                    : (object)empresa.Abertura;
            cmd.Parameters.Add("@status", SqlDbType.VarChar).Value =
                (object)empresa.Status ?? DBNull.Value;
        }
        public void AtualizarLogo(int codigo, string logo)
        {
            using (SqlConnection cn = new SqlConnection(conexao))
            {
                cn.Open();

                string sql = @"
                UPDATE tbempresa
                SET logo=@logo
                WHERE codigo=@codigo";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.Add("@codigo", SqlDbType.Int).Value = codigo;
                    cmd.Parameters.Add("@logo", SqlDbType.VarChar).Value =
                        (object)logo ?? DBNull.Value;

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
