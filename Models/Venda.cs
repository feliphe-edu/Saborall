namespace Saborall.Models
{
    public class Venda
    {
        public int IdVenda { get; set; }
        public DateTime DataVenda { get; set; }
        public int IdVendedor { get; set; }
        public decimal ValorTotal { get; set; }
        public string FormaPagamento { get; set; } = "Não informado";
    }
}