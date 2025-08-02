using Dapper;
using Npgsql;
using SistemaControleInventario.Domain.Entities;
using SistemaControleInventario.Domain.Repositories;

namespace SistemaControleInventario.Infrastructure.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly string _connectionString;

        public ProdutoRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private NpgsqlConnection GetConnection() => new NpgsqlConnection(_connectionString);

        public async Task<Produto> FindById(int id)
        {
            using var con = GetConnection();
            const string sql = "select * from produtos where id = @Id and ativo = true";
            return await con.QueryFirstOrDefaultAsync<Produto>(sql, new { Id = id });
        }

        public async Task<IEnumerable<Produto>> FindAll()
        {
            using var con = GetConnection();
            const string sql = "select * from produtos where ativo = true";
            return await con.QueryAsync<Produto>(sql);
        }

        public async Task Save(Produto produto)
        {
            using var con = GetConnection();
            const string sql = @"
                insert into produtos (ativo, nome, descricao, estoque, estoque_minimo)
                values (@Ativo, @Nome, @Descricao, @Estoque, @EstoqueMinimo)
                returning id
                ";
            var id = await con.ExecuteScalarAsync<int>(sql, produto);
            typeof(Produto).GetProperty("Id")!.SetValue(produto, id);
        }

        public async Task Update(Produto produto)
        {
            using var con = GetConnection();
            const string sql = @"
            update produtos
            set ativo = @ativo,
                nome = @Nome,
                descricao = @Descricao,
                estoque = @Estoque,
                estoque_minimo = @EstoqueMinimo,
            where id = @Id
            ";
            await con.ExecuteAsync(sql, produto);
        }

        public async Task Delete(int id)
        {
            using var con = GetConnection();
            const string sql = "update produtos set ativo = false where id = @Id";
            await con.ExecuteAsync(sql, new { Id = id });
        }
    }
}
