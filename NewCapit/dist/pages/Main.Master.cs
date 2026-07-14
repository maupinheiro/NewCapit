using DAL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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

                //if (Session["EmpresaTrabalho"] != null)
                //{
                //    string nomeEmpresa = Session["EmpresaTrabalho"].ToString();
                //    lblNucleo.Text = nomeEmpresa;

                //    // 1. Esconde todos primeiro
                //    MenuMatriz.Visible = false;
                //    MenuIpiranga.Visible = false;
                //    MenuMinas.Visible = false;
                //    MenuPernambuco.Visible = false;
                //    MenuDiadema.Visible = false;

                //    // 2. Mostra apenas o correspondente (ajuste conforme o nome que vem do banco)
                //    switch (nomeEmpresa.Trim())
                //    {
                //        case "MATRIZ":
                //            MenuMatriz.Visible = true;
                //            break;
                //        case "IPIRANGA":
                //            MenuIpiranga.Visible = true;
                //            break;
                //        case "MINAS GERAIS":
                //            MenuMinas.Visible = true;
                //            break;
                //        case "PERNAMBUCO":
                //            MenuPernambuco.Visible = true;
                //            break;
                //        case "CNT (CC)":
                //            MenuDiadema.Visible = true;
                //            break;
                //        case "Capit Logística":
                //            MenuMatriz.Visible = true;
                //            MenuIpiranga.Visible = true;
                //            MenuMinas.Visible = true;
                //            MenuPernambuco.Visible = true;
                //            MenuDiadema.Visible = true;
                //            break;
                //            // Adicione outros casos se necessário
                //    }
                //}
                //else
                //{
                //    lblNucleo.Text = "<Empresa>";
                //}

                bool temModulos = false;
                if (Session["PermissaoUsuario"] != null && !string.IsNullOrEmpty(Session["PermissaoUsuario"].ToString()))
                {
                    List<int> modulosUsuario = Session["PermissaoUsuario"].ToString()
                        .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(int.Parse)
                        .ToList();

                    // Mapeia se o usuário tem acesso a alguma das Filiais (módulos de 1 a 5)
                    MenuModuloFiliais.Visible = modulosUsuario.Any(m => m >= 1 && m <= 5);
                    MenuMatriz.Visible = modulosUsuario.Contains(1);
                    MenuIpiranga.Visible = modulosUsuario.Contains(2);
                    MenuMinas.Visible = modulosUsuario.Contains(3);
                    MenuPernambuco.Visible = modulosUsuario.Contains(4);
                    MenuDiadema.Visible = modulosUsuario.Contains(5);

                    // Mapeia os módulos numéricos do sistema
                    MenuModuloClientes.Visible = modulosUsuario.Contains(6);
                    MenuModuloProprietarios.Visible = modulosUsuario.Contains(7);
                    MenuModuloVeiculos.Visible = modulosUsuario.Contains(8);
                    MenuModuloMotoristas.Visible = modulosUsuario.Contains(9);
                    MenuModuloPostos.Visible = modulosUsuario.Contains(10);
                    MenuModuloManutencao.Visible = modulosUsuario.Contains(11);
                    MenuModuloGestao.Visible = modulosUsuario.Contains(12);
                    MenuModuloSistema.Visible = modulosUsuario.Contains(13);

                    temModulos = true;
                }
                else
                {
                    EsconderTodosOsModulos();
                }
                if (temModulos && Session["TelasPermitidas"] != null && !string.IsNullOrEmpty(Session["TelasPermitidas"].ToString()))
                {
                    // Criamos o HashSet para validação em alta performance instantânea
                    HashSet<int> telasPermitidas = new HashSet<int>(
                        Session["TelasPermitidas"].ToString()
                            .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(int.Parse)
                    );

                    // --- FILIAL MATRIZ (id_modulo = 1) ---
                    //TelaColetasMatriz.Visible = telasPermitidas.Contains(3);
                    TelaControleDePedagio.Visible = telasPermitidas.Contains(23);
                    //TelaFrm_CadPedidosMatriz.Visible = telasPermitidas.Contains(58);
                    TelaFrm_ImpSolVWMatriz.Visible = telasPermitidas.Contains(72);
                    TelaGestaoDeCargasMatriz.Visible = telasPermitidas.Contains(81);
                    TelaGestaoDeEntregasMatriz.Visible = telasPermitidas.Contains(85);
                    TelaGestaoDePedidos.Visible = telasPermitidas.Contains(89);

                    // --- FILIAL IPIRANGA (id_modulo = 2) ---
                    TelaControleDePedagioIpiranga.Visible = telasPermitidas.Contains(24);
                    TelaGestaoDeCargasIpiranga.Visible = telasPermitidas.Contains(80);
                    TelaGestaoDeEntregasIpiranga.Visible = telasPermitidas.Contains(84);
                    TelaGestaoDePedidosIpiranga.Visible = telasPermitidas.Contains(90);

                    // --- FILIAL MINAS (id_modulo = 3) ---
                    TelaControleDePedagioMinas.Visible = telasPermitidas.Contains(25);
                    TelaGestaoDeCargasMinas.Visible = telasPermitidas.Contains(82);
                    TelaGestaoDeEntregasMinas.Visible = telasPermitidas.Contains(86);
                    TelaGestaoDePedidosMinas.Visible = telasPermitidas.Contains(91);

                    // --- FILIAL PERNAMBUCO (id_modulo = 4) ---
                    TelaControleDePedagioPernambuco.Visible = telasPermitidas.Contains(26);
                    TelaGestaoDeCargasPernambuco.Visible = telasPermitidas.Contains(83);
                    TelaGestaoDeEntregasPernambuco.Visible = telasPermitidas.Contains(87);
                    TelaGestaoDePedidosPernambuco.Visible = telasPermitidas.Contains(92);

                    // --- FILIAL DIADEMA / CNT (id_modulo = 5) ---
                    TelaConsultaColetasCNT.Visible = telasPermitidas.Contains(7);
                    TelaConsultaEntregas.Visible = telasPermitidas.Contains(9);
                    TelaImportarPlanejamento.Visible = telasPermitidas.Contains(96);

                    // --- MÓDULO CLIENTES (id_modulo = 6) ---
                    TelaConsultaClientes.Visible = telasPermitidas.Contains(6);
                    TelaConsultaFretes.Visible = telasPermitidas.Contains(11);
                    TelaConsultaRotas.Visible = telasPermitidas.Contains(13);
                    TelaFrm_DistanciaEntreCidades.Visible = telasPermitidas.Contains(66);
                    TelaGerenciarRotasKrona.Visible = telasPermitidas.Contains(79);
                    TelaSimuladorFrete.Visible = telasPermitidas.Contains(112);

                    // --- MÓDULO PROPRIETÁRIOS (id_modulo = 7) ---
                    TelaConsulta_Agregados.Visible = telasPermitidas.Contains(17);
                    TelaControleCreditoAbastecimento.Visible = telasPermitidas.Contains(22);
                    TelaFreteMinimoANTT.Visible = telasPermitidas.Contains(39);

                    // --- MÓDULO VEÍCULOS (id_modulo = 8) ---
                    TelaConsultaVeiculos.Visible = telasPermitidas.Contains(15);
                    TelaControleCarretas.Visible = telasPermitidas.Contains(21);
                    TelaControlesValidades.Visible = telasPermitidas.Contains(29);
                    TelaGestaoDeMultas.Visible = telasPermitidas.Contains(88);

                    // --- MÓDULO MOTORISTAS (id_modulo = 9) ---
                    TelaConsultaMotoristas.Visible = telasPermitidas.Contains(12);
                    TelaAcertodePonto.Visible = telasPermitidas.Contains(65);
                    TelaFrm_GerarTXT.Visible = telasPermitidas.Contains(70);
                    TelaGerarTabelaDeAvaliacaoMotoristas.Visible = telasPermitidas.Contains(78);

                    // --- MÓDULO POSTOS (id_modulo = 10) ---
                    TelaControleAbastecimento.Visible = telasPermitidas.Contains(19);
                    TelaEntradaCombustivel.Visible = telasPermitidas.Contains(33);
                    TelaGestaoPostos.Visible = telasPermitidas.Contains(93);

                    // --- MÓDULO MANUTENÇÃO (id_modulo = 11) ---
                    TelaColaboradoresManutencao.Visible = telasPermitidas.Contains(2);
                    TelaConsultaFornecedores.Visible = telasPermitidas.Contains(10);
                    TelaControlaEstoque.Visible = telasPermitidas.Contains(18);
                    TelaControlePneus.Visible = telasPermitidas.Contains(28);
                    TelaDashboardManutencao.Visible = telasPermitidas.Contains(30);
                    TelaFinalizarOS.Visible = telasPermitidas.Contains(37);
                    TelaListaOS.Visible = telasPermitidas.Contains(99);
                    TelaAbrirOS.Visible = telasPermitidas.Contains(1);
                    TelaRequisicaoCompra.Visible = telasPermitidas.Contains(111);

                    // --- MÓDULO GESTÃO (id_modulo = 12) ---
                    TelaIndicadores.Visible = telasPermitidas.Contains(98);
                    TelaTabelas.Visible = telasPermitidas.Contains(98);

                    // --- MÓDULO SISTEMA (id_modulo = 13) ---
                    TelaConsultaUsuarios.Visible = telasPermitidas.Contains(14);
                    TelaControleAcesso.Visible = telasPermitidas.Contains(20);
                    TelaTrocaSenha.Visible = telasPermitidas.Contains(113);
                    TelaTrocaFoto.Visible = telasPermitidas.Contains(113);
                }
                else
                {
                    EsconderTodasAsTelas();
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

        private void EsconderTodosOsModulos()
        {
            MenuModuloFiliais.Visible = false;
            MenuModuloClientes.Visible = false;
            MenuModuloProprietarios.Visible = false;
            MenuModuloVeiculos.Visible = false;
            MenuModuloMotoristas.Visible = false;
            MenuModuloPostos.Visible = false;
            MenuModuloManutencao.Visible = false;
            MenuModuloGestao.Visible = false;
            MenuModuloSistema.Visible = false;
            EsconderTodasAsTelas();
        }

        private void EsconderTodasAsTelas()
        {
            // Reseta a visibilidade de todas as sub-telas caso falhe a validação
            /*TelaColetasMatriz.Visible = false;*/ TelaControleDePedagio.Visible = false; /*TelaFrm_CadPedidosMatriz.Visible = false;*/
            TelaAcertodePonto.Visible = false;
            TelaFrm_ImpSolVWMatriz.Visible = false; TelaGestaoDeCargasMatriz.Visible = false; TelaGestaoDeEntregasMatriz.Visible = false;
            TelaGestaoDePedidos.Visible = false; TelaControleDePedagioIpiranga.Visible = false; TelaGestaoDeCargasIpiranga.Visible = false;
            TelaGestaoDeEntregasIpiranga.Visible = false; TelaGestaoDePedidosIpiranga.Visible = false; TelaControleDePedagioMinas.Visible = false;
            TelaGestaoDeCargasMinas.Visible = false; TelaGestaoDeEntregasMinas.Visible = false; TelaGestaoDePedidosMinas.Visible = false;
            TelaControleDePedagioPernambuco.Visible = false; TelaGestaoDeCargasPernambuco.Visible = false; TelaGestaoDeEntregasPernambuco.Visible = false;
            TelaGestaoDePedidosPernambuco.Visible = false; TelaConsultaColetasCNT.Visible = false; TelaConsultaEntregas.Visible = false;
            TelaImportarPlanejamento.Visible = false; TelaConsultaClientes.Visible = false; TelaConsultaFretes.Visible = false;
            TelaConsultaRotas.Visible = false; TelaFrm_DistanciaEntreCidades.Visible = false; TelaGerenciarRotasKrona.Visible = false;
            TelaSimuladorFrete.Visible = false; TelaConsulta_Agregados.Visible = false; TelaControleCreditoAbastecimento.Visible = false;
            TelaFreteMinimoANTT.Visible = false; TelaConsultaVeiculos.Visible = false; TelaControleCarretas.Visible = false;
            TelaControlesValidades.Visible = false; TelaGestaoDeMultas.Visible = false; TelaConsultaMotoristas.Visible = false;
            TelaFrm_GerarTXT.Visible = false; TelaGerarTabelaDeAvaliacaoMotoristas.Visible = false; TelaControleAbastecimento.Visible = false;
            TelaEntradaCombustivel.Visible = false; TelaGestaoPostos.Visible = false; TelaColaboradoresManutencao.Visible = false;
            TelaConsultaFornecedores.Visible = false; TelaControlaEstoque.Visible = false; TelaControlePneus.Visible = false;
            TelaDashboardManutencao.Visible = false; TelaFinalizarOS.Visible = false; TelaListaOS.Visible = false;
            TelaRequisicaoCompra.Visible = false; TelaIndicadores.Visible = false; TelaConsultaUsuarios.Visible = false;
            TelaControleAcesso.Visible = false; TelaTrocaSenha.Visible = false;
        }

        protected void btnSair_Click(object sender, EventArgs e)
        {
            // 1. Pega o valor da session para uma variável local
            var idParaLog = Session["IdSessaoLog"].ToString();

            if (idParaLog != null)
            {
                // 2. Grava no banco PRIMEIRO
                UsersDAL.RegistrarLogout(idParaLog);
            }

            // 3. SÓ AGORA limpa a sessão
            Session.Remove("IdSessaoLog"); // Remove apenas este para garantir
            Session.Clear();
            Session.Abandon();

            // 4. Redireciona
            Response.Redirect("~/Login.aspx");
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