using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace NewCapit.dist.pages
{
    public class SqlHelper
    {
        #region Texto

        public static void NVarChar(SqlCommand cmd, string nome, int tamanho, string valor)
        {
            SqlParameter p = cmd.Parameters.Add(nome, SqlDbType.NVarChar, tamanho);
            p.Value = string.IsNullOrWhiteSpace(valor) ? (object)DBNull.Value : valor.Trim();
        }

        public static void VarChar(SqlCommand cmd, string nome, int tamanho, string valor)
        {
            SqlParameter p = cmd.Parameters.Add(nome, SqlDbType.VarChar, tamanho);
            p.Value = string.IsNullOrWhiteSpace(valor) ? (object)DBNull.Value : valor.Trim();
        }

        public static void Char(SqlCommand cmd, string nome, int tamanho, string valor)
        {
            SqlParameter p = cmd.Parameters.Add(nome, SqlDbType.Char, tamanho);
            p.Value = string.IsNullOrWhiteSpace(valor) ? (object)DBNull.Value : valor.Trim();
        }

        #endregion

        #region Inteiros

        public static void Int(SqlCommand cmd, string nome, int? valor)
        {
            SqlParameter p = cmd.Parameters.Add(nome, SqlDbType.Int);
            p.Value = valor.HasValue ? (object)valor.Value : DBNull.Value;
        }

        public static void BigInt(SqlCommand cmd, string nome, long? valor)
        {
            SqlParameter p = cmd.Parameters.Add(nome, SqlDbType.BigInt);
            p.Value = valor.HasValue ? (object)valor.Value : DBNull.Value;
        }

        public static void SmallInt(SqlCommand cmd, string nome, short? valor)
        {
            SqlParameter p = cmd.Parameters.Add(nome, SqlDbType.SmallInt);
            p.Value = valor.HasValue ? (object)valor.Value : DBNull.Value;
        }

        public static void TinyInt(SqlCommand cmd, string nome, byte? valor)
        {
            SqlParameter p = cmd.Parameters.Add(nome, SqlDbType.TinyInt);
            p.Value = valor.HasValue ? (object)valor.Value : DBNull.Value;
        }

        #endregion

        #region Decimal

        public static void Decimal(SqlCommand cmd, string nome, decimal? valor, byte precision = 18, byte scale = 2)
        {
            SqlParameter p = cmd.Parameters.Add(nome, SqlDbType.Decimal);
            p.Precision = precision;
            p.Scale = scale;
            p.Value = valor.HasValue ? (object)valor.Value : DBNull.Value;
        }

        public static void Money(SqlCommand cmd, string nome, decimal? valor)
        {
            SqlParameter p = cmd.Parameters.Add(nome, SqlDbType.Money);
            p.Value = valor.HasValue ? (object)valor.Value : DBNull.Value;
        }

        public static void Float(SqlCommand cmd, string nome, double? valor)
        {
            SqlParameter p = cmd.Parameters.Add(nome, SqlDbType.Float);
            p.Value = valor.HasValue ? (object)valor.Value : DBNull.Value;
        }

        #endregion

        #region Data

        public static void Date(SqlCommand cmd, string nome, DateTime? valor)
        {
            SqlParameter p = cmd.Parameters.Add(nome, SqlDbType.Date);
            p.Value = valor.HasValue ? (object)valor.Value.Date : DBNull.Value;
        }

        public static void DateTime(SqlCommand cmd, string nome, DateTime? valor)
        {
            SqlParameter p = cmd.Parameters.Add(nome, SqlDbType.DateTime);
            p.Value = valor.HasValue ? (object)valor.Value : DBNull.Value;
        }

        public static void Time(SqlCommand cmd, string nome, TimeSpan? valor)
        {
            SqlParameter p = cmd.Parameters.Add(nome, SqlDbType.Time);
            p.Value = valor.HasValue ? (object)valor.Value : DBNull.Value;
        }

        #endregion

        #region Booleano

        public static void Bit(SqlCommand cmd, string nome, bool? valor)
        {
            SqlParameter p = cmd.Parameters.Add(nome, SqlDbType.Bit);
            p.Value = valor.HasValue ? (object)valor.Value : DBNull.Value;
        }

        #endregion

        #region Outros

        public static void UniqueIdentifier(SqlCommand cmd, string nome, Guid? valor)
        {
            SqlParameter p = cmd.Parameters.Add(nome, SqlDbType.UniqueIdentifier);
            p.Value = valor.HasValue ? (object)valor.Value : DBNull.Value;
        }

        #endregion
    }
}
