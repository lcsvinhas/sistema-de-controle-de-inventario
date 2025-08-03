using SistemaControleInventario.Application.DTOs;
using SistemaControleInventario.Application.Exceptions;
using SistemaControleInventario.Domain.Entities;
using SistemaControleInventario.Domain.Repositories;
using SistemaControleInventario.Infrastructure.Messaging.Producers;

namespace SistemaControleInventario.Application.Services
{
    public class ProdutoService
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IEstoqueProducer _estoqueProducer;

        public ProdutoService(IProdutoRepository produtoRepository, IEstoqueProducer estoqueProducer)
        {
            _produtoRepository = produtoRepository;
            _estoqueProducer = estoqueProducer;
        }

        public async Task<ProdutoResponseDTO> AdicionarProduto(ProdutoRequestDTO dto)
        {
            var produto = new Produto(dto.Nome, dto.Descricao, dto.Estoque, dto.EstoqueMinimo);

            await _produtoRepository.Save(produto);

            if (produto.Estoque < produto.EstoqueMinimo)
            {
                await _estoqueProducer.EnviarMensagemEstoqueBaixo(produto);
            }

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

        public async Task<ProdutoResponseDTO> AtualizarProduto(int id, ProdutoRequestDTO dto)
        {
            var produto = await _produtoRepository.FindById(id);

            if (produto == null)
            {
                throw new ProdutoException("Produto inexistente.");
            }

            produto.Nome = dto.Nome;
            produto.Descricao = dto.Descricao;
            produto.Estoque = dto.Estoque;
            produto.EstoqueMinimo = dto.EstoqueMinimo;
            produto.Ativo = true;
            produto.Atualizar(dto.Nome, dto.Descricao, dto.Estoque, dto.EstoqueMinimo);

            await _produtoRepository.Update(produto);

            if (produto.Estoque < produto.EstoqueMinimo)
            {
                await _estoqueProducer.EnviarMensagemEstoqueBaixo(produto);
            }
            else
            {
                await _estoqueProducer.EnviarMensagemProdutoAtualizado(produto);
            }

            return new ProdutoResponseDTO(produto.Id, produto.Nome, produto.Descricao, produto.Estoque, produto.EstoqueMinimo);
        }

        public async Task DeletarProduto(int id)
        {
            var produto = await _produtoRepository.FindById(id);

            if (produto == null)
            {
                throw new ProdutoException("Produto inexistente.");
            }

            produto.Ativo = false;

            await _produtoRepository.Update(produto);
        }
    }
}
