namespace SistemaControleInventario.Domain.Entities
{
    public class Usuario
    {
        public int Id { get; set; }
        public bool Ativo { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public ICollection<UsuarioPerfil> UsuarioPerfis { get; set; }
        public Usuario() { }

        public Usuario(string nome, string email, string senha)
        {
            Ativo = true;
            Nome = nome;
            Email = email;
            Senha = senha;
        }
    }
}
