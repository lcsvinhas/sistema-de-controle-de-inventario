using SistemaControleInventario.Domain.Entities;

namespace SistemaControleInventario.Domain.Repositories
{
    public interface IUsuarioRepository
    {
        Task<Usuario> FindById(int id);
        Task<IEnumerable<Usuario>> FindAll();
        Task Save(Usuario produto);
        Task Update(Usuario produto);
        Task Delete(int id);
        Task<Usuario> BuscarPorEmail(string email);
        Task AddPerfisAoUsuario(int usuarioId, List<int> perfilIds);
    }
}
