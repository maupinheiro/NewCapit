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
            nomeUsuario = Session["UsuarioLogado"].ToString();

            string query = @"INSERT INTO tbcargas (
                         carga, emissao, status, tomador, entrega, peso, material, portao, situacao, previsao, 
                         codorigem, cliorigem, coddestino, clidestino, idviagem, ufcliorigem, 
                         ufclidestino, pedidos, cidorigem, ciddestino, cadastro,  
                         solicitante, empresa, rota, veiculo, quant_palet, tipo_viagem, solicitacoes, data_hora, 
                         estudo_rota, remessa, andamento, codvworigem, codvwdestino)
                     VALUES (
                         @carga, @emissao, @status, @tomador, @entrega, @peso, @material, @portao, @situacao, @previsao, 
                         @codorigem, @cliorigem, @coddestino, @clidestino, @idviagem, @ufcliorigem, 
                         @ufclidestino, @pedidos, @cidorigem, @ciddestino, @cadastro,  
                         @solicitante, @empresa, @rota, @veiculo, @quant_palet, @tipo_viagem, @solicitacoes, @data_hora, 
                         @estudo_rota, @remessa, @andamento, @codvworigem, @codvwdestino)";

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["conexao"].ToString()))
            {
                try
                {
                    SqlDataAdapter adpto = new SqlDataAdapter("SELECT nomcli, cidcli, estcli, codcli FROM tbclientes WHERE codvw = @codvw", conn);
                    adpto.SelectCommand.Parameters.AddWithValue("@codvw", codOrigem);
                    DataTable dto = new DataTable();
                    adpto.Fill(dto);

                    SqlDataAdapter adptd = new SqlDataAdapter("SELECT nomcli, cidcli, estcli, codcli FROM tbclientes WHERE codvw = @codvw", conn);
                    adptd.SelectCommand.Parameters.AddWithValue("@codvw", codDestino);
                    DataTable dtd = new DataTable();
                    adptd.Fill(dtd);

                    

                    if (dto.Rows.Count == 0)
                        codigosNaoEncontrados.Add($"Cod Origem: {codOrigem} Nome:{coleta} ");

                    if (dtd.Rows.Count == 0)
                        codigosNaoEncontrados.Add($"Destino: {codDestino} Nome:{plantaDestino}");

                    if (dto.Rows.Count > 0 && dtd.Rows.Count > 0)
                    {
                        SqlDataAdapter adpta = new SqlDataAdapter("select Distancia from tbdistanciapremio where Origem=@Origem and UF_Origem=@UF_Origem and UF_Destino=@UF_Destino and Destino=@Destino", conn);
                        adpta.SelectCommand.Parameters.AddWithValue("@UF_Origem", dto.Rows[0]["estcli"].ToString());
                        adpta.SelectCommand.Parameters.AddWithValue("@Origem", dto.Rows[0]["cidcli"].ToString());
                        adpta.SelectCommand.Parameters.AddWithValue("@UF_Destino", dtd.Rows[0]["cidcli"].ToString());
                        adpta.SelectCommand.Parameters.AddWithValue("@Destino", dtd.Rows[0]["cidcli"].ToString());
                        DataTable dta = new DataTable();
                        adptd.Fill(dta);
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@carga", nr_carga);
                        cmd.Parameters.AddWithValue("@emissao", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
                        cmd.Parameters.AddWithValue("@status", "PENDENTE");
                        cmd.Parameters.AddWithValue("@tomador", plantaSolicitante);
                        cmd.Parameters.AddWithValue("@entrega", "NORMAL");
                        cmd.Parameters.AddWithValue("@peso", peso);
                        cmd.Parameters.AddWithValue("@material", "SOLICITAÇÃO");
                        cmd.Parameters.AddWithValue("@portao", codDestino);
                        cmd.Parameters.AddWithValue("@situacao", "PRONTO");
                        //cmd.Parameters.AddWithValue("@previsao", !string.IsNullOrEmpty(dataHora.TrimEnd()) && dataHora.TrimEnd().Length >= 10 ? dataHora.TrimEnd().Substring(0, 10) : string.Empty);
                        cmd.Parameters.AddWithValue("@previsao", dataHora.TrimEnd());
                        cmd.Parameters.AddWithValue("@codorigem", dto.Rows[0]["codcli"].ToString());
                        cmd.Parameters.AddWithValue("@cliorigem", dto.Rows[0]["nomcli"].ToString());
                        cmd.Parameters.AddWithValue("@coddestino", dtd.Rows[0]["codcli"].ToString());
                        cmd.Parameters.AddWithValue("@clidestino", dtd.Rows[0]["nomcli"].ToString());
                        cmd.Parameters.AddWithValue("@idviagem", nr_carga);
                        cmd.Parameters.AddWithValue("@ufcliorigem", dto.Rows[0]["estcli"].ToString());
                        cmd.Parameters.AddWithValue("@ufclidestino", dtd.Rows[0]["estcli"].ToString());
                        cmd.Parameters.AddWithValue("@pedidos", m3);
                        cmd.Parameters.AddWithValue("@cidorigem", dto.Rows[0]["cidcli"].ToString());
                        cmd.Parameters.AddWithValue("@ciddestino", dtd.Rows[0]["cidcli"].ToString());
                        cmd.Parameters.AddWithValue("@cadastro", DateTime.Now.ToString("dd/MM/yyyy HH:mm") + " - " + nomeUsuario);
                        cmd.Parameters.AddWithValue("@solicitante", plantaSolicitante);
                        cmd.Parameters.AddWithValue("@empresa", "CNT");
                        cmd.Parameters.AddWithValue("@rota", rota);
                        cmd.Parameters.AddWithValue("@veiculo", veiculo);
                        cmd.Parameters.AddWithValue("@quant_palet", quantidadePallets);
                        cmd.Parameters.AddWithValue("@tipo_viagem", viagemTipo);
                        cmd.Parameters.AddWithValue("@solicitacoes", solicitacaoNumero);
                        cmd.Parameters.AddWithValue("@data_hora", dataHora.TrimEnd());
                        cmd.Parameters.AddWithValue("@estudo_rota", estudoRota);
                        cmd.Parameters.AddWithValue("@remessa", remessa);
                        cmd.Parameters.AddWithValue("@andamento", "PENDENTE");
                        cmd.Parameters.AddWithValue("@codvworigem", codOrigem);
                        cmd.Parameters.AddWithValue("@codvwdestino", codDestino);
                        cmd.Parameters.AddWithValue("@distancia", dta.Rows[0]["Distancia"].ToString());

                            conn.Open();
                            cmd.ExecuteNonQuery();
                            conn.Close();
                        

                        }
                    }
                }
                catch (Exception ex)
                {
                    string retorno = "Erro Sistêmico: " + ex.ToString() + " Por favor, contate o administrador de sistemas!";
                    string script = $"<script type='text/javascript'>alert('{retorno.Replace("'", "\\'")}');</script>";
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "erro", script);
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