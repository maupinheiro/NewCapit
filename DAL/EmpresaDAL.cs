using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Domain;
using System.Web.Configuration;
using System.Web;

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
                    razao_social,
                    nome_fantasia,
                    cnpj,
                    inscricao_estadual,
                    municipio,
                    uf,
                    abertura,
                    status

                FROM tbempresa

                ORDER BY nome_fantasia";

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
                    empresa.Codigo = Convert.ToInt32(dr["codigo_empresa"]); 
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
                    empresa.Complemento = dr["complemento"].ToString();
                    empresa.RNTRC = dr["rntrc"].ToString();
                    empresa.Logo = dr["logo"].ToString();
                    empresa.Status = dr["status"].ToString();
                    empresa.Tipo = dr["tipo"].ToString();
                    empresa.Situacao = dr["situacao"].ToString();
                    empresa.AtividadePrincipal = dr["atividade_principal"].ToString();
                    empresa.UsuarioCadastro = dr["usuario_cadastro"].ToString();
                    empresa.UsuarioAlteracao = dr["usuario_alteracao"].ToString();

                    if (dr["abertura"] != DBNull.Value)
                        empresa.Abertura = Convert.ToDateTime(dr["abertura"]);
                    if (dr["cadastro"] != DBNull.Value)
                        empresa.Cadastro = Convert.ToDateTime(dr["cadastro"]);
                    if (dr["data_cadastro"] != DBNull.Value)
                        empresa.DataCadastro = Convert.ToDateTime(dr["data_cadastro"]);
                    if (dr["data_alteracao"] != DBNull.Value)
                        empresa.DataAlteracao = Convert.ToDateTime(dr["data_alteracao"]);
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
        public int Salvar(EmpresaDTO empresa)
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ConnectionString))
            {
                conn.Open();

                SqlTransaction trans = conn.BeginTransaction();

                try
                {
                    // Verifica se a empresa já existe
                    bool existe;

                    using (SqlCommand cmdExiste = new SqlCommand(
                        "SELECT COUNT(*) FROM tbempresa WHERE codigo_empresa = @codigo",
                        conn, trans))
                    {
                        cmdExiste.Parameters.AddWithValue("@codigo", empresa.Codigo);

                        existe = Convert.ToInt32(cmdExiste.ExecuteScalar()) > 0;
                    }

                    if (existe)
                    {
                        // Atualiza
                        AtualizarEmpresa(conn, trans, empresa);
                    }
                    else
                    {
                        // Insere
                        InserirEmpresa(conn, trans, empresa);
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
        private void PreencherParametros(SqlCommand cmd, Domain.EmpresaDTO empresa)
        {
            cmd.Parameters.Clear();
            cmd.Parameters.Add("@codigo", SqlDbType.VarChar).Value =
                (object)empresa.Codigo ?? DBNull.Value;            
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
            cmd.Parameters.Add("@nome_uf", SqlDbType.VarChar).Value =
                (object)empresa.UFNome ?? DBNull.Value;
            cmd.Parameters.Add("@telefone", SqlDbType.VarChar).Value =
                (object)empresa.Telefone ?? DBNull.Value;
            cmd.Parameters.Add("@modal", SqlDbType.VarChar).Value =
                (object)empresa.Modal ?? DBNull.Value;
            cmd.Parameters.Add("@numero", SqlDbType.VarChar).Value =
                (object)empresa.Numero ?? DBNull.Value;
            cmd.Parameters.Add("@complemento", SqlDbType.VarChar).Value =
                (object)empresa.Complemento ?? DBNull.Value;
            cmd.Parameters.Add("@rntrc", SqlDbType.VarChar).Value =
                (object)empresa.RNTRC ?? DBNull.Value;
            cmd.Parameters.Add("@logo", SqlDbType.VarChar).Value =
                (object)empresa.Logo ?? DBNull.Value;
            cmd.Parameters.Add("@abertura", SqlDbType.Date).Value =
                empresa.Abertura.HasValue
                    ? (object)empresa.Abertura.Value
                    : DBNull.Value;
            cmd.Parameters.Add("@cadastro", SqlDbType.Date).Value =
                empresa.Cadastro == DateTime.MinValue
                    ? DBNull.Value
                    : (object)empresa.Cadastro;            
            cmd.Parameters.Add("@status", SqlDbType.VarChar).Value =
                (object)empresa.Status ?? DBNull.Value;
            cmd.Parameters.Add("@email", SqlDbType.VarChar).Value =
                (object)empresa.Email ?? DBNull.Value;
            cmd.Parameters.Add("@tipo", SqlDbType.VarChar).Value =
                (object)empresa.Tipo ?? DBNull.Value;
            cmd.Parameters.Add("@situacao", SqlDbType.VarChar).Value =
                (object)empresa.Situacao ?? DBNull.Value;
            cmd.Parameters.Add("@site", SqlDbType.VarChar).Value =
                (object)empresa.Site ?? DBNull.Value;
            cmd.Parameters.Add("@atividade_principal", SqlDbType.VarChar).Value =
                (object)empresa.AtividadePrincipal ?? DBNull.Value;    
        }
        public void AtualizarLogo(int codigo, string logo)
        {
            using (SqlConnection cn = new SqlConnection(conexao))
            {
                cn.Open();

                string sql = @"
                UPDATE tbempresa
                SET logo=@logo
                WHERE codigo_empresa=@codigo";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.Add("@codigo", SqlDbType.Int).Value = codigo;
                    cmd.Parameters.Add("@logo", SqlDbType.VarChar).Value =
                        (object)logo ?? DBNull.Value;

                    cmd.ExecuteNonQuery();
                }
            }
        }        
        private int InserirEmpresa(SqlConnection conn, SqlTransaction trans, EmpresaDTO empresa)
        {
            string sql = @"
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
                complemento,
                rntrc,
                logo,
                abertura,
                tipo,
                situacao,
                status,
                cadastro,
                atividade_principal,
                email,
                site,
                data_cadastro,
                usuario_cadastro
            )
            VALUES
            (
                @codigo,
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
                @nome_uf,
                @telefone,
                @modal,
                @numero,
                @complemento,
                @rntrc,
                @logo,
                @abertura,
                @tipo,
                @situacao,
                @status,
                @cadastro,
                @atividade_principal,
                @email,
                @site,
                @data_cadastro,
                @usuario_cadastro
            );";

            using (SqlCommand cmd = new SqlCommand(sql, conn, trans))
            {
                PreencherParametros(cmd, empresa);

                string usuario = HttpContext.Current.Session["UsuarioLogado"]?.ToString() ?? "";

                cmd.Parameters.AddWithValue("@data_cadastro", DateTime.Now);
                cmd.Parameters.AddWithValue("@usuario_cadastro", usuario);

                cmd.ExecuteNonQuery();

                return empresa.Codigo;
            }
        }
        private void AtualizarEmpresa(SqlConnection conn, SqlTransaction trans, EmpresaDTO empresa)
        {
            string sql = @"
            UPDATE tbempresa
            SET
                razao_social        = @razao_social,
                nome_fantasia       = @nome_fantasia,
                cnpj                = @cnpj,
                inscricao_estadual  = @inscricao_estadual,
                codigo_municipal    = @codigo_municipal,
                endereco            = @endereco,
                cep                 = @cep,
                bairro              = @bairro,
                municipio           = @municipio,
                uf                  = @uf,
                uf_nome             = @nome_uf,
                telefone            = @telefone,
                modal               = @modal,
                numero              = @numero,
                complemento         = @complemento,
                rntrc               = @rntrc,
                logo                = @logo,
                abertura            = @abertura,
                tipo                = @tipo,
                situacao            = @situacao,
                status              = @status,
                cadastro            = @cadastro,
                atividade_principal = @atividade_principal,
                email               = @email,
                site                = @site,
                data_alteracao      = @data_alteracao,
                usuario_alteracao   = @usuario_alteracao
            WHERE codigo_empresa = @codigo";

            using (SqlCommand cmd = new SqlCommand(sql, conn, trans))
            {
                PreencherParametros(cmd, empresa);

                string usuario = HttpContext.Current.Session["UsuarioLogado"]?.ToString() ?? "";

                cmd.Parameters.AddWithValue("@data_alteracao", DateTime.Now);
                cmd.Parameters.AddWithValue("@usuario_alteracao", usuario);

                cmd.ExecuteNonQuery();
            }
        }
    }
}
