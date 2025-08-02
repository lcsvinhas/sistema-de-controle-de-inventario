using SistemaControleInventario.Application.Exceptions;

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
            if (nome == null || nome == "")
                throw new ProdutoException("Nome do produto é obrigatório.");

            if (estoque < 0)
                throw new ProdutoException("Estoque deve ser maior ou igual a zero.");

            if (estoqueMinimo < 0)
                throw new ProdutoException("Estoque mínimo deve ser maior ou igual a zero.");

            Ativo = true;
            Nome = nome;
            Descricao = descricao;
            Estoque = estoque;
            EstoqueMinimo = estoqueMinimo;
        }
    }
}
