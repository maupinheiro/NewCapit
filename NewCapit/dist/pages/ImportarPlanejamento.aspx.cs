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
using System.Web;
using System.Web.UI;
using OfficeOpenXml;
using System.IO.Packaging;

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
            if (Session["UsuarioLogado"] != null)
            {
                string nomeUsuario = Session["UsuarioLogado"].ToString();
                var lblUsuario = nomeUsuario;



            }
            else
            {
                var lblUsuario = "<Usuário>";

                Response.Redirect("Login.aspx");
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

                // Verificar colunas obrigatórias
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
                        var cell = row.GetCell(j);
                        if (cell != null)
                        {
                            if (cell.CellType == CellType.Numeric && DateUtil.IsCellDateFormatted(cell))
                            {
                                // Trata data/hora formatada corretamente
                                dr[j] = ((DateTime)cell.DateCellValue).ToString("dd/MM/yyyy HH:mm");
                            }
                            else if (cell.CellType == CellType.Numeric)
                            {
                                // Trata campos numéricos como texto puro (ex: "REMESSA" pode ser 0001)
                                string nomeColuna = headerRow.GetCell(j)?.ToString().Trim().ToUpper();
                                if (nomeColuna == "REMESSA")
                                {
                                    dr[j] = cell.NumericCellValue.ToString("0"); // remove ponto decimal
                                }
                                else
                                {
                                    dr[j] = cell.NumericCellValue.ToString();
                                }
                            }
                            else if (cell.CellType == CellType.String)
                            {
                                dr[j] = cell.StringCellValue.Trim();
                            }
                            else
                            {
                                dr[j] = cell.ToString();
                            }
                        }
                        else
                        {
                            dr[j] = "";
                        }
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
            List<string> codigosNaoEncontrados = new List<string>();

            // Suponha que você esteja lendo os dados de um GridView ou outra fonte
            foreach (GridViewRow row in gvListCargas.Rows)
            {
                GerarNumero();

                int nr_carga = nrcarga;

                string filial = LimparCelula(row.Cells[0].Text);
                string codDestino = LimparCelula(row.Cells[1].Text);
                string plantaDestino = LimparCelula(row.Cells[2].Text);
                string rota = LimparCelula(row.Cells[3].Text);
                string veiculo = LimparCelula(row.Cells[4].Text);
                string codOrigem = LimparCelula(row.Cells[5].Text);
                string coleta = LimparCelula(row.Cells[6].Text);
                string quantidadePallets = LimparCelula(row.Cells[7].Text);
                string viagemTipo = LimparCelula(row.Cells[8].Text);
                string solicitacaoNumero = LimparCelula(row.Cells[9].Text);

                string dataHora = row.Cells[10].Text;

                string peso = LimparCelula(row.Cells[11].Text); //.Replace(",", "");
                string m3 = LimparCelula(row.Cells[12].Text);
                string estudoRota = LimparCelula(row.Cells[13].Text);
                string remessa = LimparCelula(row.Cells[14].Text);
                string plantaSolicitante = LimparCelula(row.Cells[15].Text);


                // Chamada do método ajustado
                SalvarNoBanco(filial, codDestino, plantaDestino, rota, veiculo, codOrigem, coleta, m3, quantidadePallets,
                              viagemTipo, solicitacaoNumero, dataHora, peso, estudoRota, remessa, plantaSolicitante, nr_carga,
                              codigosNaoEncontrados);
                AtualizaNumero();
                if (filial == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "toast",
           "showToast('Importação concluída com sucesso!');", true);
                    return;

                }
            }

            // Exibir alerta caso existam códigos não encontrados
            if (codigosNaoEncontrados.Count > 0)
            {
                string alerta = "Os seguintes códigos não foram encontrados:\\n" + string.Join("\\n", codigosNaoEncontrados);
                string script = $"<script type='text/javascript'>window.onload=function(){{ alert('{alerta}'); }};</script>";
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alertaCodigos", script);
            }
        }
        private string LimparCelula(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor)) return "";
            string texto = HttpUtility.HtmlDecode(valor).Trim();
            return texto == "&nbsp;" ? "" : texto;
        }

        private void SalvarNoBanco(string filial, string codDestino, string plantaDestino, string rota, string veiculo,
                   string codOrigem, string coleta, string m3, string quantidadePallets, string viagemTipo,
                   string solicitacaoNumero, string dataHora, string peso, string estudoRota, string remessa,
                   string plantaSolicitante, int nr_carga, List<string> codigosNaoEncontrados)
        {
            string nomeUsuario = Session["UsuarioLogado"]?.ToString() ?? "Sistema";

            string query = @"INSERT INTO tbcargas (carga, emissao, status, tomador, entrega, peso, material, portao, situacao, previsao, 
                     codorigem, cliorigem, coddestino, clidestino, ufcliorigem, ufclidestino, pedidos, 
                     cidorigem, ciddestino, cadastro, solicitante, empresa, rota, veiculo, quant_palet, tipo_viagem, 
                     solicitacoes, data_hora, estudo_rota, remessa, andamento, codvworigem, codvwdestino, distancia, 
                     cod_expedidor, expedidor, cid_expedidor, uf_expedidor, cod_recebedor, recebedor, cid_recebedor, uf_recebedor, nucleo) 
                     VALUES (@carga, @emissao, @status, @tomador, @entrega, @peso, @material, @portao, @situacao, @previsao, 
                     @codorigem, @cliorigem, @coddestino, @clidestino, @ufcliorigem, @ufclidestino, @pedidos, 
                     @cidorigem, @ciddestino, @cadastro, @solicitante, @empresa, @rota, @veiculo, @quant_palet, @tipo_viagem, 
                     @solicitacoes, @data_hora, @estudo_rota, @remessa, @andamento, @codvworigem, @codvwdestino, @distancia, 
                     @cod_expedidor, @expedidor, @cid_expedidor, @uf_expedidor, @cod_recebedor, @recebedor, @cid_recebedor, @uf_recebedor,@nucleo)";

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                try
                {
                    // Busca Origem e Destino
                    DataTable dto = ExecutarConsulta("SELECT nomcli, cidcli, estcli, codcli FROM tbclientes WHERE codvw = @codvw", conn, "@codvw", codOrigem);
                    DataTable dtd = ExecutarConsulta("SELECT nomcli, cidcli, estcli, codcli FROM tbclientes WHERE codvw = @codvw", conn, "@codvw", codDestino);

                    if (dto.Rows.Count == 0 || dtd.Rows.Count == 0)
                    {
                        if (dto.Rows.Count == 0) codigosNaoEncontrados.Add($"Cod Origem não encontrado: {codOrigem}");
                        if (dtd.Rows.Count == 0) codigosNaoEncontrados.Add($"Cod Destino não encontrado: {codDestino}");
                        return;
                    }

                    // Busca Distância
                    DataTable dta = ExecutarConsulta("SELECT Distancia FROM tbdistanciapremio WHERE Origem=@Origem AND UF_Origem=@UF_Origem AND UF_Destino=@UF_Destino AND Destino=@Destino", conn,
                        new Dictionary<string, object> {
                    {"@UF_Origem", dto.Rows[0]["estcli"]}, {"@Origem", dto.Rows[0]["cidcli"]},
                    {"@UF_Destino", dtd.Rows[0]["estcli"]}, {"@Destino", dtd.Rows[0]["cidcli"]}
                        });

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.Add("@carga", SqlDbType.Int).Value = nr_carga;
                        cmd.Parameters.Add("@emissao", SqlDbType.DateTime).Value = DateTime.Now;
                        cmd.Parameters.Add("@status", SqlDbType.VarChar).Value = "Pendente";
                        cmd.Parameters.Add("@tomador", SqlDbType.VarChar).Value = plantaSolicitante;
                        cmd.Parameters.Add("@entrega", SqlDbType.VarChar).Value = "Normal";

                        // Conversões seguras
                        cmd.Parameters.Add("@peso", SqlDbType.Decimal).Value = ParseDecimal(peso);
                        cmd.Parameters.Add("@pedidos", SqlDbType.Decimal).Value = ParseDecimal(m3);
                        cmd.Parameters.Add("@distancia", SqlDbType.Decimal).Value = (dta.Rows.Count > 0) ? ParseDecimal(dta.Rows[0]["Distancia"].ToString()) : (object)DBNull.Value;

                        cmd.Parameters.Add("@material", SqlDbType.VarChar).Value = "Solicitação";
                        cmd.Parameters.Add("@portao", SqlDbType.VarChar).Value = codDestino;
                        cmd.Parameters.Add("@situacao", SqlDbType.VarChar).Value = "Pronto";
                        cmd.Parameters.Add("@previsao", SqlDbType.VarChar).Value = SafeDateValueData(dataHora.Trim());

                        cmd.Parameters.Add("@codorigem", SqlDbType.VarChar).Value = dto.Rows[0]["codcli"];
                        cmd.Parameters.Add("@cliorigem", SqlDbType.VarChar).Value = dto.Rows[0]["nomcli"];
                        cmd.Parameters.Add("@coddestino", SqlDbType.VarChar).Value = dtd.Rows[0]["codcli"];
                        cmd.Parameters.Add("@clidestino", SqlDbType.VarChar).Value = dtd.Rows[0]["nomcli"];
                        
                        cmd.Parameters.Add("@ufcliorigem", SqlDbType.VarChar).Value = dto.Rows[0]["estcli"];
                        cmd.Parameters.Add("@ufclidestino", SqlDbType.VarChar).Value = dtd.Rows[0]["estcli"];
                        cmd.Parameters.Add("@cidorigem", SqlDbType.VarChar).Value = dto.Rows[0]["cidcli"];
                        cmd.Parameters.Add("@ciddestino", SqlDbType.VarChar).Value = dtd.Rows[0]["cidcli"];
                        cmd.Parameters.Add("@cadastro", SqlDbType.VarChar).Value = $"{DateTime.Now:dd/MM/yyyy HH:mm} - {nomeUsuario}";
                        cmd.Parameters.Add("@solicitante", SqlDbType.VarChar).Value = plantaSolicitante;
                        cmd.Parameters.Add("@empresa", SqlDbType.VarChar).Value = "CNT (CC)";
                        cmd.Parameters.Add("@rota", SqlDbType.VarChar).Value = rota;
                        cmd.Parameters.Add("@veiculo", SqlDbType.VarChar).Value = veiculo;
                        cmd.Parameters.Add("@quant_palet", SqlDbType.VarChar).Value = quantidadePallets;
                        cmd.Parameters.Add("@tipo_viagem", SqlDbType.VarChar).Value = viagemTipo;
                        cmd.Parameters.Add("@solicitacoes", SqlDbType.VarChar).Value = solicitacaoNumero;
                        cmd.Parameters.Add("@data_hora", SqlDbType.VarChar).Value = dataHora.Trim();
                        cmd.Parameters.Add("@estudo_rota", SqlDbType.VarChar).Value = estudoRota;
                        cmd.Parameters.Add("@remessa", SqlDbType.VarChar).Value = remessa;
                        cmd.Parameters.Add("@andamento", SqlDbType.VarChar).Value = "PENDENTE";
                        cmd.Parameters.Add("@codvworigem", SqlDbType.VarChar).Value = codOrigem;
                        cmd.Parameters.Add("@codvwdestino", SqlDbType.VarChar).Value = codDestino;
                        bool isPreliminar = viagemTipo.ToUpper().Contains("PRELIMINAR");
                        cmd.Parameters.Add("@cod_expedidor", SqlDbType.VarChar).Value = dto.Rows[0]["codcli"];
                        cmd.Parameters.Add("@expedidor", SqlDbType.VarChar).Value = dto.Rows[0]["nomcli"];
                        cmd.Parameters.Add("@cid_expedidor", SqlDbType.VarChar).Value = dto.Rows[0]["cidcli"];
                        cmd.Parameters.Add("@uf_expedidor", SqlDbType.VarChar).Value = dto.Rows[0]["estcli"];
                        cmd.Parameters.Add("@cod_recebedor", SqlDbType.VarChar).Value = isPreliminar ? "6111" : dtd.Rows[0]["codcli"];
                        cmd.Parameters.Add("@recebedor", SqlDbType.VarChar).Value = isPreliminar ? "TRANSNOVAG - DIADEMA" : dtd.Rows[0]["nomcli"];
                        cmd.Parameters.Add("@cid_recebedor", SqlDbType.VarChar).Value = isPreliminar ? "DIADEMA" : dtd.Rows[0]["cidcli"];
                        cmd.Parameters.Add("@uf_recebedor", SqlDbType.VarChar).Value = isPreliminar ? "SP" : dtd.Rows[0]["estcli"];
                        cmd.Parameters.Add("@nucleo", SqlDbType.VarChar).Value = "CNT (CC)";

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    codigosNaoEncontrados.Add($"Erro na Carga {nr_carga}: {ex.Message}");
                }
            }
        }

        // Métodos auxiliares para manter o código limpo
        private decimal ParseDecimal(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor)) return 0;
            string limpo = valor.Replace(",", ".");
            decimal.TryParse(limpo, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal result);
            return result;
        }

        private DataTable ExecutarConsulta(string sql, SqlConnection conn, string paramName, string paramValue)
        {
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue(paramName, paramValue);
                new SqlDataAdapter(cmd).Fill(dt);
            }
            return dt;
        }

        private DataTable ExecutarConsulta(string sql, SqlConnection conn, Dictionary<string, object> parameters)
        {
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                foreach (var p in parameters) cmd.Parameters.AddWithValue(p.Key, p.Value);
                new SqlDataAdapter(cmd).Fill(dt);
            }
            return dt;
        }
        private object SafeDecimalValue(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return DBNull.Value; // campo vazio → NULL no banco

            // Remove "kg" (maiúsculo ou minúsculo) e espaços
            input = System.Text.RegularExpressions.Regex.Replace(input, "(?i)kg", "").Trim();

            // Tenta converter para decimal
            decimal valor;
            if (decimal.TryParse(input, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out valor))
                return valor;
            else
                return DBNull.Value; // se não for número válido, grava NULL
        }

        private object SafeDateValue(string input)
        {
            DateTime dt;
            if (DateTime.TryParse(input, out dt))
                return dt.ToString("yyyy-MM-dd HH:mm.00");
            else
                return DBNull.Value;
        }
        private object SafeDateValueData(string input)
        {
            DateTime dt;
            if (DateTime.TryParse(input, out dt))
                return dt.ToString("yyyy-MM-dd");
            else
                return DBNull.Value;
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
            string sqli = "update tbcontador set nr_carga=@nr_carga, nm_empresa=@nm_empresa where nm_empresa=@nm_empresa";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conexao"].ToString());
            {
                SqlCommand comand = new SqlCommand(sqli, con);
                comand.Parameters.Clear();
                comand.Parameters.AddWithValue("@nr_carga", nrcarga);
                comand.Parameters.AddWithValue("@nm_empresa", "CNT");

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