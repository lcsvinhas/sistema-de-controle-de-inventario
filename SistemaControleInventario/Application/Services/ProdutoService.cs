using SistemaControleInventario.Application.DTOs;
using SistemaControleInventario.Application.Exceptions;
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

            return new ProdutoResponseDTO(produto.Id, produto.Nome, produto.Descricao, produto.Estoque, produto.EstoqueMinimo);
        }

        public async Task<ICollection<ProdutoResponseDTO>> ListarProdutos()
        {
            var produtos = await _produtoRepository.FindAll();

            return produtos.Select(p => new ProdutoResponseDTO(p.Id, p.Nome, p.Descricao, p.Estoque, p.EstoqueMinimo)).ToList();
        }

        public async Task<ProdutoResponseDTO> ListarPorId(int id)
        {
            var produto = await _produtoRepository.FindById(id);

            if (produto == null)
            {
                throw new ProdutoException("Produto inexistente.");
            }

            return new ProdutoResponseDTO(produto.Id, produto.Nome, produto.Descricao, produto.Estoque, produto.EstoqueMinimo);
        }
    }
}
