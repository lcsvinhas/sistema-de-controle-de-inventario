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

        [Fact(DisplayName = "Teste 2: Obter todas as entidades cadastradas")]
        public async Task ListarProdutos()
        {
            var produtos = new List<Produto>
            {
                new Produto("Mouse", "Mouse ergonômico sem fio com sensor de alta precisão", 20, 5),
                new Produto("Teclado", "Teclado mecânico", 15, 3)
            };

            _mock.Setup(p => p.FindAll()).ReturnsAsync(produtos);

            var resultado = await _service.ListarProdutos();

            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count);

            var lista = resultado.ToList();
            Assert.Equal("Mouse", lista[0].Nome);
            Assert.Equal("Teclado", lista[1].Nome);
        }

        [Fact(DisplayName = "Teste 3: Obter entidade por id")]
        public async Task ListarPorId()
        {
            var produto = new Produto("Mouse", "Mouse ergonômico sem fio com sensor de alta precisão", 20, 5);
            typeof(Produto).GetProperty("Id")!.SetValue(produto, 1);

            _mock.Setup(p => p.FindById(1)).ReturnsAsync(produto);

            var resultado = await _service.ListarPorId(1);

            Assert.NotNull(resultado);
            Assert.Equal(1, resultado.Id);
            Assert.Equal("Mouse", resultado.Nome);
        }
    }
}
