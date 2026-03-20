using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Configuration;

namespace NewCapit.dist.pages
{
    public class SapiensIntegrationService
    {
        private readonly string _connectionString = WebConfigurationManager.ConnectionStrings["conexao"].ToString();
        private readonly CultureInfo _culturaBR = CultureInfo.GetCultureInfo("pt-BR");

        public string GerarArquivoCompleto(int idCarga, string numeroDoc, bool ehServico)
        {
            var exportador = new StringBuilder();
            var notas = ObterNotasDaCarga(idCarga);
            var motorista = ObterMotoristaDaCarga(idCarga);
            var veiculos = ObterVeiculosDaCarga(idCarga);

            // Registros Iniciais (1 a 6)
            exportador.AppendLine(GerarRegistro01());
            if (motorista != null) exportador.AppendLine(GerarRegistro02(motorista));
            foreach (var v in veiculos)
            {
                exportador.AppendLine(GerarRegistro03(v));
                exportador.AppendLine($"4|1|{v.Placa}|2|3");
            }

            // Registro 06 e Logística (Linha 5)
            if (!ehServico && veiculos.Count > 1)
                exportador.AppendLine($"5|1|{veiculos[0].Placa}|1|{veiculos[1].Placa}|");

            string linha06 = ehServico ? GerarRegistro06NFSe(numeroDoc) : GerarRegistro06CTe(numeroDoc);
            exportador.AppendLine(linha06);

            // --- NOVOS REGISTROS (8 a 20) ---

            // Registro 08: Detalhes do Item/Serviço
            exportador.AppendLine(GerarRegistro08(numeroDoc, ehServico));

            // Registro 09: Vencimento Financeiro
            exportador.AppendLine(GerarRegistro09(numeroDoc, ehServico));

            // Registro 10: Notas Fiscais Relacionadas
            string tipoDoc = ehServico ? "NFE" : "CTE";
            exportador.Append(GerarRegistros10(numeroDoc, tipoDoc, notas));

            // Registro 13: Rota (Origem/Destino)
            exportador.AppendLine(GerarRegistro13(numeroDoc, tipoDoc, ehServico));

            // Registro 14: Pesos por Nota
            exportador.Append(GerarRegistros14(numeroDoc, tipoDoc, notas));

            // Registro 15 e 16: Seguro e Dados Bancários/Faturamento
            exportador.AppendLine(GerarRegistro15(numeroDoc, tipoDoc));
            exportador.AppendLine(GerarRegistro16(numeroDoc, tipoDoc, ehServico));

            // Registro 19 e 20: Tributos Federais (CBS/IBU)
            if (!ehServico) exportador.AppendLine(GerarRegistro19(numeroDoc)); // Apenas CTe tem Reg 19 no seu exemplo
            exportador.Append(GerarRegistros20(numeroDoc, tipoDoc, ehServico));

            return exportador.ToString();
        }

        // --- GERADORES DE LINHA BASEADOS NOS SEUS TXTs ---

        private string GerarRegistro01() => "1|55890016000109|TRANSNOVAG TRANSPORTES SA|||111501336118|01165913090|RUA CADIRIRI||03109040|PQ. MOOCA|SAO PAULO|SP||11-2126-3555||||ROD||C|0|0|0||851|00109548|1058|0|0";

        private string GerarRegistro02(MotoristaModel m)
        {
            // Segue o padrão da linha 2 do seu exemplo
            return $"2|{m.Nome}|{m.CPF}|{m.Endereco}||{m.Bairro}|{m.CEP}||{m.Cidade}|{m.UF}|{m.Telefone}|||0||1058|{m.CNH}|{m.ValidadeCNH:dd/MM/yyyy}||{m.CategoriaCNH}|{m.RG}|{m.OrgaoRG}|{m.DataEmissaoRG:dd/MM/yyyy}|{m.DataNascimento:dd/MM/yyyy}||||S/N";
        }

        private string GerarRegistro03(VeiculoModel v)
        {
            // Segue o padrão da linha 3 do seu exemplo
            return $"3|{v.Placa}|{v.UF}|0|1|1|{v.Ano}|1|1|0||{v.Chassi}|ROD|C||||0|{v.Renavam}||||0||0|P|06|01|0";
        }

        private string GerarRegistro06CTe(string n) => $"6|2|1|{n}|||{DateTime.Now:dd/MM/yyyy}|57|1|1|1|3.100,80|0,00|0,00|3.100,80|3.100,80|372,10|0,00|0,00|0,00|0,00|61881017000190|CTE|3550308|SP|SAO PAULO";

        private string GerarRegistro06NFSe(string n) => $"6|2|1|{n}||901|{DateTime.Now:dd/MM/yyyy}|00|1|1|1|278,88|0,00|0,00|278,88|0,00|0,00|0,00|0,00|278,88|13,94|61881017000190|NFE|3550308|SP|SAO PAULO";

        private string GerarRegistros10(string doc, string tipo, List<NotaFiscalModel> notas)
        {
            var sb = new StringBuilder();
            int seq = 1;
            foreach (var nf in notas)
            {
                sb.AppendLine($"10|2|1|{tipo}|{doc}|{seq}|{DateTime.Now:dd/MM/yyyy}|55|1|{nf.NumeroNfe}|{nf.ValorNf.ToString("N2", _culturaBR)}|1.000,00000|PC|1|1|5102|01|PESO|{nf.Peso.ToString("N4", _culturaBR)}|{nf.ChaveNfe.Trim()}");
                seq++;
            }
            return sb.ToString();
        }
        private string GerarRegistro08(string n, bool serv) =>
         serv ? $"8|2|1|NFE|{n}|1|90105|79401|PRESTACAO DE SERVICO|PS|1,000|278,880|5,00|13,94|278,88|278,88|0,00|0,00|0,00|0,00|0,00|278,88|1,65|4,60|278,88|7,60|21,19|53|01|01|0,00|0,00|0,00|041|0,00|88"
         : $"8|2|1|CTE|{n}|1|5352F|7940E|PRESTACAO DE SERVICO|PS|1,000|3.100,800|0,00|0,00|3.100,80|3.100,80|0,00|0,00|0,00|0,00|0,00|2.728,70|1,65|45,02|2.728,70|7,60|207,38|53|01|01|3.100,80|12,00|372,10|000|0,00|88";

        private string GerarRegistro09(string n, bool serv) =>
            $"9|2|1|{(serv ? "NFE" : "CTE")}|{n}|1|10/04/2026|{(serv ? "278,88" : "3.100,80")}";

        private string GerarRegistro13(string n, string t, bool serv) =>
            $"13|2|1|{t}|{n}|1|ROD||SAO PAULO|SP||{(serv ? "SAO PAULO" : "SALTO")}|SP|300";

        private string GerarRegistros14(string n, string t, List<NotaFiscalModel> notas)
        {
            var sb = new StringBuilder();
            int seq = 1;
            foreach (var nf in notas)
            {
                sb.AppendLine($"14|2|1|{t}|{n}|{seq}|1|01|PESO|{nf.Peso.ToString("N4", _culturaBR)}");
                seq++;
            }
            return sb.ToString();
        }

        private string GerarRegistro15(string n, string t) => $"15|2|1|{t}|{n}|1|8|8|27982017010654000050|1||";

        private string GerarRegistro16(string n, string t, bool serv)
        {
            string v = serv ? "278,88" : "3.100,80";
            return $"16|2|1|{t}|{n}|ROD|1|1|0|0|61881017000190|08170305000153||||||0|0||S||||||0||||||0|0||0||0||0|0|||0||||0|0|TRANSNOVAG TRANSPORTE SA|S|N|55890016000109||0||237|3393|||{v}|J|0|1|145,80||4|{v}||||||||98471175|8";
        }

        private string GerarRegistro19(string n) => $"19|2|1|CTE|{n}|1|1|25|372,10|20,00|74,42|M|0,00|";

        private string GerarRegistros20(string n, string t, bool serv)
        {
            string v = serv ? "278,88" : "3.100,80";
            string cbs = serv ? "2,51" : "27,91";
            string ibu = serv ? "0,28" : "3,10";
            var sb = new StringBuilder();
            sb.AppendLine($"20|2|1|{t}|{n}|1|1|27|CBS|{v}|0,90|0|0|0|0|0|0|0|{cbs}||0||0|0|0|");
            sb.AppendLine($"20|2|1|{t}|{n}|1|1|27|IBU|{v}|0,10|0|0|0|0|0|0|0|{ibu}||0||0|0|0|");
            return sb.ToString();
        }
        // --- MÉTODOS DE BUSCA (SQL) ---

        private MotoristaModel ObterMotoristaDaCarga(int idCarga)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                // 1. Query corrigida: Relacionando tbcargas com tbmotoristas corretamente
                string sql = @"SELECT m.nommot, m.cpf, m.endmot, m.baimot, m.cepmot, m.ufmot, 
                               m.numregcnh, m.catcnh, m.numrg, m.orgaorg, m.validade, 
                               m.emissaorg, m.dtnasc 
                        FROM tbcargas as c 
                        INNER JOIN tbmotoristas as m ON c.codmot = m.codmot 
                        WHERE c.carga = @carga";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@carga", idCarga);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // 2. Retorna o modelo preenchido com os dados reais do banco
                        return new MotoristaModel
                        {
                            Nome = reader["nommot"].ToString(),
                            CPF = reader["cpf"].ToString(),
                            Endereco = reader["endmot"].ToString(),
                            Bairro = reader["baimot"].ToString(),
                            CEP = reader["cepmot"].ToString(),
                            UF = reader["ufmot"].ToString(),
                            CNH = reader["numregcnh"].ToString(),
                            CategoriaCNH = reader["catcnh"].ToString(),
                            RG = reader["numrg"].ToString(),
                            OrgaoRG = reader["orgaorg"].ToString(),
                            // Conversão de datas tratadas com segurança
                            ValidadeCNH = Convert.ToDateTime(reader["validade"]),
                            DataEmissaoRG = Convert.ToDateTime(reader["emissaorg"]),
                            DataNascimento = Convert.ToDateTime(reader["dtnasc"])
                        };
                    }
                }
            }

            // Caso não encontre nenhum motorista para essa carga
            return null;
        }

        private List<VeiculoModel> ObterVeiculosDaCarga(int idCarga)
        {
            var listaVeiculos = new List<VeiculoModel>();
            string reboque1 = null;
            string reboque2 = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                // 1. Busca o veículo principal (Frota) e identifica se há reboques
                string sqlCarga = @"SELECT v.plavei, v.ufplaca, v.ano, v.chassi, v.renavan, c.reboque1, c.reboque2 
                            FROM tbcargas as c 
                            INNER JOIN tbveiculos as v ON c.frota = v.codvei 
                            WHERE c.carga = @carga";

                SqlCommand cmd = new SqlCommand(sqlCarga, conn);
                cmd.Parameters.AddWithValue("@carga", idCarga);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Adiciona o veículo principal
                        listaVeiculos.Add(new VeiculoModel
                        {
                            Placa = reader["plavei"].ToString(),
                            UF = reader["ufplaca"].ToString(),
                            Ano = reader["ano"].ToString(),
                            Chassi = reader["chassi"].ToString(),
                            Renavam = reader["renavan"].ToString()
                        });

                        // Armazena as placas dos reboques para a próxima consulta
                        reboque1 = reader["reboque1"] != DBNull.Value ? reader["reboque1"].ToString() : null;
                        reboque2 = reader["reboque2"] != DBNull.Value ? reader["reboque2"].ToString() : null;
                    }
                }

                // 2. Se houver reboques, busca os detalhes deles
                if (!string.IsNullOrEmpty(reboque1) || !string.IsNullOrEmpty(reboque2))
                {
                    string sqlReboques = @"SELECT plavei, ufplaca, ano, chassi, renavan 
                                   FROM tbveiculos 
                                   WHERE plavei = @reb1 OR plavei = @reb2";

                    SqlCommand cmdReb = new SqlCommand(sqlReboques, conn);
                    // Evita erro de parâmetro nulo passando string vazia se necessário
                    cmdReb.Parameters.AddWithValue("@reb1", reboque1 ?? "");
                    cmdReb.Parameters.AddWithValue("@reb2", reboque2 ?? "");

                    using (SqlDataReader readerReb = cmdReb.ExecuteReader())
                    {
                        while (readerReb.Read())
                        {
                            listaVeiculos.Add(new VeiculoModel
                            {
                                Placa = readerReb["plavei"].ToString(),
                                UF = readerReb["ufplaca"].ToString(),
                                Ano = readerReb["ano"].ToString(),
                                Chassi = readerReb["chassi"].ToString(),
                                Renavam = readerReb["renavan"].ToString()
                            });
                        }
                    }
                }
            }

            return listaVeiculos;
        }

        private List<NotaFiscalModel> ObterNotasDaCarga(int idCarga)
        {
            var lista = new List<NotaFiscalModel>();
            using (var conn = new SqlConnection(_connectionString))
            {
                string sql = "SELECT numeronfe, vnf, peso, chavenfe FROM tbnfe WHERE carga = @carga";
                var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@carga", idCarga);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new NotaFiscalModel
                        {
                            NumeroNfe = Convert.ToInt32(reader["numeronfe"]),
                            ValorNf = Convert.ToDecimal(reader["vnf"]),
                            Peso = Convert.ToDecimal(reader["peso"]),
                            ChaveNfe = reader["chavenfe"].ToString()
                        });
                    }
                }
            }
            return lista;
        }
    }

    // CLASSES DE MODELO
    public class MotoristaModel { public string Nome, CPF, Endereco, Bairro, CEP, Cidade, UF, Telefone, CNH, CategoriaCNH, RG, OrgaoRG; public DateTime ValidadeCNH, DataEmissaoRG, DataNascimento; }
    public class VeiculoModel { public string Placa, UF, Ano, Chassi, Renavam; }
    public class NotaFiscalModel { public int NumeroNfe; public decimal ValorNf, Peso; public string ChaveNfe; }
}