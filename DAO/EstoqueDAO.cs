using MySql.Data.MySqlClient;
using Saborall.Configs;
using Saborall.Models;

namespace Saborall.DAO
{
    public class EstoqueDAO
    {
        private readonly Conexao _conexao;

        public EstoqueDAO(Conexao conexao)
        {
            _conexao = conexao;
        }

        // LISTAR
        public List<Estoque> Listar()
        {
            var lista = new List<Estoque>();

            using var conexao = _conexao.CriarConexao();
            conexao.Open();

            string sql = """
                SELECT *
                FROM Estoque
                ORDER BY id_produto
                """;

            using var comando = new MySqlCommand(sql, conexao);
            using var reader = comando.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(new Estoque
                {
                    IdEstoque = reader.GetInt32("id_estoque"),
                    IdProduto = reader.GetInt32("id_produto"),
                    QuantidadeDisponivel = reader.GetInt32("quantidade_disponivel")
                });
            }

            return lista;
        }


        // BUSCAR PELO PRODUTO
        public Estoque? BuscarPorProduto(int idProduto)
        {
            using var conexao = _conexao.CriarConexao();
            conexao.Open();

            string sql = """
                SELECT *
                FROM Estoque
                WHERE id_produto = @idProduto
                """;

            using var comando = new MySqlCommand(sql, conexao);
            comando.Parameters.AddWithValue("@idProduto", idProduto);

            using var reader = comando.ExecuteReader();

            if (reader.Read())
            {
                return new Estoque
                {
                    IdEstoque = reader.GetInt32("id_estoque"),
                    IdProduto = reader.GetInt32("id_produto"),
                    QuantidadeDisponivel = reader.GetInt32("quantidade_disponivel")
                };
            }

            return null;
        }


        // INSERIR
        public void Inserir(Estoque estoque)
        {
            using var conexao = _conexao.CriarConexao();
            conexao.Open();

            string sql = """
                INSERT INTO Estoque
                (id_produto, quantidade_disponivel)
                VALUES
                (@idProduto, @quantidade)
                """;

            using var comando = new MySqlCommand(sql, conexao);

            comando.Parameters.AddWithValue("@idProduto", estoque.IdProduto);
            comando.Parameters.AddWithValue("@quantidade", estoque.QuantidadeDisponivel);

            comando.ExecuteNonQuery();
        }


        // ATUALIZAR
        public void Atualizar(Estoque estoque)
        {
            using var conexao = _conexao.CriarConexao();
            conexao.Open();

            string sql = """
                UPDATE Estoque
                SET quantidade_disponivel = @quantidade
                WHERE id_produto = @idProduto
                """;

            using var comando = new MySqlCommand(sql, conexao);

            comando.Parameters.AddWithValue("@quantidade", estoque.QuantidadeDisponivel);
            comando.Parameters.AddWithValue("@idProduto", estoque.IdProduto);

            comando.ExecuteNonQuery();
        }


        // EXCLUIR
        public void Excluir(int id)
        {
            using var conexao = _conexao.CriarConexao();
            conexao.Open();

            string sql = """
                DELETE FROM Estoque
                WHERE id_estoque = @id
                """;

            using var comando = new MySqlCommand(sql, conexao);
            comando.Parameters.AddWithValue("@id", id);

            comando.ExecuteNonQuery();
        }


        // ALTERAR SOMENTE A QUANTIDADE
        public void AtualizarQuantidade(int idProduto, int quantidade)
        {
            using var conexao = _conexao.CriarConexao();
            conexao.Open();

            string sql = """
                UPDATE Estoque
                SET quantidade_disponivel = @quantidade
                WHERE id_produto = @idProduto
                """;

            using var comando = new MySqlCommand(sql, conexao);

            comando.Parameters.AddWithValue("@quantidade", quantidade);
            comando.Parameters.AddWithValue("@idProduto", idProduto);

            comando.ExecuteNonQuery();
        }


        // BAIXAR ESTOQUE AUTOMATICAMENTE APÓS UMA VENDA
        public bool BaixarEstoque(int idProduto, int quantidade)
        {
            using var conexao = _conexao.CriarConexao();
            conexao.Open();

            string sql = """
                UPDATE Estoque
                SET quantidade_disponivel = quantidade_disponivel - @quantidade
                WHERE id_produto = @idProduto
                  AND quantidade_disponivel >= @quantidade
                """;

            using var comando = new MySqlCommand(sql, conexao);

            comando.Parameters.AddWithValue("@quantidade", quantidade);
            comando.Parameters.AddWithValue("@idProduto", idProduto);

            int linhasAlteradas = comando.ExecuteNonQuery();

            return linhasAlteradas > 0;
        }
    }
}