using Moq;
using SistemaControleInventario.Domain.Repositories;
using SistemaControleInventario.Domain.Entities;
using SistemaControleInventario.Application.Services;
using SistemaControleInventario.Application.DTOs;


namespace SistemaControleInventario.Tests
{
    public class ProdutoServiceTests
    {
        private readonly Mock<IProdutoRepository> _mock;
        private readonly ProdutoService _service;

        public ProdutoServiceTests()
        {
            _mock = new Mock<IProdutoRepository>();
            _service = new ProdutoService(_mock.Object);
        }

        [Fact(DisplayName = "Teste 1: Adicionar uma nova entidade")]
        public async Task AdicionarProduto()
        {
            var dto = new ProdutoRequestDTO();

            dto.Nome = "Mouse";
            dto.Descricao = "Mouse ergonômico sem fio com sensor de alta precisão";
            dto.Estoque = 20;
            dto.EstoqueMinimo = 5;

            var produto = new Produto(dto.Nome, dto.Descricao, dto.Estoque, dto.EstoqueMinimo);

            _mock.Setup(p => p.Save(It.IsAny<Produto>())).Returns(Task.CompletedTask);

            var result = await _service.AdicionarProduto(dto);

            Assert.Equal(dto.Nome, result.Nome);
            Assert.Equal(dto.Descricao, result.Descricao);
            Assert.Equal(dto.Estoque, result.Estoque);
            Assert.Equal(dto.EstoqueMinimo, result.EstoqueMinimo);
        }
    }
}
