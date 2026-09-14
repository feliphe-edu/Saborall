using MySql.Data.MySqlClient;
using Saborall.Configs;
using Saborall.Models;

namespace Saborall.DAO
{
    public class FornecedorDAO
    {
        private readonly Conexao _conexao;

        public FornecedorDAO(Conexao conexao)
        {
            _conexao = conexao;
        }

        // LISTAR
        public List<Fornecedor> Listar()
        {
            var lista = new List<Fornecedor>();

            using var conexao = _conexao.CriarConexao();
            conexao.Open();

            string sql = "SELECT * FROM Fornecedores ORDER BY nome_fantasia";

            using var comando = new MySqlCommand(sql, conexao);
            using var reader = comando.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(new Fornecedor
                {
                    IdFornecedor = reader.GetInt32("id_fornecedor"),
                    NomeEmpresa = reader.GetString("nome_empresa"),
                    NomeFantasia = reader.IsDBNull(reader.GetOrdinal("nome_fantasia"))
                        ? ""
                        : reader.GetString("nome_fantasia"),
                    Cnpj = reader.GetString("cnpj"),
                    Endereco = reader.IsDBNull(reader.GetOrdinal("endereco"))
                        ? ""
                        : reader.GetString("endereco"),
                    Email = reader.IsDBNull(reader.GetOrdinal("email"))
                        ? ""
                        : reader.GetString("email"),
                    Telefone = reader.IsDBNull(reader.GetOrdinal("telefone"))
                        ? ""
                        : reader.GetString("telefone")
                });
            }

            return lista;
        }

        // INSERIR
        public void Inserir(Fornecedor fornecedor)
        {
            using var conexao = _conexao.CriarConexao();
            conexao.Open();

            string sql = """
                INSERT INTO Fornecedores
                (nome_empresa, nome_fantasia, cnpj, endereco, email, telefone)
                VALUES
                (@nomeEmpresa, @nomeFantasia, @cnpj, @endereco, @email, @telefone)
                """;

            using var comando = new MySqlCommand(sql, conexao);

            comando.Parameters.AddWithValue("@nomeEmpresa", fornecedor.NomeEmpresa);
            comando.Parameters.AddWithValue("@nomeFantasia", fornecedor.NomeFantasia);
            comando.Parameters.AddWithValue("@cnpj", fornecedor.Cnpj);
            comando.Parameters.AddWithValue("@endereco", fornecedor.Endereco);
            comando.Parameters.AddWithValue("@email", fornecedor.Email);
            comando.Parameters.AddWithValue("@telefone", fornecedor.Telefone);

            comando.ExecuteNonQuery();
        }

        // ATUALIZAR
        public void Atualizar(Fornecedor fornecedor)
        {
            using var conexao = _conexao.CriarConexao();
            conexao.Open();

            string sql = """
                UPDATE Fornecedores
                SET nome_empresa = @nomeEmpresa,
                    nome_fantasia = @nomeFantasia,
                    cnpj = @cnpj,
                    endereco = @endereco,
                    email = @email,
                    telefone = @telefone
                WHERE id_fornecedor = @id
                """;

            using var comando = new MySqlCommand(sql, conexao);

            comando.Parameters.AddWithValue("@nomeEmpresa", fornecedor.NomeEmpresa);
            comando.Parameters.AddWithValue("@nomeFantasia", fornecedor.NomeFantasia);
            comando.Parameters.AddWithValue("@cnpj", fornecedor.Cnpj);
            comando.Parameters.AddWithValue("@endereco", fornecedor.Endereco);
            comando.Parameters.AddWithValue("@email", fornecedor.Email);
            comando.Parameters.AddWithValue("@telefone", fornecedor.Telefone);
            comando.Parameters.AddWithValue("@id", fornecedor.IdFornecedor);

            comando.ExecuteNonQuery();
        }

        // EXCLUIR
        public void Excluir(int id)
        {
            using var conexao = _conexao.CriarConexao();
            conexao.Open();

            string sql = """
                DELETE FROM Fornecedores
                WHERE id_fornecedor = @id
                """;

            using var comando = new MySqlCommand(sql, conexao);
            comando.Parameters.AddWithValue("@id", id);

            comando.ExecuteNonQuery();
        }
    }
}