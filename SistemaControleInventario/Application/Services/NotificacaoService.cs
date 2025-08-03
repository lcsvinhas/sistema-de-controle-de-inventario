using SistemaControleInventario.Application.Exceptions;
using SistemaControleInventario.Domain.Entities;
using SistemaControleInventario.Domain.Repositories;

namespace SistemaControleInventario.Application.Services
{
    public class NotificacaoService
    {
        private readonly INotificacaoRepository _repository;

        public NotificacaoService(INotificacaoRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Notificacao>> ListarNotificacoes()
        {
            var notificacoes = await _repository.FindAll();

            return notificacoes;
        }

        public async Task MarcarComoLida(int id)
        {
            var notificacao = await _repository.FindById(id);

            if (notificacao == null)
            {
                throw new NotificacaoException("Notificação inexistente.");
            }

            await _repository.MarkAsRead(id);

        }
    }
}
