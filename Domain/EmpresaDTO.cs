using System;

namespace Domain
{
    [Serializable]
    public class EmpresaDTO
    {
        public int Codigo { get; set; }
        public string CodigoEstabelecimento { get; set; }
        public string DescricaoEsbelecimento { get; set; }
        public string Tipo { get; set; }
        public string Situacao { get; set; }
        public string RazaoSocial { get; set; }
        public string NomeFantasia { get; set; }
        public string CNPJ { get; set; }
        public string InscricaoEstadual { get; set; }
        public string CodigoMunicipal { get; set; }
        public string Endereco { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Municipio { get; set; }
        public string UF { get; set; }
        public string UFNome { get; set; }
        public string CEP { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string Site { get; set; }
        public string Modal { get; set; }
        public string RNTRC { get; set; }
        public string Logo { get; set; }
        public DateTime? Abertura { get; set; }
        public string Status { get; set; }
        public DateTime? Cadastro { get; set; }
        public string AtividadePrincipal { get; set; }
        // Campos para controle do sistema
        public DateTime? DataCadastro { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public string UsuarioCadastro { get; set; }
        public string UsuarioAlteracao { get; set; }
        
    }
}
