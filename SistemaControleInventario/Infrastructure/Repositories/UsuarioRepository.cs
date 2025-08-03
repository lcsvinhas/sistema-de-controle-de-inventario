using Dapper;
using Npgsql;
using SistemaControleInventario.Domain.Entities;
using SistemaControleInventario.Domain.Repositories;

namespace SistemaControleInventario.Infrastructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly string _connectionString;

        public UsuarioRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private NpgsqlConnection GetConnection() => new NpgsqlConnection(_connectionString);

        public async Task<Usuario> FindById(int id)
        {
            using var con = GetConnection();
            const string sql = "select * from usuarios where id = @Id and ativo = true";
            return await con.QueryFirstOrDefaultAsync<Usuario>(sql, new { Id = id });
        }

        public async Task<IEnumerable<Usuario>> FindAll()
        {
            using var con = GetConnection();
            const string sql = "select * from usuarios where ativo = true";
            return await con.QueryAsync<Usuario>(sql);
        }

        public async Task Save(Usuario Usuario)
        {
            using var con = GetConnection();
            const string sql = @"
                insert into usuarios (ativo, nome, email, senha)
                values (@Ativo, @Nome, @Email, @Senha)
                returning id
                ";
            var id = await con.ExecuteScalarAsync<int>(sql, Usuario);
            typeof(Usuario).GetProperty("Id")!.SetValue(Usuario, id);
        }

        public async Task Update(Usuario Usuario)
        {
            using var con = GetConnection();
            const string sql = @"
            update usuarios
            set ativo = @ativo,
                nome = @Nome,
                email = @Email,
                senha = @Senha,
            where id = @Id
            ";
            await con.ExecuteAsync(sql, Usuario);
        }

        public async Task Delete(int id)
        {
            using var con = GetConnection();
            const string sql = "update usuarios set ativo = false where id = @Id";
            await con.ExecuteAsync(sql, new { Id = id });
        }

        public async Task<Usuario> BuscarPorEmail(string email)
        {
            using var con = GetConnection();
            const string sql = @"select * from usuarios where email = @Email and ativo = true";
            return await con.QueryFirstOrDefaultAsync<Usuario>(sql, new { Email = email });
        }

        public async Task AddPerfisAoUsuario(int usuarioId, List<int> perfilIds)
        {
            using var con = GetConnection();
            const string sql = "insert into usuarioperfis (usuario_id, perfil_id) values (@UsuarioId, @PerfilId)";

            foreach (var perfilId in perfilIds)
            {
                await con.ExecuteAsync(sql, new { UsuarioId = usuarioId, PerfilId = perfilId });
            }
        }
    }
}
