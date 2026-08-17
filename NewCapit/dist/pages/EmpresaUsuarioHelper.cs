using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;

public static class EmpresaUsuarioHelper
{
    private static string ConnectionString
    {
        get
        {
            return ConfigurationManager
                .ConnectionStrings["conexao"]
                .ConnectionString;
        }
    }

    // =========================================================
    // CARREGA EMPRESAS
    // =========================================================
    public static void CarregarEmpresas(DropDownList ddlEmpresa)
    {
        ddlEmpresa.Items.Clear();

        ddlEmpresa.Items.Add(
            new ListItem("-- Selecione a empresa --", "")
        );

        string sql = @"
            SELECT
                codigo_empresa,
                nome_fantasia,
                logo
            FROM tbempresa WHERE status = 'ATIVO'
            ORDER BY nome_fantasia";

        using (SqlConnection conn = new SqlConnection(ConnectionString))
        using (SqlCommand cmd = new SqlCommand(sql, conn))
        {
            conn.Open();

            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    string codigo = dr["codigo_empresa"]?.ToString().Trim();
                    string nome = dr["nome_fantasia"]?.ToString().Trim();
                    string logo = dr["logo"]?.ToString().Trim();

                    ListItem item = new ListItem
                    {
                        Text = codigo + " - " + nome,
                        Value = codigo
                    };

                    item.Attributes["data-nome"] = nome;
                    item.Attributes["data-logo"] = logo;

                    ddlEmpresa.Items.Add(item);
                }
            }
        }
    }


    // =========================================================
    // CARREGA EMPRESA SALVA DO USUÁRIO
    // =========================================================
    public static bool CarregarEmpresaDoUsuario(
        DropDownList ddlEmpresa)
    {
        string usuario = ObterUsuario();

        if (string.IsNullOrWhiteSpace(usuario))
            return false;

        string sql = @"
            SELECT
                cod_empresa,
                nom_empresa
            FROM tb_usuario
            WHERE nm_usuario = @usuario";

        using (SqlConnection conn = new SqlConnection(ConnectionString))
        using (SqlCommand cmd = new SqlCommand(sql, conn))
        {
            cmd.Parameters.AddWithValue("@usuario", usuario);

            conn.Open();

            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                if (!dr.Read())
                    return false;

                string codigoEmpresa =
                    dr["cod_empresa"]?.ToString().Trim();

                string nomeEmpresa =
                    dr["nom_empresa"]?.ToString().Trim();

                if (string.IsNullOrWhiteSpace(codigoEmpresa))
                    return false;

                ListItem item =
                    ddlEmpresa.Items.FindByValue(codigoEmpresa);

                if (item == null)
                    return false;

                ddlEmpresa.SelectedValue = codigoEmpresa;

                // Guarda na sessão
                HttpContext.Current.Session["CodEmpresa"] =
                    codigoEmpresa;

                HttpContext.Current.Session["NomEmpresa"] =
                    nomeEmpresa;

                return true;
            }
        }
    }


    // =========================================================
    // SALVA EMPRESA DO USUÁRIO
    // =========================================================

    public static void SalvarEmpresaUsuario(string codigoEmpresa,string nomeEmpresa)
    {
        string usuario = ObterUsuario();

        if (string.IsNullOrWhiteSpace(usuario))
            throw new Exception("ObterUsuario() não retornou o usuário logado.");

        string sql = @"
        UPDATE tb_usuario
        SET
            cod_empresa = @cod_empresa,
            nom_empresa = @nom_empresa
        WHERE LTRIM(RTRIM(nm_usuario)) = LTRIM(RTRIM(@usuario))";

        using (SqlConnection conn = new SqlConnection(ConnectionString))
        using (SqlCommand cmd = new SqlCommand(sql, conn))
        {
            cmd.Parameters.Add("@cod_empresa", SqlDbType.VarChar).Value =
                codigoEmpresa;

            cmd.Parameters.Add("@nom_empresa", SqlDbType.VarChar).Value =
                nomeEmpresa;

            cmd.Parameters.Add("@usuario", SqlDbType.VarChar).Value =
                usuario;

            conn.Open();

            int linhas = cmd.ExecuteNonQuery();

            if (linhas == 0)
            {
                throw new Exception(
                    "UPDATE não encontrou o usuário '" +
                    usuario +
                    "' na tb_usuario.");
            }
        }

        HttpContext.Current.Session["CodEmpresa"] = codigoEmpresa;
        HttpContext.Current.Session["NomEmpresa"] = nomeEmpresa;
    }




    // =========================================================
    // OBTÉM USUÁRIO LOGADO
    // =========================================================
    private static string ObterUsuario()
    {     
        object usuario =
        HttpContext.Current.Session["UsuarioSistema"];

        if (usuario == null)
            throw new Exception(
                "Session[UsuarioSistema] está NULL.");

        string nomeUsuario = usuario.ToString().Trim();

        if (string.IsNullOrWhiteSpace(nomeUsuario))
            throw new Exception(
                "Session[UsuarioSistema] está vazia.");

        return nomeUsuario;
    }

    public static DataTable ObterEmpresaUsuario()
    {
        string usuario = ObterUsuario();

        if (string.IsNullOrWhiteSpace(usuario))
            throw new Exception("Usuário não identificado.");

        string sql = @"
        SELECT
            cod_empresa,
            nom_empresa
        FROM tb_usuario
        WHERE nm_usuario = @usuario";

        DataTable dt = new DataTable();

        using (SqlConnection conn = new SqlConnection(ConnectionString))
        using (SqlCommand cmd = new SqlCommand(sql, conn))
        {
            cmd.Parameters.AddWithValue("@usuario", usuario);

            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                da.Fill(dt);
            }
        }

        return dt;
    }



    // =========================================================
    // EMPRESA ATUAL
    // =========================================================
    public static string CodEmpresa
    {
        get
        {
            return HttpContext.Current
                .Session["CodEmpresa"]?.ToString();
        }
    }


    public static string NomEmpresa
    {
        get
        {
            return HttpContext.Current
                .Session["NomEmpresa"]?.ToString();
        }
    }
}