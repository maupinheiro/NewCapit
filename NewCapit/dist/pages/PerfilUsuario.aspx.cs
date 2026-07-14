using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NewCapit
{
    public partial class PerfilUsuario : System.Web.UI.Page
    {
        string conexao = ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UsuarioSistema"] == null)
                {
                    Response.Redirect("Login.aspx");
                    return;
                }

                txtUsuario.Text = Session["UsuarioSistema"].ToString();

                CarregarFoto();
            }
        }

        private void CarregarFoto()
        {
            using (SqlConnection con = new SqlConnection(conexao))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand(
                @"SELECT foto_usuario
                  FROM tb_usuario
                  WHERE nm_usuario=@usuario", con);

                cmd.Parameters.AddWithValue("@usuario", txtUsuario.Text);

                object foto = cmd.ExecuteScalar();

                if (foto != DBNull.Value &&
                    foto != null &&
                    foto.ToString() != "")
                {
                    imgFoto.ImageUrl = foto.ToString() +
                        "?v=" + DateTime.Now.Ticks;
                }
                else
                {
                    imgFoto.ImageUrl = "~/fotos/usuario.jpg";
                }
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            lblMensagem.ForeColor = System.Drawing.Color.Red;

            if (!fuFoto.HasFile)
            {
                lblMensagem.Text = "Selecione uma imagem.";
                return;
            }

            string extensao = Path.GetExtension(fuFoto.FileName).ToLower();

            if (extensao != ".jpg" &&
                extensao != ".jpeg" &&
                extensao != ".png")
            {
                lblMensagem.Text =
                    "Somente arquivos JPG ou PNG.";
                return;
            }

            if (fuFoto.PostedFile.ContentLength > 2097152)
            {
                lblMensagem.Text =
                    "A imagem deve ter no máximo 2 MB.";
                return;
            }

            try
            {
                string usuario = txtUsuario.Text;

                string pasta = Server.MapPath("~/Fotos/");

                if (!Directory.Exists(pasta))
                    Directory.CreateDirectory(pasta);

                string arquivoJpg = Path.Combine(pasta, usuario + ".jpg");
                string arquivoJpeg = Path.Combine(pasta, usuario + ".jpeg");
                string arquivoPng = Path.Combine(pasta, usuario + ".png");

                if (File.Exists(arquivoJpg))
                    File.Delete(arquivoJpg);

                if (File.Exists(arquivoJpeg))
                    File.Delete(arquivoJpeg);

                if (File.Exists(arquivoPng))
                    File.Delete(arquivoPng);

                string nomeArquivo = usuario + extensao;

                string caminhoFisico = Path.Combine(pasta, nomeArquivo);

                fuFoto.SaveAs(caminhoFisico);

                string caminhoBanco = "~/Fotos/" + nomeArquivo;

                using (SqlConnection con = new SqlConnection(conexao))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand(@"

                    UPDATE tb_usuario
                    SET foto_usuario=@foto
                    WHERE nm_usuario=@usuario

                    ", con);

                    cmd.Parameters.AddWithValue("@foto", caminhoBanco);
                    cmd.Parameters.AddWithValue("@usuario", usuario);

                    cmd.ExecuteNonQuery();
                }

                imgFoto.ImageUrl = caminhoBanco +
                    "?v=" + DateTime.Now.Ticks;

                lblMensagem.ForeColor = System.Drawing.Color.Green;

                lblMensagem.Text =
                    "Foto alterada com sucesso.";

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
    }
}