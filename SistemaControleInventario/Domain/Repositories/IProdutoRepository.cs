using SistemaControleInventario.Domain.Entities;

namespace SistemaControleInventario.Domain.Repositories
{
    public interface IProdutoRepository
    {
        Task<Produto> FindById(int id);
        Task<IEnumerable<Produto>> FindAll();
        Task Save(Produto produto);
        Task Update(Produto produto);
        Task Delete(int id);
    }
}
