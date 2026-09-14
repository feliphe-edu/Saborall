using MySql.Data.MySqlClient;

namespace Saborall.Configs
{
    public class Conexao
    {
        private readonly string _connectionString;

        public Conexao(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Saborall")
                ?? throw new Exception("String de conexão não encontrada.");
        }

        public MySqlConnection CriarConexao()
        {
            return new MySqlConnection(_connectionString);
        }
    }
}