using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewCapit.Models.Krona
{
    public class SolicitacaoViagemRequest
    {
        public KronaService kronaService { get; set; }
    }

    public class KronaService
    {
        public UsuarioLogin usuario_login { get; set; }
        public Transportador transportador { get; set; }
        public Motorista motorista_1 { get; set; }
        public Motorista motorista_2 { get; set; }
        public Veiculo veiculo { get; set; }
        public Veiculo reboque_1 { get; set; }
        public Veiculo reboque_2 { get; set; }
        public Veiculo reboque_3 { get; set; }
        public EntidadeCompleta origem { get; set; }
        public Dictionary<string, Destino> destinos { get; set; }
        public Viagem viagem { get; set; }
    }

    public class UsuarioLogin
    {
        public string login { get; set; }
        public string senha { get; set; }
    }

    // Classe base com todos os campos de endereço e contato do JSON
    public class EntidadeCompleta
    {
        public string tipo { get; set; }
        public string cnpj { get; set; }
        public string razao_social { get; set; }
        public string nome_fantasia { get; set; }
        public string unidade { get; set; }
        public string codigo { get; set; }
        public string end_rua { get; set; }
        public string end_numero { get; set; }
        public string end_complemento { get; set; }
        public string end_bairro { get; set; }
        public string end_cidade { get; set; }
        public string end_uf { get; set; }
        public string end_cep { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string telefone_1 { get; set; }
        public string telefone_2 { get; set; }
        public string responsavel { get; set; }
        public string responsavel_cargo { get; set; }
        public string responsavel_telefone { get; set; }
        public string responsavel_celular { get; set; }
        public string responsavel_email { get; set; }
    }

    public class Transportador : EntidadeCompleta { }

    public class Motorista
    {
        public string nome { get; set; }
        public string cpf { get; set; }
        public string rg { get; set; }
        public string orgao_emissao { get; set; }
        public string rg_uf { get; set; }
        public string data_nascimento { get; set; }
        public string nome_mae { get; set; }
        public string estado_civil { get; set; }
        public string escolaridade { get; set; }
        public string cnh_numero { get; set; }
        public string cnh_categoria { get; set; }
        public string cnh_vencimento { get; set; }
        public string end_rua { get; set; }
        public string end_numero { get; set; }
        public string end_complemento { get; set; }
        public string end_bairro { get; set; }
        public string end_cidade { get; set; }
        public string end_uf { get; set; }
        public string end_cep { get; set; }
        public string celular { get; set; }
        public string fone { get; set; }
        public string nextel { get; set; }
        public string mopp { get; set; }
        public string aso { get; set; }
        public string cdd { get; set; }
        public string capacitacao { get; set; }
        public string vinculo { get; set; }
    }

    public class Veiculo
    {
        public string placa { get; set; }
        public string renavam { get; set; }
        public string chassi { get; set; }
        public string uf { get; set; }
        public string cor { get; set; }
        public string marca { get; set; }
        public string modelo { get; set; }
        public string tipo { get; set; }
        public string tecnologia { get; set; }
        public string id_rastreador { get; set; }
        public string comunicacao { get; set; }
        public string tecnologia_sec { get; set; }
        public string id_rastreador_sec { get; set; }
        public string comunicacao_sec { get; set; }
        public string fixo { get; set; }
        public string ano { get; set; }
        public string capacidade { get; set; }
        public string numero_att { get; set; }
        public string validade_antt { get; set; }
        public string numero_frota { get; set; }
        public string transp_frota { get; set; }
        public string proprietario { get; set; }
        public string proprietario_cnpj { get; set ; }
        public string end_rua { get; set; }
        public string end_numero { get; set; }
        public string end_complemento { get; set; }
        public string end_bairro { get; set; }
        public string end_cidade { get; set; }
        public string end_uf { get; set; }
        public string end_cep { get; set; }
    }

   
    public class Destino : EntidadeCompleta
    {
        public DadosAdicionais dados_adicionais { get; set; }
    }

    public class DadosAdicionais
    {
        public EntidadeCompleta remetente { get; set; }
        public string mercadoria { get; set; }
        public string valor { get; set; }
        public string norma { get; set; }
        public string grupo_norma { get; set; }
        public string nota { get; set; }
        public string observacao { get; set; }
    }

    public class Viagem
    {
        public string tipo_viagem { get; set; }
        public string rastreada { get; set; }
        public string percurso { get; set; }
        public string tipo_cliente { get; set; }
        public string doca_origem { get; set; }
        public string fpp { get; set; }
        public string mercadoria_id { get; set; }
        public string valor { get; set; }
        public string peso_total { get; set; }
        public string rota { get; set; }
        public string rota_id { get; set; }
        public string inicio_previsto { get; set; }
        public string fim_previsto { get; set; }
        public string liberacao { get; set; }
        public string numero_cliente { get; set; }
        public string observacao { get; set; }
        public string localizador1_1 { get; set; }
        public string id_localizador1_1 { get; set; }
        public string localizador1_2 { get; set; }
        public string id_localizador1_2 { get; set; }
        public string localizador1_3 { get; set; }
        public string id_localizador1_3 { get; set; }
        public string localizador2_1 { get; set; }
        public string id_localizador2_1 { get; set; }
        public string localizador2_2 { get; set; }
        public string id_localizador2_2 { get; set; }
        public string localizador2_3 { get; set; }
        public string id_localizador2_3 { get; set; }
        public string localizador3_1 { get; set; }
        public string id_localizador3_1 { get; set; }
        public string localizador3_2 { get; set; }
        public string id_localizador3_2 { get; set; }
        public string localizador3_3 { get; set; }
        public string id_localizador3_3 { get; set; }


    }

    public class KronaResponse
    {
        public string status { get; set; }
        public string mensagem { get; set; }
        public string protocolo { get; set; }
    }
}