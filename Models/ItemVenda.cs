namespace Saborall.Models
{
    public class ItemVenda
    {
        public int IdItemVenda { get; set; }

        public int IdVenda { get; set; }

        public int IdProduto { get; set; }

        public int Quantidade { get; set; }

        public decimal PrecoUnitario { get; set; }

        public decimal Subtotal { get; set; }
    }
}