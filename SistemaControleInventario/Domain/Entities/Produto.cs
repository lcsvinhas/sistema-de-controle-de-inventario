namespace SistemaControleInventario.Domain.Entities
{
    public class Produto
    {
        public int Id { get; set; }
        public bool Ativo { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public int Estoque { get; set; }
        public int EstoqueMinimo { get; set; }

        public Produto() { }

        public Produto(string nome, string descricao, int estoque, int estoqueMinimo)
        {
            Ativo = true;
            Nome = nome;
            Descricao = descricao;
            Estoque = estoque;
            EstoqueMinimo = estoqueMinimo;
        }
    }
}
