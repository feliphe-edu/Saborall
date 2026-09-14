using MySql.Data.MySqlClient;
using Saborall.Configs;
using Saborall.Models;

namespace Saborall.DAO
{
    public class ProdutoDAO
    {
        private readonly Conexao _conexao;

        public ProdutoDAO(Conexao conexao)
        {
            _conexao = conexao;
        }

        // LISTAR
        public List<Produto> Listar()
        {
            var lista = new List<Produto>();

            using var conexao = _conexao.CriarConexao();
            conexao.Open();

            string sql = """
                SELECT *
                FROM Produtos
                ORDER BY nome_produto
                """;

            using var comando = new MySqlCommand(sql, conexao);
            using var reader = comando.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(new Produto
                {
                    IdProduto = reader.GetInt32("id_produto"),
                    NomeProduto = reader.GetString("nome_produto"),

                    Categoria = reader.IsDBNull(reader.GetOrdinal("categoria"))
                        ? ""
                        : reader.GetString("categoria"),

                    Preco = reader.GetDecimal("preco"),

                    IdFornecedor = reader.IsDBNull(reader.GetOrdinal("id_fornecedor"))
                        ? null
                        : reader.GetInt32("id_fornecedor")
                });
            }

            return lista;
        }

        // INSERIR
        public void Inserir(Produto produto)
        {
            using var conexao = _conexao.CriarConexao();
            conexao.Open();

            string sql = """
                INSERT INTO Produtos
                (nome_produto, categoria, preco, id_fornecedor)
                VALUES
                (@nome, @categoria, @preco, @fornecedor)
                """;

            using var comando = new MySqlCommand(sql, conexao);

            comando.Parameters.AddWithValue("@nome", produto.NomeProduto);
            comando.Parameters.AddWithValue("@categoria", produto.Categoria);
            comando.Parameters.AddWithValue("@preco", produto.Preco);

            if (produto.IdFornecedor.HasValue)
                comando.Parameters.AddWithValue("@fornecedor", produto.IdFornecedor.Value);
            else
                comando.Parameters.AddWithValue("@fornecedor", DBNull.Value);

            comando.ExecuteNonQuery();
        }

        // ATUALIZAR
        public void Atualizar(Produto produto)
        {
            using var conexao = _conexao.CriarConexao();
            conexao.Open();

            string sql = """
                UPDATE Produtos
                SET nome_produto = @nome,
                    categoria = @categoria,
                    preco = @preco,
                    id_fornecedor = @fornecedor
                WHERE id_produto = @id
                """;

            using var comando = new MySqlCommand(sql, conexao);

            comando.Parameters.AddWithValue("@nome", produto.NomeProduto);
            comando.Parameters.AddWithValue("@categoria", produto.Categoria);
            comando.Parameters.AddWithValue("@preco", produto.Preco);

            if (produto.IdFornecedor.HasValue)
                comando.Parameters.AddWithValue("@fornecedor", produto.IdFornecedor.Value);
            else
                comando.Parameters.AddWithValue("@fornecedor", DBNull.Value);

            comando.Parameters.AddWithValue("@id", produto.IdProduto);

            comando.ExecuteNonQuery();
        }

        // EXCLUIR
        public void Excluir(int id)
        {
            using var conexao = _conexao.CriarConexao();
            conexao.Open();

            string sql = """
                DELETE FROM Produtos
                WHERE id_produto = @id
                """;

            using var comando = new MySqlCommand(sql, conexao);
            comando.Parameters.AddWithValue("@id", id);

            comando.ExecuteNonQuery();
        }
    }
}