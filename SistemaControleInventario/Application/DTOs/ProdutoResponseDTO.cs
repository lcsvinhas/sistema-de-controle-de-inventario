namespace SistemaControleInventario.Application.DTOs
{
    public record ProdutoResponseDTO(int Id, bool Ativo, string Nome, string Descricao, int Estoque, int EstoqueMinimo);
}
