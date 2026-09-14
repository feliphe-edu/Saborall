namespace Saborall.Models
{
    public class Produto
    {
        public int IdProduto { get; set; }

        public string NomeProduto { get; set; } = "";

        public string Categoria { get; set; } = "";

        public decimal Preco { get; set; }

        public int? IdFornecedor { get; set; }
    }
}