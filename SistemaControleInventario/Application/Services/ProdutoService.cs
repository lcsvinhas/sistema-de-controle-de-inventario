using SistemaControleInventario.Application.DTOs;
using SistemaControleInventario.Domain.Entities;
using SistemaControleInventario.Domain.Repositories;

namespace SistemaControleInventario.Application.Services
{
    public class ProdutoService
    {
        private readonly IProdutoRepository _produtoRepository;

        public ProdutoService(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        public async Task<ProdutoResponseDTO> AdicionarProduto(ProdutoRequestDTO dto)
        {
            var produto = new Produto(dto.Nome, dto.Descricao, dto.Estoque, dto.EstoqueMinimo);

            await _produtoRepository.Save(produto);

            return new ProdutoResponseDTO(produto.Id, produto.Ativo, produto.Nome, produto.Descricao, produto.Estoque, produto.EstoqueMinimo);
        }
    }
}
