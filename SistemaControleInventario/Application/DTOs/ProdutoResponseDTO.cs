namespace SistemaControleInventario.Application.DTOs
{
    public record ProdutoResponseDTO(int Id, string Nome, string Descricao, int Estoque, int EstoqueMinimo);
}
