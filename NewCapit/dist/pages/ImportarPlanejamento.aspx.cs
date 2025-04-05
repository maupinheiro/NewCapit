using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Web.Services.Description;
using System.Web.UI.WebControls;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Linq;

namespace NewCapit.dist.pages
{
    public partial class ImportarPlanejamento : System.Web.UI.Page
    {
        string nomeUsuario;
        int nrcarga;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UsuarioLogado"] != null)
            {
                string nomeUsuario = Session["UsuarioLogado"].ToString();
                var lblUsuario = nomeUsuario;

                //  txtAlteradoPor.Text = nomeUsuario;

            }
            else
            {
                //var lblUsuario = "<Usuário>";
                //txtAlteradoPor.Text = lblUsuario;
            }
        }
        protected void lnkCarregar_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {
                string extensao = Path.GetExtension(FileUpload1.FileName).ToLower();

                // Verifica se a extensão é válida (somente arquivos Excel)
                if (extensao == ".xls" || extensao == ".xlsx")
                {
                    string filePath = Path.Combine(Server.MapPath("~/Uploads"), FileUpload1.FileName);
                    FileUpload1.SaveAs(filePath);
                    CarregarDadosExcel(filePath);
                    lblMensagem.Text = "Arquivo carregado com sucesso!";
                }
                else
                {
                    lblMensagem.Text = "Somente arquivos do tipo Excel (.xls ou .xlsx) são permitidos.";
                }
            }
            else
            {
                lblMensagem.Text = "Selecione um arquivo para importar.";
            }
        }


        private void CarregarDadosExcel(string filePath)
        {
            DataTable dt = new DataTable();
            List<string> colunasObrigatorias = new List<string>
    {
        "FILIAL", "CODDESTINO", "PLANTA DESTINO", "ROTA", "VEÍCULO", "CODORIGEM", "COLETA FORNECEDOR",
         "Quant./ Pallet´s", "VIAGEM TIPO", "SOLICITAÇÃO Nº", "DATA /HORA",
        "PESO", "M³", "ESTUDO / ROTA", "REMESSA", "PLANTA SOLICITANTE"
    };

            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook = new XSSFWorkbook(fs);
                ISheet sheet = workbook.GetSheetAt(0);

                IRow headerRow = sheet.GetRow(0);
                int cellCount = headerRow.LastCellNum;

                List<string> colunasDaPlanilha = new List<string>();
                for (int i = 0; i < cellCount; i++)
                {
                    string nomeColuna = headerRow.GetCell(i)?.ToString().Trim();
                    if (!string.IsNullOrEmpty(nomeColuna))
                    {
                        dt.Columns.Add(nomeColuna);
                        colunasDaPlanilha.Add(nomeColuna.ToUpper());
                    }
                }

                // Verificar se todas as colunas obrigatórias estão presentes
                var colunasFaltantes = colunasObrigatorias
                    .Where(c => !colunasDaPlanilha.Contains(c.ToUpper()))
                    .ToList();

                if (colunasFaltantes.Any())
                {
                    lblMensagem.Text = "Erro: A planilha está faltando as seguintes colunas obrigatórias: " + string.Join(", ", colunasFaltantes);
                    return;
                }

                for (int i = 1; i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    if (row == null) continue;
                    DataRow dr = dt.NewRow();

                    for (int j = 0; j < cellCount; j++)
                    {
                        dr[j] = row.GetCell(j)?.ToString() ?? "";
                    }
                    dt.Rows.Add(dr);
                }
            }

            gvListCargas.DataSource = dt;
            gvListCargas.DataBind();
            lblMensagem.Text = "Planilha importada com sucesso!";
        }


        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gvListCargas.Rows)
            {
                GerarNumero();
                int nr_carga = nrcarga;
                string filial = row.Cells[0].Text;
                string codDestino = row.Cells[1].Text;
                string plantaDestino = row.Cells[2].Text;
                string rota = row.Cells[3].Text;
                string veiculo = row.Cells[4].Text;
                string codOrigem = row.Cells[5].Text;
                string coleta = row.Cells[6].Text;
                string quantidadePallets = row.Cells[7].Text;
                string viagemTipo = row.Cells[8].Text;
                string solicitacaoNumero = row.Cells[9].Text;
                string dataHora = row.Cells[10].Text;
                string peso = row.Cells[11].Text.Replace(",", "");
                string m3 = row.Cells[12].Text;
                string estudoRota = row.Cells[13].Text;
                string remessa = row.Cells[14].Text;
                string plantaSolicitante = row.Cells[15].Text;

                SalvarNoBanco(filial, codDestino, plantaDestino, rota, veiculo, codOrigem, coleta, m3, quantidadePallets, viagemTipo, solicitacaoNumero, dataHora, peso, estudoRota, remessa, plantaSolicitante, nr_carga);
            }
            gvListCargas.DataSource = null;
            gvListCargas.DataBind();
            string retorno = "Dados inseridos com sucesso!";
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("<script type = 'text/javascript'>");
            sb.Append("window.onload=function(){");
            sb.Append("alert('");
            sb.Append(retorno);
            sb.Append("')};");
            sb.Append("</script>");
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
            //lblMensagem.Text = "Dados inseridos com sucesso!";
        }

        private void SalvarNoBanco(string filial, string codDestino, string plantaDestino, string rota, string veiculo, string codOrigem, string coleta, string m3, string quantidadePallets, string viagemTipo, string solicitacaoNumero, string dataHora, string peso, string estudoRota, string remessa, string plantaSolicitante, int nr_carga)
        {

            nomeUsuario = Session["UsuarioLogado"].ToString();

            string query = @"
                            INSERT INTO tbcargas (
                                carga, emissao, status, tomador,  entrega, peso, material, portao, situacao, previsao, 
                                codorigem, cliorigem, coddestino, clidestino, idviagem,  ufcliorigem, 
                                ufclidestino,  pedidos, cidorigem, ciddestino,  cadastro,  
                                solicitante, empresa, rota, veiculo, quant_palet, tipo_viagem, solicitacoes, data_hora, 
                                estudo_rota, remessa, andamento, codvworigem, codvwdestino
                            ) VALUES (
                                @carga, @emissao, @status, @tomador, @entrega, @peso, @material, @portao, @situacao, @previsao, 
                                @codorigem, @cliorigem, @coddestino, @clidestino,  @idviagem, @ufcliorigem, 
                                @ufclidestino,  @pedidos, @cidorigem, @ciddestino, @cadastro, 
                                @solicitante, @empresa, @rota, @veiculo, @quant_palet, @tipo_viagem, @solicitacoes, @data_hora, 
                                @estudo_rota, @remessa, @andamento, @codvworigem, @codvwdestino
                            )
                        ";

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["conexao"].ToString());
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                try
                {



                    string sqlo = "select nomcli,cidcli,estcli,codcli from tbclientes where codvw='" + codOrigem + "'";
                    SqlDataAdapter adpto = new SqlDataAdapter(sqlo, conn);
                    DataTable dto = new DataTable();
                    adpto.Fill(dto);
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                        adpto.Fill(dto);
                        conn.Close();
                    }
                    string sqld = "select nomcli,cidcli,estcli,codcli from tbclientes where codvw='" + codDestino + "'";
                    SqlDataAdapter adptd = new SqlDataAdapter(sqld, conn);
                    DataTable dtd = new DataTable();
                    adptd.Fill(dtd);
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                        adptd.Fill(dtd);
                        conn.Close();
                    }

                    if (dtd.Rows.Count > 0 && dto.Rows.Count > 0)
                    {
                        // Adicione os parâmetros com os valores correspondentes
                        cmd.Parameters.AddWithValue("@carga", nr_carga);
                        cmd.Parameters.AddWithValue("@emissao", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
                        cmd.Parameters.AddWithValue("@status", "PENDENTE");
                        cmd.Parameters.AddWithValue("@tomador", plantaSolicitante);
                        cmd.Parameters.AddWithValue("@entrega", "NORMAL");
                        cmd.Parameters.AddWithValue("@peso", peso);
                        cmd.Parameters.AddWithValue("@material", "SOLICITAÇÃO");
                        cmd.Parameters.AddWithValue("@portao", codDestino);
                        cmd.Parameters.AddWithValue("@situacao", "PRONTO");
                        cmd.Parameters.AddWithValue("@previsao", dataHora.Substring(0, 10));
                        cmd.Parameters.AddWithValue("@codorigem", codOrigem);
                        cmd.Parameters.AddWithValue("@cliorigem", dto.Rows[0][0].ToString());
                        cmd.Parameters.AddWithValue("@coddestino", codDestino);
                        cmd.Parameters.AddWithValue("@clidestino", dtd.Rows[0][0].ToString());
                        cmd.Parameters.AddWithValue("@idviagem", nr_carga);
                        cmd.Parameters.AddWithValue("@ufcliorigem", dto.Rows[0][2].ToString());
                        cmd.Parameters.AddWithValue("@ufclidestino", dtd.Rows[0][2].ToString());
                        cmd.Parameters.AddWithValue("@pedidos", m3);
                        cmd.Parameters.AddWithValue("@cidorigem", dto.Rows[0][1].ToString());
                        cmd.Parameters.AddWithValue("@ciddestino", dtd.Rows[0][1].ToString());
                        cmd.Parameters.AddWithValue("@cadastro", DateTime.Now.ToString("dd/MM/yyyy HH:mm") + " - " + nomeUsuario);
                        cmd.Parameters.AddWithValue("@solicitante", plantaSolicitante);
                        cmd.Parameters.AddWithValue("@empresa", "CNT");
                        cmd.Parameters.AddWithValue("@rota", rota);
                        cmd.Parameters.AddWithValue("@veiculo", veiculo);
                        cmd.Parameters.AddWithValue("@quant_palet", quantidadePallets);
                        cmd.Parameters.AddWithValue("@tipo_viagem", viagemTipo);
                        cmd.Parameters.AddWithValue("@solicitacoes", solicitacaoNumero);
                        cmd.Parameters.AddWithValue("@data_hora", dataHora);
                        cmd.Parameters.AddWithValue("@estudo_rota", rota);
                        cmd.Parameters.AddWithValue("@remessa", remessa);
                        cmd.Parameters.AddWithValue("@andamento", "PENDENTE");
                        cmd.Parameters.AddWithValue("@codvworigem", codOrigem);
                        cmd.Parameters.AddWithValue("@codvwdestino", codDestino);


                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    string retorno = "Erro Sistêmico: "+ex.ToString()+" Por favor, contate o administrador de sistemas!";
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append("<script type = 'text/javascript'>");
                    sb.Append("window.onload=function(){");
                    sb.Append("alert('");
                    sb.Append(retorno);
                    sb.Append("')};");
                    sb.Append("</script>");
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
                }

            }

        }

        public void GerarNumero()
        {
            string sql_sequncia = " select isnull(max(nr_carga+1),1) as nr_carga from tbcontador where nm_empresa='CNT' ";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conexao"].ToString());
            {
                con.Open();

                SqlDataAdapter da = new SqlDataAdapter(sql_sequncia, con);

                DataTable dt2 = new DataTable();

                da.Fill(dt2);

                nrcarga = int.Parse(dt2.Rows[0][0].ToString());

                con.Close();
            }
           
        }
        public void AtualizaNumero()
        {
            string sqli = "update tbcontador set nr_carga=@nr_nr_carga, nm_empresa=@nm_empresa where nm_empresa=@nm_empresa";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conexao"].ToString());
            {
                SqlCommand comand = new SqlCommand(sqli, con);
                comand.Parameters.Clear();
                comand.Parameters.AddWithValue("@nr_carga", nrcarga);
                comand.Parameters.AddWithValue("@ds_tipo", "CNT");

                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                    comand.ExecuteNonQuery();
                    con.Close();

                }
            }
        }
    }
}