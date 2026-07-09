using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace NewCapit.dist.ModelsCVA
{
    public class CVARepository
    {
        private readonly string _conn =
            ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;

        //public string Salvar(CVA c)
        //{
        //    using (SqlConnection conn = new SqlConnection(_conn))
        //    {
        //        conn.Open();

        //        SqlTransaction trans = conn.BeginTransaction();

        //        try
        //        {
        //            //Gera número da CVA
        //            string numeroCVA = GerarNumeroCVA(conn, trans);

        //            c.NumeroCVA = numeroCVA;

        //            //Cabeçalho
        //            InserirCabecalho(conn, trans, c);

        //            //Produtos
        //            CopiarProdutos(conn, trans,
        //                           numeroCVA,
        //                           c.NumeroSolicitacao);

        //            //Embalagens
        //            CopiarEmbalagens(conn, trans,
        //                             numeroCVA,
        //                             c.NumeroSolicitacao);

        //            //Quantidades
        //            CopiarQuantidades(conn, trans,
        //                              numeroCVA,
        //                              c.NumeroSolicitacao);

        //            //Fornecedor
        //            InserirFornecedor(conn, trans, c);

        //            trans.Commit();

        //            return numeroCVA;
        //        }
        //        catch
        //        {
        //            trans.Rollback();
        //            throw;
        //        }
        //    }

        //}
        
    }
}