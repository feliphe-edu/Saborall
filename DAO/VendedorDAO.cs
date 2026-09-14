using MySql.Data.MySqlClient;
using Saborall.Configs;
using Saborall.Models;

namespace Saborall.DAO
{
    public class VendedorDAO
    {
        private readonly Conexao _conexao;

        public VendedorDAO(Conexao conexao)
        {
            _conexao = conexao;
        }

        // LISTAR
        public List<Vendedor> Listar()
        {
            var lista = new List<Vendedor>();

            using var conexao = _conexao.CriarConexao();
            conexao.Open();

            string sql = "SELECT * FROM Vendedores ORDER BY nome";

            using var comando = new MySqlCommand(sql, conexao);
            using var reader = comando.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(new Vendedor
                {
                    IdVendedor = reader.GetInt32("id_vendedor"),
                    Nome = reader.GetString("nome")
                });
            }

            return lista;
        }

        // INSERIR
        public void Inserir(Vendedor vendedor)
        {
            using var conexao = _conexao.CriarConexao();
            conexao.Open();

            string sql = """
                INSERT INTO Vendedores (nome)
                VALUES (@nome)
                """;

            using var comando = new MySqlCommand(sql, conexao);
            comando.Parameters.AddWithValue("@nome", vendedor.Nome);

            comando.ExecuteNonQuery();
        }

        // ATUALIZAR
        public void Atualizar(Vendedor vendedor)
        {
            using var conexao = _conexao.CriarConexao();
            conexao.Open();

            string sql = """
                UPDATE Vendedores
                SET nome = @nome
                WHERE id_vendedor = @id
                """;

            using var comando = new MySqlCommand(sql, conexao);

            comando.Parameters.AddWithValue("@nome", vendedor.Nome);
            comando.Parameters.AddWithValue("@id", vendedor.IdVendedor);

            comando.ExecuteNonQuery();
        }

        // EXCLUIR
        public void Excluir(int id)
        {
            using var conexao = _conexao.CriarConexao();
            conexao.Open();

            string sql = "DELETE FROM Vendedores WHERE id_vendedor = @id";

            using var comando = new MySqlCommand(sql, conexao);
            comando.Parameters.AddWithValue("@id", id);

            comando.ExecuteNonQuery();
        }
    }
}