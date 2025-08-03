using SistemaControleInventario.Domain.Entities;

namespace SistemaControleInventario.Domain.Repositories
{
    public interface INotificacaoRepository
    {
        Task Save(Notificacao notificacao);
        Task<IEnumerable<Notificacao>> FindAll();
        Task<Notificacao> FindById(int id);
        Task MarkAsRead(int id);
    }
}
