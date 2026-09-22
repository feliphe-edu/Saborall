using MySql.Data.MySqlClient;
using Saborall.Configs;
using Saborall.Models;

namespace Saborall.DAO
{
    public class VendaDAO
    {
        private readonly Conexao _conexao;

        public VendaDAO(Conexao conexao)
        {
            _conexao = conexao;
        }

        // =========================================================
        // LISTAR VENDAS
        // =========================================================
        public List<Venda> Listar()
        {
            var lista = new List<Venda>();

            using var conexao = _conexao.CriarConexao();
            conexao.Open();

            string sql = """
                SELECT *
                FROM Vendas
                ORDER BY data_venda DESC
                """;

            using var comando = new MySqlCommand(sql, conexao);
            using var reader = comando.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(new Venda
                {
                    IdVenda = reader.GetInt32("id_venda"),
                    DataVenda = reader.GetDateTime("data_venda"),
                    IdVendedor = reader.GetInt32("id_vendedor"),
                    ValorTotal = reader.GetDecimal("valor_total"),
                    FormaPagamento = reader.GetString("forma_pagamento")
                });
            }

            return lista;
        }

        // =========================================================
        // INSERIR VENDA
        // =========================================================
        public int InserirVenda(Venda venda)
        {
            using var conexao = _conexao.CriarConexao();
            conexao.Open();

            string sql = """
                INSERT INTO Vendas
                (
                    data_venda,
                    id_vendedor,
                    quantidade,
                    valor_total,
                    forma_pagamento
                )
                VALUES
                (
                    @data,
                    @vendedor,
                    @quantidade,
                    @total,
                    @formaPagamento
                );

                SELECT LAST_INSERT_ID();
                """;

            using var comando = new MySqlCommand(sql, conexao);

            comando.Parameters.AddWithValue("@data", venda.DataVenda);
            comando.Parameters.AddWithValue("@vendedor", venda.IdVendedor);
            comando.Parameters.AddWithValue("@quantidade", venda.Quantidade);
            comando.Parameters.AddWithValue("@total", venda.ValorTotal);
            comando.Parameters.AddWithValue("@formaPagamento", venda.FormaPagamento);

            return Convert.ToInt32(comando.ExecuteScalar());
        }

        // =========================================================
        // ATUALIZAR VENDA
        // =========================================================
        public void Atualizar(Venda venda)
        {
            using var conexao = _conexao.CriarConexao();
            conexao.Open();

            string sql = """
                UPDATE Vendas
                SET
                    data_venda = @data,
                    id_vendedor = @vendedor,
                    valor_total = @total,
                    forma_pagamento = @formaPagamento
                WHERE id_venda = @id
                """;

            using var comando = new MySqlCommand(sql, conexao);

            comando.Parameters.AddWithValue("@data", venda.DataVenda);
            comando.Parameters.AddWithValue("@vendedor", venda.IdVendedor);
            comando.Parameters.AddWithValue("@total", venda.ValorTotal);
            comando.Parameters.AddWithValue("@formaPagamento", venda.FormaPagamento);
            comando.Parameters.AddWithValue("@id", venda.IdVenda);

            comando.ExecuteNonQuery();
        }

        // =========================================================
        // EXCLUIR VENDA
        // =========================================================
        public void Excluir(int id)
        {
            using var conexao = _conexao.CriarConexao();
            conexao.Open();

            string sql = """
                DELETE FROM Vendas
                WHERE id_venda = @id
                """;

            using var comando = new MySqlCommand(sql, conexao);

            comando.Parameters.AddWithValue("@id", id);

            comando.ExecuteNonQuery();
        }

        // =========================================================
        // LISTAR ITENS DE UMA VENDA
        // =========================================================
        public List<ItemVenda> ListarItens(int idVenda)
        {
            var lista = new List<ItemVenda>();

            using var conexao = _conexao.CriarConexao();
            conexao.Open();

            string sql = """
                SELECT *
                FROM ItensVenda
                WHERE id_venda = @idVenda
                ORDER BY id_item_venda
                """;

            using var comando = new MySqlCommand(sql, conexao);

            comando.Parameters.AddWithValue("@idVenda", idVenda);

            using var reader = comando.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(new ItemVenda
                {
                    IdItemVenda = reader.GetInt32("id_item_venda"),
                    IdVenda = reader.GetInt32("id_venda"),
                    IdProduto = reader.GetInt32("id_produto"),
                    Quantidade = reader.GetInt32("quantidade"),
                    PrecoUnitario = reader.GetDecimal("preco_unitario"),
                    Subtotal = reader.GetDecimal("subtotal")
                });
            }

            return lista;
        }

        // =========================================================
        // INSERIR ITEM DA VENDA
        // =========================================================
        public void InserirItem(ItemVenda item)
        {
            using var conexao = _conexao.CriarConexao();
            conexao.Open();

            string sql = """
                INSERT INTO ItensVenda
                (
                    id_venda,
                    id_produto,
                    quantidade,
                    preco_unitario,
                    subtotal
                )
                VALUES
                (
                    @venda,
                    @produto,
                    @quantidade,
                    @preco,
                    @subtotal
                )
                """;

            using var comando = new MySqlCommand(sql, conexao);

            comando.Parameters.AddWithValue("@venda", item.IdVenda);
            comando.Parameters.AddWithValue("@produto", item.IdProduto);
            comando.Parameters.AddWithValue("@quantidade", item.Quantidade);
            comando.Parameters.AddWithValue("@preco", item.PrecoUnitario);
            comando.Parameters.AddWithValue("@subtotal", item.Subtotal);

            comando.ExecuteNonQuery();
        }

        // =========================================================
        // EXCLUIR ITEM
        // =========================================================
        public void ExcluirItem(int idItem)
        {
            using var conexao = _conexao.CriarConexao();
            conexao.Open();

            string sql = """
                DELETE FROM ItensVenda
                WHERE id_item_venda = @id
                """;

            using var comando = new MySqlCommand(sql, conexao);

            comando.Parameters.AddWithValue("@id", idItem);

            comando.ExecuteNonQuery();
        }
    }
}