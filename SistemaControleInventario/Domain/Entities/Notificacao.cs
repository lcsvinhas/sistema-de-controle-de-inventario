namespace SistemaControleInventario.Domain.Entities
{
    public class Notificacao
    {
        public int Id { get; set; }
        public string Mensagem { get; set; }
        public DateTime DataCriacao { get; set; }
        public bool Lida { get; set; }

        public Notificacao(string mensagem)
        { 
            Mensagem = mensagem;
            DataCriacao = DateTime.Now;
            Lida = false;
        }
    }
}
