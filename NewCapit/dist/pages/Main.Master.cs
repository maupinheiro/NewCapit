using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Globalization;

namespace NewCapit
{
    public partial class Main : System.Web.UI.MasterPage
    {
        public string foto;
        string id_usuario, foto_usuario;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) {
                if (Session["UsuarioLogado"] != null) {
                    string nomeUsuario = Session["UsuarioLogado"].ToString();
                    lblUsuario.Text = nomeUsuario;
                }
                else
                {
                    lblUsuario.Text = "<Usuário>";
                }

                if (Session["FuncaoUsuario"] != null)
                {
                    string nomeFuncao = Session["FuncaoUsuario"].ToString();
                   // lblFuncao.Text = nomeFuncao;
                }
                else
                {
                   // lblFuncao.Text = "<Função>";
                }

                if (Session["EmpresaTrabalho"] != null)
                {
                    string nomeEmpresa = Session["EmpresaTrabalho"].ToString();
                    lblNucleo.Text = nomeEmpresa;
                }
                else
                {
                    lblNucleo.Text = "<Empresa>";
                }

                if (Session["PermissaoUsuario"] != null)
                {
                    // Converte a string '1,2,3,4,5,6,7,8' em uma lista de inteiros
                    List<int> modulosUsuario = Session["PermissaoUsuario"].ToString()
                        .Split(',')  // Divide a string por vírgulas
                        .Select(int.Parse)  // Converte cada item para inteiro
                        .ToList();  // Converte para lista de inteiros

                    // Controle de visibilidade dos menus
                    //ClientesMenu.Visible = modulosUsuario.Contains(1);
                    //TransportadorasMenu.Visible = modulosUsuario.Contains(2);
                    //VeiculosMenu.Visible = modulosUsuario.Contains(3);
                    //MotoristasMenu.Visible = modulosUsuario.Contains(4);
                    //CargasMenu.Visible = modulosUsuario.Contains(5);
                    //EntregasMenu.Visible = modulosUsuario.Contains(6);
                    //DocumentosMenu.Visible = modulosUsuario.Contains(7);
                    //SistemaMenu.Visible = modulosUsuario.Contains(8);
                }

            }
            foto_usuario = (string)Session["FotoUsuario"];

            

            String path = Server.MapPath("../../fotos/");
            string file = foto_usuario;
            if (File.Exists(path + file))
            {
                foto = "../../fotos/" + file + "";
            }
            else
            {
                foto = "../../fotos/motoristasemfoto.jpg";
            }
           
           
        }

        public static class SafeConvert
        {
            // INT
            public static int SafeInt(string valor)
            {
                int result;
                return int.TryParse(valor, out result) ? result : 0;
            }

            // INT NULLABLE
            public static object SafeIntNullable(string valor)
            {
                int result;
                return int.TryParse(valor, out result) ? (object)result : DBNull.Value;
            }

            // DECIMAL (aceita vírgula ou ponto)
            public static decimal SafeDecimal(string valor)
            {
                if (string.IsNullOrWhiteSpace(valor))
                    return 0;

                valor = valor.Replace(",", ".");

                decimal result;
                return decimal.TryParse(
                    valor,
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out result) ? result : 0;
            }

            // DECIMAL NULLABLE
            public static object SafeDecimalNullable(string valor)
            {
                if (string.IsNullOrWhiteSpace(valor))
                    return DBNull.Value;

                valor = valor.Replace(",", ".");

                decimal result;
                return decimal.TryParse(
                    valor,
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out result) ? (object)result : DBNull.Value;
            }

            // DATETIME
            public static DateTime SafeDate(string valor)
            {
                DateTime dt;
                return DateTime.TryParse(valor, new CultureInfo("pt-BR"), DateTimeStyles.None, out dt)
                    ? dt
                    : DateTime.MinValue;
            }

            // DATETIME NULLABLE
            public static object SafeDateNullable(string valor)
            {
                DateTime dt;
                return DateTime.TryParse(valor, new CultureInfo("pt-BR"), DateTimeStyles.None, out dt)
                    ? (object)dt
                    : DBNull.Value;
            }
        }


    }
}