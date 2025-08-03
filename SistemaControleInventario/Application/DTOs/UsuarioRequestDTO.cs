namespace SistemaControleInventario.Application.DTOs
{
    public class UsuarioRequestDTO
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public List<int> Perfis { get; set; } = new();
    }
}
