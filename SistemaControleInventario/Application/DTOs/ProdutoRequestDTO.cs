using System.ComponentModel.DataAnnotations;

namespace SistemaControleInventario.Application.DTOs
{
    public class ProdutoRequestDTO
    {
        [Required(ErrorMessage = "Nome do produto é obrigatório.")]
        public string Nome { get; set; }

        public string Descricao { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Estoque deve ser maior ou igual a zero.")]
        public int Estoque { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Estoque mínimo deve ser maior ou igual a zero.")]
        public int EstoqueMinimo { get; set; }
    }
}
