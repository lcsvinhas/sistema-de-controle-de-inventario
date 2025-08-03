using Dapper;
using Npgsql;
using SistemaControleInventario.Domain.Entities;
using SistemaControleInventario.Domain.Repositories;

public class NotificacaoRepository : INotificacaoRepository
{
    private readonly string _connectionString;

    public NotificacaoRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    private NpgsqlConnection GetConnection() => new NpgsqlConnection(_connectionString);

    public async Task Save(Notificacao notificacao)
    {
        using var con = GetConnection();
        var sql = @"insert into notificacoes (mensagem, data_criacao, lida)
                    values (@Mensagem, @DataCriacao, @Lida)";
        await con.ExecuteAsync(sql, notificacao);
    }

    public async Task<IEnumerable<Notificacao>> FindAll()
    {
        using var con = GetConnection();
        var sql = "SELECT id, mensagem, data_criacao AS DataCriacao, lida FROM notificacoes order by data_criacao desc";
        return await con.QueryAsync<Notificacao>(sql);
    }

    public async Task<Notificacao> FindById(int id)
    {
        using var con = GetConnection();
        var sql = "select * from notificacoes where id = @Id";
        return await con.QueryFirstOrDefaultAsync<Notificacao>(sql, new { Id = id });
    }

    public async Task MarkAsRead(int id)
    {
        using var con = GetConnection();
        var sql = "update notificacoes set lida = true where id = @Id";
        await con.ExecuteAsync(sql, new { Id = id });
    }
}
