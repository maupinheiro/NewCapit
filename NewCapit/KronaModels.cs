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
        public EntidadeKrona transportador { get; set; } // Ajustado para EntidadeKrona
        public Motorista motorista_1 { get; set; }
        public Veiculo veiculo { get; set; }
        public Veiculo reboque_1 { get; set; }
        public EntidadeKrona origem { get; set; } // Ajustado para EntidadeKrona
        public Dictionary<string, Destino> destinos { get; set; }
        public Viagem viagem { get; set; }
    }

    public class UsuarioLogin { public string login { get; set; } public string senha { get; set; } }

    // Classe base para evitar repetir CNPJ e Razão Social em tudo
    public class EntidadeKrona { public string cnpj { get; set; } public string razao_social { get; set; } }

    public class Motorista : EntidadeKrona { public string nome { get; set; } public string cpf { get; set; } }

    public class Veiculo { public string placa { get; set; } public string renavan { get; set; } public string tecnologia { get; set; } }

    public class Destino : EntidadeKrona
    {
        public DadosAdicionais dados_adicionais { get; set; }
    }

    public class DadosAdicionais
    {
        public EntidadeKrona remetente { get; set; }
        public string nota { get; set; }
    }

    public class Viagem
    {
        public string tipo_viagem { get; set; }
        public string valor { get; set; }
        public string liberacao { get; set; }
        public string rota { get; set; }
    }

    public class KronaResponse
    {
        public string status { get; set; }
        public string mensagem { get; set; }
        public string protocolo { get; set; }
    }
}