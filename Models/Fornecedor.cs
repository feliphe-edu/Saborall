namespace Saborall.Models
{
    public class Fornecedor
    {
        public int IdFornecedor { get; set; }

        public string NomeEmpresa { get; set; } = "";

        public string NomeFantasia { get; set; } = "";

        public string Cnpj { get; set; } = "";

        public string Endereco { get; set; } = "";

        public string Email { get; set; } = "";

        public string Telefone { get; set; } = "";
    }
}