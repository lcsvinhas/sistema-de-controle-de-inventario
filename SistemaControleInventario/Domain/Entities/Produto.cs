namespace SistemaControleInventario.Domain.Entities
{
    public class Produto
    {
        public int Id { get; private set; }
        public bool Ativo { get; private set; }
        public string Nome { get; private set; }
        public string Descricao { get; private set; }
        public int Estoque { get; private set; }
        public int EstoqueMinimo { get; private set; }

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
