using SistemaControleInventario.Domain.Entities;

namespace SistemaControleInventario.Infrastructure.Messaging.Producers
{
    public interface IEstoqueProducer
    {
        Task EnviarMensagemEstoqueBaixo(Produto produto);
        Task EnviarMensagemProdutoAtualizado(Produto produto);

    }
}
