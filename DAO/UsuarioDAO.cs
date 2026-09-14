using MySql.Data.MySqlClient;
using Saborall.Configs;
using Saborall.Models;

namespace Saborall.DAO
{
    public class UsuarioDAO
    {
        private readonly Conexao _conexao;

        public UsuarioDAO(Conexao conexao)
        {
            _conexao = conexao;
        }

        // LISTAR TODOS
        public List<Usuario> Listar()
        {
            var lista = new List<Usuario>();

            using var conexao = _conexao.CriarConexao();
            conexao.Open();

            string sql = "SELECT * FROM Usuarios ORDER BY id_usuario";

            using var comando = new MySqlCommand(sql, conexao);
            using var reader = comando.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(new Usuario
                {
                    IdUsuario = reader.GetInt32("id_usuario"),
                    Email = reader.GetString("email"),
                    Senha = reader.GetString("senha")
                });
            }

            return lista;
        }

        // BUSCAR POR E-MAIL
        public Usuario? BuscarPorEmail(string email)
        {
            using var conexao = _conexao.CriarConexao();
            conexao.Open();

            string sql = """
                SELECT *
                FROM Usuarios
                WHERE email = @email
                """;

            using var comando = new MySqlCommand(sql, conexao);
            comando.Parameters.AddWithValue("@email", email);

            using var reader = comando.ExecuteReader();

            if (reader.Read())
            {
                return new Usuario
                {
                    IdUsuario = reader.GetInt32("id_usuario"),
                    Email = reader.GetString("email"),
                    Senha = reader.GetString("senha")
                };
            }

            return null;
        }

        // INSERIR
        public void Inserir(Usuario usuario)
        {
            using var conexao = _conexao.CriarConexao();
            conexao.Open();

            string sql = """
                INSERT INTO Usuarios (email, senha)
                VALUES (@email, @senha)
                """;

            using var comando = new MySqlCommand(sql, conexao);

            comando.Parameters.AddWithValue("@email", usuario.Email);
            comando.Parameters.AddWithValue("@senha", usuario.Senha);

            comando.ExecuteNonQuery();
        }

        // ATUALIZAR
        public void Atualizar(Usuario usuario)
        {
            using var conexao = _conexao.CriarConexao();
            conexao.Open();

            string sql = """
                UPDATE Usuarios
                SET email = @email,
                    senha = @senha
                WHERE id_usuario = @id
                """;

            using var comando = new MySqlCommand(sql, conexao);

            comando.Parameters.AddWithValue("@email", usuario.Email);
            comando.Parameters.AddWithValue("@senha", usuario.Senha);
            comando.Parameters.AddWithValue("@id", usuario.IdUsuario);

            comando.ExecuteNonQuery();
        }

        // EXCLUIR
        public void Excluir(int id)
        {
            using var conexao = _conexao.CriarConexao();
            conexao.Open();

            string sql = "DELETE FROM Usuarios WHERE id_usuario = @id";

            using var comando = new MySqlCommand(sql, conexao);
            comando.Parameters.AddWithValue("@id", id);

            comando.ExecuteNonQuery();
        }
    }
}