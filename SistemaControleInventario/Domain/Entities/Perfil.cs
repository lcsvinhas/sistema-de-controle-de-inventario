namespace SistemaControleInventario.Domain.Entities
{
    public class Perfil
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public ICollection<UsuarioPerfil> UsuarioPerfis { get; set; }

        public Perfil() { }

        public Perfil(string nome)
        {
            Nome = nome;
        }
    }
}
